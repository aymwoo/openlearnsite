var editor = ace.edit("left", {
	        theme: "ace/theme/chrome",
	        mode: "ace/mode/html"
	    });
        var fontsize=15;
		editor.setShowPrintMargin(false);
	    editor.setFontSize(fontsize);
	    editor.focus();
		//editor.setOption("dragEnabled", false);
		editor.getSession().setUseWorker(false);
	    editor.setOptions({
	        wrap: true,
			enableBasicAutocompletion: true,
			enableSnippets: true,
	        enableLiveAutocompletion: true
	    });

		function backward(){
			editor.undo();
		}
		
		function forward(){
			editor.redo();
		}
		
        function fontbig(){
	        fontsize+=1;
	        editor.setFontSize(fontsize);	
        }

        function fontsmall(){
	        fontsize-=1;
	        if (fontsize<16){
		        fontsize=16;
	        }
	        editor.setFontSize(fontsize);	
        }

	    var previewFrame = document.getElementById("preview-frame");
		const frameDoc = previewFrame.contentDocument || previewFrame.contentWindow.document;
		var htmlpage = document.getElementById("html_page");
	    var snum = window.__htmleditorConfig.snum;
	    var id = window.__htmleditorConfig.id;
	    var cf = window.__htmleditorConfig.codefile;
	    var fpage = window.__htmleditorConfig.fpage;
        var mypage= window.__htmleditorConfig.mypage;
		htmlpage.value = mypage;
        var myweb="../website/"+snum+"/"+mypage;
	    var codefile = decodeURIComponent(window.atob(cf)); //定义字典

	    var sessionkey = "htmlcode" + snum + "-" + id + "-" + mypage;

		// 更新预览函数
		function updatePreviewold(htmlCode) {
                frameDoc.open();
                htmlCode = updateUrlsrc(htmlCode);
                frameDoc.write(htmlCode);//同步预览
				document.title = frameDoc.title;//同步标题
                frameDoc.close();
        }
        
		function updatePreview(htmlCode) {
			// 处理 HTML
			htmlCode = updateUrlsrc(htmlCode);			
			// 直接使用 srcdoc 替换内容（这会创建全新的文档环境）
			previewFrame.srcdoc = htmlCode;			
			// 异步更新标题
			previewFrame.onload = function() {
				try {
					const frameDoc = previewFrame.contentDocument || previewFrame.contentWindow.document;
					if (frameDoc.title) {
						document.title = frameDoc.title;
					}
				} catch (e) {
					// 忽略跨域错误
				}
			};
		}

