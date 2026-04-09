using System;
namespace LearnSite.Model
{
	/// <summary>
	/// TurtleQuestion:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class TurtleQuestion
	{
		public TurtleQuestion()
		{}
		#region Model
		private int _qid;
		private int? _qmid;
		private string _qtitle;
		private string _qcontent;
		private int? _qdegree;
		private int? _qsort;
		private string _qcode;
		private string _qimg;
		private string _qurl;
		private string _qout;
		private int? _qscore;
		private DateTime? _qdate;
		/// <summary>
		/// 
		/// </summary>
		public int Qid
		{
			set{ _qid=value;}
			get{return _qid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Qmid
		{
			set{ _qmid=value;}
			get{return _qmid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Qtitle
		{
			set{ _qtitle=value;}
			get{return _qtitle;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Qcontent
		{
			set{ _qcontent=value;}
			get{return _qcontent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Qdegree
		{
			set{ _qdegree=value;}
			get{return _qdegree;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Qsort
		{
			set{ _qsort=value;}
			get{return _qsort;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Qcode
		{
			set{ _qcode=value;}
			get{return _qcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Qimg
		{
			set{ _qimg=value;}
			get{return _qimg;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Qurl
		{
			set{ _qurl=value;}
			get{return _qurl;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Qout
		{
			set{ _qout=value;}
			get{return _qout;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Qscore
		{
			set{ _qscore=value;}
			get{return _qscore;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Qdate
		{
			set{ _qdate=value;}
			get{return _qdate;}
		}
		#endregion Model

	}
}

