using System;
namespace LearnSite.Model
{
	/// <summary>
	/// Exams:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Exams
	{
		public Exams()
		{}
		#region Model
		private int _eid;
		private string _etitle;
		private string _edescription;
		private int _cid;
		private int _hid;
		private DateTime? _etime;
		private bool _eclose= true;
		private int? _escore=0;
		private int? _ecount=0;
		private string _edata;
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
		public string Etitle
		{
			set{ _etitle=value;}
			get{return _etitle;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Edescription
		{
			set{ _edescription=value;}
			get{return _edescription;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Cid
		{
			set{ _cid=value;}
			get{return _cid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int Hid
		{
			set{ _hid=value;}
			get{return _hid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Etime
		{
			set{ _etime=value;}
			get{return _etime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Eclose
		{
			set{ _eclose=value;}
			get{return _eclose;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Escore
		{
			set{ _escore=value;}
			get{return _escore;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Ecount
		{
			set{ _ecount=value;}
			get{return _ecount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Edata
		{
			set{ _edata=value;}
			get{return _edata;}
		}
		#endregion Model

	}
}

