<%@ Page Title="" Language="C#" MasterPageFile="~/student/Stud.master" StylesheetTheme="Student" AutoEventWireup="true" CodeFile="mycourse.aspx.cs" Inherits="Student_mycourse" ResponseEncoding="utf-8" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Cphs" Runat="Server">
    <div class="grid grid-cols-1 lg:grid-cols-4 gap-6 lg:gap-8 w-full max-w-full">
        <!-- Main Content (Left Column) -->
        <div class="lg:col-span-3 space-y-8 overflow-hidden min-w-0">
            <!-- New Courses -->
            <div class="space-y-4">
                <h3 class="text-lg font-bold text-slate-800 flex items-center gap-2 border-b border-slate-100 pb-2">
                    <span class="w-1.5 h-5 bg-orange-400 rounded-full inline-block"></span> 未学学案
                </h3>
<div class="overflow-x-auto w-full rounded-xl border border-slate-200 shadow-sm">
    <asp:GridView ID="GridViewnewkc" runat="server" Width="100%" 
        SkinID="GridViewInfo" onrowdatabound="GridViewnewkc_RowDataBound" 
        AutoGenerateColumns="False" 
        EnableModelValidation="True" PageSize="5" AllowPaging="True" 
        onpageindexchanging="GridViewnewkc_PageIndexChanging" 
        CssClass="w-full text-slate-600 bg-white min-w-[500px]">
        <Columns>
            <asp:BoundField DataField="cid"  Visible="false">
            <ItemStyle Width="30px" ForeColor="White" />
            </asp:BoundField>
            <asp:BoundField DataField="Cclass" HeaderText="分类" >
            <HeaderStyle HorizontalAlign="Left" CssClass="bg-slate-50 font-semibold px-4 py-3" />
            <ItemStyle Width="80px" HorizontalAlign="Left" CssClass="px-4 py-2 text-sm text-slate-500 font-medium" />
            </asp:BoundField>
            <asp:TemplateField>
                <ItemTemplate>
                    <div class="flex justify-center"><asp:Image ID="ImageLeaf" runat="server" ImageUrl="~/images/leaf.gif" /></div>
                </ItemTemplate>
                <ItemStyle Width="60px" />
            </asp:TemplateField>
            <asp:HyperLinkField DataNavigateUrlFields="cid" 
                DataNavigateUrlFormatString="~/student/showcourse.aspx?cid={0}" DataTextField="ctitle" 
                HeaderText="学案名称" >
            <HeaderStyle HorizontalAlign="Left" CssClass="bg-slate-50 font-semibold px-4 py-3" />
            <ItemStyle HorizontalAlign="Left" CssClass="px-4 py-2 font-medium text-slate-800 hover:text-blue-600 transition-colors" />
            </asp:HyperLinkField>
            <asp:BoundField DataField="Cdate" HeaderText="发布日期" >
            <HeaderStyle HorizontalAlign="Left" CssClass="bg-slate-50 font-semibold px-4 py-3" />
            <ItemStyle HorizontalAlign="Left" Width="120px" CssClass="px-4 py-2 text-sm text-slate-500" />
            </asp:BoundField>
        </Columns>
       <PagerTemplate>
            <div class="flex items-center justify-between px-4 py-3 bg-slate-50 border-t border-slate-200 sm:px-6">
                <div class="text-sm text-slate-500">
                第<asp:Label ID="lblPageIndex" runat="server" CssClass="font-medium text-slate-900 mx-1" 
                    Text="<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>"></asp:Label>
                页 / 共<asp:Label ID="lblPageCount" runat="server" CssClass="font-medium text-slate-900 mx-1" 
                    Text="<%# ((GridView)Container.Parent.Parent).PageCount %>"></asp:Label>
                页 
                </div>
                <div class="flex gap-2">
                <asp:LinkButton ID="btnFirst" runat="server" CausesValidation="False" 
                    CommandArgument="First" CommandName="Page" CssClass="px-3 py-1 text-sm border border-slate-300 rounded-md hover:bg-slate-100 transition text-slate-600" Text="首页"></asp:LinkButton>
                <asp:LinkButton ID="btnPrev" runat="server" CausesValidation="False" 
                    CommandArgument="Prev" CommandName="Page" CssClass="px-3 py-1 text-sm border border-slate-300 rounded-md hover:bg-slate-100 transition text-slate-600" Text="上一页"></asp:LinkButton>
                <asp:LinkButton ID="btnNext" runat="server" CausesValidation="False" 
                    CommandArgument="Next" CommandName="Page" CssClass="px-3 py-1 text-sm border border-slate-300 rounded-md hover:bg-slate-100 transition text-slate-600" Text="下一页"></asp:LinkButton>
                <asp:LinkButton ID="btnLast" runat="server" CausesValidation="False" 
                    CommandArgument="Last" CommandName="Page" CssClass="px-3 py-1 text-sm border border-slate-300 rounded-md hover:bg-slate-100 transition text-slate-600" Text="尾页"></asp:LinkButton>
                </div>
            </div>
        </PagerTemplate>
        <RowStyle Height="40px" CssClass="border-b border-slate-100 hover:bg-slate-50 transition" />
    </asp:GridView>
