<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" AutoEventWireup="true" CodeFile="exam.aspx.cs" Inherits="webform_exam" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="bootstrap.min.css" rel="stylesheet">   
    <link href="summernote-bs5.min.css" rel="stylesheet">  
    <link rel="stylesheet" href="github.min.css">   
    <script src="jquery-3.6.0.min.js"></script>    
    <script src="bootstrap.bundle.min.js"></script>   
    <script src="summernote-bs5.min.js"></script>   
    <script src="summernote-zh-CN.min.js"></script>   
    <script src="highlight.min.js"></script>    
    <link href="paper.css" rel="stylesheet">

    

    <div class="container exam-editor-page">
        <div class="exam-editor-shell">
            <div class="exam-editor-main">
                <div class="panel edit-area" id="editArea">
                </div>
            </div>
            <aside class="exam-editor-sidebar">
                <div class="floating-action-bar" title="测验工具栏">
                    <div class="toolbar-group toolbar-group-status">
                        <div class="exam-page-heading">
                            <div class="exam-page-eyebrow">课堂测验</div>
                            <div class="exam-page-title">测验编辑器</div>
                            <div class="exam-page-meta">统一设置标题、描述、题目内容与 AI 评价方式。</div>
                        </div>
                        <label class="floating-score-editor">
                            <span class="btn-icon">💰</span>
                            <span id="examScore" class="btn-text">00 分</span>
                        </label>
                        <div class="exam-side-ai-setting" id="examSideAiSetting">
                            <div class="exam-side-ai-setting__header">
                                <span class="exam-side-ai-setting__title">AI 评测</span>
                                <span class="exam-side-ai-setting__status" id="examSideAiStatus">当前试卷未启用</span>
                            </div>
                            <label class="exam-side-ai-setting__toggle">
                                <span class="exam-side-ai-setting__toggle-text">启用 AI 评测，提交后生成 AI 测验评估。</span>
                                <input type="checkbox" id="examSideAiToggle" onchange="updateExamAiAssessment(this.checked)">
                            </label>
                        </div>
                    </div>
                    <div class="toolbar-group toolbar-group-actions">
                        <button type="button" class="floating-btn floating-btn-preview-modal" id="previewModalBtn" title="模态预览">
                            <span class="btn-icon">👁</span>
                            <span class="btn-text">模态预览</span>
                        </button>
                        <button type="button" class="floating-btn floating-btn-save" id="saveExamBtn" title="保存为JSON文件">
                            <span class="btn-icon">💾</span>
                            <span class="btn-text">保存作品</span>
                        </button>
                        <button type="button" class="floating-btn floating-btn-import" id="importBtn" title="导入JSON文件">
                            <span class="btn-icon">📁</span>
                            <span class="btn-text">导入题库</span>
                        </button>
                        <button type="button" class="floating-btn floating-btn-clear" id="clearExamBtn" title="清空所有试题">
                            <span class="btn-icon">🗑️</span>
                            <span class="btn-text">清空题目</span>
                        </button>
                        <a href="#" class="floating-btn floating-btn-single-editor" onclick ="returnurl();" title="返回学案">
                            <span class="btn-icon">↩</span>
                            <span class="btn-text">返回学案</span>
                        </a>
                    </div>
                    <input type="file" id="importFileInput" accept=".json" style="display: none;">
                </div>
            </aside>
        </div>
    </div>
    
    <!-- 预览模态框 -->
    <div class="preview-modal" id="previewModal">
        <div class="preview-content">
            <span class="close">&times;</span>
            <div id="previewArea">
                <!-- 预览内容将在这里动态生成 -->
            </div>
        </div>
    </div>
    
    <input id="HiddenCid" type="hidden" value="<%=Cid %>" />
    <input id="HiddenEid" type="hidden" value="<%=Eid %>" />
    <input id="HiddenExamjson" type="hidden" value="<%=Examjson %>" />
    
    <!-- 引入外部JavaScript文件 -->
    <script src="exam.js"></script> 

</asp:Content>
