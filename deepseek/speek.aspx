<%@ Page Language="C#" AutoEventWireup="true" CodeFile="speek.aspx.cs" Inherits="deepseek_speek" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html lang="zh-CN">
<head  runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>人工智能 - 语音合成技术</title>
    <link rel="stylesheet" href="deepseek.css">
    <script src="../code/jquery.min.js" type="text/javascript"></script>
    <script src="../code/html2canvas.min.js" type="text/javascript"></script>
	
	

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Teacher/speek.css" />
</head>
<body>
    <div class="container">		
        <div class="left-column">		
			<!-- 聊天区域 -->
			<div class="chat-container" >
				<!-- 聊天记录将动态加载到这里🐳  -->
				<div class="wall">
				<h2><img  src="../deepseek/speek.png" onclick="example()" /> 人工智能 - 语音合成，很高兴遇见你！
				</h2>
				</div>
				<div id="chatHistory">
				
				</div>
			</div>
			
			<!-- 输入框区域 -->
			<div class="loading-container">
			<!-- 加载状态 -->
			<div id="loading" style="display: none;"><img src="../deepseek/loading.gif" />语音合成中...</div>
			
			</div>

			<!-- 输入框区域 -->
			<div class="input-container">
				<textarea id="userInput" placeholder="输入你的问题..." rows="3"  maxlength="1000" ></textarea>
				<div>
				<div class="ai-toolbar">
				<button id="btnmsg" onclick="sendText()" title="合成语音" class="ai-toolbar__btn ai-toolbar__btn--secondary" type="button"><i class="fa fa-microphone" aria-hidden="true"></i><span>合成语音</span></button>
				</div>
				<div style="margin-top:10px;">
				<select id="voiceSelect" title="选择发音人">
					<option value="zh-CN-XiaoxiaoNeural">晓晓 温暖 女</option>
					<option value="zh-CN-XiaoyiNeural">小艺 活泼 女</option>
					<option value="zh-CN-YunjianNeural">云健 稳重 男</option>
					<option value="zh-CN-YunxiNeural">云溪 阳光 男</option>
					<option value="zh-CN-YunxiaNeural">云夏 可爱 男</option>
					<option value="zh-CN-YunyangNeural">云阳 专业 男</option>
					<option value="zh-CN-liaoning-XiaobeiNeural">辽宁 小北 女</option>
					<option value="zh-CN-shaanxi-XiaoniNeural">陕西 小妮 女</option>
					<option value="zh-HK-HiuGaaiNeural">香港 晓佳 女</option>
					<option value="zh-HK-HiuMaanNeural">香港 晓文 女</option>
					<option value="zh-HK-WanLungNeural">香港 万龙 男</option>
					<option value="zh-TW-HsiaoChenNeural">台湾 晓晨 女</option>
					<option value="zh-TW-HsiaoYuNeural">台湾 晓雨 女</option>
					<option value="zh-TW-YunJheNeural">台湾 云哲 男</option>
				</select>
				</div>
				</div>
				
			</div>
        </div>	
		
        <div class="right-column">
			<!-- 导航栏 -->
			<div class="navbar">
			<img src="speek.png" /> 历史对话记录
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
        window.__speekConfig = {
            id: "<%=Id %>",
            fpage: "<%=Fpage %>"
        };
    </script>
    <script type="text/javascript" src="../js/speek.js"></script>
</body>
</html>
