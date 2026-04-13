<%@ Page Language="C#" AutoEventWireup="true" CodeFile="questionadd.aspx.cs" Inherits="exam_question_questionadd" MasterPageFile="~/teacher/Teach.master" %><asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/webform/jquery-3.6.0.min.js" type="text/javascript"></script>
    <script src="/webform/bootstrap.bundle.min.js" type="text/javascript"></script>
    <link href="/webform/summernote-bs5.min.css" rel="stylesheet" />
    <link href="/webform/paper.css" rel="stylesheet" />
    <script src="/webform/summernote-bs5.min.js"></script>
    <script src="/webform/summernote-zh-CN.min.js"></script>
</asp:Content><asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <style>
        .question-form-page { min-height: calc(100vh - 8rem); padding: 1.5rem; background: #f8fafc; }
        .question-form-shell { max-width: 1100px; margin: 0 auto; display: flex; flex-direction: column; gap: 1.25rem; }
        .form-hero, .form-card { border: 1px solid rgba(148, 163, 184, 0.18); border-radius: 1.25rem; background: #ffffff; box-shadow: 0 12px 32px -28px rgba(15, 23, 42, 0.28); }
        .form-hero { display: flex; justify-content: space-between; align-items: flex-start; gap: 1rem; padding: 1.5rem; background: linear-gradient(135deg, #ffffff 0%, #f8fafc 100%); }
        .form-title { margin: 0; color: #0f172a; font-size: 1.625rem; font-weight: 700; }
        .form-subtitle { margin: 0.75rem 0 0; color: #475569; font-size: 0.95rem; line-height: 1.7; }
        .hero-actions { display: flex; flex-wrap: wrap; gap: 0.75rem; }
        .page-btn, .form-actions input, .hero-actions a { display: inline-flex; align-items: center; justify-content: center; min-height: 2.75rem; padding: 0 1rem; border: 1px solid transparent; border-radius: 0.9rem; font-size: 0.875rem; font-weight: 600; text-decoration: none; cursor: pointer; transition: all 0.2s ease; }
        .page-btn-primary, .form-actions input[id$='btnSave'] { background: #2563eb; color: #ffffff; box-shadow: 0 10px 20px -14px rgba(37, 99, 235, 0.85); }
        .page-btn-success, .form-actions input[id$='btnSaveAdd'] { background: #16a34a; color: #ffffff; box-shadow: 0 10px 20px -14px rgba(22, 163, 74, 0.9); }
        .page-btn-secondary, .form-actions input[id$='btnCancel'], .hero-actions a { background: #ffffff; color: #475569; border-color: #cbd5e1; }
        .form-card { padding: 1.5rem; }
        .section-title { margin: 0 0 1rem; color: #0f172a; font-size: 1rem; font-weight: 700; }
        .form-group { margin-bottom: 1rem; }
        .form-group label { display: block; margin-bottom: 0.45rem; font-weight: 600; color: #334155; }
        .form-group label span.required { color: #dc2626; }
        .form-row { display: flex; gap: 1rem; }
        .form-row .form-group { flex: 1; }
        .form-control { width: 100%; min-height: 2.75rem; padding: 0.7rem 0.9rem; border: 1px solid #cbd5e1; border-radius: 0.9rem; box-sizing: border-box; background: #f8fafc; color: #0f172a; }
        .form-control:focus { outline: none; border-color: #93c5fd; background: #ffffff; box-shadow: 0 0 0 4px rgba(191, 219, 254, 0.6); }
        textarea.form-control { min-height: 7rem; resize: vertical; }
        .help-text { font-size: 0.78rem; color: #94a3b8; margin-top: 0.45rem; line-height: 1.6; }
        .options-container { border: 1px solid #e2e8f0; border-radius: 1rem; padding: 1rem; background: #f8fafc; margin-top: 0.75rem; }
        .option-item-wrapper { display: flex; align-items: center; gap: 0.5rem; margin-bottom: 0.75rem; padding: 0.5rem; background: #fff; border-radius: 0.5rem; border: 1px solid #e2e8f0; }
        .option-item-wrapper:hover { border-color: #3b82f6; }
        .option-item-wrapper .option-label { width: 2rem; font-weight: 700; color: #475569; flex-shrink: 0; }
        .option-item-wrapper .option-input { flex: 1; min-height: 2.25rem; padding: 0.5rem 0.75rem; border: 1px solid #cbd5e1; border-radius: 0.5rem; background: #fff; }
        .option-item-wrapper .option-input:focus { outline: none; border-color: #3b82f6; box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1); }
        .option-image-preview { max-width: 60px; max-height: 45px; border-radius: 4px; object-fit: cover; border: 1px solid #e2e8f0; margin-left: 0.5rem; }
        .option-image-btn { background: #f1f5f9; border: 1px solid #cbd5e1; color: #3b82f6; padding: 0.35rem 0.5rem; border-radius: 0.375rem; cursor: pointer; font-size: 0.85rem; transition: all 0.2s; }
        .option-image-btn:hover { background: #3b82f6; color: #fff; border-color: #3b82f6; }
        .option-image-btn.delete-btn { color: #dc2626; }
        .option-image-btn.delete-btn:hover { background: #dc2626; color: #fff; border-color: #dc2626; }
        .option-controls { display: flex; align-items: center; gap: 0.25rem; flex-shrink: 0; }
        .inline-choice { display: inline-flex; align-items: center; gap: 1rem; flex-wrap: wrap; }
        .inline-choice label { display: inline-flex; align-items: center; gap: 0.35rem; margin: 0; padding: 0.6rem 0.85rem; border: 1px solid #dbeafe; border-radius: 999px; background: #eff6ff; color: #1d4ed8; font-weight: 500; }
        .btn-sm { min-height: 2.25rem; padding: 0 0.85rem; }
        .btn-default { background: #ffffff; color: #475569; border: 1px solid #cbd5e1; }
        .form-actions { display: flex; justify-content: center; flex-wrap: wrap; gap: 0.75rem; margin-top: 0.5rem; }
        
        .note-link-popover,
        .note-image-popover,
        .note-video-popover,
        .note-help-popover {
            display: none !important;
        }
        .note-modal {
            display: none !important;
        }
        .modal-backdrop {
            display: none !important;
        }
        
        .note-editor {
            width: 100% !important;
            box-sizing: border-box !important;
            border: 1px solid #e2e8f0 !important;
            border-radius: 0.5rem !important;
            overflow: hidden !important;
        }
        .note-editable {
            min-height: 80px !important;
            padding: 0.75rem !important;
        }
        .note-toolbar {
            position: relative !important;
            white-space: nowrap !important;
            overflow-x: auto !important;
            overflow-y: hidden !important;
            flex-wrap: nowrap !important;
            background: rgba(255, 255, 255, 0.95) !important;
            backdrop-filter: blur(4px) !important;
            border-bottom: 1px solid #e2e8f0 !important;
            transition: opacity 0.2s ease, transform 0.2s ease !important;
            opacity: 0 !important;
            transform: translateY(-10px) !important;
        }
        .note-editor:hover .note-toolbar,
        .note-editor:focus-within .note-toolbar {
            opacity: 1 !important;
            transform: translateY(0) !important;
        }
        .note-toolbar .note-btn-group {
            display: inline-block !important;
            float: none !important;
            margin-right: 2px !important;
        }
        .note-toolbar .note-btn {
            padding: 4px 8px !important;
            font-size: 12px !important;
            line-height: 1.3 !important;
            background: transparent !important;
            border: none !important;
            color: #475569 !important;
            border-radius: 4px !important;
            transition: background 0.15s ease !important;
        }
        .note-toolbar .note-btn:hover {
            background: rgba(59, 130, 246, 0.1) !important;
            color: #2563eb !important;
        }
        .note-toolbar .note-btn.active {
            background: rgba(59, 130, 246, 0.15) !important;
            color: #2563eb !important;
        }
        .note-btn-group {
            background: transparent !important;
            border: none !important;
            box-shadow: none !important;
        }
        
        @media (max-width: 900px) { .question-form-page { padding: 1rem; } .form-hero, .form-row { flex-direction: column; } }
    </style>

    <div class="question-form-page">
        <div class="question-form-shell">
        <section class="form-hero">
            <div>
                <h2 class="form-title"><asp:Literal ID="ltlTitle" runat="server">添加题目</asp:Literal></h2>
                <p class="form-subtitle">在当前题库中维护单选、多选、填空、简答和扩展题型，统一使用考试模块的新表单风格。</p>
            </div>
            <div class="hero-actions">
                <a href="questionlist.aspx?bankId=<%= BankId %>" class="page-btn page-btn-secondary">返回题目列表</a>
            </div>
        </section>

        <asp:HiddenField ID="hfQuestionId" runat="server" />
        <asp:HiddenField ID="hfBankId" runat="server" />

        <section class="form-card">
        <h3 class="section-title">基础信息</h3>
        <div class="form-row">
            <div class="form-group">
                <label><span class="required">*</span> 题型</label>
                <asp:DropDownList ID="ddlType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlType_SelectedIndexChanged">
                    <asp:ListItem Value="1">单选题</asp:ListItem>
                    <asp:ListItem Value="2">多选题</asp:ListItem>
                    <asp:ListItem Value="3">判断题</asp:ListItem>
                    <asp:ListItem Value="4">填空题</asp:ListItem>
                    <asp:ListItem Value="5">简答题</asp:ListItem>
                    <asp:ListItem Value="6">连线题</asp:ListItem>
                    <asp:ListItem Value="7">分类题</asp:ListItem>
                    <asp:ListItem Value="9">多项填空</asp:ListItem>
                    <asp:ListItem Value="10">下拉选择</asp:ListItem>
                    <asp:ListItem Value="11">打分题</asp:ListItem>
                    <asp:ListItem Value="12">矩阵单选</asp:ListItem>
                    <asp:ListItem Value="14">NPS评分</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <label>难度</label>
                <asp:DropDownList ID="ddlDifficulty" runat="server" CssClass="form-control">
                    <asp:ListItem Value="1">简单</asp:ListItem>
                    <asp:ListItem Value="2">中等</asp:ListItem>
                    <asp:ListItem Value="3">困难</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <label>默认分值</label>
                <asp:TextBox ID="txtScore" runat="server" CssClass="form-control" Text="1"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <label><span class="required">*</span> 题目内容</label>
            <div id="summernote-content-question"></div>
            <asp:HiddenField ID="hfContent" runat="server" />
        </div>
        </section>

        <!-- 选项区域 -->
        <section class="form-card">
        <h3 class="section-title">答案设置</h3>
        <asp:Panel ID="pnlOptions" runat="server" CssClass="form-group">
            <label><span class="required">*</span> 选项设置</label>
            <p class="help-text">勾选正确答案</p>
            <div class="options-container">
                <asp:PlaceHolder ID="phOptions" runat="server"></asp:PlaceHolder>
                <asp:Button ID="btnAddOption" runat="server" Text="+ 添加选项" CssClass="page-btn btn-default btn-sm" OnClick="btnAddOption_Click" />
            </div>
        </asp:Panel>

        <!-- 判断题区域 -->
        <asp:Panel ID="pnlJudge" runat="server" CssClass="form-group" Visible="false">
            <label><span class="required">*</span> 正确答案</label>
            <div class="inline-choice">
                <label style="margin-right: 20px;"><asp:RadioButton ID="rbTrue" runat="server" GroupName="judge" /> 正确</label>
                <label><asp:RadioButton ID="rbFalse" runat="server" GroupName="judge" /> 错误</label>
            </div>
        </asp:Panel>

        <!-- 填空题区域 -->
        <asp:Panel ID="pnlFillBlank" runat="server" CssClass="form-group" Visible="false">
            <label><span class="required">*</span> 填空答案</label>
            <p class="help-text">多个空用 | 分隔，例如：答案1|答案2|答案3</p>
            <asp:TextBox ID="txtFillAnswer" runat="server" CssClass="form-control" placeholder="请输入填空答案，多个空用 | 分隔"></asp:TextBox>
        </asp:Panel>

        <!-- 简答题区域 -->
        <asp:Panel ID="pnlTextAnswer" runat="server" CssClass="form-group" Visible="false">
            <label>参考答案</label>
            <asp:TextBox ID="txtRefAnswer" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" placeholder="请输入参考答案..."></asp:TextBox>
        </asp:Panel>

        <!-- 多项填空区域 -->
        <asp:Panel ID="pnlMultipleBlank" runat="server" CssClass="form-group" Visible="false">
            <label><span class="required">*</span> 填空设置</label>
            <p class="help-text">每行一个填空，格式：答案（必填）|提示文本</p>
            <asp:TextBox ID="txtMultipleBlank" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" placeholder="答案1|提示文本1&#10;答案2|提示文本2"></asp:TextBox>
        </asp:Panel>

        <!-- 打分题区域 -->
        <asp:Panel ID="pnlScore" runat="server" CssClass="form-group" Visible="false">
            <label>打分设置</label>
            <div class="form-row">
                <div class="form-group">
                    <label>最小值</label>
                    <asp:TextBox ID="txtScoreMin" runat="server" CssClass="form-control" Text="1"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>最大值</label>
                    <asp:TextBox ID="txtScoreMax" runat="server" CssClass="form-control" Text="5"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>默认值</label>
                    <asp:TextBox ID="txtScoreDefault" runat="server" CssClass="form-control" Text="3"></asp:TextBox>
                </div>
            </div>
        </asp:Panel>

        <!-- NPS评分区域 -->
        <asp:Panel ID="pnlNps" runat="server" CssClass="form-group" Visible="false">
            <label>NPS评分设置</label>
            <div class="form-row">
                <div class="form-group">
                    <label>低分文案</label>
                    <asp:TextBox ID="txtNpsLow" runat="server" CssClass="form-control" Text="不满意"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>高分文案</label>
                    <asp:TextBox ID="txtNpsHigh" runat="server" CssClass="form-control" Text="非常满意"></asp:TextBox>
                </div>
            </div>
        </asp:Panel>

        <!-- 矩阵题区域 -->
        <asp:Panel ID="pnlMatrix" runat="server" CssClass="form-group" Visible="false">
            <label><span class="required">*</span> 矩阵设置</label>
            <div class="form-row">
                <div class="form-group">
                    <label>行标题（逗号分隔）</label>
                    <asp:TextBox ID="txtMatrixRows" runat="server" CssClass="form-control" placeholder="项目1,项目2,项目3"></asp:TextBox>
                </div>
                <div class="form-group">
                    <label>列标题（逗号分隔）</label>
                    <asp:TextBox ID="txtMatrixCols" runat="server" CssClass="form-control" placeholder="选项A,选项B,选项C"></asp:TextBox>
                </div>
            </div>
            <div class="form-group" style="margin-top:10px;">
                <label>正确答案（JSON格式）</label>
                <p class="help-text">如：{"项目1":"选项A","项目2":"选项B"}</p>
                <asp:TextBox ID="txtMatrixAnswer" runat="server" CssClass="form-control" placeholder='{"行1":"列2","行2":"列1"}'></asp:TextBox>
            </div>
        </asp:Panel>

        <!-- 连线题区域 -->
        <asp:Panel ID="pnlMatching" runat="server" CssClass="form-group" Visible="false">
            <label><span class="required">*</span> 连线设置</label>
            <p class="help-text">每行一对，格式：左边项=右边项</p>
            <asp:TextBox ID="txtMatching" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" placeholder="Python=Guido van Rossum&#10;Java=James Gosling&#10;C++=Bjarne Stroustrup"></asp:TextBox>
        </asp:Panel>

        <!-- 分类题区域 -->
        <asp:Panel ID="pnlSorting" runat="server" CssClass="form-group" Visible="false">
            <label><span class="required">*</span> 分类设置</label>
            <p class="help-text">格式：类别名:选项1,选项2,选项3（每行一个类别）</p>
            <asp:TextBox ID="txtSorting" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" placeholder="编程语言:Python,Java,C++&#10;数据库:MySQL,MongoDB,Redis"></asp:TextBox>
        </asp:Panel>
        </section>

        <section class="form-card">
        <h3 class="section-title">补充信息</h3>
        <div class="form-group">
            <label>答案解析</label>
            <asp:TextBox ID="txtAnalysis" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="答案解析（可选）"></asp:TextBox>
        </div>

        <div class="form-row">
            <div class="form-group">
                <label>知识点</label>
                <asp:TextBox ID="txtKnowledge" runat="server" CssClass="form-control" placeholder="如：函数、循环"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>标签</label>
                <asp:TextBox ID="txtTags" runat="server" CssClass="form-control" placeholder="多个标签用逗号分隔"></asp:TextBox>
            </div>
        </div>

        <!-- 隐藏字段，用于存储JavaScript收集的正确答案和选项图片 -->
        <asp:HiddenField ID="hfCorrectAnswers" runat="server" />
        <asp:HiddenField ID="hfOptionImages" runat="server" />

        <div class="form-actions">
            <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="page-btn page-btn-primary" OnClick="btnSave_Click" />
            <asp:Button ID="btnSaveAdd" runat="server" Text="保存并继续添加" CssClass="page-btn page-btn-success" OnClick="btnSaveAdd_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="取消" CssClass="page-btn page-btn-secondary" OnClick="btnCancel_Click" />
        </div>
        </section>

        <script type="text/javascript">
            var optionImages = {};

            function uploadImage(file, successCallback, errorCallback) {
                var formData = new FormData();
                formData.append('file', file);
                
                $.ajax({
                    url: '/webform/imageupload.aspx',
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false,
                    success: function(response) {
                        if (response && response.url) {
                            successCallback(response.url);
                        } else if (typeof response === 'string') {
                            successCallback(response);
                        } else {
                            if (errorCallback) {
                                errorCallback('服务器返回格式错误');
                            } else {
                                alert('图片上传失败');
                            }
                        }
                    },
                    error: function() {
                        if (errorCallback) {
                            errorCallback('网络错误');
                        } else {
                            alert('图片上传失败');
                        }
                    }
                });
            }

            function handleOptionImageUpload(label) {
                var input = document.createElement('input');
                input.type = 'file';
                input.accept = 'image/*';
                input.onchange = function(e) {
                    var file = e.target.files[0];
                    if (file) {
                        uploadImage(file, function(imageUrl) {
                            if (imageUrl) {
                                optionImages[label] = imageUrl;
                                updateOptionImagePreview(label, imageUrl);
                            }
                        });
                    }
                };
                input.click();
            }

            function updateOptionImagePreview(label, imageUrl) {
                var wrapper = document.querySelector('[data-label="' + label + '"]');
                if (wrapper) {
                    var preview = wrapper.querySelector('.option-image-preview');
                    var deleteBtn = wrapper.querySelector('.option-image-btn.delete-btn');
                    var uploadBtn = wrapper.querySelector('.option-image-btn:not(.delete-btn)');
                    
                    if (preview) {
                        preview.src = imageUrl;
                        preview.style.display = 'block';
                    }
                    if (deleteBtn) {
                        deleteBtn.style.display = 'inline-block';
                    }
                    if (uploadBtn) {
                        uploadBtn.textContent = '更换';
                    }
                }
            }

            function removeOptionImage(label) {
                delete optionImages[label];
                var wrapper = document.querySelector('[data-label="' + label + '"]');
                if (wrapper) {
                    var preview = wrapper.querySelector('.option-image-preview');
                    var deleteBtn = wrapper.querySelector('.option-image-btn.delete-btn');
                    var uploadBtn = wrapper.querySelector('.option-image-btn:not(.delete-btn)');
                    
                    if (preview) {
                        preview.src = '';
                        preview.style.display = 'none';
                    }
                    if (deleteBtn) {
                        deleteBtn.style.display = 'none';
                    }
                    if (uploadBtn) {
                        uploadBtn.textContent = '📷';
                    }
                }
            }

            function collectCorrectAnswers() {
                var hfContent = document.getElementById('<%= hfContent.ClientID %>');
                if (hfContent) {
                    hfContent.value = $('#summernote-content-question').summernote('code');
                }

                var hfImages = document.getElementById('<%= hfOptionImages.ClientID %>');
                if (hfImages) {
                    hfImages.value = JSON.stringify(optionImages);
                }

                var allRadios = document.querySelectorAll('input[type="radio"]');
                var allCheckboxes = document.querySelectorAll('input[type="checkbox"]');
                var correctAnswers = [];

                allRadios.forEach(function(rb) {
                    if (rb.checked) {
                        var match = rb.id.match(/rb_([A-H])$/);
                        if (match) {
                            correctAnswers.push(match[1]);
                        }
                    }
                });

                allCheckboxes.forEach(function(cb) {
                    if (cb.checked) {
                        var match = cb.id.match(/cb_([A-H])$/);
                        if (match) {
                            correctAnswers.push(match[1]);
                        }
                    }
                });

                var hfCorrectAnswers = document.getElementById('<%= hfCorrectAnswers.ClientID %>');
                if (hfCorrectAnswers) {
                    hfCorrectAnswers.value = correctAnswers.join(',');
                }

                return true;
            }

            $(document).ready(function() {
                $('#summernote-content-question').summernote({
                    placeholder: '点击此处编辑题目内容...',
                    height: 120,
                    minHeight: 80,
                    maxHeight: 300,
                    lang: 'zh-CN',
                    disableDragAndDrop: false,
                    fontSizes: ['10', '11', '12', '14', '16', '18', '20', '24'],
                    toolbar: [
                        ['style', ['bold', 'italic', 'clear']],
                        ['insert', ['customPicture', 'customVideo']],
                        ['code', ['codeblock']],
                        ['misc', ['pastetext']],
                        ['misc', ['blank']],
                        ['view', ['undo', 'redo']]
                    ],
                    buttons: {
                        customPicture: function(context) {
                            var ui = $.summernote.ui;
                            var button = ui.button({
                                contents: '<i class="note-icon-picture"></i>',
                                tooltip: '插入图片',
                                click: function() {
                                    var fileInput = document.createElement('input');
                                    fileInput.type = 'file';
                                    fileInput.accept = 'image/*';
                                    fileInput.style.display = 'none';
                                    
                                    fileInput.onchange = function() {
                                        var file = this.files[0];
                                        if (file) {
                                            uploadImage(file, function(response) {
                                                var imageHtml = '<img src="' + response + '" >';
                                                context.invoke('editor.pasteHTML', imageHtml);
                                            }, function(error) {
                                                alert('图片上传失败：' + error);
                                            });
                                        }
                                    };
                                    
                                    document.body.appendChild(fileInput);
                                    fileInput.click();
                                    setTimeout(function() { document.body.removeChild(fileInput); }, 1000);
                                }
                            });
                            return button.render();
                        },
                        customVideo: function(context) {
                            var ui = $.summernote.ui;
                            var button = ui.button({
                                contents: '<i class="note-icon-video"></i>',
                                tooltip: '插入视频',
                                click: function() {
                                    var videoUrl = prompt('请输入视频地址（支持优酷、腾讯、YouTube等）：', 'http://');
                                    if (videoUrl && videoUrl.trim()) {
                                        var videoHtml = '<iframe src="' + videoUrl + '" width="100%" height="200" frameborder="0" allowfullscreen></iframe>';
                                        context.invoke('editor.pasteHTML', videoHtml);
                                    }
                                }
                            });
                            return button.render();
                        },
                        blank: function(context) {
                            var ui = $.summernote.ui;
                            var button = ui.button({
                                contents: '<i class="note-icon-pencil"></i>',
                                tooltip: '插入填空符',
                                click: function() {
                                    context.invoke('editor.focus');
                                    setTimeout(function() {
                                        try {
                                            context.invoke('editor.insertText', '___');
                                        } catch (e) {
                                            console.error('插入填空符失败:', e);
                                        }
                                    }, 0);
                                }
                            });
                            return button.render();
                        }
                    },
                    popover: {
                        image: [],
                        link: [],
                        video: [],
                        air: []
                    },
                    shortcuts: false,
                    followingToolbar: false
                });

                var btnSave = document.getElementById('<%= btnSave.ClientID %>');
                var btnSaveAdd = document.getElementById('<%= btnSaveAdd.ClientID %>');

                if (btnSave) {
                    btnSave.addEventListener('click', function(e) {
                        collectCorrectAnswers();
                    });
                }
                if (btnSaveAdd) {
                    btnSaveAdd.addEventListener('click', function(e) {
                        collectCorrectAnswers();
                    });
                }
            });
        </script>
        </div>
    </div>
</asp:Content>
