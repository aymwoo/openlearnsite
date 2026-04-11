using System;
using System.IO;
using System.Linq;

namespace CommonLogicTests;

public class ActivityPlanDraftLinkageTests
{
    [Fact]
    public void ActivityPlanDraftMigration_Source_AddsLinkedMissionColumnsAndUpgradeGuards()
    {
        string source = ReadRepoFile("App_Code", "Utility", "UpdateGrade.cs");

        Assert.Contains("[LinkedMissionId] INT NULL", source, StringComparison.Ordinal);
        Assert.Contains("[LinkedListMenuId] INT NULL", source, StringComparison.Ordinal);
        Assert.Contains("ColumnExists(\"CourseActivityPlanDraft\", \"LinkedMissionId\")", source, StringComparison.Ordinal);
        Assert.Contains("ColumnExists(\"CourseActivityPlanDraft\", \"LinkedListMenuId\")", source, StringComparison.Ordinal);
    }

    [Fact]
    public void ActivityPlanSavedDraft_BuildAndParseRecord_RoundTripsLinkedIds()
    {
        var draft = BuildValidActivityPlanDraft();
        var record = LearnSite.Common.AIActivityPlanSavedDraftHelper.BuildRecord(12, 5, "认识分数", "五年级", "40分钟", "理解分数含义", "旧内容", draft, 101, 202);

        Assert.NotNull(record);
        Assert.Equal(101, record.LinkedMissionId);
        Assert.Equal(202, record.LinkedListMenuId);

        var loaded = LearnSite.Common.AIActivityPlanSavedDraftHelper.ParseRecord(record, 12, 5);

        Assert.NotNull(loaded);
        Assert.Equal(101, loaded.LinkedMissionId);
        Assert.Equal(202, loaded.LinkedListMenuId);
    }

    [Fact]
    public void ActivityPlanSavedDraft_Source_MapsLinkedIdsInHelperAndDal()
    {
        string helperSource = ReadRepoFile("App_Code", "Common", "AIActivityPlanSavedDraftHelper.cs");
        string dalSource = ReadRepoFile("App_Code", "Dal", "CourseActivityPlanDraft.cs");

        Assert.Contains("LinkedMissionId = linkedMissionId", helperSource, StringComparison.Ordinal);
        Assert.Contains("LinkedListMenuId = linkedListMenuId", helperSource, StringComparison.Ordinal);
        Assert.Contains("LinkedMissionId = record.LinkedMissionId", helperSource, StringComparison.Ordinal);
        Assert.Contains("LinkedListMenuId = record.LinkedListMenuId", helperSource, StringComparison.Ordinal);
        Assert.Contains("LinkedMissionId", dalSource, StringComparison.Ordinal);
        Assert.Contains("LinkedListMenuId", dalSource, StringComparison.Ordinal);
        Assert.Contains("update CourseActivityPlanDraft set", dalSource, StringComparison.Ordinal);
        Assert.Contains("insert into CourseActivityPlanDraft", dalSource, StringComparison.Ordinal);
    }

    private static LearnSite.Common.ActivityPlanDraft BuildValidActivityPlanDraft()
    {
        return new LearnSite.Common.ActivityPlanDraft
        {
            TeachingGoals = new System.Collections.Generic.List<string> { "理解分数含义", "能结合情境表达分数" },
            ActivitySteps = new System.Collections.Generic.List<LearnSite.Common.ActivityPlanDraftStep>
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
            Resources = new System.Collections.Generic.List<string> { "分数卡片" },
            Assessment = new System.Collections.Generic.List<string> { "观察学生是否能正确说出二分之一" },
            TeacherReminder = "注意让学生先说生活例子。"
        };
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
