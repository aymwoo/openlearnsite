<%@ WebHandler Language="C#" Class="FingerHandler" %>

using System;
using System.Web;

public class FingerHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.Charset = "utf-8";
        context.Response.ContentType = "text/plain; charset=utf-8";
        string myelevel = context.Request.QueryString["MyElevel"].ToString();
        string eh = "";
        if (!string.IsNullOrEmpty(myelevel))
        {
            LearnSite.BLL.English bl = new LearnSite.BLL.English();
            eh = bl.GetLevelwords(Int32.Parse(myelevel));
        }
        context.Response.Write(eh);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}
