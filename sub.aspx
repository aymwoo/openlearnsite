<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sub.aspx.cs" Inherits="SubmitForm" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>信息科技教室课前检查表</title>
    <style>
        * {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
        }
        body {
            font-family: "Microsoft YaHei", Arial, sans-serif;
            background-color: #f5f5f5;
            padding: 20px;
        }
        .container {
            width: 100%;
            max-width: 600px;
            margin: 20px auto;
            background-color: white;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
            padding: 30px;
        }
        h1 {
            text-align: center;
            color: #333;
            margin-bottom: 20px;
            font-size: 24px;
        }
        .alert {
            background-color: #fff3cd;
            border: 1px solid #ffc107;
            border-radius: 4px;
            padding: 12px;
            margin: 15px 0;
            font-size: 14px;
        }
        fieldset {
            margin: 15px 0;
            padding: 15px;
            border: 1px solid #ddd;
            border-radius: 4px;
        }
        legend {
            font-weight: bold;
            color: #555;
            padding: 0 10px;
        }
        fieldset div {
            margin: 8px 0;
        }
        fieldset label {
            cursor: pointer;
            margin-left: 5px;
            user-select: none;
        }
        fieldset input[type="checkbox"],
        fieldset input[type="radio"] {
            cursor: pointer;
        }
        .btn-submit {
            background-color: #28a745;
            color: white;
            border: none;
            font-weight: bold;
            font-size: 18px;
            height: 45px;
            width: 120px;
            border-radius: 4px;
            cursor: pointer;
            transition: background-color 0.3s;
        }
        .btn-submit:hover {
            background-color: #218838;
        }
        .btn-submit:active {
            background-color: #1e7e34;
        }
        .button-group {
            display: flex;
            gap: 10px;
            justify-content: center;
            margin-top: 20px;
        }
        .button-group input[type="button"],
        .button-group input[type="submit"] {
            padding: 10px 20px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
        }
        @media (max-width: 768px) {
            .container {
                padding: 20px;
                margin: 10px auto;
            }
            .button-group {
                flex-direction: column;
            }
            .button-group input[type="button"],
            .button-group input[type="submit"] {
                width: 100%;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>信息科技教室课前检查表</h1>

            <!-- 基础信息 -->
            <div style="background-color: #e3f2fd; padding: 15px; border-radius: 4px; margin-bottom: 15px;">
                <div style="color: #0d47a1; font-weight: bold; margin-bottom: 5px;">请核对你的信息是否正确！</div>
                <div>
                    <asp:Label ID="lblPcName" runat="server" Text="机号：" Font-Bold="true"></asp:Label>
                    <asp:Label ID="lblPcNameValue" runat="server" Font-Bold="true" ForeColor="#0d47a1"></asp:Label>
                </div>
                <asp:Label ID="lblIpAddress" runat="server" Text="IP地址：" Visible="false"></asp:Label>
                <asp:Label ID="lblIpAddressValue" runat="server" Visible="false"></asp:Label>
            </div>

            <!-- 隐藏字段 -->
            <asp:Label ID="sid" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="snum" runat="server" Visible="false"></asp:Label>

            <!-- 当前学生信息（登录学生） -->
            <fieldset>
                <legend>📋 当前登录学生</legend>
                <div style="font-size: 16px;">
                    <asp:Label ID="sclass" runat="server" Font-Bold="true" ForeColor="#28a745"></asp:Label>
                    <asp:Label ID="sname" runat="server" Font-Bold="true" ForeColor="#28a745"></asp:Label>
                    <span style="color: #007bff; margin-left: 10px; font-weight: bold;">（我）</span>
                </div>
            </fieldset>

            <!-- 上一次使用记录 -->
            <fieldset style="border-color: #dc3545; background-color: #fff5f5;">
                <legend style="color: #dc3545;">⚠️ 上一次使用本机的学生（请重点检查！）</legend>
                <div>
                    <asp:Label ID="lnameinfo" runat="server" Text="上一次使用本机的同学是：" Font-Bold="true"></asp:Label>
                    <asp:Label ID="lname" runat="server" Font-Bold="true" Font-Size="16px"></asp:Label>
                </div>
                <div style="color: #ff6600; margin-top: 10px; padding: 10px; background-color: #fff3cd; border-radius: 4px;">
                    🔍 请仔细检查该同学是否留下了垃圾、设备是否正常
                </div>
            </fieldset>

            <!-- 警示信息 -->
            <div class="alert">
                ⚠️ <strong>重要提示：</strong>请如实填报检查项，避免遗漏或误填。故意乱填的，视为严重违规，将取消信息课资格并记入考评。
            </div>

            <!-- 检查项分组 -->
            <fieldset>
                <legend>🧹 环境卫生检查</legend>
                <asp:CheckBoxList ID="cblHygiene" runat="server" CellPadding="5" CellSpacing="5">
                    <asp:ListItem Text="包干区（桌面、地面、抽屉等）有垃圾" Value="HasRubbish"></asp:ListItem>
                    <asp:ListItem Text="地面有污渍" Value="DrawerClean"></asp:ListItem>
                </asp:CheckBoxList>
            </fieldset>

            <fieldset>
                <legend>🪑 行为习惯检查</legend>
                <asp:CheckBoxList ID="cblEquipmentArrangement" runat="server" CellPadding="5" CellSpacing="5">
                    <asp:ListItem Text="键盘鼠标未摆放整齐" Value="EquipmentArranged"></asp:ListItem>
                    <asp:ListItem Text="椅子未归位" Value="ChairAdjusted"></asp:ListItem>
                </asp:CheckBoxList>
            </fieldset>

            <fieldset>
                <legend>💻 设备异常检查</legend>
                <asp:CheckBoxList ID="cblDeviceIssues" runat="server" CellPadding="5" CellSpacing="5">
                    <asp:ListItem Text="键盘或鼠标损坏" Value="KeyboardMouseDamaged"></asp:ListItem>
                    <asp:ListItem Text="电源线或网线被拔掉" Value="CableUnplugged"></asp:ListItem>
                    <asp:ListItem Text="鼠标键盘被拔掉" Value="PeripheralUnplugged"></asp:ListItem>
                    <asp:ListItem Text="乱涂乱画屏幕、桌面、键盘、鼠标" Value="ScreenMarked"></asp:ListItem>
                </asp:CheckBoxList>
            </fieldset>

            <!-- 正常选项 -->
            <fieldset style="background-color: #f0fff4;">
                <legend style="color: #28a745;">✅ 请仔细检查，确认是否正常</legend>
                <div>
                    <asp:RadioButton ID="rbNormal" runat="server" Text="一切正常" Checked="true" GroupName="StatusGroup" />
                </div>
            </fieldset>

            <script type="text/javascript">
                function updateNormalStatus() {
                    var rbNormal = document.getElementById('<%= rbNormal.ClientID %>');
                    var anyChecked = false;

                    // 检查所有CheckBoxList
                    var checkBoxLists = document.querySelectorAll('[id*="cblHygiene"], [id*="cblEquipmentArrangement"], [id*="cblDeviceIssues"]');
                    checkBoxLists.forEach(function(list) {
                        var checkboxes = list.getElementsByTagName('input');
                        for (var i = 0; i < checkboxes.length; i++) {
                            if (checkboxes[i].checked) {
                                anyChecked = true;
                                break;
                            }
                        }
                    });

                    if (anyChecked) {
                        rbNormal.checked = false;
                    }
                }

                // 为所有复选框添加点击事件
                document.addEventListener('DOMContentLoaded', function() {
                    var checkboxes = document.querySelectorAll('[id*="cblHygiene"] input, [id*="cblEquipmentArrangement"] input, [id*="cblDeviceIssues"] input');
                    checkboxes.forEach(function(checkbox) {
                        checkbox.addEventListener('click', updateNormalStatus);
                    });
                });
            </script>

            <!-- 操作按钮 -->
            <div class="button-group">
                <asp:Button ID="btnSubmit" runat="server" Text="提交检查"
                    OnClick="btnSubmit_Click" CssClass="btn-submit" />
                <asp:Button ID="btnjump" runat="server" Text="已提交，跳过"
                    OnClick="btnjump_Click" BackColor="#6c757d" ForeColor="White"
                    Style="border:none; padding:10px 20px; border-radius:4px; cursor:pointer;" />
                <asp:Button ID="BtnExit" runat="server" Text="重新登录"
                    OnClick="BtnExit_Click" BackColor="#dc3545" ForeColor="White"
                    Style="border:none; padding:10px 20px; border-radius:4px; cursor:pointer;" />
            </div>
        </div>
    </form>
</body>
</html>