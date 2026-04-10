var kindEditorObj;
                    var wangEditorObj;
                    var vditorObj;
                    var currentEditor = 'kindeditor';
                    var lastVditorMarkdown = null;
                    var lastVditorHtml = '';
                    var vditorReady = false;
                    var pendingVditorHtml = null;
                    var cid = window.__courseeditConfig.myCid;
                    var ty = "Course";
                    var upjs = '../kindeditor/aspnet/upload_json.aspx?cid=' + cid + '&ty=' + ty;
                    var fmjs = '../kindeditor/aspnet/file_manager_json.aspx?cid=' + cid + '&ty=' + ty;

                    KindEditor.ready(function (K) {
                        kindEditorObj = K.create('textarea[name="ctl00$Content$mcontent"]', {
                            resizeType: 1,
                            newlineTag: "br",
                            cssPath: ['../kindeditor/plugins/code/prettify.css'],
                            uploadJson: upjs,
                            fileManagerJson: fmjs,
                            allowFileManager: true,
                            filterMode: false,
                            afterCreate: function () {
                                window.setTimeout(autoSelectInitialEditor, 0);
                            }});
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
                         lastVditorMarkdown = vditorObj.getValue();
                         lastVditorHtml = vditorObj.getHTML();
                     }

                     function shouldRestoreSavedMarkdown(currentHtml) {
                         if (lastVditorMarkdown === null) return false;
                         var currentNormalized = normalizeEditorContent(currentHtml);
                         var savedNormalized = normalizeEditorContent(lastVditorHtml);
                         return currentNormalized === '' || currentNormalized === savedNormalized;
                     }

                     function autoSelectInitialEditor() {
                         var selector = document.getElementById('editorSelector');
                         var field = document.getElementById(window.__courseeditConfig.mcontentId);
                         if (!selector || !field) return;
                         if (isLikelyMarkdown(field.value)) {
                             selector.value = 'vditor';
                             switchEditor('vditor');
                         }
                     }

                     function initWangEditor() {
                         if (wangEditorObj) return;
                         const { createEditor, createToolbar } = window.wangEditor;
                         const field = document.getElementById(window.__courseeditConfig.mcontentId);
                         wangEditorObj = createEditor({
                             selector: '#wangeditor-text',
                             html: kindEditorObj ? kindEditorObj.html() : (field ? field.value : ''),
                             config: {
                                 placeholder: '请输入内容...',
                                 MENU_CONF: {
                                     uploadImage: {
                                         server: upjs,
                                         customInsert(res, insertFn) {
                                             if (res.error === 0) insertFn(res.url);
                                             else alert(res.message || '图片上传失败');
                                         }
                                     },
                                     uploadAttachment: {
                                         server: upjs,
                                         customInsert(res) {
                                             if (res.error === 0) LearnSiteEditorUploadHelper.insertUploadedLinkToWangEditor(wangEditorObj, res);
                                             else alert(res.message || '附件上传失败');
                                         }
                                     },
                                     uploadFile: {
                                         server: upjs,
                                         customInsert(res) {
                                             if (res.error === 0) LearnSiteEditorUploadHelper.insertUploadedLinkToWangEditor(wangEditorObj, res);
                                             else alert(res.message || '文件上传失败');
                                         }
                                     }
                                 }
                             }
                         });
                         createToolbar({ editor: wangEditorObj, selector: '#wangeditor-toolbar', config: {} });
                     }

                     function safeHtml2Md(html) {
                         try {
                             if (vditorObj && vditorObj.vditor && vditorObj.vditor.lute) return vditorObj.vditor.lute.HTML2Md(html);
                             var l = Lute.New();
                             return l.HTML2Md(html);
                         } catch (e) {
                             return html;
                         }
                     }

                     function initVditor() {
                         if (vditorObj) return;
                         const field = document.getElementById(window.__courseeditConfig.mcontentId);
                         let initialContent = getPreferredVditorValue(lastVditorMarkdown !== null ? lastVditorMarkdown : (kindEditorObj ? kindEditorObj.html() : (field ? field.value : '')));
                         vditorObj = new Vditor('vditor-container', {
                             height: 400,
                                 width: '100%',
                             mode: 'ir',
                             upload: { handler: function (files) { LearnSiteEditorUploadHelper.handleVditorUpload(vditorObj, upjs, files); } },
                             preview: { mode: 'both' },
                             cache: { enable: false },
                             after: () => {
                                 vditorReady = true;
                                 let contentToSet = pendingVditorHtml !== null ? pendingVditorHtml : initialContent;
                                 vditorObj.setValue(contentToSet || '');
                                 rememberVditorState();
                                 pendingVditorHtml = null;
                             }
                         });
                     }

                     function switchEditor(type) {
                         currentEditor = type;
                         var kindContainer = document.querySelector('.ke-container');
                         var wangContainer = document.getElementById('wangeditor-wrap');
                         var vditorContainer = document.getElementById('vditor-wrap');
                         var currentHtml = '';
                         if (kindContainer && kindContainer.style.display !== 'none' && kindEditorObj) currentHtml = kindEditorObj.html();
                         else if (wangContainer && wangContainer.style.display !== 'none' && wangEditorObj) currentHtml = wangEditorObj.getHtml();
                         else if (vditorContainer && vditorContainer.style.display !== 'none' && vditorObj && vditorReady) {
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
                             if (wangContainer) wangContainer.style.display = 'block';
                             initWangEditor();
                             if (wangEditorObj && currentHtml) wangEditorObj.setHtml(currentHtml);
                         } else if (type === 'vditor') {
                             if (vditorContainer) vditorContainer.style.display = 'block';
                             var vditorContent = shouldRestoreSavedMarkdown(currentHtml) ? lastVditorMarkdown : getPreferredVditorValue(currentHtml);
                             if (!vditorObj) {
                                 pendingVditorHtml = vditorContent;
                                 initVditor();
                             } else if (vditorReady) {
                                 vditorObj.setValue(vditorContent || '');
                                 rememberVditorState();
                             } else {
                                 pendingVditorHtml = vditorContent;
                             }
                         }
                     }

function syncContent() {
                         var field = document.getElementById(window.__courseeditConfig.mcontentId);
                         if (!field) return true;
                         if (currentEditor === 'kindeditor') {
                             if (kindEditorObj) field.value = kindEditorObj.html();
                         } else if (currentEditor === 'wangeditor') {
                             if (wangEditorObj) field.value = wangEditorObj.getHtml();
                         } else if (currentEditor === 'vditor') {
                             if (vditorObj) {
                                 rememberVditorState();
                                 field.value = lastVditorMarkdown || '';
                             }
                         }
                          return true;
                      }

function getActivityPlanElements() {
                        return {
                            topic: document.getElementById('activity-plan-topic'),
                            grade: document.getElementById('activity-plan-grade'),
                            duration: document.getElementById('activity-plan-duration'),
                            goals: document.getElementById('activity-plan-goals'),
                            fields: document.getElementById('activity-plan-fields'),
                            toggle: document.getElementById('activity-plan-toggle'),
                            button: document.getElementById('activity-plan-generate-btn'),
                            buttonText: document.getElementById('activity-plan-btn-text'),
                            loading: document.getElementById('activity-plan-loading'),
                            result: document.getElementById('activity-plan-result')
                        };
                    }

                    function toggleActivityPlanFields() {
                        var elements = getActivityPlanElements();
                        if (!elements.fields || !elements.toggle) {
                            return;
                        }

                        var expanded = elements.fields.style.display !== 'none';
                        elements.fields.style.display = expanded ? 'none' : 'block';
                        elements.toggle.innerText = expanded ? '展开可选信息' : '收起可选信息';
                    }

                    function getCourseEditContentValue() {
                        syncContent();
                        var field = document.getElementById(window.__courseeditConfig.mcontentId);
                        return field ? (field.value || '') : '';
                    }

                    function getDefaultCourseEditGrade() {
                        var gradeSelector = document.getElementById(window.__courseeditConfig.gradeId);
                        if (!gradeSelector) {
                            return '';
                        }

                        var selectedText = '';
                        if (gradeSelector.selectedIndex >= 0 && gradeSelector.options[gradeSelector.selectedIndex]) {
                            selectedText = gradeSelector.options[gradeSelector.selectedIndex].text;
                        }

                        return (selectedText || gradeSelector.value || '').trim();
                    }

                    function setActivityPlanProgress(percent, text, note) {
                        var progressWrap = document.getElementById('activity-plan-progress-wrap');
                        var progressBar = document.getElementById('activity-plan-progress-bar');
                        var progressText = document.getElementById('activity-plan-progress-text');
                        var progressPercent = document.getElementById('activity-plan-progress-percent');
                        var progressNote = document.getElementById('activity-plan-progress-note');
                        if (!progressWrap || !progressBar || !progressText || !progressPercent || !progressNote) {
                            return;
                        }

                        progressWrap.style.display = 'block';
                        progressBar.style.width = percent + '%';
                        progressText.innerText = text;
                        progressPercent.innerText = percent + '%';
                        progressNote.innerText = note || '';
                    }

                    function resetActivityPlanProgress() {
                        setActivityPlanProgress(0, '准备生成', '输入主题后，系统会调用默认 AI Provider 生成活动计划。');
                    }

                    function setActivityPlanLoading(isLoading) {
                        var elements = getActivityPlanElements();
                        if (!elements.button || !elements.buttonText || !elements.loading) {
                            return;
                        }

                        elements.button.disabled = isLoading;
                        elements.buttonText.innerText = isLoading ? '正在生成...' : '生成活动计划';
                        elements.loading.style.display = isLoading ? 'block' : 'none';
                    }

                    function generateActivityPlan() {
                        var elements = getActivityPlanElements();
                        if (!elements.topic || !elements.result) {
                            return;
                        }

                        var topic = elements.topic.value.trim();
                        if (!topic) {
                            alert('请输入主题或知识点');
                            elements.topic.focus();
                            return;
                        }

                        var gradeValue = elements.grade ? elements.grade.value.trim() : '';
                        if (!gradeValue) {
                            gradeValue = getDefaultCourseEditGrade();
                            if (elements.grade) {
                                elements.grade.value = gradeValue;
                            }
                        }

                        var durationValue = elements.duration ? elements.duration.value.trim() : '';
                        var goalsValue = elements.goals ? elements.goals.value.trim() : '';
                        var existingCourseContent = getCourseEditContentValue();
                        var resultArea = elements.result;

                        setActivityPlanLoading(true);
                        resultArea.textContent = '生成中，结果完成后会显示在这里。';
                        setActivityPlanProgress(10, '正在提交请求', '已发送主题、结构化字段和当前学案内容。');

                        var xhr = new XMLHttpRequest();
                        xhr.timeout = 125000;
                        xhr.open('POST', 'aiprovider_api.ashx', true);
                        xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                        xhr.onreadystatechange = function () {
                            if (xhr.readyState === 2) {
                                setActivityPlanProgress(45, '服务端处理中', '活动计划助手正在整理课堂活动方案。');
                                return;
                            }
                            if (xhr.readyState === 3) {
                                setActivityPlanProgress(75, '正在整理结果', '已收到返回数据，正在整理显示内容。');
                                return;
                            }
                            if (xhr.readyState !== 4) {
                                return;
                            }

                            setActivityPlanLoading(false);
                            if (xhr.status === 200) {
                                try {
                                    var res = JSON.parse(xhr.responseText);
                                    if (res.success) {
                                        var text = res.data || '';
                                        resultArea.textContent = text;
                                        setActivityPlanProgress(100, '生成完成', '可直接复制到备课记录或继续手动调整。');
                                    } else {
                                        resultArea.textContent = '';
                                        setActivityPlanProgress(100, '生成失败', res.msg || 'AI Provider 返回错误，请稍后重试。');
                                        alert(res.msg || '生成失败');
                                    }
                                } catch (e) {
                                    resultArea.textContent = '';
                                    setActivityPlanProgress(100, '解析失败', '响应格式不符合预期。');
                                    alert('解析响应失败');
                                }
                            } else {
                                resultArea.textContent = '';
                                setActivityPlanProgress(100, '请求失败', '接口请求未成功完成，请检查网络或服务端状态。');
                                alert('请求失败，状态码：' + xhr.status);
                            }
                        };

                        xhr.onerror = function () {
                            setActivityPlanLoading(false);
                            resultArea.textContent = '';
                            setActivityPlanProgress(100, '网络异常', '未能连接到活动计划接口。');
                            alert('网络异常，无法连接活动计划接口');
                        };

                        xhr.ontimeout = function () {
                            setActivityPlanLoading(false);
                            resultArea.textContent = '';
                            setActivityPlanProgress(100, '请求超时', '活动计划生成超过 125 秒未返回。');
                            alert('请求超时，请稍后重试');
                        };

                        xhr.send('action=activityPlan'
                            + '&topic=' + encodeURIComponent(topic)
                            + '&grade=' + encodeURIComponent(gradeValue)
                            + '&duration=' + encodeURIComponent(durationValue)
                            + '&teachingGoals=' + encodeURIComponent(goalsValue)
                            + '&existingCourseContent=' + encodeURIComponent(existingCourseContent));
                    }

                    function copyActivityPlanResult() {
                        var elements = getActivityPlanElements();
                        if (!elements.result) {
                            return;
                        }

                        var text = (elements.result.textContent || '').trim();
                        if (!text || text === '生成中，结果完成后会显示在这里。') {
                            alert('没有可复制的内容');
                            return;
                        }

                        navigator.clipboard.writeText(text).then(function () {
                            alert('已复制到剪贴板');
                        }, function () {
                            alert('复制失败，请手动复制');
                        });
                    }

(function(){
        var config = window.__courseeditConfig || {};
        if (!window.LearnSiteCourseBanner) {
            return;
        }
        window.LearnSiteCourseBanner.init({
            triggerId: config.heroEditLinkId,
            targetId: config.shellId || 'EditShell',
            hiddenBannerUrlId: config.hiddenBannerUrlId,
            hiddenCourseId: config.hiddenCourseId,
            linkId: config.hLbannerId
        });
        resetActivityPlanProgress();
    })();
