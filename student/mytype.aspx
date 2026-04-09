<%@ Page Title="" Language="C#" MasterPageFile="~/student/Stud.master"  StylesheetTheme="Student" AutoEventWireup="true" CodeFile="mytype.aspx.cs" Inherits="Student_mytype" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cphs" Runat="Server">
    <link href="../js/Typer.css" rel="stylesheet" type="text/css" />
<div class="grid grid-cols-1 lg:grid-cols-4 gap-6 lg:gap-8 w-full max-w-full">
    <!-- Main Typing Panel (Left) -->
    <div class="lg:col-span-3 space-y-6 overflow-hidden min-w-0"
         onselectstart="return false"       
         oncopy="return false"       
         oncut="return false"       
         onpaste="return false"       
         oncontextmenu="return false">
         
         <div class="bg-white border text-center border-slate-200/60 rounded-2xl shadow-sm p-4 sm:p-6 pb-2">
            <div class="flex flex-wrap gap-2 items-center mb-4 pb-4 border-b border-slate-100">
                <asp:DataList ID="DLTid" runat="server" ForeColor="#333333" RepeatColumns="36" 
                    RepeatDirection="Horizontal" RepeatLayout="Flow" CellPadding="0" CellSpacing="0">
                    <ItemTemplate>
                        <asp:HyperLink ID="id" runat="server" CssClass="inline-flex items-center justify-center w-7 h-7 text-xs font-semibold rounded-md m-0.5 transition-colors duration-200 bg-slate-100 text-slate-600 hover:bg-blue-500 hover:text-white"
                            NavigateUrl='<%# "mytype.aspx?Tid="+Eval("tid") %>' 
                            Text='<%# Eval("tid") %>'  ToolTip='<%# Eval("Ttitle") %>' 
                            Font-Underline="False"></asp:HyperLink>
                    </ItemTemplate>
                </asp:DataList>
            </div>
            
            <div class="mb-4 text-xl font-bold text-slate-800">
                <asp:Label ID="LTid" runat="server" CssClass="text-blue-600 mr-2"></asp:Label>
                <asp:Label ID="Ttitle" runat="server" ></asp:Label> 
            </div>
            
            <div id="Tcontent" class="typecontent text-lg leading-relaxed text-slate-700 bg-slate-50 p-6 rounded-xl border border-slate-200 text-left min-h-[150px] font-sans break-words mb-4">
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            </div>
            
            <div class="flex flex-wrap items-center justify-center gap-4 bg-slate-800 text-white rounded-xl p-4 shadow-inner mb-4">    
                <div class="flex items-center gap-2">
                    <span class="text-slate-300 font-medium">正确</span>
                    <input id="Text4" class="w-20 text-center font-bold text-emerald-400 bg-slate-900 border border-slate-700 rounded py-1.5 focus:outline-none" 
                             type="text" hidefocus="hideFocus" maxlength="30" readonly="readOnly" 
                             unselectable="on" value="0" name="TypeText4" />
                </div>
                <div class="flex items-center gap-2">
                    <span class="text-slate-300 font-medium">速度</span>
                    <input id="Text6" class="w-20 text-center font-bold text-blue-400 bg-slate-900 border border-slate-700 rounded py-1.5 focus:outline-none"
                             type="text" hidefocus="hideFocus" maxlength="30" readonly="readOnly" 
                             unselectable="on" name="Typeresult" value="0"  />
                </div>
                <div class="flex items-center gap-2">
                    <span class="text-slate-300 font-medium">拼音</span>
                    <input id="Textpy" class="w-32 text-center font-mono text-orange-400 bg-slate-900 border border-slate-700 rounded py-1.5 focus:outline-none" 
                             type="text" hidefocus="hideFocus" maxlength="30" readonly="readOnly" 
                             unselectable="on" />
                </div>
            </div>
            
            <textarea id="InputText" class="w-full textareacss p-4 bg-white border-2 border-slate-300 rounded-xl focus:border-blue-500 focus:ring-4 focus:ring-blue-500/10 transition-all resize-y min-h-[160px] text-lg text-slate-800" 
                      onpaste="return false;" ondragenter="return false;" ondrop="return false;" rows="6" placeholder="在此处开始打字..."></textarea>
            
            <div class="mt-4 text-center">
                <label id="Labelmsg" class="text-red-500 font-bold font-sm block h-6"></label>
                <asp:Label ID="Labeltids" runat="server" Visible="False"></asp:Label>
            </div>
        </div>
    </div>
    
    <!-- Sidebar Leaderboard (Right) -->
    <div class="lg:col-span-1 space-y-6 self-start top-24 sticky">
        <div class="bg-indigo-50/50 border border-indigo-100 rounded-2xl p-5 shadow-sm w-full overflow-x-auto min-w-0 flex flex-col items-center">
            <div class="flex justify-center gap-3 mb-6 w-full pb-4 border-b border-indigo-200/60">
                <asp:HyperLink ID="HChinese" runat="server" ImageUrl="~/images/py.png" NavigateUrl="~/student/mychinese.aspx" CssClass="transform hover:scale-110 transition duration-300 drop-shadow-md rounded-full"></asp:HyperLink> 
                <asp:HyperLink ID="HkFinger" runat="server" ImageUrl="~/images/en.png" NavigateUrl="~/student/myfinger.aspx" CssClass="transform hover:scale-110 transition duration-300 drop-shadow-md rounded-full"></asp:HyperLink>        
                <asp:HyperLink ID="HTyper" runat="server" ImageUrl="~/images/cn.png" NavigateUrl="~/student/mytype.aspx" CssClass="transform hover:scale-110 transition duration-300 drop-shadow-md rounded-full border-2 border-indigo-400"></asp:HyperLink>       
            </div>
            
            <div class="w-full mb-4">
                <script src="../js/Backcolor.js" type="text/javascript"></script>
                <script type="text/javascript">WriteBg();</script>
            </div>
            
            <h4 class="w-full text-indigo-800 font-bold mb-3 flex items-center gap-2">
                <svg class="w-5 h-5 text-yellow-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4M7.835 4.697a3.42 3.42 0 001.946-.806 3.42 3.42 0 014.438 0 3.42 3.42 0 001.946.806 3.42 3.42 0 013.138 3.138 3.42 3.42 0 00.806 1.946 3.42 3.42 0 010 4.438 3.42 3.42 0 00-.806 1.946 3.42 3.42 0 01-3.138 3.138 3.42 3.42 0 00-1.946.806 3.42 3.42 0 01-4.438 0 3.42 3.42 0 00-1.946-.806 3.42 3.42 0 01-3.138-3.138 3.42 3.42 0 00-.806-1.946 3.42 3.42 0 010-4.438 3.42 3.42 0 00.806-1.946 3.42 3.42 0 013.138-3.138z"></path></svg>
                中文输入英雄榜
            </h4>
            
