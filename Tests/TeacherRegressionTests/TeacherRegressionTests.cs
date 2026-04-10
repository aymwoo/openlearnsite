using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace TeacherRegressionTests;

public class TeacherRegressionTests
{
    private static readonly string RepoRoot = GetRepoRoot();
    private static readonly string TeacherRoot = Path.Combine(RepoRoot, "teacher");
    private static readonly string CommonRoot = Path.Combine(RepoRoot, "App_Code", "Common");

    private static readonly string[] ThreeEditorPages =
    {
        "courseedit.aspx",
        "missionadd.aspx",
        "missionedit.aspx",
        "softadd.aspx",
        "softedit.aspx",
        "exceladd.aspx",
        "exceledit.aspx",
        "pythonadd.aspx",
        "pythonedit.aspx",
        "programadd.aspx",
        "programedit.aspx",
        "kitymindadd.aspx",
        "kitymindedit.aspx",
        "graphadd.aspx",
        "graphedit.aspx",
        "htmladd.aspx",
        "htmledit.aspx",
        "txtformadd.aspx",
        "txtformedit.aspx",
        "pixeladd.aspx",
        "pixeledit.aspx",
        "topicadd.aspx",
        "topicedit.aspx",
        "qrcodeadd.aspx",
        "consoleadd.aspx",
    };

    private static readonly string[] ContentPagesReturningToCourse =
    {
        "txtformadd.aspx",
        "txtformedit.aspx",
        "graphadd.aspx",
        "graphedit.aspx",
        "htmladd.aspx",
        "htmledit.aspx",
        "pythonadd.aspx",
        "pythonedit.aspx",
        "programadd.aspx",
        "programedit.aspx",
        "pixeladd.aspx",
        "pixeledit.aspx",
        "kitymindadd.aspx",
        "kitymindedit.aspx",
        "exceladd.aspx",
        "exceledit.aspx",
        "missionadd.aspx",
        "missionedit.aspx",
        "topicadd.aspx",
        "qrcodeadd.aspx",
        "wareadd.aspx",
        "wareedit.aspx",
        "coursecreate.aspx",
    };

    private static readonly string[] ShowPagesWithEditAndReturn =
    {
        "wareshow.aspx",
        "missionshow.aspx",
        "kitymindshow.aspx",
        "programshow.aspx",
        "pythonshow.aspx",
        "pixelshow.aspx",
        "txtformshow.aspx",
        "excelshow.aspx",
        "htmlshow.aspx",
        "graphshow.aspx",
    };

    private static readonly string[] ShowPagesUsingPreviewCopy =
    {
        "missionshow.aspx",
        "wareshow.aspx",
        "programshow.aspx",
        "pythonshow.aspx",
        "kitymindshow.aspx",
        "pixelshow.aspx",
        "txtformshow.aspx",
        "excelshow.aspx",
        "htmlshow.aspx",
        "graphshow.aspx",
        "topicshow.aspx",
    };

    private static readonly string[] SeatPagesRequiringUtf8 =
    {
        "seatshow.aspx",
        "computer.aspx",
        "ip.aspx",
        "getip.aspx",
        "house.aspx",
    };

    private static readonly string[] TypingPagesUsingReturnList =
    {
        "typeadd.aspx",
        "typeedit.aspx",
        "typechineseadd.aspx",
        "typechineseedit.aspx",
        "typeshow.aspx",
        "typechineseshow.aspx",
        "typerset.aspx",
        "typechineseset.aspx",
    };

    [Fact]
    public void TeacherAspx_ShouldNotContainImageButtonControls_ExceptStartCourseIconEntry()
    {
        var exceptions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            Path.Combine(TeacherRoot, "start.aspx"),
        };

