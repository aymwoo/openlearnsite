using System;
using System.Data;
using System.Collections.Generic;
using LearnSite.Model;
namespace LearnSite.BLL
{
	/// <summary>
	/// DelStudents
	/// </summary>
	public partial class DelStudents
	{
		private readonly LearnSite.DAL.DelStudents dal=new LearnSite.DAL.DelStudents();
		public DelStudents()
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
		public bool Exists(int Did)
		{
			return dal.Exists(Did);
		}
        /// <summary>
        /// 获取删除列表中不存在的新学号
        /// </summary>
        /// <param name="Snum"></param>
        /// <returns></returns>
        public string GetNewSnum(long Snum)
        {
            while (dal.ExistsDnum(Snum.ToString()))
            {
                Snum++;//如果存在，则增加，继续循环，直至不存在！
            }
            return Snum.ToString();
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LearnSite.Model.DelStudents model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LearnSite.Model.DelStudents model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int Did)
		{
			
			return dal.Delete(Did);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string Didlist )
		{
			return dal.DeleteList(Didlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LearnSite.Model.DelStudents GetModel(int Did)
		{
			
			return dal.GetModel(Did);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public LearnSite.Model.DelStudents GetModelByCache(int Did)
		{
			
			string CacheKey = "DelStudentsModel-" + Did;
            object objModel = LearnSite.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(Did);
					if (objModel != null)
					{
                        int ModelCache = LearnSite.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LearnSite.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (LearnSite.Model.DelStudents)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
                
        /// <summary>
        /// 获得所教班级的已删除学生数据列表
        /// </summary>
        public DataSet GetListLimit(int Dgrade,int Dclass)
        {
            string strWhereIs = " Dgrade="+Dgrade+" and Dclass="+Dclass;
            return dal.GetList(strWhereIs);
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
		public List<LearnSite.Model.DelStudents> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LearnSite.Model.DelStudents> DataTableToList(DataTable dt)
		{
			return BllDataTableMappers.MapDelStudentsList(dt);
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
