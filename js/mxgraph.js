// Parses URL parameters. Supported parameters are:
	    // - lang=xy: Specifies the language of the user interface.
	    // - touch=1: Enables a touch-style user interface.
	    // - storage=local: Enables HTML5 local storage.
	    // - chrome=0: Chromeless mode.
	    var urlParams = (function (url) {
	        var result = new Object();
	        var idx = url.lastIndexOf('?');

	        if (idx > 0) {
	            var params = url.substring(idx + 1).split('&');

	            for (var i = 0; i < params.length; i++) {
	                idx = params[i].indexOf('=');

	                if (idx > 0) {
	                    result[params[i].substring(0, idx)] = params[i].substring(idx + 1);
	                }
	            }
	        }

	        return result;
	    })(window.location.href);

	    // Default resources are included in grapheditor resources
	    mxLoadResources = false;

var editor;

	    mxResources.loadDefaultBundle = false;
	    var bundle = mxResources.getDefaultBundle(RESOURCE_BASE, mxLanguage) ||
				mxResources.getSpecialBundle(RESOURCE_BASE, mxLanguage);

	    // Fixes possible asynchronous requests
	    mxUtils.getAll([bundle, STYLE_PATH + '/default.xml'], function (xhr) {
	        // Adds bundle text to resources
	        mxResources.parse(xhr[0].getText());

	        // Configures the default graph theme
	        var themes = new Object();
	        themes[Graph.prototype.defaultThemeName] = xhr[1].getDocumentElement();

	        // Main
	        editor = new Editor(urlParams['chrome'] == '0', themes);
	        new EditorUi(editor);
	        readfromnet();
	    }, function () {
	        document.body.innerHTML = '<center style="margin-top:10%;">Error loading resource files. Please check browser console.</center>';
	    });
	    //})();

	    var snum = window.__mxgraphConfig.snum_Id;
	    var savekey = "wzsxgraph" + snum;
	    var savemsg = document.getElementById("savemsg");

	    function savetoxml() {
	        $(".graph-toolbar__btn").eq(0).prop("disabled", true);
	        var format = "png";
	        var bg = '#ffffff';
	        var scale = 1;
	        var b = 1;

	        var graph = editor.graph;
	        var xml = mxUtils.getXml(new mxCodec().encode(graph.getModel()));
	        console.log(xml);
	        console.log("保存xml成功")
	        if (xml.indexOf("mxGeometry") != -1) {
	            console.log("有内容");
	            sessionStorage.setItem(savekey, xml); //sessionStorage  localStorage
	            //var filename='mxgraph'+ parseInt(Math.random()*100)+'.xml';
	            //downFile(xml,filename)

	            // New image export
	            var imgExport = new mxImageExport();
	            var bounds = graph.getGraphBounds();
	            var vs = graph.view.scale;
	            var xmlDoc = mxUtils.createXmlDocument();
	            var root = xmlDoc.createElement('output');
	            xmlDoc.appendChild(root);
	            // Renders graph. Offset will be multiplied with state's scale when painting state.
	            var xmlCanvas = new mxXmlCanvas2D(root);
	            xmlCanvas.translate(Math.floor((b / scale - bounds.x) / vs), Math.floor((b / scale - bounds.y) / vs));
	            xmlCanvas.scale(scale / vs);
	            imgExport.drawState(graph.getView().getState(graph.model.root), xmlCanvas);
	            // Puts request data together
	            var w = Math.ceil(bounds.width * scale / vs + 2 * b);
	            var h = Math.ceil(bounds.height * scale / vs + 2 * b);
	            var exml = mxUtils.getXml(root);

	            if (bg != null) {
	                bg = '&bg=' + bg;
	            }

	            var id = "' + window.__mxgraphConfig.id + '";
	            var urls = 'uploadgraph.ashx?id=' + id;
	            var formData = new FormData();
                xml=encodeURIComponent(xml);
                exml=encodeURIComponent(exml);//编码

	            formData.append('xml', xml);
	            formData.append('exml', exml);
	            formData.append('w', w);
	            formData.append('h', h);
	            formData.append('bg', bg);

	            $.ajax({
	                url: urls,
	                type: 'POST',
	                cache: false,
	                data: formData,
	                processData: false,
	                contentType: false
	            }).done(function (res) {
	                alert("保存成功！");
	                $(".graph-toolbar__btn").eq(0).prop("disabled", false);
	                console.log(res)
	            }).fail(function (res) {
	                alert("保存失败！");
	                $(".graph-toolbar__btn").eq(0).prop("disabled", false);
	                console.log(res)
	            });
	        } else {
	            console.log("无内容");
	        }
	    }

	    function readfromxml() {
	        var valuexml = sessionStorage.getItem(savekey);
	        if (valuexml != null) {
	            //console.log(valuexml);
	            var doc = mxUtils.parseXml(valuexml);
	            var codec = new mxCodec(doc);
	            var root = doc.documentElement;
	            var graph = editor.graph;
	            codec.decode(root, graph.getModel());
	            console.log("读取xml成功")
	        }
	    }
	    function readfromnet() {
	        var sessionxml = sessionStorage.getItem(savekey);
	        console.log("本地存储：");
	        //console.log(sessionxml);
	        var codefile = "' + window.__mxgraphConfig.codefile + '";
	        //console.log(codefile);
	        codefile = decodeURIComponent(codefile);
	        codefile = decodeURIComponent(codefile);//二次加密，所以这里要二次解密
	        console.log("读取作品：");
	        //console.log(codefile);
	        var viewxml;
	        if (codefile != null) viewxml = codefile;
	        if (sessionxml != null) viewxml = sessionxml;

	        if (viewxml != null) {
	            var doc = mxUtils.parseXml(viewxml);
	            var codec = new mxCodec(doc);
	            var root = doc.documentElement;
	            var graph = editor.graph;
	            codec.decode(root, graph.getModel());
	        }
	    }
	    function returnurl() {
	        window.location.href = "' + window.__mxgraphConfig.fpage + '";
	    }

	    function downFile(content, filename) {
	        var ele = document.createElement('a'); // 创建下载链接
	        ele.download = filename; //设置下载的名称
	        ele.style.display = 'none'; // 隐藏的可下载链接
	        // 字符内容转变成blob地址
	        var blob = new Blob([content]);
	        ele.href = URL.createObjectURL(blob);
	        // 绑定点击时间
	        document.body.appendChild(ele);
	        ele.click();
	        // 然后移除
	        document.body.removeChild(ele);
	    };

	    function showcontent() {
	        $("#mcontext").slideToggle();
	    }
