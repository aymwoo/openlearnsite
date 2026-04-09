<%@ Page Title="" Language="C#" MasterPageFile="~/profile/Pf.master" StylesheetTheme="Student"  AutoEventWireup="true" CodeFile="mysex.aspx.cs" Inherits="Profile_mysex" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cstu" Runat="Server">
<style>
.pf-start{padding:20px 16px;background:linear-gradient(180deg,#f8fbff 0%,#f3f7ff 100%);font-family:-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;color:#0f172a}
.pf-start *{box-sizing:border-box}
.pf-card{border:1px solid #dbe6f5;border-radius:1rem;background:#fff;box-shadow:0 8px 24px rgba(15,23,42,.06);overflow:hidden;max-width:420px;margin:0 auto}
.pf-card__head{padding:16px 20px;border-bottom:1px solid #f1f5f9;display:flex;align-items:center;gap:10px}
.pf-card__icon{width:36px;height:36px;border-radius:.75rem;background:#fdf4ff;border:1px solid #e9d5ff;display:inline-flex;align-items:center;justify-content:center;flex-shrink:0}
.pf-card__title{margin:0;font-size:16px;font-weight:800;color:#0f172a;letter-spacing:-.02em}
.pf-card__body{padding:20px}
.pf-field{display:flex;flex-direction:column;gap:5px;margin-bottom:14px}
.pf-label{font-size:11px;font-weight:700;color:#475569;text-transform:uppercase;letter-spacing:.05em}
.pf-select{display:block!important;width:100%!important;min-height:44px!important;padding:0 14px!important;border:1px solid #cbd5e1!important;border-radius:.75rem!important;background:#f8fafc!important;color:#0f172a!important;font-size:14px!important;transition:border-color .2s,box-shadow .2s}
.pf-select:focus{border-color:#c084fc!important;outline:none!important;background:#fff!important;box-shadow:0 0 0 4px rgba(192,132,252,.18)!important}
.pf-btn{display:block!important;width:100%!important;min-height:44px!important;padding:0!important;border-radius:.75rem!important;border:0!important;font-size:14px!important;font-weight:700!important;cursor:pointer!important;color:#fff!important;background:linear-gradient(135deg,#9333ea 0%,#7c3aed 100%)!important;box-shadow:0 8px 20px rgba(147,51,234,.22)!important;margin-top:4px!important;transition:transform .18s!important}
.pf-btn:hover{transform:translateY(-1px)!important}
.pf-btn[disabled]{opacity:.55!important;cursor:not-allowed!important;transform:none!important;box-shadow:none!important}
.pf-msg{display:block;margin-top:12px;padding:10px 14px;border-radius:.75rem;font-size:13px;background:#fee2e2;color:#991b1b;border:1px solid #fecaca}
</style>
<div class="pf-start">
  <div class="pf-card">
    <div class="pf-card__head">
      <div class="pf-card__icon">
        <svg width="18" height="18" fill="none" stroke="#9333ea" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"/></svg>
      </div>
      <h2 class="pf-card__title">性别修改</h2>
    </div>
    <div class="pf-card__body">
      <div class="pf-field">
        <label class="pf-label">请选择性别</label>
        <asp:DropDownList ID="DDLsex" runat="server" CssClass="pf-select"></asp:DropDownList>
      </div>
      <asp:Button ID="Btnsex" runat="server" OnClick="Btnsex_Click" Text="确定修改" TabIndex="2" SkinID="buttonSkin" CssClass="pf-btn" />
      <asp:Label ID="Labelstr" runat="server" SkinID="LabelMsgRed" CssClass="pf-msg"></asp:Label>
    </div>
  </div>
</div>
</asp:Content>

