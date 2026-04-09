<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="signin.aspx.cs" Inherits="Teacher_signin" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    

    <div class="signin-page">
        <div class="lesson-shell">
            <!-- Header -->
            <div class="lesson-hero">
                <div class="lesson-hero__content">
                    <div>
                        <h1 class="lesson-hero__title">
                            <i class="bi bi-person-check-fill" style="color: #99f6e4;"></i> 学生签到记录
                        </h1>
                        <p class="lesson-hero__subtitle">查看各班级每日签到汇总，支持数据导出与详细记录巡查</p>
                    </div>
                    
                    <div class="filter-group">
                        <div class="filter-item">
                            <span class="filter-label">年级</span>
                            <asp:DropDownList ID="DDLgrade" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="DDLgrade_SelectedIndexChanged" CssClass="filter-select">
                            </asp:DropDownList>
                        </div>
                        <div style="width: 1px; height: 16px; background: rgba(255,255,255,0.2);"></div>
                        <div class="filter-item">
                            <span class="filter-label">班级</span>
                            <asp:DropDownList ID="DDLclass" runat="server" AutoPostBack="True" 
                                onselectedindexchanged="DDLclass_SelectedIndexChanged" CssClass="filter-select">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Main Content -->
            <div class="lesson-card">
                <div class="action-bar">
                    <asp:Button ID="BtnExcel" runat="server" OnClick="BtnExcel_Click" 
                        Text="导出签到表" ToolTip="将本学期本班签到以Excel表格导出" CssClass="signin-btn signin-btn--primary" />
                    
                    <asp:Button ID="BtnExcelNoSign" runat="server" OnClick="BtnExcelNoSign_Click" 
                        Text="导出缺席表" ToolTip="将本学期本班缺席以Excel表格导出" CssClass="signin-btn signin-btn--outline" />
                </div>

                <asp:GridView ID="GVSignin" runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False" PageSize="20" Width="100%" 
                    onpageindexchanging="GVSignin_PageIndexChanging" 
                    onrowdatabound="GVSignin_RowDataBound" GridLines="None"
                    CssClass="signin-grid">
                    <Columns>
                        <asp:BoundField HeaderText="序号">
                            <ItemStyle CssClass="font-mono text-slate-400" Width="80px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Sgrade" HeaderText="年级" />
                        <asp:BoundField DataField="Sclass" HeaderText="班级">
                            <ItemStyle CssClass="font-bold text-slate-700" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Qyear" HeaderText="年份">
                            <ItemStyle CssClass="font-mono" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Qmonth" HeaderText="月份">
                            <ItemStyle CssClass="font-mono" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Qday" HeaderText="日期">
                            <ItemStyle CssClass="font-mono" />
                        </asp:BoundField>
                        <asp:HyperLinkField DataNavigateUrlFields="Sgrade,Sclass,Qyear,Qmonth,Qday" 
                            DataNavigateUrlFormatString="signshow.aspx?sgrade={0}&amp;&amp;sclass={1}&amp;&amp;qyear={2}&amp;&amp;qmonth={3}&amp;&amp;qday={4}" 
                            Text='<i class="bi bi-eye-fill"></i> 查看详细' HeaderText="操作">
                            <ItemStyle CssClass="view-link" />
                        </asp:HyperLinkField>
                    </Columns>
                    <PagerTemplate>
                        <div class="pager-container">
                            <span>第 <asp:Label ID="lblPageIndex" runat="server" Text="<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>" style="color: #4f46e5;"></asp:Label> / <asp:Label ID="lblPageCount" runat="server" Text="<%# ((GridView)Container.Parent.Parent).PageCount %>"></asp:Label> 页</span>
                            
                            <div class="pager-buttons">
                                <asp:LinkButton ID="btnFirst" runat="server" CommandArgument="First" CommandName="Page" CssClass="pager-btn">首页</asp:LinkButton>
                                <asp:LinkButton ID="btnPrev" runat="server" CommandArgument="Prev" CommandName="Page" CssClass="pager-btn">上一页</asp:LinkButton>
                                <asp:LinkButton ID="btnNext" runat="server" CommandArgument="Next" CommandName="Page" CssClass="pager-btn">下一页</asp:LinkButton>
                                <asp:LinkButton ID="btnLast" runat="server" CommandArgument="Last" CommandName="Page" CssClass="pager-btn">尾页</asp:LinkButton>
                            </div>
                        </div>
                    </PagerTemplate>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>