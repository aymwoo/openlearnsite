using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using LearnSite.DBUtility;//Please add references
namespace LearnSite.DAL
{
	/// <summary>
	/// 数据访问类:TurtleMatch
	/// </summary>
	public partial class TurtleMatch
	{
		public TurtleMatch()
		{}
		#region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsOwer(int Mid,int Mhid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from TurtleMatch");
            strSql.Append(" where Mid=@Mid and Mhid=@Mhid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Mid", SqlDbType.Int,4),                    
					new SqlParameter("@Mhid", SqlDbType.Int,4)
			};
            parameters[0].Value = Mid;
            parameters[1].Value = Mhid;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LearnSite.Model.TurtleMatch model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into TurtleMatch(");
			strSql.Append("Mhid,Mtitle,Mcontent,Mbegin,Mend,Mpublish,Mdate)");
			strSql.Append(" values (");
			strSql.Append("@Mhid,@Mtitle,@Mcontent,@Mbegin,@Mend,@Mpublish,@Mdate)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Mhid", SqlDbType.Int,4),
					new SqlParameter("@Mtitle", SqlDbType.NVarChar,50),
					new SqlParameter("@Mcontent", SqlDbType.NText),
					new SqlParameter("@Mbegin", SqlDbType.DateTime),
					new SqlParameter("@Mend", SqlDbType.DateTime),
					new SqlParameter("@Mpublish", SqlDbType.Bit,1),
					new SqlParameter("@Mdate", SqlDbType.DateTime)};
			parameters[0].Value = model.Mhid;
			parameters[1].Value = model.Mtitle;
			parameters[2].Value = model.Mcontent;
			parameters[3].Value = model.Mbegin;
			parameters[4].Value = model.Mend;
			parameters[5].Value = model.Mpublish;
			parameters[6].Value = model.Mdate;

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
		public bool Update(LearnSite.Model.TurtleMatch model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update TurtleMatch set ");
			strSql.Append("Mhid=@Mhid,");
			strSql.Append("Mtitle=@Mtitle,");
			strSql.Append("Mcontent=@Mcontent,");
			strSql.Append("Mbegin=@Mbegin,");
			strSql.Append("Mend=@Mend,");
			strSql.Append("Mpublish=@Mpublish,");
			strSql.Append("Mdate=@Mdate");
			strSql.Append(" where Mid=@Mid");
			SqlParameter[] parameters = {
					new SqlParameter("@Mhid", SqlDbType.Int,4),
					new SqlParameter("@Mtitle", SqlDbType.NVarChar,50),
					new SqlParameter("@Mcontent", SqlDbType.NText),
					new SqlParameter("@Mbegin", SqlDbType.DateTime),
					new SqlParameter("@Mend", SqlDbType.DateTime),
					new SqlParameter("@Mpublish", SqlDbType.Bit,1),
					new SqlParameter("@Mdate", SqlDbType.DateTime),
					new SqlParameter("@Mid", SqlDbType.Int,4)};
			parameters[0].Value = model.Mhid;
			parameters[1].Value = model.Mtitle;
			parameters[2].Value = model.Mcontent;
			parameters[3].Value = model.Mbegin;
			parameters[4].Value = model.Mend;
			parameters[5].Value = model.Mpublish;
			parameters[6].Value = model.Mdate;
			parameters[7].Value = model.Mid;

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
		public bool Delete(int Mid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from TurtleMatch ");
			strSql.Append(" where Mid=@Mid");
			SqlParameter[] parameters = {
					new SqlParameter("@Mid", SqlDbType.Int,4)
			};
			parameters[0].Value = Mid;

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
		public bool DeleteList(string Midlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from TurtleMatch ");
			strSql.Append(" where Mid in ("+Midlist + ")  ");
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
		public LearnSite.Model.TurtleMatch GetModel(int Mid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Mid,Mhid,Mtitle,Mcontent,Mbegin,Mend,Mpublish,Mdate from TurtleMatch ");
			strSql.Append(" where Mid=@Mid");
			SqlParameter[] parameters = {
					new SqlParameter("@Mid", SqlDbType.Int,4)
			};
			parameters[0].Value = Mid;

			LearnSite.Model.TurtleMatch model=new LearnSite.Model.TurtleMatch();
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
		public LearnSite.Model.TurtleMatch DataRowToModel(DataRow row)
		{
			LearnSite.Model.TurtleMatch model=new LearnSite.Model.TurtleMatch();
			if (row != null)
			{
				if(row["Mid"]!=null && row["Mid"].ToString()!="")
				{
					model.Mid=int.Parse(row["Mid"].ToString());
				}
				if(row["Mhid"]!=null && row["Mhid"].ToString()!="")
				{
					model.Mhid=int.Parse(row["Mhid"].ToString());
				}
				if(row["Mtitle"]!=null)
				{
					model.Mtitle=row["Mtitle"].ToString();
				}
				if(row["Mcontent"]!=null)
				{
					model.Mcontent=row["Mcontent"].ToString();
				}
				if(row["Mbegin"]!=null && row["Mbegin"].ToString()!="")
				{
					model.Mbegin=DateTime.Parse(row["Mbegin"].ToString());
				}
				if(row["Mend"]!=null && row["Mend"].ToString()!="")
				{
					model.Mend=DateTime.Parse(row["Mend"].ToString());
				}
				if(row["Mpublish"]!=null && row["Mpublish"].ToString()!="")
				{
					if((row["Mpublish"].ToString()=="1")||(row["Mpublish"].ToString().ToLower()=="true"))
					{
						model.Mpublish=true;
					}
					else
					{
						model.Mpublish=false;
					}
				}
				if(row["Mdate"]!=null && row["Mdate"].ToString()!="")
				{
					model.Mdate=DateTime.Parse(row["Mdate"].ToString());
				}
			}
			return model;
		}

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LearnSite.Model.TurtleMatch GetModel(DataTable dt, int Tsort)
        {
            LearnSite.Model.TurtleMatch model = new LearnSite.Model.TurtleMatch();
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
			strSql.Append("select Mid,Mhid,Mtitle,Mcontent,Mbegin,Mend,Mpublish,Mdate ");
			strSql.Append(" FROM TurtleMatch ");
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
			strSql.Append(" Mid,Mhid,Mtitle,Mcontent,Mbegin,Mend,Mpublish,Mdate ");
			strSql.Append(" FROM TurtleMatch ");
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
			strSql.Append("select count(1) FROM TurtleMatch ");
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
				strSql.Append("order by T.Mid desc");
			}
			strSql.Append(")AS Row, T.*  from TurtleMatch T ");
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
			parameters[0].Value = "TurtleMatch";
			parameters[1].Value = "Mid";
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

