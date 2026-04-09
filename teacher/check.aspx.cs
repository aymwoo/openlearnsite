using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Teacher_check : System.Web.UI.Page
{
    LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            verChecking();
            Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "课前检查表";
            if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
            {
                
                LoadClassFilter();
                BindGridData();
            }
            else
            {
                Response.Redirect("~/teacher/index.aspx", true);
            }
            
        }
    }




    
    private void verChecking()
    {
        if (!LearnSite.DBUtility.UpdateGrade.TableCheck())
        {
            string ch = "您的数据库未更新，现在将跳到更新程序UpGrade.aspx，请执行更新，不影响原有数据！";
            LearnSite.Common.WordProcess.Alert(ch, this.Page);
            Response.Redirect("~/upgrade.aspx", false);
        }
    }
    private void LoadClassFilter()
    {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
        {
            string sql = "SELECT DISTINCT ClassName FROM CheckRecords";
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            ddlClass.Items.Add("全部班级");
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ddlClass.Items.Add(reader["ClassName"].ToString());
                }
            }
        }
    }
// 定义排序状态存储
    private string SortExpression
    {
        get { return ViewState["SortExpression"] as string ?? "Id"; }  // 默认按Id排序
        set { ViewState["SortExpression"] = value; }
    }

    private string SortDirection
    {
        get { return ViewState["SortDirection"] as string ?? "DESC"; }  // 默认升序
        set { ViewState["SortDirection"] = value; }
    }

    
    // 修改后的数据绑定方法：支持排序
    private void BindGridData()
    {
        string sql = @"SELECT * FROM CheckRecords WHERE 1=1 " + GetFilterConditions();
        sql += " ORDER BY " + ValidateSortExpression(SortExpression) + " " + ValidateSortDirection(SortDirection);

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            AddFilterParameters(cmd);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            gvRecords.DataSource = dt;
            gvRecords.DataBind();
        }
    }

// 生成序号（支持分页）
  /*   protected void gvRecords_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // 生成序号（支持分页）
            int rowIndex = (gvRecords.PageIndex * gvRecords.PageSize) + e.Row.RowIndex + 1;
            Label lblNumber = (Label)e.Row.FindControl("lblRowNumber");
            if (lblNumber != null)
            {
                lblNumber.Text = rowIndex.ToString();
            }
        } 
        
    
        
    } 

    protected void gvRecords_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // 计算序号（考虑分页）
            int rowIndex = (gvRecords.PageIndex * gvRecords.PageSize) + e.Row.RowIndex + 1;
            Label lblNumber = new Label();
            lblNumber.Text = rowIndex.ToString();
            e.Row.Cells[1].Controls.Add(lblNumber);  // 序号列是第0列
        }
    } */

protected void gvRecords_RowDataBound(object sender, GridViewRowEventArgs e)
{
    // 1. 生成序号（支持分页）
    if (e.Row.RowType == DataControlRowType.DataRow)
    {
        int rowIndex = (gvRecords.PageIndex * gvRecords.PageSize) + e.Row.RowIndex + 1;
        Label lblNumber = (Label)e.Row.FindControl("lblRowNumber");
        if (lblNumber != null)
        {
            lblNumber.Text = rowIndex.ToString();
        }
    }

    // 2. 动态添加排序箭头（仅处理标题行）
    if (e.Row.RowType == DataControlRowType.Header)
    {
        // 获取当前排序字段和方向
        string currentSortField = ViewState["SortExpression"] != null ? 
            ViewState["SortExpression"].ToString() : "Id";
        string currentSortDirection = ViewState["SortDirection"] != null ? 
            ViewState["SortDirection"].ToString() : "ASC";

        // 遍历所有标题单元格
        foreach (TableCell cell in e.Row.Cells)
        {
            if (cell is DataControlFieldHeaderCell)
            {
                DataControlFieldHeaderCell headerCell = (DataControlFieldHeaderCell)cell;
                DataControlField field = headerCell.ContainingField;

                // 检查是否为可排序列
                if (!string.IsNullOrEmpty(field.SortExpression))
                {
                    // 获取标题中的 LinkButton（如果存在）
                    LinkButton linkButton = null;
                    if (headerCell.Controls.Count > 0)
                    {
                        linkButton = headerCell.Controls[0] as LinkButton;
                    }

                    // 如果存在 LinkButton，则直接修改其文本
                    if (linkButton != null)
                    {
                        string arrow = "";
                        if (field.SortExpression == currentSortField)
                        {
                            arrow = (currentSortDirection == "ASC") ? "▲" : "👇";
                        }
                        linkButton.Text = string.Format("{0} <span class='sort-arrow'>{1}</span>", 
                            linkButton.Text, 
                            arrow
                        );
                    }
                    else
                    {
                        // 没有 LinkButton，直接修改标题文本
                        string originalText = headerCell.Text;
                        string arrow = (field.SortExpression == currentSortField) ? 
                            (currentSortDirection == "ASC" ? "▲" : "👇") : "";
                        headerCell.Text = string.Format("{0} <span class='sort-arrow'>{1}</span>", 
                            originalText, 
                            arrow
                        );
                    }
                }
            }
        }
    }
}