<div class="w-full overflow-x-auto">
            
            <asp:GridView ID="GVTyper" runat="server" AllowPaging="True" CellPadding="2"         
                onpageindexchanging="GVTyper_PageIndexChanging" PageSize="20"
                OnRowDataBound="GVTyper_RowDataBound" Width="100%" SkinID="GridViewInfo"
                CssClass="w-full text-xs text-slate-700 bg-white border border-slate-200 rounded-lg overflow-hidden shadow-sm">
                <Columns>
                    <asp:BoundField HeaderText="名次">
                        <HeaderStyle CssClass="py-2.5 px-2 bg-slate-50 border-b border-slate-200 font-bold text-center" />
                        <ItemStyle CssClass="py-2 border-b border-slate-100 text-center font-semibold text-slate-500" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Sname" HeaderText="英雄">
                        <HeaderStyle HorizontalAlign="Left" CssClass="py-2.5 px-2 bg-slate-50 border-b border-slate-200 font-bold" />
                        <ItemStyle HorizontalAlign="Left" CssClass="py-2 px-2 border-b border-slate-100 font-medium text-slate-800" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Pscore" HeaderText="神速">
                        <HeaderStyle HorizontalAlign="Left" CssClass="py-2.5 px-2 bg-slate-50 border-b border-slate-200 font-bold" />
                        <ItemStyle HorizontalAlign="Left" CssClass="py-2 px-2 border-b border-slate-100 text-blue-600 font-bold" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Ptype" HeaderText="次数">
                        <HeaderStyle CssClass="py-2.5 px-2 bg-slate-50 border-b border-slate-200 font-bold text-center" />
                        <ItemStyle CssClass="py-2 border-b border-slate-100 text-center text-slate-400" />
                    </asp:BoundField>
                </Columns>
                <PagerTemplate>
                    <div class="flex items-center justify-center gap-1.5 py-2.5 bg-slate-50">
                        <asp:LinkButton ID="btnFirst" runat="server" CausesValidation="False" 
                            CommandArgument="First" CommandName="Page" CssClass="px-2 py-1 text-[10px] bg-white border border-slate-300 rounded text-slate-600 hover:bg-slate-100" Text="首页"></asp:LinkButton>
                        <asp:LinkButton ID="btnPrev" runat="server" CausesValidation="False" 
                            CommandArgument="Prev" CommandName="Page" CssClass="px-2 py-1 text-[10px] bg-white border border-slate-300 rounded text-slate-600 hover:bg-slate-100" Text="上页"></asp:LinkButton>
                        <asp:LinkButton ID="btnNext" runat="server" CausesValidation="False" 
                            CommandArgument="Next" CommandName="Page" CssClass="px-2 py-1 text-[10px] bg-white border border-slate-300 rounded text-slate-600 hover:bg-slate-100" Text="下页"></asp:LinkButton>
                        <asp:LinkButton ID="btnLast" runat="server" CausesValidation="False" 
                            CommandArgument="Last" CommandName="Page" CssClass="px-2 py-1 text-[10px] bg-white border border-slate-300 rounded text-slate-600 hover:bg-slate-100" Text="尾页"></asp:LinkButton>               
                    </div>
                </PagerTemplate>
                <RowStyle CssClass="hover:bg-slate-50 transition" />
            </asp:GridView>
</div>
            
            <div class="mt-6 flex flex-col gap-3 w-full">
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/student/alltyper.aspx" Target="_self"
                    CssClass="w-full flex items-center justify-center gap-2 py-2.5 text-sm bg-gradient-to-r from-blue-500 to-indigo-600 text-white font-bold rounded-xl hover:from-blue-600 hover:to-indigo-700 transition duration-300 shadow-md">
                    👑 全校英雄榜
                </asp:HyperLink>   
                <asp:HyperLink ID="HyperLink2" runat="server" Target="_self"
                    CssClass="w-full flex items-center justify-center gap-2 py-2.5 text-sm bg-white border-2 border-indigo-400 text-indigo-600 font-bold rounded-xl hover:bg-indigo-50 transition duration-300 shadow-sm">
                     班级英雄榜
                </asp:HyperLink>   
            </div>
        </div>
    </div>
</div>
    <script src="../js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="../js/Typer.js" type="text/javascript"></script>
    <script src="../js/pydic.js" type="text/javascript"></script>
    <script src="../js/wbdic.js" type="text/javascript"></script>
</asp:Content>
