using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Data.SqlClient;
using LearnSite.DBUtility;  // 假设SqlHelper在这个命名空间下

public partial class Teacher_skdjshow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        Response.Charset = "utf-8";
        
        if (!IsPostBack)
        {
            if (Request.QueryString["id"] != null)
            {
                int id = Convert.ToInt32(Request.QueryString["id"]);
                LoadData(id);
            }
            GVSkdj.DataKeyNames = new string[] { "Ssid" }; // 确保设置正确
            BindGVSkdj(); 
        }
    }

    /*private void BindGridView()
    {
        LearnSite.BLL.Skdj bll = new LearnSite.BLL.Skdj();
        DataSet ds = bll.GetAllList();
        
        if (ds != null && ds.Tables.Count > 0)
        {
            GVSkdj.DataSource = ds.Tables[0];
            GVSkdj.DataBind();
        }
    }*/
    protected void ButtonReturn_Click(object sender, EventArgs e)
    {
    // 在这里添加返回逻辑
        Response.Redirect("../teacher/teachermanage.aspx");  // 将 "PreviousPage.aspx" 替换为你想要返回的页面
    }

    protected void GVSkdj_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Text = Server.HtmlEncode(e.Row.Cells[i].Text);
            }
        }
    }
protected void btnDelete_Click(object sender, EventArgs e)
{
    LinkButton btn = (LinkButton)sender;
    int id = Convert.ToInt32(btn.CommandArgument);
    
    LearnSite.BLL.Skdj bll = new LearnSite.BLL.Skdj();
    if (bll.Delete(id))
    {
        // 删除成功，重新绑定数据
        BindGVSkdj();
        //ShowMessage("删除成功！");
    }
    else
    {
        ShowMessage("删除失败！");
    }
}

protected void btnEdit_Click(object sender, EventArgs e)
{
    LinkButton btn = (LinkButton)sender;
    int id = Convert.ToInt32(btn.CommandArgument);
    
    // 跳转到编辑页面，传递ID参数
    Response.Redirect("skdjedit.aspx?id=" + id.ToString());
}
private void ShowMessage(string msg)
{
    ClientScript.RegisterStartupScript(GetType(), "alert", string.Format("alert('{0}');", msg), true);
}    
    protected void GVSkdj_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GVSkdj.PageIndex = e.NewPageIndex;
        BindGVSkdj();
    }
   
private void LoadData(int id)
{
    LearnSite.BLL.Skdj bll = new LearnSite.BLL.Skdj();
    LearnSite.Model.Skdj model = bll.GetModel(id);
    
    if (model != null)
    {
        // 将数据绑定到控件
        txtField1.Text = model.Ssctitle;
        // 其他字段绑定
    }
}

// 这个方法暂时不需要，因为使用GridView的内联编辑功能
// protected void btnSave_Click(object sender, EventArgs e)
// {
//     LearnSite.Model.Skdj model = new LearnSite.Model.Skdj();
//     // 从控件获取数据并赋值给model
//     
//     LearnSite.BLL.Skdj bll = new LearnSite.BLL.Skdj();
//     if (bll.Update(model))
//     {
//         // 更新成功，返回列表页
//         Response.Redirect("skdjshow.aspx");
//     }
//     else
//     {
//         ShowMessage("保存失败！");
//     }
// }
    private string ConvertToUTF8(string input)
    {
        // 假设原始编码是 GB2312
        byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(input);
        return Encoding.UTF8.GetString(bytes);
     }
    
// 删除重复的绑定方法，统一使用BindGVSkdj()
// private void BindGridView()
// {
//     // 此方法已不再使用，使用统一的BindGVSkdj()方法
// }

   // 添加年份筛选下拉列表的事件处理程序
    protected void ddlYearFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGVSkdj();
    }
    // 导出当前年份数据
protected void btnExportCurrent_Click(object sender, EventArgs e)
{
    LearnSite.BLL.Skdj bll = new LearnSite.BLL.Skdj();
    bll.SkdjExcel(true);
}

protected void btnExportAll_Click(object sender, EventArgs e)
{
    LearnSite.BLL.Skdj bll = new LearnSite.BLL.Skdj();
    bll.SkdjExcel(false);
}

protected void GVSkdj_RowEditing(object sender, GridViewEditEventArgs e)
{
    GVSkdj.EditIndex = e.NewEditIndex;
    BindGVSkdj();
}

protected void GVSkdj_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
{
    GVSkdj.EditIndex = -1;
    BindGVSkdj();
}

