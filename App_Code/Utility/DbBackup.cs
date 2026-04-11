using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace LearnSite.DBUtility
{
    /// <summary>
    ///DbBackup 的摘要说明
    /// </summary>
    public class DbBackup
    {
        public DbBackup()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        static string savedir = "~/backupdb";
        static string backupPathKey = "DbBackupPhysicalPath";

        private static string GetWebBackupDirectory()
        {
            return HttpContext.Current.Server.MapPath(savedir);
        }

        private static string GetConfiguredBackupDirectory()
        {
            string configPath = ConfigurationManager.AppSettings[backupPathKey];
            if (string.IsNullOrWhiteSpace(configPath))
            {
                return string.Empty;
            }
            return configPath.Trim();
        }

        private static void AddDirectory(ArrayList dirs, string dir)
        {
            if (string.IsNullOrWhiteSpace(dir))
            {
                return;
            }
            foreach (string item in dirs)
            {
                if (string.Equals(item, dir, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }
            dirs.Add(dir);
        }

        private static string GetSqlBackupDirectory(SqlConnection con)
        {
            string configuredDir = GetConfiguredBackupDirectory();
            if (!string.IsNullOrEmpty(configuredDir))
            {
                return configuredDir;
            }

            string sql = "select convert(nvarchar(4000), serverproperty('InstanceDefaultBackupPath'))";
            object result = new SqlCommand(sql, con).ExecuteScalar();
            if (result == null || result == DBNull.Value)
            {
                return string.Empty;
            }
            return result.ToString().Trim();
        }

        private static ArrayList GetBackupDirectories()
        {
            ArrayList dirs = new ArrayList();
            AddDirectory(dirs, GetConfiguredBackupDirectory());
            AddDirectory(dirs, GetWebBackupDirectory());

            string connstr = SqlHelper.connectionString;
            using (SqlConnection con = new SqlConnection(connstr))
            {
                try
                {
                    con.Open();
                    AddDirectory(dirs, GetSqlBackupDirectory(con));
                }
                catch
                {
                }
            }

            return dirs;
        }

        private static ArrayList GetPreferredBackupDirectories(SqlConnection con)
        {
            ArrayList dirs = new ArrayList();
            AddDirectory(dirs, GetConfiguredBackupDirectory());
            AddDirectory(dirs, GetWebBackupDirectory());
            AddDirectory(dirs, GetSqlBackupDirectory(con));
            return dirs;
        }

        private static bool CanEnsureDirectory(string dir)
        {
            if (string.IsNullOrEmpty(dir))
            {
                return false;
            }

            string configuredDir = GetConfiguredBackupDirectory();
            if (!string.IsNullOrEmpty(configuredDir) && string.Equals(dir, configuredDir, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return string.Equals(dir, GetWebBackupDirectory(), StringComparison.OrdinalIgnoreCase);
        }

        private static ArrayList GetBackupFiles()
        {
            ArrayList fileList = new ArrayList();
            Hashtable exists = new Hashtable(StringComparer.OrdinalIgnoreCase);
            ArrayList dirs = GetBackupDirectories();
            foreach (string dir in dirs)
            {
                if (string.IsNullOrEmpty(dir))
                {
                    continue;
                }
                try
                {
                    if (!Directory.Exists(dir))
                    {
                        continue;
                    }
                    DirectoryInfo di = new DirectoryInfo(dir);
                    FileInfo[] fis = di.GetFiles("*.bak");
                    foreach (FileInfo fi in fis)
                    {
                        if (exists.ContainsKey(fi.FullName))
                        {
                            continue;
                        }
                        exists.Add(fi.FullName, true);
                        fileList.Add(fi);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    continue;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return fileList;
        }

        private static string ResolveBackupFilePath(string dbFileUrl)
        {
            if (Path.IsPathRooted(dbFileUrl))
            {
                return dbFileUrl;
            }
            return HttpContext.Current.Server.MapPath(dbFileUrl);
        }

        private static string CombineBackupPath(string dir, string fileName)
        {
            if (string.IsNullOrEmpty(dir))
            {
                return fileName;
            }
            if (dir.EndsWith("\\") || dir.EndsWith("/"))
            {
                return dir + fileName;
            }
            if (dir.IndexOf('\\') >= 0 && dir.IndexOf('/') < 0)
            {
                return dir + "\\" + fileName;
            }
            return dir + "/" + fileName;
        }

        private static string EscapeSqlString(string value)
        {
            return value.Replace("'", "''");
        }

        private static string EscapeSqlIdentifier(string value)
        {
            return "[" + value.Replace("]", "]]") + "]";
        }

        private static void EnsureWebBackupDirectory()
        {
            string webBackupDir = GetWebBackupDirectory();
            if (!Directory.Exists(webBackupDir))
            {
                Directory.CreateDirectory(webBackupDir);
            }
        }

        private static string TryCopyBackupToWebDirectory(string sourceFile)
        {
            if (string.IsNullOrEmpty(sourceFile) || !File.Exists(sourceFile))
            {
                return string.Empty;
            }

            string webBackupDir = GetWebBackupDirectory();
            string webBackupFile = Path.Combine(webBackupDir, Path.GetFileName(sourceFile));
            if (string.Equals(sourceFile, webBackupFile, StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            try
            {
                EnsureWebBackupDirectory();
                File.Copy(sourceFile, webBackupFile, true);
                return webBackupFile;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static DataView BuildBackupFileView(ArrayList files)
        {
            DataView dv = new DataView();
            DataSet ds = new DataSet();
            ds.Tables.Add();
            ds.Tables[0].TableName = "filetable";
            ds.Tables[0].Columns.Add("fid", typeof(Int32));
            ds.Tables[0].Columns.Add("fname", typeof(String));
            ds.Tables[0].Columns.Add("fsize", typeof(String));
            ds.Tables[0].Columns.Add("furl", typeof(String));
            ds.Tables[0].Columns.Add("fread", typeof(String));
            ds.Tables[0].Columns.Add("fdate", typeof(DateTime));

            int i = 0;
            foreach (FileInfo fi in files)
            {
                DataRow row = ds.Tables[0].NewRow();
                i++;
                row[0] = i;
                row[1] = fi.Name;
                row[2] = (fi.Length / 1024).ToString() + "kb";
                row[3] = fi.FullName;
                row[4] = fi.IsReadOnly.ToString().Substring(0, 1);
                row[5] = fi.CreationTime;
                ds.Tables[0].Rows.Add(row);
            }
            ds.AcceptChanges();
            dv = ds.Tables[0].DefaultView;
            dv.Sort = "fdate desc";
            ds.Dispose();
            return dv;
        }
        /// <summary>
        /// 备份列表
        /// </summary>
        /// <returns></returns>
        public static ArrayList Dblist()
        {
            ArrayList arl = new ArrayList();
            ArrayList files = GetBackupFiles();
            foreach (FileInfo fi in files)
            {
                arl.Add(fi.Name);
            }
            return arl;
        }

        /// <summary>
        /// 判断今天有无备份
        /// </summary>
        /// <returns></returns>
        public static bool IsTodayBackUp()
        {
            bool isright = false;
            DateTime dt = DateTime.Now;

            ArrayList files = GetBackupFiles();
            foreach (FileInfo fi in files)
            {
                if (dt.Day == fi.LastWriteTime.Day && dt.Month == fi.LastWriteTime.Month)
                {
                    isright = true;
                    break;
                }
            }
            return isright;
        }

        /// <summary>
        /// 判断每周有无备份：返回真则有备份
        /// </summary>
        /// <returns></returns>
        public static bool IsWeeksBackUp()
        {
            bool isright = false;
            DateTime dt1 = DateTime.Now.AddDays(-6);
            DateTime dtlimit = DateTime.Now.AddMonths(-6);

            ArrayList files = GetBackupFiles();
            int fc = files.Count;
            foreach (FileInfo fi in files)
            {
                DateTime fctime=fi.CreationTime;
                if (DateTime.Compare(fctime, dt1) > 0)
                {
                    isright = true;//如果备份日期大于上周日期，说明有备份
                    if (fc > 3)
                    {
                        if (DateTime.Compare(fctime, dtlimit) < 0)
                        {
                            fi.Delete();//如果备份数大于3个，且存在6个月前的数据库，则自动删除
                        }
                    }
                }
            }
            return isright;
        }
        /// <summary>
        /// 获取当前目录下的文件列表
        /// </summary>
        /// <returns></returns>
        public static DataView BackUpFileList()
        {
            return BuildBackupFileView(GetBackupFiles());
        }
        /// <summary>
        /// 获取子目录中日期最新的这个
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static string GetLastDir(string dir)
        {
            string result = "";
            if (Directory.Exists(dir))
            {
                DirectoryInfo di = new DirectoryInfo(dir);
                DirectoryInfo[] dilist = di.GetDirectories();
                DateTime newdt = DateTime.Parse("2002-01-01");
                foreach (DirectoryInfo d in dilist)
                {
                    DateTime dirdate = d.LastWriteTime;
                    if (DateTime.Compare(dirdate, newdt) > 0)
                    {
                        result = d.Name;
                        newdt = dirdate;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取当前目录下的文件列表，返回ds数据集，包含fid,fname,fsize,furl四个字段
        /// </summary>
        /// <param name="strdir">虚拟路径</param>
        /// <returns></returns>
        public static DataView FileList(string strdir)
        {
            if (string.Equals(strdir, savedir, StringComparison.OrdinalIgnoreCase))
            {
                return BuildBackupFileView(GetBackupFiles());
            }
            if (!string.IsNullOrEmpty(strdir) && !string.Equals(strdir, savedir, StringComparison.OrdinalIgnoreCase))
            {
                string saverealpath = HttpContext.Current.Server.MapPath(strdir);
                DataView dv = new DataView();
                DataSet ds = new DataSet();
                ds.Tables.Add();
                ds.Tables[0].TableName = "filetable";
                ds.Tables[0].Columns.Add("fid", typeof(Int32));
                ds.Tables[0].Columns.Add("fname", typeof(String));
                ds.Tables[0].Columns.Add("fsize", typeof(String));
                ds.Tables[0].Columns.Add("furl", typeof(String));
                ds.Tables[0].Columns.Add("fread", typeof(String));
                ds.Tables[0].Columns.Add("fdate", typeof(DateTime));

                if (Directory.Exists(saverealpath))
                {
                    DirectoryInfo di = new DirectoryInfo(saverealpath);
                    FileInfo[] fis = di.GetFiles();
                    int i = 0;
                    if (!strdir.EndsWith("/"))
                        strdir += "/";
                    foreach (FileInfo fi in fis)
                    {
                        DataRow row;
                        row = ds.Tables[0].NewRow();
                        i++;
                        row[1] = fi.Name;
                        row[2] = (fi.Length / 1024).ToString() + "kb";
                        row[3] = strdir + fi.Name;
                        row[4] = fi.IsReadOnly.ToString().Substring(0, 1);
                        row[5] = fi.CreationTime;
                        ds.Tables[0].Rows.Add(row);
                    }
                    ds.AcceptChanges();
                    dv = ds.Tables[0].DefaultView;
                    dv.Sort = "fdate desc";
                }
                ds.Dispose();
                return dv;
            }

            else
            {
                return null;
            }
        }
                
        /// <summary>
        /// 获取备份数据库名称
        /// </summary>
        /// <returns></returns>
        public static string GetMyDbName()
        {
            string connstr = SqlHelper.connectionString;
            string[] spstr = connstr.Split(';');
            string[] dbname = spstr[1].Split('=');
            return dbname[1].Trim();
        }
        /// <summary>
        ///以当前日期为文件名，数据库备份
        /// </summary>
        /// <returns></returns>
        public static string BakupMyDb()
        {
            string connstr = SqlHelper.connectionString;
            string saverealpath = HttpContext.Current.Server.MapPath(savedir);
            if (!Directory.Exists(saverealpath))
            {
                Directory.CreateDirectory(saverealpath);
            }
            string OldDbName = GetMyDbName();
            DateTime dt = DateTime.Now;
            string fname = string.Format("{0:yyyyMMddHHmmss}", dt);
            using (SqlConnection con = new SqlConnection(connstr))
            {
                Exception lastError = null;
                try
                {
                    con.Open();
                    ArrayList backupDirs = GetPreferredBackupDirectories(con);
                    foreach (string backupDir in backupDirs)
                    {
                        try
                        {
                            if (CanEnsureDirectory(backupDir) && !Directory.Exists(backupDir))
                            {
                                Directory.CreateDirectory(backupDir);
                            }
                            string BackDbNamePath = CombineBackupPath(backupDir, fname + ".bak");
                            string SqlBackStr = "backup database " + EscapeSqlIdentifier(OldDbName) + " to disk = N'" + EscapeSqlString(BackDbNamePath) + "' with init";
                            SqlCommand com = new SqlCommand(SqlBackStr, con);
                            com.ExecuteNonQuery();
                            string copiedPath = TryCopyBackupToWebDirectory(BackDbNamePath);
                            if (!string.IsNullOrEmpty(copiedPath) || string.Equals(backupDir, saverealpath, StringComparison.OrdinalIgnoreCase))
                            {
                                return "数据库备份成功！";
                            }
                            return "数据库备份成功！备份文件位置：" + BackDbNamePath;
                        }
                        catch (Exception error)
                        {
                            lastError = error;
                        }
                    }
                    if (lastError != null)
                    {
                        return "数据库备份失败！" + lastError.Message;
                    }
                    return "数据库备份失败！未找到可用的备份目录。";
                }
                catch (Exception error)
                {
                    con.Close();
                    return "数据库备份失败！" + error.Message;
                }
                finally
                {
                    con.Close();
                }
            }
        }
        /// <summary>   
        /// 第一种还原数据库文件方法，测试成功！
        /// </summary>   
        /// <param name="dbFile">数据库备份文件（含路径）</param>   
        /// <returns></returns>   
        public static string RestoreMyDb(string dbFileUrl)
        {
            string msg = "无信息";
            string dbFile = ResolveBackupFilePath(dbFileUrl);
            if (File.Exists(dbFile))
            {
                //sql数据库名   
                string dbName = GetMyDbName();
                //创建连接对象   
                string connstr = SqlHelper.connectionString;
                using (SqlConnection conn = new SqlConnection(connstr))
                {
                    //还原指定的数据库文件   
                    string sql = string.Format("use master ;declare @s varchar(8000);select @s=isnull(@s,'')+' kill '+rtrim(spID) from master..sysprocesses where dbid=db_id('{0}');select @s;exec(@s) ;RESTORE DATABASE {1} FROM DISK = N'{2}' with replace", EscapeSqlString(dbName), EscapeSqlIdentifier(dbName), EscapeSqlString(dbFile));
                    SqlCommand sqlcmd = new SqlCommand(sql, conn);
                    sqlcmd.CommandType = CommandType.Text;
                    try
                    {
                        conn.Open();
                        sqlcmd.ExecuteNonQuery();
                        msg = "数据库恢复成功！";
                    }
                    catch
                    {
                        msg = "数据库恢复失败！";
                    }
                }
            }
            else
            {
                msg = "备份文件不存在！";
            }

            return msg;
        }
        /// <summary>
        /// 第二种数据库还原方法
        /// </summary>
        /// <param name="dbFileUrl"></param>
        /// <returns></returns>
        public static string RestoreDb(string dbFileUrl)
        {
            string msg = "无信息";
            string dbFile = ResolveBackupFilePath(dbFileUrl);
            if (File.Exists(dbFile))
            {
                string connstr = SqlHelper.connectionString;
                string DBName = GetMyDbName();
                //使远程数据库转入单用户模式，断开所有已连接数据库的用户的连接并回退它们的事务。
                string RecoveryStr = string.Format("Alter DATABASE {0} set single_user with rollback immediate use master RESTORE DATABASE {1} from disk = N'{2}' with replace", EscapeSqlIdentifier(DBName), EscapeSqlIdentifier(DBName), EscapeSqlString(dbFile));
                using (SqlConnection conn = new SqlConnection(connstr))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand comm1 = new SqlCommand(RecoveryStr, conn);
                        comm1.ExecuteNonQuery(); //执行远程数据库恢复命令 

                        msg = "数据库还原成功!";
                        RecoveryStr = "Alter DATABASE " + EscapeSqlIdentifier(DBName) + " set multi_user";
                        SqlCommand comm2 = new SqlCommand(RecoveryStr, conn);
                        comm2.ExecuteNonQuery();
                        //使远程数据库转入多用户模式 
                    }
                    catch
                    {
                        RecoveryStr = "Alter DATABASE " + EscapeSqlIdentifier(DBName) + " set multi_user";
                        SqlCommand comm2 = new SqlCommand(RecoveryStr, conn);
                        comm2.ExecuteNonQuery();
                        msg = "数据库还原失败!数据库的平台版本不一致.";
                    }
                }
            }
            return msg;
        }


    }
}
