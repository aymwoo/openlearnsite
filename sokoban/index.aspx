<%@ Page Language="C#" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="sokoban_index" %>

<!doctype html>
<html>
	<head>
		<meta charset="UTF-8">
		<meta name="Keywords" content="关键词">
		<meta name="Description" content="描述">
		<title>推箱子</title>
		
	
    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Teacher/index.css" />
</head>
	<body onkeydown="doKeyDown(event)"><!--身体-->
	<div id="main" >
		<div class="banner">
			<div id="msg"></div>
			<div id="btntool">
				<input id="btnmove" type="button" class="button px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" value="历史记录" onclick="automove()">
				<input type="button" class="button px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" value="重新开始" onclick="NextLevel(0)">
				<input type="button" class="button px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0"  value="撤消一步" onclick="showBack()">		
			</div>
		</div>
		<div  class="game" >
			<canvas id="canvas" class="canvas" width="608" height="608"></canvas>
		</div>
		<div id="move" class="move"> 
		移动记录：
		</div>
        <div id="rank" class="move"> 
		英雄榜：
		</div>
		<audio id="myaudio" src="../sokoban/sound/music.ogg" controls="controls" autoplay loop="true" hidden="true" ></audio>		
		<audio id="audio" controls="controls"  hidden="true" ></audio>
	</div>	
    <script type="text/javascript">
        window.__indexConfig = {
            gsave: "<%=gsave %>",
            gnote: "<%=gnote %>",
            gstart: "<%=gstart %>",
            gpass: "<%=gpass %>",
            grank: "<%=grank %>"
        };
    </script>
    <script type="text/javascript" src="../js/index.js"></script>
	</body>
	<script src="../code/jquery.min.js" type="text/javascript"></script>
	<script src="js/mapdata100.js"></script>
	
</html>


