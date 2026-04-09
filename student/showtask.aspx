<%@ Page Title="" Language="C#" MasterPageFile="~/student/Scm.master" AutoEventWireup="true"  StylesheetTheme="Student"  CodeFile="showtask.aspx.cs" Inherits="Student_showtask" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cpcm" Runat="Server">

<div class="prog-wrap">
<div id="showcontent" class="prog-grid">

    <!-- Main Task Content -->
    <div class="prog-card" style="background:linear-gradient(160deg,#fff 0%,#f0f7ff 100%);">
        <div class="prog-card__body">
            <div class="course-node-head" style="text-align:center;padding:20px 20px 24px;border-bottom:1px solid #e2e8f0;margin:-14px -20px 16px; border-radius:0.75rem 0.75rem 0 0;">
                <asp:Label ID="LabelMtitle" runat="server" CssClass="course-node-title" style="font-size:clamp(18px,2.5vw,24px);font-weight:800;color:#0f172a;letter-spacing:-0.02em;"></asp:Label>
            </div>
            <div class="hidden" style="display:none;">
                <asp:Label ID="LabelSnum"  runat="server" Visible="False"></asp:Label>
                <asp:CheckBox ID="CkMupload" runat="server" Enabled="false" Visible="False" />
                <asp:CheckBox ID="CkMgroup" runat="server" Enabled="false" Visible="False" />
                <asp:Label ID="LabelMid" runat="server" Visible="False"></asp:Label>            
                <asp:Label ID="LabelUploadType" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="LabelMcid" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="LabelMsort"  runat="server" Visible="False"></asp:Label>
                <asp:Label ID="LabelLid"  runat="server" Visible="False"></asp:Label>
            </div>
            <div id="Mcontent" style="color:#334155;line-height:1.85;font-size:1.05rem;word-wrap:break-word;word-break:break-word;" runat="server"></div>
        </div>
    </div>

    <!-- Right Action Sidebar -->
    <div class="prog-sidebar">
        <div class="prog-card prog-sidebar-card">
            <div class="prog-card__head">
                <div class="prog-sidebar-icon">
                    <svg fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6V4m0 2a2 2 0 100 4m0-4a2 2 0 110 4m-6 8a2 2 0 100-4m0 4a2 2 0 110-4m0 4v2m0-6V4m6 6v10m6-2a2 2 0 100-4m0 4a2 2 0 110-4m0 4v2m0-6V4"></path></svg>
                </div>
                <h3 class="prog-card__title">操作面板</h3>
            </div>
            <div class="prog-card__body">
                <script src="../Plupload/plupload.full.min.js" type="text/javascript"></script>

                <div class="prog-btn-stack">
                    <input type="button" class="prog-btn-secondary" id="share" value="我的网盘" onclick="showShare()" />
                    <asp:HyperLink ID="VoteLink" runat="server" Target="_blank"
                        CssClass="prog-btn-outline" SkinID="HyperLinkPink">作品互评</asp:HyperLink>
                </div>

                <asp:Panel ID="Panelworks" runat="server">
                    <hr class="prog-divider" />
                    <div class="prog-subpanel">
                        <div class="prog-subpanel-title">
                            <svg fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-8l-4-4m0 0L8 8m4-4v12"></path></svg>
                            作品提交区
                        </div>
                        <div style="display:flex;flex-direction:column;align-items:center;gap:8px;">
                            <asp:Image runat="server" ID="upFileType" Visible="False" style="width:28px;height:28px;object-fit:contain;" />
                            <asp:HyperLink ID="oldUrl" runat="server" Visible="False" Target="_blank"
                                style="width:100%;padding:6px 10px;background:#f1f5f9;color:#64748b;font-size:12px;font-weight:600;border-radius:0.375rem;border:1px solid #e2e8f0;text-align:center;display:block;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;"></asp:HyperLink>
                            <asp:HyperLink ID="upFileUrl" runat="server" Visible="False" Target="_blank"
                                style="width:100%;padding:6px 10px;background:#f1f5f9;color:#2563eb;font-size:12px;font-weight:600;border-radius:0.375rem;border:1px solid #e2e8f0;text-align:center;display:block;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;"></asp:HyperLink>

                            <asp:Panel ID="Panelswfupload" runat="server" style="width:100%;">
                                <div id="swfu_container" style="display:flex;justify-content:center;">
                                    <div id="container" style="text-align:center;width:100%;">
                                        <a id="pickfiles" href="javascript:;" class="upload-btn">提交作品</a>
                                        <div id="filelist" style="margin-top:6px;font-size:12px;color:#64748b;"></div>
                                    </div>
                                    
                                </div>
                                <div class="prog-filetype">
                                    <asp:Image ID="ImageType" runat="server" style="width:14px;height:14px;display:inline-block;vertical-align:middle;" />
                                    限制格式 <asp:Label ID="LabelMfiletype" runat="server" style="font-weight:700;color:#334155;"></asp:Label>
                                </div>
                                <div class="prog-errmsg"><asp:Label ID="Labelmsg" runat="server" SkinID="LabelMsgRed"></asp:Label></div>
                            </asp:Panel>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>
    </div>

</div>
</div>

    <script type="text/javascript">
        window.__showtaskConfig = {
            labelMid_Text: '<%= LabelMid.Text %>',
            labelLid_Text: '<%= LabelLid.Text %>',
            labelSnum_Text: '<%= LabelSnum.Text %>',
            labelUploadType_Text: '<%= LabelUploadType.Text %>'
        };
    </script>
    <script type="text/javascript" src="../js/showtask.js"></script>
</asp:Content>
