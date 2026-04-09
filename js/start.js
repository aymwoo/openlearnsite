/* ================================================================
   start.js — 从 teacher/start.aspx 提取的页面脚本
   依赖: jQuery 1.8+, spanToolTip.js
   服务端配置通过 window.__startConfig 注入
   ================================================================ */
(function () {
    var cfg = window.__startConfig || {};
    var btnRefreshId = cfg.btnRefreshId || '';
    var lsGrade = cfg.lsGrade || '0';
    var lsClass = cfg.lsClass || '0';
    var lsCid = cfg.lsCid || '0';

    /* ================================================================
       自动刷新
       ================================================================ */
    function myrefresh() {
        var el = document.getElementById(btnRefreshId);
        if (el) el.click();
    }
    window.myrefresh = myrefresh;
    setTimeout(myrefresh, 120000);

    /* ================================================================
       弹窗辅助
       ================================================================ */
    function notsg(n, g, m) {
        var urlsg = "../teacher/notsign.aspx?nnum=" + n + "&ngrade=" + g + "&qname=" + m;
        openLessonModal(urlsg, "未签到备注", 300);
    }
    function attitude(q, m, a, c) {
        var urlat = "../teacher/attitude.aspx?qid=" + q + "&qname=" + m + "&qattitude=" + a + "&qcid=" + c;
        openLessonModal(urlat, "学习表现评价", 360);
    }
    function attitudegroup(g, m, q, c) {
        var urlat = "../teacher/attitudegroup.aspx?sg=" + g + "&ld=" + m + "&qd=" + q + "&qcid=" + c;
        openLessonModal(urlat, "小组学习表现评价", 240);
    }
    window.notsg = notsg;
    window.attitude = attitude;
    window.attitudegroup = attitudegroup;

    /* ================================================================
       Tab 切换
       ================================================================ */
    var lsCurrentTab = "online";
    function lsSwitchTab(tabName) {
        lsCurrentTab = tabName;
        var tabs = document.querySelectorAll("#ls-tab-bar .ls-tab");
        for (var i = 0; i < tabs.length; i++) {
            var t = tabs[i];
            if (t.getAttribute("data-tab") === tabName) {
                t.className = "ls-tab ls-tab--active";
            } else {
                t.className = "ls-tab";
            }
        }
        var panels = ["online", "progress", "realtime"];
        for (var j = 0; j < panels.length; j++) {
            var p = document.getElementById("ls-panel-" + panels[j]);
            if (p) p.style.display = (panels[j] === tabName) ? "block" : "none";
        }
        // 切换到实时动态时启动轮询
        var indicator = document.getElementById("ls-realtime-indicator");
        if (tabName === "realtime") {
            if (indicator) indicator.style.display = "inline-flex";
            lsStartPolling();
        } else {
            if (indicator) indicator.style.display = "none";
            lsStopPolling();
        }
    }
    window.lsSwitchTab = lsSwitchTab;

    /* ================================================================
       实时动态 AJAX 轮询
       ================================================================ */
    var lsPollingTimer = null;
    var lsPollingInterval = 8000; // 8秒轮询

    function lsStartPolling() {
        lsFetchData(); // 立即获取一次
        if (lsPollingTimer) clearInterval(lsPollingTimer);
        lsPollingTimer = setInterval(lsFetchData, lsPollingInterval);
    }

    function lsStopPolling() {
        if (lsPollingTimer) {
            clearInterval(lsPollingTimer);
            lsPollingTimer = null;
        }
    }

    var statusLabels = {
        "working": "学习中",
        "viewing": "浏览中",
        "submitted": "已提交",
        "idle": "空闲"
    };

    var stepTypeLabels = {
        "1": "活动",
        "2": "调查",
        "3": "讨论",
        "4": "表单",
        "5": "Scratch",
        "6": "资源",
        "7": "说明",
        "8": "Python",
        "9": "测评",
        "10": "流程图",
        "11": "像素画",
        "12": "网页",
        "13": "拼图",
        "14": "积木"
    };

    function lsDecodeMaybe(val) {
        if (!val) return "";
        try {
            if (/%[0-9A-Fa-f]{2}/.test(val)) {
                return decodeURIComponent(val);
            }
        } catch (e) { }
        return val;
    }

    function lsFetchData() {
        if (lsGrade === "0" && lsClass === "0") return;
        $.ajax({
            url: "../teacher/learnprogress.ashx",
            type: "GET",
            data: { action: "all", sgrade: lsGrade, sclass: lsClass, cid: lsCid },
            dataType: "json",
            timeout: 6000,
            success: function (resp) {
                if (resp && resp.ok) {
                    lsRenderProgress(resp.progress);
                    lsRenderStudents(resp.students);
                } else {
                    var container = document.getElementById("ls-rt-students");
                    if (container) {
                        container.innerHTML = '<span style="color:#ef4444;font-size:13px;">实时动态加载失败</span>';
                    }
                }
            },
            error: function () {
                var container = document.getElementById("ls-rt-students");
                if (container) {
                    container.innerHTML = '<span style="color:#ef4444;font-size:13px;">实时动态加载失败</span>';
                }
            }
        });
    }

    function lsRenderProgress(prog) {
        if (!prog) return;
        var el = function (id) { return document.getElementById(id); };
        el("ls-rt-working").innerText = prog.working || 0;
        el("ls-rt-viewing").innerText = prog.viewing || 0;
        el("ls-rt-submitted").innerText = prog.submitted || 0;
        el("ls-rt-idle").innerText = prog.idle || 0;
        el("ls-rt-total").innerText = prog.total || 0;

        // 环节分布
        var chart = el("ls-rt-steps-chart");
        if (chart && prog.steps) {
            var html = "";
            var maxCount = 0;
            for (var k in prog.steps) {
                if (prog.steps[k] > maxCount) maxCount = prog.steps[k];
            }
            if (maxCount === 0) maxCount = 1;
            for (var stepName in prog.steps) {
                var cnt = prog.steps[stepName];
                var pct = Math.round((cnt / maxCount) * 100);
                html += '<div class="ls-rt-step-bar">' +
                    '<span class="ls-rt-step-name" title="' + stepName + '">' + stepName + '</span>' +
                    '<div class="ls-rt-step-progress"><div class="ls-rt-step-fill" style="width:' + pct + '%"></div></div>' +
                    '<span class="ls-rt-step-count">' + cnt + '人</span>' +
                    '</div>';
            }
            if (html === "") {
                html = '<span style="color:#94a3b8;font-size:13px;">暂无学生在线</span>';
            }
            chart.innerHTML = html;
        }
    }

    function lsRenderStudents(students) {
        var container = document.getElementById("ls-rt-students");
        if (!container || !students) return;
        if (students.length === 0) {
            container.innerHTML = '<span style="color:#94a3b8;font-size:13px;">暂无学生状态数据</span>';
            return;
        }
        var html = "";
        for (var i = 0; i < students.length; i++) {
            var s = students[i];
            var st = s.Status || "idle";
            var label = statusLabels[st] || st;
            var sname = lsDecodeMaybe(s.Sname || "");
            var ltitle = lsDecodeMaybe(s.Ltitle || "");
            var ltype = stepTypeLabels[s.Ltype] || (s.Ltype ? ("类型" + s.Ltype) : "未知类型");
            var meta = "学案#" + (s.Cid || 0) + " · " + ltype;
            var aiBadge = '';
            if (s.HasAssessment) {
                var badgeClass = s.AssessmentFallback ? 'ls-rt-stu__badge ls-rt-stu__badge--fallback' : 'ls-rt-stu__badge';
                var badgeText = s.AssessmentFallback ? '模板评估' : 'AI评估';
                aiBadge = '<span class="' + badgeClass + '" title="最近评估：' + (s.AssessmentTime || '-') + '">' + badgeText + '</span>';
            }
            html += '<div class="ls-rt-stu ls-rt-stu--' + st + '" onclick="lsOpenStudentDetail(' + (s.Sid || 0) + ',' + (s.Lid || 0) + ')">' +
                '<span class="ls-rt-stu__name">' + sname + '</span>' +
                '<span class="ls-rt-stu__step" title="' + ltitle + '">' + (ltitle || "-") + '</span>' +
                '<span class="ls-rt-stu__meta" title="' + meta + '">' + meta + '</span>' +
                '<span class="ls-rt-stu__status ls-rt-stu__status--' + st + '">' + label + '</span>' +
                aiBadge +
                '<span class="ls-rt-stu__time">' + (s.UpdateTime || "") + '</span>' +
                '</div>';
        }
        container.innerHTML = html;
    }

    /* ================================================================
       学生详情模态框
       ================================================================ */
    function lsCloseStudentModal() {
        var modal = document.getElementById('lsStudentModal');
        if (!modal) return;
        modal.className = modal.className.replace(/\s?is-open/g, '');
        modal.setAttribute('aria-hidden', 'true');
    }
    window.lsCloseStudentModal = lsCloseStudentModal;

    function lsEscapeHtml(text) {
        if (text === null || text === undefined) return '';
        return String(text)
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/"/g, '&quot;')
            .replace(/'/g, '&#39;');
    }

    function lsRenderAnswerLog(answerLog) {
        if (!answerLog) {
            return '<div class="ls-rt-modal__content">暂无答题记录。</div>';
        }
        try {
            var parsed = JSON.parse(answerLog);
            if (!parsed || !parsed.answers || !parsed.answers.length) {
                return '<div class="ls-rt-modal__content">暂无答题记录。</div>';
            }
            var titleMap = window.lsStudentQuestionTitles || {};
            var optionMap = window.lsStudentOptionTexts || {};
            var blankMap = window.lsStudentBlankAnswers || {};
            var summary = parsed.summary || {};
            var totalQuestions = summary.totalQuestions || parsed.total || parsed.answers.length || 0;
            var earnedScore = summary.earnedScore || parsed.score || 0;
            var html = '<div class="ls-rt-modal__content">本次记录共 ' + totalQuestions + ' 题，得分 ' + earnedScore + '。</div>';
            html += '<div style="margin-top:12px;display:grid;gap:10px;">';
            for (var i = 0; i < parsed.answers.length; i++) {
                var item = parsed.answers[i];
                var isCorrect = item.isCorrect === true || item.isWrong === false;
                var state = isCorrect ? '正确' : '错误';
                var stateColor = isCorrect ? '#047857' : '#b91c1c';
                var qid = item.questionId || '';
                if (!qid && item.name && item.name.indexOf('-') > -1) {
                    qid = item.name.split('-')[1] || '';
                }
                var displayTitle = item.questionTitle || titleMap[qid] || item.name || ('第' + (i + 1) + '题');
                var answerValue = lsFormatStudentAnswer(item, optionMap);
                var answerMeta = '';
                if (item.name && item.name.indexOf('填空-') === 0) {
                    var blankMid = item.name.split('-')[2] || '';
                    if (blankMap[blankMid]) {
                        answerMeta = '<div style="margin-top:6px;color:#64748b;font-size:12px;">标准答案：' + lsEscapeHtml(blankMap[blankMid]) + '</div>';
                    }
                }
                html += '<div style="border:1px solid #e2e8f0;border-radius:12px;padding:12px;background:#fff;">'
                    + '<div style="display:flex;justify-content:space-between;gap:12px;align-items:center;">'
                    + '<strong style="color:#0f172a;">' + lsEscapeHtml(displayTitle) + '</strong>'
                    + '<span style="font-size:12px;font-weight:700;color:' + stateColor + ';">' + state + '</span>'
                    + '</div>'
                    + '<div style="margin-top:6px;color:#475569;font-size:13px;">学生答案：' + lsEscapeHtml(answerValue) + '</div>'
                    + answerMeta
                    + '</div>';
            }
            html += '</div>';
            return html;
        } catch (e) {
            return '<div class="ls-rt-modal__content">' + lsEscapeHtml(answerLog).replace(/\n/g, '<br>') + '</div>';
        }
    }

    function lsFormatStudentAnswer(item, optionMap) {
        if (!item) return '-';
        var answerValue = item.userAnswer;
        var questionType = item.questionType || '';

        if (answerValue === null || answerValue === undefined || answerValue === '') {
            return '-';
        }

        if (questionType === 'single_choice') {
            if (optionMap[String(answerValue)]) {
                return optionMap[String(answerValue)] + '（选项索引:' + answerValue + '）';
            }
            return String(answerValue);
        }

        if (questionType === 'multiple_choice') {
            if (!Array.isArray(answerValue) || answerValue.length === 0) return '-';
            return answerValue.map(function (value) {
                return optionMap[String(value)] ? optionMap[String(value)] + '（选项索引:' + value + '）' : String(value);
            }).join('；');
        }

        if (questionType === 'true_false') {
            return answerValue ? '正确' : '错误';
        }

        if (questionType === 'fill_blank') {
            return Array.isArray(answerValue) && answerValue.length ? answerValue.join('；') : '-';
        }

        if (questionType === 'matching' || questionType === 'table_question') {
            try {
                return JSON.stringify(answerValue);
            } catch (e) {
                return '-';
            }
        }

        if (questionType === 'sort_question') {
            return Array.isArray(answerValue) && answerValue.length ? answerValue.join(' -> ') : '-';
        }

        if (typeof answerValue === 'object') {
            try {
                return JSON.stringify(answerValue);
            } catch (e) {
                return '-';
            }
        }

        return String(answerValue);
    }

    function lsOpenStudentDetail(sid, lid) {
        if (!sid) return;
        var modal = document.getElementById('lsStudentModal');
        var body = document.getElementById('lsStudentModalBody');
        if (!modal || !body) return;
        body.innerHTML = '正在加载学生学习详情...';
        if (modal.className.indexOf('is-open') < 0) modal.className += ' is-open';
        modal.setAttribute('aria-hidden', 'false');

        $.ajax({
            url: '../teacher/learnprogress.ashx',
            type: 'GET',
            dataType: 'json',
            data: { action: 'studentdetail', sid: sid, cid: lsCid, lid: lid || 0, sgrade: lsGrade, sclass: lsClass },
            timeout: 6000,
            success: function (resp) {
                if (!resp || !resp.ok || !resp.data) {
                    body.innerHTML = '未获取到学生详情。';
                    return;
                }
                var data = resp.data;
                window.lsStudentQuestionTitles = data.questionTitles || {};
                window.lsStudentOptionTexts = data.optionTexts || {};
                window.lsStudentBlankAnswers = data.blankAnswers || {};
                var assessment = data.assessment;
                if (!assessment) {
                    body.innerHTML = '<div class="ls-rt-modal__section"><span class="ls-rt-modal__label">学生信息</span><div class="ls-rt-modal__content">' + (data.sname || '') + '（' + (data.snum || '') + '）</div></div>' +
                        '<div class="ls-rt-modal__section"><span class="ls-rt-modal__label">AI 测验评估</span><div class="ls-rt-modal__content">当前学案下暂无该学生的 AI 测验评估记录。</div></div>';
                    return;
                }
                var isRuleAssessment = (assessment.providerName || '') === '规则评估';
                var modeChipStyle = isRuleAssessment
                    ? 'background:#f1f5f9;color:#475569;border:1px solid #cbd5e1;'
                    : 'background:#dbeafe;color:#1d4ed8;border:1px solid #93c5fd;';
                var chips = '<span class="ls-rt-chip" style="' + modeChipStyle + '">评估模式：' + (isRuleAssessment ? '规则评估模式' : 'AI 评估已启用') + '</span>' +
                    '<span class="ls-rt-chip">Provider：' + (assessment.providerName || '-') + '</span>' +
                    '<span class="ls-rt-chip">Skill：' + (assessment.skillName || '-') + '</span>' +
                    '<span class="ls-rt-chip">得分：' + (assessment.score || 0) + ' / ' + (assessment.questionCount || 0) + '</span>' +
                    '<span class="ls-rt-chip">生成时间：' + (assessment.createdAt || '-') + '</span>';
                if (assessment.isFallback) {
                    chips += '<span class="ls-rt-chip">默认模板兜底</span>';
                }
                var answerLogHtml = lsRenderAnswerLog(assessment.answerLog || '');
                body.innerHTML = '<div class="ls-rt-modal__section"><span class="ls-rt-modal__label">学生信息</span><div class="ls-rt-modal__content">' + (data.sname || '') + '（' + (data.snum || '') + '）</div></div>' +
                    '<div class="ls-rt-modal__section"><span class="ls-rt-modal__label">AI 测验评估概览</span><div class="ls-rt-modal__content">' + chips + '</div><div class="ls-rt-modal__content" style="margin-top:12px;">' + (assessment.summary || '') + '</div></div>' +
                    '<div class="ls-rt-modal__section"><span class="ls-rt-modal__label">AI 分析与建议</span><div class="ls-rt-modal__content">' + (assessment.assessmentContent || '').replace(/\n/g, '<br>') + '</div></div>' +
                    '<div class="ls-rt-modal__section"><span class="ls-rt-modal__label">学习日志</span><div class="ls-rt-modal__content">' + (assessment.learningLog || '').replace(/\n/g, '<br>') + '</div></div>' +
                    '<div class="ls-rt-modal__section"><span class="ls-rt-modal__label">答题记录</span>' + answerLogHtml + '</div>';
            },
            error: function () {
                body.innerHTML = '加载学生详情失败，请稍后重试。';
            }
        });
    }
    window.lsOpenStudentDetail = lsOpenStudentDetail;
})();
