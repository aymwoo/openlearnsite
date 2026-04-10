using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using LearnSite.Common;

namespace CommonLogicTests;

public class CommonLogicTests : IDisposable
{
    private readonly string _tempDir;
    private readonly string _xmlFile;

    public CommonLogicTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), "CommonLogicTests_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_tempDir);
        _xmlFile = Path.Combine(_tempDir, "website.xml");
        File.WriteAllText(_xmlFile, "<?xml version=\"1.0\" encoding=\"utf-8\"?><LearnSite><website>" +
            "<add key=\"CourseType\" value=\"Word|Powerpoint|Excel\" />" +
            "<add key=\"CoursePeriod\" value=\"3\" />" +
            "<add key=\"ClassMax\" value=\"4\" />" +
            "<add key=\"CookiesFix\" value=\"1\" />" +
            "<add key=\"Term\" value=\"2\" />" +
            "<add key=\"AllowDir\" value=\"images|_private\" />" +
            "</website></LearnSite>");
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir))
        {
            Directory.Delete(_tempDir, true);
        }
    }

    [Fact]
    public void FileNameInfo_ParsesVirtualPathCorrectly()
    {
        var info = new FileNameInfo("~/upload/course/test-file.HTML");

        Assert.Equal("~/upload/course/", info.Path);
        Assert.Equal("test-file", info.Fname);
        Assert.Equal("html", info.Ext);
    }

    [Fact]
    public void FileNameInfo_ParsesPhysicalPathCorrectly()
    {
        var info = new FileNameInfo(@"C:\temp\demo\report.PDF");

        Assert.Equal(@"C:\temp\demo\", info.Path);
        Assert.Equal("report", info.Fname);
        Assert.Equal("pdf", info.Ext);
    }

    [Theory]
    [InlineData("abc123_测试", true)]
    [InlineData("abc-123", false)]
    [InlineData("空 格", false)]
    public void WordProcessCore_IsCnEnNum_ReturnsExpectedResult(string value, bool expected)
    {
        Assert.Equal(expected, WordProcessCore.IsCnEnNum(value));
    }

    [Theory]
    [InlineData("12345", true)]
    [InlineData("001", true)]
    [InlineData("12a", false)]
    [InlineData("-10", false)]
    public void WordProcessCore_IsNum_ReturnsExpectedResult(string value, bool expected)
    {
        Assert.Equal(expected, WordProcessCore.IsNum(value));
    }

    [Theory]
    [InlineData("123", true)]
    [InlineData("-123", true)]
    [InlineData("12.5", true)]
    [InlineData("0", true)]
    [InlineData("12a", false)]
    public void WordProcessCore_IsIntNum_ReturnsExpectedResult(string value, bool expected)
    {
        Assert.Equal(expected, WordProcessCore.IsIntNum(value));
    }

    [Theory]
    [InlineData("abcXYZ", true)]
    [InlineData("abc123", false)]
    [InlineData("中文", false)]
    public void WordProcessCore_IsEnglish_ReturnsExpectedResult(string value, bool expected)
    {
        Assert.Equal(expected, WordProcessCore.IsEnglish(value));
    }

    [Theory]
    [InlineData("hello", false)]
    [InlineData("你好", true)]
    [InlineData("abc你好", true)]
    public void WordProcessCore_IsZh_ReturnsExpectedResult(string value, bool expected)
    {
        Assert.Equal(expected, WordProcessCore.IsZh(value));
    }

    [Theory]
    [InlineData("abc123", true)]
    [InlineData("abc", true)]
    [InlineData("123", true)]
    [InlineData("abc_123", false)]
    [InlineData("中文", false)]
    public void WordProcessCore_IsEnNum_ReturnsExpectedResult(string value, bool expected)
    {
        Assert.Equal(expected, WordProcessCore.IsEnNum(value));
    }

    [Theory]
    [InlineData("12345678", true)]
    [InlineData("123456789", false)]
    public void WordProcessCore_StrLength_UsesEightCharacterLimit(string value, bool expected)
    {
        Assert.Equal(expected, WordProcessCore.StrLength(value));
    }

    [Fact]
    public void WordProcessCore_HashAndMd5Helpers_ReturnLowercaseExpectedLengths()
    {
        Assert.Equal(32, WordProcessCore.Hash("abc").Length);
        Assert.Equal(32, WordProcessCore.GetMD5("abc").Length);
        Assert.Equal(8, WordProcessCore.GetMD5_8bit("abc").Length);
        Assert.Equal(16, WordProcessCore.GetMD5_16bit("abc").Length);
        Assert.Equal(10, WordProcessCore.GetMD5_Nbit("abc", 10).Length);

        Assert.Equal(WordProcessCore.GetMD5("abc"), WordProcessCore.GetMD5("abc").ToLowerInvariant());
        Assert.Equal(WordProcessCore.GetMD5_16bit("abc"), WordProcessCore.GetMD5_16bit("abc").ToLowerInvariant());
    }

    [Fact]
    public void WordProcessCore_StrToLower_ReturnsLowercase()
    {
        Assert.Equal("abc123", WordProcessCore.StrToLower("AbC123"));
    }

    [Fact]
    public void WordProcessCore_GenerateRandom_ReturnsRequestedLengthAndAllowedChars()
    {
        var result = WordProcessCore.GenerateRandom(24);

        Assert.Equal(24, result.Length);
        Assert.All(result, c => Assert.True(char.IsDigit(c) || (c >= 'a' && c <= 'z')));
    }

    [Fact]
    public void WordProcessCore_GenerateRandomNum_ReturnsRequestedLengthAndDigitsOnly()
    {
        var result = WordProcessCore.GenerateRandomNum(18);

        Assert.Equal(18, result.Length);
        Assert.All(result, c => Assert.True(char.IsDigit(c)));
        Assert.DoesNotContain('4', result);
    }

    [Fact]
    public void WordProcessCore_GetRandomNum_NonPositiveMaxReturnsZero()
    {
        Assert.Equal(0, WordProcessCore.GetRandomNum(0));
        Assert.Equal(0, WordProcessCore.GetRandomNum(-1));
    }

    [Fact]
    public void WordProcessCore_GetRandomNum_PositiveMaxReturnsInRange()
    {
        var result = WordProcessCore.GetRandomNum(10);
        Assert.InRange(result, 0, 9);
    }

    [Fact]
    public void CustomActivityCatalog_GetMeta_ReturnsConfiguredCustomActivity()
    {
        var meta = CustomActivityCatalog.GetMeta("34");

        Assert.Equal("嵌入本地网页", meta.DisplayName);
        Assert.Equal("iframe", meta.FileType);
        Assert.Equal("iframe-url", meta.ExampleMode);
        Assert.Equal("~/student/iframe.aspx?lid={0}", meta.StudentEntryFormat);
    }

    [Fact]
    public void CustomActivityCatalog_GetMeta_UsesDefaultForUnknownCategory()
    {
        var meta = CustomActivityCatalog.GetMeta("999");

        Assert.Equal("11", meta.Category);
        Assert.Equal("pxl", meta.FileType);
        Assert.Equal("~/student/pixel.aspx?lid={0}", meta.StudentEntryFormat);
    }

    [Fact]
    public void CustomActivityCatalog_GetStudentEntryUrlByLid_FormatsRoute()
    {
        var url = CustomActivityCatalog.GetStudentEntryUrlByLid("24", "321");

        Assert.Equal("~/student/mqtt.aspx?lid=321", url);
    }

    [Fact]
    public void CustomActivityCatalog_BuildExampleValue_ReturnsSelectedDevices()
    {
        var deviceValues = new List<string> { "led", "pump" };

        var result = CustomActivityCatalog.BuildExampleValue("24", deviceValues, String.Empty);

        Assert.True(result.IsValid);
        Assert.Equal("led,pump,", result.ExampleValue);
        Assert.Equal("当前已启用设备：led、pump。", CustomActivityCatalog.GetExampleSummary("24", result.ExampleValue));
    }

    [Fact]
    public void CustomActivityCatalog_BuildExampleValue_RejectsUnsafeIframeUrl()
    {
        var result = CustomActivityCatalog.BuildExampleValue("34", Array.Empty<string>(), "javascript:alert(1)");

        Assert.False(result.IsValid);
        Assert.Equal(String.Empty, result.ExampleValue);
        Assert.Contains("嵌入地址格式不正确", result.ErrorMessage, StringComparison.Ordinal);
    }

    [Fact]
    public void CustomActivityCatalog_BuildExampleValue_AcceptsSafeIframeUrlAndSummary()
    {
        var result = CustomActivityCatalog.BuildExampleValue("34", Array.Empty<string>(), "~/student/demo.aspx");

        Assert.True(result.IsValid);
        Assert.Equal("~/student/demo.aspx", result.ExampleValue);
        Assert.Equal("当前嵌入地址：~/student/demo.aspx。", CustomActivityCatalog.GetExampleSummary("34", result.ExampleValue));
    }

    [Theory]
    [InlineData("https://example.com/tool", true)]
    [InlineData("~/student/demo.aspx", true)]
    [InlineData("../pages/demo.html", true)]
    [InlineData("javascript:alert(1)", false)]
    [InlineData("data:text/html;base64,abc", false)]
    [InlineData("https://bad url.com", false)]
    public void IframeUrlHelper_IsAllowed_ReturnsExpectedResult(string url, bool expected)
    {
        Assert.Equal(expected, IframeUrlHelper.IsAllowed(url));
    }

    [Fact]
    public void WordProcessCore_ServUMd5_ReturnsTwoLetterPrefixPlusMd5()
    {
        var result = WordProcessCore.Serv_u_Md5("secret");

        Assert.Equal(34, result.Length);
        Assert.True(char.IsLower(result[0]) && char.IsLower(result[1]));
        Assert.All(result.Skip(2), c => Assert.True(char.IsDigit(c) || (c >= 'a' && c <= 'f')));
    }

    [Fact]
    public void EncodingType_GetFileEncodeType_DetectsUtf8Bom()
    {
        var file = Path.Combine(_tempDir, "utf8bom.txt");
        File.WriteAllBytes(file, new byte[] { 0xEF, 0xBB, 0xBF, 0x41, 0x42 });

        var encoding = EncodingType.GetFileEncodeType(file);
        Assert.Equal(Encoding.UTF8.WebName, encoding.WebName);
    }

    [Fact]
    public void EncodingType_GetFileEncodeType_DetectsUnicodeBom()
    {
        var file = Path.Combine(_tempDir, "unicode.txt");
        File.WriteAllBytes(file, new byte[] { 0xFF, 0xFE, 0x41, 0x00 });

        var encoding = EncodingType.GetFileEncodeType(file);
        Assert.Equal(Encoding.Unicode.WebName, encoding.WebName);
    }

    [Fact]
    public void EncodingType_GetFileEncodeType_DefaultsWhenNoBom()
    {
        var file = Path.Combine(_tempDir, "ansi.txt");
        File.WriteAllBytes(file, Encoding.ASCII.GetBytes("abc"));

        var encoding = EncodingType.GetFileEncodeType(file);
        Assert.Equal(Encoding.Default.WebName, encoding.WebName);
    }

    [Fact]
    public void EncodingType_GetType_DetectsUtf8Bom()
    {
        var file = Path.Combine(_tempDir, "utf8_gettype.txt");
        File.WriteAllBytes(file, new byte[] { 0xEF, 0xBB, 0xBF, 0x61, 0x62, 0x63 });

        var encoding = EncodingType.GetType(file);
        Assert.Equal(Encoding.UTF8.WebName, encoding.WebName);
    }

    [Fact]
    public void XmlHelpCore_GetValue_ReturnsConfiguredValue()
    {
        Assert.Equal("1", XmlHelpCore.GetValue(_xmlFile, "CookiesFix"));
        Assert.Equal("2", XmlHelpCore.GetValue(_xmlFile, "Term"));
    }

    [Fact]
    public void XmlHelpCore_GetValue_MissingKeyReturnsZeroString()
    {
        Assert.Equal("0", XmlHelpCore.GetValue(_xmlFile, "MissingKey"));
    }

    [Fact]
    public void XmlHelpCore_SetValue_UpdatesExistingKey()
    {
        bool result = XmlHelpCore.SetValue(_xmlFile, "CookiesFix", "99");

        Assert.True(result);
        Assert.Equal("99", XmlHelpCore.GetValue(_xmlFile, "CookiesFix"));
    }

    [Fact]
    public void XmlHelpCore_GetPipeSeparatedValues_ReturnsExpectedSegments()
    {
        var values = XmlHelpCore.GetPipeSeparatedValues(_xmlFile, "AllowDir");

        Assert.Equal(new[] { "images", "_private" }, values);
    }

    [Fact]
    public void XmlHelpCore_GetTrimmedPipeSeparatedList_ReturnsTrimmedCourseTypes()
    {
        var values = XmlHelpCore.GetTrimmedPipeSeparatedList(_xmlFile, "CourseType");

        Assert.Equal(new[] { "Word", "Powerpoint", "Excel" }, values);
    }

    [Fact]
    public void XmlHelpCore_GetOneBasedNumericRangeFromValue_ReturnsExpectedRange()
    {
        var values = XmlHelpCore.GetOneBasedNumericRangeFromValue(_xmlFile, "CoursePeriod");

        Assert.Equal(new[] { 1, 2, 3 }, values);
    }

    [Fact]
    public void XmlHelpCore_GetQuizCourseTypes_PrependsAllDisplayOption()
    {
        var values = XmlHelpCore.GetQuizCourseTypes(_xmlFile);

        Assert.Equal("全部显示", values[0]);
        Assert.Equal(new[] { "Word", "Powerpoint", "Excel" }, values.GetRange(1, 3));
    }

    [Fact]
    public void BllDataTableMappers_MapTyperList_MapsNumericAndTextFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Tid");
        dt.Columns.Add("Ttype");
        dt.Columns.Add("Tuse");
        dt.Columns.Add("Ttitle");
        dt.Columns.Add("Tcontent");
        dt.Rows.Add("5", "2", "9", "Title A", "Content A");

        var result = LearnSite.BLL.BllDataTableMappers.MapTyperList(dt);

        Assert.Single(result);
        Assert.Equal(5, result[0].Tid);
        Assert.Equal(2, result[0].Ttype);
        Assert.Equal(9, result[0].Tuse);
        Assert.Equal("Title A", result[0].Ttitle);
        Assert.Equal("Content A", result[0].Tcontent);
    }

    [Fact]
    public void BllDataTableMappers_MapQuizGradeList_MapsBooleanVariants()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Qid");
        dt.Columns.Add("Qobj");
        dt.Columns.Add("Qclass");
        dt.Columns.Add("Qhid");
        dt.Columns.Add("Qonly");
        dt.Columns.Add("Qmore");
        dt.Columns.Add("Qjudge");
        dt.Columns.Add("Qopen");
        dt.Columns.Add("Qanswer");
        dt.Rows.Add("1", "6", "1,2", "10", "3", "4", "5", "1", "false");

        var result = LearnSite.BLL.BllDataTableMappers.MapQuizGradeList(dt);

        Assert.Single(result);
        Assert.Equal(1, result[0].Qid);
        Assert.Equal(6, result[0].Qobj);
        Assert.Equal("1,2", result[0].Qclass);
        Assert.Equal(10, result[0].Qhid);
        Assert.Equal(3, result[0].Qonly);
        Assert.Equal(4, result[0].Qmore);
        Assert.Equal(5, result[0].Qjudge);
        Assert.True(result[0].Qopen);
        Assert.False(result[0].Qanswer);
    }

    [Fact]
    public void BllDataTableMappers_MapTermTotalList_MapsScoreFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Tid");
        dt.Columns.Add("Tnum");
        dt.Columns.Add("Tterm");
        dt.Columns.Add("Tgrade");
        dt.Columns.Add("Tscore");
        dt.Columns.Add("Tgscore");
        dt.Columns.Add("Tquiz");
        dt.Columns.Add("Tattitude");
        dt.Columns.Add("Twscore");
        dt.Columns.Add("Ttscore");
        dt.Columns.Add("Tpscore");
        dt.Columns.Add("Tallscore");
        dt.Columns.Add("Tape");
        dt.Rows.Add("2", "2024001", "1", "6", "90", "91", "92", "93", "94", "95", "96", "97", "A");

        var result = LearnSite.BLL.BllDataTableMappers.MapTermTotalList(dt);

        Assert.Single(result);
        Assert.Equal(2, result[0].Tid);
        Assert.Equal("2024001", result[0].Tnum);
        Assert.Equal(1, result[0].Tterm);
        Assert.Equal(6, result[0].Tgrade);
        Assert.Equal(90, result[0].Tscore);
        Assert.Equal(91, result[0].Tgscore);
        Assert.Equal(92, result[0].Tquiz);
        Assert.Equal(93, result[0].Tattitude);
        Assert.Equal(94, result[0].Twscore);
        Assert.Equal(95, result[0].Ttscore);
        Assert.Equal(96, result[0].Tpscore);
        Assert.Equal(97, result[0].Tallscore);
        Assert.Equal("A", result[0].Tape);
    }

    [Fact]
    public void BllDataTableMappers_MapAIProviderList_MapsProviderFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Id");
        dt.Columns.Add("DisplayName");
        dt.Columns.Add("ProviderName");
        dt.Columns.Add("ModelName");
        dt.Columns.Add("ApiKey");
        dt.Columns.Add("BaseUrl");
        dt.Columns.Add("IsDefault");
        dt.Rows.Add("7", "OpenAI", "openai", "gpt-4o", "secret", "https://api.test", "true");

        var result = LearnSite.BLL.BllDataTableMappers.MapAIProviderList(dt);

        Assert.Single(result);
        Assert.Equal(7, result[0].Id);
        Assert.Equal("OpenAI", result[0].DisplayName);
        Assert.Equal("openai", result[0].ProviderName);
        Assert.Equal("gpt-4o", result[0].ModelName);
        Assert.Equal("secret", result[0].ApiKey);
        Assert.Equal("https://api.test", result[0].BaseUrl);
        Assert.True(result[0].IsDefault);
    }

    [Fact]
    public void BllDataTableMappers_MapAISkillList_MapsSkillFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Id");
        dt.Columns.Add("SkillName");
        dt.Columns.Add("PromptContent");
        dt.Columns.Add("IsActive");
        dt.Rows.Add("3", "summary", "summarize content", "1");

        var result = LearnSite.BLL.BllDataTableMappers.MapAISkillList(dt);

        Assert.Single(result);
        Assert.Equal(3, result[0].Id);
        Assert.Equal("summary", result[0].SkillName);
        Assert.Equal("summarize content", result[0].PromptContent);
        Assert.True(result[0].IsActive);
    }

    [Fact]
    public void BllDataTableMappers_MapTurtleMatchList_MapsDatesAndBoolean()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Mid");
        dt.Columns.Add("Mhid");
        dt.Columns.Add("Mtitle");
        dt.Columns.Add("Mcontent");
        dt.Columns.Add("Mbegin");
        dt.Columns.Add("Mend");
        dt.Columns.Add("Mpublish");
        dt.Columns.Add("Mdate");
        dt.Rows.Add("2", "8", "Match", "Content", "2024-01-01", "2024-01-31", "false", "2024-02-01");

        var result = LearnSite.BLL.BllDataTableMappers.MapTurtleMatchList(dt);

        Assert.Single(result);
        Assert.Equal(2, result[0].Mid);
        Assert.Equal(8, result[0].Mhid);
        Assert.Equal("Match", result[0].Mtitle);
        Assert.Equal("Content", result[0].Mcontent);
        Assert.Equal(new DateTime(2024, 1, 1), result[0].Mbegin);
        Assert.Equal(new DateTime(2024, 1, 31), result[0].Mend);
        Assert.False(result[0].Mpublish);
        Assert.Equal(new DateTime(2024, 2, 1), result[0].Mdate);
    }

    [Fact]
    public void BllDataTableMappers_MapCoursesList_MapsCoreCourseFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Cid");
        dt.Columns.Add("Ctitle");
        dt.Columns.Add("Cclass");
        dt.Columns.Add("Ccontent");
        dt.Columns.Add("Cdate");
        dt.Columns.Add("Chit");
        dt.Columns.Add("Cobj");
        dt.Columns.Add("Cterm");
        dt.Columns.Add("Cks");
        dt.Columns.Add("Cfiletype");
        dt.Columns.Add("Cupload");
        dt.Columns.Add("Chid");
        dt.Columns.Add("Cpublish");
        dt.Rows.Add("11", "Course A", "1,2", "Body", "2024-03-01", "5", "6", "1", "2", "html", "1", "9", "true");

        var result = LearnSite.BLL.BllDataTableMappers.MapCoursesList(dt);

        Assert.Single(result);
        Assert.Equal(11, result[0].Cid);
        Assert.Equal("Course A", result[0].Ctitle);
        Assert.Equal("1,2", result[0].Cclass);
        Assert.Equal("Body", result[0].Ccontent);
        Assert.Equal(new DateTime(2024, 3, 1), result[0].Cdate);
        Assert.Equal(5, result[0].Chit);
        Assert.Equal(6, result[0].Cobj);
        Assert.Equal(1, result[0].Cterm);
        Assert.Equal(2, result[0].Cks);
        Assert.Equal("html", result[0].Cfiletype);
        Assert.True(result[0].Cupload);
        Assert.Equal(9, result[0].Chid);
        Assert.True(result[0].Cpublish);
    }

    [Fact]
    public void BllDataTableMappers_MapQuizList_MapsQuizFieldsAndBooleanVariants()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Qid");
        dt.Columns.Add("Qtype");
        dt.Columns.Add("Question");
        dt.Columns.Add("Qanswer");
        dt.Columns.Add("Qanalyze");
        dt.Columns.Add("Qscore");
        dt.Columns.Add("Qclass");
        dt.Columns.Add("Qselect");
        dt.Columns.Add("Qright");
        dt.Columns.Add("Qwrong");
        dt.Columns.Add("Qaccuracy");
        dt.Rows.Add("12", "2", "What is 2+2?", "4", "basic math", "5", "6,1", "false", "10", "1", "90");

        var result = LearnSite.BLL.BllDataTableMappers.MapQuizList(dt);

        Assert.Single(result);
        Assert.Equal(12, result[0].Qid);
        Assert.Equal(2, result[0].Qtype);
        Assert.Equal("What is 2+2?", result[0].Question);
        Assert.Equal("4", result[0].Qanswer);
        Assert.Equal("basic math", result[0].Qanalyze);
        Assert.Equal(5, result[0].Qscore);
        Assert.Equal("6,1", result[0].Qclass);
        Assert.False(result[0].Qselect);
        Assert.Equal(10, result[0].Qright);
        Assert.Equal(1, result[0].Qwrong);
        Assert.Equal(90, result[0].Qaccuracy);
    }

    [Fact]
    public void BllDataTableMappers_MapSigninList_MapsSigninFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Qid");
        dt.Columns.Add("Qnum");
        dt.Columns.Add("Qattitude");
        dt.Columns.Add("Qdate");
        dt.Columns.Add("Qyear");
        dt.Columns.Add("Qmonth");
        dt.Columns.Add("Qday");
        dt.Columns.Add("Qweek");
        dt.Columns.Add("Qip");
        dt.Columns.Add("Qmachine");
        dt.Columns.Add("Qnote");
        dt.Columns.Add("Qwork");
        dt.Columns.Add("Qgrade");
        dt.Columns.Add("Qterm");
        dt.Rows.Add("18", "2024002", "4", "2024-04-05", "2024", "4", "5", "Fri", "10.0.0.8", "PC-01", "good", "2", "6", "1");

        var result = LearnSite.BLL.BllDataTableMappers.MapSigninList(dt);

        Assert.Single(result);
        Assert.Equal(18, result[0].Qid);
        Assert.Equal("2024002", result[0].Qnum);
        Assert.Equal(4, result[0].Qattitude);
        Assert.Equal(new DateTime(2024, 4, 5), result[0].Qdate);
        Assert.Equal(2024, result[0].Qyear);
        Assert.Equal(4, result[0].Qmonth);
        Assert.Equal(5, result[0].Qday);
        Assert.Equal("Fri", result[0].Qweek);
        Assert.Equal("10.0.0.8", result[0].Qip);
        Assert.Equal("PC-01", result[0].Qmachine);
        Assert.Equal("good", result[0].Qnote);
        Assert.Equal(2, result[0].Qwork);
        Assert.Equal(6, result[0].Qgrade);
        Assert.Equal(1, result[0].Qterm);
    }

    [Fact]
    public void BllDataTableMappers_MapAICustomSkillList_MapsCustomSkillFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Id");
        dt.Columns.Add("SkillName");
        dt.Columns.Add("PromptContent");
        dt.Columns.Add("SkillScope");
        dt.Columns.Add("IsActive");
        dt.Rows.Add("4", "console-helper", "help with console tasks", "chat,console", "1");

        var result = LearnSite.BLL.BllDataTableMappers.MapAICustomSkillList(dt);

        Assert.Single(result);
        Assert.Equal(4, result[0].Id);
        Assert.Equal("console-helper", result[0].SkillName);
        Assert.Equal("help with console tasks", result[0].PromptContent);
        Assert.Equal("chat,console", result[0].SkillScope);
        Assert.True(result[0].IsActive);
    }

    [Fact]
    public void AIGaugeSkillHelper_ParseGaugeItems_ParsesJsonCodeBlockAndNormalizesScores()
    {
        string content = "```json\n[{\"item\":\"创意表达\",\"score\":30},{\"item\":\"功能实现\",\"score\":30},{\"item\":\"界面美观\",\"score\":20},{\"item\":\"表达规范\",\"score\":20}]\n```";

        var result = LearnSite.Common.AIGaugeSkillHelper.ParseGaugeItems(content);

        Assert.Equal(4, result.Count);
        Assert.Equal("创意表达", result[0].Mitem);
        Assert.Equal(30, result[0].Mscore);
        Assert.Equal(1, result[0].Msort);
        Assert.Equal(100, result.Sum(x => x.Mscore ?? 0));
    }

    [Fact]
    public void AIGaugeSkillHelper_ParseGaugeItems_RebalancesInvalidScores()
    {
        string content = "1. 创意设计（10分）\n2. 功能完成\n3. 作品美观（20分）";

        var result = LearnSite.Common.AIGaugeSkillHelper.ParseGaugeItems(content);

        Assert.Equal(3, result.Count);
        Assert.Equal(100, result.Sum(x => x.Mscore ?? 0));
        Assert.All(result, item => Assert.True((item.Mscore ?? 0) > 0));
        Assert.True(new[] { 1, 2, 3 }.SequenceEqual(result.Select(x => x.Msort ?? 0)));
    }

    [Fact]
    public void ActivityPlanPromptBuilder_RequiresTopicAndUsesTopicAsPrimaryIntent()
    {
        Assert.Throws<ArgumentException>(() => LearnSite.Common.AIActivityPlanPromptBuilder.Build(new LearnSite.Common.AIActivityPlanPromptRequest
        {
            Topic = "   "
        }));

        var prompt = LearnSite.Common.AIActivityPlanPromptBuilder.Build(new LearnSite.Common.AIActivityPlanPromptRequest
        {
            Topic = "认识分数"
        });

        Assert.Contains("主题/知识点", prompt, StringComparison.Ordinal);
        Assert.Contains("认识分数", prompt, StringComparison.Ordinal);
        Assert.Contains("只返回 JSON 对象", prompt, StringComparison.Ordinal);
    }

    [Fact]
    public void ActivityPlanPromptBuilder_IncludesOptionalStructuredFieldsOnlyWhenProvided()
    {
        var prompt = LearnSite.Common.AIActivityPlanPromptBuilder.Build(new LearnSite.Common.AIActivityPlanPromptRequest
        {
            Topic = "认识分数",
            Grade = "五年级",
            Duration = "40分钟",
            TeachingGoals = "理解真分数与假分数"
        });

        Assert.Contains("授课年级：五年级", prompt, StringComparison.Ordinal);
        Assert.Contains("课时/时长：40分钟", prompt, StringComparison.Ordinal);
        Assert.Contains("教学目标：理解真分数与假分数", prompt, StringComparison.Ordinal);

        var minimalPrompt = LearnSite.Common.AIActivityPlanPromptBuilder.Build(new LearnSite.Common.AIActivityPlanPromptRequest
        {
            Topic = "认识分数"
        });

        Assert.DoesNotContain("授课年级：", minimalPrompt, StringComparison.Ordinal);
        Assert.DoesNotContain("课时/时长：", minimalPrompt, StringComparison.Ordinal);
        Assert.DoesNotContain("教学目标：", minimalPrompt, StringComparison.Ordinal);
    }

    [Fact]
    public void ActivityPlanPromptBuilder_TreatsExistingContentAsSupportingBackground()
    {
        var prompt = LearnSite.Common.AIActivityPlanPromptBuilder.Build(new LearnSite.Common.AIActivityPlanPromptRequest
        {
            Topic = "认识分数",
            ExistingCourseContent = "旧内容：小数加减法"
        });

        Assert.Contains("支持背景", prompt, StringComparison.Ordinal);
        Assert.Contains("如果与当前输入的主题冲突，以教师刚输入的主题为准", prompt, StringComparison.Ordinal);
        Assert.Contains("旧内容：小数加减法", prompt, StringComparison.Ordinal);
    }

    [Fact]
    public void ActivityPlanPromptBuilder_DeclaresStructuredDraftSchema()
    {
        var prompt = LearnSite.Common.AIActivityPlanPromptBuilder.Build(new LearnSite.Common.AIActivityPlanPromptRequest
        {
            Topic = "认识分数"
        });

        Assert.Contains("只返回 JSON 对象", prompt, StringComparison.Ordinal);
        Assert.Contains("teachingGoals", prompt, StringComparison.Ordinal);
        Assert.Contains("activitySteps", prompt, StringComparison.Ordinal);
        Assert.Contains("teacherReminder", prompt, StringComparison.Ordinal);
        Assert.Contains("assessmentCheck", prompt, StringComparison.Ordinal);
    }

    [Fact]
    public void ActivityPlanSkillBootstrap_UsesDedicatedActivityPlanScopeAndPrompt()
    {
        var model = LearnSite.BLL.AIActivityPlanSkillBootstrap.CreateDefaultSkillModel();

        Assert.Equal("activity_plan_courseedit", model.SkillScope);
        Assert.True(model.IsActive);
        Assert.Contains("活动计划", model.SkillName, StringComparison.Ordinal);
        Assert.Contains("活动计划", model.PromptContent, StringComparison.Ordinal);
        Assert.DoesNotContain("写作", model.PromptContent, StringComparison.Ordinal);
    }

    [Fact]
    public void ActivityPlanPromptBuilder_BoundsOversizedSupportingFields()
    {
        var prompt = LearnSite.Common.AIActivityPlanPromptBuilder.Build(new LearnSite.Common.AIActivityPlanPromptRequest
        {
            Topic = new string('题', LearnSite.Common.AIActivityPlanPromptBuilder.MaxTopicLength + 20),
            Grade = new string('年', LearnSite.Common.AIActivityPlanPromptBuilder.MaxGradeLength + 20),
            Duration = new string('时', LearnSite.Common.AIActivityPlanPromptBuilder.MaxDurationLength + 20),
            TeachingGoals = new string('目', LearnSite.Common.AIActivityPlanPromptBuilder.MaxTeachingGoalsLength + 50),
            ExistingCourseContent = new string('内', 4500)
        });

        Assert.DoesNotContain(new string('题', LearnSite.Common.AIActivityPlanPromptBuilder.MaxTopicLength + 1), prompt, StringComparison.Ordinal);
        Assert.DoesNotContain(new string('年', LearnSite.Common.AIActivityPlanPromptBuilder.MaxGradeLength + 1), prompt, StringComparison.Ordinal);
        Assert.DoesNotContain(new string('时', LearnSite.Common.AIActivityPlanPromptBuilder.MaxDurationLength + 1), prompt, StringComparison.Ordinal);
        Assert.DoesNotContain(new string('目', LearnSite.Common.AIActivityPlanPromptBuilder.MaxTeachingGoalsLength + 1), prompt, StringComparison.Ordinal);
        Assert.DoesNotContain(new string('内', 4001), prompt, StringComparison.Ordinal);
    }

    [Fact]
    public void ActivityPlanDraftHelper_ParseDraft_ParsesStructuredJsonCodeFence()
    {
        string response = "```json\n{\n  \"teachingGoals\": [\"理解分数含义\", \"能结合情境表达分数\"],\n  \"activitySteps\": [\n    {\n      \"title\": \"情境导入\",\n      \"minutes\": 5,\n      \"teacherAction\": \"展示分蛋糕图片并提问\",\n      \"studentAction\": \"观察图片并回答\",\n      \"interactionMethod\": \"提问交流\",\n      \"resourceSuggestion\": \"蛋糕图片或实物卡片\",\n      \"assessmentCheck\": \"根据学生表述判断是否理解平均分\"\n    }\n  ],\n  \"resources\": [\"分数卡片\"],\n  \"assessment\": [\"观察学生是否能正确说出二分之一\"],\n  \"teacherReminder\": \"注意让学生先说生活例子。\"\n}\n```";

        var draft = LearnSite.Common.AIActivityPlanDraftHelper.ParseDraft(response);

        Assert.NotNull(draft);
        Assert.True(LearnSite.Common.AIActivityPlanDraftHelper.IsValidDraft(draft));
        Assert.Equal("5分钟", draft.ActivitySteps[0].Minutes);
        Assert.Equal("情境导入", draft.ActivitySteps[0].Title);
    }

    [Fact]
    public void ActivityPlanDraftHelper_ParseDraft_NormalizesCommonAlternateFieldNames()
    {
        string response = "{\n  \"goals\": \"1. 理解分数含义\\n2. 能说出分数\",\n  \"steps\": [\n    {\n      \"name\": \"合作探究\",\n      \"duration\": \"15分钟\",\n      \"teacher\": \"组织小组操作\",\n      \"student\": \"动手分一分并记录\",\n      \"interaction\": \"小组合作\",\n      \"resource\": \"纸条和圆片\",\n      \"check\": \"巡视学生是否会用分数表示结果\"\n    }\n  ],\n  \"resourceSuggestions\": \"1. 分数圆片；2. 投影示例\",\n  \"assessmentDesign\": \"1. 口头追问；2. 板演展示\",\n  \"teacherTip\": \"关注表述不完整的学生。\"\n}";

        var draft = LearnSite.Common.AIActivityPlanDraftHelper.ParseDraft(response);

        Assert.NotNull(draft);
        Assert.Equal(2, draft.TeachingGoals.Count);
        Assert.Single(draft.ActivitySteps);
        Assert.Equal("合作探究", draft.ActivitySteps[0].Title);
        Assert.Contains("分数圆片", draft.Resources[0], StringComparison.Ordinal);
        Assert.Contains("口头追问", draft.Assessment[0], StringComparison.Ordinal);
    }

    [Fact]
    public void ActivityPlanDraftHelper_ParseDraft_FailsWhenAnyReturnedStepRemainsInvalid()
    {
        string response = "{\n  \"teachingGoals\": [\"理解分数含义\"],\n  \"activitySteps\": [\n    {\n      \"title\": \"情境导入\",\n      \"minutes\": 5,\n      \"teacherAction\": \"展示分蛋糕图片并提问\",\n      \"studentAction\": \"观察图片并回答\",\n      \"interactionMethod\": \"提问交流\",\n      \"resourceSuggestion\": \"蛋糕图片或实物卡片\",\n      \"assessmentCheck\": \"根据学生表述判断是否理解平均分\"\n    },\n    {\n      \"title\": \"错误步骤\",\n      \"minutes\": 8,\n      \"teacherAction\": \"只给出教师动作\"\n    }\n  ],\n  \"resources\": [\"分数卡片\"],\n  \"assessment\": [\"观察学生是否能正确说出二分之一\"],\n  \"teacherReminder\": \"注意让学生先说生活例子。\"\n}";

        var draft = LearnSite.Common.AIActivityPlanDraftHelper.ParseDraft(response);

        Assert.Null(draft);
    }

    [Fact]
    public void ActivityPlanDraftHelper_OnlyAllowsSupportedSectionTargets()
    {
        Assert.True(LearnSite.Common.AIActivityPlanDraftHelper.IsSupportedSectionTarget("teachingGoals"));
        Assert.True(LearnSite.Common.AIActivityPlanDraftHelper.IsSupportedSectionTarget("activitySteps"));
        Assert.True(LearnSite.Common.AIActivityPlanDraftHelper.IsSupportedSectionTarget("resources"));
        Assert.True(LearnSite.Common.AIActivityPlanDraftHelper.IsSupportedSectionTarget("assessment"));
        Assert.True(LearnSite.Common.AIActivityPlanDraftHelper.IsSupportedSectionTarget("teacherReminder"));
        Assert.False(LearnSite.Common.AIActivityPlanDraftHelper.IsSupportedSectionTarget("fullDraft"));
        Assert.False(LearnSite.Common.AIActivityPlanDraftHelper.IsSupportedSectionTarget("activitySteps[0]"));
    }

    [Fact]
    public void ActivityPlanPromptBuilder_BuildSectionRegeneration_RestrictsPromptToOneTopLevelKey()
    {
        var prompt = LearnSite.Common.AIActivityPlanPromptBuilder.BuildSectionRegeneration(new LearnSite.Common.AIActivityPlanSectionRegenerationRequest
        {
            Topic = "认识分数",
            SectionTarget = "activitySteps",
            CurrentDraft = BuildValidActivityPlanDraft()
        });

        Assert.Contains("本次只允许重写的顶层字段：activitySteps", prompt, StringComparison.Ordinal);
        Assert.Contains("返回 JSON 时只能包含一个顶层字段，且该字段名必须是 activitySteps", prompt, StringComparison.Ordinal);
        Assert.Contains("不要返回完整草案", prompt, StringComparison.Ordinal);
    }

    [Fact]
    public void ActivityPlanDraftHelper_MergeRegeneratedSection_ReplacesOnlyRequestedSection()
    {
        var currentDraft = BuildValidActivityPlanDraft();
        string response = "{\"resources\":[\"分数圆片\",\"投影课件\"]}";

        var mergedDraft = LearnSite.Common.AIActivityPlanDraftHelper.MergeRegeneratedSection(currentDraft, "resources", response);

        Assert.NotNull(mergedDraft);
        Assert.Equal(new[] { "分数圆片", "投影课件" }, mergedDraft.Resources);
        Assert.Equal(currentDraft.TeacherReminder, mergedDraft.TeacherReminder);
        Assert.Equal(currentDraft.TeachingGoals, mergedDraft.TeachingGoals);
        Assert.Single(mergedDraft.ActivitySteps);
    }

    [Fact]
    public void ActivityPlanDraftHelper_MergeRegeneratedSection_RejectsWrongKeyOrIncompleteContent()
    {
        var currentDraft = BuildValidActivityPlanDraft();

        Assert.Null(LearnSite.Common.AIActivityPlanDraftHelper.MergeRegeneratedSection(currentDraft, "resources", "{\"assessment\":[\"口头追问\"]}"));
        Assert.Null(LearnSite.Common.AIActivityPlanDraftHelper.MergeRegeneratedSection(currentDraft, "teacherReminder", "{\"teacherReminder\":\"   \"}"));
        Assert.Null(LearnSite.Common.AIActivityPlanDraftHelper.MergeRegeneratedSection(currentDraft, "activitySteps", "{\"activitySteps\":[{\"title\":\"练习\",\"minutes\":\"10分钟\",\"teacherAction\":\"组织练习\"}]}"));
    }

    [Fact]
    public void ActivityPlanSkillBootstrap_SelectsScopedSkillToActivate_WhenOnlyInactiveScopedRowsExist()
    {
        var skills = new List<LearnSite.Model.AICustomSkill>
        {
            new LearnSite.Model.AICustomSkill
            {
                Id = 2,
                SkillName = "其他技能",
                SkillScope = LearnSite.BLL.AIActivityPlanSkillBootstrap.ActivityPlanSkillScope,
                IsActive = false
            },
            new LearnSite.Model.AICustomSkill
            {
                Id = 3,
                SkillName = LearnSite.BLL.AIActivityPlanSkillBootstrap.CreateDefaultSkillModel().SkillName,
                SkillScope = LearnSite.BLL.AIActivityPlanSkillBootstrap.ActivityPlanSkillScope,
                IsActive = false
            }
        };

        var skillToActivate = LearnSite.BLL.AIActivityPlanSkillBootstrap.GetScopedSkillToActivate(skills);

        Assert.NotNull(skillToActivate);
        Assert.Equal(3, skillToActivate.Id);
        Assert.False(LearnSite.BLL.AIActivityPlanSkillBootstrap.HasAnyActiveScopedSkill(skills));
    }

    [Fact]
    public void ActivityPlanSavedDraftHelper_BuildRecord_PersistsRequestContextAndDraftJson()
    {
        var draft = BuildValidActivityPlanDraft();

        var record = LearnSite.Common.AIActivityPlanSavedDraftHelper.BuildRecord(12, 5, "认识分数", "五年级", "40分钟", "理解分数含义", "旧内容", draft);

        Assert.NotNull(record);
        Assert.Equal(12, record.Cid);
        Assert.Equal(5, record.Hid);
        Assert.Equal("认识分数", record.Topic);
        Assert.Equal("五年级", record.Grade);
        Assert.Equal("40分钟", record.Duration);
        Assert.Contains("teachingGoals", record.DraftJson, StringComparison.Ordinal);

        var loaded = LearnSite.Common.AIActivityPlanSavedDraftHelper.ParseRecord(record, 12, 5);

        Assert.NotNull(loaded);
        Assert.Equal("理解分数含义", loaded.TeachingGoals);
        Assert.Equal("旧内容", loaded.ExistingCourseContent);
        Assert.Equal(draft.TeacherReminder, loaded.Draft.TeacherReminder);
    }

    [Fact]
    public void ActivityPlanSavedDraftHelper_ParseRecord_RevalidatesDraftJsonAndFailsClosed()
    {
        var record = new LearnSite.Model.CourseActivityPlanDraft
        {
            Cid = 12,
            Hid = 5,
            Topic = "认识分数",
            DraftJson = "{\"teachingGoals\":[]}",
            UpdatedAt = new DateTime(2026, 4, 10, 8, 30, 0)
        };

        Assert.Null(LearnSite.Common.AIActivityPlanSavedDraftHelper.ParseRecord(record, 12, 5));
        Assert.Null(LearnSite.Common.AIActivityPlanSavedDraftHelper.ParseRecord(record, 99, 5));
    }

    [Fact]
    public void ActivityPlanSavedDraftHelper_BuildRecord_RejectsMissingTopicOrCourseIdentifiers()
    {
        var draft = BuildValidActivityPlanDraft();

        Assert.Null(LearnSite.Common.AIActivityPlanSavedDraftHelper.BuildRecord(0, 5, "认识分数", "", "", "", "", draft));
        Assert.Null(LearnSite.Common.AIActivityPlanSavedDraftHelper.BuildRecord(12, 0, "认识分数", "", "", "", "", draft));
        Assert.Null(LearnSite.Common.AIActivityPlanSavedDraftHelper.BuildRecord(12, 5, "   ", "", "", "", "", draft));
        Assert.Null(LearnSite.Common.AIActivityPlanSavedDraftHelper.BuildRecord(12, 5, "认识分数", "", "", "", "", null));
    }

    [Fact]
    public void CourseActivityPlanDraftDal_BuildUpsertSql_UsesSingleCurrentDraftPerCourse()
    {
        var sql = LearnSite.DAL.CourseActivityPlanDraft.BuildUpsertSql();

        Assert.Contains("if exists", sql, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("where Cid=@Cid", sql, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("update CourseActivityPlanDraft", sql, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("insert into CourseActivityPlanDraft", sql, StringComparison.OrdinalIgnoreCase);
    }

    private static LearnSite.Common.ActivityPlanDraft BuildValidActivityPlanDraft()
    {
        return new LearnSite.Common.ActivityPlanDraft
        {
            TeachingGoals = new List<string> { "理解分数含义", "能结合情境表达分数" },
            ActivitySteps = new List<LearnSite.Common.ActivityPlanDraftStep>
            {
                new LearnSite.Common.ActivityPlanDraftStep
                {
                    Sort = 1,
                    Title = "情境导入",
                    Minutes = "5分钟",
                    TeacherAction = "展示分蛋糕图片并提问",
                    StudentAction = "观察图片并回答",
                    InteractionMethod = "提问交流",
                    ResourceSuggestion = "蛋糕图片或实物卡片",
                    AssessmentCheck = "根据学生表述判断是否理解平均分"
                }
            },
            Resources = new List<string> { "分数卡片" },
            Assessment = new List<string> { "观察学生是否能正确说出二分之一" },
            TeacherReminder = "注意让学生先说生活例子。"
        };
    }

    [Fact]
    public void BllDataTableMappers_MapSoftCategoryList_MapsCategoryFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Yid");
        dt.Columns.Add("Ysort");
        dt.Columns.Add("Ytitle");
        dt.Columns.Add("Ycontent");
        dt.Columns.Add("Yopen");
        dt.Rows.Add("6", "3", "Graphics", "Drawing tools", "true");

        var result = LearnSite.BLL.BllDataTableMappers.MapSoftCategoryList(dt);

        Assert.Single(result);
        Assert.Equal(6, result[0].Yid);
        Assert.Equal(3, result[0].Ysort);
        Assert.Equal("Graphics", result[0].Ytitle);
        Assert.Equal("Drawing tools", result[0].Ycontent);
        Assert.True(result[0].Yopen);
    }

    [Fact]
    public void BllDataTableMappers_MapWorksList_MapsCoreWorkFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Wid");
        dt.Columns.Add("Wnum");
        dt.Columns.Add("Wcid");
        dt.Columns.Add("Wmid");
        dt.Columns.Add("Wmsort");
        dt.Columns.Add("Wfilename");
        dt.Columns.Add("Wurl");
        dt.Columns.Add("Wlength");
        dt.Columns.Add("Wscore");
        dt.Columns.Add("Wdate");
        dt.Columns.Add("Wip");
        dt.Columns.Add("Wtime");
        dt.Columns.Add("Wvote");
        dt.Columns.Add("Wegg");
        dt.Columns.Add("Wcheck");
        dt.Columns.Add("Wself");
        dt.Columns.Add("Wcan");
        dt.Columns.Add("Wgood");
        dt.Columns.Add("Wtype");
        dt.Columns.Add("Wgrade");
        dt.Columns.Add("Wterm");
        dt.Columns.Add("Whit");
        dt.Columns.Add("Wlscore");
        dt.Columns.Add("Wlemotion");
        dt.Columns.Add("Woffice");
        dt.Columns.Add("Wflash");
        dt.Columns.Add("Werror");
        dt.Columns.Add("Wfscore");
        dt.Rows.Add("21", "2024003", "9", "10", "2", "demo.docx", "/upload/demo.docx", "2048", "10", "2024-04-06", "10.0.0.9", "08:30", "3", "1", "true", "self note", "1", "false", "docx", "6", "2", "15", "8", "5", "false", "1", "false", "7");

        var result = LearnSite.BLL.BllDataTableMappers.MapWorksList(dt);

        Assert.Single(result);
        Assert.Equal(21, result[0].Wid);
        Assert.Equal("2024003", result[0].Wnum);
        Assert.Equal(9, result[0].Wcid);
        Assert.Equal(10, result[0].Wmid);
        Assert.Equal(2, result[0].Wmsort);
        Assert.Equal("demo.docx", result[0].Wfilename);
        Assert.Equal("/upload/demo.docx", result[0].Wurl);
        Assert.Equal(2048, result[0].Wlength);
        Assert.Equal(10, result[0].Wscore);
        Assert.Equal(new DateTime(2024, 4, 6), result[0].Wdate);
        Assert.Equal("10.0.0.9", result[0].Wip);
        Assert.Equal("08:30", result[0].Wtime);
        Assert.Equal(3, result[0].Wvote);
        Assert.Equal(1, result[0].Wegg);
        Assert.True(result[0].Wcheck);
        Assert.Equal("self note", result[0].Wself);
        Assert.True(result[0].Wcan);
        Assert.False(result[0].Wgood);
        Assert.Equal("docx", result[0].Wtype);
        Assert.Equal(6, result[0].Wgrade);
        Assert.Equal(2, result[0].Wterm);
        Assert.Equal(15, result[0].Whit);
        Assert.Equal(8, result[0].Wlscore);
        Assert.Equal(5, result[0].Wlemotion);
        Assert.False(result[0].Woffice);
        Assert.False(result[0].Wflash);
        Assert.Equal(7, result[0].Wfscore);
    }

    [Fact]
    public void BllDataTableMappers_MapSolvesList_MapsAnswerAndScoreFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Vid");
        dt.Columns.Add("Vpid");
        dt.Columns.Add("Vsid");
        dt.Columns.Add("Vanswer");
        dt.Columns.Add("Vright");
        dt.Columns.Add("Vscore");
        dt.Columns.Add("Vdate");
        dt.Rows.Add("31", "8", "15", "A", "false", "88", "2024-04-07");

        var result = LearnSite.BLL.BllDataTableMappers.MapSolvesList(dt);

        Assert.Single(result);
        Assert.Equal(31, result[0].Vid);
        Assert.Equal(8, result[0].Vpid);
        Assert.Equal(15, result[0].Vsid);
        Assert.Equal("A", result[0].Vanswer);
        Assert.False(result[0].Vright);
        Assert.Equal(88, result[0].Vscore);
        Assert.Equal(new DateTime(2024, 4, 7), result[0].Vdate);
    }

    [Fact]
    public void BllDataTableMappers_MapTxtFormList_MapsFormFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Mid");
        dt.Columns.Add("Mtitle");
        dt.Columns.Add("Mcid");
        dt.Columns.Add("Mcontent");
        dt.Columns.Add("Mdate");
        dt.Columns.Add("Mhit");
        dt.Columns.Add("Mpublish");
        dt.Columns.Add("Mdelete");
        dt.Columns.Add("Mcollabo");
        dt.Rows.Add("41", "Survey Form", "12", "<p>body</p>", "2024-04-08", "6", "1", "false", "true");

        var result = LearnSite.BLL.BllDataTableMappers.MapTxtFormList(dt);

        Assert.Single(result);
        Assert.Equal(41, result[0].Mid);
        Assert.Equal("Survey Form", result[0].Mtitle);
        Assert.Equal(12, result[0].Mcid);
        Assert.Equal("<p>body</p>", result[0].Mcontent);
        Assert.Equal(new DateTime(2024, 4, 8), result[0].Mdate);
        Assert.Equal(6, result[0].Mhit);
        Assert.True(result[0].Mpublish);
        Assert.False(result[0].Mdelete);
        Assert.True(result[0].Mcollabo);
    }

    [Fact]
    public void BllDataTableMappers_MapMenuWorksList_MapsMenuWorkFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Kid");
        dt.Columns.Add("Ksid");
        dt.Columns.Add("Klid");
        dt.Columns.Add("Ktime");
        dt.Columns.Add("Kseconds");
        dt.Columns.Add("Kcheck");
        dt.Columns.Add("Kstar");
        dt.Rows.Add("51", "1001", "88", "12", "720", "true", "4");

        var result = LearnSite.BLL.BllDataTableMappers.MapMenuWorksList(dt);

        Assert.Single(result);
        Assert.Equal(51, result[0].Kid);
        Assert.Equal(1001, result[0].Ksid);
        Assert.Equal(88, result[0].Klid);
        Assert.Equal(12, result[0].Ktime);
        Assert.Equal(720, result[0].Kseconds);
        Assert.True(result[0].Kcheck);
        Assert.Equal(4, result[0].Kstar);
    }

    [Fact]
    public void BllDataTableMappers_MapTxtFormBackList_MapsBackFormFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Rid");
        dt.Columns.Add("Rmid");
        dt.Columns.Add("Rsnum");
        dt.Columns.Add("Rsid");
        dt.Columns.Add("Rwords");
        dt.Columns.Add("Rtime");
        dt.Columns.Add("Rip");
        dt.Columns.Add("Rscore");
        dt.Columns.Add("Ryear");
        dt.Columns.Add("Rterm");
        dt.Columns.Add("Rgrade");
        dt.Columns.Add("Rclass");
        dt.Columns.Add("Ragree");
        dt.Columns.Add("Rlid");
        dt.Columns.Add("Rcontent");
        dt.Rows.Add("61", "41", "2024005", "3001", "short words", "2024-04-09", "10.0.0.10", "95", "2024", "2", "6", "3", "7", "90", "long content");

        var result = LearnSite.BLL.BllDataTableMappers.MapTxtFormBackList(dt);

        Assert.Single(result);
        Assert.Equal(61, result[0].Rid);
        Assert.Equal(41, result[0].Rmid);
        Assert.Equal("2024005", result[0].Rsnum);
        Assert.Equal(3001, result[0].Rsid);
        Assert.Equal("short words", result[0].Rwords);
        Assert.Equal(new DateTime(2024, 4, 9), result[0].Rtime);
        Assert.Equal("10.0.0.10", result[0].Rip);
        Assert.Equal(95, result[0].Rscore);
        Assert.Equal(2024, result[0].Ryear);
        Assert.Equal(2, result[0].Rterm);
        Assert.Equal(6, result[0].Rgrade);
        Assert.Equal(3, result[0].Rclass);
        Assert.Equal(7, result[0].Ragree);
        Assert.Equal(90, result[0].Rlid);
        Assert.Equal("long content", result[0].Rcontent);
    }

    [Fact]
    public void BllDataTableMappers_MapSurveyList_MapsSurveyFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Vid");
        dt.Columns.Add("Vcid");
        dt.Columns.Add("Vhid");
        dt.Columns.Add("Vtitle");
        dt.Columns.Add("Vcontent");
        dt.Columns.Add("Vtype");
        dt.Columns.Add("Vtotal");
        dt.Columns.Add("Vscore");
        dt.Columns.Add("Vaverage");
        dt.Columns.Add("Vclose");
        dt.Columns.Add("Vpoint");
        dt.Columns.Add("Vdate");
        dt.Rows.Add("71", "12", "5", "Survey A", "content", "2", "10", "100", "86", "false", "1", "2024-04-10");

        var result = LearnSite.BLL.BllDataTableMappers.MapSurveyList(dt);

        Assert.Single(result);
        Assert.Equal(71, result[0].Vid);
        Assert.Equal(12, result[0].Vcid);
        Assert.Equal(5, result[0].Vhid);
        Assert.Equal("Survey A", result[0].Vtitle);
        Assert.Equal("content", result[0].Vcontent);
        Assert.Equal(2, result[0].Vtype);
        Assert.Equal(10, result[0].Vtotal);
        Assert.Equal(100, result[0].Vscore);
        Assert.Equal(86, result[0].Vaverage);
        Assert.False(result[0].Vclose);
        Assert.True(result[0].Vpoint);
        Assert.Equal(new DateTime(2024, 4, 10), result[0].Vdate);
    }

    [Fact]
    public void BllDataTableMappers_MapSurveyItemList_MapsSurveyItemFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Mid");
        dt.Columns.Add("Mqid");
        dt.Columns.Add("Mvid");
        dt.Columns.Add("Mitem");
        dt.Columns.Add("Mscore");
        dt.Columns.Add("Mcount");
        dt.Rows.Add("81", "9", "71", "Option A", "5", "12");

        var result = LearnSite.BLL.BllDataTableMappers.MapSurveyItemList(dt);

        Assert.Single(result);
        Assert.Equal(81, result[0].Mid);
        Assert.Equal(9, result[0].Mqid);
        Assert.Equal(71, result[0].Mvid);
        Assert.Equal("Option A", result[0].Mitem);
        Assert.Equal(5, result[0].Mscore);
        Assert.Equal(12, result[0].Mcount);
    }

    [Fact]
    public void BllDataTableMappers_MapSurveyQuestionList_MapsSurveyQuestionFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Qid");
        dt.Columns.Add("Qvid");
        dt.Columns.Add("Qcid");
        dt.Columns.Add("Qtitle");
        dt.Columns.Add("Qcount");
        dt.Rows.Add("91", "71", "12", "How satisfied are you?", "30");

        var result = LearnSite.BLL.BllDataTableMappers.MapSurveyQuestionList(dt);

        Assert.Single(result);
        Assert.Equal(91, result[0].Qid);
        Assert.Equal(71, result[0].Qvid);
        Assert.Equal(12, result[0].Qcid);
        Assert.Equal("How satisfied are you?", result[0].Qtitle);
        Assert.Equal(30, result[0].Qcount);
    }

    [Fact]
    public void BllDataTableMappers_MapSoftList_MapsSoftFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Fid");
        dt.Columns.Add("Ftitle");
        dt.Columns.Add("Fcontent");
        dt.Columns.Add("Furl");
        dt.Columns.Add("Fhit");
        dt.Columns.Add("Fdate");
        dt.Columns.Add("Ffiletype");
        dt.Columns.Add("Fclass");
        dt.Columns.Add("Fhide");
        dt.Columns.Add("Fopen");
        dt.Columns.Add("Fhid");
        dt.Columns.Add("Fyid");
        dt.Rows.Add("101", "Tool A", "soft content", "/download/tool-a.zip", "8", "2024-04-11", "zip", "tools", "true", "1", "5", "2");

        var result = LearnSite.BLL.BllDataTableMappers.MapSoftList(dt);

        Assert.Single(result);
        Assert.Equal(101, result[0].Fid);
        Assert.Equal("Tool A", result[0].Ftitle);
        Assert.Equal("soft content", result[0].Fcontent);
        Assert.Equal("/download/tool-a.zip", result[0].Furl);
        Assert.Equal(8, result[0].Fhit);
        Assert.Equal(new DateTime(2024, 4, 11), result[0].Fdate);
        Assert.Equal("zip", result[0].Ffiletype);
        Assert.Equal("tools", result[0].Fclass);
        Assert.True(result[0].Fhide);
        Assert.Equal(1, result[0].Fopen);
        Assert.Equal(5, result[0].Fhid);
        Assert.Equal(2, result[0].Fyid);
    }

    [Fact]
    public void BllDataTableMappers_MapResearchList_MapsResearchFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Rid");
        dt.Columns.Add("Rsid");
        dt.Columns.Add("Ryear");
        dt.Columns.Add("Rgrade");
        dt.Columns.Add("Rclass");
        dt.Columns.Add("Rterm");
        dt.Columns.Add("Rlearn");
        dt.Columns.Add("Rplay");
        dt.Columns.Add("Rsleep");
        dt.Columns.Add("Rfree");
        dt.Columns.Add("Rdate");
        dt.Rows.Add("111", "3002", "2024", "6", "2", "1", "2.5", "1.5", "8", "3", "2024-04-12");

        var result = LearnSite.BLL.BllDataTableMappers.MapResearchList(dt);

        Assert.Single(result);
        Assert.Equal(111, result[0].Rid);
        Assert.Equal(3002, result[0].Rsid);
        Assert.Equal(2024, result[0].Ryear);
        Assert.Equal(6, result[0].Rgrade);
        Assert.Equal(2, result[0].Rclass);
        Assert.Equal(1, result[0].Rterm);
        Assert.Equal(2.5m, result[0].Rlearn);
        Assert.Equal(1.5m, result[0].Rplay);
        Assert.Equal(8m, result[0].Rsleep);
        Assert.Equal(3m, result[0].Rfree);
        Assert.Equal(new DateTime(2024, 4, 12), result[0].Rdate);
    }

    [Fact]
    public void BllDataTableMappers_MapResultList_MapsResultFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Rid");
        dt.Columns.Add("Rnum");
        dt.Columns.Add("Rscore");
        dt.Columns.Add("Rdate");
        dt.Columns.Add("Rhistory");
        dt.Columns.Add("Rwrong");
        dt.Columns.Add("Rgrade");
        dt.Columns.Add("Rterm");
        dt.Rows.Add("121", "2024008", "96", "2024-04-13", "1,2,3", "4,5", "6", "2");

        var result = LearnSite.BLL.BllDataTableMappers.MapResultList(dt);

        Assert.Single(result);
        Assert.Equal(121, result[0].Rid);
        Assert.Equal("2024008", result[0].Rnum);
        Assert.Equal(96, result[0].Rscore);
        Assert.Equal(new DateTime(2024, 4, 13), result[0].Rdate);
        Assert.Equal("1,2,3", result[0].Rhistory);
        Assert.Equal("4,5", result[0].Rwrong);
        Assert.Equal(6, result[0].Rgrade);
        Assert.Equal(2, result[0].Rterm);
    }

    [Fact]
    public void BllDataTableMappers_MapMissionList_MapsMissionFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Mid");
        dt.Columns.Add("Mtitle");
        dt.Columns.Add("Mcid");
        dt.Columns.Add("Mcontent");
        dt.Columns.Add("Mdate");
        dt.Columns.Add("Mhit");
        dt.Columns.Add("Mfiletype");
        dt.Columns.Add("Mupload");
        dt.Columns.Add("Msort");
        dt.Columns.Add("Mpublish");
        dt.Columns.Add("Mgroup");
        dt.Columns.Add("Mgid");
        dt.Rows.Add("131", "Mission A", "20", "mission body", "2024-04-14", "15", "docx", "true", "3", "1", "false", "9");

        var result = LearnSite.BLL.BllDataTableMappers.MapMissionList(dt);

        Assert.Single(result);
        Assert.Equal(131, result[0].Mid);
        Assert.Equal("Mission A", result[0].Mtitle);
        Assert.Equal(20, result[0].Mcid);
        Assert.Equal("mission body", result[0].Mcontent);
        Assert.Equal(new DateTime(2024, 4, 14), result[0].Mdate);
        Assert.Equal(15, result[0].Mhit);
        Assert.Equal("docx", result[0].Mfiletype);
        Assert.True(result[0].Mupload);
        Assert.Equal(3, result[0].Msort);
        Assert.True(result[0].Mpublish);
        Assert.False(result[0].Mgroup);
        Assert.Equal(9, result[0].Mgid);
    }

    [Fact]
    public void BllDataTableMappers_MapListMenuList_MapsListMenuFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Lid");
        dt.Columns.Add("Lcid");
        dt.Columns.Add("Lsort");
        dt.Columns.Add("Ltype");
        dt.Columns.Add("Lxid");
        dt.Columns.Add("Lshow");
        dt.Columns.Add("Ltitle");
        dt.Rows.Add("141", "20", "4", "2", "131", "true", "Menu title");

        var result = LearnSite.BLL.BllDataTableMappers.MapListMenuList(dt);

        Assert.Single(result);
        Assert.Equal(141, result[0].Lid);
        Assert.Equal(20, result[0].Lcid);
        Assert.Equal(4, result[0].Lsort);
        Assert.Equal(2, result[0].Ltype);
        Assert.Equal(131, result[0].Lxid);
        Assert.True(result[0].Lshow);
        Assert.Equal("Menu title", result[0].Ltitle);
    }

    [Fact]
    public void BllDataTableMappers_MapExamsList_MapsExamFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Eid");
        dt.Columns.Add("Etitle");
        dt.Columns.Add("Edescription");
        dt.Columns.Add("Cid");
        dt.Columns.Add("Hid");
        dt.Columns.Add("Etime");
        dt.Columns.Add("Eclose");
        dt.Columns.Add("Escore");
        dt.Columns.Add("Ecount");
        dt.Columns.Add("Edata");
        dt.Rows.Add("151", "Exam A", "desc", "20", "6", "2024-04-15", "false", "100", "25", "json-data");

        var result = LearnSite.BLL.BllDataTableMappers.MapExamsList(dt);

        Assert.Single(result);
        Assert.Equal(151, result[0].Eid);
        Assert.Equal("Exam A", result[0].Etitle);
        Assert.Equal("desc", result[0].Edescription);
        Assert.Equal(20, result[0].Cid);
        Assert.Equal(6, result[0].Hid);
        Assert.Equal(new DateTime(2024, 4, 15), result[0].Etime);
        Assert.False(result[0].Eclose);
        Assert.Equal(100, result[0].Escore);
        Assert.Equal(25, result[0].Ecount);
        Assert.Equal("json-data", result[0].Edata);
    }

    [Fact]
    public void BllDataTableMappers_MapTeacherList_MapsTeacherFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Hid");
        dt.Columns.Add("Hname");
        dt.Columns.Add("Hpwd");
        dt.Columns.Add("Hpermiss");
        dt.Columns.Add("Hnote");
        dt.Rows.Add("161", "teacher1", "pwd123", "true", "note text");

        var result = LearnSite.BLL.BllDataTableMappers.MapTeacherList(dt);

        Assert.Single(result);
        Assert.Equal(161, result[0].Hid);
        Assert.Equal("teacher1", result[0].Hname);
        Assert.Equal("pwd123", result[0].Hpwd);
        Assert.True(result[0].Hpermiss);
        Assert.Equal("note text", result[0].Hnote);
    }

    [Fact]
    public void BllDataTableMappers_MapStudentsExcelList_MapsStudentExcelFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Sid");
        dt.Columns.Add("Snum");
        dt.Columns.Add("Syear");
        dt.Columns.Add("Sgrade");
        dt.Columns.Add("Sclass");
        dt.Columns.Add("Sname");
        dt.Columns.Add("Spwd");
        dt.Columns.Add("Sex");
        dt.Columns.Add("Saddress");
        dt.Columns.Add("Sphone");
        dt.Columns.Add("Sparents");
        dt.Columns.Add("Sheadtheacher");
        dt.Columns.Add("Sscore");
        dt.Columns.Add("Squiz");
        dt.Columns.Add("Sattitude");
        dt.Columns.Add("Sape");
        dt.Rows.Add("171", "2024010", "2024", "6", "1", "Student A", "123456", "F", "addr", "123", "parent", "head", "88", "9", "5", "ape text");

        var result = LearnSite.BLL.BllDataTableMappers.MapStudentsExcelList(dt);

        Assert.Single(result);
        Assert.Equal(171, result[0].Sid);
        Assert.Equal("2024010", result[0].Snum);
        Assert.Equal(2024, result[0].Syear);
        Assert.Equal(6, result[0].Sgrade);
        Assert.Equal(1, result[0].Sclass);
        Assert.Equal("Student A", result[0].Sname);
        Assert.Equal("123456", result[0].Spwd);
        Assert.Equal("F", result[0].Sex);
        Assert.Equal("addr", result[0].Saddress);
        Assert.Equal("123", result[0].Sphone);
        Assert.Equal("parent", result[0].Sparents);
        Assert.Equal("head", result[0].Sheadtheacher);
        Assert.Equal(88, result[0].Sscore);
        Assert.Equal(9, result[0].Squiz);
        Assert.Equal(5, result[0].Sattitude);
        Assert.Equal("ape text", result[0].Sape);
    }

    [Fact]
    public void BllDataTableMappers_MapShareDiskList_MapsShareDiskFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Kid");
        dt.Columns.Add("Kown");
        dt.Columns.Add("Kyear");
        dt.Columns.Add("Kgrade");
        dt.Columns.Add("Kclass");
        dt.Columns.Add("Kgroup");
        dt.Columns.Add("Knum");
        dt.Columns.Add("Kname");
        dt.Columns.Add("Kfilename");
        dt.Columns.Add("Kfsize");
        dt.Columns.Add("Kfurl");
        dt.Columns.Add("Kftpe");
        dt.Columns.Add("Kfdate");
        dt.Rows.Add("181", "1", "2024", "6", "2", "3", "2024011", "Student B", "demo.docx", "2048", "/disk/demo.docx", "docx", "2024-04-16");

        var result = LearnSite.BLL.BllDataTableMappers.MapShareDiskList(dt);

        Assert.Single(result);
        Assert.Equal(181, result[0].Kid);
        Assert.True(result[0].Kown);
        Assert.Equal(2024, result[0].Kyear);
        Assert.Equal(6, result[0].Kgrade);
        Assert.Equal(2, result[0].Kclass);
        Assert.Equal(3, result[0].Kgroup);
        Assert.Equal("2024011", result[0].Knum);
        Assert.Equal("Student B", result[0].Kname);
        Assert.Equal("demo.docx", result[0].Kfilename);
        Assert.Equal(2048, result[0].Kfsize);
        Assert.Equal("/disk/demo.docx", result[0].Kfurl);
        Assert.Equal("docx", result[0].Kftpe);
        Assert.Equal(new DateTime(2024, 4, 16), result[0].Kfdate);
    }

    [Fact]
    public void BllDataTableMappers_MapTopicReplyList_MapsTopicReplyFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Rid");
        dt.Columns.Add("Rtid");
        dt.Columns.Add("Rsnum");
        dt.Columns.Add("Rwords");
        dt.Columns.Add("Rtime");
        dt.Columns.Add("Rip");
        dt.Columns.Add("Rscore");
        dt.Columns.Add("Rban");
        dt.Columns.Add("Rgrade");
        dt.Columns.Add("Rterm");
        dt.Columns.Add("Rcid");
        dt.Columns.Add("Rclass");
        dt.Rows.Add("191", "12", "2024012", "reply body", "2024-04-17", "10.0.0.20", "6", "false", "6", "2", "20", "3");

        var result = LearnSite.BLL.BllDataTableMappers.MapTopicReplyList(dt);

        Assert.Single(result);
        Assert.Equal(191, result[0].Rid);
        Assert.Equal(12, result[0].Rtid);
        Assert.Equal("2024012", result[0].Rsnum);
        Assert.Equal("reply body", result[0].Rwords);
        Assert.Equal(new DateTime(2024, 4, 17), result[0].Rtime);
        Assert.Equal("10.0.0.20", result[0].Rip);
        Assert.Equal(6, result[0].Rscore);
        Assert.False(result[0].Rban);
        Assert.Equal(6, result[0].Rgrade);
        Assert.Equal(2, result[0].Rterm);
        Assert.Equal(20, result[0].Rcid);
        Assert.Equal(3, result[0].Rclass);
    }

    [Fact]
    public void BllDataTableMappers_MapWorksDiscussList_MapsWorksDiscussFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Did");
        dt.Columns.Add("Dwid");
        dt.Columns.Add("Dsnum");
        dt.Columns.Add("Dwords");
        dt.Columns.Add("Dtime");
        dt.Columns.Add("Dip");
        dt.Rows.Add("201", "21", "2024013", "good work", "2024-04-18", "10.0.0.21");

        var result = LearnSite.BLL.BllDataTableMappers.MapWorksDiscussList(dt);

        Assert.Single(result);
        Assert.Equal(201, result[0].Did);
        Assert.Equal(21, result[0].Dwid);
        Assert.Equal("2024013", result[0].Dsnum);
        Assert.Equal("good work", result[0].Dwords);
        Assert.Equal(new DateTime(2024, 4, 18), result[0].Dtime);
        Assert.Equal("10.0.0.21", result[0].Dip);
    }

    [Fact]
    public void BllDataTableMappers_MapTopicDiscussList_MapsTopicDiscussFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Tid");
        dt.Columns.Add("Tcid");
        dt.Columns.Add("Ttitle");
        dt.Columns.Add("Tcontent");
        dt.Columns.Add("Tcount");
        dt.Columns.Add("Tteacher");
        dt.Columns.Add("Tdate");
        dt.Columns.Add("Tclose");
        dt.Columns.Add("Tresult");
        dt.Rows.Add("211", "20", "Topic A", "topic body", "14", "6", "2024-04-19", "true", "teacher summary");

        var result = LearnSite.BLL.BllDataTableMappers.MapTopicDiscussList(dt);

        Assert.Single(result);
        Assert.Equal(211, result[0].Tid);
        Assert.Equal(20, result[0].Tcid);
        Assert.Equal("Topic A", result[0].Ttitle);
        Assert.Equal("topic body", result[0].Tcontent);
        Assert.Equal(14, result[0].Tcount);
        Assert.Equal(6, result[0].Tteacher);
        Assert.Equal(new DateTime(2024, 4, 19), result[0].Tdate);
        Assert.True(result[0].Tclose);
        Assert.Equal("teacher summary", result[0].Tresult);
    }

    [Fact]
    public void BllDataTableMappers_MapTurtleAnswerList_MapsTurtleAnswerFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Aid");
        dt.Columns.Add("Amid");
        dt.Columns.Add("Aqid");
        dt.Columns.Add("Acode");
        dt.Columns.Add("Aimg");
        dt.Columns.Add("Aurl");
        dt.Columns.Add("Aout");
        dt.Columns.Add("Ascore");
        dt.Columns.Add("Asid");
        dt.Columns.Add("Asname");
        dt.Columns.Add("Alock");
        dt.Columns.Add("Adate");
        dt.Rows.Add("221", "31", "41", "print(1)", "img.png", "/turtle/a.png", "ok", "10", "3003", "stu-a", "true", "2024-04-20");

        var result = LearnSite.BLL.BllDataTableMappers.MapTurtleAnswerList(dt);

        Assert.Single(result);
        Assert.Equal(221, result[0].Aid);
        Assert.Equal(31, result[0].Amid);
        Assert.Equal(41, result[0].Aqid);
        Assert.Equal("print(1)", result[0].Acode);
        Assert.Equal("img.png", result[0].Aimg);
        Assert.Equal("/turtle/a.png", result[0].Aurl);
        Assert.Equal("ok", result[0].Aout);
        Assert.Equal(10, result[0].Ascore);
        Assert.Equal(3003, result[0].Asid);
        Assert.Equal("stu-a", result[0].Asname);
        Assert.True(result[0].Alock);
        Assert.Equal(new DateTime(2024, 4, 20), result[0].Adate);
    }

    [Fact]
    public void BllDataTableMappers_MapTurtleQuestionList_MapsTurtleQuestionFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Qid");
        dt.Columns.Add("Qmid");
        dt.Columns.Add("Qtitle");
        dt.Columns.Add("Qcontent");
        dt.Columns.Add("Qdegree");
        dt.Columns.Add("Qsort");
        dt.Columns.Add("Qcode");
        dt.Columns.Add("Qimg");
        dt.Columns.Add("Qurl");
        dt.Columns.Add("Qout");
        dt.Columns.Add("Qscore");
        dt.Columns.Add("Qdate");
        dt.Rows.Add("231", "31", "Question A", "content", "2", "1", "code", "res.png", "/q/res.png", "done", "8", "2024-04-21");

        var result = LearnSite.BLL.BllDataTableMappers.MapTurtleQuestionList(dt);

        Assert.Single(result);
        Assert.Equal(231, result[0].Qid);
        Assert.Equal(31, result[0].Qmid);
        Assert.Equal("Question A", result[0].Qtitle);
        Assert.Equal("content", result[0].Qcontent);
        Assert.Equal(2, result[0].Qdegree);
        Assert.Equal(1, result[0].Qsort);
        Assert.Equal("code", result[0].Qcode);
        Assert.Equal("res.png", result[0].Qimg);
        Assert.Equal("/q/res.png", result[0].Qurl);
        Assert.Equal("done", result[0].Qout);
        Assert.Equal(8, result[0].Qscore);
        Assert.Equal(new DateTime(2024, 4, 21), result[0].Qdate);
    }

    [Fact]
    public void BllDataTableMappers_MapTurtleList_MapsTurtleFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Tid");
        dt.Columns.Add("Thid");
        dt.Columns.Add("Ttilte");
        dt.Columns.Add("Tcontent");
        dt.Columns.Add("Tdegree");
        dt.Columns.Add("Tsort");
        dt.Columns.Add("Tcode");
        dt.Columns.Add("Timg");
        dt.Columns.Add("Turl");
        dt.Columns.Add("Tout");
        dt.Columns.Add("Tdate");
        dt.Columns.Add("Tstudy");
        dt.Columns.Add("Tsid");
        dt.Columns.Add("Tscore");
        dt.Columns.Add("Tip");
        dt.Rows.Add("241", "6", "Turtle A", "body", "1", "3", "code", "img", "/turtle/x.png", "out", "2024-04-22", "false", "3004", "7", "10.0.0.22");

        var result = LearnSite.BLL.BllDataTableMappers.MapTurtleList(dt);

        Assert.Single(result);
        Assert.Equal(241, result[0].Tid);
        Assert.Equal(6, result[0].Thid);
        Assert.Equal("Turtle A", result[0].Ttilte);
        Assert.Equal("body", result[0].Tcontent);
        Assert.Equal(1, result[0].Tdegree);
        Assert.Equal(3, result[0].Tsort);
        Assert.Equal("code", result[0].Tcode);
        Assert.Equal("img", result[0].Timg);
        Assert.Equal("/turtle/x.png", result[0].Turl);
        Assert.Equal("out", result[0].Tout);
        Assert.Equal(new DateTime(2024, 4, 22), result[0].Tdate);
        Assert.False(result[0].Tstudy);
        Assert.Equal(3004, result[0].Tsid);
        Assert.Equal(7, result[0].Tscore);
        Assert.Equal("10.0.0.22", result[0].Tip);
    }

    [Fact]
    public void BllDataTableMappers_MapAnswersList_MapsAnswersFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Aid");
        dt.Columns.Add("Eid");
        dt.Columns.Add("Asid");
        dt.Columns.Add("Asnum");
        dt.Columns.Add("Asname");
        dt.Columns.Add("Asgrade");
        dt.Columns.Add("Asclass");
        dt.Columns.Add("Atime");
        dt.Columns.Add("Ascore");
        dt.Columns.Add("Aspent");
        dt.Columns.Add("Adata");
        dt.Rows.Add("251", "80", "3005", "2024014", "stu-b", "6", "2", "2024-04-23", "95", "120", "answer-json");

        var result = LearnSite.BLL.BllDataTableMappers.MapAnswersList(dt);

        Assert.Single(result);
        Assert.Equal(251, result[0].Aid);
        Assert.Equal(80, result[0].Eid);
        Assert.Equal(3005, result[0].Asid);
        Assert.Equal("2024014", result[0].Asnum);
        Assert.Equal("stu-b", result[0].Asname);
        Assert.Equal(6, result[0].Asgrade);
        Assert.Equal(2, result[0].Asclass);
        Assert.Equal(new DateTime(2024, 4, 23), result[0].Atime);
        Assert.Equal(95, result[0].Ascore);
        Assert.Equal(120, result[0].Aspent);
        Assert.Equal("answer-json", result[0].Adata);
    }

    [Fact]
    public void BllDataTableMappers_MapGameList_MapsGameFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Gid");
        dt.Columns.Add("Gsid");
        dt.Columns.Add("Gsname");
        dt.Columns.Add("Gnum");
        dt.Columns.Add("Gtitle");
        dt.Columns.Add("Gsave");
        dt.Columns.Add("Gnote");
        dt.Columns.Add("Gscore");
        dt.Columns.Add("Gdate");
        dt.Rows.Add("261", "3006", "stu-c", "4", "maze", "12", "note", "0", "2024-04-24");

        var result = LearnSite.BLL.BllDataTableMappers.MapGameList(dt);

        Assert.Single(result);
        Assert.Equal(261, result[0].Gid);
        Assert.Equal(3006, result[0].Gsid);
        Assert.Equal("stu-c", result[0].Gsname);
        Assert.Equal(4, result[0].Gnum);
        Assert.Equal("maze", result[0].Gtitle);
        Assert.Equal(12, result[0].Gsave);
        Assert.Equal("note", result[0].Gnote);
        Assert.Equal(0, result[0].Gscore);
        Assert.Equal(new DateTime(2024, 4, 24), result[0].Gdate);
    }

    [Fact]
    public void BllDataTableMappers_MapChineseList_MapsChineseFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Nid");
        dt.Columns.Add("Ntitle");
        dt.Columns.Add("Ncontent");
        dt.Rows.Add("271", "Lesson A", "ni hao");

        var result = LearnSite.BLL.BllDataTableMappers.MapChineseList(dt);

        Assert.Single(result);
        Assert.Equal(271, result[0].Nid);
        Assert.Equal("Lesson A", result[0].Ntitle);
        Assert.Equal("ni hao", result[0].Ncontent);
    }

    [Fact]
    public void BllDataTableMappers_MapGaugeList_MapsGaugeFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Gid");
        dt.Columns.Add("Ghid");
        dt.Columns.Add("Gtype");
        dt.Columns.Add("Gtitle");
        dt.Columns.Add("Gcount");
        dt.Columns.Add("Gdate");
        dt.Rows.Add("281", "6", "works", "Gauge A", "4", "2024-04-25");

        var result = LearnSite.BLL.BllDataTableMappers.MapGaugeList(dt);

        Assert.Single(result);
        Assert.Equal(281, result[0].Gid);
        Assert.Equal(6, result[0].Ghid);
        Assert.Equal("works", result[0].Gtype);
        Assert.Equal("Gauge A", result[0].Gtitle);
        Assert.Equal(4, result[0].Gcount);
        Assert.Equal(new DateTime(2024, 4, 25), result[0].Gdate);
    }

    [Fact]
    public void BllDataTableMappers_MapGaugeItemList_MapsGaugeItemFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Mid");
        dt.Columns.Add("Mgid");
        dt.Columns.Add("Mitem");
        dt.Columns.Add("Mscore");
        dt.Columns.Add("Msort");
        dt.Rows.Add("291", "281", "Creativity", "5", "1");

        var result = LearnSite.BLL.BllDataTableMappers.MapGaugeItemList(dt);

        Assert.Single(result);
        Assert.Equal(291, result[0].Mid);
        Assert.Equal(281, result[0].Mgid);
        Assert.Equal("Creativity", result[0].Mitem);
        Assert.Equal(5, result[0].Mscore);
        Assert.Equal(1, result[0].Msort);
    }

    [Fact]
    public void BllDataTableMappers_MapGaugeFeedbackList_MapsGaugeFeedbackFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Fid");
        dt.Columns.Add("Fnum");
        dt.Columns.Add("Fgrade");
        dt.Columns.Add("Fclass");
        dt.Columns.Add("Fcid");
        dt.Columns.Add("Fmid");
        dt.Columns.Add("Fwid");
        dt.Columns.Add("Fgid");
        dt.Columns.Add("Fselect");
        dt.Columns.Add("Fscore");
        dt.Columns.Add("Fgood");
        dt.Columns.Add("Fdate");
        dt.Rows.Add("301", "2024015", "6", "1", "20", "291", "21", "281", "291,292", "9", "true", "2024-04-26");

        var result = LearnSite.BLL.BllDataTableMappers.MapGaugeFeedbackList(dt);

        Assert.Single(result);
        Assert.Equal(301, result[0].Fid);
        Assert.Equal("2024015", result[0].Fnum);
        Assert.Equal(6, result[0].Fgrade);
        Assert.Equal(1, result[0].Fclass);
        Assert.Equal(20, result[0].Fcid);
        Assert.Equal(291, result[0].Fmid);
        Assert.Equal(21, result[0].Fwid);
        Assert.Equal(281, result[0].Fgid);
        Assert.Equal("291,292", result[0].Fselect);
        Assert.Equal(9, result[0].Fscore);
        Assert.True(result[0].Fgood);
        Assert.Equal(new DateTime(2024, 4, 26), result[0].Fdate);
    }

    [Fact]
    public void BllDataTableMappers_MapEnglishList_MapsEnglishFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Eid");
        dt.Columns.Add("Eword");
        dt.Columns.Add("Emeaning");
        dt.Columns.Add("Elevel");
        dt.Rows.Add("311", "apple", "fruit", "2");

        var result = LearnSite.BLL.BllDataTableMappers.MapEnglishList(dt);

        Assert.Single(result);
        Assert.Equal(311, result[0].Eid);
        Assert.Equal("apple", result[0].Eword);
        Assert.Equal("fruit", result[0].Emeaning);
        Assert.Equal(2, result[0].Elevel);
    }

    [Fact]
    public void BllDataTableMappers_MapSummaryList_MapsSummaryFieldsAndFalseBoolean()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Sid");
        dt.Columns.Add("Scid");
        dt.Columns.Add("Shid");
        dt.Columns.Add("Scontent");
        dt.Columns.Add("Sdate");
        dt.Columns.Add("Sgrade");
        dt.Columns.Add("Sclass");
        dt.Columns.Add("Syear");
        dt.Columns.Add("Sshow");
        dt.Rows.Add("321", "20", "8", "summary text", "2024-04-27", "6", "2", "2024", "0");

        var result = LearnSite.BLL.BllDataTableMappers.MapSummaryList(dt);

        Assert.Single(result);
        Assert.Equal(321, result[0].Sid);
        Assert.Equal(20, result[0].Scid);
        Assert.Equal(8, result[0].Shid);
        Assert.Equal("summary text", result[0].Scontent);
        Assert.Equal(new DateTime(2024, 4, 27), result[0].Sdate);
        Assert.Equal(6, result[0].Sgrade);
        Assert.Equal(2, result[0].Sclass);
        Assert.Equal(2024, result[0].Syear);
        Assert.False(result[0].Sshow);
    }

    [Fact]
    public void BllDataTableMappers_MapPchineseList_MapsPchineseFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Pid");
        dt.Columns.Add("Psid");
        dt.Columns.Add("Psnum");
        dt.Columns.Add("Papple");
        dt.Columns.Add("Ptotal");
        dt.Columns.Add("Pspeed");
        dt.Columns.Add("Pdegree");
        dt.Columns.Add("Pyear");
        dt.Columns.Add("Pgrade");
        dt.Columns.Add("Pclass");
        dt.Columns.Add("Pterm");
        dt.Columns.Add("Pdate");
        dt.Rows.Add("331", "18", "2024018", "5", "12", "88", "3", "2024", "6", "1", "2", "2024-04-28");

        var result = LearnSite.BLL.BllDataTableMappers.MapPchineseList(dt);

        Assert.Single(result);
        Assert.Equal(331, result[0].Pid);
        Assert.Equal(18, result[0].Psid);
        Assert.Equal("2024018", result[0].Psnum);
        Assert.Equal(5, result[0].Papple);
        Assert.Equal(12, result[0].Ptotal);
        Assert.Equal(88, result[0].Pspeed);
        Assert.Equal(3, result[0].Pdegree);
        Assert.Equal(2024, result[0].Pyear);
        Assert.Equal(6, result[0].Pgrade);
        Assert.Equal(1, result[0].Pclass);
        Assert.Equal(2, result[0].Pterm);
        Assert.Equal(new DateTime(2024, 4, 28), result[0].Pdate);
    }

    [Fact]
    public void BllDataTableMappers_MapSurveyClassList_MapsSurveyClassFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Yid");
        dt.Columns.Add("Yyear");
        dt.Columns.Add("Ygrade");
        dt.Columns.Add("Yclass");
        dt.Columns.Add("Yterm");
        dt.Columns.Add("Ycid");
        dt.Columns.Add("Yvid");
        dt.Columns.Add("Yselect");
        dt.Columns.Add("Ycount");
        dt.Columns.Add("Yscore");
        dt.Columns.Add("Ydate");
        dt.Rows.Add("341", "2024", "6", "2", "1", "30", "40", "1,2,3", "3,2,1", "95", "2024-04-29");

        var result = LearnSite.BLL.BllDataTableMappers.MapSurveyClassList(dt);

        Assert.Single(result);
        Assert.Equal(341, result[0].Yid);
        Assert.Equal(2024, result[0].Yyear);
        Assert.Equal(6, result[0].Ygrade);
        Assert.Equal(2, result[0].Yclass);
        Assert.Equal(1, result[0].Yterm);
        Assert.Equal(30, result[0].Ycid);
        Assert.Equal(40, result[0].Yvid);
        Assert.Equal("1,2,3", result[0].Yselect);
        Assert.Equal("3,2,1", result[0].Ycount);
        Assert.Equal(95, result[0].Yscore);
        Assert.Equal(new DateTime(2024, 4, 29), result[0].Ydate);
    }

    [Fact]
    public void BllDataTableMappers_MapSurveyFeedbackList_MapsSurveyFeedbackFieldsWithoutUnmappedFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Fid");
        dt.Columns.Add("Fnum");
        dt.Columns.Add("Fyear");
        dt.Columns.Add("Fgrade");
        dt.Columns.Add("Fclass");
        dt.Columns.Add("Fterm");
        dt.Columns.Add("Fcid");
        dt.Columns.Add("Fvid");
        dt.Columns.Add("Fvtype");
        dt.Columns.Add("Fselect");
        dt.Columns.Add("Fscore");
        dt.Columns.Add("Fdate");
        dt.Columns.Add("Fsid");
        dt.Columns.Add("Flid");
        dt.Rows.Add("351", "2024021", "2024", "6", "3", "2", "31", "41", "5", "A,B", "88", "2024-04-30", "99", "77");

        var result = LearnSite.BLL.BllDataTableMappers.MapSurveyFeedbackList(dt);

        Assert.Single(result);
        Assert.Equal(351, result[0].Fid);
        Assert.Equal("2024021", result[0].Fnum);
        Assert.Equal(2024, result[0].Fyear);
        Assert.Equal(6, result[0].Fgrade);
        Assert.Equal(3, result[0].Fclass);
        Assert.Equal(2, result[0].Fterm);
        Assert.Equal(31, result[0].Fcid);
        Assert.Equal(41, result[0].Fvid);
        Assert.Equal(5, result[0].Fvtype);
        Assert.Equal("A,B", result[0].Fselect);
        Assert.Equal(88, result[0].Fscore);
        Assert.Equal(new DateTime(2024, 4, 30), result[0].Fdate);
        Assert.Null(result[0].Fsid);
        Assert.Equal(0, result[0].Flid);
    }

    [Fact]
    public void BllDataTableMappers_MapRoomList_MapsLegacyRoomFieldsOnly()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Rid");
        dt.Columns.Add("Rhid");
        dt.Columns.Add("Rgrade");
        dt.Columns.Add("Rclass");
        dt.Columns.Add("Rset");
        dt.Columns.Add("Rpwd");
        dt.Columns.Add("Rlock");
        dt.Columns.Add("Rip");
        dt.Columns.Add("Rgauge");
        dt.Columns.Add("Ropen");
        dt.Rows.Add("361", "5", "6", "1", "true", "pwd", "0", "192.168.1.8", "1", "1");

        var result = LearnSite.BLL.BllDataTableMappers.MapRoomList(dt);

        Assert.Single(result);
        Assert.Equal(361, result[0].Rid);
        Assert.Equal(5, result[0].Rhid);
        Assert.Equal(6, result[0].Rgrade);
        Assert.Equal(1, result[0].Rclass);
        Assert.True(result[0].Rset);
        Assert.Equal("pwd", result[0].Rpwd);
        Assert.False(result[0].Rlock);
        Assert.Equal("192.168.1.8", result[0].Rip);
        Assert.True(result[0].Rgauge);
        Assert.False(result[0].Ropen);
    }

    [Fact]
    public void BllDataTableMappers_MapStudentsList_MapsStudentFieldsAndLegacyBoolean()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Sid");
        dt.Columns.Add("Snum");
        dt.Columns.Add("Syear");
        dt.Columns.Add("Sgrade");
        dt.Columns.Add("Sclass");
        dt.Columns.Add("Sname");
        dt.Columns.Add("Spwd");
        dt.Columns.Add("Sex");
        dt.Columns.Add("Saddress");
        dt.Columns.Add("Sphone");
        dt.Columns.Add("Sparents");
        dt.Columns.Add("Sheadtheacher");
        dt.Columns.Add("Sscore");
        dt.Columns.Add("Squiz");
        dt.Columns.Add("Sattitude");
        dt.Columns.Add("Sape");
        dt.Columns.Add("Swscore");
        dt.Columns.Add("Stscore");
        dt.Columns.Add("Sallscore");
        dt.Columns.Add("Spscore");
        dt.Columns.Add("Sgroup");
        dt.Columns.Add("Sleader");
        dt.Columns.Add("Svote");
        dt.Columns.Add("Sgscore");
        dt.Columns.Add("Stxtform");
        dt.Rows.Add("371", "2024031", "2024", "6", "2", "Alice", "123", "F", "Addr", "12345", "Parent", "Teacher", "91", "4", "5", "A", "92", "93", "94", "95", "6", "true", "7", "96", "88");

        var result = LearnSite.BLL.BllDataTableMappers.MapStudentsList(dt);

        Assert.Single(result);
        Assert.Equal(371, result[0].Sid);
        Assert.Equal("2024031", result[0].Snum);
        Assert.Equal(2024, result[0].Syear);
        Assert.Equal(6, result[0].Sgrade);
        Assert.Equal(2, result[0].Sclass);
        Assert.Equal("Alice", result[0].Sname);
        Assert.Equal("123", result[0].Spwd);
        Assert.Equal("F", result[0].Sex);
        Assert.Equal("Addr", result[0].Saddress);
        Assert.Equal("12345", result[0].Sphone);
        Assert.Equal("Parent", result[0].Sparents);
        Assert.Equal("Teacher", result[0].Sheadtheacher);
        Assert.Equal(91, result[0].Sscore);
        Assert.Equal(4, result[0].Squiz);
        Assert.Equal(5, result[0].Sattitude);
        Assert.Equal("A", result[0].Sape);
        Assert.Equal(92, result[0].Swscore);
        Assert.Equal(93, result[0].Stscore);
        Assert.Equal(94, result[0].Sallscore);
        Assert.Equal(95, result[0].Spscore);
        Assert.Equal(6, result[0].Sgroup);
        Assert.True(result[0].Sleader);
        Assert.Equal(7, result[0].Svote);
        Assert.Equal(96, result[0].Sgscore);
        Assert.Null(result[0].Stxtform);
        Assert.Null(result[0].Sidle);
    }

    [Fact]
    public void BllDataTableMappers_MapIpList_MapsIpFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Iid");
        dt.Columns.Add("Ihid");
        dt.Columns.Add("Inum");
        dt.Columns.Add("Iip");
        dt.Rows.Add("381", "5", "12", "10.0.0.12");

        var result = LearnSite.BLL.BllDataTableMappers.MapIpList(dt);

        Assert.Single(result);
        Assert.Equal(381, result[0].Iid);
        Assert.Equal(5, result[0].Ihid);
        Assert.Equal(12, result[0].Inum);
        Assert.Equal("10.0.0.12", result[0].Iip);
    }

    [Fact]
    public void BllDataTableMappers_MapHouseList_MapsHouseFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Hid");
        dt.Columns.Add("Hname");
        dt.Columns.Add("Hseat");
        dt.Rows.Add("391", "Room A", "1,2,3");

        var result = LearnSite.BLL.BllDataTableMappers.MapHouseList(dt);

        Assert.Single(result);
        Assert.Equal(391, result[0].Hid);
        Assert.Equal("Room A", result[0].Hname);
        Assert.Equal("1,2,3", result[0].Hseat);
    }

    [Fact]
    public void BllDataTableMappers_MapConsolesList_MapsLegacyConsoleFieldsOnly()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Nid");
        dt.Columns.Add("Nhid");
        dt.Columns.Add("Ncid");
        dt.Columns.Add("Ntitle");
        dt.Columns.Add("Ncontent");
        dt.Columns.Add("Npublish");
        dt.Columns.Add("Ndate");
        dt.Columns.Add("Nbegin");
        dt.Rows.Add("401", "5", "30", "Console A", "content", "false", "2024-05-01", "true");

        var result = LearnSite.BLL.BllDataTableMappers.MapConsolesList(dt);

        Assert.Single(result);
        Assert.Equal(401, result[0].Nid);
        Assert.Equal(5, result[0].Nhid);
        Assert.Equal(30, result[0].Ncid);
        Assert.Equal("Console A", result[0].Ntitle);
        Assert.Equal("content", result[0].Ncontent);
        Assert.False(result[0].Npublish);
        Assert.Equal(new DateTime(2024, 5, 1), result[0].Ndate);
        Assert.False(result[0].Nbegin);
    }

    [Fact]
    public void BllDataTableMappers_MapAutonomicList_MapsAutonomicFieldsAndBooleanVariants()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Aid");
        dt.Columns.Add("Asid");
        dt.Columns.Add("Anum");
        dt.Columns.Add("Aname");
        dt.Columns.Add("Ayid");
        dt.Columns.Add("Afid");
        dt.Columns.Add("Atype");
        dt.Columns.Add("Afilename");
        dt.Columns.Add("Aurl");
        dt.Columns.Add("Alength");
        dt.Columns.Add("Ascore");
        dt.Columns.Add("Adate");
        dt.Columns.Add("Aip");
        dt.Columns.Add("Avote");
        dt.Columns.Add("Aegg");
        dt.Columns.Add("Acheck");
        dt.Columns.Add("Aself");
        dt.Columns.Add("Agood");
        dt.Columns.Add("Ayear");
        dt.Columns.Add("Agrade");
        dt.Columns.Add("Aclass");
        dt.Columns.Add("Aterm");
        dt.Columns.Add("Ahit");
        dt.Columns.Add("Aoffice");
        dt.Columns.Add("Aflash");
        dt.Columns.Add("Aerror");
        dt.Rows.Add("411", "21", "2024041", "Bob", "61", "71", "scratch", "demo.sb3", "/upload/demo.sb3", "123", "98", "2024-05-02", "127.0.0.1", "9", "2", "1", "self note", "false", "2024", "6", "3", "2", "7", "true", "0", "true");

        var result = LearnSite.BLL.BllDataTableMappers.MapAutonomicList(dt);

        Assert.Single(result);
        Assert.Equal(411, result[0].Aid);
        Assert.Equal(21, result[0].Asid);
        Assert.Equal("2024041", result[0].Anum);
        Assert.Equal("Bob", result[0].Aname);
        Assert.Equal(61, result[0].Ayid);
        Assert.Equal(71, result[0].Afid);
        Assert.Equal("scratch", result[0].Atype);
        Assert.Equal("demo.sb3", result[0].Afilename);
        Assert.Equal("/upload/demo.sb3", result[0].Aurl);
        Assert.Equal(123, result[0].Alength);
        Assert.Equal(98, result[0].Ascore);
        Assert.Equal(new DateTime(2024, 5, 2), result[0].Adate);
        Assert.Equal("127.0.0.1", result[0].Aip);
        Assert.Equal(9, result[0].Avote);
        Assert.Equal(2, result[0].Aegg);
        Assert.True(result[0].Acheck);
        Assert.Equal("self note", result[0].Aself);
        Assert.False(result[0].Agood);
        Assert.Equal(2024, result[0].Ayear);
        Assert.Equal(6, result[0].Agrade);
        Assert.Equal(3, result[0].Aclass);
        Assert.Equal(2, result[0].Aterm);
        Assert.Equal(7, result[0].Ahit);
        Assert.True(result[0].Aoffice);
        Assert.False(result[0].Aflash);
        Assert.True(result[0].Aerror);
    }

    [Fact]
    public void BllDataTableMappers_MapGroupWorkList_MapsGroupWorkFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Gid");
        dt.Columns.Add("Gnum");
        dt.Columns.Add("Gstudents");
        dt.Columns.Add("Gterm");
        dt.Columns.Add("Ggrade");
        dt.Columns.Add("Gclass");
        dt.Columns.Add("Gcid");
        dt.Columns.Add("Gmid");
        dt.Columns.Add("Gfilename");
        dt.Columns.Add("Gtype");
        dt.Columns.Add("Gurl");
        dt.Columns.Add("Glengh");
        dt.Columns.Add("Gscore");
        dt.Columns.Add("Gtime");
        dt.Columns.Add("Gvote");
        dt.Columns.Add("Gcheck");
        dt.Columns.Add("Gnote");
        dt.Columns.Add("Grank");
        dt.Columns.Add("Ghit");
        dt.Columns.Add("Gip");
        dt.Columns.Add("Gdate");
        dt.Columns.Add("Ggroup");
        dt.Rows.Add("421", "2024051", "a,b,c", "2", "6", "3", "40", "50", "demo.zip", "zip", "/group/demo.zip", "321", "87", "120", "9", "true", "good", "1", "33", "10.0.0.8", "2024-05-03", "4");

        var result = LearnSite.BLL.BllDataTableMappers.MapGroupWorkList(dt);

        Assert.Single(result);
        Assert.Equal(421, result[0].Gid);
        Assert.Equal("2024051", result[0].Gnum);
        Assert.Equal("a,b,c", result[0].Gstudents);
        Assert.Equal(2, result[0].Gterm);
        Assert.Equal(6, result[0].Ggrade);
        Assert.Equal(3, result[0].Gclass);
        Assert.Equal(40, result[0].Gcid);
        Assert.Equal(50, result[0].Gmid);
        Assert.Equal("demo.zip", result[0].Gfilename);
        Assert.Equal("zip", result[0].Gtype);
        Assert.Equal("/group/demo.zip", result[0].Gurl);
        Assert.Equal(321, result[0].Glengh);
        Assert.Equal(87, result[0].Gscore);
        Assert.Equal(120, result[0].Gtime);
        Assert.Equal(9, result[0].Gvote);
        Assert.True(result[0].Gcheck);
        Assert.Equal("good", result[0].Gnote);
        Assert.Equal(1, result[0].Grank);
        Assert.Equal(33, result[0].Ghit);
        Assert.Equal("10.0.0.8", result[0].Gip);
        Assert.Equal(new DateTime(2024, 5, 3), result[0].Gdate);
        Assert.Equal(4, result[0].Ggroup);
    }

    [Fact]
    public void BllDataTableMappers_MapJudgeArgList_MapsJudgeArgFieldsAndMissingThumbFallback()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Jid");
        dt.Columns.Add("Jhid");
        dt.Columns.Add("Jmid");
        dt.Columns.Add("Jsleep");
        dt.Columns.Add("Jinone");
        dt.Columns.Add("Jintwo");
        dt.Columns.Add("Jinthree");
        dt.Columns.Add("Joutone");
        dt.Columns.Add("Joutwo");
        dt.Columns.Add("Jouthree");
        dt.Columns.Add("Jright");
        dt.Columns.Add("Jcode");
        dt.Columns.Add("Jcid");
        dt.Columns.Add("Jimg");
        dt.Rows.Add("431", "5", "60", "1500", "1", "2", "3", "4", "5", "6", "false", "print(1)", "70", "judge.png");

        var result = LearnSite.BLL.BllDataTableMappers.MapJudgeArgList(dt);

        Assert.Single(result);
        Assert.Equal(431, result[0].Jid);
        Assert.Equal(5, result[0].Jhid);
        Assert.Equal(60, result[0].Jmid);
        Assert.Equal(1500, result[0].Jsleep);
        Assert.Equal("1", result[0].Jinone);
        Assert.Equal("2", result[0].Jintwo);
        Assert.Equal("3", result[0].Jinthree);
        Assert.Equal("4", result[0].Joutone);
        Assert.Equal("5", result[0].Joutwo);
        Assert.Equal("6", result[0].Jouthree);
        Assert.False(result[0].Jright);
        Assert.Equal("print(1)", result[0].Jcode);
        Assert.Equal(70, result[0].Jcid);
        Assert.Equal("judge.png", result[0].Jimg);
        Assert.Equal("", result[0].Jthumb);
    }

    [Fact]
    public void BllDataTableMappers_MapComputersList_MapsLegacyComputerFieldsOnly()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Pid");
        dt.Columns.Add("Pip");
        dt.Columns.Add("Pmachine");
        dt.Columns.Add("Plock");
        dt.Columns.Add("Pdate");
        dt.Columns.Add("Px");
        dt.Columns.Add("Py");
        dt.Columns.Add("Pm");
        dt.Rows.Add("441", "10.0.0.9", "pc-01", "1", "2024-05-04", "7", "8", "LabA");

        var result = LearnSite.BLL.BllDataTableMappers.MapComputersList(dt);

        Assert.Single(result);
        Assert.Equal(441, result[0].Pid);
        Assert.Equal("10.0.0.9", result[0].Pip);
        Assert.Equal("pc-01", result[0].Pmachine);
        Assert.True(result[0].Plock);
        Assert.Equal(new DateTime(2024, 5, 4), result[0].Pdate);
        Assert.Equal(0, result[0].Px);
        Assert.Equal(0, result[0].Py);
        Assert.Null(result[0].Pm);
    }

    [Fact]
    public void BllDataTableMappers_MapProblemsList_MapsLegacyProblemFieldsOnly()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Pid");
        dt.Columns.Add("Phid");
        dt.Columns.Add("Pnid");
        dt.Columns.Add("Ptitle");
        dt.Columns.Add("Pcode");
        dt.Columns.Add("Pouput");
        dt.Columns.Add("Pscore");
        dt.Columns.Add("Pdate");
        dt.Columns.Add("Psort");
        dt.Columns.Add("Pcid");
        dt.Rows.Add("451", "5", "61", "title", "print(1)", "1", "10", "2024-05-05", "3", "88");

        var result = LearnSite.BLL.BllDataTableMappers.MapProblemsList(dt);

        Assert.Single(result);
        Assert.Equal(451, result[0].Pid);
        Assert.Equal(5, result[0].Phid);
        Assert.Equal(61, result[0].Pnid);
        Assert.Equal("title", result[0].Ptitle);
        Assert.Equal("print(1)", result[0].Pcode);
        Assert.Equal("1", result[0].Pouput);
        Assert.Equal(10, result[0].Pscore);
        Assert.Equal(new DateTime(2024, 5, 5), result[0].Pdate);
        Assert.Null(result[0].Psort);
        Assert.Equal(0, result[0].Pcid);
    }

    [Fact]
    public void BllDataTableMappers_MapFlectionList_MapsFlectionFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Fid");
        dt.Columns.Add("Fcid");
        dt.Columns.Add("Fhid");
        dt.Columns.Add("Fcontent");
        dt.Columns.Add("Fdate");
        dt.Rows.Add("461", "62", "6", "reflection text", "2024-05-06");

        var result = LearnSite.BLL.BllDataTableMappers.MapFlectionList(dt);

        Assert.Single(result);
        Assert.Equal(461, result[0].Fid);
        Assert.Equal(62, result[0].Fcid);
        Assert.Equal(6, result[0].Fhid);
        Assert.Equal("reflection text", result[0].Fcontent);
        Assert.Equal(new DateTime(2024, 5, 6), result[0].Fdate);
    }

    [Fact]
    public void BllDataTableMappers_MapPfingerList_MapsPfingerFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Pid");
        dt.Columns.Add("Psnum");
        dt.Columns.Add("Pspd");
        dt.Columns.Add("Pyear");
        dt.Columns.Add("Pmonth");
        dt.Columns.Add("Pdate");
        dt.Columns.Add("Pdegree");
        dt.Columns.Add("Pgrade");
        dt.Columns.Add("Pterm");
        dt.Rows.Add("471", "2024071", "12.5", "2024", "5", "2024-05-07", "4", "6", "2");

        var result = LearnSite.BLL.BllDataTableMappers.MapPfingerList(dt);

        Assert.Single(result);
        Assert.Equal(471, result[0].Pid);
        Assert.Equal("2024071", result[0].Psnum);
        Assert.Equal(12.5m, result[0].Pspd);
        Assert.Equal(2024, result[0].Pyear);
        Assert.Equal(5, result[0].Pmonth);
        Assert.Equal(new DateTime(2024, 5, 7), result[0].Pdate);
        Assert.Equal(4, result[0].Pdegree);
        Assert.Equal(6, result[0].Pgrade);
        Assert.Equal(2, result[0].Pterm);
        Assert.Equal(0, result[0].Psid);
    }

    [Fact]
    public void BllDataTableMappers_MapDelStudentsList_MapsDeletedStudentFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Did");
        dt.Columns.Add("Dnum");
        dt.Columns.Add("Dyear");
        dt.Columns.Add("Dgrade");
        dt.Columns.Add("Dclass");
        dt.Columns.Add("Dname");
        dt.Columns.Add("Dsex");
        dt.Columns.Add("Daddress");
        dt.Columns.Add("Dphone");
        dt.Columns.Add("Dparents");
        dt.Columns.Add("Dheadtheacher");
        dt.Rows.Add("481", "2024081", "2024", "6", "1", "Tom", "M", "Addr", "123", "Parent", "Teacher");

        var result = LearnSite.BLL.BllDataTableMappers.MapDelStudentsList(dt);

        Assert.Single(result);
        Assert.Equal(481, result[0].Did);
        Assert.Equal("2024081", result[0].Dnum);
        Assert.Equal(2024, result[0].Dyear);
        Assert.Equal(6, result[0].Dgrade);
        Assert.Equal(1, result[0].Dclass);
        Assert.Equal("Tom", result[0].Dname);
        Assert.Equal("M", result[0].Dsex);
        Assert.Equal("Addr", result[0].Daddress);
        Assert.Equal("123", result[0].Dphone);
        Assert.Equal("Parent", result[0].Dparents);
        Assert.Equal("Teacher", result[0].Dheadtheacher);
    }

    [Fact]
    public void BllDataTableMappers_MapNotSignList_MapsNotSignFields()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Nid");
        dt.Columns.Add("Nnum");
        dt.Columns.Add("Ndate");
        dt.Columns.Add("Nyear");
        dt.Columns.Add("Nmonth");
        dt.Columns.Add("Nday");
        dt.Columns.Add("Nweek");
        dt.Columns.Add("Nnote");
        dt.Columns.Add("Ngrade");
        dt.Columns.Add("Nterm");
        dt.Rows.Add("491", "2024091", "2024-05-08", "2024", "5", "8", "Wed", "note", "6", "2");

        var result = LearnSite.BLL.BllDataTableMappers.MapNotSignList(dt);

        Assert.Single(result);
        Assert.Equal(491, result[0].Nid);
        Assert.Equal("2024091", result[0].Nnum);
        Assert.Equal(new DateTime(2024, 5, 8), result[0].Ndate);
        Assert.Equal(2024, result[0].Nyear);
        Assert.Equal(5, result[0].Nmonth);
        Assert.Equal(8, result[0].Nday);
        Assert.Equal("Wed", result[0].Nweek);
        Assert.Equal("note", result[0].Nnote);
        Assert.Equal(6, result[0].Ngrade);
        Assert.Equal(2, result[0].Nterm);
    }

    [Fact]
    public void BllDataTableMappers_MapPtyperList_PreservesLegacyPdegreeOverwriteBug()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Pid");
        dt.Columns.Add("Ptid");
        dt.Columns.Add("Psnum");
        dt.Columns.Add("Pscore");
        dt.Columns.Add("Pdate");
        dt.Columns.Add("Pip");
        dt.Columns.Add("Ptype");
        dt.Columns.Add("Pdegree");
        dt.Columns.Add("Pgrade");
        dt.Columns.Add("Pterm");
        dt.Rows.Add("501", "81", "2024101", "99", "2024-05-09", "10.0.0.10", "3", "7", "6", "2");

        var result = LearnSite.BLL.BllDataTableMappers.MapPtyperList(dt);

        Assert.Single(result);
        Assert.Equal(501, result[0].Pid);
        Assert.Equal(81, result[0].Ptid);
        Assert.Equal("2024101", result[0].Psnum);
        Assert.Equal(99, result[0].Pscore);
        Assert.Equal(new DateTime(2024, 5, 9), result[0].Pdate);
        Assert.Equal("10.0.0.10", result[0].Pip);
        Assert.Equal(7, result[0].Ptype);
        Assert.Null(result[0].Pdegree);
        Assert.Equal(6, result[0].Pgrade);
        Assert.Equal(2, result[0].Pterm);
        Assert.Equal(0, result[0].Psid);
    }

    // ================================================================
    // AIStudentExamSkillHelper 单元测试
    // ================================================================

    [Fact]
    public void AIStudentExamSkillHelper_GetDefaultSkillName_ReturnsExpected()
    {
        var name = LearnSite.Common.AIStudentExamSkillHelper.GetDefaultSkillName();
        Assert.Equal("AI测验评估助手", name);
    }

    [Fact]
    public void AIStudentExamSkillHelper_GetDefaultSkillPrompt_ContainsRequiredFields()
    {
        var prompt = LearnSite.Common.AIStudentExamSkillHelper.GetDefaultSkillPrompt();
        Assert.Contains("{{studentName}}", prompt);
        Assert.Contains("{{examTitle}}", prompt);
        Assert.Contains("{{score}}", prompt);
        Assert.Contains("{{questionCount}}", prompt);
        Assert.Contains("{{answerLog}}", prompt);
        Assert.Contains("summary", prompt);
        Assert.Contains("analysis", prompt);
        Assert.Contains("suggestions", prompt);
        Assert.Contains("learningLog", prompt);
    }

    [Fact]
    public void AIStudentExamSkillHelper_ReplaceTokens_ReplacesAllTokens()
    {
        string template = "学生{{studentName}}参加{{examTitle}}，得分{{score}}/{{questionCount}}，记录：{{answerLog}}";
        string result = LearnSite.Common.AIStudentExamSkillHelper.ReplaceTokens(
            template, "张三", "信息测验", 8, 10, "Q1:A Q2:B");

        Assert.Equal("学生张三参加信息测验，得分8/10，记录：Q1:A Q2:B", result);
    }

    [Fact]
    public void AIStudentExamSkillHelper_ReplaceTokens_NullTemplate_ReturnsEmpty()
    {
        string result = LearnSite.Common.AIStudentExamSkillHelper.ReplaceTokens(
            null, "张三", "信息测验", 5, 10, "log");
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void AIStudentExamSkillHelper_ReplaceTokens_NullValues_ReplacedWithEmpty()
    {
        string template = "{{studentName}}-{{examTitle}}-{{answerLog}}";
        string result = LearnSite.Common.AIStudentExamSkillHelper.ReplaceTokens(
            template, null, null, 0, 0, null);
        Assert.Equal("--", result);
    }

    [Fact]
    public void AIStudentExamSkillHelper_BuildFallbackSummary_HighScore()
    {
        // 85% -> "完成较好"
        string result = LearnSite.Common.AIStudentExamSkillHelper.BuildFallbackSummary("单元测试", 9, 10);
        Assert.Contains("完成较好", result);
        Assert.Contains("单元测试", result);
    }

    [Fact]
    public void AIStudentExamSkillHelper_BuildFallbackSummary_MediumScore()
    {
        // 70% -> "整体表现稳定"
        string result = LearnSite.Common.AIStudentExamSkillHelper.BuildFallbackSummary("单元测试", 7, 10);
        Assert.Contains("整体表现稳定", result);
    }

    [Fact]
    public void AIStudentExamSkillHelper_BuildFallbackSummary_LowScore()
    {
        // 40% -> "存在较多失分点"
        string result = LearnSite.Common.AIStudentExamSkillHelper.BuildFallbackSummary("单元测试", 4, 10);
        Assert.Contains("存在较多失分点", result);
    }

    [Fact]
    public void AIStudentExamSkillHelper_BuildFallbackSummary_ZeroQuestions()
    {
        string result = LearnSite.Common.AIStudentExamSkillHelper.BuildFallbackSummary("测试", 0, 0);
        Assert.Contains("缺少足够的答题明细", result);
    }

    [Fact]
    public void AIStudentExamSkillHelper_BuildFallbackAnalysis_ReturnsPlaceholderText()
    {
        string result = LearnSite.Common.AIStudentExamSkillHelper.BuildFallbackAnalysis("Q1:A");
        Assert.Contains("AI 评估暂不可用", result);
    }

    [Fact]
    public void AIStudentExamSkillHelper_BuildFallbackSuggestions_ReturnsThreeItems()
    {
        string result = LearnSite.Common.AIStudentExamSkillHelper.BuildFallbackSuggestions();
        Assert.Contains("1.", result);
        Assert.Contains("2.", result);
        Assert.Contains("3.", result);
    }

    [Fact]
    public void AIStudentExamSkillHelper_BuildFallbackLearningLog_IncludesAllFields()
    {
        string result = LearnSite.Common.AIStudentExamSkillHelper.BuildFallbackLearningLog(
            "李四", "期末考试", 7, 10, "Q1:正确 Q2:错误");

        Assert.Contains("李四", result);
        Assert.Contains("期末考试", result);
        Assert.Contains("7 / 10", result);
        Assert.Contains("Q1:正确 Q2:错误", result);
    }

    [Fact]
    public void AIStudentExamSkillHelper_BuildFallbackLearningLog_NullValues()
    {
        string result = LearnSite.Common.AIStudentExamSkillHelper.BuildFallbackLearningLog(
            null, null, 0, 0, null);

        Assert.Contains("学生：", result);
        Assert.Contains("测验：", result);
        Assert.Contains("0 / 0", result);
    }

    [Fact]
    public void AIStudentExamSkillHelper_ParseResponseObject_ValidJson()
    {
        string json = "{\"summary\":\"表现良好\",\"analysis\":\"掌握扎实\",\"suggestions\":\"继续努力\",\"learningLog\":\"已完成\"}";
        var obj = LearnSite.Common.AIStudentExamSkillHelper.ParseResponseObject(json);

        Assert.NotNull(obj);
        Assert.Equal("表现良好", obj["summary"]?.ToString());
        Assert.Equal("掌握扎实", obj["analysis"]?.ToString());
        Assert.Equal("继续努力", obj["suggestions"]?.ToString());
        Assert.Equal("已完成", obj["learningLog"]?.ToString());
    }

    [Fact]
    public void AIStudentExamSkillHelper_ParseResponseObject_JsonWithCodeBlock()
    {
        string content = "```json\n{\"summary\":\"不错\",\"analysis\":\"良好\"}\n```";
        var obj = LearnSite.Common.AIStudentExamSkillHelper.ParseResponseObject(content);

        Assert.NotNull(obj);
        Assert.Equal("不错", obj["summary"]?.ToString());
        Assert.Equal("良好", obj["analysis"]?.ToString());
    }

    [Fact]
    public void AIStudentExamSkillHelper_ParseResponseObject_JsonWithSurroundingText()
    {
        string content = "以下是评估结果：{\"summary\":\"完成较好\"} 感谢使用。";
        var obj = LearnSite.Common.AIStudentExamSkillHelper.ParseResponseObject(content);

        Assert.NotNull(obj);
        Assert.Equal("完成较好", obj["summary"]?.ToString());
    }

    [Fact]
    public void AIStudentExamSkillHelper_ParseResponseObject_InvalidJson_ReturnsNull()
    {
        var obj = LearnSite.Common.AIStudentExamSkillHelper.ParseResponseObject("这不是JSON");
        Assert.Null(obj);
    }

    [Fact]
    public void AIStudentExamSkillHelper_ParseResponseObject_EmptyString_ReturnsNull()
    {
        Assert.Null(LearnSite.Common.AIStudentExamSkillHelper.ParseResponseObject(""));
        Assert.Null(LearnSite.Common.AIStudentExamSkillHelper.ParseResponseObject(null));
    }

    [Fact]
    public void AIStudentExamSkillHelper_ParseResponseObject_OnlyCodeFence_ReturnsNull()
    {
        Assert.Null(LearnSite.Common.AIStudentExamSkillHelper.ParseResponseObject("```json\n```"));
    }
}
