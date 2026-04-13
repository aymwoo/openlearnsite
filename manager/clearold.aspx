<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manage.master"  StylesheetTheme="Teacher"   AutoEventWireup="true" CodeFile="clearold.aspx.cs" Inherits="Manager_clearold" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
<style type="text/css">
.co-wrap {
        --co-bg: linear-gradient(180deg, #f8fbff 0%, #f3f7ff 100%);
        --co-card: rgba(255,255,255,0.96);
        --co-border: #dbe6f5;
        --co-text: #0f172a;
        --co-muted: #64748b;
        padding: 28px;
        background: var(--co-bg);
        color: var(--co-text);
        box-sizing: border-box;
	width: 100%;
        min-height: calc(100vh - 8rem);
    }
    .co-wrap * { box-sizing: border-box; }

    .co-hero {
        border-radius: 1rem;
        padding: 28px;
        background: linear-gradient(135deg,#fff1f2 0%,#fee2e2 100%);
        color: #7f1d1d;
        border: 1px solid #fecaca;
        box-shadow: 0 4px 16px rgba(185,28,28,.08);
        margin-bottom: 24px;
    }
    .co-hero h1 { margin: 0; font-size: 28px; font-weight: 800; letter-spacing: -0.03em; }
    .co-hero p  { margin: 10px 0 0; font-size: 14px; line-height: 1.8; color: rgba(254,242,242,0.88); }

    .co-grid {
        display: grid;
        gap: 20px;
        grid-template-columns: repeat(3, minmax(0, 1fr));
    }

    .co-card {
        border-radius: 1rem;
        border: 1px solid var(--co-border);
        background: var(--co-card);
        box-shadow: 0 12px 30px rgba(15,23,42,0.05);
        overflow: hidden;
    }
    .co-card__head {
        padding: 18px 22px 0;
    }
    .co-card__title {
        margin: 0;
        font-size: 16px;
        font-weight: 800;
        color: var(--co-text);
    }
    .co-card__desc {
        margin: 6px 0 0;
        font-size: 12px;
        line-height: 1.7;
        color: var(--co-muted);
    }
    .co-card__body {
        padding: 18px 22px 22px;
        display: flex;
        flex-direction: column;
        gap: 14px;
    }

    .co-card--orange { background: linear-gradient(160deg,#fff 0%,#fffdf7 100%); border-color: #fde8c8; }
    .co-card--green  { background: linear-gradient(160deg,#fff 0%,#f7fef9 100%); border-color: #d4f0dc; }
    .co-card--red    { background: linear-gradient(160deg,#fff 0%,#fff8f8 100%); border-color: #fddede; }

    .co-field { display: flex; flex-direction: column; gap: 6px; }
    .co-label { font-size: 14px; font-weight: 700; color: #334155; }

    .co-select {
        width: 100%;
        min-height: 44px;
        padding: 0 12px;
        border: 1px solid #cbd5e1;
        border-radius: 1rem;
        background: #f8fafc;
        color: #0f172a;
        font-size: 14px;
    }
    .co-select:focus { border-color: #60a5fa; outline: none; box-shadow: 0 0 0 4px rgba(96,165,250,0.18); }

    .co-input {
        width: 80px;
        min-height: 40px;
        padding: 0 10px;
        border: 1px solid #cbd5e1;
        border-radius: 1rem;
        background: #f8fafc;
        color: #0f172a;
        font-size: 14px;
        text-align: center;
    }

    .co-row { display: flex; flex-wrap: wrap; align-items: center; gap: 10px; }

    .co-btn {
        display: inline-flex;
        align-items: center;
        justify-content: center;
        min-height: 42px;
        padding: 0 20px;
        border: 0;
        border-radius: 1rem;
        font-size: 14px;
        font-weight: 700;
        color: #fff;
        cursor: pointer;
        transition: transform 0.18s, box-shadow 0.18s;
    }
    .co-btn:hover { transform: translateY(-1px); }
    .co-btn--orange { background: linear-gradient(135deg,#f97316,#ea580c); box-shadow: 0 8px 20px rgba(249,115,22,0.25); }
    .co-btn--green  { background: linear-gradient(135deg,#16a34a,#15803d); box-shadow: 0 8px 20px rgba(22,163,74,0.25); }
    .co-btn--red    { background: linear-gradient(135deg,#dc2626,#b91c1c); box-shadow: 0 8px 20px rgba(220,38,38,0.25); }

    .co-alert {
        padding: 12px 14px;
        border-radius: 1rem;
        font-size: 14px;
        line-height: 1.7;
        font-weight: 600;
    }
    .co-alert--warn { background: #fef3c7; color: #92400e; border: 1px solid #fde68a; }
    .co-alert--danger { background: #fee2e2; color: #7f1d1d; border: 1px solid #fecaca; }

    .co-msg { font-size: 14px; font-weight: 700; color: #b91c1c; }

    @media (max-width: 900px) {
        .co-grid { grid-template-columns: 1fr; }
    }

</style>

<div class="co-wrap">
    <div class="co-hero">
        <h1>数据清理</h1>
        <p>清理历史数据、打字成绩或班级学生记录。所有操作不可撤销，执行前请务必备份数据库。</p>
    </div>

    <div class="co-grid">

        <%-- 卡片1：历史数据清理 --%>
        <div class="co-card co-card--orange">
            <div class="co-card__head">
                <h2 class="co-card__title">历史数据清理</h2>
                <p class="co-card__desc">删除指定年份之前的作品、签到及测验记录。</p>
            </div>
            <div class="co-card__body">
                <div class="co-field">
                    <span class="co-label">清理范围</span>
                    <asp:DropDownList ID="DDLyear" runat="server" CssClass="co-select">
                        <asp:ListItem Selected="True" Value="3">三年前</asp:ListItem>
                        <asp:ListItem Value="5">五年前</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="co-alert co-alert--warn">包含：作品记录、签到记录、测验记录</div>
                <asp:Button ID="ButtonClear" runat="server" SkinID="BtnNormal" Text="执行清理"
                    ToolTip="提示：将指定年前的作品记录、签到记录、讨论记录删除！"
                    onclick="ButtonClear_Click" CssClass="co-btn co-btn--orange" />
            </div>
        </div>

        <%-- 卡片2：打字成绩清理 --%>
        <div class="co-card co-card--green">
            <div class="co-card__head">
                <h2 class="co-card__title">打字成绩清理</h2>
                <p class="co-card__desc">清除全校中文打字或指法练习的所有成绩记录。</p>
            </div>
            <div class="co-card__body">
                <asp:Button ID="ButtonClearTyper" runat="server" SkinID="BtnLong" Text="清除全校中文打字成绩"
                    ToolTip="提示：将清除全校中文打字成绩！"
                    onclick="ButtonClearTyper_Click" CssClass="co-btn co-btn--green" />
                <asp:Button ID="ButtonClearFinger" runat="server" SkinID="BtnLong" Text="清除全校指法练习成绩"
                    ToolTip="提示：将清除全校指法练习成绩！"
                    onclick="ButtonClearFinger_Click" CssClass="co-btn co-btn--green" />
            </div>
        </div>

        <%-- 卡片3：班级学生清理 --%>
        <div class="co-card co-card--red">
            <div class="co-card__head">
                <h2 class="co-card__title">班级学生清理</h2>
                <p class="co-card__desc">清空指定班级的全部学生及其所有关联记录，操作不可恢复。</p>
            </div>
            <div class="co-card__body">
                <div class="co-row">
                    <div class="co-field">
                        <span class="co-label">年级</span>
                        <asp:DropDownList ID="DDLgrade" runat="server" CssClass="co-select" AutoPostBack="True"
                            onselectedindexchanged="DDLgrade_SelectedIndexChanged" style="width:80px;"></asp:DropDownList>
                    </div>
                    <div class="co-field">
                        <span class="co-label">班级</span>
                        <asp:DropDownList ID="DDLclass" runat="server" CssClass="co-select" AutoPostBack="True"
                            onselectedindexchanged="DDLclass_SelectedIndexChanged" style="width:80px;"></asp:DropDownList>
                    </div>
                    <div class="co-field">
                        <span class="co-label">学生数</span>
                        <asp:TextBox ID="TextBoxcount" runat="server" SkinID="TextBoxaa" CssClass="co-input" ReadOnly="True"></asp:TextBox>
                    </div>
                </div>
                <div class="co-alert co-alert--danger">清空后作品、签到、调查、讨论等记录将全部删除且无法恢复！</div>
                <div class="co-row">
                    <asp:CheckBox ID="CheckBoxDel" runat="server" Text="确认操作" />
                    <asp:Button ID="ButtonClearStudent" runat="server" SkinID="BtnLong" Text="清空该班级所有学生"
                        ToolTip="提示：将清空该班级的所有学生及其作品、签到、调查、讨论等记录，无法恢复！"
                        onclick="ButtonClearStudent_Click" CssClass="co-btn co-btn--red" />
                </div>
            </div>
        </div>

    </div>

    <div style="margin-top:20px;">
        <asp:Label ID="Labelmsg" runat="server" SkinID="LabelMsgRed" CssClass="co-msg">清理前注意备份数据库！</asp:Label>
    </div>
</div>
</asp:Content>
