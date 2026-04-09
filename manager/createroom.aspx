<%@ Page Title="" Language="C#" MasterPageFile="~/manager/Manage.master" AutoEventWireup="true" CodeFile="createroom.aspx.cs" Inherits="Manager_createroom" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Content" Runat="Server">
    <link href="../App_Themes/Teacher/createroom.css" rel="stylesheet" />
    
    <div class="room-page">
        <div class="room-shell">
            <div class="room-hero">
                <div class="room-hero__eyebrow">Room Setup</div>
                <h1 class="room-hero__title">班级设置</h1>
                <p class="room-hero__subtitle">用于创建和维护全校班级列表。支持按年级批量生成，也支持手动补录单个班级。</p>
                <div class="room-hero__meta">
                    <span class="room-chip">支持批量重建</span>
                    <span class="room-chip">支持手动补录</span>
                    <span class="room-chip">可分页查看全部班级</span>
                </div>
            </div>

            <div class="room-overview">
                <div class="room-overview__card">
                    <span class="room-overview__label">批量创建</span>
                    <strong class="room-overview__value">按年级区间生成班级</strong>
                    <span class="room-overview__desc">适合新学期第一次初始化班级列表，快速建立全校基础班级结构。</span>
                </div>
                <div class="room-overview__card">
                    <span class="room-overview__label">手动补录</span>
                    <strong class="room-overview__value">单独新增缺失班级</strong>
                    <span class="room-overview__desc">适合后续新增插班班级或漏建班级时快速补齐。</span>
                </div>
                <div class="room-overview__card">
                    <span class="room-overview__label">操作提醒</span>
                    <strong class="room-overview__value">批量创建会清空现有班级</strong>
                    <span class="room-overview__desc">执行前请确认已有班级数据是否仍需保留，避免误删现有班级安排。</span>
                </div>
            </div>

            <div class="room-card">
                <div class="room-card__head">
                    <h2 class="room-card__title">班级生成与补录</h2>
                    <p class="room-card__desc">先完成整校批量初始化，再根据实际情况用手动添加补齐遗漏班级。</p>
                </div>
                <div class="room-card__body">
                    <div class="room-form-grid">
                        <div class="room-section">
                            <div class="room-section__head">
                                <span class="room-section__title">批量创建班级</span>
                                <span class="room-section__hint">适合按年级区间一次性建立整校班级。</span>
                            </div>
                            <div class="room-toolbar">
                                <span class="room-label">起始年级</span>
                                <asp:DropDownList ID="DDLgrademin" runat="server" CssClass="room-select">
                                    <asp:ListItem>1</asp:ListItem><asp:ListItem>2</asp:ListItem><asp:ListItem>3</asp:ListItem><asp:ListItem>4</asp:ListItem><asp:ListItem>5</asp:ListItem><asp:ListItem>6</asp:ListItem><asp:ListItem Selected="True">7</asp:ListItem><asp:ListItem>8</asp:ListItem><asp:ListItem>9</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>11</asp:ListItem><asp:ListItem>12</asp:ListItem><asp:ListItem>13</asp:ListItem><asp:ListItem>14</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>16</asp:ListItem>
                                </asp:DropDownList>
                                <span class="room-label">结束年级</span>
                                <asp:DropDownList ID="DDLgrademax" runat="server" CssClass="room-select">
                                    <asp:ListItem>1</asp:ListItem><asp:ListItem>2</asp:ListItem><asp:ListItem>3</asp:ListItem><asp:ListItem>4</asp:ListItem><asp:ListItem>5</asp:ListItem><asp:ListItem>6</asp:ListItem><asp:ListItem>7</asp:ListItem><asp:ListItem>8</asp:ListItem><asp:ListItem Selected="True">9</asp:ListItem><asp:ListItem>10</asp:ListItem><asp:ListItem>11</asp:ListItem><asp:ListItem>12</asp:ListItem><asp:ListItem>13</asp:ListItem><asp:ListItem>14</asp:ListItem><asp:ListItem>15</asp:ListItem><asp:ListItem>16</asp:ListItem>
                                </asp:DropDownList>
                                <span class="room-label">每年级班级数上限</span>
                                <asp:DropDownList ID="DDLclassmax" runat="server" CssClass="room-select"></asp:DropDownList>
                            </div>
                            <asp:Button ID="Btncreate" runat="server" Text="批量创建" onclick="Btncreate_Click" CssClass="room-btn room-btn--primary" />
                        </div>

                        <div class="room-section">
                            <div class="room-section__head">
                                <span class="room-section__title">手动添加单个班级</span>
                                <span class="room-section__hint">适合补齐单个漏建班级，不影响已有班级列表。</span>
                            </div>
                            <div class="room-toolbar room-toolbar--compact">
                                <span class="room-muted">年级</span>
                                <asp:TextBox ID="TextBoxGrade" runat="server" CssClass="room-input"></asp:TextBox>
                                <span class="room-muted">班级</span>
                                <asp:TextBox ID="TextBoxClass" runat="server" CssClass="room-input"></asp:TextBox>
                            </div>
                            <asp:Button ID="BtncreateOne" runat="server" Text="添加该班级" onclick="BtncreateOne_Click" CssClass="room-btn room-btn--success" />
                        </div>
                    </div>
                    <asp:Label ID="Labelmsg" runat="server" CssClass="room-msg"></asp:Label>
                </div>
            </div>

            <div class="room-card">
                <div class="room-card__head">
                    <h2 class="room-card__title">全校班级列表</h2>
                    <p class="room-card__desc">创建完成后可在这里分页查看所有班级，并按需删除错误记录。</p>
                </div>
                <div class="room-grid-wrap">
                    <asp:GridView ID="GVclass" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CssClass="room-grid" GridLines="None" Width="100%"
                        onpageindexchanging="GVclass_PageIndexChanging"
                        onrowdatabound="GVclass_RowDataBound" PageSize="15" DataKeyNames="Rid"
                        onrowcommand="GVclass_RowCommand" EnableModelValidation="True" EmptyDataText="当前还没有班级，请先创建班级。">
                        <Columns>
                            <asp:BoundField HeaderText="序号"><ItemStyle Width="60px" CssClass="room-seq" /></asp:BoundField>
                            <asp:BoundField DataField="Rhid" HeaderText="教师" />
                            <asp:BoundField DataField="Rgrade" HeaderText="年级" />
                            <asp:BoundField DataField="Rclass" HeaderText="班级" />
                            <asp:ButtonField CommandName="Del" HeaderText="操作" Text="删除"><ItemStyle CssClass="room-del" /></asp:ButtonField>
                        </Columns>
                        <PagerTemplate>
                            <div class="room-pager">
                                <span>第 <asp:Label ID="lblPageIndex" runat="server" Text="<%# ((GridView)Container.Parent.Parent).PageIndex + 1 %>" CssClass="room-pager__current"></asp:Label> / <asp:Label ID="lblPageCount" runat="server" Text="<%# ((GridView)Container.Parent.Parent).PageCount %>"></asp:Label> 页</span>
                                <div class="room-pager__nav">
                                    <asp:LinkButton ID="btnFirst" runat="server" CausesValidation="False" CommandArgument="First" CommandName="Page" CssClass="room-pager__btn">首页</asp:LinkButton>
                                    <asp:LinkButton ID="btnPrev" runat="server" CausesValidation="False" CommandArgument="Prev" CommandName="Page" CssClass="room-pager__btn">上一页</asp:LinkButton>
                                    <asp:LinkButton ID="btnNext" runat="server" CausesValidation="False" CommandArgument="Next" CommandName="Page" CssClass="room-pager__btn">下一页</asp:LinkButton>
                                    <asp:LinkButton ID="btnLast" runat="server" CausesValidation="False" CommandArgument="Last" CommandName="Page" CssClass="room-pager__btn">尾页</asp:LinkButton>
                                </div>
                            </div>
                        </PagerTemplate>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
