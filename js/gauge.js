var gaugeEventSource = null;
        var gaugeStreamFinished = false;

        function setGaugeProgressStep(index) {
            var steps = document.querySelectorAll('#gaugeLoadingSteps .gauge-loading__step');
            for (var i = 0; i < steps.length; i++) {
                steps[i].className = 'gauge-loading__step';
                if (i < index) {
                    steps[i].className += ' is-done';
                }
                else if (i === index) {
                    steps[i].className += ' is-active';
                }
            }

            var desc = document.getElementById('gaugeLoadingDesc');
            if (!desc) {
                return;
            }

            if (index === 0) {
                desc.innerHTML = '系统正在创建量规基础记录，请稍候。';
            }
            else if (index === 1) {
                desc.innerHTML = '系统正在调用 AI 生成评价项，如果当前使用默认模板，也会自动继续处理。';
            }
            else {
                desc.innerHTML = '系统正在写入评价项并跳转到编辑页，请不要关闭当前页面。';
            }
        }

        function setGaugeProgressMessage(message) {
            var desc = document.getElementById('gaugeLoadingDesc');
            if (desc && message) {
                desc.innerHTML = message;
            }
        }

        function openGaugeLoading() {
            var button = document.getElementById(window.__gaugeConfig.btnaddId);
            var loading = document.getElementById('gaugeLoading');
            if (button) {
                button.disabled = true;
                button.value = '正在生成...';
            }
            if (loading && loading.className.indexOf('is-active') < 0) {
                loading.className += ' is-active';
            }
        }

        function resetGaugeButton() {
            var button = document.getElementById(window.__gaugeConfig.btnaddId);
            if (button) {
                button.disabled = false;
                button.value = '添加量规';
            }
        }

        function closeGaugeEventSource() {
            if (gaugeEventSource) {
                gaugeEventSource.close();
                gaugeEventSource = null;
            }
        }

        function parseGaugeSseData(data) {
            try {
                return JSON.parse(data);
            }
            catch (e) {
                return null;
            }
        }

        function startGaugeSseGenerate() {
            var titleInput = document.getElementById(window.__gaugeConfig.textBoxGtitleId);
            if (!titleInput || !titleInput.value || !titleInput.value.trim()) {
                return __doPostBack(window.__gaugeConfig.btnaddUniqueId, '');
            }

            var typeInput = document.getElementById(window.__gaugeConfig.dDLtypeId);
            var gaugeType = typeInput ? typeInput.value : '';
            var gaugeTitle = titleInput.value.trim();
            var requestUrl = window.__gaugeConfig.gauge_generateUrl + '?gtype=' + encodeURIComponent(gaugeType) + '&gtitle=' + encodeURIComponent(gaugeTitle);

            if (!window.EventSource) {
                return __doPostBack(window.__gaugeConfig.btnaddUniqueId, '');
            }

            openGaugeLoading();
            gaugeStreamFinished = false;
            setGaugeProgressStep(0);
            setGaugeProgressMessage('系统正在创建量规基础记录，请稍候。');
            closeGaugeEventSource();

            gaugeEventSource = new EventSource(requestUrl);
            gaugeEventSource.addEventListener('progress', function (event) {
                var payload = parseGaugeSseData(event.data);
                if (!payload) {
                    return;
                }
                var step = parseInt(payload.step, 10);
                if (!isNaN(step)) {
                    setGaugeProgressStep(Math.max(0, Math.min(2, step - 1)));
                }
                setGaugeProgressMessage(payload.message || '系统正在处理，请稍候。');
            });

            gaugeEventSource.addEventListener('done', function (event) {
                var payload = parseGaugeSseData(event.data);
                gaugeStreamFinished = true;
                closeGaugeEventSource();
                setGaugeProgressStep(2);
                setGaugeProgressMessage(payload && payload.message ? payload.message : '量规已创建，正在跳转。');
                window.setTimeout(function () {
                    if (payload && payload.redirectUrl) {
                        window.location.href = payload.redirectUrl;
                    }
                    else {
                        window.location.reload();
                    }
                }, 350);
            });

            gaugeEventSource.addEventListener('failed', function (event) {
                var payload = event && event.data ? parseGaugeSseData(event.data) : null;
                gaugeStreamFinished = true;
                closeGaugeEventSource();
                resetGaugeButton();
                alert(payload && payload.message ? payload.message : '量规生成失败，请稍后重试。');
                window.location.reload();
            });

            gaugeEventSource.onerror = function () {
                if (!gaugeEventSource || gaugeStreamFinished) {
                    return;
                }
                closeGaugeEventSource();
                resetGaugeButton();
                alert('量规生成连接已中断，请稍后重试。');
                window.location.reload();
            };

            return false;
        }
