using System;
namespace LearnSite.Model
{
    [Serializable]
    public partial class IpNet
    {
        public IpNet()
        { }
        #region Model
        private int _nid;
        private string _nnet;
        private int? _nhid;
        private string _nname;
        private string _nremark;

        public int Nid
        {
            set { _nid = value; }
            get { return _nid; }
        }

        public string Nnet
        {
            set { _nnet = value; }
            get { return _nnet; }
        }

        public int? Nhid
        {
            set { _nhid = value; }
            get { return _nhid; }
        }

        public string Nname
        {
            set { _nname = value; }
            get { return _nname; }
        }

        public string Nremark
        {
            set { _nremark = value; }
            get { return _nremark; }
        }
        #endregion Model
    }
}
