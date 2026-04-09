using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using LearnSite.Model;
using LearnSite.DBUtility;
using LearnSite.DAL;

namespace LearnSite.BLL
{
    /// <summary>
    /// 考试业务逻辑类
    /// </summary>
    public class Exam
    {
        private readonly DAL.Exam dal = new DAL.Exam();

        #region 考试管理

        /// <summary>
        /// 获取考试列表
        /// </summary>
        public List<Model.Exam> GetExamList(string teacherId, int status = -1)
        {
            return dal.GetExamList(teacherId, status);
        }

        /// <summary>
        /// 获取学生可参加的考试列表
        /// </summary>
        public List<Model.Exam> GetStudentExamList(string studentId, int classId)
        {
            return dal.GetStudentExamList(studentId, classId);
        }

        /// <summary>
        /// 根据ID获取考试
        /// </summary>
        public Model.Exam GetExamById(int examId)
        {
            return dal.GetExamById(examId);
        }

        /// <summary>
        /// 根据编码获取考试
        /// </summary>
        public Model.Exam GetExamByCode(string examCode)
        {
            return dal.GetExamByCode(examCode);
        }

        /// <summary>
        /// 添加考试
        /// </summary>
        public int AddExam(Model.Exam exam)
        {
            exam.ExamCode = GenerateExamCode();
            exam.CreateTime = DateTime.Now;
            exam.Status = 0;
            return dal.AddExam(exam);
        }

        /// <summary>
        /// 更新考试
        /// </summary>
        public bool UpdateExam(Model.Exam exam)
        {
            exam.UpdateTime = DateTime.Now;
            return dal.UpdateExam(exam);
        }

        /// <summary>
        /// 删除考试
        /// </summary>
        public bool DeleteExam(int examId)
        {
            return dal.DeleteExam(examId);
        }

        /// <summary>
        /// 发布考试
        /// </summary>
        public bool PublishExam(int examId, string teacherId)
        {
            var exam = dal.GetExamById(examId);
            if (exam == null) return false;

            exam.Status = 1;
            exam.PublishTime = DateTime.Now;
            exam.UpdateBy = teacherId;
            return UpdateExam(exam);
        }

        /// <summary>
        /// 生成考试编码
        /// </summary>
        private string GenerateExamCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper();
        }

        #endregion

        #region 考试状态管理

        /// <summary>
        /// 检查考试是否可参加
        /// </summary>
        public ExamCheckResult CheckExamAccess(int examId, string studentId, int classId)
        {
            var result = new ExamCheckResult { CanAccess = true };
            var exam = GetExamById(examId);
            
            if (exam == null)
            {
                result.CanAccess = false;
                result.Message = "考试不存在";
                return result;
            }

            // 检查时间
            var now = DateTime.Now;
            if (now < exam.StartTime)
            {
                result.CanAccess = false;
                result.Message = string.Format("考试未开始，开始时间：{0:yyyy-MM-dd HH:mm}", exam.StartTime);
                return result;
            }

            if (now > exam.EndTime)
            {
                result.CanAccess = false;
                result.Message = "考试已结束";
                return result;
            }

            // 检查参与者
            if (!CheckParticipant(exam, studentId, classId))
            {
                result.CanAccess = false;
                result.Message = "您不在本次考试的参与名单中";
                return result;
            }

            // 检查是否已提交
            var answerBll = new ExamAnswer();
            var existingAnswer = answerBll.GetAnswerByExamAndStudent(examId, studentId);
            if (existingAnswer != null && existingAnswer.Status > 0)
            {
                result.CanAccess = false;
                result.Message = "您已提交答卷，不可重复参加";
                return result;
            }

            // 检查重考次数
            if (exam.AllowRetake > 0)
            {
                int submitCount = answerBll.GetSubmitCount(examId, studentId);
                if (submitCount >= exam.AllowRetake)
                {
                    result.CanAccess = false;
                    result.Message = string.Format("您已达到最大重考次数（{0}次）", exam.AllowRetake);
                    return result;
                }
            }

            result.Exam = exam;
            return result;
        }

