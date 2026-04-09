using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
namespace LearnSite.Common
{
    /// <summary>
    ///App 的摘要说明
    /// </summary>
    public class App
    {
        public App()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 返回全局变量数目和初始化值
        /// </summary>
        /// <returns></returns>
        public static string AppCounts()
        {
            string initstr = "";
            Dictionary<string, string> userDict = HttpContext.Current.Application.Get("LearnSite_User_Dict") as Dictionary<string, string>;
            ArrayList kick = HttpContext.Current.Application.Get("LearnSite_User_kick") as ArrayList;

            if (userDict != null)
            {
                int counts = userDict.Count;
                if (counts > 0)
                {
                    initstr = initstr + " 在线:" + counts.ToString();
                }
            }
            if (kick != null)
            {
                int kcounts = kick.Count;
                if (kcounts > 0)
                {
                    string tempstr = "";
                    foreach (string ch in kick)
                    {
                        tempstr = tempstr + ch + ",";
                    }
                    initstr = initstr + " 踢除:" + tempstr.TrimEnd(',');
                }
            }
            return initstr;
        }

        /// <summary>
        /// 检查用户是否已登录
        /// </summary>
        /// <param name="strUserId">学号</param>
        /// <returns>true:已登录(阻止登录); false:未登录(允许登录)</returns>
        public static bool IsLogin(string strUserId)
        {
            bool rb = false;
            HttpContext.Current.Application.Lock();
            Dictionary<string, string> userDict = HttpContext.Current.Application.Get("LearnSite_User_Dict") as Dictionary<string, string>;
            if (userDict != null)
            {
                if (userDict.ContainsKey(strUserId))
                    rb = true;//如果存在，则返回真
            }
            HttpContext.Current.Application.UnLock();
            return rb;
        }

        /// <summary>
        /// 检查用户是否已登录（带IP检测）
        /// 如果用户已登录但IP相同，说明是关闭浏览器后重新打开，允许登录
        /// </summary>
        /// <param name="strUserId">学号</param>
        /// <param name="currentIp">当前登录IP</param>
        /// <returns>true:已在其他IP登录(阻止登录); false:未登录或同一IP(允许登录)</returns>
        public static bool IsLoginWithIp(string strUserId, string currentIp)
        {
            bool rb = false;
            HttpContext.Current.Application.Lock();
            Dictionary<string, string> userDict = HttpContext.Current.Application.Get("LearnSite_User_Dict") as Dictionary<string, string>;
            if (userDict != null && userDict.ContainsKey(strUserId))
            {
                string existingIp = userDict[strUserId];
                // 如果IP不同，才返回true（阻止登录）
                if (!string.IsNullOrEmpty(existingIp) && existingIp != currentIp)
                {
                    rb = true;
                }
                // 如果IP相同，返回false（允许登录，可能是关闭浏览器后重新打开）
            }
            HttpContext.Current.Application.UnLock();
            return rb;
        }

        /// <summary>
        /// 获取用户当前登录的IP
        /// </summary>
        public static string GetUserIp(string strUserId)
        {
            string ip = "";
            HttpContext.Current.Application.Lock();
            Dictionary<string, string> userDict = HttpContext.Current.Application.Get("LearnSite_User_Dict") as Dictionary<string, string>;
            if (userDict != null && userDict.ContainsKey(strUserId))
            {
                ip = userDict[strUserId];
            }
            HttpContext.Current.Application.UnLock();
            return ip;
        }
        public static bool Iskick(string strUserId)
        {
            bool rb = false;
            HttpContext.Current.Application.Lock();
            ArrayList kick = HttpContext.Current.Application.Get("LearnSite_User_kick") as ArrayList;
            if (kick != null)
            {
                if (kick.IndexOf(strUserId) > -1)
                    rb = true;//如果存在，则返回真
            }
            HttpContext.Current.Application.UnLock();
            return rb;
        }
        public static void AppUserAdd(string strUserId)
        {
            HttpContext.Current.Application.Lock();
            Dictionary<string, string> userDict = HttpContext.Current.Application.Get("LearnSite_User_Dict") as Dictionary<string, string>;
            if (userDict == null)
            {
                userDict = new Dictionary<string, string>();
            }
            // 添加或更新用户（Key=学号, Value=IP）
            if (!userDict.ContainsKey(strUserId))
                userDict.Add(strUserId, "");
            HttpContext.Current.Application.Add("LearnSite_User_Dict", userDict);
            HttpContext.Current.Application.UnLock();
        }

        /// <summary>
        /// 添加用户到在线列表（带IP）
        /// </summary>
        public static void AppUserAddWithIp(string strUserId, string loginIp)
        {
            HttpContext.Current.Application.Lock();
            Dictionary<string, string> userDict = HttpContext.Current.Application.Get("LearnSite_User_Dict") as Dictionary<string, string>;
            if (userDict == null)
            {
                userDict = new Dictionary<string, string>();
            }
            // 添加或更新用户及其IP
            userDict[strUserId] = loginIp;
            HttpContext.Current.Application.Add("LearnSite_User_Dict", userDict);
            HttpContext.Current.Application.UnLock();
        }

