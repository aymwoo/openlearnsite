<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manage.master" AutoEventWireup="true" CodeFile="teacher.aspx.cs" Inherits="Manager_teacher" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/teacher.css" rel="stylesheet" />
    <div class="mgr-page">
        <div class="mgr-shell">
            <div class="mgr-hero">
                <div class="mgr-hero__content">
                    <div>
                        <div class="mgr-hero__eyebrow">Teacher Management</div>
                        <h1 class="mgr-hero__title">教师管理</h1>
                        <p class="mgr-hero__subtitle">统一管理教师账号、权限说明和班级分配入口，适合在学期初集中维护教师信息。</p>
                        <div class="mgr-hero__meta">
                            <span class="mgr-chip">账号与昵称统一查看</span>
                            <span class="mgr-chip">支持班级分配跳转</span>
                            <span class="mgr-chip">可直接新增教师</span>
                        </div>
                    </div>
                    <asp:Button ID="Btnadd" runat="server" Text="＋ 添加教师" onclick="Btnadd_Click" CssClass="mgr-btn mgr-btn--primary" />
                </div>
            </div>

            <div class="mgr-overview">
                <div class="mgr-overview__card">
                    <span class="mgr-overview__label">管理目标</span>
                    <strong class="mgr-overview__value">教师账号集中维护</strong>
                    <span class="mgr-overview__desc">适合统一新增、修改或停用教师账号，减少零散配置。</span>
                </div>
                <div class="mgr-overview__card">
                    <span class="mgr-overview__label">班级管理</span>
                    <strong class="mgr-overview__value">按教师分配班级</strong>
                    <span class="mgr-overview__desc">普通教师可继续进入班级选择，管理员账号则不显示班级入口。</span>
                </div>
                <div class="mgr-overview__card">
                    <span class="mgr-overview__label">风险提示</span>
                    <strong class="mgr-overview__value">删除会清空班级绑定</strong>
                    <span class="mgr-overview__desc">删除教师时会同时清除该教师已选择的班级关联，请操作前确认。</span>
                </div>
            </div>

            <div class="mgr-card">
                <div class="mgr-card__head">
                    <h2 class="mgr-card__title">教师列表</h2>
                    <p class="mgr-card__desc">这里展示当前平台全部教师账号、权限状态和班级管理入口，便于统一维护。</p>
                </div>
                <div class="mgr-grid-wrap">
                    <asp:GridView ID="GVTeacher" runat="server" AutoGenerateColumns="False" Width="100%"
                        CssClass="mgr-grid" GridLines="None"
                        onpageindexchanging="GVTeacher_PageIndexChanging"
                        onrowdatabound="GVTeacher_RowDataBound" EnableModelValidation="True"
                        onrowcommand="GVTeacher_RowCommand" EmptyDataText="当前还没有教师账号，请先添加教师。">
                        <Columns>
                            <asp:BoundField HeaderText="序号"><ItemStyle Width="60px" CssClass="mgr-seq" /></asp:BoundField>
                            <asp:BoundField DataField="Hname" HeaderText="账号"><ItemStyle CssClass="mgr-cell--strong" /></asp:BoundField>
                            <asp:BoundField DataField="Hnick" HeaderText="昵称" />
                            <asp:BoundField DataField="Hpwd" HeaderText="密码"><ItemStyle CssClass="mgr-cell--mono" /></asp:BoundField>
                            <asp:TemplateField HeaderText="权限">
                                <ItemTemplate><asp:Label ID="LabelHpermiss" runat="server" Text='<%# Bind("Hpermiss") %>'></asp:Label></ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Hnote" HeaderText="备注"><ItemStyle CssClass="mgr-cell--muted" /></asp:BoundField>
                            <asp:BoundField DataField="Hcount" HeaderText="学案数"><ItemStyle CssClass="mgr-cell--count" /></asp:BoundField>
                            <asp:TemplateField HeaderText="班级">
                                <ItemTemplate>
                                    <asp:HyperLink ID="HyperLinkRoom" runat="server" NavigateUrl='<%# Eval("hid","roomselect.aspx?hid={0}") %>' Text="选择班级" CssClass="mgr-link"></asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:HyperLinkField DataNavigateUrlFields="hid" DataNavigateUrlFormatString="teacheredit.aspx?hid={0}" Text="修改" HeaderText="修改">
                                <ItemStyle CssClass="mgr-link" />
                            </asp:HyperLinkField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LinkButtonDel" runat="server" CausesValidation="false" CommandName="D" Text="删除" CommandArgument='<%# Bind("hid") %>' CssClass="mgr-del-btn" ToolTip="如果删除后想恢复，请手动在数据库Teacher表将该账号的删除标志重置为false！"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
