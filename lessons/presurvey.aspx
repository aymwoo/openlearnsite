<%@ Page Title="" Language="C#" MasterPageFile="~/lessons/prescm.master"  StylesheetTheme="Student"  AutoEventWireup="true" CodeFile="presurvey.aspx.cs" Inherits="Lessons_presurvey" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Ppcm" Runat="Server">
    

    <div id="student" class="presurvey-page">
        <div class="presurvey-toolbar">
            <div class="presurvey-toolbar__intro">
                <div class="presurvey-toolbar__eyebrow">教师预览</div>
                <div class="presurvey-toolbar__title">旧版 Survey 预览</div>
                <div class="presurvey-toolbar__meta">用于教师查看学生端作答页面布局与当前评估方式，预览模式下不会真实提交结果。</div>
            </div>
            <div class="presurvey-toolbar__actions">
                <div class="presurvey-badge"><%= EnableAiAssessment ? "AI 评价" : "规则模式" %></div>
                <a href="<%=ReturnUrl %>" class="presurvey-btn">返回学案</a>
            </div>
        </div>

        <div class="presurvey-card">
            <div class="course-node-head flex items-center gap-3 mb-4" style="padding:24px 24px 20px;margin:-24px -24px 16px;">
                <asp:ImageButton ID="Btnclock" runat="server" ImageUrl="~/images/clock.gif" Enabled="False" />
                <asp:Label runat="server" ID="Lbtitle" Font-Bold="True" Font-Size="16px"></asp:Label>
            </div>
            <div class="presurvey-meta">
                <div>姓名：<strong><asp:Label runat="server" ID="Lbsname" ForeColor="#0066FF">演示</asp:Label></strong></div>
                <div>学号：<strong><asp:Label runat="server" ID="Lbsnum" ForeColor="#0066FF">10101010</asp:Label></strong></div>
                <div>得分：<strong><asp:Label runat="server" ID="Lbfscore" ForeColor="#0066FF">00</asp:Label></strong></div>
                <div>类型：<strong><asp:Label runat="server" ID="Lbtypecn" ForeColor="#0066FF"></asp:Label></strong></div>
                <asp:Label runat="server" ID="Lbtype" Visible="False"></asp:Label>
                <asp:Label runat="server" ID="Lbcheck" Font-Bold="False"></asp:Label>
            </div>
        </div>

        <div class="presurvey-content">
            <div id="vcontent" runat="server"></div>
        </div>

        <div class="presurvey-preview-note">
            <div class="presurvey-preview-note__info">
                <div class="presurvey-preview-note__label">当前预览</div>
                <div class="presurvey-preview-note__title"><%= EnableAiAssessment ? "学生提交后将生成 AI 测验评估" : "学生提交后将生成规则评估摘要" %></div>
                <div class="presurvey-preview-note__desc">这里展示的是学生端作答页效果。教师预览仅用于检查题目、文案和评估方式，不会保存学生作答记录。</div>
            </div>
            <div class="presurvey-preview-note__status"><%= EnableAiAssessment ? "AI 已启用" : "规则模式" %></div>
        </div>

        <div class="presurvey-question-shell">
            <div class="presurvey-question-head">
                <div class="presurvey-question-head__title">题目预览</div>
                <div class="presurvey-question-head__meta">按学生端页面显示当前 Survey 题目列表与选项。</div>
            </div>
            <asp:DataList ID="DataListonly" runat="server" DataKeyField="Qid"
                RepeatColumns="1" RepeatLayout="Flow"
                onitemdatabound="DataListonly_ItemDataBound" >
                <ItemTemplate>
                    <div onmouseover="this.style.backgroundColor='#F8DFC9'" onmouseout="this.style.backgroundColor='' " style="margin: auto; border-bottom-style: dashed; border-bottom-width: 1px; border-bottom-color: #B0B0B0;">
                        <div style="width: 30px; float: left; left:6px; background-color: #F8DFC9;">
                            &nbsp;<asp:Label ID="Labelnum" Text='<%# Container.ItemIndex + 1%> ' runat="server" Font-Bold="True"></asp:Label>
                        </div>
                        <div style="width: 450px; float: left; left:40px">
                            &nbsp;<asp:Label ID="Labelquestion" runat="server" Text='<%# HttpUtility.HtmlDecode( Eval("Qtitle").ToString()) %>'></asp:Label>
                        </div>
                        <br />
                        <div style="margin: auto; width: 80%;">
                            <asp:RadioButtonList ID="RBLselect" runat="server"
                                RepeatLayout="Flow" RepeatColumns="1" RepeatDirection="Horizontal"
                                CellPadding="3" CellSpacing="6">
                            </asp:RadioButtonList>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:DataList>

            <div class="presurvey-action-row">
                <asp:Button ID="Btnok" runat="server" Text="提交答卷" BorderStyle="None" CssClass="presurvey-btn-disabled" />
                <asp:Button ID="Btnshow" runat="server" Text="查看结果" BorderStyle="None" CssClass="presurvey-btn-disabled" />
            </div>
            <div class="presurvey-action-note">预览模式下按钮仅用于展示学生端布局，不会执行真实提交。</div>
        </div>

        <script src="../js/jquery-1.8.2.min.js" type="text/javascript"></script>
        

        <div id="editInfo" class="presurvey-float">
            <div class="presurvey-float__label">时间流逝</div>
            <div class="presurvey-float__value"><asp:Label runat="server" ID="Lbtime" Font-Bold="True">0</asp:Label> 分钟</div>
        </div>

        <div class="presurvey-footer-note">
            注意：调查测验限时 8 分钟，每超 1 分钟扣除 1 学分。
        </div>
    </div>
    <script type="text/javascript" src="../js/presurvey.js"></script>
</asp:Content>
