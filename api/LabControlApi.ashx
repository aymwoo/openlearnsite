<%@ WebHandler Language="C#" Class="LabControlApi" %>

using System;
using System.Web;
using System.Text;

public class LabControlApi : IHttpHandler
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
                case "allowclose":
                    result = AllowClose(context);
                    break;
                case "allowallclose":
                    result = AllowAllClose(context);
                    break;
                case "getstatus":
                    result = GetStatus(context);
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
    
    private string AllowClose(HttpContext context)
    {
        string ip = context.Request["ip"] ?? "";
        if (string.IsNullOrEmpty(ip))
        {
            return "{\"code\":0,\"msg\":\"IP地址不能为空\"}";
        }
        
        string allowKey = "LabLoginAllow_" + ip;
        HttpContext.Current.Application[allowKey] = true;
        
        return "{\"code\":1,\"msg\":\"已允许关闭\"}";
    }
    
    private string AllowAllClose(HttpContext context)
    {
        string grade = context.Request["grade"] ?? "";
        string classNum = context.Request["class"] ?? "";
        
        if (string.IsNullOrEmpty(grade) || string.IsNullOrEmpty(classNum))
        {
            return "{\"code\":0,\"msg\":\"参数错误\"}";
        }
        
        LearnSite.BLL.Computers cbll = new LearnSite.BLL.Computers();
        System.Data.DataTable dt = cbll.GetClassComputers(int.Parse(grade), int.Parse(classNum));
        
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (System.Data.DataRow row in dt.Rows)
            {
                string ip = row["Cip"].ToString();
                string allowKey = "LabLoginAllow_" + ip;
                HttpContext.Current.Application[allowKey] = true;
            }
            return "{\"code\":1,\"msg\":\"已允许" + dt.Rows.Count + "台电脑关闭\"}";
        }
        
        return "{\"code\":1,\"msg\":\"没有找到电脑\"}";
    }
    
    private string GetStatus(HttpContext context)
    {
        string ip = context.Request["ip"] ?? "";
        
        StringBuilder sb = new StringBuilder();
        sb.Append("{\"code\":1,\"data\":{");
        
        if (!string.IsNullOrEmpty(ip))
        {
            string allowKey = "LabLoginAllow_" + ip;
            bool allowClose = HttpContext.Current.Application[allowKey] != null;
            sb.Append("\"ip\":\"" + ip + "\",");
            sb.Append("\"allowClose\":" + allowClose.ToString().ToLower());
        }
        
        sb.Append("}}");
        return sb.ToString();
    }
    
    public bool IsReusable
    {
        get { return false; }
    }
}
