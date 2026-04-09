<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manage.master"  StylesheetTheme="Teacher"   AutoEventWireup="true" CodeFile="clearold.aspx.cs" Inherits="Manager_clearold" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">


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
