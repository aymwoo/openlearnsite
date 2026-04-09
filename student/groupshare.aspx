<%@ Page Language="C#" AutoEventWireup="true" CodeFile="groupshare.aspx.cs" Inherits="Student_groupshare" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
        <meta charset="utf-8" />
<title></title>
    
    <script src="../js/jquery.min.js" type="text/javascript"></script>
    <script src="../js/dropzone/dropzone-min.js" type="text/javascript"></script>
    <link href="../code/css/font-awesome.min.css" rel="stylesheet" type="text/css" />

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Student/groupshare.css" />
</head>
<body  class="by">
    <form id="form1" class="dropzone" runat="server">
    <div class="share-shell">
    <div id="doc_area" class="share-card" >
    <div class="share-hero">
        <div class="share-title">
            <span class="share-title-icon"><i class="fa fa-folder-open" aria-hidden="true"></i></span>
            <asp:Label ID="Labeltitle" runat="server"></asp:Label>
        </div>
        <p class="share-subtitle">统一展示公共资源、我的网盘和小组网盘内容。支持点击或拖放上传文件，并保留原有删除与切换逻辑。</p>
        </div>
        <div class="share-content">
        <div id="file_area"  class="share-dropzone"  title="请点击或拖放文件到这里"> 
                <asp:DataList ID="Dlfilelist" runat="server" 
                    RepeatColumns="2" RepeatDirection="Horizontal" CellPadding="3" 
                    CellSpacing="3" Width="99%" 
                    HorizontalAlign="Center" onitemcommand="Dlfilelist_ItemCommand" 
                    CssClass="share-grid"
                    onitemdatabound="Dlfilelist_ItemDataBound" >
                    <ItemTemplate>
                        <div class="share-file"> 
                           <div class="share-file__head">
                            <asp:Image ID="Imageext" runat="server" ImageUrl='<%# Eval("Kftpe") %>' CssClass="share-file__icon" />
                            <asp:HyperLink ID="HLfname" runat="server" NavigateUrl='<%# Eval("Kfurl") %>' Target="_blank" Text='<%# Eval("KfnameShort") %>' Font-Underline="False" CssClass="share-file__name"></asp:HyperLink>
                            </div>
                           <div class="share-file__meta">
                            <div class="share-file__date">
                            <asp:Label ID="Labelfsize" runat="server" Text='<%# Eval("Kfsize") %>' ToolTip='<%# Eval("Kfdate") %>'></asp:Label>
                            <asp:Label ID="Labelfdate" runat="server" Text='<%# Eval("Kfdate") %>'></asp:Label>
                            </div>
                            <asp:ImageButton ID="ImgBtnDelete" runat="server" CommandArgument='<%# Eval("Kfurl") %>' 
                                CommandName="D" ImageUrl="~/images/delete.gif" ToolTip="删除" CssClass="share-delete" />
                             </div>
                        </div>
                    </ItemTemplate>
                    <SeparatorStyle BorderColor="Silver" BorderStyle="Dotted" BorderWidth="1px" />
                </asp:DataList>
         
        </div>
		<div class="share-footer">
        <div id="dleft" class="share-footer__icon">
            <asp:Image ID="Imagedisk" runat="server" Height="24px" Width="24px" 
                ImageUrl="~/images/diskgreen.gif" />
        </div>
        <div id="dright" class="share-footer__body">   
        <div class="share-toolbar">
         <asp:Button ID="BtnTea" runat="server" BackColor="#CFE4D0" BorderStyle="None" 
             Font-Bold="False" Font-Size="9pt" onclick="BtnTea_Click" Text="公共资源"  CssClass="share-toolbar__btn share-toolbar__btn--secondary" />
         <asp:Button ID="BtnStu" runat="server" BackColor="#CFE4D0" BorderStyle="None" 
             Font-Bold="False" Font-Size="9pt" onclick="BtnStu_Click" Text="我的网盘"  CssClass="share-toolbar__btn share-toolbar__btn--secondary" />
             <asp:Button ID="BtnGroup" runat="server" BackColor="#CFE4D0" BorderStyle="None" 
             Font-Bold="False" Font-Size="9pt" onclick="BtnGroup_Click" Text="小组网盘"  CssClass="share-toolbar__btn share-toolbar__btn--active" />
          <asp:CheckBox ID="CkIsGroup" runat="server" Enabled="False" Visible="False" />&nbsp;
        </div>
         <div class="share-toolbar__status">
           <asp:Label ID="Labeldisk" runat="server" Font-Size="9pt" ForeColor="#3F6159"></asp:Label>
         </div>
         </div>  
		</div>
    </div>
    </div>
    </div>
    <script type="text/javascript">
        window.__groupshareConfig = {
            isgroup: "<%=isgroup %>",
            iscommon: "<%=iscommon %>",
            can: "<%=can %>"
        };
    </script>
    <script type="text/javascript" src="../js/groupshare.js"></script>
    </form>
    
</body>
</html>
