using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using LearnSite.Model;
using LearnSite.DBUtility;

namespace LearnSite.DAL
{
    public class Exam
    {
        public List<Model.Exam> GetExamList(string teacherId, int status = -1)
        {
            var list = new List<Model.Exam>();
            var sql = "SELECT e.*, p.PaperName FROM Exam e LEFT JOIN ExamPaper p ON e.PaperId = p.PaperId WHERE 1=1";
            var parameters = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(teacherId)) { sql += " AND e.CreateBy = @CreateBy"; parameters.Add(new SqlParameter("@CreateBy", teacherId)); }
            if (status >= 0) { sql += " AND e.Status = @Status"; parameters.Add(new SqlParameter("@Status", status)); }
            sql += " ORDER BY e.CreateTime DESC";
            using (var reader = SqlHelper.ExecuteReader(CommandType.Text, sql, parameters.ToArray()))
                while (reader.Read()) list.Add(ReaderToExam(reader));
            return list;
        }

        public Model.Exam GetExamById(int examId)
        {
            var sql = "SELECT e.*, p.PaperName FROM Exam e LEFT JOIN ExamPaper p ON e.PaperId = p.PaperId WHERE e.ExamId = @ExamId";
            using (var reader = SqlHelper.ExecuteReader(CommandType.Text, sql, new SqlParameter("@ExamId", examId)))
                if (reader.Read()) return ReaderToExam(reader);
            return null;
        }

