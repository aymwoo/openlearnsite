var isup = false;
                                        var mid = window.__showtaskConfig.labelMid_Text;
                                        var lid = window.__showtaskConfig.labelLid_Text;
                                        var num = window.__showtaskConfig.labelSnum_Text;
                                        var urlstr = "uploadworkm.aspx?lid=" + lid;
                                        var uploader = new plupload.Uploader({
                                            runtimes: 'html5,html4',
                                            browse_button: 'pickfiles',
                                            container: document.getElementById('container'),
                                            url: urlstr,
                                            multi_selection: false,
                                            filters: {
                                                max_file_size: '100mb',
                                                mime_types: [
                                                    { title: "work files", extensions: window.__showtaskConfig.labelUploadType_Text }
                                                ]
                                            },
                                            init: {
                                                PostInit: function () {
                                                    document.getElementById('filelist').innerHTML = '';
                                                },
                                                FilesAdded: function (up, files) {
                                                    plupload.each(files, function (file) {
                                                        document.getElementById('filelist').innerHTML += '<div id="' + file.id + '">' + file.name + ' (' + plupload.formatSize(file.size) + ') <b></b></div>';
                                                    });
                                                    uploader.start();
                                                },
                                                UploadProgress: function (up, file) {
                                                    document.getElementById(file.id).getElementsByTagName('b')[0].innerHTML = '<span>' + file.percent + "%</span>";
                                                    if (file.percent == 100 && !isup) {
                                                        isup = true;
                                                        OfficeToPng();
                                                        if (window.LearnStatus && typeof window.LearnStatus.submitted === "function") {
                                                            window.LearnStatus.submitted();
                                                        }
                                                        alert("作品已经提交成功！");
                                                    }
                                                },
                                                UploadComplete: function (up, file) {
                                                    location.reload();
                                                },
                                                Error: function (up, err) {
                                                    document.getElementById('console').appendChild(document.createTextNode("\nError #" + err.code + ": " + err.message));
                                                }
                                            }
                                        });
                                        uploader.init();

                                        function OfficeToPng() {
                                            console.log("文档转图片调用开始");
                                            var formData = new FormData();
                                            formData.append('mid', mid);
                                            formData.append('num', num);
                                            var saveurl = "spire.ashx";
                                            $.ajax({
                                                url: saveurl,
                                                type: "POST",
                                                cache: false,
                                                data: formData,
                                                dataType: "html",
                                                processData: false,
                                                contentType: false
                                            }).done(function (res) {
                                                console.log(res);
                                            }).fail(function (res) {
                                                console.log("保存失败");
                                            });
                                        }

function jsCopy(contentid) {
        var e = document.getElementById(contentid);
        e.select();
        document.execCommand("Copy");
    }
