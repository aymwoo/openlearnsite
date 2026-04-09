<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manage.master" AutoEventWireup="true" CodeFile="ipnet.aspx.cs" Inherits="Seat_ipnet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <div>
        <br />
        <strong>网段配置（多机房支持）</strong><br />
        <br />
        <span style="color: #666666; font-size: 9pt;">说明：配置不同网段对应的机房，系统会根据学生IP地址自动识别所在机房，从而正确显示机号。</span>
        <br />
        <br />
        <asp:GridView ID="GVIpNet" runat="server" 
            AutoGenerateColumns="False" BorderColor="#E7E7E7" BorderStyle="Solid" 
            BorderWidth="1px" CellPadding="3" Font-Size="9pt" GridLines="None" Width="600px" 
            onrowdatabound="GVIpNet_RowDataBound" EnableModelValidation="True"
            HorizontalAlign="Center" onrowcommand="GVIpNet_RowCommand"
            onrowediting="GVIpNet_RowEditing" onrowcancelingedit="GVIpNet_RowCancelingEdit"
            onrowupdating="GVIpNet_RowUpdating" DataKeyNames="Nid">
            <Columns>
                <asp:BoundField HeaderText="序号" ReadOnly="True">
                    <HeaderStyle Width="50px" />
                    <ItemStyle Width="50px" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="网段">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBoxNet" runat="server" Text='<%# Bind("Nnet") %>' Width="100px"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="LabelNet" runat="server" Text='<%# Bind("Nnet") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Width="120px" />
                    <ItemStyle Width="120px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="对应机房">
                    <EditItemTemplate>
                        <asp:DropDownList ID="DDLHouse" runat="server" Font-Size="9pt">
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="LabelHouse" runat="server" Text='<%# Bind("Nname") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Width="120px" />
                    <ItemStyle Width="120px" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="备注">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBoxRemark" runat="server" Text='<%# Bind("Nremark") %>' Width="150px"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="LabelRemark" runat="server" Text='<%# Bind("Nremark") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Width="180px" />
                    <ItemStyle Width="180px" />
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="True" HeaderText="编辑" EditText="编辑" UpdateText="更新" CancelText="取消">
                    <HeaderStyle Width="60px" />
                    <ItemStyle Width="60px" />
                </asp:CommandField>
                <asp:TemplateField ShowHeader="False" HeaderText="删除">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButtonDel" runat="server" CausesValidation="false" 
                        CommandArgument='<%# Bind("Nid") %>' CommandName="Del" Text="删除"></asp:LinkButton>
                    </ItemTemplate>
                    <HeaderStyle Width="50px" />
                    <ItemStyle Width="50px" />
                </asp:TemplateField>
            </Columns>
            <RowStyle BorderStyle="None" Font-Names="Arial" Font-Size="9pt" 
                ForeColor="Black" Height="24px" />
            <HeaderStyle BackColor="#939CA2" Font-Bold="False" Font-Names="Arial" 
                Font-Size="9pt" Height="24px" />
            <AlternatingRowStyle BackColor="#E7E7E7" />
        </asp:GridView>
        <br />
        <br />
        <table style="font-size: 9pt; margin: auto; width: 500px;">
            <tr>
                <td style="text-align: right; width: 100px;">网段：</td>
                <td style="text-align: left;">
                    <asp:TextBox ID="TextBoxNet" runat="server" BorderColor="#CCCCCC" 
                        BorderStyle="Solid" BorderWidth="1px" Width="100px"></asp:TextBox>
                    <span style="color: #999999;">例如：172.16.3</span>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">对应机房：</td>
                <td style="text-align: left;">
                    <asp:DropDownList ID="DDLHouse" runat="server" Font-Size="9pt">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">备注：</td>
                <td style="text-align: left;">
                    <asp:TextBox ID="TextBoxRemark" runat="server" BorderColor="#CCCCCC" 
                        BorderStyle="Solid" BorderWidth="1px" Width="200px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td style="text-align: left;">
                    <asp:Button ID="ButtonAdd" runat="server" BackColor="#E6E6E6" 
                        BorderColor="#D4D4D4" BorderWidth="1px" Font-Size="9pt" Text="添加网段" 
                        onclick="ButtonAdd_Click" Width="80px" />
                    &nbsp;&nbsp;
                    <asp:Label ID="LabelMsg" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <hr style="border: 1px dashed #CCCCCC; width: 600px;" />
        <br />
        <span style="color: #666666; font-size: 9pt;">
            <b>使用说明：</b><br />
            1. 网段格式为IP地址的前三段，例如：172.16.3 或 192.168.1<br />
            2. 当学生从不同网段访问系统时，系统会自动根据网段识别对应的机房<br />
            3. 识别到机房后，系统会在该机房的IP列表中查找对应的机号<br />
            4. 例如：配置网段 172.16.3 对应机房A，172.16.4 对应机房B<br />
        </span>
        <br />
        <br />
    </div>
</asp:Content>
