using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using LearnSite.DBUtility;

namespace LearnSite.DAL
{
    /// <summary>
    /// 数据访问类:AIProvider
    /// </summary>
    public partial class AIProvider
    {
        public AIProvider()
        {}

        #region  Method
        
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LearnSite.Model.AIProvider model)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("insert into AIProvider(");
            strSql.Append("DisplayName,ProviderName,ModelName,ApiKey,BaseUrl,IsDefault)");
            strSql.Append(" values (");
            strSql.Append("@DisplayName,@ProviderName,@ModelName,@ApiKey,@BaseUrl,@IsDefault)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@DisplayName", SqlDbType.NVarChar,50),
                    new SqlParameter("@ProviderName", SqlDbType.NVarChar,50),
                    new SqlParameter("@ModelName", SqlDbType.NVarChar,50),
                    new SqlParameter("@ApiKey", SqlDbType.NVarChar,200),
                    new SqlParameter("@BaseUrl", SqlDbType.NVarChar,200),
                    new SqlParameter("@IsDefault", SqlDbType.Bit,1)};
            parameters[0].Value = model.DisplayName;
            parameters[1].Value = model.ProviderName;
            parameters[2].Value = model.ModelName;
            parameters[3].Value = model.ApiKey;
            parameters[4].Value = model.BaseUrl;
            parameters[5].Value = model.IsDefault;

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
        
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LearnSite.Model.AIProvider model)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("update AIProvider set ");
            strSql.Append("DisplayName=@DisplayName,");
            strSql.Append("ProviderName=@ProviderName,");
            strSql.Append("ModelName=@ModelName,");
            strSql.Append("ApiKey=@ApiKey,");
            strSql.Append("BaseUrl=@BaseUrl,");
            strSql.Append("IsDefault=@IsDefault");
            strSql.Append(" where Id=@Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@DisplayName", SqlDbType.NVarChar,50),
                    new SqlParameter("@ProviderName", SqlDbType.NVarChar,50),
                    new SqlParameter("@ModelName", SqlDbType.NVarChar,50),
                    new SqlParameter("@ApiKey", SqlDbType.NVarChar,200),
                    new SqlParameter("@BaseUrl", SqlDbType.NVarChar,200),
                    new SqlParameter("@IsDefault", SqlDbType.Bit,1),
                    new SqlParameter("@Id", SqlDbType.Int,4)};
            parameters[0].Value = model.DisplayName;
            parameters[1].Value = model.ProviderName;
            parameters[2].Value = model.ModelName;
            parameters[3].Value = model.ApiKey;
            parameters[4].Value = model.BaseUrl;
            parameters[5].Value = model.IsDefault;
            parameters[6].Value = model.Id;

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

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int Id)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("delete from AIProvider ");
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

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LearnSite.Model.AIProvider GetModel(int Id)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("select  top 1 Id,DisplayName,ProviderName,ModelName,ApiKey,BaseUrl,IsDefault from AIProvider ");
            strSql.Append(" where Id=@Id");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int,4)
            };
            parameters[0].Value = Id;

            LearnSite.Model.AIProvider model=new LearnSite.Model.AIProvider();
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


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LearnSite.Model.AIProvider DataRowToModel(DataRow row)
        {
            LearnSite.Model.AIProvider model=new LearnSite.Model.AIProvider();
            if (row != null)
            {
                if(row["Id"]!=null && row["Id"].ToString()!="")
                {
                    model.Id=int.Parse(row["Id"].ToString());
                }
                if(row["DisplayName"]!=null)
                {
                    model.DisplayName=row["DisplayName"].ToString();
                }
                if(row["ProviderName"]!=null)
                {
                    model.ProviderName=row["ProviderName"].ToString();
                }
                if(row["ModelName"]!=null)
                {
                    model.ModelName=row["ModelName"].ToString();
                }
                if(row["ApiKey"]!=null)
                {
                    model.ApiKey=row["ApiKey"].ToString();
                }
                if(row["BaseUrl"]!=null)
                {
                    model.BaseUrl=row["BaseUrl"].ToString();
                }
                if(row["IsDefault"]!=null && row["IsDefault"].ToString()!="")
                {
                    if((row["IsDefault"].ToString()=="1")||(row["IsDefault"].ToString().ToLower()=="true"))
                    {
                        model.IsDefault=true;
                    }
                    else
                    {
                        model.IsDefault=false;
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql=new StringBuilder();
            strSql.Append("select Id,DisplayName,ProviderName,ModelName,ApiKey,BaseUrl,IsDefault ");
            strSql.Append(" FROM AIProvider ");
            if(strWhere.Trim()!="")
            {
                strSql.Append(" where "+strWhere);
            }
            strSql.Append(" order by Id desc ");
            return DbHelperSQL.Query(strSql.ToString());
        }
        
        /// <summary>
        /// 设置默认模型
        /// </summary>
        public bool SetDefault(int Id)
        {
            // First reset all to false
            string resetSql = "UPDATE AIProvider SET IsDefault = 0";
            DbHelperSQL.ExecuteSql(resetSql);
            
            // Then set the selected to true
            string updateSql = "UPDATE AIProvider SET IsDefault = 1 WHERE Id = @Id";
            SqlParameter[] parameters = {
                new SqlParameter("@Id", SqlDbType.Int, 4)
            };
            parameters[0].Value = Id;
            
            int rows = DbHelperSQL.ExecuteSql(updateSql, parameters);
            return rows > 0;
        }

        #endregion  Method
    }
}
