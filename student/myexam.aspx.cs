using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class student_myexam : System.Web.UI.Page
{
    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
    public string Fpage = "#";
    public string questionList = "";
    public bool isClose = false;
    public string Lidstr = "";
    public string Cidstr = "";
    public string Vidstr = "";
    public string Vtypestr = "";
    public bool isDone = false;
    public bool EnableAiAssessment = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            showSurvey();
        }

    }

    private void showSurvey()
    {
        if (Request.QueryString["lid"] != null)
        {
            string Lid = Request.QueryString["lid"].ToString();
            LabelLid.Text = Lid;
            if (LearnSite.Common.WordProcess.IsNum(Lid))
            {
                LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
                LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
                lmodel = lbll.GetModel(Int32.Parse(Lid));
                int Cid = lmodel.Lcid.Value;
                int Vid = lmodel.Lxid.Value;
                LabelCid.Text = Cid.ToString();
                LabelVid.Text = Vid.ToString();
                Fpage = "program.aspx?lid=" + Lid + "&mill=" + DateTime.Now.Millisecond;

                Lidstr = Lid;
                Cidstr = Cid.ToString();
                Vidstr = Vid.ToString();

                string ty = "12";
                string url = "mysurveyclass.aspx?vid=" + Vidstr + "&cid=" + Cidstr + "&type=" + ty;
                Hkscore.NavigateUrl = url;

                string fnum = cook.Snum;

                LearnSite.Model.Survey vmodel = new LearnSite.Model.Survey();
                LearnSite.BLL.Survey vbll = new LearnSite.BLL.Survey();
                vmodel = vbll.GetModel(Vid);
                Lbtitle.Text = vmodel.Vtitle;
                LabelVtotal.Text = vmodel.Vtotal.ToString();
                vcontent.InnerHtml = HttpUtility.HtmlDecode(vmodel.Vcontent);
                int vtype = vmodel.Vtype.Value;
                Vtypestr = vtype.ToString();
                EnableAiAssessment = vmodel.Venableai;
                Lbtype.Text = Vtypestr;
                if (vtype > 0)
                {
                    Lbtypecn.Text = "测验";
                }
                else
                {
                    Lbtypecn.Text = "调查";
                }
                Lbsname.Text = Server.UrlDecode(cook.Sname);
                Lbsnum.Text = fnum;
                isClose = vmodel.Vclose;
                if (!isClose)
                {
                    showQuestion();
                }


                int myscore = GetMyScore(Vid, fnum);
                if (myscore != -1024)
                {
                    isDone = true;
                    //如果已经回答过调查
                    Lbcheck.Text = "已完成";
                    Lbcheck.CssClass = "survey-check-tag survey-check-tag--done";
                    Lbfscore.Text = myscore.ToString();
                }
                else
                {
                    isDone = false;
                    Lbfscore.Text = "0";
                    Lbcheck.Text = "未完成";
                    Lbcheck.CssClass = "survey-check-tag survey-check-tag--pending";
                }


                if (fnum.IndexOf('s') > -1)
                {
                    Hkscore.Visible = true;
                }
            }
        }
    }
    private int GetMyScore(int vid, string fnum)
    {
        LearnSite.BLL.SurveyFeedback fbll = new LearnSite.BLL.SurveyFeedback();
        return fbll.ExistsScore(vid, fnum);
    }
    /// <summary>
    /// 获取试题数据
    /// </summary>
    private void showQuestion()
    {
        string Lid = LabelLid.Text;
        if (LearnSite.Common.WordProcess.IsNum(Lid))
        {
            LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
            LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
            lmodel = lbll.GetModel(Int32.Parse(Lid));
            int Cid = lmodel.Lcid.Value;
            int Vid = lmodel.Lxid.Value;

            LearnSite.BLL.SurveyQuestion qbll = new LearnSite.BLL.SurveyQuestion();

            questionList = qbll.GetListQuestion(Vid); ;
        }
    }

}
