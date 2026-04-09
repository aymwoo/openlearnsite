<%@ Page Language="C#" AutoEventWireup="true" CodeFile="code.aspx.cs" Inherits="python_code" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Python绘图编程</title>

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
</head>

<link href="../code/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
<link href="../js/toolbar-buttons.css" rel="stylesheet" type="text/css" />
<script src="../code/jquery.min.js" type="text/javascript"></script>
<script src="../code/html2canvas.min.js" type="text/javascript"></script>
<script src="../code/skulpt.min.js" type="text/javascript"></script>
<script src="../code/skulpt-stdlib.js" type="text/javascript"></script>
<script src="../code/build/src/ace.js" type="text/javascript"></script>
<script src="../code/build/src/ext-language_tools.js" type="text/javascript"></script>
<link rel="stylesheet" type="text/css" href="../code/idle.css"/>

<body>
<div class="container ">
    <div id="editor" class="ace-gruvbox ace_editor" > </div>
</div>

<div class="tooltip">
	<span class="keymodel" title="界面切换">❖</span>
	<span class="keyword" title="前进">forward</span>
	<span class="keyword" title="后退">backward</span>
	<span class="keyword" title="左转">left</span>
	<span class="keyword" title="右转">right</span>
	<span class="keyword" title="画圆">circle</span>
	<span class="keyword" title="抬笔">penup</span>
	<span class="keyword" title="落笔">pendown</span>
	<span class="keyword" title="画笔颜色">pencolor</span>
	<span class="keyword" title="画笔粗细">pensize</span>
	<span class="keyword" title="填充颜色">fillcolor</span>
	<span class="keyword" title="开始填充">begin_fill</span>
	<span class="keyword" title="结束填充">end_fill</span>
	<span class="keyword" title="回家">home</span>
	<span class="keybox" title="颜色选择">✿</span>
</div>

<div id="cv" class="cvdiv"></div>

<div id="content" >
<pre id="output" class="output" ></pre>
</div>

<div  id="run" onclick="run()" title="运行代码"> </div>
<div  id="big" onclick="fontbig()" title="放大代码"> </div>
<div  id="small" onclick="fontsmall()" title="缩小代码"> </div>
<div id="colorbox"></div>

<form id="form1" runat="server"> 
<div class="idle-toolbar">
<input id="title" type="text" value="未命名" disabled="disabled" class="idle-toolbar__title px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300" />
<button type="button" id="btnerase" class="idle-toolbar__btn idle-toolbar__btn--neutral" onclick="clearcv();">
<i class="fa fa-eraser" aria-hidden="true"></i><span>整理画布</span></button>
<button type="button" id="btnupload" class="idle-toolbar__btn idle-toolbar__btn--secondary" onclick="savecode();">
<i class="fa fa-save" aria-hidden="true"></i><span>保存作品</span></button>
<button type="button" id="btnreturn" class="idle-toolbar__btn idle-toolbar__btn--neutral" onclick="returnurl();">
<i class="fa fa-reply" aria-hidden="true"></i><span>返回学案</span></button>
</div>
    <script type="text/javascript">
        window.__codeConfig = {
            snum: "<%=Snum %>",
            mid: "<%=Mid %>",
            qid: "<%=Qid %>",
            id: "<%=Id %>",
            argout: "<%=argout %>",
            codefile: "<%=Codefile %>",
            title: "<%=title %>",
            argimg: "<%=argimg %>",
            fpage: "<%=Fpage %>"
        };
    </script>
    <script type="text/javascript" src="../js/code.js"></script>
</form>

<div class="maplock"><img class="mapimg" src="<%=Midurl %>" alt=""/></div>
<div id="savemsg"></div>

<script src="../code/idle.js"  type="text/javascript"></script>
<script src="../code/colorbox.js" type="text/javascript"></script>


</body>
</html>
