using System;
namespace LearnSite.Model
{
	/// <summary>
	/// Answers:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Answers
	{
		public Answers()
		{}
		#region Model
		private int _aid;
		private int _eid;
		private int _asid;
        private string _asnum;
		private string _asname;
		private int? _asgrade;
		private int? _asclass;
		private DateTime? _atime;
		private int? _ascore;
		private int? _aspent;
		private string _adata;
		/// <summary>
		/// 
		/// </summary>
		public int Aid
		{
			set{ _aid=value;}
			get{return _aid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Eid
		{
			set{ _eid=value;}
			get{return _eid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Asid
		{
			set{ _asid=value;}
			get{return _asid;}
		}
		/// <summary>
		/// 
		/// </summary>
        public string Asnum
		{
			set{ _asnum=value;}
			get{return _asnum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Asname
		{
			set{ _asname=value;}
			get{return _asname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Asgrade
		{
			set{ _asgrade=value;}
			get{return _asgrade;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Asclass
		{
			set{ _asclass=value;}
			get{return _asclass;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Atime
		{
			set{ _atime=value;}
			get{return _atime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Ascore
		{
			set{ _ascore=value;}
			get{return _ascore;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Aspent
		{
			set{ _aspent=value;}
			get{return _aspent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Adata
		{
			set{ _adata=value;}
			get{return _adata;}
		}
		#endregion Model

	}
}

