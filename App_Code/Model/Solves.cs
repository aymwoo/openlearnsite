using System;
namespace LearnSite.Model
{
	/// <summary>
	/// Solves:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Solves
	{
		public Solves()
		{}
		#region Model
		private int _vid;
		private int? _vpid;
		private int? _vsid;
		private string _vanswer;
		private bool _vright= false;
		private int? _vscore;
        private DateTime? _vdate;
        private int _vgrade;
        private int _vterm;
        private int _vyear;
        private int? _vnid;
        private int? _vcid;
        private int _vclass;
        private int _vlid;

		/// <summary>
		/// 
		/// </summary>
		public int Vid
		{
			set{ _vid=value;}
			get{return _vid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Vpid
		{
			set{ _vpid=value;}
			get{return _vpid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Vsid
		{
			set{ _vsid=value;}
			get{return _vsid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Vanswer
		{
			set{ _vanswer=value;}
			get{return _vanswer;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Vright
		{
			set{ _vright=value;}
			get{return _vright;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Vscore
		{
			set{ _vscore=value;}
			get{return _vscore;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Vdate
		{
			set{ _vdate=value;}
			get{return _vdate;}
		}
        /// <summary>
        /// 
        /// </summary>
        public int Vgrade
        {
            set { _vgrade = value; }
            get { return _vgrade; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Vterm
        {
            set { _vterm = value; }
            get { return _vterm; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Vyear
        {
            set { _vyear = value; }
            get { return _vyear; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Vnid
        {
            set { _vnid = value; }
            get { return _vnid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Vcid
        {
            set { _vcid = value; }
            get { return _vcid; }
        }        
        /// <summary>
        /// 
        /// </summary>
        public int Vclass
        {
            set { _vclass = value; }
            get { return _vclass; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Vlid
        {
            set { _vlid = value; }
            get { return _vlid; }
        }
		#endregion Model

	}
}

