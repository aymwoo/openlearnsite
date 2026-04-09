using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using LearnSite.Model;
using LearnSite.BLL;

public partial class exam_question_banklist : System.Web.UI.Page
{
    protected TeaCook tcook = new TeaCook();
    protected bool IsSelectMode { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!tcook.IsExist())
        {
            Response.Redirect("~/index.aspx");
            return;
        }

        // 检查是否为选择模式
        IsSelectMode = Request.QueryString["select"] == "1";

        if (!IsPostBack)
        {
            if (IsSelectMode)
            {
                SetupSelectMode();
            }
            BindBankList();
        }
    }

    private void SetupSelectMode()
    {
        ltlPageTitle.Text = "选择题库";
        pnlNormalButtons.Visible = false;
        pnlSelectButtons.Visible = true;
        pnlSelectTip.Visible = true;
    }

    protected string GetCardClick(object bankId)
    {
        if (IsSelectMode)
        {
            return string.Format("location.href='questionlist.aspx?bankId={0}&select=1'", bankId);
        }
        else
        {
            return string.Format("location.href='questionlist.aspx?bankId={0}'", bankId);
        }
    }

    private void BindBankList()
    {
        var bankBll = new LearnSite.BLL.ExamQuestionBank();
        var list = bankBll.GetBankList(tcook.Hid.ToString());

        rptBanks.DataSource = list;
        rptBanks.DataBind();

        pnlEmpty.Visible = list.Count == 0;
    }

    protected void rptBanks_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        int bankId = int.Parse(e.CommandArgument.ToString());

        if (e.CommandName == "Edit")
        {
            var bankBll = new LearnSite.BLL.ExamQuestionBank();
            var bank = bankBll.GetBankById(bankId);
            if (bank != null)
            {
                ltlModalTitle.Text = "编辑题库";
                hfBankId.Value = bankId.ToString();
                txtBankName.Text = bank.BankName;
                txtBankCode.Text = bank.BankCode ?? "";
                txtDescription.Text = bank.Description ?? "";
                ClientScript.RegisterStartupScript(this.GetType(), "showModal",
                    "document.getElementById('bankModal').classList.add('show');", true);
            }
        }
        else if (e.CommandName == "Delete")
        {
            var bankBll = new LearnSite.BLL.ExamQuestionBank();
            bankBll.DeleteBank(bankId);
            BindBankList();
        }
    }

    protected void rptBanks_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            var pnlActions = e.Item.FindControl("pnlActions") as Panel;
            if (pnlActions != null)
            {
                pnlActions.Visible = !IsSelectMode;
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtBankName.Text.Trim()))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('请输入题库名称！');", true);
            return;
        }

        var bankBll = new LearnSite.BLL.ExamQuestionBank();

        if (string.IsNullOrEmpty(hfBankId.Value))
        {
            // 新建
            var bank = new LearnSite.Model.ExamQuestionBank
            {
                BankName = txtBankName.Text.Trim(),
                BankCode = txtBankCode.Text.Trim(), // BLL 层会自动生成（如果为空）
                Description = txtDescription.Text.Trim(),
                CreateBy = tcook.Hid.ToString()
            };
            bankBll.AddBank(bank);
        }
        else
        {
            // 编辑
            var bank = bankBll.GetBankById(int.Parse(hfBankId.Value));
            if (bank != null)
            {
                bank.BankName = txtBankName.Text.Trim();
                bank.BankCode = txtBankCode.Text.Trim();
                bank.Description = txtDescription.Text.Trim();
                bank.UpdateBy = tcook.Hid.ToString();
                bankBll.UpdateBank(bank);
            }
        }

        BindBankList();
        ClientScript.RegisterStartupScript(this.GetType(), "hideModal", "hideModal();", true);
    }
}
