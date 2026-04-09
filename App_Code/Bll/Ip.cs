using System;
using System.Data;
using System.Collections.Generic;
using LearnSite.Common;
using LearnSite.Model;
namespace LearnSite.BLL
{
	/// <summary>
	/// Ip
	/// </summary>
	public partial class Ip
	{
		private readonly LearnSite.DAL.Ip dal=new LearnSite.DAL.Ip();
		public Ip()
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
		public bool Exists(int Iid)
		{
			return dal.Exists(Iid);
		}

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsIp(int Ihid, string Iip)
        {
            return dal.ExistsIp(Ihid, Iip);
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LearnSite.Model.Ip model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LearnSite.Model.Ip model)
		{
			return dal.Update(model);
		}

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool UpdateIip(string Iip, int Iid)
        {
            return dal.UpdateIip(Iip, Iid);
        }
        /// <summary>
        /// 删除该机房所有IP记录
        /// </summary>
        public bool DeleteIhid(int Ihid)
        {
            return dal.DeleteIhid(Ihid);
        }
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int Iid)
		{

			return dal.Delete(Iid);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string Iidlist )
		{
			return dal.DeleteList(Iidlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LearnSite.Model.Ip GetModel(int Iid)
		{

			return dal.GetModel(Iid);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public LearnSite.Model.Ip GetModelByCache(int Iid)
		{

			string CacheKey = "IpModel-" + Iid;
            object objModel = LearnSite.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(Iid);
					if (objModel != null)
					{
                        int ModelCache = LearnSite.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LearnSite.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (LearnSite.Model.Ip)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
        public DataTable GetHouseIp(int Ihid)
        {
            string strWhere = " Ihid=" + Ihid + " order by Inum asc";
            return GetList(strWhere).Tables[0];
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
		public List<LearnSite.Model.Ip> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LearnSite.Model.Ip> DataTableToList(DataTable dt)
		{
			return BllDataTableMappers.MapIpList(dt);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

        /// <summary>
        /// 获取本班当天签到同学的 Qnum,Qname,Inum
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public DataTable GetSiginStudents(int Sgrade, int Sclass, int Ihid)
        {
            return dal.GetSiginStudents(Sgrade, Sclass, Ihid);
        }

        /// <summary>
        /// 获取本班当天签到同学的Inum- Qnum- Qname-phototype| 形式的字符串
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public string GetSiginStudentStr(int Sgrade, int Sclass, int Ihid, bool isshow)
        {
            return dal.GetSiginStudentStr(Sgrade, Sclass, Ihid,isshow);
        }

		/// <summary>
        /// 根据IP地址和机房ID获取机号
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="ihid">机房ID</param>
        /// <returns>机号，如果未找到返回空字符串</returns>
        public string GetInumByIpAndHid(string ip, int ihid)
        {
            return dal.GetInumByIpAndHid(ip, ihid);
        }

        /// <summary>
        /// 根据IP地址获取机号（自动识别机房）
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns>机号，如果未找到返回空字符串</returns>
        public string GetInumByIp(string ip)
        {
            return dal.GetInumByIp(ip);
        }

        /// <summary>
        /// 根据机房ID获取该机房的最大机号
        /// </summary>
        /// <param name="ihid">机房ID</param>
        /// <returns>最大机号</returns>
        public int GetMaxInumByHid(int ihid)
        {
            return dal.GetMaxInumByHid(ihid);
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
