using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_mygraph : System.Web.UI.Page
{
    protected string codefile = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
        {
                if (!IsPostBack)
                {
                    ShowMission();
                }
            
        }
    }

    private void ShowMission()
    {
        if (Request.QueryString["url"] != null)
        {
            string url = Request.QueryString["url"].ToString();
            url = HttpUtility.UrlDecode(url);
            string xmlstr = LearnSite.Common.XmlHelp.xmlRead(url);
            codefile = HttpUtility.UrlEncode(xmlstr).Replace("+", "%20");
        }
    }
    /*******************
在WEB编程中，经常需要通过JS传递参数给C#后台代码，如果传递的参数包括中文，则需要在JS中通过encodeURIComponent编码，对应C#中的HttpUtility.UrlEncode编码。
1、由于JS中通过encodeURIComponent编码时，将中文或者"="、空格等特殊字符转换为大写，但是C#中HttpUtility.UrlEncode编码时，则会将这些字符转换为小写。例如 .NET中方法HttpUtility.UrlEncode会将‘=’编码成‘%3d’，而不是%3D。
2、HttpUtility.UrlEncode会把“空格”编码为“+”，实际上应该编码为“%20”，我们需要手动将“+”替换为“%20”。
3、鉴于以上两点，我们需要利用HttpUtility.UrlEncode，重新封装一个C#编码方法，与JS中encodeURIComponent编码对应起来。
    ************************/


    /// 字符串转base64
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private string Base64(string str)
    {
        byte[] b = System.Text.Encoding.Default.GetBytes(str);
        //转成 Base64 形式的 System.String  
        str = Convert.ToBase64String(b);
        return str;

    }
}