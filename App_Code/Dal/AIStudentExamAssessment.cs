using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using LearnSite.DBUtility;

namespace LearnSite.DAL
{
    public class AIStudentExamAssessment
    {
        public int Add(LearnSite.Model.AIStudentExamAssessment model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into AIStudentExamAssessment(");
            strSql.Append("Fid,Sid,Snum,Sname,Cid,Lid,Vid,ProviderName,SkillName,Summary,AssessmentContent,LearningLog,AnswerLog,Score,QuestionCount,IsFallback,CreatedAt)");
            strSql.Append(" values (");
            strSql.Append("@Fid,@Sid,@Snum,@Sname,@Cid,@Lid,@Vid,@ProviderName,@SkillName,@Summary,@AssessmentContent,@LearningLog,@AnswerLog,@Score,@QuestionCount,@IsFallback,@CreatedAt)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                new SqlParameter("@Fid", SqlDbType.Int, 4),
                new SqlParameter("@Sid", SqlDbType.Int, 4),
                new SqlParameter("@Snum", SqlDbType.NVarChar, 50),
                new SqlParameter("@Sname", SqlDbType.NVarChar, 50),
                new SqlParameter("@Cid", SqlDbType.Int, 4),
                new SqlParameter("@Lid", SqlDbType.Int, 4),
                new SqlParameter("@Vid", SqlDbType.Int, 4),
                new SqlParameter("@ProviderName", SqlDbType.NVarChar, 100),
                new SqlParameter("@SkillName", SqlDbType.NVarChar, 100),
                new SqlParameter("@Summary", SqlDbType.NVarChar, 500),
                new SqlParameter("@AssessmentContent", SqlDbType.NVarChar, -1),
                new SqlParameter("@LearningLog", SqlDbType.NVarChar, -1),
                new SqlParameter("@AnswerLog", SqlDbType.NVarChar, -1),
                new SqlParameter("@Score", SqlDbType.Int, 4),
                new SqlParameter("@QuestionCount", SqlDbType.Int, 4),
                new SqlParameter("@IsFallback", SqlDbType.Bit, 1),
                new SqlParameter("@CreatedAt", SqlDbType.DateTime)
            };
            parameters[0].Value = (object)model.Fid ?? DBNull.Value;
            parameters[1].Value = (object)model.Sid ?? DBNull.Value;
            parameters[2].Value = (object)model.Snum ?? DBNull.Value;
            parameters[3].Value = (object)model.Sname ?? DBNull.Value;
            parameters[4].Value = (object)model.Cid ?? DBNull.Value;
            parameters[5].Value = (object)model.Lid ?? DBNull.Value;
            parameters[6].Value = (object)model.Vid ?? DBNull.Value;
            parameters[7].Value = (object)model.ProviderName ?? DBNull.Value;
            parameters[8].Value = (object)model.SkillName ?? DBNull.Value;
            parameters[9].Value = (object)model.Summary ?? DBNull.Value;
            parameters[10].Value = (object)model.AssessmentContent ?? DBNull.Value;
            parameters[11].Value = (object)model.LearningLog ?? DBNull.Value;
            parameters[12].Value = (object)model.AnswerLog ?? DBNull.Value;
            parameters[13].Value = (object)model.Score ?? DBNull.Value;
            parameters[14].Value = (object)model.QuestionCount ?? DBNull.Value;
            parameters[15].Value = model.IsFallback;
            parameters[16].Value = (object)model.CreatedAt ?? DateTime.Now;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
        }

        public LearnSite.Model.AIStudentExamAssessment GetLatestByStudentCourse(int sid, int cid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 * from AIStudentExamAssessment where Sid=@Sid and Cid=@Cid order by CreatedAt desc, Id desc");
            SqlParameter[] parameters = {
                new SqlParameter("@Sid", SqlDbType.Int, 4),
                new SqlParameter("@Cid", SqlDbType.Int, 4)
            };
            parameters[0].Value = sid;
            parameters[1].Value = cid;
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
                return DataRowToModel(ds.Tables[0].Rows[0]);
            return null;
        }

        public LearnSite.Model.AIStudentExamAssessment GetLatestByStudentCourseLesson(int sid, int cid, int lid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 * from AIStudentExamAssessment where Sid=@Sid and Cid=@Cid and Lid=@Lid order by CreatedAt desc, Id desc");
            SqlParameter[] parameters = {
                new SqlParameter("@Sid", SqlDbType.Int, 4),
                new SqlParameter("@Cid", SqlDbType.Int, 4),
                new SqlParameter("@Lid", SqlDbType.Int, 4)
            };
            parameters[0].Value = sid;
            parameters[1].Value = cid;
            parameters[2].Value = lid;
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
                return DataRowToModel(ds.Tables[0].Rows[0]);
            return null;
        }

        private LearnSite.Model.AIStudentExamAssessment DataRowToModel(DataRow row)
        {
            LearnSite.Model.AIStudentExamAssessment model = new LearnSite.Model.AIStudentExamAssessment();
            if (row == null)
                return model;

            if (row["Id"] != null && row["Id"].ToString() != "") model.Id = int.Parse(row["Id"].ToString());
            if (row["Fid"] != null && row["Fid"].ToString() != "") model.Fid = int.Parse(row["Fid"].ToString());
            if (row["Sid"] != null && row["Sid"].ToString() != "") model.Sid = int.Parse(row["Sid"].ToString());
            if (row["Snum"] != null) model.Snum = row["Snum"].ToString();
            if (row["Sname"] != null) model.Sname = row["Sname"].ToString();
            if (row["Cid"] != null && row["Cid"].ToString() != "") model.Cid = int.Parse(row["Cid"].ToString());
            if (row["Lid"] != null && row["Lid"].ToString() != "") model.Lid = int.Parse(row["Lid"].ToString());
            if (row["Vid"] != null && row["Vid"].ToString() != "") model.Vid = int.Parse(row["Vid"].ToString());
            if (row["ProviderName"] != null) model.ProviderName = row["ProviderName"].ToString();
            if (row["SkillName"] != null) model.SkillName = row["SkillName"].ToString();
            if (row["Summary"] != null) model.Summary = row["Summary"].ToString();
            if (row["AssessmentContent"] != null) model.AssessmentContent = row["AssessmentContent"].ToString();
            if (row["LearningLog"] != null) model.LearningLog = row["LearningLog"].ToString();
            if (row["AnswerLog"] != null) model.AnswerLog = row["AnswerLog"].ToString();
            if (row["Score"] != null && row["Score"].ToString() != "") model.Score = int.Parse(row["Score"].ToString());
            if (row["QuestionCount"] != null && row["QuestionCount"].ToString() != "") model.QuestionCount = int.Parse(row["QuestionCount"].ToString());
            if (row["IsFallback"] != null && row["IsFallback"].ToString() != "") model.IsFallback = row["IsFallback"].ToString() == "1" || row["IsFallback"].ToString().ToLower() == "true";
            if (row["CreatedAt"] != null && row["CreatedAt"].ToString() != "") model.CreatedAt = DateTime.Parse(row["CreatedAt"].ToString());
            return model;
        }
    }
}
