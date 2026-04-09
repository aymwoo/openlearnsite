using System;
namespace LearnSite.Model
{
	/// <summary>
	/// Problems:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Problems
	{
		public Problems()
		{}
		#region Model
		private int _pid;
		private int? _phid;
		private int? _pnid;
		private string _ptitle;
		private string _pcode;
		private string _pouput;
		private int? _pscore;
        private DateTime? _pdate;
        private int? _psort;
        private int _pcid;
		/// <summary>
		/// 
		/// </summary>
		public int Pid
		{
			set{ _pid=value;}
			get{return _pid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Phid
		{
			set{ _phid=value;}
			get{return _phid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Pnid
		{
			set{ _pnid=value;}
			get{return _pnid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Ptitle
		{
			set{ _ptitle=value;}
			get{return _ptitle;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Pcode
		{
			set{ _pcode=value;}
			get{return _pcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Pouput
		{
			set{ _pouput=value;}
			get{return _pouput;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Pscore
		{
			set{ _pscore=value;}
			get{return _pscore;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Pdate
		{
			set{ _pdate=value;}
			get{return _pdate;}
		}
        /// <summary>
        /// 
        /// </summary>
        public int? Psort
        {
            set { _psort = value; }
            get { return _psort; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Pcid
        {
            set { _pcid = value; }
            get { return _pcid; }
        }
		#endregion Model

	}
}

