<%@ Page Title="" Language="C#" StylesheetTheme="Student"  AutoEventWireup="true" CodeFile="myevaluate.aspx.cs" Inherits="Student_myevaluate" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <meta charset="UTF-8" />
    <title></title>  
    <script language=javascript type=text/javascript>
        document.oncontextmenu = new Function('event.returnValue=false;');
        document.onselectstart = new Function('event.returnValue=false;');
    </script> 
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
<body>
<form id="form1" runat="server">
<div class="w-full max-w-5xl mx-auto p-4 sm:p-6 space-y-4">
    <!-- Voting Header Card -->
    <div class="bg-white rounded-2xl shadow-sm border border-slate-200 p-4 sm:p-6">
        <div class="flex flex-wrap items-center gap-3 mb-4">
            <asp:Image ID="Image2" runat="server" ImageUrl="~/images/wvote.png" CssClass="w-6 h-6" />
            <asp:Label ID="Labelscope" runat="server" CssClass="font-bold text-slate-800"></asp:Label>
            <span class="text-sm font-semibold text-slate-500">作品互评：</span>
            <asp:Label ID="Labelmtitle" runat="server" CssClass="font-bold text-indigo-600"></asp:Label>
            <asp:Label ID="Labelwmid" runat="server" Visible="false"></asp:Label>
            <asp:Image ID="ImageWtype" runat="server" CssClass="w-5 h-5" />
            <asp:Label ID="LabelWtype" runat="server" CssClass="text-sm text-slate-600"></asp:Label>
        </div>
        
        <div class="flex flex-wrap gap-x-5 gap-y-1 text-sm text-slate-600 bg-slate-50 rounded-xl p-3 border border-slate-100">
            <div>作品总数：<asp:Label ID="Labelhow" runat="server" CssClass="font-bold text-slate-800"></asp:Label></div>
            <div>可投次数：<asp:Label ID="Labelegg" runat="server" CssClass="font-bold text-orange-500"></asp:Label></div>
            <div>我的得票：<asp:Label ID="Labelme" runat="server" CssClass="font-bold text-pink-500"></asp:Label></div>
            <div>互评得分：<asp:Label ID="Labelwfscore" runat="server" CssClass="font-bold text-emerald-600"></asp:Label></div>
            <asp:Label ID="LabelMgid" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="Labelwdate" runat="server" Visible="false"></asp:Label>
        </div>
    </div>
    
    <!-- Student Works List -->
    <div class="bg-white rounded-2xl shadow-sm border border-slate-200 p-4 overflow-x-auto">
        <asp:DataList ID="DataListvote" runat="server" RepeatDirection="Horizontal" 
            RepeatColumns="15" DataKeyField="wid" OnItemCommand="DataListvote_ItemCommand" 
            CellPadding="3"
            onitemdatabound="DataListvote_ItemDataBound" CssClass="w-full">
            <ItemTemplate>
                <div class="inline-flex flex-col items-center w-16 p-1.5 rounded-lg hover:bg-indigo-50 transition cursor-pointer">
                    <asp:LinkButton ID="lBtnSname" runat="server" CommandArgument='<%# Eval("Wurl") %>' CommandName="S" 
                        ToolTip="点击预览我的作品！" Text='<%# Eval("Wname") %>' 
                        CssClass="text-xs font-semibold text-blue-600 hover:text-blue-800 transition truncate w-full text-center"></asp:LinkButton>
                    <asp:Label ID="LabelWflash" runat="server" Text='<%# Eval("Wflash") %>' Visible="False"></asp:Label>
                    <asp:Label ID="LabelWid" runat="server" Text='<%# Eval("wid") %>' Visible="False"></asp:Label>
                    <asp:Label ID="LabelWnum" runat="server" Text='<%# Eval("Wnum") %>' Visible="False"></asp:Label>
                </div>
            </ItemTemplate>
        </asp:DataList>
    </div>
    
    <!-- Rating Criteria -->
    <div class="bg-indigo-50 rounded-2xl border border-indigo-100 p-4 sm:p-6">
        <asp:DataList ID="DataListGauge" runat="server"
            CellPadding="8" RepeatColumns="2" 
            onitemdatabound="DataListGauge_ItemDataBound" HorizontalAlign="Center" 
            RepeatDirection="Horizontal" CellSpacing="4" Width="100%" >
            <ItemTemplate>
                <div class="flex items-center gap-2 text-sm">
                    <asp:CheckBox ID="RbMitem" runat="server" Text='<%# Eval("Mitem") %>' CssClass="accent-indigo-500" />
                    <asp:Label ID="LabelCount" runat="server" CssClass="text-xs text-slate-500"></asp:Label>
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/images/smile16.gif" CssClass="w-4 h-4" />
                    <asp:Label ID="LbMid" runat="server" Text='<%# Eval("mid") %>' Visible="False"></asp:Label>
                    <asp:Label ID="LbMscore" runat="server" Text='<%# Eval("Mscore") %>' Visible="False"></asp:Label>
                </div>
            </ItemTemplate>
        </asp:DataList>
        
        <script>
            function check() {
                var inputs = document.getElementById("<%=DataListGauge.ClientID%>").getElementsByTagName("input");
                var s = 0;
                for (var i = 0; i < inputs.length; i++) {
                    if (inputs[i].checked) {
                        s++;
                    }
                }
                if (s > 3) {
                    alert("您最多只能选3项！");
                    return false;
                }
                else {
                    return true;
                }
            }
        </script>
    </div>
    
    <!-- Vote Action Bar -->
    <div class="bg-white rounded-2xl shadow-sm border border-slate-200 p-4 flex flex-wrap items-center justify-center gap-4">
        <asp:Label ID="Labelmsg" runat="server" CssClass="text-sm text-slate-600"></asp:Label>
        <div class="flex items-center gap-2">
            <asp:Image ID="ImageDown0" runat="server" ImageUrl="~/images/good16.png" CssClass="w-4 h-4" />
            <asp:CheckBox ID="CheckBoxGood" runat="server" Text="推荐" CssClass="text-sm font-medium text-slate-600" />
        </div>
        <asp:Button ID="BtnVote" runat="server" onclick="BtnVote_Click" 
            SkinID="buttonSkinPink" Text="请投我一票"
            CssClass="px-6 py-2.5 bg-gradient-to-r from-pink-500 to-rose-500 text-white font-bold rounded-xl hover:from-pink-600 hover:to-rose-600 transition duration-300 shadow-md border-0 cursor-pointer" />
    </div>
</div>
</form>

<!-- Preview Area -->
<div class="w-full max-w-5xl mx-auto px-4 sm:px-6 pb-8">
	<asp:Literal ID="Literal1" runat="server"></asp:Literal> 
	<asp:Label ID="Labelwname" runat="server" Visible="False"></asp:Label>
	<asp:Label ID="lbMyFeedback" runat="server" Visible="False"></asp:Label>
</div>

</body>
</html>
