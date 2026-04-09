using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Seat_ipnet : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeIsAdmin();
        if (!IsPostBack)
        {
            ShowIpNet();
            BindHouse();
        }
    }

    private void ShowIpNet()
    {
        LearnSite.BLL.IpNet bll = new LearnSite.BLL.IpNet();
        GVIpNet.DataSource = bll.GetAllNet();
        GVIpNet.DataBind();
    }

    private void BindHouse()
    {
        LearnSite.BLL.House hbll = new LearnSite.BLL.House();
        DataTable dt = hbll.GetListHouse().Tables[0];
        DDLHouse.DataSource = dt;
        DDLHouse.DataTextField = "Hname";
        DDLHouse.DataValueField = "Hid";
        DDLHouse.DataBind();
    }

    protected void GVIpNet_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex > -1)
        {
            e.Row.Cells[0].Text = Convert.ToString(e.Row.RowIndex + 1);
            LinkButton lbtn = (LinkButton)e.Row.FindControl("LinkButtonDel");
            if (lbtn != null)
            {
                lbtn.Attributes.Add("onclick", "return confirm('您确定要删除这个网段配置吗？');");
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#E1E8E1',this.style.fontWeight='';");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor,this.style.fontWeight='';");
            e.Row.Attributes.Add("onclick", "this.style.backgroundColor='#D8E0D8'; this.style.color='buttontext';this.style.cursor='default';");

            if ((e.Row.RowState & DataControlRowState.Edit) == DataControlRowState.Edit)
            {
                DropDownList ddl = (DropDownList)e.Row.FindControl("DDLHouse");
                if (ddl != null)
                {
                    BindHouse();
                    HiddenField hf = new HiddenField();
                    DataRowView drv = (DataRowView)e.Row.DataItem;
                    if (drv["Nhid"] != null && drv["Nhid"].ToString() != "")
                    {
                        string nhid = drv["Nhid"].ToString();
                        ListItem item = ddl.Items.FindByValue(nhid);
                        if (item != null)
                        {
                            ddl.SelectedValue = nhid;
                        }
                    }
                }
            }
        }
    }

    protected void GVIpNet_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Del")
        {
            string nid = e.CommandArgument.ToString();
            LearnSite.BLL.IpNet bll = new LearnSite.BLL.IpNet();
            bll.Delete(Int32.Parse(nid));
            System.Threading.Thread.Sleep(200);
            ShowIpNet();
        }
    }

    protected void GVIpNet_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GVIpNet.EditIndex = e.NewEditIndex;
        ShowIpNet();
    }

    protected void GVIpNet_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GVIpNet.EditIndex = -1;
        ShowIpNet();
    }

    protected void GVIpNet_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int nid = Int32.Parse(GVIpNet.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtNet = (TextBox)GVIpNet.Rows[e.RowIndex].FindControl("TextBoxNet");
        DropDownList ddlHouse = (DropDownList)GVIpNet.Rows[e.RowIndex].FindControl("DDLHouse");
        TextBox txtRemark = (TextBox)GVIpNet.Rows[e.RowIndex].FindControl("TextBoxRemark");

        string net = txtNet.Text.Trim();
        string remark = txtRemark.Text.Trim();
        int nhid = Int32.Parse(ddlHouse.SelectedValue);
        string nname = ddlHouse.SelectedItem.Text;

        if (string.IsNullOrEmpty(net))
        {
            LabelMsg.Text = "网段不能为空！";
            return;
        }

        LearnSite.Model.IpNet model = new LearnSite.Model.IpNet();
        model.Nid = nid;
        model.Nnet = net;
        model.Nhid = nhid;
        model.Nname = nname;
        model.Nremark = remark;

        LearnSite.BLL.IpNet bll = new LearnSite.BLL.IpNet();
        bll.Update(model);

        GVIpNet.EditIndex = -1;
        ShowIpNet();
        LabelMsg.Text = "更新成功！";
    }

    protected void ButtonAdd_Click(object sender, EventArgs e)
    {
        string net = TextBoxNet.Text.Trim();
        string remark = TextBoxRemark.Text.Trim();

        if (string.IsNullOrEmpty(net))
        {
            LabelMsg.Text = "网段不能为空！";
            return;
        }

        if (!IsValidNet(net))
        {
            LabelMsg.Text = "网段格式不正确，请输入如：172.16.3 格式！";
            return;
        }

        LearnSite.BLL.IpNet bll = new LearnSite.BLL.IpNet();
        if (bll.ExistsNet(net))
        {
            LabelMsg.Text = "该网段已存在！";
            return;
        }

        int nhid = Int32.Parse(DDLHouse.SelectedValue);
        string nname = DDLHouse.SelectedItem.Text;

        LearnSite.Model.IpNet model = new LearnSite.Model.IpNet();
        model.Nnet = net;
        model.Nhid = nhid;
        model.Nname = nname;
        model.Nremark = remark;

        bll.Add(model);

        TextBoxNet.Text = "";
        TextBoxRemark.Text = "";
        ShowIpNet();
        LabelMsg.Text = "添加成功！";
    }

    private bool IsValidNet(string net)
    {
        string[] parts = net.Split('.');
        if (parts.Length != 3)
            return false;

        foreach (string part in parts)
        {
            int num;
            if (!int.TryParse(part, out num) || num < 0 || num > 255)
                return false;
        }
        return true;
    }
}
