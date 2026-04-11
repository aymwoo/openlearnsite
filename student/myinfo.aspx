<%@ Page Title="" Language="C#" MasterPageFile="~/student/Stud.master" StylesheetTheme="Student" AutoEventWireup="true" CodeFile="myinfo.aspx.cs" Inherits="Student_myinfo" ResponseEncoding="utf-8" %>

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
<asp:Panel ID="PanelComposedCourseSummary" runat="server" Visible="False" CssClass="rounded-xl border border-blue-100 bg-blue-50/60 px-4 py-3 text-sm text-slate-700">
    <div class="font-bold text-slate-800 mb-2">整课活动进度</div>
    <asp:Literal ID="LiteralComposedCourseSummary" runat="server"></asp:Literal>
</asp:Panel>
<div class="overflow-x-auto w-full rounded-xl border border-slate-200 shadow-sm">
       <asp:GridView ID="GridViewdonekc" runat="server" AllowPaging="True" 
           AutoGenerateColumns="False" 
           EnableModelValidation="True" 
           OnPageIndexChanging="GridViewdonekc_PageIndexChanging" 
           onrowdatabound="GridViewdonekc_RowDataBound" SkinID="GridViewInfo" 
           Width="100%" PageSize="5" DataKeyNames="Cid"
           CssClass="w-full text-slate-600 bg-white min-w-[500px]">
           <Columns>
               <asp:BoundField DataField="Cid" Visible="false">
               <ItemStyle ForeColor="White" Width="30px" />
               </asp:BoundField>
                <asp:TemplateField>
                <ItemTemplate>
                    <div class="flex justify-center"><asp:Image ID="ImageLeaf" runat="server" ImageUrl="~/images/fruit.gif" Height="16px" /></div>
                </ItemTemplate>
                <ItemStyle Width="60px" />
            </asp:TemplateField>
               <asp:HyperLinkField DataNavigateUrlFields="Cid" 
                   DataNavigateUrlFormatString="~/student/showcourse.aspx?cid={0}" 
                   DataTextField="ctitle" HeaderText="学案名称">
               <HeaderStyle HorizontalAlign="Left" CssClass="bg-slate-50 font-semibold px-4 py-3" />
               <ItemStyle HorizontalAlign="Left" CssClass="px-4 py-2 font-medium text-slate-800 hover:text-green-600 transition-colors" />
               </asp:HyperLinkField>
               <asp:TemplateField HeaderText="学习进度">
                <ItemTemplate>
                    <asp:Literal ID="Process" runat="server"></asp:Literal>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" CssClass="bg-slate-50 font-semibold px-4 py-3" />
                <ItemStyle HorizontalAlign="Left" CssClass="px-4 py-2" />
               </asp:TemplateField>
            <asp:BoundField DataField="Cdate" HeaderText="完成日期" >
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
           <RowStyle Height="40px"  CssClass="border-b border-slate-100 hover:bg-slate-50 transition" />
       </asp:GridView>
