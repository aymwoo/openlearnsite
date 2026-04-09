using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class Teacher_studentnumedit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        
        if (!IsPostBack)
        {
            if (Request.QueryString["sid"] != null)
            {
                int sid = Int32.Parse(Request.QueryString["sid"].ToString());
                hfSid.Value = sid.ToString();
                LoadStudentInfo(sid);
                ShowAvailableNumbers();
            }
            else
            {
                Response.Redirect("~/teacher/teachermanage.aspx", false);
            }
        }
    }

    /// <summary>
    /// 加载学生信息
    /// </summary>
    private void LoadStudentInfo(int sid)
    {
        LearnSite.BLL.Students stubll = new LearnSite.BLL.Students();
        LearnSite.Model.Students student = stubll.GetModel(sid);
        
        if (student != null)
        {
            lblCurrentSnum.Text = student.Snum;
            lblSname.Text = student.Sname;
            lblGradeClass.Text = student.Sgrade.ToString() + "年级" + student.Sclass.ToString() + "班";
            txtNewSnum.Text = student.Snum;
        }
    }

    /// <summary>
    /// 显示当前班级可用的空缺学号
    /// </summary>
    private void ShowAvailableNumbers()
    {
        try
        {
            int sid = Int32.Parse(hfSid.Value);
            LearnSite.BLL.Students stubll = new LearnSite.BLL.Students();
            LearnSite.Model.Students student = stubll.GetModel(sid);
            
            if (student != null)
            {
                int sgrade = student.Sgrade.Value;
                int sclass = student.Sclass.Value;
                
                // 获取空缺学号
                List<string> availableNumbers = GetAvailableNumbers(sgrade, sclass);
                
                if (availableNumbers.Count > 0)
                {
                    divSuggested.Visible = true;
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<span style='color: green;'>");
                    for (int i = 0; i < Math.Min(10, availableNumbers.Count); i++)
                    {
                        if (i > 0) sb.Append(", ");
                        sb.Append(availableNumbers[i]);
                    }
                    if (availableNumbers.Count > 10)
                    {
                        sb.Append(" ... 共 " + availableNumbers.Count + " 个空缺学号");
                    }
                    sb.Append("</span>");
                    lblSuggested.Text = sb.ToString();
                }
                else
                {
                    divSuggested.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "获取空缺学号失败：" + ex.Message;
        }
    }

    /// <summary>
    /// 获取指定班级的空缺学号
    /// </summary>
    private List<string> GetAvailableNumbers(int sgrade, int sclass)
    {
        List<string> availableNumbers = new List<string>();
        
        try
        {
            // 获取该班级所有学号
            LearnSite.BLL.Students stubll = new LearnSite.BLL.Students();
            System.Data.DataSet ds = stubll.GetListStudents(sgrade, sclass);
            
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                // 获取最大学号和最小学号
                long minNum = long.MaxValue;
                long maxNum = long.MinValue;
                HashSet<long> existingNumbers = new HashSet<long>();
                
                foreach (System.Data.DataRow row in ds.Tables[0].Rows)
                {
                    string snum = row["Snum"].ToString();
                    long num;
                    if (long.TryParse(snum, out num))
                    {
                        existingNumbers.Add(num);
                        if (num < minNum) minNum = num;
                        if (num > maxNum) maxNum = num;
                    }
                }
                
                // 查找空缺学号
                for (long i = minNum; i <= maxNum; i++)
                {
                    if (!existingNumbers.Contains(i))
                    {
                        availableNumbers.Add(i.ToString());
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // 记录错误但不影响主流程
            System.Diagnostics.Debug.WriteLine("获取空缺学号失败：" + ex.Message);
        }
        
        return availableNumbers;
    }

    /// <summary>
    /// 检查学号是否可用
    /// </summary>
    protected void btnCheck_Click(object sender, EventArgs e)
    {
        string newSnum = txtNewSnum.Text.Trim();
        string currentSnum = lblCurrentSnum.Text;
        
        if (string.IsNullOrEmpty(newSnum))
        {
            lblMessage.Text = "请输入新学号！";
            return;
        }
        
        // 检查是否与当前学号相同
        if (newSnum == currentSnum)
        {
            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = "新学号与当前学号相同，无需修改。";
            btnUpdate.Enabled = false;
            return;
        }
        
        // 检查学号格式
        if (!LearnSite.Common.WordProcess.IsNum(newSnum))
        {
            lblMessage.Text = "学号必须为数字！";
            btnUpdate.Enabled = false;
            return;
        }
        
        // 检查学号是否已存在
        LearnSite.BLL.Students stubll = new LearnSite.BLL.Students();
        if (stubll.ExistsSnum(newSnum))
        {
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Text = "该学号已存在，请选择其他学号！";
            btnUpdate.Enabled = false;
        }
        else
        {
            lblMessage.ForeColor = System.Drawing.Color.Green;
            lblMessage.Text = "✓ 学号可用，可以修改。";
            btnUpdate.Enabled = true;
        }
    }

    /// <summary>
    /// 确认修改学号
    /// </summary>
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            int sid = Int32.Parse(hfSid.Value);
            string oldSnum = lblCurrentSnum.Text;
            string newSnum = txtNewSnum.Text.Trim();
            
            // 再次验证
            if (oldSnum == newSnum)
            {
                lblMessage.Text = "学号未改变！";
                return;
            }
            
            LearnSite.BLL.Students stubll = new LearnSite.BLL.Students();
            
            // 再次检查新学号是否存在
            if (stubll.ExistsSnum(newSnum))
            {
                lblMessage.Text = "该学号已存在，修改失败！";
                return;
            }
            
            // 更新学号
            bool success = stubll.UpdateSnum(sid, newSnum);
            
            if (success)
            {
                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = "学号修改成功！原学号：" + oldSnum + " → 新学号：" + newSnum;
                btnUpdate.Enabled = false;
                
                // 更新显示
                lblCurrentSnum.Text = newSnum;
                ShowAvailableNumbers();
            }
            else
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "学号修改失败，请重试！";
            }
        }
        catch (Exception ex)
        {
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Text = "修改失败：" + ex.Message;
        }
    }

    /// <summary>
    /// 取消修改
    /// </summary>
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        // 关闭窗口
        ClientScript.RegisterStartupScript(this.GetType(), "close", 
            "window.parent.TINY.box.hide();", true);
    }
}
