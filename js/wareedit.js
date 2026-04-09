var cid = window.__wareeditConfig.cid;
    
        // 页面加载完成后初始化
        window.addEventListener('load', function() {
            loadFiles();
            setupDragAndDrop();
            document.getElementById('fileInput').onchange = handleFileSelect;
        });
        // 加载文件列表 - 关键修复：直接使用路径，不进行编码
        function loadFiles() {
            var xhr = new XMLHttpRequest();
            // 关键修复：直接使用路径参数，不进行encodeURIComponent编码
            xhr.open('GET', 'ware.ashx?action=files&cid=' + cid + '&t=' + new Date().getTime(), true);
            
            xhr.onreadystatechange = function() {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    try {
                        var files = JSON.parse(xhr.responseText);
                        //console.error('文件列表结构:', files);
                        renderFileList(files);
                    } catch (e) {
                        console.error('解析文件列表失败:', e);
                        alert('加载文件列表失败: ' + xhr.responseText);
                    }
                }
            };
            xhr.send();
        }

        // 渲染文件列表 - 关键修复：图片显示缩略图
         function renderFileList(files) {
            const fileList = document.getElementById('fileList');
            
            if (files && files.length > 0) {
                let html = '';
                
                for (let i = 0; i < files.length; i++) {
                    const file = files[i];
                    const icon = getFileIcon(file.name);
                    const size = formatFileSize(file.size);
                    const isImage = isImageFile(file.name);
                    const isHtml = isHtmlFile(file.name);
                    const date = file.date;
                    //console.log("测试：\r\n",date);

                    const fname = encodeURIComponent(file.path.replace(/\\/g, '/'));//文件名编码
                    // 使用真实文件URL
                    let fileUrl = `../store/${cid}/${fname}`;
                    const filedel = file.path.replace(/\\/g, '/');
                    let filecopy = `${fname}`;
                                        

                    // 根据当前视图生成不同的HTML结构
                    
                        html += `
                        <div class="file-item list-view">
                            <div class="file-info list-view">
                                <div class="file-thumbnail list-view">
                                    ${isImage ? `<img src="${fileUrl}" alt="${file.name}"  >` : `<div style="font-size: 24px; color: #6c757d;">${icon}</div>`}
                                </div>
                                <div class="file-details list-view">
                                    <div class="file-name list-view"><a href="${fileUrl}" target="_blank">${file.name}</a></div>
                                    <div class="file-meta list-view">文件大小 ${size} 日期 ${date}</div>
                                </div>
                            </div>
                            <div class="file-actions">
                                ${isHtml ? `<button type="button" class="btn-success px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0"  onclick="selectFile('${file.name}','${fileUrl}')" title="设置为首页">设置</button>` : ""}
                                <button type="button" class="btn-danger px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" onclick="deleteFile('${filedel}')" title="删除">✖</button>
                            </div>
                        </div>`;
                    
                }
                
                fileList.innerHTML = html;
            } else {
                fileList.innerHTML = '<div class="empty-state"><i>📂</i><p>当前文件夹为空</p></div>';
            }
        }


        // 判断是否为图片文件
        function isImageFile(filename) {
            var imageExtensions = ['.jpg', '.jpeg', '.png', '.gif', '.bmp', '.webp'];
            var ext = (filename.split('.').pop() || '').toLowerCase();
            return imageExtensions.indexOf('.' + ext) !== -1;
        }
        
        // 判断是否为HTML文件
        function isHtmlFile(filename) {
            var ext = (filename.split('.').pop() || '').toLowerCase();
            return ext === 'html' || ext === 'htm';
        }

        // 获取文件图标
        function getFileIcon(filename) {
            var ext = (filename.split('.').pop() || '').toLowerCase();
            var icons = {
                'jpg': '🖼️', 'jpeg': '🖼️', 'png': '🖼️', 'gif': '🖼️', 'bmp': '🖼️', 'webp': '🖼️',
                'pdf': '📕',
                'doc': '📄', 'docx': '📄',
                'mp4': '🎬', 'avi': '🎬', 'mov': '🎬', 'mkv': '🎬',
                'mp3': '🎵', 'wav': '🎵', 'flac': '🎵',
                'txt': '📝',
                'html': '🌐', 'htm': '🌐',
                'zip': '📦', 'rar': '📦', '7z': '📦',
                'default': '📄'
            };
            
            return icons[ext] || icons.default;
        }
        
        // 格式化文件大小
        function formatFileSize(bytes) {
            if (bytes === 0) return '0 B';
            var k = 1024;
            var sizes = ['B', 'KB', 'MB', 'GB'];
            var i = Math.floor(Math.log(bytes) / Math.log(k));
            return (bytes / Math.pow(k, i)).toFixed(2) + ' ' + sizes[i];
        }

    // 设置拖放功能
    function setupDragAndDrop() {
        var uploadZone = document.getElementById('uploadZone');

        uploadZone.ondragover = function (e) {
            e.preventDefault();
            this.className += ' dragover';
        };

        uploadZone.ondragleave = function () {
            this.className = this.className.replace(' dragover', '');
        };

        uploadZone.ondrop = function (e) {
            e.preventDefault();
            this.className = this.className.replace(' dragover', '');
            handleDroppedFiles(e.dataTransfer.files);
        };
    }

    // 处理拖放的文件
    function handleDroppedFiles(files) {
        uploadFiles(files);
    }

    // 处理选择的文件
    function handleFileSelect(e) {
        uploadFiles(e.target.files);
        e.target.value = '';
    }
