<%@ WebHandler Language="C#" Class="LinkProxy" %>

using System;
using System.Web;
using System.Web.SessionState;

public class LinkProxy : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        
        string action = context.Request["action"] ?? "";
        
        switch (action)
        {
            case "getlink":
                GetEncryptedLink(context);
                break;
            case "redirect":
                RedirectWithToken(context);
                break;
            case "verify":
                VerifyToken(context);
                break;
            default:
                context.Response.Write("{\"code\":0,\"msg\":\"无效操作\"}");
                break;
        }
    }
    
    private void GetEncryptedLink(HttpContext context)
    {
        if (!LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            context.Response.Write("{\"code\":-1,\"msg\":\"请先登录\"}");
            return;
        }
        
        string originalUrl = context.Request["url"] ?? "";
        string fid = context.Request["fid"] ?? "";
        
        if (string.IsNullOrEmpty(originalUrl))
        {
            context.Response.Write("{\"code\":0,\"msg\":\"链接地址无效\"}");
            return;
        }
        
        string token = LearnSite.Common.LinkEncryption.EncryptUrl(originalUrl, context.Session.SessionID);
        
        string proxyUrl = "../student/GameProxy.aspx?token=" + HttpUtility.UrlEncode(token);
        if (!string.IsNullOrEmpty(fid))
        {
            proxyUrl += "&fid=" + fid;
        }
        
        context.Response.Write("{\"code\":1,\"url\":\"" + proxyUrl + "\"}");
    }
    
    private void RedirectWithToken(HttpContext context)
    {
        string token = context.Request["token"] ?? "";
        
        if (!LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            context.Response.Write("<script>alert('请先登录！');history.back();</script>");
            return;
        }
        
        if (string.IsNullOrEmpty(token))
        {
            context.Response.Write("<script>alert('无效访问！');history.back();</script>");
            return;
        }
        
        string originalUrl = LearnSite.Common.LinkEncryption.DecryptUrl(token, context.Session.SessionID);
        
        if (string.IsNullOrEmpty(originalUrl))
        {
            context.Response.Write("<script>alert('链接已过期或无效！');history.back();</script>");
            return;
        }
        
        context.Response.Redirect(originalUrl);
    }
    
    private void VerifyToken(HttpContext context)
    {
        string token = context.Request["token"] ?? "";
        
        if (!LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            context.Response.Write("{\"code\":-1,\"msg\":\"请先登录\"}");
            return;
        }
        
        if (string.IsNullOrEmpty(token))
        {
            context.Response.Write("{\"code\":0,\"msg\":\"无效token\"}");
            return;
        }
        
        string originalUrl = LearnSite.Common.LinkEncryption.DecryptUrl(token, context.Session.SessionID);
        
        if (string.IsNullOrEmpty(originalUrl))
        {
            context.Response.Write("{\"code\":0,\"msg\":\"链接已过期或无效\"}");
            return;
        }
        
        context.Response.Write("{\"code\":1,\"msg\":\"验证成功\"}");
    }
    
    public bool IsReusable
    {
        get { return false; }
    }
}
