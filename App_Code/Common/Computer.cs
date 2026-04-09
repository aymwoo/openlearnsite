using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Web;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
namespace LearnSite.Common
{
/// <summary>
/// Computer 的摘要说明
/// </summary>
public class Computer
{
	// 定义 Windows API 结构和函数
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	private struct MEMORYSTATUSEX
	{
		public uint dwLength;
		public uint dwMemoryLoad;
		public ulong ullTotalPhys;
		public ulong ullAvailPhys;
		public ulong ullTotalPageFile;
		public ulong ullAvailPageFile;
		public ulong ullTotalVirtual;
		public ulong ullAvailVirtual;
		public ulong ullAvailExtendedVirtual;
	}

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX lpBuffer);

	// 定义 Windows API 函数用于获取系统 CPU 时间
	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool GetSystemTimes(out long lpIdleTime, out long lpKernelTime, out long lpUserTime);

	public Computer()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}
    /// <summary>
    /// 判断是否同网段
    /// </summary>
    /// <returns></returns>
    public static bool IsSameNet()
    {
        string sevip = GetServerIp();
        string userip = GetGuestIP();
        sevip = Cutipnet(sevip);
        userip = Cutipnet(userip);
        return sevip.Equals(userip);
    }

    private static string Cutipnet(string ip)
    {
        int lastpoint = ip.IndexOf('.');//换IP的第一个字段数字，因为内网可能有多个网段 ip.LastIndexOf('.');
        if (lastpoint > -1)
        {
            ip = ip.Substring(0, lastpoint);
        }
        return ip;
    }
    public static string ServerUrl()
    {
        string strServer = "http://" +HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString();
        string strPort = ":" + Convert.ToString(HttpContext.Current.Request.ServerVariables["SERVER_PORT"]);
        string strRoot = "/";
        if (strPort.Trim() == ":80")
        {
            strPort = "";
        }
        string strUrl = strServer + strPort + strRoot;
        return strUrl;
    }
        /// <summary>
        /// 获取登录到现在相隔过去的时间（以分钟为单位），返回int值
        /// </summary>
        /// <returns></returns>
    public static int TimePassed()
    {
        int passtime = 0;
        LearnSite.Model.Cook scook=new Model.Cook();
        DateTime time1 = DateTime.Parse(scook.LoginTime);
        DateTime time2 = DateTime.Now;
        passtime = Convert.ToInt32(DatagoneMinute(time1, time2));

        return passtime;
    }
        /// <summary>
        /// 获取登录到现在相隔过去的时间（以分钟为单位），返回int值
        /// </summary>
        /// <returns></returns>
        public static int TimePassed(string LoginTime)
        {
            DateTime time1 = DateTime.Parse(LoginTime);
            DateTime time2 = DateTime.Now;
            return Convert.ToInt32(DatagoneMinute(time1, time2));
        }

        /// <summary>
        /// 判断该日期是否为今天
        /// </summary>
        /// <param name="oldTime"></param>
        /// <returns></returns>
        public static bool IsToday(string oldTime)
        {
            try
            {
                DateTime today = DateTime.Now;
                DateTime oldday = DateTime.Parse(oldTime);
                TimeSpan ts = today - oldday;
                if (ts.Days > 0)
                    return false;
                else
                    return true;
            }
            catch
            {
                return false;
            }
        }
    /// <summary>
    /// 返回Time1-Time2后的天数
    /// </summary>
    /// <param name="Time1"></param>
    /// <param name="Time2"></param>
    /// <returns></returns>
        public static int Daygone(DateTime Time1, DateTime Time2)
        {
            TimeSpan ts = Time1 - Time2;
            double dnum = Math.Round(ts.TotalDays, 0);
            return (int)dnum;
        }
        /// <summary>
        /// 返回OldTime后的天数
        /// </summary>
        /// <param name="OldTime"></param>
        /// <returns></returns>
        public static int Daysgone(DateTime OldTime)
        {
            DateTime today = DateTime.Now;
            TimeSpan ts = today - OldTime;
            double dnum = Math.Round(ts.TotalDays, 0);
            return (int)dnum;
        }
        /// <summary>
        /// 返回Time后的小时数
        /// </summary>
        /// <param name="OldTime"></param>
        /// <returns></returns>
        public static int Hourgone(DateTime OldTime)
        {
            DateTime today = DateTime.Now;
            TimeSpan ts = today - OldTime;
            double dnum = Math.Round(ts.TotalHours, 0);
            return (int)dnum;
        }
        /// <summary>
        /// 获取时间间隔，以秒为单位
        /// </summary>
        /// <param name="Time1"></param>
        /// <param name="Time2"></param>
        /// <returns></returns>
        public static string Datagone(DateTime Time1, DateTime Time2)
        {
            TimeSpan ts = new TimeSpan(Time2.Ticks - Time1.Ticks);
            return ((int)ts.TotalSeconds).ToString();
        }
        /// <summary>
        /// 获取时间间隔，以毫秒为单位
        /// </summary>
        /// <param name="Time1">旧</param>
        /// <param name="Time2">新</param>
        /// <returns></returns>
        public static string DatagoneMilliseconds(DateTime Time1, DateTime Time2)
        {
            TimeSpan ts = new TimeSpan(Time2.Ticks - Time1.Ticks);
            return ((int)ts.TotalMilliseconds).ToString();
        }
        /// <summary>
        /// 获取时间间隔，以分为单位(Time1旧，Time2新)
        /// </summary>
        /// <param name="Time1">旧</param>
        /// <param name="Time2">新</param>
        /// <returns></returns>
        public static string DatagoneMinute(DateTime Time1, DateTime Time2)
        {
            TimeSpan ts = new TimeSpan(Time2.Ticks - Time1.Ticks);
            return ((int)ts.TotalMinutes).ToString();
        }
        /// <summary>
        /// 获取时间间隔，以分为单位(Time1旧，Time2新)
        /// </summary>
        /// <param name="Time1">旧</param>
        /// <param name="Time2">新</param>
        /// <returns></returns>
        public static int GoneMinute(DateTime Time1, DateTime Time2)
        {
            TimeSpan ts = new TimeSpan(Time2.Ticks - Time1.Ticks);
            int t = (int)ts.TotalMinutes;
            if (t == 0)
                t = 1;
            return t;
        }

        /// <summary>
        /// .NET解释引擎版本
        /// </summary>
        /// <returns></returns>
        public static string GetNetCLR()
        {
            string str = Environment.Version.Major + "." + Environment.Version.Minor + "." + Environment.Version.Build + "." + Environment.Version.Revision;
            return str;
        }
        /// <summary>
        /// 虚拟目录Session总数
        /// </summary>
        /// <returns></returns>
        public static string GetSessionCount()
        {
            return HttpContext.Current.Session.Count.ToString();
        }
        /// <summary>
        /// 服务器区域语言
        /// </summary>
        /// <returns></returns>
        public static string GetServerLanguage()
        {
            string str = HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"].ToString();
            return str;
        }
        /// <summary>
        /// 本页执行时间
        /// </summary>
        /// <returns></returns>
        public static string GetPageprocess()
        {
            return HttpContext.Current.Server.ScriptTimeout.ToString() ;
        }
       //2014-3-14 要穿透代理
        public static string MyIp()
        {
            return GetGuestIP();
        }
        /// <summary>
        /// 获得客户端用户IP
        /// </summary>
        /// <returns></returns>
        public static string GetGuestIP()
        {
            //HTTP_X_FORWARDED_FOR透过代理服务器获取客户端IP
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == result || result == String.Empty)
            {
                //如果没有代理则直接获取
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (null == result || result == String.Empty)
            {
                //还是没有获取到就用UserHostAddress获取
                result = HttpContext.Current.Request.UserHostAddress;
            }
            return result;
        }
        private string getIp()
        {
            // 穿过代理服务器取远程用户真实IP地址
            string Ip = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
                {
                    if (HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null)
                        Ip = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].ToString();
                    else
                        if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
                            Ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                        else
                            Ip = "202.96.134.133";
                }
                else
                    Ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
            {
                Ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            else
            {
                Ip = "202.96.134.133";
            }
            return Ip;
        }
        /// <summary> 
        /// 取得客户端真实IP。如果有代理则取第一个非内网地址 
        /// </summary> 
        public static string IPAddress
        {

            get
            {
                string result = String.Empty;

                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (result != null && result != String.Empty)
                {
                    //可能有代理 
                    if (result.IndexOf(".") == -1)    //没有“.”肯定是非IPv4格式 
                        result = null;
                    else
                    {
                        if (result.IndexOf(",") != -1)
                        {
                            //有“,”，估计多个代理。取第一个不是内网的IP。 
                            result = result.Replace(" ", "").Replace("'", "");
                            string[] temparyip = result.Split(",;".ToCharArray());
                            for (int i = 0; i < temparyip.Length; i++)
                            {

                                if (IsIPAddress(temparyip[i])
                                    && temparyip[i].Substring(0, 3) != "10."
                                    && temparyip[i].Substring(0, 7) != "192.168"
                                    && temparyip[i].Substring(0, 7) != "172.16.")
                                {
                                    return temparyip[i];    //找到不是内网的地址 
                                }
                            }
                        }
                        else if (IsIPAddress(result)) //代理即是IP格式 
                            return result;
                        else
                            result = null;    //代理中的内容 非IP，取IP 
                    }

                }

                string IpAddress = (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null && HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty) ? HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] : HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];


                if (null == result || result == String.Empty)
                    result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                if (result == null || result == String.Empty)
                    result = HttpContext.Current.Request.UserHostAddress;

                return result;
            }
        } 
        private static bool IsIPAddress(string str1)
        {
            if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;

            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";

            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        } 
        /// <summary>
        /// 获得客户端主机名
        /// </summary>
        /// <returns></returns>
        public static string GetGuestHost(string ssIp)
        {
            try
            {
                System.Net.IPAddress ip = System.Net.IPAddress.Parse(ssIp);
                IPHostEntry iph;
                iph = System.Net.Dns.GetHostEntry(ip);
                return iph.HostName;
            }
            catch
            {
                return ssIp;
            }
        }
        /// <summary>
        /// 获取客户端主机名
        /// </summary>
        /// <param name="hostIp"></param>
        /// <returns></returns>
        public static string GetHostNameByPage(System.Web.UI.Page thePage)
        {
            return thePage.Request.UserHostName;//取不到主机名，取的是IP
        }


        /// <summary>
        /// 获得客户端浏览器IE
        /// </summary>
        /// <returns></returns>
        public static string GetGuestBrowse()
        {
            return HttpContext.Current.Request.Browser.Browser.ToString();
        }

        public static string GetGuestBrowsever()
        {
            return HttpContext.Current.Request.Browser.MajorVersion.ToString();
        }
        /// <summary>
        /// 获得客户端操作系统
        /// </summary>
        /// <returns></returns>
        public static string GetGuestPlatform()
        {
            return HttpContext.Current.Request.Browser.Platform.ToString();
        }
        /// <summary>
        /// 获得服务器IP
        /// </summary>
        /// <returns></returns>
        public static string GetServerIp()
        {
            return HttpContext.Current.Request.ServerVariables.Get("Local_Addr").ToString();
        }
        /// <summary>
        /// 获得服务器名称
        /// </summary>
        /// <returns></returns>
        public static string GetServerHost()
        {
            return HttpContext.Current.Request.ServerVariables.Get("Server_Name").ToString();
        }
        /// <summary>
        /// 获得服务器计算名
        /// </summary>
        /// <returns></returns>
        public static string GetServerName()
        {
            return HttpContext.Current.Server.MachineName;
        }
        /// <summary>
        /// 获得服务器操作系统
        /// </summary>
        /// <returns></returns>
        public static string GetServerOs()
        {
            return System.Environment.OSVersion.ToString();
        }
        /// <summary>
        /// 获得服务器CPU数
        /// </summary>
        /// <returns></returns>
        public static string GetServerCpu()
        {
            return Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS");
        }
        /// <summary>
        /// 获得CPU类型
        /// </summary>
        /// <returns></returns>
        public static string GetServerCpuClass()
        {
            return Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER");
        }
        /// <summary>
        /// 获得信息服务软件
        /// </summary>
        /// <returns></returns>
        public static string GetServerSoftWare()
        {
            return HttpContext.Current.Request.ServerVariables["SERVER_SOFTWARE"];
        }
        /// <summary>
        /// 获得DOTNET 版本
        /// </summary>
        /// <returns></returns>
        public static string GetServerDotNetVer()
        {
            return System.Environment.Version.ToString();
        }
        /// <summary>
        /// 获得脚本超时时间
        /// </summary>
        /// <returns></returns>
        public static string GetServerScriptTimeout()
        {
            return HttpContext.Current.Server.ScriptTimeout.ToString();
        }
        /// <summary>
        /// 获得开机运行时长
        /// </summary>
        /// <returns></returns>
        public static string GetServerTickCount()
        {
            int result = Environment.TickCount & Int32.MaxValue;
            TimeSpan m_WorkTimeTemp = new TimeSpan(Convert.ToInt64(Convert.ToInt64(result) * 10000));
            string m_WorkTime = m_WorkTimeTemp.Days + " 天 " + m_WorkTimeTemp.Hours + " 小时 " + m_WorkTimeTemp.Minutes + " 分钟 " + m_WorkTimeTemp.Seconds + " 秒";

            return m_WorkTime;
        }
        /// <summary>
        /// 获得进程开始时间
        /// </summary>
        /// <returns></returns>
        public static string GetServerProcessStartTime()
        {
            string starttime = "";
            try
            {
                starttime = System.Diagnostics.Process.GetCurrentProcess().StartTime.ToString();
            }
            catch
            {
                starttime = "未知";
            }
            return starttime;
        }
        /// <summary>
        /// 获得AspNet内存占用
        /// </summary>
        /// <returns></returns>
        public static string GetServerAspNetWorkingSet()
        {
            string aspnetmemory = "";
            try
            {
                aspnetmemory = ((Double)System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1048576).ToString("N2");
            }
            catch
            {
                aspnetmemory = "未知";
            }
            return aspnetmemory;
        }
        /// <summary>
        /// 获得AspNet CPU时间
        /// </summary>
        /// <returns></returns>
        public static string GetServerAspNetCpuTime()
        {
            string aspnetcuptime;
            try
            {
                aspnetcuptime = ((TimeSpan)System.Diagnostics.Process.GetCurrentProcess().TotalProcessorTime).TotalSeconds.ToString("N0");
            }
            catch
            {
                aspnetcuptime = "未知";
            }
            return aspnetcuptime;
        }
        /// <summary>
        /// 获得当前AspNet运行线程数
        /// </summary>
        /// <returns></returns>
        public static string GetServerCurrentThreadsNum()
        {
            int Threadcount = 0;
            foreach (System.Diagnostics.ProcessThread thread in System.Diagnostics.Process.GetCurrentProcess().Threads)
            { Threadcount++; }
            return Threadcount.ToString();
        }

        /// <summary>
        /// 获取今天气温，远程捕获
        /// </summary>
        /// <param name="strUrl"></param>
        /// <returns></returns>
        public static string GetWeatherToday()
        {
            string strUrl = System.Configuration.ConfigurationManager.AppSettings["Weather"].Trim();
            string WeatherToday = "  没有天气预报";
            if (UrlExistsUsingSockets(strUrl))
            {
                WebRequest wreq = WebRequest.Create(strUrl);
                using (WebResponse wresp = wreq.GetResponse())
                using (Stream s = wresp.GetResponseStream())
                using (StreamReader sr = new StreamReader(s, System.Text.Encoding.GetEncoding("utf-8")))
                {
                    string HTML = sr.ReadToEnd();
                    int laststr = HTML.LastIndexOf("℃");
                    if (laststr > 0)
                    {
                        string Newhtml = HTML.Substring(0, laststr + 1);
                        int startstr = Newhtml.LastIndexOf(">");
                        int mylen = laststr - startstr;
                        WeatherToday = Newhtml.Substring(startstr + 1, mylen);
                        WeatherToday = " 今天天气：" + WeatherToday;
                    }
                }
            }
            return WeatherToday;
        }

        /// <summary>
        /// 方法一、检测远程url是否存在
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static bool UrlExistsUsingHttpWebRequest(string url)
        {
            try
            {
                System.Net.HttpWebRequest myRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                myRequest.Method = "HEAD";
                myRequest.Timeout = 100;
                using (System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)myRequest.GetResponse())
                {
                    return (res.StatusCode == System.Net.HttpStatusCode.OK);
                }
            }
            catch (System.Net.WebException we)
            {
                System.Diagnostics.Trace.Write(we.Message);
                return false;
            }
        }
        /// <summary>
        /// 方法二、检测远程url是否存在
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static bool UrlExistsUsingSockets(string url)
        {
            if (url.StartsWith("http://")) url = url.Remove(0, "http://".Length);
            try
            {
                System.Net.IPHostEntry ipHost = System.Net.Dns.GetHostEntry(url);//GetHostEntry
                return true;
            }
            catch (System.Net.Sockets.SocketException se)
            {
                System.Diagnostics.Trace.Write(se.Message);
                return false;
            }
        }

        /// <summary>
        /// 从IP地址中提取网段前缀（前三段）
        /// </summary>
        /// <param name="ip">完整IP地址，如 172.16.3.100</param>
        /// <returns>网段前缀，如 172.16.3</returns>
        public static string GetIpNetPrefix(string ip)
        {
            if (string.IsNullOrEmpty(ip))
                return string.Empty;

            string[] parts = ip.Split('.');
            if (parts.Length >= 3)
            {
                return parts[0] + "." + parts[1] + "." + parts[2];
    }
            return string.Empty;
}

        /// <summary>
        /// 根据IP地址获取对应的机房ID
        /// 通过网段配置表查询
        /// </summary>
        /// <param name="ip">客户端IP地址</param>
        /// <returns>机房ID，如果未配置则返回null</returns>
        public static int? GetHidByIp(string ip)
        {
            if (string.IsNullOrEmpty(ip))
                return null;

            try
            {
                LearnSite.BLL.IpNet bll = new LearnSite.BLL.IpNet();
                return bll.GetHidByIp(ip);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 根据IP地址获取对应的网段名称
        /// </summary>
        /// <param name="ip">客户端IP地址</param>
        /// <returns>网段名称</returns>
        public static string GetNetNameByIp(string ip)
        {
            if (string.IsNullOrEmpty(ip))
                return string.Empty;

            try
            {
                string netPrefix = GetIpNetPrefix(ip);
                if (!string.IsNullOrEmpty(netPrefix))
                {
                    LearnSite.BLL.IpNet bll = new LearnSite.BLL.IpNet();
                    int? hid = bll.GetHidByNet(netPrefix);
                    if (hid.HasValue)
                    {
                        LearnSite.BLL.House houseBll = new LearnSite.BLL.House();
                        LearnSite.Model.House house = houseBll.GetModel(hid.Value);
                        if (house != null)
                        {
                            return house.Hname;
                        }
                    }
                }
            }
            catch
            {
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取内存使用情况
        /// </summary>
        /// <returns>内存使用情况，格式："总量: X MB, 已用: Y MB, 可用: Z MB, 缓存: W MB"</returns>
        public static string GetMemoryInfo()
        {
            try
            {
                // 检测当前平台
                bool isWindows = Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Platform == PlatformID.Win32S || 
                                Environment.OSVersion.Platform == PlatformID.Win32Windows || Environment.OSVersion.Platform == PlatformID.WinCE;
                
                if (isWindows)
                {
                    // 尝试使用 Windows API 获取系统内存信息
                    MEMORYSTATUSEX memStatus = new MEMORYSTATUSEX();
                    memStatus.dwLength = (uint)Marshal.SizeOf(memStatus);
                    
                    if (GlobalMemoryStatusEx(ref memStatus))
                    {
                        int totalMemoryMB = (int)(memStatus.ullTotalPhys / (1024 * 1024));
                        int availableMemoryMB = (int)(memStatus.ullAvailPhys / (1024 * 1024));
                        int usedMemoryMB = totalMemoryMB - availableMemoryMB;
                        uint memoryLoad = memStatus.dwMemoryLoad;
                        
                        // 显示系统总内存、已用内存、可用内存和内存使用率
                        return string.Format("总量: {0:N0} MB, 已用: {1:N0} MB, 可用: {2:N0} MB, 使用率: {3}%", totalMemoryMB, usedMemoryMB, availableMemoryMB, memoryLoad);
                    }
                    else
                    {
                        // 如果 Windows API 调用失败，返回当前进程的内存使用情况
                        System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                        long memoryUsage = currentProcess.WorkingSet64 / (1024 * 1024); // 转换为 MB
                        return string.Format("当前进程: {0:N0} MB (Windows API 调用失败)", memoryUsage);
                    }
                }
                else
                {
                    // 在非 Windows 平台上，返回当前进程的内存使用情况
                    System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                    long memoryUsage = currentProcess.WorkingSet64 / (1024 * 1024); // 转换为 MB
                    return string.Format("当前进程: {0:N0} MB (非 Windows 平台)", memoryUsage);
                }
            }
            catch (Exception ex)
            {
                // 如果出现异常，返回当前进程的内存使用情况
                try
                {
                    System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                    long memoryUsage = currentProcess.WorkingSet64 / (1024 * 1024); // 转换为 MB
                    return string.Format("当前进程: {0:N0} MB (错误: {1})", memoryUsage, ex.Message);
                }
                catch (Exception ex2)
                {
                    return "获取内存信息失败: " + ex2.Message;
                }
            }
        }

        /// <summary>
        /// 获取磁盘分区使用情况
        /// </summary>
        /// <returns>磁盘分区使用情况，格式："C: X% (X GB/X GB), D: Y% (Y GB/Y GB), ..."</returns>
        public static string GetDiskInfo()
        {
            try
            {
                System.Text.StringBuilder diskInfo = new System.Text.StringBuilder();
                foreach (System.IO.DriveInfo drive in System.IO.DriveInfo.GetDrives())
                {
                    if (drive.IsReady && drive.DriveType == System.IO.DriveType.Fixed)
                    {
                        long totalSpace = drive.TotalSize / (1024 * 1024 * 1024); // 转换为 GB
                        long freeSpace = drive.AvailableFreeSpace / (1024 * 1024 * 1024); // 转换为 GB
                        long usedSpace = totalSpace - freeSpace;
                        int usagePercent = (int)((double)usedSpace / totalSpace * 100);
                        diskInfo.AppendFormat("{0}: {1}% ({2} GB/{3} GB), ", drive.Name, usagePercent, usedSpace, totalSpace);
                    }
                }
                if (diskInfo.Length > 0)
                {
                    diskInfo.Length -= 2; // 移除最后一个逗号和空格
                }
                return diskInfo.ToString();
            }
            catch
            {
            }
            return "获取磁盘信息失败";
        }

        /// <summary>
        /// 获取占用 CPU/内存最高的进程 TOP10
        /// </summary>
        /// <returns>进程列表，格式："进程名 (PID): CPU%/内存MB, ..."</returns>
        public static string GetTopProcesses()
        {
            try
            {
                var processes = System.Diagnostics.Process.GetProcesses()
                    .Where(p => p.ProcessName != "Idle")
                    .OrderByDescending(p => {
                        try { return p.WorkingSet64; }
                        catch { return 0L; }
                    })
                    .Take(10);
                
                System.Text.StringBuilder processInfo = new System.Text.StringBuilder();
                foreach (var process in processes)
                {
                    try
                    {
                        string processName = process.ProcessName;
                        int processId = process.Id;
                        long memoryUsage = process.WorkingSet64 / (1024 * 1024); // 转换为 MB
                        
                        processInfo.AppendFormat("{0} ({1}): {2:N0}MB, ", processName, processId, memoryUsage);
                    }
                    catch
                    {
                        // 忽略无法获取信息的进程
                    }
                }
                
                if (processInfo.Length > 0)
                {
                    processInfo.Length -= 2; // 移除最后一个逗号和空格
                }
                return processInfo.ToString();
            }
            catch
            {
            }
            return "获取进程信息失败";
        }

        /// <summary>
        /// 获取网络使用率
        /// </summary>
        /// <returns>网络使用率，格式："网卡1: 发送X%/接收X%, 网卡2: 发送X%/接收X%, ..."</returns>
        public static string GetNetworkUsage()
        {
            try
            {
                // 存储第一次网络统计信息
                Dictionary<string, Tuple<long, long>> firstStats = new Dictionary<string, Tuple<long, long>>();
                foreach (System.Net.NetworkInformation.NetworkInterface nic in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up)
                    {
                        var stats = nic.GetIPv4Statistics();
                        firstStats[nic.Name] = new Tuple<long, long>(stats.BytesSent, stats.BytesReceived);
                    }
                }
                
                // 等待一段时间，让系统有时间积累网络流量
                System.Threading.Thread.Sleep(1000);
                
                // 计算网络使用率
                System.Text.StringBuilder networkInfo = new System.Text.StringBuilder();
                foreach (System.Net.NetworkInformation.NetworkInterface nic in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (nic.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up && firstStats.ContainsKey(nic.Name))
                    {
                        var stats = nic.GetIPv4Statistics();
                        long bytesSent = stats.BytesSent - firstStats[nic.Name].Item1;
                        long bytesReceived = stats.BytesReceived - firstStats[nic.Name].Item2;
                        
                        // 估计网络带宽（这里使用 100Mbps 作为默认值，实际应该根据网卡的最大带宽来计算）
                        long estimatedBandwidth = 100 * 1024 * 1024 / 8; // 100Mbps 转换为字节/秒
                        
                        // 计算使用率百分比
                        double sendUsage = 0;
                        double receiveUsage = 0;
                        if (estimatedBandwidth > 0)
                        {
                            sendUsage = (double)bytesSent / estimatedBandwidth * 100;
                            receiveUsage = (double)bytesReceived / estimatedBandwidth * 100;
                        }
                        
                        networkInfo.AppendFormat("{0}: 发送{1:N1}%/接收{2:N1}%, ", nic.Name, sendUsage, receiveUsage);
                    }
                }
                
                if (networkInfo.Length > 0)
                {
                    networkInfo.Length -= 2; // 移除最后一个逗号和空格
                }
                return networkInfo.ToString();
            }
            catch (Exception ex)
            {
                return "获取网络使用率失败: " + ex.Message;
            }
        }

        /// <summary>
        /// 获取系统负载
        /// </summary>
        /// <returns>系统负载，格式："CPU 使用率: X%, 系统平均负载: Y"</returns>
        public static string GetSystemLoad()
        {
            try
            {
                // 检测当前平台
                bool isWindows = Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Platform == PlatformID.Win32S || 
                                Environment.OSVersion.Platform == PlatformID.Win32Windows || Environment.OSVersion.Platform == PlatformID.WinCE;
                
                if (isWindows)
                {
                    // 尝试使用 Windows API 获取系统 CPU 使用率
                    long idleTime1, kernelTime1, userTime1;
                    long idleTime2, kernelTime2, userTime2;
                    
                    // 获取第一次时间
                    if (!GetSystemTimes(out idleTime1, out kernelTime1, out userTime1))
                    {
                        // 如果 Windows API 调用失败，返回当前进程的 CPU 使用情况
                        System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                        System.TimeSpan cpuTime = currentProcess.TotalProcessorTime;
                        System.TimeSpan elapsedTime = System.DateTime.Now - currentProcess.StartTime;
                        double processCpuUsage = 0;
                        
                        if (elapsedTime.TotalMilliseconds > 0)
                        {
                            // 计算当前进程的 CPU 使用率（近似值）
                            processCpuUsage = (cpuTime.TotalMilliseconds / elapsedTime.TotalMilliseconds) * 100;
                        }
                        
                        return string.Format("当前进程 CPU: {0:N1}% (Windows API 调用失败)", processCpuUsage);
                    }
                    
                    // 等待一段时间，让系统有时间积累 CPU 时间
                    System.Threading.Thread.Sleep(100);
                    
                    // 获取第二次时间
                    if (!GetSystemTimes(out idleTime2, out kernelTime2, out userTime2))
                    {
                        // 如果 Windows API 调用失败，返回当前进程的 CPU 使用情况
                        System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                        System.TimeSpan cpuTime = currentProcess.TotalProcessorTime;
                        System.TimeSpan elapsedTime = System.DateTime.Now - currentProcess.StartTime;
                        double processCpuUsage = 0;
                        
                        if (elapsedTime.TotalMilliseconds > 0)
                        {
                            // 计算当前进程的 CPU 使用率（近似值）
                            processCpuUsage = (cpuTime.TotalMilliseconds / elapsedTime.TotalMilliseconds) * 100;
                        }
                        
                        return string.Format("当前进程 CPU: {0:N1}% (Windows API 调用失败)", processCpuUsage);
                    }
                    
                    // 计算总时间差
                    long totalTimeDiff = (kernelTime2 - kernelTime1) + (userTime2 - userTime1);
                    // 计算空闲时间差
                    long idleTimeDiff = idleTime2 - idleTime1;
                    
                    // 计算 CPU 使用率
                    double systemCpuUsage = 0;
                    if (totalTimeDiff > 0)
                    {
                        systemCpuUsage = ((double)(totalTimeDiff - idleTimeDiff) / totalTimeDiff) * 100;
                    }
                    
                    return string.Format("CPU 使用率: {0:N1}%", systemCpuUsage);
                }
                else
                {
                    // 在非 Windows 平台上，返回当前进程的 CPU 使用情况
                    System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                    System.TimeSpan cpuTime = currentProcess.TotalProcessorTime;
                    System.TimeSpan elapsedTime = System.DateTime.Now - currentProcess.StartTime;
                    double processCpuUsage = 0;
                    
                    if (elapsedTime.TotalMilliseconds > 0)
                    {
                        // 计算当前进程的 CPU 使用率（近似值）
                        processCpuUsage = (cpuTime.TotalMilliseconds / elapsedTime.TotalMilliseconds) * 100;
                    }
                    
                    return string.Format("当前进程 CPU: {0:N1}% (非 Windows 平台)", processCpuUsage);
                }
            }
            catch (Exception ex)
            {
                // 如果出现异常，返回当前进程的 CPU 使用情况
                try
                {
                    System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                    System.TimeSpan cpuTime = currentProcess.TotalProcessorTime;
                    System.TimeSpan elapsedTime = System.DateTime.Now - currentProcess.StartTime;
                    double processCpuUsage = 0;
                    
                    if (elapsedTime.TotalMilliseconds > 0)
                    {
                        // 计算当前进程的 CPU 使用率（近似值）
                        processCpuUsage = (cpuTime.TotalMilliseconds / elapsedTime.TotalMilliseconds) * 100;
                    }
                    
                    return string.Format("当前进程 CPU: {0:N1}% (错误: {1})", processCpuUsage, ex.Message);
                }
                catch (Exception ex2)
                {
                    return "获取系统负载失败: " + ex2.Message;
                }
            }
        }
    }
}
