<%@ Page Language="C#" AutoEventWireup="true" StylesheetTheme="Teacher" CodeFile="softnomic.aspx.cs" Inherits="Teacher_softnomic" ResponseEncoding="utf-8" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
        <meta charset="utf-8" />
<title>自学园作品评价与展示</title>
    
    <link rel="stylesheet" type="text/css" href="../App_Themes/Teacher/softnomic.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="nomic-page">
            <div class="nomic-hero">
                <h1 class="nomic-hero__title">自学作品评价</h1>
                <p class="nomic-hero__subtitle">浏览学生自学作品，进行评分和评语反馈。</p>
            </div>

            <div class="nomic-card nomic-card--filter">
                <div class="nomic-card__head">
                    <h2 class="nomic-card__title">资源筛选</h2>
                </div>
                <div class="nomic-card__body">
                    <div class="nomic-filter-grid">
                        <div class="nomic-field">
                            <span class="nomic-label">资源分类</span>
                            <asp:DropDownList ID="DDLCategory" runat="server"
                                AutoPostBack="True" CssClass="nomic-select"
                                onselectedindexchanged="DDLCategory_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="nomic-field">
                            <span class="nomic-label">资源标题</span>
                            <asp:DropDownList ID="DDLsoft" runat="server"
                                AutoPostBack="True" CssClass="nomic-select"
                                onselectedindexchanged="DDLsoft_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>

            <div class="nomic-card nomic-card--controls">
                <div class="nomic-card__head">
                    <h2 class="nomic-card__title">播放控制与评分</h2>
                </div>
                <div class="nomic-card__body">
                    <div class="nomic-toolbar">
                        <asp:Button ID="Btnflash" runat="server" Text="刷新"
                            onclick="Btnflash_Click" SkinID="BtnSmall"
                            CssClass="nomic-btn nomic-btn--primary" />
                        <asp:Button ID="Btnrestart" runat="server" Text="重新"
                            onclick="Btnrestart_Click" SkinID="BtnSmall"
                            CssClass="nomic-btn nomic-btn--ghost" />
                        <asp:Button ID="Btnstop" runat="server" Text="继续"
                            onclick="Btnstop_Click" SkinID="BtnSmall"
                            CssClass="nomic-btn nomic-btn--ghost" />

                        <div class="nomic-divider"></div>

                        <asp:Button ID="ImgBtnLeft" runat="server"
                            Text="上一项" OnClick="ImgBtnLeft_Click"
                            CssClass="nomic-nav-btn" />
                        <asp:DropDownList ID="DDLstore" runat="server"
                            AutoPostBack="True" CssClass="nomic-student-select"
                            onselectedindexchanged="DDLstore_SelectedIndexChanged">
                            <asp:ListItem></asp:ListItem>
                        </asp:DropDownList>
                        <asp:Button ID="ImgBtnright" runat="server"
                            Text="下一项" OnClick="ImgBtnright_Click"
                            CssClass="nomic-nav-btn" />
                        <asp:Label ID="Labelnum" runat="server" CssClass="nomic-counter"></asp:Label>
                    </div>

                    <asp:Label ID="lbcurindex" runat="server" Text="0" CssClass="nomic-hidden"></asp:Label>

                    <div class="nomic-score-row">
                        <span class="nomic-label">教师评语</span>
                        <asp:TextBox ID="TextBoxWself" runat="server" CssClass="nomic-comment-input"></asp:TextBox>
                        <asp:RadioButtonList ID="RBLselect" runat="server" RepeatDirection="Horizontal"
                            AutoPostBack="True" RepeatLayout="Flow" CssClass="nomic-grade-group"
                            onselectedindexchanged="RBLselect_SelectedIndexChanged">
                            <asp:ListItem>G</asp:ListItem>
                            <asp:ListItem>A</asp:ListItem>
                            <asp:ListItem>B</asp:ListItem>
                            <asp:ListItem>C</asp:ListItem>
                            <asp:ListItem>D</asp:ListItem>
                            <asp:ListItem>E</asp:ListItem>
                            <asp:ListItem>O</asp:ListItem>
                        </asp:RadioButtonList>
                        <span class="nomic-check-group">
                            <asp:CheckBox ID="CkFlash" runat="server"
                                oncheckedchanged="CkFlash_CheckedChanged" Text="FlashLoop"
                                ToolTip="Flash播放循环设置" AutoPostBack="True" />
                        </span>
                        <asp:Button ID="Btndel" runat="server" Text="删除"
                            onclick="Btndel_Click" SkinID="BtnSmall"
                            ToolTip="删除该作品，不可恢复！" CssClass="nomic-btn nomic-btn--danger" />
                    </div>
                </div>
            </div>

            <div class="nomic-card nomic-card--preview">
                <div class="nomic-card__head">
                    <h2 class="nomic-card__title">作品预览</h2>
                </div>
                <div class="nomic-card__body">
                    <div class="nomic-preview">
                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                    </div>
                    <div style="margin-top: 12px; text-align: center;">
                        <asp:Button ID="ImgBtn" runat="server" Text="刷新展播"
                            OnClick="ImgBtn_Click" ToolTip="循环展播专用刷新" CssClass="nomic-refresh" />
                    </div>
                </div>
            </div>
        </div>

        
    <script type="text/javascript">
        window.__softnomicConfig = {
            btnstopId: "<%= Btnstop.ClientID %>",
            imgBtnId: "<%= ImgBtn.ClientID %>"
        };
    </script>
    <script type="text/javascript" src="../js/softnomic.js"></script>
    </form>
</body>
</html>
