<%@ Page Title="" Language="C#" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="studentnumedit.aspx.cs" Inherits="Teacher_studentnumedit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>学号修改</title>
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
        .suggested-numbers {
            margin-top: 8px;
            padding: 8px;
            background-color: #F0F8FF;
            border: 1px solid #B0C4DE;
            border-radius: 5px;
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
        <h3>修改学生学号</h3>
        
        <div class="student-info">
            <div class="form-row">
                <span class="label">当前学号：</span>
                <asp:Label ID="lblCurrentSnum" runat="server" Font-Bold="true" ForeColor="Blue"></asp:Label>
            </div>
            <div class="form-row">
                <span class="label">学生姓名：</span>
                <asp:Label ID="lblSname" runat="server" Font-Bold="true"></asp:Label>
            </div>
            <div class="form-row">
                <span class="label">年级班级：</span>
                <asp:Label ID="lblGradeClass" runat="server"></asp:Label>
            </div>
        </div>

        <div class="warning-box">
            <strong>⚠️ 注意：</strong>
            <ul style="margin: 5px 0; padding-left: 20px;">
                <li>修改学号将影响该学生的所有历史记录关联</li>
                <li>新学号不能与现有学号重复</li>
                <li>建议只将转入学生安排到转出学生的空缺学号</li>
            </ul>
        </div>

        <div class="info-box">
            <div class="form-row">
                <span class="label">新学号：</span>
                <asp:TextBox ID="txtNewSnum" runat="server" CssClass="textbox" BackColor="Cornsilk"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvNewSnum" runat="server" 
                    ControlToValidate="txtNewSnum" ErrorMessage="*" ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revNewSnum" runat="server"
                    ControlToValidate="txtNewSnum" ValidationExpression="^\d+$"
                    ErrorMessage="学号必须为数字" ForeColor="Red"></asp:RegularExpressionValidator>
            </div>
        </div>

        <div class="suggested-numbers" id="divSuggested" runat="server" visible="false">
            <strong>💡 可用的空缺学号：</strong><br />
            <asp:Label ID="lblSuggested" runat="server"></asp:Label>
        </div>

        <div>
            <asp:Button ID="btnCheck" runat="server" Text="检查学号" CssClass="button" 
                OnClick="btnCheck_Click" BackColor="#E0E0E0" />
            <asp:Button ID="btnUpdate" runat="server" Text="确认修改" CssClass="button" 
                OnClick="btnUpdate_Click" BackColor="#90EE90" Enabled="false" />
            <asp:Button ID="btnCancel" runat="server" Text="取消" CssClass="button" 
                OnClick="btnCancel_Click" BackColor="#FFB6C1" />
        </div>

        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
        
        <asp:HiddenField ID="hfSid" runat="server" />
    </div>
    </form>
</body>
</html>
