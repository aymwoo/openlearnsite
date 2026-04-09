<%@ Page Title="" Language="C#" MasterPageFile="~/student/Stud.master" StylesheetTheme="Student"  AutoEventWireup="true" CodeFile="myfinger.aspx.cs" Inherits="Student_myfinger" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cphs" Runat="Server">
    <link href="../images/fingering/finger.css" rel="stylesheet" type="text/css" />
<div class="grid grid-cols-1 lg:grid-cols-4 gap-6 w-full max-w-full">
    <!-- Main Keyboard & Typing Area (Left) -->
    <div class="lg:col-span-3 space-y-6 overflow-hidden min-w-0">
        <div class="bg-indigo-900/5 border border-indigo-200/50 rounded-2xl shadow-inner p-6 flex flex-col items-center">
            <div id="inputdiv" class="w-full max-w-3xl">
                <br />
        <div id="TextWord"  class="showtxt" ></div>
        <br />
        <div id="Meanword" class="meandiv" >
        </div>
        <br />
        <input id="InputWord" type="text"  class="inputtxt px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300"  onpaste= "return   false; "   ondragenter= "return   false;"   ondrop= "return   false;" tabindex="0"  autocomplete="off" />
        <br /><br />
    </div> 
	<div id="keyhand"></div>
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
	<div class="tab keyText">Tab</div><div class="keyCom" id="keyQ">Q</div>
	<div class="keyCom" id="keyW">W</div><div class="keyCom" id="keyE">E</div>
	<div class="keyCom" id="keyR">R</div><div class="keyCom" id="keyT">T</div>
	<div class="keyCom" id="keyY">Y</div><div class="keyCom" id="keyU">U</div>
	<div class="keyCom" id="keyI">I</div><div class="keyCom" id="keyO">O</div>
	<div class="keyCom" id="keyP">P</div><div class="keyCom" id="keyZKH">{<br />[</div>
	<div class="keyCom" id="keyYKH">}<br />]</div><div class="enterup1 keyText"></div>
<!--第三行-->
	<div class="cap keyText">Caps</div><div class="keyCom" id="keyA">A</div>
	<div class="keyCom" id="keyS">S</div><div class="keyCom" id="keyD">D</div>
	<div class="keyCom" id="keyF">F</div><div class="keyCom" id="keyG">G</div>
	<div class="keyCom" id="keyH">H</div><div class="keyCom" id="keyJ">J</div>
	<div class="keyCom" id="keyK">K</div><div class="keyCom" id="keyL">L</div>
	<div class="keyCom" id="keyFHMH">:<br />;</div><div class="keyCom" id="keyDYSY">"<br />'</div>
	<div class="enterup2 keyText">Enter</div>
<!--第四行-->
	<div class="shift keyText" id="shiftl">Shift</div><div class="keyCom" id="keyZ">Z</div>
	<div class="keyCom" id="keyX">X</div><div class="keyCom" id="keyC">C</div>
	<div class="keyCom" id="keyV">V</div><div class="keyCom" id="keyB">B</div>
	<div class="keyCom" id="keyN">N</div><div class="keyCom" id="keyM">M</div>
	<div class="keyCom" id="keyDHXY"><<br/>,</div><div class="keyCom" id="keyJHDY">><br />.</div>
	<div class="keyCom" id="keyXXWH">?<br />/</div><div class="shift keyText" id="shiftr">Shift</div>
<!--第五行-->
	<div class="ctrl keyText">Ctrl</div><div class="keymic keyText"></div>
	<div class="alt keyText">Alt</div><div class="space keyText" id="keyKG"></div>
	<div class="alt keyText">Alt</div><div class="keymic keyText"></div><div class="ctrl keyText">Ctrl</div>
