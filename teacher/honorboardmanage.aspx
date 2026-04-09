<%@ Page Title="荣誉榜设置" Language="C#" StylesheetTheme="Teacher" AutoEventWireup="true"
    CodeFile="honorboardmanage.aspx.cs" Inherits="LearnSite.Teacher.honorboardmanage" MasterPageFile="~/teacher/Teach.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <div class="placehold">
        <center>
            <br />
            <h2 style="color: #667eea; margin-bottom: 30px;">荣誉榜显示范围设置</h2>

            <div style="width: 600px; text-align: left; background: white; padding: 30px; border-radius: 10px; box-shadow: 0 2px 10px rgba(0,0,0,0.1);">
                <asp:Panel ID="PanelSettings" runat="server">
                    <div style="margin-bottom: 25px;">
                        <label style="font-size: 16px; font-weight: bold; color: #333; margin-bottom: 10px; display: block;">
                            📊 荣誉榜显示范围
                        </label>
                        <asp:RadioButtonList ID="RblScope" runat="server"
                            AutoPostBack="true" OnSelectedIndexChanged="RblScope_SelectedIndexChanged">
                            <asp:ListItem Value="school" Text="全校" Selected="True">
                                <span style="color: #666; font-size: 14px; margin-left: 10px;">显示全校所有学生的荣誉榜</span>
                            </asp:ListItem>
                            <asp:ListItem Value="grade">
                                <span style="color: #666; font-size: 14px;">年级（学生登录后显示自己所在年级的荣誉榜）</span>
                            </asp:ListItem>
                            <asp:ListItem Value="class">
                                <span style="color: #666; font-size: 14px;">班级（学生登录后显示自己所在班级的荣誉榜）</span>
                            </asp:ListItem>
                        </asp:RadioButtonList>
                    </div>

                    <!-- 年级和班级模式下不需要选择框，学生登录后自动根据自己所在的年级或班级显示 -->
                    <asp:Panel ID="PanelGradeClass" runat="server" Visible="false">
                    </asp:Panel>

                    <div style="margin-top: 30px; text-align: center;">
                        <asp:Button ID="BtnSave" runat="server" Text="保存设置"
                            CssClass="button" OnClick="BtnSave_Click"
                            Style="padding: 10px 30px; font-size: 16px; background: #4CAF50; color: white; border: none; border-radius: 5px; cursor: pointer;" />
                    </div>
                </asp:Panel>

                <asp:Panel ID="PanelSuccess" runat="server" Visible="false" style="text-align: center; padding: 20px;">
                    <div style="color: #4CAF50; font-size: 18px; margin-bottom: 15px;">
                        ✅ 设置已保存成功!
                    </div>
                    <asp:Button ID="BtnBack" runat="server" Text="返回" PostBackUrl="~/teacher/teachermanage.aspx"
                        Style="padding: 10px 30px; font-size: 16px; background: #667eea; color: white; border: none; border-radius: 5px; cursor: pointer;" />
                </asp:Panel>
            </div>

            <div style="width: 600px; margin-top: 30px; text-align: left; background: white; padding: 20px; border-radius: 10px; box-shadow: 0 2px 10px rgba(0,0,0,0.1);">
                <h3 style="color: #333; margin-bottom: 15px; font-size: 16px;">📖 说明</h3>
                <ul style="color: #666; font-size: 14px; line-height: 1.8; padding-left: 20px;">
                    <li><strong>全校</strong>：显示全校所有年级、班级的荣誉榜</li>
                    <li><strong>年级</strong>：学生登录后自动显示自己所在年级的荣誉榜</li>
                    <li><strong>班级</strong>：学生登录后自动显示自己所在班级的荣誉榜</li>
                    <li>年级和班级模式下不需要选择具体年级或班级，学生登录后会根据自己的年级和班级自动过滤显示</li>
                    <li>如需修改显示范围，选择后点击"保存设置"即可</li>
                </ul>
            </div>
        </center>
    </div>
</asp:Content>
