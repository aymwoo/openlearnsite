<%@ Page Title="" Language="C#" MasterPageFile="~/student/Scm.master" StylesheetTheme="Student"
    AutoEventWireup="true" CodeFile="txtform.aspx.cs" Inherits="Student_txtform" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cpcm" Runat="Server">

<div class="prog-wrap">
<div class="prog-grid">
    <!-- Main Content -->
    <div class="prog-card" style="background:linear-gradient(160deg,#fff 0%,#f0f7ff 100%);">
        <div class="prog-card__body">
        <div class="course-node-head text-center border-b border-slate-100 flex items-center justify-center gap-3" style="padding:20px 20px 24px;margin:-14px -20px 16px; border-radius:0.75rem 0.75rem 0 0;">
            <asp:Label ID="LabelMtitle" runat="server" CssClass="course-node-title" style="font-size:clamp(18px,2.5vw,24px);font-weight:800;color:#0f172a;letter-spacing:-0.02em;"></asp:Label>
            <img id="connected" alt="" src="../images/topictitle.png" style="display:none;" title="小组协作填表已开启" class="w-6 h-6" />
        </div>
        <div id="Mcontent" class="coursecontent" runat="server" style="color:#334155;line-height:1.9;font-size:16px;word-wrap:break-word;word-break:break-word;"></div>
        <div id="Mtable" class="mt-4" runat="server"></div>
        </div>
    </div>

    <!-- Right Sidebar -->
    <div class="prog-sidebar">
        <div class="prog-card prog-sidebar-card">
            <div class="prog-card__head">
                <div class="prog-sidebar-icon">
                    <svg fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 6V4m0 2a2 2 0 100 4m0-4a2 2 0 110 4m-6 8a2 2 0 100-4m0 4a2 2 0 110-4m0 4v2m0-6V4m6 6v10m6-2a2 2 0 100-4m0 4a2 2 0 110-4m0 4v2m0-6V4"></path></svg>
                </div>
                <h3 class="prog-card__title">操作面板</h3>
            </div>
            <div class="prog-card__body">
                <img id="sucessed" alt="" src="../images/sucessed.png" style="display:none;width:80px;height:auto;margin:0 auto 10px;display:none;" />
                <div class="prog-btn-stack">
                    <input id="Btnform" type="button" value="提交填写" onclick="SaveForm();" class="prog-btn" />
                    <asp:HyperLink ID="Hlresult" runat="server" SkinID="HyperLink" Target="_blank"
                        CssClass="prog-btn-outline">查看结果</asp:HyperLink>
                </div>
                <div id="msg" class="prog-info-label" style="color:#b91c1c;font-weight:700;min-height:1.4rem;margin-top:6px;"></div>
                <span class="namebox" style="position:absolute;display:none;color:#fff;background:#4f46e5;opacity:0.9;padding:4px 8px;border-radius:6px;font-size:12px;white-space:nowrap;">TextName</span>
                
            </div>
        </div>
    </div>
</div>
</div>
<asp:HiddenField ID="hiddencount" runat="server" />
    <script type="text/javascript">
        window.__txtformConfig = {
            snum: '<%=Snum %>',
            sname: '<%=Sname %>',
            sgroup: '<%=Sgroup %>',
            collabo: '<%=Collabo %>',
            serverIp: '<%=serverIp %>',
            lid: '<%=Lid %>',
            done: '<%=Done %>'
        };
    </script>
    <script type="text/javascript" src="../js/txtform.js"></script>
</asp:Content>
