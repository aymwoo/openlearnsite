<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="computers.aspx.cs" Inherits="Teacher_computers" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../js/fileupload.css" rel="stylesheet" />
    

    <div class="comp-page">
        <div class="comp-shell">
            <div class="comp-hero">
                <h1 class="comp-hero__title">计算机管理</h1>
                <p class="comp-hero__subtitle">管理电脑室IP地址、主机名绑定状态和学号分配。</p>
            </div>

            <div class="comp-grid">
                <div class="comp-card comp-card--span-12 comp-card--table">
                    <div class="comp-card__head">
                        <h2 class="comp-card__title">IP地址列表</h2>
                    </div>
                    <div class="comp-card__body">
                        <div class="comp-sort">
                            <span class="comp-sort-label">排序方式</span>
                            <asp:RadioButtonList ID="Radiobtnorder" runat="server" AutoPostBack="True"
                                onselectedindexchanged="Radiobtnorder_SelectedIndexChanged"
                                RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Selected="True" Value="1">IP地址</asp:ListItem>
                                <asp:ListItem Value="2">计算机名</asp:ListItem>
                                <asp:ListItem Value="3">日期</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>

                        <div class="comp-table-wrap">
                            <asp:GridView ID="GVComputer" runat="server"
                                AutoGenerateColumns="False" CellPadding="0" GridLines="None"
                                PageSize="20" Width="100%" EnableModelValidation="True"
                                onrowcommand="GVComputer_RowCommand"
                                onrowdatabound="GVComputer_RowDataBound" DataKeyNames="Pid">
                                <Columns>
                                    <asp:BoundField HeaderText="序号">
                                        <ItemStyle CssClass="comp-cell--seq" />
                                    </asp:BoundField>
                                    <asp:HyperLinkField DataNavigateUrlFields="Pip"
                                        DataNavigateUrlFormatString="ipstudent.aspx?qip={0}" DataTextField="Pip"
                                        HeaderText="IP地址" Target="_blank">
                                        <ItemStyle CssClass="comp-cell--ip" />
                                    </asp:HyperLinkField>
                                    <asp:TemplateField HeaderText="计算机名">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Pmachine") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="电脑室">
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Pm") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Pnum" HeaderText="分配学号" />
                                    <asp:BoundField DataField="Pon" HeaderText="是否登录" Visible="False" />
                                    <asp:CheckBoxField DataField="Plock" HeaderText="绑定状态">
                                        <ItemStyle CssClass="comp-cell--lock" />
                                    </asp:CheckBoxField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:Button ID="ImageButton1" runat="server" CausesValidation="false"
                                                CommandArgument='<%# Eval("Pid") %>' CommandName="Lock"
                                                Text="锁定" ToolTip="更新锁定状态" CssClass="comp-lock-btn" />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="comp-cell--lock-btn" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Pdate" HeaderText="更新日期">
                                        <ItemStyle CssClass="comp-cell--date" />
                                    </asp:BoundField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="false"
                                                CommandArgument='<%# Eval("Pid") %>' CommandName="Del" Text="删除" ToolTip="删除该条记录"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="comp-cell--del" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle CssClass="" />
                                <RowStyle CssClass="" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>

                <div class="comp-card comp-card--span-8 comp-card--actions">
                    <div class="comp-card__head">
                        <h2 class="comp-card__title">批量操作</h2>
                    </div>
                    <div class="comp-card__body">
                        <div class="comp-actions">
                            <asp:Button ID="BtnDelAll" runat="server" onclick="BtnDelAll_Click"
                                Text="全体删除" CssClass="comp-btn comp-btn--danger" />
                            <asp:Button ID="BtnUnlock" runat="server" onclick="BtnUnlock_Click"
                                Text="全体解绑" CssClass="comp-btn comp-btn--secondary" />
                            <asp:Button ID="BtnOnlock" runat="server"
                                Text="全体绑定" onclick="BtnOnlock_Click" CssClass="comp-btn comp-btn--success" />
                            <asp:Button ID="BtnAssign" runat="server"
                                Text="自动分配" onclick="BtnAssign_Click"
                                ToolTip="培训时用，先获取所有学生机IP，然后点自动分配学号" Visible="False" CssClass="comp-btn comp-btn--primary" />
                            <asp:Button ID="BtnClear" runat="server" Text="清除分配"
                                ToolTip="清除分配的学号" onclick="BtnClear_Click" Visible="False" CssClass="comp-btn comp-btn--secondary" />
                            <asp:Button ID="BtnRefresh" runat="server"
                                Text="刷新" onclick="BtnRefresh_Click" CssClass="comp-btn comp-btn--primary" />
                        </div>
                        <div class="comp-hint">解除绑定后，学生登录更新记录就会自动绑定。</div>
                        <div style="margin-top: 10px;">
                            <span class="comp-check-group">
                                <asp:CheckBox ID="CheckBoxhostname" runat="server" AutoPostBack="True"
                                    oncheckedchanged="CheckBoxhostname_CheckedChanged"
                                    Text="自动获取主机名" ToolTip="同网段获取正常，如果跨网段请关闭并导入主机名和IP绑定表格" />
                            </span>
                        </div>
                    </div>
                </div>

                <div class="comp-card comp-card--span-4 comp-card--import">
                    <div class="comp-card__head">
                        <h2 class="comp-card__title">导入主机名</h2>
                    </div>
                    <div class="comp-card__body">
                        <div class="comp-import-form">
                            <div class="comp-import-row">
                                <div class="ls-upload" data-accept=".xls,.xlsx" data-label="点击或拖拽上传Excel" data-hint="支持 xls / xlsx 格式">
                                    <asp:FileUpload ID="FuHostnameIp" runat="server" />
                                </div>
                                <asp:Button ID="BtnImport" runat="server" onclick="BtnImport_Click"
                                    Text="导入Excel" CssClass="comp-btn comp-btn--primary" />
                            </div>
                            <asp:Label ID="Labelmsg" runat="server" CssClass="comp-msg"></asp:Label>

                            <div class="comp-sample">
                                <div class="comp-sample__label">Excel格式参考</div>
                                <table class="comp-sample-table">
                                    <tr><th>ip</th><th>hostname</th></tr>
                                    <tr><td>192.168.0.20</td><td>pc1</td></tr>
                                    <tr><td>192.168.0.21</td><td>pc2</td></tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="../js/fileupload.js"></script>
</asp:Content>