// 处理排序事件
    protected void gvRecords_Sorting(object sender, GridViewSortEventArgs e)
    {
        // 判断是否同一列，切换排序方向
        if (e.SortExpression == SortExpression)
        {
            SortDirection = (SortDirection == "ASC") ? "DESC" : "ASC";
        }
        else
        {
            SortExpression = e.SortExpression;
            SortDirection = "DESC";
        }
        BindGridData();
    }
    // 构建筛选条件
    private string GetFilterConditions()
    {
        string conditions = "";
        if (ddlClass.SelectedIndex > 0)
            conditions += " AND ClassName = @ClassName";
        if (!string.IsNullOrEmpty(txtName.Text.Trim()))
            conditions += " AND suser LIKE @Name";
        if (!string.IsNullOrEmpty(txtStartDate.Text))
            conditions += " AND SubmitTime >= @StartDate";
        if (!string.IsNullOrEmpty(txtEndDate.Text))
            conditions += " AND SubmitTime <= @EndDate";
        
        // 检查项条件
        foreach (ListItem item in cblChecks.Items)
        {
            if (item.Selected)
                conditions += string.Format(" AND {0} = 1", item.Value);
        }
        return conditions;
    }

    // 添加参数
    private void AddFilterParameters(SqlCommand cmd)
    {
        if (ddlClass.SelectedIndex > 0)
            cmd.Parameters.AddWithValue("@ClassName", ddlClass.SelectedValue);
        if (!string.IsNullOrEmpty(txtName.Text.Trim()))
            cmd.Parameters.AddWithValue("@Name", "%" + txtName.Text.Trim() + "%");
        if (!string.IsNullOrEmpty(txtStartDate.Text))
            cmd.Parameters.AddWithValue("@StartDate", DateTime.Parse(txtStartDate.Text));
        if (!string.IsNullOrEmpty(txtEndDate.Text))
            cmd.Parameters.AddWithValue("@EndDate", DateTime.Parse(txtEndDate.Text).AddDays(1));
    }

// 处理行编辑事件
protected void gvRecords_RowEditing(object sender, GridViewEditEventArgs e)
{
    gvRecords.EditIndex = e.NewEditIndex; // 进入编辑模式
    BindGridData(); // 重新绑定数据
}
protected void gvRecords_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvRecords.EditIndex = -1;
        BindGridData();
    }

    // 分页
    protected void gvRecords_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRecords.PageIndex = e.NewPageIndex;
        BindGridData();
         
    }
// 验证排序字段（防止SQL注入）
    private string ValidateSortExpression(string expression)
    {
        string[] allowedColumns = { "Id", "ClassName", "sname", "suser", "SubmitTime","IpAddress","PcName" };
        return (Array.IndexOf(allowedColumns, expression) >= 0) ? expression : "Id";
    }

    // 验证排序方向
    private string ValidateSortDirection(string direction)
    {
        return (direction == "ASC" || direction == "DESC") ? direction : "DESC";
    }

// 示例：添加“还原”按钮
protected void btnResetSort_Click(object sender, EventArgs e)
{
    SortExpression = "Id";
    SortDirection = "DESC";
    BindGridData();
}

