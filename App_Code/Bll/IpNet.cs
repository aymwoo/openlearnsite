using System;
using System.Data;
using System.Collections.Generic;
using LearnSite.Model;

namespace LearnSite.BLL
{
    public partial class IpNet
    {
        private readonly LearnSite.DAL.IpNet dal = new LearnSite.DAL.IpNet();
        public IpNet()
        { }

        public int GetMaxId()
        {
            return dal.GetMaxId();
        }

        public bool Exists(int Nid)
        {
            return dal.Exists(Nid);
        }

        public bool ExistsNet(string Nnet)
        {
            return dal.ExistsNet(Nnet);
        }

        public int Add(LearnSite.Model.IpNet model)
        {
            return dal.Add(model);
        }

        public bool Update(LearnSite.Model.IpNet model)
        {
            return dal.Update(model);
        }

        public bool Delete(int Nid)
        {
            return dal.Delete(Nid);
        }

        public bool DeleteAll()
        {
            return dal.DeleteAll();
        }

        public LearnSite.Model.IpNet GetModel(int Nid)
        {
            return dal.GetModel(Nid);
        }

        public DataSet GetList(string strWhere)
        {
            return dal.GetList(strWhere);
        }

        public DataSet GetAllList()
        {
            return GetList("");
        }

        public DataTable GetAllNet()
        {
            return dal.GetAllNet();
        }

        public int? GetHidByNet(string ipNet)
        {
            return dal.GetHidByNet(ipNet);
        }

        public string GetNetByIp(string ip)
        {
            return dal.GetNetByIp(ip);
        }

        public int? GetHidByIp(string ip)
        {
            return dal.GetHidByIp(ip);
        }

        public List<LearnSite.Model.IpNet> GetModelList(string strWhere)
        {
            DataSet ds = dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }

        public List<LearnSite.Model.IpNet> DataTableToList(DataTable dt)
        {
            List<LearnSite.Model.IpNet> modelList = new List<LearnSite.Model.IpNet>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                LearnSite.Model.IpNet model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = new LearnSite.Model.IpNet();
                    if (dt.Rows[n]["Nid"].ToString() != "")
                    {
                        model.Nid = int.Parse(dt.Rows[n]["Nid"].ToString());
                    }
                    model.Nnet = dt.Rows[n]["Nnet"].ToString();
                    if (dt.Rows[n]["Nhid"].ToString() != "")
                    {
                        model.Nhid = int.Parse(dt.Rows[n]["Nhid"].ToString());
                    }
                    model.Nname = dt.Rows[n]["Nname"].ToString();
                    model.Nremark = dt.Rows[n]["Nremark"].ToString();
                    modelList.Add(model);
                }
            }
            return modelList;
        }
    }
}
