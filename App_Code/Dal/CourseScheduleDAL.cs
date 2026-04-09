using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

/// <summary>
/// 课程表数据访问层
/// </summary>
public class CourseScheduleDAL
{
    private readonly string connectionString;

    public CourseScheduleDAL()
    {
        try
        {
            ConnectionStringSettings connStringSettings = ConfigurationManager.ConnectionStrings["SqlServer"];
            if (connStringSettings == null)
            {
                throw new Exception("未在配置文件中找到SqlServer连接字符串");
            }
            connectionString = connStringSettings.ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("数据库连接字符串为空");
            }

            // 测试连接
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("数据库连接失败: " + ex.Message);
        }
    }

    /// <summary>
    /// Get class list
    /// </summary>
    public DataTable GetClassList()
    {
        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                string sql = @"SELECT DISTINCT Rgrade + '.' + Rclass as ClassName, Rgrade as Grade, Rclass as Class, Rgrade + Rclass as ClassID 
                             FROM Room 
                             WHERE Rgrade IS NOT NULL AND Rclass IS NOT NULL
                             ORDER BY Rgrade, Rclass";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Get class list failed: " + ex.Message);
            }
        }
        return dt;
    }



    /// <summary>
    /// Get schedule by year and term
    /// </summary>
    public DataTable GetScheduleByYear(int year, int term)
    {
        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();

                // 从CourseSchedule表直接获取数据，按学年、学期、科目过滤
                string sql = @"
                    SELECT
                        cs.TimeSlot AS TimeSlot,
                        cs.WeekDay AS WeekDay,
                        cs.ClassName AS ClassName,
                        cs.Subject AS Subject,
                        PARSENAME(REPLACE(cs.ClassName, '-', '.'), 2) AS Grade,
                        PARSENAME(REPLACE(cs.ClassName, '-', '.'), 1) AS ClassID
                    FROM CourseSchedule cs
                    WHERE cs.SchoolYear = @year
                      AND cs.Term = @term
                    ORDER BY cs.WeekDay, cs.TimeSlot";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@term", term);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Get schedule data failed: " + ex.Message);
            }
        }
        return dt;
    }

    /// <summary>
    /// Get schedule by year, term and subject
    /// </summary>
    public DataTable GetScheduleByYearAndSubject(int year, int term, string subject)
    {
        return GetScheduleByYearAndSubject(year, term, subject, 0);
    }

    public DataTable GetScheduleByYearAndSubject(int year, int term, string subject, int teacherID)
    {
        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            try
            {
                conn.Open();

                string sql = @"
                    SELECT
                        cs.TimeSlot AS TimeSlot,
                        cs.WeekDay AS WeekDay,
                        cs.ClassName AS ClassName,
                        cs.Subject AS Subject,
                        cs.TeacherID AS TeacherID,
                        PARSENAME(REPLACE(cs.ClassName, '-', '.'), 2) AS Grade,
                        PARSENAME(REPLACE(cs.ClassName, '-', '.'), 1) AS ClassID
                    FROM CourseSchedule cs
                    WHERE cs.SchoolYear = @year
                      AND cs.Term = @term
                      AND cs.Subject = @subject
                      AND (cs.TeacherID = @teacherID OR @teacherID = 0)
                    ORDER BY cs.WeekDay, cs.TimeSlot";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@year", year);
                    cmd.Parameters.AddWithValue("@term", term);
                    cmd.Parameters.AddWithValue("@subject", subject);
                    cmd.Parameters.AddWithValue("@teacherID", teacherID);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Get schedule data failed: " + ex.Message);
            }
        }
        return dt;
    }

    /// <summary>
    /// 检查指定表中是否存在指定列
    /// </summary>
    private bool CheckColumnExists(SqlConnection conn, string tableName, string columnName,SqlTransaction trans)
    {
        try
        {
            string sql = @"SELECT COUNT(*) FROM sys.columns 
                          WHERE object_id = OBJECT_ID(@tableName) AND name = @columnName";
            using (SqlCommand cmd = new SqlCommand(sql, conn, trans))
            {
                cmd.Parameters.AddWithValue("@tableName", tableName);
                cmd.Parameters.AddWithValue("@columnName", columnName);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
        catch (SqlException ex)
        {
            return false;
        }
    }

    /// <summary>
    /// Save schedule
    /// </summary>
    public bool SaveSchedule(int year, int term, List<CourseScheduleModel> schedules)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            using (SqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    string subject = schedules.Count > 0 ? schedules[0].Subject : "";
                    int teacherID = schedules.Count > 0 ? schedules[0].TeacherID : 0;

                    string deleteSql = "DELETE FROM CourseSchedule WHERE SchoolYear = @year AND Term = @term AND Subject = @subject AND TeacherID = @teacherID";

                    using (SqlCommand cmdDelete = new SqlCommand(deleteSql, conn, trans))
                    {
                        cmdDelete.Parameters.AddWithValue("@year", year);
                        cmdDelete.Parameters.AddWithValue("@term", term);
                        cmdDelete.Parameters.AddWithValue("@subject", subject);
                        cmdDelete.Parameters.AddWithValue("@teacherID", teacherID);
                        cmdDelete.ExecuteNonQuery();
                    }

                    string insertSql = @"INSERT INTO CourseSchedule
                                         (SchoolYear, Term, WeekDay, TimeSlot, ClassName, Subject, TeacherID, CreateTime)
                                         VALUES (@schoolYear, @term, @weekDay, @timeSlot, @className, @subject, @teacherID, GETDATE())";

                    foreach (CourseScheduleModel schedule in schedules)
                    {
                        using (SqlCommand cmdInsert = new SqlCommand(insertSql, conn, trans))
                        {
                            cmdInsert.Parameters.AddWithValue("@schoolYear", year);
                            cmdInsert.Parameters.AddWithValue("@term", term);
                            cmdInsert.Parameters.AddWithValue("@weekDay", schedule.WeekDay);
                            cmdInsert.Parameters.AddWithValue("@timeSlot", schedule.TimeSlot);
                            cmdInsert.Parameters.AddWithValue("@className", schedule.ClassName);
                            cmdInsert.Parameters.AddWithValue("@subject", schedule.Subject ?? (object)DBNull.Value);
                            cmdInsert.Parameters.AddWithValue("@teacherID", schedule.TeacherID);
                            cmdInsert.ExecuteNonQuery();
                        }
                    }

                    trans.Commit();
                    return true;
                }
                catch (SqlException ex)
                {
                    trans.Rollback();
                    return false;
                }
            }
        }
    }



    // 保留原有方法以保持向后兼容性（忽略term参数）
    public bool SaveSchedule(int year, List<CourseScheduleModel> schedules)
    {
        return SaveSchedule(year, 1, schedules); // term参数忽略，因为新表结构没有Term字段
    }

    // 保留原有方法以保持向后兼容性
    public DataTable GetScheduleByYear(int year)
    {
        return GetScheduleByYear(year, 1); // 默认获取该年份数据
    }
}