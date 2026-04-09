<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manage.master" AutoEventWireup="true" CodeFile="upgrade.aspx.cs" Inherits="Manager_upgrade" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <style type="text/css">
        .mgr-page { --ls-bg: linear-gradient(180deg,#f8fbff 0%,#f3f7ff 100%); --ls-border: #dbe6f5; --ls-text: #0f172a; padding: 28px; background: var(--ls-bg); min-height: calc(100vh - 8rem); box-sizing: border-box; width: 100%; }
        .mgr-page * { box-sizing: border-box; }
        .mgr-shell { display: flex; flex-direction: column; gap: 20px; }
        .mgr-hero { border: 1px solid #fecaca; border-radius: 1rem; padding: 24px 28px; background: linear-gradient(135deg,#fff1f2 0%,#fee2e2 100%); color: #7f1d1d; box-shadow: 0 4px 16px rgba(220,38,38,.08); }
        .mgr-hero__title { margin: 0; font-size: 22px; font-weight: 800; display: flex; align-items: center; gap: 10px; }
        .mgr-hero__subtitle { margin: 6px 0 0; font-size: 14px; color: rgba(255,241,242,.85); }
        .mgr-card { border: 1px solid var(--ls-border); border-radius: 1rem; background: rgba(255,255,255,.96); box-shadow: 0 12px 30px rgba(15,23,42,.05); overflow: hidden; }
        .mgr-card__head { padding: 20px 24px; border-bottom: 1px solid #f1f5f9; }
        .mgr-card__title { margin: 0; font-size: 16px; font-weight: 800; color: var(--ls-text); }
        .mgr-card__body { padding: 24px; display: flex; flex-direction: column; gap: 16px; }
        .mgr-prose { font-size: 14px; line-height: 1.9; color: #334155; }
        .mgr-prose b { color: #0f172a; }
        .mgr-alert { padding: 14px 18px; border-radius: 1rem; background: #fef2f2; border: 1px solid #fecaca; font-size: 14px; color: #7f1d1d; line-height: 1.7; font-weight: 600; text-align: center; }
        .mgr-input-readonly { min-height: 40px; padding: 0 14px; border: 1px solid #e2e8f0; border-radius: .75rem; background: #f8fafc; color: #64748b; font-size: 14px; display: inline-flex; align-items: center; }
        .mgr-btn { display: inline-flex; align-items: center; justify-content: center; min-height: 44px; padding: 0 24px; border-radius: 1rem; border: none; font-size: 14px; font-weight: 700; cursor: pointer; transition: transform .18s, box-shadow .18s; }
        .mgr-btn--danger { background: linear-gradient(135deg,#dc2626 0%,#b91c1c 100%); color: #fff; box-shadow: 0 8px 16px rgba(220,38,38,.2); }
        .mgr-btn--danger:hover { transform: translateY(-1px); box-shadow: 0 10px 20px rgba(220,38,38,.3); }
        .mgr-msg { font-size: 14px; font-weight: 700; color: #dc2626; }
    </style>
    <div class="mgr-page">
        <div class="mgr-shell">
            <div class="mgr-hero">
                <h1 class="mgr-hero__title"><i class="bi bi-arrow-up-circle-fill" style="color:#fca5a5;"></i> 学年升班</h1>
                <p class="mgr-hero__subtitle">将全体学生年级升一年，并清理已毕业班级</p>
            </div>

            <div class="mgr-card">
                <div class="mgr-card__head"><h2 class="mgr-card__title">注意事项</h2></div>
                <div class="mgr-card__body">
                    <div class="mgr-prose">
                        <b>注意事项：</b>班级设置中的班级列表必须为全校完整班级列表，以防缺班而误删升上来的班级。<br /><br />
                        <b>升班效果：</b>学生表所有年级都升一年，然后在学生表中删除班级表中不存在班级的学生。如果班级未设置，则学年升班按钮失效。<br /><br />
                        <b>意外处理：</b>处理前先进数据备份菜单中备份数据库，如果高年级的班级数缺少，则可以在班级列表中手动添加缺少的班级，不影响数据。
                    </div>
                    <div class="mgr-alert">⚠️ 注意：请学年升班后再新生导入！</div>
                </div>
            </div>

            <div class="mgr-card">
                <div class="mgr-card__head"><h2 class="mgr-card__title">执行升班</h2></div>
                <div class="mgr-card__body">
                    <asp:TextBox ID="Textthisyear" runat="server" BorderStyle="None" ReadOnly="True" CssClass="mgr-input-readonly" style="width:fit-content;"></asp:TextBox>
                    <asp:Button ID="Btnupgrade" runat="server" Text="执行学年升班" onclick="Btnupgrade_Click" CssClass="mgr-btn mgr-btn--danger" style="width:fit-content;" />
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
