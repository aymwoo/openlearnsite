const startBtn = document.getElementById('startBtn');
        const stopBtn = document.getElementById('stopBtn');
        const status = document.getElementById('status');
        const waveformCanvas = document.getElementById('waveform');
        const spectrumCanvas = document.getElementById('spectrum');
        const mfccCanvas = document.getElementById('mfcc'); // 新增 MFCC Canvas
        const audioPlayer = document.getElementById('audioPlayer');
        const recordingsList = document.getElementById('recordings');

        let audioContext, analyser, mediaRecorder, chunks = [];
        let isRecording = false;
        let isPlaying = false;
        let recordings = [];
        let mfccFeatures = []; // 存储 MFCC 特征
        let featureBuffer = []; // 存储完整特征序列
        let previousMFCC = null;
        let previousDelta = null;

        // 初始化分析器
        function initAnalyser(source) {
            analyser = audioContext.createAnalyser();
            analyser.fftSize = 2048;

            const gainNode = audioContext.createGain();
            gainNode.gain.value = 0; // 静音处理

            source.connect(analyser);
            analyser.connect(gainNode);
            gainNode.connect(audioContext.destination);
        }

        // 绘制波形
        function drawWaveform() {
            const ctx = waveformCanvas.getContext('2d');
            ctx.clearRect(0, 0, waveformCanvas.width, waveformCanvas.height);

            if (analyser) {
                const bufferLength = analyser.fftSize;
                const dataArray = new Uint8Array(bufferLength);
                analyser.getByteTimeDomainData(dataArray);

                ctx.beginPath();
                ctx.lineWidth = 2;
                ctx.strokeStyle = '#0f0';

                const sliceWidth = waveformCanvas.width * 1.0 / bufferLength;
                let x = 0;

                for (let i = 0; i < bufferLength; i++) {
                    const v = dataArray[i] / 128.0;
                    const y = v * waveformCanvas.height / 2;

                    if (i === 0) ctx.moveTo(x, y);
                    else ctx.lineTo(x, y);

                    x += sliceWidth;
                }

                ctx.stroke();
            }

            if (isRecording || isPlaying) {
                requestAnimationFrame(drawWaveform);
            }
        }

        // 绘制频谱
        function drawSpectrum() {
            const ctx = spectrumCanvas.getContext('2d');
            ctx.clearRect(0, 0, spectrumCanvas.width, spectrumCanvas.height);

            if (analyser) {
                const bufferLength = analyser.frequencyBinCount;
                const dataArray = new Uint8Array(bufferLength);
                analyser.getByteFrequencyData(dataArray);

                const barWidth = (spectrumCanvas.width / bufferLength) * 2.5;
                let x = 0;

                for (let i = 0; i < bufferLength; i++) {
                    const barHeight = dataArray[i];
                    ctx.fillStyle = `hsl(${i * 2}, 100%, 50%)`;
                    ctx.fillRect(x, spectrumCanvas.height - barHeight, barWidth, barHeight);
                    x += barWidth + 1;
                }
				
				//console.log("频谱",dataArray);
				// 将频域数据转换为线性幅值
				const spectrum = Array.from(dataArray).map(value => Math.pow(10, value / 20));
				// 计算 MFCC 特征
				mfccFeatures = calculateMFCC(spectrum, audioContext.sampleRate);
				//console.log("计算 MFCC 特征",mfccFeatures);
				
				// 计算 MFCC 特征
				//const mfccold = calculateMFCCold(spectrum, audioContext.sampleRate);
				//console.log("计算 MFCC old 特征",mfccold);
			
				drawMFCC();
            }

            if (isRecording || isPlaying) {
                requestAnimationFrame(drawSpectrum);
            }
        }

        // 计算 MFCC 特征
        function calculateMFCC(spectrum, sampleRate) {
            const numFilters = 26; // 梅尔滤波器数量
            const numCoefficients = 13; // MFCC 系数数量
            const mfcc = [];

            // 计算梅尔滤波器组
            const melFilters = createMelFilterBank(numFilters, spectrum.length, sampleRate);

            // 应用梅尔滤波器组
            for (let i = 0; i < numFilters; i++) {
                let sum = 0;
                for (let j = 0; j < spectrum.length; j++) {
                    sum += spectrum[j] * melFilters[i][j];
                }
                mfcc.push(Math.log(sum + 1e-6)); // 避免对零取对数
            }

            const logMelEnergies = mfcc;
			const dctCoefficients = dct(logMelEnergies);
			return dctCoefficients.slice(0, numCoefficients);
        }
		
		function dct(logMelEnergies) {
			const N = logMelEnergies.length;
			const coefficients = [];
			for (let k = 0; k < N; k++) {
				let sum = 0;
				for (let n = 0; n < N; n++) {
					sum += logMelEnergies[n] * Math.cos(Math.PI * k / N * (n + 0.5));
				}
				coefficients.push(sum);
			}
			return coefficients;
		}


        // 修正后的梅尔滤波器组生成
        function createMelFilterBank(numFilters, fftSize, sampleRate) {
            const melFilters = [];
            const lowMel = 0;
            const highMel = 2595 * Math.log10(1 + (sampleRate / 2) / 700);

            // 生成梅尔刻度上的点
            const melPoints = [];
            for (let i = 0; i <= numFilters + 1; i++) {
                melPoints.push(lowMel + (i / (numFilters + 1)) * (highMel - lowMel));
            }

            // 创建每个滤波器
            for (let i = 0; i < numFilters; i++) {
                const filter = new Array(fftSize).fill(0);
                // 转换为Hz
                const leftFreq = 700 * (Math.pow(10, melPoints[i] / 2595) - 1);
                const centerFreq = 700 * (Math.pow(10, melPoints[i+1] / 2595) - 1);
                const rightFreq = 700 * (Math.pow(10, melPoints[i+2] / 2595) - 1);

                // 转换为频点
                const leftBin = Math.floor((fftSize) * leftFreq / sampleRate);
                const centerBin = Math.floor((fftSize) * centerFreq / sampleRate);
                const rightBin = Math.floor((fftSize) * rightFreq / sampleRate);

                // 创建三角滤波器
				for (let j = leftBin; j <= centerBin; j++) {
					filter[j] = (j - leftBin) / (centerBin - leftBin);
				}
				for (let j = centerBin + 1; j <= rightBin; j++) {
					filter[j] = 1 - (j - centerBin) / (rightBin - centerBin);
				}

                melFilters.push(filter);
            }
            return melFilters;
        }

        // 绘制 MFCC 特征
        function drawMFCC() {
            const ctx = mfccCanvas.getContext('2d');
            ctx.clearRect(0, 0, mfccCanvas.width, mfccCanvas.height);

            if (mfccFeatures.length > 0) {
                const numCoefficients = mfccFeatures.length;
                const barWidth = mfccCanvas.width / numCoefficients;
                const maxHeight = mfccCanvas.height;

                for (let i = 0; i < numCoefficients; i++) {
                    const value = mfccFeatures[i];
                    const barHeight = (value + 10) * (maxHeight / 50); // 归一化到画布高度
                    ctx.fillStyle = `hsl(${i * 10}, 100%, 50%)`;
                    ctx.fillRect(i * barWidth, maxHeight - barHeight, barWidth, barHeight);
                }
            }

            if (isRecording || isPlaying) {
                requestAnimationFrame(drawMFCC);
            }
        }

        // 提取 MFCC 特征
        function extractMFCC() {
            if (!analyser) return;

            const bufferLength = analyser.frequencyBinCount;
            const dataArray = new Float32Array(bufferLength);
            analyser.getFloatFrequencyData(dataArray); // 获取频域数据

            // 将频域数据转换为线性幅值
            const spectrum = Array.from(dataArray).map(value => Math.pow(10, value / 20));

            // 计算 MFCC 特征
            const mfcc = calculateMFCC(spectrum, audioContext.sampleRate);
            console.log("计算 MFCC 特征",mfcc);
            mfccFeatures = mfcc;
            drawMFCC();
        }



        let recordnum = 0;

        // 添加录音到列表
        function addRecording(blob, duration) {
            recordnum++;
            const recording = {
                id: Date.now(),
                name: recordnum,
                blob: blob,
                duration: duration,
                timestamp: new Date().toLocaleString(),
                waveform: waveformCanvas.toDataURL(),
                spectrum: spectrumCanvas.toDataURL(),
                mfcc: mfccCanvas.toDataURL(), // 保存 MFCC 图像
                mfccFeature: mfccFeatures,                
                fullFeatures: featureBuffer // 存储完整特征序列
            };

            recordings.push(recording);
            //console.log(recording);
            renderRecordings();
            updateRecordingSelector();
            featureBuffer = []; // 清空特征缓存
        }

        // 渲染录音列表
        function renderRecordings() {
            recordingsList.innerHTML = recordings.map(rec => `
                <div class="recording-item">
                    <div class="recording-title">
                        <h4 contenteditable="true">录音 ${rec.name}</h4>
                        <div>${rec.timestamp}</div>
                        <div>${rec.duration.toFixed(2)}秒</div>
                    </div>
                    <div class="recording-img">
                        <img src="${rec.waveform}" title="波形图：振幅为响度"/>
                        <img src="${rec.spectrum}"  title="频谱图：振幅为频率"/>
                        <img src="${rec.mfcc}"  title="倒谱图：音色"/> <!-- 显示 MFCC 图像 -->
                    </div>
                    <div  class="recording-tool">
						<button onclick="playRecording(${rec.id})" class="recording-action recording-action--play" type="button"><i class="fa fa-play" aria-hidden="true"></i><span>播放</span></button>
						<button class="recording-action recording-action--delete" onclick="deleteRecording(${rec.id})" type="button"><i class="fa fa-trash" aria-hidden="true"></i><span>删除</span></button>
                    </div>
                </div>
            `).join('');
        }

        // 播放录音
        window.playRecording = function (id) {
            const recording = recordings.find(rec => rec.id === id);
            if (recording) {
                const url = URL.createObjectURL(recording.blob);
                audioPlayer.src = url;
                audioPlayer.hidden = true;
                audioPlayer.play();
            }
        };

        // 删除录音
        window.deleteRecording = function (id) {
            recordings = recordings.filter(rec => rec.id !== id);
            renderRecordings();
        };

        // 开始录音
        startBtn.addEventListener('click', async () => {
            try {
                const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
                audioContext = new (window.AudioContext || window.webkitAudioContext)();
                mediaRecorder = new MediaRecorder(stream);
                chunks = [];

                const startTime = Date.now();

                mediaRecorder.ondataavailable = e => chunks.push(e.data);
                mediaRecorder.onstop = async () => {
                    //extractMFCC(); // 实时提取 MFCC
                    const duration = (Date.now() - startTime) / 1000;
                    const blob = new Blob(chunks, { type: 'audio/webm' });
                    addRecording(blob, duration);//添加录音到列表
                    status.textContent = "录音已保存";
                };

                initAnalyser(audioContext.createMediaStreamSource(stream));
                mediaRecorder.start();
                isRecording = true;

                startBtn.disabled = true;
                stopBtn.disabled = false;
                status.textContent = "录音中...";
                drawWaveform();
                drawSpectrum();
                
                // 实时采集特征
                const featureInterval = setInterval(() => {
                    if (isRecording) {
                        const features = extractEnhancedMFCC();
                        featureBuffer.push(features);
                    } else {
                        clearInterval(featureInterval);
                    }
                }, 100); // 每100ms采集一次

            } catch (err) {
                status.textContent = "错误:没有找到麦克风，请确认并启用https访问！ ";
            }
        });


        // 修正后的增强特征提取
        function extractEnhancedMFCC() {
            if (!analyser) return;

            const bufferLength = analyser.frequencyBinCount;
            const dataArray = new Float32Array(bufferLength);
            analyser.getFloatFrequencyData(dataArray);
            
            // 转换为线性幅值
            const spectrum = Array.from(dataArray).map(value => Math.pow(10, value / 20));
            
            // 计算当前MFCC
            const currentMFCC = calculateMFCC(spectrum, audioContext.sampleRate);
            
            // 计算差分特征
            let delta = [];
            let deltaDelta = [];
            if (previousMFCC) {
                delta = currentMFCC.map((val, i) => val - previousMFCC[i]);
                if (previousDelta) {
                    deltaDelta = delta.map((val, i) => val - previousDelta[i]);
                }
            }
            
            // 组合特征向量
            const featureVector = [...currentMFCC, ...delta, ...deltaDelta];
            
            // 更新历史数据
            previousDelta = delta;
            previousMFCC = currentMFCC;
            
            return featureVector;
        }
        // 修正停止录音时的资源释放
        stopBtn.addEventListener('click', () => {
            mediaRecorder.stop();
            isRecording = false;
            
            // 关闭媒体流
            mediaRecorder.stream.getTracks().forEach(track => track.stop());
            
            startBtn.disabled = false;
            stopBtn.disabled = true;
            status.textContent = "保存中...";
        });

        // 播放控制
        audioPlayer.addEventListener('play', async () => {
            if (!audioPlayer.src) return;

            try {
                if (audioContext) audioContext.close();
                audioContext = new (window.AudioContext || window.webkitAudioContext)();
                isPlaying = true;
            } catch (err) {
                console.error('播放分析错误:', err);
            }
        });

        audioPlayer.addEventListener('pause', () => {
            isPlaying = false;
        });

        audioPlayer.addEventListener('ended', () => {
            isPlaying = false;
            URL.revokeObjectURL(audioPlayer.src);
        });



        // 修正选择器更新逻辑
        function updateRecordingSelector() {
            const selector1 = document.getElementById('recording1');
            const selector2 = document.getElementById('recording2');

            selector1.innerHTML = '<option value="">选择第一个录音</option>';
            selector2.innerHTML = '<option value="">选择第二个录音</option>';

            recordings.forEach(rec => {
                const optionText = `录音${rec.name} (${rec.duration.toFixed(1)}秒)`;
                
                const option1 = new Option(optionText, rec.id);
                const option2 = new Option(optionText, rec.id);
                
                selector1.add(option1);
                selector2.add(option2);
            });
        }


