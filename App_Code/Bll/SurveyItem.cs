using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using LearnSite.Common;
using LearnSite.Model;
namespace LearnSite.BLL
{
	/// <summary>
	/// SurveyItem
	/// </summary>
	public partial class SurveyItem
	{
		private readonly LearnSite.DAL.SurveyItem dal=new LearnSite.DAL.SurveyItem();
		public SurveyItem()
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
		public bool Exists(int Mid)
		{
			return dal.Exists(Mid);
		}                
        /// <summary>
        /// 是否存在该调查题Mqid的选项记录
        /// </summary>
        public bool ExistsMqid(int Mqid)
        {
            return dal.ExistsMqid(Mqid);
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LearnSite.Model.SurveyItem model)
		{
			return dal.Add(model);
		}  
                
        /// <summary>
        /// 更新一条选项数据Mblack
        /// </summary>
        public bool UpdateItemMblack(int Mqid, bool Mblack)
        {
            return dal.UpdateItemMblack(Mqid, Mblack);
        }
        /// <summary>
        /// 更新一条选项数据
        /// </summary>
        public bool UpdateItem(int Mid, string Mitem, int Mscore)
        {
            return dal.UpdateItem(Mid, Mitem, Mscore);
        }
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LearnSite.Model.SurveyItem model)
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
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string Midlist )
		{
			return dal.DeleteList(Midlist );
		}
                
        /// <summary>
        /// 删除该试题下的所有选项
        /// </summary>
        /// <param name="Mqid"></param>
        public void DelAllMqid(int Mqid)
        {
            dal.DelAllMqid(Mqid);
        }
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LearnSite.Model.SurveyItem GetModel(int Mid)
		{
			
			return dal.GetModel(Mid);
		}
                
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LearnSite.Model.SurveyItem GetModel(DataTable dt, int Tsort)
        {
            return dal.GetModel(dt, Tsort);
        }
		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public LearnSite.Model.SurveyItem GetModelByCache(int Mid)
		{
			
			string CacheKey = "SurveyItemModel-" + Mid;
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
			return (LearnSite.Model.SurveyItem)objModel;
		}
                
        /// <summary>
        /// 获得数据列表Mid,Mitem，随机排序
        /// </summary>
        public DataSet GetListItem(int Mqid)
        {
            return dal.GetListItem(Mqid);
        }

        /// <summary>
        /// 获得数据列表Mid,Mitem，随机排序
        /// </summary>
        public DataTable GetListItemDataTable(int Mqid)
        {
            DataTable dt = dal.GetListItem(Mqid).Tables[0];
            int count = dt.Rows.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    dt.Rows[i]["Mitem"] = HttpContext.Current.Server.HtmlDecode(dt.Rows[i]["Mitem"].ToString());
                }            
            }

            return dt;
        }
        /// <summary>
        /// 获得数据列表Mid,Mitem，顺序排序
        /// </summary>
        public DataTable GetListItemBlackDataTable(int Mqid)
        {
            DataTable dt = dal.GetListItemBlack(Mqid).Tables[0];
            return dt;
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetListByMqid(int Mqid)
        {
            string strWhere = " Mqid="+Mqid+" order by Mid asc";
            return GetList(strWhere);
        }
                
        /// <summary>
        /// 获得数据列表Mid,Mitem,Mcount，按Mid排序
        /// </summary>
        public DataTable GetListItemByMvid(int Mvid)
        {
            return dal.GetListItemByMvid(Mvid);
        }
                
        /// <summary>
        /// 返回调查试题所有选项和选中次数selectStr | countStr
        /// </summary>
        /// <param name="Mvid"></param>
        /// <param name="Allselect"></param>
        /// <returns></returns>
        public string GetListItemAndCount(int Mvid, string Allselect)
        {
            return dal.GetListItemAndCount(Mvid, Allselect);
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
		public List<LearnSite.Model.SurveyItem> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LearnSite.Model.SurveyItem> DataTableToList(DataTable dt)
		{
			return BllDataTableMappers.MapSurveyItemList(dt);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}
                         
        /// <summary>
        /// 读取本测验的所有分值
        /// </summary>
        public int GetItemAllScore(int Mvid)
        {
            return dal.GetItemAllScore(Mvid);
        }
        /// <summary>
        /// 统计选中记录分值
        /// </summary>
        public int GetItemScore(ArrayList fselect)
        {
            return dal.GetItemScore(fselect);
        }
                
        /// <summary>
        /// 统计选中记录分值
        /// </summary>
        public string[] GetItembbScore(ArrayList bbtext, int Mvid)
        {
            return dal.GetItembbScore(bbtext, Mvid);
        }
        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetItemCount(int Mqid)
        {
            return dal.GetItemCount(Mqid);
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
        /// 获取选项内容
        /// </summary>
        /// <param name="Mid"></param>
        /// <returns></returns>
        public string GetMitem(int Mid)
        {
            return dal.GetMitem(Mid);
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
