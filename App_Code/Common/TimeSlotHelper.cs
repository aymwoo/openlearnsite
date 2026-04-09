using System;
using System.Data;
using System.Data.SqlClient;
using LearnSite.DBUtility;

namespace LearnSite.Common
{
    /// <summary>
    /// 节次判断辅助类
    /// 提供统一的节次获取方法，供教师和学生登录时共同使用
    /// </summary>
    public static class TimeSlotHelper
    {
        /// <summary>
        /// 获取当前节次
        /// 使用TimeSlots表作为统一数据源
        /// </summary>
        /// <returns>节次名称（如"1"、"2"等），无法判断则返回"其他"</returns>
        public static string GetCurrentTimeSlot()
        {
            try
            {
                DateTime now = DateTime.Now;
                TimeSpan currentTime = now.TimeOfDay;

                // 从TimeSlots表获取当前时间段的节次信息，考虑提前15分钟进入下一节课
                // TimeSlots是全局的时间段配置，不限制教师或班级
                string sql = @"
                    SELECT TOP 1 CAST(SlotID AS VARCHAR(10)) as SlotName
                    FROM TimeSlots
                    WHERE StartTime IS NOT NULL
                      AND EndTime IS NOT NULL
                      AND DATEADD(MINUTE, -15, StartTime) <= @CurrentTime
                      AND EndTime >= @CurrentTime
                    ORDER BY DisplayOrder";

                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@CurrentTime", SqlDbType.Time)
                };
                parameters[0].Value = currentTime;

                object result = DbHelperSQL.GetSingle(sql, parameters);

                if (result != null)
                {
                    return result.ToString();
                }

                // 如果当前时间没有匹配的课程，查找最近的已结束课程节次
                sql = @"
                    SELECT TOP 1 CAST(SlotID AS VARCHAR(10)) as SlotName
                    FROM TimeSlots
                    WHERE StartTime IS NOT NULL
                      AND DATEADD(MINUTE, -15, StartTime) <= @CurrentTime
                    ORDER BY DisplayOrder DESC";

                result = DbHelperSQL.GetSingle(sql, parameters);
                if (result != null)
                {
                    return result.ToString();
                }

                // 如果TimeSlots表中没有配置，使用默认时间段映射
                return GetDefaultTimeSlot(currentTime);
            }
            catch (Exception ex)
            {
                // 记录错误日志
                // LogHelper.WriteLog("获取当前节次出错：" + ex.Message);
                return GetDefaultTimeSlot(DateTime.Now.TimeOfDay);
            }
        }

        /// <summary>
        /// 根据时间返回默认节次（当TimeSlots表中没有配置时使用）
        /// </summary>
        /// <param name="currentTime">当前时间</param>
        /// <returns>节次名称</returns>
        private static string GetDefaultTimeSlot(TimeSpan currentTime)
        {
            int hour = currentTime.Hours;
            int minute = currentTime.Minutes;

            // 常见上课时间段映射
            if (hour == 8 && minute >= 0 && minute < 55)
                return "1";
            else if (hour == 8 && minute >= 55 && hour == 9 && minute < 40)
                return "2";
            else if (hour == 10 && minute >= 0 && minute < 45)
                return "3";
            else if (hour == 10 && minute >= 55 && hour == 11 && minute < 40)
                return "4";
            else if (hour == 14 && minute >= 0 && minute < 45)
                return "5";
            else if (hour == 14 && minute >= 55 && hour == 15 && minute < 40)
                return "6";
            else if (hour == 16 && minute >= 0 && minute < 45)
                return "7";
            else if (hour == 16 && minute >= 55 && hour == 17 && minute < 40)
                return "8";
            else if (hour >= 18 && hour < 21)
                return "9";  // 晚自习或其他
            else
                return "其他";
        }

        /// <summary>
        /// 检查当前时间是否为上课时间（周一至周五）
        /// </summary>
        /// <returns>true:是上课时间; false:不是上课时间</returns>
        public static bool IsClassTime()
        {
            DayOfWeek dayOfWeek = DateTime.Now.DayOfWeek;
            // 周六、周日不是上课时间
            if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }

            // 获取当前节次
            string timeSlot = GetCurrentTimeSlot();
            // 如果节次为"其他"，说明不在上课时间段
            return timeSlot != "其他";
        }
    }
}
