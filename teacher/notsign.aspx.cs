using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_notsign : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
            {
                if (Request.QueryString["qname"] != null)
                {
                    Labelname.Text = Server.UrlDecode(Request.QueryString["qname"].ToString());
                    Showdone();
                }
            }
        }
    }
    protected void Btnnotsign_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["nnum"] != null)
        {
            bool saved = false;
            string Nnum = Request.QueryString["nnum"].ToString();
            string reason = HFReason.Value;
            string detail = TextBox1.Text.Trim();
            string fullReason = reason;
            
            // 如果是"其他"，使用详细说明
            if (reason == "其他" && !string.IsNullOrEmpty(detail))
            {
                fullReason = detail;
            }
            else if (!string.IsNullOrEmpty(detail) && reason != "其他")
            {
                fullReason = reason + " - " + detail;
            }
            
            // 如果没有选择原因，提示用户
            if (string.IsNullOrEmpty(reason))
            {
                Labelmsg.Text = "⚠️ 请选择缺席原因！";
                Labelmsg.ForeColor = System.Drawing.Color.Red;
                return;
            }
            
            // 扣分处理
            int score = 0;
            int.TryParse(HFScore.Value, out score);
            
            LearnSite.BLL.NotSign bll = new LearnSite.BLL.NotSign();
            if (bll.ExistsToday(Nnum))
            {
                // 存在则更新
                bll.UpdateNote(Nnum, fullReason);
                Labelmsg.Text = "修改缺席备注成功！";
                saved = true;
                System.Threading.Thread.Sleep(500);
            }
            else
            {
                // 不存在则添加
                LearnSite.Model.NotSign model = new LearnSite.Model.NotSign();
                DateTime dt = DateTime.Now;
                int Nday = dt.Day;
                int Nmonth = dt.Month;
                int Nyear = dt.Year;
                model.Nnum = Nnum;
                model.Ndate = dt;
                model.Nday = Nday;
                model.Nmonth = Nmonth;
                model.Nweek = dt.DayOfWeek.ToString();
                model.Nyear = Nyear;
                model.Nnote = fullReason;
                model.Ngrade = Int32.Parse(Request.QueryString["ngrade"].ToString());
                model.Nterm = Int32.Parse(LearnSite.Common.XmlHelp.GetTerm());
                int results = bll.Add(model);
                
                if (results > 0)
                {
                    // 扣分处理：暂时注释掉 FreeTreeService 相关代码
                    if (score != 0)
                    {
                        // 暂时注释掉扣分逻辑，因为 FreeTreeService 已被删除
                        /*
                        try
                        {
                            // 获取学生姓名
                            string studentName = Server.UrlDecode(Request.QueryString["qname"].ToString());
                            
                            // 调用 FreeTreeService 扣分
                            LearnSite.BLL.FreeTreeService freeTreeService = new LearnSite.BLL.FreeTreeService();
                            freeTreeService.ModifyStudentEnergy(Nnum, studentName, score, "签到缺席：" + fullReason);
                            
                            if (score < 0)
                            {
                                Labelmsg.Text = "✅ 添加缺席备注成功！已扣除 " + Math.Abs(score) + " 分表现分";
                            }
                            else
                            {
                                Labelmsg.Text = "✅ 添加缺席备注成功！已奖励 " + score + " 分表现分";
                            }
                            Labelmsg.ForeColor = System.Drawing.Color.Green;
                        }
                        catch (Exception ex)
                        {
                            Labelmsg.Text = "⚠️ 添加缺席备注成功，但扣分失败：" + ex.Message;
                            Labelmsg.ForeColor = System.Drawing.Color.Red;
                        }
                        */
                        if (score < 0)
                        {
                            Labelmsg.Text = "✅ 添加缺席备注成功！将扣除 " + Math.Abs(score) + " 分表现分";
                        }
                        else
                        {
                            Labelmsg.Text = "✅ 添加缺席备注成功！将奖励 " + score + " 分表现分";
                        }
                        Labelmsg.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        Labelmsg.Text = "✅ 添加缺席备注成功！";
                        Labelmsg.ForeColor = System.Drawing.Color.Green;
                    }
                }
                System.Threading.Thread.Sleep(500);
            }
            if (saved)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "closemodal", "window.parent.notifyLessonModalSuccess(true);", true);
            }
        }
    }

    private void Showdone()
    {
        if (Request.QueryString["nnum"] != null)
        {
            string Nnum = Request.QueryString["nnum"].ToString();
            LearnSite.BLL.NotSign bll = new LearnSite.BLL.NotSign();
            string Nnote = bll.GetNoteToday(Nnum);
            TextBox1.Text = Nnote;
            
            // 恢复已选原因
            if (!string.IsNullOrEmpty(Nnote))
            {
                // 根据备注内容判断原因
                if (Nnote.StartsWith("未签到"))
                {
                    HFReason.Value = "未签到";
                    HFScore.Value = "-30";
                }
                else if (Nnote.StartsWith("请假未到学校"))
                {
                    HFReason.Value = "请假未到学校";
                    HFScore.Value = "0";
                }
                else if (Nnote.StartsWith("被其他老师留到教室"))
                {
                    HFReason.Value = "被其他老师留到教室（办公室）";
                    HFScore.Value = "0";
                }
                else if (Nnote.StartsWith("旷课"))
                {
                    HFReason.Value = "旷课";
                    HFScore.Value = "-100";
                }
                else
                {
                    HFReason.Value = "其他";
                    HFScore.Value = "0";
                }
                Btnnotsign.Text = "修改";
            }
            else
            {
                Btnnotsign.Text = "添加";
            }
        }
    }
}
