<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master"  StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="typechinese.aspx.cs" Inherits="Teacher_typechinese" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/typing-admin.css" rel="stylesheet" />
    <style type="text/css">
        .typing-chinese-page {
            --typing-admin-page-bg: linear-gradient(180deg, #fdfaf6 0%, #fff7ed 100%);
            --typing-admin-hero-bg: linear-gradient(135deg, #9a3412 0%, #ea580c 55%, #f59e0b 100%);
            --typing-admin-hero-shadow: 0 22px 45px -28px rgba(234, 88, 12, 0.72);
            --typing-admin-primary-bg: #ea580c;
            --typing-admin-primary-hover: #c2410c;
            --typing-admin-primary-shadow: 0 14px 24px -18px rgba(234, 88, 12, 0.85);
            --typing-admin-secondary-border: #fdba74;
            --typing-admin-secondary-bg: #fff7ed;
            --typing-admin-secondary-hover: #ffedd5;
            --typing-admin-secondary-fg: #c2410c;
        }
    </style>
    <div class="typing-admin-page typing-chinese-page">
        <div class="typing-admin-shell">
            <section class="typing-admin-hero">
                <div class="typing-admin-hero-content">
                    <div class="typing-admin-eyebrow">Pinyin Practice</div>
                    <h1 class="typing-admin-title">拼音词语管理</h1>
                    <p class="typing-admin-subtitle">集中维护拼音词语练习内容，支持分页浏览、编辑和年级范围配置。</p>
                </div>
            </section>

            <section class="typing-admin-panel">
                <div class="typing-admin-toolbar">
                    <div>
                        <h2 class="typing-admin-section-title">词语列表</h2>
                        <p class="typing-admin-section-desc">浏览当前拼音词语标题，快速进入查看或编辑页面。</p>
                    </div>
                    <div class="typing-admin-action-row">
                        <asp:Button ID="BtnTypeSet" runat="server" Text="打字设置"
                            onclick="BtnTypeSet_Click" CssClass="typing-admin-btn typing-admin-btn--secondary" />
                        <asp:Button ID="BtnAdd" runat="server" Text="词语添加"
                            onclick="BtnAdd_Click" CssClass="typing-admin-btn typing-admin-btn--primary" />
                    </div>
                </div>
                <div class="typing-admin-grid-wrap">
                    <asp:GridView ID="GVType" runat="server" AllowPaging="True"
                    AutoGenerateColumns="False" CellPadding="5" SkinID="GridViewInfo"
                    PageSize="20" Width="98%" onpageindexchanging="GVType_PageIndexChanging"
                    onrowdatabound="GVType_RowDataBound" EnableModelValidation="True">
                    <Columns>
                        <asp:BoundField HeaderText="序号" >
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" Width="60px" />
                        </asp:BoundField>
                        <asp:HyperLinkField DataNavigateUrlFields="Nid" 
                            DataNavigateUrlFormatString="typechineseshow.aspx?nid={0}" DataTextField="Ntitle" 
                            HeaderText="词语标题">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField DataNavigateUrlFields="Nid" 
                            DataNavigateUrlFormatString="typechineseedit.aspx?nid={0}" Text="编辑">
                        <ControlStyle Width="30px" />
                        <ItemStyle Width="40px" />
                        </asp:HyperLinkField>
                        <asp:HyperLinkField DataNavigateUrlFields="Nid" 
                            DataNavigateUrlFormatString="typechinesedel.aspx?nid={0}" Text="删除" >
                        <ItemStyle Width="40px" />
                        </asp:HyperLinkField>
                    </Columns>
                    <pagertemplate>
                        <div  class="pagediv">
                            第<asp:Label ID="lblPageIndex" runat="server" 
                                text="<%# ((GridView)Container.Parent.Parent).PageIndex + 1  %>" />
                            页  共<asp:Label ID="lblPageCount" runat="server" 
                                text="<%# ((GridView)Container.Parent.Parent).PageCount  %>" />
                            页 
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
                </asp:GridView>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
