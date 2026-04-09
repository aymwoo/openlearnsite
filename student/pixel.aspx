<%@ Page Language="C#" AutoEventWireup="true"  ValidateRequest="false" EnableViewStateMac="false"  CodeFile="pixel.aspx.cs" Inherits="Student_pixel" ResponseEncoding="utf-8" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
      <meta charset="utf-8" />
<title>Pixel Art Maker 像素艺术画</title>
  <meta name="viewport" content="width=device-width, initial-scale=1"/>
  <link rel="stylesheet" href="../pixelartmaker/style.css"/>  
  <link rel="stylesheet" href="../deepseek/all.min.css">
	<style type="text/css">
		.pixel-toolbar {
			display: inline-flex;
			flex-wrap: wrap;
			gap: 10px;
			align-items: center;
			margin-left: 18px;
			vertical-align: middle;
		}

		.pixel-toolbar__btn {
			display: inline-flex;
			align-items: center;
			justify-content: center;
			gap: 8px;
			min-width: 110px;
			height: 40px;
			padding: 0 14px;
			border: 0;
			border-radius: 12px;
			background: linear-gradient(135deg, #2563eb 0%, #1d4ed8 100%);
			color: #ffffff;
			font-size: 14px;
			font-weight: 700;
			white-space: nowrap;
			box-shadow: 0 14px 28px -18px rgba(37, 99, 235, 0.82);
			cursor: pointer;
			transition: transform .2s ease, box-shadow .2s ease, filter .2s ease;
		}

		.pixel-toolbar__btn:hover {
			transform: translateY(-1px);
			box-shadow: 0 18px 30px -18px rgba(37, 99, 235, 0.9);
			filter: brightness(1.03);
		}

		.pixel-toolbar__btn--secondary {
			background: linear-gradient(135deg, #0f766e 0%, #0f766e 100%);
			box-shadow: 0 14px 28px -18px rgba(15, 118, 110, 0.78);
		}

		.pixel-toolbar__btn--neutral {
			background: linear-gradient(135deg, #475569 0%, #334155 100%);
			box-shadow: 0 14px 28px -18px rgba(51, 65, 85, 0.72);
		}

		.pixel-toolbar__btn:disabled {
			opacity: .68;
			cursor: wait;
			transform: none;
		}

		@media (max-width: 900px) {
			.pixel-toolbar {
				display: flex;
				margin: 14px 0 0;
			}

			.pixel-toolbar__btn {
				flex: 1 1 110px;
				min-width: 0;
			}
		}
	</style>

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
</head>
<body>

<div id="main">
  <div id="pick">
	  <div class="left">  
		<img id="logo" src="../pixelartmaker/logo.png"  alt="像素蘑菇小兵！"/>  
	  </div>
	  <div class="right">
		<h1 > 
		Pixel Art Maker 像素画 <input type="color" id="colorPicker"/>
		<span class="pixel-toolbar">
			<button id="savebtn" class="pixel-toolbar__btn" type="button"><i class="fa fa-save" aria-hidden="true"></i><span>保存作品</span></button>
			<button id="playbtn" class="pixel-toolbar__btn pixel-toolbar__btn--secondary" type="button"><i class="fa fa-play" aria-hidden="true"></i><span>播放预览</span></button>
			<button id="returnbtn" class="pixel-toolbar__btn pixel-toolbar__btn--neutral" type="button"><i class="fa fa-reply" aria-hidden="true"></i><span>返回学案</span></button>
		</span>
		 </h1>
			<table id="palette"></table> 		 
	  </div>
  </div>
<div id="petcolor" >
	<table id="paletteB"></table>
</div>
 
<table id="pixel_canvas"></table>

<div id="framelist">
	<div id="frameadd" >
		<div id="plus" title="复制当前帧">+</div>
		<div id="minus" title="删除当前帧">-</div>
	</div>
		<div id="fm1" class="frame"></div>
</div>
</div>

<audio id="myaudio" src="../pixelartmaker/music.mp3" autoplay="autoplay" hidden="true" ></audio>	

<script type="text/javascript" >
    var id = "<%=Id %>";
    var pixfile = "<%=PixFile %>";

    function returnurl() {
        if (confirm('是否离开当前活动页面？请先保存作品。') == true) {
            window.location.href = "<%=Fpage %>"
        }
    }
</script>
<script src='../pixelartmaker/lz-string-1.4.4.js' type="text/javascript" ></script>
<script src='../pixelartmaker/jquery.min.js' type="text/javascript" ></script>
<script src='../pixelartmaker/jquery-ui.js' type="text/javascript" ></script>
<script src="../pixelartmaker/html2canvas.min.js" type="text/javascript" ></script>
<script src="../pixelartmaker/pixelartmaker.js" type="text/javascript" ></script>
<script src='../pixelartmaker/gif.js' type="text/javascript" ></script>
<script src='../pixelartmaker/gif.worker.js' type="text/javascript" ></script>
</body>
</html>
