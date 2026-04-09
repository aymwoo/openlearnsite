<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="gauge.aspx.cs" Inherits="Teacher_gauge" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link rel="stylesheet" type="text/css" href="/App_Themes/Teacher/gauge.css" />

    <div class="gauge-page">
        <div class="gauge-shell">
            <div class="gauge-hero">
                <h1 class="gauge-hero__title">
                    <i class="bi bi-ui-checks-grid" style="color: #c7d2fe;"></i> 自定义量化评价标准
                </h1>
                <p class="gauge-hero__subtitle">创建和管理作品互评的量化指标库，支持多种作品类型的评价维度配置</p>
            </div>

            <div class="gauge-card">
                <div class="gauge-card__head">
                    <div>
                        <h2 class="gauge-card__title">量规列表</h2>
                        <p class="gauge-card__desc">点击标题可编辑具体评价项目，已使用的量规无法删除</p>
                    </div>
                </div>
                <div class="gauge-card__body">
                    <asp:GridView ID="GVGauge" runat="server" 
                        AutoGenerateColumns="False" DataKeyNames="Gid" Width="100%" 
                        CssClass="gauge-grid" 
                        onrowcommand="GVGauge_RowCommand" EnableModelValidation="True" 
                        onrowdatabound="GVGauge_RowDataBound" GridLines="None">
                        <Columns>
                            <asp:BoundField HeaderText="序号">
                                <ItemStyle CssClass="font-mono text-slate-400" Width="80px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Gtype" HeaderText="分类">
                                <ItemStyle CssClass="font-semibold text-slate-700" />
                            </asp:BoundField>
                            <asp:HyperLinkField DataNavigateUrlFields="Gid"  
                                DataNavigateUrlFormatString="~/teacher/gaugeitem.aspx?gid={0}" 
                                DataTextField="Gtitle" HeaderText="标题">
                                <ItemStyle CssClass="gauge-link text-left" />
                            </asp:HyperLinkField>
                            <asp:BoundField DataField="Gcount" HeaderText="使用次数">
                                <ItemStyle CssClass="font-mono text-emerald-600 font-semibold" Width="100px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Gdate" HeaderText="日期">
                                <ItemStyle CssClass="text-slate-500 text-xs" Width="120px" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                    <asp:LinkButton ID="BtnEdit" runat="server" CausesValidation="false" 
                                        CommandArgument='<%# Eval("Gid") %>' CommandName="Del" 
                                        Text="✖" ToolTip="删除" CssClass="gauge-delete-btn"></asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle Width="80px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <div class="gauge-card">
                <div class="gauge-card__head">
                    <div>
                        <h2 class="gauge-card__title">添加量规</h2>
                        <p class="gauge-card__desc">选择作品类型并填写量规标题，创建新的评价标准</p>
                    </div>
                </div>
                <div class="gauge-card__body">
                    <div class="gauge-form">
                        <div class="gauge-field">
                            <span class="gauge-label">作品类型</span>
                            <asp:DropDownList ID="DDLtype" runat="server" CssClass="gauge-select"></asp:DropDownList>
                        </div>
                        <div class="gauge-field">
                            <span class="gauge-label">量规标题</span>
                            <asp:TextBox ID="TextBoxGtitle" runat="server" CssClass="gauge-input" placeholder="例如：Scratch游戏作品互评表"></asp:TextBox>
                        </div>
                        <asp:Button ID="Btnadd" runat="server" Text="添加量规" onclick="Btnadd_Click" UseSubmitBehavior="false" OnClientClick="return startGaugeSseGenerate();" CssClass="gauge-btn" />
                    </div>
                </div>
            </div>

            <div class="gauge-alert">
                <div class="gauge-alert__item">
                    <i class="bi bi-exclamation-triangle-fill text-amber-500 gauge-alert__icon"></i>
                    <span class="gauge-alert__text"><strong>注意：</strong>评价标准一旦被使用后，将无法删除，请慎重填写！</span>
                </div>
                <div class="gauge-alert__item">
                    <i class="bi bi-info-circle-fill text-blue-500 gauge-alert__icon"></i>
                    <span class="gauge-alert__text">当活动中未指定互评评价标准时，系统将自动选取相应作品类型中的第一条评价标准。</span>
                </div>
            </div>
        </div>
    </div>

    <div id="gaugeLoading" class="gauge-loading" aria-live="polite" aria-busy="true">
        <div class="gauge-loading__card">
            <div class="gauge-loading__spinner"></div>
            <p class="gauge-loading__title">正在生成量规</p>
            <p id="gaugeLoadingDesc" class="gauge-loading__desc">系统正在创建量规并调用 AI 自动生成评价项，请稍候，不要关闭当前页面。</p>
            <ul id="gaugeLoadingSteps" class="gauge-loading__steps">
                <li class="gauge-loading__step is-active" data-step="0">
                    <span class="gauge-loading__step-index">1</span>
                    <span>正在创建量规记录</span>
                </li>
                <li class="gauge-loading__step" data-step="1">
                    <span class="gauge-loading__step-index">2</span>
                    <span>正在调用 AI 生成评价项</span>
                </li>
                <li class="gauge-loading__step" data-step="2">
                    <span class="gauge-loading__step-index">3</span>
                    <span>正在写入量规项并准备跳转</span>
                </li>
            </ul>
        </div>
    </div>

    
    <script type="text/javascript">
        window.__gaugeConfig = {
            btnaddId: '<%= Btnadd.ClientID %>',
            textBoxGtitleId: '<%= TextBoxGtitle.ClientID %>',
            dDLtypeId: '<%= DDLtype.ClientID %>',
            gauge_generateUrl: '<%= ResolveUrl("~/teacher/gauge_generate.ashx") %>',
            btnaddUniqueId: '<%= Btnadd.UniqueID %>'
        };
    </script>
    <script type="text/javascript" src="../js/gauge.js"></script>
</asp:Content>
