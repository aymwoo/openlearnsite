<%@ Page Title="" Language="C#" MasterPageFile="~/profile/Pf.master" StylesheetTheme="Student" AutoEventWireup="true" CodeFile="myclass.aspx.cs" Inherits="Profile_myclass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cstu" Runat="Server">
<style>
.pf-start{padding:20px 16px;background:linear-gradient(180deg,#f8fbff 0%,#f3f7ff 100%);font-family:-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;color:#0f172a}
.pf-start *{box-sizing:border-box}
.pf-card{border:1px solid #dbe6f5;border-radius:1rem;background:#fff;box-shadow:0 8px 24px rgba(15,23,42,.06);overflow:hidden;max-width:420px;margin:0 auto}
.pf-card__head{padding:16px 20px;border-bottom:1px solid #f1f5f9;display:flex;align-items:center;gap:10px}
.pf-card__icon{width:36px;height:36px;border-radius:.75rem;background:#f0fdf4;border:1px solid #bbf7d0;display:inline-flex;align-items:center;justify-content:center;flex-shrink:0}
.pf-card__title{margin:0;font-size:16px;font-weight:800;color:#0f172a;letter-spacing:-.02em}
.pf-card__body{padding:20px}
.pf-field{display:flex;flex-direction:column;gap:5px;margin-bottom:14px}
.pf-label{font-size:11px;font-weight:700;color:#475569;text-transform:uppercase;letter-spacing:.05em}
.pf-current{display:inline-flex;align-items:center;min-height:38px;padding:0 14px;background:#f0fdf4;border:1px solid #bbf7d0;border-radius:.625rem;color:#166534;font-weight:700;font-size:15px}
.pf-select{display:block!important;width:100%!important;min-height:44px!important;padding:0 14px!important;border:1px solid #cbd5e1!important;border-radius:.75rem!important;background:#f8fafc!important;color:#0f172a!important;font-size:14px!important;transition:border-color .2s,box-shadow .2s}
.pf-select:focus{border-color:#4ade80!important;outline:none!important;background:#fff!important;box-shadow:0 0 0 4px rgba(74,222,128,.18)!important}
.pf-btn{display:block!important;width:100%!important;min-height:44px!important;padding:0!important;border-radius:.75rem!important;border:0!important;font-size:14px!important;font-weight:700!important;cursor:pointer!important;color:#fff!important;background:linear-gradient(135deg,#16a34a 0%,#15803d 100%)!important;box-shadow:0 8px 20px rgba(22,163,74,.22)!important;margin-top:4px!important;transition:transform .18s!important}
.pf-btn:hover{transform:translateY(-1px)!important}
.pf-btn[disabled]{opacity:.55!important;cursor:not-allowed!important;transform:none!important;box-shadow:none!important}
.pf-msg{display:block;margin-top:12px;padding:10px 14px;border-radius:.75rem;font-size:13px;background:#fee2e2;color:#991b1b;border:1px solid #fecaca}
</style>
<div class="pf-start">
  <div class="pf-card">
    <div class="pf-card__head">
      <div class="pf-card__icon">
        <svg width="18" height="18" fill="none" stroke="#16a34a" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4"/></svg>
      </div>
      <h2 class="pf-card__title">班级修改</h2>
    </div>
    <div class="pf-card__body">
      <div class="pf-field">
        <span class="pf-label">当前所在班级</span>
        <span class="pf-current"><asp:Label ID="Labelclass" runat="server"></asp:Label></span>
      </div>
      <div class="pf-field">
        <label class="pf-label">选择新班级</label>
        <asp:DropDownList ID="DDLclass" runat="server" CssClass="pf-select"></asp:DropDownList>
      </div>
      <asp:Button ID="Btnclass" runat="server" OnClick="Btnclass_Click" Text="确定修改" TabIndex="2" SkinID="buttonSkin" Enabled="False" CssClass="pf-btn" />
      <asp:Label ID="Labelstr" runat="server" SkinID="LabelMsgRed" CssClass="pf-msg"></asp:Label>
    </div>
  </div>
</div>
</asp:Content>

