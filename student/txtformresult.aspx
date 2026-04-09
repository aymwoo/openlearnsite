<%@ Page Language="C#"  StylesheetTheme="Student" AutoEventWireup="true" CodeFile="txtformresult.aspx.cs" Inherits="Student_txtformresult" ResponseEncoding="utf-8" %>
<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
        <meta charset="utf-8" />
<title></title>
    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <style>
        body { 
            font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Noto Sans SC", sans-serif;
            background: #f8fafc;
            -webkit-font-smoothing: antialiased;
            margin: 0;
        }
        .topichead {
            padding: 0.75rem 1rem;
            background: linear-gradient(135deg, #f8fafc, #f1f5f9);
            border-bottom: 1px solid #e2e8f0;
            display: flex; flex-wrap: wrap; align-items: center; justify-content: space-between; gap: 0.5rem;
            border-radius: 0.75rem 0.75rem 0 0;
        }
        .topicleft { display: flex; flex-wrap: wrap; align-items: center; gap: 0.5rem; flex: 1; }
        .topicright { display: flex; flex-wrap: wrap; align-items: center; justify-content: flex-end; gap: 0.5rem; }
    </style>
</head>
<body oncontextmenu="return false" ondragstart="return false" onselectstart ="return false" onselect="document.selection.empty()" oncopy="document.selection.empty()" onbeforecopy="return false" onmouseup="document.selection.empty()">
    <form id="form1" runat="server">
<div id="topper" class="w-full max-w-5xl mx-auto p-4 sm:p-6 space-y-6">
    <!-- Title Card -->
    <div class="bg-white rounded-2xl shadow-sm border border-slate-200 p-6 text-center">
        <anthem:Image ID="Image2" runat="server" ImageUrl="~/images/inquiry.png" CssClass="w-8 h-8 inline-block mb-2" />
        <anthem:Label ID="LbMtitle" runat="server" CssClass="text-lg sm:text-xl font-extrabold text-slate-800 tracking-tight block"></anthem:Label>
    </div>
    
    <!-- Controls Header -->
    <div class="flex items-center justify-between flex-wrap gap-3">
        <div class="flex items-center gap-2">
            <h3 class="text-lg font-bold text-slate-800 flex items-center gap-2">
                <span class="w-1.5 h-5 bg-indigo-500 rounded-full inline-block"></span> 当前列表
            </h3>
            <anthem:Label ID="Labelreplycount" runat="server" CssClass="text-sm text-slate-500 font-medium"></anthem:Label>
            <anthem:ImageButton ID="ImageBtngoodall" runat="server" 
                ImageUrl="~/images/right.gif" onclick="ImageBtngoodall_Click" 
                ToolTip="给所有未评分的填表加6分" Visible="False" CssClass="w-4 h-4 opacity-70 hover:opacity-100 transition" />
            <anthem:ImageButton ID="ImageBtngood2" runat="server" 
                ImageUrl="~/images/right.gif" onclick="ImageBtngood2_Click" 
                ToolTip="给所有未评分的填表加2分" Visible="False" CssClass="w-4 h-4 opacity-70 hover:opacity-100 transition" />
        </div>
        <div class="flex items-center gap-2">
            <anthem:ImageButton ID="ImageBtnFresh" runat="server" 
                ImageUrl="~/images/refresh2.gif" onclick="ImageBtnFresh_Click" CssClass="w-5 h-5 opacity-70 hover:opacity-100 transition cursor-pointer" />
            <anthem:HyperLink ID="HLbottom" runat="server" BorderStyle="None" 
                BorderWidth="0px" ImageUrl="~/images/bottom.png" NavigateUrl="#bottom" 
                ToolTip="跳到底部" CssClass="w-5 h-5 opacity-70 hover:opacity-100 transition"></anthem:HyperLink>
        </div>
    </div>
    
    <!-- Form Results Grid -->
    <div class="space-y-4">
        <anthem:GridView ID="GVtxtform" runat="server" AutoGenerateColumns="False" 
            CellPadding="1" Width="100%" 
            onrowdatabound="GVtxtform_RowDataBound"  
            DataKeyNames="rid" PageSize="5" CellSpacing="1" 
            ShowHeader="False" GridLines="None" 
            onrowcommand="GVtxtform_RowCommand" CssClass="w-full">
             <Columns>
                 <asp:TemplateField>
                     <ItemTemplate>   
                     <div class="bg-white rounded-xl border border-slate-200 shadow-sm overflow-hidden mb-4 hover:shadow-md transition">
                         <div class="topichead">
                             <div class="topicleft">
                                 <anthem:Image ID="Imageflag" runat="server" ImageUrl="~/images/topicnormal.png" CssClass="w-4 h-4" />
                                 <anthem:Label ID="Labelfloor" runat="server" CssClass="text-xs text-slate-500 font-medium"></anthem:Label><span class="text-xs text-slate-400">楼</span>
                                 <anthem:Label ID="Labelsname" runat="server" Text='<%# Bind("Sname") %> ' CssClass="font-bold text-slate-800"></anthem:Label>
                                 <span class="text-xs text-slate-400">：</span>
                                 <anthem:Label ID="Labeldate" runat="server" Text='<%# Bind("Rtime") %> ' CssClass="text-xs text-slate-400"></anthem:Label>
                                 <span class="text-xs text-slate-400 ml-2">学分：</span>
                                 <anthem:Label ID="Labelscore" runat="server" Text='<%# Bind("Rscore") %> ' ToolTip="学分" CssClass="text-sm font-bold text-emerald-600"></anthem:Label>
                                 <anthem:Image ID="Imageagree" runat="server" Visible="False" ImageUrl="~/images/good16.png" CssClass="w-4 h-4" />
                                 <anthem:Label ID="Labelsnum" runat="server" Text='<%# Bind("Rsnum") %> ' Visible="False"></anthem:Label>
                             </div>
                             <div class="topicright">                        
                                 <anthem:ImageButton ID="ImageButtonGood" runat="server" 
                                     CausesValidation="false" CommandArgument='<%# Bind("rid") %>'
                                     CommandName="Good" ImageUrl="~/images/right.gif" ToolTip="加2分" CssClass="w-4 h-4 opacity-60 hover:opacity-100 transition cursor-pointer"></anthem:ImageButton>
                                 <anthem:ImageButton ID="ImageButtonless" runat="server" 
                                     CausesValidation="false" CommandArgument='<%# Bind("rid") %>'
                                     CommandName="Less" ImageUrl="~/images/ban.gif" ToolTip="减2分" CssClass="w-4 h-4 opacity-60 hover:opacity-100 transition cursor-pointer"></anthem:ImageButton>
                                 <span class="text-xs text-slate-400">赞(</span><anthem:Label ID="Labelagree" runat="server" Text='<%# Bind("Ragree") %> ' CssClass="text-sm font-bold text-pink-500"></anthem:Label><span class="text-xs text-slate-400">)</span>
                                 <anthem:ImageButton ID="ImageButtonAgree" runat="server" 
                                     CausesValidation="false" CommandArgument='<%# Bind("rid") %>'
                                     CommandName="Agree" ImageUrl="~/images/good24.gif" ToolTip="点赞" CssClass="w-5 h-5 opacity-60 hover:opacity-100 transition cursor-pointer"></anthem:ImageButton>
                             </div>
                         </div>
                         <div class="p-4 text-slate-700 leading-relaxed text-sm">
                             <%# UnEdit(HttpUtility.HtmlDecode( Eval("Rwords").ToString()))%>
                         </div>
                     </div>
                     </ItemTemplate>
                 </asp:TemplateField>
             </Columns>             
             <HeaderStyle Font-Bold="False" />
      </anthem:GridView>
    </div>
    
    <div id="bottom"></div>
    
    <!-- Bottom Controls -->
    <div class="flex items-center justify-between flex-wrap gap-3">
        <div></div>
        <div class="flex items-center gap-2">
            <anthem:ImageButton ID="ImageBtnFreshtwo" runat="server" 
                ImageUrl="~/images/refresh2.gif" onclick="ImageBtnFresh_Click" CssClass="w-5 h-5 opacity-70 hover:opacity-100 transition cursor-pointer" />
            <anthem:HyperLink ID="HLtop" runat="server" BorderStyle="None" BorderWidth="0px" 
                ImageUrl="~/images/top.png" NavigateUrl="#topper" ToolTip="跳到顶部" CssClass="w-5 h-5 opacity-70 hover:opacity-100 transition"></anthem:HyperLink>
        </div>
    </div>
    
    <div class="text-sm text-slate-400">
        <anthem:Label ID="Labelnostu" runat="server"></anthem:Label>    
    </div>
</div>
    </form>
</body>
</html>
