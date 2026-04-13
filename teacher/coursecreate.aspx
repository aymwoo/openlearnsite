<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="coursecreate.aspx.cs" Inherits="Teacher_coursecreate" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <style type="text/css">
        .course-create-page {
            --workspace-page-bg: linear-gradient(180deg, #f8fafc 0%, #eef2ff 100%);
            --workspace-hero-bg: linear-gradient(135deg, #312e81 0%, #4338ca 55%, #6366f1 100%);
            --workspace-primary-bg: #4f46e5;
            --workspace-primary-hover: #4338ca;
            --workspace-primary-shadow: 0 14px 24px -18px rgba(79, 70, 229, 0.85);
        }

        .course-create-shell {
            max-width: 980px;
            margin: 0 auto;
        }

        .course-create-grid {
            display: grid;
            grid-template-columns: repeat(2, minmax(0, 1fr));
            gap: 0.75rem;
        }

        .course-create-field-full {
            grid-column: 1 / -1;
        }

        .course-create-select {
            height: 2.25rem;
            min-width: 140px;
            width: 100%;
        }

        .course-create-msg {
            color: #ef4444;
            font-size: 0.875rem;
            margin-top: 0.5rem;
            display: block;
        }

        .course-create-actions {
            display: flex;
            gap: 16px;
            margin-top: 24px;
            padding-top: 24px;
            background: transparent;
            border: 0;
            border-top: 1px dashed #cbd5e1;
            border-radius: 0;
            box-shadow: none;
        }

        .course-create-primary-btn {
            padding: 10px 28px;
            background: linear-gradient(135deg, var(--workspace-primary-bg) 0%, var(--workspace-primary-hover) 100%);
            color: white;
            font-size: 15px;
            font-weight: 800;
            border: none;
            border-radius: 0.375rem;
            box-shadow: 0 4px 14px rgba(79, 70, 229, 0.25);
            cursor: pointer;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
        }
        .course-create-primary-btn:hover {
            transform: translateY(-2px);
            box-shadow: 0 8px 20px rgba(79, 70, 229, 0.35);
        }

        .course-create-secondary-btn {
            padding: 10px 28px;
            background: #ffffff;
            color: #475569;
            font-size: 15px;
            font-weight: 700;
            border: 1px solid #cbd5e1;
            border-radius: 0.375rem;
            box-shadow: 0 2px 4px rgba(0,0,0,0.02);
            cursor: pointer;
            transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
        }
        .course-create-secondary-btn:hover {
            background: #f8fafc;
            color: #0f172a;
            border-color: #94a3b8;
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(0,0,0,0.05);
        }

        @media (max-width: 768px) {
            .course-create-grid {
                grid-template-columns: 1fr;
            }
        }
    </style>

    <div class="course-create-page">
        <div class="course-create-shell">
            <section class="course-create-hero">
                <div class="course-create-hero-content">
                    <span class="course-create-eyebrow">Create Course Plan</span>
                    <h1 class="course-create-title">学案创建</h1>
                    <p class="course-create-subtitle">在不改变原有创建流程的前提下，使用更清晰的表单布局整理学案标题、分类、年级、课节和发布设置。</p>
                </div>
            </section>

            <section class="course-create-panel">
                <h2 class="course-create-section-title">基础信息</h2>
                <p class="course-create-section-desc">以下字段仍沿用原有提交逻辑与事件绑定，创建成功后继续跳转到学案编辑页。</p>

                <div class="course-create-msg">
                    <asp:Label ID="Labelmsg" runat="server"></asp:Label>
                </div>

                <div class="course-create-grid">
                    <div class="course-create-field course-create-field-full">
                        <label class="course-create-label" for="<%= Texttitle.ClientID %>">学案名称</label>
                        <asp:TextBox ID="Texttitle" runat="server" SkinID="TextBoxNormal" CssClass="course-create-input"></asp:TextBox>
                    </div>

                    <div class="course-create-field">
                        <label class="course-create-label" for="<%= DDLclass.ClientID %>">学案分类</label>
                        <asp:DropDownList ID="DDLclass" runat="server" CssClass="course-create-select"></asp:DropDownList>
                    </div>

                    <div class="course-create-field">
                        <label class="course-create-label" for="<%= DDLcobj.ClientID %>">教学年级</label>
                        <asp:DropDownList ID="DDLcobj" runat="server" AutoPostBack="True" onselectedindexchanged="DDLcobj_SelectedIndexChanged" CssClass="course-create-select"></asp:DropDownList>
                    </div>

                    <div class="course-create-field">
                        <label class="course-create-label" for="<%= DDLCks.ClientID %>">按排课节</label>
                        <asp:DropDownList ID="DDLCks" runat="server" CssClass="course-create-select"></asp:DropDownList>
                    </div>

                    <div class="course-create-field">
                        <span class="course-create-label">当前学期</span>
                        <div class="course-create-note">
                            <asp:Label ID="Labelterm" runat="server" Text="Label"></asp:Label>
                        </div>
                    </div>

                    <div class="course-create-field course-create-field-full">
                        <span class="course-create-label">发布设置</span>
                        <label class="course-create-publish" for="<%= Checkcpublish.ClientID %>">
                            <asp:CheckBox ID="Checkcpublish" runat="server" Text="是否发布" Checked="True" />
                        </label>
                    </div>
                </div>

                <div class="course-create-actions">
                    <asp:Button ID="BtnCreate" runat="server" Text="创建学案" onclick="BtnCreate_Click" CssClass="course-create-primary-btn" />
                    <asp:Button ID="Btnreturn" runat="server" Text="返回学案" onclick="Btnreturn_Click" CssClass="course-create-secondary-btn" />
                </div>
            </section>
        </div>
    </div>
</asp:Content>
