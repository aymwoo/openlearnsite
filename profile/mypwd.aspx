<%@ Page Title="" Language="C#" MasterPageFile="~/profile/Pf.master" StylesheetTheme="Student"  AutoEventWireup="true" CodeFile="mypwd.aspx.cs" Inherits="Profile_mypwd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cstu" Runat="Server">
<style>
.pf-start{padding:20px 16px;background:linear-gradient(180deg,#f8fbff 0%,#f3f7ff 100%);font-family:-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;color:#0f172a}
.pf-start *{box-sizing:border-box}
.pf-card{border:1px solid #dbe6f5;border-radius:1rem;background:#fff;box-shadow:0 8px 24px rgba(15,23,42,.06);overflow:hidden;max-width:420px;margin:0 auto}
.pf-card__head{padding:16px 20px;border-bottom:1px solid #f1f5f9;display:flex;align-items:center;gap:10px}
.pf-card__icon{width:36px;height:36px;border-radius:.75rem;background:#fef3c7;border:1px solid #fde68a;display:inline-flex;align-items:center;justify-content:center;flex-shrink:0}
.pf-card__title{margin:0;font-size:16px;font-weight:800;color:#0f172a;letter-spacing:-.02em}
.pf-card__body{padding:20px}
.pf-field{display:flex;flex-direction:column;gap:5px;margin-bottom:14px}
.pf-label{font-size:11px;font-weight:700;color:#475569;text-transform:uppercase;letter-spacing:.05em}
.pf-input{display:block!important;width:100%!important;min-height:44px!important;padding:0 14px!important;border:1px solid #cbd5e1!important;border-radius:.75rem!important;background:#f8fafc!important;color:#0f172a!important;font-size:14px!important;letter-spacing:.1em!important;transition:border-color .2s,box-shadow .2s}
.pf-input:focus{border-color:#60a5fa!important;outline:none!important;background:#fff!important;box-shadow:0 0 0 4px rgba(96,165,250,.18)!important}
.pf-divider{height:1px;background:#f1f5f9;margin:16px 0}
.pf-btn{display:block!important;width:100%!important;min-height:44px!important;padding:0!important;border-radius:.75rem!important;border:0!important;font-size:14px!important;font-weight:700!important;cursor:pointer!important;color:#fff!important;background:linear-gradient(135deg,#d97706 0%,#b45309 100%)!important;box-shadow:0 8px 20px rgba(217,119,6,.22)!important;margin-top:4px!important;transition:transform .18s!important}
.pf-btn:hover{transform:translateY(-1px)!important}
.pf-btn[disabled]{opacity:.55!important;cursor:not-allowed!important;transform:none!important;box-shadow:none!important}
.pf-msg{display:block;margin-top:12px;padding:10px 14px;border-radius:.75rem;font-size:13px;background:#fee2e2;color:#991b1b;border:1px solid #fecaca}
.pf-note{font-size:12px;color:#94a3b8;margin-top:4px}
</style>
<div class="pf-start">
  <div class="pf-card">
    <div class="pf-card__head">
      <div class="pf-card__icon">
        <svg width="18" height="18" fill="none" stroke="#d97706" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z"/></svg>
      </div>
      <h2 class="pf-card__title">密码修改</h2>
    </div>
    <div class="pf-card__body">
      <div class="pf-field">
        <label class="pf-label">旧密码</label>
        <asp:TextBox ID="TextBoxoldpwd" runat="server" TextMode="Password" TabIndex="1" SkinID="TextBox" ToolTip="旧密码确认框" CssClass="pf-input"></asp:TextBox>
      </div>
      <div class="pf-divider"></div>
      <div class="pf-field">
        <label class="pf-label">新密码</label>
        <asp:TextBox ID="TextBoxpwd" runat="server" TextMode="Password" TabIndex="2" SkinID="TextBox" ToolTip="新密码" CssClass="pf-input"></asp:TextBox>
      </div>
      <div class="pf-field">
        <label class="pf-label">确认新密码</label>
        <asp:TextBox ID="TextBoxpwd0" runat="server" TextMode="Password" TabIndex="3" SkinID="TextBox" ToolTip="确认新密码" CssClass="pf-input"></asp:TextBox>
      </div>
      <asp:Button ID="Btnedit" runat="server" OnClick="Btnedit_Click" Text="确定修改" TabIndex="4" SkinID="buttonSkin" Enabled="False" CssClass="pf-btn" />
      <asp:Label ID="Labelmsg" runat="server" SkinID="LabelMsgRed" CssClass="pf-msg"></asp:Label>
    </div>
  </div>
</div>
</asp:Content>

