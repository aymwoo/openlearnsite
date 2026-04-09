using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using LearnSite.DBUtility;//Please add references
namespace LearnSite.DAL
{
	/// <summary>
	/// 数据访问类:Solves
	/// </summary>
	public partial class Solves
	{
		public Solves()
		{}
		#region  Method

        public void updatescore()
        {
            string mysql = "update Solves set Vscore=Pscore  from Problems where Vpid=Pid";
            DbHelperSQL.ExecuteSql(mysql);        
        }


        public void updateclass()
        {
            string SolvesTable = "Solves";
            string Vclassstr = "Vclass";
            if (DbHelperSQL.ColumnExists(SolvesTable, Vclassstr))
            {
                string strsql1 = "update Solves set Vclass=Sclass from Solves,Students where Vsid=Sid";
                DbHelperSQL.ExecuteSql(strsql1);
            }

        }

        public void init()
        {
            string strsql1 = "update Solves set Vyear=Syear,Vclass=Sclass from Solves,Students where Vsid=Sid";
            DbHelperSQL.ExecuteSql(strsql1);
            string strsql2 = "update Solves set Vnid=Pnid from Solves,Problems where Vpid=Pid";
            DbHelperSQL.ExecuteSql(strsql2);
            string strsql3 = "update Solves set Vcid=Ncid from Solves,Problems,Consoles where Vpid=Pid and Pnid=Nid ";
            DbHelperSQL.ExecuteSql(strsql3);
        }

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Vid", "Solves"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Vid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Solves");
			strSql.Append(" where Vid=@Vid");
			SqlParameter[] parameters = {
					new SqlParameter("@Vid", SqlDbType.Int,4)
			};
			parameters[0].Value = Vid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public string GetScore(int Vpid, int Vsid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Vscore ");
            strSql.Append(" FROM Solves ");
            strSql.Append(" where Vpid=@Vpid and Vsid=@Vsid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Vpid", SqlDbType.Int,4),
					new SqlParameter("@Vsid", SqlDbType.Int,4)
			};
            parameters[0].Value = Vpid;
            parameters[1].Value = Vsid;

