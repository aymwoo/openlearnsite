<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher"  Validaterequest="false" AutoEventWireup="true" CodeFile="softadd.aspx.cs" Inherits="Teacher_softadd" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../js/fileupload.css" rel="stylesheet" />

    <div class="soft-add-page">
        <div class="lesson-shell">
            <div class="lesson-hero">
                <h1 class="lesson-hero__title"><i class="bi bi-plus-square" style="color: #a5b4fc;"></i> 添加学习资源</h1>
                <p class="lesson-hero__subtitle">在此处添加教程、微课、软件等资源，配置访问规则、评分方式与附件内容。</p>
            </div>

            <div class="lesson-card">
                <h2 class="lesson-card__title">基础设置</h2>
                <div class="form-grid">
                    <div class="form-field form-field--4">
                        <label class="form-label">资源名称</label>
                        <asp:TextBox ID="Texttitle" runat="server" CssClass="form-input"></asp:TextBox>
                    </div>
                    <div class="form-field form-field--3">
                        <label class="form-label">资源分类</label>
                        <asp:DropDownList ID="ddlcategory" runat="server" CssClass="form-select"></asp:DropDownList>
                    </div>
                    <div class="form-field form-field--3">
                        <label class="form-label">资源属性</label>
                        <asp:DropDownList ID="DDLclass" runat="server" CssClass="form-select">
                            <asp:ListItem Selected="True">教程</asp:ListItem>
                            <asp:ListItem>微课</asp:ListItem>
                            <asp:ListItem>资料</asp:ListItem>
                            <asp:ListItem>软件</asp:ListItem>
                            <asp:ListItem>游戏</asp:ListItem>
                            <asp:ListItem>课程</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-field form-field--2">
                        <label class="form-label">学分限制</label>
                        <asp:DropDownList ID="DDLopen" runat="server" CssClass="form-select">
                            <asp:ListItem Value="10">A</asp:ListItem>
                            <asp:ListItem Value="8">B</asp:ListItem>
                            <asp:ListItem Value="6">C</asp:ListItem>
                            <asp:ListItem Value="4">D</asp:ListItem>
                            <asp:ListItem Value="2">E</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-field form-field--3">
                        <label class="form-label">评分方式</label>
                        <asp:DropDownList ID="DDLscoreType" runat="server" CssClass="form-select">
                            <asp:ListItem Value="original">原学分制</asp:ListItem>
                            <asp:ListItem Value="comprehensive">综合评分制</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-field form-field--2">
                        <label class="form-label">综合得分</label>
                        <asp:TextBox ID="TXTscore" runat="server" CssClass="form-input" Text="60" ToolTip="学生综合得分达到此值才能访问资源（0-100分）"></asp:TextBox>
                    </div>
                    <div class="form-field form-field--7" style="flex-direction: row; gap: 20px; align-items: center; margin-top: 10px; flex-wrap: wrap;">
                        <label style="display: flex; align-items: center; gap: 8px; font-size: 14px; font-weight: 600; color: #334155; cursor: pointer;">
                            <asp:CheckBox ID="CheckBoxFhide" runat="server" Text="是否隐藏" />
                        </label>
                        <label style="display: flex; align-items: center; gap: 8px; font-size: 14px; font-weight: 600; color: #334155; cursor: pointer;">
                            <asp:CheckBox ID="CheckBoxFhid" runat="server" Text="是否共享" />
                        </label>
                    </div>
                </div>
            </div>

            <div class="lesson-card">
                <div class="editor-header">
                    <h2 class="lesson-card__title" style="margin:0;">资源详情内容</h2>
                    <div>
                        <label class="form-label" style="margin-right: 8px;">编辑器选择:</label>
                        <select id="editorSelector" onchange="switchEditor(this.value)" class="form-select" style="width: auto; height: 36px; padding: 0 30px 0 10px; display: inline-block;">
                            <option value="kindeditor" selected>原生编辑器 (KindEditor)</option>
                            <option value="wangeditor">富文本编辑器 (WangEditor)</option>
                            <option value="vditor">Markdown编辑器 (Vditor)</option>
                        </select>
                    </div>
                </div>

                <link href="../js/vendors/wangeditor/style.css" rel="stylesheet">
                <link rel="stylesheet" href="../js/vendors/vditor/index.css" />
                <script src="../js/vendors/vditor/index.min.js"></script>
                <script src="../js/vendors/wangeditor/index.js"></script>

                <script charset="utf-8" src="../kindeditor/kindeditor-min.js"></script>
                <script charset="utf-8" src="../kindeditor/lang/zh_CN.js"></script>
                <script src="../teacher/editor-upload-helper.js" type="text/javascript"></script>

                <script>
                    var editor;
                    var cid = '-1';
                    var ty = 'Soft';
                    var upjs = '../kindeditor/aspnet/upload_json.aspx?cid=' + cid + '&ty=' + ty;
                    var fmjs = '../kindeditor/aspnet/file_manager_json.aspx?cid=' + cid + '&ty=' + ty;
                    KindEditor.ready(function (K) {
                        editor = K.create('textarea[name="textareaItem"]', {
                            resizeType: 1,
                            newlineTag: 'br',
                            uploadJson: upjs,
                            fileManagerJson: fmjs,
                            allowFileManager: true,
                            filterMode: false
                        });
                    });
                </script>

                <div class="editor-stage">
                    <div id="wangeditor-wrap" style="display:none; width: 100%; border-bottom: 1px solid #ccc; z-index: 100;">
                        <div id="wangeditor-toolbar" style="border-bottom: 1px solid #ccc;"></div>
                        <div id="wangeditor-text" style="height: 350px;"></div>
                    </div>

                    <div id="vditor-wrap" style="display:none; width: 100%; margin-bottom: 10px;">
                        <div id="vditor-container"></div>
                    </div>

                    <textarea name="textareaItem" style="width: 100%; height:400px; border:none; padding:10px;"></textarea>
                </div>
            </div>

            <div class="lesson-card">
                <h2 class="lesson-card__title">附件及操作</h2>
                <div class="form-grid">
                    <div class="form-field form-field--12">
                        <label class="form-label">上传可限制资源（如软件包、配套素材）</label>
                        <div class="ls-upload" data-label="点击或拖拽上传资源文件" data-hint="支持任意文件类型">
                            <asp:FileUpload ID="FUsoft" runat="server" />
                        </div>
                    </div>
                </div>

                <asp:Label ID="Labelmsg" runat="server" style="display: block; margin-top: 12px; color: #dc2626; font-weight: 600; font-size: 14px;"></asp:Label>

                <div class="soft-actions">
                    <asp:Button ID="Btnadd" runat="server" Text="添加资源" OnClick="Btnadd_Click" OnClientClick="return syncContent();" CssClass="soft-btn soft-btn--primary" />
                    <asp:Button ID="Btnreturn" runat="server" Text="返回列表" OnClick="Btnreturn_Click" CssClass="soft-btn soft-btn--secondary" />
                </div>

                <div class="info-note">
                    <strong><i class="bi bi-info-circle-fill mr-1"></i> 注明：</strong>
                    如果资源属性为【教程】或【微课】，则学生在浏览该资源学习时，能够提交自学作品！
                </div>
            </div>
        </div>
    </div>
    <script src="../js/fileupload.js"></script>
    <script type="text/javascript" src="../js/softadd.js"></script>
</asp:Content>
