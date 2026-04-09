using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading;

namespace LearnSite.DBUtility
{
    public class DatabaseConnectionSettings
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }

    public static class DatabaseSetupHelper
    {
        public static string BuildTargetConnectionString(DatabaseConnectionSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            return String.Format("Data Source={0};Initial Catalog={1};uid={2};pwd={3};Connect Timeout=5;", settings.Server, settings.Database, settings.User, settings.Password);
        }

        public static string BuildMasterConnectionString(DatabaseConnectionSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            return String.Format("Data Source={0};Initial Catalog=master;uid={1};pwd={2};Connect Timeout=5;", settings.Server, settings.User, settings.Password);
        }

        public static bool TryGetCurrentConnectionSettings(out DatabaseConnectionSettings settings)
        {
            settings = null;
            try
            {
                string myconnstr = ConfigurationManager.ConnectionStrings["SqlServer"].ConnectionString;
                string[] constr = DbLinkEdit.ReadSqlConfig(myconnstr);
                if (constr != null && constr.Length > 3)
                {
                    DatabaseConnectionSettings current = new DatabaseConnectionSettings();
                    current.Server = constr[0];
                    current.Database = constr[1];
                    current.User = constr[2];
                    current.Password = constr[3];
                    if (current.Server != "" && current.Database != "" && current.User != "")
                    {
                        settings = current;
                        return true;
                    }
                }
            }
            catch
            {
            }
            return false;
        }

        public static bool MasterDbExist(DatabaseConnectionSettings settings)
        {
            if (settings == null)
            {
                return false;
            }
            string masterConnstring = BuildMasterConnectionString(settings);
            return DbLinkEdit.DatabaseExist(masterConnstring);
        }

        public static bool TargetDbExist(DatabaseConnectionSettings settings)
        {
            if (settings == null)
            {
                return false;
            }
            string masterConnstring = BuildMasterConnectionString(settings);
            using (SqlConnection conn = new SqlConnection(masterConnstring))
            {
                using (SqlCommand cmd = new SqlCommand("select count(1) from sys.databases where name=@dbname", conn))
                {
                    cmd.Parameters.AddWithValue("@dbname", settings.Database);
                    conn.Open();
                    object obj = cmd.ExecuteScalar();
                    return obj != null && Convert.ToInt32(obj) > 0;
                }
            }
        }

        public static void CreateDatabase(DatabaseConnectionSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            string safeDbName = settings.Database.Replace("]", "]]");
            string masterConnstring = BuildMasterConnectionString(settings);
            string sql = "if db_id(N'" + settings.Database.Replace("'", "''") + "') is null create database [" + safeDbName + "]";
            DbLinkEdit.CreatSql(masterConnstring, sql);
        }

        public static void WaitForTargetDatabaseReady(DatabaseConnectionSettings settings, int maxAttempts, int delayMilliseconds)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            string connectionString = BuildTargetConnectionString(settings);
            Exception lastError = null;
            for (int i = 0; i < maxAttempts; i++)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    lastError = ex;
                    if (i < maxAttempts - 1)
                    {
                        Thread.Sleep(delayMilliseconds);
                    }
                }
            }

            if (lastError != null)
            {
                throw new Exception("数据库已创建，但系统暂时还不能连接到新库。请稍后重试。原始错误：" + lastError.Message, lastError);
            }
        }

        public static int CreateTableWithRetry(DatabaseConnectionSettings settings, int maxAttempts, int delayMilliseconds)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            string connectionString = BuildTargetConnectionString(settings);
            Exception lastError = null;
            for (int i = 0; i < maxAttempts; i++)
            {
                try
                {
                    return SqlHelper.CreateTable(connectionString);
                }
                catch (Exception ex)
                {
                    lastError = ex;
                    if (i < maxAttempts - 1)
                    {
                        Thread.Sleep(delayMilliseconds);
                    }
                }
            }

            if (lastError != null)
            {
                throw new Exception("连接到目标数据库后导入基础表失败。请确认 SQL Server 已完全就绪，并检查网络/TCP 配置。原始错误：" + lastError.Message, lastError);
            }
            throw new Exception("连接到目标数据库后导入基础表失败。请确认 SQL Server 已完全就绪，并检查网络/TCP 配置。");
        }
    }
}
