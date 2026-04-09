<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CourseSchedule.aspx.cs" Inherits="kcb_CourseSchedule" MasterPageFile="~/teacher/Teach.master" %><asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <style type="text/css">
        .header-section {
            width: 90%;
            max-width: 800px;
            margin: 0 auto 10px;
            background: white;
            padding: 5px 10px 10px;
            border-radius: 8px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
        }
        h2 {
            color: #333;
            text-align: center;
            margin: 0 0 10px;
            padding: 0 0 5px;
            border-bottom: 1px solid #eee;
        }
        .schedule-table {
            width: 90%;
            max-width: 900px;
            margin: 20px auto;
            border-collapse: separate;
            border-spacing: 0;
            font-size: 16px;
        }
        .schedule-table th {
            background-color: #9EA9B1;
            color: #111111;
            padding: 6px;
            text-align: center;
            font-weight: bold;
            width: auto;
        }
        .schedule-table td {
            padding: 10px;
            border: 1px solid #ddd;
            text-align: center;
            width: auto; /* 移除固定宽度设置 */
        }
        .schedule-table tr:nth-child(even) {
            background-color: #f9f9f9;
        }
        .schedule-table tr:hover {
            background-color: #f1f1f1;
        }
        .time-column {
            background-color: #f5f5f5;
            font-weight: bold;
        }
        .control-group {
            margin: 15px 0;
            text-align: center;
        }
        .btn {
            padding: 6px 12px;
            margin: 0 4px;
            background-color: #f0f0f0;
            color: #333333;
            border: 1px solid #ddd;
            border-radius: 4px;
            cursor: pointer;
            font-family: 'Microsoft YaHei', Arial;
            font-size: 11pt;
            text-align: center;
            display: inline-block;
            text-decoration: none;
            transition: all 0.3s ease;
        }
        .btn:hover {
            background-color: #e0e0e0;
            transform: translateY(-1px);
            box-shadow: 0 1px 3px rgba(0,0,0,0.1);
        }
        .btn-add-year {
            background-color: #f0f0f0;
            color: #333333;
            border-color: #ddd;
        }
        .btn-add-year:hover {
            background-color: #e0e0e0;
        }
        .btn-save {
            background-color: #f0f0f0;
            color: #333333;
            border-color: #ddd;
        }
        .btn-save:hover {
            background-color: #e0e0e0;
        }
        .btn-export {
            background-color: #f0f0f0;
            color: #333333;
            border-color: #ddd;
        }
        .btn-export:hover {
            background-color: #e0e0e0;
        }
        .btn-time-slot {
            background-color: #f0f0f0;
            color: #333333;
            border-color: #ddd;
        }
        .btn-time-slot:hover {
            background-color: #e0e0e0;
        }
        .message {
            padding: 10px;
            margin: 10px 0;
            border-radius: 4px;
            text-align: center;
        }
        .success {
            background-color: #dff0d8;
            color: #3c763d;
        }
        .error {
            background-color: #f2dede;
            color: #a94442;
        }
        select[id*="ddlClass"] {
            -webkit-appearance: none;
            -moz-appearance: none;
            appearance: none;
            background: url('data:image/svg+xml;utf8,<svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24"><path fill="none" d="M0 0h24v24H0z"/><path d="M7 10l5 5 5-5z"/></svg>') no-repeat right 8px center/12px 12px;
            padding-right: 5px !important;
        }
        .class-select {
        width: 95%;
        padding: 8px;
        border: none;
        background-color: #f8f9fa;
        font-weight: bold;
        font-size: 24px; /* 从14px增大到24px */
        border-radius: 4px;
        box-shadow: 0 0 0 1px #ddd;
    }
    .class-select option {
        padding: 8px;
        font-weight: bold;
        font-size: 24px; /* 从14px增大到24px */
    }
    .class-select:hover {
        background-color: #e9ecef;
    }
    .class-select option {
        padding: 8px;
        font-weight: bold;
        font-size: 14px;
    }
    </style>
        <div class="header-section" style="width: 90%; max-width: 900px; margin: 0 auto;" runat="server" id="HeaderSection">
            <h2>课程表</h2>
            <div class="control-group" style="width: 100%; max-width: 900px; margin: 0 auto;">
                学年度：<asp:DropDownList ID="ddlYear" runat="server" AutoPostBack="true" 
                    OnSelectedIndexChanged="ddlYear_SelectedIndexChanged" Width="90px" style="padding: 4px 4px; font-size: 11pt;">
                </asp:DropDownList>
                学期：<asp:DropDownList ID="ddlTerm" runat="server" AutoPostBack="true" 
                    OnSelectedIndexChanged="ddlTerm_SelectedIndexChanged" Width="70px" style="padding: 4px 4px; font-size: 11pt;">
                    <asp:ListItem Text="1期" Value="1" />
                    <asp:ListItem Text="2期" Value="2" />
                </asp:DropDownList>
                    <asp:Button ID="btnCopySchedule" runat="server" Text="复制课表" OnClick="btnCopySchedule_Click" CssClass="btn btn-add-year" />
                    &nbsp;&nbsp;
                    科目：<asp:DropDownList ID="ddlSubject" runat="server" Width="100px" style="padding: 4px 4px; font-size: 11pt;" AutoPostBack="true" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged">
                        <asp:ListItem Text="信息技术" Value="信息技术" />
                        <asp:ListItem Text="人工智能" Value="人工智能" />
                        <asp:ListItem Text="机器人" Value="机器人" />
                        <asp:ListItem Text="社团" Value="社团" />
                    </asp:DropDownList>
                    &nbsp;&nbsp;
                    <asp:Button ID="btnSave" runat="server" Text="保存" OnClick="btnSave_Click" CssClass="btn btn-save" />
                    <asp:Button ID="btnExport" runat="server" Text="导出" OnClick="btnExport_Click" CssClass="btn btn-export" />
                    <asp:HyperLink ID="hlTimeSlot" runat="server" NavigateUrl="~/kcb/TimeSlotManagement.aspx" Text="作息时间" CssClass="btn btn-time-slot" />
                    <asp:Button ID="btnReturn" runat="server" Text="返回" OnClick="btnReturn_Click" CssClass="btn" ToolTip="返回管理页面" />
            </div>
            <asp:Label ID="lblMessage" runat="server" CssClass="message" Visible="false"></asp:Label>
        </div>

        <table class="schedule-table">
            <tr>
                <th class="style3">节次</th>
                <th class="style3">时间</th>
                <th>星期一</th>
                <th>星期二</th>
                <th>星期三</th>
                <th>星期四</th>
                <th>星期五</th>
                <th>星期六</th>
                <th>星期日</th>
            </tr>
            <!-- 插入Repeater控件 -->
            <asp:Repeater ID="rptTimeSlots" runat="server"> 
                <ItemTemplate> 
                    <tr> 
                        <td><%# Eval("SlotName") %></td>
                        <td class="time-column"><%# GetTimeSlotText(Container.ItemIndex + 1) %></td> 
                        <td><asp:DropDownList ID="ddlClass1" runat="server" CssClass="class-select"></asp:DropDownList></td>
                        <td><asp:DropDownList ID="ddlClass2" runat="server" CssClass="class-select"></asp:DropDownList></td>
                        <td><asp:DropDownList ID="ddlClass3" runat="server" CssClass="class-select"></asp:DropDownList></td>
                        <td><asp:DropDownList ID="ddlClass4" runat="server" CssClass="class-select"></asp:DropDownList></td>
                        <td><asp:DropDownList ID="ddlClass5" runat="server" CssClass="class-select"></asp:DropDownList></td>
                        <td><asp:DropDownList ID="ddlClass6" runat="server" CssClass="class-select"></asp:DropDownList></td>
                        <td><asp:DropDownList ID="ddlClass7" runat="server" CssClass="class-select"></asp:DropDownList></td>
                    </tr> 
                </ItemTemplate> 
            </asp:Repeater>
            <!-- 后续的课程表表格结构 -->
            </table>
        </div>
</asp:Content>