// 上传文件 - 关键修复：直接使用当前路径，不进行编码
function uploadFiles(files) {
    if (!files || files.length === 0) return;
    
    // 显示进度条
    var progressContainer = document.getElementById('uploadProgressContainer');
    var progressBar = document.getElementById('uploadProgressBar');
    var progressPercent = document.getElementById('uploadPercent');
    var uploadStatus = document.getElementById('uploadStatus');
    var fileName = document.getElementById('uploadFileName');
    
    // 检查单个文件大小
    var maxFileSize = 100 ; // 200MB
    var filesize = Math.trunc(files[0].size/1024/1024);
    if (filesize > maxFileSize) {
        alert(files[0].name + '\r\n\r\n 文件大小'+filesize+'MB，超过限制（最大'+maxFileSize+'MB）');
        return;
    }

    // 显示第一个文件名
    fileName.textContent = files[0].name;
    progressContainer.style.display = 'block';
    progressBar.style.width = '0%';
    progressPercent.textContent = '0%';
    uploadStatus.textContent = '准备上传...';
    
    var formData = new FormData();
    for (var i = 0; i < files.length; i++) {
        formData.append('files', files[i]);
    }
    
    var xhr = new XMLHttpRequest();
    xhr.open('POST', 'ware.ashx?action=upload&cid='+cid, true);
    
    // 添加上传进度监听
    xhr.upload.onprogress = function(e) {
        if (e.lengthComputable) {
            var percentComplete = (e.loaded / e.total) * 100;
            var roundedPercent = Math.round(percentComplete);
            
            progressBar.style.width = percentComplete + '%';
            progressPercent.textContent = roundedPercent + '%';
            
            if (percentComplete < 100) {
                uploadStatus.textContent = '上传中...';
            }
        }
    };
    
    xhr.onreadystatechange = function() {
        if (xhr.readyState == 4) {
            if (xhr.status == 200) {
                try {
                    var data = JSON.parse(xhr.responseText);
                    if (data.success) {
                        progressBar.style.width = '100%';
                        progressPercent.textContent = '100%';
                        uploadStatus.textContent = '上传完成！';
                        
                        // 延迟隐藏进度条，让用户看到完成状态
                        setTimeout(function() {
                            progressContainer.style.display = 'none';
                            loadFiles();
                        }, 1000);
                    } else {
                        uploadStatus.textContent = '上传失败: ' + data.message;
                        progressBar.style.backgroundColor = '#dc3545';
                        setTimeout(function() {
                            progressContainer.style.display = 'none';
                            alert('上传失败: ' + data.message);
                        }, 2000);
                    }
                } catch (e) {
                    uploadStatus.textContent = '上传失败，解析响应错误';
                    progressBar.style.backgroundColor = '#dc3545';
                    setTimeout(function() {
                        progressContainer.style.display = 'none';
                        alert('上传失败，解析响应错误');
                    }, 2000);
                }
            } else {
                uploadStatus.textContent = '上传失败，文件过大，状态码: ' + xhr.status;
                progressBar.style.backgroundColor = '#dc3545';
                setTimeout(function() {
                    progressContainer.style.display = 'none';
                    alert('上传失败，文件过大，状态码: ' + xhr.status);
                }, 2000);
            }
        }
    };
    
    xhr.send(formData);
}

// 删除文件 - 关键修复：直接使用文件路径，不进行编码
function deleteFile(filePath) {
    if (confirm('确定要删除这个文件吗？')) {
        var xhr = new XMLHttpRequest();
        console.log('删除文件，原始路径:', filePath);
        xhr.open('GET', 'ware.ashx?action=delete&cid='+cid+'&path=' + filePath, true);
        xhr.onreadystatechange = function() {
            if (xhr.readyState == 4 && xhr.status == 200) {
                try {
                    var data = JSON.parse(xhr.responseText);
                    if (data.success) {
                        loadFiles();
                    } else {
                        alert('删除失败: ' + data.message);
                    }
                } catch (e) {
                    alert('删除失败，解析响应错误');
                }
            }
        };
        xhr.send();
    }
}
    
function selectFile(fileName,filePath) {

    var TextBoxHtml = document.getElementById(window.__wareeditConfig.textBoxHtmlId);
    TextBoxHtml.value = decodeURIComponent(filePath);
    console.log("设置为首页",filePath);
    
}
