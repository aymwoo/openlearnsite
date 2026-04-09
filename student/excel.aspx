<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" EnableViewStateMac="false"  CodeFile="excel.aspx.cs" Inherits="student_excel" ResponseEncoding="utf-8" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8">
    <title></title>
    <link rel='stylesheet' href='../../Plugins/luckysheet/static/pluginsCss.css'/>
    <link rel='stylesheet' href='../../Plugins/luckysheet/static/plugins.css'/>
    <link rel='stylesheet' href='../../Plugins/luckysheet/static/luckysheet.css'/>
    <link rel='stylesheet' href='../../Plugins/luckysheet/static/iconfont.css'/>
    <script type="text/javascript" src="../../Plugins/luckysheet/static/plugin.js"></script>
    <script type="text/javascript" src="../../Plugins/luckysheet/static/luckysheet.umd.js"></script>
    <script type="text/javascript" src="../../Plugins/luckysheet/static/luckyexcel.umd.js"></script>
	<link rel="stylesheet" href="../deepseek/all.min.css">
	<style>
		.excel-toolbar{ position:absolute; z-index:999; top:13px; right:40px; display:flex; gap:10px; align-items:center; }
		.excel-toolbar__btn{ display:inline-flex; align-items:center; justify-content:center; gap:8px; min-width:112px; height:40px; padding:0 16px; border:0; border-radius:12px; background:linear-gradient(135deg,#2563eb 0%,#1d4ed8 100%); color:#fff; font-size:14px; font-weight:700; white-space:nowrap; box-shadow:0 14px 28px -18px rgba(37,99,235,.82); cursor:pointer; transition:transform .2s ease, box-shadow .2s ease, filter .2s ease; }
		.excel-toolbar__btn:hover{ transform:translateY(-1px); box-shadow:0 18px 30px -18px rgba(37,99,235,.9); filter:brightness(1.03); }
		.excel-toolbar__btn--secondary{ background:linear-gradient(135deg,#0f766e 0%,#0f766e 100%); box-shadow:0 14px 28px -18px rgba(15,118,110,.78); }
		.excel-toolbar__btn--neutral{ background:linear-gradient(135deg,#475569 0%,#334155 100%); box-shadow:0 14px 28px -18px rgba(51,65,85,.72); }
		.save{
            position: absolute;
            z-index: 999;
            top: 13px;
			margin: 2px;
			float: right;
			right:360px;
		}
		.returnurl{
            position: absolute;
            z-index: 999;
            top: 13px;
			margin: 2px;
			float: right;
			right:240px;
		}

	</style>

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
</head>
<body> 
<div class="excel-toolbar">
<button  onclick="save()" class="excel-toolbar__btn excel-toolbar__btn--secondary" type="button"><i class="fa fa-save" aria-hidden="true"></i><span>保存作品</span></button> 
<button  onclick="returnurl()" class="excel-toolbar__btn excel-toolbar__btn--neutral" type="button"><i class="fa fa-reply" aria-hidden="true"></i><span>返回学案</span></button>
</div>
<div id="lucky" style="margin:0px;padding:0px;position:absolute;width:100%;height:100%;left: 0px;top: 0px; font-size:16px;"></div>
</body>

<script type="text/javascript">

	var port=":8180";//端口
	var str=location.host+port;//location.host;192.168.1.3
	console.log(str);
	var lor=location.origin+port;// location.origin;http://192.168.1.3
	console.log(lor);

    var title="";//"<%=Titles%>";
	var reurl="<%=Fpage %>";
	var user="<%=Owner%>";
    var serverip = "<%=serverIp %>";
    if (serverip != "") {
        str = serverip+port;
        lor = "http://"+serverip+port;
    }

    options = {
	    userInfo:user,
	    container: 'lucky',
	    title: title,
	    lang: 'zh',
	    plugins:['chart'],
	    showsheetbar:false,
	    myFolderUrl:"#",
		allowUpdate: true,
		loadUrl: lor+"/load",
		updateUrl: "ws://" + str + "/update?name="+user
    }
    //window.luckysheet.create(options);

    function returnurl() {
        if (confirm('是否离开当前活动页面？请先保存作品。') == true) {
            window.location.href = "<%=Fpage %>";
        }
    }

    function save(){
	    console.log("保存信息");
		window.luckysheet.exitEditMode();
	    var excel = window.luckysheet.toJson(); 
        //console.log("保存",excel); 
		console.log("在线保存！"); 

        var id = "<%=Id %>";
        var urls = 'uploadexcel.ashx?id=' + id;
        var formData = new FormData();
        excel = JSON.stringify(excel);//将json对象转换成json对符串
        excel = encodeURIComponent(excel);//url编码
		
        formData.append('title', title);
        formData.append('excel', excel);

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
        }).fail(function (res) {
            alert("保存失败！");
            //console.log(res)
        });
    }
    /*
    导出的json字符串可以直接当作`luckysheet.create(options)`初始化工作簿时的参数`options`使用，
    使用场景在用户自己操作表格后想要手动保存全部的参数，再去别处初始化这个表格使用，类似一个luckysheet专有格式的导入导出
  
*/  

   window.addEventListener('load', function () {
        var codefile = "<%=codefile %>";
        var example="<%=Exampleurl %>";	
		var xhr = new XMLHttpRequest();
		var liveurl=location.origin+port+"/islive";
		xhr.open("GET", liveurl, true);
		xhr.onreadystatechange = function() {
			var response = xhr.responseText;
			if (xhr.readyState === 4 && xhr.status === 200) {
				// 在这里处理返回的数据
				if(response){			
					options['allowUpdate']=true;
					options['loadUrl']=lor+"/load",
					options['updateUrl']= "ws://" + str + "/update?name="+user			
					
					window.luckysheet.create(options);
					console.log("在线协作正常");
				}
			}
			else{
				if (codefile != "") {
					//console.log("读取",codefile);
					codefile = decodeURIComponent(codefile);
					codefile=JSON.parse(codefile);//将json字符串转换成json对象 
					//console.log("读取",codefile);
					window.luckysheet.create(codefile);
					console.log("读取存档！");
				}
				else{
					if(example!=""){                
						loadHandler(example);
						console.log("读取实例");
					}  
				}
			}
		};
		xhr.send();	


		function loadHandler(url) {
			//url = "demo.xlsx"; //文件链接"demo.xlsx"
			var name = ""; //文件名
			if (url == "") {
				return;
			}
			LuckyExcel.transformExcelToLuckyByUrl(url, name, function (exportJson, luckysheetfile) {

				if (exportJson.sheets == null || exportJson.sheets.length == 0) {
					alert("只支持xlsx格式文档!");
					return;
				}
				//console.log(exportJson, luckysheetfile);
				//window.luckysheet.destroy();
				
				options['data']=exportJson.sheets;
				window.luckysheet.create(options);
			});

		}
	
	});
</script>
</html>
