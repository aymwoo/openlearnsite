<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="works.aspx.cs" Inherits="Teacher_works" ResponseEncoding="utf-8" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link rel="stylesheet" type="text/css" href="../webform/bootstrap-icons.min.css" />

    <style>
        .course-page { padding: 24px; }
        .course-shell { display: flex; flex-direction: column; gap: 24px; }
        .course-hero {
            position: relative;
            overflow: hidden;
            border-radius: 24px;
            padding: 28px 32px;
            background: linear-gradient(135deg, #0f172a 0%, #4338ca 48%, #7c3aed 100%);
            color: #eef2ff;
            box-shadow: 0 20px 46px rgba(15, 23, 42, 0.16);
        }
        .course-hero::after {
            content: "";
            position: absolute;
            top: -48px;
            right: -52px;
            width: 220px;
            height: 220px;
            border-radius: 999px;
            background: rgba(255,255,255,0.08);
        }
        .course-hero-content {
            position: relative;
            z-index: 1;
            display: flex;
            justify-content: space-between;
            gap: 20px;
            flex-wrap: wrap;
            align-items: flex-start;
        }
        .course-eyebrow {
            display: inline-block;
            padding: 6px 10px;
            border-radius: 999px;
            background: rgba(255,255,255,0.12);
            border: 1px solid rgba(255,255,255,0.14);
            font-size: 12px;
            font-weight: 800;
            letter-spacing: 0.08em;
            text-transform: uppercase;
        }
        .course-title { margin: 14px 0 0; font-size: 34px; font-weight: 800; letter-spacing: 0.04em; }
        .course-subtitle { margin: 10px 0 0; max-width: 760px; color: rgba(238, 242, 255, 0.9); line-height: 1.75; }
        .course-hero-action { display: flex; gap: 12px; flex-wrap: wrap; }
        .course-secondary-btn, .course-primary-btn {
            border-radius: 14px;
            min-width: 118px;
            height: 44px;
            padding: 0 18px;
            box-sizing: border-box;
            font-weight: 700;
            font-size: 14px;
            line-height: 44px;
            text-align: center;
            cursor: pointer;
            transition: transform 0.18s ease, box-shadow 0.18s ease;
        }
        .course-secondary-btn {
            background: rgba(255,255,255,0.96);
            border: 1px solid #c7d2fe;
            color: #4338ca;
        }
        .course-primary-btn {
            background: #22c55e;
            border: 1px solid #4ade80;
            color: #052e16;
        }
        .course-term-btn {
            background: linear-gradient(135deg, rgba(59,130,246,0.22) 0%, rgba(67,56,202,0.3) 100%);
            border-color: rgba(191,219,254,0.5);
            color: #f8fafc;
            box-shadow: 0 12px 28px rgba(30, 41, 59, 0.22);
            backdrop-filter: blur(10px);
        }
        .course-secondary-btn:hover, .course-primary-btn:hover { transform: translateY(-1px); }
        .course-term-btn:hover {
            background: linear-gradient(135deg, #1d4ed8 0%, #3730a3 100%);
            border-color: rgba(191,219,254,0.72);
            color: #ffffff;
            box-shadow: 0 16px 34px rgba(30, 64, 175, 0.34);
        }
        .course-term-btn:focus {
            outline: none;
            box-shadow: 0 0 0 3px rgba(191,219,254,0.22), 0 16px 34px rgba(15, 23, 42, 0.24);
        }
        .course-toolbar, .course-table-panel {
            background: #fff;
            border: 1px solid #e2e8f0;
            border-radius: 22px;
            box-shadow: 0 14px 32px rgba(15, 23, 42, 0.06);
        }
        .course-toolbar { padding: 22px 24px; }
        .course-toolbar-grid {
            display: grid;
            grid-template-columns: repeat(2, minmax(0, 1fr));
            gap: 16px;
        }
        .course-field {
            padding: 16px 18px;
            border-radius: 18px;
            background: linear-gradient(180deg, #f8fbff 0%, #eff6ff 100%);
            border: 1px solid #dbeafe;
        }
        .course-field-label {
            display: block;
            margin-bottom: 8px;
            font-size: 13px;
            color: #64748b;
            font-weight: 700;
        }
        .course-select {
            min-width: 88px;
            height: 42px;
            padding: 0 12px;
            border: 1px solid #cbd5e1;
            border-radius: 12px;
            background: #fff;
            color: #0f172a;
            font-weight: 700;
        }
        .course-note {
            display: inline-flex;
            align-items: center;
            min-height: 42px;
            padding: 0 14px;
            border-radius: 12px;
            background: #ffffff;
            border: 1px solid #cbd5e1;
            color: #1e40af;
            font-weight: 800;
        }
        .course-table-panel { overflow: hidden; }
        .course-table-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            gap: 16px;
            padding: 22px 24px 0;
        }
        .course-table-title { margin: 0; font-size: 20px; font-weight: 800; color: #0f172a; }
        .course-table-desc { margin: 6px 0 0; color: #64748b; font-size: 14px; }
        .course-table-chip {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            padding: 8px 12px;
            border-radius: 999px;
            background: #eef2ff;
            color: #4338ca;
            font-size: 12px;
            font-weight: 800;
        }
        .course-table-wrap {
            padding: 22px 24px 24px;
            overflow-x: auto;
        }
        .course-grid {
            border-collapse: separate;
            border-spacing: 0;
            min-width: 920px;
        }
        .course-grid th {
            position: sticky;
            top: 0;
            background: #eff6ff;
            color: #1e3a8a;
            font-size: 13px;
            font-weight: 800;
            padding: 12px 10px;
            border-bottom: 1px solid #dbeafe;
        }
        .course-grid td {
            padding: 12px 10px;
            border-bottom: 1px solid #eef2f7;
            background: #fff;
            color: #0f172a;
            font-size: 13px;
        }
        .course-grid tr:nth-child(even) td { background: #fcfdff; }
        .course-badge {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            min-width: 38px;
            padding: 4px 10px;
            border-radius: 999px;
            font-size: 12px;
            font-weight: 800;
            text-decoration: none;
        }
        .course-badge-warning { background: #fff7ed; color: #c2410c; }
        .course-icon-btn {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 34px;
            height: 34px;
            border-radius: 999px;
            background: #eef2ff;
            color: #4338ca;
            text-decoration: none;
        }
        .course-pager {
            display: flex;
            justify-content: center;
            align-items: center;
            flex-wrap: wrap;
            gap: 10px;
            padding: 16px 0 4px;
        }
        .course-pager a, .course-pager span {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            min-height: 34px;
        }
        .course-pager a {
            padding: 0 12px;
            border-radius: 10px;
            background: #f8fafc;
            border: 1px solid #e2e8f0;
            color: #334155 !important;
            text-decoration: none;
            font-weight: 700;
        }
        @media (max-width: 820px) {
            .course-toolbar-grid { grid-template-columns: 1fr; }
        }
        @media (max-width: 640px) {
            .course-page { padding: 16px; }
            .course-hero { padding: 22px 20px; }
            .course-title { font-size: 28px; }
            .course-toolbar, .course-table-wrap { padding-left: 18px; padding-right: 18px; }
            .course-table-header { padding-left: 18px; padding-right: 18px; }
        }
    </style>

    <div class="course-page">
        <div class="course-shell">
            <section class="course-hero">
                <div class="course-hero-content">
                    <div>
                        <span class="course-eyebrow">Teacher Workspace</span>
                        <h1 class="course-title">作品评价</h1>
                        <p class="course-subtitle">按年级筛选当前发布的学案，快速预览并评价各班级学生提交的作品情况。</p>
                    </div>
                    <div class="course-hero-action">
                        <button type="button" id="pg" onclick="package()" class="course-secondary-btn" style="color: #4f46e5; border-color: #c7d2fe; display: inline-flex; align-items: center; gap: 0.4rem;">
                            <i class="bi bi-box-seam"></i> 作品打包
                        </button>
                        <asp:Button ID="Btnterm" runat="server" Text="学期总评" onclick="Btnterm_Click"
                            ToolTip="跳转到学期总评页面" CssClass="course-primary-btn course-term-btn" />
                    </div>
                </div>
            </section>

            <section class="course-toolbar">
                <div class="course-toolbar-grid">
                    <div class="course-field">
                        <span class="course-field-label">作品选择</span>
                        <div style="display: flex; align-items: center; gap: 0.5rem;">
                            <asp:DropDownList ID="DDLgrade" runat="server" Width="80px"
                                EnableTheming="True" AutoPostBack="True"
                                onselectedindexchanged="DDLgrade_SelectedIndexChanged" CssClass="course-select">
                            </asp:DropDownList>
                            <span class="course-field-label" style="margin-bottom: 0;">年级</span>
                        </div>
                    </div>
                    <div class="course-field">
                        <span class="course-field-label">当前学期</span>
                        <asp:Label ID="Labelmsg" runat="server" CssClass="course-note"></asp:Label>
                    </div>
                </div>
            </section>

            <section class="course-table-panel">
                <div class="course-table-header">
                    <div>
                        <h2 class="course-table-title">学案列表</h2>
                        <p class="course-table-desc">查看各个学案下各班级提交的作品，点击未评数或查看图标进入详细评价页面。</p>
                    </div>
                    <span class="course-table-chip">最多每页 20 条</span>
                </div>

                <div class="course-table-wrap custom-scrollbar">
                    <asp:GridView ID="GVCourse" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" DataKeyNames="Cid"
                        PageSize="20" Width="100%" CssClass="course-grid"
                        onpageindexchanging="GVCourse_PageIndexChanging"
                        onrowdatabound="GVCourse_RowDataBound" CellPadding="0"
                        EnableModelValidation="True" ForeColor="#111111" GridLines="None">
                        <AlternatingRowStyle BackColor="#FBFDFF" />
                        <Columns>
                            <asp:BoundField DataField="Cid" HeaderText="序号" InsertVisible="False"
                                ReadOnly="True" SortExpression="Cid" >
                                <ControlStyle Width="50px" />
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:HyperLinkField DataTextField="Ctitle" HeaderText="学案" >
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:HyperLinkField>
                            <asp:BoundField DataField="Cclass" HeaderText="类型" SortExpression="Cclass" >
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="未评数">
                                <ItemTemplate>
                                    <asp:HyperLink ID="HlNoCheck" runat="server" CssClass="course-badge course-badge-warning"></asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="评价">
                                <ItemTemplate>
                                    <asp:HyperLink ID="HLCheck" runat="server" Target="_blank"
                                        NavigateUrl='<%# "workcheck.aspx?cid=" + Eval("Cid") + "&grade=" + Eval("Cobj") %>'
                                        CssClass="course-icon-btn course-icon-btn-sm course-icon-info mx-auto">
                                        <i class="bi bi-eye"></i>
                                    </asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Cdate" HeaderText="日期" SortExpression="Cdate" >
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" Width="160px" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="#F8FAFC" Font-Bold="True" ForeColor="#0F172A" />
                        <HeaderStyle BackColor="#F8FAFC" Font-Bold="True" ForeColor="#475569" />
                        <PagerStyle CssClass="course-pager-row" BackColor="#F8FAFC" ForeColor="#111111" HorizontalAlign="Center" />
                        <pagertemplate>
                            <div class="course-pager">
                                <span>第<asp:Label ID="lblPageIndex" runat="server" text="<%# ((GridView)Container.Parent.Parent).PageIndex + 1  %>" />页</span>
                                <span>共<asp:Label ID="lblPageCount" runat="server" text="<%# ((GridView)Container.Parent.Parent).PageCount  %>" />页</span>
                                <asp:LinkButton ID="btnFirst" runat="server" causesvalidation="False"
                                    commandargument="First" commandname="Page" Font-Underline="False"
                                    ForeColor="Black" text="首页" />
                                <asp:LinkButton ID="btnPrev" runat="server" causesvalidation="False"
                                    commandargument="Prev" commandname="Page" Font-Underline="False"
                                    ForeColor="Black" text="上一页" />
                                <asp:LinkButton ID="btnNext" runat="server" causesvalidation="False"
                                    commandargument="Next" commandname="Page" Font-Underline="False"
                                    ForeColor="Black" text="下一页" />
                                <asp:LinkButton ID="btnLast" runat="server" causesvalidation="False"
                                    commandargument="Last" commandname="Page" Font-Underline="False"
                                    ForeColor="Black" text="尾页" />
                            </div>
                        </pagertemplate>
                        <RowStyle BackColor="#FFFFFF" />
                        <SelectedRowStyle BackColor="#E0E7FF" Font-Bold="True" ForeColor="#312E81" />
                    </asp:GridView>
                </div>
            </section>
        </div>

        
    </div>
    <script type="text/javascript" src="../js/works.js"></script>
</asp:Content>
