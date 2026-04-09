using System;
using System.Data;
using System.Collections.Generic;
using LearnSite.Model;
namespace LearnSite.BLL
{
	/// <summary>
	/// Summary
	/// </summary>
	public partial class Summary
	{
		private readonly LearnSite.DAL.Summary dal=new LearnSite.DAL.Summary();
		public Summary()
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
		public bool Exists(int Sid)
		{
			return dal.Exists(Sid);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LearnSite.Model.Summary model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LearnSite.Model.Summary model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int Sid)
		{
			
			return dal.Delete(Sid);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string Sidlist )
		{
			return dal.DeleteList(Sidlist );
		}
                        
        /// <summary>
        /// 得到指定活动内容的总结一个对象实体
        /// </summary>
        public LearnSite.Model.Summary GetModelByClass(int Scid, int Shid, int Sgrade, int Sclass)
        {
            return dal.GetModelByClass(Scid, Shid, Sgrade, Sclass);
        }
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LearnSite.Model.Summary GetModel(int Sid)
		{
			
			return dal.GetModel(Sid);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public LearnSite.Model.Summary GetModelByCache(int Sid)
		{
			
			string CacheKey = "SummaryModel-" + Sid;
            object objModel = LearnSite.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(Sid);
					if (objModel != null)
					{
                        int ModelCache = LearnSite.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LearnSite.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (LearnSite.Model.Summary)objModel;
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
		public List<LearnSite.Model.Summary> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LearnSite.Model.Summary> DataTableToList(DataTable dt)
		{
			return BllDataTableMappers.MapSummaryList(dt);
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
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  Method
	}
}
