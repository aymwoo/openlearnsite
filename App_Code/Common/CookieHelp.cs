using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LearnSite.Common
{
    /// <summary>
    ///CookieHelp 的摘要说明
    /// </summary>
    public class CookieHelp
    {
        public CookieHelp()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 对字符串进行url编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string UrlEncode(string str)
        {
            return HttpContext.Current.Server.UrlEncode(str);
        }
        public static string cfx = LearnSite.Common.XmlHelp.GetCookiesFix();
        //public static string serverNameVer = LearnSite.DBUtility.DbLinkEdit.serverNameFixnew();
        //public static string tempcfx = cfx + serverNameVer;
        public static string stuCookieNname =cooknamefx("S");
        public static string teaCookieNname = cooknamefx("T");
        public static string mngCookieNname = cooknamefx("M");

        private static string cooknamefx(string k)
        {
            string tempcfx =cfx+ LearnSite.DBUtility.DbLinkEdit.serverNameFixnew();
            return k + tempcfx;        
        }

        // 将学号保存到公开的cookie上，给pptist创建数据库名称用
        public static bool SetPPTistSnum(string Snum) {
            string stucookie = "PPTistudent";
            HttpCookie StuCookie = new HttpCookie(stucookie);

            StuCookie.Value = Snum;
            StuCookie.Expires = DateTime.Now.AddDays(1);
            StuCookie.Path = "/";
            StuCookie.HttpOnly = false; //如果设置为true，则页面使用js无法获取cookie
            StuCookie.Secure = HttpContext.Current.Request.IsSecureConnection;
            HttpContext.Current.Response.AppendCookie(StuCookie);

            if (string.IsNullOrEmpty(Snum)) {
                return false;
            }
            else{
                return true;
            }
        }

        public static bool SetStudentCookies(LearnSite.Model.Students stumod, string logip)
        {
            bool isset = false;
            if (stumod != null)
            {
                LearnSite.BLL.Signin gbll = new BLL.Signin();
                LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
                // 判断该班级是否开启了固定IP登录模式
                if (rm.IsLoginLock(stumod.Sgrade.Value, stumod.Sclass.Value))
                {
                    // 检查当前登录IP是否与学生的固定座位对应的IP一致（如果学生未设置固定座位，则允许登录）
                    if (gbll.IsSameIp(stumod.Snum, logip))
                    {
                        //如果固定IP登录模式开启，且IP验证通过，写cookies
                        isset = SetStuCookie(stumod, logip);
                    }
                }
                else
                {
                    //如果未开启固定IP登录模式，直接写cookies
                    isset = SetStuCookie(stumod, logip);
                }
            }
            return isset;
        }
        /// <summary>
        /// 设置学生的cookies值
        /// </summary>
        /// <param name="stmodel"></param>
        ///Snum,Syear,Sgrade,Sclass,Sname,Spwd,Sex,Saddress,Sphone,Sparents,Sheadtheacher,Sscore,Sattitude,Sape
        public static bool SetStuCookie(LearnSite.Model.Students stmodel, string LoginIp)
        {
            if (stmodel != null)
            {
                LearnSite.BLL.Room rm = new LearnSite.BLL.Room();

                string Rhid = rm.GetRoomRhid(stmodel.Sgrade.Value, stmodel.Sclass.Value);
                DateTime LoginTime = DateTime.Now;

                string ThisTerm = LearnSite.Common.XmlHelp.GetTerm();

                HttpCookie StuCookie = new HttpCookie(stuCookieNname);
                if (stmodel.Sex == null)
                    stmodel.Sex = "无";
                if (stmodel.Sscore == null)
                    stmodel.Sscore = 0;
                if (stmodel.Squiz == null)
                    stmodel.Squiz = 0;
                if (stmodel.Sattitude == null)
                    stmodel.Sattitude = 0;
                if (stmodel.Sape == null)
                    stmodel.Sape = "无";
                if (stmodel.Sgroup == null)
                    stmodel.Sgroup = 0;
                if (String.IsNullOrEmpty(Rhid))
                    Rhid = "0";
                if (stmodel.Spscore == null)
                    stmodel.Spscore = 0;
                if (stmodel.Stxtform == null)
                    stmodel.Stxtform = 0;
                if (stmodel.Sidle == null)
                    stmodel.Sidle = 0;
                               
                Model.Cook Cook = new Model.Cook();
                Cook.Sid = stmodel.Sid;
                Cook.Snum= UrlEncode(stmodel.Snum);
                Cook.Syear= stmodel.Syear.Value;
                Cook.Sgrade = stmodel.Sgrade.Value;
                Cook.Sclass = stmodel.Sclass.Value;
                Cook.Sname= UrlEncode(stmodel.Sname.Trim());
                Cook.Spwd= Common.WordProcess.GetMD5_16bit(stmodel.Spwd);//原8位改16位
                Cook.Sex=UrlEncode(stmodel.Sex);

                Cook.Sscore = stmodel.Sscore.Value + stmodel.Spscore.Value + stmodel.Stxtform.Value + stmodel.Sidle.Value ;

                Cook.Squiz = stmodel.Squiz.Value;
                Cook.Sattitude = stmodel.Sattitude.Value;
                Cook.Sape= UrlEncode(stmodel.Sape);
                Cook.Sgroup = stmodel.Sgroup.Value;
                Cook.LoginTime= LoginTime.ToString();
                Cook.LoginIp= LoginIp;
                Cook.ThisTerm = Int32.Parse(ThisTerm);
                Cook.Rhid= Int32.Parse(Rhid);
                Cook.RankImage = UrlEncode(LearnSite.Common.Rank.RankImage(Cook.Sscore + Cook.Sattitude, true));
                Cook.Ss = stuCookieNname;//cookie名称校验
                Cook.SessionId = "";

                StuCookie.Value = Cook.ToStr();
                
                string str = LearnSite.Common.XmlHelp.GetStudentCookiesPeriod();
                StuCookie.Expires = StudentCookiesPeriod(str);
                StuCookie.Path = "/";
                StuCookie.HttpOnly = true;
                StuCookie.Secure = HttpContext.Current.Request.IsSecureConnection;
                HttpContext.Current.Response.AppendCookie(StuCookie);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 清除管理员的cookie值 - 优化版本，增强安全性和异常处理
        /// </summary>
        public static void ClearManagerCookies()
        {
            try
            {
                // 清除主要管理员Cookie
                ClearCookieByName(mngCookieNname);
                
                // 清除相关的系统Cookie
                string[] systemCookies = { "ASP.NET_SessionId", FormsAuthentication.FormsCookieName };
                foreach (string cookieName in systemCookies)
                {
                    ClearCookieByName(cookieName);
                }
                
                // 清除Session
                if (HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session.RemoveAll();
                    HttpContext.Current.Session.Abandon();
                }
                
                // 清除Request.Cookies
                if (HttpContext.Current.Request != null && HttpContext.Current.Request.Cookies != null)
                {
                    HttpContext.Current.Request.Cookies.Clear();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("清除管理员Cookie失败: " + ex.Message);
            }
        }
        
        /// <summary>
        /// 清除教师的cookie值 - 优化版本，增强安全性和异常处理
        /// </summary>
        public static void ClearTeacherCookies()
        {
            try
            {
                // 清除主要教师Cookie
                ClearCookieByName(teaCookieNname);
                
                // 清除相关的系统Cookie
                string[] systemCookies = { "ASP.NET_SessionId", FormsAuthentication.FormsCookieName };
                foreach (string cookieName in systemCookies)
                {
                    ClearCookieByName(cookieName);
                }
                
                // 清除Session
                if (HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session.RemoveAll();
                    HttpContext.Current.Session.Abandon();
                }
                
                // 清除Request.Cookies
                if (HttpContext.Current.Request != null && HttpContext.Current.Request.Cookies != null)
                {
                    HttpContext.Current.Request.Cookies.Clear();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("清除教师Cookie失败: " + ex.Message);
            }
        }
        
        /// <summary>
        /// 清除学生的cookie值 - 优化版本，增强安全性和异常处理
        /// </summary>
        public static void ClearStudentCookies()
        {
            try
            {
                // 清除主要学生Cookie
                ClearCookieByName(stuCookieNname);
                
                // 清除相关的系统Cookie
                string[] systemCookies = { "ASP.NET_SessionId", FormsAuthentication.FormsCookieName };
                foreach (string cookieName in systemCookies)
                {
                    ClearCookieByName(cookieName);
                }
                
                // 清除Session
                if (HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session.RemoveAll();
                    HttpContext.Current.Session.Abandon();
                }
                
                // 清除Request.Cookies
                if (HttpContext.Current.Request != null && HttpContext.Current.Request.Cookies != null)
                {
                    HttpContext.Current.Request.Cookies.Clear();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("清除学生Cookie失败: " + ex.Message);
            }
        }
        
        /// <summary>
        /// 统一的Cookie清除方法，增强安全性
        /// </summary>
        /// <param name="cookieName">要清除的Cookie名称</param>
        private static void ClearCookieByName(string cookieName)
        {
            try
            {
                if (HttpContext.Current.Request != null && HttpContext.Current.Request.Cookies[cookieName] != null)
                {
                    HttpCookie oldCookie = HttpContext.Current.Request.Cookies[cookieName];
                    
                    // 创建新的Cookie用于覆盖
                    HttpCookie clearCookie = new HttpCookie(cookieName, "");
                    clearCookie.Expires = DateTime.Now.AddYears(-1);
                    clearCookie.Path = "/";
                    clearCookie.HttpOnly = true;
                    clearCookie.Secure = HttpContext.Current.Request.IsSecureConnection;
                    
                    // 添加到响应中
                    if (HttpContext.Current.Response != null)
                    {
                        HttpContext.Current.Response.Cookies.Add(clearCookie);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("清除Cookie " + cookieName + " 失败: " + ex.Message);
            }
        }

        public static bool IsStudentLogin()
        {
            if (HttpContext.Current.Request.Cookies[stuCookieNname] != null)
            {
                Model.Cook cook = new Model.Cook();
                if (cook.Ss == stuCookieNname)
                    return true;
                else
                {
                    ClearStudentCookies();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public static bool IsTeacherLogin()
        {
            if (HttpContext.Current.Request.Cookies[teaCookieNname] != null)
            {
                Model.TeaCook tcook = new Model.TeaCook();
                if (tcook.Ss == teaCookieNname)
                    return true;
                else
                {
                    ClearTeacherCookies();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public static bool IsManagerLogin()
        {
            try
            {
                if (HttpContext.Current.Request.Cookies[mngCookieNname] != null)
                {
                    Model.MngCook mcook = new Model.MngCook();
                    if (mcook.IsExist() && mcook.Ss == mngCookieNname)
                        return true;
                    else
                    {
                        ClearManagerCookies();
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("IsManagerLogin失败: " + ex.Message);
                ClearManagerCookies();
                return false;
            }
        }
        public static void KickStudent()
        {
            if (HttpContext.Current.Request.Cookies[stuCookieNname] != null)  //增加cookies判斷 (2011-9-27修）
            {
                if (LearnSite.Common.XmlHelp.GetSingleLogin())//如果是单点登录
                {
                    Model.Cook cook = new Model.Cook();
                    string mySnum = cook.Snum;
                    if (LearnSite.Common.App.Iskick(mySnum))  //如果学生学号在踢除全局变量中
                    {
                        LearnSite.Common.App.AppKickUserRemove(mySnum);//将踢除列表中的学号去掉（2011-9-15修）
                        ClearStudentCookies();  //则清除该项学生的页面cookies，要求重登录。
                        int millsed = DateTime.Now.Millisecond;
                        string rurl = "~/index.aspx?kick=" + mySnum + "&mill=" + millsed.ToString();
                        System.Threading.Thread.Sleep(200);
                        HttpContext.Current.Response.Redirect(rurl, false);
                    }
                }
            }
        }
        /// <summary>
        /// 修改cookies集合中的某项cookie的值
        /// </summary>
        /// <param name="cookiesName">cookies集合名称</param>
        /// <param name="itemName">某项cookie名称</param>
        /// <param name="newValue">赋值内容</param>
        public static void EditCookiesItem(string cookiesName, string itemName, string newValue)
        {
            HttpCookie cok = HttpContext.Current.Request.Cookies[cookiesName];
            if (cok != null)
            {
                cok.Values.Set(itemName, newValue);
                string str = LearnSite.Common.XmlHelp.GetStudentCookiesPeriod();
                if (str != "0")
                {
                    cok.Expires = StudentCookiesPeriod(str);
                }
                HttpContext.Current.Response.AppendCookie(cok);
            }
        }
        /// <summary>
        /// 判断是否存在cookies，不存在则跳转到登录窗口
        /// </summary>
        public static void JudgeTeacherCookies()
        {
            if (HttpContext.Current.Request.Cookies[teaCookieNname] == null)//如果没登录，跳出
            {
                HttpContext.Current.Response.Redirect("~/teacher/index.aspx", true);
            }
            else
            {
                Model.TeaCook tmcook = new Model.TeaCook();
                if (tmcook.Ss == teaCookieNname)
                {
                    /*
                    LearnSite.BLL.Room rbll = new BLL.Room();
                    if (!rbll.ExistMyClass(tmcook.Hid))
                    {
                        //如果没有任教班级，则始终跳转至信息页面
                        HttpContext.Current.Response.Redirect("~/teacher/infomation.aspx", true);
                    }
                     
                     */
                }
                else
                {
                    ClearTeacherCookies();//非法cookies，清除再跳转
                    System.Threading.Thread.Sleep(500);
                    HttpContext.Current.Response.Redirect("~/teacher/index.aspx", true);
                }
            }
        }

        /// <summary>
        /// （学案列表）简单判断是否存在cookies，不存在则跳转到登录窗口
        /// </summary>
        public static void IsTeacherCookies()
        {
            if (HttpContext.Current.Request.Cookies[teaCookieNname] == null)//如果没登录，跳出
            {
                HttpContext.Current.Response.Redirect("~/teacher/index.aspx", true);
            }
        }

        /// <summary>
        /// 如果没登录，跳转到登录首页
        /// 如果登录，但权限为假，也跳转到教师首页
        /// </summary>
        public static void JudgeIsAdmin()
        {
            try
            {
                if (HttpContext.Current == null || HttpContext.Current.Request == null || HttpContext.Current.Response == null)
                {
                    return;
                }
                
                if (HttpContext.Current.Request.Cookies[mngCookieNname] == null)//没登录跳出
                {
                    HttpContext.Current.Response.Redirect("~/teacher/index.aspx", false);
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                    return;
                }
                else
                {
                    Model.MngCook mncook = new Model.MngCook();
                    if (mncook.IsExist() && mncook.Hpermiss)
                    {
                        // 只检查cookie是否存在和权限是否正确，不检查SessionID
                        // 这样即使Session过期，只要cookie有效，就能访问管理页面
                    }
                    else
                    {
                        ClearManagerCookies();//非法cookies，清除再跳转
                        Others.ClearClientPageCache();
                        System.Threading.Thread.Sleep(500);
                        HttpContext.Current.Response.Redirect("~/teacher/index.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("JudgeIsAdmin失败: " + ex.Message);
                try
                {
                    ClearManagerCookies();
                    if (HttpContext.Current != null && HttpContext.Current.Response != null)
                    {
                        HttpContext.Current.Response.Redirect("~/teacher/index.aspx", false);
                        HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
                catch { }
            }
        }
        /// <summary>
        /// 根据权限设置教师或管理员cookies
        /// </summary>
        /// <param name="tcmodel"></param>
        /// <param name="permiss"></param>
        /// <returns></returns>
        public static bool SetTMCookies(LearnSite.Model.Teacher tcmodel, bool permiss)
        {
            if (permiss)
                return SetManagerCookies(tcmodel);
            else
                return SetTeacherCookies(tcmodel);
        }

        /// <summary>
        /// 设置教师的cookies值
        /// </summary>
        /// <param name="Model"></param>
        private static bool SetTeacherCookies(LearnSite.Model.Teacher tcmodel)
        {
            if (tcmodel != null)
            {
                HttpCookie TCookies = new HttpCookie(teaCookieNname);
                Model.TeaCook teacook = new Model.TeaCook();
                teacook.Hid = tcmodel.Hid;
                teacook.Hname=UrlEncode(tcmodel.Hname);
                teacook.Hnick= UrlEncode(tcmodel.Hnick);
                teacook.Hroom=UrlEncode(tcmodel.Hroom);
                teacook.Hpermiss=tcmodel.Hpermiss;
                teacook.Ss = teaCookieNname;
                HttpContext.Current.Session[teaCookieNname] = teaCookieNname;
                teacook.SessionId = HttpContext.Current.Session.SessionID;

                TCookies.Value = teacook.ToStr();

                TCookies.Expires = DateTime.Now.AddDays(1);
                TCookies.Path = "/";
                TCookies.HttpOnly = true;
                TCookies.Secure = HttpContext.Current.Request.IsSecureConnection;
                HttpContext.Current.Response.AppendCookie(TCookies);
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 设置管理员的cookies值
        /// </summary>
        /// <param name="Model"></param>
        private static bool SetManagerCookies(LearnSite.Model.Teacher tcmodel)
        {
            if (tcmodel != null)
            {
                HttpCookie MCookies = new HttpCookie(mngCookieNname);

                Model.MngCook mngcook = new Model.MngCook();
                mngcook.Hid = tcmodel.Hid;
                mngcook.Hname = UrlEncode(tcmodel.Hname);
                mngcook.Hnick = UrlEncode(tcmodel.Hnick);
                mngcook.Hroom = UrlEncode(tcmodel.Hroom);
                mngcook.Hpermiss = tcmodel.Hpermiss;
                mngcook.Ss = mngCookieNname;
                HttpContext.Current.Session[mngCookieNname] = mngCookieNname;
                mngcook.SessionId = HttpContext.Current.Session.SessionID;

                MCookies.Value = mngcook.ToStr();

                MCookies.Expires = DateTime.Now.AddDays(1);
                MCookies.Path = "/";
                MCookies.HttpOnly = true;
                MCookies.Secure = HttpContext.Current.Request.IsSecureConnection;
                HttpContext.Current.Response.AppendCookie(MCookies);
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 如果有登录，则返回1
        /// </summary>
        /// <param name="rnd"></param>
        /// <returns></returns>
        public static bool IsTeacher()
        {
            if (HttpContext.Current.Request.Cookies[teaCookieNname] != null)
            {
                return true;
            }
            if (HttpContext.Current.Request.Cookies[stuCookieNname] != null)
            {
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// 跳回首页
        /// </summary>
        public static void JudgeStudentCookies()
        {
            HttpContext.Current.Response.Redirect("~/index.aspx", true);
        }

        public static string SetMainPageTitle()
        {
            string str = LearnSite.Common.XmlHelp.SiteTitle() + "--->";
            return str;
        }
        public static string GetVersion()
        {
            string copyright = " LearnSite" + LearnSite.Common.WordProcess.NewVersion() + " ";
            return copyright;
        }

        private static DateTime StudentCookiesPeriod(string str)
        {
            DateTime myPeriod = DateTime.MinValue;
            switch (str)
            {
                case "1":
                    myPeriod = DateTime.Now.AddHours(1);
                    break;
                case "2":
                    myPeriod = DateTime.Now.AddHours(2);
                    break;
                case "3":
                    myPeriod = DateTime.Now.AddHours(3);
                    break;
                case "4":
                    myPeriod = DateTime.Now.AddHours(12);
                    break;
                case "5":
                    myPeriod = DateTime.Now.AddDays(1);
                    break;
            }
            return myPeriod;
        }
        /// <summary>
        /// 模拟该班级学生登录cookies设置
        /// </summary>
        /// <param name="Sgrade"></param>
        /// <param name="Sclass"></param>
        public static void SimulationStudentCookies(int Sgrade, int Sclass)
        {
            if (HttpContext.Current.Request.Cookies[teaCookieNname] != null)
            {
                Model.TeaCook tcook = new Model.TeaCook();
                string Rhid = tcook.Hid.ToString();
                LearnSite.BLL.Students stubll = new BLL.Students();
                LearnSite.Model.Students sModel = new LearnSite.Model.Students();
                string Syear = stubll.GetYear(Sgrade);
                string rrr = Syear + Sgrade.ToString() + Sclass.ToString();
                sModel.Sid = 0 - Int32.Parse(rrr);//将入学年度、年级和班级作为模拟学生的ID
                sModel.Syear =Int32.Parse( Syear);
                sModel.Snum = "s"+Rhid + Syear +Sgrade.ToString()+ Sclass.ToString();
                sModel.Sgrade = Sgrade;
                sModel.Sclass = Sclass;
                sModel.Sname = "学生" + Sgrade.ToString() + Sclass.ToString() ;
                sModel.Spwd = "12345678";
                sModel.Sex = "男";
                sModel.Sscore = 100;
                sModel.Squiz = 50;
                sModel.Sattitude = 25;
                sModel.Sape = "A";
                sModel.Sgroup = -1;

                DateTime LoginTime = DateTime.Now;
                string LoginIp = LearnSite.Common.Computer.GetGuestIP();
                string ThisTerm = LearnSite.Common.XmlHelp.GetTerm();
                HttpCookie StuCookie = new HttpCookie(stuCookieNname);

                Model.Cook Cook = new Model.Cook();
                Cook.Sid = sModel.Sid;
                Cook.Snum = UrlEncode(sModel.Snum);
                Cook.Syear = sModel.Syear.Value;
                Cook.Sgrade = sModel.Sgrade.Value;
                Cook.Sclass = sModel.Sclass.Value;
                Cook.Sname = UrlEncode(sModel.Sname.Trim());
                Cook.Spwd = Common.WordProcess.GetMD5_16bit(sModel.Spwd);
                Cook.Sex = UrlEncode(sModel.Sex);
                Cook.Sscore = sModel.Sscore.Value;
                Cook.Squiz = sModel.Squiz.Value;
                Cook.Sattitude = sModel.Sattitude.Value;
                Cook.Sape = UrlEncode(sModel.Sape);
                Cook.Sgroup = sModel.Sgroup.Value;
                Cook.LoginTime = LoginTime.ToString();
                Cook.LoginIp = LoginIp;
                Cook.ThisTerm = Int32.Parse(ThisTerm);
                Cook.Rhid = Int32.Parse(Rhid);
                Cook.RankImage = UrlEncode(LearnSite.Common.Rank.RankImage(sModel.Sscore.Value + sModel.Sattitude.Value, true));
                Cook.Ss = stuCookieNname;//cookie名称校验
                Cook.SessionId = "";
                
                StuCookie.Value = Cook.ToStr();

                string str = LearnSite.Common.XmlHelp.GetStudentCookiesPeriod();
                if (str != "0")
                {
                    StuCookie.Expires = StudentCookiesPeriod(str);
                    StuCookie.Path = "/";
                    StuCookie.HttpOnly = true;
                }
                HttpContext.Current.Response.AppendCookie(StuCookie);
                LearnSite.Common.App.AppUserAdd(sModel.Snum);
            }
        }
        
    }
}