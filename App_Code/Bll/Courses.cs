using System;
using System.Data;
using System.Collections.Generic;
using LearnSite.Model;
namespace LearnSite.BLL
{
	/// <summary>
	/// 业务逻辑类Courses 的摘要说明。
	/// </summary>
	public class Courses
	{
		private readonly LearnSite.DAL.Courses dal=new LearnSite.DAL.Courses();
		public Courses()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

        /// <summary>
        /// 将所有学案编号和标题生成对照目录放到学案包目录下，方便以后查看
        /// </summary>
        /// <param name="xmlpath"></param>
        public void CreatPackageNameList(string xmlpath)
        {
            dal.CreatPackageNameList(xmlpath);
        }
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Cid)
		{
			return dal.Exists(Cid);
		}

        /// <summary>
        /// 返回任务完成情况
        /// </summary>
        /// <param name="Cid"></param>
        /// <returns></returns>
        public int[] Workrecord(string Cid)
        {
            return dal.Workrecord(Cid);
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LearnSite.Model.Courses model)
		{
			return dal.Add(model);
		}

        /// <summary>
        /// 创建新学案（Ctitle,Cclass，Cdate,Cobj,Cterm,Cks,Cfiletype,Cupload,Cpublish），返回自动编号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Create(LearnSite.Model.Courses model)
        {
            return dal.Create(model);
        }

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LearnSite.Model.Courses model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// 更新一条数据，更新学案内容
        /// </summary>
        public void UpdateCourse(LearnSite.Model.Courses model)
        {
            dal.UpdateCourse(model);
        }                
        /// <summary>
        /// 更新一条数据,更新学案内容，学案包导入时专用
        /// </summary>
        public void UpdateCcontent(int Cid, string Ccontent, string Cbanner)
        {
            dal.UpdateCcontent(Cid, Ccontent, Cbanner);
        }
        /// <summary>
        /// 修改学案发布状态
        /// </summary>
        public void UpdateCpublish(int Cid, bool Cpublish)
        {
            dal.UpdateCpublish(Cid, Cpublish);
        }

