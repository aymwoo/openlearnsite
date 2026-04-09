using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Data;

public partial class Student_showtask : System.Web.UI.Page
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
                    if (LearnSite.Common.WordProcess.IsNum(LabelMid.Text))
                        ShowIpWorkDone();
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
                string sWmid = lmodel.Lxid.ToString();
                string sWmsort = model.Msort.ToString();
                string sWfiletype = model.Mfiletype.ToLower();
                LabelMtitle.Text = model.Mtitle;
                LabelMcid.Text = model.Mcid.ToString();
                Mcontent.InnerHtml = HttpUtility.HtmlDecode(model.Mcontent);
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
                LabelMid.Text = sWmid;
                LabelLid.Text = Lid;
                switch (sWfiletype)
                {
                    case "doc":
                        LabelUploadType.Text = "doc,docx";
                        break;
                    case "ppt":
                        LabelUploadType.Text = "ppt,pptx";
                        break;
                    case "xls":
                        LabelUploadType.Text = "xls,xlsx";
                        break;
                    case "office":
                        LabelUploadType.Text = "doc,docx,ppt,pptx,xls,xlsx";
                        break;
                    case "sb":
                        LabelUploadType.Text = "sb,sb2,sb3";
                        break;
                    case "flash":
                        LabelUploadType.Text = "swf,fla";
                        break;
                    case "png":
                    case "jpg":
                        LabelUploadType.Text = "png,jpg,jpeg,gif";
                        break;
                    default:
                        LabelUploadType.Text = sWfiletype;
                        break;
                }

                if (!CkMupload.Checked)
                {
                    Panelworks.Visible = false;
                }

                ImageType.ImageUrl = "~/images/filetype/" + sWfiletype + ".gif";
                if (model.Microworld)
                {
                    LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
                    string wurl = wbll.ShowLastTypeWorks(sSnum, sWfiletype,sWcid);//获取该学生最近同类型作品
                    if (wurl != "")
                    {
                        oldUrl.Text = "上次作品";
                        oldUrl.NavigateUrl = "~/student/download.aspx?id=" + LearnSite.Common.EnDeCode.Encrypt(wurl, "ls");
                    }
                    else
                        oldUrl.Text = "";
                }
            }
            else
            {
                Mcontent.InnerHtml = "此学案活动不存在！";
                Panelworks.Visible = false;
            }
        }

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
                Labelmsg.Text = "已评分!<br/>你不可以重新提交！";
                Panelswfupload.Visible = false;
            }
            else
            {
                if (LearnSite.Common.XmlHelp.GetWorkIpLimit())//判断有无进行IP限制
                {
                    if (Snum == SnumDone || isTeacher(Wid, Snum))
                    {
                        Labelmsg.Text = "你可修改后再提交！";
                        Panelswfupload.Visible = true;
                    }
                    else
                    {
                        Panelswfupload.Visible = false;
                        if (LabelMfiletype.Text != "htm")
                            Labelmsg.Text = SnumDone + "学号<br/>已在该IP提交作品.！";
                    }
                }
                else
                {
                    Labelmsg.Text = "你可修改后再提交！";
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
                        //DateTime dt = DateTime.Now;
                        //string today = dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day;
                        //Labelmsg.Text = today;
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
                    //DateTime dt = DateTime.Now;
                    //string today = dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day;
                    //Labelmsg.Text = today;
                    Panelswfupload.Visible = true;
                }
                else
                {
                    Labelmsg.Text = "请先提交前面作品！";
                }
            }
        }

        if (upFileUrl.Visible)
        {
            oldUrl.Visible = false;
        }
        else
        {
            oldUrl.Visible = true;
        }
    }


}