<%@ Page Language="C#" AutoEventWireup="true" CodeFile="qrcode.aspx.cs" Inherits="student_qrcode" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head id="Head1"  runat="server">
<meta charset="UTF-8">
<title>二维码生成器</title>
<link rel="stylesheet" href="../plugins/qrcode/spectre.min.css">
<link rel="stylesheet" href="../plugins/qrcode/style.css">
<script src="../plugins/qrcode/vue.min.js"></script>
<script src="../plugins/qrcode/qrcanvas@3"></script>
<script src="../plugins/qrcode/jsQR.js"></script>
<script src="../code/jquery.min.js"></script>
	<link rel="stylesheet" href="../deepseek/all.min.css">
	<style type="text/css">
		.qrcode-toolbar {
			display: flex;
			flex-wrap: wrap;
			gap: 10px;
			align-items: center;
			justify-content: center;
		}

		.qrcode-toolbar__btn {
			display: inline-flex;
			align-items: center;
			justify-content: center;
			gap: 8px;
			min-width: 110px;
			height: 40px;
			padding: 0 16px;
			border: 0;
			border-radius: 12px;
			background: linear-gradient(135deg, #2563eb 0%, #1d4ed8 100%);
			color: #ffffff;
			font-size: 14px;
			font-weight: 700;
			white-space: nowrap;
			box-shadow: 0 14px 28px -18px rgba(37, 99, 235, 0.82);
			cursor: pointer;
			transition: transform .2s ease, box-shadow .2s ease, filter .2s ease;
		}

		.qrcode-toolbar__btn:hover {
			transform: translateY(-1px);
			box-shadow: 0 18px 30px -18px rgba(37, 99, 235, 0.9);
			filter: brightness(1.03);
		}

		.qrcode-toolbar__btn--neutral {
			background: linear-gradient(135deg, #475569 0%, #334155 100%);
			box-shadow: 0 14px 28px -18px rgba(51, 65, 85, 0.72);
		}
	</style>

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
</head>
<body>
<div id="app" class="container" style="display:none" v-show="true">
  <div class="mt-2">
  <br>
  </div>
  <div class="columns">
    <div class="form-horizontal">
    <h1 style="text-align:center;">二维码生成器</h1>
	<br>
      <div class="form-group">
        <div class="column col-3">
          <div class="form-label ">内容:</div>
        </div>
        <div class="column">
          <textarea id="inputText" class="form-input " v-model="settings.qrtext"></textarea>
        </div>
      </div>
      <div class="form-group">
        <div class="column col-3">
          <div class="form-label ">颜色:</div>
        </div>
        <div class="column">
          <div class="text-right">
            <button class="btn btn-link btn-sm mr-1 px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" v-for="theme in themes" v-text="theme" @click.prevent="loadTheme(theme)"></button>
          </div>
        </div>
      </div>
	  <br>
      <div class="form-group">
        <div class="column col-4">
          <label class="form-checkbox ">
            <input type="checkbox" v-model="settings.logo">
            <i class="form-icon"></i>
            徽标
          </label>
        </div>
        <ul class="tab column" v-show="settings.logo">
          <li class="tab-item" :class="{ active: settings.logoType === 'image' }">
            <a href="#" @click.prevent="settings.logoType = 'image'">图片</a>
          </li>
          <li class="tab-item" :class="{ active: settings.logoType === 'text' }">
            <a href="#" @click.prevent="settings.logoType = 'text'">文字</a>
          </li>
        </ul>
      </div>
      <div class="form-group" v-show="settings.logo">
        <div class="column">
          <div class="form-group" v-show="settings.logoType === 'image'">
            <div class="column col-3">
              <label class="form-label label-sm">图片:</label>
            </div>
            <div class="column">
              <img src="../plugins/qrcode/logo.png" ref="logo" @load="update" style="max-width:330px;">
              <input class="form-input input-sm" type="file" @change="loadImage($event,'logo')">
            </div>
          </div>
          <div v-show="settings.logoType === 'text'">
            <div class="form-group">
              <div class="column col-3">
                <label class="form-label label-sm">文字:</label>
              </div>
              <div class="column">
                <input class="form-input input-sm" v-model="settings.logoText">
              </div>
            </div>
            <div class="form-group">
              <div class="column col-3">
                <label class="form-label label-sm">字体:</label>
              </div>
              <div class="column">
                <input class="form-input input-sm" placeholder="CSS font string" v-model="settings.logoFont">
              </div>
            </div>
            <div class="form-group">
              <div class="column col-3">
                <label class="form-label label-sm">颜色:</label>
              </div>
              <div class="column">
                <input class="form-input input-sm" type="color" v-model="settings.logoColor">
              </div>
            </div>
            <div class="form-group">
              <div class="column">
                <label class="form-checkbox ">
                  <input type="checkbox" v-model="settings.logoBold">
                  <i class="form-icon"></i>
                  粗体
                </label>
              </div>
              <div class="column">
                <label class="form-checkbox ">
                  <input type="checkbox" v-model="settings.logoItalic">
                  <i class="form-icon"></i>
                  斜体
                </label>
              </div>
            </div>
          </div>
        </div>
      </div>
		

	</div>
	
    <div class="column">
		<div class="flex flex-col items-center gap-4 mt-4">
		<qr-canvas :options="options"></qr-canvas>		
	        <div class="qrcode-toolbar">
			<button id="savebtn" class="qrcode-toolbar__btn" type="button"><i class="fa fa-save" aria-hidden="true"></i><span>保存作品</span></button>
	        	<button id="returnbtn" class="qrcode-toolbar__btn qrcode-toolbar__btn--neutral" type="button"><i class="fa fa-reply" aria-hidden="true"></i><span>返回学案</span></button>
		</div>
		</div>
    </div>
	
  </div>
</div>
<script type="text/javascript" >
    var id = "<%=Id %>";
    var words = unescape("<%=Words %>");
    var thumb = "<%=Thumb %>";
    window.__learnStatus = {
        snum: "<%= Snum %>",
        sname: "<%= LsSname %>",
        sgrade: "<%= LsSgrade %>",
        sclass: "<%= LsSclass %>",
        sid: "<%= LsSid %>",
        cid: "<%= LsCid %>",
        lid: "<%= LsLid %>",
        ltitle: "<%= LsLtitle %>",
        ltype: "<%= LsLtype %>"
    };
</script>
<script src="../js/learnstatus.js" type="text/javascript"></script>
<script src="../plugins/qrcode/index.js" type="text/javascript"></script>
<script type="text/javascript" >
    function returnurl() {
        if (confirm('是否离开当前活动页面？请先保存作品。') == true) {
            window.location.href = "<%=Fpage %>"
        }
    }
    $("#savebtn").on("click", function (e) {
        savework();
    });

    $("#returnbtn").on("click", function (e) {
        returnurl();
    });


    function savework() {
        var title = "";
        var Cover = blob(canvas.toDataURL());
        var Content = escape(document.getElementById("inputText").value);
        var Extension = "qrcode";
        var urls = 'uploadtopic.ashx?id=' + id;
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
            if (window.LearnStatus && typeof window.LearnStatus.submitted === "function") {
                window.LearnStatus.submitted();
            }
            alert("保存成功！");
            console.log(res)
        });
    }
</script>
</body>
</html>
