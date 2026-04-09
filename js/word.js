var id = window.__wordConfig.id;
    var key = "word" + id;
    var words = window.__wordConfig.words;

    function returnurl() {
        if (confirm('是否离开当前活动页面？请先保存作品。') == true) {
            window.location.href = window.__wordConfig.fpage
        }
    }

    function savework() {
        var title = "";
        var canvas = document.getElementsByTagName('canvas')[0];
        var Cover = blob(canvas.toDataURL());
        var Content = window.btoa(encodeURIComponent(JSON.stringify(editor.command.getValue())));
        var Extension = "word";
        var urls = 'uploadtopic.ashx?id=' + id;
        var formData = new FormData();
        formData.append('title', title);
        formData.append('cover', Cover);
        formData.append('content', Content);
        formData.append('ext', Extension);

        $.ajax({
            url: urls,
            type: 'POST',
            cache: false,
            data: formData,
            processData: false,
            contentType: false
        }).done(function (res) {
            alert("保存成功！");
            console.log(res)
        });
    }

    function showwork() {
        if (words != "") {
            var savedoc = JSON.parse(decodeURIComponent(atob(words)));
            //console.log(savedoc);
            editor.command.executeSetValue(savedoc.data);
            console.log("恢复文档");
        }
        else {
            var value = localStorage.getItem(key);
            if (value != null) {
                var docvalue = JSON.parse(value);
                editor.command.executeSetValue(docvalue.data);
                console.log("读取缓存");
            }
            else {
                console.log("新建文档");
            }
        }
    }
    setTimeout(showwork, 1000); //延迟执行加载文档
    function blob(dataURI) {
        var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];
        var byteString = atob(dataURI.split(',')[1]);
        var arrayBuffer = new ArrayBuffer(byteString.length);
        var intArray = new Uint8Array(arrayBuffer);

        for (var i = 0; i < byteString.length; i++) {
            intArray[i] = byteString.charCodeAt(i);
        }
        return new Blob([intArray], { type: mimeString });
    }

    // 设置定时器，每隔10秒调用一次saveToLocalStorage()函数
    setInterval(function () {
        var value = editor.command.getValue();
        saveToLocalStorage(key, value); // 调用保存到本地存储的函数
    }, 10000);

    // 保存到本地存储的函数
    function saveToLocalStorage(key, value) {
        localStorage.setItem(key, JSON.stringify(value));
        console.log("自动缓存");
    }

    function test() {
        console.log("字符串");
        var newWords = editor.command.getValue();
        var a = JSON.stringify(newWords.data);
        a = window.btoa(encodeURIComponent(a))
        console.log(a);
        b = decodeURIComponent(atob(a));
        b = JSON.parse(b);
        console.log("原格式");
        console.log(b);

        var words = '{"header":[],"main":[],"footer":[]}';
    }
