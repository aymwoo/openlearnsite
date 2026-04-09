<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pythonblock.aspx.cs" Inherits="student_pythonblock" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server"> 
      <meta charset="utf-8" />
<title></title>
<link href="../code/block.css" rel="stylesheet" type="text/css" />
<link href="../code/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
    <script src="../js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="../js/jquery-ui-1.8.24.custom.min.js" type="text/javascript"></script>
<script src="../code/build/src/ace.js" type="text/javascript"></script>
<script src="../code/build/src/ext-language_tools.js" type="text/javascript"></script>
<script src="../code/skulpt.min.js?ver=20211202" type="text/javascript"></script>
<script src="../code/skulpt-stdlib.js" type="text/javascript"></script>
<script src="../code/html2canvas.min.js" type="text/javascript"></script>
	<link rel="stylesheet" href="../deepseek/all.min.css">
	

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Student/pythonblock.css" />
</head>
<body>
<div>
<div class="banner">
	 <i class="fa fa-codepen" ></i> <a id="title" >Python拼图编程：<%=Titles%></a>
</div>
<div id="main">
<div id="left">
&nbsp;请将第一块积木拖放到这里！
</div>
<div id="right"></div>
</div>

<div id="content" >
	<%=Mcontents %>
	<br /><br />
</div>
<div id="result">
<div id="savemsg"></div>
<pre id="output" ></pre>
</div>
<div id="cv" ></div>
<div  id="big" onclick="fontbig()" title="放大代码"> </div>
<div  id="small" onclick="fontsmall()" title="缩小代码"> </div>
<div class="map"><img class="mapimg" src="<%=Midurl %>" alt=""/></div>
<div id="done"><img src="../images/sucessed.png"></img></div>
<audio id="myaudio" src="../code/adsorb.ogg" controls="controls"  hidden="true" ></audio>
<div id="sideby" class="block-toolbar">
<button  onclick="helper()" class="block-toolbar__btn" type="button">
<i class="fa fa-book" aria-hidden="true"></i><span>查看学案</span></button>
<button  onclick="runit()" class="block-toolbar__btn" type="button">
<i class="fa fa-play-circle" aria-hidden="true"></i><span>运行代码</span></button>
<button  onclick="savecode()" class="block-toolbar__btn block-toolbar__btn--secondary" type="button">
<i class="fa fa-save" aria-hidden="true"></i><span>保存作品</span></button>
<button  onclick="returnurl()" class="block-toolbar__btn block-toolbar__btn--neutral" type="button">
<i class="fa fa-reply" aria-hidden="true"></i><span>返回学案</span></button>
</div>
	

    </div>
    <script type="text/javascript">
        window.__pythonblockConfig = {
            snum: "<%=Snum %>",
            id: "<%=Id %>",
            fpage: "<%=Fpage %>",
            argcode: "<%=argcode %>",
            argin: "<%=argin %>",
            argout0: "<%=argout0 %>",
            argout1: "<%=argout1 %>",
            argout2: "<%=argout2 %>",
            mpass: "<%=mpass %>"
        };
    </script>
    <script type="text/javascript" src="../js/pythonblock.js"></script>
</body>
</html>
