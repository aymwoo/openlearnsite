<%@ Page Language="C#" AutoEventWireup="true" CodeFile="questionimport.aspx.cs" Inherits="exam_question_questionimport" MasterPageFile="~/teacher/Teach.master" %><asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <style>
        .import-page { min-height: calc(100vh - 8rem); padding: 1.5rem; background: #f8fafc; }
        .import-shell { max-width: 1100px; margin: 0 auto; display: flex; flex-direction: column; gap: 1.25rem; }
        .import-hero, .help-box, .import-form, .result-box, .error-panel { border: 1px solid rgba(148, 163, 184, 0.18); border-radius: 1.25rem; background: #ffffff; box-shadow: 0 12px 32px -28px rgba(15, 23, 42, 0.28); }
        .import-hero { display: flex; justify-content: space-between; align-items: flex-start; gap: 1rem; padding: 1.5rem; background: linear-gradient(135deg, #ffffff 0%, #f8fafc 100%); }
        .import-title { margin: 0; color: #0f172a; font-size: 1.625rem; font-weight: 700; }
        .import-subtitle { margin: 0.75rem 0 0; color: #475569; font-size: 0.95rem; line-height: 1.7; }
        .hero-actions { display: flex; flex-wrap: wrap; gap: 0.75rem; }
        .page-btn, .form-actions input, .hero-actions a { display: inline-flex; align-items: center; justify-content: center; min-height: 2.75rem; padding: 0 1rem; border: 1px solid transparent; border-radius: 0.9rem; font-size: 0.875rem; font-weight: 600; text-decoration: none; cursor: pointer; transition: all 0.2s ease; }
        .page-btn-primary, .form-actions input[id$='btnImport'] { background: #2563eb; color: #ffffff; box-shadow: 0 10px 20px -14px rgba(37, 99, 235, 0.85); }
        .page-btn-secondary, .form-actions input[id$='btnClear'], .form-actions input[id$='btnContinue'], .hero-actions a { background: #ffffff; color: #475569; border-color: #cbd5e1; }
        .help-box, .import-form, .error-panel { padding: 1.5rem; }
        .help-box h4 { margin: 0 0 1rem 0; color: #0f172a; font-size: 1rem; }
        .help-box p { margin: 0.35rem 0; color: #475569; font-size: 0.88rem; line-height: 1.7; }
        .help-box pre { background: #f8fafc; padding: 1rem; border-radius: 0.9rem; overflow-x: auto; font-size: 0.78rem; color: #334155; }
        .form-group { margin-bottom: 1rem; }
        .form-group label { display: block; margin-bottom: 0.45rem; color: #334155; font-weight: 600; }
        .form-group input, .form-group select, .form-group textarea { width: 100%; padding: 0.75rem 0.9rem; border: 1px solid #cbd5e1; border-radius: 0.9rem; font-size: 0.875rem; background: #f8fafc; box-sizing: border-box; }
        .form-group textarea { min-height: 320px; font-family: Consolas, Monaco, monospace; font-size: 0.82rem; }
        .form-actions { display: flex; flex-wrap: wrap; gap: 0.75rem; margin-top: 1.25rem; }
        .result-box { padding: 1.5rem; }
        .result-success { background: linear-gradient(135deg, #ecfdf5 0%, #f0fdf4 100%); color: #15803d; }
        .result-error { background: linear-gradient(135deg, #fef2f2 0%, #fff5f5 100%); color: #dc2626; }
        .result-warning { background: linear-gradient(135deg, #fffbeb 0%, #fffef7 100%); color: #d97706; }
        .stats { margin-top: 1rem; padding: 1rem; background: rgba(255,255,255,0.8); border-radius: 0.9rem; }
        .stats span { margin-right: 1.25rem; }
        .error-panel { margin-top: 1.25rem; }
        @media (max-width: 900px) { .import-page { padding: 1rem; } .import-hero { flex-direction: column; } }
    </style>

    <div class="import-page">
        <div class="import-shell">
        <section class="import-hero">
            <div>
                <h2 class="import-title"><asp:Literal ID="ltlBankName" runat="server"></asp:Literal> - 批量导入题目</h2>
                <p class="import-subtitle">支持按约定格式一次性导入多种题型，减少重复录入成本，并保持题库维护页的视觉一致性。</p>
            </div>
            <div class="hero-actions">
                <a href="questionlist.aspx?bankId=<%= BankId %>" class="page-btn page-btn-secondary">返回列表</a>
            </div>
        </section>

        <asp:Panel ID="pnlImport" runat="server">
            <div class="help-box">
                <h4>导入格式说明</h4>
                <p>每行一道题目，使用竖线 | 分隔各字段。格式如下：</p>
                <pre>题型|题目内容|选项|答案|解析|分值|难度|知识点</pre>
                <p><strong>题型：</strong></p>
                <p>1=单选，2=多选，3=判断，4=填空，5=简答</p>
                <p>6=连线，7=分类，8=组合，9=多项填空，10=下拉选择</p>
                <p>11=打分题，12=矩阵单选，13=矩阵多选，14=NPS评分</p>
                <p><strong>选项格式：</strong>选项之间用 ## 分隔，如：A.选项1##B.选项2##C.选项3##D.选项4</p>
                <p><strong>答案格式：</strong></p>
                <p> - 单选/判断：直接写选项字母，如 A 或 B</p>
                <p> - 多选：多个答案用逗号分隔，如 A,B,C</p>
                <p> - 填空：多个空用竖线分隔，如 答案1|答案2|答案3</p>
                <p> - 多项填空：答案用竖线分隔，如 答案1|答案2|答案3</p>
                <p> - 简答：直接写答案文本</p>
                <p> - 打分题：写默认分值，如 3</p>
                <p> - NPS：写默认分值，如 8</p>
                <p> - 矩阵题：JSON格式，如 {"行1":"列2","行2":"列1"}</p>
                <p><strong>分值：</strong>数字，如 2 或 5</p>
                <p><strong>难度：</strong>1=简单，2=中等，3=困难</p>
                <p><strong>知识点：</strong>可选字段</p>
                <br/>
                <p><strong>示例：</strong></p>
                <pre>1|以下哪个是C#的数据类型？|A. var##B. dynamic##C. both##D. none|C|C#支持var和dynamic两种类型|2|1|C#基础
2|以下哪些是面向对象的特性？|A. 封装##B. 继承##C. 多态##D. 以上都是|A,B,C|面向对象三大特性|3|2|面向对象
3|C#是一种面向对象的编程语言。||对||2|1|C#基础
4|C#中string是___类型，int是___类型。|引用|值|string是引用类型，int是值类型|4|2|C#数据类型
5|请简述C#中接口和抽象类的区别。||接口只定义契约不包含实现，抽象类可以包含部分实现...|10|3|C#高级特性
9|Python中___是列表，___是字典，___是集合。|list|dict|set|Python基础数据类型|4|2|Python基础
11|请对本次服务进行评分（1-5分）|1##2##3##4##5|4|满意度调查|5|1|客户服务
14|您有多大可能向朋友推荐我们的产品？||8|NPS评分题|5|1|用户调研</pre>
            </div>

            <div class="import-form">
                <div class="form-group">
                    <label>题目内容（每行一道题）</label>
                    <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" placeholder="请按照格式粘贴题目内容..."></asp:TextBox>
                </div>

                <div class="form-actions">
                    <asp:Button ID="btnImport" runat="server" Text="开始导入" CssClass="page-btn page-btn-primary" OnClick="btnImport_Click" />
                    <a href="questionlist.aspx?bankId=<%= BankId %>" class="page-btn page-btn-secondary">返回列表</a>
                    <asp:Button ID="btnClear" runat="server" Text="清空内容" CssClass="page-btn page-btn-secondary" OnClick="btnClear_Click" />
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="pnlResult" runat="server" Visible="false">
            <div class="result-box" id="resultBox" runat="server">
                <asp:Literal ID="ltlResult" runat="server"></asp:Literal>
                <div class="stats" id="statsBox" runat="server">
                    <span>总数：<asp:Literal ID="ltlTotal" runat="server"></asp:Literal></span>
                    <span>成功：<asp:Literal ID="ltlSuccess" runat="server"></asp:Literal></span>
                    <span>失败：<asp:Literal ID="ltlFailed" runat="server"></asp:Literal></span>
                </div>
            </div>
            
            <div style="margin-top: 20px;">
                <a href="questionlist.aspx?bankId=<%= BankId %>" class="page-btn page-btn-primary">查看题目列表</a>
                <asp:Button ID="btnContinue" runat="server" Text="继续导入" CssClass="page-btn page-btn-secondary" OnClick="btnContinue_Click" />
            </div>

            <asp:Panel ID="pnlErrorDetails" runat="server" Visible="false" CssClass="error-panel">
                <h4>错误详情：</h4>
                <asp:Literal ID="ltlErrorDetails" runat="server"></asp:Literal>
            </asp:Panel>
        </asp:Panel>
        </div>
    </div>
</asp:Content>
