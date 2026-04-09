<%@ Page Validaterequest="false" Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher"  AutoEventWireup="true" CodeFile="courseedit.aspx.cs" Inherits="Teacher_courseedit" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link rel="stylesheet" type="text/css" href="/App_Themes/Teacher/courseshow.css" />
    

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
