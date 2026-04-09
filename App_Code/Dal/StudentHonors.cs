using System;
using System.Collections.Generic;
using System.Data;
using LearnSite.Model;
using LearnSite.DBUtility;

namespace LearnSite.Dal
{
    /// <summary>
    /// 学生荣誉数据访问层
    /// </summary>
    public class StudentHonors
    {

        #region 添加荣誉记录

        /// <summary>
        /// 添加学生荣誉记录
        /// </summary>
        public int AddStudentHonor(StudentHonor honor)
        {
            string sql = "INSERT INTO StudentHonors (Snum, HonorCode, HonorLevel, EarnDate, EarnCount, Continuous, Term, Remarks) "
                       + "VALUES ('" + honor.Snum + "', '" + honor.HonorCode + "', " + honor.HonorLevel + ", "
                       + "'" + honor.EarnDate.ToString("yyyy-MM-dd HH:mm:ss") + "', " + honor.EarnCount + ", "
                       + honor.Continuous + ", " + (string.IsNullOrEmpty(honor.Term) ? "NULL" : "'" + honor.Term + "'") + ", "
                       + (string.IsNullOrEmpty(honor.Remarks) ? "NULL" : "'" + honor.Remarks + "'") + ")";
            object result = DbHelperSQL.GetSingle(sql);
            if (result != null)
            {
                return Convert.ToInt32(result);
            }
            return 0;
        }

        #endregion

        #region 更新荣誉记录

        /// <summary>
        /// 更新学生荣誉等级
        /// </summary>
        public bool UpdateHonorLevel(string snum, string honorCode, int honorLevel, int earnCount, int continuous)
        {
            string sql = "UPDATE StudentHonors SET HonorLevel = " + honorLevel + ", EarnCount = " + earnCount
                       + ", Continuous = " + continuous + " WHERE Snum = '" + snum + "' AND HonorCode = '" + honorCode + "'";
            return DbHelperSQL.ExecuteSql(sql) > 0;
        }

        #endregion

        #region 查询荣誉记录

