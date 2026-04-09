using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_workcheck : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!IsPostBack)
        {
            BtnWp.Attributes["OnClick"] = "return confirm('您确定要将本活动作品一键设置为未评吗？');";
            Btnreturn.Attributes.Add("onclick", "window.opener=null;window.open('','_self'); window.close()");
            if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
            {
                if (Request.QueryString["grade"] != null  && Request.QueryString["cid"] != null)
                {
                    ShowCid();
                    ShowWorks();
                }
            }
        }
    }

    private void ShowCid()
    {
        Labelshow.Text = Request.QueryString["grade"].ToString() + "年级";
        Labeltxt.Text = "班---作品展示";
        int Sgrade = Int32.Parse(Request.QueryString["grade"].ToString());
        string myCid = Request.QueryString["cid"].ToString();//直接url传递
        string cterm = LearnSite.Common.XmlHelp.GetTerm();

        LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
        DDLclass.DataSource = rm.GetLimitClass(Sgrade);
        DDLclass.DataTextField = "Rclass";
        DDLclass.DataValueField = "Rclass";
        DDLclass.DataBind();

        LearnSite.BLL.Courses cbll = new LearnSite.BLL.Courses();
        Labeltitle.Text = cbll.GetTitle(Int32.Parse(myCid));
        ShowUploadMsort();
    }
    private void ShowUploadMsort()
    {
        string dcid = Request.QueryString["cid"].ToString();//直接url传递
        if (dcid != "")
        {
            LearnSite.BLL.Mission bll = new LearnSite.BLL.Mission();
            DDLmid.DataSource = bll.GetUploadMidMtitle(Int32.Parse(dcid));
            DDLmid.DataTextField = "Mstitle";
            DDLmid.DataValueField = "Mid";
            DDLmid.DataBind();
        }
    }

    private void ShowDoneWorks()
    {
        int Sgrade = Int32.Parse(Request.QueryString["grade"].ToString());
        int Sclass = Int32.Parse(DDLclass.SelectedValue);
        int Wmid = Int32.Parse(DDLmid.SelectedValue);
        string mySort = RBsort.SelectedValue;
        LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
        DataTable dt = wbll.ShowClassWorksBySort(Sgrade, Sclass, Wmid, mySort);

        // 优先从 Signin 表获取当天签到的机号，没有签到记录则使用 Students 表中的 Sseat 字段
        if (dt != null && dt.Columns.Count > 0 && dt.Rows.Count > 0)
        {
            LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
            foreach (DataRow row in dt.Rows)
            {
                string snum = row["Wnum"].ToString();
                // 优先从 Signin 表获取当天签到的机号
                string todayMachine = sbll.GetTodayMachine(snum);
                if (!string.IsNullOrEmpty(todayMachine) && todayMachine != "-")
                {
                    // 使用当天签到的机号（可能已换座位）
                    row["Sseat"] = todayMachine;
    }
                // 如果没有签到记录，保持 Students 表中的 Sseat 字段不变
            }
        }

        DataListworks.DataSource = dt;
        DataListworks.DataBind();//Wid,Sname,Wurl,Wvote,Wscore,Qwork,Wcheck,Sseat
    }
    /// <summary>
    /// 
    /// </summary>
    private void ShowWorks()
    {
        if (Request.QueryString["grade"] != null )
        {
            int Sgrade = Int32.Parse(Request.QueryString["grade"].ToString());
            int Sclass = Int32.Parse(DDLclass.SelectedValue);
            LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
            int Syear = sbll.GetYear(Sgrade, Sclass);
            string myCid = Request.QueryString["cid"].ToString();//直接url传递
            if (myCid != "" )
            {
                if (DDLmid.SelectedValue != "" && DDLmid.Items.Count > 0)
                {
                    int Wmid = Int32.Parse(DDLmid.SelectedValue);
                    LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
                    ShowDoneWorks(); //独立出来方便刷新                   
                    Labelcounts.Text = DataListworks.Items.Count.ToString();

                    // 获取未提交作品的学生列表
                    DataSet dsNoworks = wbll.ShowTodayNotWorks(Syear, Sgrade, Sclass, Wmid);

                    // 优先从 Signin 表获取当天签到的机号，没有签到记录则使用 Students 表中的 Sseat 字段
                    if (dsNoworks != null && dsNoworks.Tables.Count > 0 && dsNoworks.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in dsNoworks.Tables[0].Rows)
                        {
                            string snum = row["Snum"].ToString();
                            // 优先从 Signin 表获取当天签到的机号
                            string todayMachine = sbll.GetTodayMachine(snum);
                            if (!string.IsNullOrEmpty(todayMachine) && todayMachine != "-")
                            {
                                // 使用当天签到的机号（可能已换座位）
                                row["Sseat"] = todayMachine;
                            }
                            // 如果没有签到记录，保持 Students 表中的 Sseat 字段不变
                        }
                    }

                    DataListNoworks.DataSource = dsNoworks;
                    DataListNoworks.DataBind();

                    Labelmsg.Text = CalculateScores();
                    LearnSite.BLL.Mission mbll = new LearnSite.BLL.Mission();
                    string Mfiletype = mbll.GetMfiletype(Wmid).ToLower();
                    showGroup();//显示小组作品
                    HLautoplay.Visible = false;
                    if (!string.IsNullOrEmpty(Mfiletype))
                    {
                        ImageType.Visible = true;
                        ImageType.ImageUrl = "~/images/filetype/" + Mfiletype + ".gif";
                        string urlstr = Sgrade.ToString() + "&sc=" + Sclass.ToString() + "&ci=" + myCid + "&mi=" + Wmid.ToString() + "&ty=" + Mfiletype;
                        HLautoplay.Visible = true;
                        HLautoplay.NavigateUrl = "~/teacher/circleshow.aspx?sg=" + urlstr;
                        HLautoplay.ImageUrl = "~/images/flashauto.png";
                        if (Mfiletype == "py" || Mfiletype == "ware")
                        {
                            BtnCheck.Visible = true;
                        }
                        else
                        {
                            BtnCheck.Visible = false;
                        }
                    }
                    else
                    {
                        HLautoplay.Visible = false;
                        HLgroupplay.Visible = false;
                    }
                }
            }
            else
            {
                Labelmsg.Text = "没找到发布的学案和活动！";
                ImageType.Visible = false;
            }
        }
    }
    protected void DataListworks_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        HyperLink hl = new HyperLink();
        hl = (HyperLink)e.Item.FindControl("HyperLink1");
        string Wurl = ((Label)e.Item.FindControl("Labelurl")).Text;
        hl.NavigateUrl = "~/student/download.aspx?id=" + LearnSite.Common.EnDeCode.Encrypt(Wurl , "ls");

        bool gflash = ((CheckBox)e.Item.FindControl("Checkwflash")).Checked;
        bool gerror = ((CheckBox)e.Item.FindControl("Checkwerror")).Checked;
        string gemotion = ((Label)e.Item.FindControl("Labelwlemotion")).Text;
        if (gemotion == "1")
        {
            hl.BackColor = System.Drawing.Color.DarkSeaGreen;
            hl.ForeColor = System.Drawing.Color.White;
        }
        HyperLink hlf = new HyperLink();
        hlf = (HyperLink)e.Item.FindControl("Hlflash");
        if (gflash)//有转换，显示
        {
            string furl = LearnSite.Common.WordProcess.SwfName(Wurl);
            hlf.NavigateUrl = "~/student/download.aspx?id=" + LearnSite.Common.EnDeCode.Encrypt(furl + "&True", "ls");
            hlf.Visible = true;
            if (gerror)
            {
                hlf.Enabled = false;
                hlf.ImageUrl = "~/images/flasherror.png";
                hlf.ToolTip = "文档转换异常！";
            }
        }
        else
        {
            hlf.Visible = false;
        }

        Label ls = new Label();
        ls = (Label)e.Item.FindControl("Labelscore");
        int Wscore = Int32.Parse(ls.Text);
        if (Wscore == 2 || Wscore == 1)
            ((LinkButton)e.Item.FindControl("LE")).BackColor = Labelscore.BackColor;
        if (Wscore == 4 || Wscore == 3)
            ((LinkButton)e.Item.FindControl("LD")).BackColor = Labelscore.BackColor;
        if (Wscore == 6 || Wscore == 5)
            ((LinkButton)e.Item.FindControl("LC")).BackColor = Labelscore.BackColor;
        if (Wscore == 8 || Wscore == 7)
            ((LinkButton)e.Item.FindControl("LB")).BackColor = Labelscore.BackColor;
        if (Wscore == 10 || Wscore == 9)
            ((LinkButton)e.Item.FindControl("LA")).BackColor = Labelscore.BackColor;
        if (Wscore == 12)
            ((LinkButton)e.Item.FindControl("LG")).BackColor = Labelscore.BackColor;
    }
    protected void DataListworks_ItemCommand(object source, DataListCommandEventArgs e)
    {
        int Wscore = 0;
        int Wid = Int32.Parse(DataListworks.DataKeys[e.Item.ItemIndex].ToString());
        LearnSite.BLL.Works ws = new LearnSite.BLL.Works();
        if (e.CommandName == "G")
        {
            Wscore = 12;
        }
        if (e.CommandName == "A")
        {
            Wscore = 10;
        }
        if (e.CommandName == "B")
        {
            Wscore = 8;
        }
        if (e.CommandName == "C")
        {
            Wscore = 6;
        }
        if (e.CommandName == "D")
        {
            Wscore = 4;
        }
        if (e.CommandName == "E")
        {
            Wscore = 2;
        }
        ws.ScoreWork(Wid, Wscore);
        System.Threading.Thread.Sleep(200);
        ShowDoneWorks();
    }
    protected void BtnA_Click(object sender, EventArgs e)
    {
        QuickSetScore("A");
        ShowDoneWorks();
    }
    protected void BtnB_Click(object sender, EventArgs e)
    {
        QuickSetScore("B");
        ShowDoneWorks();
    }
    private void QuickSetScore(string ape)
    {
        if (Request.QueryString["grade"] != null )
        {
            if (Request.QueryString["cid"] != "")
            {
                if (DDLmid.SelectedValue != "")
                {
                    int Sgrade = Int32.Parse(Request.QueryString["grade"].ToString());
                    int Sclass = Int32.Parse(DDLclass.SelectedValue);
                    int Wcid = Int32.Parse(Request.QueryString["cid"].ToString());//直接url传递);
                    int Wmid = Int32.Parse(DDLmid.SelectedValue);
                    LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
                    switch (ape)
                    {
                        case "A":
                            wbll.WorkSetScore(Wcid, Sgrade, Sclass, Wmid, 10);//将本班该活动未评作品全评为A
                            break;
                        case "B":
                            wbll.WorkSetScore(Wcid, Sgrade, Sclass, Wmid, 8);//将本班该活动未评作品全评为B
                            break;
                        case "K":
                            wbll.WorkSetScore(Wcid, Sgrade, Sclass, Wmid, 0);//将本班该活动未评作品全评为0
                            break;
                        case "Check":
                            wbll.WorkSetWcheckClass(Wmid, Sgrade, Sclass);//将该活动已得分标志全评
                            break;
                        case "W":
                            wbll.WorkSetNoneWcheck(Wcid, Sgrade, Sclass, Wmid);//将本班该活动为未评作品
                            break;   
                    }
                }
            }
        }
    }

    protected void DDLmid_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowWorks();
    }
    protected void CB_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = sender as CheckBox;
        //取得当前被选中项的索引  
        int index = (chk.NamingContainer as DataListItem).ItemIndex;
        //取得当前选中项中的某个值，最好找ID。
        Label lbl = this.DataListworks.Items[index].FindControl("Labelwid") as Label;
        LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
        wbll.CancleScoreWork(Int32.Parse(lbl.Text), chk.Checked);
        System.Threading.Thread.Sleep(200);
        ShowDoneWorks();
    }
    /// <summary>
    /// 等级分布统计，返回分布值
    /// </summary>
    private string CalculateScores()
    {
        int gc = 0;
        int ac = 0;
        int bc = 0;
        int cc = 0;
        int dc = 0;
        int ec = 0;
        int score = 0;
        foreach (DataListItem item in this.DataListworks.Items)
        {
            string thisscore = ((Label)item.FindControl("Labelscore")).Text;
            if (thisscore != "")
            {
                score = Int32.Parse(thisscore);
                switch (score)
                {
                    case 12:
                        gc++;
                        break;
                    case 10:
                    case 9:
                        ac++;
                        break;
                    case 8:
                    case 7:
                        bc++;
                        break;
                    case 6:
                    case 5:
                        cc++;
                        break;
                    case 4:
                    case 3:
                        dc++;
                        break;
                    case 2:
                    case 1:
                        ec++;
                        break;
                }
            }
        }
        string rstr = "等级分布：G " + gc.ToString() + " .  A " + ac.ToString() + " .  B " + bc.ToString() + " .  C " + cc.ToString() + " .  D " + dc.ToString() + " .  E " + ec.ToString();
        return rstr;
    }
    protected void DataListgroup_ItemCommand(object source, DataListCommandEventArgs e)
    {
        int Ggrade = Int32.Parse(Request.QueryString["grade"].ToString());
        int Cterm = Int32.Parse(LearnSite.Common.XmlHelp.GetTerm());
        int Gscore = 0;
        int Gid = Int32.Parse(DataListgroup.DataKeys[e.Item.ItemIndex].ToString());
        LearnSite.BLL.GroupWork gbll = new LearnSite.BLL.GroupWork();
        string cdme = e.CommandName;
        Gscore = Int32.Parse(cdme);
        gbll.UpdateGscore(Gid, Gscore, Ggrade, Cterm); //评分
        System.Threading.Thread.Sleep(500);
        showGroup();
    }
    protected void DataListgroup_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        HyperLink hl = new HyperLink();
        hl = (HyperLink)e.Item.FindControl("HyperLinkg1");
        string gurl = ((Label)e.Item.FindControl("Labelgurl")).Text;
        hl.NavigateUrl = "~/student/download.aspx?id=" + LearnSite.Common.EnDeCode.Encrypt(gurl , "ls");
        Label ls = new Label();
        ls = (Label)e.Item.FindControl("Labelgscore");
        int Gscore = Int32.Parse(ls.Text);
        if (Gscore > 2)
        {
            string cn = "L" + ls.Text;
            ((LinkButton)e.Item.FindControl(cn)).BackColor = Labelscore.BackColor;
        }
    }
    private void showGroup()
    {
        int Ggrade = Int32.Parse(Request.QueryString["grade"].ToString());
        int Gclass = Int32.Parse(DDLclass.SelectedValue);

        if (DDLmid.SelectedValue != "")
        {
            string myCid = Request.QueryString["cid"].ToString();//直接url传递
            int Gmid = Int32.Parse(DDLmid.SelectedValue);
            LearnSite.BLL.GroupWork gbll = new LearnSite.BLL.GroupWork();
            DataListgroup.DataSource = gbll.GetMissionGroup(Ggrade, Gclass, Gmid);
            DataListgroup.DataBind();
            if (DataListgroup.Items.Count > 0)
            {
                string urlstr = Ggrade.ToString() + "&sc=" + Gclass.ToString() + "&ci=" + myCid + "&mi=" + Gmid.ToString();
                HLgroupplay.Visible = true;
                HLgroupplay.NavigateUrl = "~/teacher/circlegroups.aspx?sg=" + urlstr;
                HLgroupplay.ImageUrl = "~/images/weboffice.png";
            }
            else
            {
                HLgroupplay.Visible = false;
            }
        }
    }
    protected void DDLCid_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowUploadMsort();
        ShowWorks();
    }
    protected void ImgBtnFlasherror_Click(object sender, EventArgs e)
    {
        int Sgrade = Int32.Parse(Request.QueryString["grade"].ToString());
        int Sclass = Int32.Parse(DDLclass.SelectedValue);
        string myCid = Request.QueryString["cid"].ToString();//直接url传递
        if (DDLmid.SelectedValue != "")
        {
            int Wmid = Int32.Parse(DDLmid.SelectedValue);
            LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
            wbll.ClearWflasherror(Sgrade, Sclass, Wmid, Int32.Parse(myCid));
        }
        System.Threading.Thread.Sleep(200);
        ShowDoneWorks();
    }
    protected void CBg_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chg = sender as CheckBox;
        //取得当前被选中项的索引  
        int index = (chg.NamingContainer as DataListItem).ItemIndex;
        //取得当前选中项中的某个值，最好找ID。
        Label lblg = this.DataListgroup.Items[index].FindControl("Labelgid") as Label;
        LearnSite.BLL.GroupWork gbll = new LearnSite.BLL.GroupWork();
        gbll.CancelGscore(Int32.Parse(lblg.Text), chg.Checked);
        System.Threading.Thread.Sleep(200);
        showGroup();
    }
    protected void Btnreflash_Click(object sender, EventArgs e)
    {
        ShowWorks();
    }
    protected void RBsort_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowDoneWorks();
    }
    protected void BtnCk_Click(object sender, EventArgs e)
    {
        QuickSetScore("K");
        ShowDoneWorks();
    }
    protected void DDLclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowDoneWorks();
        ShowWorks();
    }
    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        QuickSetScore("Check");
        ShowDoneWorks();
    }
    protected void BtnWp_Click(object sender, EventArgs e)
    {
        QuickSetScore("W");
        ShowDoneWorks();
    }
}
