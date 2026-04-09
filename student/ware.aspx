<%@ Page Title="" Language="C#" MasterPageFile="~/student/Scm.master" StylesheetTheme="Student"
    AutoEventWireup="true" CodeFile="ware.aspx.cs" Inherits="Student_ware" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cpcm" runat="Server">
<div class="grid grid-cols-1 lg:grid-cols-5 gap-6 lg:gap-8 w-full max-w-full">
    <!-- Main Content (Iframe Preview) -->
    <div class="lg:col-span-4">
        <div id="previewArea" class="bg-white rounded-2xl shadow-sm border border-slate-200 overflow-hidden">
            <div class="course-node-head px-6 py-5 sm:px-8 sm:py-6 border-b border-slate-100">
                <div class="flex items-center gap-3">
                    <div class="w-9 h-9 rounded-xl bg-gradient-to-tr from-blue-500 to-indigo-500 flex items-center justify-center text-white flex-shrink-0">
                        <svg class="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9.75 17L9 20l-1-1-1 1 .75-3M15 6.75H9m6 3H9m8 8H7a2 2 0 01-2-2V6a2 2 0 012-2h10l5 5v6.75a2 2 0 01-2 2h-3"></path></svg>
                    </div>
                    <div class="min-w-0">
                        <div class="text-sm text-slate-500 font-medium">课件任务</div>
                        <asp:Label ID="LabelMtitle" runat="server" CssClass="course-node-title block text-xl sm:text-2xl font-extrabold text-slate-800 tracking-tight"></asp:Label>
                    </div>
                </div>
            </div>
            <iframe id="wareframe" src="<%=WareUrl %>" style="width:100%;min-height:80vh; border: none; border-radius: 1rem;"></iframe>
        </div>
    </div>

    <!-- Right Sidebar -->
    <div class="lg:col-span-1 flex flex-col gap-6">
        <div class="bg-slate-50/80 rounded-2xl border border-slate-200/60 p-5 shadow-sm sticky top-24 flex flex-col items-center gap-4">
            <asp:Image ID="Thumbnail" runat="server" CssClass="max-w-full rounded-xl shadow-sm border border-slate-200" />
            
            <input id="Btnform" type="button" value="保存" onclick="SaveIframe();"
                class="w-full py-2.5 bg-gradient-to-r from-blue-500 to-indigo-600 text-white font-bold rounded-xl hover:from-blue-600 hover:to-indigo-700 transition duration-300 shadow-md border-0 cursor-pointer" />
            
            <div id="msg" class="text-red-500 font-bold text-sm min-h-[1.5rem]"></div>
        </div>
    </div>
</div>

<script src="../code/html2canvas.min.js" type="text/javascript"></script>
<script type="text/javascript">
	var id = "<%=Id %>";
	var wareframe = document.getElementById("wareframe");
	var msg = document.getElementById("msg");
    var quiztitle="";
    var quizvalue=0;

    function setIframeHeight(iframe) {
        if (iframe) {
            var iframeWin = iframe.contentWindow || iframe.contentDocument.parentWindow;
            if (iframeWin.document.body) {
                iframe.height = iframeWin.document.documentElement.scrollHeight || iframeWin.document.body.scrollHeight;
            }
        }
    };
 
    window.addEventListener('load', function () {
        setIframeHeight(document.getElementById('wareframe'));
    });

    function SaveIframe() { 
        var htmlcode ="";// 使用缩略图预览
        var iframeContent = wareframe.contentDocument || wareframe.contentWindow.document;
		html2canvas(iframeContent.body, {
                        allowTaint: false,
                        useCORS: true,
                        scale: 1,
                        logging: false
                    }).then(pic => {					
			var urls = '../student/uploadtopic.ashx?id=' + id;
			var title = "";
			var Cover = blob(pic.toDataURL("image/jpg",0.5)); 
			var Content = htmlcode;
			var Extension = "ware";
			var formData = new FormData();
			formData.append('title', title);
			formData.append('cover', Cover);
			formData.append('content', Content);
			formData.append('ext', Extension);
			formData.append('score', quizvalue);

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
                var message = "保存成功！  "+quiztitle+" ："+quizvalue;
				alert(message);
                location.reload();

			}).fail(function (res) {
				console.log(res)
			}); 	
		
		});	
    }
                
    function returnurl() {
        if (confirm('是否要离开此页面？') == true) {
            window.location.href = courseurl;
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

    //从iframe中的页面向父页面传递数据
    window.addEventListener("message", receiveMessage, false);
    function receiveMessage(event) {
        var data = JSON.parse(event.data);
        quiztitle = data.name;
        quizvalue = data.value;
        //console.log("接收到的数据：", data);
    }

</script>
</asp:Content>
