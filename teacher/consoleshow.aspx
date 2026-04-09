<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" AutoEventWireup="true" CodeFile="consoleshow.aspx.cs" Inherits="Teacher_consoleshow" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <script src="../markdown/lib/marked.min.js"></script>
    <link rel="stylesheet" href="../js/vendors/reveal/dist/reveal.css" />
    <link rel="stylesheet" href="../js/vendors/reveal/dist/theme/white.css" />
    <link rel="stylesheet" href="../js/vendors/highlight/github.min.css" />
    <script src="../webform/highlight.min.js"></script>
    <link href="../App_Themes/Teacher/content-show-markdown.css" rel="stylesheet" />
    <link href="../App_Themes/Teacher/admin-form.css" rel="stylesheet" />
    <style type="text/css">
        .console-show-page {
            --admin-form-page-bg: linear-gradient(180deg, #f8fafc 0%, #eef2ff 100%);
            --admin-form-hero-bg: linear-gradient(135deg, #1e293b 0%, #334155 55%, #475569 100%);
            --admin-form-hero-shadow: 0 22px 45px -28px rgba(51, 65, 85, 0.72);
            --admin-form-primary-bg: #334155;
            --admin-form-primary-hover: #1e293b;
            --admin-form-primary-shadow: 0 14px 24px -18px rgba(51, 65, 85, 0.85);
            --admin-form-secondary-border: #cbd5e1;
            --admin-form-secondary-bg: #f8fafc;
            --admin-form-secondary-hover: #f1f5f9;
            --admin-form-secondary-fg: #334155;
        }

        .console-show-content {
            line-height: 1.8;
            word-break: break-word;
        }

        .console-problem-grid {
            overflow-x: auto;
        }
    </style>

    <div class="admin-form-page console-show-page">
        <div class="admin-form-shell">
            <section class="admin-form-hero">
                <div class="admin-form-hero-content">
                    <div class="admin-form-eyebrow">Console Test</div>
                    <h1 class="admin-form-title"><asp:Label runat="server" ID="Lbtitle"></asp:Label></h1>
                    <p class="admin-form-subtitle">预览当前测评说明、启停状态和试题列表效果。</p>
                </div>
            </section>

            <section class="admin-form-panel">
                <div class="admin-form-toolbar">
                    <div>
                        <h2 class="admin-form-section-title">测评信息</h2>
                        <p class="admin-form-section-desc">这里显示当前测评日期、说明与启停状态。</p>
                    </div>
                    <div class="admin-form-action-row">
                        <asp:Button ID="BtnEdit" runat="server" Text="编辑内容" ToolTip="点击修改"
                            OnClick="BtnEdit_Click" CssClass="admin-form-btn admin-form-btn--primary" />
                        <asp:Button ID="Btnclock" runat="server" Text="测评状态"
                            OnClick="Btnclock_Click" ToolTip="测评启用或停止" CssClass="admin-form-btn admin-form-btn--secondary" />
                    </div>
                </div>
                <div class="admin-form-kv">
                    <div class="admin-form-kv-item">
                        <span class="admin-form-kv-label">日期</span>
                        <span class="admin-form-kv-value"><asp:Label runat="server" ID="Lbdate"></asp:Label></span>
                    </div>
                </div>
            </section>

            <section class="admin-form-panel">
                <h2 class="admin-form-section-title">测评说明</h2>
                <p class="admin-form-section-desc">以下为当前交互测评的说明内容。</p>
                <div id="vcontent" runat="server" class="console-show-content"></div>
            </section>

            <section class="admin-form-actions">
                <div class="admin-form-action-row">
                    <asp:Button ID="Btnadd" runat="server" onclick="Btnadd_Click"
                        Text="添加试题" CssClass="admin-form-btn admin-form-btn--primary" />
                    <asp:Button ID="Btnreturn" runat="server" onclick="Btnreturn_Click"
                        Text="返回学案" CssClass="admin-form-btn admin-form-btn--secondary" />
                    <asp:HyperLink ID="Hkconsole" runat="server" NavigateUrl="#"
                        Target="_blank" CssClass="admin-form-link">预览效果</asp:HyperLink>
                </div>
            </section>

            <section class="admin-form-panel">
                <h2 class="admin-form-section-title">试题列表</h2>
                <p class="admin-form-section-desc">支持试题增删改以及上下调整顺序。</p>
                <div class="console-problem-grid">
                    <asp:GridView ID="GVProblem" runat="server" SkinID="GridViewInfo"
                            AutoGenerateColumns="False" Width="980px" CellPadding="5" Font-Size="11pt"
                        EnableModelValidation="True" HorizontalAlign="Center"
                        onrowcommand="GVProblem_RowCommand"
                        onrowdatabound="GVProblem_RowDataBound" DataKeyNames="Pid" >
                            <Columns>
                                <asp:BoundField HeaderText="序号">
                                <HeaderStyle Width="40px" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="试题">
                                    <ItemTemplate>
                                        <asp:Label ID="LabelPtitle" runat="server" Text='<%# HttpUtility.HtmlDecode(DataBinder.Eval(Container.DataItem,"Ptitle").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="600px" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="ImageBtnTop" runat="server" CausesValidation="False"
                                            CommandName="Top" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'
                                            Text="上" ToolTip="向上移" Font-Underline="False"></asp:LinkButton>
                                    </ItemTemplate>
                                     <ItemStyle Width="20px" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="ImageBtnBottom" runat="server" CausesValidation="False"
                                            CommandName="Bottom" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'
                                            Text="下" ToolTip="向下移" Font-Underline="False"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="20px" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="Pscore" HeaderText="分值">
                                <HeaderStyle Width="40px" />
                                </asp:BoundField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HyperLink ID="HyperLinkPid" runat="server" Text="编辑" CssClass="admin-form-link"></asp:HyperLink>
                                    </ItemTemplate>
                                    <HeaderStyle Width="40px" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="BtnDel" runat="server" CausesValidation="false"
                                          CommandArgument='<%# ((GridViewRow) Container).RowIndex %>' CommandName="Del" Text="删除"></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="40px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                </div>
            </section>
        </div>
    </div>
    <script type="text/javascript">
        window.__contentShowMarkdown = {
            contentId: '<%= vcontent.ClientID %>'
        };
    </script>
    <script type="text/javascript" src="../js/content-show-markdown.js"></script>
</asp:Content>
