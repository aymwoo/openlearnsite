function myrefresh() {
                var stxt = document.getElementById(window.__softnomicConfig.btnstopId).value;
                if (stxt == "暂停") {
                    document.getElementById(window.__softnomicConfig.imgBtnId).click();
                }
            }
            setTimeout("myrefresh()", 8000);
