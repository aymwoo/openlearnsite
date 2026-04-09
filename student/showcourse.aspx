<%@ Page Title="" Language="C#" MasterPageFile="~/student/Scm.master" StylesheetTheme="Student"  Validaterequest="false"  AutoEventWireup="true" CodeFile="showcourse.aspx.cs" Inherits="Student_showcourse" ResponseEncoding="utf-8" %>
<asp:Content ID="Content1" ContentPlaceHolderID="Cpcm" Runat="Server">
<style type="text/css">
    .student-doc-wrap {
        background: transparent;
        border: none;
        border-radius: 0;
        box-shadow: none;
        overflow: hidden;
        margin-top: 0;
        margin-bottom: 1rem;
    }
    .student-doc-head {
        background: linear-gradient(135deg, #f8fafc 0%, #ffffff 100%);
        border-bottom: 1px solid #f1f5f9;
        padding: 40px 30px;
        text-align: center;
        position: relative;
    }
    .has-course-banner .student-doc-head {
        border-bottom-color: rgba(255,255,255,0.2);
    }
    .student-doc-head::before {
        content: '';
        position: absolute;
        top: 0; left: 0; right: 0;
        height: 5px;
        background: linear-gradient(90deg, #6366f1, #3b82f6, #0ea5e9, #10b981);
    }
    .has-course-banner .student-doc-head::before {
        background: linear-gradient(90deg, rgba(255,255,255,0.92), rgba(191,219,254,0.92), rgba(167,243,208,0.9));
    }
    .student-doc-title {
        font-size: 2rem;
        font-weight: 800;
        color: #1e293b;
        letter-spacing: -0.025em;
        line-height: 1.3;
        margin: 0;
    }
    .has-course-banner .student-doc-title {
        color: #ffffff;
        text-shadow: 0 2px 12px rgba(15,23,42,0.45);
    }
    .student-doc-meta {
        margin-top: 0.75rem;
        font-size: 0.875rem;
        color: #94a3b8;
        font-weight: 500;
        display: flex;
        justify-content: center;
        align-items: center;
        gap: 0.5rem;
    }
    .has-course-banner .student-doc-meta,
    .has-course-banner .courseother {
        color: rgba(255,255,255,0.88) !important;
        text-shadow: 0 1px 8px rgba(15,23,42,0.35);
    }
    /* Intelligent Content Rendering Rules */
    .student-doc-body {
        padding: 40px 10% 80px 10%;
        font-size: 1.05rem;
        color: #334155;
        line-height: 1.9;
    }
    @media (max-width: 640px) {
        .student-doc-body { padding: 30px 20px 60px 20px; }
        .student-doc-title { font-size: 1.5rem; }
    }
    .student-doc-body img {
        max-width: 100%;
        height: auto;
        border-radius: 12px;
        box-shadow: 0 4px 12px rgba(0,0,0,0.05);
        margin: 1.5rem 0;
    }
    .student-doc-body p { margin-bottom: 1.2rem; }
    .student-doc-body h1, .student-doc-body h2, .student-doc-body h3 {
        color: #0f172a;
        font-weight: 700;
        margin-top: 2rem;
        margin-bottom: 1rem;
    }
    .student-doc-body ul, .student-doc-body ol {
        margin-left: 2rem;
        margin-bottom: 1.5rem;
    }
    .student-doc-body pre {
        background: #f8fafc;
        padding: 1rem;
        border-radius: 12px;
        border: 1px solid #e2e8f0;
        overflow-x: auto;
        font-family: monospace;
    }
</style>

<div class="w-full max-w-[900px] mx-auto">
    <div class="student-doc-wrap">
        <!-- Course Title Header -->
        <div class="student-doc-head course-node-head">
            <asp:Label ID="LabelCtitle" runat="server" CssClass="student-doc-title course-node-title"></asp:Label>
            <div class="student-doc-meta">
                <i class="bi bi-journal-text"></i> 学案正文浏览
            </div>
            <div class="courseother mt-2 text-center text-sm text-slate-500">	
            </div>
        </div>
        
        <!-- Rich Text HTML Injection Area -->
        <div id="Ccontent" class="student-doc-body" style="word-wrap:break-word; word-break:break-all;" runat="server">   
        </div>
    </div>
</div>
</asp:Content>
