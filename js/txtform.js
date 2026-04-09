var snum = window.__txtformConfig.snum;
                    var sname = window.__txtformConfig.sname;
                    var sgroup = window.__txtformConfig.sgroup;
                    var isopen = true;
                    var collabo = window.__txtformConfig.collabo;
                    var serverip = window.__txtformConfig.serverIp;

                    if (collabo == "false") {
                        $("#connected").hide();
                        isopen = false;
                    }

                    $("#connected").click(function () {
                        if (isopen) {
                            isopen = false;
                            $(this).css("filter", "hue-rotate(200deg)");
                            $(this).attr("title", "不接收小组协作内容");
                        }
                        else {
                            isopen = true;
                            $(this).css("filter", "");
                            $(this).attr("title", "接收小组协作内容");
                        }
                    });

                    function tableshow() {
                        var table = $("div.coursecontent table");
                        table.css("table-layout", "auto");
                        var tds = [];
                        var idx = 0;
                        table.find("tr").each(function () {
                            $(this).find("td").each(function () {
                                $(this).attr("id", "cell" + idx);
                                idx += 1;
                                var cellContent = $(this).text();
                                cellContent = jQuery.trim(cellContent);
                                var cellCan = $(this).attr("contenteditable");
                                var cellId = $(this).attr("id");
                                if (cellCan) {
                                    tds.push(cellContent);
                                    $(this).text(cellContent);
                                }
                            });
                        });
                        return tds;
                    }

                    var oldid = "";
                    var isconnect = false;

                    var start = function () {
                        tableshow();

                        var msg = "正在连接协作服务...\n";
                        var hostip = location.host;
                        if (serverip != "") {
                            hostip = serverip;
                        }
                        console.log(hostip, msg);

                        var wsurl = "ws://" + hostip + ":8188/";
                        window.ws = new WebSocket(wsurl);

                        ws.onmessage = function (evt) {
                            var msg = JSON.parse(evt.data);
                            var idstr = "#" + msg[0];
                            var textstr = msg[1];
                            var snumstr = msg[2];
                            var namestr = msg[3];
                            var txtform = msg[4];
                            var sgroupstr = msg[5];
                            var talktimestr = msg[6];

                            if (isopen) {
                                if (txtform == "txtform") {
                                    if (sgroup == sgroupstr) {
                                        if (oldid != "") {
                                            $(oldid).attr({ contenteditable: "true" });
                                        }
                                        oldid = idstr;
                                        $(idstr).text(textstr);
                                        $(".namebox").text(namestr + "正在输入...");
                                        $(".namebox").show();

                                        var p = $(idstr).offset();
                                        p.left = p.left + textstr.length;
                                        p.top = p.top;
                                        $(".namebox").offset(p);
                                        console.log("接收小组成员", namestr, "协作信息", talktimestr);
                                        if (snum != snumstr) {
                                            $(idstr).attr({ contenteditable: "none" });
                                        }
                                    }
                                    else {
                                        $(".namebox").hide();
                                        console.log("...");
                                    }
                                }
                            }
                        };

                        ws.onopen = function () {
                            msg = '.. 已连接\n';
                            console.log(msg);
                            isconnect = true;
                            if (collabo == "true") {
                                $("#connected").show();
                            }
                        };

                        ws.onclose = function () {
                            msg = '.. 已断开\n';
                            console.log(msg);
                            isconnect = false;
                            $(".namebox").hide();
                        }

                        ws.onerror = function (e) {
                            console.log("发送失败!");
                            $("#connected").hide();
                        }

                        $("td").keyup(function (e) {
                            e.preventDefault();
                            var da = new Date;
                            var talktime = da.toLocaleString();

                            var dic = [];
                            dic.push($(this).attr("id"));
                            dic.push($(this).text());
                            dic.push(snum);
                            dic.push(sname);
                            dic.push("txtform");
                            dic.push(sgroup);
                            dic.push(talktime);

                            var dicstr = JSON.stringify(dic);
                            if (isconnect && isopen) {
                                ws.send(dicstr);
                            }
                        });

                        $("td").click(function () {});
                    }

                    if (collabo == "false") {
                        console.log("独立模式");
                    }
                    else {
                        window.addEventListener('load', start);
                        console.log("协作模式");
                    }

                    var timer = setInterval(function () {
                        if (isconnect) {
                            $(".namebox").hide();
                            if (oldid != "") {
                                $(oldid).attr({ contenteditable: "true" });
                            }
                        }
                    }, 8000);

                    function SaveForm() {
                        var saveurl = "saveform.ashx?lid=" + window.__txtformConfig.lid;
                        var contentstr = $("div.coursecontent").html();
                        var wordstr = "";
                        $(function () {
                            $("div.coursecontent table").each(function (index, element) {
                                var htmlstr = $(element).prop('outerHTML') + "<br>";
                                wordstr = wordstr + htmlstr;
                            })
                        })
                        var formData = new FormData();
                        formData.append('Word', wordstr);
                        formData.append('Content', contentstr);
                        $.ajax({
                            url: saveurl,
                            type: 'POST',
                            cache: false,
                            data: formData,
                            processData: false,
                            contentType: false
                        }).done(function (res) {
                            $("#sucessed").show();
                            if (window.LearnStatus && typeof window.LearnStatus.submitted === "function") {
                                window.LearnStatus.submitted();
                            }
                            alert("提交成功！");
                            location.reload();
                        }).fail(function (res) {
                            console.log(res)
                        });
                    }

                    var isdone = window.__txtformConfig.done;
                    if (isdone == "true") {
                        $("#sucessed").show();
                    }
                    else {
                        $("#sucessed").hide();
                    }

                    function HTMLDecode(text) {
                        var temp = document.createElement("div");
                        temp.innerHTML = text;
                        var output = temp.innerText || temp.textContent;
                        temp = null;
                        return output;
                    }
