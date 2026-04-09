$(function () {
            $(init);
            function init() {
                $('.computer').draggable({ opacity: 0.35, helper: 'original', grid: [2, 2],
                    containment: "#sortable", snap: true, snapTolerance: 8,
                    drag: function (event, ui) {
                        var offset = $(this).offset(); //小电脑图标在页面的位置 
                        var l = offset.left;
                        var t = offset.top;
                        var idname = $(this).attr("id");
                        var msgstr = idname + ' 左: ' + l + ' 上: ' + t;
                        $('#msg').html(msgstr);
                    }

                });
                $(".computer").css("cursor", "move");
                $(".computer").disableSelection();
                $(oldseat);
            }
        });
        //垂直翻转（镜像）
        function vturn() {
            var tmin = 888;
            var tmax = 0;
            $(".computer").each(function () {
                var offset = $(this).offset(); //小电脑图标在页面的位置 
                var t = offset.top;
                if (t < tmin)
                    tmin = t;
                if (t > tmax)
                    tmax = t;
            });
            var middle = (tmax + tmin) / 2 + 20;    //取到中间位置
            $(".computer").each(function () {
                var offset = $(this).offset(); //小电脑图标在页面的位置 
                var cl = offset.left;
                var ct = offset.top;
                if (ct < middle) {
                    var midd = middle - ct; //在中间线以内
                    ct = middle + midd - 40;
                }
                else {
                    var mid = ct - middle; //在中间线以外
                    ct = middle - mid - 40; //有点晕晕
                }
                $(this).offset({
                    "top": ct,
                    "left": cl
                });
            });
        }
        //水平翻转（镜像）
        function hturn() {
            var lmin = 888;
            var lmax = 0;
            $(".computer").each(function () {
                var offset = $(this).offset(); //小电脑图标在页面的位置 
                var l = offset.left;
                if (l < lmin)
                    lmin = l;
                if (l > lmax)
                    lmax = l;
            });
            var middle = (lmax + lmin) / 2 + 30;    //取到中间位置
            $(".computer").each(function () {
                var offset = $(this).offset(); //小电脑图标在页面的位置 
                var cl = offset.left;
                var ct = offset.top;
                if (cl < middle) {
                    var midd = middle - cl; //在中间线以内
                    cl = middle + midd - 60;
                }
                else {
                    var mid = cl - middle; //在中间线以外
                    cl = middle - mid - 60; //有点晕晕
                }
                $(this).offset({
                    "top": ct,
                    "left": cl
                });
            });
        }
        function calu() {
            var str = "";
            var roomoffset = $('#sortable').offset();
            var rl = roomoffset.left;
            var rt = roomoffset.top;
            $(".computer").each(function () {
                var offset = $(this).offset(); //小电脑图标在页面的位置 
                var xl = offset.left;
                var xt = offset.top;
                var xidname = $(this).attr("id");
                switch (xidname.length) {
                    case 1:
                        xidname = ".." + xidname;
                        break;
                    case 2:
                        xidname = "." + xidname;
                        break;
                }
                str = str + xidname + "：左:" + (xl - rl).toString() + "&nbsp;上:" + (xt - rt).toString() + "<br />";
            });
            $('#showMessage').html(str);
            $("#showMessage").dialog();
        }
        function save() {
            var cookstr = "";
            $(".computer").each(function () {
                var offset = $(this).offset(); //小电脑图标在页面的位置 
                var xl = offset.left;
                var xt = offset.top;
                var xidname = $(this).attr("id");
                cookstr = cookstr + xidname + ":" + xl + "," + xt + "|";
            });
            var selecthnum = $('#ddll').val(); //设定的列数
            var seatnum = $('#TextBoxall').attr("value"); //设定的电脑数
            var sortway = $("input[name='RadioBtnSelect']:checked").val();

            var cookcollect = 'ls_' + selecthnum + "_" + seatnum + "_" + sortway;
            var roomcollect = cookcollect + 'room';
            $.cookie(cookcollect, cookstr); //将所有电脑的偏移量保存

            var roomoffset = $('#sortable').offset();
            var rl = roomoffset.left;
            $.cookie(roomcollect, rl); //将房间的偏移量保存
            $('#msg').html("将当前布置临时保存!");
        }

        function uploadseat() {
            var cookstr = "";
            var hid = window.__computerConfig.getHid;
            $(".computer").each(function () {
                var offset = $(this).offset(); //小电脑图标在页面的位置 
                var xl = offset.left;
                var xt = offset.top;
                var xidname = $(this).attr("id");
                cookstr = cookstr + xidname + ":" + xl + "," + xt + "|";
            });
            var selecthnum = $('#ddll').val(); //设定的列数
            var seatnum = $('#TextBoxall').attr("value"); //设定的电脑数
            var sortway = $("input[name='RadioBtnSelect']:checked").val();

            var roomoffset = $('#sortable').offset();
            var rl = roomoffset.left;

            var collects = selecthnum + "-" + seatnum + "-" + sortway + "-" + rl + "-" + cookstr;
            //设定的列数－电脑数－纵横方向－房间偏移量－电脑位置
            $(SaveSeats(hid, collects)); //异步保存到数据库
        }
        function oldseat() {
            var done = window.__computerConfig.this_firstshow;
            if (done.length > 10) {
                var old_collects = done.split('-');
                var slnum = old_collects[0];
                var sallnum = old_collects[1];
                var ssortway = old_collects[2];
                var srl = old_collects[3];
                var scook = old_collects[4];
                $('#ddll').val(slnum);
                $('#TextBoxall').attr("value", sallnum);
                $("input[name='RadioBtnSelect']:checked").val(ssortway);
                oldshow(srl, scook);
            }
        }
        function oldshow(roomset, cookset) {
            var croomoffset = $('#sortable').offset();
            var crl = croomoffset.left;

            if (cookset != null) {
                var cookslist = cookset.split('|');
                var cookscount = cookslist.length - 1;
                var roomfix = 0;
                if (roomset != null) {
                    roomfix = roomset - crl;
                }
                for (i = 0; i < cookscount; i++) {
                    var cook = cookslist[i].split(':');
                    var cookname = cook[0];
                    var cookoffset = cook[1].split(',');
                    var cookleft = cookoffset[0] - roomfix; // js加运算不对，只能采用减运算
                    var cooktop = cookoffset[1];
                    restore(cookname, cookleft, cooktop);
                }
                $('#msg').html("显示数据库保存布置!");
            }
        }
        function reshow() {
            var selecthnum = $('#ddll').val(); //设定的列数
            var seatnum = $('#TextBoxall').attr("value"); //设定的电脑数
            var sortway = $("input[name='RadioBtnSelect']:checked").val();

            var cookcollect = 'ls_' + selecthnum + "_" + seatnum + "_" + sortway;
            var roomcollect = cookcollect + 'room';

            var cookset = $.cookie(cookcollect);

            var croomoffset = $('#sortable').offset();
            var crl = croomoffset.left;
            var roomset = $.cookie(roomcollect);

            if (cookset != null) {
                var cookslist = cookset.split('|');
                var cookscount = cookslist.length - 1;
                var roomfix = 0;
                if (roomset != null) {
                    roomfix = roomset - crl;
                }
                for (i = 0; i < cookscount; i++) {
                    var cook = cookslist[i].split(':');
                    var cookname = cook[0];
                    var cookoffset = cook[1].split(',');
                    var cookleft = cookoffset[0] - roomfix; // js加运算不对，只能采用减运算
                    var cooktop = cookoffset[1];
                    restore(cookname, cookleft, cooktop);
                }
                $('#msg').html("恢复到上次临时布置!");
            }
        }
        function restore(cn, cl, ct) {
            $(".computer").each(function () {
                var id = $(this).attr("id");
                if (id === cn) {
                    $(this).offset({
                        "top": ct,
                        "left": cl
                    });
                }
            });
        }

        function lefttoleft() {
            $(".computer").each(function () {
                var coffset = $(this).offset();
                var cl = coffset.left;
                var ct = coffset.top;
                cl = cl - 32;
                $(this).offset({
                    "top": ct,
                    "left": cl
                });

            });
        }
        function lefttoright() {
            $(".computer").each(function () {
                var coffset = $(this).offset();
                var cl = coffset.left;
                var ct = coffset.top;
                cl = parseInt(cl) + 32;
                $(this).offset({
                    "top": ct,
                    "left": cl
                });

            });
        }
        function toptotop() {
            $(".computer").each(function () {
                var coffset = $(this).offset();
                var cl = coffset.left;
                var ct = coffset.top;
                ct = parseInt(ct) - 32;
                $(this).offset({
                    "top": ct,
                    "left": cl
                });

            });
        }
        function toptodown() {
            $(".computer").each(function () {
                var coffset = $(this).offset();
                var cl = coffset.left;
                var ct = coffset.top;
                ct = parseInt(ct) + 32;
                $(this).offset({
                    "top": ct,
                    "left": cl
                });

            });
        }
