<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="termscores.aspx.cs" Inherits="Teacher_termscores" ResponseEncoding="utf-8" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <style>
        .term-page { padding: 24px; }
        .term-shell { display: flex; flex-direction: column; gap: 24px; }
        .term-hero {
            position: relative;
            overflow: hidden;
            border-radius: 24px;
            padding: 28px 32px;
            background: linear-gradient(135deg, #0f172a 0%, #2563eb 52%, #22c55e 100%);
            color: #eff6ff;
            box-shadow: 0 18px 44px rgba(15, 23, 42, 0.16);
        }
        .term-hero::after {
            content: "";
            position: absolute;
            right: -40px;
            top: -40px;
            width: 180px;
            height: 180px;
            border-radius: 999px;
            background: rgba(255,255,255,0.09);
        }
        .term-hero__content {
            position: relative;
            z-index: 1;
            display: flex;
            justify-content: space-between;
            gap: 20px;
            flex-wrap: wrap;
            align-items: flex-start;
        }
        .term-hero__title { margin: 0; font-size: 32px; font-weight: 800; letter-spacing: 0.04em; }
        .term-hero__subtitle { margin: 10px 0 0; max-width: 760px; color: rgba(239,246,255,0.9); line-height: 1.75; }
        .term-hero__meta {
            display: inline-flex;
            align-items: center;
            gap: 10px;
            padding: 10px 14px;
            border-radius: 14px;
            background: rgba(15,23,42,0.22);
            border: 1px solid rgba(255,255,255,0.18);
            font-weight: 700;
            white-space: nowrap;
        }
        .term-grid {
            display: grid;
            grid-template-columns: repeat(12, minmax(0, 1fr));
            gap: 20px;
        }
        .term-card {
            grid-column: span 12;
            background: #fff;
            border: 1px solid #e2e8f0;
            border-radius: 22px;
            box-shadow: 0 14px 32px rgba(15, 23, 42, 0.06);
        }
        .term-card--span-8 { grid-column: span 8; }
        .term-card--span-4 { grid-column: span 4; }
        .term-card__head { padding: 22px 24px 0; }
        .term-card__title { margin: 0; font-size: 20px; font-weight: 800; color: #0f172a; }
        .term-card__desc { margin: 6px 0 0; color: #64748b; font-size: 14px; }
        .term-card__body { padding: 22px 24px 24px; }
        .term-toolbar {
            display: flex;
            flex-wrap: wrap;
            gap: 14px;
            align-items: end;
        }
        .term-field {
            display: flex;
            flex-direction: column;
            gap: 8px;
            min-width: 120px;
        }
        .term-field__label { font-size: 13px; color: #64748b; font-weight: 700; }
        .term-select {
            min-width: 88px;
            height: 42px;
            padding: 0 12px;
            border: 1px solid #cbd5e1;
            border-radius: 12px;
            background: #fff;
            color: #0f172a;
            font-weight: 700;
        }
        .term-actions {
            display: flex;
            flex-wrap: wrap;
            gap: 12px;
            margin-top: 18px;
        }
        .term-btn, .term-btn input[type=submit], .term-btn input[type=button] {
            border: none;
            border-radius: 12px;
            padding: 11px 16px;
            font-weight: 700;
        }
        .term-weight-grid {
            display: grid;
            grid-template-columns: repeat(2, minmax(0, 1fr));
            gap: 14px;
        }
        .term-weight-item {
            padding: 16px;
            border-radius: 16px;
            border: 1px solid #dbeafe;
            background: linear-gradient(180deg, #f8fbff 0%, #eff6ff 100%);
        }
        .term-weight-label {
            display: block;
            margin-bottom: 10px;
            color: #475569;
            font-size: 13px;
            font-weight: 700;
            line-height: 1.5;
        }
        .term-msg {
            margin-top: 18px;
            padding: 14px 16px;
            border-radius: 14px;
            background: #fff7ed;
            border: 1px solid #fed7aa;
            color: #9a3412;
            font-weight: 700;
        }
        .term-table-wrap {
            overflow-x: auto;
            border-radius: 18px;
            border: 1px solid #e2e8f0;
            background: #fff;
        }
        .term-table-wrap table {
            width: 100%;
            min-width: 1180px;
            border-collapse: separate;
            border-spacing: 0;
        }
        .term-table-wrap th {
            position: sticky;
            top: 0;
            z-index: 1;
            background: #eff6ff;
            color: #1e3a8a;
            font-weight: 800;
            font-size: 13px;
            padding: 12px 10px;
            border-bottom: 1px solid #dbeafe;
            white-space: nowrap;
        }
        .term-table-wrap td {
            padding: 11px 10px;
            border-bottom: 1px solid #eef2f7;
            text-align: center;
            color: #0f172a;
            font-size: 13px;
            white-space: nowrap;
        }
        .term-table-wrap tr:nth-child(even) td { background: #fcfdff; }
        .term-table-wrap tr:hover td { background: #f8fbff; }
        @media (max-width: 1080px) {
            .term-card--span-8, .term-card--span-4 { grid-column: span 12; }
        }
        @media (max-width: 720px) {
            .term-page { padding: 16px; }
            .term-hero { padding: 22px 20px; }
            .term-hero__title { font-size: 26px; }
            .term-card__head, .term-card__body { padding-left: 18px; padding-right: 18px; }
            .term-weight-grid { grid-template-columns: 1fr; }
        }
    </style>

    <div class="term-page">
        <div class="term-shell">
            <section class="term-hero">
                <div class="term-hero__content">
                    <div>
                        <h1 class="term-hero__title">学期总评中心</h1>
                        <p class="term-hero__subtitle">集中完成学期成绩折算、自动评价、归档导出与班级总评查看，支持按班级快速切换。</p>
                    </div>
                    <div class="term-hero__meta">
                        当前学期
                        <strong>第 <asp:Label ID="Lbterm" runat="server"></asp:Label> 学期</strong>
                    </div>
                </div>
            </section>

            <div class="term-grid">
                <section class="term-card term-card--span-8">
                    <div class="term-card__head">
                        <h2 class="term-card__title">班级与总评操作</h2>
                        <p class="term-card__desc">先选择班级，再执行未评作品设置、总分折算、期末总评和导出操作。</p>
                    </div>
                    <div class="term-card__body">
                        <div class="term-toolbar">
                            <div class="term-field">
                                <span class="term-field__label">年级</span>
                                <asp:DropDownList ID="DDLgrade" runat="server" CssClass="term-select" AutoPostBack="True" onselectedindexchanged="DDLgrade_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="term-field">
                                <span class="term-field__label">班级</span>
                                <asp:DropDownList ID="DDLclass" runat="server" CssClass="term-select" AutoPostBack="True" onselectedindexchanged="DDLclass_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="term-actions">
                            <asp:Button ID="BtnScoresNo" runat="server" OnClick="BtnScoresNo_Click" Text="未评设置 C" CssClass="term-btn" ToolTip="所教班级未评作品全部设置为 C，即分值 6" />
                            <asp:Button ID="BtnScores" runat="server" OnClick="BtnScore_Click" Text="总分折算" CssClass="term-btn" ToolTip="先统计总分，再得出折算总分" />
                            <asp:Button ID="Btnape" runat="server" onclick="Btnape_Click" Text="期末总评" CssClass="term-btn" />
                            <asp:Button ID="BtnExcel" runat="server" OnClick="BtnExcel_Click" Text="导出 Excel" CssClass="term-btn" ToolTip="将学生期末成绩以 Excel 表格导出" />
                            <asp:Button ID="Btntermview" runat="server" Text="学期查询" OnClick="Btntermview_Click" CssClass="term-btn" />
                            <asp:Button ID="Btnback" runat="server" Text="返回作品页" OnClick="Btnback_Click" CssClass="term-btn" />
                        </div>

                        <div class="term-msg">
                            <asp:Label ID="Labelmsg" runat="server" SkinID="LabelMsgRed"></asp:Label>
                        </div>
                    </div>
                </section>

                <section class="term-card term-card--span-4">
                    <div class="term-card__head">
                        <h2 class="term-card__title">总分折算比重</h2>
                        <p class="term-card__desc">调整不同维度在总评中的百分比权重。</p>
                    </div>
                    <div class="term-card__body">
                        <div class="term-weight-grid">
                            <div class="term-weight-item">
                                <span class="term-weight-label">作品 + 小组 + 讨论 + 表单 + 测评</span>
                                <asp:DropDownList ID="DDLwork" runat="server" CssClass="term-select">
                                    <asp:ListItem Selected="True">100</asp:ListItem>
                                    <asp:ListItem>90</asp:ListItem>
                                    <asp:ListItem>80</asp:ListItem>
                                    <asp:ListItem>70</asp:ListItem>
                                    <asp:ListItem>60</asp:ListItem>
                                    <asp:ListItem>50</asp:ListItem>
                                    <asp:ListItem>40</asp:ListItem>
                                    <asp:ListItem>30</asp:ListItem>
                                    <asp:ListItem>20</asp:ListItem>
                                    <asp:ListItem>10</asp:ListItem>
                                    <asp:ListItem>0</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="term-weight-item">
                                <span class="term-weight-label">测验</span>
                                <asp:DropDownList ID="DDLexam" runat="server" CssClass="term-select">
                                    <asp:ListItem>100</asp:ListItem>
                                    <asp:ListItem>90</asp:ListItem>
                                    <asp:ListItem>80</asp:ListItem>
                                    <asp:ListItem>70</asp:ListItem>
                                    <asp:ListItem>60</asp:ListItem>
                                    <asp:ListItem Selected="True">50</asp:ListItem>
                                    <asp:ListItem>40</asp:ListItem>
                                    <asp:ListItem>30</asp:ListItem>
                                    <asp:ListItem>20</asp:ListItem>
                                    <asp:ListItem>10</asp:ListItem>
                                    <asp:ListItem>0</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="term-weight-item">
                                <span class="term-weight-label">打字成绩</span>
                                <asp:DropDownList ID="DDLtyper" runat="server" CssClass="term-select">
                                    <asp:ListItem Selected="True">0</asp:ListItem>
                                    <asp:ListItem>10</asp:ListItem>
                                    <asp:ListItem>20</asp:ListItem>
                                    <asp:ListItem>30</asp:ListItem>
                                    <asp:ListItem>40</asp:ListItem>
                                    <asp:ListItem>50</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="term-weight-item">
                                <span class="term-weight-label">课堂表现</span>
                                <asp:DropDownList ID="DDLattitude" runat="server" CssClass="term-select">
                                    <asp:ListItem Selected="True">0</asp:ListItem>
                                    <asp:ListItem>10</asp:ListItem>
                                    <asp:ListItem>20</asp:ListItem>
                                    <asp:ListItem>30</asp:ListItem>
                                    <asp:ListItem>40</asp:ListItem>
                                    <asp:ListItem>50</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="term-weight-item">
                                <span class="term-weight-label">表单测评</span>
                                <asp:DropDownList ID="DDLsurvey" runat="server" CssClass="term-select">
                                    <asp:ListItem Selected="True">0</asp:ListItem>
                                    <asp:ListItem>10</asp:ListItem>
                                    <asp:ListItem>20</asp:ListItem>
                                    <asp:ListItem>30</asp:ListItem>
                                    <asp:ListItem>40</asp:ListItem>
                                    <asp:ListItem>50</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="term-weight-item">
                                <span class="term-weight-label">签到情况</span>
                                <asp:DropDownList ID="DDLsignin" runat="server" CssClass="term-select">
                                    <asp:ListItem Selected="True">0</asp:ListItem>
                                    <asp:ListItem>10</asp:ListItem>
                                    <asp:ListItem>20</asp:ListItem>
                                    <asp:ListItem>30</asp:ListItem>
                                    <asp:ListItem>40</asp:ListItem>
                                    <asp:ListItem>50</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </section>

                <section class="term-card term-card--span-12">
                    <div class="term-card__head">
                        <h2 class="term-card__title">班级学期成绩表</h2>
                        <p class="term-card__desc">展示当前班级学期综合数据，可结合上方操作按钮进行实时刷新。</p>
                    </div>
                    <div class="term-card__body">
                        <div class="term-table-wrap">
                            <asp:GridView ID="GVCourse" runat="server" AutoGenerateColumns="False" GridLines="None" Width="100%" onrowdatabound="GVCourse_RowDataBound">
                                <Columns>
                                    <asp:BoundField HeaderText="序号" />
                                    <asp:BoundField DataField="Snum" HeaderText="学号" />
                                    <asp:BoundField DataField="Sgradeclass" HeaderText="班级" />
                                    <asp:HyperLinkField DataNavigateUrlFields="Snum" DataNavigateUrlFormatString="studentwork.aspx?snum={0}" DataTextField="Sname" HeaderText="姓名" Target="_blank" />
                                    <asp:BoundField DataField="Sscore" HeaderText="作品" />
                                    <asp:BoundField DataField="Sgscore" HeaderText="小组" />
                                    <asp:BoundField DataField="Spscore" HeaderText="讨论" />
                                    <asp:BoundField DataField="Stxtform" HeaderText="表单" />
                                    <asp:BoundField DataField="Svscore" HeaderText="测验" />
                                    <asp:BoundField DataField="Schinese" HeaderText="拼音" />
                                    <asp:BoundField DataField="Sfscore" HeaderText="英语" />
                                    <asp:BoundField DataField="Stscore" HeaderText="中文" />
                                    <asp:BoundField DataField="Sidle" HeaderText="测评" />
                                    <asp:BoundField DataField="Sattitude" HeaderText="表现" />
                                    <asp:BoundField DataField="Sallscore" HeaderText="总分" />
                                    <asp:BoundField DataField="Sape" HeaderText="评定" />
                                    <asp:BoundField DataField="Stenscore" HeaderText="等级" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </section>
            </div>
        </div>
    </div>
</asp:Content>
