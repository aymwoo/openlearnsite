<%@ Page Title="" Language="C#" MasterPageFile="~/student/Scm.master" StylesheetTheme="Student" Validaterequest="false" AutoEventWireup="true" CodeFile="topicdiscuss.aspx.cs" Inherits="Student_topicdiscuss" ResponseEncoding="utf-8" %>
<%@ Register Assembly="Anthem" Namespace="Anthem" TagPrefix="anthem" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Cpcm" Runat="Server">
			<asp:Label ID="LabelCid" runat="server" Visible="False"></asp:Label>
			<asp:Label ID="LabelLid" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="LabelTid" runat="server" Visible="False"></asp:Label> 


<div id="topper" class="w-full max-w-5xl mx-auto space-y-6">
    <!-- Topic Title -->
    <div class="bg-white rounded-2xl shadow-sm border border-slate-200 overflow-hidden">
        <div class="course-node-head p-6 sm:p-8">
        <div class="flex items-center gap-3 mb-4">
            <asp:ImageButton ID="Btnclock" runat="server" ImageUrl="~/images/clock.gif" 
                onclick="Btnclock_Click" Enabled="False" CssClass="w-6 h-6 opacity-70 hover:opacity-100 transition" />        
            <anthem:Label ID="Labeltopic" runat="server" CssClass="course-node-title text-xl sm:text-2xl font-extrabold text-slate-800 tracking-tight"></anthem:Label>
            <anthem:CheckBox ID="TcloseCheck" runat="server" Visible="False" />
        </div>
        
        <div ID="Topics" runat="server" class="text-slate-700 leading-relaxed text-base p-2"></div>
        <div ID="TopicsResult" runat="server" class="topictext mt-4"></div>
        </div>
    </div>
    
    <!-- Post List Header -->
    <div class="flex items-center justify-between flex-wrap gap-3">
        <div class="flex items-center gap-2">
            <h3 class="text-lg font-bold text-slate-800 flex items-center gap-2">
                <span class="w-1.5 h-5 bg-indigo-500 rounded-full inline-block"></span> 帖子列表
            </h3>
            <anthem:Label ID="Labelreplycount" runat="server" CssClass="text-sm text-slate-500 font-medium"></anthem:Label>
            <anthem:imagebutton ID="ImageBtngoodall" runat="server" 
                ImageUrl="~/images/right.gif" onclick="ImageBtngoodall_Click" 
                ToolTip="给所有未评分的帖子加2分" Visible="False" CssClass="w-4 h-4 opacity-70 hover:opacity-100 transition" />
            <anthem:imagebutton ID="ImageBtngood2" runat="server" 
                ImageUrl="~/images/right.gif" onclick="ImageBtngood2_Click" 
                ToolTip="给所有未评分的帖子加6分" Visible="False" CssClass="w-4 h-4 opacity-70 hover:opacity-100 transition" />
        </div>
        <div class="flex items-center gap-2">
            <anthem:ImageButton ID="ImageBtnFresh" runat="server" 
                ImageUrl="~/images/refresh2.gif" onclick="ImageBtnFresh_Click" ToolTip="刷新贴子" CssClass="w-5 h-5 opacity-70 hover:opacity-100 transition cursor-pointer" />
            <anthem:HyperLink ID="HLbottom" runat="server" BorderStyle="None" 
                BorderWidth="0px" ImageUrl="~/images/bottom.png" NavigateUrl="#bottom" 
                ToolTip="跳到底部" CssClass="w-5 h-5 opacity-70 hover:opacity-100 transition"></anthem:HyperLink>
        </div>
    </div>
    
    <!-- Discussion Posts -->
    <div class="space-y-4">
        <anthem:GridView ID="GVtopicDiscuss" runat="server" AutoGenerateColumns="False" 
            CellPadding="0" Width="100%" 
            onrowdatabound="GVtopicDiscuss_RowDataBound"  
            DataKeyNames="rid" PageSize="5" CellSpacing="1" 
            ShowHeader="False" GridLines="None" 
            onrowcommand="GVtopicDiscuss_RowCommand" CssClass="w-full">
             <Columns>
                 <asp:TemplateField>
                     <ItemTemplate>   
                     <div class="bg-white rounded-xl border border-slate-200 shadow-sm overflow-hidden mb-4 hover:shadow-md transition">
                         <div class="topichead">
                             <div class="topicleft">
                                 <anthem:Image ID="Imagestu" runat="server" CssClass="imgstu" />                                            
                                 <anthem:Label ID="Labelsname" runat="server" Text='<%# Bind("Sname") %> ' CssClass="font-semibold text-slate-800"></anthem:Label>
                                 <anthem:Image ID="Imageagree" runat="server" Visible="False" ImageUrl="~/images/good16.png" CssClass="w-4 h-4" />
                                <anthem:CheckBox ID="Ckedit" runat="server" Checked='<%# Bind("Redit") %> ' Visible="False" />
                                <anthem:Label ID="Labelsnum" runat="server" Text='<%# Bind("Rsnum") %> ' Visible="False"></anthem:Label>
                                <anthem:CheckBox ID="CheckSleader" runat="server" Checked='<%# Bind("Sleader") %> ' Visible="False" />
                             </div>
                             <div class="topicright">
                                 <anthem:Image ID="Imagegroup" runat="server" ImageUrl="~/images/gcard.gif" CssClass="w-4 h-4" />
                                 <anthem:Label ID="Labelscore" runat="server" Text='<%# Bind("Rscore") %> ' ToolTip="学分" CssClass="text-emerald-600 font-bold text-sm"></anthem:Label><span class="text-xs text-slate-400">学分</span>
                                 <anthem:ImageButton ID="ImageButtonAgree" runat="server" 
                                     CausesValidation="false" CommandArgument='<%# Bind("rid") %>'
                                     CommandName="Agree" ImageUrl="~/images/good24.gif" ToolTip="点赞" CssClass="w-5 h-5 opacity-70 hover:opacity-100 transition cursor-pointer"></anthem:ImageButton>
                                 <anthem:Label ID="Labelagree" runat="server" Text='<%# Bind("Ragree") %> ' CssClass="text-pink-500 font-medium text-sm"></anthem:Label><span class="text-xs text-slate-400">赞</span>
                                 <anthem:Image ID="Imageflag" runat="server" ImageUrl="~/images/topicnormal.png" CssClass="w-4 h-4" />
                                 <anthem:Label ID="Labelfloor" runat="server" CssClass="text-xs text-slate-500"></anthem:Label><span class="text-xs text-slate-400">楼</span>
                                <anthem:ImageButton ID="ImageButtonEdit" runat="server" 
                                     CausesValidation="false" CommandArgument='<%# Bind("rid") %>'
                                     CommandName="Reply" ImageUrl="~/images/edno.gif" CssClass="w-4 h-4 opacity-60 hover:opacity-100 transition cursor-pointer" />
                                 <anthem:ImageButton ID="ImageButtonGood" runat="server" 
                                     CausesValidation="false" CommandArgument='<%# Bind("rid") %>'
                                     CommandName="Good" ImageUrl="~/images/right.gif" ToolTip="加2分" CssClass="w-4 h-4 opacity-60 hover:opacity-100 transition cursor-pointer" />
                                 <anthem:ImageButton ID="ImageButtonless" runat="server" 
                                     CausesValidation="false" CommandArgument='<%# Bind("rid") %>'
                                     CommandName="Less" ImageUrl="~/images/ban.gif" ToolTip="减2分" CssClass="w-4 h-4 opacity-60 hover:opacity-100 transition cursor-pointer" />
                                 <anthem:ImageButton ID="ImageButtonDel" runat="server" CausesValidation="false" 
                                     CommandArgument='<%# Bind("rid") %>' CommandName="Del" 
                                     ImageUrl="~/images/delete.gif" CssClass="w-4 h-4 opacity-60 hover:opacity-100 transition cursor-pointer" />
                             </div>
                         </div>
                         <div class="topictext">
                             <%# HttpUtility.HtmlDecode( Eval("Rwords").ToString())%>
                             <div class="text-right text-xs text-slate-400 mt-3 pt-2 border-t border-slate-100">
                                 时间：<anthem:Label ID="Labeldate" runat="server" Text='<%# Bind("Rtime") %> '></anthem:Label> 
                                 &nbsp; IP：<anthem:Label ID="Labelip" runat="server" Text='<%# Bind("Rip") %> '></anthem:Label>
                             </div>
                         </div>
                     </div>
                     </ItemTemplate>
                 </asp:TemplateField>
             </Columns>             
             <HeaderStyle Font-Bold="False" />
      </anthem:GridView>
    </div>
      
    <div id="bottom"></div>
    
    <!-- Bottom Controls -->
    <div class="flex items-center justify-between flex-wrap gap-3">
        <div class="flex items-center gap-2">
            <h3 class="text-lg font-bold text-slate-800 flex items-center gap-2">
                <span class="w-1.5 h-5 bg-green-500 rounded-full inline-block"></span> 讨论回复
            </h3>
            <asp:Label ID="Labelreplycountbtm" runat="server" CssClass="text-sm text-slate-500 font-medium"></asp:Label>
        </div>
        <div class="flex items-center gap-2">
            <anthem:ImageButton ID="ImageBtnFreshtwo" runat="server" 
                ImageUrl="~/images/refresh2.gif" onclick="ImageBtnFresh_Click" 
                ToolTip="刷新贴子" CssClass="w-5 h-5 opacity-70 hover:opacity-100 transition cursor-pointer" />
            <anthem:HyperLink ID="HLtop" runat="server" BorderStyle="None" BorderWidth="0px" 
                ImageUrl="~/images/top.png" NavigateUrl="#topper" ToolTip="跳到顶部" CssClass="w-5 h-5 opacity-70 hover:opacity-100 transition"></anthem:HyperLink>
        </div>
    </div>

    <!-- Reply Editor -->
    <div id="plant" runat="server">
        <div class="bg-white rounded-2xl shadow-sm border border-slate-200 p-6 space-y-4">
            <textarea name="textareaWord" style="width: 100%;height:260px;" ></textarea> 
            <script charset="utf-8" src="../kindeditor/kindeditor-min.js" type="text/javascript"></script>
            <script charset="utf-8" src="../kindeditor/lang/zh_CN.js" type="text/javascript"></script>
            <script src="../code/jquery.min.js" type="text/javascript"></script>
            

            
            
            <div class="flex items-center justify-between flex-wrap gap-3">
                <div class="text-sm text-slate-500">
                    当前输入了 <span class="word_count font-bold text-indigo-600">0</span> 个文字（不少于2个汉字，最多为300汉字）
                </div>
                <asp:Button ID="Btnword" runat="server" Text="发表讨论" 
                    onclick="Btnword_Click" BorderStyle="None"
                    CssClass="px-6 py-2.5 bg-gradient-to-r from-blue-500 to-indigo-600 text-white font-bold rounded-xl hover:from-blue-600 hover:to-indigo-700 transition duration-300 shadow-md border-0 cursor-pointer" 
                    Width="120px" />
            </div>
            <anthem:Label ID="Labeldiscuss" runat="server" SkinID="LabelMsgRed" CssClass="text-red-500 font-bold text-sm"></anthem:Label>
        </div>
    </div>
    
    <div class="text-sm text-slate-400 mt-4">
        <anthem:Label ID="Labelnostu" runat="server"></anthem:Label>    
    </div>
</div>

<!-- Image Lightbox Overlay -->
<div id="outerdiv" style="position:fixed;top:0;left:0;background:rgba(0,0,0,0.8);z-index:9999;width:100%;height:100%;display:none;cursor:pointer;">
    <div id="innerdiv" style="position:absolute;">
        <img id="bigimg" style="pointer-events: none; border-radius: 8px;" src="" />
    </div>
</div>
    <script type="text/javascript">
        window.__topicdiscussConfig = {
            myCid: '<%=myCid %>'
        };
    </script>
    <script type="text/javascript" src="../js/topicdiscuss.js"></script>
</asp:Content>
