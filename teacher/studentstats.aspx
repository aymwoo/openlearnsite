<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="studentstats.aspx.cs" Inherits="Teacher_studentstats" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <style>
        .stats-page { padding: 24px; }
        .stats-shell { display: flex; flex-direction: column; gap: 24px; }
        .stats-hero {
            position: relative;
            overflow: hidden;
            border-radius: 24px;
            padding: 28px 32px;
            background: linear-gradient(135deg, #0f172a 0%, #4f46e5 48%, #06b6d4 100%);
            color: #eff6ff;
            box-shadow: 0 20px 46px rgba(15,23,42,0.16);
        }
        .stats-hero::after {
            content: "";
            position: absolute;
            right: -48px;
            top: -48px;
            width: 220px;
            height: 220px;
            border-radius: 999px;
            background: rgba(255,255,255,0.08);
        }
        .stats-hero__content {
            position: relative;
            z-index: 1;
            display: flex;
            justify-content: space-between;
            gap: 20px;
            flex-wrap: wrap;
            align-items: flex-start;
        }
        .stats-hero__title { margin: 0; font-size: 32px; font-weight: 800; }
        .stats-hero__subtitle { margin: 10px 0 0; max-width: 760px; color: rgba(239,246,255,0.9); line-height: 1.75; }
        .stats-toolbar {
            display: flex;
            gap: 10px;
            flex-wrap: wrap;
            align-items: center;
            padding: 10px 14px;
            border-radius: 14px;
            background: rgba(15,23,42,0.22);
            border: 1px solid rgba(255,255,255,0.16);
        }
        .stats-select, .stats-btn {
            height: 40px;
            border-radius: 12px;
            font-weight: 700;
        }
        .stats-select { padding: 0 12px; border: none; color: #0f172a; }
        .stats-btn {
            padding: 0 14px;
            border: 1px solid rgba(255,255,255,0.18);
            background: rgba(255,255,255,0.12);
            color: #fff;
        }
        .stats-empty {
            text-align: center;
            padding: 48px 20px;
            color: #64748b;
            font-size: 16px;
            background: #fff;
            border: 1px solid #e2e8f0;
            border-radius: 22px;
            box-shadow: 0 14px 34px rgba(15,23,42,0.06);
        }
        .stats-empty__icon { font-size: 52px; margin-bottom: 12px; }
        .stats-filter-card, .stats-grid-card {
            background: #fff;
            border: 1px solid #e2e8f0;
            border-radius: 22px;
            box-shadow: 0 14px 34px rgba(15,23,42,0.06);
        }
        .stats-filter-card { padding: 22px 24px; }
        .stats-filter-grid {
            display: grid;
            grid-template-columns: repeat(2, minmax(0, 1fr));
            gap: 16px;
        }
        .stats-filter-item {
            padding: 16px 18px;
            border-radius: 18px;
            background: linear-gradient(180deg, #f8fbff 0%, #eff6ff 100%);
            border: 1px solid #dbeafe;
            display: flex;
            align-items: center;
            gap: 10px;
            flex-wrap: wrap;
        }
        .stats-filter-item label { color: #64748b; font-size: 13px; font-weight: 700; }
        .stats-summary {
            display: grid;
            grid-template-columns: repeat(6, minmax(0, 1fr));
            gap: 14px;
        }
        .stats-card {
            background: #fff;
            border: 1px solid #e2e8f0;
            border-radius: 20px;
            padding: 18px 16px;
            text-align: center;
            box-shadow: 0 10px 24px rgba(15,23,42,0.05);
        }
        .stats-card__value { font-size: 30px; font-weight: 800; line-height: 1.1; }
        .stats-card__label { margin-top: 6px; color: #64748b; font-size: 13px; }
        .stats-card--green .stats-card__value { color: #059669; }
        .stats-card--blue .stats-card__value { color: #2563eb; }
        .stats-card--orange .stats-card__value { color: #ea580c; }
        .stats-card--red .stats-card__value { color: #dc2626; }
        .stats-grid-card__head { display:flex; justify-content:space-between; align-items:center; gap:16px; padding:22px 24px 0; flex-wrap:wrap; }
        .stats-grid-card__title { margin:0; font-size:20px; font-weight:800; color:#0f172a; }
        .stats-grid-card__desc { margin:6px 0 0; color:#64748b; font-size:14px; }
        .stats-grid-card__body { padding:22px 24px 24px; }
        .stats-table-wrap { overflow-x:auto; border-radius:18px; border:1px solid #e2e8f0; }
        .stats-table-wrap table { width:100%; min-width:1500px; border-collapse:separate; border-spacing:0; }
        .stats-table-wrap th { position:sticky; top:0; background:#eff6ff; color:#1e3a8a; font-size:13px; font-weight:800; padding:12px 10px; border-bottom:1px solid #dbeafe; text-align:center; }
        .stats-table-wrap td { padding:11px 10px; border-bottom:1px solid #eef2f7; background:#fff; color:#0f172a; font-size:13px; text-align:center; }
        .stats-table-wrap tr:nth-child(even) td { background:#fcfdff; }
        .add-score { color: #059669; font-weight: bold; }
        .sub-score { color: #dc2626; font-weight: bold; }
        .reason-cell { font-size: 12px; text-align: left !important; max-width: 220px; word-break: break-all; }
        .reason-add { color: #059669; }
        .reason-sub { color: #dc2626; }
        @media (max-width: 1180px) { .stats-summary { grid-template-columns: repeat(3, minmax(0, 1fr)); } }
        @media (max-width: 820px) { .stats-filter-grid { grid-template-columns: 1fr; } .stats-summary { grid-template-columns: repeat(2, minmax(0, 1fr)); } }
        @media (max-width: 640px) {
            .stats-page { padding:16px; }
            .stats-hero { padding:22px 20px; }
            .stats-hero__title { font-size:26px; }
            .stats-summary { grid-template-columns: 1fr; }
            .stats-grid-card__head, .stats-grid-card__body, .stats-filter-card { padding-left:18px; padding-right:18px; }
        }
    </style>

    <div class="stats-page">
        <div class="stats-shell">
            <section class="stats-hero">
                <div class="stats-hero__content">
                    <div>
                        <h1 class="stats-hero__title">学生统计中心</h1>
                        <p class="stats-hero__subtitle">查看当前上课班级在指定学期与课程下的签到、作业、表现分和综合得分等统计结果。</p>
                    </div>
                    <div class="stats-toolbar">
                        <asp:DropDownList ID="DDLgrade" runat="server" AutoPostBack="True" onselectedindexchanged="DDLgrade_SelectedIndexChanged" CssClass="stats-select"></asp:DropDownList>
                        <span>年级</span>
                        <asp:DropDownList ID="DDLclass" runat="server" AutoPostBack="True" onselectedindexchanged="DDLclass_SelectedIndexChanged" CssClass="stats-select"></asp:DropDownList>
                        <span>班级</span>
                        <asp:Button ID="BtnReturn" runat="server" Text="返回" CssClass="stats-btn" onclick="BtnReturn_Click" ToolTip="返回管理页面" />
                    </div>
                </div>
            </section>

            <asp:Panel ID="PanelNoClass" runat="server" Visible="false">
                <div class="stats-empty">
                    <div class="stats-empty__icon">📊</div>
                    <div>当前没有正在上课的班级，请先在“开始上课”页面选择班级开始上课。</div>
                </div>
            </asp:Panel>

            <asp:Panel ID="PanelContent" runat="server" Visible="true">
                <section class="stats-filter-card">
                    <div class="stats-filter-grid">
                        <div class="stats-filter-item">
                            <label>学期</label>
                            <asp:DropDownList ID="DDLterm" runat="server" AutoPostBack="True" onselectedindexchanged="DDLterm_SelectedIndexChanged" CssClass="stats-select"></asp:DropDownList>
                        </div>
                        <div class="stats-filter-item">
                            <label>课程</label>
                            <asp:DropDownList ID="DDLcourse" runat="server" AutoPostBack="True" onselectedindexchanged="DDLcourse_SelectedIndexChanged" CssClass="stats-select"></asp:DropDownList>
                        </div>
                    </div>
                </section>

                <div class="stats-summary">
                    <div class="stats-card stats-card--green"><div class="stats-card__value"><asp:Label ID="lblTotalStudents" runat="server">0</asp:Label></div><div class="stats-card__label">学生总数</div></div>
                    <div class="stats-card stats-card--blue"><div class="stats-card__value"><asp:Label ID="lblTotalSignin" runat="server">0</asp:Label></div><div class="stats-card__label">签到总次数</div></div>
                    <div class="stats-card stats-card--orange"><div class="stats-card__value"><asp:Label ID="lblTotalWorks" runat="server">0</asp:Label></div><div class="stats-card__label">作业提交总数</div></div>
                    <div class="stats-card"><div class="stats-card__value"><asp:Label ID="lblAvgScore" runat="server">0</asp:Label></div><div class="stats-card__label">平均综合分</div></div>
                    <div class="stats-card stats-card--green"><div class="stats-card__value"><asp:Label ID="lblTotalAddCount" runat="server">0</asp:Label></div><div class="stats-card__label">加分总次数</div></div>
                    <div class="stats-card stats-card--red"><div class="stats-card__value"><asp:Label ID="lblTotalSubCount" runat="server">0</asp:Label></div><div class="stats-card__label">扣分总次数</div></div>
                </div>

                <section class="stats-grid-card">
                    <div class="stats-grid-card__head">
                        <div>
                            <h2 class="stats-grid-card__title">学生详细统计</h2>
                            <p class="stats-grid-card__desc">展示签到、作业、表现分、加减分原因和综合分等明细。</p>
                        </div>
                    </div>
                    <div class="stats-grid-card__body">
                        <div class="stats-table-wrap">
                            <asp:GridView ID="GVStats" runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" PageSize="30" onpageindexchanging="GVStats_PageIndexChanging" OnRowDataBound="GVStats_RowDataBound">
                                <Columns>
                                    <asp:BoundField HeaderText="序号" DataField="RowNum" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="学号" DataField="Snum" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="姓名" DataField="Sname" ItemStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="座位" DataField="Sseat" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="应到" DataField="ShouldCount" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="签到" DataField="SignCount" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="作业数" DataField="WorkCount" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="作业分" DataField="WorkScore" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="作品分" DataField="Sscore" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="测验分" DataField="Squiz" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="加分次数" DataField="AddCount" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="扣分次数" DataField="SubCount" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="加分分数" DataField="TotalAddScore" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="扣分分数" DataField="TotalSubScore" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="表现分" DataField="Sattitude" ItemStyle-Width="60px" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField HeaderText="加分原因" DataField="AddReasons" ItemStyle-CssClass="reason-cell reason-add" />
                                    <asp:BoundField HeaderText="扣分原因" DataField="SubReasons" ItemStyle-CssClass="reason-cell reason-sub" />
                                    <asp:BoundField HeaderText="综合分" DataField="Stenscore" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </section>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
