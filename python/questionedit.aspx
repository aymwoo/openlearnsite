<%@ Page Language="C#" AutoEventWireup="true" CodeFile="questionedit.aspx.cs" Inherits="python_questionedit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>    
    <link href="../code/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <link href="../js/toolbar-buttons.css" rel="stylesheet" type="text/css" />
    

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Teacher/questionedit.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>

<div id="divmain">

<div id="divleft">
	<div id="editor"></div>
</div>

<div id="divright">
<div id="title">
<h2 style="text-align:center;"> 
    题目：<asp:TextBox ID="TextBoxTitle" runat="server" Width="80%" CssClass="px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300"></asp:TextBox>
    </h2>
<div class="ls-toolbar question-toolbar">
<button id="BtnRun" type="button" onclick="passcheck()" class="ls-toolbar__btn">
<i class="fa fa-play-circle" aria-hidden="true"></i><span>运行代码</span></button>
<button id="BtnSave" type="button" onclick="save()" class="ls-toolbar__btn ls-toolbar__btn--secondary">
<i class="fa fa-save" aria-hidden="true"></i><span>保存作品</span></button>
<button id="BtnReturn" type="button" onclick="back()" class="ls-toolbar__btn ls-toolbar__btn--neutral">
<i class="fa fa-reply" aria-hidden="true"></i><span>返回学案</span></button>
</div>

<div id="savemsg"></div>
</div>
</div>

<pre id="output" class="output" ></pre>
<div id="cv" ></div>

</div>

  <script src="../code/build/src/ace.js" type="text/javascript"></script>
  <script src="../code/build/src/ext-language_tools.js" type="text/javascript"></script>
  <script src="../code/build/src/ext-beautify.js" type="text/javascript"></script>
  

<script src="../code/skulpt.min.js" type="text/javascript"></script>
<script src="../code/skulpt-stdlib.js" type="text/javascript"></script>
<script src="../code/html2canvas.min.js" type="text/javascript"></script>
<script src="../code/jquery.min.js" type="text/javascript"></script>

    </div>
    <script type="text/javascript">
        window.__questioneditConfig = {
            id: "<%=Id %>",
            mid: "<%=Mid %>",
            code: "<%=code %>",
            fpage: "<%=Fpage %>"
        };
    </script>
    <script type="text/javascript" src="../js/questionedit.js"></script>
    </form>
</body>
</html>
