<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" Validaterequest="false"  AutoEventWireup="true" CodeFile="pixeladd.aspx.cs" Inherits="Teacher_pixeladd" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../js/vendors/wangeditor/style.css" rel="stylesheet" />
    <link rel="stylesheet" href="../js/vendors/vditor/index.css" />
    

    <div class="content-add-page pixel-add-page">
        <div class="content-add-shell is-medium">
            <section class="content-add-hero">
                <div class="content-add-hero-content">
                    <span class="content-add-eyebrow">Add Custom Theme</span>
                    <h1 class="content-add-title">添加主题应用</h1>
                    <p class="content-add-subtitle">保留应用类型切换、物联网设备选择、嵌入地址输入、发布状态与量规逻辑，仅重构页面层次和输入布局。</p>
                </div>
            </section>

            <section class="content-add-panel">
                <h2 class="content-add-section-title">基础设置</h2>
                <p class="content-add-section-desc">主题类型切换仍会触发原有 `DDLTitle_SelectedIndexChanged`，设备和地址输入显隐逻辑保持不变。</p>
                <div class="content-add-grid">
                    <div class="content-add-field">
                        <label class="content-add-label" for="<%= DDLTitle.ClientID %>">应用类型</label>
                        <asp:DropDownList ID="DDLTitle" runat="server" Font-Size="Medium" onselectedindexchanged="DDLTitle_SelectedIndexChanged" AutoPostBack="True" CssClass="content-add-select">
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

                    <div class="content-add-field content-add-field-wide">
                        <div class="pixel-add-type-note">
                            <span class="pixel-add-chip" style='background:<%= GetActivityBadgeBackground() %>;color:<%= GetActivityBadgeForeground() %>;'>
                                <img src="<%= GetActivityIconUrl() %>" alt="" />
                                <%= GetActivityDisplayName() %>
                            </span>
                            <div class="pixel-add-type-copy"><%= GetActivityDescription() %></div>
                        </div>
                        <div class="pixel-add-type-list">
                            <div class="pixel-add-type-item">
                                <strong>学生端入口</strong>
                                <span class="pixel-add-route"><%= GetStudentEntryUrl() %></span>
                            </div>
                            <div class="pixel-add-type-item">
                                <strong>创建重点</strong>
                                <%= GetEditFocusText() %>
                            </div>
                        </div>
                    </div>

                    <div class="content-add-field content-add-field-wide">
                        <label class="content-add-label" for="<%= Texttitle.ClientID %>">主题名称</label>
                        <asp:TextBox ID="Texttitle" runat="server" SkinID="TextBoxNormal" Width="300px" CssClass="content-add-input"></asp:TextBox>
                    </div>

                    <div class="content-add-field">
                        <span class="content-add-label">发布设置</span>
                        <div class="content-add-checks">
                            <asp:CheckBox ID="CheckPublish" runat="server" Text="是否发布" Checked="True" />
                        </div>
                    </div>

                    <div class="content-add-field">
                        <label class="content-add-label" for="<%= DDLMgid.ClientID %>">评价标准</label>
                        <asp:DropDownList ID="DDLMgid" runat="server" Font-Size="9pt" Width="160px" Font-Names="Arial" CssClass="content-add-select"></asp:DropDownList>
                    </div>

                    <div class="content-add-field content-add-field-wide">
                        <span class="content-add-label">设备选择</span>
                        <div class="pixel-add-device-list">
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
                    </div>

                    <div class="content-add-field content-add-field-wide">
                        <label class="content-add-label" for="<%= Texturl.ClientID %>">嵌入地址</label>
                        <asp:TextBox ID="Texturl" runat="server" SkinID="TextBoxNormal" Width="200px" Visible="False" CssClass="content-add-input">https://image.baidu.com</asp:TextBox>
                    </div>
                </div>
            </section>

            <section class="content-add-editor">
                <h2 class="content-add-section-title">主题说明</h2>
                <div class="pixel-add-editor-wrap">
                    <p class="content-add-section-desc" style="margin:0;">支持 KindEditor、WangEditor 和 Vditor 三种编辑方式切换。</p>
                    <div>
                        <label class="content-add-label" for="editorSelector">编辑器选择</label><br />
                        <select id="editorSelector" onchange="switchEditor(this.value)" class="pixel-add-editor-select">
                            <option value="kindeditor" selected>原生编辑器 (KindEditor)</option>
                            <option value="wangeditor">WangEditor</option>
                            <option value="vditor">Vditor</option>
                        </select>
                    </div>
                </div>
                <script charset="utf-8" src="../kindeditor/kindeditor-min.js"></script>
                <script charset="utf-8" src="../kindeditor/lang/zh_CN.js"></script>
                <script src="../js/vendors/vditor/index.min.js"></script>
                <script src="../js/vendors/wangeditor/index.js"></script>
                <script src="../teacher/editor-upload-helper.js" type="text/javascript"></script>
                
                <div class="content-add-editor-stage pixel-add-editor-stage custom-scrollbar">
                    <div id="wangeditor-wrap" style="display:none; width:100%; position:relative; border:1px solid #ccc; z-index:100;">
                        <div id="wangeditor-toolbar" style="border-bottom:1px solid #ccc;"></div>
                        <div id="wangeditor-text" style="height:350px;"></div>
                    </div>
                    <div id="vditor-wrap" style="display:none; width:100%; position:relative; margin-bottom:10px;">
                        <div id="vditor-container"></div>
                    </div>
                    <textarea name="textareaItem"></textarea>
                </div>
            </section>

            <section class="content-add-feedback">
                <h2 class="content-add-section-title">处理反馈</h2>
                <p class="content-add-section-desc">主题为空或说明不完整时，提示信息仍由原页面逻辑输出。</p>
                <asp:Label ID="Labelmsg" runat="server"></asp:Label>
            </section>

            <section class="content-add-actions">
                <asp:Button ID="Btnadd" runat="server" Text="添加主题" OnClick="Btnadd_Click" OnClientClick="return syncContent();" CssClass="content-add-primary" />
                <asp:Button ID="BtnCourse" runat="server" Text="返回学案" OnClick="BtnCourse_Click" CssClass="content-add-secondary" />
            </section>
        </div>
    </div>
    <script type="text/javascript">
        window.__pixeladdConfig = {
            myCid: '<%=myCid() %>'
        };
    </script>
    <script type="text/javascript" src="../js/pixeladd.js"></script>
</asp:Content>
