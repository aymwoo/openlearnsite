/**
 * learnstatus.js - 学生学习状态上报模块
 * 
 * 在学生端各学案学习页面中自动上报学习状态：
 * - 页面加载时上报 "viewing" 状态
 * - 用户有交互操作时上报 "working" 状态
 * - 定期发送心跳（每30秒）
 * - 页面关闭时上报移除状态
 * 
 * 使用方式：在 Scm.master 中引入此脚本，并通过 data 属性或全局变量传递学生信息
 */
(function () {
    // 从全局变量读取学生信息（由 Scm.master 注入）
    var lsConfig = window.__learnStatus;
    if (!lsConfig || !lsConfig.snum || lsConfig.snum === "") return;

    var reportUrl = "../student/learnstatus.ashx";
    var heartbeatInterval = 30000; // 30秒心跳
    var idleTimeout = 60000;       // 60秒无操作视为idle
    var currentStatus = "viewing";
    var lastActivity = Date.now();
    var heartbeatTimer = null;
    var idleTimer = null;
    var hasKicked = false;

    function handleKick() {
        if (hasKicked) return;
        hasKicked = true;
        try {
            clearInterval(heartbeatTimer);
            clearTimeout(idleTimer);
        } catch (e) { }
        window.location.href = "../student/myinfo.aspx?action=logout&kick=1";
    }

    /**
     * 发送状态数据到服务器
     */
    function sendStatus(action, status) {
        var formData = "action=" + encodeURIComponent(action) +
            "&snum=" + encodeURIComponent(lsConfig.snum) +
            "&sname=" + encodeURIComponent(lsConfig.sname) +
            "&sgrade=" + encodeURIComponent(lsConfig.sgrade) +
            "&sclass=" + encodeURIComponent(lsConfig.sclass) +
            "&cid=" + encodeURIComponent(lsConfig.cid) +
            "&lid=" + encodeURIComponent(lsConfig.lid) +
            "&ltitle=" + encodeURIComponent(lsConfig.ltitle) +
            "&ltype=" + encodeURIComponent(lsConfig.ltype) +
            "&status=" + encodeURIComponent(status) +
            "&sid=" + encodeURIComponent(lsConfig.sid);

        try {
            var xhr = new XMLHttpRequest();
            xhr.open("POST", reportUrl, true);
            xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            xhr.timeout = 5000;
            xhr.onreadystatechange = function () {
                if (xhr.readyState === 4 && xhr.status === 200) {
                    try {
                        var data = JSON.parse(xhr.responseText || "{}");
                        if (data && data.kick) {
                            handleKick();
                        }
                    } catch (ex) { }
                }
            };
            xhr.send(formData);
        } catch (e) {
            // 静默失败，不影响学生正常使用
        }
    }

    /**
     * 更新状态
     */
    function updateStatus(newStatus) {
        if (newStatus !== currentStatus) {
            currentStatus = newStatus;
            sendStatus("update", currentStatus);
        }
    }

    /**
     * 用户活动检测
     */
    function onUserActivity() {
        lastActivity = Date.now();
        if (currentStatus === "idle") {
            updateStatus("working");
        } else if (currentStatus === "viewing") {
            updateStatus("working");
        }
        // 重置idle计时器
        clearTimeout(idleTimer);
        idleTimer = setTimeout(function () {
            updateStatus("idle");
        }, idleTimeout);
    }

    /**
     * 心跳
     */
    function heartbeat() {
        sendStatus("heartbeat", currentStatus);
    }

    /**
     * 初始化
     */
    function init() {
        // 1. 上报初始 viewing 状态
        sendStatus("update", "viewing");

        // 2. 监听用户交互事件
        var events = ["click", "keydown", "scroll", "mousemove", "touchstart", "input"];
        for (var i = 0; i < events.length; i++) {
            document.addEventListener(events[i], onUserActivity, { passive: true });
        }

        // 3. 设置心跳
        heartbeatTimer = setInterval(heartbeat, heartbeatInterval);

        // 4. 设置idle检测
        idleTimer = setTimeout(function () {
            updateStatus("idle");
        }, idleTimeout);

        // 5. 页面关闭时发送移除状态（使用 sendBeacon 保证发送）
        window.addEventListener("beforeunload", function () {
            var formData = "action=remove&snum=" + encodeURIComponent(lsConfig.snum);
            if (navigator.sendBeacon) {
                var blob = new Blob([formData], { type: "application/x-www-form-urlencoded" });
                navigator.sendBeacon(reportUrl, blob);
            } else {
                // 降级：同步 XHR
                try {
                    var xhr = new XMLHttpRequest();
                    xhr.open("POST", reportUrl, false);
                    xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
                    xhr.send(formData);
                } catch (e) { }
            }
        });
    }

    // 页面加载后初始化
    if (document.readyState === "complete" || document.readyState === "interactive") {
        init();
    } else {
        document.addEventListener("DOMContentLoaded", init);
    }

    // 暴露 API 供外部调用（如提交作品时手动上报 submitted 状态）
    window.LearnStatus = {
        setStatus: function (status) {
            updateStatus(status);
        },
        submitted: function () {
            updateStatus("submitted");
        }
    };
})();
