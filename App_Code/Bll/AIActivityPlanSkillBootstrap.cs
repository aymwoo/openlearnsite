using System;
using System.Collections.Generic;
using System.Linq;

namespace LearnSite.BLL
{
    public static class AIActivityPlanSkillBootstrap
    {
        public const string ActivityPlanSkillScope = "activity_plan_courseedit";

        public static void EnsureDefaultSkill()
        {
            try
            {
                List<LearnSite.Model.AICustomSkill> scopedSkills = GetScopedSkills();
                if (HasAnyActiveScopedSkill(scopedSkills))
                {
                    return;
                }

                object skillBll = CreateSkillBll();
                if (skillBll == null)
                {
                    return;
                }

                LearnSite.Model.AICustomSkill reusableSkill = GetScopedSkillToActivate(scopedSkills);
                if (reusableSkill != null)
                {
                    reusableSkill.IsActive = true;
                    skillBll.GetType().GetMethod("Update").Invoke(skillBll, new object[] { reusableSkill });
                    return;
                }

                skillBll.GetType().GetMethod("Add").Invoke(skillBll, new object[] { CreateDefaultSkillModel() });
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
                PromptContent = "你是面向一线教师的活动计划助手。请围绕教师当前输入的主题、年级、课时和教学目标，给出适合课堂实施、便于教师审核的活动计划草案。已有课程内容仅作为支持背景，如果与当前输入冲突，以当前输入的主题为准。",
                SkillScope = ActivityPlanSkillScope,
                IsActive = true
            };
        }

        internal static bool HasAnyActiveScopedSkill(IEnumerable<LearnSite.Model.AICustomSkill> skills)
        {
            if (skills == null)
            {
                return false;
            }

            foreach (LearnSite.Model.AICustomSkill skill in skills)
            {
                if (skill != null && skill.IsActive && ScopeMatches(skill.SkillScope))
                {
                    return true;
                }
            }

            return false;
        }

        internal static LearnSite.Model.AICustomSkill GetScopedSkillToActivate(IEnumerable<LearnSite.Model.AICustomSkill> skills)
        {
            if (skills == null)
            {
                return null;
            }

            string defaultSkillName = CreateDefaultSkillModel().SkillName;
            LearnSite.Model.AICustomSkill defaultScopedSkill = skills.FirstOrDefault(skill => skill != null
                && ScopeMatches(skill.SkillScope)
                && string.Equals(skill.SkillName, defaultSkillName, StringComparison.Ordinal));
            if (defaultScopedSkill != null)
            {
                return defaultScopedSkill;
            }

            return skills.FirstOrDefault(skill => skill != null && ScopeMatches(skill.SkillScope));
        }

        private static object CreateSkillBll()
        {
            Type skillBllType = Type.GetType("LearnSite.BLL.AICustomSkill");
            if (skillBllType == null)
            {
                return null;
            }

            return Activator.CreateInstance(skillBllType);
        }

        private static List<LearnSite.Model.AICustomSkill> GetScopedSkills()
        {
            object skillBll = CreateSkillBll();
            if (skillBll == null)
            {
                return null;
            }

            return skillBll.GetType().GetMethod("GetModelList").Invoke(skillBll, new object[]
            {
                "SkillScope like '%" + ActivityPlanSkillScope + "%' "
            }) as List<LearnSite.Model.AICustomSkill>;
        }

        private static bool ScopeMatches(string skillScope)
        {
            return !string.IsNullOrEmpty(skillScope)
                && skillScope.IndexOf(ActivityPlanSkillScope, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
