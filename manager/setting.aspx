<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manage.master" AutoEventWireup="true" CodeFile="setting.aspx.cs" Inherits="Manager_setting" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/setting.css" rel="stylesheet" />
    <div class="mgr-page">
        <div class="mgr-shell">
            <div class="mgr-hero">
                <div class="mgr-hero__eyebrow">System Settings</div>
                <h1 class="mgr-hero__title">系统设置</h1>
                <p class="mgr-hero__subtitle">配置平台全局参数，包括登录方式、学期、上传策略和访问限制。大部分修改会立即生效。</p>
                <div class="mgr-hero__meta">
                    <span class="mgr-chip">全局配置即时生效</span>
                    <span class="mgr-chip">支持登录与上传策略控制</span>
                    <span class="mgr-chip">适合学期初集中调整</span>
                </div>
            </div>

            <div class="mgr-overview">
                <div class="mgr-overview__card">
                    <span class="mgr-overview__label">登录策略</span>
                    <strong class="mgr-overview__value">控制学生与教师访问方式</strong>
                    <span class="mgr-overview__desc">可设置学生登录模式、单点登录限制，以及教师平台的网络访问约束。</span>
                </div>
                <div class="mgr-overview__card">
                    <span class="mgr-overview__label">资源策略</span>
                    <strong class="mgr-overview__value">下载与作品查看时间</strong>
                    <span class="mgr-overview__desc">统一设置资源下载限制、作品开放查看时间和上传控件模式。</span>
                </div>
                <div class="mgr-overview__card">
                    <span class="mgr-overview__label">操作提醒</span>
                    <strong class="mgr-overview__value">部分操作会影响全站</strong>
                    <span class="mgr-overview__desc">例如“全部学案收回隐藏”会批量影响学生端展示，请确认后执行。</span>
                </div>
            </div>

            <div class="mgr-card">
                <div class="mgr-card__head">
                    <h2 class="mgr-card__title">当前设置</h2>
                    <p class="mgr-card__desc">按分组维护平台参数，修改后会在下方显示即时结果提示。</p>
                </div>
                <div class="mgr-card__body">
                    <div class="mgr-section">
                        <div class="mgr-section__head">
                            <span class="mgr-section__title">基础信息</span>
                            <span class="mgr-section__hint">优先维护站点名称和当前学期等基础参数。</span>
                        </div>
                        <div class="mgr-field">
                            <span class="mgr-label">网站名称</span>
                            <asp:TextBox ID="TextBoxsite" runat="server" CssClass="mgr-input mgr-input--wide"></asp:TextBox>
                            <asp:Button ID="Buttonsite" runat="server" Text="修改" onclick="Buttonsite_Click" CssClass="mgr-btn mgr-btn--primary" />
                        </div>
                        <div class="mgr-field">
                            <span class="mgr-label">当前学期</span>
                            <asp:DropDownList ID="DDLterm" runat="server" CssClass="mgr-select" AutoPostBack="True" onselectedindexchanged="DDLterm_SelectedIndexChanged">
                                <asp:ListItem Value="1">第一学期</asp:ListItem>
                                <asp:ListItem Value="2">第二学期</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="mgr-section">
                        <div class="mgr-section__head">
                            <span class="mgr-section__title">登录与访问</span>
                            <span class="mgr-section__hint">控制学生和教师的登录方式及访问限制。</span>
                        </div>
                        <div class="mgr-field-grid">
                            <div class="mgr-field-card">
                                <span class="mgr-label">学生登录方式</span>
                                <asp:DropDownList ID="DDLLoginMode" runat="server" CssClass="mgr-select" AutoPostBack="True" onselectedindexchanged="DDLLoginMode_SelectedIndexChanged" ToolTip="选择学生登录方式">
                                    <asp:ListItem Value="0">个人密码</asp:ListItem>
                                    <asp:ListItem Value="1">班级密码</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="mgr-field-card">
                                <span class="mgr-label">账号登录限制</span>
                                <asp:CheckBox ID="CheckBoxSingleLogin" runat="server" AutoPostBack="True" oncheckedchanged="CheckBoxSingleLogin_CheckedChanged" Text="一个账号只能在一台电脑登录" ToolTip="选中则一个学生账号不能在多台电脑登录同个平台！" />
                            </div>
                            <div class="mgr-field-card mgr-field-card--full">
                                <span class="mgr-label">教师平台登录限制</span>
                                <div class="mgr-inline-row">
                                    <asp:CheckBox ID="CheckBoxLogin" runat="server" oncheckedchanged="CheckBoxLogin_CheckedChanged" Text="限制为跟服务器同网段才能登录" AutoPostBack="True" />
                                    <asp:Image ID="ImageLogin" runat="server" ImageUrl="~/images/green.gif" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="mgr-section">
                        <div class="mgr-section__head">
                            <span class="mgr-section__title">资源与作品策略</span>
                            <span class="mgr-section__hint">配置资源下载、作品查看开放时间和提交限制。</span>
                        </div>
                        <div class="mgr-field-grid">
                            <div class="mgr-field-card">
                                <span class="mgr-label">资源下载限制</span>
                                <asp:CheckBox ID="CheckBoxDownCan" runat="server" AutoPostBack="True" oncheckedchanged="CheckBoxDownCan_CheckedChanged" Text="是否限制下载" />
                            </div>
                            <div class="mgr-field-card">
                                <span class="mgr-label">资源下载时间</span>
                                <div class="mgr-inline-row">
                                    <asp:DropDownList ID="DDLDownTime" runat="server" AutoPostBack="True" CssClass="mgr-select" onselectedindexchanged="DDLDownTime_SelectedIndexChanged">
                                        <asp:ListItem Value="10">10分钟</asp:ListItem>
                                        <asp:ListItem Value="20">20分钟</asp:ListItem>
                                        <asp:ListItem Value="30">30分钟</asp:ListItem>
                                        <asp:ListItem Value="40">40分钟</asp:ListItem>
                                        <asp:ListItem Value="50">50分钟</asp:ListItem>
                                        <asp:ListItem Value="60">60分钟</asp:ListItem>
                                    </asp:DropDownList>
                                    <span class="mgr-help">之后可下载</span>
                                </div>
                            </div>
                            <div class="mgr-field-card">
                                <span class="mgr-label">作品查看时间</span>
                                <div class="mgr-inline-row">
                                    <asp:DropDownList ID="DDLworkdowntime" runat="server" CssClass="mgr-select" AutoPostBack="True" onselectedindexchanged="DDLworkdowntime_SelectedIndexChanged">
                                        <asp:ListItem>0</asp:ListItem><asp:ListItem>1</asp:ListItem><asp:ListItem>2</asp:ListItem><asp:ListItem>3</asp:ListItem><asp:ListItem>4</asp:ListItem><asp:ListItem>5</asp:ListItem><asp:ListItem>6</asp:ListItem><asp:ListItem>7</asp:ListItem><asp:ListItem>8</asp:ListItem><asp:ListItem>9</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>11</asp:ListItem><asp:ListItem>12</asp:ListItem>
                                    </asp:DropDownList>
                                    <span class="mgr-help">天后可以查看</span>
                                </div>
                            </div>
                            <div class="mgr-field-card mgr-field-card--full">
                                <span class="mgr-label">作品提交限制</span>
                                <asp:CheckBox ID="CheckBoxWorkIp" runat="server" AutoPostBack="True" oncheckedchanged="CheckBoxWorkIp_CheckedChanged" Text="对同班作品提交进行IP限制（同班一个IP限制提交一份作品）" />
                            </div>
                        </div>
                    </div>

                    <div class="mgr-section">
                        <div class="mgr-section__head">
                            <span class="mgr-section__title">会话与上传</span>
                            <span class="mgr-section__hint">调整学生登录有效时间和平台上传组件模式。</span>
                        </div>
                        <div class="mgr-field-grid">
                            <div class="mgr-field-card">
                                <span class="mgr-label">Cookies失效</span>
                                <asp:DropDownList ID="DDLCookiesPeriod" runat="server" AutoPostBack="True" CssClass="mgr-select" onselectedindexchanged="DDLCookiesPeriod_SelectedIndexChanged">
                                    <asp:ListItem Value="0">关闭失效</asp:ListItem>
                                    <asp:ListItem Value="1">45分钟</asp:ListItem>
                                    <asp:ListItem Value="2">1小时</asp:ListItem>
                                    <asp:ListItem Value="3">3小时</asp:ListItem>
                                    <asp:ListItem Value="4">5小时</asp:ListItem>
                                    <asp:ListItem Value="5">永久</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="mgr-field-card">
                                <span class="mgr-label">作品上传控件</span>
                                <asp:DropDownList ID="DDLUploadMode" runat="server" AutoPostBack="True" CssClass="mgr-select" onselectedindexchanged="DDLUploadMode_SelectedIndexChanged">
                                    <asp:ListItem Value="0">普通无刷新方式上传</asp:ListItem>
                                    <asp:ListItem Value="1">Plupload方式上传</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="mgr-section mgr-section--warn">
                        <div class="mgr-section__head">
                            <span class="mgr-section__title">批量操作</span>
                            <span class="mgr-section__hint">危险操作会立即影响全站课程展示，请谨慎执行。</span>
                        </div>
                        <div class="mgr-field">
                            <span class="mgr-label">全部学案收回</span>
                            <asp:Button ID="Btnpublish" runat="server" Text="一键收回隐藏" onclick="Btnpublish_Click" ToolTip="收回的学案只是在学生界面不显示，教师界面仍显示并可再设置成发布状态" CssClass="mgr-btn mgr-btn--amber" />
                        </div>
                    </div>

                    <asp:Label ID="Labelmsg" runat="server" CssClass="mgr-msg"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
