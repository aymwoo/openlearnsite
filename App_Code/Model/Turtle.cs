using System;
namespace LearnSite.Model
{
	/// <summary>
	/// Turtle:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Turtle
	{
		public Turtle()
		{}
		#region Model
		private int _tid;
		private int? _thid;
		private string _ttilte;
		private string _tcontent;
		private int? _tdegree;
		private int? _tsort;
		private string _tcode;
		private string _timg;
		private string _turl;
		private string _tout;
        private DateTime? _tdate;
        private bool _tstudy = false;
        private int _tsid=0;
        private int _tscore=0;
        private string _tip="";
        
		/// <summary>
		/// 
		/// </summary>
		public int Tid
		{
			set{ _tid=value;}
			get{return _tid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Thid
		{
			set{ _thid=value;}
			get{return _thid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Ttilte
		{
			set{ _ttilte=value;}
			get{return _ttilte;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Tcontent
		{
			set{ _tcontent=value;}
			get{return _tcontent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Tdegree
		{
			set{ _tdegree=value;}
			get{return _tdegree;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Tsort
		{
			set{ _tsort=value;}
			get{return _tsort;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Tcode
		{
			set{ _tcode=value;}
			get{return _tcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Timg
		{
			set{ _timg=value;}
			get{return _timg;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Turl
		{
			set{ _turl=value;}
			get{return _turl;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Tout
		{
			set{ _tout=value;}
			get{return _tout;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Tdate
		{
			set{ _tdate=value;}
			get{return _tdate;}
		}
        /// <summary>
        /// 
        /// </summary>
        public bool Tstudy
        {
            set { _tstudy = value; }
            get { return _tstudy; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Tsid
        {
            set { _tsid = value; }
            get { return _tsid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Tscore
        {
            set { _tscore = value; }
            get { return _tscore; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Tip
        {
            set { _tip = value; }
            get { return _tip; }
        }
		#endregion Model

	}
}

