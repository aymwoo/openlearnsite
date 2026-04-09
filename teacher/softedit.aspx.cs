using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

public partial class Teacher_softedit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
        {
            if (Request.QueryString["fid"] != null)
            {
                if (!IsPostBack)
                {
                    Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "资源编辑页面";
                    GetCategory();
                    ShowSoft();
                }
            }
            else
            {
                Response.Redirect("~/teacher/soft.aspx", false);
            }
        }
    }

    private void GetCategory()
    {
        LearnSite.BLL.SoftCategory ybll = new LearnSite.BLL.SoftCategory();
        ddlcategory.DataSource = ybll.GetListCategory();
        ddlcategory.DataTextField = "Ytitle";
        ddlcategory.DataValueField = "Yid";
        ddlcategory.DataBind();
    }

    private void ShowSoft()
    {
        string fid = Request.QueryString["fid"].ToString();
        LearnSite.Model.Soft model = new LearnSite.Model.Soft();
        LearnSite.BLL.Soft bll = new LearnSite.BLL.Soft();
        model = bll.GetModel(int.Parse(fid));
        DDLclass.SelectedValue = model.Fclass;
        mcontent.Value = HttpUtility.HtmlDecode(model.Fcontent);
        Texttitle.Text = model.Ftitle;
        Linkold.CommandArgument = model.Furl;
        Linkold.Text = model.Furl.Substring(model.Furl.LastIndexOf("/") + 1);
        LabelFhit.Text = model.Fhit.ToString();
        LabelFfiletype.Text = model.Ffiletype;
        CheckBoxFhide.Checked = model.Fhide;
        ddlcategory.SelectedValue = model.Fyid.ToString();
        
        int fopenValue = model.Fopen ?? 0;
        if (fopenValue >= 10000)
        {
            int requiredScore = fopenValue - 10000;
            DDLscoreType.SelectedValue = "comprehensive";
            TXTscore.Text = requiredScore.ToString();
        }
        else
        {
            DDLscoreType.SelectedValue = "original";
            DDLopen.SelectedValue = fopenValue.ToString();
        }
        
        if (model.Fhid > 0)
            CheckBoxFhid.Checked = false;
        else
            CheckBoxFhid.Checked = true;
    }

    private void EditSoft()
    {
        if (Texttitle.Text != "" && ddlcategory.SelectedValue != "")
        {
            LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
            string Fhid = tcook.Hid.ToString();
            LearnSite.Model.Soft soft = new LearnSite.Model.Soft();
            string fid = Request.QueryString["fid"].ToString();
            soft.Fid = Int32.Parse(fid);
            string fclass = DDLclass.SelectedValue;
            soft.Fclass = fclass;
            soft.Fup = false;
            if (fclass == "教程" || fclass == "微课" || fclass == "课程")
                soft.Fup = true;
            soft.Fcontent = HttpUtility.HtmlEncode(LearnSite.Common.MarkdownContentGuard.NormalizeCodeFences(mcontent.Value));
            soft.Fdate = DateTime.Now;
            soft.Fhit = Int32.Parse(LabelFhit.Text);
            soft.Ftitle = HttpUtility.HtmlEncode(Texttitle.Text.Trim());
            soft.Ffiletype = LabelFfiletype.Text;
            soft.Furl = Linkold.CommandArgument;
            soft.Fhide = CheckBoxFhide.Checked;
            
            string scoreType = DDLscoreType.SelectedValue;
            if (scoreType == "comprehensive")
            {
                int requiredScore = 60;
                if (!string.IsNullOrEmpty(TXTscore.Text))
                {
                    int.TryParse(TXTscore.Text, out requiredScore);
                    if (requiredScore < 0) requiredScore = 0;
                    if (requiredScore > 100) requiredScore = 100;
                }
                soft.Fopen = 10000 + requiredScore;
            }
            else
            {
                soft.Fopen = Int32.Parse(DDLopen.SelectedValue);
            }
            
            soft.Fyid = Int32.Parse(ddlcategory.SelectedValue);
            if (!CheckBoxFhid.Checked)
                soft.Fhid = Int32.Parse(Fhid);
            else
                soft.Fhid = -1;
            if (soft.Ftitle != "" && soft.Fcontent != "")
            {
                if (FUsoft.PostedFile.FileName != "")
                {
                    string uploadfile = FUsoft.PostedFile.FileName;
                    soft.Ffiletype = uploadfile.Substring(uploadfile.LastIndexOf(".") + 1);
                    soft.Furl = LearnSite.Common.Fileupload.Fupload(FUsoft);
                }
                LearnSite.BLL.Soft bll = new LearnSite.BLL.Soft();
                bll.Update(soft);

                Labelmsg.Text = "修改资源成功！";
                System.Threading.Thread.Sleep(500);
                string url = "~/teacher/softview.aspx?Fid=" + fid;
                Response.Redirect(url, false);
            }
            else
            {
                Labelmsg.Text = "请添加资源内容介绍！";
            }
        }
        else
        {
            Labelmsg.Text = "请添加资源标题！";
        }
    }

    protected void Btnreturn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/teacher/soft.aspx", false);
    }

    protected void Btnedit_Click(object sender, EventArgs e)
    {
        EditSoft();
    }

    protected void Linkold_Click(object sender, EventArgs e)
    {
        string furl = Linkold.CommandArgument;
        if (furl != "")
        {
            LearnSite.Common.FileDown.DownLoadOut(furl);
        }
    }
}
