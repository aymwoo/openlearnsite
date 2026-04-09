<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master"  Validaterequest="false" AutoEventWireup="true" CodeFile="htmladd.aspx.cs" Inherits="teacher_htmladd" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/htmladd.css" rel="stylesheet" />
    <link href="../js/vendors/wangeditor/style.css" rel="stylesheet" />
    <link rel="stylesheet" href="../js/vendors/vditor/index.css" />
    

    <div class="content-add-page html-add-page">
        <div class="content-add-shell is-medium">
            <section class="content-add-hero">
                <div class="content-add-hero-content">
                    <span class="content-add-eyebrow">Add HTML Page</span>
                    <h1 class="content-add-title">添加网页主题</h1>
                    <p class="content-add-subtitle">保留网页主题、发布状态、页面文件名与说明内容的原有保存逻辑，只优化界面层次、表单布局和编辑体验。</p>
                </div>
            </section>

            <section class="content-add-panel">
                <h2 class="content-add-section-title">基础设置</h2>
                <p class="content-add-section-desc">网页文件名和评价量规仍按现有后台逻辑写入，创建后继续返回学案页面。</p>
                <div class="content-add-grid">
                    <div class="content-add-field content-add-field-wide">
                        <label class="content-add-label" for="<%= Texttitle.ClientID %>">网页主题</label>
                        <asp:TextBox ID="Texttitle" runat="server" SkinID="TextBoxNormal" Width="220px" CssClass="content-add-input"></asp:TextBox>
                    </div>

                    <div class="content-add-field">
                        <span class="content-add-label">发布设置</span>
                        <div class="content-add-checks">
                            <asp:CheckBox ID="CheckPublish" runat="server" Text="是否发布" Checked="True" />
                        </div>
                    </div>

                    <div class="content-add-field">
                        <label class="content-add-label" for="<%= DDLfilename.ClientID %>">网页文件名</label>
                        <asp:DropDownList ID="DDLfilename" runat="server" CssClass="content-add-select">
                            <asp:ListItem>index.html</asp:ListItem>
                            <asp:ListItem>w1.html</asp:ListItem>
                            <asp:ListItem>w2.html</asp:ListItem>
                            <asp:ListItem>w3.html</asp:ListItem>
                            <asp:ListItem>w4.html</asp:ListItem>
                            <asp:ListItem>w5.html</asp:ListItem>
                            <asp:ListItem>a1.html</asp:ListItem>
                            <asp:ListItem>a2.html</asp:ListItem>
                            <asp:ListItem>a3.html</asp:ListItem>
                            <asp:ListItem>a4.html</asp:ListItem>
                            <asp:ListItem>a5.html</asp:ListItem>
                        </asp:DropDownList>
                    </div>

                    <div class="content-add-field content-add-field-wide">
                        <label class="content-add-label" for="<%= DDLMgid.ClientID %>">评价标准</label>
                        <asp:DropDownList ID="DDLMgid" runat="server" Font-Size="9pt" Width="160px" Font-Names="Arial" CssClass="content-add-select"></asp:DropDownList>
                    </div>
                </div>
            </section>

            <section class="content-add-editor">
                <h2 class="content-add-section-title">网页说明</h2>
                <div class="html-add-editor-wrap">
                    <p class="content-add-section-desc" style="margin:0;">支持 KindEditor、WangEditor 和 Vditor 三种编辑方式切换。</p>
                    <div>
                        <label class="content-add-label" for="editorSelector">编辑器选择</label><br />
                        <select id="editorSelector" onchange="switchEditor(this.value)" class="html-add-editor-select">
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
                
                <div class="content-add-editor-stage html-add-editor-stage custom-scrollbar">
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
                <p class="content-add-section-desc">主题为空或内容不完整时，提示信息仍由原逻辑输出。</p>
                <asp:Label ID="Labelmsg" runat="server"></asp:Label>
            </section>

            <section class="content-add-actions">
                <asp:Button ID="Btnadd" runat="server" Text="添加主题" OnClick="Btnadd_Click" OnClientClick="return syncContent();" CssClass="content-add-primary" />
                <asp:Button ID="BtnCourse" runat="server" Text="返回学案" OnClick="BtnCourse_Click" CssClass="content-add-secondary" />
            </section>
        </div>
    </div>
    <script type="text/javascript">
        window.__htmladdConfig = {
            myCid: '<%=myCid() %>'
        };
    </script>
    <script type="text/javascript" src="../js/htmladd.js"></script>
</asp:Content>
