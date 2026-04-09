function returnurl() {
            if (confirm('是否离开当前活动页面？请先保存作品。') == true) {
                window.location.href = window.__examConfig.fpage
            }
        }
