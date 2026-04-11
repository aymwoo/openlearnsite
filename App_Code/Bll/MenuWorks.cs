using System;
using System.Data;
using System.Collections.Generic;
using LearnSite.Common;
using LearnSite.Model;
using System.Collections;
namespace LearnSite.BLL
{
	/// <summary>
	/// MenuWorks
	/// </summary>
	public partial class MenuWorks
	{
		private readonly LearnSite.DAL.MenuWorks dal=new LearnSite.DAL.MenuWorks();
		public MenuWorks()
		{}
		#region  BasicMethod

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
		public bool Exists(int Kid)
		{
			return dal.Exists(Kid);
		}
        
        
                
        /// <summary>
        /// 阅读任务标记为已学
        /// </summary>
        /// <param name="Ksid"></param>
        /// <returns></returns>
        public string readCids(int Ksid)
        {
            return dal.readCids(Ksid);
        }
        /// <summary>
        /// 花费时间
        /// </summary>
        public int SpendTime(int Ksid, int klid)
        {
            return dal.SpendTime(Ksid, klid);
        }

        public int SpendSeconds(int Ksid, int klid)
        {
            return dal.SpendSeconds(Ksid, klid);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Ksid, int klid)
        {
            return dal.Exists(Ksid,klid);
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(LearnSite.Model.MenuWorks model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 确保每个学生每个活动只有一条完成记录
		/// </summary>
		public bool EnsureCompletion(LearnSite.Model.MenuWorks model)
		{
			if (model == null || !model.Ksid.HasValue || !model.Klid.HasValue)
			{
				return false;
			}

			return Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LearnSite.Model.MenuWorks model)
		{
			return dal.Update(model);
		}
                
        /// <summary>
        /// 删除指定用户的任务记录
        /// </summary>
        public bool DeleteMenuWork(int Ksid, int klid)
        {
            return dal.DeleteMenuWork(Ksid, klid);
        }
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int Kid)
		{
			
			return dal.Delete(Kid);
		}

                
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LearnSite.Model.MenuWorks GetModelme(int Sid, int Klid)
        {
            return dal.GetModelme(Sid,Klid);
        }
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LearnSite.Model.MenuWorks GetModel(int Kid)
		{
			
			return dal.GetModel(Kid);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public LearnSite.Model.MenuWorks GetModelByCache(int Kid)
		{
			
			string CacheKey = "MenuWorksModel-" + Kid;
            object objModel = LearnSite.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(Kid);
					if (objModel != null)
					{
                        int ModelCache = LearnSite.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LearnSite.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (LearnSite.Model.MenuWorks)objModel;
		}
                
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public ArrayList GetMyListLid(int Ksid)
        {
            return dal.GetMyListLid(Ksid);
        }
                
        /// <summary>
        /// 查询完成的活动记录数
        /// </summary>
        /// <param name="Ksid"></param>
        /// <param name="lidall"></param>
        /// <returns></returns>
        public int GetMyLidCount(int Ksid, string lidall)
        {
            return dal.GetMyLidCount(Ksid, lidall);
        }
                
        /// <summary>
        /// 查询完成的活动记录数
        /// </summary>
        /// <param name="Ksid"></param>
        /// <param name="lidall"></param>
        /// <returns></returns>
        public ArrayList GetMyLids(int Ksid, string lidall)
        {
            return dal.GetMyLids(Ksid, lidall);
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
		public List<LearnSite.Model.MenuWorks> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LearnSite.Model.MenuWorks> DataTableToList(DataTable dt)
		{
			return BllDataTableMappers.MapMenuWorksList(dt);
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

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}