protected void GVSkdj_RowUpdating(object sender, GridViewUpdateEventArgs e)
{
    try
    {
        // 获取编辑行的所有控件
        GridViewRow row = GVSkdj.Rows[e.RowIndex];
        int id = Convert.ToInt32(GVSkdj.DataKeys[e.RowIndex].Value);
        
        // 获取所有编辑字段的值
        TextBox txtYear = (TextBox)row.FindControl("txtYear");
        TextBox txtMonth = (TextBox)row.FindControl("txtMonth");
        TextBox txtDay = (TextBox)row.FindControl("txtDay");
        TextBox txtWeek = (TextBox)row.FindControl("txtWeek");
        TextBox txtSession = (TextBox)row.FindControl("txtSession");
        TextBox txtGrade = (TextBox)row.FindControl("txtGrade");
        TextBox txtClass = (TextBox)row.FindControl("txtClass");
        TextBox txtTitle = (TextBox)row.FindControl("txtTitle");
        TextBox txtTeacher = (TextBox)row.FindControl("txtTeacher");
        TextBox txtNote = (TextBox)row.FindControl("txtNote");
        
        // 验证所有控件是否找到
        if (txtYear != null && txtMonth != null && txtDay != null && txtWeek != null && 
            txtSession != null && txtGrade != null && txtClass != null && txtTitle != null && 
            txtTeacher != null && txtNote != null)
        {
            // 获取所有字段的值
            int year = Convert.ToInt32(txtYear.Text.Trim());
            int month = Convert.ToInt32(txtMonth.Text.Trim());
            int day = Convert.ToInt32(txtDay.Text.Trim());
            string week = txtWeek.Text.Trim();
            string session = txtSession.Text.Trim();
            int grade = Convert.ToInt32(txtGrade.Text.Trim());
            int classNum = Convert.ToInt32(txtClass.Text.Trim());
            string title = txtTitle.Text.Trim();
            string teacher = txtTeacher.Text.Trim();
            string note = txtNote.Text.Trim();
            
            // 创建日期对象
            DateTime date = new DateTime(year, month, day);
            
            // 直接使用数据库连接进行更新，确保更新真正执行
            string sql = @"UPDATE Skdj SET 
                        Ssyear=@Ssyear, Ssmonth=@Ssmonth, Ssday=@Ssday, 
                        Ssweek=@Ssweek, Ssession=@Ssession, Ssgrade=@Ssgrade, 
                        Ssclass=@Ssclass, Ssctitle=@Ssctitle, Sstname=@Sstname, 
                        Ssnotes=@Ssnotes, Ssdate=@Ssdate 
                        WHERE Ssid=@Ssid";
            
            string connString = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
            
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Ssyear", year);
                    cmd.Parameters.AddWithValue("@Ssmonth", month);
                    cmd.Parameters.AddWithValue("@Ssday", day);
                    cmd.Parameters.AddWithValue("@Ssweek", week);
                    cmd.Parameters.AddWithValue("@Ssession", session);
                    cmd.Parameters.AddWithValue("@Ssgrade", grade);
                    cmd.Parameters.AddWithValue("@Ssclass", classNum);
                    cmd.Parameters.AddWithValue("@Ssctitle", title);
                    cmd.Parameters.AddWithValue("@Sstname", teacher);
                    cmd.Parameters.AddWithValue("@Ssnotes", note);
                    cmd.Parameters.AddWithValue("@Ssdate", date);
                    cmd.Parameters.AddWithValue("@Ssid", id);
                    
                    int rowsAffected = cmd.ExecuteNonQuery();
                    
                    if (rowsAffected > 0)
                    {
                        // 更新成功
                        GVSkdj.EditIndex = -1;
                        BindGVSkdj();
                        ShowMessage("更新成功！");
                    }
                    else
                    {
                        ShowMessage("更新失败！可能记录不存在。");
                    }
                }
            }
        }
        else
        {
            ShowMessage("未找到所有编辑控件！");
        }
    }
    catch (Exception ex)
    {
        ShowMessage("更新时发生错误：" + ex.Message);
    }
}
private void BindGVSkdj()
{
    try
    {
        // 使用BLL层获取数据，确保数据一致性
        LearnSite.BLL.Skdj bll = new LearnSite.BLL.Skdj();
        DataSet ds = bll.GetAllList();
        
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            DataView dv = dt.DefaultView;
            
            // 根据下拉列表选择筛选条件
            if (ddlYearFilter.SelectedValue == "current")
            {
                int currentYear = DateTime.Now.Year;
                dv.RowFilter = "Ssyear = " + currentYear.ToString();
            }
            else
            {
                dv.RowFilter = ""; // 显示所有记录
            }
            
            // 按Ssid降序排序
            dv.Sort = "Ssid DESC";
            DataTable sortedTable = dv.ToTable();
            
            // 添加RowNumber列
            if(!sortedTable.Columns.Contains("RowNumber"))
            {
                sortedTable.Columns.Add("RowNumber", typeof(int));
            }
            
            // 计算序号
            for (int i = 0; i < sortedTable.Rows.Count; i++)
            {
                sortedTable.Rows[i]["RowNumber"] = i + 1;
            }
            
            // 绑定数据
            GVSkdj.DataSource = sortedTable;
            GVSkdj.DataBind();
            
            // 调试信息
            System.Diagnostics.Debug.WriteLine("数据绑定成功，记录数: " + sortedTable.Rows.Count + ", 筛选条件: " + ddlYearFilter.SelectedValue);
        }
        else
        {
            GVSkdj.DataSource = null;
            GVSkdj.DataBind();
            System.Diagnostics.Debug.WriteLine("没有找到数据记录");
        }
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine("数据绑定失败: " + ex.Message);
        GVSkdj.DataSource = null;
        GVSkdj.DataBind();
    }
}
}
