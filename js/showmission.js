/* ================================================================
   showmission.js — 从 student/showmission.aspx 提取的页面脚本
   依赖: KindEditor, marked.js, highlight.js
   服务端配置通过 window.__showmissionConfig 注入
   ================================================================ */

/* ================================================================
   KindEditor 上传按钮 — 个人作品
   ================================================================ */
(function () {
    var cfg = window.__showmissionConfig || {};
    var lid = cfg.lid || '';
    var urlstr = "uploadworkm.aspx?lid=" + lid;
    KindEditor.ready(function (K) {
        if (!K('#uploadButton')[0]) {
            return;
        }
        var uploadbutton = K.uploadbutton({
            button: K('#uploadButton')[0],
            fieldName: 'imgFile',
            url: urlstr,
            afterUpload: function (data) {
                if (data.error === 0) {
                    if (window.LearnStatus && typeof window.LearnStatus.submitted === "function") {
                        window.LearnStatus.submitted();
                    }
                    alert("作品已经提交成功！");
                    location.reload();
                }
                else { alert(data.message); }
            },
            afterError: function (str) { alert('出错信息: ' + str); }
        });
        uploadbutton.fileBox.change(function (e) { uploadbutton.submit(); });
    });
})();

/* ================================================================
   KindEditor 上传按钮 — 小组作品
   ================================================================ */
(function () {
    var cfg = window.__showmissionConfig || {};
    var lid = cfg.lid || '';
    var gurlstr = "uploadgroupm.aspx?lid=" + lid;
    KindEditor.ready(function (K) {
        if (!K('#uploadgroupButton')[0]) {
            return;
        }
        var uploadgroupbutton = K.uploadbutton({
            button: K('#uploadgroupButton')[0],
            fieldName: 'imgFilegroup',
            url: gurlstr,
            afterUpload: function (data) {
                if (data.error === 0) {
                    if (window.LearnStatus && typeof window.LearnStatus.submitted === "function") {
                        window.LearnStatus.submitted();
                    }
                    alert("小组作品已经提交成功！");
                    location.reload(true);
                }
                else { alert(data.message); }
            },
            afterError: function (str) { alert('出错信息: ' + str); }
        });
        uploadgroupbutton.fileBox.change(function (e) { uploadgroupbutton.submit(); });
    });
})();

/* ================================================================
   Markdown / Reveal.js / Mermaid 渲染引擎
   ================================================================ */
