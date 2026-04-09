<%@ Page Title="" Language="C#" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="studentseatedit.aspx.cs" Inherits="Teacher_studentseatedit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>机号修改</title>
    <style type="text/css">
        body {
            font-family: Arial, sans-serif;
            font-size: 10pt;
            margin: 5px;
            padding: 0;
        }
        .info-box {
            background-color: #FFFACD;
            border: 1px solid #DAA520;
            padding: 8px;
            margin-bottom: 10px;
            border-radius: 5px;
        }
        .warning-box {
            background-color: #FFE4E1;
            border: 1px solid #CD5C5C;
            padding: 8px;
            margin-bottom: 10px;
            border-radius: 5px;
        }
        .student-info {
            background-color: #E8F4F8;
            border: 1px solid #87CEEB;
            padding: 8px;
            margin-bottom: 10px;
            border-radius: 5px;
        }
        .form-row {
            margin: 5px 0;
        }
        .label {
            display: inline-block;
            width: 80px;
            font-weight: bold;
        }
        .textbox {
            width: 120px;
            padding: 3px;
        }
        .button {
            padding: 6px 15px;
            margin: 3px;
            cursor: pointer;
            font-size: 10pt;
        }
        .radio-group {
            margin: 8px 0;
        }
        .radio-group label {
            margin-right: 15px;
            font-size: 10pt;
        }
        h3 {
            margin: 5px 0 10px 0;
            font-size: 14pt;
        }
        ul {
            margin: 3px 0;
            padding-left: 18px;
        }
        li {
            margin: 2px 0;
            font-size: 10pt;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h3>修改学生机号</h3>
        
        <div class="student-info">
            <div class="form-row">
                <span class="label">当前机号：</span>
                <asp:Label ID="lblCurrentSeat" runat="server" Font-Bold="true" ForeColor="Blue"></asp:Label>
            </div>
            <div class="form-row">
                <span class="label">学生姓名：</span>
                <asp:Label ID="lblSname" runat="server" Font-Bold="true"></asp:Label>
            </div>
            <div class="form-row">
                <span class="label">学号：</span>
                <asp:Label ID="lblSnum" runat="server"></asp:Label>
            </div>
            <div class="form-row">
                <span class="label">年级班级：</span>
                <asp:Label ID="lblGradeClass" runat="server"></asp:Label>
            </div>
        </div>

        <div class="radio-group">
            <asp:RadioButtonList ID="rblEditType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblEditType_SelectedIndexChanged">
                <asp:ListItem Value="permanent" Selected="True">永久修改（保存到数据库）</asp:ListItem>
                <asp:ListItem Value="temp">临时换机（1小时后自动失效）</asp:ListItem>
            </asp:RadioButtonList>
        </div>

        <div class="warning-box" id="divWarning" runat="server">
            <strong>⚠️ 注意：</strong>
            <ul style="margin: 5px 0; padding-left: 20px;">
                <li id="liPermanent" runat="server">永久修改将直接更新学生表中的机号信息</li>
                <li id="liTemp" runat="server" visible="false">临时换机仅在1小时内有效，不会保存到数据库</li>
                <li>请确保新机号在机房座位范围内</li>
            </ul>
        </div>

        <div class="info-box">
            <div class="form-row">
                <span class="label">新机号：</span>
                <asp:TextBox ID="txtNewSeat" runat="server" CssClass="textbox" BackColor="Cornsilk"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNewSeat" runat="server" 
                    ControlToValidate="txtNewSeat" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revNewSeat" runat="server"
                    ControlToValidate="txtNewSeat" ValidationExpression="^\d+$"
                    ErrorMessage="机号必须为数字" ForeColor="Red"></asp:RegularExpressionValidator>
            </div>
        </div>

        <div>
            <asp:Button ID="btnUpdate" runat="server" Text="确认修改" CssClass="button" 
                OnClick="btnUpdate_Click" BackColor="#90EE90" />
            <asp:Button ID="btnCancel" runat="server" Text="取消" CssClass="button" 
                OnClick="btnCancel_Click" BackColor="#FFB6C1" />
        </div>

        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
        
        <asp:HiddenField ID="hfSid" runat="server" />
        <asp:HiddenField ID="hfSnum" runat="server" />
        <asp:HiddenField ID="hfSgrade" runat="server" />
        <asp:HiddenField ID="hfSclass" runat="server" />
    </div>
    </form>
</body>
</html>