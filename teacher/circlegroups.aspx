<%@ Page Language="C#" AutoEventWireup="true" StylesheetTheme="Teacher"  CodeFile="circlegroups.aspx.cs" Inherits="Teacher_circlegroups" ResponseEncoding="utf-8" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
        <meta charset="utf-8" />
<title>小组作品展示</title>
    <style type="text/css">
        body.group-circle-page {
            margin: 0;
            background: linear-gradient(180deg, #f8fafc 0%, #eef6ff 100%);
            font-family: Arial, "Microsoft YaHei", sans-serif;
            color: #0f172a;
        }

        .group-shell {
            padding: 1rem;
        }

        .group-hero,
        .group-panel,
        .group-viewer {
            background: rgba(255, 255, 255, 0.92);
            border: 1px solid rgba(148, 163, 184, 0.18);
            border-radius: 1rem;
            box-shadow: 0 12px 28px -24px rgba(15, 23, 42, 0.35);
            margin-bottom: 1rem;
        }

        .group-hero {
            padding: 1.25rem 1.5rem;
            background: linear-gradient(135deg, #0f766e 0%, #0f9b8e 55%, #22c55e 100%);
            color: #eff6ff;
        }

        .group-title {
            margin: 0;
            font-size: 1.4rem;
            font-weight: 800;
        }

        .group-panel {
            padding: 1rem 1.25rem;
            text-align: center;
        }

        .group-links {
            display: flex;
            flex-wrap: wrap;
            gap: 0.5rem;
            justify-content: center;
        }

        .group-chip {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            min-height: 2.4rem;
            padding: 0 0.9rem;
            border-radius: 0.75rem;
            background: #eef6ff;
            border: 1px solid #bfdbfe;
            color: #1d4ed8;
            text-decoration: none;
            font-weight: 700;
        }

        .group-btn {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            min-height: 2.5rem;
            padding: 0 1rem;
            border: 0;
            border-radius: 0.75rem;
            background: #2563eb;
            color: #ffffff;
            font-weight: 700;
            cursor: pointer;
        }

        .group-btn--secondary {
            background: #eff6ff;
            color: #1d4ed8;
            border: 1px solid #bfdbfe;
        }

        .group-select {
            min-height: 2.5rem;
            padding: 0 0.85rem;
            border: 1px solid #cbd5e1;
            border-radius: 0.75rem;
            background: #ffffff;
        }

        .group-viewer {
            min-height: 70vh;
            padding: 0.75rem;
            overflow: auto;
            text-align: center;
        }
    </style>
</head>
<body class="group-circle-page">
    <form id="form1" runat="server">
    <div class="group-shell">
        <section class="group-hero">
            <h1 class="group-title"><asp:Label  ID="LabeMtitle" runat="server" Font-Bold="True" Font-Size="10pt"></asp:Label></h1>
        </section>

        <section class="group-panel">
            <asp:DataList ID="DLgroups" runat="server" RepeatDirection="Horizontal"
                RepeatLayout="Flow" CellPadding="3" RepeatColumns="6"
                onitemdatabound="DLgroups_ItemDataBound" CellSpacing="3"
                onitemcommand="DLgroups_ItemCommand">
                <ItemTemplate>
                    <asp:LinkButton ID="LbSgtitle" runat="server" Text='<%# Eval("Sgtitle") %>' CommandName="S" CssClass="group-chip"></asp:LinkButton>
                    <asp:Label ID="LabelSid" runat="server" Text='<%# Eval("Sid") %>' Visible="false"></asp:Label>
                    <asp:Label ID="LabelSname" runat="server" Text='<%# Eval("Sname") %>' Visible="false"></asp:Label>
                    <asp:Label ID="LabelSnum" runat="server" Text='<%# Eval("Snum") %>' Visible="false"></asp:Label>
                </ItemTemplate>
            </asp:DataList>
            <br />
            <asp:Label ID="Labelgid" runat="server" Visible="False"></asp:Label>
            <asp:Button ID="ImgBtnrefresh" runat="server" Text="轮播下一组" OnClick="ImgBtnrefresh_Click" CssClass="group-btn group-btn--secondary" />
            <asp:Label ID="Labelpos" runat="server" Text="0" Visible="False"></asp:Label>
            <asp:Label ID="Labellastpos" runat="server" Text="0" Visible="False"></asp:Label>
            <br />
            <img alt="组长" src="../images/gflag.gif" /><asp:Label ID="LabelSgtitle" runat="server" Font-Bold="True" ForeColor="#0066FF"></asp:Label>
            &nbsp;组长：<asp:Label ID="LabelLeader" runat="server" Font-Bold="False" ForeColor="#0066FF"></asp:Label>
            &nbsp;成员：<asp:Label ID="Labelmember" runat="server" ForeColor="#3399FF"></asp:Label>
            &nbsp;学分
            <asp:DropDownList ID="DDLGscores" runat="server" AutoPostBack="True" Font-Size="9pt" onselectedindexchanged="DDLGscores_SelectedIndexChanged" Width="40px" Font-Bold="False" Font-Names="Arial" ForeColor="#336600" CssClass="group-select">
                <asp:ListItem Value="20">A+</asp:ListItem>
                <asp:ListItem Value="19">A</asp:ListItem>
                <asp:ListItem Value="18">A-</asp:ListItem>
                <asp:ListItem Value="17">B+</asp:ListItem>
                <asp:ListItem Value="16">B</asp:ListItem>
                <asp:ListItem Value="15">B-</asp:ListItem>
                <asp:ListItem Value="14">C+</asp:ListItem>
                <asp:ListItem Value="13">C</asp:ListItem>
                <asp:ListItem Value="12">C-</asp:ListItem>
                <asp:ListItem Value="0">0</asp:ListItem>
            </asp:DropDownList>
            <br />
            <br />
            <asp:Button ID="ImgBtnLeft" runat="server" Text="上一组" OnClick="ImgBtnLeft_Click" CssClass="group-btn group-btn--secondary" />
            &nbsp;<asp:Button ID="BtnCicle" runat="server" onclick="BtnCicle_Click" Text="播放" CssClass="group-btn" />
            &nbsp;<asp:Button ID="ImgBtnright" runat="server" Text="下一组" OnClick="ImgBtnright_Click" CssClass="group-btn group-btn--secondary" />
            <script type ="text/javascript" >
                function myrefresh() {
                    var stxt = document.getElementById("<%= BtnCicle.ClientID %>").value;
                    if (stxt == "暂停") {
                        document.getElementById("<%= ImgBtnrefresh.ClientID %>").click();
                    }
                }
                setTimeout("myrefresh()", 8000);
            </script>
        </section>

        <section class="group-viewer">
            <asp:Literal ID="LiteralView" runat="server"></asp:Literal>
        </section>
    </div>
    </form>
</body>
</html>
