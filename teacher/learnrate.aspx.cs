using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Drawing;

public partial class teacher_learnrate : System.Web.UI.Page
{
    protected string colorfast = "#D7F3CD";
    protected string colornormal = "#ACE798";
    protected string colorslow = "#8EDE75";
    private List<ActivityHeatStat> _activityStats = new List<ActivityHeatStat>();

    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!IsPostBack)
        {
            //Btnreturn.Attributes.Add("onclick", "window.opener=null;window.open('','_self'); window.close()");
            if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
            {
                if (Request.QueryString["wgrade"] != null && Request.QueryString["wclass"] != null && Request.QueryString["wcid"] != null)
                {
                    ShowCid();
                    showrate();
                }
            }
        }

    }
    private void ShowCid()
    {
        LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
        int Hid = tcook.Hid;
        LabelGradeClass.Text = Request.QueryString["wgrade"].ToString() + "年级" + Request.QueryString["wclass"].ToString() + "班";
        int Sgrade = Int32.Parse(Request.QueryString["wgrade"].ToString());
        int Sclass = Int32.Parse(Request.QueryString["wclass"].ToString());
        string myCid = Request.QueryString["wcid"].ToString();//直接url传递
        string cterm = LearnSite.Common.XmlHelp.GetTerm();

        LearnSite.BLL.Courses cbll = new LearnSite.BLL.Courses();
        DDLCid.DataSource = cbll.ShowCidCtitle(Hid, Sgrade, Int32.Parse(cterm));
        DDLCid.DataTextField = "Ctitle";
        DDLCid.DataValueField = "Cid";
        DDLCid.DataBind();
        if (myCid != "")
        {
            DDLCid.SelectedValue = myCid;//设置为自动获取的今天本班学案Cid
        }
    }
    private void showrate()
    {
        string cidSelect = DDLCid.SelectedValue;
        if (!string.IsNullOrEmpty(cidSelect))
        {
            int Sgrade = Int32.Parse(Request.QueryString["wgrade"].ToString());
            int Sclass = Int32.Parse(Request.QueryString["wclass"].ToString());
            int Cid = Int32.Parse(cidSelect);
            LearnSite.BLL.Courses cbll = new LearnSite.BLL.Courses();
            string Ctitle = cbll.GetTitle(Cid);
            DateTime dt1 = DateTime.Now;
            DataTable dt = cbll.CourseRate(Cid, Sgrade, Sclass);
            DateTime dt2 = DateTime.Now;
            _activityStats = BuildActivityStats(dt);
            GridViewclass.DataSource = dt.DefaultView.ToTable();
            GridViewclass.DataBind();

            dt.Dispose();//强制释放
            Labelmsg.Text = "最近刷新：" + dt2.ToString("HH:mm:ss") + "，耗时 " + LearnSite.Common.Computer.DatagoneMilliseconds(dt1, dt2) + " 毫秒";
            Btnreflash.ToolTip = "刷新";
            this.Page.Title = LabelGradeClass.Text;
        }
    }

    protected void GridViewclass_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            for (int i = 2; i < e.Row.Cells.Count; i++)
            {
                ActivityHeatStat stat = i - 2 < _activityStats.Count ? _activityStats[i - 2] : null;
                if (stat == null)
                    continue;

                Color headerColor = stat.HasData ? GetHeaderHeatColor(stat.Severity) : ColorTranslator.FromHtml("#e2e8f0");
                e.Row.Cells[i].BackColor = headerColor;
                e.Row.Cells[i].ForeColor = stat.Severity > 0.58 ? Color.White : ColorTranslator.FromHtml("#0f172a");
                e.Row.Cells[i].BorderColor = ColorTranslator.FromHtml("#FFFFFF");
                e.Row.Cells[i].ToolTip = stat.HasData
                    ? "平均停留：" + FormatSecondsAsMinuteSecond(stat.AvgSeconds) + "，完成 " + stat.CompletedCount.ToString() + "/" + stat.TotalCount.ToString() + " 人"
                    : "暂无学习记录";
            }
        }

        if (e.Row.RowIndex > -1)
        {
            int count = e.Row.Cells.Count;
            for (int i = 2; i < count; i++)
            {
                if (e.Row.Cells[i].Text != "-1")
                {                    
                    int sptime = Int32.Parse(e.Row.Cells[i].Text);
                    if (sptime > 0)
                    {
                        string color = colornormal;
                        if (sptime < 300)
                            color = colorfast;
                        if (sptime > 600)
                            color = colorslow;

                        e.Row.Cells[i].BackColor = System.Drawing.ColorTranslator.FromHtml(color);
                        e.Row.Cells[i].ToolTip = "停留时间：" + FormatSecondsAsMinuteSecond(sptime);
                    }
                    e.Row.Cells[i].BorderColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                }
                e.Row.Cells[i].Text = "";
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //当鼠标放上去的时候 先保存当前行的背景颜色 并给附一颜色 
            e.Row.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#E1E8E1',this.style.fontWeight='';");
            //当鼠标离开的时候 将背景颜色还原的以前的颜色 
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor,this.style.fontWeight='';");
            //单击行改变行背景颜色 
            e.Row.Attributes.Add("onclick", "this.style.backgroundColor='#D8E0D8'; this.style.color='buttontext';this.style.cursor='default';");
        }
    }
    protected void Btnreflash_Click(object sender, EventArgs e)
    {
        showrate();
    }

    protected void BtnreflashText_Click(object sender, EventArgs e)
    {
        showrate();
    }

    protected void DDLCid_SelectedIndexChanged(object sender, EventArgs e)
    {
        showrate();
    }

    private string FormatSecondsAsMinuteSecond(int seconds)
    {
        TimeSpan ts = TimeSpan.FromSeconds(seconds);
        int totalMinutes = (int)ts.TotalMinutes;
        return totalMinutes.ToString(CultureInfo.InvariantCulture) + "分" + ts.Seconds.ToString("00", CultureInfo.InvariantCulture) + "秒";
    }

    private string FormatSecondsAsMinuteSecond(double seconds)
    {
        TimeSpan ts = TimeSpan.FromSeconds(seconds);
        int totalMinutes = (int)ts.TotalMinutes;
        return totalMinutes.ToString(CultureInfo.InvariantCulture) + "分" + ts.Seconds.ToString("00", CultureInfo.InvariantCulture) + "秒";
    }

    private List<ActivityHeatStat> BuildActivityStats(DataTable dt)
    {
        List<ActivityHeatStat> stats = new List<ActivityHeatStat>();
        if (dt == null)
            return stats;

        double maxAvg = 0;
        for (int c = 2; c < dt.Columns.Count; c++)
        {
            int completed = 0;
            double totalSeconds = 0;
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                int spent;
                if (int.TryParse(dt.Rows[r][c].ToString(), out spent) && spent > 0)
                {
                    completed++;
                    totalSeconds += spent;
                }
            }

            ActivityHeatStat stat = new ActivityHeatStat();
            stat.TotalCount = dt.Rows.Count;
            stat.CompletedCount = completed;
            stat.HasData = completed > 0;
            stat.AvgSeconds = completed > 0 ? totalSeconds / completed : 0;
            if (stat.AvgSeconds > maxAvg)
                maxAvg = stat.AvgSeconds;
            stats.Add(stat);
        }

        if (maxAvg <= 0)
            return stats;

        for (int i = 0; i < stats.Count; i++)
        {
            if (stats[i].HasData)
            {
                double completionPenalty = 1d - ((double)stats[i].CompletedCount / Math.Max(1, stats[i].TotalCount));
                double severity = (stats[i].AvgSeconds / maxAvg) * 0.75d + completionPenalty * 0.25d;
                stats[i].Severity = Math.Max(0d, Math.Min(1d, severity));
            }
        }

        return stats;
    }

    private Color GetHeaderHeatColor(double severity)
    {
        if (severity <= 0.5d)
        {
            double t = severity / 0.5d;
            return BlendColor(ColorTranslator.FromHtml("#dcfce7"), ColorTranslator.FromHtml("#fde68a"), t);
        }

        return BlendColor(ColorTranslator.FromHtml("#fde68a"), ColorTranslator.FromHtml("#f87171"), (severity - 0.5d) / 0.5d);
    }

    private Color BlendColor(Color start, Color end, double factor)
    {
        int r = (int)Math.Round(start.R + (end.R - start.R) * factor);
        int g = (int)Math.Round(start.G + (end.G - start.G) * factor);
        int b = (int)Math.Round(start.B + (end.B - start.B) * factor);
        return Color.FromArgb(r, g, b);
    }

    private class ActivityHeatStat
    {
        public bool HasData;
        public int TotalCount;
        public int CompletedCount;
        public double AvgSeconds;
        public double Severity;
    }
}
