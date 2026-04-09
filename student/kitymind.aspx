<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" EnableViewStateMac="false"  CodeFile="kitymind.aspx.cs" Inherits="student_kitymind" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    	<meta charset=utf-8>
	<!-- bower:css -->
	<link rel="stylesheet" href="../../plugins/km/bower_components/bootstrap/dist/css/bootstrap.css" />
	<link rel="stylesheet" href="../../plugins/km/bower_components/codemirror/lib/codemirror.css" />
	<link rel="stylesheet" href="../../plugins/km/bower_components/hotbox/hotbox.css" />
	<link rel="stylesheet" href="../../plugins/km/bower_components/kityminder-core/dist/kityminder.core.css" />
	<link rel="stylesheet" href="../../plugins/km/bower_components/color-picker/dist/color-picker.min.css" />
	<!-- endbower -->

	<link rel="stylesheet" href="../../plugins/km/kityminder.editor.css">
		

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Student/kitymind.css" />
</head>

<body ng-app="kityminderDemo" >
    <form id="form1" runat="server">
       <div id="mcontext" style="display: none; background: #fffdea; overflow-y: auto; overflow-x: hidden;
            position: absolute;  width: 500px; height: 50%; z-index: 999;opacity:0.9; font-size: 16px;
            right: 0px; bottom: 0px; padding: 2px;">
            <div style="margin:10px; ">
            <h4><%=Titles%></h4>
            <%=Mcontents %>
            </div>
        </div>
    <script type="text/javascript">
        window.__kitymindConfig = {
            snum: "<%= Snum %>",
            lsSname: "<%= LsSname %>",
            lsSgrade: "<%= LsSgrade %>",
            lsSclass: "<%= LsSclass %>",
            lsSid: "<%= LsSid %>",
            lsCid: "<%= LsCid %>",
            lsLid: "<%= LsLid %>",
            lsLtitle: "<%= LsLtitle %>",
            lsLtype: "<%= LsLtype %>",
            codefile: "<%=codefile %>",
            fpage: "<%=Fpage %>",
            id: "<%=Id %>"
        };
    </script>
    <script type="text/javascript" src="../js/kitymind.js"></script>
    </form>
<div class="fixed top-3 right-6 z-[9999] flex items-center gap-3">
    <a href="#" onclick="return downfile(this);" class="px-5 py-2 bg-gradient-to-r from-emerald-500 to-teal-500 text-white text-sm font-bold rounded-lg shadow-md hover:from-emerald-600 hover:to-teal-600 hover:shadow-lg transition-all duration-300 flex items-center gap-2 border border-emerald-400">
        <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M8 7H5a2 2 0 00-2 2v9a2 2 0 002 2h14a2 2 0 002-2V9a2 2 0 00-2-2h-3m-1 4l-3 3m0 0l-3-3m3 3V4"></path></svg>
        保存作品
    </a>
    <a href="#" onclick="returnurl();" class="px-5 py-2 bg-gradient-to-r from-slate-600 to-slate-700 text-white text-sm font-bold rounded-lg shadow-md hover:from-slate-700 hover:to-slate-800 hover:shadow-lg transition-all duration-300 flex items-center gap-2 border border-slate-500">
        <svg class="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 19l-7-7m0 0l7-7m-7 7h18"></path></svg>
        返回
    </a>
</div>
<kityminder-editor on-init="initEditor(editor, minder)" data-theme="fresh-green"></kityminder-editor>

<!-- bower:js -->
<script src="../../plugins/km/bower_components/jquery/dist/jquery.js"></script>
<script src="../../plugins/km/bower_components/bootstrap/dist/js/bootstrap.js"></script>
<script src="../../plugins/km/bower_components/angular/angular.js"></script>
<script src="../../plugins/km/bower_components/angular-bootstrap/ui-bootstrap-tpls.js"></script>
<script src="../../plugins/km/bower_components/codemirror/lib/codemirror.js"></script>
<script src="../../plugins/km/bower_components/codemirror/mode/xml/xml.js"></script>
<script src="../../plugins/km/bower_components/codemirror/mode/javascript/javascript.js"></script>
<script src="../../plugins/km/bower_components/codemirror/mode/css/css.js"></script>
<script src="../../plugins/km/bower_components/codemirror/mode/htmlmixed/htmlmixed.js"></script>
<script src="../../plugins/km/bower_components/codemirror/mode/markdown/markdown.js"></script>
<script src="../../plugins/km/bower_components/codemirror/addon/mode/overlay.js"></script>
<script src="../../plugins/km/bower_components/codemirror/mode/gfm/gfm.js"></script>
<script src="../../plugins/km/bower_components/angular-ui-codemirror/ui-codemirror.js"></script>
<script src="../../plugins/km/bower_components/marked/lib/marked.js"></script>
<script src="../../plugins/km/bower_components/kity/dist/kity.min.js"></script>
<script src="../../plugins/km/bower_components/hotbox/hotbox.js"></script>
<script src="../../plugins/km/bower_components/json-diff/json-diff.js"></script>
<script src="../../plugins/km/bower_components/kityminder-core/dist/kityminder.core.min.js"></script>
<script src="../../plugins/km/bower_components/color-picker/dist/color-picker.min.js"></script>
<!-- endbower -->

<script src="../../plugins/km/kityminder.editor.js"></script>


<script src="../js/learnstatus.js" type="text/javascript"></script>


</body>
</html>
