using System;
using System.Data;
using System.Web.UI.WebControls;
public partial class Student_mytotal : System.Web.UI.Page
{
    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            LearnSite.Common.CookieHelp.KickStudent();
            if (!IsPostBack)
            {
                ShowMyTotal();
            }
        }
        else
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }
    }

    private void ShowMyTotal()
    {
        DateTime dt1 = DateTime.Now;

        int mySid = cook.Sid;
        int Sgrade = cook.Sgrade;
        int Sclass = cook.Sclass;
        string mysnum = cook.Snum;
        int cterm = cook.ThisTerm;

        LearnSite.BLL.Courses cbll = new LearnSite.BLL.Courses();
        // 获取该学生本学期的所有学案
        DataTable dtCourses = cbll.GetCoursesByGradeTerm(Sgrade, cterm);

        if (dtCourses == null || dtCourses.Rows.Count == 0)
        {
            Labelmsg.Text = "暂无学习记录";
            return;
        }

        // 创建结果表，以学案为行，活动为列
        DataTable dtResult = new DataTable();
        dtResult.Columns.Add("课时", typeof(int));
        dtResult.Columns.Add("学习内容", typeof(string));

        // 用于记录学案是否已添加
        System.Collections.Generic.Dictionary<int, DataRow> courseRows = new System.Collections.Generic.Dictionary<int, DataRow>();

        // 遍历每个学案，添加活动列和行
        foreach (DataRow drCourse in dtCourses.Rows)
        {
            int Cid = Convert.ToInt32(drCourse["Cid"]);
            string Ctitle = drCourse["Ctitle"].ToString();
            int Cks = Convert.ToInt32(drCourse["Cks"]);

            // 如果这个学案还没有行，就创建一行
            if (!courseRows.ContainsKey(Cid))
            {
                DataRow dr = dtResult.NewRow();
                dr["课时"] = Cks;
                dr["学习内容"] = Ctitle;
                dtResult.Rows.Add(dr);
                courseRows[Cid] = dr;
            }

            // 获取该学案的所有学习活动
            LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
            DataTable dtMenu = lbll.GetShowedMenu(Cid).Tables[0];

            if (dtMenu != null && dtMenu.Rows.Count > 0)
            {
                foreach (DataRow drMenu in dtMenu.Rows)
                {
                    string Ltype = drMenu["Ltype"].ToString();
                    int Lxid = Convert.ToInt32(drMenu["Lxid"]);
                    string Ltitle = drMenu["Ltitle"].ToString();

                    // 添加活动列（如果还没添加）
                    string colName = Ltitle + " (" + Ltype + ")";
                    if (!dtResult.Columns.Contains(colName))
                    {
                        dtResult.Columns.Add(colName, typeof(string));
                    }

                    // 获取分数
                    string score = GetStudentActivityScore(mySid, mysnum, Ltype, Lxid);
                    courseRows[Cid][colName] = score;
                }
            }
        }

        // 添加课堂表现和汇总列
        dtResult.Columns.Add("课堂表现", typeof(string));
        dtResult.Columns.Add("汇总", typeof(string));

        // 填充课堂表现和汇总
        foreach (DataRow drCourse in dtCourses.Rows)
        {
            int Cid = Convert.ToInt32(drCourse["Cid"]);
            DataRow dr = courseRows[Cid];

            // 获取课堂表现
            string attitude = GetStudentAttitude(mySid, Cid);
            dr["课堂表现"] = attitude;

            // 计算汇总
            int totalScore = 0;
            foreach (DataColumn col in dtResult.Columns)
            {
                if (col.ColumnName != "课时" && col.ColumnName != "学习内容" && col.ColumnName != "课堂表现" && col.ColumnName != "汇总")
                {
                    string score = dr[col].ToString();
                    if (!string.IsNullOrEmpty(score) && score != "0")
                    {
                        try
                        {
                            totalScore += Convert.ToInt32(score);
                        }
                        catch { }
                    }
                }
            }
            if (!string.IsNullOrEmpty(attitude) && attitude != "0")
            {
                try
                {
                    totalScore += Convert.ToInt32(attitude);
                }
                catch { }
            }
            dr["汇总"] = totalScore.ToString();
        }

        GridViewMyTotal.DataSource = dtResult;
        GridViewMyTotal.DataBind();
        GridViewMyTotal.CssClass = "compact-table";

        // 设置列宽为自适应
        foreach (DataControlField column in GridViewMyTotal.Columns)
        {
            column.ItemStyle.Width = Unit.Percentage(5);
            column.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
            column.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
        }

        Labelmsg.Text = "共 " + dtResult.Rows.Count + " 个学案";

        DateTime dt2 = DateTime.Now;
        LabelTitle.Text = "我的学习汇总（" + Sgrade + "年级" + Sclass + "班）";
    }

    private string GetStudentActivityScore(int Sid, string Snum, string Ltype, int Lxid)
    {
        try
        {
            switch (Ltype)
            {
                case "1": // 活动上传作品
                case "5": // scratch编程
                case "8": // py编程
                case "10": // 流程图
                case "11": // 像素画
                case "12": // 单网页
                case "13": // 拼图
                case "14": // 积木
                    LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
                    LearnSite.Model.Works wmodel = wbll.GetModelByStu(Lxid, Snum);
                    if (wmodel != null)
                    {
                        return wmodel.Wscore.ToString();
                    }
                    break;
                case "2": // 调查
                    LearnSite.BLL.SurveyFeedback fbll = new LearnSite.BLL.SurveyFeedback();
                    int score = fbll.ExistsScore(Lxid, Snum);
                    if (score > -1024)
                    {
                        return score.ToString();
                    }
                    break;
                case "3": // 讨论
                    LearnSite.BLL.TopicReply rbll = new LearnSite.BLL.TopicReply();
                    DataSet dsr = rbll.GetList("Rtid=" + Lxid + " and Rsid=" + Sid);
                    if (dsr != null && dsr.Tables[0].Rows.Count > 0)
                    {
                        return dsr.Tables[0].Rows[0]["Rscore"].ToString();
                    }
                    break;
                case "4": // 表单
                    LearnSite.BLL.TxtFormBack xbll = new LearnSite.BLL.TxtFormBack();
                    DataSet dsx = xbll.GetList("Rmid=" + Lxid + " and Rsid=" + Sid);
                    if (dsx != null && dsx.Tables[0].Rows.Count > 0)
                    {
                        return dsx.Tables[0].Rows[0]["Rscore"].ToString();
                    }
                    break;
                case "6": // solve
                    LearnSite.BLL.Solves vbll = new LearnSite.BLL.Solves();
                    DataSet dsv = vbll.GetList("Vmid=" + Lxid + " and Vsid=" + Sid);
                    if (dsv != null && dsv.Tables[0].Rows.Count > 0)
                    {
                        return dsv.Tables[0].Rows[0]["Vscore"].ToString();
                    }
                    break;
                case "7": // 阅读
                    LearnSite.BLL.MenuWorks kbll = new LearnSite.BLL.MenuWorks();
                    DataSet dsk = kbll.GetList("Kmid=" + Lxid + " and Ksid=" + Sid);
                    if (dsk != null && dsk.Tables[0].Rows.Count > 0)
                    {
                        return dsk.Tables[0].Rows[0]["Kscore"].ToString();
                    }
                    break;
            }
        }
        catch
        {
        }
        return "0";
    }

    private string GetStudentAttitude(int Sid, int Cid)
    {
        try
        {
            LearnSite.BLL.Signin gbll = new LearnSite.BLL.Signin();
            DataTable dt = gbll.GetStudentSigninBySidCid(Sid, Cid);
            if (dt != null && dt.Rows.Count > 0)
            {
                int attitude = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    attitude += Convert.ToInt32(dr["Qattitude"]);
                }
                return attitude.ToString();
            }
        }
        catch
        {
        }
        return "0";
    }
}
