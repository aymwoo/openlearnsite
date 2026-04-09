using System;
using System.Data;
using System.Collections.Generic;
using LearnSite.Common;
using LearnSite.Model;
using System.Web;
namespace LearnSite.BLL
{
	/// <summary>
	/// JudgeArg
	/// </summary>
	public partial class JudgeArg
	{
		private readonly LearnSite.DAL.JudgeArg dal=new LearnSite.DAL.JudgeArg();
		public JudgeArg()
		{}
		#region  BasicMethod

                
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Jmid)
        {
            return dal.Exists(Jmid);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LearnSite.Model.JudgeArg model)
		{
			return dal.Add(model);
		}
        public void initCid()
        {
            dal.initCid();
        }

        public int Addsave(int Jmid, int hid)
        {
            string code = HttpContext.Current.Request.Form["code"];
            int myid = 0;
            if (code.Length > 1)
            {
                HttpPostedFile pngf = HttpContext.Current.Request.Files["cover"];
                HttpPostedFile pngfull = HttpContext.Current.Request.Files["fullcover"];
                string arg1 = HttpContext.Current.Request.Form["arg1"];
                string arg2 = HttpContext.Current.Request.Form["arg2"];
                string arg3 = HttpContext.Current.Request.Form["arg3"];

                string res1 = HttpContext.Current.Request.Form["res1"];
                string res2 = HttpContext.Current.Request.Form["res2"];
                string res3 = HttpContext.Current.Request.Form["res3"];
                string id = HttpContext.Current.Request.Form["id"];
                string cid = HttpContext.Current.Request.Form["cid"];
                string mid = HttpContext.Current.Request.Form["mid"];
                string resimg = HttpContext.Current.Request.Form["resimg"];

                Model.JudgeArg model = new Model.JudgeArg();
                model.Jcode = code;
                model.Jhid = hid;
                model.Jmid = Int32.Parse(id);
                model.Jinone = arg1;
                model.Jintwo = arg2;
                model.Jinthree = arg3;
                model.Joutone = res1;
                model.Joutwo = res2;
                model.Jouthree = res3;
                model.Jright = true;
                model.Jsleep = 1000;
                model.Jcid = Int32.Parse(cid);
                model.Jimg = resimg;

                string Wthumbnail = LearnSite.Store.CourseStore.SetCourseStore(cid) + "resimg" + mid + ".png";
                string Fthumbnail = LearnSite.Store.CourseStore.SetCourseStore(cid) + "fullcover" + mid + ".png";
                model.Jthumb = Wthumbnail.Replace("~", "..");

                string Wthumbnailpath = HttpContext.Current.Server.MapPath(Wthumbnail);
                string Fthumbnailpath = HttpContext.Current.Server.MapPath(Fthumbnail);
                if (pngf != null && pngf.ContentLength > 0)
                {
                    //LearnSite.Common.Log.Addlog("图片尺寸", pngf.ContentLength.ToString());
                    pngf.SaveAs(Wthumbnailpath);
                    pngfull.SaveAs(Fthumbnailpath);
                }

                myid = dal.GetIdByMid(Jmid);
                if (myid > 0)
                {
                    model.Jid = myid;
                    Update(model);
                }
                else
                {
                    myid = Add(model);
                }
            }
            else
            {
                dal.DeleteJmid(Jmid);
            }
            return myid;
        }


		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LearnSite.Model.JudgeArg model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int Jid)
		{
			
			return dal.Delete(Jid);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LearnSite.Model.JudgeArg GetModel(int Jid)
		{
			
			return dal.GetModel(Jid);
		}
        
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LearnSite.Model.JudgeArg GetModel(DataTable dt, int Tsort)
        {
            return dal.GetModel(dt, Tsort);
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LearnSite.Model.JudgeArg GetModelByMid(int Jmid)
        {
            return dal.GetModelByMid(Jmid);
        }
		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public LearnSite.Model.JudgeArg GetModelByCache(int Jid)
		{
			
			string CacheKey = "JudgeArgModel-" + Jid;
			object objModel = LearnSite.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(Jid);
					if (objModel != null)
					{
						int ModelCache = LearnSite.Common.ConfigHelper.GetConfigInt("ModelCache");
						LearnSite.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (LearnSite.Model.JudgeArg)objModel;
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
		public List<LearnSite.Model.JudgeArg> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LearnSite.Model.JudgeArg> DataTableToList(DataTable dt)
		{
			return BllDataTableMappers.MapJudgeArgList(dt);
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
