using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

public partial class Teacher_signinmanage : System.Web.UI.Page
{
    LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();

    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!IsPostBack)
        {
            Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "签到管理";
            Lbterm.Text = LearnSite.Common.XmlHelp.GetTerm();
            InitDDL();
        }
    }

    private void InitDDL()
    {
        LearnSite.BLL.Room rbll = new LearnSite.BLL.Room();
        
        DataTable dtCurrentClass = rbll.GetCurrentTeachingClass(tcook.Hid);
        
        if (dtCurrentClass == null || dtCurrentClass.Rows.Count == 0)
        {
            PanelNoClass.Visible = true;
            PanelContent.Visible = false;
            return;
        }
        
        PanelNoClass.Visible = false;
        PanelContent.Visible = true;
        
        DataTable dtGrade = rbll.GetGrade(tcook.Hid);
        DDLgrade.DataSource = dtGrade;
        DDLgrade.DataTextField = "Rgrade";
        DDLgrade.DataValueField = "Rgrade";
        DDLgrade.DataBind();
        
        int firstGrade = Convert.ToInt32(dtCurrentClass.Rows[0]["Rgrade"]);
        int firstClass = Convert.ToInt32(dtCurrentClass.Rows[0]["Rclass"]);
        
        foreach (ListItem item in DDLgrade.Items)
        {
            if (item.Value == firstGrade.ToString())
            {
                item.Selected = true;
                break;
            }
        }
        
        BindClassDropDown();
        
        foreach (ListItem item in DDLclass.Items)
        {
            if (item.Value == firstClass.ToString())
            {
                item.Selected = true;
                break;
            }
        }
        
        ShowSignin();
    }

    private void BindClassDropDown()
    {
        if (DDLgrade.Items.Count > 0)
        {
            int Rgrade = Int32.Parse(DDLgrade.SelectedValue);
            LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
            DDLclass.DataSource = rm.GetLimitClass(Rgrade);
            DDLclass.DataTextField = "Rclass";
            DDLclass.DataValueField = "Rclass";
            DDLclass.DataBind();
        }
    }

    protected void DDLgrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindClassDropDown();
        ShowSignin();
    }

    protected void DDLclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowSignin();
    }

    private void ShowSignin()
    {
        if (DDLgrade.Items.Count > 0 && DDLclass.Items.Count > 0)
        {
            int Sgrade = Int32.Parse(DDLgrade.SelectedValue);
            int Sclass = Int32.Parse(DDLclass.SelectedValue);

            LearnSite.BLL.Signin signinBll = new LearnSite.BLL.Signin();
            DataSet ds = signinBll.GetTodaySigninList(Sgrade, Sclass);

            DataTable dt = ds.Tables[0];
            dt.Columns.Add("RowNum", typeof(int));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["RowNum"] = i + 1;
            }

            GVSignin.DataSource = dt;
            GVSignin.DataBind();
        }
    }

    protected void BtnAddScore_Click(object sender, EventArgs e)
    {
        List<int> selectedIds = GetSelectedSigninIds();
        if (selectedIds.Count == 0)
        {
            lblAddMsg.Text = "请先选择要加分的学生！";
            return;
        }

        List<string> selectedReasons = new List<string>();
        foreach (ListItem item in CBLAddReason.Items)
        {
            if (item.Selected)
            {
                selectedReasons.Add(item.Value);
            }
        }

        if (selectedReasons.Count == 0)
        {
            lblAddMsg.Text = "请至少选择一项加分明细！";
            return;
        }

        int scorePerItem = 5;
        int totalScore = scorePerItem * selectedReasons.Count;
        string reasons = string.Join("、", selectedReasons);

        LearnSite.BLL.Signin signinBll = new LearnSite.BLL.Signin();
        int count = signinBll.SelectedAddAttitudeScore(selectedIds, totalScore, reasons);

        lblAddMsg.Text = string.Format("成功为 {0} 位学生加了 {1} 分（{2}）", count, totalScore, reasons);
        lblSubMsg.Text = "";
        lblUnSignMsg.Text = "";
        ShowSignin();
    }

    protected void BtnSubScore_Click(object sender, EventArgs e)
    {
        List<int> selectedIds = GetSelectedSigninIds();
        if (selectedIds.Count == 0)
        {
            lblSubMsg.Text = "请先选择要减分的学生！";
            return;
        }

        List<string> selectedReasons = new List<string>();
        foreach (ListItem item in CBLSubReason.Items)
        {
            if (item.Selected)
            {
                selectedReasons.Add(item.Value);
            }
        }

        if (selectedReasons.Count == 0)
        {
            lblSubMsg.Text = "请至少选择一项减分明细！";
            return;
        }

        int scorePerItem = 5;
        int totalScore = scorePerItem * selectedReasons.Count;
        string reasons = string.Join("、", selectedReasons);

        LearnSite.BLL.Signin signinBll = new LearnSite.BLL.Signin();
        int count = signinBll.SelectedSubAttitudeScore(selectedIds, totalScore, reasons);

        lblSubMsg.Text = string.Format("成功为 {0} 位学生扣了 {1} 分（{2}）", count, totalScore, reasons);
        lblAddMsg.Text = "";
        lblUnSignMsg.Text = "";
        ShowSignin();
    }
    
    protected void BtnUnSignSub_Click(object sender, EventArgs e)
    {
        if (DDLgrade.Items.Count > 0 && DDLclass.Items.Count > 0)
        {
            int Sgrade = Int32.Parse(DDLgrade.SelectedValue);
            int Sclass = Int32.Parse(DDLclass.SelectedValue);
            
            LearnSite.BLL.Signin signinBll = new LearnSite.BLL.Signin();
            int count = signinBll.SubScoreToUnsignStudents(Sgrade, Sclass, 30, "未签到扣分");
            
            lblUnSignMsg.Text = string.Format("成功为 {0} 位未签到学生扣了 30 分", count);
            lblAddMsg.Text = "";
            lblSubMsg.Text = "";
            ShowSignin();
        }
    }

    protected void BtnReturn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/teacher/teachermanage.aspx", false);
    }

    protected void GVSignin_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVSignin.PageIndex = e.NewPageIndex;
        ShowSignin();
    }

    protected void CBSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow row in GVSignin.Rows)
        {
            CheckBox cb = (CheckBox)row.FindControl("CBSelect");
            cb.Checked = CBSelectAll.Checked;
        }
    }

    private List<int> GetSelectedSigninIds()
    {
        List<int> selectedIds = new List<int>();
        foreach (GridViewRow row in GVSignin.Rows)
        {
            CheckBox cb = (CheckBox)row.FindControl("CBSelect");
            if (cb != null && cb.Checked)
            {
                int qid = Convert.ToInt32(GVSignin.DataKeys[row.RowIndex].Value);
                selectedIds.Add(qid);
            }
        }
        return selectedIds;
    }
}
