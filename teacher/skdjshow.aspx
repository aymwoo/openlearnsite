<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="skdjshow.aspx.cs" Inherits="Teacher_skdjshow" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">  
    <div class="placehold">  
        <div class="cline"></div>
        信息科技上课登记表
        <br />
        <div class="centerdiv">
        <asp:DropDownList ID="ddlYearFilter" runat="server" AutoPostBack="true" 
            OnSelectedIndexChanged="ddlYearFilter_SelectedIndexChanged">
            <asp:ListItem Value="current" Text="当前年份记录" Selected="True" />
            <asp:ListItem Value="all" Text="所有记录" />
        </asp:DropDownList>
        <!-- 添加文本框控件 -->
        <asp:TextBox ID="txtField1" runat="server"></asp:TextBox>
            <!-- 添加导出按钮 -->
        <asp:Button ID="btnExportCurrent" runat="server" Text="导出当前年份记录" 
            OnClick="btnExportCurrent_Click" />
        <asp:Button ID="btnExportAll" runat="server" Text="导出所有记录" 
            OnClick="btnExportAll_Click" />
        <asp:GridView ID="GVSkdj" runat="server" 
            AutoGenerateColumns="False"
            PageSize="20"  
            AllowPaging="true"
            OnPageIndexChanging="GVSkdj_PageIndexChanging"
            OnRowEditing="GVSkdj_RowEditing"
            OnRowCancelingEdit="GVSkdj_RowCancelingEdit" 
            OnRowUpdating="GVSkdj_RowUpdating"
            DataKeyNames="Ssid"
            CellPadding="2"
            Width="100%" ToolTip="上课登记记录"  SkinID="GridViewInfo"
            onrowdatabound="GVSkdj_RowDataBound" EnableModelValidation="True">            
            <FooterStyle BackColor="White" ForeColor="#333333" />
            <Columns>
                <asp:BoundField DataField="RowNumber" HeaderText="序号" ReadOnly="true" />
                
                <asp:TemplateField HeaderText="年">
                    <ItemTemplate>
                        <asp:Label ID="lblYear" runat="server" Text='<%# Eval("Ssyear") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtYear" runat="server" Text='<%# Bind("Ssyear") %>' Width="50px"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="月">
                    <ItemTemplate>
                        <asp:Label ID="lblMonth" runat="server" Text='<%# Eval("Ssmonth") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtMonth" runat="server" Text='<%# Bind("Ssmonth") %>' Width="30px"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="日">
                    <ItemTemplate>
                        <asp:Label ID="lblDay" runat="server" Text='<%# Eval("Ssday") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDay" runat="server" Text='<%# Bind("Ssday") %>' Width="30px"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="星期">
                    <ItemTemplate>
                        <asp:Label ID="lblWeek" runat="server" Text='<%# Eval("Ssweek") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtWeek" runat="server" Text='<%# Bind("Ssweek") %>' Width="40px"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="节次">
                    <ItemTemplate>
                        <asp:Label ID="lblSession" runat="server" Text='<%# Eval("Ssession") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtSession" runat="server" Text='<%# Bind("Ssession") %>' Width="40px"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="年级">
                    <ItemTemplate>
                        <asp:Label ID="lblGrade" runat="server" Text='<%# Eval("Ssgrade") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtGrade" runat="server" Text='<%# Bind("Ssgrade") %>' Width="40px"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="班级">
                    <ItemTemplate>
                        <asp:Label ID="lblClass" runat="server" Text='<%# Eval("Ssclass") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtClass" runat="server" Text='<%# Bind("Ssclass") %>' Width="40px"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="上课内容">
                    <ItemTemplate>
                        <asp:Label ID="lblTitle" runat="server" Text='<%# Eval("Ssctitle") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtTitle" runat="server" Text='<%# Bind("Ssctitle") %>' Width="200px"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="上课教师">
                    <ItemTemplate>
                        <asp:Label ID="lblTeacher" runat="server" Text='<%# Eval("Sstname") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtTeacher" runat="server" Text='<%# Bind("Sstname") %>' Width="80px"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="备注">
                    <ItemTemplate>
                        <asp:Label ID="lblNote" runat="server" Text='<%# Eval("Ssnotes") %>'></asp:Label>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:TextBox ID="txtNote" runat="server" Text='<%# Bind("Ssnotes") %>' Width="150px"></asp:TextBox>
                    </EditItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="操作">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="编辑" />
                        <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("Ssid") %>' Text="删除" OnClick="btnDelete_Click" OnClientClick="return confirm('确定要删除吗？');" />
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" Text="更新" />
                        <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" Text="取消" />
                    </EditItemTemplate>
                </asp:TemplateField>
            </Columns>
            <PagerSettings Mode="NumericFirstLast" FirstPageText="首页" LastPageText="末页" />
            <PagerStyle HorizontalAlign="Center" />
        </asp:GridView>
        </div>
        <br />
        <asp:Button ID="ButtonReturn" runat="server" onclick="ButtonReturn_Click" Text="返回" SkinID="BtnNormal" />
        <br />
    </div>
</asp:Content>