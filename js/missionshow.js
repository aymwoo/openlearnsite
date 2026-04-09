(function () {
            var markdownStorageKey = 'teacher-missionshow-markdown-enabled';
            var revealStorageKey = 'teacher-missionshow-reveal-enabled';

            function getContentElement() {
                return window.__missionshowConfig ? document.getElementById(window.__missionshowConfig.mcontentId) : null;
            }

            function getHiddenRawElement() {
                return window.__missionshowConfig ? document.getElementById(window.__missionshowConfig.hiddenMissionRawId) : null;
            }

            function getMarkdownToggle() {
                return window.__missionshowConfig ? document.getElementById(window.__missionshowConfig.markdownToggleId) : null;
            }

            function getMarkdownStatus() {
                return window.__missionshowConfig ? document.getElementById(window.__missionshowConfig.markdownToggleStatusId) : null;
            }

            function getRevealToggle() {
                return window.__missionshowConfig ? document.getElementById(window.__missionshowConfig.revealToggleId) : null;
            }

            function getRevealStatus() {
                return window.__missionshowConfig ? document.getElementById(window.__missionshowConfig.revealToggleStatusId) : null;
            }

            function getStoredFlagOrNull(key) {
                try {
                    var stored = localStorage.getItem(key);
                    return stored === null ? null : stored === '1';
                } catch (e) {
                    return null;
                }
            }

            function setStoredFlag(key, enabled) {
                try {
                    localStorage.setItem(key, enabled ? '1' : '0');
                } catch (e) {
                }
            }

            function updateSwitch(toggle, status, enabled) {
                if (toggle) {
                    toggle.className = enabled ? 'mission-show-toggle-switch is-on' : 'mission-show-toggle-switch';
                    toggle.setAttribute('aria-pressed', enabled ? 'true' : 'false');
                }
                if (status) {
                    status.innerText = enabled ? '当前：开启' : '当前：关闭';
                }
            }

            function looksLikeMarkdown(text) {
                return window.ContentShowMarkdown && window.ContentShowMarkdown.looksLikeMarkdown
                    ? window.ContentShowMarkdown.looksLikeMarkdown(text, true)
                    : false;
            }

            function bindToggleHandlers() {
                var markdownToggle = getMarkdownToggle();
                var revealToggle = getRevealToggle();

                if (markdownToggle && !markdownToggle.getAttribute('data-bound')) {
                    markdownToggle.setAttribute('data-bound', '1');
                    markdownToggle.addEventListener('click', function () {
                        window.toggleMissionMarkdown();
                    });
                }

                if (revealToggle && !revealToggle.getAttribute('data-bound')) {
                    revealToggle.setAttribute('data-bound', '1');
                    revealToggle.addEventListener('click', function () {
                        window.toggleMissionReveal();
                    });
                }
            }

            function isRevealMarkdownDocument(text) {
                return window.ContentShowMarkdown && window.ContentShowMarkdown.isRevealMarkdownDocument
                    ? window.ContentShowMarkdown.isRevealMarkdownDocument(text)
                    : false;
            }

            function getMissionSource() {
                var content = getContentElement();
                var hidden = getHiddenRawElement();
                var hiddenValue = hidden ? (hidden.value || '') : '';
                return window.ContentShowMarkdown && window.ContentShowMarkdown.getMarkdownSource
                    ? window.ContentShowMarkdown.getMarkdownSource(content, hiddenValue)
                    : (hiddenValue || '');
            }

            function restoreOriginalHtml(content) {
                if (window.ContentShowMarkdown && window.ContentShowMarkdown.restoreOriginalHtml) {
                    window.ContentShowMarkdown.restoreOriginalHtml(content, 'mission-show-content');
                    return;
                }
                content.className = 'mission-show-content';
                content.innerHTML = content.getAttribute('data-original-html') || '';
            }

            function renderMissionContent() {
                var content = getContentElement();
                if (!content) {
                    return;
                }

                if (!content.getAttribute('data-original-html')) {
                    content.setAttribute('data-original-html', content.innerHTML || '');
                }

                var markdownSource = getMissionSource();
                var storedMarkdown = getStoredFlagOrNull(markdownStorageKey);
                var storedReveal = getStoredFlagOrNull(revealStorageKey);
                var autoMarkdown = looksLikeMarkdown(markdownSource);
                var autoReveal = autoMarkdown && isRevealMarkdownDocument(markdownSource);
                var markdownEnabled = storedMarkdown === null ? autoMarkdown : storedMarkdown;
                var revealEnabled = storedReveal === null ? autoReveal : storedReveal;

                updateSwitch(getMarkdownToggle(), getMarkdownStatus(), markdownEnabled);
                updateSwitch(getRevealToggle(), getRevealStatus(), revealEnabled);

                if (getMarkdownStatus() && storedMarkdown === null && markdownEnabled) {
                    getMarkdownStatus().innerText = '当前：自动开启';
                }

                if (getRevealStatus() && storedReveal === null && revealEnabled) {
                    getRevealStatus().innerText = '当前：自动开启';
                }

                if (!markdownEnabled || !window.marked) {
                    restoreOriginalHtml(content);
                    return;
                }

                if (!looksLikeMarkdown(markdownSource)) {
                    restoreOriginalHtml(content);
                    if (getMarkdownStatus()) getMarkdownStatus().innerText = '当前：开启，未检测到 Markdown';
                    return;
                }

                if (window.ContentShowMarkdown && window.ContentShowMarkdown.renderIntoContent) {
                    window.ContentShowMarkdown.renderIntoContent(content, {
                        source: markdownSource,
                        enableReveal: revealEnabled,
                        renderedClass: 'mission-markdown',
                        originalClass: 'mission-show-content',
                        wrapInVditorReset: true
                    });
                } else {
                    restoreOriginalHtml(content);
                }
            }

            window.toggleMissionMarkdown = function () {
                var current = getStoredFlagOrNull(markdownStorageKey);
                if (current === null) {
                    current = looksLikeMarkdown(getMissionSource());
                }
                var nextEnabled = !current;
                setStoredFlag(markdownStorageKey, nextEnabled);
                renderMissionContent();
            };

            window.toggleMissionReveal = function () {
                var current = getStoredFlagOrNull(revealStorageKey);
                if (current === null) {
                    current = isRevealMarkdownDocument(getMissionSource());
                }
                var nextEnabled = !current;
                setStoredFlag(revealStorageKey, nextEnabled);
                renderMissionContent();
            };

            if (document.readyState === 'loading') {
                document.addEventListener('DOMContentLoaded', function () {
                    bindToggleHandlers();
                    renderMissionContent();
                });
            } else {
                bindToggleHandlers();
                renderMissionContent();
            }
        })();
