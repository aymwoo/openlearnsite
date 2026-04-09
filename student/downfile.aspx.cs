using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LearnSite.BLL;

public partial class Student_downfile : System.Web.UI.Page
{
    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            if (!IsPostBack)
            {

                ShowFile();
                ShowList();
                ShowTime();
                ShowUpload();
            }
        }
        else
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }
    }

    protected string strcut(string  str)
    {
        if (str.Length > 20)
            return LearnSite.Common.WordProcess.CnCutString(str,20,"...");
        else
            return str;
    }
    private void ShowUpload()
    {
        if (Request.QueryString["fid"] != null)
        {
            string ch = Labelclass.Text;
            switch (ch)
            {
                case "微课":
                case "教程":                    {
                        Panelswfupload.Visible = true;
                        string Fid = Request.QueryString["fid"].ToString();
                        LearnSite.BLL.Autonomic abll = new LearnSite.BLL.Autonomic();
                        LearnSite.Model.Autonomic amodel = new LearnSite.Model.Autonomic();
                        amodel = abll.GetModel(cook.Sid, Int32.Parse(Fid));
                        if (amodel != null)
                        {
                            if (amodel.Acheck)
                                Panelswfupload.Visible = false;
                            else
                                Panelswfupload.Visible = true;

                            upFileUrl.Visible = true;
                            upFileType.Visible = true;
                            upFileType.ImageUrl = "~/images/filetype/" + amodel.Atype.ToLower() + ".gif";
                            upFileUrl.Text = Server.UrlDecode(amodel.Afilename);
                            upFileUrl.NavigateUrl = "~/student/download.aspx?id=" + LearnSite.Common.EnDeCode.Encrypt(amodel.Aurl,"ls");
                        }
                        break;
                    }
                case "课程":
                    {
                        if (LBtnfile.Visible)
                        {
                            Panelswfupload.Visible = true;
                            string Fid = Request.QueryString["fid"].ToString();

                            LearnSite.BLL.Autonomic abll = new LearnSite.BLL.Autonomic();
                            LearnSite.Model.Autonomic amodel = new LearnSite.Model.Autonomic();
                            amodel = abll.GetModel(cook.Sid, Int32.Parse(Fid));
                            if (amodel != null)
                            {
                                if (amodel.Acheck)
                                    Panelswfupload.Visible = false;
                                else
                                    Panelswfupload.Visible = true;

                                upFileUrl.Visible = true;
                                upFileType.Visible = true;
                                upFileType.ImageUrl = "~/images/filetype/" + amodel.Atype.ToLower() + ".gif";
                                upFileUrl.Text = Server.UrlDecode(amodel.Afilename);
                                upFileUrl.NavigateUrl = "~/student/download.aspx?id=" + LearnSite.Common.EnDeCode.Encrypt(amodel.Aurl, "ls");
                            }
                        }
                        else
                        {
                            Panelswfupload.Visible = false;
                        }
                        break;
                    }
                default:
                    Panelswfupload.Visible = false;
                    break;
            }
        }
    }
    private void ShowTime()
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            string ch = Labelclass.Text;
            switch (ch)
            {
                case "微课":
                case "教程":
                case "资料":
                    {
                        LBtnfile.Visible = false;
                        break;
                    }
                case "软件":
                    {
                        LearnSite.BLL.Soft st = new LearnSite.BLL.Soft();
                        if (st.IsDownCan())
                        {
                            LBtnfile.Visible = false;
                            Labelmsg.Text = "";
                        }
                        else
                        {
                            Labelcontent.Text = "<br/><br/><br/><div style='text-align: center'>隐藏内容</div><br/><br/><br/>";
                            Labelmsg.Text = "登录" + LearnSite.Common.XmlHelp.GetDowntime().ToString() + "分钟后可下载";
                            LBtnfile.Visible = false;
                        }
                        break;
                    }
                case "游戏":
                case "课程":
                    {
                        // 使用保存的原始Fopen值
                        int requiredScore = ViewState["FopenValue"] != null ? Convert.ToInt32(ViewState["FopenValue"]) : 0;

                        // 判断评分方式
                        if (requiredScore >= 10000)
                        {
                            // 综合评分制
                            LearnSite.BLL.StudentScoreService scoreService = new LearnSite.BLL.StudentScoreService();
                            int comprehensiveScore = scoreService.GetComprehensiveScore(cook.Snum);
                            int actualRequiredScore = requiredScore - 10000;

                            if (comprehensiveScore >= actualRequiredScore)
                            {
                                LBtnfile.Visible = false;
                                HLurl.Visible = true;
                                Labelmsg.Text = "你的综合得分" + comprehensiveScore.ToString() + "分，已达到要求！";
                            }
                            else
                            {
                                Labelcontent.Text = "<br/><br/><br/><div style='text-align: center'>隐藏内容</div><br/><br/><br/>";
                                Labelmsg.Text = "你的综合得分" + comprehensiveScore.ToString() + "分，需要达到" + actualRequiredScore.ToString() + "分才能访问！";
                                LBtnfile.Visible = false;
                                HLurl.Visible = false;
                            }
                        }
                        else
                        {
                            // 原学分制 - 使用本节课学分
                            int currentScore = GetCurrentLessonScore(cook.Snum);
                            if (currentScore > requiredScore - 1)
                            {
                                LBtnfile.Visible = false;
                                HLurl.Visible = true;
                                Labelmsg.Text = "你本节课的学分" + currentScore.ToString() + "分，已达到要求！";
                            }
                            else
                            {
                                Labelcontent.Text = "<br/><br/><br/><div style='text-align: center'>隐藏内容</div><br/><br/><br/>";
                                Labelmsg.Text = "你本节课的学分" + currentScore.ToString() + "分，需要达到" + requiredScore.ToString() + "分才能访问！";
                                LBtnfile.Visible = false;
                                HLurl.Visible = false;
                            }
                        }
                        break;
                    }
            }
            if (HLurl.NavigateUrl == "")
            {
                LBtnfile.Visible = false;
            }
        }
    }

    /// <summary>
    /// 获取学生在当前课程中的学分
    /// </summary>
    /// <param name="snum">学生学号</param>
    /// <returns>当前课程学分</returns>
    private int GetCurrentLessonScore(string snum)
    {
        try
        {
            // 直接从数据库中查询学生的最新学分，避免使用缓存数据
            string sql = "SELECT Sscore FROM Students WHERE Snum = @Snum";
            System.Data.SqlClient.SqlParameter[] parameters = {
                new System.Data.SqlClient.SqlParameter("@Snum", snum)
            };
            object result = LearnSite.DBUtility.DbHelperSQL.GetSingle(sql, parameters);
            if (result != null && result != DBNull.Value)
            {
                return Convert.ToInt32(result);
            }
            return 0;
        }
        catch (Exception)
        {
            return 0;
        }
    }
    private void ShowFile()
    {
        if (Request.QueryString["fid"] != null)
        {
            string Fidstr = Request.QueryString["fid"].ToString();
            if (LearnSite.Common.WordProcess.IsNum(Fidstr))
            {
                LabelFid.Text = Fidstr;
                LabelSid.Text = cook.Sid.ToString();
                int Fid = Int32.Parse(Fidstr);
                LearnSite.Model.Soft smodel = new LearnSite.Model.Soft();
                LearnSite.BLL.Soft st = new LearnSite.BLL.Soft();
                smodel = st.GetModel(Fid);
                Labeltitle.Text = smodel.Ftitle;
                Labelhit.Text = smodel.Fhit.ToString();
                Labeldate.Text = smodel.Fdate.ToString();
                Labelfiletype.Text = smodel.Ffiletype;
                string typestr = Labelfiletype.Text;
                if (typestr == "")
                {
                    typestr = "read";
                    ImageDown.Visible = false;
                }
                ImageType.ImageUrl = "~/images/filetype/" + typestr.ToLower() + ".gif";
                Labelclass.Text = smodel.Fclass;
                HLurl.NavigateUrl = "javascript:void(0);";
                HLurl.Attributes["data-fid"] = Fidstr;
                HLurl.Attributes["onclick"] = "accessResource(" + Fidstr + "); return false;";
                HLurl.Text = "访问资源";

                ViewState["FopenValue"] = smodel.Fopen ?? 0;

                // 根据评分方式显示不同的内容
                int fopenValue = smodel.Fopen ?? 0;
                if (fopenValue >= 10000)
                {
                    // 综合评分制
                    Labelopen.Text = "综合" + (fopenValue - 10000).ToString() + "分";
                }
                else
                {
                    // 原学分制
                    string grade = "";
                    switch (fopenValue)
                    {
                        case 10: grade = "A"; break;
                        case 8: grade = "B"; break;
                        case 6: grade = "C"; break;
                        case 4: grade = "D"; break;
                        case 2: grade = "E"; break;
                        default: grade = fopenValue.ToString(); break;
                    }
                    Labelopen.Text = "学分" + grade;
                }

                string processedContent = LearnSite.Common.LinkEncryption.ProcessContentLinks(HttpUtility.HtmlDecode(smodel.Fcontent), Fidstr);
                Labelcontent.Text = processedContent;
                LabelFyid.Text= smodel.Fyid.ToString();
                LBtnfile.Visible = false;
                st.UpdateFhit(Fid);
            }
        }
    }
    private void ShowList()
    {

        LearnSite.BLL.Soft st = new LearnSite.BLL.Soft();
        string fyid = LabelFyid.Text;
        if (!string.IsNullOrEmpty(fyid))
        {
            int yid = Int32.Parse(fyid);
            GVSoft.DataSource = st.GetShowSoftList(cook.Rhid.ToString(), yid);
            GVSoft.DataBind();
            LearnSite.BLL.SoftCategory ybll = new LearnSite.BLL.SoftCategory();
            GVSoft.HeaderRow.Cells[0].Text = ybll.GetTitle(yid);
        }
    }
    protected void LBtnfile_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["fid"] != null && HLurl.NavigateUrl != "")
        {
            // 检查学生是否有权限下载该资源
            if (CanDownloadResource())
            {
                LearnSite.Common.FileDown.DownLoadOut(HLurl.NavigateUrl);
            }
            else
            {
                Labelmsg.Text = "你没有权限下载此资源！";
                LBtnfile.Visible = false;
            }
        }
    }

    /// <summary>
    /// 检查学生是否有权限下载资源
    /// </summary>
    /// <returns>是否有权限</returns>
    private bool CanDownloadResource()
    {
        string ch = Labelclass.Text;
        switch (ch)
        {
            case "微课":
            case "教程":
            case "资料":
                return true;
            case "软件":
                LearnSite.BLL.Soft st = new LearnSite.BLL.Soft();
                return st.IsDownCan();
            case "游戏":
            case "课程":
                // 使用保存的原始Fopen值
                int requiredScore = ViewState["FopenValue"] != null ? Convert.ToInt32(ViewState["FopenValue"]) : 0;

                // 判断评分方式
                if (requiredScore >= 10000)
                {
                    // 综合评分制
                    LearnSite.BLL.StudentScoreService scoreService = new LearnSite.BLL.StudentScoreService();
                    int comprehensiveScore = scoreService.GetComprehensiveScore(cook.Snum);
                    return comprehensiveScore >= (requiredScore - 10000);
                }
                else
                {
                    // 原学分制 - 使用本节课学分
                    int currentScore = GetCurrentLessonScore(cook.Snum);
                    return currentScore > requiredScore - 1;
                }
            default:
                return false;
        }
    }
    protected void GVSoft_RowDataBound(object sender, GridViewRowEventArgs e)
    {
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
    protected void GVSoft_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView theGrid = sender as GridView;  // refer to the GridView
        int newPageIndex = 0;

        if (-2 == e.NewPageIndex)
        { // when click the "GO" Button
            TextBox txtNewPageIndex = null;

            GridViewRow pagerRow = theGrid.BottomPagerRow;

            if (null != pagerRow)
            {
                txtNewPageIndex = pagerRow.FindControl("txtNewPageIndex") as TextBox;   // refer to the TextBox with the NewPageIndex value
            }

            if (null != txtNewPageIndex)
            {

                newPageIndex = int.Parse(txtNewPageIndex.Text) - 1; // get the NewPageIndex
            }
        }
        else
        {  // when click the first, last, previous and next Button
            newPageIndex = e.NewPageIndex;
        }

        // check to prevent form the NewPageIndex out of the range
        newPageIndex = newPageIndex < 0 ? 0 : newPageIndex;
        newPageIndex = newPageIndex >= theGrid.PageCount ? theGrid.PageCount - 1 : newPageIndex;
        theGrid.PageIndex = newPageIndex;
        ShowList();
    }
}
