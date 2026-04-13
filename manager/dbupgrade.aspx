<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manage.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="dbupgrade.aspx.cs" Inherits="Manager_DbUpgrade" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">
<style type="text/css">
.dbu-wrap {
        --dbu-bg:      linear-gradient(180deg, #f8fbff 0%, #f3f7ff 100%);
        --dbu-card:    rgba(255,255,255,0.96);
        --dbu-border:  #dbe6f5;
        --dbu-text:    #0f172a;
        --dbu-muted:   #64748b;
        --dbu-primary: #2563eb;
        --dbu-success: #16a34a;
        --dbu-warn:    #d97706;
        --dbu-danger:  #dc2626;
        padding: 28px;
        background: var(--dbu-bg);
        color: var(--dbu-text);
        font-family: ui-sans-serif, system-ui, sans-serif;
    }
    .dbu-wrap * { box-sizing: border-box; }

    .dbu-shell { display: flex; flex-direction: column; gap: 20px; }

    /* Hero */
    .dbu-hero {
        position: relative; overflow: hidden;
        border: 1px solid var(--dbu-border); border-radius: 0.75rem;
        padding: 24px 28px;
        background:
            radial-gradient(circle at top left, rgba(59,130,246,0.18), transparent 38%),
            radial-gradient(circle at right center, rgba(14,165,233,0.16), transparent 26%),
            linear-gradient(135deg, #0f172a 0%, #1d4ed8 52%, #38bdf8 100%);
        color: #eff6ff;
        box-shadow: 0 28px 60px rgba(37,99,235,0.2);
    }
    .dbu-hero__content {
        position: relative; z-index: 1;
        display: flex; flex-wrap: wrap; justify-content: space-between;
        gap: 16px; align-items: center;
    }
    .dbu-hero__title { margin: 0; font-size: 26px; font-weight: 800; letter-spacing: -0.03em; }
    .dbu-hero__sub { margin: 6px 0 0; font-size: 13px; color: rgba(239,246,255,0.85); }

    /* Cards */
    .dbu-grid { display: grid; gap: 20px; grid-template-columns: 1fr 1fr; }
    .dbu-card {
        border: 1px solid var(--dbu-border); border-radius: 0.75rem;
        background: var(--dbu-card);
        box-shadow: 0 4px 20px rgba(15,23,42,0.05);
    }
    .dbu-card--full { grid-column: span 2; }
    .dbu-card__head { padding: 18px 22px 0; }
    .dbu-card__title {
        margin: 0; font-size: 15px; font-weight: 800;
        letter-spacing: -0.02em; color: var(--dbu-text);
    }
    .dbu-card__desc { margin: 4px 0 0; font-size: 12px; color: var(--dbu-muted); }
    .dbu-card__body { padding: 16px 22px 20px; }

    /* Status badges */
    .dbu-badge {
        display: inline-flex; align-items: center; gap: 5px;
        padding: 3px 10px; border-radius: 9999px;
        font-size: 12px; font-weight: 700;
    }
    .dbu-badge--ok      { background: #dcfce7; color: #15803d; }
    .dbu-badge--warn    { background: #fef3c7; color: #92400e; }
    .dbu-badge--danger  { background: #fee2e2; color: #991b1b; }

    /* Stat row */
    .dbu-stats { display: flex; gap: 24px; flex-wrap: wrap; margin-top: 10px; }
    .dbu-stat { display: flex; flex-direction: column; gap: 3px; }
    .dbu-stat__val { font-size: 28px; font-weight: 800; letter-spacing: -0.04em; color: var(--dbu-primary); }
    .dbu-stat__lbl { font-size: 11px; color: var(--dbu-muted); font-weight: 600; }

    /* Table */
    .dbu-table { width: 100%; border-collapse: collapse; font-size: 13px; }
    .dbu-table th {
        padding: 8px 12px; text-align: left; font-size: 11px; font-weight: 700;
        letter-spacing: 0.07em; text-transform: uppercase;
        color: #64748b; background: #f8fafc; border-bottom: 1px solid #e2e8f0;
    }
    .dbu-table td { padding: 10px 12px; border-bottom: 1px solid #f1f5f9; vertical-align: top; }
    .dbu-table tbody tr:last-child td { border-bottom: none; }
    .dbu-table tbody tr:hover td { background: #f8fafc; }

    /* Row colors */
    .dbu-row--pending td { background: #fffbeb; }
    .dbu-row--success td { background: #f0fdf4; }
    .dbu-row--fail    td { background: #fff1f2; }

    /* Buttons */
    .dbu-btn {
        display: inline-flex; align-items: center; gap: 7px;
        padding: 10px 22px; border: none; border-radius: 0.5rem;
        font-size: 14px; font-weight: 700; cursor: pointer;
        transition: opacity 0.15s, transform 0.1s;
    }
    .dbu-btn:hover { opacity: 0.88; transform: translateY(-1px); }
    .dbu-btn:active { transform: translateY(0); }
    .dbu-btn--primary { background: #2563eb; color: #fff; box-shadow: 0 4px 12px rgba(37,99,235,0.25); }
    .dbu-btn--slate   { background: #e2e8f0; color: #334155; }

    /* Log output */
    .dbu-log {
        border-radius: 0.5rem; border: 1px solid #e2e8f0; background: #f8fafc;
        padding: 14px 16px; font-size: 13px; line-height: 1.8;
        min-height: 60px; max-height: 320px; overflow-y: auto;
    }
    .dbu-log-line--ok   { color: #15803d; }
    .dbu-log-line--err  { color: #dc2626; font-weight: 700; }
    .dbu-log-line--info { color: #1e3a8a; }

    .dbu-divider { height: 1px; background: #e2e8f0; margin: 14px 0; }
    .dbu-alert {
        padding: 10px 14px; border-radius: 0.5rem; font-size: 13px;
        background: #dbeafe; color: #1e3a8a; border: 1px solid #bfdbfe;
    }
    .dbu-alert--ok   { background: #dcfce7; color: #14532d; border-color: #bbf7d0; }
    .dbu-alert--warn { background: #fef3c7; color: #78350f; border-color: #fde68a; }
    .dbu-alert--err  { background: #fee2e2; color: #7f1d1d; border-color: #fecaca; }

</style>

<div class="dbu-wrap">
<div class="dbu-shell">

    <!-- Hero -->
    <div class="dbu-hero">
        <div class="dbu-hero__content">
            <div>
                <h1 class="dbu-hero__title">数据库版本管理</h1>
                <p class="dbu-hero__sub">检测并执行数据库结构升级，所有迁移均为幂等操作，可安全重复运行。</p>
            </div>
            <div style="flex-shrink:0;">
                <asp:Label ID="LabelStatus" runat="server" />
            </div>
        </div>
    </div>

    <!-- Stats row -->
    <div class="dbu-grid">
        <div class="dbu-card">
            <div class="dbu-card__head">
                <h2 class="dbu-card__title">数据库状态</h2>
                <p class="dbu-card__desc">当前版本检测结果</p>
            </div>
            <div class="dbu-card__body">
                <div class="dbu-stats">
                    <div class="dbu-stat">
                        <span class="dbu-stat__val" id="statTotal" runat="server"><asp:Literal ID="LitTotal" runat="server" /></span>
                        <span class="dbu-stat__lbl">全部迁移</span>
                    </div>
                    <div class="dbu-stat">
                        <span class="dbu-stat__val" style="color:#15803d;"><asp:Literal ID="LitApplied" runat="server" /></span>
                        <span class="dbu-stat__lbl">已完成</span>
                    </div>
                    <div class="dbu-stat">
                        <span class="dbu-stat__val" style="color:#d97706;"><asp:Literal ID="LitPending" runat="server" /></span>
                        <span class="dbu-stat__lbl">待执行</span>
                    </div>
                </div>
                <div class="dbu-divider"></div>
                <asp:Literal ID="LitDbAlert" runat="server" />
            </div>
        </div>

        <div class="dbu-card">
            <div class="dbu-card__head">
                <h2 class="dbu-card__title">一键升级</h2>
                <p class="dbu-card__desc">运行所有待执行迁移，已完成的迁移将自动跳过</p>
            </div>
            <div class="dbu-card__body">
                <asp:Literal ID="LitPendingList" runat="server" />
                <div class="dbu-divider"></div>
                <asp:Button ID="BtnInitialize" runat="server" Text="创建数据库并初始化" CssClass="dbu-btn dbu-btn--slate"
                    OnClick="BtnInitialize_Click" OnClientClick="return confirm('确认执行数据库初始化？系统会在需要时自动创建数据库、导入基础表结构并执行全部迁移。');" />
                &nbsp;
                <asp:Button ID="BtnRunAll" runat="server" Text="一键升级数据库" CssClass="dbu-btn dbu-btn--primary"
                    OnClick="BtnRunAll_Click" OnClientClick="return confirm('确认执行所有待升级迁移？');" />
                &nbsp;
                <asp:Button ID="BtnBackfill" runat="server" Text="标记已完成（现有库）" CssClass="dbu-btn dbu-btn--slate"
                    OnClick="BtnBackfill_Click" OnClientClick="return confirm('将把当前所有迁移标记为已完成（不会修改数据库结构）。仅在已经是最新版的数据库上使用。确定？');" />
                <asp:Literal ID="LitRunResult" runat="server" />
            </div>
        </div>
    </div>

    <!-- History -->
    <div class="dbu-card dbu-card--full">
        <div class="dbu-card__head">
            <h2 class="dbu-card__title">迁移历史记录</h2>
            <p class="dbu-card__desc">_DbMigrations 表中的所有记录</p>
        </div>
        <div class="dbu-card__body">
            <asp:Literal ID="LitHistory" runat="server" />
        </div>
    </div>

    <!-- All migrations reference -->
    <div class="dbu-card dbu-card--full">
        <div class="dbu-card__head">
            <h2 class="dbu-card__title">全部迁移列表</h2>
            <p class="dbu-card__desc">系统定义的所有迁移（绿色=已完成，黄色=待执行）</p>
        </div>
        <div class="dbu-card__body">
            <asp:Literal ID="LitAllMigrations" runat="server" />
        </div>
    </div>

</div>
</div>
</asp:Content>
