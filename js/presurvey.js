function passrefresh() {
                var target = document.getElementById("ctl00_Ppcm_Lbtime");
                if (!target) return;
                var oldt = parseInt(target.innerHTML || target.textContent || "0", 10) || 0;
                target.innerHTML = oldt + 1;
        }
        setTimeout("passrefresh()", 60000); //指定60秒刷新一次
