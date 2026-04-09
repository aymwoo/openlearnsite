using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using LearnSite.DBUtility;
using System.Data.SqlClient;

public partial class kcb_ClassInfo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.HeaderEncoding = System.Text.Encoding.UTF8;
            BindClassInfo();
        }
    }

    private void BindClassInfo()
    {
        string sql = @"SELECT ClassID, ClassName, Grade, Class, IsActive 
                  FROM ClassInfo 
                  ORDER BY ClassID";
    
        DataTable dt = DbHelperSQL.Query(sql).Tables[0];
        gvCourseSchedule.DataSource = dt;
        gvCourseSchedule.DataBind();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = @"INSERT INTO ClassInfo (ClassName, Grade, Class, IsActive)
                          VALUES ('三1', 3, 1, 1)";
            
            int affectedRows = DbHelperSQL.ExecuteSql(sql);
            
            if (affectedRows > 0)
            {
                lblMessage.Text = "默认班级添加成功，请编辑详细信息";
                lblMessage.CssClass = "message success";
                BindClassInfo();
            }
            else
            {
                lblMessage.Text = "班级添加失败";
                lblMessage.CssClass = "message error";
            }
            lblMessage.Visible = true;
        }
        catch (Exception ex)
        {
            lblMessage.Text = "添加班级出错: " + ex.Message;
            lblMessage.CssClass = "message error";
            lblMessage.Visible = true;
        }
    }

    protected void gvCourseSchedule_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try {
            gvCourseSchedule.EditIndex = e.NewEditIndex;
            BindClassInfo();
        }
        catch (Exception ex) {
            lblMessage.Text = "编辑失败: " + ex.Message;
            lblMessage.CssClass = "message error";
            lblMessage.Visible = true;
        }
    }

    protected void gvCourseSchedule_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            if (e.RowIndex < 0 || e.RowIndex >= gvCourseSchedule.Rows.Count)
                throw new Exception("无效的行索引");
                
            GridViewRow row = gvCourseSchedule.Rows[e.RowIndex];
            if (row == null)
                throw new Exception("未能获取行对象");
                
            int classId = Convert.ToInt32(gvCourseSchedule.DataKeys[e.RowIndex].Value);
            
            TextBox txtClassName = row.FindControl("txtClassName") as TextBox;
            TextBox txtGrade = row.FindControl("txtGrade") as TextBox;
            TextBox txtClass = row.FindControl("txtClass") as TextBox;
            CheckBox chkActive = row.FindControl("chkActive") as CheckBox;
        
            string className = txtClassName.Text;
            string grade = txtGrade.Text;
            string cls = txtClass.Text;
            bool isActive = chkActive.Checked;
        
            string sql = @"UPDATE ClassInfo 
                      SET ClassName = @ClassName,
                          Grade = @Grade,
                          Class = @Class,
                          IsActive = @IsActive
                      WHERE ClassID = @ClassID";
            
            SqlParameter[] parameters = {
                new SqlParameter("@ClassName", className),
                new SqlParameter("@Grade", grade),
                new SqlParameter("@Class", cls),
                new SqlParameter("@IsActive", isActive),
                new SqlParameter("@ClassID", classId)
            };
    
            int affectedRows = DbHelperSQL.ExecuteSql(sql, parameters);
            
            if (affectedRows > 0)
            {
                lblMessage.Text = "更新成功";
                lblMessage.CssClass = "message success";
                gvCourseSchedule.EditIndex = -1;
                BindClassInfo();
            }
            else
            {
                lblMessage.Text = "更新失败，未找到匹配记录";
                lblMessage.CssClass = "message error";
            }
            lblMessage.Visible = true;
        }
        catch (Exception ex)
        {
            lblMessage.Text = "更新失败: " + ex.Message;
            lblMessage.CssClass = "message error";
            lblMessage.Visible = true;
        }
    }

    protected void gvCourseSchedule_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvCourseSchedule.EditIndex = -1;
        BindClassInfo();
    }

    protected void gvCourseSchedule_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int classId = Convert.ToInt32(gvCourseSchedule.DataKeys[e.RowIndex].Value);
        string sql = "DELETE FROM ClassInfo WHERE ClassID = @ClassID";
        DbHelperSQL.ExecuteSql(sql, new SqlParameter("@ClassID", classId));
        BindClassInfo();
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("CourseSchedule.aspx");
    }
}