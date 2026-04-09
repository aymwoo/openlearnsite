<%@ Page Language="C#" AutoEventWireup="true" CodeFile="deepseek.aspx.cs" Inherits="deepseek_deepseek" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html lang="zh-CN">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>人工智能 - 探索未知之境</title>
    <link rel="stylesheet" href="deepseek.css">
    <!-- Markdown 渲染库 -->
    <script src="marked.min.js"></script>
    <!-- 代码高亮库 -->
    <link rel="stylesheet" href="github-dark.min.css">
    <script src="highlight.min.js"></script>
    <!-- Font Awesome 图标库 -->
    <link rel="stylesheet" href="all.min.css">
    <script src="../code/jquery.min.js" type="text/javascript"></script>
    <script src="../code/html2canvas.min.js" type="text/javascript"></script>
	
	

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Teacher/deepseek.css" />
</head>
<body>
    <div class="container" id="maincontainer">		
        <div class="left-column">		
			<!-- 聊天区域 -->
			<div class="chat-container" id="chatHistory">
				<!-- 聊天记录将动态加载到这里🐳  -->
				<div class="wall">
				<h2><img class ="logo" src="deepseek.svg" /> 我是 人工智能 DeepSeek，很高兴见到你！</h2>
				<div>我可以帮你写作、编程、绘画等各种创意内容，请把你的任务交给我吧~</div>
				</div>
			</div>
			
			<!-- 输入框区域 -->
			<div class="loading-container">
			<!-- 加载状态 -->
			<div id="loading" style="display: none;"><img src="loading.gif" />思考中...</div>
			
			</div>

			<!-- 输入框区域 -->
			<div class="input-container">
				<textarea id="userInput" placeholder="输入你的问题..." rows="2" maxlength="500"></textarea>
				<div class="ai-toolbar">
				<button id="btnmsg" onclick="sendMessage()" title="人工智能聊天" class="ai-toolbar__btn" type="button"><i class="fa fa-comments" aria-hidden="true"></i><span>智能对话</span></button>
				<button id="btntran" onclick="sendChinese()" title="中文翻译为英文" class="ai-toolbar__btn ai-toolbar__btn--secondary" type="button"><i class="fa fa-language" aria-hidden="true"></i><span>翻译润色</span></button>
				<button id="btnphoto" style="display: none;" onclick="sendPhoto()" title="文本生成图片" class="ai-toolbar__btn" type="button"><i class="fa fa-image" aria-hidden="true"></i><span>生成图片</span></button>
				</div>
			</div>
        </div>	
		
        <div class="right-column">
			<!-- 导航栏 -->
			<div class="navbar">
			<img class ="logo" src="deepseek.svg" /> 人工智能 DeepSeek
			</div>
			<!-- 聊天记录栏 -->
			<div id ="chatbar">			
			</div>			
            <div id="footbar">
				<div class="ai-toolbar">
				<button type="button" onclick="savechat()" class="ai-toolbar__btn ai-toolbar__btn--secondary" title="保存作品到服务器" >
				<i class="fa fa-save" aria-hidden="true"></i> 保存作品</button>
				<button onclick="returnurl()" class="ai-toolbar__btn ai-toolbar__btn--neutral" title="返回到学案页面" type="button">
				<i class="fa fa-reply" aria-hidden="true"></i> 返回学案</button>            
				</div>
            </div>
        </div>

 </div>
    
    <script type="text/javascript">
        window.__deepseekConfig = {
            id: "<%=Id %>",
            fpage: "<%=Fpage %>"
        };
    </script>
    <script type="text/javascript" src="../js/deepseek.js"></script>
</body>
</html>
