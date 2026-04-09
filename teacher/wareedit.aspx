<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master"  Validaterequest="false" AutoEventWireup="true" CodeFile="wareedit.aspx.cs" Inherits="teacher_wareedit" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
<link href="../App_Themes/Teacher/wareedit.css" rel="stylesheet" />
<link href="../App_Themes/Teacher/admin-form.css" rel="stylesheet" />

<div class="admin-form-page ware-page">
    <div class="admin-form-shell">
        <section class="admin-form-hero">
            <div class="admin-form-hero-content">
                <div class="admin-form-eyebrow">Web Courseware</div>
                <h1 class="admin-form-title">编辑网页课件</h1>
                <p class="admin-form-subtitle">维护课件资源文件与首页入口，修改后会同步更新当前学案中的网页课件主题信息。</p>
            </div>
        </section>

        <section class="admin-form-panel">
            <h2 class="admin-form-section-title">课件设置</h2>
            <p class="admin-form-section-desc">调整主题名称、发布状态和首页地址，文件列表中的 HTML 文件仍可一键设置为首页。</p>
            <div class="admin-form-grid">
                <div class="admin-form-field">
                    <label class="admin-form-label" for="<%= Texttitle.ClientID %>">课件主题</label>
                    <asp:TextBox ID="Texttitle" runat="server"  SkinID="TextBoxNormal"
                        Width="220px"  CssClass="admin-form-input"></asp:TextBox>
                </div>
                <div class="admin-form-field">
                    <label class="admin-form-label" for="<%= CheckPublish.ClientID %>">发布状态</label>
                    <div class="admin-form-static"><asp:CheckBox ID="CheckPublish" runat="server" Text="是否发布"  Checked="True" /></div>
                </div>
                <div class="admin-form-field admin-form-field-wide">
                    <label class="admin-form-label" for="<%= TextBoxHtml.ClientID %>">课件首页</label>
                    <asp:TextBox ID="TextBoxHtml" runat="server" Width="300px" CssClass="admin-form-input"></asp:TextBox>
                </div>
            </div>
        </section>

        <section class="admin-form-panel">
            <h2 class="admin-form-section-title">文件管理</h2>
            <p class="admin-form-section-desc">继续上传、删除或选择资源文件，右侧列表会实时显示当前课件目录内容。</p>
            <div class="main-content">
             <!-- 左侧文件夹导航 -->
             <div class="sidebar">
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
                 <div class="file-list" id="fileList">
                     <div class="empty-state">
                     </div>
                 </div>
             </div>
            </div>
        </div>
     <div  class="placehold">
        <div class="bg-blue-50 border-l-4 border-blue-500 p-4 mb-4 rounded shadow-sm text-left">
            <div class="flex items-center mb-2">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="text-blue-500 mr-2" viewBox="0 0 16 16" aria-hidden="true" focusable="false">
                    <path d="M8 16A8 8 0 1 0 8 0a8 8 0 0 0 0 16zM8.93 6.588 8.558 9.29c-.07.34-.294.533-.635.533-.194 0-.487-.07-.686-.246l.088-.416c.145.07.294.105.416.105.197 0 .28-.07.32-.246l.287-2.134c.07-.34.294-.533.635-.533.194 0 .487.07.686.246l-.088.416a1.02 1.02 0 0 0-.416-.105c-.197 0-.28.07-.32.246zM8 5.5a1 1 0 1 1 0-2 1 1 0 0 1 0 2z"/>
                </svg>
                <h4 class="text-blue-800 font-bold m-0">学习平台成绩采集集成指南</h4>
            </div>
            <p class="text-sm text-blue-900 mb-2">
                为方便学习平台准确获取并采集HTML课件中产生的学生评估分数，请将以下代码放入交互网页（如游戏、测验）的<strong>提交按钮事件</strong>或<strong>分数生成事件</strong>中：
            </p>
            <div class="bg-slate-800 rounded p-3 relative group">
                <code class="text-green-400 text-sm font-mono block">
                    const message = { name: "测验名称", value: score }; // 消息字典为测验名称和score成绩<br/>
                    window.parent.postMessage(JSON.stringify(message), "*"); // 向父页面发送消息
                </code>
                <button type="button" onclick="navigator.clipboard.writeText('const message = { name: \'测验名称\', value: score };\nwindow.parent.postMessage(JSON.stringify(message), \'*\');'); alert('代码已复制到剪贴板');" class="absolute top-2 right-2 bg-slate-600 text-white text-xs px-2 py-1 rounded opacity-0 group-hover:opacity-100 group-focus-within:opacity-100 focus:opacity-100 focus-visible:opacity-100 focus:outline-none focus-visible:ring-2 focus-visible:ring-blue-400 focus-visible:ring-offset-2 focus-visible:ring-offset-slate-800 transition-opacity cursor-pointer border-0">
                    复制代码
                </button>
            </div>
            <p class="text-xs text-blue-700 mt-2">
                <strong>说明：</strong> <code>score</code> 变量必须是数值类型。当触发此代码时，平台会自动捕获分数，学生点击保存时即可将成绩汇总至教师端。
            </p>
        </div>
         <br />
              <asp:Button ID="Btnedit" runat="server"  Text="修改主题" OnClick="Btnedit_Click"  SkinID="BtnNormal"  CssClass="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" />&nbsp;&nbsp;&nbsp;
              <asp:Button ID="BtnCourse" runat="server"  Text="学案返回" OnClick="BtnCourse_Click"  SkinID="BtnNormal"  CssClass="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" /><br />
         <br />
         </div>
           
</div>


    <script type="text/javascript">
        window.__wareeditConfig = {
            cid: '<%=Cid %>',
            textBoxHtmlId: '<%= TextBoxHtml.ClientID %>'
        };
    </script>
    <script type="text/javascript" src="../js/wareedit.js"></script>
</asp:Content>
