<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" EnableViewStateMac="false" CodeFile="mxgraph.aspx.cs" Inherits="Student_mxgraph" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>流程图</title>
	<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
	<meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <link rel="stylesheet" type="text/css" href="../mxgraph/styles/grapheditor.css">
	
	<script type="text/javascript" src="../mxgraph/js/Init.js"></script>
	<script type="text/javascript" src="../mxgraph/deflate/pako.min.js"></script>
	<script type="text/javascript" src="../mxgraph/deflate/base64.js"></script>
	<script type="text/javascript" src="../mxgraph/jscolor/jscolor.js"></script>
	<script type="text/javascript" src="../mxgraph/sanitizer/sanitizer.min.js"></script>
	<script type="text/javascript" src="../mxgraph/mxClient.js"></script>
	<script type="text/javascript" src="../mxgraph/js/EditorUi.js"></script>
	<script type="text/javascript" src="../mxgraph/js/Editor.js"></script>
	<script type="text/javascript" src="../mxgraph/js/Sidebar.js"></script>
	<script type="text/javascript" src="../mxgraph/js/Graph.js"></script>
	<script type="text/javascript" src="../mxgraph/js/Format.js"></script>
	<script type="text/javascript" src="../mxgraph/js/Shapes.js"></script>
	<script type="text/javascript" src="../mxgraph/js/Actions.js"></script>
	<script type="text/javascript" src="../mxgraph/js/Menus.js"></script>
	<script type="text/javascript" src="../mxgraph/js/Toolbar.js"></script>
	<script type="text/javascript" src="../mxgraph/js/Dialogs.js"></script>
    <script src="../code/jquery.min.js" type="text/javascript"></script>
 	

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <link rel="stylesheet" href="../deepseek/all.min.css">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Student/mxgraph.css" />
</head>
<body class="geEditor">
<div class="graph-toolbar">
    <button class="graph-toolbar__btn" onclick="savetoxml()" type="button">
        <i class="fa fa-save graph-toolbar__icon" aria-hidden="true"></i>
        <span>保存流程图</span>
    </button>
    <button onclick="showcontent()" type="button" class="graph-toolbar__btn graph-toolbar__btn--secondary">
        <i class="fa fa-book-open graph-toolbar__icon" aria-hidden="true"></i>
        <span>查看学案</span>
    </button>
    <button onclick="returnurl()" type="button" class="graph-toolbar__btn graph-toolbar__btn--neutral">
        <i class="fa fa-reply" aria-hidden="true"></i>
        <span>返回学案</span>
    </button>
</div>
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
        window.__mxgraphConfig = {
            snum_Id: "<%=Snum+Id %>",
            id: "<%=Id %>",
            codefile: "<%=codefile %>",
            fpage: "<%=Fpage %>"
        };
    </script>
    <script type="text/javascript" src="../js/mxgraph.js"></script>
    </form>

	

</body>
</html>
