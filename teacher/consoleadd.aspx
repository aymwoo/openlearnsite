<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" Validaterequest="false" AutoEventWireup="true" CodeFile="consoleadd.aspx.cs" Inherits="Teacher_consoleadd" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/course-content-add.css" rel="stylesheet" />
    <link href="../js/vendors/wangeditor/style.css" rel="stylesheet" />
    <link rel="stylesheet" href="../js/vendors/vditor/index.css" />
    

    <div class="content-add-page console-add-page">
        <div class="content-add-shell is-medium">

            <!-- Hero Banner -->
            <section class="content-add-hero">
                <div class="content-add-hero-content">
                    <div class="content-add-eyebrow">
                        &#128187; 交互式 Python 测评
                    </div>
                    <h1 class="content-add-title">测评内容编辑</h1>
                    <p class="content-add-subtitle">编写 Python 交互式测评的标准语句与测试用例，学生提交后将自动评分。</p>
                </div>
            </section>

            <!-- Settings Panel -->
            <section class="content-add-panel">
                <h2 class="content-add-section-title">基本设置</h2>
                <div class="content-add-field-group">
                    <div class="content-add-field">
                        <label class="content-add-label">测评名称</label>
                        <asp:TextBox ID="Texttitle" runat="server" SkinID="TextBoxNormal" Width="340px"
                            CssClass="content-add-input"></asp:TextBox>
                    </div>
                    <div class="content-add-field">
                        <label class="content-add-label">发布状态</label>
                        <asp:CheckBox ID="Publish" runat="server" Text="立即发布" CssClass="content-add-checkbox" />
                    </div>
                </div>
            </section>

            <!-- Editor Panel -->
            <section class="content-add-editor">
                <div class="content-add-editor-toolbar">
                    <div>
                        <h2 class="content-add-section-title">测评内容</h2>
                        <p class="content-add-section-desc">支持 KindEditor、WangEditor 和 Vditor 三种编辑方式切换。</p>
                    </div>
                    <div>
                        <label class="content-add-label" for="editorSelector">编辑器选择</label><br />
                        <select id="editorSelector" onchange="switchEditor(this.value)" class="content-add-select" style="width:auto;min-width:160px;">
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
                

                <div class="content-add-editor-stage console-add-editor-stage">
                    <div id="wangeditor-wrap" style="display:none; width:100%; position:relative; border:1px solid #ccc; z-index:100; margin-bottom:10px;">
                        <div id="wangeditor-toolbar" style="border-bottom:1px solid #ccc;"></div>
                        <div id="wangeditor-text" style="height:420px;"></div>
                    </div>
                    <div id="vditor-wrap" style="display:none; width:100%; position:relative; margin-bottom:10px;">
                        <div id="vditor-container"></div>
                    </div>
                    <textarea id="mcontent" runat="server" style="width:100%;height:500px;"></textarea>
                </div>
            </section>

            <!-- Feedback -->
            <section class="content-add-feedback">
                <asp:Label ID="Labelmsg" runat="server" CssClass="content-add-feedback-msg"></asp:Label>
            </section>

            <!-- Actions -->
            <div class="content-add-actions">
                <asp:Button ID="Btnadd" runat="server" Text="添加测评"
                    onclick="Btnadd_Click"
                    OnClientClick="return syncContent();"
                    CssClass="content-add-btn-primary" />
                <asp:Button ID="BtnCourse" runat="server" Text="返回学案"
                    onclick="BtnCourse_Click"
                    CssClass="content-add-btn-secondary" />
            </div>

        </div>
    </div>
    <script type="text/javascript">
        window.__consoleaddConfig = {
            myCid: '<%=myCid() %>',
            mcontentId: '<%= mcontent.ClientID %>'
        };
    </script>
    <script type="text/javascript" src="../js/consoleadd.js"></script>
</asp:Content>
