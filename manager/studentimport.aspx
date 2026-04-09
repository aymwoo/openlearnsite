<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manage.master" AutoEventWireup="true" CodeFile="studentimport.aspx.cs" Inherits="Manager_studentimport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../js/fileupload.css" rel="stylesheet" />
    <link href="../App_Themes/Teacher/studentimport.css" rel="stylesheet" />
    
    <div class="mgr-page">
        <div class="mgr-shell">
            <div class="mgr-hero">
                <div class="mgr-hero__eyebrow">Student Import</div>
                <h1 class="mgr-hero__title">新生导入</h1>
                <p class="mgr-hero__subtitle">通过 Excel 批量导入学生信息。建议先下载模板，检查学号与班级字段，再执行导入。</p>
                <div class="mgr-hero__meta">
                    <span class="mgr-chip">支持 .xls</span>
                    <span class="mgr-chip">两步完成导入</span>
                    <span class="mgr-chip">自动检测重复学号</span>
                </div>
            </div>

            <div class="mgr-overview">
                <div class="mgr-overview__card">
                    <span class="mgr-overview__label">导入方式</span>
                    <strong class="mgr-overview__value">Excel 批量导入</strong>
                    <span class="mgr-overview__desc">适合新学期统一导入整班学生名单。</span>
                </div>
                <div class="mgr-overview__card">
                    <span class="mgr-overview__label">必填字段</span>
                    <strong class="mgr-overview__value">学号 / 年级 / 班级 / 姓名</strong>
                    <span class="mgr-overview__desc">入学年度、年级、班级必须是数字，学号建议不超过 12 位。</span>
                </div>
                <div class="mgr-overview__card">
                    <span class="mgr-overview__label">导入保护</span>
                    <strong class="mgr-overview__value">重复学号自动拦截</strong>
                    <span class="mgr-overview__desc">系统会同时检查 Excel 内部重复和平台已存在的重复数据。</span>
                </div>
            </div>

            <div class="mgr-card">
                <div class="mgr-card__head"><h2 class="mgr-card__title">导入步骤</h2></div>
                <div class="mgr-card__body">
                    <div class="mgr-steps">
                        <div class="mgr-step">
                            <span class="mgr-step__num">1</span>
                            <div class="mgr-step__body">
                                <span class="mgr-step__label">选择Excel文件并上传</span>
                                <span class="mgr-step__hint">先将 Excel 数据导入到临时学生表，成功后才允许执行正式导入。</span>
                                
                                <div class="ls-upload" data-accept=".xls" data-label="点击或拖拽上传 Excel 文件" data-hint="支持 .xls 格式">
                                    <asp:FileUpload ID="FileUpExcel" runat="server" />
                                </div>
                                
                                <div class="mgr-option-row">
                                    <asp:CheckBox ID="CheckBox1" runat="server" Text="密码转换为姓名拼音缩写" ToolTip="是否在获取数据时自动将密码转换为学生姓名拼音缩写" />
                                </div>
                                <asp:Button ID="ButtonInsert" runat="server" Text="上传 Excel" OnClick="ButtonInsert_Click" CssClass="mgr-btn mgr-btn--primary" ToolTip="上传并导入临时学生表" />
                            </div>
                        </div>
                        <div class="mgr-step">
                            <span class="mgr-step__num">2</span>
                            <div class="mgr-step__body">
                                <span class="mgr-step__label">确认数据并导入平台</span>
                                <span class="mgr-step__hint">只有在上传和预检查成功后，才建议执行正式导入。</span>
                                <asp:Button ID="ButtonAppend" runat="server" Text="导入数据" OnClick="ButtonAppend_Click" Enabled="False" CssClass="mgr-btn mgr-btn--green" ToolTip="将上传的学生临时表数据导入平台学生表中" />
                            </div>
                        </div>
                    </div>

                    <div id="Loading" class="mgr-loading" style="display:none;">
                        <asp:Image ID="Image2" runat="server" ImageUrl="~/images/load2.gif" />
                        <input id="Textcmd" class="mgr-loading__text" type="text" />
                    </div>
                </div>
            </div>

            <div class="mgr-panel-grid">
                <div class="mgr-card">
                    <div class="mgr-card__head"><h2 class="mgr-card__title">注意事项</h2></div>
                    <div class="mgr-card__body">
                        <div class="mgr-alert">
                            <div>导入 Excel 数据中必须包含：学号、入学年度、年级、班级、姓名、密码、性别。</div>
                            <div>入学年度、年级、班级必须为数字；学号必须为数字且尽量不超过 12 位。</div>
                        </div>
                        <asp:Label ID="Labelmsg" runat="server" CssClass="mgr-msg"></asp:Label>
                    </div>
                </div>

                <div class="mgr-card">
                    <div class="mgr-card__head"><h2 class="mgr-card__title">快捷操作</h2></div>
                    <div class="mgr-card__body">
                        <div class="mgr-action-list">
                            <span class="mgr-action-list__hint">建议先下载模板填写，再上传校验。如需重试，可清除本次导入的临时数据。</span>
                        </div>
                        <div class="mgr-action-row">
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/说明必读/学生导入模板.xls" Target="_blank" CssClass="mgr-btn mgr-btn--link">下载学生信息Excel模板</asp:HyperLink>
                            <asp:Button ID="ButtonClear" runat="server" Text="清除最近导入数据" OnClick="ButtonClear_Click" CssClass="mgr-btn mgr-btn--danger" ToolTip="只删除刚才导入的数据，以方便重新导入！" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="mgr-card">
                <div class="mgr-card__head">
                    <h2 class="mgr-card__title">导入数据检验重复列表</h2>
                    <p class="mgr-card__desc">当 Excel 内部或平台已有学号重复时，会在这里展示异常记录，便于回查和修正。</p>
                </div>
                <div class="mgr-grid-wrap">
                    <asp:GridView ID="GVrepeat" runat="server" CssClass="mgr-grid" GridLines="None" Width="100%"
                        Font-Size="13px" PageSize="25" EnableTheming="False" EnableViewState="False" AutoGenerateColumns="True"
                        EmptyDataText="当前没有重复数据。">
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
    
    <script src="../js/fileupload.js"></script>
</asp:Content>
