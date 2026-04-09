<%@ Page Title="" Language="C#" MasterPageFile="~/profile/Pf.master"   StylesheetTheme="Student"  AutoEventWireup="true" CodeFile="myterm.aspx.cs" Inherits="Profile_myterm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cstu" Runat="Server">
<link rel="stylesheet" type="text/css" href="../App_Themes/Student/myterm.css" />

<div class="pf-start">
  <div class="pf-hero pf-hero--blue">
    <div>
      <span class="pf-hero__eyebrow">Learning Archive</span>
      <h1 class="pf-hero__title">我的学习成果</h1>
      <p class="pf-hero__subtitle">查看各学期作品分、测验分、小组分、讨论分与表现分，快速了解自己的阶段性学习表现。</p>
    </div>
    <div class="pf-hero__panel">
      <div class="pf-hero__panel-label">阅读方式</div>
      <div class="pf-hero__panel-text">每张卡片代表一个学期，顶部展示学期与综合素质，底部展示该学期总评分。</div>
    </div>
  </div>

  <div class="pf-card pf-card--transparent">
    <div class="pf-card__head pf-card__head--simple">
      <div class="pf-card__heading">
        <div class="pf-card__icon pf-card__icon--blue">
          <svg width="17" height="17" fill="none" stroke="#2563eb" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4M7.835 4.697a3.42 3.42 0 001.946-.806 3.42 3.42 0 014.438 0 3.42 3.42 0 001.946.806 3.42 3.42 0 013.138 3.138 3.42 3.42 0 00.806 1.946 3.42 3.42 0 010 4.438 3.42 3.42 0 00-.806 1.946 3.42 3.42 0 01-3.138 3.138 3.42 3.42 0 00-1.946.806 3.42 3.42 0 01-4.438 0 3.42 3.42 0 00-1.946-.806 3.42 3.42 0 01-3.138-3.138 3.42 3.42 0 00-.806-1.946 3.42 3.42 0 010-4.438 3.42 3.42 0 00.806-1.946 3.42 3.42 0 013.138-3.138z"/></svg>
        </div>
        <div>
          <h2 class="pf-card__title">学期成绩档案</h2>
          <p class="pf-card__desc">按学期汇总你的综合学习成果。</p>
        </div>
      </div>
    </div>
    <div class="pf-card__body pf-card__body--flat">
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
  </div>
</div>
</asp:Content>
