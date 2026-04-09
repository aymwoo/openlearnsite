using System;
using System.Data;
using System.Collections.Generic;
using LearnSite.Model;
namespace LearnSite.BLL
{
    /// <summary>
    /// AIProvider
    /// </summary>
    public partial class AIProvider
    {
        private readonly LearnSite.DAL.AIProvider dal=new LearnSite.DAL.AIProvider();
        public AIProvider()
        {}
        #region  Method

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LearnSite.Model.AIProvider model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(LearnSite.Model.AIProvider model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int Id)
        {
            return dal.Delete(Id);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LearnSite.Model.AIProvider GetModel(int Id)
        {
            return dal.GetModel(Id);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }
        
        /// <summary>
        /// 设置默认模型
        /// </summary>
        public bool SetDefault(int Id)
        {
            return dal.SetDefault(Id);
        }
        
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LearnSite.Model.AIProvider> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LearnSite.Model.AIProvider> DataTableToList(DataTable dt)
        {
            return BllDataTableMappers.MapAIProviderList(dt);
        }

        #endregion  Method
    }
}
