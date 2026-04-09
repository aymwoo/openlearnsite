<%@ Page Language="C#" AutoEventWireup="true" CodeFile="turtle.aspx.cs" Inherits="Python_turtle" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Python绘画编程</title>
  <link href="../code/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
  <link href="../code/site.css" rel="stylesheet" type="text/css" />
  <link href="../js/toolbar-buttons.css" rel="stylesheet" type="text/css" />
    <script src="../kindeditor/plugins/code/prettify.js" type="text/javascript"></script>
    <link href="../kindeditor/plugins/code/prettify.css?ver=621" rel="stylesheet" type="text/css" />

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
</head>

<body  onload="prettyPrint(); ">

<div class="banner">
	<i class="fa fa-codepen" ></i> <input id="title"  type="text"  value="Python 绘画编程" class="px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300" />
</div>
<div class="main" >
	<div class="left" id="editor"></div>
	<div class="right">
		<div id="result">
		<pre id="output" ></pre>
		<div id="cv" ></div>
		</div>
	</div>
</div>

<div  id="memory" onclick="remember()" title="回放代码" > </div>
<div  id="big" onclick="fontbig()" title="放大代码"> </div>
<div  id="small" onclick="fontsmall()" title="缩小代码"> </div>
<div id="colorbox"></div>

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

<div id="savemsg"></div>

<div class="py-toolbar">
<button type="button" onclick="runit()" class="py-toolbar__btn">
<i class="fa fa-caret-right" aria-hidden="true"></i><span>运行代码</span></button>
<button type="button" onclick="savecode()" class="py-toolbar__btn py-toolbar__btn--secondary">
<i class="fa fa-save" aria-hidden="true"></i><span>保存作品</span></button>
<button type="button" onclick="returnurl()" class="py-toolbar__btn py-toolbar__btn--neutral">
<i class="fa fa-reply" aria-hidden="true"></i><span>返回学案</span></button>
</div>
</div>

<script src="../code/colorbox.js" type="text/javascript"></script>
<script src="../code/skulpt.min.js?ver=20211202" type="text/javascript"></script>
<script src="../code/skulpt-stdlib.js" type="text/javascript"></script>
<script src="../code/html2canvas.min.js" type="text/javascript"></script>
<script src="../code/jquery.min.js" type="text/javascript"></script>

<script src="../code/build/src/ace.js" type="text/javascript"></script>
<script src="../code/build/src/ext-language_tools.js" type="text/javascript"></script>
<script src="../code/build/src/ext-beautify.js" type="text/javascript"></script>


    <script type="text/javascript">
        window.__turtleConfig = {
            snum: "<%=Snum %>",
            id: "<%=Id %>",
            codefile: "<%=Codefile %>",
            title: "<%=title %>"
        };
    </script>
    <script type="text/javascript" src="../js/turtle.js"></script>
</body>
</html>
