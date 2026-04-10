using System;
using System.Collections;
using System.Collections.Generic;

namespace LearnSite.BLL
{
    public static class AIActivityPlanSkillBootstrap
    {
        public const string ActivityPlanSkillScope = "activity_plan_courseedit";

        public static void EnsureDefaultSkill()
        {
            try
            {
                object skillBll = Activator.CreateInstance(Type.GetType("LearnSite.BLL.AICustomSkill"));
                if (skillBll == null)
                {
                    return;
                }

                object existingSkills = skillBll.GetType().GetMethod("GetModelList").Invoke(skillBll, new object[]
                {
                    "SkillScope like '%" + ActivityPlanSkillScope + "%'"
                });
                if (HasAnyItem(existingSkills))
                {
                    return;
                }

                LearnSite.Model.AICustomSkill model = CreateDefaultSkillModel();
                skillBll.GetType().GetMethod("Add").Invoke(skillBll, new object[] { model });
            }
            catch
            {
            }
        }

        public static LearnSite.Model.AICustomSkill CreateDefaultSkillModel()
        {
            return new LearnSite.Model.AICustomSkill
            {
                SkillName = "活动计划助手（课程编辑）",
                PromptContent = "你是活动计划助手。请围绕教师当前输入的主题、年级、课时和教学目标，生成结构清晰、适合课堂实施的活动计划。已有课程内容仅作为支持背景，如果与当前输入冲突，以当前输入的主题为准。",
                SkillScope = ActivityPlanSkillScope,
                IsActive = true
            };
        }

        private static bool HasAnyItem(object value)
        {
            IEnumerable enumerable = value as IEnumerable;
            if (enumerable == null)
            {
                return false;
            }

            foreach (object item in enumerable)
            {
                if (item != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
