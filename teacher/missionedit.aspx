<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" Validaterequest="false" AutoEventWireup="true" CodeFile="missionedit.aspx.cs" Inherits="Teacher_missionedit" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/course-content-add.css" rel="stylesheet" />
    <link href="../App_Themes/Teacher/missionedit.css" rel="stylesheet" />

    <div class="content-add-page mission-add-page missionedit-page">
        <div class="content-add-shell is-medium">
            <section class="content-add-hero">
                <div class="content-add-hero-content">
                    <span class="content-add-eyebrow">Edit Learning Activity</span>
                    <h1 class="content-add-title">编辑学习活动</h1>
                    <p class="content-add-subtitle">保持现有活动标题、提交方式、分组协作、远程图片和评价量规逻辑不变，同时兼容 KindEditor、WangEditor 和 Vditor 的稳定保存。</p>
                </div>
            </section>

            <section class="content-add-panel">
                <h2 class="content-add-section-title">活动设置</h2>
                <p class="content-add-section-desc">修改后将更新当前活动内容，并返回活动详情页查看效果。</p>
                <div class="content-add-grid">
                    <div class="content-add-field content-add-field-title">
                        <label class="content-add-label" for="<%= Texttitle.ClientID %>">活动名称</label>
                        <asp:TextBox ID="Texttitle" runat="server" SkinID="TextBoxNormal" CssClass="content-add-input"></asp:TextBox>
                    </div>

                    <div class="content-add-field content-add-field-type">
                        <label class="content-add-label" for="<%= DDLmfiletype.ClientID %>">作品类型</label>
                        <asp:DropDownList ID="DDLmfiletype" runat="server" Font-Names="Arial" CssClass="content-add-select content-add-select-worktype"></asp:DropDownList>
                    </div>

                    <div class="content-add-field content-add-field-rubric">
                        <label class="content-add-label" for="<%= DDLMgid.ClientID %>">评价标准</label>
                        <asp:DropDownList ID="DDLMgid" runat="server" Font-Size="9pt" Font-Names="Arial" CssClass="content-add-select"></asp:DropDownList>
                    </div>

                    <div class="content-add-field content-add-field-options">
                        <span class="content-add-label">活动选项</span>
                        <div class="content-add-checks">
                            <asp:CheckBox ID="CheckUpload" runat="server" Text="是否提交" Checked="True" />
                            <asp:CheckBox ID="CheckPublish" runat="server" Text="是否发布" Checked="True" />
                            <asp:CheckBox ID="CheckGroup" runat="server" Text="小组合作" />
                            <asp:CheckBox ID="CheckRemote" runat="server" Text="远程图片" ToolTip="自动下载远程图片，有时失效！" />
                            <asp:CheckBox ID="CheckMicoWorld" runat="server" Text="上次作品" Checked="False" ToolTip="显示上一节课作品提供下载，适合项目学习连续制作" />
                        </div>
                    </div>
                </div>
            </section>

            <section class="content-add-editor">
                <link href="../js/vendors/wangeditor/style.css" rel="stylesheet">
                <link rel="stylesheet" href="../js/vendors/vditor/index.css" />
                <script src="../js/vendors/vditor/index.min.js"></script>
                <script src="../js/vendors/wangeditor/index.js"></script>

                <div class="content-add-editor-toolbar">
                    <div>
                        <h2 class="content-add-section-title">活动说明</h2>
                        <p class="content-add-section-desc">支持 KindEditor、WangEditor 和 Vditor 三种编辑方式切换，保存时会自动同步当前编辑器内容。</p>
                    </div>
                    <div class="mission-add-toolbar-actions">
                        <label class="content-add-label" for="editorSelector">编辑器选择</label><br />
                        <select id="editorSelector" onchange="switchEditor(this.value)" class="content-add-editor-select">
                            <option value="kindeditor" selected>原生编辑器 (KindEditor)</option>
                            <option value="wangeditor">富文本编辑器 (WangEditor)</option>
                            <option value="vditor">Markdown编辑器 (Vditor)</option>
                        </select>
                        <div id="vditorPasteControls" class="mission-add-paste-controls" style="display:none;">
                            <span class="mission-add-paste-label">Vditor 粘贴</span>
                            <label class="mission-add-paste-option"><input type="radio" name="vditorPasteMode" value="keep" checked="checked" /> 保持原样</label>
                            <label class="mission-add-paste-option"><input type="radio" name="vditorPasteMode" value="plain" /> 清理格式</label>
                            <button type="button" class="mission-add-paste-btn" onclick="pastePlainTextToVditor()">粘贴纯文本</button>
                        </div>
                    </div>
                </div>

                <script charset="utf-8" src="../kindeditor/kindeditor-min.js"></script>
                <script charset="utf-8" src="../kindeditor/lang/zh_CN.js"></script>
                <script src="../teacher/editor-upload-helper.js" type="text/javascript"></script>

                <div class="missionedit-editor-wrap">
                    <div class="content-add-editor-stage mission-add-editor-stage custom-scrollbar">
                        <div id="wangeditor-wrap" style="display:none; width: 100%; position:relative; border: 1px solid #ccc; z-index: 100;">
                            <div id="wangeditor-toolbar" style="border-bottom: 1px solid #ccc;"></div>
                            <div id="wangeditor-text" style="height: 350px;"></div>
                        </div>

                        <div id="vditor-wrap" style="display:none; width: 100%; position:relative; margin-bottom: 10px;">
                            <div id="vditor-container"></div>
                        </div>

                        <textarea id="mcontent" runat="server" style="width: 100%; height:550px; box-sizing:border-box;"></textarea>
                        <input type="hidden" id="editorContentPayload" name="editorContentPayload" />
                        <input type="hidden" id="editorSyncSource" name="editorSyncSource" />
                        <input type="hidden" id="editorSyncLength" name="editorSyncLength" />
                    </div>
                </div>
            </section>

            <section class="content-add-feedback">
                <h2 class="content-add-section-title">保存反馈</h2>
                <p class="content-add-section-desc">如果标题或活动说明为空，将在这里显示明确提示。</p>
                <asp:Label ID="Labelmsg" runat="server" CssClass="missionedit-message"></asp:Label>
            </section>

            <section class="content-add-actions">
                <asp:Button ID="Btnedit" runat="server" Text="修改活动" OnClick="Btnedit_Click" OnClientClick="return syncContent();" CssClass="content-add-primary" />
                <asp:Button ID="BtnCourse" runat="server" Text="返回学案" OnClick="BtnCourse_Click" CssClass="content-add-secondary" />
            </section>
        </div>
    </div>
    <script type="text/javascript">
        window.__missioneditConfig = {
            myCid: '<%=myCid() %>',
            mcontentId: '<%= mcontent.ClientID %>'
        };
    </script>
    <script type="text/javascript" src="../js/missionedit.js"></script>
    <script type="text/javascript">
        (function () {
            var form = document.forms[0];
            if (form && !form.getAttribute('data-missionedit-sync-bound')) {
                form.setAttribute('data-missionedit-sync-bound', '1');
                form.addEventListener('submit', function () {
                    if (typeof syncContent === 'function') {
                        syncContent();
                    }
                });
            }
        })();
    </script>
</asp:Content>
