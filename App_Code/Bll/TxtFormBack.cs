
using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using LearnSite.Common;
using LearnSite.Model;
using System.Web;
namespace LearnSite.BLL
{
	/// <summary>
	/// TxtFormBack
	/// </summary>
	public partial class TxtFormBack
	{
		private readonly LearnSite.DAL.TxtFormBack dal=new LearnSite.DAL.TxtFormBack();
		public TxtFormBack()
		{}
		#region  BasicMethod

        /// <summary>
        ///判断是否已提交
        /// </summary>
        /// <param name="Rsid"></param>
        /// <param name="Rmid"></param>
        /// <returns></returns>
        public int GetRid(string Rsid, string Rmid)
        {
            return dal.GetRid(Rsid, Rmid);
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LearnSite.Model.TxtFormBack model)
		{
			return dal.Add(model);
		}

        public void UpdateAllScore(string Ryear, string Rgrade, string Rclass, string Rmid, int Rscore)
        {
            dal.UpdateAllScore(Ryear, Rgrade, Rclass, Rmid, Rscore);
        }
        public void UpdateScore(int Rid, int Rscore)
        {
            dal.UpdateScore(Rid, Rscore);
        }
        public void UpdateAgree(int Rid)
        {
            dal.UpdateAgree(Rid);
        }
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LearnSite.Model.TxtFormBack model)
		{
			return dal.Update(model);
		}
                
        /// <summary>
        /// 更新内容 Rwords, Rtime,Rid
        /// </summary>
        public bool UpdateContent(LearnSite.Model.TxtFormBack model)
        {
            return dal.UpdateContent(model);
        }
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int Rid)
		{
			
			return dal.Delete(Rid);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LearnSite.Model.TxtFormBack GetModel(int Rid)
		{
			
			return dal.GetModel(Rid);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public LearnSite.Model.TxtFormBack GetModelByCache(int Rid)
		{
			
			string CacheKey = "TxtFormBackModel-" + Rid;
			object objModel = LearnSite.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(Rid);
					if (objModel != null)
					{
						int ModelCache = LearnSite.Common.ConfigHelper.GetConfigInt("ModelCache");
						LearnSite.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (LearnSite.Model.TxtFormBack)objModel;
		}

         /// <summary>
        /// 获得该表单本班数据列表
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Rmid">对应表单的自动编号Mid</param>
        /// <returns></returns>
        public DataTable GetClassTxtFormScore(int Sgrade, int Sclass, int Rmid)
        {
            return dal.GetClassTxtFormScore(Sgrade, Sclass, Rmid);
        }

        /// <summary>
        /// 获取该班级的表单列表
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        /// <param name="Rmid"></param>
        /// <returns></returns>
        public DataSet GetListclass(string Rgrade, string Rclass, string Rmid, int Sgroup, int Sid)
        {
            return dal.GetListclass(Rgrade, Rclass, Rmid,Sgroup,Sid);
        }

        public string GetUndoStus(int Rgrade, int Rclass, int Rmid)
        {
            StringBuilder stus = new StringBuilder("未填写同学：");
            DataTable dt = dal.GetUndo(Rgrade, Rclass, Rmid);
            int count = dt.Rows.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    stus.Append(dt.Rows[i][0].ToString());
                    if (i < count - 1)
                        stus.Append("，");
                }
            }
            return stus.ToString();
        }
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
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
		public List<LearnSite.Model.TxtFormBack> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LearnSite.Model.TxtFormBack> DataTableToList(DataTable dt)
		{
			return BllDataTableMappers.MapTxtFormBackList(dt);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			return dal.GetRecordCount(strWhere);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
		}
        
                
        /// <summary>
        /// 获取当前班级学过的学案Cid
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        /// <param name="Rterm"></param>
        /// <param name="Ryear"></param>
        /// <returns></returns>
        public string ShowDoneBackCids(int Rgrade, int Rclass, int Rterm, int Ryear)
        {
            return dal.ShowDoneBackCids(Rgrade, Rclass, Rterm, Ryear);
        }
        
        /// <summary>
        /// 获取某学生学过的学案Cid
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public string ShowStuDoneBackCids(string Snum, int Rterm, int Rgrade)
        {
            return dal.ShowStuDoneBackCids(Snum, Rterm, Rgrade);
        }
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  BasicMethod
		#region  ExtensionMethod
        /// <summary>
        /// 保存表单数据
        /// </summary>
        /// <returns></returns>
        public string SaveFormContent()
        {
            string msg = "提交失败!";
            string Lid = HttpContext.Current.Request.QueryString["lid"].ToString();
            if (LearnSite.Common.WordProcess.IsNum(Lid))
            {
                LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
                LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
                lmodel = lbll.GetModel(Int32.Parse(Lid));
                string Mid = lmodel.Lxid.ToString();

                string word = HttpUtility.HtmlEncode(HttpContext.Current.Request.Form["Word"]);
                string content = HttpUtility.HtmlEncode(HttpContext.Current.Request.Form["Content"]);
                if (HttpContext.Current.Request.Cookies[Common.CookieHelp.stuCookieNname] != null)
                {
                    if (Common.WordProcess.IsNum(Mid))
                    {
                        LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
                        int Syear = cook.Syear;
                        int Sgrade = cook.Sgrade;
                        int Sclass = cook.Sclass;
                        int Wsid = cook.Sid;
                        string Wip = cook.LoginIp;
                        string Sname = cook.Sname;
                        string Snum = cook.Snum;
                        int Wterm = cook.ThisTerm;
                        string Wtime = cook.LoginTime;
                        DateTime Wdate = DateTime.Now;

                        Model.TxtFormBack rmodel = new Model.TxtFormBack();
                        BLL.TxtFormBack rbll = new BLL.TxtFormBack();
                        rmodel.Rmid = Int32.Parse(Mid);
                        rmodel.Ragree = 0;
                        rmodel.Rclass = Sclass;
                        rmodel.Rgrade = Sgrade;
                        rmodel.Rip = Wip;
                        rmodel.Rscore = 0;
                        rmodel.Rsid = Wsid;
                        rmodel.Rsnum = Snum;
                        rmodel.Rterm = Wterm;
                        rmodel.Rtime = Wdate;
                        rmodel.Rwords = word;
                        rmodel.Ryear = Syear;
                        rmodel.Rlid = Int32.Parse(Lid);
                        rmodel.Rcontent = content;

                        int Rid = rbll.GetRid(Wsid.ToString(), Mid);
                        rmodel.Rid = Rid;
                        if (Rid > 0)
                        {
                            if (rbll.UpdateContent(rmodel)) //如果提交过表单，再更新提交的内容
                                msg = "修改内容成功!";
                            else
                                msg = "老师已评价，不可修改!";
                        }
                        else
                        {
                            rbll.Add(rmodel);//否则提交表单内容

                            //添加课堂活动记录
                            Model.MenuWorks kmodel = new Model.MenuWorks();
                            kmodel.Klid = Int32.Parse(Lid);
                            kmodel.Ksid = Wsid;
                            kmodel.Ktime = LearnSite.Common.Computer.GoneMinute(DateTime.Parse(Wtime), Wdate);
                            kmodel.Kseconds = Int32.Parse(LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate));
                            kmodel.Kcheck = false;
                            BLL.MenuWorks kbll = new MenuWorks();
                            kbll.Add(kmodel);

                            msg = "提交内容成功!";
                        }
                    }
                    else
                    {
                        msg = "Mid参数格式错误!";
                    }
                }
                else
                {
                    msg = "未登录，无权限！cookies无效";
                }
            }
            return msg;        
        }

		#endregion  ExtensionMethod
	}
}
