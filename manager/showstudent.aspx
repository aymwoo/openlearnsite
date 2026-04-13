<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manage.master" AutoEventWireup="true" CodeFile="showstudent.aspx.cs" Inherits="Manager_showstudent" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <style type="text/css">
        .mgr-page { padding: 28px; background: var(--ls-bg); min-height: calc(100vh - 8rem); box-sizing: border-box; width: 100%; }
        .mgr-page * { box-sizing: border-box; }
        .mgr-shell { display: flex; flex-direction: column; gap: 20px; }
        .mgr-card { border: 1px solid var(--ls-border); border-radius: 1rem; background: rgba(255,255,255,.96); box-shadow: 0 12px 30px rgba(15,23,42,.05); overflow: hidden; }
        .mgr-card__head { padding: 20px 24px; border-bottom: 1px solid #f1f5f9; display: flex; align-items: center; justify-content: space-between; }
        .mgr-card__title { margin: 0; font-size: 16px; font-weight: 800; color: var(--ls-text); }
        .mgr-grid { width: 100%; border-collapse: collapse; }
        .mgr-grid th { background: #f8fafc; padding: 14px 16px; font-size: 14px; font-weight: 700; color: #64748b; text-align: center; border-bottom: 2px solid #f1f5f9; }
        .mgr-grid td { padding: 12px 16px; font-size: 14px; color: #334155; border-bottom: 1px solid #f1f5f9; text-align: center; }
        .mgr-grid tbody tr:hover { background-color: #f1f5f9; }
        .mgr-grid tbody tr:hover td { background-color: transparent; }
        .pager-container { padding: 14px 20px; background: #f8fafc; display: flex; align-items: center; justify-content: space-between; font-size: 14px; font-weight: 600; color: #64748b; }
        .pager-buttons { display: flex; gap: 8px; }
        .pager-btn { padding: 5px 12px; background: #fff; border: 1px solid #e2e8f0; border-radius: 8px; color: #475569; text-decoration: none; transition: all .2s; }
        .pager-btn:hover { background: #f1f5f9; }
        .mgr-btn { display: inline-flex; align-items: center; justify-content: center; min-height: 40px; padding: 0 18px; border-radius: 1rem; border: none; font-size: 14px; font-weight: 700; cursor: pointer; }
        .mgr-btn--outline { background: #fff; color: #475569; border: 1px solid #e2e8f0; }
        .mgr-btn--outline:hover { background: #f8fafc; }
        .mgr-meta { font-size: 14px; color: #64748b; }
    </style>
    <div class="mgr-page">
        <div class="mgr-shell">
            <div class="mgr-card">
                <div class="mgr-card__head">
                    <h2 class="mgr-card__title">临时导入学生列表</h2>
                    <div style="display:flex;align-items:center;gap:12px;">
                        <asp:Label ID="Labelcount" runat="server" CssClass="mgr-meta"></asp:Label>
                        <asp:Button ID="ButtonReturn" runat="server" Text="返回" OnClick="ButtonInsert_Click" CssClass="mgr-btn mgr-btn--outline" />
                    </div>
                </div>
                <asp:GridView ID="GVstudent" runat="server" CssClass="mgr-grid" GridLines="None"
                    Width="100%" AllowPaging="True" PageSize="25"
                    OnPageIndexChanging="GVstudent_PageIndexChanging"
                    OnRowDataBound="GVstudent_RowDataBound"
                    AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="Snum" HeaderText="学号"><ItemStyle CssClass="font-mono text-indigo-600 font-bold" /></asp:BoundField>
                        <asp:BoundField DataField="Syear" HeaderText="入学年度" />
                        <asp:BoundField DataField="Sgrade" HeaderText="年级" />
                        <asp:BoundField DataField="Sclass" HeaderText="班级" />
                        <asp:BoundField DataField="Sname" HeaderText="姓名"><ItemStyle CssClass="font-semibold" /></asp:BoundField>
                        <asp:BoundField DataField="Spwd" HeaderText="密码"><ItemStyle CssClass="font-mono text-slate-400" /></asp:BoundField>
                        <asp:BoundField DataField="Sex" HeaderText="性别" />
                        <asp:BoundField DataField="Saddress" HeaderText="家庭住址" />
                        <asp:BoundField DataField="Sphone" HeaderText="联系电话" />
                        <asp:BoundField DataField="Sparents" HeaderText="家长姓名" />
                        <asp:BoundField DataField="Sheadtheacher" HeaderText="班主任" />
                    </Columns>
                    <PagerTemplate>
                        <div class="pager-container">
                            <span>第 <asp:Label ID="lblPageIndex" runat="server" Text="<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>" style="color:#4f46e5;"></asp:Label> / <asp:Label ID="lblPageCount" runat="server" Text="<%# ((GridView)Container.Parent.Parent).PageCount %>"></asp:Label> 页</span>
                            <div class="pager-buttons">
                                <asp:LinkButton ID="btnFirst" runat="server" CausesValidation="False" CommandArgument="First" CommandName="Page" CssClass="pager-btn">首页</asp:LinkButton>
                                <asp:LinkButton ID="btnPrev" runat="server" CausesValidation="False" CommandArgument="Prev" CommandName="Page" CssClass="pager-btn">上一页</asp:LinkButton>
                                <asp:LinkButton ID="btnNext" runat="server" CausesValidation="False" CommandArgument="Next" CommandName="Page" CssClass="pager-btn">下一页</asp:LinkButton>
                                <asp:LinkButton ID="btnLast" runat="server" CausesValidation="False" CommandArgument="Last" CommandName="Page" CssClass="pager-btn">尾页</asp:LinkButton>
                            </div>
                        </div>
                    </PagerTemplate>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