protected void btnFilter_Click(object sender, EventArgs e)
    {
        BindGridData();
    }
    // 编辑保存
    protected void gvRecords_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int id = (int)gvRecords.DataKeys[e.RowIndex].Value;
        GridViewRow row = gvRecords.Rows[e.RowIndex];

        // 获取所有可编辑字段
        TextBox txtComment = (TextBox)row.FindControl("txtComment");
        TextBox suser = (TextBox)row.FindControl("suser");
        CheckBox chkHasRubbish = (CheckBox)row.FindControl("chkHasRubbish");
        CheckBox chkDrawerClean = (CheckBox)row.FindControl("chkDrawerClean");
        CheckBox chkEquipmentArranged = (CheckBox)row.FindControl("chkEquipmentArranged");
        CheckBox chkChairAdjusted = (CheckBox)row.FindControl("chkChairAdjusted");
        CheckBox chkKeyboardMouseDamaged = (CheckBox)row.FindControl("chkKeyboardMouseDamaged");
        CheckBox chkCableUnplugged = (CheckBox)row.FindControl("chkCableUnplugged");
        CheckBox chkPeripheralUnplugged = (CheckBox)row.FindControl("chkPeripheralUnplugged");
        CheckBox chkScreenMarked = (CheckBox)row.FindControl("chkScreenMarked");

        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
        {
            string sql = @"
                UPDATE CheckRecords SET 
                    Comment = @Comment,
                    suser=@suser,
                    HasRubbish = @HasRubbish,
                    DrawerClean = @DrawerClean,
                    EquipmentArranged = @EquipmentArranged,
                    ChairAdjusted = @ChairAdjusted,
                    KeyboardMouseDamaged = @KeyboardMouseDamaged,
                    CableUnplugged = @CableUnplugged,
                    PeripheralUnplugged = @PeripheralUnplugged,
                    ScreenMarked = @ScreenMarked
                WHERE Id = @Id";

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Comment", txtComment.Text);
            cmd.Parameters.AddWithValue("@suser", suser.Text);
            cmd.Parameters.AddWithValue("@HasRubbish", chkHasRubbish.Checked);
            cmd.Parameters.AddWithValue("@DrawerClean", chkDrawerClean.Checked);
            cmd.Parameters.AddWithValue("@EquipmentArranged", chkEquipmentArranged.Checked);
            cmd.Parameters.AddWithValue("@ChairAdjusted", chkChairAdjusted.Checked);
            cmd.Parameters.AddWithValue("@KeyboardMouseDamaged", chkKeyboardMouseDamaged.Checked);
            cmd.Parameters.AddWithValue("@CableUnplugged", chkCableUnplugged.Checked);
            cmd.Parameters.AddWithValue("@PeripheralUnplugged", chkPeripheralUnplugged.Checked);
            cmd.Parameters.AddWithValue("@ScreenMarked", chkScreenMarked.Checked);
            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
        gvRecords.EditIndex = -1;
        BindGridData();
    }

    // 导出Excel
    protected void btnExport_Click(object sender, EventArgs e)
    {
        DataTable dt = GetFilteredData();
        string csv = DataTableToCsv(dt);
        
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=Export.csv");
        Response.Charset = "UTF-8";
        Response.ContentType = "text/csv";
        Response.Output.Write(csv);
        Response.Flush();
        Response.End();
    }

    // 批量删除
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvRecords.Rows)
        {
            CheckBox chk = (CheckBox)row.FindControl("chkSelect");
            if (chk != null && chk.Checked)
            {
                int id = (int)gvRecords.DataKeys[row.RowIndex].Value;
                DeleteRecord(id);
            }
        }
        BindGridData();
    }



    // 全选/反选功能
    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkAll = (CheckBox)gvRecords.HeaderRow.FindControl("chkSelectAll");
        foreach (GridViewRow row in gvRecords.Rows)
        {
            CheckBox chk = (CheckBox)row.FindControl("chkSelect");
            if (chk != null) chk.Checked = chkAll.Checked;
        }
    }






    // 辅助方法：获取筛选数据
    private DataTable GetFilteredData()
    {
        string sql = @"SELECT * FROM CheckRecords WHERE 1=1" + GetFilterConditions();
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            AddFilterParameters(cmd);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }
    }

    // 辅助方法：DataTable转CSV
    private string DataTableToCsv(DataTable dt)
    {
        using (StringWriter sw = new StringWriter())
        {
            foreach (DataColumn col in dt.Columns)
                sw.Write(col.ColumnName + ",");
            sw.WriteLine();

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn col in dt.Columns)
                    sw.Write(row[col].ToString().Replace(",", ";") + ",");
                sw.WriteLine();
            }
            return sw.ToString();
        }
    }

    // 辅助方法：删除记录
    private void DeleteRecord(int id)
    {
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString))
        {
            string sql = "DELETE FROM CheckRecords WHERE Id = @Id";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }

    protected void btnReturn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/teacher/teachermanage.aspx", false);
    }
}
