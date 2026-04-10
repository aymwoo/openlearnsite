<%@ Page Title="" Language="C#" EnableEventValidation = "false" StylesheetTheme="Student" AutoEventWireup="true"
    CodeFile="mytotal.aspx.cs" Inherits="Student_mytotal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
<title></title>
<style type="text/css">
    .compact-table {
        width: 100%;
        table-layout: auto;
        border-collapse: collapse;
    }
    .compact-table td {
        white-space: normal;
        word-wrap: break-word;
        word-break: break-all;
        padding: 8px 12px;
        text-align: center;
        min-width: 80px;
        max-width: 200px;
    }
    .compact-table td:nth-child(2) {
        min-width: 200px;
        max-width: 400px;
        text-align: left;
        padding-left: 15px;
    }
    .compact-table th {
        white-space: normal;
        word-wrap: break-word;
        word-break: break-all;
        padding: 10px 15px;
        font-size: 12px;
        text-align: center;
        font-weight: bold;
        min-width: 80px;
        max-width: 200px;
    }
    .compact-table th:nth-child(2) {
        min-width: 200px;
        max-width: 400px;
        text-align: left;
        padding-left: 15px;
    }
    .compact-table td:hover {
        background-color: #f5f5f5;
        position: relative;
    }
    .compact-table td:hover::after {
        content: attr(data-fulltext);
        position: absolute;
        left: 0;
        top: 0;
        width: 100%;
        background-color: #fff;
        padding: 8px 12px;
        box-shadow: 0 2px 8px rgba(0,0,0,0.15);
        z-index: 1000;
        white-space: normal;
        word-wrap: break-word;
        text-align: left;
        min-width: 200px;
        max-width: 500px;
    }
</style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="text-align: center;">
            <asp:Label ID="LabelTitle" runat="server" Font-Bold="True" Font-Size="14pt" Text="我的学习汇总"></asp:Label>
            <br />
            <br />
            <div style="margin: auto; text-align: center">
            <center>
                <asp:GridView ID="GridViewMyTotal" runat="server"
                    CellPadding="0" CellSpacing="0" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None"
                    BorderWidth="1px" Font-Names="Arial" HorizontalAlign="Center"
                    EnableModelValidation="True" Width="25%" AutoGenerateColumns="True">
                    <RowStyle ForeColor="#000066" />
                    <HeaderStyle BackColor="#305E9C" Font-Bold="True" ForeColor="White" />
                </asp:GridView>
                <br />
                <asp:Label ID="Labelmsg" runat="server" ForeColor="#666666"></asp:Label>
                <br />
                <br />
            </center>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
