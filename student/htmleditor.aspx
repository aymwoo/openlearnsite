<%@ Page Language="C#" AutoEventWireup="true" CodeFile="htmleditor.aspx.cs" Inherits="student_htmleditor" ResponseEncoding="utf-8" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head >  
  <meta charset="UTF-8">
  
<link href="../code/css/font-awesome.min.css" rel="stylesheet" type="text/css" />
<script src="../code/jquery.min.js" type="text/javascript"></script>
<script src="../code/build/src/ace.js" type="text/javascript"></script>
<script src="../code/build/src/ext-language_tools.js" type="text/javascript"></script>
<link href="../js/tinybox.css" rel="stylesheet" type="text/css" />
<script src="../js/tinybox.js" type="text/javascript"></script>
    <script src="../code/html2canvas.min.js" type="text/javascript"></script>
	<link rel="stylesheet" href="../deepseek/all.min.css">
	

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Student/htmleditor.css" />
</head>
<body>
    <div>
    <div class="html_banner">
	    <span class="icon">网页</span>	 
        <span class="spl"></span>
		<input type="text" id="html_page" name="pagename" readonly title="网页文件名称" class="px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300">
    </div>
<div id="tooltip">
	<span class="keyword" title="样式">style</span>
	<span class="keyword" title="居中">center</span>
	<span class="keyword" title="一级标题">h1</span>
	<span class="keyword" title="超链接">a</span>	
	<span class="keyword" title="段落">p</span>
	<span class="keyword" title="层">div</span>
	<span class="keyword" title="图片">img</span>
	<span class="keyword" title="视频">video</span>
	<span class="keyword" title="音频">audio</span>
	<span class="keyword" title="表单">form</span>
	<span class="keyword" title="换行">br</span>
	<span class="keyword" title="文本框">input</span>
	<span class="keyword" title="用户名 文本框">username</span>
	<span class="keyword" title="内容 文本框">content</span>
	<span class="keyword" title="提交按钮">submit</span>
</div>	
<div id="tool">
	<i class="fa fa-plus" aria-hidden="true"  onclick="fontbig()" title="放大代码"></i>
	<i class="fa fa-minus" aria-hidden="true" onclick="fontsmall()" title="缩小代码"></i>
	<i class="fa fa-undo" aria-hidden="true"  onclick="backward()" title="撤销"></i>
	<i class="fa fa-rotate-right" aria-hidden="true" onclick="forward()" title="重做"></i>
</div>
	
        <div id="main">
        <div id="left"></div>
        <div id="resize" title="左右拖动"></div>
        <div id="right">
			<iframe id="preview-frame" ></iframe>
		</div>
        </div>
<div  id="sidebyleft">
</div>

<div id="sideby" class="html-toolbar">
<button onclick="example()" class="html-toolbar__btn" title="网页模板" type="button">
<i class="fa fa-file-code-o" aria-hidden="true"></i><span>网页模板</span></button>
<button onclick="showMission()" class="html-toolbar__btn" title="查看学案" type="button">
<i class="fa fa-book" aria-hidden="true"></i><span>查看学案</span></button>
<button onclick="showShare()" class="html-toolbar__btn" title="网页空间" type="button">
<i class="fa fa-hdd-o" aria-hidden="true"></i><span>网页空间</span></button>
<button type="button" onclick="savehtml()" class="html-toolbar__btn html-toolbar__btn--secondary buttonsave" title="保存作品到服务器" >
<i class="fa fa-save" aria-hidden="true"></i><span>保存作品</span></button>
<button onclick="returnurl()" class="html-toolbar__btn html-toolbar__btn--neutral" title="返回学案页面" type="button">
<i class="fa fa-reply" aria-hidden="true"></i><span>返回学案</span></button>
</div>

   <div id="mcontext" style="display: none; background: #D1D1D1; overflow-y: auto; overflow-x: hidden;
        position: absolute;   height: 420px; z-index: 888;  bottom: 0px; right:0px;opacity:99%; ">
        <div style="margin:6px; ">
        <%=Mcontents %><br />
        </div>
    </div>

	

    
    
	
    </div>
    <script type="text/javascript">
        window.__htmleditorConfig = {
            snum: "<%=Snum %>",
            id: "<%=Id %>",
            codefile: "<%=codefile %>",
            fpage: "<%=Fpage %>",
            mypage: "<%=Mypage %>"
        };
    </script>
    <script type="text/javascript" src="../js/htmleditor.js"></script>
</body>
</html>