        /// <summary>
        /// 检查是否在参与名单中
        /// </summary>
        private bool CheckParticipant(Model.Exam exam, string studentId, int classId)
        {
            switch (exam.ParticipantType)
            {
                case 1: // 指定班级
                    var classIds = exam.Participants != null ? exam.Participants.Split(',') : null;
                    return classIds != null && Array.IndexOf(classIds, classId.ToString()) >= 0;
                case 2: // 全校
                    return true;
                case 3: // 指定学生
                    var studentIds = exam.Participants != null ? exam.Participants.Split(',') : null;
                    return studentIds != null && Array.Exists(studentIds, s => s == studentId);
                default:
                    return true;
            }
        }

        /// <summary>
        /// 验证考试密码
        /// </summary>
        public bool VerifyPassword(int examId, string password)
        {
            var exam = GetExamById(examId);
            if (exam == null || string.IsNullOrEmpty(exam.Password))
                return true;
            return exam.Password == password;
        }

        #endregion

        #region 统计分析

        /// <summary>
        /// 获取考试统计信息
        /// </summary>
        public Model.ExamStatistics GetExamStatistics(int examId)
        {
            return dal.GetExamStatistics(examId);
        }

        /// <summary>
        /// 更新考试状态（定时任务调用）
        /// </summary>
        public void UpdateExamStatus()
        {
            dal.UpdateExamStatus();
        }