        public static void AppUserRemove(string strUserId)
        {
            HttpContext.Current.Application.Lock();
            if (HttpContext.Current.Application["LearnSite_User_Dict"] != null)
            {
                Dictionary<string, string> userDict = HttpContext.Current.Application.Get("LearnSite_User_Dict") as Dictionary<string, string>;
                if (userDict != null && userDict.ContainsKey(strUserId))
                {
                    userDict.Remove(strUserId);
                    HttpContext.Current.Application.Add("LearnSite_User_Dict", userDict);
                }
            }
            HttpContext.Current.Application.UnLock();
        }
        /// <summary>
        /// 匹配移除
        /// </summary>
        /// <param name="strUserId"></param>
        public static void AppUserMatchRemove(string strUserId)
        {
            HttpContext.Current.Application.Lock();
            Dictionary<string, string> userDict = HttpContext.Current.Application.Get("LearnSite_User_Dict") as Dictionary<string, string>;
            if (userDict != null && userDict.Count > 0)
            {
                // 找出所有匹配的键
                List<string> keysToRemove = new List<string>();
                foreach (var key in userDict.Keys)
                {
                    if (key.IndexOf(strUserId) > -1)
                        keysToRemove.Add(key);
                }
                // 移除匹配的键
                foreach (var key in keysToRemove)
                {
                    userDict.Remove(key);
                }
                HttpContext.Current.Application.Add("LearnSite_User_Dict", userDict);
            }
            HttpContext.Current.Application.UnLock();
        }
        /// <summary>
        /// 教师退出时，匹配移除当前上课班级学生
        /// </summary>
        /// <param name="strUserId"></param>
        public static void CurrentClassRemove(int Rhid)
        {
            HttpContext.Current.Application.Lock();
            Dictionary<string, string> userDict = HttpContext.Current.Application.Get("LearnSite_User_Dict") as Dictionary<string, string>;
            ArrayList kick = HttpContext.Current.Application.Get("LearnSite_User_kick") as ArrayList;
            if (userDict != null && userDict.Count > 0)
            {
                BLL.Room rbll = new BLL.Room();
                ArrayList students = rbll.GetCurrentClassSnum(Rhid);
                if (students != null)
                {
                    foreach (string stu in students)
                    {
                        if (userDict.ContainsKey(stu))
                            userDict.Remove(stu);

                        if (kick != null && kick.IndexOf(stu) > -1)
                            kick.Remove(stu);
                    }
                }
                HttpContext.Current.Application.Add("LearnSite_User_Dict", userDict);
            }
            HttpContext.Current.Application.UnLock();
        }

        public static void AppKickUserAdd(string strUserId)
        {
            HttpContext.Current.Application.Lock();
            ArrayList kick = HttpContext.Current.Application.Get("LearnSite_User_Kick") as ArrayList;
            if (kick == null)
            {
                kick = new ArrayList();
                kick.Add(strUserId);
            }
            else
            {
                if (kick.IndexOf(strUserId) < 0)
                    kick.Add(strUserId);//如果不存在则添加            
            }
            HttpContext.Current.Application.Add("LearnSite_User_Kick", kick);
            HttpContext.Current.Application.UnLock();
        }

        public static void AppKickUserRemove(string strUserId)
        {
            HttpContext.Current.Application.Lock();
            if (HttpContext.Current.Application["LearnSite_User_Kick"] != null)
            {
                ArrayList kick = HttpContext.Current.Application.Get("LearnSite_User_Kick") as ArrayList;
                if (kick != null)
                {
                    int kcount = kick.Count;
                    if (kcount > 0)
                    {
                        for (int i = 0; i < kcount; i++)
                        {
                            if (kick.IndexOf(strUserId) > -1)
                            {
                                kick.Remove(strUserId);
                            }
                        }
                    }
                }
                HttpContext.Current.Application.Add("LearnSite_User_Kick", kick);
            }
            HttpContext.Current.Application.UnLock();
        }


        /// <summary>
        /// 匹配踢除该班级学生
        /// </summary>
        /// <param name="strUserId"></param>
        public static void GradeClassRemove(int Sgrade, int Sclass)
        {
            HttpContext.Current.Application.Lock();
            Dictionary<string, string> userDict = HttpContext.Current.Application.Get("LearnSite_User_Dict") as Dictionary<string, string>;
            ArrayList kick = HttpContext.Current.Application.Get("LearnSite_User_kick") as ArrayList;
            if (userDict != null && userDict.Count > 0)
            {
                BLL.Room rbll = new BLL.Room();
                ArrayList students = rbll.GetGradeClassSnum(Sgrade, Sclass);
                if (students != null)
                {
                    foreach (string stu in students)
                    {
                        if (userDict.ContainsKey(stu))
                        {
                            userDict.Remove(stu);//如果存在，则移除

                            if (kick == null)
                            {
                                kick = new ArrayList();
                                kick.Add(stu);
                            }
                            else
                            {
                                if (kick.IndexOf(stu) < 0)
                                    kick.Add(stu);//如果不存在则添加            
                            }
                        }
                    }
                }
                HttpContext.Current.Application.Add("LearnSite_User_Dict", userDict);
                HttpContext.Current.Application.Add("LearnSite_User_Kick", kick);
            }
            HttpContext.Current.Application.UnLock();
        }
    }
}