<%@ Page Language="C#" AutoEventWireup="true" StylesheetTheme="Teacher" CodeFile="circleshow.aspx.cs" Inherits="Teacher_circleshow" ResponseEncoding="utf-8" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
        <meta charset="utf-8" />
<title>学生文档作品自动展示</title>
    <script src="../js/jquery.min.js" type="text/javascript"></script>
    
    <link rel="stylesheet" type="text/css" href="../App_Themes/Teacher/circleshow.css" />
</head>
<body class="circle-page">
    <form id="form1" runat="server">
    <div class="circle-shell">
        <section class="circle-hero">
            <h1 class="circle-title"><asp:Label  ID="LabeMtitle" runat="server" Font-Bold="True"></asp:Label></h1>
            <p class="circle-subtitle">自动循环展示班级作品，并支持教师现场评分、筛选、点评与删除。</p>
        </section>

        <section class="circle-panel">
            <div class="circle-toolbar">
                <div class="circle-controls">
                    <asp:Button ID="Btnflash" runat="server" Text="刷新" onclick="Btnflash_Click" Width="40px" CssClass="circle-btn" />
                    <asp:Button ID="Btnrestart" runat="server" Text="重新" onclick="Btnrestart_Click" Width="40px" CssClass="circle-btn" />
                    <asp:Button ID="Btnstop" runat="server" Text="继续" onclick="Btnstop_Click" Width="40px" CssClass="circle-btn circle-btn--secondary" />
                    <asp:Button ID="ImgBtnLeft" runat="server" Text="上一项" OnClick="ImgBtnLeft_Click" CssClass="circle-btn circle-btn--ghost" />
                    <asp:DropDownList ID="DDLstore" runat="server" Font-Bold="True" Width="100px" AutoPostBack="True" Font-Size="12pt" onselectedindexchanged="DDLstore_SelectedIndexChanged" CssClass="circle-select">
                        <asp:ListItem></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="ImgBtnright" runat="server" Text="下一项" OnClick="ImgBtnright_Click" CssClass="circle-btn circle-btn--ghost" />
                    <asp:Label ID="Labelnum" runat="server" Font-Names="Arial" Font-Size="9pt"></asp:Label>
                </div>
                <div id="stuname" class="circle-student-name">
                    <asp:Label ID="Labelname" runat="server"></asp:Label>
                </div>
            </div>
            <asp:Label ID="lbcurindex" runat="server" Text="0" Visible="False"></asp:Label>
        </section>

        <section class="circle-panel">
            <div class="circle-filters">
                <asp:Button ID="ImgBtnTextbox" runat="server" CommandName="v" Text="隐藏评语" OnClick="ImgBtnTextbox_Click" CssClass="circle-btn circle-btn--ghost" />
                <span>教师评语：</span>
                <asp:TextBox ID="TextBoxWself" runat="server" BorderColor="Silver" BorderStyle="Dashed" BorderWidth="1px" BackColor="#FDF5E3" CssClass="circle-input"></asp:TextBox>
                <asp:Image ID="Image2" runat="server" ImageUrl="~/images/token.png" />
                <span>加分：</span>
                <asp:TextBox ID="TextBoxWdsocre" runat="server" MaxLength="2" Width="40px" BackColor="#FDF5E3" SkinID="TextBoxNum" Height="19px" CssClass="circle-select">0</asp:TextBox>
                <asp:RadioButtonList ID="RBLselect" runat="server" RepeatDirection="Horizontal" Visible="True" Font-Size="16pt" AutoPostBack="True" onselectedindexchanged="RBLselect_SelectedIndexChanged" RepeatLayout="Flow" CellPadding="0" CellSpacing="18" Width="240px">
                    <Items>
                        <asp:ListItem>G</asp:ListItem>
                        <asp:ListItem>A</asp:ListItem>
                        <asp:ListItem>B</asp:ListItem>
                        <asp:ListItem>C</asp:ListItem>
                        <asp:ListItem>D</asp:ListItem>
                        <asp:ListItem>E</asp:ListItem>
                        <asp:ListItem>O</asp:ListItem>
                    </Items>
                </asp:RadioButtonList>
                <asp:Button ID="ImgBtn" runat="server" Text="刷新展播" OnClick="ImgBtn_Click" ToolTip="循环展播专用刷新" CssClass="circle-btn circle-btn--secondary" />
                <asp:Button ID="BtnCheck" runat="server" Text="标记已评" OnClick="BtnCheck_Click" ToolTip="将自动得分作品设置为已评" CssClass="circle-btn circle-btn--primary" />
                <img id="showname" src="../images/help.png"  alt="显示姓名"/>
                <asp:DropDownList ID="DDLname" runat="server" AutoPostBack="True" Width="60px" onselectedindexchanged="DDLname_SelectedIndexChanged" CssClass="circle-select">
                    <asp:ListItem></asp:ListItem>
                </asp:DropDownList>
                <asp:CheckBox ID="CkselectG" runat="server" Text="筛Ｇ评" ToolTip="推荐作品筛选" AutoPostBack="True" oncheckedchanged="CkselectG_CheckedChanged" />
                <asp:CheckBox ID="CheckselectA" runat="server" Text="筛A评" ToolTip="优秀作品筛选" AutoPostBack="True" oncheckedchanged="CheckselectA_CheckedChanged" />
                <asp:CheckBox ID="CheckBoxW" runat="server" Text="筛未评" ToolTip="未评作品筛选" AutoPostBack="True" oncheckedchanged="CheckBoxW_CheckedChanged" />
                <asp:Button ID="ImageBtnDel" runat="server" Text="删除作品" OnClick="ImageBtnDel_Click" ToolTip="删除作品" CssClass="circle-btn circle-btn--danger" />
            </div>
        </section>

        <section class="circle-viewer">
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
        </section>

        <section class="circle-panel">
            <asp:HyperLink ID="Hlcode" runat="server" Font-Size="11pt" Target="_blank" Visible="False" CssClass="circle-btn">查看脚本</asp:HyperLink>
        </section>
    </div>
    <script type="text/javascript">
        window.__circleshowConfig = {
            btnstopId: "<%= Btnstop.ClientID %>",
            imgBtnId: "<%= ImgBtn.ClientID %>"
        };
    </script>
    <script type="text/javascript" src="../js/circleshow.js"></script>
    </form>
    
</body>
</html>
