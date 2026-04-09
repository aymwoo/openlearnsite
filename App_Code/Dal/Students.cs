using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Data.SqlClient;
using LearnSite.DBUtility;//请先添加引用
namespace LearnSite.DAL
{
	/// <summary>
	/// 数据访问类Students。
	/// </summary>
	public class Students
	{
		// 用于存储班级最大签到次数的静态字典
		public static Dictionary<string, int> maxSigninDict = new Dictionary<string, int>();
		// 用于存储班级最大签到次数对应的学生学号
		public static Dictionary<string, string> maxSigninStudentDict = new Dictionary<string, string>();

		public Students()
		{}
		#region  成员方法

        /// <summary>
        /// 初始化ztype打字成绩
        /// </summary>
        public void initSztype()
        {
            string mysql = "update Students set Sztype=0 where Sztype is null";
            DbHelperSQL.ExecuteSql(mysql);
        }
        /// <summary>
        /// 更新ztype打字成绩
        /// </summary>
        /// <param name="Sztype"></param>
        public void updateSztype(int Sztype)
        {
            Model.Cook cook = new Model.Cook();
            int Sid = cook.Sid;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Students set Sztype=Sztype+@Sztype ");
            strSql.Append(" where  Sid=@Sid");

            SqlParameter[] parameters = {
					new SqlParameter("@Sztype", SqlDbType.Int,4),
					new SqlParameter("@Sid", SqlDbType.Int,4)};
            parameters[0].Value = Sztype;
            parameters[1].Value = Sid;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters); 
        }

        /// <summary>
        /// 判断该学号是否允许个人模式登录
        /// </summary>
        /// <param name="Snum"></param>
        /// <returns></returns>
        public bool isRlogin(string Snum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct  count(1) from Room,Students ");
            strSql.Append(" where  Rlogin=1 and Sgrade=Rgrade and Snum=@Snum");
            SqlParameter[] parameters = {
					new SqlParameter("@Snum", SqlDbType.NVarChar,50)};
            parameters[0].Value = Snum;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 初始化中文拼音总计
        /// </summary>
        public void initSchinese()
        {
            string mysql = "update Students set Schinese=0 where Schinese is null";
            DbHelperSQL.ExecuteSql(mysql);
        }
        /// <summary>
        /// 初始化表单分数总计
        /// </summary>
        public void initStxtform()
        {
            string mysql = "update Students set Stxtform=0 where Stxtform is null";
            DbHelperSQL.ExecuteSql(mysql);
        }
        /// <summary>
        /// 初始化作业加分总计
        /// </summary>
        public void initSwdscore()
        {
            string mysql = "update Students set Swdscore=0 where Swdscore is null";
            DbHelperSQL.ExecuteSql(mysql);
        }
		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Sid", "Students"); 
		}

        public long maxGradeSnumInit(int Sgrade, int Sclass)
        {
            string strWhere = " Sgrade=" + Sgrade;
            string gsnum=DateTime.Now.Year.ToString()+Sgrade.ToString()+Sclass.ToString()+"001";
            long test = DbHelperSQL.FieldMaxValue("Snum", "Students", strWhere);
            if (test == 1)
                return long.Parse(gsnum);
            else
                return test + 1;
        }


