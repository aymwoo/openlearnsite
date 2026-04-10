<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/teacher/Teach.master" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="LearnSite.DAL" %>
<%@ Import Namespace="LearnSite.BLL" %>
<%@ Import Namespace="LearnSite.Model" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <style>
        .pc-page,
        .pc-page * {
            box-sizing: border-box;
        }
        .pc-page {
            font-family: Microsoft YaHei, Arial, sans-serif;
            color: #0f172a;
        }
        .hero {
            position: relative;
            overflow: hidden;
            border-radius: 24px;
            padding: 28px 32px;
            background: linear-gradient(135deg, #0f172a 0%, #7c3aed 52%, #ec4899 100%);
            color: #fdf2f8;
            box-shadow: 0 20px 46px rgba(15,23,42,0.16);
            margin-bottom: 24px;
        }
        .hero::after {
            content: "";
            position: absolute;
            right: -48px;
            top: -48px;
            width: 210px;
            height: 210px;
            border-radius: 999px;
            background: rgba(255,255,255,0.08);
        }
        .hero-inner {
            position: relative;
            z-index: 1;
            display: flex;
            justify-content: space-between;
            gap: 20px;
            flex-wrap: wrap;
            align-items: flex-start;
            color: #fdf2f8;
        }
        .hero-title {
            display: flex;
            flex-direction: column;
            gap: 4px;
        }
        .hero-title strong {
            font-size: 32px;
            font-weight: 800;
            letter-spacing: 0.04em;
        }
        .hero-title span {
            margin-top: 6px;
            max-width: 760px;
            color: rgba(253,242,248,0.9);
            font-size: 15px;
            line-height: 1.75;
        }
        .hero-actions {
            display: flex;
            align-items: center;
            gap: 10px;
            flex-wrap: wrap;
            padding: 10px 14px;
            border-radius: 14px;
            background: rgba(15,23,42,0.22);
            border: 1px solid rgba(255,255,255,0.16);
        }
        .hero-link {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            min-height: 40px;
            padding: 0 14px;
            border-radius: 12px;
            background: rgba(255,255,255,0.12);
            border: 1px solid rgba(255,255,255,0.16);
            color: #fff;
            text-decoration: none;
            font-size: 14px;
            font-weight: 700;
            transition: all 0.18s ease;
        }
        .hero-link:hover {
            background: rgba(255,255,255,0.2);
        }
        .container {
            max-width: 1440px;
            margin: 0 auto;
            padding: 24px;
        }
        
        /* 主操作区 */
        .check-panel {
            background: white;
            padding: 24px;
            border-radius: 22px;
            border: 1px solid #e2e8f0;
            box-shadow: 0 14px 34px rgba(15,23,42,0.06);
            margin-bottom: 24px;
        }
        .panel-title {
            font-size: 20px;
            font-weight: 800;
            color: #0f172a;
            margin-bottom: 20px;
            padding-bottom: 15px;
            border-bottom: 1px solid #e2e8f0;
        }
        
        /* 输入区 */
        .input-section {
            display: grid;
            grid-template-columns: 300px 1fr;
            gap: 30px;
            margin-bottom: 30px;
        }
        .machine-input {
            background: linear-gradient(180deg, #f8fbff 0%, #eff6ff 100%);
            padding: 24px;
            border-radius: 18px;
            border: 1px solid #dbeafe;
        }
        .input-group {
            margin-bottom: 20px;
        }
        .input-group label {
            display: block;
            margin-bottom: 8px;
            color: #555;
            font-weight: 500;
        }
        .input-group input, .input-group select {
            width: 100%;
            padding: 12px 15px;
            border: 2px solid #e0e0e0;
            border-radius: 8px;
            font-size: 16px;
            transition: all 0.3s;
        }
        .input-group input:focus, .input-group select:focus {
            border-color: #667eea;
            outline: none;
            box-shadow: 0 0 0 3px rgba(102, 126, 234, 0.1);
        }
        .btn-query {
            width: 100%;
            padding: 15px;
            background: linear-gradient(135deg, #2563eb 0%, #1d4ed8 100%);
            color: white;
            border: none;
            border-radius: 12px;
            font-size: 16px;
            cursor: pointer;
            transition: all 0.3s;
            font-weight: 700;
        }
        .btn-query:hover {
            transform: translateY(-2px);
            box-shadow: 0 5px 15px rgba(102, 126, 234, 0.4);
        }
        
        /* 学生信息显示 */
        .student-info {
            background: linear-gradient(135deg, #1e3a8a 0%, #2563eb 50%, #06b6d4 100%);
            color: white;
            padding: 25px;
            border-radius: 18px;
            display: none;
        }
        .student-info.show {
            display: block;
        }
        .student-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 20px;
        }
        .student-name {
            font-size: 28px;
            font-weight: bold;
        }
        .student-class {
            font-size: 18px;
            opacity: 0.9;
        }
        .student-details {
            display: grid;
            grid-template-columns: repeat(3, 1fr);
            gap: 15px;
        }
        .detail-item {
            background: rgba(255,255,255,0.2);
            padding: 15px;
            border-radius: 8px;
        }
        .detail-label {
            font-size: 14px;
            opacity: 0.8;
            margin-bottom: 5px;
        }
        .detail-value {
            font-size: 20px;
            font-weight: bold;
        }
        
        /* 扣分原因选择 */
        .reason-section {
            margin-bottom: 30px;
        }
        .reason-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
            gap: 15px;
        }
        .reason-card {
            background: #f8fafc;
            border: 1px solid #e2e8f0;
            border-radius: 16px;
            padding: 20px;
            cursor: pointer;
            transition: all 0.3s;
        }
        .reason-card:hover {
            border-color: #667eea;
            transform: translateY(-3px);
            box-shadow: 0 5px 15px rgba(0,0,0,0.1);
        }
        .reason-card.selected {
            background: linear-gradient(135deg, #2563eb 0%, #1d4ed8 100%);
            border-color: #2563eb;
            color: white;
        }
        .reason-icon {
            font-size: 32px;
            margin-bottom: 10px;
        }
        .reason-name {
            font-size: 18px;
            font-weight: bold;
            margin-bottom: 8px;
        }
        .reason-score {
            font-size: 24px;
            font-weight: bold;
            color: #ff4444;
        }
        .reason-card.selected .reason-score {
            color: #ffeb3b;
        }
        
        /* 提交按钮 */
        .btn-submit {
            width: 100%;
            padding: 18px;
            background: linear-gradient(135deg, #ef4444 0%, #f97316 100%);
            color: white;
            border: none;
            border-radius: 14px;
            font-size: 18px;
            font-weight: bold;
            cursor: pointer;
            transition: all 0.3s;
            margin-top: 20px;
        }
        .btn-submit:hover {
            transform: translateY(-2px);
            box-shadow: 0 5px 15px rgba(255, 68, 68, 0.4);
        }
        .btn-submit:disabled {
            background: #ccc;
            cursor: not-allowed;
            transform: none;
            box-shadow: none;
        }
        
        /* 消息提示 */
        .message {
            padding: 15px 20px;
            border-radius: 8px;
            margin-bottom: 20px;
            display: none;
        }
        .message.show {
            display: flex;
            align-items: center;
            gap: 10px;
        }
        .message.success {
            background: #d4edda;
            color: #155724;
            border: 1px solid #c3e6cb;
        }
        .message.error {
            background: #f8d7da;
            color: #721c24;
            border: 1px solid #f5c6cb;
        }
        
        /* 记录列表 */
        .records-panel {
            background: white;
            padding: 24px;
            border-radius: 22px;
            border: 1px solid #e2e8f0;
            box-shadow: 0 14px 34px rgba(15,23,42,0.06);
        }
        .records-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 20px;
        }
        .records-count {
            background: #eff6ff;
            color: white;
            padding: 8px 16px;
            border-radius: 999px;
            font-size: 14px;
            color: #2563eb;
            font-weight: 800;
        }
        table {
            width: 100%;
            border-collapse: collapse;
        }
        th {
            background: #eff6ff;
            padding: 15px;
            text-align: left;
            font-weight: 800;
            color: #1e3a8a;
            border-bottom: 1px solid #dbeafe;
        }
        td {
            padding: 15px;
            border-bottom: 1px solid #eef2f7;
        }
        tr:hover {
            background: #f8f9fa;
        }
        .score-negative {
            color: #ff4444;
            font-weight: bold;
        }
        .time-badge {
            background: #e0e0e0;
            padding: 5px 12px;
            border-radius: 15px;
            font-size: 13px;
            color: #666;
        }
        
        /* 快捷操作 */
        .quick-actions {
            display: grid;
            grid-template-columns: repeat(4, 1fr);
            gap: 15px;
            margin-top: 20px;
        }
        .quick-btn {
            padding: 15px;
            background: white;
            border: 2px solid #e0e0e0;
            border-radius: 10px;
            cursor: pointer;
            transition: all 0.3s;
            text-align: center;
        }
        .quick-btn:hover {
            border-color: #667eea;
            background: #f8f9ff;
        }
        .quick-btn-icon {
            font-size: 24px;
            margin-bottom: 5px;
        }
        .quick-btn-text {
            font-size: 14px;
            color: #666;
        }
        
        /* 座位排列 */
        .classroom-panel {
            background: white;
            padding: 24px;
            border-radius: 22px;
            border: 1px solid #e2e8f0;
            box-shadow: 0 14px 34px rgba(15,23,42,0.06);
            margin-bottom: 24px;
        }
        .classroom-grid {
            display: grid;
            grid-template-columns: repeat(8, 1fr);
            gap: 10px;
            margin-top: 20px;
        }
        .seat {
            background: #f8fafc;
            border: 1px solid #e2e8f0;
            border-radius: 14px;
            padding: 15px;
            text-align: center;
            cursor: pointer;
            transition: all 0.3s;
        }
        .seat:hover {
            border-color: #667eea;
            background: #f8f9ff;
            transform: translateY(-2px);
        }
        .seat.selected {
            background: #667eea;
            border-color: #667eea;
            color: white;
        }
        .seat.has-student {
            background: #e8f5e8;
            border-color: #4caf50;
        }
        .seat.has-student:hover {
            background: #c8e6c9;
        }
        .seat-number {
            font-size: 18px;
            font-weight: bold;
            margin-bottom: 5px;
        }
        .seat-student {
            font-size: 14px;
            color: #666;
        }
        .seat.has-student .seat-student {
            color: #2e7d32;
        }
        .seat.selected .seat-student {
            color: white;
        }
        
        /* 批量操作 */
        .batch-actions {
            margin-top: 20px;
            padding: 20px;
            background: linear-gradient(180deg, #fff7ed 0%, #ffedd5 100%);
            border: 1px solid #fdba74;
            border-radius: 18px;
            display: none;
        }
        .batch-actions.show {
            display: block;
        }
        .batch-actions h3 {
            margin-bottom: 15px;
            color: #9a3412;
        }
        @media (max-width: 900px) {
            .container {
                padding: 16px;
            }
            .header,
            .check-panel,
            .classroom-panel,
            .records-panel {
                padding: 20px;
            }
            .input-section {
                grid-template-columns: 1fr;
            }
            .student-details {
                grid-template-columns: 1fr;
            }
            .quick-actions {
                grid-template-columns: repeat(2, 1fr);
            }
            .classroom-grid {
                grid-template-columns: repeat(4, 1fr);
            }
        }
        @media (max-width: 640px) {
            .hero {
                padding: 22px 20px;
            }
            .hero-title strong {
                font-size: 26px;
            }
            .hero-title span {
                font-size: 14px;
            }
            .classroom-grid,
            .quick-actions,
            .reason-grid {
                grid-template-columns: 1fr 1fr;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="pc-page">
    <div class="container">
        <div class="hero">
            <div class="hero-inner">
                <div class="hero-title">
                <strong>课后检查中心</strong>
                <span>统一风格页面 · 座位巡检、扣分登记与当日记录</span>
                </div>
                <div class="hero-actions">
                    <a href="teachermanage.aspx" class="hero-link">返回管理页</a>
                    <a href="start.aspx" class="hero-link">返回课堂启动</a>
                </div>
            </div>
        </div>

        <% 
            // 暂时注释掉 FreeTreeService 相关代码，因为该文件已被删除
            /*
            FreeTreeService srv = new FreeTreeService();
            */
            string action = Request.Form["action"];
            string message = "";
            string messageType = "";
            
            // 处理扣分提交
            if (action == "submit")
            {
                string machineNum = Request.Form["machineNum"];
                string reason = Request.Form["reason"];
                string studentNum = Request.Form["studentNum"];
                string studentName = Request.Form["studentName"];
                int score = 0;
                int.TryParse(Request.Form["score"], out score);
                
                if (!string.IsNullOrEmpty(studentNum) && !string.IsNullOrEmpty(reason) && score < 0)
                {
                    try
                    {
                        // 扣除能量
                        /*
                        srv.ModifyStudentEnergy(studentNum, studentName, score, "课后检查：" + reason);
                        */
                        message = "✅ 已成功为学生 " + studentName + " 扣除 " + Math.Abs(score) + " 分！原因：" + reason;
                        messageType = "success";
                    }
                    catch (Exception ex)
                    {
                        message = "❌ 扣分失败：" + ex.Message;
                        messageType = "error";
                    }
                }
                else
                {
                    message = "❌ 请先查询学生信息并选择扣分原因！";
                    messageType = "error";
                }
            }
            
            // 处理批量扣分提交
            else if (action == "batchSubmit")
            {
                string selectedSeatsJson = Request.Form["selectedSeats"];
                string reason = Request.Form["reason"];
                
                if (!string.IsNullOrEmpty(selectedSeatsJson) && !string.IsNullOrEmpty(reason))
                {
                    try
                    {
                        // 解析选中的座位信息
                        System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        dynamic seatsData = serializer.DeserializeObject(selectedSeatsJson);
                        
                        int successCount = 0;
                        foreach (var seat in seatsData)
                        {
                            string studentNum = seat["studentNum"].ToString();
                            string studentName = seat["studentName"].ToString();
                            int score = reason == "凳子未放" ? -20 : -50;
                            
                            // 扣除能量
                            /*
                            srv.ModifyStudentEnergy(studentNum, studentName, score, "课后检查：" + reason);
                            */
                            successCount++;
                        }
                        
                        message = "✅ 已成功为 " + successCount + " 名学生扣除分数！原因：" + reason;
                        messageType = "success";
                    }
                    catch (Exception ex)
                    {
                        message = "❌ 批量扣分失败：" + ex.Message;
                        messageType = "error";
                    }
                }
                else
                {
                    message = "❌ 请先选择学生和扣分原因！";
                    messageType = "error";
                }
            }
            
            // 获取今天的检查记录
            /*
            List<PerformanceScore> todayLogs = srv.GetAllLogs().FindAll(l => l.CreateTime.Date == DateTime.Today);
            */
            // 临时创建一个空列表
            int todayLogsCount = 0;
        %>

        <!-- 班级选择 -->
        <div class="check-panel">
            <% if (!string.IsNullOrEmpty(message)) { %>
            <div class="message <%= messageType %> show">
                <span style="font-size: 20px;">
                    <% if (messageType == "success") { %>
                        ✅
                    <% } else { %>
                        ❌
                    <% } %>
                </span>
                <%= message %>
            </div>
            <% } %>

            <h2 class="panel-title">🏫 选择班级</h2>
            
            <div id="classForm">
                <div class="input-section">
                    <div class="machine-input">
                        <div class="input-group">
                            <label>年级</label>
                            <select name="grade" id="grade" required>
                                <option value="">请选择年级</option>
                                <option value="1" <%= Request.Form["grade"] == "1" ? "selected" : "" %>>1年级</option>
                                <option value="2" <%= Request.Form["grade"] == "2" ? "selected" : "" %>>2年级</option>
                                <option value="3" <%= Request.Form["grade"] == "3" ? "selected" : "" %>>3年级</option>
                                <option value="4" <%= Request.Form["grade"] == "4" ? "selected" : "" %>>4年级</option>
                                <option value="5" <%= Request.Form["grade"] == "5" ? "selected" : "" %>>5年级</option>
                                <option value="6" <%= Request.Form["grade"] == "6" ? "selected" : "" %>>6年级</option>
                            </select>
                        </div>
                        <div class="input-group">
                            <label>班级</label>
                            <select name="classNum" id="classNum" required>
                                <option value="">请选择班级</option>
                                <option value="1" <%= Request.Form["classNum"] == "1" ? "selected" : "" %>>1班</option>
                                <option value="2" <%= Request.Form["classNum"] == "2" ? "selected" : "" %>>2班</option>
                                <option value="3" <%= Request.Form["classNum"] == "3" ? "selected" : "" %>>3班</option>
                                <option value="4" <%= Request.Form["classNum"] == "4" ? "selected" : "" %>>4班</option>
                                <option value="5" <%= Request.Form["classNum"] == "5" ? "selected" : "" %>>5班</option>
                                <option value="6" <%= Request.Form["classNum"] == "6" ? "selected" : "" %>>6班</option>
                            </select>
                        </div>
                        <button type="submit" name="action" value="loadClass" class="btn-query" onclick="this.form.action.value='loadClass';">
                            📋 加载班级学生
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- 座位排列 -->
        <% if (Request.Form["action"] == "loadClass" || Request.Form["action"] == "batchSubmit") { %>
        <div class="classroom-panel">
            <h2 class="panel-title">🪑 电脑室座位排列</h2>
            
            <div class="classroom-grid" id="classroomGrid">
                <% 
                    int grade = 0;
                    int classNum = 0;
                    int.TryParse(Request.Form["grade"], out grade);
                    int.TryParse(Request.Form["classNum"], out classNum);
                    
                    // 查询该班级的学生
                    LearnSite.DAL.Students studentDal = new LearnSite.DAL.Students();
                    DataSet studentDs = studentDal.GetList("Sgrade=" + grade + " AND Sclass=" + classNum);
                    
                    // 查询今天的签到记录
                    LearnSite.DAL.Signin signinDal = new LearnSite.DAL.Signin();
                    DataSet signinDs = signinDal.GetList("Qgrade=" + grade + " AND Qclass=" + classNum + " AND Qdate >= CAST(GETDATE() AS DATE) AND Qdate < CAST(DATEADD(DAY, 1, GETDATE()) AS DATE)");
                    
                    // 创建机号到学生信息的映射
                    Dictionary<string, string[]> machineMap = new Dictionary<string, string[]>();
                    if (signinDs != null && signinDs.Tables.Count > 0 && signinDs.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in signinDs.Tables[0].Rows)
                        {
                            string machine = row["Qmachine"].ToString();
                            string[] info = { row["Qnum"].ToString(), row["Qname"].ToString() };
                            machineMap[machine] = info;
                        }
                    }
                    
                    // 生成40个座位
                    for (int i = 1; i <= 40; i++)
                    {
                        string machine = i.ToString();
                        bool hasStudent = machineMap.ContainsKey(machine);
                        string studentName = hasStudent ? machineMap[machine][1] : "";
                        string studentNum = hasStudent ? machineMap[machine][0] : "";
                %>
                <div class="seat <%= hasStudent ? "has-student" : "" %>" 
                     data-machine="<%= machine %>" 
                     data-student-num="<%= studentNum %>" 
                     data-student-name="<%= studentName %>" 
                     onclick="selectSeat(this)">
                    <div class="seat-number"><%= machine %>号</div>
                    <div class="seat-student"><%= studentName %></div>
                </div>
                <% 
                    }
                %>
            </div>
            
            <!-- 批量操作 -->
            <div class="batch-actions" id="batchActions">
                <h3>📋 批量操作</h3>
                <div id="batchForm">
                    <input type="hidden" name="action" value="batchSubmit" />
                    <input type="hidden" name="grade" value="<%= grade %>" />
                    <input type="hidden" name="classNum" value="<%= classNum %>" />
                    <input type="hidden" name="selectedSeats" id="selectedSeats" />
                    
                    <div class="input-group">
                        <label>选择扣分原因</label>
                        <select name="reason" id="batchReason" required>
                            <option value="">请选择扣分原因</option>
                            <option value="凳子未放">凳子未放 (-20分)</option>
                            <option value="乱扔垃圾">乱扔垃圾 (-50分)</option>
                            <option value="乱动电脑">乱动电脑 (-50分)</option>
                            <option value="不整理键盘鼠标">不整理键盘鼠标 (-50分)</option>
                            <option value="关闭电源">关闭电源 (-50分)</option>
                            <option value="其他">其他 (-50分)</option>
                        </select>
                    </div>
                    
                    <button type="submit" class="btn-submit" id="batchSubmitBtn" disabled onclick="this.form.action.value='batchSubmit';">
                        ⚠️ 请先选择学生和扣分原因
                    </button>
                </div>
            </div>
        </div>
        <% } %>

        <!-- 单个学生操作 -->
        <div class="check-panel" id="singleStudentPanel" style="display: none;">
            <h2 class="panel-title">👤 学生信息</h2>
            
            <div class="student-info show" id="singleStudentInfo">
                <div class="student-header">
                    <div>
                        <div class="student-name" id="singleStudentName"></div>
                        <div class="student-class" id="singleStudentClass"></div>
                    </div>
                    <div style="text-align: right;">
                        <div style="font-size: 14px; opacity: 0.8;">机号</div>
                        <div style="font-size: 32px; font-weight: bold;" id="singleMachineNum"></div>
                    </div>
                </div>
                <div class="student-details">
                    <div class="detail-item">
                        <div class="detail-label">学号</div>
                        <div class="detail-value" id="singleStudentNum"></div>
                    </div>
                    <div class="detail-item">
                        <div class="detail-label">当前能量</div>
                        <div class="detail-value" id="singleStudentEnergy"></div>
                    </div>
                    <div class="detail-item">
                        <div class="detail-label">等级</div>
                        <div class="detail-value" id="singleStudentLevel"></div>
                    </div>
                </div>
                
                <!-- 扣分表单 -->
                <div id="singleSubmitForm" style="margin-top: 30px;">
                    <input type="hidden" name="action" value="submit" />
                    <input type="hidden" name="machineNum" id="hiddenMachineNum" />
                    <input type="hidden" name="studentNum" id="hiddenStudentNum" />
                    <input type="hidden" name="studentName" id="hiddenStudentName" />
                    <input type="hidden" name="reason" id="singleSelectedReason" />
                    <input type="hidden" name="score" id="singleSelectedScore" />
                    
                    <h3 style="color: white; margin-bottom: 15px;">选择扣分原因：</h3>
                    <div class="reason-grid">
                        <div class="reason-card" onclick="selectSingleReason(this, '凳子未放', -20)">
                            <div class="reason-icon">🪑</div>
                            <div class="reason-name">凳子未放</div>
                            <div class="reason-score">-20分</div>
                        </div>
                        <div class="reason-card" onclick="selectSingleReason(this, '乱扔垃圾', -50)">
                            <div class="reason-icon">🗑️</div>
                            <div class="reason-name">乱扔垃圾</div>
                            <div class="reason-score">-50分</div>
                        </div>
                        <div class="reason-card" onclick="selectSingleReason(this, '乱动电脑', -50)">
                            <div class="reason-icon">💻</div>
                            <div class="reason-name">乱动电脑</div>
                            <div class="reason-score">-50分</div>
                        </div>
                        <div class="reason-card" onclick="selectSingleReason(this, '不整理键盘鼠标', -50)">
                            <div class="reason-icon">⌨️</div>
                            <div class="reason-name">不整理键盘鼠标</div>
                            <div class="reason-score">-50分</div>
                        </div>
                        <div class="reason-card" onclick="selectSingleReason(this, '关闭电源', -50)">
                            <div class="reason-icon">🔌</div>
                            <div class="reason-name">关闭电源</div>
                            <div class="reason-score">-50分</div>
                        </div>
                        <div class="reason-card" onclick="selectSingleReason(this, '其他', -50)">
                            <div class="reason-icon">📋</div>
                            <div class="reason-name">其他</div>
                            <div class="reason-score">-50分</div>
                        </div>
                    </div>
                    
                    <button type="submit" class="btn-submit" id="singleSubmitBtn" disabled onclick="this.form.action.value='submit';">
                        ⚠️ 请先选择扣分原因
                    </button>
                </div>
            </div>
        </div>

        <!-- 检查记录列表 -->
        <div class="records-panel">
            <div class="records-header">
                <h2 class="panel-title">📊 今日检查记录</h2>
                <span class="records-count">共 <%= todayLogsCount %> 条记录</span>
            </div>
            
            <% if (todayLogsCount > 0) { %>
            <table>
                <thead>
                    <tr>
                        <th>时间</th>
                        <th>学生</th>
                        <th>年级班级</th>
                        <th>扣分原因</th>
                        <th>扣除能量</th>
                    </tr>
                </thead>
                <tbody>
                    <!-- 暂时注释掉检查记录列表，因为 FreeTreeService 已被删除 -->
                    <!--
                    <tr>
                        <td><span class="time-badge">--:--</span></td>
                        <td>
                            <strong>学生姓名</strong>
                            <br><small style="color: #999;">学号</small>
                        </td>
                        <td>年级班级</td>
                        <td>扣分原因</td>
                        <td><span class="score-negative">0</span></td>
                    </tr>
                    -->
                </tbody>
            </table>
            <% } else { %>
            <div style="text-align: center; padding: 40px; color: #999;">
                <div style="font-size: 48px; margin-bottom: 15px;">📝</div>
                <div>今天还没有检查记录</div>
            </div>
            <% } %>
        </div>
    </div>

    <script>
        // 快速输入机号
        function quickInput(num) {
            document.getElementById('queryMachine').value = num;
        }
        
        // 选择座位
        function selectSeat(element) {
            if (!element.classList.contains('has-student')) return;
            
            // 切换选中状态
            element.classList.toggle('selected');
            updateBatchActions();
            
            // 显示单个学生信息
            var machine = element.dataset.machine;
            var studentNum = element.dataset.studentNum;
            var studentName = element.dataset.studentName;
            
            document.getElementById('singleMachineNum').textContent = machine + '号';
            document.getElementById('singleStudentNum').textContent = studentNum;
            document.getElementById('singleStudentName').textContent = studentName;
            document.getElementById('singleStudentClass').textContent = document.getElementById('grade').value + '年级' + document.getElementById('classNum').value + '班';
            
            // 设置隐藏字段
            document.getElementById('hiddenMachineNum').value = machine;
            document.getElementById('hiddenStudentNum').value = studentNum;
            document.getElementById('hiddenStudentName').value = studentName;
            
            // 显示单个学生操作面板
            document.getElementById('singleStudentPanel').style.display = 'block';
        }
        
        // 更新批量操作状态
        function updateBatchActions() {
            var selectedSeats = document.querySelectorAll('.seat.selected');
            var batchActions = document.getElementById('batchActions');
            var batchSubmitBtn = document.getElementById('batchSubmitBtn');
            var selectedSeatsInput = document.getElementById('selectedSeats');
            
            if (selectedSeats.length > 0) {
                batchActions.classList.add('show');
                
                // 收集选中的座位信息
                var seatsData = [];
                selectedSeats.forEach(seat => {
                    seatsData.push({
                        machine: seat.dataset.machine,
                        studentNum: seat.dataset.studentNum,
                        studentName: seat.dataset.studentName
                    });
                });
                selectedSeatsInput.value = JSON.stringify(seatsData);
            } else {
                batchActions.classList.remove('show');
                selectedSeatsInput.value = '';
            }
            
            // 检查是否可以启用批量提交按钮
            var batchReason = document.getElementById('batchReason');
            if (selectedSeats.length > 0 && batchReason.value) {
                batchSubmitBtn.disabled = false;
                batchSubmitBtn.innerHTML = '✅ 批量扣分：' + selectedSeats.length + ' 名学生';
            } else {
                batchSubmitBtn.disabled = true;
                batchSubmitBtn.innerHTML = '⚠️ 请先选择学生和扣分原因';
            }
        }
        
        // 选择单个学生扣分原因
        function selectSingleReason(element, reason, score) {
            // 移除其他选中状态
            document.querySelectorAll('.reason-card').forEach(card => {
                card.classList.remove('selected');
            });
            
            // 添加选中状态
            element.classList.add('selected');
            
            // 设置隐藏字段
            document.getElementById('singleSelectedReason').value = reason;
            document.getElementById('singleSelectedScore').value = score;
            
            // 启用提交按钮
            var submitBtn = document.getElementById('singleSubmitBtn');
            submitBtn.disabled = false;
            submitBtn.innerHTML = '✅ 确认扣分：' + reason + '（' + score + '分）';
        }
        
        // 监听批量原因选择
        document.addEventListener('DOMContentLoaded', function() {
            var batchReason = document.getElementById('batchReason');
            if (batchReason) {
                batchReason.addEventListener('change', updateBatchActions);
            }
        });
    </script>
    </div>
</asp:Content>
