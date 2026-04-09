var id = window.__iframeConfig.id;
		var courseurl = window.__iframeConfig.fpage;
		var homeurl= window.__iframeConfig.mexample; 

		var homeframe = document.getElementById("homeframe");		
		homeland();
		
		function homeland(){
			homeframe.src=homeurl;
			//console.log("首页",homeurl);			
		}
		function backward(){
			window.history.back();
			console.log("后退");			
		}
		function forward(){
			window.history.forward();
			console.log("前进");			
		}	
        var docurl = document.URL;
		var ipurl = docurl.substring(0, docurl.lastIndexOf("/"));
		
        function returnurl() {
            if (confirm('是否离开当前活动页面？请先保存作品。') == true) {
                window.location.href = courseurl;
            }
        }

        // 上传图片
        var isup = false;
        var urlstr = "uploadworkm.aspx?lid=" + window.__iframeConfig.lid;
        var uploader = new plupload.Uploader({
            runtimes: 'html5,html4',
            browse_button: 'savebtn', // you can pass an id...
            url: urlstr,
            multi_selection: false,
            filters: {
                max_file_size: '60mb',
                mime_types: [
			        { title: "work files", extensions: window.__iframeConfig.ext }
		        ]
            },
            init: {
                FilesAdded: function (up, files) {
                    uploader.start();
                },
                UploadProgress: function (up, file) {
                    if (file.percent == 100 && !isup) {
                        isup = true;
                    }
                },
                UploadComplete: function (up, file) {
                    alert("作品已经提交成功！");
                }
            }
        });

        uploader.init();
