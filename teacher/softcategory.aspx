<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher"
    AutoEventWireup="true" CodeFile="softcategory.aspx.cs" Inherits="Teacher_softcategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" runat="Server">
    

    <div class="cate-page">
        <div class="cate-shell">
            <div class="cate-hero">
                <div class="cate-hero__content">
                    <div>
                        <h1 class="cate-hero__title">资源分类设置</h1>
                        <p class="cate-hero__subtitle">管理资源分类，支持添加、编辑、排序和删除操作。</p>
                    </div>
                    <asp:Button ID="Btnreturn" runat="server" Text="返回资源列表" OnClick="Btnreturn_Click"
                        CssClass="cate-hero__btn" />
                </div>
            </div>

            <div class="cate-card cate-card--list">
                <div class="cate-card__head">
                    <h2 class="cate-card__title">分类列表</h2>
                </div>
                <div class="cate-card__body">
                    <div class="cate-table-wrap">
                        <asp:GridView ID="GVCategory" runat="server" AutoGenerateColumns="False"
                            DataKeyNames="Yid" Width="100%" CellPadding="0" GridLines="None"
                            OnRowCommand="GVCategory_RowCommand" EnableModelValidation="True"
                            OnRowDataBound="GVCategory_RowDataBound" OnRowCancelingEdit="GVCategory_RowCancelingEdit"
                            OnRowEditing="GVCategory_RowEditing" OnRowUpdating="GVCategory_RowUpdating">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="LabelYid" runat="server" Text='<%# Bind("Yid") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="cate-cell--id" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="资源类别">
                                    <ItemTemplate>
                                        <asp:Label ID="LabelYtitle" runat="server" Text='<%# Bind("Ytitle") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TBoxYtitle" runat="server" Text='<%# Bind("Ytitle") %>'
                                            CssClass="cate-edit-input" MaxLength="30"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle CssClass="cate-cell--title" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="ImageBtnTop" runat="server" CausesValidation="False" CommandName="Top"
                                            CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' Text="上" ToolTip="向上移"
                                            Font-Underline="False"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="cate-cell--move" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="ImageBtnBottom" runat="server" CausesValidation="False" CommandName="Bottom"
                                            CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' Text="下" ToolTip="向下移"
                                            Font-Underline="False"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="cate-cell--move" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:Button ID="ImageButton1" runat="server" CausesValidation="False"
                                            CommandName="Edit" Text="编辑" CssClass="cate-inline-btn" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Button ID="ImageButton1" runat="server" CausesValidation="True"
                                            CommandName="Update" Text="更新" CssClass="cate-inline-btn" />
                                        <asp:Button ID="ImageButton2" runat="server" CausesValidation="False"
                                            CommandName="Cancel" Text="取消" CssClass="cate-inline-btn" />
                                    </EditItemTemplate>
                                    <ItemStyle CssClass="cate-cell--edit" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="BtnDel" runat="server" CausesValidation="false" CommandName="Del"
                                            CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' Text="删除"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="cate-cell--del" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="" />
                            <RowStyle CssClass="" />
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <div class="cate-card cate-card--add">
                <div class="cate-card__head">
                    <h2 class="cate-card__title">添加分类</h2>
                </div>
                <div class="cate-card__body">
                    <div class="cate-add-form">
                        <div class="cate-field">
                            <span class="cate-label">类别名称</span>
                            <asp:TextBox ID="TextBoxNewYtitle" runat="server" MaxLength="30"
                                CssClass="cate-input"></asp:TextBox>
                        </div>
                        <asp:Button ID="Btnadd" runat="server" Text="添加分类" OnClick="Btnadd_Click"
                            CssClass="cate-btn cate-btn--primary" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
