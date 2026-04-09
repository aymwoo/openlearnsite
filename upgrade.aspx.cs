using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;

public partial class UpGrade : System.Web.UI.Page
{
    protected string CurrentDbVersion = "未知";
    protected string TargetDbVersion = "1.9.1.1";
    protected string UpgradeMode = "未知";
    protected string UpgradeDecision = "待检查";
    protected string RiskLevel = "中";
    protected string ConnectedDatabaseName = "未知";
    protected string LastAnalyzeTime = "尚未检查";
    protected string UpgradeSummaryHtml = "";
    protected string UpgradeRiskHtml = "";
    protected string PendingMigrationHtml = "";
    protected string PendingStructureHtml = "";
    protected string PendingDataHtml = "";
    protected string PendingPerformanceHtml = "";
    protected bool CanUpgrade = false;
    private int CollapsibleListSeed = 0;
    private bool? DatabaseAvailable;

    protected void BtnAnalyze_Click(object sender, EventArgs e)
    {
        checkdatabase();
        Labelmsg.Text = "已重新完成数据库升级前检查，请根据结果判断是否执行升级。";
    }

    protected void BtnExportReport_Click(object sender, EventArgs e)
    {
        checkdatabase();

        string html = BuildReportHtml();
        string fileName = "learnsite-upgrade-report-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".html";
        Response.Clear();
        Response.ContentType = "text/html; charset=utf-8";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
        Response.Write(html);
        Response.End();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        TargetDbVersion = GetDisplayTargetVersion();
        if (IsDatabaseAvailable())
        {
            CurrentDbVersion = LearnSite.DBUtility.UpdateGrade.GetCurrentVersion();
        }
        else
        {
            CurrentDbVersion = "无法连接";
        }

        if (!IsPostBack)
            checkdatabase();
    }
    protected void Btnupgrade_Click(object sender, EventArgs e)
    {
        if (LearnSite.DBUtility.UpdateGrade.TableExistCheck())
        {
            bool needLegacyUpgrade = !LearnSite.DBUtility.UpdateGrade.VersionCheck();
            List<LearnSite.DBUtility.MigrationEntry> pendingMigrations = LearnSite.DBUtility.DbMigration.GetPendingMigrations();
            bool hasPendingMigrations = pendingMigrations != null && pendingMigrations.Count > 0;
            if (needLegacyUpgrade || hasPendingMigrations)
            {
                AnalyzeUpgradeState();
                if (!CanUpgrade)
                {
                    Labelmsg.Text = "当前数据库未通过升级前检查，请先按提示处理后再执行升级。";
                    return;
                }

                if (needLegacyUpgrade)
                {
                    Oldupdate();//旧网站更新
                    LearnSite.DBUtility.UpdateGrade.UpdateTableEnglish();
                    LearnSite.DBUtility.UpdateGrade.UpdateTable1500();
                    LearnSite.DBUtility.UpdateGrade.UpdateTable1600();
                    LearnSite.DBUtility.UpdateGrade.UpdateTable1700();
                    LearnSite.DBUtility.UpdateGrade.UpdateTable1800();
                    LearnSite.DBUtility.UpdateGrade.UpdateTable1900();
                    LearnSite.DBUtility.UpdateGrade.UpdateTable1910();
                }
                List<LearnSite.DBUtility.MigrationResult> pendingResults = LearnSite.DBUtility.DbMigration.RunAllPending();

                StringBuilder resultSummary = new StringBuilder();
                foreach (LearnSite.DBUtility.MigrationResult result in pendingResults)
                {
                    if (!result.Success)
                    {
                        resultSummary.Append(" 迁移 ").Append(result.Version).Append(" 失败：").Append(result.Message);
                        break;
                    }
                }

                Labelmsg.Text = "升级完毕，请删除本页面！以免数据库出错！" + resultSummary.ToString();

                string ch = "数据库已经更新到最新版，点击跳回教师首页！";
                LearnSite.Common.CookieHelp.ClearTeacherCookies();
                LearnSite.Common.CookieHelp.ClearStudentCookies();//教师退出的话把本机模拟学生角色登录的学生平台也退出
                System.Threading.Thread.Sleep(500);
                LearnSite.Common.WordProcess.AlertJump(ch, "teacher/index.aspx", this.Page);
                return;
            }
            else
            {                
                string ch = "已经更新过了！请点击跳回教师首页！\n若要求运行一次则补丁已经修正！";
                LearnSite.Common.WordProcess.AlertJump(ch, "teacher/index.aspx", this.Page);
                return;
            }
        }
        else
        {
            string ch = "很抱歉您创建的数据库为空，不存在数据表，请仔细查看或请求帮助！";
            LearnSite.Common.WordProcess.Alert(ch, this.Page);
        }
    }

