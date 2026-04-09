<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master"  StylesheetTheme="Teacher" Validaterequest="false"  AutoEventWireup="true" CodeFile="pixeledit.aspx.cs" Inherits="Teacher_pixeledit" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/admin-form.css" rel="stylesheet" />
    <link href="../js/vendors/wangeditor/style.css" rel="stylesheet" />
    <link rel="stylesheet" href="../js/vendors/vditor/index.css" />
    

    <div class="admin-form-page pixel-edit-page">
        <div class="admin-form-shell">
            <section class="admin-form-hero">
                <div class="admin-form-hero-content">
                    <div class="admin-form-eyebrow">Edit Custom Activity</div>
                    <h1 class="admin-form-title">编辑在线活动</h1>
                    <p class="admin-form-subtitle">保留当前活动类型、保存逻辑和学生端入口，只优化页面层次、活动说明和专属配置展示，方便教师快速完成修改。</p>
                </div>
            </section>

            <section class="admin-form-panel">
                <div class="admin-form-toolbar">
                    <div>
                        <h2 class="admin-form-section-title">活动设置</h2>
                        <p class="admin-form-section-desc">当前类型由活动创建时决定。这里继续沿用原有业务字段，只调整输入布局与说明文案。</p>
                    </div>
                </div>

                <div class="admin-form-grid">
                    <div class="admin-form-field admin-form-field-wide">
                        <label class="admin-form-label" for="<%= Texttitle.ClientID %>">活动名称</label>
                        <asp:TextBox ID="Texttitle" runat="server" SkinID="TextBoxNormal" CssClass="admin-form-input"></asp:TextBox>
                    </div>

                    <div class="admin-form-field">
                        <label class="admin-form-label" for="<%= DDLTitle.ClientID %>">当前活动类型</label>
                        <asp:DropDownList ID="DDLTitle" runat="server" Font-Size="Medium" Enabled="False" AutoPostBack="True" CssClass="admin-form-select admin-form-readonly">
                            <asp:ListItem Value="11">像素画</asp:ListItem>
                            <asp:ListItem Value="36">素材库</asp:ListItem>
                            <asp:ListItem Value="37">网站设计</asp:ListItem>
                            <asp:ListItem Value="17">二维码</asp:ListItem>
                            <asp:ListItem Value="18">在线文档</asp:ListItem>
                            <asp:ListItem Value="19">演示文稿</asp:ListItem>
                            <asp:ListItem Value="20">海报设计</asp:ListItem>
                            <asp:ListItem Value="21">风格迁移</asp:ListItem>
                            <asp:ListItem Value="22">图像分类</asp:ListItem>
                            <asp:ListItem Value="23">人脸识别</asp:ListItem>
                            <asp:ListItem Value="24">物联网MQTT</asp:ListItem>
                            <asp:ListItem Value="25">手绘画布</asp:ListItem>
                            <asp:ListItem Value="26">推箱子地图</asp:ListItem>
                            <asp:ListItem Value="27">人工智能对话</asp:ListItem>
                            <asp:ListItem Value="28">语音合成</asp:ListItem>
                            <asp:ListItem Value="29">文字识别</asp:ListItem>
                            <asp:ListItem Value="30">声音分析</asp:ListItem>
                            <asp:ListItem Value="31">井字棋</asp:ListItem>
                            <asp:ListItem Value="32">手写数字识别</asp:ListItem>
                            <asp:ListItem Value="33">Markdown写作</asp:ListItem>
                            <asp:ListItem Value="34">嵌入本地网页</asp:ListItem>
                            <asp:ListItem Value="35">文生图</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="admin-form-field">
                        <label class="admin-form-label" for="<%= DDLMgid.ClientID %>">评价标准</label>
                        <asp:DropDownList ID="DDLMgid" runat="server" Font-Size="9pt" Font-Names="Arial" CssClass="admin-form-select"></asp:DropDownList>
                    </div>

                    <div class="admin-form-field">
                        <span class="admin-form-label">发布设置</span>
                        <div class="admin-form-checks">
                            <asp:CheckBox ID="CheckPublish" runat="server" Text="是否发布" Checked="True" />
                        </div>
                    </div>

                    <div class="admin-form-field admin-form-field-wide">
                        <div class="admin-form-type-note">
                            <span class="admin-form-chip" style='background:<%= GetActivityBadgeBackground() %>;color:<%= GetActivityBadgeForeground() %>;'>
                                <img src="<%= GetActivityIconUrl() %>" alt="" class="admin-form-chip-icon" />
                                <%= GetActivityDisplayName() %>
                            </span>
                            <div class="admin-form-type-copy"><%= GetActivityDescription() %></div>
                        </div>
                    </div>
                </div>
            </section>

            <section class="admin-form-list">
                <h2 class="admin-form-section-title">开展方式</h2>
                <p class="admin-form-section-desc">当前活动会在教师端统一进入说明预览页，再由学生端按照类型跳转到对应工具页面开展学习任务。</p>
                <div class="admin-form-list-items">
                    <div class="admin-form-list-item">
                        <strong>教师端预览入口</strong>
                        当前活动修改后，课程列表仍从 `pixelshow.aspx` 进入预览和再次编辑。
                    </div>
                    <div class="admin-form-list-item">
                        <strong>学生端实际入口</strong>
                        <span class="admin-form-link-inline"><%= GetStudentEntryUrl() %></span>
                    </div>
                    <div class="admin-form-list-item">
                        <strong>本页编辑重点</strong>
                        <%= GetEditFocusText() %>
                    </div>
                </div>
            </section>

            <asp:Panel ID="PanelDeviceConfig" runat="server" CssClass="admin-form-panel" Visible="False">
                <h2 class="admin-form-section-title">物联网设备设置</h2>
                <p class="admin-form-section-desc">学生端会根据这里勾选的设备项展示可用的控制或采集能力，建议只保留本课需要的设备。</p>
                <div class="admin-form-device-list">
                    <asp:CheckBoxList ID="Ckdevice" runat="server" RepeatLayout="Flow" Visible="False" RepeatDirection="Horizontal" Font-Size="Small">
                        <asp:ListItem Value="led">小灯</asp:ListItem>
                        <asp:ListItem Value="fan">风扇</asp:ListItem>
                        <asp:ListItem Value="pump">水泵</asp:ListItem>
                        <asp:ListItem Value="temperature">温度</asp:ListItem>
                        <asp:ListItem Value="humidity">湿度</asp:ListItem>
                        <asp:ListItem Value="sound">声音</asp:ListItem>
                        <asp:ListItem Value="light">亮度</asp:ListItem>
                        <asp:ListItem Value="distance">距离</asp:ListItem>
                    </asp:CheckBoxList>
                </div>
            </asp:Panel>

            <asp:Panel ID="PanelIframeConfig" runat="server" CssClass="admin-form-panel" Visible="False">
                <h2 class="admin-form-section-title">嵌入网页设置</h2>
                <p class="admin-form-section-desc">学生端会直接加载该地址开展操作。建议优先使用站内页面或可信链接，避免失效地址影响课堂使用。</p>
                <div class="admin-form-grid">
                    <div class="admin-form-field admin-form-field-wide">
                        <label class="admin-form-label" for="<%= Texturl.ClientID %>">嵌入地址</label>
                        <asp:TextBox ID="Texturl" runat="server" SkinID="TextBoxNormal" Visible="False" CssClass="admin-form-input">https://image.baidu.com</asp:TextBox>
                        <p class="admin-form-hint">当前地址会保存到活动扩展参数中，学生打开活动时直接加载。</p>
                    </div>
                </div>
            </asp:Panel>

            <section class="admin-form-panel">
                <div class="admin-form-editor-toolbar">
                    <div>
                        <h2 class="admin-form-section-title">活动说明</h2>
                        <p class="admin-form-section-desc">支持 KindEditor、WangEditor 和 Vditor 三种编辑方式切换，提交前会自动同步到原有 `mcontent` 字段。</p>
                    </div>
                    <div>
                        <label class="admin-form-label" for="editorSelector">编辑器</label><br />
                        <select id="editorSelector" onchange="switchEditor(this.value)" class="admin-form-editor-select">
                            <option value="kindeditor" selected>原生编辑器 (KindEditor)</option>
                            <option value="wangeditor">富文本编辑器 (WangEditor)</option>
                            <option value="vditor">Markdown 编辑器 (Vditor)</option>
                        </select>
                    </div>
                </div>

                <script charset="utf-8" src="../kindeditor/kindeditor-min.js"></script>
                <script charset="utf-8" src="../kindeditor/lang/zh_CN.js"></script>
                <script src="../js/vendors/vditor/index.min.js"></script>
                <script src="../js/vendors/wangeditor/index.js"></script>
                <script src="../teacher/editor-upload-helper.js" type="text/javascript"></script>
                

                <div class="admin-form-editor-stage">
                    <div id="wangeditor-wrap" style="display:none; width:100%; position:relative; border:1px solid #cbd5e1; z-index:100; margin-bottom:10px; border-radius:14px; overflow:hidden;">
                        <div id="wangeditor-toolbar" style="border-bottom:1px solid #cbd5e1;"></div>
                        <div id="wangeditor-text" style="height:360px;"></div>
                    </div>
                    <div id="vditor-wrap" style="display:none; width:100%; position:relative; margin-bottom:10px;">
                        <div id="vditor-container"></div>
                    </div>
                    <textarea id="mcontent" runat="server"></textarea>
                </div>
            </section>

            <section class="admin-form-feedback">
                <h2 class="admin-form-section-title">处理反馈</h2>
                <p class="admin-form-section-desc">标题或说明为空时，系统仍沿用原有提示逻辑输出反馈信息。</p>
                <asp:Label ID="Labelmsg" runat="server"></asp:Label>
            </section>

            <section class="admin-form-actions">
                <div class="admin-form-action-row">
                    <asp:Button ID="Btnedit" runat="server" Text="保存修改" OnClick="Btnedit_Click" OnClientClick="return syncContent();" CssClass="admin-form-btn admin-form-btn--primary" />
                    <asp:Button ID="BtnCourse" runat="server" Text="返回学案" OnClick="BtnCourse_Click" CssClass="admin-form-btn admin-form-btn--secondary" />
                </div>
            </section>
        </div>
    </div>
    <script type="text/javascript">
        window.__pixeleditConfig = {
            myCid: '<%=myCid() %>',
            mcontentId: '<%= mcontent.ClientID %>'
        };
    </script>
    <script type="text/javascript" src="../js/pixeledit.js"></script>
</asp:Content>