// 欧氏距离计算
window.euclideanDistance = function(vecA, vecB) {
    return Math.sqrt(
        vecA.reduce((sum, val, i) => sum + Math.pow(val - vecB[i], 2), 0)
    );
};

// 增强版比较函数
window.compareRecordings = function() {
    const resultDiv = document.getElementById('result');
    const id1 = document.getElementById('recording1').value;
    const id2 = document.getElementById('recording2').value;

    // 验证选择
    if (!id1 || !id2) {
        resultDiv.innerHTML = '<div class="dissimilar">请选择两个录音文件</div>';
        return;
    }

    // 获取录音数据
    const rec1 = recordings.find(r => r.id == id1);
    const rec2 = recordings.find(r => r.id == id2);

    // 特征校验
    if (!rec1?.mfccFeature?.length || !rec2?.mfccFeature?.length) {
        resultDiv.innerHTML = '<div class="dissimilar">特征数据不完整</div>';
        return;
    }

    try {
        // 执行DTW比较
        const rawDistance = euclideanDistance(
            rec1.mfccFeature, 
            rec2.mfccFeature
        );
        console.log("欧氏距离计算",rawDistance);
        // 生成可视化结果
        const similarity = (1 - rawDistance/100) * 100;
		
        renderComparisonResult(similarity, rec1, rec2);
        
    } catch (error) {
        console.error('比较出错:', error);
        resultDiv.innerHTML = '<div class="dissimilar">比较过程发生错误</div>';
    }
};

