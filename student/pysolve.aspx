<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pysolve.aspx.cs" Inherits="Student_pysolve" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta charset="utf-8" />
<title></title>

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center">
        <asp:Label ID="Labeltitle" runat="server" Font-Bold="True" ></asp:Label>
        <br />
        <br />   
        <div style="text-align: center; margin:auto;">     
        <asp:GridView ID="GVsolve" runat="server" Font-Size="11pt" 
            HorizontalAlign="Center">
        </asp:GridView>
        </div>
    </div>
    </form>
</body>
</html>
