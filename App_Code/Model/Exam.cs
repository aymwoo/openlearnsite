using System;
using System.Collections.Generic;

namespace LearnSite.Model
{
    /// <summary>
    /// 考试实体类
    /// </summary>
    public class Exam
    {
        public int ExamId { get; set; }
        public string ExamCode { get; set; }
        public string ExamName { get; set; }
        public int PaperId { get; set; }
        
        /// <summary>
        /// 考试类型：1正式考试 2模拟考试 3练习
        /// </summary>
        public int ExamType { get; set; }
        
        /// <summary>
        /// 时间模式：1固定时间 2时间段模式（发布后N天内有效）
        /// </summary>
        public int TimeMode { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 有效天数（TimeMode=2时使用）
        /// </summary>
        public int ValidDays { get; set; }

        public int Duration { get; set; }
        public int LateMinutes { get; set; }
        public int AllowRetake { get; set; }
        
        /// <summary>
        /// 显示答案：0不显示 1交卷后显示 2结束后显示
        /// </summary>
        public int ShowAnswer { get; set; }
        
        /// <summary>
        /// 显示分数：0不显示 1交卷后显示 2结束后显示
        /// </summary>
        public int ShowScore { get; set; }
        
        public int ShowRank { get; set; }
        public string AntiCheat { get; set; }
        public string Password { get; set; }
        public string IpWhitelist { get; set; }
        public int MaxParticipants { get; set; }
        
        /// <summary>
        /// 参与者类型：1指定班级 2全校 3指定学生
        /// </summary>
        public int ParticipantType { get; set; }
        
        public string Participants { get; set; }
        
        /// <summary>
        /// 状态：0未发布 1已发布 2进行中 3已结束 4已归档
        /// </summary>
        public int Status { get; set; }
        
        public DateTime? PublishTime { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        
        // 扩展属性
        public string PaperName { get; set; }
        public decimal TotalScore { get; set; }
        public int ParticipantCount { get; set; }
        public int SubmittedCount { get; set; }
    }

    /// <summary>
    /// 试卷实体类
    /// </summary>
    public class ExamPaper
    {
        public int PaperId { get; set; }
        public string PaperCode { get; set; }
        public string PaperName { get; set; }
        
        /// <summary>
        /// 类型：1普通试卷 2随机试卷 3AB卷
        /// </summary>
        public int PaperType { get; set; }
        
        public int? SubjectId { get; set; }
        public int? GradeId { get; set; }
        public decimal TotalScore { get; set; }
        public decimal PassScore { get; set; }
        public int QuestionCount { get; set; }
        public int Duration { get; set; }
        public string Description { get; set; }
        
        /// <summary>
        /// 试卷结构JSON
        /// </summary>
        public string Sections { get; set; }
        
        /// <summary>
        /// 随机组卷配置JSON
        /// </summary>
        public string RandomConfig { get; set; }
        
        public int Status { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        
        // 扩展属性
        public string SubjectName { get; set; }
        public string GradeName { get; set; }
    }

    /// <summary>
    /// 题库实体类
    /// </summary>
    public class ExamQuestionBank
    {
        public int BankId { get; set; }
        public string BankName { get; set; }
        public string BankCode { get; set; }
        public int? SubjectId { get; set; }
        public int? GradeId { get; set; }
        public int? CourseId { get; set; }
        public string Description { get; set; }
        public int QuestionCount { get; set; }
        public int Status { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
    }

    /// <summary>
    /// 题目实体类
    /// </summary>
    public class ExamQuestion
    {
        public long QuestionId { get; set; }
        public int BankId { get; set; }
        
        /// <summary>
        /// 年级ID（用于课堂测试时按年级筛选题目）
        /// </summary>
        public int? GradeId { get; set; }
        
        /// <summary>
        /// 课程ID（学案ID，用于课堂测试时按课程筛选题目）
        /// </summary>
        public int? CourseId { get; set; }
        
        /// <summary>
        /// 题型：
        /// 1=单选 2=多选 3=判断 4=填空 5=简答 
        /// 6=连线 7=分类 8=组合题
        /// 9=多项填空 10=下拉选择 11=打分题
        /// 12=矩阵单选 13=矩阵多选 14=NPS评分
        /// </summary>
        public int QuestionType { get; set; }
        
        public string QuestionContent { get; set; }
        public string QuestionText { get; set; }
        
        /// <summary>
        /// 选项JSON - 格式根据题型不同：
        /// 单选/多选/下拉: [{Label:"A",Content:"选项内容",IsCorrect:false}]
        /// 填空: [{Label:"空1",Content:"答案"}]
        /// 多项填空: [{Label:"空1",Content:"答案1"},{Label:"空2",Content:"答案2"}]
        /// 打分题: {Min:1,Max:5,Default:3}
        /// 矩阵题: {Rows:["行1","行2"],Cols:["列1","列2"]}
        /// NPS: {Min:0,Max:10,LowText:"不满意",HighText:"非常满意"}
        /// </summary>
        public string Options { get; set; }
        
        /// <summary>
        /// 答案JSON - 格式根据题型不同：
        /// 单选: "A"
        /// 多选: "A,B,C"
        /// 判断: "对" 或 "错"
        /// 填空: "答案1|答案2"
        /// 多项填空: ["答案1","答案2"]
        /// 打分题: 5
        /// 矩阵单选: {"行1":"列1","行2":"列2"}
        /// 矩阵多选: {"行1":["列1","列2"],"行2":["列1"]}
        /// NPS: 8
        /// </summary>
        public string Answer { get; set; }
        
        /// <summary>
        /// 题目配置JSON - 额外配置：
        /// 计分模式、答案匹配规则、部分得分设置等
        /// </summary>
        public string QuestionConfig { get; set; }
        
        public string Analysis { get; set; }
        public decimal Score { get; set; }
        
        /// <summary>
        /// 难度：1简单 2中等 3困难
        /// </summary>
        public int Difficulty { get; set; }
        
        public string KnowledgePoint { get; set; }
        public string Tags { get; set; }
        public string Image { get; set; }
        public string Audio { get; set; }
        public string Video { get; set; }
        public long? ParentId { get; set; }
        public int SortOrder { get; set; }
        public int Status { get; set; }
        public int UseCount { get; set; }
        public decimal CorrectRate { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateTime { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateTime { get; set; }
        
        // 扩展属性
        public string BankName { get; set; }
        public string QuestionTypeName { get; set; }
        public string CourseName { get; set; } // 课程名称（扩展属性，用于显示）
    }

    /// <summary>
    /// 试卷题目关联实体
    /// </summary>
    public class ExamPaperQuestion
    {
        public long Id { get; set; }
        public int PaperId { get; set; }
        public long QuestionId { get; set; }
        public string SectionName { get; set; }
        public decimal Score { get; set; }
        public int SortOrder { get; set; }
        
        // 扩展属性
        public ExamQuestion Question { get; set; }
    }

    /// <summary>
    /// 答卷实体类
    /// </summary>
    public class ExamAnswer
    {
        public long AnswerId { get; set; }
        public int ExamId { get; set; }
        public int PaperId { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public int? ClassId { get; set; }
        
        /// <summary>
        /// 答案JSON
        /// </summary>
        public string Answers { get; set; }
        
        public string TempAnswers { get; set; }
        public string RandomQuestions { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? SubmitTime { get; set; }
        public int Duration { get; set; }
        public decimal TotalScore { get; set; }
        public decimal ObjectiveScore { get; set; }
        public decimal? SubjectiveScore { get; set; }
        public string ScoreDetails { get; set; }
        
        /// <summary>
        /// 状态：0答题中 1已提交 2已批改
        /// </summary>
        public int Status { get; set; }
        
        public string IpAddress { get; set; }
        public string UserAgent { get; set; }
        public string MarkedBy { get; set; }
        public DateTime? MarkTime { get; set; }
        public string Remark { get; set; }
        
        // 扩展属性
        public string ExamName { get; set; }
        public string ClassName { get; set; }
    }

    /// <summary>
    /// 成绩实体类
    /// </summary>
    public class ExamResult
    {
        public long ResultId { get; set; }
        public int ExamId { get; set; }
        public long AnswerId { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public int? ClassId { get; set; }
        public decimal TotalScore { get; set; }
        public decimal ObjectiveScore { get; set; }
        public decimal SubjectiveScore { get; set; }
        public int? RankInClass { get; set; }
        public int? RankInGrade { get; set; }
        public int CorrectCount { get; set; }
        public int WrongCount { get; set; }
        public int PartialCount { get; set; }
        public int Duration { get; set; }
        public DateTime? SubmitTime { get; set; }
        public int Status { get; set; }
        
        // 扩展属性
        public string ExamName { get; set; }
        public string ClassName { get; set; }
        public decimal PassScore { get; set; }
        public decimal TotalScoreMax { get; set; }
        public bool IsPassed { get { return TotalScore >= PassScore; } }
    }

    /// <summary>
    /// 考试设置实体类
    /// </summary>
    public class ExamSetting
    {
        public int SettingId { get; set; }
        public string SettingKey { get; set; }
        public string SettingValue { get; set; }
        public string Description { get; set; }
    }

    #region JSON数据结构类

    /// <summary>
    /// 试卷结构
    /// </summary>
    public class PaperSection
    {
        public string SectionName { get; set; }
        public string SectionDesc { get; set; }
        public List<SectionQuestion> Questions { get; set; }
    }

    public class SectionQuestion
    {
        public long QuestionId { get; set; }
        public decimal Score { get; set; }
        public int SortOrder { get; set; }
    }

    /// <summary>
    /// 题目选项
    /// </summary>
    public class QuestionOption
    {
        public string Label { get; set; }  // A, B, C, D
        public string Content { get; set; }
        public string Image { get; set; }
        public bool IsCorrect { get; set; }
    }

    /// <summary>
    /// 打分题配置
    /// </summary>
    public class ScoreConfig
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public int Default { get; set; }
        public string Style { get; set; }
        public string LowLabel { get; set; }
        public string HighLabel { get; set; }
    }

    /// <summary>
    /// 矩阵题配置
    /// </summary>
    public class MatrixConfig
    {
        public List<string> Rows { get; set; }   // 行标题
        public List<string> Cols { get; set; }   // 列标题
        public bool AllowMultiple { get; set; }  // 是否多选（矩阵多选）
    }

    /// <summary>
    /// NPS评分配置
    /// </summary>
    public class NpsConfig
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public string LowText { get; set; }
        public string MidText { get; set; }
        public string HighText { get; set; }
        public int LowThreshold { get; set; }
        public int HighThreshold { get; set; }
    }

    /// <summary>
    /// 多项填空配置
    /// </summary>
    public class MultipleBlankConfig
    {
        public List<BlankItem> Blanks { get; set; }
    }

    public class BlankItem
    {
        public string Label { get; set; }        // 填空标签，如"空1"
        public string Placeholder { get; set; }  // 提示文本
        public bool Required { get; set; }       // 是否必填
        public string Answer { get; set; }       // 正确答案
        public decimal Score { get; set; }       // 该空分值
    }

    /// <summary>
    /// 下拉选择题配置
    /// </summary>
    public class SelectConfig
    {
        public List<QuestionOption> Options { get; set; }
        public bool Searchable { get; set; }     // 是否可搜索
        public bool Multiple { get; set; }       // 是否多选
        public string Placeholder { get; set; }  // 提示文本
    }

    /// <summary>
    /// 计分模式
    /// </summary>
    public enum ScoreMode
    {
        OnlyOne = 1,        // 只有一个正确答案
        SelectAll = 2,      // 全部选对才得分
        SelectCorrect = 3,  // 按正确答案计分（选对得分，选错扣分）
        SelectPartial = 4,  // 按选中项算分（部分得分）
        Manual = 5,         // 人工打分
        None = 0            // 不计分
    }

    /// <summary>
    /// 题目配置（用于QuestionConfig字段）
    /// </summary>
    public class QuestionConfigModel
    {
        public ScoreMode ScoreMode { get; set; }
        public decimal? PartialScoreRatio { get; set; }
        public bool CaseSensitive { get; set; }
        public bool TrimWhitespace { get; set; }
        public ScoreConfig ScoreConfig { get; set; }
        public MatrixConfig MatrixConfig { get; set; }
        public NpsConfig NpsConfig { get; set; }
        public MultipleBlankConfig MultipleBlankConfig { get; set; }
    }

    /// <summary>
    /// 随机组卷配置
    /// </summary>
    public class RandomConfig
    {
        public bool IsRandom { get; set; }
        public List<RandomRule> Rules { get; set; }
    }

    public class RandomRule
    {
        public int? BankId { get; set; }
        public int QuestionType { get; set; }
        public int Difficulty { get; set; }
        public int Count { get; set; }
        public decimal ScorePerQuestion { get; set; }
    }

    /// <summary>
    /// 防作弊设置
    /// </summary>
    public class AntiCheatConfig
    {
        public bool DisableCopy { get; set; }
        public bool DisablePaste { get; set; }
        public bool DisableRightClick { get; set; }
        public bool DisablePrint { get; set; }
        public bool DetectSwitch { get; set; }  // 是否检测切屏
        public int MaxSwitchCount { get; set; }  // 最大切屏次数
        public bool AutoSubmitOnSwitch { get; set; }  // 切屏超限自动提交
        public bool FaceRecognition { get; set; }  // 人脸识别
        public bool WebcamMonitor { get; set; }  // 摄像头监控
        public bool ScreenRecord { get; set; }  // 屏幕录制
    }

    /// <summary>
    /// 评分详情
    /// </summary>
    public class ScoreDetail
    {
        public long QuestionId { get; set; }
        public string UserAnswer { get; set; }
        public string CorrectAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public decimal Score { get; set; }
        public decimal MaxScore { get; set; }
        public string Remark { get; set; }
    }

    #endregion

    /// <summary>
    /// 考试统计信息
    /// </summary>
    public class ExamStatistics
    {
        public int TotalParticipants { get; set; }
        public int SubmittedCount { get; set; }
        public int InProgressCount { get; set; }
        public decimal AverageScore { get; set; }
        public decimal MaxScore { get; set; }
        public decimal MinScore { get; set; }
        public decimal PassRate { get; set; }
        public int ExcellentCount { get; set; }
        public int GoodCount { get; set; }
        public int PassCount { get; set; }
        public int FailCount { get; set; }
    }
}
