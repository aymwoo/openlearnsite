<%@ Page Language="C#" AutoEventWireup="true" CodeFile="programing.aspx.cs" Inherits="Student_programing" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta charset="utf-8" />
<title></title>
    <script src="../js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="../Statics/swfobject.js" type="text/javascript"></script>
    <link rel="stylesheet" href="../deepseek/all.min.css">
    <script type="text/javascript">
        window.onbeforeunload = function () { return "是否离开当前活动页面？请先保存作品。"; }
        var fwidth = "100%";
        var fheight = "100%";
        // （scratch目录不能为根目录，否则读不出）

        var Scratch = Scratch || {};
        Scratch.FlashApp = Scratch.FlashApp || {};
        var editorId = "scratch";

        function handleEmbedStatus(e) {
            var scratch = $(document.getElementById(editorId));
            Scratch.FlashApp.ASobj = scratch[0];
            Scratch.FlashApp.$ASobj = $(Scratch.FlashApp.ASobj);
        }

        // enables the SWF to log errors
        function JSthrowError(e) {
            if (window.onerror) window.onerror(e, 'swf', 0);
            else console.error(e);
        }

        function JSeditorReady() {
            return true;
        }

        function handleParameters() { }

        var flashvars = {
            canDown:true,     //设定菜单显示或隐藏下载项目功能
            extensionDevMode: true,
            microworldMode: false,
            viewMode: false,
            project: '<%=Filename %>',
            autostart: 'false',
            pm: '<%=Id %>',
            projectTitle: '<%=Titles %>',
            projectOwner: '<%=Owner %>'
        };
        var params = {
            menu: "false",
            scale: "noScale",
            allowFullscreen: "true",
            allowScriptAccess: "always",
            bgcolor: "",
            wmode: "transparent" //direct can cause issues with FP settings & webcam
        };

        var swfFile = "../Statics/MinMake.swf";
        swfobject.embedSWF(swfFile,"scratch", fwidth, fheight, "10.0.0","../Statics/expressInstall.swf",flashvars, params, null, handleEmbedStatus);
    </script>
    <style type="text/css">
        html, body { height:100%; overflow:hidden;}
        body{margin: 0;}
		.scratch-toolbar { display:inline-flex; flex-wrap:wrap; gap:10px; align-items:center; }
		.scratch-toolbar__btn { display:inline-flex; align-items:center; justify-content:center; gap:8px; min-width:112px; height:40px; padding:0 16px; border:0; border-radius:12px; background:linear-gradient(135deg,#2563eb 0%,#1d4ed8 100%); color:#fff; font-size:14px; font-weight:700; white-space:nowrap; box-shadow:0 14px 28px -18px rgba(37,99,235,.82); cursor:pointer; transition:transform .2s ease, box-shadow .2s ease, filter .2s ease; }
		.scratch-toolbar__btn:hover { transform:translateY(-1px); box-shadow:0 18px 30px -18px rgba(37,99,235,.9); filter:brightness(1.03); }
		.scratch-toolbar__btn--secondary { background:linear-gradient(135deg,#0f766e 0%,#0f766e 100%); box-shadow:0 14px 28px -18px rgba(15,118,110,.78); }
		.scratch-toolbar__btn--neutral { background:linear-gradient(135deg,#475569 0%,#334155 100%); box-shadow:0 14px 28px -18px rgba(51,65,85,.72); }
    </style>

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
</head>
<body>
    <div style="text-align: right; position: absolute; right: 50px; top: 3px; font-size: 11pt;
        z-index: 2;">
        <div id="barbtn" class="scratch-toolbar">
        <span id="uploading" style="color:#fff; font-weight:bold;"></span>
            <img id="bill" src="../images/bill.png" alt="学习单" />
            <button id="savebtn" class="scratch-toolbar__btn scratch-toolbar__btn--secondary" type="button"><i class="fa fa-save" aria-hidden="true"></i><span>保存作品</span></button>
            <button id="returnbtn" class="scratch-toolbar__btn scratch-toolbar__btn--neutral" type="button"><i class="fa fa-reply" aria-hidden="true"></i><span>返回学案</span></button>
        </div>
    </div>
    <div id="scratch">
    <object classid="clsid27CDB6E-AE6D-11cf-96B8-444553540000">
     <embed width="360" height="360" type="application/x-shockwave-flash"></embed>
    </object>
    </div>
    <div id="mcontext" style="display: none; background: #fff; overflow-y: auto; overflow-x: hidden;
        position: absolute; width: 500px; max-height: 710px; min-height: 220px; z-index: 999;
        left: 0px; top: 0px; padding: 2px;">
        <div style="height: 16px; text-align: right; right: 50px;">
            <img id="zoom" src="../images/zoom.gif" alt="放大缩小" /></div>
        <%=Mcontents %>
    </div>
    <script type="text/javascript">
        function hidebutton() {
            $('#barbtn').hide(); //隐藏    
        }
        function showbutton() {
            $('#barbtn').show(); //显示    
        }
        function showsave() {
            $('#savebtn').show(); //显示立即保存按钮
        }
        function hidesave() {
            $('#savebtn').hide(); //隐藏立即保存按钮
        }
        function showreturn() {
            $('#uploading').html("");
            $('#returnbtn').show(); //显示返回按钮
        }
        $("#mcontext").dblclick(function () {
            $("#mcontext").slideToggle();
        });
        $("#bill").click(function () {
            $("#mcontext").slideToggle();
        });
        $("#savebtn").click(function () {
            var obj = swfobject.getObjectById('scratch');
            obj.SaveNow();
            $('#savebtn').hide(); //显示立即保存按钮
            $('#returnbtn').hide(); //隐藏返回按钮
            $('#uploading').html("正在上传作品中……，请稍等！");
        });
        $("#returnbtn").click(function () {
            self.location = '<%=Fpage %>';
        });
        $("#zoom").toggle(function () {
            $("#mcontext").width(810);
        }, function () {
            $("#mcontext").width(500);
        });
    </script>
        <script src="../Statics/extensions/scratch_ext.js" type="text/javascript"></script>
        <script src="../Statics/extensions/scratch_plugin.js" type="text/javascript"></script>
        <script src="../Statics/extensions/scratch_nmh.js" type="text/javascript"></script>
        <script src="../Statics/extensions/scratch_proxies.js" type="text/javascript"></script>
</body>
</html>
