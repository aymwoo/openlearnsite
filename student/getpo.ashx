<%@ WebHandler Language="C#" Class="getpo" %>

using System;
using System.Web;

public class getpo : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.Charset = "utf-8";
        context.Response.ContentType = "text/plain; charset=utf-8";
        if (context.Request.QueryString["lang"] != null)
        {
            string lang = context.Request.QueryString["lang"].ToString();
            string pofile = "~/Statics/locale/" + lang + ".po";
            string popath = context.Server.MapPath(pofile);
            //获取文件的二进制数据。
            string datas = System.IO.File.ReadAllText(popath);
            //将二进制数据写入到输出流中。
            HttpContext.Current.Response.Write(datas);
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}
