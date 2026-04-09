using System;
namespace LearnSite.Model
{
	/// <summary>
	/// TurtleMatch:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class TurtleMatch
	{
		public TurtleMatch()
		{}
		#region Model
		private int _mid;
		private int? _mhid;
		private string _mtitle;
		private string _mcontent;
		private DateTime? _mbegin;
		private DateTime? _mend;
		private bool _mpublish= true;
		private DateTime? _mdate;
		/// <summary>
		/// 
		/// </summary>
		public int Mid
		{
			set{ _mid=value;}
			get{return _mid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Mhid
		{
			set{ _mhid=value;}
			get{return _mhid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Mtitle
		{
			set{ _mtitle=value;}
			get{return _mtitle;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Mcontent
		{
			set{ _mcontent=value;}
			get{return _mcontent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Mbegin
		{
			set{ _mbegin=value;}
			get{return _mbegin;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Mend
		{
			set{ _mend=value;}
			get{return _mend;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Mpublish
		{
			set{ _mpublish=value;}
			get{return _mpublish;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Mdate
		{
			set{ _mdate=value;}
			get{return _mdate;}
		}
		#endregion Model

	}
}