function updateUrlsrc(html) {
    const doc = new DOMParser().parseFromString(html, 'text/html');
    const root = `../website/${snum}/`;
    const isExternal = url => url && /^(https?:|\/\/|data:)/i.test(url);
    const hasRoot = url => url && (url.startsWith(root) || url.startsWith(`website/${snum}/`));
    
    // 处理 HTML 元素的属性
    const processElementAttr = (el, attr) => {
        const url = el.getAttribute(attr)?.trim();
        if (!url || isExternal(url) || hasRoot(url)) return;
        el.setAttribute(attr, root + (url.startsWith('/') ? url.substring(1) : url));
    };
    
    // 处理 JavaScript 代码中的数组对象
    const processJavaScript = () => {
        // 获取所有的 script 标签
        const scripts = doc.querySelectorAll('script:not([src])');
        
        scripts.forEach(script => {
            const scriptContent = script.textContent;
            if (!scriptContent) return;
            
            // 查找并替换数组对象中的 src 属性
            let modifiedContent = scriptContent;
            
            // 方法1: 使用正则表达式匹配常见的数组格式
            const patterns = [
                // 匹配 musicList 或类似数组中的 src 属性
                /(const\s+\w+\s*=\s*\[\s*\{[\s\S]*?src:\s*["'])([^"']+)(["'][\s\S]*?\}\s*\])/gi,
                // 匹配对象中的 src 属性（更通用的模式）
                /(src:\s*["'])([^"']+)(["'])/gi,
                // 匹配 JSON 格式的 src 属性
                /("src":\s*")([^"]+)(")/gi
            ];
            
            patterns.forEach(pattern => {
                modifiedContent = modifiedContent.replace(pattern, (match, prefix, url, suffix) => {
                    if (!url || isExternal(url) || hasRoot(url)) {
                        return match; // 不处理外部链接或已包含根路径的链接
                    }
                    return prefix + root + (url.startsWith('/') ? url.substring(1) : url) + suffix;
                });
            });
                       
            // 如果内容有修改，更新 script 标签
            if (modifiedContent !== scriptContent) {
                script.textContent = modifiedContent;
            }
        });
    };
    
    // 处理 HTML 元素
    [
        ['img', 'src'],
        ['audio', 'src'],
        ['video', 'src'],
        ['source', 'src'],
        ['link[rel="stylesheet"][href]', 'href'],
        ['a[href]', 'href'],
        ['script[src]', 'src']  // 处理有 src 属性的 script 标签
    ].forEach(([sel, attr]) => {
        doc.querySelectorAll(sel).forEach(el => processElementAttr(el, attr));
    });
    
    // 处理内联 JavaScript 代码
    processJavaScript();
    
    return doc.documentElement.outerHTML;
}
		
		function autosaving(){
			var codestr = editor.getValue();
	        localStorage .setItem(sessionkey, codestr);//如果行数大于模板数量8行就自动保存
			//console.log("自动保存");	        
		}
		
	    function autopreview() {
	        var codestr = editor.getValue();
	        if (codestr != null && codestr != "") {
				updatePreview(codestr);
	        }
	    }
	    function getsession() {
	        var codestr = localStorage.getItem(sessionkey);
	        if (codestr != null && codestr != "") {
	            editor.setValue(codestr, 1);
	            //localStorage .clear();//读取后清除，防止污染
	            updatePreview(codestr);
				console.log("读取临时缓存");
				console.log(codestr);
	        }
			else{
				console.log("读取临时缓存无");
			}
	    }
		
		editor.getSession().on('change', function() {
			autopreview();
			autosaving();
            $(".buttonsave").css("background-color","#FF8080" );
		  }
		);

	    if (codefile != '' && codefile != null) {
	        setcode(codefile);
	        updatePreview(codefile);       
	        console.log("读取数据库储存程序");
			//console.log(codefile);			
			$(".buttonsave").css("background-color","" );
	    }
		else{
			getsession();	       
		}
		
	    function setcode() {
	        editor.setValue(codefile, 1);
	    }
        function blob(dataURI) {
            var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0]; 
            var byteString = atob(dataURI.split(',')[1]); 
            var arrayBuffer = new ArrayBuffer(byteString.length); 
            var intArray = new Uint8Array(arrayBuffer); 

            for (var i = 0; i < byteString.length; i++) {
                intArray[i] = byteString.charCodeAt(i);
            }
            return new Blob([intArray], {type: mimeString});
        }

	    function savehtml() { 
			var preview = document.getElementById("left");  
        	var htmlcode = editor.getValue();
        	if (htmlcode != null && htmlcode != "") {					
        	        localStorage .setItem(sessionkey, htmlcode); //保存时更新临时数据
        	        var urls = 'uploadhtml.ashx?id=' + id;
        	        var formData = new FormData();
        	        var encodehtml = window.btoa(encodeURIComponent(htmlcode));
					//console.log(encodehtml);
        	        formData.append('codefile', encodehtml);
        	        formData.append('mypage', mypage);
                    formData.append('cover', "");

        	        $.ajax({
        	            url: urls,
        	            type: 'POST',
        	            cache: false,
        	            data: formData,
        	            processData: false,
        	            contentType: false
        	        }).done(function (res) {
        	            alert("保存成功！");
						$(".buttonsave").css("background-color","" );
        	            //console.log(res);
        	        }).fail(function (res) {
        	            console.log(res)
        	        }); 	
        	}
        }

        function showhtml(){
            window.open(myweb,"blank");  
           // window.location.href=mypage;
        }

		function downhtml(){
			var filename="mypage";
			var type="text/html";
			var content= editor.getValue();
			var ele = document.createElement('a');// 创建下载链接
			ele.download = filename;//设置下载的名称
			ele.style.display = 'none';// 隐藏的可下载链接
			// 字符内容转变成blob地址
			var blober = new Blob([content],{type});
			ele.href = URL.createObjectURL(blober);
			// 绑定点击时间
			document.body.appendChild(ele);
			ele.click();
			// 然后移除
			document.body.removeChild(ele);
		}

        function returnurl() {
			localStorage .clear(); //返回后清除，防止污染
            if (confirm('是否离开当前活动页面？请先保存作品。') == true) {
                window.location.href = fpage;
            }
        }
        
        function example() {
			if( editor.session.getLength()<8)
			{
				var examplecode = "JTNDaHRtbCUzRSUwRCUwQSUyMCUyMCUyMCUyMCUzQ2hlYWQlM0UlMEQlMEElMjAlMjAlMjAlMjAlMjAlMjAlMjAlMjAlM0N0aXRsZSUzRSVFNyVCRCU5MSVFOSVBMSVCNSVFNiVBMCU4NyVFOSVBMiU5OCUzQyUyRnRpdGxlJTNFJTBEJTBBJTIwJTIwJTIwJTIwJTNDJTJGaGVhZCUzRSUwRCUwQSUyMCUyMCUyMCUyMCUzQ2JvZHklM0UlMEQlMEElMjAlMjAlMjAlMjAlMjAlMjAlMjAlMjAlMEQlMEElMjAlMjAlMjAlMjAlM0MlMkZib2R5JTNFJTBEJTBBJTNDJTJGaHRtbCUzRQ";
				var exampledecode = decodeURIComponent(window.atob(examplecode)); //网页模板
				updatePreview(exampledecode);
				editor.setValue(exampledecode, 1);
			}
        }

