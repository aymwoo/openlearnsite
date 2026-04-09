<%@ Page Title="" Language="C#" MasterPageFile="~/profile/Pf.master"  StylesheetTheme="Student" AutoEventWireup="true" CodeFile="mychange.aspx.cs" Inherits="Profile_mychange" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cstu" Runat="Server">
<link rel="stylesheet" type="text/css" href="../App_Themes/Student/mychange.css" />

<div class="pf-start">
  <div class="pf-hero pf-hero--amber">
    <div>
      <span class="pf-hero__eyebrow">Leader Vote</span>
      <h1 class="pf-hero__title">推荐组长</h1>
      <p class="pf-hero__subtitle">根据团队协作情况推荐适合的组长；如果你具备权限，也可以直接任命或撤销当前组长。</p>
    </div>
    <div class="pf-hero__panel">
      <div class="pf-hero__panel-label">说明</div>
      <div class="pf-hero__panel-text">推荐票会显示在同学卡片中；管理员可直接点击头像区域切换组长身份。</div>
    </div>
  </div>

  <div class="pf-card">
    <div class="pf-card__head">
      <div class="pf-card__heading">
        <div class="pf-card__icon pf-card__icon--amber">
          <svg width="17" height="17" fill="none" stroke="#d97706" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11.049 2.927c.3-.921 1.603-.921 1.902 0l1.519 4.674a1 1 0 00.95.69h4.915c.969 0 1.371 1.24.588 1.81l-3.976 2.888a1 1 0 00-.363 1.118l1.518 4.674c.3.922-.755 1.688-1.538 1.118l-3.976-2.888a1 1 0 00-1.176 0l-3.976 2.888c-.783.57-1.838-.197-1.538-1.118l1.518-4.674a1 1 0 00-.363-1.118l-3.976-2.888c-.784-.57-.38-1.81.588-1.81h4.914a1 1 0 00.951-.69l1.519-4.674z"/></svg>
        </div>
        <div>
          <h2 class="pf-card__title">候选同学</h2>
          <p class="pf-card__desc">按同学卡片查看当前票数，并进行推荐或组长切换操作。</p>
        </div>
      </div>
      <span class="pf-badge pf-badge--amber">小组协作</span>
    </div>
    <div class="pf-card__body">
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
            <div class="pf-stu-card__avatar-wrap">
              <asp:ImageButton ID="ImageBtnGroup" runat="server" CausesValidation="False"
                  CommandArgument='<%# Eval("Sid") %>' CommandName="ChangeGroup"
                  ImageUrl="~/images/gcard.gif" CssClass="pf-stu-card__img" />
            </div>
            <span class="pf-stu-card__votes"><asp:Label ID="Labelvote" runat="server" Text='<%# Eval("Steam") %>' ToolTip="组长票数"></asp:Label> 票</span>
            <asp:ImageButton ID="LinkBtnVote" runat="server"
                CommandArgument='<%# Eval("Sid") %>' CommandName="Vote"
                ToolTip="点击推荐组长" ImageUrl="~/images/good.png"
                CausesValidation="False" CssClass="pf-stu-card__vote-btn" />
          </div>
        </ItemTemplate>
      </asp:DataList>
    </div>
  </div>
</div>
</asp:Content>
