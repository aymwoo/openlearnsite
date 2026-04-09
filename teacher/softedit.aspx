<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher"  Validaterequest="false"  AutoEventWireup="true" CodeFile="softedit.aspx.cs" Inherits="Teacher_softedit" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
  <link href="../js/fileupload.css" rel="stylesheet" />
  <link href="../js/vendors/wangeditor/style.css" rel="stylesheet" />
  <link rel="stylesheet" href="../js/vendors/vditor/index.css" />
  <div   class="placehold">
    <div  class="softdiv">
        &nbsp;&nbsp; 资源名称：<asp:TextBox 
            ID="Texttitle" runat="server"  Width="500px"  SkinID="TextBoxNormal" CssClass="px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300"></asp:TextBox>
        <br />
&nbsp;&nbsp; 资源分类：<asp:DropDownList ID="ddlcategory" runat="server">
        </asp:DropDownList>
&nbsp;资源属性：<asp:DropDownList ID="DDLclass" runat="server" Font-Size="9pt" Width="60px">
            <asp:ListItem Selected="True">教程</asp:ListItem>
            <asp:ListItem>微课</asp:ListItem>
            <asp:ListItem>资料</asp:ListItem>
            <asp:ListItem>软件</asp:ListItem>
            <asp:ListItem>游戏</asp:ListItem>
            <asp:ListItem>课程</asp:ListItem>
        </asp:DropDownList>
        &nbsp;评分方式：<asp:DropDownList ID="DDLscoreType" runat="server" Font-Size="9pt">
            <asp:ListItem Value="original">原学分制</asp:ListItem>
            <asp:ListItem Value="comprehensive">综合评分制</asp:ListItem>
        </asp:DropDownList>
        &nbsp;学分限制：<asp:DropDownList ID="DDLopen" runat="server" Font-Size="9pt">
            <asp:ListItem Value="10">A</asp:ListItem>
            <asp:ListItem Value="8">B</asp:ListItem>
            <asp:ListItem Value="6">C</asp:ListItem>
            <asp:ListItem Value="4">D</asp:ListItem>
            <asp:ListItem Value="2">E</asp:ListItem>
        </asp:DropDownList>
        &nbsp;综合得分：<asp:TextBox ID="TXTscore" runat="server" Width="40px" Text="60" ToolTip="学生综合得分达到此值才能访问资源（0-100分）"></asp:TextBox>
        &nbsp;<asp:CheckBox ID="CheckBoxFhide" runat="server" Text="是否隐藏" />
        &nbsp;<asp:CheckBox ID="CheckBoxFhid" runat="server" Text="是否共享" />
        </div>
    <div   class="softcontent" >
           <div style="margin:0 0 10px 0; display:flex; align-items:center; gap:8px; flex-wrap:wrap;">
               <span style="font-size:13px;font-weight:700;color:#334155;">编辑器：</span>
               <select id="editorSelector" onchange="switchEditor(this.value)" style="min-height:36px;padding:0 28px 0 10px;border:1px solid #cbd5e1;border-radius:8px;background:#fff;color:#0f172a;">
                   <option value="kindeditor" selected>KindEditor</option>
                   <option value="wangeditor">WangEditor</option>
                   <option value="vditor">Vditor</option>
               </select>
           </div>
           <script charset="utf-8" src="../kindeditor/kindeditor-min.js"></script>
		<script charset="utf-8" src="../kindeditor/lang/zh_CN.js"></script>
		<script src="../js/vendors/vditor/index.min.js"></script>
		<script src="../js/vendors/wangeditor/index.js"></script>
		<script src="../teacher/editor-upload-helper.js" type="text/javascript"></script>
		
    <div id="wangeditor-wrap" style="display:none; width: 780px; position:relative; border:1px solid #ccc; z-index:100; margin-bottom:10px;">
        <div id="wangeditor-toolbar" style="border-bottom:1px solid #ccc;"></div>
        <div id="wangeditor-text" style="height:300px;"></div>
    </div>
    <div id="vditor-wrap" style="display:none; width: 780px; position:relative; margin-bottom:10px;">
        <div id="vditor-container"></div>
    </div>
		<script>
		    var editor;
            var cid= '-1';
            var ty="Soft";
            var upjs= '../kindeditor/aspnet/upload_json.aspx?cid='+cid+'&ty='+ty;
            var fmjs='../kindeditor/aspnet/file_manager_json.aspx?cid='+cid+'&ty='+ty;
		    KindEditor.ready(function (K) {
                editor = K.create('textarea[name="ctl00$Content$mcontent"]', {
		            resizeType: 1,
		            newlineTag: "br",                    
				uploadJson : upjs,
				fileManagerJson : fmjs,
				allowFileManager: true,
				filterMode: false          
		        });
		    });
		</script>
    <textarea  id ="mcontent" runat ="server" style="width: 780px; height:300px;" ></textarea>  
<br />              
    </div>
     <div  class="softcenter">
              <asp:Label ID="LabelFhit" runat="server" Visible="False"></asp:Label>
              <asp:Label ID="LabelFfiletype" runat="server" Visible="False"></asp:Label>
              <br />
              原上传可限制资源：<asp:LinkButton ID="Linkold" runat="server" 
                  onclick="Linkold_Click"></asp:LinkButton>
              <br />
              <br />
              更新可限制资源：
                <div class="ls-upload" data-label="点击或拖拽上传资源文件" data-hint="可上传任意类型文件">
                    <asp:FileUpload ID="FUsoft" runat="server" />
                </div>
               <br />
               <asp:Label ID="Labelmsg" runat="server" ></asp:Label>
              <br />
         <br />
              <asp:Button ID="Btnedit" runat="server"  Text="保存修改" OnClick="Btnedit_Click" OnClientClick="return syncContent();"
                  CssClass="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" />
               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
              <asp:Button ID="Btnreturn" runat="server"  Text="返回列表" OnClick="Btnreturn_Click"
                  CssClass="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" />
               <br />
               <br />         
         </div>
          <br />           
        </div>
    <script src="../js/fileupload.js"></script>
    <script type="text/javascript">
        window.__softeditConfig = {
            mcontentId: '<%= mcontent.ClientID %>'
        };
    </script>
    <script type="text/javascript" src="../js/softedit.js"></script>
</asp:Content>
