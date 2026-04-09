<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master"   StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="gaugeitem.aspx.cs" Inherits="Teacher_gaugeitem" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link rel="stylesheet" type="text/css" href="/App_Themes/Teacher/gaugeitem.css" />

    <div class="gaugeitem-page">
        <div class="gaugeitem-shell">
            <section class="gaugeitem-hero">
                <div class="gaugeitem-hero__content">
                    <div class="gaugeitem-hero__intro">
                        <span class="gaugeitem-hero__eyebrow">Rubric Editor</span>
                        <h1 class="gaugeitem-hero__title">量规项管理</h1>
                        <p class="gaugeitem-hero__subtitle">维护当前量规的评价描述、分值和排序，也可以通过 AI 快速补充或重新生成量规项。</p>
                    </div>
                    <div class="gaugeitem-hero-panel">
                        <div class="gaugeitem-hero-panel__label">当前量规</div>
                        <div class="gaugeitem-hero-panel__title"><asp:Label ID="LabelGtitle" runat="server"></asp:Label></div>
                        <div class="gaugeitem-hero-panel__meta"><asp:Label ID="LabelProviderName" runat="server"></asp:Label></div>
                    </div>
                </div>
            </section>

            <section class="gaugeitem-card gaugeitem-card--primary">
                <div class="gaugeitem-card__head">
                    <div>
                        <h2 class="gaugeitem-card__title">AI 辅助与量规说明</h2>
                        <p class="gaugeitem-card__desc">可以保留现有量规并追加生成，也可以清空后重新用 AI 生成一整套评价项。</p>
                    </div>
                    <div class="gaugeitem-toolbar-actions">
                        <input id="BtnAppendAI" type="button" value="追加AI生成" class="gaugeitem-ai-btn gaugeitem-ai-btn--secondary" onclick="return startGaugeRegenerate('append');" />
                        <input id="BtnRegenerateAI" type="button" value="重新用AI生成一次" class="gaugeitem-ai-btn" onclick="return startGaugeRegenerate('replace');" />
                    </div>
                </div>
                <div class="gaugeitem-card__body">
                    <asp:Panel ID="PanelAIGenerated" runat="server" Visible="false" CssClass="gauge-ai-notice">
                        <p class="gauge-ai-notice__title"><asp:Label ID="LabelAIGeneratedTitle" runat="server"></asp:Label></p>
                        <asp:Label ID="LabelAIGeneratedMsg" runat="server" CssClass="gauge-ai-notice__msg"></asp:Label>
                        <asp:BulletedList ID="BulletedListAIItems" runat="server" CssClass="gauge-ai-notice__list"></asp:BulletedList>
                    </asp:Panel>
                </div>
            </section>

            <section class="gaugeitem-card">
                <div class="gaugeitem-card__head">
                    <div>
                        <h2 class="gaugeitem-card__title">量规项列表</h2>
                        <p class="gaugeitem-card__desc">支持在线编辑评价描述和分值，便于快速整理当前量规结构。</p>
                    </div>
                </div>
                <div class="gaugeitem-card__body">
                    <div class="gaugeitem-table-wrap">
                        <asp:GridView ID="GVGaugeItem" runat="server" SkinID="GridViewInfo"
                            AutoGenerateColumns="False" DataKeyNames="Mid" Width="100%" CellPadding="6"
                            Font-Size="9pt" onrowcommand="GVGaugeItem_RowCommand"
                            EnableModelValidation="True" onrowdatabound="GVGaugeItem_RowDataBound"
                            onrowcancelingedit="GVGaugeItem_RowCancelingEdit"
                            onrowediting="GVGaugeItem_RowEditing"
                            onrowupdating="GVGaugeItem_RowUpdating"
                            CssClass="gaugeitem-grid">
                            <Columns>
                                <asp:TemplateField HeaderText="序号">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Msort") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="48px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="评价描述">
                                    <ItemTemplate>
                                        <asp:Label ID="LabelMitem" runat="server" Text='<%# Bind("Mitem") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBoxMitem" runat="server" Text='<%# Bind("Mitem") %>' Font-Size="9pt" Width="200px" Height="12px" BackColor="#FFFFCC"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="240px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="分值">
                                    <ItemTemplate>
                                        <asp:Label ID="LabelMscore" runat="server" Text='<%# Bind("Mscore") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBoxMscore" runat="server" Text='<%# Bind("Mscore") %>' Font-Size="9pt" Width="20px" Height="12px" BackColor="#FFFFCC"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle Width="60px" />
                                </asp:TemplateField>
                                <asp:CommandField ShowEditButton="True">
                                    <ItemStyle Width="90px" />
                                </asp:CommandField>
                                <asp:TemplateField HeaderText="删除">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="BtnDel" runat="server" CausesValidation="false"
                                            CommandArgument='<%# Eval("Mid") %>' CommandName="Del" Text="删除" CssClass="gaugeitem-del-btn"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="72px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </section>

            <section class="gaugeitem-card">
                <div class="gaugeitem-card__head">
                    <div>
                        <h2 class="gaugeitem-card__title">添加量规项</h2>
                        <p class="gaugeitem-card__desc">补充新的评价描述，并设置分值和显示顺序。</p>
                    </div>
                </div>
                <div class="gaugeitem-card__body">
                    <div class="gaugeitem-form-grid">
                        <div class="gaugeitem-field gaugeitem-field--desc">
                            <label class="gaugeitem-label" for="<%= TextBoxMitem.ClientID %>">评价描述</label>
                            <asp:TextBox ID="TextBoxMitem" runat="server" SkinID="TextBoxNormal" Width="180px" CssClass="gaugeitem-input"></asp:TextBox>
                        </div>
                        <div class="gaugeitem-field gaugeitem-field--score">
                            <label class="gaugeitem-label" for="<%= DDLscore.ClientID %>">分值</label>
                            <asp:DropDownList ID="DDLscore" runat="server" Font-Size="9pt" CssClass="gaugeitem-select">
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem Selected="True">2</asp:ListItem>
                                <asp:ListItem>3</asp:ListItem>
                                <asp:ListItem>4</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>-1</asp:ListItem>
                                <asp:ListItem>-2</asp:ListItem>
                                <asp:ListItem>-3</asp:ListItem>
                                <asp:ListItem>-4</asp:ListItem>
                                <asp:ListItem>-5</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="gaugeitem-field gaugeitem-field--sort">
                            <label class="gaugeitem-label" for="<%= DDLsort.ClientID %>">顺序</label>
                            <asp:DropDownList ID="DDLsort" runat="server" Font-Size="9pt" CssClass="gaugeitem-select">
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem>2</asp:ListItem>
                                <asp:ListItem>3</asp:ListItem>
                                <asp:ListItem>4</asp:ListItem>
                                <asp:ListItem>5</asp:ListItem>
                                <asp:ListItem>6</asp:ListItem>
                                <asp:ListItem>7</asp:ListItem>
                                <asp:ListItem>8</asp:ListItem>
                                <asp:ListItem>9</asp:ListItem>
                                <asp:ListItem>10</asp:ListItem>
                                <asp:ListItem>11</asp:ListItem>
                                <asp:ListItem>12</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="gaugeitem-form-actions">
                        <asp:Button ID="Btnadd" runat="server" Text="添加量规项" onclick="Btnadd_Click" CssClass="gaugeitem-submit-btn" />
                        <asp:Button ID="Btnreturn" runat="server" Text="返回列表" onclick="Btnreturn_Click" CssClass="gaugeitem-return-btn" />
                    </div>
                </div>
            </section>
        </div>
    </div>
    <div id="gaugeItemLoading" class="gauge-ai-loading" aria-live="polite" aria-busy="true">
        <div class="gauge-ai-loading__card">
            <div class="gauge-ai-loading__spinner"></div>
            <p class="gauge-ai-loading__title">正在重新生成量规项</p>
            <p id="gaugeItemLoadingDesc" class="gauge-ai-loading__desc">系统正在读取当前量规并调用 AI 重新生成，请稍候。</p>
            <ul id="gaugeItemLoadingSteps" class="gauge-ai-loading__steps">
                <li class="gauge-ai-loading__step is-active"><span class="gauge-ai-loading__step-index">1</span><span>正在读取当前量规</span></li>
                <li class="gauge-ai-loading__step"><span class="gauge-ai-loading__step-index">2</span><span>正在调用 AI 生成评价项</span></li>
                <li class="gauge-ai-loading__step"><span class="gauge-ai-loading__step-index">3</span><span>正在覆盖旧量规项并写入新内容</span></li>
            </ul>
        </div>
    </div>
    <script type="text/javascript">
        window.__gaugeitemConfig = {
            gauge_generateUrl: '<%= ResolveUrl("~/teacher/gauge_generate.ashx") %>',
            request_QueryString_gid: '<%= Request.QueryString["gid"] %>'
        };
    </script>
    <script type="text/javascript" src="../js/gaugeitem.js"></script>
</asp:Content>
