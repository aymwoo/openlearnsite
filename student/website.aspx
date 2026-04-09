<%@ Page Language="C#" AutoEventWireup="true" CodeFile="website.aspx.cs" Inherits="student_website" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>信息科技素材库</title>
    <link href="../js/website.css" rel="stylesheet" type="text/css" />
    <!-- 引入KindEditor富文本编辑器 -->
    <script src="../code/jquery.min.js" type="text/javascript"></script>
    <script src="../code/html2canvas.min.js" type="text/javascript"></script>
	<link rel="stylesheet" href="../deepseek/all.min.css">
	

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Student/website.css" />
</head>
<body>
    <!-- 移除了表单标签，避免按钮点击导致表单提交 -->
    <div id="workHistory"  class="container">
        <div class="header">           
            <div class="button-container website-toolbar">                
                <sp class="banner website-toolbar__brand">
                    <img src="../images/weblogo.png" alt="网站设计" />
                    <span><asp:Label ID="Labelname" runat="server" ></asp:Label> 在线网站设计</span>
                </sp> 
                <div class="website-toolbar__actions">
                    <button class="btn return-button website-toolbar__btn website-toolbar__btn--neutral" type="button" onclick="returnurl();"><i class="fa fa-reply" aria-hidden="true"></i><span>返回学案</span></button>
                    <button class="btn save-button website-toolbar__btn" type="button" onclick="savework();"><i class="fa fa-save" aria-hidden="true"></i><span>保存网站</span></button>
                </div>
            </div>
        </div>
        <hr class ="hrclass"/>
        <div class="main-content">
            <!-- 左侧文件夹导航 -->
            <div class="sidebar">
                <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 15px;">
                    <h3 class="root-title" onclick="goToRoot()">📁 网页目录</h3>                    
                    <button type="button" class="btn btn-success px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" onclick="showCreateFolderModal()" style="display:none">新建目录</button>
                </div>

                <div class="folder-tree" id="folderTree">
                    <div class="empty-state">
                        <i>📁</i>
                        <p>加载中...</p>
                    </div>
                </div>
                <div class="upload-zone" id="uploadZone">
                    <i class="file-icon" style="font-size: 48px; color: #007bff;">📄</i>
                    <h3>拖放文件到此处上传</h3>
                    <input type="file" id="fileInput" multiple style="display: none;">                    
                    <button type="button" class="btn btn-primary px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" onclick="document.getElementById('fileInput').click()">选择文件</button>
                    <p style="font-size: 13px; ">支持图片、文档、音频、视频文件</p>
                        <!-- 新增：上传进度条 -->
                    <div id="uploadProgressContainer" style="display: none; width: 100%; margin-top: 15px;">
                        <div style="display: flex; justify-content: space-between; margin-bottom: 5px;">
                            <span id="uploadFileName" style="font-size: 12px;"></span>
                            <span id="uploadPercent" style="font-size: 12px;">0%</span>
                        </div>
                        <div class="progress-bar" style="width: 100%; height: 6px; background-color: #e9ecef; border-radius: 3px; overflow: hidden;">
                            <div id="uploadProgressBar" style="width: 0%; height: 100%; background-color: #007bff; transition: width 0.3s ease;"></div>
                        </div>
                        <div id="uploadStatus" style="font-size: 12px; text-align: center; margin-top: 5px;"></div>
                    </div>
                    
                </div>
            </div>
            
            <!-- 右侧内容区域 -->
            <div class="content">
                <!-- 文档操作按钮区域 -->
                <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 5px;">
                    <h3 id="currentFolderTitle" contenteditable="true" onblur="renameFolderOnBlur(this)" ></h3>
                    <div>                      
                    <button type="button" class="btn btn-primary px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" onclick="showCreateDocModal()">新建网页</button>  
                    </div>
                </div>
                
                
                <div class="file-list" id="fileList">
                    <div class="empty-state">
                        <i>📂</i>
                        <p>当前文件夹为空</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <!-- 新建文件夹模态框 -->
    <div class="modal" id="createFolderModal">
        <div class="modal-content">
            <div class="modal-header">
                <h3>新建文件夹</h3>
                <button type="button" class="modal-close px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" onclick="closeCreateFolderModal()">&times;</button>
            </div>
            <div class="form-group">
                <input type="text" class="form-control px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300" id="newFolderName" placeholder="请输入文件夹名称">
            </div>
            <div style="text-align: right;">
                <button type="button" class="btn px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" onclick="closeCreateFolderModal()" style="margin-right: 10px;">取消</button>
                <button type="button" class="btn btn-primary px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" onclick="createFolder()">创建</button>
            </div>
        </div>
    </div>
    
    <!-- 新建网页模态框 -->
    <div class="modal" id="createDocModal">
        <div class="modal-content" style="width: 600px; height: 200px; ">
            <div class="modal-header">
                <h3>新建网页</h3>
                <button type="button" class="modal-close px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" onclick="closeCreateDocModal()">&times;</button>
            </div>
            <div class="form-group">
                <input type="text" class="form-control px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300" id="docTitle" placeholder="请输入网页文件名">
                <label class="docname">示例：
                    <span class="spname" title="首页">index</span>
                    <span class="spname" title="图片">photo</span>
                    <span class="spname" title="音频">audio</span>
                    <span class="spname" title="视频">video</span>
                    <span class="spname" title="关于">about</span>
                </label>
            </div>
            <div style="text-align: right;">
                <button type="button" class="btn px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" onclick="closeCreateDocModal()" style="margin-right: 10px;">取消</button>
                <button type="button" class="btn btn-primary px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" onclick="saveDocument()">创建网页</button>
            </div>
        </div>
    </div>
    
    <!-- 提示消息 -->
    <div id="toast" class="toast hidden">链接已复制到剪贴板</div>

    
    <script type="text/javascript">
        window.__websiteConfig = {
            mysnum: "<%=mysnum%>",
            id: "<%=Id %>",
            lid: "<%=Lid %>",
            fpage: "<%=Fpage %>"
        };
    </script>
    <script type="text/javascript" src="../js/website.js"></script>
</body>
</html>
