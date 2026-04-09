<%@ Page Language="C#" AutoEventWireup="true" CodeFile="stuworkcircle.aspx.cs" Inherits="Teacher_stuworkcircle" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta charset="utf-8" />
<title></title>

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <style type="text/css">
        .swc-nav-btn,
        .swc-tool-btn {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            min-height: 2.2rem;
            padding: 0 0.9rem;
            border: 1px solid #cbd5e1;
            border-radius: 0.6rem;
            background: #ffffff;
            color: #334155;
            font-weight: 700;
            cursor: pointer;
        }

        .swc-tool-btn {
            background: #eff6ff;
            color: #1d4ed8;
            border-color: #bfdbfe;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center; font-family: 宋体, Arial, Helvetica, sans-serif; font-size: 11pt;">    
        <br />
        学号：<asp:Label ID="LabelSnum" runat="server"></asp:Label>
&nbsp;姓名：<asp:Label ID="LabelSname" runat="server"></asp:Label>
&nbsp;作品总分：<asp:Label ID="LabelWscore" runat="server"></asp:Label>
&nbsp;
        本学期作品列表<br />
        <br />
        <div style="margin: auto; width: 98%;">
<center>
    <div style="font-family: 宋体, Arial, Helvetica, sans-serif; font-size: 9pt">
        <asp:Button ID="ImgBtnLeft" runat="server" Text="上一项"
            OnClick="ImgBtnLeft_Click" CssClass="swc-nav-btn" />
    <asp:DropDownList ID="DDLstore" runat="server" 
            Font-Bold="True" Width="300px" AutoPostBack="True" Font-Size="12pt" 
            onselectedindexchanged="DDLstore_SelectedIndexChanged">
        <asp:ListItem></asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="ImgBtnright" runat="server"
            Text="下一项" OnClick="ImgBtnright_Click" CssClass="swc-nav-btn" />
         <br />
            <asp:Label ID="lbcount" runat="server"></asp:Label>
        <asp:Button ID="ImgBtn" runat="server" Text="刷新展播"
            OnClick="ImgBtn_Click" ToolTip="循环展播专用刷新" CssClass="swc-tool-btn" />
         <br />
        </div>        
        <div style=" font-family: 宋体, Arial, Helvetica, sans-serif; font-size: 11pt; margin: 2px; " >
        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </div>
        </center>
    </div>
         <asp:Label ID="lbcurindex" runat="server" Text="0" Visible="False"></asp:Label>
        <br />
        <br />
        <br />
     <asp:Button ID="Btnclose" runat="server"   Text="关闭" BackColor="WhiteSmoke" 
            BorderColor="#CCCCCC" BorderStyle="None" Font-Size="9pt" Height="20px" 
            Width="60px"  CssClass="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" />
        <br />
        <br />
        <asp:Label ID="Labelmsg" runat="server"></asp:Label>
        <br />
        <br />
    </div>
    </form>
</body>
</html>
