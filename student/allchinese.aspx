<%@ Page Title="" Language="C#"  StylesheetTheme="Student" AutoEventWireup="true" CodeFile="allchinese.aspx.cs" Inherits="Student_allchinese" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
        <meta charset="utf-8" />
<title></title>   
    
    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
<div class="w-full max-w-6xl mx-auto space-y-6 pt-8 pb-12 px-4 sm:px-6">
    <div class="bg-white rounded-3xl shadow-[0_8px_30px_rgb(0,0,0,0.04)] border border-slate-200/60 p-6 sm:p-10 text-center relative overflow-hidden">
        <!-- Decorative background elements -->
        <div class="absolute top-0 right-0 -mr-20 -mt-20 w-64 h-64 rounded-full bg-blue-50 opacity-50 blur-3xl pointer-events-none"></div>
        <div class="absolute bottom-0 left-0 -ml-20 -mb-20 w-80 h-80 rounded-full bg-indigo-50 opacity-40 blur-3xl pointer-events-none"></div>
        
        <div class="relative z-10">
            <h2 class="text-3xl sm:text-4xl font-extrabold text-slate-800 mb-8 flex items-center justify-center gap-4">
                <span class="text-4xl sm:text-5xl drop-shadow-sm">👑</span> 
                <span class="bg-clip-text text-transparent bg-gradient-to-r from-slate-800 to-indigo-800">
                    拼音输入英雄榜
                </span>
            </h2>
            
            <div class="mb-8 flex justify-center items-center gap-3">
                <span class="text-slate-600 font-medium whitespace-nowrap">排版显示:</span>
                <asp:DropDownList ID="DDLselect" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="DDLselect_SelectedIndexChanged"
                    CssClass="bg-slate-50 border border-slate-300 text-slate-700 text-sm rounded-xl focus:ring-indigo-500 focus:border-indigo-500 block p-2.5 cursor-pointer outline-none transition shadow-sm font-medium">
                    <asp:ListItem Value="1">全校排行显示</asp:ListItem>
                    <asp:ListItem Value="2">年级排行显示</asp:ListItem>
                    <asp:ListItem Selected="True" Value="3">班级排行显示</asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="overflow-x-auto bg-white rounded-2xl border border-slate-200 shadow-sm">
                <asp:GridView ID="GVFinger" runat="server" AutoGenerateColumns="False" CellPadding="0" 
                    Width="100%" PageSize="40" 
                    OnRowDataBound="GVFinger_RowDataBound" AllowPaging="True" 
                    onpageindexchanging="GVFinger_PageIndexChanging" SkinID="GridViewInfo" 
                            EnableModelValidation="True" CssClass="w-full text-slate-600 text-sm">
                    <Columns>
                        <asp:BoundField HeaderText="名次">
                            <HeaderStyle CssClass="bg-slate-50 font-semibold px-4 py-4 text-center border-b border-slate-200 text-slate-700" />
                            <ItemStyle CssClass="text-center py-3.5 font-bold text-slate-500" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Psnum" HeaderText="学号">
                            <HeaderStyle CssClass="bg-slate-50 font-semibold px-4 py-4 text-center border-b border-slate-200 text-slate-700" />
                            <ItemStyle CssClass="text-center py-3.5 font-mono text-slate-500 text-xs" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Sname" HeaderText="姓名">
                            <HeaderStyle HorizontalAlign="Left" CssClass="bg-slate-50 font-semibold px-4 py-4 border-b border-slate-200 text-slate-700" />
                            <ItemStyle HorizontalAlign="Left" CssClass="py-3.5 font-bold text-slate-800 text-base" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Sgrade" HeaderText="年级">
                            <HeaderStyle CssClass="bg-slate-50 font-semibold px-4 py-4 text-center border-b border-slate-200 text-slate-700" />
                            <ItemStyle CssClass="text-center py-3.5 font-medium text-slate-600" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Sclass" HeaderText="班级">
                            <HeaderStyle CssClass="bg-slate-50 font-semibold px-4 py-4 text-center border-b border-slate-200 text-slate-700" />
                            <ItemStyle CssClass="text-center py-3.5 font-medium text-slate-600" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Ptotal" HeaderText="收集苹果数">
                            <HeaderStyle HorizontalAlign="Left" CssClass="bg-slate-50 font-semibold px-4 py-4 border-b border-slate-200 text-slate-700" />
                            <ItemStyle HorizontalAlign="Left" CssClass="py-3.5 font-extrabold text-orange-500 text-lg flex items-center gap-1.5 before:content-['🍎']" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Pdate" HeaderText="日期">
                            <HeaderStyle HorizontalAlign="Left" CssClass="bg-slate-50 font-semibold px-4 py-4 hidden sm:table-cell border-b border-slate-200 text-slate-700" />
                            <ItemStyle Width="120px" HorizontalAlign="Left" CssClass="py-3.5 text-xs text-slate-400 hidden sm:table-cell" />
                        </asp:BoundField>
                    </Columns>
                    <PagerTemplate>
                        <div class="flex flex-col sm:flex-row items-center justify-between px-6 py-4 bg-slate-50/80 border-t border-slate-200 gap-4">
                            <div class="text-sm font-medium text-slate-500">
                                第 <asp:Label ID="lblPageIndex" runat="server" CssClass="font-bold text-indigo-600 text-base mx-1" Text="<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>"></asp:Label> 
                                <span class="mx-1 text-slate-300">/</span> 
                                共 <asp:Label ID="lblPageCount" runat="server" CssClass="font-bold text-slate-700 mx-1" Text="<%# ((GridView)Container.Parent.Parent).PageCount %>"></asp:Label> 页
                            </div>
                            <div class="flex gap-2 bg-white p-1 rounded-lg shadow-sm border border-slate-200">
                                <asp:LinkButton ID="btnFirst" runat="server" CausesValidation="False" CommandArgument="First"
                                    CommandName="Page" CssClass="px-3 py-1.5 text-sm rounded hover:bg-indigo-50 hover:text-indigo-600 transition text-slate-600 font-medium" Text="首页"></asp:LinkButton>
                                <asp:LinkButton ID="btnPrev" runat="server" CausesValidation="False" CommandArgument="Prev"
                                    CommandName="Page" CssClass="px-3 py-1.5 text-sm rounded hover:bg-indigo-50 hover:text-indigo-600 transition text-slate-600 font-medium" Text="上页"></asp:LinkButton>
                                <div class="w-px h-6 bg-slate-200 self-center mx-1"></div>
                                <asp:LinkButton ID="btnNext" runat="server" CausesValidation="False" CommandArgument="Next"
                                    CommandName="Page" CssClass="px-3 py-1.5 text-sm rounded hover:bg-indigo-50 hover:text-indigo-600 transition text-slate-600 font-medium" Text="下页"></asp:LinkButton>
                                <asp:LinkButton ID="btnLast" runat="server" CausesValidation="False" CommandArgument="Last"
                                    CommandName="Page" CssClass="px-3 py-1.5 text-sm rounded hover:bg-indigo-50 hover:text-indigo-600 transition text-slate-600 font-medium" Text="尾页"></asp:LinkButton>
                            </div>
                        </div>
                    </PagerTemplate>
                    <RowStyle CssClass="border-b border-slate-100 hover:bg-slate-50/80 transition duration-150" />
                    <AlternatingRowStyle CssClass="bg-slate-50/30 border-b border-slate-100 hover:bg-slate-50/80 transition duration-150" />
                </asp:GridView>
            </div>
        </div>
    </div>
</div>
    </form>
</body>
</html>
