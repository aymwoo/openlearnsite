/**
 * Vitest 单元测试: 考试系统核心逻辑
 * 
 * 测试 exam.js / preview.js 中的纯逻辑函数：
 * - checkAnswer: 8种题型的答案判定
 * - scoreShortAnswer: 简答题关键词评分
 * - calculateEarnedScore: 各题型得分计算
 * - getQuestionTypeText: 题型文本映射
 * - createQuestionData: 题目数据结构创建
 */
import { describe, it, expect } from 'vitest';
import {
  getQuestionTypeText,
  checkAnswer,
  scoreShortAnswer,
  calculateEarnedScore,
  createQuestionData,
  parseExamAiSseData,
  buildExamSseUrl,
} from './exam-functions.js';

// ============================================================
// getQuestionTypeText
// ============================================================
describe('getQuestionTypeText - 题型文本映射', () => {
  it('返回所有8种题型的中文名称', () => {
    expect(getQuestionTypeText('single_choice')).toBe('单选题');
    expect(getQuestionTypeText('multiple_choice')).toBe('多选题');
    expect(getQuestionTypeText('fill_blank')).toBe('填空题');
    expect(getQuestionTypeText('true_false')).toBe('判断题');
    expect(getQuestionTypeText('matching')).toBe('连线题');
    expect(getQuestionTypeText('sort_question')).toBe('排序题');
    expect(getQuestionTypeText('table_question')).toBe('表格题');
    expect(getQuestionTypeText('short_answer')).toBe('简答题');
  });

  it('未知题型返回"未知题型"', () => {
    expect(getQuestionTypeText('unknown')).toBe('未知题型');
    expect(getQuestionTypeText('')).toBe('未知题型');
    expect(getQuestionTypeText(undefined)).toBe('未知题型');
  });
});

// ============================================================
// checkAnswer - 单选题
// ============================================================
describe('checkAnswer - 单选题', () => {
  const question = { type: 'single_choice', answer: 2, score: 5 };

  it('正确答案返回 true', () => {
    expect(checkAnswer(question, 2)).toBe(true);
  });

  it('错误答案返回 false', () => {
    expect(checkAnswer(question, 0)).toBe(false);
    expect(checkAnswer(question, 3)).toBe(false);
  });

  it('null/undefined 返回 false', () => {
    expect(checkAnswer(question, null)).toBe(false);
    expect(checkAnswer(question, undefined)).toBe(false);
  });
});

// ============================================================
// checkAnswer - 判断题
// ============================================================
describe('checkAnswer - 判断题', () => {
  it('答案为 true 时正确判定', () => {
    const q = { type: 'true_false', answer: true, score: 3 };
    expect(checkAnswer(q, true)).toBe(true);
    expect(checkAnswer(q, false)).toBe(false);
  });

  it('答案为 false 时正确判定', () => {
    const q = { type: 'true_false', answer: false, score: 3 };
    expect(checkAnswer(q, false)).toBe(true);
    expect(checkAnswer(q, true)).toBe(false);
  });
});

// ============================================================
// checkAnswer - 多选题
// ============================================================
describe('checkAnswer - 多选题', () => {
  const question = {
    type: 'multiple_choice',
    answer: [0, 2, 3],
    score: 10,
  };

  it('完全正确（顺序一致）返回 true', () => {
    expect(checkAnswer(question, [0, 2, 3])).toBe(true);
  });

  it('完全正确（顺序不同）返回 true', () => {
    expect(checkAnswer(question, [3, 0, 2])).toBe(true);
  });

  it('多选一个返回 false', () => {
    expect(checkAnswer(question, [0, 1, 2, 3])).toBe(false);
  });

  it('少选一个返回 false', () => {
    expect(checkAnswer(question, [0, 2])).toBe(false);
  });

  it('完全错误返回 false', () => {
    expect(checkAnswer(question, [1])).toBe(false);
  });

  it('空数组返回 false', () => {
    expect(checkAnswer(question, [])).toBe(false);
  });

  it('非数组返回 false', () => {
    expect(checkAnswer(question, 0)).toBe(false);
  });
});

