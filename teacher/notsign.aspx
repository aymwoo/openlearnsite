<%@ Page Title="" Language="C#" StylesheetTheme="Teacher" AutoEventWireup="true"   CodeFile="notsign.aspx.cs" Inherits="Teacher_notsign" ResponseEncoding="utf-8" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8" />
<title></title>
    <link href="../App_Themes/Teacher/admin-form.css" rel="stylesheet" />
    <style type="text/css">
        .popup-page {
            --admin-form-page-bg: linear-gradient(180deg, #f8fafc 0%, #eef6ff 100%);
            --admin-form-hero-bg: linear-gradient(135deg, #0f766e 0%, #0f9b8e 55%, #22c55e 100%);
            --admin-form-hero-shadow: 0 22px 45px -28px rgba(15, 118, 110, 0.72);
            --admin-form-primary-bg: #0f766e;
            --admin-form-primary-hover: #0d675f;
            --admin-form-primary-shadow: 0 14px 24px -18px rgba(15, 118, 110, 0.85);
            --admin-form-focus: #14b8a6;
            --admin-form-focus-ring: rgba(20, 184, 166, 0.14);
        }
    .by{margin:0px}
    .phold{margin: auto; width:400px; text-align: center;font-size: 11pt;font-family: Arial;}
    .hearder{ background-color: #939CA2;height: 18px;text-align: center;line-height: 18px;}
    .reason-item{text-align: left; padding: 8px 15px; margin: 5px 0; background: #f5f5f5; border-radius: 5px;}
    .reason-item input{margin-right: 8px;}
    .reason-item label{cursor: pointer;}
    .score-warning{color: #ff4444; font-weight: bold; font-size: 12pt; margin: 10px 0;}
    .reason-section{margin: 15px 0; padding: 10px; background: #fff; border: 1px solid #ddd; border-radius: 8px;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div  class="phold" >
    <div  class="hearder"> 
        对<asp:Label ID="Labelname" runat="server" Font-Bold="True"></asp:Label>&nbsp;同学缺席备注
     </div>    
        <br />
        <div class="reason-section">
            <div style="font-weight: bold; margin-bottom: 10px; text-align: center;">缺席原因：</div>
            <div class="reason-item">
                <input type="radio" name="reasonRadio" id="reason1" value="未签到|-30" />
                <label for="reason1">未签到 <span style="color: #ff9800;">(-30分)</span></label>
            </div>
            <div class="reason-item">
                <input type="radio" name="reasonRadio" id="reason2" value="请假未到学校|0" />
                <label for="reason2">请假未到学校 <span style="color: #4caf50;">(不扣分)</span></label>
            </div>
            <div class="reason-item">
                <input type="radio" name="reasonRadio" id="reason3" value="被其他老师留到教室（办公室）|0" />
                <label for="reason3">被其他老师留到教室（办公室） <span style="color: #4caf50;">(不扣分)</span></label>
            </div>
            <div class="reason-item">
                <input type="radio" name="reasonRadio" id="reason4" value="旷课|-100" />
                <label for="reason4">旷课 <span style="color: #f44336;">(-100分)</span></label>
            </div>
            <div class="reason-item">
                <input type="radio" name="reasonRadio" id="reason5" value="其他" />
                <label for="reason5">其他（请填写说明）</label>
            </div>
        </div>
        
        <div style="text-align: left; margin: 10px 0;">
            详细说明：<br />
            <asp:TextBox ID="TextBox1" runat="server" Width="370px" Height="80px" 
            BackColor="#FFE7CE" TextMode="MultiLine"></asp:TextBox>
        </div>
        
        <asp:HiddenField ID="HFReason" runat="server" />
        <asp:HiddenField ID="HFScore" runat="server" />
        
    <asp:Label ID="Labelmsg" runat="server" ForeColor="Red"></asp:Label>
        <br />
    <asp:Button ID="Btnnotsign" runat="server"  Text="确定"  
        onclick="Btnnotsign_Click"  SkinID="BtnNormal"  />
</div>
</form>
    <script type="text/javascript">
        // 监听单选按钮变化
        document.querySelectorAll('input[name="reasonRadio"]').forEach(function(radio) {
            radio.addEventListener('change', function() {
                var value = this.value;
                if (value.indexOf('|') > -1) {
                    var parts = value.split('|');
                    document.getElementById('<%= HFReason.ClientID %>').value = parts[0];
                    document.getElementById('<%= HFScore.ClientID %>').value = parts[1];
                    
                    // 显示扣分提示
                    var score = parseInt(parts[1]);
                    var msgLabel = document.getElementById('<%= Labelmsg.ClientID %>');
                    if (score < 0) {
                        msgLabel.innerText = '⚠️ 将扣除 ' + Math.abs(score) + ' 分表现分';
                        msgLabel.style.color = '#ff9800';
                    } else {
                        msgLabel.innerText = '✅ 此原因不扣分';
                        msgLabel.style.color = '#4caf50';
                    }
                } else {
                    document.getElementById('<%= HFReason.ClientID %>').value = value;
                    document.getElementById('<%= HFScore.ClientID %>').value = '0';
                    document.getElementById('<%= Labelmsg.ClientID %>').innerText = '';
                }
            });
        });
        
        // 页面加载时恢复选中状态
        window.onload = function() {
            var savedReason = document.getElementById('<%= HFReason.ClientID %>').value;
            if (savedReason) {
                document.querySelectorAll('input[name="reasonRadio"]').forEach(function(radio) {
                    if (radio.value.indexOf('|') > -1 && radio.value.split('|')[0] === savedReason) {
                        radio.checked = true;
                    } else if (radio.value === savedReason) {
                        radio.checked = true;
                    }
                });
            }
        };
    </script>
</body>
</html>
