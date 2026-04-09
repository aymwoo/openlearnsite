<%@ Page Title="" Language="C#" MasterPageFile="~/student/Stud.master" StylesheetTheme="Student" AutoEventWireup="true" CodeFile="allfinger.aspx.cs" Inherits="Student_allfinger" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cphs" Runat="Server">
<style type="text/css">
    .finger-rank-link,
    .finger-rank-link:link,
    .finger-rank-link:visited {
        color: #ffffff !important;
        text-decoration: none;
    }

    .finger-rank-link:hover,
    .finger-rank-link:focus {
        color: #ffffff !important;
        text-decoration: none;
        filter: brightness(1.03);
    }
</style>
<div class="w-full max-w-6xl mx-auto space-y-6">
    <div class="bg-white rounded-2xl shadow-sm border border-slate-200/60 p-6 sm:p-10 text-center">
        <h2 class="text-3xl font-extrabold text-slate-800 mb-6 flex items-center justify-center gap-3">
            <span class="text-4xl">👑</span> 英文输入英雄榜
        </h2>
        
        <div class="mb-8 flex justify-center items-center gap-3">
            <span class="text-slate-600 font-medium whitespace-nowrap">排版显示:</span>
            <asp:DropDownList ID="DDLselect" runat="server" AutoPostBack="True" 
                onselectedindexchanged="DDLselect_SelectedIndexChanged"
                CssClass="bg-slate-50 border border-slate-300 text-slate-700 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block p-2 cursor-pointer outline-none transition">
                <asp:ListItem Value="1">全校排行显示</asp:ListItem>
                <asp:ListItem Value="2">年级排行显示</asp:ListItem>
                <asp:ListItem Selected="True" Value="3">班级排行显示</asp:ListItem>
            </asp:DropDownList>
        </div>

        <div class="overflow-x-auto bg-white rounded-xl border border-slate-200">
                <asp:GridView ID="GVFinger" runat="server" AutoGenerateColumns="False"  CellPadding="0" 
                    Width="100%" PageSize="40" 
                    OnRowDataBound="GVFinger_RowDataBound" AllowPaging="True" 
                    onpageindexchanging="GVFinger_PageIndexChanging" SkinID="GridViewInfo" 
                            EnableModelValidation="True" CssClass="w-full text-slate-600 text-sm">
                    <Columns>
                        <asp:BoundField HeaderText="名次" >
                            <HeaderStyle CssClass="bg-slate-50 font-semibold px-4 py-3 text-center" />
                            <ItemStyle CssClass="text-center py-2.5 font-medium text-slate-500" />
						</asp:BoundField>
                        <asp:BoundField DataField="Psnum" HeaderText="学号" >
                            <HeaderStyle CssClass="bg-slate-50 font-semibold px-4 py-3 text-center" />
                            <ItemStyle CssClass="text-center py-2.5 font-mono text-slate-500" />
						</asp:BoundField>
                        <asp:BoundField DataField="Sname" HeaderText="姓名" >
                            <HeaderStyle HorizontalAlign="Left" CssClass="bg-slate-50 font-semibold px-4 py-3" />
                            <ItemStyle HorizontalAlign="Left" CssClass="py-2.5 font-bold text-slate-800" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Sgrade" HeaderText="年级" >
                            <HeaderStyle CssClass="bg-slate-50 font-semibold px-4 py-3 text-center" />
                            <ItemStyle CssClass="text-center py-2.5" />
						</asp:BoundField>
                        <asp:BoundField DataField="Sclass" HeaderText="班级" >
                            <HeaderStyle CssClass="bg-slate-50 font-semibold px-4 py-3 text-center" />
                            <ItemStyle CssClass="text-center py-2.5" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Pspd" HeaderText="速度" >
                            <HeaderStyle HorizontalAlign="Left" CssClass="bg-slate-50 font-semibold px-4 py-3" />
                            <ItemStyle HorizontalAlign="Left" CssClass="py-2.5 font-bold text-blue-600 text-base" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Pdate" HeaderText="日期" >
                            <HeaderStyle HorizontalAlign="Left" CssClass="bg-slate-50 font-semibold px-4 py-3 hidden sm:table-cell" />
                            <ItemStyle Width="180px" HorizontalAlign="Left" CssClass="py-2.5 text-xs text-slate-400 hidden sm:table-cell" />
                        </asp:BoundField>
                    </Columns>
                    <PagerTemplate>
                        <div class="flex items-center justify-between px-4 py-3 bg-slate-50 border-t border-slate-200">
                            <div class="text-sm text-slate-500">
                                第 <asp:Label ID="lblPageIndex" runat="server" CssClass="font-medium text-slate-900 mx-0.5" Text="<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>"></asp:Label> /
                                <asp:Label ID="lblPageCount" runat="server" CssClass="font-medium text-slate-900 mx-0.5" Text="<%# ((GridView)Container.Parent.Parent).PageCount %>"></asp:Label> 页
                            </div>
                            <div class="flex gap-2">
                                <asp:LinkButton ID="btnFirst" runat="server" CausesValidation="False" CommandArgument="First"
                                    CommandName="Page" CssClass="px-3 py-1.5 text-sm border border-slate-300 rounded-md hover:bg-slate-100 transition text-slate-600 font-medium bg-white" Text="首页"></asp:LinkButton>
                                <asp:LinkButton ID="btnPrev" runat="server" CausesValidation="False" CommandArgument="Prev"
                                    CommandName="Page" CssClass="px-3 py-1.5 text-sm border border-slate-300 rounded-md hover:bg-slate-100 transition text-slate-600 font-medium bg-white" Text="上页"></asp:LinkButton>
                                <asp:LinkButton ID="btnNext" runat="server" CausesValidation="False" CommandArgument="Next"
                                    CommandName="Page" CssClass="px-3 py-1.5 text-sm border border-slate-300 rounded-md hover:bg-slate-100 transition text-slate-600 font-medium bg-white" Text="下页"></asp:LinkButton>
                                <asp:LinkButton ID="btnLast" runat="server" CausesValidation="False" CommandArgument="Last"
                                    CommandName="Page" CssClass="px-3 py-1.5 text-sm border border-slate-300 rounded-md hover:bg-slate-100 transition text-slate-600 font-medium bg-white" Text="尾页"></asp:LinkButton>
                            </div>
                        </div>
                    </PagerTemplate>
                    <RowStyle CssClass="border-b border-slate-100 hover:bg-slate-50 transition" />
                </asp:GridView>
        </div>
        
        <div class="mt-8">
            <asp:HyperLink ID="HLtyperank" runat="server" NavigateUrl="~/student/typerank.aspx" 
                Target="_blank" CssClass="finger-rank-link inline-flex items-center justify-center gap-2 px-8 py-3 bg-gradient-to-r from-emerald-400 to-teal-500 text-white font-bold text-lg rounded-xl shadow-lg hover:from-emerald-500 hover:to-teal-600 transition-all transform hover:scale-105">
                ⚔️ 打字擂台榜
            </asp:HyperLink>
        </div>
    </div>
</div>
</asp:Content>


