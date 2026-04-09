<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Analysis.aspx.cs" Inherits="webform_Analysis" ResponseEncoding="utf-8" Culture="zh-CN" UICulture="zh-CN" %>

<!DOCTYPE html>
<html lang="zh-CN">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <title>学生成绩表</title>
    

    <link href="../js/css/tailwind-utilities-2.2.19.min.css" rel="stylesheet">
    <link rel="stylesheet" type="text/css" href="../App_Themes/Teacher/Analysis.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="page-shell">
            <div class="hero">
                <h1>学生成绩分析</h1>
                <p>查看班级测验得分、用时、题目正确率与未参加名单。</p>
            </div>

        <div class="container">
            <div class="header">
                <h2 class ="txtcenter">学生成绩表</h2>
                <p class ="txtcenter">实时成绩、平均分与用时统计</p>
                <div class="txtcenter">
                    <span class="page-status"><%=PageStatus %></span>
                </div>
            </div>
            
            <div class="table-container">
                <asp:Repeater ID="RepeaterList" runat="server">
                    <HeaderTemplate>
                        <table>
                            <thead>
                                <tr>
                                    <th>序号</th>
                                    <th>姓名</th>
                                    <th>成绩</th>
                                    <th>用时（分）</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td class="number-cell"><%# Container.ItemIndex + 1 %></td>
                            <td class="name-cell"><%# HttpUtility.UrlDecode(Eval("Asname").ToString()) %></td>
                            <td class="score-cell"><%# Eval("Ascore") %></td>
                            <td class="time-cell"><%# Eval("Aspent") %></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
            
            <div class="stats">
                <div class="stat-item">
                    <h3><%=Persons %>人</h3>
                    <p>总人数</p>
                </div>
                <div class="stat-item">
                    <h3><%=avgScore %></h3>
                    <p>平均分</p>
                </div>
                <div class="stat-item">
                    <h3><%=avgSpent %>分钟</h3>
                    <p>平均用时</p>
                </div>
            </div>
            <div class="table-container">
                <h3 class="analysis-title">详题分析</h3>
                 <asp:Repeater ID="RepeaterAnalysis" runat="server">
                    <HeaderTemplate>
                        <table style="width:100%;">
                        <tr>
                            <th>序号</th>
                            <th>题型</th>
                            <th>题目内容</th>
                            <th>答对数</th>
                            <th>正确率</th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td class="sizesmall"><%# Container.ItemIndex + 1 %></td>
                            <td class="size"><%# Eval("QuestionTypeText") %></td>
                            <td><%# Eval("QuestionTitle") %></td>
                            <td  class="size"><%# Eval("CorrectCount") %></td>
                            <td  class="size"><%# Eval("Accuracy") %>%</td>
                        </tr>
                    </ItemTemplate>
                     <FooterTemplate></table></FooterTemplate>
                 </asp:Repeater>
            </div>
            <div class="footer">
            <h3>未测验学生名单</h3>
                 <asp:Repeater ID="RepeaterNo" runat="server">
                    <ItemTemplate>                        
                            <span class="snameno" ><%# Eval("Sname")%></span>
                    </ItemTemplate>
                 </asp:Repeater>
            </div>
        </div>
        </div>
    </form>
</body>
</html>
