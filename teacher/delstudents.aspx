<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master"  StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="delstudents.aspx.cs" Inherits="Teacher_delstudents" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/admin-form.css" rel="stylesheet" />
    <style type="text/css">
        .recover-page {
            --admin-form-page-bg: linear-gradient(180deg, #f8fafc 0%, #eef6ff 100%);
            --admin-form-hero-bg: linear-gradient(135deg, #0f766e 0%, #0f9b8e 55%, #22c55e 100%);
            --admin-form-hero-shadow: 0 22px 45px -28px rgba(15, 118, 110, 0.72);
            --admin-form-primary-bg: #0f766e;
            --admin-form-primary-hover: #0d675f;
            --admin-form-primary-shadow: 0 14px 24px -18px rgba(15, 118, 110, 0.85);
            --admin-form-secondary-border: #99f6e4;
            --admin-form-secondary-bg: #ecfeff;
            --admin-form-secondary-hover: #cffafe;
            --admin-form-secondary-fg: #115e59;
        }

        .recover-grid {
            overflow-x: auto;
        }
    </style>
    <div class="admin-form-page recover-page">
        <div class="admin-form-shell">
            <section class="admin-form-hero">
                <div class="admin-form-hero-content">
                    <div class="admin-form-eyebrow">Deleted Students</div>
                    <h1 class="admin-form-title"><asp:Label ID="Labelgradeclass" runat="server"></asp:Label> 已删除学生</h1>
                    <p class="admin-form-subtitle">可恢复已删除学生到学生表中，恢复后的个人密码默认为 12345。</p>
                </div>
            </section>
            <section class="admin-form-panel">
                <h2 class="admin-form-section-title">学生列表</h2>
                <p class="admin-form-section-desc">支持恢复单个学生，或永久删除记录。</p>
                <div class="recover-grid">
                    <asp:GridView ID="GVStudent" runat="server" AutoGenerateColumns="False"
                        Width="100%" CellPadding="3" PageSize="24" SkinID="GridViewInfo"
                        OnRowDataBound="GVStudent_RowDataBound" EnableModelValidation="True"
                        DataKeyNames="Did" onrowcommand="GVStudent_RowCommand">
                        <Columns>
                            <asp:BoundField HeaderText="序号" />
                            <asp:BoundField DataField="Dnum" HeaderText="学号" />
                            <asp:BoundField DataField="Dname" HeaderText="姓名" />
                            <asp:BoundField DataField="Dyear" HeaderText="入学年度" />
                            <asp:BoundField DataField="Dgrade" HeaderText="年级" />
                            <asp:BoundField DataField="Dclass" HeaderText="班级" />
                            <asp:BoundField DataField="Dsex" HeaderText="性别" />
                            <asp:BoundField DataField="Dheadtheacher" HeaderText="班主任" />
                            <asp:BoundField DataField="Dparents" HeaderText="父母" />
                            <asp:BoundField DataField="Dphone" HeaderText="联系电话" />
                            <asp:TemplateField HeaderText="操作" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:LinkButton ID="BtnRevive" runat="server" CausesValidation="false"
                                    CommandArgument='<%# Eval("Did") %>' CommandName="Revive" Text="恢复"></asp:LinkButton>
                                    <asp:LinkButton ID="LinkBtnDel" runat="server" CausesValidation="false"
                                    CommandArgument='<%# Eval("Did") %>' CommandName="Del" Text="永久删除"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </section>
            <section class="admin-form-actions">
                <div class="admin-form-action-row">
                    <asp:LinkButton ID="LinkBtncancel" runat="server" OnClick="LinkBtncancel_Click" CssClass="admin-form-btn admin-form-btn--secondary">返回列表</asp:LinkButton>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