// 结果可视化渲染
function renderComparisonResult(similarity, rec1, rec2) {
    const resultDiv = document.getElementById('result');
	
	if(similarity < 0){
		similarity = 0;
	}
    const similarityText = similarity.toFixed(1) + '%';
    
    let resultClass = 'dissimilar';
    if (similarity > 75) resultClass = 'similar';
    else if (similarity > 50) resultClass = 'medium';
	if(similarity>0){
		resultDiv.innerHTML = `
			<div class="${resultClass}">
				<div class="similarity-meter">
					<div class="bar" style="text-align:center; width: ${similarity}%">${similarityText}相似度</div>
				</div>
			</div>
		`;
	}
	else{
		resultDiv.innerHTML = `
			<div class="${resultClass}">
				<div class="similarity-meter">
					<span>${similarityText}相似度</span>
				</div>
			</div>
		`;	
	}

}

var docurl = document.URL;
var ipurl = docurl.substring(0, docurl.lastIndexOf("/"));
var id = window.__soundlabConfig.id;
function returnurl() {
    if (confirm('是否离开当前活动页面？请先保存作品。') == true) {
        window.location.href = window.__soundlabConfig.fpage
    }
}

function savechat() { 
	var preview = document.getElementById("recordhistory");
    var htmlcode ="";// preview.innerHTML;使用缩略图预览
    if (recordings.length>0) {
        html2canvas(preview).then(pic => {					
        	var urls = '../student/uploadtopic.ashx?id=' + id;
			var title = "";
			var Cover = blob(pic.toDataURL("image/jpg",0.5)); 
			var Content = htmlcode;
			var Extension = "sound";
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
