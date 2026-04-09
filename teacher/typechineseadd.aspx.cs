using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_typechineseadd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!IsPostBack)
            Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "拼音词语添加页面";
    }
    protected void BtnAdd_Click(object sender, EventArgs e)
    {
        if (Ttitle.Text != "" && Tcontent.Text != "")
        {
            LearnSite.Model.Chinese cmodel = new LearnSite.Model.Chinese();
            cmodel.Ntitle = HttpUtility.HtmlEncode(Ttitle.Text.Trim());
            cmodel.Ncontent = HttpUtility.HtmlEncode(Tcontent.Text);
            LearnSite.BLL.Chinese bll = new LearnSite.BLL.Chinese();
            int Nid = bll.Add(cmodel);
            System.Threading.Thread.Sleep(200);
            string url = "~/teacher/typechineseshow.aspx?nid=" + Nid;
            Response.Redirect(url, false);
        }
        else
        {
            Labelmsg.Text = "请输入拼音词语标题及内容！";
        }
    }
    protected void Btnreturn_Click(object sender, EventArgs e)
    {
        string url = "~/teacher/typechinese.aspx";
        Response.Redirect(url, false);
    }
    protected void BtnNoSet_Click(object sender, EventArgs e)
    {
        string mystr = Tcontent.Text;
        string newstr = ClearHtml(mystr);
        Tcontent.Text = newstr;
        Labelmsg.Text = "有" + LearnSite.Common.WordProcess.CheckSpaces(Tcontent.Text) + "个空格无法清除！  当前文章长度：" + newstr.Length.ToString();
     }

        private string ClearHtml(string mystr)
    {
        string cstr = LearnSite.Common.WordProcess.DropHTML(mystr);//清除所有Html格式
        cstr = LearnSite.Common.WordProcess.NoSpaces(cstr); //采用Trim去空格        
        return cstr;
    }
}