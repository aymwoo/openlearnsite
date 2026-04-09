<%@ WebHandler Language="C#" Class="GameAccess" %>

using System;
using System.Web;
using System.Text;

public class GameAccess : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        string action = context.Request["action"] ?? "";
        
        switch (action)
        {
            case "verify":
                VerifyAndRedirect(context);
                break;
            case "check":
                CheckSession(context);
                break;
            default:
                context.Response.Write("无效请求");
                break;
        }
    }
    
    private void VerifyAndRedirect(HttpContext context)
    {
        if (!LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            context.Response.Write("<script>alert('请先登录！');history.back();</script>");
            return;
        }
        
        string gamePath = context.Request["path"] ?? "";
        if (string.IsNullOrEmpty(gamePath))
        {
            context.Response.Write("<script>alert('游戏路径错误！');history.back();</script>");
            return;
        }
        
        if (!gamePath.StartsWith("/") && !gamePath.StartsWith("~/"))
        {
            gamePath = "/" + gamePath;
        }
        
        string sessionKey = "GameAccess_" + gamePath.Replace("/", "_").Replace("~", "");
        context.Session[sessionKey] = true;
        context.Session[sessionKey + "_Time"] = DateTime.Now;
        context.Session[sessionKey + "_Sid"] = new LearnSite.Model.Cook().Sid;
        
        string token = Guid.NewGuid().ToString("N");
        context.Session["GameToken_" + token] = gamePath;
        context.Session["GameToken_" + token + "_Time"] = DateTime.Now;
        
        string baseUrl = context.Request.Url.GetLeftPart(UriPartial.Authority) + context.Request.ApplicationPath;
        if (!baseUrl.EndsWith("/"))
        {
            baseUrl += "/";
        }
        
        string fullUrl = baseUrl + gamePath.TrimStart('/', '~');
        if (fullUrl.Contains("?"))
        {
            fullUrl += "&token=" + token;
        }
        else
        {
            fullUrl += "?token=" + token;
        }
        
        context.Response.Redirect(fullUrl, false);
    }
    
    private void CheckSession(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        
        if (!LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            context.Response.Write("{\"code\":0,\"msg\":\"未登录\"}");
            return;
        }
        
        string token = context.Request["token"] ?? "";
        if (string.IsNullOrEmpty(token))
        {
            context.Response.Write("{\"code\":0,\"msg\":\"无效访问\"}");
            return;
        }
        
        string tokenKey = "GameToken_" + token;
        string gamePath = context.Session[tokenKey] as string;
        
        if (string.IsNullOrEmpty(gamePath))
        {
            context.Response.Write("{\"code\":0,\"msg\":\"链接已过期或无效\"}");
            return;
        }
        
        DateTime? tokenTime = context.Session[tokenKey + "_Time"] as DateTime?;
        if (tokenTime == null || (DateTime.Now - tokenTime.Value).TotalMinutes > 30)
        {
            context.Session.Remove(tokenKey);
            context.Session.Remove(tokenKey + "_Time");
            context.Response.Write("{\"code\":0,\"msg\":\"链接已过期\"}");
            return;
        }
        
        int sid = new LearnSite.Model.Cook().Sid;
        string sessionKey = "GameAccess_" + gamePath.Replace("/", "_").Replace("~", "");
        object sessionSid = context.Session[sessionKey + "_Sid"];
        
        if (sessionSid == null || (int)sessionSid != sid)
        {
            context.Response.Write("{\"code\":0,\"msg\":\"权限验证失败\"}");
            return;
        }
        
        context.Response.Write("{\"code\":1,\"msg\":\"验证通过\"}");
    }
    
    public bool IsReusable
    {
        get { return false; }
    }
}
