var gaugeItemEventSource = null;
    var gaugeItemStreamFinished = false;

    function setGaugeItemProgressStep(index) {
        var steps = document.querySelectorAll('#gaugeItemLoadingSteps .gauge-ai-loading__step');
        for (var i = 0; i < steps.length; i++) {
            steps[i].className = 'gauge-ai-loading__step';
            if (i < index) {
                steps[i].className += ' is-done';
            }
            else if (i === index) {
                steps[i].className += ' is-active';
            }
        }
    }

    function setGaugeItemProgressMessage(message) {
        var desc = document.getElementById('gaugeItemLoadingDesc');
        if (desc && message) {
            desc.innerHTML = message;
        }
    }

    function openGaugeItemLoading(mode) {
        var loading = document.getElementById('gaugeItemLoading');
        var button = document.getElementById('BtnRegenerateAI');
        var appendButton = document.getElementById('BtnAppendAI');
        if (button) {
            button.disabled = true;
            button.value = '正在重新生成...';
        }
        if (appendButton) {
            appendButton.disabled = true;
            appendButton.value = mode === 'append' ? '正在追加生成...' : '追加AI生成';
        }
        if (loading && loading.className.indexOf('is-active') < 0) {
            loading.className += ' is-active';
        }
    }

    function resetGaugeItemButton() {
        var button = document.getElementById('BtnRegenerateAI');
        var appendButton = document.getElementById('BtnAppendAI');
        if (button) {
            button.disabled = false;
            button.value = '重新用AI生成一次';
        }
        if (appendButton) {
            appendButton.disabled = false;
            appendButton.value = '追加AI生成';
        }
    }

    function closeGaugeItemEventSource() {
        if (gaugeItemEventSource) {
            gaugeItemEventSource.close();
            gaugeItemEventSource = null;
        }
    }

    function parseGaugeItemSseData(data) {
        try {
            return JSON.parse(data);
        }
        catch (e) {
            return null;
        }
    }

    function startGaugeRegenerate(mode) {
        var confirmText = mode === 'append'
            ? '追加生成会保留现有量规项，并在后面新增 AI 生成内容，是否继续？'
            : '重新生成会清空当前已有量规项，并使用 AI 重新写入，是否继续？';
        if (!confirm(confirmText)) {
            return false;
        }

        if (!window.EventSource) {
            alert('当前浏览器不支持实时进度，请更换浏览器后再试。');
            return false;
        }

        var requestUrl = window.__gaugeitemConfig.gauge_generateUrl + '?mode=regen&gid=' + window.__gaugeitemConfig.request_QueryString_gid + '&regenBehavior=' + encodeURIComponent(mode || 'replace');
        openGaugeItemLoading(mode || 'replace');
        gaugeItemStreamFinished = false;
        setGaugeItemProgressStep(0);
        setGaugeItemProgressMessage(mode === 'append' ? '系统正在读取当前量规并准备追加生成。' : '系统正在读取当前量规并准备重新生成。');
        closeGaugeItemEventSource();

        gaugeItemEventSource = new EventSource(requestUrl);
        gaugeItemEventSource.addEventListener('progress', function (event) {
            var payload = parseGaugeItemSseData(event.data);
            if (!payload) {
                return;
            }
            var step = parseInt(payload.step, 10);
            if (!isNaN(step)) {
                setGaugeItemProgressStep(Math.max(0, Math.min(2, step - 1)));
            }
            setGaugeItemProgressMessage(payload.message || '系统正在处理，请稍候。');
        });

        gaugeItemEventSource.addEventListener('done', function (event) {
            var payload = parseGaugeItemSseData(event.data);
            gaugeItemStreamFinished = true;
            closeGaugeItemEventSource();
            setGaugeItemProgressStep(2);
            setGaugeItemProgressMessage(payload && payload.message ? payload.message : '量规项已重新生成，正在刷新页面。');
            window.setTimeout(function () {
                if (payload && payload.redirectUrl) {
                    window.location.href = payload.redirectUrl;
                }
                else {
                    window.location.reload();
                }
            }, 350);
        });

        gaugeItemEventSource.addEventListener('failed', function (event) {
            var payload = event && event.data ? parseGaugeItemSseData(event.data) : null;
            gaugeItemStreamFinished = true;
            closeGaugeItemEventSource();
            resetGaugeItemButton();
            alert(payload && payload.message ? payload.message : '重新生成失败，请稍后重试。');
            window.location.reload();
        });

        gaugeItemEventSource.onerror = function () {
            if (!gaugeItemEventSource || gaugeItemStreamFinished) {
                return;
            }
            closeGaugeItemEventSource();
            resetGaugeItemButton();
            alert('重新生成连接已中断，请稍后重试。');
            window.location.reload();
        };

        return false;
    }
