<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master"  StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="courseimport.aspx.cs" Inherits="Teacher_courseimport" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../js/fileupload.css" rel="stylesheet" />
    

    <div class="course-import-page">
        <div class="course-import-shell">
            <section class="course-import-hero">
                <div class="course-import-hero-content">
                    <span class="course-import-eyebrow">Import Course Package</span>
                    <h1 class="course-import-title">平台专用学案包导入</h1>
                    <p class="course-import-subtitle">保留原有导入、返回和结果展示逻辑，优化上传区、提示信息和导入结果列表的层次与可读性。</p>
                </div>
            </section>

            <section class="course-import-panel">
                <h2 class="course-import-section-title">导入设置</h2>
                <p class="course-import-section-desc">当前年级、文件上传与按钮事件都保持不变，仍使用平台原有导入处理流程。</p>

                <div class="course-import-grid">
                    <div class="course-import-field">
                        <span class="course-import-label">当前选择年级</span>
                        <div class="course-import-note">
                            <asp:Label ID="Labelgrade" runat="server" Font-Bold="False"></asp:Label>
                            <span style="margin-left: 0.35rem;">年级</span>
                        </div>
                    </div>

                    <div class="course-import-field" style="grid-column: 1 / -1; margin-top: 1rem;">
                        <span class="course-import-label" style="display:block; margin-bottom: 0.75rem; font-weight:700; color:#334155; font-size:14px;">选择学案包文件</span>
                        <div class="ls-upload" data-accept=".zip" data-label="点击或拖拽上传学案包" data-hint="仅支持 .zip 压缩包文件">
                            <asp:FileUpload ID="FudPackage" runat="server" />
                        </div>
                    </div>

                    <div class="course-import-action-group" style="grid-column: 1 / -1;">
                        <asp:Button ID="Btnimport" runat="server" onclick="Btnimport_Click" Text="立刻导入" CssClass="course-import-primary-btn" />
                        <asp:Button ID="Btnreturn" runat="server" onclick="Btnreturn_Click" Text="返回列表" CssClass="course-import-secondary-btn" />
                    </div>
                </div>
            </section>

            <section class="course-import-feedback">
                <h2 class="course-import-section-title">导入说明与结果</h2>
                <p class="course-import-section-desc">下方消息仍由原始后端导入结果输出，包括错误码、成功提示和用时信息。</p>
                <asp:Label ID="Labelmsg" runat="server" Font-Size="9pt" ForeColor="Red" Height="38px">*必须使用本平台生成的学案包*<br /><br />注意：学案包导入功能为版本向下兼容，不向上兼容！</asp:Label>
            </section>

            <section class="course-import-table-panel">
                <h2 class="course-import-section-title">当前导入的学案列表</h2>
                <p class="course-import-section-desc">每次成功导入后，仍按照原有逻辑展示当前新导入学案。</p>
                <div class="course-import-table-wrap custom-scrollbar">
                    <asp:GridView ID="GVCourse" runat="server"
                        AutoGenerateColumns="False" DataKeyNames="Cid"
                        PageSize="20" Width="100%" CellPadding="3"
                        EnableModelValidation="True" Font-Size="9pt" ForeColor="#111111"
                        GridLines="None" Caption="当前导入的学案列表：" CaptionAlign="Left" CssClass="course-import-gridview">
                        <AlternatingRowStyle BackColor="#FBFDFF" />
                        <Columns>
                            <asp:BoundField DataField="Cobj" HeaderText="年级">
                                <ItemStyle Width="30px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Cks" HeaderText="课节">
                                <ItemStyle Width="30px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Ctitle" HeaderText="学案" />
                            <asp:BoundField DataField="Cclass" HeaderText="类型" SortExpression="Cclass">
                                <ItemStyle Width="50px" />
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle BackColor="#F8FAFC" ForeColor="#475569" />
                        <RowStyle BackColor="#FFFFFF" Height="24px" />
                        <SelectedRowStyle BackColor="#DBEAFE" Font-Bold="True" ForeColor="#1E3A8A" />
                    </asp:GridView>
                </div>
                <asp:Label ID="LabelnewCids" runat="server" Visible="False"></asp:Label>
            </section>
        </div>
    </div>

<script src="../js/fileupload.js"></script>
</asp:Content>
