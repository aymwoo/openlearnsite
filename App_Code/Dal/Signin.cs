using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using LearnSite.DBUtility;
using System.Globalization;
using System.Web;
using System.IO;
namespace LearnSite.DAL
{
	/// <summary>
	/// 数据访问类Signin。
	/// </summary>
	public class Signin
	{
		public Signin()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Qid", "Signin"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Qid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Signin");
			strSql.Append(" where Qid=@Qid ");
			SqlParameter[] parameters = {
					new SqlParameter("@Qid", SqlDbType.Int,4)};
			parameters[0].Value = Qid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
        public int Add(LearnSite.Model.Signin model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Signin(");
            strSql.Append("Qnum,Qattitude,Qdate,Qyear,Qmonth,Qday,Qweek,Qip,Qmachine,Qnote,Qwork,Qgrade,Qterm,Qsid,Qname,Qclass,Qsyear,Qcid,Qtitle,Qsession)");//新增,Qtitle,Qsession
            strSql.Append(" values (");
            strSql.Append("@Qnum,@Qattitude,@Qdate,@Qyear,@Qmonth,@Qday,@Qweek,@Qip,@Qmachine,@Qnote,@Qwork,@Qgrade,@Qterm,@Qsid,@Qname,@Qclass,@Qsyear,@Qcid,@Qtitle,@Qsession)");//新增,@Qtitle,@Qsession
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Qnum", SqlDbType.NVarChar,50),
					new SqlParameter("@Qattitude", SqlDbType.Int,4),
					new SqlParameter("@Qdate", SqlDbType.DateTime),
					new SqlParameter("@Qyear", SqlDbType.Int,4),
					new SqlParameter("@Qmonth", SqlDbType.Int,4),
					new SqlParameter("@Qday", SqlDbType.Int,4),
					new SqlParameter("@Qweek", SqlDbType.NVarChar,50),
					new SqlParameter("@Qip", SqlDbType.NVarChar,50),
					new SqlParameter("@Qmachine", SqlDbType.NVarChar,50),
					new SqlParameter("@Qnote", SqlDbType.NVarChar,50),
					new SqlParameter("@Qwork", SqlDbType.Int,4),
					new SqlParameter("@Qgrade", SqlDbType.Int,4),
					new SqlParameter("@Qterm", SqlDbType.Int,4),
                    new SqlParameter("@Qsid", SqlDbType.Int,4),
                    new SqlParameter("@Qname", SqlDbType.NVarChar,50),
                    new SqlParameter("@Qclass", SqlDbType.Int,4),
                    new SqlParameter("@Qsyear", SqlDbType.Int,4),
                    new SqlParameter("@Qcid", SqlDbType.Int,4),
                    new SqlParameter("@Qtitle", SqlDbType.NVarChar,100),//
                    new SqlParameter("@Qsession", SqlDbType.VarChar,50)};//
            parameters[0].Value = model.Qnum;
            parameters[1].Value = model.Qattitude;
            parameters[2].Value = model.Qdate;
            parameters[3].Value = model.Qyear;
            parameters[4].Value = model.Qmonth;
            parameters[5].Value = model.Qday;
            parameters[6].Value = model.Qweek;
            parameters[7].Value = model.Qip;
            parameters[8].Value = model.Qmachine;
            parameters[9].Value = model.Qnote;
            parameters[10].Value = model.Qwork;
            parameters[11].Value = model.Qgrade;
            parameters[12].Value = model.Qterm;
            parameters[13].Value = model.Qsid;
            parameters[14].Value = model.Qname;
            parameters[15].Value = model.Qclass;
            parameters[16].Value = model.Qsyear;
            parameters[17].Value = model.Qcid;
            parameters[18].Value = model.Qtitle;
            parameters[19].Value = model.Qsession;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
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
        /// 签到，增加一条数据Qnum,Qdate,Qyear,Qmonth,Qday,Qweek,Qip,Qmachine
        /// </summary>
        private int AddToday(LearnSite.Model.Signin model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Signin(");
            strSql.Append("Qnum,Qdate,Qyear,Qmonth,Qday,Qweek,Qip,Qmachine,Qgrade,Qterm,Qtitle,Qsession)");//新增,Qtitle,Qsession
            strSql.Append(" values (");
            strSql.Append("@Qnum,@Qdate,@Qyear,@Qmonth,@Qday,@Qweek,@Qip,@Qmachine,@Qgrade,@Qterm,@Qtitle,@Qsession)");//新增,@Qtitle,@Qsession
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Qnum", SqlDbType.NVarChar,50),
					new SqlParameter("@Qdate", SqlDbType.DateTime),
					new SqlParameter("@Qyear", SqlDbType.Int,4),
					new SqlParameter("@Qmonth", SqlDbType.Int,4),
					new SqlParameter("@Qday", SqlDbType.Int,4),
					new SqlParameter("@Qweek", SqlDbType.NVarChar,50),
					new SqlParameter("@Qip", SqlDbType.NVarChar,50),
                    new SqlParameter("@Qmachine", SqlDbType.NVarChar,50),
					new SqlParameter("@Qgrade", SqlDbType.Int,4),
					new SqlParameter("@Qterm", SqlDbType.Int,4),
            		new SqlParameter("@Qtitle", SqlDbType.NVarChar,100),//新增
                    new SqlParameter("@Qsession", SqlDbType.NVarChar,50)};//新增
            parameters[0].Value = model.Qnum;
            parameters[1].Value = model.Qdate;
            parameters[2].Value = model.Qyear;
            parameters[3].Value = model.Qmonth;
            parameters[4].Value = model.Qday;
            parameters[5].Value = model.Qweek;
            parameters[6].Value = model.Qip;
            parameters[7].Value = model.Qmachine;
            parameters[8].Value = model.Qgrade;
            parameters[9].Value = model.Qterm;
            parameters[10].Value = model.Qtitle;//新增
            parameters[11].Value = model.Qsession;//新增

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }


		/// <summary>
		/// 更新一条数据
		/// </summary>
        public bool Update(LearnSite.Model.Signin model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Signin set ");
            strSql.Append("Qnum=@Qnum,");
            strSql.Append("Qattitude=@Qattitude,");
            strSql.Append("Qdate=@Qdate,");
            strSql.Append("Qyear=@Qyear,");
            strSql.Append("Qmonth=@Qmonth,");
            strSql.Append("Qday=@Qday,");
            strSql.Append("Qweek=@Qweek,");
            strSql.Append("Qip=@Qip,");
            strSql.Append("Qmachine=@Qmachine,");
            strSql.Append("Qnote=@Qnote,");
            strSql.Append("Qwork=@Qwork,");
            strSql.Append("Qgrade=@Qgrade,");
            strSql.Append("Qterm=@Qterm,");
            strSql.Append("Qtitle=@Qtitle,");//新增
            strSql.Append("Qsession=@Qsession");//新增
            strSql.Append(" where Qid=@Qid");
            SqlParameter[] parameters = {
					new SqlParameter("@Qnum", SqlDbType.NVarChar,50),
					new SqlParameter("@Qattitude", SqlDbType.Int,4),
					new SqlParameter("@Qdate", SqlDbType.DateTime),
					new SqlParameter("@Qyear", SqlDbType.Int,4),
					new SqlParameter("@Qmonth", SqlDbType.Int,4),
					new SqlParameter("@Qday", SqlDbType.Int,4),
					new SqlParameter("@Qweek", SqlDbType.NVarChar,50),
					new SqlParameter("@Qip", SqlDbType.NVarChar,50),
					new SqlParameter("@Qmachine", SqlDbType.NVarChar,50),
					new SqlParameter("@Qnote", SqlDbType.NVarChar,50),
					new SqlParameter("@Qwork", SqlDbType.Int,4),
					new SqlParameter("@Qgrade", SqlDbType.Int,4),
					new SqlParameter("@Qterm", SqlDbType.Int,4),
                    new SqlParameter("@Qtitle", SqlDbType.NVarChar,100),//新增
                    new SqlParameter("@Qsession", SqlDbType.NVarChar,50),//新增
					new SqlParameter("@Qid", SqlDbType.Int,4)};
            parameters[0].Value = model.Qnum;
            parameters[1].Value = model.Qattitude;
            parameters[2].Value = model.Qdate;
            parameters[3].Value = model.Qyear;
            parameters[4].Value = model.Qmonth;
            parameters[5].Value = model.Qday;
            parameters[6].Value = model.Qweek;
            parameters[7].Value = model.Qip;
            parameters[8].Value = model.Qmachine;
            parameters[9].Value = model.Qnote;
            parameters[10].Value = model.Qwork;
            parameters[11].Value = model.Qgrade;
            parameters[12].Value = model.Qterm;
            parameters[13].Value = model.Qtitle;//新增
            parameters[14].Value = model.Qsession;//新增
            parameters[15].Value = model.Qid;

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
        /// 更新该学号今天的作品数量
        /// </summary>
        /// <param name="Qnum"></param>
        /// <param name="Qwork"></param>
        public void UpdateQwork(int Qsid,int Wcid)
        {
            DateTime Qdate = DateTime.Now;
            int Qyear = Qdate.Year;
            int Qmonth = Qdate.Month;
            int Qday = Qdate.Day;
            string mysql = "update Signin set Qwork=(select count(*) from Works where Wcid=" + Wcid + " and Wsid=" + Qsid +") where Qsid=" + Qsid + " and Qyear=" + Qyear + " and Qmonth=" + Qmonth + " and Qday=" + Qday;
            DbHelperSQL.ExecuteSql(mysql);        
        }
        /// <summary>
        /// 更新一条数据（给学生学习表现评分）
        /// </summary>
        public void UpdateAttitude(int Qid,int Qattitude,string Qnote,int Qcid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Signin set ");            
            strSql.Append("Qattitude=@Qattitude,");            
            strSql.Append("Qnote=@Qnote,");
            strSql.Append("Qcid=@Qcid");
            strSql.Append(" where Qid=@Qid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Qid", SqlDbType.Int,4),
					new SqlParameter("@Qattitude", SqlDbType.Int,4),
					new SqlParameter("@Qnote", SqlDbType.NVarChar,50),
					new SqlParameter("@Qcid", SqlDbType.Int,4)};
            parameters[0].Value = Qid;
            parameters[1].Value = Qattitude;
            parameters[2].Value = Qnote;
            parameters[3].Value = Qcid;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int Qid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Signin ");
			strSql.Append(" where Qid=@Qid ");
			SqlParameter[] parameters = {
					new SqlParameter("@Qid", SqlDbType.Int,4)};
			parameters[0].Value = Qid;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}
        /// <summary>
        /// 清除几年前的签到记录
        /// </summary>
        /// <param name="Wyear"></param>
        public int DeleteOldyear(int Wyear)
        {
            DateTime olddate = DateTime.Now.AddYears(-Wyear);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Signin ");
            strSql.Append(" where  Qdate <@Qdate");
            SqlParameter[] parameters = {
					new SqlParameter("@Qdate", SqlDbType.DateTime)};
            parameters[0].Value = olddate;

            return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
        public LearnSite.Model.Signin GetModel(int Qid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Qid,Qnum,Qattitude,Qdate,Qyear,Qmonth,Qday,Qweek,Qip,Qmachine,Qnote,Qwork,Qgrade,Qterm,Qgroup,Qgscore from Signin ");
            strSql.Append(" where Qid=@Qid");
            SqlParameter[] parameters = {
					new SqlParameter("@Qid", SqlDbType.Int,4)};
            parameters[0].Value = Qid;

            LearnSite.Model.Signin model = new LearnSite.Model.Signin();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Qid"].ToString() != "")
                {
                    model.Qid = int.Parse(ds.Tables[0].Rows[0]["Qid"].ToString());
                }
                model.Qnum = ds.Tables[0].Rows[0]["Qnum"].ToString();
                if (ds.Tables[0].Rows[0]["Qattitude"].ToString() != "")
                {
                    model.Qattitude = int.Parse(ds.Tables[0].Rows[0]["Qattitude"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Qdate"].ToString() != "")
                {
                    model.Qdate = DateTime.Parse(ds.Tables[0].Rows[0]["Qdate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Qyear"].ToString() != "")
                {
                    model.Qyear = int.Parse(ds.Tables[0].Rows[0]["Qyear"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Qmonth"].ToString() != "")
                {
                    model.Qmonth = int.Parse(ds.Tables[0].Rows[0]["Qmonth"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Qday"].ToString() != "")
                {
                    model.Qday = int.Parse(ds.Tables[0].Rows[0]["Qday"].ToString());
                }
                model.Qweek = ds.Tables[0].Rows[0]["Qweek"].ToString();
                model.Qip = ds.Tables[0].Rows[0]["Qip"].ToString();
                model.Qmachine = ds.Tables[0].Rows[0]["Qmachine"].ToString();
                model.Qnote = ds.Tables[0].Rows[0]["Qnote"].ToString();
                if (ds.Tables[0].Rows[0]["Qwork"].ToString() != "")
                {
                    model.Qwork = int.Parse(ds.Tables[0].Rows[0]["Qwork"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Qgrade"].ToString() != "")
                {
                    model.Qgrade = int.Parse(ds.Tables[0].Rows[0]["Qgrade"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Qterm"].ToString() != "")
                {
                    model.Qterm = int.Parse(ds.Tables[0].Rows[0]["Qterm"].ToString());
                }
                model.Qgroup = ds.Tables[0].Rows[0]["Qgroup"].ToString();
                if (ds.Tables[0].Rows[0]["Qgscore"].ToString() != "")
                {
                    model.Qgscore = int.Parse(ds.Tables[0].Rows[0]["Qgscore"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据学号得到一个对象实体，最近的签到记录
        /// </summary>
        public LearnSite.Model.Signin GetModelm(string Qnum)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Qid,Qnum,Qattitude,Qdate,Qyear,Qmonth,Qday,Qweek,Qip,Qmachine,Qnote,Qwork,Qgrade,Qterm,Qgroup,Qgscore,Qsid,Qname,Qclass,Qsyear from Signin ");
            strSql.Append(" where Qnum=@Qnum order by Qdate desc");
            SqlParameter[] parameters = {
					new SqlParameter("@Qnum", SqlDbType.NVarChar,50)};
            parameters[0].Value = Qnum;

            LearnSite.Model.Signin model = new LearnSite.Model.Signin();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Qid"].ToString() != "")
                {
                    model.Qid = int.Parse(ds.Tables[0].Rows[0]["Qid"].ToString());
                }
                model.Qnum = ds.Tables[0].Rows[0]["Qnum"].ToString();
                if (ds.Tables[0].Rows[0]["Qattitude"].ToString() != "")
                {
                    model.Qattitude = int.Parse(ds.Tables[0].Rows[0]["Qattitude"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Qdate"].ToString() != "")
                {
                    model.Qdate = DateTime.Parse(ds.Tables[0].Rows[0]["Qdate"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Qyear"].ToString() != "")
                {
                    model.Qyear = int.Parse(ds.Tables[0].Rows[0]["Qyear"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Qmonth"].ToString() != "")
                {
                    model.Qmonth = int.Parse(ds.Tables[0].Rows[0]["Qmonth"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Qday"].ToString() != "")
                {
                    model.Qday = int.Parse(ds.Tables[0].Rows[0]["Qday"].ToString());
                }
                model.Qweek = ds.Tables[0].Rows[0]["Qweek"].ToString();
                model.Qip = ds.Tables[0].Rows[0]["Qip"].ToString();
                model.Qmachine = ds.Tables[0].Rows[0]["Qmachine"].ToString();
                model.Qnote = ds.Tables[0].Rows[0]["Qnote"].ToString();
                if (ds.Tables[0].Rows[0]["Qwork"].ToString() != "")
                {
                    model.Qwork = int.Parse(ds.Tables[0].Rows[0]["Qwork"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Qgrade"].ToString() != "")
                {
                    model.Qgrade = int.Parse(ds.Tables[0].Rows[0]["Qgrade"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Qterm"].ToString() != "")
                {
                    model.Qterm = int.Parse(ds.Tables[0].Rows[0]["Qterm"].ToString());
                }
                model.Qgroup = ds.Tables[0].Rows[0]["Qgroup"].ToString();
                if (ds.Tables[0].Rows[0]["Qgscore"].ToString() != "")
                {
                    model.Qgscore = int.Parse(ds.Tables[0].Rows[0]["Qgscore"].ToString());
                }
                //,Qsid,Qname,Qclass,Qsyear
                if (ds.Tables[0].Rows[0]["Qsid"].ToString() != "")
                {
                    model.Qsid = int.Parse(ds.Tables[0].Rows[0]["Qsid"].ToString());
                }
                model.Qname = ds.Tables[0].Rows[0]["Qname"].ToString();
                if (ds.Tables[0].Rows[0]["Qclass"].ToString() != "")
                {
                    model.Qclass = int.Parse(ds.Tables[0].Rows[0]["Qclass"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Qsyear"].ToString() != "")
                {
                    model.Qsyear = int.Parse(ds.Tables[0].Rows[0]["Qsyear"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 查询班级某年某月某日的详细 已签到记录11111111
        /// </summary>
        /// <param name="Qgrade"></param>
        /// <param name="Qclass"></param>
        /// <param name="Qyear"></param>
        /// <param name="Qmonth"></param>
        /// <param name="Qday"></param>
        public DataSet Signclassdetail(int Qgrade, int Qclass, int Qyear, int Qmonth, int Qday)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct Qid, Qnum, Qname,Qgrade,Qclass,Qip,Qmachine,Qdate,Qweek,Qattitude,Qnote,Qwork,Qgroup,Qgscore ");
            strSql.Append(" FROM Signin ");
            strSql.Append(" WHERE Qgrade=@Qgrade AND Qclass=@Qclass AND Qyear=@Qyear AND Qmonth=@Qmonth AND Qday=@Qday ORDER BY Qnum ASC");
            SqlParameter[] parameters = {
					new SqlParameter("@Qgrade", SqlDbType.Int,4),
                    new SqlParameter("@Qclass", SqlDbType.Int,4),
                    new SqlParameter("@Qyear", SqlDbType.Int,4),                    
                    new SqlParameter("@Qmonth", SqlDbType.Int,4),                    
                    new SqlParameter("@Qday", SqlDbType.Int,4)};

            parameters[0].Value = Qgrade;
            parameters[1].Value = Qclass;
            parameters[2].Value = Qyear;
            parameters[3].Value = Qmonth;
            parameters[4].Value = Qday;

            return DbHelperSQL.Query(strSql.ToString(), parameters);
        }
        public DataSet SignclassdetailSort(int Qgrade, int Qclass, int Qyear, int Qmonth, int Qday, string Qtitle, int sort, string Qsession)//????, string Qtitle , string Qsession
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct Qid, Qnum, Qname,Qgrade,Qclass,Qip,Qmachine,Qdate,Qweek,Qattitude,Qnote,Qwork,Qgroup,Qgscore,Qtitle,Qsession,Qyear,Qmonth,Qday,Qterm ");
            strSql.Append(" FROM Signin ");
            strSql.Append(" WHERE Qgrade=@Qgrade AND Qclass=@Qclass");
            
            // 只有当Qyear大于0时才添加年份筛选条件
            if (Qyear > 0)
            {
                strSql.Append(" AND Qyear=@Qyear");
            }
            
            // 只有当Qmonth大于0时才添加月份筛选条件
            if (Qmonth > 0)
            {
                strSql.Append(" AND Qmonth=@Qmonth");
            }
            
            // 只有当Qday大于0时才添加日期筛选条件
            if (Qday > 0)
            {
                strSql.Append(" AND Qday=@Qday");
            }
            // 添加Qtitle和Qsession的查询条件
            if (!string.IsNullOrEmpty(Qtitle))
            {
                strSql.Append(" AND Qtitle=@Qtitle");
            }
            if (!string.IsNullOrEmpty(Qsession))
            {
                strSql.Append(" AND Qsession=@Qsession");
            }
            if (sort == 0)
            {
                strSql.Append(" ORDER BY Qnum ASC");
            }
            else
            {
                strSql.Append(" ORDER BY Qip ASC");
            }
            
            // 创建参数列表
            List<SqlParameter> paramList = new List<SqlParameter>();
            SqlParameter paramGrade = new SqlParameter("@Qgrade", SqlDbType.Int,4);
            paramGrade.Value = Qgrade;
            paramList.Add(paramGrade);
            
            SqlParameter paramClass = new SqlParameter("@Qclass", SqlDbType.Int,4);
            paramClass.Value = Qclass;
            paramList.Add(paramClass);
            
            // 只有当Qyear大于0时才添加年份参数
            if (Qyear > 0)
            {
                SqlParameter paramYear = new SqlParameter("@Qyear", SqlDbType.Int,4);
                paramYear.Value = Qyear;
                paramList.Add(paramYear);
            }
            
            // 只有当Qmonth大于0时才添加月份参数
            if (Qmonth > 0)
            {
                SqlParameter paramMonth = new SqlParameter("@Qmonth", SqlDbType.Int,4);
                paramMonth.Value = Qmonth;
                paramList.Add(paramMonth);
            }
            
            // 只有当Qday大于0时才添加日期参数
            if (Qday > 0)
            {
                SqlParameter paramDay = new SqlParameter("@Qday", SqlDbType.Int,4);
                paramDay.Value = Qday;
                paramList.Add(paramDay);
            }
            
            // 添加可选参数
            if (!string.IsNullOrEmpty(Qtitle))
            {
                SqlParameter paramTitle = new SqlParameter("@Qtitle", SqlDbType.NVarChar, 100);
                paramTitle.Value = Qtitle;
                paramList.Add(paramTitle);
            }
            if (!string.IsNullOrEmpty(Qsession))
            {
                SqlParameter paramSession = new SqlParameter("@Qsession", SqlDbType.NVarChar, 50);
                paramSession.Value = Qsession;
                paramList.Add(paramSession);
            }

            return DbHelperSQL.Query(strSql.ToString(), paramList.ToArray());
        }
        /// <summary>
        /// 某班级签到导出到Excel
        /// </summary>
        public void SignExcel(int Sgrade, int Sclass,int Qterm)
        {
            DateTime dt = DateTime.Now;
            string today = dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day;
            string FileName =Sgrade.ToString()+"_"+Sclass.ToString()+"_"+Qterm.ToString()+System.Web.HttpUtility.UrlEncode("签到") + today;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Qnum as 学号, Sname as 姓名,Sgrade as 年级,Sclass as 班级,Qip as 签到IP,Qdate as 签到日期,Qattitude as 学习表现,Qnote as 备注,Qwork as 作品数量");
            strSql.Append(" FROM Signin,Students ");
            strSql.Append(" WHERE Qnum=Snum AND Sgrade=Qgrade AND Sgrade=@Sgrade AND Sclass=@Sclass AND Qterm=@Qterm ORDER BY Qnum ASC");
            SqlParameter[] parameters = {
					new SqlParameter("@Sgrade", SqlDbType.Int,4),
                    new SqlParameter("@Sclass", SqlDbType.Int,4),
                    new SqlParameter("@Qterm", SqlDbType.Int,4)};

            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;
            parameters[2].Value = Qterm;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(),parameters);
            Common.DataExcel.DataSetToExcel(ds, FileName);
		}
        /// <summary>
        /// 导出签到表到Excel（带表头标题和边框）
        /// </summary>
        private void SignExcelWithHeader(DataSet ds, string fileName)
        {
            try
            {
                HttpResponse resp = HttpContext.Current.Response;
                resp.Clear();
                resp.Buffer = true;
                resp.Charset = "gb2312";
                resp.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                resp.AppendHeader("Content-disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName + ".xls", System.Text.Encoding.UTF8));
                resp.ContentType = "application/ms-excel";

                StringWriter sfw = new StringWriter();
                DataTable dt = ds.Tables[0];

                // 写入表头标题（居中、加粗）
                sfw.WriteLine("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
                sfw.WriteLine("<head>");
                sfw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html;charset=gb2312\">");
                sfw.WriteLine("<style>");
                sfw.WriteLine("table {border-collapse: collapse; mso-excel-xformy: no;}");
                sfw.WriteLine("td {border: 0.5pt solid black; padding: 5px; text-align: center;}");
                sfw.WriteLine("th {border: 0.5pt solid black; padding: 5px; font-weight: bold; text-align: center;}");
                sfw.WriteLine(".header {font-size: 16pt; font-weight: bold; text-align: center;}");
                sfw.WriteLine("</style>");
                sfw.WriteLine("</head>");
                sfw.WriteLine("<body>");

                // 表头标题行
                sfw.WriteLine("<table>");
                sfw.WriteLine("<tr><td colspan=\"8\" class=\"header\">学生签到表</td></tr>");

                // 写入列标题
                sfw.WriteLine("<tr>");
                sfw.WriteLine("<th>学号</th>");
                sfw.WriteLine("<th>姓名</th>");
                sfw.WriteLine("<th>年级</th>");
                sfw.WriteLine("<th>班级</th>");
                sfw.WriteLine("<th>签到IP</th>");
                sfw.WriteLine("<th>签到日期</th>");
                sfw.WriteLine("<th>学习表现</th>");
                sfw.WriteLine("<th>备注</th>");
                sfw.WriteLine("<th>作品数量</th>");
                sfw.WriteLine("</tr>");

                // 写入数据行
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sfw.WriteLine("<tr>");
                    sfw.WriteLine("<td>" + dt.Rows[i]["学号"].ToString() + "</td>");
                    sfw.WriteLine("<td>" + dt.Rows[i]["姓名"].ToString() + "</td>");
                    sfw.WriteLine("<td>" + dt.Rows[i]["年级"].ToString() + "</td>");
                    sfw.WriteLine("<td>" + dt.Rows[i]["班级"].ToString() + "</td>");
                    sfw.WriteLine("<td>" + dt.Rows[i]["签到IP"].ToString() + "</td>");
                    sfw.WriteLine("<td>" + dt.Rows[i]["签到日期"].ToString() + "</td>");
                    sfw.WriteLine("<td>" + dt.Rows[i]["学习表现"].ToString() + "</td>");
                    sfw.WriteLine("<td>" + dt.Rows[i]["备注"].ToString() + "</td>");
                    sfw.WriteLine("<td>" + dt.Rows[i]["作品数量"].ToString() + "</td>");
                    sfw.WriteLine("</tr>");
                }

                sfw.WriteLine("</table>");
                sfw.WriteLine("</body>");
                sfw.WriteLine("</html>");

                resp.Write(sfw.ToString());
                resp.Flush();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
        }
        /// <summary>
        /// 根据学号查询签到记录，按年级、学期排序
        /// </summary>
        /// <param name="Snum"></param>
        /// <returns></returns>
        public DataSet SignSnumdetail(string Snum)
        {
            string mysql = "select distinct Qid, Qnum, Sname,Sgrade,Sclass,Qip,Qdate,Qweek,Qattitude,Qnote,Qwork,Qterm  FROM Signin,Students where Qnum=Snum and Snum='" + Snum + "' order by Sgrade asc,Qterm asc";
            return DbHelperSQL.Query(mysql);
        }

        /// <summary>
        /// 学生界面今天签到显示
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Qyear"></param>
        /// <param name="Qmonth"></param>
        /// <param name="Qday"></param>
        /// <returns></returns>
        public DataSet OnlineToday(int Sgrade, int Sclass, int Qyear, int Qmonth, int Qday)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct Qid, Qnum, Sname,Sleader,Sgroup,Qip,right(Qdate,8) as Qdate ");
            strSql.Append(" FROM Signin,Students ");
            strSql.Append(" WHERE Sgrade=@Sgrade AND Sclass=@Sclass AND Qday=@Qday AND Qmonth=@Qmonth AND Qyear=@Qyear AND Qsid=Sid ORDER BY Qdate ASC");
            SqlParameter[] parameters = {
					new SqlParameter("@Sgrade", SqlDbType.Int,4),
                    new SqlParameter("@Sclass", SqlDbType.Int,4),
                    new SqlParameter("@Qyear", SqlDbType.Int,4),                    
                    new SqlParameter("@Qmonth", SqlDbType.Int,4),                    
                    new SqlParameter("@Qday", SqlDbType.Int,4)};

            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;
            parameters[2].Value = Qyear;
            parameters[3].Value = Qmonth;
            parameters[4].Value = Qday;

            return DbHelperSQL.Query(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 查询班级某年某月某日的详细 未签到记录0000000
        /// </summary>
        /// <param name="Qgrade"></param>
        /// <param name="Qclass"></param>
        /// <param name="Qyear"></param>
        /// <param name="Qmonth"></param>
        /// <param name="Qday"></param>
        public DataSet NoSignclassdetail(int Sgrade, int Sclass, int Qyear, int Qmonth, int Qday)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  Snum,Sgrade,Sclass,Sname,Saddress,Sphone,Sparents,Sheadtheacher,Sattitude ");
            strSql.Append(" FROM Students  ");
            strSql.Append(" WHERE Sgrade=@Sgrade AND Sclass=@Sclass AND Snum not in (");
            strSql.Append("select distinct Snum ");
            strSql.Append(" FROM Signin,Students ");
            strSql.Append(" WHERE Snum=Qnum AND Sgrade=@Sgrade AND Sclass=@Sclass AND Qyear=@Qyear AND Qmonth=@Qmonth AND Qday=@Qday ");
            strSql.Append(")");
            strSql.Append(" ORDER BY Snum");
            SqlParameter[] parameters = {
					new SqlParameter("@Sgrade", SqlDbType.Int,4),
                    new SqlParameter("@Sclass", SqlDbType.Int,4),
                    new SqlParameter("@Qyear", SqlDbType.Int,4),
					new SqlParameter("@Qmonth", SqlDbType.Int,4),
					new SqlParameter("@Qday", SqlDbType.Int,4)};
            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;
            parameters[2].Value = Qyear;
            parameters[3].Value = Qmonth;
            parameters[4].Value = Qday;

            return DbHelperSQL.Query(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 查询班级签到列表
        /// </summary>
        /// <param name="Qgrade"></param>
        /// <param name="Qclass"></param>
        /// <returns></returns>
        public DataSet GetSignClass(int Sgrade,int Sclass)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct Sgrade,Sclass,Qyear,Qmonth,Qday ");
            strSql.Append(" FROM Signin,Students ");
            strSql.Append(" WHERE Qnum=Snum AND Sgrade=@Sgrade AND Sclass=@Sclass ORDER BY Qyear DESC,Qmonth DESC,Qday DESC");
            SqlParameter[] parameters = {
					new SqlParameter("@Sgrade", SqlDbType.Int,4),
                    new SqlParameter("@Sclass", SqlDbType.Int,4)};
            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;

            return DbHelperSQL.Query(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 获取今天签到的同学
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public DataTable GetSiginStudents(int Sgrade, int Sclass)
        {
            DateTime today = DateTime.Now;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct Qnum, Sname,Qip ");
            strSql.Append(" FROM Signin,Students ");
            strSql.Append(" WHERE Sgrade=@Sgrade AND Sclass=@Sclass AND Sid=Qsid AND Qyear=@Qyear AND Qmonth=@Qmonth AND Qday=@Qday ");

            SqlParameter[] parameters = {
					new SqlParameter("@Sgrade", SqlDbType.Int,4),
                    new SqlParameter("@Sclass", SqlDbType.Int,4),
                    new SqlParameter("@Qyear", SqlDbType.Int,4),                    
                    new SqlParameter("@Qmonth", SqlDbType.Int,4),                    
                    new SqlParameter("@Qday", SqlDbType.Int,4)};

            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;
            parameters[2].Value = today.Year;
            parameters[3].Value = today.Month;
            parameters[4].Value = today.Day;

            return DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
        }
        /// <summary>
        /// 查询开始上课页面，班级签到列表
        /// </summary>
        /// <param name="Qgrade"></param>
        /// <param name="Qclass"></param>
        /// <param name="Qyear"></param>
        /// <param name="Qmonth"></param>
        /// <param name="Qday"></param>
        /// <returns></returns>
        public DataSet StartSignClass(int Sgrade, int Sclass, int Qyear, int Qmonth, int Qday, string signsort)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct Qid,Qip, Qnum, Qname,Sleader,Sgroup,Sgtitle,Qattitude,left(Qmachine,12) as QmachineShort,Qnote,Qwork,Qgroup,Qgscore ");
            strSql.Append(" FROM Signin,Students ");
            strSql.Append(" WHERE Sgrade=@Sgrade AND Sclass=@Sclass AND Sid=Qsid AND Qyear=@Qyear AND Qmonth=@Qmonth AND Qday=@Qday ");
            switch (signsort)
            { 
                case "0":
                    strSql.Append(" ORDER BY QmachineShort ASC ");
                    break;
                case "1":
                    strSql.Append("  ORDER BY Qnum ASC");
                    break;
                case "2":
                    strSql.Append("  ORDER BY Sgroup ASC,Sleader DESC");
                    break;
                case "4":
                    strSql.Append("  ORDER BY QmachineShort ASC");
                    break;
            }
            SqlParameter[] parameters = {
				new SqlParameter("@Sgrade", SqlDbType.Int,4),
                    new SqlParameter("@Sclass", SqlDbType.Int,4),
                    new SqlParameter("@Qyear", SqlDbType.Int,4),                    
                    new SqlParameter("@Qmonth", SqlDbType.Int,4),                    
                    new SqlParameter("@Qday", SqlDbType.Int,4)};

            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;
            parameters[2].Value = Qyear;
            parameters[3].Value = Qmonth;
            parameters[4].Value = Qday;

            return DbHelperSQL.Query(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 返回今天作品未提交的学生 Qid, Qnum, Sname,Sscore
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Qyear"></param>
        /// <param name="Qmonth"></param>
        /// <param name="Qday"></param>
        /// <param name="Qwork"></param>
        /// <returns></returns>
        public DataSet GetNoWorkStudents(int Sgrade, int Sclass,int Qyear,int Qmonth,int Qday,int Qwork)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct Qid, Qnum, Sname,Sscore ");
            strSql.Append(" FROM Signin,Students ");
            strSql.Append(" WHERE Qsid=Sid AND Sgrade=@Sgrade AND Sclass=@Sclass AND Qyear=@Qyear AND Qmonth=@Qmonth AND Qday=@Qday AND Qwork=@Qwork ORDER BY Qnum ASC");
            SqlParameter[] parameters = {
					new SqlParameter("@Sgrade", SqlDbType.Int,4),
                    new SqlParameter("@Sclass", SqlDbType.Int,4),
                    new SqlParameter("@Qyear", SqlDbType.Int,4),                    
                    new SqlParameter("@Qmonth", SqlDbType.Int,4),                    
                    new SqlParameter("@Qday", SqlDbType.Int,4),                    
                    new SqlParameter("@Qwork", SqlDbType.Int,4)};

            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;
            parameters[2].Value = Qyear;
            parameters[3].Value = Qmonth;
            parameters[4].Value = Qday;
            parameters[5].Value = Qwork;

            return DbHelperSQL.Query(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 查询开始上课页面，班级没有签到列表
        /// </summary>
        /// <param name="Qgrade"></param>
        /// <param name="Qclass"></param>
        /// <param name="Qyear"></param>
        /// <param name="Qmonth"></param>
        /// <param name="Qday"></param>
        /// <returns></returns>
        public DataTable StartNoSignClass(int Sgrade, int Sclass, int Syear, int Qyear, int Qmonth, int Qday)
        {
            StringBuilder mySql = new StringBuilder();
            mySql.Append("select Sid, Snum,Sname,Sscore");
            mySql.Append(" FROM Students  ");
            mySql.Append(" WHERE Sgrade=@Sgrade AND Sclass=@Sclass order by Sid");

            SqlParameter[] parameters = {
					new SqlParameter("@Sgrade", SqlDbType.Int,4),
                    new SqlParameter("@Sclass", SqlDbType.Int,4),
                    new SqlParameter("@Syear", SqlDbType.Int,4),
                    new SqlParameter("@Qyear", SqlDbType.Int,4),                    
                    new SqlParameter("@Qmonth", SqlDbType.Int,4),                    
                    new SqlParameter("@Qday", SqlDbType.Int,4)};

            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;
            parameters[2].Value = Syear;
            parameters[3].Value = Qyear;
            parameters[4].Value = Qmonth;
            parameters[5].Value = Qday;

            DataTable dtstu = DbHelperSQL.Query(mySql.ToString(), parameters).Tables[0];

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Qsid ");
            strSql.Append(" FROM Signin ");
            strSql.Append(" WHERE  Qsyear=@Syear AND Qgrade=@Sgrade AND Qclass=@Sclass AND Qyear=@Qyear AND Qmonth=@Qmonth AND Qday=@Qday  order by Qsid ");
            DataTable dtsignin = DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
            int sgcount = dtsignin.Rows.Count;
            if (sgcount > 0)
            {
                int stucount = dtstu.Rows.Count;
                if (sgcount < stucount)
                {
                    for (int i = 0; i < stucount; i++)
                    {
                        string Sid = dtstu.Rows[i]["Sid"].ToString();
                        for (int j = 0; j < sgcount; j++)
                        {
                            if (Sid == dtsignin.Rows[j]["Qsid"].ToString())
                            {
                                dtstu.Rows[i].Delete();//如果签到表中有记录则删除
                                break;
                            }
                        }
                    }
                    dtsignin.Dispose();//返回前清内存表
                    return dtstu;
                }
                else
                {
                    return null;//如果签到表人数少于班级人数，则执行上面代码，否则返回空
                }
            }
            else
            {
                return dtstu;
            }
        }

        /// <summary>
        /// 查询开始上课页面，班级没有签到列表
        /// </summary>
        /// <param name="Qgrade"></param>
        /// <param name="Qclass"></param>
        /// <param name="Qyear"></param>
        /// <param name="Qmonth"></param>
        /// <param name="Qday"></param>
        /// <returns></returns>
        public DataTable StartNoSignClassTwo(int Sgrade, int Sclass, int Syear, int Qyear, int Qmonth, int Qday)
        {
            StringBuilder mySql = new StringBuilder();
            mySql.Append("select Sid, Snum,Sname,Sscore");
            mySql.Append(" FROM Students  ");
            mySql.Append(" WHERE Sgrade=@Sgrade AND Sclass=@Sclass AND Sid not in ");

            mySql.Append("(select Sid ");
            mySql.Append(" FROM Signin,Students ");
            mySql.Append(" WHERE  Sgrade=@Sgrade AND Sclass=@Sclass AND Sid=Qsid  AND Qyear=@Qyear AND Qmonth=@Qmonth AND Qday=@Qday  ");
            mySql.Append(" ) order by Sid");

            SqlParameter[] parameters = {
					new SqlParameter("@Sgrade", SqlDbType.Int,4),
                    new SqlParameter("@Sclass", SqlDbType.Int,4),
                    new SqlParameter("@Syear", SqlDbType.Int,4),
                    new SqlParameter("@Qyear", SqlDbType.Int,4),                    
                    new SqlParameter("@Qmonth", SqlDbType.Int,4),                    
                    new SqlParameter("@Qday", SqlDbType.Int,4)};

            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;
            parameters[2].Value = Syear;
            parameters[3].Value = Qyear;
            parameters[4].Value = Qmonth;
            parameters[5].Value = Qday;

            return DbHelperSQL.Query(mySql.ToString(), parameters).Tables[0];
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Qid,Qnum,Qattitude,Qdate,Qyear,Qmonth,Qday,Qweek,Qip,Qmachine,Qnote,Qwork,Qgrade,Qterm ");
            strSql.Append(" FROM Signin ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }
        /// <summary>
        /// 判断当前登录的IP是否与学生固定座位对应的IP一致
        /// 注意：优先检查临时座位，过期则使用固定座位
        /// </summary>
        /// <param name="Qnum">学号</param>
        /// <param name="LoginIp">当前登录IP</param>
        /// <returns>true:允许登录(IP匹配或未设置固定IP); false:IP不匹配，拒绝登录</returns>
        public bool IsSameIp(string Qnum,string LoginIp)
        {
            // 1. 先检查是否有有效的临时座位
            LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
            DataTable tempSeat = sbll.GetTempSeat(Qnum);

            if (tempSeat != null && tempSeat.Rows.Count > 0)
            {
                string tempIp = tempSeat.Rows[0]["TempIp"].ToString();
                // 如果临时座位IP为空，表示允许在任意空位登录
                if (string.IsNullOrEmpty(tempIp))
                {
                    return true;  // 允许在任意空位登录
                }
                if (tempIp == LoginIp)
                {
                    return true;  // 临时IP匹配，允许登录
                }
                return false; // 临时IP不匹配，拒绝登录
            }

            // 2. 没有临时座位，检查固定座位
            // 从Students表获取学生的固定座位号
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select Sseat from Students ");
            strSql.Append(" where Snum=@Snum ");

            SqlParameter[] parameters = {
					new SqlParameter("@Snum", SqlDbType.VarChar)};

            parameters[0].Value = Qnum;

            string seatNum = DbHelperSQL.FindString(strSql.ToString(),parameters);

            // 如果学生没有设置固定座位，允许登录（学生首次登录时会自动记录当前座位为固定座位）
            if (string.IsNullOrEmpty(seatNum))
            {
                return true;
            }

            // 从Computers表查询该座位号对应的IP
            LearnSite.BLL.Computers cbll = new LearnSite.BLL.Computers();
            string fixedIp = cbll.GetIpByMachine(seatNum);

            // 如果Computers表中没有该座位号，允许登录
            if (string.IsNullOrEmpty(fixedIp))
            {
                return true;
            }

            // 如果登录IP与固定座位对应的IP相同，允许登录
            if (fixedIp == LoginIp)
            {
                return true;
            }

            // IP不同，不允许登录（需要教师通过座位管理清空或重新分配固定座位）
            return false;
        }

        /// <summary>
        /// 获取最近三个月内本机登录过的学生姓名
        /// </summary>
        /// <param name="Qip"></param>
        /// <returns></returns>
        public DataSet GetIpStudents(string Qip)
        {
            DateTime dt = DateTime.Now.AddMonths(-3);
            string mysql = "select Qip,Qdate,Qmachine,Sname,Sgrade,Sclass,Sex,Sscore,Saddress,Sheadtheacher from Signin,Students where Qnum=Snum and Qip='"+Qip+"' and Qdate>'"+dt.ToString()+"' order by Qdate desc";
            return DbHelperSQL.Query(mysql);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" Qid,Qnum,Qattitude,Qdate,Qyear,Qmonth,Qday,Qweek,Qip,Qmachine,Qnote,Qwork,Qgrade,Qterm ");
            strSql.Append(" FROM Signin ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }
        /// <summary>
        /// 在签到表中删除学生表中不存在班级的学生
        /// </summary>
        public void Upgrade()
        {            
            string mysql = "delete Signin where Qnum not in (select Snum from Students )";
            DbHelperSQL.ExecuteSql(mysql);
        }
        /// <summary>
        ///签到，如果今天的签到记录不存在则增加一条
        ///签到参数：学号、日期、IP
        /// Qnum,Qattitude,Qdate,Qyear,Qmonth,Qday,Qweek,Qip,Qnote,Qwork
        /// </summary>
        /// <param name="smodel"></param>
        public int SigninToday(string Qnum, DateTime Qdate, string Qip, int Qgrade, int Qterm, int Qsid, string Qname, int Qclass, int Qsyear, string Qtitle, string Qsession)//新增, string Qtitle, string Qsession
        {
            int sg = 0;
            int Qyear = Qdate.Year;
            int Qmonth = Qdate.Month;
            int Qday = Qdate.Day;
            BLL.Computers cbll = new BLL.Computers();
            string Qmachine = cbll.GetmachineByIp(Qip);
            string Qweek = Qdate.DayOfWeek.ToString();
            BLL.Room rbll = new BLL.Room();
            string cid= rbll.GetRcid(Qgrade, Qclass);

            Model.Signin qmodel = new LearnSite.Model.Signin();
            qmodel.Qnum = Qnum;
            qmodel.Qattitude = 0;
            qmodel.Qdate = Qdate;
            qmodel.Qyear = Qyear;
            qmodel.Qmonth = Qmonth;
            qmodel.Qday = Qday;
            qmodel.Qweek = Qweek;
            qmodel.Qip = Qip;
            qmodel.Qnote = "";
            qmodel.Qwork = 0;
            qmodel.Qgrade = Qgrade;
            qmodel.Qterm = Qterm;
            qmodel.Qmachine = Qmachine;
            qmodel.Qsid = Qsid;
            qmodel.Qname = Qname;
            qmodel.Qclass = Qclass;
            qmodel.Qsyear = Qsyear;
            // 如果教师未开始上课(cid为空)，清空课程标题和课时信息，避免显示错误的"信息技术"
            if (cid != "")
            {
                qmodel.Qtitle = Qtitle;
                qmodel.Qsession = Qsession;
                qmodel.Qcid = Int32.Parse(cid);
            }
            else
            {
                qmodel.Qtitle = "";
                qmodel.Qsession = "";
                qmodel.Qcid = 0;
            }
            

            string mysql = "select count(1) from Signin where Qsid=" + Qsid + " and Qday=" + Qday + " and Qmonth=" + Qmonth + " and Qyear=" + Qyear;
            if (!DbHelperSQL.Exists(mysql))
            {
                Add(qmodel);
                sg = 2;
            }
            else
            {
                string sqlstr = "update Signin set Qip='" + Qip + "',Qmachine='" + Qmachine + "' where Qsid=" + Qsid + " and Qyear=" + Qyear + " and Qmonth=" + Qmonth + " and Qday=" + Qday;
                DbHelperSQL.ExecuteSql(sqlstr);
                sg = 3;
            }
            return sg;
        }

        public  int UpdateSgroup(int Sgroup,string Qgroup,int Qgscore,int Qcid)
        {
            DateTime dt = DateTime.Now;
            string today = dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day;
            string mysql = "update Signin set Qgroup='" + Qgroup + "',Qgscore=" + Qgscore + ",Qcid=" + Qcid + " where Qdate>'" + today + "' and Qnum in (select Snum from Students where Sgroup=" + Sgroup + ")";
            return DbHelperSQL.ExecuteSql(mysql);
        }
        /// <summary>
        /// 删除该班级的签到记录
        /// </summary>
        /// <param name="Qgrade"></param>
        /// <param name="Qclass"></param>
        /// <param name="Qsyear"></param>
        /// <returns></returns>
        public int DelSignClass(int Qgrade, int Qclass, int Qsyear)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" delete from Signin ");
            strSql.Append(" WHERE Qgrade=@Qgrade AND Qclass=@Qclass AND Qsyear=@Qsyear");

            SqlParameter[] parameters = {
					new SqlParameter("@Qgrade", SqlDbType.Int,4),
                    new SqlParameter("@Qclass", SqlDbType.Int,4),
                    new SqlParameter("@Qsyear", SqlDbType.Int,4)};

            parameters[0].Value = Qgrade;
            parameters[1].Value = Qclass;
            parameters[2].Value = Qsyear;

            return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 获取最近一个月签到学生的姓名和IP列表
        /// Qsid,Qgrade,Qclass,Qname,Qip
        /// </summary>
        /// <param name="Rhid"></param>
        /// <returns></returns>
        public DataTable GetQnameQip(int Rhid, int weeks)
        {
            DateTime today = DateTime.Now;
            int days = 0 - weeks * 7;
            DateTime Qdate = today.AddDays(days);

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select Qsid,Qgrade,Qclass,Qname,Qip,Qdate,Sgroup,Sleader,Sgtitle from Signin,Room,Students  ");
            strSql.Append(" where Rhid=@Rhid and Rgrade=Sgrade and Rclass=Sclass and Qgrade=Sgrade and ");
            strSql.Append(" Qclass=Sclass and Qsid=Sid and Qdate>@Qdate order by Qsid,Qdate desc");

            SqlParameter[] parameters = {
					new SqlParameter("@Rhid", SqlDbType.Int,4),
					new SqlParameter("@Qdate", SqlDbType.DateTime)};

            parameters[0].Value = Rhid;
            parameters[1].Value = Qdate;

            DataTable dt = DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];

            //string teststr =LearnSite.Common.ClassModel.ClassModeSavePath(Rhid.ToString()) + @"\sql" + Rhid + "all.xml";
            //dt.WriteXml(teststr);//测试用
            dt.Columns.Remove("Qdate");//dt.Columns.Remove(dt.Columns["Qdate"]);//删除日期列，减少容量
            //string teststr2 = LearnSite.Common.ClassModel.ClassModeSavePath(Rhid.ToString()) + @"\sql2" + Rhid + "all.xml";
            //dt.WriteXml(teststr2);//测试用
            int dtcount = dt.Rows.Count;
            int lastQsid = 0;
            DataTable newTable = dt.Clone();
            for (int i = 0; i < dtcount; i++)
            {
                int Qsid = Int32.Parse(dt.Rows[i]["Qsid"].ToString());
                if (Qsid != lastQsid)
                {
                    lastQsid = Qsid;
                    newTable.ImportRow(dt.Rows[i]); //如果当前编号不等于旧的//将一行复制到另个表，同结构                  
                }
            }
           // string teststr3 = LearnSite.Common.ClassModel.ClassModeSavePath(Rhid.ToString()) + @"\sql3" + Rhid + "all.xml";
           // newTable.WriteXml(teststr3);//测试用
            return newTable;
        }
        /// <summary>
        /// 获得该本班本课的课堂表现
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Qcid"></param>
        /// <returns></returns>
        public DataTable GetClassListQattitude(int Sgrade, int Sclass, int Qcid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Qnum as Snum,Qattitude as Score ");
            strSql.Append(" FROM Signin ");
            string strWhere = " Qcid=" + Qcid + " and Qgrade=" + Sgrade + " and Qclass=" + Sclass + " order by Qnum ";
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString()).Tables[0];
        }
        /// <summary>
        /// 获取组长的小组表现分
        /// </summary>
        /// <param name="Qnum"></param>
        /// <param name="Qcid"></param>
        /// <returns></returns>
        public string GetLeaderQgroup(string Qnum, int Qcid)
        {
            string mysql = "select  Qgroup from Signin where Qnum='" + Qnum + "' and Qcid=" + Qcid;
            return DbHelperSQL.FindString(mysql);
        }
        /// <summary>
        /// 获取组长的小组表现分
        /// </summary>
        /// <param name="Qsid"></param>
        /// <param name="Qcid"></param>
        /// <returns></returns>
        public int GetLeaderQgroup(int Qsid, int Qcid)
        {
            string mysql = "select  Qgscore from Signin where Qsid=" + Qsid + " and Qcid=" + Qcid;
            return DbHelperSQL.FindNum(mysql);
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
			parameters[0].Value = "Signin";
			parameters[1].Value = "ID";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/
        public System.Data.DataSet GetSigninListByYear(int year)
        {
            string sql = string.Format("SELECT * FROM Signin WHERE YEAR(Qdate) = {0} ORDER BY Qdate DESC", year);
            return DbHelperSQL.Query(sql);
        }
        public System.Data.DataSet GetAllSigninList()
        {
            string sql = "SELECT * FROM Signin ORDER BY Qdate DESC";
            return DbHelperSQL.Query(sql);
        }

        /// <summary>
        /// 获取指定年级班级当天的签到列表
        /// </summary>
        /// <param name="Sgrade">年级</param>
        /// <param name="Sclass">班级</param>
        /// <returns>签到数据集</returns>
        public DataSet GetTodaySigninList(int Sgrade, int Sclass)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT s.Qid, s.Qnum, st.Sname, s.Qdate, s.Qmachine, s.Qip, s.Qattitude, s.Qnote ");
            strSql.Append("FROM Signin s ");
            strSql.Append("INNER JOIN Students st ON s.Qnum = st.Snum ");
            strSql.Append("WHERE s.Qgrade = @Sgrade ");
            strSql.Append("AND s.Qclass = @Sclass ");
            strSql.Append("AND CONVERT(nvarchar(10), s.Qdate, 120) = CONVERT(nvarchar(10), GETDATE(), 120) ");
            strSql.Append("ORDER BY s.Qdate DESC");

            SqlParameter[] parameters = {
                new SqlParameter("@Sgrade", SqlDbType.Int, 4),
                new SqlParameter("@Sclass", SqlDbType.Int, 4)
            };
            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;


            return DbHelperSQL.Query(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 获取指定学生当天的表现分
        /// </summary>
        /// <param name="Snum">学号</param>
        /// <returns>表现分</returns>
        public int GetTodayAttitudeBySnum(string Snum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Qattitude FROM Signin ");
            strSql.Append("WHERE Qnum = @Snum ");
            strSql.Append("AND CONVERT(nvarchar(10), Qdate, 120) = CONVERT(nvarchar(10), GETDATE(), 120)");

            SqlParameter[] parameters = {
                new SqlParameter("@Snum", SqlDbType.NVarChar, 50)
            };
            parameters[0].Value = Snum;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                object value = ds.Tables[0].Rows[0]["Qattitude"];
                return value != DBNull.Value ? Convert.ToInt32(value) : 0;
            }
            return 0;
        }

        /// <summary>
        /// 获取指定学生最新的表现评语
        /// </summary>
        /// <param name="Snum">学号</param>
        /// <returns>表现评语</returns>
        public string GetLatestAttitudeNote(string Snum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT TOP 1 Qnote FROM Signin ");
            strSql.Append("WHERE Qnum = @Snum ");
            strSql.Append("AND (Qnote IS NOT NULL AND Qnote != '') ");
            strSql.Append("ORDER BY Qdate DESC");

            SqlParameter[] parameters = {
                new SqlParameter("@Snum", SqlDbType.NVarChar, 50)
            };
            parameters[0].Value = Snum;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0]["Qnote"].ToString();
            }
            return "暂无评语";
        }

        /// <summary>
        /// 批量给当天已签到的学生加学习表现分
        /// </summary>
        /// <param name="Sgrade">年级</param>
        /// <param name="Sclass">班级</param>
        /// <param name="addScore">加分数值</param>
        /// <param name="reason">加分原因</param>
        /// <returns>影响的学生数量</returns>
        public int BatchAddAttitudeScore(int Sgrade, int Sclass, int addScore, string reason)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Signin ");
            strSql.Append("SET Qattitude = ISNULL(Qattitude, 0) + @AddScore, ");
            strSql.Append("Qnote = ISNULL(Qnote, '') + '; +' + @Reason ");
            strSql.Append("WHERE Qgrade = @Sgrade ");
            strSql.Append("AND Qclass = @Sclass ");
            strSql.Append("AND CONVERT(nvarchar(10), Qdate, 120) = CONVERT(nvarchar(10), GETDATE(), 120)");

            SqlParameter[] parameters = {
                new SqlParameter("@Sgrade", SqlDbType.Int, 4),
                new SqlParameter("@Sclass", SqlDbType.Int, 4),
                new SqlParameter("@AddScore", SqlDbType.Int, 4),
                new SqlParameter("@Reason", SqlDbType.NVarChar, 200)
            };
            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;
            parameters[2].Value = addScore;
            parameters[3].Value = reason;

            return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 批量给当天已签到的学生扣学习表现分
        /// </summary>
        /// <param name="Sgrade">年级</param>
        /// <param name="Sclass">班级</param>
        /// <param name="subScore">扣分数值</param>
        /// <param name="reason">扣分原因</param>
        /// <returns>影响的学生数量</returns>
        public int BatchSubAttitudeScore(int Sgrade, int Sclass, int subScore, string reason)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Signin ");
            strSql.Append("SET Qattitude = ISNULL(Qattitude, 0) - @SubScore, ");
            strSql.Append("Qnote = ISNULL(Qnote, '') + '; -' + @Reason ");
            strSql.Append("WHERE Qgrade = @Sgrade ");
            strSql.Append("AND Qclass = @Sclass ");
            strSql.Append("AND CONVERT(nvarchar(10), Qdate, 120) = CONVERT(nvarchar(10), GETDATE(), 120)");

            SqlParameter[] parameters = {
                new SqlParameter("@Sgrade", SqlDbType.Int, 4),
                new SqlParameter("@Sclass", SqlDbType.Int, 4),
                new SqlParameter("@SubScore", SqlDbType.Int, 4),
                new SqlParameter("@Reason", SqlDbType.NVarChar, 200)
            };
            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;
            parameters[2].Value = subScore;
            parameters[3].Value = reason;

            return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 给选中的学生加学习表现分
        /// </summary>
        /// <param name="selectedIds">选中的签到记录ID列表</param>
        /// <param name="addScore">加分数值</param>
        /// <param name="reason">加分原因</param>
        /// <returns>影响的学生数量</returns>
        public int SelectedAddAttitudeScore(List<int> selectedIds, int addScore, string reason)
        {
            if (selectedIds == null || selectedIds.Count == 0)
                return 0;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Signin ");
            strSql.Append("SET Qattitude = ISNULL(Qattitude, 0) + @AddScore, ");
            strSql.Append("Qnote = ISNULL(Qnote, '') + '; +' + @Reason ");
            strSql.Append("WHERE Qid IN (");

            for (int i = 0; i < selectedIds.Count; i++)
            {
                strSql.Append("@Qid" + i);
                if (i < selectedIds.Count - 1)
                    strSql.Append(",");
                else
                    strSql.Append(")");
            }

            SqlParameter[] parameters = new SqlParameter[selectedIds.Count + 2];
            parameters[0] = new SqlParameter("@AddScore", SqlDbType.Int, 4);
            parameters[1] = new SqlParameter("@Reason", SqlDbType.NVarChar, 200);
            parameters[0].Value = addScore;
            parameters[1].Value = reason;

            for (int i = 0; i < selectedIds.Count; i++)
            {
                parameters[i + 2] = new SqlParameter("@Qid" + i, SqlDbType.Int, 4);
                parameters[i + 2].Value = selectedIds[i];
            }

            return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 给选中的学生扣学习表现分
        /// </summary>
        /// <param name="selectedIds">选中的签到记录ID列表</param>
        /// <param name="subScore">扣分数值</param>
        /// <param name="reason">扣分原因</param>
        /// <returns>影响的学生数量</returns>
        public int SelectedSubAttitudeScore(List<int> selectedIds, int subScore, string reason)
        {
            if (selectedIds == null || selectedIds.Count == 0)
                return 0;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Signin ");
            strSql.Append("SET Qattitude = ISNULL(Qattitude, 0) - @SubScore, ");
            strSql.Append("Qnote = ISNULL(Qnote, '') + '; -' + @Reason ");
            strSql.Append("WHERE Qid IN (");

            for (int i = 0; i < selectedIds.Count; i++)
            {
                strSql.Append("@Qid" + i);
                if (i < selectedIds.Count - 1)
                    strSql.Append(",");
                else
                    strSql.Append(")");
            }

            SqlParameter[] parameters = new SqlParameter[selectedIds.Count + 2];
            parameters[0] = new SqlParameter("@SubScore", SqlDbType.Int, 4);
            parameters[1] = new SqlParameter("@Reason", SqlDbType.NVarChar, 200);
            parameters[0].Value = subScore;
            parameters[1].Value = reason;

            for (int i = 0; i < selectedIds.Count; i++)
            {
                parameters[i + 2] = new SqlParameter("@Qid" + i, SqlDbType.Int, 4);
                parameters[i + 2].Value = selectedIds[i];
            }

            return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 根据学生ID和课程ID获取签到记录
        /// </summary>
        /// <param name="Sid">学生ID</param>
        /// <param name="Cid">课程ID</param>
        /// <returns>签到记录列表</returns>
        public DataTable GetStudentSigninBySidCid(int Sid, int Cid)
        {
            string mysql = "select Qattitude from Signin where Qcid=" + Cid + " and Qsid=" + Sid;
            return DbHelperSQL.Query(mysql).Tables[0];
        }

        /// <summary>
        /// 为未签到的学生加分（更新Students表的Sscore字段）
        /// </summary>
        /// <param name="Sgrade">年级</param>
        /// <param name="Sclass">班级</param>
        /// <param name="score">加分数值</param>
        /// <param name="reason">加分原因</param>
        /// <returns>影响的学生数量</returns>
        public int AddScoreToUnsignStudents(int Sgrade, int Sclass, int score, string reason)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Students SET Sscore = ISNULL(Sscore, 0) + @Score ");
            strSql.Append("WHERE Sgrade = @Sgrade AND Sclass = @Sclass ");
            strSql.Append("AND Sid NOT IN (");
            strSql.Append("SELECT Qsid FROM Signin ");
            strSql.Append("WHERE Qgrade = @Sgrade AND Qclass = @Sclass ");
            strSql.Append("AND Qyear = YEAR(GETDATE()) AND Qmonth = MONTH(GETDATE()) AND Qday = DAY(GETDATE())");
            strSql.Append(")");

            SqlParameter[] parameters = {
                new SqlParameter("@Sgrade", SqlDbType.Int, 4),
                new SqlParameter("@Sclass", SqlDbType.Int, 4),
                new SqlParameter("@Score", SqlDbType.Int, 4)
            };
            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;
            parameters[2].Value = score;

            return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 为未签到的学生减分（更新Students表的Sscore字段）
        /// </summary>
        /// <param name="Sgrade">年级</param>
        /// <param name="Sclass">班级</param>
        /// <param name="score">扣分数值</param>
        /// <param name="reason">扣分原因</param>
        /// <returns>影响的学生数量</returns>
        public int SubScoreToUnsignStudents(int Sgrade, int Sclass, int score, string reason)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Students SET Sscore = ISNULL(Sscore, 0) - @Score ");
            strSql.Append("WHERE Sgrade = @Sgrade AND Sclass = @Sclass ");
            strSql.Append("AND Sid NOT IN (");
            strSql.Append("SELECT Qsid FROM Signin ");
            strSql.Append("WHERE Qgrade = @Sgrade AND Qclass = @Sclass ");
            strSql.Append("AND Qyear = YEAR(GETDATE()) AND Qmonth = MONTH(GETDATE()) AND Qday = DAY(GETDATE())");
            strSql.Append(")");

            SqlParameter[] parameters = {
                new SqlParameter("@Sgrade", SqlDbType.Int, 4),
                new SqlParameter("@Sclass", SqlDbType.Int, 4),
                new SqlParameter("@Score", SqlDbType.Int, 4)
            };
            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;
            parameters[2].Value = score;

            return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

		#endregion  成员方法
	}
}

