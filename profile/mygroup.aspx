<%@ Page Title="" Language="C#" MasterPageFile="~/profile/Pf.master"  StylesheetTheme="Student"  AutoEventWireup="true" CodeFile="mygroup.aspx.cs" Inherits="Profile_mygroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cstu" Runat="Server">
<link rel="stylesheet" type="text/css" href="../App_Themes/Student/mygroup.css" />

<div class="pf-start">
  <div class="pf-hero">
    <div>
      <span class="pf-hero__eyebrow">Group Center</span>
      <h1 class="pf-hero__title">我的小组</h1>
      <p class="pf-hero__subtitle">查看班级已有小组、申请加入合适的小组，并维护自己的小组名称，方便协作学习与课堂分工。</p>
    </div>
    <div class="pf-hero__panel">
      <div class="pf-hero__panel-label">操作提示</div>
      <div class="pf-hero__panel-text">加入前可先查看组长和成员名单；如果已加入小组，可以先退组，再重新参加其他小组。</div>
    </div>
  </div>

  <div class="pf-card">
    <div class="pf-card__head">
      <div class="pf-card__heading">
        <div class="pf-card__icon pf-card__icon--indigo">
          <svg width="17" height="17" fill="none" stroke="#4f46e5" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0z"/></svg>
        </div>
        <div>
          <h2 class="pf-card__title">小组申请</h2>
          <p class="pf-card__desc">查看班级已有小组，并根据组长和成员情况选择加入或退组。</p>
        </div>
      </div>
      <span class="pf-badge pf-badge--indigo">协作分组</span>
    </div>
    <div class="pf-card__body">
      <asp:Panel ID="Panelapply" runat="server">
        <div class="pf-grid-wrap">
          <asp:GridView ID="GVgroup" runat="server"
              AutoGenerateColumns="False" SkinID="GridViewInfo"
              onrowdatabound="GVgroup_RowDataBound"
              Width="100%" EnableModelValidation="True" CellPadding="0"
              DataKeyNames="Sid" onrowcommand="GVgroup_RowCommand"
              CssClass="w-full" GridLines="None" BorderWidth="0">
              <Columns>
                  <asp:BoundField HeaderText="序号" Visible="false"><ItemStyle Width="40px" /></asp:BoundField>
                  <asp:BoundField DataField="Sgtitle" HeaderText="小组名称"><ItemStyle Width="140px" HorizontalAlign="Left" /></asp:BoundField>
                  <asp:TemplateField HeaderText="组长">
                      <ItemTemplate>
                          <span class="pf-leader-chip">
                            <svg width="13" height="13" fill="#f59e0b" viewBox="0 0 20 20" style="flex-shrink:0;vertical-align:middle"><path d="M9.049 2.927c.3-.921 1.603-.921 1.902 0l1.07 3.292a1 1 0 00.95.69h3.462c.969 0 1.371 1.24.588 1.81l-2.8 2.034a1 1 0 00-.364 1.118l1.07 3.292c.3.921-.755 1.688-1.54 1.118l-2.8-2.034a1 1 0 00-1.175 0l-2.8 2.034c-.784.57-1.838-.197-1.539-1.118l1.07-3.292a1 1 0 00-.364-1.118L2.98 8.72c-.783-.57-.38-1.81.588-1.81h3.461a1 1 0 00.951-.69l1.07-3.292z"/></svg>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Sname") %>'></asp:Label>
                          </span>
                      </ItemTemplate>
                      <ItemStyle Width="110px" HorizontalAlign="Left" />
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="成员">
                      <ItemTemplate>
                          <asp:Label ID="Labelmember" runat="server"></asp:Label>
                      </ItemTemplate>
                      <ItemStyle HorizontalAlign="Left" />
                  </asp:TemplateField>
                  <asp:TemplateField HeaderText="操作" ShowHeader="False">
                      <ItemTemplate>
                          <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false"
                              CommandArgument='<%# Eval("Sid") %>' CommandName="AddGroup" Text="参加"></asp:LinkButton>
                          <asp:LinkButton ID="LinkButton2" runat="server" CausesValidation="false"
                              CommandArgument='<%# Eval("Sid") %>' CommandName="outGroup" Text="退组"></asp:LinkButton>
                      </ItemTemplate>
                      <ItemStyle Width="110px" />
                  </asp:TemplateField>
              </Columns>
              <RowStyle Height="44px" />
          </asp:GridView>
        </div>
      </asp:Panel>

      <asp:Panel ID="PanelSgtitle" runat="server">
        <div class="pf-subcard">
          <div class="pf-subcard__head">
            <div>
              <h3 class="pf-subcard__title">修改我的小组名称</h3>
              <p class="pf-subcard__desc">名称建议简短明确，方便同学识别和教师查看分组情况。</p>
            </div>
          </div>
          <div class="pf-name-row">
            <span class="pf-name-label">我的小组名称：</span>
            <asp:TextBox ID="TextBox1" runat="server" CssClass="pf-input"></asp:TextBox>
            <asp:Button ID="BtnSgtitle" runat="server" onclick="BtnSgtitle_Click" SkinID="buttonSkin" Text="修改" CssClass="pf-btn-sm" />
          </div>
        </div>
      </asp:Panel>
    </div>
  </div>

  <div class="pf-card">
    <div class="pf-card__head">
      <div class="pf-card__heading">
        <div class="pf-card__icon pf-card__icon--cyan">
          <svg width="17" height="17" fill="none" stroke="#0891b2" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z"/></svg>
        </div>
        <div>
          <h2 class="pf-card__title">未加入小组的同学</h2>
          <p class="pf-card__desc">可以先观察还未组队的同学，便于重新组织协作分工。</p>
        </div>
      </div>
      <span class="pf-badge pf-badge--cyan">待分组名单</span>
    </div>
    <div class="pf-card__body">
      <div class="pf-free-box">
        <div class="pf-free-title">当前未加入小组的成员</div>
        <asp:Label ID="Labelfree" runat="server"></asp:Label>
      </div>
    </div>
  </div>
</div>
</asp:Content>
