<%@ Page Title="我的荣誉" Language="C#" StylesheetTheme="Student" AutoEventWireup="true"
    CodeFile="myhonors.aspx.cs" Inherits="LearnSite.Student.myhonors" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
    <title>我的荣誉</title>
    <style>
        .my-honor-container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 20px;
            background: #f5f7fa;
        }

        /* 个人信息卡片 */
        .profile-card {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 40px;
            border-radius: 15px;
            margin-bottom: 30px;
            box-shadow: 0 8px 25px rgba(102, 126, 234, 0.3);
        }

        .profile-content {
            display: flex;
            align-items: center;
            gap: 30px;
        }

        .profile-avatar {
            width: 120px;
            height: 120px;
            background: rgba(255, 255, 255, 0.2);
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 48px;
            font-weight: bold;
        }

        .profile-info {
            flex: 1;
        }

        .profile-name {
            font-size: 32px;
            font-weight: bold;
            margin: 0 0 10px 0;
        }

        .profile-details {
            font-size: 16px;
            opacity: 0.9;
            margin-bottom: 15px;
        }

        .profile-stats {
            display: flex;
            gap: 30px;
        }

        .stat-item {
            text-align: center;
        }

        .stat-value {
            font-size: 32px;
            font-weight: bold;
        }

        .stat-label {
            font-size: 14px;
            opacity: 0.8;
        }

        /* 荣誉统计卡片 */
        .honor-stats-grid {
            display: grid;
            grid-template-columns: repeat(4, 1fr);
            gap: 20px;
            margin-bottom: 30px;
        }

        .honor-stat-card {
            background: white;
            padding: 25px;
            border-radius: 15px;
            text-align: center;
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.08);
            transition: transform 0.3s ease;
        }

        .honor-stat-card:hover {
            transform: translateY(-5px);
        }

        .stat-icon {
            font-size: 40px;
            margin-bottom: 10px;
        }

        .stat-number {
            font-size: 36px;
            font-weight: bold;
            color: #667eea;
        }

        .stat-title {
            font-size: 14px;
            color: #666;
            margin-top: 5px;
        }

        /* 荣誉墙 */
        .honor-wall-section {
            background: white;
            padding: 30px;
            border-radius: 15px;
            margin-bottom: 30px;
            box-shadow: 0 4px 15px rgba(0, 0, 0, 0.08);
        }

        .section-title {
            font-size: 24px;
            font-weight: bold;
            color: #333;
            margin-bottom: 25px;
            padding-bottom: 15px;
            border-bottom: 2px solid #f0f0f0;
        }

        .honor-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
            gap: 20px;
        }

        .honor-item {
            background: #f8f9fa;
            border-radius: 12px;
            padding: 20px;
            text-align: center;
            transition: all 0.3s ease;
            cursor: pointer;
            border: 2px solid transparent;
        }

        .honor-item:hover {
            background: white;
            border-color: #667eea;
            transform: translateY(-5px);
            box-shadow: 0 8px 20px rgba(102, 126, 234, 0.2);
        }

        .honor-item.bronze {
            border-left: 4px solid #cd7f32;
        }

        .honor-item.silver {
            border-left: 4px solid #c0c0c0;
        }

        .honor-item.gold {
            border-left: 4px solid #ffd700;
            background: linear-gradient(135deg, #fff9e6 0%, #ffffff 100%);
        }

        .honor-emoji {
            font-size: 48px;
            margin-bottom: 10px;
        }

        .honor-name {
            font-size: 16px;
            font-weight: bold;
            color: #333;
            margin-bottom: 5px;
        }

        .honor-level {
            font-size: 13px;
            color: #666;
            margin-bottom: 10px;
        }

        .honor-count {
            font-size: 12px;
            color: #999;
        }

        /* 待解锁荣誉 */
        .locked-honor {
            opacity: 0.5;
            filter: grayscale(100%);
        }

        .lock-icon {
            position: absolute;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            font-size: 32px;
            color: #999;
        }

        /* 荣誉时间线 */
        .honor-timeline {
            max-width: 800px;
            margin: 0 auto;
        }

        .timeline-item {
            display: flex;
            padding: 20px 0;
            border-bottom: 1px solid #eee;
        }

        .timeline-date {
            width: 150px;
            font-size: 14px;
            color: #999;
            flex-shrink: 0;
        }

        .timeline-content {
            flex: 1;
        }

        .timeline-honor-name {
            font-weight: bold;
            color: #333;
            font-size: 16px;
        }

        .timeline-honor-level {
            font-size: 14px;
            color: #666;
            margin-top: 5px;
        }

        /* 空状态 */
        .empty-honors {
            text-align: center;
            padding: 60px 20px;
            color: #999;
        }

        .empty-honors-icon {
            font-size: 80px;
            margin-bottom: 20px;
        }

        .empty-honors-text {
            font-size: 18px;
            margin-bottom: 10px;
        }

        .empty-honors-hint {
            font-size: 14px;
        }

        /* 返回按钮 */
        .back-button {
            display: inline-block;
            padding: 10px 30px;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            text-decoration: none;
            border-radius: 25px;
            font-weight: bold;
            transition: transform 0.3s ease;
        }

        .back-button:hover {
            transform: translateY(-2px);
        }

        /* 响应式设计 */
        @media (max-width: 768px) {
            .profile-content {
                flex-direction: column;
                text-align: center;
            }

            .honor-stats-grid {
                grid-template-columns: repeat(2, 1fr);
            }

            .honor-grid {
                grid-template-columns: repeat(2, 1fr);
            }

            .profile-stats {
                justify-content: center;
            }

            .timeline-item {
                flex-direction: column;
            }

            .timeline-date {
                width: 100%;
                margin-bottom: 5px;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="my-honor-container">
            <!-- 个人信息卡片 -->
            <div class="profile-card">
                <div class="profile-content">
                    <div class="profile-avatar">
                        <asp:Label ID="LblAvatar" runat="server"></asp:Label>
                    </div>
                    <div class="profile-info">
                        <h1 class="profile-name">
                            <asp:Label ID="LblName" runat="server"></asp:Label>
                        </h1>
                        <div class="profile-details">
                            <asp:Label ID="LblClass" runat="server"></asp:Label> |
                            学号：<asp:Label ID="LblSnum" runat="server"></asp:Label> |
                            总分：<asp:Label ID="LblTotalScore" runat="server"></asp:Label>
                        </div>
                        <div class="profile-stats">
                            <div class="stat-item">
                                <div class="stat-value">
                                    <asp:Label ID="LblHonorCount" runat="server">0</asp:Label>
                                </div>
                                <div class="stat-label">获得荣誉</div>
                            </div>
                            <div class="stat-item">
                                <div class="stat-value" style="color: #ffd700;">
                                    <asp:Label ID="LblGoldCount" runat="server">0</asp:Label>
                                </div>
                                <div class="stat-label">金色荣誉</div>
                            </div>
                            <div class="stat-item">
                                <div class="stat-value" style="color: #c0c0c0;">
                                    <asp:Label ID="LblSilverCount" runat="server">0</asp:Label>
                                </div>
                                <div class="stat-label">银白荣誉</div>
                            </div>
                            <div class="stat-item">
                                <div class="stat-value" style="color: #cd7f32;">
                                    <asp:Label ID="LblBronzeCount" runat="server">0</asp:Label>
                                </div>
                                <div class="stat-label">青铜荣誉</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- 荣誉统计 -->
            <div class="honor-stats-grid">
                <div class="honor-stat-card">
                    <div class="stat-icon">🏆</div>
                    <div class="stat-number">
                        <asp:Label ID="StatTotal" runat="server">0</asp:Label>
                    </div>
                    <div class="stat-title">荣誉总数</div>
                </div>
                <div class="honor-stat-card">
                    <div class="stat-icon">🥇</div>
                    <div class="stat-number" style="color: #ffd700;">
                        <asp:Label ID="StatGold" runat="server">0</asp:Label>
                    </div>
                    <div class="stat-title">金色荣誉</div>
                </div>
                <div class="honor-stat-card">
                    <div class="stat-icon">🥈</div>
                    <div class="stat-number" style="color: #c0c0c0;">
                        <asp:Label ID="StatSilver" runat="server">0</asp:Label>
                    </div>
                    <div class="stat-title">银白荣誉</div>
                </div>
                <div class="honor-stat-card">
                    <div class="stat-icon">🥉</div>
                    <div class="stat-number" style="color: #cd7f32;">
                        <asp:Label ID="StatBronze" runat="server">0</asp:Label>
                    </div>
                    <div class="stat-title">青铜荣誉</div>
                </div>
            </div>

            <!-- 已获得荣誉 -->
            <div class="honor-wall-section">
                <h2 class="section-title">✨ 我的荣誉墙</h2>
                <asp:Panel ID="PanelMyHonors" runat="server" CssClass="honor-grid">
                    <!-- 动态生成的荣誉 -->
                </asp:Panel>
                <asp:Panel ID="PanelEmptyHonors" runat="server" CssClass="empty-honors" Visible="false">
                    <div class="empty-honors-icon">📭</div>
                    <div class="empty-honors-text">还没有获得任何荣誉</div>
                    <div class="empty-honors-hint">继续努力学习，争取早日获得荣誉吧！</div>
                </asp:Panel>
            </div>

            <!-- 待解锁荣誉 -->
            <div class="honor-wall-section">
                <h2 class="section-title">🔓 待解锁荣誉</h2>
                <asp:Panel ID="PanelLockedHonors" runat="server" CssClass="honor-grid">
                    <!-- 动态生成的待解锁荣誉 -->
                </asp:Panel>
            </div>

            <!-- 荣誉时间线 -->
            <div class="honor-wall-section">
                <h2 class="section-title">📅 荣誉获得记录</h2>
                <asp:Panel ID="PanelTimeline" runat="server" CssClass="honor-timeline">
                    <!-- 动态生成的时间线 -->
                </asp:Panel>
            </div>

            <!-- 返回按钮 -->
            <div style="text-align: center; margin-top: 30px;">
                <a href="honorboard.aspx" class="back-button">进入荣誉榜</a>
                <a href="myinfo.aspx" class="back-button" style="margin-left: 15px;">返回我的学案</a>
            </div>
        </div>
    </form>
</body>
</html>
