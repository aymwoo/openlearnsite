<%@ Page Language="C#" AutoEventWireup="true" CodeFile="examedit.aspx.cs" Inherits="exam_examedit" MasterPageFile="~/teacher/Teach.master" %><asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="/js/MenuCookie.js" type="text/javascript"></script>
    <script src="/js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/kindeditor/plugins/code/prettify.js" type="text/javascript"></script>
    <script src="/js/ruffle.js" type="text/javascript"></script>
</asp:Content><asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <style>
        .exam-form-page { min-height: calc(100vh - 8rem); padding: 1.5rem; background: #f8fafc; }
        .exam-form-shell { max-width: 1080px; margin: 0 auto; display: flex; flex-direction: column; gap: 1.25rem; }
        .exam-form-hero, .exam-form-card, .info-box { border: 1px solid rgba(148, 163, 184, 0.18); border-radius: 1.25rem; background: #ffffff; box-shadow: 0 12px 32px -28px rgba(15, 23, 42, 0.28); }
        .exam-form-hero { display: flex; justify-content: space-between; align-items: flex-start; gap: 1rem; padding: 1.5rem; background: linear-gradient(135deg, #ffffff 0%, #f8fafc 100%); }
        .exam-form-title { margin: 0; color: #0f172a; font-size: 1.625rem; font-weight: 700; line-height: 1.2; }
        .exam-form-subtitle { margin: 0.75rem 0 0; color: #475569; font-size: 0.95rem; line-height: 1.7; max-width: 42rem; }
        .hero-actions { display: flex; flex-wrap: wrap; gap: 0.75rem; }
        .page-btn, .form-actions input, .form-actions a { display: inline-flex; align-items: center; justify-content: center; min-height: 2.75rem; padding: 0 1rem; border: 1px solid transparent; border-radius: 0.9rem; font-size: 0.875rem; font-weight: 600; text-decoration: none; cursor: pointer; transition: all 0.2s ease; }
        .page-btn-secondary, .form-actions input[id$='btnCancel'] { background: #ffffff; color: #475569; border-color: #cbd5e1; }
        .page-btn-secondary:hover, .form-actions input[id$='btnCancel']:hover { color: #1e293b; border-color: #94a3b8; background: #f8fafc; }
        .page-btn-primary, .form-actions input[id$='btnSave'] { background: #2563eb; color: #ffffff; box-shadow: 0 10px 20px -14px rgba(37, 99, 235, 0.85); }
        .page-btn-primary:hover, .form-actions input[id$='btnSave']:hover { background: #1d4ed8; }
        .exam-form-card { padding: 1.5rem; }
        .section-title { margin: 0 0 1rem; color: #0f172a; font-size: 1rem; font-weight: 700; }
        .form-group { margin-bottom: 1rem; }
        .form-row { display: flex; gap: 1rem; }
        .form-row .form-group { flex: 1; }
        .form-group label { display: block; margin-bottom: 0.45rem; color: #334155; font-size: 0.88rem; font-weight: 600; }
        .form-group label span.required { color: #dc2626; }
        .form-control, select.form-control, textarea.form-control, input.form-control { width: 100%; min-height: 2.75rem; padding: 0.7rem 0.9rem; border: 1px solid #cbd5e1; border-radius: 0.9rem; box-sizing: border-box; background: #f8fafc; color: #0f172a; font-size: 0.875rem; transition: border-color 0.2s ease, box-shadow 0.2s ease, background 0.2s ease; }
        .form-control:focus { border-color: #93c5fd; background: #ffffff; outline: none; box-shadow: 0 0 0 4px rgba(191, 219, 254, 0.6); }
        textarea.form-control { min-height: 7rem; resize: vertical; }
        .checkbox-list { display: flex; flex-wrap: wrap; gap: 0.75rem 1rem; }
        .checkbox-list label { display: inline-flex; align-items: center; margin: 0; padding: 0.6rem 0.85rem; border: 1px solid #dbeafe; border-radius: 999px; background: #eff6ff; color: #1d4ed8; font-weight: 500; }
        .checkbox-list input { margin-right: 0.45rem; }
        .help-text { margin: 0.45rem 0 0; color: #94a3b8; font-size: 0.78rem; line-height: 1.6; }
        .inline-setting { display: inline-flex; align-items: center; gap: 0.6rem; margin-top: 0.75rem; padding: 0.75rem 0.9rem; border-radius: 0.9rem; background: #f8fafc; }
        .inline-setting label { margin: 0; white-space: nowrap; }
        .inline-setting .form-control { width: 5rem; min-height: 2.5rem; padding-left: 0.75rem; padding-right: 0.75rem; }
        .form-actions { display: flex; justify-content: center; gap: 0.75rem; margin-top: 0.5rem; }
        .info-box { padding: 1rem 1.25rem; background: linear-gradient(135deg, #fff7ed 0%, #fffbeb 100%); }
        .info-box .title { margin-bottom: 0.35rem; color: #c2410c; font-weight: 700; }
        .info-box, .info-box * { color: #9a3412; }
        @media (max-width: 900px) {
            .exam-form-page { padding: 1rem; }
            .exam-form-hero, .form-row { flex-direction: column; }
            .hero-actions { width: 100%; }
        }
    </style>

    <div class="exam-form-page">
        <div class="exam-form-shell">
            <section class="exam-form-hero">
                <div>
                    <h2 class="exam-form-title">编辑考试</h2>
                    <p class="exam-form-subtitle">调整考试配置、时间窗口和参与范围。页面视觉已与考试列表和教师端其他管理页保持一致。</p>
                </div>
                <div class="hero-actions">
                    <a href="examlist.aspx" class="page-btn page-btn-secondary">返回列表</a>
                </div>
            </section>

            <asp:Panel ID="pnlInfo" runat="server" Visible="false" CssClass="info-box">
                <div class="title">提示</div>
                <asp:Literal ID="ltlInfo" runat="server"></asp:Literal>
            </asp:Panel>

            <section class="exam-form-card">
                <h3 class="section-title">基础信息</h3>
                <div class="form-group">
                    <label><span class="required">*</span> 考试名称</label>
                    <asp:TextBox ID="txtExamName" runat="server" CssClass="form-control" placeholder="请输入考试名称"></asp:TextBox>
                </div>

                <div class="form-row">
                    <div class="form-group">
                        <label><span class="required">*</span> 选择试卷</label>
                        <asp:DropDownList ID="ddlPaper" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>考试类型</label>
                        <asp:DropDownList ID="ddlExamType" runat="server" CssClass="form-control">
                            <asp:ListItem Value="1">正式考试</asp:ListItem>
                            <asp:ListItem Value="2">模拟考试</asp:ListItem>
                            <asp:ListItem Value="3">练习模式</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </section>

            <section class="exam-form-card">
                <h3 class="section-title">时间与范围</h3>
                <div class="form-group">
                    <label>考试时间设置</label>
                    <div class="checkbox-list">
                        <label><asp:RadioButton ID="rbFixedTime" runat="server" GroupName="TimeMode" Checked="true" AutoPostBack="true" OnCheckedChanged="TimeMode_Changed" /> 固定时间段</label>
                        <label><asp:RadioButton ID="rbValidDays" runat="server" GroupName="TimeMode" AutoPostBack="true" OnCheckedChanged="TimeMode_Changed" /> 发布后有效期内</label>
                    </div>
                </div>

                <asp:Panel ID="pnlFixedTime" runat="server" Visible="true">
                    <div class="form-row">
                        <div class="form-group">
                            <label><span class="required">*</span> 开始时间</label>
                            <asp:TextBox ID="txtStartTime" runat="server" CssClass="form-control" TextMode="DateTimeLocal"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label><span class="required">*</span> 结束时间</label>
                            <asp:TextBox ID="txtEndTime" runat="server" CssClass="form-control" TextMode="DateTimeLocal"></asp:TextBox>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlValidDays" runat="server" Visible="false">
                    <div class="form-row">
                        <div class="form-group">
                            <label><span class="required">*</span> 发布时间</label>
                            <asp:TextBox ID="txtPublishTime" runat="server" CssClass="form-control" TextMode="DateTimeLocal"></asp:TextBox>
                            <p class="help-text">考试从此时间开始生效。</p>
                        </div>
                        <div class="form-group">
                            <label><span class="required">*</span> 有效期限</label>
                            <asp:DropDownList ID="ddlValidDays" runat="server" CssClass="form-control">
                                <asp:ListItem Value="1">1天内</asp:ListItem>
                                <asp:ListItem Value="3">3天内</asp:ListItem>
                                <asp:ListItem Value="7">一周内（7天）</asp:ListItem>
                                <asp:ListItem Value="14">两周内（14天）</asp:ListItem>
                                <asp:ListItem Value="30">一个月内（30天）</asp:ListItem>
                                <asp:ListItem Value="90">三个月内（90天）</asp:ListItem>
                                <asp:ListItem Value="0">永久有效</asp:ListItem>
                            </asp:DropDownList>
                            <p class="help-text">学生可在此期限内任意时间参加考试。</p>
                        </div>
                    </div>
                </asp:Panel>

                <div class="form-row">
                    <div class="form-group">
                        <label>考试时长（分钟）</label>
                        <asp:TextBox ID="txtDuration" runat="server" CssClass="form-control" Text="60"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>允许迟到（分钟）</label>
                        <asp:TextBox ID="txtLateMinutes" runat="server" CssClass="form-control" Text="0"></asp:TextBox>
                    </div>
                </div>

                <div class="form-group">
                    <label>参与对象</label>
                    <asp:DropDownList ID="ddlParticipantType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlParticipantType_SelectedIndexChanged">
                        <asp:ListItem Value="1">指定班级</asp:ListItem>
                        <asp:ListItem Value="2">全校学生</asp:ListItem>
                        <asp:ListItem Value="3">指定学生</asp:ListItem>
                    </asp:DropDownList>
                </div>

                <asp:Panel ID="pnlClassSelect" runat="server" Visible="false" CssClass="form-group">
                    <label>选择班级</label>
                    <div class="checkbox-list">
                        <asp:CheckBoxList ID="cblClasses" runat="server" RepeatColumns="4" RepeatLayout="Flow"></asp:CheckBoxList>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlStudentSelect" runat="server" Visible="false" CssClass="form-group">
                    <label>选择学生</label>
                    <asp:TextBox ID="txtStudents" runat="server" CssClass="form-control" placeholder="输入学生学号，多个用逗号分隔"></asp:TextBox>
                </asp:Panel>
            </section>

            <section class="exam-form-card">
                <h3 class="section-title">发布与防作弊</h3>
                <div class="form-group">
                    <label>考试密码（留空则不需要密码）</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="设置考试密码"></asp:TextBox>
                </div>

                <div class="form-group">
                    <label>考试设置</label>
                    <div class="checkbox-list">
                        <label><asp:CheckBox ID="chkShowAnswer" runat="server" /> 交卷后显示答案</label>
                        <label><asp:CheckBox ID="chkShowScore" runat="server" Checked="true" /> 交卷后显示分数</label>
                        <label><asp:CheckBox ID="chkShowRank" runat="server" /> 显示排名</label>
                    </div>
                </div>

                <div class="form-group">
                    <label>防作弊设置</label>
                    <div class="checkbox-list">
                        <label><asp:CheckBox ID="chkDisableCopy" runat="server" Checked="true" /> 禁止复制</label>
                        <label><asp:CheckBox ID="chkDisablePaste" runat="server" Checked="true" /> 禁止粘贴</label>
                        <label><asp:CheckBox ID="chkDetectSwitch" runat="server" Checked="true" /> 检测切屏</label>
                    </div>
                    <div class="inline-setting">
                        <label>最大切屏次数</label>
                        <asp:TextBox ID="txtMaxSwitch" runat="server" CssClass="form-control" Text="3"></asp:TextBox>
                        <span class="help-text">超过此次数自动提交</span>
                    </div>
                </div>

                <div class="form-actions">
                    <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="page-btn page-btn-primary" OnClick="btnSave_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="取消" CssClass="page-btn page-btn-secondary" OnClick="btnCancel_Click" />
                </div>
            </section>
        </div>
    </div>
</asp:Content>
