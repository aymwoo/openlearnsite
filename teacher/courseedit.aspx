<%@ Page Validaterequest="false" Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher"  AutoEventWireup="true" CodeFile="courseedit.aspx.cs" Inherits="Teacher_courseedit" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link rel="stylesheet" type="text/css" href="/App_Themes/Teacher/courseshow.css" />
    <style type="text/css">
        .activity-plan-draft {
            display: flex;
            flex-direction: column;
            gap: 0.85rem;
            white-space: normal;
        }

        .activity-plan-draft-meta {
            padding: 0.75rem 0.85rem;
            border-radius: 0.9rem;
            background: rgba(99, 102, 241, 0.08);
            color: #4338ca;
            font-size: 0.82rem;
            line-height: 1.6;
        }

        .activity-plan-card {
            border: 1px solid rgba(99, 102, 241, 0.14);
            border-radius: 0.95rem;
            padding: 0.9rem;
            background: #ffffff;
            box-shadow: 0 10px 24px -22px rgba(79, 70, 229, 0.55);
        }

        .activity-plan-card-head {
            display: flex;
            align-items: flex-start;
            justify-content: space-between;
            gap: 0.75rem;
            margin-bottom: 0.65rem;
        }

        .activity-plan-card-actions {
            display: flex;
            flex-wrap: wrap;
            justify-content: flex-end;
            align-items: center;
            gap: 0.45rem;
        }

        .activity-plan-card-head .activity-plan-card-title {
            margin: 0;
        }

        .activity-plan-section-action {
            border: 1px solid rgba(99, 102, 241, 0.18);
            background: rgba(99, 102, 241, 0.06);
            color: #4338ca;
            border-radius: 999px;
            padding: 0.35rem 0.75rem;
            font-size: 0.78rem;
            font-weight: 600;
            cursor: pointer;
        }

        .activity-plan-section-action[disabled] {
            cursor: not-allowed;
            opacity: 0.7;
        }

        .activity-plan-section-select {
            display: inline-flex;
            align-items: center;
            gap: 0.35rem;
            padding: 0.3rem 0.65rem;
            border-radius: 999px;
            background: rgba(15, 23, 42, 0.04);
            color: #334155;
            font-size: 0.78rem;
            font-weight: 600;
        }

        .activity-plan-section-select input {
            margin: 0;
        }

        .activity-plan-section-status {
            margin-top: 0.65rem;
            font-size: 0.78rem;
            line-height: 1.5;
        }

        .activity-plan-section-status.is-loading {
            color: #4338ca;
        }

        .activity-plan-section-status.is-error {
            color: #b91c1c;
        }

        .activity-plan-card-title {
            margin: 0 0 0.65rem;
            font-size: 0.95rem;
            font-weight: 700;
            color: #312e81;
        }

        .activity-plan-list {
            margin: 0;
            padding-left: 1.1rem;
            color: #334155;
        }

        .activity-plan-list li + li,
        .activity-plan-step + .activity-plan-step {
            margin-top: 0.6rem;
        }

        .activity-plan-step {
            padding: 0.8rem;
            border-radius: 0.85rem;
            background: linear-gradient(180deg, #fafbfd 0%, #f8f7ff 100%);
            border: 1px solid rgba(99, 102, 241, 0.1);
        }

        .activity-plan-step-head {
            display: flex;
            justify-content: space-between;
            gap: 0.75rem;
            align-items: center;
            margin-bottom: 0.65rem;
        }

        .activity-plan-step-title {
            font-weight: 700;
            color: #1e293b;
        }

        .activity-plan-step-minutes {
            color: #5b21b6;
            font-size: 0.82rem;
            font-weight: 600;
            white-space: nowrap;
        }

        .activity-plan-step-grid {
            display: grid;
            gap: 0.55rem;
        }

        .activity-plan-field-label {
            display: block;
            margin-bottom: 0.15rem;
            color: #6366f1;
            font-size: 0.78rem;
            font-weight: 700;
        }

        .activity-plan-field-value {
            color: #334155;
            line-height: 1.65;
        }

        .activity-plan-draft-banner {
            display: none;
            margin-bottom: 0.85rem;
            padding: 0.85rem 0.95rem;
            border-radius: 0.9rem;
            border: 1px solid rgba(59, 130, 246, 0.16);
            background: rgba(239, 246, 255, 0.95);
            color: #1d4ed8;
        }

        .activity-plan-draft-banner-title {
            margin: 0 0 0.25rem;
            font-size: 0.88rem;
            font-weight: 700;
        }

        .activity-plan-draft-banner-text {
            margin: 0;
            font-size: 0.8rem;
            line-height: 1.6;
            color: #1e40af;
        }

        .activity-plan-draft-banner-actions {
            display: flex;
            flex-wrap: wrap;
            gap: 0.55rem;
            margin-top: 0.7rem;
        }

        .activity-plan-footer-actions {
            display: flex;
            flex-wrap: wrap;
            gap: 0.55rem;
        }
    </style>
    

    <div class="course-edit-page">
        <div class="course-edit-shell" id="EditShell">
            <section id="EditHeroSection" class="course-edit-hero">
                <div class="course-edit-hero-content">
                    <h1 class="course-edit-title">学案编辑</h1>
                </div>
                <div class="course-show-hero-actions course-edit-hero-actions">
                    <a id="HeroEditLink" runat="server" class="course-show-hero-edit" title="编辑横幅" aria-label="编辑横幅">
                        <svg viewBox="0 0 24 24" aria-hidden="true"><path d="M12 5H7a2 2 0 0 0-2 2v10a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2v-5"></path><path d="M16.5 4.5a2.12 2.12 0 1 1 3 3L12 15l-4 1 1-4 7.5-7.5z"></path></svg>
                    </a>
                </div>
            </section>

            <section class="course-edit-panel">
                <div class="course-edit-grid">
                    <div class="course-edit-field course-edit-field-wide">
                        <label class="course-edit-label" for="<%= Texttitle.ClientID %>">学案名称</label>
                        <asp:TextBox ID="Texttitle" runat="server" SkinID="TextBoxNormal" CssClass="course-edit-input"></asp:TextBox>
                    </div>

                    <div class="course-edit-field">
                        <label class="course-edit-label" for="<%= DDLclass.ClientID %>">学案分类</label>
                        <asp:DropDownList ID="DDLclass" runat="server" Font-Size="9pt" CssClass="course-edit-select"></asp:DropDownList>
                    </div>

                    <div class="course-edit-field">
                        <label class="course-edit-label" for="<%= DDLcobj.ClientID %>">授课年级</label>
                        <asp:DropDownList ID="DDLcobj" runat="server" Font-Size="9pt" CssClass="course-edit-select"></asp:DropDownList>
                    </div>

                    <div class="course-edit-field">
                        <label class="course-edit-label" for="<%= DDLCterm.ClientID %>">学期设置</label>
                        <div class="course-edit-static">
                            第&nbsp;<asp:DropDownList ID="DDLCterm" runat="server" Font-Names="Arial" Font-Size="8pt" CssClass="course-edit-select">
                                <asp:ListItem>1</asp:ListItem>
                                <asp:ListItem Selected="True">2</asp:ListItem>
                            </asp:DropDownList>
                            学期
                        </div>
                    </div>

                    <div class="course-edit-field">
                        <label class="course-edit-label" for="<%= DDLCks.ClientID %>">课节</label>
                        <div class="course-edit-static">
                            第&nbsp;<asp:DropDownList ID="DDLCks" runat="server" Font-Size="8pt" Font-Names="Arial" CssClass="course-edit-select"></asp:DropDownList>
                            课节
                        </div>
                    </div>

                    <div class="course-edit-field">
                        <span class="course-edit-label">发布设置</span>
                        <label class="course-edit-publish" for="<%= CheckPublish.ClientID %>">
                            <asp:CheckBox ID="CheckPublish" runat="server" Text="是否发布" Checked="True" />
                        </label>
                    </div>
                    <asp:HiddenField ID="HiddenBannerUrl" runat="server" />
                    <asp:HiddenField ID="HiddenCourseId" runat="server" Value='<%= myCid() %>' />
                </div>
            </section>

            <section class="course-edit-editor-panel">
                <link href="../js/vendors/wangeditor/style.css" rel="stylesheet">
                <link rel="stylesheet" href="../js/vendors/vditor/index.css" />
                <script src="../js/vendors/vditor/index.min.js"></script>
                <script src="../js/vendors/wangeditor/index.js"></script>

                <div class="course-edit-editor-toolbar">
                    <div>
                        <h2 class="course-edit-section-title">内容编辑</h2>
                        <p class="course-edit-section-desc">支持 KindEditor、WangEditor 和 Vditor 三种模式切换。</p>
                    </div>
                    <div>
                        <label class="course-edit-label">编辑器</label>
                        <select id="editorSelector" onchange="switchEditor(this.value)" class="course-edit-editor-select">
                            <option value="kindeditor" selected>KindEditor（原生）</option>
                            <option value="wangeditor">WangEditor（富文本）</option>
                            <option value="vditor">Vditor（Markdown）</option>
                        </select>
                    </div>
                </div>

                <script charset="utf-8" src="../kindeditor/kindeditor-min.js"></script>
                <script charset="utf-8" src="../kindeditor/lang/zh_CN.js"></script>
                <script src="../teacher/editor-upload-helper.js" type="text/javascript"></script>
                

                <div class="editor-ai-layout course-edit-ai-layout">
                    <div class="editor-container course-edit-main-editor">
                        <div class="course-edit-editor-stage custom-scrollbar">
                            <div id="wangeditor-wrap" style="display:none; width:100%; position:relative; border: 1px solid #ccc; z-index: 100;">
                                <div id="wangeditor-toolbar" style="border-bottom: 1px solid #ccc;"></div>
                                <div id="wangeditor-text" style="height: 350px;"></div>
                            </div>

                            <div id="vditor-wrap" style="display:none; width:100%; position:relative; margin-bottom: 10px;">
                                <div id="vditor-container"></div>
                            </div>

                            <textarea id="mcontent" runat="server" style="width:100%; height:400px;"></textarea>
                        </div>
                    </div>

                    <aside class="ai-assistant-panel courseedit-plan-panel" id="courseedit-plan-panel">
                        <div class="ai-panel-header">
                            <span class="ai-panel-icon" aria-hidden="true">
                                <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M12 3l1.8 3.6L18 8.4l-3 2.9.7 4.1-3.7-1.9-3.7 1.9.7-4.1-3-2.9 4.2-.8L12 3z"></path></svg>
                            </span>
                            <div class="ai-panel-heading">
                                <strong class="ai-panel-title">活动计划助手</strong>
                                <p class="ai-panel-desc">围绕当前主题快速生成课堂活动计划，保留学案编辑为主、计划辅助为辅。</p>
                            </div>
                        </div>
                        <div class="ai-panel-body">
                            <div class="ai-panel-tip">
                                <span class="ai-panel-tip-badge">使用建议</span>
                                <p class="ai-panel-tip-text">先输入主题或知识点，再按需补充年级、课时和教学目标。当前学案内容会自动作为支持背景发送。</p>
                            </div>
                            <div id="activity-plan-draft-banner" class="activity-plan-draft-banner" aria-live="polite">
                                <p class="activity-plan-draft-banner-title">继续上次草案</p>
                                <p id="activity-plan-draft-banner-text" class="activity-plan-draft-banner-text">当前课程存在已保存的活动计划草案，可选择继续编辑或重新开始。</p>
                                <div class="activity-plan-draft-banner-actions">
                                    <button type="button" id="activity-plan-resume-btn" class="ai-action-btn" onclick="resumeSavedActivityPlanDraft()">继续上次草案</button>
                                </div>
                            </div>
                            <div class="ai-panel-group">
                                <label class="ai-panel-label" for="activity-plan-topic">主题 / 知识点 <span style="color:#dc2626;">*</span></label>
                                <textarea id="activity-plan-topic" class="ai-prompt-input" placeholder="例如：分数的初步认识"></textarea>
                            </div>
                            <div class="ai-panel-group">
                                <button type="button" id="activity-plan-toggle" class="ai-action-btn" onclick="toggleActivityPlanFields()">展开可选信息</button>
                            </div>
                            <div id="activity-plan-fields" class="ai-panel-group" style="display:none;">
                                <label class="ai-panel-label" for="activity-plan-grade">授课年级</label>
                                <input id="activity-plan-grade" type="text" class="course-edit-input" placeholder="默认读取当前学案年级，可手动覆盖" />
                                <label class="ai-panel-label" for="activity-plan-duration" style="margin-top:10px;">课时/时长</label>
                                <input id="activity-plan-duration" type="text" class="course-edit-input" placeholder="例如：1课时 / 40分钟" />
                                <label class="ai-panel-label" for="activity-plan-goals" style="margin-top:10px;">教学目标</label>
                                <textarea id="activity-plan-goals" class="ai-prompt-input" placeholder="例如：理解分数含义，能结合情境表达分数"></textarea>
                            </div>
                            <button type="button" id="activity-plan-generate-btn" class="ai-generate-btn" onclick="generateActivityPlan()">
                                <div id="activity-plan-loading" class="ai-loading-spinner"></div>
                                <span id="activity-plan-btn-text">生成活动计划</span>
                            </button>
                            <div id="activity-plan-progress-wrap" class="ai-progress-wrap">
                                <div class="ai-progress-header">
                                    <span id="activity-plan-progress-text" class="ai-progress-text">准备生成</span>
                                    <span id="activity-plan-progress-percent" class="ai-progress-percent">0%</span>
                                </div>
                                <div class="ai-progress-track">
                                    <div id="activity-plan-progress-bar" class="ai-progress-bar"></div>
                                </div>
                                <div id="activity-plan-progress-note" class="ai-progress-note">输入主题后，系统会调用默认 AI Provider 生成活动计划。</div>
                            </div>
                            <div class="ai-panel-group">
                                <label class="ai-panel-label" for="activity-plan-result">计划草案预览</label>
                                <div id="activity-plan-result" class="ai-result-area" aria-live="polite"></div>
                            </div>
                        </div>
                        <div class="ai-panel-footer">
                            <div class="activity-plan-footer-actions">
                                <button type="button" class="ai-action-btn" onclick="copyActivityPlanResult()">复制草案</button>
                                <button type="button" id="activity-plan-save-draft-btn" class="ai-action-btn" onclick="saveCurrentActivityPlanDraft()">保存草案</button>
                                <button type="button" id="activity-plan-apply-selected-btn" class="ai-action-btn" onclick="applySelectedActivityPlanSections()">应用所选章节</button>
                            </div>
                        </div>
                    </aside>
                </div>
            </section>

            <section class="course-edit-feedback">
                <asp:Label ID="Labelmsg" runat="server" CssClass="course-edit-msg"></asp:Label>
            </section>

            <section class="course-edit-actions">
                <asp:Button ID="Btnedit" runat="server" Text="保存学案" onclick="Btnedit_Click" OnClientClick="return syncContent();" CssClass="course-edit-primary-btn" />
                <asp:Button ID="Btnreturn" runat="server" Text="返回列表" onclick="Btnreturn_Click" CssClass="course-edit-secondary-btn" />
            </section>

            <div class="course-edit-hidden" aria-hidden="true">
                <asp:HyperLink ID="HLbanner" runat="server" Target="_blank" CssClass="course-edit-banner-link">查看横幅</asp:HyperLink>
            </div>
        </div>
    </div>

    <div id="BannerModal" class="course-show-banner-modal" aria-hidden="true">
        <div class="course-show-banner-dialog" role="dialog" aria-modal="true" aria-labelledby="BannerModalTitle">
            <div class="course-show-banner-dialog-head">
                <div>
                    <h2 id="BannerModalTitle" class="course-show-banner-dialog-title">编辑课程横幅</h2>
                    <p class="course-show-banner-dialog-desc">拖入图片或从本地选择新封面，预览确认后直接更新当前学案横幅。</p>
                </div>
                <button id="BannerModalClose" type="button" class="course-show-banner-close" aria-label="关闭弹窗">
                    <svg viewBox="0 0 24 24" aria-hidden="true"><path d="M6 6l12 12"></path><path d="M18 6l-12 12"></path></svg>
                </button>
            </div>
            <div class="course-show-banner-dialog-body">
                <div class="course-show-banner-upload">
                    <div id="BannerDropzone" class="course-show-banner-dropzone">
                        <svg viewBox="0 0 24 24" aria-hidden="true"><path d="M12 16V7"></path><path d="M8.5 10.5L12 7l3.5 3.5"></path><path d="M5 17v1a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2v-1"></path><rect x="3" y="3" width="18" height="18" rx="3"></rect></svg>
                        <p class="course-show-banner-drop-title">拖动图片到这里上传横幅</p>
                        <p class="course-show-banner-drop-desc">支持 png、jpg、jpeg、gif、webp，建议使用横向大图，大小不超过 5MB。</p>
                        <label for="BannerFileInput" class="course-show-banner-browse">选择图片</label>
                        <input id="BannerFileInput" type="file" class="course-show-banner-file" accept="image/png,image/jpeg,image/jpg,image/gif,image/webp" />
                    </div>
                    <div class="course-show-banner-meta">
                        <span class="course-show-banner-chip">拖拽上传</span>
                        <span class="course-show-banner-chip">粘贴图片</span>
                        <span class="course-show-banner-chip">实时预览</span>
                        <span class="course-show-banner-chip">无刷新保存</span>
                    </div>
                    <div id="BannerUploadProgress" class="course-show-banner-progress" hidden="hidden">
                        <div id="BannerUploadProgressBar" class="course-show-banner-progress-bar"></div>
                    </div>
                    <div id="BannerUploadStatus" class="course-show-banner-status"></div>
                </div>
                <div class="course-show-banner-preview-card">
                    <div class="course-show-banner-preview-head">
                        <span id="BannerCurrentState" class="course-show-banner-current">当前封面：默认样式</span>
                        <p class="course-show-banner-preview-title">封面预览</p>
                        <p class="course-show-banner-preview-desc">保存后将立即替换当前编辑页背景横幅，并保持当前页面停留。</p>
                    </div>
                    <div id="BannerPreviewStage" class="course-show-banner-stage">
                        <div id="BannerPreviewEmpty" class="course-show-banner-stage-empty">当前还没有选择新的横幅图片</div>
                        <div class="course-show-banner-stage-copy">
                            <strong><asp:Literal ID="LiteralBannerPreviewTitle" runat="server">学案横幅预览</asp:Literal></strong>
                            <span>新的学案横幅将应用到当前编辑页背景</span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="course-show-banner-dialog-foot">
                <div class="course-show-banner-foot-note">上传成功后会立即更新学案横幅，并同步刷新当前编辑页背景。</div>
                <div class="course-show-banner-actions">
                    <button id="BannerModalCancel" type="button" class="course-show-banner-btn">取消</button>
                    <button id="BannerUploadButton" type="button" class="course-show-banner-btn primary">保存横幅</button>
                </div>
            </div>
        </div>
    </div>

    
    <script type="text/javascript">
        window.__courseeditConfig = {
            myCid: '<%=myCid() %>',
            mcontentId: '<%= mcontent.ClientID %>',
            gradeId: '<%= DDLcobj.ClientID %>',
            hiddenBannerUrlId: '<%= HiddenBannerUrl.ClientID %>',
            hLbannerId: '<%= HLbanner.ClientID %>',
            hiddenCourseId: '<%= HiddenCourseId.ClientID %>',
            heroEditLinkId: '<%= HeroEditLink.ClientID %>',
            shellId: 'EditShell'
        };
    </script>
    <script type="text/javascript" src="/js/course-banner-modal.js"></script>
    <script type="text/javascript" src="../js/courseedit.js"></script>
</asp:Content>
