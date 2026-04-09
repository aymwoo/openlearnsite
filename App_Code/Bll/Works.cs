using System;
using System.Data;
using System.Collections.Generic;
using LearnSite.Model;
using System.Web;
using System.IO;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using com.mxgraph;
using System.Xml;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace LearnSite.BLL
{
	/// <summary>
	/// 业务逻辑类Works 的摘要说明。
	/// </summary>
	public class Works
	{
		private readonly LearnSite.DAL.Works dal=new LearnSite.DAL.Works();
		public Works()
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
		public bool Exists(int Wid)
		{
			return dal.Exists(Wid);
		}
                 /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsWcid(int Wcid)
        {
            return dal.ExistsWcid(Wcid);
        }
        /// <summary>
        /// 是否存在该学号任务作品
        /// </summary>
        public bool ExistsMyMissonWork(int Wmid, string Wnum)
        {
            return dal.ExistsMyMissonWork(Wmid, Wnum);
        }
               
        /// <summary>
        /// 是否存在该学号上一个任务作品,Wmsort为上一个可提交任务序号
        /// </summary>
        public bool ExistsMyFirstWork(int Wcid, string Wnum, int Wmsort)
        {
            return dal.ExistsMyFirstWork(Wcid, Wnum, Wmsort);
        }
		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LearnSite.Model.Works model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(LearnSite.Model.Works model)
		{
			dal.Update(model);
		}
        /// <summary>
        /// 初始化新添加字段Woffice
        /// </summary>
        /// <returns></returns>
        public int UpdateWoffice()
        {
            return dal.UpdateWoffice();
        }  
                
        /// <summary>
        /// 初始化新添加字段Wfscore 互评的平均分
        /// </summary>
        /// <returns></returns>
        public void InitWfscore()
        {
            dal.InitWfscore();
        }
                
        /// <summary>
        /// 更新字段Wfscore
        /// </summary>
        /// <returns></returns>
        public void UpdateWfscore(int Wid, int Wfscore)
        {
            dal.UpdateWfscore(Wid, Wfscore);
        }
                
        /// <summary>
        /// 获取Wfscore
        /// </summary>
        /// <returns></returns>
        public int GetWfscore(int Wid)
        {
            return dal.GetWfscore(Wid);
        }
        /// <summary>
        /// 清除该班级该活动作品的异常转换标志
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wid"></param>
        /// <param name="Wcid"></param>
        public void ClearWflasherror(int Sgrade, int Sclass, int Wmid, int Wcid)
        {
            dal.ClearWflasherror(Sgrade, Sclass, Wmid, Wcid);
        }
        /// <summary>
        /// 更新一条数据,给一个作品评价(参数传送 Wid,Wscore, Wcheck)
        /// </summary>
        public int ScoreOneWork(LearnSite.Model.Works model)
        {
           return dal.ScoreOneWork(model);
        }

        /// <summary>
        /// 更新指定Wid作品的积分,不用数据类型
        /// </summary>
        /// <param name="Wid"></param>
        /// <param name="Wscore"></param>
        public void ScoreWork(int Wid, int Wscore)
        {
            dal.ScoreWork(Wid, Wscore);
        }
                
        /// <summary>
        /// 获取本班本活动学分列表
        /// </summary>
        /// <param name="Wid"></param>
        /// <param name="Wgrade"></param>
        /// <param name="Wclass"></param>
        /// <returns></returns>
        public DataTable getScoreList(int Wmid, int Wgrade, int Wclass)
        {
            return dal.getScoreList(Wmid, Wgrade, Wclass);
        }
        /// <summary>
        /// 设置指定Wid作品的评价状态和积分为零
        /// </summary>
        /// <param name="Wid"></param>
        /// <param name="Wcheck"></param>
        public void CancleScoreWork(int Wid, bool Wcheck)
        {
            dal.CancleScoreWork(Wid, Wcheck);
        }
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int Wid)
		{
			
			dal.Delete(Wid);
		}
                
        /// <summary>
        /// 删除一个班级的作业记录
        /// </summary>
        public int DelClass(int Wgrade, int Wclass, int Wyear)
        {
            return dal.DelClass(Wgrade, Wclass, Wyear);
        }
        /// <summary>
        /// 清除几年前的未推荐的作品记录
        /// </summary>
        /// <param name="Wyear"></param>
        public int DeleteOldyear(int Wyear)
        {
            return dal.DeleteOldyear(Wyear);
        }
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LearnSite.Model.Works GetModel(int Wid)
		{
			
			return dal.GetModel(Wid);
		}
                
        /// <summary>
        /// 学生和活动编号得到一个对象实体
        /// </summary>
        public LearnSite.Model.Works GetModelByStu(int Mid, string Snum)
        {
            return dal.GetModelByStu(Mid, Snum);
        }
		/// <summary>
		/// 得到一个对象实体，从缓存中。
		/// </summary>
		public LearnSite.Model.Works GetModelByCache(int Wid)
		{
			
			string CacheKey = "WorksModel-" + Wid;
            object objModel = LearnSite.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(Wid);
					if (objModel != null)
					{
                        int ModelCache = LearnSite.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LearnSite.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (LearnSite.Model.Works)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
        public string GetWid(string Wmid, string Wnum)
        {
            return dal.GetWid(Wmid, Wnum);
        }
        public string GetWcode(string Wmid, string Wnum)
        {
            return dal.GetWcode(Wmid, Wnum);
        }
        public string GetWcode(string Wid)
        {
            return dal.GetWcode(Wid);
        }
        public string GetWidold(string Wmid, string Wnum)
        {
            return dal.GetWidold(Wmid, Wnum);
        }
        /// <summary>
        /// 根据学生表的年级、班级(不影响班级升学)
        /// 多表联合查询作品，返回dataset
        /// </summary>
        /// <param name="Wcid"></param>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wmid"></param>
        /// <param name="Wcheck"></param>
        /// <returns></returns>
        public DataSet GetListWcheckWork(int Wcid, int Sgrade, int Sclass, int Wmid, bool Wcheck,string sort)
        {
            return dal.GetListWcheckWork(Wcid, Sgrade, Sclass, Wmid, Wcheck, sort);
        }                
        /// <summary>
        /// 显示所教班级该学案所有未评作品
        /// select Wid,Wnum,Wmid,Wmsort,Wurl,Wtype,Wscore,Wtime,Wvote,Wcheck,Wself,Wcan,Wgood,Sname,Sgrade,Sclass,Mtitle
        /// </summary>
        /// <param name="Wcid"></param>
        /// <returns></returns>
        public DataTable GetListNoWcheckWork(int Wcid, int Wclass)
        {
            return dal.GetListNoWcheckWork(Wcid, Wclass);
        }
         /// <summary>
        /// 显示所教班级该学案所有未评班级
        /// select Sclass
        /// </summary>
        /// <param name="Wcid"></param>
        /// <returns></returns>
        public DataTable GetListNoWcheckClass(int Wcid)
        {
            return dal.GetListNoWcheckClass(Wcid);
        }
        /// <summary>
        /// 显示所教某班级该学案所有未评作品
        /// select Wid,Wnum,Wmid,Wmsort,Wurl,Wtype,Wscore,Wtime,Wvote,Wcheck,Wself,Wcan,Wgood,Sname,Sgrade,Sclass,Mtitle
        /// </summary>
        /// <param name="Wcid"></param>
        /// <returns></returns>
        public DataTable GetListNoWcheckWork(int Wcid, int Wgrade, int Wclass,int hid)
        {
            return dal.GetListNoWcheckWork(Wcid, Wgrade, Wclass,hid);
        } 
        /// <summary>
        /// 根据学生生的年级、班级(不影响班级升学)
        /// 设置该班本学案未评价的活动全部积分为10
        /// </summary>
        /// <param name="Wcid"></param>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wmid"></param>
        public void WorkSetA(int Wcid, int Sgrade, int Sclass, int Wmsort)
        {
            dal.WorkSetA(Wcid, Sgrade, Sclass, Wmsort);
        }
        /// <summary>
        /// 设置该班本学案未评价的活动全部积分为6
        /// </summary>
        /// <param name="Wcid"></param>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wmsort"></param>
        public void WorkSetP(int Wcid, int Sgrade, int Sclass, int Wmsort)
        {
            dal.WorkSetP(Wcid, Sgrade, Sclass, Wmsort);
        }
                
        /// <summary>
        /// 根据学生生的年级、班级(不影响班级升学)
        /// 设置该班本学案未评价的活动全部积分为Wscore
        /// </summary>
        /// <param name="Wcid"></param>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wmid"></param>
        /// <param name="Wscore"></param>
        public void WorkSetScore(int Wcid, int Sgrade, int Sclass, int Wmid, int Wscore)
        {
            dal.WorkSetScore(Wcid, Sgrade, Sclass, Wmid, Wscore);
        }

                
        /// <summary>
        /// 根据学生生的年级、班级(不影响班级升学)
        /// 设置该班本学案的活动作品为未评价
        /// </summary>
        /// <param name="Wcid"></param>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wmid"></param>
        public void WorkSetNoneWcheck(int Wcid, int Sgrade, int Sclass, int Wmid)
        {
            dal.WorkSetNoneWcheck(Wcid, Sgrade, Sclass, Wmid);
        }
        /// <summary>
        /// 根据学生生的年级、班级(不影响班级升学)
        /// 设置该班本学案未评价的活动积分为10的作品为已评价
        /// </summary>
        /// <param name="Wcid"></param>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wmid"></param>
        public void WorkSetWcheck(int Wcid, int Sgrade, int Sclass, int Wmid)
        {
            dal.WorkSetWcheck(Wcid, Sgrade, Sclass, Wmid);
        }
        /// <summary>
        /// 根据学生生的年级未评价的活动积分为10的作品为已评价
        /// </summary>
        /// <param name="Wcid"></param>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wmid"></param>
        public void WorkSetWcheckall(int Sgrade)
        {
            dal.WorkSetWcheckall(Sgrade);
        }        
        /// <summary>
        /// 根据学生生的年级、班级(不影响班级升学)
        /// 设置该班本学案未评价的活动积分为10的作品为已评价
        /// </summary>
        /// <param name="Wcid"></param>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wmid"></param>
        public void WorkSetWcheck(int Wcid, int Sgrade, int Sclass)
        {
            dal.WorkSetWcheck(Wcid, Sgrade, Sclass);
        }
        /// <summary>
        /// 设置本学案未评价的活动已得分的作品为已评价
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wmid"></param>
        public void WorkSetWcheckClass(int Wmid, int Sgrade, int Sclass)
        {
            dal.WorkSetWcheckClass(Wmid, Sgrade, Sclass);
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
		public List<LearnSite.Model.Works> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LearnSite.Model.Works> DataTableToList(DataTable dt)
        {
            return BllDataTableMappers.MapWorksList(dt);
        }

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

                /// <summary>
        /// 显示本年级优秀作品的20条记录
        /// </summary>
        /// <param name="Wgrade"></param>
        /// <param name="GridViewwork"></param>
        public DataTable ShowBestWork(int Sgrade, int Syear, int Sterm)
        {
            return dal.ShowBestWork(Sgrade, Syear, Sterm).Tables[0];
        }

        /// <summary>
        /// 显示我的所有作品
        /// </summary>
        /// <param name="Wnum"></param>
        /// <returns></returns>
        public DataTable ShowMywork(string Snum)
        {
            return dal.ShowMywork(Snum);
        }  
                
        /// <summary>
        /// 显示我的所有有缩略图的作品
        /// </summary>
        /// <param name="Wnum"></param>
        /// <returns></returns>
        public DataTable ShowMyAllWork(string Wnum)
        {
            return dal.ShowMyAllWork(Wnum);
        }
        /// <summary>
        /// 显示我本年级本学期的所有作品
        /// </summary>
        /// <param name="Wnum"></param>
        /// <param name="Wgrade"></param>
        /// <param name="Wterm"></param>
        /// <returns></returns>
        public DataSet ShowThisTermWorks(string Wnum, int Wgrade, int Wterm)
        {
            return dal.ShowThisTermWorks(Wnum, Wgrade, Wterm);
        }
         /// <summary>
        /// 显示我本年级本学期的所有作品
        /// </summary>
        /// <param name="Wnum"></param>
        /// <param name="Wgrade"></param>
        /// <param name="Wterm"></param>
        /// <returns></returns>
        public DataSet ShowThisTermWorksCircle(string Wnum, int Wgrade, int Wterm)
        {
            return dal.ShowThisTermWorksCircle(Wnum, Wgrade, Wterm);
        }
        /// <summary>
        /// 显示我的所有作品
        /// </summary>
        /// <param name="Wnum"></param>
        /// <returns></returns>
        public DataTable ShowMyAllWorks(string Wnum)
        {
            return dal.ShowMyAllWorks(Wnum);
        }
        /// <summary>
        /// 列表我有所作品的学案活动代号和分值
        /// </summary>
        /// <param name="Wnum"></param>
        /// <returns></returns>
        public DataSet ShowMyworkScore(string Wnum)
        {
            return ShowMyworkScore(Wnum);
        }
        /// <summary>
        /// 根据作品Wid获得学案名称
        /// </summary>
        /// <param name="Wid"></param>
        /// <returns></returns>
        public string GetCtitle(int Wid)
        {
            return dal.GetCtitle(Wid);
        }
                
        /// <summary>
        /// 阅读量Whit加1
        /// </summary>
        /// <param name="Wid"></param>
        public void UpdateWhit(int Wid)
        {
            dal.UpdateWhit(Wid);
        }

        /// <summary>
        /// 投票Wvote加1
        /// </summary>
        /// <param name="Wid"></param>
        public void UpdateWvote(int Wid)
        {
            dal.UpdateWvote(Wid);
        }
        /// <summary>
        /// 投票Wegg减1
        /// </summary>
        /// <param name="Wid"></param>
        public void UpdateWegg(int Wid)
        {
            dal.UpdateWegg(Wid);
        }
        public void UpdateWegg(int Wmid, string Wnum)
        {
            dal.UpdateWegg(Wmid, Wnum);
        }
        /// <summary>
        /// 获得本作品Wegg
        /// </summary>
        /// <param name="Wid"></param>
        /// <returns></returns>
        public int GetWegg(int Wid)
        {
           return dal.GetWegg(Wid);
        }
                
        /// <summary>
        /// 获得本作品Wegg
        /// </summary>
        /// <param name="Wid"></param>
        /// <returns></returns>
        public int GetWegg(int Wmid, string Wnum)
        {
            return dal.GetWegg(Wmid, Wnum);
        }
                
        /// <summary>
        /// 获得本作品Wegg
        /// </summary>
        /// <param name="Wid"></param>
        /// <returns></returns>
        public int GetWegg(int Wmid, int Wsid)
        {
            return dal.GetWegg(Wmid, Wsid);//引用了自己造成页面崩溃；并引起网站应用程序池停止
        }
        /// <summary>
        /// 显示该学案该学号完成作品的数量
        /// </summary>
        /// <param name="Wcid"></param>
        /// <param name="Wnum"></param>
        /// <returns></returns>
        public string HowCidWorks(int Wcid, string Wnum)
        {
            return dal.HowCidWorks(Wcid, Wnum);
        }
                /// <summary>
        /// 显示该学案该学号完成作品的数量
        /// </summary>
        /// <param name="Wcid"></param>
        /// <param name="Wnum"></param>
        /// <returns></returns>
        public int CountCidWorks(int Wcid, string Wnum)
        {
            return dal.CountCidWorks(Wcid, Wnum);
        }
                /// <summary>
        /// 显示该学案该学号完成作品的数量
        /// </summary>
        /// <param name="Wcid"></param>
        /// <param name="Wnum"></param>
        /// <returns></returns>
        public string HowCidWorks(int Wcid, int Wsid)
        {
            return dal.HowCidWorks(Wcid, Wsid);
        }
        /// <summary>
        /// 显示该学案本班完成作品的数量
        /// </summary>
        /// <param name="Wcid"></param>
        /// <param name="Snums"></param>
        /// <returns></returns>
        public string HowCourseWorks(int Wcid, string Snums)
        {
            return dal.HowCourseWorks(Wcid, Snums);
        }

        /// <summary>
        /// 显示该学案本班完成提交作品和测评作品的数量
        /// </summary>
        /// <param name="Wcid"></param>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public string HowCourseWorks(int Wcid, int Sgrade, int Sclass)
        {
            return dal.HowCourseWorks(Wcid, Sgrade, Sclass);
        }
        /// <summary>
        /// 显示该学案本任务本班完成作品的数量
        /// </summary>
        /// <param name="Wcid"></param>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wmsort"></param>
        /// <returns></returns>
        public string HowWorks(int Syear, int Sgrade, int Sclass, int Wmid)
        {
            return dal.HowWorks(Syear,Sgrade, Sclass, Wmid);
        }

        /// <summary>
        /// 根据学号，获得本学案的作品列表，只返回Wid,Wmsort,Wurl,Wscore,Wip,Wcheck
        /// </summary>
        /// <param name="Wcid"></param>
        /// <param name="Wnum"></param>
        /// <returns></returns>
        public DataSet MyHowWorks(int Wcid, string Wnum)
        {
            return dal.MyHowWorks(Wcid, Wnum);
        }
        /// <summary>
        /// 根据学号和活动mid，获得本活动的作品列表，只返回Wid,Wmsort,Wurl,Wscore,Wip,Wcheck,Wself,Wcan
        /// </summary>
        /// <param name="Wmid"></param>
        /// <param name="Wnum"></param>
        /// <returns></returns>
        public DataSet MyMissonWorks(int Wmid, string Wnum)
        {
            return dal.MyMissonWorks(Wmid, Wnum);
        }
        /// <summary>
        /// 获得该学案本任务本班完成作品
        /// </summary>
        /// <param name="Wcid"></param>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wmid"></param>
        /// <returns></returns>
        public DataTable ShowMissionWorks(int Syear, int Sgrade, int Sclass, int Wmid)
        {
            return dal.ShowMissionWorks(Syear, Sgrade, Sclass, Wmid);
        }

        /// <summary>
        /// 获得该学案本任务本班完成组内作品
        /// </summary>
        /// <param name="Wcid"></param>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wmid"></param>
        /// <returns></returns>
        public DataTable ShowMissionWorksGroup(int Sgrade, int Sclass, int Wmid,int Sgroup)
        {
            return dal.ShowMissionWorksGroup(Sgrade, Sclass, Wmid,Sgroup);
        }

        /// <summary>
        /// 查询今天本班本任务作品列表
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wcid"></param>
        /// <param name="Wmid"></param>
        /// <returns>Wid,Sname,Wurl,Wvote,Wscore,Qwork</returns>
        public DataSet ShowTodayWorks(int Sgrade, int Sclass, int Wcid, int Wmid)
        {            
            return dal.ShowTodayWorks(Sgrade, Sclass, Wcid, Wmid);
        }
        /// <summary>
        /// 查询本班本任务作品列表
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wcid"></param>
        /// <param name="Wmid"></param>
        /// <returns>Wid,Sname,Wurl,Wvote,Wscore,Qwork</returns>
        public DataSet ShowClassWorks(int Sgrade, int Sclass, int Wcid, int Wmid)
        {
            return dal.ShowClassWorks(Sgrade, Sclass, Wcid, Wmid);
        }             
        /// <summary>
        /// 查询本班本任务作品列表
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wcid"></param>
        /// <param name="Wmid"></param>
        /// <returns>Wid,Sname,Wurl,Wvote,Wscore,Qwork</returns>
        public DataTable ShowClassWorksBySort(int Sgrade, int Sclass, int Wmid, string Sort)
        {
            return dal.ShowClassWorksBySort(Sgrade, Sclass, Wmid, Sort);
        }
        /// <summary>
        /// 查询本课优秀作品列表
        /// </summary>
        /// <param name="Wcid"></param>
        /// <returns>Wid,Sname,Wurl</returns>
        public DataTable ShowCircleGoodSelect(int Wcid)
        {
            return dal.ShowCircleGood( Wcid);
        }
        /// <summary>
        /// 查询本班本任务作品列表
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wcid"></param>
        /// <param name="Wmid"></param>
        /// <returns>Wid,Sname,Wurl</returns>
        public DataTable ShowCircleWorksSelect(int Sgrade, int Sclass, int Wcid, int Wmid,int Wscore,bool Wnone)
        {
            switch (Wscore)
            {
                case 12:
                    return dal.ShowCircleGood(Sgrade, Sclass, Wcid, Wmid);
                case 10:
                    return dal.ShowCircleScore(Sgrade, Sclass, Wcid, Wmid,Wscore);
                default:
                    if (Wnone)
                    {
                        return dal.ShowCircleWorksNoPass(Sgrade, Sclass, Wcid, Wmid);
                    }
                    else
                    {
                        return dal.ShowCircleWorks(Sgrade, Sclass, Wcid, Wmid);
                    }
            }
        }
                /// <summary>
        /// 查询本班本任务作品列表
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wcid"></param>
        /// <param name="Wmid"></param>
        /// <returns>Wid,Sname,Wurl</returns>
        public DataTable ShowCircleWorksNoPass(int Sgrade, int Sclass, int Wcid, int Wmid)
        {
            return dal.ShowCircleWorksNoPass(Sgrade, Sclass, Wcid, Wmid);
        }
        /// <summary>
        /// 查询本班本任务作品列表
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wcid"></param>
        /// <param name="Wmid"></param>
        /// <returns>Wid,Sname,Wurl</returns>
        public DataTable ShowCircleWorks(int Sgrade, int Sclass, int Wcid, int Wmid)
        {
            return dal.ShowCircleWorks(Sgrade, Sclass, Wcid, Wmid);
        }
        /// <summary>
        /// 查询本班本任务作品列表,flash专用
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wcid"></param>
        /// <param name="Wmid"></param>
        /// <param name="Wid"></param>
        /// <returns></returns>
        public DataSet ShowClassFlashWorks(int Sgrade, int Sclass, int Wcid, int Wmid)
        {
            return dal.ShowClassFlashWorks(Sgrade, Sclass, Wcid, Wmid);
        }  
        /// <summary>
        /// 查询本班本任务作品列表,office专用
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wcid"></param>
        /// <param name="Wmid"></param>
        /// <param name="Wid"></param>
        /// <returns></returns>
        public DataSet ShowClassOfficeWorks(int Sgrade, int Sclass, int Wcid, int Wmid)
        {
            return dal.ShowClassOfficeWorks(Sgrade, Sclass, Wcid, Wmid);
        }
        /// <summary>
        /// 查询本班本任务作品列表,Photo专用
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wcid"></param>
        /// <param name="Wmid"></param>
        /// <param name="Wid"></param>
        /// <returns></returns>
        public DataSet ShowClassPhotoWorks(int Sgrade, int Sclass, int Wcid, int Wmid)
        {
            return dal.ShowClassPhotoWorks(Sgrade, Sclass, Wcid, Wmid);
        }                
        /// <summary>
        /// 查询本班本任务作品列表,Swf专用
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wcid"></param>
        /// <param name="Wmid"></param>
        /// <param name="Wid"></param>
        /// <returns></returns>
        public DataSet ShowClassSwfWorks(int Sgrade, int Sclass, int Wcid, int Wmid)
        {
            return dal.ShowClassWtypeWorks(Sgrade, Sclass, Wcid, Wmid,"swf");
        }

                
        /// <summary>
        /// 查询本班本任务作品列表,htm专用
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wcid"></param>
        /// <param name="Wmid"></param>
        /// <param name="Wid"></param>
        /// <returns></returns>
        public DataSet ShowClasshtmWorks(int Sgrade, int Sclass, int Wcid, int Wmid)
        {
            return dal.ShowClassWtypeWorks(Sgrade, Sclass, Wcid, Wmid,"htm");
        }
        /// <summary>
        /// 查询本班本任务作品列表,通用
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wcid"></param>
        /// <param name="Wmid"></param>
        /// <param name="Wtype"></param>
        /// <returns></returns>
        public DataSet ShowClassWtypeWorks(int Sgrade, int Sclass, int Wcid, int Wmid,string Wtype)
        {
            return dal.ShowClassWtypeWorks(Sgrade, Sclass, Wcid, Wmid, Wtype);
        }
        /// <summary>
        /// 查询本班本任务未完成学生姓名列表
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wcid"></param>
        /// <param name="Wmid"></param>
        /// <returns></returns>
        public DataTable ShowClassNoWorks(int Syear, int Sgrade, int Sclass, int Wmid)
        {
            return dal.ShowClassNoWorks(Syear, Sgrade, Sclass,Wmid);
        }
                /// <summary>
        /// 查询今天本班本任务未完成学生姓名列表
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wcid"></param>
        /// <param name="Wmid"></param>
        /// <returns></returns>
        public DataSet ShowTodayNotWorks(int Syear, int Sgrade, int Sclass, int Wmid)
        {
            return dal.ShowTodayNotWorks(Syear, Sgrade, Sclass, Wmid);
        }
        /// <summary>
        /// 查询今天本班本任务未完成学生姓名列表
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wcid"></param>
        /// <param name="Wmid"></param>
        /// <returns></returns>
        public DataSet ShowTodayNoWorks(int Sgrade, int Sclass, int Wcid, int Wmid)
        {
            return dal.ShowTodayNoWorks(Sgrade, Sclass, Wcid, Wmid);
        }
        /// <summary>
        /// 查询今天本班学案Cid
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public string GetTodayCid(int Sgrade, int Sclass,int Syear)
        {
            return dal.GetTodayCid(Sgrade, Sclass,Syear);
        }
        /// <summary>
        /// 查找本班本学案本任务本机Ip 已经完成的学号
       /// </summary>
       /// <param name="Sgrade"></param>
       /// <param name="Sclass"></param>
       /// <param name="Wcid"></param>
       /// <param name="Wmid"></param>
       /// <param name="Wip"></param>
       /// <returns></returns>
        public string IpWorkDoneSnum(int Sgrade, int Sclass, int Wcid, int Wmid, string Wip)
        {
            return dal.IpWorkDoneSnum(Sgrade, Sclass, Wcid, Wmid, Wip);
        } 
                
        /// <summary>
        /// 判断该学号本任务作品是否检验通过
        /// </summary>
        /// <param name="Cid"></param>
        /// <returns></returns>
        public bool WorkPass(int Wsid, int Wmid)
        {
            return dal.WorkPass(Wsid, Wmid);
        }
        /// <summary>
        /// 根据学号和活动编号返回作品链接
        /// </summary>
        /// <param name="Wnum"></param>
        /// <param name="Wmid"></param>
        /// <returns></returns>
        public string WorkUrl(string Wnum, int Wmid)
        {
            return dal.WorkUrl(Wnum, Wmid);
        }

        /// <summary>
        /// 判断该学号本任务作品是否提交并记录,返回Wid的值
        /// </summary>
        /// <param name="Cid"></param>
        /// <returns></returns>
        public string WorkDone(string Wnum, int Wcid, int Wmid)
        {
            return dal.WorkDone(Wnum, Wcid, Wmid);

        }
                
        /// <summary>
        /// 判断该学号本任务作品是否提交评价
        /// </summary>
        /// <param name="Wnum"></param>
        /// <param name="Wcid"></param>
        /// <param name="Wmid"></param>
        /// <returns></returns>
        public bool WorkDoneChecked(string Wnum, int Wcid, int Wmid)
        {
            return dal.WorkDoneChecked(Wnum, Wcid, Wmid);
        }
        /// <summary>
        /// 检查该作品是否已经评分,是则返回真
        /// </summary>
        /// <param name="Wid"></param>
        /// <returns></returns>
        public  bool IsChecked(int Wid)
        {
            return dal.IsChecked(Wid);
        }
        /// <summary>
        /// 作品提交， 更新一条数据
        /// </summary>
        /// <param name="Wid"></param>
        /// <param name="Wurl"></param>
        /// <param name="Wfilename"></param>
        /// <param name="Wlength"></param>
        /// <param name="Wdate"></param>
        /// <param name="Wcan"></param>
        public void UpdateWorkUp(int Wid, string Wurl, string Wfilename, int Wlength, DateTime Wdate, bool Wcan, string Wthumbnail)
        {
            dal.UpdateWorkUp(Wid, Wurl, Wfilename, Wlength, Wdate, Wcan, Wthumbnail);
        }

        /// <summary>
        /// 作品提交， 更新一条数据Wthumbnail='" + imgurl 
        /// </summary>
        /// <param name="Wid"></param>
        /// <param name="Wurl"></param>
        /// <param name="Wfilename"></param>
        /// <param name="Wlength"></param>
        /// <param name="Wdate"></param>
        /// <param name="Wcan"></param>
        public void UpdateHtml(int Wid, string Wurl, string Wfilename, int Wlength, DateTime Wdate, bool Wcan, string Wthumbnail, string Wtitle, string Wcode)
        {
            dal.UpdateHtml(Wid, Wurl, Wfilename, Wlength, Wdate, Wcan, Wthumbnail, Wtitle, Wcode);
        }

        /// <summary>
        /// 主题作品提交， 更新一条数据Wthumbnail='" + imgurl 
        /// </summary>
        /// <param name="Wid"></param>
        /// <param name="Wurl"></param>
        /// <param name="Wfilename"></param>
        /// <param name="Wlength"></param>
        /// <param name="Wdate"></param>
        /// <param name="Wcan"></param>
        public void UpdateTopic(int Wid, string Wurl, string Wfilename, int Wlength, DateTime Wdate, bool Wcan, string Wthumbnail, string Wtitle, string Wcode, string Wtype, int Wscore)
        {
            dal.UpdateTopic(Wid, Wurl, Wfilename, Wlength, Wdate, Wcan, Wthumbnail, Wtitle, Wcode, Wtype, Wscore);
        }
        public void UpdateTopic(int Wid, string Wurl, string Wfilename, int Wlength, DateTime Wdate, bool Wcan, string Wthumbnail, string Wtitle, string Wcode, string Wtype)
        {
            int Wscore = 0;
            dal.UpdateTopic(Wid, Wurl, Wfilename, Wlength, Wdate, Wcan, Wthumbnail, Wtitle, Wcode, Wtype, Wscore);
        }
                
        /// <summary>
        /// 作品提交， 更新一条数据Wthumbnail='" + imgurl 
        /// </summary>
        /// <param name="Wid"></param>
        /// <param name="Wurl"></param>
        /// <param name="Wfilename"></param>
        /// <param name="Wlength"></param>
        /// <param name="Wdate"></param>
        /// <param name="Wcan"></param>
        public void UpdatepythonUpIp(int Wid, string Wurl, string Wfilename, int Wlength, DateTime Wdate, bool Wcan, string Wthumbnail, string Wtitle, string Wcode, string Wdict, bool Wpass, string Wtype, string Wip)
        {
            int Wscore = 0;
            if (Wpass) Wscore = 10;
            dal.UpdatepythonUpIp(Wid, Wurl, Wfilename, Wlength, Wdate, Wcan, Wthumbnail, Wtitle, Wcode, Wdict, Wscore, Wpass, Wtype,Wip);

        }
        /// <summary>
        /// 作品提交， 更新一条数据
        /// </summary>
        /// <param name="Wid"></param>
        /// <param name="Wurl"></param>
        /// <param name="Wfilename"></param>
        /// <param name="Wlength"></param>
        /// <param name="Wdate"></param>
        /// <param name="Wcan"></param>
        public void UpdatepythonUp(int Wid, string Wurl, string Wfilename, int Wlength, DateTime Wdate, bool Wcan, string Wthumbnail, string Wtitle, string Wcode, string Wdict, bool Wpass,string Wtype)
        {
            int Wscore = 0;
            if (Wpass) Wscore = 10;
            dal.UpdatepythonUp(Wid, Wurl, Wfilename, Wlength, Wdate, Wcan, Wthumbnail, Wtitle, Wcode, Wdict, Wscore,Wpass,Wtype);
        }
        /// <summary>
        /// 作品提交， 更新一条数据
        /// </summary>
        /// <param name="Wid"></param>
        /// <param name="Wurl"></param>
        /// <param name="Wfilename"></param>
        /// <param name="Wlength"></param>
        /// <param name="Wdate"></param>
        /// <param name="Wcan"></param>
        public void UpdategraphUp(int Wid, string Wurl, string Wfilename, int Wlength, DateTime Wdate, bool Wcan, string Wthumbnail, string Wtitle, string Wcode, string Wdict)
        {
            dal.UpdategraphUp(Wid, Wurl, Wfilename, Wlength, Wdate, Wcan, Wthumbnail, Wtitle, Wcode, Wdict);
        }
        /// <summary>
        /// 作品提交， 更新一条数据
        /// </summary>
        /// <param name="Wid"></param>
        /// <param name="Wurl"></param>
        /// <param name="Wfilename"></param>
        /// <param name="Wlength"></param>
        /// <param name="Wdate"></param>
        /// <param name="Wcan"></param>
        public void UpdateWorkUp(int Wid, string Wurl, string Wfilename, int Wlength, DateTime Wdate, bool Wcan, string Wthumbnail, string Wtitle)
        {
            dal.UpdateWorkUp(Wid, Wurl, Wfilename, Wlength, Wdate, Wcan, Wthumbnail,Wtitle);
        }
        public void UpdateWorkUpday(int Wid, string Wurl, string Wfilename, int Wlength, DateTime Wdate, bool Wcan, string Wthumbnail, string Wtitle)
        {
            dal.UpdateWorkUpday(Wid, Wurl, Wfilename, Wlength, Wdate, Wcan, Wthumbnail, Wtitle);
        }
        /// <summary>
        ///作品提交 增加一条数据
        ///(Wnum, Wcid,Wmid,Wmsort, Wfilename,Wtype, Wurl,Wlength, Wdate, Wip, Wtime)
        /// </summary>
        /// 
        public int AddWorkUp(LearnSite.Model.Works model)
        {
           return dal.AddWorkUp(model);
        }
                
        /// <summary>
        ///python作品提交 增加一条数据
        ///(Wnum,Wcid,Wmid,Wmsort,Wfilename,Wtype,Wurl,Wlength,Wdate,Wip,Wtime,Wegg,Wgrade,Wterm,Woffice,Wflash,Wsid,Wclass,Wname,Wyear,Wthumbnail,Wtitle,Wcode,Wdict,Wscore,Wpass)
        /// </summary>
        /// 
        public int AddPythonUp(LearnSite.Model.Works model)
        {
            return dal.AddPythonUp(model);
        }
        /// <summary>
        /// 更新自我评价
        /// </summary>
        /// <param name="Wmid"></param>
        /// <param name="Wself"></param>
        public void UpdateWself(int Wid, string Wself)
        {
            dal.UpdateWself(Wid,Wself);    
        }
        /// <summary>
        /// 更新是否要求教师评价作品
        /// </summary>
        /// <param name="Wmid"></param>
        /// <param name="Wnum"></param>
        /// <param name="Wcan"></param>
        public void UpdateWcan(int Wmid, int Wnum, bool Wcan)
        {
            dal.UpdateWcan(Wmid, Wnum, Wcan);
        }        
        /// <summary>
        /// 最佳作品推荐字段自动取反
        /// </summary>
        /// <param name="Wid"></param>
        public void UpdateWgood(int Wid)
        {
            dal.UpdateWgood(Wid);
        }
                
        /// <summary>
        /// 最佳作品推荐字段设置为真
        /// </summary>
        /// <param name="Wid"></param>
        public void WgoodBest(int Wid)
        {
            dal.WgoodBest(Wid);
        }
        /// <summary>
        /// 最佳作品推荐字段设置为假
        /// </summary>
        /// <param name="Wid"></param>
        public void WgoodNormal(int Wid)
        {
            dal.WgoodNormal(Wid);
        }
                /// <summary>
        /// 获得该学案最佳作品列表Wid,Sname,Wurl,Wvote,Wgood
        /// </summary>
        /// <param name="Wcid"></param>
        /// <param name="Sgrade"></param>
        /// <returns></returns>
        public DataSet ShowCourseBestWorks(int Wcid, int Sgrade)
        {
            return dal.ShowCourseBestWorks(Wcid, Sgrade);
        }
                  
        /// <summary>
        /// 将所教班级所有未评作品的评分都设置为P，即6分
        /// </summary>
        public void WorkNoScoreSetP(int Rhid)
        {
            dal.WorkNoScoreSetP(Rhid);
        }
        /// <summary>
        /// 显示本学案未评数
        /// </summary>
        /// <param name="Wcid"></param>
        /// <param name="Sgrade"></param>
        /// <returns></returns>
        public string ShowNotCheckCounts(int Wcid, int Sgrade)
        {
            return dal.ShowNotCheckCounts(Wcid, Sgrade);
        }

        /// <summary>
        /// 显示该学案本任务本班未评数
        /// </summary>
        /// <param name="Wcid"></param>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wmid"></param>
        /// <returns></returns>
        public string ClassNotCheckWorks(int Wcid, int Sgrade, int Sclass, int Wmid)
        {
            return dal.ClassNotCheckWorks(Wcid, Sgrade, Sclass, Wmid);
        }
                
        /// <summary>
        /// 获取今天作业的平均分
        /// </summary>
        /// <param name="Wnum"></param>
        /// <returns></returns>
        public int GetTodayWorkScores(string Wnum)
        {
            return dal.GetTodayWorkScores(Wnum);
        }
                
        /// <summary>
        /// 教师未评时可给组内成员作品评分
        /// </summary>
        /// <param name="Wid"></param>
        /// <param name="Wlscore"></param>
        public void Updatelscore(int Wid, int Wlscore)
        {
            dal.Updatelscore(Wid, Wlscore);
        }   
        /// <summary>
        ///  给学生作品打分
        /// </summary>
        /// <param name="Wmid"></param>
        /// <param name="Wnum"></param>
        /// <param name="Wlscore"></param>
        public void Updatemscore(int Wmid, string Wnum, int Wlscore)
        {
            dal.Updatemscore(Wmid, Wnum, Wlscore);
        }
         /// <summary>
        /// 查询本班本任务作品列表
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <param name="Wcid"></param>
        /// <param name="Wmid"></param>
        /// <returns>Wid,Sname,Wurl</returns>
        public DataTable ShowCircleGood(int Sgrade, int Sclass, int Wcid, int Wmid)
        {
            return dal.ShowCircleGood(Sgrade, Sclass, Wcid, Wmid);
        }
        /// <summary>
        /// 给学生作品打分并评语
        /// </summary>
        /// <param name="Wmid"></param>
        /// <param name="Wnum"></param>
        /// <param name="Wlscore"></param>
        /// <param name="Wself"></param>
        public void Updatemscoreself(int Wmid, string Wnum, int Wlscore, string Wself, int Wdscore)
        {
            dal.Updatemscoreself(Wmid, Wnum, Wlscore, Wself, Wdscore);
        }
                
        /// <summary>
        /// 给自动得分的学生作品设置为已评
        /// </summary>
        /// <param name="Wmid"></param>
        /// <param name="Wnum"></param>
        public void UpdateWcheck(int Wmid, string Wnum)
        {
            dal.UpdateWcheck(Wmid, Wnum);
        }
        /// <summary>
        /// 给学生作品打分并评语
        /// </summary>
        /// <param name="Wcid"></param>
        /// <param name="Wmid"></param>
        /// <param name="Wnum"></param>
        /// <param name="Wlscore"></param>
        /// <param name="Wself"></param>
        /// <param name="Wdscore"></param>
        public void Updatemscoreself(string Wcid, string Wmid, string Wnum, int Wlscore, string Wself, int Wdscore)
        {
            dal.Updatemscoreself(Wcid,Wmid, Wnum, Wlscore, Wself,Wdscore);
        }
        /// <summary>
        /// 根据学号和任务ID返回成绩值
        /// </summary>
        /// <param name="Wmid"></param>
        /// <param name="Wnum"></param>
        /// <returns></returns>
        public string GetmyScore(int Wmid, string Wnum)
        {
            return dal.GetmyScore(Wmid, Wnum);
        }                
        ///<summary>
        /// 删除该学号该活动的作品
        /// </summary>
        /// <param name="Wmid"></param>
        /// <param name="Wnum"></param>
        public void Delmywork(int Wmid, string Wnum)
        {
            dal.Delmywork(Wmid,Wnum);
        }
        /// <summary>
        /// 根据学号和任务ID返回成绩值和评语
        /// </summary>
        /// <param name="Wmid"></param>
        /// <param name="Wnum"></param>
        /// <returns></returns>
        public string[] GetmyScoreWself(int Wmid, string Wnum)
        {
            return dal.GetmyScoreWself(Wmid, Wnum);
        }                
        /// <summary>
        /// 根据学号和任务ID返回成绩值和评语
        /// </summary>
        /// <param name="Cid"></param>
        /// <param name="Wmid"></param>
        /// <param name="Wnum"></param>
        /// <returns></returns>
        public string[] GetmyScoreWself(string Cid, string Wmid, string Wnum)
        {
            return dal.GetmyScoreWself(Cid,Wmid,Wnum);
        }
        /// <summary>
        /// 获取本组内同学作品
        /// </summary>
        /// <param name="Sgroup"></param>
        /// <param name="Wmid"></param>
        /// <returns></returns>
        public DataSet GetGroupWorks(int Sgroup, int Wmid)
        {
            return dal.GetGroupWorks(Sgroup, Wmid);
        }
               
        /// <summary>
        /// 显示本学期我未学外的某课所有优秀作品
        /// </summary>
        /// <param name="Wnum"></param>
        /// <param name="Cobj"></param>
        /// <param name="Cid"></param>
        /// <param name="Sgrade"></param>
        /// <returns></returns>
        public DataSet ShowAllGood(string Wnum, int Cobj, int Cid, int Sgrade)
        {
            return dal.ShowAllGood(Wnum, Cobj, Cid, Sgrade);
        }
                
        /// <summary>
        /// 显示该学案的优秀推荐作品
        /// </summary>
        /// <param name="Cid"></param>
        /// <returns></returns>
        public DataSet ShowCourseGoodWorks(int Cid)
        {
            return dal.ShowCourseGoodWorks(Cid);
        }
                
        /// <summary>
        /// 获取当前班级学过的学案Cid
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public string ShowDoneWorkCids(int Sgrade, int Sclass, int Wterm, int Wyear)
        {
            return dal.ShowDoneWorkCids(Sgrade,Sclass, Wterm, Wyear);
        }
                
        /// <summary>
        /// 获取Wurl
        /// </summary>
        /// <param name="Wid"></param>
        /// <returns></returns>
        public string GetWorkWurl(int Wid)
        {
            return dal.GetWorkWurl(Wid);
        }
                
        /// <summary>
        /// 获取Wurl
        /// </summary>
        /// <param name="Wmid"></param>
        /// <param name="Wnum"></param>
        /// <returns></returns>
        public string GetWorkWurla(int Wmid, string Wnum)
        {
            return dal.GetWorkWurla(Wmid, Wnum);
        }
        /// <summary>
        /// 获取某学生学过的学案Cid
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public string ShowStuDoneWorkCids(string Snum, int Wterm, int Wgrade)
        {
            return dal.ShowStuDoneWorkCids(Snum,Wterm,Wgrade);
        }
                
        /// <summary>
        /// 获取最新的作品评语
        /// </summary>
        /// <param name="Snum"></param>
        /// <returns></returns>
        public string[] ShowLastWorkSelf(int Sid)
        {
            return dal.ShowLastWorkSelf(Sid);
        } 
                
        /// <summary>
        /// 获取该学生最近同类型作品
        /// </summary>
        /// <param name="Sid"></param>
        /// <param name="Cid"></param>
        /// <returns></returns>
        public string ShowLastTypeWorks(string Wnum, string Wtype, string Wcid)
        {
            return dal.ShowLastTypeWorks(Wnum, Wtype,Wcid);
        }
        /// <summary>
        /// 获取该学生最新的作业列表8个记录
        /// </summary>
        /// <param name="Sid"></param>
        /// <param name="Cid"></param>
        /// <returns></returns>
        public DataTable ShowLastWorks(int Sid, int Sgrade, int Term, int Cid)
        {
            return dal.ShowLastWorks(Sid, Sgrade, Term, Cid);
        }
        
        public DataTable ShowWyears()
        {
            return dal.ShowWyears();
        }

        public DataTable ShowWgrades(int Wyear)
        {
            return dal.ShowWgrades(Wyear);
        }

        public DataTable ShowWclass(int Wyear, int Wgrade)
        {
            return dal.ShowWclass(Wyear, Wgrade);
        }
        public DataTable ShowWclassWcids(int Wyear, int Wgrade, int Wclass, int Wterm)
        {
            return dal.ShowWclassWcids(Wyear, Wgrade, Wclass, Wterm);
        }
        public DataTable ShowWclassWmids(int Wyear, int Wgrade, int Wclass, int Wterm, int Wcid)
        {
            return dal.ShowWclassWmids(Wyear, Wgrade, Wclass, Wterm, Wcid);
        }
        public DataTable ShowWclassWorks(int Wyear, int Wgrade, int Wclass, int Wterm,int Wmid)
        {
            return dal.ShowWclassWorks(Wyear, Wgrade, Wclass, Wterm, Wmid);
        }
                
        /// <summary>
        /// 获取所有得分12的优秀作品列表
        /// Wid,Wcid,Wmid,Wurl,Wname,Wgrade,Wclass,Wyear,Wtype,Wscore,Wdate,Ctitle
        /// </summary>
        /// <returns></returns>
        public DataTable GetListGoodWorks()
        {
            return dal.GetListGoodWorks();
        }
        /// <summary>
        /// Wid,Wurl,Wname,Wscore
        /// </summary>
        /// <param name="Wcid"></param>
        /// <returns></returns>
        public DataTable GetCourseWorks(int Wcid)
        {
            return dal.GetCourseWorks(Wcid);
        }
        public string GetHtmMid(int Wcid, string Wnum)
        {
            return dal.GetHtmMid(Wcid, Wnum);
        }
        public string GetHtmCid(string Wnum)
        {
            return dal.GetHtmCid(Wnum);
        }
        /// <summary>
        /// 初始化加分值
        /// </summary>
        public void initWdscore()
        {
            dal.initWdscore();
        }

        /// <summary>
        /// 标记缩略图
        /// </summary>
        /// <param name="Wnum"></param>
        /// <param name="Wmid"></param>
        /// <param name="imgurl"></param>
        public void upWthumbnail(string Wnum, string Wmid, string imgurl)
        {
            dal.upWthumbnail(Wnum, Wmid,imgurl);
        }

        public void SaveProject(string id)
        {
            string[] arrayid = id.Split('-');
            string Wcid = arrayid[0];
            string Wmid = arrayid[1];
            string Wlid = arrayid[2];

            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

            int Wsid = cook.Sid;
            string Wnum = cook.Snum;
            string Wname = cook.Sname;
            string Wgrade = cook.Sgrade.ToString();
            string Wclass = cook.Sclass.ToString();
            string Wyear = cook.Syear.ToString();
            string Wterm = cook.ThisTerm.ToString();
            string Wip = cook.LoginIp;
            string Wtime = cook.LoginTime;
            DateTime Wdate = DateTime.Now;

            string MySavePath = LearnSite.Common.WorkUpload.GetWurl(Wyear, Wgrade, Wclass, Wcid, Wmid);//获得作品保存路径（如果不存在，自动创建）
            string RndTime = LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate);
            string OnlyFileName = Wnum + "_" + Wcid + "_" + Wmid + "_" + RndTime;
            string NewFileName = OnlyFileName + ".sb3";
            string Wurl = MySavePath + "/" + NewFileName;
            string SaveFile = HttpContext.Current.Server.MapPath(Wurl);

            string NewThumbnail = OnlyFileName + ".png";
            string Wthumbnail = MySavePath + "/" + NewThumbnail;
            string thumbnailpath = HttpContext.Current.Server.MapPath(Wthumbnail);
            int flen = 0;
            int plen = 0;
            string title = "未命名";
            try
            {
                title = HttpContext.Current.Request.Form["title"];

                HttpPostedFile pngf = HttpContext.Current.Request.Files["cover"];
                plen = pngf.ContentLength;
                pngf.SaveAs(thumbnailpath);

                HttpPostedFile sbf = HttpContext.Current.Request.Files["file"];
                flen = sbf.ContentLength;
                sbf.SaveAs(SaveFile);
                
                //title = HttpContext.Current.Request.Form["title"];

                //string msg = "图片大小" + plen.ToString() + "文件大小" + flen.ToString() + "标题" + title;

                //LearnSite.Common.Log.Addlog("scratch3作品上传调试信息：", msg);
            }
            catch (Exception ec)
            {
                LearnSite.Common.Log.Addlog("保存出错信息", ec.StackTrace );// ec.StackTrace可以出详细信息
            }

            bool checkcan = true;

            BLL.Works bll = new BLL.Works();
            Model.Works model = new Model.Works();//////////////////
            model = bll.GetModelByStu(Int32.Parse(Wmid), Wnum);
            //string Wid = bll.WorkDone(Wnum, Int32.Parse(Wcid), Int32.Parse(Wmid));//返回空字符表示不存在该记录
            //记录到数据库
            if (model !=null)
            {
                int today = DateTime.Now.DayOfYear;
                int workday = model.Wdate.Value.DayOfYear;
                if (today == workday)
                {
                    bll.UpdateWorkUp(model.Wid, Wurl, NewFileName, flen, Wdate, checkcan, Wthumbnail, title);//更新Wfilename, Wurl,Wlength, Wdate
                }
                else
                {
                    bll.UpdateWorkUpday(model.Wid, Wurl, NewFileName, flen, Wdate, checkcan, Wthumbnail, title);//更新Wfilename, Wurl,Wlength, Wdate
                }
            }
            else
            {
                //Wnum,Wcid,Wmid,Wmsort,Wfilename,Wtype,Wurl,Wlength,Wdate,Wip
                //,Wtime,Wegg,Wgrade,Wterm,Woffice,Wflash,Wsid,Wclass,Wname,Wyear
                Model.Works wmodel = new Model.Works();//////////////////
                wmodel.Wnum = Wnum;
                wmodel.Wcid = Int32.Parse(Wcid);
                wmodel.Wmid = Int32.Parse(Wmid);
                wmodel.Wmsort = 5;
                wmodel.Wfilename = NewFileName;
                wmodel.Wtype = "sb3";
                wmodel.Wurl = Wurl;
                wmodel.Wlength = flen;
                wmodel.Wdate = Wdate;
                wmodel.Wip = Wip;
                wmodel.Wtime = Wtime;
                wmodel.Wcan = checkcan;
                wmodel.Wcheck = false;
                wmodel.Wegg = 12;//设定票数为12张
                wmodel.Whit = 0;
                wmodel.Wgrade = Int32.Parse(Wgrade);
                wmodel.Wterm = Int32.Parse(Wterm);
                wmodel.Woffice = false;
                wmodel.Wsid = Wsid;
                wmodel.Wclass = Int32.Parse(Wclass);
                wmodel.Wname = HttpUtility.UrlDecode(Wname);
                wmodel.Wyear = Int32.Parse(Wyear);
                wmodel.Wflash = false;
                wmodel.Werror = false;
                wmodel.Wthumbnail = Wthumbnail;
                wmodel.Wtitle = title;
                wmodel.Wlid = Int32.Parse(Wlid);

                bll.AddWorkUp(wmodel);//添加作品提交记录
                BLL.Signin sn = new BLL.Signin();
                sn.UpdateQwork(Wsid, Int32.Parse(Wcid));//更新今天签到表中的作品数量

                //添加课堂活动记录
                Model.MenuWorks kmodel = new Model.MenuWorks();
                kmodel.Klid = Int32.Parse(Wlid);
                kmodel.Ksid = Wsid;
                kmodel.Ktime = LearnSite.Common.Computer.GoneMinute(DateTime.Parse(Wtime), Wdate);
                kmodel.Kseconds = Int32.Parse(LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate));
                kmodel.Kcheck = false;
                BLL.MenuWorks kbll = new MenuWorks();
                kbll.Add(kmodel);
            }

        }

        public void SavePython(string id)
        {
            string[] arrayid = id.Split('-');
            string Wcid = arrayid[0];
            string Wmid = arrayid[1];
            string Wlid = arrayid[2];

            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

            int Wsid = cook.Sid;
            string Wnum = cook.Snum;
            string Wname = cook.Sname;
            string Wgrade = cook.Sgrade.ToString();
            string Wclass = cook.Sclass.ToString();
            string Wyear = cook.Syear.ToString();
            string Wterm = cook.ThisTerm.ToString();
            string Wip = cook.LoginIp;
            string Wtime = cook.LoginTime;
            DateTime Wdate = DateTime.Now;
            string Wtype = "py";

            string MySavePath = LearnSite.Common.WorkUpload.GetWurl(Wyear, Wgrade, Wclass, Wcid, Wmid);//获得作品保存路径（如果不存在，自动创建）
            string RndTime = LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate);
            string OnlyFileName = Wnum + "_" + Wcid + "_" + Wmid + "_" + RndTime;
            string NewFileName = OnlyFileName + "."+Wtype;
            string Wurl = MySavePath + "/" + NewFileName;
            string SaveFile = HttpContext.Current.Server.MapPath(Wurl);

            string NewThumbnail = OnlyFileName + ".png";
            string Wthumbnail = MySavePath + "/" + NewThumbnail;
            string thumbnailpath = HttpContext.Current.Server.MapPath(Wthumbnail);
            int flen = 0;
            int plen = 0;
            string title = "";
            string codefile = "";
            string codedict = "";
            BLL.Works bll = new BLL.Works();
            Model.Works model = new Model.Works();//////////////////
            model = bll.GetModelByStu(Int32.Parse(Wmid), Wnum);
            bool saved = false;
            bool passed = false;
            if (model != null)
                saved = model.Wcheck;//判断作品是否评价过

            if (!saved)
            {
                try
                {
                    HttpPostedFile pngf = HttpContext.Current.Request.Files["cover"];
                    plen = pngf.ContentLength;
                    codefile = HttpContext.Current.Request.Form["codefile"];
                    string pass = HttpContext.Current.Request.Form["pass"];
                    if (pass == "1" || pass == "2") passed = true;//如果标志为1,则表示程序检验通过

                    if (!String.IsNullOrEmpty(codefile))
                    {
                        byte[] c = Convert.FromBase64String(codefile);
                        string cf = System.Text.Encoding.Default.GetString(c);
                        cf = HttpUtility.UrlDecode(cf);//HttpUtility.UrlDecode 等于js中的decodeURIComponent

                        pngf.SaveAs(thumbnailpath);

                        System.IO.File.WriteAllText(SaveFile, cf);
                        flen = codefile.Length;

                    }
                    codedict = HttpContext.Current.Request.Form["codedict"];

                    string msg = "图片大小" + plen.ToString() + "代码" + codefile + "记录" + codedict;

                   // LearnSite.Common.Log.Addlog("python作品上传调试信息：", msg);
                }
                catch (Exception ec)
                {
                    LearnSite.Common.Log.Addlog("保存出错信息", ec.StackTrace);// ec.StackTrace可以出详细信息
                }

                bool checkcan = true;

                //string Wid = bll.WorkDone(Wnum, Int32.Parse(Wcid), Int32.Parse(Wmid));//返回空字符表示不存在该记录
                //记录到数据库
                if (model != null)
                {
                    //程序检测未通过的才更新，已经完成并通过的不更新
                    if (passed)
                    {
                        bll.UpdatepythonUpIp(model.Wid, Wurl, NewFileName, flen, Wdate, checkcan, Wthumbnail, title, codefile, codedict, passed, Wtype, Wip);//更新Wfilename, Wurl,Wlength, Wdate  
                    }
                    else
                    {
                        bll.UpdatepythonUp(model.Wid, Wurl, NewFileName, flen, Wdate, checkcan, Wthumbnail, title, codefile, codedict, passed, Wtype);//更新Wfilename, Wurl,Wlength, Wdate                    
                    }
                }
                else
                {
                    //Wnum,Wcid,Wmid,Wmsort,Wfilename,Wtype,Wurl,Wlength,Wdate,Wip
                    //,Wtime,Wegg,Wgrade,Wterm,Woffice,Wflash,Wsid,Wclass,Wname,Wyear
                    Model.Works wmodel = new Model.Works();//////////////////
                    wmodel.Wnum = Wnum;
                    wmodel.Wcid = Int32.Parse(Wcid);
                    wmodel.Wmid = Int32.Parse(Wmid);
                    wmodel.Wmsort = 5;
                    wmodel.Wfilename = NewFileName;
                    wmodel.Wtype = Wtype;
                    wmodel.Wurl = Wurl;
                    wmodel.Wlength = flen;
                    wmodel.Wdate = Wdate;
                    wmodel.Wip = Wip;
                    wmodel.Wtime = Wtime;
                    wmodel.Wcan = checkcan;
                    wmodel.Wcheck = false;
                    wmodel.Wegg = 12;//设定票数为12张
                    wmodel.Whit = 0;
                    wmodel.Wgrade = Int32.Parse(Wgrade);
                    wmodel.Wterm = Int32.Parse(Wterm);
                    wmodel.Woffice = false;
                    wmodel.Wsid = Wsid;
                    wmodel.Wclass = Int32.Parse(Wclass);
                    wmodel.Wname = HttpUtility.UrlDecode(Wname);
                    wmodel.Wyear = Int32.Parse(Wyear);
                    wmodel.Wflash = false;
                    wmodel.Werror = false;
                    wmodel.Wthumbnail = Wthumbnail;
                    wmodel.Wtitle = title;
                    wmodel.Wcode = codefile;
                    wmodel.Wdict = codedict;
                    wmodel.Wpass = passed;
                    if (passed) wmodel.Wscore = 10;
                    else wmodel.Wscore = 0;
                    wmodel.Wlid = Int32.Parse(Wlid);

                    bll.AddPythonUp(wmodel);//添加作品提交记录
                    BLL.Signin sn = new BLL.Signin();
                    sn.UpdateQwork(Wsid, Int32.Parse(Wcid));//更新今天签到表中的作品数量

                    //添加课堂活动记录
                    Model.MenuWorks kmodel = new Model.MenuWorks();
                    kmodel.Klid = Int32.Parse(Wlid);
                    kmodel.Ksid = Wsid;
                    kmodel.Ktime = LearnSite.Common.Computer.GoneMinute(DateTime.Parse(Wtime), Wdate);
                    kmodel.Kseconds = Int32.Parse(LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate));
                    kmodel.Kcheck = false;
                    BLL.MenuWorks kbll = new MenuWorks();
                    kbll.Add(kmodel);
                }
            }

        }

        public void SaveBlock(string id)
        {
            string[] arrayid = id.Split('-');
            string Wcid = arrayid[0];
            string Wmid = arrayid[1];
            string Wlid = arrayid[2];

            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

            int Wsid = cook.Sid;
            string Wnum = cook.Snum;
            string Wname = cook.Sname;
            string Wgrade = cook.Sgrade.ToString();
            string Wclass = cook.Sclass.ToString();
            string Wyear = cook.Syear.ToString();
            string Wterm = cook.ThisTerm.ToString();
            string Wip = cook.LoginIp;
            string Wtime = cook.LoginTime;
            DateTime Wdate = DateTime.Now;
            string Wtype = "py";

            string MySavePath = LearnSite.Common.WorkUpload.GetWurl(Wyear, Wgrade, Wclass, Wcid, Wmid);//获得作品保存路径（如果不存在，自动创建）
            string RndTime = LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate);
            string OnlyFileName = Wnum + "_" + Wcid + "_" + Wmid + "_" + RndTime;
            string NewFileName = OnlyFileName + "." + Wtype;
            string Wurl = MySavePath + "/" + NewFileName;
            string SaveFile = HttpContext.Current.Server.MapPath(Wurl);

            string NewThumbnail = OnlyFileName + ".png";
            string Wthumbnail = MySavePath + "/" + NewThumbnail;
            string thumbnailpath = HttpContext.Current.Server.MapPath(Wthumbnail);
            int flen = 0;
            int plen = 0;
            string title = "";
            string codefile = "";
            string codedict = "";
            BLL.Works bll = new BLL.Works();
            Model.Works model = new Model.Works();//////////////////
            model = bll.GetModelByStu(Int32.Parse(Wmid), Wnum);
            bool saved = false;
            bool passed = false;
            if (model != null)
                saved = model.Wcheck;//判断作品是否评价过

            if (!saved)
            {
                try
                {
                    HttpPostedFile pngf = HttpContext.Current.Request.Files["cover"];
                    plen = pngf.ContentLength;
                    codefile = HttpContext.Current.Request.Form["codefile"];
                    string pass = HttpContext.Current.Request.Form["pass"];
                    if (pass == "1" || pass == "2" || pass == "3") passed = true;//如果标志为1,则表示程序检验通过

                    if (!String.IsNullOrEmpty(codefile))
                    {
                        byte[] c = Convert.FromBase64String(codefile);
                        string cf = System.Text.Encoding.Default.GetString(c);
                        cf = HttpUtility.UrlDecode(cf);//HttpUtility.UrlDecode 等于js中的decodeURIComponent

                        pngf.SaveAs(thumbnailpath);

                        System.IO.File.WriteAllText(SaveFile, cf);
                        flen = codefile.Length;

                    }
                    codedict = HttpContext.Current.Request.Form["codedict"];

                    string msg = "图片大小" + plen.ToString() + "代码" + codefile + "记录" + codedict;

                    // LearnSite.Common.Log.Addlog("python作品上传调试信息：", msg);
                }
                catch (Exception ec)
                {
                    LearnSite.Common.Log.Addlog("保存出错信息", ec.StackTrace);// ec.StackTrace可以出详细信息
                }

                bool checkcan = true;

                //string Wid = bll.WorkDone(Wnum, Int32.Parse(Wcid), Int32.Parse(Wmid));//返回空字符表示不存在该记录
                //记录到数据库
                if (model != null)
                {
                    bll.UpdatepythonUp(model.Wid, Wurl, NewFileName, flen, Wdate, checkcan, Wthumbnail, title, codefile, codedict, passed,Wtype);//更新Wfilename, Wurl,Wlength, Wdate
                }
                else
                {
                    //Wnum,Wcid,Wmid,Wmsort,Wfilename,Wtype,Wurl,Wlength,Wdate,Wip
                    //,Wtime,Wegg,Wgrade,Wterm,Woffice,Wflash,Wsid,Wclass,Wname,Wyear
                    Model.Works wmodel = new Model.Works();//////////////////
                    wmodel.Wnum = Wnum;
                    wmodel.Wcid = Int32.Parse(Wcid);
                    wmodel.Wmid = Int32.Parse(Wmid);
                    wmodel.Wmsort = 5;
                    wmodel.Wfilename = NewFileName;
                    wmodel.Wtype = Wtype;
                    wmodel.Wurl = Wurl;
                    wmodel.Wlength = flen;
                    wmodel.Wdate = Wdate;
                    wmodel.Wip = Wip;
                    wmodel.Wtime = Wtime;
                    wmodel.Wcan = checkcan;
                    wmodel.Wcheck = false;
                    wmodel.Wegg = 12;//设定票数为12张
                    wmodel.Whit = 0;
                    wmodel.Wgrade = Int32.Parse(Wgrade);
                    wmodel.Wterm = Int32.Parse(Wterm);
                    wmodel.Woffice = false;
                    wmodel.Wsid = Wsid;
                    wmodel.Wclass = Int32.Parse(Wclass);
                    wmodel.Wname = HttpUtility.UrlDecode(Wname);
                    wmodel.Wyear = Int32.Parse(Wyear);
                    wmodel.Wflash = false;
                    wmodel.Werror = false;
                    wmodel.Wthumbnail = Wthumbnail;
                    wmodel.Wtitle = title;
                    wmodel.Wcode = codefile;
                    wmodel.Wdict = codedict;
                    wmodel.Wpass = passed;
                    if (passed) wmodel.Wscore = 10;
                    else wmodel.Wscore = 0;
                    wmodel.Wlid = Int32.Parse(Wlid);

                    bll.AddPythonUp(wmodel);//添加作品提交记录
                    BLL.Signin sn = new BLL.Signin();
                    sn.UpdateQwork(Wsid, Int32.Parse(Wcid));//更新今天签到表中的作品数量

                    //添加课堂活动记录
                    Model.MenuWorks kmodel = new Model.MenuWorks();
                    kmodel.Klid = Int32.Parse(Wlid);
                    kmodel.Ksid = Wsid;
                    kmodel.Ktime = LearnSite.Common.Computer.GoneMinute(DateTime.Parse(Wtime), Wdate);
                    kmodel.Kseconds = Int32.Parse(LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate));
                    kmodel.Kcheck = false;
                    BLL.MenuWorks kbll = new MenuWorks();
                    kbll.Add(kmodel);
                }
            }

        }

        public void SaveHtml(string id)
        {
            string[] arrayid = id.Split('-');
            string Wcid = arrayid[0];
            string Wmid = arrayid[1];
            string Wlid = arrayid[2];

            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

            int Wsid = cook.Sid;
            string Wnum = cook.Snum;
            string Wname = cook.Sname;
            string Wgrade = cook.Sgrade.ToString();
            string Wclass = cook.Sclass.ToString();
            string Wyear = cook.Syear.ToString();
            string Wterm = cook.ThisTerm.ToString();
            string Wip = cook.LoginIp;
            string Wtime = cook.LoginTime;
            DateTime Wdate = DateTime.Now;

            string mypage = HttpContext.Current.Request.Form["mypage"];

            string MyWebPath = LearnSite.Common.WorkUpload.GetWeb(Wnum);//获得网页保存路径（如果不存在，自动创建）
            string NewFileName = mypage;

            string Wurl = MyWebPath + "/" + NewFileName;
            string SaveFile = HttpContext.Current.Server.MapPath(Wurl);
            string[] htmlname = mypage.Split('.');
            string Wthumbnail = "";

            int flen = 0;
            string title = "";
            string codefile = "";
            BLL.Works bll = new BLL.Works();
            Model.Works model = new Model.Works();//////////////////
            model = bll.GetModelByStu(Int32.Parse(Wmid), Wnum);
            bool saved = false;
            if (model != null)
                saved = model.Wcheck;//判断作品是否评价过

            if (!saved)
            {
                try
                {
                    codefile = HttpContext.Current.Request.Form["codefile"];
                    HttpPostedFile cover = HttpContext.Current.Request.Files["cover"];

                    if (!String.IsNullOrEmpty(codefile))
                    {
                        byte[] c = Convert.FromBase64String(codefile);
                        string cf = System.Text.Encoding.UTF8.GetString(c);
                        cf = HttpUtility.UrlDecode(cf);//HttpUtility.UrlDecode 等于js中的decodeURIComponent

                        System.IO.File.WriteAllText(SaveFile, cf, Encoding.UTF8);
                        flen = codefile.Length;

                        if (cover.ContentLength > 0)
                        {
                            Wthumbnail = MyWebPath + "/" + htmlname[0] + ".ico";
                            string thumbnailpath = HttpContext.Current.Server.MapPath(Wthumbnail);
                            cover.SaveAs(thumbnailpath);
                        }
                    }
                    // LearnSite.Common.Log.Addlog("python作品上传调试信息：", msg);
                }
                catch (Exception ec)
                {
                    LearnSite.Common.Log.Addlog("保存出错信息", ec.StackTrace);// ec.StackTrace可以出详细信息
                }

                bool checkcan = true;

                //string Wid = bll.WorkDone(Wnum, Int32.Parse(Wcid), Int32.Parse(Wmid));//返回空字符表示不存在该记录
                //记录到数据库
                if (model != null)
                {
                    bll.UpdateHtml(model.Wid, Wurl, NewFileName, flen, Wdate, checkcan, Wthumbnail,title,codefile);//更新Wfilename, Wurl,Wlength, Wdate
                }
                else
                {
                    //Wnum,Wcid,Wmid,Wmsort,Wfilename,Wtype,Wurl,Wlength,Wdate,Wip
                    //,Wtime,Wegg,Wgrade,Wterm,Woffice,Wflash,Wsid,Wclass,Wname,Wyear
                    Model.Works wmodel = new Model.Works();//////////////////
                    wmodel.Wnum = Wnum;
                    wmodel.Wcid = Int32.Parse(Wcid);
                    wmodel.Wmid = Int32.Parse(Wmid);
                    wmodel.Wmsort = 12;
                    wmodel.Wfilename = NewFileName;
                    wmodel.Wtype = "html";
                    wmodel.Wurl = Wurl;
                    wmodel.Wlength = flen;
                    wmodel.Wdate = Wdate;
                    wmodel.Wip = Wip;
                    wmodel.Wtime = Wtime;
                    wmodel.Wcan = checkcan;
                    wmodel.Wcheck = false;
                    wmodel.Wegg = 12;//设定票数为12张
                    wmodel.Whit = 0;
                    wmodel.Wgrade = Int32.Parse(Wgrade);
                    wmodel.Wterm = Int32.Parse(Wterm);
                    wmodel.Woffice = false;
                    wmodel.Wsid = Wsid;
                    wmodel.Wclass = Int32.Parse(Wclass);
                    wmodel.Wname = HttpUtility.UrlDecode(Wname);
                    wmodel.Wyear = Int32.Parse(Wyear);
                    wmodel.Wflash = false;
                    wmodel.Werror = false;
                    wmodel.Wtitle = title;
                    wmodel.Wcode = codefile;
                    wmodel.Wlid = Int32.Parse(Wlid);

                    bll.AddWorkUp(wmodel);//添加作品提交记录
                    BLL.Signin sn = new BLL.Signin();
                    sn.UpdateQwork(Wsid, Int32.Parse(Wcid));//更新今天签到表中的作品数量

                    //添加课堂活动记录
                    Model.MenuWorks kmodel = new Model.MenuWorks();
                    kmodel.Klid = Int32.Parse(Wlid);
                    kmodel.Ksid = Wsid;
                    kmodel.Ktime = LearnSite.Common.Computer.GoneMinute(DateTime.Parse(Wtime), Wdate);
                    kmodel.Kseconds = Int32.Parse(LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate));
                    kmodel.Kcheck = false;
                    BLL.MenuWorks kbll = new MenuWorks();
                    kbll.Add(kmodel);
                }
            }

        }
        public void SaveGraph(string id)
        {
            string[] arrayid = id.Split('-');
            string Wcid = arrayid[0];
            string Wmid = arrayid[1];
            string Wlid = arrayid[2];

            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

            int Wsid = cook.Sid;
            string Wnum = cook.Snum;
            string Wname = cook.Sname;
            string Wgrade = cook.Sgrade.ToString();
            string Wclass = cook.Sclass.ToString();
            string Wyear = cook.Syear.ToString();
            string Wterm = cook.ThisTerm.ToString();
            string Wip = cook.LoginIp;
            string Wtime = cook.LoginTime;
            DateTime Wdate = DateTime.Now;

            string MySavePath = LearnSite.Common.WorkUpload.GetWurl(Wyear, Wgrade, Wclass, Wcid, Wmid);//获得作品保存路径（如果不存在，自动创建）
            string RndTime = LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate);
            string OnlyFileName = Wnum + "_" + Wcid + "_" + Wmid +"_" + RndTime;
            string NewFileName = OnlyFileName + ".xml";
            string Wurl = MySavePath + "/" + NewFileName;
            string SaveFile = HttpContext.Current.Server.MapPath(Wurl);
            string eNewFileName = OnlyFileName + "e.xml";
            string eWurl = MySavePath + "/" + eNewFileName;
            string eSaveFile = HttpContext.Current.Server.MapPath(eWurl);

            string NewThumbnail = OnlyFileName + ".png";
            string Wthumbnail = MySavePath + "/" + NewThumbnail;
            string thumbnailpath = HttpContext.Current.Server.MapPath(Wthumbnail);
            int flen = 0;
            int plen = 0;
            string title = "";
            string codefile = "";
            string codedict = "";
            BLL.Works bll = new BLL.Works();
            Model.Works model = new Model.Works();//////////////////
            model = bll.GetModelByStu(Int32.Parse(Wmid), Wnum);
            bool saved = false;
            if (model != null)
                saved = model.Wcheck;//判断作品是否评价过

            if (!saved)
            {
                try
                {
                    codefile = HttpContext.Current.Request.Form["xml"];
                    codedict = HttpContext.Current.Request.Form["exml"];
                    string width = HttpContext.Current.Request.Form["w"];
                    string height = HttpContext.Current.Request.Form["h"];

                    if (!String.IsNullOrEmpty(codefile))
                    {
                        string cf = HttpUtility.UrlDecode(codefile);//HttpUtility.UrlDecode 等于js中的decodeURIComponent                        
                        System.IO.File.WriteAllText(SaveFile, cf);
                        flen = codefile.Length;

                        if (codedict != null && width != null && height != null )
                        {
                            string cd = HttpUtility.UrlDecode(codedict);
                            int w = int.Parse(width);
                            int h = int.Parse(height);
                            Color background =Color.Transparent;
                            Image image = mxUtils.CreateImage(w, h, background);
                            Graphics g = Graphics.FromImage(image);
                            g.SmoothingMode = SmoothingMode.HighQuality;
                            mxSaxOutputHandler handler = new mxSaxOutputHandler(new mxGdiCanvas2D(g));
                            handler.Read(new XmlTextReader(new StringReader(cd)));

                            image.Save(thumbnailpath, ImageFormat.Png);
                        }
                    }

                    string msg = "图片大小" + plen.ToString() + "代码" + codefile + "记录" + codedict;

                    // LearnSite.Common.Log.Addlog("python作品上传调试信息：", msg);
                }
                catch (Exception ec)
                {
                    LearnSite.Common.Log.Addlog("保存出错信息", ec.StackTrace);// ec.StackTrace可以出详细信息
                }

                bool checkcan = true;

                //string Wid = bll.WorkDone(Wnum, Int32.Parse(Wcid), Int32.Parse(Wmid));//返回空字符表示不存在该记录
                //记录到数据库
                if (model != null)
                {
                    bll.UpdategraphUp(model.Wid, Wurl, NewFileName, flen, Wdate, checkcan, Wthumbnail, title, codefile, codedict);//更新Wfilename, Wurl,Wlength, Wdate

                }
                else
                {
                    //Wnum,Wcid,Wmid,Wmsort,Wfilename,Wtype,Wurl,Wlength,Wdate,Wip
                    //,Wtime,Wegg,Wgrade,Wterm,Woffice,Wflash,Wsid,Wclass,Wname,Wyear
                    Model.Works wmodel = new Model.Works();//////////////////
                    wmodel.Wnum = Wnum;
                    wmodel.Wcid = Int32.Parse(Wcid);
                    wmodel.Wmid = Int32.Parse(Wmid);
                    wmodel.Wmsort = 10;
                    wmodel.Wfilename = NewFileName;
                    wmodel.Wtype = "xml";
                    wmodel.Wurl = Wurl;
                    wmodel.Wlength = flen;
                    wmodel.Wdate = Wdate;
                    wmodel.Wip = Wip;
                    wmodel.Wtime = Wtime;
                    wmodel.Wcan = checkcan;
                    wmodel.Wcheck = false;
                    wmodel.Wegg = 12;//设定票数为12张
                    wmodel.Whit = 0;
                    wmodel.Wgrade = Int32.Parse(Wgrade);
                    wmodel.Wterm = Int32.Parse(Wterm);
                    wmodel.Woffice = false;
                    wmodel.Wsid = Wsid;
                    wmodel.Wclass = Int32.Parse(Wclass);
                    wmodel.Wname = HttpUtility.UrlDecode(Wname);
                    wmodel.Wyear = Int32.Parse(Wyear);
                    wmodel.Wflash = false;
                    wmodel.Werror = false;
                    wmodel.Wthumbnail = Wthumbnail;
                    wmodel.Wtitle = title;
                    wmodel.Wcode = codefile;
                    wmodel.Wdict = codedict;
                    wmodel.Wlid = Int32.Parse(Wlid);

                    bll.AddWorkUp(wmodel);//添加作品提交记录
                    BLL.Signin sn = new BLL.Signin();
                    sn.UpdateQwork(Wsid, Int32.Parse(Wcid));//更新今天签到表中的作品数量

                    //添加课堂活动记录
                    Model.MenuWorks kmodel = new Model.MenuWorks();
                    kmodel.Klid = Int32.Parse(Wlid);
                    kmodel.Ksid = Wsid;
                    kmodel.Ktime = LearnSite.Common.Computer.GoneMinute(DateTime.Parse(Wtime), Wdate);
                    kmodel.Kseconds = Int32.Parse(LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate));
                    kmodel.Kcheck = false;
                    BLL.MenuWorks kbll = new MenuWorks();
                    kbll.Add(kmodel);
                }
            }

        }


        public void SaveBlockpy(string id)
        {
            string[] arrayid = id.Split('-');
            string Wcid = arrayid[0];
            string Wmid = arrayid[1];
            string Wlid = arrayid[2];

            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

            int Wsid = cook.Sid;
            string Wnum = cook.Snum;
            string Wname = cook.Sname;
            string Wgrade = cook.Sgrade.ToString();
            string Wclass = cook.Sclass.ToString();
            string Wyear = cook.Syear.ToString();
            string Wterm = cook.ThisTerm.ToString();
            string Wip = cook.LoginIp;
            string Wtime = cook.LoginTime;
            DateTime Wdate = DateTime.Now;
            string Wtype = "block";

            string MySavePath = LearnSite.Common.WorkUpload.GetWurl(Wyear, Wgrade, Wclass, Wcid, Wmid);//获得作品保存路径（如果不存在，自动创建）
            string RndTime = LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate);
            string OnlyFileName = Wnum + "_" + Wcid + "_" + Wmid + "_" + RndTime;
            string NewFileName = OnlyFileName + "." + Wtype;
            string Wurl = MySavePath + "/" + NewFileName;
            string SaveFile = HttpContext.Current.Server.MapPath(Wurl);

            string NewThumbnail = OnlyFileName + ".png";
            string Wthumbnail = MySavePath + "/" + NewThumbnail;
            string thumbnailpath = HttpContext.Current.Server.MapPath(Wthumbnail);
            int flen = 0;
            int plen = 0;
            string title = "";
            string codefile = "";
            string codedict = "";
            BLL.Works bll = new BLL.Works();
            Model.Works model = new Model.Works();//////////////////
            model = bll.GetModelByStu(Int32.Parse(Wmid), Wnum);
            bool saved = false;
            bool passed = false;
            if (model != null)
                saved = model.Wcheck;//判断作品是否评价过

            if (!saved)
            {
                //LearnSite.Common.Log.Addlog("1保存路径信息:", thumbnailpath);

                codefile = HttpContext.Current.Request.Form["xml"];
                string pass = HttpContext.Current.Request.Form["pass"];
                HttpPostedFile pngf = HttpContext.Current.Request.Files["cover"];
                if (pass == "1" || pass == "2")
                {
                    passed = true;//如果标志为1,则表示程序检验通过
                }

                //string msga = "代码长度：" + codefile.Length.ToString() + "检测:" + pass + "缩略图:" + pngf.ContentLength.ToString();
                //LearnSite.Common.Log.Addlog("2保存信息:", msga);
                plen = pngf.ContentLength;

                try
                {
                    if (plen > 0)
                    {
                        pngf.SaveAs(thumbnailpath);
                        //LearnSite.Common.Log.Addlog("3保存成功信息", "保存成功");// ec.StackTrace可以出详细信息
                    }
                }
                catch (Exception ec)
                {
                    string msgstr = pass + "\n" + codefile + "\n" + thumbnailpath + "\n 图片大小：" + plen;
                    LearnSite.Common.Log.Addlog("4保存出错信息", ec.StackTrace + msgstr);// ec.StackTrace可以出详细信息
                }

                bool checkcan = true;

                if (model != null)
                {
                    bll.UpdatepythonUp(model.Wid, Wurl, NewFileName, flen, Wdate, checkcan, Wthumbnail, title, codefile, codedict, passed, Wtype);//更新Wfilename, Wurl,Wlength, Wdate

                }
                else
                {
                    Model.Works wmodel = new Model.Works();
                    wmodel.Wnum = Wnum;
                    wmodel.Wcid = Int32.Parse(Wcid);
                    wmodel.Wmid = Int32.Parse(Wmid);
                    wmodel.Wmsort = 10;
                    wmodel.Wfilename = NewFileName;
                    wmodel.Wtype = Wtype;
                    wmodel.Wurl = Wurl;
                    wmodel.Wlength = flen;
                    wmodel.Wdate = Wdate;
                    wmodel.Wip = Wip;
                    wmodel.Wtime = Wtime;
                    wmodel.Wcan = checkcan;
                    wmodel.Wcheck = false;
                    wmodel.Wegg = 12;//设定票数为12张
                    wmodel.Whit = 0;
                    wmodel.Wgrade = Int32.Parse(Wgrade);
                    wmodel.Wterm = Int32.Parse(Wterm);
                    wmodel.Woffice = false;
                    wmodel.Wsid = Wsid;
                    wmodel.Wclass = Int32.Parse(Wclass);
                    wmodel.Wname = HttpUtility.UrlDecode(Wname);
                    wmodel.Wyear = Int32.Parse(Wyear);
                    wmodel.Wflash = false;
                    wmodel.Werror = false;
                    wmodel.Wthumbnail = Wthumbnail;
                    wmodel.Wtitle = title;
                    wmodel.Wcode = codefile;
                    wmodel.Wdict = codedict;
                    wmodel.Wlid = Int32.Parse(Wlid);

                    bll.AddWorkUp(wmodel);//添加作品提交记录
                    BLL.Signin sn = new BLL.Signin();
                    sn.UpdateQwork(Wsid, Int32.Parse(Wcid));//更新今天签到表中的作品数量

                    //添加课堂活动记录
                    Model.MenuWorks kmodel = new Model.MenuWorks();
                    kmodel.Klid = Int32.Parse(Wlid);
                    kmodel.Ksid = Wsid;
                    kmodel.Ktime = LearnSite.Common.Computer.GoneMinute(DateTime.Parse(Wtime), Wdate);
                    kmodel.Kseconds = Int32.Parse(LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate));
                    kmodel.Kcheck = false;
                    BLL.MenuWorks kbll = new MenuWorks();
                    kbll.Add(kmodel);
                }
            }

        }


        public void SaveMpptx(string id)
        {
            string[] arrayid = id.Split('-');
            string Wcid = arrayid[0];
            string Wmid = arrayid[1];
            string Wlid = arrayid[2];

            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

            int Wsid = cook.Sid;
            string Wnum = cook.Snum;
            string Wname = cook.Sname;
            string Wgrade = cook.Sgrade.ToString();
            string Wclass = cook.Sclass.ToString();
            string Wyear = cook.Syear.ToString();
            string Wterm = cook.ThisTerm.ToString();
            string Wip = cook.LoginIp;
            string Wtime = cook.LoginTime;
            DateTime Wdate = DateTime.Now;

            string MySavePath = LearnSite.Common.WorkUpload.GetWurl(Wyear, Wgrade, Wclass, Wcid, Wmid);//获得作品保存路径（如果不存在，自动创建）
            string RndTime = LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate);
            string OnlyFileName = Wnum + "_" + Wcid + "_" + Wmid + "_" + RndTime;

            string Wthumbnail = MySavePath + "/" + OnlyFileName + ".png";
            string thumbnailpath = HttpContext.Current.Server.MapPath(Wthumbnail);

            string NewFileName = OnlyFileName + ".mpptx";
            string Wurl = MySavePath + "/" + NewFileName;
            string SaveFile = HttpContext.Current.Server.MapPath(Wurl);

            int flen = 0;
            int plen = 0;
            string title = "";
            string codefile = "";
            string codedict = "";
            string Ext = "mpptx";
            BLL.Works bll = new BLL.Works();
            Model.Works model = new Model.Works();//////////////////
            model = bll.GetModelByStu(Int32.Parse(Wmid), Wnum);
            bool saved = false;
            if (model != null)
                saved = model.Wcheck;//判断作品是否评价过

            if (!saved)
            {
                try
                {
                    HttpPostedFile pptfile = HttpContext.Current.Request.Files["myppt"];
                    HttpPostedFile cover = HttpContext.Current.Request.Files["cover"];
                    plen = pptfile.ContentLength;
                    if (plen > 0)
                    {
                        cover.SaveAs(thumbnailpath);
                        pptfile.SaveAs(SaveFile);
                        //LearnSite.Common.Log.Addlog("3保存成功信息", "保存成功");// ec.StackTrace可以出详细信息
                    }


                    // LearnSite.Common.Log.Addlog("python作品上传调试信息：", msg);
                }
                catch (Exception ec)
                {
                    LearnSite.Common.Log.Addlog("保存出错信息", ec.StackTrace);// ec.StackTrace可以出详细信息
                }

                bool checkcan = true;

                //string Wid = bll.WorkDone(Wnum, Int32.Parse(Wcid), Int32.Parse(Wmid));//返回空字符表示不存在该记录
                //记录到数据库
                if (model != null)
                {
                    bll.UpdateTopic(model.Wid, Wurl, NewFileName, flen, Wdate, checkcan, Wthumbnail, title, codefile, Ext);//更新Wfilename, Wurl,Wlength, Wdate

                }
                else
                {
                    //Wnum,Wcid,Wmid,Wmsort,Wfilename,Wtype,Wurl,Wlength,Wdate,Wip
                    //,Wtime,Wegg,Wgrade,Wterm,Woffice,Wflash,Wsid,Wclass,Wname,Wyear
                    Model.Works wmodel = new Model.Works();//////////////////
                    wmodel.Wnum = Wnum;
                    wmodel.Wcid = Int32.Parse(Wcid);
                    wmodel.Wmid = Int32.Parse(Wmid);
                    wmodel.Wmsort = 15;
                    wmodel.Wfilename = NewFileName;
                    wmodel.Wtype = Ext;
                    wmodel.Wurl = Wurl;
                    wmodel.Wlength = flen;
                    wmodel.Wdate = Wdate;
                    wmodel.Wip = Wip;
                    wmodel.Wtime = Wtime;
                    wmodel.Wcan = checkcan;
                    wmodel.Wcheck = false;
                    wmodel.Wegg = 12;//设定票数为12张
                    wmodel.Whit = 0;
                    wmodel.Wgrade = Int32.Parse(Wgrade);
                    wmodel.Wterm = Int32.Parse(Wterm);
                    wmodel.Woffice = false;
                    wmodel.Wsid = Wsid;
                    wmodel.Wclass = Int32.Parse(Wclass);
                    wmodel.Wname = HttpUtility.UrlDecode(Wname);
                    wmodel.Wyear = Int32.Parse(Wyear);
                    wmodel.Wflash = false;
                    wmodel.Werror = false;
                    wmodel.Wtitle = title;
                    wmodel.Wcode = codefile;
                    wmodel.Wdict = codedict;
                    wmodel.Wlid = Int32.Parse(Wlid);
                    wmodel.Wthumbnail = Wthumbnail;

                    bll.AddWorkUp(wmodel);//添加作品提交记录
                    BLL.Signin sn = new BLL.Signin();
                    sn.UpdateQwork(Wsid, Int32.Parse(Wcid));//更新今天签到表中的作品数量

                    //添加课堂活动记录
                    Model.MenuWorks kmodel = new Model.MenuWorks();
                    kmodel.Klid = Int32.Parse(Wlid);
                    kmodel.Ksid = Wsid;
                    kmodel.Ktime = LearnSite.Common.Computer.GoneMinute(DateTime.Parse(Wtime), Wdate);
                    kmodel.Kseconds = Int32.Parse(LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate));
                    kmodel.Kcheck = false;
                    BLL.MenuWorks kbll = new MenuWorks();
                    kbll.Add(kmodel);
                }
            }

        }

        public void SavePptist(string id)
        {
            string[] arrayid = id.Split('-');//-
            string Wcid = arrayid[0];
            string Wmid = arrayid[1];
            string Wlid = arrayid[2];

            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

            int Wsid = cook.Sid;
            string Wnum = cook.Snum;
            string Wname = cook.Sname;
            string Wgrade = cook.Sgrade.ToString();
            string Wclass = cook.Sclass.ToString();
            string Wyear = cook.Syear.ToString();
            string Wterm = cook.ThisTerm.ToString();
            string Wip = cook.LoginIp;
            string Wtime = cook.LoginTime;
            DateTime Wdate = DateTime.Now;

            string MySavePath = LearnSite.Common.WorkUpload.GetWurl(Wyear, Wgrade, Wclass, Wcid, Wmid);//获得作品保存路径（如果不存在，自动创建）
            string RndTime = LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate);
            string OnlyFileName = Wnum + "_" + Wcid + "_" + Wmid + "_" + RndTime;

            string Wthumbnail = MySavePath + "/" + OnlyFileName + ".png";
            string thumbnailpath = HttpContext.Current.Server.MapPath(Wthumbnail);

            string NewFileName = OnlyFileName + ".pptist";
            string Wurl = MySavePath + "/" + NewFileName;
            string SaveFile = HttpContext.Current.Server.MapPath(Wurl);

            int flen = 0;
            int plen = 0;
            string title = "";
            string codefile = "";
            string codedict = "";
            string Ext = "pptist";
            BLL.Works bll = new BLL.Works();
            Model.Works model = new Model.Works();//////////////////
            model = bll.GetModelByStu(Int32.Parse(Wmid), Wnum);
            bool saved = false;
            if (model != null)
                saved = model.Wcheck;//判断作品是否评价过

            if (!saved)
            {
                try
                {
                    HttpPostedFile pptfile = HttpContext.Current.Request.Files["myppt"];
                    HttpPostedFile cover = HttpContext.Current.Request.Files["cover"];
                    plen = pptfile.ContentLength;
                    if (plen > 0)
                    {
                        cover.SaveAs(thumbnailpath);
                        pptfile.SaveAs(SaveFile);
                        //LearnSite.Common.Log.Addlog("3保存成功信息", "保存成功");// ec.StackTrace可以出详细信息
                    }


                    // LearnSite.Common.Log.Addlog("python作品上传调试信息：", msg);
                }
                catch (Exception ec)
                {
                    LearnSite.Common.Log.Addlog("保存出错信息", ec.StackTrace);// ec.StackTrace可以出详细信息
                }

                bool checkcan = true;

                //string Wid = bll.WorkDone(Wnum, Int32.Parse(Wcid), Int32.Parse(Wmid));//返回空字符表示不存在该记录
                //记录到数据库
                if (model != null)
                {
                    bll.UpdateTopic(model.Wid, Wurl, NewFileName, flen, Wdate, checkcan, Wthumbnail, title, codefile, Ext);//更新Wfilename, Wurl,Wlength, Wdate

                }
                else
                {
                    //Wnum,Wcid,Wmid,Wmsort,Wfilename,Wtype,Wurl,Wlength,Wdate,Wip
                    //,Wtime,Wegg,Wgrade,Wterm,Woffice,Wflash,Wsid,Wclass,Wname,Wyear
                    Model.Works wmodel = new Model.Works();//////////////////
                    wmodel.Wnum = Wnum;
                    wmodel.Wcid = Int32.Parse(Wcid);
                    wmodel.Wmid = Int32.Parse(Wmid);
                    wmodel.Wmsort = 15;
                    wmodel.Wfilename = NewFileName;
                    wmodel.Wtype = Ext;
                    wmodel.Wurl = Wurl;
                    wmodel.Wlength = flen;
                    wmodel.Wdate = Wdate;
                    wmodel.Wip = Wip;
                    wmodel.Wtime = Wtime;
                    wmodel.Wcan = checkcan;
                    wmodel.Wcheck = false;
                    wmodel.Wegg = 12;//设定票数为12张
                    wmodel.Whit = 0;
                    wmodel.Wgrade = Int32.Parse(Wgrade);
                    wmodel.Wterm = Int32.Parse(Wterm);
                    wmodel.Woffice = false;
                    wmodel.Wsid = Wsid;
                    wmodel.Wclass = Int32.Parse(Wclass);
                    wmodel.Wname = HttpUtility.UrlDecode(Wname);
                    wmodel.Wyear = Int32.Parse(Wyear);
                    wmodel.Wflash = false;
                    wmodel.Werror = false;
                    wmodel.Wtitle = title;
                    wmodel.Wcode = codefile;
                    wmodel.Wdict = codedict;
                    wmodel.Wlid = Int32.Parse(Wlid);
                    wmodel.Wthumbnail = Wthumbnail;

                    bll.AddWorkUp(wmodel);//添加作品提交记录
                    BLL.Signin sn = new BLL.Signin();
                    sn.UpdateQwork(Wsid, Int32.Parse(Wcid));//更新今天签到表中的作品数量

                    //添加课堂活动记录
                    Model.MenuWorks kmodel = new Model.MenuWorks();
                    kmodel.Klid = Int32.Parse(Wlid);
                    kmodel.Ksid = Wsid;
                    kmodel.Ktime = LearnSite.Common.Computer.GoneMinute(DateTime.Parse(Wtime), Wdate);
                    kmodel.Kseconds = Int32.Parse(LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate));
                    kmodel.Kcheck = false;
                    BLL.MenuWorks kbll = new MenuWorks();
                    kbll.Add(kmodel);
                }
            }

        }


        public void SaveMqtt(string id)
        {
            string[] arrayid = id.Split('-');
            string Wcid = arrayid[0];
            string Wmid = arrayid[1];
            string Wlid = arrayid[2];

            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
            int Wsid = cook.Sid;
            string Wnum = cook.Snum;
            string Wname = cook.Sname;
            string Wgrade = cook.Sgrade.ToString();
            string Wclass = cook.Sclass.ToString();
            string Wyear = cook.Syear.ToString();
            string Wterm = cook.ThisTerm.ToString();
            string Wip = cook.LoginIp;
            string Wtime = cook.LoginTime;
            DateTime Wdate = DateTime.Now;

            string MySavePath = LearnSite.Common.WorkUpload.GetWurl(Wyear, Wgrade, Wclass, Wcid, Wmid);//获得作品保存路径（如果不存在，自动创建）
            string RndTime = LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate);
            string OnlyFileName = Wnum + "_" + Wcid + "_" + Wmid + "_" + RndTime;
            string NewThumbnail = OnlyFileName + ".png";
            string Wthumbnail =  MySavePath + "/" + NewThumbnail;
            string thumbnailpath = HttpContext.Current.Server.MapPath(Wthumbnail);

            int flen = 0;
            int plen = 0;
            string NewFileName = "";
            string Wurl = "";
            string title = "";
            string codefile = "";
            string codedict = "";
            string Ext = "";
            BLL.Works bll = new BLL.Works();
            Model.Works model = new Model.Works();//////////////////
            model = bll.GetModelByStu(Int32.Parse(Wmid), Wnum);
            bool saved = false;
            if (model != null)
                saved = model.Wcheck;//判断作品是否评价过

            if (!saved)
            {
                try
                {
                    codefile = HttpContext.Current.Request.Form["content"];
                    Ext = HttpContext.Current.Request.Form["ext"];
                    HttpPostedFile cover = HttpContext.Current.Request.Files["cover"];
                    plen = cover.ContentLength;
                    if (plen > 0)
                    {
                        cover.SaveAs(thumbnailpath);
                    }
                }
                catch (Exception ec)
                {
                    LearnSite.Common.Log.Addlog("保存出错信息", ec.StackTrace);// ec.StackTrace可以出详细信息
                }

                bool checkcan = true;

                //string Wid = bll.WorkDone(Wnum, Int32.Parse(Wcid), Int32.Parse(Wmid));//返回空字符表示不存在该记录
                //记录到数据库
                if (model != null)
                {
                    bll.UpdateTopic(model.Wid, Wurl, NewFileName, flen, Wdate, checkcan, Wthumbnail, title, codefile, Ext);//更新Wfilename, Wurl,Wlength, Wdate

                }
                else
                {
                    //Wnum,Wcid,Wmid,Wmsort,Wfilename,Wtype,Wurl,Wlength,Wdate,Wip
                    //,Wtime,Wegg,Wgrade,Wterm,Woffice,Wflash,Wsid,Wclass,Wname,Wyear
                    Model.Works wmodel = new Model.Works();//////////////////
                    wmodel.Wnum = Wnum;
                    wmodel.Wcid = Int32.Parse(Wcid);
                    wmodel.Wmid = Int32.Parse(Wmid);
                    wmodel.Wmsort = 15;
                    wmodel.Wfilename = NewFileName;
                    wmodel.Wtype = Ext;
                    wmodel.Wurl = Wurl;
                    wmodel.Wlength = flen;
                    wmodel.Wdate = Wdate;
                    wmodel.Wip = Wip;
                    wmodel.Wtime = Wtime;
                    wmodel.Wcan = checkcan;
                    wmodel.Wcheck = false;
                    wmodel.Wegg = 12;//设定票数为12张
                    wmodel.Whit = 0;
                    wmodel.Wgrade = Int32.Parse(Wgrade);
                    wmodel.Wterm = Int32.Parse(Wterm);
                    wmodel.Woffice = false;
                    wmodel.Wsid = Wsid;
                    wmodel.Wclass = Int32.Parse(Wclass);
                    wmodel.Wname = HttpUtility.UrlDecode(Wname);
                    wmodel.Wyear = Int32.Parse(Wyear);
                    wmodel.Wflash = false;
                    wmodel.Werror = false;
                    wmodel.Wtitle = title;
                    wmodel.Wcode = codefile;
                    wmodel.Wdict = codedict;
                    wmodel.Wlid = Int32.Parse(Wlid);
                    wmodel.Wthumbnail = Wthumbnail;

                    bll.AddWorkUp(wmodel);//添加作品提交记录
                    BLL.Signin sn = new BLL.Signin();
                    sn.UpdateQwork(Wsid, Int32.Parse(Wcid));//更新今天签到表中的作品数量

                    //添加课堂活动记录
                    Model.MenuWorks kmodel = new Model.MenuWorks();
                    kmodel.Klid = Int32.Parse(Wlid);
                    kmodel.Ksid = Wsid;
                    kmodel.Ktime = LearnSite.Common.Computer.GoneMinute(DateTime.Parse(Wtime), Wdate);
                    kmodel.Kseconds = Int32.Parse(LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate));
                    kmodel.Kcheck = false;
                    BLL.MenuWorks kbll = new MenuWorks();
                    kbll.Add(kmodel);
                }
            }

        }



        public void SaveFace(string id)
        {
            string[] arrayid = id.Split('-');
            string Wcid = arrayid[0];
            string Wmid = arrayid[1];
            string Wlid = arrayid[2];

            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

            int Wsid = cook.Sid;
            string Wnum = cook.Snum;
            string Wname = cook.Sname;
            string Wgrade = cook.Sgrade.ToString();
            string Wclass = cook.Sclass.ToString();
            string Wyear = cook.Syear.ToString();
            string Wterm = cook.ThisTerm.ToString();
            string Wip = cook.LoginIp;
            string Wtime = cook.LoginTime;
            DateTime Wdate = DateTime.Now;

            string MySavePath = LearnSite.Common.WorkUpload.GetWurl(Wyear, Wgrade, Wclass, Wcid, Wmid);//获得作品保存路径（如果不存在，自动创建）
            string RndTime = LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate);
            string OnlyFileName = Wnum + "_" + Wcid + "_" + Wmid + "_" + RndTime;

            string NewFileName = "";
            string Wthumbnail = MySavePath + "/" + OnlyFileName + ".png";
            string thumbnailpath = HttpContext.Current.Server.MapPath(Wthumbnail);
            string Wurl = Wthumbnail;

            int flen = 0;
            int plen = 0;
            string title = "";
            string codefile = "";
            string codedict = "";
            string Ext = "face";
            BLL.Works bll = new BLL.Works();
            Model.Works model = new Model.Works();//////////////////
            model = bll.GetModelByStu(Int32.Parse(Wmid), Wnum);
            bool saved = false;
            if (model != null)
                saved = model.Wcheck;//判断作品是否评价过

            if (!saved)
            {
                try
                {
                    HttpPostedFile cover = HttpContext.Current.Request.Files["cover"];
                    plen = cover.ContentLength;
                    if (plen > 0)
                    {
                        cover.SaveAs(thumbnailpath);
                    }
                }
                catch (Exception ec)
                {
                    LearnSite.Common.Log.Addlog("保存出错信息", ec.StackTrace);// ec.StackTrace可以出详细信息
                }

                bool checkcan = true;

                //string Wid = bll.WorkDone(Wnum, Int32.Parse(Wcid), Int32.Parse(Wmid));//返回空字符表示不存在该记录
                //记录到数据库
                if (model != null)
                {
                    bll.UpdateTopic(model.Wid, Wurl, NewFileName, flen, Wdate, checkcan, Wthumbnail, title, codefile, Ext);//更新Wfilename, Wurl,Wlength, Wdate

                }
                else
                {
                    //Wnum,Wcid,Wmid,Wmsort,Wfilename,Wtype,Wurl,Wlength,Wdate,Wip
                    //,Wtime,Wegg,Wgrade,Wterm,Woffice,Wflash,Wsid,Wclass,Wname,Wyear
                    Model.Works wmodel = new Model.Works();//////////////////
                    wmodel.Wnum = Wnum;
                    wmodel.Wcid = Int32.Parse(Wcid);
                    wmodel.Wmid = Int32.Parse(Wmid);
                    wmodel.Wmsort = 15;
                    wmodel.Wfilename = NewFileName;
                    wmodel.Wtype = Ext;
                    wmodel.Wurl = Wurl;
                    wmodel.Wlength = flen;
                    wmodel.Wdate = Wdate;
                    wmodel.Wip = Wip;
                    wmodel.Wtime = Wtime;
                    wmodel.Wcan = checkcan;
                    wmodel.Wcheck = false;
                    wmodel.Wegg = 12;//设定票数为12张
                    wmodel.Whit = 0;
                    wmodel.Wgrade = Int32.Parse(Wgrade);
                    wmodel.Wterm = Int32.Parse(Wterm);
                    wmodel.Woffice = false;
                    wmodel.Wsid = Wsid;
                    wmodel.Wclass = Int32.Parse(Wclass);
                    wmodel.Wname = HttpUtility.UrlDecode(Wname);
                    wmodel.Wyear = Int32.Parse(Wyear);
                    wmodel.Wflash = false;
                    wmodel.Werror = false;
                    wmodel.Wtitle = title;
                    wmodel.Wcode = codefile;
                    wmodel.Wdict = codedict;
                    wmodel.Wlid = Int32.Parse(Wlid);
                    wmodel.Wthumbnail = Wthumbnail;

                    bll.AddWorkUp(wmodel);//添加作品提交记录
                    BLL.Signin sn = new BLL.Signin();
                    sn.UpdateQwork(Wsid, Int32.Parse(Wcid));//更新今天签到表中的作品数量

                    //添加课堂活动记录
                    Model.MenuWorks kmodel = new Model.MenuWorks();
                    kmodel.Klid = Int32.Parse(Wlid);
                    kmodel.Ksid = Wsid;
                    kmodel.Ktime = LearnSite.Common.Computer.GoneMinute(DateTime.Parse(Wtime), Wdate);
                    kmodel.Kseconds = Int32.Parse(LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate));
                    kmodel.Kcheck = false;
                    BLL.MenuWorks kbll = new MenuWorks();
                    kbll.Add(kmodel);
                }
            }

        }


        public void SaveModel(string id)
        {
            string[] arrayid = id.Split('-');
            string Wcid = arrayid[0];
            string Wmid = arrayid[1];
            string Wlid = arrayid[2];

            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

            int Wsid = cook.Sid;
            string Wnum = cook.Snum;
            string Wname = cook.Sname;
            string Wgrade = cook.Sgrade.ToString();
            string Wclass = cook.Sclass.ToString();
            string Wyear = cook.Syear.ToString();
            string Wterm = cook.ThisTerm.ToString();
            string Wip = cook.LoginIp;
            string Wtime = cook.LoginTime;
            DateTime Wdate = DateTime.Now;

            string MySavePath = LearnSite.Common.WorkUpload.GetWurl(Wyear, Wgrade, Wclass, Wcid, Wmid);//获得作品保存路径（如果不存在，自动创建）

            string OnlyFileName = HttpContext.Current.Request.Form["modelName"];

            string Wthumbnail = MySavePath + "/" + OnlyFileName + ".png";
            string thumbnailpath = HttpContext.Current.Server.MapPath(Wthumbnail);

            string NewFileName = OnlyFileName + ".json";
            string Wurl = MySavePath + "/" + NewFileName;
            string SaveFile = HttpContext.Current.Server.MapPath(Wurl);

            int flen = 0;
            int plen = 0;
            string title = "";
            string codefile = "";
            string codedict = "";
            string Ext = "mlimg";
            BLL.Works bll = new BLL.Works();
            Model.Works model = new Model.Works();//////////////////
            model = bll.GetModelByStu(Int32.Parse(Wmid), Wnum);
            bool saved = false;
            if (model != null)
                saved = model.Wcheck;//判断作品是否评价过

            if (!saved)
            {
                try
                {
                    string base64 = HttpContext.Current.Request.Form["base64"];
                    HttpPostedFile cover = HttpContext.Current.Request.Files["cover"];
                    plen = base64.Length;
                    if (plen > 0)
                    {
                        cover.SaveAs(thumbnailpath);
                        System.IO.File.WriteAllText(SaveFile, base64);
                    }
                }
                catch (Exception ec)
                {
                    LearnSite.Common.Log.Addlog("保存出错信息", ec.StackTrace);// ec.StackTrace可以出详细信息
                }

                bool checkcan = true;

                //string Wid = bll.WorkDone(Wnum, Int32.Parse(Wcid), Int32.Parse(Wmid));//返回空字符表示不存在该记录
                //记录到数据库
                if (model != null)
                {
                    bll.UpdateTopic(model.Wid, Wurl, NewFileName, flen, Wdate, checkcan, Wthumbnail, title, codefile, Ext);//更新Wfilename, Wurl,Wlength, Wdate

                }
                else
                {
                    //Wnum,Wcid,Wmid,Wmsort,Wfilename,Wtype,Wurl,Wlength,Wdate,Wip
                    //,Wtime,Wegg,Wgrade,Wterm,Woffice,Wflash,Wsid,Wclass,Wname,Wyear
                    Model.Works wmodel = new Model.Works();//////////////////
                    wmodel.Wnum = Wnum;
                    wmodel.Wcid = Int32.Parse(Wcid);
                    wmodel.Wmid = Int32.Parse(Wmid);
                    wmodel.Wmsort = 15;
                    wmodel.Wfilename = NewFileName;
                    wmodel.Wtype = Ext;
                    wmodel.Wurl = Wurl;
                    wmodel.Wlength = flen;
                    wmodel.Wdate = Wdate;
                    wmodel.Wip = Wip;
                    wmodel.Wtime = Wtime;
                    wmodel.Wcan = checkcan;
                    wmodel.Wcheck = false;
                    wmodel.Wegg = 12;//设定票数为12张
                    wmodel.Whit = 0;
                    wmodel.Wgrade = Int32.Parse(Wgrade);
                    wmodel.Wterm = Int32.Parse(Wterm);
                    wmodel.Woffice = false;
                    wmodel.Wsid = Wsid;
                    wmodel.Wclass = Int32.Parse(Wclass);
                    wmodel.Wname = HttpUtility.UrlDecode(Wname);
                    wmodel.Wyear = Int32.Parse(Wyear);
                    wmodel.Wflash = false;
                    wmodel.Werror = false;
                    wmodel.Wtitle = title;
                    wmodel.Wcode = codefile;
                    wmodel.Wdict = codedict;
                    wmodel.Wlid = Int32.Parse(Wlid);
                    wmodel.Wthumbnail = Wthumbnail;

                    bll.AddWorkUp(wmodel);//添加作品提交记录
                    BLL.Signin sn = new BLL.Signin();
                    sn.UpdateQwork(Wsid, Int32.Parse(Wcid));//更新今天签到表中的作品数量

                    //添加课堂活动记录
                    Model.MenuWorks kmodel = new Model.MenuWorks();
                    kmodel.Klid = Int32.Parse(Wlid);
                    kmodel.Ksid = Wsid;
                    kmodel.Ktime = LearnSite.Common.Computer.GoneMinute(DateTime.Parse(Wtime), Wdate);
                    kmodel.Kseconds = Int32.Parse(LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate));
                    kmodel.Kcheck = false;
                    BLL.MenuWorks kbll = new MenuWorks();
                    kbll.Add(kmodel);
                }
            }

        }


        public void SavePoster(string id)
        {
            string[] arrayid = id.Split('-');
            string Wcid = arrayid[0];
            string Wmid = arrayid[1];
            string Wlid = arrayid[2];

            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

            int Wsid = cook.Sid;
            string Wnum = cook.Snum;
            string Wname = cook.Sname;
            string Wgrade = cook.Sgrade.ToString();
            string Wclass = cook.Sclass.ToString();
            string Wyear = cook.Syear.ToString();
            string Wterm = cook.ThisTerm.ToString();
            string Wip = cook.LoginIp;
            string Wtime = cook.LoginTime;
            DateTime Wdate = DateTime.Now;

            string MySavePath = LearnSite.Common.WorkUpload.GetWurl(Wyear, Wgrade, Wclass, Wcid, Wmid);//获得作品保存路径（如果不存在，自动创建）
            string RndTime = LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate);
            string OnlyFileName = Wnum + "_" + Wcid + "_" + Wmid + "_" + RndTime;

            string Wthumbnail = MySavePath + "/" + OnlyFileName + ".png";
            string thumbnailpath = HttpContext.Current.Server.MapPath(Wthumbnail);

            string NewFileName = OnlyFileName + ".json";
            string Wurl = MySavePath + "/" + NewFileName;
            string SaveFile = HttpContext.Current.Server.MapPath(Wurl);

            int flen = 0;
            int plen = 0;
            string title = "";
            string codefile = "";
            string codedict = "";
            string Ext = "poster";
            BLL.Works bll = new BLL.Works();
            Model.Works model = new Model.Works();//////////////////
            model = bll.GetModelByStu(Int32.Parse(Wmid), Wnum);
            bool saved = false;
            if (model != null)
                saved = model.Wcheck;//判断作品是否评价过

            if (!saved)
            {
                try
                {
                    HttpPostedFile posterfile = HttpContext.Current.Request.Files["poster"];
                    HttpPostedFile cover = HttpContext.Current.Request.Files["cover"];
                    plen = posterfile.ContentLength;
                    if (plen > 0)
                    {
                        cover.SaveAs(thumbnailpath);
                        posterfile.SaveAs(SaveFile);
                        //LearnSite.Common.Log.Addlog("3保存成功信息", "保存成功");// ec.StackTrace可以出详细信息
                    }

                    // LearnSite.Common.Log.Addlog("python作品上传调试信息：", msg);
                }
                catch (Exception ec)
                {
                    LearnSite.Common.Log.Addlog("保存出错信息", ec.StackTrace);// ec.StackTrace可以出详细信息
                }

                bool checkcan = true;

                //string Wid = bll.WorkDone(Wnum, Int32.Parse(Wcid), Int32.Parse(Wmid));//返回空字符表示不存在该记录
                //记录到数据库
                if (model != null)
                {
                    bll.UpdateTopic(model.Wid, Wurl, NewFileName, flen, Wdate, checkcan, Wthumbnail, title, codefile, Ext);//更新Wfilename, Wurl,Wlength, Wdate

                }
                else
                {
                    //Wnum,Wcid,Wmid,Wmsort,Wfilename,Wtype,Wurl,Wlength,Wdate,Wip
                    //,Wtime,Wegg,Wgrade,Wterm,Woffice,Wflash,Wsid,Wclass,Wname,Wyear
                    Model.Works wmodel = new Model.Works();//////////////////
                    wmodel.Wnum = Wnum;
                    wmodel.Wcid = Int32.Parse(Wcid);
                    wmodel.Wmid = Int32.Parse(Wmid);
                    wmodel.Wmsort = 15;
                    wmodel.Wfilename = NewFileName;
                    wmodel.Wtype = Ext;
                    wmodel.Wurl = Wurl;
                    wmodel.Wlength = flen;
                    wmodel.Wdate = Wdate;
                    wmodel.Wip = Wip;
                    wmodel.Wtime = Wtime;
                    wmodel.Wcan = checkcan;
                    wmodel.Wcheck = false;
                    wmodel.Wegg = 12;//设定票数为12张
                    wmodel.Whit = 0;
                    wmodel.Wgrade = Int32.Parse(Wgrade);
                    wmodel.Wterm = Int32.Parse(Wterm);
                    wmodel.Woffice = false;
                    wmodel.Wsid = Wsid;
                    wmodel.Wclass = Int32.Parse(Wclass);
                    wmodel.Wname = HttpUtility.UrlDecode(Wname);
                    wmodel.Wyear = Int32.Parse(Wyear);
                    wmodel.Wflash = false;
                    wmodel.Werror = false;
                    wmodel.Wtitle = title;
                    wmodel.Wcode = codefile;
                    wmodel.Wdict = codedict;
                    wmodel.Wlid = Int32.Parse(Wlid);
                    wmodel.Wthumbnail = Wthumbnail;

                    bll.AddWorkUp(wmodel);//添加作品提交记录
                    BLL.Signin sn = new BLL.Signin();
                    sn.UpdateQwork(Wsid, Int32.Parse(Wcid));//更新今天签到表中的作品数量

                    //添加课堂活动记录
                    Model.MenuWorks kmodel = new Model.MenuWorks();
                    kmodel.Klid = Int32.Parse(Wlid);
                    kmodel.Ksid = Wsid;
                    kmodel.Ktime = LearnSite.Common.Computer.GoneMinute(DateTime.Parse(Wtime), Wdate);
                    kmodel.Kseconds = Int32.Parse(LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate));
                    kmodel.Kcheck = false;
                    BLL.MenuWorks kbll = new MenuWorks();
                    kbll.Add(kmodel);
                }
            }

        }

        public static string DecodeBase64(string base64EncodedData)
        {
            // 解码Base64字符串
            byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public void SaveTopic(string id)
        {
            string[] arrayid = id.Split('-');
            string Wcid = arrayid[0];
            string Wmid = arrayid[1];
            string Wlid = arrayid[2];

            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
            int Wsid = cook.Sid;
            string Wnum = cook.Snum;
            string Wname = cook.Sname;
            string Wgrade = cook.Sgrade.ToString();
            string Wclass = cook.Sclass.ToString();
            string Wyear = cook.Syear.ToString();
            string Wterm = cook.ThisTerm.ToString();
            string Wip = cook.LoginIp;
            string Wtime = cook.LoginTime;
            DateTime Wdate = DateTime.Now;

            string MySavePath = LearnSite.Common.WorkUpload.GetWurl(Wyear, Wgrade, Wclass, Wcid, Wmid);//获得作品保存路径（如果不存在，自动创建）
            //string RndTime = LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate);
            string OnlyFileName = Wnum + "_" + Wcid + "_" + Wmid;// +"_" + RndTime;
            string NewThumbnail = OnlyFileName + ".png";
            string Wthumbnail = MySavePath + "/" + NewThumbnail;
            string thumbnailpath = HttpContext.Current.Server.MapPath(Wthumbnail);

            int flen = 0;
            int plen = 0;
            string NewFileName = "";
            string Wurl = "";
            string title = "";
            string codefile = "";
            string codedict = "";
            string Ext = "";
            string Wscore = HttpContext.Current.Request.Form["score"] ?? "0";
            BLL.Works bll = new BLL.Works();
            Model.Works model = new Model.Works();//////////////////
            model = bll.GetModelByStu(Int32.Parse(Wmid), Wnum);
            bool saved = false;
            if (model != null)
                saved = model.Wcheck;//判断作品是否评价过

            if (!saved)
            {
                try
                {
                    title = HttpContext.Current.Request.Form["title"];
                    HttpPostedFile cover = HttpContext.Current.Request.Files["cover"];
                    codefile = HttpContext.Current.Request.Form["content"];
                    Ext = HttpContext.Current.Request.Form["ext"];
                    Wurl = MySavePath + "/" + OnlyFileName + "." + Ext;
                    if (Ext == "pxl")
                    {
                        NewThumbnail = OnlyFileName + ".gif";
                        Wthumbnail = MySavePath + "/" + NewThumbnail;
                        thumbnailpath = HttpContext.Current.Server.MapPath(Wthumbnail);
                    }
                    plen = cover.ContentLength;
                    if (plen > 0)
                    {
                        cover.SaveAs(thumbnailpath);
                        //LearnSite.Common.Log.Addlog("3保存成功信息", "保存成功");// ec.StackTrace可以出详细信息
                    }
                    if (Ext == "excalidraw")
                    {
                        string Wpath = HttpContext.Current.Server.MapPath(Wurl);
                        string jsoncode = DecodeBase64(codefile);
                         jsoncode = HttpContext.Current.Server.UrlDecode(jsoncode);
                        File.WriteAllText(Wpath, jsoncode);
                    }

                    // LearnSite.Common.Log.Addlog("python作品上传调试信息：", msg);
                }
                catch (Exception ec)
                {
                    LearnSite.Common.Log.Addlog("保存出错信息", ec.StackTrace);// ec.StackTrace可以出详细信息
                }

                bool checkcan = true;

                //string Wid = bll.WorkDone(Wnum, Int32.Parse(Wcid), Int32.Parse(Wmid));//返回空字符表示不存在该记录
                //记录到数据库
                if (model != null)
                {
                    bll.UpdateTopic(model.Wid, Wurl, NewFileName, flen, Wdate, checkcan, Wthumbnail, title, codefile, Ext, Int32.Parse(Wscore));//更新Wfilename, Wurl,Wlength, Wdate

                }
                else
                {
                    //Wnum,Wcid,Wmid,Wmsort,Wfilename,Wtype,Wurl,Wlength,Wdate,Wip
                    //,Wtime,Wegg,Wgrade,Wterm,Woffice,Wflash,Wsid,Wclass,Wname,Wyear
                    Model.Works wmodel = new Model.Works();//////////////////
                    wmodel.Wnum = Wnum;
                    wmodel.Wcid = Int32.Parse(Wcid);
                    wmodel.Wmid = Int32.Parse(Wmid);
                    wmodel.Wmsort = 15;
                    wmodel.Wfilename = NewFileName;
                    wmodel.Wtype = Ext;
                    wmodel.Wurl = Wurl;
                    wmodel.Wlength = flen;
                    wmodel.Wdate = Wdate;
                    wmodel.Wip = Wip;
                    wmodel.Wtime = Wtime;
                    wmodel.Wcan = checkcan;
                    wmodel.Wcheck = false;
                    wmodel.Wegg = 12;//设定票数为12张
                    wmodel.Whit = 0;
                    wmodel.Wgrade = Int32.Parse(Wgrade);
                    wmodel.Wterm = Int32.Parse(Wterm);
                    wmodel.Woffice = false;
                    wmodel.Wsid = Wsid;
                    wmodel.Wclass = Int32.Parse(Wclass);
                    wmodel.Wname = HttpUtility.UrlDecode(Wname);
                    wmodel.Wyear = Int32.Parse(Wyear);
                    wmodel.Wflash = false;
                    wmodel.Werror = false;
                    wmodel.Wtitle = title;
                    wmodel.Wcode = codefile;
                    wmodel.Wdict = codedict;
                    wmodel.Wlid = Int32.Parse(Wlid);
                    wmodel.Wthumbnail = Wthumbnail;
                    if (Wscore != "0")
                    {
                        wmodel.Wscore = Int32.Parse(Wscore);
                    }

                    bll.AddWorkUp(wmodel);//添加作品提交记录
                    BLL.Signin sn = new BLL.Signin();
                    sn.UpdateQwork(Wsid, Int32.Parse(Wcid));//更新今天签到表中的作品数量

                    //添加课堂活动记录
                    Model.MenuWorks kmodel = new Model.MenuWorks();
                    kmodel.Klid = Int32.Parse(Wlid);
                    kmodel.Ksid = Wsid;
                    kmodel.Ktime = LearnSite.Common.Computer.GoneMinute(DateTime.Parse(Wtime), Wdate);
                    kmodel.Kseconds = Int32.Parse(LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate));
                    kmodel.Kcheck = false;
                    BLL.MenuWorks kbll = new MenuWorks();
                    kbll.Add(kmodel);
                }
            }

        }

        public void SavePixel(string id)
        {
            string[] arrayid = id.Split('-');
            string Wcid = arrayid[0];
            string Wmid = arrayid[1];
            string Wlid = arrayid[2];

            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

            int Wsid = cook.Sid;
            string Wnum = cook.Snum;
            string Wname = cook.Sname;
            string Wgrade = cook.Sgrade.ToString();
            string Wclass = cook.Sclass.ToString();
            string Wyear = cook.Syear.ToString();
            string Wterm = cook.ThisTerm.ToString();
            string Wip = cook.LoginIp;
            string Wtime = cook.LoginTime;
            DateTime Wdate = DateTime.Now;

            string MySavePath = LearnSite.Common.WorkUpload.GetWurl(Wyear, Wgrade, Wclass, Wcid, Wmid);//获得作品保存路径（如果不存在，自动创建）
            string RndTime = LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate);
            string OnlyFileName = Wnum + "_" + Wcid + "_" + Wmid + "_" + RndTime;
            string NewFileName = OnlyFileName + ".pxl";
            string Wurl = MySavePath + "/" + NewFileName;
            string SaveFile = HttpContext.Current.Server.MapPath(Wurl);

            string Wthumbnail = "";
            int flen = 0;
            string title = "";
            string pxlfile = "";
            BLL.Works bll = new BLL.Works();
            Model.Works model = new Model.Works();//////////////////
            model = bll.GetModelByStu(Int32.Parse(Wmid), Wnum);
            bool saved = false;
            if (model != null)
                saved = model.Wcheck;//判断作品是否评价过

            if (!saved)
            {
                try
                {
                    pxlfile = HttpContext.Current.Request.Form["pxl"];

                    if (!String.IsNullOrEmpty(pxlfile))
                    {
                        string cf = HttpUtility.UrlDecode(pxlfile);//HttpUtility.UrlDecode 等于js中的decodeURIComponent                        
                        System.IO.File.WriteAllText(SaveFile, cf);
                        flen = pxlfile.Length;
                   }
                    
                    // LearnSite.Common.Log.Addlog("python作品上传调试信息：", msg);
                }
                catch (Exception ec)
                {
                    LearnSite.Common.Log.Addlog("保存出错信息", ec.StackTrace);// ec.StackTrace可以出详细信息
                }

                bool checkcan = true;

                //string Wid = bll.WorkDone(Wnum, Int32.Parse(Wcid), Int32.Parse(Wmid));//返回空字符表示不存在该记录
                //记录到数据库
                if (model != null)
                {
                    bll.UpdateWorkUp(model.Wid, Wurl, NewFileName, flen, Wdate, checkcan, Wthumbnail);//更新Wfilename, Wurl,Wlength, Wdate
                }
                else
                {
                    //Wnum,Wcid,Wmid,Wmsort,Wfilename,Wtype,Wurl,Wlength,Wdate,Wip
                    //,Wtime,Wegg,Wgrade,Wterm,Woffice,Wflash,Wsid,Wclass,Wname,Wyear
                    Model.Works wmodel = new Model.Works();//////////////////
                    wmodel.Wnum = Wnum;
                    wmodel.Wcid = Int32.Parse(Wcid);
                    wmodel.Wmid = Int32.Parse(Wmid);
                    wmodel.Wmsort = 11;
                    wmodel.Wfilename = NewFileName;
                    wmodel.Wtype = "pxl";
                    wmodel.Wurl = Wurl;
                    wmodel.Wlength = flen;
                    wmodel.Wdate = Wdate;
                    wmodel.Wip = Wip;
                    wmodel.Wtime = Wtime;
                    wmodel.Wcan = checkcan;
                    wmodel.Wcheck = false;
                    wmodel.Wegg = 12;//设定票数为12张
                    wmodel.Whit = 0;
                    wmodel.Wgrade = Int32.Parse(Wgrade);
                    wmodel.Wterm = Int32.Parse(Wterm);
                    wmodel.Woffice = false;
                    wmodel.Wsid = Wsid;
                    wmodel.Wclass = Int32.Parse(Wclass);
                    wmodel.Wname = HttpUtility.UrlDecode(Wname);
                    wmodel.Wyear = Int32.Parse(Wyear);
                    wmodel.Wflash = false;
                    wmodel.Werror = false;
                    wmodel.Wtitle = title;
                    wmodel.Wlid = Int32.Parse(Wlid);

                    bll.AddWorkUp(wmodel);//添加作品提交记录
                    BLL.Signin sn = new BLL.Signin();
                    sn.UpdateQwork(Wsid, Int32.Parse(Wcid));//更新今天签到表中的作品数量

                    //添加课堂活动记录
                    Model.MenuWorks kmodel = new Model.MenuWorks();
                    kmodel.Klid = Int32.Parse(Wlid);
                    kmodel.Ksid = Wsid;
                    kmodel.Ktime = LearnSite.Common.Computer.GoneMinute(DateTime.Parse(Wtime), Wdate);
                    kmodel.Kseconds = Int32.Parse(LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate));
                    kmodel.Kcheck = false;
                    BLL.MenuWorks kbll = new MenuWorks();
                    kbll.Add(kmodel);
                }
            }

        }


        public void Savekitymind(string id)
        {
            string[] arrayid = id.Split('-');
            string Wcid = arrayid[0];
            string Wmid = arrayid[1];
            string Wlid = arrayid[2];

            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

            int Wsid = cook.Sid;
            string Wnum = cook.Snum;
            string Wname = cook.Sname;
            string Wgrade = cook.Sgrade.ToString();
            string Wclass = cook.Sclass.ToString();
            string Wyear = cook.Syear.ToString();
            string Wterm = cook.ThisTerm.ToString();
            string Wip = cook.LoginIp;
            string Wtime = cook.LoginTime;
            DateTime Wdate = DateTime.Now;

            string MySavePath = LearnSite.Common.WorkUpload.GetWurl(Wyear, Wgrade, Wclass, Wcid, Wmid);//获得作品保存路径（如果不存在，自动创建）
            string RndTime = LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate);
            string OnlyFileName = Wnum + "_" + Wcid + "_" + Wmid + "_" + RndTime;
            string NewFileName = OnlyFileName + ".km";
            string Wurl = MySavePath + "/" + NewFileName;
            string SaveFile = HttpContext.Current.Server.MapPath(Wurl);

            string NewThumbnail = OnlyFileName + ".png";
            string Wthumbnail = MySavePath + "/" + NewThumbnail;
            string thumbnailpath = HttpContext.Current.Server.MapPath(Wthumbnail);

            int flen = 0;
            int plen = 0;
            string title = "";
            string codefile = "";
            string codedict = "";
            BLL.Works bll = new BLL.Works();
            Model.Works model = new Model.Works();//////////////////
            model = bll.GetModelByStu(Int32.Parse(Wmid), Wnum);
            bool saved = false;
            if (model != null)
                saved = model.Wcheck;//判断作品是否评价过

            if (!saved)
            {
                try
                {
                    title = HttpContext.Current.Request.Form["title"];
                    codefile = HttpContext.Current.Request.Form["km"];

                    if (!String.IsNullOrEmpty(codefile))
                    {
                        string cf = HttpUtility.UrlDecode(codefile);//HttpUtility.UrlDecode 等于js中的decodeURIComponent                        
                        System.IO.File.WriteAllText(SaveFile, cf);
                        flen = codefile.Length;
                    }

                    HttpPostedFile pngf = HttpContext.Current.Request.Files["thumb"];
                    plen = pngf.ContentLength;

                    if (plen > 0)
                    {
                        pngf.SaveAs(thumbnailpath);
                        //LearnSite.Common.Log.Addlog("3保存成功信息", "保存成功");// ec.StackTrace可以出详细信息
                    }


                    // LearnSite.Common.Log.Addlog("python作品上传调试信息：", msg);
                }
                catch (Exception ec)
                {
                    LearnSite.Common.Log.Addlog("保存出错信息", ec.StackTrace);// ec.StackTrace可以出详细信息
                }

                bool checkcan = true;

                //string Wid = bll.WorkDone(Wnum, Int32.Parse(Wcid), Int32.Parse(Wmid));//返回空字符表示不存在该记录
                //记录到数据库
                if (model != null)
                {
                    bll.UpdateHtml(model.Wid, Wurl, NewFileName, flen, Wdate, checkcan, Wthumbnail, title, codefile);//更新Wfilename, Wurl,Wlength, Wdate

                }
                else
                {
                    //Wnum,Wcid,Wmid,Wmsort,Wfilename,Wtype,Wurl,Wlength,Wdate,Wip
                    //,Wtime,Wegg,Wgrade,Wterm,Woffice,Wflash,Wsid,Wclass,Wname,Wyear
                    Model.Works wmodel = new Model.Works();//////////////////
                    wmodel.Wnum = Wnum;
                    wmodel.Wcid = Int32.Parse(Wcid);
                    wmodel.Wmid = Int32.Parse(Wmid);
                    wmodel.Wmsort = 15;
                    wmodel.Wfilename = NewFileName;
                    wmodel.Wtype = "km";
                    wmodel.Wurl = Wurl;
                    wmodel.Wlength = flen;
                    wmodel.Wdate = Wdate;
                    wmodel.Wip = Wip;
                    wmodel.Wtime = Wtime;
                    wmodel.Wcan = checkcan;
                    wmodel.Wcheck = false;
                    wmodel.Wegg = 12;//设定票数为12张
                    wmodel.Whit = 0;
                    wmodel.Wgrade = Int32.Parse(Wgrade);
                    wmodel.Wterm = Int32.Parse(Wterm);
                    wmodel.Woffice = false;
                    wmodel.Wsid = Wsid;
                    wmodel.Wclass = Int32.Parse(Wclass);
                    wmodel.Wname = HttpUtility.UrlDecode(Wname);
                    wmodel.Wyear = Int32.Parse(Wyear);
                    wmodel.Wflash = false;
                    wmodel.Werror = false;
                    wmodel.Wtitle = title;
                    wmodel.Wcode = codefile;
                    wmodel.Wdict = codedict;
                    wmodel.Wlid = Int32.Parse(Wlid);
                    wmodel.Wthumbnail = Wthumbnail;

                    bll.AddWorkUp(wmodel);//添加作品提交记录
                    BLL.Signin sn = new BLL.Signin();
                    sn.UpdateQwork(Wsid, Int32.Parse(Wcid));//更新今天签到表中的作品数量

                    //添加课堂活动记录
                    Model.MenuWorks kmodel = new Model.MenuWorks();
                    kmodel.Klid = Int32.Parse(Wlid);
                    kmodel.Ksid = Wsid;
                    kmodel.Ktime = LearnSite.Common.Computer.GoneMinute(DateTime.Parse(Wtime), Wdate);
                    kmodel.Kseconds = Int32.Parse(LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate));
                    kmodel.Kcheck = false;
                    BLL.MenuWorks kbll = new MenuWorks();
                    kbll.Add(kmodel);
                }
            }

        }


        public void SaveExcel(string id)
        {
            string[] arrayid = id.Split('-');
            string Wcid = arrayid[0];
            string Wmid = arrayid[1];
            string Wlid = arrayid[2];

            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

            int Wsid = cook.Sid;
            string Wnum = cook.Snum;
            string Wname = cook.Sname;
            string Wgrade = cook.Sgrade.ToString();
            string Wclass = cook.Sclass.ToString();
            string Wyear = cook.Syear.ToString();
            string Wterm = cook.ThisTerm.ToString();
            string Wip = cook.LoginIp;
            string Wtime = cook.LoginTime;
            DateTime Wdate = DateTime.Now;

            string MySavePath = LearnSite.Common.WorkUpload.GetWurl(Wyear, Wgrade, Wclass, Wcid, Wmid);//获得作品保存路径（如果不存在，自动创建）
            string RndTime = LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate);
            string OnlyFileName = Wnum + "_" + Wcid + "_" + Wmid + "_" + RndTime;
            string NewFileName = OnlyFileName + ".sheet";
            string Wurl = MySavePath + "/" + NewFileName;
            string SaveFile = HttpContext.Current.Server.MapPath(Wurl);

            string Wthumbnail = "";
            int flen = 0;
            string title = "";
            string codefile = "";
            string codedict = "";
            BLL.Works bll = new BLL.Works();
            Model.Works model = new Model.Works();//////////////////
            model = bll.GetModelByStu(Int32.Parse(Wmid), Wnum);
            bool saved = false;
            if (model != null)
                saved = model.Wcheck;//判断作品是否评价过

            if (!saved)
            {
                try
                {
                    title = HttpContext.Current.Request.Form["title"];
                    codefile = HttpContext.Current.Request.Form["excel"];

                    // LearnSite.Common.Log.Addlog("python作品上传调试信息：", msg);
                }
                catch (Exception ec)
                {
                    LearnSite.Common.Log.Addlog("保存出错信息", ec.StackTrace);// ec.StackTrace可以出详细信息
                }

                bool checkcan = true;

                //string Wid = bll.WorkDone(Wnum, Int32.Parse(Wcid), Int32.Parse(Wmid));//返回空字符表示不存在该记录
                //记录到数据库
                if (model != null)
                {
                    bll.UpdateHtml(model.Wid, Wurl, NewFileName, flen, Wdate, checkcan, Wthumbnail, title, codefile);//更新Wfilename, Wurl,Wlength, Wdate

                }
                else
                {
                    //Wnum,Wcid,Wmid,Wmsort,Wfilename,Wtype,Wurl,Wlength,Wdate,Wip
                    //,Wtime,Wegg,Wgrade,Wterm,Woffice,Wflash,Wsid,Wclass,Wname,Wyear
                    Model.Works wmodel = new Model.Works();//////////////////
                    wmodel.Wnum = Wnum;
                    wmodel.Wcid = Int32.Parse(Wcid);
                    wmodel.Wmid = Int32.Parse(Wmid);
                    wmodel.Wmsort = 16;
                    wmodel.Wfilename = NewFileName;
                    wmodel.Wtype = "sheet";
                    wmodel.Wurl = Wurl;
                    wmodel.Wlength = flen;
                    wmodel.Wdate = Wdate;
                    wmodel.Wip = Wip;
                    wmodel.Wtime = Wtime;
                    wmodel.Wcan = checkcan;
                    wmodel.Wcheck = false;
                    wmodel.Wegg = 12;//设定票数为12张
                    wmodel.Whit = 0;
                    wmodel.Wgrade = Int32.Parse(Wgrade);
                    wmodel.Wterm = Int32.Parse(Wterm);
                    wmodel.Woffice = false;
                    wmodel.Wsid = Wsid;
                    wmodel.Wclass = Int32.Parse(Wclass);
                    wmodel.Wname = HttpUtility.UrlDecode(Wname);
                    wmodel.Wyear = Int32.Parse(Wyear);
                    wmodel.Wflash = false;
                    wmodel.Werror = false;
                    wmodel.Wtitle = title;
                    wmodel.Wcode = codefile;
                    wmodel.Wdict = codedict;
                    wmodel.Wlid = Int32.Parse(Wlid);

                    bll.AddWorkUp(wmodel);//添加作品提交记录
                    BLL.Signin sn = new BLL.Signin();
                    sn.UpdateQwork(Wsid, Int32.Parse(Wcid));//更新今天签到表中的作品数量

                    //添加课堂活动记录
                    Model.MenuWorks kmodel = new Model.MenuWorks();
                    kmodel.Klid = Int32.Parse(Wlid);
                    kmodel.Ksid = Wsid;
                    kmodel.Ktime = LearnSite.Common.Computer.GoneMinute(DateTime.Parse(Wtime), Wdate);
                    kmodel.Kcheck = false;
                    BLL.MenuWorks kbll = new MenuWorks();
                    kbll.Add(kmodel);
                }
            }

        }

        private int FindPos(string source, string word)
        {
            Regex regex = new Regex(word);
            Match mc;
            mc = regex.Match(source);
            return mc.Index;
        }
        public void SaveThumbnail(string id, byte[] pngfile)
        {
            string[] arrayid = id.Split('-');
            string Wcid = arrayid[0];
            string Wmid = arrayid[1];

            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

            int Wsid = cook.Sid;
            string Wnum = cook.Snum;
            string Wname = cook.Sname;
            string Wgrade = cook.Sgrade.ToString();
            string Wclass = cook.Sclass.ToString();
            string Wyear = cook.Syear.ToString();
            string Wterm = cook.ThisTerm.ToString();

            DateTime Pdate = DateTime.Now;
            string MySavePath = Common.WorkUpload.GetWurl(Wyear, Wgrade, Wclass, Wcid, Wmid);//获得作品保存路径（如果不存在，自动创建）
            string OnlyFileName = Wnum + "_" + Wcid + "_" + Wmid;
            string NewFileName = OnlyFileName + ".png";
            string Wurl = MySavePath + "/" + NewFileName;
            string SaveFile = HttpContext.Current.Server.MapPath(Wurl);

            Works bll = new Works();
            bll.upWthumbnail(Wnum, Wmid, Wurl);//标记缩略图
            
            MemoryStream ms = new MemoryStream();
            ms.Write(pngfile,0, pngfile.Length);
            Image image = Image.FromStream(ms);         
            image.Save(SaveFile);

            image.Dispose();

        }

        public void SworkToBytes(string id)
        {
            string sburl = "~/Statics/Cat.sb2";

            if (id.Contains("-"))
            {
                string[] arrayid = id.Split('-');
                string Wcid = arrayid[0];
                string Wmid = arrayid[1];
                LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
                string Wnum = cook.Snum;
                Model.Works work = new Model.Works();
                work = GetModelByStu(Int32.Parse(Wmid), Wnum);
                if (work != null)
                {
                    sburl = work.Wurl;
                }
                else
                {
                    LearnSite.Model.Mission model = new LearnSite.Model.Mission();
                    LearnSite.BLL.Mission mn = new LearnSite.BLL.Mission();
                    model = mn.GetModel(Int32.Parse(Wmid));
                    if (model != null)
                    {
                        sburl = model.Mexample;
                    }
                }
            }
            else
            {
                sburl= GetWorkWurl(Int32.Parse(id));
            }

            string sbpath = HttpContext.Current.Server.MapPath(sburl);
            //获取文件的二进制数据。
            byte[] datas = System.IO.File.ReadAllBytes(sbpath);
            //将二进制数据写入到输出流中。
            HttpContext.Current.Response.OutputStream.Write(datas, 0, datas.Length);
        }

        public int GetSbCount()
        {
            return dal.GetSbCount();
        }

        public DataTable GetSb(int page, int sbcount)
        {
            return dal.GetSb(page,sbcount);
        }
        public DataTable GetSbPage(int startIndex, int endIndex)
        {
            return dal.GetSbPage(startIndex, endIndex);
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
