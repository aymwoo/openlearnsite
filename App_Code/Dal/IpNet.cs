using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using LearnSite.DBUtility;

namespace LearnSite.DAL
{
    public partial class IpNet
    {
        public IpNet()
        { }

        public int GetMaxId()
        {
            return DbHelperSQL.GetMaxID("Nid", "IpNet");
        }

        public bool Exists(int Nid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from IpNet");
            strSql.Append(" where Nid=@Nid");
            SqlParameter[] parameters = {
                    new SqlParameter("@Nid", SqlDbType.Int, 4)};
            parameters[0].Value = Nid;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        public bool ExistsNet(string Nnet)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from IpNet");
            strSql.Append(" where Nnet=@Nnet");
            SqlParameter[] parameters = {
                    new SqlParameter("@Nnet", SqlDbType.NVarChar, 50)};
            parameters[0].Value = Nnet;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        public int Add(LearnSite.Model.IpNet model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into IpNet(");
            strSql.Append("Nnet,Nhid,Nname,Nremark)");
            strSql.Append(" values (");
            strSql.Append("@Nnet,@Nhid,@Nname,@Nremark)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@Nnet", SqlDbType.NVarChar, 50),
                    new SqlParameter("@Nhid", SqlDbType.Int, 4),
                    new SqlParameter("@Nname", SqlDbType.NVarChar, 100),
                    new SqlParameter("@Nremark", SqlDbType.NVarChar, 200)};
            parameters[0].Value = model.Nnet;
            parameters[1].Value = model.Nhid;
            parameters[2].Value = model.Nname;
            parameters[3].Value = model.Nremark;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        public bool Update(LearnSite.Model.IpNet model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update IpNet set ");
            strSql.Append("Nnet=@Nnet,");
            strSql.Append("Nhid=@Nhid,");
            strSql.Append("Nname=@Nname,");
            strSql.Append("Nremark=@Nremark");
            strSql.Append(" where Nid=@Nid");
            SqlParameter[] parameters = {
                    new SqlParameter("@Nnet", SqlDbType.NVarChar, 50),
                    new SqlParameter("@Nhid", SqlDbType.Int, 4),
                    new SqlParameter("@Nname", SqlDbType.NVarChar, 100),
                    new SqlParameter("@Nremark", SqlDbType.NVarChar, 200),
                    new SqlParameter("@Nid", SqlDbType.Int, 4)};
            parameters[0].Value = model.Nnet;
            parameters[1].Value = model.Nhid;
            parameters[2].Value = model.Nname;
            parameters[3].Value = model.Nremark;
            parameters[4].Value = model.Nid;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Delete(int Nid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from IpNet ");
            strSql.Append(" where Nid=@Nid");
            SqlParameter[] parameters = {
                    new SqlParameter("@Nid", SqlDbType.Int, 4)};
            parameters[0].Value = Nid;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteAll()
        {
            string strSql = "delete from IpNet";
            int rows = DbHelperSQL.ExecuteSql(strSql);
            return rows > 0;
        }

        public LearnSite.Model.IpNet GetModel(int Nid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 Nid,Nnet,Nhid,Nname,Nremark from IpNet ");
            strSql.Append(" where Nid=@Nid");
            SqlParameter[] parameters = {
                    new SqlParameter("@Nid", SqlDbType.Int, 4)};
            parameters[0].Value = Nid;

            LearnSite.Model.IpNet model = new LearnSite.Model.IpNet();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Nid"].ToString() != "")
                {
                    model.Nid = int.Parse(ds.Tables[0].Rows[0]["Nid"].ToString());
                }
                model.Nnet = ds.Tables[0].Rows[0]["Nnet"].ToString();
                if (ds.Tables[0].Rows[0]["Nhid"].ToString() != "")
                {
                    model.Nhid = int.Parse(ds.Tables[0].Rows[0]["Nhid"].ToString());
                }
                model.Nname = ds.Tables[0].Rows[0]["Nname"].ToString();
                model.Nremark = ds.Tables[0].Rows[0]["Nremark"].ToString();
                return model;
            }
            else
            {
                return null;
            }
        }

        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Nid,Nnet,Nhid,Nname,Nremark ");
            strSql.Append(" FROM IpNet ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        public DataSet GetAllList()
        {
            return GetList("");
        }

        public DataTable GetAllNet()
        {
            string strSql = "select Nid,Nnet,Nhid,Nname,Nremark from IpNet order by Nnet asc";
            return DbHelperSQL.Query(strSql).Tables[0];
        }

        public int? GetHidByNet(string ipNet)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 Nhid from IpNet where Nnet=@Nnet");
            SqlParameter[] parameters = {
                    new SqlParameter("@Nnet", SqlDbType.NVarChar, 50)};
            parameters[0].Value = ipNet;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj != null && obj != DBNull.Value)
            {
                return Convert.ToInt32(obj);
            }
            return null;
        }

        public string GetNetByIp(string ip)
        {
            if (string.IsNullOrEmpty(ip))
                return string.Empty;

            string[] parts = ip.Split('.');
            if (parts.Length >= 3)
            {
                string netPrefix = parts[0] + "." + parts[1] + "." + parts[2];

                StringBuilder strSql = new StringBuilder();
                strSql.Append("select top 1 Nnet from IpNet where Nnet=@Nnet");
                SqlParameter[] parameters = {
                        new SqlParameter("@Nnet", SqlDbType.NVarChar, 50)};
                parameters[0].Value = netPrefix;

                object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
                if (obj != null)
                {
                    return obj.ToString();
                }
            }
            return string.Empty;
        }

        public int? GetHidByIp(string ip)
        {
            if (string.IsNullOrEmpty(ip))
                return null;

            string[] parts = ip.Split('.');
            if (parts.Length >= 3)
            {
                string netPrefix = parts[0] + "." + parts[1] + "." + parts[2];

                StringBuilder strSql = new StringBuilder();
                strSql.Append("select top 1 Nhid from IpNet where Nnet=@Nnet");
                SqlParameter[] parameters = {
                        new SqlParameter("@Nnet", SqlDbType.NVarChar, 50)};
                parameters[0].Value = netPrefix;

                object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
                if (obj != null && obj != DBNull.Value)
                {
                    return Convert.ToInt32(obj);
                }
            }
            return null;
        }
    }
}
