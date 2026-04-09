<%@ Page Title="" Language="C#" MasterPageFile="~/student/Scm.master" AutoEventWireup="true" StylesheetTheme="Student"  CodeFile="program.aspx.cs" Inherits="Student_program" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cpcm" Runat="Server">


<div class="prog-wrap">
    <link href="../kindeditor/themes/me/me.css" rel="stylesheet" type="text/css" />
    <script charset="utf-8" src="../kindeditor/kindeditor-min.js" type="text/javascript"></script>
    <script charset="utf-8" src="../kindeditor/lang/zh_CN.js" type="text/javascript"></script>

    <div class="prog-grid">

        <!-- ══ Main Content ══ -->
        <div class="prog-card prog-main-card">
            <div class="prog-card__body">
                <!-- Hidden state labels -->
                <div class="hidden" style="display:none;">
                    <asp:Label ID="LabelSnum" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="LabelMid" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="LabelUploadType" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="LabelMcid" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="LabelMsort" runat="server" Visible="False"></asp:Label>
                    <asp:CheckBox ID="CheckBack" runat="server" Visible="False" />
                    <asp:CheckBox ID="CheckBlock" runat="server" Visible="False" />
                    <asp:CheckBox ID="CheckBlockpy" runat="server" Visible="False" />
                    <asp:Label ID="LabelLid" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="LabelLtype" runat="server" Visible="False"></asp:Label>
                </div>

                <div class="prog-main-title course-node-head">
                    <asp:Label ID="LabelMtitle" runat="server" CssClass="course-node-title"></asp:Label>
                </div>

                <div id="Mcontent" class="prog-content-area" style="word-wrap:break-word; word-break:break-word;" runat="server">
                </div>
            </div>
        </div>

        <!-- ══ Right Sidebar ══ -->
        <div class="prog-sidebar">
            <div class="prog-card prog-sidebar-card">
                <div class="prog-card__head">
                    <div class="prog-sidebar-icon">
                        <svg fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
                                d="M12 6V4m0 2a2 2 0 100 4m0-4a2 2 0 110 4m-6 8a2 2 0 100-4m0 4a2 2 0 110-4m0 4v2m0-6V4m6 6v10m6-2a2 2 0 100-4m0 4a2 2 0 110-4m0 4v2m0-6V4"></path>
                        </svg>
                    </div>
                    <h3 class="prog-card__title" style="flex:1; padding-left:10px;">操作面板</h3>
                </div>

                <div class="prog-card__body">
                    <!-- Quick actions -->
                    <div class="prog-btn-stack">
                        <input type="button"
                            class="prog-btn-secondary"
                            id="share" value="我的网盘"
                            onclick="showShare()" />
                        <asp:HyperLink ID="VoteLink" runat="server" Target="_blank"
                            CssClass="prog-btn-outline"
                            SkinID="HyperLinkPink">作品互评</asp:HyperLink>
                    </div>

                    <hr class="prog-divider" />

                    <!-- Work preview -->
                    <div class="prog-preview">
                        <asp:Image ID="Thumbnail" runat="server"
                            CssClass="prog-preview__img" />
                        <div id="pixelsmall" runat="server"></div>
                        <asp:Label ID="Wtitle" runat="server"
                            CssClass="prog-preview__label"></asp:Label>
                    </div>

                    <hr class="prog-divider" />

                    <!-- Action buttons -->
                    <div class="prog-btn-stack">
                        <asp:Button ID="BtnScratch" runat="server"
                            Font-Bold="True"
                            onclick="BtnScratch_Click"
                            SkinID="buttonSkinPink"
                            Text="开始创作"
                            CssClass="prog-btn" />

                        <asp:Label ID="Labelscratch" runat="server"
                            CssClass="prog-info-label"></asp:Label>

                        <asp:Button ID="BtnBegin" runat="server"
                            Font-Bold="True"
                            onclick="BtnBegin_Click"
                            SkinID="buttonSkinPink"
                            Text="开关指令"
                            Visible="False"
                            CssClass="prog-btn-secondary" />

                        <asp:Button ID="ButtonClear" runat="server"
                            Font-Bold="True"
                            SkinID="buttonSkinPink"
                            Text="清除提交"
                            ToolTip="清除模拟学生提交的本项作品"
                            onclick="ButtonClear_Click"
                            Visible="False"
                            CssClass="prog-btn-danger" />

                        <asp:Label ID="Labelmsg" runat="server"
                            SkinID="LabelMsgRed"
                            CssClass="prog-info-label"
                            style="color:#b91c1c; font-weight:700;"></asp:Label>

                        <asp:Image ID="ImagePass" runat="server"
                            ImageUrl="~/images/sucessed.png"
                            Visible="False"
                            CssClass="prog-preview__pass" />
                    </div>
                </div>
            </div>
        </div>

    </div>
</div>


    <script type="text/javascript" src="../js/program.js"></script>
</asp:Content>
