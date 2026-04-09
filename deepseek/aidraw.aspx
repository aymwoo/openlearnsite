<%@ Page Language="C#" AutoEventWireup="true" CodeFile="aidraw.aspx.cs" Inherits="deepseek_aidraw" %>

<html lang="zh-CN">
<head id="Head1"  runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>AI绘画- 探索未知之境</title>
    <link rel="stylesheet" href="deepseek.css">
    <!-- Font Awesome 图标库 -->
    <link rel="stylesheet" href="all.min.css">
    <script src="../code/jquery.min.js" type="text/javascript"></script>
    <script src="../code/html2canvas.min.js" type="text/javascript"></script>
	
	

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Teacher/aidraw.css" />
</head>
<body>
    <div class="container" id="maincontainer">		
        <div class="left-column">		
			<!-- 聊天区域 -->
				<h2 style="text-align:center;" ><img class ="logo"  src="draw.png" /> AI绘画，想象与创意</h2>			
			<!-- 输入框区域 -->
			<div class="loading-container">
			<!-- 加载状态 -->
			<div id="loading" style="display: none;"><img src="loading.gif" />思考中...</div>
			
			</div>
                <div class="prompt-helper">
                    <h3>绘画提示词</h3>
                    <div class="prompt-tags-container">
                        <div class="prompt-tag" data-prompt="一只可爱的猫咪在玩毛线球">🐱猫咪玩毛线球</div>
                        <div class="prompt-tag" data-prompt="一条彩色的小鱼在海底游泳，周围有珊瑚">🐟彩色小鱼游泳</div>
                        <div class="prompt-tag" data-prompt="一个宇航员在太空中漂浮">👨‍🚀宇航员太空漂浮</div>
                        <div class="prompt-tag" data-prompt="一座魔法城堡周围有绿色的森林和小精灵">🏰魔法城堡和精灵</div>
                        <div class="prompt-tag" data-prompt="一只恐龙在森林里吃水果">🍎恐龙吃水果</div>
                        <div class="prompt-tag" data-prompt="一个卡通风格小女孩在堆雪人">⛄小女孩堆雪人</div>
                        <div class="prompt-tag" data-prompt="一艘宇宙飞船正在星际航行">🚀宇宙飞船航行</div>
                        <div class="prompt-tag" data-prompt="一只热气球飞越山脉，山下有小村庄">🎈热气球飞行</div>
                        <div class="prompt-tag" data-prompt="一个机器人在玩足球">⚽机器人踢足球</div>
                        <div class="prompt-tag" data-prompt="一个水下城市，有鱼儿和美人鱼">🏰水下城市</div>
                        <div class="prompt-tag" data-prompt="一个童话森林，有大大的蘑菇房子和小动物">🍄童话森林</div>
                    </div>
                </div>
				
			<!-- 输入框区域 -->
			<div class="input-container">
				<textarea id="userInput" placeholder="一只可爱的小狗在草地上玩耍，旁边有蝴蝶在飞舞" rows="1"></textarea>
				<div class="ai-toolbar">
				<button id="btnphoto" onclick="aiPhoto()" title="文本生成图片" class="ai-toolbar__btn" type="button"><i class="fa fa-image" aria-hidden="true"></i><span>生成图片</span></button>
				<button  type = "button" onclick="savechat()" class="ai-toolbar__btn ai-toolbar__btn--secondary"  title="保存作品到服务器" >
				<i class="fa fa-save" aria-hidden="true"></i> 保存作品</button>
				<button  onclick="returnurl()" class="ai-toolbar__btn ai-toolbar__btn--neutral" title="返回到学案页面" type="button">
				<i class="fa fa-reply" aria-hidden="true"></i> 返回学案</button> 
				</div>
			</div>
			
			<div class="photo-container" id="chatHistory">
				<!-- 聊天记录将动态加载到这里🐳  -->
			</div>
        </div>	
		
        <div class="right-column">
			<!-- 导航栏 -->
			<div class="photobar">
			绘画作品列表
			</div>
			<!-- 聊天记录栏 -->
			<div id ="chatbar">			
			</div>			
            <div id="footbar">           
            </div>
        </div>

 </div>
    
    <script type="text/javascript">
        window.__aidrawConfig = {
            id: "<%=Id %>",
            fpage: "<%=Fpage %>"
        };
    </script>
    <script type="text/javascript" src="../js/aidraw.js"></script>
</body>
</html>
