<%@ Page Language="C#" MasterPageFile="~/student/Scm.master" StylesheetTheme="Student" Validaterequest="false" AutoEventWireup="true" CodeFile="summaryedit.aspx.cs" Inherits="Student_summaryedit" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cpcm" Runat="Server">
<div class="w-full max-w-4xl mx-auto">
    <div class="bg-white rounded-2xl shadow-sm border border-slate-200 p-6 sm:p-8 space-y-6 overflow-hidden">
        <!-- Header -->
        <div class="course-node-head flex items-center gap-3 pb-4 border-b border-slate-100" style="padding:24px 24px 20px;margin:-24px -24px 0;">
            <div class="w-8 h-8 rounded-lg bg-gradient-to-tr from-blue-500 to-indigo-500 flex items-center justify-center text-white">
                <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z"></path></svg>
            </div>
            <div>
                <span class="text-sm text-slate-500 font-medium">学案名称</span>
                <div class="course-node-title text-lg font-bold text-slate-800"><asp:Label ID="Label1" runat="server"></asp:Label></div>
            </div>
        </div>
        
        <!-- Editor Area -->
        <div>
            <h3 class="text-sm font-semibold text-slate-500 uppercase tracking-wider mb-3">总结内容</h3>
            <script charset="utf-8" src="../kindeditor/kindeditor-min.js"></script>
            <script charset="utf-8" src="../kindeditor/lang/zh_CN.js"></script>
            <script>
                var editor;
                var cid= <%=myCid() %>;
                var ty="Course";
                var upjs= '../kindeditor/aspnet/upload_json.aspx?cid='+cid+'&Ty='+ty;
                var fmjs='../kindeditor/aspnet/file_manager_json.aspx?cid='+cid+'&Ty='+ty;
                KindEditor.ready(function (K) {
                    editor = K.create('textarea[name="textareaItem"]', {
                        resizeType: 1,
                        newlineTag: "br",                    
                        uploadJson : upjs,
                        fileManagerJson : fmjs,
                        allowFileManager : true		            
                    });
                });
            </script>
            <textarea name="textareaItem" style="width: 100%; height:300px;" ><%=contentstr %></textarea>
        </div>
        
        <!-- Footer -->
        <div class="flex items-center justify-between pt-4 border-t border-slate-100">
            <div class="text-sm text-slate-400">
                撰写日期：<asp:Label ID="Label6" runat="server" CssClass="text-slate-600 font-medium"></asp:Label>
            </div>
            <asp:Button ID="ButtonEdit" runat="server" onclick="ButtonEdit_Click" 
                Text="添加总结" SkinID="buttonSkinPink" Enabled="False"
                CssClass="px-6 py-2.5 bg-gradient-to-r from-blue-500 to-indigo-600 text-white font-bold rounded-xl hover:from-blue-600 hover:to-indigo-700 transition duration-300 shadow-md border-0 cursor-pointer" />
        </div>
    </div>
</div>
</asp:Content>
