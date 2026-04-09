<%@ Page Language="C#" AutoEventWireup="true"  StylesheetTheme="Student" CodeFile="autonomic.aspx.cs" Inherits="Student_autonomic" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
        <meta charset="utf-8" />
<title></title>   
    <link href="../App_Themes/student/StyleSheet.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .divcate{margin:auto; padding: 2px; background-color: #E0ECFE; font-size: 11pt; font-weight: bold; text-align: left; height: 24px; width:100%;}
        .divlate{margin:auto; padding: 2px; background-color: #E0ECFE; font-size: 11pt; font-weight: bold; text-align: left; height: 24px; width:100%;}
        .licss{font-size: 11pt; height:30px; width:400px; text-align: left; border-width: 1px; border-bottom-style: dashed; border-color: #CCCCCC}
        .licss1{font-size: 11pt; height:24px; width:98%; text-align: left; border-width: 1px; border-bottom-style: dashed; border-color: #CCCCCC}
        .licss2{font-size: 11pt; height:24px; width:98%; text-align: left; border-width: 1px; border-bottom-style: dashed; border-color: #CCCCCC; background-color:#eeeeee}
    </style>
    
    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
      <div class="studmasterhead">
       <div class="banner"></div>
       <div class="path"></div>
      <div class="w-full max-w-6xl mx-auto">
<div class="grid grid-cols-1 lg:grid-cols-4 gap-6 lg:gap-8 w-full max-w-full text-left p-4">
    <!-- Main Content -->
    <div class="lg:col-span-3 space-y-6 overflow-hidden min-w-0">
        <div class="bg-white rounded-2xl shadow-sm border border-slate-200/60 p-6">
            <h3 class="text-lg font-bold text-slate-800 flex items-center gap-2 border-b border-slate-100 pb-2 mb-4">
                <span class="w-1.5 h-5 bg-blue-500 rounded-full inline-block"></span> 资源分类
            </h3>
            <asp:DataList ID="DLCategory" runat="server" RepeatColumns="1" 
                RepeatDirection="Horizontal" Width="100%" CellPadding="0" CellSpacing="0" 
                DataKeyField="yid" onitemdatabound="DLCategory_ItemDataBound"
                CssClass="w-full">
                <ItemTemplate>
                    <div class="mb-4 border border-slate-200 rounded-xl overflow-hidden hover:shadow-md transition">
                        <div class="bg-blue-50 px-4 py-3 flex items-center gap-2 border-b border-slate-200">
                            <img alt="" src="../images/filetype/read.gif" class="w-4 h-4" />
                            <asp:HyperLink ID="HLYtitle" runat="server" Text='<%# Eval("Ytitle") %>' CssClass="font-bold text-slate-700 hover:text-blue-600 transition"></asp:HyperLink>
                        </div>
                        <div class="p-4 bg-white">
                            <%#ListNews(Eval("yid"),5, "text-slate-600 border-b border-slate-100 py-2 hover:bg-slate-50 transition block px-2 truncate",30)%>    
                        </div>
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </div>
    </div>
    
    <!-- Sidebar -->
    <div class="lg:col-span-1 space-y-6 self-start">
        <div class="bg-indigo-50 border border-indigo-100 rounded-2xl p-5 shadow-sm">
            <h4 class="text-indigo-800 font-bold mb-4 flex items-center gap-2 border-b border-indigo-200/60 pb-2">
                <svg class="w-5 h-5 text-indigo-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10"></path></svg>
                我的作品
            </h4>
            
            <ul class="space-y-2">
                <asp:Repeater ID="RepMy" runat="server" >
                    <ItemTemplate>
                        <li class="border-b border-slate-200/60 pb-2">
                            <a href="<%#GetdownUrl(Eval("Aurl").ToString())%>" target="_blank" class="text-slate-600 hover:text-indigo-600 transition font-medium text-sm block truncate"><%#strcut( Eval("Ftitle").ToString())%></a>
                        </li>
                    </ItemTemplate>
                    <AlternatingItemTemplate>
                        <li class="border-b border-slate-200/60 pb-2">
                            <a href="<%#GetdownUrl(Eval("Aurl").ToString())%>" target="_blank" class="text-slate-600 hover:text-indigo-600 transition font-medium text-sm block truncate"><%#strcut( Eval("Ftitle").ToString())%></a>
                        </li>
                    </AlternatingItemTemplate>
                </asp:Repeater>
            </ul>
        </div>
    </div>
</div>      
        </div>
       </div>
    </form>
</body>
</html>
