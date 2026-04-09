using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using LearnSite.DBUtility;//Please add references
namespace LearnSite.DAL
{
	/// <summary>
	/// 数据访问类:TurtleQuestion
	/// </summary>
	public partial class TurtleQuestion
	{
		public TurtleQuestion()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Qid", "TurtleQuestion"); 
		}

        public bool updateQsort(int Qid, bool way)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update TurtleQuestion set ");
            if (way)
                strSql.Append("Qsort=Qsort+1");
            else
                strSql.Append("Qsort=Qsort-1");

            strSql.Append(" where Qid=@Qid");
            SqlParameter[] parameters = {
					new SqlParameter("@Qid", SqlDbType.Int,4)};
            parameters[0].Value = Qid;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
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
        /// 初始化序号
        /// </summary>
        /// <param name="Lcid"></param>
        public void Qsortnew(int Qmid)
        {
            string mysql = "select Qid from TurtleQuestion where Qmid=" + Qmid + " order by Qsort";
            DataTable dt = DbHelperSQL.Query(mysql).Tables[0];
            int cn = dt.Rows.Count;
            if (cn > 0)
            {
                StringBuilder sbSql = new StringBuilder();
                for (int i = 0; i < cn; i++)
                {
                    string Qid = dt.Rows[i][0].ToString();
                    int ps = i + 1;
                    sbSql.AppendFormat("update TurtleQuestion set Qsort={0} where Qid={1};", ps, Qid);
                }
                DbHelperSQL.ExecuteSql(sbSql.ToString());
            }
        }

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Qid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from TurtleQuestion");
			strSql.Append(" where Qid=@Qid");
			SqlParameter[] parameters = {
					new SqlParameter("@Qid", SqlDbType.Int,4)
			};
			parameters[0].Value = Qid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LearnSite.Model.TurtleQuestion model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into TurtleQuestion(");
			strSql.Append("Qmid,Qtitle,Qcontent,Qdegree,Qsort,Qcode,Qimg,Qurl,Qout,Qscore,Qdate)");
			strSql.Append(" values (");
			strSql.Append("@Qmid,@Qtitle,@Qcontent,@Qdegree,@Qsort,@Qcode,@Qimg,@Qurl,@Qout,@Qscore,@Qdate)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Qmid", SqlDbType.Int,4),
					new SqlParameter("@Qtitle", SqlDbType.NVarChar,50),
					new SqlParameter("@Qcontent", SqlDbType.NText),
					new SqlParameter("@Qdegree", SqlDbType.Int,4),
					new SqlParameter("@Qsort", SqlDbType.Int,4),
					new SqlParameter("@Qcode", SqlDbType.NText),
					new SqlParameter("@Qimg", SqlDbType.NVarChar,50),
					new SqlParameter("@Qurl", SqlDbType.NVarChar,50),
					new SqlParameter("@Qout", SqlDbType.NVarChar,200),
					new SqlParameter("@Qscore", SqlDbType.Int,4),
					new SqlParameter("@Qdate", SqlDbType.DateTime)};
			parameters[0].Value = model.Qmid;
			parameters[1].Value = model.Qtitle;
			parameters[2].Value = model.Qcontent;
			parameters[3].Value = model.Qdegree;
			parameters[4].Value = model.Qsort;
			parameters[5].Value = model.Qcode;
			parameters[6].Value = model.Qimg;
			parameters[7].Value = model.Qurl;
			parameters[8].Value = model.Qout;
			parameters[9].Value = model.Qscore;
			parameters[10].Value = model.Qdate;

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
		public bool Update(LearnSite.Model.TurtleQuestion model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update TurtleQuestion set ");
			strSql.Append("Qmid=@Qmid,");
			strSql.Append("Qtitle=@Qtitle,");
			strSql.Append("Qcontent=@Qcontent,");
			strSql.Append("Qdegree=@Qdegree,");
			strSql.Append("Qsort=@Qsort,");
			strSql.Append("Qcode=@Qcode,");
			strSql.Append("Qimg=@Qimg,");
			strSql.Append("Qurl=@Qurl,");
			strSql.Append("Qout=@Qout,");
			strSql.Append("Qscore=@Qscore,");
			strSql.Append("Qdate=@Qdate");
			strSql.Append(" where Qid=@Qid");
			SqlParameter[] parameters = {
					new SqlParameter("@Qmid", SqlDbType.Int,4),
					new SqlParameter("@Qtitle", SqlDbType.NVarChar,50),
					new SqlParameter("@Qcontent", SqlDbType.NText),
					new SqlParameter("@Qdegree", SqlDbType.Int,4),
					new SqlParameter("@Qsort", SqlDbType.Int,4),
					new SqlParameter("@Qcode", SqlDbType.NText),
					new SqlParameter("@Qimg", SqlDbType.NVarChar,50),
					new SqlParameter("@Qurl", SqlDbType.NVarChar,50),
					new SqlParameter("@Qout", SqlDbType.NVarChar,200),
					new SqlParameter("@Qscore", SqlDbType.Int,4),
					new SqlParameter("@Qdate", SqlDbType.DateTime),
					new SqlParameter("@Qid", SqlDbType.Int,4)};
			parameters[0].Value = model.Qmid;
			parameters[1].Value = model.Qtitle;
			parameters[2].Value = model.Qcontent;
			parameters[3].Value = model.Qdegree;
			parameters[4].Value = model.Qsort;
			parameters[5].Value = model.Qcode;
			parameters[6].Value = model.Qimg;
			parameters[7].Value = model.Qurl;
			parameters[8].Value = model.Qout;
			parameters[9].Value = model.Qscore;
			parameters[10].Value = model.Qdate;
			parameters[11].Value = model.Qid;

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
		public bool Delete(int Qid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from TurtleQuestion ");
			strSql.Append(" where Qid=@Qid");
			SqlParameter[] parameters = {
					new SqlParameter("@Qid", SqlDbType.Int,4)
			};
			parameters[0].Value = Qid;

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
		public bool DeleteList(string Qidlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from TurtleQuestion ");
			strSql.Append(" where Qid in ("+Qidlist + ")  ");
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
		public LearnSite.Model.TurtleQuestion GetModel(int Qid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Qid,Qmid,Qtitle,Qcontent,Qdegree,Qsort,Qcode,Qimg,Qurl,Qout,Qscore,Qdate from TurtleQuestion ");
			strSql.Append(" where Qid=@Qid");
			SqlParameter[] parameters = {
					new SqlParameter("@Qid", SqlDbType.Int,4)
			};
			parameters[0].Value = Qid;

			LearnSite.Model.TurtleQuestion model=new LearnSite.Model.TurtleQuestion();
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
		public LearnSite.Model.TurtleQuestion DataRowToModel(DataRow row)
		{
			LearnSite.Model.TurtleQuestion model=new LearnSite.Model.TurtleQuestion();
			if (row != null)
			{
				if(row["Qid"]!=null && row["Qid"].ToString()!="")
				{
					model.Qid=int.Parse(row["Qid"].ToString());
				}
				if(row["Qmid"]!=null && row["Qmid"].ToString()!="")
				{
					model.Qmid=int.Parse(row["Qmid"].ToString());
				}
				if(row["Qtitle"]!=null)
				{
					model.Qtitle=row["Qtitle"].ToString();
				}
				if(row["Qcontent"]!=null)
				{
					model.Qcontent=row["Qcontent"].ToString();
				}
				if(row["Qdegree"]!=null && row["Qdegree"].ToString()!="")
				{
					model.Qdegree=int.Parse(row["Qdegree"].ToString());
				}
				if(row["Qsort"]!=null && row["Qsort"].ToString()!="")
				{
					model.Qsort=int.Parse(row["Qsort"].ToString());
				}
				if(row["Qcode"]!=null)
				{
					model.Qcode=row["Qcode"].ToString();
				}
				if(row["Qimg"]!=null)
				{
					model.Qimg=row["Qimg"].ToString();
				}
				if(row["Qurl"]!=null)
				{
					model.Qurl=row["Qurl"].ToString();
				}
				if(row["Qout"]!=null)
				{
					model.Qout=row["Qout"].ToString();
				}
				if(row["Qscore"]!=null && row["Qscore"].ToString()!="")
				{
					model.Qscore=int.Parse(row["Qscore"].ToString());
				}
				if(row["Qdate"]!=null && row["Qdate"].ToString()!="")
				{
					model.Qdate=DateTime.Parse(row["Qdate"].ToString());
				}
			}
			return model;
		}


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LearnSite.Model.TurtleQuestion GetModel(DataTable dt, int Tsort)
        {
            LearnSite.Model.TurtleQuestion model = new LearnSite.Model.TurtleQuestion();
            int Count = dt.Rows.Count;
            if (Count > 0)
            {
                if (Tsort < Count)
                {
                    return DataRowToModel(dt.Rows[Tsort]);
                }
                return null;
            }
            else
            {
                return null;
            }
        }

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Qid,Qmid,Qtitle,Qcontent,Qdegree,Qsort,Qcode,Qimg,Qurl,Qout,Qscore,Qdate ");
			strSql.Append(" FROM TurtleQuestion ");
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
			strSql.Append(" Qid,Qmid,Qtitle,Qcontent,Qdegree,Qsort,Qcode,Qimg,Qurl,Qout,Qscore,Qdate ");
			strSql.Append(" FROM TurtleQuestion ");
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
			strSql.Append("select count(1) FROM TurtleQuestion ");
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
				strSql.Append("order by T.Qid desc");
			}
			strSql.Append(")AS Row, T.*  from TurtleQuestion T ");
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
			parameters[0].Value = "TurtleQuestion";
			parameters[1].Value = "Qid";
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

