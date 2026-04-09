<%@ Page Title="" Language="C#" MasterPageFile="~/student/Stud.master" StylesheetTheme="Student"  AutoEventWireup="true" CodeFile="mychinese.aspx.cs" Inherits="Student_mychinese" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cphs" Runat="Server">
 <link href="../images/fingering/finger.css" rel="stylesheet" type="text/css" />
 <script src="../js/jquery.cookie.js" type="text/javascript"></script>
<div class="grid grid-cols-1 lg:grid-cols-4 gap-6 w-full max-w-full">
    <!-- Main Keyboard & Typing Area (Left) -->
    <div class="lg:col-span-3 space-y-6 overflow-hidden min-w-0">
        <div class="bg-indigo-900/5 border border-indigo-200/50 rounded-2xl shadow-inner p-6 flex flex-col items-center">
            <div id="inputdiv" class="w-full max-w-3xl flex flex-col items-center">
        <div>
            <asp:DataList ID="DataList1" runat="server" CellPadding="3" 
                HorizontalAlign="Center" onitemdatabound="DataList1_ItemDataBound" 
                RepeatDirection="Horizontal" RepeatLayout="Flow" CellSpacing="3">
                <ItemTemplate>                    
                    <asp:Label ID="Lbtitle" runat="server" Text='<%# Eval("Ntitle") %>' CssClass="hand" ></asp:Label>
                    <asp:Label ID="Lbid" runat="server" Text='<%# Eval("nid") %>' Visible="false"></asp:Label>
                </ItemTemplate>
            </asp:DataList>

        </div>
        <br />
        <asp:Label ID="Lbnid" runat="server" Text="0"  CssClass="unsee"></asp:Label>
        <br />
        <br />
        <br />
            <div id="Typepingyin" class="typepy">
            </div>
            <div id="Typechinese" class="typecn">
            </div>
        <br />
        <br />
            <input id="InputWord" class="typewd px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300" type="text" onpaste="return   false; " ondragenter="return   false;" ondrop= "return   false;" tabindex="0" autocomplete="off" />
            <br />
        <br />
    </div>     
    <div id="keyboard">
