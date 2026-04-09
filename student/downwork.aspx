<%@ Page Title="" Language="C#" StylesheetTheme="Student" Validaterequest="false" AutoEventWireup="true" CodeFile="downwork.aspx.cs" Inherits="Student_downwork" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
        <meta charset="utf-8" />
<title>作品下载</title> 
    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <style>
        body { 
            font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Noto Sans SC", sans-serif;
            background: #f8fafc;
            -webkit-font-smoothing: antialiased;
            margin: 0;
        }
    </style>
</head>
<body ondragstart="return false" onselectstart ="return false" >
     <form id="form1" runat="server">
<div class="w-full max-w-4xl mx-auto p-4 sm:p-6 space-y-6">
    <!-- Work Info Card -->
    <div class="bg-white rounded-2xl shadow-sm border border-slate-200 p-6">
        <div class="flex flex-wrap items-center gap-2 mb-4 text-slate-800">
            <span class="text-sm text-slate-500">〖</span>
            <asp:Label ID="Labelmission" runat="server" CssClass="font-bold text-indigo-600"></asp:Label>
            <span class="text-sm text-slate-500">〗</span>
            <span class="font-bold text-slate-800">作品下载：</span>
            <asp:Image ID ="ImageType" runat="server" CssClass="w-5 h-5" />
            <asp:HyperLink ID="HLfile" runat="server" Visible="False" BorderStyle="None" 
                CssClass="inline-flex items-center gap-1 px-3 py-1.5 bg-emerald-50 text-emerald-700 font-semibold text-sm rounded-lg hover:bg-emerald-100 transition border border-emerald-200" Target="_blank">作品</asp:HyperLink>
            <asp:Label ID="Labelsize" runat="server" CssClass="text-xs text-slate-400"></asp:Label>
            <asp:Label ID="Labelmsg" runat="server" CssClass="text-sm text-slate-600"></asp:Label>
            <asp:Image ID="Imagegood" runat="server" ImageUrl="~/images/good16.png" CssClass="w-4 h-4" />
            <asp:Label ID="Labelgood" runat="server" CssClass="font-bold text-pink-500 text-sm"></asp:Label>
            <asp:Label ID="LabelWdate" runat="server" CssClass="text-xs text-slate-400 ml-auto"></asp:Label>
        </div>
        
        <div class="flex flex-wrap gap-x-6 gap-y-1 bg-slate-50 rounded-xl p-3 border border-slate-100 text-sm text-slate-600">
            <span>作品评分：<asp:Label ID="LbWscore" runat="server" CssClass="font-bold text-orange-500"></asp:Label></span>
            <span>作品加分：<asp:Label ID="LbWdscore" runat="server" CssClass="font-bold text-emerald-600"></asp:Label></span>
            <span>互评得分：<asp:Label ID="LbWfscore" runat="server" CssClass="font-bold text-blue-600"></asp:Label></span>
            <span>教师评语：<asp:Label ID="LbWself" runat="server" CssClass="font-medium text-indigo-600"></asp:Label></span>
        </div>
    </div>
    
    <!-- Work Preview -->
    <div class="bg-white rounded-2xl shadow-sm border border-slate-200 p-6">
        <asp:Literal ID="Literal1" runat="server"></asp:Literal> 
    </div>
    
    <asp:Label ID="Labelwid" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="Labeltype" runat="server" Visible="False"></asp:Label>
    <asp:Label ID="Labelwurl" runat="server" Visible="False"></asp:Label>
</div>
     </form>
</body>
</html>
