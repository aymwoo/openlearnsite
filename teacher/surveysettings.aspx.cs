using System;
using System.Web;

public partial class teacher_surveysettings : System.Web.UI.Page
{
    private int Cid = 0;
    private int Vid = 0;
    private int Lid = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.IsTeacherCookies();
        LoadIds();
        if (!IsPostBack)
        {
            BindLinks();
            BindSurvey();
        }
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        LoadIds();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1912();

        LearnSite.BLL.Survey bll = new LearnSite.BLL.Survey();
        LearnSite.Model.Survey model = bll.GetModel(Vid);
        if (model == null)
        {
            ShowMessage("未找到对应的 Survey 活动。", true);
            return;
        }

        model.Venableai = CheckBoxEnableAi.Checked;
        if (bll.Update(model))
        {
            ShowMessage(CheckBoxEnableAi.Checked ? "已启用当前 Survey 的 AI 评价。" : "已关闭当前 Survey 的 AI 评价，学生提交时将只生成规则评估摘要。", false);
        }
        else
        {
            ShowMessage("保存失败，请稍后重试。", true);
        }

        BindLinks();
        BindSurvey();
    }

    private void LoadIds()
    {
        if (Request.QueryString["cid"] != null)
            Int32.TryParse(Request.QueryString["cid"], out Cid);
        if (Request.QueryString["vid"] != null)
            Int32.TryParse(Request.QueryString["vid"], out Vid);
        if (Request.QueryString["lid"] != null)
            Int32.TryParse(Request.QueryString["lid"], out Lid);
    }

    private void BindLinks()
    {
        HyperLinkReturn.NavigateUrl = "courseshow.aspx?cid=" + Cid;
        HyperLinkPreview.NavigateUrl = "../lessons/presurvey.aspx?vid=" + Vid + "&cid=" + Cid;
        HyperLinkStudent.NavigateUrl = Lid > 0 ? "../student/myexam.aspx?lid=" + Lid : "../lessons/presurvey.aspx?vid=" + Vid + "&cid=" + Cid;
    }

    private void BindSurvey()
    {
        LearnSite.BLL.Survey bll = new LearnSite.BLL.Survey();
        LearnSite.Model.Survey model = bll.GetModel(Vid);
        if (model == null)
        {
            LiteralTitle.Text = "未找到 Survey 活动";
            LiteralType.Text = "未知";
            LiteralCid.Text = Cid.ToString();
            LiteralVid.Text = Vid.ToString();
            LiteralStatus.Text = "当前活动不存在";
            LiteralContent.Text = "请返回学案后重新进入。";
            CheckBoxEnableAi.Checked = false;
            BtnSave.Enabled = false;
            return;
        }

        LiteralTitle.Text = Server.HtmlEncode(model.Vtitle);
        LiteralType.Text = model.Vtype.HasValue && model.Vtype.Value > 0 ? "测验" : "调查";
        LiteralCid.Text = Cid.ToString();
        LiteralVid.Text = model.Vid.ToString();
        LiteralStatus.Text = model.Venableai ? "当前活动已启用 AI 评价" : "当前活动未启用 AI 评价";
        LiteralContent.Text = HttpUtility.HtmlDecode(model.Vcontent ?? string.Empty);
        CheckBoxEnableAi.Checked = model.Venableai;
    }

    private void ShowMessage(string text, bool isError)
    {
        LabelMessage.Visible = true;
        LabelMessage.Text = Server.HtmlEncode(text);
        LabelMessage.CssClass = isError ? "survey-settings-message survey-settings-message--error" : "survey-settings-message survey-settings-message--success";
    }
}
