using System;
using System.Data;
using System.Collections.Generic;
using LearnSite.Common;
using LearnSite.Model;
using System.Web;
namespace LearnSite.BLL
{
	/// <summary>
	/// TurtleQuestion
	/// </summary>
	public partial class TurtleQuestion
	{
		private readonly LearnSite.DAL.TurtleQuestion dal=new LearnSite.DAL.TurtleQuestion();
		public TurtleQuestion()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}
        //更新序号
        public bool updateQsort(int Qid, bool way)
        {
            return dal.updateQsort(Qid, way);
        }

        /// <summary>
        /// 初始化序号
        /// </summary>
        /// <param name="Lcid"></param>
        public void Qsortnew(int Qmid)
        {
            dal.Qsortnew(Qmid);
        }
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Qid)
		{
			return dal.Exists(Qid);
		}

        public int Addsave(int hid)
        {
            int myid = 0;

            string mid = HttpContext.Current.Request.Form["mid"];
            LearnSite.BLL.TurtleMatch mbll = new TurtleMatch();
            if (mbll.IsOwer(Int32.Parse(mid), hid))
            {
                string code = HttpContext.Current.Request.Form["code"];
                if (!String.IsNullOrEmpty(code))
                {
                    HttpPostedFile pngf = HttpContext.Current.Request.Files["cover"];
                    string id = HttpContext.Current.Request.Form["id"];
                    string resimg = HttpContext.Current.Request.Form["resimg"];
                    string title = HttpContext.Current.Request.Form["title"];
                    string rest = HttpContext.Current.Request.Form["rest"];

                    Model.TurtleQuestion model = new Model.TurtleQuestion();
                    model.Qcode = code;
                    model.Qmid = Int32.Parse(mid);
                    model.Qtitle = title;
                    model.Qcontent = "";
                    model.Qdegree = 1;
                    model.Qsort = 0;
                    model.Qimg = resimg;
                    model.Qout = rest;
                    model.Qscore = 10;
                    model.Qdate = DateTime.Now;

                    DateTime today = DateTime.Now;
                    string root = "/python/imgmatch";
                    LearnSite.Store.CourseStore.MakeNewDir(root);

                    string Wthumbnail = root + "/" + mid + "-" + today.ToString("yyyy-MM-dd-HH-mm-ss") + ".png";
                    model.Qurl = Wthumbnail;

                    if (!string.IsNullOrEmpty(id))
                    {
                        myid = Int32.Parse(id);
                        model.Qid = myid;
                        Update(model);
                    }
                    else
                    {
                        myid = Add(model);
                    }

                    if (pngf != null && pngf.ContentLength > 0)
                    {
                        string Wthumbnailpath = HttpContext.Current.Server.MapPath(Wthumbnail);
                        pngf.SaveAs(Wthumbnailpath);
                    }
                }
            }
            return myid;
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LearnSite.Model.TurtleQuestion model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LearnSite.Model.TurtleQuestion model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int Qid)
		{
			
			return dal.Delete(Qid);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LearnSite.Model.TurtleQuestion GetModel(int Qid)
		{
			
			return dal.GetModel(Qid);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public LearnSite.Model.TurtleQuestion GetModelByCache(int Qid)
		{
			
			string CacheKey = "TurtleQuestionModel-" + Qid;
            object objModel = LearnSite.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(Qid);
					if (objModel != null)
					{
                        int ModelCache = LearnSite.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LearnSite.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (LearnSite.Model.TurtleQuestion)objModel;
		}

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetListQuestion(string Qmid)
        {
            string strWhere = " Qmid=" + Qmid+" order by Qsort";
            return dal.GetList(strWhere);
        }

                
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LearnSite.Model.TurtleQuestion GetModel(DataTable dt, int Tsort)
        {
            return dal.GetModel(dt, Tsort);
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
		public List<LearnSite.Model.TurtleQuestion> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LearnSite.Model.TurtleQuestion> DataTableToList(DataTable dt)
		{
			return BllDataTableMappers.MapTurtleQuestionList(dt);
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
