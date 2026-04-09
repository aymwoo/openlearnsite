<%@ WebHandler Language="C#" Class="ResourceAccess" %>

using System;
using System.Web;

public class ResourceAccess : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        string token = context.Request["token"] ?? "";
        string fidStr = context.Request["fid"] ?? "";
        
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(fidStr))
        {
            context.Response.Write("参数错误");
            return;
        }
        
        if (!LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            context.Response.Write("请先登录");
            return;
        }
        
        int fid = 0;
        if (!int.TryParse(fidStr, out fid))
        {
            context.Response.Write("参数错误");
            return;
        }
        
        LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
        string tokenKey = "ResourceToken_" + fid + "_" + cook.Snum;
        string sessionToken = context.Session[tokenKey] as string;
        
        if (string.IsNullOrEmpty(sessionToken) || sessionToken != token)
        {
            context.Response.Write("验证失败，请重新访问");
            return;
        }
        
        DateTime? tokenTime = context.Session[tokenKey + "_Time"] as DateTime?;
        if (tokenTime == null || (DateTime.Now - tokenTime.Value).TotalMinutes > 5)
        {
            context.Session.Remove(tokenKey);
            context.Session.Remove(tokenKey + "_Time");
            context.Response.Write("链接已过期，请重新访问");
            return;
        }
        
        LearnSite.BLL.Soft st = new LearnSite.BLL.Soft();
        LearnSite.Model.Soft smodel = st.GetModel(fid);
        
        if (smodel == null)
        {
            context.Response.Write("资源不存在");
            return;
        }
        
        context.Session.Remove(tokenKey);
        context.Session.Remove(tokenKey + "_Time");
        
        string furl = smodel.Furl;
        if (string.IsNullOrEmpty(furl))
        {
            context.Response.Write("资源链接无效");
            return;
        }
        
        st.UpdateFhit(fid);
        
        if (furl.StartsWith("http://") || furl.StartsWith("https://"))
        {
            context.Response.Redirect(furl, false);
        }
        else
        {
            LearnSite.Common.FileDown.DownLoadOut(furl);
        }
    }
    
    public bool IsReusable
    {
        get { return false; }
    }
}
