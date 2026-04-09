(function () {
    var ids = window.__courseshowIds || {};

    function initBannerModal() {
        if (!window.LearnSiteCourseBanner) {
            return;
        }
        window.LearnSiteCourseBanner.init({
            triggerId: ids.heroEditLink,
            targetId: ids.heroSection,
            hiddenBannerUrlId: ids.hiddenBannerUrl,
            hiddenCourseId: ids.hiddenCourseId
        });
    }

    function initDragSort() {
        var list = document.getElementById('MenuList');
        if (!list) { return; }

        var activeRow = null;
        var dragProxy = null;
        var placeholder = null;
        var dragOffsetY = 0;
        var startOrder = '';
        var lastClientY = 0;
        var rafId = 0;
        var moveHandler = null;
        var endHandler = null;
        var currentOrderValue = '';
        var isDirty = false;
        var isSaving = false;
        var saveButton = document.getElementById('MenuSortSaveButton');
        var statusNode = document.getElementById('MenuSortStatus');
        var hiddenOrder = document.getElementById(ids.hiddenSortOrder);
        var courseIdField = document.getElementById(ids.hiddenCourseId);
        var saveButtonDefaultText = saveButton ? (saveButton.innerHTML || '保存排序') : '保存排序';
        var statusTimer = 0;

        function getRows() {
            return Array.prototype.slice.call(list.children).filter(function (el) {
                return el.getAttribute && el.getAttribute('data-lid');
            });
        }

        function getOrder() {
            return getRows().map(function (r) { return r.getAttribute('data-lid'); });
        }

        function setStatus(message, statusClass) {
            if (!statusNode) { return; }
            if (statusTimer) { window.clearTimeout(statusTimer); statusTimer = 0; }
            statusNode.className = 'course-show-save-status' + (statusClass ? ' ' + statusClass : '');
            statusNode.innerHTML = message || '';
            if (statusClass === 'is-success' && message) {
                statusTimer = window.setTimeout(function () {
                    statusNode.className = 'course-show-save-status is-success is-fading';
                    window.setTimeout(function () {
                        if (!isDirty) { statusNode.className = 'course-show-save-status'; statusNode.innerHTML = ''; }
                    }, 280);
                }, 1600);
            }
        }

        function setSaveButtonState(saving) {
            if (!saveButton) { return; }
            saveButton.disabled = saving;
            saveButton.innerHTML = saving ? '保存中...' : saveButtonDefaultText;
        }

        function setDirtyState(dirty) {
            isDirty = dirty;
            currentOrderValue = getOrder().join(',');
            if (hiddenOrder) { hiddenOrder.value = currentOrderValue; }
            if (saveButton) { saveButton.style.display = dirty ? 'inline-flex' : 'none'; setSaveButtonState(false); }
            if (!dirty) { setStatus('', ''); } else { setStatus('顺序已调整，点击保存后生效', ''); }
        }

        function updateSortBadges() {
            getRows().forEach(function (row, i) {
                var label = row.querySelector('[id$="LabelLsort"]');
                if (label) { label.textContent = String(i + 1); }
            });
        }

        function removeProxy() {
            if (dragProxy && dragProxy.parentNode) { dragProxy.parentNode.removeChild(dragProxy); }
            dragProxy = null;
        }

        function removePlaceholder() {
            if (placeholder && placeholder.parentNode) { placeholder.parentNode.removeChild(placeholder); }
            placeholder = null;
        }

        function createProxy(row) {
            removeProxy();
            var rect = row.getBoundingClientRect();
            var clone = row.cloneNode(true);
            clone.className = clone.className.replace(/\s?dragging/g, '');
            clone.style.width = rect.width + 'px';
            clone.style.boxSizing = 'border-box';
            var wrapper = document.createElement('div');
            wrapper.className = 'course-show-drag-proxy';
            wrapper.style.width = rect.width + 'px';
            wrapper.appendChild(clone);
            document.body.appendChild(wrapper);
            dragProxy = wrapper;
        }

        function createPlaceholder() {
            removePlaceholder();
            var el = document.createElement('div');
            el.className = 'course-show-placeholder';
            placeholder = el;
        }

        function moveProxy(clientY) {
            if (!dragProxy || !activeRow) { return; }
            var rect = activeRow.getBoundingClientRect();
            dragProxy.style.transform = 'translate3d(' + rect.left + 'px,' + (clientY - dragOffsetY) + 'px,0)';
        }

        function movePlaceholder(clientY) {
            if (!placeholder || !activeRow) { return; }
            var rows = getRows().filter(function (r) { return r !== activeRow; });
            for (var i = 0; i < rows.length; i++) {
                var rect = rows[i].getBoundingClientRect();
                if (clientY < rect.top + rect.height / 2) {
                    list.insertBefore(placeholder, rows[i]);
                    return;
                }
            }
            list.appendChild(placeholder);
        }

        function applyRowMove() {
            if (!activeRow || !placeholder || !placeholder.parentNode) { return; }
            list.insertBefore(activeRow, placeholder);
            updateSortBadges();
        }

        function autoScroll(clientY) {
            var edge = 72, step = 18;
            if (clientY < edge) { window.scrollBy(0, -step); }
            else if (clientY > window.innerHeight - edge) { window.scrollBy(0, step); }
        }

        function handlePointerMove(clientY) {
            lastClientY = clientY;
            if (rafId) { return; }
            rafId = window.requestAnimationFrame(function () {
                rafId = 0;
                moveProxy(lastClientY);
                movePlaceholder(lastClientY);
                applyRowMove();
                autoScroll(lastClientY);
            });
        }

        function bindActiveEvents() {
            if (!moveHandler) {
                moveHandler = function (ev) {
                    if (!activeRow) { return; }
                    ev = ev || window.event;
                    if (ev.preventDefault && ev.touches) { ev.preventDefault(); }
                    handlePointerMove(getClientY(ev));
                };
            }
            if (!endHandler) { endHandler = function () { finishDrag(); }; }
            if (document.addEventListener) {
                document.addEventListener('mousemove', moveHandler, false);
                document.addEventListener('mouseup', endHandler, false);
                document.addEventListener('touchmove', moveHandler, false);
                document.addEventListener('touchend', endHandler, false);
                document.addEventListener('touchcancel', endHandler, false);
            }
        }

        function unbindActiveEvents() {
            if (document.removeEventListener) {
                if (moveHandler) { document.removeEventListener('mousemove', moveHandler, false); document.removeEventListener('touchmove', moveHandler, false); }
                if (endHandler) { document.removeEventListener('mouseup', endHandler, false); document.removeEventListener('touchend', endHandler, false); document.removeEventListener('touchcancel', endHandler, false); }
            }
        }

        function saveSort() {
            if (!isDirty || isSaving) { return; }
            var cid = courseIdField ? courseIdField.value : '';
            var orderValue = currentOrderValue || getOrder().join(',');
            if (!cid || !orderValue) { setStatus('保存失败，缺少排序数据', 'is-error'); return; }
            isSaving = true;
            setSaveButtonState(true);
            setStatus('正在保存排序...', '');
            if (!window.jQuery || !jQuery.ajax) {
                isSaving = false; setSaveButtonState(false); setStatus('保存失败，页面缺少 AJAX 支持', 'is-error'); return;
            }
            jQuery.ajax({
                type: 'POST', url: 'courseshow.aspx/SaveSort',
                contentType: 'application/json; charset=utf-8', dataType: 'json',
                data: JSON.stringify({ cid: cid, order: orderValue }),
                success: function (response) {
                    isSaving = false;
                    var ok = response && response.d === true;
                    setSaveButtonState(false);
                    if (ok) { setDirtyState(false); setStatus('排序已保存', 'is-success'); }
                    else { setStatus('保存失败，请重试', 'is-error'); }
                },
                error: function () { isSaving = false; setSaveButtonState(false); setStatus('保存失败，请检查网络后重试', 'is-error'); }
            });
        }

        function startDrag(row, clientY) {
            activeRow = row;
            startOrder = getOrder().join(',');
            dragOffsetY = Math.max(16, clientY - row.getBoundingClientRect().top);
            createProxy(row);
            row.className += ' dragging';
            createPlaceholder();
            list.insertBefore(placeholder, row.nextSibling);
            moveProxy(clientY);
            bindActiveEvents();
        }

        function finishDrag() {
            if (!activeRow) { return; }
            if (rafId) { window.cancelAnimationFrame(rafId); rafId = 0; }
            activeRow.className = activeRow.className.replace(/\s?dragging/g, '');
            removeProxy();
            removePlaceholder();
            unbindActiveEvents();
            var currentOrder = getOrder().join(',');
            if (startOrder && currentOrder && startOrder !== currentOrder) {
                updateSortBadges();
                setDirtyState(true);
            }
            activeRow = null;
            startOrder = '';
        }

        function getClientY(ev) {
            if (typeof ev.clientY === 'number') { return ev.clientY; }
            if (ev.touches && ev.touches.length) { return ev.touches[0].clientY; }
            if (ev.changedTouches && ev.changedTouches.length) { return ev.changedTouches[0].clientY; }
            return 0;
        }

        function bindRow(row) {
            if (!row || !row.getAttribute('data-lid')) { return; }
            var handle = row.querySelector('.course-show-drag');
            if (!handle) { return; }
            handle.onmousedown = function (ev) {
                ev = ev || window.event;
                if (ev.preventDefault) { ev.preventDefault(); }
                startDrag(row, getClientY(ev));
                return false;
            };
            handle.ontouchstart = function (ev) {
                ev = ev || window.event;
                if (ev.preventDefault) { ev.preventDefault(); }
                startDrag(row, getClientY(ev));
                return false;
            };
        }

        getRows().forEach(bindRow);
        updateSortBadges();
        currentOrderValue = getOrder().join(',');
        if (hiddenOrder) { hiddenOrder.value = currentOrderValue; }

        if (saveButton) {
            saveButton.onclick = function () { saveSort(); return false; };
        }

        if (window.addEventListener) {
            window.addEventListener('beforeunload', function (ev) {
                if (!isDirty) { return; }
                var message = '当前排序尚未保存，离开页面将丢失本次调整。';
                if (ev) { ev.returnValue = message; }
                return message;
            }, false);
        }
    }

    if (window.addEventListener) {
        window.addEventListener('load', initBannerModal, false);
        window.addEventListener('load', initDragSort, false);
    } else if (window.attachEvent) {
        window.attachEvent('onload', initBannerModal);
        window.attachEvent('onload', initDragSort);
    }
})();
