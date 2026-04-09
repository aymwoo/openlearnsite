<%@ WebHandler Language="C#" Class="learnstatus" %>

using System;
using System.Web;

/// <summary>
/// 学生学习状态上报接口
/// 学生端通过AJAX POST上报当前学习状态
/// </summary>
public class learnstatus : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.Charset = "utf-8";
        context.Response.ContentType = "application/json; charset=utf-8";
        context.Response.AddHeader("Cache-Control", "no-cache, no-store");

        try
        {
            string action = context.Request.Form["action"] ?? context.Request.QueryString["action"] ?? "";

            switch (action)
            {
                case "update":
                    HandleUpdate(context);
                    break;
                case "remove":
                    HandleRemove(context);
                    break;
                case "heartbeat":
                    HandleHeartbeat(context);
                    break;
                default:
                    context.Response.Write("{\"ok\":false,\"msg\":\"unknown action\"}");
                    break;
            }
        }
        catch (Exception ex)
        {
            context.Response.Write("{\"ok\":false,\"msg\":\"" + ex.Message.Replace("\"", "'") + "\"}");
        }
    }

    private void HandleUpdate(HttpContext context)
    {
        string snum = context.Request.Form["snum"] ?? "";
        string sname = context.Request.Form["sname"] ?? "";
        string sgrade = context.Request.Form["sgrade"] ?? "0";
        string sclass = context.Request.Form["sclass"] ?? "0";
        string cid = context.Request.Form["cid"] ?? "0";
        string lid = context.Request.Form["lid"] ?? "0";
        string ltitle = context.Request.Form["ltitle"] ?? "";
        string ltype = context.Request.Form["ltype"] ?? "";
        string status = context.Request.Form["status"] ?? "viewing";
        string sid = context.Request.Form["sid"] ?? "0";

        if (string.IsNullOrEmpty(snum))
        {
            context.Response.Write("{\"ok\":false,\"msg\":\"missing snum\"}");
            return;
        }

        if (LearnSite.Common.App.Iskick(snum))
        {
            context.Response.Write("{\"ok\":true,\"kick\":true}");
            return;
        }

        LearnSite.Common.LearnStatus.UpdateStatus(
            snum,
            HttpUtility.UrlDecode(sname),
            int.Parse(sgrade),
            int.Parse(sclass),
            int.Parse(cid),
            int.Parse(lid),
            HttpUtility.UrlDecode(ltitle),
            ltype,
            status,
            int.Parse(sid)
        );

        context.Response.Write("{\"ok\":true,\"kick\":false}");
    }

    private void HandleRemove(HttpContext context)
    {
        string snum = context.Request.Form["snum"] ?? "";
        if (!string.IsNullOrEmpty(snum))
        {
            LearnSite.Common.LearnStatus.RemoveStatus(snum);
        }
        context.Response.Write("{\"ok\":true}");
    }

    private void HandleHeartbeat(HttpContext context)
    {
        // 心跳：只更新时间，不改变状态
        string snum = context.Request.Form["snum"] ?? "";
        string sname = context.Request.Form["sname"] ?? "";
        string sgrade = context.Request.Form["sgrade"] ?? "0";
        string sclass = context.Request.Form["sclass"] ?? "0";
        string cid = context.Request.Form["cid"] ?? "0";
        string lid = context.Request.Form["lid"] ?? "0";
        string ltitle = context.Request.Form["ltitle"] ?? "";
        string ltype = context.Request.Form["ltype"] ?? "";
        string status = context.Request.Form["status"] ?? "idle";
        string sid = context.Request.Form["sid"] ?? "0";

        if (!string.IsNullOrEmpty(snum))
        {
            if (LearnSite.Common.App.Iskick(snum))
            {
                context.Response.Write("{\"ok\":true,\"kick\":true}");
                return;
            }

            LearnSite.Common.LearnStatus.UpdateStatus(
                snum,
                HttpUtility.UrlDecode(sname),
                int.Parse(sgrade),
                int.Parse(sclass),
                int.Parse(cid),
                int.Parse(lid),
                HttpUtility.UrlDecode(ltitle),
                ltype,
                status,
                int.Parse(sid)
            );
        }

        context.Response.Write("{\"ok\":true,\"kick\":false}");
    }

    public bool IsReusable
    {
        get { return false; }
    }
}
