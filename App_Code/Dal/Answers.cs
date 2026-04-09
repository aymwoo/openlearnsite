using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using LearnSite.DBUtility;//Please add references
namespace LearnSite.DAL
{
	/// <summary>
	/// 数据访问类:Answers
	/// </summary>
	public partial class Answers
	{
		public Answers()
		{}
		#region  BasicMethod
        

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LearnSite.Model.Answers model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Answers(");
			strSql.Append("Eid,Asid,Asnum,Asname,Asgrade,Asclass,Atime,Ascore,Aspent,Adata)");
			strSql.Append(" values (");
			strSql.Append("@Eid,@Asid,@Asnum,@Asname,@Asgrade,@Asclass,@Atime,@Ascore,@Aspent,@Adata)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Eid", SqlDbType.Int,4),
					new SqlParameter("@Asid", SqlDbType.Int,4),
					new SqlParameter("@Asnum", SqlDbType.NVarChar,50),
					new SqlParameter("@Asname", SqlDbType.NVarChar,50),
					new SqlParameter("@Asgrade", SqlDbType.Int,4),
					new SqlParameter("@Asclass", SqlDbType.Int,4),
					new SqlParameter("@Atime", SqlDbType.DateTime),
					new SqlParameter("@Ascore", SqlDbType.Int,4),
					new SqlParameter("@Aspent", SqlDbType.Int,4),
					new SqlParameter("@Adata", SqlDbType.NText)};
			parameters[0].Value = model.Eid;
			parameters[1].Value = model.Asid;
			parameters[2].Value = model.Asnum;
			parameters[3].Value = model.Asname;
			parameters[4].Value = model.Asgrade;
			parameters[5].Value = model.Asclass;
			parameters[6].Value = model.Atime;
			parameters[7].Value = model.Ascore;
			parameters[8].Value = model.Aspent;
			parameters[9].Value = model.Adata;

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
		public bool Update(LearnSite.Model.Answers model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Answers set ");
			strSql.Append("Eid=@Eid,");
			strSql.Append("Asid=@Asid,");
			strSql.Append("Asnum=@Asnum,");
			strSql.Append("Asname=@Asname,");
			strSql.Append("Asgrade=@Asgrade,");
			strSql.Append("Asclass=@Asclass,");
			strSql.Append("Atime=@Atime,");
			strSql.Append("Ascore=@Ascore,");
			strSql.Append("Aspent=@Aspent,");
			strSql.Append("Adata=@Adata");
			strSql.Append(" where Aid=@Aid");
			SqlParameter[] parameters = {
					new SqlParameter("@Eid", SqlDbType.Int,4),
					new SqlParameter("@Asid", SqlDbType.Int,4),
					new SqlParameter("@Asnum", SqlDbType.NVarChar,50),
					new SqlParameter("@Asname", SqlDbType.NVarChar,50),
					new SqlParameter("@Asgrade", SqlDbType.Int,4),
					new SqlParameter("@Asclass", SqlDbType.Int,4),
					new SqlParameter("@Atime", SqlDbType.DateTime),
					new SqlParameter("@Ascore", SqlDbType.Int,4),
					new SqlParameter("@Aspent", SqlDbType.Int,4),
					new SqlParameter("@Adata", SqlDbType.NText),
					new SqlParameter("@Aid", SqlDbType.Int,4)};
			parameters[0].Value = model.Eid;
			parameters[1].Value = model.Asid;
			parameters[2].Value = model.Asnum;
			parameters[3].Value = model.Asname;
			parameters[4].Value = model.Asgrade;
			parameters[5].Value = model.Asclass;
			parameters[6].Value = model.Atime;
			parameters[7].Value = model.Ascore;
			parameters[8].Value = model.Aspent;
			parameters[9].Value = model.Adata;
			parameters[10].Value = model.Aid;

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
		public bool Delete(int Aid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Answers ");
			strSql.Append(" where Aid=@Aid");
			SqlParameter[] parameters = {
					new SqlParameter("@Aid", SqlDbType.Int,4)
			};
			parameters[0].Value = Aid;

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
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string Aidlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Answers ");
			strSql.Append(" where Aid in ("+Aidlist + ")  ");
			int rows=DbHelperSQL.ExecuteSql(strSql.ToString());
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
		public LearnSite.Model.Answers GetModel(int Aid)
		{			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Aid,Eid,Asid,Asnum,Asname,Asgrade,Asclass,Atime,Ascore,Aspent,Adata from Answers ");
			strSql.Append(" where Aid=@Aid");
			SqlParameter[] parameters = {
					new SqlParameter("@Aid", SqlDbType.Int,4)
			};
			parameters[0].Value = Aid;

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
        public LearnSite.Model.Answers GetModelme(int Eid, int Asid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1  * from Answers ");
            strSql.Append(" where Eid=@Eid and Asid=@Asid");
            SqlParameter[] parameters = {
					new SqlParameter("@Eid", SqlDbType.Int,4),                    
					new SqlParameter("@Asid", SqlDbType.Int,4)
			};
            parameters[0].Value = Eid;
            parameters[1].Value = Asid;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
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
		public LearnSite.Model.Answers DataRowToModel(DataRow row)
		{
            if (row != null)
            {
			    LearnSite.Model.Answers model=new LearnSite.Model.Answers();
                if (row["Aid"] != null && row["Aid"].ToString() != "")
                {
                    model.Aid = int.Parse(row["Aid"].ToString());
                }
                if (row["Eid"] != null && row["Eid"].ToString() != "")
                {
                    model.Eid = int.Parse(row["Eid"].ToString());
                }
                if (row["Asid"] != null && row["Asid"].ToString() != "")
                {
                    model.Asid = int.Parse(row["Asid"].ToString());
                }
                if (row["Asnum"] != null && row["Asnum"].ToString() != "")
                {
                    model.Asnum = row["Asnum"].ToString();
                }
                if (row["Asname"] != null)
                {
                    model.Asname = row["Asname"].ToString();
                }
                if (row["Asgrade"] != null && row["Asgrade"].ToString() != "")
                {
                    model.Asgrade = int.Parse(row["Asgrade"].ToString());
                }
                if (row["Asclass"] != null && row["Asclass"].ToString() != "")
                {
                    model.Asclass = int.Parse(row["Asclass"].ToString());
                }
                if (row["Atime"] != null && row["Atime"].ToString() != "")
                {
                    model.Atime = DateTime.Parse(row["Atime"].ToString());
                }
                if (row["Ascore"] != null && row["Ascore"].ToString() != "")
                {
                    model.Ascore = int.Parse(row["Ascore"].ToString());
                }
                if (row["Aspent"] != null && row["Aspent"].ToString() != "")
                {
                    model.Aspent = int.Parse(row["Aspent"].ToString());
                }
                if (row["Adata"] != null)
                {
                    model.Adata = row["Adata"].ToString();
                }
                return model;
            }
            else {
                return null;
            }
		}

        /// <summary>
        /// 获得未参加班级测验的学生列表
        /// </summary>
        public DataTable GetListClassSname(int Eid, int Asgrade, int Asclass)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT s.Sname ");
            strSql.Append("FROM Students s LEFT JOIN Answers a ON  s.Sid = a.Asid AND s.Snum = a.Asnum AND s.Sgrade = a.Asgrade AND s.Sclass = a.Asclass AND a.Eid = @Eid WHERE a.Aid IS NULL AND s.Sgrade = @Asgrade AND s.Sclass = @Asclass ");

            SqlParameter[] parameters = {
					new SqlParameter("@Eid", SqlDbType.Int,4),                    
					new SqlParameter("@Asgrade", SqlDbType.Int,4),                    
					new SqlParameter("@Asclass", SqlDbType.Int,4)
			};
            parameters[0].Value = Eid;
            parameters[1].Value = Asgrade;
            parameters[2].Value = Asclass;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);

            return ds.Tables[0];
        }
		/// <summary>
		/// 获得班级测验所有数据列表
		/// </summary>
        public DataTable GetListClassScore(int Eid, int Asgrade, int Asclass)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Aid,Eid,Asid,Asnum,Asname,Asgrade,Asclass,Atime,Ascore,Aspent,Adata ");
            strSql.Append(" FROM Answers,Students Where Eid=@Eid and Asgrade =@Asgrade and Asclass=@Asclass and Asid=Sid ");

            SqlParameter[] parameters = {
					new SqlParameter("@Eid", SqlDbType.Int,4),                    
					new SqlParameter("@Asgrade", SqlDbType.Int,4),                    
					new SqlParameter("@Asclass", SqlDbType.Int,4)
			};
            parameters[0].Value = Eid;
            parameters[1].Value = Asgrade;
            parameters[2].Value = Asclass;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);

            return ds.Tables[0];
		}
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Aid,Eid,Asid,Asnum,Asname,Asgrade,Asclass,Atime,Ascore,Aspent,Adata ");
            strSql.Append(" FROM Answers ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" Aid,Eid,Asid,Asnum,Asname,Asgrade,Asclass,Atime,Ascore,Aspent,Adata ");
			strSql.Append(" FROM Answers ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM Answers ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			object obj = DbHelperSQL.GetSingle(strSql.ToString());
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
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.Aid desc");
			}
			strSql.Append(")AS Row, T.*  from Answers T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "Answers";
			parameters[1].Value = "Aid";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