</div>
            </div>

            <!-- Done Courses -->
            <div class="space-y-4">
                <h3 class="text-lg font-bold text-slate-800 flex items-center gap-2 border-b border-slate-100 pb-2">
                    <span class="w-1.5 h-5 bg-green-500 rounded-full inline-block"></span> 已学学案
                </h3>
<div class="overflow-x-auto w-full rounded-xl border border-slate-200 shadow-sm">
       <asp:GridView ID="GridViewdonekc" runat="server" AllowPaging="True" 
           AutoGenerateColumns="False" 
           EnableModelValidation="True" 
           OnPageIndexChanging="GridViewdonekc_PageIndexChanging" 
           onrowdatabound="GridViewdonekc_RowDataBound" SkinID="GridViewInfo" 
           Width="100%" PageSize="5"
           CssClass="w-full text-slate-600 bg-white min-w-[500px]">
           <Columns>
               <asp:BoundField DataField="cid" Visible="false">
               <ItemStyle ForeColor="White" Width="30px" />
               </asp:BoundField>
               <asp:BoundField DataField="Cclass" HeaderText="分类">
               <HeaderStyle HorizontalAlign="Left" CssClass="bg-slate-50 font-semibold px-4 py-3" />
               <ItemStyle HorizontalAlign="Left" Width="80px" CssClass="px-4 py-2 text-sm text-slate-500 font-medium" />
               </asp:BoundField>
                <asp:TemplateField>
                <ItemTemplate>
                    <div class="flex justify-center"><asp:Image ID="ImageLeaf" runat="server" ImageUrl="~/images/fruit.gif" Height="16px" /></div>
                </ItemTemplate>
                <ItemStyle Width="60px" />
            </asp:TemplateField>
               <asp:HyperLinkField DataNavigateUrlFields="cid" 
                   DataNavigateUrlFormatString="~/student/showcourse.aspx?cid={0}" 
                   DataTextField="ctitle" HeaderText="学案名称">
               <HeaderStyle HorizontalAlign="Left" CssClass="bg-slate-50 font-semibold px-4 py-3" />
               <ItemStyle HorizontalAlign="Left" CssClass="px-4 py-2 font-medium text-slate-800 hover:text-green-600 transition-colors" />
               </asp:HyperLinkField>
               <asp:BoundField DataField="cobj" HeaderText="年级" >
               <HeaderStyle HorizontalAlign="Left" CssClass="bg-slate-50 font-semibold px-4 py-3" />
               <ItemStyle Width="60px" CssClass="px-4 py-2 text-sm text-slate-600" />
               </asp:BoundField>
               <asp:BoundField DataField="cterm" HeaderText="学期" >
               <HeaderStyle HorizontalAlign="Left" CssClass="bg-slate-50 font-semibold px-4 py-3" />
               <ItemStyle Width="60px" CssClass="px-4 py-2 text-sm text-slate-600" />
               </asp:BoundField>
           </Columns>
           <PagerTemplate>
                <div class="flex items-center justify-between px-4 py-3 bg-slate-50 border-t border-slate-200 sm:px-6">
                    <div class="text-sm text-slate-500">
                    第<asp:Label ID="lblPageIndex" runat="server" CssClass="font-medium text-slate-900 mx-1" 
                        Text="<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>"></asp:Label>
                    页 / 共<asp:Label ID="lblPageCount" runat="server" CssClass="font-medium text-slate-900 mx-1" 
                        Text="<%# ((GridView)Container.Parent.Parent).PageCount %>"></asp:Label>
                    页 
                    </div>
                    <div class="flex gap-2">
                    <asp:LinkButton ID="btnFirst" runat="server" CausesValidation="False" 
                        CommandArgument="First" CommandName="Page" CssClass="px-3 py-1 text-sm border border-slate-300 rounded-md hover:bg-slate-100 transition text-slate-600" Text="首页"></asp:LinkButton>
                    <asp:LinkButton ID="btnPrev" runat="server" CausesValidation="False" 
                        CommandArgument="Prev" CommandName="Page" CssClass="px-3 py-1 text-sm border border-slate-300 rounded-md hover:bg-slate-100 transition text-slate-600" Text="上一页"></asp:LinkButton>
                    <asp:LinkButton ID="btnNext" runat="server" CausesValidation="False" 
                        CommandArgument="Next" CommandName="Page" CssClass="px-3 py-1 text-sm border border-slate-300 rounded-md hover:bg-slate-100 transition text-slate-600" Text="下一页"></asp:LinkButton>
                    <asp:LinkButton ID="btnLast" runat="server" CausesValidation="False" 
                        CommandArgument="Last" CommandName="Page" CssClass="px-3 py-1 text-sm border border-slate-300 rounded-md hover:bg-slate-100 transition text-slate-600" Text="尾页"></asp:LinkButton>
                    </div>
                </div>
            </PagerTemplate>
           <RowStyle Height="40px" CssClass="border-b border-slate-100 hover:bg-slate-50 transition" />
       </asp:GridView>
