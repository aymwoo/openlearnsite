<%@ WebHandler Language="C#" Class="Savetype" %>

using System;
using System.Web;

public class Savetype : IHttpHandler {

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.Charset = "utf-8";
        context.Response.ContentType = "text/plain; charset=utf-8";
        string Ptid = context.Request.QueryString["Ptid"].ToString();
        string TypeScore = context.Request.Form["Ts"].ToString();

        LearnSite.BLL.Ptyper bll = new LearnSite.BLL.Ptyper();
        context.Response.Write(bll.Savemytype(Ptid, TypeScore));
    } 
        
    public bool IsReusable {
        get {
            return false;
        }
    }    

}
