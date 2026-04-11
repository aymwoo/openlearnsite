using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using LearnSite.DBUtility;

namespace LearnSite.DAL
{
    public class CourseActivityPlanDraft
    {
        public LearnSite.Model.CourseActivityPlanDraft GetCurrentByCourse(int cid, int hid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 Id,Cid,Hid,Topic,Grade,Duration,TeachingGoalsInput,ExistingCourseContentSnapshot,DraftJson,LinkedMissionId,LinkedListMenuId,CreatedAt,UpdatedAt ");
            strSql.Append("from CourseActivityPlanDraft where Cid=@Cid and Hid=@Hid order by Id desc");
            SqlParameter[] parameters = {
                new SqlParameter("@Cid", SqlDbType.Int, 4),
                new SqlParameter("@Hid", SqlDbType.Int, 4)
            };
            parameters[0].Value = cid;
            parameters[1].Value = hid;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            return DataRowToModel(ds.Tables[0].Rows[0]);
        }

        public bool UpsertCurrent(LearnSite.Model.CourseActivityPlanDraft model)
        {
            SqlParameter[] parameters = BuildParameters(model);
            return DbHelperSQL.ExecuteSql(BuildUpsertSql(), parameters) > 0;
        }

        public bool DeleteCurrent(int cid, int hid)
        {
            string sql = "delete from CourseActivityPlanDraft where Cid=@Cid and Hid=@Hid";
            SqlParameter[] parameters = {
                new SqlParameter("@Cid", SqlDbType.Int, 4),
                new SqlParameter("@Hid", SqlDbType.Int, 4)
            };
            parameters[0].Value = cid;
            parameters[1].Value = hid;
            return DbHelperSQL.ExecuteSql(sql, parameters) > 0;
        }

        public static string BuildUpsertSql()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("if exists(select 1 from CourseActivityPlanDraft where Cid=@Cid) ");
            strSql.Append("begin ");
            strSql.Append("update CourseActivityPlanDraft set Hid=@Hid,Topic=@Topic,Grade=@Grade,Duration=@Duration,");
            strSql.Append("TeachingGoalsInput=@TeachingGoalsInput,ExistingCourseContentSnapshot=@ExistingCourseContentSnapshot,");
            strSql.Append("DraftJson=@DraftJson,LinkedMissionId=isnull(@LinkedMissionId,LinkedMissionId),LinkedListMenuId=isnull(@LinkedListMenuId,LinkedListMenuId),UpdatedAt=@UpdatedAt where Cid=@Cid; ");
            strSql.Append("end ");
            strSql.Append("else ");
            strSql.Append("begin ");
            strSql.Append("insert into CourseActivityPlanDraft(Cid,Hid,Topic,Grade,Duration,TeachingGoalsInput,ExistingCourseContentSnapshot,DraftJson,LinkedMissionId,LinkedListMenuId,CreatedAt,UpdatedAt) ");
            strSql.Append("values(@Cid,@Hid,@Topic,@Grade,@Duration,@TeachingGoalsInput,@ExistingCourseContentSnapshot,@DraftJson,@LinkedMissionId,@LinkedListMenuId,@CreatedAt,@UpdatedAt); ");
            strSql.Append("end");
            return strSql.ToString();
        }

