using System;
namespace LearnSite.Model
{
	/// <summary>
	/// TurtleAnswer:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class TurtleAnswer
	{
		public TurtleAnswer()
		{}
		#region Model
		private int _aid;
		private int? _amid;
		private int? _aqid;
		private string _acode;
		private string _aimg;
		private string _aurl;
		private string _aout;
		private int? _ascore;
		private int? _asid;
		private string _asname;
		private bool _alock= false;
		private DateTime? _adate;
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
		public int? Amid
		{
			set{ _amid=value;}
			get{return _amid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Aqid
		{
			set{ _aqid=value;}
			get{return _aqid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Acode
		{
			set{ _acode=value;}
			get{return _acode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Aimg
		{
			set{ _aimg=value;}
			get{return _aimg;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Aurl
		{
			set{ _aurl=value;}
			get{return _aurl;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Aout
		{
			set{ _aout=value;}
			get{return _aout;}
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
		public int? Asid
		{
			set{ _asid=value;}
			get{return _asid;}
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
		public bool Alock
		{
			set{ _alock=value;}
			get{return _alock;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Adate
		{
			set{ _adate=value;}
			get{return _adate;}
		}
		#endregion Model

	}
}

