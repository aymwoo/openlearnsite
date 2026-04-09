using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using LearnSite.DBUtility;//Please add references
namespace LearnSite.DAL
{
	/// <summary>
	/// 数据访问类:Problems
	/// </summary>
	public partial class Problems
	{
		public Problems()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Pid", "Problems"); 
		}

        public void init()
        {
            string mysql = " update Problems set Psort=Pid ";
            DbHelperSQL.ExecuteSql(mysql);
        }
        public void initCid()
        {
            string mysql = " update Problems set Pcid=Ncid from Problems,Consoles where Pnid=Nid ";
            DbHelperSQL.ExecuteSql(mysql);
        }
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Pid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Problems");
			strSql.Append(" where Pid=@Pid");
			SqlParameter[] parameters = {
					new SqlParameter("@Pid", SqlDbType.Int,4)
			};
			parameters[0].Value = Pid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}

        public bool updatePsort(int Pid,bool way)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Problems set ");
            if(way)
                strSql.Append("Psort=Psort+1");
            else
                strSql.Append("Psort=Psort-1");

            strSql.Append(" where Pid=@Pid");
            SqlParameter[] parameters = {
					new SqlParameter("@Pid", SqlDbType.Int,4)};
            parameters[0].Value  = Pid;

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
        public void Psortnew(int Pnid)
        {
            string mysql = "select Pid from Problems where Pnid=" + Pnid + " order by Psort";
            DataTable dt = DbHelperSQL.Query(mysql).Tables[0];
            int cn = dt.Rows.Count;
            if (cn > 0)
            {
                StringBuilder strSql = new StringBuilder();
                for (int i = 0; i < cn; i++)
                {
                    string Pid = dt.Rows[i][0].ToString();
                    int ps = i + 1;
                    strSql.AppendFormat("update Problems set Psort={0} where Pid={1};", ps, Pid);
                }
                if (strSql.Length > 0)
                {
                    DbHelperSQL.ExecuteSql(strSql.ToString());
                }
            }
        }

        /// <summary>
        /// 统计记录数
        /// </summary>
        public int Pcount(int Pnid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select max(Psort) from Problems");
            strSql.Append(" where Pnid=@Pnid");
            SqlParameter[] parameters = {
					new SqlParameter("@Pnid", SqlDbType.Int,4)
			};
            parameters[0].Value = Pnid;

            return DbHelperSQL.FindNum(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LearnSite.Model.Problems model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Problems(");
			strSql.Append("Phid,Pnid,Ptitle,Pcode,Pouput,Pscore,Pdate,Psort,Pcid)");
			strSql.Append(" values (");
            strSql.Append("@Phid,@Pnid,@Ptitle,@Pcode,@Pouput,@Pscore,@Pdate,@Psort,@Pcid)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Phid", SqlDbType.Int,4),
					new SqlParameter("@Pnid", SqlDbType.Int,4),
					new SqlParameter("@Ptitle", SqlDbType.NVarChar,200),
					new SqlParameter("@Pcode", SqlDbType.NVarChar,200),
					new SqlParameter("@Pouput", SqlDbType.NVarChar,200),
					new SqlParameter("@Pscore", SqlDbType.Int,4),
					new SqlParameter("@Pdate", SqlDbType.DateTime),
					new SqlParameter("@Psort", SqlDbType.Int,4),
					new SqlParameter("@Pcid", SqlDbType.Int,4)};
			parameters[0].Value = model.Phid;
			parameters[1].Value = model.Pnid;
			parameters[2].Value = model.Ptitle;
			parameters[3].Value = model.Pcode;
			parameters[4].Value = model.Pouput;
			parameters[5].Value = model.Pscore;
            parameters[6].Value = model.Pdate;
            parameters[7].Value = model.Psort;
            parameters[8].Value = model.Pcid;

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
        public bool UpdateProblem(LearnSite.Model.Problems model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Problems set ");
            strSql.Append("Ptitle=@Ptitle,");
            strSql.Append("Pcode=@Pcode,");
            strSql.Append("Pouput=@Pouput,");
            strSql.Append("Pscore=@Pscore,");
            strSql.Append("Pdate=@Pdate");
            strSql.Append(" where Pid=@Pid");
            SqlParameter[] parameters = {
					new SqlParameter("@Ptitle", SqlDbType.NVarChar,200),
					new SqlParameter("@Pcode", SqlDbType.NVarChar,200),
					new SqlParameter("@Pouput", SqlDbType.NVarChar,200),
					new SqlParameter("@Pscore", SqlDbType.Int,4),
					new SqlParameter("@Pdate", SqlDbType.DateTime),
					new SqlParameter("@Pid", SqlDbType.Int,4)};
            parameters[0].Value = model.Ptitle;
            parameters[1].Value = model.Pcode;
            parameters[2].Value = model.Pouput;
            parameters[3].Value = model.Pscore;
            parameters[4].Value = model.Pdate;
            parameters[5].Value = model.Pid;

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
		public bool Update(LearnSite.Model.Problems model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Problems set ");
			strSql.Append("Phid=@Phid,");
			strSql.Append("Pnid=@Pnid,");
			strSql.Append("Ptitle=@Ptitle,");
			strSql.Append("Pcode=@Pcode,");
			strSql.Append("Pouput=@Pouput,");
			strSql.Append("Pscore=@Pscore,");
			strSql.Append("Pdate=@Pdate");
			strSql.Append(" where Pid=@Pid");
			SqlParameter[] parameters = {
					new SqlParameter("@Phid", SqlDbType.Int,4),
					new SqlParameter("@Pnid", SqlDbType.Int,4),
					new SqlParameter("@Ptitle", SqlDbType.NVarChar,200),
					new SqlParameter("@Pcode", SqlDbType.NVarChar,200),
					new SqlParameter("@Pouput", SqlDbType.NVarChar,200),
					new SqlParameter("@Pscore", SqlDbType.Int,4),
					new SqlParameter("@Pdate", SqlDbType.DateTime),
					new SqlParameter("@Pid", SqlDbType.Int,4)};
			parameters[0].Value = model.Phid;
			parameters[1].Value = model.Pnid;
			parameters[2].Value = model.Ptitle;
			parameters[3].Value = model.Pcode;
			parameters[4].Value = model.Pouput;
			parameters[5].Value = model.Pscore;
			parameters[6].Value = model.Pdate;
			parameters[7].Value = model.Pid;

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
		public bool Delete(int Pid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Problems ");
			strSql.Append(" where Pid=@Pid");
			SqlParameter[] parameters = {
					new SqlParameter("@Pid", SqlDbType.Int,4)
			};
			parameters[0].Value = Pid;

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
		public bool DeleteList(string Pidlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Problems ");
			strSql.Append(" where Pid in ("+Pidlist + ")  ");
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
        public LearnSite.Model.Problems GetModel(DataTable dt, int Tsort)
        {
            LearnSite.Model.Problems model = new LearnSite.Model.Problems();
            int Count = dt.Rows.Count;
            if (Count > 0)
            {
                if (Tsort < Count)
                {
                    if (dt.Rows[Tsort]["Pid"] != null && dt.Rows[Tsort]["Pid"].ToString() != "")
                    {
                        model.Pid = int.Parse(dt.Rows[Tsort]["Pid"].ToString());
                    }
                    if (dt.Rows[Tsort]["Phid"] != null && dt.Rows[Tsort]["Phid"].ToString() != "")
                    {
                        model.Phid = int.Parse(dt.Rows[Tsort]["Phid"].ToString());
                    }
                    if (dt.Rows[Tsort]["Pnid"] != null && dt.Rows[Tsort]["Pnid"].ToString() != "")
                    {
                        model.Pnid = int.Parse(dt.Rows[Tsort]["Pnid"].ToString());
                    }
                    if (dt.Rows[Tsort]["Ptitle"] != null && dt.Rows[Tsort]["Ptitle"].ToString() != "")
                    {
                        model.Ptitle = dt.Rows[Tsort]["Ptitle"].ToString();
                    }
                    if (dt.Rows[Tsort]["Pcode"] != null && dt.Rows[Tsort]["Pcode"].ToString() != "")
                    {
                        model.Pcode = dt.Rows[Tsort]["Pcode"].ToString();
                    }
                    if (dt.Rows[Tsort]["Pouput"] != null && dt.Rows[Tsort]["Pouput"].ToString() != "")
                    {
                        model.Pouput = dt.Rows[Tsort]["Pouput"].ToString();
                    }
                    else {
                        model.Pouput = "";                    
                    }
                    if (dt.Rows[Tsort]["Pscore"] != null && dt.Rows[Tsort]["Pscore"].ToString() != "")
                    {
                        model.Pscore = int.Parse(dt.Rows[Tsort]["Pscore"].ToString());
                    }
                    if (dt.Rows[Tsort]["Pdate"] != null && dt.Rows[Tsort]["Pdate"].ToString() != "")
                    {
                        model.Pdate = DateTime.Parse(dt.Rows[Tsort]["Pdate"].ToString());
                    }
                    if (dt.Columns.Contains("Psort") && dt.Rows[Tsort]["Psort"] != null && dt.Rows[Tsort]["Psort"].ToString() != "")
                    {
                        model.Psort = int.Parse(dt.Rows[Tsort]["Psort"].ToString());
                    }
                    else
                    {
                        model.Psort = 0;
                    }

                    if (dt.Rows[Tsort]["Pcid"] != null && dt.Rows[Tsort]["Pcid"].ToString() != "")
                    {
                        model.Pcid = int.Parse(dt.Rows[Tsort]["Pcid"].ToString());
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
		public LearnSite.Model.Problems GetModel(int Pid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Pid,Phid,Pnid,Ptitle,Pcode,Pouput,Pscore,Pdate from Problems ");
			strSql.Append(" where Pid=@Pid");
			SqlParameter[] parameters = {
					new SqlParameter("@Pid", SqlDbType.Int,4)
			};
			parameters[0].Value = Pid;

			LearnSite.Model.Problems model=new LearnSite.Model.Problems();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["Pid"]!=null && ds.Tables[0].Rows[0]["Pid"].ToString()!="")
				{
					model.Pid=int.Parse(ds.Tables[0].Rows[0]["Pid"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Phid"]!=null && ds.Tables[0].Rows[0]["Phid"].ToString()!="")
				{
					model.Phid=int.Parse(ds.Tables[0].Rows[0]["Phid"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Pnid"]!=null && ds.Tables[0].Rows[0]["Pnid"].ToString()!="")
				{
					model.Pnid=int.Parse(ds.Tables[0].Rows[0]["Pnid"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Ptitle"]!=null && ds.Tables[0].Rows[0]["Ptitle"].ToString()!="")
				{
					model.Ptitle=ds.Tables[0].Rows[0]["Ptitle"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Pcode"]!=null && ds.Tables[0].Rows[0]["Pcode"].ToString()!="")
				{
					model.Pcode=ds.Tables[0].Rows[0]["Pcode"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Pouput"]!=null && ds.Tables[0].Rows[0]["Pouput"].ToString()!="")
				{
					model.Pouput=ds.Tables[0].Rows[0]["Pouput"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Pscore"]!=null && ds.Tables[0].Rows[0]["Pscore"].ToString()!="")
				{
					model.Pscore=int.Parse(ds.Tables[0].Rows[0]["Pscore"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Pdate"]!=null && ds.Tables[0].Rows[0]["Pdate"].ToString()!="")
				{
					model.Pdate=DateTime.Parse(ds.Tables[0].Rows[0]["Pdate"].ToString());
				}
				return model;
			}
			else
			{
				return null;
			}
		}
        /// <summary>
        /// 获得Nid数据列表
        /// Pid,Ptitle,Pcode,Pouput,Pscore
        /// </summary>
        public string GetListNidjson(int Nid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Pid,Ptitle,Pcode,Pouput,Pscore");
            strSql.Append(" FROM Problems ");
            strSql.Append(" where Pnid=@Pnid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Pnid", SqlDbType.Int,4)
			};
            parameters[0].Value = Nid;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
            return json;
        }
        /// <summary>
        /// 获得Nid数据列表
        /// </summary>
        public DataTable GetListNidTable(int Nid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Pid ");
            strSql.Append(" FROM Problems ");
            strSql.Append(" where Pnid=@Pnid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Pnid", SqlDbType.Int,4)
			};
            parameters[0].Value = Nid;

            return DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
        }

        /// <summary>
        /// 获得Nid数据列表
        /// </summary>
        public DataSet GetListNid(int Nid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Pid,Phid,Pnid,Ptitle,Pcode,Pouput,Pscore,Pdate,Psort ");
            strSql.Append(" FROM Problems ");
            strSql.Append(" where Pnid=@Pnid ");
            strSql.Append(" order by Psort ,Pid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Pnid", SqlDbType.Int,4)
			};
            parameters[0].Value = Nid;

            return DbHelperSQL.Query(strSql.ToString(),parameters);
        }

        /// <summary>
        /// 获得Nid数据列表
        /// </summary>
        public DataTable GetListNidSid(int Nid, int Sid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Pid,Pscore ");
            strSql.Append(" FROM Problems ");
            strSql.Append(" where Pnid=@Pnid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Pnid", SqlDbType.Int,4)
			};
            parameters[0].Value = Nid;
            DataTable dt = DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
            dt.Columns.Add("Vscore");
            int len = dt.Rows.Count;
            BLL.Solves bll = new BLL.Solves();
            for (int i = 0; i < len; i++)
            {
                int pid = Int32.Parse(dt.Rows[i]["Pid"].ToString());
                string score = bll.GetScore(pid, Sid);
                dt.Rows[i]["Vscore"] = score;
            }

            return dt;
        }

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Pid,Phid,Pnid,Ptitle,Pcode,Pouput,Pscore,Pdate ");
			strSql.Append(" FROM Problems ");
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
			strSql.Append(" Pid,Phid,Pnid,Ptitle,Pcode,Pouput,Pscore,Pdate ");
			strSql.Append(" FROM Problems ");
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
			strSql.Append("select count(1) FROM Problems ");
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
				strSql.Append("order by T.Pid desc");
			}
			strSql.Append(")AS Row, T.*  from Problems T ");
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
			parameters[0].Value = "Problems";
			parameters[1].Value = "Pid";
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

