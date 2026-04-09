using System;
using System.Data;
using System.Collections.Generic;
using LearnSite.Model;
using System.Web;
namespace LearnSite.BLL
{
	/// <summary>
	/// Pfinger
	/// </summary>
	public class Pfinger
	{
		private readonly LearnSite.DAL.Pfinger dal=new LearnSite.DAL.Pfinger();
		public Pfinger()
		{}
		#region  Method

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
		public bool Exists(int Pid)
		{
			return dal.Exists(Pid);
		}
               
        /// <summary>
        /// 返回该学号指法成绩
        /// </summary>
        public string GetPsnum(string Psnum)
        {
            return dal.GetPsnum(Psnum);
        }
        /// <summary>
        /// 是否存在本月该学号指法记录，返回Pid
        /// </summary>
        public string ExistsMonth(string Psnum, int Pyear, int Pmonth)
        {
            return dal.ExistsMonth(Psnum, Pyear, Pmonth);
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LearnSite.Model.Pfinger model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LearnSite.Model.Pfinger model)
		{
			return dal.Update(model);
		}
        /// <summary>
        /// 清空表
        /// </summary>
        public int ClearTb()
        {
           return  dal.ClearTb();
        }        
        /// <summary>
        /// 清空所教班级的学生指法记录
        /// </summary>
        public void ClearMyTb(int Rhid)
        {
            dal.ClearMyTb(Rhid);
        }
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int Pid)
		{
			
			return dal.Delete(Pid);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string Pidlist )
		{
			return dal.DeleteList(Pidlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LearnSite.Model.Pfinger GetModel(int Pid)
		{
			
			return dal.GetModel(Pid);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public LearnSite.Model.Pfinger GetModelByCache(int Pid)
		{
			
			string CacheKey = "PfingerModel-" + Pid;
            object objModel = LearnSite.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(Pid);
					if (objModel != null)
					{
                        int ModelCache = LearnSite.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LearnSite.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (LearnSite.Model.Pfinger)objModel;
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
		public List<LearnSite.Model.Pfinger> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LearnSite.Model.Pfinger> DataTableToList(DataTable dt)
		{
			return BllDataTableMappers.MapPfingerList(dt);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}                
        /// <summary>
        /// 显示班级指法英雄榜记录
        /// </summary>
        /// <param name="GVtyper"></param>
        public DataSet ShowClassFingerScore(int Sgrade, int Sclass)
        {
            return dal.ShowClassFingerScore(Sgrade, Sclass);
        }
        /// <summary>
        /// 显示年级段指法英雄榜记录
        /// </summary>
        /// <param name="GVtyper"></param>
        public DataSet ShowAllFingerScore(int Sgrade)
        {
            return dal.ShowAllFingerScore(Sgrade);
        }                
        /// <summary>
        /// 显示学校指法英雄榜记录
        /// </summary>
        /// <param name="GVtyper"></param>
        public DataSet ShowSchoolFingerScore()
        {
            return dal.ShowSchoolFingerScore();
        }
        /// <summary>
        /// 保存成绩
        /// </summary>
        /// <param name="psnum"></param>
        /// <param name="myspd"></param>
        /// <returns></returns>
        public bool saveSpd(string myspd)
        {
            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
            if (cook!= null)
            {
                return dal.saveSpd(cook.Snum, myspd,cook.Sgrade);
            }
            else
                return false;
        }
                
        /// <summary>
        /// 显示年级段指法英雄榜记录
        /// </summary>
        /// <param name="GVtyper"></param>
        public DataSet ShowTopFingerScore(int Sgrade, int nTop)
        {
            return dal.ShowTopFingerScore(Sgrade, nTop);
        }
                
        /// <summary>
        /// 显示年级段指法英雄榜记录
        /// </summary>
        /// <param name="GVtyper"></param>
        public DataSet ShowTopFingerScoreAs(int Sgrade, int nTop)
        {
            return dal.ShowTopFingerScoreAs(Sgrade, nTop);
        }
                
        /// <summary>
        /// 显示学校指法英雄榜记录
        /// </summary>
        /// <param name="GVtyper"></param>
        public DataSet ShowSchoolTopFingerScore(int nTop)
        {
            return dal.ShowSchoolTopFingerScore(nTop);
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
