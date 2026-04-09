using System;
namespace LearnSite.Model
{
	/// <summary>
	/// JudgeArg:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class JudgeArg
	{
		public JudgeArg()
		{}
		#region Model
		private int _jid;
		private int? _jhid;
		private int? _jmid;
		private int? _jsleep=1000;
		private string _jinone;
		private string _jintwo;
		private string _jinthree;
		private string _joutone;
		private string _joutwo;
		private string _jouthree;
		private bool _jright= false;
        private string _jcode;
        private int _jcid;
        private string _jimg;
        private string _jthumb="";
		/// <summary>
		/// 
		/// </summary>
		public int Jid
		{
			set{ _jid=value;}
			get{return _jid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Jhid
		{
			set{ _jhid=value;}
			get{return _jhid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Jmid
		{
			set{ _jmid=value;}
			get{return _jmid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Jsleep
		{
			set{ _jsleep=value;}
			get{return _jsleep;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Jinone
		{
			set{ _jinone=value;}
			get{return _jinone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Jintwo
		{
			set{ _jintwo=value;}
			get{return _jintwo;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Jinthree
		{
			set{ _jinthree=value;}
			get{return _jinthree;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Joutone
		{
			set{ _joutone=value;}
			get{return _joutone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Joutwo
		{
			set{ _joutwo=value;}
			get{return _joutwo;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Jouthree
		{
			set{ _jouthree=value;}
			get{return _jouthree;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Jright
		{
			set{ _jright=value;}
			get{return _jright;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Jcode
		{
			set{ _jcode=value;}
			get{return _jcode;}
		}
        /// <summary>
        /// 
        /// </summary>
        public int Jcid
        {
            set { _jcid = value; }
            get { return _jcid; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Jimg
        {
            set { _jimg = value; }
            get { return _jimg; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Jthumb
        {
            set { _jthumb = value; }
            get { return _jthumb; }
        }
		#endregion Model

	}
}

