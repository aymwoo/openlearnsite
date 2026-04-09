<%@ Page Language="C#" AutoEventWireup="true" CodeFile="codeshare.aspx.cs" Inherits="Student_codeshare" ResponseEncoding="utf-8" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
        <meta charset="utf-8" />
<title></title>
    <style type="text/css">
        body { margin: 0; display: flex; justify-content: center; align-items: flex-start; min-height: 100vh; background: #f0f2f5; }
    </style>
    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
</head>
<body>
<div class="flex flex-col items-center p-4">
  <script type="text/javascript">
    window.scratchConfig = {
      
      handleVmInitialized: (vm) => {
        window.vm = vm
        console.log("VM")
        
      },
      handleProjectLoaded:() => {
        console.log("load")

      },
      handleDefaultProjectLoaded:() => {

          window.scratch.loadProject("<%=sbfile %>", () => { 
             console.log("add")
             window.scratch.setProjectName("<%=sbtitle %>")
          })
      },
    }
  </script>

  <div id="scratchplayer" class="bg-white rounded-xl shadow-md border border-slate-200 overflow-hidden" style="width:482px; height:400px;"></div>

<script type="text/javascript" src="../scratch/lib.min.js"></script>
<script type="text/javascript" src="../scratch/chunks/player.js"></script>
</div>
</body>
</html>
