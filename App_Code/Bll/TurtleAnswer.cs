using System;
using System.Data;
using System.Collections.Generic;
using LearnSite.Common;
using LearnSite.Model;
using System.Web;
namespace LearnSite.BLL
{
	/// <summary>
	/// TurtleAnswer
	/// </summary>
	public partial class TurtleAnswer
	{
		private readonly LearnSite.DAL.TurtleAnswer dal=new LearnSite.DAL.TurtleAnswer();
		public TurtleAnswer()
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
		public bool Exists(int Aid)
		{
			return dal.Exists(Aid);
		}
                
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Isdone(int Aqid)
        {
            return dal.Isdone(Aqid);
        }

                
        /// <summary>
        /// 返回本次编程比赛排行名单及成绩  Asid,Sname,Sgrade,Sclass
        /// </summary>
        /// <param name="Amid"></param>
        /// <returns></returns>
        public DataTable GetListRank(int Amid)
        {
            return dal.GetListRank(Amid);
        }

        public int Addsave()
        {
            int myid = 0;
            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
            if (cook.IsExist())
            {
                string code = HttpContext.Current.Request.Form["code"];
                if (!String.IsNullOrEmpty(code))
                {
                    HttpPostedFile pngf = HttpContext.Current.Request.Files["cover"];
                    string id = HttpContext.Current.Request.Form["id"];
                    string qid = HttpContext.Current.Request.Form["qid"];
                    string mid = HttpContext.Current.Request.Form["mid"];
                    string output = HttpContext.Current.Request.Form["output"];
                    string aimg = HttpContext.Current.Request.Form["aimg"];
                    string pass = HttpContext.Current.Request.Form["pass"];

                    Model.TurtleAnswer model = new Model.TurtleAnswer();

                    model.Amid = Int32.Parse(mid);
                    model.Aqid = Int32.Parse(qid);
                    if (pass == "1")
                        model.Ascore = 10;
                    else
                        model.Ascore = 0;
                    model.Aimg = aimg;
                    model.Aurl = "";
                    model.Aout = output;
                    model.Alock = false;
                    model.Adate = DateTime.Now;
                    model.Asid = cook.Sid;
                    model.Asname = cook.Sname;
                    model.Acode = code;

                    if (!string.IsNullOrEmpty(id))
                    {
                        myid = Int32.Parse(id);
                        model.Aid = myid;
                        Update(model);
                    }
                    else
                    {
                        myid = Add(model);
                    }
                }
            }
            return myid;
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LearnSite.Model.TurtleAnswer model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LearnSite.Model.TurtleAnswer model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int Aid)
		{
			
			return dal.Delete(Aid);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LearnSite.Model.TurtleAnswer GetModel(int Aid)
		{			
			return dal.GetModel(Aid);
		}

                
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LearnSite.Model.TurtleAnswer GetModelByQidSid(int Aqid, int Asid)
        {
            return dal.GetModelByQidSid(Aqid, Asid);
        }
		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public LearnSite.Model.TurtleAnswer GetModelByCache(int Aid)
		{
			
			string CacheKey = "TurtleAnswerModel-" + Aid;
            object objModel = LearnSite.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(Aid);
					if (objModel != null)
					{
                        int ModelCache = LearnSite.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LearnSite.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (LearnSite.Model.TurtleAnswer)objModel;
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
		public List<LearnSite.Model.TurtleAnswer> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LearnSite.Model.TurtleAnswer> DataTableToList(DataTable dt)
		{
			return BllDataTableMappers.MapTurtleAnswerList(dt);
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
