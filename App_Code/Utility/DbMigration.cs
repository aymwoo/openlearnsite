using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace LearnSite.DBUtility
{
    /// <summary>
    /// A migration entry describing one schema change.
    /// </summary>
    public class MigrationEntry
    {
        public string Version { get; set; }
        public string Description { get; set; }
        public Action Apply { get; set; }
    }

    /// <summary>
    /// Result of a single migration execution.
    /// </summary>
    public class MigrationResult
    {
        public string Version { get; set; }
        public string Description { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    /// <summary>
    /// Database-backed migration engine.
    /// Tracks applied migrations in the _DbMigrations table and executes pending ones in order.
    /// </summary>
    public class DbMigration
    {
        private const string TableName = "_DbMigrations";

        // ---------------------------------------------------------------
        // Ordered list of all migrations (historical + new).
        // Each entry maps to an existing UpdateGrade static method.
        // ---------------------------------------------------------------
        public static readonly List<MigrationEntry> AllMigrations = new List<MigrationEntry>
        {
            new MigrationEntry { Version = "1.0.0",  Description = "基础字段更新 (updateDatabase / CreateNewTable)",       Apply = () => { UpdateGrade.updateDatabase(); UpdateGrade.CreateNewTable(); } },
            new MigrationEntry { Version = "1.0.5",  Description = "补丁 105 - 字段补充",                                  Apply = UpdateGrade.UpdateTable105  },
            new MigrationEntry { Version = "1.0.6",  Description = "补丁 106",                                             Apply = UpdateGrade.UpdateTable106  },
            new MigrationEntry { Version = "1.0.7",  Description = "补丁 107",                                             Apply = UpdateGrade.UpdateTable107  },
            new MigrationEntry { Version = "1.0.8",  Description = "Html 扩展名更新",                                     Apply = UpdateGrade.UpdateHtmlToHtm },
            new MigrationEntry { Version = "1.0.80", Description = "补丁 108",                                             Apply = UpdateGrade.UpdateTable108  },
            new MigrationEntry { Version = "1.0.81", Description = "补丁 1081 - Works.Wgrade/Wterm",                       Apply = UpdateGrade.UpdateTable1081 },
            new MigrationEntry { Version = "1.0.82", Description = "补丁 1082",                                             Apply = UpdateGrade.UpdateTable1082 },
            new MigrationEntry { Version = "1.0.92", Description = "补丁 1092",                                             Apply = UpdateGrade.UpdateTable1092 },
            new MigrationEntry { Version = "1.0.93", Description = "补丁 1093",                                             Apply = UpdateGrade.UpdateTable1093 },
            new MigrationEntry { Version = "1.0.94", Description = "补丁 1094",                                             Apply = UpdateGrade.UpdateTable1094 },
            new MigrationEntry { Version = "1.0.95", Description = "补丁 1095",                                             Apply = UpdateGrade.UpdateTable1095 },
            new MigrationEntry { Version = "1.0.96", Description = "补丁 1096",                                             Apply = UpdateGrade.UpdateTable1096 },
            new MigrationEntry { Version = "1.0.98", Description = "补丁 1098",                                             Apply = UpdateGrade.UpdateTable1098 },
            new MigrationEntry { Version = "1.1.0",  Description = "补丁 1100",                                             Apply = UpdateGrade.UpdateTable1100 },
            new MigrationEntry { Version = "1.1.1",  Description = "补丁 1101",                                             Apply = UpdateGrade.UpdateTable1101 },
            new MigrationEntry { Version = "1.1.2",  Description = "补丁 1102",                                             Apply = UpdateGrade.UpdateTable1102 },
            new MigrationEntry { Version = "1.1.3",  Description = "新增已删除学生表 DelStudents",                         Apply = UpdateGrade.UpdateTable1103 },
            new MigrationEntry { Version = "1.1.5",  Description = "补丁 1105",                                             Apply = UpdateGrade.UpdateTable1105 },
            new MigrationEntry { Version = "1.1.6",  Description = "补丁 1106",                                             Apply = UpdateGrade.UpdateTable1106 },
            new MigrationEntry { Version = "1.1.7",  Description = "补丁 1107",                                             Apply = UpdateGrade.UpdateTable1107 },
            new MigrationEntry { Version = "1.1.8",  Description = "补丁 1108",                                             Apply = UpdateGrade.UpdateTable1108 },
            new MigrationEntry { Version = "1.1.9",  Description = "补丁 1109",                                             Apply = UpdateGrade.UpdateTable1109 },
            new MigrationEntry { Version = "1.1.10", Description = "补丁 1110",                                             Apply = UpdateGrade.UpdateTable1110 },
            new MigrationEntry { Version = "1.2.0",  Description = "补丁 1200",                                             Apply = UpdateGrade.UpdateTable1200 },
            new MigrationEntry { Version = "1.2.1",  Description = "补丁 1201",                                             Apply = UpdateGrade.UpdateTable1201 },
            new MigrationEntry { Version = "1.2.2",  Description = "补丁 1202",                                             Apply = UpdateGrade.UpdateTable1202 },
            new MigrationEntry { Version = "1.2.3",  Description = "补丁 1203",                                             Apply = UpdateGrade.UpdateTable1203 },
            new MigrationEntry { Version = "1.2.5",  Description = "补丁 1205",                                             Apply = UpdateGrade.UpdateTable1205 },
            new MigrationEntry { Version = "1.2.6",  Description = "补丁 1206",                                             Apply = UpdateGrade.UpdateTable1206 },
            new MigrationEntry { Version = "1.2.7",  Description = "补丁 1207",                                             Apply = UpdateGrade.UpdateTable1207 },
            new MigrationEntry { Version = "1.2.8",  Description = "补丁 1208",                                             Apply = UpdateGrade.UpdateTable1208 },
            new MigrationEntry { Version = "1.2.9",  Description = "补丁 1209",                                             Apply = UpdateGrade.UpdateTable1209 },
            new MigrationEntry { Version = "1.2.10", Description = "补丁 1210",                                             Apply = UpdateGrade.UpdateTable1210 },
            new MigrationEntry { Version = "1.2.11", Description = "补丁 1211",                                             Apply = UpdateGrade.UpdateTable1211 },
            new MigrationEntry { Version = "1.2.12", Description = "补丁 1212",                                             Apply = UpdateGrade.UpdateTable1212 },
            new MigrationEntry { Version = "1.2.13", Description = "补丁 1213",                                             Apply = UpdateGrade.UpdateTable1213 },
            new MigrationEntry { Version = "1.2.14", Description = "补丁 1214",                                             Apply = UpdateGrade.UpdateTable1214 },
            new MigrationEntry { Version = "1.2.15", Description = "补丁 1215",                                             Apply = UpdateGrade.UpdateTable1215 },
            new MigrationEntry { Version = "1.2.16", Description = "补丁 1216",                                             Apply = UpdateGrade.UpdateTable1216 },
            new MigrationEntry { Version = "1.2.17", Description = "补丁 1217",                                             Apply = UpdateGrade.UpdateTable1217 },
            new MigrationEntry { Version = "1.2.18", Description = "补丁 1218",                                             Apply = UpdateGrade.UpdateTable1218 },
            new MigrationEntry { Version = "1.2.20", Description = "补丁 1220",                                             Apply = UpdateGrade.UpdateTable1220 },
            new MigrationEntry { Version = "1.2.22", Description = "补丁 1222",                                             Apply = UpdateGrade.UpdateTable1222 },
            new MigrationEntry { Version = "1.2.26", Description = "补丁 1226",                                             Apply = UpdateGrade.UpdateTable1226 },
            new MigrationEntry { Version = "1.2.28", Description = "补丁 1228",                                             Apply = UpdateGrade.UpdateTable1228 },
            new MigrationEntry { Version = "1.2.29", Description = "补丁 1229",                                             Apply = UpdateGrade.UpdateTable1229 },
            new MigrationEntry { Version = "1.2.30", Description = "补丁 1230",                                             Apply = UpdateGrade.UpdateTable1230 },
            new MigrationEntry { Version = "1.2.32", Description = "补丁 1232",                                             Apply = UpdateGrade.UpdateTable1232 },
            new MigrationEntry { Version = "1.2.51", Description = "补丁 1251",                                             Apply = UpdateGrade.UpdateTable1251 },
            new MigrationEntry { Version = "1.2.52", Description = "补丁 1252",                                             Apply = UpdateGrade.UpdateTable1252 },
            new MigrationEntry { Version = "1.2.53", Description = "补丁 1253",                                             Apply = UpdateGrade.UpdateTable1253 },
            new MigrationEntry { Version = "1.2.60", Description = "补丁 1260",                                             Apply = UpdateGrade.UpdateTable1260 },
            new MigrationEntry { Version = "1.2.80", Description = "补丁 1280",                                             Apply = UpdateGrade.UpdateTable1280 },
            new MigrationEntry { Version = "1.3.0",  Description = "补丁 1300",                                             Apply = UpdateGrade.UpdateTable1300 },
            new MigrationEntry { Version = "1.3.20", Description = "补丁 1320",                                             Apply = UpdateGrade.UpdateTable1320 },
            new MigrationEntry { Version = "1.3.30", Description = "补丁 1330",                                             Apply = UpdateGrade.UpdateTable1330 },
            new MigrationEntry { Version = "1.3.32", Description = "补丁 1332",                                             Apply = UpdateGrade.UpdateTable1332 },
            new MigrationEntry { Version = "1.3.33", Description = "补丁 1333",                                             Apply = UpdateGrade.UpdateTable1333 },
            new MigrationEntry { Version = "1.3.35", Description = "补丁 1335",                                             Apply = UpdateGrade.UpdateTable1335 },
            new MigrationEntry { Version = "1.3.36", Description = "补丁 1336",                                             Apply = UpdateGrade.UpdateTable1336 },
            new MigrationEntry { Version = "1.3.37", Description = "补丁 1337",                                             Apply = UpdateGrade.UpdateTable1337 },
            new MigrationEntry { Version = "1.3.38", Description = "补丁 1338",                                             Apply = UpdateGrade.UpdateTable1338 },
            new MigrationEntry { Version = "1.3.39", Description = "补丁 1339",                                             Apply = UpdateGrade.UpdateTable1339 },
            new MigrationEntry { Version = "1.3.50", Description = "补丁 1350",                                             Apply = UpdateGrade.UpdateTable1350 },
            new MigrationEntry { Version = "1.3.52", Description = "补丁 1352",                                             Apply = UpdateGrade.UpdateTable1352 },
            new MigrationEntry { Version = "1.3.60", Description = "补丁 1360",                                             Apply = UpdateGrade.UpdateTable1360 },
            new MigrationEntry { Version = "1.3.65", Description = "补丁 1365",                                             Apply = UpdateGrade.UpdateTable1365 },
            new MigrationEntry { Version = "1.5.0",  Description = "English 打字词库 / 扩展字段",                          Apply = () => { UpdateGrade.UpdateTableEnglish(); UpdateGrade.UpdateTable1500(); } },
            new MigrationEntry { Version = "1.6.0",  Description = "补丁 1600",                                             Apply = UpdateGrade.UpdateTable1600 },
            new MigrationEntry { Version = "1.7.0",  Description = "新增 AIProvider 表及默认数据",                         Apply = UpdateGrade.UpdateTable1700 },
            new MigrationEntry { Version = "1.8.0",  Description = "新增 AISkill 表",                                      Apply = UpdateGrade.UpdateTable1800 },
            new MigrationEntry { Version = "1.8.0.4", Description = "新增在线考试 Exam 主表",                               Apply = UpdateGrade.UpdateTable1804 },
            new MigrationEntry { Version = "1.8.0.5", Description = "新增在线考试题库表 ExamQuestionBank",                 Apply = UpdateGrade.UpdateTable1805 },
            new MigrationEntry { Version = "1.8.0.6", Description = "新增在线考试题目表 ExamQuestion",                     Apply = UpdateGrade.UpdateTable1806 },
            new MigrationEntry { Version = "1.8.0.7", Description = "新增在线考试试卷表 ExamPaper",                        Apply = UpdateGrade.UpdateTable1807 },
            new MigrationEntry { Version = "1.8.0.8", Description = "新增在线考试试卷题目关联表 ExamPaperQuestion",         Apply = UpdateGrade.UpdateTable1808 },
            new MigrationEntry { Version = "1.8.0.9", Description = "新增在线考试答卷表 ExamAnswer",                        Apply = UpdateGrade.UpdateTable1809 },
            new MigrationEntry { Version = "1.8.1.0", Description = "新增在线考试作答日志表 ExamAnswerLog",                 Apply = UpdateGrade.UpdateTable1810 },
            new MigrationEntry { Version = "1.8.1.1", Description = "新增在线考试成绩表 ExamResult",                        Apply = UpdateGrade.UpdateTable1811 },
            new MigrationEntry { Version = "1.8.1.2", Description = "新增考试题型字典表 ExamDictQuestionType",             Apply = UpdateGrade.UpdateTable1812 },
            new MigrationEntry { Version = "1.8.1.3", Description = "新增考试问卷表 ExamSurvey",                            Apply = UpdateGrade.UpdateTable1813 },
            new MigrationEntry { Version = "1.8.1.4", Description = "新增考试问卷答卷表 ExamSurveyAnswer",                  Apply = UpdateGrade.UpdateTable1814 },
            new MigrationEntry { Version = "1.9.0",  Description = "新增 AICustomSkill 表",                                 Apply = UpdateGrade.UpdateTable1900 },
            new MigrationEntry { Version = "1.9.1.1",  Description = "新增学生测验 AI 评估表",                             Apply = UpdateGrade.UpdateTable1911 },
            new MigrationEntry { Version = "1.9.1.2",  Description = "为旧版 Survey 测验新增 AI 评价开关字段",              Apply = UpdateGrade.UpdateTable1912 },
        };

        // ---------------------------------------------------------------
        // Infrastructure
        // ---------------------------------------------------------------

        /// <summary>
        /// Ensures the _DbMigrations tracking table exists.
        /// </summary>
        public static void EnsureMigrationsTable()
        {
            if (!DbHelperSQL.TabExists(TableName))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("CREATE TABLE [dbo].[" + TableName + "] (");
                sb.Append("  [MigrationId]  INT           IDENTITY(1,1) PRIMARY KEY,");
                sb.Append("  [Version]      NVARCHAR(20)  NOT NULL,");
                sb.Append("  [Description]  NVARCHAR(200) NOT NULL,");
                sb.Append("  [AppliedAt]    DATETIME      NOT NULL DEFAULT GETDATE(),");
                sb.Append("  [Success]      BIT           NOT NULL DEFAULT 1");
                sb.Append(")");
                DbHelperSQL.ExecuteSql(sb.ToString());
            }
        }

        /// <summary>
        /// Returns the set of version strings already recorded in _DbMigrations.
        /// </summary>
        public static HashSet<string> GetAppliedVersions()
        {
            EnsureMigrationsTable();
            HashSet<string> applied = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            string sql = "SELECT [Version] FROM [dbo].[" + TableName + "] WHERE [Success] = 1";
            DataSet ds = DbHelperSQL.Query(sql);
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                    applied.Add(row["Version"].ToString());
            }
            return applied;
        }

        /// <summary>
        /// Returns migration entries that have not yet been applied successfully.
        /// </summary>
        public static List<MigrationEntry> GetPendingMigrations()
        {
            HashSet<string> applied = GetAppliedVersions();
            List<MigrationEntry> pending = new List<MigrationEntry>();
            foreach (MigrationEntry m in AllMigrations)
            {
                if (!applied.Contains(m.Version))
                    pending.Add(m);
            }
            return pending;
        }

        /// <summary>
        /// Returns all rows from _DbMigrations ordered by MigrationId ascending.
        /// </summary>
        public static DataTable GetMigrationHistory()
        {
            EnsureMigrationsTable();
            string sql = "SELECT [MigrationId],[Version],[Description],[AppliedAt],[Success] FROM [dbo].[" + TableName + "] ORDER BY [MigrationId] ASC";
            DataSet ds = DbHelperSQL.Query(sql);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return new DataTable();
        }

        /// <summary>
        /// Executes all pending migrations in order and returns a result log.
        /// </summary>
        public static List<MigrationResult> RunAllPending()
        {
            EnsureMigrationsTable();
            List<MigrationEntry> pending = GetPendingMigrations();
            List<MigrationResult> results = new List<MigrationResult>();

            foreach (MigrationEntry m in pending)
            {
                MigrationResult r = ApplyMigration(m);
                results.Add(r);
                if (!r.Success) break; // stop on first failure
            }
            return results;
        }

        /// <summary>
        /// Applies a single migration and records it in _DbMigrations.
        /// </summary>
        public static MigrationResult ApplyMigration(MigrationEntry m)
        {
            MigrationResult result = new MigrationResult
            {
                Version = m.Version,
                Description = m.Description,
                Success = false,
                Message = ""
            };
            try
            {
                m.Apply();
                result.Success = true;
                result.Message = "成功";
                RecordMigration(m.Version, m.Description, true);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
                RecordMigration(m.Version, m.Description, false);
            }
            return result;
        }

        private static void RecordMigration(string version, string description, bool success)
        {
            // Truncate description to 200 chars just in case
            if (description != null && description.Length > 200)
                description = description.Substring(0, 200);
            string sql = string.Format(
                "INSERT INTO [dbo].[{0}] ([Version],[Description],[AppliedAt],[Success]) VALUES (N'{1}',N'{2}',GETDATE(),{3})",
                TableName,
                version.Replace("'", "''"),
                description.Replace("'", "''"),
                success ? 1 : 0);
            DbHelperSQL.ExecuteSql(sql);
        }

        /// <summary>
        /// Backfill: marks all migrations up to the current DB state as applied
        /// without re-running them. Call this once on an existing production DB
        /// so pending list starts clean. Only records versions not already tracked.
        /// </summary>
        public static void BackfillApplied()
        {
            EnsureMigrationsTable();
            // Only backfill if the DB is already fully up-to-date (VersionCheck passes)
            if (!UpdateGrade.VersionCheck()) return;

            HashSet<string> applied = GetAppliedVersions();
            foreach (MigrationEntry m in AllMigrations)
            {
                if (!applied.Contains(m.Version))
                    RecordMigration(m.Version, m.Description, true);
            }
        }
    }
}
