<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="seatmanage.aspx.cs" Inherits="Teacher_seatmanage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <div class="placehold">
        <div class="chead">
            座位管理：
            <asp:DropDownList ID="DDLgrade" runat="server" Width="50px"
                EnableTheming="True" AutoPostBack="True"
                onselectedindexchanged="DDLgrade_SelectedIndexChanged">
            </asp:DropDownList>
            年级
            <asp:DropDownList ID="DDLclass" runat="server" Width="50px"
                EnableTheming="True" AutoPostBack="True"
                onselectedindexchanged="DDLclass_SelectedIndexChanged">
            </asp:DropDownList>
            班级
            <asp:DropDownList ID="DDLhouse" runat="server" Width="120px"
                EnableTheming="True" AutoPostBack="True"
                onselectedindexchanged="DDLhouse_SelectedIndexChanged">
            </asp:DropDownList>
            机房
            <asp:Button ID="BtnReturn" runat="server" Text="返回"
                SkinID="BtnNormal" onclick="BtnReturn_Click"
                ToolTip="返回教师管理首页" />
        </div>

        <div style="margin-top: 10px;">
            <fieldset style="padding: 10px; width: 95%;">
                <legend>批量操作座位</legend>
                <asp:Button ID="BtnAssignBySnum" runat="server" Text="按学号分配"
                    SkinID="BtnNormal" onclick="BtnAssignBySnum_Click"
                    ToolTip="根据学号顺序，将学生分配到机房座位（1号学生→1号机）" />
                &nbsp;
                <asp:Button ID="BtnImportFromSignin" runat="server" Text="从记录导入"
                    SkinID="BtnNormal" onclick="BtnImportFromSignin_Click"
                    ToolTip="从Signin表中导入学生最后一次登录的机号，仅导入未设置固定座位的学生" OnClientClick="return confirm('确定要从登录记录中导入机号吗？仅会更新未设置固定座位的学生。');" />
                &nbsp;
                <asp:Button ID="BtnClearAll" runat="server" Text="清空座位"
                    SkinID="BtnNormal" onclick="BtnClearAll_Click"
                    ToolTip="清空班级所有学生的固定座位，允许重新分配" OnClientClick="return confirm('确定要清空所有学生的座位吗？');" />
                &nbsp;
                <asp:Label ID="lblBatchMsg" runat="server" ForeColor="Blue" Font-Size="9pt"></asp:Label>
            </fieldset>
        </div>

        <div style="margin-top: 10px;">
            <fieldset style="padding: 10px; width: 95%;">
                <legend>查找学生</legend>
                <table>
                    <tr>
                        <td>
                            <asp:RadioButton ID="RBSearchBySnum" runat="server" Text="按学号" GroupName="SearchType" />
                            <asp:RadioButton ID="RBSearchBySeat" runat="server" Text="按座位号" GroupName="SearchType" Checked="true" />
                        </td>
                        <td>
                            <asp:TextBox ID="TBsearchKeyword" runat="server" Width="100px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="BtnSearch" runat="server" Text="查找"
                                SkinID="BtnNormal" onclick="BtnSearch_Click" />
                        </td>
                        <td>
                            <asp:Button ID="BtnClearSearch" runat="server" Text="清除查找"
                                SkinID="BtnNormal" onclick="BtnClearSearch_Click" />
                        </td>
                        <td>
                            <asp:Label ID="lblSearchMsg" runat="server" ForeColor="Blue" Font-Size="9pt"></asp:Label>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>

        <div style="margin-top: 10px;">
            <fieldset style="padding: 10px; width: 95%;">
                <legend>单独换机</legend>
                <table>
                    <tr>
                        <td>学号：</td>
                        <td>
                            <asp:TextBox ID="TBsnum" runat="server" Width="100px"></asp:TextBox>
                        </td>
                        <td>座位号：</td>
                        <td>
                            <asp:TextBox ID="TBseatNum" runat="server" Width="50px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="BtnAssignSeat" runat="server" Text="分配"
                                SkinID="BtnNormal" onclick="BtnAssignSeat_Click" />
                        </td>
                        <td>
                            <asp:Button ID="BtnClearSeat" runat="server" Text="解除绑定"
                                SkinID="BtnNormal" onclick="BtnClearSeat_Click"
                                ToolTip="清空该学生的固定座位，允许自由登录" />
                        </td>
                        <td>
                            <asp:Button ID="BtnTempAssignSeat" runat="server" Text="临时换座位（指定座位）"
                                SkinID="BtnNormal" onclick="BtnTempAssignSeat_Click"
                                ToolTip="为学生临时分配指定座位（本节课有效，下节课恢复原座位）"
                                OnClientClick="return confirm('确定要临时分配座位吗？下节课学生将恢复原座位。');" />
                        </td>
                        <td>
                            <asp:Label ID="lblSingleMsg" runat="server" ForeColor="Blue" Font-Size="9pt"></asp:Label>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>

        <div style="margin-top: 10px;">
            <asp:GridView ID="GVstudents" runat="server" AllowPaging="True"
                AutoGenerateColumns="False" PageSize="20" Width="100%"
                onpageindexchanging="GVstudents_PageIndexChanging"
                SkinID="GridViewInfo" CellPadding="5">
                <Columns>
                    <asp:BoundField HeaderText="序号" DataField="RowNum" />
                    <asp:BoundField HeaderText="学号" DataField="Snum" />
                    <asp:BoundField HeaderText="姓名" DataField="Sname" />
                    <asp:BoundField HeaderText="座位号" DataField="Sseat" NullDisplayText="未设置" />
                    <asp:TemplateField HeaderText="操作">
                        <ItemTemplate>
                            <asp:Button ID="BtnRowTempSeat" runat="server" Text="临时换座位（任意空位）"
                                SkinID="BtnNormal" CommandArgument='<%# Eval("Snum") %>'
                                OnClick="BtnRowTempSeat_Click"
                                ToolTip="允许学生在任意空位临时登录（本节课有效，下节课恢复原座位）"
                                OnClientClick='<%# "return confirm(\"确定要允许 " + Eval("Sname") + " (" + Eval("Snum") + ") 在任意空位临时登录吗？下节课将恢复原座位。\");" %>' />
                            &nbsp;
                            <asp:Button ID="BtnRowEdit" runat="server" Text="修改"
                                SkinID="BtnNormal" CommandArgument='<%# Eval("Snum") %>'
                                OnClientClick='<%# "fillStudentInfo(\"" + Eval("Snum") + "\", \"" + Eval("Sseat") + "\"); return false;" %>'
                                ToolTip="将该学生信息填充到下方单独换机输入框，方便直接修改" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <script type="text/javascript">
        // 将学生信息填充到下方单独换机输入框
        function fillStudentInfo(snum, seat) {
            var snumInput = document.getElementById('<%= TBsnum.ClientID %>');
            var seatNumInput = document.getElementById('<%= TBseatNum.ClientID %>');

            if (snumInput) {
                snumInput.value = snum;
            }

            if (seatNumInput && seat && seat !== '未设置') {
                seatNumInput.value = seat;
            } else if (seatNumInput) {
                seatNumInput.value = '';
            }

            // 聚焦到座位号输入框，方便直接输入新座位号
            if (seatNumInput) {
                seatNumInput.focus();
            }
        }

        // 页面加载完成后，为查找输入框添加回车键事件
        window.onload = function() {
            var searchInput = document.getElementById('<%= TBsearchKeyword.ClientID %>');
            if (searchInput) {
                searchInput.addEventListener('keypress', function(e) {
                    if (e.key === 'Enter') {
                        e.preventDefault();
                        // 触发查找按钮的点击事件
                        var searchBtn = document.getElementById('<%= BtnSearch.ClientID %>');
                        if (searchBtn) {
                            searchBtn.click();
                        }
                    }
                });
            }
        };
    </script>
</asp:Content>