        /// <summary>
        /// 得到最大Snum　；不取班级了
        /// </summary>
        public long GetMaxSnum(int Sgrade, int Sclass)
        {
            string strGradeWhere = " Sgrade=" + Sgrade;
            long maxGradeSnum = maxGradeSnumInit(Sgrade, Sclass);

            if (!ExistsSnum(maxGradeSnum.ToString()))
            {
                return maxGradeSnum;//不存在，则返回年级学号最大值
            }
            else
            {
                long tt = DbHelperSQL.FieldMaxValueNoWhere("Snum", "Students") + 1;
                return tt;//不存在，则返回全校学号最大值
            }
        }
        /// <summary>
        /// 是否存在该学号
        /// </summary>
        public bool ExistsSnum(string Snum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Students");
            strSql.Append(" where Snum=@Snum ");
            SqlParameter[] parameters = {
					new SqlParameter("@Snum", SqlDbType.NVarChar,50)};
            parameters[0].Value = Snum;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Sid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Students");
			strSql.Append(" where Sid=@Sid ");
			SqlParameter[] parameters = {
					new SqlParameter("@Sid", SqlDbType.Int,4)};
			parameters[0].Value = Sid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 班级测评情况
        /// </summary>
        public DataTable SolveAll(int Sgrade,int Sclass,int Nid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Sid, Sgrade as 年级,Sclass as 班级,Sname as 姓名 from Students");
            strSql.Append(" where Sgrade=@Sgrade and Sclass=@Sclass ");
            SqlParameter[] parameters = {
					new SqlParameter("@Sgrade", SqlDbType.Int,4),
					new SqlParameter("@Sclass", SqlDbType.Int,4)};
            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;

            DataTable dt = DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
            int scount=dt.Rows.Count;

            BLL.Problems pbll = new BLL.Problems();
            DataTable pdt= pbll.GetListNidTable(Nid);
            int pcount = pdt.Rows.Count;
            if (pcount > 0)
            {
                //将所有题目添加到学生表的列中
                for (int i = 0; i < pcount; i++)
                {
                    int n = i + 1;
                    string clm = "第" + n.ToString() + "题";
                    dt.Columns.Add(clm);
                }
                BLL.Solves vbll = new BLL.Solves();
                //添加每一位学生成绩
                for (int j = 0; j < scount; j++)
                {
                    int sid = Int32.Parse( dt.Rows[j][0].ToString());
                    for (int k = 0; k < pcount; k++)
                    {
                        int m = k + 1;
                        string clm = "第" + m.ToString() + "题";
                        int pid = Int32.Parse(pdt.Rows[k][0].ToString());
                        string vscore = vbll.GetScore(pid, sid);
                        if (!string.IsNullOrEmpty(vscore)) vscore = "√";
                        dt.Rows[j][clm] = vscore;
                    }

                }

            }
            dt.Columns.Remove("Sid");
            return dt;
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LearnSite.Model.Students model)
		{
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Students(");
            strSql.Append("Snum,Syear,Sgrade,Sclass,Sname,Spwd,Sex,Saddress,Sphone,Sparents,Sheadtheacher,Sscore,Squiz,Sattitude,Sape)");
            strSql.Append(" values (");
            strSql.Append("@Snum,@Syear,@Sgrade,@Sclass,@Sname,@Spwd,@Sex,@Saddress,@Sphone,@Sparents,@Sheadtheacher,@Sscore,@Squiz,@Sattitude,@Sape)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Snum", SqlDbType.NVarChar,50),
					new SqlParameter("@Syear", SqlDbType.Int,4),
					new SqlParameter("@Sgrade", SqlDbType.Int,4),
					new SqlParameter("@Sclass", SqlDbType.Int,4),
					new SqlParameter("@Sname", SqlDbType.NVarChar,50),
					new SqlParameter("@Spwd", SqlDbType.NVarChar,50),
					new SqlParameter("@Sex", SqlDbType.NVarChar,2),
					new SqlParameter("@Saddress", SqlDbType.NVarChar,200),
					new SqlParameter("@Sphone", SqlDbType.NVarChar,50),
					new SqlParameter("@Sparents", SqlDbType.NVarChar,50),
					new SqlParameter("@Sheadtheacher", SqlDbType.NVarChar,50),
					new SqlParameter("@Sscore", SqlDbType.Int,4),
					new SqlParameter("@Squiz", SqlDbType.Int,4),
					new SqlParameter("@Sattitude", SqlDbType.Int,4),
					new SqlParameter("@Sape", SqlDbType.NVarChar,10)};
            parameters[0].Value = model.Snum;
            parameters[1].Value = model.Syear;
            parameters[2].Value = model.Sgrade;
            parameters[3].Value = model.Sclass;
            parameters[4].Value = model.Sname;
            parameters[5].Value = model.Spwd;
            parameters[6].Value = model.Sex;
            parameters[7].Value = model.Saddress;
            parameters[8].Value = model.Sphone;
            parameters[9].Value = model.Sparents;
            parameters[10].Value = model.Sheadtheacher;
            parameters[11].Value = model.Sscore;
            parameters[12].Value = model.Squiz;
            parameters[13].Value = model.Sattitude;
            parameters[14].Value = model.Sape;

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
        /// 增加一位学生
        /// </summary>
        public int AddStudent(LearnSite.Model.Students model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("if not exists(select Snum from Students where Snum=@Snum) ");
            strSql.Append("insert into Students(");
            strSql.Append("Snum,Syear,Sgrade,Sclass,Sname,Spwd,Sex,Saddress,Sphone,Sparents,Sheadtheacher,Sscore,Sattitude)");
            strSql.Append(" values (");
            strSql.Append("@Snum,@Syear,@Sgrade,@Sclass,@Sname,@Spwd,@Sex,@Saddress,@Sphone,@Sparents,@Sheadtheacher,@Sscore,@Sattitude)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Snum", SqlDbType.NVarChar,50),
					new SqlParameter("@Syear", SqlDbType.Int,4),
					new SqlParameter("@Sgrade", SqlDbType.Int,4),
					new SqlParameter("@Sclass", SqlDbType.Int,4),
					new SqlParameter("@Sname", SqlDbType.NVarChar,50),
					new SqlParameter("@Spwd", SqlDbType.NVarChar,50),
					new SqlParameter("@Sex", SqlDbType.NVarChar,2),
					new SqlParameter("@Saddress", SqlDbType.NVarChar,200),
					new SqlParameter("@Sphone", SqlDbType.NVarChar,50),
					new SqlParameter("@Sparents", SqlDbType.NVarChar,50),
					new SqlParameter("@Sheadtheacher", SqlDbType.NVarChar,50),
					new SqlParameter("@Sscore", SqlDbType.Int,4),
					new SqlParameter("@Sattitude", SqlDbType.Int,4)};
            parameters[0].Value = model.Snum;
            parameters[1].Value = model.Syear;
            parameters[2].Value = model.Sgrade;
            parameters[3].Value = model.Sclass;
            parameters[4].Value = model.Sname;
            parameters[5].Value = model.Spwd;
            parameters[6].Value = model.Sex;
            parameters[7].Value = model.Saddress;
            parameters[8].Value = model.Sphone;
            parameters[9].Value = model.Sparents;
            parameters[10].Value = model.Sheadtheacher;
            parameters[11].Value = model.Sscore;
            parameters[12].Value = model.Sattitude;

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
        /// 更新本班学生的密码
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        public void UpdateMyClassPwd(int Sgrade,int Sclass,string Spwd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Students set Spwd=@Spwd ");
            strSql.Append("where Sgrade=@Sgrade and Sclass=@Sclass ");
            SqlParameter[] parameters = {
					new SqlParameter("@Sgrade", SqlDbType.Int,4),
					new SqlParameter("@Sclass", SqlDbType.Int,4),
					new SqlParameter("@Spwd", SqlDbType.NVarChar,50)};
            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;
            parameters[2].Value = Spwd;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新该学号学生的密码
        /// </summary>
        /// <param name="Snum"></param>
        /// <param name="Spwd"></param>
        public void UpdatePwd(string Snum, string Spwd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Students set Spwd=@Spwd  ");
            strSql.Append("where Snum=@Snum ");
            SqlParameter[] parameters = {
					new SqlParameter("@Snum", SqlDbType.NVarChar,50),
					new SqlParameter("@Spwd", SqlDbType.NVarChar,50)};
            parameters[0].Value = Snum;
            parameters[1].Value = Spwd;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新该Sid学生的密码
        /// </summary>
        /// <param name="Sid"></param>
        /// <param name="Spwd"></param>
        public void UpdateSidPwd(string Sid, string Spwd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Students set Spwd=@Spwd  ");
            strSql.Append("where Sid=@Sid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Sid", SqlDbType.Int,4),
					new SqlParameter("@Spwd", SqlDbType.NVarChar,50)};
            parameters[0].Value = Sid;
            parameters[1].Value = Spwd;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新该学号学生的性别
        /// </summary>
        /// <param name="Snum"></param>
        /// <param name="Sex"></param>
        public void UpdateSex(string Snum, string Sex)
        {
            string mysql = "update Students set Sex='" + Sex + "'  where Snum='" + Snum + "'";
            DbHelperSQL.ExecuteSql(mysql);
        }
        /// <summary>
        /// 对该生换班
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Sname"></param>
        /// <returns></returns>
        public bool UpdateDivide(int Sgrade, int Sclass, string Sname)
        {
            bool Okd = false;
            string sqlstr = "select count(*) from Students where Sgrade=" + Sgrade + " and Sname='" + Sname + "'";
            string fstr = DbHelperSQL.FindString(sqlstr);//查找该年级姓名学生人数
            int fcount = 0;
            if (fstr != "")
                fcount = Int32.Parse(fstr);
            if (fcount == 1)//如果刚好一位，则进行分班
            {
                string mysql = "update Students set Sclass=" + Sclass + " where Sgrade=" + Sgrade + " and Sname='" + Sname + "'";
                DbHelperSQL.ExecuteSql(mysql);
                Okd = true;
            }
            return Okd;
        }
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LearnSite.Model.Students model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Students set ");
			strSql.Append("Syear=@Syear,");
			strSql.Append("Sgrade=@Sgrade,");
			strSql.Append("Sclass=@Sclass,");
			strSql.Append("Sname=@Sname,");
			strSql.Append("Spwd=@Spwd,");
			strSql.Append("Sex=@Sex,");
			strSql.Append("Saddress=@Saddress,");
			strSql.Append("Sphone=@Sphone,");
			strSql.Append("Sparents=@Sparents,");
			strSql.Append("Sheadtheacher=@Sheadtheacher");
			strSql.Append(" where Sid=@Sid ");
			SqlParameter[] parameters = {
					new SqlParameter("@Sid", SqlDbType.Int,4),
					new SqlParameter("@Syear", SqlDbType.Int,4),
					new SqlParameter("@Sgrade", SqlDbType.Int,4),
					new SqlParameter("@Sclass", SqlDbType.Int,4),
					new SqlParameter("@Sname", SqlDbType.NVarChar,50),
					new SqlParameter("@Spwd", SqlDbType.NVarChar,50),
					new SqlParameter("@Sex", SqlDbType.NVarChar,2),
					new SqlParameter("@Saddress", SqlDbType.NVarChar,200),
					new SqlParameter("@Sphone", SqlDbType.NVarChar,50),
					new SqlParameter("@Sparents", SqlDbType.NVarChar,50),
					new SqlParameter("@Sheadtheacher", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.Sid;
			parameters[1].Value = model.Syear;
			parameters[2].Value = model.Sgrade;
			parameters[3].Value = model.Sclass;
			parameters[4].Value = model.Sname;
			parameters[5].Value = model.Spwd;
			parameters[6].Value = model.Sex;
			parameters[7].Value = model.Saddress;
			parameters[8].Value = model.Sphone;
			parameters[9].Value = model.Sparents;
			parameters[10].Value = model.Sheadtheacher;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 更新学生的固定座位信息
		/// </summary>
		/// <param name="Snum">学号</param>
		/// <param name="Seat">座位号</param>
		public bool UpdateFixedSeat(string Snum, string Seat)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("update Students set Sseat=@Sseat");
			strSql.Append(" where Snum=@Snum");

			SqlParameter[] parameters = {
						new SqlParameter("@Snum", SqlDbType.NVarChar,50),
						new SqlParameter("@Sseat", SqlDbType.NVarChar,20)};

			parameters[0].Value = Snum;
			parameters[1].Value = Seat;

			int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
			return rows > 0;
		}

		/// <summary>
		/// 清空班级所有学生的固定座位信息
		/// </summary>
		/// <param name="Sgrade">年级</param>
		/// <param name="Sclass">班级</param>
		public void ClearAllSeats(int Sgrade, int Sclass)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("update Students set Sseat=null");
			strSql.Append(" where Sgrade=@Sgrade and Sclass=@Sclass");

			SqlParameter[] parameters = {
						new SqlParameter("@Sgrade", SqlDbType.Int,4),
						new SqlParameter("@Sclass", SqlDbType.Int,4)};

			parameters[0].Value = Sgrade;
			parameters[1].Value = Sclass;

			DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
		}

		/// <summary>
		/// 按学号顺序分配班级学生的固定座位（基于Computers表）
		/// </summary>
		/// <param name="Sgrade">年级</param>
		/// <param name="Sclass">班级</param>
		/// <param name="HouseId">机房ID</param>
		/// <returns>分配成功的学生数量</returns>
		public int AssignSeatsBySnum(int Sgrade, int Sclass, int HouseId)
		{
			// 获取班级学生按学号排序
			string strSql = @"SELECT Sid, Snum, Sname, Sgrade, Sclass
							FROM Students
							WHERE Sgrade=@Sgrade AND Sclass=@Sclass
							ORDER BY CONVERT(int, Snum) ASC";

			SqlParameter[] parameters = {
						new SqlParameter("@Sgrade", SqlDbType.Int,4),
						new SqlParameter("@Sclass", SqlDbType.Int,4)};

			parameters[0].Value = Sgrade;
			parameters[1].Value = Sclass;

			DataSet ds = DbHelperSQL.Query(strSql, parameters);
			if (ds.Tables[0].Rows.Count == 0)
				return 0;

			// 获取机房的IP列表（从Computers表）
			string sqlIp = @"SELECT Pmachine FROM Computers WHERE Pm=(SELECT Hname FROM House WHERE Hid=@Hid) ORDER BY Pmachine ASC";
			SqlParameter[] ipParams = { new SqlParameter("@Hid", SqlDbType.Int,4) };
			ipParams[0].Value = HouseId;
			DataSet dsIp = DbHelperSQL.Query(sqlIp, ipParams);

			if (dsIp.Tables[0].Rows.Count == 0)
				return 0;

			// 逐个分配座位
			int assignedCount = 0;
			foreach (DataRow rowStudent in ds.Tables[0].Rows)
			{
				string snum = rowStudent["Snum"].ToString();
				int seatIndex = assignedCount + 1; // 座位号从1开始

				// 如果Computers表中有足够的座位
				if (seatIndex <= dsIp.Tables[0].Rows.Count)
				{
					string seatNum = dsIp.Tables[0].Rows[seatIndex - 1]["Pmachine"].ToString();

					// 更新学生的固定座位（不再存储IP）
					UpdateFixedSeat(snum, seatNum);
					assignedCount++;
				}
				else
				{
					break;
				}
			}

			return assignedCount;
		}

	/// <summary>
	/// 清空指定学生的固定座位信息
	/// </summary>
	/// <param name="Snum">学号</param>
	public void ClearStudentSeat(string Snum)
	{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("update Students set Sseat=null");
			strSql.Append(" where Snum=@Snum");

			SqlParameter[] parameters = {
						new SqlParameter("@Snum", SqlDbType.NVarChar,50)};

			parameters[0].Value = Snum;

			DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
	}

	/// <summary>
	/// 为学生临时分配座位（只对当前节课有效）
	/// </summary>
	/// <param name="Snum">学号</param>
	/// <param name="TempIp">临时IP</param>
	/// <param name="TempSeat">临时座位号</param>
	/// <param name="ExpireMinutes">有效时长（分钟）</param>
	/// <returns>是否成功</returns>
	public bool TempAssignSeat(string Snum, string TempIp, string TempSeat, int ExpireMinutes)
	{
		// 先清理该学生的过期临时座位
		ClearExpiredTempSeat(Snum);

		// 添加新的临时座位记录
		StringBuilder strSql = new StringBuilder();
		strSql.Append("insert into TempSeat(");
		strSql.Append("Snum,TempIp,TempSeat,ExpireTime,CreateTime)");
		strSql.Append(" values(");
		strSql.Append("@Snum,@TempIp,@TempSeat,@ExpireTime,@CreateTime)");

		SqlParameter[] parameters = {
					new SqlParameter("@Snum", SqlDbType.NVarChar,50),
					new SqlParameter("@TempIp", SqlDbType.NVarChar,50),
					new SqlParameter("@TempSeat", SqlDbType.NVarChar,50),
					new SqlParameter("@ExpireTime", SqlDbType.DateTime),
					new SqlParameter("@CreateTime", SqlDbType.DateTime)};

		parameters[0].Value = Snum;
		parameters[1].Value = TempIp;
		parameters[2].Value = TempSeat;
		parameters[3].Value = DateTime.Now.AddMinutes(ExpireMinutes);
		parameters[4].Value = DateTime.Now;

		int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
		return rows > 0;
	}

	/// <summary>
	/// 清理过期的临时座位记录
	/// </summary>
	/// <param name="Snum">学号，如果为空则清理所有过期记录</param>
	/// <returns>清理的记录数</returns>
	public int ClearExpiredTempSeat(string Snum)
	{
		StringBuilder strSql = new StringBuilder();
		strSql.Append("delete from TempSeat where ExpireTime < @ExpireTime");

		List<SqlParameter> parameters = new List<SqlParameter>();
		parameters.Add(new SqlParameter("@ExpireTime", SqlDbType.DateTime) { Value = DateTime.Now });

		if (!string.IsNullOrEmpty(Snum))
		{
			strSql.Append(" and Snum=@Snum");
			parameters.Add(new SqlParameter("@Snum", SqlDbType.NVarChar, 50) { Value = Snum });
		}

		return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters.ToArray());
	}

	/// <summary>
	/// 清除指定学生的所有临时座位记录（包括未过期的）
	/// </summary>
	/// <param name="Snum">学号</param>
	/// <returns>清除的记录数</returns>
	public int ClearTempSeat(string Snum)
	{
		StringBuilder strSql = new StringBuilder();
		strSql.Append("delete from TempSeat where Snum=@Snum");

		SqlParameter[] parameters = {
					new SqlParameter("@Snum", SqlDbType.NVarChar,50)};

		parameters[0].Value = Snum;
		return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
	}

	/// <summary>
	/// 获取学生的有效临时座位信息
	/// </summary>
	/// <param name="Snum">学号</param>
	/// <returns>临时座位信息，如果不存在或已过期则返回null</returns>
	public DataTable GetTempSeat(string Snum)
	{
		// 先清理过期记录
		ClearExpiredTempSeat(Snum);

		StringBuilder strSql = new StringBuilder();
		strSql.Append("select top 1 Snum,TempIp,TempSeat,ExpireTime,CreateTime ");
		strSql.Append(" from TempSeat ");
		strSql.Append(" where Snum=@Snum and ExpireTime > @ExpireTime");
		strSql.Append(" order by CreateTime desc");

		SqlParameter[] parameters = {
					new SqlParameter("@Snum", SqlDbType.NVarChar,50),
					new SqlParameter("@ExpireTime", SqlDbType.DateTime)};

		parameters[0].Value = Snum;
		parameters[1].Value = DateTime.Now;

		return DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
	}

	/// <summary>
	/// 获取学生当天签到的机号
	/// </summary>
	/// <param name="Snum">学号</param>
	/// <returns>当天签到机号字符串</returns>
	public string GetTodayMachine(string Snum)
	{
		if (string.IsNullOrEmpty(Snum))
		{
			return "-";
		}

		DateTime dt = DateTime.Now;
		string today = dt.ToString("yyyy-MM-dd");

		StringBuilder strSql = new StringBuilder();
		strSql.Append("select top 1 Qmachine, Qdate ");
		strSql.Append(" from Signin ");
		strSql.Append(" where Qnum=@Snum and (CONVERT(varchar(10), Qdate, 120) = @Today OR Qdate >= @Today)");
		strSql.Append(" order by Qdate desc");

		SqlParameter[] parameters = {
					new SqlParameter("@Snum", SqlDbType.NVarChar,50),
					new SqlParameter("@Today", SqlDbType.NVarChar,50)};

		parameters[0].Value = Snum;
		parameters[1].Value = today;

		DataTable dtResult = DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
		if (dtResult != null && dtResult.Rows.Count > 0)
		{
			string machine = dtResult.Rows[0]["Qmachine"].ToString();
			string qdate = dtResult.Rows[0]["Qdate"].ToString();
			// 记录调试信息
			System.Diagnostics.Debug.WriteLine(string.Format("学号:{0}, 日期:{1}, 机号:{2}", Snum, qdate, machine));
			return !string.IsNullOrEmpty(machine) ? machine : "-";
		}

		// 如果没有找到当天的记录，尝试查找最近一次签到记录
		strSql.Clear();
		strSql.Append("select top 1 Qmachine, Qdate ");
		strSql.Append(" from Signin ");
		strSql.Append(" where Qnum=@Snum");
		strSql.Append(" order by Qdate desc");

		DataTable dtResult2 = DbHelperSQL.Query(strSql.ToString(), new SqlParameter("@Snum", Snum)).Tables[0];
		if (dtResult2 != null && dtResult2.Rows.Count > 0)
		{
			string machine = dtResult2.Rows[0]["Qmachine"].ToString();
			string qdate = dtResult2.Rows[0]["Qdate"].ToString();
			System.Diagnostics.Debug.WriteLine(string.Format("学号:{0} (最近记录), 日期:{1}, 机号:{2}", Snum, qdate, machine));
			return !string.IsNullOrEmpty(machine) ? machine : "-";
		}

		System.Diagnostics.Debug.WriteLine(string.Format("学号:{0} 无签到记录", Snum));
		return "-";
	}

	/// <summary>
	/// 为指定学生分配指定座位
	/// </summary>
	/// <param name="Snum">学号</param>
	/// <param name="HouseId">机房ID</param>
	/// <param name="SeatNum">座位号</param>
	/// <returns>是否成功</returns>
	public bool AssignSeatToStudent(string Snum, int HouseId, int SeatNum)
	{
			// 检查该机房的该座位号是否存在（从Ip表查询）
			string sqlIp = @"SELECT Iip FROM Ip WHERE Ihid=@Ihid AND Inum=@Inum";
			SqlParameter[] ipParams = {
						new SqlParameter("@Ihid", SqlDbType.Int,4),
						new SqlParameter("@Inum", SqlDbType.Int,4)};
			ipParams[0].Value = HouseId;
			ipParams[1].Value = SeatNum;

			object result = DbHelperSQL.GetSingle(sqlIp, ipParams);
			if (result == null || result == DBNull.Value)
				return false;

			// 更新学生的固定座位（只存储座位号）
			UpdateFixedSeat(Snum, SeatNum.ToString());
			return true;
	}

		/// <summary>
		/// 从Signin表导入学生登录机号到Students表（只导入座位号）
		/// </summary>
		/// <param name="Sgrade">年级</param>
		/// <param name="Sclass">班级</param>
		/// <returns>导入的学生数量</returns>
		public int ImportSeatsFromSignin(int Sgrade, int Sclass)
		{
			// 使用UPDATE语句将Signin表中每个学生最后一次登录的机号更新到Students表
			// 只更新Students表中Sseat为空或为NULL的记录
			StringBuilder strSql = new StringBuilder();
			strSql.Append("UPDATE s SET s.Sseat = signin.Qmachine ");
			strSql.Append("FROM Students s ");
			strSql.Append("INNER JOIN ( ");
			strSql.Append("    SELECT Qnum, Qmachine, ROW_NUMBER() OVER (PARTITION BY Qnum ORDER BY Qdate DESC, Qid DESC) AS rn ");
			strSql.Append("    FROM Signin ");
			strSql.Append("    WHERE Qgrade = @Sgrade AND Qclass = @Sclass ");
			strSql.Append("    AND Qmachine IS NOT NULL AND Qmachine != '' ");
			strSql.Append(") signin ON s.Snum = signin.Qnum AND signin.rn = 1 ");
			strSql.Append("WHERE s.Sgrade = @Sgrade AND s.Sclass = @Sclass ");
			strSql.Append("AND (s.Sseat IS NULL OR s.Sseat = '')");

			SqlParameter[] parameters = {
						new SqlParameter("@Sgrade", SqlDbType.Int,4),
						new SqlParameter("@Sclass", SqlDbType.Int,4)};
			parameters[0].Value = Sgrade;
			parameters[1].Value = Sclass;

			return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
		}

		/// <summary>
		/// 删除一条学生数据（删除学生表，网页表中的同学号）
		/// </summary>
		public void Delete(int Sid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Students ");
			strSql.Append(" where Sid=@Sid ");
			SqlParameter[] parameters = {
					new SqlParameter("@Sid", SqlDbType.Int,4)};
			parameters[0].Value = Sid;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);

		}

        /// <summary>
        /// 根据年级和班级 随机得到该班某个学生一个对象实体
        /// </summary>
        public LearnSite.Model.Students GetRndModel(int Sgrade, int Sclass)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Sid,Snum,Syear,Sgrade,Sclass,Sname,Spwd,Sex,Saddress,Sphone,Sparents,Sheadtheacher,Sscore,Squiz,Sattitude,Sape,Swscore,Stscore,Sallscore,Spscore,Sgroup,Sleader,Svote,Sgscore,Stxtform,Sidle,Sfixedip,Sseat  from Students ");
            strSql.Append(" where Sgrade=@Sgrade and Sclass=@Sclass ");
            strSql.Append(" order by  NewID()");
            SqlParameter[] parameters = {
                    new SqlParameter("@Sgrade", SqlDbType.Int,4),
                    new SqlParameter("@Sclass", SqlDbType.Int,4)};
            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;

            LearnSite.Model.Students model = new LearnSite.Model.Students();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Sid"].ToString() != "")
                {
                    model.Sid = int.Parse(ds.Tables[0].Rows[0]["Sid"].ToString());
                }
                model.Snum = ds.Tables[0].Rows[0]["Snum"].ToString();
                if (ds.Tables[0].Rows[0]["Syear"].ToString() != "")
                {
                    model.Syear = int.Parse(ds.Tables[0].Rows[0]["Syear"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sgrade"].ToString() != "")
                {
                    model.Sgrade = int.Parse(ds.Tables[0].Rows[0]["Sgrade"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sclass"].ToString() != "")
                {
                    model.Sclass = int.Parse(ds.Tables[0].Rows[0]["Sclass"].ToString());
                }
                model.Sname = ds.Tables[0].Rows[0]["Sname"].ToString();
                model.Spwd = ds.Tables[0].Rows[0]["Spwd"].ToString();
                model.Sex = ds.Tables[0].Rows[0]["Sex"].ToString();
                model.Saddress = ds.Tables[0].Rows[0]["Saddress"].ToString();
                model.Sphone = ds.Tables[0].Rows[0]["Sphone"].ToString();
                model.Sparents = ds.Tables[0].Rows[0]["Sparents"].ToString();
                model.Sheadtheacher = ds.Tables[0].Rows[0]["Sheadtheacher"].ToString();
                if (ds.Tables[0].Rows[0]["Sscore"].ToString() != "")
                {
                    model.Sscore = int.Parse(ds.Tables[0].Rows[0]["Sscore"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Squiz"].ToString() != "")
                {
                    model.Squiz = int.Parse(ds.Tables[0].Rows[0]["Squiz"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sattitude"].ToString() != "")
                {
                    model.Sattitude = int.Parse(ds.Tables[0].Rows[0]["Sattitude"].ToString());
                }
                model.Sape = ds.Tables[0].Rows[0]["Sape"].ToString();
                if (ds.Tables[0].Rows[0]["Swscore"].ToString() != "")
                {
                    model.Swscore = int.Parse(ds.Tables[0].Rows[0]["Swscore"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Stscore"].ToString() != "")
                {
                    model.Stscore = int.Parse(ds.Tables[0].Rows[0]["Stscore"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sallscore"].ToString() != "")
                {
                    model.Sallscore = int.Parse(ds.Tables[0].Rows[0]["Sallscore"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Spscore"].ToString() != "")
                {
                    model.Spscore = int.Parse(ds.Tables[0].Rows[0]["Spscore"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sgroup"].ToString() != "")
                {
                    model.Sgroup = int.Parse(ds.Tables[0].Rows[0]["Sgroup"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sleader"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Sleader"].ToString() == "1") || (ds.Tables[0].Rows[0]["Sleader"].ToString().ToLower() == "true"))
                    {
                        model.Sleader = true;
                    }
                    else
                    {
                        model.Sleader = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Svote"].ToString() != "")
                {
                    model.Svote = int.Parse(ds.Tables[0].Rows[0]["Svote"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sgscore"].ToString() != "")
                {
                    model.Sgscore = int.Parse(ds.Tables[0].Rows[0]["Sgscore"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Stxtform"].ToString() != "")
                {
                    model.Stxtform = int.Parse(ds.Tables[0].Rows[0]["Stxtform"].ToString());
                }

                if (ds.Tables[0].Rows[0]["Sidle"].ToString() != "")
                {
                    model.Sidle = int.Parse(ds.Tables[0].Rows[0]["Sidle"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sfixedip"] != null)
                {
                    model.Sfixedip = ds.Tables[0].Rows[0]["Sfixedip"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Sseat"] != null)
                {
                    model.Sseat = ds.Tables[0].Rows[0]["Sseat"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Sseat"] != null)
                {
                    model.Sseat = ds.Tables[0].Rows[0]["Sseat"].ToString();
                }
                return model;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 根据学号和密码 得到学生一个对象实体
        /// </summary>
        public LearnSite.Model.Students GetStudentModel(string Snum,string Spwd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Sid,Snum,Syear,Sgrade,Sclass,Sname,Spwd,Sex,Saddress,Sphone,Sparents,Sheadtheacher,Sscore,Squiz,Sattitude,Sape,Swscore,Stscore,Sallscore,Spscore,Sgroup,Sleader,Svote,Sgscore,Stxtform,Sidle,Sfixedip,Sseat from Students ");
            strSql.Append(" where Snum=@Snum and Spwd=@Spwd ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Snum", SqlDbType.NVarChar,50),
                    new SqlParameter("@Spwd", SqlDbType.NVarChar,50)};
            parameters[0].Value = Snum;
            parameters[1].Value = Spwd;

            LearnSite.Model.Students model = new LearnSite.Model.Students();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Sid"].ToString() != "")
                {
                    model.Sid = int.Parse(ds.Tables[0].Rows[0]["Sid"].ToString());
                }
                model.Snum = ds.Tables[0].Rows[0]["Snum"].ToString();
                if (ds.Tables[0].Rows[0]["Syear"].ToString() != "")
                {
                    model.Syear = int.Parse(ds.Tables[0].Rows[0]["Syear"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sgrade"].ToString() != "")
                {
                    model.Sgrade = int.Parse(ds.Tables[0].Rows[0]["Sgrade"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sclass"].ToString() != "")
                {
                    model.Sclass = int.Parse(ds.Tables[0].Rows[0]["Sclass"].ToString());
                }
                model.Sname = ds.Tables[0].Rows[0]["Sname"].ToString();
                model.Spwd = ds.Tables[0].Rows[0]["Spwd"].ToString();
                model.Sex = ds.Tables[0].Rows[0]["Sex"].ToString();
                model.Saddress = ds.Tables[0].Rows[0]["Saddress"].ToString();
                model.Sphone = ds.Tables[0].Rows[0]["Sphone"].ToString();
                model.Sparents = ds.Tables[0].Rows[0]["Sparents"].ToString();
                model.Sheadtheacher = ds.Tables[0].Rows[0]["Sheadtheacher"].ToString();
                if (ds.Tables[0].Rows[0]["Sscore"].ToString() != "")
                {
                    model.Sscore = int.Parse(ds.Tables[0].Rows[0]["Sscore"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Squiz"].ToString() != "")
                {
                    model.Squiz = int.Parse(ds.Tables[0].Rows[0]["Squiz"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sattitude"].ToString() != "")
                {
                    model.Sattitude = int.Parse(ds.Tables[0].Rows[0]["Sattitude"].ToString());
                }
                model.Sape = ds.Tables[0].Rows[0]["Sape"].ToString();
                if (ds.Tables[0].Rows[0]["Swscore"].ToString() != "")
                {
                    model.Swscore = int.Parse(ds.Tables[0].Rows[0]["Swscore"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Stscore"].ToString() != "")
                {
                    model.Stscore = int.Parse(ds.Tables[0].Rows[0]["Stscore"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sallscore"].ToString() != "")
                {
                    model.Sallscore = int.Parse(ds.Tables[0].Rows[0]["Sallscore"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Spscore"].ToString() != "")
                {
                    model.Spscore = int.Parse(ds.Tables[0].Rows[0]["Spscore"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sgroup"].ToString() != "")
                {
                    model.Sgroup = int.Parse(ds.Tables[0].Rows[0]["Sgroup"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sleader"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Sleader"].ToString() == "1") || (ds.Tables[0].Rows[0]["Sleader"].ToString().ToLower() == "true"))
                    {
                        model.Sleader = true;
                    }
                    else
                    {
                        model.Sleader = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Svote"].ToString() != "")
                {
                    model.Svote = int.Parse(ds.Tables[0].Rows[0]["Svote"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sgscore"].ToString() != "")
                {
                    model.Sgscore = int.Parse(ds.Tables[0].Rows[0]["Sgscore"].ToString());
                }

                if (ds.Tables[0].Rows[0]["Stxtform"].ToString() != "")
                {
                    model.Stxtform = int.Parse(ds.Tables[0].Rows[0]["Stxtform"].ToString());
                }

                if (ds.Tables[0].Rows[0]["Sidle"].ToString() != "")
                {
                    model.Sidle = int.Parse(ds.Tables[0].Rows[0]["Sidle"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sfixedip"] != null)
                {
                    model.Sfixedip = ds.Tables[0].Rows[0]["Sfixedip"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Sseat"] != null)
                {
                    model.Sseat = ds.Tables[0].Rows[0]["Sseat"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Sseat"] != null)
                {
                    model.Sseat = ds.Tables[0].Rows[0]["Sseat"].ToString();
                }
                return model;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 根据自动编号返回姓名
        /// </summary>
        /// <param name="Sid"></param>
        /// <returns></returns>
        public string GetSnameBySid(int Sid)
        {
            string mysql = "select Sname from Students where Sid="+Sid;
            return DbHelperSQL.FindString(mysql);
        }
        /// <summary>
        /// 根据学号返回姓名
        /// </summary>
        /// <param name="Sid"></param>
        /// <returns></returns>
        public string GetSnameBySnum(string Snum)
        {
            string mysql = "select Sname from Students where Snum='" + Snum+"'";
            return DbHelperSQL.FindString(mysql);
        }
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LearnSite.Model.Students GetModel(int Sid)
		{
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Sid,Snum,Syear,Sgrade,Sclass,Sname,Spwd,Sex,Saddress,Sphone,Sparents,Sheadtheacher,Sscore,Squiz,Sattitude,Sape,Swscore,Stscore,Sallscore,Spscore,Sgroup,Sleader,Svote,Sgscore,Stxtform,Sidle,Sfixedip,Sseat  from Students ");
            strSql.Append(" where Sid=@Sid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Sid", SqlDbType.Int,4)};
            parameters[0].Value = Sid;

            LearnSite.Model.Students model = new LearnSite.Model.Students();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Sid"].ToString() != "")
                {
                    model.Sid = int.Parse(ds.Tables[0].Rows[0]["Sid"].ToString());
                }
                model.Snum = ds.Tables[0].Rows[0]["Snum"].ToString();
                if (ds.Tables[0].Rows[0]["Syear"].ToString() != "")
                {
                    model.Syear = int.Parse(ds.Tables[0].Rows[0]["Syear"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sgrade"].ToString() != "")
                {
                    model.Sgrade = int.Parse(ds.Tables[0].Rows[0]["Sgrade"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sclass"].ToString() != "")
                {
                    model.Sclass = int.Parse(ds.Tables[0].Rows[0]["Sclass"].ToString());
                }
                model.Sname = ds.Tables[0].Rows[0]["Sname"].ToString();
                model.Spwd = ds.Tables[0].Rows[0]["Spwd"].ToString();
                model.Sex = ds.Tables[0].Rows[0]["Sex"].ToString();
                model.Saddress = ds.Tables[0].Rows[0]["Saddress"].ToString();
                model.Sphone = ds.Tables[0].Rows[0]["Sphone"].ToString();
                model.Sparents = ds.Tables[0].Rows[0]["Sparents"].ToString();
                model.Sheadtheacher = ds.Tables[0].Rows[0]["Sheadtheacher"].ToString();
                if (ds.Tables[0].Rows[0]["Sscore"].ToString() != "")
                {
                    model.Sscore = int.Parse(ds.Tables[0].Rows[0]["Sscore"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Squiz"].ToString() != "")
                {
                    model.Squiz = int.Parse(ds.Tables[0].Rows[0]["Squiz"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sattitude"].ToString() != "")
                {
                    model.Sattitude = int.Parse(ds.Tables[0].Rows[0]["Sattitude"].ToString());
                }
                model.Sape = ds.Tables[0].Rows[0]["Sape"].ToString();
                if (ds.Tables[0].Rows[0]["Swscore"].ToString() != "")
                {
                    model.Swscore = int.Parse(ds.Tables[0].Rows[0]["Swscore"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Stscore"].ToString() != "")
                {
                    model.Stscore = int.Parse(ds.Tables[0].Rows[0]["Stscore"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sallscore"].ToString() != "")
                {
                    model.Sallscore = int.Parse(ds.Tables[0].Rows[0]["Sallscore"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Spscore"].ToString() != "")
                {
                    model.Spscore = int.Parse(ds.Tables[0].Rows[0]["Spscore"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sgroup"].ToString() != "")
                {
                    model.Sgroup = int.Parse(ds.Tables[0].Rows[0]["Sgroup"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sleader"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Sleader"].ToString() == "1") || (ds.Tables[0].Rows[0]["Sleader"].ToString().ToLower() == "true"))
                    {
                        model.Sleader = true;
                    }
                    else
                    {
                        model.Sleader = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Svote"].ToString() != "")
                {
                    model.Svote = int.Parse(ds.Tables[0].Rows[0]["Svote"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sgscore"].ToString() != "")
                {
                    model.Sgscore = int.Parse(ds.Tables[0].Rows[0]["Sgscore"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Stxtform"].ToString() != "")
                {
                    model.Stxtform = int.Parse(ds.Tables[0].Rows[0]["Stxtform"].ToString());
                }

                if (ds.Tables[0].Rows[0]["Sidle"].ToString() != "")
                {
                    model.Sidle = int.Parse(ds.Tables[0].Rows[0]["Sidle"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sfixedip"] != null)
                {
                    model.Sfixedip = ds.Tables[0].Rows[0]["Sfixedip"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Sseat"] != null)
                {
                    model.Sseat = ds.Tables[0].Rows[0]["Sseat"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Sseat"] != null)
                {
                    model.Sseat = ds.Tables[0].Rows[0]["Sseat"].ToString();
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据学号，得到一个对象实体
        /// </summary>
        public LearnSite.Model.Students SnumGetModel(string Snum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Sid,Snum,Syear,Sgrade,Sclass,Sname,Spwd,Sex,Saddress,Sphone,Sparents,Sheadtheacher,Sscore,Squiz,Sattitude,Sape,Swscore,Stscore,Sallscore,Spscore,Sgroup,Sleader,Svote,Sgscore,Stxtform,Sidle,Sfixedip,Sseat  from Students");
            strSql.Append(" where Snum=@Snum ");
            SqlParameter[] parameters = {
					new SqlParameter("@Snum", SqlDbType.NVarChar,50)};
            parameters[0].Value = Snum;

            LearnSite.Model.Students model = new LearnSite.Model.Students();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Sid"].ToString() != "")
                {
                    model.Sid = int.Parse(ds.Tables[0].Rows[0]["Sid"].ToString());
                }
                model.Snum = ds.Tables[0].Rows[0]["Snum"].ToString();
                if (ds.Tables[0].Rows[0]["Syear"].ToString() != "")
                {
                    model.Syear = int.Parse(ds.Tables[0].Rows[0]["Syear"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sgrade"].ToString() != "")
                {
                    model.Sgrade = int.Parse(ds.Tables[0].Rows[0]["Sgrade"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sclass"].ToString() != "")
                {
                    model.Sclass = int.Parse(ds.Tables[0].Rows[0]["Sclass"].ToString());
                }
                model.Sname = ds.Tables[0].Rows[0]["Sname"].ToString();
                model.Spwd = ds.Tables[0].Rows[0]["Spwd"].ToString();
                model.Sex = ds.Tables[0].Rows[0]["Sex"].ToString();
                model.Saddress = ds.Tables[0].Rows[0]["Saddress"].ToString();
                model.Sphone = ds.Tables[0].Rows[0]["Sphone"].ToString();
                model.Sparents = ds.Tables[0].Rows[0]["Sparents"].ToString();
                model.Sheadtheacher = ds.Tables[0].Rows[0]["Sheadtheacher"].ToString();
                if (ds.Tables[0].Rows[0]["Sscore"].ToString() != "")
                {
                    model.Sscore = int.Parse(ds.Tables[0].Rows[0]["Sscore"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Squiz"].ToString() != "")
                {
                    model.Squiz = int.Parse(ds.Tables[0].Rows[0]["Squiz"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sattitude"].ToString() != "")
                {
                    model.Sattitude = int.Parse(ds.Tables[0].Rows[0]["Sattitude"].ToString());
                }
                model.Sape = ds.Tables[0].Rows[0]["Sape"].ToString();
                if (ds.Tables[0].Rows[0]["Swscore"].ToString() != "")
                {
                    model.Swscore = int.Parse(ds.Tables[0].Rows[0]["Swscore"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Stscore"].ToString() != "")
                {
                    model.Stscore = int.Parse(ds.Tables[0].Rows[0]["Stscore"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sallscore"].ToString() != "")
                {
                    model.Sallscore = int.Parse(ds.Tables[0].Rows[0]["Sallscore"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Spscore"].ToString() != "")
                {
                    model.Spscore = int.Parse(ds.Tables[0].Rows[0]["Spscore"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sgroup"].ToString() != "")
                {
                    model.Sgroup = int.Parse(ds.Tables[0].Rows[0]["Sgroup"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sleader"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Sleader"].ToString() == "1") || (ds.Tables[0].Rows[0]["Sleader"].ToString().ToLower() == "true"))
                    {
                        model.Sleader = true;
                    }
                    else
                    {
                        model.Sleader = false;
                    }
                }
                if (ds.Tables[0].Rows[0]["Svote"].ToString() != "")
                {
                    model.Svote = int.Parse(ds.Tables[0].Rows[0]["Svote"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sgscore"].ToString() != "")
                {
                    model.Sgscore = int.Parse(ds.Tables[0].Rows[0]["Sgscore"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Stxtform"].ToString() != "")
                {
                    model.Stxtform = int.Parse(ds.Tables[0].Rows[0]["Stxtform"].ToString());
                }

                if (ds.Tables[0].Rows[0]["Sidle"].ToString() != "")
                {
                    model.Sidle = int.Parse(ds.Tables[0].Rows[0]["Sidle"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Sfixedip"] != null)
                {
                    model.Sfixedip = ds.Tables[0].Rows[0]["Sfixedip"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Sseat"] != null)
                {
                    model.Sseat = ds.Tables[0].Rows[0]["Sseat"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Sseat"] != null)
                {
                    model.Sseat = ds.Tables[0].Rows[0]["Sseat"].ToString();
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 学生表基本数据导出Excel
        /// </summary>
        public void StudentsToExcel()
        {
            DateTime dt = DateTime.Now;
            string today = dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day;
            string FileName = "StudentsToExcel" + today;
            string strSql = "select Snum as 学号,Syear as 入学年度, Sgrade as 年级,Sclass as 班级, Sname as 姓名,Spwd as 密码,Sex as 性别,Saddress as 家庭住址,Sphone as 联系电话,Sparents as 家长姓名,Sheadtheacher as 班主任  FROM Students order by Sgrade asc,Sclass asc,Snum asc  ";
            DataSet ds = DbHelperSQL.Query(strSql);
            Common.DataExcel.DataSetToExcel(ds, FileName);
        }

        /// <summary>
        /// 最终成绩评定导出Excel
        /// </summary>
        public void TermExcel(int hid)
        {
            DateTime dt = DateTime.Now;
            string today = dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day;
            string FileName = "TermExcel" + today;
            string strSql = "select  Snum as 学号,(cast(Sgrade as nvarchar(20))+'.'+cast(Sclass as  nvarchar(20))) as 班级,Sname as 姓名,Sscore as 作品,Sgscore as 小组,Spscore as 讨论,Stxtform as 表单,Svscore as 调查,Swscore as 网页, Squiz as 测验,Schinese as 拼音,Sfscore as 英语,Stscore as 中文,Sattitude as 表现,Sallscore as 总分折算,Sape as 评定,Stenscore as 综合  FROM Students,Room where Sgrade=Rgrade and Sclass=Rclass and Rhid=" + hid.ToString() + " order by Sgrade asc,Sclass asc,Snum asc  ";
            DataSet ds = DbHelperSQL.Query(strSql);
            Common.DataExcel.DataSetToExcel(ds, FileName);
        }

        /// <summary>
        /// 批量更新所有学期总积分
        /// </summary>
        public void TeamScores()
        {
            string mysql = "UPDATE Students SET Sscore=ISNULL((SELECT SUM(Wscore) FROM Works WHERE Wnum=Students.Snum), 0)";
            DbHelperSQL.ExecuteSql(mysql);
        }
        /// <summary>
        /// 批量更新所教班级当前学期小组合作分
        /// </summary>
        public void ThisTeamGroupScores(int Rhid)
        {
            string strSql = " select Snum,Sgrade from Students,Room where Sgrade=Rgrade and Sclass=Rclass  and Rhid=" + Rhid;
            DataSet ds = DbHelperSQL.GetDataSet(strSql);
            int counts = ds.Tables[0].Rows.Count;
            int Cterm = Int32.Parse(LearnSite.Common.XmlHelp.GetTerm());
            if (counts > 0)
            {
                for (int i = 0; i < counts; i++)
                {
                    string Snum = ds.Tables[0].Rows[i]["Snum"].ToString();
                    if (ds.Tables[0].Rows[i]["Sgrade"].ToString() != "")
                    {
                        int Cobj = Int32.Parse(ds.Tables[0].Rows[i]["Sgrade"].ToString());
                        TotalmySgscore(Snum, Cterm, Cobj);
                    }
                }
            }
        }
        /// <summary>
        /// 统计单个学生小组合作得分
        /// </summary>
        /// <param name="Snum"></param>
        /// <param name="Cterm"></param>
        /// <param name="Cobj"></param>
        public void TotalmySgscore(string Snum, int Cterm, int Cobj)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Students SET Sgscore=");
            strSql.Append("(select ISNULL(sum(Gscore),0) from GroupWork WHERE ");
            strSql.Append("Ggrade=@Cobj  and Gterm=@Cterm  ");
            string str = "and Gstudents like '%" + Snum + "%' )";
            strSql.Append(str);
            strSql.Append(" where Snum=@Snum ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Snum", SqlDbType.NVarChar,50),
                    new SqlParameter("@Cterm", SqlDbType.Int,4),
					new SqlParameter("@Cobj", SqlDbType.Int,4)};

            parameters[0].Value = Snum;
            parameters[1].Value = Cterm;
            parameters[2].Value = Cobj;
            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 统计单个学生小组合作得分，这个有问题，统计不成功2013-4-11号发现
        /// </summary>
        /// <param name="Snum"></param>
        /// <param name="Cterm"></param>
        /// <param name="Cobj"></param>
        public void TotalmySgscorenew(string Snum, int Cterm, int Cobj)
        {
            string mysql = "select Snum from Students where Sid=(select top 1 Sgroup from Students where Snum='" + Snum + "')";
            string leadSnum = DbHelperSQL.FindString(mysql);
            if (!string.IsNullOrEmpty(leadSnum))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("UPDATE Students SET Sgscore=");
                strSql.Append("(select ISNULL(sum(Gscore),0) from GroupWork WHERE Gnum=@leadSnum and Gstudents like '%@Snum%' ");
                strSql.Append(" and Ggrade=@Cobj  and Gterm=@Cterm ) ");
                strSql.Append(" where Snum=@Snum ");
                SqlParameter[] parameters = {
                    new SqlParameter("@Snum", SqlDbType.NVarChar,50),
                    new SqlParameter("@Cterm", SqlDbType.Int,4),
					new SqlParameter("@Cobj", SqlDbType.Int,4),
                    new SqlParameter("@leadSnum", SqlDbType.NVarChar,50)};

                parameters[0].Value = Snum;
                parameters[1].Value = Cterm;
                parameters[2].Value = Cobj;
                parameters[3].Value = leadSnum;
                DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            }
        }


        public void TotalSgscore(string gstudnets,int Cobj,int Cterm)
        {
            string[] gstu = gstudnets.Split('_');
            int gcount = gstu.Length;
            if (gcount > 0)
            {
                StringBuilder sqlgw = new StringBuilder();
                foreach (string stu in gstu)
                {
                    sqlgw.Append("update Students set Sgscore=(select ISNULL(sum(Gscore),0) from GroupWork where Gterm=" + Cterm + " and Ggrade=" + Cobj + " and Gstudents like '%" + stu + "%' ) where Snum='" + stu + "';");
                }
                DbHelperSQL.ExecuteSql(sqlgw.ToString());//批量更新小组成员得分，避免N+1查询
            }
        }     
        /// <summary>
        /// 批量更新所教班级当前学期作品总积分和表现总积分、调查测验分、表单得分//ISNULL  COALESCE
        /// </summary>
        public void ThisTeamScoresNew(int Rhid)
        {
            string strSql = " select distinct Syear,Sgrade from Students,Room where Sgrade=Rgrade and Sclass=Rclass  and Rhid=" + Rhid;
            DataSet ds = DbHelperSQL.GetDataSet(strSql);
            int counts = ds.Tables[0].Rows.Count;
            int Cterm = Int32.Parse(LearnSite.Common.XmlHelp.GetTerm());
            if (counts > 0)
            {
                for (int i = 0; i < counts; i++)
                {
                    if (ds.Tables[0].Rows[i]["Sgrade"].ToString() != "")
                    {
                        int Syear = Int32.Parse(ds.Tables[0].Rows[i]["Syear"].ToString());
                        int Sgrade = Int32.Parse(ds.Tables[0].Rows[i]["Sgrade"].ToString());
                        AllClassTeamScoresNew(Sgrade, Syear, Cterm);//调用批量
                    }
                }
            }
        }
        /// <summary>
        /// 批量更新所有班级当前学期作品总积分和表现总积分、调查测验分、表单得分等（批量group方法）
        /// </summary>
        private void AllClassTeamScoresNew(int Sgrade, int Syear, int Cterm)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Students set Sscore=nc.ws  from( SELECT Wnum, ISNULL(SUM(Wscore)+SUM(Wdscore),0)as ws FROM Works ");
            strSql.Append(" where Wyear=@Syear and Wcid in ( ");
            strSql.Append("select Cid from Courses where Cterm=@Cterm and Cobj=@Sgrade");
            strSql.Append(" ) group by Wnum )as nc ");
            strSql.Append(" where Snum=nc.Wnum and Sgrade=@Sgrade ");

            SqlParameter[] parameters = {
					new SqlParameter("@Sgrade", SqlDbType.Int,4),
                    new SqlParameter("@Cterm", SqlDbType.Int,4),
                    new SqlParameter("@Syear", SqlDbType.Int,4)};

            parameters[0].Value = Sgrade;
            parameters[1].Value = Cterm;
            parameters[2].Value = Syear;
            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);//统计班级当前学期作品总积分

            StringBuilder mySql = new StringBuilder();
            mySql.Append("update Students set Sattitude=qt.sqe  from( SELECT Qnum, ISNULL(SUM(Qattitude),0)as sqe FROM Signin ");
            mySql.Append(" where Qsyear=@Syear and Qgrade=@Sgrade and Qterm=@Cterm  group by Qnum )as qt ");
            mySql.Append(" where Snum=qt.Qnum and Sgrade=@Sgrade ");

            DbHelperSQL.ExecuteSql(mySql.ToString(), parameters);//统计班级当前学期表现分

            StringBuilder mySqlv = new StringBuilder();
            mySqlv.Append("update Students set Svscore=sv.sqe  from( SELECT Fnum, ISNULL(SUM(Fscore),0)as sqe FROM SurveyFeedback ");
            mySqlv.Append(" where Fvtype=1 and Fyear=@Syear and Fgrade=@Sgrade and Fterm=@Cterm  group by Fnum )as sv ");
            mySqlv.Append(" where Snum=sv.Fnum and Sgrade=@Sgrade ");

            DbHelperSQL.ExecuteSql(mySqlv.ToString(), parameters);//统计班级当前学期调查分

            StringBuilder mySqlp = new StringBuilder();
            mySqlp.Append("update Students set Spscore=sp.sqe  from( SELECT Rsnum, ISNULL(SUM(Rscore),0)as sqe FROM TopicReply ");
            mySqlp.Append(" where Ryear=@Syear and Rgrade=@Sgrade and Rterm=@Cterm  group by Rsnum )as sp ");
            mySqlp.Append(" where Snum=sp.Rsnum and Sgrade=@Sgrade ");

            DbHelperSQL.ExecuteSql(mySqlp.ToString(), parameters);//统计班级当前学期主题讨论分

            StringBuilder mySqlf = new StringBuilder();
            mySqlf.Append("update Students set Stxtform=sp.sqe  from( SELECT Rsnum, ISNULL(SUM(Rscore),0)as sqe FROM TxtFormBack ");
            mySqlf.Append(" where Ryear=@Syear and Rgrade=@Sgrade and Rterm=@Cterm  group by Rsnum )as sp ");
            mySqlf.Append(" where Snum=sp.Rsnum and Sgrade=@Sgrade ");

            DbHelperSQL.ExecuteSql(mySqlf.ToString(), parameters);//统计班级当前学期表单得分

        }
        /// <summary>
        /// 批量更新该班级当前学期作品总积分和表现总积分（批量group方法）
        /// </summary>
        public void ThisClassTeamScoresNew(int Sgrade, int Sclass)
        {
            int Cterm = Int32.Parse(LearnSite.Common.XmlHelp.GetTerm());
            int Syear = GetYear(Sgrade,Sclass);
            string mysqla = "update Students set Sscore=0,Sattitude=0 where Sgrade=" + Sgrade + " and Sclass=" + Sclass;
            DbHelperSQL.ExecuteSql(mysqla);//统计前将成绩清空
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Students set Sscore=nc.ws  from( SELECT Wnum, ISNULL(SUM(Wscore)+SUM(Wdscore),0)as ws FROM Works ");
            strSql.Append(" where Wyear=@Syear and Wcid in ( ");
            strSql.Append("select Cid from Courses where Cterm=@Cterm and Cobj=@Sgrade");
            strSql.Append(" ) group by Wnum )as nc ");
            strSql.Append(" where Snum=nc.Wnum and Sgrade=@Sgrade and Sclass=@Sclass ");

            SqlParameter[] parameters = {
					new SqlParameter("@Sgrade", SqlDbType.Int,4),
                    new SqlParameter("@Sclass", SqlDbType.Int,4),
                    new SqlParameter("@Cterm", SqlDbType.Int,4),
                    new SqlParameter("@Syear", SqlDbType.Int,4)};

            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;
            parameters[2].Value = Cterm;
            parameters[3].Value = Syear;
            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);//统计班级当前学期作品总积分

            StringBuilder mySql = new StringBuilder();
            mySql.Append("update Students set Sattitude=qt.sqe  from( SELECT Qnum, ISNULL(SUM(Qattitude),0)as sqe FROM Signin ");
            mySql.Append(" where Qsyear=@Syear and Qgrade=@Sgrade and Qclass=@Sclass and Qterm=@Cterm  group by Qnum )as qt ");
            mySql.Append(" where Snum=qt.Qnum and Sgrade=@Sgrade and Sclass=@Sclass ");

            DbHelperSQL.ExecuteSql(mySql.ToString(), parameters);//

        }

        /// <summary>
        /// 统计单个学生主题讨论得分
        /// </summary>
        /// <param name="Snum"></param>
        /// <param name="Cterm"></param>
        /// <param name="Cobj"></param>
        public void TotalmySpscore(string Snum, int Cterm, int Cobj)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Students SET Spscore=");
            strSql.Append("(select ISNULL(sum(Rscore),0) from TopicReply WHERE Rnum=@Snum ");
            strSql.Append(" and Rgrade=@Cobj  and Rterm=@Cterm ) ");
            strSql.Append(" where Snum=@Snum ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Snum", SqlDbType.NVarChar,50),
                    new SqlParameter("@Cterm", SqlDbType.Int,4),
					new SqlParameter("@Cobj", SqlDbType.Int,4)};

            parameters[0].Value = Snum;
            parameters[1].Value = Cterm;
            parameters[2].Value = Cobj;
            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 统计单个学生的调查测验分
        /// </summary>
        /// <param name="Snum"></param>
        /// <param name="Cterm"></param>
        /// <param name="Cobj"></param>
        public void TotalmySvscore(string Snum, int Cterm, int Cobj)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Students SET Svscore=");
            strSql.Append("(select ISNULL(sum(Fscore),0) from SurveyFeedback WHERE Fvtype=1 and Fnum=@Snum ");
            strSql.Append(" and Fgrade=@Cobj  and Fterm=@Cterm ) ");
            strSql.Append(" where Snum=@Snum ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Snum", SqlDbType.NVarChar,50),
                    new SqlParameter("@Cterm", SqlDbType.Int,4),
					new SqlParameter("@Cobj", SqlDbType.Int,4)};

            parameters[0].Value = Snum;
            parameters[1].Value = Cterm;
            parameters[2].Value = Cobj;
            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 统计一个学生的表现分
        /// </summary>
        /// <param name="Snum"></param>
        /// <param name="Cterm"></param>
        /// <param name="Cobj"></param>
        public void TotalmySattitude(string Snum, int Cterm, int Cobj)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Students SET Sattitude=");
            strSql.Append("(SELECT ISNULL(SUM(Qattitude),0) FROM Signin WHERE Qnum=@Snum ");
            strSql.Append(" and Qgrade=@Cobj  and Qterm=@Cterm ) ");
            strSql.Append(" where Snum=@Snum ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Snum", SqlDbType.NVarChar,50),
                    new SqlParameter("@Cterm", SqlDbType.Int,4),
					new SqlParameter("@Cobj", SqlDbType.Int,4)};

            parameters[0].Value = Snum;
            parameters[1].Value = Cterm;
            parameters[2].Value = Cobj;
            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 统计一个学生的作品分
        /// </summary>
        /// <param name="Snum"></param>
        /// <param name="Cterm"></param>
        /// <param name="Cobj"></param>
        public void TotalmySscores(string Snum, int Cterm, int Cobj)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Students SET Sscore=");
            strSql.Append("(SELECT ISNULL(SUM(Wscore),0) FROM Works WHERE Wnum=@Snum and ");
            strSql.Append(" Wcid in (select Cid from Courses where Cterm=@Cterm and Cobj=@Cobj ))");
            strSql.Append(" FROM Students WHERE Snum=@Snum");
            SqlParameter[] parameters = {
                    new SqlParameter("@Snum", SqlDbType.NVarChar,50),
                    new SqlParameter("@Cterm", SqlDbType.Int,4),
					new SqlParameter("@Cobj", SqlDbType.Int,4)};

            parameters[0].Value = Snum;
            parameters[1].Value = Cterm;
            parameters[2].Value = Cobj;
            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 终结性评定
        /// </summary>
        /// <param name="perA"></param>
        /// <param name="perE"></param>
        public void TermAPE(int perA,int perE)
        {
            string strSql = "UPDATE Students SET Sape='P'";
            DbHelperSQL.ExecuteSql(strSql);

            LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
            int SgradeMin = rm.GetMinRgrade();
            int SgradeMax = rm.GetMaxRgrade();
            int SclassMin = rm.GetMinRclass();
            int SclassMax = rm.GetMaxRclass();
            for (int i = SgradeMin; i < SgradeMax+1; i++)
            {
                for (int j = SclassMin; j < SclassMax+1; j++)
                {
                    int Sgrade = i;
                    int Sclass = j;
                    int Scount=0;
                    string mysql = "SELECT MAX(Sallscore) From Students WHERE Sgrade=" + Sgrade + " AND Sclass=" + Sclass;
                    int Sallscore = DbHelperSQL.FindNum(mysql);

                    string strcount = "SELECT COUNT(*) From Students WHERE Sgrade=" + Sgrade + " AND Sclass=" + Sclass ;
                    object objcount= DbHelperSQL.GetSingle(strcount);
                    if (objcount != null)
                        Scount = Int32.Parse(objcount.ToString());
                    else
                    {
                        break;
                    }
                    int setA = Scount *perA/100;
                    //int setE = Scount * perE/100;
                    int EscoreLimit = Sallscore * perE / 100;
                    string strA = "UPDATE Students SET Sape='A' WHERE  Sid IN (SELECT TOP " + setA + " Sid FROM Students WHERE Sgrade='" + Sgrade + "' AND Sclass='" + Sclass + "' ORDER BY Sallscore DESC) ";
                    DbHelperSQL.ExecuteSql(strA);
                    //string strE = "UPDATE Students SET Sape='E' WHERE  Sid IN (SELECT TOP " + setE + " Sid FROM Students WHERE Sgrade='" + Sgrade + "' AND Sclass='" + Sclass + "' ORDER BY Sallscore ASC) ";
                    //DbHelperSQL.ExecuteSql(strE);
                    string strKill = "UPDATE Students SET Sape='E' WHERE Sallscore<" + EscoreLimit;//总分低于Ｅ的分值时就评为Ｅ
                    DbHelperSQL.ExecuteSql(strKill);
                }
            }
        }

        /// <summary>
        /// 终结性评定
        /// </summary>
        public void TermABCD()
        {
            // 直接按期末总分进行评定：
            // 优秀：>= 80分
            // 良好：60-80分
            // 及格：30-60分
            // 不及格：< 30分且> 0
            // 0分：= 0

            // 第1步：先清空所有学生的评定（包括空格）
            int clearCount = DbHelperSQL.ExecuteSql("UPDATE Students SET Stenscore=0, Sape='', Sallscore=ISNULL(Sallscore,0) WHERE Sallscore IS NULL");

            // 第2步：评定优秀（>=80分）- 不检查Sape，直接覆盖
            int countA = DbHelperSQL.ExecuteSql("UPDATE Students SET Sape='优秀', Stenscore=10 WHERE ISNULL(Sallscore,0) >= 80");

            // 第3步：评定良好（60-80分）- 只评定尚未评定的（处理空格）
            int countB = DbHelperSQL.ExecuteSql("UPDATE Students SET Sape='良好', Stenscore=8 WHERE ISNULL(Sallscore,0) >= 60 AND ISNULL(Sallscore,0) < 80 AND LTRIM(RTRIM(ISNULL(Sape,'')))=''");

            // 第4步：评定及格（30-60分）- 只评定尚未评定的（处理空格）
            int countC = DbHelperSQL.ExecuteSql("UPDATE Students SET Sape='及格', Stenscore=6 WHERE ISNULL(Sallscore,0) >= 30 AND ISNULL(Sallscore,0) < 60 AND LTRIM(RTRIM(ISNULL(Sape,'')))=''");

            // 第5步：评定不及格（<30分且>0）- 只评定尚未评定的（处理空格）
            int countD = DbHelperSQL.ExecuteSql("UPDATE Students SET Sape='不及格', Stenscore=4 WHERE ISNULL(Sallscore,0) > 0 AND ISNULL(Sallscore,0) < 30 AND LTRIM(RTRIM(ISNULL(Sape,'')))=''");

            // 第6步：评定0分 - 只评定尚未评定的（处理空格）
            int countE = DbHelperSQL.ExecuteSql("UPDATE Students SET Sape='不及格', Stenscore=2 WHERE ISNULL(Sallscore,0) = 0 AND LTRIM(RTRIM(ISNULL(Sape,'')))=''");

            System.Text.StringBuilder debugLog = new System.Text.StringBuilder();
            debugLog.AppendLine("TermABCD执行完成");
            debugLog.AppendLine("清空评定: " + clearCount + "条");
            debugLog.AppendLine("优秀(>=80分): " + countA + "人");
            debugLog.AppendLine("良好(60-80分): " + countB + "人");
            debugLog.AppendLine("及格(30-60分): " + countC + "人");
            debugLog.AppendLine("不及格(<30分): " + countD + "人");
            debugLog.AppendLine("0分: " + countE + "人");
            debugLog.AppendLine("总计: " + (countA + countB + countC + countD + countE) + "人");

            // 将调试日志写入文件
            try
            {
                string logFilePath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/term_abcd_debug.log");
                System.IO.File.WriteAllText(logFilePath, debugLog.ToString());
            }
            catch { }
        }


        /// <summary>
        /// 终结性评定
        /// </summary>
        public void TermABCDE()
        {
            string strSqlp = "UPDATE Students SET Stenscore='0'";
            DbHelperSQL.ExecuteSql(strSqlp);

            LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
            int SgradeMin = rm.GetMinRgrade();
            int SgradeMax = rm.GetMaxRgrade();
            int SclassMin = rm.GetMinRclass();
            int SclassMax = rm.GetMaxRclass();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE s SET Stenscore = s.Sallscore * 10 / max_scores.Smaxscore ");
            strSql.Append("FROM Students s ");
            strSql.Append("INNER JOIN (");
            strSql.Append("    SELECT Sgrade, Sclass, MAX(Sallscore) as Smaxscore ");
            strSql.Append("    FROM Students ");
            strSql.Append("    WHERE Sgrade >= @SgradeMin AND Sgrade <= @SgradeMax ");
            strSql.Append("      AND Sclass >= @SclassMin AND Sclass <= @SclassMax ");
            strSql.Append("    GROUP BY Sgrade, Sclass ");
            strSql.Append("    HAVING MAX(Sallscore) > 0");
            strSql.Append(") max_scores ON s.Sgrade = max_scores.Sgrade AND s.Sclass = max_scores.Sclass ");

            SqlParameter[] parameters = {
                new SqlParameter("@SgradeMin", SqlDbType.Int, 4),
                new SqlParameter("@SgradeMax", SqlDbType.Int, 4),
                new SqlParameter("@SclassMin", SqlDbType.Int, 4),
                new SqlParameter("@SclassMax", SqlDbType.Int, 4)};

            parameters[0].Value = SgradeMin;
            parameters[1].Value = SgradeMax;
            parameters[2].Value = SclassMin;
            parameters[3].Value = SclassMax;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);

            string sqla = "UPDATE Students SET Sape='A' WHERE Sgrade >= " + SgradeMin + " AND Sgrade <= " + SgradeMax + " AND Sclass >= " + SclassMin + " AND Sclass <= " + SclassMax + " AND Stenscore>8";
            DbHelperSQL.ExecuteSql(sqla);
            string sqlb = "UPDATE Students SET Sape='B' WHERE Sgrade >= " + SgradeMin + " AND Sgrade <= " + SgradeMax + " AND Sclass >= " + SclassMin + " AND Sclass <= " + SclassMax + " AND Stenscore>6 AND Stenscore<9 ";
            DbHelperSQL.ExecuteSql(sqlb);
            string sqlc = "UPDATE Students SET Sape='C' WHERE Sgrade >= " + SgradeMin + " AND Sgrade <= " + SgradeMax + " AND Sclass >= " + SclassMin + " AND Sclass <= " + SclassMax + " AND Stenscore>4 AND Stenscore<7 ";
            DbHelperSQL.ExecuteSql(sqlc);
            string sqld = "UPDATE Students SET Sape='D' WHERE Sgrade >= " + SgradeMin + " AND Sgrade <= " + SgradeMax + " AND Sclass >= " + SclassMin + " AND Sclass <= " + SclassMax + " AND Stenscore>2 AND Stenscore<5 ";
            DbHelperSQL.ExecuteSql(sqld);
            string sqle = "UPDATE Students SET Sape='E' WHERE Sgrade >= " + SgradeMin + " AND Sgrade <= " + SgradeMax + " AND Sclass >= " + SclassMin + " AND Sclass <= " + SclassMax + " AND Stenscore>0 AND Stenscore<3 ";
            DbHelperSQL.ExecuteSql(sqle);
        }

        /// <summary>
        /// 获得作品总评数据列表
        /// </summary>
        public DataSet GetListTerm(int Sgrade, int Sclass)
		{
            string currentTerm = LearnSite.Common.XmlHelp.GetTerm();
            StringBuilder strSql=new StringBuilder();
            strSql.Append("select Sid,Snum,(STR(Sgrade)+'.'+STR(Sclass)) as Sgradeclass,Sname,Sscore,Squiz,Sattitude,Swscore,Stscore,Sallscore,Sape,Spscore,Sgscore,Sfscore,Svscore,Stxtform,Schinese,Stenscore,Sidle,");
            strSql.Append("(SELECT COUNT(DISTINCT Qdate) FROM Signin WHERE Signin.Qnum = Students.Snum AND Signin.Qgrade = Students.Sgrade AND Signin.Qclass = Students.Sclass AND Signin.Qterm=" + currentTerm + ") as SignInCount ");
            strSql.Append(" FROM Students ");
            strSql.Append(" where Sgrade=@Sgrade and Sclass=@Sclass ");
            SqlParameter[] parameters = {
					new SqlParameter("@Sgrade", SqlDbType.Int,4),
                    new SqlParameter("@Sclass", SqlDbType.Int,4)};

            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;

            return DbHelperSQL.Query(strSql.ToString(),parameters);
		}
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Sid,Snum,Syear,Sgrade,Sclass,Sname,Spwd,Sex,Saddress,Sphone,Sparents,Sheadtheacher,Sscore,Squiz,Sattitude,Sape,Swscore,Stscore,Sallscore,Spscore,Sgroup,Sleader,Svote,Sgscore ");
            strSql.Append(" FROM Students ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得条件数据列表
        /// </summary>
        public DataSet GetSqlList(string strSql)
        {
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得单个学生数据
        /// </summary>
        public DataSet GetOneStudent(int Sid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Sid,Snum,Syear,Sgrade,Sclass,Sname,Spwd,Sex,Saddress,Sphone,Sparents,Sheadtheacher,Sscore,Squiz,Sattitude,Sape,Swscore,Stscore,Sallscore,Spscore,Sgroup,Sleader,Svote,Sgscore ");
            strSql.Append(" FROM Students ");
            strSql.Append(" where Sid=@Sid");
            SqlParameter[] parameters = {
					new SqlParameter("@Sid", SqlDbType.Int,4)};

            parameters[0].Value = Sid;

            return DbHelperSQL.Query(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 获得当前班级学生表现数据列表
        /// </summary>
        /// <param name="Syear"></param>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public DataTable GetListSattitude(int Syear, int Sgrade, int Sclass)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Sid,Snum,Sgrade,Sclass,Sname,Sattitude ");
            strSql.Append(" FROM Students ");
            strSql.Append(" where Syear=@Syear and Sgrade=@Sgrade and Sclass=@Sclass  order by Snum asc");
            SqlParameter[] parameters = {
					new SqlParameter("@Sgrade", SqlDbType.Int,4),
                    new SqlParameter("@Sclass", SqlDbType.Int,4),
                    new SqlParameter("@Syear", SqlDbType.Int,4)};

            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;
            parameters[2].Value = Syear;

            return DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
        }
        /// <summary>
        /// 获得学生管理页数据列表
        /// </summary>
	public DataSet GetListStudents(int Sgrade,int Sclass, string sortField)
	{
		StringBuilder strSql = new StringBuilder();
		strSql.Append("select Sid,Snum,Spwd,Sgrade,Sclass,Sname,Sex,Sphone,Sscore,Squiz,Sattitude,Sleader,Sgroup,Sfixedip,Sseat ");
		strSql.Append(" FROM Students ");
		strSql.Append(" where Sgrade=@Sgrade and Sclass=@Sclass");

		// 添加排序逻辑
		if (!string.IsNullOrEmpty(sortField))
		{
			if (sortField == "Qmachine")
                {
                    strSql.Append(" ORDER BY CASE WHEN ISNUMERIC(Snum) = 1 THEN CAST(Snum AS INT) ELSE 0 END ASC");
                }
                else if (sortField == "Snum")
                {
                    strSql.Append(" ORDER BY Snum ASC");
                }
            }
            
            SqlParameter[] parameters = {
					new SqlParameter("@Sgrade", SqlDbType.Int,4),
                    new SqlParameter("@Sclass", SqlDbType.Int,4)};

            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;

            return DbHelperSQL.Query(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 按年级和班级分页获取学生列表（默认按机号排序）
        /// </summary>
        public DataSet GetListStudents(int Sgrade,int Sclass)
        {
            return GetListStudents(Sgrade, Sclass, "Qmachine");
        }

        /// <summary>
        /// ��ñ���ѧ��ѧ�ź����������б�
        /// </summary>
        public DataSet GetStudentsSnumSname(int Sgrade, int Sclass)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Snum,Sname");
            strSql.Append(" FROM Students ");
            strSql.Append(" where Sgrade=@Sgrade and Sclass=@Sclass");
            SqlParameter[] parameters = {
					new SqlParameter("@Sgrade", SqlDbType.Int,4),
                    new SqlParameter("@Sclass", SqlDbType.Int,4)};

            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;

            return DbHelperSQL.Query(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 获得本班学生Sid学号和姓名数据列表
        /// </summary>
        public DataSet GetStudentsSnumSidSname(int Sgrade, int Sclass)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Sid,Snum,Sname");
            strSql.Append(" FROM Students ");
            strSql.Append(" where Sgrade=@Sgrade and Sclass=@Sclass");
            SqlParameter[] parameters = {
					new SqlParameter("@Sgrade", SqlDbType.Int,4),
                    new SqlParameter("@Sclass", SqlDbType.Int,4)};

            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;

            return DbHelperSQL.Query(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 获得本年级所有学生列表
        /// </summary>
        public DataTable GetStudentsSnumSname(int Sgrade)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Snum,Sclass,Sname");
            strSql.Append(" FROM Students ");
            strSql.Append(" where Sgrade=@Sgrade order by Sclass asc,Snum asc");
            SqlParameter[] parameters = {
					new SqlParameter("@Sgrade", SqlDbType.Int,4)};
            parameters[0].Value = Sgrade;

            return DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
        }
        /// <summary>
        /// 获得全体学生的学年列表
        /// </summary>
        public DataSet GetAllYears()
        {
            string strSql = "select distinct Syear FROM Students order by Syear asc ";
            return DbHelperSQL.Query(strSql);
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
            strSql.Append(" Sid,Snum,Syear,Sgrade,Sclass,Sname,Spwd,Sex,Saddress,Sphone,Sparents,Sheadtheacher,Sscore,Squiz,Sattitude,Sape,Swscore,Stscore,Sallscore,Spscore,Sgroup,Sleader,Svote,Sgscore ");
            strSql.Append(" FROM Students ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            // 只有当filedOrder不为空时才添加ORDER BY子句
            if (!string.IsNullOrEmpty(filedOrder))
            {
                strSql.Append(" order by " + filedOrder);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 从学生表得到该年份的记录数
        /// </summary>
        /// <param name="Syear"></param>
        /// <returns></returns>
        public int FindCount(int Syear)
        {
            int fcount = 0;
            string strSql = "select count(*) from Students where Syear="+Syear;
            string findstr = DbHelperSQL.FindString(strSql);
            if (findstr != "")
            {
                fcount = Int32.Parse(findstr);
            }
            return fcount;
        }
        /// <summary>
        /// 学生表所有学生年级都升一级，并删除学生表中超过班级表最高年级+2的学生
        /// </summary>
        public void Upgrade()
        {
            string strSql = "update Students set Sgrade=Sgrade+1";
            DbHelperSQL.ExecuteSql(strSql);
            System.Threading.Thread.Sleep(1000);
            BLL.Room rbll = new BLL.Room();
            int maxGrade = rbll.GetMaxRgrade()+2;
            string mysql = "delete Students where Sgrade>" + maxGrade;
            DbHelperSQL.ExecuteSql(mysql);            
        }


        /// <summary>
        /// 获得该年级的入学年份
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public string GetYear(int Sgrade)
        {
            string mysql = "select top 1  Syear from Students where Sgrade=" + Sgrade ;
            string getstr = DbHelperSQL.FindString(mysql);
            if (getstr == "")
                getstr = DateTime.Now.Year.ToString();
            return getstr;
        }
        /// <summary>
        /// 获得该年级的入学年份
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public int GetYear(int Sgrade,int Sclass)
        {
            string mysql = "select top 1 Syear from Students where Sgrade=" + Sgrade+" and Sclass="+Sclass;
            object aa = DbHelperSQL.GetSingle(mysql);
            if (aa != null)
            {
                return int.Parse(aa.ToString());
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 根据年级、班级获得姓名和学号
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public DataSet GetNameNum(int Sgrade, int Sclass)
        {
            string mysql = "select Snum,Sname from Students where Sgrade=" + Sgrade + " and Sclass=" + Sclass;
            return DbHelperSQL.GetDataSet(mysql);      
        }

        /// <summary>
        /// 显示本年级积分最高的20条记录
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="GridViewscore"></param>
        public  DataSet ShowTopScore(int Sgrade)
        {
            string mysql = "Select top 20 Snum,Sgrade,Sclass,Sname,(Sscore+Spscore+Stxtform+Sidle)as Sscore from Students where Sgrade=" + Sgrade + "  ORDER BY Sscore DESC";
            return DbHelperSQL.GetDataSet(mysql);
        }
        /// <summary>
        /// 显示本班级20条记录
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public DataSet ShowTopScore(int Sgrade, int Sclass)
        {
            string mysql = "Select top 20 Sname,(Sscore+Spscore+Stxtform+Sidle)as Sscore from Students where Sgrade=" + Sgrade + " and Sclass=" + Sclass + "  ORDER BY Sscore DESC";
            return DbHelperSQL.GetDataSet(mysql);
        }
        /// <summary>
        /// 显示本班级所有积分记录
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public DataSet ShowMyclassScore(int Sgrade, int Sclass)
        {
            string mysql = "Select Sname,(Sscore+Spscore+Stxtform+Sidle)as Sscore from Students where Sgrade=" + Sgrade + " and Sclass=" + Sclass + "  ORDER BY Sscore DESC";
            return DbHelperSQL.GetDataSet(mysql);        
        }
        /// <summary>
        /// 查询学生表的记录总数
        /// </summary>
        /// <returns></returns>
        public int GetCounts()
        {
           return DbHelperSQL.TableCounts("Students");
        }
        /// <summary>
        /// 更新该学号学生的测验成绩
        /// </summary>
        /// <param name="Snum"></param>
        /// <param name="Squiz"></param>
        public void SetSquiz(int Rsid, int Squiz)
        {
            string mysql = "update Students set Squiz=" + Squiz + "  where Sid=" + Rsid ;
            DbHelperSQL.ExecuteSql(mysql);
        }
        /// <summary>
        /// 获得本年级测验成绩最高的50条记录
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <returns></returns>
        public DataSet TopGradeQuiz(int Sgrade)
        {
            string mysql = "Select top 50 Snum,(STR(Sgrade)+STR(Sclass)) as Sgradeclass,Sname,Squiz from Students where Sgrade=" + Sgrade + " and Squiz>0  ORDER BY Squiz DESC";
            return DbHelperSQL.GetDataSet(mysql);        
        }
        /// <summary>
        /// 获得本班级测验成绩所有记录
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public DataSet TopClassQuiz(int Sgrade, int Sclass)
        {
            string mysql = "Select  Snum,(STR(Sgrade)+STR(Sclass)) as Sgradeclass,Sname,Squiz from Students where Sgrade=" + Sgrade + " and Sclass="+Sclass+" and Squiz>0  ORDER BY Squiz DESC";
            return DbHelperSQL.GetDataSet(mysql);
        }
        /// <summary>
        /// 获得我的测验平均成绩
        /// </summary>
        /// <param name="Snum"></param>
        /// <returns></returns>
        public string MySquiz(string Snum)
        {
            string mysql = "select Squiz from Students where Snum='"+Snum+"'";
            return DbHelperSQL.FindString(mysql);
        }
        /// <summary>
        /// 根据学号和班级密码，判断该学号是否存在
        /// </summary>
        /// <param name="Snum"></param>
        /// <param name="Rpwd"></param>
        /// <returns></returns>
        public bool ExistsLogin(string Snum,string Rpwd)
        {
            string mysql = "select count(1) from Students,Room where Sgrade=Rgrade and Sclass=Rclass and Snum='"+Snum+"' and Rpwd='"+Rpwd+"'";
            return DbHelperSQL.Exists(mysql);        
        }
        /// <summary>
        /// 根据学号和个人密码，判断该学号是否存在
        /// </summary>
        /// <param name="Snum"></param>
        /// <param name="Spwd"></param>
        /// <returns></returns>
        public bool ExistsLoginSelf(string Snum, string Spwd)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Students");
            strSql.Append(" where Snum=@Snum and Spwd=@Spwd");
            SqlParameter[] parameters = {
                    new SqlParameter("@Snum", SqlDbType.NVarChar,50),
					new SqlParameter("@Spwd", SqlDbType.NVarChar,50)};
            parameters[0].Value = Snum;
            parameters[1].Value = Spwd;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新学生表中网页制作成绩
        /// </summary>
        public void UpdateWebScore()
        {
            string mysql = "update Students set Students.Swscore= Webstudy.Wscore from Students,Webstudy where Snum=Wnum and Webstudy.Wscore is not null";
            DbHelperSQL.ExecuteSql(mysql);
        }
        /// <summary>
        /// 登录账号教师所教班级按设定百分比计算总分
        /// </summary>
        /// <param name="persscore">作品分组权重</param>
        /// <param name="persquiz">测验权重</param>
        /// <param name="perstscore">打字技能权重</param>
        /// <param name="perattitude">表现权重</param>
        /// <param name="persurvey">调查问卷权重</param>
        /// <param name="perssignin">签到权重</param>
        /// <param name="Rhid">教师ID</param>
        public void UpdateAllScore(int persscore, int persquiz, int perstscore, int perattitude, int persurvey, int perssignin, int Rhid)
        {
            // 获取该教师所教的所有班级
            string getClassesSql = "SELECT DISTINCT Sgrade, Sclass FROM Students, Room WHERE Sgrade=Rgrade AND Sclass=Rclass AND Rhid=" + Rhid;
            DataSet classDs = DbHelperSQL.Query(getClassesSql);

            if (classDs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow classRow in classDs.Tables[0].Rows)
                {
                    int grade = Convert.ToInt32(classRow["Sgrade"]);
                    int classNum = Convert.ToInt32(classRow["Sclass"]);

                    // 计算该班级的最大签到次数（该班级中签到次数最多的学生的签到次数）
                    // 使用 COUNT(DISTINCT Qdate) 来避免同一天重复签到的问题
                    // 同时获取该学生的学号，用于验证
                    string currentTerm = LearnSite.Common.XmlHelp.GetTerm();
                    string maxSigninSql = "SELECT TOP 1 COUNT(DISTINCT Qdate) as cnt, Qnum FROM Signin WHERE Qgrade=" + grade + " AND Qclass=" + classNum + " AND Qterm=" + currentTerm + " GROUP BY Qnum ORDER BY COUNT(DISTINCT Qdate) DESC";
                    DataSet maxDs = DbHelperSQL.Query(maxSigninSql);
                    int maxSignin = 1;
                    string maxSigninStudent = "";
                    if (maxDs.Tables[0].Rows.Count > 0)
                    {
                        maxSignin = Convert.ToInt32(maxDs.Tables[0].Rows[0]["cnt"]);
                        maxSigninStudent = maxDs.Tables[0].Rows[0]["Qnum"].ToString();
                    }
                    if (maxSignin == 0) maxSignin = 1; // 避免除零

                    // 将最大签到次数存储到静态变量中，供前端查询
                    maxSigninDict[grade + "-" + classNum] = maxSignin;
                    maxSigninStudentDict[grade + "-" + classNum] = maxSigninStudent;

                    // 更新总分,签到分数直接计算而不存储
                    // 单项分数超过100分的按100分计算，避免总分超过100分
                    // 先计算签到分，再更新总分
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("UPDATE Students SET Sallscore = ");
                    strSql.Append("ISNULL(CASE WHEN (Sscore+Sgscore+Spscore+Stxtform+Sidle)>100 THEN 100 ELSE (Sscore+Sgscore+Spscore+Stxtform+Sidle) END,0)*@persscore/100.0 + ");
                    strSql.Append("ISNULL(CASE WHEN Svscore>100 THEN 100 ELSE Svscore END,0)*@persurvey/100.0 + ");
                    strSql.Append("ISNULL(CASE WHEN Squiz>100 THEN 100 ELSE Squiz END,0)*@persquiz/100.0 + ");
                    strSql.Append("ISNULL(CASE WHEN Sattitude>100 THEN 100 ELSE Sattitude END,0)*@perattitude/100.0 + ");
                    strSql.Append("ISNULL(CASE WHEN (Stscore+Sfscore+Schinese)>100 THEN 100 ELSE (Stscore+Sfscore+Schinese) END,0)*@perstscore/100.0 + ");
                    strSql.Append("ISNULL((SELECT CAST(COUNT(DISTINCT Qdate) AS FLOAT)*100.0/" + maxSignin + " * @perssignin/100.0 ");
                    strSql.Append("FROM Signin WHERE Qnum=Students.Snum AND Qgrade=Students.Sgrade AND Qclass=Students.Sclass AND Qterm=" + currentTerm + "), 0) ");
                    strSql.Append("WHERE Sgrade=" + grade + " AND Sclass=" + classNum);

                    SqlParameter[] parameters = {
                        new SqlParameter("@persscore", SqlDbType.Int,4),
                        new SqlParameter("@persquiz", SqlDbType.Int,4),
                        new SqlParameter("@perstscore", SqlDbType.Int,4),
                        new SqlParameter("@perattitude", SqlDbType.Int,4),
                        new SqlParameter("@persurvey", SqlDbType.Int,4),
                        new SqlParameter("@perssignin", SqlDbType.Int,4)};

                    parameters[0].Value = persscore;
                    parameters[1].Value = persquiz;
                    parameters[2].Value = perstscore;
                    parameters[3].Value = perattitude;
                    parameters[4].Value = persurvey;
                    parameters[5].Value = perssignin;

                    string sqlDebug = strSql.ToString();
                    System.Diagnostics.Debug.WriteLine("UpdateAllScore SQL: " + sqlDebug);
                    int result = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                    System.Diagnostics.Debug.WriteLine("UpdateAllScore affected rows: " + result);
                }
            }
        }

        /// <summary>
        /// 登录账号教师所教班级按设定百分比计算总分（支持作品分字段过滤）
        /// </summary>
        /// <param name="persscore">作品分组权重</param>
        /// <param name="persquiz">测验权重</param>
        /// <param name="perstscore">打字技能权重</param>
        /// <param name="perattitude">表现权重</param>
        /// <param name="persurvey">调查问卷权重</param>
        /// <param name="perssignin">签到权重</param>
        /// <param name="Rhid">教师ID</param>
        /// <param name="useWork">是否使用作品分</param>
        /// <param name="useGroup">是否使用小组分</param>
        /// <param name="useDiscuss">是否使用讨论分</param>
        /// <param name="useForm">是否使用表单分</param>
        /// <param name="useIdle">是否使用测评分</param>
        /// <param name="useSurvey">是否使用调查问卷分</param>
        public void UpdateAllScoreWithFilter(int persscore, int persquiz, int perstscore, int perattitude, int persurvey, int perssignin, int Rhid,
                                           bool useWork, bool useGroup, bool useDiscuss, bool useForm, bool useIdle, bool useSurvey)
        {
            // 获取该教师所教的所有班级
            string getClassesSql = "SELECT DISTINCT Sgrade, Sclass FROM Students, Room WHERE Sgrade=Rgrade AND Sclass=Rclass AND Rhid=" + Rhid;
            DataSet classDs = DbHelperSQL.Query(getClassesSql);

            if (classDs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow classRow in classDs.Tables[0].Rows)
                {
                    int grade = Convert.ToInt32(classRow["Sgrade"]);
                    int classNum = Convert.ToInt32(classRow["Sclass"]);

                    // 计算该班级的最大签到次数
                    string currentTerm = LearnSite.Common.XmlHelp.GetTerm();
                    string maxSigninSql = "SELECT TOP 1 COUNT(DISTINCT Qdate) as cnt FROM Signin WHERE Qgrade=" + grade + " AND Qclass=" + classNum + " AND Qterm=" + currentTerm + " GROUP BY Qnum ORDER BY COUNT(DISTINCT Qdate) DESC";
                    DataSet maxDs = DbHelperSQL.Query(maxSigninSql);
                    int maxSignin = 1;
                    if (maxDs.Tables[0].Rows.Count > 0)
                    {
                        maxSignin = Convert.ToInt32(maxDs.Tables[0].Rows[0]["cnt"]);
                    }
                    if (maxSignin == 0) maxSignin = 1;

                    // 将最大签到次数存储到静态变量中
                    maxSigninDict[grade + "-" + classNum] = maxSignin;
                    maxSigninStudentDict[grade + "-" + classNum] = "";

                    // 构建作品分的动态计算公式
                    StringBuilder workScoreFormula = new StringBuilder();
                    if (useWork) workScoreFormula.Append("+ISNULL(Sscore,0)");
                    if (useGroup) workScoreFormula.Append("+ISNULL(Sgscore,0)");
                    if (useDiscuss) workScoreFormula.Append("+ISNULL(Spscore,0)");
                    if (useForm) workScoreFormula.Append("+ISNULL(Stxtform,0)");
                    if (useIdle) workScoreFormula.Append("+ISNULL(Sidle,0)");
                    if (useSurvey) workScoreFormula.Append("+ISNULL(Svscore,0)");

                    string workScoreExpr = workScoreFormula.Length > 0 ? workScoreFormula.ToString().Substring(1) : "0";

                    // 更新总分
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("UPDATE Students SET Sallscore = ");
                    strSql.Append("CASE WHEN (" + workScoreExpr + ")>100 THEN 100 ELSE (" + workScoreExpr + ") END*@persscore/100.0 + ");
                    // 调查问卷已合并到作品分计算中，不再单独计算
                    strSql.Append("ISNULL(CASE WHEN Squiz>100 THEN 100 ELSE Squiz END,0)*@persquiz/100.0 + ");
                    strSql.Append("ISNULL(CASE WHEN Sattitude>100 THEN 100 ELSE Sattitude END,0)*@perattitude/100.0 + ");
                    strSql.Append("ISNULL(CASE WHEN (Stscore+Sfscore+Schinese)>100 THEN 100 ELSE (Stscore+Sfscore+Schinese) END,0)*@perstscore/100.0 + ");
                    strSql.Append("ISNULL((SELECT CAST(COUNT(DISTINCT Qdate) AS FLOAT)*100.0/" + maxSignin + " * @perssignin/100.0 ");
                    strSql.Append("FROM Signin WHERE Qnum=Students.Snum AND Qgrade=Students.Sgrade AND Qclass=Students.Sclass AND Qterm=" + currentTerm + "), 0) ");
                    strSql.Append("WHERE Sgrade=" + grade + " AND Sclass=" + classNum);

                    SqlParameter[] parameters = {
                        new SqlParameter("@persscore", SqlDbType.Int,4),
                        new SqlParameter("@persquiz", SqlDbType.Int,4),
                        new SqlParameter("@perstscore", SqlDbType.Int,4),
                        new SqlParameter("@perattitude", SqlDbType.Int,4),
                        new SqlParameter("@persurvey", SqlDbType.Int,4),
                        new SqlParameter("@perssignin", SqlDbType.Int,4)};

                    parameters[0].Value = persscore;
                    parameters[1].Value = persquiz;
                    parameters[2].Value = perstscore;
                    parameters[3].Value = perattitude;
                    parameters[4].Value = persurvey;
                    parameters[5].Value = perssignin;

                    string sqlDebug = strSql.ToString();
                    System.Diagnostics.Debug.WriteLine("UpdateAllScoreWithFilter SQL: " + sqlDebug);
                    int result = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
                    System.Diagnostics.Debug.WriteLine("UpdateAllScoreWithFilter affected rows: " + result);
                }
            }
        }
        /// <summary>
        /// 更新学生表的打字成绩
        /// </summary>
        public void UpdateStscore()
        {
            string nowterm = LearnSite.Common.XmlHelp.GetTerm();
            string mysql = "update Students set Stscore=Pdegree from Students,Ptyper where Snum=Psnum and Sgrade=Pgrade and Pterm=" + nowterm;
            DbHelperSQL.ExecuteSql(mysql);
        }

        /// <summary>
        /// 根据学号，更新班级
        /// </summary>
        /// <param name="Sclass"></param>
        /// <param name="Snum"></param>
        public void UpdateStuclass(int Sclass, string Snum)
        {
            string sqlstr = "update Students set Sclass="+Sclass+" where Snum='"+Snum+"'";
            DbHelperSQL.ExecuteSql(sqlstr);
        }
        /// <summary>
        /// 根据教师自动编号Hid，返回所教班级的Syear,Sclass数据集
        /// </summary>
        /// <param name="Rhid"></param>
        /// <returns></returns>
        public DataSet TeacherSyearSclass(int hid)
        {
            string mysql = "select distinct Syear,Sgrade,Sclass from Students where Sclass in (select Rclass from Room where Rhid="+hid+")";
            return DbHelperSQL.Query(mysql);
        }
        /// <summary>
        /// 将所教班级的学生密码如果为原初始化密码则更新转换为姓名拼音缩写（如果转换的不是字母或数字，则不更新密码）
        /// </summary>
        /// <param name="hid"></param>
        /// <param name="Spwd"></param>
        public string SpwdToSpell(int hid,string Spwd)
        {
            string str = "";
            string mysql = "select Snum,Sname from Students,Room where Sgrade=Rgrade and Sclass=Rclass and Rhid="+hid+" and Spwd='"+Spwd+"'";
            DataSet ds = DbHelperSQL.Query(mysql);
            int counts = ds.Tables[0].Rows.Count;
            int right = 0;
            if (counts > 0)
            {
                for (int i = 0; i < counts; i++)
                {
                    string mySnum = ds.Tables[0].Rows[i]["Snum"].ToString();
                    string mySname = ds.Tables[0].Rows[i]["Sname"].ToString();
                    string spellname = Common.Gbk2Spell.Chinese.FirstLetter(mySname.Replace(" ", ""));//取姓名的拼音缩写为密码
                    if (LearnSite.Common.WordProcess.IsEnNum(spellname))
                    {
                        BLL.Students sbll = new BLL.Students();
                        sbll.UpdatePwd(mySnum, spellname);//如果缩写为字母或数字则更新
                        right++;
                    }
                }
            }
            str = "符合原初始化密码的所教学生总数为："+counts.ToString()+"位 转换成功："+right.ToString();
            return str;
        }
        /// <summary>
        /// 将所有学生的测验统计成绩更新为其测验最高分
        /// </summary>
        public void UpdateBestSquiz()
        {
            string mysql = "select Sid,Sgrade from Students ";
            DataSet ds = DbHelperSQL.Query(mysql);
            int counts = ds.Tables[0].Rows.Count;
            if (counts > 0)
            {
                BLL.Result rbll = new BLL.Result();
                string Rterm = LearnSite.Common.XmlHelp.GetTerm();

                for (int i = 0; i < counts; i++)
                {
                    int  mySid =Int32.Parse( ds.Tables[0].Rows[i]["Sid"].ToString());
                    string mySgrade = ds.Tables[0].Rows[i]["Sgrade"].ToString();
                    if (!string.IsNullOrEmpty(mySgrade))
                    {
                        int rscorebest = rbll.GetMax(mySid, mySgrade, Rterm);
                        SetSquiz(mySid, rscorebest);
                    }
                }
            }
        }
        /// <summary>
        /// 初始化Sleader值，数据库升级时用
        /// </summary>
        public void InitSleader()
        {
            string mysql = "update Students set Sleader=0 where Sleader is null";
            DbHelperSQL.ExecuteSql(mysql);
        }
        /// <summary>
        /// 自动分配小组
        /// </summary>
        public void AutoSleader(int Sgrade, int Sclass, int Leadnum, int groupmax)
        {
            if (Leadnum > 1)
            {
                string mysql = "update Students set Sleader=0,Sgroup=null,Sgtitle=null where Sgrade=" + Sgrade + " and Sclass=" + Sclass;
                DbHelperSQL.ExecuteSql(mysql);

                string mysqler = "update Students set  Sgtitle=Sname, Sleader=1,Sgroup=Sid where Sid in ( select top " + Leadnum + " Sid from Students where  Sgrade=" + Sgrade + " and Sclass=" + Sclass + " order by Sscore desc)";
                DbHelperSQL.ExecuteSql(mysqler);

                string sqlstr = "select Sid from Students where Sleader=0 and  Sgrade=" + Sgrade + " and Sclass=" + Sclass + " order by Sscore desc";
                DataTable dt = DbHelperSQL.Query(sqlstr).Tables[0];

                string sqllead = "select Sid from Students where Sleader=1 and  Sgrade=" + Sgrade + " and Sclass=" + Sclass + " order by Sscore asc";
                DataTable dtlead = DbHelperSQL.Query(sqllead).Tables[0];
                int count = dt.Rows.Count;//9

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < count; i++)
                {
                    string sid = dt.Rows[i]["Sid"].ToString();
                    string leadsid = dtlead.Rows[i % Leadnum]["Sid"].ToString();//依次分配学生
                    sb.Append("update Students set Sgroup=").Append(leadsid).Append(" where   Sid=").Append(sid).Append(";");
                }
                if (sb.Length > 0)
                {
                    DbHelperSQL.ExecuteSql(sb.ToString());
                }
            }
        }

        /// <summary>
        /// 任命或卸任组长
        /// </summary>
        /// <param name="Snum"></param>
        public void ChangeSleader(int Sid)
        {
            string mysql = "update Students set Sleader=Sleader^1 where Sid=" + Sid;
            DbHelperSQL.ExecuteSql(mysql);

            string wrdsql = "update Students set Sgroup=null where Sgroup=" + Sid; ;//将本组的成员全退组（无论任命或卸任）
            DbHelperSQL.ExecuteSql(wrdsql);

            string strsql = "update Students set Sgroup=Sid where Sleader=1 and Sid=" + Sid;//更新组号为自动编号
            DbHelperSQL.ExecuteSql(strsql);

            string gtitlesql = "update Students set Sgtitle=Sname where Sleader=1 and Sid=" + Sid;//小组名称默认无
            DbHelperSQL.ExecuteSql(gtitlesql);
        }
        /// <summary>
        /// 获取班级所有小组队长信息
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public DataSet ClassGroup(int Sgrade, int Sclass)
        {
            string mysql = "select Sid,Snum,Sname,Sgroup,Sgtitle from Students where Sleader=1 and Sgrade="+Sgrade+" and Sclass="+Sclass+" order by Snum asc";
            return DbHelperSQL.Query(mysql);        
        }

        /// <summary>
        /// 获取本小组成员名单
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Sgroup"></param>
        /// <returns></returns>
        public string GroupMember(int Sgrade, int Sclass, int Sgroup)
        {
            string mysql = "select Sname from Students where Sleader=0 and Sgroup="+Sgroup+" and Sgrade=" + Sgrade + " and Sclass=" + Sclass + " order by Snum asc";
            DataSet ds = DbHelperSQL.Query(mysql);
            string str = "";
            int counts = ds.Tables[0].Rows.Count;
            if (counts > 0)
            {
                for (int i = 0; i < counts; i++)
                {
                    string Sname = ds.Tables[0].Rows[i]["Sname"].ToString();
                    str = str + Sname;
                    if (i < counts - 1)
                    {
                        str = str + "、";
                    }
                }
            }
            return str;
        }


        /// <summary>
        /// 获取本班未参加小组名单
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public string FreeMember(int Sgrade, int Sclass)
        {
            string mysql = "select Sname from Students where Sgroup=0 and Sgrade=" + Sgrade + " and Sclass=" + Sclass + " order by Snum asc";
            DataSet ds = DbHelperSQL.Query(mysql);
            string str = "";
            int counts = ds.Tables[0].Rows.Count;
            if (counts > 0)
            {
                for (int i = 0; i < counts; i++)
                {
                    string Sname = ds.Tables[0].Rows[i]["Sname"].ToString();
                    str = str + Sname;
                    if (i < counts - 1)
                    {
                        str = str + "、";
                    }
                }
            }
            return str;
        }

        /// <summary>
        /// 根据学号获取同组成员的所有学号
        /// </summary>
        /// <param name="Snum"></param>
        /// <returns></returns>
        public string GroupSnum(string Snum)
        {
            string mysql = "select Snum from Students where Sgroup=(select top 1 Sgroup from Students where Snum='"+Snum+"')";
            DataSet ds = DbHelperSQL.Query(mysql);
            string str = "";
            int counts = ds.Tables[0].Rows.Count;
            if (counts > 0)
            {
                for (int i = 0; i < counts; i++)
                {
                    string Sname = ds.Tables[0].Rows[i]["Snum"].ToString();
                    str = str + Sname;
                    if (i < counts - 1)
                    {
                        str = str + "_";
                    }
                }
            }
            return str;
        }
        /// <summary>
        /// 更新该学号的小组号
        /// 如果原组长已卸任则加入新小组
        /// </summary>
        /// <param name="Snum"></param>
        /// <param name="Sgroup"></param>
        public int AddThisGroup(string Snum, int Sgroup)
        {
            if (!FindLeader(Snum))//如果原组长已卸任则加入新小组
            {
                string mysql = "update Students set Sgroup=" + Sgroup + " where   Snum='" + Snum + "'";
                return DbHelperSQL.ExecuteSql(mysql);
            }
            return 0;
        }
        /// 退组
        /// </summary>
        /// <param name="Snum"></param>
        /// <param name="Sgroup"></param>
        public int OutThisGroup(string Snum)
        {
            string mysql = "update Students set Sgroup= 0 where Sleader=0 and  Snum='" + Snum + "'";
            return DbHelperSQL.ExecuteSql(mysql);
        }
        public void AddThisGroup(int Sid, int Sgroup)
        {
            string mysql = "update Students set Sgroup=" + Sgroup + " where   Sid=" + Sid;
            DbHelperSQL.ExecuteSql(mysql);
        }
        /// <summary>
        /// 获取本班本小组人数
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Sgroup"></param>
        /// <returns></returns>
        public int GetGroupCount(int Sgrade, int Sclass, int Sgroup)
        {
            string mysql = "select count(*) from Students where Sgrade="+Sgrade+" and Sclass="+Sclass+" and Sgroup="+Sgroup;
            string str = DbHelperSQL.FindString(mysql);
            if (str == "")
                return 0;
            else
                return Int32.Parse(str);
        }
        /// <summary>
        /// 将该学号非组长同学退组
        /// </summary>
        /// <param name="Snum"></param>
        public void QuitThitGroup(string Snum)
        {
            string mysql = "update Students set Sgroup=null where Sleader=0 and Snum='"+Snum+"'";
            DbHelperSQL.ExecuteSql(mysql);
        }
        /// <summary>
        /// 将该学号非组长同学退组
        /// </summary>
        /// <param name="Sid"></param>
        public void QuitThitGroup(int Sid)
        {
            string mysql = "update Students set Sgroup=null where Sleader=0 and Sid=" + Sid;
            DbHelperSQL.ExecuteSql(mysql);
        }
        /// <summary>
        /// 寻索该组号的组长是否存在
        /// </summary>
        /// <param name="Sid"></param>
        /// <returns></returns>
        private bool FindLeader(string Snum)
        {
            string mysql = "select count(1) from Students where Sleader=1 and Sid=( select top 1 Sgroup from Students where Snum='"+Snum+"')";
            return DbHelperSQL.Exists(mysql);
        }
        /// <summary>
        /// 是否组长
        /// </summary>
        /// <param name="Snum"></param>
        /// <returns></returns>
        public bool IsLeader(string Snum)
        {
            string mysql = "select count(1) from Students where Sleader=1 and Snum='" + Snum + "'";
            return DbHelperSQL.Exists(mysql);
        }
        /// <summary>
        /// 是否组长
        /// </summary>
        /// <param name="Sid"></param>
        /// <returns></returns>
        public bool IsLeaderSid(int Sid)
        {
            string mysql = "select count(1) from Students where Sleader=1 and Sid=" + Sid;
            return DbHelperSQL.Exists(mysql);
        }
        /// <summary>
        /// 获取小组名称
        /// </summary>
        /// <param name="Sid"></param>
        /// <returns></returns>
        public string GetSgtitle(int Sgroup)
        {
            string mysql = "select Sgtitle from Students where Sleader=1 and Sid=" + Sgroup;
            return DbHelperSQL.FindString(mysql);
        }
        /// <summary>
        /// 更新小组名称
        /// </summary>
        /// <param name="Sid"></param>
        /// <param name="Sgtitle"></param>
        /// <returns></returns>
        public int UpdateSgtitle(int Sgroup, string Sgtitle)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE Students SET Sgtitle=@Sgtitle");
            strSql.Append(" where Sgroup=@Sgroup ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Sgroup", SqlDbType.Int,4),
                    new SqlParameter("@Sgtitle", SqlDbType.NVarChar,50)};

            parameters[0].Value = Sgroup;
            parameters[1].Value = Sgtitle;

            return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 获取该学号年级
        /// </summary>
        /// <param name="Snum"></param>
        /// <returns></returns>
        public int GetSgrade(string Snum)
        {
            string mysql = "select top 1 Sgrade from Students where  Snum='" + Snum + "'";
            string restr = DbHelperSQL.FindString(mysql);
            if (restr != "")
            {
                return Int32.Parse(restr);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 根据年级和班级获取学生列表
        /// </summary>
        /// <param name="Sgrade">年级</param>
        /// <param name="Sclass">班级</param>
        /// <returns>学生列表DataTable</returns>
        public DataTable GetListByGradeClass(int Sgrade, int Sclass)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Sid, Snum, Sname, Sseat ");
            strSql.Append("FROM Students ");
            strSql.Append("where Sgrade=@Sgrade and Sclass=@Sclass ");
            strSql.Append("order by Snum asc");
            SqlParameter[] parameters = {
                    new SqlParameter("@Sgrade", SqlDbType.Int,4),
                    new SqlParameter("@Sclass", SqlDbType.Int,4)};

            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;

            return DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
        }
        /// <summary>
        /// 根据学号，修改姓名
        /// </summary>
        /// <param name="Snum"></param>
        /// <param name="Sname"></param>
        /// <returns></returns>
        public int ChangeSname(string Snum, string Sname)
        {
            string mysql = "update Students set Sname='"+Sname+"' where Snum='"+Snum+"'";
            return DbHelperSQL.ExecuteSql(mysql);
        }

        public int InitSfscore()
        {
            string mysql = "update Students set Sfscore=0 where  Sfscore is null";
            return DbHelperSQL.ExecuteSql(mysql);
        }

        public int InitSvscore()
        {
            string mysql = "update Students set Svscore=0 where  Svscore is null";
            return DbHelperSQL.ExecuteSql(mysql);
        }

        public int InitSidle()
        {
            string mysql = "update Students set Sidle=0 where  Sidle is null";
            return DbHelperSQL.ExecuteSql(mysql);
        }

        /// <summary>
        /// 更新指法成绩
        /// </summary>
        /// <returns></returns>
        public int UpdateSfscore()
        {
            string nowterm = LearnSite.Common.XmlHelp.GetTerm();
            string mysql = "update Students set Sfscore=Pspd from Students,Pfinger where Snum=Psnum and Sgrade=Pgrade and Pterm="+nowterm;
            return DbHelperSQL.ExecuteSql(mysql);
        }
        /// <summary>
        /// 更新中文拼音成绩
        /// </summary>
        /// <returns></returns>
        public int UpdateSchinese()
        {
            string nowterm = LearnSite.Common.XmlHelp.GetTerm();
            string mysql = "update Students set Schinese=Ptotal/200  from Students,Pchinese where Snum=Psnum and Sgrade=Pgrade and Pterm=" + nowterm;
            return DbHelperSQL.ExecuteSql(mysql);
        }

        /// <summary>
        /// 统计前，先清空学生成绩
        /// </summary>
        public void ClearAllScores(int hid)
        {
            string strsql = "update Students set Sscore=0,Squiz=0,Sattitude=0,Sape='',Stenscore=0,Swscore=0,Stscore=0,Sallscore=0,Spscore=0,Sgscore=0,Sfscore=0,Svscore=0,Stxtform=0 from Students,Room where Sgrade=Rgrade and Sclass=Rclass  and Rhid=" + hid.ToString();
            DbHelperSQL.ExecuteSql(strsql);//先清空
        }

        public string GetLeader(int Sid)
        {
            string mysql = "select Sname from Students where Sid=(select top 1 Sgroup from Students where Sid=" + Sid + ")";
            return DbHelperSQL.FindString(mysql);
        }

        /// <summary>
        /// 返回小组名称
        /// </summary>
        /// <param name="Sid"></param>
        /// <returns></returns>
        public string GetMySgtitle(int Sid)
        {
            string mysql = "select Sgtitle from Students where Sid=(select top 1 Sgroup from Students where Sid=" + Sid + ")";
            return DbHelperSQL.FindString(mysql);
        }

        public string GetLeaderByGroup(int Sgroup)
        {
            string mysql = "select Sname from Students where Sid=" + Sgroup;
            return DbHelperSQL.FindString(mysql);
        }
        /// <summary>
        /// 解除分组
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public int NoGroup(int Sgrade, int Sclass)
        {
            string mysql = "update Students set Sgroup=null,Sleader=0,Steam=null where  Sgrade=" + Sgrade + " and Sclass=" + Sclass;
            return DbHelperSQL.ExecuteSql(mysql);
        }

        /// <summary>
        /// 获取当前班级学号集合用,分隔
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public string ShowClassSnums(int Sgrade, int Sclass)
        {
            string mysql = "SELECT Snum FROM Students where Sgrade=" + Sgrade + " and Sclass=" + Sclass ;
            DataTable dt = DbHelperSQL.Query(mysql).Tables[0];
            int n = dt.Rows.Count;
            if (n > 0)
            {
                string strtemp = "";
                for (int i = 0; i < n; i++)
                {
                    strtemp = strtemp +"'"+ dt.Rows[i]["Snum"].ToString() + "',";
                }
                if (strtemp.EndsWith(","))
                    strtemp = strtemp.Substring(0, strtemp.Length - 1);
                return strtemp;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取当前班级学生编号集合用,分隔
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public string ShowClassSids(int Sgrade, int Sclass)
        {
            string mysql = "SELECT Sid FROM Students where Sgrade=" + Sgrade + " and Sclass=" + Sclass;
            DataTable dt = DbHelperSQL.Query(mysql).Tables[0];
            int n = dt.Rows.Count;
            if (n > 0)
            {
                string strtemp = "";
                for (int i = 0; i < n; i++)
                {
                    strtemp = strtemp + dt.Rows[i]["Sid"].ToString()+",";
                }
                if (strtemp.EndsWith(","))
                    strtemp = strtemp.Substring(0, strtemp.Length - 1);
                return strtemp;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 获取当前年级学生编号集合用,分隔
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <returns></returns>
        public string ShowGradeSids(int Sgrade)
        {
            string mysql = "SELECT Sid FROM Students where Sgrade=" + Sgrade;
            DataTable dt = DbHelperSQL.Query(mysql).Tables[0];
            int n = dt.Rows.Count;
            if (n > 0)
            {
                string strtemp = "";
                for (int i = 0; i < n; i++)
                {
                    strtemp = strtemp + dt.Rows[i]["Sid"].ToString() + ",";
                }
                if (strtemp.EndsWith(","))
                    strtemp = strtemp.Substring(0, strtemp.Length - 1);
                return strtemp;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 获取该班级的人数
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public int CountClassMate(int Sgrade, int Sclass)
        {
            string mysql = "select count(*) from Students where Sgrade=" + Sgrade + " and Sclass=" + Sclass;
            object ob = DbHelperSQL.GetSingle(mysql);
            if (ob != null)
                return Convert.ToInt32(ob);
            else
                return 0;//如果没有，则返回零
        }
        /// <summary>
        /// 清除该班级的所有学生记录
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public int DeleteClassMate(int Sgrade, int Sclass)
        {
            string mysql = "delete Students where Sgrade=" + Sgrade + " and Sclass=" + Sclass;
            return DbHelperSQL.ExecuteSql(mysql);
        }
        /// <summary>
        /// 修正因某些原因引起的组长的组号为0的情况
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        private void FixGroupSid(int Sgrade, int Sclass)
        {
            string mysql = "update Students set Sgroup=Sid where Sleader=1 and Sgroup=0 and  Sgrade=" + Sgrade + " and Sclass=" + Sclass;
            DbHelperSQL.ExecuteSql(mysql);
        }
        /// <summary>
        /// 汇总表小组统计
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="dttotal"></param>
        /// <returns></returns>
        public DataTable groupscores(int Sgrade, int Sclass, DataTable dttotal, int Gcid)
        {
            FixGroupSid(Sgrade, Sclass);
            string mysql = "select Sid,Snum,Sname,Sgtitle from Students where Sleader=1 and  Sgrade=" + Sgrade + " and Sclass=" + Sclass;
            DataTable dt = DbHelperSQL.Query(mysql).Tables[0];
            int n = dt.Rows.Count;
            dt.Columns.Add("Sgscore", typeof(int));//4
            dt.Columns.Add("Svscore", typeof(float));//5
            dt.Columns.Add("Sgwork", typeof(int));//6小组作品分
            dt.Columns.Add("Sgattitude", typeof(int));//7小组表现分
            //LearnSite.Common.Log.AddLogTable(dt);//调试信息
            if (dttotal.Columns.Count > 0 && n > 0)
            {
                string sqlstr = "select Snum,Sgroup from Students where Sgroup>0 and  Sgrade=" + Sgrade + " and Sclass=" + Sclass;
                DataTable dmp = DbHelperSQL.Query(sqlstr).Tables[0];
                int p = dmp.Rows.Count;
                if (p > 0)
                {
                    dttotal.Columns["汇总"].ColumnName = "Stotals";
                    dttotal.Columns.Add("Sgroup", typeof(int));
                    GetSgroups(dttotal, dmp); //将汇总表新列赋值
                    LearnSite.BLL.GroupWork gbll = new BLL.GroupWork();
                    LearnSite.BLL.Signin sgbll = new BLL.Signin();
                    for (int i = 0; i < n; i++)
                    {
                        string sid = dt.Rows[i][0].ToString();
                        string snum = dt.Rows[i][1].ToString();
                        string fliter = "Sgroup=" + sid;
                        dt.Rows[i][4] = Convert.ToInt32(dttotal.Compute("sum(Stotals)", fliter));
                        double num = Convert.ToDouble(dttotal.Compute("avg(Stotals)", fliter));
                        dt.Rows[i][5] = Convert.ToDouble(num.ToString("0.0"));
                        int sgroupnum = Int32.Parse(sid);
                        dt.Rows[i][6] = gbll.GetGscore(sgroupnum, Gcid);
                        dt.Rows[i][7] = sgbll.GetLeaderQgroup(sgroupnum, Gcid);
                    }

                    //LearnSite.Common.Log.AddLogTable(dt);//调试信息
                }
            }
            return dt;
        }
        private void GetSgroups(DataTable dttotal, DataTable dmp)
        {
            int scount = dttotal.Rows.Count;
            for (int i = 0; i < scount; i++)
            {
                string snum = dttotal.Rows[i]["学号"].ToString();
                int dcount = dmp.Rows.Count;
                for (int j = 0; j < dcount; j++)
                {
                    string xsnum = dmp.Rows[j]["Snum"].ToString();//第2张表获取的学号字段要重命名为Snum
                    string mygroup = dmp.Rows[j]["Sgroup"].ToString();//第2张表获取的分值字段要重命名为Sgroup
                    if (snum == xsnum)
                    {
                        dttotal.Rows[i]["Sgroup"] = mygroup;
                        break;
                    }
                }
            }         
        }
        /// <summary>
        /// 获取未参组班级内学生
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public DataTable NoGroupStudents(int Sgrade, int Sclass,string sort)
        {
            string mysql = "";
            switch (sort)
            { 
                case "0":
                    mysql = "select Sid,Snum,Sname,Sscore from Students where Sgrade=" + Sgrade + " and Sclass=" + Sclass + " and (Sgroup =0 or Sgroup is null) order by Sscore desc";
                    break;
                case "1":
                    mysql = "select Sid,Snum,Sname,Sscore from Students where Sgrade=" + Sgrade + " and Sclass=" + Sclass + " and (Sgroup =0 or Sgroup is null) order by Snum desc";
                    break;            
            }
            return DbHelperSQL.Query(mysql).Tables[0];
        }

        /// <summary>
        /// 获取本小组成员姓名和学号 Snum as Head, Sname,Sex
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Sgroup"></param>
        /// <param name="Sid"></param>
        /// <returns></returns>
        public DataTable Teamer(int Sgrade, int Sclass,int Sgroup, string Snum, string Sname, string Sex)
        {
            string mysql = "select Snum,Sname,Sex from Students where Sgroup<>0 and  Sgroup=" + Sgroup + " and Sgrade=" + Sgrade + " and Sclass=" + Sclass + "  order by Sscore desc";
            if (Snum.IndexOf('s') > -1)
            {
                mysql = "select Snum,Sname,Sex from Students where Sleader=1 and Sgroup<>0 and  Sgrade=" + Sgrade + " and Sclass=" + Sclass + "  order by Sscore desc";
            }

            DataTable dt = DbHelperSQL.Query(mysql).Tables[0];
            dt.Columns.Add("Avatar", typeof(System.String));
            int dcount = dt.Rows.Count;
            if (dcount > 0)
            {
                for (int i = 0; i < dcount; i++)
                {
                    string snuma = dt.Rows[i][0].ToString();
                    string sex = dt.Rows[i][2].ToString();
                    string imgurl = LearnSite.Common.Photo.GetStudentPhotoUrl(snuma, sex).Replace("~", "..");
                    dt.Rows[i][0] = "stu" + snuma;
                    dt.Rows[i][3] = imgurl;
                }
            }
            else
            {
                DataRow dw = dt.NewRow();

                string imgurl = LearnSite.Common.Photo.GetStudentPhotoUrl(Snum, Sex);
                string Head = imgurl.Replace("~", "..");

                dw[0] = "stu" + Snum;
                dw[1] = Sname;
                dw[2] = Sex;
                dw[3] = Head;
                dt.Rows.Add(dw);
            }

            return dt;
        }

        /// <summary>
        /// 聊天表情图标
        /// </summary>
        /// <returns></returns>
        public DataTable Emo() {
            DataTable dt = new DataTable();
            dt.Columns.Add("Emo",typeof(string));
            for (int i = 1; i < 61; i++)
            {
                string num = i.ToString("D2");
                string emourl = "../code/imgchat/emo/emo_" + num + ".gif";
                DataRow row = dt.NewRow();
                row[0] = emourl;
                dt.Rows.Add(row);           
            }
            return dt;
        }


        /// <summary>
        /// 获取本小组成员姓名字符串，以顿号为分隔符
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Sgroup"></param>
        /// <param name="Sid"></param>
        /// <returns></returns>
        public string GroupTeam(int Sgrade, int Sclass, int Sgroup)
        {
            string mysql = "select Sname from Students where Sgroup<>0 and Sgroup=" + Sgroup + " and Sgrade=" + Sgrade + " and Sclass=" + Sclass + "order by Sleader desc";
            DataTable dt = DbHelperSQL.Query(mysql).Tables[0];
            int dcount = dt.Rows.Count;
            string numstr = "";
            if (dcount > 0)
            {
                for (int i = 0; i < dcount; i++)
                {
                    numstr = numstr + dt.Rows[i][0].ToString();
                    if (i < dcount - 1)
                    {
                        numstr = numstr + "、";
                    }
                }
            }

            return numstr;
        }

        /// <summary>
        /// 获取本小组成员字符串，以逗号为分隔符
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Sgroup"></param>
        /// <returns></returns>
        public string GroupSnums(int Sgrade, int Sclass, int Sgroup)
        {
            string mysql = "select Snum from Students where  Sgroup=" + Sgroup + " and Sgrade=" + Sgrade + " and Sclass=" + Sclass + " order by Sscore desc";
            DataTable dt = DbHelperSQL.Query(mysql).Tables[0];
            int dcount = dt.Rows.Count;
            string numstr = "";
            if (dcount > 0) {
                for (int i = 0; i < dcount; i++) {
                    numstr = numstr + dt.Rows[i][0].ToString();
                    if (i < dcount - 1) {
                        numstr = numstr + ",";
                    }                
                }            
            }

            return numstr;
        }

        /// <summary>
        /// 获取本小组成员
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Sgroup"></param>
        /// <returns></returns>
        public DataTable GroupMembers(int Sgrade, int Sclass, int Sgroup)
        {
            string mysql = "select Sid,Sname,Sscore from Students where Sleader=0 and Sgroup=" + Sgroup + " and Sgrade=" + Sgrade + " and Sclass=" + Sclass + " order by Sscore desc";
            return  DbHelperSQL.Query(mysql).Tables[0];
        }
        /// <summary>
        /// 获取本小组网盘成员（包括组长）Sid,Snum,Sname
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Sgroup"></param>
        /// <returns></returns>
        public DataTable GroupDiskMembers(int Sgrade, int Sclass, int Sgroup)
        {
            string mysql = "select Sid,Snum,Sname from Students where  Sgroup=" + Sgroup + " and Sgrade=" + Sgrade + " and Sclass=" + Sclass + " order by Sid";
            return DbHelperSQL.Query(mysql).Tables[0];
        }
        /// <summary>
        /// 获取本组内的作品平均分
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Sgroup"></param>
        /// <returns></returns>
        public string MyGroupSscores(int Sgrade, int Sclass, int Sgroup)
        {
            string mysql = "select avg(Sscore) from Students where  Sgroup=" + Sgroup + " and Sgrade=" + Sgrade + " and Sclass=" + Sclass;
            return DbHelperSQL.FindString(mysql);
        }
        /// <summary>
        /// 初始化小组名称为组长姓名
        /// </summary>
        /// <returns></returns>
        public int InitSgtitle()
        {
            string mysql = "update Students set Sgtitle=Sname where Sleader=1 and  Sgtitle is null";
            return DbHelperSQL.ExecuteSql(mysql);
        }
        /// <summary>
        /// 根据Sid获取组号
        /// </summary>
        /// <param name="Sid"></param>
        /// <returns></returns>
        public int GetSgroup(int Sid)
        {
            string mysql = "select Sgroup from Students where Sid=" + Sid;
            return DbHelperSQL.FindNum(mysql);
        }
        public void UpdateKaoxu(string kaoxu, string Sname)
        {
            string mysql = "update Students set Skaoxu='"+kaoxu+"' where Sname='"+Sname+"'";
            DbHelperSQL.ExecuteSql(mysql);
        }

        public int UpdateStat(int Sgrade, int Classone ,int Classtwo ,DateTime Wdate)
        {
            // update Students set Stat=1 from Students,Works where Snum=Wnum and Wgrade=8 and (Wclass=3 or Wclass=4) and Wdate>'2018-10-16'
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Students set Stat=1 from Students,Works ");
            strSql.Append("  where Snum=Wnum and Wgrade=@Sgrade and (Wclass=@Classone or Wclass=@Classtwo) and Wdate>@Wdate  ");
            SqlParameter[] parameters = {
					new SqlParameter("@Sgrade", SqlDbType.Int,4),                                        
					new SqlParameter("@Classone", SqlDbType.Int,4),                                        
					new SqlParameter("@Classtwo", SqlDbType.Int,4),        
					new SqlParameter("@Wdate", SqlDbType.DateTime)};
            parameters[0].Value = Sgrade;
            parameters[1].Value = Classone;
            parameters[2].Value = Classtwo;
            parameters[3].Value = Wdate;

          return  DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);

        }

        public int UpdateClass(int Sgrade, int Classone, int Classtwo,int Classset)
        {
            // update Students set Stat=1 from Students,Works where Snum=Wnum and Wgrade=8 and (Wclass=3 or Wclass=4) and Wdate>'2018-10-16'
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Students set Sclass=@Classset from Students ");
            strSql.Append("  where Sgrade=@Sgrade and (Sclass=@Classone or Sclass=@Classtwo) and Stat=1 ");
            SqlParameter[] parameters = {
					new SqlParameter("@Sgrade", SqlDbType.Int,4),                                        
					new SqlParameter("@Classone", SqlDbType.Int,4),                                        
					new SqlParameter("@Classtwo", SqlDbType.Int,4),                                        
					new SqlParameter("@Classset", SqlDbType.Int,4)};

            parameters[0].Value = Sgrade;
            parameters[1].Value = Classone;
            parameters[2].Value = Classtwo;
            parameters[3].Value = Classset;

            return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);

        }

        public int  UpdateClassNoSign(int Sgrade, int Classone, int Classtwo, int Classset)
        {
            // update Students set Stat=1 from Students,Works where Snum=Wnum and Wgrade=8 and (Wclass=3 or Wclass=4) and Wdate>'2018-10-16'
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Students set Sclass=@Classset from Students ");
            strSql.Append("  where Sgrade=@Sgrade and (Sclass=@Classone or Sclass=@Classtwo) and (Stat is null or Stat=0) ");
            SqlParameter[] parameters = {
					new SqlParameter("@Sgrade", SqlDbType.Int,4),                                        
					new SqlParameter("@Classone", SqlDbType.Int,4),                                        
					new SqlParameter("@Classtwo", SqlDbType.Int,4),                                        
					new SqlParameter("@Classset", SqlDbType.Int,4)};

            parameters[0].Value = Sgrade;
            parameters[1].Value = Classone;
            parameters[2].Value = Classtwo;
            parameters[3].Value = Classset;

            return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 获得组长推荐列表
        /// </summary>
        public DataTable GetListTeam(int Sgrade, int Sclass)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Sid,Snum,Sname,Sleader ");
            strSql.Append(" FROM Students ");
            strSql.Append(" where Sgrade=@Sgrade and Sclass=@Sclass ");
            SqlParameter[] parameters = {
					new SqlParameter("@Sgrade", SqlDbType.Int,4),
                    new SqlParameter("@Sclass", SqlDbType.Int,4)};

            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;

            DataTable dt = DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
            int dcount = dt.Rows.Count;
            if (dcount > 0)
            {
                dt.Columns.Add("Steam", typeof(int));
                for (int i = 0; i < dcount; i++)
                {
                    string sid = dt.Rows[i]["Sid"].ToString();
                    string mysql = "select count(*) FROM Students where Steam=" + sid;
                    int steam = DbHelperSQL.FindNum(mysql);
                    dt.Rows[i]["Steam"] = steam;
                }

            }

            return dt;
        }


        /// <summary>
        /// 更新该Sid学生的组长推荐
        /// </summary>
        /// <param name="Sid">自己ID</param>
        /// <param name="Steam">组长ID</param>
        public void UpdateSidSteam(int Sid, int Steam)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Students set Steam=@Steam ");
            strSql.Append("where Sid=@Sid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Sid", SqlDbType.Int,4),
					new SqlParameter("@Steam", SqlDbType.Int,4)};
            parameters[0].Value = Sid;
            parameters[1].Value = Steam;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
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
			parameters[0].Value = "Students";
			parameters[1].Value = "ID";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

        /// <summary>
        /// 获取所有年级列表
        /// </summary>
        public DataSet GetGradeList()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct Sgrade as Grade from Students order by Sgrade asc");
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 根据年级获取班级列表
        /// </summary>
        public DataSet GetClassList(int Sgrade)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct Sclass as Class from Students where Sgrade=@Sgrade order by Sclass asc");
            SqlParameter[] parameters = {
                    new SqlParameter("@Sgrade", SqlDbType.Int,4)};

            parameters[0].Value = Sgrade;
            return DbHelperSQL.Query(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据学号统计签到次数
        /// </summary>
        /// <param name="snum">学号</param>
        /// <param name="Sgrade">年级</param>
        /// <param name="Sclass">班级</param>
        /// <returns>签到次数</returns>
        public int GetSignInCountBySnum(string snum, int Sgrade, int Sclass)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(*) as SignCount ");
            strSql.Append("FROM Signin ");
            strSql.Append("WHERE Qnum = @Snum ");
            strSql.Append("AND Qgrade = @Sgrade ");
            strSql.Append("AND Qclass = @Sclass");

            SqlParameter[] parameters = {
                new SqlParameter("@Snum", SqlDbType.NVarChar, 50),
                new SqlParameter("@Sgrade", SqlDbType.Int, 4),
                new SqlParameter("@Sclass", SqlDbType.Int, 4)
            };
            parameters[0].Value = snum;
            parameters[1].Value = Sgrade;
            parameters[2].Value = Sclass;

            object result = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (result != null && result != DBNull.Value)
            {
                return Convert.ToInt32(result);
            }
            return 0;
        }

        /// <summary>
        /// 更新学生学号
        /// </summary>
        /// <param name="Sid">学生ID</param>
        /// <param name="newSnum">新学号</param>
        /// <returns>是否成功</returns>
        public bool UpdateSnum(int Sid, string newSnum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Students set ");
            strSql.Append("Snum=@Snum ");
            strSql.Append(" where Sid=@Sid");
            
            SqlParameter[] parameters = {
                new SqlParameter("@Snum", SqlDbType.NVarChar, 50),
                new SqlParameter("@Sid", SqlDbType.Int, 4)
            };
            parameters[0].Value = newSnum;
            parameters[1].Value = Sid;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            return rows > 0;
        }

        /// <summary>
        /// 获取班级学生统计信息（包含签到次数、作业完成情况等）
        /// </summary>
        /// <param name="Sgrade">年级</param>
        /// <param name="Sclass">班级</param>
        /// <param name="term">学期，0表示全部</param>
        /// <param name="cid">课程ID，0表示全部</param>
        /// <returns>学生统计DataTable</returns>
        public DataTable GetStudentStats(int Sgrade, int Sclass, int term, int cid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT s.Sid, s.Snum, s.Sname, s.Sseat, ");
            strSql.Append("ISNULL(s.Sscore, 0) as Sscore, ");
            strSql.Append("ISNULL(s.Sattitude, 0) as Sattitude, ");
            strSql.Append("ISNULL(s.Squiz, 0) as Squiz, ");
            strSql.Append("ISNULL(s.Swscore, 0) as Swscore, ");
            strSql.Append("ISNULL(s.Stenscore, 0) as Stenscore, ");
            
            string termFilter = term > 0 ? " AND Qterm = " + term : "";
            string cidFilter = cid > 0 ? " AND Qcid = " + cid : "";
            string workTermFilter = term > 0 ? " AND Wterm = " + term : "";
            string workCidFilter = cid > 0 ? " AND Wcid = " + cid : "";
            
            strSql.Append("(SELECT COUNT(*) FROM Signin WHERE Qnum = s.Snum AND Qgrade = s.Sgrade AND Qclass = s.Sclass" + termFilter + cidFilter + ") as SignCount, ");
            strSql.Append("(SELECT COUNT(*) FROM Works WHERE Wnum = s.Snum AND Wgrade = s.Sgrade AND Wclass = s.Sclass" + workTermFilter + workCidFilter + ") as WorkCount, ");
            strSql.Append("(SELECT ISNULL(SUM(Wscore), 0) FROM Works WHERE Wnum = s.Snum AND Wgrade = s.Sgrade AND Wclass = s.Sclass" + workTermFilter + workCidFilter + ") as WorkScore, ");
            strSql.Append("(SELECT COUNT(*) FROM Signin WHERE Qnum = s.Snum AND Qgrade = s.Sgrade AND Qclass = s.Sclass AND Qattitude > 0" + termFilter + cidFilter + ") as AddCount, ");
            strSql.Append("(SELECT COUNT(*) FROM Signin WHERE Qnum = s.Snum AND Qgrade = s.Sgrade AND Qclass = s.Sclass AND Qattitude < 0" + termFilter + cidFilter + ") as SubCount, ");
            strSql.Append("(SELECT ISNULL(SUM(CASE WHEN Qattitude > 0 THEN Qattitude ELSE 0 END), 0) FROM Signin WHERE Qnum = s.Snum AND Qgrade = s.Sgrade AND Qclass = s.Sclass" + termFilter + cidFilter + ") as TotalAddScore, ");
            strSql.Append("(SELECT ISNULL(SUM(CASE WHEN Qattitude < 0 THEN ABS(Qattitude) ELSE 0 END), 0) FROM Signin WHERE Qnum = s.Snum AND Qgrade = s.Sgrade AND Qclass = s.Sclass" + termFilter + cidFilter + ") as TotalSubScore, ");
            strSql.Append("(SELECT STUFF((SELECT '、' + Qnote FROM Signin WHERE Qnum = s.Snum AND Qgrade = s.Sgrade AND Qclass = s.Sclass AND Qattitude > 0 AND Qnote IS NOT NULL AND Qnote != ''" + termFilter + cidFilter + " FOR XML PATH('')), 1, 1, '') ) as AddReasons, ");
            strSql.Append("(SELECT STUFF((SELECT '、' + Qnote FROM Signin WHERE Qnum = s.Snum AND Qgrade = s.Sgrade AND Qclass = s.Sclass AND Qattitude < 0 AND Qnote IS NOT NULL AND Qnote != ''" + termFilter + cidFilter + " FOR XML PATH('')), 1, 1, '') ) as SubReasons, ");
            strSql.Append("(SELECT MAX(cnt) FROM (SELECT COUNT(*) as cnt FROM Signin WHERE Qgrade = " + Sgrade + " AND Qclass = " + Sclass + termFilter + cidFilter + " GROUP BY Qnum) t) as ShouldCount ");
            strSql.Append("FROM Students s ");
            strSql.Append("WHERE s.Sgrade = @Sgrade AND s.Sclass = @Sclass ");
            strSql.Append("ORDER BY s.Snum ASC");

            SqlParameter[] parameters = {
                new SqlParameter("@Sgrade", SqlDbType.Int, 4),
                new SqlParameter("@Sclass", SqlDbType.Int, 4)
            };
            parameters[0].Value = Sgrade;
            parameters[1].Value = Sclass;

            return DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
        }

		#endregion  成员方法
	}
}

