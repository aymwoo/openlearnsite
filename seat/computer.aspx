<%@ Page Language="C#" ResponseEncoding="utf-8" AutoEventWireup="true" CodeFile="computer.aspx.cs" Inherits="Seat_computer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>机房电脑布置图</title>
    <script src="../js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.24.custom.min.js" type="text/javascript"></script>
    <script src="../js/jquery.cookie.js" type="text/javascript"></script>
    <link href="../js/computer.css" rel="stylesheet" type="text/css" />
    <script src="../js/seatsave.js" type="text/javascript"></script>
    

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div id="houserfloor" class="floor">
        <div id="computerhouse" class="house">
            <div class="menu">
                &nbsp;<a onclick="$(vturn);return false" href="#" title="垂直翻转所有电脑位置">垂直翻转</a>&nbsp;
                &nbsp;<a onclick="$(hturn);return false" href="#" title="水平翻转所有电脑位置">水平翻转</a>&nbsp;
                选择列数
                <asp:DropDownList runat="server" ID="ddll" Width="40px">
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                    <asp:ListItem Selected="True">6</asp:ListItem>
                    <asp:ListItem>7</asp:ListItem>
                    <asp:ListItem>8</asp:ListItem>
                    <asp:ListItem>9</asp:ListItem>
                    <asp:ListItem>10</asp:ListItem>
                    <asp:ListItem>11</asp:ListItem>
                    <asp:ListItem>12</asp:ListItem>
                </asp:DropDownList>
                电脑总数
                <asp:TextBox ID="TextBoxall" runat="server" Width="30px" Wrap="False" CssClass="px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300">30</asp:TextBox>
                <asp:RadioButtonList ID="RadioBtnSelect" runat="server" RepeatDirection="Horizontal"
                    RepeatLayout="Flow" ToolTip="电脑编号次序按纵向或横向">
                    <asp:ListItem Selected="True" Value="0">纵向</asp:ListItem>
                    <asp:ListItem Value="1">横向</asp:ListItem>
                </asp:RadioButtonList>
                <asp:Button ID="Buttoninit" runat="server" Font-Size="9pt" OnClick="Buttoninit_Click"
                    Text="初始化布置" ToolTip="初始化当前机房布置！" Width="80px"  CssClass="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" />&nbsp;&nbsp; 
                    <a onclick="$(save);return false" href="#" title="保存当前布置">保存</a>
                    &nbsp; <a onclick="$(reshow);return false" href="#" title="恢复到上次保存的布置">恢复</a>
                     &nbsp; <a onclick="$(uploadseat);return false" href="#" title="将当前布置提交给平台数据库">提交</a>
                <label id="msg" class="msgtext">
                </label>
            </div>
            <div id="sortable" class="sortablediv">
                <asp:Literal ID="myhouse" runat="server">
                <div></div>
                </asp:Literal>
            </div>
            <div style="text-align: center; font-size: 9pt">            
                &nbsp;<a onclick="$(lefttoleft);return false" href="#" title="水平左移所有电脑位置">水平左移←</a>&nbsp;
                &nbsp;<a onclick="$(lefttoright);return false" href="#" title="水平右移所有电脑位置">水平右移→</a>&nbsp;
                &nbsp;<a onclick="$(toptotop);return false" href="#" title="垂直上移所有电脑位置">垂直上移↑</a>&nbsp;
                &nbsp;<a onclick="$(toptodown);return false" href="#" title="垂直下移所有电脑位置">垂直下移↓</a>&nbsp;
            </div>
            <div id="showMessage" style="display: none;">
            </div>
        </div>
    </div>
    <script type="text/javascript">
        window.__computerConfig = {
            getHid: "<%=getHid() %>",
            this_firstshow: "<%=this.firstshow %>"
        };
    </script>
    <script type="text/javascript" src="../js/computer.js"></script>
    </form>
</body>
</html>
