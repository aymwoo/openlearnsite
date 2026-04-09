<%@ WebHandler Language="C#" Class="saveform" %>

using System;
using System.Web;

public class saveform : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.Charset = "utf-8";
        context.Response.ContentType = "text/plain; charset=utf-8";
        string result = savemyform();
        context.Response.Write(result);
    }

    private string savemyform()
    {
        LearnSite.BLL.TxtFormBack tkbll = new LearnSite.BLL.TxtFormBack();
        return tkbll.SaveFormContent();
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}
