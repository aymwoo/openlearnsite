<%@ Page Language="C#" AutoEventWireup="true" CodeFile="handnum.aspx.cs" Inherits="student_handnum" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta charset="utf-8">
<title>人工智能神经网络—识别手写数字</title>
<link rel="stylesheet" href="../ai/handnum/css/index.css">    
<script type="text/javascript" src="../ai/handnum/js/index.js"></script>
<script type="text/javascript" src="../ai/handnum/js/flexible.js"></script>
<script src='../ai/handnum/js/pixi.js' ></script>
<script src="../ai/handnum/js/tensorflow1.1.js"></script>
<script src="../ai/handnum/js/axis.js"></script>
<script src="../ai/handnum/js/jquery-3.1.1.js"></script>
<script src="../ai/handnum/js/neural.js"></script>
<script src="../code/html2canvas.min.js" type="text/javascript"></script>
<link rel="stylesheet" href="../deepseek/all.min.css">


    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Student/handnum.css" />
</head>

<body>
	<!-- 主DIV -->
	<div class="main">
		<div class="topdiv">
			<div class="topdiv_img">
				人工智能神经网络—识别手写数字
			</div>
		</div>
		<div class="leftdiv">
			<!-- 文字 -->
			<div class="divtitle" style=" width: auto; text-align: left; padding-left: 0.2rem; background-color: rgb(142, 140, 140, 0.5);">
				手写图片识别采用卷积神经网络，使用 65000 张图片进行训练。
			</div>
			<div class="divtitle" style="margin-top: 5px">训练</div>
			<div class="divprogress1">
				<div class="divprogress1_title">
					<span>图片数量：</span> 
					<span id="imageLoading">图片加载中 ...</span>
				</div>
				<div class="divprogress1_bj">
					<div id="progress" style="width: 0%;"></div>
				</div>
				<div id="progressTxt" style="color: white;margin: 10px;opacity: 0.6;">
					训练进度
				</div>
				<div class="divprogress1_btns">
					<!-- <botton class="btn btn1" onclick="readTrainData()"></botton> -->
					<!-- <botton class="btn btn2" onclick="draw100()"></botton> -->
					<!-- <botton class="btn btnsave" onclick="save()"></botton> -->
					<botton class="btn btn3" onclick="train()" title="点击开始训练模型"></botton>
				</div>
			</div>
			
			<div class="datalist">

			</div>	

		</div>
		
		<div class="rightdiv">
			<div id="message" class="divtitle" style=" width: auto; text-align: center;  background-color: rgb(142, 140, 140, 0.6);">
				在黑色区域用鼠标手写数字，点击“识别”按钮
			</div>
			<div class="divtitle">手写输入</div>
			<div class="divsxsr">
				<table border=0>
					<tr>
						<td>
							<div id="hand" class="divsxsr_1"></div>
						</td>
						<td>
							<div class="handnum-tools">
								<button type="button" class="handnum-toolbtn" onclick="doClear()" title="清空画布"><i class="fa fa-eraser" aria-hidden="true"></i><span>清空</span></button>
								<button type="button" class="handnum-toolbtn handnum-toolbtn--secondary" onclick="test()" title="识别手写数字"><i class="fa fa-search" aria-hidden="true"></i><span>识别</span></button>
								<button type="button" class="handnum-toolbtn handnum-toolbtn--neutral" onclick="returnurl()" title="返回学案"><i class="fa fa-reply" aria-hidden="true"></i><span>返回学案</span></button>
							</div>
						</td>
					</tr>
				</table>
				
				
			</div>

			<div class="divtitle">识别结果</div>
			<div id="resultlist" class="resultlist">

				<div  class="resultlist_div">
					<div  class="resultlist_left" style="font-weight: 600;">
					数字
					</div>
					<div class="resultlist_right" style=" text-align: center; font-weight: 600;">
					相似度
					</div>
				</div>

			</div>
		</div>

	</div>
	
    <script type="text/javascript">
        window.__handnumConfig = {
            id: "<%=Id %>",
            fpage: "<%=Fpage %>"
        };
    </script>
    <script type="text/javascript" src="../js/handnum.js"></script>
</body>

<script src="../ai/handnum/js/hand.js"></script>
	

</html>
