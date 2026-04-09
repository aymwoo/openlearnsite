using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_downwork : System.Web.UI.Page
{
    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

    public string WorkSnum = "";
    public string MyFeedback = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            if (!IsPostBack)
            {
                ShowWork();
            }
        }
        else
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }
    }

    private void ShowWork()
    {

        string Wfiletype, Wurl;
        string Wid = Request.QueryString["wid"].ToString();
        if (LearnSite.Common.WordProcess.IsNum(Wid))
        {
            LearnSite.Model.Works wmodel = new LearnSite.Model.Works();
            LearnSite.BLL.Works ws = new LearnSite.BLL.Works();
            wmodel = ws.GetModel(Int32.Parse(Wid));
            string workfilename = wmodel.Wfilename;
            WorkSnum = wmodel.Wnum;
            Wurl = wmodel.Wurl;
            string Wcode = wmodel.Wcode;
            Wfiletype = wmodel.Wtype;
            int worklength = wmodel.Wlength.Value;
            decimal kblen = worklength / 1024;
            LbWscore.Text = wmodel.Wscore.ToString();
            LbWfscore.Text = wmodel.Wfscore.ToString();
            LabelWdate.Text = wmodel.Wdate.ToString();
            LbWself.Text = HttpUtility.HtmlDecode(wmodel.Wself);
            LbWdscore.Text = wmodel.Wdscore.ToString();
            bool Wflash = wmodel.Wflash.Value;
            DateTime workdate = wmodel.Wdate.Value;
            TimeSpan ts = DateTime.Now - workdate;
            int days = Int32.Parse(LearnSite.Common.XmlHelp.GetWorkDowntime());//获取作品查看天数

            int wgrade = wmodel.Wgrade.Value;
            int wterm = wmodel.Wterm.Value;
            string Wip = wmodel.Wip;
            ShowFeedback(Int32.Parse(Wid));
            LearnSite.BLL.Students stbll = new LearnSite.BLL.Students();
            string mySname = stbll.GetSnameBySnum(WorkSnum);
            if (mySname != "")
            {
                HLfile.Text = mySname + "." + Wfiletype;
            }
            else
            {
                HLfile.Text = "模拟学生" + WorkSnum + "." + Wfiletype;
            }
            Labelsize.Text = kblen.ToString("N2") + "kb";
            Labelgood.Text = "推荐" + LearnSite.Common.WordProcess.StrCountNew(MyFeedback, "T,").ToString() + "次";
            Labelwid.Text = Wid;
            Labeltype.Text = Wfiletype;
            Labelwurl.Text = Wurl;

            Literal1.Text = LearnSite.Common.ViewPage.SelectWritePlugin(Wid, Wfiletype, Wurl, Wcode, wmodel.Wthumbnail, true, false, WorkSnum);

            ImageType.ImageUrl = "~/images/filetype/" + Wfiletype.ToLower() + ".gif";
            if (ts.Days < days)
            {
                int waitdays = days - ts.Days;

                if (LearnSite.Common.XmlHelp.GetWorkIpLimit())///如果作品提交IP限制
                {
                    if (cook.Snum == WorkSnum && cook.LoginIp == Wip)
                        HLfile.Visible = true;
                    else
                    {
                        HLfile.Visible = false;
                        Labelmsg.Text = waitdays.ToString() + "天后可下载";
                    }
                }
                else
                {
                    if (cook.Snum == WorkSnum && cook.LoginIp == Wip)
                    {
                        HLfile.Visible = true;
                    }
                    else
                    {
                        HLfile.Visible = false;//否则 IP没限制或IP不同则限制几天后下载
                        Labelmsg.Text = waitdays.ToString() + "天后可下载";
                    }
                }
            }
            else
            {
                if (cook.Sgrade == wgrade && cook.ThisTerm == wterm)
                {
                    if (cook.Snum == WorkSnum && cook.LoginIp == Wip)
                    {
                        HLfile.Visible = true;//本学期的作品自己可见，别人不可下载
                    }
                    else
                    {
                        HLfile.Visible = false;//如果是本学期的作品，则无法下载，呵呵
                        Labelmsg.Text = "隐藏下载";
                        Labelmsg.ToolTip = "该作品在版权保护期内，暂时无法下载.";
                    }
                }
                else
                {
                    HLfile.Visible = true;//超过，都可下载
                }
            }

            HLfile.NavigateUrl = "~/student/download.aspx?id=" + LearnSite.Common.EnDeCode.Encrypt(Wurl, "ls");
            LearnSite.BLL.Mission mbll = new LearnSite.BLL.Mission();
            Labelmission.Text = mbll.GetMissionTitle(wmodel.Wmid.Value);
        }
    }
    private bool isOffice(string filetype)
    {
        bool isok = false;
        switch (filetype)
        {
            case "doc":
            case "docx":
            case "xls":
            case "xlsx":
            case "ppt":
            case "pptx":
            case "wps":
            case "wpp":
            case "et":
                isok = true;
                break;
        }
        return isok;
    }

    private void ShowFeedback(int Wid)
    {
        LearnSite.BLL.GaugeFeedback fbll = new LearnSite.BLL.GaugeFeedback();
        MyFeedback = fbll.GetWorkFeedback(Wid);
    }

}
