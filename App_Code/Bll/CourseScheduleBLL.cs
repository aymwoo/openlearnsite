using System;
using System.Data;
using System.Collections.Generic;

/// <summary>
/// 课程表业务逻辑层
/// </summary>
public class CourseScheduleBLL
{
    private CourseScheduleDAL dal;

    public CourseScheduleBLL()
    {
        dal = new CourseScheduleDAL();
    }

    public DataTable GetClassList()
    {
        try
        {
            return dal.GetClassList();
        }
        catch (Exception ex)
        {
            throw new Exception("获取班级列表失败: " + ex.Message);
        }
    }

    public DataTable GetScheduleByYear(int year)
    {
        try
        {
            if (year <= 0)
            {
                throw new ArgumentException("无效的学年");
            }
            return dal.GetScheduleByYear(year, 1); // 默认获取1期数据
        }
        catch (Exception ex)
        {
            throw new Exception("获取课程表数据失败: " + ex.Message);
        }
    }

    public DataTable GetScheduleByYearAndTerm(int year, int term)
    {
        try
        {
            if (year <= 0)
            {
                throw new ArgumentException("无效的学年");
            }
            if (term != 1 && term != 2)
            {
                throw new ArgumentException("学期只能是1或2");
            }
            return dal.GetScheduleByYear(year, term);
        }
        catch (Exception ex)
        {
            throw new Exception("获取课程表数据失败: " + ex.Message);
        }
    }

    public DataTable GetScheduleByYearTermAndSubject(int year, int term, string subject)
    {
        return GetScheduleByYearTermAndSubject(year, term, subject, 0);
    }

    public DataTable GetScheduleByYearTermAndSubject(int year, int term, string subject, int teacherID)
    {
        try
        {
            if (year <= 0)
            {
                throw new ArgumentException("无效的学年");
            }
            if (term != 1 && term != 2)
            {
                throw new ArgumentException("学期只能是1或2");
            }
            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentException("科目不能为空");
            }
            return dal.GetScheduleByYearAndSubject(year, term, subject, teacherID);
        }
        catch (Exception ex)
        {
            throw new Exception("获取课程表数据失败: " + ex.Message);
        }
    }

    public bool SaveSchedule(int year, int term, List<CourseScheduleModel> schedules)
    {
        try
        {
            if (year <= 0)
            {
                throw new ArgumentException("学年必须大于0");
            }
            if (term != 1 && term != 2)
            {
                throw new ArgumentException("学期只能是1或2");
            }
            
            if (schedules == null || schedules.Count == 0)
            {
                throw new ArgumentException("课程表数据不能为空");
            }

            // 验证每个schedule对象并设置学期
            foreach (CourseScheduleModel schedule in schedules)
            {
                schedule.Term = term; // 设置学期
                
                if (string.IsNullOrEmpty(schedule.ClassName))
                {
                    throw new ArgumentException("班级名称不能为空");
                }
                if (schedule.WeekDay < 1 || schedule.WeekDay > 7)
                {
                    throw new ArgumentException("星期值必须在1-7之间");
                }
                if (schedule.TimeSlot < 1)
                {
                    throw new ArgumentException("时间段值必须大于0");
                }
            }

            return dal.SaveSchedule(year, term, schedules);
        }
        catch (Exception ex)
        {
            throw new Exception("保存课程表数据时出错: " + ex.Message);
        }
    }

    // 保留原有方法以保持向后兼容性
    public bool SaveSchedule(int year, List<CourseScheduleModel> schedules)
    {
        return SaveSchedule(year, 1, schedules); // 默认保存到1期
    }
}