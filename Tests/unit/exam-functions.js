/**
 * 从 exam.js / preview.js 中提取的纯逻辑函数
 * 
 * 这些函数原本是全局函数，不依赖 DOM，可以独立测试。
 * 这里将它们以 ES Module 方式导出，供 Vitest 测试使用。
 * 
 * 注意: 保持与源文件中的实现完全一致，确保测试的是真实逻辑。
 */

/**
 * 题目类型中文映射 (来自 exam.js:1232)
 */
export function getQuestionTypeText(type) {
  const typeMap = {
    'single_choice': '单选题',
    'multiple_choice': '多选题',
    'fill_blank': '填空题',
    'true_false': '判断题',
    'matching': '连线题',
    'sort_question': '排序题',
    'table_question': '表格题',
    'short_answer': '简答题'
  };
  return typeMap[type] || '未知题型';
}

/**
 * 检查答案是否正确 (来自 preview.js:1206 / exam.js:2818)
 */
export function checkAnswer(question, userAnswer) {
  if (userAnswer === null || userAnswer === undefined) return false;

  let correctCount = 0;
  let totalCount = 0;

  switch (question.type) {
    case 'single_choice':
    case 'true_false':
      return userAnswer === question.answer;

    case 'multiple_choice':
      if (!Array.isArray(userAnswer) || !Array.isArray(question.answer)) return false;
      if (userAnswer.length !== question.answer.length) return false;
      const sortedUserAnswer = [...userAnswer].sort((a, b) => a - b);
      const sortedCorrectAnswer = [...question.answer].sort((a, b) => a - b);
      return sortedUserAnswer.every((answer, index) => answer === sortedCorrectAnswer[index]);

    case 'fill_blank':
      if (!Array.isArray(userAnswer)) return false;
      const correctAnswers = (question.blanks || []).map(blank =>
        blank.answer.toString().trim().toLowerCase()
      );
      const userAnswers = userAnswer.map(answer =>
        answer.toString().trim().toLowerCase()
      ).filter(answer => answer !== '');
      if (userAnswers.length !== correctAnswers.length) return false;
      const correctAnswersCopy = [...correctAnswers];
      for (let userAns of userAnswers) {
        const index = correctAnswersCopy.indexOf(userAns);
        if (index === -1) return false;
        correctAnswersCopy.splice(index, 1);
      }
      return correctAnswersCopy.length === 0;

    case 'matching':
      const userConnections = userAnswer;
      const correctConnections = question.answer;
      if (Object.keys(userConnections).length !== Object.keys(correctConnections).length) return false;
      for (let leftIndex in correctConnections) {
        const leftIndexStr = leftIndex.toString();
        const correctRightIndex = correctConnections[leftIndex];
        const userRightIndex = userConnections[leftIndexStr];
        if (userRightIndex !== correctRightIndex) return false;
      }
      return true;

    case 'sort_question':
      if (!Array.isArray(userAnswer) || userAnswer.length !== question.answer.length) return false;
      return userAnswer.every((item, index) => item === question.answer[index]);

    case 'table_question':
      const tableCorrectAnswers = question.answer;
      const tableUserAnswers = userAnswer || {};
      correctCount = 0;
      totalCount = Object.keys(tableCorrectAnswers).length;
      if (totalCount === 0) return false;
      for (let key in tableCorrectAnswers) {
        if (tableUserAnswers[key] && tableUserAnswers[key].toString().trim().toLowerCase() === tableCorrectAnswers[key].toString().trim().toLowerCase()) {
          correctCount++;
        }
      }
      return correctCount === totalCount;

    case 'short_answer':
      return userAnswer && userAnswer.toString().trim().length > 0;

    default:
      return false;
  }
}

/**
 * 简答题关键词评分 (来自 preview.js:1316 / exam.js:1491)
 */
export function scoreShortAnswer(studentAnswer, question) {
  if (!question.keywords || question.keywords.length === 0) {
    return {
      score: 0,
      maxScore: question.score,
      feedback: '未设置关键词，无法自动评分'
    };
  }

  const answer = studentAnswer.toLowerCase();
  const keywords = question.keywords.map(k => k.toLowerCase());
  const threshold = question.keywordThreshold || 60;

  let matchedCount = 0;
  const matchedKeywords = [];

  keywords.forEach(keyword => {
    if (answer.includes(keyword)) {
      matchedCount++;
      matchedKeywords.push(keyword);
    }
  });

  const matchPercentage = (matchedCount / keywords.length) * 100;

  let score = 0;
  if (matchPercentage >= threshold) {
    score = question.score;
  } else if (matchPercentage >= threshold * 0.5) {
    score = Math.round(question.score * 0.7);
  } else if (matchPercentage > 0) {
    score = Math.round(question.score * 0.3);
  }

  return {
    score: score,
    maxScore: question.score,
    matchPercentage: Math.round(matchPercentage),
    matchedKeywords: matchedKeywords,
    totalKeywords: keywords.length,
    feedback: `匹配了 ${matchedCount}/${keywords.length} 个关键词 (${Math.round(matchPercentage)}%)`
  };
}

