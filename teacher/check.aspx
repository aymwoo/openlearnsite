<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="check.aspx.cs" Inherits="Teacher_check" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>

    <style>
        .check-page { padding: 24px; }
        .check-shell { display: flex; flex-direction: column; gap: 24px; }
        .check-hero {
            position: relative;
            overflow: hidden;
            border-radius: 24px;
            padding: 28px 32px;
            background: linear-gradient(135deg, #0f172a 0%, #2563eb 50%, #06b6d4 100%);
            color: #eff6ff;
            box-shadow: 0 20px 46px rgba(15,23,42,0.16);
        }
        .check-hero::after {
            content: "";
            position: absolute;
            right: -48px;
            top: -48px;
            width: 210px;
            height: 210px;
            border-radius: 999px;
            background: rgba(255,255,255,0.08);
        }
        .check-hero__content { position: relative; z-index: 1; }
        .check-hero__title { margin: 0; font-size: 32px; font-weight: 800; letter-spacing: 0.04em; }
        .check-hero__subtitle { margin: 10px 0 0; max-width: 760px; color: rgba(239,246,255,0.9); line-height: 1.75; }
        .check-card {
            background: #fff;
            border: 1px solid #e2e8f0;
            border-radius: 22px;
            box-shadow: 0 14px 34px rgba(15,23,42,0.06);
            overflow: hidden;
        }
        .check-card__head { padding: 22px 24px 0; }
        .check-card__title { margin: 0; font-size: 20px; font-weight: 800; color: #0f172a; }
        .check-card__desc { margin: 6px 0 0; color: #64748b; font-size: 14px; }
        .check-card__body { padding: 22px 24px 24px; }
        .check-filter-grid {
            display: grid;
            grid-template-columns: repeat(12, minmax(0, 1fr));
            gap: 14px;
        }
        .check-field {
            grid-column: span 3;
            display: flex;
            flex-direction: column;
            gap: 8px;
        }
        .check-field--wide { grid-column: span 6; }
        .check-label { font-size: 13px; color: #64748b; font-weight: 700; }
        .check-input, .check-select {
            height: 42px;
            padding: 0 12px;
            border: 1px solid #cbd5e1;
            border-radius: 12px;
            background: #fff;
            color: #0f172a;
            font-weight: 700;
        }
        .check-range {
            display: grid;
            grid-template-columns: 1fr 28px 1fr;
            gap: 8px;
            align-items: center;
        }
        .check-sep { text-align: center; color: #64748b; font-weight: 700; }
        .check-list-wrap {
            grid-column: span 12;
            padding: 16px 18px;
            border-radius: 18px;
            background: linear-gradient(180deg, #f8fbff 0%, #eff6ff 100%);
            border: 1px solid #dbeafe;
        }
        .check-list-wrap table { width: 100%; }
        .check-list-wrap td { padding: 6px 10px 6px 0; }
        .check-actions { display: flex; flex-wrap: wrap; gap: 12px; margin-top: 18px; }
        .check-btn {
            border: none;
            border-radius: 12px;
            padding: 11px 16px;
            font-weight: 700;
            cursor: pointer;
        }
        .check-btn--primary { background: #2563eb; color: #fff; }
        .check-btn--secondary { background: #f8fafc; color: #334155; border: 1px solid #e2e8f0; }
        .check-table-wrap {
            overflow-x: auto;
            border-radius: 18px;
            border: 1px solid #e2e8f0;
            background: #fff;
        }
        .check-table-wrap table {
            width: 100%;
            min-width: 1600px;
            border-collapse: separate;
            border-spacing: 0;
        }
        .check-table-wrap th {
            position: sticky;
            top: 0;
            background: #eff6ff;
            color: #1e3a8a;
            font-size: 13px;
            font-weight: 800;
            padding: 12px 10px;
            border-bottom: 1px solid #dbeafe;
            text-align: center;
        }
        .check-table-wrap td {
            padding: 11px 10px;
            border-bottom: 1px solid #eef2f7;
            background: #fff;
            color: #0f172a;
            font-size: 13px;
            text-align: center;
        }
        .check-table-wrap tr:nth-child(even) td { background: #fcfdff; }
        .sort-arrow { color: #2563eb; font-weight: bold; margin-left: 5px; }
        @media (max-width: 980px) {
            .check-field, .check-field--wide { grid-column: span 12; }
        }
        @media (max-width: 640px) {
            .check-page { padding: 16px; }
            .check-hero { padding: 22px 20px; }
            .check-hero__title { font-size: 26px; }
            .check-card__head, .check-card__body { padding-left: 18px; padding-right: 18px; }
            .check-range { grid-template-columns: 1fr; }
            .check-sep { display: none; }
        }
    </style>

    <div class="check-page">
        <div class="check-shell">
            <section class="check-hero">
                <div class="check-hero__content">
                    <h1 class="check-hero__title">课前检查记录</h1>
                    <p class="check-hero__subtitle">按班级、学生、时间范围和检查项筛选机房检查记录，支持排序、编辑、导出与批量删除。</p>
                </div>
            </section>

            <section class="check-card">
                <div class="check-card__head">
                    <h2 class="check-card__title">筛选条件</h2>
                    <p class="check-card__desc">组合班级、学生和时间范围，快速定位具体检查记录。</p>
                </div>
                <div class="check-card__body">
                    <div class="check-filter-grid">
                        <div class="check-field">
                            <span class="check-label">提交者班级</span>
                            <asp:DropDownList ID="ddlClass" runat="server" CssClass="check-select"></asp:DropDownList>
                        </div>
                        <div class="check-field">
                            <span class="check-label">使用学生</span>
                            <asp:TextBox ID="txtName" runat="server" CssClass="check-input"></asp:TextBox>
                        </div>
                        <div class="check-field check-field--wide">
                            <span class="check-label">提交时间</span>
                            <div class="check-range">
                                <asp:TextBox ID="txtStartDate" runat="server" CssClass="check-input datepicker"></asp:TextBox>
                                <span class="check-sep">至</span>
                                <asp:TextBox ID="txtEndDate" runat="server" CssClass="check-input datepicker"></asp:TextBox>
                            </div>
                        </div>
                        <div class="check-list-wrap">
                            <span class="check-label" style="display:block;margin-bottom:10px;">检查项</span>
                            <asp:CheckBoxList ID="cblChecks" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Text="有垃圾" Value="HasRubbish"></asp:ListItem>
                                <asp:ListItem Text="地面污渍" Value="DrawerClean"></asp:ListItem>
                                <asp:ListItem Text="设备未摆放" Value="EquipmentArranged"></asp:ListItem>
                                <asp:ListItem Text="椅子未归位" Value="ChairAdjusted"></asp:ListItem>
                                <asp:ListItem Text="键鼠损坏" Value="KeyboardMouseDamaged"></asp:ListItem>
                                <asp:ListItem Text="线缆拔掉" Value="CableUnplugged"></asp:ListItem>
                                <asp:ListItem Text="外设拔掉" Value="PeripheralUnplugged"></asp:ListItem>
                                <asp:ListItem Text="屏幕涂画" Value="ScreenMarked"></asp:ListItem>
                            </asp:CheckBoxList>
                        </div>
                    </div>
                    <div class="check-actions">
                        <asp:Button ID="btnFilter" runat="server" Text="筛选记录" OnClick="btnFilter_Click" CssClass="check-btn check-btn--primary" />
                        <asp:Button ID="btnReturn" runat="server" Text="返回管理页" OnClick="btnReturn_Click" ToolTip="返回管理页面" CssClass="check-btn check-btn--secondary" />
                    </div>
                </div>
            </section>

            <section class="check-card">
                <div class="check-card__head">
                    <h2 class="check-card__title">批量操作</h2>
                    <p class="check-card__desc">支持导出筛选结果、批量删除以及恢复默认排序。</p>
                </div>
                <div class="check-card__body">
                    <div class="check-actions" style="margin-top:0;">
                        <asp:Button ID="btnExport" runat="server" Text="导出 Excel" OnClick="btnExport_Click" CssClass="check-btn check-btn--primary" />
                        <asp:Button ID="btnDelete" runat="server" Text="批量删除" OnClick="btnDelete_Click" CssClass="check-btn check-btn--secondary" />
                        <asp:Button ID="btnResetSort" runat="server" Text="复原排序" OnClick="btnResetSort_Click" CssClass="check-btn check-btn--secondary" />
                    </div>
                </div>
            </section>

            <section class="check-card">
                <div class="check-card__head">
                    <h2 class="check-card__title">检查记录列表</h2>
                    <p class="check-card__desc">支持排序、分页与行内编辑修改。</p>
                </div>
                <div class="check-card__body">
                    <div class="check-table-wrap">
                        <asp:GridView ID="gvRecords" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" AllowSorting="True" OnSorting="gvRecords_Sorting" OnRowDataBound="gvRecords_RowDataBound" AllowPaging="True" PageSize="50" OnPageIndexChanging="gvRecords_PageIndexChanging" OnRowEditing="gvRecords_RowEditing" OnRowUpdating="gvRecords_RowUpdating" OnRowCancelingEdit="gvRecords_RowCancelingEdit">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkSelectAll_CheckedChanged" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="序号" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="PcName" HeaderText="设备号" ReadOnly="true" SortExpression="PcName" />
                                <asp:BoundField DataField="IpAddress" HeaderText="IP地址" ReadOnly="true" SortExpression="IpAddress" />
                                <asp:BoundField DataField="ClassName" HeaderText="提交学生班级" ReadOnly="true" SortExpression="ClassName" />
                                <asp:BoundField DataField="sname" HeaderText="提交学生" ReadOnly="true" SortExpression="sname" />
                                <asp:TemplateField HeaderText="使用学生" SortExpression="suser">
                                    <ItemTemplate><%# Eval("suser") %></ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="suser" runat="server" Text='<%# Bind("suser") %>' TextMode="MultiLine" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="有垃圾">
                                    <ItemTemplate><asp:CheckBox ID="chkHasRubbish" runat="server" Checked='<%# Eval("HasRubbish") %>' Enabled="false" /></ItemTemplate>
                                    <EditItemTemplate><asp:CheckBox ID="chkHasRubbish" runat="server" Checked='<%# Bind("HasRubbish") %>' /></EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="有污渍">
                                    <ItemTemplate><asp:CheckBox ID="chkDrawerClean" runat="server" Checked='<%# Eval("DrawerClean") %>' Enabled="false" /></ItemTemplate>
                                    <EditItemTemplate><asp:CheckBox ID="chkDrawerClean" runat="server" Checked='<%# Bind("DrawerClean") %>' /></EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="设备乱">
                                    <ItemTemplate><asp:CheckBox ID="chkEquipmentArranged" runat="server" Checked='<%# Eval("EquipmentArranged") %>' Enabled="false" /></ItemTemplate>
                                    <EditItemTemplate><asp:CheckBox ID="chkEquipmentArranged" runat="server" Checked='<%# Bind("EquipmentArranged") %>' /></EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="椅乱">
                                    <ItemTemplate><asp:CheckBox ID="chkChairAdjusted" runat="server" Checked='<%# Eval("ChairAdjusted") %>' Enabled="false" /></ItemTemplate>
                                    <EditItemTemplate><asp:CheckBox ID="chkChairAdjusted" runat="server" Checked='<%# Bind("ChairAdjusted") %>' /></EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="键鼠损">
                                    <ItemTemplate><asp:CheckBox ID="chkKeyboardMouseDamaged" runat="server" Checked='<%# Eval("KeyboardMouseDamaged") %>' Enabled="false" /></ItemTemplate>
                                    <EditItemTemplate><asp:CheckBox ID="chkKeyboardMouseDamaged" runat="server" Checked='<%# Bind("KeyboardMouseDamaged") %>' /></EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="拔线缆">
                                    <ItemTemplate><asp:CheckBox ID="chkCableUnplugged" runat="server" Checked='<%# Eval("CableUnplugged") %>' Enabled="false" /></ItemTemplate>
                                    <EditItemTemplate><asp:CheckBox ID="chkCableUnplugged" runat="server" Checked='<%# Bind("CableUnplugged") %>' /></EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="拔键鼠">
                                    <ItemTemplate><asp:CheckBox ID="chkPeripheralUnplugged" runat="server" Checked='<%# Eval("PeripheralUnplugged") %>' Enabled="false" /></ItemTemplate>
                                    <EditItemTemplate><asp:CheckBox ID="chkPeripheralUnplugged" runat="server" Checked='<%# Bind("PeripheralUnplugged") %>' /></EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="涂屏">
                                    <ItemTemplate><asp:CheckBox ID="chkScreenMarked" runat="server" Checked='<%# Eval("ScreenMarked") %>' Enabled="false" /></ItemTemplate>
                                    <EditItemTemplate><asp:CheckBox ID="chkScreenMarked" runat="server" Checked='<%# Bind("ScreenMarked") %>' /></EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="SubmitTime" HeaderText="提交时间" DataFormatString="{0:yyyy-MM-dd HH:mm}" ReadOnly="true" SortExpression="SubmitTime" />
                                <asp:TemplateField HeaderText="备注">
                                    <ItemTemplate><%# Eval("Comment") %></ItemTemplate>
                                    <EditItemTemplate><asp:TextBox ID="txtComment" runat="server" Text='<%# Bind("Comment") %>' TextMode="MultiLine" /></EditItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField ShowEditButton="True" ShowCancelButton="True" />
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" />
                        </asp:GridView>
                    </div>
                </div>
            </section>
        </div>
    </div>

    <script>
        $(function () {
            $(".datepicker").datepicker({
                dateFormat: "yy-mm-dd",
                changeMonth: true,
                changeYear: true
            });
        });
    </script>
</asp:Content>
