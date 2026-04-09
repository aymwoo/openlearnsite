<%@ Page Title="学生荣誉榜" Language="C#" StylesheetTheme="Student" AutoEventWireup="true"
    CodeFile="honorboard.aspx.cs" Inherits="LearnSite.Student.honorboard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>学生荣誉榜</title>
    <style>
        /* 荣誉榜样式 */
        .honor-container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
            background: #f5f7fa;
        }

        .honor-header {
            text-align: center;
            padding: 30px 0;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            border-radius: 10px;
            margin-bottom: 30px;
        }

        .honor-header h1 {
            font-size: 32px;
            margin: 0 0 10px 0;
        }

        .honor-header p {
            font-size: 16px;
            margin: 0;
            opacity: 0.9;
        }

        /* 标签页导航 */
        .honor-tabs {
            display: flex;
            gap: 10px;
            margin-bottom: 30px;
            flex-wrap: wrap;
        }

        .honor-tab {
            padding: 12px 30px;
            background: white;
            border: 2px solid #e0e0e0;
            border-radius: 25px;
            cursor: pointer;
            font-size: 16px;
            font-weight: bold;
            transition: all 0.3s ease;
        }

        .honor-tab:hover {
            background: #f0f0f0;
            transform: translateY(-2px);
        }

        .honor-tab.active {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            border-color: #667eea;
        }

        /* 筛选区域 */
        .filter-section {
            background: white;
            padding: 20px;
            border-radius: 10px;
            margin-bottom: 30px;
            display: flex;
            gap: 15px;
            align-items: center;
            flex-wrap: wrap;
        }

        .filter-section label {
            font-weight: bold;
            margin-right: 10px;
        }

        .filter-section select,
        .filter-section input {
            padding: 8px 15px;
            border: 1px solid #ddd;
            border-radius: 5px;
            font-size: 14px;
        }

        .filter-section button {
            padding: 8px 25px;
            background: #667eea;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 14px;
            font-weight: bold;
            transition: background 0.3s ease;
        }

        .filter-section button:hover {
            background: #5568d3;
        }

        /* 荣誉网格 */
        .honor-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
            gap: 25px;
            margin-bottom: 30px;
        }

        /* 荣誉卡片 */
        .honor-card {
            background: white;
            border-radius: 15px;
            padding: 25px;
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.08);
            transition: all 0.3s ease;
            cursor: pointer;
        }

        .honor-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 8px 25px rgba(0, 0, 0, 0.15);
        }

        .honor-card-header {
            display: flex;
            align-items: center;
            margin-bottom: 15px;
        }

        .honor-icon {
            font-size: 48px;
            margin-right: 15px;
        }

        .honor-title {
            font-size: 20px;
            font-weight: bold;
            color: #333;
        }

        .honor-type {
            font-size: 14px;
            color: #999;
            margin-top: 5px;
        }

        .honor-students {
            margin-top: 15px;
            padding-top: 15px;
            border-top: 1px solid #eee;
        }

        .honor-student-item {
            display: flex;
            align-items: center;
            padding: 10px 0;
            border-bottom: 1px solid #f5f5f5;
        }

        .honor-student-item:last-child {
            border-bottom: none;
        }

        .honor-student-avatar {
            width: 40px;
            height: 40px;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            color: white;
            font-weight: bold;
            font-size: 16px;
            margin-right: 15px;
        }

        .honor-student-info {
            flex: 1;
        }

        .honor-student-name {
            font-weight: bold;
            color: #333;
            font-size: 15px;
        }

        .honor-student-class {
            font-size: 13px;
            color: #999;
        }

        /* 徽章样式 */
        .badge {
            display: inline-block;
            padding: 5px 12px;
            border-radius: 15px;
            font-size: 12px;
            font-weight: bold;
            margin-left: 10px;
        }

        .badge.bronze {
            background: linear-gradient(135deg, #cd7f32 0%, #a0522d 100%);
            color: white;
        }

        .badge.silver {
            background: linear-gradient(135deg, #c0c0c0 0%, #a9a9a9 100%);
            color: white;
        }

        .badge.gold {
            background: linear-gradient(135deg, #ffd700 0%, #ffb347 100%);
            color: white;
        }

        /* 空状态 */
        .empty-state {
            text-align: center;
            padding: 60px 20px;
            color: #999;
        }

        .empty-state-icon {
            font-size: 80px;
            margin-bottom: 20px;
        }

        .empty-state-text {
            font-size: 18px;
        }

        /* 响应式设计 */
        @media (max-width: 768px) {
            .honor-grid {
                grid-template-columns: 1fr;
            }

            .honor-tabs {
                justify-content: center;
            }

            .filter-section {
                flex-direction: column;
                align-items: stretch;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="honor-container">
            <!-- 头部 -->
            <div class="honor-header">
                <h1>🏆 学生荣誉榜 🏆</h1>
                <p>展示学生在学习活动中的优秀表现和成就</p>
            </div>

            <!-- 标签页 -->
            <div class="honor-tabs">
                <asp:Button ID="BtnAll" runat="server" Text="全部荣誉" CssClass="honor-tab active" OnClick="BtnAll_Click" />
                <asp:Button ID="BtnAcademic" runat="server" Text="学术成就" CssClass="honor-tab" OnClick="BtnAcademic_Click" />
                <asp:Button ID="BtnBehavior" runat="server" Text="行为表现" CssClass="honor-tab" OnClick="BtnBehavior_Click" />
                <asp:Button ID="BtnTeam" runat="server" Text="团队协作" CssClass="honor-tab" OnClick="BtnTeam_Click" />
                <asp:Button ID="BtnSpecialty" runat="server" Text="特色专长" CssClass="honor-tab" OnClick="BtnSpecialty_Click" />
                <asp:Button ID="BtnComprehensive" runat="server" Text="综合荣誉" CssClass="honor-tab" OnClick="BtnComprehensive_Click" />
            </div>

            <!-- 筛选区域 -->
        <div id="FilterSection" class="filter-section" runat="server">
            <label>学期：</label>
            <asp:DropDownList ID="DdlTerm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed">
                <asp:ListItem Text="全部" Value=""></asp:ListItem>
                <asp:ListItem Text="第一学期" Value="TERM1"></asp:ListItem>
                <asp:ListItem Text="第二学期" Value="TERM2"></asp:ListItem>
            </asp:DropDownList>

            <asp:Button ID="BtnRefresh" runat="server" Text="刷新荣誉榜" OnClick="BtnRefresh_Click" />
            <asp:Button ID="BtnMyHonors" runat="server" Text="我的荣誉" OnClick="BtnMyHonors_Click" />
            <asp:Button ID="BtnBack" runat="server" Text="返回" OnClick="BtnBack_Click" />
        </div>

            <!-- 荣誉网格 -->
            <asp:Panel ID="PanelHonors" runat="server" CssClass="honor-grid">
                <!-- 动态生成的荣誉卡片 -->
            </asp:Panel>

            <!-- 空状态 -->
            <asp:Panel ID="PanelEmpty" runat="server" CssClass="empty-state" Visible="false">
                <div class="empty-state-icon">📭</div>
                <div class="empty-state-text">暂无荣誉数据</div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