    private string GetDisplayTargetVersion()
    {
        List<LearnSite.DBUtility.MigrationEntry> all = LearnSite.DBUtility.DbMigration.AllMigrations;
        if (all != null && all.Count > 0)
        {
            return all[all.Count - 1].Version;
        }
        return LearnSite.DBUtility.UpdateGrade.GetTargetVersion();
    }

    private void AnalyzeUpgradeState()
    {
        Btnupgrade.Enabled = true;
        Btnupgrade.CssClass = "upgrade-btn-primary";
        ConnectedDatabaseName = GetCurrentDatabaseName();
        LastAnalyzeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        List<string> summary = new List<string>();
        List<string> risks = new List<string>();
        List<string> pending = new List<string>();
        List<string> pendingStructure = new List<string>();
        List<string> pendingData = new List<string>();
        List<string> pendingPerformance = new List<string>();

        bool hasStudents = LearnSite.DBUtility.DbHelperSQL.TabExists("Students");
        bool hasEnglish = LearnSite.DBUtility.DbHelperSQL.TabExists("English");
        bool hasMigrationTable = LearnSite.DBUtility.DbHelperSQL.TabExists("_DbMigrations");
        bool hasAICustomSkill = LearnSite.DBUtility.DbHelperSQL.TabExists("AICustomSkill");
        bool hasAIStudentExamAssessment = LearnSite.DBUtility.DbHelperSQL.TabExists("AIStudentExamAssessment");
        bool hasSurveyEnableAi = LearnSite.DBUtility.DbHelperSQL.ColumnExists("Survey", "Venableai");
        bool hasKseconds = LearnSite.DBUtility.DbHelperSQL.ColumnExists("MenuWorks", "Kseconds");
        bool hasIndex = false;
        try
        {
            object indexCount = LearnSite.DBUtility.DbHelperSQL.GetSingle("select count(1) from sys.indexes where name='IX_MenuWorks_Klid_Ksid' and object_id = object_id('MenuWorks')");
            hasIndex = indexCount != null && Convert.ToInt32(indexCount) > 0;
        }
        catch
        {
            hasIndex = false;
        }

        UpgradeMode = hasMigrationTable ? "新版本迁移库" : (hasStudents ? "旧版本数据库" : "空库 / 未初始化数据库");

        if (!hasStudents)
        {
            UpgradeDecision = "不能直接升级";
            RiskLevel = "高";
            CanUpgrade = false;
            Btnupgrade.Enabled = false;
            Btnupgrade.CssClass = "upgrade-btn-primary upgrade-btn-disabled";
            summary.Add("当前数据库缺少 `Students` 基础表，不能按覆盖升级流程直接升级。请先创建数据表，再执行更新。");
            risks.Add("如果数据库为空或连接错库，直接执行升级不会得到可用系统。");
        }
        else
        {
            summary.Add("已识别到旧系统数据库，可执行覆盖升级检查。系统将优先补齐历史字段，再执行新版本迁移。");
            CanUpgrade = true;
            UpgradeDecision = "可以升级";
            RiskLevel = "中";
        }

        if (!hasEnglish)
        {
            summary.Add("缺少 `English` 表或词库数据，升级时会补建并初始化英文词典。该步骤执行时间可能略长。");
            risks.Add("如果数据库用户权限不足，词库初始化可能失败。建议使用具备建表和写入权限的账号。");
        }

        if (!hasAICustomSkill)
        {
            summary.Add("缺少 `AICustomSkill` 表，升级后将补齐 AI 自定义技能配置。");
        }

        if (!hasAIStudentExamAssessment)
        {
            summary.Add("缺少 `AIStudentExamAssessment` 表，升级后学生测验 AI 评估才能入库并在教师端查看。");
        }

        if (!hasSurveyEnableAi)
        {
            summary.Add("旧版 `Survey` 测验尚未具备独立 AI 评价开关字段，升级后才能按单个测验控制是否启用 AI 评价。");
        }

        if (!hasKseconds)
        {
            summary.Add("`MenuWorks.Kseconds` 秒级停留字段尚未存在，升级时会补齐并尝试从 `Ktime` 回填历史值。");
            risks.Add("回填 `Kseconds` 会扫描 `MenuWorks` 历史数据。若数据量较大，升级页面可能停留较久。");
        }

        if (!hasIndex)
        {
            summary.Add("缺少 `IX_MenuWorks_Klid_Ksid` 查询优化索引，升级时会自动创建。");
        }

        List<LearnSite.DBUtility.MigrationEntry> pendingMigrations = LearnSite.DBUtility.DbMigration.GetPendingMigrations();
        foreach (LearnSite.DBUtility.MigrationEntry migration in pendingMigrations)
        {
            string text = migration.Version + " - " + migration.Description;
            pending.Add(text);
            string description = migration.Description ?? string.Empty;
            if (description.IndexOf("索引", StringComparison.OrdinalIgnoreCase) >= 0 || description.IndexOf("性能", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                pendingPerformance.Add(text);
            }
            else if (description.IndexOf("默认", StringComparison.OrdinalIgnoreCase) >= 0 || description.IndexOf("词库", StringComparison.OrdinalIgnoreCase) >= 0 || description.IndexOf("初始化", StringComparison.OrdinalIgnoreCase) >= 0 || description.IndexOf("数据", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                pendingData.Add(text);
            }
            else
            {
                pendingStructure.Add(text);
            }
        }

        if (pending.Count == 0)
        {
            pending.Add("当前未检测到新的数据库迁移项，若仍需执行升级，系统将只做旧版本兼容补丁检查。");
        }

        if (hasMigrationTable && pendingMigrations.Count == 0 && LearnSite.DBUtility.UpdateGrade.VersionCheck())
        {
            UpgradeDecision = "数据库已是最新";
            RiskLevel = "低";
            CanUpgrade = true;
            summary.Add("已检测到迁移记录表 `_DbMigrations` 且当前版本已是最新，执行升级主要用于补跑一次修复性补丁。");
        }

        if (!hasMigrationTable)
        {
            risks.Add("当前数据库还没有 `_DbMigrations` 迁移跟踪表。升级时系统会自动创建该表，并把新迁移按顺序补齐。");
        }

        risks.Add("强烈建议先做数据库备份，再执行升级。若你是直接解压覆盖旧站点，必须确认 `web.config` 仍指向正确的旧库。");
        risks.Add("升级期间不要同时让教师或学生继续操作系统，避免数据写入和结构变更并发。");

        UpgradeSummaryHtml = BuildCollapsibleListHtml(summary, "upgrade-check-list", 5);
        UpgradeRiskHtml = BuildCollapsibleListHtml(risks, "upgrade-risk-list", 4);
        PendingMigrationHtml = BuildCollapsibleListHtml(pending, "upgrade-pending-list", 6);
        PendingStructureHtml = BuildCollapsibleListHtml(pendingStructure, "upgrade-pending-list", 5);
        PendingDataHtml = BuildCollapsibleListHtml(pendingData, "upgrade-pending-list", 5);
        PendingPerformanceHtml = BuildCollapsibleListHtml(pendingPerformance, "upgrade-pending-list", 5);
    }

    private string BuildListHtml(List<string> items, string cssClass)
    {
        if (items == null || items.Count == 0)
        {
            return "<div class='" + cssClass + "'><div class='upgrade-check-empty'>暂无数据</div></div>";
        }

        StringBuilder sb = new StringBuilder();
        sb.Append("<ul class='").Append(cssClass).Append("'>");
        foreach (string item in items)
        {
            sb.Append("<li>").Append(FormatListItem(item, cssClass)).Append("</li>");
        }
        sb.Append("</ul>");
        return sb.ToString();
    }

    private string BuildCollapsibleListHtml(List<string> items, string cssClass, int previewCount)
    {
        if (items == null || items.Count == 0)
        {
            return BuildListHtml(items, cssClass);
        }

        if (items.Count <= previewCount)
        {
            return BuildListHtml(items, cssClass);
        }

        CollapsibleListSeed++;
        string hiddenId = "upgrade-list-more-" + CollapsibleListSeed;
        int remain = items.Count - previewCount;

        StringBuilder sb = new StringBuilder();
        sb.Append("<div class='upgrade-collapsible'>");
        sb.Append("<ul class='").Append(cssClass).Append("'>");
        for (int i = 0; i < previewCount; i++)
        {
            sb.Append("<li>").Append(FormatListItem(items[i], cssClass)).Append("</li>");
        }
        sb.Append("</ul>");
        sb.Append("<div id='").Append(hiddenId).Append("' class='upgrade-collapsible__more'>");
        sb.Append("<ul class='").Append(cssClass).Append("'>");
        for (int i = previewCount; i < items.Count; i++)
        {
            sb.Append("<li>").Append(FormatListItem(items[i], cssClass)).Append("</li>");
        }
        sb.Append("</ul></div>");
        sb.Append("<button type='button' class='upgrade-collapse-btn' data-open='0' onclick=\"toggleUpgradeList(this, '").Append(hiddenId).Append("', ").Append(remain).Append(")\"><span class='upgrade-collapse-btn__arrow'>▶</span><span>展开全部（剩余 ").Append(remain).Append(" 项）</span></button>");
        sb.Append("</div>");
        return sb.ToString();
    }

    private string FormatListItem(string item, string cssClass)
    {
        string text = Server.HtmlEncode(item ?? string.Empty);
        if (cssClass == "upgrade-risk-list")
        {
            string[] dangerWords = new string[] { "不能直接升级", "为空", "连接错库", "必须", "备份", "不要同时", "权限不足" };
            for (int i = 0; i < dangerWords.Length; i++)
            {
                string word = Server.HtmlEncode(dangerWords[i]);
                text = text.Replace(word, "<strong>" + word + "</strong>");
            }
        }
        return text;
    }

    private string GetCurrentDatabaseName()
    {
        try
        {
            string myconnstr = System.Configuration.ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
            string[] constr = LearnSite.DBUtility.DbLinkEdit.ReadSqlConfig(myconnstr);
            if (constr != null && constr.Length > 1 && !string.IsNullOrEmpty(constr[1]))
            {
                return constr[1];
            }
        }
        catch
        {
        }
        return "未知";
    }

    private string BuildReportHtml()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<!DOCTYPE html><html><head><meta charset='utf-8' /><title>LearnSite 升级前检查报告</title>");
        sb.Append("<style>body{font-family:Segoe UI,Microsoft YaHei,Arial,sans-serif;padding:24px;color:#0f172a;}h1{font-size:28px;}h2{margin-top:24px;font-size:18px;}ul{line-height:1.9;} .meta{display:grid;grid-template-columns:repeat(auto-fit,minmax(220px,1fr));gap:12px;margin:20px 0;} .card{border:1px solid #dbeafe;border-radius:12px;padding:14px 16px;background:#f8fbff;} .label{display:block;font-size:12px;font-weight:700;color:#64748b;margin-bottom:6px;text-transform:uppercase;} .value{font-size:18px;font-weight:800;color:#1d4ed8;} </style>");
        sb.Append("</head><body>");
        sb.Append("<h1>LearnSite 升级前检查报告</h1>");
        sb.Append("<div class='meta'>");
        sb.Append("<div class='card'><span class='label'>数据库名称</span><div class='value'>").Append(Server.HtmlEncode(ConnectedDatabaseName)).Append("</div></div>");
        sb.Append("<div class='card'><span class='label'>检查时间</span><div class='value'>").Append(Server.HtmlEncode(LastAnalyzeTime)).Append("</div></div>");
        sb.Append("<div class='card'><span class='label'>当前版本</span><div class='value'>").Append(Server.HtmlEncode(CurrentDbVersion)).Append("</div></div>");
        sb.Append("<div class='card'><span class='label'>目标版本</span><div class='value'>").Append(Server.HtmlEncode(TargetDbVersion)).Append("</div></div>");
        sb.Append("<div class='card'><span class='label'>数据库识别结果</span><div class='value'>").Append(Server.HtmlEncode(UpgradeMode)).Append("</div></div>");
        sb.Append("<div class='card'><span class='label'>升级结论</span><div class='value'>").Append(Server.HtmlEncode(UpgradeDecision)).Append("</div></div>");
        sb.Append("</div>");
        sb.Append("<h2>升级前检查结果</h2>").Append(UpgradeSummaryHtml);
        sb.Append("<h2>待执行迁移</h2>").Append(PendingMigrationHtml);
        sb.Append("<h2>结构变更</h2>").Append(PendingStructureHtml);
        sb.Append("<h2>初始化数据</h2>").Append(PendingDataHtml);
        sb.Append("<h2>性能优化</h2>").Append(PendingPerformanceHtml);
        sb.Append("<h2>风险提示</h2>").Append(UpgradeRiskHtml);
        sb.Append("</body></html>");
        return sb.ToString();
    }

    private void checkdatabase()
    {
        LastAnalyzeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        ConnectedDatabaseName = GetCurrentDatabaseName();
        LearnSite.DBUtility.DatabaseConnectionSettings settings;
        bool hasCurrentConfig = LearnSite.DBUtility.DatabaseSetupHelper.TryGetCurrentConnectionSettings(out settings);
        bool masterAvailable = hasCurrentConfig && LearnSite.DBUtility.DatabaseSetupHelper.MasterDbExist(settings);
        bool targetDatabaseExists = masterAvailable && LearnSite.DBUtility.DatabaseSetupHelper.TargetDbExist(settings);

        if (!IsDatabaseAvailable())//如果数据库不存在
        {
            if (masterAvailable && !targetDatabaseExists)
            {
                CurrentDbVersion = "未创建";
                UpgradeMode = "SQL Server 已连接 / 数据库不存在";
                UpgradeDecision = "可以初始化新库";
                RiskLevel = "中";
                UpgradeSummaryHtml = BuildListHtml(new List<string>
                {
                    "当前已经能够连接到 SQL Server，但目标数据库 `" + settings.Database + "` 还不存在。",
                    "可以直接点击“创建数据库并初始化”，系统会自动创建 `" + settings.Database + "` 数据库、导入基础表结构，并补齐当前版本所需初始化数据。",
                    "如果这不是你想使用的数据库名称，也可以先修改下方连接配置，再重新检查。"
                }, "upgrade-check-list");
                UpgradeRiskHtml = BuildListHtml(new List<string>
                {
                    "初始化前仍建议确认服务器地址、实例名和数据库名称是否正确，避免在错误服务器上新建库。",
                    "执行初始化的账号需要具备创建数据库、建表和写入数据权限。",
                    "如果当前 SQL Server 磁盘空间不足或账号权限受限，初始化过程可能失败。"
                }, "upgrade-risk-list");
                PendingMigrationHtml = BuildListHtml(new List<string>
                {
                    "系统将自动创建数据库。",
                    "系统将导入 `sql/learnsite.sql` 基础表结构。",
                    "系统将执行当前版本所需的初始化迁移与默认数据补齐。"
                }, "upgrade-pending-list");
                PendingStructureHtml = BuildListHtml(new List<string>
                {
                    "创建 learnsite 数据库。",
                    "导入基础数据表。",
                    "补齐迁移记录表和新增业务表。"
                }, "upgrade-pending-list");
                PendingDataHtml = BuildListHtml(new List<string>
                {
                    "初始化英文词库和默认数据。"
                }, "upgrade-pending-list");
                PendingPerformanceHtml = BuildListHtml(new List<string>
                {
                    "补齐当前版本要求的索引与性能优化项。"
                }, "upgrade-pending-list");
                Panel1.Visible = true;
                showPanel();
                Btnupgrade.Enabled = false;
                Btnupgrade.Visible = false;
                BtnCreateTable.Visible = true;
                BtnCreateTable.Text = "创建数据库并初始化";
                Labelmsg.Text = "当前已连接到 SQL Server，但数据库“" + settings.Database + "”不存在。可以直接点击“创建数据库并初始化”。";
                return;
            }

            CurrentDbVersion = "无法连接";
            UpgradeMode = "数据库连接失败";
            UpgradeDecision = "请先修改连接配置";
            RiskLevel = "高";
            UpgradeSummaryHtml = BuildListHtml(new List<string>
            {
                "当前程序还没有连上 SQL Server，暂时无法读取数据库版本、数据表和迁移状态。",
                "请先检查数据库服务器地址、实例名、数据库名称、账号和密码是否正确。",
                "保存配置成功后，请点击“重新检查数据库”，确认连接恢复正常。"
            }, "upgrade-check-list");
            UpgradeRiskHtml = BuildListHtml(new List<string>
            {
                "如果连接串仍指向错误服务器或错误实例，首页和教师页都会继续跳转到本页。",
                "如果 SQL Server 未启动、未开启 TCP/IP，或 sa 账号不可用，保存配置后仍然无法连接。"
            }, "upgrade-risk-list");
            PendingMigrationHtml = BuildListHtml(new List<string>
            {
                "连接数据库成功后，系统才会显示待执行迁移和升级建议。"
            }, "upgrade-pending-list");
            PendingStructureHtml = BuildListHtml(new List<string>(), "upgrade-pending-list");
            PendingDataHtml = BuildListHtml(new List<string>(), "upgrade-pending-list");
            PendingPerformanceHtml = BuildListHtml(new List<string>(), "upgrade-pending-list");
            Panel1.Visible = true;
            showPanel();
            Btnupgrade.Enabled = false;
            Btnupgrade.Visible = false;
            BtnCreateTable.Visible = false;
            BtnCreateTable.Text = "创建数据表并初始化";
            Labelmsg.Text = "当前程序还没有连接上 SQL Server。请先检查数据库服务器名称或实例名、数据库名称、账号和密码；如果 SQL Server 未启动或未开启 TCP/IP，也会导致这里无法连接。";
        }
        else
        {
            if (LearnSite.DBUtility.SqlHelper.CountTable() > 0)
            {
                AnalyzeUpgradeState();
                BtnCreateTable.Visible = false;
                BtnCreateTable.Text = "创建数据表并初始化";
                Panel1.Visible = false;
                Btnupgrade.Enabled = true;
                Btnupgrade.Visible = true;
                Labelmsg.Text = "数据库连接正常！请先阅读升级预检查结果，再决定是否执行更新。";
            }
            else {
                BtnCreateTable.Visible = true;
                BtnCreateTable.Text = "创建数据表并初始化";
                Btnupgrade.Visible = false;
                Panel1.Visible = false;
                UpgradeMode = "空库 / 未初始化数据库";
                UpgradeDecision = "可以初始化新库";
                RiskLevel = "中";
                CurrentDbVersion = "未初始化";
                UpgradeSummaryHtml = BuildListHtml(new List<string>
                {
                    "当前数据库已经存在，但还是空库，没有任何业务数据表。",
                    "点击“创建数据表并初始化”后，系统会导入基础表结构，并自动执行当前版本初始化迁移。"
                }, "upgrade-check-list");
                UpgradeRiskHtml = BuildListHtml(new List<string>
                {
                    "初始化前仍建议确认当前连接的是目标数据库，避免误操作到其他空库。",
                    "初始化过程中需要建表和写入默认数据权限。"
                }, "upgrade-risk-list");
                PendingMigrationHtml = BuildListHtml(new List<string>
                {
                    "导入 `sql/learnsite.sql` 基础表结构。",
                    "执行当前版本所需初始化迁移与默认数据补齐。"
                }, "upgrade-pending-list");
                PendingStructureHtml = BuildListHtml(new List<string>
                {
                    "创建业务基础表。",
                    "补齐迁移记录表和新增业务表。"
                }, "upgrade-pending-list");
                PendingDataHtml = BuildListHtml(new List<string>
                {
                    "初始化英文词库和默认数据。"
                }, "upgrade-pending-list");
                PendingPerformanceHtml = BuildListHtml(new List<string>
                {
                    "补齐当前版本要求的索引与性能优化项。"
                }, "upgrade-pending-list");
                Labelmsg.Text = "数据库连接正常！当前是空库，可以直接点击“创建数据表并初始化”。";
            }
        }
    }

    private bool IsDatabaseAvailable()
    {
        if (!DatabaseAvailable.HasValue)
        {
            DatabaseAvailable = LearnSite.DBUtility.SqlHelper.DatabaseExist();
        }
        return DatabaseAvailable.Value;
    }

    private void showPanel()
    {
        string myconnstr = System.Configuration.ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
        string[] constr = LearnSite.DBUtility.DbLinkEdit.ReadSqlConfig(myconnstr);
        TextBoxSqlServer.Text = constr[0];
        TextBoxDbName.Text = constr[1];
        TextBoxDbUser.Text = constr[2];
        TextBoxDbPwd.Text = constr[3];
    }
    private bool MasterDbExist(string dbserver, string dbuser, string dbpwd)
    {
        string masterdb = "master";
        string masterConnstring = String.Format("Data Source={0};Initial Catalog={1};uid={2};pwd={3};", dbserver, masterdb, dbuser, dbpwd);
        return LearnSite.DBUtility.DbLinkEdit.DatabaseExist(masterConnstring);
    }
    protected void Buttonedit_Click(object sender, EventArgs e)
    {
        string dbserver = TextBoxSqlServer.Text;
        string dbname = TextBoxDbName.Text;
        string dbuser = TextBoxDbUser.Text;
        string dbpwd = TextBoxDbPwd.Text;
        if (dbserver != "" && dbname != "" && dbuser != "" && dbpwd != "")
        {
            string teststr = dbserver + dbname + dbuser + dbpwd;
            if (teststr.IndexOf(';') < 0 && teststr.IndexOf('=') < 0)
            {
                string namevalue = "SqlServer";
               // string ftpnamevalue = "Ftp";
               // string ftpdbname = dbname + ftpnamevalue;
                if (MasterDbExist(dbserver, dbuser, dbpwd))
                {
                    Buttonedit.Enabled = false;
                    LearnSite.DBUtility.DbLinkEdit.WriteSqlConfig(namevalue, dbserver, dbname, dbuser, dbpwd);
                   // LearnSite.DBUtility.DbLinkEdit.WriteSqlConfig(ftpnamevalue, dbserver, ftpdbname, dbuser, dbpwd);
                    string url = "~/upgrade.aspx";
                    LearnSite.Common.WordProcess.Alert("Webconfig修改成功！", this.Page);
                    Response.Redirect(url, false);
                }
                else
                {
                    Labelmsg.Text = "数据库服务器名称、账号、密码填写可能错误！";
                }
            }
            else
            {
                Labelmsg.Text = "请不要填写非法字符，浪费时间！";
            }
        }
        else
        {
            Labelmsg.Text = "请填写正确的数据库服务器名称等！";
        }
    }

    private void Oldupdate()
    {
        LearnSite.DBUtility.UpdateGrade.updateDatabase();
        LearnSite.DBUtility.UpdateGrade.CreateNewTable();
        LearnSite.DBUtility.UpdateGrade.UpdateTable105();
        LearnSite.DBUtility.UpdateGrade.UpdateTable106();
        LearnSite.DBUtility.UpdateGrade.UpdateTable107();
        LearnSite.DBUtility.UpdateGrade.UpdateHtmlToHtm();
        LearnSite.DBUtility.UpdateGrade.UpdateTable108();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1081();//未更新Works表中的Wgrade,Wterm
        LearnSite.DBUtility.UpdateGrade.UpdateTable1082();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1092();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1093();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1094();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1095();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1096();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1098();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1100();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1101();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1102();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1103();//新增已删除学生表DelStudents
        LearnSite.DBUtility.UpdateGrade.UpdateTable1105();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1106();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1107();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1108();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1109();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1110();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1200();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1201();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1202();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1203();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1205();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1206();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1207();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1208();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1209();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1210();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1211();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1212();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1213();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1214();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1215();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1216();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1217();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1218();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1220();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1222();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1226();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1228();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1229();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1230();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1232();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1251();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1252();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1253();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1260();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1280();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1300();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1320();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1330();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1332();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1333();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1335();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1336();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1337();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1338();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1339();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1350();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1352();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1360();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1365();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1700();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1800();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1801();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1802();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1803();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1804();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1805();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1806();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1807();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1808();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1809();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1810();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1811();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1812();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1813();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1814();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1815();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1816();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1817();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1818();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1819();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1820();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1821();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1822();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1823();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1824();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1825();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1826();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1827();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1828();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1829();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1830();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1831();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1832();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1833();
        LearnSite.DBUtility.UpdateGrade.UpdateTable1834();
    }
    protected void BtnCreateTable_Click(object sender, EventArgs e)
    {
        try
        {
            LearnSite.DBUtility.DatabaseConnectionSettings settings;
            bool createdDatabase = false;
            if (LearnSite.DBUtility.DatabaseSetupHelper.TryGetCurrentConnectionSettings(out settings)
                && LearnSite.DBUtility.DatabaseSetupHelper.MasterDbExist(settings)
                && !LearnSite.DBUtility.DatabaseSetupHelper.TargetDbExist(settings))
            {
                LearnSite.DBUtility.DatabaseSetupHelper.CreateDatabase(settings);
                LearnSite.DBUtility.DatabaseSetupHelper.WaitForTargetDatabaseReady(settings, 8, 800);
                createdDatabase = true;
            }

            DatabaseAvailable = null;
            int n = LearnSite.DBUtility.DatabaseSetupHelper.CreateTableWithRetry(settings, 5, 800);
            List<LearnSite.DBUtility.MigrationResult> pendingResults = LearnSite.DBUtility.DbMigration.RunAllPending();
            StringBuilder resultSummary = new StringBuilder();
            foreach (LearnSite.DBUtility.MigrationResult result in pendingResults)
            {
                if (!result.Success)
                {
                    resultSummary.Append(" 初始化迁移 ").Append(result.Version).Append(" 失败：").Append(result.Message);
                    break;
                }
            }

            string actionText = createdDatabase ? "数据库创建并初始化完成" : "数据表创建并初始化完成";
            Labelmsg.Text = actionText + "！" + n.ToString() + resultSummary.ToString();
            if (resultSummary.Length == 0)
            {
                LearnSite.Common.WordProcess.Alert(actionText + "，系统已自动补齐当前版本所需数据。", this.Page);
                Response.Redirect("~/upgrade.aspx", false);
            }
        }
        catch (Exception ex)
        {
            Labelmsg.Text = "初始化失败：" + ex.Message;
        }
    }
}
