<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master"   StylesheetTheme="Teacher" AutoEventWireup="true"  CodeFile="course.aspx.cs" Inherits="Teacher_course" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link rel="stylesheet" type="text/css" href="../webform/bootstrap-icons.min.css" />
    

    <div class="course-page">
        <div class="course-shell">
            <section class="course-hero">
                <div class="course-hero-content">
                    <div>
                        <span class="course-eyebrow">Teacher Workspace</span>
                        <h1 class="course-title">学案管理</h1>
                        <p class="course-subtitle">集中查看、筛选与维护当前年级学案，保留原有业务流程与数据操作，只优化页面布局、视觉层次和交互反馈。</p>
                    </div>
                    <div class="course-hero-action">
                        <asp:Button ID="Btnadd" runat="server" Text="添加学案" onclick="Btnadd_Click" CssClass="course-primary-btn" />
                    </div>
                </div>
            </section>

            <section class="course-toolbar">
                <div class="course-toolbar-grid">
                    <div class="course-field">
                        <span class="course-field-label">选择年级</span>
                        <asp:DropDownList ID="DDLgrade" runat="server" EnableTheming="True" AutoPostBack="True" onselectedindexchanged="DDLgrade_SelectedIndexChanged" CssClass="course-select"></asp:DropDownList>
                    </div>
                    <div class="course-field">
                        <span class="course-field-label">选择学期</span>
                        <asp:DropDownList ID="DDLterm" runat="server" EnableTheming="True" AutoPostBack="True" onselectedindexchanged="DDLterm_SelectedIndexChanged" ToolTip="选择要显示学案的学期，不改变后台默认学期设置" CssClass="course-select">
                            <asp:ListItem Value="1">第一学期</asp:ListItem>
                            <asp:ListItem Value="2">第二学期</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="course-field">
                        <span class="course-field-label">当前设置</span>
                        <asp:Label ID="Labelmsg" runat="server" CssClass="course-note"></asp:Label>
                    </div>
                    <div class="course-field">
                        <span class="course-field-label">提示区域</span>
                        <asp:Label ID="Labelspace" runat="server" CssClass="course-spacer"></asp:Label>
                    </div>
                </div>
            </section>

            <section class="course-table-panel">
                <div class="course-table-header">
                    <div>
                        <h2 class="course-table-title">学案列表</h2>
                        <p class="course-table-desc">支持发布、推荐、分析、编辑和转移操作，所有现有按钮事件与命令保持不变。</p>
                    </div>
                    <span class="course-table-chip">最多每页 20 条</span>
                </div>
                <div class="course-table-wrap custom-scrollbar">
                    <asp:GridView ID="GVCourse" runat="server" AllowPaging="True"
                        AutoGenerateColumns="False" DataKeyNames="Cid"
                        PageSize="20" Width="100%" CssClass="course-grid"
                        onpageindexchanging="GVCourse_PageIndexChanging"
                        onrowdatabound="GVCourse_RowDataBound" CellPadding="0"
                        EnableModelValidation="True"
                        onrowcommand="GVCourse_RowCommand" ForeColor="#111111" GridLines="None">
                        <AlternatingRowStyle BackColor="#FBFDFF" />
                        <Columns>
                            <asp:BoundField DataField="Cks" HeaderText="课节">
                                <ControlStyle Width="20px" />
                            </asp:BoundField>
                            <asp:HyperLinkField DataNavigateUrlFields="Cid"
                                DataNavigateUrlFormatString="~/teacher/courseshow.aspx?cid={0}"
                                DataTextField="Ctitle" HeaderText="学案">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:HyperLinkField>
                            <asp:BoundField DataField="Cclass" HeaderText="类型" SortExpression="Cclass">
                                <HeaderStyle HorizontalAlign="Left" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="打包">
                                <ItemTemplate>
                                    <asp:HyperLink ID="HlPackage" runat="server" NavigateUrl='<%# "~/teacher/package.aspx?cid=" + Eval("Cid") %>' Text='<i class="bi bi-download"></i>' ToolTip="打包下载" CssClass="course-icon-btn course-icon-primary"></asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="发布" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LbtnCpublish" runat="server" CausesValidation="false"
                                        CommandArgument='<%# Bind("Cid") %>' CommandName="Cp" Text='<%# Eval("Cpublish") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="作品">
                                <ItemTemplate>
                                    <asp:HyperLink ID="HlAnalyse" runat="server" NavigateUrl='<%# "~/teacher/courseanalyse.aspx?cid=" + Eval("Cid") %>' Text='<i class="bi bi-bar-chart-line"></i>' ToolTip="作品分析" CssClass="course-icon-btn course-icon-info"></asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="探讨">
                                <ItemTemplate>
                                    <asp:HyperLink ID="Hl" runat="server" Text='<i class="bi bi-chat-dots"></i>' ToolTip="探讨反思" CssClass="course-icon-btn course-icon-info"></asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="推荐" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LbtnCgood" runat="server" CausesValidation="false"
                                        CommandArgument='<%# Bind("Cid") %>' CommandName="Cg" ToolTip="默认为True，学生平台作品收藏学案列表中显示；False则不显示!" Text='<%# Eval("Cgood") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="内容">
                                <ItemTemplate>
                                    <asp:HyperLink ID="HlEdit" runat="server" NavigateUrl='<%# "~/teacher/courseedit.aspx?cid=" + Eval("Cid") %>' Text='<i class="bi bi-pencil-square"></i>' ToolTip="编辑内容" CssClass="course-icon-btn course-icon-warning"></asp:HyperLink>
                                </ItemTemplate>
                                <ItemStyle Width="40px" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="日期" SortExpression="Cdate">
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem,"Cdate","{0:d}")%>'></asp:Label>
                                </ItemTemplate>
                                <ControlStyle Width="70px" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="管理" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LbtnCold" runat="server" CausesValidation="false"
                                        CommandArgument='<%# Bind("Cid") %>' ToolTip="转移到学案仓库中保留" CommandName="Cu" Text='<i class="bi bi-archive"></i>' CssClass="course-icon-btn course-icon-danger"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle Width="40px" HorizontalAlign="Center" />
                            </asp:TemplateField>
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

            <section class="course-footer">
                <asp:Button ID="Btnimport" runat="server" Text="导入学案" onclick="Btnimport_Click" CssClass="course-secondary-btn" />
                <asp:Button ID="Btnold" runat="server" Text="学案仓库" onclick="Btnold_Click" CssClass="course-secondary-btn" />
            </section>
        </div>

        
    </div>
    <script type="text/javascript" src="../js/course.js"></script>
</asp:Content>
