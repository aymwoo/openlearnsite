<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manage.master" AutoEventWireup="true" CodeFile="backup.aspx.cs" Inherits="Manager_backup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <style type="text/css">
        .mgr-page { --ls-bg: linear-gradient(180deg,#f8fbff 0%,#f3f7ff 100%); --ls-border: #dbe6f5; --ls-text: #0f172a; --ls-muted: #64748b; padding: 28px; background: var(--ls-bg); min-height: calc(100vh - 8rem); box-sizing: border-box; width: 100%; }
        .mgr-page * { box-sizing: border-box; }
        .mgr-shell { display: flex; flex-direction: column; gap: 20px; }
        .mgr-hero { border: 1px solid #c7d2fe; border-radius: 1rem; padding: 24px 28px; background: linear-gradient(135deg,#eef2ff 0%,#e0e7ff 100%); color: #312e81; box-shadow: 0 4px 16px rgba(99,102,241,.08); }
        .mgr-hero__title { margin: 0; font-size: 22px; font-weight: 800; display: flex; align-items: center; gap: 10px; }
        .mgr-hero__subtitle { margin: 6px 0 0; font-size: 14px; color: rgba(238,242,255,.85); }
        .mgr-card { border: 1px solid var(--ls-border); border-radius: 1rem; background: rgba(255,255,255,.96); box-shadow: 0 12px 30px rgba(15,23,42,.05); overflow: hidden; }
        .mgr-card__head { padding: 20px 24px; border-bottom: 1px solid #f1f5f9; display: flex; align-items: center; justify-content: space-between; gap: 12px; }
        .mgr-card__title { margin: 0; font-size: 16px; font-weight: 800; color: var(--ls-text); }
        .mgr-card__body { padding: 20px 24px; display: flex; flex-direction: column; gap: 16px; }
        .mgr-prose { font-size: 14px; line-height: 1.9; color: #334155; }
        .mgr-btn { display: inline-flex; align-items: center; justify-content: center; min-height: 40px; padding: 0 18px; border-radius: 1rem; border: none; font-size: 14px; font-weight: 700; cursor: pointer; transition: transform .18s, box-shadow .18s; }
        .mgr-btn--primary { background: linear-gradient(135deg,#4338ca 0%,#3730a3 100%); color: #fff; box-shadow: 0 8px 16px rgba(67,56,202,.2); }
        .mgr-btn--primary:hover { transform: translateY(-1px); }
        .mgr-backup-list { display: grid; grid-template-columns: repeat(auto-fill,minmax(280px,1fr)); gap: 12px; }
        .mgr-backup-item { display: flex; align-items: center; justify-content: space-between; gap: 10px; padding: 12px 16px; border: 1px solid #e2e8f0; border-radius: 1rem; background: linear-gradient(180deg,#fff 0%,#f8fafc 100%); }
        .mgr-backup-item__info { display: flex; flex-direction: column; gap: 2px; min-width: 0; }
        .mgr-backup-item__name { font-size: 14px; font-weight: 700; color: #334155; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
        .mgr-backup-item__meta { font-size: 12px; color: #64748b; }
        .mgr-msg { font-size: 14px; font-weight: 700; color: #dc2626; }
    </style>
    <div class="mgr-page">
        <div class="mgr-shell">
            <div class="mgr-hero">
                <h1 class="mgr-hero__title"><i class="bi bi-database-fill-gear" style="color:#a5b4fc;"></i> 数据库备份与恢复</h1>
                <p class="mgr-hero__subtitle">定期备份数据库，保障平台数据安全</p>
            </div>

            <div class="mgr-card">
                <div class="mgr-card__head">
                    <h2 class="mgr-card__title">操作说明</h2>
                </div>
                <div class="mgr-card__body">
                    <div class="mgr-prose">
                        数据库备份以当前短日期+时+分为文件名保存到网站下 BackupDb 文件夹中。<b>注意：恢复前一定要备份当前的数据库后再进行，并自行妥善保存好备份的数据库。</b><br /><br />
                        恢复功能的使用条件：网站的数据库的账号和密码跟 master 一致，并且要恢复的备份跟原数据库名称相同，否则就会出错。如果不一致，请参考说明必读中的还原数据库截图演示图片，进行手工恢复。<br /><br />
                        <b>网站迁移说明：</b>将原网站文件夹复制到新电脑的（除C盘外）分区中，去只读并加 everyone 可读写权限，并将数据库文件复制过去附加到数据库服务器中，同时修改 web.config 中的数据库连接字符串。
                    </div>
                </div>
            </div>

            <div class="mgr-card">
                <div class="mgr-card__head">
                    <h2 class="mgr-card__title">备份列表</h2>
                    <asp:Button ID="Btnbackup" runat="server" Text="立即备份" onclick="Btnbackup_Click" CssClass="mgr-btn mgr-btn--primary" />
                </div>
                <div class="mgr-card__body">
                    <div class="mgr-backup-list">
                        <asp:DataList ID="DlDbBackup" runat="server" RepeatColumns="1" RepeatDirection="Vertical"
                            CellPadding="0" CellSpacing="0"
                            onitemdatabound="DlDbBackup_ItemDataBound"
                            onitemcommand="DlDbBackup_ItemCommand">
                            <ItemTemplate>
                                <div class="mgr-backup-item">
                                    <div class="mgr-backup-item__info">
                                        <asp:HyperLink ID="HLfname" runat="server" Target="_blank" Text='<%# Eval("fname") %>' CssClass="mgr-backup-item__name"></asp:HyperLink>
                                        <span class="mgr-backup-item__meta">
                                            <asp:Label ID="Labelfsize" runat="server" Text='<%# Eval("fsize") %>'></asp:Label>
                                            <asp:Label ID="Labelfread" runat="server" Text='<%# Eval("fread") %>' ToolTip="是否只读（T：只读 | F：可写）" style="color:#16a34a;margin-left:6px;"></asp:Label>
                                        </span>
                                    </div>
                                    <asp:Label ID="Labelfid" runat="server" Text='<%# Eval("fid") %>' style="display:none;"></asp:Label>
                                    <asp:Label ID="Labelurl" runat="server" Text='<%# Eval("furl") %>' Visible="false"></asp:Label>
                                    <asp:ImageButton ID="ImgBtnReStore" runat="server" CommandName="ReStore"
                                        ImageUrl="~/images/works.gif" ToolTip="将当前数据库恢复到该备份日期状态"
                                        CommandArgument='<%# Eval("furl") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                    <div id="Loading" style="display:none;color:#dc2626;font-size:14px;">
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/images/load2.gif" />
                        <input id="Textcmd" style="border:none;background:transparent;" type="text" />
                    </div>
                    <asp:Label ID="Labelmsg" runat="server" CssClass="mgr-msg"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
