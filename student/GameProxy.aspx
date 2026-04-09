<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GameProxy.aspx.cs" Inherits="student_GameProxy" %>

<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>游戏加载中...</title>
    <style>
        * { margin: 0; padding: 0; }
        html, body { width: 100%; height: 100%; overflow: hidden; }
        #gameFrame { width: 100%; height: 100%; border: none; }
        #loading {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            font-family: Arial, sans-serif;
            font-size: 18px;
            color: #666;
            text-align: center;
        }
        #loading .spinner {
            width: 40px;
            height: 40px;
            margin: 0 auto 20px;
            border: 4px solid #f3f3f3;
            border-top: 4px solid #3498db;
            border-radius: 50%;
            animation: spin 1s linear infinite;
        }
        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }
        #error {
            display: none;
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            text-align: center;
            font-family: Arial, sans-serif;
        }
        #error h2 { color: #e74c3c; margin-bottom: 20px; }
        #error p { color: #666; margin-bottom: 10px; }
        #error a { color: #3498db; text-decoration: none; }
    </style>
</head>
<body>
    <div id="loading">
        <div class="spinner"></div>
        <div>正在加载游戏，请稍候...</div>
    </div>
    <div id="error">
        <h2 id="errorTitle">访问受限</h2>
        <p id="errorMsg"></p>
        <p><a href="default.aspx">返回首页</a></p>
    </div>
    <iframe id="gameFrame" style="display:none;"></iframe>
    
    <script type="text/javascript">
        document.addEventListener('contextmenu', function(e) {
            e.preventDefault();
            return false;
        });
        
        document.addEventListener('keydown', function(e) {
            if (e.ctrlKey && (e.key === 'u' || e.key === 'U')) {
                e.preventDefault();
                return false;
            }
            if (e.key === 'F12') {
                e.preventDefault();
                return false;
            }
            if (e.ctrlKey && e.shiftKey && (e.key === 'I' || e.key === 'i')) {
                e.preventDefault();
                return false;
            }
            if (e.ctrlKey && e.shiftKey && (e.key === 'J' || e.key === 'j')) {
                e.preventDefault();
                return false;
            }
            if (e.ctrlKey && e.shiftKey && (e.key === 'C' || e.key === 'c')) {
                e.preventDefault();
                return false;
            }
        });
        
        function showError(title, msg) {
            document.getElementById('loading').style.display = 'none';
            document.getElementById('error').style.display = 'block';
            document.getElementById('errorTitle').innerText = title;
            document.getElementById('errorMsg').innerText = msg;
        }
        
        function loadGame(url) {
            document.getElementById('loading').style.display = 'none';
            var frame = document.getElementById('gameFrame');
            frame.style.display = 'block';
            frame.src = url;
        }
    </script>
    <asp:Literal ID="litScript" runat="server"></asp:Literal>
</body>
</html>
