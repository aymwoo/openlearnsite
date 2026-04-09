-- =============================================
-- 智能试卷生成系统 - 极简两表设计
-- 只包含试卷表(exam)和答题表(answer)
-- 无外键约束，无索引，结构简单
-- =============================================

-- 1. 试卷表 (exam)
CREATE TABLE [dbo].[Exams] (
    [Eid] INT IDENTITY(1,1)  PRIMARY KEY,
    [Etitle] NVARCHAR(200) NOT NULL,            -- 试卷标题
    [Edescription] NVARCHAR(500) NULL,          -- 试卷描述
    [Cid]  INT NOT NULL,                    -- 学案id
    [Hid]  INT NOT NULL,                    -- 教师id
    [Etime] DATETIME NULL,                  -- 创建时间
    [Eclose] bit DEFAULT 1,                   -- 状态：0草稿 1发布
    [Escore] INT DEFAULT 0,                     -- 总分
    [Ecount] INT DEFAULT 0,                     -- 题目数量
    [Edata] NTEXT NOT NULL                    -- 题目数据(JSON格式)
);

-- 2. 答题表 (answer)
CREATE TABLE [dbo].[Answers] (
    [Aid] INT IDENTITY(1,1)  PRIMARY KEY,
    [Eid] INT NOT NULL,                -- 试卷ID
    [Asid] INT NOT NULL,               -- 学生ID
    [Asnum] NVARCHAR(50) NULL,              -- 学生学号
    [Asname] NVARCHAR(50) NULL,        -- 学生姓名
    [Asgrade] INT NULL,                -- 学生班级
    [Asclass] INT NULL,                -- 学生班级
    [Atime] DATETIME NULL,             -- 提交时间
    [Ascore] INT NULL,                 -- 总得分
    [Aspent] INT NULL,                 -- 用时(秒)
    [Adata] NTEXT NULL                  -- 答案数据(JSON格式)
);

-- =============================================
-- 表注释
-- =============================================

-- 试卷表注释
EXEC sp_addextendedproperty 
    @name = N'MS_Description', @value = N'试卷主表，存储试卷基本信息和题目数据',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'exam';

EXEC sp_addextendedproperty 
    @name = N'MS_Description', @value = N'试卷唯一标识',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'exam',
    @level2type = N'COLUMN', @level2name = N'id';

EXEC sp_addextendedproperty 
    @name = N'MS_Description', @value = N'题目数据，JSON格式存储所有题目信息',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'exam',
    @level2type = N'COLUMN', @level2name = N'questions_data';

-- 答题表注释
EXEC sp_addextendedproperty 
    @name = N'MS_Description', @value = N'答题记录表，存储学生答题信息和答案数据',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'answer';

EXEC sp_addextendedproperty 
    @name = N'MS_Description', @value = N'答案数据，JSON格式存储学生的所有答案',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE', @level1name = N'answer',
    @level2type = N'COLUMN', @level2name = N'answers_data';

-- =============================================
-- 示例数据插入
-- =============================================

-- 插入示例试卷
INSERT INTO [dbo].[exam] (
    [title], 
    [description], 
    [creator],
    [total_score],
    [time_limit],
    [pass_score],
    [question_count],
    [questions_data]
) VALUES (
    N'信息科技初级试卷',
    N'请仔细阅读题目并作答',
    N'admin',
    100,
    60,
    60,
    4,
    N'{
        "title": "信息科技初级试卷",
        "description": "请仔细阅读题目并作答",
        "questions": [
            {
                "id": "q_1704067200000",
                "type": "single_choice",
                "title": "<p>以下哪个是JavaScript的数据类型？</p>",
                "score": 25,
                "options": ["String", "Integer", "Float", "Character"],
                "answer": 0
            },
            {
                "id": "q_1704067260000",
                "type": "multiple_choice",
                "title": "<p>HTML5新增的语义化标签包括：</p>",
                "score": 25,
                "options": ["<header>", "<nav>", "<section>", "<article>"],
                "answer": [0, 1, 2, 3]
            },
            {
                "id": "q_1704067320000",
                "type": "fill_blank",
                "title": "<p>CSS中设置元素宽度的属性是___，设置高度的属性是___。</p>",
                "score": 25,
                "blanks": [
                    {"answer": "width"},
                    {"answer": "height"}
                ]
            },
            {
                "id": "q_1704067380000",
                "type": "short_answer",
                "title": "<p>请简述HTTP协议的特点。</p>",
                "score": 25,
                "answer": "HTTP协议是无状态的、基于请求-响应模式的应用层协议",
                "keywords": ["无状态", "请求", "响应", "应用层"],
                "keywordThreshold": 60
            }
        ]
    }'
);

-- 插入示例答题记录
DECLARE @exam_id UNIQUEIDENTIFIER;
SELECT @exam_id = id FROM [dbo].[exam] WHERE title = N'信息科技初级试卷';

