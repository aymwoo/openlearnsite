window.LearnSiteCourseBanner = (function () {
    function normalizeBannerUrl(url) {
        if (!url) {
            return '';
        }

        url = (url + '').replace(/^\s+|\s+$/g, '');
        if (!url) {
            return '';
        }

        if (/^https?:\/\//i.test(url)) {
            var parser = document.createElement('a');
            parser.href = url;
            return parser.pathname + (parser.search || '');
        }

        return url;
    }

    function init(options) {
        options = options || {};

        var trigger = document.getElementById(options.triggerId);
        var modal = document.getElementById(options.modalId || 'BannerModal');
        var closeButton = document.getElementById(options.closeButtonId || 'BannerModalClose');
        var cancelButton = document.getElementById(options.cancelButtonId || 'BannerModalCancel');
        var uploadButton = document.getElementById(options.uploadButtonId || 'BannerUploadButton');
        var dropzone = document.getElementById(options.dropzoneId || 'BannerDropzone');
        var fileInput = document.getElementById(options.fileInputId || 'BannerFileInput');
        var stage = document.getElementById(options.stageId || 'BannerPreviewStage');
        var empty = document.getElementById(options.emptyId || 'BannerPreviewEmpty');
        var status = document.getElementById(options.statusId || 'BannerUploadStatus');
        var progress = document.getElementById(options.progressId || 'BannerUploadProgress');
        var progressBar = document.getElementById(options.progressBarId || 'BannerUploadProgressBar');
        var currentState = document.getElementById(options.currentStateId || 'BannerCurrentState');
        var target = document.getElementById(options.targetId);
        var hiddenBannerUrl = document.getElementById(options.hiddenBannerUrlId);
        var courseIdField = document.getElementById(options.hiddenCourseId);
        var bannerLink = options.linkId ? document.getElementById(options.linkId) : null;
        var selectedFile = null;
        var localPreviewUrl = '';
        var previewUrl = hiddenBannerUrl ? hiddenBannerUrl.value : '';
        var originalUrl = previewUrl;
        var isUploading = false;

        if (!trigger || !modal || !dropzone || !fileInput || !stage || !uploadButton) {
            return;
        }

        function updateBannerLink(url) {
            if (!bannerLink) {
                return;
            }

            if (url) {
                bannerLink.href = url;
                bannerLink.style.display = 'inline-flex';
            } else {
                bannerLink.removeAttribute('href');
                bannerLink.style.display = 'none';
            }
        }

        function setStatus(message, cls) {
            if (!status) {
                return;
            }

            status.className = 'course-show-banner-status';
            if (cls) {
                status.className += ' ' + cls;
            }
            status.innerHTML = message || '';
        }

        function setCurrentState(hasBanner, pending) {
            if (!currentState) {
                return;
            }

            if (pending) {
                currentState.innerHTML = '当前封面：待上传新图片';
                return;
            }

            currentState.innerHTML = hasBanner ? '当前封面：已设置课程横幅' : '当前封面：默认样式';
        }

        function setProgress(value, visible) {
            if (!progress || !progressBar) {
                return;
            }

            if (visible) {
                progress.removeAttribute('hidden');
            } else {
                progress.setAttribute('hidden', 'hidden');
            }

            progressBar.style.width = Math.max(0, Math.min(100, value || 0)) + '%';
        }

        function setUploadingState(uploading) {
            isUploading = uploading;
            uploadButton.disabled = uploading;
            uploadButton.innerHTML = uploading ? '正在上传...' : '重新上传';
            if (!uploading) {
                setProgress(0, false);
            }
        }

        function pulsePreviewStage() {
            stage.className = stage.className.replace(/\s?is-refreshing/g, '');
            stage.offsetWidth;
            stage.className += ' is-refreshing';
            window.setTimeout(function () {
                stage.className = stage.className.replace(/\s?is-refreshing/g, '');
            }, 460);
        }

        function pulseTarget() {
            if (!target) {
                return;
            }

            target.className = target.className.replace(/\s?is-refreshing/g, '');
            target.offsetWidth;
            target.className += ' is-refreshing';
            window.setTimeout(function () {
                target.className = target.className.replace(/\s?is-refreshing/g, '');
            }, 460);
        }

        function applyStage(url) {
            previewUrl = url || '';
            stage.style.backgroundImage = previewUrl ? "url('" + previewUrl.replace(/'/g, "%27") + "')" : '';
            if (previewUrl) {
                if (stage.className.indexOf('has-image') === -1) {
                    stage.className += ' has-image';
                }
                if (empty) {
                    empty.style.display = 'none';
                }
            } else {
                stage.className = stage.className.replace(/\s?has-image/g, '');
                if (empty) {
                    empty.style.display = 'flex';
                }
            }
        }

        function clearLocalPreview() {
            if (localPreviewUrl && window.URL && window.URL.revokeObjectURL) {
                window.URL.revokeObjectURL(localPreviewUrl);
            }
            localPreviewUrl = '';
        }

        function applyTarget(url) {
            if (!target) {
                return;
            }

            if (url) {
                if (target.className.indexOf('has-banner') === -1) {
                    target.className += ' has-banner';
                }
                target.style.backgroundImage = "url('" + url.replace(/'/g, "%27") + "')";
            } else {
                target.className = target.className.replace(/\s?has-banner/g, '');
                target.style.backgroundImage = '';
            }

            pulseTarget();
        }

        function uploadSelectedBanner() {
            var cid = courseIdField ? courseIdField.value : '';
            var formData;

            if (isUploading) {
                return false;
            }

            if (!selectedFile) {
                setStatus('请先选择一张新的横幅图片', 'is-error');
                return false;
            }

            if (!cid) {
                setStatus('缺少课程编号，无法上传横幅', 'is-error');
                return false;
            }

            if (!window.FormData || !window.jQuery || !jQuery.ajax) {
                setStatus('当前页面缺少上传能力支持', 'is-error');
                return false;
            }

            formData = new FormData();
            formData.append('action', 'upload');
            formData.append('cid', cid);
            formData.append('banner', selectedFile);

            setUploadingState(true);
            setProgress(8, true);
            setStatus('正在上传并更新横幅...', '');

            jQuery.ajax({
                url: options.uploadUrl || 'coursebanner.ashx',
                type: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                dataType: 'json',
                xhr: function () {
                    var xhr = jQuery.ajaxSettings.xhr();
                    if (xhr && xhr.upload) {
                        xhr.upload.onprogress = function (ev) {
                            if (ev.lengthComputable) {
                                setProgress(Math.round((ev.loaded / ev.total) * 100), true);
                            }
                        };
                    }
                    return xhr;
                },
                success: function (response) {
                    var storedUrl;
                    setUploadingState(false);
                    if (!response || response.success !== true || !response.bannerUrl) {
                        setStatus(response && response.message ? response.message : '横幅更新失败，请重试', 'is-error');
                        return;
                    }

                    storedUrl = normalizeBannerUrl(response.bannerUrl);
                    originalUrl = storedUrl;
                    selectedFile = null;
                    clearLocalPreview();
                    if (hiddenBannerUrl) {
                        hiddenBannerUrl.value = storedUrl;
                    }
                    updateBannerLink(storedUrl);
                    fileInput.value = '';
                    applyStage(response.bannerUrl);
                    applyTarget(response.bannerUrl);
                    setCurrentState(true, false);
                    setStatus(response.message || '横幅已更新', 'is-success');
                    if (typeof options.onUploadSuccess === 'function') {
                        options.onUploadSuccess(response, storedUrl);
                    }
                    window.setTimeout(function () {
                        closeModal();
                    }, 700);
                },
                error: function () {
                    setUploadingState(false);
                    setStatus('上传失败，请检查网络后重试', 'is-error');
                }
            });

            return false;
        }

        function validateAndPreview(file) {
            var type = (file.type || '').toLowerCase();
            var objectUrl;
            var img;

            if (type.indexOf('image/') !== 0) {
                setStatus('请选择图片文件', 'is-error');
                return;
            }

            if (file.size > 5 * 1024 * 1024) {
                setStatus('图片大小不能超过 5MB', 'is-error');
                return;
            }

            if (!window.URL || !window.URL.createObjectURL) {
                selectedFile = file;
                setStatus('已选择新图片，正在上传横幅...', '');
                uploadSelectedBanner();
                return;
            }

            clearLocalPreview();
            objectUrl = window.URL.createObjectURL(file);
            localPreviewUrl = objectUrl;
            img = new Image();
            img.onload = function () {
                var width = img.width || 0;
                var height = img.height || 0;
                var ratio = height ? (width / height) : 0;
                var tips = [];

                if (width < 960 || height < 320) {
                    clearLocalPreview();
                    setStatus('建议上传更大的横幅图片，至少 960 x 320', 'is-error');
                    return;
                }

                if (ratio < 2.1) {
                    tips.push('当前图片偏窄，建议使用更宽的横向封面');
                }

                if (ratio > 4.6) {
                    tips.push('当前图片过宽，封面展示时可能被裁切');
                }

                selectedFile = file;
                applyStage(objectUrl);
                pulsePreviewStage();
                setCurrentState(true, true);
                setStatus(tips.length ? tips.join('；') + '，正在上传横幅...' : '图片校验通过，正在上传横幅...', tips.length ? '' : '');
                uploadSelectedBanner();
            };

            img.onerror = function () {
                clearLocalPreview();
                setStatus('图片读取失败，请重新选择', 'is-error');
            };

            img.src = objectUrl;
        }

        function openModal() {
            selectedFile = null;
            fileInput.value = '';
            modal.className += modal.className.indexOf('is-open') === -1 ? ' is-open' : '';
            modal.setAttribute('aria-hidden', 'false');
            setStatus('', '');
            setCurrentState(!!originalUrl, false);
            applyStage(originalUrl);
        }

        function closeModal() {
            modal.className = modal.className.replace(/\s?is-open/g, '');
            modal.setAttribute('aria-hidden', 'true');
            dropzone.className = dropzone.className.replace(/\s?is-dragover/g, '');
            clearLocalPreview();
            selectedFile = null;
            applyStage(originalUrl);
        }

        function pickFile(file) {
            if (!file) {
                return;
            }

            validateAndPreview(file);
        }

        trigger.onclick = function () {
            if (trigger.getAttribute('aria-disabled') === 'true') {
                return false;
            }
            openModal();
            return false;
        };

        if (closeButton) {
            closeButton.onclick = function () {
                closeModal();
                return false;
            };
        }

        if (cancelButton) {
            cancelButton.onclick = function () {
                closeModal();
                return false;
            };
        }

        modal.onclick = function (ev) {
            ev = ev || window.event;
            if (ev.target === modal) {
                closeModal();
            }
        };

        if (document.addEventListener) {
            document.addEventListener('keydown', function (ev) {
                ev = ev || window.event;
                if ((ev.key === 'Escape' || ev.keyCode === 27) && modal.className.indexOf('is-open') > -1) {
                    closeModal();
                }
            }, false);
        }

        fileInput.onchange = function () {
            if (fileInput.files && fileInput.files.length) {
                pickFile(fileInput.files[0]);
            }
        };

        if (modal.addEventListener) {
            modal.addEventListener('paste', function (ev) {
                var items = ev.clipboardData && ev.clipboardData.items ? ev.clipboardData.items : null;
                var i;
                if (!items) {
                    return;
                }

                for (i = 0; i < items.length; i++) {
                    if (items[i].kind === 'file' && items[i].type.indexOf('image/') === 0) {
                        pickFile(items[i].getAsFile());
                        if (ev.preventDefault) {
                            ev.preventDefault();
                        }
                        return;
                    }
                }
            }, false);
        }

        function stopEvent(ev) {
            if (ev.preventDefault) {
                ev.preventDefault();
            }
            if (ev.stopPropagation) {
                ev.stopPropagation();
            }
        }

        dropzone.ondragenter = dropzone.ondragover = function (ev) {
            stopEvent(ev);
            if (dropzone.className.indexOf('is-dragover') === -1) {
                dropzone.className += ' is-dragover';
            }
            return false;
        };

        dropzone.ondragleave = function (ev) {
            stopEvent(ev);
            dropzone.className = dropzone.className.replace(/\s?is-dragover/g, '');
            return false;
        };

        dropzone.ondrop = function (ev) {
            stopEvent(ev);
            dropzone.className = dropzone.className.replace(/\s?is-dragover/g, '');
            var files = ev.dataTransfer ? ev.dataTransfer.files : null;
            if (files && files.length) {
                pickFile(files[0]);
            }
            return false;
        };

        uploadButton.onclick = function () {
            return uploadSelectedBanner();
        };

        originalUrl = normalizeBannerUrl(originalUrl);
        if (hiddenBannerUrl) {
            hiddenBannerUrl.value = originalUrl;
        }
        updateBannerLink(originalUrl);
        applyStage(originalUrl);
        applyTarget(originalUrl);
        setCurrentState(!!originalUrl, false);
    }

    return {
        init: init,
        normalizeBannerUrl: normalizeBannerUrl
    };
})();