// ============================================================
// checkAnswer - 填空题
// ============================================================
describe('checkAnswer - 填空题', () => {
  const question = {
    type: 'fill_blank',
    blanks: [
      { answer: 'TCP' },
      { answer: 'IP' },
    ],
    score: 10,
  };

  it('完全正确返回 true', () => {
    expect(checkAnswer(question, ['TCP', 'IP'])).toBe(true);
  });

  it('大小写不敏感', () => {
    expect(checkAnswer(question, ['tcp', 'ip'])).toBe(true);
    expect(checkAnswer(question, ['Tcp', 'Ip'])).toBe(true);
  });

  it('答案顺序无关（不考虑顺序匹配）', () => {
    expect(checkAnswer(question, ['IP', 'TCP'])).toBe(true);
  });

  it('前后空格自动去除', () => {
    expect(checkAnswer(question, [' TCP ', ' IP '])).toBe(true);
  });

  it('答案数量不足返回 false', () => {
    expect(checkAnswer(question, ['TCP'])).toBe(false);
  });

  it('有空白答案被过滤后数量不足返回 false', () => {
    expect(checkAnswer(question, ['TCP', ''])).toBe(false);
  });

  it('错误答案返回 false', () => {
    expect(checkAnswer(question, ['UDP', 'IP'])).toBe(false);
  });
});

// ============================================================
// checkAnswer - 连线题
// ============================================================
describe('checkAnswer - 连线题', () => {
  const question = {
    type: 'matching',
    answer: { 0: 1, 1: 0, 2: 2 },
    score: 10,
  };

  it('完全正确返回 true', () => {
    expect(checkAnswer(question, { '0': 1, '1': 0, '2': 2 })).toBe(true);
  });

  it('部分正确返回 false', () => {
    expect(checkAnswer(question, { '0': 1, '1': 0, '2': 0 })).toBe(false);
  });

  it('连线不完整返回 false', () => {
    expect(checkAnswer(question, { '0': 1 })).toBe(false);
  });

  it('空对象返回 false', () => {
    expect(checkAnswer(question, {})).toBe(false);
  });
});

// ============================================================
// checkAnswer - 排序题
// ============================================================
describe('checkAnswer - 排序题', () => {
  const question = {
    type: 'sort_question',
    answer: [2, 0, 3, 1],
    score: 8,
  };

  it('正确顺序返回 true', () => {
    expect(checkAnswer(question, [2, 0, 3, 1])).toBe(true);
  });

  it('错误顺序返回 false', () => {
    expect(checkAnswer(question, [0, 1, 2, 3])).toBe(false);
  });

  it('长度不同返回 false', () => {
    expect(checkAnswer(question, [2, 0, 3])).toBe(false);
  });
});

// ============================================================
// checkAnswer - 表格题
// ============================================================
describe('checkAnswer - 表格题', () => {
  const question = {
    type: 'table_question',
    answer: { '0,1': '北京', '1,1': '上海', '2,1': '广州' },
    score: 15,
  };

  it('全部正确返回 true', () => {
    expect(checkAnswer(question, { '0,1': '北京', '1,1': '上海', '2,1': '广州' })).toBe(true);
  });

  it('大小写不敏感', () => {
    const q = {
      type: 'table_question',
      answer: { '0,1': 'Hello', '1,1': 'World' },
      score: 10,
    };
    expect(checkAnswer(q, { '0,1': 'hello', '1,1': 'world' })).toBe(true);
  });

  it('部分正确返回 false', () => {
    expect(checkAnswer(question, { '0,1': '北京', '1,1': '上海', '2,1': '深圳' })).toBe(false);
  });

  it('空答案返回 false', () => {
    expect(checkAnswer(question, {})).toBe(false);
  });
});

// ============================================================
// checkAnswer - 简答题
// ============================================================
describe('checkAnswer - 简答题', () => {
  const question = { type: 'short_answer', score: 20 };

  it('有内容返回 true（简答只检查是否有答案）', () => {
    expect(checkAnswer(question, '这是我的回答')).toBe(true);
  });

  it('空字符串返回 falsy', () => {
    // 源码: userAnswer && userAnswer.toString().trim().length > 0
    // 空字符串 '' 是 falsy，但不是严格 false
    expect(checkAnswer(question, '')).toBeFalsy();
    expect(checkAnswer(question, '   ')).toBeFalsy();
  });

  it('null/undefined 返回 false', () => {
    expect(checkAnswer(question, null)).toBe(false);
    expect(checkAnswer(question, undefined)).toBe(false);
  });
});

