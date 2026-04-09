using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using LearnSite.DBUtility;

public partial class Manager_DbUpgrade : System.Web.UI.Page
{
    protected void BtnInitialize_Click(object sender, EventArgs e)
    {
        try
        {
            DatabaseConnectionSettings settings;
            bool createdDatabase = false;
            if (DatabaseSetupHelper.TryGetCurrentConnectionSettings(out settings)
                && DatabaseSetupHelper.MasterDbExist(settings)
                && !DatabaseSetupHelper.TargetDbExist(settings))
            {
                DatabaseSetupHelper.CreateDatabase(settings);
                DatabaseSetupHelper.WaitForTargetDatabaseReady(settings, 8, 800);
                createdDatabase = true;
            }

            int n = DatabaseSetupHelper.CreateTableWithRetry(settings, 5, 800);
            List<MigrationResult> results = DbMigration.RunAllPending();
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='dbu-divider'></div><div class='dbu-log'>");
            sb.Append("<div class='dbu-log-line--ok'>✔ ");
            sb.Append(createdDatabase ? "数据库已创建并导入基础表结构" : "基础表结构已导入");
            sb.Append("，影响行数 ").Append(n).Append("。</div>");
            foreach (MigrationResult r in results)
            {
                string cls = r.Success ? "dbu-log-line--ok" : "dbu-log-line--err";
                string icon = r.Success ? "✔" : "✘";
                sb.AppendFormat(
                    "<div class='{0}'>{1} [{2}] {3}  -  {4}</div>",
                    cls, icon,
                    System.Web.HttpUtility.HtmlEncode(r.Version),
                    System.Web.HttpUtility.HtmlEncode(r.Description),
                    System.Web.HttpUtility.HtmlEncode(r.Message));
                if (!r.Success) break;
            }
            sb.Append("</div>");
            LitRunResult.Text = sb.ToString();
        }
        catch (Exception ex)
        {
            LitRunResult.Text = "<div class='dbu-divider'></div><div class='dbu-alert dbu-alert--err'>初始化失败：" + System.Web.HttpUtility.HtmlEncode(ex.Message) + "</div>";
        }

        RefreshUI();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            RefreshUI();
    }

    // ------------------------------------------------------------------
    // Button: run all pending migrations
    // ------------------------------------------------------------------
    protected void BtnRunAll_Click(object sender, EventArgs e)
    {
        List<MigrationResult> results = DbMigration.RunAllPending();
        StringBuilder sb = new StringBuilder();
        sb.Append("<div class='dbu-divider'></div><div class='dbu-log'>");
        if (results.Count == 0)
        {
            sb.Append("<span class='dbu-log-line--info'>没有待执行的迁移。</span>");
        }
        else
        {
            foreach (MigrationResult r in results)
            {
                string cls = r.Success ? "dbu-log-line--ok" : "dbu-log-line--err";
                string icon = r.Success ? "✔" : "✘";
                sb.AppendFormat(
                    "<div class='{0}'>{1} [{2}] {3}  —  {4}</div>",
                    cls, icon,
                    System.Web.HttpUtility.HtmlEncode(r.Version),
                    System.Web.HttpUtility.HtmlEncode(r.Description),
                    System.Web.HttpUtility.HtmlEncode(r.Message));
            }
        }
        sb.Append("</div>");
        LitRunResult.Text = sb.ToString();
        RefreshUI();
    }

    // ------------------------------------------------------------------
    // Button: backfill (mark all as applied without re-running)
    // ------------------------------------------------------------------
    protected void BtnBackfill_Click(object sender, EventArgs e)
    {
        DbMigration.BackfillApplied();
        LitRunResult.Text = "<div class='dbu-divider'></div>" +
            "<div class='dbu-alert dbu-alert--ok'>已将所有迁移标记为完成。</div>";
        RefreshUI();
    }

