using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization;

namespace LearnSite.Common
{
    /// <summary>
    /// LearnStatus: 管理学生实时学习状态，存储在ASP.NET Application state中
    /// 学生端通过AJAX上报状态，教师端通过AJAX轮询获取
    /// </summary>
    public class LearnStatus
    {
        private const string APP_KEY = "LearnSite_LearnStatus";

        /// <summary>
        /// 学生学习状态模型
        /// </summary>
        public class StudentStatus
        {
            public string Snum { get; set; }       // 学号
            public string Sname { get; set; }       // 姓名
            public int Sgrade { get; set; }         // 年级
            public int Sclass { get; set; }         // 班级
            public int Cid { get; set; }            // 学案ID
            public int Lid { get; set; }            // 当前环节ID
            public string Ltitle { get; set; }      // 当前环节标题
            public string Ltype { get; set; }       // 当前环节类型
            public string Status { get; set; }      // 状态: viewing/working/submitted/idle
            public string UpdateTime { get; set; }  // 最后更新时间
            public int Sid { get; set; }            // 学生ID
        }

        /// <summary>
        /// 获取状态字典（线程安全）
        /// </summary>
        private static Dictionary<string, StudentStatus> GetStatusDict()
        {
            HttpApplicationState app = HttpContext.Current.Application;
            Dictionary<string, StudentStatus> dict = app[APP_KEY] as Dictionary<string, StudentStatus>;
            if (dict == null)
            {
                app.Lock();
                try
                {
                    dict = app[APP_KEY] as Dictionary<string, StudentStatus>;
                    if (dict == null)
                    {
                        dict = new Dictionary<string, StudentStatus>();
                        app[APP_KEY] = dict;
                    }
                }
                finally
                {
                    app.UnLock();
                }
            }
            return dict;
        }

        /// <summary>
        /// 更新学生学习状态
        /// </summary>
        public static void UpdateStatus(string snum, string sname, int sgrade, int sclass,
            int cid, int lid, string ltitle, string ltype, string status, int sid)
        {
            HttpApplicationState app = HttpContext.Current.Application;
            app.Lock();
            try
            {
                Dictionary<string, StudentStatus> dict = app[APP_KEY] as Dictionary<string, StudentStatus>;
                if (dict == null)
                {
                    dict = new Dictionary<string, StudentStatus>();
                    app[APP_KEY] = dict;
                }

                string key = snum; // 使用学号作为唯一键
                StudentStatus ss = new StudentStatus
                {
                    Snum = snum,
                    Sname = sname,
                    Sgrade = sgrade,
                    Sclass = sclass,
                    Cid = cid,
                    Lid = lid,
                    Ltitle = ltitle,
                    Ltype = ltype,
                    Status = status,
                    UpdateTime = DateTime.Now.ToString("HH:mm:ss"),
                    Sid = sid
                };
                dict[key] = ss;
            }
            finally
            {
                app.UnLock();
            }
        }

        /// <summary>
        /// 移除学生状态（下线时调用）
        /// </summary>
        public static void RemoveStatus(string snum)
        {
            HttpApplicationState app = HttpContext.Current.Application;
            app.Lock();
            try
            {
                Dictionary<string, StudentStatus> dict = app[APP_KEY] as Dictionary<string, StudentStatus>;
                if (dict != null && dict.ContainsKey(snum))
                {
                    dict.Remove(snum);
                }
            }
            finally
            {
                app.UnLock();
            }
        }

        /// <summary>
        /// 移除指定班级的所有学生状态
        /// </summary>
        public static void RemoveClassStatus(int sgrade, int sclass)
        {
            HttpApplicationState app = HttpContext.Current.Application;
            app.Lock();
            try
            {
                Dictionary<string, StudentStatus> dict = app[APP_KEY] as Dictionary<string, StudentStatus>;
                if (dict != null)
                {
                    List<string> toRemove = new List<string>();
                    foreach (var kvp in dict)
                    {
                        if (kvp.Value.Sgrade == sgrade && kvp.Value.Sclass == sclass)
                        {
                            toRemove.Add(kvp.Key);
                        }
                    }
                    foreach (string key in toRemove)
                    {
                        dict.Remove(key);
                    }
                }
            }
            finally
            {
                app.UnLock();
            }
        }

        /// <summary>
        /// 获取指定班级的所有学生状态，返回JSON字符串
        /// </summary>
        public static string GetClassStatusJson(int sgrade, int sclass, int cid)
        {
            Dictionary<string, StudentStatus> dict = GetStatusDict();
            List<StudentStatus> result = new List<StudentStatus>();

            // 清理超过30分钟未更新的条目
            DateTime threshold = DateTime.Now.AddMinutes(-30);
            List<string> expired = new List<string>();

            foreach (var kvp in dict)
            {
                DateTime updateTime;
                if (DateTime.TryParse(DateTime.Now.ToString("yyyy-MM-dd ") + kvp.Value.UpdateTime, out updateTime))
                {
                    if (updateTime < threshold)
                    {
                        expired.Add(kvp.Key);
                        continue;
                    }
                }

                if (kvp.Value.Sgrade == sgrade && kvp.Value.Sclass == sclass && (cid <= 0 || kvp.Value.Cid == cid))
                {
                    result.Add(kvp.Value);
                }
            }

            // 异步清理过期条目
            if (expired.Count > 0)
            {
                HttpApplicationState app = HttpContext.Current.Application;
                app.Lock();
                try
                {
                    foreach (string key in expired)
                    {
                        dict.Remove(key);
                    }
                }
                finally
                {
                    app.UnLock();
                }
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(result);
        }

        public static string GetClassStatusJson(int sgrade, int sclass)
        {
            return GetClassStatusJson(sgrade, sclass, 0);
        }

        /// <summary>
        /// 获取指定班级学生在各环节的分布统计，返回JSON
        /// </summary>
        public static string GetClassProgressJson(int sgrade, int sclass, int cid)
        {
            Dictionary<string, StudentStatus> dict = GetStatusDict();

            // 按环节标题统计人数
            Dictionary<string, int> stepCount = new Dictionary<string, int>();
            // 按状态统计
            int viewingCount = 0, workingCount = 0, submittedCount = 0, idleCount = 0;
            int totalOnline = 0;

            foreach (var kvp in dict)
            {
                if (kvp.Value.Sgrade == sgrade && kvp.Value.Sclass == sclass && (cid <= 0 || kvp.Value.Cid == cid))
                {
                    totalOnline++;
                    string title = kvp.Value.Ltitle;
                    if (!string.IsNullOrEmpty(title))
                    {
                        if (stepCount.ContainsKey(title))
                            stepCount[title]++;
                        else
                            stepCount[title] = 1;
                    }

                    switch (kvp.Value.Status)
                    {
                        case "viewing": viewingCount++; break;
                        case "working": workingCount++; break;
                        case "submitted": submittedCount++; break;
                        default: idleCount++; break;
                    }
                }
            }

            // 构建结果
            var progress = new
            {
                total = totalOnline,
                viewing = viewingCount,
                working = workingCount,
                submitted = submittedCount,
                idle = idleCount,
                steps = stepCount
            };

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(progress);
        }

        public static string GetClassProgressJson(int sgrade, int sclass)
        {
            return GetClassProgressJson(sgrade, sclass, 0);
        }
    }
}