            return DbHelperSQL.FindString(strSql.ToString(),parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsPidSid(int Vpid,int Vsid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Solves");
            strSql.Append(" where Vpid=@Vpid and Vsid=@Vsid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Vpid", SqlDbType.Int,4),
					new SqlParameter("@Vsid", SqlDbType.Int,4)
			};
            parameters[0].Value = Vpid;
            parameters[1].Value = Vsid;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LearnSite.Model.Solves model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Solves(");
			strSql.Append("Vpid,Vsid,Vanswer,Vright,Vscore,Vdate,Vgrade,Vterm,Vyear,Vnid,Vcid,Vclass)");
			strSql.Append(" values (");
            strSql.Append("@Vpid,@Vsid,@Vanswer,@Vright,@Vscore,@Vdate,@Vgrade,@Vterm,@Vyear,@Vnid,@Vcid,@Vclass)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Vpid", SqlDbType.Int,4),
					new SqlParameter("@Vsid", SqlDbType.Int,4),
					new SqlParameter("@Vanswer", SqlDbType.NVarChar,200),
					new SqlParameter("@Vright", SqlDbType.Bit,1),
					new SqlParameter("@Vscore", SqlDbType.Int,4),
					new SqlParameter("@Vdate", SqlDbType.DateTime),
					new SqlParameter("@Vgrade", SqlDbType.Int,4),
					new SqlParameter("@Vterm", SqlDbType.Int,4),
					new SqlParameter("@Vyear", SqlDbType.Int,4),
					new SqlParameter("@Vnid", SqlDbType.Int,4),
					new SqlParameter("@Vcid", SqlDbType.Int,4),
					new SqlParameter("@Vclass", SqlDbType.Int,4)};
			parameters[0].Value = model.Vpid;
			parameters[1].Value = model.Vsid;
			parameters[2].Value = model.Vanswer;
			parameters[3].Value = model.Vright;
			parameters[4].Value = model.Vscore;
            parameters[5].Value = model.Vdate;
            parameters[6].Value = model.Vgrade;
            parameters[7].Value = model.Vterm;
            parameters[8].Value = model.Vyear;
            parameters[9].Value = model.Vnid;
            parameters[10].Value = model.Vcid;
            parameters[11].Value = model.Vclass;

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
        /// 同步
        /// </summary>
        public void UpdateVgradeVterm()
        {
            string vterm = LearnSite.Common.XmlHelp.GetTerm();
            string mysql = "update Solves set Vgrade=Sgrade,Vterm=" + vterm + " from Solves,Students where Vsid=Sid ";
            DbHelperSQL.ExecuteSql(mysql);
        }

        public DataTable GetClassListScore(int Sgrade, int Sclass, int Vnid)
        {
            Students sbll = new Students();
            DataTable dtstus = sbll.GetStudentsSnumSidSname(Sgrade, Sclass).Tables[0];//学号和姓名
            string Ltitlestr = "Score";
            dtstus.Columns.Add(Ltitlestr, typeof(string));
            int dcount = dtstus.Rows.Count;
            for (int i = 0; i < dcount; i++)
            {
                string sid = dtstus.Rows[i]["Sid"].ToString();
                string mysql = "select sum(Vscore) from Solves where Vsid=" + sid + " and Vnid=" + Vnid.ToString();
                string score = DbHelperSQL.FindString(mysql);
                dtstus.Rows[i]["Score"] = score;
            }
            DataView dv = dtstus.DefaultView;
            DataTable dtnew = dv.ToTable("mysovle", true, new string[] {"Snum","Score"});
            return dtnew;
        }

        /// <summary>
        /// 更新并返回本学期测评成绩
        /// </summary>
        /// <param name="Vsid"></param>
        /// <param name="Vgrade"></param>
        /// <param name="Vterm"></param>
        public string UpdateSidle(string Vsid, string Vyear, string Vgrade,string Vclass, string Vterm) 
        {
            string mysql = "select sum(Vscore) from Solves where Vsid=" + Vsid + " and Vyear=" + Vyear + " and Vgrade=" + Vgrade + " and Vclass=" + Vclass + " and Vterm=" + Vterm;
            string Sidle = DbHelperSQL.FindNum(mysql).ToString();
            string sqlstr = "update Students set Sidle=" + Sidle + " where Sid=" + Vsid;
            DbHelperSQL.ExecuteSql(sqlstr);
            return Sidle;
        }

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LearnSite.Model.Solves model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Solves set ");
			strSql.Append("Vpid=@Vpid,");
			strSql.Append("Vsid=@Vsid,");
			strSql.Append("Vanswer=@Vanswer,");
			strSql.Append("Vright=@Vright,");
			strSql.Append("Vscore=@Vscore,");
			strSql.Append("Vdate=@Vdate");
			strSql.Append(" where Vid=@Vid");
			SqlParameter[] parameters = {
					new SqlParameter("@Vpid", SqlDbType.Int,4),
					new SqlParameter("@Vsid", SqlDbType.Int,4),
					new SqlParameter("@Vanswer", SqlDbType.NVarChar,200),
					new SqlParameter("@Vright", SqlDbType.Bit,1),
					new SqlParameter("@Vscore", SqlDbType.Int,4),
					new SqlParameter("@Vdate", SqlDbType.DateTime),
					new SqlParameter("@Vid", SqlDbType.Int,4)};
			parameters[0].Value = model.Vpid;
			parameters[1].Value = model.Vsid;
			parameters[2].Value = model.Vanswer;
			parameters[3].Value = model.Vright;
			parameters[4].Value = model.Vscore;
			parameters[5].Value = model.Vdate;
			parameters[6].Value = model.Vid;

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
		public bool Delete(int Vid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Solves ");
			strSql.Append(" where Vid=@Vid");
			SqlParameter[] parameters = {
					new SqlParameter("@Vid", SqlDbType.Int,4)
			};
			parameters[0].Value = Vid;

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
		public bool DeleteList(string Vidlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Solves ");
			strSql.Append(" where Vid in ("+Vidlist + ")  ");
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
		public LearnSite.Model.Solves GetModel(int Vid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Vid,Vpid,Vsid,Vanswer,Vright,Vscore,Vdate from Solves ");
			strSql.Append(" where Vid=@Vid");
			SqlParameter[] parameters = {
					new SqlParameter("@Vid", SqlDbType.Int,4)
			};
			parameters[0].Value = Vid;

			LearnSite.Model.Solves model=new LearnSite.Model.Solves();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["Vid"]!=null && ds.Tables[0].Rows[0]["Vid"].ToString()!="")
				{
					model.Vid=int.Parse(ds.Tables[0].Rows[0]["Vid"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Vpid"]!=null && ds.Tables[0].Rows[0]["Vpid"].ToString()!="")
				{
					model.Vpid=int.Parse(ds.Tables[0].Rows[0]["Vpid"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Vsid"]!=null && ds.Tables[0].Rows[0]["Vsid"].ToString()!="")
				{
					model.Vsid=int.Parse(ds.Tables[0].Rows[0]["Vsid"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Vanswer"]!=null && ds.Tables[0].Rows[0]["Vanswer"].ToString()!="")
				{
					model.Vanswer=ds.Tables[0].Rows[0]["Vanswer"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Vright"]!=null && ds.Tables[0].Rows[0]["Vright"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["Vright"].ToString()=="1")||(ds.Tables[0].Rows[0]["Vright"].ToString().ToLower()=="true"))
					{
						model.Vright=true;
					}
					else
					{
						model.Vright=false;
					}
				}
				if(ds.Tables[0].Rows[0]["Vscore"]!=null && ds.Tables[0].Rows[0]["Vscore"].ToString()!="")
				{
					model.Vscore=int.Parse(ds.Tables[0].Rows[0]["Vscore"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Vdate"]!=null && ds.Tables[0].Rows[0]["Vdate"].ToString()!="")
				{
					model.Vdate=DateTime.Parse(ds.Tables[0].Rows[0]["Vdate"].ToString());
				}
				return model;
			}
			else
			{
				return null;
			}
		}
        //Rgrade, Rclass, Wterm, Syear);
        public string ShowDoneSovleCids(string Rgrade, string Rclass, string Wterm, string Syear)
        {
            string mysql = "select distinct Vcid from Solves where Vgrade='" + Rgrade + "' and Vclass='" + Rclass + "' and Vterm='" + Wterm + "' and Vyear='" + Syear+"'";
            DataTable dt = DbHelperSQL.Query(mysql).Tables[0];
            int n = dt.Rows.Count;
            if (n > 0)
            {
                string strtemp = "";
                for (int i = 0; i < n; i++)
                {
                    strtemp = strtemp + dt.Rows[i]["Vcid"].ToString() + ",";
                }
                return strtemp;
            }
            else
            {
                return "";
            }
        }



        public string ShowStuDoneSovleCids(string Sid, string Rterm, string Rgrade)
        {
            string mysql = "select distinct Vcid from Solves where Vsid=" + Sid + " and Vterm='" + Rterm + "' and Vgrade='" + Rgrade + "'";
            DataTable dt = DbHelperSQL.Query(mysql).Tables[0];
            int n = dt.Rows.Count;
            if (n > 0)
            {
                string strtemp = "";
                string cidstr = "";
                for (int i = 0; i < n; i++)
                {
                    cidstr = dt.Rows[i]["Vcid"].ToString();
                    if (!string.IsNullOrEmpty(cidstr))
                    {
                        strtemp = strtemp + cidstr + ",";
                    }
                }
                return strtemp;
            }
            else
            {
                return "";
            }
        }


		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Vid,Vpid,Vsid,Vanswer,Vright,Vscore,Vdate ");
			strSql.Append(" FROM Solves ");
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
			strSql.Append(" Vid,Vpid,Vsid,Vanswer,Vright,Vscore,Vdate ");
			strSql.Append(" FROM Solves ");
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
			strSql.Append("select count(1) FROM Solves ");
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
				strSql.Append("order by T.Vid desc");
			}
			strSql.Append(")AS Row, T.*  from Solves T ");
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
			parameters[0].Value = "Solves";
			parameters[1].Value = "Vid";
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

