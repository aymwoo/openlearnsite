<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ocr.aspx.cs" Inherits="deepseek_ocr" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html runat="server" lang="zh-CN">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>人工智能 - 文字识别技术 OCR</title>
    <link rel="stylesheet" href="deepseek.css">
    <script src="../code/jquery.min.js" type="text/javascript"></script>
    <script src="../code/html2canvas.min.js" type="text/javascript"></script>
	
	<link rel="stylesheet" href="all.min.css">
	

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Teacher/ocr.css" />
</head>
<body>
    <div class="container">		
        <div class="left-column">		
			<!-- 聊天区域 -->
			<div class="chat-container" id="chatHistory">
				<!-- 聊天记录将动态加载到这里🐳  -->
				<div class="wall">
				<h2><img  src="ocr.png" /> 人工智能 - 文字识别 OCR，很高兴见到你！
				</h2>
				</div>
			</div>
			
			<!-- 输入框区域 -->
			<div class="loading-container">
			<!-- 加载状态 -->
			<div id="loading" style="display: none;"><img src="loading.gif" />文字识别中...</div>
			
			</div>

			<!-- 输入框区域 -->
			<div class="input-container">
				<div class="other">
					<input type="file" name="file" id="userfile" accept="image/*" hidden>
					<div class="ai-toolbar">
					<button id="uploadbtn" class="ai-toolbar__btn" type="button"><i class="fa fa-upload" aria-hidden="true"></i><span>上传图片</span></button>
					<button id="btnmsg" onclick="sendText()" title="文字识别" class="ai-toolbar__btn ai-toolbar__btn--secondary" type="button"><i class="fa fa-font" aria-hidden="true"></i><span>文字识别</span></button>
					</div>
				</div>
				
			</div>
        </div>	
		
        <div class="right-column">
			<!-- 导航栏 -->
			<div class="navbar">
			<img src="ocr.png" /> 历史对话记录
			</div>
			<!-- 聊天记录栏 -->
			<div id ="chatbar">			
			</div>			
            <div id="footbar">
				<div class="ai-toolbar">
				<button  type = "button" onclick="savechat()" class="ai-toolbar__btn ai-toolbar__btn--secondary"  title="保存作品到服务器" >
				<i class="fa fa-save" aria-hidden="true"></i> 保存作品</button>
				<button  onclick="returnurl()" class="ai-toolbar__btn ai-toolbar__btn--neutral" title="返回到学案页面" type="button">
				<i class="fa fa-reply" aria-hidden="true"></i> 返回学案</button>            
				</div>
            </div>
        </div>

 </div>
    
    <script type="text/javascript">
        window.__ocrConfig = {
            id: "<%=Id %>",
            fpage: "<%=Fpage %>"
        };
    </script>
    <script type="text/javascript" src="../js/ocr.js"></script>
</body>
</html>
