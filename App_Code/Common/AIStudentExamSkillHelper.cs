using System;
using Newtonsoft.Json.Linq;

namespace LearnSite.Common
{
    public static class AIStudentExamSkillHelper
    {
        private const string DefaultSkillName = "AI测验评估助手";

        public static string GetDefaultSkillName()
        {
            return DefaultSkillName;
        }

        public static string GetDefaultSkillPrompt()
        {
            return "你是一名小学信息技术课程测验分析助手。请根据学生姓名 {{studentName}}、测验标题 {{examTitle}}、得分 {{score}}/{{questionCount}} 以及作答记录 {{answerLog}}，输出一个 JSON 对象，字段固定为 summary、analysis、suggestions、learningLog。summary 用一句话概括本次表现；analysis 用 2-4 条概括学生掌握情况；suggestions 用 2-3 条改进建议；learningLog 用于教师查看本次答题过程记录。不要输出 Markdown 代码块。";
        }

        public static string ReplaceTokens(string template, string studentName, string examTitle, int score, int questionCount, string answerLog)
        {
            string content = template ?? string.Empty;
            return content
                .Replace("{{studentName}}", studentName ?? string.Empty)
                .Replace("{{examTitle}}", examTitle ?? string.Empty)
                .Replace("{{score}}", score.ToString())
                .Replace("{{questionCount}}", questionCount.ToString())
                .Replace("{{answerLog}}", answerLog ?? string.Empty);
        }

        public static string BuildFallbackSummary(string examTitle, int score, int questionCount)
        {
            if (questionCount <= 0)
                return "本次测验已完成，但缺少足够的答题明细。";

            double percent = questionCount == 0 ? 0 : (double)score / Math.Max(questionCount, 1);
            if (percent >= 0.85)
                return "本次“" + examTitle + "”完成较好，基础知识掌握比较扎实。";
            if (percent >= 0.6)
                return "本次“" + examTitle + "”整体表现稳定，但仍有部分知识点需要巩固。";
            return "本次“" + examTitle + "”存在较多失分点，建议尽快针对薄弱知识进行复习。";
        }

        public static string BuildFallbackAnalysis(string answerLog)
        {
            return "AI 评估暂不可用，系统已保留本次作答记录。\n请结合下方学习日志与答题详情，查看学生具体薄弱点。";
        }

        public static string BuildFallbackSuggestions()
        {
            return "1. 先回看错题对应知识点。\n2. 结合课堂任务再次练习相近题型。\n3. 下一次提交前先逐题检查。";
        }

        public static string BuildFallbackLearningLog(string studentName, string examTitle, int score, int questionCount, string answerLog)
        {
            return "学生：" + (studentName ?? string.Empty) + "\n测验：" + (examTitle ?? string.Empty) + "\n得分：" + score + " / " + questionCount + "\n作答记录：\n" + (answerLog ?? string.Empty);
        }

        public static JObject ParseResponseObject(string responseContent)
        {
            string cleaned = (responseContent ?? string.Empty).Replace("```json", string.Empty).Replace("```", string.Empty).Trim();
            if (string.IsNullOrEmpty(cleaned))
                return null;

            int start = cleaned.IndexOf('{');
            int end = cleaned.LastIndexOf('}');
            if (start >= 0 && end > start)
                cleaned = cleaned.Substring(start, end - start + 1);

            try
            {
                return JObject.Parse(cleaned);
            }
            catch
            {
                return null;
            }
        }
    }
}
