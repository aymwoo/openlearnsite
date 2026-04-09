// ace.require("ace/ext/language_tools");
    // 初始化editor(）
    var aeditor = ace.edit("editor");
    aeditor.setOptions({
      // 默认:false
      wrap: true, // 换行
      // autoScrollEditorIntoView: false, // 自动滚动编辑器视图
      enableLiveAutocompletion: true, // 智能补全
      enableSnippets: true, // 启用代码段
      //enableBasicAutocompletion: true, // 启用基本完成 不推荐使用
    });
    // 设置主题  cobalt monokai vscode xcode textmate sqlserver  twilight
    aeditor.setTheme("ace/theme/textmate");
    // 设置编辑语言
    aeditor.getSession().setMode("ace/mode/python");
    aeditor.setFontSize(24);
    aeditor.setReadOnly(false)
    aeditor.getSession().setTabSize(4);

var mycode = document.getElementById("code");
    var pprint = document.getElementById("print");

    var mypre = document.getElementById("output");
    var result = document.getElementById("result");
    var savemsg = document.getElementById("savemsg");

    function outf(text) {
        mypre.innerText = mypre.innerText + text;
    }
    function builtinRead(x) {
        if (Sk.builtinFiles === undefined || Sk.builtinFiles["files"][x] === undefined)
            throw "File not found: '" + x + "'";
        return Sk.builtinFiles["files"][x];
    }

    result.onclick = function () {
        output.focus();
    }

    function initedit() {
        var cc = mycode.value;
        var pp = pprint.value;
        if (cc.length > 0) {
            aeditor.setValue(cc);
            mypre.innerText = pp;
        }
    }

    window.addEventListener('load', function () {
        mycode = document.getElementById("code");
        pprint = document.getElementById("print");
        console.log(mycode);
        console.log(pprint);
        initedit();
    });

    function myfun() {
        return new Promise(function (resolve, reject) {
            var myinput = document.createElement("input");
            myinput.setAttribute("type", "text");
            myinput.setAttribute("class", "input");
            mypre.appendChild(myinput);
            myinput.focus();
            result.onclick = function () {
                myinput.focus();
            }

            myinput.onkeypress = function () {
                if (event.keyCode == 13) {
                    args = myinput.value;
                    console.log(args);
                    resolve(args);
                    mypre.removeChild(myinput);
                    temp = mypre.innerText;
                    temp = temp + args;
                    mypre.innerHTML = temp + "\n";
                }
            }
        })
    }
    function clearit() {
        output.innerHTML = '';
        mypre.innerHTML = '';
        pprint.value = '';
    }
    function runit() {
        var prog = aeditor.getValue();
        mypre.innerHTML = '';
        output.innerHTML = '';
        Sk.pre = "output";
        Sk.configure({ output: outf, read: builtinRead, __future__: Sk.python3, inputfun: myfun });

        var myPromise = Sk.misceval.asyncToPromise(function () {
            return Sk.importMainWithBody("<stdin>", false, prog, true);
        });

        myPromise.then(function (mod) {
            console.log('运行成功!');
            mycode.value = prog;
            pprint.value = output.innerText;
            console.log('代码：');
            console.log(mycode.value);
            console.log('输出结果：');
            console.log(pprint.value);
            //getsvg();
        },
    function (err) {
        var msg = err.toString();
        console.log(msg);
        mypre.innerHTML = msg;
    });
    }

    function getsvg() {
        var op = output.innerHTML;
        if (op == '') {
            var canvas = document.createElement("canvas");
            if (canvas != null) {
                var dataUrl = canvas.toDataURL('image/jpeg');
                pprint.value = dataUrl;
                console.log(dataUrl);
            }
        }
    }

    document.onkeyup = keyUp;
    function keyUp() {
        var prog = aeditor.getValue();
        mycode.value = prog;
        voice();
    }
    function voice() {
        var audio = document.createElement("audio");
        audio.src = '../code/code.ogg';
        audio.play();
    }

