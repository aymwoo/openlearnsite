var editor;
                var ty = "Topic";
                var cid = window.__topicdiscussConfig.myCid;
                var upjs = '../kindeditor/aspnet/upload_json.aspx?cid=' + cid + '&ty=' + ty;
                KindEditor.ready(function (K) {
                    editor = K.create('textarea[name="textareaWord"]', {
                        filterMode: false,
                        resizeType: 1,
                        pasteType: 1,
                        newlineTag: "br",
                        allowPreviewEmoticons: false,
                        uploadJson: upjs,
                        allowImageUpload: true,
                        items: ['formatblock', 'fontname', 'fontsize', '|', 'bold', 'italic', 'forecolor', 'hilitecolor', 'removeformat', '|', 'justifyleft', 'justifycenter', 'justifyright', 'image'],
                        afterChange: function () {
                            K('.word_count').html(this.count('text'));
                        }
                    });
                });

$(".topictext img").click(function () {
                    console.log("图片点击");
                    var _this = $(this);
                    imgShow("#outerdiv", "#innerdiv", "#bigimg", _this);
                });

                $(".topictext img").each(function (i) {
                    $(this).attr("oncontextmenu", "return false;");
                });

                function imgShow(outerdiv, innerdiv, bigimg, _this) {
                    debugger
                    var src = _this.attr("src");
                    $(bigimg).attr("src", src);
                    $("<img/>").attr("src", src).on('load', function () {
                        debugger
                        var windowW = $(window).width()
                        var windowH = $(window).height();
                        var realWidth = this.width;
                        var readHeight = this.height;
                        var imgWidth, imgHeight;
                        var scale = 0.8;
                        if (realWidth > windowW + scale) {
                            imgHeight = windowH * scale;
                            imgWidth = imgHeight / readHeight * realWidth;
                            if (imgWidth > windowW * scale) {
                                imgWidth = windowW * scale;
                            }
                        } else if (realWidth > windowW * scale) {
                            imgWidth = windowW * scale;
                            imgHeight = imgWidth / realWidth * readHeight;
                        } else {
                            imgWidth = realWidth;
                            imgHeight = readHeight;
                        }
                        $(bigimg).css("width", imgWidth);
                        var w = (windowW - imgWidth) / 2;
                        var h = (windowH - imgHeight) / 2;
                        $(innerdiv).css({ "top": h, "left": w });
                        $(outerdiv).fadeIn("fast");
                    });
                    $(outerdiv).click(function () {
                        $(this).fadeOut("fast");
                    });
                };
