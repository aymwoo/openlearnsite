using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using LearnSite.DBUtility;

namespace LearnSite.DAL
{
    /// <summary>
    /// 数据访问类:AICustomSkill
    /// </summary>
    public partial class AICustomSkill
    {
        public AICustomSkill()
        {}

        #region  Method

        public int Add(LearnSite.Model.AICustomSkill model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into AICustomSkill(");
            strSql.Append("SkillName,PromptContent,SkillScope,IsActive)");
            strSql.Append(" values (");
            strSql.Append("@SkillName,@PromptContent,@SkillScope,@IsActive)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@SkillName", SqlDbType.NVarChar, 100),
                    new SqlParameter("@PromptContent", SqlDbType.NVarChar, -1),
                    new SqlParameter("@SkillScope", SqlDbType.NVarChar, 200),
                    new SqlParameter("@IsActive", SqlDbType.Bit, 1)};
            parameters[0].Value = model.SkillName;
            parameters[1].Value = model.PromptContent;
            parameters[2].Value = model.SkillScope ?? "";
            parameters[3].Value = model.IsActive;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
                return 0;
            else
                return Convert.ToInt32(obj);
        }

        public bool Update(LearnSite.Model.AICustomSkill model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update AICustomSkill set ");
            strSql.Append("SkillName=@SkillName,");
            strSql.Append("PromptContent=@PromptContent,");
            strSql.Append("SkillScope=@SkillScope,");
            strSql.Append("IsActive=@IsActive");
            strSql.Append(" where Id=@Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@SkillName", SqlDbType.NVarChar, 100),
                    new SqlParameter("@PromptContent", SqlDbType.NVarChar, -1),
                    new SqlParameter("@SkillScope", SqlDbType.NVarChar, 200),
                    new SqlParameter("@IsActive", SqlDbType.Bit, 1),
                    new SqlParameter("@Id", SqlDbType.Int, 4)};
            parameters[0].Value = model.SkillName;
            parameters[1].Value = model.PromptContent;
            parameters[2].Value = model.SkillScope ?? "";
            parameters[3].Value = model.IsActive;
            parameters[4].Value = model.Id;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            return rows > 0;
        }

        public bool Delete(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from AICustomSkill ");
            strSql.Append(" where Id=@Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int, 4)
            };
            parameters[0].Value = Id;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            return rows > 0;
        }

        public LearnSite.Model.AICustomSkill GetModel(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 Id,SkillName,PromptContent,SkillScope,IsActive from AICustomSkill ");
            strSql.Append(" where Id=@Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int, 4)
            };
            parameters[0].Value = Id;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
                return DataRowToModel(ds.Tables[0].Rows[0]);
            else
                return null;
        }

        public LearnSite.Model.AICustomSkill DataRowToModel(DataRow row)
        {
            LearnSite.Model.AICustomSkill model = new LearnSite.Model.AICustomSkill();
            if (row != null)
            {
                if (row["Id"] != null && row["Id"].ToString() != "")
                    model.Id = int.Parse(row["Id"].ToString());
                if (row["SkillName"] != null)
                    model.SkillName = row["SkillName"].ToString();
                if (row["PromptContent"] != null)
                    model.PromptContent = row["PromptContent"].ToString();
                if (row["SkillScope"] != null)
                    model.SkillScope = row["SkillScope"].ToString();
                if (row["IsActive"] != null && row["IsActive"].ToString() != "")
                    model.IsActive = (row["IsActive"].ToString() == "1" || row["IsActive"].ToString().ToLower() == "true");
            }
            return model;
        }

        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id,SkillName,PromptContent,SkillScope,IsActive ");
            strSql.Append(" FROM AICustomSkill ");
            if (strWhere.Trim() != "")
                strSql.Append(" where " + strWhere);
            strSql.Append(" order by Id desc ");
            return DbHelperSQL.Query(strSql.ToString());
        }

        #endregion  Method
    }
}