var keditor;
	var wangEditorObj;
	var vditorObj;
	var currentEditor = 'kindeditor';
	var lastVditorMarkdown = null;
	var lastVditorHtml = '';
	var vditorReady = false;
	var pendingVditorHtml = null;
	var cid= window.__problemConfig.myCid;
	var ty="Course";
	var upjs= '../kindeditor/aspnet/upload_json.aspx?cid='+cid+'&ty='+ty;
	var fmjs='../kindeditor/aspnet/file_manager_json.aspx?cid='+cid+'&ty='+ty;
	KindEditor.ready(function (K) {
		keditor = K.create('#mcontent', {
		    resizeType: 1,
		    pasteType: 1,
		    newlineTag: "br",				
			uploadJson : upjs,
			fileManagerJson : fmjs,
			allowFileManager : true,
			filterMode : false,
		    allowImageUpload: true,
		    items: ['fontname', 'fontsize', '|', 'bold', 'italic','removeformat','image','about'] ,
		    afterCreate: function () {
		        window.setTimeout(autoSelectInitialProblemEditor, 0);
		    }
		});
	});
  function isProblemHtml(content) {
      return /<\/?[a-z][\s\S]*>/i.test(content || '');
  }

  function isLikelyProblemMarkdown(content) {
      if (!content) return false;
      return /```/.test(content)
          || /^#{1,6}\s/m.test(content)
          || /^\s*[-*+]\s/m.test(content)
          || /^\s*\d+\.\s/m.test(content)
          || /\[[^\]]+\]\([^)]+\)/.test(content);
  }

  function normalizeProblemContent(content) {
      return (content || '').replace(/\s+/g, ' ').trim();
  }

  function getPreferredProblemVditorValue(content) {
      if (!content) return '';
      return isProblemHtml(content) ? safeProblemHtml2Md(content) : content;
  }

  function rememberProblemVditorState() {
      if (!vditorObj) return;
      lastVditorMarkdown = vditorObj.getValue();
      lastVditorHtml = vditorObj.getHTML();
  }

  function shouldRestoreProblemMarkdown(currentHtml) {
      if (lastVditorMarkdown === null) return false;
      var currentNormalized = normalizeProblemContent(currentHtml);
      var savedNormalized = normalizeProblemContent(lastVditorHtml);
      return currentNormalized === '' || currentNormalized === savedNormalized;
  }

  function autoSelectInitialProblemEditor() {
      var selector = document.getElementById('editorSelector');
      var mcontent = document.getElementById('mcontent');
      if (!selector || !mcontent) return;
      if (isLikelyProblemMarkdown(mcontent.value)) {
          selector.value = 'vditor';
          switchProblemEditor('vditor');
      }
  }

  function initProblemWangEditor() {
      if (wangEditorObj) return;
      const { createEditor, createToolbar } = window.wangEditor;
      const mcontent = document.getElementById('mcontent');
      wangEditorObj = createEditor({
          selector: '#problem-wangeditor-text',
          html: keditor ? keditor.html() : mcontent.value,
          config: {
              placeholder: '请输入试题内容...',
              MENU_CONF: {
                  uploadImage: { server: upjs, customInsert(res, insertFn) { if (res.error === 0) insertFn(res.url); else alert(res.message || '图片上传失败'); } },
                  uploadAttachment: { server: upjs, customInsert(res) { if (res.error === 0) LearnSiteEditorUploadHelper.insertUploadedLinkToWangEditor(wangEditorObj, res); else alert(res.message || '附件上传失败'); } },
                  uploadFile: { server: upjs, customInsert(res) { if (res.error === 0) LearnSiteEditorUploadHelper.insertUploadedLinkToWangEditor(wangEditorObj, res); else alert(res.message || '文件上传失败'); } }
              }
          }
      });
      createToolbar({ editor: wangEditorObj, selector: '#problem-wangeditor-toolbar', config: {} });
  }

  function safeProblemHtml2Md(html) {
      try {
          if (vditorObj && vditorObj.vditor && vditorObj.vditor.lute) return vditorObj.vditor.lute.HTML2Md(html);
          var l = Lute.New();
          return l.HTML2Md(html);
      } catch (e) {
          return html;
      }
  }

  function initProblemVditor() {
      if (vditorObj) return;
      const mcontent = document.getElementById('mcontent');
      var initialContent = getPreferredProblemVditorValue(lastVditorMarkdown !== null ? lastVditorMarkdown : (keditor ? keditor.html() : mcontent.value));
      vditorObj = new Vditor('problem-vditor-container', {
          height: 260,
          mode: 'ir',
          upload: { handler: function (files) { LearnSiteEditorUploadHelper.handleVditorUpload(vditorObj, upjs, files); } },
          preview: { mode: 'both' },
          cache: { enable: false },
          after: function () {
              vditorReady = true;
              var contentToSet = pendingVditorHtml !== null ? pendingVditorHtml : initialContent;
              vditorObj.setValue(contentToSet || '');
              rememberProblemVditorState();
              pendingVditorHtml = null;
          }
      });
  }

  function syncProblemContent() {
      var mcontent = document.getElementById('mcontent');
      if (!mcontent) return true;
      if (currentEditor === 'kindeditor' && keditor) mcontent.value = keditor.html();
      else if (currentEditor === 'wangeditor' && wangEditorObj) mcontent.value = wangEditorObj.getHtml();
      else if (currentEditor === 'vditor' && vditorObj) {
          rememberProblemVditorState();
          mcontent.value = lastVditorMarkdown || '';
      }
      return true;
  }

  function switchProblemEditor(type) {
      currentEditor = type;
      var kindContainer = document.querySelector('.ke-container');
      var wangContainer = document.getElementById('problem-wangeditor-wrap');
      var vditorContainer = document.getElementById('problem-vditor-wrap');
      var currentHtml = '';
      if (kindContainer && kindContainer.style.display !== 'none' && keditor) currentHtml = keditor.html();
      else if (wangContainer && wangContainer.style.display !== 'none' && wangEditorObj) currentHtml = wangEditorObj.getHtml();
      else if (vditorContainer && vditorContainer.style.display !== 'none' && vditorObj && vditorReady) {
          rememberProblemVditorState();
          currentHtml = lastVditorHtml;
      }
      if (kindContainer) kindContainer.style.display = 'none';
      if (wangContainer) wangContainer.style.display = 'none';
      if (vditorContainer) vditorContainer.style.display = 'none';
      if (type === 'kindeditor') {
          if (kindContainer) kindContainer.style.display = 'block';
          if (keditor && currentHtml) keditor.html(currentHtml);
      } else if (type === 'wangeditor') {
          if (wangContainer) wangContainer.style.display = 'block';
          initProblemWangEditor();
          if (wangEditorObj && currentHtml) wangEditorObj.setHtml(currentHtml);
      } else if (type === 'vditor') {
          if (vditorContainer) vditorContainer.style.display = 'block';
          var vditorContent = shouldRestoreProblemMarkdown(currentHtml) ? lastVditorMarkdown : getPreferredProblemVditorValue(currentHtml);
          if (!vditorObj) {
              pendingVditorHtml = vditorContent;
              initProblemVditor();
          } else if (vditorReady) {
              vditorObj.setValue(vditorContent || '');
              rememberProblemVditorState();
          } else {
              pendingVditorHtml = vditorContent;
          }
      }
  }
