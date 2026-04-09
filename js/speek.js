let messageHistory = []; // 新增：存储对话历史的数组
		
		var port=":2000";//端口
		var lor=location.origin+port;// http://192.168.1.3
		console.log(location.origin);
		
        const apiChatUrl = lor+"/voice";
			
		const userTextarea = document.getElementById("userInput");
		const userchatbar = document.getElementById("chatbar");
        const sendButtonmsg = document.getElementById("btnmsg");
		const selectElement = document.getElementById("voiceSelect");

        async function sendText() {
            const userInput = userTextarea.value;
            const selectedValue = selectElement.value;
            if (!userInput) return;
			
            sendButtonmsg.disabled = true;
			userTextarea.disabled = true;

            // 新增：将用户消息添加到历史记录
			const userMessage = { role: selectedValue, content: userInput };
            messageHistory.push(userMessage);
            addMessage("user", userInput);

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
				chatTitle.innerHTML=userInput;
				userchatbar.appendChild(chatTitle);
			
            } catch (error) {
                console.error("Error:", error);
                const errorMessage = { role: "assistant", content: `Error: ${error.message}` };
                addMessage("sys", `Error: ${error.message}`);
            } finally {
                document.getElementById("loading").style.display = "none";
                sendButtonmsg.disabled = false;
				userTextarea.disabled = false;
                document.getElementById("userInput").value = "";
            }
        }
		
		
		function addMessage(role, content) {
			const chatHistory = document.getElementById("chatHistory");
			const messageDiv = document.createElement("div");
			messageDiv.className = `message ${role}`;
			if (role === "bot") {
				pauseAllAudio();//暂停所有
				content = "../deepseek/"+ content;
				messageDiv.innerHTML = ` <audio src="${content}" autoplay controls ></audio> <img class="download" src="../deepseek/down.gif" onclick="download('${content}')" title="点击下载语音" />`;
			}else {
				messageDiv.innerHTML = `<div class="user">📝 ${content}</div>`;
			}
			chatHistory.appendChild(messageDiv);

			// 滚动到底部
			chatHistory.scrollTop = chatHistory.scrollHeight;
			userTextarea.style.height = "auto";
		}

        // 自动调整输入框高度
        document.getElementById("userInput").addEventListener("input", (event) => {
            event.target.style.height = "auto";
            event.target.style.height = event.target.scrollHeight + "px";
        });
		
		function pauseAllAudio() {
			// 获取页面中的所有音频元素
			const audioElements = document.getElementsByTagName('audio');
			
			// 遍历每个音频元素，调用 pause 方法
			for (let audio of audioElements) {
				audio.pause(); // 暂停当前音频
			}
		}
		
		function download(url) {
			fetch(url)
				.then(response => response.blob())
				.then(blob => {
					const link = document.createElement('a');
					link.href = URL.createObjectURL(blob);
					var lastOf = url.lastIndexOf('/');
					var filename = url.substr(lastOf + 1); 
					link.download = filename;
					link.click();
				});
		}


        var docurl = document.URL;
		var ipurl = docurl.substring(0, docurl.lastIndexOf("/"));
		var id = window.__speekConfig.id;
        function returnurl() {
            if (confirm('是否离开当前活动页面？请先保存作品。') == true) {
                window.location.href = window.__speekConfig.fpage
            }
        }

        function savechat() { 
	        var preview = document.getElementById("chatHistory");
            var htmlcode = preview.innerHTML; //使用缩略图预览
        	if (messageHistory.length>0) {
                html2canvas(preview).then(pic => {					
        	        var urls = '../student/uploadtopic.ashx?id=' + id;
			        var title = "";
			        var Cover = blob(pic.toDataURL("image/jpg",0.5)); 
                    //var encodehtml = window.btoa(encodeURIComponent(htmlcode));
			        var Content = htmlcode;
			        var Extension = "speek";
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
        
		function example(){
			fetch("example.txt")
			　　.then((res) => res.text())
			　　.then(data => {
			　　	userTextarea.value = data;//文章样本
				})
		}
