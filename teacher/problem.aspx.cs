using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_problem : System.Web.UI.Page
{
    LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "添加试题";

        if (!IsPostBack)
        {
            showProblem();
        }
    }

    private void showProblem()
    {
        if (Request.QueryString["Pid"] != null && Request.QueryString["nid"] != null)
        {
            Btnadd.Text = "保存修改";
            string pid = Request.QueryString["Pid"].ToString();
            string nid = Request.QueryString["nid"].ToString();
            LearnSite.Model.Problems model = new LearnSite.Model.Problems();
            LearnSite.BLL.Problems bll = new LearnSite.BLL.Problems();
            model = bll.GetModel(Int32.Parse(pid));
            mcontent.Value = HttpUtility.HtmlDecode(model.Ptitle);
            code.Value = model.Pcode;
            print.Value = model.Pouput;
        }    
    }
    protected string myCid()
    {
        if (Request.QueryString["ncid"] != null)
        {
            return Request.QueryString["ncid"].ToString();
        }
        else
        {
            return "";
        }
    }
    protected void Btnadd_Click(object sender, EventArgs e)
    {
        string title = LearnSite.Common.MarkdownContentGuard.NormalizeCodeFences(mcontent.Value); 
        string mycode = code.Value;
        string ouput = print.Value;
        if (title.Length > 0 && mycode.Length > 0)
        {
            LearnSite.Model.Problems model = new LearnSite.Model.Problems();
            model.Pcode = mycode;
            model.Pdate = DateTime.Now;
            model.Phid = tcook.Hid;
            model.Pouput = ouput;//这里输出结果，是否要编码？
            model.Pscore = Int32.Parse(ddscore.SelectedValue);
            model.Ptitle = HttpUtility.HtmlEncode(title);
            LearnSite.BLL.Problems bll = new LearnSite.BLL.Problems();
            if (Request.QueryString["nid"] != null && Request.QueryString["ncid"] != null)
            {
                string nid = Request.QueryString["nid"].ToString();
                string cid = Request.QueryString["ncid"].ToString();
                model.Pnid = Int32.Parse(nid);

                if (Request.QueryString["Pid"] != null)
                {
                    string pid = Request.QueryString["Pid"].ToString();
                    model.Pid = Int32.Parse(pid);
                    bll.UpdateProblem(model);

                    string url = "~/teacher/consoleshow.aspx?nid=" + nid + "&ncid=" + cid;
                    if (Request.QueryString["lid"] != null)
                    {
                        url += "&lid=" + Request.QueryString["lid"].ToString();
                    }
                    Response.Redirect(url);
                }
                else
                {
                    int count = bll.Pcount(Int32.Parse(nid));
                    model.Pcid = Int32.Parse(cid);
                    model.Psort = count + 1;
                    int n = bll.Add(model);

                    string url = "~/teacher/consoleshow.aspx?nid=" + nid + "&ncid=" + cid;
                    if (Request.QueryString["lid"] != null)
                    {
                        url += "&lid=" + Request.QueryString["lid"].ToString();
                    }
                    Response.Redirect(url);
                }
            }


        }
    }

    protected void Btnreturn_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["nid"] != null && Request.QueryString["ncid"] != null)
        {
            string nid = Request.QueryString["nid"].ToString();
            string cid = Request.QueryString["ncid"].ToString();
            string url = "~/teacher/consoleshow.aspx?nid=" + nid + "&ncid=" + cid;
            if (Request.QueryString["lid"] != null)
            {
                url += "&lid=" + Request.QueryString["lid"].ToString();
            }
            Response.Redirect(url);
        }
    }
}
