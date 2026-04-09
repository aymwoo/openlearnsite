<%@ Page Title="" Language="C#" MasterPageFile="~/student/Scm.master" AutoEventWireup="true" CodeFile="myexam.aspx.cs" Inherits="student_myexam" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cpcm" Runat="Server">
<link href="../App_Themes/Student/myexam.css" rel="stylesheet" />


<div class="survey-exam-page">
    <div class="survey-exam-toolbar">
        <div class="survey-exam-toolbar__intro">
            <div class="survey-exam-toolbar__eyebrow">课堂测验</div>
            <div class="survey-exam-toolbar__title">旧版 Survey 作答</div>
            <div class="survey-exam-toolbar__meta">完成后提交答卷，系统会按当前测验设置生成 AI 评价或规则评估摘要。</div>
        </div>
        <div class="survey-exam-toolbar__actions">
            <div class="survey-toolbar-badge">当前得分 <%=Lbfscore.Text %> 分</div>
            <asp:HyperLink ID="Hkscore" runat="server" Target="_blank" Visible="False" Text="查看统计"
                CssClass="survey-toolbar-btn survey-toolbar-btn--analysis"></asp:HyperLink>
            <button type="button" class="survey-toolbar-btn survey-toolbar-btn--neutral" onclick="returnurl();">
                <span>返回学案</span>
            </button>
        </div>
    </div>

    <div class="survey-header-card">
        <div class="course-node-head flex items-center gap-3 mb-4" style="padding:24px 24px 20px;margin:-24px -24px 16px;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/images/clock.gif" CssClass="w-8 h-8" />
            <asp:Label runat="server" ID="Lbtitle" CssClass="course-node-title text-xl font-extrabold text-slate-800 tracking-tight"></asp:Label>
        </div>

        <div class="flex flex-wrap gap-x-6 gap-y-2 items-center py-3 px-4 bg-slate-50 rounded-xl border border-slate-100 text-sm text-slate-600">
            <div class="flex items-center gap-1.5">
                <svg class="w-4 h-4 text-blue-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z"></path></svg>
                <span class="text-slate-500">姓名</span>
                <asp:Label runat="server" ID="Lbsname" CssClass="font-semibold text-slate-800"></asp:Label>
            </div>
            <div class="flex items-center gap-1.5">
                <svg class="w-4 h-4 text-indigo-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10 6H5a2 2 0 00-2 2v9a2 2 0 002 2h14a2 2 0 002-2V8a2 2 0 00-2-2h-5m-4 0V5a2 2 0 114 0v1m-4 0a2 2 0 104 0"></path></svg>
                <span class="text-slate-500">学号</span>
                <asp:Label runat="server" ID="Lbsnum" CssClass="font-semibold text-slate-800"></asp:Label>
            </div>
            <div class="flex items-center gap-1.5">
                <svg class="w-4 h-4 text-emerald-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"></path></svg>
                <span class="text-slate-500">得分</span>
                <asp:Label runat="server" ID="Lbfscore" CssClass="font-bold text-emerald-600"></asp:Label>
            </div>
            <div class="flex items-center gap-1.5">
                <svg class="w-4 h-4 text-amber-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M7 7h.01M7 3h5c.512 0 1.024.195 1.414.586l7 7a2 2 0 010 2.828l-7 7a2 2 0 01-2.828 0l-7-7A1.994 1.994 0 013 12V7a4 4 0 014-4z"></path></svg>
                <span class="text-slate-500">类型</span>
                <asp:Label runat="server" ID="Lbtypecn" CssClass="font-semibold text-slate-800"></asp:Label>
            </div>
            <asp:Label runat="server" ID="Lbcheck"></asp:Label>
            <asp:Label runat="server" ID="Lbtype" Visible="False"></asp:Label>
			<asp:Label ID="LabelCid" runat="server" Visible="False"></asp:Label>
			<asp:Label ID="LabelLid" runat="server" Visible="False"></asp:Label>
            <asp:Label ID="LabelVid" runat="server" Visible="False"></asp:Label> 
            <asp:Label ID="LabelVtotal" runat="server" Visible="False"></asp:Label>
        </div>
    </div>

    <div class="survey-content-card">
        <div id="vcontent" runat="server" class="text-slate-700 leading-relaxed"></div>
    </div>

    <div class="survey-assessment-card">
        <div class="survey-assessment-card__info">
            <div class="survey-assessment-card__label">评价方式</div>
            <div class="survey-assessment-card__title"><%= EnableAiAssessment ? "AI 评价" : "规则评估摘要" %></div>
            <div class="survey-assessment-card__desc"><%= EnableAiAssessment ? ("当前测验已启用 AI 评价，提交后会调用 AI Provider 生成测验反馈。") : "当前测验未启用 AI 评价，提交后会生成规则评估摘要。" %></div>
        </div>
        <div class="survey-assessment-card__status"><%= EnableAiAssessment ? "AI 已启用" : "规则模式" %></div>
    </div>

    <div class="quizarea">
        <div class="survey-quiz-head">
            <div class="survey-quiz-head__title">开始答题</div>
            <div class="survey-quiz-head__meta">共 <%=LabelVtotal.Text %> 题，请完成后再提交答卷。</div>
        </div>
        <div id="questionPage"></div>
        <div class="btnsubmit">
            <input id="btnupload" class="px-6 py-2.5 bg-gradient-to-r from-blue-500 to-indigo-600 text-white font-bold rounded-xl hover:from-blue-600 hover:to-indigo-700 transition duration-300 shadow-md border-0 cursor-pointer" type="button" value="提交答卷" />
            <div class="survey-submit-note"><%= isDone ? "你已完成本次测验，如需更新结果请联系教师。" : (isClose ? "当前测验尚未开始或已关闭，请等待教师开启后再提交。" : "提交后会自动生成当前测验对应的评估结果。") %></div>
        </div>   
    </div>
