<%@ Page Title="" Language="C#" MasterPageFile="~/teacher/Teach.master" StylesheetTheme="Teacher" AutoEventWireup="true" CodeFile="mythware.aspx.cs" Inherits="Teacher_mythware" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../js/fileupload.css" rel="stylesheet" />
    

    <div class="myth-page">
        <div class="myth-shell">
            <div class="myth-hero">
                <h1 class="myth-hero__title">极域班级模型</h1>
                <p class="myth-hero__subtitle">根据签到记录生成极域ClassModel班级模型文件，支持上传原有模型并自动匹配学生座位。</p>
            </div>

            <div class="myth-grid">
                <div class="myth-card myth-card--span-5 myth-card--files">
                    <div class="myth-card__head">
                        <h2 class="myth-card__title">模型文件</h2>
                        <asp:Button ID="ImgBtnDown" runat="server"
                            Text="打包下载" OnClick="ImgBtnDown_Click"
                            ToolTip="点击打包下载" CssClass="myth-download-btn" />
                    </div>
                    <div class="myth-card__body">
                        <asp:DataList ID="Dlfilelist" runat="server"
                            RepeatColumns="1" RepeatDirection="Vertical" RepeatLayout="Flow"
                            CssClass="myth-file-list" CellPadding="0" CellSpacing="0">
                            <ItemTemplate>
                                <div class="myth-file-item">
                                    <span class="myth-file-id"><asp:Label ID="Labelfid" runat="server" Text='<%# Eval("fid") %>'></asp:Label></span>
                                    <asp:HyperLink ID="HLfname" runat="server" Target="_blank" Text='<%# Eval("fname") %>'></asp:HyperLink>
                                    <span class="myth-file-size"><asp:Label ID="Labelfsize" runat="server" Text='<%# Eval("fsize") %>'></asp:Label></span>
                                    <span class="myth-file-flag"><asp:Label ID="Labelfread" runat="server" Text='<%# Eval("fread") %>' ToolTip="是否只读（T：只读 | F：可写）"></asp:Label></span>
                                    <asp:Label ID="Labelurl" runat="server" Text='<%# Eval("furl") %>' CssClass="myth-hidden"></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                </div>

                <div class="myth-card myth-card--span-7 myth-card--build">
                    <div class="myth-card__head">
                        <h2 class="myth-card__title">生成模型</h2>
                    </div>
                    <div class="myth-card__body">
                        <div class="myth-form">
                            <div class="myth-field">
                                <span class="myth-label">上传原有班级模型（xml/cls格式）</span>
                                <div class="ls-upload" data-accept=".xml,.cls" data-label="点击或拖拽上传班级模型" data-hint="支持 xml / cls 格式">
                                    <asp:FileUpload ID="FuClassModel" runat="server" />
                                </div>
                            </div>

                            <div>
                                <span class="myth-check-group">
                                    <asp:CheckBox ID="CkMachine" runat="server" Text="空余学生机预处理为主机名"
                                        ToolTip="主机名与IP对应表有记录则有效" Checked="True" />
                                </span>
                            </div>

                            <div class="myth-field">
                                <span class="myth-label">签到时间范围</span>
                                <asp:DropDownList ID="DDLmonth" runat="server" CssClass="myth-select">
                                    <asp:ListItem Value="1" Selected="True">1周内</asp:ListItem>
                                    <asp:ListItem Value="2">2周内</asp:ListItem>
                                    <asp:ListItem Value="3">3周内</asp:ListItem>
                                    <asp:ListItem Value="4">4周内</asp:ListItem>
                                    <asp:ListItem Value="5">5周内</asp:ListItem>
                                    <asp:ListItem Value="6">6周内</asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <div class="myth-field">
                                <span class="myth-label">电脑室名称</span>
                                <div class="myth-room-row">
                                    <asp:TextBox ID="TextBoxRoom" runat="server" CssClass="myth-input" style="width: 120px;"></asp:TextBox>
                                    <asp:HyperLink ID="Hlkroom" runat="server"
                                        NavigateUrl="~/teacher/myseat.aspx" Target="_blank"
                                        ToolTip="机房视图预览" CssClass="myth-room-preview">
                                        <img src="../images/zoom.gif" alt="预览" />
                                    </asp:HyperLink>
                                </div>
                            </div>

                            <div>
                                <asp:Button ID="BtnBuild" runat="server" onclick="BtnBuild_Click"
                                    Text="生成任教班级模型" CssClass="myth-btn" />
                            </div>

                            <asp:Label ID="Labelmsg" runat="server" CssClass="myth-msg"></asp:Label>

                            <div class="myth-hint">根据最近几周内签到表的姓名与IP对应，生成所教班级模型，完成后可点击打包按钮下载。</div>
                        </div>
                    </div>
                </div>
            </div>

            <asp:Label ID="Labeldirhid" runat="server" CssClass="myth-hidden"></asp:Label>
            <asp:Label ID="Labeldir" runat="server" CssClass="myth-hidden"></asp:Label>
        </div>
    </div>
    <script src="../js/fileupload.js"></script>
</asp:Content>
