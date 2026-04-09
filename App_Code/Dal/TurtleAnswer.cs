
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using LearnSite.DBUtility;//Please add references
namespace LearnSite.DAL
{
	/// <summary>
	/// 数据访问类:TurtleAnswer
	/// </summary>
	public partial class TurtleAnswer
	{
		public TurtleAnswer()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Aid", "TurtleAnswer"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Aid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from TurtleAnswer");
			strSql.Append(" where Aid=@Aid");
			SqlParameter[] parameters = {
					new SqlParameter("@Aid", SqlDbType.Int,4)
			};
			parameters[0].Value = Aid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Isdone(int Aqid)
        {
            Model.Cook cook = new Model.Cook();
            if (cook.IsExist())
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select count(1) from TurtleAnswer");
                strSql.Append(" where Aqid=@Aqid and Asid=@Asid");
                SqlParameter[] parameters = {
					new SqlParameter("@Aqid", SqlDbType.Int,4),
					new SqlParameter("@Asid", SqlDbType.Int,4)
			    };
                parameters[0].Value = Aqid;
                parameters[1].Value = cook.Sid;

                return DbHelperSQL.Exists(strSql.ToString(), parameters);
            }
            return false;
        } 

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LearnSite.Model.TurtleAnswer model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into TurtleAnswer(");
			strSql.Append("Amid,Aqid,Acode,Aimg,Aurl,Aout,Ascore,Asid,Asname,Alock,Adate)");
			strSql.Append(" values (");
			strSql.Append("@Amid,@Aqid,@Acode,@Aimg,@Aurl,@Aout,@Ascore,@Asid,@Asname,@Alock,@Adate)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Amid", SqlDbType.Int,4),
					new SqlParameter("@Aqid", SqlDbType.Int,4),
					new SqlParameter("@Acode", SqlDbType.NText),
					new SqlParameter("@Aimg", SqlDbType.NVarChar,50),
					new SqlParameter("@Aurl", SqlDbType.NVarChar,50),
					new SqlParameter("@Aout", SqlDbType.NVarChar,200),
					new SqlParameter("@Ascore", SqlDbType.Int,4),
					new SqlParameter("@Asid", SqlDbType.Int,4),
					new SqlParameter("@Asname", SqlDbType.NVarChar,50),
					new SqlParameter("@Alock", SqlDbType.Bit,1),
					new SqlParameter("@Adate", SqlDbType.DateTime)};
			parameters[0].Value = model.Amid;
			parameters[1].Value = model.Aqid;
			parameters[2].Value = model.Acode;
			parameters[3].Value = model.Aimg;
			parameters[4].Value = model.Aurl;
			parameters[5].Value = model.Aout;
			parameters[6].Value = model.Ascore;
			parameters[7].Value = model.Asid;
			parameters[8].Value = model.Asname;
			parameters[9].Value = model.Alock;
			parameters[10].Value = model.Adate;

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
		public bool Update(LearnSite.Model.TurtleAnswer model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update TurtleAnswer set ");
			strSql.Append("Amid=@Amid,");
			strSql.Append("Aqid=@Aqid,");
			strSql.Append("Acode=@Acode,");
			strSql.Append("Aimg=@Aimg,");
			strSql.Append("Aurl=@Aurl,");
			strSql.Append("Aout=@Aout,");
			strSql.Append("Ascore=@Ascore,");
			strSql.Append("Asid=@Asid,");
			strSql.Append("Asname=@Asname,");
			strSql.Append("Alock=@Alock,");
			strSql.Append("Adate=@Adate");
			strSql.Append(" where Aid=@Aid");
			SqlParameter[] parameters = {
					new SqlParameter("@Amid", SqlDbType.Int,4),
					new SqlParameter("@Aqid", SqlDbType.Int,4),
					new SqlParameter("@Acode", SqlDbType.NText),
					new SqlParameter("@Aimg", SqlDbType.NVarChar,50),
					new SqlParameter("@Aurl", SqlDbType.NVarChar,50),
					new SqlParameter("@Aout", SqlDbType.NVarChar,200),
					new SqlParameter("@Ascore", SqlDbType.Int,4),
					new SqlParameter("@Asid", SqlDbType.Int,4),
					new SqlParameter("@Asname", SqlDbType.NVarChar,50),
					new SqlParameter("@Alock", SqlDbType.Bit,1),
					new SqlParameter("@Adate", SqlDbType.DateTime),
					new SqlParameter("@Aid", SqlDbType.Int,4)};
			parameters[0].Value = model.Amid;
			parameters[1].Value = model.Aqid;
			parameters[2].Value = model.Acode;
			parameters[3].Value = model.Aimg;
			parameters[4].Value = model.Aurl;
			parameters[5].Value = model.Aout;
			parameters[6].Value = model.Ascore;
			parameters[7].Value = model.Asid;
			parameters[8].Value = model.Asname;
			parameters[9].Value = model.Alock;
			parameters[10].Value = model.Adate;
			parameters[11].Value = model.Aid;

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
			strSql.Append("delete from TurtleAnswer ");
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
			strSql.Append("delete from TurtleAnswer ");
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
        public LearnSite.Model.TurtleAnswer GetModelByQidSid(int Aqid,int Asid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Aid,Amid,Aqid,Acode,Aimg,Aurl,Aout,Ascore,Asid,Asname,Alock,Adate from TurtleAnswer ");
            strSql.Append(" where Aqid=@Aqid and Asid=@Asid");
            SqlParameter[] parameters = {
					new SqlParameter("@Aqid", SqlDbType.Int,4),
					new SqlParameter("@Asid", SqlDbType.Int,4)
			};
            parameters[0].Value = Aqid;
            parameters[1].Value = Asid;

            LearnSite.Model.TurtleAnswer model = new LearnSite.Model.TurtleAnswer();
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
		public LearnSite.Model.TurtleAnswer GetModel(int Aid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Aid,Amid,Aqid,Acode,Aimg,Aurl,Aout,Ascore,Asid,Asname,Alock,Adate from TurtleAnswer ");
			strSql.Append(" where Aid=@Aid");
			SqlParameter[] parameters = {
					new SqlParameter("@Aid", SqlDbType.Int,4)
			};
			parameters[0].Value = Aid;

			LearnSite.Model.TurtleAnswer model=new LearnSite.Model.TurtleAnswer();
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
		public LearnSite.Model.TurtleAnswer DataRowToModel(DataRow row)
		{
			LearnSite.Model.TurtleAnswer model=new LearnSite.Model.TurtleAnswer();
			if (row != null)
			{
				if(row["Aid"]!=null && row["Aid"].ToString()!="")
				{
					model.Aid=int.Parse(row["Aid"].ToString());
				}
				if(row["Amid"]!=null && row["Amid"].ToString()!="")
				{
					model.Amid=int.Parse(row["Amid"].ToString());
				}
				if(row["Aqid"]!=null && row["Aqid"].ToString()!="")
				{
					model.Aqid=int.Parse(row["Aqid"].ToString());
				}
				if(row["Acode"]!=null)
				{
					model.Acode=row["Acode"].ToString();
				}
				if(row["Aimg"]!=null)
				{
					model.Aimg=row["Aimg"].ToString();
				}
				if(row["Aurl"]!=null)
				{
					model.Aurl=row["Aurl"].ToString();
				}
				if(row["Aout"]!=null)
				{
					model.Aout=row["Aout"].ToString();
				}
				if(row["Ascore"]!=null && row["Ascore"].ToString()!="")
				{
					model.Ascore=int.Parse(row["Ascore"].ToString());
				}
				if(row["Asid"]!=null && row["Asid"].ToString()!="")
				{
					model.Asid=int.Parse(row["Asid"].ToString());
				}
				if(row["Asname"]!=null)
				{
					model.Asname=row["Asname"].ToString();
				}
				if(row["Alock"]!=null && row["Alock"].ToString()!="")
				{
					if((row["Alock"].ToString()=="1")||(row["Alock"].ToString().ToLower()=="true"))
					{
						model.Alock=true;
					}
					else
					{
						model.Alock=false;
					}
				}
				if(row["Adate"]!=null && row["Adate"].ToString()!="")
				{
					model.Adate=DateTime.Parse(row["Adate"].ToString());
				}
			}
			return model;
		}
        /// <summary>
        /// 返回本次编程比赛排行名单及成绩  Asid,Sname,Sgrade,Sclass
        /// </summary>
        /// <param name="Amid"></param>
        /// <returns></returns>
        public DataTable GetListRank(int Amid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct Asid,Sname,Sgrade,Sclass from TurtleAnswer,Students ");
            strSql.Append(" where Amid=@Amid and Asid=Sid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Amid", SqlDbType.Int,4)
			};
            parameters[0].Value = Amid;

            DataTable dt = DbHelperSQL.Query(strSql.ToString(),parameters).Tables[0];
            dt.Columns.Add("Scores");
            int n = dt.Rows.Count;
            if (n > 0) { 
                for (int i=0;i<n;i++)
                {
                    string asid = dt.Rows[i]["Asid"].ToString();
                    string mysql = " select  sum(ascore) from TurtleAnswer where Amid=" + Amid.ToString() + " and Asid=" + asid;
                    int scores= DbHelperSQL.FindNum(mysql);
                    dt.Rows[i]["Scores"] = scores;
                }
            }

            DataView dv = dt.DefaultView;
            dv.Sort = "Scores desc";

            return dv.ToTable();
        }

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Aid,Amid,Aqid,Acode,Aimg,Aurl,Aout,Ascore,Asid,Asname,Alock,Adate ");
			strSql.Append(" FROM TurtleAnswer ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
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
			strSql.Append(" Aid,Amid,Aqid,Acode,Aimg,Aurl,Aout,Ascore,Asid,Asname,Alock,Adate ");
			strSql.Append(" FROM TurtleAnswer ");
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
			strSql.Append("select count(1) FROM TurtleAnswer ");
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
			strSql.Append(")AS Row, T.*  from TurtleAnswer T ");
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
			parameters[0].Value = "TurtleAnswer";
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

