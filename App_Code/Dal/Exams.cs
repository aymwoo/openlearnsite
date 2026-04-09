using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using LearnSite.DBUtility;//Please add references
namespace LearnSite.DAL
{
	/// <summary>
	/// 数据访问类:Exams
	/// </summary>
	public partial class Exams
	{
		public Exams()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LearnSite.Model.Exams model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Exams(");
			strSql.Append("Etitle,Edescription,Cid,Hid,Etime,Eclose,Escore,Ecount,Edata)");
			strSql.Append(" values (");
			strSql.Append("@Etitle,@Edescription,@Cid,@Hid,@Etime,@Eclose,@Escore,@Ecount,@Edata)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Etitle", SqlDbType.NVarChar,200),
					new SqlParameter("@Edescription", SqlDbType.NVarChar,500),
					new SqlParameter("@Cid", SqlDbType.Int,4),
					new SqlParameter("@Hid", SqlDbType.Int,4),
					new SqlParameter("@Etime", SqlDbType.DateTime),
					new SqlParameter("@Eclose", SqlDbType.Bit,1),
					new SqlParameter("@Escore", SqlDbType.Int,4),
					new SqlParameter("@Ecount", SqlDbType.Int,4),
					new SqlParameter("@Edata", SqlDbType.NText)};
			parameters[0].Value = model.Etitle;
			parameters[1].Value = model.Edescription;
			parameters[2].Value = model.Cid;
			parameters[3].Value = model.Hid;
			parameters[4].Value = model.Etime;
			parameters[5].Value = model.Eclose;
			parameters[6].Value = model.Escore;
			parameters[7].Value = model.Ecount;
			parameters[8].Value = model.Edata;

			object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);
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
		/// 更新一条数据
		/// </summary>
		public bool Update(LearnSite.Model.Exams model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Exams set ");
			strSql.Append("Etitle=@Etitle,");
			strSql.Append("Edescription=@Edescription,");
			strSql.Append("Cid=@Cid,");
			strSql.Append("Hid=@Hid,");
			strSql.Append("Etime=@Etime,");
			strSql.Append("Eclose=@Eclose,");
			strSql.Append("Escore=@Escore,");
			strSql.Append("Ecount=@Ecount,");
			strSql.Append("Edata=@Edata");
			strSql.Append(" where Eid=@Eid");
			SqlParameter[] parameters = {
					new SqlParameter("@Etitle", SqlDbType.NVarChar,200),
					new SqlParameter("@Edescription", SqlDbType.NVarChar,500),
					new SqlParameter("@Cid", SqlDbType.Int,4),
					new SqlParameter("@Hid", SqlDbType.Int,4),
					new SqlParameter("@Etime", SqlDbType.DateTime),
					new SqlParameter("@Eclose", SqlDbType.Bit,1),
					new SqlParameter("@Escore", SqlDbType.Int,4),
					new SqlParameter("@Ecount", SqlDbType.Int,4),
					new SqlParameter("@Edata", SqlDbType.NText),
					new SqlParameter("@Eid", SqlDbType.Int,4)};
			parameters[0].Value = model.Etitle;
			parameters[1].Value = model.Edescription;
			parameters[2].Value = model.Cid;
			parameters[3].Value = model.Hid;
			parameters[4].Value = model.Etime;
			parameters[5].Value = model.Eclose;
			parameters[6].Value = model.Escore;
			parameters[7].Value = model.Ecount;
			parameters[8].Value = model.Edata;
			parameters[9].Value = model.Eid;

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
		/// 删除一条数据
		/// </summary>
		public bool Delete(int Eid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Exams ");
			strSql.Append(" where Eid=@Eid");
			SqlParameter[] parameters = {
					new SqlParameter("@Eid", SqlDbType.Int,4)
			};
			parameters[0].Value = Eid;

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
		public bool DeleteList(string Eidlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Exams ");
			strSql.Append(" where Eid in ("+Eidlist + ")  ");
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
		public LearnSite.Model.Exams GetModel(int Eid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Eid,Etitle,Edescription,Cid,Hid,Etime,Eclose,Escore,Ecount,Edata from Exams ");
			strSql.Append(" where Eid=@Eid");
			SqlParameter[] parameters = {
					new SqlParameter("@Eid", SqlDbType.Int,4)
			};
			parameters[0].Value = Eid;

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
        public LearnSite.Model.Exams GetModelDataRow(DataTable dt, int Tsort)
        {
            DataRow row = dt.Rows[Tsort];
            return DataRowToModel(row);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public LearnSite.Model.Exams DataRowToModel(DataRow row)
		{
			LearnSite.Model.Exams model=new LearnSite.Model.Exams();
			if (row != null)
			{
				if(row["Eid"]!=null && row["Eid"].ToString()!="")
				{
					model.Eid=int.Parse(row["Eid"].ToString());
				}
				if(row["Etitle"]!=null)
				{
					model.Etitle=row["Etitle"].ToString();
				}
				if(row["Edescription"]!=null)
				{
					model.Edescription=row["Edescription"].ToString();
				}
				if(row["Cid"]!=null && row["Cid"].ToString()!="")
				{
					model.Cid=int.Parse(row["Cid"].ToString());
				}
				if(row["Hid"]!=null && row["Hid"].ToString()!="")
				{
					model.Hid=int.Parse(row["Hid"].ToString());
				}
				if(row["Etime"]!=null && row["Etime"].ToString()!="")
				{
					model.Etime=DateTime.Parse(row["Etime"].ToString());
				}
				if(row["Eclose"]!=null && row["Eclose"].ToString()!="")
				{
					if((row["Eclose"].ToString()=="1")||(row["Eclose"].ToString().ToLower()=="true"))
					{
						model.Eclose=true;
					}
					else
					{
						model.Eclose=false;
					}
				}
				if(row["Escore"]!=null && row["Escore"].ToString()!="")
				{
					model.Escore=int.Parse(row["Escore"].ToString());
				}
				if(row["Ecount"]!=null && row["Ecount"].ToString()!="")
				{
					model.Ecount=int.Parse(row["Ecount"].ToString());
				}
				if(row["Edata"]!=null)
				{
					model.Edata=row["Edata"].ToString();
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Eid,Etitle,Edescription,Cid,Hid,Etime,Eclose,Escore,Ecount,Edata ");
			strSql.Append(" FROM Exams ");
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
			strSql.Append(" Eid,Etitle,Edescription,Cid,Hid,Etime,Eclose,Escore,Ecount,Edata ");
			strSql.Append(" FROM Exams ");
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
			strSql.Append("select count(1) FROM Exams ");
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
				strSql.Append("order by T.Eid desc");
			}
			strSql.Append(")AS Row, T.*  from Exams T ");
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
			parameters[0].Value = "Exams";
			parameters[1].Value = "Eid";
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

