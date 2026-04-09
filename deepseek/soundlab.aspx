<%@ Page Language="C#" AutoEventWireup="true" CodeFile="soundlab.aspx.cs" Inherits="deepseek_soundlab" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>

<head  runat="server">
    <title>🎙️ SoundLab 在线声音分析</title>
    
    
    <script src="../code/jquery.min.js" type="text/javascript"></script>
    <script src="../code/html2canvas.min.js" type="text/javascript"></script>
	<link rel="stylesheet" href="all.min.css">
	

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Teacher/soundlab.css" />
</head>
<body>
    <div class="container">
        <h1 title="声音的三要素：音高、音色、响度">🎙️ ️SoundLab 在线声音分析</h1>
        <div class="dashboard">
            <div class="card">
                <h3>📈 实时波形</h3>
                <canvas id="waveform" width="800" height="150" title="波形图：振幅为响度"></canvas>
            </div>

            <div class="card">
                <h3>🌈 频谱分析</h3>
                <canvas id="spectrum" width="800" height="150" title="频谱图：振幅为频率"></canvas>
            </div>

            <!-- 新增 MFCC 特征显示区域 -->
            <div class="card">
                <h3>🎵 MFCC 特征</h3>
                <canvas id="mfcc" width="800" height="150" title="倒谱图：音色"></canvas>
            </div>
        </div>
        <div class="controls">
	            <div class="sound-toolbar">
	            <button id="startBtn" class="sound-toolbar__btn" type="button"><i class="fa fa-microphone" aria-hidden="true"></i><span>开始录音</span></button>
	            <button id="stopBtn" disabled class="sound-toolbar__btn sound-toolbar__btn--secondary" type="button"><i class="fa fa-stop" aria-hidden="true"></i><span>停止录音</span></button>
	            <button  type = "button" onclick="savechat()" title="保存作品到服务器" class="sound-toolbar__btn sound-toolbar__btn--secondary">
			<i class="fa fa-save" aria-hidden="true"></i> 保存作品</button>
			<button onclick="returnurl()" title="返回到学案页面" class="sound-toolbar__btn sound-toolbar__btn--neutral" type="button">
			<i class="fa fa-reply" aria-hidden="true"></i> 返回学案</button>  
	            </div>

        </div>
        <div id="status">准备就绪</div>
        <audio id="audioPlayer" controls hidden></audio>

		<div class="recordhistory" id="recordhistory" >
			<div class="recordings-list">
				<h3>📁 录音列表</h3>
				<div id="recordings"></div>
			</div>
			<!-- 新增比较功能区域 -->
			<div class="compare-section">
				<h3>🔊 声音相似度对比</h3>
				<div class="compare-controls">
					<select id="recording1">
						<option value="">选择第一个录音</option>
					</select>
					<select id="recording2">
						<option value="">选择第二个录音</option>
					</select>
					<button onclick="compareRecordings()" class="sound-toolbar__btn" type="button">开始比较</button>
				</div>
				<div id="result" class="result-box"></div>
			</div>
		</div>
    </div>

    
    <script type="text/javascript">
        window.__soundlabConfig = {
            id: "<%=Id %>",
            fpage: "<%=Fpage %>"
        };
    </script>
    <script type="text/javascript" src="../js/soundlab.js"></script>
</body>

</html>
