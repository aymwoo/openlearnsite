using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_studentseatedit : System.Web.UI.Page
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
            lblSnum.Text = student.Snum;
            hfSnum.Value = student.Snum;
            lblSname.Text = student.Sname;
            lblGradeClass.Text = student.Sgrade.ToString() + "年级" + student.Sclass.ToString() + "班";
            hfSgrade.Value = student.Sgrade.ToString();
            hfSclass.Value = student.Sclass.ToString();
            
            // 获取当前机号（优先显示临时机号）
            string currentSeat = stubll.GetActualSeat(student.Snum);
            lblCurrentSeat.Text = string.IsNullOrEmpty(currentSeat) ? "未设置" : currentSeat;
            
            // 提取实际机号（去除"(临时)"后缀）
            string actualSeat = currentSeat.Replace("(临时)", "").Trim();
            txtNewSeat.Text = actualSeat;
        }
    }

    /// <summary>
    /// 编辑类型切换
    /// </summary>
    protected void rblEditType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblEditType.SelectedValue == "temp")
        {
            liPermanent.Visible = false;
            liTemp.Visible = true;
        }
        else
        {
            liPermanent.Visible = true;
            liTemp.Visible = false;
        }
    }

    /// <summary>
    /// 确认修改机号
    /// </summary>
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            string snum = hfSnum.Value;
            string newSeat = txtNewSeat.Text.Trim();
            string oldSeat = lblCurrentSeat.Text;
            
            if (string.IsNullOrEmpty(newSeat))
            {
                lblMessage.Text = "请输入新机号！";
                return;
            }
            
            // 检查是否与当前机号相同
            if (newSeat == oldSeat)
            {
                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = "新机号与当前机号相同，无需修改。";
                return;
            }
            
            LearnSite.BLL.Students stubll = new LearnSite.BLL.Students();
            
            if (rblEditType.SelectedValue == "permanent")
            {
                // 永久修改 - 保存到数据库
                bool success = stubll.UpdateFixedSeatBool(snum, newSeat);
                
                if (success)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "机号修改成功！原机号：" + oldSeat + " → 新机号：" + newSeat;
                    lblCurrentSeat.Text = newSeat;
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "机号修改失败，请重试！";
                }
            }
            else
            {
                // 临时换机 - 保存到数据库，1小时后失效
                LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
                bool success = sbll.TempAssignSeat(snum, "", newSeat, 60);
                
                if (success)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "临时换机成功！原机号：" + oldSeat + " → 临时机号：" + newSeat + "（1小时后自动失效）";
                    lblCurrentSeat.Text = newSeat + "(临时)";
                }
                else
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "临时换机失败，请重试！";
                }
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
