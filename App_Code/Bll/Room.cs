using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using LearnSite.Model;
namespace LearnSite.BLL
{
    /// <summary>
    /// 业务逻辑类Room 的摘要说明。
    /// </summary>
    public class Room
    {
        private readonly LearnSite.DAL.Room dal = new LearnSite.DAL.Room();
        public Room()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return dal.GetMaxId();
        }

        public int GetMinRgrade()
        {
            return dal.GetMinRgrade();
        }
        public int GetMaxRgrade()
        {
            return dal.GetMaxRgrade();
        }
        public int GetMinRclass()
        {
            return dal.GetMinRclass();
        }
        public int GetMaxRclass()
        {
            return dal.GetMaxRclass();
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Rid)
        {
            return dal.Exists(Rid);
        }
        /// <summary>
        /// 是否存在该班级
        /// </summary>
        public bool ExistsGC(int Rgrade, int Rclass)
        {
            return dal.ExistsGC(Rgrade, Rclass);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(LearnSite.Model.Room model)
        {
            return dal.Add(model);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void AddRoom(int Rgrade, int Rclass)
        {
            dal.AddRoom(Rgrade, Rclass);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(LearnSite.Model.Room model)
        {
            dal.Update(model);
        }
        /// <summary>
        /// 更新一条班级选择数据
        /// </summary>
        public void UpdateRhid(int Rid, int Rhid)
        {
            dal.UpdateRhid(Rid, Rhid);
        }
        /// <summary>
        /// 清除所有班级选择
        /// </summary>
        public void UpdateRhidNone()
        {
            dal.UpdateRhidNone();
        }

        /// <summary>
        /// 清除该教师所有班级选择
        /// </summary>
        public void ClearRhid(int Rhid)
        {
            dal.ClearRhid(Rhid);
        }
        /// <summary>
        /// 更新班级机房布置
        /// </summary>
        public void UpdateRseat(int Rgrade, int Rclass, int Houseid)
        {
            dal.UpdateRseat(Rgrade, Rclass, Houseid);
        }
        /// <summary>
        /// 更新班级密码显示方式：1显示；0隐藏
        /// </summary>
        public void UpdateRip(int Rgrade, int Rclass, bool isshow)
        {
            dal.UpdateRip(Rgrade, Rclass, isshow);
        }
        /// <summary>
        /// 更新班级密码显示方式：1显示；0隐藏
        /// </summary>
        public bool GetRip(int Rgrade, int Rclass)
        {
            return dal.GetRip(Rgrade, Rclass);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int Rid)
        {

            dal.Delete(Rid);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void DeleteAll()
        {
            dal.DeleteAll();
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LearnSite.Model.Room GetModel(int Rid)
        {

            return dal.GetModel(Rid);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LearnSite.Model.Room GetModel(int Rgrade, int Rclass)
        {
            return dal.GetModel(Rgrade, Rclass);
        }
        /// <summary>
        /// 得到一个对象实体，从缓存中。
        /// </summary>
        public LearnSite.Model.Room GetModelByCache(int Rid)
        {

            string CacheKey = "RoomModel-" + Rid;
            object objModel = LearnSite.Common.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetModel(Rid);
                    if (objModel != null)
                    {
                        int ModelCache = LearnSite.Common.ConfigHelper.GetConfigInt("ModelCache");
                        LearnSite.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (LearnSite.Model.Room)objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        /// <summary>
        /// 获取闯关控制值
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        /// <returns></returns>
        public bool GetRpass(int Rgrade, int Rclass)
        {
            return dal.GetRpass(Rgrade, Rclass);
        }

        /// <summary>
        /// 更新闯关控制
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        /// <param name="RPass"></param>
        public void UpdateRpass(int Rgrade, int Rclass, bool Rpass)
        {
            dal.UpdateRpass(Rgrade, Rclass, Rpass);
        }
        /// <summary>
        /// 获取互评控制值
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        /// <returns></returns>
        public bool GetRgauge(int Rgrade, int Rclass)
        {
            return dal.GetRgauge(Rgrade, Rclass);
        }
        /// <summary>
        /// 更新互评控制
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        /// <param name="Rgauge"></param>
        public void UpdateMyRgauge(int Rgrade, int Rclass, bool Rgauge)
        {
            dal.UpdateMyRgauge(Rgrade, Rclass, Rgauge);
        }

        /// <summary>
        /// 班级编程控制开关
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        /// <param name="Rscratch"></param>
        public void UpdateMyRscratch(int Rgrade, int Rclass, bool Rscratch)
        {
            dal.UpdateMyRscratch(Rgrade, Rclass, Rscratch);
        }

        /// <summary>
        /// 允许本班单独个人模式登录开关
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        /// <param name="Rlogin"></param>
        public void UpdateMyRlogin(int Rgrade, int Rclass, bool Rlogin)
        {
            dal.UpdateMyRlogin(Rgrade, Rclass, Rlogin);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            return dal.GetList(Top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LearnSite.Model.Room> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LearnSite.Model.Room> DataTableToList(DataTable dt)
        {
            return BllDataTableMappers.MapRoomList(dt);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetAllList()
        {
            return GetList("");
        }

        /// <summary>
        /// 获得指定Rhid的班级数据列表(年级班级别名为Rgradeclass
        /// </summary>
        public DataSet GetMyClassList(int Rhid)
        {
            return dal.GetMyClassList(Rhid);
        }
        /// <summary>
        ///  从年级表中获得不重复的年级
        /// </summary>
        /// <returns></returns>
        public DataTable GetGrade(int Rhid)
        {
            return dal.GetGrade(Rhid).Tables[0];
        }
        /// <summary>
        ///  从年级表中获得不重复的年级
        /// </summary>
        /// <returns></returns>
        public DataTable GetFindGrade()
        {
            return dal.GetFindGrade().Tables[0];
        }

        /// <summary>
        /// 从年级表中获得不重复的所有年级
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllGrade()
        {
            return dal.GetAllGrade();
        }       
        /// <summary>
        /// 从年级表中获得不重复的所有可注册年级
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllRegGrade()
        {
            return dal.GetAllRegGrade();
        }

        /// <summary>
        /// 学案年级列表专用，显示教过的年级及当前班级列表中的年级
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllCourseGrade()
        {
            return dal.GetAllCourseGrade();
        }

        /// <summary>
        /// 获得指定Rhid的班级数据列表(Rgrade,Rclass
        /// </summary>
        public DataTable GetMyGradeClass(int Rhid)
        {
            return dal.GetMyGradeClass(Rhid);
        }
        /// <summary>
        /// 从年级表中获得不重复的班级
        /// </summary>
        /// <returns></returns>
        public DataSet GetFindClass()
        {
            return dal.GetFindClass();
        }

        /// <summary>
        /// 从下拉列表框中选取年级后重新获得不重复的班级,限制所教班级
        /// </summary>
        /// <returns></returns>
        public DataTable GetLimitClass(int Rgrade)
        {
            return dal.GetLimitClass(Rgrade).Tables[0];
        }

        /// <summary>
        /// 从下拉列表框中选取年级后重新获得不重复的所有班级，不限制
        /// </summary>
        /// <returns></returns>
        public DataSet GetAllClass()
        {
            return dal.GetAllClass();
        }
        /// <summary>
        /// 从下拉列表框中选取年级后重新获得不重复的所有班级，不限制
        /// </summary>
        /// <returns></returns>
        public DataSet GetLimitAllClass(int Rgrade)
        {
            return dal.GetLimitAllClass(Rgrade);
        }                
        /// <summary>
        /// 获取可注册班级
        /// </summary>
        /// <returns></returns>
        public DataSet GetRegClass(int Rgrade)
        {
            return dal.GetRegClass(Rgrade);
        }
        /// <summary>
        /// 将要上课班级设置，方便学生查询账号，返回设置密码(6位)
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        public string TeachingRoomSet(int Rhid, int Rgrade, int Rclass, int pwdlen)
        {
            return dal.TeachingRoomSet(Rhid, Rgrade, Rclass, pwdlen);
        }

        /// <summary>
        /// 根据年级范围和班级最大值，循环生成所有班级数
        /// </summary>
        /// <param name="RgradeMin"></param>
        /// <param name="RgradeMax"></param>
        /// <param name="RclassMax"></param>
        public void CreateRoom(int RgradeMin, int RgradeMax, int RclassMax)
        {
            dal.CreateRoom(RgradeMin, RgradeMax, RclassMax);
        }


        /// <summary>
        /// 查询班级表中记录数
        /// </summary>
        /// <returns></returns>
        public int GradeCount()
        {
            return dal.GradeCount();
        }

        /// <summary>
        /// 根据年级和班级，获取密码
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        /// <returns></returns>
        public string GetRoomPwd(int Rgrade, int Rclass)
        {
            return dal.GetRoomPwd(Rgrade, Rclass);
        }
        /// <summary>
        /// 根据年级和班级，获取教师Rhid
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        /// <returns></returns>
        public string GetRoomRhid(int Rgrade, int Rclass)
        {
            return dal.GetRoomRhid(Rgrade, Rclass);
        }
        /// <summary>
        /// 将所教的当前上课班级变为不上课
        /// </summary>
        /// <param name="Rhid"></param>
        public void UnlineClass(int Rhid)
        {
            dal.UnlineClass(Rhid);
        }

        /// <summary>
        /// 查询当前上课班级的所有学生学号
        /// </summary>
        /// <param name="Rhid"></param>
        /// <returns></returns>
        public ArrayList GetCurrentClassSnum(int Rhid)
        {
            return dal.GetCurrentClassSnum(Rhid);
        }
        /// <summary>
        /// 查询该班级的所有学生学号
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        /// <returns></returns>
        public ArrayList GetGradeClassSnum(int Sgrade, int Sclass)
        {
            return dal.GetGradeClassSnum(Sgrade, Sclass);
        }

        /// <summary>
        /// 获取当前教师正在上课的班级（Rset=1）
        /// </summary>
        /// <param name="Rhid">教师ID</param>
        /// <returns>返回年级和班级的DataTable</returns>
        public DataTable GetCurrentTeachingClass(int Rhid)
        {
            return dal.GetCurrentTeachingClass(Rhid);
        }

        /// <summary>
        /// 更新班级的固定IP登录开关状态
        /// </summary>
        /// <param name="Rgrade">年级</param>
        /// <param name="Rclass">班级</param>
        /// <param name="Rlock">是否开启固定IP登录</param>
        public void UpdateLock(int Rgrade, int Rclass, bool Rlock)
        {
            dal.UpdateLock(Rgrade, Rclass, Rlock);
        }
        /// <summary>
        /// 数据表字段更新时用
        /// </summary>
        public void InitLock()
        {
            dal.InitLock();
        }

        /// <summary>
        /// 判断该班级的固定IP登录模式是否开启
        /// </summary>
        /// <param name="Rgrade">年级</param>
        /// <param name="Rclass">班级</param>
        /// <returns>true:已开启固定IP登录; false:未开启</returns>
        public bool IsLoginLock(int Rgrade, int Rclass)
        {
            return dal.IsLoginLock(Rgrade, Rclass);
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void UpdateRgauge()
        {
            dal.UpdateRgauge();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void UpdateRgroupMax()
        {
            dal.UpdateRgroupMax();
        }
        public void SetRgroupMax(int Rgrade, int Rclass, int groupMax)
        {
            dal.SetRgroupMax(Rgrade, Rclass, groupMax);
        }
        public int GetRgroupMax(int Rgrade, int Rclass)
        {
            return dal.GetRgroupMax(Rgrade, Rclass);
        }

        /// <summary>
        /// 查询是否有任教班级
        /// </summary>
        /// <param name="Rhid"></param>
        /// <returns></returns>
        public bool ExistMyClass(int Rhid)
        {
            return dal.ExistMyClass(Rhid);
        }

        public void SetRclassedit(int Rgrade, int Rclass, bool Rclassedit)
        {
            dal.SetRclassedit(Rgrade, Rclass, Rclassedit);
        }

        public void SetRphotoedit(int Rgrade, int Rclass, bool Rphotoedit)
        {
            dal.SetRphotoedit(Rgrade, Rclass, Rphotoedit);
        }


        public void SetRsexedit(int Rgrade, int Rclass, bool Rsexedit)
        {
            dal.SetRsexedit(Rgrade, Rclass, Rsexedit);
        }

        public void SetRreg(int Rgrade, int Rclass, bool Rreg)
        {
            dal.SetRreg(Rgrade, Rclass, Rreg);
        }

        public void SetRnameedit(int Rgrade, int Rclass, bool Rnameedit)
        {
            dal.SetRnameedit(Rgrade, Rclass, Rnameedit);
        }

        public bool GetRclassedit(int Rgrade, int Rclass)
        {
            return dal.GetRclassedit(Rgrade, Rclass);
        }
        public bool GetRphotoedit(int Rgrade, int Rclass)
        {
            return dal.GetRphotoedit(Rgrade, Rclass);
        }
        public bool GetRsexedit(int Rgrade, int Rclass)
        {
            return dal.GetRsexedit(Rgrade, Rclass);
        }
        public bool GetRnameedit(int Rgrade, int Rclass)
        {
            return dal.GetRnameedit(Rgrade, Rclass);
        }

        /// <summary>
        /// 给班级设置上课的学案编号标志
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        /// <param name="Rcid"></param>
        public void UpdateRcid(int Rgrade, int Rclass, int Rcid)
        {
            dal.UpdateRcid(Rgrade, Rclass, Rcid);
        }
        /// <summary>
        /// 获取该班级当前上课学案的编号
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        /// <returns></returns>
        public string GetRcid(int Rgrade, int Rclass)
        {
            return dal.GetRcid(Rgrade, Rclass);
        }

        /// <summary>
        /// 更新Ropen
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        /// <param name="Ropen"></param>
        public void UpdateRopen(int Rgrade, int Rclass, bool Ropen)
        {
            dal.UpdateRopen(Rgrade, Rclass, Ropen);
        }
        /// <summary>
        /// 判断是不是公开课模式
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        /// <returns></returns>
        public bool IsRopen(int Rgrade, int Rclass)
        {
            return dal.IsRopen(Rgrade, Rclass);
        }

        /// <summary>
        /// 判断是不是公开课模式，返回Rcid，如果不是，返回""空字符
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        /// <returns></returns>
        public string IsRopenRcid(int Rgrade, int Rclass)
        {
            return dal.IsRopenRcid(Rgrade, Rclass);
        }
        public int initRshare()
        {
            return dal.initRshare();
        }
        public int initRgroupshare()
        {
            return dal.initRgroupshare();
        }
        public void initRpwdsee()
        {
            dal.initRpwdsee();
        }

        /// <summary>
        /// 更新Rshare
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        /// <param name="Rshare"></param>
        public void UpdateRshare(int Rgrade, int Rclass, bool Rshare)
        {
            dal.UpdateRshare(Rgrade, Rclass, Rshare);
        }       
        /// <summary>
        /// 更新Rgroupshare
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        /// <param name="Rgroupshare"></param>
        public void UpdateRgroupshare(int Rgrade, int Rclass, bool Rgroupshare)
        {
            dal.UpdateRgroupshare(Rgrade, Rclass, Rgroupshare);
        }
        /// <summary>
        /// 判断是否共享Rshare
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        public bool IsRshare(int Rgrade, int Rclass)
        {
            return dal.IsRshare(Rgrade, Rclass);
        }                
        /// <summary>
        /// 判断是否共享Rgroupshare
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        public bool IsRgroupshare(int Rgrade, int Rclass)
        {
            return dal.IsRgroupshare(Rgrade, Rclass);
        }
        /// <summary>
        /// 更新Rpwdsee
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        /// <param name="Rpwdsee"></param>
        public void UpdateRpwdsee(int Rgrade, int Rclass, bool Rpwdsee)
        {
            dal.UpdateRpwdsee(Rgrade, Rclass, Rpwdsee);
        }
        /// <summary>
        /// 判断是否班级密码可查Rpwdsee
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        public bool IsRpwdsee(int Rgrade, int Rclass)
        {
            return dal.IsRpwdsee(Rgrade, Rclass);
        }
        /// <summary>
        /// 获取年级中文打字设置
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rhid"></param>
        /// <returns></returns>
        public string GetRtyper(int Rgrade, int Rhid)
        {
            return dal.GetRtyper(Rgrade, Rhid);
        }                
        /// <summary>
        /// 获取班级中文打字设置
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        /// <returns></returns>
        public string GetRtyperByClass(int Rgrade, int Rclass)
        {
            return dal.GetRtyperByClass(Rgrade, Rclass);
        }
        /// <summary>
        /// 设置所教年级中文打字指定文章
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rtyper"></param>
        /// <param name="Rhid"></param>
        public void SetRtyper(int Rgrade, string Rtyper, int Rhid)
        {
            dal.SetRtyper(Rgrade, Rtyper, Rhid);
        }


        /// <summary>
        /// 获取年级拼音词语打字设置
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rhid"></param>
        /// <returns></returns>
        public string GetRchinese(int Rgrade, int Rhid)
        {
            return dal.GetRchinese(Rgrade, Rhid);
        }
        /// <summary>
        /// 获取班级拼音词语打字设置
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        /// <returns></returns>
        public string GetRchineseByClass(int Rgrade, int Rclass)
        {
            return dal.GetRchineseByClass(Rgrade, Rclass);
        }

        /// <summary>
        /// 获取Rid
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rclass"></param>
        /// <returns></returns>
        public int GetRidByGradeClass(int Rgrade, int Rclass)
        {
            return dal.GetRidByGradeClass(Rgrade, Rclass);
        }
        /// <summary>
        /// 设置所教年级拼音词语打字指定文章
        /// </summary>
        /// <param name="Rgrade"></param>
        /// <param name="Rchinese"></param>
        /// <param name="Rhid"></param>
        public void SetRchinese(int Rgrade, string Rchinese, int Rhid)
        {
            dal.SetRchinese(Rgrade, Rchinese, Rhid);
        }
        /// <summary>
        /// 初始化字段默认为0
        /// </summary>
        public void initRreg()
        {
            dal.initRreg();
        }

        public bool IsRscratch(int Rgrade, int Rclass)
        {
           return dal.IsRscratch(Rgrade, Rclass);
        }

        public void updateRscratch(int Rgrade, int Rclass, bool Rscratch)
        {
            dal.updateRscratch(Rgrade, Rclass, Rscratch);
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
