<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master"  StylesheetTheme="Teacher" Validaterequest="false"  AutoEventWireup="true" CodeFile="topicadd.aspx.cs" Inherits="Teacher_topicadd" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../js/vendors/wangeditor/style.css" rel="stylesheet" />
    <link rel="stylesheet" href="../js/vendors/vditor/index.css" />
    

    <div class="content-add-page topic-add-page">
        <div class="content-add-shell is-medium">
            <section class="content-add-hero">
                <div class="content-add-hero-content">
                    <span class="content-add-eyebrow">Add Discussion Topic</span>
                    <h1 class="content-add-title">添加讨论主题</h1>
                    <p class="content-add-subtitle">保留现有标题、暂停状态和富文本内容提交逻辑，仅优化讨论主题创建页的布局、层次和输入体验。</p>
                </div>
            </section>

            <section class="content-add-panel">
                <h2 class="content-add-section-title">基础设置</h2>
                <p class="content-add-section-desc">讨论主题标题和暂停状态沿用当前后台逻辑，创建后仍会回到学案展示页。</p>
                <div class="content-add-grid">
                    <div class="content-add-field content-add-field-wide">
                        <label class="content-add-label" for="<%= Texttitle.ClientID %>">讨论主题</label>
                        <asp:TextBox ID="Texttitle" runat="server" SkinID="TextBoxNormal" Width="400px" CssClass="content-add-input"></asp:TextBox>
                    </div>
                    <div class="content-add-field content-add-field-wide">
                        <span class="content-add-label">状态设置</span>
                        <div class="content-add-checks">
                            <asp:CheckBox ID="CheckPublish" runat="server" Text="是否暂停" />
                        </div>
                    </div>
                </div>
            </section>

            <section class="content-add-editor">
                <h2 class="content-add-section-title">讨论说明</h2>
                <div class="topic-add-editor-wrap">
                    <p class="content-add-section-desc" style="margin:0;">支持 KindEditor、WangEditor 和 Vditor 三种编辑方式切换。</p>
                    <div>
                        <label class="content-add-label" for="editorSelector">编辑器选择</label><br />
                        <select id="editorSelector" onchange="switchEditor(this.value)" class="topic-add-editor-select">
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
                
                <div class="content-add-editor-stage topic-add-editor-stage custom-scrollbar">
                    <div id="wangeditor-wrap" style="display:none; width:100%; position:relative; border:1px solid #ccc; z-index:100;">
                        <div id="wangeditor-toolbar" style="border-bottom:1px solid #ccc;"></div>
                        <div id="wangeditor-text" style="height:380px;"></div>
                    </div>
                    <div id="vditor-wrap" style="display:none; width:100%; position:relative; margin-bottom:10px;">
                        <div id="vditor-container"></div>
                    </div>
                    <textarea name="textareaItem"></textarea>
                </div>
            </section>

            <section class="content-add-feedback">
                <h2 class="content-add-section-title">处理反馈</h2>
                <p class="content-add-section-desc">标题或内容为空时，错误提示仍由原页面逻辑输出。</p>
                <asp:Label ID="Labelmsg" runat="server"></asp:Label>
            </section>

            <section class="content-add-actions">
                <asp:Button ID="Btnadd" runat="server" Text="添加主题" OnClick="Btnadd_Click" OnClientClick="return syncContent();" CssClass="topic-btn topic-btn--primary" />
                <asp:Button ID="BtnCourse" runat="server" Text="返回学案" OnClick="BtnCourse_Click" CssClass="topic-btn topic-btn--secondary" />
            </section>
        </div>
    </div>
    <script type="text/javascript">
        window.__topicaddConfig = {
            myCid: '<%=myCid() %>'
        };
    </script>
    <script type="text/javascript" src="../js/topicadd.js"></script>
</asp:Content>
