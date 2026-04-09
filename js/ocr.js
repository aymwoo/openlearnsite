let messageHistory = []; // 新增：存储对话历史的数组
		
		var port=":2000";//端口
		var lor =location.origin + port;// http://192.168.1.3
		console.log(lor);
		
        const apiUploadUrl = lor+"/upload";
        const apiChatUrl = lor+"/ocr";
			
		const userfile = document.getElementById("userfile");
		const uploadbtn = document.getElementById("uploadbtn");
		const userchatbar = document.getElementById("chatbar");
        const sendButtonmsg = document.getElementById("btnmsg");
		let imgfilename ;
        sendButtonmsg.disabled = true;
		
		//console.log(userform.action);
		// 为表单添加提交事件监听器
        userfile.addEventListener('change', async (event) => {
			  const files = event.target.files;
			  if (!files) return;

			  // 创建图片对象					
				//console.log("上传文件是",files[0]);
				var formData = new FormData(); // 创建FormData对象
				formData.append("file",files[0]);
				// 使用Fetch API发送POST请求
				fetch(apiUploadUrl, {
					method: 'POST',
					body: formData
				})
				.then(response => response.json()) // 将响应解析为JSON
				.then(data => {
					// 在页面上显示服务器响应
					var result =  JSON.parse(JSON.stringify(data));
					imgfilename = result["response"];
					console.log(imgfilename);
					addMessage("upload", imgfilename);				
					sendButtonmsg.disabled = false;
				})
				.catch(error => {
					console.error('Error:', error); // 在控制台显示错误信息
				});
        });
		
		uploadbtn.addEventListener('click', function() {
			userfile.click();
		});

        async function sendText() {
            const userInput = imgfilename;
            if (!userInput) return;
			
            sendButtonmsg.disabled = true;
			uploadbtn.disabled = true;

            // 新增：将用户消息添加到历史记录
			const userMessage = { role: "upload", content: userInput };
            messageHistory.push(userMessage);

            try {
                document.getElementById("loading").style.display = "block";
                const response = await fetch(apiChatUrl, {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({ messages: userMessage }) 
                });
                
                const data = await response.json();
                
                // 新增：将AI回复添加到历史记录
                const botMessage = { role: "assistant", content: data.response };
                addMessage("bot", data.response);
			
				const chatTitle = document.createElement("div");
				chatTitle.className="chattitle";
				chatTitle.innerHTML=data.response;
				userchatbar.appendChild(chatTitle);
			
            } catch (error) {
                console.error("Error:", error);
                const errorMessage = { role: "assistant", content: `Error: ${error.message}` };
                addMessage("sys", `Error: ${error.message}`);
            } finally {
                document.getElementById("loading").style.display = "none";				
				uploadbtn.disabled  = false;
				imgfilename ="";
            }
        }
		
		
		function addMessage(role, content) {
			const chatHistory = document.getElementById("chatHistory");
			const messageDiv = document.createElement("div");
			messageDiv.className = `message ${role}`;
			if (role === "upload") {				
				messageDiv.innerHTML = `<img class="upimg" src="../deepseek/uploads/${content}" />`;
			}else {
				messageDiv.innerHTML = `<div class="user">${content}</div>`;
			}
			chatHistory.appendChild(messageDiv);

			// 滚动到底部
			chatHistory.scrollTop = chatHistory.scrollHeight;
		}


        var docurl = document.URL;
		var ipurl = docurl.substring(0, docurl.lastIndexOf("/"));
		var id = window.__ocrConfig.id;
        function returnurl() {
            if (confirm('是否离开当前活动页面？请先保存作品。') == true) {
                window.location.href = window.__ocrConfig.fpage
            }
        }

        function savechat() { 
	        var preview = document.getElementById("chatHistory");
            var htmlcode ="";// preview.innerHTML;使用缩略图预览
        	if (messageHistory.length>0) {
                html2canvas(preview).then(pic => {					
        	        var urls = '../student/uploadtopic.ashx?id=' + id;
			        var title = "";
			        var Cover = blob(pic.toDataURL("image/jpg",0.5)); 
			        var Content = htmlcode;
			        var Extension = "ocr";
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
