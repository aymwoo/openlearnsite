using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using LearnSite.DBUtility;//Please add references
namespace LearnSite.DAL
{
	/// <summary>
	/// 数据访问类:JudgeArg
	/// </summary>
	public partial class JudgeArg
	{
		public JudgeArg()
		{}
		#region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Jmid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from JudgeArg");
            strSql.Append(" where Jmid=@Jmid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Jmid", SqlDbType.Int,4)};
            parameters[0].Value = Jmid;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }
        public void initCid()
        {
            string mysql = " update JudgeArg set Jcid=Mcid from JudgeArg,Mission where Jmid=Mid ";
            DbHelperSQL.ExecuteSql(mysql);
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LearnSite.Model.JudgeArg model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into JudgeArg(");
            strSql.Append("Jhid,Jmid,Jsleep,Jinone,Jintwo,Jinthree,Joutone,Joutwo,Jouthree,Jright,Jcode,Jcid,Jimg,Jthumb)");
			strSql.Append(" values (");
            strSql.Append("@Jhid,@Jmid,@Jsleep,@Jinone,@Jintwo,@Jinthree,@Joutone,@Joutwo,@Jouthree,@Jright,@Jcode,@Jcid,@Jimg,@Jthumb)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Jhid", SqlDbType.Int,4),
					new SqlParameter("@Jmid", SqlDbType.Int,4),
					new SqlParameter("@Jsleep", SqlDbType.Int,4),
					new SqlParameter("@Jinone", SqlDbType.NVarChar,50),
					new SqlParameter("@Jintwo", SqlDbType.NVarChar,50),
					new SqlParameter("@Jinthree", SqlDbType.NVarChar,50),
					new SqlParameter("@Joutone", SqlDbType.NVarChar,200),
					new SqlParameter("@Joutwo", SqlDbType.NVarChar,200),
					new SqlParameter("@Jouthree", SqlDbType.NVarChar,200),
					new SqlParameter("@Jright", SqlDbType.Bit,1),
					new SqlParameter("@Jcode", SqlDbType.NText),
					new SqlParameter("@Jcid", SqlDbType.Int,4),
					new SqlParameter("@Jimg", SqlDbType.NVarChar,200),
					new SqlParameter("@Jthumb", SqlDbType.NVarChar,200)};
			parameters[0].Value = model.Jhid;
			parameters[1].Value = model.Jmid;
			parameters[2].Value = model.Jsleep;
			parameters[3].Value = model.Jinone;
			parameters[4].Value = model.Jintwo;
			parameters[5].Value = model.Jinthree;
			parameters[6].Value = model.Joutone;
			parameters[7].Value = model.Joutwo;
			parameters[8].Value = model.Jouthree;
			parameters[9].Value = model.Jright;
            parameters[10].Value = model.Jcode;
            parameters[11].Value = model.Jcid;
            parameters[12].Value = model.Jimg;
            parameters[13].Value = model.Jthumb;

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
		public bool Update(LearnSite.Model.JudgeArg model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update JudgeArg set ");
			strSql.Append("Jhid=@Jhid,");
			strSql.Append("Jmid=@Jmid,");
			strSql.Append("Jsleep=@Jsleep,");
			strSql.Append("Jinone=@Jinone,");
			strSql.Append("Jintwo=@Jintwo,");
			strSql.Append("Jinthree=@Jinthree,");
			strSql.Append("Joutone=@Joutone,");
			strSql.Append("Joutwo=@Joutwo,");
			strSql.Append("Jouthree=@Jouthree,");
			strSql.Append("Jright=@Jright,");
            strSql.Append("Jcode=@Jcode,");
            strSql.Append("Jimg=@Jimg,");
            strSql.Append("Jthumb=@Jthumb");
			strSql.Append(" where Jid=@Jid");
			SqlParameter[] parameters = {
					new SqlParameter("@Jhid", SqlDbType.Int,4),
					new SqlParameter("@Jmid", SqlDbType.Int,4),
					new SqlParameter("@Jsleep", SqlDbType.Int,4),
					new SqlParameter("@Jinone", SqlDbType.NVarChar,50),
					new SqlParameter("@Jintwo", SqlDbType.NVarChar,50),
					new SqlParameter("@Jinthree", SqlDbType.NVarChar,50),
					new SqlParameter("@Joutone", SqlDbType.NVarChar,200),
					new SqlParameter("@Joutwo", SqlDbType.NVarChar,200),
					new SqlParameter("@Jouthree", SqlDbType.NVarChar,200),
					new SqlParameter("@Jright", SqlDbType.Bit,1),
					new SqlParameter("@Jcode", SqlDbType.NText),
					new SqlParameter("@Jid", SqlDbType.Int,4),
					new SqlParameter("@Jimg", SqlDbType.NVarChar,200),
					new SqlParameter("@Jthumb", SqlDbType.NVarChar,200)};
			parameters[0].Value = model.Jhid;
			parameters[1].Value = model.Jmid;
			parameters[2].Value = model.Jsleep;
			parameters[3].Value = model.Jinone;
			parameters[4].Value = model.Jintwo;
			parameters[5].Value = model.Jinthree;
			parameters[6].Value = model.Joutone;
			parameters[7].Value = model.Joutwo;
			parameters[8].Value = model.Jouthree;
			parameters[9].Value = model.Jright;
			parameters[10].Value = model.Jcode;
            parameters[11].Value = model.Jid;
            parameters[12].Value = model.Jimg;
            parameters[13].Value = model.Jthumb;

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
		public bool Delete(int Jid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from JudgeArg ");
			strSql.Append(" where Jid=@Jid");
			SqlParameter[] parameters = {
					new SqlParameter("@Jid", SqlDbType.Int,4)
			};
			parameters[0].Value = Jid;

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
		public bool DeleteList(string Jidlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from JudgeArg ");
			strSql.Append(" where Jid in ("+Jidlist + ")  ");
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
		public LearnSite.Model.JudgeArg GetModel(int Jid)
		{
			
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select  top 1 Jid,Jhid,Jmid,Jsleep,Jinone,Jintwo,Jinthree,Joutone,Joutwo,Jouthree,Jright,Jcode,Jcid,Jimg,Jthumb from JudgeArg ");
			strSql.Append(" where Jid=@Jid");
			SqlParameter[] parameters = {
					new SqlParameter("@Jid", SqlDbType.Int,4)
			};
			parameters[0].Value = Jid;

			LearnSite.Model.JudgeArg model=new LearnSite.Model.JudgeArg();
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
        public int GetIdByMid(int Jmid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Jid from JudgeArg ");
            strSql.Append(" where Jmid=@Jmid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Jmid", SqlDbType.Int,4)
			};
            parameters[0].Value = Jmid;

            return DbHelperSQL.FindNum(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteJmid(int Jmid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from JudgeArg ");
            strSql.Append(" where Jmid=@Jmid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Jmid", SqlDbType.Int,4)
			};
            parameters[0].Value = Jmid;

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
        /// 得到一个对象实体
        /// </summary>
        public LearnSite.Model.JudgeArg GetModelByMid(int Jmid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Jid,Jhid,Jmid,Jsleep,Jinone,Jintwo,Jinthree,Joutone,Joutwo,Jouthree,Jright,Jcode,Jcid,Jimg,Jthumb from JudgeArg ");
            strSql.Append(" where Jmid=@Jmid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Jmid", SqlDbType.Int,4)
			};
            parameters[0].Value = Jmid;

            LearnSite.Model.JudgeArg model = new LearnSite.Model.JudgeArg();
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
        public LearnSite.Model.JudgeArg GetModel(DataTable dt, int Tsort)
        {            
            LearnSite.Model.JudgeArg model = new LearnSite.Model.JudgeArg();
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
		/// 得到一个对象实体
		/// </summary>
        public LearnSite.Model.JudgeArg DataRowToModel(DataRow row)
        {
            LearnSite.Model.JudgeArg model = new LearnSite.Model.JudgeArg();
            if (row != null)
            {
                if (row["Jid"] != null && row["Jid"].ToString() != "")
                {
                    model.Jid = int.Parse(row["Jid"].ToString());
                }
                if (row["Jhid"] != null && row["Jhid"].ToString() != "")
                {
                    model.Jhid = int.Parse(row["Jhid"].ToString());
                }
                if (row["Jmid"] != null && row["Jmid"].ToString() != "")
                {
                    model.Jmid = int.Parse(row["Jmid"].ToString());
                }
                if (row["Jsleep"] != null && row["Jsleep"].ToString() != "")
                {
                    model.Jsleep = int.Parse(row["Jsleep"].ToString());
                }
                if (row["Jinone"] != null)
                {
                    model.Jinone = row["Jinone"].ToString();
                }
                if (row["Jintwo"] != null)
                {
                    model.Jintwo = row["Jintwo"].ToString();
                }
                if (row["Jinthree"] != null)
                {
                    model.Jinthree = row["Jinthree"].ToString();
                }
                if (row["Joutone"] != null)
                {
                    model.Joutone = row["Joutone"].ToString();
                }
                if (row["Joutwo"] != null)
                {
                    model.Joutwo = row["Joutwo"].ToString();
                }
                if (row["Jouthree"] != null)
                {
                    model.Jouthree = row["Jouthree"].ToString();
                }
                if (row["Jright"] != null && row["Jright"].ToString() != "")
                {
                    if ((row["Jright"].ToString() == "1") || (row["Jright"].ToString().ToLower() == "true"))
                    {
                        model.Jright = true;
                    }
                    else
                    {
                        model.Jright = false;
                    }
                }
                if (row["Jcode"] != null)
                {
                    model.Jcode = row["Jcode"].ToString();
                }
                if (row["Jcid"] != null && row["Jcid"].ToString() != "")
                {
                    model.Jcid = int.Parse(row["Jcid"].ToString());
                }

                if (row["Jimg"] != null)
                {
                    model.Jimg = row["Jimg"].ToString();
                }

                if (row.Table.Columns.Contains("Jthumb"))
                {
                    if (row["Jthumb"] != null)
                    {
                        model.Jthumb = row["Jthumb"].ToString();
                    }
                }
                else
                {
                    model.Jthumb = "";
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
            strSql.Append("select Jid,Jhid,Jmid,Jsleep,Jinone,Jintwo,Jinthree,Joutone,Joutwo,Jouthree,Jright,Jcode,Jcid,Jimg,Jthumb ");
			strSql.Append(" FROM JudgeArg ");
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
            strSql.Append(" Jid,Jhid,Jmid,Jsleep,Jinone,Jintwo,Jinthree,Joutone,Joutwo,Jouthree,Jright,Jcode,Jcid,Jimg,Jthumb ");
			strSql.Append(" FROM JudgeArg ");
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
			strSql.Append("select count(1) FROM JudgeArg ");
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
				strSql.Append("order by T.Jid desc");
			}
			strSql.Append(")AS Row, T.*  from JudgeArg T ");
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
			parameters[0].Value = "JudgeArg";
			parameters[1].Value = "Jid";
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

