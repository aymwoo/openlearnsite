//acceptedFiles: ".txt,.pdf,.doc,.docx,.xlsx,.xls,.ppt,.pptx,.png,.jpg,.jpeg,.gif,.mp4,.py,.wav,.mp3,.psd,.fla,.rar",
        var isgroup = window.__groupshareConfig.isgroup;
        var iscommon = window.__groupshareConfig.iscommon;
        var can = window.__groupshareConfig.can;
        var urlstr = "share.ashx?isgroup=" + isgroup + "&iscommon=" + iscommon;
        if (can == "True") {
            $("#file_area").addClass("can-upload");
            $("#file_area").dropzone({
                url: urlstr,
                method: "POST",
                addRemoveLinks: true,
                maxFiles: 1, //一次性上传的文件数量上限
                maxFilesize: 30, //MB
                uploadMultiple: false,
                parallelUploads: 100,
                previewsContainer: false,
                success: function (file, response, e) {
                    alert(response);
                    location.reload();
                }
            });
        }
        else {
            $("#doc_area").attr("title", "");
            $("#file_area").removeClass("can-upload").attr("title", "");
        }
