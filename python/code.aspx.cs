using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class python_code : System.Web.UI.Page
{
    protected string Id = "";
    protected string Mid = "";
    protected string Qid = "";
    protected string Snum = " ";
    protected string Fpage = "#";
    protected string Codefile = " ";
    protected string title = "";

    protected string argout = "";
    protected string argimg = "";

    protected string Midurl = "../images/none.gif ";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["mid"] != null && Request.QueryString["id"] != null)
        {
            if (!IsPostBack)
            {
                ShowMission();
            }
        }
    }

    private void ShowMission()
    {
        Mid = Request.QueryString["mid"].ToString();
        Qid = Request.QueryString["id"].ToString();
        Fpage = "question.aspx?mid=" +  Mid ;

        if (LearnSite.Common.WordProcess.IsNum(Qid))
        {
            LearnSite.Model.TurtleQuestion model = new LearnSite.Model.TurtleQuestion();
            LearnSite.BLL.TurtleQuestion bll = new LearnSite.BLL.TurtleQuestion();
            model = bll.GetModel(Int32.Parse(Qid));
            if (model != null)
            {
                title = model.Qtitle;
                Midurl = model.Qurl;
                argout = Base64(HttpUtility.UrlEncode(model.Qout).Replace("+", "%20"));
                argimg = model.Qimg;

                LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
                string Sname = cook.Sname;
                Snum = cook.Snum;

                LearnSite.BLL.TurtleAnswer abll = new LearnSite.BLL.TurtleAnswer();
                LearnSite.Model.TurtleAnswer amodel = new LearnSite.Model.TurtleAnswer();
                amodel = abll.GetModelByQidSid(Int32.Parse(Qid), cook.Sid);
                if (amodel != null)
                {
                    Id = amodel.Aid.ToString();
                    Codefile = amodel.Acode;//如果不为空，则获取原来的作品链接
                }

                this.Page.Title = HttpUtility.UrlDecode(Sname) + " " + Snum + " " + model.Qtitle;
            }
        }

    }

    /// <summary>
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