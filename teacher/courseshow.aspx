<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="courseshow.aspx.cs" Inherits="Teacher_courseshow" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <script src="../markdown/lib/marked.min.js"></script>
    <link rel="stylesheet" href="../js/vendors/reveal/dist/reveal.css" />
    <link rel="stylesheet" href="../js/vendors/reveal/dist/theme/white.css" />
    <link rel="stylesheet" href="../js/vendors/highlight/github.min.css" />
    <script src="../webform/highlight.min.js"></script>
    <link href="../App_Themes/Teacher/content-show-markdown.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="/App_Themes/Teacher/courseshow.css" />

    <div class="course-show-page course-page">
        <div class="course-show-shell course-shell">
            <section id="HeroSection" runat="server" class="course-show-hero course-hero">
                <div class="course-show-hero-overlay" aria-hidden="true"></div>
                <div class="course-show-hero-content">
                    <div class="course-show-hero-header">
                        <div class="course-show-hero-top">
                            <div class="course-show-hero-copy">
                                <div class="course-show-hero-nav">
                                    <asp:LinkButton ID="LinkBtnReturn" runat="server" OnClick="LinkBtnReturn_Click" CssClass="course-show-back-link" title="返回列表"><svg viewBox="0 0 24 24" aria-hidden="true"><path d="M19 12H5"></path><path d="M12 19l-7-7 7-7"></path></svg>返回列表</asp:LinkButton>
                                    <span class="course-show-hero-kicker">学案详情</span>
                                </div>
                                <div class="course-show-title-wrap">
                                    <asp:Label ID="LabelCtitle" runat="server" CssClass="course-show-title course-title"></asp:Label>
                                </div>
                                <div class="course-show-meta">
                                    <span class="course-show-meta-chip">
                                        <span class="course-show-meta-chip-icon"><svg viewBox="0 0 24 24" aria-hidden="true"><rect x="3" y="4" width="18" height="18" rx="2"></rect><path d="M16 2v4"></path><path d="M8 2v4"></path><path d="M3 10h18"></path></svg></span>
                                        <span class="course-show-meta-chip-label">日期</span>
                                        <span class="course-show-meta-chip-value"><asp:Label ID="LabelCdate" runat="server"></asp:Label></span>
                                    </span>
                                    <span class="course-show-meta-chip">
                                        <span class="course-show-meta-chip-icon"><svg viewBox="0 0 24 24" aria-hidden="true"><path d="M4 20h16a2 2 0 0 0 2-2V8a2 2 0 0 0-2-2h-7.93a2 2 0 0 1-1.66-.9l-.82-1.2A2 2 0 0 0 7.93 3H4a2 2 0 0 0-2 2v13c0 1.1.9 2 2 2z"></path></svg></span>
                                        <span class="course-show-meta-chip-label">分类</span>
                                        <span class="course-show-meta-chip-value"><asp:Label ID="LabelCclass" runat="server"></asp:Label></span>
                                    </span>
                                    <span class="course-show-meta-chip">
                                        <span class="course-show-meta-chip-icon"><svg viewBox="0 0 24 24" aria-hidden="true"><path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"></path><circle cx="9" cy="7" r="4"></circle></svg></span>
                                        <span class="course-show-meta-chip-label">年级</span>
                                        <span class="course-show-meta-chip-value"><asp:Label ID="LabelCobj" runat="server"></asp:Label></span>
                                    </span>
                                    <span class="course-show-meta-chip">
                                        <span class="course-show-meta-chip-icon"><svg viewBox="0 0 24 24" aria-hidden="true"><circle cx="12" cy="12" r="10"></circle><path d="M12 6v6l4 2"></path></svg></span>
                                        <span class="course-show-meta-chip-label">学期</span>
                                        <span class="course-show-meta-chip-value">第 <asp:Label ID="LabelCterm" runat="server"></asp:Label> 学期</span>
                                    </span>
                                    <span class="course-show-meta-chip">
                                        <span class="course-show-meta-chip-icon"><svg viewBox="0 0 24 24" aria-hidden="true"><path d="M2 3h6a4 4 0 0 1 4 4v14a3 3 0 0 0-3-3H2z"></path><path d="M22 3h-6a4 4 0 0 0-4 4v14a3 3 0 0 1 3-3h7z"></path></svg></span>
                                        <span class="course-show-meta-chip-label">课节</span>
                                        <span class="course-show-meta-chip-value">第 <asp:Label ID="LabelCks" runat="server"></asp:Label> 课</span>
                                    </span>
                                </div>
                                <span id="ReadonlyNote" class="course-show-tool-readonly-note" runat="server" hidden="hidden">当前为旧版学案视图，部分新增与编辑入口已按原逻辑禁用</span>
                            </div>
                            <div class="course-show-hero-actions">
                                <a id="HeroEditLink" runat="server" class="course-show-hero-edit" title="编辑横幅" aria-label="编辑横幅">
                                    <svg viewBox="0 0 24 24" aria-hidden="true"><path d="M12 5H7a2 2 0 0 0-2 2v10a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2v-5"></path><path d="M16.5 4.5a2.12 2.12 0 1 1 3 3L12 15l-4 1 1-4 7.5-7.5z"></path></svg>
                                </a>
                                <asp:Button ID="BtnEdit" runat="server" Text="编辑学案" ToolTip="点击修改" OnClick="BtnEdit_Click" CssClass="course-show-hidden course-show-edit-btn" />
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            <section class="course-show-preview-panel course-table-panel">
                <div class="course-show-preview-head">
                    <div>
                        <h2 class="course-show-preview-title course-show-section-title">学案预览</h2>
                        <p class="course-show-preview-desc course-show-section-desc">下方内容继续沿用原有学案正文输出，只优化首屏阅读层次。</p>
                    </div>
                </div>
                <div id="Ccontent" class="course-show-content course-show-preview-content" runat="server"></div>
            </section>
            <div class="course-show-hidden" aria-hidden="true">
                <asp:Image ID="Imagebanner" runat="server" ToolTip="横幅图片" />
                <span id="BannerEmpty" runat="server">当前学案未设置横幅图片</span>
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
                                <p class="course-show-banner-preview-desc">保存后将立即替换顶部课程封面，并保持当前页面停留。</p>
                            </div>
                            <div id="BannerPreviewStage" class="course-show-banner-stage">
                                <div id="BannerPreviewEmpty" class="course-show-banner-stage-empty">当前还没有选择新的横幅图片</div>
                                <div class="course-show-banner-stage-copy">
                                    <strong><asp:Label ID="LabelBannerPreviewTitle" runat="server"></asp:Label></strong>
                                    <span>新的课程横幅将应用到当前学案首页封面</span>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="course-show-banner-dialog-foot">
                        <div class="course-show-banner-foot-note">上传成功后会立即更新课程横幅，并同步刷新当前页面顶部封面背景。</div>
                        <div class="course-show-banner-actions">
                            <button id="BannerModalCancel" type="button" class="course-show-banner-btn">取消</button>
                            <button id="BannerUploadButton" type="button" class="course-show-banner-btn primary">保存横幅</button>
                        </div>
                    </div>
                </div>
            </div>

            <section class="course-show-tools course-table-panel">
                <div class="course-show-tools-head">
                    <div>
                        <h2 class="course-show-section-title">添加课堂内容</h2>
                        <p class="course-show-section-desc">所有入口仍然对应原有新增页面与跳转参数，仅升级为图标化操作卡片。</p>
                    </div>
                </div>

                <div class="course-show-tool-grid">
                    <asp:LinkButton ID="LinkBtnAdd" runat="server" OnClick="LinkBtnAdd_Click" CssClass="course-show-tool course-show-tool-mission" title="学习活动">
                        <span class="course-show-tool-icon"><svg viewBox="0 0 24 24" aria-hidden="true"><path d="M12 3l7 4v5c0 5-3.5 7.5-7 9-3.5-1.5-7-4-7-9V7l7-4z"></path><path d="M9.5 12l1.8 1.8 3.7-4.3"></path></svg></span>
                        <span class="course-show-tool-copy"><span class="course-show-tool-title">添加活动</span><span class="course-show-tool-subtitle">学习活动 / 任务</span></span>
                    </asp:LinkButton>
                    <asp:LinkButton ID="LinkBtnAddTopic" runat="server" OnClick="LinkBtnAddTopic_Click" CssClass="course-show-tool course-show-tool-topic" title="课堂讨论板">
                        <span class="course-show-tool-icon"><svg viewBox="0 0 24 24" aria-hidden="true"><path d="M4 6.5a3.5 3.5 0 0 1 3.5-3.5h9A3.5 3.5 0 0 1 20 6.5v5A3.5 3.5 0 0 1 16.5 15H10l-4 4v-4.5A3.5 3.5 0 0 1 4 11.5z"></path><path d="M8 8h8"></path><path d="M8 11h5"></path></svg></span>
                        <span class="course-show-tool-copy"><span class="course-show-tool-title">添加讨论</span><span class="course-show-tool-subtitle">讨论区 / 互动</span></span>
                    </asp:LinkButton>
                    <asp:LinkButton ID="LinkButtonAddExam" runat="server" OnClick="LinkBtnAddExam_Click" CssClass="course-show-tool course-show-tool-exam" title="课堂测验">
                        <span class="course-show-tool-icon"><svg viewBox="0 0 24 24" aria-hidden="true"><path d="M8 3h8"></path><path d="M9 3v4"></path><path d="M15 3v4"></path><rect x="4" y="7" width="16" height="14" rx="2"></rect><path d="M8 12h8"></path><path d="M8 16h5"></path></svg></span>
                        <span class="course-show-tool-copy"><span class="course-show-tool-title">添加测验</span><span class="course-show-tool-subtitle">课堂测验 / 反馈</span></span>
                    </asp:LinkButton>
                    <asp:LinkButton ID="LinkBtnAddTxtForm" runat="server" OnClick="LinkBtnAddTxtForm_Click" CssClass="course-show-tool course-show-tool-form" title="表格填写">
                        <span class="course-show-tool-icon"><svg viewBox="0 0 24 24" aria-hidden="true"><rect x="4" y="3" width="16" height="18" rx="2"></rect><path d="M8 8h8"></path><path d="M8 12h8"></path><path d="M8 16h5"></path></svg></span>
                        <span class="course-show-tool-copy"><span class="course-show-tool-title">添加填表</span><span class="course-show-tool-subtitle">表单 / 采集</span></span>
                    </asp:LinkButton>
                    <asp:LinkButton ID="LinkBtnProgram" runat="server" OnClick="LinkBtnProgram_Click" CssClass="course-show-tool course-show-tool-block" title="Scratch积木编程">
                        <span class="course-show-tool-icon"><svg viewBox="0 0 24 24" aria-hidden="true"><path d="M8 4h4a2 2 0 0 1 2 2v1h2a2 2 0 0 1 2 2v3h-3a2 2 0 1 0 0 4h3v3a2 2 0 0 1-2 2h-4v-3H8a2 2 0 0 1-2-2v-4h3a2 2 0 1 0 0-4H6V6a2 2 0 0 1 2-2z"></path></svg></span>
                        <span class="course-show-tool-copy"><span class="course-show-tool-title">积木编程</span><span class="course-show-tool-subtitle">Scratch / Block</span></span>
                    </asp:LinkButton>
                    <asp:LinkButton ID="LinkBtnPython" runat="server" OnClick="LinkBtnPython_Click" CssClass="course-show-tool course-show-tool-python" title="在线Python编程">
                        <span class="course-show-tool-icon"><svg viewBox="0 0 24 24" aria-hidden="true"><path d="M9 4h4a3 3 0 0 1 3 3v2H9a2 2 0 0 0-2 2v2"></path><path d="M15 20h-4a3 3 0 0 1-3-3v-2h7a2 2 0 0 0 2-2v-2"></path><path d="M9 6h.01"></path><path d="M15 18h.01"></path></svg></span>
                        <span class="course-show-tool-copy"><span class="course-show-tool-title">Python编程</span><span class="course-show-tool-subtitle">代码 / 编写</span></span>
                    </asp:LinkButton>
                    <asp:LinkButton ID="LinkBtnConsole" runat="server" OnClick="LinkBtnConsole_Click" CssClass="course-show-tool course-show-tool-console" title="在线Python测评">
                        <span class="course-show-tool-icon"><svg viewBox="0 0 24 24" aria-hidden="true"><rect x="3" y="4" width="18" height="14" rx="2"></rect><path d="M7 8l3 3-3 3"></path><path d="M13 14h4"></path></svg></span>
                        <span class="course-show-tool-copy"><span class="course-show-tool-title">Python测评</span><span class="course-show-tool-subtitle">自动评测</span></span>
                    </asp:LinkButton>
                    <asp:LinkButton ID="LinkButtonGraph" runat="server" OnClick="LinkBtnGraph_Click" CssClass="course-show-tool course-show-tool-graph" title="在线流程图">
                        <span class="course-show-tool-icon"><svg viewBox="0 0 24 24" aria-hidden="true"><rect x="4" y="4" width="6" height="4" rx="1"></rect><rect x="14" y="10" width="6" height="4" rx="1"></rect><rect x="4" y="16" width="6" height="4" rx="1"></rect><path d="M10 6h4"></path><path d="M17 10V8H7v8"></path><path d="M10 18h4"></path></svg></span>
                        <span class="course-show-tool-copy"><span class="course-show-tool-title">流程图</span><span class="course-show-tool-subtitle">图形化表达</span></span>
                    </asp:LinkButton>
                    <asp:LinkButton ID="LinkButtonPixel" runat="server" OnClick="LinkButtonPixel_Click" CssClass="course-show-tool course-show-tool-pixel" title="在线应用">
                        <span class="course-show-tool-icon"><svg viewBox="0 0 24 24" aria-hidden="true"><path d="M7 4h3v3H7z"></path><path d="M11 4h3v3h-3z"></path><path d="M15 4h3v3h-3z"></path><path d="M7 8h3v3H7z"></path><path d="M11 8h3v3h-3z"></path><path d="M15 8h3v3h-3z"></path><path d="M7 12h3v3H7z"></path><path d="M11 12h3v3h-3z"></path><path d="M15 12h3v3h-3z"></path><path d="M11 16h3v3h-3z"></path></svg></span>
                        <span class="course-show-tool-copy"><span class="course-show-tool-title">主题应用</span><span class="course-show-tool-subtitle">主题实验 / 创作</span></span>
                    </asp:LinkButton>
                    <asp:LinkButton ID="LinkButtonHtml" runat="server" OnClick="LinkButtonHtml_Click" CssClass="course-show-tool course-show-tool-html" title="单网页设计">
                        <span class="course-show-tool-icon"><svg viewBox="0 0 24 24" aria-hidden="true"><path d="M8 9l-3 3 3 3"></path><path d="M16 9l3 3-3 3"></path><path d="M14 5l-4 14"></path></svg></span>
                        <span class="course-show-tool-copy"><span class="course-show-tool-title">Html网页</span><span class="course-show-tool-subtitle">网页设计</span></span>
                    </asp:LinkButton>
                    <asp:LinkButton ID="LinkButtonKm" runat="server" OnClick="LinkButtonKm_Click" CssClass="course-show-tool course-show-tool-mind" title="在线思维导图">
                        <span class="course-show-tool-icon"><svg viewBox="0 0 24 24" aria-hidden="true"><circle cx="12" cy="12" r="2.5"></circle><path d="M12 4v3"></path><path d="M12 17v3"></path><path d="M4 12h3"></path><path d="M17 12h3"></path><path d="M6.8 6.8l2.1 2.1"></path><path d="M15.1 15.1l2.1 2.1"></path><path d="M17.2 6.8l-2.1 2.1"></path><path d="M8.9 15.1l-2.1 2.1"></path></svg></span>
                        <span class="course-show-tool-copy"><span class="course-show-tool-title">思维导图</span><span class="course-show-tool-subtitle">脑图 / 梳理</span></span>
                    </asp:LinkButton>
                    <asp:LinkButton ID="LinkButtonExcel" runat="server" OnClick="LinkButtonExcel_Click" CssClass="course-show-tool course-show-tool-sheet" title="在线表格">
                        <span class="course-show-tool-icon"><svg viewBox="0 0 24 24" aria-hidden="true"><rect x="4" y="4" width="16" height="16" rx="2"></rect><path d="M9 4v16"></path><path d="M15 4v16"></path><path d="M4 9h16"></path><path d="M4 15h16"></path></svg></span>
                        <span class="course-show-tool-copy"><span class="course-show-tool-title">在线表格</span><span class="course-show-tool-subtitle">数据 / 表格</span></span>
                    </asp:LinkButton>
                    <asp:LinkButton ID="LinkButtonware" runat="server" OnClick="LinkButtonware_Click" CssClass="course-show-tool course-show-tool-ware" title="嵌入网页小课件">
                        <span class="course-show-tool-icon"><svg viewBox="0 0 24 24" aria-hidden="true"><rect x="3" y="5" width="18" height="12" rx="2"></rect><path d="M8 21h8"></path><path d="M12 17v4"></path><path d="M7 9h10"></path><path d="M7 13h6"></path></svg></span>
                        <span class="course-show-tool-copy"><span class="course-show-tool-title">网页课件</span><span class="course-show-tool-subtitle">嵌入课件</span></span>
                    </asp:LinkButton>
                </div>
            </section>

            <section class="course-show-menu-panel course-table-panel">
                <div class="course-show-menu-head">
                    <div>
                        <h2 class="course-show-section-title">导航栏目</h2>
                        <p class="course-show-section-desc">保留原有跳转、发布与删除逻辑，并新增拖拽排序能力。</p>
                    </div>
                    <div class="course-show-menu-toolbar">
                        <span class="course-show-menu-hint">可拖动左侧手柄调整顺序</span>
                        <button id="MenuSortSaveButton" type="button" class="course-show-save-btn course-primary-btn" style="display:none;">保存排序</button>
                    </div>
                </div>

                <asp:Panel ID="PanelComposedTeacherSummary" runat="server" Visible="False">
                    <div style="margin-bottom:16px;padding:14px 16px;border-radius:16px;border:1px solid #dbeafe;background:#f8fbff;">
                        <div style="font-size:14px;font-weight:700;color:#0f172a;margin-bottom:8px;">整课活动发布进度</div>
                        <asp:Literal ID="LiteralComposedTeacherSummary" runat="server"></asp:Literal>
                    </div>
                </asp:Panel>

                <asp:HiddenField ID="HiddenSortOrder" runat="server" />
                <asp:HiddenField ID="HiddenCourseId" runat="server" />
                <asp:HiddenField ID="HiddenBannerUrl" runat="server" />
                <asp:Button ID="BtnApplySort" runat="server" Text="apply sort" CssClass="course-show-hidden" OnClick="BtnApplySort_Click" />
                <div id="MenuSortStatus" class="course-show-save-status"></div>

                <div class="course-show-menu-wrap">
                    <div class="course-show-menu-header">
                        <div class="course-show-menu-col-order">顺序</div>
                        <div class="course-show-menu-col-type">类型</div>
                        <div class="course-show-menu-col-title">导航栏目</div>
                        <div class="course-show-menu-col-state">发布</div>
                        <div class="course-show-menu-col-action">操作</div>
                    </div>
                    <div id="MenuList" class="course-show-menu-list">
                        <asp:Repeater ID="RptListMenu" runat="server" OnItemDataBound="RptListMenu_ItemDataBound" OnItemCommand="RptListMenu_ItemCommand">
                            <ItemTemplate>
                                <div id="MenuRow" runat="server" class="course-show-menu-row" data-lid='<%# Eval("Lid") %>'>
                                    <div class="course-show-menu-col-order">
                                        <div class="course-show-order-wrap">
                                            <span class="course-show-drag" title="拖动排序">::</span>
                                            <span class="course-show-order-badge"><asp:Label ID="LabelLsort" runat="server" Text='<%# Eval("Lsort") %>'></asp:Label></span>
                                        </div>
                                    </div>
                                    <div class="course-show-menu-col-type">
                                        <span class="course-show-type-badge">
                                            <asp:Image ID="Image4" runat="server" ImageUrl="~/images/new_none.gif" CssClass="course-show-type-icon" />
                                            <asp:Label ID="Label4" runat="server" CssClass="course-show-type-text"></asp:Label>
                                        </span>
                                    </div>
                                    <div class="course-show-menu-col-title">
                                        <asp:HyperLink ID="HlLtitle" runat="server" NavigateUrl="" Text='<%# Eval("Ltitle") %>' CssClass="course-show-link"></asp:HyperLink>
                                    </div>
                                    <div class="course-show-menu-col-state">
                                        <asp:LinkButton ID="LinkBtnShow" runat="server" CausesValidation="false" CommandName="P" CommandArgument='<%# Eval("Lid") %>' Text='<%# Eval("lshow") %>' ToolTip="True显示，False隐藏" CssClass="course-show-state-btn"></asp:LinkButton>
                                    </div>
                                    <div class="course-show-menu-col-action">
                                        <asp:LinkButton ID="LinkBtnDel" runat="server" CausesValidation="false" CommandName="D" CommandArgument='<%# Eval("Lid") %>' Text="删除" ToolTip="请认真确定是否删除，不可恢复！" CssClass="course-show-danger-btn"></asp:LinkButton>
                                    </div>
                                    <asp:Label ID="LabelLid" runat="server" Text='<%# Eval("Lid") %>' style="display:none"></asp:Label>
                                    <asp:Label ID="LabelLxid" runat="server" Text='<%# Eval("Lxid") %>' style="display:none"></asp:Label>
                                    <asp:Label ID="LabelLtype" runat="server" Text='<%# Eval("Ltype") %>' style="display:none"></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </section>
        </div>
    </div>

    <script type="text/javascript">
        window.__courseshowIds = {
            heroEditLink: '<%= HeroEditLink.ClientID %>',
            heroSection: '<%= HeroSection.ClientID %>',
            hiddenBannerUrl: '<%= HiddenBannerUrl.ClientID %>',
            hiddenCourseId: '<%= HiddenCourseId.ClientID %>',
            hiddenSortOrder: '<%= HiddenSortOrder.ClientID %>'
        };
    </script>
    <script type="text/javascript" src="/js/course-banner-modal.js"></script>
    <script type="text/javascript" src="/js/courseshow.js"></script>
    <script type="text/javascript">
        window.__contentShowMarkdown = {
            contentId: '<%= Ccontent.ClientID %>'
        };
    </script>
    <script type="text/javascript" src="../js/content-show-markdown.js"></script>
</asp:Content>
