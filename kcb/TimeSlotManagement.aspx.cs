using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using LearnSite.DBUtility;
using System.Data.SqlClient;

public partial class kcb_TimeSlotManagement : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.HeaderEncoding = System.Text.Encoding.UTF8;
            BindTimeSlots();
        }
    }

    private void BindTimeSlots()
    {
        try
        {
            string sql = "SELECT SlotID, SlotName, StartTime, EndTime FROM TimeSlots ORDER BY DisplayOrder";
            DataTable dt = DbHelperSQL.Query(sql).Tables[0];
            
            // 验证和清理时间数据，并格式化显示
            foreach (DataRow row in dt.Rows)
            {
                // 验证并格式化StartTime
                if (row["StartTime"] != DBNull.Value)
                {
                    string startTimeStr = row["StartTime"].ToString();
                    DateTime startTime;
                    if (!DateTime.TryParse(startTimeStr, out startTime))
                    {
                        // 如果时间格式无效，设置为默认时间
                        row["StartTime"] = "08:00";
                    }
                    else
                    {
                        // 格式化为HH:mm显示格式
                        row["StartTime"] = startTime.ToString("HH:mm");
                    }
                }
                else
                {
                    row["StartTime"] = "08:00";
                }
                
                // 验证并格式化EndTime
                if (row["EndTime"] != DBNull.Value)
                {
                    string endTimeStr = row["EndTime"].ToString();
                    DateTime endTime;
                    if (!DateTime.TryParse(endTimeStr, out endTime))
                    {
                        // 如果时间格式无效，设置为默认时间
                        row["EndTime"] = "08:45";
                    }
                    else
                    {
                        // 格式化为HH:mm显示格式
                        row["EndTime"] = endTime.ToString("HH:mm");
                    }
                }
                else
                {
                    row["EndTime"] = "08:45";
                }
            }
            
            gvTimeSlots.DataSource = dt;
            gvTimeSlots.DataBind();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "绑定数据时出错: " + ex.Message;
            lblMessage.CssClass = "message error";
            lblMessage.Visible = true;
            
            // 创建一个空的DataTable以防止页面崩溃
            DataTable emptyDt = new DataTable();
            emptyDt.Columns.Add("SlotID", typeof(int));
            emptyDt.Columns.Add("SlotName", typeof(string));
            emptyDt.Columns.Add("StartTime", typeof(string));
            emptyDt.Columns.Add("EndTime", typeof(string));
            
            // 添加一个空行
            DataRow emptyRow = emptyDt.NewRow();
            emptyRow["SlotID"] = 0;
            emptyRow["SlotName"] = "";
            emptyRow["StartTime"] = "";
            emptyRow["EndTime"] = "";
            emptyDt.Rows.Add(emptyRow);
            
            gvTimeSlots.DataSource = emptyDt;
            gvTimeSlots.DataBind();
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            // 修改SQL语句，使用IDENTITY列自动生成SlotID
            string sql = @"INSERT INTO TimeSlots (SlotName, StartTime, EndTime, DisplayOrder) 
                          VALUES ('1', '08:00:00', '08:45:00', 
                          (SELECT ISNULL(MAX(DisplayOrder),0)+1 FROM TimeSlots))";
            
            int affectedRows = DbHelperSQL.ExecuteSql(sql);
            
            if (affectedRows > 0)
            {
                lblMessage.Text = "默认节次添加成功，请编辑详细信息";
                lblMessage.CssClass = "message success";
                BindTimeSlots(); // 重新绑定数据
            }
            else
            {
                lblMessage.Text = "节次添加失败";
                lblMessage.CssClass = "message error";
            }
            lblMessage.Visible = true;
        }
        catch (Exception ex)
        {
            lblMessage.Text = "添加节次出错: " + ex.Message;
            lblMessage.CssClass = "message error";
            lblMessage.Visible = true;
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/kcb/CourseSchedule.aspx");
    }

    protected void gvTimeSlots_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvTimeSlots.EditIndex = e.NewEditIndex;
        BindTimeSlots();
    }

    protected void gvTimeSlots_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            // 添加更严格的索引检查
            if (e.RowIndex < 0 || e.RowIndex >= gvTimeSlots.Rows.Count || 
                gvTimeSlots.DataKeys.Count <= e.RowIndex)
            {
                lblMessage.Text = "无效的行索引，请刷新页面后重试";
                lblMessage.CssClass = "message error";
                lblMessage.Visible = true;
                return;
            }

            GridViewRow row = gvTimeSlots.Rows[e.RowIndex];
            int slotId = Convert.ToInt32(gvTimeSlots.DataKeys[e.RowIndex].Value);
            
            // 获取编辑后的值
            string slotName = ((TextBox)row.Cells[1].Controls[0]).Text;
            string startTimeText = ((TextBox)row.Cells[2].Controls[0]).Text;
            string endTimeText = ((TextBox)row.Cells[3].Controls[0]).Text;

            // 验证时间格式 - 支持多种格式
            DateTime startTime, endTime;
            if (!DateTime.TryParse(startTimeText, out startTime))
            {
                // 尝试解析纯时间格式
                TimeSpan timeSpan;
                if (!TimeSpan.TryParse(startTimeText, out timeSpan))
                {
                    lblMessage.Text = "开始时间格式不正确，请使用HH:mm格式";
                    lblMessage.CssClass = "message error";
                    lblMessage.Visible = true;
                    return;
                }
                else
                {
                    startTime = DateTime.Today.Add(timeSpan);
                }
            }

            if (!DateTime.TryParse(endTimeText, out endTime))
            {
                // 尝试解析纯时间格式
                TimeSpan timeSpan;
                if (!TimeSpan.TryParse(endTimeText, out timeSpan))
                {
                    lblMessage.Text = "结束时间格式不正确，请使用HH:mm格式";
                    lblMessage.CssClass = "message error";
                    lblMessage.Visible = true;
                    return;
                }
                else
                {
                    endTime = DateTime.Today.Add(timeSpan);
                }
            }

            // 验证时间逻辑
            if (endTime <= startTime)
            {
                lblMessage.Text = "结束时间必须晚于开始时间";
                lblMessage.CssClass = "message error";
                lblMessage.Visible = true;
                return;
            }

            // 更新数据库 - 使用标准的时间格式
            string sql = "UPDATE TimeSlots SET SlotName=@SlotName, StartTime=@StartTime, EndTime=@EndTime WHERE SlotID=@SlotID";
            SqlParameter[] parameters = {
                new SqlParameter("@SlotName", slotName),
                new SqlParameter("@StartTime", startTime.ToString("HH:mm:ss")),
                new SqlParameter("@EndTime", endTime.ToString("HH:mm:ss")),
                new SqlParameter("@SlotID", slotId)
            };

            int affectedRows = DbHelperSQL.ExecuteSql(sql, parameters);
            
            if (affectedRows > 0)
            {
                lblMessage.Text = "更新成功";
                lblMessage.CssClass = "message success";
                gvTimeSlots.EditIndex = -1;
                BindTimeSlots();
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

    protected void gvTimeSlots_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        // 取消编辑逻辑
    }

    protected void gvTimeSlots_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int slotId = Convert.ToInt32(gvTimeSlots.DataKeys[e.RowIndex].Value);
        string sql = "DELETE FROM TimeSlots WHERE SlotID=@SlotID";
        DbHelperSQL.ExecuteSql(sql, new SqlParameter("@SlotID", slotId));
        BindTimeSlots();
    }

    protected void gvTimeSlots_SelectedIndexChanged(object sender, EventArgs e)
    {
        // 获取当前选中的行
        GridViewRow row = gvTimeSlots.SelectedRow;
        
        // 这里可以添加处理选中行变更的逻辑
        // 例如：
        // string slotID = gvTimeSlots.DataKeys[row.RowIndex].Value.ToString();
        // lblMessage.Text = "已选择节次: " + slotID;
        // lblMessage.Visible = true;
    }
}