<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="teachermanage.aspx.cs" Inherits="Teacher_teachermanage" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Teacher/student.css" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <style>
        .manage-page { padding: 24px; }
        .manage-shell { display: flex; flex-direction: column; gap: 24px; }
        .manage-hero {
            position: relative;
            overflow: hidden;
            border-radius: 24px;
            padding: 28px 32px;
            background: linear-gradient(135deg, #0f172a 0%, #4f46e5 50%, #06b6d4 100%);
            color: #eff6ff;
            box-shadow: 0 20px 46px rgba(15, 23, 42, 0.16);
        }
        .manage-hero::after {
            content: "";
            position: absolute;
            top: -60px;
            right: -40px;
            width: 220px;
            height: 220px;
            border-radius: 999px;
            background: rgba(255,255,255,0.08);
        }
        .manage-hero__content {
            position: relative;
            z-index: 1;
            display: flex;
            justify-content: space-between;
            gap: 20px;
            flex-wrap: wrap;
            align-items: flex-start;
        }
        .manage-hero__title { margin: 0; font-size: 32px; font-weight: 800; letter-spacing: 0.04em; }
        .manage-hero__subtitle { margin: 10px 0 0; max-width: 760px; color: rgba(239,246,255,0.9); line-height: 1.75; }
        .manage-hero__chip {
            display: inline-flex;
            align-items: center;
            gap: 10px;
            padding: 10px 14px;
            border-radius: 14px;
            background: rgba(15,23,42,0.22);
            border: 1px solid rgba(255,255,255,0.16);
            font-weight: 700;
        }
        .manage-grid {
            display: grid;
            grid-template-columns: repeat(12, minmax(0, 1fr));
            gap: 20px;
        }
        .manage-section {
            grid-column: span 12;
            background: #fff;
            border: 1px solid #e2e8f0;
            border-radius: 22px;
            box-shadow: 0 14px 34px rgba(15, 23, 42, 0.06);
            overflow: hidden;
        }
        .manage-section--student {
            border-color: #bbf7d0;
            background: linear-gradient(180deg, #ffffff 0%, #f6fff8 100%);
        }
        .manage-section--teach {
            border-color: #bfdbfe;
            background: linear-gradient(180deg, #ffffff 0%, #f7fbff 100%);
        }
        .manage-section--system {
            border-color: #fecaca;
            background: linear-gradient(180deg, #ffffff 0%, #fff8f8 100%);
        }
        .manage-student-panel {
            border-color: #ddd6fe;
            background: linear-gradient(180deg, #ffffff 0%, #faf8ff 100%);
        }
        .manage-section__head {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 16px;
            padding: 22px 24px 0;
        }
        .manage-section__title { margin: 0; font-size: 20px; font-weight: 800; color: #0f172a; }
        .manage-section__desc { margin: 6px 0 0; color: #64748b; font-size: 14px; }
        .manage-section__body { padding: 22px 24px 24px; }
        .manage-section--student .manage-badge { background: #dcfce7; color: #15803d; }
        .manage-section--teach .manage-badge { background: #dbeafe; color: #1d4ed8; }
        .manage-section--system .manage-badge { background: #fee2e2; color: #dc2626; }
        .manage-badge {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            min-width: 84px;
            padding: 8px 12px;
            border-radius: 999px;
            font-size: 12px;
            font-weight: 800;
        }
        .manage-link-grid {
            display: grid;
            grid-template-columns: repeat(4, minmax(0, 1fr));
            gap: 14px;
        }
        .manage-link {
            display: flex;
            align-items: center;
            gap: 14px;
            padding: 18px 18px;
            border-radius: 18px;
            border: 1px solid #e2e8f0;
            background: linear-gradient(180deg, #ffffff 0%, #f8fbff 100%);
            text-decoration: none;
            color: #0f172a;
            transition: transform 0.18s ease, box-shadow 0.18s ease, border-color 0.18s ease;
        }
        .manage-link:hover {
            transform: translateY(-2px);
            box-shadow: 0 12px 24px rgba(59, 130, 246, 0.12);
            border-color: #93c5fd;
            color: #0f172a;
        }
        .manage-link__icon {
            width: 48px;
            height: 48px;
            border-radius: 16px;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            flex-shrink: 0;
            font-size: 24px;
            box-shadow: inset 0 0 0 1px rgba(255,255,255,0.5);
        }
        .manage-section--student .manage-link__icon { background: linear-gradient(135deg, #dcfce7 0%, #bbf7d0 100%); }
        .manage-section--teach .manage-link__icon { background: linear-gradient(135deg, #dbeafe 0%, #bfdbfe 100%); }
        .manage-section--system .manage-link__icon { background: linear-gradient(135deg, #fee2e2 0%, #fecaca 100%); }
        .manage-link__text { display: flex; flex-direction: column; gap: 4px; min-width: 0; }
        .manage-link__title { font-size: 15px; font-weight: 800; }
        .manage-link__desc { font-size: 13px; color: #64748b; line-height: 1.5; }
        .manage-student-panel {
            grid-column: span 12;
            display: flex;
            flex-direction: column;
            gap: 20px;
        }
        .manage-student-panel__content {
            overflow: hidden;
            max-height: 0;
            opacity: 0;
            transition: max-height 0.28s ease, opacity 0.2s ease;
        }
        .manage-student-panel.is-open .manage-student-panel__content {
            max-height: 5000px;
            opacity: 1;
        }
        .manage-collapse-btn {
            display: inline-flex;
            align-items: center;
            gap: 8px;
            min-height: 38px;
            padding: 0 14px;
            border-radius: 12px;
            border: 1px solid #ddd6fe;
            background: #f5f3ff;
            color: #6d28d9;
            font-weight: 700;
            cursor: pointer;
        }
        .manage-collapse-btn svg {
            transition: transform 0.2s ease;
        }
        .manage-student-panel.is-open .manage-collapse-btn svg {
            transform: rotate(180deg);
        }
        .manage-student-head {
            display: flex;
            justify-content: space-between;
            gap: 16px;
            align-items: flex-end;
            flex-wrap: wrap;
        }
        .manage-student-head__title {
            margin: 0;
            font-size: 20px;
            font-weight: 800;
            color: #0f172a;
        }
        .manage-student-head__desc {
            margin: 6px 0 0;
            color: #64748b;
            font-size: 14px;
        }
        @media (max-width: 1180px) {
            .manage-link-grid { grid-template-columns: repeat(3, minmax(0, 1fr)); }
        }
        @media (max-width: 860px) {
            .manage-link-grid { grid-template-columns: repeat(2, minmax(0, 1fr)); }
        }
        @media (max-width: 640px) {
            .manage-page { padding: 16px; }
            .manage-hero { padding: 22px 20px; }
            .manage-hero__title { font-size: 26px; }
            .manage-section__head, .manage-section__body { padding-left: 18px; padding-right: 18px; }
            .manage-link-grid { grid-template-columns: 1fr; }
        }
    </style>

    <div class="manage-page">
        <div class="manage-shell">
            <section class="manage-hero">
                <div class="manage-hero__content">
                    <div>
                        <h1 class="manage-hero__title">课堂管理中心</h1>
                        <p class="manage-hero__subtitle">汇总学生管理、教学巡检与机房系统工具入口，方便教师在课堂前、中、后快速切换常用管理功能。</p>
                    </div>
                    <div class="manage-hero__chip">Teacher Control Hub</div>
                </div>
            </section>

            <div class="manage-grid">
                <section class="manage-section manage-student-panel">
                    <div class="manage-section__head">
                        <div>
                            <h2 class="manage-section__title">学生名册与权限</h2>
                            <p class="manage-section__desc">直接在管理中心内完成班级学生浏览、密码初始化、分组操作与资料权限设置。</p>
                        </div>
                        <div style="display:flex;align-items:center;gap:10px;flex-wrap:wrap;">
                            <span class="manage-badge" style="background:#ede9fe;color:#6d28d9;">Roster</span>
                            <button type="button" id="toggleStudentPanel" class="manage-collapse-btn" aria-expanded="false">
                                <span>展开名册</span>
                                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="m6 9 6 6 6-6"></path></svg>
                            </button>
                        </div>
                    </div>
                    <div class="manage-section__body manage-student-panel__content">
                        <div class="stu-page" style="padding:0;">
                            <div class="stu-shell">
                                <div class="stu-hero">
                                    <div class="stu-hero__content">
                                        <div>
                                            <h1 class="stu-hero__title">学生管理</h1>
                                            <p class="stu-hero__subtitle">管理班级学生信息、分组、密码及权限设置</p>
                                        </div>
                                        <div class="filter-group">
                                            <div class="filter-item">
                                                <span class="filter-label">年级</span>
                                                <asp:DropDownList ID="DDLgrade" runat="server" AutoPostBack="True" CssClass="filter-select" onselectedindexchanged="DDLgrade_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                            <div style="width:1px;height:16px;background:rgba(255,255,255,0.2);"></div>
                                            <div class="filter-item">
                                                <span class="filter-label">班级</span>
                                                <asp:DropDownList ID="DDLclass" runat="server" AutoPostBack="True" CssClass="filter-select" onselectedindexchanged="DDLclass_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="stu-card">
                                    <div class="stu-card__head">
                                        <div>
                                            <h2 class="stu-card__title">学生列表</h2>
                                            <asp:Label ID="Label1" runat="server" CssClass="stu-card__desc"></asp:Label>
                                        </div>
                                        <asp:HyperLink ID="HkaddStu" runat="server" CssClass="stu-add-link">添加学生</asp:HyperLink>
                                    </div>

                                    <div class="stu-list">
                                        <div class="stu-list-header">
                                            <div class="stu-list-header-cell">序号</div>
                                            <div class="stu-list-header-cell">学号</div>
                                            <div class="stu-list-header-cell">密码</div>
                                            <div class="stu-list-header-cell"></div>
                                            <div class="stu-list-header-cell">年级</div>
                                            <div class="stu-list-header-cell">班级</div>
                                            <div class="stu-list-header-cell">姓名</div>
                                            <div class="stu-list-header-cell">性别</div>
                                            <div class="stu-list-header-cell"></div>
                                            <div class="stu-list-header-cell">组号</div>
                                            <div class="stu-list-header-cell">成绩</div>
                                            <div class="stu-list-header-cell">作品</div>
                                            <div class="stu-list-header-cell">表现</div>
                                            <div class="stu-list-header-cell">操作</div>
                                        </div>
                                        <asp:Repeater ID="RptStudent" runat="server" OnItemDataBound="RptStudent_ItemDataBound" OnItemCommand="RptStudent_ItemCommand">
                                            <ItemTemplate>
                                                <div class="stu-list-row">
                                                    <div class="stu-list-cell" style="color:#94a3b8;font-size:12px;"><asp:Label ID="LabelRowIndex" runat="server"></asp:Label></div>
                                                    <div class="stu-list-cell stu-list-cell--snum" style="font-weight:700;color:#4f46e5;font-family:monospace;"><%# Eval("Snum") %></div>
                                                    <div class="stu-list-cell" style="color:#94a3b8;"><asp:Label ID="Labelpwd" runat="server" Text="******" ToolTip='<%# Eval("Spwd") %>'></asp:Label></div>
                                                    <div class="stu-list-cell"><asp:Button ID="ImageButton1" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Sid") %>' CommandName="ChangePwd" Text="重置" ToolTip="自动更新密码" CssClass="stu-mini-btn" /></div>
                                                    <div class="stu-list-cell"><%# Eval("Sgrade") %></div>
                                                    <div class="stu-list-cell" style="font-weight:600;"><%# Eval("Sclass") %></div>
                                                    <div class="stu-list-cell" style="text-align:left;"><asp:HyperLink ID="Hlname" runat="server" Text='<%# Eval("Sname") %>' ToolTip='<%# Eval("Sid") %>' CssClass="stu-name-link"></asp:HyperLink></div>
                                                    <div class="stu-list-cell"><%# Eval("Sex") %></div>
                                                    <div class="stu-list-cell"><asp:Button ID="ImageBtnGroup" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Sid") %>' CommandName="ChangeGroup" Text="分组" CssClass="stu-mini-btn" /></div>
                                                    <div class="stu-list-cell"><asp:LinkButton ID="LinkBtnQuit" runat="server" CausesValidation="false" CommandArgument='<%# Eval("Sid") %>' CommandName="QuitGroup" Text='<%# Eval("Sgroup") %>' style="display:inline-flex;align-items:center;justify-content:center;width:26px;height:26px;border-radius:0.4rem;background:#f1f5f9;color:#475569;font-size:12px;font-weight:800;text-decoration:none;border:1px solid #e2e8f0;"></asp:LinkButton></div>
                                                    <div class="stu-list-cell"><a href='<%# "studentwork.aspx?snum=" + Eval("Snum") %>' class="stu-link" target="_blank"><%# Eval("Sscore") %></a></div>
                                                    <div class="stu-list-cell"><a href='<%# "studentworks.aspx?snum=" + Eval("Snum") %>' class="stu-link" target="_blank">浏览</a></div>
                                                    <div class="stu-list-cell" style="color:#f43f5e;font-weight:700;"><%# Eval("Sattitude") %></div>
                                                    <div class="stu-list-cell"><a href='<%# "studentdel.aspx?sid=" + Eval("Sid") + "&sgrade=" + Eval("Sgrade") + "&sclass=" + Eval("Sclass") %>' class="stu-link" style="color:#f43f5e;">删除</a></div>
                                                    <asp:Label ID="LabelSleader" runat="server" Text='<%# Eval("Sleader") %>' style="display:none;"></asp:Label>
                                                    <asp:Label ID="LabelSnum" runat="server" Text='<%# Eval("Snum") %>' style="display:none;"></asp:Label>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                    <div id="PagerDiv" runat="server" class="pager-container">
                                        <span>第 <asp:Label ID="LblPageIndex" runat="server" style="color:#4f46e5;font-weight:800;"></asp:Label> / <asp:Label ID="LblPageCount" runat="server"></asp:Label> 页</span>
                                        <div class="pager-buttons">
                                            <asp:LinkButton ID="btnFirst" runat="server" CommandName="Page" CommandArgument="First" CssClass="pager-btn" OnClick="Pager_Click">首页</asp:LinkButton>
                                            <asp:LinkButton ID="btnPrev" runat="server" CommandName="Page" CommandArgument="Prev" CssClass="pager-btn" OnClick="Pager_Click">上一页</asp:LinkButton>
                                            <asp:LinkButton ID="btnNext" runat="server" CommandName="Page" CommandArgument="Next" CssClass="pager-btn" OnClick="Pager_Click">下一页</asp:LinkButton>
                                            <asp:LinkButton ID="btnLast" runat="server" CommandName="Page" CommandArgument="Last" CssClass="pager-btn" OnClick="Pager_Click">尾页</asp:LinkButton>
                                        </div>
                                    </div>
                                </div>

                                <div class="stu-card">
                                    <div class="stu-card__head">
                                        <div>
                                            <h2 class="stu-card__title">批量操作</h2>
                                            <p class="stu-card__desc">密码管理、分组操作与数据导出</p>
                                        </div>
                                    </div>
                                    <div class="stu-action-bar">
                                        <div class="stu-action-group">
                                            <div class="stu-input-group">
                                                <span class="stu-input-group__label">初始密码</span>
                                                <asp:TextBox ID="TextBoxPwd" runat="server" CssClass="stu-input-group__input">12345</asp:TextBox>
                                            </div>
                                            <asp:Button ID="BtnSpwdInit" runat="server" OnClick="BtnSpwdInit_Click" Text="初始化本班密码" ToolTip="将本班所有学生的密码初始为左侧自定义密码" CssClass="stu-btn stu-btn--amber" />
                                            <asp:Button ID="BtnSpell" runat="server" OnClick="BtnSpell_Click" Text="转拼音缩写" ToolTip="将当前为原初始化密码的学生密码转换为其姓名拼音缩写" CssClass="stu-btn stu-btn--outline" />
                                        </div>
                                        <div class="stu-action-group">
                                            <div class="stu-input-group">
                                                <span class="stu-input-group__label">小组上限</span>
                                                <asp:DropDownList ID="DDLgroupMax" runat="server" CssClass="stu-input-group__input" style="min-width:60px;font-weight:800;text-align:center;cursor:pointer;" AutoPostBack="True" onselectedindexchanged="DDLgroupMax_SelectedIndexChanged">
                                                    <asp:ListItem>0</asp:ListItem><asp:ListItem>1</asp:ListItem><asp:ListItem>2</asp:ListItem><asp:ListItem>3</asp:ListItem><asp:ListItem>4</asp:ListItem><asp:ListItem>5</asp:ListItem><asp:ListItem Selected="True">6</asp:ListItem><asp:ListItem>7</asp:ListItem><asp:ListItem>8</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <asp:Button ID="Btngroups" runat="server" Text="分组管理" onclick="Btngroups_Click" CssClass="stu-btn stu-btn--primary" />
                                            <asp:Button ID="BtnNoGroup" runat="server" OnClick="BtnNoGroup_Click" Text="解除分组" ToolTip="一键将本班所有学生解除分组" CssClass="stu-btn stu-btn--danger" />
                                            <asp:Button ID="BtnExcel" runat="server" OnClick="BtnExcel_Click" Text="导出学生" ToolTip="将所有学生的基本信息导出Excel" CssClass="stu-btn stu-btn--green" />
                                            <asp:Button ID="BtnRevive" runat="server" Text="恢复学生" onclick="BtnRevive_Click" CssClass="stu-btn stu-btn--blue" />
                                        </div>
                                    </div>
                                </div>

                                <div class="stu-perm-card">
                                    <div class="stu-perm-row">
                                        <span class="stu-perm-label">权限设置</span>
                                        <div class="stu-perm-divider"></div>
                                        <label class="stu-check-item"><asp:CheckBox ID="Ckreg" runat="server" oncheckedchanged="Ckreg_CheckedChanged" AutoPostBack="True" />允许在线注册</label>
                                        <div class="stu-perm-divider"></div>
                                        <span class="stu-perm-label">个人资料修改</span>
                                        <label class="stu-check-item"><asp:CheckBox ID="Ckclass" runat="server" ToolTip="允许学生修改个人资料中的班级" oncheckedchanged="Ckclass_CheckedChanged" AutoPostBack="True" />改班级</label>
                                        <label class="stu-check-item"><asp:CheckBox ID="Ckphoto" runat="server" ToolTip="允许学生修改个人资料中的相片" oncheckedchanged="Ckphoto_CheckedChanged" AutoPostBack="True" />改相片</label>
                                        <label class="stu-check-item"><asp:CheckBox ID="Cksex" runat="server" ToolTip="允许学生修改个人资料中的性别" oncheckedchanged="Cksex_CheckedChanged" AutoPostBack="True" />改性别</label>
                                        <label class="stu-check-item"><asp:CheckBox ID="Ckname" runat="server" ToolTip="允许学生修改个人资料中的姓名" oncheckedchanged="Ckname_CheckedChanged" AutoPostBack="True" />改姓名</label>
                                    </div>
                                </div>

                                <asp:Label ID="Labelmsg" runat="server" CssClass="stu-msg"></asp:Label>
                            </div>
                        </div>
                    </div>
                </section>

                <section class="manage-section manage-section--student">
                    <div class="manage-section__head">
                        <div>
                            <h2 class="manage-section__title">学生工具</h2>
                            <p class="manage-section__desc">围绕课堂表现、统计、检查、评价与荣誉展示的快捷入口。</p>
                        </div>
                        <span class="manage-badge">Student</span>
                    </div>
                    <div class="manage-section__body">
                        <div class="manage-link-grid">
                            <asp:HyperLink ID="HLsignin" runat="server" NavigateUrl="~/teacher/signinmanage.aspx" CssClass="manage-link">
                                <span class="manage-link__icon"><svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M12 3l2.4 4.86L20 8.76l-4 3.9.94 5.47L12 15.9l-4.94 2.23L8 12.66l-4-3.9 5.6-.9L12 3z"></path></svg></span>
                                <span class="manage-link__text"><span class="manage-link__title">表现评价</span><span class="manage-link__desc">查看并管理学生课堂表现记录</span></span>
                            </asp:HyperLink>
                            <asp:HyperLink ID="HLstudentstats" runat="server" NavigateUrl="~/teacher/studentstats.aspx" CssClass="manage-link">
                                <span class="manage-link__icon"><svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M4 19h16"></path><path d="M7 16V8"></path><path d="M12 16V5"></path><path d="M17 16v-6"></path></svg></span>
                                <span class="manage-link__text"><span class="manage-link__title">学习统计</span><span class="manage-link__desc">统计学生学习数据与参与情况</span></span>
                            </asp:HyperLink>
                            <asp:HyperLink ID="HLpostclasscheck" runat="server" NavigateUrl="~/teacher/PostClassCheck.aspx" CssClass="manage-link">
                                <span class="manage-link__icon"><svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="11" cy="11" r="7"></circle><path d="m21 21-4.3-4.3"></path></svg></span>
                                <span class="manage-link__text"><span class="manage-link__title">课后检查</span><span class="manage-link__desc">课后作业与课堂成果抽查入口</span></span>
                            </asp:HyperLink>
                            <asp:HyperLink ID="HLclasscheck" runat="server" NavigateUrl="~/teacher/check.aspx" CssClass="manage-link">
                                <span class="manage-link__icon"><svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M20 6 9 17l-5-5"></path></svg></span>
                                <span class="manage-link__text"><span class="manage-link__title">课前检查</span><span class="manage-link__desc">上课前机房与学生状态检查</span></span>
                            </asp:HyperLink>
                            <asp:HyperLink ID="HLpingjia" runat="server" NavigateUrl="~/pingjia/pingjia.aspx" CssClass="manage-link">
                                <span class="manage-link__icon"><svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M4 5h16v12H7l-3 3V5z"></path><path d="M8 9h8"></path><path d="M8 13h5"></path></svg></span>
                                <span class="manage-link__text"><span class="manage-link__title">学生评价</span><span class="manage-link__desc">进入学生综合评价模块</span></span>
                            </asp:HyperLink>
                            <asp:HyperLink ID="HLhonorboard" runat="server" NavigateUrl="~/teacher/honorboardmanage.aspx" CssClass="manage-link">
                                <span class="manage-link__icon"><svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M8 21h8"></path><path d="M12 17v4"></path><path d="M7 4h10v3a5 5 0 0 1-10 0V4z"></path><path d="M17 5h3a2 2 0 0 1-2 2"></path><path d="M7 5H4a2 2 0 0 0 2 2"></path></svg></span>
                                <span class="manage-link__text"><span class="manage-link__title">荣誉榜设置</span><span class="manage-link__desc">配置荣誉榜展示与荣誉规则</span></span>
                            </asp:HyperLink>
                        </div>
                    </div>
                </section>

                <section class="manage-section manage-section--teach">
                    <div class="manage-section__head">
                        <div>
                            <h2 class="manage-section__title">教学管理</h2>
                            <p class="manage-section__desc">课表、登记与签到等教学过程类管理功能入口。</p>
                        </div>
                        <span class="manage-badge">Teaching</span>
                    </div>
                    <div class="manage-section__body">
                        <div class="manage-link-grid">
                            <asp:HyperLink ID="HLskdjshow" runat="server" NavigateUrl="~/teacher/skdjshow.aspx" CssClass="manage-link">
                                <span class="manage-link__icon"><svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M9 3h6v4H9z"></path><path d="M9 3H7a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h10a2 2 0 0 0 2-2V5a2 2 0 0 0-2-2h-2"></path><path d="M9 12h6"></path><path d="M9 16h4"></path></svg></span>
                                <span class="manage-link__text"><span class="manage-link__title">登记查看</span><span class="manage-link__desc">查看课堂登记与教学记录信息</span></span>
                            </asp:HyperLink>
                            <asp:HyperLink ID="HLsigninview" runat="server" NavigateUrl="~/teacher/signin.aspx" CssClass="manage-link">
                                <span class="manage-link__icon"><svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M12 2v20"></path><path d="m5 7 7-4 7 4-7 4-7-4z"></path></svg></span>
                                <span class="manage-link__text"><span class="manage-link__title">签到查看</span><span class="manage-link__desc">浏览学生签到历史和统计情况</span></span>
                            </asp:HyperLink>
                            <asp:HyperLink ID="HLlesson" runat="server" NavigateUrl="~/kcb/CourseSchedule.aspx" CssClass="manage-link">
                                <span class="manage-link__icon"><svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="3" y="5" width="18" height="16" rx="2"></rect><path d="M16 3v4"></path><path d="M8 3v4"></path><path d="M3 11h18"></path></svg></span>
                                <span class="manage-link__text"><span class="manage-link__title">课表管理</span><span class="manage-link__desc">维护机房课程表与上课时间安排</span></span>
                            </asp:HyperLink>
                        </div>
                    </div>
                </section>

                <section class="manage-section manage-section--system">
                    <div class="manage-section__head">
                        <div>
                            <h2 class="manage-section__title">系统管理</h2>
                            <p class="manage-section__desc">机房设备映射、广播模型等系统级支持工具。</p>
                        </div>
                        <span class="manage-badge">System</span>
                    </div>
                    <div class="manage-section__body">
                        <div class="manage-link-grid">
                            <asp:HyperLink ID="HLcomputer" runat="server" NavigateUrl="~/teacher/computers.aspx" CssClass="manage-link">
                                <span class="manage-link__icon"><svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><rect x="3" y="4" width="18" height="12" rx="2"></rect><path d="M8 20h8"></path><path d="M12 16v4"></path></svg></span>
                                <span class="manage-link__text"><span class="manage-link__title">机器名 IP 对应表</span><span class="manage-link__desc">查看计算机名称与 IP 地址对应关系</span></span>
                            </asp:HyperLink>
                            <asp:HyperLink ID="HLmythware" runat="server" NavigateUrl="~/teacher/mythware.aspx" CssClass="manage-link">
                                <span class="manage-link__icon"><svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M4 5h16v10H4z"></path><path d="M2 19h20"></path><path d="M8 9h8"></path></svg></span>
                                <span class="manage-link__text"><span class="manage-link__title">极域班级模型</span><span class="manage-link__desc">查看并维护极域广播相关模型数据</span></span>
                            </asp:HyperLink>
                        </div>
                    </div>
                </section>
            </div>
        </div>
    </div>

    <script type="text/javascript" src="../js/student.js"></script>
    <script type="text/javascript">
        (function () {
            var section = document.querySelector('.manage-student-panel');
            var btn = document.getElementById('toggleStudentPanel');
            if (!section || !btn) return;
            var key = 'teachermanage.studentpanel.open';

            function setOpen(open) {
                section.classList.toggle('is-open', open);
                btn.setAttribute('aria-expanded', open ? 'true' : 'false');
                btn.querySelector('span').textContent = open ? '收起名册' : '展开名册';
                try { localStorage.setItem(key, open ? '1' : '0'); } catch (e) {}
            }

            var params = new URLSearchParams(window.location.search);
            var hasQuerySelection = params.has('sgrade') || params.has('sclass');
            var stored = null;
            try { stored = localStorage.getItem(key); } catch (e) {}

            if (hasQuerySelection) {
                setOpen(true);
            } else {
                setOpen(stored === '1');
            }

            btn.addEventListener('click', function () {
                setOpen(!section.classList.contains('is-open'));
            });
        })();
    </script>
</asp:Content>
