/**
 * LearnSite — Shared Drag-and-Drop FileUpload Component
 * =====================================================
 * Wraps ASP.NET <asp:FileUpload> controls with a modern drag-and-drop zone.
 * The original <input type="file"> remains functional for PostBack.
 *
 * Usage:
 *   1. Include CSS + JS:
 *        <link href="~/js/fileupload.css" rel="stylesheet" />
 *        <script src="~/js/fileupload.js"></script>
 *
 *   2. Wrap the FileUpload control in a div.ls-upload with data attributes:
 *        <div class="ls-upload"
 *             data-accept=".xml,.cls"
 *             data-max-size="10240"
 *             data-label="点击或拖拽上传文件"
 *             data-hint="支持 xml / cls 格式">
 *            <asp:FileUpload ID="FuClassModel" runat="server" />
 *        </div>
 *
 *   Data attributes (all optional):
 *     data-accept     Comma-separated extensions, e.g. ".xml,.cls,.zip"
 *     data-max-size   Max file size in KB (e.g. "2048" = 2 MB)
 *     data-label      Primary text shown in the drop zone
 *     data-hint       Secondary hint text (file types, size notes)
 */
(function () {
    'use strict';

    // SVG icons
    var ICON_UPLOAD = '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"><path d="M7 16a4 4 0 0 1-.88-7.903A5 5 0 1 1 15.9 6h.1a5 5 0 0 1 1 9.9"/><path d="M15 13l-3-3m0 0l-3 3m3-3v12"/></svg>';
    var ICON_CHECK = '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"><path d="M9 12l2 2 4-4"/><circle cx="12" cy="12" r="10"/></svg>';
    var ICON_WARN = '<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"><path d="M12 9v4m0 4h.01M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z"/></svg>';

    /**
     * Format file size in human-readable form
     */
    function formatSize(bytes) {
        if (bytes < 1024) return bytes + ' B';
        if (bytes < 1024 * 1024) return (bytes / 1024).toFixed(1) + ' KB';
        return (bytes / (1024 * 1024)).toFixed(2) + ' MB';
    }

    /**
     * Validate file against accept extensions and max size
     * Returns { valid: bool, error: string|null }
     */
    function validateFile(file, acceptStr, maxSizeKB) {
        // Extension check
        if (acceptStr) {
            var exts = acceptStr.split(',').map(function (e) {
                return e.trim().toLowerCase().replace(/^\.?/, '.');
            });
            var fileName = file.name.toLowerCase();
            var dotIdx = fileName.lastIndexOf('.');
            var fileExt = dotIdx >= 0 ? fileName.substring(dotIdx) : '';
            if (exts.length > 0 && exts[0] !== '' && exts.indexOf(fileExt) === -1) {
                return { valid: false, error: '不支持此文件类型，请选择 ' + exts.join(' / ') + ' 文件' };
            }
        }
        // Size check
        if (maxSizeKB) {
            var maxBytes = parseFloat(maxSizeKB) * 1024;
            if (file.size > maxBytes) {
                return { valid: false, error: '文件过大（' + formatSize(file.size) + '），最大允许 ' + formatSize(maxBytes) };
            }
        }
        return { valid: true, error: null };
    }

    /**
     * Initialize a single .ls-upload wrapper
     */
    function initUploadZone(zone) {
        // Find the file input inside
        var fileInput = zone.querySelector('input[type="file"]');
        if (!fileInput) return;

        // Read config from data attributes
        var acceptStr = zone.getAttribute('data-accept') || '';
        var maxSizeKB = zone.getAttribute('data-max-size') || '';
        var labelText = zone.getAttribute('data-label') || '点击或拖拽上传文件';
        var hintText = zone.getAttribute('data-hint') || '';

        // Set accept attribute on the native input for OS file dialog filtering
        if (acceptStr) {
            fileInput.setAttribute('accept', acceptStr);
        }

        // Build inner DOM (prepend before file input so input stays on top via z-index)
        var iconEl = document.createElement('div');
        iconEl.className = 'ls-upload__icon';
        iconEl.innerHTML = ICON_UPLOAD;

        var textEl = document.createElement('div');
        textEl.className = 'ls-upload__text';
        textEl.textContent = labelText;

        var hintEl = document.createElement('div');
        hintEl.className = 'ls-upload__hint';
        hintEl.textContent = hintText;

        var fileNameEl = document.createElement('div');
        fileNameEl.className = 'ls-upload__file-name';

        var sizeEl = document.createElement('div');
        sizeEl.className = 'ls-upload__size';

        var errorEl = document.createElement('div');
        errorEl.className = 'ls-upload__error';

        var removeBtn = document.createElement('button');
        removeBtn.type = 'button';
        removeBtn.className = 'ls-upload__remove';
        removeBtn.title = '清除选择';
        removeBtn.innerHTML = '&times;';

        // Remove any existing CssClass / inline styles from the file input
        // (keep only position/opacity from CSS)
        fileInput.removeAttribute('class');
        fileInput.removeAttribute('style');

        // Insert elements
        zone.insertBefore(removeBtn, fileInput);
        zone.insertBefore(iconEl, fileInput);
        zone.insertBefore(textEl, fileInput);
        if (hintText) {
            zone.insertBefore(hintEl, fileInput);
        }
        zone.insertBefore(fileNameEl, fileInput);
        zone.insertBefore(sizeEl, fileInput);
        zone.insertBefore(errorEl, fileInput);

        // State management
        function setState(file) {
            zone.classList.remove('ls-upload--has-file', 'ls-upload--error', 'ls-upload--dragover');
            errorEl.textContent = '';
            fileNameEl.textContent = '';
            sizeEl.textContent = '';

            if (!file) {
                // Reset
                iconEl.innerHTML = ICON_UPLOAD;
                textEl.textContent = labelText;
                return;
            }

            var result = validateFile(file, acceptStr, maxSizeKB);
            fileNameEl.textContent = file.name;
            sizeEl.textContent = formatSize(file.size);

            if (result.valid) {
                zone.classList.add('ls-upload--has-file');
                iconEl.innerHTML = ICON_CHECK;
                textEl.textContent = '文件已选择';
            } else {
                zone.classList.add('ls-upload--error');
                iconEl.innerHTML = ICON_WARN;
                textEl.textContent = '文件无效';
                errorEl.textContent = result.error;
            }
        }

        // File input change
        fileInput.addEventListener('change', function () {
            var file = this.files && this.files[0];
            setState(file || null);
        });

        // Drag events
        zone.addEventListener('dragenter', function (e) {
            e.preventDefault();
            e.stopPropagation();
            zone.classList.add('ls-upload--dragover');
        });

        zone.addEventListener('dragover', function (e) {
            e.preventDefault();
            e.stopPropagation();
            zone.classList.add('ls-upload--dragover');
        });

        zone.addEventListener('dragleave', function (e) {
            e.preventDefault();
            e.stopPropagation();
            // Only remove if leaving the zone (not entering a child)
            if (!zone.contains(e.relatedTarget)) {
                zone.classList.remove('ls-upload--dragover');
            }
        });

        zone.addEventListener('drop', function (e) {
            e.preventDefault();
            e.stopPropagation();
            zone.classList.remove('ls-upload--dragover');

            var files = e.dataTransfer && e.dataTransfer.files;
            if (files && files.length > 0) {
                // We need to set the file on the native input.
                // Direct assignment to input.files via DataTransfer is supported in modern browsers.
                try {
                    var dt = new DataTransfer();
                    dt.items.add(files[0]);
                    fileInput.files = dt.files;
                } catch (err) {
                    // Fallback: older browsers can't set input.files
                    // The file won't be submitted via PostBack, but we show the preview
                    // and user can click to re-select
                }
                setState(files[0]);
            }
        });

        // Remove/clear button
        removeBtn.addEventListener('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            // Clear the native input
            try {
                var dt = new DataTransfer();
                fileInput.files = dt.files;
            } catch (err) {
                fileInput.value = '';
            }
            setState(null);
        });

        // Mark as initialized
        zone.setAttribute('data-ls-init', 'true');
    }

    /**
     * Initialize all .ls-upload zones on the page
     */
    function initAll() {
        var zones = document.querySelectorAll('.ls-upload:not([data-ls-init])');
        for (var i = 0; i < zones.length; i++) {
            initUploadZone(zones[i]);
        }
    }

    // Auto-init on DOM ready
    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initAll);
    } else {
        initAll();
    }

    // Expose for manual init (e.g. after dynamic content load)
    window.LSUpload = {
        init: initAll,
        initZone: initUploadZone
    };
})();
