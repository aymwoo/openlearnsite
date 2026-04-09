<%@ Page Language="C#" AutoEventWireup="true" CodeFile="mqtt.aspx.cs" Inherits="student_mqtt" ResponseEncoding="utf-8" %>
<!DOCTYPE html>
<html >
<head runat="server">
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>物联网MQTT服务</title>
	<script src="../code/mqtt/mqtt.min.js" ></script>
    <script src="../code/jquery.min.js"></script>
    <link href="../code/mqtt/mqtt.css" rel="stylesheet" type="text/css" />
	<script src="../code/chart.js"></script>
	<link rel="stylesheet" href="../deepseek/all.min.css">
	

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Student/mqtt.css" />
</head>

<body>
	<div class="banner"></div>
	<div class="mqttset">	
	<table>
		<tr>
			<td>
		<img id="broker" src="../code/mqtt/ready.png" title="Mqtt状态" />
		MQTT地址：<input id ="txtIp" type="text"  readonly class="px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300" />
		MQTT端口：<input id ="txtPort" type="text" readonly class="px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300" />			
			</td>
			<td>
		客户端ID：<input id ="txtId" type="text" readonly class="px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300" />
		用户：<input id ="txtUser" type="text"  readonly class="px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300" />
		密码：<input id ="txtPwd" type="text"  readonly class="px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300" />
			
			</td>
			<td>
		<div class="mqtt-toolbar">
		<button id="btnConnect" class="mqtt-toolbar__btn"><i class="fa fa-plug" aria-hidden="true"></i><span>点击连接</span></button>
		<button id="savebtn" onclick="savework();" class="mqtt-toolbar__btn mqtt-toolbar__btn--secondary" type="button"><i class="fa fa-save" aria-hidden="true"></i><span>保存实验</span></button>
		<button id="returnbtn" onclick="returnurl();" class="mqtt-toolbar__btn mqtt-toolbar__btn--neutral" type="button"><i class="fa fa-reply" aria-hidden="true"></i><span>返回学案</span></button>
		</div>			
			</td>
		</tr>
		<tr>
			<td>
				<div class="messages"></div>					
			</td>
			<td>
				<div class="center">	
				
				<img id="led" class="device " src="../code/mqtt/led.png" title="灯光控制 led" />
				<img id="fan" class="device " src="../code/mqtt/fan.png" title="风扇控制 fan" />
				<img id="pump" class="device " src="../code/mqtt/pump.png" title="水泵控制 pump" />
				
				<br>
				主题：<input id ="txtTopic" type="text" readonly class="px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300" /><br><br>
				消息：<input id="txtPayload" type="text" value="off" readonly class="px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300" /><br><br>
				<div >				
				<div class="mqtt-toolbar">
				<button id="btnPublish" disabled="true" class="mqtt-toolbar__btn" type="button"><i class="fa fa-paper-plane" aria-hidden="true"></i><span>发布消息</span></button>
				<button id="btnSub" disabled="true" class="mqtt-toolbar__btn mqtt-toolbar__btn--secondary" type="button"><i class="fa fa-rss" aria-hidden="true"></i><span>订阅主题</span></button><br>		
				</div>
				</div>
				<div class="sesor">
				<table>
				<tr >
					<td><img id="distance" class="measure" src="../code/mqtt/distance.png" title="距离 dist" />	
					</td>
					<td><img id="temperature" class="measure" src="../code/mqtt/temperature.png" title="温度 temp" />	
					</td>
					<td><img id="humidity" class="measure" src="../code/mqtt/humidity.png" title="湿度 humi" />	
					</td>
					<td><img id="light" class="measure" src="../code/mqtt/light.png" title="亮度 light" />
					</td>
					<td><img id="sound" class="measure" src="../code/mqtt/mic.png" title="声音 sound" />	
					</td>
				</tr>
				<tr>
					<td><div id="distancenum" class="data" >0</div>	
					</td>
					<td><div id="temperaturenum" class="data" >0</div>	
					</td>
					<td><div id="humiditynum" class="data" >0</div>	
					</td>
					<td><div id="lightnum" class="data" >0</div>	
					</td>
					<td><div id="soundnum" class="data" >0</div>
					</td>
				</tr>
				</table>					
				</div>
				<div>
					<br>	
				</div>	
				</div>
			</td>
			<td>
				<div class="linkuser"></div>
				<div class="divleft" >
				已订阅主题:
					<select class="subtopics">
					</select>
					&nbsp;
				<button id="btnUnSub" disabled="true" class="mqtt-toolbar__btn mqtt-toolbar__btn--neutral" type="button"><i class="fa fa-ban" aria-hidden="true"></i><span>取消订阅</span></button>
			</div>
		</td>
		</tr>
		
	</table>
	<div>
	  <canvas id="myChart" height="60"></canvas>
	  <div>
	  <span id="msg" style="float:right;"></span>
	  <img class="volume" src="../code/mqtt/volume.png" title="声音警报" />	
	  <input class ="alert px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300" type="text"  value="16000"/>
	  </div>
	  <audio id="audio" controls="controls"  hidden="true" ></audio>
	</div>
	
	</div>
    <script type="text/javascript">
        window.__mqttConfig = {
            id: "<%=Id %>",
            snum: "<%=Snum %>",
            workDevice: "<%=workDevice %>",
            serverIp: "<%=serverIp %>",
            fpage: "<%=Fpage %>"
        };
    </script>
    <script type="text/javascript" src="../js/mqtt.js"></script>
</body>


</html>
