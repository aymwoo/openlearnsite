(function () {
    var officialRevealThemeOptions = [
        { value: 'white', label: 'White' },
        { value: 'black', label: 'Black' },
        { value: 'sky', label: 'Sky' },
        { value: 'beige', label: 'Beige' },
        { value: 'simple', label: 'Simple' },
        { value: 'serif', label: 'Serif' },
        { value: 'moon', label: 'Moon' },
        { value: 'night', label: 'Night' },
        { value: 'solarized', label: 'Solarized' }
    ];
    var studentRevealThemeOptions = [
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

    function useStudentRevealThemes() {
        return !!window.__showmissionConfig;
    }

    function getRevealThemeOptions() {
        return useStudentRevealThemes() ? studentRevealThemeOptions : officialRevealThemeOptions;
    }

    function normalizeRevealTheme(theme) {
        var value = (theme || '').toLowerCase();
        var options = getRevealThemeOptions();
        for (var i = 0; i < options.length; i++) {
            if (options[i].value === value) {
                return value;
            }
        }
        return options[0].value;
    }

    function getConfig() {
        return window.__contentShowMarkdown || null;
    }

    function getContentElement() {
        var config = getConfig();
        return config ? document.getElementById(config.contentId) : null;
    }

    function hasMeaningfulHtml(html) {
        var normalized = (html || '').trim();
        if (!normalized) {
            return false;
        }

        return /<\s*([a-z][\w:-]*)\b[^>]*>/i.test(normalized);
    }

    function stripHtmlToText(html) {
        var holder = document.createElement('div');
        holder.innerHTML = html || '';
        return (holder.textContent || holder.innerText || '').replace(/\r/g, '').trim();
    }

    function looksLikeMarkdown(text, allowPlainMultiline) {
        if (!text) {
            return false;
        }

        var normalized = text.replace(/\r/g, '').trim();
        if (!normalized) {
            return false;
        }

        if (/^\s*<[^>]+>/m.test(normalized) && !/^\s*<(pre|code)\b/i.test(normalized)) {
            return false;
        }

        if (allowPlainMultiline && /\n/.test(normalized)) {
            return true;
        }

        return /(^|\n)\s{0,3}(#{1,6}\s+.+|[-*+]\s+.+|\d+\.\s+.+|>\s+.+|```[\s\S]*?```|~~~[\s\S]*?~~~|\|.+\|\s*$|!\[[^\]]*\]\([^\)]+\)|\[[^\]]+\]\([^\)]+\)|-{3,}|\*{3,}|`[^`]+`)/m.test(normalized);
    }

    function isRevealMarkdownDocument(text) {
        if (!looksLikeMarkdown(text, true)) {
            return false;
        }

        var normalized = text.replace(/\r/g, '').trim();
        if (!normalized) {
            return false;
        }

        return /^```(?:reveal|revealjs)\b/m.test(normalized)
            || /^\s*--\s*$/m.test(normalized)
            || /^\s*(?:note|notes)\s*:/mi.test(normalized)
            || /^\s*\.element\s*:/mi.test(normalized)
            || /^\s*<!--\s*\.slide\s*:/mi.test(normalized)
            || /^\s*<!--\s*\.element\s*:/mi.test(normalized)
            || normalized.split(/^---$/m).filter(function (part) { return part.trim(); }).length >= 2;
    }

    function escapeHtml(value) {
        return (value || '')
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;');
    }

    function getRevealThemeOptionsHtml() {
        return getRevealThemeOptions().map(function (theme) {
            return '<option value="' + escapeHtml(theme.value) + '">' + escapeHtml(theme.label) + '</option>';
        }).join('');
    }

    function applyRevealThemeClass(node, theme) {
        var nextTheme = normalizeRevealTheme(theme);
        var options = getRevealThemeOptions();
        var i;

        for (i = 0; i < options.length; i++) {
            node.classList.remove('reveal-theme-' + options[i].value);
        }

        node.classList.add('reveal-theme-' + nextTheme);
        node.setAttribute('data-theme', nextTheme);

        return nextTheme;
    }

    function getRevealThemeHref(theme) {
        return '../js/vendors/reveal/dist/theme/' + normalizeRevealTheme(theme) + '.css';
    }

    function ensureRevealThemeStylesheet(theme) {
        var href = getRevealThemeHref(theme);
        var link = document.querySelector('link[data-reveal-theme-stylesheet="1"]')
            || document.querySelector('link[href*="/js/vendors/reveal/dist/theme/"]')
            || document.querySelector('link[href*="../js/vendors/reveal/dist/theme/"]')
            || document.querySelector('link[href*="js/vendors/reveal/dist/theme/"]');

        if (!link) {
            link = document.createElement('link');
            link.rel = 'stylesheet';
            document.head.appendChild(link);
        }

        link.setAttribute('data-reveal-theme-stylesheet', '1');
        if (link.getAttribute('href') !== href) {
            link.setAttribute('href', href);
        }
    }

    function applyRevealTheme(node, theme) {
        if (useStudentRevealThemes()) {
            return applyRevealThemeClass(node, theme);
        }

        var nextTheme = normalizeRevealTheme(theme);
        node.setAttribute('data-theme', nextTheme);
        ensureRevealThemeStylesheet(nextTheme);
        return nextTheme;
    }

    function parseRevealMarkdown(blockText) {
        var cleaned = blockText.replace(/^```(?:reveal|revealjs)?\s*/i, '').replace(/```\s*$/, '').trim();
        var slidesHtml = cleaned.split(/^---$/m).map(function (section) {
            var verticalSlides = section.trim().split(/^--$/m).map(function (item) {
                return item.trim();
            }).filter(Boolean);

            if (verticalSlides.length > 1) {
                return '<section>' + verticalSlides.map(function (slide) {
                    return '<section>' + marked.parse(slide) + '</section>';
                }).join('') + '</section>';
            }

            return '<section>' + marked.parse(section.trim()) + '</section>';
        }).filter(Boolean).join('');

        var initialTheme = getRevealThemeOptions()[0].value;
        return '<div class="reveal-toolbar"><span class="reveal-toolbar-title">Reveal.js 幻灯片</span><div class="reveal-toolbar-actions"><select class="reveal-theme-select">' + getRevealThemeOptionsHtml() + '</select><button type="button" class="reveal-nav-btn reveal-prev-btn">上一页</button><span class="reveal-page-indicator">1 / 1</span><button type="button" class="reveal-nav-btn reveal-next-btn">下一页</button><button type="button" class="reveal-fullscreen-btn">放映</button></div></div><div class="reveal-stage"><div class="reveal reveal-theme-' + escapeHtml(initialTheme) + '" data-theme="' + escapeHtml(initialTheme) + '"><div class="slides">' + slidesHtml + '</div></div></div>';
    }

    function looksLikeMermaidDocument(text) {
        var normalized = (text || '').replace(/\r/g, '').trim();
        if (!normalized) {
            return false;
        }

        if (/^```(?:mermaid|mmd)\b/i.test(normalized)) {
            return false;
        }

        if (!/^(?:graph\s+(?:TB|BT|RL|LR|TD)|flowchart\s+(?:TB|BT|RL|LR|TD)|sequenceDiagram|classDiagram|stateDiagram(?:-v2)?|erDiagram|journey|gantt|pie|mindmap|timeline|quadrantChart|requirementDiagram|gitGraph|c4Context|c4Container|c4Component|c4Dynamic|c4Deployment)\b/i.test(normalized)) {
            return false;
        }

        return !/(^|\n)```|(^|\n)---\s*$|(^|\n)#{1,6}\s|(^|\n)>\s|\[[^\]]+\]\([^)]+\)/m.test(normalized);
    }

    function wrapBareMermaidDocument(content, source) {
        var mermaidHost = document.createElement('div');
        var mermaidNode = document.createElement('div');
        mermaidHost.className = 'mermaid-host';
        mermaidNode.className = 'mermaid';
        mermaidNode.setAttribute('data-mermaid-source', source);
        mermaidNode.textContent = source;
        mermaidHost.appendChild(mermaidNode);
        content.innerHTML = '';
        content.appendChild(mermaidHost);
    }

    function getMarkdownSource(content, preferredSource) {
        if (preferredSource && looksLikeMarkdown(preferredSource, true)) {
            return preferredSource;
        }

        var originalHtml = content.getAttribute('data-original-html') || content.innerHTML || '';
        var text = content.textContent || '';
        var allowPlainMultiline = !hasMeaningfulHtml(originalHtml);

        if (looksLikeMarkdown(text, allowPlainMultiline)) {
            return text;
        }

        var plain = stripHtmlToText(originalHtml);
        return looksLikeMarkdown(plain, allowPlainMultiline) ? plain : '';
    }

    function wrapSpecialBlocks(root, enableReveal) {
        var codeBlocks = root.querySelectorAll('pre code');
        Array.prototype.forEach.call(codeBlocks, function (code) {
            var pre = code.parentNode;
            if (!pre || !pre.parentNode) {
                return;
            }

            var className = code.className || '';
            var langMatch = className.match(/(?:language|lang)-([\w-]+)/i);
            var lang = langMatch ? langMatch[1].toLowerCase() : '';
            var rawCode = code.textContent || '';

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

            if (enableReveal && (lang === 'reveal' || lang === 'revealjs') && !pre.parentNode.classList.contains('reveal-host')) {
                var revealHost = document.createElement('div');
                revealHost.className = 'reveal-host';
                revealHost.innerHTML = parseRevealMarkdown('```' + lang + '\n' + rawCode + '\n```');
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
            var className = block.className || '';
            if (/(?:language|lang)-(?:mermaid|mmd)\b/i.test(className)) {
                return;
            }
            window.hljs.highlightElement(block);
        });
    }

    function applyCodeLineNumbers(root) {
        var blocks = root.querySelectorAll('pre code');
        Array.prototype.forEach.call(blocks, function (block) {
            var pre = block.parentNode;
            if (!pre || !pre.parentNode || pre.parentNode.classList.contains('code-block-wrap')) {
                return;
            }

            var lineCount = block.textContent.replace(/\n$/, '').split('\n').length;
            if (lineCount < 1) {
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

    function renderMermaid(root) {
        var mermaidNodes = root.querySelectorAll('.mermaid');
        if (!mermaidNodes.length) {
            return;
        }

        function showMermaidError(node, source, error) {
            var message = error && error.message ? error.message : '未知错误';
            node.innerHTML = '<div class="mermaid-error-card"><div class="mermaid-error-title">Mermaid 图表解析失败</div><div class="mermaid-error-message">' + escapeHtml(message) + '</div><details class="mermaid-error-details"><summary>查看原始 Mermaid 源码</summary><pre><code>' + escapeHtml(source) + '</code></pre></details></div>';
        }

        function doRender() {
            if (!window.mermaid) {
                return;
            }

            window.mermaid.initialize({ startOnLoad: false, securityLevel: 'loose', theme: 'default' });
            Array.prototype.forEach.call(mermaidNodes, function (node, index) {
                var source = node.getAttribute('data-mermaid-source') || node.textContent || '';
                if (!source.trim()) {
                    return;
                }

                try {
                    var renderId = 'content-show-mermaid-' + Date.now() + '-' + index;
                    var result = window.mermaid.render(renderId, source);
                    if (result && typeof result.then === 'function') {
                        result.then(function (rendered) {
                            if (rendered && rendered.svg) {
                                node.innerHTML = rendered.svg;
                            }
                        }).catch(function (error) {
                            showMermaidError(node, source, error);
                        });
                    } else if (result && result.svg) {
                        node.innerHTML = result.svg;
                    }
                } catch (e) {
                    showMermaidError(node, source, e);
                }
            });
        }

        if (window.mermaid) {
            doRender();
        } else {
            loadScript('../js/vendors/mermaid/mermaid.min.js', function () {
                doRender();
            });
        }
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
                var themeSelect = host.querySelector('.reveal-theme-select');
                var prevBtn = host.querySelector('.reveal-prev-btn');
                var nextBtn = host.querySelector('.reveal-next-btn');
                var fullscreenBtn = host.querySelector('.reveal-fullscreen-btn');
                var pageIndicator = host.querySelector('.reveal-page-indicator');

                if (!node || node.getAttribute('data-reveal-ready')) {
                    return;
                }

                var deck = new window.Reveal(node, {
                    embedded: true,
                    hash: false,
                    controls: true,
                    progress: true,
                    center: false,
                    transition: 'slide',
                    width: 1280,
                    height: 720,
                    margin: 0.04,
                    minScale: 0.2,
                    maxScale: 1.2
                });

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

                    deck.configure({ embedded: !isFullscreen });
                    deck.layout();
                }

                function updateIndicator() {
                    if (pageIndicator) {
                        var indices = deck.getIndices();
                        pageIndicator.innerText = (indices.h + 1) + ' / ' + Math.max(1, deck.getTotalSlides());
                    }
                }

                deck.initialize().then(function () {
                    var activeTheme = applyRevealTheme(node, node.getAttribute('data-theme'));
                    if (themeSelect) {
                        themeSelect.value = activeTheme;
                    }
                    updateIndicator();
                    node.setAttribute('data-reveal-ready', '1');
                    syncFullscreenState();
                });

                deck.on('slidechanged', updateIndicator);
                deck.on('ready', updateIndicator);

                if (prevBtn && !prevBtn.getAttribute('data-bound')) {
                    prevBtn.setAttribute('data-bound', '1');
                    prevBtn.addEventListener('click', function () {
                        deck.prev();
                        updateIndicator();
                    });
                }

                if (nextBtn && !nextBtn.getAttribute('data-bound')) {
                    nextBtn.setAttribute('data-bound', '1');
                    nextBtn.addEventListener('click', function () {
                        deck.next();
                        updateIndicator();
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

                if (themeSelect && !themeSelect.getAttribute('data-bound')) {
                    themeSelect.setAttribute('data-bound', '1');
                    themeSelect.addEventListener('change', function () {
                        themeSelect.value = applyRevealTheme(node, themeSelect.value);
                        deck.layout();
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
        } else {
            loadScript('../js/vendors/reveal/dist/reveal.js', function () {
                doRender();
            });
        }
    }

    function restoreOriginalHtml(content, originalClass) {
        var storedClass = content.getAttribute('data-original-class');
        content.className = originalClass || storedClass || content.className;
        content.innerHTML = content.getAttribute('data-original-html') || '';
    }

    function renderIntoContent(content, options) {
        options = options || {};
        if (!content || !window.marked) {
            return { rendered: false, reason: 'unavailable' };
        }

        if (!content.getAttribute('data-original-html')) {
            content.setAttribute('data-original-html', content.innerHTML || '');
        }
        if (!content.getAttribute('data-original-class')) {
            content.setAttribute('data-original-class', content.className || '');
        }

        var source = options.source || getMarkdownSource(content);
        if (!looksLikeMarkdown(source, true)) {
            restoreOriginalHtml(content, options.originalClass);
            return { rendered: false, reason: 'not-markdown', source: source };
        }

        marked.setOptions({ breaks: true, gfm: true });

        var enableReveal = options.enableReveal !== false;
        var renderedClass = options.renderedClass || content.getAttribute('data-original-class') || content.className;
        content.className = renderedClass;

        if (enableReveal && isRevealMarkdownDocument(source)) {
            content.innerHTML = '<div class="vditor-reset"><div class="reveal-host">' + parseRevealMarkdown(source) + '<div class="render-note">当前已按 Reveal.js 演示文稿模式渲染。</div></div></div>';
        } else if (looksLikeMermaidDocument(source) && !/^```(?:mermaid|mmd)\b/i.test(source.replace(/\r/g, '').trim())) {
            if (options.wrapInVditorReset) {
                content.innerHTML = '<div class="vditor-reset"></div>';
                wrapBareMermaidDocument(content.firstChild, source);
            } else {
                wrapBareMermaidDocument(content, source);
            }
        } else if (options.wrapInVditorReset) {
            content.innerHTML = '<div class="vditor-reset">' + marked.parse(source) + '</div>';
        } else {
            content.innerHTML = marked.parse(source);
        }

        wrapSpecialBlocks(content, enableReveal);
        applyCodeHighlight(content);
        applyCodeLineNumbers(content);
        renderMermaid(content);
        if (enableReveal) {
            renderReveal(content);
        }

        return { rendered: true, source: source, reveal: enableReveal && isRevealMarkdownDocument(source) };
    }

    function renderContent() {
        var content = getContentElement();
        if (!content || !window.marked) {
            return;
        }

        var source = getMarkdownSource(content);
        if (!source) {
            return;
        }

        var originalClass = content.getAttribute('data-original-class') || content.className || '';
        var renderedClass = originalClass.indexOf('content-show-markdown') >= 0 ? originalClass : (originalClass ? originalClass + ' content-show-markdown' : 'content-show-markdown');
        renderIntoContent(content, {
            source: source,
            enableReveal: true,
            renderedClass: renderedClass,
            wrapInVditorReset: false,
            originalClass: originalClass
        });
    }

    window.ContentShowMarkdown = {
        hasMeaningfulHtml: hasMeaningfulHtml,
        stripHtmlToText: stripHtmlToText,
        looksLikeMarkdown: looksLikeMarkdown,
        looksLikeMermaidDocument: looksLikeMermaidDocument,
        isRevealMarkdownDocument: isRevealMarkdownDocument,
        getMarkdownSource: getMarkdownSource,
        restoreOriginalHtml: restoreOriginalHtml,
        renderIntoContent: renderIntoContent
    };

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', renderContent);
    } else {
        renderContent();
    }
})();
