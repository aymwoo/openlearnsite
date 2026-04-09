<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" AutoEventWireup="true" CodeFile="judgeedit.aspx.cs" Inherits="Teacher_judgeedit" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
<link href="../App_Themes/Teacher/judgeedit.css" rel="stylesheet" />
<link href="../code/css/font-awesome.min.css" rel="stylesheet" type="text/css" />

<div id="divmain">

<div id="divleft">
	<div id="editor"></div>
</div>


<div id="divright">
<div id="title">
<h2 style="text-align:center;"> <asp:Label ID="LabelTitle" runat="server" Text="标题"></asp:Label></h2>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<input id="BtnRun" type="button" value="运行" style=" width:100px;" onclick="passcheck()"  class="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<input id="BtnSave" type="button" value="保存" style=" width:100px;" onclick="save()" title="有代码保存会实现自动批改，无代码保存则删除自动批改"  class="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
<input id="BtnReturn" type="button" value="返回" style=" width:100px;" onclick="back()"  class="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" />
</div>
<p>
输入：<input id="input1" type="text" class="input px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300"  value=""/>
<pre id="output1" class="output" ></pre>
</p>

<p>
输入：<input id="input2" type="text"  class="input px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300"  value=""/>
<pre id="output2" class="output" ></pre>
</p>

<p>
输入：<input id="input3" type="text"  class="input px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300"  value=""/>
<pre id="output3" class="output" ></pre>
</p>
<p><strong>特别注意</strong>：<br>如果是while循环，记得只放第一个跳出循环的条件输入参数，将其它两个参数要留空，否则检验无效。检验只判断跳出循环的条件情况。</p>


</div>

<div id="savemsg"></div>
<div id="cv" ></div>

</div>

  <script src="../code/build/src/ace.js" type="text/javascript"></script>
  <script src="../code/build/src/ext-language_tools.js" type="text/javascript"></script>
  <script src="../code/build/src/ext-beautify.js" type="text/javascript"></script>
  

<script src="../code/skulpt.min.js" type="text/javascript"></script>
<script src="../code/skulpt-stdlib.js" type="text/javascript"></script>
<script src="../code/html2canvas.min.js" type="text/javascript"></script>
<script src="../code/jquery.min.js" type="text/javascript"></script>


    <script type="text/javascript">
        window.__judgeeditConfig = {
            id: '<%=Id %>',
            cid: '<%=Cid %>',
            mid: '<%=Mid %>',
            code: '<%=code %>',
            arg1: '<%=arg1 %>',
            arg2: '<%=arg2 %>',
            arg3: '<%=arg3 %>',
            fpage: '<%=Fpage %>'
        };
    </script>
    <script type="text/javascript" src="../js/judgeedit.js"></script>
</asp:Content>