        private static SqlParameter[] BuildParameters(LearnSite.Model.CourseActivityPlanDraft model)
        {
            SqlParameter[] parameters = {
                new SqlParameter("@Cid", SqlDbType.Int, 4),
                new SqlParameter("@Hid", SqlDbType.Int, 4),
                new SqlParameter("@Topic", SqlDbType.NVarChar, 200),
                new SqlParameter("@Grade", SqlDbType.NVarChar, 50),
                new SqlParameter("@Duration", SqlDbType.NVarChar, 50),
                new SqlParameter("@TeachingGoalsInput", SqlDbType.NVarChar, 500),
                new SqlParameter("@ExistingCourseContentSnapshot", SqlDbType.NVarChar, -1),
                new SqlParameter("@DraftJson", SqlDbType.NVarChar, -1),
                new SqlParameter("@LinkedMissionId", SqlDbType.Int),
                new SqlParameter("@LinkedListMenuId", SqlDbType.Int),
                new SqlParameter("@CreatedAt", SqlDbType.DateTime),
                new SqlParameter("@UpdatedAt", SqlDbType.DateTime)
            };
            parameters[0].Value = model.Cid;
            parameters[1].Value = model.Hid;
            parameters[2].Value = model.Topic;
            parameters[3].Value = model.Grade ?? string.Empty;
            parameters[4].Value = model.Duration ?? string.Empty;
            parameters[5].Value = model.TeachingGoalsInput ?? string.Empty;
            parameters[6].Value = model.ExistingCourseContentSnapshot ?? string.Empty;
            parameters[7].Value = model.DraftJson ?? string.Empty;
            parameters[8].Value = model.LinkedMissionId.HasValue ? (object)model.LinkedMissionId.Value : DBNull.Value;
            parameters[9].Value = model.LinkedListMenuId.HasValue ? (object)model.LinkedListMenuId.Value : DBNull.Value;
            parameters[10].Value = model.CreatedAt == DateTime.MinValue ? DateTime.Now : model.CreatedAt;
            parameters[11].Value = model.UpdatedAt == DateTime.MinValue ? DateTime.Now : model.UpdatedAt;
            return parameters;
        }

        public LearnSite.Model.CourseActivityPlanDraft DataRowToModel(DataRow row)
        {
            LearnSite.Model.CourseActivityPlanDraft model = new LearnSite.Model.CourseActivityPlanDraft();
            if (row == null)
            {
                return model;
            }

            if (row["Id"] != null && row["Id"].ToString() != string.Empty)
            {
                model.Id = int.Parse(row["Id"].ToString());
            }
            if (row["Cid"] != null && row["Cid"].ToString() != string.Empty)
            {
                model.Cid = int.Parse(row["Cid"].ToString());
            }
            if (row["Hid"] != null && row["Hid"].ToString() != string.Empty)
            {
                model.Hid = int.Parse(row["Hid"].ToString());
            }
            model.Topic = row["Topic"] == null ? string.Empty : row["Topic"].ToString();
            model.Grade = row["Grade"] == null ? string.Empty : row["Grade"].ToString();
            model.Duration = row["Duration"] == null ? string.Empty : row["Duration"].ToString();
            model.TeachingGoalsInput = row["TeachingGoalsInput"] == null ? string.Empty : row["TeachingGoalsInput"].ToString();
            model.ExistingCourseContentSnapshot = row["ExistingCourseContentSnapshot"] == null ? string.Empty : row["ExistingCourseContentSnapshot"].ToString();
            model.DraftJson = row["DraftJson"] == null ? string.Empty : row["DraftJson"].ToString();
            if (row.Table.Columns.Contains("LinkedMissionId") && row["LinkedMissionId"] != DBNull.Value && row["LinkedMissionId"].ToString() != string.Empty)
            {
                model.LinkedMissionId = int.Parse(row["LinkedMissionId"].ToString());
            }
            if (row.Table.Columns.Contains("LinkedListMenuId") && row["LinkedListMenuId"] != DBNull.Value && row["LinkedListMenuId"].ToString() != string.Empty)
            {
                model.LinkedListMenuId = int.Parse(row["LinkedListMenuId"].ToString());
            }
            if (row["CreatedAt"] != null && row["CreatedAt"].ToString() != string.Empty)
            {
                model.CreatedAt = DateTime.Parse(row["CreatedAt"].ToString());
            }
            if (row["UpdatedAt"] != null && row["UpdatedAt"].ToString() != string.Empty)
            {
                model.UpdatedAt = DateTime.Parse(row["UpdatedAt"].ToString());
            }
            return model;
        }
    }
}
