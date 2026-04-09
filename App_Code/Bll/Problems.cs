using System;
using System.Data;
using System.Collections.Generic;
using LearnSite.Common;
using LearnSite.Model;
namespace LearnSite.BLL
{
	/// <summary>
	/// Problems
	/// </summary>
	public partial class Problems
	{
		private readonly LearnSite.DAL.Problems dal=new LearnSite.DAL.Problems();
		public Problems()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}
        public void init()
        {
            dal.init();
        }
        public void initCid()
        {
            dal.initCid();
        }
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Pid)
		{
			return dal.Exists(Pid);
		}
        //更新序号
        public bool updatePsort(int Pid, bool way)
        {
            return dal.updatePsort(Pid, way);
        }
        
        /// <summary>
        /// 初始化序号
        /// </summary>
        /// <param name="Lcid"></param>
        public void Psortnew(int Pnid)
        {
            dal.Psortnew(Pnid);
        }
        /// <summary>
        /// 统计记录数
        /// </summary>
        public int Pcount(int Pnid)
        {
            return dal.Pcount(Pnid);
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LearnSite.Model.Problems model)
		{
			return dal.Add(model);
		}
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool UpdateProblem(LearnSite.Model.Problems model)
        {
            return dal.UpdateProblem(model);
        }
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LearnSite.Model.Problems model)
		{
			return dal.Update(model);
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
        public LearnSite.Model.Problems GetModel(DataTable dt, int Tsort)
        {
            return dal.GetModel(dt,Tsort);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LearnSite.Model.Problems GetModel(int Pid)
		{
			
			return dal.GetModel(Pid);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public LearnSite.Model.Problems GetModelByCache(int Pid)
		{
			
			string CacheKey = "ProblemsModel-" + Pid;
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
			return (LearnSite.Model.Problems)objModel;
		}

        public string GetListJson(int Nid)
        {
            DataTable dt = GetListNid(Nid).Tables[0];
            int len = dt.Rows.Count;
            for (int i = 0; i < len; i++)
            {
                string ptitle = dt.Rows[i]["Ptitle"].ToString();
                dt.Rows[i]["Ptitle"] = System.Web.HttpUtility.HtmlDecode(ptitle);
            }

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
            return json;
        }
                
        /// <summary>
        /// 获得Nid数据列表
        /// Pid
        /// </summary>
        public DataTable GetListNidTable(int Nid)
        {
            return dal.GetListNidTable(Nid);
        }
        /// <summary>
        /// 获得Nid数据列表
        /// </summary>
        public DataSet GetListNid(int Nid)
        {
            return dal.GetListNid(Nid);
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
		public List<LearnSite.Model.Problems> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LearnSite.Model.Problems> DataTableToList(DataTable dt)
		{
			return BllDataTableMappers.MapProblemsList(dt);
		}
                
        /// <summary>
        /// 获得Nid数据列表
        /// </summary>
        public DataTable GetListNidSid(int Nid, int Sid)
        {
            return dal.GetListNidSid(Nid, Sid);
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
