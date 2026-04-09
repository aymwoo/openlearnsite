using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_signshow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (!IsPostBack)
        {
            Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "签到详细页面";
            ShowSignin();
            ShowNoSign();
        }
    }
    protected void ButtonReturn_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/teacher/signin.aspx", false);
    }

    protected void GVSignin_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //当鼠标放上去的时候 先保存当前行的背景颜色 并给附一颜色 
            e.Row.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#E1E8E1',this.style.fontWeight='';");
            //当鼠标离开的时候 将背景颜色还原的以前的颜色 
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor,this.style.fontWeight='';");
            //单击行改变行背景颜色 
            e.Row.Attributes.Add("onclick", "this.style.backgroundColor='#D8E0D8'; this.style.color='buttontext';this.style.cursor='default';");
        }
    }
    protected void GVNoSign_RowDataBound(object sender, GridViewRowEventArgs e)
    {        
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int Qyear = Int32.Parse(Request.QueryString["qyear"].ToString());
            int Qmonth = Int32.Parse(Request.QueryString["qmonth"].ToString());
            int Qday = Int32.Parse(Request.QueryString["qday"].ToString());
            string Nnum = e.Row.Cells[1].Text;
            LearnSite.BLL.NotSign bll = new LearnSite.BLL.NotSign();
            string Nnote = bll.GetNoteThisday(Nnum,Qyear,Qmonth,Qday);
            e.Row.Cells[10].Text = Nnote;
            //当鼠标放上去的时候 先保存当前行的背景颜色 并给附一颜色 
            e.Row.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='#E1E8E1',this.style.fontWeight='';");
            //当鼠标离开的时候 将背景颜色还原的以前的颜色 
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor,this.style.fontWeight='';");
            //单击行改变行背景颜色 
            e.Row.Attributes.Add("onclick", "this.style.backgroundColor='#D8E0D8'; this.style.color='buttontext';this.style.cursor='default';");
        }
    }
    private void ShowSignin()
    {
        if (Request.QueryString["sgrade"] != null)
        {
            int Sgrade =Int32.Parse( Request.QueryString["sgrade"].ToString());
            int Sclass =Int32.Parse( Request.QueryString["sclass"].ToString());
            int Qyear =Int32.Parse( Request.QueryString["qyear"].ToString());
            int Qmonth =Int32.Parse( Request.QueryString["qmonth"].ToString());
            int Qday =Int32.Parse( Request.QueryString["qday"].ToString());
            string Qtitle = Request.QueryString["qtitle"] != null ? Request.QueryString["qtitle"].ToString() : string.Empty;
            string Qsession = Request.QueryString["qs"] != null ? Request.QueryString["qs"].ToString() : string.Empty; // 添加Qsession参数
            int sort = RBtnList.SelectedIndex;
            LearnSite.BLL.Signin sign = new LearnSite.BLL.Signin();
            GVSignin.DataSource = sign.SignclassdetailSort(Sgrade, Sclass, Qyear, Qmonth, Qday, Qtitle, sort, Qsession);
            GVSignin.DataBind();
            Labelsignin.Text = "[" + GVSignin.Rows.Count.ToString() + "位]";
        }
    }

    private void ShowNoSign()
    {
        if (Request.QueryString["sgrade"] != null)
        {
            int Sgrade = Int32.Parse(Request.QueryString["sgrade"].ToString());
            int Sclass = Int32.Parse(Request.QueryString["sclass"].ToString());
            int Qyear = Int32.Parse(Request.QueryString["qyear"].ToString());
            int Qmonth = Int32.Parse(Request.QueryString["qmonth"].ToString());
            int Qday = Int32.Parse(Request.QueryString["qday"].ToString());
            LearnSite.BLL.Signin sign = new LearnSite.BLL.Signin();            
            GVNoSign.DataSource = sign.NoSignclassdetail(Sgrade, Sclass, Qyear, Qmonth, Qday);
            GVNoSign.DataBind();
            Labelnosign.Text = "[" + GVNoSign.Rows.Count.ToString() + "位]";
        }
    }

    protected void RBtnList_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowSignin();
        ShowNoSign();
    }
    
    protected void RBtnList2_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowSignin();
        ShowNoSign();
    }
    
    protected void BtnExportCurrent_Click(object sender, EventArgs e)
    {
        try
        {
            // 获取当前年份
            int currentYear = DateTime.Now.Year;
            ExportToExcel(true, currentYear);
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('导出失败：" + ex.Message + "');</script>");
        }
    }

    protected void BtnExportAll_Click(object sender, EventArgs e)
    {
        try
        {
            ExportToExcel(false, 0);
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('导出失败：" + ex.Message + "');</script>");
        }
    }

    protected void BtnExportRegister_Click(object sender, EventArgs e)
    {
        try
        {
            // 获取当前年份
            int currentYear = DateTime.Now.Year;
            ExportRegisterToExcel(true, currentYear);
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('导出失败：" + ex.Message + "');</script>");
        }
    }

    protected void BtnExportAllRegister_Click(object sender, EventArgs e)
    {
        try
        {
            ExportRegisterToExcel(false, 0);
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('导出全部登记册失败：" + ex.Message + "');</script>");
        }
    }

    private void ExportRegisterToExcel(bool currentYearOnly, int year)
    {
        try
        {
            // 获取当前登录教师信息
            LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
            string teacherName = HttpContext.Current.Server.UrlDecode(tcook.Hname);
            

            
            // 创建数据表
            System.Data.DataTable dt = new System.Data.DataTable();
            // 明确指定列的数据类型，确保正确排序
            dt.Columns.Add("年", typeof(int));
            dt.Columns.Add("月", typeof(int));
            dt.Columns.Add("日", typeof(int));
            dt.Columns.Add("星期");
            dt.Columns.Add("节次");
            dt.Columns.Add("年级");
            dt.Columns.Add("班级");
            dt.Columns.Add("姓名");
            dt.Columns.Add("上课内容");
            dt.Columns.Add("机号");
            dt.Columns.Add("上课教师");

            // 获取数据
            LearnSite.BLL.Signin bll = new LearnSite.BLL.Signin();
            System.Data.DataSet ds;
            
            if (currentYearOnly)
            {
                ds = bll.GetSigninListByYear(year);
            }
            else
            {
                ds = bll.GetAllSigninList();
            }

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                // 填充数据
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    System.Data.DataRow dr = dt.NewRow();
                    // 使用数值类型存储年月日，确保正确排序
                    dr["年"] = Convert.ToInt32(ds.Tables[0].Rows[i]["Qyear"]);
                    dr["月"] = Convert.ToInt32(ds.Tables[0].Rows[i]["Qmonth"]);
                    dr["日"] = Convert.ToInt32(ds.Tables[0].Rows[i]["Qday"]);
                    dr["星期"] = ConvertWeekToChinese(ds.Tables[0].Rows[i]["Qweek"].ToString());
                    dr["节次"] = ds.Tables[0].Rows[i]["Qsession"].ToString();
                    dr["年级"] = ds.Tables[0].Rows[i]["Qgrade"].ToString();
                    dr["班级"] = ds.Tables[0].Rows[i]["Qclass"].ToString();
                    dr["姓名"] = ds.Tables[0].Rows[i]["Qname"].ToString();
                    dr["上课内容"] = ds.Tables[0].Rows[i]["Qtitle"].ToString();
                    dr["机号"] = ds.Tables[0].Rows[i]["Qmachine"].ToString();
                    dr["上课教师"] = teacherName;
                    dt.Rows.Add(dr);
                }

                // 按日期升序排列（1月排在前面）
                dt.DefaultView.Sort = "年 ASC, 月 ASC, 日 ASC, 节次 ASC";
                dt = dt.DefaultView.ToTable();

                // 设置Excel文件名
                string fileName = currentYearOnly ? 
                    string.Format("登记册_{0}年_{1}.xls", year, DateTime.Now.ToString("yyyyMMdd_HHmmss")) : 
                    string.Format("登记册_全部_{0}.xls", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

                // 设置输出头
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "GB2312";
                Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
                Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
                Response.ContentType = "application/vnd.ms-excel";

                // 输出Excel内容
                Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=GB2312\">");
                // 添加打印样式
                Response.Write("<style type='text/css'>");
                Response.Write("@page {size: A4 portrait; margin-left: 2cm; margin-right: 0.5cm; margin-top: 0.5cm; margin-bottom: 0.5cm;}");
                Response.Write("table {width: 100%; border-collapse: collapse; font-size: 10pt; margin-left: auto; margin-right: 0;}");
                // 调整各列宽度，确保所有列在一页显示
                Response.Write(".col-year, .col-month, .col-day {width: 30px;}");
                Response.Write(".col-week {width: 40px;}");
                Response.Write(".col-session {width: 40px;}");
                Response.Write(".col-grade {width: 50px;}");
                Response.Write(".col-class {width: 60px;}");
                Response.Write(".col-name {width: 70px;}");
                Response.Write(".col-title {width: 140px;}");
                Response.Write(".col-machine {width: 15px;}"); // 机号列宽度
                Response.Write(".col-teacher {width: 120px;}"); // 上课教师列宽度，增加宽度确保显示完整
                Response.Write("td {padding: 3px; text-align: center; vertical-align: middle;}");
                // 确保表头在每页都打印
                Response.Write("thead {display: table-header-group;}");
                // 避免表格行被分页符分割
                Response.Write("tr {page-break-inside: avoid;}");
                // 防止表格过宽
                Response.Write("body {margin: 0; padding: 0;}");
                Response.Write("</style>");
                
                // 添加标题行（居中显示）
                Response.Write("<table border='0' cellspacing='0' cellpadding='0' style='margin-bottom: 20px;'><tr>");
                Response.Write("<td colspan='" + dt.Columns.Count + "' style='text-align:center; font-size:16pt; font-weight:bold; padding:10px;'>");
                Response.Write("信息科技学生上机登记册");
                Response.Write("</td></tr></table>");
                
                // 输出数据表格
                Response.Write("<table border='1' cellspacing='0' cellpadding='0'>");
                // 使用thead确保表头在每页都打印
                Response.Write("<thead>");
                // 输出表头，为各列添加对应的CSS类
                Response.Write("<tr>");
                foreach (System.Data.DataColumn column in dt.Columns)
                {
                    string cssClass = "";
                    switch (column.ColumnName)
                    {
                        case "年": cssClass = "col-year";
                            break;
                        case "月": cssClass = "col-month";
                            break;
                        case "日": cssClass = "col-day";
                            break;
                        case "星期": cssClass = "col-week";
                            break;
                        case "节次": cssClass = "col-session";
                            break;
                        case "年级": cssClass = "col-grade";
                            break;
                        case "班级": cssClass = "col-class";
                            break;
                        case "姓名": cssClass = "col-name";
                            break;
                        case "上课内容": cssClass = "col-title";
                            break;
                        case "机号": cssClass = "col-machine";
                            break;
                        case "上课教师": cssClass = "col-teacher";
                            break;
                    }
                    Response.Write("<td class='" + cssClass + "' style='font-weight:bold;background-color:#cccccc;text-align:center'>" + column.ColumnName + "</td>");
                }
                Response.Write("</tr>");
                Response.Write("</thead>");
                Response.Write("<tbody>");

                // 输出数据行，为各列添加对应的CSS类
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    Response.Write("<tr>");
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string cssClass = "";
                        switch (dt.Columns[i].ColumnName)
                        {
                            case "年": cssClass = "col-year";
                                break;
                            case "月": cssClass = "col-month";
                                break;
                            case "日": cssClass = "col-day";
                                break;
                            case "星期": cssClass = "col-week";
                                break;
                            case "节次": cssClass = "col-session";
                                break;
                            case "年级": cssClass = "col-grade";
                                break;
                            case "班级": cssClass = "col-class";
                                break;
                            case "姓名": cssClass = "col-name";
                                break;
                            case "上课内容": cssClass = "col-title";
                                break;
                            case "机号": cssClass = "col-machine";
                                break;
                            case "上课教师": cssClass = "col-teacher";
                                break;
                        }
                        // 如果是年级列，将数字转换为汉字
                        string cellValue = row[i].ToString();
                        if (dt.Columns[i].ColumnName == "年级")
                        {
                            cellValue = ConvertGradeToChinese(cellValue);
                        }
                        Response.Write("<td class='" + cssClass + "'>" + cellValue + "</td>");
                    }
                    Response.Write("</tr>");
                }
                Response.Write("</tbody>");
                Response.Write("</table>");
                Response.End();
            }
            else
            {
                Response.Write("<script>alert('没有找到可导出的记录！');</script>");
            }
        }
        catch (Exception ex)
        {
            Response.Write("<script>alert('导出登记册失败：" + ex.Message + "');</script>");
        }
    }

    private void ExportToExcel(bool currentYearOnly, int year)
    {
        // 获取当前登录教师信息
        LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
        string teacherName = HttpContext.Current.Server.UrlDecode(tcook.Hname);
        
        // 创建数据表
        System.Data.DataTable dt = new System.Data.DataTable();
        dt.Columns.Add("序号");
        dt.Columns.Add("学号");
        dt.Columns.Add("年级");
        dt.Columns.Add("班级");
        dt.Columns.Add("姓名");
        dt.Columns.Add("作品");
        dt.Columns.Add("表现");
        dt.Columns.Add("备注");
        dt.Columns.Add("组评");
        dt.Columns.Add("分值");
        dt.Columns.Add("机号");
        dt.Columns.Add("节次");
        dt.Columns.Add("上课内容");
        dt.Columns.Add("日期");
        dt.Columns.Add("上课教师");

        // 获取数据
        LearnSite.BLL.Signin bll = new LearnSite.BLL.Signin();
        System.Data.DataSet ds;
        
        if (currentYearOnly)
        {
            ds = bll.GetSigninListByYear(year);
        }
        else
        {
            ds = bll.GetAllSigninList();
        }

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            // 填充数据
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                System.Data.DataRow dr = dt.NewRow();
                dr["序号"] = (i + 1).ToString();
                dr["学号"] = ds.Tables[0].Rows[i]["Qnum"].ToString();
                dr["年级"] = ds.Tables[0].Rows[i]["Qgrade"].ToString();
                dr["班级"] = ds.Tables[0].Rows[i]["Qclass"].ToString();
                dr["姓名"] = ds.Tables[0].Rows[i]["Qname"].ToString();
                dr["作品"] = ds.Tables[0].Rows[i]["Qwork"].ToString();
                dr["表现"] = ds.Tables[0].Rows[i]["Qattitude"].ToString();
                dr["备注"] = ds.Tables[0].Rows[i]["Qnote"].ToString();
                dr["组评"] = ds.Tables[0].Rows[i]["Qgroup"].ToString();
                dr["分值"] = ds.Tables[0].Rows[i]["Qgscore"].ToString();
                dr["机号"] = ds.Tables[0].Rows[i]["Qmachine"].ToString();
                dr["节次"] = ds.Tables[0].Rows[i]["Qsession"].ToString();
                dr["上课内容"] = ds.Tables[0].Rows[i]["Qtitle"].ToString();
                // 使用DateTime类型存储日期，确保正确排序
                DateTime dateValue;
                if (DateTime.TryParse(ds.Tables[0].Rows[i]["Qdate"].ToString(), out dateValue))
                {
                    dr["日期"] = dateValue;
                }
                else
                {
                    dr["日期"] = ds.Tables[0].Rows[i]["Qdate"].ToString();
                }
                dr["上课教师"] = teacherName;
                dt.Rows.Add(dr);
            }

            // 按日期升序排列（1月排在前面）
            dt.DefaultView.Sort = "日期 ASC, 节次 ASC";
            dt = dt.DefaultView.ToTable();

            // 设置Excel文件名
            string fileName = currentYearOnly ? 
                string.Format("签到记录_{0}年_{1}.xls", year, DateTime.Now.ToString("yyyyMMdd_HHmmss")) : 
                string.Format("签到记录_全部_{0}.xls", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            // 设置输出头
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "GB2312";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            Response.ContentType = "application/vnd.ms-excel";

            // 输出Excel内容
            Response.Write("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=GB2312\">");
            // 添加打印样式
            Response.Write("<style type='text/css'>");
                Response.Write("@page {size: A4 portrait; margin-left: 2cm; margin-right: 0.5cm; margin-top: 0.5cm; margin-bottom: 0.5cm;}");
                Response.Write("table {width: 100%; border-collapse: collapse; font-size: 9pt; margin-left: auto; margin-right: 0;}");
                // 调整各列宽度，确保所有列在一页显示
                Response.Write(".col-serial {width: 20px;}");
                Response.Write(".col-id {width: 60px;}");
                Response.Write(".col-grade {width: 40px;}");
                Response.Write(".col-class {width: 50px;}");
                Response.Write(".col-name {width: 60px;}");
                Response.Write(".col-work, .col-attitude, .col-note {width: 70px;}");
                Response.Write(".col-group {width: 40px;}");
                Response.Write(".col-score {width: 30px;}");
                Response.Write(".col-machine {width: 15px;}"); // 机号列宽度
                Response.Write(".col-session {width: 40px;}");
                Response.Write(".col-title {width: 110px;}");
                Response.Write(".col-date {width: 70px;}");
                Response.Write(".col-teacher {width: 90px;}"); // 上课教师列宽度
            Response.Write("td {padding: 3px; text-align: center; vertical-align: middle;}");
            // 确保表头在每页都打印
            Response.Write("thead {display: table-header-group;}");
            // 避免表格行被分页符分割
            Response.Write("tr {page-break-inside: avoid;}");
            // 防止表格过宽
            Response.Write("body {margin: 0; padding: 0;}");
            Response.Write("</style>");
            
            // 添加标题行（居中显示）
            Response.Write("<table border='0' cellspacing='0' cellpadding='0' style='margin-bottom: 20px;'><tr>");
            Response.Write("<td colspan='" + dt.Columns.Count + "' style='text-align:center; font-size:16pt; font-weight:bold; padding:10px;'>");
            Response.Write("信息科技学生签到记录");
            Response.Write("</td></tr></table>");
            
            Response.Write("<table border='1' cellspacing='0' cellpadding='0'>");
            // 使用thead确保表头在每页都打印
            Response.Write("<thead>");
            // 输出表头，为各列添加对应的CSS类
            Response.Write("<tr>");
            foreach (System.Data.DataColumn column in dt.Columns)
            {
                string cssClass = "";
                switch (column.ColumnName)
                {
                    case "序号": cssClass = "col-serial";
                        break;
                    case "学号": cssClass = "col-id";
                        break;
                    case "年级": cssClass = "col-grade";
                        break;
                    case "班级": cssClass = "col-class";
                        break;
                    case "姓名": cssClass = "col-name";
                        break;
                    case "作品": cssClass = "col-work";
                        break;
                    case "表现": cssClass = "col-attitude";
                        break;
                    case "备注": cssClass = "col-note";
                        break;
                    case "组评": cssClass = "col-group";
                        break;
                    case "分值": cssClass = "col-score";
                        break;
                    case "机号": cssClass = "col-machine";
                        break;
                    case "节次": cssClass = "col-session";
                        break;
                    case "上课内容": cssClass = "col-title";
                        break;
                    case "日期": cssClass = "col-date";
                        break;
                    case "上课教师": cssClass = "col-teacher";
                        break;
                }
                Response.Write("<td class='" + cssClass + "' style='font-weight:bold;background-color:#cccccc;text-align:center'>" + column.ColumnName + "</td>");
            }
            Response.Write("</tr>");
            Response.Write("</thead>");
            Response.Write("<tbody>");

            // 输出数据行，为各列添加对应的CSS类
            foreach (System.Data.DataRow row in dt.Rows)
            {
                Response.Write("<tr>");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    string cssClass = "";
                    switch (dt.Columns[i].ColumnName)
                    {
                        case "序号": cssClass = "col-serial";
                            break;
                        case "学号": cssClass = "col-id";
                            break;
                        case "年级": cssClass = "col-grade";
                            break;
                        case "班级": cssClass = "col-class";
                            break;
                        case "姓名": cssClass = "col-name";
                            break;
                        case "作品": cssClass = "col-work";
                            break;
                        case "表现": cssClass = "col-attitude";
                            break;
                        case "备注": cssClass = "col-note";
                            break;
                        case "组评": cssClass = "col-group";
                            break;
                        case "分值": cssClass = "col-score";
                            break;
                        case "机号": cssClass = "col-machine";
                            break;
                        case "节次": cssClass = "col-session";
                            break;
                        case "上课内容": cssClass = "col-title";
                            break;
                        case "日期": cssClass = "col-date";
                            break;
                        case "上课教师": cssClass = "col-teacher";
                            break;
                        }
                        // 如果是年级列，将数字转换为汉字
                        string cellValue = row[i].ToString();
                        if (dt.Columns[i].ColumnName == "年级")
                        {
                            cellValue = ConvertGradeToChinese(cellValue);
                        }
                        Response.Write("<td class='" + cssClass + "'>" + cellValue + "</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</tbody>");
            Response.Write("</table>");
            Response.End();
        }
        else
        {
            Response.Write("<script>alert('没有找到可导出的记录！');</script>");
        }
    }
    
    /// <summary>
    /// GetCurrentTeacherName 方法实现 - 获取当前登录教师姓名
    /// </summary>
    /// <returns></returns>
    public string GetCurrentTeacherName()
    {
        try
        {
            LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
            return HttpContext.Current.Server.UrlDecode(tcook.Hname);
        }
        catch
        {
            return "未知教师";
        }
    }

    /// <summary>
    /// 将英文星期转换为中文星期
    /// </summary>
    /// <param name="englishWeek">英文星期（如：Monday）</param>
    /// <returns>中文星期（如：星期一）</returns>
    protected string ConvertWeekToChinese(string englishWeek)
    {
        if (string.IsNullOrEmpty(englishWeek))
            return string.Empty;
            
        switch (englishWeek.ToLower())
        {
            case "monday":
                return "星期一";
            case "tuesday":
                return "星期二";
            case "wednesday":
                return "星期三";
            case "thursday":
                return "星期四";
            case "friday":
                return "星期五";
            case "saturday":
                return "星期六";
            case "sunday":
                return "星期日";
            default:
                return englishWeek; // 如果不是标准的英文星期，直接返回原文
        }
    }
    
    /// <summary>
    /// 将数字年级转换为汉字表示
    /// </summary>
    /// <param name="gradeNum">数字年级（如：1, 2）</param>
    /// <returns>汉字年级（如：一, 二）</returns>
    protected string ConvertGradeToChinese(string gradeNum)
    {
        if (string.IsNullOrEmpty(gradeNum))
            return string.Empty;
        
        int num;
        if (int.TryParse(gradeNum, out num))
        {
            switch (num)
            {
                case 1:
                    return "一";
                case 2:
                    return "二";
                case 3:
                    return "三";
                case 4:
                    return "四";
                case 5:
                    return "五";
                case 6:
                    return "六";
                default:
                    return gradeNum; // 如果是其他数字，直接返回原文
            }
        }
        return gradeNum; // 如果不能解析为数字，直接返回原文
    }
}
