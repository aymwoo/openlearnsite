<%@ Page Title="" Language="C#" MasterPageFile="~/student/Stud.master" StylesheetTheme="Student"
    AutoEventWireup="true" CodeFile="downfile.aspx.cs" Inherits="Student_downfile" ResponseEncoding="utf-8" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Cphs" runat="Server">
    <div class="grid grid-cols-1 lg:grid-cols-4 gap-6 lg:gap-8 w-full max-w-full">
        <!-- Main Content (Left Column) -->
        <div class="lg:col-span-3 space-y-6 overflow-hidden min-w-0">
            <div class="bg-white rounded-2xl shadow-sm border border-slate-200/60 p-6 sm:p-10">
                <div class="text-center pb-6 border-b border-slate-100">
                    <asp:Label ID="Labeltitle" runat="server" CssClass="text-2xl sm:text-3xl font-extrabold text-slate-800 tracking-tight"></asp:Label>
                </div>
                
                <div class="flex flex-wrap gap-4 items-center justify-center py-4 bg-slate-50 mt-4 rounded-xl border border-slate-100 text-sm text-slate-600 shadow-inner">
                    <div class="flex items-center gap-1.5"><span class="font-bold text-slate-500 text-xs uppercase tracking-wider">属性</span> <asp:Label ID="Labelclass" runat="server" CssClass="font-medium text-slate-800"></asp:Label></div>
                    <div class="w-px h-4 bg-slate-300 hidden sm:block"></div>
                    <div class="flex items-center gap-1.5"><span class="font-bold text-slate-500 text-xs uppercase tracking-wider">格式</span> <asp:Image ID="ImageType" runat="server" CssClass="w-4 h-4 inline-block" /> <asp:Label ID="Labelfiletype" runat="server" CssClass="font-medium text-slate-800"></asp:Label></div>
                    <div class="w-px h-4 bg-slate-300 hidden sm:block"></div>
                    <div class="flex items-center gap-1.5"><span class="font-bold text-slate-500 text-xs uppercase tracking-wider">点击率</span> <asp:Label ID="Labelhit" runat="server" CssClass="font-medium text-emerald-600"></asp:Label></div>
                    <div class="w-px h-4 bg-slate-300 hidden sm:block"></div>
                    <div class="flex items-center gap-1.5"><span class="font-bold text-slate-500 text-xs uppercase tracking-wider">更新日期</span> <asp:Label ID="Labeldate" runat="server" CssClass="font-medium text-slate-800"></asp:Label></div>
                    <div class="w-px h-4 bg-slate-300 hidden sm:block"></div>
                    <div class="flex items-center gap-1.5"><span class="font-bold text-slate-500 text-xs uppercase tracking-wider">学分</span> <asp:Label ID="Labelopen" runat="server" CssClass="font-bold text-orange-500"></asp:Label></div>
                    
                    <asp:Label ID="LabelFyid" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="LabelFid" runat="server" Visible="False"></asp:Label>
                    <asp:Label ID="LabelSid" runat="server" Visible="False"></asp:Label>
                </div>
                
                <div class="mt-8 text-slate-700 leading-loose text-[1.1rem]">
    <script type="text/javascript">
        // 页面加载后检查是否有游戏链接需要跳转
        window.onload = function() {
            var gameUrl = localStorage.getItem('gameUrl');
            if (gameUrl) {
                // 清除 localStorage 中的游戏链接
                localStorage.removeItem('gameUrl');
                // 跳转到游戏链接
                window.location.href = gameUrl;
            }
        };

        function accessResource(fid) {
            // 先获取资源信息，了解需要扣除的学分
            var infoXhr = new XMLHttpRequest();
            infoXhr.open('POST', '../api/ScoreProxy.ashx?action=getinfo', true);
            infoXhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
            infoXhr.onreadystatechange = function() {
                if (infoXhr.readyState === 4) {
                    if (infoXhr.status === 200) {
                        try {
                            var infoResult = JSON.parse(infoXhr.responseText);
                            if (infoResult.code === 1) {
                                // 检查是否已经扣除过该资源的学分
                                if (infoResult.hasDeducted) {
                                    alert('已经扣除过该资源的学分，无法重复扣除');
                                    return;
                                }
                                
                                // 显示扣除确认
                                if (confirm('访问此资源需要扣除 ' + infoResult.score + ' 学分，你当前的学分为 ' + infoResult.currentScore + ' 分，是否继续？')) {
                                    // 扣除学分
                                    var deductXhr = new XMLHttpRequest();
                                    deductXhr.open('POST', '../api/ScoreProxy.ashx?action=deduct', true);
                                    deductXhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                                    deductXhr.onreadystatechange = function() {
                                        if (deductXhr.readyState === 4) {
                                            if (deductXhr.status === 200) {
                                                try {
                                                    var deductResult = JSON.parse(deductXhr.responseText);
                                                    if (deductResult.code === 1) {
                                                        // 学分扣除成功，继续访问资源
                                                        var resourceXhr = new XMLHttpRequest();
                                                        resourceXhr.open('POST', '../api/ResourceProxy.ashx?action=geturl&fid=' + fid, true);
                                                        resourceXhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
                                                        resourceXhr.onreadystatechange = function() {
                                                            if (resourceXhr.readyState === 4) {
                                                                if (resourceXhr.status === 200) {
                                                                    try {
                                                                        var result = JSON.parse(resourceXhr.responseText);
                                                                        if (result.code === 1) {
                                                                            // 保存游戏链接到 localStorage
                                                                            localStorage.setItem('gameUrl', result.url);
                                                                            // 刷新页面，更新学分显示
                                                                            location.reload();
                                                                        } else {
                                                                            alert(result.msg || '无法访问该资源');
                                                                        }
                                                                    } catch (e) {
                                                                        alert('访问失败，请重试');
                                                                    }
                                                                } else {
                                                                    alert('网络错误，请重试');
                                                                }
                                                            }
                                                        };
                                                        resourceXhr.send();
                                                    } else {
                                                        alert(deductResult.msg || '扣除学分失败');
                                                    }
                                                } catch (e) {
                                                    alert('扣除学分失败，请重试');
                                                }
                                            } else {
                                                alert('网络错误，无法扣除学分');
                                            }
                                        }
                                    };
                                    deductXhr.send('fid=' + fid);
                                }
                            } else {
                                alert(infoResult.msg || '获取资源信息失败');
                            }
                        } catch (e) {
                            alert('获取资源信息失败，请重试');
                        }
                    } else {
                        alert('网络错误，无法获取资源信息');
                    }
                }
            };
            infoXhr.send('fid=' + fid);
        }

        function openSecureLink(element) {
            var linkUrl = element.getAttribute('data-link');
            var fid = element.getAttribute('data-fid');

            if (!linkUrl) {
                alert('链接无效');
                return;
            }

            var xhr = new XMLHttpRequest();
            xhr.open('POST', '../api/LinkProxy.ashx?action=getlink', true);
            xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
            xhr.onreadystatechange = function() {
                if (xhr.readyState === 4) {
                    if (xhr.status === 200) {
                        try {
                            var result = JSON.parse(xhr.responseText);
                            if (result.code === 1) {
                                window.location.href = result.url;
                            } else {
                                alert(result.msg || '无法访问该链接');
                            }
                        } catch (e) {
                            window.location.href = linkUrl;
                        }
                    } else {
                        alert('网络错误，请重试');
                    }
                }
            };
            xhr.send('url=' + encodeURIComponent(linkUrl) + '&fid=' + (fid || ''));
        }

        (function() {
            document.addEventListener('contextmenu', function(e) {
                var target = e.target;
                while (target) {
                    if (target.tagName === 'A' && target.hasAttribute('data-link')) {
                        e.preventDefault();
                        alert('此链接受保护，无法右键复制！');
                        return false;
                    }
                    target = target.parentElement;
                }
            });

            document.addEventListener('keydown', function(e) {
                if (e.ctrlKey && e.key === 'u') {
                    e.preventDefault();
                    alert('此页面禁止查看源代码！');
                    return false;
                }
                if (e.key === 'F12') {
                    e.preventDefault();
                    alert('此页面禁止使用开发者工具！');
                    return false;
                }
            });

            document.addEventListener('copy', function(e) {
                var selection = window.getSelection();
                var container = selection.anchorNode;
                while (container && container !== document) {
                    if (container.nodeType === 1) {
                        var links = container.getElementsByTagName('a');
                        for (var i = 0; i < links.length; i++) {
                            if (links[i].hasAttribute('data-link')) {
                                e.preventDefault();
                                alert('此内容受保护，无法复制！');
                                return false;
                            }
                        }
                    }
                    container = container.parentNode;
                }
            });
        })();
    </script>
    <div id="student">
        <div class="left">
            <br />
            <asp:Label ID="Labeltitle" runat="server" SkinID="LabelLightBlue" Width="98%" CssClass="txts24center"
                Height="24px"></asp:Label>
            <br />
            <div style="padding: 2px; margin: auto; border-bottom-style: dashed; border-width: 1px;
                border-color: #CCCCCC">
                属性：<asp:Label ID="Labelclass" runat="server" SkinID="LabelFileShow"></asp:Label>
                格式：<asp:Image ID="ImageType" runat="server" />
                <asp:Label ID="Labelfiletype" runat="server" SkinID="LabelFileShow"></asp:Label>
                点击率：<asp:Label ID="Labelhit" runat="server" SkinID="LabelFileShow"></asp:Label>
                更新日期：<asp:Label ID="Labeldate" runat="server" SkinID="LabelFileShow"></asp:Label>
                评分方式：<asp:Label ID="Labelopen" runat="server" SkinID="LabelFileShow"></asp:Label>
                <asp:Label ID="LabelFyid" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="LabelFid" runat="server" Visible="False"></asp:Label>
                <asp:Label ID="LabelSid" runat="server" Visible="False"></asp:Label>
            </div>
            <center>
                <div>
                    <br />
                    <div class="downcontent">
                        <asp:Literal ID="Labelcontent" runat="server"></asp:Literal>
                    </div>
                </div>
            </center>
            <br />
            <asp:Label ID="Labelmsg" runat="server"></asp:Label>
            <br />
            <asp:Image ID="ImageDown" runat="server" ImageUrl="~/images/down1.gif" />
            <asp:LinkButton ID="LBtnfile" runat="server" OnClick="LBtnfile_Click" Visible="False"
                Font-Underline="False" BorderColor="#7DBF80" BorderStyle="Dashed" BorderWidth="1px"
                CssClass="txtszcenter" Height="18px" BackColor="#E2F3E3" Width="80px">点击下载</asp:LinkButton>
            <br />
            <asp:HyperLink ID="HLurl" runat="server"></asp:HyperLink>
            <br />
        </div>
        <div class="right">
            <div style="width: 170px">
                <br />
                <asp:GridView ID="GVSoft" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    OnPageIndexChanging="GVSoft_PageIndexChanging" OnRowDataBound="GVSoft_RowDataBound"
                    Width="98%" SkinID="GridViewInfo" EnableModelValidation="True" CellPadding="4">
                    <AlternatingRowStyle BackColor="#E9EFF5" />
                    <Columns>
                        <asp:TemplateField HeaderText="标题">
                            <ItemTemplate>
                                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("fid", "downfile.aspx?Fid={0}") %>'
                                    Text='<%# strcut( Eval("Ftitle").ToString()) %>' ToolTip='<%# Eval("Ftitle")%>'>
                                </asp:HyperLink>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" Width="280px" />
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle HorizontalAlign="Center" />
                    <HeaderStyle Height="24px" BackColor="#DEE7F1" BorderColor="LightSteelBlue" BorderStyle="Solid"
                        BorderWidth="1px" />
                    <PagerStyle HorizontalAlign="Center" Font-Size="9pt" />
                    <PagerTemplate>
                        <div>
                            <asp:LinkButton ID="btnFirst" runat="server" CausesValidation="False" CommandArgument="First"
                                CommandName="Page" Font-Underline="False" ForeColor="Black" Text="首页" />&nbsp;
                            <asp:LinkButton ID="btnPrev" runat="server" CausesValidation="False" CommandArgument="Prev"
                                CommandName="Page" Font-Underline="False" ForeColor="Black" Text="上页" />&nbsp;
                            <asp:LinkButton ID="btnNext" runat="server" CausesValidation="False" CommandArgument="Next"
                                CommandName="Page" Font-Underline="False" ForeColor="Black" Text="下页" />&nbsp;
                            <asp:LinkButton ID="btnLast" runat="server" CausesValidation="False" CommandArgument="Last"
                                CommandName="Page" Font-Underline="False" ForeColor="Black" Text="尾页" />
                        </div>
                    </PagerTemplate>
                    <RowStyle Height="30px" />
                </asp:GridView>
                <br />
                <asp:Image runat="server" ID="upFileType" Visible="False" />
                <asp:HyperLink ID="upFileUrl" runat="server" Height="16px" Visible="False" Target="_blank">[upFileUrl]</asp:HyperLink>
                <br />
                <br />
                <asp:Panel ID="Panelswfupload" runat="server">
                    <link href="../kindeditor/themes/me/me.css" rel="stylesheet" type="text/css" />
                    <script type="text/javascript" charset="utf-8" src="../kindeditor/kindeditor-min.js"></script>
                    <script type="text/javascript" charset="utf-8" src="../kindeditor/lang/zh_CN.js"></script>
                    <div id="swfu_container" style="margin: 0px 30px;">
                        <div style="text-align: center; margin: auto">
                            <script type="text/javascript">
                                KindEditor.ready(function (K) {
                                    var uploadbutton = K.uploadbutton({
                                        button: K('#uploadButton')[0],
                                        fieldName: 'imgFile',
                                        url: 'autoupload.aspx?yid=<%=LabelFyid.Text %>&fid=<%=LabelFid.Text %>&sid=<%=LabelSid.Text %>',
                                        afterUpload: function (data) {
                                            if (data.error == 0) {
                                                alert(data.message);
                                                location.reload();
                                            } else {
                                                alert(data.message);
                                            }
                                        },
                                        afterError: function (str) {
                                            alert('出错信息: ' + str);
                                        }
                                    });
                                    uploadbutton.fileBox.change(function (e) {
                                        uploadbutton.submit();
                                    });
                                });
                            </script>                            
                                <input type="button" id="uploadButton" value="作品保存" />                   
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- Sidebar (Right Column) -->
        <div class="lg:col-span-1 space-y-6 self-start top-24 sticky">
            <div class="bg-slate-50 border border-slate-200 rounded-2xl p-5 shadow-sm overflow-hidden flex flex-col items-center">
                <h4 class="w-full text-slate-700 font-bold mb-3 flex items-center gap-2 border-b border-slate-200 pb-2">
                    <svg class="w-5 h-5 text-slate-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 012-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10"></path></svg>
                    相关软件下载
                </h4>
                
                <div class="w-full overflow-x-auto min-w-0 mb-4 rounded border border-slate-200">
                    <asp:GridView ID="GVSoft" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        OnPageIndexChanging="GVSoft_PageIndexChanging" OnRowDataBound="GVSoft_RowDataBound"
                        Width="100%" SkinID="GridViewInfo" EnableModelValidation="True" CellPadding="4"
                        CssClass="w-full text-xs text-slate-700 min-w-min">
                        <AlternatingRowStyle BackColor="#f8fafc" />
                        <Columns>
                            <asp:TemplateField HeaderText="标题">
                                <ItemTemplate>
                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# Eval("fid", "downfile.aspx?Fid={0}") %>'
                                        Text='<%# strcut( Eval("Ftitle").ToString()) %>' ToolTip='<%# Eval("Ftitle")%>'
                                        CssClass="font-medium text-slate-800 hover:text-indigo-600 block truncate max-w-[200px]">
                                    </asp:HyperLink>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" CssClass="py-2 px-3 bg-slate-100 text-slate-600 font-semibold" />
                                <ItemStyle HorizontalAlign="Left" CssClass="py-2 px-3 border-b border-slate-100" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle HorizontalAlign="Center" CssClass="bg-slate-50 py-2 border-t border-slate-200" />
                        <PagerTemplate>
                            <div class="flex gap-2 justify-center py-1">
                                <asp:LinkButton ID="btnFirst" runat="server" CausesValidation="False" CommandArgument="First"
                                    CommandName="Page" CssClass="px-2 py-1 text-[10px] border border-slate-300 rounded hover:bg-slate-200" Text="首页" />
                                <asp:LinkButton ID="btnPrev" runat="server" CausesValidation="False" CommandArgument="Prev"
                                    CommandName="Page" CssClass="px-2 py-1 text-[10px] border border-slate-300 rounded hover:bg-slate-200" Text="上页" />
                                <asp:LinkButton ID="btnNext" runat="server" CausesValidation="False" CommandArgument="Next"
                                    CommandName="Page" CssClass="px-2 py-1 text-[10px] border border-slate-300 rounded hover:bg-slate-200" Text="下页" />
                                <asp:LinkButton ID="btnLast" runat="server" CausesValidation="False" CommandArgument="Last"
                                    CommandName="Page" CssClass="px-2 py-1 text-[10px] border border-slate-300 rounded hover:bg-slate-200" Text="尾页" />
                            </div>
                        </PagerTemplate>
                        <RowStyle CssClass="hover:bg-slate-50 transition" />
                    </asp:GridView>
                </div>
                
                <div class="w-full flex flex-col items-center gap-3">
                    <asp:Image runat="server" ID="upFileType" Visible="False" CssClass="w-8 h-8 object-contain" />
                    <asp:HyperLink ID="upFileUrl" runat="server" Visible="False" Target="_blank" CssClass="px-4 py-2 bg-blue-50 text-blue-600 border border-blue-200 font-bold rounded-lg hover:bg-blue-100 transition duration-300 shadow-sm text-center w-full truncate">[upFileUrl]</asp:HyperLink>
                    
                    <asp:Panel ID="Panelswfupload" runat="server" CssClass="w-full">
                        <link href="../kindeditor/themes/me/me.css" rel="stylesheet" type="text/css" />
                        <script type="text/javascript" charset="utf-8" src="../kindeditor/kindeditor-min.js"></script>
                        <script type="text/javascript" charset="utf-8" src="../kindeditor/lang/zh_CN.js"></script>
                        <div id="swfu_container" class="w-full flex justify-center mt-2">
                                <script type="text/javascript">
                                    KindEditor.ready(function (K) {
                                        var uploadbutton = K.uploadbutton({
                                            button: K('#uploadButton')[0],
                                            fieldName: 'imgFile',
                                            url: 'autoupload.aspx?yid=<%=LabelFyid.Text %>&fid=<%=LabelFid.Text %>&sid=<%=LabelSid.Text %>',
                                            afterUpload: function (data) {
                                                if (data.error == 0) {
                                                    alert(data.message);
                                                    location.reload();
                                                } else {
                                                    alert(data.message);
                                                }
                                            },
                                            afterError: function (str) {
                                                alert('出错信息: ' + str);
                                            }
                                        });
                                        uploadbutton.fileBox.change(function (e) {
                                            uploadbutton.submit();
                                        });
                                    });
                                </script>                            
                                <input type="button" id="uploadButton" value="作品保存"  class="px-4 py-2 bg-emerald-500 text-white rounded hover:bg-emerald-600 transition duration-300 shadow-md border-0 w-full" />                   
                        </div>
                    </asp:Panel>
                    
                    <asp:HyperLink ID="Hltonomic" runat="server" ImageUrl="~/images/nomic.gif" NavigateUrl="~/student/autonomic.aspx"
                        Target="_blank" BorderStyle="None" CssClass="mt-4 transform hover:scale-105 transition duration-300 drop-shadow-md rounded-xl overflow-hidden block w-full flex justify-center"></asp:HyperLink>
                </div>
            </div>
            
            <div class="hidden">
                <link href="../js/tinybox.css" rel="stylesheet" type="text/css" />
                <script src="../js/tinybox.js" type="text/javascript"></script>
                <script type="text/javascript">
                    function showShare() {
                        var urlat = "../student/groupshare.aspx";
                        TINY.box.show({ iframe: urlat, boxid: 'frameless', width: 600, height: 400, fixed: false, maskopacity: 60, close: true })
                    }   
                </script>
            </div>
        </div>
    </div>
</asp:Content>
