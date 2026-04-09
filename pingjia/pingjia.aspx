<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="pingjia.aspx.cs" Inherits="pingjia_pingjia" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <div  class="placehold">
        <div  >
           年级<asp:DropDownList ID="DDLgrade" runat="server"
                Font-Size="9pt" Width="50px"
                onselectedindexchanged="DDLgrade_SelectedIndexChanged" AutoPostBack="True"> </asp:DropDownList>
            班级<asp:DropDownList ID="DDLclass" runat="server" Font-Size="9pt"
                Width="50px" AutoPostBack="True"
                onselectedindexchanged="DDLclass_SelectedIndexChanged">
            </asp:DropDownList>&nbsp;第<asp:Label ID="Lbterm" runat="server"></asp:Label>
            学期&nbsp;
            <asp:Button   ID="BtnScoresNo" runat="server"  OnClick="BtnScoresNo_Click"
                Text="末评设置"  SkinID="BtnNormal" ToolTip="所教班级未评作品全部设置为C，即分值6" />
            &nbsp;&nbsp;
            <asp:Button   ID="BtnScores" runat="server"  OnClick="BtnScore_Click"
                Text="总分折算"  SkinID="BtnNormal" ToolTip="先统计总分，再得出折算总分" />
            &nbsp;&nbsp;
            <asp:Button ID="Btnape" runat="server"  onclick="Btnape_Click" Text="期末总评"
                SkinID="BtnNormal" />
            &nbsp;&nbsp;
            <asp:Button ID="BtnExcel" runat="server"  OnClick="BtnExcel_Click"
                Text="导出Excel"  SkinID="BtnNormal" ToolTip="将学生评价数据以Excel表格导出" />
            &nbsp;&nbsp;
            <asp:Button ID="Btntermview" runat="server"  Text="学期查询"  OnClick="Btntermview_Click"
                SkinID="BtnNormal" />
            &nbsp;&nbsp;
            <asp:Button ID="Btnback" runat="server"  Text="返回"  OnClick="Btnback_Click" SkinID="BtnNormal" />
            <br />
            <br />
            总分折算设置(各项权重,总和应为100%)==&nbsp;
            作品：<asp:DropDownList ID="DDLwork" runat="server" Font-Size="9pt" Width="50px" AutoPostBack="True" OnSelectedIndexChanged="DDLwork_SelectedIndexChanged">
                <asp:ListItem>0</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>20</asp:ListItem>
                <asp:ListItem>30</asp:ListItem>
                <asp:ListItem>40</asp:ListItem>
                <asp:ListItem Selected="True">50</asp:ListItem>
                <asp:ListItem>60</asp:ListItem>
                <asp:ListItem>70</asp:ListItem>
                <asp:ListItem>80</asp:ListItem>
                <asp:ListItem>90</asp:ListItem>
                <asp:ListItem>100</asp:ListItem>
            </asp:DropDownList>&nbsp;
            <asp:CheckBox ID="ChkWork" runat="server" Text="作品" Checked="true" AutoPostBack="True" OnCheckedChanged="Chk_CheckedChanged" />&nbsp;
            <asp:CheckBox ID="ChkGroup" runat="server" Text="小组" Checked="true" AutoPostBack="True" OnCheckedChanged="Chk_CheckedChanged" />&nbsp;
            <asp:CheckBox ID="ChkDiscuss" runat="server" Text="讨论" Checked="true" AutoPostBack="True" OnCheckedChanged="Chk_CheckedChanged" />&nbsp;
            <asp:CheckBox ID="ChkForm" runat="server" Text="表单" Checked="true" AutoPostBack="True" OnCheckedChanged="Chk_CheckedChanged" />&nbsp;
            <asp:CheckBox ID="ChkIdle" runat="server" Text="测评" Checked="true" AutoPostBack="True" OnCheckedChanged="Chk_CheckedChanged" />&nbsp;
            <asp:CheckBox ID="ChkSurvey" runat="server" Text="调查" Checked="true" AutoPostBack="True" OnCheckedChanged="Chk_CheckedChanged" />
            测验：<asp:DropDownList ID="DDLquiz" runat="server" Font-Size="9pt" Width="50px" AutoPostBack="True" OnSelectedIndexChanged="DDLquiz_SelectedIndexChanged">
                <asp:ListItem>0</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>20</asp:ListItem>
                <asp:ListItem Selected="True">30</asp:ListItem>
                <asp:ListItem>40</asp:ListItem>
                <asp:ListItem>50</asp:ListItem>
                <asp:ListItem>60</asp:ListItem>
                <asp:ListItem>70</asp:ListItem>
                <asp:ListItem>80</asp:ListItem>
                <asp:ListItem>90</asp:ListItem>
                <asp:ListItem>100</asp:ListItem>
            </asp:DropDownList>&nbsp;
            打字技能：<asp:DropDownList ID="DDLtyper" runat="server" Font-Size="9pt" Width="50px" AutoPostBack="True" OnSelectedIndexChanged="DDLtyper_SelectedIndexChanged">
                <asp:ListItem Selected="True">0</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>15</asp:ListItem>
            </asp:DropDownList>&nbsp;
            表现：<asp:DropDownList ID="DDLattitude" runat="server" Font-Size="9pt" Width="50px" AutoPostBack="True" OnSelectedIndexChanged="DDLattitude_SelectedIndexChanged">
                <asp:ListItem>0</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem Selected="True">10</asp:ListItem>
                <asp:ListItem>20</asp:ListItem>
                <asp:ListItem>30</asp:ListItem>
                <asp:ListItem>40</asp:ListItem>
                <asp:ListItem>50</asp:ListItem>
                <asp:ListItem>60</asp:ListItem>
                <asp:ListItem>70</asp:ListItem>
                <asp:ListItem>80</asp:ListItem>
                <asp:ListItem>90</asp:ListItem>
                <asp:ListItem>100</asp:ListItem>
            </asp:DropDownList>&nbsp;
            签到：<asp:DropDownList ID="DDLsignin" runat="server" Font-Size="9pt" Width="50px" AutoPostBack="True" OnSelectedIndexChanged="DDLsignin_SelectedIndexChanged">
                <asp:ListItem>0</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem Selected="True">10</asp:ListItem>
                <asp:ListItem>20</asp:ListItem>
                <asp:ListItem>30</asp:ListItem>
                <asp:ListItem>40</asp:ListItem>
                <asp:ListItem>50</asp:ListItem>
                <asp:ListItem>60</asp:ListItem>
                <asp:ListItem>70</asp:ListItem>
                <asp:ListItem>80</asp:ListItem>
                <asp:ListItem>90</asp:ListItem>
                <asp:ListItem>100</asp:ListItem>
            </asp:DropDownList>
            <asp:Label ID="LabelWeightSum" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
            <br />
            <br />
            期末总评每个班级的分数比重：优秀&gt;80%、良好&gt;60%、及格&gt;30%、不及格=0%<br />
            <asp:Label ID="Labelmsg" runat="server"  SkinID="LabelMsgRed"
                ></asp:Label>
            </div>
            <asp:GridView ID="GVStudents" runat="server" AutoGenerateColumns="False"
                 DataKeyNames="Sid"  SkinID="GVmission" OnRowDataBound="GVStudents_RowDataBound"
                PageSize="25" Width="98%" EnableModelValidation="True" >
                <Columns>
                    <asp:BoundField HeaderText="编号" />
                    <asp:BoundField DataField="Snum" HeaderText="学号" />
                    <asp:BoundField DataField="Sgradeclass" HeaderText="班级" />
                    <asp:HyperLinkField DataNavigateUrlFields="Snum"
                        DataNavigateUrlFormatString="../teacher/studentwork.aspx?snum={0}" DataTextField="Sname"
                        HeaderText="姓名" Target="_blank"  />
                    <asp:BoundField DataField="Sscore" HeaderText="作品" />
                    <asp:BoundField DataField="Sgscore" HeaderText="小组" />
                    <asp:BoundField DataField="Spscore" HeaderText="讨论" />
                    <asp:BoundField DataField="Stxtform" HeaderText="表单" />
                    <asp:BoundField DataField="Svscore" HeaderText="调查" />
                    <asp:BoundField DataField="Squiz" HeaderText="测验" />
                    <asp:BoundField DataField="Schinese" HeaderText="拼音" />
                    <asp:BoundField DataField="Sfscore" HeaderText="英语" />
                    <asp:BoundField DataField="Stscore" HeaderText="中文" />
                    <asp:BoundField DataField="Sidle" HeaderText="测评" />
                    <asp:BoundField DataField="Sattitude" HeaderText="表现" />
                    <asp:BoundField DataField="SignInCount" HeaderText="签到次数" />
                    <asp:BoundField DataField="Sallscore" HeaderText="总分" />
                    <asp:BoundField DataField="Sape" HeaderText="评定" />
                    <asp:BoundField DataField="Stenscore" HeaderText="评定" />
                </Columns>
            </asp:GridView>
            <br />
            <br />
        </div>
</asp:Content>
