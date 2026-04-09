function toggleUpgradeList(button, targetId, remainCount) {
            var target = document.getElementById(targetId);
            if (!button || !target) {
                return false;
            }

            var isOpen = button.getAttribute('data-open') === '1';
            if (isOpen) {
                target.className = 'upgrade-collapsible__more';
                button.setAttribute('data-open', '0');
                button.innerHTML = '<span class="upgrade-collapse-btn__arrow">▶</span><span>展开全部（剩余 ' + remainCount + ' 项）</span>';
            }
            else {
                target.className = 'upgrade-collapsible__more is-open';
                button.setAttribute('data-open', '1');
                button.innerHTML = '<span class="upgrade-collapse-btn__arrow">▶</span><span>收起列表</span>';
            }
            return false;
        }

        function toggleUpgradeTips(button) {
            var target = document.getElementById('upgradeTipsMore');
            if (!button || !target) {
                return false;
            }

            var isOpen = button.getAttribute('data-open') === '1';
            if (isOpen) {
                target.className = 'upgrade-tips__more';
                button.setAttribute('data-open', '0');
                button.innerHTML = '<span class="upgrade-collapse-btn__arrow">▶</span><span>展开升级说明</span>';
            }
            else {
                target.className = 'upgrade-tips__more is-open';
                button.setAttribute('data-open', '1');
                button.innerHTML = '<span class="upgrade-collapse-btn__arrow">▶</span><span>收起升级说明</span>';
            }
            return false;
        }

        var upgradeProgressTimer = null;

        function showUpgradeProgress() {
            var mask = document.getElementById('upgradeProgressMask');
            var button = document.getElementById(window.__upgradeConfig.btnupgradeId);
            if (mask) {
                mask.className = 'upgrade-progress-mask is-open';
            }
            window.setTimeout(function () {
                if (button) {
                    button.disabled = true;
                    button.className = 'upgrade-btn-primary upgrade-btn-disabled';
                    button.value = '正在升级...';
                }
            }, 0);

            var fill = document.getElementById('upgradeProgressFill');
            var desc = document.getElementById('upgradeProgressDesc');
            var steps = document.querySelectorAll('#upgradeProgressSteps .upgrade-progress-step');
            var progressPoints = [12, 36, 68, 92];
            var progressTexts = [
                '正在检查旧版本结构并准备升级环境...',
                '正在补齐历史表字段、词库和兼容补丁...',
                '正在执行新版本迁移与初始化数据写入...',
                '正在整理结果并准备跳转，请不要关闭页面...'
            ];
            var current = 0;

            function renderStep(index) {
                if (fill) {
                    fill.style.width = progressPoints[index] + '%';
                }
                if (desc) {
                    desc.innerHTML = progressTexts[index];
                }
                for (var i = 0; i < steps.length; i++) {
                    steps[i].className = 'upgrade-progress-step';
                    if (i < index) {
                        steps[i].className += ' is-done';
                    } else if (i === index) {
                        steps[i].className += ' is-active';
                    }
                }
            }

            renderStep(0);
            if (upgradeProgressTimer) {
                window.clearInterval(upgradeProgressTimer);
            }
            upgradeProgressTimer = window.setInterval(function () {
                if (current < progressPoints.length - 1) {
                    current++;
                    renderStep(current);
                }
            }, 1400);
            return true;
        }
