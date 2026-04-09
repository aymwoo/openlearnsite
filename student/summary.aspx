<%@ Page Language="C#" MasterPageFile="~/student/Scm.master"  AutoEventWireup="true" CodeFile="summary.aspx.cs" Inherits="Student_summary" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cpcm" Runat="Server">
<div class="w-full max-w-4xl mx-auto">
    <div class="bg-white rounded-2xl shadow-sm border border-slate-200 p-6 sm:p-8 space-y-6 overflow-hidden">
        <!-- Header -->
        <div class="course-node-head flex items-center gap-3 pb-4 border-b border-slate-100" style="padding:24px 24px 20px;margin:-24px -24px 0;">
            <div class="w-8 h-8 rounded-lg bg-gradient-to-tr from-blue-500 to-indigo-500 flex items-center justify-center text-white">
                <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"></path></svg>
            </div>
            <div>
                <span class="text-sm text-slate-500 font-medium">学案名称</span>
                <div class="course-node-title text-lg font-bold text-slate-800"><asp:Label ID="Label1" runat="server"></asp:Label></div>
            </div>
            <asp:ImageButton ID="BtnEdit" runat="server" ToolTip="编辑" 
                ImageUrl="~/images/edit.gif" onclick="BtnEdit_Click" 
                Enabled="False" CssClass="ml-auto w-6 h-6 opacity-60 hover:opacity-100 transition" />
        </div>
        
        <!-- Summary Content -->
        <div>
            <h3 class="text-sm font-semibold text-slate-500 uppercase tracking-wider mb-3">总结内容</h3>
            <div id="contents" runat="server" 
                class="p-4 bg-slate-50 border border-slate-200 rounded-xl min-h-[120px] text-slate-700 leading-relaxed overflow-auto"></div>
        </div>
        
        <!-- Footer -->
        <div class="flex justify-end text-sm text-slate-400 pt-2 border-t border-slate-100">
            撰写日期：<asp:Label ID="Label6" runat="server" CssClass="text-slate-600 font-medium ml-1"></asp:Label>
        </div>
    </div>
</div>
</asp:Content>
