<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pythonblockly.aspx.cs" Inherits="student_pythonblockly" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">  
  <title>Python Blockly 积木编程</title>
  <meta charset="UTF-8">
	<link href="../code/blockpy/blockpy.css" rel="stylesheet" type="text/css" />
	<link href="../code/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
	<script src="../js/jquery-1.8.2.min.js" type="text/javascript"></script>
	<script src="../js/jquery-ui-1.8.24.custom.min.js" type="text/javascript"></script>
	<script src="../code/build/src/ace.js" type="text/javascript"></script>
	<script src="../code/build/src/ext-language_tools.js" type="text/javascript"></script>
	<script src="../code/skulpt.min.js?ver=20211202" type="text/javascript"></script>
	<script src="../code/skulpt-stdlib.js" type="text/javascript"></script>
    <script src="../code/blockpy/blockly_compressed.js"></script>
    <script src="../code/blockpy/blocks_compressed.js"></script>
    <script src="../code/blockpy/python_compressed.js"></script>
    <script src="../code/blockpy/msg/en.js"></script>
<script src="../code/blockpy/storage.js"></script>
<script src="../code/html2canvas.min.js" type="text/javascript"></script>
	<link rel="stylesheet" href="../deepseek/all.min.css">
	

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Student/pythonblockly.css" />
</head>
<body>
<div>
<div class="banner">
	 <i class="fa fa-codepen" ></i> <a id="title" >Python Blockly 积木编程</a> &nbsp;  
</div>
<div id="main">
<div id="left">
<div id="blocklyDiv" style="height: 95vh; width: 100wh;"></div>
</div>
<div id="right">
	<div id="editor"></div>
</div>

<div id="done"><img src="../images/sucessed.png"></img></div>
<div id="content" >
    <div class="map"><img class="mapimg" src="<%=Midurl %>" alt=""/></div>
	<h2 ><%=Titles%></h2>  
	<%=Mcontents %>
	<br /><br />
</div>

</div>


<div id="result">
<div id="savemsg"></div>
<pre id="output" ></pre>
</div>
<div id="cv" ></div>
<audio id="audio" controls="controls"  hidden="true" ></audio>
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

<xml  id="toolbox" style="display: none">
  <category name="编程" colour="#5b80a5">
	<block type="py_start"></block> 
	<block type="py_input"></block>
	<block type="py_print"></block>
	<block type="py_assign"></block> 
	<block type="py_text"></block> 
    <block type="math_number">
		<field name="NUM">0</field>
    </block>
	<block type="py_data"></block> 
	<block type="py_range"></block> 
	<block type="py_for"></block>
    <block type="controls_if"></block>    
    <block type="py_while"></block> 
    <block type="py_break"></block> 
  </category> 
  <category name="绘图" colour="#5ba55b">
    <block type="py_import"></block>  
    <block type="turtle_move"></block> 
    <block type="turtle_turn"></block>	
    <block type="turtle_circle"></block> 
    <block type="turtle_pensize"></block> 
    <block type="turtle_pen"></block> 
    <block type="turtle_fill"></block> 
    <block type="turtle_color"></block>
    <block type="turtle_goto"></block>
    <block type="turtle_fun"></block>
    <block type="turtle_write"></block> 
    <block type="turtle_sleep"></block> 
  </category> 
</xml>


	

	
	
    </div>
    <script type="text/javascript">
        window.__pythonblocklyConfig = {
            snum: "<%=Snum %>",
            mback: "<%=mback %>",
            codefile: "<%=codefile %>",
            id: "<%=Id %>",
            fpage: "<%=Fpage %>",
            argcode: "<%=argcode %>",
            argin: "<%=argin %>",
            argout0: "<%=argout0 %>",
            argout1: "<%=argout1 %>",
            argout2: "<%=argout2 %>",
            argimg: "<%=argimg %>",
            mhelp: "<%=mhelp %>"
        };
    </script>
    <script type="text/javascript" src="../js/pythonblockly.js"></script>
</body>
</html>
