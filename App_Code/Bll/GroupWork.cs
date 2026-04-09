using System;
using System.Data;
using System.Collections.Generic;
using LearnSite.Model;
namespace LearnSite.BLL
{
	/// <summary>
	/// 业务逻辑类GroupWork 的摘要说明。
	/// </summary>
	public class GroupWork
	{
		private readonly LearnSite.DAL.GroupWork dal=new LearnSite.DAL.GroupWork();
		public GroupWork()
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
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Gid)
		{
			return dal.Exists(Gid);
		}
        /// <summary>
        /// 是否存在小组作品
        /// </summary>
        /// <param name="Gnum"></param>
        /// <param name="Gmid"></param>
        /// <returns></returns>
        public bool Exists(string Gnum, int Gmid)
        {
            return dal.Exists(Gnum, Gmid);
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LearnSite.Model.GroupWork model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LearnSite.Model.GroupWork model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int Gid)
		{
			
			dal.Delete(Gid);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LearnSite.Model.GroupWork GetModel(int Gid)
		{
			
			return dal.GetModel(Gid);
		}
        /// <summary>
        /// 根据组长学号和活动编号得到一个对象实体
        /// </summary>
        public LearnSite.Model.GroupWork GetModelBySnum(string Gnum, int Gmid)
        {
            return dal.GetModelBySnum(Gnum, Gmid);
        }
		/// <summary>
		/// 得到一个对象实体，从缓存中。
		/// </summary>
		public LearnSite.Model.GroupWork GetModelByCache(int Gid)
		{
			
			string CacheKey = "GroupWorkModel-" + Gid;
            object objModel = LearnSite.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(Gid);
					if (objModel != null)
					{
                        int ModelCache = LearnSite.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LearnSite.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (LearnSite.Model.GroupWork)objModel;
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
		public List<LearnSite.Model.GroupWork> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LearnSite.Model.GroupWork> DataTableToList(DataTable dt)
		{
			return BllDataTableMappers.MapGroupWorkList(dt);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}
               
        /// <summary>
        /// 获取自己参与小组的合作作品
        /// </summary>
        /// <param name="Snum"></param>
        /// <returns></returns>
        public DataSet GetMyWorks(string Snum)
        {
            return dal.GetMyWorks(Snum);
        }
                
        /// <summary>
        /// 获取该学号组长提交作品的是否存在
        /// </summary>
        /// <param name="Snum"></param>
        /// <param name="Gmid"></param>
        /// <returns></returns>
        public bool DoneGroupWork(string Snum, int Gmid)
        {
            return dal.DoneGroupWork(Snum, Gmid);
        }
                 
        /// <summary>
        /// 获取该组号提交作品的是否存在
        /// </summary>
        /// <param name="Ggroup"></param>
        /// <param name="Gmid"></param>
        /// <returns></returns>
        public bool DoneGroupWork(int Ggroup, int Gmid)
        {
            return dal.DoneGroupWork(Ggroup, Gmid);
        }
        /// <summary>
        /// 获取该学号组长提交作品的是否存在
        /// </summary>
        /// <param name="Snum"></param>
        /// <param name="Gmid"></param>
        /// <returns></returns>
        public string DoneGroupWorkUrl(string Snum, int Gmid)
        {
            return dal.DoneGroupWorkUrl(Snum, Gmid);
        }
        /// <summary>
        /// 判断作品是否评价
        /// </summary>
        /// <param name="Snum"></param>
        /// <param name="Gmid"></param>
        /// <returns></returns>
        public bool CheckGroupWork(string Snum, int Gmid)
        {
            return dal.CheckGroupWork(Snum, Gmid);
        }
        /// <summary>
        /// 给小组作品评分
        /// </summary>
        /// <param name="Gid"></param>
        /// <param name="Gscore"></param>
        public void UpdateGscore(int Gid, int Gscore, int Cobj, int Cterm)
        {
            dal.UpdateGscore(Gid, Gscore, Cobj, Cterm);
        }
                
        /// <summary>
        /// 给小组作品取消评价
        /// </summary>
        /// <param name="Gid"></param>
        /// <param name="Gscore"></param>
        public void CancelGscore(int Gid, bool Gcheck)
        {
            dal.CancelGscore(Gid, Gcheck);
        }
        /// <summary>
        /// 获取某班级某活动小组合作作品列表
        /// </summary>
        /// <param name="Ggrade"></param>
        /// <param name="Gclass"></param>
        /// <param name="Gmid"></param>
        /// <returns></returns>
        public DataSet GetMissionGroup(int Ggrade, int Gclass, int Gmid)
        {
            return dal.GetMissionGroup(Ggrade, Gclass, Gmid);
        }
               
        /// <summary>
        /// 获取本课程小组作品总分
        /// </summary>
        /// <param name="Gnum"></param>
        /// <param name="Gcid"></param>
        /// <returns></returns>
        public int GetGscore(string Gnum, int Gcid)
        {
            return dal.GetGscore(Gnum, Gcid);
        }
         /// <summary>
        /// 获取本课小组作品总分
        /// </summary>
        /// <param name="Ggroup">组号</param>
        /// <param name="Gcid">学案编号</param>
        /// <returns></returns>
        public int GetGscore(int Ggroup, int Gcid)
        {
            return dal.GetGscore(Ggroup, Gcid);
        }
        /// <summary>
        /// 获取本班本学期本小组的合作作品总分
        /// </summary>
        /// <param name="Ggroup">组号</param>
        /// <param name="Ggrade"></param>
        /// <param name="Gclass"></param>
        /// <param name="Gterm"></param>
        /// <returns></returns>
        public int GetGscoreAll(int Ggroup, int Ggrade, int Gclass, int Gterm)
        {
            return dal.GetGscoreAll(Ggroup, Ggrade, Gclass, Gterm);
        }
                
        /// <summary>
        /// 初始化新增字段Ggroup 组号（小组作品表）
        /// </summary>
        /// <returns></returns>
        public int initGgroup()
        {
            return dal.initGgroup();
        }               
        /// <summary>
        /// 初始化新增字段Gyear 组长入学年度（小组作品表）
        /// </summary>
        /// <returns></returns>
        public int initGyear()
        {
            return dal.initGyear();
        }
		/// <summary>
		/// 获得数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  成员方法
	}
}
