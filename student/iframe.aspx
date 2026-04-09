<%@ Page Language="C#" AutoEventWireup="true" CodeFile="iframe.aspx.cs" Inherits="student_iframe" ResponseEncoding="utf-8" %>

<html lang="zh-CN">
<head id="Head1" runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>嵌入网页活动</title>
    <link rel="stylesheet" href="../deepseek/all.min.css">
    <script src="../code/jquery.min.js" type="text/javascript"></script>
    <script src="../Plupload/plupload.full.min.js" type="text/javascript"></script>
	

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Student/iframe.css" />
</head>
<body>
    <div class="iframe-page">
        <section class="iframe-hero">
            <div class="iframe-hero-body">
                <div class="iframe-eyebrow"><i class="fa fa-window-maximize" aria-hidden="true"></i> 嵌入网页活动</div>
                <h1 class="iframe-title">外部工具学习区</h1>
                <p class="iframe-subtitle">先阅读活动说明，再在下方嵌入页面中完成操作。提交作品后可返回学案继续学习。</p>
                <div class="iframe-toolbar">
                    <button onclick="homeland()" class="iframe-btn"><i class="fa fa-home" aria-hidden="true"></i> 首页</button>
                    <button onclick="backward()" class="iframe-btn"><i class="fa fa-arrow-left" aria-hidden="true"></i> 后退</button>
                    <button onclick="forward()" class="iframe-btn"><i class="fa fa-arrow-right" aria-hidden="true"></i> 前进</button>
                    <button id="savebtn" type="button" class="iframe-btn iframe-btn-primary" title="将PSD格式图片上传到服务器上"><i class="fa fa-upload" aria-hidden="true"></i> 提交作品</button>
                    <button onclick="returnurl()" class="iframe-btn iframe-btn-neutral"><i class="fa fa-reply" aria-hidden="true"></i> 返回学案</button>
                </div>
            </div>
        </section>

        <div class="iframe-layout">
            <section class="iframe-panel">
                <div class="iframe-panel-head">
                    <h2 class="iframe-panel-title">活动说明</h2>
                    <p class="iframe-panel-desc">以下内容由教师在活动编辑页维护，帮助你明确本次任务要求、完成标准和操作注意事项。</p>
                </div>
                <div id="Mcontents" class="iframe-mission"><%= HttpUtility.HtmlDecode(Mcontents) %></div>
            </section>

            <section class="iframe-panel">
                <div class="iframe-panel-head">
                    <h2 class="iframe-panel-title">嵌入网页</h2>
                    <p class="iframe-panel-desc">如果目标网页没有自动加载，可使用“首页”重新打开。完成操作后记得提交作品并返回学案。</p>
                </div>
                <div class="iframe-frame-wrap">
                    <iframe id="homeframe" title="嵌入网页操作区" sandbox="allow-scripts allow-same-origin allow-forms allow-popups allow-downloads">
                      您的浏览器不支持 iframe。
                    </iframe>
                </div>
            </section>
        </div>
    </div>
    
    <script type="text/javascript">
        window.__iframeConfig = {
            id: "<%=Id %>",
            fpage: "<%=Fpage %>",
            mexample: "<%=Mexample %>",
            lid: "<%=Lid %>",
            ext: "<%=Ext %>"
        };
    </script>
    <script type="text/javascript" src="../js/iframe.js"></script>
</body>
</html>