        public int AddExam(Model.Exam exam)
        {
            var sql = @"INSERT INTO Exam (ExamCode, ExamName, PaperId, ExamType, TimeMode, StartTime, EndTime, ValidDays, Duration, 
                        LateMinutes, ShowAnswer, ShowScore, ShowRank, Password, ParticipantType, Participants, 
                        AntiCheat, Status, CreateBy, CreateTime) 
                        VALUES (@ExamCode, @ExamName, @PaperId, @ExamType, @TimeMode, @StartTime, @EndTime, @ValidDays, @Duration, 
                        @LateMinutes, @ShowAnswer, @ShowScore, @ShowRank, @Password, @ParticipantType, @Participants, 
                        @AntiCheat, @Status, @CreateBy, @CreateTime); SELECT SCOPE_IDENTITY()";
            var parameters = new SqlParameter[] {
                new SqlParameter("@ExamCode", exam.ExamCode ?? ""), 
                new SqlParameter("@ExamName", exam.ExamName ?? ""),
                new SqlParameter("@PaperId", exam.PaperId), 
                new SqlParameter("@ExamType", exam.ExamType),
                new SqlParameter("@TimeMode", exam.TimeMode),
                new SqlParameter("@StartTime", exam.StartTime), 
                new SqlParameter("@EndTime", exam.EndTime),
                new SqlParameter("@ValidDays", exam.ValidDays),
                new SqlParameter("@Duration", exam.Duration), 
                new SqlParameter("@LateMinutes", exam.LateMinutes),
                new SqlParameter("@ShowAnswer", exam.ShowAnswer),
                new SqlParameter("@ShowScore", exam.ShowScore),
                new SqlParameter("@ShowRank", exam.ShowRank),
                new SqlParameter("@Password", exam.Password ?? ""),
                new SqlParameter("@ParticipantType", exam.ParticipantType),
                new SqlParameter("@Participants", exam.Participants ?? ""),
                new SqlParameter("@AntiCheat", exam.AntiCheat ?? ""),
                new SqlParameter("@Status", exam.Status),
                new SqlParameter("@CreateBy", exam.CreateBy ?? ""), 
                new SqlParameter("@CreateTime", exam.CreateTime)
            };
            return Convert.ToInt32(SqlHelper.ExecuteScalar(CommandType.Text, sql, parameters));
        }

        public bool DeleteExam(int examId)
        {
            var sql = "DELETE FROM Exam WHERE ExamId = @ExamId";
            return SqlHelper.ExecuteNonQuery(CommandType.Text, sql, new SqlParameter("@ExamId", examId)) > 0;
        }

        public List<Model.Exam> GetStudentExamList(string studentId, int classId)
        {
            var list = new List<Model.Exam>();
            // 简化查询：获取所有已发布的考试（Status >= 1）
            // 前端根据时间判断是"进行中"、"即将开始"还是"已结束"
            var sql = @"SELECT e.*, p.PaperName, p.TotalScore FROM Exam e 
                        LEFT JOIN ExamPaper p ON e.PaperId = p.PaperId 
                        WHERE e.Status >= 1 
                        ORDER BY e.StartTime DESC";
            using (var reader = SqlHelper.ExecuteReader(CommandType.Text, sql, null))
                while (reader.Read()) list.Add(ReaderToExam(reader));
            return list;
        }

        public Model.Exam GetExamByCode(string examCode)
        {
            var sql = "SELECT e.*, p.PaperName FROM Exam e LEFT JOIN ExamPaper p ON e.PaperId = p.PaperId WHERE e.ExamCode = @ExamCode";
            using (var reader = SqlHelper.ExecuteReader(CommandType.Text, sql, new SqlParameter("@ExamCode", examCode)))
                if (reader.Read()) return ReaderToExam(reader);
            return null;
        }

        public bool UpdateExam(Model.Exam exam)
        {
            var sql = @"UPDATE Exam SET ExamName = @ExamName, PaperId = @PaperId, ExamType = @ExamType, 
                        TimeMode = @TimeMode, StartTime = @StartTime, EndTime = @EndTime, ValidDays = @ValidDays, Duration = @Duration, 
                        LateMinutes = @LateMinutes, ShowAnswer = @ShowAnswer, ShowScore = @ShowScore,
                        ShowRank = @ShowRank, Password = @Password, ParticipantType = @ParticipantType,
                        Participants = @Participants, AntiCheat = @AntiCheat, Status = @Status,
                        UpdateTime = @UpdateTime, UpdateBy = @UpdateBy WHERE ExamId = @ExamId";
            return SqlHelper.ExecuteNonQuery(CommandType.Text, sql, new SqlParameter[] {
                new SqlParameter("@ExamId", exam.ExamId), 
                new SqlParameter("@ExamName", exam.ExamName ?? ""),
                new SqlParameter("@PaperId", exam.PaperId), 
                new SqlParameter("@ExamType", exam.ExamType),
                new SqlParameter("@TimeMode", exam.TimeMode),
                new SqlParameter("@StartTime", exam.StartTime), 
                new SqlParameter("@EndTime", exam.EndTime),
                new SqlParameter("@ValidDays", exam.ValidDays),
                new SqlParameter("@Duration", exam.Duration), 
                new SqlParameter("@LateMinutes", exam.LateMinutes),
                new SqlParameter("@ShowAnswer", exam.ShowAnswer),
                new SqlParameter("@ShowScore", exam.ShowScore),
                new SqlParameter("@ShowRank", exam.ShowRank),
                new SqlParameter("@Password", exam.Password ?? ""),
                new SqlParameter("@ParticipantType", exam.ParticipantType),
                new SqlParameter("@Participants", exam.Participants ?? ""),
                new SqlParameter("@AntiCheat", exam.AntiCheat ?? ""),
                new SqlParameter("@Status", exam.Status),
                new SqlParameter("@UpdateTime", exam.UpdateTime ?? DateTime.Now), 
                new SqlParameter("@UpdateBy", exam.UpdateBy ?? "")
            }) > 0;
        }

        public Model.ExamStatistics GetExamStatistics(int examId)
        {
            var stats = new Model.ExamStatistics();
            var sql = @"SELECT COUNT(*) as Total, AVG(TotalScore) as AvgScore, MAX(TotalScore) as MaxScore, MIN(TotalScore) as MinScore FROM ExamResult WHERE ExamId = @ExamId";
            using (var reader = SqlHelper.ExecuteReader(CommandType.Text, sql, new SqlParameter("@ExamId", examId)))
            {
                if (reader.Read())
                {
                    stats.TotalParticipants = Convert.ToInt32(reader["Total"]);
                    stats.SubmittedCount = stats.TotalParticipants;
                    stats.AverageScore = reader["AvgScore"] != DBNull.Value ? Convert.ToDecimal(reader["AvgScore"]) : 0;
                    stats.MaxScore = reader["MaxScore"] != DBNull.Value ? Convert.ToDecimal(reader["MaxScore"]) : 0;
                    stats.MinScore = reader["MinScore"] != DBNull.Value ? Convert.ToDecimal(reader["MinScore"]) : 0;
                }
            }
            return stats;
        }

        public void UpdateExamStatus()
        {
            var now = DateTime.Now;
            SqlHelper.ExecuteNonQuery(CommandType.Text, "UPDATE Exam SET Status = 2 WHERE Status = 1 AND EndTime < @Now", 
                new SqlParameter("@Now", now));
        }

        private Model.Exam ReaderToExam(IDataReader reader)
        {
            var exam = new Model.Exam {
                ExamId = reader["ExamId"] != DBNull.Value ? Convert.ToInt32(reader["ExamId"]) : 0,
                ExamCode = reader["ExamCode"] != DBNull.Value ? reader["ExamCode"].ToString() : null,
                ExamName = reader["ExamName"] != DBNull.Value ? reader["ExamName"].ToString() : null,
                PaperId = reader["PaperId"] != DBNull.Value ? Convert.ToInt32(reader["PaperId"]) : 0,
                ExamType = reader["ExamType"] != DBNull.Value ? Convert.ToInt32(reader["ExamType"]) : 1,
                TimeMode = reader["TimeMode"] != DBNull.Value ? Convert.ToInt32(reader["TimeMode"]) : 1,
                StartTime = reader["StartTime"] != DBNull.Value ? Convert.ToDateTime(reader["StartTime"]) : DateTime.MinValue,
                EndTime = reader["EndTime"] != DBNull.Value ? Convert.ToDateTime(reader["EndTime"]) : DateTime.MinValue,
                ValidDays = reader["ValidDays"] != DBNull.Value ? Convert.ToInt32(reader["ValidDays"]) : 0,
                Duration = reader["Duration"] != DBNull.Value ? Convert.ToInt32(reader["Duration"]) : 60,
                LateMinutes = reader["LateMinutes"] != DBNull.Value ? Convert.ToInt32(reader["LateMinutes"]) : 0,
                Status = reader["Status"] != DBNull.Value ? Convert.ToInt32(reader["Status"]) : 0,
                ParticipantType = reader["ParticipantType"] != DBNull.Value ? Convert.ToInt32(reader["ParticipantType"]) : 1,
                Participants = reader["Participants"] != DBNull.Value ? reader["Participants"].ToString() : null,
                CreateBy = reader["CreateBy"] != DBNull.Value ? reader["CreateBy"].ToString() : null,
                PaperName = reader["PaperName"] != DBNull.Value ? reader["PaperName"].ToString() : null
            };
            
            // 尝试读取 TotalScore
            try {
                int totalScoreIdx = reader.GetOrdinal("TotalScore");
                if (!reader.IsDBNull(totalScoreIdx))
                    exam.TotalScore = reader.GetDecimal(totalScoreIdx);
            } catch { }
            
            return exam;
        }
    }

    public class ExamPaper {
        public List<Model.ExamPaper> GetPaperList(string teacherId, int status = -1) {
            var list = new List<Model.ExamPaper>();
            var sql = "SELECT * FROM ExamPaper WHERE 1=1";
            var parameters = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(teacherId)) { sql += " AND CreateBy = @CreateBy"; parameters.Add(new SqlParameter("@CreateBy", teacherId)); }
            if (status >= 0) { sql += " AND Status = @Status"; parameters.Add(new SqlParameter("@Status", status)); }
            sql += " ORDER BY CreateTime DESC";
            using (var reader = SqlHelper.ExecuteReader(CommandType.Text, sql, parameters.ToArray()))
                while (reader.Read()) list.Add(ReaderToPaper(reader));
            return list;
        }

        public List<Model.ExamPaper> GetPaperListByCourse(int courseId) {
            var list = new List<Model.ExamPaper>();
            var sql = "SELECT * FROM ExamPaper WHERE CourseId = @CourseId AND Status >= 1";
            using (var reader = SqlHelper.ExecuteReader(CommandType.Text, sql, new SqlParameter("@CourseId", courseId)))
                while (reader.Read()) list.Add(ReaderToPaper(reader));
            return list;
        }

        public List<Model.ExamPaper> GetPaperListByWhere(string whereClause) {
            var list = new List<Model.ExamPaper>();
            var sql = "SELECT * FROM ExamPaper WHERE " + whereClause;
            using (var reader = SqlHelper.ExecuteReader(CommandType.Text, sql))
                while (reader.Read()) list.Add(ReaderToPaper(reader));
            return list;
        }

        public Model.ExamPaper GetPaperById(int paperId) {
            var sql = "SELECT * FROM ExamPaper WHERE PaperId = @PaperId";
            using (var reader = SqlHelper.ExecuteReader(CommandType.Text, sql, new SqlParameter("@PaperId", paperId)))
                if (reader.Read()) return ReaderToPaper(reader);
            return null;
        }
        public int AddPaper(Model.ExamPaper p) {
            var sql = "INSERT INTO ExamPaper (PaperCode, PaperName, PaperType, TotalScore, PassScore, QuestionCount, Duration, Description, Status, CreateBy, CreateTime) VALUES (@PaperCode, @PaperName, @PaperType, @TotalScore, @PassScore, @QuestionCount, @Duration, @Description, @Status, @CreateBy, @CreateTime); SELECT SCOPE_IDENTITY()";
            return Convert.ToInt32(SqlHelper.ExecuteScalar(CommandType.Text, sql, new SqlParameter[] {
                new SqlParameter("@PaperCode", p.PaperCode ?? ""), 
                new SqlParameter("@PaperName", p.PaperName ?? ""),
                new SqlParameter("@PaperType", p.PaperType),
                new SqlParameter("@TotalScore", p.TotalScore), 
                new SqlParameter("@PassScore", p.PassScore),
                new SqlParameter("@QuestionCount", p.QuestionCount),
                new SqlParameter("@Duration", p.Duration), 
                new SqlParameter("@Description", p.Description ?? ""),
                new SqlParameter("@Status", p.Status),
                new SqlParameter("@CreateBy", p.CreateBy ?? ""), 
                new SqlParameter("@CreateTime", p.CreateTime)
            }));
        }
        public bool UpdatePaper(Model.ExamPaper p) {
            var sql = "UPDATE ExamPaper SET PaperName = @PaperName, TotalScore = @TotalScore, QuestionCount = @QuestionCount, Duration = @Duration, UpdateTime = @UpdateTime WHERE PaperId = @PaperId";
            return SqlHelper.ExecuteNonQuery(CommandType.Text, sql, new SqlParameter[] {
                new SqlParameter("@PaperId", p.PaperId), 
                new SqlParameter("@PaperName", p.PaperName ?? ""),
                new SqlParameter("@TotalScore", p.TotalScore), 
                new SqlParameter("@QuestionCount", p.QuestionCount),
                new SqlParameter("@Duration", p.Duration),
                new SqlParameter("@UpdateTime", DateTime.Now)
            }) > 0;
        }
        public bool DeletePaper(int paperId) {
            return SqlHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM ExamPaper WHERE PaperId = @PaperId", new SqlParameter("@PaperId", paperId)) > 0;
        }
        public List<Model.ExamPaperQuestion> GetPaperQuestions(int paperId) {
            var list = new List<Model.ExamPaperQuestion>();
            var sql = @"SELECT pq.*, q.QuestionType, q.QuestionContent, q.QuestionText, q.Options, q.Answer, q.Analysis, q.Difficulty
                        FROM ExamPaperQuestion pq
                        LEFT JOIN ExamQuestion q ON pq.QuestionId = q.QuestionId
                        WHERE pq.PaperId = @PaperId
                        ORDER BY pq.SortOrder, pq.Id";
            using (var reader = SqlHelper.ExecuteReader(CommandType.Text, sql, new SqlParameter("@PaperId", paperId)))
            {
                while (reader.Read())
                {
                    var pq = new Model.ExamPaperQuestion
                    {
                        Id = reader["Id"] != DBNull.Value ? Convert.ToInt64(reader["Id"]) : 0,
                        PaperId = reader["PaperId"] != DBNull.Value ? Convert.ToInt32(reader["PaperId"]) : 0,
                        QuestionId = reader["QuestionId"] != DBNull.Value ? Convert.ToInt64(reader["QuestionId"]) : 0,
                        SectionName = reader["SectionName"] != DBNull.Value ? reader["SectionName"].ToString() : null,
                        Score = reader["Score"] != DBNull.Value ? Convert.ToDecimal(reader["Score"]) : 1,
                        SortOrder = reader["SortOrder"] != DBNull.Value ? Convert.ToInt32(reader["SortOrder"]) : 0,
                        Question = new Model.ExamQuestion
                        {
                            QuestionId = reader["QuestionId"] != DBNull.Value ? Convert.ToInt64(reader["QuestionId"]) : 0,
                            QuestionType = reader["QuestionType"] != DBNull.Value ? Convert.ToInt32(reader["QuestionType"]) : 1,
                            QuestionContent = reader["QuestionContent"] != DBNull.Value ? reader["QuestionContent"].ToString() : null,
                            QuestionText = reader["QuestionText"] != DBNull.Value ? reader["QuestionText"].ToString() : null,
                            Options = reader["Options"] != DBNull.Value ? reader["Options"].ToString() : null,
                            Answer = reader["Answer"] != DBNull.Value ? reader["Answer"].ToString() : null,
                            Analysis = reader["Analysis"] != DBNull.Value ? reader["Analysis"].ToString() : null,
                            Difficulty = reader["Difficulty"] != DBNull.Value ? Convert.ToInt32(reader["Difficulty"]) : 1,
                            Score = reader["Score"] != DBNull.Value ? Convert.ToDecimal(reader["Score"]) : 1
                        }
                    };
                    list.Add(pq);
                }
            }
            return list;
        }

        public int AddPaperQuestion(Model.ExamPaperQuestion paperQuestion) {
            var sql = "INSERT INTO ExamPaperQuestion (PaperId, QuestionId, SectionName, Score, SortOrder) VALUES (@PaperId, @QuestionId, @SectionName, @Score, @SortOrder); SELECT SCOPE_IDENTITY()";
            return Convert.ToInt32(SqlHelper.ExecuteScalar(CommandType.Text, sql, new SqlParameter[] {
                new SqlParameter("@PaperId", paperQuestion.PaperId),
                new SqlParameter("@QuestionId", paperQuestion.QuestionId),
                new SqlParameter("@SectionName", paperQuestion.SectionName ?? ""),
                new SqlParameter("@Score", paperQuestion.Score),
                new SqlParameter("@SortOrder", paperQuestion.SortOrder)
            }));
        }

        public int BatchAddPaperQuestions(int paperId, List<Model.ExamPaperQuestion> paperQuestions) {
            int count = 0;
            foreach (var pq in paperQuestions)
            {
                if (AddPaperQuestion(pq) > 0)
                {
                    count++;
                }
            }
            return count;
        }

        public bool DeletePaperQuestion(long id) {
            var sql = "DELETE FROM ExamPaperQuestion WHERE Id = @Id";
            return SqlHelper.ExecuteNonQuery(CommandType.Text, sql, new SqlParameter("@Id", id)) > 0;
        }

        public bool SavePaperQuestions(int paperId, List<Model.PaperSection> sections) {
            // 先删除旧的关联
            SqlHelper.ExecuteNonQuery(CommandType.Text, "DELETE FROM ExamPaperQuestion WHERE PaperId = @PaperId", new SqlParameter("@PaperId", paperId));

            int sortOrder = 0;
            foreach (var section in sections)
            {
                foreach (var q in section.Questions)
                {
                    var sql = @"INSERT INTO ExamPaperQuestion (PaperId, QuestionId, SectionName, Score, SortOrder)
                                VALUES (@PaperId, @QuestionId, @SectionName, @Score, @SortOrder)";
                    SqlHelper.ExecuteNonQuery(CommandType.Text, sql, new SqlParameter[] {
                        new SqlParameter("@PaperId", paperId),
                        new SqlParameter("@QuestionId", q.QuestionId),
                        new SqlParameter("@SectionName", section.SectionName ?? ""),
                        new SqlParameter("@Score", q.Score),
                        new SqlParameter("@SortOrder", sortOrder++)
                    });
                }
            }
            return true;
        }
        private Model.ExamPaper ReaderToPaper(IDataReader reader) {
            return new Model.ExamPaper {
                PaperId = Convert.ToInt32(reader["PaperId"]),
                PaperCode = reader["PaperCode"] != DBNull.Value ? reader["PaperCode"].ToString() : null,
                PaperName = reader["PaperName"] != DBNull.Value ? reader["PaperName"].ToString() : null,
                PaperType = reader["PaperType"] != DBNull.Value ? Convert.ToInt32(reader["PaperType"]) : 1,
                TotalScore = reader["TotalScore"] != DBNull.Value ? Convert.ToDecimal(reader["TotalScore"]) : 100,
                PassScore = reader["PassScore"] != DBNull.Value ? Convert.ToDecimal(reader["PassScore"]) : 60,
                QuestionCount = reader["QuestionCount"] != DBNull.Value ? Convert.ToInt32(reader["QuestionCount"]) : 0,
                Duration = reader["Duration"] != DBNull.Value ? Convert.ToInt32(reader["Duration"]) : 60,
                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                Status = reader["Status"] != DBNull.Value ? Convert.ToInt32(reader["Status"]) : 0
            };
        }
    }

    public class ExamQuestionBank {
        public List<Model.ExamQuestionBank> GetBankList(string teacherId = null) {
            var list = new List<Model.ExamQuestionBank>();
            var sql = "SELECT * FROM ExamQuestionBank WHERE Status = 1";
            var parameters = new List<SqlParameter>();
            if (!string.IsNullOrEmpty(teacherId)) { sql += " AND CreateBy = @CreateBy"; parameters.Add(new SqlParameter("@CreateBy", teacherId)); }
            sql += " ORDER BY CreateTime DESC";
            using (var reader = SqlHelper.ExecuteReader(CommandType.Text, sql, parameters.ToArray()))
                while (reader.Read()) list.Add(new Model.ExamQuestionBank { BankId = Convert.ToInt32(reader["BankId"]), BankName = reader["BankName"] != DBNull.Value ? reader["BankName"].ToString() : null, QuestionCount = reader["QuestionCount"] != DBNull.Value ? Convert.ToInt32(reader["QuestionCount"]) : 0 });
            return list;
        }
        public Model.ExamQuestionBank GetBankById(int bankId) {
            var sql = "SELECT * FROM ExamQuestionBank WHERE BankId = @BankId";
            using (var reader = SqlHelper.ExecuteReader(CommandType.Text, sql, new SqlParameter("@BankId", bankId)))
                if (reader.Read())
                {
                    return new Model.ExamQuestionBank
                    {
                        BankId = Convert.ToInt32(reader["BankId"]),
                        BankName = reader["BankName"] != DBNull.Value ? reader["BankName"].ToString() : null,
                        BankCode = reader["BankCode"] != DBNull.Value ? reader["BankCode"].ToString() : null,
                        SubjectId = reader["SubjectId"] != DBNull.Value ? (int?)Convert.ToInt32(reader["SubjectId"]) : null,
                        GradeId = reader["GradeId"] != DBNull.Value ? (int?)Convert.ToInt32(reader["GradeId"]) : null,
                        Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                        QuestionCount = reader["QuestionCount"] != DBNull.Value ? Convert.ToInt32(reader["QuestionCount"]) : 0,
                        Status = reader["Status"] != DBNull.Value ? Convert.ToInt32(reader["Status"]) : 1,
                        CreateBy = reader["CreateBy"] != DBNull.Value ? reader["CreateBy"].ToString() : null,
                        CreateTime = reader["CreateTime"] != DBNull.Value ? Convert.ToDateTime(reader["CreateTime"]) : DateTime.Now,
                        UpdateBy = reader["UpdateBy"] != DBNull.Value ? reader["UpdateBy"].ToString() : null,
                        UpdateTime = reader["UpdateTime"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["UpdateTime"]) : null
                    };
                }
            return null;
        }
        public int AddBank(Model.ExamQuestionBank bank) {
            var sql = "INSERT INTO ExamQuestionBank (BankName, BankCode, Status, CreateBy, CreateTime) VALUES (@BankName, @BankCode, @Status, @CreateBy, @CreateTime); SELECT SCOPE_IDENTITY()";
            return Convert.ToInt32(SqlHelper.ExecuteScalar(CommandType.Text, sql, new SqlParameter[] {
                new SqlParameter("@BankName", bank.BankName ?? ""),
                new SqlParameter("@BankCode", bank.BankCode ?? ""),
                new SqlParameter("@Status", bank.Status),
                new SqlParameter("@CreateBy", bank.CreateBy ?? ""),
                new SqlParameter("@CreateTime", bank.CreateTime)
            }));
        }
        public bool UpdateBank(Model.ExamQuestionBank bank) {
            return SqlHelper.ExecuteNonQuery(CommandType.Text, "UPDATE ExamQuestionBank SET BankName = @BankName, BankCode = @BankCode, Description = @Description, UpdateBy = @UpdateBy, UpdateTime = @UpdateTime WHERE BankId = @BankId",
                new SqlParameter[] {
                    new SqlParameter("@BankId", bank.BankId),
                    new SqlParameter("@BankName", bank.BankName ?? ""),
                    new SqlParameter("@BankCode", bank.BankCode ?? ""),
                    new SqlParameter("@Description", bank.Description ?? ""),
                    new SqlParameter("@UpdateBy", bank.UpdateBy ?? ""),
                    new SqlParameter("@UpdateTime", DateTime.Now)
                }) > 0;
        }
        public bool DeleteBank(int bankId) {
            return SqlHelper.ExecuteNonQuery(CommandType.Text, "UPDATE ExamQuestionBank SET Status = 0 WHERE BankId = @BankId", new SqlParameter("@BankId", bankId)) > 0;
        }
        public void UpdateQuestionCount(int bankId) {
            SqlHelper.ExecuteNonQuery(CommandType.Text, "UPDATE ExamQuestionBank SET QuestionCount = (SELECT COUNT(*) FROM ExamQuestion WHERE BankId = @BankId) WHERE BankId = @BankId", new SqlParameter("@BankId", bankId));
        }
    }

    public class ExamQuestion {
        public Tuple<List<Model.ExamQuestion>, int> GetQuestionList(int bankId, int pageIndex, int pageSize, int? questionType = null, int? difficulty = null, string keyword = null, int? gradeId = null, int? courseId = null) {
            var list = new List<Model.ExamQuestion>();
            var sql = "SELECT * FROM ExamQuestion WHERE Status = 1";
            var countSql = "SELECT COUNT(*) FROM ExamQuestion WHERE Status = 1";
            var parameters = new List<SqlParameter>();
            
            // BankId=0 表示获取所有题库的题目
            if (bankId > 0) {
                sql += " AND BankId = @BankId";
                countSql += " AND BankId = @BankId";
                parameters.Add(new SqlParameter("@BankId", bankId));
            }
            
            if (questionType.HasValue) { sql += " AND QuestionType = @QuestionType"; countSql += " AND QuestionType = @QuestionType"; parameters.Add(new SqlParameter("@QuestionType", questionType.Value)); }
            if (difficulty.HasValue) { sql += " AND Difficulty = @Difficulty"; countSql += " AND Difficulty = @Difficulty"; parameters.Add(new SqlParameter("@Difficulty", difficulty.Value)); }
            if (gradeId.HasValue) { sql += " AND GradeId = @GradeId"; countSql += " AND GradeId = @GradeId"; parameters.Add(new SqlParameter("@GradeId", gradeId.Value)); }
            if (courseId.HasValue) { sql += " AND CourseId = @CourseId"; countSql += " AND CourseId = @CourseId"; parameters.Add(new SqlParameter("@CourseId", courseId.Value)); }
            if (!string.IsNullOrEmpty(keyword)) { sql += " AND QuestionText LIKE @Keyword"; countSql += " AND QuestionText LIKE @Keyword"; parameters.Add(new SqlParameter("@Keyword", "%" + keyword + "%")); }
            var total = Convert.ToInt32(SqlHelper.ExecuteScalar(CommandType.Text, countSql, parameters.ToArray()));
            sql += " ORDER BY CreateTime DESC OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
            parameters.Add(new SqlParameter("@Offset", (pageIndex - 1) * pageSize));
            parameters.Add(new SqlParameter("@PageSize", pageSize));
            using (var reader = SqlHelper.ExecuteReader(CommandType.Text, sql, parameters.ToArray()))
                while (reader.Read()) list.Add(ReaderToQuestion(reader));
            return Tuple.Create(list, total);
        }
        public Model.ExamQuestion GetQuestionById(long questionId) {
            var sql = "SELECT * FROM ExamQuestion WHERE QuestionId = @QuestionId";
            using (var reader = SqlHelper.ExecuteReader(CommandType.Text, sql, new SqlParameter("@QuestionId", questionId)))
                if (reader.Read()) return ReaderToQuestion(reader);
            return null;
        }
        public long AddQuestion(Model.ExamQuestion q) {
            try {
                var sql = "INSERT INTO ExamQuestion (BankId, GradeId, CourseId, QuestionType, QuestionContent, QuestionText, Options, Answer, Analysis, Score, Difficulty, KnowledgePoint, Tags, QuestionConfig, Status, CreateBy, CreateTime) VALUES (@BankId, @GradeId, @CourseId, @QuestionType, @QuestionContent, @QuestionText, @Options, @Answer, @Analysis, @Score, @Difficulty, @KnowledgePoint, @Tags, @QuestionConfig, @Status, @CreateBy, @CreateTime); SELECT SCOPE_IDENTITY()";
                return Convert.ToInt64(SqlHelper.ExecuteScalar(CommandType.Text, sql, new SqlParameter[] {
                    new SqlParameter("@BankId", q.BankId), 
                    new SqlParameter("@GradeId", q.GradeId ?? (object)DBNull.Value),
                    new SqlParameter("@CourseId", q.CourseId ?? (object)DBNull.Value),
                    new SqlParameter("@QuestionType", q.QuestionType),
                    new SqlParameter("@QuestionContent", q.QuestionContent ?? ""), new SqlParameter("@QuestionText", q.QuestionText ?? ""),
                    new SqlParameter("@Options", q.Options ?? ""), new SqlParameter("@Answer", q.Answer ?? ""),
                    new SqlParameter("@Analysis", q.Analysis ?? ""),
                    new SqlParameter("@Score", q.Score), new SqlParameter("@Difficulty", q.Difficulty),
                    new SqlParameter("@KnowledgePoint", q.KnowledgePoint ?? ""),
                    new SqlParameter("@Tags", q.Tags ?? ""),
                    new SqlParameter("@QuestionConfig", q.QuestionConfig ?? ""),
                    new SqlParameter("@Status", q.Status), new SqlParameter("@CreateBy", q.CreateBy ?? ""),
                    new SqlParameter("@CreateTime", q.CreateTime)
                }));
            }
            catch (Exception ex) {
                System.Diagnostics.Trace.WriteLine("AddQuestion错误: " + ex.Message);
                // 如果 QuestionConfig 列不存在，尝试不包含该列的插入
                if (ex.Message.Contains("QuestionConfig") || ex.Message.Contains("column") || ex.Message.Contains("无效的列名")) {
                    var sql2 = "INSERT INTO ExamQuestion (BankId, GradeId, CourseId, QuestionType, QuestionContent, QuestionText, Options, Answer, Analysis, Score, Difficulty, KnowledgePoint, Tags, Status, CreateBy, CreateTime) VALUES (@BankId, @GradeId, @CourseId, @QuestionType, @QuestionContent, @QuestionText, @Options, @Answer, @Analysis, @Score, @Difficulty, @KnowledgePoint, @Tags, @Status, @CreateBy, @CreateTime); SELECT SCOPE_IDENTITY()";
                    return Convert.ToInt64(SqlHelper.ExecuteScalar(CommandType.Text, sql2, new SqlParameter[] {
                        new SqlParameter("@BankId", q.BankId), 
                        new SqlParameter("@GradeId", q.GradeId ?? (object)DBNull.Value),
                        new SqlParameter("@CourseId", q.CourseId ?? (object)DBNull.Value),
                        new SqlParameter("@QuestionType", q.QuestionType),
                        new SqlParameter("@QuestionContent", q.QuestionContent ?? ""), new SqlParameter("@QuestionText", q.QuestionText ?? ""),
                        new SqlParameter("@Options", q.Options ?? ""), new SqlParameter("@Answer", q.Answer ?? ""),
                        new SqlParameter("@Analysis", q.Analysis ?? ""),
                        new SqlParameter("@Score", q.Score), new SqlParameter("@Difficulty", q.Difficulty),
                        new SqlParameter("@KnowledgePoint", q.KnowledgePoint ?? ""),
                        new SqlParameter("@Tags", q.Tags ?? ""),
                        new SqlParameter("@Status", q.Status), new SqlParameter("@CreateBy", q.CreateBy ?? ""),
                        new SqlParameter("@CreateTime", q.CreateTime)
                    }));
                }
                throw;
            }
        }
        public bool UpdateQuestion(Model.ExamQuestion q) {
            return SqlHelper.ExecuteNonQuery(CommandType.Text, 
                "UPDATE ExamQuestion SET BankId = @BankId, GradeId = @GradeId, CourseId = @CourseId, QuestionContent = @QuestionContent, QuestionText = @QuestionText, Options = @Options, Answer = @Answer, Analysis = @Analysis, Score = @Score, Difficulty = @Difficulty, KnowledgePoint = @KnowledgePoint, Tags = @Tags, UpdateTime = @UpdateTime WHERE QuestionId = @QuestionId",
                new SqlParameter[] { 
                    new SqlParameter("@QuestionId", q.QuestionId), 
                    new SqlParameter("@BankId", q.BankId), 
                    new SqlParameter("@GradeId", q.GradeId ?? (object)DBNull.Value),
                    new SqlParameter("@CourseId", q.CourseId ?? (object)DBNull.Value),
                    new SqlParameter("@QuestionContent", q.QuestionContent ?? ""),
                    new SqlParameter("@QuestionText", q.QuestionText ?? ""),
                    new SqlParameter("@Options", q.Options ?? ""), new SqlParameter("@Answer", q.Answer ?? ""),
                    new SqlParameter("@Analysis", q.Analysis ?? ""),
                    new SqlParameter("@Score", q.Score), new SqlParameter("@Difficulty", q.Difficulty),
                    new SqlParameter("@KnowledgePoint", q.KnowledgePoint ?? ""), new SqlParameter("@Tags", q.Tags ?? ""),
                    new SqlParameter("@UpdateTime", DateTime.Now) }) > 0;
        }
        public bool DeleteQuestion(long questionId) {
            return SqlHelper.ExecuteNonQuery(CommandType.Text, "UPDATE ExamQuestion SET Status = 0 WHERE QuestionId = @QuestionId", new SqlParameter("@QuestionId", questionId)) > 0;
        }
        public List<Model.ExamQuestion> RandomPick(int bankId, int questionType, int difficulty, int count) {
            var list = new List<Model.ExamQuestion>();
            var sql = "SELECT TOP (@Count) * FROM ExamQuestion WHERE BankId = @BankId AND QuestionType = @QuestionType AND Difficulty = @Difficulty AND Status = 1 ORDER BY NEWID()";
            using (var reader = SqlHelper.ExecuteReader(CommandType.Text, sql, new SqlParameter[] {
                new SqlParameter("@Count", count), new SqlParameter("@BankId", bankId), new SqlParameter("@QuestionType", questionType), new SqlParameter("@Difficulty", difficulty) }))
                while (reader.Read()) list.Add(ReaderToQuestion(reader));
            return list;
        }
        private Model.ExamQuestion ReaderToQuestion(IDataReader reader) {
            return new Model.ExamQuestion {
                QuestionId = reader["QuestionId"] != DBNull.Value ? Convert.ToInt64(reader["QuestionId"]) : 0,
                BankId = reader["BankId"] != DBNull.Value ? Convert.ToInt32(reader["BankId"]) : 0,
                QuestionType = reader["QuestionType"] != DBNull.Value ? Convert.ToInt32(reader["QuestionType"]) : 1,
                QuestionContent = reader["QuestionContent"] != DBNull.Value ? reader["QuestionContent"].ToString() : null,
                QuestionText = reader["QuestionText"] != DBNull.Value ? reader["QuestionText"].ToString() : null,
                Options = reader["Options"] != DBNull.Value ? reader["Options"].ToString() : null,
                Answer = reader["Answer"] != DBNull.Value ? reader["Answer"].ToString() : null,
                Analysis = reader["Analysis"] != DBNull.Value ? reader["Analysis"].ToString() : null,
                Score = reader["Score"] != DBNull.Value ? Convert.ToDecimal(reader["Score"]) : 1,
                Difficulty = reader["Difficulty"] != DBNull.Value ? Convert.ToInt32(reader["Difficulty"]) : 1,
                KnowledgePoint = reader["KnowledgePoint"] != DBNull.Value ? reader["KnowledgePoint"].ToString() : null,
                Tags = reader["Tags"] != DBNull.Value ? reader["Tags"].ToString() : null
            };
        }

        /// <summary>
        /// 根据Quiz题目ID获取Exam题目
        /// </summary>
        public Model.ExamQuestion GetQuestionByQuizId(int quizId) {
            var sql = @"SELECT q.*, b.BankName
                        FROM ExamQuestion q
                        LEFT JOIN Quiz qz ON CAST(qz.BankId AS VARCHAR) = CAST(q.BankId AS VARCHAR)
                        LEFT JOIN ExamQuestionBank b ON q.BankId = b.BankId
                        WHERE qz.Qid = @QuizId";
            using (var reader = SqlHelper.ExecuteReader(CommandType.Text, sql, new SqlParameter("@QuizId", quizId)))
                if (reader.Read()) return ReaderToQuestion(reader);
            return null;
        }

        /// <summary>
        /// 批量删除题目
        /// </summary>
        public bool BatchDeleteQuestions(string questionIds) {
            var sql = string.Format("DELETE FROM ExamQuestion WHERE QuestionId IN ({0})", questionIds);
            return SqlHelper.ExecuteNonQuery(CommandType.Text, sql) > 0;
        }

        /// <summary>
        /// 更新题目使用次数
        /// </summary>
        public bool IncrementUseCount(long questionId) {
            var sql = "UPDATE ExamQuestion SET UseCount = UseCount + 1 WHERE QuestionId = @QuestionId";
            return SqlHelper.ExecuteNonQuery(CommandType.Text, sql, new SqlParameter("@QuestionId", questionId)) > 0;
        }

        /// <summary>
        /// 更新题目正确率
        /// </summary>
        public bool UpdateCorrectRate(long questionId, decimal correctRate) {
            var sql = "UPDATE ExamQuestion SET CorrectRate = @CorrectRate WHERE QuestionId = @QuestionId";
            return SqlHelper.ExecuteNonQuery(CommandType.Text, sql,
                new SqlParameter("@QuestionId", questionId),
                new SqlParameter("@CorrectRate", correctRate)) > 0;
        }
    }

    public class ExamAnswer {
        public List<Model.ExamAnswer> GetAnswerList(int examId, int? classId = null, string keyword = null) {
            var list = new List<Model.ExamAnswer>();
            var sql = "SELECT * FROM ExamAnswer WHERE ExamId = @ExamId";
            var parameters = new List<SqlParameter> { new SqlParameter("@ExamId", examId) };
            if (classId.HasValue) { sql += " AND ClassId = @ClassId"; parameters.Add(new SqlParameter("@ClassId", classId.Value)); }
            if (!string.IsNullOrEmpty(keyword)) { sql += " AND (StudentId LIKE @Keyword OR StudentName LIKE @Keyword)"; parameters.Add(new SqlParameter("@Keyword", "%" + keyword + "%")); }
            sql += " ORDER BY SubmitTime DESC";
            using (var reader = SqlHelper.ExecuteReader(CommandType.Text, sql, parameters.ToArray()))
                while (reader.Read()) list.Add(ReaderToAnswer(reader));
            return list;
        }
        public Model.ExamAnswer GetAnswerById(long answerId) {
            var sql = "SELECT * FROM ExamAnswer WHERE AnswerId = @AnswerId";
            using (var reader = SqlHelper.ExecuteReader(CommandType.Text, sql, new SqlParameter("@AnswerId", answerId)))
                if (reader.Read()) return ReaderToAnswer(reader);
            return null;
        }
        public Model.ExamAnswer GetAnswerByExamAndStudent(int examId, string studentId) {
            var sql = "SELECT * FROM ExamAnswer WHERE ExamId = @ExamId AND StudentId = @StudentId ORDER BY AnswerId DESC";
            using (var reader = SqlHelper.ExecuteReader(CommandType.Text, sql, new SqlParameter[] {
                new SqlParameter("@ExamId", examId), new SqlParameter("@StudentId", studentId) }))
                if (reader.Read()) return ReaderToAnswer(reader);
            return null;
        }
        public long AddAnswer(Model.ExamAnswer a) {
            var sql = "INSERT INTO ExamAnswer (ExamId, PaperId, StudentId, StudentName, ClassId, StartTime, Status, IpAddress, UserAgent) VALUES (@ExamId, @PaperId, @StudentId, @StudentName, @ClassId, @StartTime, @Status, @IpAddress, @UserAgent); SELECT SCOPE_IDENTITY()";
            return Convert.ToInt64(SqlHelper.ExecuteScalar(CommandType.Text, sql, new SqlParameter[] {
                new SqlParameter("@ExamId", a.ExamId), new SqlParameter("@PaperId", a.PaperId),
                new SqlParameter("@StudentId", a.StudentId ?? ""), new SqlParameter("@StudentName", a.StudentName ?? ""),
                new SqlParameter("@ClassId", a.ClassId ?? (object)DBNull.Value), new SqlParameter("@StartTime", a.StartTime),
                new SqlParameter("@Status", a.Status), new SqlParameter("@IpAddress", a.IpAddress ?? ""),
                new SqlParameter("@UserAgent", a.UserAgent ?? "")
            }));
        }
        public bool TempSaveAnswer(long answerId, string answers) {
            return SqlHelper.ExecuteNonQuery(CommandType.Text, "UPDATE ExamAnswer SET Answers = @Answers WHERE AnswerId = @AnswerId", 
                new SqlParameter[] { new SqlParameter("@AnswerId", answerId), new SqlParameter("@Answers", answers ?? "") }) > 0;
        }
        public bool SubmitAnswer(Model.ExamAnswer a) {
            var sql = @"UPDATE ExamAnswer SET Answers = @Answers, SubmitTime = @SubmitTime, Duration = @Duration, 
                        TotalScore = @TotalScore, ObjectiveScore = @ObjectiveScore, ScoreDetails = @ScoreDetails, Status = 1, IpAddress = @IpAddress WHERE AnswerId = @AnswerId";
            return SqlHelper.ExecuteNonQuery(CommandType.Text, sql, new SqlParameter[] {
                new SqlParameter("@AnswerId", a.AnswerId), new SqlParameter("@Answers", a.Answers ?? ""),
                new SqlParameter("@SubmitTime", a.SubmitTime ?? DateTime.Now), new SqlParameter("@Duration", a.Duration),
                new SqlParameter("@TotalScore", a.TotalScore), new SqlParameter("@ObjectiveScore", a.ObjectiveScore),
                new SqlParameter("@ScoreDetails", a.ScoreDetails ?? ""), new SqlParameter("@IpAddress", a.IpAddress ?? "")
            }) > 0;
        }
        public int GetSubmitCount(int examId, string studentId) {
            var sql = "SELECT COUNT(*) FROM ExamAnswer WHERE ExamId = @ExamId AND StudentId = @StudentId AND Status > 0";
            return Convert.ToInt32(SqlHelper.ExecuteScalar(CommandType.Text, sql, new SqlParameter[] {
                new SqlParameter("@ExamId", examId), new SqlParameter("@StudentId", studentId) }));
        }
        private Model.ExamAnswer ReaderToAnswer(IDataReader reader) {
            return new Model.ExamAnswer {
                AnswerId = reader["AnswerId"] != DBNull.Value ? Convert.ToInt64(reader["AnswerId"]) : 0,
                ExamId = reader["ExamId"] != DBNull.Value ? Convert.ToInt32(reader["ExamId"]) : 0,
                PaperId = reader["PaperId"] != DBNull.Value ? Convert.ToInt32(reader["PaperId"]) : 0,
                StudentId = reader["StudentId"] != DBNull.Value ? reader["StudentId"].ToString() : null,
                StudentName = reader["StudentName"] != DBNull.Value ? reader["StudentName"].ToString() : null,
                ClassId = reader["ClassId"] != DBNull.Value ? Convert.ToInt32(reader["ClassId"]) : 0,
                StartTime = reader["StartTime"] != DBNull.Value ? Convert.ToDateTime(reader["StartTime"]) : DateTime.MinValue,
                SubmitTime = reader["SubmitTime"] != DBNull.Value ? Convert.ToDateTime(reader["SubmitTime"]) : (DateTime?)null,
                Duration = reader["Duration"] != DBNull.Value ? Convert.ToInt32(reader["Duration"]) : 0,
                TotalScore = reader["TotalScore"] != DBNull.Value ? Convert.ToDecimal(reader["TotalScore"]) : 0,
                ObjectiveScore = reader["ObjectiveScore"] != DBNull.Value ? Convert.ToDecimal(reader["ObjectiveScore"]) : 0,
                Answers = reader["Answers"] != DBNull.Value ? reader["Answers"].ToString() : null,
                TempAnswers = reader["TempAnswers"] != DBNull.Value ? reader["TempAnswers"].ToString() : null,
                ScoreDetails = reader["ScoreDetails"] != DBNull.Value ? reader["ScoreDetails"].ToString() : null,
                Status = reader["Status"] != DBNull.Value ? Convert.ToInt32(reader["Status"]) : 0
            };
        }
    }

    public class ExamResult {
        public List<Model.ExamResult> GetResultList(int examId) {
            var list = new List<Model.ExamResult>();
            using (var reader = SqlHelper.ExecuteReader(CommandType.Text, "SELECT * FROM ExamResult WHERE ExamId = @ExamId ORDER BY TotalScore DESC", new SqlParameter("@ExamId", examId)))
                while (reader.Read()) list.Add(new Model.ExamResult { StudentId = reader["StudentId"] != DBNull.Value ? reader["StudentId"].ToString() : null, StudentName = reader["StudentName"] != DBNull.Value ? reader["StudentName"].ToString() : null, TotalScore = Convert.ToDecimal(reader["TotalScore"]) });
            return list;
        }
        public void AddOrUpdate(Model.ExamResult r) {
            var sql = "INSERT INTO ExamResult (ExamId, StudentId, StudentName, TotalScore, SubmitTime, Status) VALUES (@ExamId, @StudentId, @StudentName, @TotalScore, @SubmitTime, @Status)";
            SqlHelper.ExecuteNonQuery(CommandType.Text, sql, new SqlParameter[] {
                new SqlParameter("@ExamId", r.ExamId), new SqlParameter("@StudentId", r.StudentId),
                new SqlParameter("@StudentName", r.StudentName), new SqlParameter("@TotalScore", r.TotalScore),
                new SqlParameter("@SubmitTime", DateTime.Now), new SqlParameter("@Status", 1)
            });
        }
    }
}