</div>
        </div>
    </div>
    
    <!-- Right Sidebar -->
    <div class="lg:col-span-1 space-y-6 self-start top-24 sticky text-center flex flex-col items-center">
        <div class="bg-indigo-50/50 border border-indigo-100 rounded-2xl p-5 shadow-sm overflow-hidden w-full flex flex-col items-center">
        <div class="flex justify-center gap-3 mb-6 w-full pb-4 border-b border-indigo-200/60">
            <asp:HyperLink ID="HChinese" runat="server" ImageUrl="~/images/py.png" NavigateUrl="~/student/mychinese.aspx" CssClass="transform hover:scale-110 transition duration-300 drop-shadow-md rounded-full"></asp:HyperLink> 
            <asp:HyperLink ID="HkFinger" runat="server" ImageUrl="~/images/en.png" NavigateUrl="~/student/myfinger.aspx" CssClass="transform hover:scale-110 transition duration-300 drop-shadow-md rounded-full border-2 border-indigo-400"></asp:HyperLink>        
            <asp:HyperLink ID="HTyper" runat="server" ImageUrl="~/images/cn.png" NavigateUrl="~/student/mytype.aspx" CssClass="transform hover:scale-110 transition duration-300 drop-shadow-md rounded-full"></asp:HyperLink>       
        </div>
        
        <div class="flex items-center gap-3 mb-4 w-full bg-white p-3 rounded-xl border border-slate-200 shadow-sm">
            <span class="text-sm font-bold text-slate-700 whitespace-nowrap">选择级别：</span>
            <select name="ls" id="levelselect" onchange="changelevel()" class="w-full flex-1 py-1.5 px-2 bg-slate-50 border border-slate-300 text-slate-700 text-sm rounded focus:ring-blue-500 focus:border-blue-500 outline-none">
                <option value="0">小学英语</option>
                <option value="1">中考英语</option>
                <option value="2">高考英语</option>
                <option value="3" selected="selected">编程英语</option>
            </select>
        </div>
        
        <div class="w-full bg-slate-800 text-slate-300 text-sm rounded-xl p-4 shadow-inner text-left font-mono space-y-1.5 mb-4 border border-slate-700">
            <div id="snum" style="display:none"><%=this.mysnum%></div>
            <div class="letter"></div>
            <div id="lrpe" class="letter"></div>
            <div id="lnum" class="letter"></div>
            <div id="lrig" class="letter text-emerald-400 font-bold"></div>
            <div id="lwrg" class="letter text-red-400 font-bold"></div>
            <div id="wnum" class="letter"></div>
            <div id="lspd" class="letter text-blue-300"></div>
            <div id="wspd" class="letter text-blue-300"></div>
            <div id="lsec" class="letter"></div>
            <div id="weid" class="letter"></div>
            <div class="letter"></div>    
        </div>
        
        <div id="oldspd" runat="server" class="text-sm text-slate-500 mb-2 font-medium"></div> 
        <div id="msg" class="text-sm font-bold text-red-500 mb-4 h-5"></div>
        
        <div class="w-full flex flex-col gap-3">
            <asp:HyperLink ID="HLfinger" runat="server" NavigateUrl="~/student/allfinger.aspx" Target="_self" 
                CssClass="w-full flex justify-center py-2.5 text-sm bg-gradient-to-r from-blue-500 to-indigo-600 text-white font-bold rounded-xl hover:from-blue-600 hover:to-indigo-700 transition duration-300 shadow-md">
                👑 英文打字英雄榜
            </asp:HyperLink>
        </div>
        </div>
        
        <div id="gamesDiv" runat="server" class="bg-slate-50 border border-slate-200 rounded-2xl p-5 shadow-sm overflow-hidden w-full space-y-3">
            <h4 class="text-sm font-bold text-slate-500 mb-2 border-b border-slate-200 pb-2">休闲益智区</h4>
            <asp:HyperLink ID="Hlztype" runat="server" NavigateUrl="~/ztype/index.html" Target="_blank" 
                CssClass="w-full flex justify-center py-2 text-sm bg-white border border-indigo-300 text-indigo-600 font-medium rounded-lg hover:bg-indigo-50 transition duration-300 shadow-sm" >🚀 太空打字游戏</asp:HyperLink>
            <asp:HyperLink ID="Hlbox" runat="server" NavigateUrl="~/sokoban/index.aspx" Target="_blank" 
                CssClass="w-full flex justify-center py-2 text-sm bg-white border border-indigo-300 text-indigo-600 font-medium rounded-lg hover:bg-indigo-50 transition duration-300 shadow-sm" >📦 推箱子游戏</asp:HyperLink>
            <asp:HyperLink ID="Hlwuziqi" runat="server" NavigateUrl="~/wuziqi/index.aspx" Target="_blank" 
                CssClass="w-full flex justify-center py-2 text-sm bg-white border border-indigo-300 text-indigo-600 font-medium rounded-lg hover:bg-indigo-50 transition duration-300 shadow-sm" >🤖 AI 五子棋</asp:HyperLink>
        </div>  
        
        <div id="victory" style="display:none" class="w-full mt-4 flex justify-center">
            <img src="../js/images/v.gif" class="w-24 h-auto drop-shadow-md" alt="Victory"/>
        </div>
    </div>
</div>
<br />
    <script src="../js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="../js/Finger.js" type="text/javascript"></script>

    <div id="tempdiv" style=" display:none"></div>

</div>
</asp:Content>
