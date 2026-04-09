<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master"  StylesheetTheme="Teacher" Validaterequest="false"   AutoEventWireup="true" CodeFile="txtformedit.aspx.cs" Inherits="Teacher_txtformedit" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
<link href="../js/vendors/wangeditor/style.css" rel="stylesheet" />
<link rel="stylesheet" href="../js/vendors/vditor/index.css" />
<div  class="cplace">
    <div  class="cleft">
        &nbsp;表格名称：<asp:TextBox ID="Texttitle" runat="server"  SkinID="TextBoxNormal" 
            Width="200px"  CssClass="px-3 py-2 border border-gray-300 rounded focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition duration-300"></asp:TextBox>
        <asp:CheckBox ID="CheckPublish" runat="server" Text="是否发布"  Checked="True" />
        <asp:CheckBox ID="CheckCollabo" runat="server" Text="是否协同"  Checked="True" />
        </div>
    <div  >
        <div style="margin:0 0 10px 0; display:flex; align-items:center; gap:8px; flex-wrap:wrap;">
            <span style="font-size:13px;font-weight:700;color:#334155;">编辑器：</span>
            <select id="editorSelector" onchange="switchEditor(this.value)" style="min-height:36px;padding:0 28px 0 10px;border:1px solid #cbd5e1;border-radius:8px;background:#fff;color:#0f172a;">
                <option value="kindeditor" selected>KindEditor</option>
                <option value="wangeditor">WangEditor</option>
                <option value="vditor">Vditor</option>
            </select>
        </div>
        <script type="text/javascript" charset="utf-8" src="../kindeditor/kindeditor-min.js"></script>
		<script type="text/javascript"  charset="utf-8" src="../kindeditor/lang/zh_CN.js"></script>
		<script src="../js/vendors/vditor/index.min.js"></script>
		<script src="../js/vendors/wangeditor/index.js"></script>
		<script src="../teacher/editor-upload-helper.js" type="text/javascript"></script>
		
    <div id="wangeditor-wrap" style="display:none; width: 780px; position:relative; border:1px solid #ccc; z-index:100; margin-bottom:10px;">
        <div id="wangeditor-toolbar" style="border-bottom:1px solid #ccc;"></div>
        <div id="wangeditor-text" style="height:360px;"></div>
    </div>
    <div id="vditor-wrap" style="display:none; width: 780px; position:relative; margin-bottom:10px;">
        <div id="vditor-container"></div>
    </div>
    <textarea  id ="mcontent" runat ="server" style="width: 780px; height:450px;" ></textarea>
    </div>
     <div  class="placehold">
               <asp:Label ID="Labelmsg" runat="server" ></asp:Label>
               <br />
               <br />
         <br />
              <asp:Button ID="Btnedit" runat="server"  Text="修改表单" OnClick="Btnedit_Click" OnClientClick="return syncContent();"
                   CssClass="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" />&nbsp;&nbsp;&nbsp;
              <asp:Button ID="BtnCourse" runat="server"  Text="返回学案" OnClick="BtnCourse_Click" CssClass="admin-form-btn admin-form-btn--secondary" /><br />
         <br />
         </div>
           
        </div>
    <script type="text/javascript">
        window.__txtformeditConfig = {
            myCid: '<%=myCid() %>',
            mcontentId: '<%= mcontent.ClientID %>'
        };
    </script>
    <script type="text/javascript" src="../js/txtformedit.js"></script>
</asp:Content>
