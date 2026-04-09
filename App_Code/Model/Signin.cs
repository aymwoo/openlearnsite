using System;
namespace LearnSite.Model
{
	/// <summary>
	/// 实体类Signin 
	/// </summary>
	[Serializable]
    public class Signin
    {
        public Signin()
        { }
        #region Model
        private int _qid;
        private string _qnum;
        private int? _qattitude;
        private DateTime? _qdate;
        private int? _qyear;
        private int? _qmonth;
        private int? _qday;
        private string _qweek;
        private string _qip;
        private string _qmachine;
        private string _qnote;
        private int? _qwork;
        private int? _qgrade;
        private int? _qterm;
        private string _qgroup;
        private int _qgscore;
        private int _qsid;
        private string _qname;
        private int _qclass;
        private int _qsyear;
        private int _qcid;
        private string _Qtitle;//新增
        private string _Qsession;//新增
        /// <summary>
        /// 
        /// </summary>
        public int Qid
        {
            set { _qid = value; }
            get { return _qid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Qnum
        {
            set { _qnum = value; }
            get { return _qnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Qattitude
        {
            set { _qattitude = value; }
            get { return _qattitude; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? Qdate
        {
            set { _qdate = value; }
            get { return _qdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Qyear
        {
            set { _qyear = value; }
            get { return _qyear; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Qmonth
        {
            set { _qmonth = value; }
            get { return _qmonth; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Qday
        {
            set { _qday = value; }
            get { return _qday; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Qweek
        {
            set { _qweek = value; }
            get { return _qweek; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Qip
        {
            set { _qip = value; }
            get { return _qip; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Qmachine
        {
            set { _qmachine = value; }
            get { return _qmachine; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Qnote
        {
            set { _qnote = value; }
            get { return _qnote; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Qwork
        {
            set { _qwork = value; }
            get { return _qwork; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? Qgrade
        {
            set { _qgrade = value; }
            get { return _qgrade; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Qterm
        {
            set { _qterm = value; }
            get { return _qterm; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Qgroup
        {
            set { _qgroup = value; }
            get { return _qgroup; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Qgscore
        {
            set { _qgscore = value; }
            get { return _qgscore; }
        }        
        /// <summary>
        /// 
        /// </summary>
        public int Qsid
        {
            set { _qsid = value; }
            get { return _qsid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Qname
        {
            set { _qname = value; }
            get { return _qname; }
        }        
        /// <summary>
        /// 
        /// </summary>
        public int Qclass
        {
            set { _qclass = value; }
            get { return _qclass; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Qsyear
        {
            set { _qsyear = value; }
            get { return _qsyear; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Qcid
        {
            set { _qcid = value; }
            get { return _qcid; }
        }
        /// <summary>
        /// 机房日志 学习内容，学案标题
        /// </summary>
        public string Qtitle
        {
            set { _Qtitle = value; }
            get { return _Qtitle; }
        }
        /// <summary>
        /// 机房日志 学习内容，学案标题
        /// </summary>
        public string Qsession
        {
            set { _Qsession = value; }
            get { return _Qsession; }
        }
        #endregion Model

    }
}