        /// <summary>
        /// 获取学生所有荣誉
        /// </summary>
        public List<StudentHonor> GetStudentHonors(string snum)
        {
            string sql = "SELECT * FROM StudentHonors WHERE Snum = '" + snum + "' ORDER BY EarnDate DESC";
            DataSet ds = DbHelperSQL.Query(sql);
            
            List<StudentHonor> honors = new List<StudentHonor>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    honors.Add(GetHonorFromRow(row));
                }
            }
            return honors;
        }

        /// <summary>
        /// 获取学生指定荣誉
        /// </summary>
        public StudentHonor GetStudentHonor(string snum, string honorCode)
        {
            string sql = "SELECT * FROM StudentHonors WHERE Snum = '" + snum + "' AND HonorCode = '" + honorCode + "'";
            DataSet ds = DbHelperSQL.Query(sql);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return GetHonorFromRow(ds.Tables[0].Rows[0]);
            }
            return null;
        }

        /// <summary>
        /// 获取荣誉排行榜
        /// </summary>
        public List<HonorRanking> GetHonorRanking(string honorCode, string syear, int topN)
        {
            string sql = "SELECT TOP " + topN + " s.Snum, s.Sname, s.Sgrade, s.Sclass, sh.HonorLevel, sh.EarnCount, sh.EarnDate "
                       + "FROM Students s INNER JOIN StudentHonors sh ON s.Snum = sh.Snum "
                       + "WHERE sh.HonorCode = '" + honorCode + "' ";

            // 如果指定了学年，添加学年筛选条件
            if (!string.IsNullOrEmpty(syear))
            {
                sql += "AND s.Syear = '" + syear + "' ";
            }

            sql += "ORDER BY sh.HonorLevel DESC, sh.EarnCount DESC, sh.EarnDate";

            DataSet ds = DbHelperSQL.Query(sql);

            List<HonorRanking> rankings = new List<HonorRanking>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    rankings.Add(new HonorRanking
                    {
                        Snum = row["Snum"].ToString(),
                        Sname = row["Sname"].ToString(),
                        Sgrade = row["Sgrade"].ToString(),
                        Sclass = row["Sclass"].ToString(),
                        HonorLevel = Convert.ToInt32(row["HonorLevel"]),
                        EarnCount = Convert.ToInt32(row["EarnCount"]),
                        EarnDate = Convert.ToDateTime(row["EarnDate"])
                    });
                }
            }
            return rankings;
        }

        /// <summary>
        /// 获取指定年级的荣誉排行榜
        /// </summary>
        public List<HonorRanking> GetHonorRankingByGrade(string honorCode, string syear, string sgrade, int topN)
        {
            string sql = "SELECT TOP " + topN + " s.Snum, s.Sname, s.Sgrade, s.Sclass, sh.HonorLevel, sh.EarnCount, sh.EarnDate "
                       + "FROM Students s LEFT JOIN StudentHonors sh ON s.Snum = sh.Snum AND sh.HonorCode = '" + honorCode + "' "
                       + "WHERE s.Sgrade = " + sgrade + " ";

            // 如果指定了学年，添加学年筛选条件
            if (!string.IsNullOrEmpty(syear))
            {
                sql += "AND s.Syear = '" + syear + "' ";
            }

            sql += "ORDER BY sh.HonorLevel DESC, sh.EarnCount DESC, sh.EarnDate";

            System.Diagnostics.Debug.WriteLine("GetHonorRankingByGrade SQL: " + sql);

            DataSet ds = DbHelperSQL.Query(sql);

            System.Diagnostics.Debug.WriteLine("GetHonorRankingByGrade ResultCount: " + (ds != null && ds.Tables.Count > 0 ? ds.Tables[0].Rows.Count : 0));

            List<HonorRanking> rankings = new List<HonorRanking>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    // 只包含有荣誉记录的学生
                    if (row["HonorLevel"] != DBNull.Value)
                    {
                        rankings.Add(new HonorRanking
                        {
                            Snum = row["Snum"].ToString(),
                            Sname = row["Sname"].ToString(),
                            Sgrade = row["Sgrade"].ToString(),
                            Sclass = row["Sclass"].ToString(),
                            HonorLevel = Convert.ToInt32(row["HonorLevel"]),
                            EarnCount = Convert.ToInt32(row["EarnCount"]),
                            EarnDate = ConvertToDate(row["EarnDate"])
                        });
                    }
                }
            }
            return rankings;
        }

        /// <summary>
        /// 获取指定年级和班级的荣誉排行榜
        /// </summary>
        public List<HonorRanking> GetHonorRankingByClass(string honorCode, string syear, string sgrade, string sclass, int topN)
        {
            string sql = "SELECT TOP " + topN + " s.Snum, s.Sname, s.Sgrade, s.Sclass, sh.HonorLevel, sh.EarnCount, sh.EarnDate "
                       + "FROM Students s LEFT JOIN StudentHonors sh ON s.Snum = sh.Snum AND sh.HonorCode = '" + honorCode + "' "
                       + "WHERE s.Sgrade = " + sgrade + " AND s.Sclass = " + sclass + " ";

            // 如果指定了学年，添加学年筛选条件
            if (!string.IsNullOrEmpty(syear))
            {
                sql += "AND s.Syear = '" + syear + "' ";
            }

            sql += "ORDER BY sh.HonorLevel DESC, sh.EarnCount DESC, sh.EarnDate";

            System.Diagnostics.Debug.WriteLine("GetHonorRankingByClass SQL: " + sql);

            DataSet ds = DbHelperSQL.Query(sql);

            System.Diagnostics.Debug.WriteLine("GetHonorRankingByClass ResultCount: " + (ds != null && ds.Tables.Count > 0 ? ds.Tables[0].Rows.Count : 0));

            List<HonorRanking> rankings = new List<HonorRanking>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    // 只包含有荣誉记录的学生
                    if (row["HonorLevel"] != DBNull.Value)
                    {
                        rankings.Add(new HonorRanking
                        {
                            Snum = row["Snum"].ToString(),
                            Sname = row["Sname"].ToString(),
                            Sgrade = row["Sgrade"].ToString(),
                            Sclass = row["Sclass"].ToString(),
                            HonorLevel = Convert.ToInt32(row["HonorLevel"]),
                            EarnCount = Convert.ToInt32(row["EarnCount"]),
                            EarnDate = ConvertToDate(row["EarnDate"])
                        });
                    }
                }
            }
            return rankings;
        }

        /// <summary>
        /// 辅助方法：安全转换为DateTime
        /// </summary>
        private DateTime ConvertToDate(object value)
        {
            if (value == DBNull.Value)
            {
                return DateTime.Now;
            }
            return Convert.ToDateTime(value);
        }

        /// <summary>
        /// 获取班级荣誉统计
        /// </summary>
        public List<ClassHonorStat> GetClassHonorStats(string syear, string sgrade, string sclass)
        {
            string sql = "SELECT hc.HonorType, hc.HonorName, COUNT(DISTINCT sh.Snum) AS StudentCount "
                       + "FROM StudentHonors sh INNER JOIN HonorConfig hc ON sh.HonorCode = hc.HonorCode "
                       + "INNER JOIN Students s ON sh.Snum = s.Snum "
                       + "WHERE s.Syear = '" + syear + "' AND s.Sgrade = '" + sgrade + "' AND s.Sclass = '" + sclass + "' "
                       + "GROUP BY hc.HonorType, hc.HonorName ORDER BY StudentCount DESC";
            DataSet ds = DbHelperSQL.Query(sql);

            List<ClassHonorStat> stats = new List<ClassHonorStat>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    stats.Add(new ClassHonorStat
                    {
                        HonorType = row["HonorType"].ToString(),
                        HonorName = row["HonorName"].ToString(),
                        StudentCount = Convert.ToInt32(row["StudentCount"])
                    });
                }
            }
            return stats;
        }

        /// <summary>
        /// 获取学生荣誉统计
        /// </summary>
        public StudentHonorStat GetStudentHonorStat(string snum)
        {
            string sql = "SELECT COUNT(*) AS TotalHonors, "
                       + "SUM(CASE WHEN HonorLevel = 3 THEN 1 ELSE 0 END) AS GoldCount, "
                       + "SUM(CASE WHEN HonorLevel = 2 THEN 1 ELSE 0 END) AS SilverCount, "
                       + "SUM(CASE WHEN HonorLevel = 1 THEN 1 ELSE 0 END) AS BronzeCount "
                       + "FROM StudentHonors WHERE Snum = '" + snum + "'";
            DataSet ds = DbHelperSQL.Query(sql);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                return new StudentHonorStat
                {
                    TotalHonors = Convert.ToInt32(row["TotalHonors"]),
                    GoldCount = row["GoldCount"] != DBNull.Value ? Convert.ToInt32(row["GoldCount"]) : 0,
                    SilverCount = row["SilverCount"] != DBNull.Value ? Convert.ToInt32(row["SilverCount"]) : 0,
                    BronzeCount = row["BronzeCount"] != DBNull.Value ? Convert.ToInt32(row["BronzeCount"]) : 0
                };
            }
            return new StudentHonorStat();
        }

        #endregion

        #region 获取荣誉配置

        /// <summary>
        /// 获取所有荣誉配置
        /// </summary>
        public List<HonorConfig> GetAllHonorConfigs()
        {
            string sql = "SELECT * FROM HonorConfig WHERE IsActive = 1 ORDER BY SortOrder";
            DataSet ds = DbHelperSQL.Query(sql);

            List<HonorConfig> configs = new List<HonorConfig>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    configs.Add(GetConfigFromRow(row));
                }
            }
            return configs;
        }

        /// <summary>
        /// 根据类型获取荣誉配置
        /// </summary>
        public List<HonorConfig> GetHonorConfigsByType(string honorType)
        {
            string sql = "SELECT * FROM HonorConfig WHERE HonorType = '" + honorType + "' AND IsActive = 1 ORDER BY SortOrder";
            DataSet ds = DbHelperSQL.Query(sql);

            List<HonorConfig> configs = new List<HonorConfig>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    configs.Add(GetConfigFromRow(row));
                }
            }
            return configs;
        }

        /// <summary>
        /// 获取单个荣誉配置
        /// </summary>
        public HonorConfig GetHonorConfig(string honorCode)
        {
            string sql = "SELECT * FROM HonorConfig WHERE HonorCode = '" + honorCode + "'";
            DataSet ds = DbHelperSQL.Query(sql);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return GetConfigFromRow(ds.Tables[0].Rows[0]);
            }
            return null;
        }

        #endregion

        #region 辅助方法

        private StudentHonor GetHonorFromRow(DataRow row)
        {
            return new StudentHonor
            {
                ID = Convert.ToInt32(row["ID"]),
                Snum = row["Snum"].ToString(),
                HonorCode = row["HonorCode"].ToString(),
                HonorLevel = Convert.ToInt32(row["HonorLevel"]),
                EarnDate = Convert.ToDateTime(row["EarnDate"]),
                EarnCount = Convert.ToInt32(row["EarnCount"]),
                Continuous = Convert.ToInt32(row["Continuous"]),
                Term = row["Term"] != DBNull.Value ? row["Term"].ToString() : null,
                Remarks = row["Remarks"] != DBNull.Value ? row["Remarks"].ToString() : null
            };
        }

        private HonorConfig GetConfigFromRow(DataRow row)
        {
            return new HonorConfig
            {
                HonorCode = row["HonorCode"].ToString(),
                HonorName = row["HonorName"].ToString(),
                HonorType = row["HonorType"].ToString(),
                IconClass = row["IconClass"] != DBNull.Value ? row["IconClass"].ToString() : null,
                IconEmoji = row["IconEmoji"] != DBNull.Value ? row["IconEmoji"].ToString() : null,
                Description = row["Description"] != DBNull.Value ? row["Description"].ToString() : null,
                IsActive = Convert.ToBoolean(row["IsActive"]),
                SortOrder = Convert.ToInt32(row["SortOrder"])
            };
        }

        #endregion
    }
}
