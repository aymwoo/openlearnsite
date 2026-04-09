<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manage.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="dbupgrade.aspx.cs" Inherits="Manager_DbUpgrade" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="server">


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
