using System;
namespace LearnSite.Model
{
	/// <summary>
	/// Consoles:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Consoles
	{
		public Consoles()
		{}
		#region Model
		private int _nid;
		private int? _nhid;
		private int? _ncid;
		private string _ntitle;
		private string _ncontent;
		private bool _npublish= false;
        private DateTime? _ndate;
        private bool _nbegin = false;
		/// <summary>
		/// 
		/// </summary>
		public int Nid
		{
			set{ _nid=value;}
			get{return _nid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Nhid
		{
			set{ _nhid=value;}
			get{return _nhid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Ncid
		{
			set{ _ncid=value;}
			get{return _ncid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Ntitle
		{
			set{ _ntitle=value;}
			get{return _ntitle;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Ncontent
		{
			set{ _ncontent=value;}
			get{return _ncontent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Npublish
		{
			set{ _npublish=value;}
			get{return _npublish;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Ndate
		{
			set{ _ndate=value;}
			get{return _ndate;}
		}
        /// <summary>
        /// 
        /// </summary>
        public bool Nbegin
        {
            set { _nbegin = value; }
            get { return _nbegin; }
        }
		#endregion Model

	}
}

