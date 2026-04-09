<%@ Page Language="C#" AutoEventWireup="true" CodeFile="style.aspx.cs" Inherits="student_style" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html>
<head>
  <meta charset="UTF-8">
  <title>图像风格迁移</title>
  <link rel="stylesheet" type="text/css" href="../ai/styleml5/style.css">
  <script src="../code/jquery.min.js"></script>
  <script src="../ai/styleml5/libraries/p5.min.js"></script>
  <script src="../ai/styleml5/libraries/p5.dom.min.js"></script>
  <script src="../ai/styleml5/libraries/ml5.min.js"></script>
	<link rel="stylesheet" href="../deepseek/all.min.css">
	


    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Student/style.css" />
</head>

<body>
  <div class="body-container row">
  <h2 class="center-text">图像风格迁移</h2>  
  <div class="input-container white-box col-3">
    <h3 class="title">① 输入图像</h3>
    <div class="center-container">
      <img id="input-img" src='../ai/styleml5/images/girlwithpearl.jpg'/>
      <div id="input-source" class="reverse-img"></div>
    </div>

    <div class="style-container">
      <div class="container">
          <br><br><br><br><br>
      </div>
    </div>
    <div class="style-container">
	  
      <input id="uploader" name="inputImgFile" type="file" accept="image/*">
	  <button class="style-toolbar__btn" onclick="uploadImg()" type="button"><i class="fa fa-upload" aria-hidden="true"></i><span>上传图片</span></button>
      <div class="hideme" onclick="useWebcam()" >使用我的网络摄像头</div>
    </div>
    <div class="hideme">
	      <button class="style-toolbar__btn style-toolbar__btn--secondary" onclick="onPredictClick()" type="button"><i class="fa fa-arrow-right" aria-hidden="true"></i><span>传输图像</span></button>
    </div>
  </div>

  <div id="learning-container" class="white-box col-3">
    <h3 class="title">② 选择风格</h3>
    <div class="center-container">
      <img id="style-img"src='../ai/styleml5/images/wave.jpg' >
    </div>
	<div id="msg"></div>
    <div class="style-container">
      <div class="container">
        <a class="imageAnchor" href="#" >
          <img class="image" id="wave" alt="浮世绘画 葛饰北斋 《神奈川冲浪》" src='../ai/styleml5/images/wave.jpg' onclick="updateStyleImg(this)"/>
        </a>
      </div>
      <div class="container">
        <a class="imageAnchor" href="#">
          <img class="image" id="la_muse" alt="毕加索 西班牙 《缪斯女神》"  src='../ai/styleml5/images/la_muse.jpg' onclick="updateStyleImg(this)"/>
        </a>
      </div>
      <div class="container">
        <a class="imageAnchor" href="#">
          <img class="image" id="rain_princess" alt="以色列 Leonid Afremov 油画 《绚丽光影》"  src='../ai/styleml5/images/rain_princess.jpg' onclick="updateStyleImg(this)"/>
        </a>
      </div>
    </div>
    <div class="style-container">
      <div class="container">
        <a class="imageAnchor" href="#">
          <img class="image" id="udnie" alt="法国 Francis Picabia 布面油画"  src='../ai/styleml5/images/udnie.jpg' onclick="updateStyleImg(this)"/>
        </a>
      </div>
      <div class="container">
        <a class="imageAnchor" href="#">
          <img class="image" id="wreck"  alt="英国 William Turner 画廊油画 《船只失事》" src='../ai/styleml5/images/wreck.jpg' onclick="updateStyleImg(this)"/>
        </a>
      </div>
      <div class="container">
        <a class="imageAnchor" href="#">
          <img class="image" id="scream" alt="挪威 爱德华蒙克 《呐喊》"  src='../ai/styleml5/images/scream.jpg' onclick="updateStyleImg(this)"/>
        </a>
      </div>
    </div>
    <div class="style-container">
      <div class="container">
        <a class="imageAnchor" href="#">
          <img class="image" id="fuchun"  alt="元 黄公望 山水画 《富春山居图》" src='../ai/styleml5/images/fuchun.jpg' onclick="updateStyleImg(this)"/>
        </a>
      </div>
      <div class="container">
        <a class="imageAnchor" href="#">
          <img class="image" id="zhangdaqian"  alt="张大千 国画 《荷花》" src='../ai/styleml5/images/zhangdaqian.jpg' onclick="updateStyleImg(this)"/>
        </a>
      </div>
      <div class="container">
        <a class="imageAnchor" href="#">
          <img class="image" id="mathura"  alt="马图拉 雕刻" src='../ai/styleml5/images/mathura.jpg' onclick="updateStyleImg(this)"/>
        </a>
      </div>
    </div>
  </div>

  <div class="white-box output-container col-3">
    <h3 class="title">③ 输出图像</h3>
    <div id="output-img-container">
	</div>
	
    <div class="style-container">
      <div class="note">
        将风格迁移到输入图像上，生成新的图像。
      </div>
    </div>
    <div class="style-container">
      <div class="style-toolbar">
      <button id="savebtn" class="style-toolbar__btn" onclick="onProduct()" type="button"><i class="fa fa-save" aria-hidden="true"></i><span>保存图像</span></button>
      </div>
    </div>
    <div class="style-container">
      <div class="style-toolbar">
      <button class="style-toolbar__btn style-toolbar__btn--neutral" onclick="returnurl()" type="button"><i class="fa fa-reply" aria-hidden="true"></i><span>返回学案</span></button>
      </div>
    </div>
  </div>

  </div>
    <script type="text/javascript">
        window.__styleConfig = {
            id: "<%=Id %>",
            fpage: "<%=Fpage %>"
        };
    </script>
    <script type="text/javascript" src="../js/style.js"></script>
</body>

</html>
