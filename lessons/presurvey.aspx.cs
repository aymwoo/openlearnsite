using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Lessons_presurvey : System.Web.UI.Page
{
    public bool EnableAiAssessment = false;
    public string ReturnUrl = "#";

    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.IsTeacherCookies();
        if (!IsPostBack)
        {
            showSurvey();
        }
    }

    private void showSurvey()
    {
        if (Request.QueryString["vid"] != null && Request.QueryString["cid"] != null)
        {
            string vid = Request.QueryString["vid"].ToString();
            string cid = Request.QueryString["cid"].ToString();
            ReturnUrl = "../teacher/courseshow.aspx?cid=" + cid;

            LearnSite.Model.Survey vmodel = new LearnSite.Model.Survey();
            LearnSite.BLL.Survey vbll = new LearnSite.BLL.Survey();
            vmodel = vbll.GetModel(Int32.Parse(vid));
            Lbtitle.Text = vmodel.Vtitle;
            vcontent.InnerHtml = HttpUtility.HtmlDecode(vmodel.Vcontent);
            EnableAiAssessment = vmodel.Venableai;
            int vtype = vmodel.Vtype.Value;
            Lbtype.Text = vtype.ToString();
            if (vtype > 0)
            {
                Lbtypecn.Text = "测验";
            }
            else
            {
                Lbtypecn.Text = "调查";
            }
            showQuestion();
            bool isClose = vmodel.Vclose;
            if (isClose)
            {
                Btnclock.ImageUrl = "~/images/clockred.gif" + "?temp=" + DateTime.Now.Millisecond.ToString();
                Btnclock.ToolTip = "调查已经关闭，请咨询老师！";
            }
            else
            {
                Btnclock.ImageUrl = "~/images/clock.gif" + "?temp=" + DateTime.Now.Millisecond.ToString();
                Btnclock.ToolTip = "调查开启，请开始回答！";
            }

            Btnok.Enabled = false;
            Btnshow.Enabled = false;
        }
    }

    private void showQuestion()
    {
        if (Request.QueryString["vid"] != null && Request.QueryString["cid"] != null)
        {
            string qvid = Request.QueryString["vid"].ToString();
            LearnSite.BLL.SurveyQuestion qbll = new LearnSite.BLL.SurveyQuestion();
            DataListonly.DataSource = qbll.GetListByQvid(Int32.Parse(qvid));
            DataListonly.DataBind();
        }
    }
    protected void DataListonly_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        string qid = DataListonly.DataKeys[e.Item.ItemIndex].ToString();
        if (!string.IsNullOrEmpty(qid))
        {
            RadioButtonList rbl = (RadioButtonList)e.Item.FindControl("RBLselect");
            LearnSite.BLL.SurveyItem mbll = new LearnSite.BLL.SurveyItem();
            rbl.DataSource = mbll.GetListItemDataTable(Int32.Parse(qid));
            rbl.DataTextField = "Mitem";
            rbl.DataValueField = "Mid";
            rbl.DataBind();
        }
    }
}
