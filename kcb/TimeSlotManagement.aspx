<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TimeSlotManagement.aspx.cs" Inherits="kcb_TimeSlotManagement" MasterPageFile="~/teacher/Teach.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <style type="text/css">
        .grid-view { width: 60%; margin: 20px auto; }
        .form-group { margin: 10px 0; }
        .btn { padding: 5px 15px; margin: 0 5px; }
        .schedule-table td, .schedule-table th { 
            border: 1px solid #ddd; 
            padding: 12px;  /* 增加内边距 */
            text-align: center; 
            width: 12%;
            font-size: 14px; /* 增大字体大小 */
        }
        .time-column {
            width: 10%;
            font-size: 16px; /* 时间列字体更大 */
            font-weight: bold;
        }
    .container {
        width: 95%;
        max-width: 1000px;
        margin: 0 auto;
        background: white;
        padding: 5px 20px 20px;
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
    .grid-view {
        width: 100%;
        margin: 20px auto;
        font-size: 16px;
        border-collapse: separate;
        border-spacing: 0;
    }
    .grid-view th {
        background-color: #9EA9B1;
        color: #111111;
        padding: 15px;
        text-align: center;
        font-weight: bold;
    }
    .grid-view td {
        padding: 15px;
        border: 1px solid #ddd;
        text-align: center;
        vertical-align: middle;
        white-space: nowrap;
    }
    .grid-view tr:nth-child(even) {
        background-color: #f9f9f9;
    }
    .grid-view tr:hover {
        background-color: #f1f1f1;
    }
    .form-group { 
        margin: 10px 0;
        text-align: center;
    }
    .btn {
        padding: 8px 16px;
        margin: 0 5px;
        background-color: #E6E6E6;
        color: #333333;
        border: 1px solid #D4D4D4;
        border-radius: 4px;
        cursor: pointer;
        font-family: Arial;
        font-size: 11pt;
        width: auto;
        height: 40px;
        text-align: center;
    }
    .btn:hover {
        background-color: #D4D4D4;
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
    </style>
        <div style="width: 80%; max-width: 600px; margin: 0 auto;">
            <h2 style="text-align: center">上课时间修改</h2>
            <div class="form-group">
                <asp:Button ID="btnAdd" runat="server" Text="添加节次" OnClick="btnAdd_Click" CssClass="btn" />
                <asp:Button ID="btnBack" runat="server" Text="返回" OnClick="btnBack_Click" CssClass="btn" />
            </div>
            <asp:GridView ID="gvTimeSlots" runat="server" AutoGenerateColumns="false" 
                DataKeyNames="SlotID" 
                CssClass="grid-view" OnRowEditing="gvTimeSlots_RowEditing" 
                OnRowUpdating="gvTimeSlots_RowUpdating" OnRowCancelingEdit="gvTimeSlots_RowCancelingEdit"
                OnRowDeleting="gvTimeSlots_RowDeleting" 
                onselectedindexchanged="gvTimeSlots_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="SlotID" HeaderText="序号" ReadOnly="true" />
                    <asp:BoundField DataField="SlotName" HeaderText="节次" />
                    <asp:BoundField DataField="StartTime" HeaderText="开始时间" />
                    <asp:BoundField DataField="EndTime" HeaderText="结束时间" />
                    <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
                </Columns>
            </asp:GridView>
            <asp:Label ID="lblMessage" runat="server" CssClass="message" Visible="false"></asp:Label>
        </div>
</asp:Content>