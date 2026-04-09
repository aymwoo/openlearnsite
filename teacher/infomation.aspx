<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="infomation.aspx.cs" Inherits="Teacher_infomation" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
<style type="text/css">
    .info-wrap {
        padding: 28px;
        background: linear-gradient(180deg,#f8fbff 0%,#f3f7ff 100%);
        color: #0f172a;
        box-sizing: border-box;
    }
    .info-wrap * { box-sizing: border-box; }

    .info-hero {
        border-radius: 1rem;
        padding: 24px 28px;
        background: linear-gradient(135deg,#eff6ff 0%,#dbeafe 100%);
        color: #1e3a8a;
        border: 1px solid #bfdbfe;
        box-shadow: 0 4px 16px rgba(37,99,235,.08);
        margin-bottom: 24px;
    }
    .info-hero__title { margin: 0; font-size: 24px; font-weight: 800; letter-spacing: -0.02em; color: #0f172a; }
    .info-hero__title span { color: #0f172a !important; }
    .info-hero__sub   { margin: 6px 0 0; font-size: 14px; color: #1e40af; }

    .info-card {
        border-radius: 1rem;
        border: 1px solid #dbe6f5;
        background: rgba(255,255,255,.96);
        box-shadow: 0 12px 30px rgba(15,23,42,.05);
        margin-bottom: 20px;
    }
    .info-card__head { padding: 18px 22px 0; }
    .info-card__title { margin: 0; font-size: 16px; font-weight: 800; color: #0f172a; }
    .info-card__body  { padding: 18px 22px 22px; }

    .info-class-grid {
        display: flex;
        flex-wrap: wrap;
        gap: 10px;
    }
    .info-class-item {
        display: flex;
        align-items: center;
        justify-content: center;
        min-width: 72px;
        min-height: 40px;
        padding: 0 14px;
        border-radius: 6px;
        border: 1px solid #bfdbfe;
        background: linear-gradient(180deg,#fff 0%,#eff6ff 100%);
        font-size: 14px;
        font-weight: 700;
        color: #1d4ed8;
        text-decoration: none;
        transition: transform .15s, box-shadow .15s;
    }
    .info-class-item:hover { transform: translateY(-2px); box-shadow: 0 6px 16px rgba(37,99,235,.15); }

    /* 循环色系 */
    .info-class-item:nth-child(6n+1) { background:#eff6ff; border-color:#bfdbfe; color:#1d4ed8; }
    .info-class-item:nth-child(6n+2) { background:#f0fdf4; border-color:#bbf7d0; color:#15803d; }
    .info-class-item:nth-child(6n+3) { background:#fefce8; border-color:#fde68a; color:#a16207; }
    .info-class-item:nth-child(6n+4) { background:#fff1f2; border-color:#fecaca; color:#b91c1c; }
    .info-class-item:nth-child(6n+5) { background:#f5f3ff; border-color:#ddd6fe; color:#6d28d9; }
    .info-class-item:nth-child(6n+6) { background:#ecfeff; border-color:#a5f3fc; color:#0e7490; }
    .info-cc-0 { background:#eff6ff; border-color:#bfdbfe; color:#1d4ed8; }
    .info-cc-1 { background:#f0fdf4; border-color:#bbf7d0; color:#15803d; }
    .info-cc-2 { background:#fefce8; border-color:#fde68a; color:#a16207; }
    .info-cc-3 { background:#fff1f2; border-color:#fecaca; color:#b91c1c; }
    .info-cc-4 { background:#f5f3ff; border-color:#ddd6fe; color:#6d28d9; }
    .info-cc-5 { background:#ecfeff; border-color:#a5f3fc; color:#0e7490; }

    .info-meta {
        display: flex;
        flex-wrap: wrap;
        align-items: center;
        gap: 16px;
        padding: 16px 22px;
        border-radius: 1rem;
        border: 1px solid #dbe6f5;
        background: #f8fbff;
        margin-bottom: 20px;
        font-size: 14px;
        color: #334155;
    }
    .info-meta__label { font-weight: 700; color: #64748b; }
    .info-meta__value { font-weight: 700; color: #0f172a; }

    .info-btn {
        display: inline-flex; align-items: center; justify-content: center;
        min-height: 44px; padding: 0 24px; border: 0; border-radius: 0.375rem;
        font-size: 14px; font-weight: 700; color: #fff; cursor: pointer;
        background: linear-gradient(135deg,#dc2626 0%,#b91c1c 100%);
        box-shadow: 0 8px 16px rgba(220,38,38,.2);
        transition: transform .15s;
    }
    .info-btn:hover { transform: translateY(-1px); }

    .info-msg { font-size: 14px; font-weight: 700; color: #334155; margin-top: 8px; }
</style>

<div class="info-wrap">

    <div class="info-hero">
        <h1 class="info-hero__title"><asp:Label ID="Labelwelcome" runat="server"></asp:Label></h1>
        <p class="info-hero__sub">教师信息中心 · 班级管理与系统操作</p>
    </div>

    <div class="info-card">
        <div class="info-card__head">
            <h2 class="info-card__title">我的班级列表</h2>
        </div>
        <div class="info-card__body">
            <div id="class-grid" class="info-class-grid"></div>
            <asp:DataList ID="DLmyclass" runat="server" RepeatColumns="10"
                RepeatDirection="Horizontal" CellPadding="0" CellSpacing="0"
                onitemdatabound="DLmyclass_ItemDataBound" style="display:none;">
                <ItemTemplate>
                    <asp:HyperLink ID="HyperRgradeclass" runat="server" Font-Underline="False"
                        Text='<%# Eval("Rgradeclass") %>' CssClass="info-class-item"></asp:HyperLink>
                    <asp:Label ID="LabelRset" runat="server" Text='<%# Eval("Rset") %>' Visible="False"></asp:Label>
                    <asp:Label ID="LabelRreg" runat="server" Text='<%# Eval("Rreg") %>' Visible="False"></asp:Label>
                </ItemTemplate>
            </asp:DataList>
        </div>
    </div>

    <div class="info-meta">
        <span class="info-meta__label">当前学期</span>
        <span class="info-meta__value"><asp:Label ID="Labelterm" runat="server"></asp:Label></span>
    </div>

    <div style="display:flex;align-items:center;gap:16px;flex-wrap:wrap;">
        <asp:Label ID="Labelmsg" runat="server" SkinID="LabelMsgBlack" CssClass="info-msg"></asp:Label>
    </div>

</div>
<script>
(function(){
    var grid = document.getElementById('class-grid');
    document.querySelectorAll('.info-class-item').forEach(function(el, i){
        el.classList.add('info-cc-' + (i % 6));
        grid.appendChild(el);
    });
})();
</script>
</asp:Content>