    // ------------------------------------------------------------------
    // Render helpers
    // ------------------------------------------------------------------
    private void RefreshUI()
    {
        DatabaseConnectionSettings settings;
        bool hasCurrentConfig = DatabaseSetupHelper.TryGetCurrentConnectionSettings(out settings);
        bool masterAvailable = hasCurrentConfig && DatabaseSetupHelper.MasterDbExist(settings);
        bool targetDatabaseExists = masterAvailable && DatabaseSetupHelper.TargetDbExist(settings);
        bool dbOk = SqlHelper.DatabaseExist();
        bool isEmptyDatabase = dbOk && SqlHelper.CountTable() == 0;
        int totalCount = DbMigration.AllMigrations.Count;
        HashSet<string> applied = dbOk ? DbMigration.GetAppliedVersions() : new HashSet<string>();
        List<MigrationEntry> pending = new List<MigrationEntry>();
        foreach (MigrationEntry m in DbMigration.AllMigrations)
            if (!applied.Contains(m.Version)) pending.Add(m);

        LitTotal.Text   = totalCount.ToString();
        LitApplied.Text = applied.Count.ToString();
        LitPending.Text = pending.Count.ToString();

        // DB status badge
        bool versionOk = dbOk && UpdateGrade.VersionCheck();
        if (!dbOk && masterAvailable && !targetDatabaseExists)
        {
            LabelStatus.Text = "<span class='dbu-badge dbu-badge--warn'>数据库不存在</span>";
            LitDbAlert.Text = "<div class='dbu-alert dbu-alert--warn'>当前已连接到 SQL Server，但数据库 <code>" + System.Web.HttpUtility.HtmlEncode(settings.Database) + "</code> 不存在。可以直接点击下方“创建数据库并初始化”。</div>";
        }
        else if (!dbOk)
        {
            LabelStatus.Text = "<span class='dbu-badge dbu-badge--danger'>数据库不可用</span>";
            LitDbAlert.Text  = "<div class='dbu-alert dbu-alert--err'>无法连接到数据库，请检查连接设置。</div>";
        }
        else if (isEmptyDatabase)
        {
            LabelStatus.Text = "<span class='dbu-badge dbu-badge--warn'>空库未初始化</span>";
            LitDbAlert.Text  = "<div class='dbu-alert dbu-alert--warn'>当前数据库已存在，但还没有业务表。可以直接点击下方“创建数据表并初始化”。</div>";
        }
        else if (!versionOk)
        {
            LabelStatus.Text = "<span class='dbu-badge dbu-badge--warn'>需要升级</span>";
            LitDbAlert.Text  = "<div class='dbu-alert dbu-alert--warn'>数据库结构需要更新，请点击“一键升级数据库”。</div>";
        }
        else
        {
            LabelStatus.Text = "<span class='dbu-badge dbu-badge--ok'>已是最新版</span>";
            LitDbAlert.Text  = "<div class='dbu-alert dbu-alert--ok'>数据库结构已是最新版本，无需升级。</div>";
        }

        // Pending list summary
        if (!dbOk && masterAvailable && !targetDatabaseExists)
        {
            LitPendingList.Text = "<div class='dbu-alert dbu-alert--warn'>目标数据库尚不存在。初始化时将自动创建数据库、导入基础表，并执行全部迁移。</div>";
        }
        else if (isEmptyDatabase)
        {
            LitPendingList.Text = "<div class='dbu-alert dbu-alert--warn'>当前为空库。初始化时将导入基础表，并执行全部迁移。</div>";
        }
        else if (pending.Count == 0)
        {
            LitPendingList.Text = "<div class='dbu-alert dbu-alert--ok'>当前没有待执行的迁移。</div>";
        }
        else
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='dbu-alert dbu-alert--warn' style='margin-bottom:10px;'>以下 <strong>");
            sb.Append(pending.Count);
            sb.Append("</strong> 个迁移尚未执行：</div>");
            sb.Append("<ul style='margin:0;padding-left:18px;font-size:13px;line-height:2;'>");
            foreach (MigrationEntry m in pending)
            {
                sb.AppendFormat("<li><code style='color:#92400e;font-weight:700;'>[{0}]</code> {1}</li>",
                    System.Web.HttpUtility.HtmlEncode(m.Version),
                    System.Web.HttpUtility.HtmlEncode(m.Description));
            }
            sb.Append("</ul>");
            LitPendingList.Text = sb.ToString();
        }

