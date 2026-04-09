<%@ Page Language="C#" AutoEventWireup="true" CodeFile="upgrade.aspx.cs" Inherits="UpGrade" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="UTF-8" />
    <title>LearnSite信息技术学习平台数据库更新页面</title>
        <link rel="stylesheet" type="text/css" href="../App_Themes/Teacher/StyleSheet.css" />
    

    <link href="js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    
    <link rel="stylesheet" type="text/css" href="App_Themes/Teacher/upgrade.css" />
</head>
<body>
       <form id="form1" runat="server" > 
        <div class="upgrade-page">
            <div class="upgrade-shell">
                <div class="upgrade-hero">
                    <div class="upgrade-hero__top">
                        <div class="upgrade-hero__logo">
                            <asp:Image ID="Imagelogo" runat="server" ImageUrl="~/images/learnsite.gif" ToolTip="信息技术教学平台 LearnSite&#13;Powered By Asp.net2.0+Sql2005Express&#13;温州水乡设计编写 编程平台：Visual Studio 2008 C#" Height="24px" />
                        </div>
                    </div>
                    <h1 class="upgrade-title">数据库升级中心</h1>
                    <p class="upgrade-subtitle">
                        从旧版 LearnSite 升级到当前版本，系统会自动判断缺失的数据表、字段与索引并执行补齐。<br />
                    </p>
                </div>

                <div class="upgrade-card">
                    <div class="upgrade-card__head">
                        <h2 class="upgrade-card__title">升级说明</h2>
                        <p class="upgrade-card__desc">执行更新前建议先备份数据库。系统会自动判断是否需要升级，不会重复添加已有字段和索引。</p>
                    </div>
                    <div class="upgrade-card__body">
                        <div class="upgrade-tips__summary">
                            本页支持旧版 LearnSite 数据库升级、新版迁移补齐和首次安装初始化。建议先备份数据库，再根据下方预检查结果决定是否执行升级。
                        </div>
                        <div id="upgradeTipsMore" class="upgrade-tips__more">
                            <div class="upgrade-tips">
                                <div class="upgrade-tip upgrade-tip--warn">
                                    <span class="upgrade-tip__title">升级前必读</span>
                                    <div class="upgrade-tip__text">请先备份数据库，以防中断或误操作时可以快速恢复。</div>
                                </div>
                                <div class="upgrade-tip">
                                    <span class="upgrade-tip__title">自动升级内容</span>
                                    <div class="upgrade-tip__text">自动补齐表、字段、索引，并执行必要的初始化数据修复。</div>
                                </div>
                                <div class="upgrade-tip">
                                    <span class="upgrade-tip__title">全新安装说明</span>
                                    <div class="upgrade-tip__text">首次安装也需要执行本页，用于创建基础词典和初始化默认数据。</div>
                                </div>
                            </div>
                        </div>
                        <button type="button" class="upgrade-collapse-btn" data-open="0" onclick="return toggleUpgradeTips(this);"><span class="upgrade-collapse-btn__arrow">▶</span><span>展开升级说明</span></button>

                        <div class="upgrade-version-grid">
                            <div class="upgrade-version-card">
                                <span class="upgrade-version-card__label">当前连接数据库</span>
                                <div class="upgrade-version-card__value upgrade-version-card__value--compact"><%= ConnectedDatabaseName %></div>
                            </div>
                            <div class="upgrade-version-card">
                                <span class="upgrade-version-card__label">最近预检查时间</span>
                                <div class="upgrade-version-card__value upgrade-version-card__value--compact"><%= LastAnalyzeTime %></div>
                            </div>
                            <div class="upgrade-version-card upgrade-version-card--combined">
                                <span class="upgrade-version-card__label">数据库迁移版本对比</span>
                                <div class="upgrade-version-compare">
                                    <div class="upgrade-version-compare__item">
                                        <span class="upgrade-version-compare__hint">旧版本</span>
                                        <div class="upgrade-version-compare__value"><%= CurrentDbVersion %></div>
                                    </div>
                                    <div class="upgrade-version-compare__arrow">→</div>
                                    <div class="upgrade-version-compare__item">
                                        <span class="upgrade-version-compare__hint">新版本</span>
                                        <div class="upgrade-version-compare__value"><%= TargetDbVersion %></div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="upgrade-check-grid">
                            <div class="upgrade-check-card <%=(UpgradeMode.IndexOf("空库") >= 0 ? "upgrade-check-card--warn" : "") %>">
                                <div class="upgrade-check-card__icon"><%=(UpgradeMode.IndexOf("空库") >= 0 ? "!" : "DB") %></div>
                                <span class="upgrade-check-card__label">数据库识别结果</span>
                                <div class="upgrade-check-card__value"><%= UpgradeMode %></div>
                                <div class="upgrade-check-card__meta">系统会根据基础表、迁移记录和关键字段识别当前数据库所处阶段。</div>
                            </div>
                            <div class="upgrade-check-card <%=(UpgradeDecision == "数据库已是最新" ? "upgrade-check-card--success" : (RiskLevel == "高" ? "upgrade-check-card--danger" : (RiskLevel == "中" ? "upgrade-check-card--warn" : ""))) %>">
                                <div class="upgrade-check-card__icon"><%=(UpgradeDecision == "数据库已是最新" ? "✓" : (RiskLevel == "高" ? "!" : "Go")) %></div>
                                <span class="upgrade-check-card__label">升级结论</span>
                                <div class="upgrade-check-card__value"><%= UpgradeDecision %></div>
                                <div class="upgrade-check-card__meta">先看这个结论，再决定是否执行升级。</div>
                            </div>
                            <div class="upgrade-check-card <%=(RiskLevel == "低" ? "upgrade-check-card--success" : (RiskLevel == "高" ? "upgrade-check-card--danger" : (RiskLevel == "中" ? "upgrade-check-card--warn" : ""))) %>">
                                <div class="upgrade-check-card__icon"><%=(RiskLevel == "低" ? "✓" : (RiskLevel == "高" ? "!" : "~")) %></div>
                                <span class="upgrade-check-card__label">风险等级</span>
                                <div class="upgrade-check-card__value"><%= RiskLevel %></div>
                                <div class="upgrade-check-card__meta">高风险代表当前数据库不适合直接执行覆盖升级。</div>
                            </div>
                        </div>

                        <div style="margin-top:18px; text-align:left; max-width:760px; margin-left:auto; margin-right:auto;">
                            <div class="upgrade-card__title" style="font-size:16px; margin-bottom:10px;">升级前检查结果</div>
                            <%= UpgradeSummaryHtml %>
                        </div>

                        <div style="margin-top:18px; text-align:left; max-width:760px; margin-left:auto; margin-right:auto;">
                            <div class="upgrade-card__title" style="font-size:16px; margin-bottom:10px;">待执行迁移</div>
                            <%= PendingMigrationHtml %>
                        </div>

                        <div style="margin-top:18px; text-align:left; max-width:760px; margin-left:auto; margin-right:auto;">
                            <div class="upgrade-card__title" style="font-size:16px; margin-bottom:10px;">待执行迁移分组</div>
                            <div class="upgrade-check-grid">
                                <div class="upgrade-check-card">
                                    <span class="upgrade-check-card__label">结构变更</span>
                                    <%= PendingStructureHtml %>
                                </div>
                                <div class="upgrade-check-card">
                                    <span class="upgrade-check-card__label">初始化数据</span>
                                    <%= PendingDataHtml %>
                                </div>
                                <div class="upgrade-check-card">
                                    <span class="upgrade-check-card__label">性能优化</span>
                                    <%= PendingPerformanceHtml %>
                                </div>
                            </div>
                        </div>

                        <div style="margin-top:18px; text-align:left; max-width:760px; margin-left:auto; margin-right:auto;">
                            <div class="upgrade-card__title" style="font-size:16px; margin-bottom:10px; color:#b45309;">风险提示</div>
                            <%= UpgradeRiskHtml %>
                        </div>

                        <div style="margin-top:18px; text-align:left; max-width:760px; margin-left:auto; margin-right:auto;">
                            <div class="upgrade-card__title" style="font-size:16px; margin-bottom:10px;">本次重点升级项</div>
                            <ol class="upgrade-list">
                                <li>增加 AI 相关数据表与自定义技能表升级逻辑。</li>
                                <li>为 `MenuWorks` 增加 `Kseconds` 字段，支持学生环节停留秒级统计。</li>
                                <li>自动将已有 `Ktime` 历史数据回填为秒级值。</li>
                                <li>新增 `IX_MenuWorks_Klid_Ksid` 索引，优化 `/teacher/learnrate.aspx` 查询性能。</li>
                            </ol>
                        </div>

                        <div class="upgrade-actions">
                            <asp:Button ID="BtnAnalyze" runat="server" Font-Size="9pt" Text="重新检查数据库"
                                onclick="BtnAnalyze_Click" CssClass="upgrade-btn-secondary" />
                            <asp:Button ID="BtnExportReport" runat="server" Font-Size="9pt" Text="导出检查报告"
                                onclick="BtnExportReport_Click" CssClass="upgrade-btn-secondary" />
                            <asp:Button ID="Btnupgrade" runat="server" Font-Size="9pt" Text="确认后执行升级" OnClientClick="if(!confirm('请确认你已经完成数据库备份，并且当前连接的是需要升级的旧库。是否继续执行升级？')) return false; showUpgradeProgress();"
                                onclick="Btnupgrade_Click" CssClass="upgrade-btn-primary" />
                            <asp:Button ID="BtnCreateTable" runat="server" onclick="BtnCreateTable_Click" 
                                Text="创建数据表" CssClass="upgrade-btn-secondary" />
                        </div>

                        <asp:Label ID="Labelmsg" runat="server" Font-Bold="True" ForeColor="Red" CssClass="upgrade-msg"></asp:Label>
                    </div>
                </div>

                <asp:Panel ID="Panel1" runat="server" Visible="False" CssClass="upgrade-card">
                    <div class="upgrade-card__head">
                        <h2 class="upgrade-card__title">数据库连接配置</h2>
                        <p class="upgrade-card__desc">如果首页或教师页跳到了这里，通常就是数据库连接失败。请先在下面修改数据库服务器、实例名和账号信息。</p>
                    </div>
                    <div class="upgrade-card__body">
                        <div class="upgrade-form">
                            <div class="upgrade-form__row">
                                <label class="upgrade-form__label" for="TextBoxSqlServer">数据库服务器名称</label>
                                <asp:TextBox ID="TextBoxSqlServer" runat="server" CssClass="upgrade-input"></asp:TextBox>
                            </div>
                            <div class="upgrade-form__row">
                                <label class="upgrade-form__label" for="TextBoxDbName">数据库名称</label>
                                <asp:TextBox ID="TextBoxDbName" runat="server" CssClass="upgrade-input"></asp:TextBox>
                            </div>
                            <div class="upgrade-form__row">
                                <label class="upgrade-form__label" for="TextBoxDbUser">数据库用户</label>
                                <asp:TextBox ID="TextBoxDbUser" runat="server" ToolTip="默认为sa，以便管理员后台备份数据库，否则备份不了！" CssClass="upgrade-input">sa</asp:TextBox>
                                <div class="upgrade-form__hint">默认建议使用 `sa`，便于后续后台数据库备份功能。</div>
                            </div>
                            <div class="upgrade-form__row">
                                <label class="upgrade-form__label" for="TextBoxDbPwd">数据库密码</label>
                                <asp:TextBox ID="TextBoxDbPwd" runat="server" ToolTip="默认为sa" CssClass="upgrade-input"></asp:TextBox>
                            </div>
                            <div class="upgrade-actions" style="margin-top:8px;">
                                <asp:Button ID="Buttonedit" runat="server" Font-Size="9pt"
                                    onclick="Buttonedit_Click" Text="保存配置" CssClass="upgrade-btn-primary" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>

        <div id="upgradeProgressMask" class="upgrade-progress-mask" aria-live="polite" aria-busy="true">
            <div class="upgrade-progress-card">
                <p class="upgrade-progress-title">正在执行数据库升级</p>
                <p id="upgradeProgressDesc" class="upgrade-progress-desc">正在检查旧版本结构并准备升级环境...</p>
                <div class="upgrade-progress-bar">
                    <div id="upgradeProgressFill" class="upgrade-progress-bar__fill"></div>
                </div>
                <ul id="upgradeProgressSteps" class="upgrade-progress-steps">
                    <li class="upgrade-progress-step is-active"><span class="upgrade-progress-step__dot">1</span><span>检查旧版本结构</span></li>
                    <li class="upgrade-progress-step"><span class="upgrade-progress-step__dot">2</span><span>补齐历史字段和兼容补丁</span></li>
                    <li class="upgrade-progress-step"><span class="upgrade-progress-step__dot">3</span><span>执行新版本迁移与初始化</span></li>
                    <li class="upgrade-progress-step"><span class="upgrade-progress-step__dot">4</span><span>整理结果并完成跳转</span></li>
                </ul>
            </div>
        </div>
    <script type="text/javascript">
        window.__upgradeConfig = {
            btnupgradeId: "<%= Btnupgrade.ClientID %>"
        };
    </script>
    <script type="text/javascript" src="js/upgrade.js"></script>
    </form>
</body>
</html>
