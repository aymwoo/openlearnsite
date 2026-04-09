<%@ WebHandler Language="C#" Class="ResourceProxy" %>

using System;
using System.Web;
using System.Text;

public class ResourceProxy : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.Charset = "utf-8";
        
        string action = context.Request["action"] ?? "";
        
        try
        {
            if (action == "geturl")
            {
                GetResourceUrl(context);
            }
            else if (action == "check")
            {
                CheckAccess(context);
            }
            else
            {
                context.Response.Write("{\"code\":-1,\"msg\":\"未知操作\"}");
            }
        }
        catch (Exception ex)
        {
            context.Response.Write("{\"code\":-1,\"msg\":\"" + ex.Message.Replace("\"", "\\\"") + "\"}");
        }
    }
    
    private void GetResourceUrl(HttpContext context)
    {
        if (!LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            context.Response.Write("{\"code\":0,\"msg\":\"请先登录\"}");
            return;
        }
        
        string fidStr = context.Request["fid"] ?? "";
        if (string.IsNullOrEmpty(fidStr))
        {
            context.Response.Write("{\"code\":0,\"msg\":\"参数错误\"}");
            return;
        }
        
        int fid = 0;
        if (!int.TryParse(fidStr, out fid))
        {
            context.Response.Write("{\"code\":0,\"msg\":\"参数错误\"}");
            return;
        }
        
        LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
        LearnSite.BLL.Soft st = new LearnSite.BLL.Soft();
        LearnSite.Model.Soft smodel = st.GetModel(fid);
        
        if (smodel == null)
        {
            context.Response.Write("{\"code\":0,\"msg\":\"资源不存在\"}");
            return;
        }
        
        // 由于已经在 ScoreProxy.ashx 中检查了学分并扣除了学分，这里直接允许访问
        bool canAccess = true;
        
        string fclass = smodel.Fclass;
        if (fclass == "软件")
        {
            // 软件类型仍然需要检查下载时间限制
            canAccess = st.IsDownCan();
        }
        
        if (!canAccess)
        {
            context.Response.Write("{\"code\":0,\"msg\":\"权限不足\"}");
            return;
        }
        
        string token = GenerateToken(context, fid, cook.Snum);
        string proxyUrl = context.Request.Url.GetLeftPart(UriPartial.Authority) + context.Request.ApplicationPath;
        if (!proxyUrl.EndsWith("/"))
        {
            proxyUrl += "/";
        }
        proxyUrl += "api/ResourceAccess.ashx?token=" + token + "&fid=" + fid;
        
        context.Response.Write("{\"code\":1,\"url\":\"" + proxyUrl + "\"}");
    }
    
    private void CheckAccess(HttpContext context)
    {
        if (!LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            context.Response.Write("{\"code\":0,\"msg\":\"请先登录\"}");
            return;
        }
        
        string fidStr = context.Request["fid"] ?? "";
        if (string.IsNullOrEmpty(fidStr))
        {
            context.Response.Write("{\"code\":0,\"msg\":\"参数错误\"}");
            return;
        }
        
        int fid = int.Parse(fidStr);
        LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
        
        string tokenKey = "ResourceToken_" + fid + "_" + cook.Snum;
        string sessionToken = context.Session[tokenKey] as string;
        string requestToken = context.Request["token"] ?? "";
        
        if (!string.IsNullOrEmpty(sessionToken) && sessionToken == requestToken)
        {
            context.Response.Write("{\"code\":1,\"msg\":\"验证通过\"}");
        }
        else
        {
            context.Response.Write("{\"code\":0,\"msg\":\"验证失败\"}");
        }
    }
    
    private string GenerateToken(HttpContext context, int fid, string snum)
    {
        string token = Guid.NewGuid().ToString("N");
        string tokenKey = "ResourceToken_" + fid + "_" + snum;
        context.Session[tokenKey] = token;
        context.Session[tokenKey + "_Time"] = DateTime.Now;
        return token;
    }
    
    public bool IsReusable
    {
        get { return false; }
    }
}
