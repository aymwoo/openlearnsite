using System;
namespace LearnSite.Model
{
	/// <summary>
	/// MenuWorks:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class MenuWorks
	{
		public MenuWorks()
		{}
		#region Model
		private int _kid;
		private int? _ksid;
		private int? _klid;
		private int? _ktime;
        private int? _kseconds;
        private bool _kcheck;
        private int  _kstar = 0 ;
		/// <summary>
		/// 
		/// </summary>
		public int Kid
		{
			set{ _kid=value;}
			get{return _kid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Ksid
		{
			set{ _ksid=value;}
			get{return _ksid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Klid
		{
			set{ _klid=value;}
			get{return _klid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Ktime
		{
			set{ _ktime=value;}
			get{return _ktime;}
		}
		/// <summary>
		/// 秒级停留时长
		/// </summary>
		public int? Kseconds
		{
			set{ _kseconds=value;}
			get{return _kseconds;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Kcheck
		{
			set{ _kcheck=value;}
			get{return _kcheck;}
		}
        /// <summary>
        /// 
        /// </summary>
        public int Kstar
        {
            set { _kstar = value; }
            get { return _kstar; }
        }
		#endregion Model

	}
}