window.addEventListener('load', function () {
            var main = document.getElementById('main');
            var resize = document.getElementById('resize');
            var left = document.getElementById('left');
            var right = document.getElementById('right');
            var minwidth = 400;
            // 初始化布局
            resize.style.left = (main.clientWidth / 2) + 'px';

            resize.onmousedown = function (e) {
                e.preventDefault();
                // 添加拖动样式
                resize.classList.add('dragging');
                // 临时禁用iframe的指针事件
                previewFrame.style.pointerEvents = 'none';
                // 记录初始位置
                var startX = e.clientX;
                var startLeftWidth = left.offsetWidth;
                document.onmousemove = function (e) {
                    // 计算宽度变化
                    var deltaX = e.clientX - startX;
                    var newLeftWidth = startLeftWidth + deltaX;
                    // 应用最小宽度限制
                    if (newLeftWidth < minwidth) newLeftWidth = minwidth;
                    if (newLeftWidth > main.clientWidth - minwidth) newLeftWidth = main.clientWidth - minwidth;

                    // 设置左侧宽度
                    left.style.width = newLeftWidth + 'px';
                    // 设置右侧宽度
                    right.style.width = (main.clientWidth - newLeftWidth - resize.offsetWidth) + 'px';
                    editor.resize(); // 重新调整编辑器大小
                };
                document.onmouseup = function () {
                    // 移除拖动样式
                    resize.classList.remove('dragging');
                    // 恢复iframe的指针事件
                    previewFrame.style.pointerEvents = 'auto';
                    // 清除事件监听
                    document.onmousemove = null;
                    document.onmouseup = null;
                };
            };
        });


        $(".keyword").click(function () {
            var keytxts = $(this).html();
            var keystr = keytxts.split('<br>');
            var keytxt = keystr[0];
            console.log(keytxt);
            var cmdstr = "";
            switch (keytxt) {
                case "html":
                    cmdstr = "<html>\r\n\n\n</html>";
                    break;
                case "head":
                    cmdstr = "<head>\r\n\t</head>";
                    break;
                case "title":
                    cmdstr = "<title></title>";
                    break;
                case "style":
                    cmdstr = "\r\n\t";
                    break;
                case "body":
                    cmdstr = '<body bgcolor=" " background=" ">\r\n\t</body>';
                    break;
                case "center":
                    cmdstr = "\r\n\t<center>\r\n\n\t</center>";
                    break;
                case "h1":
                    cmdstr = "\r\n\t\t<h1> </h1>";
                    break;
                case "a":
                    cmdstr = '\r\n\t\t\t<a href=" #链接网址 ">链接文字</a>';
                    break;
                case "font":
                    cmdstr = '<font face="宋体" size=16  color="red">文字</font>';
                    break;
                case "p":
                    cmdstr = "\r\n\t\t<p>\r\n\n\t\t</p>";
                    break;
                case "div":
                    cmdstr = "\r\n\t<div>\r\n\n\t</div>";
                    break;
                case "img":
                    cmdstr = '\r\n\t\t<img src=" " width="300" ></img>';
                    break;
                case "video":
                    cmdstr = '\r\n\t\t<video src=" " width="500"  controls></video>';
                    break;
                case "audio":
                    cmdstr = '\r\n\t\t<audio src=" " controls></audio>';
                    break;
                case "form":
                    cmdstr = '\r\n\t<form action="../../website/post.html" target="_blank">\r\n\n\t</form>';
                    break;
                case "br":
                    cmdstr = '\r\n\t<br>';
                    break;
                case "input":
                    cmdstr = '\r\n\t\t<input type="text" name="userinput"  class="px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300">';
                    break;
                case "username":
                    cmdstr = '\r\n\t\t<input type="text" name="username"  class="px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300">';
                    break;
                case "content":
                    cmdstr = '\r\n\t\t<input type="text" name="content"  class="px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300">';
                    break;
                case "submit":
                    cmdstr = '\r\n\t\t<input type="submit" value="提交"  class="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0">';
                    break;
                default:
                    break;
            }

            var cursorPosition = editor.getCursorPosition();
            editor.session.insert(cursorPosition, cmdstr);
            editor.focus();

        });

function showShare() {
            var urlat = "webspace.aspx";
            TINY.box.show({ iframe: urlat, boxid: 'frameless', width: 550, height: 400, fixed: false, maskopacity: 100, close: true })
        }
        function showMission() {
            $("#mcontext").slideToggle();
        }
