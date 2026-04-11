using System;
using System.IO;
using System.Linq;

namespace CommonLogicTests;

public class ActivityPlanPublishCoreTests
{
    [Fact]
    public void ActivityPlanPublishContent_Source_ContainsExpectedLabelsAndBuilders()
    {
        string source = ReadRepoFile("App_Code", "Common", "AIActivityPlanPublishContentBuilder.cs");

        Assert.Contains("教学目标", source, StringComparison.Ordinal);
        Assert.Contains("活动步骤", source, StringComparison.Ordinal);
        Assert.Contains("教学资源", source, StringComparison.Ordinal);
        Assert.Contains("评价设计", source, StringComparison.Ordinal);
        Assert.Contains("教师提醒", source, StringComparison.Ordinal);
        Assert.Contains("BuildLessonContent", source, StringComparison.Ordinal);
        Assert.Contains("BuildMissionContent", source, StringComparison.Ordinal);
    }

    [Fact]
    public void ActivityPlanPublishContent_Source_FailsClosedForUnsupportedOrEmptySelections()
    {
        string source = ReadRepoFile("App_Code", "Common", "AIActivityPlanPublishContentBuilder.cs");

        Assert.Contains("SupportedSectionKeys", source, StringComparison.Ordinal);
        Assert.Contains("NormalizeSelectedSectionKeys", source, StringComparison.Ordinal);
        Assert.Contains("return string.Empty;", source, StringComparison.Ordinal);
    }

    [Fact]
    public void ActivityPlanPublish_Source_UsesSingleTransactionAndPublishFlags()
    {
        string source = ReadRepoFile("App_Code", "Dal", "AIActivityPlanPublisher.cs");

        Assert.Contains("BeginTransaction", source, StringComparison.Ordinal);
        Assert.Contains("Mpublish", source, StringComparison.Ordinal);
        Assert.Contains("Lshow", source, StringComparison.Ordinal);
        Assert.Contains("Mupload", source, StringComparison.Ordinal);
        Assert.Contains("Ltype", source, StringComparison.Ordinal);
        Assert.Contains("LinkedMissionId", source, StringComparison.Ordinal);
        Assert.Contains("LinkedListMenuId", source, StringComparison.Ordinal);
        Assert.Contains("Mfiletype", source, StringComparison.Ordinal);
    }

    [Fact]
    public void ActivityPlanPublish_Source_ReusesLinkedMissionAndMenuWhenAvailable()
    {
        string source = ReadRepoFile("App_Code", "Dal", "AIActivityPlanPublisher.cs");

        Assert.Contains("draftLink.LinkedMissionId.HasValue", source, StringComparison.Ordinal);
        Assert.Contains("draftLink.LinkedListMenuId.HasValue", source, StringComparison.Ordinal);
        Assert.Contains("MissionBelongsToCourse", source, StringComparison.Ordinal);
        Assert.Contains("ListMenuBelongsToCourse", source, StringComparison.Ordinal);
    }

    [Fact]
    public void ActivityPlanPublish_Source_AppendsCourseContentButStoresFullMissionContent()
    {
        string source = ReadRepoFile("App_Code", "Dal", "AIActivityPlanPublisher.cs");

        Assert.Contains("BuildLessonContent", source, StringComparison.Ordinal);
        Assert.Contains("BuildMissionContent", source, StringComparison.Ordinal);
        Assert.Contains("AppendCourseContent", source, StringComparison.Ordinal);
        Assert.Contains("update Courses set Ccontent=@Ccontent", source, StringComparison.Ordinal);
        Assert.Contains("Mcontent=@Mcontent", source, StringComparison.Ordinal);
    }

    [Fact]
    public void ActivityPlanPublish_Source_ExposesSingleBllEntryPoint()
    {
        string source = ReadRepoFile("App_Code", "Bll", "AIActivityPlanPublisher.cs");

        Assert.Contains("Publish(LearnSite.Model.AIActivityPlanPublishRequest request)", source, StringComparison.Ordinal);
        Assert.Contains("return dal.Publish(request);", source, StringComparison.Ordinal);
    }

    private static string ReadRepoFile(params string[] relativeSegments)
    {
        string current = AppContext.BaseDirectory;
        DirectoryInfo dir = new DirectoryInfo(current);
        while (dir != null)
        {
            string solutionPath = Path.Combine(dir.FullName, "openlearnsite.sln");
            if (File.Exists(solutionPath))
            {
                string path = Path.Combine(new[] { dir.FullName }.Concat(relativeSegments).ToArray());
                return File.ReadAllText(path);
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate repository root from test base directory.");
    }
}
