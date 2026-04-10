var kindEditorObj;
                    var wangEditorObj;
                    var vditorObj;
                    var currentEditor = 'kindeditor';
                    var lastVditorMarkdown = null;
                    var lastVditorHtml = '';
                     var vditorReady = false;
                     var pendingVditorHtml = null;
                     var lastActivityPlanDraftResponse = null;
                      var activityPlanSectionStates = {
                          teachingGoals: { isLoading: false, error: '' },
                          activitySteps: { isLoading: false, error: '' },
                          resources: { isLoading: false, error: '' },
                          assessment: { isLoading: false, error: '' },
                          teacherReminder: { isLoading: false, error: '' }
                      };
                      var activityPlanSelectionState = {
                          teachingGoals: false,
                          activitySteps: false,
                          resources: false,
                          assessment: false,
                          teacherReminder: false
                      };
                      var activityPlanDraftStatus = {
                          hasDraft: false,
                          updatedAt: ''
                      };
                      var activityPlanSectionLabels = {
                          teachingGoals: '教学目标',
                          activitySteps: '活动步骤',
                          resources: '教学资源',
                          assessment: '评价设计',
                          teacherReminder: '教师提醒'
                      };
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
                             result: document.getElementById('activity-plan-result'),
                             draftBanner: document.getElementById('activity-plan-draft-banner'),
                             draftBannerText: document.getElementById('activity-plan-draft-banner-text')
                         };
                      }

                      function clearActivityPlanResult() {
                          var elements = getActivityPlanElements();
                          if (!elements.result) {
                              return;
                          }

                          elements.result.textContent = '';
                          lastActivityPlanDraftResponse = null;
                          resetActivityPlanSectionStates();
                      }

                     function renderActivityPlanPlaceholder(message) {
                         var elements = getActivityPlanElements();
                         if (!elements.result) {
                             return;
                         }

                         elements.result.textContent = message || '';
                     }

                      function appendTextElement(parent, tagName, className, text) {
                         var element = document.createElement(tagName);
                         if (className) {
                             element.className = className;
                         }
                         element.textContent = text || '';
                         parent.appendChild(element);
                          return element;
                      }

                      function getActivityPlanSectionState(sectionKey) {
                          return activityPlanSectionStates[sectionKey] || { isLoading: false, error: '' };
                      }

                      function resetActivityPlanSectionStates() {
                           var keys = Object.keys(activityPlanSectionStates);
                           for (var i = 0; i < keys.length; i++) {
                               activityPlanSectionStates[keys[i]] = { isLoading: false, error: '' };
                           }
                       }

                      function resetActivityPlanSelections() {
                          var keys = Object.keys(activityPlanSelectionState);
                          for (var i = 0; i < keys.length; i++) {
                              activityPlanSelectionState[keys[i]] = false;
                          }
                      }

                      function getSelectedActivityPlanSections() {
                          var keys = Object.keys(activityPlanSelectionState);
                          var selected = [];
                          for (var i = 0; i < keys.length; i++) {
                              if (activityPlanSelectionState[keys[i]]) {
                                  selected.push(keys[i]);
                              }
                          }

                          return selected;
                      }

                      function toggleActivityPlanSectionSelection(sectionKey, isSelected) {
                          if (!Object.prototype.hasOwnProperty.call(activityPlanSelectionState, sectionKey)) {
                              return;
                          }

                          activityPlanSelectionState[sectionKey] = !!isSelected;
                      }

                      function getActivityPlanSectionLabel(sectionKey) {
                          return activityPlanSectionLabels[sectionKey] || sectionKey;
                      }

                      function escapeActivityPlanHtml(text) {
                          return (text || '')
                              .replace(/&/g, '&amp;')
                              .replace(/</g, '&lt;')
                              .replace(/>/g, '&gt;')
                              .replace(/"/g, '&quot;')
                              .replace(/'/g, '&#39;');
                      }

                      function setActivityPlanDraftStatus(hasDraft, updatedAt) {
                          activityPlanDraftStatus = {
                              hasDraft: !!hasDraft,
                              updatedAt: updatedAt || ''
                          };
                          renderActivityPlanDraftBanner();
                      }

                      function renderActivityPlanDraftBanner() {
                          var elements = getActivityPlanElements();
                          if (!elements.draftBanner || !elements.draftBannerText) {
                              return;
                          }

                          if (!activityPlanDraftStatus.hasDraft) {
                              elements.draftBanner.style.display = 'none';
                              return;
                          }

                          var bannerText = '当前课程存在已保存的活动计划草案，可选择继续编辑。';
                          if (activityPlanDraftStatus.updatedAt) {
                              bannerText = '当前课程存在已保存的活动计划草案，最近更新于 ' + activityPlanDraftStatus.updatedAt + '。';
                          }

                          elements.draftBannerText.textContent = bannerText;
                          elements.draftBanner.style.display = 'block';
                      }

                      function setActivityPlanSectionState(sectionKey, isLoading, error) {
                          if (!activityPlanSectionStates[sectionKey]) {
                              return;
                          }

                          activityPlanSectionStates[sectionKey] = {
                              isLoading: !!isLoading,
                              error: error || ''
                          };
                      }

                      function appendActivityPlanSectionHeader(card, title, sectionKey) {
                           var state = getActivityPlanSectionState(sectionKey);
                           var head = document.createElement('div');
                           head.className = 'activity-plan-card-head';
                           appendTextElement(head, 'h4', 'activity-plan-card-title', title);

                           var actions = document.createElement('div');
                           actions.className = 'activity-plan-card-actions';

                           var selectLabel = document.createElement('label');
                           selectLabel.className = 'activity-plan-section-select';
                           var checkbox = document.createElement('input');
                           checkbox.type = 'checkbox';
                           checkbox.checked = !!activityPlanSelectionState[sectionKey];
                           checkbox.onchange = function () {
                               toggleActivityPlanSectionSelection(sectionKey, checkbox.checked);
                           };
                           selectLabel.appendChild(checkbox);
                           selectLabel.appendChild(document.createTextNode('选择'));
                           actions.appendChild(selectLabel);

                           var button = document.createElement('button');
                           button.type = 'button';
                           button.className = 'activity-plan-section-action';
                           button.textContent = '应用本节';
                           button.disabled = false;
                           button.onclick = function () {
                               applySelectedActivityPlanSections([sectionKey]);
                           };
                           actions.appendChild(button);

                           button = document.createElement('button');
                           button.type = 'button';
                           button.className = 'activity-plan-section-action';
                           button.textContent = state.isLoading ? '正在重生成...' : '重生成本节';
                           button.disabled = state.isLoading;
                           button.onclick = function () {
                               regenerateActivityPlanSection(sectionKey);
                           };
                           actions.appendChild(button);
                           head.appendChild(actions);
                           card.appendChild(head);
                       }

                      function appendActivityPlanSectionStatus(card, sectionKey) {
                          var state = getActivityPlanSectionState(sectionKey);
                          if (state.isLoading) {
                              appendTextElement(card, 'div', 'activity-plan-section-status is-loading', '正在重生成当前章节，其余预览内容保持不变。');
                          } else if (state.error) {
                              appendTextElement(card, 'div', 'activity-plan-section-status is-error', state.error);
                          }
                      }

                      function renderActivityPlanListCard(container, title, items, sectionKey) {
                          if (!items || !items.length) {
                              return;
                          }

                          var card = document.createElement('section');
                          card.className = 'activity-plan-card';
                          appendActivityPlanSectionHeader(card, title, sectionKey);
                          var list = document.createElement('ul');
                          list.className = 'activity-plan-list';
                          for (var i = 0; i < items.length; i++) {
                              appendTextElement(list, 'li', '', items[i]);
                          }
                          card.appendChild(list);
                          appendActivityPlanSectionStatus(card, sectionKey);
                          container.appendChild(card);
                      }

                      function renderActivityPlanStepsCard(container, steps, sectionKey) {
                          if (!steps || !steps.length) {
                              return;
                          }

                          var card = document.createElement('section');
                          card.className = 'activity-plan-card';
                          appendActivityPlanSectionHeader(card, '活动步骤', sectionKey);

                          for (var i = 0; i < steps.length; i++) {
                              var step = steps[i] || {};
                             var stepWrap = document.createElement('div');
                             stepWrap.className = 'activity-plan-step';

                             var head = document.createElement('div');
                             head.className = 'activity-plan-step-head';
                             appendTextElement(head, 'div', 'activity-plan-step-title', (i + 1) + '. ' + (step.title || '未命名步骤'));
                             appendTextElement(head, 'div', 'activity-plan-step-minutes', step.minutes || '');
                             stepWrap.appendChild(head);

                             var grid = document.createElement('div');
                             grid.className = 'activity-plan-step-grid';
                             appendActivityPlanField(grid, '教师活动', step.teacherAction);
                             appendActivityPlanField(grid, '学生活动', step.studentAction);
                             appendActivityPlanField(grid, '互动方式', step.interactionMethod);
                             appendActivityPlanField(grid, '资源建议', step.resourceSuggestion);
                             appendActivityPlanField(grid, '评价检查', step.assessmentCheck);
                             stepWrap.appendChild(grid);

                             card.appendChild(stepWrap);
                          }

                          appendActivityPlanSectionStatus(card, sectionKey);
                          container.appendChild(card);
                      }

                     function appendActivityPlanField(container, label, value) {
                         var field = document.createElement('div');
                         appendTextElement(field, 'span', 'activity-plan-field-label', label);
                         appendTextElement(field, 'div', 'activity-plan-field-value', value || '');
                         container.appendChild(field);
                     }

                      function renderActivityPlanDraft(responseData) {
                           var elements = getActivityPlanElements();
                           if (!elements.result || !responseData || !responseData.draft) {
                               return;
                           }

                          var draft = responseData.draft;
                          lastActivityPlanDraftResponse = responseData;
                          elements.result.textContent = '';
                          resetActivityPlanSelections();

                          var container = document.createElement('div');
                          container.className = 'activity-plan-draft';

                          var meta = document.createElement('div');
                          meta.className = 'activity-plan-draft-meta';
                          meta.textContent = '当前为预览草案，不会自动写入学案内容。Provider：' + (responseData.providerDisplayName || '未标注') + '；技能：' + (responseData.skillName || '默认技能');
                          container.appendChild(meta);

                          renderActivityPlanListCard(container, '教学目标', draft.teachingGoals || [], 'teachingGoals');
                          renderActivityPlanStepsCard(container, draft.activitySteps || [], 'activitySteps');
                          renderActivityPlanListCard(container, '教学资源', draft.resources || [], 'resources');
                          renderActivityPlanListCard(container, '评价设计', draft.assessment || [], 'assessment');

                          var reminderCard = document.createElement('section');
                          reminderCard.className = 'activity-plan-card';
                          appendActivityPlanSectionHeader(reminderCard, '教师提醒', 'teacherReminder');
                          appendTextElement(reminderCard, 'div', 'activity-plan-field-value', draft.teacherReminder || '');
                          appendActivityPlanSectionStatus(reminderCard, 'teacherReminder');
                          container.appendChild(reminderCard);

                          elements.result.appendChild(container);
                       }

                      function buildActivityPlanApplyBlocks(sectionKeys) {
                          if (!lastActivityPlanDraftResponse || !lastActivityPlanDraftResponse.draft || !sectionKeys || !sectionKeys.length) {
                              return '';
                          }

                          var draft = lastActivityPlanDraftResponse.draft;
                          var blocks = [];
                          for (var i = 0; i < sectionKeys.length; i++) {
                              var sectionKey = sectionKeys[i];
                              var title = getActivityPlanSectionLabel(sectionKey);
                              if (sectionKey === 'teachingGoals') {
                                  blocks.push('<p><strong>【活动计划-' + title + '】</strong></p><ul><li>' + (draft.teachingGoals || []).map(escapeActivityPlanHtml).join('</li><li>') + '</li></ul>');
                              } else if (sectionKey === 'activitySteps') {
                                  var steps = draft.activitySteps || [];
                                  var stepHtml = [];
                                  for (var j = 0; j < steps.length; j++) {
                                      var step = steps[j] || {};
                                      stepHtml.push('<li><strong>' + escapeActivityPlanHtml((j + 1) + '. ' + (step.title || '')) + '</strong><br />时长：' + escapeActivityPlanHtml(step.minutes || '') + '<br />教师活动：' + escapeActivityPlanHtml(step.teacherAction || '') + '<br />学生活动：' + escapeActivityPlanHtml(step.studentAction || '') + '<br />互动方式：' + escapeActivityPlanHtml(step.interactionMethod || '') + '<br />资源建议：' + escapeActivityPlanHtml(step.resourceSuggestion || '') + '<br />评价检查：' + escapeActivityPlanHtml(step.assessmentCheck || '') + '</li>');
                                  }
                                  blocks.push('<p><strong>【活动计划-' + title + '】</strong></p><ol>' + stepHtml.join('') + '</ol>');
                              } else if (sectionKey === 'resources') {
                                  blocks.push('<p><strong>【活动计划-' + title + '】</strong></p><ul><li>' + (draft.resources || []).map(escapeActivityPlanHtml).join('</li><li>') + '</li></ul>');
                              } else if (sectionKey === 'assessment') {
                                  blocks.push('<p><strong>【活动计划-' + title + '】</strong></p><ul><li>' + (draft.assessment || []).map(escapeActivityPlanHtml).join('</li><li>') + '</li></ul>');
                              } else if (sectionKey === 'teacherReminder') {
                                  blocks.push('<p><strong>【活动计划-' + title + '】</strong></p><p>' + escapeActivityPlanHtml(draft.teacherReminder || '') + '</p>');
                              }
                          }

                          return blocks.join('');
                      }

                      function confirmActivityPlanApply(sectionKeys) {
                          if (!sectionKeys || !sectionKeys.length) {
                              alert('请先选择要应用的章节');
                              return false;
                          }

                          var sectionNames = [];
                          for (var i = 0; i < sectionKeys.length; i++) {
                              sectionNames.push(getActivityPlanSectionLabel(sectionKeys[i]));
                          }

                          return window.confirm('将把以下章节追加到当前学案内容：' + sectionNames.join('、') + '。确认继续吗？');
                      }

                      function setCourseEditContentValue(content) {
                          var field = document.getElementById(window.__courseeditConfig.mcontentId);
                          if (!field) {
                              return;
                          }

                          field.value = content || '';
                          if (currentEditor === 'kindeditor') {
                              if (kindEditorObj) {
                                  kindEditorObj.html(field.value);
                              }
                          } else if (currentEditor === 'wangeditor') {
                              if (wangEditorObj) {
                                  wangEditorObj.setHtml(field.value);
                              }
                          } else if (currentEditor === 'vditor') {
                              if (vditorObj && vditorReady) {
                                  vditorObj.setValue(safeHtml2Md(field.value));
                                  rememberVditorState();
                              } else {
                                  pendingVditorHtml = safeHtml2Md(field.value);
                              }
                          }
                      }

                      function appendActivityPlanSectionsToEditor(sectionKeys) {
                          var field = document.getElementById(window.__courseeditConfig.mcontentId);
                          if (!field) {
                              return;
                          }

                          syncContent();
                          var existingContent = field.value || '';
                          var appendedContent = buildActivityPlanApplyBlocks(sectionKeys);
                          if (!appendedContent) {
                              return;
                          }

                          if (existingContent && !/[\r\n]$/.test(existingContent)) {
                              existingContent += '\n';
                          }
                          appendedContent = existingContent ? ('\n' + appendedContent) : appendedContent;
                          field.value = existingContent + appendedContent;
                          setCourseEditContentValue(field.value);
                      }

                      function applySelectedActivityPlanSections(sectionKeys) {
                          if (!lastActivityPlanDraftResponse || !lastActivityPlanDraftResponse.draft) {
                              alert('请先生成或恢复活动计划草案');
                              return;
                          }

                          var sectionsToApply = sectionKeys && sectionKeys.length ? sectionKeys : getSelectedActivityPlanSections();
                          if (!confirmActivityPlanApply(sectionsToApply)) {
                              return;
                          }

                          appendActivityPlanSectionsToEditor(sectionsToApply);
                      }

                      function collectCurrentActivityPlanDraftPayload() {
                          if (!lastActivityPlanDraftResponse || !lastActivityPlanDraftResponse.draft) {
                              return null;
                          }

                          var requestContext = buildActivityPlanRequestContext();
                          return {
                              topic: requestContext.topic,
                              grade: requestContext.grade,
                              duration: requestContext.duration,
                              teachingGoals: requestContext.teachingGoals,
                              existingCourseContent: requestContext.existingCourseContent,
                              currentDraft: JSON.stringify(lastActivityPlanDraftResponse.draft || {})
                          };
                      }

                      function saveCurrentActivityPlanDraft() {
                          var payload = collectCurrentActivityPlanDraftPayload();
                          if (!payload) {
                              alert('当前没有可保存的活动计划草案');
                              return;
                          }

                          var xhr = new XMLHttpRequest();
                          xhr.open('POST', 'aiprovider_api.ashx', true);
                          xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                          xhr.onreadystatechange = function () {
                              if (xhr.readyState !== 4) {
                                  return;
                              }

                              if (xhr.status === 200) {
                                  try {
                                      var res = JSON.parse(xhr.responseText || '{}');
                                      if (res.success) {
                                          setActivityPlanDraftStatus(true, new Date().toISOString().slice(0, 19));
                                          alert('草案已保存');
                                      } else {
                                          alert(res.msg || '保存草案失败');
                                      }
                                  } catch (e) {
                                      alert('保存草案返回解析失败');
                                  }
                              } else {
                                  alert('保存草案失败，状态码：' + xhr.status);
                              }
                          };

                          xhr.send('action=activityPlanSaveDraft'
                              + '&cid=' + encodeURIComponent(cid)
                              + '&topic=' + encodeURIComponent(payload.topic)
                              + '&grade=' + encodeURIComponent(payload.grade)
                              + '&duration=' + encodeURIComponent(payload.duration)
                              + '&teachingGoals=' + encodeURIComponent(payload.teachingGoals)
                              + '&existingCourseContent=' + encodeURIComponent(payload.existingCourseContent)
                              + '&currentDraft=' + encodeURIComponent(payload.currentDraft));
                      }

                      function applyLoadedActivityPlanDraft(data) {
                          var elements = getActivityPlanElements();
                          if (!data || !data.draft || !elements.topic) {
                              return;
                          }

                          elements.topic.value = data.topic || '';
                          if (elements.grade) {
                              elements.grade.value = data.grade || '';
                          }
                          if (elements.duration) {
                              elements.duration.value = data.duration || '';
                          }
                          if (elements.goals) {
                              elements.goals.value = data.teachingGoals || '';
                          }

                          renderActivityPlanDraft({
                              providerDisplayName: '已保存草案',
                              skillName: '继续编辑',
                              draft: data.draft
                          });
                          setActivityPlanDraftStatus(true, data.updatedAt || '');
                      }

                      function resumeSavedActivityPlanDraft() {
                          var xhr = new XMLHttpRequest();
                          xhr.open('POST', 'aiprovider_api.ashx', true);
                          xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                          xhr.onreadystatechange = function () {
                              if (xhr.readyState !== 4) {
                                  return;
                              }

                              if (xhr.status === 200) {
                                  try {
                                      var res = JSON.parse(xhr.responseText || '{}');
                                      if (res.success && res.data) {
                                          applyLoadedActivityPlanDraft(res.data);
                                      } else {
                                          alert(res.msg || '恢复草案失败');
                                      }
                                  } catch (e) {
                                      alert('恢复草案返回解析失败');
                                  }
                              } else {
                                  alert('恢复草案失败，状态码：' + xhr.status);
                              }
                          };

                          xhr.send('action=activityPlanLoadDraft&cid=' + encodeURIComponent(cid));
                      }

                      function deleteSavedActivityPlanDraft(callback) {
                          var xhr = new XMLHttpRequest();
                          xhr.open('POST', 'aiprovider_api.ashx', true);
                          xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                          xhr.onreadystatechange = function () {
                              if (xhr.readyState !== 4) {
                                  return;
                              }

                              if (xhr.status === 200) {
                                  try {
                                      var res = JSON.parse(xhr.responseText || '{}');
                                      if (res.success) {
                                          setActivityPlanDraftStatus(false, '');
                                          if (callback) {
                                              callback(true);
                                          }
                                      } else if (callback) {
                                          callback(false, res.msg || '删除草案失败');
                                      }
                                  } catch (e) {
                                      if (callback) {
                                          callback(false, '删除草案返回解析失败');
                                      }
                                  }
                              } else if (callback) {
                                  callback(false, '删除草案失败，状态码：' + xhr.status);
                              }
                          };

                          xhr.send('action=activityPlanDeleteDraft&cid=' + encodeURIComponent(cid));
                      }

                      function checkSavedActivityPlanDraftStatus(callback) {
                          var xhr = new XMLHttpRequest();
                          xhr.open('POST', 'aiprovider_api.ashx', true);
                          xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                          xhr.onreadystatechange = function () {
                              if (xhr.readyState !== 4) {
                                  return;
                              }

                              if (xhr.status === 200) {
                                  try {
                                      var res = JSON.parse(xhr.responseText || '{}');
                                      if (res.success && res.data) {
                                          setActivityPlanDraftStatus(!!res.data.hasDraft, res.data.updatedAt || '');
                                          if (callback) {
                                              callback({ hasDraft: !!res.data.hasDraft, updatedAt: res.data.updatedAt || '' });
                                          }
                                      } else if (callback) {
                                          callback({ hasDraft: false, updatedAt: '' });
                                      }
                                  } catch (e) {
                                      if (callback) {
                                          callback({ hasDraft: activityPlanDraftStatus.hasDraft, updatedAt: activityPlanDraftStatus.updatedAt || '' });
                                      }
                                  }
                              } else if (callback) {
                                  callback({ hasDraft: activityPlanDraftStatus.hasDraft, updatedAt: activityPlanDraftStatus.updatedAt || '' });
                              }
                          };

                          xhr.send('action=activityPlanDraftStatus&cid=' + encodeURIComponent(cid));
                      }

                      function maybeHandleSavedDraftBeforeGenerate(requestContext, continueGenerate) {
                          checkSavedActivityPlanDraftStatus(function (status) {
                              if (!status || !status.hasDraft) {
                                  continueGenerate();
                                  return;
                              }

                              var keepEditing = window.confirm('当前课程已有已保存草案。选择“确定”将继续上次草案，选择“取消”将替换旧草案并重新生成。');
                              if (keepEditing) {
                                  resumeSavedActivityPlanDraft();
                                  return;
                              }

                              deleteSavedActivityPlanDraft(function (success, msg) {
                                  if (!success) {
                                      alert(msg || '替换旧草案失败');
                                      return;
                                  }

                                  continueGenerate();
                              });
                          });
                      }

                      function buildActivityPlanRequestContext() {
                          var elements = getActivityPlanElements();
                          var topic = elements.topic ? elements.topic.value.trim() : '';
                          var gradeValue = elements.grade ? elements.grade.value.trim() : '';
                          if (!gradeValue) {
                              gradeValue = getDefaultCourseEditGrade();
                              if (elements.grade) {
                                  elements.grade.value = gradeValue;
                              }
                          }

                          return {
                              topic: topic,
                              grade: gradeValue,
                              duration: elements.duration ? elements.duration.value.trim() : '',
                              teachingGoals: elements.goals ? elements.goals.value.trim() : '',
                              existingCourseContent: getCourseEditContentValue()
                          };
                      }

                      function regenerateActivityPlanSection(sectionTarget) {
                          if (!lastActivityPlanDraftResponse || !lastActivityPlanDraftResponse.draft) {
                              return;
                          }

                          if (!activityPlanSectionStates[sectionTarget] || getActivityPlanSectionState(sectionTarget).isLoading) {
                              return;
                          }

                          var requestContext = buildActivityPlanRequestContext();
                          if (!requestContext.topic) {
                              alert('请输入主题或知识点');
                              var elements = getActivityPlanElements();
                              if (elements.topic) {
                                  elements.topic.focus();
                              }
                              return;
                          }

                          setActivityPlanSectionState(sectionTarget, true, '');
                          renderActivityPlanDraft(lastActivityPlanDraftResponse);

                          var previousResponse = lastActivityPlanDraftResponse;
                          var xhr = new XMLHttpRequest();
                          xhr.timeout = 125000;
                          xhr.open('POST', 'aiprovider_api.ashx', true);
                          xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                          xhr.onreadystatechange = function () {
                              if (xhr.readyState !== 4) {
                                  return;
                              }

                              if (xhr.status === 200) {
                                  try {
                                      var res = JSON.parse(xhr.responseText);
                                      if (res.success && res.data && res.data.draft) {
                                          setActivityPlanSectionState(sectionTarget, false, '');
                                          lastActivityPlanDraftResponse = res.data;
                                          renderActivityPlanDraft(lastActivityPlanDraftResponse);
                                      } else {
                                          setActivityPlanSectionState(sectionTarget, false, (res.msg || '本节重生成失败，请稍后重试。'));
                                          lastActivityPlanDraftResponse = previousResponse;
                                          renderActivityPlanDraft(lastActivityPlanDraftResponse);
                                      }
                                  } catch (e) {
                                      setActivityPlanSectionState(sectionTarget, false, '本节返回结果解析失败，请稍后重试。');
                                      lastActivityPlanDraftResponse = previousResponse;
                                      renderActivityPlanDraft(lastActivityPlanDraftResponse);
                                  }
                              } else {
                                  setActivityPlanSectionState(sectionTarget, false, '本节请求失败，状态码：' + xhr.status);
                                  lastActivityPlanDraftResponse = previousResponse;
                                  renderActivityPlanDraft(lastActivityPlanDraftResponse);
                              }
                          };

                          xhr.onerror = function () {
                              setActivityPlanSectionState(sectionTarget, false, '本节网络异常，未能连接活动计划接口。');
                              lastActivityPlanDraftResponse = previousResponse;
                              renderActivityPlanDraft(lastActivityPlanDraftResponse);
                          };

                          xhr.ontimeout = function () {
                              setActivityPlanSectionState(sectionTarget, false, '本节重生成超时，请稍后重试。');
                              lastActivityPlanDraftResponse = previousResponse;
                              renderActivityPlanDraft(lastActivityPlanDraftResponse);
                          };

                          xhr.send('action=activityPlanRegenerateSection'
                              + '&topic=' + encodeURIComponent(requestContext.topic)
                              + '&grade=' + encodeURIComponent(requestContext.grade)
                              + '&duration=' + encodeURIComponent(requestContext.duration)
                              + '&teachingGoals=' + encodeURIComponent(requestContext.teachingGoals)
                              + '&existingCourseContent=' + encodeURIComponent(requestContext.existingCourseContent)
                              + '&sectionTarget=' + encodeURIComponent(sectionTarget)
                              + '&currentDraft=' + encodeURIComponent(JSON.stringify(previousResponse.draft || {})));
                      }

                     function buildActivityPlanCopyText() {
                         if (!lastActivityPlanDraftResponse || !lastActivityPlanDraftResponse.draft) {
                             return '';
                         }

                         var draft = lastActivityPlanDraftResponse.draft;
                         var sections = [];
                         sections.push('【教学目标】\n' + (draft.teachingGoals || []).join('\n'));

                         var steps = draft.activitySteps || [];
                         var stepLines = [];
                         for (var i = 0; i < steps.length; i++) {
                             var step = steps[i] || {};
                             stepLines.push((i + 1) + '. ' + (step.title || ''));
                             stepLines.push('时长：' + (step.minutes || ''));
                             stepLines.push('教师活动：' + (step.teacherAction || ''));
                             stepLines.push('学生活动：' + (step.studentAction || ''));
                             stepLines.push('互动方式：' + (step.interactionMethod || ''));
                             stepLines.push('资源建议：' + (step.resourceSuggestion || ''));
                             stepLines.push('评价检查：' + (step.assessmentCheck || ''));
                             stepLines.push('');
                         }
                         sections.push('【活动步骤】\n' + stepLines.join('\n').trim());
                         sections.push('【教学资源】\n' + (draft.resources || []).join('\n'));
                         sections.push('【评价设计】\n' + (draft.assessment || []).join('\n'));
                         sections.push('【教师提醒】\n' + (draft.teacherReminder || ''));
                         return sections.join('\n\n').trim();
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

                         var requestContext = buildActivityPlanRequestContext();
                          var runGenerateRequest = function () {
                              setActivityPlanLoading(true);
                              clearActivityPlanResult();
                              renderActivityPlanPlaceholder('生成中，结构化草案完成后会显示在这里。');
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
                                              renderActivityPlanDraft(res.data || null);
                                              setActivityPlanProgress(100, '生成完成', '已生成结构化预览草案，当前不会自动写入学案内容。');
                                          } else {
                                              clearActivityPlanResult();
                                              setActivityPlanProgress(100, '生成失败', res.msg || 'AI Provider 返回错误，请稍后重试。');
                                              alert(res.msg || '生成失败');
                                          }
                                      } catch (e) {
                                          clearActivityPlanResult();
                                          setActivityPlanProgress(100, '解析失败', '响应格式不符合预期。');
                                          alert('解析响应失败');
                                      }
                                  } else {
                                      clearActivityPlanResult();
                                      setActivityPlanProgress(100, '请求失败', '接口请求未成功完成，请检查网络或服务端状态。');
                                      alert('请求失败，状态码：' + xhr.status);
                                  }
                              };

                              xhr.onerror = function () {
                                  setActivityPlanLoading(false);
                                  clearActivityPlanResult();
                                  setActivityPlanProgress(100, '网络异常', '未能连接到活动计划接口。');
                                  alert('网络异常，无法连接活动计划接口');
                              };

                              xhr.ontimeout = function () {
                                  setActivityPlanLoading(false);
                                  clearActivityPlanResult();
                                  setActivityPlanProgress(100, '请求超时', '活动计划生成超过 125 秒未返回。');
                                  alert('请求超时，请稍后重试');
                              };

                              xhr.send('action=activityPlan'
                                  + '&topic=' + encodeURIComponent(topic)
                                  + '&grade=' + encodeURIComponent(requestContext.grade)
                                  + '&duration=' + encodeURIComponent(requestContext.duration)
                                  + '&teachingGoals=' + encodeURIComponent(requestContext.teachingGoals)
                                  + '&existingCourseContent=' + encodeURIComponent(requestContext.existingCourseContent));
                          };

                          maybeHandleSavedDraftBeforeGenerate(requestContext, runGenerateRequest);
                      }

                     function copyActivityPlanResult() {
                         var text = buildActivityPlanCopyText();
                         if (!text) {
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
        checkSavedActivityPlanDraftStatus();
    })();
