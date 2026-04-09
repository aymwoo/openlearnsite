<%@ WebHandler Language="C#" Class="share" %>

using System;
using System.Web;

public class share : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {

        context.Response.ContentType = "text/plain; charset=utf-8";
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.Charset = "utf-8";
        string result = "";
        if (context.Request.QueryString["isweb"] != null)
        {
            string isweb = context.Request.QueryString["isweb"].ToString();
            result = LearnSite.Common.ShareDisk.SaveWebNew();
        }
        if (context.Request.QueryString["isgroup"] != null && context.Request.QueryString["iscommon"]!=null)
        {
            string isgroup = context.Request.QueryString["isgroup"].ToString();
            string iscommon = context.Request.QueryString["iscommon"].ToString();
            result = LearnSite.Common.ShareDisk.SaveFileNew(bool.Parse(isgroup), bool.Parse(iscommon));
        }
        context.Response.Write(result);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
