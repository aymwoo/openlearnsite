using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using LearnSite.DBUtility;//Please add references
namespace LearnSite.DAL
{
	/// <summary>
	/// 数据访问类:Game
	/// </summary>
	public partial class Game
	{
		public Game()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Gid", "Game"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Gid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Game");
			strSql.Append(" where Gid=@Gid");
			SqlParameter[] parameters = {
					new SqlParameter("@Gid", SqlDbType.Int,4)
			};
			parameters[0].Value = Gid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}



        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsSave(int Gsid,string Gtitle,int Gsave)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Game");
            strSql.Append(" where Gsid=@Gsid and Gtitle=@Gtitle and Gsave=@Gsave ");
            SqlParameter[] parameters = {
					new SqlParameter("@Gsid", SqlDbType.Int,4),
					new SqlParameter("@Gtitle", SqlDbType.NVarChar,50),
					new SqlParameter("@Gsave", SqlDbType.Int,4)};

            parameters[0].Value = Gsid;
            parameters[1].Value = Gtitle;
            parameters[2].Value = Gsave;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LearnSite.Model.Game model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Game(");
			strSql.Append("Gsid,Gsname,Gnum,Gtitle,Gsave,Gnote,Gscore,Gdate)");
			strSql.Append(" values (");
			strSql.Append("@Gsid,@Gsname,@Gnum,@Gtitle,@Gsave,@Gnote,@Gscore,@Gdate)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Gsid", SqlDbType.Int,4),
					new SqlParameter("@Gsname", SqlDbType.NVarChar,50),
					new SqlParameter("@Gnum", SqlDbType.Int,4),
					new SqlParameter("@Gtitle", SqlDbType.NVarChar,50),
					new SqlParameter("@Gsave", SqlDbType.Int,4),
					new SqlParameter("@Gnote", SqlDbType.NText),
					new SqlParameter("@Gscore", SqlDbType.Int,4),
					new SqlParameter("@Gdate", SqlDbType.DateTime)};
			parameters[0].Value = model.Gsid;
			parameters[1].Value = model.Gsname;
			parameters[2].Value = model.Gnum;
			parameters[3].Value = model.Gtitle;
			parameters[4].Value = model.Gsave;
			parameters[5].Value = model.Gnote;
			parameters[6].Value = model.Gscore;
			parameters[7].Value = model.Gdate;

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
        public bool UpdateSave(int Gsid, string Gtitle, int Gsave,string Gnote,int Gnum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Game set ");
            strSql.Append("Gnum=@Gnum,");
            strSql.Append("Gnote=@Gnote,");
            strSql.Append("Gdate=@Gdate");
            strSql.Append(" where Gsid=@Gsid and Gtitle=@Gtitle and Gsave=@Gsave ");
            SqlParameter[] parameters = {
					new SqlParameter("@Gsid", SqlDbType.Int,4),
					new SqlParameter("@Gnum", SqlDbType.Int,4),
					new SqlParameter("@Gtitle", SqlDbType.NVarChar,50),
					new SqlParameter("@Gsave", SqlDbType.Int,4),
					new SqlParameter("@Gnote", SqlDbType.NText),
					new SqlParameter("@Gdate", SqlDbType.DateTime)};
            parameters[0].Value = Gsid;
            parameters[1].Value = Gnum;
            parameters[2].Value = Gtitle;
            parameters[3].Value = Gsave;
            parameters[4].Value = Gnote;
            parameters[5].Value = DateTime.Now;

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
		/// 更新一条数据
		/// </summary>
		public bool Update(LearnSite.Model.Game model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Game set ");
			strSql.Append("Gsid=@Gsid,");
			strSql.Append("Gsname=@Gsname,");
			strSql.Append("Gnum=@Gnum,");
			strSql.Append("Gtitle=@Gtitle,");
			strSql.Append("Gsave=@Gsave,");
			strSql.Append("Gnote=@Gnote,");
			strSql.Append("Gscore=@Gscore,");
			strSql.Append("Gdate=@Gdate");
			strSql.Append(" where Gid=@Gid");
			SqlParameter[] parameters = {
					new SqlParameter("@Gsid", SqlDbType.Int,4),
					new SqlParameter("@Gsname", SqlDbType.NVarChar,50),
					new SqlParameter("@Gnum", SqlDbType.Int,4),
					new SqlParameter("@Gtitle", SqlDbType.NVarChar,50),
					new SqlParameter("@Gsave", SqlDbType.Int,4),
					new SqlParameter("@Gnote", SqlDbType.NText),
					new SqlParameter("@Gscore", SqlDbType.Int,4),
					new SqlParameter("@Gdate", SqlDbType.DateTime),
					new SqlParameter("@Gid", SqlDbType.Int,4)};
			parameters[0].Value = model.Gsid;
			parameters[1].Value = model.Gsname;
			parameters[2].Value = model.Gnum;
			parameters[3].Value = model.Gtitle;
			parameters[4].Value = model.Gsave;
			parameters[5].Value = model.Gnote;
			parameters[6].Value = model.Gscore;
			parameters[7].Value = model.Gdate;
			parameters[8].Value = model.Gid;

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
		public bool Delete(int Gid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Game ");
			strSql.Append(" where Gid=@Gid");
			SqlParameter[] parameters = {
					new SqlParameter("@Gid", SqlDbType.Int,4)
			};
			parameters[0].Value = Gid;

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
		public bool DeleteList(string Gidlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Game ");
			strSql.Append(" where Gid in ("+Gidlist + ")  ");
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
		public LearnSite.Model.Game GetModel(int Gid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Gid,Gsid,Gsname,Gnum,Gtitle,Gsave,Gnote,Gscore,Gdate from Game ");
			strSql.Append(" where Gid=@Gid");
			SqlParameter[] parameters = {
					new SqlParameter("@Gid", SqlDbType.Int,4)
			};
			parameters[0].Value = Gid;

			LearnSite.Model.Game model=new LearnSite.Model.Game();
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
        public LearnSite.Model.Game GetModelGameMax(int Gsid, string Gtitle)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Gid,Gsid,Gsname,Gnum,Gtitle,Gsave,Gnote,Gscore,Gdate from Game ");
            strSql.Append(" where Gsid=@Gsid and Gtitle=@Gtitle ");
            strSql.Append(" order by Gsave desc ");
            SqlParameter[] parameters = {
					new SqlParameter("@Gsid", SqlDbType.Int,4),
					new SqlParameter("@Gtitle", SqlDbType.NVarChar,50)};

            parameters[0].Value = Gsid;
            parameters[1].Value = Gtitle;

            LearnSite.Model.Game model = new LearnSite.Model.Game();
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
        public LearnSite.Model.Game GetModelGame(int Gsid, string Gtitle, int Gsave)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Gid,Gsid,Gsname,Gnum,Gtitle,Gsave,Gnote,Gscore,Gdate from Game ");
            strSql.Append(" where Gsid=@Gsid and Gtitle=@Gtitle and Gsave=@Gsave ");
            SqlParameter[] parameters = {
					new SqlParameter("@Gsid", SqlDbType.Int,4),
					new SqlParameter("@Gtitle", SqlDbType.NVarChar,50),
					new SqlParameter("@Gsave", SqlDbType.Int,4)};

            parameters[0].Value = Gsid;
            parameters[1].Value = Gtitle;
            parameters[2].Value = Gsave;

            LearnSite.Model.Game model = new LearnSite.Model.Game();
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
		public LearnSite.Model.Game DataRowToModel(DataRow row)
		{
			LearnSite.Model.Game model=new LearnSite.Model.Game();
			if (row != null)
			{
				if(row["Gid"]!=null && row["Gid"].ToString()!="")
				{
					model.Gid=int.Parse(row["Gid"].ToString());
				}
				if(row["Gsid"]!=null && row["Gsid"].ToString()!="")
				{
					model.Gsid=int.Parse(row["Gsid"].ToString());
				}
				if(row["Gsname"]!=null && row["Gsname"].ToString()!="")
				{
					model.Gsname=row["Gsname"].ToString();
				}
				if(row["Gnum"]!=null && row["Gnum"].ToString()!="")
				{
					model.Gnum=int.Parse(row["Gnum"].ToString());
				}
				if(row["Gtitle"]!=null)
				{
					model.Gtitle=row["Gtitle"].ToString();
				}
				if(row["Gsave"]!=null && row["Gsave"].ToString()!="")
				{
					model.Gsave=int.Parse(row["Gsave"].ToString());
				}
				if(row["Gnote"]!=null)
				{
					model.Gnote=row["Gnote"].ToString();
				}
				if(row["Gscore"]!=null && row["Gscore"].ToString()!="")
				{
					model.Gscore=int.Parse(row["Gscore"].ToString());
				}
				if(row["Gdate"]!=null && row["Gdate"].ToString()!="")
				{
					model.Gdate=DateTime.Parse(row["Gdate"].ToString());
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
			strSql.Append("select Gid,Gsid,Gsname,Gnum,Gtitle,Gsave,Gnote,Gscore,Gdate ");
			strSql.Append(" FROM Game ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetRank(int Top,string Gtitle)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct Gsname,Gsave,Sgrade,Sclass  ");
            strSql.Append(" FROM Game,Students Where  Gsid=Sid and Gtitle='" + Gtitle + "' and Gsave>9 ");
            strSql.Append(" order by Gsave DESC ");
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetWuziqiRank(int Top, string Gtitle)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Gsname,Gsave,Sgrade,Sclass,Gnum,Gdate ");
            strSql.Append(" FROM Game,Students Where Gscore=0 and Gsid=Sid and Gtitle='" + Gtitle + "' ");
            strSql.Append(" order by Gdate DESC, Gsave DESC ");
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
			strSql.Append(" Gid,Gsid,Gsname,Gnum,Gtitle,Gsave,Gnote,Gscore,Gdate ");
			strSql.Append(" FROM Game ");
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
			strSql.Append("select count(1) FROM Game ");
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
				strSql.Append("order by T.Gid desc");
			}
			strSql.Append(")AS Row, T.*  from Game T ");
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
			parameters[0].Value = "Game";
			parameters[1].Value = "Gid";
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

