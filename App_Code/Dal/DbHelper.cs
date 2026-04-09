using System;
using System.Data;
using System.Data.SqlClient;

namespace LearnSite.DAL
{
    public static class DbHelper
    {
        // 🔥 这是万能连接字符串，直接用SQL账号登录，跳过IIS权限问题
        private static string ConnString
        {
            get
            {
                return "Data Source=.;Initial Catalog=learnsite;User ID=sa;Password=learnsite@1234;";
            }
        }

        public static DataTable Query(string sql, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }

        public static int Execute(string sql, params SqlParameter[] parameters)
        {
            int rows = 0;
            using (SqlConnection conn = new SqlConnection(ConnString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    
                    conn.Open();
                    rows = cmd.ExecuteNonQuery();
                }
            }
            return rows;
        }
    }
}