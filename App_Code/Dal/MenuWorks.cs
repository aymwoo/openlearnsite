using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using LearnSite.DBUtility;
using System.Collections;//Please add references
namespace LearnSite.DAL
{
	/// <summary>
	/// 数据访问类:MenuWorks
	/// </summary>
	public partial class MenuWorks
	{
		public MenuWorks()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Kid", "MenuWorks"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Kid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from MenuWorks");
			strSql.Append(" where Kid=@Kid ");
			SqlParameter[] parameters = {
					new SqlParameter("@Kid", SqlDbType.Int,4)			};
			parameters[0].Value = Kid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Ksid,int klid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from MenuWorks");
            strSql.Append(" where Ksid=@Ksid and klid=@klid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Ksid", SqlDbType.Int,4),                                        
					new SqlParameter("@klid", SqlDbType.Int,4)};
            parameters[0].Value = Ksid;
            parameters[1].Value = klid;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }
       
        /// <summary>
        /// 阅读任务标记为已学
        /// </summary>
        /// <param name="Ksid"></param>
        /// <returns></returns>
        public string readCids(int Ksid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct Lcid from ListMenu,MenuWorks ");
            strSql.Append(" where ksid=@Ksid and klid=Lid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Ksid", SqlDbType.Int,4)};
            parameters[0].Value = Ksid;

            string cids = "";
            DataTable dt = DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];
            int count = dt.Rows.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    string cidstr = dt.Rows[i]["Lcid"].ToString();
                    if (!string.IsNullOrEmpty(cidstr))
                    {
                        cids = cids + cidstr + ",";
                    }
                }
            }

            return cids;
        }


        /// <summary>
        /// /// 花费时间
        /// </summary>
        public int SpendTime(int Ksid, int klid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Ktime from MenuWorks");
            strSql.Append(" where Ksid=@Ksid and klid=@klid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Ksid", SqlDbType.Int,4),                                        
					new SqlParameter("@klid", SqlDbType.Int,4)};
            parameters[0].Value = Ksid;
            parameters[1].Value = klid;

            return DbHelperSQL.FindNumber(strSql.ToString(), parameters);
        }

        public int SpendSeconds(int Ksid, int klid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select isnull(Kseconds, Ktime * 60) from MenuWorks");
            strSql.Append(" where Ksid=@Ksid and klid=@klid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Ksid", SqlDbType.Int,4),
					new SqlParameter("@klid", SqlDbType.Int,4)};
            parameters[0].Value = Ksid;
            parameters[1].Value = klid;

            return DbHelperSQL.FindNumber(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(LearnSite.Model.MenuWorks model)
		{
			if (model == null || !model.Ksid.HasValue || !model.Klid.HasValue)
			{
				return false;
			}

			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into MenuWorks(");
			strSql.Append("Ksid,Klid,Ktime,Kseconds,Kcheck,Kstar)");
			strSql.Append(" select ");
			strSql.Append("@Ksid,@Klid,@Ktime,@Kseconds,@Kcheck,@Kstar");
			strSql.Append(" where not exists (");
			strSql.Append("select 1 from MenuWorks with (UPDLOCK, HOLDLOCK) where Ksid=@Ksid and Klid=@Klid)");
			int kseconds = model.Kseconds.HasValue ? model.Kseconds.Value : (model.Ktime.HasValue ? model.Ktime.Value * 60 : 0);
			SqlParameter[] parameters = {
					new SqlParameter("@Ksid", SqlDbType.Int,4),
					new SqlParameter("@Klid", SqlDbType.Int,4),
					new SqlParameter("@Ktime", SqlDbType.Int,4),
					new SqlParameter("@Kseconds", SqlDbType.Int,4),
					new SqlParameter("@Kcheck", SqlDbType.Bit,1),
					new SqlParameter("@Kstar", SqlDbType.Int,4)};
			parameters[0].Value = model.Ksid;
			parameters[1].Value = model.Klid;
			parameters[2].Value = model.Ktime;
	            parameters[3].Value = kseconds;
	            parameters[4].Value = model.Kcheck;
	            parameters[5].Value = model.Kstar;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			if (Exists(model.Ksid.Value, model.Klid.Value))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(LearnSite.Model.MenuWorks model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update MenuWorks set ");
			strSql.Append("Ksid=@Ksid,");
			strSql.Append("Klid=@Klid,");
			strSql.Append("Ktime=@Ktime,");
			strSql.Append("Kseconds=@Kseconds,");
            strSql.Append("Kcheck=@Kcheck,");
            strSql.Append("Kstar=@Kstar");
			strSql.Append(" where Kid=@Kid ");
			int kseconds = model.Kseconds.HasValue ? model.Kseconds.Value : (model.Ktime.HasValue ? model.Ktime.Value * 60 : 0);
			SqlParameter[] parameters = {
					new SqlParameter("@Ksid", SqlDbType.Int,4),
					new SqlParameter("@Klid", SqlDbType.Int,4),
					new SqlParameter("@Ktime", SqlDbType.Int,4),
					new SqlParameter("@Kseconds", SqlDbType.Int,4),
					new SqlParameter("@Kcheck", SqlDbType.Bit,1),
					new SqlParameter("@Kid", SqlDbType.Int,4),
					new SqlParameter("@Kstar", SqlDbType.Int,4)};
			parameters[0].Value = model.Ksid;
			parameters[1].Value = model.Klid;
			parameters[2].Value = model.Ktime;
			parameters[3].Value = kseconds;
			parameters[4].Value = model.Kcheck;
	            parameters[5].Value = model.Kid;
	            parameters[6].Value = model.Kstar;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

        /// <summary>
        /// 删除指定用户的任务记录
        /// </summary>
        public bool DeleteMenuWork(int Ksid, int klid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from MenuWorks");
            strSql.Append(" where Ksid=@Ksid and klid=@klid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Ksid", SqlDbType.Int,4),                                        
					new SqlParameter("@klid", SqlDbType.Int,4)};
            parameters[0].Value = Ksid;
            parameters[1].Value = klid;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int Kid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from MenuWorks ");
			strSql.Append(" where Kid=@Kid ");
			SqlParameter[] parameters = {
					new SqlParameter("@Kid", SqlDbType.Int,4)			};
			parameters[0].Value = Kid;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string Kidlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from MenuWorks ");
			strSql.Append(" where Kid in ("+Kidlist + ")  ");
			int rows=DbHelperSQL.ExecuteSql(strSql.ToString());
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LearnSite.Model.MenuWorks GetModelme(int Ksid, int Klid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Kid,Ksid,Klid,Ktime,Kseconds,Kcheck,Kstar from MenuWorks ");
            strSql.Append(" where Ksid=@Ksid and Klid=@Klid");
            SqlParameter[] parameters = {
					new SqlParameter("@Ksid", SqlDbType.Int,4),                                        
					new SqlParameter("@Klid", SqlDbType.Int,4)};
            parameters[0].Value = Ksid;
            parameters[1].Value = Klid;

            LearnSite.Model.MenuWorks model = new LearnSite.Model.MenuWorks();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LearnSite.Model.MenuWorks GetModel(int Kid)
		{
			
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select  top 1 Kid,Ksid,Klid,Ktime,Kseconds,Kcheck,Kstar from MenuWorks ");
			strSql.Append(" where Kid=@Kid ");
			SqlParameter[] parameters = {
					new SqlParameter("@Kid", SqlDbType.Int,4)			};
			parameters[0].Value = Kid;

			LearnSite.Model.MenuWorks model=new LearnSite.Model.MenuWorks();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				return DataRowToModel(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LearnSite.Model.MenuWorks DataRowToModel(DataRow row)
		{
			LearnSite.Model.MenuWorks model=new LearnSite.Model.MenuWorks();
			if (row != null)
			{
				if(row["Kid"]!=null && row["Kid"].ToString()!="")
				{
					model.Kid=int.Parse(row["Kid"].ToString());
				}
				if(row["Ksid"]!=null && row["Ksid"].ToString()!="")
				{
					model.Ksid=int.Parse(row["Ksid"].ToString());
				}
				if(row["Klid"]!=null && row["Klid"].ToString()!="")
				{
					model.Klid=int.Parse(row["Klid"].ToString());
				}
				if(row["Ktime"]!=null && row["Ktime"].ToString()!="")
				{
					model.Ktime=int.Parse(row["Ktime"].ToString());
				}
				if(row.Table.Columns.Contains("Kseconds") && row["Kseconds"]!=null && row["Kseconds"].ToString()!="")
				{
					model.Kseconds=int.Parse(row["Kseconds"].ToString());
				}
				if(row["Kcheck"]!=null && row["Kcheck"].ToString()!="")
				{
					if((row["Kcheck"].ToString()=="1")||(row["Kcheck"].ToString().ToLower()=="true"))
					{
						model.Kcheck=true;
					}
					else
					{
						model.Kcheck=false;
					}
                }
                if (row["Kstar"] != null && row["Kstar"].ToString() != "")
                {
                    model.Kstar = int.Parse(row["Kstar"].ToString());
                }
			}
			return model;
		}

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public ArrayList GetMyListLid(int Ksid)
        {
            ArrayList mylids = new ArrayList();
            string strWhere = "Ksid='" + Ksid.ToString() + "'";
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Kid,Ksid,Klid,Ktime,Kseconds,Kcheck,Kstar ");
            strSql.Append(" FROM MenuWorks ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            DataTable dt = DbHelperSQL.Query(strSql.ToString()).Tables[0];
            int count = dt.Rows.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    mylids.Add(dt.Rows[i]["Klid"]);
                }
            }
            return mylids;
        }

        /// <summary>
        /// 查询完成的活动记录数
        /// </summary>
        /// <param name="Ksid"></param>
        /// <param name="lidall"></param>
        /// <returns></returns>
        public int GetMyLidCount(int Ksid,string lidall) {
            string mysql = "select count(*) FROM MenuWorks  WHERE Ksid=" + Ksid.ToString() + " and Klid in (" + lidall + ")";
            return DbHelperSQL.FindNum(mysql);
        }
        /// <summary>
        /// 查询完成的活动记录数
        /// </summary>
        /// <param name="Ksid"></param>
        /// <param name="lidall"></param>
        /// <returns></returns>
        public ArrayList GetMyLids(int Ksid, string lidall)
        {
            string mysql = "select Klid FROM MenuWorks  WHERE Ksid=" + Ksid.ToString() + " and Klid in (" + lidall + ")";
            DataTable dt = DbHelperSQL.Query(mysql).Tables[0];
            int count= dt.Rows.Count;
            ArrayList array=new ArrayList();
            if (count > 0)
            {
                for (int i = 0; i < count; i++) {
                    array.Add(dt.Rows[i]);                
                }           
            }
            
            return array;
        }
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Kid,Ksid,Klid,Ktime,Kseconds,Kcheck,Kstar ");
			strSql.Append(" FROM MenuWorks ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
            strSql.Append(" Kid,Ksid,Klid,Ktime,Kseconds,Kcheck,Kstar ");
			strSql.Append(" FROM MenuWorks ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM MenuWorks ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			object obj = DbHelperSQL.GetSingle(strSql.ToString());
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.Kid desc");
			}
			strSql.Append(")AS Row, T.*  from MenuWorks T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "MenuWorks";
			parameters[1].Value = "Kid";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}
