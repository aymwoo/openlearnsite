<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master"  StylesheetTheme="Teacher"  AutoEventWireup="true" CodeFile="softview.aspx.cs" Inherits="Teacher_softview" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <div class="left">
        <br />
        <asp:Label ID="Labeltitle" runat="server" 
        Width="800px" CssClass="textcenter" Height="24px" BackColor="#E0E7FE" 
            BorderColor="#CAD6FD" BorderStyle="Solid" BorderWidth="1px"></asp:Label>
        <br />
        <div style="padding: 2px; margin: auto; border-bottom-style: dashed; border-width: 1px; border-color: #CCCCCC">资源：<asp:Label ID="Labelclass" runat="server"  SkinID="LabelFileShow"></asp:Label>
            格式：<asp:Image ID="ImageType" runat="server" />
        <asp:Label ID="Labelfiletype" runat="server"  ></asp:Label>
    点击率：<asp:Label ID="Labelhit" runat="server"  ></asp:Label>
    更新日期：<asp:Label ID="Labeldate" runat="server"  ></asp:Label>
    学分：<asp:Label ID="Labelopen" runat="server"  ></asp:Label>
            &nbsp;
            <asp:Button ID="BtnEdit" runat="server" Text="编辑内容" ToolTip="点击修改"
            OnClick="BtnEdit_Click" CssClass="admin-form-btn admin-form-btn--primary" />
        &nbsp;&nbsp;&nbsp;&nbsp; <asp:Button ID="BtnReturnSmall" runat="server" Text="返回列表" ToolTip="返回"
            OnClick="BtnReturnSmall_Click" CssClass="admin-form-btn admin-form-btn--secondary" />
        </div>
        <br />
        <center>
            <div >
                <br />
                <div style="padding: 2px; margin: auto; text-align: left; line-height: 18px; width: 780px;" >
                    <asp:Literal ID="Labelcontent" runat="server"></asp:Literal>
                </div>
                <br />
            </div>
        </center>
        <br />
        <div>
        <asp:Image ID="ImageDown" runat="server" ImageUrl="~/images/down1.gif" />
        <asp:LinkButton ID="LBtnfile" runat="server" 
        OnClick="LBtnfile_Click" Font-Underline="False" 
        BorderColor="#7DBF80" BorderStyle="Dashed" BorderWidth="1px" 
        CssClass="txtszcenter" Height="18px" BackColor="#E2F3E3" Width="80px">点击下载</asp:LinkButton>
        <br />
    <asp:HyperLink ID="HLurl" runat="server" Visible="false"  CssClass="px-4 py-2 bg-green-500 text-white rounded hover:bg-green-600 transition duration-300 shadow-md text-center inline-block"></asp:HyperLink>
        <br />
        <br />
              <asp:Button ID="Btnreturn" runat="server"  Text="返回列表" OnClick="Btnreturn_Click"
                  CssClass="px-4 py-2 bg-blue-500 text-white rounded hover:bg-blue-600 transition duration-300 shadow-md border-0" />
        <br />
        <br />
        <br />
        </div>
    </div>
</asp:Content>
