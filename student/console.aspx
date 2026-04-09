<%@ Page Title="" Language="C#" MasterPageFile="~/student/Scm.master" AutoEventWireup="true" CodeFile="console.aspx.cs" Inherits="Student_console" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cpcm" Runat="Server">
<link href="../App_Themes/Student/console.css" rel="stylesheet" />


<asp:Label ID="LabelCid" runat="server" Visible="False"></asp:Label>
<asp:Label ID="LabelLid" runat="server" Visible="False"></asp:Label>
<asp:Label ID="LabelNid" runat="server" Visible="False"></asp:Label>

<div class="prog-wrap">
<div class="prog-grid">

    <!-- Main Content -->
    <div class="prog-card" style="background:linear-gradient(160deg,#fff 0%,#f0f7ff 100%);">
        <div class="prog-card__body">
            <div class="course-node-head" style="text-align:center;padding:20px 20px 24px;border-bottom:1px solid #e2e8f0;margin:-14px -20px 16px; border-radius:0.75rem 0.75rem 0 0;">
                <asp:Label ID="LabelMtitle" runat="server" CssClass="course-node-title" style="font-size:clamp(18px,2.5vw,24px);font-weight:800;color:#0f172a;letter-spacing:-0.02em;"></asp:Label>
            </div>
            <div id="Mcontent" style="color:#334155;line-height:1.85;font-size:1.05rem;word-wrap:break-word;word-break:break-word;" runat="server"></div>
        </div>
    </div>

    <!-- Right Sidebar -->
    <div class="prog-sidebar">
        <div class="prog-card prog-sidebar-card">
            <div class="prog-card__head">
                <div class="prog-sidebar-icon">
                    <svg fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-3 7h3m-3 4h3m-6-4h.01M9 16h.01"></path></svg>
                </div>
                <h3 class="prog-card__title">测评面板</h3>
            </div>
            <div class="prog-card__body">
                <link href="../kindeditor/themes/me/me.css" rel="stylesheet" type="text/css" />
                <script charset="utf-8" src="../kindeditor/kindeditor-min.js" type="text/javascript"></script>
                <script charset="utf-8" src="../kindeditor/lang/zh_CN.js" type="text/javascript"></script>

                <div class="prog-gv-wrap">
                    <asp:GridView ID="GVSolve" runat="server" EnableModelValidation="True"
                        AutoGenerateColumns="False" onrowdatabound="GVSolve_RowDataBound"
                        Width="100%" CssClass="">
                        <Columns>
                            <asp:BoundField HeaderText="题目">
                                <HeaderStyle />
                                <ItemStyle />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="得分">
                                <ItemTemplate>
                                    <asp:Label ID="Labelscore" runat="server" Text='<%# Bind("Vscore") %>' CssClass="score-label"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="Labelflag" runat="server"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>

                <div class="prog-btn-stack">
                    <asp:Button ID="BtnIdle" runat="server" Font-Bold="True"
                        SkinID="buttonSkinPink" Text="开始测评" onclick="BtnIdle_Click"
                        CssClass="prog-btn prog-btn-primary" />

                    <asp:ImageButton ID="Btnclock" runat="server" ImageUrl="~/images/clock.gif"
                        onclick="Btnclock_Click" style="width:32px;height:32px;opacity:0.7;transition:opacity 0.2s;" />

                    <asp:Image ID="Imagepass" runat="server" ImageUrl="~/images/pass.png" style="width:72px;height:auto;opacity:0.85;" />

                    <asp:HyperLink ID="Hlsolve" runat="server" Target="_blank"
                        CssClass="prog-btn-outline-green">班级测评报告</asp:HyperLink>
                </div>
            </div>
        </div>
    </div>

</div>
</div>
</asp:Content>
