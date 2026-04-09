<%@ WebHandler Language="C#" Class="upchat" %>

using System;
using System.Web;

public class upchat : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.Charset = "utf-8";
        context.Response.ContentType = "application/json; charset=utf-8";
        string result = LearnSite.Common.chathistory.UpChatFile();
        context.Response.Write(result);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
