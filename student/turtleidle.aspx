<%@ Page Language="C#" AutoEventWireup="true" CodeFile="turtleidle.aspx.cs" Inherits="Student_turtleidle" ResponseEncoding="utf-8" %>

<html xmlns="http://www.w3.org/1999/xhtml"> 
<head id="Head1" runat="server">
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Python绘图编程</title>
    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
</head>

<link href="../code/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
<script src="../code/jquery.min.js" type="text/javascript"></script>
<script src="../code/html2canvas.min.js" type="text/javascript"></script>
<script src="../code/skulpt.min.js" type="text/javascript"></script>
<script src="../code/skulpt-stdlib.js" type="text/javascript"></script>
<script src="../code/build/src/ace.js" type="text/javascript"></script>
<script src="../code/build/src/ext-language_tools.js" type="text/javascript"></script>
<link rel="stylesheet" type="text/css" href="../code/idleturtle.css"/>
<link href="../js/toolbar-buttons.css" rel="stylesheet" type="text/css" />
<script src="../kindeditor/plugins/code/prettify.js" type="text/javascript"></script>
<link href="../kindeditor/plugins/code/prettify.css?ver=621" rel="stylesheet" type="text/css" />

<body  onload="prettyPrint(); ">

<div class="main" >
	<div id="done"><img src="../images/sucessed.png"></img></div>
	<div class="description">
		<h2 ><%=Titles%></h2>
		<div id="content" >
		<div class="map"><img class="mapimg" src="<%=Midurl %>" alt=""/></div>
				<%=Mcontents %>
				<br /><br />   
		</div>	
	</div>
	<div id="editor" class="ace-gruvbox ace_editor" > </div>
	<div class="right">
		<div id="cv" class="cvdiv"></div>
	</div>
</div>

<div class="tooltip"></div>
<div  id="big" onclick="fontbig()" title="放大代码"> </div>
<div  id="small" onclick="fontsmall()" title="缩小代码"> </div>
<div id="colorbox"></div>
<div id="codexample">
	<div id="codeplace"></div>
	<div id="codebutton">
	<button type="button" class="btncode idle-subbtn" id="prev">上一页</button><button type="button" class="btncode idle-subbtn" id="next">下一页</button>
	</div>
</div>

<form id="form1" runat="server"> 
<div class="idle-toolbar">
<button type="button" id="btnclear" class="idle-toolbar__btn idle-toolbar__btn--neutral" onclick="clearcv()">
<i class="fa fa-eraser" aria-hidden="true"></i><span>整理画布</span></button>
<button type="button" id="btnrun" class="idle-toolbar__btn" onclick="run()">
<i class="fa fa-play-circle" aria-hidden="true"></i><span>运行代码</span></button>
<button type="button" id="btnsave" class="idle-toolbar__btn idle-toolbar__btn--secondary" onclick="savecode()">
<i class="fa fa-save" aria-hidden="true"></i><span>保存作品</span></button>
<button type="button" id="btnreturn" class="idle-toolbar__btn idle-toolbar__btn--neutral" onclick="returnurl()">
<i class="fa fa-reply" aria-hidden="true"></i><span>返回学案</span></button>
</div>
    <script type="text/javascript">
        window.__turtleidleConfig = {
            snum: "<%=Snum %>",
            id: "<%=Id %>",
            codefile: "<%=Codefile %>",
            argimg: "<%=argimg %>",
            fpage: "<%=Fpage %>",
            mhelp: "<%=mhelp %>"
        };
    </script>
    <script type="text/javascript" src="../js/turtleidle.js"></script>
</form>

<div id="savemsg"></div>

<script src="../code/idle.js"  type="text/javascript"></script>
<script src="../code/colorbox.js" type="text/javascript"></script>
<script src="../code/example.js" type="text/javascript"></script>


</body>
</html>
