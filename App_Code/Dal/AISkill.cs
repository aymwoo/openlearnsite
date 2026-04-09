using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using LearnSite.DBUtility;

namespace LearnSite.DAL
{
    /// <summary>
    /// 数据访问类:AISkill
    /// </summary>
    public partial class AISkill
    {
        public AISkill()
        {}

        #region  Method

        public int Add(LearnSite.Model.AISkill model)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("insert into AISkill(");
            strSql.Append("SkillName,PromptContent,IsActive)");
            strSql.Append(" values (");
            strSql.Append("@SkillName,@PromptContent,@IsActive)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@SkillName", SqlDbType.NVarChar,100),
                    new SqlParameter("@PromptContent", SqlDbType.NVarChar,-1),
                    new SqlParameter("@IsActive", SqlDbType.Bit,1)};
            parameters[0].Value = model.SkillName;
            parameters[1].Value = model.PromptContent;
            parameters[2].Value = model.IsActive;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        public bool Update(LearnSite.Model.AISkill model)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("update AISkill set ");
            strSql.Append("SkillName=@SkillName,");
            strSql.Append("PromptContent=@PromptContent,");
            strSql.Append("IsActive=@IsActive");
            strSql.Append(" where Id=@Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@SkillName", SqlDbType.NVarChar,100),
                    new SqlParameter("@PromptContent", SqlDbType.NVarChar,-1),
                    new SqlParameter("@IsActive", SqlDbType.Bit,1),
                    new SqlParameter("@Id", SqlDbType.Int,4)};
            parameters[0].Value = model.SkillName;
            parameters[1].Value = model.PromptContent;
            parameters[2].Value = model.IsActive;
            parameters[3].Value = model.Id;

            int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Delete(int Id)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("delete from AISkill ");
            strSql.Append(" where Id=@Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int,4)
            };
            parameters[0].Value = Id;

            int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public LearnSite.Model.AISkill GetModel(int Id)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("select  top 1 Id,SkillName,PromptContent,IsActive from AISkill ");
            strSql.Append(" where Id=@Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int,4)
            };
            parameters[0].Value = Id;

            LearnSite.Model.AISkill model=new LearnSite.Model.AISkill();
            DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
            if(ds.Tables[0].Rows.Count>0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        public LearnSite.Model.AISkill DataRowToModel(DataRow row)
        {
            LearnSite.Model.AISkill model=new LearnSite.Model.AISkill();
            if (row != null)
            {
                if(row["Id"]!=null && row["Id"].ToString()!="")
                {
                    model.Id=int.Parse(row["Id"].ToString());
                }
                if(row["SkillName"]!=null)
                {
                    model.SkillName=row["SkillName"].ToString();
                }
                if(row["PromptContent"]!=null)
                {
                    model.PromptContent=row["PromptContent"].ToString();
                }
                if(row["IsActive"]!=null && row["IsActive"].ToString()!="")
                {
                    if((row["IsActive"].ToString()=="1")||(row["IsActive"].ToString().ToLower()=="true"))
                    {
                        model.IsActive=true;
                    }
                    else
                    {
                        model.IsActive=false;
                    }
                }
            }
            return model;
        }

        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("select Id,SkillName,PromptContent,IsActive ");
            strSql.Append(" FROM AISkill ");
            if(strWhere.Trim()!="")
            {
                strSql.Append(" where "+strWhere);
            }
            strSql.Append(" order by Id desc ");
            return DbHelperSQL.Query(strSql.ToString());
        }

        #endregion  Method
    }
}
