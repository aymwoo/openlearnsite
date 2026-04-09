<%@ Page Language="C#"  StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="workcheck.aspx.cs" Inherits="Teacher_workcheck" ResponseEncoding="utf-8" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
        <meta charset="utf-8" />
<title>作品展示</title>
    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    
    <link rel="stylesheet" type="text/css" href="../App_Themes/Teacher/workcheck.css" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:Image ID="Imagelogo" runat="server" ImageUrl="~/images/learnsite.gif" Height="1px" style="display:none" />

    <div class="ws-shell">
        <!-- Control Bar -->
        <div class="ws-control-bar">
            <div class="ws-control-title">
                <asp:Label ID="Labelshow" runat="server"></asp:Label>
                <asp:DropDownList ID="DDLclass" runat="server" Font-Size="9pt"
                    Width="60px" AutoPostBack="True" Font-Bold="True"
                    onselectedindexchanged="DDLclass_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Label ID="Labeltxt" runat="server"></asp:Label>
            </div>

            <!-- Course & Activity selectors -->
            <div class="ws-control-row">
                <span class="ws-control-label">学案名称：</span>
                <asp:Label ID="Labeltitle" runat="server" Font-Bold="False"></asp:Label>
                <span class="ws-control-label">活动选择：</span>
                <asp:DropDownList ID="DDLmid" runat="server" AutoPostBack="True"
                    onselectedindexchanged="DDLmid_SelectedIndexChanged">
                </asp:DropDownList>
            </div>

            <!-- Stats & Legend -->
            <div class="ws-control-row">
                <span class="ws-legend">
                    <asp:Label ID="Labelcolor" runat="server" BackColor="#CDE2FE" Width="12px" Height="12px" CssClass="ws-legend-dot"></asp:Label>
                    作品标志
                </span>
                <span class="ws-legend">
                    <asp:Label ID="Labelscore" runat="server" BackColor="#FFCC99" Width="12px" Height="12px" CssClass="ws-legend-dot"></asp:Label>
                    评价等级
                </span>
                <span class="ws-stat-badge">
                    作品总数：<asp:Label ID="Labelcounts" runat="server"></asp:Label>
                </span>
                <asp:Label ID="Labelmsg" runat="server" CssClass="ws-grade-dist"></asp:Label>
                <asp:Image ID="ImageType" runat="server" style="height:20px" />
                <asp:Button ID="ImgBtnFlasherror" runat="server"
                    Text="清除异常" OnClick="ImgBtnFlasherror_Click"
                    ToolTip="Office文档转换异常标志清除重新转换" CssClass="ws-btn ws-btn-danger" />
            </div>

            <!-- Actions -->
            <div class="ws-control-row">
                <div class="ws-actions">
                    <asp:Button ID="BtnCheck" runat="server" Text="批量设为已评" OnClick="BtnCheck_Click"
                        ToolTip="将本班自动得分作品全部设置为已评"
                        CssClass="ws-btn ws-btn-warning" />
                    <asp:Button ID="BtnA" runat="server" Text="一键评A" SkinID="BtnSmall"
                        onclick="BtnA_Click" ToolTip="将本班该活动未评的作品，全部评为A"
                        CssClass="ws-btn ws-btn-primary" />
                    <asp:Button ID="BtnB" runat="server" Text="一键评B" SkinID="BtnSmall"
                        onclick="BtnB_Click" ToolTip="将本班该活动未评的作品，全部评为B"
                        CssClass="ws-btn ws-btn-primary" />
                    <asp:Button ID="BtnCk" runat="server" Text="一键已评" SkinID="BtnSmall"
                        onclick="BtnCk_Click" ToolTip="不用给分的作品，一健全评为０"
                        CssClass="ws-btn ws-btn-success" />
                    <asp:Button ID="BtnWp" runat="server" Text="一键未评" SkinID="BtnSmall"
                        onclick="BtnWp_Click" ToolTip="所有作品一键未评"
                        CssClass="ws-btn ws-btn-neutral" />
                    <asp:HyperLink ID="HLautoplay" runat="server" Target="_blank"
                        ToolTip="个人作品自动展播"
                        CssClass="ws-btn ws-btn-success">[HLautoplay]</asp:HyperLink>
                    <asp:HyperLink ID="HLgroupplay" runat="server" Target="_blank"
                        ToolTip="小组作品自动展播"
                        CssClass="ws-btn ws-btn-info">[HLgroupplay]</asp:HyperLink>
                    <asp:Button ID="Btnreturn" runat="server" Text="关闭窗口" SkinID="BtnSmall"
                        CssClass="ws-btn ws-btn-danger" />
                </div>
            </div>

            <!-- Sort -->
            <div class="ws-control-row">
                <span class="ws-control-label">排序：</span>
                <div class="ws-sort-group">
                    <asp:RadioButtonList ID="RBsort" runat="server" AutoPostBack="True"
                        Font-Size="9pt" onselectedindexchanged="RBsort_SelectedIndexChanged"
                        RepeatDirection="Horizontal" RepeatLayout="Flow">
                        <Items>
                            <asp:ListItem Value="0" Selected="True">时间排序</asp:ListItem>
                            <asp:ListItem Value="1">学号排序</asp:ListItem>
                            <asp:ListItem Value="2">IP 排序</asp:ListItem>
                            <asp:ListItem Value="3">小组排序</asp:ListItem>
                            <asp:ListItem Value="4">投票排序</asp:ListItem>
                        </Items>
                    </asp:RadioButtonList>
                </div>
            </div>
        </div>

        <!-- Individual Works -->
        <div class="ws-section-title">个人作品</div>
        <div class="ws-cards-grid">
            <asp:DataList ID="DataListworks" runat="server" RepeatDirection="Horizontal"
                RepeatColumns="8" DataKeyField="Wid" CellPadding="0" CellSpacing="0"
                onitemdatabound="DataListworks_ItemDataBound"
                onitemcommand="DataListworks_ItemCommand">
                <ItemStyle CssClass="m-1" />
                <ItemTemplate>
                    <div class="divscore">
                        <div>
                            <asp:HyperLink ID="HyperLink1" runat="server" Text='<%# Eval("Sname") %>'
                                ToolTip='<%# HttpUtility.HtmlDecode(Eval("Wself").ToString()) %>'
                                Target="_blank" CssClass="workname"></asp:HyperLink>
                            <asp:CheckBox ID="CB" runat="server" Checked='<%# Eval("Wcheck") %>'
                                EnableTheming="True"
                                ToolTip="评价状态：取消则评分为0并可重新提交，选中则初始评分为0并不可重新提交"
                                oncheckedchanged="CB_CheckedChanged" AutoPostBack="True" BorderStyle="None" />
                        </div>
                        <div>
                            <asp:Label ID="Wv" runat="server" Text='<%# Eval("Wvote") %>' ToolTip="票数"></asp:Label>&nbsp;
                            <asp:Label ID="Wf" runat="server" Text='<%# Eval("Wfscore") %>' ToolTip="互评"></asp:Label>&nbsp;
                            <asp:Label ID="Wl" runat="server" Text='<%# Eval("Wlscore") %>' ToolTip="组评" ForeColor="#0066FF"></asp:Label>
                            <asp:HyperLink ID="Hlflash" runat="server" Height="12px" Target="_blank"
                                ImageUrl="~/images/flashview.png" ToolTip="Flash格式预览" Visible="False"></asp:HyperLink>
                        </div>
                        <div>
                            <asp:LinkButton ID="LG" runat="server" CommandArgument="Wid" CommandName="G" ToolTip="收藏12分" CssClass="wscored wscored-g">G</asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="LA" runat="server" CommandArgument="Wid" CommandName="A" ToolTip="优秀10分" CssClass="wscored wscored-a">A</asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="LB" runat="server" CommandArgument="Wid" CommandName="B" ToolTip="良好8分" CssClass="wscored wscored-b">B</asp:LinkButton>
                        </div>
                        <div>
                            <asp:LinkButton ID="LC" runat="server" CommandArgument="Wid" CommandName="C" ToolTip="一般6分" CssClass="wscored wscored-c">C</asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="LD" runat="server" CommandArgument="Wid" CommandName="D" ToolTip="落后4分" CssClass="wscored wscored-d">D</asp:LinkButton>
                            &nbsp;<asp:LinkButton ID="LE" runat="server" CommandArgument="Wid" CommandName="E" ToolTip="不及格2分" CssClass="wscored wscored-e">E</asp:LinkButton>
                        </div>
                        <asp:Label ID="Labelscore" runat="server" Text='<%# Eval("Wscore") %>' Visible="False"></asp:Label>
                        <asp:Label ID="Labelurl" runat="server" Text='<%# Eval("Wurl") %>' Visible="False"></asp:Label>
                        <asp:Label ID="Labelwid" runat="server" Text='<%# Eval("Wid") %>' Visible="False"></asp:Label>
                        <asp:CheckBox ID="Checkwflash" runat="server" Checked='<%# Eval("Wflash") %>' Visible="False" />
                        <asp:CheckBox ID="Checkwerror" runat="server" Checked='<%# Eval("Werror") %>' Visible="False" />
                        <asp:Label ID="Labelwlemotion" runat="server" Text='<%# Eval("Wlemotion") %>' Visible="False"></asp:Label>
                    </div>
                </ItemTemplate>
            </asp:DataList>
        </div>

        <!-- Group Works -->
        <div class="ws-group-section">
            <div class="ws-section-title">小组作品</div>
            <div class="ws-cards-grid">
                <asp:DataList ID="DataListgroup" runat="server" RepeatDirection="Horizontal"
                    RepeatColumns="6" DataKeyField="Gid" CellPadding="0" CellSpacing="0"
                    onitemcommand="DataListgroup_ItemCommand"
                    onitemdatabound="DataListgroup_ItemDataBound" Caption="小组作品">
                    <ItemStyle CssClass="m-1" />
                    <ItemTemplate>
                        <div class="divgroupscore">
                            <div>
                                <asp:HyperLink ID="HyperLinkg1" runat="server" Text='<%# Eval("Sgtitle") %>'
                                    ToolTip='<%# Eval("Gnote") %>' Target="_blank" CssClass="groupname"></asp:HyperLink>
                            </div>
                            <asp:Label ID="Wvg" runat="server" Text='<%# Eval("Gvote") %>' ToolTip="票数"></asp:Label>
                            <asp:CheckBox ID="CBg" runat="server" AutoPostBack="True" BorderStyle="None"
                                Checked='<%# Eval("Gcheck") %>' EnableTheming="True"
                                oncheckedchanged="CBg_CheckedChanged"
                                ToolTip="评价状态：取消则评分为0并可重新提交，选中则初始评分为0并不可重新提交" />
                            <div>
                                <asp:LinkButton ID="L20" runat="server" CommandArgument="Gid" CommandName="20" ToolTip="20学分" CssClass="wscored wscored-a">A+</asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="L19" runat="server" CommandArgument="Gid" CommandName="19" ToolTip="19学分" CssClass="wscored wscored-a">A</asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="L18" runat="server" CommandArgument="Gid" CommandName="18" ToolTip="18学分" CssClass="wscored wscored-a">A-</asp:LinkButton>
                            </div>
                            <div>
                                <asp:LinkButton ID="L17" runat="server" CommandArgument="Gid" CommandName="17" ToolTip="17学分" CssClass="wscored wscored-b">B+</asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="L16" runat="server" CommandArgument="Gid" CommandName="16" ToolTip="16学分" CssClass="wscored wscored-b">B</asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="L15" runat="server" CommandArgument="Gid" CommandName="15" ToolTip="15学分" CssClass="wscored wscored-b">B-</asp:LinkButton>
                            </div>
                            <div>
                                <asp:LinkButton ID="L14" runat="server" CommandArgument="Gid" CommandName="14" ToolTip="14学分" CssClass="wscored wscored-c">C+</asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="L13" runat="server" CommandArgument="Gid" CommandName="13" ToolTip="13学分" CssClass="wscored wscored-c">C</asp:LinkButton>
                                &nbsp;<asp:LinkButton ID="L12" runat="server" CommandArgument="Gid" CommandName="12" ToolTip="12学分" CssClass="wscored wscored-c">C-</asp:LinkButton>
                            </div>
                            <asp:Label ID="Labelgscore" runat="server" Text='<%# Eval("Gscore") %>' Visible="False"></asp:Label>
                            <asp:Label ID="Labelgurl" runat="server" Text='<%# Eval("Gurl") %>' Visible="False"></asp:Label>
                            <asp:Label ID="Labelgid" runat="server" Text='<%# Eval("Gid") %>' Visible="False"></asp:Label>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
            </div>
        </div>

        <!-- Not Submitted -->
        <div class="ws-nowork-section">
            <div class="ws-nowork-title">未提交作品学生列表</div>
            <div class="ws-nowork-grid">
                <asp:DataList ID="DataListNoworks" runat="server" RepeatDirection="Horizontal"
                    RepeatColumns="8" CellPadding="0" CellSpacing="0">
                    <ItemTemplate>
                        <span style="display:inline-block;background:#fef3c7;border:1px solid #fde68a;border-radius:6px;padding:2px 8px;font-size:0.8rem;color:#92400e;margin:2px;"
                            title='<%# Eval("Sscore") %>'><%# Eval("Sname") %></span>
                    </ItemTemplate>
                </asp:DataList>
            </div>
        </div>

        <!-- Refresh -->
        <div class="ws-refresh-note">
            <asp:Button ID="Btnreflash" runat="server" Text="立即刷新"
                OnClick="Btnreflash_Click"
                CssClass="ws-btn ws-btn-neutral" />
            每30秒自动刷新
        </div>
    </div>

    
    <script type="text/javascript">
        window.__workcheckConfig = {
            btnreflashId: "<%= Btnreflash.ClientID %>"
        };
    </script>
    <script type="text/javascript" src="../js/workcheck.js"></script>
    </form>
</body>
</html>
