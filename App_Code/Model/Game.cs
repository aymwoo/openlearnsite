using System;
namespace LearnSite.Model
{
	/// <summary>
	/// Game:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Game
	{
		public Game()
		{}
		#region Model
		private int _gid;
		private int? _gsid;
        private string _gsname;
		private int? _gnum;
		private string _gtitle;
		private int? _gsave;
		private string _gnote;
		private int? _gscore;
		private DateTime? _gdate;
		/// <summary>
		/// 
		/// </summary>
		public int Gid
		{
			set{ _gid=value;}
			get{return _gid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Gsid
		{
			set{ _gsid=value;}
			get{return _gsid;}
		}
		/// <summary>
		/// 
		/// </summary>
        public string Gsname
		{
			set{ _gsname=value;}
			get{return _gsname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Gnum
		{
			set{ _gnum=value;}
			get{return _gnum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Gtitle
		{
			set{ _gtitle=value;}
			get{return _gtitle;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Gsave
		{
			set{ _gsave=value;}
			get{return _gsave;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Gnote
		{
			set{ _gnote=value;}
			get{return _gnote;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Gscore
		{
			set{ _gscore=value;}
			get{return _gscore;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Gdate
		{
			set{ _gdate=value;}
			get{return _gdate;}
		}
		#endregion Model

	}
}

