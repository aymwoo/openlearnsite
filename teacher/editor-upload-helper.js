window.LearnSiteEditorUploadHelper = (function () {
    function uploadToKindEditor(upjs, file, callback) {
        var formData = new FormData();
        formData.append('imgFile', file);

        var xhr = new XMLHttpRequest();
        xhr.open('POST', upjs, true);
        xhr.onreadystatechange = function () {
            if (xhr.readyState === 4) {
                var res = {};
                try { res = JSON.parse(xhr.responseText || '{}'); } catch (e) { }
                callback(res);
            }
        };
        xhr.send(formData);
    }

    function insertUploadedLinkToWangEditor(editor, res) {
        if (!editor || !res || res.error !== 0 || !res.url) return;
        var name = res.filename || res.url.split('/').pop();
        editor.dangerouslyInsertHtml('<a href="' + res.url + '" target="_blank">' + name + '</a>');
    }

    function handleVditorUpload(vditor, upjs, files) {
        if (!vditor || !files || !files.length) return;
        uploadToKindEditor(upjs, files[0], function (res) {
            if (res.error === 0 && res.url) {
                var file = files[0];
                var isImage = file.type && file.type.indexOf('image/') === 0;
                var name = res.filename || file.name || res.url.split('/').pop();
                var text = isImage ? ('![' + name + '](' + res.url + ')') : ('[' + name + '](' + res.url + ')');
                vditor.insertValue(text);
            } else {
                alert(res.message || '上传失败');
            }
        });
    }

    return {
        uploadToKindEditor: uploadToKindEditor,
        insertUploadedLinkToWangEditor: insertUploadedLinkToWangEditor,
        handleVditorUpload: handleVditorUpload
    };
})();