        foreach (var file in Directory.GetFiles(TeacherRoot, "*.aspx", SearchOption.TopDirectoryOnly))
        {
            var content = File.ReadAllText(file);
            if (exceptions.Contains(file))
            {
                Assert.DoesNotContain("<asp:ImageButton ID=\"PubSet\"", content, StringComparison.OrdinalIgnoreCase);
                continue;
            }

            Assert.DoesNotContain("<asp:ImageButton", content, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Fact]
    public void TeacherCodeBehind_ShouldNotContainImageClickEventArgs()
    {
        foreach (var file in Directory.GetFiles(TeacherRoot, "*.cs", SearchOption.TopDirectoryOnly))
        {
            var content = File.ReadAllText(file);
            Assert.DoesNotContain("ImageClickEventArgs", content, StringComparison.Ordinal);
        }
    }

    [Theory]
    [MemberData(nameof(GetThreeEditorPages))]
    public void ThreeEditorPages_ShouldKeepSwitchSyncAndSubmitHooks(string relativePath)
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, relativePath));
        Assert.Contains("switchEditor(", content, StringComparison.Ordinal);
        Assert.Contains("syncContent()", content, StringComparison.Ordinal);
        Assert.Contains("OnClientClick=\"return syncContent();\"", content, StringComparison.Ordinal);
    }

    [Fact]
    public void ProblemPage_ShouldKeepProblemSpecificEditorHooks()
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, "problem.aspx"));

        Assert.Contains("switchProblemEditor(", content, StringComparison.Ordinal);
        Assert.Contains("syncProblemContent()", content, StringComparison.Ordinal);
        Assert.Contains("OnClientClick=\"return syncProblemContent();\"", content, StringComparison.Ordinal);
        Assert.Contains("id=\"editorSelector\"", content, StringComparison.Ordinal);
    }

    [Fact]
    public void TeacherAspx_ShouldUseLoadEventListenersInsteadOfWindowOnload()
    {
        foreach (var file in Directory.GetFiles(TeacherRoot, "*.aspx", SearchOption.TopDirectoryOnly))
        {
            var content = File.ReadAllText(file);
            Assert.DoesNotContain("window.onload =", content, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void StartPage_ShouldKeepBulkMenuButtonsAndSwitcherOnly()
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, "start.aspx"));

        Assert.Contains("ID=\"BtnMenuOpenAll\"", content, StringComparison.Ordinal);
        Assert.Contains("ID=\"BtnMenuCloseAll\"", content, StringComparison.Ordinal);
        Assert.Contains("ID=\"BtnSwitchToggle\"", content, StringComparison.Ordinal);

        Assert.DoesNotContain("MenuStatus", content, StringComparison.Ordinal);
        Assert.DoesNotContain("lesson-menu-meta", content, StringComparison.Ordinal);
        Assert.DoesNotContain("lesson-menu-status", content, StringComparison.Ordinal);
        Assert.DoesNotContain("点击切换", content, StringComparison.Ordinal);
    }

    [Fact]
    public void StartCodeBehind_ShouldUseCurrentControlTypesForMenuBindings()
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, "start.aspx.cs"));

        Assert.Contains("imgbtn = (Button)e.Item.FindControl(\"PubSet\")", content, StringComparison.Ordinal);
        Assert.Contains("imgbtn = (LinkButton)e.Item.FindControl(\"imgBtn\")", content, StringComparison.Ordinal);
        Assert.Contains("BatchSetCurrentCourseMenuVisibility", content, StringComparison.Ordinal);
        Assert.DoesNotContain("FindControl(\"PubSet\")", content.Replace("imgbtn = (Button)e.Item.FindControl(\"PubSet\")", string.Empty), StringComparison.Ordinal);
    }

    [Fact]
    public void StartCodeBehind_ItemDataBoundHandlersShouldGuardNonDataItems()
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, "start.aspx.cs"));

        Assert.Contains("protected void DLdonekc_ItemDataBound", content, StringComparison.Ordinal);
        Assert.Contains("protected void DLnotline_ItemDataBound", content, StringComparison.Ordinal);
        Assert.Contains("protected void DLnewkc_ItemDataBound", content, StringComparison.Ordinal);
        Assert.Contains("protected void DataListMenu_ItemDataBound", content, StringComparison.Ordinal);

        var guard = "if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)";
        Assert.True(CountOccurrences(content, guard) >= 4, "Expected item-type guards on all critical ItemDataBound handlers.");
    }

    [Fact]
    public void CustomActivityCatalog_ShouldKeepUnifiedRoutingAndExampleHelpers()
    {
        var content = File.ReadAllText(Path.Combine(CommonRoot, "CustomActivityCatalog.cs"));

        Assert.Contains("public static string GetStudentEntryUrlByLid", content, StringComparison.Ordinal);
        Assert.Contains("public static string GetFileType", content, StringComparison.Ordinal);
        Assert.Contains("public static CustomActivityExampleResult BuildExampleValue", content, StringComparison.Ordinal);
        Assert.Contains("public static string GetExampleSummary", content, StringComparison.Ordinal);
        Assert.Contains("\"iframe\"", content, StringComparison.Ordinal);
        Assert.Contains("\"mqtt\"", content, StringComparison.Ordinal);
        Assert.Contains("\"~/student/iframe.aspx?lid={0}\"", content, StringComparison.Ordinal);
    }

    [Fact]
    public void IpythonTeacherPreview_ShouldPreserveTeacherReturnAndNoSaveBehavior()
    {
        var content = File.ReadAllText(Path.Combine(RepoRoot, "code", "ipython.js"));

        Assert.Contains("var isTeacherPreview = pathname.indexOf(\"/teacher/\") !== -1;", content, StringComparison.Ordinal);
        Assert.Contains("returnurl = ipurl + \"/consoleshow.aspx?nid=\" + hnid + \"&ncid=\" + hcid + \"&lid=\" + hlid;", content, StringComparison.Ordinal);
        Assert.Contains("if(isTeacherPreview)", content.Replace(" ", string.Empty), StringComparison.Ordinal);
        Assert.Contains("当前测评暂无试题，请先返回继续编辑。", content, StringComparison.Ordinal);
    }

    [Fact]
    public void ConsoleFlow_ShouldPreserveLidAcrossTeacherPages()
    {
        var consoleShow = File.ReadAllText(Path.Combine(TeacherRoot, "consoleshow.aspx.cs"));
        var consoleAdd = File.ReadAllText(Path.Combine(TeacherRoot, "consoleadd.aspx.cs"));
        var problemCodeBehind = File.ReadAllText(Path.Combine(TeacherRoot, "problem.aspx.cs"));

        Assert.Contains("if (Request.QueryString[\"lid\"] != null)", consoleShow, StringComparison.Ordinal);
        Assert.Contains("url += \"&lid=\" + Request.QueryString[\"lid\"].ToString();", consoleShow, StringComparison.Ordinal);
        Assert.Contains("url += \"&lid=\" + Request.QueryString[\"lid\"].ToString();", consoleAdd, StringComparison.Ordinal);
        Assert.Contains("url += \"&lid=\" + Request.QueryString[\"lid\"].ToString();", problemCodeBehind, StringComparison.Ordinal);
    }

    [Fact]
    public void ConsolePreviewPage_ShouldKeepTeacherPreviewHiddenFieldsAndReturnButton()
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, "consolepreview.aspx"));

        Assert.Contains("<asp:HiddenField ID=\"hidenjson\"", content, StringComparison.Ordinal);
        Assert.Contains("<asp:HiddenField ID=\"hidennid\"", content, StringComparison.Ordinal);
        Assert.Contains("<asp:HiddenField ID=\"hidencid\"", content, StringComparison.Ordinal);
        Assert.Contains("<asp:HiddenField ID=\"hidenlid\"", content, StringComparison.Ordinal);
        Assert.Contains("<a id=\"btnreturn\" href=\"#\" class=\"button\" >返回学案</a>", content, StringComparison.Ordinal);
        Assert.Contains("new ipythonExample(editor);", content, StringComparison.Ordinal);
    }

    [Fact]
    public void ConsolePreviewCodeBehind_ShouldRequireAndPopulateTeacherPreviewRouteValues()
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, "consolepreview.aspx.cs"));

        Assert.Contains("Request.QueryString[\"nid\"] != null && Request.QueryString[\"ncid\"] != null && Request.QueryString[\"lid\"] != null", content, StringComparison.Ordinal);
        Assert.Contains("hidenjson.Value=json;", content, StringComparison.Ordinal);
        Assert.Contains("hidennid.Value = nid;", content, StringComparison.Ordinal);
        Assert.Contains("hidencid.Value = cid;", content, StringComparison.Ordinal);
        Assert.Contains("hidenlid.Value = lid;", content, StringComparison.Ordinal);
    }

    [Fact]
    public void IpythonTeacherPreview_ShouldHideReturnInitiallyAndSkipTeacherSaveIdlePosts()
    {
        var content = File.ReadAllText(Path.Combine(RepoRoot, "code", "ipython.js"));

        Assert.Contains("$(btnreturn).attr('href', returnurl);", content, StringComparison.Ordinal);
        Assert.Contains("$(btnreturn).hide();", content, StringComparison.Ordinal);
        Assert.Contains("if(isTeacherPreview){", content.Replace(" ", string.Empty), StringComparison.Ordinal);
        Assert.Contains("console.log(\"Teacher\");", content, StringComparison.Ordinal);
        Assert.Contains("var saveurl = ipurl + \"/SaveIdle.ashx\";", content, StringComparison.Ordinal);
    }

    [Fact]
    public void ProblemPage_ShouldReuseKindEditorUploadBackendsAcrossAllEditors()
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, "problem.aspx"));

        Assert.Contains("var upjs= '../kindeditor/aspnet/upload_json.aspx?cid='+cid+'&ty='+ty;", content, StringComparison.Ordinal);
        Assert.Contains("var fmjs='../kindeditor/aspnet/file_manager_json.aspx?cid='+cid+'&ty='+ty;", content, StringComparison.Ordinal);
        Assert.Contains("uploadJson : upjs,", content, StringComparison.Ordinal);
        Assert.Contains("fileManagerJson : fmjs,", content, StringComparison.Ordinal);
        Assert.Contains("uploadImage: { server: upjs", content, StringComparison.Ordinal);
        Assert.Contains("uploadAttachment: { server: upjs", content, StringComparison.Ordinal);
        Assert.Contains("uploadFile: { server: upjs", content, StringComparison.Ordinal);
        Assert.Contains("LearnSiteEditorUploadHelper.handleVditorUpload(vditorObj, upjs, files);", content, StringComparison.Ordinal);
    }

    [Fact]
    public void ConsoleAddPage_ShouldReuseKindEditorUploadBackendsAcrossAllEditors()
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, "consoleadd.aspx"));

        Assert.Contains("var upjs = '../kindeditor/aspnet/upload_json.aspx?cid=' + cid + '&ty=' + ty;", content, StringComparison.Ordinal);
        Assert.Contains("var fmjs = '../kindeditor/aspnet/file_manager_json.aspx?cid=' + cid + '&ty=' + ty;", content, StringComparison.Ordinal);
        Assert.Contains("uploadJson: upjs,", content, StringComparison.Ordinal);
        Assert.Contains("fileManagerJson: fmjs,", content, StringComparison.Ordinal);
        Assert.Contains("uploadImage: { server: upjs", content, StringComparison.Ordinal);
        Assert.Contains("uploadAttachment: { server: upjs", content, StringComparison.Ordinal);
        Assert.Contains("uploadFile: { server: upjs", content, StringComparison.Ordinal);
        Assert.Contains("LearnSiteEditorUploadHelper.handleVditorUpload(vditorObj, upjs, files);", content, StringComparison.Ordinal);
    }

    [Fact]
    public void EditorUploadHelper_ShouldKeepKindEditorCompatibleUploadReuse()
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, "editor-upload-helper.js"));

        Assert.Contains("formData.append('imgFile', file);", content, StringComparison.Ordinal);
        Assert.Contains("JSON.parse(xhr.responseText || '{}')", content, StringComparison.Ordinal);
        Assert.Contains("editor.dangerouslyInsertHtml('<a href=\"' + res.url + '\" target=\"_blank\">' + name + '</a>');", content, StringComparison.Ordinal);
        Assert.Contains("uploadToKindEditor(upjs, files[0], function (res)", content, StringComparison.Ordinal);
        Assert.Contains("vditor.insertValue(text);", content, StringComparison.Ordinal);
    }

    [Fact]
    public void ConsoleAddCodeBehind_ShouldKeepLidOnEditAndReturnPaths()
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, "consoleadd.aspx.cs"));

        Assert.Contains("url = \"~/teacher/consoleshow.aspx?ncid=\" + Ncid + \"&nid=\" + nid;", content, StringComparison.Ordinal);
        Assert.Contains("url += \"&lid=\" + Request.QueryString[\"lid\"].ToString();", content, StringComparison.Ordinal);
        Assert.Contains("url = \"~/teacher/consoleshow.aspx?ncid=\" + Cid + \"&nid=\" + nid;", content, StringComparison.Ordinal);
    }

    [Fact]
    public void ProblemCodeBehind_ShouldKeepHtmlEncodingAndLidAwareReturnFlow()
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, "problem.aspx.cs"));

        Assert.Contains("mcontent.InnerText = HttpUtility.HtmlDecode(model.Ptitle);", content, StringComparison.Ordinal);
        Assert.Contains("model.Ptitle = HttpUtility.HtmlEncode(title);", content, StringComparison.Ordinal);
        Assert.Contains("string url = \"~/teacher/consoleshow.aspx?nid=\" + nid + \"&ncid=\" + cid;", content, StringComparison.Ordinal);
        Assert.Contains("url += \"&lid=\" + Request.QueryString[\"lid\"].ToString();", content, StringComparison.Ordinal);
    }

    [Fact]
    public void ConsoleShowPage_ShouldKeepPreviewActionsAndProblemGridControls()
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, "consoleshow.aspx"));

        Assert.Contains("Text=\"编辑内容\"", content, StringComparison.Ordinal);
        Assert.Contains("Text=\"测评状态\"", content, StringComparison.Ordinal);
        Assert.Contains("Text=\"添加试题\"", content, StringComparison.Ordinal);
        Assert.Contains("Text=\"返回学案\"", content, StringComparison.Ordinal);
        Assert.Contains("<asp:HyperLink ID=\"Hkconsole\"", content, StringComparison.Ordinal);
        Assert.Contains("预览效果", content, StringComparison.Ordinal);
        Assert.Contains("CommandName=\"Top\"", content, StringComparison.Ordinal);
        Assert.Contains("CommandName=\"Bottom\"", content, StringComparison.Ordinal);
        Assert.Contains("CommandName=\"Del\"", content, StringComparison.Ordinal);
        Assert.Contains("Text=\"编辑\"", content, StringComparison.Ordinal);
    }

    [Fact]
    public void ConsoleShowCodeBehind_ShouldKeepPreviewToggleAndProblemNavigationBehavior()
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, "consoleshow.aspx.cs"));

        Assert.Contains("Hkconsole.Visible = false;", content, StringComparison.Ordinal);
        Assert.Contains("string url = \"~/teacher/consolepreview.aspx?nid=\" + nid + \"&ncid=\" + cid + \"&lid=\" + lid;", content, StringComparison.Ordinal);
        Assert.Contains("Hkconsole.NavigateUrl = url;", content, StringComparison.Ordinal);
        Assert.Contains("Hkconsole.Visible = true;", content, StringComparison.Ordinal);
        Assert.Contains("Btnclock.Text = \"已暂停\";", content, StringComparison.Ordinal);
        Assert.Contains("Btnclock.Text = \"已启用\";", content, StringComparison.Ordinal);
        Assert.Contains("Btnclock.ToolTip = \"点击启用测评\";", content, StringComparison.Ordinal);
        Assert.Contains("Btnclock.ToolTip = \"点击暂停测评\";", content, StringComparison.Ordinal);
        Assert.Contains("vcontent.InnerHtml = HttpUtility.HtmlDecode(nmodel.Ncontent);", content, StringComparison.Ordinal);
        Assert.Contains("mbll.Psortnew(Int32.Parse(nid));", content, StringComparison.Ordinal);
        Assert.Contains("mbll.updatePsort(Int32.Parse(pid), false);", content, StringComparison.Ordinal);
        Assert.Contains("mbll.updatePsort(toplid, true);", content, StringComparison.Ordinal);
        Assert.Contains("mbll.updatePsort(bottomlid, false);", content, StringComparison.Ordinal);
        Assert.Contains("mbll.updatePsort(Int32.Parse(pid), true);", content, StringComparison.Ordinal);
        Assert.Contains("((LinkButton)e.Row.FindControl(\"BtnDel\")).OnClientClick = strjs;", content, StringComparison.Ordinal);
        Assert.Contains("((HyperLink)e.Row.FindControl(\"HyperLinkPid\")).NavigateUrl = url;", content, StringComparison.Ordinal);
    }

    [Fact]
    public void ConsoleShowCodeBehind_ShouldKeepColdModeEditingLockout()
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, "consoleshow.aspx.cs"));

        Assert.Contains("if (Request.QueryString[\"cold\"] != null)", content, StringComparison.Ordinal);
        Assert.Contains("BtnEdit.Enabled = false;", content, StringComparison.Ordinal);
        Assert.Contains("Btnadd.Enabled = false;", content, StringComparison.Ordinal);
    }

    [Fact]
    public void StartCodeBehind_ShouldKeepMenuTypeIconMappingsIncludingConsoleAndWare()
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, "start.aspx.cs"));

        Assert.Contains("case \"9\"://交互式python测评", content, StringComparison.Ordinal);
        Assert.Contains("imgurl = \"~/images/console.png\";", content, StringComparison.Ordinal);
        Assert.Contains("case \"38\"://网页课件", content, StringComparison.Ordinal);
        Assert.Contains("imgurl = \"~/images/ware.png\";", content, StringComparison.Ordinal);
        Assert.Contains("case \"25\"://手绘画布", content, StringComparison.Ordinal);
        Assert.Contains("imgurl = GetCustomActivityIconUrl(\"25\");", content, StringComparison.Ordinal);
        Assert.Contains("dt.Rows[i][\"Limgurl\"] = imgurl;", content, StringComparison.Ordinal);
        Assert.Contains("DataListMenu.DataSource = dt;", content, StringComparison.Ordinal);
        Assert.Contains("DataListMenu.DataBind();", content, StringComparison.Ordinal);
    }

    [Fact]
    public void StartCodeBehind_ShouldKeepMenuToggleTooltipsAndVisibilityRefresh()
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, "start.aspx.cs"));

        Assert.Contains("imgbtn.ToolTip = \"点击隐藏\";", content, StringComparison.Ordinal);
        Assert.Contains("imgbtn.ToolTip = \"点击发布\";", content, StringComparison.Ordinal);
        Assert.Contains("lt.ToolTip = \"已发布\";", content, StringComparison.Ordinal);
        Assert.Contains("lt.ToolTip = \"已隐藏\";", content, StringComparison.Ordinal);
        Assert.Contains("lbll.UpdateLshow(Lid);", content, StringComparison.Ordinal);
        Assert.Contains("showMenu();", content, StringComparison.Ordinal);
    }

    [Fact]
    public void StartCodeBehind_ShouldKeepBatchMenuVisibilityGuardAndSelectiveUpdates()
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, "start.aspx.cs"));

        Assert.Contains("private void BatchSetCurrentCourseMenuVisibility(bool isOpen)", content, StringComparison.Ordinal);
        Assert.Contains("if (String.IsNullOrEmpty(cid))", content, StringComparison.Ordinal);
        Assert.Contains("bool currentShow = Convert.ToBoolean(dt.Rows[i][\"Lshow\"]);", content, StringComparison.Ordinal);
        Assert.Contains("if (currentShow != isOpen)", content, StringComparison.Ordinal);
        Assert.Contains("lbll.OpenLshow(lid);", content, StringComparison.Ordinal);
        Assert.Contains("lbll.CloseLshow(lid);", content, StringComparison.Ordinal);
        Assert.Contains("System.Threading.Thread.Sleep(200);", content, StringComparison.Ordinal);
        Assert.Contains("showMenu();", content, StringComparison.Ordinal);
    }

    [Fact]
    public void MissionAndProgramShowPages_ShouldKeepSpecialLinksAndReturnButtons()
    {
        var missionShow = File.ReadAllText(Path.Combine(TeacherRoot, "missionshow.aspx"));
        var programShow = File.ReadAllText(Path.Combine(TeacherRoot, "programshow.aspx"));
        var pythonShow = File.ReadAllText(Path.Combine(TeacherRoot, "pythonshow.aspx"));
        var graphShow = File.ReadAllText(Path.Combine(TeacherRoot, "graphshow.aspx"));

        Assert.Contains("Text=\"编辑内容\"", missionShow, StringComparison.Ordinal);
        Assert.Contains("Text=\"返回学案\"", missionShow, StringComparison.Ordinal);
        Assert.Contains("ID=\"HLMgid\"", missionShow, StringComparison.Ordinal);
        Assert.Contains(">评价标准<", missionShow, StringComparison.Ordinal);
        Assert.Contains("Text=\"是否提交\"", missionShow, StringComparison.Ordinal);
        Assert.Contains("Text=\"上次作品\"", missionShow, StringComparison.Ordinal);
        Assert.Contains("Text=\"是否发布\"", missionShow, StringComparison.Ordinal);
        Assert.Contains("Text=\"小组合作\"", missionShow, StringComparison.Ordinal);

        Assert.Contains("Text=\"编辑内容\"", programShow, StringComparison.Ordinal);
        Assert.Contains("Text=\"返回学案\"", programShow, StringComparison.Ordinal);
        Assert.Contains("ID=\"Hlexample\"", programShow, StringComparison.Ordinal);
        Assert.Contains("实例下载", programShow, StringComparison.Ordinal);
        Assert.Contains("ID=\"HLMgid\"", programShow, StringComparison.Ordinal);
        Assert.Contains(">评价标准<", programShow, StringComparison.Ordinal);
        Assert.Contains("Text=\"是否发布\"", programShow, StringComparison.Ordinal);
        Assert.Contains("Text=\"作品继承\"", programShow, StringComparison.Ordinal);

        Assert.Contains("Text=\"编辑内容\"", pythonShow, StringComparison.Ordinal);
        Assert.Contains("Text=\"返回学案\"", pythonShow, StringComparison.Ordinal);
        Assert.Contains("ID=\"HLauto\"", pythonShow, StringComparison.Ordinal);
        Assert.Contains("自动批改", pythonShow, StringComparison.Ordinal);
        Assert.Contains("ID=\"HlExample\"", pythonShow, StringComparison.Ordinal);
        Assert.Contains("编程实例", pythonShow, StringComparison.Ordinal);
        Assert.Contains("Text=\"分步\"", pythonShow, StringComparison.Ordinal);
        Assert.Contains("Text=\"绘图\"", pythonShow, StringComparison.Ordinal);
        Assert.Contains("Text=\"拼图\"", pythonShow, StringComparison.Ordinal);
        Assert.Contains("Text=\"积木\"", pythonShow, StringComparison.Ordinal);

        Assert.Contains("Text=\"编辑内容\"", graphShow, StringComparison.Ordinal);
        Assert.Contains("Text=\"返回学案\"", graphShow, StringComparison.Ordinal);
        Assert.Contains("ID=\"Hlexample\"", graphShow, StringComparison.Ordinal);
        Assert.Contains("ID=\"HLMgid\"", graphShow, StringComparison.Ordinal);
        Assert.Contains(">评价标准<", graphShow, StringComparison.Ordinal);
        Assert.Contains("Text=\"是否发布\"", graphShow, StringComparison.Ordinal);
    }

    [Fact]
    public void MissionAndProgramShowCodeBehind_ShouldKeepShowContractsAndColdReturnFlow()
    {
        var missionShow = File.ReadAllText(Path.Combine(TeacherRoot, "missionshow.aspx.cs"));
        var programShow = File.ReadAllText(Path.Combine(TeacherRoot, "programshow.aspx.cs"));
        var pythonShow = File.ReadAllText(Path.Combine(TeacherRoot, "pythonshow.aspx.cs"));
        var graphShow = File.ReadAllText(Path.Combine(TeacherRoot, "graphshow.aspx.cs"));

        Assert.Contains("if (Request.QueryString[\"mcid\"] != null && Request.QueryString[\"mid\"] != null && Request.QueryString[\"lid\"] != null)", missionShow, StringComparison.Ordinal);
        Assert.Contains("BtnEdit.Enabled = false;", missionShow, StringComparison.Ordinal);
        Assert.Contains("Mcontent.InnerHtml = HttpUtility.HtmlDecode(model.Mcontent);", missionShow, StringComparison.Ordinal);
        Assert.Contains("HLMgid.NavigateUrl = \"~/teacher/gaugeitem.aspx?gid=\" + Mgid.ToString();", missionShow, StringComparison.Ordinal);
        Assert.Contains("string url = \"~/teacher/missionedit.aspx?mcid=\" + Mcid + \"&mid=\" + Mid + \"&lid=\" + Lid;", missionShow, StringComparison.Ordinal);
        Assert.Contains("url = url + \"&cold=T\";", missionShow, StringComparison.Ordinal);

        Assert.Contains("Mcontent.InnerHtml = HttpUtility.HtmlDecode(model.Mcontent);", programShow, StringComparison.Ordinal);
        Assert.Contains("Hlexample.NavigateUrl = sburl;", programShow, StringComparison.Ordinal);
        Assert.Contains("Hlexample.Text = LearnSite.Common.WordProcess.getshortfname(sburl);", programShow, StringComparison.Ordinal);
        Assert.Contains("Hlexample.Visible = false;", programShow, StringComparison.Ordinal);
        Assert.Contains("HLMgid.NavigateUrl = \"~/teacher/gaugeitem.aspx?gid=\" + Mgid.ToString();", programShow, StringComparison.Ordinal);
        Assert.Contains("string url = \"~/teacher/programedit.aspx?mcid=\" + Mcid + \"&mid=\" + Mid;", programShow, StringComparison.Ordinal);
        Assert.Contains("url = url + \"&cold=T\";", programShow, StringComparison.Ordinal);

        Assert.Contains("HLauto.NavigateUrl = \"~/teacher/judgeedit.aspx?mcid=\" + Mcid + \"&mid=\" + Mid;", pythonShow, StringComparison.Ordinal);
        Assert.Contains("Imgauto.ImageUrl = \"~/images/flashview.png\";", pythonShow, StringComparison.Ordinal);
        Assert.Contains("Imgauto.ImageUrl = \"~/images/flasherror.png\";", pythonShow, StringComparison.Ordinal);
        Assert.Contains("Checkblock.Checked = true;", pythonShow, StringComparison.Ordinal);
        Assert.Contains("Checkblockpy.Checked = true;", pythonShow, StringComparison.Ordinal);
        Assert.Contains("HlExample.NavigateUrl = model.Mexample;", pythonShow, StringComparison.Ordinal);
        Assert.Contains("Mcontent.InnerHtml = \"这里是python编程页面，你走错地方了!\";", pythonShow, StringComparison.Ordinal);
        Assert.Contains("string url = \"~/teacher/pythonedit.aspx?mcid=\" + Mcid + \"&mid=\" + Mid;", pythonShow, StringComparison.Ordinal);
        Assert.Contains("url = url + \"&cold=T\";", pythonShow, StringComparison.Ordinal);

        Assert.Contains("if (model.Mfiletype == \"xml\")", graphShow, StringComparison.Ordinal);
        Assert.Contains("Hlexample.Text = filename;", graphShow, StringComparison.Ordinal);
        Assert.Contains("Hlexample.NavigateUrl = examurl;", graphShow, StringComparison.Ordinal);
        Assert.Contains("HLMgid.NavigateUrl = \"~/teacher/gaugeitem.aspx?gid=\" + Mgid.ToString();", graphShow, StringComparison.Ordinal);
        Assert.Contains("Mcontent.InnerHtml = \"这里是流程图页面，你走错地方了!\";", graphShow, StringComparison.Ordinal);
        Assert.Contains("string url = \"~/teacher/graphedit.aspx?mcid=\" + Mcid + \"&mid=\" + Mid;", graphShow, StringComparison.Ordinal);
        Assert.Contains("url = url + \"&cold=T\";", graphShow, StringComparison.Ordinal);
    }

    [Fact]
    public void TopicShow_ShouldKeepToggleEditAndColdAwareReturnBehavior()
    {
        var topicShow = File.ReadAllText(Path.Combine(TeacherRoot, "topicshow.aspx"));
        var topicShowCodeBehind = File.ReadAllText(Path.Combine(TeacherRoot, "topicshow.aspx.cs"));

        Assert.Contains("Text=\"讨论状态\"", topicShow, StringComparison.Ordinal);
        Assert.Contains("Text=\"编辑内容\"", topicShow, StringComparison.Ordinal);
        Assert.Contains("Text=\"返回学案\"", topicShow, StringComparison.Ordinal);

        Assert.Contains("BtnEdit.Enabled = false;", topicShowCodeBehind, StringComparison.Ordinal);
        Assert.Contains("Btnclock.Text = \"已暂停\";", topicShowCodeBehind, StringComparison.Ordinal);
        Assert.Contains("Btnclock.Text = \"已开启\";", topicShowCodeBehind, StringComparison.Ordinal);
        Assert.Contains("Btnclock.ToolTip = \"点击开启讨论\";", topicShowCodeBehind, StringComparison.Ordinal);
        Assert.Contains("Btnclock.ToolTip = \"点击暂停讨论\";", topicShowCodeBehind, StringComparison.Ordinal);
        Assert.Contains("Tcontent.InnerHtml = HttpUtility.HtmlDecode(model.Tcontent);", topicShowCodeBehind, StringComparison.Ordinal);
        Assert.Contains("string url = \"~/teacher/topicedit.aspx?tcid=\" + LabelMcid.Text + \"&tid=\" + Labeltid.Text;", topicShowCodeBehind, StringComparison.Ordinal);
        Assert.Contains("tdbll.UpdateTclose(tid);", topicShowCodeBehind, StringComparison.Ordinal);
        Assert.Contains("url = url + \"&cold=T\";", topicShowCodeBehind, StringComparison.Ordinal);
    }

    [Fact]
    public void TermAndPackagePages_ShouldKeepExportAndPackagingEntryPoints()
    {
        var termView = File.ReadAllText(Path.Combine(TeacherRoot, "termview.aspx"));
        var termViewCodeBehind = File.ReadAllText(Path.Combine(TeacherRoot, "termview.aspx.cs"));
        var termScores = File.ReadAllText(Path.Combine(TeacherRoot, "termscores.aspx"));
        var packagePage = File.ReadAllText(Path.Combine(TeacherRoot, "package.aspx"));
        var packageCodeBehind = File.ReadAllText(Path.Combine(TeacherRoot, "package.aspx.cs"));

        Assert.Contains("Text=\"导出 Excel\"", termView, StringComparison.Ordinal);
        Assert.Contains("Text=\"成绩浏览\"", termView, StringComparison.Ordinal);
        Assert.Contains("Text=\"返回查询\"", termView, StringComparison.Ordinal);
        Assert.Contains("DataNavigateUrlFormatString=\"studentwork.aspx?snum={0}&amp;sgrade={1}&amp;sterm={2}\"", termView, StringComparison.Ordinal);
        Assert.Contains("tbll.GetGradeTermScore(tyear, tgrade, tclass, tterm)", termViewCodeBehind, StringComparison.Ordinal);
        Assert.Contains("tbll.TotalTermExcel(tyear, tgrade, tterm);", termViewCodeBehind, StringComparison.Ordinal);
        Assert.Contains("Response.Redirect(\"~/teacher/termscores.aspx\", false);", termViewCodeBehind, StringComparison.Ordinal);

        Assert.Contains("Text=\"未评设置C\"", termScores, StringComparison.Ordinal);
        Assert.Contains("Text=\"总分折算\"", termScores, StringComparison.Ordinal);
        Assert.Contains("Text=\"期末总评\"", termScores, StringComparison.Ordinal);
        Assert.Contains("Text=\"导出 Excel\"", termScores, StringComparison.Ordinal);
        Assert.Contains("Text=\"学期查询\"", termScores, StringComparison.Ordinal);
        Assert.Contains("Text=\"返回查询\"", termScores, StringComparison.Ordinal);
        Assert.Contains("DataNavigateUrlFormatString=\"studentwork.aspx?snum={0}\"", termScores, StringComparison.Ordinal);

        Assert.Contains("Text=\"开始打包\"", packagePage, StringComparison.Ordinal);
        Assert.Contains("Text=\"下载压缩包\"", packagePage, StringComparison.Ordinal);
        Assert.Contains("Text=\"返回学案\"", packagePage, StringComparison.Ordinal);
        Assert.Contains("PackageExists(Cid)", packageCodeBehind, StringComparison.Ordinal);
        Assert.Contains("BtnZip.Text = \"重打\";", packageCodeBehind, StringComparison.Ordinal);
        Assert.Contains("LearnSite.Store.XmlCourse.CourseToXml(Cid)", packageCodeBehind, StringComparison.Ordinal);
        Assert.Contains("LearnSite.Store.Package.ZipToPackageFile(Cid);", packageCodeBehind, StringComparison.Ordinal);
        Assert.Contains("LearnSite.Common.FileDown.DownPackageFile(myurl, CtitelFileName);", packageCodeBehind, StringComparison.Ordinal);
        Assert.Contains("hl.NavigateUrl = \"~/student/download.aspx?id=\" + LearnSite.Common.EnDeCode.Encrypt(Wurl, \"ls\");", packageCodeBehind, StringComparison.Ordinal);
        Assert.Contains("cbll.CreatPackageNameList(xmlpath);", packageCodeBehind, StringComparison.Ordinal);
    }

    [Fact]
    public void WarePages_ShouldUseLoadEventListenerAndServerResolvedTextboxId()
    {
        var wareAdd = File.ReadAllText(Path.Combine(TeacherRoot, "wareadd.aspx"));
        var wareEdit = File.ReadAllText(Path.Combine(TeacherRoot, "wareedit.aspx"));

        Assert.Contains("window.addEventListener('load'", wareAdd, StringComparison.Ordinal);
        Assert.Contains("window.addEventListener('load'", wareEdit, StringComparison.Ordinal);
        Assert.Contains("<%= TextBoxHtml.ClientID %>", wareAdd, StringComparison.Ordinal);
        Assert.Contains("<%= TextBoxHtml.ClientID %>", wareEdit, StringComparison.Ordinal);
    }

    [Fact]
    public void WarePages_ShouldKeepRawPathFileActionsAndHomepageSelectionFlow()
    {
        var wareAdd = File.ReadAllText(Path.Combine(TeacherRoot, "wareadd.aspx"));
        var wareEdit = File.ReadAllText(Path.Combine(TeacherRoot, "wareedit.aspx"));

        Assert.Contains("xhr.open('GET', 'ware.ashx?action=files&cid=' + cid + '&t=' + new Date().getTime(), true);", wareAdd, StringComparison.Ordinal);
        Assert.Contains("xhr.open('GET', 'ware.ashx?action=delete&cid='+cid+'&path=' + filePath, true);", wareAdd, StringComparison.Ordinal);
        Assert.Contains("TextBoxHtml.value = decodeURIComponent(filePath);", wareAdd, StringComparison.Ordinal);
        Assert.Contains("onclick=\"selectFile('${file.name}','${fileUrl}')\"", wareAdd, StringComparison.Ordinal);
        Assert.Contains("onclick=\"deleteFile('${filedel}')\"", wareAdd, StringComparison.Ordinal);

        Assert.Contains("xhr.open('GET', 'ware.ashx?action=files&cid=' + cid + '&t=' + new Date().getTime(), true);", wareEdit, StringComparison.Ordinal);
        Assert.Contains("xhr.open('GET', 'ware.ashx?action=delete&cid='+cid+'&path=' + filePath, true);", wareEdit, StringComparison.Ordinal);
        Assert.Contains("TextBoxHtml.value = decodeURIComponent(filePath);", wareEdit, StringComparison.Ordinal);
        Assert.Contains("onclick=\"selectFile('${file.name}','${fileUrl}')\"", wareEdit, StringComparison.Ordinal);
        Assert.Contains("onclick=\"deleteFile('${filedel}')\"", wareEdit, StringComparison.Ordinal);
    }

    [Fact]
    public void WarePages_ShouldKeepUploadProgressAndDelayedRefreshBehavior()
    {
        var wareAdd = File.ReadAllText(Path.Combine(TeacherRoot, "wareadd.aspx"));
        var wareEdit = File.ReadAllText(Path.Combine(TeacherRoot, "wareedit.aspx"));

        Assert.Contains("id=\"uploadProgressContainer\"", wareAdd, StringComparison.Ordinal);
        Assert.Contains("id=\"uploadProgressBar\"", wareAdd, StringComparison.Ordinal);
        Assert.Contains("id=\"uploadPercent\"", wareAdd, StringComparison.Ordinal);
        Assert.Contains("id=\"uploadStatus\"", wareAdd, StringComparison.Ordinal);
        Assert.True(CountOccurrences(wareAdd, "setTimeout(function() {") >= 4, "Expected delayed progress cleanup branches in wareadd.aspx.");
        Assert.Contains("loadFiles();", wareAdd, StringComparison.Ordinal);

        Assert.Contains("id=\"uploadProgressContainer\"", wareEdit, StringComparison.Ordinal);
        Assert.Contains("id=\"uploadProgressBar\"", wareEdit, StringComparison.Ordinal);
        Assert.Contains("id=\"uploadPercent\"", wareEdit, StringComparison.Ordinal);
        Assert.Contains("id=\"uploadStatus\"", wareEdit, StringComparison.Ordinal);
        Assert.True(CountOccurrences(wareEdit, "setTimeout(function() {") >= 4, "Expected delayed progress cleanup branches in wareedit.aspx.");
        Assert.Contains("loadFiles();", wareEdit, StringComparison.Ordinal);
    }

    [Fact]
    public void WareCodeBehind_ShouldKeepMissionBackPersistenceAndCourseReturnFlow()
    {
        var wareAddCodeBehind = File.ReadAllText(Path.Combine(TeacherRoot, "wareadd.aspx.cs"));
        var wareEditCodeBehind = File.ReadAllText(Path.Combine(TeacherRoot, "wareedit.aspx.cs"));

        Assert.Contains("if (Texttitle.Text != \"\" && TextBoxHtml.Text != \"\")", wareAddCodeBehind, StringComparison.Ordinal);
        Assert.Contains("mission.Mback = TextBoxHtml.Text;", wareAddCodeBehind, StringComparison.Ordinal);
        Assert.Contains("mission.Mupload = true;", wareAddCodeBehind, StringComparison.Ordinal);
        Assert.Contains("mission.Mfiletype = \"ware\";", wareAddCodeBehind, StringComparison.Ordinal);
        Assert.Contains("string url = \"~/teacher/courseshow.aspx?cid=\" + Mcid.ToString();", wareAddCodeBehind, StringComparison.Ordinal);
        Assert.Contains("string url = \"~/teacher/courseshow.aspx?cid=\" + Cid;", wareAddCodeBehind, StringComparison.Ordinal);

        Assert.Contains("TextBoxHtml.Text = Server.UrlDecode(mission.Mback);", wareEditCodeBehind, StringComparison.Ordinal);
        Assert.Contains("mission.Mback = TextBoxHtml.Text;", wareEditCodeBehind, StringComparison.Ordinal);
        Assert.Contains("mission.Mupload = true;", wareEditCodeBehind, StringComparison.Ordinal);
        Assert.Contains("mission.Mfiletype = \"ware\";", wareEditCodeBehind, StringComparison.Ordinal);
        Assert.Contains("lbll.UpdateMenuThree(lmodel);", wareEditCodeBehind, StringComparison.Ordinal);
        Assert.Contains("string url = \"~/teacher/courseshow.aspx?cid=\" + Mcid.ToString();", wareEditCodeBehind, StringComparison.Ordinal);
        Assert.Contains("string url = \"~/teacher/courseshow.aspx?cid=\" + Cid;", wareEditCodeBehind, StringComparison.Ordinal);
    }

    [Fact]
    public void WareShow_ShouldKeepPreviewIframeAndCoursewareLinkContracts()
    {
        var wareShow = File.ReadAllText(Path.Combine(TeacherRoot, "wareshow.aspx"));
        var wareShowCodeBehind = File.ReadAllText(Path.Combine(TeacherRoot, "wareshow.aspx.cs"));

        Assert.Contains("Text=\"编辑内容\"", wareShow, StringComparison.Ordinal);
        Assert.Contains("Text=\"返回学案\"", wareShow, StringComparison.Ordinal);
        Assert.Contains("<asp:HyperLink ID=\"HyperLinkHtml\"", wareShow, StringComparison.Ordinal);
        Assert.Contains("Target=\"_blank\"", wareShow, StringComparison.Ordinal);
        Assert.Contains("<iframe id=\"htmliframe\" src=\"<%=WareUrl %>\" class=\"ware-show-frame\"></iframe>", wareShow, StringComparison.Ordinal);

        Assert.Contains("HyperLinkHtml.NavigateUrl = model.Mback;", wareShowCodeBehind, StringComparison.Ordinal);
        Assert.Contains("WareUrl = model.Mback;", wareShowCodeBehind, StringComparison.Ordinal);
        Assert.Contains("url = url + \"&cold=T\";", wareShowCodeBehind, StringComparison.Ordinal);
        Assert.Contains("string url = \"~/teacher/wareedit.aspx?mcid=\" + Mcid + \"&mid=\" + Mid;", wareShowCodeBehind, StringComparison.Ordinal);
    }

    [Fact]
    public void CourseEdit_ShouldContainActivityPlanAssistantPanelMarkup()
    {
        var courseEdit = File.ReadAllText(Path.Combine(TeacherRoot, "courseedit.aspx"));

        Assert.Contains("活动计划助手", courseEdit, StringComparison.Ordinal);
        Assert.Contains("id=\"activity-plan-topic\"", courseEdit, StringComparison.Ordinal);
        Assert.Contains("id=\"activity-plan-grade\"", courseEdit, StringComparison.Ordinal);
        Assert.Contains("id=\"activity-plan-duration\"", courseEdit, StringComparison.Ordinal);
        Assert.Contains("id=\"activity-plan-goals\"", courseEdit, StringComparison.Ordinal);
        Assert.DoesNotContain("学科", courseEdit, StringComparison.Ordinal);
        Assert.Contains("OnClientClick=\"return syncContent();\"", courseEdit, StringComparison.Ordinal);
    }

    [Theory]
    [MemberData(nameof(GetContentPagesReturningToCourse))]
    public void ContentPages_ShouldKeepReturnToCourseCopy(string relativePath)
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, relativePath));
        Assert.Contains("Text=\"返回学案\"", content, StringComparison.Ordinal);
    }

    [Theory]
    [MemberData(nameof(GetShowPagesWithEditAndReturn))]
    public void ShowPages_ShouldKeepEditContentAndReturnCourseButtons(string relativePath)
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, relativePath));
        Assert.Contains("Text=\"编辑内容\"", content, StringComparison.Ordinal);
        Assert.Contains("Text=\"返回学案\"", content, StringComparison.Ordinal);
    }

    [Theory]
    [MemberData(nameof(GetTypingPagesUsingReturnList))]
    public void TypingPages_ShouldUseReturnListCopy(string relativePath)
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, relativePath));
        Assert.Contains("Text=\"返回列表\"", content, StringComparison.Ordinal);
    }

    [Fact]
    public void StatusPages_ShouldKeepModernStatusButtonLabels()
    {
        var consoleShow = File.ReadAllText(Path.Combine(TeacherRoot, "consoleshow.aspx"));
        var topicShow = File.ReadAllText(Path.Combine(TeacherRoot, "topicshow.aspx"));
        var circleShow = File.ReadAllText(Path.Combine(TeacherRoot, "circleshow.aspx"));
        var workNoScore = File.ReadAllText(Path.Combine(TeacherRoot, "worknoscore.aspx"));
        var stuWorkCircle = File.ReadAllText(Path.Combine(TeacherRoot, "stuworkcircle.aspx"));
        var softNomic = File.ReadAllText(Path.Combine(TeacherRoot, "softnomic.aspx"));

        Assert.Contains("Text=\"测评状态\"", consoleShow, StringComparison.Ordinal);
        Assert.Contains("Text=\"讨论状态\"", topicShow, StringComparison.Ordinal);
        Assert.Contains("Text=\"标记已评\"", circleShow, StringComparison.Ordinal);
        Assert.Contains("Text=\"刷新展播\"", circleShow, StringComparison.Ordinal);
        Assert.Contains("Text=\"刷新展播\"", workNoScore, StringComparison.Ordinal);
        Assert.Contains("Text=\"刷新展播\"", stuWorkCircle, StringComparison.Ordinal);
        Assert.Contains("Text=\"刷新展播\"", softNomic, StringComparison.Ordinal);
    }

    [Fact]
    public void StartPage_MenuAreaShouldKeepBatchButtonsAndSwitcherMarkup()
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, "start.aspx"));

        Assert.Contains("Text=\"全部开启\"", content, StringComparison.Ordinal);
        Assert.Contains("Text=\"全部关闭\"", content, StringComparison.Ordinal);
        Assert.Contains("ID=\"imgBtn\"", content, StringComparison.Ordinal);
        Assert.Contains("ID=\"BtnSwitchToggle\"", content, StringComparison.Ordinal);
        Assert.Contains("lesson-switch lesson-switch--on", content, StringComparison.Ordinal);
    }

    [Fact]
    public void StartCodeBehind_ShouldKeepBatchOpenCloseCalls()
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, "start.aspx.cs"));

        Assert.Contains("BatchSetCurrentCourseMenuVisibility(true);", content, StringComparison.Ordinal);
        Assert.Contains("BatchSetCurrentCourseMenuVisibility(false);", content, StringComparison.Ordinal);
        Assert.Contains("lbll.OpenLshow(lid);", content, StringComparison.Ordinal);
        Assert.Contains("lbll.CloseLshow(lid);", content, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("consoleadd.aspx", "Text=\"返回学案\"")]
    [InlineData("consoleshow.aspx", "Text=\"返回学案\"")]
    [InlineData("problem.aspx", "Text=\"添加题目\"")]
    [InlineData("problem.aspx", "Text=\"返回测评\"")]
    [InlineData("package.aspx", "Text=\"开始打包\"")]
    [InlineData("package.aspx", "Text=\"下载压缩包\"")]
    [InlineData("termview.aspx", "Text=\"导出 Excel\"")]
    [InlineData("termscores.aspx", "Text=\"导出 Excel\"")]
    [InlineData("studentadd.aspx", "Text=\"添加学生\"")]
    [InlineData("studentedit.aspx", "Text=\"保存修改\"")]
    public void TeacherButtonCopy_ShouldRetainNormalizedLabels(string relativePath, string expectedSnippet)
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, relativePath));
        Assert.Contains(expectedSnippet, content, StringComparison.Ordinal);
    }

    [Theory]
    [MemberData(nameof(GetShowPagesUsingPreviewCopy))]
    public void ShowPages_ShouldDescribePreviewInsteadOfEditingInPageCopy(string relativePath)
    {
        var content = File.ReadAllText(Path.Combine(TeacherRoot, relativePath));

        Assert.True(content.IndexOf("预览当前", StringComparison.Ordinal) >= 0 || content.IndexOf("当前", StringComparison.Ordinal) >= 0, $"Expected preview-focused copy in {relativePath}.");
        Assert.DoesNotContain("并可进入编辑页面", content, StringComparison.Ordinal);
        Assert.DoesNotContain("继续维护", content, StringComparison.Ordinal);
        Assert.DoesNotContain("保留原有", content, StringComparison.Ordinal);
        Assert.DoesNotContain("仅优化展示区版式", content, StringComparison.Ordinal);
    }

    [Theory]
    [MemberData(nameof(GetSeatPagesRequiringUtf8))]
    public void SeatPages_ShouldDeclareUtf8Encoding(string relativePath)
    {
        var content = File.ReadAllText(Path.Combine(RepoRoot, "seat", relativePath));

        Assert.Contains("ResponseEncoding=\"utf-8\"", content, StringComparison.Ordinal);

        if (!relativePath.Equals("house.aspx", StringComparison.OrdinalIgnoreCase))
        {
            Assert.Contains("charset=utf-8", content, StringComparison.OrdinalIgnoreCase);
        }
    }

    public static IEnumerable<object[]> GetThreeEditorPages()
    {
        foreach (var page in ThreeEditorPages)
        {
            yield return new object[] { page };
        }
    }

    public static IEnumerable<object[]> GetContentPagesReturningToCourse()
    {
        foreach (var page in ContentPagesReturningToCourse)
        {
            yield return new object[] { page };
        }
    }

    public static IEnumerable<object[]> GetShowPagesWithEditAndReturn()
    {
        foreach (var page in ShowPagesWithEditAndReturn)
        {
            yield return new object[] { page };
        }
    }

    public static IEnumerable<object[]> GetTypingPagesUsingReturnList()
    {
        foreach (var page in TypingPagesUsingReturnList)
        {
            yield return new object[] { page };
        }
    }

    public static IEnumerable<object[]> GetShowPagesUsingPreviewCopy()
    {
        foreach (var page in ShowPagesUsingPreviewCopy)
        {
            yield return new object[] { page };
        }
    }

    public static IEnumerable<object[]> GetSeatPagesRequiringUtf8()
    {
        foreach (var page in SeatPagesRequiringUtf8)
        {
            yield return new object[] { page };
        }
    }

    private static int CountOccurrences(string content, string value)
    {
        var count = 0;
        var index = 0;
        while ((index = content.IndexOf(value, index, StringComparison.Ordinal)) >= 0)
        {
            count++;
            index += value.Length;
        }

        return count;
    }

    private static string GetRepoRoot()
    {
        var current = AppContext.BaseDirectory;
        while (!string.IsNullOrEmpty(current))
        {
            if (File.Exists(Path.Combine(current, "learnsite-wz.sln")) || File.Exists(Path.Combine(current, "openlearnsite.sln")))
            {
                return current;
            }

            current = Directory.GetParent(current)?.FullName ?? string.Empty;
        }

        throw new InvalidOperationException("Could not locate repository root.");
    }
}
