<%@ Page Language="C#" AutoEventWireup="true" CodeFile="examstart.aspx.cs" Inherits="student_examstart" MasterPageFile="~/student/Stud.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cphs" runat="server">
    <style>
        .exam-page { padding: 20px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); min-height: calc(100vh - 120px); }
        .exam-header { background: #fff; padding: 20px 25px; border-radius: 12px; margin-bottom: 20px; display: flex; justify-content: space-between; align-items: center; box-shadow: 0 4px 12px rgba(0,0,0,0.08); }
        .exam-header h2 { margin: 0; font-size: 20px; font-weight: 600; color: #1a1a1a; }
        .exam-info { font-size: 14px; color: #666; margin-top: 8px; }
        .exam-timer { position: fixed; top: 15px; right: 20px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); padding: 12px 25px; border-radius: 25px; font-weight: bold; z-index: 100; box-shadow: 0 4px 15px rgba(102, 126, 234, 0.4); color: #fff; font-size: 16px; }
        .exam-timer.warning { background: linear-gradient(135deg, #ff6b6b 0%, #ee5a24 100%); }
        .question-nav { position: fixed; right: 20px; top: 80px; background: #fff; border-radius: 12px; padding: 20px; width: 180px; box-shadow: 0 4px 20px rgba(0,0,0,0.12); }
        .question-nav h4 { margin: 0 0 15px 0; font-size: 15px; font-weight: 600; color: #1a1a1a; }
        .question-nav .nav-grid { display: grid; grid-template-columns: repeat(5, 1fr); gap: 8px; }
        .question-nav .nav-item { width: 28px; height: 28px; display: flex; align-items: center; justify-content: center; border-radius: 6px; font-size: 13px; cursor: pointer; background: #f5f5f5; color: #666; transition: all 0.3s ease; font-weight: 500; }
        .question-nav .nav-item:hover { background: #667eea; color: #fff; transform: translateY(-2px); }
        .question-nav .nav-item.answered { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: #fff; }
        .question-nav .nav-item.current { border: 2px solid #667eea; background: #f0f3ff; color: #667eea; }
        .question-container { max-width: 900px; margin: 0 auto; }
        .question-card { background: #fff; border-radius: 12px; padding: 25px; margin-bottom: 20px; box-shadow: 0 2px 12px rgba(0,0,0,0.06); transition: all 0.3s ease; border-left: 4px solid transparent; }
        .question-card:hover { box-shadow: 0 4px 20px rgba(0,0,0,0.1); transform: translateY(-2px); }
        .question-card.current { border-left-color: #667eea; }
        .question-title { font-size: 17px; margin-bottom: 20px; line-height: 1.8; color: #1a1a1a; font-weight: 500; }
        .question-title .q-num { display: inline-block; width: 32px; height: 32px; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: #fff; border-radius: 50%; text-align: center; line-height: 32px; margin-right: 12px; font-weight: 600; font-size: 14px; }
        .question-title .q-type { color: #667eea; margin-left: 12px; font-weight: 500; background: #f0f3ff; padding: 2px 8px; border-radius: 4px; font-size: 13px; }
        .question-title .q-score { color: #666; margin-left: 12px; font-size: 14px; background: #f5f5f5; padding: 2px 8px; border-radius: 4px; }
        .options-list label { display: flex; align-items: center; padding: 14px 18px; margin-bottom: 12px; border: 2px solid #e8e8e8; border-radius: 10px; cursor: pointer; transition: all 0.3s ease; background: #fafafa; }
        .options-list label:hover { border-color: #667eea; background: #f0f3ff; transform: translateX(5px); }
        .options-list label.selected { border-color: #667eea; background: linear-gradient(135deg, #f0f3ff 0%, #e8f0fe 100%); }
        .options-list input { margin-right: 12px; transform: scale(1.2); }
        .fillblank-input { width: 100%; padding: 12px 15px; border: 2px solid #e8e8e8; border-radius: 8px; margin: 8px 0; transition: all 0.3s ease; background: #fafafa; }
        .fillblank-input:focus { border-color: #667eea; outline: none; background: #fff; box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1); }
        .textarea-answer { width: 100%; min-height: 180px; padding: 15px; border: 2px solid #e8e8e8; border-radius: 8px; resize: vertical; transition: all 0.3s ease; background: #fafafa; font-size: 14px; line-height: 1.6; }
        .textarea-answer:focus { border-color: #667eea; outline: none; background: #fff; box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1); }
        .score-container { display: flex; flex-direction: column; align-items: center; padding: 25px 0; }
        .score-stars { display: flex; gap: 12px; margin: 20px 0; }
        .score-star { font-size: 36px; cursor: pointer; color: #d9d9d9; transition: all 0.2s; }
        .score-star.active, .score-star:hover { color: #faad14; transform: scale(1.1); }
        .score-labels { display: flex; justify-content: space-between; width: 100%; max-width: 350px; font-size: 13px; color: #666; font-weight: 500; }
        .nps-container { text-align: center; padding: 25px 0; }
        .nps-scale { display: flex; justify-content: center; gap: 6px; margin: 20px 0; }
        .nps-item { width: 40px; height: 40px; display: flex; align-items: center; justify-content: center; border: 2px solid #e8e8e8; border-radius: 8px; cursor: pointer; font-size: 15px; font-weight: 600; transition: all 0.3s ease; background: #fafafa; }
        .nps-item:hover { border-color: #667eea; background: #f0f3ff; transform: translateY(-3px); }
        .nps-item.active { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: #fff; border-color: #667eea; transform: scale(1.1); }
        .nps-item.nps-low { background: linear-gradient(135deg, #ff6b6b 0%, #ee5a24 100%); color: #fff; border-color: #ff6b6b; }
        .nps-item.nps-mid { background: linear-gradient(135deg, #feca57 0%, #ff9f43 100%); color: #fff; border-color: #feca57; }
        .nps-item.nps-high { background: linear-gradient(135deg, #1dd1a1 0%, #10ac84 100%); color: #fff; border-color: #1dd1a1; }
        .nps-labels { display: flex; justify-content: space-between; width: 100%; max-width: 450px; margin: 12px auto; font-size: 13px; color: #666; font-weight: 500; }
        .matrix-container { overflow-x: auto; }
        .matrix-table { width: 100%; border-collapse: collapse; margin: 20px 0; border-radius: 8px; overflow: hidden; }
        .matrix-table th, .matrix-table td { padding: 14px; text-align: center; border: 1px solid #e8e8e8; }
        .matrix-table th { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: #fff; font-weight: 600; }
        .matrix-table td:first-child { text-align: left; background: #fafafa; font-weight: 600; color: #1a1a1a; }
        .matrix-table tr:hover td { background: #f0f3ff; }
        .matrix-table input[type="radio"], .matrix-table input[type="checkbox"] { transform: scale(1.3); cursor: pointer; }
        .multiple-blank-item { margin: 12px 0; display: flex; align-items: center; gap: 12px; padding: 10px; background: #fafafa; border-radius: 8px; }
        .multiple-blank-item label { min-width: 90px; font-weight: 600; color: #1a1a1a; }
        .multiple-blank-item input { flex: 1; padding: 10px 15px; border: 2px solid #e8e8e8; border-radius: 6px; transition: all 0.3s ease; background: #fff; }
        .multiple-blank-item input:focus { border-color: #667eea; outline: none; box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1); }
        .exam-footer { position: fixed; bottom: 0; left: 0; right: 0; background: #fff; padding: 18px; text-align: center; box-shadow: 0 -4px 20px rgba(0,0,0,0.1); z-index: 100; }
        .btn { display: inline-block; padding: 12px 35px; border: none; border-radius: 25px; cursor: pointer; font-size: 15px; margin: 0 8px; font-weight: 600; transition: all 0.3s ease; }
        .btn:hover { transform: translateY(-2px); box-shadow: 0 4px 12px rgba(0,0,0,0.15); }
        .btn-primary { background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: #fff; }
        .btn-success { background: linear-gradient(135deg, #1dd1a1 0%, #10ac84 100%); color: #fff; }
        .btn-default { background: #f5f5f5; color: #666; }
        .btn-default:hover { background: #e8e8e8; }
        .progress-bar { height: 6px; background: #e8e8e8; border-radius: 3px; margin: 12px 0; overflow: hidden; }
        .progress-bar .progress { height: 100%; background: linear-gradient(90deg, #667eea 0%, #764ba2 100%); border-radius: 3px; transition: width 0.3s ease; }
        .hidden { display: none; }
        
        @media (max-width: 768px) {
            .question-nav { display: none; }
            .exam-header { flex-direction: column; text-align: center; }
            .exam-timer { position: relative; top: 0; right: 0; margin: 10px auto; display: inline-block; }
            .question-container { padding: 0 10px; }
            .question-card { padding: 20px; }
            .options-list label { padding: 12px 14px; }
        }
    </style>

    <asp:HiddenField ID="hfExamId" runat="server" />
    <asp:HiddenField ID="hfAnswerId" runat="server" />
    <asp:HiddenField ID="hfRemainingTime" runat="server" />
    <asp:HiddenField ID="hfMaxSwitch" runat="server" Value="3" />

    <div class="exam-page">
        <div class="exam-header">
            <div>
                <h2><asp:Literal ID="ltlExamName" runat="server"></asp:Literal></h2>
                <div class="exam-info">
                    总分：<asp:Literal ID="ltlTotalScore" runat="server"></asp:Literal>分 | 
                    题数：<asp:Literal ID="ltlQuestionCount" runat="server"></asp:Literal>题
                </div>
            </div>
        </div>

        <div class="exam-timer" id="examTimer">
            <span id="timerDisplay">00:00:00</span>
        </div>

        <div class="question-nav" id="questionNav">
            <h4>答题卡</h4>
            <div class="nav-grid" id="navGrid"></div>
            <div class="progress-bar"><div class="progress" id="progressBar" style="width:0%"></div></div>
            <div style="font-size:12px;color:#666;margin-top:5px;">已答 <span id="answeredCount">0</span>/<span id="totalCount">0</span></div>
        </div>

        <div class="question-container" id="questionsContainer">
            <asp:Repeater ID="rptQuestions" runat="server" OnItemDataBound="rptQuestions_ItemDataBound">
                <ItemTemplate>
                    <div class="question-card" id="q<%# Container.ItemIndex + 1 %>" data-index="<%# Container.ItemIndex + 1 %>">
                        <asp:HiddenField ID="hfQuestionId" runat="server" Value='<%# Eval("QuestionId") %>' />
                        <div class="question-title">
                            <span class="q-num"><%# Container.ItemIndex + 1 %></span>
                            <%# Eval("QuestionContent") %>
                            <span class="q-type">[<%# GetTypeName(Eval("QuestionType")) %>]</span>
                            <span class="q-score">(<%# Eval("Score") %>分)</span>
                        </div>
                        
                        <!-- 单选题 -->
                        <asp:Panel ID="pnlRadio" runat="server" Visible="false" CssClass="options-list">
                            <asp:RadioButtonList ID="rblOptions" runat="server" RepeatLayout="Flow"></asp:RadioButtonList>
                        </asp:Panel>
                        
                        <!-- 多选题 -->
                        <asp:Panel ID="pnlCheckbox" runat="server" Visible="false" CssClass="options-list">
                            <asp:CheckBoxList ID="cblOptions" runat="server" RepeatLayout="Flow"></asp:CheckBoxList>
                        </asp:Panel>
                        
                        <!-- 判断题 -->
                        <asp:Panel ID="pnlJudge" runat="server" Visible="false" CssClass="options-list">
                            <label><input type="radio" name="judge_<%# Container.ItemIndex %>" value="T" /> 正确</label>
                            <label><input type="radio" name="judge_<%# Container.ItemIndex %>" value="F" /> 错误</label>
                        </asp:Panel>
                        
                        <!-- 填空题 -->
                        <asp:Panel ID="pnlFillBlank" runat="server" Visible="false">
                            <asp:PlaceHolder ID="phFillBlanks" runat="server"></asp:PlaceHolder>
                        </asp:Panel>
                        
                        <!-- 简答题 -->
                        <asp:Panel ID="pnlTextarea" runat="server" Visible="false">
                            <asp:TextBox ID="txtAnswer" runat="server" CssClass="textarea-answer" TextMode="MultiLine" Rows="6" placeholder="请输入答案..."></asp:TextBox>
                        </asp:Panel>
                        
                        <!-- 下拉选择题 -->
                        <asp:Panel ID="pnlSelect" runat="server" Visible="false">
                            <asp:DropDownList ID="ddlSelect" runat="server" CssClass="form-control" style="width:100%;padding:10px;border:1px solid #d9d9d9;border-radius:4px;"></asp:DropDownList>
                        </asp:Panel>
                        
                        <!-- 打分题 -->
                        <asp:Panel ID="pnlScore" runat="server" Visible="false">
                            <div class="score-container">
                                <asp:HiddenField ID="hfScoreValue" runat="server" Value="0" />
                                <div class="score-stars" id="scoreStars"></div>
                                <div class="score-labels">
                                    <span id="scoreLowLabel">不满意</span>
                                    <span id="scoreHighLabel">非常满意</span>
                                </div>
                            </div>
                        </asp:Panel>
                        
                        <!-- NPS评分 -->
                        <asp:Panel ID="pnlNps" runat="server" Visible="false">
                            <div class="nps-container">
                                <asp:HiddenField ID="hfNpsValue" runat="server" Value="" />
                                <div class="nps-scale" id="npsScale"></div>
                                <div class="nps-labels">
                                    <span id="npsLowLabel">不满意</span>
                                    <span id="npsMidLabel">一般</span>
                                    <span id="npsHighLabel">非常满意</span>
                                </div>
                            </div>
                        </asp:Panel>
                        
                        <!-- 矩阵题 -->
                        <asp:Panel ID="pnlMatrix" runat="server" Visible="false">
                            <div class="matrix-container">
                                <asp:Literal ID="ltlMatrixTable" runat="server"></asp:Literal>
                            </div>
                        </asp:Panel>
                        
                        <!-- 多项填空 -->
                        <asp:Panel ID="pnlMultipleBlank" runat="server" Visible="false">
                            <asp:PlaceHolder ID="phMultipleBlanks" runat="server"></asp:PlaceHolder>
                        </asp:Panel>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div class="exam-footer">
            <asp:Button ID="btnTempSave" runat="server" Text="暂存答案" CssClass="btn btn-default" OnClick="btnTempSave_Click" />
            <asp:Button ID="btnSubmit" runat="server" Text="提交答卷" CssClass="btn btn-success" OnClick="btnSubmit_Click" />
        </div>
    </div>

    <script type="text/javascript">
        var remainingSeconds = parseInt(document.getElementById('<%= hfRemainingTime.ClientID %>').value) || 7200;
        var timerInterval;
        var switchCount = 0;
        var maxSwitch = parseInt(document.getElementById('<%= hfMaxSwitch.ClientID %>').value) || 3;
        var totalQuestions = <%= TotalQuestions %>;

        // 初始化答题卡
        function initNav() {
            var navGrid = document.getElementById('navGrid');
            for (var i = 1; i <= totalQuestions; i++) {
                var item = document.createElement('div');
                item.className = 'nav-item';
                item.textContent = i;
                item.onclick = (function(n) { return function() { scrollToQuestion(n); }; })(i);
                navGrid.appendChild(item);
            }
            document.getElementById('totalCount').textContent = totalQuestions;
        }

        function scrollToQuestion(n) {
            var q = document.getElementById('q' + n);
            if (q) {
                q.scrollIntoView({ behavior: 'smooth', block: 'center' });
                document.querySelectorAll('.nav-item').forEach(function(el, idx) {
                    el.classList.toggle('current', idx + 1 === n);
                });
            }
        }

        function updateProgress() {
            var answered = 0;
            document.querySelectorAll('.question-card').forEach(function(card, idx) {
                var hasAnswer = false;
                var radios = card.querySelectorAll('input[type="radio"]:checked');
                var checkboxes = card.querySelectorAll('input[type="checkbox"]:checked');
                var texts = card.querySelectorAll('input[type="text"], textarea');
                
                if (radios.length > 0) hasAnswer = true;
                if (checkboxes.length > 0) hasAnswer = true;
                texts.forEach(function(t) { if (t.value.trim()) hasAnswer = true; });
                
                var navItems = document.querySelectorAll('.nav-item');
                if (navItems[idx]) {
                    navItems[idx].classList.toggle('answered', hasAnswer);
                }
                if (hasAnswer) answered++;
            });
            document.getElementById('answeredCount').textContent = answered;
            document.getElementById('progressBar').style.width = (answered / totalQuestions * 100) + '%';
        }

        function startTimer() {
            timerInterval = setInterval(function() {
                remainingSeconds--;
                updateTimerDisplay();
                if (remainingSeconds <= 300) {
                    document.getElementById('examTimer').classList.add('warning');
                }
                if (remainingSeconds <= 0) {
                    clearInterval(timerInterval);
                    alert('考试时间已到，系统将自动提交答卷！');
                    document.getElementById('<%= btnSubmit.ClientID %>').click();
                }
            }, 1000);
        }

        function updateTimerDisplay() {
            var hours = Math.floor(remainingSeconds / 3600);
            var minutes = Math.floor((remainingSeconds % 3600) / 60);
            var seconds = remainingSeconds % 60;
            document.getElementById('timerDisplay').textContent = 
                String(hours).padStart(2, '0') + ':' + 
                String(minutes).padStart(2, '0') + ':' + 
                String(seconds).padStart(2, '0');
        }

        // 自动保存
        setInterval(function() {
            <%= Page.ClientScript.GetPostBackEventReference(btnTempSave, "") %>
        }, 60000);

        // 防作弊
        document.addEventListener('copy', function(e) { e.preventDefault(); });
        document.addEventListener('paste', function(e) { e.preventDefault(); });
        document.addEventListener('contextmenu', function(e) { e.preventDefault(); });

        // 切屏检测
        document.addEventListener('visibilitychange', function() {
            if (document.hidden) {
                switchCount++;
                if (switchCount >= maxSwitch) {
                    alert('切屏次数过多（' + switchCount + '次），系统将自动提交！');
                    document.getElementById('<%= btnSubmit.ClientID %>').click();
                } else {
                    alert('警告：您已切屏' + switchCount + '次，超过' + maxSwitch + '次将自动提交！');
                }
            }
        });

        // 监听答案变化
        document.addEventListener('change', updateProgress);
        document.addEventListener('input', updateProgress);

        // 初始化
        window.onload = function() {
            initNav();
            startTimer();
            updateProgress();
        };

        function confirmSubmit() {
            var unanswered = totalQuestions - parseInt(document.getElementById('answeredCount').textContent);
            var msg = '确定提交答卷吗？';
            if (unanswered > 0) {
                msg = '您还有 ' + unanswered + ' 题未作答，确定提交吗？';
            }
            return confirm(msg);
        }
    </script>
</asp:Content>
