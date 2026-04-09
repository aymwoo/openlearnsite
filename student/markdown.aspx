<%@ Page Language="C#" AutoEventWireup="true" CodeFile="markdown.aspx.cs" Inherits="student_markdown" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head runat="server">
        <meta charset="utf-8" />
        <title>Markdown写作</title>
        <link rel="stylesheet" href="../markdown/css/editormd.css" />
        <link rel="shortcut icon" href="../markdown/favicon.ico" type="image/x-icon" />
		<link rel="stylesheet" href="../deepseek/all.min.css">
		<style type="text/css">
			.markdown-toolbar {
				display: flex;
				flex-wrap: wrap;
				align-items: center;
				gap: 12px;
				padding-bottom: 8px;
			}

			.markdown-toolbar__brand {
				display: flex;
				align-items: center;
				gap: 12px;
				min-width: 0;
				flex: 1 1 320px;
			}

			.markdown-toolbar__brand img {
				width: 32px;
				height: 32px;
				flex-shrink: 0;
			}

			.markdown-toolbar__actions {
				display: flex;
				flex-wrap: wrap;
				justify-content: flex-end;
				gap: 10px;
			}

			.markdown-toolbar__btn {
				display: inline-flex;
				align-items: center;
				justify-content: center;
				gap: 8px;
				min-width: 110px;
				height: 40px;
				padding: 0 16px;
				border: 0;
				border-radius: 12px;
				background: linear-gradient(135deg, #2563eb 0%, #1d4ed8 100%);
				color: #ffffff;
				font-size: 14px;
				font-weight: 700;
				white-space: nowrap;
				box-shadow: 0 14px 28px -18px rgba(37, 99, 235, 0.82);
				cursor: pointer;
				transition: transform .2s ease, box-shadow .2s ease, filter .2s ease;
			}

			.markdown-toolbar__btn:hover {
				transform: translateY(-1px);
				box-shadow: 0 18px 30px -18px rgba(37, 99, 235, 0.9);
				filter: brightness(1.03);
			}

			.markdown-toolbar__btn--neutral {
				background: linear-gradient(135deg, #475569 0%, #334155 100%);
				box-shadow: 0 14px 28px -18px rgba(51, 65, 85, 0.72);
			}

			#mdTitle {
				min-width: 0;
				flex: 1 1 auto;
				font-size: 1.3rem;
				font-weight: 700;
				line-height: 1.4;
				padding: 4px 8px;
				border-radius: 10px;
			}

			#mdTitle:focus {
				outline: none;
				background: #eff6ff;
			}

			@media (max-width: 768px) {
				.markdown-toolbar__actions {
					width: 100%;
					justify-content: stretch;
				}

				.markdown-toolbar__btn {
					flex: 1 1 120px;
					min-width: 0;
				}
			}
		</style>
    
    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
</head>
    <body>
        <div id="layout">
	            <div class="markdown-toolbar">
				<div class="markdown-toolbar__brand">
					<img src="../markdown/favicon.png" alt="Markdown" />
					<div id="mdTitle" contenteditable="true"><%=Mytitle%></div>
				</div>
				<div class="markdown-toolbar__actions">
					<button type="button" onclick="savemd()" title="保存作品" class="markdown-toolbar__btn"><i class="fa fa-save" aria-hidden="true"></i><span>保存作品</span></button>
					<button type="button" onclick="returnurl()" title="返回学案" class="markdown-toolbar__btn markdown-toolbar__btn--neutral"><i class="fa fa-reply" aria-hidden="true"></i><span>返回学案</span></button>
				</div>
            </div>
			<div >
				<div id="editormd" >
					<textarea style="display:none;"><%=codefile %></textarea>
				</div>
			</div>
        </div>
        <script src="../js/jquery.min.js"></script>
        <script src="../code/html2canvas.min.js" type="text/javascript"></script>
        <script src="../markdown/editormd.js"></script>
<script type="text/javascript">
    var mdEditor;

    $(function () {
        mdEditor = editormd("editormd", {
            height: 640,
            syncScrolling: "single",
            path: "../markdown/lib/",
            toolbarIcons: "me",
            table: true,
			imageUpload: true, // 开启图片上传功能
			imageFormats: ["jpg", "jpeg", "gif", "png", "bmp", "webp"], // 允许上传的图片格式
			imageUploadURL: "uploadmd.ashx", // 图片上传接口的 URL
			onload: function () {
				console.log('编辑器准备就绪.');
			}
        });
    });

//---------------------------------------------------------------------------//
	var docurl = document.URL;
    var ipurl = docurl.substring(0, docurl.lastIndexOf("/"));
    var id = "<%=Id %>";
    function returnurl() {
        if (confirm('是否离开当前活动页面？请先保存作品。') == true) {
            window.location.href = "<%=Fpage %>"
        }
    }

    function savemd() { 
	    var preview = document.getElementById("md-preview");
	    var mdTitle = document.getElementById("mdTitle");
        // 获取 Markdown 内容       
        var htmlcode =mdEditor.getMarkdown(); // 使用缩略图预览 mdTitle
        // 获取 HTML 内容
        //var htmlContent = mdEditor.getHTML();
        if (htmlcode) {
            html2canvas(preview).then(pic => {					
        	    var urls = '../student/uploadtopic.ashx?id=' + id;
			    var title = mdTitle.innerText;
			    var Cover = blob(pic.toDataURL("image/jpg",0.5)); 
			    var Content = htmlcode;
			    var Extension = "markdown";
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
        	    }).fail(function (res) {
        	        console.log(res)
        	    }); 	
            
            });		
        }
        else{
            alert("请先写作，才可以保存！");
        }
    }

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

</script>
    </body>
</html>
