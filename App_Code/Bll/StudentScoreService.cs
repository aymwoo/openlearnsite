using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using LearnSite.DBUtility;

namespace LearnSite.BLL
{
    /// <summary>
    /// 学生综合评分服务类
    /// </summary>
    public class StudentScoreService
    {
        /// <summary>
        /// 获取学生综合得分（0-100分）
        /// </summary>
        /// <param name="studentNum">学号</param>
        /// <returns>综合得分</returns>
        public int GetComprehensiveScore(string studentNum)
        {
            try
            {
                // 获取各项分数
                double attitudeScore = GetAttitudeScore(studentNum);
                double workScore = GetWorkScore(studentNum);
                double examScore = GetExamScore(studentNum);
                double signInScore = GetSignInScore(studentNum);
                
                // 加权平均（表现分30% + 作业分40% + 测验分20% + 签到分10%）
                double comprehensiveScore = attitudeScore * 0.3 + workScore * 0.4 + examScore * 0.2 + signInScore * 0.1;
                
                return Convert.ToInt32(comprehensiveScore);
            }
            catch (Exception ex)
            {
                Console.WriteLine("计算综合得分时出错: " + ex.Message);
                return 0;
            }
        }
        
        /// <summary>
        /// 获取学生表现分（最近30天）
        /// </summary>
        /// <param name="studentNum">学号</param>
        /// <returns>表现分（0-100）</returns>
        public double GetAttitudeScore(string studentNum)
        {
            try
            {
                // 获取最近30天的表现分
                string sql = @"SELECT ISNULL(AVG(CAST(Qattitude AS DECIMAL(10,2))), 0) AS AvgAttitude
                               FROM Signin 
                               WHERE Qnum = @StudentNum 
                               AND Qdate >= DATEADD(DAY, -30, GETDATE())";
                
                SqlParameter[] parameters = {
                    new SqlParameter("@StudentNum", SqlDbType.NVarChar, 50)
                };
                parameters[0].Value = studentNum;
                
                object result = DbHelperSQL.GetSingle(sql, parameters);
                if (result != null && result != DBNull.Value)
                {
                    double avgAttitude = Convert.ToDouble(result);
                    // 直接返回表现分的平均值，因为 Qattitude 已经是 0-100 的分数
                    return avgAttitude;
                }
                return 50; // 默认50分
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取表现分时出错: " + ex.Message);
                return 50;
            }
        }
        
        /// <summary>
        /// 获取学生作业分（最近30天）
        /// </summary>
        /// <param name="studentNum">学号</param>
        /// <returns>作业分（0-100）</returns>
        public double GetWorkScore(string studentNum)
        {
            try
            {
                // 获取最近30天的作业平均分
                string sql = @"SELECT ISNULL(AVG(CAST(Wscore AS DECIMAL(10,2))), 0) AS AvgWorkScore
                               FROM Works 
                               WHERE Wnum = @StudentNum 
                               AND Wdate >= DATEADD(DAY, -30, GETDATE())
                               AND Wscore > 0";
                
                SqlParameter[] parameters = {
                    new SqlParameter("@StudentNum", SqlDbType.NVarChar, 50)
                };
                parameters[0].Value = studentNum;
                
                object result = DbHelperSQL.GetSingle(sql, parameters);
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToDouble(result);
                }
                return 50; // 默认50分
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取作业分时出错: " + ex.Message);
                return 50;
            }
        }
        
        /// <summary>
        /// 获取学生测验分（最近30天）
        /// </summary>
        /// <param name="studentNum">学号</param>
        /// <returns>测验分（0-100）</returns>
        public double GetExamScore(string studentNum)
        {
            try
            {
                // 获取最近30天的测验平均分
                string sql = @"SELECT ISNULL(AVG(CAST(Escore AS DECIMAL(10,2))), 0) AS AvgExamScore
                               FROM Examresult 
                               WHERE Enum = @StudentNum 
                               AND Edate >= DATEADD(DAY, -30, GETDATE())
                               AND Escore > 0";
                
                SqlParameter[] parameters = {
                    new SqlParameter("@StudentNum", SqlDbType.NVarChar, 50)
                };
                parameters[0].Value = studentNum;
                
                object result = DbHelperSQL.GetSingle(sql, parameters);
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToDouble(result);
                }
                return 50; // 默认50分
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取测验分时出错: " + ex.Message);
                return 50;
            }
        }
        
        /// <summary>
        /// 获取学生签到分（最近30天）
        /// </summary>
        /// <param name="studentNum">学号</param>
        /// <returns>签到分（0-100）</returns>
        public double GetSignInScore(string studentNum)
        {
            try
            {
                // 获取最近30天的签到率
                string sql = @"SELECT 
                                COUNT(*) AS TotalDays,
                                SUM(CASE WHEN Qnum IS NOT NULL THEN 1 ELSE 0 END) AS SignInDays
                               FROM (
                                   SELECT DISTINCT CAST(Qdate AS DATE) AS Qdate
                                   FROM Signin 
                                   WHERE Qgrade = (SELECT Sgrade FROM Students WHERE Snum = @StudentNum)
                                   AND Qclass = (SELECT Sclass FROM Students WHERE Snum = @StudentNum)
                                   AND Qdate >= DATEADD(DAY, -30, GETDATE())
                               ) AS Dates
                               LEFT JOIN Signin s ON CAST(s.Qdate AS DATE) = Dates.Qdate AND s.Qnum = @StudentNum";
                
                SqlParameter[] parameters = {
                    new SqlParameter("@StudentNum", SqlDbType.NVarChar, 50)
                };
                parameters[0].Value = studentNum;
                
                DataSet ds = DbHelperSQL.Query(sql, parameters);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int totalDays = Convert.ToInt32(ds.Tables[0].Rows[0]["TotalDays"]);
                    int signInDays = Convert.ToInt32(ds.Tables[0].Rows[0]["SignInDays"]);
                    
                    if (totalDays > 0)
                    {
                        return (double)signInDays / totalDays * 100;
                    }
                }
                return 50; // 默认50分
            }
            catch (Exception ex)
            {
                Console.WriteLine("获取签到分时出错: " + ex.Message);
                return 50;
            }
        }
        
        /// <summary>
        /// 获取学生综合得分详情
        /// </summary>
        /// <param name="studentNum">学号</param>
        /// <returns>包含各项分数的字典</returns>
        public Dictionary<string, double> GetScoreDetails(string studentNum)
        {
            Dictionary<string, double> details = new Dictionary<string, double>();
            
            details["AttitudeScore"] = GetAttitudeScore(studentNum);
            details["WorkScore"] = GetWorkScore(studentNum);
            details["ExamScore"] = GetExamScore(studentNum);
            details["SignInScore"] = GetSignInScore(studentNum);
            details["ComprehensiveScore"] = GetComprehensiveScore(studentNum);
            
            return details;
        }
        
        /// <summary>
        /// 检查学生是否达到资源访问要求
        /// </summary>
        /// <param name="studentNum">学号</param>
        /// <param name="requiredScore">需要的综合得分</param>
        /// <returns>是否可以访问</returns>
        public bool CanAccessResource(string studentNum, int requiredScore)
        {
            int comprehensiveScore = GetComprehensiveScore(studentNum);
            return comprehensiveScore >= requiredScore;
        }
    }
}
