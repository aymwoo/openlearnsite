<%@ Page Language="C#" AutoEventWireup="true"  StylesheetTheme="Student" CodeFile="autonomiccategory.aspx.cs" Inherits="Student_autonomiccategory" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
        <meta charset="utf-8" />
<title></title>   
    <link href="../App_Themes/student/StyleSheet.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .divcate{margin:auto; padding: 2px; background-color: #E0ECFE; font-size: 11pt; font-weight: bold; text-align: left; height: 24px; width:360px;}
        .licss{font-size: 11pt; height:30px; width:360px; text-align: left; border-width: 1px; border-bottom-style: dashed; border-color: #CCCCCC}
        .licss1{font-size: 11pt; height:24px; width:98%; text-align: left; border-width: 1px; border-bottom-style: dashed; border-color: #CCCCCC}
        .licss2{font-size: 11pt; height:24px; width:98%; text-align: left; border-width: 1px; border-bottom-style: dashed; border-color: #CCCCCC; background-color:#eeeeee}
    </style>
    
    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
      <div class="studmasterhead">
            <div class="banner"> <img alt="" src="../images/autonomic.gif" /></div>
             <div class="path"></div>
      <div class="w-full max-w-6xl mx-auto">
        <div class="grid grid-cols-1 lg:grid-cols-4 gap-6 lg:gap-8 w-full max-w-full text-left p-4">
            <!-- Main Content -->
            <div class="lg:col-span-3 space-y-6 overflow-hidden min-w-0">
                <div class="bg-white rounded-2xl shadow-sm border border-slate-200/60 p-6 overflow-x-auto min-w-0">
                    <h3 class="text-lg font-bold text-slate-800 flex items-center gap-2 border-b border-slate-100 pb-2 mb-4">
                        <span class="w-1.5 h-5 bg-blue-500 rounded-full inline-block"></span> 资源分类列表
                    </h3>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
                        EnableModelValidation="True" Width="100%" AllowPaging="True" 
                        CellPadding="4" onrowdatabound="GridView1_RowDataBound" PageSize="20" 
                        onpageindexchanging="GridView1_PageIndexChanging" 
                        CssClass="w-full text-sm text-slate-700 min-w-[700px] border border-slate-200 rounded-lg overflow-hidden shadow-sm">
                        <HeaderStyle CssClass="bg-slate-50 border-b border-slate-200 font-bold" />
                        <Columns>
                            <asp:BoundField HeaderText="序号" >
                                <HeaderStyle CssClass="py-2.5 px-3 text-center" />
                                <ItemStyle CssClass="py-2 text-center border-b border-slate-100 text-slate-500" />
                            </asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="flex justify-center">
                                        <asp:Image ID="Imagetype" runat="server" ImageUrl='<%# GetfileType(Eval("Atype").ToString()) %>' CssClass="w-5 h-5 object-contain" />
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle CssClass="py-2.5" />
                                <ItemStyle CssClass="py-2 border-b border-slate-100" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="作品">
                                <ItemTemplate>
                                    <asp:HyperLink ID="HyperLinkUrl" runat="server" 
                                        NavigateUrl='<%# GetdownUrl(Eval("Aurl").ToString()) %>' 
                                        Text='<%# Eval("Ftitle") %>'  Target="_blank" CssClass="font-medium text-slate-800 hover:text-blue-600 transition truncate block max-w-sm"></asp:HyperLink>
                                </ItemTemplate>
                                <HeaderStyle CssClass="py-2.5 px-3 text-left" />
                                <ItemStyle HorizontalAlign="Left" CssClass="py-2 px-3 border-b border-slate-100" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="Ascore" HeaderText="学分" >
                                <HeaderStyle CssClass="py-2.5 px-3 text-center" />
                                <ItemStyle CssClass="py-2 text-center border-b border-slate-100 font-bold text-orange-500" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="姓名">
                                <ItemTemplate>
                                    <asp:Label ID="LabelAname" runat="server" Text='<%# HttpUtility.UrlDecode(Eval("Aname").ToString()) %>' CssClass="text-slate-600 font-medium"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="py-2.5 px-3 text-left" />
                                <ItemStyle CssClass="py-2 px-3 border-b border-slate-100" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="日期">
                                <ItemTemplate>
                                    <asp:Label ID="LabelAdate" runat="server" Text='<%# Bind("Adate") %>' CssClass="text-slate-500 text-xs"></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="py-2.5 px-3 text-left hidden sm:table-cell" />
                                <ItemStyle CssClass="py-2 px-3 border-b border-slate-100 hidden sm:table-cell" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Image ID="Imagegood" runat="server" ImageUrl="~/images/new_none.gif" CssClass="mx-auto" />
                                </ItemTemplate>
                                <ItemStyle CssClass="py-2 border-b border-slate-100" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBoxgood" runat="server" Checked='<%# Bind("Agood") %>' 
                                        Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerTemplate>
                            <div class="flex items-center justify-center gap-2 py-3 bg-slate-50">
                                <span class="text-xs text-slate-500 mr-2">
                                第 <asp:Label ID="lblPageIndex" runat="server" Text="<%# ((GridView)Container.Parent.Parent).PageIndex + 1  %>" CssClass="font-bold text-slate-800" /> 页 
                                / 共 <asp:Label ID="lblPageCount" runat="server" Text="<%# ((GridView)Container.Parent.Parent).PageCount  %>" CssClass="font-bold text-slate-800" /> 页
                                </span>              
                                <asp:LinkButton ID="btnFirst" runat="server" CausesValidation="False" CommandArgument="First"
                                    CommandName="Page" CssClass="px-2 py-1 text-xs border border-slate-300 rounded hover:bg-slate-200 text-slate-600" Text="首页" />
                                <asp:LinkButton ID="btnPrev" runat="server" CausesValidation="False" CommandArgument="Prev"
                                    CommandName="Page" CssClass="px-2 py-1 text-xs border border-slate-300 rounded hover:bg-slate-200 text-slate-600" Text="上一页" />
                                <asp:LinkButton ID="btnNext" runat="server" CausesValidation="False" CommandArgument="Next"
                                    CommandName="Page" CssClass="px-2 py-1 text-xs border border-slate-300 rounded hover:bg-slate-200 text-slate-600" Text="下一页" />
                                <asp:LinkButton ID="btnLast" runat="server" CausesValidation="False" CommandArgument="Last"
                                    CommandName="Page" CssClass="px-2 py-1 text-xs border border-slate-300 rounded hover:bg-slate-200 text-slate-600" Text="尾页" />
                            </div>
                        </PagerTemplate>
                        <RowStyle CssClass="hover:bg-slate-50 transition" />
                    </asp:GridView>
                </div>
            </div>
            
            <!-- Sidebar -->
            <div class="lg:col-span-1 space-y-6 self-start">
                <div class="bg-slate-50 border border-slate-100 rounded-2xl p-5 shadow-sm">
                    <h4 class="text-slate-700 font-bold mb-4 flex items-center gap-2 border-b border-slate-200/60 pb-2">
                        <svg class="w-5 h-5 text-yellow-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11.049 2.927c.3-.921 1.603-.921 1.902 0l1.519 4.674a1 1 0 00.95.69h4.915c.969 0 1.371 1.24.588 1.81l-3.976 2.888a1 1 0 00-.363 1.118l1.518 4.674c.3.922-.755 1.688-1.538 1.118l-3.976-2.888a1 1 0 00-1.176 0l-3.976 2.888c-.783.57-1.838-.197-1.538-1.118l1.518-4.674a1 1 0 00-.363-1.118l-3.976-2.888c-.784-.57-.38-1.81.588-1.81h4.914a1 1 0 00.951-.69l1.519-4.674z"></path></svg>
                        优秀作品榜
                    </h4>
                    
                    <ul class="space-y-2 mb-6">
                        <asp:Repeater ID="RepMy" runat="server" >
                            <ItemTemplate>
                                <li class="border-b border-slate-200/50 pb-2">
                                    <a href='<%#Viewurl(Eval("Aurl").ToString())%>' target="_blank" class="text-slate-600 hover:text-blue-600 transition font-medium text-sm block truncate"><%#Strcut( Eval("Ftitle").ToString())%></a>
                                </li>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <li class="border-b border-slate-200/50 pb-2">
                                    <a href='<%#Viewurl(Eval("Aurl").ToString())%>' target="_blank" class="text-slate-600 hover:text-blue-600 transition font-medium text-sm block truncate"><%#Strcut( Eval("Ftitle").ToString())%></a>
                                </li>
                            </AlternatingItemTemplate>
                        </asp:Repeater>
                    </ul>
                    
                    <asp:HyperLink ID="HyperLink1" runat="server" CssClass="w-full text-center px-4 py-2.5 bg-gradient-to-r from-emerald-500 to-teal-600 text-white font-bold rounded-xl hover:from-emerald-600 hover:to-teal-700 transition duration-300 shadow-md block" 
                        NavigateUrl="~/student/autonomic.aspx" Target="_self">在线资源</asp:HyperLink>
                </div>
            </div>
        </div>
</div>      
        </div>
       </div>
    </form>
</body>
</html>
