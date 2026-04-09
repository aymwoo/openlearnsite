<%@ Page Language="C#" AutoEventWireup="true" CodeFile="typerank.aspx.cs" Inherits="Student_typerank" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta charset="utf-8" />
<title>文字输入擂台榜</title>
    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <style>
        body { 
            font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Noto Sans SC", sans-serif;
            background: linear-gradient(135deg, #f8fafc 0%, #e0e7ff 100%);
            -webkit-font-smoothing: antialiased;
            margin: 0; min-height: 100vh;
        }
        .rank-card {
            display: inline-flex; flex-direction: column; align-items: center;
            margin: 4px; padding: 12px 8px;
            border: 1px solid #e2e8f0; border-radius: 12px;
            background: white; text-align: center;
            transition: all 0.2s;
            min-width: 90px;
        }
        .rank-card:hover { box-shadow: 0 4px 12px rgba(0,0,0,0.08); transform: translateY(-2px); }
        .rank-card img { border-radius: 50%; object-fit: cover; border: 2px solid #e0e7ff; }
    </style>
</head>

<body>
    <form id="form1" runat="server">
    <div class="w-full max-w-6xl mx-auto p-4 sm:p-6 space-y-8">
        <!-- Chinese Input Section -->
        <div class="bg-white rounded-2xl shadow-sm border border-slate-200 p-6 sm:p-8">
            <h2 class="text-xl font-extrabold text-slate-800 mb-6 flex items-center gap-2">
                <span class="w-1.5 h-6 bg-emerald-500 rounded-full inline-block"></span> 中文输入擂台
            </h2>
            
            <div class="mb-6">
                <h3 class="text-sm font-bold text-slate-500 uppercase tracking-wider mb-3">🏆 全校榜</h3>
                <div class="flex flex-wrap justify-center">
                    <asp:DataList ID="DataList_allc" runat="server" 
                        onitemdatabound="DataList_allc_ItemDataBound" RepeatColumns="12" 
                        RepeatDirection="Horizontal">
                        <ItemTemplate>
                            <div class="rank-card">
                                <asp:Image ID="allcImage1" runat="server" Height="64px" Width="64px" /><br />
                                <asp:Label ID="allcLabelsname" runat="server" Text='<%# Eval("Sname") %>' CssClass="text-sm font-bold text-slate-800 mt-1"></asp:Label><br />
                                <asp:Label ID="allcLabelpsnum" runat="server" Text='<%# Eval("Psnum") %>' Visible="false"></asp:Label>
                                <span class="text-xs text-slate-400">
                                    <asp:Label ID="allcLabelsgrade" runat="server" Text='<%# Eval("Sgrade") %>'></asp:Label>.
                                    <asp:Label ID="allcsclass" runat="server" Text='<%# Eval("Sclass") %>'></asp:Label>班
                                </span><br />
                                <span class="text-xs font-bold text-emerald-600"><asp:Label ID="allcLabelpscore" runat="server" Text='<%# Eval("Pscore") %>'></asp:Label>字/分</span>
                            </div>
                        </ItemTemplate>
                    </asp:DataList>
                </div>
            </div>
            
            <asp:DataList ID="DataList_wai" runat="server" 
                onitemdatabound="DataList_wai_ItemDataBound" CssClass="w-full">
                <ItemTemplate>
                    <div class="mb-4">
                        <h3 class="text-sm font-bold text-indigo-600 mb-2 border-b border-slate-100 pb-1">
                            <asp:Label ID="Labelgrade" runat="server" Text='<%# Eval("Rgrade") %>' ></asp:Label>
                        </h3>
                        <div class="flex flex-wrap justify-center">
                            <asp:DataList ID="DataList_li" runat="server" 
                                onitemdatabound="DataList_li_ItemDataBound" RepeatColumns="12" 
                                RepeatDirection="Horizontal">
                                <ItemTemplate>
                                    <div class="rank-card">
                                        <asp:Image ID="Image1" runat="server" Height="64px" Width="64px" /><br />
                                        <asp:Label ID="Labelsname" runat="server" Text='<%# Eval("Sname") %>' CssClass="text-sm font-bold text-slate-800 mt-1"></asp:Label><br />
                                        <asp:Label ID="Labelpsnum" runat="server" Text='<%# Eval("Psnum") %>' Visible="false"></asp:Label>
                                        <span class="text-xs text-slate-400">
                                            <asp:Label ID="Labelsgrade" runat="server" Text='<%# Eval("Sgrade") %>'></asp:Label>.
                                            <asp:Label ID="sclass" runat="server" Text='<%# Eval("Sclass") %>'></asp:Label>班
                                        </span><br />
                                        <span class="text-xs font-bold text-emerald-600"><asp:Label ID="Labelpscore" runat="server" Text='<%# Eval("Pscore") %>'></asp:Label>字/分</span>
                                    </div>
                                </ItemTemplate>
                            </asp:DataList>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </div>
        
        <!-- English Input Section -->
        <div class="bg-white rounded-2xl shadow-sm border border-slate-200 p-6 sm:p-8">
            <h2 class="text-xl font-extrabold text-slate-800 mb-6 flex items-center gap-2">
                <span class="w-1.5 h-6 bg-blue-500 rounded-full inline-block"></span> 英文输入擂台
            </h2>
            
            <div class="mb-6">
                <h3 class="text-sm font-bold text-slate-500 uppercase tracking-wider mb-3">🏆 全校榜</h3>
                <div class="flex flex-wrap justify-center">
                    <asp:DataList ID="DataList_enall" runat="server" 
                        onitemdatabound="DataList_enall_ItemDataBound" RepeatColumns="12" 
                        RepeatDirection="Horizontal">
                        <ItemTemplate>
                            <div class="rank-card">
                                <asp:Image ID="lenImage1" runat="server" Height="64px" Width="64px" /><br />
                                <asp:Label ID="lenLabelsname" runat="server" Text='<%# Eval("Sname") %>' CssClass="text-sm font-bold text-slate-800 mt-1"></asp:Label><br />
                                <asp:Label ID="lenLabelpsnum" runat="server" Text='<%# Eval("Psnum") %>' Visible="false"></asp:Label>
                                <span class="text-xs text-slate-400">
                                    <asp:Label ID="lenLabelsgrade" runat="server" Text='<%# Eval("Sgrade") %>'></asp:Label>.
                                    <asp:Label ID="lensclass" runat="server" Text='<%# Eval("Sclass") %>'></asp:Label>班
                                </span><br />
                                <span class="text-xs font-bold text-blue-600"><asp:Label ID="lenLabelpspd" runat="server" Text='<%# Eval("Pspd") %>'></asp:Label>词/分</span>
                            </div>
                        </ItemTemplate>
                    </asp:DataList>
                </div>
            </div>
            
            <asp:DataList ID="DataList_enwai" runat="server" 
                onitemdatabound="DataList_enwai_ItemDataBound" CssClass="w-full">
                <ItemTemplate>
                    <div class="mb-4">
                        <h3 class="text-sm font-bold text-indigo-600 mb-2 border-b border-slate-100 pb-1">
                            <asp:Label ID="enLabelgrade" runat="server" Text='<%# Eval("Rgrade") %>' ></asp:Label>
                        </h3>
                        <div class="flex flex-wrap justify-center">
                            <asp:DataList ID="DataList_enli" runat="server" 
                                onitemdatabound="DataList_enli_ItemDataBound" RepeatColumns="12" 
                                RepeatDirection="Horizontal">
                                <ItemTemplate>
                                    <div class="rank-card">
                                        <asp:Image ID="enImage1" runat="server" Height="64px" Width="64px" /><br />
                                        <asp:Label ID="enLabelsname" runat="server" Text='<%# Eval("Sname") %>' CssClass="text-sm font-bold text-slate-800 mt-1"></asp:Label><br />
                                        <asp:Label ID="enLabelpsnum" runat="server" Text='<%# Eval("Psnum") %>' Visible="false"></asp:Label>
                                        <span class="text-xs text-slate-400">
                                            <asp:Label ID="enLabelsgrade" runat="server" Text='<%# Eval("Sgrade") %>'></asp:Label>.
                                            <asp:Label ID="ensclass" runat="server" Text='<%# Eval("Sclass") %>'></asp:Label>班
                                        </span><br />
                                        <span class="text-xs font-bold text-blue-600"><asp:Label ID="enLabelpspd" runat="server" Text='<%# Eval("Pspd") %>'></asp:Label>词/分</span>
                                    </div>
                                </ItemTemplate>
                            </asp:DataList>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </div>
        
        <asp:Label ID="Labeltop" runat="server" Text="8" Visible="False"></asp:Label>
    </div>
    </form>
</body>
</html>
