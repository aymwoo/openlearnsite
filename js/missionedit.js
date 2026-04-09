var kindEditorObj;
            var wangEditorObj;
            var vditorObj;
            var currentEditor = 'kindeditor';
            var lastVditorMarkdown = null;
            var lastVditorHtml = '';
            var vditorPasteMode = 'keep';

            var cid= window.__missioneditConfig.myCid;
            var ty="Course";
            var upjs= '../kindeditor/aspnet/upload_json.aspx?cid='+cid+'&Ty='+ty;
            var fmjs='../kindeditor/aspnet/file_manager_json.aspx?cid='+cid+'&Ty='+ty;

		    KindEditor.ready(function (K) {
		        kindEditorObj = K.create('textarea[name="ctl00$Content$mcontent"]', {
		            resizeType: 1,
		            newlineTag: "br", 
				uploadJson : upjs,
				fileManagerJson : fmjs,
				allowFileManager : true,
				filterMode : false,
					afterCreate : function() {
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
                controls.style.display = type === 'vditor' ? 'inline-flex' : 'none';
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
                    return;
                }

                navigator.clipboard.readText().then(function (text) {
                    if (!text) {
                        return;
                    }

                    var textarea = getVditorTextarea();
                    if (!textarea) {
                        return;
                    }

                    insertTextAtCursor(textarea, text);
                    textarea.dispatchEvent(new Event('input', { bubbles: true }));
                }, function () {
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
                var mcontent = document.getElementById(window.__missioneditConfig.mcontentId);
                if (!selector || !mcontent) return;
                if (isLikelyMarkdown(mcontent.value)) {
                    selector.value = 'vditor';
                    switchEditor('vditor');
                }
            }

            function initWangEditor() {
                if (wangEditorObj) return;
                const { createEditor, createToolbar } = window.wangEditor;
                const mcontent = document.getElementById(window.__missioneditConfig.mcontentId);

                wangEditorObj = createEditor({
                    selector: '#wangeditor-text',
                    html: kindEditorObj ? kindEditorObj.html() : mcontent.value,
                        config: {
                            placeholder: '请输入内容...',
                            MENU_CONF: {
                                uploadImage: {
                                    server: upjs,
                                    customInsert(res, insertFn) {
                                        if (res.error === 0) {
                                            insertFn(res.url);
                                        } else {
                                            alert(res.message || '图片上传失败');
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
                                            alert(res.message || '附件上传失败');
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
                                            alert(res.message || '文件上传失败');
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
                } catch(e) {
                    return html;
                }
            }

            function initVditor() {
                if (vditorObj) return;
                const mcontent = document.getElementById(window.__missioneditConfig.mcontentId);
                let initialContent = getPreferredVditorValue(lastVditorMarkdown !== null ? lastVditorMarkdown : mcontent.value);

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
                var mcontent = document.getElementById(window.__missioneditConfig.mcontentId);
                var payload = document.getElementById('editorContentPayload');
                var syncSourceField = document.getElementById('editorSyncSource');
                var syncLengthField = document.getElementById('editorSyncLength');
                if (!mcontent) {
                    return true;
                }

                var content = '';
                var syncSource = currentEditor + '-empty';
                if (currentEditor === 'kindeditor') {
                    if (kindEditorObj) {
                        kindEditorObj.sync();
                        content = kindEditorObj.html() || '';
                        syncSource = 'kindeditor-html';
                    }
                } else if (currentEditor === 'wangeditor') {
                    if (wangEditorObj) {
                        content = wangEditorObj.getHtml() || '';
                        syncSource = 'wangeditor-html';
                    }
                } else if (currentEditor === 'vditor') {
                    if (vditorObj) {
                        var liveTextarea = getVditorTextarea();
                        rememberVditorState();
                        content = lastVditorMarkdown || vditorObj.getValue() || ((liveTextarea && liveTextarea.value) ? liveTextarea.value : '') || vditorObj.getHTML() || '';
                        syncSource = lastVditorMarkdown ? 'vditor-markdown' : ((liveTextarea && liveTextarea.value) ? 'vditor-dom' : 'vditor-html');
                    }
                }

                mcontent.value = content;
                if (payload) {
                    payload.value = content;
                }
                if (syncSourceField) {
                    syncSourceField.value = syncSource;
                }
                if (syncLengthField) {
                    syncLengthField.value = String((content || '').length);
                }
                return true;
            }
