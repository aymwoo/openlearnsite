using System;
using System.Data;
using System.Collections.Generic;
using LearnSite.Common;
using LearnSite.Model;
namespace LearnSite.BLL
{
	/// <summary>
	/// Solves
	/// </summary>
	public partial class Solves
	{
		private readonly LearnSite.DAL.Solves dal=new LearnSite.DAL.Solves();
		public Solves()
		{}
		#region  Method

        public void updateclass()
        {
            dal.updateclass();
        }

        public void updatescore()
        {
            dal.updatescore();
        }

        public void init()
        {
            dal.init();
        }

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Vid)
		{
			return dal.Exists(Vid);
		}
        /// <summary>
        /// 保存测评成绩
        /// </summary>
        /// <param name="Vpid"></param>
        /// <param name="Vsid"></param>
        /// <param name="Vscore"></param>
        /// <param name="Vanswer"></param>
        /// <returns></returns>
        public bool SaveScore(int Vpid, int Vscore, string Vanswer, int Vnid, int Vcid, int Vlid)
        {
            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
            DateTime todate = DateTime.Now;
            Model.Solves model = new Model.Solves();
            model.Vanswer = Vanswer;
            model.Vdate = todate;
            model.Vpid = Vpid;
            model.Vright = true;
            model.Vscore = Vscore;
            model.Vsid = cook.Sid;
            model.Vgrade = cook.Sgrade;
            model.Vclass = cook.Sclass;
            model.Vterm = cook.ThisTerm;
            model.Vyear = cook.Syear;
            model.Vnid = Vnid;
            model.Vcid = Vcid;

            if (!ExistsPidSid(Vpid, cook.Sid))
            {
                Add(model);

                //添加课堂活动记录
                LearnSite.Model.MenuWorks kmodel = new LearnSite.Model.MenuWorks();
                kmodel.Klid = Vlid;
                kmodel.Ksid = cook.Sid;
                kmodel.Ktime = LearnSite.Common.Computer.GoneMinute(DateTime.Parse(cook.LoginTime), todate);
                kmodel.Kseconds = Int32.Parse(LearnSite.Common.Computer.Datagone(DateTime.Parse(cook.LoginTime), todate));
                kmodel.Kcheck = false;
                LearnSite.BLL.MenuWorks kbll = new LearnSite.BLL.MenuWorks();
                if (!kbll.Exists(cook.Sid, Vlid))
                {
                    kbll.Add(kmodel);
                }
                return true;
            }
            else
            {
                return false;
            }
        }   

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public string GetScore(int Vpid, int Vsid)
        {
            return dal.GetScore(Vpid, Vsid);
        }
        public DataTable GetClassListScore(int Sgrade, int Sclass, int Vnid)
        {
            return dal.GetClassListScore(Sgrade, Sclass, Vnid);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsPidSid(int Vpid, int Vsid)
        {
            return dal.ExistsPidSid(Vpid, Vsid);
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LearnSite.Model.Solves model)
		{
			return dal.Add(model);
		}
         
                
        /// <summary>
        /// 同步
        /// </summary>
        public void UpdateVgradeVterm()
        {
            dal.UpdateVgradeVterm();
        }
        /// <summary>
        /// 更新并返回本学期测评成绩
        /// </summary>
        /// <param name="Vsid"></param>
        /// <param name="Vgrade"></param>
        /// <param name="Vterm"></param>
        public string UpdateSidle(string Vsid, string Vyear, string Vgrade, string Vclass, string Vterm) 
        {
            return dal.UpdateSidle(Vsid,Vyear,Vgrade,Vclass,Vterm);
        }
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LearnSite.Model.Solves model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int Vid)
		{
			
			return dal.Delete(Vid);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string Vidlist )
		{
			return dal.DeleteList(Vidlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LearnSite.Model.Solves GetModel(int Vid)
		{
			
			return dal.GetModel(Vid);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public LearnSite.Model.Solves GetModelByCache(int Vid)
		{
			
			string CacheKey = "SolvesModel-" + Vid;
			object objModel = LearnSite.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(Vid);
					if (objModel != null)
					{
						int ModelCache = LearnSite.Common.ConfigHelper.GetConfigInt("ModelCache");
						LearnSite.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (LearnSite.Model.Solves)objModel;
		}

        public string ShowDoneSovleCids(string Rgrade, string Rclass, string Wterm, string Syear)
        {
            return dal.ShowDoneSovleCids(Rgrade, Rclass, Wterm, Syear);
        }


        public string ShowStuDoneSovleCids(string Sid, string Rterm, string Rgrade)
        {
            return dal.ShowStuDoneSovleCids(Sid, Rterm, Rgrade);
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
		public List<LearnSite.Model.Solves> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LearnSite.Model.Solves> DataTableToList(DataTable dt)
		{
			return BllDataTableMappers.MapSolvesList(dt);
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
		/// 分页获取数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  Method
	}
}
