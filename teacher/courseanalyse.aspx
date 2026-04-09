<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" AutoEventWireup="true" CodeFile="courseanalyse.aspx.cs" Inherits="Teacher_courseanalyse" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
<style type="text/css">
    /* ===== Hero Header ===== */
    .ca-hero {
        background: linear-gradient(135deg, #1e3a5f 0%, #1d4ed8 100%);
        border-radius: 14px;
        padding: 1.5rem 1.75rem;
        margin-bottom: 1.25rem;
        display: flex;
        align-items: flex-start;
        justify-content: space-between;
        gap: 1rem;
        flex-wrap: wrap;
    }
    .ca-hero-left { flex: 1; min-width: 0; }
    .ca-hero-label {
        font-size: 0.72rem;
        font-weight: 700;
        text-transform: uppercase;
        letter-spacing: 0.1em;
        color: #93c5fd;
        margin-bottom: 0.35rem;
    }
    .ca-hero-title {
        font-size: 1.3rem;
        font-weight: 800;
        color: #ffffff;
        word-break: break-word;
        line-height: 1.3;
    }
    .ca-hero-back {
        flex-shrink: 0;
        display: flex;
        align-items: center;
        cursor: pointer;
    }
    .ca-hero-back .ca-back-btn {
        display: inline-flex;
        align-items: center;
        justify-content: center;
        min-height: 2.6rem;
        padding: 0 1rem;
        border-radius: 9999px;
        border: 1px solid rgba(191, 219, 254, 0.55);
        background: rgba(239, 246, 255, 0.14);
        color: #ffffff;
        font-weight: 700;
        transition: all 0.2s ease;
    }
    .ca-hero-back .ca-back-btn:hover {
        background: rgba(255, 255, 255, 0.2);
    }

    /* ===== Stats Bar ===== */
    .ca-stats-bar {
        background: #ffffff;
        border: 1px solid #e2e8f0;
        border-radius: 12px;
        padding: 0.9rem 1.25rem;
        margin-bottom: 1.25rem;
        box-shadow: 0 1px 8px rgba(0,0,0,0.04);
        font-size: 0.88rem;
        color: #475569;
        font-weight: 500;
    }
    .ca-stats-bar strong { color: #1e293b; }

    /* ===== Viewer Card ===== */
    .ca-viewer-card {
        background: #ffffff;
        border: 1px solid #e2e8f0;
        border-radius: 14px;
        box-shadow: 0 2px 12px rgba(0,0,0,0.05);
        overflow: hidden;
        margin-bottom: 1.25rem;
    }
    .ca-viewer-toolbar {
        background: #f8fafc;
        border-bottom: 1px solid #e2e8f0;
        padding: 0.65rem 1.25rem;
        display: flex;
        align-items: center;
        gap: 0.75rem;
        flex-wrap: wrap;
    }
    .ca-viewer-label {
        font-size: 0.75rem;
        font-weight: 700;
        text-transform: uppercase;
        letter-spacing: 0.07em;
        color: #64748b;
    }
    .ca-viewer-counter {
        font-size: 0.8rem;
        color: #94a3b8;
        background: #f1f5f9;
        border: 1px solid #e2e8f0;
        border-radius: 6px;
        padding: 2px 9px;
        font-weight: 600;
    }
    .ca-nav-btn {
        display: inline-flex;
        align-items: center;
        justify-content: center;
        min-height: 30px;
        padding: 0 12px;
        border-radius: 8px;
        background: #eff6ff;
        border: 1px solid #bfdbfe;
        cursor: pointer;
        transition: background 0.15s;
        color: #1d4ed8;
        font-size: 12px;
        font-weight: 700;
    }
    .ca-nav-btn:hover { background: #dbeafe; }
    .ca-viewer-body {
        padding: 1.25rem;
        min-height: 200px;
    }

    /* ===== Empty state ===== */
    .ca-empty {
        text-align: center;
        padding: 3rem 1rem;
        color: #94a3b8;
        font-size: 0.95rem;
    }
</style>

<!-- Hero -->
<div class="ca-hero">
    <div class="ca-hero-left">
        <div class="ca-hero-label">学案分析</div>
        <div class="ca-hero-title">
            <asp:Label ID="Labeltitle" runat="server"></asp:Label>
        </div>
    </div>
    <div class="ca-hero-back">
        <asp:Button ID="ImageButton1" runat="server"
            Text="返回学案"
            OnClick="ImageButton1_Click"
            ToolTip="返回学案列表"
            CssClass="ca-back-btn" />
    </div>
</div>

<!-- Stats Bar -->
<div class="ca-stats-bar">
    <asp:Label ID="Labeldistribution" runat="server"></asp:Label>
</div>

<!-- Starred Works Viewer -->
<div id="divview" runat="server" visible="false">
    <div class="ca-viewer-card">
        <div class="ca-viewer-toolbar">
            <span class="ca-viewer-label">&#11088; 收藏作品 (G级)</span>
            <div class="ca-nav-btn">
                <asp:Button ID="ImgBtnLeft" runat="server"
                    Text="上一项" OnClick="ImgBtnLeft_Click" />
            </div>
            <asp:DropDownList ID="DDLstore" runat="server"
                Font-Bold="True" Width="180px" AutoPostBack="True"
                Font-Size="10pt"
                onselectedindexchanged="DDLstore_SelectedIndexChanged">
                <asp:ListItem></asp:ListItem>
            </asp:DropDownList>
            <div class="ca-nav-btn">
                <asp:Button ID="ImgBtnright" runat="server"
                    Text="下一项" OnClick="ImgBtnright_Click" />
            </div>
            <span class="ca-viewer-counter">
                <asp:Label ID="lbcount" runat="server"></asp:Label>
            </span>
        </div>
        <div class="ca-viewer-body">
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </div>
    </div>
</div>
</asp:Content>
