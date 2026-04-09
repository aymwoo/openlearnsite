<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master"   StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="gaugeitem.aspx.cs" Inherits="Teacher_gaugeitem" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">

<div>
<div class="centerdiv">
<div style=" margin: auto; width: 680px; font-size:11pt; text-align:center">
                    <div class="gaugeitem-toolbar">
                        <span class="gaugeitem-provider"><asp:Label ID="LabelProviderName" runat="server"></asp:Label></span>
                        <div style="display:flex;gap:10px;">
                            <input id="BtnAppendAI" type="button" value="追加AI生成" class="gaugeitem-ai-btn" onclick="return startGaugeRegenerate('append');" />
                            <input id="BtnRegenerateAI" type="button" value="重新用AI生成一次" class="gaugeitem-ai-btn" onclick="return startGaugeRegenerate('replace');" />
                        </div>
                    </div>
                    <asp:Panel ID="PanelAIGenerated" runat="server" Visible="false" CssClass="gauge-ai-notice">
                        <p class="gauge-ai-notice__title"><asp:Label ID="LabelAIGeneratedTitle" runat="server"></asp:Label></p>
                        <asp:Label ID="LabelAIGeneratedMsg" runat="server" CssClass="gauge-ai-notice__msg"></asp:Label>
                        <asp:BulletedList ID="BulletedListAIItems" runat="server" CssClass="gauge-ai-notice__list"></asp:BulletedList>
                    </asp:Panel>
                    <br />
                    自定义评价标准：<asp:Label ID="LabelGtitle" runat="server" Font-Bold="True"></asp:Label>
                    <br />
                    <asp:GridView ID="GVGaugeItem" runat="server"  SkinID="GridViewInfo"
                            AutoGenerateColumns="False"  DataKeyNames="Mid"  Width="100%" CellPadding="6" 
                            Font-Size="9pt"  onrowcommand="GVGaugeItem_RowCommand" 
                        EnableModelValidation="True" onrowdatabound="GVGaugeItem_RowDataBound" 
                        onrowcancelingedit="GVGaugeItem_RowCancelingEdit" 
                        onrowediting="GVGaugeItem_RowEditing" 
                        onrowupdating="GVGaugeItem_RowUpdating" >
                            <Columns>
                                <asp:TemplateField HeaderText="序号">
                                <ItemTemplate>
                                 <asp:Label ID="Label1" runat="server" Text='<%# Bind("Msort") %>'></asp:Label> 
                                </ItemTemplate>
                                    <ItemStyle Width="30px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="评价描述">
								<ItemTemplate>
                                   <asp:Label ID="LabelMitem" runat="server" Text='<%# Bind("Mitem") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBoxMitem" runat="server" Text='<%# Bind("Mitem") %>'     Font-Size="9pt"  Width="200px" Height="12px" BackColor="#FFFFCC"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Left"  Width="200px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="分值">
                                    <ItemTemplate>
                                        <asp:Label ID="LabelMscore" runat="server" Text='<%# Bind("Mscore") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TextBoxMscore" runat="server" Text='<%# Bind("Mscore") %>'     Font-Size="9pt"  Width="20px" Height="12px" BackColor="#FFFFCC"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemStyle Width="30px" />
                                </asp:TemplateField>
                                <asp:CommandField ShowEditButton="True" >
								<ItemStyle Width="70px" />
                                </asp:CommandField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="BtnDel" runat="server" CausesValidation="false" 
                                          CommandArgument='<%# Eval("Mid") %>'   CommandName="Del" Text="删除"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Width="30px" />
                                </asp:TemplateField>
                            </Columns>               
                        </asp:GridView>
                    <br />
                    <div >
                            <br />
                        评价描述：<asp:TextBox ID="TextBoxMitem" runat="server" SkinID="TextBoxNormal" 
                        Width="180px" CssClass="px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300"></asp:TextBox>
                        分值<asp:DropDownList ID="DDLscore" runat="server"  Font-Size="9pt">
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem Selected="True">2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>5</asp:ListItem>
                            <asp:ListItem>-1</asp:ListItem>
                            <asp:ListItem>-2</asp:ListItem>
                            <asp:ListItem>-3</asp:ListItem>
                            <asp:ListItem>-4</asp:ListItem>
                            <asp:ListItem>-5</asp:ListItem>
            </asp:DropDownList>
                        顺序<asp:DropDownList ID="DDLsort" runat="server"  Font-Size="9pt">
                            <asp:ListItem>1</asp:ListItem>
                            <asp:ListItem>2</asp:ListItem>
                            <asp:ListItem>3</asp:ListItem>
                            <asp:ListItem>4</asp:ListItem>
                            <asp:ListItem>5</asp:ListItem>
                            <asp:ListItem>6</asp:ListItem>
                            <asp:ListItem>7</asp:ListItem>
                            <asp:ListItem>8</asp:ListItem>
                            <asp:ListItem>9</asp:ListItem>
                            <asp:ListItem>10</asp:ListItem>
                            <asp:ListItem>11</asp:ListItem>
                            <asp:ListItem>12</asp:ListItem>
            </asp:DropDownList>
                        &nbsp;<asp:Button ID="Btnadd" runat="server"  Text="添加量规项"  onclick="Btnadd_Click"
                            CssClass="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" />
                    &nbsp;<asp:Button ID="Btnreturn" runat="server"  Text="返回列表"  onclick="Btnreturn_Click"
                            Width="60px"  CssClass="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" />
                    <br />
                    </div>
                         </div>
                         
                         </div>
    <div id="gaugeItemLoading" class="gauge-ai-loading" aria-live="polite" aria-busy="true">
        <div class="gauge-ai-loading__card">
            <div class="gauge-ai-loading__spinner"></div>
            <p class="gauge-ai-loading__title">正在重新生成量规项</p>
            <p id="gaugeItemLoadingDesc" class="gauge-ai-loading__desc">系统正在读取当前量规并调用 AI 重新生成，请稍候。</p>
            <ul id="gaugeItemLoadingSteps" class="gauge-ai-loading__steps">
                <li class="gauge-ai-loading__step is-active"><span class="gauge-ai-loading__step-index">1</span><span>正在读取当前量规</span></li>
                <li class="gauge-ai-loading__step"><span class="gauge-ai-loading__step-index">2</span><span>正在调用 AI 生成评价项</span></li>
                <li class="gauge-ai-loading__step"><span class="gauge-ai-loading__step-index">3</span><span>正在覆盖旧量规项并写入新内容</span></li>
            </ul>
        </div>
    </div>
    <br />
<br />
</div>


    <script type="text/javascript">
        window.__gaugeitemConfig = {
            gauge_generateUrl: '<%= ResolveUrl("~/teacher/gauge_generate.ashx") %>',
            request_QueryString_gid: '<%= Request.QueryString["gid"] %>'
        };
    </script>
    <script type="text/javascript" src="../js/gaugeitem.js"></script>
</asp:Content>
