<%@ Page Language="C#" AutoEventWireup="true" CodeFile="manage.aspx.cs" Inherits="Python_manage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h2>Python绘画编程导入导出管理页面</h2>
        <p>&nbsp;</p>
        <p>&nbsp;</p>

    <p>
        使用说明：点击导出作品后，将会在python下thumbnail目录生成作品表的xml格式导出数据，</p>
        <p>
            将thumbnail目录复制到其他学习平台同目录下，再点导入作品即可新增。
    </p>
    <center>
        <p>
        <asp:Button ID="Btnpackage" runat="server" Text="导出作品" onclick="Btnpackage_Click"  CssClass="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" />
    &nbsp;&nbsp;&nbsp;
        <asp:HyperLink ID="Hlurl" runat="server" CssClass="px-4 py-2 bg-green-500 text-white rounded hover:bg-green-600 transition duration-300 shadow-md text-center inline-block">下载地址</asp:HyperLink>
    &nbsp;&nbsp;&nbsp; <asp:Button ID="Btnimport" runat="server" Text="导入作品" onclick="Btnimport_Click"  CssClass="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" />

    </p>
        <p>
            &nbsp;</p> 

    <p style=" text-align:left;"> 
        使用说明：点击导出比赛后，将会在python下imgmatch目录生成作品表的xml格式导出数据，</p>
        <p style=" text-align:left;"> 
        将imgmatch目录复制到其他学习平台同目录下，再点导入比赛即可新增。</p> 
            
   <p>
        &nbsp;<asp:DropDownList ID="DropDownListmatch" runat="server">
            </asp:DropDownList>
&nbsp;&nbsp;
        <asp:Button ID="Buttonmatch" runat="server" Text="导出比赛" 
                onclick="Buttonmatch_Click"   CssClass="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" />
    &nbsp;&nbsp;&nbsp;
        <asp:HyperLink ID="Hlmatch" runat="server" CssClass="px-4 py-2 bg-green-500 text-white rounded hover:bg-green-600 transition duration-300 shadow-md text-center inline-block">下载地址</asp:HyperLink>
    &nbsp;&nbsp;&nbsp; <asp:Button ID="Buttonimport" runat="server" Text="导入比赛" 
                onclick="Buttonimport_Click"  CssClass="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" />

    </p>

    <p>
        &nbsp;</p>
        <p>
     <asp:Label ID="Labelmsg" runat="server" ></asp:Label>   
    </p>

    </center>

    </div>
    </form>
</body>
</html>