</div>
            </div>
        </div>

        <!-- Sidebar (Right Column) -->
        <div class="lg:col-span-1 border border-slate-200/60 bg-slate-50/50 rounded-2xl p-5 shadow-sm flex flex-col items-center space-y-6 self-start top-24 sticky">
            
            <div class="relative w-32 h-32 mt-2">
                <div class="absolute inset-0 bg-gradient-to-tr from-blue-100 to-indigo-100 rounded-full blur-md opacity-70"></div>
                <asp:Image ID="Imageface" runat="server" CssClass="relative w-32 h-32 rounded-full object-cover border-4 border-white shadow-lg z-10" />    
                <div id="DivRank" class="absolute -bottom-3 -right-2 bg-gradient-to-r from-orange-400 to-pink-500 text-white text-xs font-bold px-3 py-1 rounded-full shadow-md border-2 border-white z-20">
                    <asp:Label ID="LabelRank" runat="server"></asp:Label>
                </div>
            </div> 
            
            <div class="w-full space-y-2.5 mt-4">
                <div class="flex items-center justify-between p-2.5 rounded-xl bg-white border border-slate-100 shadow-sm text-sm hover:shadow transition-shadow">
                    <div class="flex items-center gap-2 text-slate-500 font-medium">
                        <svg class="w-4 h-4 text-indigo-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 6H5a2 2 0 00-2 2v9a2 2 0 002 2h14a2 2 0 002-2V8a2 2 0 00-2-2h-5m-4 0V5a2 2 0 114 0v1m-4 0a2 2 0 104 0m-5 8a2 2 0 100-4 2 2 0 000 4zm0 0c1.306 0 2.417.835 2.83 2M9 14a3.001 3.001 0 00-2.83 2M15 11h3m-3 4h2"></path></svg>
                        学号
                    </div>
                    <div class="font-semibold text-slate-800"><asp:Label ID="snum" runat="server" ></asp:Label></div>
                </div>
                
                <div class="flex items-center justify-between p-2.5 rounded-xl bg-white border border-slate-100 shadow-sm text-sm hover:shadow transition-shadow">
                    <div class="flex items-center gap-2 text-slate-500 font-medium">
                        <svg class="w-4 h-4 text-emerald-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4"></path></svg>
                        班级
                    </div>
                    <div class="font-semibold text-slate-800"><asp:Label ID="sclass" runat="server" ></asp:Label></div>
                </div>
                
                <div class="flex items-center justify-between p-2.5 rounded-xl bg-white border border-slate-100 shadow-sm text-sm hover:shadow transition-shadow">
                    <div class="flex items-center gap-2 text-slate-500 font-medium">
                        <svg class="w-4 h-4 text-blue-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"></path></svg>
                        姓名
                    </div>
                    <div class="font-semibold text-slate-800"><asp:Label ID="sname" runat="server" ></asp:Label></div>
                </div>  
                
                <div class="flex items-center justify-between p-2.5 rounded-xl bg-white border border-slate-100 shadow-sm text-sm hover:shadow transition-shadow">
                    <div class="flex items-center gap-2 text-slate-500 font-medium">
                        <svg class="w-4 h-4 text-amber-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z"></path></svg>
                        组长
                    </div>
                    <div class="font-semibold text-slate-800"><asp:Label ID="sleadername" runat="server" ></asp:Label></div>
                </div> 

                <div class="flex items-center justify-between p-2.5 rounded-xl bg-white border border-slate-100 shadow-sm text-xs hover:shadow transition-shadow">
                    <div class="flex items-center gap-2 text-slate-400 font-medium">
                        <svg class="w-3.5 h-3.5 text-slate-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M21 12a9 9 0 01-9 9m9-9a9 9 0 00-9-9m9 9H3m9 9a9 9 0 01-9-9m9 9c1.657 0 3-4.03 3-9s-1.343-9-3-9m0 18c-1.657 0-3-4.03-3-9s1.343-9 3-9m-9 9a9 9 0 019-9"></path></svg>
                        终端 IP
                    </div>
                    <div class="font-mono text-slate-500"><asp:Label ID="Labelip" runat="server" SkinID="LabelSize8" ></asp:Label></div>
                </div> 
            </div>
            
            <asp:Label ID="LabelCids" runat="server" ForeColor="White" Visible="false"></asp:Label>
        </div>
    </div>
</asp:Content>