</div>

<div id="examAiLoading" style="display:none;position:fixed;inset:0;z-index:9999;background:rgba(15,23,42,.45);backdrop-filter:blur(3px);align-items:center;justify-content:center;">
    <div style="width:min(92vw,420px);padding:28px 24px;border-radius:20px;background:rgba(255,255,255,.98);box-shadow:0 24px 50px rgba(15,23,42,.22);text-align:center;">
        <div style="width:56px;height:56px;margin:0 auto 16px;border-radius:999px;border:5px solid #dbeafe;border-top-color:#2563eb;animation:gaugeitem-spin .9s linear infinite;"></div>
        <p id="examAiLoadingTitle" style="margin:0;font-size:18px;font-weight:800;color:#0f172a;">正在提交测验并生成 AI 评估</p>
        <p id="examAiLoadingDesc" style="margin:10px 0 0;font-size:14px;line-height:1.7;color:#64748b;">系统正在提交测验结果，请稍候。</p>
        <p id="examAiProvider" style="margin:10px 0 0;font-size:12px;color:#475569;">当前评估方式：<%= EnableAiAssessment ? ("AI Provider - " + LearnSite.BLL.AIStudentExamGenerator.GetDefaultProviderDisplayName()) : "规则评估摘要" %></p>
    </div>
</div>

<div id="examAiSummary" style="display:none;max-width:980px;margin:16px auto 0;padding:14px 16px;border-radius:16px;border:1px solid #bfdbfe;background:linear-gradient(135deg,#eff6ff 0%,#f8fbff 100%);color:#1e3a8a;box-shadow:0 10px 24px rgba(37,99,235,.08);">
    <div id="examAiSummaryLabel" style="font-size:12px;font-weight:700;color:#475569;">AI 简短反馈</div>
    <div id="examAiSummaryText" style="margin-top:8px;font-size:14px;line-height:1.7;color:#1d4ed8;"></div>
</div>


    <script type="text/javascript">
        window.__myexamConfig = {
            questionList: <%=questionList %>,
            isClose: <%=isClose.ToString().ToLower() %>,
            isDone: <%=isDone.ToString().ToLower() %>,
            enableAiAssessment: <%=EnableAiAssessment.ToString().ToLower() %>,
            lidstr: '<%=Lidstr %>',
            cidstr: '<%=Cidstr %>',
            vidstr: '<%=Vidstr %>',
            vtypestr: '<%=Vtypestr %>',
            fpage: '<%=Fpage %>',
            learnSite_BLL_AIStudentExamGenerator_GetDefaultProviderDisplayName: '<%= LearnSite.BLL.AIStudentExamGenerator.GetDefaultProviderDisplayName() %>'
        };
    </script>
    <script type="text/javascript" src="../js/myexam.js"></script>
</asp:Content>
