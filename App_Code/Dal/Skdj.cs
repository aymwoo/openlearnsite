using System;
using System.Data;
using System.Data.SqlClient;
using LearnSite.Common;
using System.Text;
using LearnSite.DBUtility;//请先添加引用
using System.Web;
using System.IO;

namespace LearnSite.DAL
{
    public class Skdj
    {
        public DataSet GetAllList()
        {
            string sql = "SELECT * FROM Skdj";
            return DbHelperSQL.Query(sql);
        }

        public int addskdj(int Sstid, DateTime Ssdate, int Ssgrade, int Ssclass, string Ssctitle, int Ssyear, int Ssmonth, int Ssday, string Ssweek, string Ssession, string Ssnotes, string Sstname)
        {
            StringBuilder strSql = new StringBuilder();
            string sql = @"INSERT INTO Skdj (Sstid, Ssdate, Ssgrade, Ssclass, Ssctitle, Ssyear, Ssmonth, Ssday, Ssweek, Ssession, Ssnotes, Sstname) 
                   VALUES (@Sstid, @Ssdate, @Ssgrade, @Ssclass, @Ssctitle, @Ssyear, @Ssmonth, @Ssday, @Ssweek, @Ssession, @Ssnotes, @Sstname);
                   SELECT SCOPE_IDENTITY();";
            /*string sql = "INSERT INTO Skdj (Sstid, Ssdate, Ssgrade, Ssclass, Ssctitle, Ssyear, Ssmonth, Ssday, Ssweek, Ssession, Ssnotes) " +
                         "VALUES (@Sstid, @Ssdate, @Ssgrade, @Ssclass, @Ssctitle, @Ssyear, @Ssmonth, @Ssday, @Ssweek, @Ssession, @Ssnotes)";
                         SELECT SCOPE_IDENTITY();";*/

            SqlParameter[] parameters = {
                new SqlParameter("@Sstid", SqlDbType.Int),
                new SqlParameter("@Ssdate", SqlDbType.DateTime),
                new SqlParameter("@Ssgrade", SqlDbType.Int),
                new SqlParameter("@Ssclass", SqlDbType.Int),
                new SqlParameter("@Ssctitle", SqlDbType.NVarChar, 100),
                new SqlParameter("@Ssyear", SqlDbType.Int),
                new SqlParameter("@Ssmonth", SqlDbType.Int),
                new SqlParameter("@Ssday", SqlDbType.Int),
                new SqlParameter("@Ssweek", SqlDbType.NVarChar, 10),
                new SqlParameter("@Ssession", SqlDbType.NVarChar, 10),
                new SqlParameter("@Ssnotes", SqlDbType.NVarChar, 100),
                new SqlParameter("@Sstname", SqlDbType.NVarChar, 50)
        };

            parameters[0].Value = Sstid;
        parameters[1].Value = Ssdate;
        parameters[2].Value = Ssgrade;
        parameters[3].Value = Ssclass;
        parameters[4].Value = Ssctitle;
        parameters[5].Value = Ssyear;
        parameters[6].Value = Ssmonth;
        parameters[7].Value = Ssday;
        parameters[8].Value = Ssweek;
        parameters[9].Value = Ssession;
        parameters[10].Value = Ssnotes;
        parameters[11].Value = Sstname;

            //DbHelperSQL.ExecuteNonQuery(sql, parameters);
            object obj = DbHelperSQL.GetSingle(sql, parameters);
            return obj == null ? 0 : Convert.ToInt32(obj);
            //object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            //return obj == null ? 0 : Convert.ToInt32(obj);
            /*if (obj == null)
            {
                return 1;// 表示插入成功
            }
            else
            {
                return Convert.ToInt32(obj);// 或者其他表示失败的值
            }*/
        }

        public bool Exists(string condition)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Skdj");
            if (!string.IsNullOrEmpty(condition))
            {
               strSql.Append(" where " + condition);
            }
               return DbHelperSQL.Exists(strSql.ToString());
        }
        
