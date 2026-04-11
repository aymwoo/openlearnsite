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
        Assert.Contains("计划草案预览", courseEdit, StringComparison.Ordinal);
        Assert.Contains("id=\"activity-plan-result\"", courseEdit, StringComparison.Ordinal);
        Assert.Contains("生成活动计划", courseEdit, StringComparison.Ordinal);
        Assert.Contains("复制草案", courseEdit, StringComparison.Ordinal);
        Assert.DoesNotContain("学科", courseEdit, StringComparison.Ordinal);
        Assert.Contains("OnClientClick=\"return syncContent();\"", courseEdit, StringComparison.Ordinal);
        Assert.Contains("activity-plan-preview-note", courseEdit, StringComparison.Ordinal);
        Assert.Contains("整课模式会按顺序展示课堂环节卡片", courseEdit, StringComparison.Ordinal);
        Assert.Contains("activity-plan-block-card", courseEdit, StringComparison.Ordinal);
        Assert.Contains("activity-plan-block-meta-list", courseEdit, StringComparison.Ordinal);
        Assert.Contains("activity-plan-block-type-badge", courseEdit, StringComparison.Ordinal);
        Assert.Contains("activity-plan-type-preview", courseEdit, StringComparison.Ordinal);
        Assert.Contains("activity-plan-type-preview-title", courseEdit, StringComparison.Ordinal);
        Assert.Contains("activity-plan-inquiry-summary", courseEdit, StringComparison.Ordinal);
    }

    [Fact]
    public void CourseEdit_ShouldKeepPlanningRequestAndSafeRenderingHooks()
    {
        var courseEditScript = File.ReadAllText(Path.Combine(RepoRoot, "js", "courseedit.js"));

        Assert.Contains("switchEditor(", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("syncContent()", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("action=fullLessonGenerate", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("action=fullLessonRegenerateBlock", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("activity-plan-topic", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("activity-plan-grade", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("activity-plan-duration", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("activity-plan-goals", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("existingCourseContent", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("blockKey", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("currentDraft", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("renderActivityPlanDraft", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("renderFullLessonDraft", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("buildActivityPlanCopyText", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("regenerateFullLessonBlock", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("lastFullLessonDraftResponse = previousResponse", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("document.createElement", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("blockType", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("teachingPurpose", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("lessonPosition", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("minutes", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("buildActivityPlanPreviewItems", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("appendActivityPlanTypePreview", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("getBlockTypeDisplayName", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("removeFullLessonBlock", courseEditScript, StringComparison.Ordinal);
        Assert.DoesNotContain("regenerate-step", courseEditScript, StringComparison.Ordinal);
        Assert.DoesNotContain("resultArea.innerHTML = text", courseEditScript, StringComparison.Ordinal);
    }

    [Fact]
    public void CourseEdit_ShouldExposeApplyAndResumePanelMarkup()
    {
        var courseEdit = File.ReadAllText(Path.Combine(TeacherRoot, "courseedit.aspx"));

        Assert.Contains("id=\"activity-plan-draft-banner\"", courseEdit, StringComparison.Ordinal);
        Assert.Contains("id=\"activity-plan-resume-btn\"", courseEdit, StringComparison.Ordinal);
        Assert.Contains("id=\"activity-plan-save-draft-btn\"", courseEdit, StringComparison.Ordinal);
        Assert.Contains("id=\"activity-plan-apply-selected-btn\"", courseEdit, StringComparison.Ordinal);
        Assert.Contains("继续上次整课草案", courseEdit, StringComparison.Ordinal);
        Assert.Contains("保存草案", courseEdit, StringComparison.Ordinal);
        Assert.Contains("应用所选章节", courseEdit, StringComparison.Ordinal);
        Assert.Contains("id=\"activity-plan-publish-toggle\"", courseEdit, StringComparison.Ordinal);
        Assert.Contains("id=\"activity-plan-publish-btn\"", courseEdit, StringComparison.Ordinal);
        Assert.Contains("活动默认保持隐藏", courseEdit, StringComparison.Ordinal);
        Assert.DoesNotContain("id=\"activity-plan-publish-toggle\" runat=\"server\"", courseEdit, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CourseEdit_ShouldKeepAppendOnlyApplyAndDraftResumeHooks()
    {
        var courseEditScript = File.ReadAllText(Path.Combine(RepoRoot, "js", "courseedit.js"));

        Assert.Contains("var activityPlanDraftStatus =", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("var lastFullLessonDraftResponse = null;", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("resetFullLessonBlockStates", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("setFullLessonBlockState", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("var activityPlanSelectionState =", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("toggleActivityPlanSectionSelection", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("applySelectedActivityPlanSections", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("confirmActivityPlanApply", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("appendActivityPlanSectionsToEditor", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("escapeActivityPlanHtml", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("action=fullLessonDraftStatus", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("fullLessonSaveDraft", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("fullLessonLoadDraft", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("fullLessonDeleteDraft", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("function publishActivityPlan()", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("action=activityPlanPublish", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("updatedCourseContent", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("maybeHandleSavedDraftBeforeGenerate", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("checkSavedActivityPlanDraftStatus(function (status)", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("window.confirm", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("整课草案当前保持预览优先，本阶段不会自动写入学案正文", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("确认发布当前整课草案", courseEditScript, StringComparison.Ordinal);
        Assert.DoesNotContain("action=saveCourse", courseEditScript, StringComparison.Ordinal);
        Assert.DoesNotContain("Btnedit.click()", courseEditScript, StringComparison.Ordinal);
    }

    [Fact]
    public void CourseEdit_ShouldNameSectionsBeforeAppendAndAvoidOverwriteLogic()
    {
        var courseEditScript = File.ReadAllText(Path.Combine(RepoRoot, "js", "courseedit.js"));

        Assert.Contains("教学目标", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("活动步骤", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("教学资源", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("评价设计", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("教师提醒", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("field.value = existingContent + appendedContent", courseEditScript, StringComparison.Ordinal);
        Assert.DoesNotContain("replace(existingContent", courseEditScript, StringComparison.Ordinal);
        Assert.DoesNotContain("dedupe", courseEditScript, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CourseEdit_FullLessonPreview_ShouldRenderOrderedBlockCardsFromServerDtoOnly()
    {
        var courseEditScript = File.ReadAllText(Path.Combine(RepoRoot, "js", "courseedit.js"));

        Assert.Contains("isFullLessonDraftResponse", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("renderFullLessonDraft", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("activity-plan-block-order", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("活动类型", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("教学目的", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("课堂位置", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("预计时长", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("block.blockType", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("block.teachingPurpose", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("block.lessonPosition", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("block.minutes", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("buildQuizPreviewItems", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("buildResourceStudyPreviewItems", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("buildWebCoursewarePreviewItems", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("buildGuidedInquiryPreviewItems", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("已有活动摘要", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("引导探究", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("appendGuidedInquirySummary", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("为何使用引导探究回退", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("guidedInquiry", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("试卷名称", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("阅读内容摘要", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("首页地址", courseEditScript, StringComparison.Ordinal);
        Assert.DoesNotContain("innerHTML = block", courseEditScript, StringComparison.Ordinal);
    }

    [Fact]
    public void CourseEdit_FullLessonBlockActions_ShouldStayScopedByBlockKeyAndKeepBodyUntouched()
    {
        var courseEditScript = File.ReadAllText(Path.Combine(RepoRoot, "js", "courseedit.js"));

        Assert.Contains("removeFullLessonBlock(blockKey)", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("regenerateFullLessonBlock(blockKey)", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("setFullLessonBlockState(blockKey, true, '')", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("action=fullLessonRegenerateBlock", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("&blockKey=", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("currentBlocks.length <= 1", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("nextBlocks[j].sort = j + 1;", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("draftType === 'fullLesson' ? 'fullLessonSaveDraft' : 'activityPlanSaveDraft'", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("fullLessonLoadDraft", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("fullLessonDraftStatus", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("appendTypedPreviewCopyLines", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("summarizeHtmlText", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("课件摘要", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("讲解卡片数", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("整课草案当前保持预览优先，本阶段不会自动写入学案正文", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("确认发布当前整课草案", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("action=fullLessonPublish", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("publishFullLessonDraft()", courseEditScript, StringComparison.Ordinal);
        Assert.Contains("window.confirm(confirmText)", courseEditScript, StringComparison.Ordinal);
    }

    [Fact]
    public void ActivityPlanPublish_Handler_ShouldStayAuthorizedAndDelegateToPublishCore()
    {
        var handler = File.ReadAllText(Path.Combine(TeacherRoot, "aiprovider_api.ashx"));

        Assert.Contains("case \"activityPlanPublish\":", handler, StringComparison.Ordinal);
        Assert.Contains("TryGetAuthorizedCourse", handler, StringComparison.Ordinal);
        Assert.Contains("AIActivityPlanDraftHelper.ParseDraft", handler, StringComparison.Ordinal);
        Assert.Contains("AIActivityPlanPublisher", handler, StringComparison.Ordinal);
        Assert.Contains("updatedCourseContent", handler, StringComparison.Ordinal);
        Assert.DoesNotContain("insert into Mission", handler, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("insert into ListMenu", handler, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CourseEdit_FullLessonHandler_ShouldExposeParallelAuthorizedDraftActions()
    {
        var handler = File.ReadAllText(Path.Combine(TeacherRoot, "aiprovider_api.ashx"));

        Assert.Contains("case \"fullLessonGenerate\":", handler, StringComparison.Ordinal);
        Assert.Contains("case \"fullLessonRegenerateBlock\":", handler, StringComparison.Ordinal);
        Assert.Contains("case \"fullLessonDraftStatus\":", handler, StringComparison.Ordinal);
        Assert.Contains("case \"fullLessonSaveDraft\":", handler, StringComparison.Ordinal);
        Assert.Contains("case \"fullLessonLoadDraft\":", handler, StringComparison.Ordinal);
        Assert.Contains("case \"fullLessonDeleteDraft\":", handler, StringComparison.Ordinal);
        Assert.Contains("TryGetAuthorizedCourse", handler, StringComparison.Ordinal);
        Assert.Contains("AIActivityPlanDraftHelper.ParseFullLessonDraft", handler, StringComparison.Ordinal);
        Assert.Contains("AIActivityPlanSavedDraftHelper.BuildFullLessonRecord", handler, StringComparison.Ordinal);
        Assert.Contains("AIActivityPlanSavedDraftHelper.ParseFullLessonRecord", handler, StringComparison.Ordinal);
        Assert.Contains("string.Equals(block.BlockKey, blockKey, StringComparison.OrdinalIgnoreCase)", handler, StringComparison.Ordinal);
        Assert.Contains("string.Equals(block.BlockType, currentBlock.BlockType, StringComparison.OrdinalIgnoreCase)", handler, StringComparison.Ordinal);
        Assert.Contains("GetFullLessonTotalMinutes(duration, fullLessonDraft.Blocks)", handler, StringComparison.Ordinal);
        Assert.Contains("BlockType = \"resource-study\"", handler, StringComparison.Ordinal);
        Assert.Contains("BlockType = \"webCourseware\"", handler, StringComparison.Ordinal);
        Assert.Contains("BuildQuizPayload", handler, StringComparison.Ordinal);
        Assert.Contains("BuildResourceStudyPayload", handler, StringComparison.Ordinal);
        Assert.Contains("BuildWebCoursewarePayload", handler, StringComparison.Ordinal);
        Assert.Contains("/ai/courseware/preview.html?topic=", handler, StringComparison.Ordinal);
        Assert.Contains("&source=published", handler, StringComparison.Ordinal);
        Assert.Contains("BuildWebCoursewarePreviewUrl", handler, StringComparison.Ordinal);
        Assert.Contains("lessonSummary = block.WebCourseware.LessonSummary", handler, StringComparison.Ordinal);
        Assert.Contains("explanationCards = block.WebCourseware.ExplanationCards.Select", handler, StringComparison.Ordinal);
        Assert.Contains("practiceItems = block.WebCourseware.PracticeItems.Select", handler, StringComparison.Ordinal);
        Assert.Contains("BuildGuidedInquiryPayload", handler, StringComparison.Ordinal);
        Assert.Contains("ResolveFullLessonBlockType", handler, StringComparison.Ordinal);
        Assert.Contains("guidedInquiry = block.GuidedInquiry == null ? null", handler, StringComparison.Ordinal);
        Assert.Contains("return \"guidedInquiry\";", handler, StringComparison.Ordinal);
        Assert.Contains("ltype = block.Quiz.Ltype", handler, StringComparison.Ordinal);
        Assert.Contains("mcontent = block.ResourceStudy.Mcontent", handler, StringComparison.Ordinal);
        Assert.Contains("mfiletype = block.WebCourseware.Mfiletype", handler, StringComparison.Ordinal);
        Assert.DoesNotContain("new LearnSite.BLL.Mission()", handler, StringComparison.Ordinal);
        Assert.DoesNotContain("new LearnSite.BLL.Exam()", handler, StringComparison.Ordinal);
        Assert.DoesNotContain("new LearnSite.BLL.ListMenu()", handler, StringComparison.Ordinal);
        Assert.True(File.Exists(Path.Combine(RepoRoot, "ai", "courseware", "preview.html")));
    }

    [Fact]
    public void CourseEdit_FullLessonHandler_ShouldKeepActivityPlanPublishPathUntouched()
    {
        var handler = File.ReadAllText(Path.Combine(TeacherRoot, "aiprovider_api.ashx"));

        Assert.Contains("case \"activityPlanPublish\":", handler, StringComparison.Ordinal);
        Assert.Contains("AIActivityPlanPublisher", handler, StringComparison.Ordinal);
        Assert.Contains("updatedCourseContent", handler, StringComparison.Ordinal);
        Assert.Contains("case \"fullLessonPublish\":", handler, StringComparison.Ordinal);
        Assert.Contains("AIActivityPlanDraftHelper.ParseFullLessonDraft", handler, StringComparison.Ordinal);
        Assert.Contains("private void FullLessonPublish(HttpContext context)", handler, StringComparison.Ordinal);
        Assert.Contains("IsSupportedPublishedBlockType", handler, StringComparison.Ordinal);
        Assert.Contains("整课草案包含当前暂不支持发布的环节类型", handler, StringComparison.Ordinal);
        Assert.Contains("整课草案发布失败，请检查环节发布配置后重试。", handler, StringComparison.Ordinal);
    }

    [Fact]
    public void FullLessonDraftHelper_Source_ShouldFailClosedForGuidedInquiryPayloads()
    {
        var helper = File.ReadAllText(Path.Combine(CommonRoot, "AIActivityPlanDraftHelper.cs"));

        Assert.Contains("public GuidedInquiryBlockPayload GuidedInquiry", helper, StringComparison.Ordinal);
        Assert.Contains("NormalizeGuidedInquiryPayload", helper, StringComparison.Ordinal);
        Assert.Contains("IsValidGuidedInquiryPayload", helper, StringComparison.Ordinal);
        Assert.Contains("IsGuidedInquiryBlockType", helper, StringComparison.Ordinal);
        Assert.Contains("payload.Steps == null", helper, StringComparison.Ordinal);
        Assert.Contains("step.Sort != i + 1", helper, StringComparison.Ordinal);
        Assert.Contains("public List<WebCoursewareExplanationCardPayload> ExplanationCards", helper, StringComparison.Ordinal);
        Assert.Contains("public List<WebCoursewarePracticeItemPayload> PracticeItems", helper, StringComparison.Ordinal);
        Assert.Contains("NormalizeWebCoursewareExplanationCards", helper, StringComparison.Ordinal);
        Assert.Contains("NormalizeWebCoursewarePracticeItems", helper, StringComparison.Ordinal);
    }

    [Fact]
    public void CourseEdit_FullLessonDraftPersistence_ShouldReuseSingleCurrentDraftRecord()
    {
        var dal = File.ReadAllText(Path.Combine(RepoRoot, "App_Code", "Dal", "CourseActivityPlanDraft.cs"));
        var model = File.ReadAllText(Path.Combine(RepoRoot, "App_Code", "Model", "CourseActivityPlanDraft.cs"));

        Assert.Contains("where Cid=@Cid and Hid=@Hid", dal, StringComparison.Ordinal);
        Assert.Contains("delete from CourseActivityPlanDraft where Cid=@Cid and Hid=@Hid", dal, StringComparison.Ordinal);
        Assert.Contains("public string DraftJson", model, StringComparison.Ordinal);
        Assert.DoesNotContain("FullLessonDraftJson", model, StringComparison.Ordinal);
    }

    [Fact]
    public void ActivityPlanStudentEntry_Publisher_ShouldKeepMissionMenuContract()
    {
        var publisher = File.ReadAllText(Path.Combine(RepoRoot, "App_Code", "Dal", "AIActivityPlanPublisher.cs"));

        Assert.Contains("@Ltype", publisher, StringComparison.Ordinal);
        Assert.Contains("command.Parameters.AddWithValue(\"@Ltype\", 1);", publisher, StringComparison.Ordinal);
        Assert.Contains("command.Parameters.AddWithValue(\"@Mupload\", true);", publisher, StringComparison.Ordinal);
        Assert.Contains("command.Parameters.AddWithValue(\"@Lshow\", request.PublishToStudents);", publisher, StringComparison.Ordinal);
        Assert.Contains("BuildFullLessonLessonContent", publisher, StringComparison.Ordinal);
        Assert.Contains("PublishFullLesson", publisher, StringComparison.Ordinal);
        Assert.Contains("block.BlockKey", publisher, StringComparison.Ordinal);
        Assert.Contains("publishLinks[block.BlockKey]", publisher, StringComparison.Ordinal);
        Assert.Contains("@Ltype", publisher, StringComparison.Ordinal);
        Assert.Contains("case \"mission\":", publisher, StringComparison.Ordinal);
        Assert.Contains("PublishMissionBlock", publisher, StringComparison.Ordinal);
        Assert.Contains("BuildMissionBlockContent", publisher, StringComparison.Ordinal);
        Assert.Contains("BuildPublishedWebCoursewareBackUrl", publisher, StringComparison.Ordinal);
        Assert.Contains("UpdateMissionBackUrl", publisher, StringComparison.Ordinal);
        Assert.Contains("\"lid=\" + listMenuId.ToString()", publisher, StringComparison.Ordinal);
        Assert.Contains("\"mid=\" + missionId.ToString()", publisher, StringComparison.Ordinal);
    }

    [Fact]
    public void WebCoursewareStudentRuntime_ShouldExposePayloadLookupAndFallbackAwarePreview()
    {
        var handler = File.ReadAllText(Path.Combine(RepoRoot, "student", "webcoursewarepayload.ashx"));
        var wareCodeBehind = File.ReadAllText(Path.Combine(RepoRoot, "student", "ware.aspx.cs"));
        var preview = File.ReadAllText(Path.Combine(RepoRoot, "ai", "courseware", "preview.html"));

        Assert.Contains("webcoursewarepayload : IHttpHandler", handler, StringComparison.Ordinal);
        Assert.Contains("context.Request[\"lid\"]", handler, StringComparison.Ordinal);
        Assert.Contains("context.Request[\"mid\"]", handler, StringComparison.Ordinal);
        Assert.Contains("WebCoursewareRuntimePayloadResolver.Resolve", handler, StringComparison.Ordinal);
        Assert.Contains("AppendRuntimeLocator", wareCodeBehind, StringComparison.Ordinal);
        Assert.Contains("loadPublishedPayload", preview, StringComparison.Ordinal);
        Assert.Contains("/student/webcoursewarepayload.ashx?", preview, StringComparison.Ordinal);
        Assert.Contains("renderView(createDefaultView(topic));", preview, StringComparison.Ordinal);
    }

    [Fact]
    public void ActivityPlanStudentEntry_ShowMissionRoute_ShouldStayPinnedToLegacyMissionPage()
    {
        var studentMenu = File.ReadAllText(Path.Combine(RepoRoot, "student", "Scm.master.cs"));

        Assert.Contains("case \"1\":", studentMenu, StringComparison.Ordinal);
        Assert.Contains("ma.NavigateUrl = \"~/student/showmission.aspx?lid=\" + Lid;", studentMenu, StringComparison.Ordinal);
        Assert.DoesNotContain("ma.NavigateUrl = \"~/student/show\" + mUrl + \".aspx?lid=\" + Lid;", studentMenu, StringComparison.Ordinal);
    }

    [Fact]
    public void UploadWork_ActivityPlanSubmission_ShouldResolveMissionFromListMenuLid()
    {
        var uploadWork = File.ReadAllText(Path.Combine(RepoRoot, "student", "uploadwork.aspx.cs"));

        Assert.Contains("Request.QueryString[\"lid\"]", uploadWork, StringComparison.Ordinal);
        Assert.Contains("WordProcess.IsNum(lidValue)", uploadWork, StringComparison.Ordinal);
        Assert.Contains("lmodel = lbll.GetModel(Int32.Parse(Wlid));", uploadWork, StringComparison.Ordinal);
        Assert.Contains("lmodel == null || !lmodel.Lxid.HasValue", uploadWork, StringComparison.Ordinal);
        Assert.Contains("string Wmid = lmodel.Lxid.Value.ToString();", uploadWork, StringComparison.Ordinal);
        Assert.Contains("mmodel = mbll.GetModel(lmodel.Lxid.Value);", uploadWork, StringComparison.Ordinal);
        Assert.Contains("ws.GetModelByStu(Int32.Parse(Wmid), Wnum)", uploadWork, StringComparison.Ordinal);
        Assert.Contains("ws.UpdateWorkUp(wmodelp.Wid, Wurl, NewFileName, Wlength, Wdate, checkcan, \"\")", uploadWork, StringComparison.Ordinal);
        Assert.Contains("ws.AddWorkUp(wmodel);", uploadWork, StringComparison.Ordinal);
        Assert.Contains("string Wextention = mmodel.Mfiletype;", uploadWork, StringComparison.Ordinal);
        Assert.Contains("limitext.Contains(Wfiletype)", uploadWork, StringComparison.Ordinal);
        Assert.Contains("ws.EnsureMenuWorksCompletion(Int32.Parse(Wsid), Int32.Parse(Wlid), DateTime.Parse(LoginTime), Wdate);", uploadWork, StringComparison.Ordinal);
        Assert.DoesNotContain("kbll.Add(kmodel);", uploadWork, StringComparison.Ordinal);
    }

    [Fact]
    public void UploadWorkM_ActivityPlanSubmission_ShouldKeepAlternateLidToMissionContract()
    {
        var uploadWorkM = File.ReadAllText(Path.Combine(RepoRoot, "student", "uploadworkm.aspx.cs"));

        Assert.Contains("Request.QueryString[\"lid\"]", uploadWorkM, StringComparison.Ordinal);
        Assert.Contains("WordProcess.IsNum(lidValue)", uploadWorkM, StringComparison.Ordinal);
        Assert.Contains("lmodel = lbll.GetModel(Int32.Parse(Wlid));", uploadWorkM, StringComparison.Ordinal);
        Assert.Contains("lmodel == null || !lmodel.Lxid.HasValue", uploadWorkM, StringComparison.Ordinal);
        Assert.Contains("string Wmid = lmodel.Lxid.Value.ToString();", uploadWorkM, StringComparison.Ordinal);
        Assert.Contains("mmodel = mbll.GetModel(lmodel.Lxid.Value);", uploadWorkM, StringComparison.Ordinal);
        Assert.Contains("ws.GetModelByStu(Int32.Parse(Wmid), Wnum)", uploadWorkM, StringComparison.Ordinal);
        Assert.Contains("ws.UpdateWorkUp(wmodelp.Wid, Wurl, NewFileName, Wlength, Wdate, checkcan, \"\")", uploadWorkM, StringComparison.Ordinal);
        Assert.Contains("ws.AddWorkUp(wmodel);", uploadWorkM, StringComparison.Ordinal);
        Assert.Contains("wmodel.Wlid = Int32.Parse(Wlid);", uploadWorkM, StringComparison.Ordinal);
        Assert.Contains("work_upload.InputStream != null && work_upload.ContentLength < maxSize", uploadWorkM, StringComparison.Ordinal);
        Assert.Contains("ws.EnsureMenuWorksCompletion(Int32.Parse(Wsid), Int32.Parse(Wlid), DateTime.Parse(LoginTime), Wdate);", uploadWorkM, StringComparison.Ordinal);
        Assert.DoesNotContain("kbll.Add(kmodel);", uploadWorkM, StringComparison.Ordinal);
    }

    [Fact]
    public void ShowMission_ActivityPlanStudentShell_ShouldExposeGuidedSectionsAndUploadPanel()
    {
        var showMissionPage = File.ReadAllText(Path.Combine(RepoRoot, "student", "showmission.aspx"));

        Assert.Contains("ID=\"PanelActivityGuide\"", showMissionPage, StringComparison.Ordinal);
        Assert.Contains("ID=\"LiteralActivityGuideNotice\"", showMissionPage, StringComparison.Ordinal);
        Assert.Contains("学习目标", showMissionPage, StringComparison.Ordinal);
        Assert.Contains("学习建议", showMissionPage, StringComparison.Ordinal);
        Assert.Contains("活动说明", showMissionPage, StringComparison.Ordinal);
        Assert.Contains("任务步骤", showMissionPage, StringComparison.Ordinal);
        Assert.Contains("ID=\"LiteralActivityGuideSteps\"", showMissionPage, StringComparison.Ordinal);
        Assert.Contains("ID=\"Panelworks\"", showMissionPage, StringComparison.Ordinal);
        Assert.Contains("HiddenMissionRaw", showMissionPage, StringComparison.Ordinal);
    }

    [Fact]
    public void ShowMission_ActivityPlanStudentCodeBehind_ShouldShapeGuidanceAndFailClosed()
    {
        var showMissionCodeBehind = File.ReadAllText(Path.Combine(RepoRoot, "student", "showmission.aspx.cs"));

        Assert.Contains("BuildActivityGuideView", showMissionCodeBehind, StringComparison.Ordinal);
        Assert.Contains("PanelActivityGuide.Visible = guide != null;", showMissionCodeBehind, StringComparison.Ordinal);
        Assert.Contains("LiteralActivityGuideNotice.Text = \"<p>请先阅读活动主题与任务步骤，再按顺序完成学习任务，最后在右侧作品提交区上传结果。</p>\";", showMissionCodeBehind, StringComparison.Ordinal);
        Assert.Contains("LiteralActivityGuideGoal.Text = guide.GoalHtml;", showMissionCodeBehind, StringComparison.Ordinal);
        Assert.Contains("LiteralActivityGuideSteps.Text = guide.StepsHtml;", showMissionCodeBehind, StringComparison.Ordinal);
        Assert.Contains("HiddenMissionRaw.Value = decodedContent;", showMissionCodeBehind, StringComparison.Ordinal);
        Assert.Contains("Mcontent.InnerHtml = decodedContent;", showMissionCodeBehind, StringComparison.Ordinal);
        Assert.Contains("Mcontent.InnerHtml = \"此学案活动不存在！\";", showMissionCodeBehind, StringComparison.Ordinal);
    }

    [Fact]
    public void ShowMission_ActivityPlanSubmissionState_ShouldExposeExplicitReadyResubmitAndLockedMessages()
    {
        var showMissionCodeBehind = File.ReadAllText(Path.Combine(RepoRoot, "student", "showmission.aspx.cs"));

        Assert.Contains("ShowReadyToSubmitState", showMissionCodeBehind, StringComparison.Ordinal);
        Assert.Contains("ShowResubmitState", showMissionCodeBehind, StringComparison.Ordinal);
        Assert.Contains("ShowLockedSubmissionState", showMissionCodeBehind, StringComparison.Ordinal);
        Assert.Contains("今天还没有提交作品，可先完成任务后上传结果！", showMissionCodeBehind, StringComparison.Ordinal);
        Assert.Contains("你已经提交过该活动作品，可修改后重新提交！", showMissionCodeBehind, StringComparison.Ordinal);
        Assert.Contains("该活动作品已被老师评价，当前不能重新提交！", showMissionCodeBehind, StringComparison.Ordinal);
        Assert.Contains("Panelswfupload.Visible = true;", showMissionCodeBehind, StringComparison.Ordinal);
        Assert.Contains("Panelswfupload.Visible = false;", showMissionCodeBehind, StringComparison.Ordinal);
    }

    [Fact]
    public void ShowMission_ActivityPlanSubmissionState_ShouldKeepIpAndPriorWorkRestrictionsExplicit()
    {
        var showMissionCodeBehind = File.ReadAllText(Path.Combine(RepoRoot, "student", "showmission.aspx.cs"));

        Assert.Contains("ShowIpBlockedState", showMissionCodeBehind, StringComparison.Ordinal);
        Assert.Contains("ShowPreviousWorkRequiredState", showMissionCodeBehind, StringComparison.Ordinal);
        Assert.Contains("已在该IP提交作品！", showMissionCodeBehind, StringComparison.Ordinal);
        Assert.Contains("请先提交前面作品！", showMissionCodeBehind, StringComparison.Ordinal);
        Assert.Contains("LabelMfiletype.Text != \"htm\"", showMissionCodeBehind, StringComparison.Ordinal);
    }

    [Fact]
    public void ActivityPlanCompletion_StudentMenu_ShouldKeepFinishStateBackedByMenuWorksAndWorkPass()
    {
        var studentMenu = File.ReadAllText(Path.Combine(RepoRoot, "student", "Scm.master.cs"));
        var studentMaster = File.ReadAllText(Path.Combine(RepoRoot, "student", "Scm.master"));

        Assert.Contains("LearnSite.BLL.MenuWorks kbll = new LearnSite.BLL.MenuWorks();", studentMenu, StringComparison.Ordinal);
        Assert.Contains("lcount = kbll.GetMyLidCount(cook.Sid, lidall);", studentMenu, StringComparison.Ordinal);
        Assert.Contains("bool codepass = wbll.WorkPass(cook.Sid, Int32.Parse(Lxidstr));", studentMenu, StringComparison.Ordinal);
        Assert.Contains("ma.ImageUrl = urlfinish;", studentMenu, StringComparison.Ordinal);
        Assert.Contains("LoadPublishedCourseSummaries", studentMenu, StringComparison.Ordinal);
        Assert.Contains("BuildMenuTitle", studentMenu, StringComparison.Ordinal);
        Assert.Contains("ApplyComposedProgressVisual", studentMenu, StringComparison.Ordinal);
        Assert.Contains("ComposedLessonSummaryHtml", studentMenu, StringComparison.Ordinal);
        Assert.Contains("scmComposedSummary", studentMaster, StringComparison.Ordinal);
        Assert.Contains("整课活动进度", studentMenu, StringComparison.Ordinal);
        Assert.DoesNotContain("AICompletion", studentMenu, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ActivityPlanCompletion_StudentSummary_ShouldKeepCompletedCourseAggregationOnLegacyModels()
    {
        var myInfoPage = File.ReadAllText(Path.Combine(RepoRoot, "student", "myinfo.aspx"));
        var myInfo = File.ReadAllText(Path.Combine(RepoRoot, "student", "myinfo.aspx.cs"));

        Assert.Contains("string wcids = wbll.ShowStuDoneWorkCids(mysnum, Cterm, Cgrade);", myInfo, StringComparison.Ordinal);
        Assert.Contains("LearnSite.BLL.MenuWorks kbll = new LearnSite.BLL.MenuWorks();", myInfo, StringComparison.Ordinal);
        Assert.Contains("string rcids = kbll.readCids(Int32.Parse(mySid));", myInfo, StringComparison.Ordinal);
        Assert.Contains("LabelCids.Text = LearnSite.Common.WordProcess.SimpleWordsNew(allcids);", myInfo, StringComparison.Ordinal);
        Assert.Contains("AppendComposedProgress", myInfo, StringComparison.Ordinal);
        Assert.Contains("BindComposedCourseSummary", myInfo, StringComparison.Ordinal);
        Assert.Contains("LoadPublishedCourseSummaries", myInfo, StringComparison.Ordinal);
        Assert.Contains("PanelComposedCourseSummary", myInfoPage, StringComparison.Ordinal);
        Assert.Contains("LiteralComposedCourseSummary", myInfoPage, StringComparison.Ordinal);
        Assert.DoesNotContain("AICompletion", myInfo, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ComposedRuntime_StudentBoundary_ShouldStayOnExistingMenuDrivenRuntimePages()
    {
        var studentMenu = File.ReadAllText(Path.Combine(RepoRoot, "student", "Scm.master.cs"));
        var showMissionPage = File.ReadAllText(Path.Combine(RepoRoot, "student", "showmission.aspx"));
        var showMissionCodeBehind = File.ReadAllText(Path.Combine(RepoRoot, "student", "showmission.aspx.cs"));
        var helper = File.ReadAllText(Path.Combine(CommonRoot, "AIActivityPlanComposedRuntimeHelper.cs"));

        Assert.Contains("~/student/showmission.aspx?lid=", studentMenu, StringComparison.Ordinal);
        Assert.Contains("~/student/description.aspx?lid=", studentMenu, StringComparison.Ordinal);
        Assert.Contains("~/student/ware.aspx?lid=", studentMenu, StringComparison.Ordinal);
        Assert.Contains("~/webform/preview.aspx?lid=", studentMenu, StringComparison.Ordinal);
        Assert.Contains("PanelComposedRuntime", showMissionPage, StringComparison.Ordinal);
        Assert.Contains("LiteralComposedRuntime", showMissionPage, StringComparison.Ordinal);
        Assert.Contains("BindComposedRuntimeContext", showMissionCodeBehind, StringComparison.Ordinal);
        Assert.Contains("LoadPublishedCourseSummaries", showMissionCodeBehind, StringComparison.Ordinal);
        Assert.Contains("BuildComposedRuntimeSummaries", helper, StringComparison.Ordinal);
        Assert.Contains("LoadPublishedCourseSummaries", helper, StringComparison.Ordinal);
        Assert.Contains("FindSummaryByListMenuId", helper, StringComparison.Ordinal);
        Assert.Contains("RuntimeRouteType", helper, StringComparison.Ordinal);
        Assert.DoesNotContain("activityplanruntime.aspx", studentMenu, StringComparison.OrdinalIgnoreCase);
        Assert.False(File.Exists(Path.Combine(RepoRoot, "student", "activityplanruntime.aspx")));
    }

    [Fact]
    public void ComposedRuntime_TeacherAndStudentSummaryBoundary_ShouldStayOnCurrentSurfaces()
    {
        var myInfo = File.ReadAllText(Path.Combine(RepoRoot, "student", "myinfo.aspx.cs"));
        var courseShowPage = File.ReadAllText(Path.Combine(RepoRoot, "teacher", "courseshow.aspx"));
        var courseShow = File.ReadAllText(Path.Combine(RepoRoot, "teacher", "courseshow.aspx.cs"));

        Assert.Contains("LearnSite.BLL.MenuWorks kbll = new LearnSite.BLL.MenuWorks();", myInfo, StringComparison.Ordinal);
        Assert.Contains("missionshow.aspx?mcid=", courseShow, StringComparison.Ordinal);
        Assert.Contains("wareshow.aspx?mcid=", courseShow, StringComparison.Ordinal);
        Assert.Contains("~/webform/exam.aspx?cid=", courseShow, StringComparison.Ordinal);
        Assert.Contains("PanelComposedTeacherSummary", courseShowPage, StringComparison.Ordinal);
        Assert.Contains("LiteralComposedTeacherSummary", courseShowPage, StringComparison.Ordinal);
        Assert.Contains("BindComposedTeacherSummary", courseShow, StringComparison.Ordinal);
        Assert.Contains("LoadPublishedCourseSummaries", courseShow, StringComparison.Ordinal);
        Assert.Contains("GetTeacherProgressText", courseShow, StringComparison.Ordinal);
        Assert.DoesNotContain("activityplanruntime.aspx", courseShow, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("AIRuntime", myInfo, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CourseShow_ComposedProgress_ShouldStayOnExistingCourseSurface()
    {
        var courseShowPage = File.ReadAllText(Path.Combine(RepoRoot, "teacher", "courseshow.aspx"));
        var courseShow = File.ReadAllText(Path.Combine(RepoRoot, "teacher", "courseshow.aspx.cs"));

        Assert.Contains("PanelComposedTeacherSummary", courseShowPage, StringComparison.Ordinal);
        Assert.Contains("LiteralComposedTeacherSummary", courseShowPage, StringComparison.Ordinal);
        Assert.Contains("BindComposedTeacherSummary", courseShow, StringComparison.Ordinal);
        Assert.Contains("LoadPublishedCourseSummaries", courseShow, StringComparison.Ordinal);
        Assert.Contains("ResolveAnyStudentMenuWork", courseShow, StringComparison.Ordinal);
        Assert.DoesNotContain("activityplanruntime.aspx", courseShow, StringComparison.OrdinalIgnoreCase);
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
