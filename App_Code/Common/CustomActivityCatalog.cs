using System;
using System.Collections.Generic;

namespace LearnSite.Common
{
    public sealed class CustomActivityMeta
    {
        public string Category { get; set; } = String.Empty;
        public string DisplayName { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public string StudentEntryUrl { get; set; } = String.Empty;
        public string StudentEntryFormat { get; set; } = String.Empty;
        public string EditFocus { get; set; } = String.Empty;
        public string FileType { get; set; } = String.Empty;
        public string IconUrl { get; set; } = String.Empty;
        public string BadgeBackground { get; set; } = String.Empty;
        public string BadgeForeground { get; set; } = String.Empty;
        public string ExampleMode { get; set; } = String.Empty;
    }

    public sealed class CustomActivityExampleResult
    {
        public bool IsValid { get; set; }
        public string ExampleValue { get; set; } = String.Empty;
        public string ErrorMessage { get; set; } = String.Empty;
    }

    public sealed class CustomActivityCatalog
    {
        private static readonly CustomActivityMeta DefaultMeta = Create(
            "11",
            "像素画",
            "学生端进入像素画工具，按活动说明完成像素创作、临摹或主题设计。",
            "~/student/pixel.aspx?lid=[活动目录编号]",
            "~/student/pixel.aspx?lid={0}",
            "重点维护活动说明、发布状态和评价标准，让学生在进入工具前先明确任务目标与提交要求。",
            "pxl",
            "~/images/pixel.png",
            "#dbeafe",
            "#1d4ed8",
            String.Empty);

        private static readonly IDictionary<string, CustomActivityMeta> ActivityMap = CreateActivityMap();

        public static bool IsCustomActivityType(string category)
        {
            int type;
            if (Int32.TryParse(category, out type))
            {
                return type == 11 || (type >= 17 && type <= 37);
            }
            return false;
        }

        public static CustomActivityMeta GetMeta(string category)
        {
            CustomActivityMeta meta;
            if (!String.IsNullOrEmpty(category) && ActivityMap.TryGetValue(category, out meta))
            {
                return meta;
            }
            return DefaultMeta;
        }

        public static string GetStudentEntryUrlByLid(string category, string lid)
        {
            CustomActivityMeta meta = GetMeta(category);
            return String.Format(meta.StudentEntryFormat, lid);
        }

        public static string GetFileType(string category)
        {
            return GetMeta(category).FileType;
        }

        public static CustomActivityExampleResult BuildExampleValue(string category, IEnumerable<string> selectedDeviceValues, string iframeUrl)
        {
            CustomActivityExampleResult result = new CustomActivityExampleResult();
            result.IsValid = true;
            result.ExampleValue = String.Empty;
            result.ErrorMessage = String.Empty;
            string exampleMode = GetMeta(category).ExampleMode;

            switch (exampleMode)
            {
                case "device-list":
                    if (selectedDeviceValues != null)
                    {
                        foreach (string deviceValue in selectedDeviceValues)
                        {
                            if (!String.IsNullOrEmpty(deviceValue))
                            {
                                result.ExampleValue += deviceValue + ",";
                            }
                        }
                    }
                    break;
                case "iframe-url":
                    string trimmedUrl = iframeUrl == null ? String.Empty : iframeUrl.Trim();
                    if (!IframeUrlHelper.IsAllowed(trimmedUrl))
                    {
                        result.IsValid = false;
                        result.ErrorMessage = "嵌入地址格式不正确，请填写 http/https 地址或站内相对路径！";
                        return result;
                    }
                    result.ExampleValue = trimmedUrl;
                    break;
            }

            return result;
        }

        public static string GetExampleSummary(string category, string exampleValue)
        {
            if (String.IsNullOrWhiteSpace(exampleValue))
            {
                return String.Empty;
            }

            string exampleMode = GetMeta(category).ExampleMode;

            switch (exampleMode)
            {
                case "device-list":
                    return "当前已启用设备：" + exampleValue.Trim(',').Replace(",", "、") + "。";
                case "iframe-url":
                    return "当前嵌入地址：" + exampleValue.Trim() + "。";
                default:
                    return String.Empty;
            }
        }

        private static CustomActivityMeta Create(string category, string displayName, string description, string studentEntryUrl, string studentEntryFormat, string editFocus, string fileType, string iconUrl, string badgeBackground, string badgeForeground, string exampleMode)
        {
            CustomActivityMeta meta = new CustomActivityMeta();
            meta.Category = category;
            meta.DisplayName = displayName;
            meta.Description = description;
            meta.StudentEntryUrl = studentEntryUrl;
            meta.StudentEntryFormat = studentEntryFormat;
            meta.EditFocus = editFocus;
            meta.FileType = fileType;
            meta.IconUrl = iconUrl;
            meta.BadgeBackground = badgeBackground;
            meta.BadgeForeground = badgeForeground;
            meta.ExampleMode = exampleMode;
            return meta;
        }