        public int Add(LearnSite.Model.Skdj model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Skdj(");
            strSql.Append("Sstid,Ssgrade,Ssclass,Ssctitle,Ssdate,Ssyear,Ssmonth,Ssday,Ssweek,Ssnotes,Sstname)");
            strSql.Append(" values (");
            strSql.Append("@Sstid,@Ssgrade,@Ssclass,@Ssctitle,@Ssdate,@Ssyear,@Ssmonth,@Ssday,@Ssweek,@Ssnotes,@Sstname)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                new SqlParameter("@Sstid", SqlDbType.Int,4),
                new SqlParameter("@Ssgrade", SqlDbType.Int,4),
                new SqlParameter("@Ssclass", SqlDbType.Int,4),
                new SqlParameter("@Ssctitle", SqlDbType.NVarChar,50),
                new SqlParameter("@Ssdate", SqlDbType.DateTime),
                new SqlParameter("@Ssyear", SqlDbType.Int,4),
                new SqlParameter("@Ssmonth", SqlDbType.Int,4),
                new SqlParameter("@Ssday", SqlDbType.Int,4),
                new SqlParameter("@Ssweek", SqlDbType.NVarChar,10),
                new SqlParameter("@Ssnotes", SqlDbType.NVarChar, 100),
                new SqlParameter("@Sstname", SqlDbType.NVarChar, 50)

            };
            parameters[0].Value = model.Sstid;
            parameters[1].Value = model.Ssgrade;
            parameters[2].Value = model.Ssclass;
            parameters[3].Value = model.Ssctitle;
            parameters[4].Value = model.Ssdate;
            parameters[5].Value = model.Ssyear;
            parameters[6].Value = model.Ssmonth;
            parameters[7].Value = model.Ssday;
            parameters[8].Value = model.Ssweek;
            parameters[9].Value = model.Ssnotes;
            parameters[10].Value = model.Sstname;
    
       // 执行SQL语句并返回新插入记录的ID
            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                 return 0;   // 插入失败
            }
            else
            {
                return Convert.ToInt32(obj);   // 返回新插入记录的ID
            } 
        }   
    public bool Delete(int id)
    {
        StringBuilder strSql = new StringBuilder();
        strSql.Append("DELETE FROM Skdj ");
        strSql.Append(" WHERE Ssid=@Ssid");
        SqlParameter[] parameters = {
            new SqlParameter("@Ssid", SqlDbType.Int)
    };
        parameters[0].Value = id;

        int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        return rows > 0;
    }
    public LearnSite.Model.Skdj GetModel(int id)
{
    StringBuilder strSql = new StringBuilder();
    strSql.Append("SELECT * FROM Skdj ");
    strSql.Append(" WHERE Ssid=@Ssid");
    SqlParameter[] parameters = {
        new SqlParameter("@Ssid", SqlDbType.Int)
    };
    parameters[0].Value = id;

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

private LearnSite.Model.Skdj DataRowToModel(DataRow row)
{
    LearnSite.Model.Skdj model = new LearnSite.Model.Skdj();
    
    if (row["Ssid"] != null && row["Ssid"].ToString() != "")
        model.Ssid = int.Parse(row["Ssid"].ToString());
    if (row["Sstid"] != null && row["Sstid"].ToString() != "")
        model.Sstid = int.Parse(row["Sstid"].ToString());
    if (row["Ssdate"] != null && row["Ssdate"].ToString() != "")
        model.Ssdate = DateTime.Parse(row["Ssdate"].ToString());
    if (row["Ssyear"] != null && row["Ssyear"].ToString() != "")
        model.Ssyear = int.Parse(row["Ssyear"].ToString());
    if (row["Ssmonth"] != null && row["Ssmonth"].ToString() != "")
        model.Ssmonth = int.Parse(row["Ssmonth"].ToString());
    if (row["Ssday"] != null && row["Ssday"].ToString() != "")
        model.Ssday = int.Parse(row["Ssday"].ToString());
    if (row["Ssweek"] != null)
        model.Ssweek = row["Ssweek"].ToString();
    if (row["Ssgrade"] != null && row["Ssgrade"].ToString() != "")
        model.Ssgrade = int.Parse(row["Ssgrade"].ToString());
    if (row["Ssclass"] != null && row["Ssclass"].ToString() != "")
        model.Ssclass = int.Parse(row["Ssclass"].ToString());
    if (row["Ssctitle"] != null)
        model.Ssctitle = row["Ssctitle"].ToString();
    if (row["Sstname"] != null)
        model.Sstname = row["Sstname"].ToString();
    if (row["Ssnotes"] != null)
        model.Ssnotes = row["Ssnotes"].ToString();
    
    return model;
}

    public bool Update(LearnSite.Model.Skdj model)
{
    StringBuilder strSql = new StringBuilder();
    strSql.Append("UPDATE Skdj SET ");
    strSql.Append("Sstid=@Sstid,");
    strSql.Append("Ssgrade=@Ssgrade,");
    strSql.Append("Ssclass=@Ssclass,");
    strSql.Append("Ssctitle=@Ssctitle,");
    strSql.Append("Ssdate=@Ssdate,");
    strSql.Append("Ssyear=@Ssyear,");
    strSql.Append("Ssmonth=@Ssmonth,");
    strSql.Append("Ssday=@Ssday,");
    strSql.Append("Ssweek=@Ssweek,");
    strSql.Append("Ssnotes=@Ssnotes,");
    strSql.Append("Sstname=@Sstname");
    strSql.Append(" WHERE Ssid=@Ssid");
    
    SqlParameter[] parameters = {
        new SqlParameter("@Sstid", SqlDbType.Int),
        new SqlParameter("@Ssgrade", SqlDbType.Int),
        new SqlParameter("@Ssclass", SqlDbType.Int),
        new SqlParameter("@Ssctitle", SqlDbType.NVarChar, 50),
        new SqlParameter("@Ssdate", SqlDbType.DateTime),
        new SqlParameter("@Ssyear", SqlDbType.Int),
        new SqlParameter("@Ssmonth", SqlDbType.Int),
        new SqlParameter("@Ssday", SqlDbType.Int),
        new SqlParameter("@Ssweek", SqlDbType.NVarChar, 10),
        new SqlParameter("@Ssnotes", SqlDbType.NVarChar, 100),
        new SqlParameter("@Sstname", SqlDbType.NVarChar, 50),
        new SqlParameter("@Ssid", SqlDbType.Int)
    };
    
    // 参数赋值
    parameters[0].Value = model.Sstid;
    parameters[1].Value = model.Ssgrade;
    parameters[2].Value = model.Ssclass;
    parameters[3].Value = model.Ssctitle;
    parameters[4].Value = model.Ssdate;
    parameters[5].Value = model.Ssyear;
    parameters[6].Value = model.Ssmonth;
    parameters[7].Value = model.Ssday;
    parameters[8].Value = model.Ssweek;
    parameters[9].Value = model.Ssnotes;
    parameters[10].Value = model.Sstname;
    parameters[11].Value = model.Ssid;
    
    int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
    return rows > 0;
}

/// <summary>
/// 获取今日课程记录
/// </summary>
public DataTable GetTodayRecords()
{
    string sql = "SELECT Ssdate, Ssgrade, Ssclass, Ssctitle as Stitle, Sstname as Ssname FROM Skdj WHERE CONVERT(date,Ssdate)=CONVERT(date,GETDATE()) ORDER BY Ssdate DESC";
    return DbHelperSQL.Query(sql).Tables[0];
}

/// <summary>
/// 导出上课登记表到Excel
/// </summary>
/// <param name="currentYearOnly">是否只导出当前年份</param>
public void SkdjExcel(bool currentYearOnly)
{
    DateTime dt = DateTime.Now;
    string today = dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day;
    string fileName = currentYearOnly ?
        "上课登记表_" + dt.Year.ToString() + "年_" + today :
        "上课登记表_全部记录_" + today;

    StringBuilder strSql = new StringBuilder();
    strSql.Append("SELECT Ssyear AS 年份, Ssmonth AS 月, Ssday AS 日, Ssweek AS 星期, ");
    strSql.Append("Ssgrade AS 年级, Ssclass AS 班级, Ssctitle AS 上课内容, Sstname AS 上课教师, Ssnotes AS 备注 ");
    strSql.Append("FROM Skdj ");

    if (currentYearOnly)
    {
        strSql.Append("WHERE Ssyear = @Ssyear ");
    }

    strSql.Append("ORDER BY Ssyear ASC, Ssmonth ASC, Ssday ASC");

    SqlParameter[] parameters = null;
    if (currentYearOnly)
    {
        parameters = new SqlParameter[]
        {
            new SqlParameter("@Ssyear", SqlDbType.Int)
        };
        parameters[0].Value = dt.Year;
    }

    DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);

    // 使用自定义方法导出，添加表头标题和边框
    SkdjExcelWithHeader(ds, fileName);
}

/// <summary>
/// 导出上课登记表到Excel（带表头标题和边框）
/// </summary>
private void SkdjExcelWithHeader(DataSet ds, string fileName)
{
    try
    {
        HttpResponse resp = HttpContext.Current.Response;
        resp.Clear();
        resp.Buffer = true;
        resp.Charset = "gb2312";
        resp.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        resp.AppendHeader("Content-disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(fileName + ".xls", System.Text.Encoding.UTF8));
        resp.ContentType = "application/ms-excel";

        StringWriter sfw = new StringWriter();
        DataTable dt = ds.Tables[0];

        // 写入表头标题（居中、加粗）
        sfw.WriteLine("<html xmlns:x=\"urn:schemas-microsoft-com:office:excel\">");
        sfw.WriteLine("<head>");
        sfw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html;charset=gb2312\">");
        sfw.WriteLine("<style>");
        sfw.WriteLine("table {border-collapse: collapse; mso-excel-xformy: no;}");
        sfw.WriteLine("td {border: 0.5pt solid black; padding: 5px; text-align: center;}");
        sfw.WriteLine("th {border: 0.5pt solid black; padding: 5px; font-weight: bold; text-align: center;}");
        sfw.WriteLine(".header {font-size: 16pt; font-weight: bold; text-align: center;}");
        sfw.WriteLine("</style>");
        sfw.WriteLine("</head>");
        sfw.WriteLine("<body>");

        // 表头标题行
        sfw.WriteLine("<table>");
        sfw.WriteLine("<tr><td colspan=\"9\" class=\"header\">信息科技上课登记表</td></tr>");

        // 写入列标题
        sfw.WriteLine("<tr>");
        sfw.WriteLine("<th>序号</th>");
        sfw.WriteLine("<th>年份</th>");
        sfw.WriteLine("<th>月</th>");
        sfw.WriteLine("<th>日</th>");
        sfw.WriteLine("<th>星期</th>");
        sfw.WriteLine("<th>年级</th>");
        sfw.WriteLine("<th>班级</th>");
        sfw.WriteLine("<th>上课内容</th>");
        sfw.WriteLine("<th>上课教师</th>");
        sfw.WriteLine("<th>备注</th>");
        sfw.WriteLine("</tr>");

        // 写入数据行
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            sfw.WriteLine("<tr>");
            sfw.WriteLine("<td>" + (i + 1).ToString() + "</td>");
            sfw.WriteLine("<td>" + dt.Rows[i]["年份"].ToString() + "</td>");
            sfw.WriteLine("<td>" + dt.Rows[i]["月"].ToString() + "</td>");
            sfw.WriteLine("<td>" + dt.Rows[i]["日"].ToString() + "</td>");
            sfw.WriteLine("<td>" + dt.Rows[i]["星期"].ToString() + "</td>");
            sfw.WriteLine("<td>" + dt.Rows[i]["年级"].ToString() + "</td>");
            sfw.WriteLine("<td>" + dt.Rows[i]["班级"].ToString() + "</td>");
            sfw.WriteLine("<td>" + dt.Rows[i]["上课内容"].ToString() + "</td>");
            sfw.WriteLine("<td>" + dt.Rows[i]["上课教师"].ToString() + "</td>");
            sfw.WriteLine("<td>" + dt.Rows[i]["备注"].ToString() + "</td>");
            sfw.WriteLine("</tr>");
        }

        sfw.WriteLine("</table>");
        sfw.WriteLine("</body>");
        sfw.WriteLine("</html>");

        resp.Write(sfw.ToString());
        resp.Flush();
    }
    catch (Exception e)
    {
        throw e;
    }
    finally
    {
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }
}
    }

}       