</div>
            </div>

            <!-- Active Students Board -->
            <div class="space-y-4">
                <h3 class="text-lg font-bold text-slate-800 flex items-center gap-2 border-b border-slate-100 pb-2 mt-4">
                    <span class="w-1.5 h-5 bg-blue-500 rounded-full inline-block"></span> 今天签到的同学
                </h3>
                <div class="bg-indigo-50/50 rounded-2xl p-4 border border-indigo-100/60 w-full overflow-x-auto min-w-0">
                    <asp:DataList ID="DataListonline" runat="server" DataKeyField="Qid"
                                RepeatLayout="Flow"
                        onitemdatabound="DataListonline_ItemDataBound" CssClass="flex flex-nowrap md:flex-wrap gap-3">
                        <ItemTemplate>
                            <div class="rounded-lg border border-white bg-white/70 shadow-sm overflow-hidden flex flex-col items-center p-1.5 hover:shadow-md hover:scale-105 transition-all w-24 shrink-0">
                                <div class="w-full text-center py-1 bg-blue-50/80 rounded mb-1">
                                    <asp:HyperLink ID="HyperQname" runat="server" CssClass="text-sm font-semibold text-slate-800 hover:text-blue-600 transition block truncate cursor-pointer"
                                        Text='<%# Eval("Sname") %>' ToolTip='<%# Eval("Qip") %>' ></asp:HyperLink>
                                </div>
                                <div class="flex flex-col items-center justify-center">
                                    <asp:Image ID="Imageflag" runat="server" CssClass="my-1 rounded-sm shadow-sm opacity-90" />
                                    <asp:Label ID="Labeltime" runat="server" Text='<%# Eval("Qdate") %>' CssClass="text-xs text-slate-400 block truncate w-full text-center mt-1"></asp:Label>
                                </div>
                                <asp:Label ID="LabelSleader" runat="server" Text='<%# Eval("Sleader") %>' Visible="false" ></asp:Label>
                                <asp:Label ID="LabelSgroup" runat="server" Text='<%# Eval("Sgroup") %>' Visible="false" ></asp:Label>
                                <asp:Label ID="LabelQnum" runat="server" Text='<%# Eval("Qnum") %>' Visible="false" ></asp:Label>
                            </div>
                        </ItemTemplate>
                    </asp:DataList>
                </div>
            </div>
        </div>

        <!-- Sidebar (Right Column) -->
        <div class="lg:col-span-1 border border-slate-200/60 bg-slate-50/50 rounded-2xl p-5 shadow-sm flex flex-col items-center space-y-4 self-start top-24 sticky">

            <!-- Student Avatar + Brief Info Card -->
            <div class="w-full bg-white border border-slate-200 rounded-xl p-4 flex flex-col items-center gap-3">
                <!-- Avatar with rank badge -->
                <div style="position:relative; flex-shrink:0;">
                    <asp:Image ID="Imageface" runat="server" style="width:72px; height:72px; border-radius:50%; object-fit:cover; border:3px solid #e0e7ff; box-shadow:0 4px 12px rgba(79,70,229,0.15);" />
                    <div style="position:absolute; bottom:-3px; right:-3px; background:linear-gradient(135deg,#fb923c,#ec4899); color:#fff; font-size:9px; font-weight:700; padding:1px 6px; border-radius:9999px; box-shadow:0 1px 3px rgba(0,0,0,0.18); border:2px solid #fff; line-height:1.5;">
                        <asp:Label ID="LabelRank" runat="server"></asp:Label>
                    </div>
                </div>
                <!-- Name -->
                <div class="text-base font-bold text-slate-800 tracking-tight">
                    <asp:Label ID="sname" runat="server"></asp:Label>
                </div>
                <!-- Info chips -->
                <div style="display:flex; flex-direction:column; gap:0.375rem; width:100%;">
                    <div style="display:flex; align-items:center; gap:0.375rem; padding:0.3rem 0.625rem; border-radius:0.5rem; background:#f8fafc; border:1px solid #e2e8f0; font-size:0.75rem; color:#475569;">
                        <svg style="width:0.75rem; height:0.75rem; color:#818cf8; flex-shrink:0;" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 6H5a2 2 0 00-2 2v9a2 2 0 002 2h14a2 2 0 002-2V8a2 2 0 00-2-2h-5m-4 0V5a2 2 0 114 0v1m-4 0a2 2 0 104 0m-5 8a2 2 0 100-4 2 2 0 000 4zm0 0c1.306 0 2.417.835 2.83 2M9 14a3.001 3.001 0 00-2.83 2M15 11h3m-3 4h2"></path></svg>
                        <asp:Label ID="snum" runat="server" style="color:#334155; font-weight:600;"></asp:Label>
                    </div>
                    <div style="display:flex; align-items:center; gap:0.375rem; padding:0.3rem 0.625rem; border-radius:0.5rem; background:#f8fafc; border:1px solid #e2e8f0; font-size:0.75rem; color:#475569;">
                        <svg style="width:0.75rem; height:0.75rem; color:#34d399; flex-shrink:0;" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4"></path></svg>
                        <asp:Label ID="sclass" runat="server" style="color:#334155; font-weight:600;"></asp:Label>
                    </div>
                    <div style="display:flex; align-items:center; gap:0.375rem; padding:0.3rem 0.625rem; border-radius:0.5rem; background:#f8fafc; border:1px solid #e2e8f0; font-size:0.75rem; color:#475569;">
                        <svg style="width:0.75rem; height:0.75rem; color:#fbbf24; flex-shrink:0;" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z"></path></svg>
                        <asp:HyperLink ID="HLgroup" runat="server" style="color:#4f46e5; font-weight:700; text-decoration:none;">加入小组</asp:HyperLink>
                    </div>
                    <div style="display:flex; align-items:center; gap:0.375rem; padding:0.3rem 0.625rem; border-radius:0.5rem; background:#f8fafc; border:1px solid #e2e8f0; font-size:0.75rem; color:#475569; overflow:hidden; text-overflow:ellipsis; white-space:nowrap;">
                        <svg style="width:0.75rem; height:0.75rem; color:#94a3b8; flex-shrink:0;" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197m13.5-9a2.5 2.5 0 11-5 0 2.5 2.5 0 015 0z"></path></svg>
                        <asp:Label ID="Labelteam" runat="server" style="color:#475569; font-weight:500; overflow:hidden; text-overflow:ellipsis;"></asp:Label>
                    </div>
                    <div style="display:flex; align-items:center; gap:0.375rem; padding:0.3rem 0.625rem; border-radius:0.5rem; background:#f8fafc; border:1px solid #e2e8f0; font-size:0.75rem; color:#475569;">
                        <svg style="width:0.75rem; height:0.75rem; color:#22c55e; flex-shrink:0;" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m5 2a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
                        今日表现：<asp:Label ID="lblTodayAttitude" runat="server" style="color:#166534; font-weight:700;"></asp:Label>
                    </div>
                    <div style="display:flex; align-items:center; gap:0.375rem; padding:0.3rem 0.625rem; border-radius:0.5rem; background:#f8fafc; border:1px solid #e2e8f0; font-size:0.75rem; color:#475569;">
                        <svg style="width:0.75rem; height:0.75rem; color:#a855f7; flex-shrink:0;" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11.049 2.927c.3-.921 1.603-.921 1.902 0l1.519 4.674a1 1 0 00.95.69h4.915c.969 0 1.371 1.24.588 1.81l-3.976 2.888a1 1 0 00-.363 1.118l1.518 4.674c.3.922-.755 1.688-1.538 1.118l-3.976-2.888a1 1 0 00-1.176 0l-3.976 2.888c-.783.57-1.838-.197-1.538-1.118l1.518-4.674a1 1 0 00-.363-1.118L2.98 9.29c-.784-.57-.38-1.81.588-1.81h4.914a1 1 0 00.951-.69l1.519-4.674z"></path></svg>
                        学分：<asp:Label ID="sscore" runat="server" style="color:#334155; font-weight:700;"></asp:Label>
                        <span style="color:#cbd5e1;">|</span>
                        表现分：<asp:Label ID="sattitude" runat="server" style="color:#334155; font-weight:700;"></asp:Label>
                    </div>
                    <div style="display:flex; align-items:center; gap:0.375rem; padding:0.3rem 0.625rem; border-radius:0.5rem; background:#f8fafc; border:1px solid #e2e8f0; font-size:0.75rem; color:#475569;">
                        <svg style="width:0.75rem; height:0.75rem; color:#f59e0b; flex-shrink:0;" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8c-1.657 0-3 1.343-3 3v5h6v-5c0-1.657-1.343-3-3-3zm0 0V5m0 0l-2 2m2-2 2 2"></path></svg>
                        总分：<asp:Label ID="lblTotalScore" runat="server" style="color:#92400e; font-weight:700;"></asp:Label>
                        <span style="color:#cbd5e1;">|</span>
                        座位：<asp:Label ID="Labelseat" runat="server" style="color:#334155; font-weight:700;"></asp:Label>
                    </div>
                </div>
            </div>

            <!-- 我的资料 / 系统退出 buttons -->
            <div class="flex flex-col sm:flex-row lg:flex-col gap-3 w-full">
                <asp:Button ID="BtnProfile" runat="server" OnClick="BtnProfile_Click"
                    Text="我的资料" CausesValidation="False" OnClientClick="openModernGroupModal(); return false;"
                    CssClass="flex-1 w-full flex justify-center py-2.5 px-4 border border-slate-300 rounded-xl text-sm font-semibold text-slate-700 bg-white hover:bg-slate-50 transition-all duration-300 shadow-sm cursor-pointer" />
                <asp:Button ID="BtnExit" runat="server" onclick="BtnExit_Click" 
                    Enabled="False" Text="" 
                    CssClass="flex-1 w-full flex justify-center py-2.5 px-4 rounded-xl text-sm font-semibold text-white bg-red-500 hover:bg-red-600 focus:ring-2 focus:ring-offset-2 focus:ring-red-500 transition-all duration-300 shadow-md border-0 cursor-pointer disabled:opacity-50 disabled:cursor-not-allowed" />
            </div>

            <!-- 最新作品评语 -->
            <div class="w-full bg-orange-50 border border-orange-100 rounded-xl p-4">
                <div class="flex items-center gap-2 text-orange-600 font-semibold text-sm mb-2">
                    <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 10h.01M12 10h.01M16 10h.01M9 16H5a2 2 0 01-2-2V6a2 2 0 012-2h14a2 2 0 012 2v8a2 2 0 01-2 2h-5l-5 5v-5z"></path></svg>
                    最新作品评语
                </div>
                <div class="text-sm text-slate-700 leading-relaxed min-h-[3rem] italic p-1 border-l-2 border-orange-300 ml-1 pl-2 font-medium">
                    <asp:Label ID="LabelWself" runat="server" ></asp:Label>
                </div>
                <div class="mt-3 text-right">
                    <asp:HyperLink ID="Hlwork" runat="server" CssClass="text-xs inline-flex items-center gap-1 text-orange-500 hover:text-orange-700 font-bold transition cursor-pointer">
                        查看作品 <svg class="w-3 h-3" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M14 5l7 7m0 0l-7 7m7-7H3"></path></svg>
                    </asp:HyperLink>
                </div>
            </div>

            <div class="w-full bg-emerald-50 border border-emerald-100 rounded-xl p-4">
                <div class="flex items-center gap-2 text-emerald-600 font-semibold text-sm mb-2">
                    <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12h6m-6 4h6M7 4h10a2 2 0 012 2v12a2 2 0 01-2 2H7a2 2 0 01-2-2V6a2 2 0 012-2z"></path></svg>
                    最新表现评语
                </div>
                <div class="text-sm text-slate-700 leading-relaxed min-h-[3rem] p-1 border-l-2 border-emerald-300 ml-1 pl-2 font-medium">
                    <asp:Label ID="LabelAttitudeNote" runat="server"></asp:Label>
                </div>
            </div>

            <asp:Label ID="LabelCids" runat="server" ForeColor="White" Visible="false"></asp:Label>
            
            <!-- Modern Tailwind CSS Modal for '我的资料'（多标签页整合版） -->
            <div id="modernGroupModal" class="fixed inset-0 z-[9999] hidden" aria-labelledby="modal-title" role="dialog" aria-modal="true">
                <!-- Background backdrop -->
                <div id="modernGroupModalBackdrop" class="fixed inset-0 bg-slate-900/60 backdrop-blur-sm transition-opacity opacity-0" aria-hidden="true"></div>

                <div class="fixed inset-0 z-10 flex items-end sm:items-center justify-center p-3 sm:p-6 overflow-y-auto">
                        <!-- Modal panel -->
                        <div id="modernGroupModalPanel" class="relative w-full sm:max-w-2xl lg:max-w-3xl transform rounded-2xl bg-white text-left shadow-2xl transition-all opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95 border border-slate-100 flex flex-col" style="max-height:calc(100vh - 3rem); overflow:hidden;">

                            

                            <!-- ── Header ── -->
                            <div id="mgm-header">
                                <div id="mgm-header-title">
                                    <div id="mgm-header-icon">
                                        <svg width="16" height="16" fill="none" stroke="#fff" stroke-width="2.5" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"/></svg>
                                    </div>
                                    <div>
                                        <h3 id="modal-title">我的资料</h3>
                                        <div id="mgm-header-sub">个人信息 · 学习档案</div>
                                    </div>
                                </div>
                                <button id="mgm-close-btn" type="button" onclick="closeModernGroupModal()" aria-label="关闭">
                                    <svg width="14" height="14" fill="none" stroke="currentColor" stroke-width="2.5" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12"/></svg>
                                </button>
                            </div>

                            <!-- ── Student Info Bar ── -->
                            <div id="modalStudentBar">
                                <div id="mgm-avatar-wrap">
                                    <img id="modalStudentAvatar" src="" alt="头像" />
                                    <div id="mgm-avatar-ring"></div>
                                </div>
                                <div id="mgm-student-info">
                                    <div id="mgm-name-row">
                                        <span id="modalStudentName"></span>
                                        <span id="modalStudentRankBadge"></span>
                                    </div>
                                    <div id="mgm-meta-row">
                                        <span class="mgm-chip">
                                            <svg width="10" height="10" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" d="M10 6H5a2 2 0 00-2 2v9a2 2 0 002 2h14a2 2 0 002-2V8a2 2 0 00-2-2h-5m-4 0V5a2 2 0 114 0v1"/></svg>
                                            <span id="modalStudentNum"></span>
                                        </span>
                                        <span class="mgm-sep"></span>
                                        <span class="mgm-chip grey">
                                            <svg width="10" height="10" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5"/></svg>
                                            <span id="modalStudentClass"></span>
                                        </span>
                                    </div>
                                </div>
                            </div>

                            <!-- ── Tab Bar ── -->
                            <nav id="mgm-tabbar" aria-label="资料导航">
                                <button id="tab-group" type="button" onclick="switchProfileTab('../profile/mygroup.aspx', 'tab-group')"
                                    class="profile-tab profile-tab-btn active">
                                    <svg fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0z"></path></svg>
                                    小组
                                </button>
                                <button id="tab-sign" type="button" onclick="switchProfileTab('../profile/mysign.aspx', 'tab-sign')"
                                    class="profile-tab profile-tab-btn">
                                    <svg fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-6 9l2 2 4-4"></path></svg>
                                    签到
                                </button>
                                <button id="tab-term" type="button" onclick="switchProfileTab('../profile/myterm.aspx', 'tab-term')"
                                    class="profile-tab profile-tab-btn">
                                    <svg fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4M7.835 4.697a3.42 3.42 0 001.946-.806 3.42 3.42 0 014.438 0 3.42 3.42 0 001.946.806 3.42 3.42 0 013.138 3.138 3.42 3.42 0 00.806 1.946 3.42 3.42 0 010 4.438 3.42 3.42 0 00-.806 1.946 3.42 3.42 0 01-3.138 3.138 3.42 3.42 0 00-1.946.806 3.42 3.42 0 01-4.438 0 3.42 3.42 0 00-1.946-.806 3.42 3.42 0 01-3.138-3.138 3.42 3.42 0 00-.806-1.946 3.42 3.42 0 010-4.438 3.42 3.42 0 00.806-1.946 3.42 3.42 0 013.138-3.138z"></path></svg>
                                    成果
                                </button>
                                <button id="tab-photo" type="button" onclick="switchProfileTab('../profile/myphoto.aspx', 'tab-photo')"
                                    class="profile-tab profile-tab-btn">
                                    <svg fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z"></path></svg>
                                    相片
                                </button>
                                <button id="tab-name" type="button" onclick="switchProfileTab('../profile/myname.aspx', 'tab-name')"
                                    class="profile-tab profile-tab-btn">
                                    <svg fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15.232 5.232l3.536 3.536m-2.036-5.036a2.5 2.5 0 113.536 3.536L6.5 21.036H3v-3.572L16.732 3.732z"></path></svg>
                                    姓名
                                </button>
                                <button id="tab-sex" type="button" onclick="switchProfileTab('../profile/mysex.aspx', 'tab-sex')"
                                    class="profile-tab profile-tab-btn">
                                    <svg fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"></path></svg>
                                    性别
                                </button>
                                <button id="tab-pwd" type="button" onclick="switchProfileTab('../profile/mypwd.aspx', 'tab-pwd')"
                                    class="profile-tab profile-tab-btn">
                                    <svg fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z"></path></svg>
                                    密码
                                </button>
                                <button id="tab-class" type="button" onclick="switchProfileTab('../profile/myclass.aspx', 'tab-class')"
                                    class="profile-tab profile-tab-btn">
                                    <svg fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4"></path></svg>
                                    班级
                                </button>
                                <button id="tab-change" type="button" onclick="switchProfileTab('../profile/mychange.aspx', 'tab-change')"
                                    class="profile-tab profile-tab-btn">
                                    <svg fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11.049 2.927c.3-.921 1.603-.921 1.902 0l1.519 4.674a1 1 0 00.95.69h4.915c.969 0 1.371 1.24.588 1.81l-3.976 2.888a1 1 0 00-.363 1.118l1.518 4.674c.3.922-.755 1.688-1.538 1.118l-3.976-2.888a1 1 0 00-1.176 0l-3.976 2.888c-.783.57-1.838-.197-1.538-1.118l1.518-4.674a1 1 0 00-.363-1.118l-3.976-2.888c-.784-.57-.38-1.81.588-1.81h4.914a1 1 0 00.951-.69l1.519-4.674z"></path></svg>
                                    组长
                                </button>
                            </nav>
                            <!-- Content (Iframe) -->
                            <div class="flex-1 min-h-0 bg-white">
                                <iframe id="modernGroupModalIframe" src="" class="w-full border-none block" style="height:clamp(360px, 65vh, 640px);" title="我的资料"></iframe>
                            </div>
                        </div>
                </div>
            </div>

            
        </div>
    </div>
    <script type="text/javascript">
        window.__myinfoConfig = {
            imagefaceId: '<%= Imageface.ClientID %>',
            snameId: '<%= sname.ClientID %>',
            snumId: '<%= snum.ClientID %>',
            sclassId: '<%= sclass.ClientID %>',
            labelRankId: '<%= LabelRank.ClientID %>',
            btnExitId: '<%= BtnExit.ClientID %>'
        };
    </script>
    <script type="text/javascript" src="../js/myinfo.js"></script>
</asp:Content>
