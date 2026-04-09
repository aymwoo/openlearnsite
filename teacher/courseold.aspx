<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master"  StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="courseold.aspx.cs" Inherits="Teacher_courseold" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    

    <div class="course-old-page">
        <div class="course-old-shell">
            <section class="course-old-hero">
                <div class="course-old-hero-content">
                    <div>
                        <span class="course-old-eyebrow">Archive Workspace</span>
                        <h1 class="course-old-title">学案仓库</h1>
                        <p class="course-old-subtitle">集中查看已转入仓库的学案，保留原有启用、删除与筛选流程，仅优化布局层次、视觉表现与移动端浏览体验。</p>
                    </div>
                    <div class="course-old-hero-action">
                        <asp:Button ID="Btnreturn" runat="server" Text="返回学案列表" onclick="Btnreturn_Click" CssClass="course-old-secondary-btn" />
                    </div>
                </div>
            </section>

            <section class="course-old-toolbar">
                <div class="course-old-toolbar-grid">
                    <div class="course-old-field">
                        <span class="course-old-field-label">选择年级</span>
                        <asp:DropDownList ID="DDLgrade" runat="server" Width="60px" EnableTheming="True" AutoPostBack="True" onselectedindexchanged="DDLgrade_SelectedIndexChanged" CssClass="course-old-select"></asp:DropDownList>
                    </div>
                    <div class="course-old-field">
                        <span class="course-old-field-label">选择学期</span>
                        <asp:DropDownList ID="DDLterm" runat="server" EnableTheming="True" AutoPostBack="True" onselectedindexchanged="DDLterm_SelectedIndexChanged" ToolTip="选择要显示学案的学期，不改变后台默认学期设置" CssClass="course-old-select">
                            <asp:ListItem Value="1">第一学期</asp:ListItem>
                            <asp:ListItem Value="2">第二学期</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="course-old-field">
                        <span class="course-old-field-label">当前设置</span>
                        <asp:Label ID="Labelmsg" runat="server" Font-Bold="False" CssClass="course-old-note"></asp:Label>
                    </div>
                    <div class="course-old-field">
                        <span class="course-old-field-label">提示区域</span>
                        <asp:Label ID="Labelspace" runat="server" CssClass="course-old-spacer"></asp:Label>
                    </div>
                </div>
            </section>

            <section class="course-old-table-panel">
                <div class="course-old-table-header">
                    <div>
                        <h2 class="course-old-table-title">仓库学案列表</h2>
                        <p class="course-old-table-desc">支持浏览、重新启用和删除。所有原有数据绑定、命令按钮和页面事件保持不变。</p>
                    </div>
                    <span class="course-old-table-chip">仓库内容仅支持浏览</span>
                </div>
                <div class="course-old-table-wrap custom-scrollbar">
                    <asp:GridView ID="GVCourse" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" DataKeyNames="Cid"
                        PageSize="20" Width="100%" CssClass="course-old-grid"
                        onpageindexchanging="GVCourse_PageIndexChanging"
                        onrowdatabound="GVCourse_RowDataBound" CellPadding="6"
                        EnableModelValidation="True"
                        onrowcommand="GVCourse_RowCommand" ForeColor="#111111" GridLines="None">
                        <AlternatingRowStyle BackColor="#FBFDFF" />
                        <Columns>
                            <asp:BoundField DataField="Cobj" HeaderText="年级">
                                <ControlStyle Width="20px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Cterm" HeaderText="学期" />
                            <asp:BoundField DataField="Cks" HeaderText="课节">
                                <ControlStyle Width="20px" />
                            </asp:BoundField>
                            <asp:HyperLinkField DataNavigateUrlFields="Cid"
                                DataNavigateUrlFormatString="~/teacher/courseshow.aspx?cid={0}&amp;cold=T"
                                DataTextField="Ctitle" HeaderText="仓库学案">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:HyperLinkField>
                            <asp:BoundField DataField="Cclass" HeaderText="类型" SortExpression="Cclass" />
                            <asp:TemplateField HeaderText="操作" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false"
                                        CommandArgument='<%# Bind("Cid") %>' CommandName="U" ToolTip="将此学案重新启用，在学案列表中显示出来" Text="启用"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="日期" SortExpression="Cdate">
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem,"Cdate","{0:d}")%>'></asp:Label>
                                </ItemTemplate>
                                <ControlStyle Width="70px" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:TemplateField>
                            <asp:HyperLinkField DataNavigateUrlFields="Cid,Cobj"
                                DataNavigateUrlFormatString="~/teacher/coursedel.aspx?cid={0}&grade={1}" Text="删除">
                                <ItemStyle Width="60px" />
                            </asp:HyperLinkField>
                        </Columns>
                        <FooterStyle BackColor="#F8FAFC" Font-Bold="True" ForeColor="#0F172A" />
                        <HeaderStyle BackColor="#F8FAFC" Font-Bold="True" ForeColor="#475569" />
                        <PagerStyle CssClass="course-old-pager-row" BackColor="#F8FAFC" ForeColor="#111111" HorizontalAlign="Center" />
                        <pagertemplate>
                            <div class="course-old-pager">
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
                        <SelectedRowStyle BackColor="#CFFAFE" Font-Bold="True" ForeColor="#155E75" />
                    </asp:GridView>
                </div>
            </section>

            <section class="course-old-note-box">
                <span class="course-old-warning">仓库中的学案只能浏览，不能编辑</span>
            </section>
        </div>
    </div>
</asp:Content>
