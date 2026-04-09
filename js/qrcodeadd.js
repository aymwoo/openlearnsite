var editor;
            var wangEditorObj;
            var vditorObj;
            var currentEditor = 'kindeditor';
                    var lastVditorMarkdown = null;
                    var lastVditorHtml = '';
                    var vditorReady = false;
            var pendingVditorHtml = null;
            var cid= window.__qrcodeaddConfig.myCid;
            var ty="Course";
            var upjs= '../kindeditor/aspnet/upload_json.aspx?cid='+cid+'&ty='+ty;
            var fmjs='../kindeditor/aspnet/file_manager_json.aspx?cid='+cid+'&ty='+ty;
		    KindEditor.ready(function (K) {
		        editor = K.create('textarea[name="textareaItem"]', {
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
                 var field = document.getElementsByName('textareaItem')[0];
                 if (!selector || !field) return;
                 if (isLikelyMarkdown(field.value)) {
                     selector.value = 'vditor';
                     switchEditor('vditor');
                 }
             }

             function initWangEditor() {
                 if (wangEditorObj) return;
                 const { createEditor, createToolbar } = window.wangEditor;
                 const field = document.getElementsByName('textareaItem')[0];
                 wangEditorObj = createEditor({
                     selector: '#wangeditor-text',
                     html: editor ? editor.html() : (field ? field.value : ''),
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
                 const field = document.getElementsByName('textareaItem')[0];
                 let initialContent = getPreferredVditorValue(lastVditorMarkdown !== null ? lastVditorMarkdown : (editor ? editor.html() : (field ? field.value : '')));
                 vditorObj = new Vditor('vditor-container', {
                     height: 420,
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
                 if (kindContainer && kindContainer.style.display !== 'none' && editor) currentHtml = editor.html();
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
                     if (editor && currentHtml) editor.html(currentHtml);
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
                 var field = document.getElementsByName('textareaItem')[0];
                 if (!field) return true;
                 if (currentEditor === 'kindeditor') {
                     if (editor) field.value = editor.html();
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
