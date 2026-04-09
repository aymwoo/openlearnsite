<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manage.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="divide.aspx.cs" Inherits="Manager_divide" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../js/fileupload.css" rel="stylesheet" />
    <style type="text/css">
        .mgr-page { --ls-bg: linear-gradient(180deg,#f8fbff 0%,#f3f7ff 100%); --ls-border: #dbe6f5; --ls-text: #0f172a; padding: 28px; background: var(--ls-bg); min-height: calc(100vh - 8rem); box-sizing: border-box; width: 100%; }
        .mgr-page * { box-sizing: border-box; }
        .mgr-shell { display: flex; flex-direction: column; gap: 20px; }
        .mgr-hero { border: 1px solid #bbf7d0; border-radius: 1rem; padding: 24px 28px; background: linear-gradient(135deg,#f0fdf4 0%,#dcfce7 100%); color: #14532d; box-shadow: 0 4px 16px rgba(5,150,105,.08); }
        .mgr-hero__title { margin: 0; font-size: 22px; font-weight: 800; display: flex; align-items: center; gap: 10px; }
        .mgr-hero__subtitle { margin: 6px 0 0; font-size: 14px; color: rgba(236,253,245,.85); }
        .mgr-card { border: 1px solid var(--ls-border); border-radius: 1rem; background: rgba(255,255,255,.96); box-shadow: 0 12px 30px rgba(15,23,42,.05); overflow: hidden; }
        .mgr-card__head { padding: 20px 24px; border-bottom: 1px solid #f1f5f9; }
        .mgr-card__title { margin: 0; font-size: 16px; font-weight: 800; color: var(--ls-text); }
        .mgr-card__body { padding: 24px; display: flex; flex-direction: column; gap: 16px; }
        .mgr-prose { font-size: 14px; line-height: 1.9; color: #334155; }
        .mgr-prose li { margin-bottom: 6px; }
        .mgr-toolbar { display: flex; align-items: center; gap: 12px; flex-wrap: wrap; }
        .mgr-btn { display: inline-flex; align-items: center; justify-content: center; min-height: 40px; padding: 0 18px; border-radius: 1rem; border: none; font-size: 14px; font-weight: 700; cursor: pointer; transition: transform .18s, box-shadow .18s; }
        .mgr-btn--green { background: linear-gradient(135deg,#16a34a 0%,#15803d 100%); color: #fff; box-shadow: 0 8px 16px rgba(22,163,74,.2); }
        .mgr-btn--green:hover { transform: translateY(-1px); }
        .mgr-msg { font-size: 14px; font-weight: 700; color: #dc2626; }
        .mgr-table-demo { border-collapse: collapse; font-size: 14px; }
        .mgr-table-demo th, .mgr-table-demo td { border: 1px solid #e2e8f0; padding: 8px 16px; text-align: center; }
        .mgr-table-demo th { background: #f8fafc; font-weight: 700; color: #64748b; }
    </style>
    <div class="mgr-page">
        <div class="mgr-shell">
            <div class="mgr-hero">
                <h1 class="mgr-hero__title"><i class="bi bi-shuffle" style="color:#6ee7b7;"></i> 重新分班</h1>
                <p class="mgr-hero__subtitle">通过Excel上传新分班数据，批量更新学生班级</p>
            </div>

            <div class="mgr-card">
                <div class="mgr-card__head"><h2 class="mgr-card__title">分班说明</h2></div>
                <div class="mgr-card__body">
                    <div class="mgr-prose">
                        <ol class="mgr-prose" style="padding-left:20px;">
                            <li>请在新学期学年升班后再进行操作。</li>
                            <li>请上传该年级段重新分班后的Excel表格。</li>
                            <li>进行分班操作后，用所教班级的教师账号进行确认。</li>
                            <li>学校所提供的重新分班表格中可能有新的插班生，需要教师在上课前，在教师平台的学生管理中添加。</li>
                            <li>在您分班操作前，请注意做好备份！</li>
                            <li>平台分班是根据上传的学生的班级替换原班级，请注意同姓名学生，不对同姓名学生进行分班。如果年级和班级格式错误，请处理Excel中年级班级格式。</li>
                        </ol>
                    </div>
                </div>
            </div>

            <div class="mgr-card">
                <div class="mgr-card__head"><h2 class="mgr-card__title">上传分班表格</h2></div>
                <div class="mgr-card__body">
                    <div class="mgr-toolbar" style="flex-direction:column;align-items:stretch;">
                        <div class="ls-upload" data-accept=".xls,.xlsx" data-label="点击或拖拽上传分班表格" data-hint="支持 xls / xlsx 格式">
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                        </div>
                        <asp:Button ID="Btndivide" runat="server" Text="重新分班" onclick="Btndivide_Click" CssClass="mgr-btn mgr-btn--green" />
                    </div>
                    <asp:Label ID="Labelmsg" runat="server" CssClass="mgr-msg"></asp:Label>
                    <div>
                        <p style="font-size:14px;font-weight:700;color:#334155;margin:0 0 10px;">分班表格 Excel 格式示例：</p>
                        <table class="mgr-table-demo">
                            <tr><th>年级</th><th>班级</th><th>姓名</th></tr>
                            <tr><td>8</td><td>1</td><td>张三</td></tr>
                            <tr><td>8</td><td>1</td><td>李四</td></tr>
                        </table>
                        <p style="font-size:12px;color:#64748b;margin:8px 0 0;">如果导入出错，请注意Excel中年级、班级列格式！</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="../js/fileupload.js"></script>
</asp:Content>
