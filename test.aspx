<%@ Page Language="C#" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div>    
        asp.net works ok !&nbsp; 工作正常！<br />
        <br />
        此服务器日期格式是否正常：<br />
        <br />
        <asp:Label ID="Label1" runat="server"></asp:Label>
    </div>
    <br />
    <br />
    <div>远程出错详细信息是否正常 
        <asp:Button ID="Button1" runat="server" BackColor="#CCCCCC" 
            BorderColor="Silver" BorderStyle="None" Font-Size="9pt" onclick="Button1_Click" 
            Text="测试专用"  CssClass="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" />
    </div>
    </form>
</body>
</html>
