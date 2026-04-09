using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using LearnSite.DBUtility;//Please add references
namespace LearnSite.DAL
{
	/// <summary>
	/// 数据访问类:Consoles
	/// </summary>
	public partial class Consoles
	{
		public Consoles()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Nid", "Consoles"); 
		}
        /// <summary>
        /// 标题
        /// </summary>
        /// <param name="Nid"></param>
        /// <returns></returns>
        public string GetTitle(int Nid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Ntitle from Consoles");
            strSql.Append(" where Nid=@Nid");
            SqlParameter[] parameters = {
					new SqlParameter("@Nid", SqlDbType.Int,4)
			};
            parameters[0].Value = Nid;

            return DbHelperSQL.FindString(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 标题
        /// </summary>
        /// <param name="Nid"></param>
        /// <returns></returns>
        public string GetCTitle(int Nid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Ctitle from Courses, Consoles");
            strSql.Append(" where Cid=Ncid and Nid=@Nid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Nid", SqlDbType.Int,4)
			};
            parameters[0].Value = Nid;

            return DbHelperSQL.FindString(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Nid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Consoles");
			strSql.Append(" where Nid=@Nid");
			SqlParameter[] parameters = {
					new SqlParameter("@Nid", SqlDbType.Int,4)
			};
			parameters[0].Value = Nid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LearnSite.Model.Consoles model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Consoles(");
			strSql.Append("Nhid,Ncid,Ntitle,Ncontent,Npublish,Ndate)");
			strSql.Append(" values (");
			strSql.Append("@Nhid,@Ncid,@Ntitle,@Ncontent,@Npublish,@Ndate)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Nhid", SqlDbType.Int,4),
					new SqlParameter("@Ncid", SqlDbType.Int,4),
					new SqlParameter("@Ntitle", SqlDbType.NVarChar,50),
					new SqlParameter("@Ncontent", SqlDbType.NText),
					new SqlParameter("@Npublish", SqlDbType.Bit,1),
					new SqlParameter("@Ndate", SqlDbType.DateTime)};
			parameters[0].Value = model.Nhid;
			parameters[1].Value = model.Ncid;
			parameters[2].Value = model.Ntitle;
			parameters[3].Value = model.Ncontent;
			parameters[4].Value = model.Npublish;
			parameters[5].Value = model.Ndate;

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

        public void InitNbegin() {

            string mysql = "update Consoles set Nbegin=1 where Nbegin is null ";
            DbHelperSQL.ExecuteSql(mysql);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool UpdateNbegin(int Nid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Consoles set ");
            strSql.Append("Nbegin=Nbegin^1");
            strSql.Append(" where Nid=@Nid");
            SqlParameter[] parameters = {
					new SqlParameter("@Nid", SqlDbType.Int,4)};

            parameters[0].Value = Nid;

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
        public bool UpdatePublish(int Nid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Consoles set ");
            strSql.Append("Npublish=Npublish^1");
            strSql.Append(" where Nid=@Nid");
            SqlParameter[] parameters = {
					new SqlParameter("@Nid", SqlDbType.Int,4)};

            parameters[0].Value = Nid;

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
		public bool Update(LearnSite.Model.Consoles model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Consoles set ");
			strSql.Append("Nhid=@Nhid,");
			strSql.Append("Ncid=@Ncid,");
			strSql.Append("Ntitle=@Ntitle,");
			strSql.Append("Ncontent=@Ncontent,");
			strSql.Append("Npublish=@Npublish,");
			strSql.Append("Ndate=@Ndate");
			strSql.Append(" where Nid=@Nid");
			SqlParameter[] parameters = {
					new SqlParameter("@Nhid", SqlDbType.Int,4),
					new SqlParameter("@Ncid", SqlDbType.Int,4),
					new SqlParameter("@Ntitle", SqlDbType.NVarChar,50),
					new SqlParameter("@Ncontent", SqlDbType.NText),
					new SqlParameter("@Npublish", SqlDbType.Bit,1),
					new SqlParameter("@Ndate", SqlDbType.DateTime),
					new SqlParameter("@Nid", SqlDbType.Int,4)};
			parameters[0].Value = model.Nhid;
			parameters[1].Value = model.Ncid;
			parameters[2].Value = model.Ntitle;
			parameters[3].Value = model.Ncontent;
			parameters[4].Value = model.Npublish;
			parameters[5].Value = model.Ndate;
			parameters[6].Value = model.Nid;

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
		public bool Delete(int Nid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Consoles ");
			strSql.Append(" where Nid=@Nid");
			SqlParameter[] parameters = {
					new SqlParameter("@Nid", SqlDbType.Int,4)
			};
			parameters[0].Value = Nid;

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
		public bool DeleteList(string Nidlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Consoles ");
			strSql.Append(" where Nid in ("+Nidlist + ")  ");
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
        public LearnSite.Model.Consoles GetModel(DataTable dt, int Tsort)
        {

            LearnSite.Model.Consoles model = new LearnSite.Model.Consoles();
            int Count = dt.Rows.Count;
            if (Count > 0)
            {
                if (Tsort < Count)
                {
                    if (dt.Rows[Tsort]["Nid"] != null && dt.Rows[Tsort]["Nid"].ToString() != "")
                    {
                        model.Nid = int.Parse(dt.Rows[Tsort]["Nid"].ToString());
                    }
                    if (dt.Rows[Tsort]["Nhid"] != null && dt.Rows[Tsort]["Nhid"].ToString() != "")
                    {
                        model.Nhid = int.Parse(dt.Rows[Tsort]["Nhid"].ToString());
                    }
                    if (dt.Rows[Tsort]["Ncid"] != null && dt.Rows[Tsort]["Ncid"].ToString() != "")
                    {
                        model.Ncid = int.Parse(dt.Rows[Tsort]["Ncid"].ToString());
                    }
                    if (dt.Rows[Tsort]["Ntitle"] != null && dt.Rows[Tsort]["Ntitle"].ToString() != "")
                    {
                        model.Ntitle = dt.Rows[Tsort]["Ntitle"].ToString();
                    }
                    if (dt.Rows[Tsort]["Ncontent"] != null && dt.Rows[Tsort]["Ncontent"].ToString() != "")
                    {
                        model.Ncontent = dt.Rows[Tsort]["Ncontent"].ToString();
                    }
                    if (dt.Rows[Tsort]["Npublish"] != null && dt.Rows[Tsort]["Npublish"].ToString() != "")
                    {
                        if ((dt.Rows[Tsort]["Npublish"].ToString() == "1") || (dt.Rows[Tsort]["Npublish"].ToString().ToLower() == "true"))
                        {
                            model.Npublish = true;
                        }
                        else
                        {
                            model.Npublish = false;
                        }
                    }
                    if (dt.Rows[Tsort]["Ndate"] != null && dt.Rows[Tsort]["Ndate"].ToString() != "")
                    {
                        model.Ndate = DateTime.Parse(dt.Rows[Tsort]["Ndate"].ToString());
                    }
                    return model;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LearnSite.Model.Consoles GetModel(int Nid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Nid,Nhid,Ncid,Ntitle,Ncontent,Npublish,Ndate,Nbegin from Consoles ");
			strSql.Append(" where Nid=@Nid");
			SqlParameter[] parameters = {
					new SqlParameter("@Nid", SqlDbType.Int,4)
			};
			parameters[0].Value = Nid;

			LearnSite.Model.Consoles model=new LearnSite.Model.Consoles();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["Nid"]!=null && ds.Tables[0].Rows[0]["Nid"].ToString()!="")
				{
					model.Nid=int.Parse(ds.Tables[0].Rows[0]["Nid"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Nhid"]!=null && ds.Tables[0].Rows[0]["Nhid"].ToString()!="")
				{
					model.Nhid=int.Parse(ds.Tables[0].Rows[0]["Nhid"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Ncid"]!=null && ds.Tables[0].Rows[0]["Ncid"].ToString()!="")
				{
					model.Ncid=int.Parse(ds.Tables[0].Rows[0]["Ncid"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Ntitle"]!=null && ds.Tables[0].Rows[0]["Ntitle"].ToString()!="")
				{
					model.Ntitle=ds.Tables[0].Rows[0]["Ntitle"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Ncontent"]!=null && ds.Tables[0].Rows[0]["Ncontent"].ToString()!="")
				{
					model.Ncontent=ds.Tables[0].Rows[0]["Ncontent"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Npublish"]!=null && ds.Tables[0].Rows[0]["Npublish"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["Npublish"].ToString()=="1")||(ds.Tables[0].Rows[0]["Npublish"].ToString().ToLower()=="true"))
					{
						model.Npublish=true;
					}
					else
					{
						model.Npublish=false;
					}
				}
				if(ds.Tables[0].Rows[0]["Ndate"]!=null && ds.Tables[0].Rows[0]["Ndate"].ToString()!="")
				{
					model.Ndate=DateTime.Parse(ds.Tables[0].Rows[0]["Ndate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Nbegin"] != null && ds.Tables[0].Rows[0]["Nbegin"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Nbegin"].ToString() == "1") || (ds.Tables[0].Rows[0]["Nbegin"].ToString().ToLower() == "true"))
                    {
                        model.Nbegin = true;
                    }
                    else
                    {
                        model.Nbegin = false;
                    }
                }
				return model;
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
			strSql.Append("select Nid,Nhid,Ncid,Ntitle,Ncontent,Npublish,Ndate ");
			strSql.Append(" FROM Consoles ");
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
			strSql.Append(" Nid,Nhid,Ncid,Ntitle,Ncontent,Npublish,Ndate ");
			strSql.Append(" FROM Consoles ");
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
			strSql.Append("select count(1) FROM Consoles ");
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
				strSql.Append("order by T.Nid desc");
			}
			strSql.Append(")AS Row, T.*  from Consoles T ");
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
			parameters[0].Value = "Consoles";
			parameters[1].Value = "Nid";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  Method
	}
}

