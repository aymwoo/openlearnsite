<%@ Page Title="" Language="C#" MasterPageFile="~/profile/Pf.master"  StylesheetTheme="Student" AutoEventWireup="true" CodeFile="mychange.aspx.cs" Inherits="Profile_mychange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cstu" Runat="Server">
<style>
.pf-start{padding:20px 16px;background:linear-gradient(180deg,#f8fbff 0%,#f3f7ff 100%);font-family:-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;color:#0f172a}
.pf-start *{box-sizing:border-box}
.pf-title{margin:0 0 16px;font-size:18px;font-weight:800;color:#0f172a;letter-spacing:-.03em;display:flex;align-items:center;gap:8px}
.pf-stu-grid{display:flex;flex-wrap:wrap;gap:10px}
.pf-stu-card{border:1px solid #e2e8f0;border-radius:.875rem;background:linear-gradient(180deg,#fff 0%,#f8fafc 100%);padding:12px 8px;display:flex;flex-direction:column;align-items:center;gap:6px;width:108px;box-shadow:0 4px 12px rgba(15,23,42,.04);transition:box-shadow .2s,transform .2s,border-color .2s;cursor:pointer}
.pf-stu-card:hover{box-shadow:0 10px 28px rgba(15,23,42,.1);transform:translateY(-3px);border-color:#c7d2fe}
.pf-stu-card__name{font-size:13px;font-weight:700;color:#1e293b;text-align:center;line-height:1.3;min-height:34px;display:flex;align-items:center;justify-content:center}
.pf-stu-card__img{width:44px;height:44px;border-radius:.625rem;object-fit:cover;border:2px solid #e2e8f0;background:#f1f5f9}
.pf-stu-card__votes{font-size:11px;font-weight:700;color:#64748b;background:#f1f5f9;border-radius:99px;padding:2px 8px}
.pf-stu-card__vote-btn{display:inline-flex;align-items:center;justify-content:center;gap:4px;height:30px;padding:0 12px;border-radius:99px;background:#eff6ff;border:1px solid #bfdbfe;cursor:pointer;font-size:11px;font-weight:700;color:#2563eb;transition:background .2s,transform .2s}
.pf-stu-card__vote-btn:hover{background:#dbeafe;transform:scale(1.08)}
</style>
<div class="pf-start">
  <h2 class="pf-title">
    <svg width="20" height="20" fill="none" stroke="#f59e0b" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11.049 2.927c.3-.921 1.603-.921 1.902 0l1.519 4.674a1 1 0 00.95.69h4.915c.969 0 1.371 1.24.588 1.81l-3.976 2.888a1 1 0 00-.363 1.118l1.518 4.674c.3.922-.755 1.688-1.538 1.118l-3.976-2.888a1 1 0 00-1.176 0l-3.976 2.888c-.783.57-1.838-.197-1.538-1.118l1.518-4.674a1 1 0 00-.363-1.118l-3.976-2.888c-.784-.57-.38-1.81.588-1.81h4.914a1 1 0 00.951-.69l1.519-4.674z"/></svg>
    推荐组长
  </h2>
  <asp:DataList ID="DataListstu" runat="server" DataKeyField="Sid"
      RepeatLayout="Flow" Width="100%"
      onitemcommand="DataListstu_ItemCommand"
      onitemdatabound="DataListstu_ItemDataBound"
      CssClass="pf-stu-grid">
    <ItemTemplate>
      <div class="pf-stu-card">
        <asp:Label ID="LabelSleader" runat="server" Text='<%# Bind("Sleader") %>' Visible="false"></asp:Label>
        <asp:Label ID="LabelSnum" runat="server" Text='<%# Bind("Snum") %>' Visible="false"></asp:Label>
        <div class="pf-stu-card__name">
          <asp:HyperLink ID="HyperQname" runat="server" Font-Underline="False" Text='<%# Eval("Sname") %>'></asp:HyperLink>
        </div>
        <asp:ImageButton ID="ImageBtnGroup" runat="server" CausesValidation="False"
            CommandArgument='<%# Eval("Sid") %>' CommandName="ChangeGroup"
            ImageUrl="~/images/gcard.gif" CssClass="pf-stu-card__img" style="border-radius:.625rem" />
        <span class="pf-stu-card__votes"><asp:Label ID="Labelvote" runat="server" Text='<%# Eval("Steam") %>' ToolTip="组长票数"></asp:Label> 票</span>
        <asp:ImageButton ID="LinkBtnVote" runat="server"
            CommandArgument='<%# Eval("Sid") %>' CommandName="Vote"
            ToolTip="点击推荐组长" ImageUrl="~/images/good.png"
            CausesValidation="False" CssClass="pf-stu-card__vote-btn" style="width:auto;padding:0 10px;" />
      </div>
    </ItemTemplate>
  </asp:DataList>
</div>
</asp:Content>

