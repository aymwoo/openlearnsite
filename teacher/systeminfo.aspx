<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" enableViewStateMac="false" CodeFile="systeminfo.aspx.cs" Inherits="Teacher_systeminfo" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <style>
        .sys-page {
            padding: 24px;
        }
        .sys-shell {
            display: flex;
            flex-direction: column;
            gap: 24px;
        }
        .sys-hero {
            position: relative;
            overflow: hidden;
            border-radius: 24px;
            padding: 28px 32px;
            background: linear-gradient(135deg, #0f172a 0%, #1d4ed8 52%, #38bdf8 100%);
            color: #eff6ff;
            box-shadow: 0 20px 45px rgba(15, 23, 42, 0.18);
        }
        .sys-hero::after {
            content: "";
            position: absolute;
            right: -60px;
            top: -60px;
            width: 220px;
            height: 220px;
            border-radius: 999px;
            background: rgba(255,255,255,0.08);
        }
        .sys-hero__content {
            position: relative;
            z-index: 1;
            display: flex;
            justify-content: space-between;
            gap: 24px;
            align-items: flex-start;
            flex-wrap: wrap;
        }
        .sys-hero__title {
            margin: 0;
            font-size: 32px;
            font-weight: 800;
            letter-spacing: 0.04em;
        }
        .sys-hero__subtitle {
            margin: 10px 0 0;
            max-width: 720px;
            color: rgba(239, 246, 255, 0.88);
            line-height: 1.7;
            font-size: 15px;
        }
        .sys-platform {
            display: inline-flex;
            align-items: center;
            gap: 10px;
            padding: 10px 14px;
            border-radius: 14px;
            background: rgba(15, 23, 42, 0.22);
            border: 1px solid rgba(255,255,255,0.16);
            font-weight: 700;
            white-space: nowrap;
        }
        .sys-grid {
            display: grid;
            grid-template-columns: repeat(12, minmax(0, 1fr));
            gap: 20px;
        }
        .sys-card {
            grid-column: span 12;
            background: #ffffff;
            border: 1px solid #e2e8f0;
            border-radius: 22px;
            box-shadow: 0 14px 35px rgba(15, 23, 42, 0.06);
            overflow: hidden;
        }
        .sys-card__head {
            display: flex;
            justify-content: space-between;
            align-items: center;
            gap: 16px;
            padding: 22px 24px 0;
        }
        .sys-card__title {
            margin: 0;
            font-size: 20px;
            font-weight: 800;
            color: #0f172a;
        }
        .sys-card__desc {
            margin: 6px 0 0;
            color: #64748b;
            font-size: 14px;
        }
        .sys-card__body {
            padding: 22px 24px 24px;
        }
        .sys-card--span-4 {
            grid-column: span 4;
        }
        .sys-card--span-8 {
            grid-column: span 8;
        }
        .sys-card--span-6 {
            grid-column: span 6;
        }
        .sys-stat-grid {
            display: grid;
            grid-template-columns: repeat(3, minmax(0, 1fr));
            gap: 14px;
        }
        .sys-stat-item {
            padding: 16px 18px;
            border-radius: 18px;
            background: linear-gradient(180deg, #f8fbff 0%, #eef6ff 100%);
            border: 1px solid #dbeafe;
        }
        .sys-stat-item__label {
            display: block;
            color: #64748b;
            font-size: 13px;
            margin-bottom: 6px;
        }
        .sys-stat-item__value {
            display: block;
            color: #1d4ed8;
            font-size: 26px;
            font-weight: 800;
            line-height: 1.2;
        }
        .sys-links {
            display: grid;
            grid-template-columns: repeat(2, minmax(0, 1fr));
            gap: 14px;
        }
        .sys-link {
            display: flex;
            align-items: center;
            justify-content: space-between;
            gap: 14px;
            padding: 18px 20px;
            border-radius: 18px;
            background: linear-gradient(135deg, #f8fafc 0%, #eef4ff 100%);
            border: 1px solid #dbeafe;
            text-decoration: none;
            color: #0f172a;
            transition: transform 0.18s ease, box-shadow 0.18s ease, border-color 0.18s ease;
        }
        .sys-link:hover {
            transform: translateY(-2px);
            box-shadow: 0 12px 22px rgba(59, 130, 246, 0.12);
            border-color: #93c5fd;
            color: #0f172a;
        }
        .sys-link__icon {
            width: 42px;
            height: 42px;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            border-radius: 14px;
            background: #dbeafe;
            color: #2563eb;
            font-size: 20px;
            font-weight: 700;
            flex-shrink: 0;
        }
        .sys-link__text {
            display: flex;
            flex-direction: column;
            gap: 4px;
            min-width: 0;
        }
        .sys-link__title {
            font-weight: 800;
            font-size: 15px;
        }
        .sys-link__desc {
            color: #64748b;
            font-size: 13px;
        }
        .sys-link__arrow {
            color: #94a3b8;
            font-size: 18px;
        }
        .sys-table {
            display: grid;
            gap: 12px;
        }
        .sys-table__row {
            display: grid;
            grid-template-columns: 160px 1fr 180px 1fr;
            gap: 12px 16px;
            align-items: stretch;
        }
        .sys-table__cell {
            padding: 14px 16px;
            border-radius: 16px;
            background: #f8fafc;
            border: 1px solid #e2e8f0;
            min-height: 54px;
            display: flex;
            align-items: center;
        }
        .sys-table__cell--label {
            color: #475569;
            font-weight: 700;
            background: #f1f5f9;
        }
        .sys-table__value {
            display: flex;
            align-items: center;
            gap: 10px;
            color: #0f172a;
            font-weight: 600;
            word-break: break-word;
        }
        .sys-status-dot {
            width: 12px;
            height: 12px;
            border-radius: 999px;
            box-shadow: 0 0 0 4px rgba(34, 197, 94, 0.12);
            flex-shrink: 0;
        }
        .sys-wide-value {
            white-space: pre-wrap;
            word-break: break-word;
            line-height: 1.7;
        }
        .sys-log-link {
            display: inline-flex;
            align-items: center;
            gap: 8px;
            padding: 10px 14px;
            border-radius: 12px;
            background: #eff6ff;
            color: #2563eb !important;
            text-decoration: none !important;
            font-weight: 700;
        }
        .sys-log-link:hover {
            background: #dbeafe;
        }
        @media (max-width: 1100px) {
            .sys-card--span-4,
            .sys-card--span-8,
            .sys-card--span-6 {
                grid-column: span 12;
            }
            .sys-table__row {
                grid-template-columns: 150px 1fr;
            }
        }
        @media (max-width: 720px) {
            .sys-page {
                padding: 16px;
            }
            .sys-hero {
                padding: 22px 20px;
                border-radius: 20px;
            }
            .sys-hero__title {
                font-size: 26px;
            }
            .sys-card__head,
            .sys-card__body {
                padding-left: 18px;
                padding-right: 18px;
            }
            .sys-stat-grid,
            .sys-links {
                grid-template-columns: 1fr;
            }
            .sys-table__row {
                grid-template-columns: 1fr;
            }
        }
    </style>

    <div class="sys-page">
        <div class="sys-shell">
            <section class="sys-hero">
                <div class="sys-hero__content">
                    <div>
                        <h1 class="sys-hero__title">系统信息中心</h1>
                        <p class="sys-hero__subtitle">集中查看平台运行状态、服务器环境、数据规模与系统资源占用，便于日常巡检和问题定位。</p>
                    </div>
                    <div class="sys-platform">
                        <span>运行平台</span>
                        <strong><asp:Label ID="Labelcomputer" runat="server"></asp:Label></strong>
                    </div>
                </div>
            </section>

            <div class="sys-grid">
                <section class="sys-card sys-card--span-8">
                    <div class="sys-card__head">
                        <div>
                            <h2 class="sys-card__title">网站分析统计</h2>
                            <p class="sys-card__desc">当前教学平台内核心数据总量一览。</p>
                        </div>
                    </div>
                    <div class="sys-card__body">
                        <div class="sys-stat-grid">
                            <div class="sys-stat-item">
                                <span class="sys-stat-item__label">学案总数</span>
                                <span class="sys-stat-item__value"><asp:Label ID="Label15" runat="server"></asp:Label></span>
                            </div>
                            <div class="sys-stat-item">
                                <span class="sys-stat-item__label">作品总数</span>
                                <span class="sys-stat-item__value"><asp:Label ID="Label16" runat="server"></asp:Label></span>
                            </div>
                            <div class="sys-stat-item">
                                <span class="sys-stat-item__label">学生总数</span>
                                <span class="sys-stat-item__value"><asp:Label ID="Label17" runat="server"></asp:Label></span>
                            </div>
                            <div class="sys-stat-item">
                                <span class="sys-stat-item__label">签到次数</span>
                                <span class="sys-stat-item__value"><asp:Label ID="Label18" runat="server"></asp:Label></span>
                            </div>
                            <div class="sys-stat-item">
                                <span class="sys-stat-item__label">打字次数</span>
                                <span class="sys-stat-item__value"><asp:Label ID="Label19" runat="server"></asp:Label></span>
                            </div>
                            <div class="sys-stat-item">
                                <span class="sys-stat-item__label">资源总数</span>
                                <span class="sys-stat-item__value"><asp:Label ID="Label20" runat="server"></asp:Label></span>
                            </div>
                        </div>
                    </div>
                </section>

                <section class="sys-card sys-card--span-4">
                    <div class="sys-card__head">
                        <div>
                            <h2 class="sys-card__title">快捷入口</h2>
                            <p class="sys-card__desc">常用机房与日志管理功能。</p>
                        </div>
                    </div>
                    <div class="sys-card__body">
                        <div class="sys-links">
                            <asp:HyperLink ID="HLcomputer" runat="server" NavigateUrl="~/teacher/computers.aspx" CssClass="sys-link" EnableTheming="False" EnableViewState="False">
                                <span class="sys-link__icon">IP</span>
                                <span class="sys-link__text">
                                    <span class="sys-link__title">机器名 IP 对应表</span>
                                    <span class="sys-link__desc">查看机房终端名称与地址映射</span>
                                </span>
                                <span class="sys-link__arrow">›</span>
                            </asp:HyperLink>
                            <asp:HyperLink ID="HLmythware" runat="server" NavigateUrl="~/teacher/mythware.aspx" CssClass="sys-link" EnableTheming="False" EnableViewState="False">
                                <span class="sys-link__icon">机</span>
                                <span class="sys-link__text">
                                    <span class="sys-link__title">极域班级模型</span>
                                    <span class="sys-link__desc">查看机房广播软件相关设置</span>
                                </span>
                                <span class="sys-link__arrow">›</span>
                            </asp:HyperLink>
                        </div>
                    </div>
                </section>

                <section class="sys-card sys-card--span-12">
                    <div class="sys-card__head">
                        <div>
                            <h2 class="sys-card__title">服务器状态</h2>
                            <p class="sys-card__desc">当前主机环境、Asp.Net 进程资源占用与运行时指标。</p>
                        </div>
                    </div>
                    <div class="sys-card__body">
                        <div class="sys-table">
                            <div class="sys-table__row">
                                <div class="sys-table__cell sys-table__cell--label">服务器 IP</div>
                                <div class="sys-table__cell"><div class="sys-table__value"><asp:Label ID="Label1" runat="server"></asp:Label><asp:Image ID="ImageLogin" runat="server" CssClass="sys-status-dot" ImageUrl="~/images/green.gif" /></div></div>
                                <div class="sys-table__cell sys-table__cell--label">.NET 引擎版本</div>
                                <div class="sys-table__cell"><div class="sys-table__value"><asp:Label ID="Label8" runat="server"></asp:Label></div></div>
                            </div>
                            <div class="sys-table__row">
                                <div class="sys-table__cell sys-table__cell--label">服务器名称</div>
                                <div class="sys-table__cell"><div class="sys-table__value"><asp:Label ID="Label2" runat="server"></asp:Label></div></div>
                                <div class="sys-table__cell sys-table__cell--label">脚本超时时间</div>
                                <div class="sys-table__cell"><div class="sys-table__value"><asp:Label ID="Label9" runat="server"></asp:Label></div></div>
                            </div>
                            <div class="sys-table__row">
                                <div class="sys-table__cell sys-table__cell--label">操作系统</div>
                                <div class="sys-table__cell"><div class="sys-table__value"><asp:Label ID="Label3" runat="server"></asp:Label></div></div>
                                <div class="sys-table__cell sys-table__cell--label">开机运行时长</div>
                                <div class="sys-table__cell"><div class="sys-table__value"><asp:Label ID="Label10" runat="server"></asp:Label></div></div>
                            </div>
                            <div class="sys-table__row">
                                <div class="sys-table__cell sys-table__cell--label">CPU 数</div>
                                <div class="sys-table__cell"><div class="sys-table__value"><asp:Label ID="Label4" runat="server"></asp:Label></div></div>
                                <div class="sys-table__cell sys-table__cell--label">进程开始时间</div>
                                <div class="sys-table__cell"><div class="sys-table__value"><asp:Label ID="Label11" runat="server"></asp:Label></div></div>
                            </div>
                            <div class="sys-table__row">
                                <div class="sys-table__cell sys-table__cell--label">CPU 类型</div>
                                <div class="sys-table__cell"><div class="sys-table__value"><asp:Label ID="Label5" runat="server"></asp:Label></div></div>
                                <div class="sys-table__cell sys-table__cell--label">AspNet 内存占用</div>
                                <div class="sys-table__cell"><div class="sys-table__value"><asp:Label ID="Label12" runat="server"></asp:Label></div></div>
                            </div>
                            <div class="sys-table__row">
                                <div class="sys-table__cell sys-table__cell--label">信息服务软件</div>
                                <div class="sys-table__cell"><div class="sys-table__value"><asp:Label ID="Label7" runat="server"></asp:Label></div></div>
                                <div class="sys-table__cell sys-table__cell--label">AspNet CPU 时间</div>
                                <div class="sys-table__cell"><div class="sys-table__value"><asp:Label ID="Label13" runat="server"></asp:Label></div></div>
                            </div>
                            <div class="sys-table__row">
                                <div class="sys-table__cell sys-table__cell--label">服务器区域语言</div>
                                <div class="sys-table__cell"><div class="sys-table__value"><asp:Label ID="Label21" runat="server"></asp:Label></div></div>
                                <div class="sys-table__cell sys-table__cell--label">AspNet 当前线程数</div>
                                <div class="sys-table__cell"><div class="sys-table__value"><asp:Label ID="Label14" runat="server"></asp:Label></div></div>
                            </div>
                            <div class="sys-table__row">
                                <div class="sys-table__cell sys-table__cell--label">网站平台版本</div>
                                <div class="sys-table__cell"><div class="sys-table__value"><asp:Label ID="Label6" runat="server"></asp:Label></div></div>
                                <div class="sys-table__cell sys-table__cell--label">Session 总数</div>
                                <div class="sys-table__cell"><div class="sys-table__value"><asp:Label ID="Label22" runat="server"></asp:Label></div></div>
                            </div>
                            <div class="sys-table__row">
                                <div class="sys-table__cell sys-table__cell--label">全局变量数</div>
                                <div class="sys-table__cell"><div class="sys-table__value"><asp:Label ID="Label23" runat="server"></asp:Label></div></div>
                                <div class="sys-table__cell sys-table__cell--label">网站异常记录</div>
                                <div class="sys-table__cell">
                                    <asp:HyperLink ID="HLsitelog" runat="server" NavigateUrl="~/teacher/sitelog.aspx" EnableTheming="False" EnableViewState="False" Target="_blank" ToolTip="请及时向温州水乡回复修正！" CssClass="sys-log-link">查看异常日志</asp:HyperLink>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>

                <section class="sys-card sys-card--span-12">
                    <div class="sys-card__head">
                        <div>
                            <h2 class="sys-card__title">系统详细信息</h2>
                            <p class="sys-card__desc">补充展示系统内核、资源占用、负载与进程概览。</p>
                        </div>
                    </div>
                    <div class="sys-card__body">
                        <div class="sys-table">
                            <div class="sys-table__row">
                                <div class="sys-table__cell sys-table__cell--label">系统版本</div>
                                <div class="sys-table__cell"><div class="sys-table__value"><asp:Label ID="Label24" runat="server"></asp:Label></div></div>
                                <div class="sys-table__cell sys-table__cell--label">内存使用率</div>
                                <div class="sys-table__cell"><div class="sys-table__value"><asp:Label ID="Label25" runat="server"></asp:Label></div></div>
                            </div>
                            <div class="sys-table__row">
                                <div class="sys-table__cell sys-table__cell--label">磁盘占用</div>
                                <div class="sys-table__cell"><div class="sys-table__value"><asp:Label ID="Label26" runat="server"></asp:Label></div></div>
                                <div class="sys-table__cell sys-table__cell--label">系统负载</div>
                                <div class="sys-table__cell"><div class="sys-table__value"><asp:Label ID="Label27" runat="server"></asp:Label></div></div>
                            </div>
                            <div class="sys-table__row">
                                <div class="sys-table__cell sys-table__cell--label">网络使用率</div>
                                <div class="sys-table__cell" style="grid-column: span 3;"><div class="sys-table__value sys-wide-value"><asp:Label ID="Label28" runat="server"></asp:Label></div></div>
                            </div>
                            <div class="sys-table__row">
                                <div class="sys-table__cell sys-table__cell--label">进程 TOP10</div>
                                <div class="sys-table__cell" style="grid-column: span 3;"><div class="sys-table__value sys-wide-value"><asp:Label ID="Label29" runat="server"></asp:Label></div></div>
                            </div>
                        </div>
                    </div>
                </section>
            </div>
        </div>
    </div>
</asp:Content>
