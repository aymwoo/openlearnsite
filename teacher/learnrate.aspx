<%@ Page Language="C#" AutoEventWireup="true" CodeFile="learnrate.aspx.cs" Inherits="teacher_learnrate" ResponseEncoding="utf-8" Culture="zh-CN" UICulture="zh-CN" %>

<%@ Register assembly="Anthem" namespace="Anthem" tagprefix="anthem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    
    <link rel="stylesheet" type="text/css" href="../App_Themes/Teacher/learnrate.css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="lr-page">
        <div class="lr-shell">
            <div class="lr-hero">
                <div class="lr-hero__row">
                    <div>
                        <h1 class="lr-title">学习进度</h1>
                        <p class="lr-subtitle">按学案项目查看班级学习节奏，颜色越深表示用时越长。</p>
                    </div>
                    <div class="lr-toolbar">
                        <span class="lr-toolbar-label"><asp:Label ID="LabelGradeClass" runat="server"></asp:Label></span>
                        <asp:DropDownList ID="DDLCid" runat="server" CssClass="lr-select" AutoPostBack="True"
                            onselectedindexchanged="DDLCid_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>

            <div class="lr-card">
                <div class="lr-card__head">
                    <div>
                        <h2 class="lr-card__title">班级进度热力表</h2>
                        <p class="lr-card__desc">绿色较浅表示完成较快，绿色较深表示停留更久。</p>
                    </div>
                    <asp:Button ID="BtnreflashText" runat="server" Text="刷新数据" CssClass="lr-refresh" OnClick="BtnreflashText_Click" />
                </div>
                <div class="lr-grid-wrap">
                    <anthem:GridView ID="GridViewclass" runat="server" OnRowDataBound="GridViewclass_RowDataBound"
                        TabIndex="1" CellPadding="2" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None"
                        BorderWidth="2px" Font-Names="Arial" HorizontalAlign="Center"
                        EnableModelValidation="True">
                        <RowStyle HorizontalAlign="Center" BorderStyle="None" />
                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#305E9C" Font-Bold="True" ForeColor="White" />
                    </anthem:GridView>
                </div>
                <div class="lr-msg">
                    <asp:Label ID="Labelmsg" runat="server"></asp:Label>
                </div>
            </div>

            <asp:Button ID="Btnreflash" runat="server" Text="刷新" OnClick="Btnreflash_Click" Style="display:none;" />
        </div>
    </div>

    
    <script type="text/javascript">
        window.__learnrateConfig = {
            btnreflashId: "<%= Btnreflash.ClientID %>"
        };
    </script>
    <script type="text/javascript" src="../js/learnrate.js"></script>
    </form>
</body>
</html>