        private static IDictionary<string, CustomActivityMeta> CreateActivityMap()
        {
            Dictionary<string, CustomActivityMeta> map = new Dictionary<string, CustomActivityMeta>();
            Add(map, Create("17", "二维码", "学生端打开二维码工具，按活动说明生成、测试或分享二维码作品。", "~/student/qrcode.aspx?lid=[活动目录编号]", "~/student/qrcode.aspx?lid={0}", "重点维护活动说明、发布状态和评价标准，让学生明确二维码生成目标与应用场景。", "qrcode", "~/images/qrcode.png", "#dcfce7", "#166534", String.Empty));
            Add(map, Create("18", "在线文档", "学生端进入在线文档页面，适合写作、协作文稿和课堂记录。", "~/student/word.aspx?lid=[活动目录编号]", "~/student/word.aspx?lid={0}", "重点维护活动说明、发布状态和评价标准，让学生清楚写作要求与协作方式。", "word", "~/images/word.png", "#dbeafe", "#1d4ed8", String.Empty));
            Add(map, Create("19", "演示文稿", "学生端进入演示文稿工具，适合制作汇报、展示页和项目路演内容。", "~/student/pptist.aspx?lid=[活动目录编号]", "~/student/pptist.aspx?lid={0}", "重点维护活动说明、发布状态和评价标准，让学生明确演示结构和展示重点。", "pptist", "~/images/pptist.png", "#fee2e2", "#b91c1c", String.Empty));
            Add(map, Create("20", "海报设计", "学生端进入海报设计工具，适合完成宣传页、主题海报和版式设计任务。", "~/fabriceditor/poster.aspx?lid=[活动目录编号]", "~/fabriceditor/poster.aspx?lid={0}", "重点维护活动说明、发布状态和评价标准，让学生清楚主题风格、版式要求和提交标准。", "poster", "~/images/poster.png", "#ffedd5", "#c2410c", String.Empty));
            Add(map, Create("21", "风格迁移", "学生端进入风格迁移页面，通过上传图片和模型效果体验图像风格变化。", "~/student/style.aspx?lid=[活动目录编号]", "~/student/style.aspx?lid={0}", "重点维护活动说明、发布状态和评价标准，引导学生比较原图、风格图和生成效果。", "style", "~/images/style.png", "#ede9fe", "#6d28d9", String.Empty));
            Add(map, Create("22", "图像分类", "学生端进入图像分类页面，围绕图片识别结果开展人工智能体验活动。", "~/machine/imageclass.aspx?lid=[活动目录编号]", "~/machine/imageclass.aspx?lid={0}", "重点维护活动说明、发布状态和评价标准，引导学生观察分类结果与模型局限。", "mlimg", "~/images/mlimg.png", "#e0f2fe", "#0369a1", String.Empty));
            Add(map, Create("23", "人脸识别", "学生端进入人脸识别页面，结合拍照或上传图片完成识别体验。", "~/faceai/face.aspx?lid=[活动目录编号]", "~/faceai/face.aspx?lid={0}", "重点维护活动说明、发布状态和评价标准，提醒学生关注识别效果和隐私边界。", "face", "~/images/face.png", "#fae8ff", "#a21caf", String.Empty));
            Add(map, Create("24", "物联网MQTT", "学生端进入 MQTT 物联网页面，依据本页勾选的设备项进行控制、联动或数据采集。", "~/student/mqtt.aspx?lid=[活动目录编号]", "~/student/mqtt.aspx?lid={0}", "重点维护活动说明、发布状态、评价标准和设备选项，确保学生端只看到本课要用的硬件能力。", "mqtt", "~/images/mqtt.png", "#dcfce7", "#15803d", "device-list"));
            Add(map, Create("25", "手绘画布", "学生端进入手绘画布，适合结构草图、流程草图和创意表达。", "~/student/draw.aspx?lid=[活动目录编号]", "~/student/draw.aspx?lid={0}", "重点维护活动说明、发布状态和评价标准，明确学生要绘制的结构、草图或创意内容。", "excalidraw", "~/images/excalidraw.png", "#ede9fe", "#7c3aed", String.Empty));
            Add(map, Create("26", "推箱子地图", "学生端进入推箱子地图编辑工具，适合地图设计与关卡创作。", "~/student/sokoban.aspx?lid=[活动目录编号]", "~/student/sokoban.aspx?lid={0}", "重点维护活动说明、发布状态和评价标准，强调地图规则、难度和关卡设计目标。", "sokoban", "~/images/sokoban.png", "#fef3c7", "#b45309", String.Empty));
            Add(map, Create("27", "人工智能对话", "学生端进入人工智能对话页面，适合提示词体验、问答练习和角色对话。", "~/deepseek/deepseek.aspx?lid=[活动目录编号]", "~/deepseek/deepseek.aspx?lid={0}", "重点维护活动说明、发布状态和评价标准，引导学生围绕提示词、提问质量和总结输出开展任务。", "ai", "~/images/ai.png", "#dbeafe", "#1d4ed8", String.Empty));
            Add(map, Create("28", "语音合成", "学生端进入语音合成页面，适合播报文本、配音体验和多媒体创作。", "~/deepseek/speek.aspx?lid=[活动目录编号]", "~/deepseek/speek.aspx?lid={0}", "重点维护活动说明、发布状态和评价标准，让学生明确文本脚本、音色尝试和成品要求。", "speek", "~/images/speek.png", "#cffafe", "#0f766e", String.Empty));
            Add(map, Create("29", "文字识别", "学生端进入文字识别页面，适合图片取字、资料提取和 OCR 体验。", "~/deepseek/ocr.aspx?lid=[活动目录编号]", "~/deepseek/ocr.aspx?lid={0}", "重点维护活动说明、发布状态和评价标准，让学生明确识别对象、提取内容和后续整理要求。", "ocr", "~/images/ocr.png", "#fef9c3", "#a16207", String.Empty));
            Add(map, Create("30", "声音分析", "学生端进入声音分析页面，适合观察音量变化、声波特征和实时反馈。", "~/deepseek/soundlab.aspx?lid=[活动目录编号]", "~/deepseek/soundlab.aspx?lid={0}", "重点维护活动说明、发布状态和评价标准，引导学生围绕声音采集、现象观察和数据记录开展活动。", "sound", "~/images/sound.png", "#fee2e2", "#be123c", String.Empty));
            Add(map, Create("31", "井字棋", "学生端进入井字棋页面，适合博弈体验、规则理解和 AI 小游戏活动。", "~/deepseek/tic-tac-toe.aspx?lid=[活动目录编号]", "~/deepseek/tic-tac-toe.aspx?lid={0}", "重点维护活动说明、发布状态和评价标准，让学生清楚挑战规则、记录方式和策略思考要求。", "tic-tac-toe", "~/images/tic-tac-toe.png", "#fce7f3", "#be185d", String.Empty));
            Add(map, Create("32", "手写数字识别", "学生端进入手写数字识别页面，体验手写输入与模型预测结果。", "~/student/handnum.aspx?lid=[活动目录编号]", "~/student/handnum.aspx?lid={0}", "重点维护活动说明、发布状态和评价标准，引导学生观察输入差异与模型识别结果。", "handnum", "~/images/handnum.png", "#ede9fe", "#5b21b6", String.Empty));
            Add(map, Create("33", "Markdown写作", "学生端进入 Markdown 编辑器，适合项目说明、过程记录和反思日志。", "~/student/markdown.aspx?lid=[活动目录编号]", "~/student/markdown.aspx?lid={0}", "重点维护活动说明、发布状态和评价标准，让学生清楚文档结构、格式要求和输出目标。", "markdown", "~/images/markdown.png", "#e2e8f0", "#334155", String.Empty));
            Add(map, Create("34", "嵌入本地网页", "学生端直接加载本页配置的嵌入地址，在指定网页中开展学习任务。", "~/student/iframe.aspx?lid=[活动目录编号]", "~/student/iframe.aspx?lid={0}", "重点维护活动说明、发布状态、评价标准和嵌入地址，保证学生打开后能直接进入目标网页。", "iframe", "~/images/iframe.png", "#dbeafe", "#1e40af", "iframe-url"));
            Add(map, Create("35", "文生图", "学生端进入文生图页面，围绕提示词设计、图像生成和作品比较开展活动。", "~/deepseek/aidraw.aspx?lid=[活动目录编号]", "~/deepseek/aidraw.aspx?lid={0}", "重点维护活动说明、发布状态和评价标准，让学生明确提示词设计目标和作品对比方式。", "text-to-image", "~/images/text-to-image.png", "#fae8ff", "#9333ea", String.Empty));
            Add(map, Create("36", "素材库", "学生端进入素材库页面，适合检索、整理和引用课程所需资源。", "~/student/webstore.aspx?lid=[活动目录编号]", "~/student/webstore.aspx?lid={0}", "重点维护活动说明、发布状态和评价标准，让学生清楚资源检索范围和整理要求。", "web", "~/images/web.png", "#cffafe", "#0f766e", String.Empty));
            Add(map, Create("37", "网站设计", "学生端进入网站设计页面，适合网页结构搭建、内容编辑和站点发布练习。", "~/student/website.aspx?lid=[活动目录编号]", "~/student/website.aspx?lid={0}", "重点维护活动说明、发布状态和评价标准，让学生明确站点结构、页面要求和展示目标。", "website", "~/images/website.png", "#dbeafe", "#1d4ed8", String.Empty));
            return map;
        }

        private static void Add(IDictionary<string, CustomActivityMeta> map, CustomActivityMeta meta)
        {
            map[meta.Category] = meta;
        }
    }
}
