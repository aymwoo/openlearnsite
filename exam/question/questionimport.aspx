<%@ Page Language="C#" AutoEventWireup="true" CodeFile="questionimport.aspx.cs" Inherits="exam_question_questionimport" MasterPageFile="~/teacher/Teach.master" %><asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/webform/jquery-3.6.0.min.js" type="text/javascript"></script>
    <script src="/webform/bootstrap.bundle.min.js" type="text/javascript"></script>
    <link href="/webform/summernote-bs5.min.css" rel="stylesheet" />
    <link href="/webform/paper.css" rel="stylesheet" />
    <script src="/webform/summernote-bs5.min.js"></script>
    <script src="/webform/summernote-zh-CN.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <style>
        .import-page { min-height: calc(100vh - 8rem); padding: 1.5rem; background: #f8fafc; }
        .import-shell { max-width: 1100px; margin: 0 auto; display: flex; flex-direction: column; gap: 1.25rem; }
        .import-hero, .help-box, .import-form, .result-box, .error-panel { border: 1px solid rgba(148, 163, 184, 0.18); border-radius: 1.25rem; background: #ffffff; box-shadow: 0 12px 32px -28px rgba(15, 23, 42, 0.28); }
        .import-hero { display: flex; justify-content: space-between; align-items: flex-start; gap: 1rem; padding: 1.5rem; background: linear-gradient(135deg, #ffffff 0%, #f8fafc 100%); }
        .import-title { margin: 0; color: #0f172a; font-size: 1.625rem; font-weight: 700; }
        .import-subtitle { margin: 0.75rem 0 0; color: #475569; font-size: 0.95rem; line-height: 1.7; }
        .hero-actions { display: flex; flex-wrap: wrap; gap: 0.75rem; }
        .page-btn, .form-actions input, .hero-actions a { display: inline-flex; align-items: center; justify-content: center; min-height: 2.75rem; padding: 0 1rem; border: 1px solid transparent; border-radius: 0.9rem; font-size: 0.875rem; font-weight: 600; text-decoration: none; cursor: pointer; transition: all 0.2s ease; }
        .page-btn-primary, .form-actions input[id$='btnImport'] { background: #2563eb; color: #ffffff; box-shadow: 0 10px 20px -14px rgba(37, 99, 235, 0.85); }
        .page-btn-secondary, .form-actions input[id$='btnClear'], .form-actions input[id$='btnContinue'], .hero-actions a { background: #ffffff; color: #475569; border-color: #cbd5e1; }
        .help-box, .import-form, .error-panel { padding: 1.5rem; }
        .help-box h4 { margin: 0 0 1rem 0; color: #0f172a; font-size: 1rem; }
        .help-box p { margin: 0.35rem 0; color: #475569; font-size: 0.88rem; line-height: 1.7; }
        .help-box pre { background: #f8fafc; padding: 1rem; border-radius: 0.9rem; overflow-x: auto; font-size: 0.78rem; color: #334155; }
        .form-group { margin-bottom: 1rem; }
        .form-group label { display: block; margin-bottom: 0.45rem; color: #334155; font-weight: 600; }
        .form-group input, .form-group select, .form-group textarea { width: 100%; padding: 0.75rem 0.9rem; border: 1px solid #cbd5e1; border-radius: 0.9rem; font-size: 0.875rem; background: #f8fafc; box-sizing: border-box; }
        .form-group textarea { min-height: 320px; font-family: Consolas, Monaco, monospace; font-size: 0.82rem; }
        .form-actions { display: flex; flex-wrap: wrap; gap: 0.75rem; margin-top: 1.25rem; }
        .result-box { padding: 1.5rem; }
        .result-success { background: linear-gradient(135deg, #ecfdf5 0%, #f0fdf4 100%); color: #15803d; }
        .result-error { background: linear-gradient(135deg, #fef2f2 0%, #fff5f5 100%); color: #dc2626; }
        .result-warning { background: linear-gradient(135deg, #fffbeb 0%, #fffef7 100%); color: #d97706; }
        .stats { margin-top: 1rem; padding: 1rem; background: rgba(255,255,255,0.8); border-radius: 0.9rem; }
        .stats span { margin-right: 1.25rem; }
        .error-panel { margin-top: 1.25rem; }
        
        /* 图片上传相关样式 */
        .image-upload-section { margin-top: 1rem; padding: 1rem; background: #f8fafc; border-radius: 0.9rem; border: 1px dashed #cbd5e1; }
        .image-upload-section h5 { margin: 0 0 0.75rem; color: #334155; font-size: 0.9rem; }
        .image-list { display: flex; flex-wrap: wrap; gap: 0.5rem; margin-top: 0.5rem; }
        .image-item { position: relative; width: 80px; height: 60px; border: 1px solid #e2e8f0; border-radius: 4px; overflow: hidden; }
        .image-item img { width: 100%; height: 100%; object-fit: cover; }
        .image-item .remove-btn { position: absolute; top: 2px; right: 2px; width: 18px; height: 18px; background: rgba(220,38,38,0.9); color: #fff; border: none; border-radius: 50%; cursor: pointer; font-size: 10px; line-height: 18px; text-align: center; }
        .image-item .image-name { position: absolute; bottom: 0; left: 0; right: 0; background: rgba(0,0,0,0.6); color: #fff; font-size: 8px; padding: 2px; text-align: center; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
        .upload-btn { display: inline-flex; align-items: center; gap: 0.5rem; padding: 0.5rem 1rem; background: #3b82f6; color: #fff; border: none; border-radius: 0.5rem; cursor: pointer; font-size: 0.85rem; }
        .upload-btn:hover { background: #2563eb; }
        .image-code { margin-top: 0.5rem; padding: 0.5rem; background: #fff; border-radius: 4px; font-family: monospace; font-size: 0.8rem; color: #475569; }
        
        @media (max-width: 900px) { .import-page { padding: 1rem; } .import-hero { flex-direction: column; } }
    </style>

    <div class="import-page">
        <div class="import-shell">
        <section class="import-hero">
            <div>
                <h2 class="import-title"><asp:Literal ID="ltlBankName" runat="server"></asp:Literal> - 批量导入题目</h2>
                <p class="import-subtitle">支持按约定格式一次性导入多种题型，减少重复录入成本，并保持题库维护页的视觉一致性。</p>
            </div>
            <div class="hero-actions">
                <a href="questionlist.aspx?bankId=<%= BankId %>" class="page-btn page-btn-secondary">返回列表</a>
            </div>
        </section>

        <asp:Panel ID="pnlImport" runat="server">
            <div class="help-box">
                <h4>导入格式说明</h4>
                <p>每行一道题目，使用竖线 | 分隔各字段。格式如下：</p>
                <pre>题型|题目内容|选项|答案|解析|分值|难度|知识点</pre>
                <p><strong>题型：</strong></p>
                <p>1=单选，2=多选，3=判断，4=填空，5=简答</p>
                <p>6=连线，7=分类，8=组合，9=多项填空，10=下拉选择</p>
                <p>11=打分题，12=矩阵单选，13=矩阵多选，14=NPS评分</p>
                <p><strong>选项格式：</strong>选项之间用 ## 分隔，如：A.选项1##B.选项2##C.选项3##D.选项4</p>
                <p><strong>答案格式：</strong></p>
                <p> - 单选/判断：直接写选项字母，如 A 或 B</p>
                <p> - 多选：多个答案用逗号分隔，如 A,B,C</p>
                <p> - 填空：多个空用竖线分隔，如 答案1|答案2|答案3</p>
                <p> - 多项填空：答案用竖线分隔，如 答案1|答案2|答案3</p>
                <p> - 简答：直接写答案文本</p>
                <p> - 打分题：写默认分值，如 3</p>
                <p> - NPS：写默认分值，如 8</p>
                <p> - 矩阵题：JSON格式，如 {"行1":"列2","行2":"列1"}</p>
                <p><strong>分值：</strong>数字，如 2 或 5</p>
                <p><strong>难度：</strong>1=简单，2=中等，3=困难</p>
                <p><strong>知识点：</strong>可选字段</p>
                <br/>
                <p><strong>示例：</strong></p>
                <pre>1|以下哪个是C#的数据类型？|A. var##B. dynamic##C. both##D. none|C|C#支持var和dynamic两种类型|2|1|C#基础
2|以下哪些是面向对象的特性？|A. 封装##B. 继承##C. 多态##D. 以上都是|A,B,C|面向对象三大特性|3|2|面向对象
3|C#是一种面向对象的编程语言。||对||2|1|C#基础
4|C#中string是___类型，int是___类型。|引用|值|string是引用类型，int是值类型|4|2|C#数据类型
5|请简述C#中接口和抽象类的区别。||接口只定义契约不包含实现，抽象类可以包含部分实现...|10|3|C#高级特性
9|Python中___是列表，___是字典，___是集合。|list|dict|set|Python基础数据类型|4|2|Python基础
11|请对本次服务进行评分（1-5分）|1##2##3##4##5|4|满意度调查|5|1|客户服务
14|您有多大可能向朋友推荐我们的产品？||8|NPS评分题|5|1|用户调研</pre>
                <br/>
                <p><strong>图片格式说明：</strong></p>
                <p>选项中可以插入图片，格式为：<code>{img:图片代码}</code></p>
                <p>例如：<code>A. 这是选项文字{img:img001}|B. 另一个选项</code></p>
                <p>先上传图片获取代码，然后在选项中使用该代码</p>
            </div>

            <div class="image-upload-section">
                <h5>📷 图片上传（用于选项图片）</h5>
                <input type="file" id="imageUploadInput" accept="image/*" multiple style="display:none;">
                <button type="button" class="upload-btn" onclick="document.getElementById('imageUploadInput').click();">
                    📤 上传图片
                </button>
                <div class="image-list" id="uploadedImageList"></div>
                <div class="image-code" id="imageCodeDisplay" style="display:none;">
                    <strong>图片代码：</strong> <span id="currentImageCode"></span>
                    <button type="button" onclick="copyImageCode();" style="margin-left:10px;padding:2px 8px;background:#3b82f6;color:#fff;border:none;border-radius:3px;cursor:pointer;">复制</button>
                </div>
                <div style="margin-top:0.5rem;font-size:0.8rem;color:#64748b;">
                    上传图片后，复制图片代码粘贴到选项中使用。例如：A. 这是选项文字{img:img001}
                </div>
            </div>

            <div class="import-form">
                <div class="form-group">
                    <label>题目内容（每行一道题）</label>
                    <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" placeholder="请按照格式粘贴题目内容..."></asp:TextBox>
                </div>

                <div class="form-actions">
                    <asp:Button ID="btnImport" runat="server" Text="开始导入" CssClass="page-btn page-btn-primary" OnClick="btnImport_Click" />
                    <a href="questionlist.aspx?bankId=<%= BankId %>" class="page-btn page-btn-secondary">返回列表</a>
                    <asp:Button ID="btnClear" runat="server" Text="清空内容" CssClass="page-btn page-btn-secondary" OnClick="btnClear_Click" />
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlResult" runat="server" Visible="false">
            <div class="result-box" id="resultBox" runat="server">
                <asp:Literal ID="ltlResult" runat="server"></asp:Literal>
                <div class="stats" id="statsBox" runat="server">
                    <span>总数：<asp:Literal ID="ltlTotal" runat="server"></asp:Literal></span>
                    <span>成功：<asp:Literal ID="ltlSuccess" runat="server"></asp:Literal></span>
                    <span>失败：<asp:Literal ID="ltlFailed" runat="server"></asp:Literal></span>
                </div>
            </div>
            
            <div style="margin-top: 20px;">
                <a href="questionlist.aspx?bankId=<%= BankId %>" class="page-btn page-btn-primary">查看题目列表</a>
                <asp:Button ID="btnContinue" runat="server" Text="继续导入" CssClass="page-btn page-btn-secondary" OnClick="btnContinue_Click" />
            </div>

            <asp:Panel ID="pnlErrorDetails" runat="server" Visible="false" CssClass="error-panel">
                <h4>错误详情：</h4>
                <asp:Literal ID="ltlErrorDetails" runat="server"></asp:Literal>
            </asp:Panel>
        </asp:Panel>
        </div>
    </div>

    <script type="text/javascript">
        var uploadedImages = {};
        var imageCounter = 0;
        
        function getImageUrl(fileName) {
            if (!fileName) return '';
            if (fileName.indexOf('/') >= 0) return fileName;
            return '../webform/uploads/' + fileName;
        }
        
        function uploadImageFile(file, onSuccess, onError) {
            if (!file || !file.type.startsWith('image/')) {
                if (onError) onError('请选择有效的图片文件');
                return;
            }
            if (file.size > 5 * 1024 * 1024) {
                if (onError) onError('图片大小不能超过5MB');
                return;
            }
            
            var formData = new FormData();
            formData.append('file', file);
            
            var xhr = new XMLHttpRequest();
            xhr.open('POST', '../webform/upimg.ashx?action=image', true);
            
            xhr.onload = function() {
                if (xhr.status === 200) {
                    var response = xhr.responseText.trim();
                    if (response && response.indexOf('ERROR') !== 0) {
                        if (onSuccess) onSuccess(response);
                    } else {
                        if (onError) onError(response || '上传失败');
                    }
                } else {
                    if (onError) onError('上传失败');
                }
            };
            
            xhr.onerror = function() {
                if (onError) onError('网络错误');
            };
            
            xhr.send(formData);
        }
        
        function generateImageCode() {
            imageCounter++;
            return 'img' + String(imageCounter).padStart(3, '0');
        }
        
        function addImageToList(code, fileName) {
            var list = document.getElementById('uploadedImageList');
            var item = document.createElement('div');
            item.className = 'image-item';
            item.id = 'image-item-' + code;
            item.innerHTML = '<img src="' + getImageUrl(fileName) + '" alt="' + code + '">' +
                '<button type="button" class="remove-btn" onclick="removeImage(\'' + code + '\')">×</button>' +
                '<div class="image-name">' + code + '</div>';
            list.appendChild(item);
            
            uploadedImages[code] = fileName;
            showImageCode(code);
        }
        
        function showImageCode(code) {
            var display = document.getElementById('imageCodeDisplay');
            var codeSpan = document.getElementById('currentImageCode');
            display.style.display = 'block';
            codeSpan.innerText = '{img:' + code + '}';
        }
        
        function copyImageCode() {
            var codeSpan = document.getElementById('currentImageCode');
            var code = codeSpan.innerText;
            if (navigator.clipboard) {
                navigator.clipboard.writeText(code).then(function() {
                    alert('已复制: ' + code);
                });
            } else {
                var input = document.createElement('input');
                input.value = code;
                document.body.appendChild(input);
                input.select();
                document.execCommand('copy');
                document.body.removeChild(input);
                alert('已复制: ' + code);
            }
        }
        
        function removeImage(code) {
            var item = document.getElementById('image-item-' + code);
            if (item) item.remove();
            delete uploadedImages[code];
        }
        
        document.getElementById('imageUploadInput').addEventListener('change', function(e) {
            var files = e.target.files;
            if (!files || files.length === 0) return;
            
            for (var i = 0; i < files.length; i++) {
                (function(file) {
                    uploadImageFile(file, function(response) {
                        var code = generateImageCode();
                        addImageToList(code, response);
                        console.log('图片上传成功:', code, response);
                    }, function(error) {
                        alert('图片上传失败：' + error);
                        console.error('图片上传失败:', error);
                    });
                })(files[i]);
            }
            
            e.target.value = '';
        });
        
        console.log('questionimport.aspx 图片上传功能已加载');
    </script>
</asp:Content>