// ============================================================
// scoreShortAnswer - 简答题评分
// ============================================================
describe('scoreShortAnswer - 简答题关键词评分', () => {
  const question = {
    type: 'short_answer',
    score: 10,
    keywords: ['计算机', '网络', '协议', '数据', '传输'],
    keywordThreshold: 60,
  };

  it('无关键词设置返回0分', () => {
    const q = { type: 'short_answer', score: 10, keywords: [] };
    const result = scoreShortAnswer('任何答案', q);
    expect(result.score).toBe(0);
    expect(result.feedback).toContain('未设置关键词');
  });

  it('匹配全部关键词 - 满分', () => {
    const result = scoreShortAnswer('计算机网络使用协议进行数据传输', question);
    expect(result.score).toBe(10);
    expect(result.matchPercentage).toBe(100);
    expect(result.matchedKeywords).toHaveLength(5);
  });

  it('匹配超过阈值(>=60%) - 满分', () => {
    // 3/5 = 60%，刚好达到阈值
    const result = scoreShortAnswer('计算机网络使用协议', question);
    expect(result.score).toBe(10);
    expect(result.matchPercentage).toBe(60);
  });

  it('匹配在30%-60%之间 - 70%分数', () => {
    // 2/5 = 40%，在 threshold*0.5 (30%) 到 threshold (60%) 之间
    const result = scoreShortAnswer('计算机网络很好', question);
    expect(result.score).toBe(7); // Math.round(10 * 0.7)
    expect(result.matchPercentage).toBe(40);
  });

  it('匹配少于30% - 30%分数', () => {
    // 1/5 = 20%，大于0但小于30%
    const result = scoreShortAnswer('计算机是好东西', question);
    expect(result.score).toBe(3); // Math.round(10 * 0.3)
    expect(result.matchPercentage).toBe(20);
  });

  it('无匹配关键词 - 0分', () => {
    const result = scoreShortAnswer('今天天气很好', question);
    expect(result.score).toBe(0);
    expect(result.matchPercentage).toBe(0);
  });

  it('大小写不敏感匹配', () => {
    const q = {
      type: 'short_answer',
      score: 10,
      keywords: ['HTML', 'CSS', 'JavaScript'],
      keywordThreshold: 60,
    };
    const result = scoreShortAnswer('html和css是前端基础', q);
    expect(result.matchedKeywords).toContain('html');
    expect(result.matchedKeywords).toContain('css');
  });

  it('默认阈值60%', () => {
    const q = {
      type: 'short_answer',
      score: 10,
      keywords: ['A', 'B', 'C'],
      // 不设置 keywordThreshold，默认 60
    };
    // 2/3 = 66.7%，超过默认60%阈值，应得满分
    const result = scoreShortAnswer('A B', q);
    expect(result.score).toBe(10);
  });
});

