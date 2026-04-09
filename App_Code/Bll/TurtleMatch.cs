using System;
using System.Data;
using System.Collections.Generic;
using LearnSite.Common;
using LearnSite.Model;
namespace LearnSite.BLL
{
	/// <summary>
	/// TurtleMatch
	/// </summary>
	public partial class TurtleMatch
	{
		private readonly LearnSite.DAL.TurtleMatch dal=new LearnSite.DAL.TurtleMatch();
		public TurtleMatch()
		{}
		#region  BasicMethod


                
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool IsOwer(int Mid, int Mhid)
        {
            return dal.IsOwer(Mid, Mhid);
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LearnSite.Model.TurtleMatch model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LearnSite.Model.TurtleMatch model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int Mid)
		{
			
			return dal.Delete(Mid);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LearnSite.Model.TurtleMatch GetModel(int Mid)
		{
			
			return dal.GetModel(Mid);
		}
        
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LearnSite.Model.TurtleMatch GetModel(DataTable dt, int Tsort)
        {
            return dal.GetModel(dt, Tsort);
        }
		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public LearnSite.Model.TurtleMatch GetModelByCache(int Mid)
		{
			
			string CacheKey = "TurtleMatchModel-" + Mid;
            object objModel = LearnSite.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(Mid);
					if (objModel != null)
					{
                        int ModelCache = LearnSite.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LearnSite.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (LearnSite.Model.TurtleMatch)objModel;
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
		public List<LearnSite.Model.TurtleMatch> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LearnSite.Model.TurtleMatch> DataTableToList(DataTable dt)
		{
			return BllDataTableMappers.MapTurtleMatchList(dt);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetMidList(string Mid)
        {
            return GetList(" Mid=" + Mid);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllHidList(int hid)
        {
            return GetList("  Mhid="+hid.ToString());
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
