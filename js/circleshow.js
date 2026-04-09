function myrefresh() {
            var stxt = document.getElementById(window.__circleshowConfig.btnstopId).value;
            if (stxt == "暂停") {
                document.getElementById(window.__circleshowConfig.imgBtnId).click();
            }
        }
        setTimeout("myrefresh()", 8000);

        $("#showname").click(function () {
            $("#stuname").slideToggle();
        });
