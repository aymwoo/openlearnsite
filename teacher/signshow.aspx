<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="signshow.aspx.cs" Inherits="Teacher_signshow" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">  
    <div   class="placehold">  
        <div  class="cline"> </div>
<asp:RadioButtonList ID="RBtnList" runat="server" AutoPostBack="True" 
            onselectedindexchanged="RBtnList_SelectedIndexChanged" 
            RepeatDirection="Horizontal" RepeatLayout="Flow">
            <asp:ListItem Selected="True" Value="0">学号排序</asp:ListItem>
            <asp:ListItem Value="1">机号排序</asp:ListItem>
        </asp:RadioButtonList>

    <%-- 在 ButtonReturn 按钮前添加导出按钮 --%>
    <asp:Button ID="BtnExportCurrent" runat="server" Text="导出签到记录"  Width="150px" 
        onclick="BtnExportCurrent_Click" SkinID="BtnNormal" />
    &nbsp;&nbsp;
    <asp:Button ID="BtnExportAll" runat="server" Text="导出全部签到记录"   Width="150px"
        onclick="BtnExportAll_Click" SkinID="BtnNormal" />
    &nbsp;&nbsp;
    <asp:Button ID="BtnExportRegister" runat="server" Text="导出登记册"  Width="150px" 
        onclick="BtnExportRegister_Click" SkinID="BtnNormal" />
    &nbsp;&nbsp;
    <asp:Button ID="BtnExportAllRegister" runat="server" Text="导出全部登记册"   Width="150px"
        onclick="BtnExportAllRegister_Click" SkinID="BtnNormal" />
    &nbsp;&nbsp;
    <asp:Button ID="ButtonReturnTop" runat="server" onclick="ButtonReturn_Click" Text="返回" SkinID="BtnNormal" />

    <br />

        已签到列表：<asp:Label ID="Labelsignin" runat="server" Font-Size="9pt"></asp:Label>
        <br />
        <div class="centerdiv">
        <asp:GridView ID="GVSignin" runat="server" AutoGenerateColumns="False"            
            CellPadding="2" PageSize="15"
            Width="100%" ToolTip="已签到的记录"  SkinID="GridViewInfo"
            onrowdatabound="GVSignin_RowDataBound" EnableModelValidation="True">
            <FooterStyle BackColor="White" ForeColor="#333333" />
            <Columns>
                <asp:BoundField />
                <asp:BoundField DataField="Qnum" HeaderText="学号" />
                <asp:BoundField DataField="Qgrade" HeaderText="年级" />
                <asp:BoundField DataField="Qclass" HeaderText="班级" />
                <asp:BoundField DataField="Qname" HeaderText="姓名" >
                <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Qwork" HeaderText="作品" />
                <asp:BoundField DataField="Qattitude" HeaderText="表现" />
                <asp:BoundField DataField="Qnote" HeaderText="备注" />
                <asp:BoundField DataField="Qgroup" HeaderText="组评" />
                <asp:BoundField DataField="Qgscore" HeaderText="分值" />
                <asp:BoundField DataField="Qmachine" HeaderText="机号" />
                <asp:BoundField DataField="Qterm" HeaderText="学期" />
                <asp:BoundField DataField="Qyear" HeaderText="年" />
                <asp:BoundField DataField="Qmonth" HeaderText="月" />
                <asp:BoundField DataField="Qday" HeaderText="日" />
                <asp:TemplateField HeaderText="星期">
                    <ItemTemplate>
                        <asp:Literal ID="LiteralWeek" runat="server" Text='<%# ConvertWeekToChinese(Eval("Qweek").ToString()) %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Qsession" HeaderText="节次" />
                <asp:BoundField DataField="Qtitle" HeaderText="上课内容" />
                <asp:TemplateField HeaderText="上课教师">
                    <ItemTemplate>
                        <asp:Literal ID="LiteralTeacher" runat="server" Text='<%# GetCurrentTeacherName() %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Qdate" HeaderText="日期" />
            </Columns>
        </asp:GridView>
        <div  class="cline"></div>
            未签到列表：<asp:Label ID="Labelnosign" runat="server" Font-Size="9pt"></asp:Label>
        <br />
        <asp:GridView ID="GVNoSign" runat="server" AutoGenerateColumns="False"
           CellPadding="2"  Width="100%"  ToolTip="未签到列表"  SkinID="GridViewInfo"
            onrowdatabound="GVNoSign_RowDataBound" DataKeyNames="Snum">
            <Columns>
                <asp:BoundField />
                <asp:BoundField DataField="Snum" HeaderText="学号" />
                <asp:BoundField DataField="Sgrade" HeaderText="年级" />
                <asp:BoundField DataField="Sclass" HeaderText="班级" />
                <asp:BoundField DataField="Sname" HeaderText="姓名" >
                <ItemStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField DataField="Sattitude" HeaderText="表现" />
                <asp:BoundField DataField="Sheadtheacher" HeaderText="班主任" />
                <asp:BoundField DataField="Sparents" HeaderText="父母" />
                <asp:BoundField DataField="Saddress" HeaderText="家庭地址" />
                <asp:BoundField DataField="Sphone" HeaderText="联系电话" />
                <asp:BoundField HeaderText="缺席原因" />
            </Columns>
        </asp:GridView>
        </div>
        <asp:RadioButtonList ID="RBtnList2" runat="server" AutoPostBack="True" 
            onselectedindexchanged="RBtnList2_SelectedIndexChanged" 
            RepeatDirection="Horizontal" RepeatLayout="Flow">
            <asp:ListItem Selected="True" Value="0">学号排序</asp:ListItem>
            <asp:ListItem Value="1">机号排序</asp:ListItem>
        </asp:RadioButtonList>
        <br />
    <br />
    <asp:Button ID="ButtonReturnBottom" runat="server" onclick="ButtonReturn_Click" Text="返回" SkinID="BtnNormal" />
        <br />
    <br />
</div> 
</asp:Content>
