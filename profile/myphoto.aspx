<%@ Page Title="" Language="C#" MasterPageFile="~/profile/Pf.master"  StylesheetTheme="Student"  AutoEventWireup="true" CodeFile="myphoto.aspx.cs" Inherits="Profile_myphoto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cstu" Runat="Server">
<link href="../js/fileupload.css" rel="stylesheet" />
<style>
*{box-sizing:border-box}
.ph-wrap{
    padding:16px;
    font-family:-apple-system,BlinkMacSystemFont,'Segoe UI',Roboto,sans-serif;
    background:linear-gradient(160deg,#fff7ed 0%,#fff 60%);
    min-height:100%;
}
.ph-card{
    max-width:340px; margin:0 auto;
    background:#fff; border:1px solid #fed7aa;
    border-radius:14px; overflow:hidden;
    box-shadow:0 4px 20px rgba(234,88,12,.08);
}

/* ── header ── */
.ph-head{
    display:flex; align-items:center; gap:8px;
    padding:11px 16px; border-bottom:1px solid #fff7ed;
    background:linear-gradient(to right,#fff7ed,#fff);
}
.ph-head-icon{
    width:28px; height:28px; border-radius:8px;
    background:#fff7ed; border:1px solid #fed7aa;
    display:flex; align-items:center; justify-content:center; flex-shrink:0;
}
.ph-head h2{margin:0;font-size:13px;font-weight:800;color:#9a3412;letter-spacing:-.01em;}

/* ── body ── */
.ph-body{padding:14px 16px;}
.ph-inner{display:flex;flex-direction:column;gap:12px;}

/* submit btn */
.ph-btn-row{display:flex;gap:8px;align-items:center;}
.ph-btn{
    flex:1!important; display:flex!important; align-items:center!important; justify-content:center!important;
    height:34px!important;
    border-radius:9px!important; border:0!important;
    font-size:12px!important; font-weight:700!important; letter-spacing:.01em!important;
    cursor:pointer!important; color:#fff!important;
    background:linear-gradient(135deg,#ea580c,#c2410c)!important;
    box-shadow:0 3px 10px rgba(234,88,12,.18)!important;
    transition:transform .15s, box-shadow .15s!important; white-space:nowrap!important;
}
.ph-btn:hover{transform:translateY(-1px)!important;box-shadow:0 5px 14px rgba(234,88,12,.28)!important;}
.ph-btn[disabled]{opacity:.5!important;cursor:not-allowed!important;transform:none!important;box-shadow:none!important;}

/* restrict msg */
.ph-msg{
    flex:1; display:inline-flex!important; align-items:center!important; justify-content:center!important;
    height:34px; padding:0 10px; border-radius:9px;
    font-size:11px; font-weight:600;
    background:#fee2e2; color:#991b1b;
    border:1px solid #fecaca; text-align:center;
    white-space:nowrap;
}
.ph-avatar img{width:100%;height:100%;object-fit:cover;display:block;}

/* upload zone — override default sizes */
.ph-wrap .ls-upload{
    padding:14px 16px; gap:4px;
    border-radius:10px;
}
.ph-wrap .ls-upload__icon{width:36px;height:36px;}
.ph-wrap .ls-upload__icon svg{width:18px;height:18px;}
.ph-wrap .ls-upload__text{font-size:12px;}
.ph-wrap .ls-upload__hint{font-size:11px;}

/* submit btn */
.ph-btn{
    display:flex!important; align-items:center!important; justify-content:center!important;
    width:100%!important; height:36px!important;
    border-radius:9px!important; border:0!important;
    font-size:12px!important; font-weight:700!important; letter-spacing:.01em!important;
    cursor:pointer!important; color:#fff!important;
    background:linear-gradient(135deg,#ea580c,#c2410c)!important;
    box-shadow:0 3px 10px rgba(234,88,12,.22)!important;
    transition:transform .15s, box-shadow .15s!important;
}
.ph-btn:hover{transform:translateY(-1px)!important;box-shadow:0 5px 14px rgba(234,88,12,.3)!important;}
.ph-btn[disabled]{opacity:.5!important;cursor:not-allowed!important;transform:none!important;box-shadow:none!important;}

/* restrict msg */
.ph-msg{
    display:block; width:100%;
    padding:7px 12px; border-radius:8px;
    font-size:11px; font-weight:600;
    background:#fee2e2; color:#991b1b;
    border:1px solid #fecaca; text-align:center;
}

/* note */
.ph-note{
    font-size:11px; color:#94a3b8; text-align:center;
    line-height:1.6; padding:8px 12px;
    background:#f8fafc; border-radius:8px;
    border:1px solid #f1f5f9;
}
</style>

<div class="ph-wrap">
  <div class="ph-card">

    <div class="ph-head">
      <div class="ph-head-icon">
        <svg width="14" height="14" fill="none" stroke="#ea580c" stroke-width="2" viewBox="0 0 24 24">
          <path stroke-linecap="round" stroke-linejoin="round" d="M4 16l4.586-4.586a2 2 0 012.828 0L16 16m-2-2l1.586-1.586a2 2 0 012.828 0L20 14m-6-6h.01M6 20h12a2 2 0 002-2V6a2 2 0 00-2-2H6a2 2 0 00-2 2v12a2 2 0 002 2z"/>
        </svg>
      </div>
      <h2>修改相片</h2>
    </div>

    <div class="ph-body">
      <asp:Panel ID="Panel1" runat="server" CssClass="ph-inner">

        <!-- 当前头像 -->
        <div class="ph-avatar-row">
          <div class="ph-avatar">
            <asp:Image ID="Imageface" runat="server" />
          </div>
        </div>

        <!-- 上传区域 -->
        <div class="ls-upload" data-accept=".jpg,.jpeg,.png" data-max-size="2048"
             data-label="点击或拖拽上传相片" data-hint="jpg / jpeg / png，≤ 2048 KB">
          <asp:FileUpload ID="PhotoFileUpload" runat="server" />
        </div>

        <!-- 提交 + 受限提示 并排 -->
        <div class="ph-btn-row">
          <asp:Button ID="Btnphoto" runat="server" Enabled="False"
              onclick="Btnphoto_Click" SkinID="buttonSkin"
              Text="提交相片" CssClass="ph-btn" />
          <asp:Label ID="Labelstr" runat="server" SkinID="LabelMsgRed" CssClass="ph-msg"></asp:Label>
        </div>

        <!-- 说明 -->
        <p class="ph-note">支持 jpg / jpeg / png，大小不超过 2048 KB<br/>过大将自动缩小至宽 320px</p>

      </asp:Panel>
    </div>

  </div>
</div>
<script src="../js/fileupload.js"></script>
</asp:Content>
