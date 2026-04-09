const ctx = document.getElementById('myChart');
    var id = window.__mqttConfig.id;
	var user= window.__mqttConfig.snum; 
	var workdevice= window.__mqttConfig.workDevice; 
    var serverip=window.__mqttConfig.serverIp;

    function returnurl() {
        if (confirm('是否离开当前活动页面？请先保存作品。') == true) {
            window.location.href = window.__mqttConfig.fpage;
        }
    }
    
	var count=0;//信息数据

    function savework() {
        //var workcount=count-pubcount;
        if(count>0)
        {
            var title = "";
            var Content=$(".messages").html();
            var Cover = blob(ctx.toDataURL());
            var Extension = "mqtt";
            var urls = 'uploadmqtt.ashx?id=' + id;
            var formData = new FormData();
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
                //console.log(res)
            });
        }
        else{
            alert("请先连接控制板进行实验，发布主题后才能保存！");
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

    var clientId = 'stu'+ user+ Math.random().toString(16).substr(2, 6);//添加随机数，方便多开页面，mqtt支持不验证连接
	var pwsd='123456';
	var webip=window.location.host;
    if(serverip!=""){
        webip=serverip;
    }

	$('#txtIp').val(webip);
	$('#txtPort').val(1883);
	$('#txtId').val('m'+user);//设备客户端id与网页客户端id必须不一样
	$('#txtUser').val(user);
	$('#txtPwd').val(pwsd);
	
	
	var client;
    var host = 'ws://'+webip+':5000/mqtt';

    const options = {
        keepalive: 30,
        clientId: clientId,
	    username: user,
	    password: pwsd,
        protocolId: 'MQTT',
        protocolVersion: 4,
        clean: true
    }

	console.log('物联网实验室');	
	var isled=false;
	var countlist=[];
	var msglist=[];
	var islive=false;//是否连接
    var pubcount=0;//自己发布主题数量	
	var begintime = "";//连接时间
	var startime="";//采样时间
	var sampling="";//当前采样时间
	
	var machine=['led','fan','pump','temperature','humidity','sound','light','distance'];//配置设备列表
	var work=['led','fan','pump','temperature','humidity','sound','light','distance'];//使用设备列表，选择一个即可
	
    if(workdevice){
        work=workdevice.split(',');
		console.log(work);
    }
        
    function getWork(mea){
    	switch(mea){
			case 'distance':
				mea='dist';
				break;
			case 'temperature':
				mea='temp';
				break;
			case 'humidity':
				mea='humi';
				break;
		}
        return mea;
    }

	var topicset=getWork(work[0]) +'/'+user;//设置设备主题名称
	$('#txtTopic').val(topicset);

	machine.forEach(function(dev){
		var mach='#'+dev;
		$(mach).hide();
		var machnum=mach+'num';
		$(machnum).hide();		
	});
	
	work.forEach(function(dev){
		if(dev){
			var wk='#'+dev;
			console.log("工作设备",wk)
			$(wk).show();
			var wknum=wk+'num';
			$(wknum).show();	
		}		
	});
	
    
    $("#myChart").dblclick(function(){
	  const el = document.createElement('a');
	  el.href = ctx.toDataURL();
	  el.download = '传感器采样图表';
	  const event = new MouseEvent('click');
	  el.dispatchEvent(event);
	});
	
	$('input').on('focus', function() {
        // 在input获得焦点时，选择所有文本
        $(this).select();
    });
	
	$('#btnConnect').click(function () {		
		console.log('正在连接mqtt服务……');		
		client = mqtt.connect(host, options);
		$(this).attr('disabled',true);	
	
	$('.measure').click(function () {
		var mea="";
		switch(this.id){
			case 'distance':
				mea='dist';
				break;
			case 'temperature':
				mea='temp';
				break;
			case 'humidity':
				mea='humi';
				break;
			case 'sound':
				mea='sound';
				break;
			case 'light':
				mea='light';
				break;		
		}
		$('#txtTopic').val(mea+"/"+user);
    });

		
	$('.device').click(function () {
		$('#txtTopic').val(this.id+'/'+user);
		console.log(isled);
		if(isled){		
			$('#txtPayload').val("off");
			var icon=this.src.replace('gif','png');
			$(this).attr("src",icon);
			isled=false;
		}	
		else{
			$('#txtPayload').val("on");	
			var icon=this.src.replace('png','gif');
			$(this).attr("src",icon);
			isled=true;	
			cur	=this.id;		
		}
    });

	var cur='led';
	
	$('#btnSub').click(function () {
		var mytopic= $('#txtTopic').val();		
		console.log("订阅主题：",mytopic);		
        client.subscribe(mytopic, { qos: 0 });	
		addsubtopic(mytopic);
    });	
	
	function addsubtopic(topic){
		var exists = false; 
		$('.subtopics option').each(function(){
		  if (this.value == topic) {
			exists = true;
		  }
		});
		if(!exists){
			var op="<option>"+topic+"</option>";
			$('.subtopics').append(op);
			$('.linkuser').append("已订阅主题："+topic+"<br>");
			$('#btnUnSub').attr('disabled',false);
		}
	}
	
	$('#btnUnSub').click(function () {
		var mytopic= $('.subtopics').find("option:selected").text();
		if(mytopic){		
			console.log("取消订阅：",mytopic);		
			client.unsubscribe(mytopic, { qos: 0 })	
			$('.linkuser').append("已取消订阅："+mytopic+"<br>");	
			$(".subtopics option:selected").remove(); 
		}
		mytopic= $('.subtopics').find("option:selected").text();
		if(!mytopic){
			$(this).attr('disabled',true);
		}
    });	
	
	$('#btnPublish').click(function () {
        pubcount++;
		var mytopic= $('#txtTopic').val();		
		console.log("发布主题：",mytopic);
		
		var mypayload= $('#txtPayload').val();		
		console.log("负载消息：",mypayload);
		
		client.publish(mytopic, mypayload, { qos: 0, retain: false });//发布		
    });
	
	
    client.on('error', function (err) {
		$('.linkuser').append("连接Mqtt服务失败！");
        console.log(err);
        client.end();
    })

    client.on('connect', function () {
		var str= " MQTT服务已连接.<br>";
		islive=true;
        console.log(clientId+"已连接.");
		begintime= Date.now();
		$('.linkuser').append(str);
		$('#broker').attr("src","../code/mqtt/run.png");
        client.subscribe(topicset, { qos: 0 });//订阅默认Led主题
		addsubtopic(topicset);
        //client.publish(topic, 'wss secure connection 成功 demo...!', { qos: 0, retain: false })
		$('#btnPublish').attr('disabled',false);
		$('#btnSub').attr('disabled',false);
		$('#btnUnSub').attr('disabled',false);
		$('#btnConnect').text("已经连接");
		$(".device").css("filter",'grayscale(0)');
		$(".measure").css("filter",'grayscale(0)');
    })

    client.on('message', function (topic, message, packet) {
		const decoder = new TextDecoder();//字节转字符串
        //console.log(packet.topic,decoder.decode(packet.payload));
		receiveMessage(topic,message,packet);		
    })

    client.on('close', function () {
		if(islive){
			var str='MQTT服务已断开！';
			console.log(str);
			$('.linkuser').append(str+"<br>");
			$(".linkuser").scrollTop($(".linkuser").height()*count);
			$('#broker').attr("src","../code/mqtt/ready.png");
		}
		islive=false;
    })
	
	receiveMessage = function (topic, message, type) {
		if(count==0){
			startime=Date.now();//采样开始时间
		}
		count++;
		sampling=Date.now();
		
		const decoder = new TextDecoder();//字节转字符串		
		message=decoder.decode(message);//字节转字符串
		
		if(isNum(message)){
			countlist.push(count);
			msglist.push(message);//记录消息
			notice(message);//是否发警报声
		}				
		//console.log('计数：',countlist);
		//console.log('信息：',msglist);
		updateChartData();//更新图表
		//var d = new Date();//d.toLocaleString()
		//adding new message
		$(".messages").append(count+'. 主题： ' + topic + '  消息： ' + message + '<hr>');
		//autoscrolling to the bottom
		$(".messages").scrollTop($(".messages").height()*count);
		
		if(topic.includes('led')){
			//console.log(topic,message);
			switch(message){
				case 'off':
					$('#led').attr("src","../code/mqtt/led.png");
					break;
				case 'on':
					$('#led').attr("src","../code/mqtt/led.gif");
					break;
			}			
		}
		if(topic.includes('fan')){
			//console.log(topic,message);
			switch(message){
				case 'off':
					$('#fan').attr("src","../code/mqtt/fan.png");
					break;
				case 'on':
					$('#fan').attr("src","../code/mqtt/fan.gif");
					break;
			}			
		}
		
		if(topic.includes('pump')){
			//console.log(topic,message);
			switch(message){
				case 'off':
					$('#pump').attr("src","../code/mqtt/pump.png");
					break;
				case 'on':
					$('#pump').attr("src","../code/mqtt/pump.gif");
					break;
			}			
		}
		
		if(topic.includes('dist')){
			$('#distancenum').text(message);
		}
		if(topic.includes('light')){
			$('#lightnum').text(message);
		}
		if(topic.includes('sound')){
			$('#soundnum').text(message);
		}
		if(topic.includes('temp')){
			$('#temperaturenum').text(message);
		}
		if(topic.includes('humi')){
			$('#humiditynum').text(message);
		}
		
	}	
	
    });
	function isNum(val) {
	  return !isNaN(val)
	}	
	
	function notice(message) {
		var currentvalue=parseInt(message);
		var setalert= parseInt($('.alert').val());	
		if(currentvalue<setalert){
			var audio = document.getElementById("audio");
			audio.src = '../code/mqtt/help.mp3';
			var playPromise = audio.play();

			if (playPromise) {
				playPromise.then(() => {
					// 音频加载成功
					// 音频的播放需要耗时
					setTimeout(() => {
						// 后续操作
						//console.log("play.");
					}, audio.duration * 1000); // audio.duration 为音频的时长单位为秒

				}).catch((e) => {
					// 音频加载失败
				});
			}
		}		
	}

// 创建图表实例 line bar 
    var myChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: countlist, // X轴标签
            datasets: [{
                label: '传感器数据采样图表', // 数据集名称
                data: msglist, // 初始数据
                backgroundColor: 'rgba(68, 187, 109, 0.6)', // 数据集颜色
                borderColor: 'rgba(68, 187, 109, 0.8)', // 数据集边框颜色
                borderWidth: 1,
                pointRadius: 1 // 设置点的半径为2
            }]
        },
        options: {
            title: {
                display: true,
                text: "Custom Chart Title"
            },
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
    // 更新图表数据的函数
    function updateChartData() {
        // 假设我们获取了新的数据
        var labels = countlist;
        var newData = msglist;
        var se = parseInt((sampling - startime) / 1000);
        var mu = parseInt(se / 60);
        var sd = se - mu * 60;
        //console.log('采样持续时间：',mu,'秒');
        $("#msg").html('采样持续时间：' + mu + '分' + sd + '秒');
        // 使用Chart.js的update方法更新数据
        myChart.data.labels = labels;
        myChart.data.datasets.data = newData;
        //myChart.data.datasets.forEach(function (dataset) {
        //    dataset.data = newData;
        //});

        // 重绘图表
        myChart.update();

        return "已更新"
    }
