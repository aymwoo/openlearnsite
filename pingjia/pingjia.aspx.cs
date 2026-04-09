using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pingjia_pingjia : System.Web.UI.Page
{
    LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();

    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!IsPostBack)
        {
            Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "学生评价";
            Lbterm.Text = LearnSite.Common.XmlHelp.GetTerm();
            Grade();
            DDLgrade.SelectedIndex = 0;
            Getclass();
            DDLclass.SelectedIndex = 0;
            showstudents();
            UpdateWeightSumLabel();
        }
    }

    /// <summary>
    /// 更新权重总和显示标签
    /// </summary>
    private void UpdateWeightSumLabel()
    {
        int persscore = int.Parse(DDLwork.SelectedValue);
        int persquiz = int.Parse(DDLquiz.SelectedValue);
        int perstscore = int.Parse(DDLtyper.SelectedValue);
        int perattitude = int.Parse(DDLattitude.SelectedValue);
        int persurvey = 0; // 调查问卷已合并到作品中，固定为0
        int perssignin = int.Parse(DDLsignin.SelectedValue);

        int totalWeight = persscore + persquiz + perstscore + perattitude + persurvey + perssignin;

        if (totalWeight == 100)
        {
            LabelWeightSum.Text = "(当前总和: " + totalWeight + "%)";
            LabelWeightSum.ForeColor = System.Drawing.Color.Green;
        }
        else
        {
            LabelWeightSum.Text = "(当前总和: " + totalWeight + "%) ⚠ 必须为100%";
            LabelWeightSum.ForeColor = System.Drawing.Color.Red;
        }
    }

    protected void BtnScoresNo_Click(object sender, EventArgs e)
    {
        LearnSite.BLL.Works works = new LearnSite.BLL.Works();
        works.WorkNoScoreSetP(tcook.Hid);
        Labelmsg.Text = "所教班级未评作品已经被评为C即6分！";
        System.Threading.Thread.Sleep(500);
        showstudents();
    }

    protected void BtnScore_Click(object sender, EventArgs e)
    {
        int persscore = int.Parse(DDLwork.SelectedValue);
        int persquiz = int.Parse(DDLquiz.SelectedValue);
        int perstscore = int.Parse(DDLtyper.SelectedValue);
        int perattitude = int.Parse(DDLattitude.SelectedValue);
        int persurvey = 0; // 调查问卷已合并到作品中，固定为0
        int perssignin = int.Parse(DDLsignin.SelectedValue);

        int totalWeight = persscore + persquiz + perstscore + perattitude + persurvey + perssignin;

        // 检查权重总和是否为100%
        if (totalWeight != 100)
        {
            Labelmsg.Text = "<span style='color: red;'>错误：权重总和必须为100%，当前总和为" + totalWeight + "%！请调整各项权重设置。</span>";
            return;
        }

        int hid = tcook.Hid;
        DateTime nowtime1 = DateTime.Now;
        LearnSite.BLL.Students stu = new LearnSite.BLL.Students();
        stu.ClearAllScores(hid);
        stu.InitSidle();
        stu.UpdateBestSquiz();//取回测验最高成绩
        stu.ThisTeamScoresNew(hid);//批量更新所教所有班级当前学期作品总积分和表现总积分、调查测验分

        stu.ThisTeamGroupScores(hid);//批量更新所教班级当前学期小组合作分

        stu.UpdateStscore();// 更新学生表的打字成绩
        stu.UpdateSfscore();//更新学生表的指法成绩
        stu.UpdateSchinese();//更新学生表的中文拼音成绩

        // 获取复选框选择状态
        bool useWork = ChkWork.Checked;
        bool useGroup = ChkGroup.Checked;
        bool useDiscuss = ChkDiscuss.Checked;
        bool useForm = ChkForm.Checked;
        bool useIdle = ChkIdle.Checked;
        bool useSurvey = ChkSurvey.Checked;

        stu.UpdateAllScoreWithFilter(persscore, persquiz, perstscore, perattitude, persurvey, perssignin, hid,
                                   useWork, useGroup, useDiscuss, useForm, useIdle, useSurvey);
        DateTime nowtime2 = DateTime.Now;
        Labelmsg.Text = "统计用时：" + LearnSite.Common.Computer.DatagoneMilliseconds(nowtime1, nowtime2) + "毫秒 (权重总和:" + totalWeight + "%)";
        System.Threading.Thread.Sleep(200);
        showstudents();
    }

    protected void Btnape_Click(object sender, EventArgs e)
    {
        int persscore = int.Parse(DDLwork.SelectedValue);
        int persquiz = int.Parse(DDLquiz.SelectedValue);
        int perstscore = int.Parse(DDLtyper.SelectedValue);
        int perattitude = int.Parse(DDLattitude.SelectedValue);
        int persurvey = 0; // 调查问卷已合并到作品中，固定为0
        int perssignin = int.Parse(DDLsignin.SelectedValue);

        int totalWeight = persscore + persquiz + perstscore + perattitude + persurvey + perssignin;

        // 检查权重总和是否为100%
        if (totalWeight != 100)
        {
            Labelmsg.Text = "<span style='color: red;'>错误：权重总和必须为100%，当前总和为" + totalWeight + "%！请调整各项权重设置。</span>";
            return;
        }

        int hid = tcook.Hid;
        DateTime nowtime1 = DateTime.Now;
        LearnSite.BLL.Students stu = new LearnSite.BLL.Students();
        stu.ClearAllScores(hid);
        stu.InitSidle();
        stu.UpdateBestSquiz();//取回测验最高成绩
        stu.ThisTeamScoresNew(hid);//批量更新所教所有班级当前学期作品总积分和表现总积分、调查测验分
        stu.ThisTeamGroupScores(hid);//批量更新所教班级当前学期小组合作分
        stu.UpdateStscore();// 更新学生表的打字成绩
        stu.UpdateSfscore();//更新学生表的指法成绩
        stu.UpdateSchinese();//更新学生表的中文拼音成绩

        // 获取复选框选择状态
        bool useWork = ChkWork.Checked;
        bool useGroup = ChkGroup.Checked;
        bool useDiscuss = ChkDiscuss.Checked;
        bool useForm = ChkForm.Checked;
        bool useIdle = ChkIdle.Checked;
        bool useSurvey = ChkSurvey.Checked;

        stu.UpdateAllScoreWithFilter(persscore, persquiz, perstscore, perattitude, persurvey, perssignin, hid,
                                   useWork, useGroup, useDiscuss, useForm, useIdle, useSurvey);

        stu.TermABCD();
        DateTime nowtime2 = DateTime.Now;
        System.Threading.Thread.Sleep(1000);
        LearnSite.BLL.TermTotal mbll = new LearnSite.BLL.TermTotal();
        mbll.TermScore();//生成学期统计表

        Labelmsg.Text = "自动评价用时：" + LearnSite.Common.Computer.DatagoneMilliseconds(nowtime1, nowtime2) + "毫秒，本学期成绩自动存档成功！";
        showstudents();
    }

    protected void BtnExcel_Click(object sender, EventArgs e)
    {
        ExportToExcel();
    }

    protected void Btntermview_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/teacher/termview.aspx", false);
    }

    private void showstudents()
    {
        int Sgrade = Int32.Parse(DDLgrade.SelectedValue.ToString());
        int Sclass = Int32.Parse(DDLclass.SelectedValue.ToString());
        LearnSite.BLL.Students stus = new LearnSite.BLL.Students();
        GVStudents.DataSource = stus.GetListTerm(Sgrade, Sclass);
        GVStudents.DataBind();
        Btnape.ToolTip = "开始自动评价汇总，并保存当前学期成绩表";
    }

    /// <summary>
    /// 导出Excel
    /// </summary>
    private void ExportToExcel()
    {
        try
        {
            int Sgrade = Int32.Parse(DDLgrade.SelectedValue.ToString());
            int Sclass = Int32.Parse(DDLclass.SelectedValue.ToString());

            // 获取数据
            LearnSite.BLL.Students stus = new LearnSite.BLL.Students();
            DataTable dt = stus.GetListTerm(Sgrade, Sclass).Tables[0];

            // 设置响应头
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "GB2312";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + Server.UrlEncode("学生评价_" + Sgrade + "年" + Sclass + "班.xls"));
            Response.ContentType = "application/ms-excel";

            // 构建Excel内容
            System.IO.StringWriter sw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);

            // 构建表格
            System.Web.UI.WebControls.GridView gvExport = new System.Web.UI.WebControls.GridView();
            gvExport.DataSource = dt;
            gvExport.AllowPaging = false;
            gvExport.DataBind();

            // 输出
            gvExport.RenderControl(hw);
            Response.Write(sw.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            Labelmsg.Text = "导出失败：" + ex.Message;
        }
    }

    protected void GVStudents_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex > -1)
        {
            e.Row.Cells[0].Text = Convert.ToString(e.Row.RowIndex + 1);
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //当鼠标放上去的时候 先保存当前行的背景颜色 并给附一颜色
            e.Row.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#E1E8E1',this.style.fontWeight='';");
            //当鼠标离开的时候 将背景颜色还原的以前的颜色
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor,this.style.fontWeight='';");
            //单击行改变行背景颜色
            e.Row.Attributes.Add("onclick", "this.style.backgroundColor='#D8E0D8'; this.style.color='buttontext';this.style.cursor='default';");
        }
    }

    private void Grade()
    {
        LearnSite.BLL.Room room = new LearnSite.BLL.Room();
        DDLgrade.DataSource = room.GetGrade(tcook.Hid);
        DDLgrade.DataTextField = "Rgrade";
        DDLgrade.DataValueField = "Rgrade";
        DDLgrade.DataBind();
    }

    private void Getclass()
    {
        LearnSite.BLL.Room room = new LearnSite.BLL.Room();
        DDLclass.DataSource = room.GetLimitClass(Int32.Parse(DDLgrade.SelectedValue));
        DDLclass.DataTextField = "Rclass";
        DDLclass.DataValueField = "Rclass";
        DDLclass.DataBind();
    }

    protected void Btnback_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/teacher/works.aspx", false);
    }

    protected void DDLgrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(DDLgrade.SelectedValue))
        {
            LearnSite.BLL.Room room = new LearnSite.BLL.Room();
            DDLclass.DataSource = room.GetLimitClass(Int32.Parse(DDLgrade.SelectedValue));
            DDLclass.DataTextField = "Rclass";
            DDLclass.DataValueField = "Rclass";
            DDLclass.DataBind();
            showstudents();
        }
    }

    protected void DDLclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        showstudents();
        Labelmsg.Text = "";
    }

    protected void DDLwork_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateWeightSumLabel();
    }

    protected void DDLquiz_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateWeightSumLabel();
    }

    protected void DDLtyper_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateWeightSumLabel();
    }

    protected void DDLattitude_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateWeightSumLabel();
    }

    protected void DDLsignin_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateWeightSumLabel();
    }

    protected void Chk_CheckedChanged(object sender, EventArgs e)
    {
        // 复选框状态改变时，无需更新权重总和，因为权重参数没有变化
        // 这里可以添加其他逻辑，比如提示用户当前选择了哪些字段
    }
}
