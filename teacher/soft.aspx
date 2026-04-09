<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="soft.aspx.cs" Inherits="Teacher_soft" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <style>
        .soft-page { padding: 24px; }
        .soft-shell { display: flex; flex-direction: column; gap: 24px; }
        .soft-hero {
            position: relative;
            overflow: hidden;
            border-radius: 24px;
            padding: 28px 32px;
            background: linear-gradient(135deg, #0f172a 0%, #0ea5e9 52%, #22c55e 100%);
            color: #ecfeff;
            box-shadow: 0 20px 46px rgba(15, 23, 42, 0.16);
        }
        .soft-hero::after {
            content: "";
            position: absolute;
            top: -48px;
            right: -48px;
            width: 210px;
            height: 210px;
            border-radius: 999px;
            background: rgba(255,255,255,0.08);
        }
        .soft-hero__content {
            position: relative;
            z-index: 1;
            display: flex;
            justify-content: space-between;
            align-items: flex-start;
            gap: 20px;
            flex-wrap: wrap;
        }
        .soft-hero__title { margin: 0; font-size: 32px; font-weight: 800; letter-spacing: 0.04em; }
        .soft-hero__subtitle { margin: 10px 0 0; max-width: 760px; color: rgba(236,254,255,0.9); line-height: 1.75; }
        .soft-hero__action {
            display: inline-flex;
            align-items: center;
            gap: 10px;
            padding: 10px 14px;
            border-radius: 14px;
            background: rgba(15,23,42,0.22);
            border: 1px solid rgba(255,255,255,0.16);
            color: #fff;
            text-decoration: none;
            font-weight: 700;
        }
        .soft-card {
            background: #fff;
            border: 1px solid #e2e8f0;
            border-radius: 22px;
            box-shadow: 0 14px 34px rgba(15, 23, 42, 0.06);
            overflow: hidden;
        }
        .soft-card__head {
            display: flex;
            justify-content: space-between;
            align-items: center;
            gap: 16px;
            padding: 22px 24px 0;
            flex-wrap: wrap;
        }
        .soft-card__title { margin: 0; font-size: 20px; font-weight: 800; color: #0f172a; }
        .soft-card__filter {
            display: flex;
            align-items: center;
            gap: 12px;
            flex-wrap: wrap;
            padding: 14px 16px;
            border-radius: 16px;
            background: linear-gradient(180deg, #f8fbff 0%, #eff6ff 100%);
            border: 1px solid #dbeafe;
        }
        .soft-card__label { color: #64748b; font-size: 13px; font-weight: 700; }
        .soft-card__body { padding: 22px 24px 24px; }
        .soft-select {
            min-width: 180px;
            height: 42px;
            padding: 0 12px;
            border: 1px solid #cbd5e1;
            border-radius: 12px;
            background: #fff;
            color: #0f172a;
            font-weight: 700;
        }
        .soft-count {
            display: inline-flex;
            align-items: center;
            min-height: 42px;
            padding: 0 14px;
            border-radius: 12px;
            background: #ffffff;
            border: 1px solid #cbd5e1;
            color: #0f766e;
            font-weight: 800;
        }
        .soft-table-wrap {
            overflow-x: auto;
            border-radius: 18px;
            border: 1px solid #e2e8f0;
            background: #fff;
        }
        .soft-table-wrap table {
            width: 100%;
            min-width: 1080px;
            border-collapse: separate;
            border-spacing: 0;
        }
        .soft-table-wrap th {
            position: sticky;
            top: 0;
            background: #eff6ff;
            color: #1e3a8a;
            font-size: 13px;
            font-weight: 800;
            padding: 12px 10px;
            border-bottom: 1px solid #dbeafe;
            text-align: center;
        }
        .soft-table-wrap td {
            padding: 12px 10px;
            border-bottom: 1px solid #eef2f7;
            color: #0f172a;
            font-size: 13px;
            text-align: center;
            background: #fff;
        }
        .soft-table-wrap tr:nth-child(even) td { background: #fcfdff; }
        .soft-toggle-btn {
            border: none;
            border-radius: 10px;
            padding: 8px 12px;
            background: #eef2ff;
            color: #4338ca;
            font-weight: 700;
            cursor: pointer;
        }
        .soft-pager {
            padding: 16px 0 4px;
            background: #fff;
        }
        .soft-pager__info {
            display: inline-flex;
            align-items: center;
            gap: 8px;
            color: #475569;
            font-weight: 700;
        }
        .soft-pager__info-num { color: #2563eb; font-weight: 800; }
        .soft-pager__btns {
            display: inline-flex;
            flex-wrap: wrap;
            gap: 10px;
            margin-left: 14px;
        }
        .soft-pager__btn {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            min-height: 34px;
            padding: 0 12px;
            border-radius: 10px;
            background: #f8fafc;
            border: 1px solid #e2e8f0;
            color: #334155 !important;
            text-decoration: none;
            font-weight: 700;
        }
        @media (max-width: 640px) {
            .soft-page { padding: 16px; }
            .soft-hero { padding: 22px 20px; }
            .soft-hero__title { font-size: 26px; }
            .soft-card__head, .soft-card__body { padding-left: 18px; padding-right: 18px; }
            .soft-card__filter { width: 100%; }
            .soft-pager__btns { margin-left: 0; }
        }
    </style>

    <div class="soft-page">
        <div class="soft-shell">
            <div class="soft-hero">
                <div class="soft-hero__content">
                    <div>
                        <h1 class="soft-hero__title">资源管理</h1>
                        <p class="soft-hero__subtitle">浏览、添加和维护教学资源，按分类筛选查看列表，快速切换发布状态与下载内容。</p>
                    </div>
                    <asp:HyperLink ID="HLSoftAdd" runat="server" NavigateUrl="~/teacher/softadd.aspx" CssClass="soft-hero__action">+ 添加资源</asp:HyperLink>
                </div>
            </div>

            <div class="soft-card">
                <div class="soft-card__head">
                    <h2 class="soft-card__title">资源列表</h2>
                    <div class="soft-card__filter">
                        <span class="soft-card__label">分类筛选</span>
                        <asp:DropDownList ID="ddlcategory" runat="server" AutoPostBack="True" onselectedindexchanged="ddlcategory_SelectedIndexChanged" CssClass="soft-select"></asp:DropDownList>
                        <asp:Label ID="Label1" runat="server" CssClass="soft-count"></asp:Label>
                    </div>
                </div>
                <div class="soft-card__body">
                    <div class="soft-table-wrap">
                        <asp:GridView ID="GVSource" runat="server" AllowPaging="True" AutoGenerateColumns="False" PageSize="20" Width="100%" onpageindexchanging="GVSource_PageIndexChanging" onrowdatabound="GVSource_RowDataBound" EnableModelValidation="True" onrowcommand="GVSource_RowCommand" CellPadding="0" GridLines="None">
                            <AlternatingRowStyle CssClass="soft-row--alt" />
                            <Columns>
                                <asp:BoundField HeaderText="序号" />
                                <asp:BoundField DataField="Fclass" HeaderText="属性" />
                                <asp:HyperLinkField DataNavigateUrlFields="Fid" DataNavigateUrlFormatString="~/teacher/softview.aspx?fid={0}" DataTextField="Ftitle" HeaderText="标题" />
                                <asp:BoundField DataField="Ffiletype" HeaderText="格式" />
                                <asp:BoundField DataField="Fhit" HeaderText="下载" />
                                <asp:BoundField DataField="Fopen" HeaderText="学分" />
                                <asp:HyperLinkField DataNavigateUrlFields="Furl" HeaderText="下载" Text="获取" Target="_blank" />
                                <asp:CheckBoxField DataField="Fhide" HeaderText="隐藏" ReadOnly="True" />
                                <asp:TemplateField ShowHeader="False" HeaderText="状态">
                                    <ItemTemplate>
                                        <asp:Button ID="ImageButton1" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Fid") %>' CommandName="Change" Text="切换状态" ToolTip="发布：无或隐藏：√" CssClass="soft-toggle-btn" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Fdate" HeaderText="修改日期" />
                                <asp:HyperLinkField DataNavigateUrlFields="Fid,Furl" DataNavigateUrlFormatString="~/teacher/softdel.aspx?fid={0}&amp;&amp;furl={1}" Text="删除" HeaderText="管理" />
                            </Columns>
                            <PagerStyle CssClass="soft-pager" />
                            <PagerTemplate>
                                <span class="soft-pager__info">
                                    第 <asp:Label ID="lblPageIndex" runat="server" text="<%# ((GridView)Container.Parent.Parent).PageIndex + 1  %>" CssClass="soft-pager__info-num" /> 页
                                    共 <asp:Label ID="lblPageCount" runat="server" text="<%# ((GridView)Container.Parent.Parent).PageCount  %>" CssClass="soft-pager__info-num" /> 页
                                </span>
                                <div class="soft-pager__btns">
                                    <asp:LinkButton ID="btnFirst" runat="server" causesvalidation="False" commandargument="First" commandname="Page" text="首页" CssClass="soft-pager__btn" />
                                    <asp:LinkButton ID="btnPrev" runat="server" causesvalidation="False" commandargument="Prev" commandname="Page" text="上一页" CssClass="soft-pager__btn" />
                                    <asp:LinkButton ID="btnNext" runat="server" causesvalidation="False" commandargument="Next" commandname="Page" text="下一页" CssClass="soft-pager__btn" />
                                    <asp:LinkButton ID="btnLast" runat="server" causesvalidation="False" commandargument="Last" commandname="Page" text="尾页" CssClass="soft-pager__btn" />
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
