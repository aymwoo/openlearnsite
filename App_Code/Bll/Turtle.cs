using System;
using System.Data;
using System.Collections.Generic;
using LearnSite.Common;
using LearnSite.Model;
using System.Web;
namespace LearnSite.BLL
{
	/// <summary>
	/// Turtle
	/// </summary>
	public partial class Turtle
	{
		private readonly LearnSite.DAL.Turtle dal=new LearnSite.DAL.Turtle();
		public Turtle()
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
		public bool Exists(int Tid)
		{
			return dal.Exists(Tid);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LearnSite.Model.Turtle model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LearnSite.Model.Turtle model)
		{
			return dal.Update(model);
		}

        /// <summary>
        /// 上传一个作品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Upload(string id,int hid,int sid) 
        {
            bool isup = false;
            int res = 0;
            int score = 0;
            string ip = Common.Computer.GetGuestIP();

            LearnSite.Model.Turtle model = new LearnSite.Model.Turtle();
            DateTime today = DateTime.Now;

            string root = "/python/thumbnail";
            LearnSite.Store.CourseStore.MakeNewDir(root);

            string Wthumbnail = "/python/thumbnail/" + today.ToString("yyyy-MM-dd-HH-mm-ss") + ".png";
            string Wthumbnailpath = HttpContext.Current.Server.MapPath(Wthumbnail);

            HttpPostedFile pngf = HttpContext.Current.Request.Files["cover"];
            string tcode = HttpContext.Current.Request.Form["tcode"];
            string timg = HttpContext.Current.Request.Form["timg"];
            string tout = HttpContext.Current.Request.Form["tout"];
            string title = HttpContext.Current.Request.Form["title"];
            string study = HttpContext.Current.Request.Form["study"];

            model.Tcode = tcode;
            model.Tcontent = "";
            model.Tdate = today;
            model.Tdegree = 1;
            model.Thid = hid;
            model.Timg = timg;
            model.Tout = tout;
            model.Tsort = 0;
            model.Ttilte = title;
            model.Turl = Wthumbnail;
            model.Tsid = sid;
            model.Tscore = score;
            model.Tip = ip;
            if (study == "0")
            {
                model.Tstudy = false;
            }
            else
            {
                model.Tstudy = true;
            }

            if (string.IsNullOrEmpty(id))
            {
                res = Add(model);
                if (res > 0) isup = true;
            }
            else
            {
                res = Int32.Parse(id);
                model.Tid = res;
                if (Update(model)) isup = true;
            }

            if ( isup && pngf.ContentLength > 0 )
            {
                pngf.SaveAs(Wthumbnailpath);
            }

            return res;
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int Tid)
		{
			
			return dal.Delete(Tid);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LearnSite.Model.Turtle GetModel(int Tid)
		{
			
			return dal.GetModel(Tid);
		}
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LearnSite.Model.Turtle GetModel(DataTable dt, int Tsort)
        {
            return dal.GetModel(dt, Tsort);
        }
		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public LearnSite.Model.Turtle GetModelByCache(int Tid)
		{
			
			string CacheKey = "TurtleModel-" + Tid;
            object objModel = LearnSite.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(Tid);
					if (objModel != null)
					{
                        int ModelCache = LearnSite.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LearnSite.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (LearnSite.Model.Turtle)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
        public DataTable GetList(string strWhere)
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
		public List<LearnSite.Model.Turtle> GetModelList(string strWhere)
		{
			DataTable dt = dal.GetList(strWhere);
			return DataTableToList(dt);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LearnSite.Model.Turtle> DataTableToList(DataTable dt)
		{
			return BllDataTableMappers.MapTurtleList(dt);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetTeacherList()
		{
            string strWhere = " Thid>0 ";
            return GetList(strWhere);
		}

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetAllList()
        {
            string strWhere = " ";
            return GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetVisitList(string strWhere)
        {
            return GetList(strWhere);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataTable GetSketchList(string strWhere)
        {
            return GetList(strWhere);
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
