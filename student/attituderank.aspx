<%@ Page Language="C#" AutoEventWireup="true" CodeFile="attituderank.aspx.cs" Inherits="Student_attituderank" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
        <meta charset="utf-8" />
<title>课堂小测验班级排行</title>
    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <style>
        * { margin: 0; padding: 0; box-sizing: border-box; }
        body { 
            font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Noto Sans SC", sans-serif;
            background: linear-gradient(135deg, #f8fafc 0%, #e0e7ff 100%);
            min-height: 100vh;
            -webkit-font-smoothing: antialiased;
        }
    </style>
</head>
<body>
<form id="form1" runat="server">
<div class="min-h-screen flex flex-col items-center justify-center p-4 sm:p-8">
    <div class="w-full max-w-2xl bg-white rounded-2xl shadow-lg border border-slate-200 p-6 sm:p-8 space-y-6">
        <!-- Header -->
        <div class="text-center">
            <div class="inline-flex items-center gap-2 px-4 py-2 bg-indigo-50 rounded-xl mb-4">
                <svg class="w-5 h-5 text-indigo-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 19v-6a2 2 0 00-2-2H5a2 2 0 00-2 2v6a2 2 0 002 2h2a2 2 0 002-2zm0 0V9a2 2 0 012-2h2a2 2 0 012 2v10m-6 0a2 2 0 002 2h2a2 2 0 002-2m0 0V5a2 2 0 012-2h2a2 2 0 012 2v14a2 2 0 01-2 2h-2a2 2 0 01-2-2z"></path></svg>
                <span class="text-sm font-bold text-indigo-700">班级排行榜</span>
            </div>
            <asp:Label ID="Labeltitle" runat="server" CssClass="block text-xl font-extrabold text-slate-800 tracking-tight"></asp:Label>
        </div>

        <!-- Rank Table -->
        <div class="overflow-x-auto rounded-xl border border-slate-200 shadow-sm">
            <asp:GridView ID="GridViewclass" runat="server" 
                AutoGenerateColumns="False"                         
                onrowdatabound="GridViewclass_RowDataBound" 
                Width="100%" CellPadding="3" EnableModelValidation="True" 
                GridLines="None"
                CssClass="w-full text-sm text-slate-600 bg-white">
                <Columns>
                    <asp:BoundField HeaderText="序号">
                        <HeaderStyle CssClass="bg-gradient-to-r from-indigo-500 to-blue-500 text-white font-semibold px-4 py-3 text-center" />
                        <ItemStyle CssClass="px-4 py-2.5 text-center font-bold text-indigo-600 border-b border-slate-100" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Sgrade" HeaderText="年级">
                        <HeaderStyle CssClass="bg-gradient-to-r from-indigo-500 to-blue-500 text-white font-semibold px-4 py-3 text-center" />
                        <ItemStyle CssClass="px-4 py-2.5 text-center border-b border-slate-100" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Sclass" HeaderText="班级">
                        <HeaderStyle CssClass="bg-gradient-to-r from-indigo-500 to-blue-500 text-white font-semibold px-4 py-3 text-center" />
                        <ItemStyle CssClass="px-4 py-2.5 text-center border-b border-slate-100" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Snum" HeaderText="学号">
                        <HeaderStyle CssClass="bg-gradient-to-r from-indigo-500 to-blue-500 text-white font-semibold px-4 py-3 text-center" />
                        <ItemStyle CssClass="px-4 py-2.5 text-center border-b border-slate-100 font-mono" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Sname" HeaderText="姓名">
                        <HeaderStyle CssClass="bg-gradient-to-r from-indigo-500 to-blue-500 text-white font-semibold px-4 py-3 text-center" />
                        <ItemStyle CssClass="px-4 py-2.5 text-center font-semibold text-slate-800 border-b border-slate-100" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Sattitude" HeaderText="表现">
                        <HeaderStyle CssClass="bg-gradient-to-r from-indigo-500 to-blue-500 text-white font-semibold px-4 py-3 text-center" />
                        <ItemStyle CssClass="px-4 py-2.5 text-center font-bold text-emerald-600 border-b border-slate-100" />
                    </asp:BoundField>
                </Columns>  
                <RowStyle CssClass="hover:bg-slate-50 transition" />
            </asp:GridView>
        </div>
        
        <!-- Close Button -->
        <div class="text-center pt-2">
            <asp:Button ID="Btnreturn" runat="server" Text="关闭"
                CssClass="px-8 py-2.5 bg-slate-100 text-slate-600 font-semibold rounded-xl hover:bg-slate-200 transition duration-300 border border-slate-300 cursor-pointer" />
        </div>
    </div>
</div>
</form>
</body>
</html>
