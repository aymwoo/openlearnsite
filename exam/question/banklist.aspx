<%@ Page Language="C#" AutoEventWireup="true" CodeFile="banklist.aspx.cs" Inherits="exam_question_banklist" MasterPageFile="~/teacher/Teach.master" %><asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- 使用绝对路径重新引用JS文件，覆盖Master中的相对路径 -->
    <script src="/js/MenuCookie.js" type="text/javascript"></script>
    <script src="/js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="/kindeditor/plugins/code/prettify.js" type="text/javascript"></script>
    <script src="/js/ruffle.js" type="text/javascript"></script>
</asp:Content><asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <style>
        .bank-page { min-height: calc(100vh - 8rem); padding: 1.5rem; background: #f8fafc; }
        .bank-shell { display: flex; flex-direction: column; gap: 1.25rem; }
        .bank-hero, .select-tip, .bank-card, .modal-content, .empty-panel { border: 1px solid rgba(148, 163, 184, 0.18); border-radius: 1.25rem; background: #ffffff; box-shadow: 0 12px 32px -28px rgba(15, 23, 42, 0.28); }
        .bank-hero { display: flex; justify-content: space-between; align-items: flex-start; gap: 1rem; padding: 1.5rem; background: linear-gradient(135deg, #ffffff 0%, #f8fafc 100%); }
        .bank-title-main { margin: 0; color: #0f172a; font-size: 1.625rem; font-weight: 700; }
        .bank-subtitle { margin: 0.75rem 0 0; color: #475569; font-size: 0.95rem; line-height: 1.7; }
        .hero-actions { display: flex; flex-wrap: wrap; gap: 0.75rem; }
        .page-btn, .hero-actions button, .hero-actions a, .modal-footer button, .modal-footer input { display: inline-flex; align-items: center; justify-content: center; min-height: 2.75rem; padding: 0 1rem; border: 1px solid transparent; border-radius: 0.9rem; font-size: 0.875rem; font-weight: 600; text-decoration: none; cursor: pointer; transition: all 0.2s ease; }
        .page-btn-primary, .hero-actions button, .modal-footer input { background: #2563eb; color: #ffffff; box-shadow: 0 10px 20px -14px rgba(37, 99, 235, 0.85); }
        .page-btn-secondary, .hero-actions a, .modal-footer button { background: #ffffff; color: #475569; border-color: #cbd5e1; }
        .page-btn-secondary:hover, .hero-actions a:hover, .modal-footer button:hover { color: #1e293b; border-color: #94a3b8; background: #f8fafc; }
        .bank-grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(300px, 1fr)); gap: 1rem; }
        .bank-card { padding: 1.25rem; cursor: pointer; transition: transform 0.2s ease, box-shadow 0.2s ease; }
        .bank-card:hover { transform: translateY(-2px); box-shadow: 0 18px 36px -26px rgba(15, 23, 42, 0.32); }
        .bank-card .name { font-size: 1rem; font-weight: 700; color: #0f172a; margin-bottom: 0.75rem; }
        .bank-card .info { font-size: 0.84rem; color: #64748b; margin-bottom: 0.35rem; line-height: 1.7; }
        .bank-card .count { display: inline-flex; align-items: center; justify-content: center; background: #dbeafe; color: #1d4ed8; padding: 0.28rem 0.75rem; border-radius: 999px; font-size: 0.75rem; font-weight: 700; margin-top: 0.75rem; }
        .bank-card .actions { margin-top: 1rem; padding-top: 1rem; border-top: 1px solid #e2e8f0; }
        .bank-card .actions a, .bank-card .actions input { display: inline-flex; align-items: center; justify-content: center; margin-right: 0.6rem; color: #2563eb; text-decoration: none; font-size: 0.8rem; font-weight: 600; }
        .bank-card .actions a.danger, .bank-card .actions input.danger { color: #dc2626; }
        .bank-card.select-mode { border: 2px solid #22c55e; }
        .bank-card.select-mode .name::after { content: ' (点击选择题目)'; font-size: 12px; color: #16a34a; }
        .select-tip { padding: 1rem 1.25rem; background: linear-gradient(135deg, #eff6ff 0%, #f8fbff 100%); color: #1d4ed8; }
        .empty-panel { padding: 3rem 1.5rem; text-align: center; color: #94a3b8; }
        .modal { display: none; position: fixed; top: 0; left: 0; right: 0; bottom: 0; background: rgba(15, 23, 42, 0.45); z-index: 1000; align-items: center; justify-content: center; padding: 1rem; }
        .modal.show { display: flex; }
        .modal-content { width: 420px; max-width: 95%; padding: 1.5rem; }
        .modal-header { margin-bottom: 1rem; }
        .modal-header h3 { margin: 0; color: #0f172a; font-size: 1.1rem; }
        .form-group { margin-bottom: 1rem; }
        .form-group label { display: block; margin-bottom: 0.45rem; color: #334155; font-weight: 600; }
        .form-control { width: 100%; min-height: 2.75rem; padding: 0.7rem 0.9rem; border: 1px solid #cbd5e1; border-radius: 0.9rem; box-sizing: border-box; background: #f8fafc; color: #0f172a; }
        .modal-footer { margin-top: 1.25rem; display: flex; justify-content: flex-end; gap: 0.75rem; }
        @media (max-width: 900px) { .bank-page { padding: 1rem; } .bank-hero { flex-direction: column; } }
    </style>

    <div class="bank-page">
        <div class="bank-shell">
        <section class="bank-hero">
            <div>
                <h2 class="bank-title-main"><asp:Literal ID="ltlPageTitle" runat="server" Text="题库管理"></asp:Literal></h2>
                <p class="bank-subtitle">管理题库、批量导入课堂测验题目，并为试卷创建提供统一的题目来源。</p>
            </div>
            <div>
                <asp:Panel ID="pnlNormalButtons" runat="server" CssClass="hero-actions">
                    <button type="button" class="page-btn page-btn-primary" onclick="showAddModal()">+ 新建题库</button>
                    <button type="button" class="page-btn page-btn-primary" onclick="showImportModal()">从课堂测验导入</button>
                    <a href="../examlist.aspx" class="page-btn page-btn-secondary">返回考试</a>
                </asp:Panel>
                <asp:Panel ID="pnlSelectButtons" runat="server" Visible="false" CssClass="hero-actions">
                    <a href="../paper/paperadd.aspx" class="page-btn page-btn-secondary">取消选择</a>
                </asp:Panel>
            </div>
        </section>

        <asp:Panel ID="pnlSelectTip" runat="server" Visible="false" CssClass="select-tip">
            请点击题库卡片，从中选择题目添加到试卷
        </asp:Panel>

        <div class="bank-grid">
            <asp:Repeater ID="rptBanks" runat="server" OnItemCommand="rptBanks_ItemCommand" OnItemDataBound="rptBanks_ItemDataBound">
                <ItemTemplate>
                    <div class="bank-card<%# IsSelectMode ? " select-mode" : "" %>" onclick="<%# GetCardClick(Eval("BankId")) %>">
                        <div class="name"><%# Eval("BankName") %></div>
                        <div class="info"><%# Eval("Description") %></div>
                        <div class="info">创建时间：<%# Eval("CreateTime", "{0:yyyy-MM-dd}") %></div>
                        <span class="count"><%# Eval("QuestionCount") %> 道题目</span>
                        <asp:Panel ID="pnlActions" runat="server" CssClass="actions" onclick="event.stopPropagation();">
                            <a href="questionlist.aspx?bankId=<%# Eval("BankId") %>">管理题目</a>
                            <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("BankId") %>'>编辑</asp:LinkButton>
                            <asp:LinkButton ID="lbtnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("BankId") %>' CssClass="danger" OnClientClick="return confirm('确定删除此题库吗？');">删除</asp:LinkButton>
                        </asp:Panel>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="empty-panel">
            暂无题库，请点击"新建题库"添加
        </asp:Panel>
        </div>
    </div>

    <!-- 新建/编辑题库弹窗 -->
    <div id="bankModal" class="modal">
        <div class="modal-content">
            <div class="modal-header">
                <h3><asp:Literal ID="ltlModalTitle" runat="server">新建题库</asp:Literal></h3>
            </div>
            <asp:HiddenField ID="hfBankId" runat="server" />
            <div class="form-group">
                <label>题库名称 <span style="color:red">*</span></label>
                <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control" placeholder="请输入题库名称"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>题库编码 <span style="color:#999;font-weight:normal">(留空则自动生成)</span></label>
                <asp:TextBox ID="txtBankCode" runat="server" CssClass="form-control" placeholder="例如：BK_MATH_001，留空自动生成"></asp:TextBox>
            </div>
            <div class="form-group">
                <label>描述</label>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="题库描述"></asp:TextBox>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" onclick="hideModal()">取消</button>
                <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="btn btn-primary" OnClick="btnSave_Click" />
            </div>
        </div>
    </div>

    <!-- 从课堂测验导入弹窗 -->
    <div id="importModal" class="modal">
        <div class="modal-content" style="width: 600px; max-height: 80vh; overflow-y: auto;">
            <div class="modal-header">
                <h3>从课堂测验导入题目</h3>
            </div>
            <div class="form-group">
                <label>选择课堂测验：</label>
                <select id="examSelect" class="form-control" onchange="loadExamQuestions(this.value)">
                    <option value="">-- 请选择课堂测验 --</option>
                </select>
            </div>
            <div class="form-group">
                <label>选择题库：</label>
                <select id="targetBankSelect" class="form-control">
                    <option value="">-- 请选择题库 --</option>
                </select>
            </div>
            <div class="form-group">
                <label>年级：</label>
                <select id="gradeSelect" class="form-control">
                    <option value="">-- 请选择年级 --</option>
                </select>
            </div>
            <div class="form-group">
                <label>难度：</label>
                <select id="difficultySelect" class="form-control">
                    <option value="1">简单</option>
                    <option value="2">中等</option>
                    <option value="3">困难</option>
                </select>
            </div>
            <div class="form-group">
                <label>标签（用逗号分隔）：</label>
                <input type="text" id="tagsInput" class="form-control" placeholder="例如：选择题,基础题">
            </div>
            <div class="form-group">
                <label>题目列表：</label>
                <div id="questionList" style="max-height: 200px; overflow-y: auto; border: 1px solid #d9d9d9; padding: 10px;">
                    <div style="color: #999; text-align: center;">请先选择课堂测验</div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" onclick="hideImportModal()">取消</button>
                <button type="button" class="btn btn-primary" onclick="importQuestions()">导入题目</button>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        let currentExamQuestions = []; // 存储当前加载的题目列表
        
        function showAddModal() {
            document.getElementById('<%= hfBankId.ClientID %>').value = '';
            document.getElementById('<%= txtBankName.ClientID %>').value = '';
            document.getElementById('<%= txtBankCode.ClientID %>').value = '';
            document.getElementById('<%= txtDescription.ClientID %>').value = '';
            document.getElementById('bankModal').classList.add('show');
        }
        function hideModal() {
            document.getElementById('bankModal').classList.remove('show');
        }
        function showEditModal(id, name, code, desc) {
            document.getElementById('<%= hfBankId.ClientID %>').value = id;
            document.getElementById('<%= txtBankName.ClientID %>').value = name;
            document.getElementById('<%= txtBankCode.ClientID %>').value = code;
            document.getElementById('<%= txtDescription.ClientID %>').value = desc;
            document.getElementById('bankModal').classList.add('show');
        }
        
        // 导入功能
        function showImportModal() {
            document.getElementById('importModal').classList.add('show');
            loadExamList();
            loadBankList();
            loadGradeList();
        }
        
        function hideImportModal() {
            document.getElementById('importModal').classList.remove('show');
        }
        
        function loadExamList() {
            fetch('/exam/GetExamList.ashx')
                .then(response => response.json())
                .then(data => {
                    console.log('GetExamList返回数据:', data); // 调试信息
                    if (data.success && data.exams) {
                        const examSelect = document.getElementById('examSelect');
                        examSelect.innerHTML = '<option value="">-- 请选择课堂测验 --</option>';
                        data.exams.forEach(exam => {
                            const option = document.createElement('option');
                            option.value = exam.Vid; // 修改为Vid
                            option.textContent = exam.Vtitle; // 修改为Vtitle
                            examSelect.appendChild(option);
                        });
                    }
                })
                .catch(error => {
                    console.error('加载课堂测验列表失败:', error);
                    alert('加载课堂测验列表失败！');
                });
        }
        
        function loadBankList() {
            const targetBankSelect = document.getElementById('targetBankSelect');
            targetBankSelect.innerHTML = '<option value="">-- 请选择题库 --</option>';
            
            // 从Repeater中获取题库列表
            const bankCards = document.querySelectorAll('.bank-card');
            bankCards.forEach(card => {
                const bankName = card.querySelector('.name').textContent;
                const bankId = card.getAttribute('onclick').match(/bankId=(\d+)/)[1];
                const option = document.createElement('option');
                option.value = bankId;
                option.textContent = bankName;
                targetBankSelect.appendChild(option);
            });
        }
        
        function loadGradeList() {
            fetch('/exam/GetGradeList.ashx')
                .then(response => response.json())
                .then(data => {
                    if (data.success && data.grades) {
                        const gradeSelect = document.getElementById('gradeSelect');
                        gradeSelect.innerHTML = '<option value="">-- 请选择年级 --</option>';
                        data.grades.forEach(grade => {
                            const option = document.createElement('option');
                            option.value = grade.GradeId;
                            option.textContent = grade.GradeName;
                            gradeSelect.appendChild(option);
                        });
                    }
                })
                .catch(error => {
                    console.error('加载年级列表失败:', error);
                    alert('加载年级列表失败！');
                });
        }
        
        function loadExamQuestions(eid) {
            if (!eid) {
                document.getElementById('questionList').innerHTML = '<div style="color: #999; text-align: center;">请先选择课堂测验</div>';
                currentExamQuestions = [];
                return;
            }
            
            console.log('加载题目，eid:', eid);
            
            fetch('/exam/GetExamQuestions.ashx?eid=' + eid)
                .then(response => {
                    console.log('响应状态:', response.status);
                    return response.json();
                })
                .then(data => {
                    console.log('返回数据:', data);
                    
                    if (data.success && data.questions) {
                        currentExamQuestions = data.questions;
                        const questionListDiv = document.getElementById('questionList');
                        
                        if (data.questions.length === 0) {
                            questionListDiv.innerHTML = '<div style="color: #999; text-align: center;">该课堂测验暂无题目</div>';
                        } else {
                            let html = '';
                            data.questions.forEach((q, index) => {
                                html += `
                                    <div style="padding: 8px; border-bottom: 1px solid #f0f0f0;">
                                        <input type="checkbox" id="q_${index}" value="${index}" checked>
                                        <label for="q_${index}" style="margin-left: 5px;">
                                            ${index + 1}. ${q.typeText} - ${q.title.substring(0, 50)}${q.title.length > 50 ? '...' : ''}
                                        </label>
                                    </div>
                                `;
                            });
                            questionListDiv.innerHTML = html;
                        }
                    } else {
                        console.error('加载失败:', data);
                        document.getElementById('questionList').innerHTML = '<div style="color: red; text-align: center;">加载失败：' + (data.message || '未知错误') + '</div>';
                        currentExamQuestions = [];
                    }
                })
                .catch(error => {
                    console.error('加载题目列表失败:', error);
                    document.getElementById('questionList').innerHTML = '<div style="color: red; text-align: center;">加载题目列表失败：' + error.message + '</div>';
                    currentExamQuestions = [];
                });
        }
        
        function importQuestions() {
            const bankId = document.getElementById('targetBankSelect').value;
            const gradeId = document.getElementById('gradeSelect').value;
            const difficulty = document.getElementById('difficultySelect').value;
            const tags = document.getElementById('tagsInput').value;
            
            if (!bankId) {
                alert('请选择题库！');
                return;
            }
            
            if (!gradeId) {
                alert('请选择年级！');
                return;
            }
            
            if (currentExamQuestions.length === 0) {
                alert('请先选择课堂测验！');
                return;
            }
            
            // 获取选中的题目
            const selectedQuestions = [];
            const checkboxes = document.querySelectorAll('#questionList input[type="checkbox"]:checked');
            checkboxes.forEach(cb => {
                const index = parseInt(cb.value);
                const qData = currentExamQuestions[index].data;
                console.log('选中题目数据:', index, qData);
                selectedQuestions.push(qData);
            });
            
            console.log('准备导入的题目数量:', selectedQuestions.length);
            console.log('第一道题目:', selectedQuestions[0]);
            
            if (selectedQuestions.length === 0) {
                alert('请至少选择一道题目！');
                return;
            }
            
            // 构建请求数据
            const requestData = {
                bankId: parseInt(bankId),
                gradeId: parseInt(gradeId),
                difficulty: parseInt(difficulty),
                tags: tags,
                questions: selectedQuestions
            };
            
            // 发送到服务器
            fetch('/exam/ImportExamQuestionsToBank.ashx', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(requestData)
            })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    alert(`成功导入 ${data.savedCount} 道题目到题库！`);
                    hideImportModal();
                    // 刷新页面以更新题库题目数量
                    location.reload();
                } else {
                    alert('导入失败：' + (data.message || '未知错误'));
                }
            })
            .catch(error => {
                console.error('导入题目失败:', error);
                alert('导入题目失败：' + error.message);
            });
        }
    </script>
</asp:Content>