        #endregion
    }

    /// <summary>
    /// 考试访问检查结果
    /// </summary>
    public class ExamCheckResult
    {
        public bool CanAccess { get; set; }
        public string Message { get; set; }
        public Model.Exam Exam { get; set; }
    }

    /// <summary>
    /// 试卷业务逻辑类
    /// </summary>
    public class ExamPaper
    {
        private readonly DAL.ExamPaper dal = new DAL.ExamPaper();

        /// <summary>
        /// 获取试卷列表
        /// </summary>
        public List<Model.ExamPaper> GetPaperList(string teacherId, int status = -1)
        {
            return dal.GetPaperList(teacherId, status);
        }

        /// <summary>
        /// 根据条件获取试卷列表
        /// </summary>
        public List<Model.ExamPaper> GetModelList(string whereClause)
        {
            return dal.GetPaperListByWhere(whereClause);
        }

        /// <summary>
        /// 根据课程ID获取试卷列表
        /// </summary>
        public List<Model.ExamPaper> GetPaperListByCourse(int courseId)
        {
            return dal.GetPaperListByCourse(courseId);
        }

        /// <summary>
        /// 根据ID获取试卷
        /// </summary>
        public Model.ExamPaper GetPaperById(int paperId)
        {
            return dal.GetPaperById(paperId);
        }

        /// <summary>
        /// 添加试卷
        /// </summary>
        public int AddPaper(Model.ExamPaper paper)
        {
            paper.PaperCode = GeneratePaperCode();
            paper.CreateTime = DateTime.Now;
            paper.Status = 0;
            return dal.AddPaper(paper);
        }

        /// <summary>
        /// 更新试卷
        /// </summary>
        public bool UpdatePaper(Model.ExamPaper paper)
        {
            paper.UpdateTime = DateTime.Now;
            return dal.UpdatePaper(paper);
        }

        /// <summary>
        /// 删除试卷
        /// </summary>
        public bool DeletePaper(int paperId)
        {
            // 检查是否被考试引用
            var examBll = new Exam();
            var exams = examBll.GetExamList(null);
            // TODO: 检查引用
            return dal.DeletePaper(paperId);
        }

        /// <summary>
        /// 获取试卷题目列表
        /// </summary>
        public List<Model.ExamPaperQuestion> GetPaperQuestions(int paperId)
        {
            return dal.GetPaperQuestions(paperId);
        }

        /// <summary>
        /// 保存试卷题目
        /// </summary>
        public bool SavePaperQuestions(int paperId, List<Model.PaperSection> sections)
        {
            return dal.SavePaperQuestions(paperId, sections);
        }

        /// <summary>
        /// 添加试卷题目关联
        /// </summary>
        public int AddPaperQuestion(Model.ExamPaperQuestion paperQuestion)
        {
            return dal.AddPaperQuestion(paperQuestion);
        }

        /// <summary>
        /// 批量添加试卷题目关联
        /// </summary>
        public int BatchAddPaperQuestions(int paperId, List<Model.ExamPaperQuestion> paperQuestions)
        {
            return dal.BatchAddPaperQuestions(paperId, paperQuestions);
        }

        /// <summary>
        /// 删除试卷题目关联
        /// </summary>
        public bool DeletePaperQuestion(long id)
        {
            return dal.DeletePaperQuestion(id);
        }

        /// <summary>
        /// 复制试卷
        /// </summary>
        public int CopyPaper(int paperId, string newName, string teacherId)
        {
            var source = GetPaperById(paperId);
            if (source == null) return 0;

            var newPaper = new Model.ExamPaper
            {
                PaperName = newName,
                PaperType = source.PaperType,
                SubjectId = source.SubjectId,
                GradeId = source.GradeId,
                TotalScore = source.TotalScore,
                PassScore = source.PassScore,
                Duration = source.Duration,
                Description = source.Description,
                Sections = source.Sections,
                RandomConfig = source.RandomConfig,
                CreateBy = teacherId
            };

            int newId = AddPaper(newPaper);
            if (newId > 0)
            {
                // 复制题目关联
                var questions = GetPaperQuestions(paperId);
                // TODO: 复制题目关联
            }
            return newId;
        }

        private string GeneratePaperCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
        }
    }

    /// <summary>
    /// 题库业务逻辑类
    /// </summary>
    public class ExamQuestionBank
    {
        private readonly DAL.ExamQuestionBank dal = new DAL.ExamQuestionBank();

        public List<Model.ExamQuestionBank> GetBankList(string teacherId)
        {
            return dal.GetBankList(teacherId);
        }

        public Model.ExamQuestionBank GetBankById(int bankId)
        {
            return dal.GetBankById(bankId);
        }

        public int AddBank(Model.ExamQuestionBank bank)
        {
            bank.CreateTime = DateTime.Now;
            bank.QuestionCount = 0;
            bank.Status = 1;

            // 自动生成 BankCode（如果未提供）
            if (string.IsNullOrEmpty(bank.BankCode))
            {
                bank.BankCode = GenerateBankCode(bank.BankName);
            }

            return dal.AddBank(bank);
        }

        /// <summary>
        /// 自动生成题库编码
        /// </summary>
        private string GenerateBankCode(string bankName)
        {
            // 生成规则: BK + 时间戳后4位 + 随机3位
            string timestamp = DateTime.Now.ToString("yyMMddHHmmss");
            string random = new Random().Next(100, 999).ToString();
            return "BK" + timestamp + random;
        }

        public bool UpdateBank(Model.ExamQuestionBank bank)
        {
            bank.UpdateTime = DateTime.Now;
            return dal.UpdateBank(bank);
        }

        public bool DeleteBank(int bankId)
        {
            return dal.DeleteBank(bankId);
        }

        /// <summary>
        /// 更新题库题目数量
        /// </summary>
        public void UpdateQuestionCount(int bankId)
        {
            dal.UpdateQuestionCount(bankId);
        }
    }

    /// <summary>
    /// 题目业务逻辑类
    /// </summary>
    public class ExamQuestion
    {
        private readonly DAL.ExamQuestion dal = new DAL.ExamQuestion();

        /// <summary>
        /// 分页获取题目列表
        /// </summary>
        public Tuple<List<Model.ExamQuestion>, int> GetQuestionList(int bankId, int pageIndex, int pageSize, int? questionType = null, int? difficulty = null, string keyword = null, int? gradeId = null, int? courseId = null)
        {
            return dal.GetQuestionList(bankId, pageIndex, pageSize, questionType, difficulty, keyword, gradeId, courseId);
        }

        public Model.ExamQuestion GetQuestionById(long questionId)
        {
            return dal.GetQuestionById(questionId);
        }

        public long AddQuestion(Model.ExamQuestion question)
        {
            question.CreateTime = DateTime.Now;
            question.Status = 1;
            question.QuestionText = StripHtml(question.QuestionContent);
            return dal.AddQuestion(question);
        }

        public bool UpdateQuestion(Model.ExamQuestion question)
        {
            question.UpdateTime = DateTime.Now;
            question.QuestionText = StripHtml(question.QuestionContent);
            return dal.UpdateQuestion(question);
        }

        public bool DeleteQuestion(long questionId)
        {
            return dal.DeleteQuestion(questionId);
        }

        /// <summary>
        /// 批量导入题目
        /// </summary>
        public int ImportQuestions(int bankId, List<Model.ExamQuestion> questions, string createBy)
        {
            int count = 0;
            foreach (var q in questions)
            {
                q.BankId = bankId;
                q.CreateBy = createBy;
                if (AddQuestion(q) > 0)
                    count++;
            }
            
            // 更新题库数量
            new ExamQuestionBank().UpdateQuestionCount(bankId);
            return count;
        }

        /// <summary>
        /// 随机抽取题目
        /// </summary>
        public List<Model.ExamQuestion> RandomPick(int bankId, int questionType, int difficulty, int count)
        {
            return dal.RandomPick(bankId, questionType, difficulty, count);
        }

        /// <summary>
        /// 根据Quiz题目ID获取Exam题目
        /// </summary>
        public Model.ExamQuestion GetQuestionByQuizId(int quizId)
        {
            return dal.GetQuestionByQuizId(quizId);
        }

        /// <summary>
        /// 批量删除题目
        /// </summary>
        public bool BatchDeleteQuestions(string questionIds)
        {
            return dal.BatchDeleteQuestions(questionIds);
        }

        /// <summary>
        /// 更新题目使用次数
        /// </summary>
        public bool IncrementUseCount(long questionId)
        {
            return dal.IncrementUseCount(questionId);
        }

        /// <summary>
        /// 更新题目正确率
        /// </summary>
        public bool UpdateCorrectRate(long questionId, decimal correctRate)
        {
            return dal.UpdateCorrectRate(questionId, correctRate);
        }

        /// <summary>
        /// 去除HTML标签
        /// </summary>
        private string StripHtml(string html)
        {
            if (string.IsNullOrEmpty(html)) return "";
            return System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
        }
    }

    /// <summary>
    /// 答卷业务逻辑类
    /// </summary>
    public class ExamAnswer
    {
        private readonly DAL.ExamAnswer dal = new DAL.ExamAnswer();

        /// <summary>
        /// 获取答卷列表
        /// </summary>
        public List<Model.ExamAnswer> GetAnswerList(int examId, int? classId = null, string keyword = null)
        {
            return dal.GetAnswerList(examId, classId, keyword);
        }

        public Model.ExamAnswer GetAnswerById(long answerId)
        {
            return dal.GetAnswerById(answerId);
        }

        public Model.ExamAnswer GetAnswerByExamAndStudent(int examId, string studentId)
        {
            return dal.GetAnswerByExamAndStudent(examId, studentId);
        }

        /// <summary>
        /// 开始答题
        /// </summary>
        public long StartAnswer(int examId, int paperId, string studentId, string studentName, int classId, string ip, string userAgent)
        {
            // 检查是否有未提交的答卷
            var existing = GetAnswerByExamAndStudent(examId, studentId);
            if (existing != null && existing.Status == 0)
            {
                return existing.AnswerId;
            }

            var answer = new Model.ExamAnswer
            {
                ExamId = examId,
                PaperId = paperId,
                StudentId = studentId,
                StudentName = studentName,
                ClassId = classId,
                StartTime = DateTime.Now,
                Status = 0,
                IpAddress = ip,
                UserAgent = userAgent
            };

            return dal.AddAnswer(answer);
        }

        /// <summary>
        /// 暂存答案
        /// </summary>
        public bool TempSaveAnswer(long answerId, string answers)
        {
            return dal.TempSaveAnswer(answerId, answers);
        }

        /// <summary>
        /// 提交答卷
        /// </summary>
        public SubmitResult SubmitAnswer(long answerId, string answers, string ip)
        {
            var result = new SubmitResult();
            var answer = GetAnswerById(answerId);
            if (answer == null)
            {
                result.Success = false;
                result.Message = "答卷不存在";
                return result;
            }

            if (answer.Status > 0)
            {
                result.Success = false;
                result.Message = "答卷已提交，不可重复提交";
                return result;
            }

            // 计算分数
            var scoreResult = CalculateScore(answer.ExamId, answer.PaperId, answers);
            
            answer.Answers = answers;
            answer.SubmitTime = DateTime.Now;
            answer.Duration = (int)(DateTime.Now - answer.StartTime).TotalSeconds;
            answer.TotalScore = scoreResult.TotalScore;
            answer.ObjectiveScore = scoreResult.ObjectiveScore;
            answer.ScoreDetails = scoreResult.ScoreDetailsJson;
            answer.Status = 1;
            answer.IpAddress = ip;

            if (dal.SubmitAnswer(answer))
            {
                result.Success = true;
                result.TotalScore = answer.TotalScore;
                result.Duration = answer.Duration;

                // 更新成绩统计
                UpdateResult(answer);
            }
            else
            {
                result.Success = false;
                result.Message = "提交失败";
            }

            return result;
        }

        /// <summary>
        /// 计算分数
        /// </summary>
        private ScoreResult CalculateScore(int examId, int paperId, string answers)
        {
            var result = new ScoreResult();
            var paperBll = new ExamPaper();
            var paper = paperBll.GetPaperById(paperId);
            var questions = paperBll.GetPaperQuestions(paperId);

            // 解析答案JSON
            var userAnswers = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<long, string>>(answers);
            var scoreDetails = new List<ScoreDetail>();
            decimal totalScore = 0;
            decimal objectiveScore = 0;

            foreach (var pq in questions)
            {
                var detail = new ScoreDetail
                {
                    QuestionId = pq.QuestionId,
                    MaxScore = pq.Score
                };

                if (pq.Question != null)
                {
                    detail.CorrectAnswer = pq.Question.Answer;
                    
                    // 获取用户答案
                    if (userAnswers.ContainsKey(pq.QuestionId))
                    {
                        detail.UserAnswer = userAnswers[pq.QuestionId];
                    }

                    // 自动评分（客观题）
                    if (IsAutoScoreType(pq.Question.QuestionType))
                    {
                        detail.IsCorrect = CheckAnswer(detail.UserAnswer, detail.CorrectAnswer, pq.Question.QuestionType);
                        detail.Score = detail.IsCorrect ? pq.Score : 0;
                        objectiveScore += detail.Score;
                    }
                    else
                    {
                        // 主观题暂不评分
                        detail.Score = 0;
                    }
                }

                totalScore += detail.Score;
                scoreDetails.Add(detail);
            }

            result.TotalScore = totalScore;
            result.ObjectiveScore = objectiveScore;
            result.ScoreDetails = scoreDetails;
            result.ScoreDetailsJson = Newtonsoft.Json.JsonConvert.SerializeObject(scoreDetails);

            return result;
        }

        /// <summary>
        /// 判断题型是否自动评分
        /// </summary>
        private bool IsAutoScoreType(int questionType)
        {
            // 1单选 2多选 3判断 4填空 6连线 7分类 可自动评分
            // 5简答 8组合 需人工评分
            return questionType == 1 || questionType == 2 || questionType == 3 || 
                   questionType == 4 || questionType == 6 || questionType == 7;
        }

        /// <summary>
        /// 检查答案是否正确
        /// </summary>
        private bool CheckAnswer(string userAnswer, string correctAnswer, int questionType)
        {
            if (string.IsNullOrEmpty(userAnswer) || string.IsNullOrEmpty(correctAnswer))
                return false;

            // 标准化答案格式
            userAnswer = userAnswer.Trim().ToUpper();
            correctAnswer = correctAnswer.Trim().ToUpper();

            switch (questionType)
            {
                case 1: // 单选
                case 3: // 判断
                    return userAnswer == correctAnswer;

                case 2: // 多选
                    var userOpts = userAnswer.Split(',').OrderBy(x => x).ToArray();
                    var correctOpts = correctAnswer.Split(',').OrderBy(x => x).ToArray();
                    return userOpts.SequenceEqual(correctOpts);

                case 4: // 填空
                    var userFills = userAnswer.Split('|');
                    var correctFills = correctAnswer.Split('|');
                    if (userFills.Length != correctFills.Length) return false;
                    for (int i = 0; i < userFills.Length; i++)
                    {
                        if (userFills[i].Trim() != correctFills[i].Trim())
                            return false;
                    }
                    return true;

                default:
                    return userAnswer == correctAnswer;
            }
        }

        /// <summary>
        /// 更新成绩表
        /// </summary>
        private void UpdateResult(Model.ExamAnswer answer)
        {
            var result = new Model.ExamResult
            {
                ExamId = answer.ExamId,
                AnswerId = answer.AnswerId,
                StudentId = answer.StudentId,
                StudentName = answer.StudentName,
                ClassId = answer.ClassId,
                TotalScore = answer.TotalScore,
                ObjectiveScore = answer.ObjectiveScore,
                SubjectiveScore = answer.SubjectiveScore ?? 0,
                Duration = answer.Duration,
                SubmitTime = answer.SubmitTime,
                Status = 1
            };

            new DAL.ExamResult().AddOrUpdate(result);
        }

        public int GetSubmitCount(int examId, string studentId)
        {
            return dal.GetSubmitCount(examId, studentId);
        }
    }

    /// <summary>
    /// 提交结果
    /// </summary>
    public class SubmitResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public decimal TotalScore { get; set; }
        public int Duration { get; set; }
    }

    /// <summary>
    /// 评分结果
    /// </summary>
    public class ScoreResult
    {
        public decimal TotalScore { get; set; }
        public decimal ObjectiveScore { get; set; }
        public List<Model.ScoreDetail> ScoreDetails { get; set; }
        public string ScoreDetailsJson { get; set; }
    }

    /// <summary>
    /// 随机组卷业务逻辑
    /// </summary>
    public class ExamRandomGenerator
    {
        private readonly ExamQuestion questionBll = new ExamQuestion();

        /// <summary>
        /// 生成随机试卷
        /// </summary>
        public List<Model.ExamQuestion> GenerateRandomPaper(string randomConfigJson)
        {
            var config = Newtonsoft.Json.JsonConvert.DeserializeObject<Model.RandomConfig>(randomConfigJson);
            if (config == null || !config.IsRandom) return new List<Model.ExamQuestion>();

            var questions = new List<Model.ExamQuestion>();
            var random = new Random();

            foreach (var rule in config.Rules)
            {
                var picked = questionBll.RandomPick(
                    rule.BankId ?? 0,
                    rule.QuestionType,
                    rule.Difficulty,
                    rule.Count
                );

                foreach (var q in picked)
                {
                    q.Score = rule.ScorePerQuestion;
                    questions.Add(q);
                }
            }

            return questions;
        }
    }
}
