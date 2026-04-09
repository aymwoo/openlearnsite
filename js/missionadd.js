var missionToastTimer = null;
                    var missionToastIcons = {
                        success: '<svg viewBox="0 0 20 20" fill="none" stroke="currentColor" stroke-width="2"><path d="M16.5 5.5 8 14 3.5 9.5" stroke-linecap="round" stroke-linejoin="round"></path></svg>',
                        error: '<svg viewBox="0 0 20 20" fill="none" stroke="currentColor" stroke-width="2"><path d="M10 6v4" stroke-linecap="round"></path><circle cx="10" cy="13.5" r="0.8" fill="currentColor" stroke="none"></circle><path d="M8.7 3.8 2.9 14a1.5 1.5 0 0 0 1.3 2.2h11.6a1.5 1.5 0 0 0 1.3-2.2L11.3 3.8a1.5 1.5 0 0 0-2.6 0Z" stroke-linejoin="round"></path></svg>',
                        info: '<svg viewBox="0 0 20 20" fill="none" stroke="currentColor" stroke-width="2"><path d="M10 8.2v4.3" stroke-linecap="round"></path><circle cx="10" cy="5.6" r="0.9" fill="currentColor" stroke="none"></circle><circle cx="10" cy="10" r="7" stroke-linecap="round"></circle></svg>'
                    };

                    function showToast(message, type) {
                        var toast = document.getElementById('mission-toast');
                        var toastIcon = document.getElementById('mission-toast-icon');
                        var toastMessage = document.getElementById('mission-toast-message');
                        var toastType = type === 'success' ? 'success' : (type === 'info' ? 'info' : 'error');
                        var duration = toastType === 'success' ? 2200 : (toastType === 'info' ? 2400 : 3400);

                        if (!toast || !message) {
                            return;
                        }

                        toast.className = 'mission-toast';
                        toast.classList.add('is-visible');
                        toast.classList.add('is-' + toastType);

                        if (toastIcon) {
                            toastIcon.innerHTML = missionToastIcons[toastType];
                        }

                        if (toastMessage) {
                            toastMessage.textContent = message;
                        }

                        if (missionToastTimer) {
                            clearTimeout(missionToastTimer);
                        }

                        missionToastTimer = setTimeout(function () {
                            toast.className = 'mission-toast';
                        }, duration);
                    }

                    var kindEditorObj;
                    var wangEditorObj;
                    var vditorObj;
                    var currentEditor = 'kindeditor';
                    var lastVditorMarkdown = null;
                    var lastVditorHtml = '';
                    var vditorPasteMode = 'keep';

                    var cid = window.__missionaddConfig.myCid;
                    var ty = "Course";
                    var upjs = '../kindeditor/aspnet/upload_json.aspx?cid=' + cid + '&Ty=' + ty;
                    var fmjs = '../kindeditor/aspnet/file_manager_json.aspx?cid=' + cid + '&Ty=' + ty;

                    KindEditor.ready(function (K) {
                        kindEditorObj = K.create('textarea[name="textareaItem"]', {
                            width: '100%',
                            resizeType: 1,
                            newlineTag: "br",
                            uploadJson: upjs,
                            fileManagerJson: fmjs,
                            allowFileManager: true,
                            filterMode: false,
                            afterCreate: function () {
                                this.loadPlugin('autoheight');
                                window.setTimeout(autoSelectInitialEditor, 0);
                            }
                        });
                    });

                    function isProbablyHtml(content) {
                        return /<\/?[a-z][\s\S]*>/i.test(content || '');
                    }

                    function isLikelyMarkdown(content) {
                        if (!content) return false;
                        return /```/.test(content)
                            || /^#{1,6}\s/m.test(content)
                            || /^\s*[-*+]\s/m.test(content)
                            || /^\s*\d+\.\s/m.test(content)
                            || /\[[^\]]+\]\([^)]+\)/.test(content);
                    }

                    function normalizeEditorContent(content) {
                        return (content || '').replace(/\s+/g, ' ').trim();
                    }

                    function getPreferredVditorValue(content) {
                        if (!content) return '';
                        return isProbablyHtml(content) ? safeHtml2Md(content) : content;
                    }

                    function rememberVditorState() {
                        if (!vditorObj) return;
                        lastVditorMarkdown = vditorObj.getValue() || '';
                        lastVditorHtml = vditorObj.getHTML() || '';
                        if (!lastVditorMarkdown && lastVditorHtml) {
                            lastVditorMarkdown = safeHtml2Md(lastVditorHtml);
                        }
                    }

                    function getSelectedVditorPasteMode() {
                        var checked = document.querySelector('input[name="vditorPasteMode"]:checked');
                        return checked ? checked.value : 'keep';
                    }

                    function updateVditorPasteControls(type) {
                        var controls = document.getElementById('vditorPasteControls');
                        if (!controls) return;
                        controls.style.display = type === 'vditor' ? 'flex' : 'none';
                    }

                    function insertTextAtCursor(target, text) {
                        if (!target) return;
                        var start = target.selectionStart || 0;
                        var end = target.selectionEnd || 0;
                        var value = target.value || '';
                        target.value = value.slice(0, start) + text + value.slice(end);
                        var cursor = start + text.length;
                        target.selectionStart = cursor;
                        target.selectionEnd = cursor;
                    }

                    function getVditorTextarea() {
                        var container = document.getElementById('vditor-container');
                        if (!container) return null;
                        return container.querySelector('.vditor-ir textarea, .vditor-sv textarea, .vditor-wysiwyg textarea');
                    }

                    function attachVditorPasteHandler() {
                        var textarea = getVditorTextarea();
                        if (!textarea || textarea.dataset.pasteBound === 'true') return;

                        textarea.dataset.pasteBound = 'true';
                        textarea.addEventListener('paste', function (event) {
                            vditorPasteMode = getSelectedVditorPasteMode();
                            if (vditorPasteMode !== 'plain') {
                                return;
                            }

                            var clipboard = event.clipboardData || window.clipboardData;
                            if (!clipboard) {
                                return;
                            }

                            var text = clipboard.getData('text/plain');
                            if (typeof text !== 'string') {
                                return;
                            }

                            event.preventDefault();
                            insertTextAtCursor(textarea, text);
                            textarea.dispatchEvent(new Event('input', { bubbles: true }));
                        });
                    }

                    function pastePlainTextToVditor() {
                        if (currentEditor !== 'vditor') {
                            showToast('请先切换到 Vditor 编辑器', 'info');
                            return;
                        }

                        navigator.clipboard.readText().then(function(text) {
                            if (!text) {
                                showToast('剪贴板里没有可粘贴的文本', 'info');
                                return;
                            }

                            var textarea = getVditorTextarea();
                            if (!textarea) {
                                showToast('当前还未找到 Vditor 输入区', 'error');
                                return;
                            }

                            insertTextAtCursor(textarea, text);
                            textarea.dispatchEvent(new Event('input', { bubbles: true }));
                            showToast('已按纯文本粘贴到 Vditor', 'success');
                        }, function() {
                            showToast('浏览器不允许读取剪贴板，请使用 Ctrl+Shift+V 或切换清理格式后直接粘贴', 'info');
                        });
                    }

                    window.pastePlainTextToVditor = pastePlainTextToVditor;

                    function shouldRestoreSavedMarkdown(currentHtml) {
                        if (lastVditorMarkdown === null) return false;
                        var currentNormalized = normalizeEditorContent(currentHtml);
                        var savedNormalized = normalizeEditorContent(lastVditorHtml);
                        return currentNormalized === '' || currentNormalized === savedNormalized;
                    }

                    function autoSelectInitialEditor() {
                        var selector = document.getElementById('editorSelector');
                        var ta = document.getElementsByName('textareaItem')[0];
                        if (!selector || !ta) return;
                        if (isLikelyMarkdown(ta.value)) {
                            selector.value = 'vditor';
                            switchEditor('vditor');
                        }
                    }

                    function initWangEditor() {
                        if (wangEditorObj) return;
                        const { createEditor, createToolbar } = window.wangEditor;
                        const ta = document.getElementsByName('textareaItem')[0];

                        wangEditorObj = createEditor({
                            selector: '#wangeditor-text',
                            html: kindEditorObj ? kindEditorObj.html() : (ta ? ta.value : ''),
                            config: {
                                placeholder: '请输入内容...',
                                MENU_CONF: {
                                    uploadImage: {
                                        server: upjs,
                                        customInsert(res, insertFn) {
                                            if (res.error === 0) {
                                                insertFn(res.url);
                                            } else {
                                                showToast(res.message || '图片上传失败', 'error');
                                            }
                                        }
                                    },
                                    uploadAttachment: {
                                        server: upjs,
                                        customInsert(res, insertFn) {
                                            if (res.error === 0) {
                                                if (wangEditorObj) {
                                                    LearnSiteEditorUploadHelper.insertUploadedLinkToWangEditor(wangEditorObj, res);
                                                }
                                            } else {
                                                showToast(res.message || '附件上传失败', 'error');
                                            }
                                        }
                                    },
                                    uploadFile: {
                                        server: upjs,
                                        customInsert(res, insertFn) {
                                            if (res.error === 0) {
                                                if (wangEditorObj) {
                                                    LearnSiteEditorUploadHelper.insertUploadedLinkToWangEditor(wangEditorObj, res);
                                                }
                                            } else {
                                                showToast(res.message || '文件上传失败', 'error');
                                            }
                                        }
                                    }
                                }
                            }
                        });

                        createToolbar({
                            editor: wangEditorObj,
                            selector: '#wangeditor-toolbar',
                            config: {}
                        });
                    }

                    let pendingVditorHtml = null;
                    let vditorReady = false;

                    function safeHtml2Md(html) {
                        try {
                            if (vditorObj && vditorObj.vditor && vditorObj.vditor.lute) {
                                return vditorObj.vditor.lute.HTML2Md(html);
                            }
                            var l = Lute.New();
                            return l.HTML2Md(html);
                        } catch (e) {
                            return html;
                        }
                    }

                    function initVditor() {
                        if (vditorObj) return;
                        const ta = document.getElementsByName('textareaItem')[0];
                        let initialContent = getPreferredVditorValue(lastVditorMarkdown !== null ? lastVditorMarkdown : (ta ? ta.value : ''));

                        vditorObj = new Vditor('vditor-container', {
                            height: 400,
                            width: '100%',
                            mode: 'ir',
                            upload: {
                                handler: function (files) {
                                    LearnSiteEditorUploadHelper.handleVditorUpload(vditorObj, upjs, files);
                                }
                            },
                            preview: {
                                mode: 'both'
                            },
                            cache: {
                                enable: false
                            },
                            after: () => {
                                vditorReady = true;
                                let contentToSet = pendingVditorHtml !== null ? getPreferredVditorValue(pendingVditorHtml) : initialContent;
                                vditorObj.setValue(contentToSet || '');
                                rememberVditorState();
                                pendingVditorHtml = null;
                                window.setTimeout(attachVditorPasteHandler, 0);
                            }
                        });
                    }

                    function switchEditor(type) {
                        currentEditor = type;
                        updateVditorPasteControls(type);
                        var kindContainer = document.querySelector('.ke-container');
                        var wangContainer = document.getElementById('wangeditor-wrap');
                        var vditorContainer = document.getElementById('vditor-wrap');

                        var currentHtml = '';
                        if (kindContainer && kindContainer.style.display !== 'none' && kindEditorObj) {
                            currentHtml = kindEditorObj.html();
                        } else if (wangContainer && wangContainer.style.display !== 'none' && wangEditorObj) {
                            currentHtml = wangEditorObj.getHtml();
                        } else if (vditorContainer && vditorContainer.style.display !== 'none' && vditorObj) {
                            rememberVditorState();
                            currentHtml = lastVditorHtml;
                        }

                        if (kindContainer) kindContainer.style.display = 'none';
                        if (wangContainer) wangContainer.style.display = 'none';
                        if (vditorContainer) vditorContainer.style.display = 'none';

                        if (type === 'kindeditor') {
                            if (kindContainer) kindContainer.style.display = 'block';
                            if (kindEditorObj && currentHtml) kindEditorObj.html(currentHtml);
                        } else if (type === 'wangeditor') {
                            if (wangContainer) {
                                wangContainer.style.display = 'block';
                            }
                            initWangEditor();
                            if (wangEditorObj && currentHtml) {
                                wangEditorObj.setHtml(currentHtml);
                            }
                        } else if (type === 'vditor') {
                            if (vditorContainer) {
                                vditorContainer.style.display = 'block';
                            }
                            var vditorContent = shouldRestoreSavedMarkdown(currentHtml) ? lastVditorMarkdown : getPreferredVditorValue(currentHtml);
                            if (!vditorObj) {
                                pendingVditorHtml = vditorContent;
                                initVditor();
                            } else if (vditorReady) {
                                vditorObj.setValue(vditorContent || '');
                                rememberVditorState();
                                window.setTimeout(attachVditorPasteHandler, 0);
                            } else {
                                pendingVditorHtml = vditorContent;
                            }
                        }
                    }

                    function syncContent() {
                        var ta = document.getElementsByName('textareaItem')[0];
                        var payload = document.getElementById('editorContentPayload');
                        if (!ta) {
                            return true;
                        }

                        var content = '';
                        if (currentEditor === 'kindeditor') {
                            if (kindEditorObj) {
                                kindEditorObj.sync();
                                content = kindEditorObj.html() || '';
                            }
                        } else if (currentEditor === 'wangeditor') {
                            if (wangEditorObj) {
                                content = wangEditorObj.getHtml() || '';
                            }
                        } else if (currentEditor === 'vditor') {
                            if (vditorObj) {
                                var liveTextarea = getVditorTextarea();
                                rememberVditorState();
                                content = lastVditorMarkdown || vditorObj.getValue() || ((liveTextarea && liveTextarea.value) ? liveTextarea.value : '') || vditorObj.getHTML() || '';
                            }
                        }

                        ta.value = content;
                        if (payload) {
                            payload.value = content;
                        }
                        return true;
                    }

function setAIProgress(percent, text, note) {
                        var progressWrap = document.getElementById('ai-progress-wrap');
                        var progressBar = document.getElementById('ai-progress-bar');
                        var progressText = document.getElementById('ai-progress-text');
                        var progressPercent = document.getElementById('ai-progress-percent');
                        var progressNote = document.getElementById('ai-progress-note');

                        progressWrap.style.display = 'block';
                        progressBar.style.width = percent + '%';
                        progressText.innerText = text;
                        progressPercent.innerText = percent + '%';
                        progressNote.innerText = note || '';
                    }

                    function resetAIProgress() {
                        var progressWrap = document.getElementById('ai-progress-wrap');
                        var progressBar = document.getElementById('ai-progress-bar');
                        var progressText = document.getElementById('ai-progress-text');
                        var progressPercent = document.getElementById('ai-progress-percent');
                        var progressNote = document.getElementById('ai-progress-note');

                        progressWrap.style.display = 'none';
                        progressBar.style.width = '0%';
                        progressText.innerText = '准备生成';
                        progressPercent.innerText = '0%';
                        progressNote.innerText = '输入提示词后，系统会调用当前默认 AI Provider 生成教学内容。';
                    }

                    function generateAIContent() {
                        var prompt = document.getElementById('ai-prompt').value.trim();
                        if (!prompt) {
                            showToast('请输入提示词', 'info');
                            return;
                        }
                        
                        var btn = document.getElementById('ai-generate-btn');
                        var btnText = document.getElementById('ai-btn-text');
                        var loading = document.getElementById('ai-loading');
                        var resultArea = document.getElementById('ai-result');
                        
                        btn.disabled = true;
                        btnText.innerText = '正在生成...';
                        loading.style.display = 'block';
                        resultArea.innerHTML = '<span style="color:#64748b;">生成中，结果完成后会显示在这里。</span>';
                        setAIProgress(10, '正在提交请求', '已将教学内容需求发送到 AI 服务，请稍候。');
                        
                        var xhr = new XMLHttpRequest();
                        xhr.timeout = 125000;
                        xhr.open("POST", "aiprovider_api.ashx", true);
                        xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                        xhr.onreadystatechange = function() {
                            if (xhr.readyState === 2) {
                                setAIProgress(45, '服务端处理中', '请求已送达，AI 正在分析提示词并生成内容。');
                                return;
                            }

                            if (xhr.readyState === 3) {
                                setAIProgress(75, '正在整理结果', '已收到返回数据，正在整理生成结果。');
                                return;
                            }

                            if (xhr.readyState === 4) {
                                btn.disabled = false;
                                btnText.innerText = '生成内容';
                                loading.style.display = 'none';
                                
                                if (xhr.status === 200) {
                                    try {
                                        var res = JSON.parse(xhr.responseText);
                                        if (res.success) {
                                            var text = res.data;
                                            resultArea.innerText = text;
                                            setAIProgress(100, '生成完成', 'AI 教学助手已返回内容，可复制或一键插入编辑器。');
                                            showToast('AI 内容生成完成', 'success');
                                        } else {
                                            resultArea.innerHTML = '';
                                            setAIProgress(100, '生成失败', 'AI Provider 已返回错误，请检查默认模型配置或稍后重试。');
                                            showToast(res.msg || '生成失败', 'error');
                                        }
                                    } catch (e) {
                                            resultArea.innerHTML = '';
                                            setAIProgress(100, '解析失败', '已收到响应，但结果格式不符合预期。');
                                        showToast('解析响应失败', 'error');
                                    }
                                } else {
                                    resultArea.innerHTML = '';
                                    setAIProgress(100, '请求失败', '接口请求未成功完成，请检查网络或服务端状态。');
                                    showToast('请求失败，状态码：' + xhr.status, 'error');
                                }
                            }
                        };
                        xhr.onerror = function() {
                            btn.disabled = false;
                            btnText.innerText = '生成内容';
                            loading.style.display = 'none';
                            resultArea.innerHTML = '';
                            setAIProgress(100, '网络异常', '未能连接到 AI Provider 接口，请检查网络或服务器配置。');
                            showToast('网络异常，无法连接 AI 接口', 'error');
                        };
                        xhr.ontimeout = function() {
                            btn.disabled = false;
                            btnText.innerText = '生成内容';
                            loading.style.display = 'none';
                            resultArea.innerHTML = '';
                            setAIProgress(100, '请求超时', 'AI 生成超过 125 秒未返回，可能是模型响应较慢、提示词较长或服务端繁忙。');
                            showToast('请求超时，请稍后重试', 'error');
                        };
                        xhr.send("action=chat&prompt=" + encodeURIComponent(prompt));
                    }
                    
                    function copyAIContent() {
                        var resultArea = document.getElementById('ai-result');
                        var text = resultArea.innerText;
                        if (!text || text.indexOf('生成中，结果完成后会显示在这里。') !== -1 || text.indexOf('错误：') === 0 || text.indexOf('请求失败') === 0 || text.indexOf('网络异常') === 0 || text.indexOf('请求超时') === 0 || text.indexOf('解析响应失败') === 0) {
                            showToast('没有可复制的内容', 'info');
                            return;
                        }
                        
                        navigator.clipboard.writeText(text).then(function() {
                            showToast('已复制到剪贴板', 'success');
                        }, function(err) {
                            showToast('复制失败: ' + err, 'error');
                        });
                    }
                    
                    function formatTextToHtml(text) {
                        return text.replace(/\n/g, '<br/>');
                    }
                    
                    function insertAIContent() {
                        var resultArea = document.getElementById('ai-result');
                        var text = resultArea.innerText;
                        if (!text || text.indexOf('生成中，结果完成后会显示在这里。') !== -1 || text.indexOf('错误：') === 0 || text.indexOf('请求失败') === 0 || text.indexOf('网络异常') === 0 || text.indexOf('请求超时') === 0 || text.indexOf('解析响应失败') === 0) {
                            showToast('没有可插入的内容', 'info');
                            return;
                        }
                        
                        if (currentEditor === 'kindeditor') {
                            if (kindEditorObj) {
                                kindEditorObj.insertHtml(formatTextToHtml(text));
                            }
                        } else if (currentEditor === 'wangeditor') {
                            if (wangEditorObj) {
                                // WangEditor V5
                                wangEditorObj.dangerouslyInsertHtml(formatTextToHtml(text));
                            }
                        } else if (currentEditor === 'vditor') {
                            if (vditorObj) {
                                vditorObj.insertValue(text);
                            }
                        }
                        showToast('已成功插入到编辑器', 'success');
                    }