// ============================================================
// calculateEarnedScore
// ============================================================
describe('calculateEarnedScore - 得分计算', () => {
  describe('单选题/判断题 - 对得满分，错得0分', () => {
    it('单选正确得满分', () => {
      const q = { type: 'single_choice', answer: 1, score: 5 };
      expect(calculateEarnedScore(q, 1, true)).toBe(5);
    });

    it('单选错误得0分', () => {
      const q = { type: 'single_choice', answer: 1, score: 5 };
      expect(calculateEarnedScore(q, 0, false)).toBe(0);
    });

    it('判断正确得满分', () => {
      const q = { type: 'true_false', answer: true, score: 3 };
      expect(calculateEarnedScore(q, true, true)).toBe(3);
    });
  });

  describe('多选题 - 按比例得分，选错扣分', () => {
    const q = { type: 'multiple_choice', answer: [0, 1, 2], score: 12 };

    it('全部正确得满分', () => {
      expect(calculateEarnedScore(q, [0, 1, 2], true)).toBe(12);
    });

    it('部分正确按比例得分', () => {
      // 选了 [0, 1]，两个对的各得 4分 = 8分
      expect(calculateEarnedScore(q, [0, 1], false)).toBe(8);
    });

    it('选对一些选错一些 - 扣分后取最低0', () => {
      // 选了 [0, 3]，一个对(+4)一个错(-4) = 0
      expect(calculateEarnedScore(q, [0, 3], false)).toBe(0);
    });

    it('全选错 - 0分（不为负）', () => {
      expect(calculateEarnedScore(q, [3, 4, 5], false)).toBe(0);
    });

    it('空答案 - 0分', () => {
      expect(calculateEarnedScore(q, [], false)).toBe(0);
    });
  });

  describe('连线题 - 按比例得分', () => {
    const q = { type: 'matching', answer: { 0: 1, 1: 0, 2: 2 }, score: 9 };

    it('全部正确得满分', () => {
      expect(calculateEarnedScore(q, { '0': 1, '1': 0, '2': 2 }, true)).toBe(9);
    });

    it('2/3 正确得 2/3 分', () => {
      expect(calculateEarnedScore(q, { '0': 1, '1': 0, '2': 0 }, false)).toBe(6);
    });

    it('1/3 正确得 1/3 分', () => {
      expect(calculateEarnedScore(q, { '0': 1, '1': 1, '2': 0 }, false)).toBe(3);
    });

    it('全部错误得0分', () => {
      expect(calculateEarnedScore(q, { '0': 0, '1': 1, '2': 1 }, false)).toBe(0);
    });

    it('空连线得0分', () => {
      expect(calculateEarnedScore(q, {}, false)).toBe(0);
    });
  });

  describe('表格题 - 按比例得分', () => {
    const q = {
      type: 'table_question',
      answer: { '0,1': '苹果', '1,1': '香蕉' },
      score: 10,
    };

    it('全部正确得满分', () => {
      expect(calculateEarnedScore(q, { '0,1': '苹果', '1,1': '香蕉' }, true)).toBe(10);
    });

    it('1/2 正确得一半分', () => {
      expect(calculateEarnedScore(q, { '0,1': '苹果', '1,1': '西瓜' }, false)).toBe(5);
    });

    it('全部错误得0分', () => {
      expect(calculateEarnedScore(q, { '0,1': '西瓜', '1,1': '西瓜' }, false)).toBe(0);
    });
  });

  describe('简答题 - 走 scoreShortAnswer 评分', () => {
    const q = {
      type: 'short_answer',
      score: 10,
      keywords: ['编程', '算法', '数据结构'],
      keywordThreshold: 60,
    };

    it('匹配足够关键词得满分', () => {
      // 2/3 = 66.7% >= 60%
      expect(calculateEarnedScore(q, '编程和算法很重要', true)).toBe(10);
    });

    it('匹配不够关键词按规则得分', () => {
      // 1/3 = 33.3%，在30%-60%之间，得70%
      expect(calculateEarnedScore(q, '编程很有趣', false)).toBe(7);
    });
  });
});

// ============================================================
// createQuestionData - 题目数据结构
// ============================================================
describe('createQuestionData - 题目数据结构创建', () => {
  it('单选题 - 包含 options 和 answer', () => {
    const q = createQuestionData('single_choice');
    expect(q.type).toBe('single_choice');
    expect(q.options).toHaveLength(4);
    expect(q.answer).toBe(0);
    expect(q.score).toBe(5);
    expect(q.id).toMatch(/^q_\d+$/);
  });

  it('多选题 - answer 是数组', () => {
    const q = createQuestionData('multiple_choice');
    expect(q.type).toBe('multiple_choice');
    expect(q.options).toHaveLength(4);
    expect(Array.isArray(q.answer)).toBe(true);
    expect(q.answer).toEqual([0]);
  });

  it('填空题 - 包含 blanks 数组', () => {
    const q = createQuestionData('fill_blank');
    expect(q.type).toBe('fill_blank');
    expect(q.blanks).toHaveLength(2);
    expect(q.blanks[0]).toHaveProperty('answer');
  });

  it('判断题 - answer 为布尔值', () => {
    const q = createQuestionData('true_false');
    expect(q.type).toBe('true_false');
    expect(q.answer).toBe(true);
  });

  it('连线题 - 包含 leftItems/rightItems/answer', () => {
    const q = createQuestionData('matching');
    expect(q.type).toBe('matching');
    expect(q.leftItems).toHaveLength(3);
    expect(q.rightItems).toHaveLength(3);
    expect(q.answer).toEqual({ 0: 0, 1: 1, 2: 2 });
  });

  it('排序题 - 包含 items 和 answer 数组', () => {
    const q = createQuestionData('sort_question');
    expect(q.type).toBe('sort_question');
    expect(q.items).toHaveLength(4);
    expect(q.answer).toEqual([0, 1, 2, 3]);
  });

  it('表格题 - 包含 tableData 和 answer', () => {
    const q = createQuestionData('table_question');
    expect(q.type).toBe('table_question');
    expect(q.tableData.headers).toHaveLength(2);
    expect(q.tableData.rows).toHaveLength(3);
    expect(Object.keys(q.answer)).toHaveLength(3);
  });

  it('简答题 - 包含 keywords 和 keywordThreshold', () => {
    const q = createQuestionData('short_answer');
    expect(q.type).toBe('short_answer');
    expect(q.answer).toBe('');
    expect(q.keywords).toHaveLength(2);
    expect(q.keywordThreshold).toBe(60);
  });
});

