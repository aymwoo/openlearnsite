<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manage.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="Manager_index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <style type="text/css">
        .mgr-page { padding: 28px; background: var(--ls-bg); min-height: calc(100vh - 8rem); box-sizing: border-box; width: 100%; }
        .mgr-page * { box-sizing: border-box; }
        .mgr-shell { display: flex; flex-direction: column; gap: 20px; }
        .mgr-hero { position: relative; overflow: hidden; border: 1px solid #c7d2fe; border-radius: 1rem; padding: 24px 28px; background: linear-gradient(135deg,#eef2ff 0%,#e0e7ff 100%); color: #312e81; box-shadow: 0 4px 16px rgba(99,102,241,.08); }
        .mgr-hero__title { margin: 0; font-size: 22px; font-weight: 800; letter-spacing: -.02em; display: flex; align-items: center; gap: 10px; }
        .mgr-hero__subtitle { margin: 6px 0 0; font-size: 14px; color: rgba(238,242,255,.85); }
        .mgr-card { border: 1px solid var(--ls-border); border-radius: 1rem; background: rgba(255,255,255,.96); box-shadow: 0 12px 30px rgba(15,23,42,.05); overflow: hidden; }
        .mgr-card__head { padding: 20px 24px; border-bottom: 1px solid #f1f5f9; }
        .mgr-card__title { margin: 0; font-size: 16px; font-weight: 800; color: var(--ls-text); }
        .mgr-card__body { padding: 20px 24px; }
        .mgr-prose { font-size: 14px; line-height: 1.9; color: #334155; }
        .mgr-prose b { color: #0f172a; }
        .mgr-flow { display: flex; flex-wrap: wrap; gap: 8px; align-items: center; margin-top: 16px; }
        .mgr-flow__step { display: inline-flex; align-items: center; justify-content: center; padding: 6px 16px; border-radius: 1rem; background: #eef2ff; border: 1px solid #c7d2fe; color: #3730a3; font-size: 14px; font-weight: 700; }
        .mgr-flow__arrow { color: #94a3b8; font-size: 16px; }
        .mgr-btn { display: inline-flex; align-items: center; justify-content: center; min-height: 40px; padding: 0 20px; border-radius: 1rem; border: none; font-size: 14px; font-weight: 700; cursor: pointer; transition: transform .18s, box-shadow .18s; }
        .mgr-btn--danger { background: linear-gradient(135deg,#dc2626 0%,#b91c1c 100%); color: #fff; box-shadow: 0 8px 16px rgba(220,38,38,.2); }
        .mgr-btn--danger:hover { transform: translateY(-1px); box-shadow: 0 10px 20px rgba(220,38,38,.3); }
        .mgr-alert { padding: 14px 18px; border-radius: 1rem; background: #fefce8; border: 1px solid #fde68a; font-size: 14px; color: #78350f; line-height: 1.7; }
    </style>
    <div class="mgr-page">
        <div class="mgr-shell">
            <div class="mgr-hero">
                <h1 class="mgr-hero__title"><i class="bi bi-grid-3x3-gap-fill" style="color:#a5b4fc;"></i> 系统管理后台</h1>
                <p class="mgr-hero__subtitle">LearnSite 信息科技学习平台 · 管理员控制台</p>
            </div>

            <div class="mgr-card">
                <div class="mgr-card__head"><h2 class="mgr-card__title">平台简介</h2></div>
                <div class="mgr-card__body">
                    <div class="mgr-prose">
                        本系统基于三层架构原理编写，可扩展性强。系统分三种角色登录：学生、教师、管理员。<br /><br />
                        <b>教师平台功能：</b>学案管理、学生管理、作品管理、签到管理、网页管理、打字管理、资源管理七大功能。<br /><br />
                        <b>管理后台功能：</b>系统设置、教师管理、班级设置、新生导入、空间生成、学年升班七项。<br /><br />
                        历史版本：ITMS1.0 → Magnet2.2 → LearnSite1.100 → LearnSite1.3.3.3 &nbsp; 2009年8月——2021年11月 &nbsp; 温州水乡
                    </div>
                    <div class="mgr-flow">
                        <span class="mgr-flow__step">班级设置</span>
                        <span class="mgr-flow__arrow">→</span>
                        <span class="mgr-flow__step">教师管理</span>
                        <span class="mgr-flow__arrow">→</span>
                        <span class="mgr-flow__step">新生导入</span>
                        <span class="mgr-flow__arrow">→</span>
                        <span class="mgr-flow__step">空间生成</span>
                    </div>
                </div>
            </div>

            <div class="mgr-card">
                <div class="mgr-card__head"><h2 class="mgr-card__title">操作流程说明</h2></div>
                <div class="mgr-card__body">
                    <div class="mgr-prose">
                        <b>管理员：</b>创建全校完整班级列表 → 添加教师并给教师选择指定的班级 → 使用学生Excel模板导入新生<br /><br />
                        <b>教师：</b>教师平台登录 → 备课（使用活动、调查、讨论完成学案设计）→ 上课（查看签到、查看作品等情况）→ 评价 → 反思<br /><br />
                        <b>学生：</b>学生平台登录 → 浏览当前学案 → 完成导学及准备部分 → 完成课堂活动、调查、讨论等 → 作品展示 → 师生互动小结
                    </div>
                </div>
            </div>

            <div class="mgr-card">
                <div class="mgr-card__head"><h2 class="mgr-card__title">系统退出</h2></div>
                <div class="mgr-card__body" style="display:flex;flex-direction:column;gap:14px;">
                    <div class="mgr-alert">退出后将清除当前管理员登录状态，需重新登录才能进入后台。</div>
                    <asp:Button ID="Btnlogout" runat="server" Text="退出登录" onclick="Btnlogout_Click" CssClass="mgr-btn mgr-btn--danger" style="width:fit-content;" />
                </div>
            </div>

            <asp:TextBox ID="TextBox1" runat="server" ReadOnly="true" Width="1px" style="display:none;"></asp:TextBox>
            <asp:TextBox ID="TextBox3" runat="server" ReadOnly="True" Width="1px" style="display:none;"></asp:TextBox>
            <asp:TextBox ID="TextBox7" runat="server" ReadOnly="True" Width="1px" style="display:none;"></asp:TextBox>
            <asp:TextBox ID="TextBox2" runat="server" ReadOnly="True" Width="1px" style="display:none;"></asp:TextBox>
            <asp:TextBox ID="TextBox8" runat="server" ReadOnly="True" Width="1px" style="display:none;"></asp:TextBox>
            <asp:TextBox ID="TextBox4" runat="server" ReadOnly="True" Width="1px" style="display:none;"></asp:TextBox>
            <asp:TextBox ID="TextBox9" runat="server" ReadOnly="True" Width="1px" style="display:none;"></asp:TextBox>
            <asp:TextBox ID="TextBox5" runat="server" ReadOnly="True" Width="1px" style="display:none;"></asp:TextBox>
        </div>
    </div>
</asp:Content>
