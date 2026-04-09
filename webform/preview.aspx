<%@ Page Title="" Language="C#" MasterPageFile="~/student/Scm.master" AutoEventWireup="true" CodeFile="preview.aspx.cs" Inherits="webform_preview" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cpcm" Runat="Server">
    <link rel="stylesheet" href="github.min.css">    
    <script src="highlight.min.js"></script>    
    <link href="preview.css" rel="stylesheet">
    
    <input id="HiddenDone" type="hidden" value="<%=Done %>" />
    <input id="HiddenLid" type="hidden" value="<%=Lid %>" />
    <input id="HiddenCid" type="hidden" value="<%=Cid %>" />
    <input id="HiddenEid" type="hidden" value="<%=Eid %>" />
    <input id="HiddenExamjson" type="hidden" value="<%=Examjson %>" />

    <div class="preview-container">
        <div class="floating-buttons" title="测验工具栏">
            <div class="preview-toolbar-intro">
                <div class="preview-toolbar-eyebrow">课堂测验</div>
                <div class="preview-toolbar-title">在线作答</div>
                <div class="preview-toolbar-meta">完成后提交答卷，系统会按当前测验设置生成评估结果。</div>
            </div>
            <div class="preview-toolbar-actions">
                <img id="submitImg" src="../images/passed.png" alt="提交状态" style="display: none;" />
                <span id="submitScore" class="score-submit" style="display: none;">💰 <%=Score %>分</span>
                <asp:HyperLink ID="HyperLinkAnalysis" runat="server" Target="_blank" CssClass="score-analysis">查看统计</asp:HyperLink>
                <button type="button" class="floating-btn btn-submit" id="submitBtn" title="提交试卷">
                    <span class="btn-icon">✔</span>
                    <span class="btn-text">提交答卷</span>
                </button>
            </div>
        </div>

        <div class="exam-paper" id="examPaper">
            <div class="exam-info">
                <h1 class="exam-title" id="examTitle">课堂测验</h1>
                <p class="exam-desc" id="examDescription">试卷描述</p>
            </div>

            <div class="questions-container" id="questionsContainer">
                <!-- 题目将在这里动态生成 -->
            </div>

            <div class="empty-state" id="emptyState" style="display: none;">
                <p>暂无题目，请先添加题目</p>
            </div>
        </div>
    </div>
    
    <!-- AI 评估加载遮罩 -->
    <div id="examAiLoading" style="display:none;position:fixed;inset:0;z-index:9999;background:rgba(15,23,42,.45);backdrop-filter:blur(3px);align-items:center;justify-content:center;">
        <div style="width:min(92vw,420px);padding:28px 24px;border-radius:20px;background:rgba(255,255,255,.98);box-shadow:0 24px 50px rgba(15,23,42,.22);text-align:center;">
            <div style="width:56px;height:56px;margin:0 auto 16px;border-radius:999px;border:5px solid #dbeafe;border-top-color:#2563eb;animation:examai-spin .9s linear infinite;"></div>
            <p id="examAiLoadingTitle" style="margin:0;font-size:18px;font-weight:800;color:#0f172a;">正在提交测验并生成 AI 评估</p>
            <p id="examAiLoadingDesc" style="margin:10px 0 0;font-size:14px;line-height:1.7;color:#64748b;">系统正在提交测验结果，请稍候。</p>
        </div>
    </div>
    <style>@keyframes examai-spin{to{transform:rotate(360deg)}}</style>

    <!-- AI 摘要反馈区域 -->
    <div id="examAiSummary" style="display:none;max-width:980px;margin:16px auto 0;padding:14px 16px;border-radius:16px;border:1px solid #bfdbfe;background:linear-gradient(135deg,#eff6ff 0%,#f8fbff 100%);color:#1e3a8a;box-shadow:0 10px 24px rgba(37,99,235,.08);">
        <div id="examAiSummaryLabel" style="font-size:12px;font-weight:700;color:#475569;">AI 简短反馈</div>
        <div id="examAiSummaryText" style="margin-top:8px;font-size:14px;line-height:1.7;color:#1d4ed8;"></div>
    </div>

    <!-- 预览页面脚本 -->
    <script src="preview.js"></script>

</asp:Content>
