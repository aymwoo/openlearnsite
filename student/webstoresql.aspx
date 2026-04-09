<%@ Page Language="C#" AutoEventWireup="true" CodeFile="webstoresql.aspx.cs" Inherits="student_webstoresql" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>信息科技素材库</title>
    <link href="../js/webstore.css" rel="stylesheet" type="text/css" />
    <!-- 引入KindEditor富文本编辑器 -->
    <script charset="utf-8" src="../kindeditor/kindeditor-min.js"></script>
    <script charset="utf-8" src="../kindeditor/lang/zh_CN.js"></script>
    <script src="../code/jquery.min.js" type="text/javascript"></script>
    <script src="../code/html2canvas.min.js" type="text/javascript"></script>
	<link rel="stylesheet" href="../deepseek/all.min.css">
	

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Student/webstoresql.css" />
</head>
<body>
    <!-- 移除了表单标签，避免按钮点击导致表单提交 -->
    <div id="workHistory"  class="container">
        <div class="header">           
            <div class="button-container store-toolbar">                
                <sp class="banner store-toolbar__brand">
                    <span>🌏</span>
                    <span>信息科技素材库</span>
                </sp> 
                <div class="store-toolbar__actions">
                    <button class="btn return-button store-toolbar__btn store-toolbar__btn--neutral" type="button" onclick="returnurl();"><i class="fa fa-reply" aria-hidden="true"></i><span>返回学案</span></button>
                    <button class="btn save-button store-toolbar__btn" type="button" onclick="savework();"><i class="fa fa-save" aria-hidden="true"></i><span>保存作品</span></button>
                </div>
            </div>
        </div>
        <hr class ="hrclass"/>
        <div class="main-content">
            <!-- 左侧文件夹导航 -->
            <div class="sidebar">
                <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 15px;">
                    <h3 class="root-title" onclick="goToRoot()">
                    💼 <asp:Label ID="Labelname" runat="server" ></asp:Label>云盘</h3>
	                    <button type="button" class="btn btn-success store-subbtn store-subbtn--success" onclick="showCreateFolderModal()">新建文件夹</button>
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
	                    <button type="button" class="btn btn-primary store-subbtn" onclick="document.getElementById('fileInput').click()">选择文件</button>
                    <p style="font-size: 13px; ">支持图片、文档、音频、视频文件</p>                        <!-- 新增：上传进度条 -->
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
                        <!-- 视图切换按钮 -->
                        <div class="view-toggle">
                            <div class="view-toggle-btn active" id="listViewBtn" onclick="toggleView('list')" title="列表视图">
                                <span>≡</span>
                            </div>
                            <div class="view-toggle-btn" id="gridViewBtn" onclick="toggleView('grid')" title="平铺视图">
                                <span>田</span>
                            </div>
                        </div>
                    <h3 id="currentFolderTitle" contenteditable="false" ondblclick="editFolder(this)" onblur="renameFolderOnBlur(this)" ></h3>
                    <div>
	                        <button type="button" class="btn btn-primary store-subbtn" onclick="showCreateDocModal()">新建文档</button>
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
	                <button type="button" class="modal-close store-subbtn store-subbtn--neutral" onclick="closeCreateFolderModal()">&times;</button>
            </div>
            <div class="form-group">
                <input type="text" class="form-control px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300" id="newFolderName" placeholder="请输入文件夹名称">
            </div>
            <div style="text-align: right;">
	                <button type="button" class="btn store-subbtn store-subbtn--neutral" onclick="closeCreateFolderModal()" style="margin-right: 10px;">取消</button>
	                <button type="button" class="btn btn-primary store-subbtn store-subbtn--success" onclick="createFolder()">创建</button>
            </div>
        </div>
    </div>
    
    <!-- 新建文档模态框 -->
    <div class="modal" id="createDocModal">
        <div class="modal-content" style="width: 90%; max-width: 1000px; max-height: 90vh; overflow-y: auto;">
            <div class="modal-header">
                <h3>新建文档</h3>
	                <button type="button" class="modal-close store-subbtn store-subbtn--neutral" onclick="closeCreateDocModal()">&times;</button>
            </div>
            <div class="form-group">
                <label for="docTitle">文档标题</label>
                <input type="text" class="form-control px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300" id="docTitle" placeholder="请输入文档标题">
            </div>
            <div class="form-group">
                <textarea class="form-control" id="docContent" rows="15" placeholder="请输入文档内容..." style="resize: vertical; width: 100%; height: 400px;"></textarea>
            </div>
            <div style="text-align: right;">
	                <button type="button" class="btn store-subbtn store-subbtn--neutral" onclick="closeCreateDocModal()" style="margin-right: 10px;">取消</button>
	                <button type="button" class="btn btn-primary store-subbtn store-subbtn--success" onclick="saveDocument()">保存文档</button>
            </div>
        </div>
    </div>
    
    <!-- 提示消息 -->
    <div id="toast" class="toast hidden">链接已复制到剪贴板</div>

    
    <script type="text/javascript">
        window.__webstoresqlConfig = {
            mysnum: "<%=mysnum%>",
            id: "<%=Id %>",
            fpage: "<%=Fpage %>"
        };
    </script>
    <script type="text/javascript" src="../js/webstoresql.js"></script>
</body>
</html>
