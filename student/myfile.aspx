<%@ Page Title="" Language="C#" MasterPageFile="~/student/Stud.master" StylesheetTheme="Student" AutoEventWireup="true" CodeFile="myfile.aspx.cs" Inherits="Student_myfile" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cphs" Runat="Server">


<div class="space-y-6 w-full">
    <!-- Page Header -->
    <div class="flex items-center gap-3">
        <div class="w-10 h-10 rounded-xl bg-indigo-100 flex items-center justify-center flex-shrink-0">
            <svg class="w-5 h-5 text-indigo-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4"/>
            </svg>
        </div>
        <div>
            <h2 class="text-xl font-bold text-slate-800 leading-tight">在线资源</h2>
            <p class="text-sm text-slate-500 mt-0.5">浏览并下载老师分享的学习资源</p>
        </div>
    </div>

    <div class="grid grid-cols-1 lg:grid-cols-4 gap-6 lg:gap-8 w-full max-w-full">
        <!-- Main Content -->
        <div class="lg:col-span-3 space-y-4 overflow-hidden min-w-0">

            <div class="bg-white rounded-2xl border border-slate-200/80 shadow-sm overflow-hidden">
                <!-- Table header bar -->
                <div class="flex items-center justify-between px-5 py-4 border-b border-slate-100 bg-gradient-to-r from-indigo-50/60 to-transparent">
                    <div class="flex items-center gap-2">
                        <svg class="w-4 h-4 text-indigo-500" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z"/>
                        </svg>
                        <span class="font-semibold text-slate-700 text-sm">资源列表</span>
                    </div>
                    <span class="text-xs text-slate-400 bg-slate-100 px-2 py-1 rounded-full">点击标题下载</span>
                </div>

                <div id="GVSoftWrapper" class="overflow-x-auto w-full">
                    <asp:GridView ID="GVSoft" runat="server" AllowPaging="True" 
                        AutoGenerateColumns="False" 
                        OnPageIndexChanging="GVSoft_PageIndexChanging" 
                        OnRowDataBound="GVSoft_RowDataBound" Width="100%" SkinID="GridViewInfo" 
                        PageSize="20" EnableModelValidation="True" CellPadding="0"
                        CssClass="w-full text-slate-600 bg-white min-w-[560px]">
                        <AlternatingRowStyle CssClass="bg-slate-50/50" />
                        <Columns>
                            <asp:BoundField HeaderText="#">
                                <HeaderStyle CssClass="bg-slate-50 text-slate-500 font-semibold text-xs uppercase tracking-wide px-4 py-3 w-12 text-center" />
                                <ItemStyle CssClass="text-center font-medium text-slate-400 text-sm px-4 py-3" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Fclass" HeaderText="分类">
                                <HeaderStyle CssClass="bg-slate-50 text-slate-500 font-semibold text-xs uppercase tracking-wide px-3 py-3 w-20" />
                                <ItemStyle CssClass="text-center px-3 py-3" />
                            </asp:BoundField>
                            <asp:HyperLinkField DataNavigateUrlFields="fid" 
                                DataNavigateUrlFormatString="downfile.aspx?fid={0}" HeaderText="资源标题" 
                                DataTextField="Ftitle">
                                <HeaderStyle HorizontalAlign="Left" CssClass="bg-slate-50 text-slate-500 font-semibold text-xs uppercase tracking-wide px-4 py-3" />
                                <ItemStyle HorizontalAlign="Left" CssClass="px-4 py-3 font-medium text-indigo-600 hover:text-indigo-800 transition-colors" />
                            </asp:HyperLinkField>
                            <asp:BoundField DataField="Ffiletype" HeaderText="格式">
                                <HeaderStyle CssClass="bg-slate-50 text-slate-500 font-semibold text-xs uppercase tracking-wide px-3 py-3 w-20 text-center" />
                                <ItemStyle CssClass="text-center px-3 py-3" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Fhit" HeaderText="下载">
                                <HeaderStyle CssClass="bg-slate-50 text-slate-500 font-semibold text-xs uppercase tracking-wide px-3 py-3 w-20 text-center" />
                                <ItemStyle CssClass="text-center px-3 py-3" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Fdate" HeaderText="更新日期">
                                <HeaderStyle CssClass="bg-slate-50 text-slate-500 font-semibold text-xs uppercase tracking-wide px-4 py-3 hidden md:table-cell w-28 text-center" />
                                <ItemStyle Width="120px" CssClass="text-center text-xs text-slate-400 hidden md:table-cell px-4 py-3" />
                            </asp:BoundField>
                        </Columns>
                        <pagertemplate>
                            <div class="flex items-center justify-between px-5 py-3 bg-slate-50 border-t border-slate-100">
                                <div class="text-xs text-slate-500">
                                    第 <asp:Label ID="lblPageIndex" runat="server" CssClass="font-semibold text-slate-700"
                                        text="<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>" /> 页 / 共
                                    <asp:Label ID="lblPageCount" runat="server" CssClass="font-semibold text-slate-700"
                                        text="<%# ((GridView)Container.Parent.Parent).PageCount %>" /> 页
                                </div>
                                <div class="flex gap-1.5">
                                    <asp:LinkButton ID="btnFirst" runat="server" causesvalidation="False" commandargument="First" commandname="Page"
                                        CssClass="px-3 py-1.5 text-xs font-medium border border-slate-200 rounded-lg hover:bg-indigo-50 hover:border-indigo-300 hover:text-indigo-600 transition text-slate-600" text="首页" />
                                    <asp:LinkButton ID="btnPrev" runat="server" causesvalidation="False" commandargument="Prev" commandname="Page"
                                        CssClass="px-3 py-1.5 text-xs font-medium border border-slate-200 rounded-lg hover:bg-indigo-50 hover:border-indigo-300 hover:text-indigo-600 transition text-slate-600" text="上一页" />
                                    <asp:LinkButton ID="btnNext" runat="server" causesvalidation="False" commandargument="Next" commandname="Page"
                                        CssClass="px-3 py-1.5 text-xs font-medium border border-slate-200 rounded-lg hover:bg-indigo-50 hover:border-indigo-300 hover:text-indigo-600 transition text-slate-600" text="下一页" />
                                    <asp:LinkButton ID="btnLast" runat="server" causesvalidation="False" commandargument="Last" commandname="Page"
                                        CssClass="px-3 py-1.5 text-xs font-medium border border-slate-200 rounded-lg hover:bg-indigo-50 hover:border-indigo-300 hover:text-indigo-600 transition text-slate-600" text="尾页" />
                                </div>
                            </div>
                        </pagertemplate>
                        <RowStyle CssClass="border-b border-slate-100 hover:bg-indigo-50/30 transition-colors" />
                    </asp:GridView>
                </div>
            </div>
        </div>

        <!-- Sidebar -->
        <div class="lg:col-span-1 space-y-5 self-start top-24 sticky">
            <!-- Category Card -->
            <div class="bg-white border border-slate-200/80 rounded-2xl shadow-sm overflow-hidden">
                <div class="px-4 py-4 bg-gradient-to-r from-indigo-50 to-purple-50/40 border-b border-indigo-100/60 flex items-center gap-2">
                    <svg class="w-4 h-4 text-indigo-500 flex-shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M3 7v10a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-6l-2-2H5a2 2 0 00-2 2z"/>
                    </svg>
                    <span class="font-bold text-indigo-800 text-sm">资源分类</span>
                </div>
                <div class="p-3">
                    <!-- All resources link -->
                    <a href="myfile.aspx" class="cat-btn mb-1">
                        <svg class="cat-btn-icon w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 6h16M4 10h16M4 14h16M4 18h16"/>
                        </svg>
                        全部资源
                    </a>
                    <asp:GridView ID="GVcategory" runat="server" AutoGenerateColumns="False" 
                        EnableModelValidation="True" 
                        ShowHeader="False" 
                        SkinID="GridViewMission" Width="100%" DataKeyNames="yid" 
                        onrowdatabound="GVcategory_RowDataBound"
                        CssClass="w-full" GridLines="None">
                        <Columns>
                            <asp:HyperLinkField DataNavigateUrlFields="yid" 
                                DataNavigateUrlFormatString="~/student/myfile.aspx?yid={0}" 
                                DataTextField="Ytitle" Target="_self">
                                <ItemStyle HorizontalAlign="Left" CssClass="py-0.5" />
                            </asp:HyperLinkField>
                        </Columns>
                        <RowStyle CssClass="" />
                    </asp:GridView>
                </div>
            </div>

            <!-- Tips card -->
            <div class="bg-amber-50 border border-amber-200/60 rounded-2xl p-4 shadow-sm">
                <div class="flex items-start gap-3">
                    <div class="w-8 h-8 rounded-lg bg-amber-100 flex items-center justify-center flex-shrink-0 mt-0.5">
                        <svg class="w-4 h-4 text-amber-600" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"/>
                        </svg>
                    </div>
                    <div>
                        <p class="text-xs font-semibold text-amber-800 mb-1">使用提示</p>
                        <p class="text-xs text-amber-700 leading-relaxed">点击资源标题即可下载文件。如遇无法下载，请联系老师。</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>


    <script type="text/javascript">
        window.__myfileConfig = {
            gVSoftId: '<%= GVSoft.ClientID %>',
            gVcategoryId: '<%= GVcategory.ClientID %>'
        };
    </script>
    <script type="text/javascript" src="../js/myfile.js"></script>
</asp:Content>
