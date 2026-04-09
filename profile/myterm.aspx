<%@ Page Title="" Language="C#" MasterPageFile="~/profile/Pf.master"   StylesheetTheme="Student"  AutoEventWireup="true" CodeFile="myterm.aspx.cs" Inherits="Profile_myterm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cstu" Runat="Server">
<style>
.pf-start{padding:20px 16px;background:linear-gradient(180deg,#f8fbff 0%,#f3f7ff 100%);font-family:-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;color:#0f172a}
.pf-start *{box-sizing:border-box}
.pf-title{margin:0 0 16px;font-size:18px;font-weight:800;color:#0f172a;letter-spacing:-.03em;display:flex;align-items:center;gap:8px}
.pf-term-grid{display:grid;gap:14px;grid-template-columns:repeat(auto-fill,minmax(280px,1fr))}
.pf-term-card{border:1px solid #dbe6f5;border-radius:1rem;background:#fff;box-shadow:0 8px 24px rgba(15,23,42,.05);overflow:hidden}
.pf-term-card__hero{padding:14px 16px;background:linear-gradient(135deg,#1e3a8a 0%,#2563eb 60%,#38bdf8 100%);color:#fff;display:flex;align-items:center;gap:12px}
.pf-term-card__img{width:48px;height:48px;border-radius:.75rem;object-fit:cover;border:2px solid rgba(255,255,255,.3);background:rgba(255,255,255,.1);flex-shrink:0}
.pf-term-card__meta{flex:1;min-width:0}
.pf-term-card__period{font-size:11px;font-weight:700;letter-spacing:.06em;color:rgba(255,255,255,.75);text-transform:uppercase;margin-bottom:2px}
.pf-term-card__name{font-size:15px;font-weight:800;color:#fff;letter-spacing:-.01em}
.pf-term-card__ape{margin-top:4px}
.pf-ape-badge{display:inline-flex;align-items:center;font-size:11px;font-weight:700;padding:2px 8px;border-radius:99px;background:rgba(255,255,255,.2);color:#fff;border:1px solid rgba(255,255,255,.3)}
.pf-term-card__body{padding:14px 16px}
.pf-term-row{display:flex;align-items:center;justify-content:space-between;padding:6px 0;border-bottom:1px solid #f1f5f9;font-size:13px;gap:8px}
.pf-term-row:last-child{border-bottom:0}
.pf-term-row__label{color:#64748b;font-weight:500;white-space:nowrap;flex-shrink:0}
.pf-term-row__value{font-weight:800;color:#1e293b;white-space:nowrap;flex-shrink:0}
.pf-term-card__footer{padding:10px 16px;background:#f8fafc;border-top:2px solid #dbe6f5;display:flex;justify-content:space-between;align-items:center}
.pf-term-total-label{font-size:12px;font-weight:700;color:#475569;text-transform:uppercase;letter-spacing:.05em}
.pf-term-total-value{font-size:22px;font-weight:900;color:#2563eb;letter-spacing:-.02em}
</style>
<div class="pf-start">
  <h2 class="pf-title">
    <svg width="20" height="20" fill="none" stroke="#2563eb" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4M7.835 4.697a3.42 3.42 0 001.946-.806 3.42 3.42 0 014.438 0 3.42 3.42 0 001.946.806 3.42 3.42 0 013.138 3.138 3.42 3.42 0 00.806 1.946 3.42 3.42 0 010 4.438 3.42 3.42 0 00-.806 1.946 3.42 3.42 0 01-3.138 3.138 3.42 3.42 0 00-1.946.806 3.42 3.42 0 01-4.438 0 3.42 3.42 0 00-1.946-.806 3.42 3.42 0 01-3.138-3.138 3.42 3.42 0 00-.806-1.946 3.42 3.42 0 010-4.438 3.42 3.42 0 00.806-1.946 3.42 3.42 0 013.138-3.138z"/></svg>
    我的学习成果
  </h2>
  <asp:DataList ID="DLterm" runat="server" RepeatLayout="Flow" CssClass="pf-term-grid">
    <ItemTemplate>
      <div class="pf-term-card">
        <div class="pf-term-card__hero">
          <asp:Image ID="Image1" runat="server" ImageUrl="~/images/term.png" CssClass="pf-term-card__img" />
          <div class="pf-term-card__meta">
            <div class="pf-term-card__period">
              <asp:Label ID="Label1" runat="server" Text='<%# Eval("Tgrade") %>'></asp:Label>年级 · 第<asp:Label ID="Label2" runat="server" Text='<%# Eval("Tterm") %>'></asp:Label>学期
            </div>
            <div class="pf-term-card__name"><asp:Label ID="Label11" runat="server" Text='<%# Eval("Sname") %>'></asp:Label></div>
            <div class="pf-term-card__ape"><span class="pf-ape-badge">综合素质：<asp:Label ID="Label10" runat="server" Text='<%# Eval("Tape") %>'></asp:Label></span></div>
          </div>
        </div>
        <div class="pf-term-card__body">
          <div class="pf-term-row"><span class="pf-term-row__label">作品分</span><span class="pf-term-row__value"><asp:Label ID="Label3" runat="server" Text='<%# Eval("Tscore") %>'></asp:Label></span></div>
          <div class="pf-term-row"><span class="pf-term-row__label">测验分</span><span class="pf-term-row__value"><asp:Label ID="Label31" runat="server" Text='<%# Eval("Tvscore") %>'></asp:Label></span></div>
          <div class="pf-term-row"><span class="pf-term-row__label">小组分</span><span class="pf-term-row__value"><asp:Label ID="Label4" runat="server" Text='<%# Eval("Tgscore") %>'></asp:Label></span></div>
          <div class="pf-term-row"><span class="pf-term-row__label">讨论分</span><span class="pf-term-row__value"><asp:Label ID="Label5" runat="server" Text='<%# Eval("Tpscore") %>'></asp:Label></span></div>
          <div class="pf-term-row"><span class="pf-term-row__label">常识分</span><span class="pf-term-row__value"><asp:Label ID="Label6" runat="server" Text='<%# Eval("Tquiz") %>'></asp:Label></span></div>
          <div class="pf-term-row"><span class="pf-term-row__label">打字分</span><span class="pf-term-row__value"><asp:Label ID="Label7" runat="server" Text='<%# Eval("Ttscore") %>'></asp:Label></span></div>
          <div class="pf-term-row"><span class="pf-term-row__label">表现分</span><span class="pf-term-row__value"><asp:Label ID="Label8" runat="server" Text='<%# Eval("Tattitude") %>'></asp:Label></span></div>
        </div>
        <div class="pf-term-card__footer">
          <span class="pf-term-total-label">总评分</span>
          <span class="pf-term-total-value"><asp:Label ID="Label9" runat="server" Text='<%# Eval("Tallscore") %>'></asp:Label></span>
        </div>
      </div>
    </ItemTemplate>
  </asp:DataList>
</div>
</asp:Content>

