<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" Validaterequest="false" AutoEventWireup="true" CodeFile="missionadd.aspx.cs" Inherits="Teacher_missionadd" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    

    <div class="content-add-page mission-add-page">
        <div class="content-add-shell is-medium">
            <section class="content-add-hero">
                <div class="content-add-hero-content">
                    <span class="content-add-eyebrow">Add Learning Activity</span>
                    <h1 class="content-add-title">添加学习活动</h1>
                    <p class="content-add-subtitle">保留活动标题、作品类型、提交方式、分组协作、远程图片和评价量规等原有业务逻辑，只重构页面布局与编辑体验。</p>
                </div>
            </section>

            <section class="content-add-panel">
                <h2 class="content-add-section-title">活动设置</h2>
                <p class="content-add-section-desc">以下字段仍沿用当前后台逻辑与提交方式，创建成功后继续返回当前学案页面。</p>
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
                            <asp:CheckBox ID="CheckMicoWorld" runat="server" Text="上次作品" Checked="False" ToolTip="显示上一节课作品，适合项目学习" />
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
                        <p class="content-add-section-desc">支持 KindEditor、WangEditor 和 Vditor 三种编辑方式切换，仍通过原有 `textareaItem` 完成内容提交。</p>
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
                

                <div class="editor-ai-layout">
                    <div class="editor-container">
                        <div class="content-add-editor-stage mission-add-editor-stage custom-scrollbar">
                            <div id="wangeditor-wrap" style="display:none; width: 100%; position:relative; border: 1px solid #ccc; z-index: 100;">
                                <div id="wangeditor-toolbar" style="border-bottom: 1px solid #ccc;"></div>
                                <div id="wangeditor-text" style="height: 350px;"></div>
                            </div>

                            <div id="vditor-wrap" style="display:none; width: 100%; position:relative; margin-bottom: 10px;">
                                <div id="vditor-container"></div>
                            </div>

                            <textarea name="textareaItem"></textarea>
                            <input type="hidden" id="editorContentPayload" name="editorContentPayload" />
                        </div>
                    </div>
                    
                    <div class="ai-assistant-panel">
                        <div class="ai-panel-header">
                            <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"><path d="M12 2a2 2 0 0 1 2 2c-.11.89-.34 2.08-1.52 3.16C11.3 8.24 10.38 9 8 9a2 2 0 0 1-2-2c.11-.89.34-2.08 1.52-3.16C8.7 2.76 9.62 2 12 2Z"></path><path d="M12 10v12"></path><path d="M12 10a2 2 0 0 0-2 2c.11.89.34 2.08 1.52 3.16.89.8 1.81 1.56 4.19 1.56a2 2 0 0 0 2-2c-.11-.89-.34-2.08-1.52-3.16C15.3 10.76 14.38 10 12 10Z"></path></svg>
                            AI 教学助手
                        </div>
                        <div class="ai-panel-body">
                            <div>
                                <label style="font-size: 0.85rem; color: #64748b; margin-bottom: 0.5rem; display: block;">描述您需要的教学内容：</label>
                                <textarea id="ai-prompt" class="ai-prompt-input" placeholder="例如：帮我生成一份关于《Python条件判断》的学案，包含学习目标、示例代码和练习题。"></textarea>
                            </div>
                            <button type="button" id="ai-generate-btn" class="ai-generate-btn" onclick="generateAIContent()">
                                <div id="ai-loading" class="ai-loading-spinner"></div>
                                <span id="ai-btn-text">生成内容</span>
                            </button>
                            <div id="ai-progress-wrap" class="ai-progress-wrap">
                                <div class="ai-progress-header">
                                    <span id="ai-progress-text" class="ai-progress-text">准备生成</span>
                                    <span id="ai-progress-percent" class="ai-progress-percent">0%</span>
                                </div>
                                <div class="ai-progress-track">
                                    <div id="ai-progress-bar" class="ai-progress-bar"></div>
                                </div>
                                <div id="ai-progress-note" class="ai-progress-note">输入提示词后，系统会调用当前默认 AI Provider 生成教学内容。</div>
                            </div>
                            <div>
                                <label style="font-size: 0.85rem; color: #64748b; margin-bottom: 0.5rem; display: block;">生成结果：</label>
                                <div id="ai-result" class="ai-result-area"></div>
                            </div>
                        </div>
                        <div class="ai-panel-footer">
                            <button type="button" class="ai-action-btn" onclick="copyAIContent()">复制结果</button>
                            <button type="button" class="ai-action-btn primary" onclick="insertAIContent()">一键插入编辑器</button>
                        </div>
                    </div>
                </div>
                
                
            </section>

            <section class="content-add-actions">
                <asp:Button ID="Btnadd" runat="server" Text="添加活动" OnClick="Btnadd_Click" OnClientClick="return syncContent();" CssClass="content-add-primary" />
                <asp:Button ID="BtnCourse" runat="server" Text="返回学案" OnClick="BtnCourse_Click" CssClass="content-add-secondary" />
            </section>
        </div>
    </div>
    <div id="mission-toast" class="mission-toast" aria-live="polite">
        <span id="mission-toast-icon" class="mission-toast-icon"></span>
        <span id="mission-toast-message" class="mission-toast-message"></span>
    </div>
    <script type="text/javascript">
        window.__missionaddConfig = {
            myCid: '<%=myCid() %>'
        };
    </script>
    <script type="text/javascript" src="../js/missionadd.js"></script>
</asp:Content>
