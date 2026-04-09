var editor = ace.edit("editor");
      editor.setOptions({
          wrap: true,
          enableLiveAutocompletion: true,
          enableSnippets: true
      });
      editor.setTheme("ace/theme/textmate");
      editor.getSession().setMode("ace/mode/python");
      editor.setFontSize(24);
      editor.setReadOnly(false)
      editor.getSession().setTabSize(4);

var id = window.__questioneditConfig.id;
    var mid=window.__questioneditConfig.mid;
    var key = "turtle2022"+ id;    
    document.getElementById("BtnSave").setAttribute("disabled", true);    
    var boxtitle = document.getElementById("TextBoxTitle");
    var mypre = document.getElementById("output");

    function outf(text) {
        var outid = "output" ;
        mypre.innerHTML = mypre.innerHTML + text;
    }
    function builtinRead(x) {
        if (Sk.builtinFiles === undefined || Sk.builtinFiles["files"][x] === undefined)
            throw "File not found: '" + x + "'";
        return Sk.builtinFiles["files"][x];
    }

    function myfun() {
        return new Promise(function (resolve, reject) {
            temp = mypre.innerText;
            temp = temp + args;
            mypre.innerHTML = temp + "\n";
        })
    }

    function passcheck(){
		savemsg.innerHTML= "";
	    runit();		
		document.getElementById("BtnSave").removeAttribute("disabled");
    }

    function clear() {
        mypre.innerHTML ="";
    }
    function runit() {
        var prog = editor.getValue();
        sessionStorage.setItem(key, prog);
        clear();
        Sk.pre = "output";
        Sk.configure({ output: outf, read: builtinRead, execLimit: 30000, __future__: Sk.python3, inputfun: myfun });

        (Sk.TurtleGraphics || (Sk.TurtleGraphics = {})).target = 'cv';
        var myPromise = Sk.misceval.asyncToPromise(function () {
            return Sk.importMainWithBody("<stdin>", false, prog, true);
        });

        myPromise.then(function (mod) {
            console.log('succeed!');
        },
    function (err) {
        var msg = err.toString();
        console.log(msg);
    });
}

function codeshow() {
    var value = sessionStorage.getItem(key);   
	if(value!=null)  editor.setValue(value);
}

function saveshow(){
    var scode=window.__questioneditConfig.code;
    if(scode!=""){
        scode=decodeURIComponent(window.atob(scode));
        editor.setValue(scode);        
    }
}

codeshow();
saveshow();