INSERT INTO [dbo].[answer] (
    [exam_id],
    [student_id],
    [student_name],
    [student_class],
    [submit_time],
    [total_score],
    [status],
    [time_spent],
    [answers_data]
) VALUES (
    @exam_id,
    N'2024001',
    N'张三',
    N'计算机1班',
    GETDATE(),
    85.5,
    2,
    1800,
    N'{
        "q_1704067200000": {
            "type": "single_choice",
            "answer": 0,
            "score": 25,
            "isCorrect": true
        },
        "q_1704067260000": {
            "type": "multiple_choice", 
            "answer": [0, 1, 2],
            "score": 18.75,
            "isCorrect": false
        },
        "q_1704067320000": {
            "type": "fill_blank",
            "answer": ["width", "height"],
            "score": 25,
            "isCorrect": true
        },
        "q_1704067380000": {
            "type": "short_answer",
            "answer": "HTTP是无状态协议，采用请求响应模式",
            "score": 16.75,
            "isCorrect": false,
            "matchedKeywords": ["无状态", "请求", "响应"]
        }
    }'
);

-- =============================================
-- 常用查询示例
-- =============================================

-- 1. 查询所有试卷基本信息
SELECT 
    id,
    title,
    description,
    creator,
    create_time,
    status,
    total_score,
    time_limit,
    question_count,
    view_count,
    submit_count
FROM [dbo].[exam]
WHERE status = 2; -- 已发布的试卷

-- 2. 查询试卷详细信息（包含题目）
SELECT 
    id,
    title,
    description,
    total_score,
    time_limit,
    questions_data
FROM [dbo].[exam]
WHERE id = 'your-exam-id';

-- 3. 查询学生答题记录
SELECT 
    a.id,
    e.title AS exam_title,
    a.student_id,
    a.student_name,
    a.student_class,
    a.start_time,
    a.submit_time,
    a.total_score,
    a.status,
    a.time_spent
FROM [dbo].[answer] a, [dbo].[exam] e
WHERE a.exam_id = e.id
  AND a.student_id = '2024001'
ORDER BY a.submit_time DESC;

-- 4. 查询试卷统计信息
SELECT 
    e.id,
    e.title,
    e.total_score AS max_score,
    COUNT(a.id) AS total_submissions,
    AVG(a.total_score) AS avg_score,
    MAX(a.total_score) AS max_score_achieved,
    MIN(a.total_score) AS min_score_achieved,
    COUNT(CASE WHEN a.total_score >= e.pass_score THEN 1 END) AS pass_count
FROM [dbo].[exam] e, [dbo].[answer] a
WHERE e.id = a.exam_id
  AND a.status = 2 -- 已提交
GROUP BY e.id, e.title, e.total_score, e.pass_score;

-- 5. 查询学生答题详情
SELECT 
    a.id,
    e.title AS exam_title,
    a.student_name,
    a.total_score,
    a.submit_time,
    a.answers_data
FROM [dbo].[answer] a, [dbo].[exam] e
WHERE a.exam_id = e.id
  AND a.id = 'your-answer-id';

-- 6. 查询正在答题的学生
SELECT 
    a.student_id,
    a.student_name,
    e.title AS exam_title,
    a.start_time,
    DATEDIFF(MINUTE, a.start_time, GETDATE()) AS elapsed_minutes,
    e.time_limit
FROM [dbo].[answer] a, [dbo].[exam] e
WHERE a.exam_id = e.id
  AND a.status = 1 -- 答题中
  AND (e.time_limit IS NULL OR DATEDIFF(MINUTE, a.start_time, GETDATE()) < e.time_limit);

-- 7. 更新试卷浏览次数
UPDATE [dbo].[exam] 
SET view_count = view_count + 1 
WHERE id = 'your-exam-id';

-- 8. 更新试卷提交次数
UPDATE [dbo].[exam] 
SET submit_count = submit_count + 1 
WHERE id = 'your-exam-id';

-- =============================================
-- JSON数据操作示例（SQL Server 2016+）
-- =============================================

-- 查询试卷中的题目数量
SELECT 
    id,
    title,
    JSON_VALUE(questions_data, '$.questions.size()') AS question_count_from_json
FROM [dbo].[exam];

-- 查询试卷中特定类型的题目
SELECT 
    e.id,
    e.title,
    q.value AS question_data
FROM [dbo].[exam] e
CROSS APPLY OPENJSON(e.questions_data, '$.questions') q
WHERE JSON_VALUE(q.value, '$.type') = 'single_choice';

-- 查询学生某道题的答案
SELECT 
    a.student_name,
    JSON_VALUE(a.answers_data, '$.q_1704067200000.answer') AS student_answer,
    JSON_VALUE(a.answers_data, '$.q_1704067200000.score') AS question_score
FROM [dbo].[answer] a
WHERE a.student_id = '2024001';

-- =============================================
-- 数据清理和维护
-- =============================================

-- 删除超时未提交的答题记录（超过时间限制2倍）
UPDATE a 
SET status = 3 -- 超时
FROM [dbo].[answer] a, [dbo].[exam] e
WHERE a.exam_id = e.id
  AND a.status = 1 -- 答题中
  AND e.time_limit IS NOT NULL
  AND DATEDIFF(MINUTE, a.start_time, GETDATE()) > (e.time_limit * 2);

-- 清理草稿状态的旧试卷（30天前创建且未发布）
DELETE FROM [dbo].[exam]
WHERE status = 1 -- 草稿
  AND create_time < DATEADD(DAY, -30, GETDATE());

-- 备份已归档的试卷数据
SELECT * 
INTO [dbo].[exam_archive_backup]
FROM [dbo].[exam]
WHERE status = 3 -- 归档
  AND create_time < DATEADD(YEAR, -1, GETDATE());