<!--第一行-->
	<div class="keyCom" id="keyDHSY">~<br />`</div><div class="keyCom" id="key1">!<br />1</div>
	<div class="keyCom" id="key2">@<br />2</div><div class="keyCom" id="key3">#<br />3</div>
	<div class="keyCom" id="key4">$<br />4</div><div class="keyCom" id="key5">%<br />5</div>
	<div class="keyCom" id="key6">^<br />6</div><div class="keyCom" id="key7">&amp;<br />7</div>
	<div class="keyCom" id="key8">*<br />8</div><div class="keyCom" id="key9">(<br />9</div>
	<div class="keyCom" id="key0">)<br />0</div><div class="keyCom" id="keyJHSX">_<br />-</div>
	<div class="keyCom" id="keyDHJH">+<br />=</div><div class="keyCom" id="keyXXSX">|<br />\</div>
	<div class="key2 keyText">←</div>
<!--第二行-->
	<div class="tab keyText">Tab</div><div class="keyCom" id="keyQ">q</div>
	<div class="keyCom" id="keyW">w</div><div class="keyCom" id="keyE">e</div>
	<div class="keyCom" id="keyR">r</div><div class="keyCom" id="keyT">t</div>
	<div class="keyCom" id="keyY">y</div><div class="keyCom" id="keyU">u</div>
	<div class="keyCom" id="keyI">i</div><div class="keyCom" id="keyO">o</div>
	<div class="keyCom" id="keyP">p</div><div class="keyCom" id="keyZKH">{<br />[</div>
	<div class="keyCom" id="keyYKH">}<br />]</div><div class="enterup1 keyText"></div>
<!--第三行-->
	<div class="cap keyText">Caps</div><div class="keyCom" id="keyA">a</div>
	<div class="keyCom" id="keyS">s</div><div class="keyCom" id="keyD">d</div>
	<div class="keyCom" id="keyF">f</div><div class="keyCom" id="keyG">g</div>
	<div class="keyCom" id="keyH">h</div><div class="keyCom" id="keyJ">j</div>
	<div class="keyCom" id="keyK">k</div><div class="keyCom" id="keyL">l</div>
	<div class="keyCom" id="keyFHMH">:<br />;</div><div class="keyCom" id="keyDYSY">"<br />'</div>
	<div class="enterup2 keyText">Enter</div>
<!--第四行-->
	<div class="shift keyText" id="shiftl">Shift</div><div class="keyCom" id="keyZ">z</div>
	<div class="keyCom" id="keyX">x</div><div class="keyCom" id="keyC">c</div>
	<div class="keyCom" id="keyV">v</div><div class="keyCom" id="keyB">b</div>
	<div class="keyCom" id="keyN">n</div><div class="keyCom" id="keyM">m</div>
	<div class="keyCom" id="keyDHXY"><<br/>,</div><div class="keyCom" id="keyJHDY">><br />.</div>
	<div class="keyCom" id="keyXXWH">?<br />/</div><div class="shift keyText" id="shiftr">Shift</div>
<!--第五行-->
	<div class="ctrl keyText">Ctrl</div><div class="keymic keyText"></div>
        </div>
    </div>
    
    <!-- Right Sidebar -->
    <div class="lg:col-span-1 space-y-6 self-start top-24 sticky text-center flex flex-col items-center">
        <div class="bg-indigo-50/50 border border-indigo-100 rounded-2xl p-5 shadow-sm overflow-hidden w-full flex flex-col items-center">
    <div>
        <div class="flex justify-center gap-3 mb-6 w-full pb-4 border-b border-indigo-200/60">
            <asp:HyperLink ID="HChinese" runat="server" ImageUrl="~/images/py.png" NavigateUrl="~/student/mychinese.aspx" CssClass="transform hover:scale-110 transition duration-300 drop-shadow-md rounded-full border-2 border-indigo-400"></asp:HyperLink> 
            <asp:HyperLink ID="HkFinger" runat="server" ImageUrl="~/images/en.png" NavigateUrl="~/student/myfinger.aspx" CssClass="transform hover:scale-110 transition duration-300 drop-shadow-md rounded-full"></asp:HyperLink>        
            <asp:HyperLink ID="HTyper" runat="server" ImageUrl="~/images/cn.png" NavigateUrl="~/student/mytype.aspx" CssClass="transform hover:scale-110 transition duration-300 drop-shadow-md rounded-full"></asp:HyperLink>       
        </div>
        
        <div id="oldspd" class="w-full bg-slate-800 text-slate-300 text-sm rounded-xl p-4 shadow-inner text-left font-mono mb-4 border border-slate-700 flex items-center justify-between">
            <div class="flex items-center gap-2">
                <img src="../images/apple.gif" alt="apple" class="w-5 h-5"/>
                <span>收集的苹果数：</span>
            </div>
            <span id="totalapples" class="text-orange-400 font-bold text-lg"></span>
        </div> 
        
        <div id="apples" class="applecss bg-white border border-slate-200 rounded-xl p-3 w-full min-h-[100px] mb-4 shadow-sm flex flex-wrap gap-2 justify-center items-center"></div>
        
        <div id="msg" class="text-sm font-bold text-red-500 mb-4 h-5"></div>
        
        <div class="w-full flex flex-col gap-3">
            <asp:HyperLink ID="HLfinger" runat="server" NavigateUrl="~/student/allchinese.aspx" Target="_blank" 
                CssClass="w-full flex justify-center py-2.5 text-sm bg-gradient-to-r from-blue-500 to-indigo-600 text-white font-bold rounded-xl hover:from-blue-600 hover:to-indigo-700 transition duration-300 shadow-md">
                👑 拼音输入英雄榜
            </asp:HyperLink>
        </div>     
        
        <div id="debug" class="text-xs text-slate-400 mt-4 break-all"></div>
        </div>
    </div>
</div>
    <script src="../js/pydic.js" type="text/javascript"></script>
    <script src="../js/Chinese.js" type="text/javascript"></script>
</div>
</asp:Content>
