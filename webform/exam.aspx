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
                        <button type="button" class="floating-btn floating-btn-bank" id="importFromBankBtn" title="从题库导入">
                            <span class="btn-icon">📚</span>
                            <span class="btn-text">题库导入</span>
                        </button>
                        <button type="button" class="floating-btn floating-btn-import" id="importBtn" title="导入JSON文件">
                            <span class="btn-icon">📁</span>
                            <span class="btn-text">导入文件</span>
                        </button>
                        <a href="convertor.html" target="_blank" class="floating-btn floating-btn-convertor" title="题目格式转换工具" id="convertorBtn">
                            <span class="btn-icon">✨</span>
                            <span class="btn-text">格式转换</span>
                        </a>
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
            </div>
        </div>
    </div>
    
    <!-- 题库导入模态框 -->
    <div class="bank-modal" id="bankModal">
        <div class="bank-modal-content">
            <div class="bank-modal-header">
                <h3>从题库导入题目</h3>
                <button type="button" class="bank-modal-close" onclick="closeBankModal()">&times;</button>
            </div>
            <div class="bank-modal-body">
                <div class="bank-sidebar">
                    <div class="bank-filter">
                        <select id="bankSelect" onchange="loadBankQuestions()">
                            <option value="0">全部题库</option>
                        </select>
                    </div>
                    <div class="bank-filter">
                        <select id="questionTypeFilter" onchange="loadBankQuestions()">
                            <option value="0">全部题型</option>
                            <option value="1">单选题</option>
                            <option value="2">多选题</option>
                            <option value="3">判断题</option>
                            <option value="4">填空题</option>
                            <option value="5">简答题</option>
                        </select>
                    </div>
                    <div class="bank-filter">
                        <select id="difficultyFilter" onchange="loadBankQuestions()">
                            <option value="0">全部难度</option>
                            <option value="1">简单</option>
                            <option value="2">中等</option>
                            <option value="3">困难</option>
                        </select>
                    </div>
                    <div class="bank-filter">
                        <input type="text" id="keywordFilter" placeholder="搜索题目..." onkeyup="searchQuestions(event)">
                    </div>
                </div>
                <div class="bank-main">
                    <div class="bank-toolbar">
                        <label class="bank-select-all">
                            <input type="checkbox" id="selectAllQuestions" onchange="toggleSelectAll()">
                            <span>全选</span>
                        </label>
                        <span class="bank-selected-count">已选 <strong id="selectedCount">0</strong> 题</span>
                    </div>
                    <div class="bank-question-list" id="bankQuestionList">
                        <div class="bank-loading">加载中...</div>
                    </div>
                    <div class="bank-pagination" id="bankPagination">
                    </div>
                </div>
            </div>
            <div class="bank-modal-footer">
                <button type="button" class="bank-btn bank-btn-secondary" onclick="closeBankModal()">取消</button>
                <button type="button" class="bank-btn bank-btn-primary" onclick="importSelectedQuestions()">导入选中题目</button>
            </div>
        </div>
    </div>
    
    <input id="HiddenCid" type="hidden" value="<%=Cid %>" />
    <input id="HiddenEid" type="hidden" value="<%=Eid %>" />
    <input id="HiddenExamjson" type="hidden" value="<%=Examjson %>" />
    
    <link href="questionbank.css" rel="stylesheet">
    <script src="exam.js"></script> 

</asp:Content>
