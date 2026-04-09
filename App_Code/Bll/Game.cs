using System;
using System.Data;
using System.Collections.Generic;
using LearnSite.Common;
using LearnSite.Model;
namespace LearnSite.BLL
{
	/// <summary>
	/// Game
	/// </summary>
	public partial class Game
	{
		private readonly LearnSite.DAL.Game dal=new LearnSite.DAL.Game();
		public Game()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Gid)
		{
			return dal.Exists(Gid);
		}

                
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsSave(int Gsid, string Gtitle, int Gsave)
        {
            return dal.ExistsSave(Gsid, Gtitle, Gsave);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(LearnSite.Model.Game model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LearnSite.Model.Game model)
		{
			return dal.Update(model);
		}

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool UpdateSave(int Gsid, string Gtitle, int Gsave, string Gnote, int Gnum)
        {
            return dal.UpdateSave(Gsid, Gtitle,Gsave, Gnote, Gnum);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int Gid)
		{
			
			return dal.Delete(Gid);
		}

                
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LearnSite.Model.Game GetModelGameMax(int Gsid, string Gtitle)
        {
            return dal.GetModelGameMax(Gsid, Gtitle);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LearnSite.Model.Game GetModel(int Gid)
		{
			
			return dal.GetModel(Gid);
		}
                
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LearnSite.Model.Game GetModelGame(int Gsid, string Gtitle, int Gsave)
        {
            return dal.GetModelGame(Gsid, Gtitle, Gsave);
        }
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
        
        // <summary>
        /// 获得前几行数据
        /// </summary>
        public string GetRank(int Top, string Gtitle)
        {
            DataTable dt = dal.GetRank(Top, Gtitle).Tables[0];
            string result="";
            List<string> arrayname = new List<string>();
            int count =dt.Rows.Count;
            for(int i=0;i<count;i++){
                string a = dt.Rows[i][0].ToString();
                if (!arrayname.Contains(a)) {
                    int b = Int32.Parse(dt.Rows[i][1].ToString())+1;
                    string c = dt.Rows[i][2].ToString();
                    string d = dt.Rows[i][3].ToString();
                    result = result + "第" + b.ToString()+"关 "+c+d+"班 "+a+" <br>";
                }
                arrayname.Add(a);//将姓名添加到数组中
            }
            
            return result;
        }

        // <summary>
        /// 获得前几行数据
        /// </summary>
        public string GetWuziqiRank(int Top, string Gtitle)
        {
            DataTable dt = dal.GetWuziqiRank(Top, Gtitle).Tables[0];
            string result = "";
            List<string> arrayname = new List<string>();
            int count = dt.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                string a = dt.Rows[i][0].ToString();
                if (!arrayname.Contains(a))
                {
                    string b = dt.Rows[i][1].ToString() ;
                    string c = dt.Rows[i][2].ToString();
                    string d = dt.Rows[i][3].ToString();
                    string e = dt.Rows[i][4].ToString();
                    switch (b)
                    { 
                        case "1":
                            b = "入门";
                            break;
                        case "2":
                            b = "中等";
                            break;
                        case "3":
                            b = "大师";
                            break;
                    }
                    result = result + "<p class='rank'>" + c + d + "班 " + a.PadRight(10) + "   难度：" + b.ToString() + "  " + e + "步获胜" + " </p>";
                }
                arrayname.Add(a);//将姓名添加到数组中
            }

            return result;
        }

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LearnSite.Model.Game> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<LearnSite.Model.Game> DataTableToList(DataTable dt)
		{
			return BllDataTableMappers.MapGameList(dt);
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			return dal.GetRecordCount(strWhere);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}
