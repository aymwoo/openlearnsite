<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manage.master" StylesheetTheme="Teacher"  AutoEventWireup="true" CodeFile="copygood.aspx.cs" Inherits="Manager_copygood" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
<style type="text/css">
    .cg-wrap { padding: 28px; background: linear-gradient(180deg,#f8fbff 0%,#f3f7ff 100%); box-sizing: border-box; }
    .cg-wrap * { box-sizing: border-box; }
    .cg-hero { border-radius: 1rem; padding: 24px 28px; background: linear-gradient(135deg,#f0fdf4 0%,#dcfce7 100%); color: #14532d; border: 1px solid #bbf7d0; box-shadow: 0 4px 16px rgba(22,163,74,.08); margin-bottom: 24px; }
    .cg-hero h1 { margin: 0; font-size: 24px; font-weight: 800; letter-spacing: -0.02em; }
    .cg-hero p  { margin: 8px 0 0; font-size: 14px; color: #166534; line-height: 1.8; }
    .cg-card { border-radius: 1rem; border: 1px solid #dbe6f5; background: rgba(255,255,255,.96); box-shadow: 0 12px 30px rgba(15,23,42,.05); margin-bottom: 20px; }
    .cg-card__head { padding: 18px 22px 0; }
    .cg-card__title { margin: 0; font-size: 16px; font-weight: 800; color: #0f172a; }
    .cg-card__body  { padding: 18px 22px 22px; display: flex; flex-direction: column; gap: 14px; }
    .cg-alert { padding: 12px 16px; border-radius: 1rem; background: #f0fdf4; border: 1px solid #bbf7d0; font-size: 14px; color: #166534; line-height: 1.8; }
    .cg-btn { display: inline-flex; align-items: center; justify-content: center; min-height: 44px; padding: 0 24px; border: 0; border-radius: 1rem; font-size: 14px; font-weight: 700; color: #fff; cursor: pointer; background: linear-gradient(135deg,#16a34a 0%,#15803d 100%); box-shadow: 0 8px 16px rgba(22,163,74,.2); transition: transform .15s; }
    .cg-btn:hover { transform: translateY(-1px); }
    .cg-msg { font-size: 14px; font-weight: 700; color: #15803d; }
</style>

<div class="cg-wrap">
    <div class="cg-hero">
        <h1>备份优秀作品</h1>
        <p>将所有 12 分的推荐作品复制到网站 GoodStore 目录下，按入学年度、学案分级保存，方便提取独立展示。</p>
    </div>
    <div class="cg-card">
        <div class="cg-card__head"><h2 class="cg-card__title">执行备份</h2></div>
        <div class="cg-card__body">
            <div class="cg-alert">仅复制评分达到 12 分的推荐作品，不影响原始数据，可重复执行。</div>
            <div style="display:flex;align-items:center;gap:16px;flex-wrap:wrap;">
                <asp:Button ID="Btnbackup" runat="server" Text="备份优秀作品" onclick="Btnbackup_Click" CssClass="cg-btn" />
                <asp:Label ID="Labelmsg" runat="server" CssClass="cg-msg"></asp:Label>
            </div>
        </div>
    </div>
</div>
</asp:Content>