        /// <summary>
        /// 修改学案发布状态
        /// </summary>
        public void UpdateCpublish(int Cid)
        {
            dal.UpdateCpublish(Cid);
        }                
        /// <summary>
        /// 修改学案入库状态
        /// </summary>
        public void UpdateCold(int Cid)
        {
            dal.UpdateCold(Cid);
        }
        /// <summary>
        /// 修改学案推荐状态
        /// </summary>
        public void UpdateCgood(int Cid)
        {
            dal.UpdateCgood(Cid);
        }
		/// <summary>
		/// 删除一条数据
		/// </summary>
        public void Delete(int Cid, int Chid)
		{

			dal.Delete(Cid,Chid);
		}

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void DeleteCourse(int Cid, int Chid)
        {
            dal.DeleteCourse(Cid, Chid);
        }
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LearnSite.Model.Courses GetModel(int Cid)
		{

			return dal.GetModel(Cid);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中。
		/// </summary>
		public LearnSite.Model.Courses GetModelByCache(int Cid)
		{

			string CacheKey = "CoursesModel-" + Cid;
            object objModel = LearnSite.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(Cid);
					if (objModel != null)
					{
                        int ModelCache = LearnSite.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LearnSite.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (LearnSite.Model.Courses)objModel;
		}


                /// <summary>
        /// 从表中第一条记录得到一个对象实体
        /// </summary>
        public LearnSite.Model.Courses GetTableModel(DataTable dt)
        {
            return dal.GetTableModel(dt);
        }


        /// <summary>
        /// 获取网页制作活动Mid
        /// </summary>
        /// <param name="Mcid"></param>
        /// <returns></returns>
        public string GetHtmlMid(string Mcid)
        {
            return dal.GetHtmlMid(Mcid);
        }

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public  DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetListNoWhere(string strWhere)
        {
            return dal.GetListNo(strWhere);
        }
        /// <summary>
        /// 获得无内容学案列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet GetListNoContent(string strWhere)
        {
            return dal.GetListNoContent(strWhere);
        }               
        /// <summary>
        /// 获取本学期本教师的学案编号和标题列表
        /// </summary>
        /// <param name="Chid"></param>
        /// <param name="Cobj"></param>
        /// <param name="Cterm"></param>
        /// <returns></returns>
        public DataSet GetListCidCtitle(int Chid, int Cobj, int Cterm)
        {
            return dal.GetListCidCtitle(Chid, Cobj, Cterm);
        }
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LearnSite.Model.Courses> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LearnSite.Model.Courses> DataTableToList(DataTable dt)
		{
			return BllDataTableMappers.MapCoursesList(dt);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public  DataSet GetAllList()
		{
			return GetList("");
		}
        /// <summary>
        ///无内容， 无条件，按日期排序
        /// </summary>
        /// <returns></returns>
        public DataSet GetListNo()
        {
            string strOrder = " order by Cdate DESC";
            return GetListNoWhere(strOrder);
        }
        /// <summary>
        /// 无内容， 无条件，按学期、年级、课时升序排序
        /// </summary>
        /// <param name="Cterm"></param>
        /// <param name="Cobj"></param>
        /// <returns></returns>
        public DataSet GetListLession(int Cterm, int Cobj,int Chid)
        {
            return dal.GetListLession(Cterm, Cobj,Chid);
        }
        /// <summary>
        /// 获得指定年级、指定学期的所有学案，无内容
        /// </summary>
        /// <param name="Cobj"></param>
        /// <param name="Cterm"></param>
        /// <returns></returns>
        public DataSet GetLimitList(int Cobj, int Cterm)
        {
            string strWhere = "Cobj=" + Cobj + " and Cterm=" + Cterm + " order by Cks DESC";
            return GetListNoContent(strWhere);
        }
        /// <summary>
        /// 获得指定年级、指定学期的已学学案，无内容，标题
        /// </summary>
        /// <param name="Cobj"></param>
        /// <param name="Cterm"></param>
        /// <returns></returns>
        public DataSet GetLimitGoodList(int Cobj, int Cterm, string Snum)
        {
            string strWhere = "Cobj=" + Cobj + " and Cterm=" + Cterm + " and Cid in(select distinct Wcid from Works where Wnum='" + Snum + "') order by Cks ASC";
            return dal.GetListGoodNoContent(strWhere);
        }
        /// <summary>
        /// 获得指定年级、指定学期的所有学案，无内容，标题
        /// </summary>
        /// <param name="Cobj"></param>
        /// <param name="Cterm"></param>
        /// <returns></returns>
        public DataSet GetAllGoodList(int Cobj, int Cterm)
        {
            string strWhere = "Cobj=" + Cobj + " and Cterm=" + Cterm + " and  Cgood=1 and  Cdelete=0 and Cold=0 order by Cks ASC";
            return dal.GetListGoodNoContent(strWhere);
        }
        /// <summary>
        /// 获得指定教师、指定年级、指定学期的所有学案，无内容，标题
        /// </summary>
        /// <param name="Cobj"></param>
        /// <param name="Cterm"></param>
        /// <returns></returns>
        public DataSet GetAllGoodListByChid(int Cobj, int Cterm,int Chid)
        {
            string strWhere = "Chid=" + Chid + " and Cobj=" + Cobj + " and Cterm=" + Cterm + " and  Cgood=1 and Cdelete=0 and Cold=0 order by Cks ASC";
            return dal.GetListGoodNoContent(strWhere);
        }
        /// <summary>
        /// 获得该教师指定年级、指定学期的所有学案，无内容
        /// </summary>
        /// <param name="Cobj"></param>
        /// <param name="Cterm"></param>
        /// <param name="Chid"></param>
        /// <returns></returns>
        public DataSet GetLimitHidList(int Cobj, int Cterm, int Chid)
        {
            string strWhere = "Chid=" + Chid + " and  Cobj=" + Cobj + " and Cterm=" + Cterm + " and Cdelete=0 and Cold=0 order by Cks DESC";
            return GetListNoContent(strWhere);
        }
        /// <summary>
        /// 获得该教师入库的学案，无内容
        /// </summary>
        /// <param name="Cobj"></param>
        /// <param name="Cterm"></param>
        /// <param name="Chid"></param>
        /// <returns></returns>
        public DataSet GetOldHidList(int Cobj, int Cterm, int Chid)
        {
            string strWhere = "Chid=" + Chid + " and  Cobj=" + Cobj + " and Cterm=" + Cterm + " and Cdelete=0 and Cold=1  order by Cks DESC";
            return GetListNoContent(strWhere);
        }
        /// <summary>
        /// 获得指定cid的一条学案记录
        /// </summary>
        /// <param name="Cid"></param>
        /// <returns></returns>
        public DataSet GetCourseDetail(int Cid)
        {
            string strWhere = "Cid=" + Cid ;
            return GetList(strWhere);
        }

        /// <summary>
        /// 根据学案编号，返回学案标题
        /// </summary>
        /// <param name="Fcid"></param>
        /// <returns></returns>
        public string GetTitle(int Cid)
        {
            return dal.GetTitle(Cid);
        }

        /// <summary>
        /// 根据学案编号，返回学案横幅
        /// </summary>
        /// <param name="Fcid"></param>
        /// <returns></returns>
        public string GetBanner(int Cid)
        {
            return dal.GetBanner(Cid);
        }
        /// <summary>
        /// 根据教师、年级、学期返回学案名称和编号
        /// </summary>
        /// <param name="Cobj"></param>
        /// <param name="Cterm"></param>
        /// <param name="Chid"></param>
        /// <returns></returns>
        public DataTable  ShowCidCtitle(int Chid, int Cobj, int Cterm)
        {
            return dal.ShowCidCtitle(Chid, Cobj, Cterm).Tables[0];
        }
        /// <summary>
        /// 根据年级显示本班教师最新未学学案
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="GridViewnewkc"></param>
        public DataTable ShowNewCourse(int Sgrade, int Cterm, int Chid, string Cids)
        {
            return dal.ShowNewCourse(Sgrade, Cterm,Chid,Cids);
        }                
        /// <summary>
        /// 根据年级显示本班教师最新未学学案
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="GridViewnewkc"></param>
        public DataTable ShowNewCourseNew(int Sgrade, int Cterm, int Chid, string Cids)
        {
            return dal.ShowNewCourseNew(Sgrade, Cterm, Chid, Cids);
        }
        /// <summary>
        /// 根据Snum显示已学学案 (Snum为Students表的学号)
        /// </summary>
        /// <param name="Sid"></param>
        /// <param name="GridViewdonekc"></param>
        public DataTable ShowDoneCourse(string Cids)
        {
            return dal.ShowDoneCourse(Cids);
        }                
        /// <summary>
        /// 根据Snum显示已学学案 (Snum为Students表的学号)
        /// </summary>
        /// <param name="Sid"></param>
        /// <param name="GridViewdonekc"></param>
        public DataTable ShowDoneCourseNew(string Cids)
        {
            return dal.ShowDoneCourseNew(Cids);
        }
        /// <summary>
        /// 根据班级显示已学学案
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public DataTable ShowClassDoneCourse(int Sgrade, int Chid, string Cids)
        {
            return dal.ShowClassDoneCourse(Sgrade,Chid,Cids);
        }
        /// <summary>
        /// 根据班级显示未学学案，教师上课页面
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public DataTable ShowClassnewCourse(int Sgrade, int Chid, string Cids)
        {
            return dal.ShowClassnewCourse(Sgrade,Chid,Cids);
        }
        /// <summary>
        /// 根据学案编号返回学案名称
        /// </summary>
        /// <param name="Cid"></param>
        /// <returns></returns>
        public string ShowCtitle(int Cid)
        {
          return  dal.ShowCtitle(Cid);
        }
         /// <summary>
        /// 获取指定学期，指定年级的课时最大值
        /// </summary>
        /// <param name="Cterm"></param>
        /// <param name="Cobj"></param>
        /// <returns></returns>
        public int CksMaxValue(int Cterm, int Cobj,int Chid)
        {
            return dal.CksMaxValue(Cterm, Cobj,Chid);
        }

        /// <summary>
        /// 获取网页制作学案的年级列表
        /// </summary>
        /// <returns></returns>
        public DataSet GethtmGrade()
        {
            return dal.GethtmGrade();
        }
        /// <summary>
        /// 全部学案收回隐藏
        /// </summary>
        public void HideCourse()
        {
            dal.HideCourse();
        }                
        /// <summary>
        /// 将学案的上传类型跟该学案活动更新一致
        /// </summary>
        public void UpdateCfiletype()
        {
            dal.UpdateCfiletype();
        }

        public void InitCdelete()
        {
            dal.InitCdelete();
        }

        public void InitCold()
        {
            dal.InitCold();
        }


        /// <summary>
        /// 返回本课的活动、调查、讨论、表单 汇总表
        /// </summary>
        /// <param name="Cid"></param>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        public DataTable CourseRate(int Cid, int Sgrade, int Sclass)
        {
            Students sbll = new Students();
            DataTable dtstus = sbll.GetStudentsSnumSidSname(Sgrade, Sclass).Tables[0];//学号和姓名
            int scount=dtstus.Rows.Count;
            if (scount> 0)
            {
                ListMenu lbll = new ListMenu();
                DataTable dt = lbll.GetShowedMenu(Cid).Tables[0];
                int dcount = dt.Rows.Count;
                if (dcount > 0)
                {
                    List<int> lidList = new List<int>();
                    for (int i = 0; i < dcount; i++)
                    {
                        string Ltype = dt.Rows[i]["Ltype"].ToString();//获取学案项目类型：1活动2调查3讨论4表单
                        int Lxid = Convert.ToInt32(dt.Rows[i]["Lxid"].ToString());//获取对应项目ID编号
                        int Lid = Convert.ToInt32(dt.Rows[i]["Lid"].ToString());//获取菜单ID编号
                        string Ltitle = dt.Rows[i]["Ltitle"].ToString().Replace(" ", "");//获取菜单标题
                        string displayTitle = GetUniqueColumnName(dtstus.Columns, Ltitle);
                        string Ltitlestr = "l" + Ltype + "x" + Lxid.ToString() + "c" + i.ToString();

                        dtstus.Columns.Add(Ltitlestr, typeof(string));
                        dtstus.Columns[Ltitlestr].ColumnName = displayTitle;
                        lidList.Add(Lid);
                    }

                    MenuWorks kbll = new MenuWorks();
                    Dictionary<string, int> spendLookup = new Dictionary<string, int>();
                    if (lidList.Count > 0)
                    {
                        string lidWhere = string.Join(",", lidList.ToArray());
                        DataTable dtWorks = kbll.GetList("Klid in (" + lidWhere + ")").Tables[0];
                        int workCount = dtWorks.Rows.Count;
                        for (int w = 0; w < workCount; w++)
                        {
                            string sidStr = dtWorks.Rows[w]["Ksid"].ToString();
                            string lidStr = dtWorks.Rows[w]["Klid"].ToString();
                            int seconds = 0;
                            if (dtWorks.Columns.Contains("Kseconds") && dtWorks.Rows[w]["Kseconds"] != DBNull.Value && dtWorks.Rows[w]["Kseconds"].ToString() != "")
                            {
                                seconds = Convert.ToInt32(dtWorks.Rows[w]["Kseconds"]);
                            }
                            else if (dtWorks.Rows[w]["Ktime"] != DBNull.Value && dtWorks.Rows[w]["Ktime"].ToString() != "")
                            {
                                seconds = Convert.ToInt32(dtWorks.Rows[w]["Ktime"]) * 60;
                            }
                            spendLookup[sidStr + "-" + lidStr] = seconds;
                        }
                        dtWorks.Dispose();
                    }

                    for (int j = 0; j < scount; j++)
                    {
                        int pretime = 0;
                        for (int i = 0; i < dcount; i++)
                        {
                            int Sid = Int32.Parse(dtstus.Rows[j][0].ToString());
                            int Lid = Int32.Parse(dt.Rows[i][0].ToString());//获取对应项目ID编号
                            int nowtime = 0;
                            string workKey = Sid.ToString() + "-" + Lid.ToString();
                            if (spendLookup.ContainsKey(workKey))
                            {
                                nowtime = spendLookup[workKey];
                            }
                            dtstus.Rows[j][i + 3] = nowtime - pretime;
                            pretime = nowtime;//将前个作品花费时间减去
                        }
                    }
                }
                dt.Dispose();
            }
            dtstus.Columns["Snum"].ColumnName = "学号";
            dtstus.Columns["Sname"].ColumnName = "姓名";
            dtstus.Columns.RemoveAt(0);
            return dtstus;
        }

        /// <summary>
        /// 返回本课的活动、调查、讨论、表单 汇总表
        /// </summary>
        /// <param name="Cid"></param>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        public DataTable CourseTotals(int Cid, int Sgrade, int Sclass)
        {
            Students sbll = new Students();
            DataTable dtstus = sbll.GetStudentsSnumSname(Sgrade, Sclass).Tables[0];//学号和姓名
            if (dtstus.Rows.Count > 0)
            {
                ListMenu lbll = new ListMenu();
                DataTable dt = lbll.GetShowedMenu(Cid).Tables[0];
                int dcount = dt.Rows.Count;
                if (dcount > 0)
                {
                    Works wbll = new Works();
                    SurveyFeedback fbll = new SurveyFeedback();
                    TopicReply rbll = new TopicReply();
                    TxtFormBack xbll = new TxtFormBack();
                    Solves vbll = new Solves();

                    for (int i = 0; i < dcount; i++)
                    {
                        string Ltype = dt.Rows[i]["Ltype"].ToString();//获取学案项目类型：1活动2调查3讨论4表单
                        int Lxid = Convert.ToInt32(dt.Rows[i]["Lxid"].ToString());//获取对应项目ID编号
                        string Ltitle = dt.Rows[i]["Ltitle"].ToString().Replace(" ", "");//获取菜单标题
                        string displayTitle = GetUniqueColumnName(dtstus.Columns, Ltitle);
                        string Ltitlestr = "l" + Ltype + "x" + Lxid.ToString()+"c"+i.ToString();
                        switch (Ltype)
                        {
                            case "1"://活动上传作品
                            case "5"://scratch编程
                            case "8"://py编程
                            case "10"://流程图
                            case "11"://像素画
                            case "12"://单网页
                            case "13"://拼图
                            case "14"://积木
                                DataTable dtms = wbll.getScoreList(Lxid, Sgrade, Sclass);
                                if (dtms.Rows.Count > 0)
                                {
                                    dtstus.Columns.Add(Ltitlestr, typeof(string));
                                    GetScore(dtstus, Ltitlestr, dtms);
                                    dtstus.Columns[Ltitlestr].ColumnName = displayTitle;
                                }
                                dtms.Dispose();
                                break;
                            case "2"://调查
                                DataTable dtsf = fbll.GetClassScore(Lxid, Sgrade, Sclass);
                                if (dtsf.Rows.Count > 0)
                                {
                                    dtstus.Columns.Add(Ltitlestr, typeof(string));
                                    GetScore(dtstus, Ltitlestr, dtsf);
                                    dtstus.Columns[Ltitlestr].ColumnName = displayTitle;
                                }
                                dtsf.Dispose();
                                break;
                            case "3"://讨论
                                DataTable dttr = rbll.GetClassListScore(Sgrade, Sclass, Lxid);
                                if (dttr.Rows.Count > 0)
                                {
                                    dtstus.Columns.Add(Ltitlestr, typeof(string));
                                    GetScore(dtstus, Ltitlestr, dttr);
                                    dtstus.Columns[Ltitlestr].ColumnName = displayTitle;
                                }
                                dttr.Dispose();
                                break;
                            case "4"://表单
                                DataTable dttx =xbll.GetClassTxtFormScore(Sgrade, Sclass, Lxid);
                                if (dttx.Rows.Count > 0)
                                {
                                    dtstus.Columns.Add(Ltitlestr, typeof(string));
                                    GetScore(dtstus, Ltitlestr, dttx);
                                    dtstus.Columns[Ltitlestr].ColumnName = displayTitle;
                                }
                                dttx.Dispose();
                                break;
                            case "9":///交互式python测评
                                DataTable dttv = vbll.GetClassListScore(Sgrade, Sclass, Lxid);
                                if (dttv.Rows.Count > 0)
                                {
                                    dtstus.Columns.Add(Ltitlestr, typeof(string));
                                    GetScore(dtstus, Ltitlestr, dttv);
                                    dtstus.Columns[Ltitlestr].ColumnName = displayTitle;
                                }
                                dttv.Dispose();
                                break;
                        }
                    }
                }
                dt.Dispose();
                //汇总
                int cml = dtstus.Columns.Count;
                if (cml >= 2)
                {
                    Signin gbll = new Signin();
                    DataTable dtatd = gbll.GetClassListQattitude(Sgrade, Sclass, Cid);
                    if (dtatd.Rows.Count > 0)
                    {
                        string clmatd = "clmattitude";
                        dtstus.Columns.Add(clmatd, typeof(string));
                        GetScore(dtstus, clmatd, dtatd);
                        dtstus.Columns[clmatd].ColumnName = "课堂表现";
                        cml = cml + 1;//新增了课堂表现列
                    }
                    dtstus.Columns.Add("汇总", typeof(int));
                    dtTotal(dtstus, cml);
                }
            }
            dtstus.Columns["Snum"].ColumnName = "学号";
            dtstus.Columns["Sname"].ColumnName = "姓名";
            return dtstus;
        }
        /// <summary>
        /// 汇总
        /// </summary>
        /// <param name="dtt"></param>
        /// <param name="cml"></param>
        private void dtTotal(DataTable dtt,int cml)
        {
            int cn = dtt.Rows.Count;

            if (cn > 0)
            {
                for (int i = 0; i < cn; i++)
                {
                    int totalscores = 0;
                    for (int j = 2; j < cml; j++)
                    {
                        string cmn = dtt.Rows[i][j].ToString();
                        if (!string.IsNullOrEmpty(cmn))
                            totalscores = totalscores + Int32.Parse(cmn);
                    }
                    dtt.Rows[i][cml] = totalscores;
                }
            }
        }

        private string GetUniqueColumnName(DataColumnCollection columns, string title)
        {
            string baseName = string.IsNullOrEmpty(title) ? "未命名" : title;
            string candidate = baseName;
            int suffix = 2;

            while (columns.Contains(candidate))
            {
                candidate = baseName + "(" + suffix.ToString() + ")";
                suffix++;
            }

            return candidate;
        }
        /// <summary>
        /// 将第2张表的值赋值给第1张相同学号的指定列上
        /// </summary>
        /// <param name="dtstu"></param>
        /// <param name="clmn"></param>
        /// <param name="dtscore"></param>
        /// <returns></returns>
        private void GetScore(DataTable dtstu, string clmn, DataTable dtscore)
        {
            int scount = dtstu.Rows.Count;
            for (int i = 0; i < scount; i++)
            {
                string snum = dtstu.Rows[i]["Snum"].ToString();
                int dcount = dtscore.Rows.Count;
                for (int j = 0; j < dcount; j++)
                {
                    string xsnum = dtscore.Rows[j]["Snum"].ToString();//第2张表获取的学号字段要重命名为Snum
                    string myscore= dtscore.Rows[j]["Score"].ToString();//第2张表获取的分值字段要重命名为Score
                    if (snum == xsnum)
                    {
                        dtstu.Rows[i][clmn] = myscore;
                        break;
                    }
                }
            }            
        }

        /// <summary>
        /// 获取Cid集的所有学案列表，返回字段Cid,Cobj,Cks,Ctitle,Cclass
        /// </summary>
        /// <param name="cids"></param>
        /// <returns></returns>
        public DataSet getCidsCourses(string cids)
        {
            return dal.getCidsCourses(cids);
        }

        /// <summary>
        /// 获取上一节课的编号cid
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="Snum"></param>
        /// <returns></returns>
        public string getLastwork(int cid,string Snum)
        {
            string example = "";
            Model.Courses cs = new Model.Courses();
            cs=GetModel(cid);
            if (cs != null)
            {
                int ks = cs.Cks.Value - 1;
                int hid = cs.Chid.Value;
                int term = cs.Cterm.Value;
                int grade = cs.Cobj.Value;
                int lastcid = dal.GetKsCid(hid, grade, term, ks);//获取上一节课的编号cid
                if (lastcid > 0)
                {
                    BLL.Mission mbll = new Mission();
                    int mid = mbll.GetMissionMid(lastcid);
                    if (mid > 0)
                    {
                        BLL.Works wbll = new Works();
                        example = wbll.GetWorkWurla(mid, Snum);
                    }
                }
            }


            return example;
        }


        /// <summary>
        /// 获取上一节课作品
        /// </summary>
        /// <param name="Cid"></param>
        /// <param name="Wnum"></param>
        /// <param name="Wtype"></param>
        /// <returns></returns>
        public string getPrework(int Cid, string Wnum,string Wtype)
        {
            return dal.getPrework(Cid, Wnum, Wtype);
        }

		/// <summary>
        /// 获取课程进度数据
        /// </summary>
        /// <param name="teacherId">教师ID</param>
        /// <param name="term">学期</param>
        /// <returns></returns>
        public DataTable GetLessonPregress(int teacherId, int term)
        {
            return dal.GetLessonPregress(teacherId, term);
        }

        /// <summary>
        /// 获取课程进度数据（基于skdj表的优化版本）
        /// </summary>
        /// <param name="teacherId">教师ID</param>
        /// <param name="term">学期</param>
        /// <returns></returns>
        public DataTable GetLessonPregressBySkdj(int teacherId, int term)
        {
            return dal.GetLessonPregressBySkdj(teacherId, term);
        }

        /// <summary>
        /// 根据当前时间获取教师的课程安排
        /// </summary>
        /// <param name="teacherId">教师ID</param>
        /// <param name="currentTime">当前时间</param>
        /// <param name="dayOfWeek">星期几(1-5)</param>
        /// <returns></returns>
        public DataTable GetCurrentSchedule(int teacherId, TimeSpan currentTime, int dayOfWeek)
        {
            return dal.GetCurrentSchedule(teacherId, currentTime, dayOfWeek);
        }

        /// <summary>
        /// 获取教师的下一节课安排
        /// </summary>
        /// <param name="teacherId">教师ID</param>
        /// <param name="currentTime">当前时间</param>
        /// <param name="dayOfWeek">星期几(1-5)</param>
        /// <returns></returns>
        public DataTable GetNextSchedule(int teacherId, TimeSpan currentTime, int dayOfWeek)
        {
            return dal.GetNextSchedule(teacherId, currentTime, dayOfWeek);
        }

		/// <summary>
		/// 获得数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  成员方法

        /// <summary>
        /// 获取指定学生本学期所有学案的汇总数据（包含每个学案的所有学习活动）
        /// </summary>
        /// <param name="Sid">学生ID</param>
        /// <param name="Sgrade">年级</param>
        /// <param name="Sclass">班级</param>
        /// <param name="Cterm">学期</param>
        /// <returns>学习汇总数据表</returns>
        public DataTable GetStudentCourseTotals(int Sid, int Sgrade, int Sclass, int Cterm)
        {
            // 获取该学生本学期的所有学案
            DataTable dtCourses = GetCoursesByGradeTerm(Sgrade, Cterm);

            if (dtCourses == null || dtCourses.Rows.Count == 0)
            {
                return new DataTable();
	}

            // 创建结果表
            DataTable dtResult = new DataTable();
            dtResult.Columns.Add("课时", typeof(int));
            dtResult.Columns.Add("学习内容", typeof(string));

            // 遍历每个学案
            foreach (DataRow drCourse in dtCourses.Rows)
            {
                int Cid = Convert.ToInt32(drCourse["Cid"]);
                string Ctitle = drCourse["Ctitle"].ToString();
                int Cks = Convert.ToInt32(drCourse["Cks"]);

                // 获取该学案的所有学习活动
                ListMenu lbll = new ListMenu();
                DataTable dtMenu = lbll.GetShowedMenu(Cid).Tables[0];

                if (dtMenu != null && dtMenu.Rows.Count > 0)
                {
                    // 添加该学案的每个学习活动
                    foreach (DataRow drMenu in dtMenu.Rows)
                    {
                        string Ltype = drMenu["Ltype"].ToString();
                        int Lxid = Convert.ToInt32(drMenu["Lxid"]);
                        string Ltitle = drMenu["Ltitle"].ToString();

                        DataRow dr = dtResult.NewRow();
                        dr["课时"] = Cks;
                        dr["学习内容"] = Ctitle + " - " + Ltitle;
                        dtResult.Rows.Add(dr);
}
                }
                else
                {
                    // 如果没有学习活动，只显示学案标题
                    DataRow dr = dtResult.NewRow();
                    dr["课时"] = Cks;
                    dr["学习内容"] = Ctitle;
                    dtResult.Rows.Add(dr);
                }
            }

            return dtResult;
        }

        /// <summary>
        /// 获取指定年级学期的所有学案
        /// </summary>
        public DataTable GetCoursesByGradeTerm(int Sgrade, int Cterm)
        {
            return dal.GetCoursesByGradeTerm(Sgrade, Cterm);
        }
	}
}

