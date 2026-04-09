<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClassInfo.aspx.cs" Inherits="kcb_ClassInfo" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>班级信息管理</title>
    <style type="text/css">
        body {
            font-family: 'Microsoft YaHei', SimSun, Arial, sans-serif;
            margin: 20px;
            background-color: #f5f5f5;
        }
        .container {
            width: 95%;
            max-width: 1000px;
            margin: 0 auto;
            background: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }
        h2 {
            color: #333;
            text-align: center;
            margin-bottom: 20px;
            padding-bottom: 10px;
            border-bottom: 1px solid #eee;
        }
        .grid-view {
            width: 100%;
            margin: 20px auto;
            border-collapse: collapse;
        }
        .grid-view th {
            background-color: #9EA9B1;
            color: #111111;
            padding: 12px;
            text-align: center;
            font-size: 11pt;
            font-weight: normal;
        }
        .grid-view td {
            padding: 8px;
            border: 1px solid #ddd;
            text-align: center;
        }
        .btn {
            padding: 8px 16px;
            margin: 0 5px;
            background-color: #E6E6E6;
            color: #333333;
            border: 1px solid #D4D4D4;
            border-radius: 4px;
            cursor: pointer;
            font-family: Arial;
            font-size: 11pt;
            width: auto;
            height: 45px;
            text-align: center;
        }
        .btn:hover {
            background-color: #D4D4D4;
        }
        .message {
            color: #d9534f;
            font-weight: bold;
            margin-top: 10px;
            display: block;
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 90%; max-width: 800px; margin: 0 auto;">
            <h2 style="text-align: center">班级信息管理</h2>
            <div style="text-align: center; margin-bottom: 20px;">
                <asp:Button ID="btnAdd" runat="server" Text="添加班级" OnClick="btnAdd_Click" CssClass="btn" />
                <asp:Button ID="btnBack" runat="server" Text="返回" OnClick="btnBack_Click" CssClass="btn" style="margin-left: 10px;" />
            </div>
            <asp:GridView ID="gvCourseSchedule" runat="server" 
                AutoGenerateColumns="false" 
                DataKeyNames="ClassID" 
                CssClass="grid-view"
                OnRowEditing="gvCourseSchedule_RowEditing"
                OnRowUpdating="gvCourseSchedule_RowUpdating"
                OnRowCancelingEdit="gvCourseSchedule_RowCancelingEdit"
                OnRowDeleting="gvCourseSchedule_RowDeleting">
                <Columns>
                    <asp:TemplateField HeaderText="班级ID">
                        <ItemTemplate><%# Eval("ClassID") %></ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lblClassID" runat="server" Text='<%# Eval("ClassID") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="班级名称">
                        <ItemTemplate><%# Eval("ClassName") %></ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtClassName" runat="server" Text='<%# Bind("ClassName") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="年级班级">
                        <ItemTemplate><%# Eval("Grade") %></ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtGrade" runat="server" Text='<%# Bind("Grade") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="班级">
                        <ItemTemplate><%# Eval("Class") %></ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtClass" runat="server" Text='<%# Bind("Class") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="是否激活">
                        <ItemTemplate><%# Eval("IsActive") %></ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="chkActive" runat="server" Checked='<%# Bind("IsActive") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
                </Columns>
            </asp:GridView>
            <asp:Label ID="lblMessage" runat="server" CssClass="message" Visible="false"></asp:Label>
        </div>
    </form>
</body>
</html>
