using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using LearnSite.DBUtility;//Please add references
namespace LearnSite.DAL
{
	/// <summary>
	/// 数据访问类:Turtle
	/// </summary>
	public partial class Turtle
	{
		public Turtle()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Tid", "Turtle"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Tid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Turtle");
			strSql.Append(" where Tid=@Tid");
			SqlParameter[] parameters = {
					new SqlParameter("@Tid", SqlDbType.Int,4)
			};
			parameters[0].Value = Tid;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(LearnSite.Model.Turtle model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Turtle(");
			strSql.Append("Thid,Ttilte,Tcontent,Tdegree,Tsort,Tcode,Timg,Turl,Tout,Tdate,Tstudy,Tsid,Tscore,Tip)");
			strSql.Append(" values (");
            strSql.Append("@Thid,@Ttilte,@Tcontent,@Tdegree,@Tsort,@Tcode,@Timg,@Turl,@Tout,@Tdate,@Tstudy,@Tsid,@Tscore,@Tip)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Thid", SqlDbType.Int,4),
					new SqlParameter("@Ttilte", SqlDbType.NVarChar,50),
					new SqlParameter("@Tcontent", SqlDbType.NText),
					new SqlParameter("@Tdegree", SqlDbType.Int,4),
					new SqlParameter("@Tsort", SqlDbType.Int,4),
					new SqlParameter("@Tcode", SqlDbType.NText),
					new SqlParameter("@Timg", SqlDbType.NVarChar,50),
					new SqlParameter("@Turl", SqlDbType.NVarChar,50),
					new SqlParameter("@Tout", SqlDbType.NVarChar,200),
					new SqlParameter("@Tdate", SqlDbType.DateTime),
					new SqlParameter("@Tstudy", SqlDbType.Bit),
					new SqlParameter("@Tsid", SqlDbType.Int,4),
					new SqlParameter("@Tscore", SqlDbType.Int,4),
					new SqlParameter("@Tip", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.Thid;
			parameters[1].Value = model.Ttilte;
			parameters[2].Value = model.Tcontent;
			parameters[3].Value = model.Tdegree;
			parameters[4].Value = model.Tsort;
			parameters[5].Value = model.Tcode;
			parameters[6].Value = model.Timg;
			parameters[7].Value = model.Turl;
			parameters[8].Value = model.Tout;
            parameters[9].Value = model.Tdate;
            parameters[10].Value = model.Tstudy;
            parameters[11].Value = model.Tsid;
            parameters[12].Value = model.Tscore;
            parameters[13].Value = model.Tip;

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
		public bool Update(LearnSite.Model.Turtle model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Turtle set ");
			strSql.Append("Ttilte=@Ttilte,");
			strSql.Append("Tcontent=@Tcontent,");
			strSql.Append("Tdegree=@Tdegree,");
			strSql.Append("Tsort=@Tsort,");
			strSql.Append("Tcode=@Tcode,");
			strSql.Append("Timg=@Timg,");
			strSql.Append("Turl=@Turl,");
			strSql.Append("Tout=@Tout,");
            strSql.Append("Tdate=@Tdate,");
            strSql.Append("Tstudy=@Tstudy,");
            strSql.Append("Tscore=@Tscore,");
            strSql.Append("Tip=@Tip");
            strSql.Append(" where Tid=@Tid and Thid=@Thid and Tsid=@Tsid ");
			SqlParameter[] parameters = {
					new SqlParameter("@Thid", SqlDbType.Int,4),
					new SqlParameter("@Ttilte", SqlDbType.NVarChar,50),
					new SqlParameter("@Tcontent", SqlDbType.NText),
					new SqlParameter("@Tdegree", SqlDbType.Int,4),
					new SqlParameter("@Tsort", SqlDbType.Int,4),
					new SqlParameter("@Tcode", SqlDbType.NText),
					new SqlParameter("@Timg", SqlDbType.NVarChar,50),
					new SqlParameter("@Turl", SqlDbType.NVarChar,50),
					new SqlParameter("@Tout", SqlDbType.NVarChar,200),
					new SqlParameter("@Tdate", SqlDbType.DateTime),
					new SqlParameter("@Tid", SqlDbType.Int,4),
					new SqlParameter("@Tstudy", SqlDbType.Bit),
					new SqlParameter("@Tsid", SqlDbType.Int,4),
					new SqlParameter("@Tscore", SqlDbType.Int,4),
					new SqlParameter("@Tip", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.Thid;
			parameters[1].Value = model.Ttilte;
			parameters[2].Value = model.Tcontent;
			parameters[3].Value = model.Tdegree;
			parameters[4].Value = model.Tsort;
			parameters[5].Value = model.Tcode;
			parameters[6].Value = model.Timg;
			parameters[7].Value = model.Turl;
			parameters[8].Value = model.Tout;
			parameters[9].Value = model.Tdate;
            parameters[10].Value = model.Tid;
            parameters[11].Value = model.Tstudy;
            parameters[12].Value = model.Tsid;
            parameters[13].Value = model.Tscore;
            parameters[14].Value = model.Tip;

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
		public bool Delete(int Tid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Turtle ");
			strSql.Append(" where Tid=@Tid");
			SqlParameter[] parameters = {
					new SqlParameter("@Tid", SqlDbType.Int,4)
			};
			parameters[0].Value = Tid;

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
		public bool DeleteList(string Tidlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Turtle ");
			strSql.Append(" where Tid in ("+Tidlist + ")  ");
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
		public LearnSite.Model.Turtle GetModel(int Tid)
		{
			
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select  top 1 Tid,Thid,Ttilte,Tcontent,Tdegree,Tsort,Tcode,Timg,Turl,Tout,Tdate,Tstudy,Tsid,Tscore,Tip from Turtle ");
			strSql.Append(" where Tid=@Tid");
			SqlParameter[] parameters = {
					new SqlParameter("@Tid", SqlDbType.Int,4)
			};
			parameters[0].Value = Tid;

			LearnSite.Model.Turtle model=new LearnSite.Model.Turtle();
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
		public LearnSite.Model.Turtle DataRowToModel(DataRow row)
		{
			LearnSite.Model.Turtle model=new LearnSite.Model.Turtle();
			if (row != null)
			{
				if(row["Tid"]!=null && row["Tid"].ToString()!="")
				{
					model.Tid=int.Parse(row["Tid"].ToString());
				}
				if(row["Thid"]!=null && row["Thid"].ToString()!="")
				{
					model.Thid=int.Parse(row["Thid"].ToString());
				}
				if(row["Ttilte"]!=null)
				{
					model.Ttilte=row["Ttilte"].ToString();
				}
				if(row["Tcontent"]!=null)
				{
					model.Tcontent=row["Tcontent"].ToString();
				}
				if(row["Tdegree"]!=null && row["Tdegree"].ToString()!="")
				{
					model.Tdegree=int.Parse(row["Tdegree"].ToString());
				}
				if(row["Tsort"]!=null && row["Tsort"].ToString()!="")
				{
					model.Tsort=int.Parse(row["Tsort"].ToString());
				}
				if(row["Tcode"]!=null)
				{
					model.Tcode=row["Tcode"].ToString();
				}
				if(row["Timg"]!=null)
				{
					model.Timg=row["Timg"].ToString();
				}
				if(row["Turl"]!=null)
				{
					model.Turl=row["Turl"].ToString();
				}
				if(row["Tout"]!=null)
				{
					model.Tout=row["Tout"].ToString();
				}
				if(row["Tdate"]!=null && row["Tdate"].ToString()!="")
				{
					model.Tdate=DateTime.Parse(row["Tdate"].ToString());
				}
                if (row["Tstudy"] != null && row["Tstudy"].ToString() != "")
                {
                    if ((row["Tstudy"].ToString() == "1") || (row["Tstudy"].ToString().ToLower() == "true"))
                    {
                        model.Tstudy = true;
                    }
                    else
                    {
                        model.Tstudy = false;
                    }
                }

                if (row["Tsid"] != null && row["Tsid"].ToString() != "")
                {
                    model.Tsid = int.Parse(row["Tsid"].ToString());
                }
                if (row["Tscore"] != null && row["Tscore"].ToString() != "")
                {
                    model.Tscore = int.Parse(row["Tscore"].ToString());
                }
                if (row["Tip"] != null)
                {
                    model.Tip = row["Tip"].ToString();
                }
			}
			return model;
		}

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public LearnSite.Model.Turtle GetModel(DataTable dt, int Tsort)
        {
            LearnSite.Model.Turtle model = new LearnSite.Model.Turtle();
            int Count = dt.Rows.Count;
            if (Count > 0)
            {
                if (Tsort < Count)
                {
                    return DataRowToModel(dt.Rows[Tsort]);
                }
                return null;
            }
            else
            {
                return null;
            }
        }

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataTable GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select Tid,Thid,Ttilte,Tcontent,Tdegree,Tsort,Tcode,Timg,Turl,Tout,Tdate,Tstudy,Tsid,Tscore,Tip ");
			strSql.Append(" FROM Turtle ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}

           // LearnSite.Common.Log.Addlog("绘画编程作品查询：", strSql.ToString());

            DataSet ds=DbHelperSQL.Query(strSql.ToString());

            DataTable dt = ds.Tables[0];
            DataColumn col = new DataColumn("Tpage", typeof(string));//新增链接字段
            dt.Columns.Add(col);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                object tstudy = dt.Rows[i]["Tstudy"];
                if ((tstudy.ToString() == "1") || (tstudy.ToString().ToLower() == "true"))
                {
                    dt.Rows[i]["Tpage"] = "idle.aspx?id=" + dt.Rows[i]["Tid"].ToString(); 
                }
                else
                {
                    dt.Rows[i]["Tpage"] = "turtle.aspx?id=" + dt.Rows[i]["Tid"].ToString();
                }
            }

            return dt;
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
            strSql.Append(" Tid,Thid,Ttilte,Tcontent,Tdegree,Tsort,Tcode,Timg,Turl,Tout,Tdate,Tstudy,Tsid,Tscore,Tip ");
			strSql.Append(" FROM Turtle ");
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
			strSql.Append("select count(1) FROM Turtle ");
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
				strSql.Append("order by T.Tid desc");
			}
			strSql.Append(")AS Row, T.*  from Turtle T ");
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
			parameters[0].Value = "Turtle";
			parameters[1].Value = "Tid";
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

