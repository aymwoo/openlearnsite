<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master"  StylesheetTheme="Teacher"  AutoEventWireup="true" CodeFile="termview.aspx.cs" Inherits="Teacher_termview" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/admin-form.css" rel="stylesheet" />
    <style type="text/css">
        .termview-page {
            --admin-form-page-bg: linear-gradient(180deg, #f8fafc 0%, #eef2ff 100%);
            --admin-form-hero-bg: linear-gradient(135deg, #1e1b4b 0%, #4338ca 55%, #6366f1 100%);
            --admin-form-hero-shadow: 0 22px 45px -28px rgba(79, 70, 229, 0.75);
            --admin-form-primary-bg: #4f46e5;
            --admin-form-primary-hover: #4338ca;
            --admin-form-primary-shadow: 0 14px 24px -18px rgba(79, 70, 229, 0.85);
            --admin-form-secondary-border: #c7d2fe;
            --admin-form-secondary-bg: #eef2ff;
            --admin-form-secondary-hover: #e0e7ff;
            --admin-form-secondary-fg: #3730a3;
        }

        .termview-page .admin-form-grid-wrap {
            overflow-x: auto;
        }
    </style>
    <div class="admin-form-page termview-page">
        <div class="admin-form-shell">
            <section class="admin-form-hero">
                <div class="admin-form-hero-content">
                    <div class="admin-form-eyebrow">Term Score</div>
                    <h1 class="admin-form-title">期末成绩评定</h1>
                    <p class="admin-form-subtitle">按入学年度、年级、班级和学期筛选期末成绩，并支持浏览与导出 Excel。</p>
                </div>
            </section>

            <section class="admin-form-panel">
                <div class="admin-form-toolbar">
                    <div>
                        <h2 class="admin-form-section-title">筛选条件</h2>
                        <p class="admin-form-section-desc">选择目标学年和班级后，可浏览当前成绩表或导出为 Excel。</p>
                    </div>
                    <div class="admin-form-action-row">
                        <asp:Button ID="BtnExcel" runat="server" OnClick="BtnExcel_Click"
                            Text="导出 Excel" ToolTip="将学生期末成绩以Excel表格导出" CssClass="admin-form-btn admin-form-btn--primary" />
                        <asp:Button ID="Btnshow" runat="server" OnClick="Btnshow_Click" Text="成绩浏览"
                            CssClass="admin-form-btn admin-form-btn--secondary" />
                        <asp:Button ID="Btnback" runat="server" Text="返回查询" OnClick="Btnback_Click" CssClass="admin-form-btn admin-form-btn--secondary" />
                    </div>
                </div>
                <div class="admin-form-grid">
                    <div class="admin-form-field">
                        <label class="admin-form-label" for="<%= DDLYear.ClientID %>">入学年度</label>
                        <asp:DropDownList ID="DDLYear" runat="server"
                            Font-Size="9pt" AutoPostBack="True"
                            onselectedindexchanged="DDLYear_SelectedIndexChanged" CssClass="admin-form-select"> </asp:DropDownList>
                    </div>
                    <div class="admin-form-field">
                        <label class="admin-form-label" for="<%= DDLgrade.ClientID %>">年级</label>
                        <asp:DropDownList ID="DDLgrade" runat="server" Font-Size="9pt" CssClass="admin-form-select"> </asp:DropDownList>
                    </div>
                    <div class="admin-form-field">
                        <label class="admin-form-label" for="<%= DDLclass.ClientID %>">班级</label>
                        <asp:DropDownList ID="DDLclass" runat="server" Font-Size="9pt" CssClass="admin-form-select"></asp:DropDownList>
                    </div>
                    <div class="admin-form-field">
                        <label class="admin-form-label" for="<%= DDLterm.ClientID %>">学期</label>
                        <asp:DropDownList ID="DDLterm" runat="server" Font-Size="9pt"
                                EnableTheming="True" CssClass="admin-form-select">
                            <asp:ListItem Value="1">第一学期</asp:ListItem>
                            <asp:ListItem Value="2">第二学期</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </section>

            <section class="admin-form-feedback">
                <asp:Label ID="Labelmsg" runat="server" SkinID="LabelMsgRed"></asp:Label>
            </section>

            <section class="admin-form-panel">
                <h2 class="admin-form-section-title">成绩列表</h2>
                <p class="admin-form-section-desc">显示当前筛选条件下的学生期末成绩构成和总分评定。</p>
                <div class="admin-form-grid-wrap">
                    <asp:GridView ID="GVTermScore" runat="server" AutoGenerateColumns="False"
                         DataKeyNames="Tid"  SkinID="GVmission"
                        PageSize="25" Width="98%" EnableModelValidation="True"
                    onrowdatabound="GVTermScore_RowDataBound" HorizontalAlign="Center" >
                        <Columns>
                            <asp:BoundField HeaderText="编号" />
                            <asp:BoundField DataField="Tnum" HeaderText="学号" />
                            <asp:BoundField DataField="Tgrade" HeaderText="年级" />
                            <asp:BoundField DataField="Tclass" HeaderText="班级" />
                            <asp:HyperLinkField DataNavigateUrlFields="Tnum,Tgrade,Tterm"
                                DataNavigateUrlFormatString="studentwork.aspx?snum={0}&amp;sgrade={1}&amp;sterm={2}"
                                DataTextField="Tname" HeaderText="姓名" Target="_blank" />
                            <asp:BoundField DataField="Tscore" HeaderText="作品" />
                            <asp:BoundField DataField="Tgscore" HeaderText="小组" />
                            <asp:BoundField DataField="Tpscore" HeaderText="讨论" />
                            <asp:BoundField DataField="Ttxtform" HeaderText="表单" />
                            <asp:BoundField DataField="Tvscore" HeaderText="调查" />
                            <asp:BoundField DataField="Twscore" HeaderText="网页" />
                            <asp:BoundField DataField="Tchinese" HeaderText="拼音" />
                            <asp:BoundField DataField="Tfscore" HeaderText="英语" />
                            <asp:BoundField DataField="Ttscore" HeaderText="中文" />
                            <asp:BoundField DataField="Tattitude" HeaderText="表现" />
                            <asp:BoundField DataField="Tallscore" HeaderText="总分" />
                            <asp:BoundField DataField="Tape" HeaderText="评定" />
                        </Columns>
                    </asp:GridView>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