        // History table
        DataTable history = dbOk ? DbMigration.GetMigrationHistory() : new DataTable();
        if (history.Rows.Count == 0)
        {
            LitHistory.Text = "<div class='dbu-alert'>暂无迁移历史记录。执行升级后此处将显示记录。</div>";
        }
        else
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table class='dbu-table'>");
            sb.Append("<thead><tr><th>#</th><th>版本</th><th>描述</th><th>执行时间</th><th>状态</th></tr></thead>");
            sb.Append("<tbody>");
            foreach (DataRow row in history.Rows)
            {
                bool ok = Convert.ToBoolean(row["Success"]);
                string rowCls = ok ? "dbu-row--success" : "dbu-row--fail";
                string badgeCls = ok ? "dbu-badge--ok" : "dbu-badge--danger";
                string badgeTxt = ok ? "成功" : "失败";
                sb.AppendFormat(
                    "<tr class='{0}'><td>{1}</td><td><code>{2}</code></td><td>{3}</td><td style='white-space:nowrap;'>{4}</td><td><span class='dbu-badge {5}'>{6}</span></td></tr>",
                    rowCls,
                    row["MigrationId"],
                    System.Web.HttpUtility.HtmlEncode(row["Version"].ToString()),
                    System.Web.HttpUtility.HtmlEncode(row["Description"].ToString()),
                    Convert.ToDateTime(row["AppliedAt"]).ToString("yyyy-MM-dd HH:mm:ss"),
                    badgeCls, badgeTxt);
            }
            sb.Append("</tbody></table>");
            LitHistory.Text = sb.ToString();
        }

        // All-migrations reference table
        StringBuilder allSb = new StringBuilder();
        allSb.Append("<table class='dbu-table'>");
        allSb.Append("<thead><tr><th>#</th><th>版本</th><th>描述</th><th>状态</th></tr></thead>");
        allSb.Append("<tbody>");
        int idx = 1;
        foreach (MigrationEntry m in DbMigration.AllMigrations)
        {
            bool isDone = applied.Contains(m.Version);
            string rowCls = isDone ? "dbu-row--success" : "dbu-row--pending";
            string badgeCls = isDone ? "dbu-badge--ok" : "dbu-badge--warn";
            string badgeTxt = isDone ? "已完成" : "待执行";
            allSb.AppendFormat(
                "<tr class='{0}'><td>{1}</td><td><code>{2}</code></td><td>{3}</td><td><span class='dbu-badge {4}'>{5}</span></td></tr>",
                rowCls, idx++,
                System.Web.HttpUtility.HtmlEncode(m.Version),
                System.Web.HttpUtility.HtmlEncode(m.Description),
                badgeCls, badgeTxt);
        }
        allSb.Append("</tbody></table>");
        LitAllMigrations.Text = allSb.ToString();

        BtnInitialize.Visible = (!dbOk && masterAvailable && !targetDatabaseExists) || isEmptyDatabase;
        BtnInitialize.Text = (!dbOk && masterAvailable && !targetDatabaseExists) ? "创建数据库并初始化" : "创建数据表并初始化";
        BtnInitialize.Enabled = BtnInitialize.Visible;
        BtnRunAll.Enabled = dbOk && !isEmptyDatabase && pending.Count > 0;
        BtnBackfill.Enabled = dbOk && !isEmptyDatabase;
    }

}
