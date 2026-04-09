<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manage.master" AutoEventWireup="true" CodeFile="teacheradd.aspx.cs" Inherits="Manager_teacheradd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <style type="text/css">
        .mgr-page { --ls-bg: linear-gradient(180deg,#f8fbff 0%,#f3f7ff 100%); --ls-border: #dbe6f5; --ls-text: #0f172a; padding: 28px; background: var(--ls-bg); min-height: calc(100vh - 8rem); box-sizing: border-box; width: 100%; }
        .mgr-page * { box-sizing: border-box; }
        .mgr-shell { display: flex; flex-direction: column; gap: 20px; }
        .mgr-hero { border: 1px solid #bfdbfe; border-radius: 1rem; padding: 24px 28px; background: linear-gradient(135deg,#eff6ff 0%,#dbeafe 100%); color: #1e3a8a; box-shadow: 0 4px 16px rgba(37,99,235,.08); }
        .mgr-hero__title { margin: 0; font-size: 22px; font-weight: 800; display: flex; align-items: center; gap: 10px; }
        .mgr-card { border: 1px solid var(--ls-border); border-radius: 1rem; background: rgba(255,255,255,.96); box-shadow: 0 12px 30px rgba(15,23,42,.05); overflow: hidden; }
        .mgr-card__head { padding: 20px 24px; border-bottom: 1px solid #f1f5f9; }
        .mgr-card__title { margin: 0; font-size: 16px; font-weight: 800; color: var(--ls-text); }
        .mgr-card__body { padding: 24px; display: flex; flex-direction: column; gap: 18px; }
        .mgr-field { display: flex; flex-direction: column; gap: 6px; }
        .mgr-label { font-size: 14px; font-weight: 700; color: #334155; }
        .mgr-input { min-height: 44px; padding: 0 14px; border: 1px solid #cbd5e1; border-radius: .75rem; background: #f8fafc; color: #0f172a; font-size: 14px; width: 100%; transition: border-color .2s, box-shadow .2s; }
        .mgr-input:focus { border-color: #60a5fa; outline: none; background: #fff; box-shadow: 0 0 0 4px rgba(96,165,250,.18); }
        .mgr-textarea { min-height: 80px; padding: 10px 14px; resize: vertical; }
        .mgr-actions { display: flex; gap: 10px; }
        .mgr-btn { display: inline-flex; align-items: center; justify-content: center; min-height: 44px; padding: 0 20px; border-radius: 1rem; border: none; font-size: 14px; font-weight: 700; cursor: pointer; transition: transform .18s, box-shadow .18s; }
        .mgr-btn--primary { background: linear-gradient(135deg,#4f46e5 0%,#4338ca 100%); color: #fff; box-shadow: 0 8px 16px rgba(79,70,229,.2); }
        .mgr-btn--primary:hover { transform: translateY(-1px); }
        .mgr-btn--outline { background: #fff; color: #475569; border: 1px solid #e2e8f0; }
        .mgr-btn--outline:hover { background: #f8fafc; }
        .mgr-msg { font-size: 14px; font-weight: 700; color: #dc2626; }
    </style>
    <div class="mgr-page">
        <div class="mgr-shell">
            <div class="mgr-hero">
                <h1 class="mgr-hero__title"><i class="bi bi-person-plus-fill" style="color:#93c5fd;"></i> 添加教师</h1>
            </div>
            <div class="mgr-card">
                <div class="mgr-card__head"><h2 class="mgr-card__title">教师信息</h2></div>
                <div class="mgr-card__body">
                    <div class="mgr-field">
                        <span class="mgr-label">账号</span>
                        <asp:TextBox ID="Texthname" runat="server" CssClass="mgr-input"></asp:TextBox>
                    </div>
                    <div class="mgr-field">
                        <span class="mgr-label">昵称</span>
                        <asp:TextBox ID="Texthnick" runat="server" CssClass="mgr-input"></asp:TextBox>
                    </div>
                    <div class="mgr-field">
                        <span class="mgr-label">密码</span>
                        <asp:TextBox ID="Texthpwd" runat="server" CssClass="mgr-input"></asp:TextBox>
                    </div>
                    <div class="mgr-field">
                        <span class="mgr-label">权限</span>
                        <label style="display:inline-flex;align-items:center;gap:8px;cursor:pointer;font-size:14px;color:#334155;font-weight:600;">
                            <asp:CheckBox ID="Ckhpermiss" runat="server" Text="" style="margin:0;" />
                            设置为管理员
                        </label>
                    </div>
                    <div class="mgr-field">
                        <span class="mgr-label">备注</span>
                        <asp:TextBox ID="Texthnote" runat="server" TextMode="MultiLine" CssClass="mgr-input mgr-textarea"></asp:TextBox>
                    </div>
                    <div class="mgr-actions">
                        <asp:Button ID="Btnadd" runat="server" Text="确定添加" onclick="Btnadd_Click" CssClass="mgr-btn mgr-btn--primary" />
                        <asp:Button ID="Btnreturn" runat="server" Text="返回" onclick="Btnreturn_Click" CssClass="mgr-btn mgr-btn--outline" />
                    </div>
                    <asp:Label ID="Labelmsg" runat="server" CssClass="mgr-msg"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
