using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_courseshow : System.Web.UI.Page
{
    private List<LearnSite.Common.AIActivityPlanComposedRuntimeBlockSummary> _composedSummaries;

    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!IsPostBack)
        {
            Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "学案显示页面";
            showcourse();
            showmenu();
            if (Request.QueryString["cold"] != null)
            {
                BtnEdit.Enabled = false;
                LinkBtnAdd.Enabled = false;
                LinkBtnAddTopic.Enabled = false;
                LinkBtnAddTxtForm.Enabled = false;
                HeroEditLink.Attributes["aria-disabled"] = "true";
                HeroEditLink.Attributes["class"] += " is-disabled";
                HeroEditLink.Attributes.Remove("href");
                ReadonlyNote.Attributes.Remove("hidden");
            }
        }
    }

    private void showcourse()
    {
        if (Request.QueryString["cid"] != null)
        {
            string Cid = Request.QueryString["cid"].ToString();
            LearnSite.Model.Courses model = new LearnSite.Model.Courses();
            LearnSite.BLL.Courses cs = new LearnSite.BLL.Courses();
            model = cs.GetModel(Int32.Parse(Cid));
            if (model != null)
            {
                LabelCtitle.Text = model.Ctitle;
                LabelBannerPreviewTitle.Text = model.Ctitle;
                LabelCdate.Text = model.Cdate.ToString();
                LabelCclass.Text = model.Cclass;
                LabelCobj.Text = model.Cobj.ToString();
                LabelCterm.Text = model.Cterm.ToString();
                LabelCks.Text = model.Cks.ToString();
                Ccontent.InnerHtml = HttpUtility.HtmlDecode(model.Ccontent);
                if (model.Cbanner != "")
                {
                    Imagebanner.ImageUrl = model.Cbanner;
                    HiddenBannerUrl.Value = ResolveUrl(model.Cbanner);
                    HeroSection.Attributes["class"] += " has-banner";
                    HeroSection.Style["background-image"] = "url('" + ResolveUrl(model.Cbanner) + "')";
                    BannerEmpty.Visible = false;
                }
                else
                {
                    HiddenBannerUrl.Value = "";
                    HeroSection.Attributes["class"] = HeroSection.Attributes["class"].Replace(" has-banner", "");
                    HeroSection.Style.Remove("background-image");
                    Imagebanner.Visible = false;
                    BannerEmpty.Visible = true;
                }
            }
        }
    }
    private void showmenu()
    {
        if (Request.QueryString["cid"] != null)
        {
            string Cid = Request.QueryString["cid"].ToString();
            HiddenCourseId.Value = Cid;
            int courseId = Int32.Parse(Cid);
            _composedSummaries = LoadComposedSummaries(courseId);
            BindComposedTeacherSummary();
            LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
            RptListMenu.DataSource = lbll.GetMenu(courseId);
            RptListMenu.DataBind();
        }
    }
    protected void LinkBtnAdd_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["cid"] != null)
        {
            string Mcid = Request.QueryString["cid"].ToString();
            string url = "~/teacher/missionadd.aspx?mcid=" + Mcid;
            Response.Redirect(url, false);
        }
    }
    protected void LinkBtnReturn_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["cold"] != null)
        {
            Response.Redirect("~/teacher/courseold.aspx", false);
        }
        else
        {
            Response.Redirect("~/teacher/course.aspx", false);
        }
    }
    protected void LinkBtnAddTopic_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["cid"] != null)
        {
            string Mcid = Request.QueryString["cid"].ToString();
            string url = "~/teacher/topicadd.aspx?mcid=" + Mcid;
            Response.Redirect(url, true);
        }
    }
    protected void LinkBtnAddTxtForm_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["cid"] != null)
        {
            string Vcid = Request.QueryString["cid"].ToString();
            string url = "~/teacher/txtformadd.aspx?mcid=" + Vcid;
            Response.Redirect(url, true);
        }
    }
    protected void LinkBtnProgram_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["cid"] != null)
        {
            string Vcid = Request.QueryString["cid"].ToString();
            string url = "~/teacher/programadd.aspx?mcid=" + Vcid;
            Response.Redirect(url, true);
        }
    }

    protected void BtnEdit_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["cid"] != null)
        {
            string Cid = Request.QueryString["cid"].ToString();
            string url = "~/teacher/courseedit.aspx?cid=" + Cid;
            Response.Redirect(url, true);
        }
    }
    protected void RptListMenu_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int Lid = Convert.ToInt32(e.CommandArgument);
        LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
        LearnSite.Model.ListMenu model = lbll.GetModel(Lid);
        if (model == null) { showmenu(); return; }
        int lxid = model.Lxid ?? 0;
        string ltype = model.Ltype.ToString();

        if (e.CommandName == "P")
        {
            lbll.UpdateLshow(Lid);
        }
        if (e.CommandName == "D")
        {
            switch (ltype)
            {
                case "1": case "5": case "6": case "8": case "10": case "11": case "12":
                case "13": case "14": case "15": case "16": case "17": case "18": case "19":
                case "20": case "21": case "22": case "23": case "24": case "25": case "26":
                case "27": case "28": case "29": case "30": case "31": case "32": case "33":
                case "34": case "35": case "36": case "37": case "38": case "39":
                    new LearnSite.BLL.Mission().DeleteMission(lxid);
                    lbll.Delete(Lid);
                    break;
                case "2":
                    new LearnSite.BLL.Survey().Delete(lxid);
                    lbll.Delete(Lid);
                    break;
                case "3":
                    new LearnSite.BLL.TopicDiscuss().Delete(lxid);
                    lbll.Delete(Lid);
                    break;
                case "4":
                    new LearnSite.BLL.TxtForm().Delete(lxid);
                    lbll.Delete(Lid);
                    break;
                case "9":
                    new LearnSite.BLL.Consoles().Delete(lxid);
                    lbll.Delete(Lid);
                    break;
            }
        }

        System.Threading.Thread.Sleep(200);
        showmenu();
    }
    protected void RptListMenu_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

        HyperLink hl = (HyperLink)e.Item.FindControl("HlLtitle");
        string lxid = ((Label)e.Item.FindControl("LabelLxid")).Text;
        string ltype = ((Label)e.Item.FindControl("LabelLtype")).Text;
        string lid = ((Label)e.Item.FindControl("LabelLid")).Text;
        string Cid = Request.QueryString["cid"].ToString();
        string Cold = Request.QueryString["cold"] != null ? "&cold=T" : "";
        LinkButton showButton = (LinkButton)e.Item.FindControl("LinkBtnShow");
        Image img = (Image)e.Item.FindControl("Image4");
        Label lbl = (Label)e.Item.FindControl("Label4");
        LearnSite.Common.AIActivityPlanComposedRuntimeBlockSummary composedSummary = null;
        int listMenuId;
        if (Int32.TryParse(lid, out listMenuId))
        {
            composedSummary = LearnSite.Common.AIActivityPlanComposedRuntimeHelper.FindSummaryByListMenuId(_composedSummaries, listMenuId);
        }
            switch (ltype)
            {
                case "1":
                    img.ImageUrl = "~/images/mission.png";
                    lbl.Text = "练习";
                    hl.NavigateUrl = "missionshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "2":
                    img.ImageUrl = "~/images/survey.png";
                    lbl.Text = "调查";
                    hl.NavigateUrl = "surveysettings.aspx?cid=" + Cid + "&vid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "6"://描述
                    img.ImageUrl = "~/images/description.png";
                    lbl.Text = "阅读";
                    hl.NavigateUrl = "missionshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "3":
                    img.ImageUrl = "~/images/topic.png";
                    lbl.Text = "讨论";
                    hl.NavigateUrl = "topicshow.aspx?tcid=" + Cid + "&tid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "4":
                    img.ImageUrl = "~/images/inquiry.png";
                    lbl.Text = "填表";
                    hl.NavigateUrl = "txtformshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "5"://编程 
                    img.ImageUrl = "~/images/program.png";                   
                    lbl.Text = "积木";
                    hl.NavigateUrl = "programshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "8"://编程  
                    img.ImageUrl = "~/images/python.png";                  
                    lbl.Text = "代码";
                    hl.NavigateUrl = "pythonshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "9"://测评 
                    img.ImageUrl = "~/images/console.png";                   
                    lbl.Text = "测评";
                    hl.NavigateUrl = "consoleshow.aspx?ncid=" + Cid + "&nid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "10"://流程图  
                    img.ImageUrl = "~/images/mxgraph.png";                  
                    lbl.Text = "流程";
                    hl.NavigateUrl = "graphshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "11"://像素画  
                    ApplyCustomActivityVisual(img, lbl, "11");
                    hl.NavigateUrl = "pixelshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "12"://网页
                    img.ImageUrl = "~/images/html.png";
                    lbl.Text = "网页";
                    hl.NavigateUrl = "htmlshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "13"://编程  
                    img.ImageUrl = "~/images/pythonblock.png";
                    lbl.Text = "拼图";
                    hl.NavigateUrl = "pythonshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "14"://python积木编程  
                    img.ImageUrl = "~/images/blockpy.png";
                    lbl.Text = "积木";
                    hl.NavigateUrl = "pythonshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "15"://思维导图  
                    img.ImageUrl = "~/images/kitymind.png";
                    lbl.Text = "脑图";
                    hl.NavigateUrl = "kitymindshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "16"://表格处理 
                    img.ImageUrl = "~/images/sheet.png";
                    lbl.Text = "表格";
                    hl.NavigateUrl = "excelshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "17"://二维码 
                    ApplyCustomActivityVisual(img, lbl, "17");
                    hl.NavigateUrl = "pixelshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "18"://在线文档 
                    ApplyCustomActivityVisual(img, lbl, "18");
                    hl.NavigateUrl = "pixelshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "19"://在线演示文稿
                    ApplyCustomActivityVisual(img, lbl, "19");
                    hl.NavigateUrl = "pixelshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "20"://在线海报设计
                    ApplyCustomActivityVisual(img, lbl, "20");
                    hl.NavigateUrl = "pixelshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "21"://风格迁移 图像分类
                    ApplyCustomActivityVisual(img, lbl, "21");
                    hl.NavigateUrl = "pixelshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "22"://图像分类
                    ApplyCustomActivityVisual(img, lbl, "22");
                    hl.NavigateUrl = "pixelshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "23"://人脸识别
                    ApplyCustomActivityVisual(img, lbl, "23");
                    hl.NavigateUrl = "pixelshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "24"://物联网mqtt
                    ApplyCustomActivityVisual(img, lbl, "24");
                    hl.NavigateUrl = "pixelshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "25"://手绘画布
                    ApplyCustomActivityVisual(img, lbl, "25");
                    hl.NavigateUrl = "pixelshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "26"://推箱子地图
                    ApplyCustomActivityVisual(img, lbl, "26");
                    hl.NavigateUrl = "pixelshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "27"://人工智能对话
                    ApplyCustomActivityVisual(img, lbl, "27");
                    hl.NavigateUrl = "pixelshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "28"://语音合成
                    ApplyCustomActivityVisual(img, lbl, "28");
                    hl.NavigateUrl = "pixelshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "29"://文字识别
                    ApplyCustomActivityVisual(img, lbl, "29");
                    hl.NavigateUrl = "pixelshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "30"://声音分析
                    ApplyCustomActivityVisual(img, lbl, "30");
                    hl.NavigateUrl = "pixelshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "31"://井字棋
                    ApplyCustomActivityVisual(img, lbl, "31");
                    hl.NavigateUrl = "pixelshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "32"://手写数字识别
                    ApplyCustomActivityVisual(img, lbl, "32");
                    hl.NavigateUrl = "pixelshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "33"://markdown写作
                    ApplyCustomActivityVisual(img, lbl, "33");
                    hl.NavigateUrl = "pixelshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "34"://iframe嵌入网页
                    ApplyCustomActivityVisual(img, lbl, "34");
                    hl.NavigateUrl = "pixelshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "35"://文生图
                    ApplyCustomActivityVisual(img, lbl, "35");
                    hl.NavigateUrl = "pixelshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "36"://素材库
                    ApplyCustomActivityVisual(img, lbl, "36");
                    hl.NavigateUrl = "pixelshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "37"://网站设计
                    ApplyCustomActivityVisual(img, lbl, "37");
                    hl.NavigateUrl = "pixelshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "38"://网页课件
                    img.ImageUrl = "~/images/ware.png";
                    lbl.Text = "网页";
                    hl.NavigateUrl = "wareshow.aspx?mcid=" + Cid + "&mid=" + lxid + "&lid=" + lid + Cold;
                    break;
                case "39"://课堂测验
                    img.ImageUrl = "~/images/wvote.png";
                    lbl.Text = "测验";
                    hl.NavigateUrl = "~/webform/exam.aspx?cid=" + Cid + "&eid=" + lxid + "&lid=" + lid + Cold;
                    break;
            }

        if (composedSummary != null)
        {
            hl.Text = "第" + composedSummary.Sort.ToString() + "环 " + hl.Text;
            hl.ToolTip = GetTeacherProgressText(composedSummary);
            lbl.Text = lbl.Text + " · " + GetTeacherStateShortText(composedSummary);
        }

        bool isPublished = false;
        Boolean.TryParse(showButton.Text, out isPublished);
        showButton.Text = isPublished ? "已发布" : "未发布";
        if (!isPublished)
        {
            showButton.CssClass += " is-off";
            // div行通过JS在客户端已有data-lid，is-hidden由aspx模板的class绑定处理
            // 在服务端找到父div并加class
            System.Web.UI.HtmlControls.HtmlGenericControl rowDiv =
                (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("MenuRow");
            if (rowDiv != null) rowDiv.Attributes["class"] += " is-hidden";
        }

        string strjs = "if(confirm('您确定要删除吗?'))return true;else return false; ";
        ((LinkButton)e.Item.FindControl("LinkBtnDel")).OnClientClick = strjs;
    }

    private List<LearnSite.Common.AIActivityPlanComposedRuntimeBlockSummary> LoadComposedSummaries(int cid)
    {
        LearnSite.BLL.Courses cbll = new LearnSite.BLL.Courses();
        LearnSite.Model.Courses courseModel = cbll.GetModel(cid);
        if (courseModel == null || !courseModel.Chid.HasValue)
        {
            return null;
        }

        LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
        LearnSite.BLL.MenuWorks kbll = new LearnSite.BLL.MenuWorks();
        LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
        return LearnSite.Common.AIActivityPlanComposedRuntimeHelper.LoadPublishedCourseSummaries(
            cid,
            courseModel.Chid.Value,
            lid => lbll.GetModel(lid),
            lid => ResolveAnyStudentMenuWork(kbll, lid),
            missionId => wbll.GetRecordCount("Wmid=" + missionId.ToString()) > 0);
    }

    private LearnSite.Model.MenuWorks ResolveAnyStudentMenuWork(LearnSite.BLL.MenuWorks kbll, int lid)
    {
        if (kbll == null || lid <= 0)
        {
            return null;
        }

        System.Data.DataSet ds = kbll.GetList("Klid=" + lid.ToString());
        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        {
            return null;
        }

        return kbll.DataTableToList(ds.Tables[0])[0];
    }

    private void BindComposedTeacherSummary()
    {
        if (_composedSummaries == null || _composedSummaries.Count == 0)
        {
            PanelComposedTeacherSummary.Visible = false;
            LiteralComposedTeacherSummary.Text = string.Empty;
            return;
        }

        int completed = 0;
        List<string> items = new List<string>();
        for (int i = 0; i < _composedSummaries.Count; i++)
        {
            LearnSite.Common.AIActivityPlanComposedRuntimeBlockSummary summary = _composedSummaries[i];
            if (summary == null)
            {
                continue;
            }

            if (summary.CompletionState == "completed")
            {
                completed++;
            }

            items.Add("<span style='display:inline-block;margin:0 8px 8px 0;padding:6px 10px;border-radius:999px;background:#eff6ff;border:1px solid #bfdbfe;color:#1d4ed8;font-size:12px;font-weight:600;'>第" + summary.Sort.ToString() + "环 " + HttpUtility.HtmlEncode(summary.Title ?? string.Empty) + " · " + HttpUtility.HtmlEncode(GetTeacherProgressText(summary)) + "</span>");
        }

        PanelComposedTeacherSummary.Visible = true;
        LiteralComposedTeacherSummary.Text = "<div style='margin-bottom:8px;color:#475569;'>当前已完成 <strong>" + completed.ToString() + "</strong> / <strong>" + _composedSummaries.Count.ToString() + "</strong> 个整课环节。</div>" + string.Join(string.Empty, items.ToArray());
    }

    private string GetTeacherProgressText(LearnSite.Common.AIActivityPlanComposedRuntimeBlockSummary summary)
    {
        if (summary == null)
        {
            return string.Empty;
        }

        switch (summary.CompletionState)
        {
            case "completed":
                return "已有学生完成";
            case "incomplete":
                return "已发布待完成";
            default:
                return summary.IsRuntimeReady ? "可进入待完成" : "发布信息未齐";
        }
    }

    private string GetTeacherStateShortText(LearnSite.Common.AIActivityPlanComposedRuntimeBlockSummary summary)
    {
        if (summary == null)
        {
            return string.Empty;
        }

        return summary.CompletionState == "completed" ? "已完成" : (summary.CompletionState == "incomplete" ? "待完成" : "待核对");
    }
    protected void ImageButton1_Click(object sender, EventArgs e)
    {
        showmenu();
    }
    protected void LinkBtnPython_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["cid"] != null)
        {
            string Vcid = Request.QueryString["cid"].ToString();
            string url = "~/teacher/pythonadd.aspx?mcid=" + Vcid;
            Response.Redirect(url, true);
        }

    }
    protected void LinkBtnConsole_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["cid"] != null)
        {
            string cid = Request.QueryString["cid"].ToString();
            string url = "~/teacher/consoleadd.aspx?cid=" + cid;
            Response.Redirect(url, true);
        }

    }
    protected void LinkBtnGraph_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["cid"] != null)
        {
            string Vcid = Request.QueryString["cid"].ToString();
            string url = "~/teacher/graphadd.aspx?mcid=" + Vcid;
            Response.Redirect(url, true);
        }

    }
    protected void LinkButtonPixel_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["cid"] != null)
        {
            string Vcid = Request.QueryString["cid"].ToString();
            string url = "~/teacher/pixeladd.aspx?mcid=" + Vcid;
            Response.Redirect(url, true);
        }

    }
    protected void LinkButtonHtml_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["cid"] != null)
        {
            string Vcid = Request.QueryString["cid"].ToString();
            string url = "~/teacher/htmladd.aspx?mcid=" + Vcid;
            Response.Redirect(url, true);
        }

    }
    protected void LinkButtonKm_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["cid"] != null)
        {
            string Vcid = Request.QueryString["cid"].ToString();
            string url = "~/teacher/kitymindadd.aspx?mcid=" + Vcid;
            Response.Redirect(url, true);
        }

    }
    protected void LinkButtonExcel_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["cid"] != null)
        {
            string Vcid = Request.QueryString["cid"].ToString();
            string url = "~/teacher/exceladd.aspx?mcid=" + Vcid;
            Response.Redirect(url, true);
        }

    }
    protected void LinkButtonware_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["cid"] != null)
        {
            string Vcid = Request.QueryString["cid"].ToString();
            string url = "~/teacher/wareadd.aspx?mcid=" + Vcid;
            Response.Redirect(url, true);
        }
    }
    protected void LinkBtnAddExam_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["cid"] != null)
        {
            string Vcid = Request.QueryString["cid"].ToString();
            string url = "~/webform/exam.aspx?cid=" + Vcid;
            Response.Redirect(url, true);
        }

    }

    protected void BtnApplySort_Click(object sender, EventArgs e)
    {
        int cid;
        if (Request.QueryString["cid"] != null && Int32.TryParse(Request.QueryString["cid"], out cid))
        {
            ApplyCustomSort(HiddenSortOrder.Value, cid);
        }
        showmenu();
    }

    [WebMethod]
    public static bool SaveSort(string cid, string order)
    {
        int courseId;
        if (!Int32.TryParse(cid, out courseId))
        {
            return false;
        }

        return ApplyCustomSort(order, courseId);
    }

    private static bool ApplyCustomSort(string orderValue, int cid)
    {
        if (string.IsNullOrEmpty(orderValue))
        {
            return false;
        }

        string[] orderItems = orderValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        if (orderItems.Length == 0)
        {
            return false;
        }

        LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
        int sortIndex = 1;
        bool updated = false;

        foreach (string item in orderItems)
        {
            int lid;
            if (!Int32.TryParse(item, out lid))
            {
                continue;
            }

            LearnSite.Model.ListMenu model = lbll.GetModel(lid);
            if (model == null || model.Lcid != cid)
            {
                continue;
            }

            model.Lsort = sortIndex;
            lbll.Update(model);
            sortIndex++;
            updated = true;
        }

        if (!updated)
        {
            return false;
        }

        lbll.Lsortsncy(cid);
        return true;
    }

    private void ApplyCustomActivityVisual(Image img, Label lbl, string category)
    {
        LearnSite.Common.CustomActivityMeta meta = LearnSite.Common.CustomActivityCatalog.GetMeta(category);
        img.ImageUrl = meta.IconUrl;
        lbl.Text = meta.DisplayName;
    }
}
