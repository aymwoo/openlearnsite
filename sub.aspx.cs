using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;

public partial class SubmitForm : System.Web.UI.Page
{
    private LearnSite.Model.Cook cook = new LearnSite.Model.Cook();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                InitializePageData();
                LoadStudentInfo();  // 先加载学生信息
                BindLastUserData(); // 再绑定上一次使用记录
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Page_Load错误: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("堆栈跟踪: " + ex.StackTrace);
            }
        }
        CheckLoginStatus();
    }

    // 初始化基础数据
    private void InitializePageData()
    {
        string ip = LearnSite.Common.Computer.MyIp();
        lblIpAddressValue.Text = ip;
        lblPcNameValue.Text = GetHostName(ip);
    }

    // 加载学生信息
    private void LoadStudentInfo()
    {
        if (!LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            Response.Redirect("~/index.aspx");
            return;
        }

        snum.Text = cook.Snum;
        sid.Text = cook.Sid.ToString();
        sname.Text = Server.UrlDecode(cook.Sname);
        sclass.Text = string.Format("{0}年级{1}班", cook.Sgrade, cook.Sclass);
    }

    // 获取主机名
    private string GetHostName(string ip)
    {
        LearnSite.BLL.Computers cbll = new LearnSite.BLL.Computers();
        LearnSite.Model.Computers cmodel = cbll.GetModelByIp(ip);
        return (cmodel != null) ? cmodel.Pmachine : "未知主机";
    }

    // 绑定上一次使用记录
    private void BindLastUserData()
    {
        string ip = lblIpAddressValue.Text;
        int currentSid = 0;

        // 安全地获取当前学生ID
        if (!int.TryParse(sid.Text, out currentSid))
        {
            lname.Text = "学生信息异常，无法查询上机记录";
            lname.ForeColor = System.Drawing.Color.Orange;
            return;
        }

        try
        {
            bool foundRecord = false;

            // 使用单个连接执行查询
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
            {
                conn.Open();

                // 优先从CheckRecords表查询该电脑的上一次检查记录
                using (SqlCommand cmd = new SqlCommand(
                    @"SELECT TOP 1 xuehao, sname, ClassName FROM CheckRecords
                      WHERE IpAddress=@Ip AND snid<>@CurrentSid
                      ORDER BY Id DESC", conn))
                {
                    cmd.Parameters.AddWithValue("@Ip", ip);
                    cmd.Parameters.AddWithValue("@CurrentSid", currentSid);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lname.Text = string.Format("{0}({1})班 {2}",
                                reader["xuehao"],
                                reader["ClassName"],
                                reader["sname"]);
                            lname.ForeColor = System.Drawing.Color.Red;
                            foundRecord = true;
                        }
                    }
                }

                // 如果CheckRecords表没有记录，从Signin表查询
                if (!foundRecord)
                {
                    using (SqlCommand cmd = new SqlCommand(
                        @"SELECT TOP 1 qgrade, qclass, qname FROM Signin
                          WHERE qip=@Ip AND (qsid IS NULL OR qsid<>@CurrentSid)
                          ORDER BY qdate DESC", conn))
                    {
                        cmd.Parameters.AddWithValue("@Ip", ip);
                        cmd.Parameters.AddWithValue("@CurrentSid", currentSid);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lname.Text = string.Format("{0}({1})班 {2}",
                                    reader["qgrade"],
                                    reader["qclass"],
                                    reader["qname"]);
                                lname.ForeColor = System.Drawing.Color.Red;
                                foundRecord = true;
                            }
                        }
                    }
                }
            }

            // 如果都没有记录（第一次使用该电脑）
            if (!foundRecord)
            {
                lname.Text = "本机首次使用，无上机记录";
                lname.ForeColor = System.Drawing.Color.Green;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("BindLastUserData错误: " + ex.Message);
            lname.Text = "查询上机记录时发生错误";
            lname.ForeColor = System.Drawing.Color.Orange;
        }
    }

    // 表单提交
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (rbNormal.Checked && !HasAnyIssues())
        {
            // 选择"正常"且没有勾选任何问题项，直接跳转
            SafeRedirect();
        }
        else if (SaveFormData())
        {
            SetSubmissionCookie();
            SafeRedirect();
        }
        else
        {
            Response.Write("<script>alert('提交失败，请重试！')</script>");
        }
    }

    // 检查是否有任何问题项被选中
    private bool HasAnyIssues()
    {
        return CheckListSelected(cblHygiene) ||
               CheckListSelected(cblEquipmentArrangement) ||
               CheckListSelected(cblDeviceIssues);
    }

    // 检查CheckBoxList是否至少选择一项
    private bool CheckListSelected(CheckBoxList list)
    {
        foreach (ListItem item in list.Items)
        {
            if (item.Selected) return true;
        }
        return false;
    }

    // 保存数据
    private bool SaveFormData()
    {
        // 验证必要字段
        if (string.IsNullOrEmpty(sclass.Text) || string.IsNullOrEmpty(sname.Text) || string.IsNullOrEmpty(snum.Text))
        {
            System.Diagnostics.Debug.WriteLine("保存失败：学生信息不完整");
            Response.Write("<script>alert('学生信息不完整，请重新登录！')</script>");
            return false;
        }

        string sql = @"INSERT INTO CheckRecords (
            PcName, IpAddress, ClassName,
            HasRubbish, DrawerClean, EquipmentArranged, ChairAdjusted,
            KeyboardMouseDamaged, CableUnplugged, PeripheralUnplugged, ScreenMarked,
            snid, xuehao, sname, Suser, SubmitTime
        ) VALUES (
            @PcName, @Ip, @Class,
            @Rubbish, @Drawer, @Equipment, @Chair,
            @KeyboardDamaged, @CableUnplugged, @PeripheralUnplugged, @ScreenMarked,
            @snid, @xuehao, @sname, @lname, GETDATE()
        )";

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            AddParameters(cmd);

            try
            {
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine("保存成功，影响行数: " + rowsAffected);
                return true;
            }
            catch (SqlException sqlEx)
            {
                // 记录详细的 SQL 错误信息
                System.Diagnostics.Debug.WriteLine("SQL错误: " + sqlEx.Message);
                System.Diagnostics.Debug.WriteLine("错误号: " + sqlEx.Number);
                System.Diagnostics.Debug.WriteLine("SQL: " + sql);

                string errorMsg = "提交失败";
                if (sqlEx.Number == 208) // 表或视图不存在
                {
                    errorMsg = "系统表不存在，请联系管理员";
                }
                else if (sqlEx.Number == 207) // 列名无效
                {
                    errorMsg = "数据库结构异常，请联系管理员";
                }
                else if (sqlEx.Number == 547) // 外键约束
                {
                    errorMsg = "数据关联错误，请重试";
                }

                Response.Write("<script>alert('" + errorMsg + "！')</script>");
                return false;
            }
            catch (Exception ex)
            {
                // 记录其他错误
                System.Diagnostics.Debug.WriteLine("保存检查记录失败: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("SQL: " + sql);
                Response.Write("<script>alert('提交失败，请重试！')</script>");
                return false;
            }
        }
    }

    // 添加参数
    private void AddParameters(SqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@PcName", lblPcNameValue.Text);
        cmd.Parameters.AddWithValue("@Ip", lblIpAddressValue.Text);
        cmd.Parameters.AddWithValue("@Class", sclass.Text);

        // 绑定检查项参数
        cmd.Parameters.AddWithValue("@Rubbish", cblHygiene.Items[0].Selected);
        cmd.Parameters.AddWithValue("@Drawer", cblHygiene.Items[1].Selected);
        cmd.Parameters.AddWithValue("@Equipment", cblEquipmentArrangement.Items[0].Selected);
        cmd.Parameters.AddWithValue("@Chair", cblEquipmentArrangement.Items[1].Selected);
        cmd.Parameters.AddWithValue("@KeyboardDamaged", cblDeviceIssues.Items[0].Selected);
        cmd.Parameters.AddWithValue("@CableUnplugged", cblDeviceIssues.Items[1].Selected);
        cmd.Parameters.AddWithValue("@PeripheralUnplugged", cblDeviceIssues.Items[2].Selected);
        cmd.Parameters.AddWithValue("@ScreenMarked", cblDeviceIssues.Items[3].Selected);

        // 学生信息参数
        cmd.Parameters.AddWithValue("@snid", sid.Text);
        cmd.Parameters.AddWithValue("@xuehao", snum.Text);
        cmd.Parameters.AddWithValue("@sname", sname.Text);

        // 处理上一次使用的学生信息：如果是"本机首次使用，无上机记录"，则保存空字符串
        string lastUser = lname.Text;
        if (lastUser.Contains("本机首次使用") || lastUser.Contains("无上机记录"))
        {
            lastUser = "";
        }
        cmd.Parameters.AddWithValue("@lname", lastUser);
    }

    // 安全跳转
    private void SafeRedirect()
    {
        string targetUrl = GetSafeRedirectUrl();
        Response.Redirect(string.IsNullOrEmpty(targetUrl) ? 
            "~/student/myinfo.aspx" : targetUrl);
    }

    // 获取安全URL
    private string GetSafeRedirectUrl()
    {
        string encodedUrl = Request.QueryString["url"];
        if (string.IsNullOrEmpty(encodedUrl)) return null;

        string decodedUrl = HttpUtility.UrlDecode(encodedUrl);
        return IsValidLocalUrl(decodedUrl) ? decodedUrl : null;
    }

    // 验证本地URL
    private bool IsValidLocalUrl(string url)
    {
        if (Uri.IsWellFormedUriString(url, UriKind.Relative)) return true;
        
        Uri absoluteUri;
        if (Uri.TryCreate(url, UriKind.Absolute, out absoluteUri))
        {
            return string.Equals(
                absoluteUri.Authority, 
                Request.Url.Authority, 
                StringComparison.OrdinalIgnoreCase
            );
        }
        return false;
    }

    // 设置提交Cookie
    private void SetSubmissionCookie()
    {
        HttpCookie cookie = new HttpCookie("HasSubmittedForm", DateTime.Now.ToString());
        cookie.Expires = DateTime.Now.AddDays(1);
        Response.Cookies.Add(cookie);
    }

    // 其他按钮事件
    protected void btnjump_Click(object sender, EventArgs e)
    {
        SafeRedirect();
    }

    protected void BtnExit_Click(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.ClearStudentCookies();
        Session.Abandon();
        Response.Redirect("~/index.aspx");
    }

    // 检查登录状态（已被LoadStudentInfo中的检查替代，此方法仅作防御性检查）
    private void CheckLoginStatus()
    {
        if (!LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }
    }
}