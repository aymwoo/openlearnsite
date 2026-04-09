window.__learnStatus = {
        snum: window.__kitymindConfig.snum,
        sname: window.__kitymindConfig.lsSname,
        sgrade: window.__kitymindConfig.lsSgrade,
        sclass: window.__kitymindConfig.lsSclass,
        sid: window.__kitymindConfig.lsSid,
        cid: window.__kitymindConfig.lsCid,
        lid: window.__kitymindConfig.lsLid,
        ltitle: window.__kitymindConfig.lsLtitle,
        ltype: window.__kitymindConfig.lsLtype
    };

    window.addEventListener('load', function () {
        var codefile = window.__kitymindConfig.codefile;
        if (codefile != "") {
            codefile = decodeURIComponent(codefile);
            console.log(codefile);
            var fileType = 'json'
            editor.minder.importData(fileType, codefile).then(function (data) {
                console.log(data)
            });
        }
    });
    var fpage = window.__kitymindConfig.fpage;
    function returnurl() {
        if (confirm('是否要离开此页面？') == true) {
            window.location.href = window.__kitymindConfig.fpage
        }
    }
    //点击导出链接自动下载
    function downfile(link) {
        var title = editor.minder.getRoot().getData("text");
        exportType = 'json';
        console.log("保存信息");

        var content = editor.minder.exportData(exportType);
        var strJson = content.fulfillValue;

        exportType = 'png';
        editor.minder.exportData(exportType).then(function (content) {
            var blob = new Blob();
            blob = dataURLtoBlob(content); //将base64编码转换为blob对象

            var id = window.__kitymindConfig.id;
            var urls = 'uploadkitymind.ashx?id=' + id;
            var formData = new FormData();
            //console.log("编码信息");
            var km = encodeURIComponent(strJson);

            formData.append('title', title);
            formData.append('km', km);
            formData.append('thumb', blob);
            //console.log(km);
            //console.log(blob);

            $.ajax({
                url: urls,
                type: 'POST',
                cache: false,
                data: formData,
                processData: false,
                contentType: false
            }).done(function (res) {
                if (window.LearnStatus && typeof window.LearnStatus.submitted === "function") {
                    window.LearnStatus.submitted();
                }
                alert("保存成功！");
                $(".export").attr("disabled", "false");
                console.log(res)
            }).fail(function (res) {
                alert("保存失败！");
                console.log(res)
            });

        });
    }

    //base64转换为图片blob
    function dataURLtoBlob(dataurl) {
        var arr = dataurl.split(',');
        //注意base64的最后面中括号和引号是不转译的
        var _arr = arr[1].substring(0, arr[1].length - 2);
        var mime = arr[0].match(/:(.*?);/)[1],
    bstr = atob(_arr),
    n = bstr.length,
    u8arr = new Uint8Array(n);
        while (n--) {
            u8arr[n] = bstr.charCodeAt(n);
        }
        return new Blob([u8arr], {
            type: mime
        });
    }

angular.module('kityminderDemo', ['kityminderEditor'])
	.controller('MainController', function ($scope) {
	    $scope.initEditor = function (editor, minder) {
	        window.editor = editor;
	        window.minder = minder;
	    };
	});
