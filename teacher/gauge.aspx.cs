using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_gauge : System.Web.UI.Page
{
    LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!IsPostBack)
        {
            LearnSite.BLL.AIGaugeGenerator.EnsureDefaultGaugeSkill();
            Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "评价量规页面";
            ShowGauge();
            ShowGtype();
        }
    }

    private void ShowGauge()
    {
        if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
        {
            string hid = tcook.Hid.ToString();
            LearnSite.BLL.Gauge gbll = new LearnSite.BLL.Gauge();
            GVGauge.DataSource = gbll.GetTeacherList(hid);
            GVGauge.DataBind();
        }
    }
    private void ShowGtype()
    {
        DDLtype.DataSource = LearnSite.Common.TypeNameList.WorksType();
        DDLtype.DataBind();
    }
    protected void Btnadd_Click(object sender, EventArgs e)
    {
        string Gtitle = HttpUtility.HtmlEncode(TextBoxGtitle.Text.Trim());
        if (!string.IsNullOrEmpty(Gtitle))
        {
            string hid = tcook.Hid.ToString(); 
            string Gtype = DDLtype.SelectedValue;
            LearnSite.Model.Gauge gmodel = new LearnSite.Model.Gauge();
            gmodel.Gcount = 0;
            gmodel.Gdate = DateTime.Now;
            gmodel.Ghid = Int32.Parse(hid);
            gmodel.Gtitle = Gtitle;
            gmodel.Gtype = Gtype;
            LearnSite.BLL.Gauge gbll = new LearnSite.BLL.Gauge();
            int gaugeId = gbll.Add(gmodel);
            System.Threading.Thread.Sleep(200);
            TextBoxGtitle.Text = "";
            ShowGauge();
            if (gaugeId > 0)
            {
                LearnSite.BLL.AIGaugeGenerator generator = new LearnSite.BLL.AIGaugeGenerator();
                LearnSite.BLL.GaugeGenerationResult result = generator.Generate(Gtype, Gtitle);
                int savedCount = generator.SaveItems(gaugeId, result.Items);
                string msg = result.Message;
                if (savedCount > 0)
                    msg += " 已写入 " + savedCount + " 条评价项。";
                else
                    msg += " 评价项写入失败，请手动补充。";

                string itemPreview = string.Join("|", result.Items.Select(item => (item.Msort ?? 0).ToString() + "." + (item.Mitem ?? string.Empty) + "（" + (item.Mscore ?? 0).ToString() + "分）"));
                string redirectUrl = string.Format("~/teacher/gaugeitem.aspx?gid={0}&aimsg={1}&aiitems={2}&aifallback={3}",
                    gaugeId,
                    HttpUtility.UrlEncode(msg),
                    HttpUtility.UrlEncode(itemPreview),
                    result.UsedFallback ? "1" : "0");
                Response.Redirect(redirectUrl, false);
                Context.ApplicationInstance.CompleteRequest();
            }
            else
            {
                LearnSite.Common.WordProcess.Alert("量规创建失败，请稍后重试！", this.Page);
            }
        }
        else
        {
            LearnSite.Common.WordProcess.Alert("请输入自定义评价标准的标题！", this.Page);
        }
    }
    protected void GVGauge_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Del")
        {
            string Gid = e.CommandArgument.ToString();
            LearnSite.BLL.GaugeItem mbll = new LearnSite.BLL.GaugeItem();
            if (!mbll.ExistsMgid(Int32.Parse(Gid)))
            {
                LearnSite.BLL.Gauge gbll = new LearnSite.BLL.Gauge();
                gbll.Delete(Int32.Parse(Gid));
                System.Threading.Thread.Sleep(200);
                ShowGauge();
            }
            else
            {
                LearnSite.Common.WordProcess.Alert("请先删除该评价标准下的内容项！", this.Page);
            }
        }
    }
    protected void GVGauge_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex > -1)
        {
            e.Row.Cells[0].Text = Convert.ToString(e.Row.RowIndex + 1);
            string strjs = "if(confirm('您确定要删除该评价量规吗?'))return true;else return false; ";
            ((LinkButton)e.Row.FindControl("BtnEdit")).OnClientClick = strjs;
        }
    }
}
