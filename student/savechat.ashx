<%@ WebHandler Language="C#" Class="savechat" %>

using System;
using System.Web;

public class savechat : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.Charset = "utf-8";
        context.Response.ContentType = "text/plain; charset=utf-8";

        string dic = HttpContext.Current.Request.Form["dic"];
        string result = LearnSite.Common.chathistory.add(dic).ToString();

        context.Response.Write(result);//返回内存表记录数
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
