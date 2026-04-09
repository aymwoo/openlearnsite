<%@ Page Title="" Language="C#" ResponseEncoding="utf-8" MasterPageFile="~/manager/Manage.master" AutoEventWireup="true" CodeFile="house.aspx.cs" Inherits="Seat_house" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
<style type="text/css">
    .hs-wrap {
        padding: 28px;
        background: linear-gradient(180deg,#f8fbff 0%,#f3f7ff 100%);
        color: #0f172a;
        box-sizing: border-box;
    }
    .hs-wrap * { box-sizing: border-box; }

    .hs-hero {
        border-radius: 1rem;
        padding: 24px 28px;
        background: linear-gradient(135deg,#eff6ff 0%,#dbeafe 100%);
        color: #1e3a8a;
        border: 1px solid #bfdbfe;
        box-shadow: 0 4px 16px rgba(37,99,235,.08);
        margin-bottom: 24px;
    }
    .hs-hero h1 { margin: 0; font-size: 24px; font-weight: 800; letter-spacing: -0.02em; }
    .hs-hero p  { margin: 8px 0 0; font-size: 14px; line-height: 1.8; color: #1e40af; }

    .hs-card {
        border-radius: 1rem;
        border: 1px solid #dbe6f5;
        background: rgba(255,255,255,.96);
        box-shadow: 0 12px 30px rgba(15,23,42,.05);
        overflow: hidden;
        margin-bottom: 20px;
    }
    .hs-card__head { padding: 18px 22px 0; }
    .hs-card__title { margin: 0; font-size: 16px; font-weight: 800; color: #0f172a; }
    .hs-card__desc  { margin: 6px 0 0; font-size: 12px; color: #64748b; line-height: 1.7; }
    .hs-card__body  { padding: 18px 22px 22px; }

    .hs-table { width: 100%; border-collapse: collapse; font-size: 14px; }
    .hs-table th {
        padding: 12px 16px; background: #f8fafc; font-size: 14px; font-weight: 700;
        color: #64748b; text-align: center; border-bottom: 2px solid #f1f5f9;
    }
    .hs-table td {
        padding: 12px 16px; text-align: center; border-bottom: 1px solid #f1f5f9; color: #0f172a;
    }
    .hs-table tbody tr:hover { background: #f8fbff; }

    .hs-link {
        display: inline-flex; align-items: center; justify-content: center;
        min-height: 32px; padding: 0 14px; border-radius: 1rem;
        font-size: 14px; font-weight: 700; text-decoration: none;
        background: #eff6ff; color: #1d4ed8; border: 1px solid #bfdbfe;
        transition: transform .15s;
    }
    .hs-link:hover { transform: translateY(-1px); }
    .hs-link--danger { background: #fff1f2; color: #b91c1c; border-color: #fecaca; }

    .hs-row { display: flex; flex-wrap: wrap; align-items: center; gap: 10px; margin-top: 4px; }
    .hs-label { font-size: 14px; font-weight: 700; color: #334155; }
    .hs-input {
        min-height: 42px; padding: 0 14px; border: 1px solid #cbd5e1;
        border-radius: 1rem; background: #f8fafc; color: #0f172a; font-size: 14px;
        transition: border-color .2s, box-shadow .2s;
    }
    .hs-input:focus { border-color: #60a5fa; outline: none; background: #fff; box-shadow: 0 0 0 4px rgba(96,165,250,.18); }
    .hs-btn {
        display: inline-flex; align-items: center; justify-content: center;
        min-height: 42px; padding: 0 20px; border: 0; border-radius: 1rem;
        font-size: 14px; font-weight: 700; color: #fff; cursor: pointer;
        background: linear-gradient(135deg,#2563eb 0%,#1d4ed8 100%);
        box-shadow: 0 8px 16px rgba(37,99,235,.2);
        transition: transform .15s;
    }
    .hs-btn:hover { transform: translateY(-1px); }

    .hs-card--green { background: linear-gradient(160deg,#fff 0%,#f7fef9 100%); border-color: #d4f0dc; }
</style>

<div class="hs-wrap">

    <div class="hs-hero">
        <h1>机房布置</h1>
        <p>管理机房列表，配置每间机房的电脑座位与IP对应关系，并可启用手工布置模式。</p>
    </div>

    <div class="hs-card">
        <div class="hs-card__head">
            <h2 class="hs-card__title">机房列表</h2>
            <p class="hs-card__desc">点击"布置"配置电脑座位，点击"对应"设置IP映射，点击"删除"移除该机房。</p>
        </div>
        <div class="hs-card__body">
            <asp:GridView ID="GVHouse" runat="server"
                AutoGenerateColumns="False" GridLines="None" Width="100%"
                CssClass="hs-table"
                onrowdatabound="GVHouse_RowDataBound" EnableModelValidation="True"
                onrowcommand="GVHouse_RowCommand">
                <Columns>
                    <asp:BoundField HeaderText="序号" />
                    <asp:BoundField DataField="Hname" HeaderText="机房名称" />
                    <asp:HyperLinkField DataNavigateUrlFields="hid"
                        DataNavigateUrlFormatString="computer.aspx?Hid={0}" HeaderText="电脑"
                        Text="布置" Target="_blank" ControlStyle-CssClass="hs-link" />
                    <asp:HyperLinkField DataNavigateUrlFields="hid"
                        DataNavigateUrlFormatString="ip.aspx?Hid={0}" HeaderText="IP表"
                        Text="对应" Target="_blank" ControlStyle-CssClass="hs-link" />
                    <asp:TemplateField ShowHeader="False" HeaderText="操作">
                        <ItemTemplate>
                            <asp:LinkButton ID="LinkButtonDel" runat="server" CausesValidation="false"
                                CommandArgument='<%# Bind("hid") %>' CommandName="Del"
                                Text="删除" CssClass="hs-link hs-link--danger"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="" />
                <RowStyle Height="44px" />
                <AlternatingRowStyle BackColor="#f8fbff" />
            </asp:GridView>
        </div>
    </div>

    <div class="hs-card">
        <div class="hs-card__head">
            <h2 class="hs-card__title">添加机房</h2>
        </div>
        <div class="hs-card__body">
            <div class="hs-row">
                <span class="hs-label">机房名称</span>
                <asp:TextBox ID="TextBoxHname" runat="server" CssClass="hs-input" style="width:200px;"></asp:TextBox>
                <asp:Button ID="Buttonadd" runat="server" Text="添加" onclick="Buttonadd_Click" CssClass="hs-btn" />
            </div>
        </div>
    </div>

    <div class="hs-card hs-card--green">
        <div class="hs-card__body">
            <asp:CheckBox ID="CkBox" runat="server" oncheckedchanged="CkBox_CheckedChanged"
                Text="启用手工机房布置" AutoPostBack="True" />
        </div>
    </div>

</div>
</asp:Content>
