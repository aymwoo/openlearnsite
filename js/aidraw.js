const userTextarea = document.getElementById("userInput");
		const chatHistory = document.getElementById("chatHistory");
		const userchatbar = document.getElementById("chatbar");
        const sendButtonphoto = document.getElementById("btnphoto");
		var imgload = new Image();
		imgload.src = "loading.gif";
        var count =0;
        var myprompt = "";
		var port=":2000";//端口
		var lor=location.origin+port;// http://192.168.1.3
		console.log(location.origin);
		
        const apiPhotoUrl = lor+"/photo";
		
		//文生图
		async function aiPhoto(){
			sendButtonphoto.disabled = true;
			chatHistory.innerHTML ="";
			chatHistory.appendChild(imgload);
			var prompt = userTextarea.value.trim();
            width = 640;
            height = 480;
            seed = 30;
			model = 'flux';	
			
			myprompt = prompt;
            const userMessage = { role: "user", content: prompt };
			
			response = await fetch(apiPhotoUrl, {
				method: "POST",
				headers: { "Content-Type": "application/json" },
				body: JSON.stringify({ messages: userMessage }) // 改为发送完整对话历史
			});
			
			const data = await response.json();
			addMessage(data.response);
		}
	
	
	function addMessage(content){
			
		// 获取canvas元素和绘图上下文
		var canvas = document.createElement('canvas');
		canvas.width = width;
		canvas.height = height;
		var ctx = canvas.getContext('2d');		
					// 创建一个新的Image对象
		var img = new Image();
		img.src = content;
		img.title = myprompt;
		// 当图片加载完成后，将其绘制到canvas上
		img.onload = function() {				 
			// 在canvas上绘制图片，可以根据需要调整位置和大小
			chatHistory.innerHTML ="";
			img.id="photoview";
			chatHistory.appendChild(img);										
				
            // 平滑滚动到顶部表单区域
            window.scrollTo({
                top: userTextarea.offsetTop - 200,
                behavior: 'smooth'
            });
			count++;
            myprompt = prompt
			sendButtonphoto.disabled = false;
		};
			
		// 创建缩略图img元素
		var thumb = document.createElement('img');
		thumb.src = img.src;
		thumb.width = 160; 
		thumb.title = myprompt;
		// 添加onclick事件
		thumb.onclick = function() {
			var photoview = document.getElementById("photoview");
			photoview.src = this.src;
		};
		userchatbar.appendChild(thumb);	
	}
	
	
	// 自动调整输入框高度
	document.getElementById("userInput").addEventListener("input", (event) => {
		event.target.style.height = "auto";
		event.target.style.height = event.target.scrollHeight + "px";
	});
		
		
	initPromptTags();
	initStyleSelector();
	
	    /**
     * 初始化提示词标签
     */
    function initPromptTags() {
        // 获取提示词容器
        const promptTagsContainer = document.querySelector('.prompt-tags-container');
        if (!promptTagsContainer) {
            console.error('找不到提示词容器元素');
            return;
        }
        
        // 克隆并替换容器以移除之前的事件监听器
        const newContainer = promptTagsContainer.cloneNode(true);
        promptTagsContainer.parentNode.replaceChild(newContainer, promptTagsContainer);
        
        // 获取所有提示词标签
        const promptTags = newContainer.querySelectorAll('.prompt-tag');
        
        // 为每个提示词标签添加点击事件
        promptTags.forEach(tag => {
            tag.addEventListener('click', function(event) {
                // 阻止事件冒泡
                event.stopPropagation();
                
                // 获取提示词内容
                const promptText = this.getAttribute('data-prompt');
                if (!promptText) return;
                
                // 获取提示词输入框
                const promptInput = document.getElementById('userInput');
                if (!promptInput) return;
                
                // 填充到输入框
                promptInput.value = promptText;
                               
                // 让输入框获得焦点
                promptInput.focus();
                
                // 添加简单的动画效果
                this.style.transform = 'scale(0.9)';
                setTimeout(() => {
                    this.style.transform = '';
                }, 200);                
                
            });
        });
        
    }
	
	/**
     * 初始化风格选择器
     */
    function initStyleSelector() {
        // 获取所有风格标签及其父元素
        const styleLabels = document.querySelectorAll('.style-tag');
        
        // 找到默认选中的风格并添加active类
        const defaultStyle = document.querySelector('.style-tag input:checked');
        if (defaultStyle) {
            defaultStyle.parentElement.classList.add('active');
        }
        
        // 清除并重新添加点击事件
        styleLabels.forEach(label => {
            // 清除可能存在的原有事件监听
            const newLabel = label.cloneNode(true);
            label.parentNode.replaceChild(newLabel, label);
            
            // 获取标签中的radio input元素
            const radioInput = newLabel.querySelector('input[type="radio"]');
            
            // 为标签添加点击事件（不是为radio添加change事件）
            newLabel.addEventListener('click', (e) => {
                // 阻止事件冒泡
                e.stopPropagation();
                
                // 确保radio被选中
                if (radioInput) {
                    radioInput.checked = true;
                    
                    // 触发一个人工的change事件
                    const changeEvent = new Event('change', { bubbles: true });
                    radioInput.dispatchEvent(changeEvent);
                    
                    // 高亮显示选中的标签
                    document.querySelectorAll('.style-tag').forEach(el => {
                        el.classList.remove('active');
                    });
                    newLabel.classList.add('active');
                    
                    // 显示提示
                    const styleName = newLabel.querySelector('span').textContent.trim();
					defaultstyle = styleName;
                    // 如果用户正在输入提示词，可以提示风格已改变
                    const promptInput = document.getElementById('prompt');
                    if (promptInput && promptInput.value.trim() !== '') {
                        // 已禁用弹窗提示
                    }
                }
            });
        });
        
    }
	
    
        var docurl = document.URL;
		var ipurl = docurl.substring(0, docurl.lastIndexOf("/"));
		var id = window.__aidrawConfig.id;
        function returnurl() {
            if (confirm('是否离开当前活动页面？请先保存作品。') == true) {
                window.location.href = window.__aidrawConfig.fpage
            }
        }

        function savechat() { 
	        var preview = document.getElementById("chatHistory");
            var htmlcode ="";// preview.innerHTML;使用缩略图预览
        	if (count > 0) {
                html2canvas(preview, { allowTaint: true }).then(pic => {					
        	        var urls = '../student/uploadtopic.ashx?id=' + id;
			        var title = "";//绘画提示词
			        var Cover = blob(pic.toDataURL("image/jpg",0.5)); 
			        var Content = htmlcode;
			        var Extension = "text-to-image";
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
