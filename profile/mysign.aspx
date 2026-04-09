<%@ Page Title="" Language="C#" MasterPageFile="~/profile/Pf.master"   StylesheetTheme="Student" AutoEventWireup="true" CodeFile="mysign.aspx.cs" Inherits="Profile_mysign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cstu" Runat="Server">
<style>
.pf-start{padding:20px 16px;background:linear-gradient(180deg,#f8fbff 0%,#f3f7ff 100%);font-family:-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;color:#0f172a}
.pf-start *{box-sizing:border-box}
.pf-card{border:1px solid #dbe6f5;border-radius:1rem;background:#fff;box-shadow:0 8px 24px rgba(15,23,42,.06);overflow:hidden;margin-bottom:16px}
.pf-card__head{padding:14px 18px;border-bottom:1px solid #f1f5f9;display:flex;align-items:center;justify-content:space-between;gap:10px}
.pf-card__heading{display:flex;align-items:center;gap:10px}
.pf-card__icon{width:34px;height:34px;border-radius:.75rem;display:inline-flex;align-items:center;justify-content:center;flex-shrink:0}
.pf-card__icon--teal{background:#ccfbf1;border:1px solid #99f6e4}
.pf-card__icon--rose{background:#ffe4e6;border:1px solid #fecdd3}
.pf-card__title{margin:0;font-size:15px;font-weight:800;color:#0f172a;letter-spacing:-.02em}
.pf-badge{display:inline-flex;align-items:center;font-size:11px;font-weight:700;padding:2px 8px;border-radius:99px}
.pf-badge--teal{background:#ccfbf1;color:#0f766e;border:1px solid #99f6e4}
.pf-badge--rose{background:#ffe4e6;color:#be123c;border:1px solid #fecdd3}
.pf-card__body{padding:0}
/* GridView table */
.pf-grid-wrap{overflow-x:auto}
.pf-grid-wrap table{width:100%;border-collapse:collapse;font-size:13px}
.pf-grid-wrap th{background:#f8fafc;padding:9px 12px;font-weight:700;color:#475569;font-size:11px;text-transform:uppercase;letter-spacing:.05em;border-bottom:1px solid #e2e8f0;text-align:left;white-space:nowrap}
.pf-grid-wrap td{padding:10px 12px;border-bottom:1px solid #f1f5f9;color:#1e293b;vertical-align:middle}
.pf-grid-wrap tr:last-child td{border-bottom:0}
.pf-grid-wrap tr:hover td{background:#f8fafc}
/* pager */
.pf-pager{display:flex;align-items:center;gap:6px;padding:10px 12px;background:#f8fafc;border-top:1px solid #e2e8f0;font-size:12px;color:#64748b;flex-wrap:wrap}
.pf-pager a{display:inline-flex;align-items:center;justify-content:center;min-height:26px;padding:0 10px;border-radius:.5rem;border:1px solid #e2e8f0;background:#fff;color:#475569;font-size:11px;font-weight:700;text-decoration:none;cursor:pointer;transition:background .2s}
.pf-pager a:hover{background:#eff6ff;border-color:#bfdbfe;color:#2563eb}
</style>
<div class="pf-start">
  <!-- 签到记录 -->
  <div class="pf-card">
    <div class="pf-card__head">
      <div class="pf-card__heading">
        <div class="pf-card__icon pf-card__icon--teal">
          <svg width="17" height="17" fill="none" stroke="#0d9488" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-6 9l2 2 4-4"/></svg>
        </div>
        <h2 class="pf-card__title">签到记录</h2>
      </div>
      <span class="pf-badge pf-badge--teal"><asp:Label ID="Labelsignin" runat="server"></asp:Label></span>
    </div>
    <div class="pf-card__body">
      <div class="pf-grid-wrap">
        <asp:GridView ID="GVSignin" runat="server" AutoGenerateColumns="False"
            CellPadding="0" PageSize="15" Width="100%" ToolTip="签到记录"
            SkinID="GridViewInfo" onrowdatabound="GVSignin_RowDataBound"
            AllowPaging="True" onpageindexchanging="GVSignin_PageIndexChanging"
            EnableModelValidation="True" GridLines="None" BorderWidth="0">
            <FooterStyle BackColor="White" ForeColor="#333333" />
            <Columns>
                <asp:BoundField />
                <asp:BoundField DataField="Qnum" HeaderText="学号" />
                <asp:BoundField DataField="Sgrade" HeaderText="年级" />
                <asp:BoundField DataField="Sclass" HeaderText="班级" />
                <asp:BoundField DataField="Sname" HeaderText="姓名"><ItemStyle HorizontalAlign="Left" /></asp:BoundField>
                <asp:BoundField DataField="Qwork" HeaderText="作品" />
                <asp:BoundField DataField="Qattitude" HeaderText="表现" />
                <asp:BoundField DataField="Qnote" HeaderText="备注" />
                <asp:BoundField DataField="Qip" HeaderText="IP地址" />
                <asp:BoundField DataField="Qdate" HeaderText="日期"><ItemStyle Width="160px" HorizontalAlign="Left" /></asp:BoundField>
            </Columns>
            <PagerTemplate>
              <div class="pf-pager">
                第<asp:Label ID="lblPageIndex" runat="server" Text="<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>"></asp:Label>页&nbsp;/&nbsp;共<asp:Label ID="lblPageCount" runat="server" Text="<%# ((GridView)Container.Parent.Parent).PageCount %>"></asp:Label>页
                <asp:LinkButton ID="btnFirst" runat="server" CausesValidation="False" CommandArgument="First" CommandName="Page" Text="首页"></asp:LinkButton>
                <asp:LinkButton ID="btnPrev" runat="server" CausesValidation="False" CommandArgument="Prev" CommandName="Page" Text="上一页"></asp:LinkButton>
                <asp:LinkButton ID="btnNext" runat="server" CausesValidation="False" CommandArgument="Next" CommandName="Page" Text="下一页"></asp:LinkButton>
                <asp:LinkButton ID="btnLast" runat="server" CausesValidation="False" CommandArgument="Last" CommandName="Page" Text="尾页"></asp:LinkButton>
              </div>
            </PagerTemplate>
        </asp:GridView>
      </div>
    </div>
  </div>

  <!-- 缺席记录 -->
  <div class="pf-card">
    <div class="pf-card__head">
      <div class="pf-card__heading">
        <div class="pf-card__icon pf-card__icon--rose">
          <svg width="17" height="17" fill="none" stroke="#e11d48" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 14l2-2m0 0l2-2m-2 2l-2-2m2 2l2 2m7-2a9 9 0 11-18 0 9 9 0 0118 0z"/></svg>
        </div>
        <h2 class="pf-card__title">缺席记录</h2>
      </div>
      <span class="pf-badge pf-badge--rose"><asp:Label ID="Labelnosign" runat="server"></asp:Label></span>
    </div>
    <div class="pf-card__body">
      <div class="pf-grid-wrap">
        <asp:GridView ID="GVNoSign" runat="server" AutoGenerateColumns="False"
            CellPadding="0" Width="100%" ToolTip="缺席记录" SkinID="GridViewInfo"
            onrowdatabound="GVNoSign_RowDataBound" DataKeyNames="Snum"
            EnableModelValidation="True" GridLines="None" BorderWidth="0">
            <Columns>
                <asp:BoundField />
                <asp:BoundField DataField="Snum" HeaderText="学号" />
                <asp:BoundField DataField="Sgrade" HeaderText="年级" />
                <asp:BoundField DataField="Sclass" HeaderText="班级" />
                <asp:BoundField DataField="Sname" HeaderText="姓名"><ItemStyle HorizontalAlign="Left" /></asp:BoundField>
                <asp:BoundField DataField="Sex" HeaderText="性别" />
                <asp:BoundField DataField="Sheadtheacher" HeaderText="班主任" />
                <asp:BoundField HeaderText="缺席原因" DataField="Nnote" />
                <asp:BoundField DataField="Ndate" HeaderText="日期"><ItemStyle HorizontalAlign="Left" Width="120px" /></asp:BoundField>
            </Columns>
        </asp:GridView>
      </div>
    </div>
  </div>
</div>
</asp:Content>

