<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" Validaterequest="false" AutoEventWireup="true" CodeFile="pythonadd.aspx.cs"  inherits="Teacher_pythonadd" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../js/fileupload.css" rel="stylesheet" />
    <link href="../js/vendors/wangeditor/style.css" rel="stylesheet" />
    <link rel="stylesheet" href="../js/vendors/vditor/index.css" />
    

    <div class="content-add-page python-add-page">
        <div class="content-add-shell is-medium">
            <section class="content-add-hero">
                <div class="content-add-hero-content">
                    <span class="content-add-eyebrow">Add Python Exercise</span>
                    <h1 class="content-add-title">添加 Python 编程主题</h1>
                    <p class="content-add-subtitle">保留原有 Python 练习主题、发布模式、拼图/积木选项、绘图帮助和示例文件上传逻辑，只重构页面布局与交互层次。</p>
                </div>
            </section>

            <section class="content-add-panel">
                <h2 class="content-add-section-title">基础设置</h2>
                <p class="content-add-section-desc">以下字段仍按当前后台逻辑处理，创建后继续返回学案展示页。</p>
                <div class="content-add-grid">
                    <div class="content-add-field content-add-field-wide">
                        <label class="content-add-label" for="<%= Texttitle.ClientID %>">Python 主题</label>
                        <asp:TextBox ID="Texttitle" runat="server" SkinID="TextBoxNormal" Width="200px" CssClass="content-add-input"></asp:TextBox>
                    </div>

                    <div class="content-add-field">
                        <label class="content-add-label" for="<%= DDLMgid.ClientID %>">评价标准</label>
                        <asp:DropDownList ID="DDLMgid" runat="server" Font-Size="9pt" Width="160px" Font-Names="Arial" CssClass="content-add-select"></asp:DropDownList>
                    </div>

                    <div class="content-add-field">
                        <span class="content-add-label">练习标识</span>
                        <div class="content-add-static">
                            <span class="python-add-badge"><img src="../images/python.png" alt="python" /> Python 练习</span>
                        </div>
                    </div>

                    <div class="content-add-field content-add-field-wide">
                        <span class="content-add-label">运行模式</span>
                        <div class="content-add-checks">
                            <asp:CheckBox ID="CheckPublish" runat="server" Text="发布" Checked="True" />
                            <asp:CheckBox ID="CheckBack" runat="server" Text="分步" ToolTip="命令行和编辑器模式切换" />
                            <asp:CheckBox ID="Checkhelp" runat="server" Text="绘图" ToolTip="编程和绘图语句帮助切换" />
                            <asp:CheckBox ID="Checkblock" runat="server" Text="拼图" ToolTip="拼图编程模式" />
                            <asp:CheckBox ID="Checkblockpy" runat="server" Text="积木" ToolTip="积木编程模式（优先）" />
                        </div>
                    </div>

                    <div class="content-add-field content-add-field-wide">
                        <label class="content-add-label" for="<%= Fupload.ClientID %>">Python 示例文件</label>
                        <div class="ls-upload" data-accept=".py" data-label="点击或拖拽上传示例文件" data-hint="支持 py 格式">
                            <asp:FileUpload ID="Fupload" runat="server" />
                        </div>
                    </div>
                </div>
            </section>

            <section class="content-add-editor">
                <h2 class="content-add-section-title">编程说明</h2>
                <div class="python-add-editor-wrap">
                    <p class="content-add-section-desc" style="margin:0;">支持 KindEditor、WangEditor 和 Vditor 三种编辑方式切换。</p>
                    <div>
                        <label class="content-add-label" for="editorSelector">编辑器选择</label><br />
                        <select id="editorSelector" onchange="switchEditor(this.value)" class="python-add-editor-select">
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
                
                <div class="content-add-editor-stage python-add-editor-stage custom-scrollbar">
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
                <p class="content-add-section-desc">主题为空或内容不完整时，提示信息仍由原后台逻辑输出。</p>
                <asp:Label ID="Labelmsg" runat="server"></asp:Label>
            </section>

            <section class="content-add-actions">
                <asp:Button ID="Btnadd" runat="server" Text="添加主题" OnClick="Btnadd_Click" OnClientClick="return syncContent();" CssClass="content-add-primary" />
                <asp:Button ID="BtnCourse" runat="server" Text="返回学案" OnClick="BtnCourse_Click" CssClass="content-add-secondary" />
            </section>
        </div>
    </div>
    <script src="../js/fileupload.js"></script>
    <script type="text/javascript">
        window.__pythonaddConfig = {
            myCid: '<%=myCid() %>'
        };
    </script>
    <script type="text/javascript" src="../js/pythonadd.js"></script>
</asp:Content>
