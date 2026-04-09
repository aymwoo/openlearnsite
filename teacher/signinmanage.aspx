<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="signinmanage.aspx.cs" Inherits="Teacher_signinmanage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <style>
        .signin-page { padding: 24px; }
        .signin-shell { display: flex; flex-direction: column; gap: 24px; }
        .signin-hero {
            position: relative;
            overflow: hidden;
            border-radius: 24px;
            padding: 28px 32px;
            background: linear-gradient(135deg, #0f172a 0%, #7c3aed 52%, #ec4899 100%);
            color: #fdf2f8;
            box-shadow: 0 20px 46px rgba(15,23,42,0.16);
        }
        .signin-hero::after {
            content: "";
            position: absolute;
            right: -48px;
            top: -48px;
            width: 210px;
            height: 210px;
            border-radius: 999px;
            background: rgba(255,255,255,0.08);
        }
        .signin-hero__content {
            position: relative;
            z-index: 1;
            display: flex;
            justify-content: space-between;
            gap: 20px;
            flex-wrap: wrap;
            align-items: flex-start;
        }
        .signin-hero__title { margin: 0; font-size: 32px; font-weight: 800; }
        .signin-hero__subtitle { margin: 10px 0 0; max-width: 760px; color: rgba(253,242,248,0.9); line-height: 1.75; }
        .signin-selectors {
            display: flex;
            gap: 10px;
            flex-wrap: wrap;
            align-items: center;
            padding: 10px 14px;
            border-radius: 14px;
            background: rgba(15,23,42,0.22);
            border: 1px solid rgba(255,255,255,0.16);
        }
        .signin-select, .signin-btn {
            height: 40px;
            border-radius: 12px;
            font-weight: 700;
        }
        .signin-select { padding: 0 12px; border: none; color: #0f172a; }
        .signin-btn {
            padding: 0 14px;
            border: 1px solid rgba(255,255,255,0.18);
            background: rgba(255,255,255,0.12);
            color: #fff;
        }
        .signin-card {
            background: #fff;
            border: 1px solid #e2e8f0;
            border-radius: 22px;
            box-shadow: 0 14px 34px rgba(15,23,42,0.06);
            overflow: hidden;
        }
        .signin-empty {
            text-align: center;
            padding: 48px 20px;
            color: #64748b;
            font-size: 16px;
        }
        .signin-empty__icon { font-size: 52px; margin-bottom: 12px; }
        .signin-panels {
            display: grid;
            grid-template-columns: repeat(2, minmax(0, 1fr));
            gap: 20px;
        }
        .signin-panel {
            background: #fff;
            border: 1px solid #e2e8f0;
            border-radius: 22px;
            overflow: hidden;
            box-shadow: 0 14px 34px rgba(15,23,42,0.06);
        }
        .signin-panel__head { padding: 18px 20px; color: #fff; font-size: 18px; font-weight: 800; }
        .signin-panel--add .signin-panel__head { background: linear-gradient(135deg, #059669 0%, #22c55e 100%); }
        .signin-panel--sub .signin-panel__head { background: linear-gradient(135deg, #dc2626 0%, #f97316 100%); }
        .signin-score {
            text-align: center;
            padding: 20px;
            background: #f8fafc;
            border-bottom: 1px solid #e2e8f0;
        }
        .signin-score__num { font-size: 72px; font-weight: 800; line-height: 1; }
        .signin-score__num--add { color: #059669; }
        .signin-score__num--sub { color: #dc2626; }
        .signin-score__meta { margin-top: 8px; color: #64748b; font-size: 13px; }
        .signin-panel__body { padding: 20px; }
        .signin-checks table { width: 100%; }
        .signin-checks td { padding: 6px 10px 6px 0; }
        .signin-footer {
            display: flex;
            justify-content: space-between;
            align-items: center;
            gap: 12px;
            flex-wrap: wrap;
            margin-top: 16px;
            padding-top: 14px;
            border-top: 1px solid #e2e8f0;
        }
        .signin-msg { font-size: 13px; font-weight: 700; }
        .signin-btn--action {
            border: none;
            border-radius: 12px;
            padding: 11px 16px;
            font-weight: 700;
            color: #fff;
        }
        .signin-btn--add { background: #059669; }
        .signin-btn--sub { background: #dc2626; }
        .signin-alert {
            display: flex;
            justify-content: space-between;
            align-items: center;
            gap: 16px;
            flex-wrap: wrap;
            padding: 18px 20px;
            border-radius: 20px;
            background: linear-gradient(135deg, #fff7ed 0%, #ffedd5 100%);
            border: 1px solid #fdba74;
        }
        .signin-alert__title { font-size: 16px; font-weight: 800; color: #9a3412; }
        .signin-alert__desc { margin-top: 4px; color: #c2410c; font-size: 13px; }
        .signin-alert__btn {
            border: none;
            border-radius: 12px;
            padding: 11px 16px;
            background: #ea580c;
            color: #fff;
            font-weight: 700;
        }
        .signin-table-card { background: #fff; border: 1px solid #e2e8f0; border-radius: 22px; box-shadow: 0 14px 34px rgba(15,23,42,0.06); }
        .signin-table-card__head { display:flex; justify-content:space-between; align-items:center; gap:16px; padding:22px 24px 0; flex-wrap:wrap; }
        .signin-table-card__title { margin:0; font-size:20px; font-weight:800; color:#0f172a; }
        .signin-table-card__desc { margin:6px 0 0; color:#64748b; font-size:14px; }
        .signin-table-card__body { padding:22px 24px 24px; }
        .signin-table-wrap { overflow-x:auto; border-radius:18px; border:1px solid #e2e8f0; }
        .signin-table-wrap table { width:100%; min-width:1000px; border-collapse:separate; border-spacing:0; }
        .signin-table-wrap th { position:sticky; top:0; background:#eff6ff; color:#1e3a8a; font-size:13px; font-weight:800; padding:12px 10px; border-bottom:1px solid #dbeafe; text-align:center; }
        .signin-table-wrap td { padding:11px 10px; border-bottom:1px solid #eef2f7; text-align:center; color:#0f172a; font-size:13px; background:#fff; }
        .signin-table-wrap tr:nth-child(even) td { background:#fcfdff; }
        @media (max-width: 980px) { .signin-panels { grid-template-columns: 1fr; } }
        @media (max-width: 640px) {
            .signin-page { padding:16px; }
            .signin-hero { padding:22px 20px; }
            .signin-hero__title { font-size:26px; }
            .signin-table-card__head, .signin-table-card__body { padding-left:18px; padding-right:18px; }
        }
    </style>

    <script type="text/javascript">
        function updateAddScore() {
            var checkboxes = document.getElementById('<%= CBLAddReason.ClientID %>').getElementsByTagName('input');
            var count = 0;
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].type == 'checkbox' && checkboxes[i].checked) {
                    count++;
                }
            }
            var totalScore = count * 5;
            document.getElementById('<%= lblAddTotal.ClientID %>').innerText = "+" + totalScore;
            document.getElementById('<%= lblAddCount.ClientID %>').innerText = "选中 " + count + " 项，每项5分";
        }

        function updateSubScore() {
            var checkboxes = document.getElementById('<%= CBLSubReason.ClientID %>').getElementsByTagName('input');
            var count = 0;
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].type == 'checkbox' && checkboxes[i].checked) {
                    count++;
                }
            }
            var totalScore = count * 5;
            document.getElementById('<%= lblSubTotal.ClientID %>').innerText = "-" + totalScore;
            document.getElementById('<%= lblSubCount.ClientID %>').innerText = "选中 " + count + " 项，每项5分";
        }
    </script>

    <div class="signin-page">
        <div class="signin-shell">
            <section class="signin-hero">
                <div class="signin-hero__content">
                    <div>
                        <h1 class="signin-hero__title">签到表现评价</h1>
                        <p class="signin-hero__subtitle">对当前上课班级学生进行批量加分、扣分和未签到扣分处理，并同步查看当天签到名单。</p>
                    </div>
                    <div class="signin-selectors">
                        <asp:DropDownList ID="DDLgrade" runat="server" AutoPostBack="True" onselectedindexchanged="DDLgrade_SelectedIndexChanged" CssClass="signin-select"></asp:DropDownList>
                        <span>年级</span>
                        <asp:DropDownList ID="DDLclass" runat="server" AutoPostBack="True" onselectedindexchanged="DDLclass_SelectedIndexChanged" CssClass="signin-select"></asp:DropDownList>
                        <span>班级</span>
                        <asp:Label ID="Lbterm" runat="server"></asp:Label>
                        <asp:Button ID="BtnReturn" runat="server" Text="返回" CssClass="signin-btn" onclick="BtnReturn_Click" ToolTip="返回教师管理首页" />
                    </div>
                </div>
            </section>

            <asp:Panel ID="PanelNoClass" runat="server" Visible="false">
                <div class="signin-card signin-empty">
                    <div class="signin-empty__icon">📚</div>
                    <div>当前没有正在上课的班级，请先在“开始上课”页面选择班级开始上课。</div>
                </div>
            </asp:Panel>

            <asp:Panel ID="PanelContent" runat="server" Visible="true">
                <div class="signin-panels">
                    <section class="signin-panel signin-panel--add">
                        <div class="signin-panel__head">批量加分（每项 5 分）</div>
                        <div class="signin-score">
                            <div class="signin-score__num signin-score__num--add"><asp:Label ID="lblAddTotal" runat="server">+0</asp:Label></div>
                            <div class="signin-score__meta"><asp:Label ID="lblAddCount" runat="server">选中 0 项，每项5分</asp:Label></div>
                        </div>
                        <div class="signin-panel__body">
                            <div class="signin-checks">
                                <asp:CheckBoxList ID="CBLAddReason" runat="server" RepeatLayout="Flow" onclick="updateAddScore()">
                                    <asp:ListItem Value="路队有序">路队有序</asp:ListItem>
                                    <asp:ListItem Value="按时签到">按时签到</asp:ListItem>
                                    <asp:ListItem Value="认真学习">认真学习</asp:ListItem>
                                    <asp:ListItem Value="爱护公物">爱护公物</asp:ListItem>
                                    <asp:ListItem Value="保持卫生">保持卫生</asp:ListItem>
                                    <asp:ListItem Value="遵守纪律">遵守纪律</asp:ListItem>
                                </asp:CheckBoxList>
                            </div>
                            <div class="signin-footer">
                                <asp:Label ID="lblAddMsg" runat="server" CssClass="signin-msg" ForeColor="#059669"></asp:Label>
                                <asp:Button ID="BtnAddScore" runat="server" Text="确定加分" CssClass="signin-btn--action signin-btn--add" onclick="BtnAddScore_Click" />
                            </div>
                        </div>
                    </section>

                    <section class="signin-panel signin-panel--sub">
                        <div class="signin-panel__head">批量扣分（每项 5 分）</div>
                        <div class="signin-score">
                            <div class="signin-score__num signin-score__num--sub"><asp:Label ID="lblSubTotal" runat="server">-0</asp:Label></div>
                            <div class="signin-score__meta"><asp:Label ID="lblSubCount" runat="server">选中 0 项，每项5分</asp:Label></div>
                        </div>
                        <div class="signin-panel__body">
                            <div class="signin-checks">
                                <asp:CheckBoxList ID="CBLSubReason" runat="server" RepeatLayout="Flow" onclick="updateSubScore()">
                                    <asp:ListItem Value="迟到">迟到</asp:ListItem>
                                    <asp:ListItem Value="早退">早退</asp:ListItem>
                                    <asp:ListItem Value="无故缺席">无故缺席</asp:ListItem>
                                    <asp:ListItem Value="课堂讲话">课堂讲话</asp:ListItem>
                                    <asp:ListItem Value="影响秩序">影响秩序</asp:ListItem>
                                    <asp:ListItem Value="卫生欠佳">卫生欠佳</asp:ListItem>
                                </asp:CheckBoxList>
                            </div>
                            <div class="signin-footer">
                                <asp:Label ID="lblSubMsg" runat="server" CssClass="signin-msg" ForeColor="#dc2626"></asp:Label>
                                <asp:Button ID="BtnSubScore" runat="server" Text="确定扣分" CssClass="signin-btn--action signin-btn--sub" onclick="BtnSubScore_Click" />
                            </div>
                        </div>
                    </section>
                </div>

                <div class="signin-alert">
                    <div>
                        <div class="signin-alert__title">未签到统一扣分</div>
                        <div class="signin-alert__desc">对当前班级今天未签到学生统一扣除 30 分，并记录“未签到扣分”。</div>
                    </div>
                    <div style="display:flex;align-items:center;gap:12px;flex-wrap:wrap;">
                        <asp:Label ID="lblUnSignMsg" runat="server" CssClass="signin-msg" ForeColor="#c2410c"></asp:Label>
                        <asp:Button ID="BtnUnSignSub" runat="server" Text="执行未签到扣分" CssClass="signin-alert__btn" onclick="BtnUnSignSub_Click" />
                    </div>
                </div>

                <section class="signin-table-card">
                    <div class="signin-table-card__head">
                        <div>
                            <h2 class="signin-table-card__title">今日签到列表</h2>
                            <p class="signin-table-card__desc">勾选学生后可执行上方批量加分或扣分操作。</p>
                        </div>
                        <div style="display:flex;align-items:center;gap:8px;color:#64748b;font-size:13px;font-weight:700;">
                            <asp:CheckBox ID="CBSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="CBSelectAll_CheckedChanged" />
                            全选本页
                        </div>
                    </div>
                    <div class="signin-table-card__body">
                        <div class="signin-table-wrap">
                            <asp:GridView ID="GVSignin" runat="server" AutoGenerateColumns="False" Width="100%" AllowPaging="True" PageSize="30" DataKeyNames="Qid" onpageindexchanging="GVSignin_PageIndexChanging">
                                <Columns>
                                    <asp:TemplateField HeaderText="选择">
                                        <ItemTemplate><asp:CheckBox ID="CBSelect" runat="server" /></ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="序号" DataField="RowNum" />
                                    <asp:BoundField HeaderText="学号" DataField="Snum" />
                                    <asp:BoundField HeaderText="姓名" DataField="Sname" />
                                    <asp:BoundField HeaderText="座位" DataField="Sseat" />
                                    <asp:BoundField HeaderText="签到时间" DataField="Qtime" />
                                    <asp:BoundField HeaderText="表现分" DataField="Qattitude" />
                                    <asp:BoundField HeaderText="加分原因" DataField="Qgood" />
                                    <asp:BoundField HeaderText="扣分原因" DataField="Qbad" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </section>
            </asp:Panel>
        </div>
    </div>
</asp:Content>
