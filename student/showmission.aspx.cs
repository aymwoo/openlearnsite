using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;

public partial class Student_showmission : System.Web.UI.Page
{
    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            if (Request.QueryString["lid"] != null)
            {
                LearnSite.Common.CookieHelp.KickStudent();
                if (!IsPostBack)
                {
                    ShowMission();
                }
            }
        }
        else
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }
    }

    private void ShowMission()
    {
        string Lid = Request.QueryString["lid"].ToString();

        if (LearnSite.Common.WordProcess.IsNum(Lid))
        {

            LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
            LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
            lmodel = lbll.GetModel(Int32.Parse(Lid));
            if (lmodel == null || !lmodel.Lxid.HasValue)
            {
                ShowMissionNotFound();
                return;
            }

            LearnSite.Model.Mission model = new LearnSite.Model.Mission();
            LearnSite.BLL.Mission mn = new LearnSite.BLL.Mission();

            model = mn.GetModel(lmodel.Lxid.Value);
            if (model != null)
            {
                int sSyear = cook.Syear;
                int sSclass = cook.Sclass;
                string sSnum = cook.Snum;
                string sLoginIp = cook.LoginIp;

                string sWcid = model.Mcid.ToString();
                string sWmsort = model.Msort.ToString();
                string sWfiletype = model.Mfiletype;
                LabelMtitle.Text = model.Mtitle;
                LabelMcid.Text = model.Mcid.ToString();
                string decodedContent = HttpUtility.HtmlDecode(model.Mcontent);
                HiddenMissionRaw.Value = decodedContent;
                Mcontent.InnerHtml = decodedContent;
                LearnSite.Common.ActivityPlanMissionGuideView guide = LearnSite.Common.AIActivityPlanMissionViewHelper.BuildActivityGuideView(model.Mcontent);
                PanelActivityGuide.Visible = guide != null;
                if (guide != null)
                {
                    LiteralActivityGuideGoal.Text = guide.GoalHtml;
                    LiteralActivityGuideInstructions.Text = guide.InstructionsHtml;
                    LiteralActivityGuideSteps.Text = guide.StepsHtml;
                }
                else
                {
                    LiteralActivityGuideGoal.Text = string.Empty;
                    LiteralActivityGuideInstructions.Text = string.Empty;
                    LiteralActivityGuideSteps.Text = string.Empty;
                }
                LabelSnum.Text = sSnum;
                LabelMfiletype.Text = sWfiletype;
                bool isupload = model.Mupload;
                CkMupload.Checked = isupload;
                if (isupload)
                    VoteLink.Visible = true;
                else
                    VoteLink.Visible = false;
                CkMgroup.Checked = model.Mgroup;
                LabelMsort.Text = sWmsort;
                LabelMid.Text = lmodel.Lxid.ToString();
                LabelLid.Text = Lid;
                upFileTypeGroup.ImageUrl = "~/images/filetype/" + LabelMfiletype.Text.ToLower() + ".gif";
                upFileUrlGroup.Text = "小组作品";
                switch (sWfiletype)
                {
                    case "doc":
                        LabelUploadType.Text = "*.doc;*.docx";
                        break;
                    case "ppt":
                        LabelUploadType.Text = "*.ppt;*.pptx";
                        break;
                    case "xls":
                        LabelUploadType.Text = "*.xls;*.xlsx";
                        break;
                    case "office":
                        LabelUploadType.Text = "*.doc;*.docx;*.ppt;*.pptx;*.xls;*.xlsx";
                        break;
                    case "sb":
                        LabelUploadType.Text = "*.sb;*.sb2";
                        break;
                    case "flash":
                        LabelUploadType.Text = "*.swf;*.fla";
                        break;
                    case "png":
                    case "jpg":
                        LabelUploadType.Text = "*.png;*.jpg;*.jpeg;*.gif";
                        break;
                    default:
                        LabelUploadType.Text = "*." + sWfiletype;
                        break;
                }
                if (model.Mgroup)
                {
                    LearnSite.BLL.GroupWork gbll = new LearnSite.BLL.GroupWork();
                    string gurl = gbll.DoneGroupWorkUrl(sSnum, lmodel.Lxid.Value);
                    upFileTypeGroup.Visible = true;
                    upFileUrlGroup.Visible = true;
                    upFileUrlGroup.NavigateUrl = "~/student/download.aspx?id=" + LearnSite.Common.EnDeCode.Encrypt(gurl, "ls");
                    LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
                    if (sbll.IsLeader(sSnum))//如果是组长则显示小组面板
                    {
                        Panelgroup.Visible = true;
                        showgroupWork();
                        if (gurl != "")
                        {
                            if (gbll.CheckGroupWork(sSnum, lmodel.Lxid.Value))
                            {
                                PanelGroupUp.Visible = false;
                                Labelgroupmsg.Text = "小组作品已评！<br/>不可再重新提交！";
                            }
                            else
                            {
                                Labelgroupmsg.Text = "小组作品未评！<br/>可修改后重新提交！";
                            }
                        }
                        else
                        {
                            upFileTypeGroup.Visible = false;
                            upFileUrlGroup.Visible = false;
                        }
                    }
                    else
                    {
                        Panelgroup.Visible = false;
                        upFileTypeGroup.Visible = false;
                    }
                }
                else
                {
                    Panelgroup.Visible = false;
                    upFileTypeGroup.Visible = false;
                }

                if (!CkMupload.Checked)
                {
                    Panelworks.Visible = false;
                }

                ImageType.ImageUrl = "~/images/filetype/" + LabelMfiletype.Text.ToLower() + ".gif";
                ShowIpWorkDone();
            }
            else
            {
                ShowMissionNotFound();
            }

        }
        else
        {
            ShowMissionNotFound();
        }

    }

    private void ShowMissionNotFound()
    {
        HiddenMissionRaw.Value = string.Empty;
        PanelActivityGuide.Visible = false;
        LiteralActivityGuideGoal.Text = string.Empty;
        LiteralActivityGuideInstructions.Text = string.Empty;
        LiteralActivityGuideSteps.Text = string.Empty;
        Mcontent.InnerHtml = "此学案活动不存在！";
        Panelworks.Visible = false;
        Panelgroup.Visible = false;
    }
    private bool isTeacher(string Wid, string Snum)
    {
        if (Wid != "" && Snum.StartsWith("s"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ShowIpWorkDone()
    {
        string Sname = cook.Sname;
        int Sgrade = cook.Sgrade;
        int Sclass = cook.Sclass;
        string Snum = cook.Snum;
        string Wip = cook.LoginIp;

        string Wcid = LabelMcid.Text;
        string Wmid = LabelMid.Text;

        // Guard: if labels were not populated (e.g. model was null), bail out early to avoid FormatException
        if (!LearnSite.Common.WordProcess.IsNum(Wcid) || !LearnSite.Common.WordProcess.IsNum(Wmid))
            return;

        LearnSite.BLL.Works ws = new LearnSite.BLL.Works();
        string Wid = ws.WorkDone(Snum, Int32.Parse(Wcid), Int32.Parse(Wmid));//返回空字符表示不存在该记录
        string SnumDone = ws.IpWorkDoneSnum(Sgrade, Sclass, Int32.Parse(Wcid), Int32.Parse(Wmid), Wip);
        string retureUrl = ws.WorkUrl(Snum, Int32.Parse(Wmid));
        VoteLink.NavigateUrl = "~/student/myevaluate.aspx?mid=" + Wmid + "&cid=" + Wcid;
        if (LearnSite.Common.XmlHelp.GetWorkIpLimit())//判断有无进行IP限制
        {
            if (Snum == SnumDone || isTeacher(Wid, Snum))
            {
                if (retureUrl != "")
                {
                    upFileUrl.Visible = true;
                    upFileType.Visible = true;
                    upFileType.ImageUrl = "~/images/filetype/" + LabelMfiletype.Text.ToLower() + ".gif";
                    upFileUrl.Text = Server.UrlDecode(Sname);
                    upFileUrl.NavigateUrl = "~/student/download.aspx?id=" + LearnSite.Common.EnDeCode.Encrypt(retureUrl, "ls");
                }
            }
        }
        else
        {
            if (retureUrl != "")
            {
                upFileUrl.Visible = true;
                upFileType.Visible = true;
                upFileType.ImageUrl = "~/images/filetype/" + LabelMfiletype.Text.ToLower() + ".gif";
                upFileUrl.Text = Server.UrlDecode(Sname);
                upFileUrl.NavigateUrl = "~/student/download.aspx?id=" + LearnSite.Common.EnDeCode.Encrypt(retureUrl, "ls");
            }
        }
        if (Wid != "")//判断有无作品提交
        {
            bool ischeck = ws.IsChecked(Int32.Parse(Wid));
            if (ischeck)//判断作品有无评价
            {
                Labelmsg.Text = "该作品已经评分!<br/>你不可以重新提交！";
                Panelswfupload.Visible = false;
            }
            else
            {
                if (LearnSite.Common.XmlHelp.GetWorkIpLimit())//判断有无进行IP限制
                {
                    if (Snum == SnumDone || isTeacher(Wid, Snum))
                    {
                        Labelmsg.Text = "已提交该活动作品！<br/>可修改后重新提交！";
                        Panelswfupload.Visible = true;
                    }
                    else
                    {
                        Panelswfupload.Visible = false;
                        if (LabelMfiletype.Text != "htm")
                            Labelmsg.Text = SnumDone + "学号<br/>已在该IP提交作品！";
                    }
                }
                else
                {
                    Labelmsg.Text = "已提交该活动作品！！<br/>可修改后重新提交！";
                    Panelswfupload.Visible = true;
                }
            }
        }
        else
        {
            LearnSite.BLL.Mission mbll = new LearnSite.BLL.Mission();
            string Wmsort = LabelMsort.Text;
            if (!LearnSite.Common.WordProcess.IsNum(Wmsort))
                return;
            int minMsort = mbll.GetLastMaxMsort(Int32.Parse(Wcid), Int32.Parse(Wmsort));//任务活动中查询
            bool isExitFirstWork = ws.ExistsMyFirstWork(Int32.Parse(Wcid), Snum, minMsort);

            if (LearnSite.Common.XmlHelp.GetWorkIpLimit())//判断有无进行IP限制
            {
                if (SnumDone == "")
                {
                    if (isExitFirstWork || minMsort == 0)//如果是上个任务已经提交或是第一个任务，则显示提交按钮
                    {
                        DateTime dt = DateTime.Now;
                        string today = dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day;
                        Labelmsg.Text = today;
                        Panelswfupload.Visible = true;
                    }
                    else
                    {
                        Labelmsg.Text = "请先提交前面作品！";
                    }
                }
                else
                {
                    Panelswfupload.Visible = false;
                    if (LabelMfiletype.Text != "htm")
                        Labelmsg.Text = SnumDone + "学号<br/>已在该IP提交作品！";
                }
            }
            else
            {
                if (isExitFirstWork || minMsort == 0)//如果是上个任务已经提交或是第一个任务，则显示提交按钮
                {
                    DateTime dt = DateTime.Now;
                    string today = dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day;
                    Labelmsg.Text = today;
                    Panelswfupload.Visible = true;
                }
                else
                {
                    Labelmsg.Text = "请先提交前面作品！";
                }
            }
        }
    }

    protected void GVgwork_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex > -1)
        {
            HyperLink hl = (HyperLink)e.Row.FindControl("HyperLinkWurl");
            hl.NavigateUrl = "~/student/download.aspx?id=" + LearnSite.Common.EnDeCode.Encrypt(hl.ToolTip, "ls");
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
    protected void GVgwork_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string Wid = e.CommandArgument.ToString();
        int Wlscore = 0;
        if (e.CommandName == "A")
        {
            Wlscore = 10;
            updatelscore(Wid, Wlscore);
        }
        if (e.CommandName == "P")
        {
            Wlscore = 6;
            updatelscore(Wid, Wlscore);
        }
        if (e.CommandName == "E")
        {
            Wlscore = 2;
            updatelscore(Wid, Wlscore);
        }
    }

    private void updatelscore(string Wid, int Wlscore)
    {
        LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
        wbll.Updatelscore(Int32.Parse(Wid), Wlscore);
        System.Threading.Thread.Sleep(200);
        showgroupWork();
    }
    private void showgroupWork()
    {
        if (LabelMid.Text != "")
        {
            int aaSid = cook.Sid;
            LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
            GVgwork.DataSource = wbll.GetGroupWorks(aaSid, Int32.Parse(LabelMid.Text));
            GVgwork.DataBind();
        }
    }
}
