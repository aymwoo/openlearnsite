<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master"  Validaterequest="false" AutoEventWireup="true" CodeFile="exceladd.aspx.cs" Inherits="teacher_exceladd" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
<link href="../App_Themes/Teacher/exceladd.css" rel="stylesheet" />
<link href="../js/fileupload.css" rel="stylesheet" />


<div class="content-add-page exceladd-page">
    <div class="content-add-shell is-medium">

        <!-- Hero -->
        <section class="content-add-hero">
            <div class="content-add-hero-content">
                <span class="content-add-eyebrow">Add Excel Activity</span>
                <h1 class="content-add-title">添加表格处理主题</h1>
                <p class="content-add-subtitle">可上传 xls / xlsx 作为初始实例文件，学生提交后使用在线表格编辑器处理并保存成果。支持自定义评价量规与发布状态。</p>
            </div>
        </section>

        <!-- Settings panel -->
        <section class="content-add-panel">
            <h2 class="content-add-section-title">主题设置</h2>
            <p class="content-add-section-desc">以下字段沿用原有后台逻辑，创建成功后自动返回当前学案页面。</p>
            <div class="content-add-grid">

                <div class="content-add-field content-add-field-wide">
                    <label class="content-add-label" for="<%= Texttitle.ClientID %>">主题名称</label>
                    <asp:TextBox ID="Texttitle" runat="server" SkinID="TextBoxNormal" CssClass="content-add-input"></asp:TextBox>
                </div>

                <div class="content-add-field">
                    <label class="content-add-label" for="<%= DDLMgid.ClientID %>">评价标准</label>
                    <asp:DropDownList ID="DDLMgid" runat="server" Font-Size="9pt" Font-Names="Arial" CssClass="content-add-select"></asp:DropDownList>
                </div>

                <div class="content-add-field content-add-field-wide">
                    <span class="content-add-label">发布选项</span>
                    <div class="content-add-checks">
                        <asp:CheckBox ID="CheckPublish" runat="server" Text="是否发布" Checked="True" />
                    </div>
                </div>

                <div class="content-add-field content-add-field-wide">
                    <span class="content-add-label">实例文件（可选）</span>
                    <div class="ls-upload" data-accept=".xls,.xlsx" data-label="点击或拖拽上传表格实例" data-hint="支持 xls / xlsx 格式，学生作业页面将以此为初始文件">
                        <asp:FileUpload ID="Fupload" runat="server" />
                    </div>
                </div>

            </div>
        </section>

        <!-- Editor panel -->
        <section class="content-add-editor">
            <link href="../js/vendors/wangeditor/style.css" rel="stylesheet">
            <link rel="stylesheet" href="../js/vendors/vditor/index.css" />
            <script src="../js/vendors/vditor/index.min.js"></script>
            <script src="../js/vendors/wangeditor/index.js"></script>

            <div class="content-add-editor-toolbar">
                <div>
                    <h2 class="content-add-section-title">活动说明</h2>
                    <p class="content-add-section-desc">为表格活动编写说明或要求，支持 KindEditor、WangEditor 和 Vditor 三种编辑方式切换。</p>
                </div>
                <div>
                    <label class="content-add-label" for="editorSelector">编辑器选择</label><br />
                    <select id="editorSelector" onchange="switchEditor(this.value)" class="content-add-editor-select">
                        <option value="kindeditor" selected>原生编辑器 (KindEditor)</option>
                        <option value="wangeditor">富文本编辑器 (WangEditor)</option>
                        <option value="vditor">Markdown编辑器 (Vditor)</option>
                    </select>
                </div>
            </div>

                <script charset="utf-8" src="../kindeditor/kindeditor-min.js"></script>
                <script charset="utf-8" src="../kindeditor/lang/zh_CN.js"></script>
                <script src="../teacher/editor-upload-helper.js" type="text/javascript"></script>
                

            <div class="content-add-editor-stage exceladd-page">
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

        <!-- Feedback -->
        <section class="content-add-feedback">
            <h2 class="content-add-section-title">处理反馈</h2>
            <p class="content-add-section-desc">标题或内容为空时，错误信息按原有逻辑显示。</p>
            <asp:Label ID="Labelmsg" runat="server"></asp:Label>
        </section>

        <!-- Actions -->
        <section class="content-add-actions">
            <asp:Button ID="Btnadd" runat="server" Text="添加主题" OnClick="Btnadd_Click" OnClientClick="return syncContent();" CssClass="content-add-primary" />
            <asp:Button ID="BtnCourse" runat="server" Text="返回学案" OnClick="BtnCourse_Click" CssClass="content-add-secondary" />
        </section>

    </div>
</div>

<script src="../js/fileupload.js"></script>
    <script type="text/javascript">
        window.__exceladdConfig = {
            myCid: '<%=myCid() %>'
        };
    </script>
    <script type="text/javascript" src="../js/exceladd.js"></script>
</asp:Content>
