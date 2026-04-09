<%@ WebHandler Language="C#" Class="LabLoginApi" %>

using System;
using System.Web;
using System.Data;
using System.Text;
using System.Collections.Generic;

public class LabLoginApi : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.Charset = "utf-8";
        
        string action = context.Request["action"] ?? "";
        string result = "";
        
        try
        {
            switch (action)
            {
                case "getclassinfo":
                    result = GetClassInfo(context);
                    break;
                case "getstudents":
                    result = GetStudents(context);
                    break;
                case "login":
                    result = StudentLogin(context);
                    break;
                case "checkteacher":
                    result = CheckTeacherControl(context);
                    break;
                default:
                    result = "{\"code\":-1,\"msg\":\"未知操作\"}";
                    break;
            }
        }
        catch (Exception ex)
        {
            result = "{\"code\":-1,\"msg\":\"" + ex.Message.Replace("\"", "\\\"") + "\"}";
        }
        
        context.Response.Write(result);
    }
    
    private string GetClassInfo(HttpContext context)
    {
        LearnSite.BLL.Room rbll = new LearnSite.BLL.Room();
        DataTable dt = rbll.GetCurrentTeachingClass(GetCurrentTeacherHid(context));
        
        if (dt == null || dt.Rows.Count == 0)
        {
            return "{\"code\":0,\"msg\":\"当前没有上课班级\",\"data\":null}";
        }
        
        int grade = Convert.ToInt32(dt.Rows[0]["Rgrade"]);
        int classNum = Convert.ToInt32(dt.Rows[0]["Rclass"]);
        
        LearnSite.BLL.Courses cbll = new LearnSite.BLL.Courses();
        string cid = rbll.IsRopenRcid(grade, classNum);
        string courseTitle = "";
        
        if (!string.IsNullOrEmpty(cid))
        {
            LearnSite.Model.Courses course = cbll.GetModel(int.Parse(cid));
            if (course != null)
            {
                courseTitle = course.Ctitle;
            }
        }
        
        StringBuilder sb = new StringBuilder();
        sb.Append("{");
        sb.Append("\"code\":1,");
        sb.Append("\"msg\":\"成功\",");
        sb.Append("\"data\":{");
        sb.Append("\"grade\":" + grade + ",");
        sb.Append("\"class\":" + classNum + ",");
        sb.Append("\"course\":\"" + courseTitle + "\",");
        sb.Append("\"term\":" + LearnSite.Common.XmlHelp.GetIntTerm());
        sb.Append("}}");
        
        return sb.ToString();
    }
    
    private string GetStudents(HttpContext context)
    {
        int grade = 0, classNum = 0;
        int.TryParse(context.Request["grade"], out grade);
        int.TryParse(context.Request["class"], out classNum);
        
        if (grade == 0 || classNum == 0)
        {
            return "{\"code\":0,\"msg\":\"参数错误\"}";
        }
        
        string ip = GetClientIP(context);
        LearnSite.BLL.Computers cbll = new LearnSite.BLL.Computers();
        string machineName = cbll.GetmachineByIp(ip);
        
        LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
        DataTable dt = sbll.GetListByGradeClass(grade, classNum);
        
        if (dt == null || dt.Rows.Count == 0)
        {
            return "{\"code\":0,\"msg\":\"没有学生数据\"}";
        }
        
        StringBuilder sb = new StringBuilder();
        sb.Append("{\"code\":1,");
        sb.Append("\"machine\":\"" + machineName + "\",");
        sb.Append("\"ip\":\"" + ip + "\",");
        sb.Append("\"students\":[");
        
        bool first = true;
        foreach (DataRow row in dt.Rows)
        {
            if (!first) sb.Append(",");
            first = false;
            
            string snum = row["Snum"].ToString();
            string sname = row["Sname"].ToString();
            string sseat = row["Sseat"] != null ? row["Sseat"].ToString() : "";
            
            sb.Append("{");
            sb.Append("\"snum\":\"" + snum + "\",");
            sb.Append("\"sname\":\"" + sname + "\",");
            sb.Append("\"sseat\":\"" + sseat + "\"");
            sb.Append("}");
        }
        
        sb.Append("]}");
        return sb.ToString();
    }
    
    private string StudentLogin(HttpContext context)
    {
        string snum = context.Request["snum"] ?? "";
        string password = context.Request["password"] ?? "";
        string ip = GetClientIP(context);
        int grade = 0, classNum = 0;
        int.TryParse(context.Request["grade"], out grade);
        int.TryParse(context.Request["class"], out classNum);
        
        if (string.IsNullOrEmpty(snum) || string.IsNullOrEmpty(password))
        {
            return "{\"code\":0,\"msg\":\"学号和密码不能为空\"}";
        }
        
        LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
        LearnSite.Model.Students model = null;
        
        int loginMode = LearnSite.Common.XmlHelp.LoginMode();
        if (loginMode == 1)
        {
            if (sbll.ExistsLogin(snum, password))
            {
                model = sbll.SnumGetModel(snum);
            }
            else if (sbll.isRlogin(snum))
            {
                model = sbll.GetStudentModel(snum, password);
            }
        }
        else
        {
            model = sbll.GetStudentModel(snum, password);
        }
        
        if (model == null)
        {
            return "{\"code\":0,\"msg\":\"学号或密码错误\"}";
        }
        
        int Qgrade = model.Sgrade.Value;
        int Qclass = model.Sclass.Value;
        int Qsid = model.Sid;
        string Qname = model.Sname;
        int Qsyear = model.Syear.Value;
        int Qterm = LearnSite.Common.XmlHelp.GetIntTerm();
        string Qsession = LearnSite.Common.TimeSlotHelper.GetCurrentTimeSlot();
        
        LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
        string cid = rm.IsRopenRcid(Qgrade, Qclass);
        string Qtitle = "";
        if (!string.IsNullOrEmpty(cid))
        {
            LearnSite.BLL.Courses cs = new LearnSite.BLL.Courses();
            LearnSite.Model.Courses courseModel = cs.GetModel(int.Parse(cid));
            if (courseModel != null)
            {
                Qtitle = courseModel.Ctitle;
            }
        }
        
        LearnSite.BLL.Signin gbll = new LearnSite.BLL.Signin();
        gbll.SigninToday(snum, DateTime.Now, ip, Qgrade, Qterm, Qsid, Qname, Qclass, Qsyear, Qtitle, Qsession);
        
        LearnSite.BLL.Computers cbll = new LearnSite.BLL.Computers();
        string machineName = cbll.GetmachineByIp(ip);
        
        if (rm.IsLoginLock(Qgrade, Qclass) && string.IsNullOrEmpty(model.Sseat))
        {
            sbll.UpdateFixedSeat(snum, machineName);
        }
        
        string baseUrl = GetBaseUrl(context);
        string redirectUrl = baseUrl + "student/showcourse.aspx?Cid=" + cid;
        if (string.IsNullOrEmpty(cid))
        {
            redirectUrl = baseUrl + "student/myinfo.aspx";
        }
        
        StringBuilder sb = new StringBuilder();
        sb.Append("{");
        sb.Append("\"code\":1,");
        sb.Append("\"msg\":\"登录成功\",");
        sb.Append("\"data\":{");
        sb.Append("\"sid\":" + Qsid + ",");
        sb.Append("\"snum\":\"" + snum + "\",");
        sb.Append("\"sname\":\"" + Qname + "\",");
        sb.Append("\"grade\":" + Qgrade + ",");
        sb.Append("\"class\":" + Qclass + ",");
        sb.Append("\"redirect\":\"" + redirectUrl + "\"");
        sb.Append("}}");
        
        return sb.ToString();
    }
    
    private string CheckTeacherControl(HttpContext context)
    {
        string ip = GetClientIP(context);
        string allowKey = "LabLoginAllow_" + ip;
        
        if (HttpContext.Current.Application[allowKey] != null)
        {
            bool allow = (bool)HttpContext.Current.Application[allowKey];
            if (allow)
            {
                HttpContext.Current.Application.Remove(allowKey);
                return "{\"code\":1,\"allow\":true}";
            }
        }
        
        return "{\"code\":1,\"allow\":false}";
    }
    
    private int GetCurrentTeacherHid(HttpContext context)
    {
        if (context.Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
        {
            HttpCookie cookie = context.Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname];
            if (cookie["Hid"] != null)
            {
                return int.Parse(cookie["Hid"]);
            }
        }
        return 0;
    }
    
    private string GetClientIP(HttpContext context)
    {
        string ip = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (string.IsNullOrEmpty(ip))
        {
            ip = context.Request.ServerVariables["REMOTE_ADDR"];
        }
        return ip;
    }
    
    private string GetBaseUrl(HttpContext context)
    {
        string scheme = context.Request.Url.Scheme;
        string host = context.Request.Url.Host;
        int port = context.Request.Url.Port;
        
        string baseUrl = scheme + "://" + host;
        if (port != 80 && port != 443)
        {
            baseUrl += ":" + port;
        }
        baseUrl += context.Request.ApplicationPath;
        if (!baseUrl.EndsWith("/"))
        {
            baseUrl += "/";
        }
        return baseUrl;
    }
    
    public bool IsReusable
    {
        get { return false; }
    }
}