function toHex(r,g,b) {
	return ("00000" + (r << 16 | g << 8 | b).toString(16)).slice(-6);
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

function save(){
    var codevalue= editor.getValue();  
	if(codevalue!=null&&codevalue!=""){	
        var Cover="";
		var wi=0;
		var he=0;
		var mi_mxl=0;
		var mi_mxr=0;
		var resimg="";
		var res=document.querySelector("#result");
		var obj=$("#cv").find("canvas")[1];
		
		
		
		
		console.log('裁剪开始');		
		
		var opts = {
	      backgroundColor: "transparent",
	      
	      
	    };
		console.log(obj);
		if (obj!=null){			
			html2canvas(obj,opts ).then(pic => {
			   
			   var dataURL=pic.toDataURL();
			

			   if(dataURL=="data:,"){
					dataURL="data:image/gif;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVQImWNgYGBgAAAABQABh6FO1AAAAABJRU5ErkJggg==";
			   }

			   var img = new Image();  
				img.src = dataURL;  
				img.onload = function(){
					var c = document.createElement('canvas');
					var ctx = c.getContext('2d');
					c.width = img.width;
					c.height = img.height;
					ctx.drawImage(img,0,0);
					var imgData = ctx.getImageData(0, 0, c.width, c.height).data;
					var lOffset = c.width, rOffset = 0,tOffset = c.height, bOffset = 0;
					for (var i = 0; i < c.width; i++) {
						for (var j = 0; j < c.height; j++) {
							var pos = (i + c.width * j) * 4
							if (imgData[pos] == 255 || imgData[pos + 1] == 255 || imgData[pos + 2] == 255 || imgData[pos + 3] == 255) {
								bOffset = Math.max(j, bOffset); 
								rOffset = Math.max(i, rOffset); 
								tOffset = Math.min(j, tOffset); 
								lOffset = Math.min(i, lOffset); 
							}
						}
					}
										
					var x = document.createElement("canvas");
					x.width = rOffset-lOffset;
					x.height = bOffset-tOffset;
					var xx = x.getContext("2d");
					xx.drawImage(img, lOffset, tOffset, x.width, x.height, 0, 0, x.width, x.height);
					
					
					var newimgData = xx.getImageData(0, 0, x.width, x.height).data;
					var arrsetl=new Array();
					var arrsetr=new Array();
					
					var xwidth = x.width;
					var xheght = x.height;
					var lcount=0;
					var rcount=0;
					var colors=new Array();
					var alphas=new Array();
					
					for (var m=0;m<xwidth;m++){
						for (var n=0;n<xheght;n++){
							var newpos = (m + xwidth * n) * 4;
							var np0= newimgData[newpos];
							var np1= newimgData[newpos+1];
							var np2= newimgData[newpos+2];
							var np3= newimgData[newpos+3];	
							
							if (np0 >0 || np1 >0  || np2 >0  || np3 >0 ) {	
								var npall=toHex(np0,np1,np2);
								if(m==n){
									arrsetl.push(m);
									lcount++;
									colors.push(npall);
								}
								if(m==xwidth-n){
									arrsetr.push(m);
									rcount++;
									colors.push(npall);
								}
								if(n==xheght-1){								
									colors.push(npall);
								}
								var half=parseInt(xwidth/2);
								if(m==half){
									colors.push(npall);
									
									alphas.push(np3);
								}
							}
						}					
					}	

					let setcolors=new Set(colors);
					let newcolors=Array.from(setcolors);
                    newcolors=colorcomp(newcolors);
					let colorsize=newcolors.length;
					
					console.log("颜色：",newcolors);
					console.log("透明度：",alphas);
					
					
					
					var minNl = Math.min.apply(null,arrsetl);	
					var maxNl = Math.max.apply(null,arrsetl);	
					mi_mxl=(maxNl-minNl)*Math.sqrt(2).toFixed(0);
					
					
					var minNr = Math.min.apply(null,arrsetr);	
					var maxNr = Math.max.apply(null,arrsetr);	
					mi_mxr=(maxNr-minNr)*Math.sqrt(2).toFixed(0);
					var milr=mi_mxl+"x"+mi_mxr+"x"+lcount+"x"+rcount+"x"+colorsize;					
					
					resimg=x.width+"x"+x.height+"x"+milr;				

					console.log("自动裁剪");
					dataURL = x.toDataURL();
                    
			        Cover=blob(dataURL);
					
					console.log("绘图尺寸：",resimg);					
					upload(codevalue,resimg,Cover);
					
				}			

			});	
		}
		else{
			console.log("没有绘图");
			upload(codevalue,resimg,Cover);
		}

	}
}

function colorcomp(colors){
	var n=colors.length;

	for(i=0;i<n;i++){
		for(j=i+1;j<n;j++){	
			var fcolor=colors[i];		
			var ncolor=colors[j];
			
			//console.log(ncolor);
			var fr=fcolor.substring(0,2)
			var fg=fcolor.substring(2,4)
			var fb=fcolor.substring(4,6)
			//console.log("rgb:",fr,fg,fb);
			var nr=ncolor.substring(0,2)
			var ng=ncolor.substring(2,4)
			var nb=ncolor.substring(4,6)
			//console.log("rgb:",nr,ng,nb);

			var absR=hex2int(fr)-hex2int(nr);
			var absG=hex2int(fg)-hex2int(ng);
			var absB=hex2int(fb)-hex2int(nb);
			//console.log("abs:",absR,absG,absB);
			var dis=Math.sqrt(absR*absR+absG*absG+absB*absB);
			//console.log(fcolor,"-",j,"-",ncolor," dis:",dis);
			if(dis<72){
				colors.splice(j,1);
				n--;
				j--;
			}

		}		
	}
	console.log("颜色：",colors);
	return  colors;
}

function hex2int(hex) {
    var len = hex.length, a = new Array(len), code;
    for (var i = 0; i < len; i++) {
        code = hex.charCodeAt(i);
        if (48<=code && code < 58) {
            code -= 48;
        } else {
            code = (code & 0xdf) - 65 + 10;
        }
        a[i] = code;
    }
    
    return a.reduce(function(acc, c) {
        acc = 16 * acc + c;
        return acc;
    }, 0);
}

function upload(codevalue,resimg,Cover){
    var rest=mypre.innerHTML;
	var title=boxtitle.value;
	var urls = 'questionsave.ashx?id=' + id;
	var formData = new FormData();
	formData.append('cover', Cover);
	formData.append('code', window.btoa(encodeURIComponent(codevalue)));
	formData.append('resimg', resimg);
	formData.append('id', id);
	formData.append('mid', mid);
	formData.append('title', title);
	formData.append('rest', rest);

	$.ajax({
		url: urls,
		type: 'POST',
		cache: false,
		data: formData,
		processData: false,
		contentType: false
	}).done(function (res) {
		console.log(res);
		if(res>0){ 	
			savemsg.innerHTML= "保存成功！";
		}
		if(res==0) 	savemsg.innerHTML= "保存失败！";
		if(res<0) 	savemsg.innerHTML= "请先登录！";
	}).fail(function (res) {
		savemsg.innerHTML= "保存异常！";
		console.log(res)
	}); 
}

function back(){
    if (confirm('是否离开当前活动页面？请先保存作品。') == true) {
        window.location.href=window.__questioneditConfig.fpage;
    }
}
