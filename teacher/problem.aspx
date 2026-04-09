<%@ Page Title="" Language="C#"  Validaterequest="false"  AutoEventWireup="true" CodeFile="problem.aspx.cs" Inherits="Teacher_problem" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
        <meta charset="utf-8" />
<link rel="stylesheet" type="text/css" href="../App_Themes/Teacher/StyleSheet.css" />
    <link href="../js/vendors/wangeditor/style.css" rel="stylesheet" />
    <link rel="stylesheet" href="../js/vendors/vditor/index.css" />
    <script src="../js/MenuCookie.js" type="text/javascript"></script>
    <script src="../js/jquery-1.8.2.min.js" type="text/javascript"></script>
    

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Teacher/problem.css" />
</head>
<body >
       <form id="form1" runat="server" > 
       <div  >
        <div  class="mainhead" onclick="ShowMenu()">  
            <asp:Image ID="Imagelogo" runat="server" ImageUrl="~/images/learnsite.gif"  ToolTip = "信息技术学习平台 LearnSite &#13;Powered By Asp.net2.0+Sql2005Express &#13;温州水乡设计编写" Height="24px" />
        </div>
        <div class="mainarea">
        <div  id="MenuDiv"  class="mainleft" >
            <div id="navig">
            <ul id="navigul">
            <li class="navmenuhead"></li>
            <li class="navigli"><a href="../teacher/start.aspx">上课</a></li>
            <li class="navigli"><a href="../teacher/course.aspx">备课</a></li>
            <li class="navigli"><a href="../teacher/gauge.aspx">量规</a></li>
            <li class="navigli"><a href="../teacher/works.aspx">作品</a></li>
            <li class="navigli"><a href="../teacher/signin.aspx">签到</a></li>
            <li class="navigli"><a href="../teacher/teachermanage.aspx">管理</a></li>
            <li class="navigli"><a href="../quiz/quiz.aspx">测验</a></li>
            <li class="navigli"><a href="../teacher/typer.aspx">打字</a></li>
            <li class="navigli"><a href="../teacher/soft.aspx">资源</a></li>
            <li class="navigli"><a href="../teacher/infomation.aspx">信息</a></li>
            <li class="navigli"><a href="../teacher/systeminfo.aspx">状态</a></li>
            <li class="navigli"><a href="../teacher/helper.aspx">帮助</a></li>
            <li class="navmenu">            
            <div onclick="HideMenu()">
                <asp:Label ID="LabelVer" runat="server" Font-Size="8pt"></asp:Label>
            </div>
            </li>
            </ul>
            </div>
        </div>  
        <div class="mainright">
        <div  class="mainrighttop"></div>
        <div class="mainrightcontent">
    <div style="margin: 10px;  ">
        <div style="margin: auto; text-align: left; min-width:800px;">
            <b>试题内容：&nbsp; </b><asp:DropDownList ID="ddscore" runat="server" Height="16px" 
                Width="40px">
                <asp:ListItem Value="1">1</asp:ListItem>
                <asp:ListItem Selected="True">2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
            </asp:DropDownList>分<br />
<div class="problem-editor-switch">
    <span style="font-size:13px;font-weight:700;color:#334155;">编辑器：</span>
    <select id="editorSelector" onchange="switchProblemEditor(this.value)">
        <option value="kindeditor" selected>KindEditor</option>
        <option value="wangeditor">WangEditor</option>
        <option value="vditor">Vditor</option>
    </select>
</div>
<div id="problem-wangeditor-wrap" style="display:none; position:relative; border:1px solid #ccc; z-index:100; margin-bottom:10px;">
    <div id="problem-wangeditor-toolbar" style="border-bottom:1px solid #ccc;"></div>
    <div id="problem-wangeditor-text" style="height:220px;"></div>
</div>
<div id="problem-vditor-wrap" style="display:none; position:relative; margin-bottom:10px;">
    <div id="problem-vditor-container"></div>
</div>
&nbsp;<textarea id ="mcontent" runat ="server" ClientIDMode="Static" name="textareaWord" style="width: 980px; height:120px;" ></textarea>
        </div>

<div id="editor"></div>
<div id="result">
<pre id="output" > </pre>
</div>
<div style="margin: auto; ">
    <br />
    &nbsp;&nbsp;
    <asp:Button ID="Btnadd" runat="server" OnClick="Btnadd_Click" OnClientClick="return syncProblemContent();"
        Text="添加题目"  CssClass="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" />
    &nbsp; &nbsp;<asp:Button ID="Btnreturn" runat="server" OnClick="Btnreturn_Click"
        Text="返回测评"   CssClass="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" />
    <br />
    <br />
</div>     
    <asp:HiddenField ID="code" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="print" runat="server" ClientIDMode="Static" />
<div id="centerbar">
<button  onclick="runit()" type="button"  class="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0"> 
<i class="fa fa-play" aria-hidden="true"></i>运行
</button>
&nbsp; &nbsp;
<button  onclick="clearit()" type="button"  class="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0"> 
<i class="fa fa-play" aria-hidden="true"></i>清空
</button>
</div>
    </div>
  <!-- 主要文件 -->
  <script src="../code/build/src/ace.js" type="text/javascript"></script>
  <!-- 用来提供代码提示和自动补全的插件 -->
  <script src="../code/build/src/ext-language_tools.js" type="text/javascript"></script>
  <script src="../code/build/src/ext-beautify.js" type="text/javascript"></script>
  


<script src="../code/skulpt.min.js" type="text/javascript"></script>
<script src="../code/skulpt-stdlib.js" type="text/javascript"></script>
<script src="../code/html2canvas.min.js" type="text/javascript"></script>
<script src="../code/jquery.min.js" type="text/javascript"></script>


<script charset="utf-8" src="../kindeditor/kindeditor-min.js" type="text/javascript"></script>
<script charset="utf-8" src="../kindeditor/lang/zh_CN.js" type="text/javascript"></script>
<script src="../js/vendors/vditor/index.min.js"></script>
<script src="../js/vendors/wangeditor/index.js"></script>
<script src="../teacher/editor-upload-helper.js" type="text/javascript"></script>
 
        </div>         
        </div>   
        </div>
    </div>    
    <script type="text/javascript">
        window.__problemConfig = {
            myCid: "<%=myCid() %>"
        };
    </script>
    <script type="text/javascript" src="../js/problem.js"></script>
    </form>
</body>
</html>
