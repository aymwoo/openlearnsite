<%@ WebHandler Language="C#" Class="gauge_generate" %>

using System;
using System.Linq;
using System.Text;
using System.Web;

public class gauge_generate : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.BufferOutput = false;
        context.Response.Charset = "utf-8";
        context.Response.ContentType = "text/event-stream";
        context.Response.CacheControl = "no-cache";
        context.Response.Headers["X-Accel-Buffering"] = "no";

        if (context.Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] == null)
        {
            WriteEvent(context, "failed", "{" + JsonPair("message", "未登录或登录已失效。") + "}");
            return;
        }

        string gaugeType = (context.Request["gtype"] ?? string.Empty).Trim();
        string gaugeTitle = HttpUtility.HtmlEncode((context.Request["gtitle"] ?? string.Empty).Trim());
        string mode = (context.Request["mode"] ?? "create").Trim().ToLower();
        string regenBehavior = (context.Request["regenBehavior"] ?? "replace").Trim().ToLower();
        int gaugeId = 0;
        if (string.IsNullOrEmpty(gaugeTitle))
        {
            WriteEvent(context, "failed", "{" + JsonPair("message", "请输入量规标题。") + "}");
            return;
        }

        try
        {
            LearnSite.BLL.AIGaugeGenerator.EnsureDefaultGaugeSkill();
            LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
            if (!tcook.IsExist())
            {
                WriteEvent(context, "failed", "{" + JsonPair("message", "教师身份无效，请重新登录。") + "}");
                return;
            }

            LearnSite.BLL.Gauge gaugeBll = new LearnSite.BLL.Gauge();
            bool clearBeforeSave = false;
            if (mode == "regen")
            {
                int.TryParse(context.Request["gid"], out gaugeId);
                LearnSite.Model.Gauge currentGauge = gaugeBll.GetModel(gaugeId);
                if (currentGauge == null || currentGauge.Ghid != tcook.Hid)
                {
                    WriteEvent(context, "failed", "{" + JsonPair("message", "未找到当前量规，或无权限重新生成。") + "}");
                    return;
                }
                gaugeType = currentGauge.Gtype ?? gaugeType;
                gaugeTitle = currentGauge.Gtitle ?? gaugeTitle;
                clearBeforeSave = regenBehavior != "append";
                WriteProgress(context, 1, clearBeforeSave ? "正在读取当前量规并准备重新生成..." : "正在读取当前量规并准备追加生成...");
            }
            else
            {
                WriteProgress(context, 1, "正在创建量规记录...");
                LearnSite.Model.Gauge gauge = new LearnSite.Model.Gauge();
                gauge.Ghid = tcook.Hid;
                gauge.Gcount = 0;
                gauge.Gdate = DateTime.Now;
                gauge.Gtitle = gaugeTitle;
                gauge.Gtype = gaugeType;
                gaugeId = gaugeBll.Add(gauge);
            }

            if (gaugeId <= 0)
            {
                WriteEvent(context, "failed", "{" + JsonPair("message", mode == "regen" ? "重新生成失败，请稍后重试。" : "量规创建失败，请稍后重试。") + "}");
                return;
            }

            LearnSite.BLL.AIGaugeGenerator generator = new LearnSite.BLL.AIGaugeGenerator();
            WriteProgress(context, 2, "正在调用 AI Provider “" + LearnSite.BLL.AIGaugeGenerator.GetDefaultProviderDisplayName() + "” 生成评价项...");
            LearnSite.BLL.GaugeGenerationResult result = generator.Generate(gaugeType, gaugeTitle, delegate(string stage, string message)
            {
                if (stage == "ai" || stage == "parse" || stage == "fallback" || stage == "ai_done")
                {
                    WriteProgress(context, 2, message);
                }
            });

            WriteProgress(context, 3, "正在写入量规项...");
            if (clearBeforeSave)
            {
                WriteProgress(context, 3, "AI 结果已生成，正在覆盖旧量规项...");
                LearnSite.BLL.GaugeItem clearBll = new LearnSite.BLL.GaugeItem();
                clearBll.DeleteByMgid(gaugeId);
            }
            else if (mode == "regen")
            {
                WriteProgress(context, 3, "AI 结果已生成，正在追加新量规项...");
                int currentCount = new LearnSite.BLL.GaugeItem().GetModelList("Mgid=" + gaugeId + " order by Msort asc").Count;
                for (int i = 0; i < result.Items.Count; i++)
                {
                    result.Items[i].Msort = currentCount + i + 1;
                }
            }
            int savedCount = generator.SaveItems(gaugeId, result.Items, delegate(string stage, string message)
            {
                if (stage == "save" || stage == "save_done")
                {
                    WriteProgress(context, 3, message);
                }
            });

            string msg = result.Message;
            if (savedCount > 0)
                msg += " 已写入 " + savedCount + " 条评价项。";
            else
                msg += " 评价项写入失败，请手动补充。";

            string itemPreview = string.Join("|", result.Items.Select(item => (item.Msort ?? 0).ToString() + "." + (item.Mitem ?? string.Empty) + "（" + (item.Mscore ?? 0).ToString() + "分）"));
            string redirectUrl = string.Format("teacher/gaugeitem.aspx?gid={0}&aimsg={1}&aiitems={2}&aifallback={3}&provider={4}",
                gaugeId,
                HttpUtility.UrlEncode(msg),
                HttpUtility.UrlEncode(itemPreview),
                result.UsedFallback ? "1" : "0",
                HttpUtility.UrlEncode(result.ProviderDisplayName ?? string.Empty));

            string doneJson = "{" +
                JsonPair("message", savedCount > 0 ? (mode == "regen" ? (clearBeforeSave ? "量规项已重新生成，正在刷新页面。" : "量规项已追加生成，正在刷新页面。") : "量规已创建，正在跳转到编辑页。") : (mode == "regen" ? "量规项处理未成功完成。" : "量规已创建，但评价项未成功写入。")) + "," +
                JsonPair("redirectUrl", ResolveAppRelative(context, "~/" + redirectUrl)) + "," +
                JsonPair("fallback", result.UsedFallback ? "1" : "0") +
                "}";
            WriteEvent(context, "done", doneJson);
        }
        catch (Exception ex)
        {
            WriteEvent(context, "failed", "{" + JsonPair("message", ex.Message.Replace("\"", "'").Replace("\r", " ").Replace("\n", " ")) + "}");
        }
    }

    private static void WriteProgress(HttpContext context, int step, string message)
    {
        string json = "{" + JsonPair("step", step.ToString()) + "," + JsonPair("message", message) + "}";
        WriteEvent(context, "progress", json);
    }

    private static void WriteEvent(HttpContext context, string eventName, string data)
    {
        context.Response.Write("event: " + eventName + "\n");
        context.Response.Write("data: " + data + "\n\n");
        context.Response.Flush();
    }

    private static string JsonPair(string key, string value)
    {
        return "\"" + EscapeJson(key) + "\":\"" + EscapeJson(value) + "\"";
    }

    private static string EscapeJson(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        StringBuilder sb = new StringBuilder(value.Length + 8);
        foreach (char ch in value)
        {
            switch (ch)
            {
                case '\\': sb.Append("\\\\"); break;
                case '"': sb.Append("\\\""); break;
                case '\r': sb.Append("\\r"); break;
                case '\n': sb.Append("\\n"); break;
                case '\t': sb.Append("\\t"); break;
                default: sb.Append(ch); break;
            }
        }
        return sb.ToString();
    }

    private static string ResolveAppRelative(HttpContext context, string appRelativeUrl)
    {
        return VirtualPathUtility.ToAbsolute(appRelativeUrl, context.Request.ApplicationPath);
    }

    public bool IsReusable
    {
        get { return false; }
    }
}