// ============================================================
// 边界情况
// ============================================================
describe('边界情况', () => {
  it('checkAnswer - 未知题型返回 false', () => {
    expect(checkAnswer({ type: 'unknown' }, 'anything')).toBe(false);
  });

  it('calculateEarnedScore - 连线题空答案对象', () => {
    const q = { type: 'matching', answer: {}, score: 10 };
    expect(calculateEarnedScore(q, {}, true)).toBe(0);
  });

  it('calculateEarnedScore - 多选题空正确答案', () => {
    const q = { type: 'multiple_choice', answer: [], score: 10 };
    expect(calculateEarnedScore(q, [0, 1], false)).toBe(0);
  });

  it('checkAnswer - 填空题非数组答案返回 false', () => {
    const q = { type: 'fill_blank', blanks: [{ answer: 'test' }] };
    expect(checkAnswer(q, 'test')).toBe(false);
  });
});

// ============================================================
// parseExamAiSseData - SSE 数据解析
// ============================================================
describe('parseExamAiSseData - SSE JSON 数据解析', () => {
  it('解析有效 JSON 字符串', () => {
    const result = parseExamAiSseData('{"message":"处理中","progress":50}');
    expect(result).toEqual({ message: '处理中', progress: 50 });
  });

  it('解析 done 事件数据（含 summary）', () => {
    const data = JSON.stringify({
      message: '提交成功',
      summary: '本次测验完成较好，基础知识掌握比较扎实。'
    });
    const result = parseExamAiSseData(data);
    expect(result.message).toBe('提交成功');
    expect(result.summary).toContain('完成较好');
  });

  it('无效 JSON 返回 null', () => {
    expect(parseExamAiSseData('not json')).toBeNull();
    expect(parseExamAiSseData('{broken')).toBeNull();
    expect(parseExamAiSseData('')).toBeNull();
  });

  it('嵌套对象正确解析', () => {
    const data = JSON.stringify({
      message: 'ok',
      detail: { analysis: '掌握扎实', suggestions: ['复习', '练习'] }
    });
    const result = parseExamAiSseData(data);
    expect(result.detail.analysis).toBe('掌握扎实');
    expect(result.detail.suggestions).toHaveLength(2);
  });
});

// ============================================================
// buildExamSseUrl - SSE 提交 URL 构建
// ============================================================
describe('buildExamSseUrl - SSE 提交 URL 构建', () => {
  it('正确拼接所有参数', () => {
    const url = buildExamSseUrl('uploadanswer.ashx', {
      lid: '101', cid: '5', eid: '20',
      score: 85, spend: 12, qcount: 10,
      adata: '{"answers":[]}'
    });
    expect(url).toContain('uploadanswer.ashx?');
    expect(url).toContain('lid=101');
    expect(url).toContain('cid=5');
    expect(url).toContain('eid=20');
    expect(url).toContain('score=85');
    expect(url).toContain('spend=12');
    expect(url).toContain('qcount=10');
    expect(url).toContain('adata=');
  });

  it('特殊字符被 URL 编码', () => {
    const url = buildExamSseUrl('uploadanswer.ashx', {
      lid: '1', cid: '2', eid: '3',
      score: 0, spend: 0, qcount: 0,
      adata: '{"key":"值 & 特殊"}'
    });
    // 中文和特殊字符应被编码
    expect(url).not.toContain(' & ');
    expect(url).toContain('adata=');
    // 验证 URL 解码后能还原
    const adataParam = url.split('adata=')[1];
    expect(decodeURIComponent(adataParam)).toBe('{"key":"值 & 特殊"}');
  });

  it('空 adata 不会导致错误', () => {
    const url = buildExamSseUrl('uploadanswer.ashx', {
      lid: '1', cid: '2', eid: '3',
      score: 0, spend: 0, qcount: 0,
      adata: ''
    });
    expect(url).toContain('adata=');
  });
});
