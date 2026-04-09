<%@ Page Language="C#" AutoEventWireup="true" CodeFile="consolepreview.aspx.cs" Inherits="Teacher_consolepreview" ResponseEncoding="utf-8" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Python交互解释器WEB-IDLE</title>
  <link href="../js/toolbar-buttons.css" rel="stylesheet" type="text/css" />
  <style type="text/css">
      body.console-preview-page {
          margin: 0;
          min-height: 100vh;
          background: linear-gradient(180deg, #0f172a 0%, #1e293b 100%);
          color: #e2e8f0;
          font-family: Arial, "Microsoft YaHei", sans-serif;
      }

      .console-preview-shell {
          padding: 1.25rem;
      }

      .console-preview-hero {
          margin-bottom: 1rem;
          padding: 1.25rem 1.5rem;
          border-radius: 1.25rem;
          background: linear-gradient(135deg, rgba(37, 99, 235, 0.35), rgba(99, 102, 241, 0.22));
          border: 1px solid rgba(148, 163, 184, 0.2);
          box-shadow: 0 18px 36px -28px rgba(15, 23, 42, 0.75);
      }

      .console-preview-title {
          margin: 0;
          font-size: 1.5rem;
          font-weight: 700;
      }

      .console-preview-desc {
          margin: 0.5rem 0 0;
          color: #cbd5e1;
          line-height: 1.7;
      }

      .console-preview-toolbar {
          margin-top: 1rem;
      }
  </style>
</head>

<script src="../code/skulpt.min.js" type="text/javascript"></script>
<script src="../code/skulpt-stdlib.js" type="text/javascript"></script>
<script src="../code/build/src/ace.js" type="text/javascript"></script>
<script src="../code/build/src/ext-language_tools.js" type="text/javascript"></script>
<link rel="stylesheet" type="text/css" href="../code/ipython.css"/>
<script type="text/javascript">
    var ide;
    window.addEventListener("load", function (event) {
        var editor = document.getElementById("editor");
        ide = new ipythonExample(editor);
    })
    //<div class="container ace-gruvbox ace_editor"> 会增加一个文本框
    //ace.js 修改12px/normal为28px
</script>

<body class="console-preview-page">
<div class="console-preview-shell">
    <div class="console-preview-hero">
        <h1 class="console-preview-title">Python 交互解释器预览</h1>
        <p class="console-preview-desc">这里保留原有 WEB-IDLE 交互功能，仅补充一个更清晰的页面外壳，方便教师从学案中返回。</p>
        <div class="ls-toolbar console-preview-toolbar">
            <a id="btnreturn" href="#" class="ls-toolbar__btn ls-toolbar__btn--neutral"><i class="fa fa-reply" aria-hidden="true"></i><span>返回学案</span></a>
        </div>
    </div>
    <div class="container ">
        <div id="editor" class="ace-gruvbox ace_editor" > </div>
    </div>
</div>
    <form id="form1" runat="server">
    <div>
        <asp:HiddenField ID="hidenjson" runat="server" />
        <asp:HiddenField ID="hidennid" runat="server" />
        <asp:HiddenField ID="hidencid" runat="server" />
        <asp:HiddenField ID="hidenlid" runat="server" />
    </div>
    </form>
<script src="../code/jquery.min.js" type="text/javascript"></script>
<script src="../code/ipython.js?ver=20211015"  type="text/javascript"></script>
</body>
</html>