(function () {
    var cfg = window.__showmissionConfig || {};
    var mContentId = cfg.mContentId || '';
    var markdownToggleId = cfg.markdownToggleId || '';
    var markdownToggleStatusId = cfg.markdownToggleStatusId || '';
    var hiddenMissionRawId = cfg.hiddenMissionRawId || '';
    var revealToggleId = cfg.revealToggleId || '';
    var revealToggleStatusId = cfg.revealToggleStatusId || '';

    var markdownStorageKey = 'showmission-markdown-enabled';
    var revealStorageKey = 'showmission-reveal-enabled';
    var revealThemeOptions = [
        { value: 'default', label: '默认主题' },
        { value: 'white', label: 'White' },
        { value: 'sky', label: 'Sky' },
        { value: 'beige', label: 'Beige' },
        { value: 'simple', label: 'Simple' },
        { value: 'serif', label: 'Serif' },
        { value: 'moon', label: 'Moon' },
        { value: 'night', label: 'Night' },
        { value: 'solarized', label: 'Solarized' }
    ];

    function getRevealThemeOptionsHtml() {
        return revealThemeOptions.map(function (theme) {
            return '<option value="' + escapeHtml(theme.value) + '">' + escapeHtml(theme.label) + '</option>';
        }).join('');
    }

    function normalizeRevealTheme(theme) {
        var value = (theme || '').toLowerCase();
        for (var i = 0; i < revealThemeOptions.length; i++) {
            if (revealThemeOptions[i].value === value) {
                return value;
            }
        }
        return 'default';
    }

    function isDarkRevealTheme(theme) {
        var value = normalizeRevealTheme(theme);
        return value === 'default' || value === 'moon' || value === 'night';
    }

    function getContentElement() {
        return document.getElementById(mContentId);
    }

    function getToggleElement() {
        return document.getElementById(markdownToggleId);
    }

    function getToggleStatusElement() {
        return document.getElementById(markdownToggleStatusId);
    }

    function getRevealToggleElement() {
        return document.getElementById(revealToggleId);
    }

    function getRevealStatusElement() {
        return document.getElementById(revealToggleStatusId);
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

    function stripHtmlToText(html) {
        var holder = document.createElement('div');
        holder.innerHTML = html || '';

        var lines = [];

        function walk(node) {
            if (!node) {
                return;
            }

            if (node.nodeType === 3) {
                lines.push(node.nodeValue);
                return;
            }

            if (node.nodeType !== 1) {
                return;
            }

            var tag = node.tagName.toLowerCase();
            if (tag === 'br') {
                lines.push('\n');
                return;
            }

            if (tag === 'pre') {
                lines.push(node.textContent || '');
                lines.push('\n');
                return;
            }

            var children = node.childNodes;
            for (var i = 0; i < children.length; i++) {
                walk(children[i]);
            }

            if (/^(p|div|section|article|li|ul|ol|blockquote|h1|h2|h3|h4|h5|h6)$/i.test(tag)) {
                lines.push('\n');
            }
        }

        var nodes = holder.childNodes;
        for (var j = 0; j < nodes.length; j++) {
            walk(nodes[j]);
        }

        return lines.join('').replace(/\r/g, '').replace(/\n{3,}/g, '\n\n').trim();
    }

    function getMarkdownSource(rawText) {
        var text = (rawText || '').replace(/\r/g, '').trim();
        if (!text) {
            return '';
        }

        if (!/<[a-z][\s\S]*>/i.test(text)) {
            return text;
        }

        var plainText = stripHtmlToText(text);
        if (looksLikeMarkdown(plainText)) {
            return plainText;
        }

        return text;
    }

    function getMissionSource(content, hidden) {
        var hiddenValue = hidden ? (hidden.value || '') : '';
        var contentHtml = content ? (content.innerHTML || '') : '';
        var contentText = content ? (content.textContent || '') : '';

        if (looksLikeMarkdown(hiddenValue)) {
            return hiddenValue;
        }

        var hiddenMarkdown = getMarkdownSource(hiddenValue);
        if (looksLikeMarkdown(hiddenMarkdown)) {
            return hiddenMarkdown;
        }

        var htmlMarkdown = getMarkdownSource(contentHtml);
        if (looksLikeMarkdown(htmlMarkdown)) {
            return htmlMarkdown;
        }

        if (looksLikeMarkdown(contentText)) {
            return contentText;
        }

        return hiddenValue || contentHtml || contentText || '';
    }

    function looksLikeMarkdown(text) {
        if (!text) {
            return false;
        }

        var normalized = (text || '').replace(/\r/g, '').trim();
        if (!normalized) {
            return false;
        }

        if (/^\s*<[^>]+>/m.test(normalized) && !/^\s*<(pre|code)\b/i.test(normalized)) {
            return false;
        }

        return /(^|\n)\s{0,3}(#{1,6}\s+.+|[-*+]\s+.+|\d+\.\s+.+|>\s+.+|```[\s\S]*?```|~~~[\s\S]*?~~~|\|.+\|\s*$|!\[[^\]]*\]\([^\)]+\)|\[[^\]]+\]\([^\)]+\)|-{3,}|\*{3,}|`[^`]+`)/m.test(normalized);
    }

    function isRevealMarkdownDocument(text) {
        if (!looksLikeMarkdown(text)) {
            return false;
        }

        var normalized = (text || '').replace(/\r/g, '').trim();
        if (!normalized) {
            return false;
        }

        if (/^```(?:reveal|revealjs)\b/m.test(normalized)) {
            return true;
        }

        if (/^\s*--\s*$/m.test(normalized)) {
            return true;
        }

        if (/^\s*(?:note|notes)\s*:/mi.test(normalized)) {
            return true;
        }

        if (/^\s*\.element\s*:/mi.test(normalized)) {
            return true;
        }

        if (/^\s*<!--\s*\.slide\s*:/mi.test(normalized) || /^\s*<!--\s*\.element\s*:/mi.test(normalized)) {
            return true;
        }

        var parts = normalized.split(/^---$/m).map(function (section) {
            return section.trim();
        }).filter(function (section) {
            return !!section;
        });

        if (parts.length < 2) {
            return false;
        }

        var contentfulParts = 0;
        for (var i = 0; i < parts.length; i++) {
            if (/^(#{1,6}\s+|>|[-*+]\s+|\d+\.\s+|```|~~~|\w)/m.test(parts[i])) {
                contentfulParts++;
            }
        }

        return contentfulParts >= 2;
    }

    function escapeHtml(value) {
        return (value || '')
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;');
    }

    function parseRevealMarkdown(blockText) {
        var cleaned = blockText
            .replace(/^```(?:reveal|revealjs)?\s*/i, '')
            .replace(/```\s*$/, '')
            .trim();
        var presentationAttrs = { startH: 0, startV: 0 };

        function splitSlides(input, separator) {
            return input.split(separator).map(function (section) {
                return section.trim();
            }).filter(function (section) {
                return !!section;
            });
        }

        function escapeAttr(value) {
            return (value || '')
                .replace(/&/g, '&amp;')
                .replace(/"/g, '&quot;')
                .replace(/</g, '&lt;')
                .replace(/>/g, '&gt;');
        }

        function parseDirectiveAttributes(raw) {
            var attrs = {};
            var regex = /([\w:-]+)\s*=\s*"([^"]*)"/g;
            var match;
            while ((match = regex.exec(raw || '')) !== null) {
                attrs[match[1]] = match[2];
            }
            return attrs;
        }

        function attrsToString(attrs) {
            var parts = [];
            for (var key in attrs) {
                if (Object.prototype.hasOwnProperty.call(attrs, key) && attrs[key] !== '') {
                    parts.push(key + '="' + escapeAttr(attrs[key]) + '"');
                }
            }
            return parts.length ? ' ' + parts.join(' ') : '';
        }

        function applyElementDirective(html, attrs) {
            return html.replace(/<(p|li|h1|h2|h3|h4|h5|h6|blockquote|pre|table)([^>]*)>([\s\S]*?)<\/\1>(?![\s\S]*<(p|li|h1|h2|h3|h4|h5|h6|blockquote|pre|table))/i, function (_, tag, currentAttrs, inner) {
                var nextAttrs = currentAttrs;

                for (var key in attrs) {
                    if (!Object.prototype.hasOwnProperty.call(attrs, key)) {
                        continue;
                    }

                    if (key === 'class') {
                        var classMatch = nextAttrs.match(/class="([^"]*)"/i);
                        if (classMatch) {
                            nextAttrs = nextAttrs.replace(/class="([^"]*)"/i, 'class="$1 ' + escapeAttr(attrs[key]) + '"');
                        } else {
                            nextAttrs += ' class="' + escapeAttr(attrs[key]) + '"';
                        }
                    } else if (new RegExp(key + '="[^"]*"', 'i').test(nextAttrs)) {
                        nextAttrs = nextAttrs.replace(new RegExp(key + '="[^"]*"', 'i'), key + '="' + escapeAttr(attrs[key]) + '"');
                    } else {
                        nextAttrs += ' ' + key + '="' + escapeAttr(attrs[key]) + '"';
                    }
                }

                return '<' + tag + nextAttrs + '>' + inner + '</' + tag + '>';
            });
        }

        cleaned = cleaned.replace(/^\s*<!--\s*\.presentation\s*:\s*(.*?)\s*-->\s*$/gmi, function (_, raw) {
            var presentationDirectiveAttrs = parseDirectiveAttributes(raw);
            if (presentationDirectiveAttrs['data-start-h'] !== undefined) {
                presentationAttrs.startH = parseInt(presentationDirectiveAttrs['data-start-h'], 10) || 0;
            }
            if (presentationDirectiveAttrs['data-start-v'] !== undefined) {
                presentationAttrs.startV = parseInt(presentationDirectiveAttrs['data-start-v'], 10) || 0;
            }
            return '';
        }).trim();

        function renderSlideMarkdown(sectionText) {
            var notes = [];
            var elementDirectives = [];
            var slideAttrs = {};
            var lines = sectionText.split('\n');
            var markdownLines = [];

            for (var i = 0; i < lines.length; i++) {
                var line = lines[i];
                var noteMatch = line.match(/^\s*(?:note|notes)\s*:\s*(.*)$/i);
                if (noteMatch) {
                    notes.push(noteMatch[1] || '');
                    continue;
                }

                var commentSlideMatch = line.match(/^\s*<!--\s*\.slide\s*:\s*(.*?)\s*-->\s*$/i);
                if (commentSlideMatch) {
                    var slideDirectiveAttrs = parseDirectiveAttributes(commentSlideMatch[1]);
                    for (var slideKey in slideDirectiveAttrs) {
                        if (Object.prototype.hasOwnProperty.call(slideDirectiveAttrs, slideKey)) {
                            slideAttrs[slideKey] = slideDirectiveAttrs[slideKey];
                        }
                    }
                    continue;
                }

                var commentElementMatch = line.match(/^\s*<!--\s*\.element\s*:\s*(.*?)\s*-->\s*$/i);
                if (commentElementMatch) {
                    elementDirectives.push(parseDirectiveAttributes(commentElementMatch[1]));
                    continue;
                }

                var elementMatch = line.match(/^\s*\.element\s*:\s*class\s*=\s*"([^"]+)"\s*$/i);
                if (elementMatch) {
                    elementDirectives.push({ class: elementMatch[1] });
                    continue;
                }

                markdownLines.push(line);
            }

            var html = marked.parse(markdownLines.join('\n').trim());

            if (elementDirectives.length) {
                for (var j = 0; j < elementDirectives.length; j++) {
                    html = applyElementDirective(html, elementDirectives[j]);
                }
            }

            if (notes.length) {
                html += '<aside class="notes">' + marked.parse(notes.join('\n')) + '</aside>';
            }

            return {
                html: html,
                attrs: slideAttrs
            };
        }

        var horizontalSlides = splitSlides(cleaned, /^---$/m);
        if (!horizontalSlides.length) {
            return '<div class="render-note">未检测到可展示的幻灯片内容。</div>';
        }

        var slidesHtml = horizontalSlides.map(function (section) {
            var verticalSlides = splitSlides(section, /^--$/m);
            if (verticalSlides.length > 1) {
                return '<section>' + verticalSlides.map(function (verticalSection) {
                    var verticalSlide = renderSlideMarkdown(verticalSection);
                    return '<section' + attrsToString(verticalSlide.attrs) + '>' + verticalSlide.html + '</section>';
                }).join('') + '</section>';
            }

            var slide = renderSlideMarkdown(section);
            return '<section' + attrsToString(slide.attrs) + '>' + slide.html + '</section>';
        }).join('');

        return '<div class="reveal-toolbar"><span class="reveal-toolbar-title">Reveal.js 幻灯片</span><div class="reveal-toolbar-actions"><select class="reveal-theme-select">' + getRevealThemeOptionsHtml() + '</select><button type="button" class="reveal-nav-btn reveal-prev-btn">上一页</button><span class="reveal-page-indicator">1 / 1</span><button type="button" class="reveal-nav-btn reveal-next-btn">下一页</button><button type="button" class="reveal-fullscreen-btn">放映</button></div></div><div class="reveal-stage"><div class="reveal reveal-theme-default" data-theme="default" data-start-h="' + escapeAttr(String(presentationAttrs.startH || 0)) + '" data-start-v="' + escapeAttr(String(presentationAttrs.startV || 0)) + '"><div class="slides">' + slidesHtml + '</div></div></div>';
    }

    function wrapSpecialBlocks(root) {
        var codeBlocks = root.querySelectorAll('pre code');
        Array.prototype.forEach.call(codeBlocks, function (code) {
            var pre = code.parentNode;
            var className = code.className || '';
            var rawCode = code.textContent || '';
            var langMatch = className.match(/language-([\w-]+)/i);
            var lang = langMatch ? langMatch[1].toLowerCase() : '';

            if ((lang === 'mermaid' || lang === 'mmd') && !pre.parentNode.classList.contains('mermaid-host')) {
                var mermaidHost = document.createElement('div');
                var mermaidNode = document.createElement('div');
                mermaidHost.className = 'mermaid-host';
                mermaidNode.className = 'mermaid';
                mermaidNode.setAttribute('data-mermaid-source', rawCode);
                mermaidNode.textContent = rawCode;
                mermaidHost.appendChild(mermaidNode);
                pre.parentNode.replaceChild(mermaidHost, pre);
                return;
            }

            if ((lang === 'reveal' || lang === 'revealjs') && !pre.parentNode.classList.contains('reveal-host')) {
                var revealHost = document.createElement('div');
                revealHost.className = 'reveal-host';
                revealHost.innerHTML = parseRevealMarkdown('```' + lang + '\n' + rawCode + '\n```') + '<div class="render-note">支持 `---` 横向分隔、`--` 纵向分隔、`Note:` 备注、`.element: class="fragment ..."`、`<!-- .slide: ... -->`、`<!-- .element: ... -->`，以及 `<!-- .presentation: data-start-h="1" data-start-v="0" -->` 指定起始页。</div>';
                pre.parentNode.replaceChild(revealHost, pre);
            }
        });
    }

    function applyCodeHighlight(root) {
        if (!window.hljs) {
            return;
        }

        var blocks = root.querySelectorAll('pre code');
        Array.prototype.forEach.call(blocks, function (block) {
            hljs.highlightElement(block);
        });
    }

    function applyCodeLineNumbers(root) {
        var blocks = root.querySelectorAll('pre code');
        Array.prototype.forEach.call(blocks, function (block) {
            var pre = block.parentNode;
            if (!pre || pre.parentNode.classList.contains('code-block-wrap')) {
                return;
            }

            var lineCount = block.textContent.replace(/\n$/, '').split('\n').length;
            if (!lineCount || lineCount < 1) {
                return;
            }

            var wrap = document.createElement('div');
            wrap.className = 'code-block-wrap';

            var lineNumbers = document.createElement('div');
            lineNumbers.className = 'code-line-numbers';

            var nums = [];
            for (var i = 1; i <= lineCount; i++) {
                nums.push(i);
            }
            lineNumbers.textContent = nums.join('\n');

            pre.parentNode.insertBefore(wrap, pre);
            wrap.appendChild(lineNumbers);
            wrap.appendChild(pre);
        });
    }

    function loadScript(src, callback) {
        var existing = document.querySelector('script[data-src="' + src + '"]');
        if (existing) {
            if (callback) {
                if (existing.getAttribute('data-loaded') === '1') {
                    callback();
                } else {
                    existing.addEventListener('load', callback, { once: true });
                }
            }
            return;
        }

        var script = document.createElement('script');
        script.src = src;
        script.setAttribute('data-src', src);
        script.onload = function () {
            script.setAttribute('data-loaded', '1');
            if (callback) {
                callback();
            }
        };
        document.head.appendChild(script);
    }

    function renderMermaid(root, forceRerender) {
        var mermaidNodes = root.querySelectorAll('.mermaid');
        if (!mermaidNodes.length) {
            return;
        }

        function getOffscreenContainer() {
            var container = document.getElementById('mermaid-offscreen-render');
            if (!container) {
                container = document.createElement('div');
                container.id = 'mermaid-offscreen-render';
                container.style.cssText = 'position:fixed;left:-9999px;top:-9999px;width:2000px;height:2000px;overflow:hidden;visibility:visible;opacity:0;pointer-events:none;z-index:-1;';
                document.body.appendChild(container);
            }
            container.innerHTML = '';
            return container;
        }

        function renderMermaidSvg(instance, renderId, source, callback) {
            var result;
            var offscreen = getOffscreenContainer();

            if (!instance || typeof instance.render !== 'function') {
                callback(new Error('Mermaid unavailable'));
                return;
            }

            try {
                result = instance.render(renderId, source, offscreen);
            } catch (syncErr) {
                callback(syncErr);
                return;
            }

            if (result && typeof result.then === 'function') {
                result.then(function (renderResult) {
                    callback(null, renderResult);
                }).catch(function (error) {
                    callback(error);
                });
                return;
            }

            if (result && result.svg) {
                callback(null, result);
            } else {
                callback(new Error('Unexpected render result'));
            }
        }

        function doRender(forceRerender) {
            if (!window.mermaid) {
                return;
            }
            var revealNode = root && root.classList && root.classList.contains('reveal')
                ? root
                : (root.querySelector ? root.querySelector('.reveal') : null);
            var mermaidTheme = revealNode && isDarkRevealTheme(revealNode.getAttribute('data-theme')) ? 'dark' : 'default';
            var renderQueue = [];

            window.mermaid.initialize({ startOnLoad: false, securityLevel: 'loose', theme: mermaidTheme });

            Array.prototype.forEach.call(mermaidNodes, function (node, index) {
                var source = node.getAttribute('data-mermaid-source') || node.textContent || '';

                if (!source.trim()) {
                    return;
                }

                node.setAttribute('data-mermaid-source', source);

                if (!forceRerender && node.getAttribute('data-mermaid-rendered') === '1'
                    && node.getAttribute('data-mermaid-theme') === mermaidTheme) {
                    return;
                }

                renderQueue.push({ node: node, source: source, index: index });
            });

            if (!renderQueue.length) {
                return;
            }

            function processNext(qi) {
                if (qi >= renderQueue.length) {
                    var revealDeck = revealNode && revealNode.__missionRevealDeck;
                    if (revealDeck) {
                        revealDeck.layout();
                    }
                    return;
                }

                var item = renderQueue[qi];
                var renderId = 'mission-mermaid-' + Date.now() + '-' + item.index;

                item.node.removeAttribute('data-processed');

                renderMermaidSvg(window.mermaid, renderId, item.source, function (error, result) {
                    if (!error && result && result.svg) {
                        item.node.innerHTML = result.svg;
                        if (typeof result.bindFunctions === 'function') {
                            result.bindFunctions(item.node);
                        }
                        item.node.setAttribute('data-mermaid-rendered', '1');
                        item.node.setAttribute('data-mermaid-theme', mermaidTheme);
                    } else {
                        item.node.textContent = item.source;
                    }

                    processNext(qi + 1);
                });
            }

            processNext(0);
        }

        if (window.mermaid) {
            doRender(forceRerender);
            return;
        }

        loadScript('../js/vendors/mermaid/mermaid.min.js', function() { doRender(forceRerender); });
    }

    function convertRevealMermaidBlocks(root) {
        var codeBlocks = root.querySelectorAll('.reveal pre code');
        Array.prototype.forEach.call(codeBlocks, function (code) {
            var pre = code.parentNode;
            if (!pre) {
                return;
            }

            var className = code.className || '';
            var langMatch = className.match(/language-([\w-]+)/i);
            var lang = langMatch ? langMatch[1].toLowerCase() : '';
            if (lang !== 'mermaid' && lang !== 'mmd') {
                return;
            }

            if (pre.parentNode && pre.parentNode.classList.contains('mermaid-host')) {
                return;
            }

            var mermaidHost = document.createElement('div');
            var mermaidNode = document.createElement('div');
            mermaidHost.className = 'mermaid-host';
            mermaidNode.className = 'mermaid';
            mermaidNode.setAttribute('data-mermaid-source', code.textContent || '');
            mermaidNode.textContent = code.textContent || '';
            mermaidHost.appendChild(mermaidNode);
            pre.parentNode.replaceChild(mermaidHost, pre);
        });
    }

    function limitRevealCodeBlocks(root) {
        var pres = root.querySelectorAll('.reveal pre');
        Array.prototype.forEach.call(pres, function (pre) {
            pre.classList.remove('reveal-scroll-code');
            if (pre.scrollHeight > Math.min(window.innerHeight * 0.34, 320)) {
                pre.classList.add('reveal-scroll-code');
            }
        });
    }

    function renderReveal(root) {
        var revealHosts = root.querySelectorAll('.reveal-host');
        if (!revealHosts.length) {
            return;
        }

        function doRender() {
            if (!window.Reveal) {
                return;
            }

            Array.prototype.forEach.call(revealHosts, function (host) {
                var node = host.querySelector('.reveal');
                var fullscreenBtn = host.querySelector('.reveal-fullscreen-btn');
                var themeSelect = host.querySelector('.reveal-theme-select');
                var prevBtn = host.querySelector('.reveal-prev-btn');
                var nextBtn = host.querySelector('.reveal-next-btn');
                var pageIndicator = host.querySelector('.reveal-page-indicator');
                if (!node) {
                    return;
                }

                if (node.getAttribute('data-reveal-ready')) {
                    return;
                }

                convertRevealMermaidBlocks(host);
                limitRevealCodeBlocks(host);

                function getDeckSize() {
                    var stage = host.querySelector('.reveal-stage');
                    var stageWidth = stage ? stage.clientWidth : 960;
                    var width = Math.max(640, Math.min(1280, stageWidth));
                    return {
                        width: width,
                        height: Math.round(width * 9 / 16)
                    };
                }

                function autofitSlide(section) {
                    if (!section) {
                        return;
                    }

                    var codeBlocks = section.querySelectorAll('pre');
                    Array.prototype.forEach.call(codeBlocks, function (pre) {
                        pre.classList.remove('reveal-scroll-code');
                        if (pre.scrollHeight > Math.min(window.innerHeight * 0.34, 320)) {
                            pre.classList.add('reveal-scroll-code');
                        }
                    });

                    section.style.removeProperty('--slide-autofit-scale');
                    section.removeAttribute('data-autofit-scale');

                    var scale = 1;
                    var minScale = 0.62;
                    var attempts = 0;

                    while (attempts < 12) {
                        var fitsHeight = section.scrollHeight <= section.clientHeight + 2;
                        var fitsWidth = section.scrollWidth <= section.clientWidth + 2;
                        if (fitsHeight && fitsWidth) {
                            break;
                        }

                        scale -= 0.04;
                        if (scale < minScale) {
                            scale = minScale;
                        }

                        section.style.setProperty('--slide-autofit-scale', scale.toFixed(2) + 'em');
                        section.setAttribute('data-autofit-scale', '1');
                        attempts++;

                        if (scale === minScale) {
                            break;
                        }
                    }
                }

                function autofitAllSlides() {
                    var sections = node.querySelectorAll('.slides section:not(section section)');
                    Array.prototype.forEach.call(sections, function (section) {
                        autofitSlide(section);

                        var childNodes = section.children || [];
                        Array.prototype.forEach.call(childNodes, function (child) {
                            if (child.tagName && child.tagName.toLowerCase() === 'section') {
                                autofitSlide(child);
                            }
                        });
                    });
                }

                function updateIndicator(deck) {
                    if (!pageIndicator) {
                        return;
                    }

                    var indices = deck.getIndices();
                    var total = Math.max(1, deck.getTotalSlides());
                    pageIndicator.innerText = (indices.h + 1) + ' / ' + total;
                }

                function renderCurrentSlideMermaid(forceRerender) {
                    convertRevealMermaidBlocks(host);
                    renderMermaid(node, forceRerender);
                }

                function applyRevealTheme(theme) {
                    var nextTheme = normalizeRevealTheme(theme);

                    for (var i = 0; i < revealThemeOptions.length; i++) {
                        node.classList.remove('reveal-theme-' + revealThemeOptions[i].value);
                    }

                    node.classList.add('reveal-theme-' + nextTheme);
                    node.setAttribute('data-theme', nextTheme);

                    if (themeSelect) {
                        themeSelect.value = nextTheme;
                    }
                }

                function getRevealViewport() {
                    if (node.closest) {
                        return node.closest('.reveal-viewport');
                    }
                    return null;
                }

                function syncFullscreenState() {
                    var viewport = getRevealViewport();
                    var fullscreenElement = document.fullscreenElement || document.webkitFullscreenElement || null;
                    var isFullscreen = !!(fullscreenElement && viewport && (fullscreenElement === viewport || viewport.contains(fullscreenElement)));

                    deck.configure({
                        embedded: !isFullscreen
                    });

                    setTimeout(function () {
                        limitRevealCodeBlocks(host);
                        renderCurrentSlideMermaid(isFullscreen);
                        autofitAllSlides();
                        deck.layout();
                    }, 60);
                }

                var deckSize = getDeckSize();
                var deck = new window.Reveal(node, {
                    embedded: true,
                    hash: false,
                    controls: true,
                    progress: true,
                    center: false,
                    transition: 'slide',
                    width: deckSize.width,
                    height: deckSize.height,
                    margin: 0.04,
                    minScale: 0.2,
                    maxScale: 1.2
                });
                node.__missionRevealDeck = deck;

                deck.initialize().then(function () {
                    var startH = parseInt(node.getAttribute('data-start-h') || '0', 10) || 0;
                    var startV = parseInt(node.getAttribute('data-start-v') || '0', 10) || 0;
                    applyRevealTheme(node.getAttribute('data-theme') || 'default');
                    deck.slide(startH, startV);
                    convertRevealMermaidBlocks(host);
                    limitRevealCodeBlocks(host);
                    renderCurrentSlideMermaid();
                    deck.layout();
                    autofitAllSlides();
                    deck.layout();
                    updateIndicator(deck);
                    node.setAttribute('data-reveal-ready', '1');
                    syncFullscreenState();
                });

                deck.on('slidechanged', function () {
                    limitRevealCodeBlocks(host);
                    renderCurrentSlideMermaid();
                    autofitAllSlides();
                    deck.layout();
                    updateIndicator(deck);
                });

                deck.on('ready', function () {
                    limitRevealCodeBlocks(host);
                    renderCurrentSlideMermaid();
                    autofitAllSlides();
                    deck.layout();
                    updateIndicator(deck);
                });

                window.addEventListener('resize', function () {
                    var nextSize = getDeckSize();
                    deck.configure({
                        width: nextSize.width,
                        height: nextSize.height
                    });
                    limitRevealCodeBlocks(host);
                    autofitAllSlides();
                    deck.layout();
                });

                if (prevBtn && !prevBtn.getAttribute('data-bound')) {
                    prevBtn.setAttribute('data-bound', '1');
                    prevBtn.addEventListener('click', function () {
                        deck.prev();
                        updateIndicator(deck);
                    });
                }

                if (nextBtn && !nextBtn.getAttribute('data-bound')) {
                    nextBtn.setAttribute('data-bound', '1');
                    nextBtn.addEventListener('click', function () {
                        deck.next();
                        updateIndicator(deck);
                    });
                }

                if (themeSelect && !themeSelect.getAttribute('data-bound')) {
                    themeSelect.setAttribute('data-bound', '1');
                    themeSelect.addEventListener('change', function () {
                        applyRevealTheme(themeSelect.value);
                        limitRevealCodeBlocks(host);
                        renderCurrentSlideMermaid(true);
                        autofitAllSlides();
                        deck.layout();
                    });
                }

                if (fullscreenBtn && !fullscreenBtn.getAttribute('data-bound')) {
                    fullscreenBtn.setAttribute('data-bound', '1');
                    fullscreenBtn.addEventListener('click', function () {
                        var viewport = getRevealViewport();
                        if (!viewport) {
                            return;
                        }

                        if (viewport.requestFullscreen) {
                            viewport.requestFullscreen();
                        } else if (viewport.webkitRequestFullscreen) {
                            viewport.webkitRequestFullscreen();
                        }

                        setTimeout(syncFullscreenState, 200);
                    });
                }

                if (!host.getAttribute('data-fullscreen-bound')) {
                    host.setAttribute('data-fullscreen-bound', '1');
                    document.addEventListener('fullscreenchange', syncFullscreenState);
                    document.addEventListener('webkitfullscreenchange', syncFullscreenState);
                }
            });
        }

        if (window.Reveal) {
            doRender();
            return;
        }

        loadScript('../js/vendors/reveal/dist/reveal.js', doRender);
    }

    function getMarkdownEnabled() {
        try {
            var stored = localStorage.getItem(markdownStorageKey);
            return stored === null ? true : stored === '1';
        } catch (e) {
            return true;
        }
    }

    function setMarkdownEnabled(enabled) {
        try {
            localStorage.setItem(markdownStorageKey, enabled ? '1' : '0');
        } catch (e) {
        }
    }

    function updateToggleUI(enabled) {
        var toggle = getToggleElement();
        var status = getToggleStatusElement();
        if (!toggle || !status) {
            return;
        }

        toggle.className = enabled ? 'prog-toggle-switch is-on' : 'prog-toggle-switch';
        toggle.setAttribute('aria-pressed', enabled ? 'true' : 'false');
        status.innerText = enabled ? '当前：开启' : '当前：关闭';
    }

    function setToggleStatusText(text) {
        var status = getToggleStatusElement();
        if (status) {
            status.innerText = text;
        }
    }

    function restoreMissionHtml(content, originalHtml) {
        content.className = '';
        content.style.color = '#334155';
        content.style.lineHeight = '1.85';
        content.style.fontSize = '1.05rem';
        content.style.wordWrap = 'break-word';
        content.style.wordBreak = 'break-word';
        content.innerHTML = originalHtml;
    }

    function restoreOriginalHtml(content) {
        if (window.ContentShowMarkdown && window.ContentShowMarkdown.restoreOriginalHtml) {
            window.ContentShowMarkdown.restoreOriginalHtml(content, 'mission-show-content');
            return;
        }
        content.className = 'mission-show-content';
        content.innerHTML = content.getAttribute('data-original-html') || '';
    }

    function renderMissionMarkdown(forceEnabled) {
        var content = getContentElement();
        var hidden = document.getElementById(hiddenMissionRawId);

        if (!content || !hidden) {
            return;
        }

        if (!content.getAttribute('data-original-html')) {
            content.setAttribute('data-original-html', content.innerHTML || '');
        }

        var markdownSource = getMissionSource(content, hidden);
        var storedMarkdown = getStoredFlagOrNull(markdownStorageKey);
        var storedReveal = getStoredFlagOrNull(revealStorageKey);
        var autoMarkdown = window.ContentShowMarkdown && window.ContentShowMarkdown.looksLikeMarkdown
            ? window.ContentShowMarkdown.looksLikeMarkdown(markdownSource, true)
            : looksLikeMarkdown(markdownSource);
        var autoReveal = window.ContentShowMarkdown && window.ContentShowMarkdown.isRevealMarkdownDocument
            ? window.ContentShowMarkdown.isRevealMarkdownDocument(markdownSource)
            : false;
        var enabled = typeof forceEnabled === 'boolean' ? forceEnabled : (storedMarkdown === null ? autoMarkdown : storedMarkdown);
        var revealEnabled = storedReveal === null ? autoReveal : storedReveal;
        updateToggleUI(enabled);
        updateRevealToggleUI(revealEnabled);

        if (!enabled) {
            restoreOriginalHtml(content);
            setToggleStatusText('当前：关闭');
            return;
        }

        if (!window.marked) {
            restoreOriginalHtml(content);
            setToggleStatusText('当前：开启，未加载解析器');
            return;
        }

        if (!autoMarkdown) {
            restoreOriginalHtml(content);
            setToggleStatusText('当前：开启，未检测到 Markdown');
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
            setToggleStatusText(storedMarkdown === null && autoMarkdown ? '当前：自动开启' : '当前：开启');
            setRevealStatusText(storedReveal === null && revealEnabled ? '当前：自动开启' : (revealEnabled ? '当前：开启' : '当前：关闭'));
        } else {
            restoreOriginalHtml(content);
            setToggleStatusText('当前：开启，渲染失败');
        }
    }

    window.toggleMissionMarkdown = function () {
        var current = getStoredFlagOrNull(markdownStorageKey);
        if (current === null) {
            current = looksLikeMarkdown(getMissionSource(getContentElement(), document.getElementById(hiddenMissionRawId)));
        }
        var nextEnabled = !current;
        setStoredFlag(markdownStorageKey, nextEnabled);
        renderMissionMarkdown(nextEnabled);
    };

    function updateRevealToggleUI(enabled) {
        var toggle = getRevealToggleElement();
        var status = getRevealStatusElement();
        if (toggle) {
            toggle.className = enabled ? 'prog-toggle-switch is-on' : 'prog-toggle-switch';
            toggle.setAttribute('aria-pressed', enabled ? 'true' : 'false');
        }
        if (status) {
            status.innerText = enabled ? '当前：开启' : '当前：关闭';
        }
    }

    function setRevealStatusText(text) {
        var status = getRevealStatusElement();
        if (status) {
            status.innerText = text;
        }
    }

    window.toggleMissionReveal = function () {
        var current = getStoredFlagOrNull(revealStorageKey);
        if (current === null) {
            current = window.ContentShowMarkdown && window.ContentShowMarkdown.isRevealMarkdownDocument
                ? window.ContentShowMarkdown.isRevealMarkdownDocument(getMissionSource(getContentElement(), document.getElementById(hiddenMissionRawId)))
                : false;
        }
        var nextEnabled = !current;
        setStoredFlag(revealStorageKey, nextEnabled);
        renderMissionMarkdown();
    };

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', function () {
            updateToggleUI(getStoredFlagOrNull(markdownStorageKey) === true);
            updateRevealToggleUI(getStoredFlagOrNull(revealStorageKey) === true);
            renderMissionMarkdown();
        });
    } else {
        updateToggleUI(getStoredFlagOrNull(markdownStorageKey) === true);
        updateRevealToggleUI(getStoredFlagOrNull(revealStorageKey) === true);
        renderMissionMarkdown();
    }
})();

/* ================================================================
   全局辅助函数
   ================================================================ */
function jsCopy(contentid) {
    var e = document.getElementById(contentid);
    e.select();
    document.execCommand("Copy");
}
