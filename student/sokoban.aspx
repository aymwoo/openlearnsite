<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sokoban.aspx.cs" Inherits="student_sokoban" ResponseEncoding="utf-8" %>

<!doctype html>
<html>
<head id="Head1" runat="server">
		<meta charset="UTF-8">
		<meta name="Keywords" content="关键词">
		<meta name="Description" content="描述">
		<title>推箱子地图编辑器</title>
		
	<link rel="stylesheet" href="../deepseek/all.min.css">
	
    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Student/sokoban.css" />
</head>
	<body onkeydown="doKeyDown(event)"><!--身体-->
	<div id="main" >
		<div class="banner">
			<div id="msg">☸ 推箱子地图编辑器 ☸</div>
			<div id="btntool">
				<div class="sokoban-toolbar">
				<input type="button" class="sokoban-toolbar__btn" value="重新开始" onclick="NextLevel(0)">
				<input type="button" class="sokoban-toolbar__btn" value="撤消一步" onclick="showBack()">	
				<input type="button" class="sokoban-toolbar__btn sokoban-toolbar__btn--secondary" id="savebtn" value="保存地图" onclick="saveMap()">	
				<input type="button" class="sokoban-toolbar__btn" value="清空地图" onclick="clearMap()">	
				<input type="button" class="sokoban-toolbar__btn" value="撤消" title="撤消" onclick="backWord()">	
				<input type="button" class="sokoban-toolbar__btn" value="恢复" title="恢复" onclick="forWord()">		
				<input type="button" class="sokoban-toolbar__btn sokoban-toolbar__btn--neutral" value="返回学案" onclick="returnurl()">
				</div>
			</div>
		</div>
		<div  class="game" >
			<canvas id="canvas" class="canvas" width="608" height="608"></canvas>
		</div>
		<div id="move" class="move"> 
		<table>
			<tr>
				<td class="sp" data-num="0" ><img src="../sokoban/images/block.png" title="草地" /></td>
				<td class="sp" data-num="1" ><img src="../sokoban/images/wall.png" title="树木"  /></td>
				<td class="sp" data-num="2" ><img src="../sokoban/images/ball.png" title="目标"   /></td>
				<td class="sp" data-num="3" ><img src="../sokoban/images/box.png"  title="箱子"  /></td>
				<td class="sp" data-num="4" ><img src="../sokoban/images/down.png" title="人物"   /></td>
			</tr>			
			<tr>
				<td>0</td>
				<td>1</td>
				<td>2</td>
				<td>3</td>
				<td>4</td>
			</tr>			
			<tr>
				<td>草地</td>
				<td>树木</td>
				<td>目标</td>
				<td>箱子</td>
				<td>人物</td>
			</tr>
		</table>			
		
		</div>	

		<table id="mapTable" class="map"></table>

		<audio id="myaudio" src="../sokoban/sound/music.ogg" controls="controls" loop="true" hidden="true" ></audio>		
		<audio id="audio" controls="controls"  hidden="true" ></audio>
	</div>	
    <script type="text/javascript">
        window.__sokobanConfig = {
            mapData: "<%=mapData %>",
            id: "<%=Id %>",
            fpage: "<%=Fpage %>"
        };
    </script>
    <script type="text/javascript" src="../js/sokoban.js"></script>
	</body>
	<script src="../code/jquery.min.js" type="text/javascript"></script>
	<script src="../sokoban/js/mapdata.js"></script>
	
</html>