/**
 * 计算用户得分 (来自 preview.js:1360 / exam.js:2681)
 */
export function calculateEarnedScore(question, userAnswer, isCorrect) {
  if (question.type === 'matching') {
    const correctConnections = question.answer;
    const userConnections = userAnswer || {};
    let correctCount = 0;
    let totalCount = Object.keys(correctConnections).length;
    if (totalCount === 0) return 0;
    for (let leftIndex in correctConnections) {
      const leftIndexStr = leftIndex.toString();
      const correctRightIndex = correctConnections[leftIndex];
      const userRightIndex = userConnections[leftIndexStr];
      if (userRightIndex === correctRightIndex) correctCount++;
    }
    const correctRatio = correctCount / totalCount;
    return Math.round(question.score * correctRatio);
  }

  if (question.type === 'multiple_choice') {
    const correctAnswers = question.answer || [];
    const userAnswers = userAnswer || [];
    if (correctAnswers.length === 0) return 0;
    const scorePerOption = question.score / correctAnswers.length;
    let earnedScore = 0;
    userAnswers.forEach(userChoice => {
      if (correctAnswers.includes(userChoice)) {
        earnedScore += scorePerOption;
      } else {
        earnedScore -= scorePerOption;
      }
    });
    return Math.max(0, Math.round(earnedScore));
  }

  if (question.type === 'table_question') {
    const correctAnswers = question.answer;
    const userAnswers = userAnswer || {};
    let correctCount = 0;
    let totalCount = Object.keys(correctAnswers).length;
    if (totalCount === 0) return 0;
    for (let key in correctAnswers) {
      if (userAnswers[key] && userAnswers[key].toString().trim().toLowerCase() === correctAnswers[key].toString().trim().toLowerCase()) {
        correctCount++;
      }
    }
    const correctRatio = correctCount / totalCount;
    return Math.round(question.score * correctRatio);
  }

  if (question.type === 'short_answer') {
    const scoreResult = scoreShortAnswer(userAnswer, question);
    return scoreResult.score;
  }

  return isCorrect ? question.score : 0;
}

/**
 * 创建新题目数据结构 (来自 exam.js:143 的数据部分)
 */
export function createQuestionData(type) {
  const questionId = 'q_' + Date.now();
  let newQuestion = {
    id: questionId,
    type: type,
    title: '',
    score: 5
  };

  switch (type) {
    case 'single_choice':
      newQuestion.options = ['', '', '', ''];
      newQuestion.answer = 0;
      break;
    case 'multiple_choice':
      newQuestion.options = ['', '', '', ''];
      newQuestion.answer = [0];
      break;
    case 'fill_blank':
      newQuestion.blanks = [{ answer: '' }, { answer: '' }];
      break;
    case 'true_false':
      newQuestion.answer = true;
      break;
    case 'matching':
      newQuestion.leftItems = ['', '', ''];
      newQuestion.rightItems = ['', '', ''];
      newQuestion.answer = { 0: 0, 1: 1, 2: 2 };
      break;
    case 'sort_question':
      newQuestion.items = ['', '', '', ''];
      newQuestion.answer = [0, 1, 2, 3];
      break;
    case 'table_question':
      newQuestion.tableData = {
        headers: ['', ''],
        rows: [['', ''], ['', ''], ['', '']]
      };
      newQuestion.answer = { '0,1': '', '1,1': '', '2,1': '' };
      break;
    case 'short_answer':
      newQuestion.answer = '';
      newQuestion.keywords = ['', ''];
      newQuestion.keywordThreshold = 60;
      break;
  }

  return newQuestion;
}

// ================================================================
// SSE 提交相关纯函数 (来自 preview.js 新增的 AI 评估功能)
// ================================================================

/**
 * 解析 SSE 事件中的 JSON 数据 (来自 preview.js:1147)
 */
export function parseExamAiSseData(data) {
  try { return JSON.parse(data); } catch (e) { return null; }
}

/**
 * 构建 SSE 提交的 URL 查询字符串 (从 preview.js:1042-1048 提取)
 *
 * @param {string} baseUrl - 基础 URL (如 'uploadanswer.ashx')
 * @param {{lid:string, cid:string, eid:string, score:number, spend:number, qcount:number, adata:string}} params
 * @returns {string} 带查询参数的完整 URL
 */
export function buildExamSseUrl(baseUrl, params) {
  return baseUrl
    + '?lid=' + encodeURIComponent(params.lid)
    + '&cid=' + encodeURIComponent(params.cid)
    + '&eid=' + encodeURIComponent(params.eid)
    + '&score=' + encodeURIComponent(params.score)
    + '&spend=' + encodeURIComponent(params.spend)
    + '&qcount=' + encodeURIComponent(params.qcount)
    + '&adata=' + encodeURIComponent(params.adata);
}
