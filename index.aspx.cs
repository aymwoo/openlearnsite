using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class index : System.Web.UI.Page
{
    protected string SiteTitle { get; private set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        SiteTitle = LearnSite.Common.XmlHelp.SiteTitle();
        if (string.IsNullOrEmpty(SiteTitle))
        {
            SiteTitle = "信息科技学习网站";
        }
        LitSiteTitle.Text = SiteTitle;

        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
            OpenJump(cook.Sgrade, cook.Sclass);//跳转选择
        }
        else
        {
            if (!IsPostBack)
            {
                if (!verChecking())
                    return;
                ShowFoot();
                Btnlogin.Attributes["onClick"] = "return doubleCheck()";
                this.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle();
                Request.Cookies.Clear();
            }
        }
    }
    private void ShowFoot()
    {
        DateTime dt3 = DateTime.Now;
        string pp = LearnSite.Common.Computer.MyIp();// Page.Request.UserHostAddress;
        DateTime dt4 = DateTime.Now;
        if (Request.QueryString["mysnum"] != null)
        {
            string myname = "";
            string mysnum=Request.QueryString["mysnum"].ToString();
            TextBoxuser.Text = mysnum;
            TextBoxpwd.Focus();
            if (Request.QueryString["myname"] != null)
            {
                myname = Request.QueryString["myname"].ToString();
            }
            LearnSite.BLL.Signin gbll = new LearnSite.BLL.Signin();
            if (!gbll.IsSameIp(mysnum, pp))
            {
                // IsSameIp从Students表查座位号，再从Computers表查IP进行对比
                // IP不匹配时，显示学生的固定座位信息
                LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
                LearnSite.Model.Students student = sbll.SnumGetModel(mysnum);
                string seatInfo = (student != null && !string.IsNullOrEmpty(student.Sseat)) ? student.Sseat : "未设置";
                Labelmsg.Text = myname + "，这不是你的座位，你的固定座位是: " + seatInfo;
                Labelmsg.ForeColor = System.Drawing.Color.Red;
            }
            else {
                Labelmsg.Text = "欢迎 "+ myname + " 同学";
                Labelmsg.ForeColor = System.Drawing.Color.Black;
            }
        }
        Labelterm.Text = LearnSite.Common.XmlHelp.GetTerm();
        int loginm = LearnSite.Common.XmlHelp.LoginMode();//获取登录方式 0表示个人密码方式登录 1表示班级密码方式登录


        DateTime dt5 = DateTime.Now;
        Labelhostname.Text = GetHostNameMy(pp);
        DateTime dt6 = DateTime.Now;
        Labelip.Text = pp;
        Labelloadtime.Text = "IP：" + LearnSite.Common.Computer.DatagoneMilliseconds(dt3, dt4) + "毫秒&nbsp;&nbsp;" + "&nbsp;&nbsp;主机名：" + LearnSite.Common.Computer.DatagoneMilliseconds(dt5, dt6) + "毫秒&nbsp;";

        if (loginm == 1)
        {
            Labelversion.Text = "『班级模式』";
        }
        else
        {
            Labelversion.Text = "『个人模式』";
        }

        /*
        LearnSite.BLL.Computers cbll = new LearnSite.BLL.Computers();
        string autosnum = cbll.getIpSnum(pp);//获取自动分配的学号，填充到登录页面，方便培训
        if (autosnum != "")
        {
            TextBoxuser.Text = autosnum;
            TextBoxuser.ReadOnly = true;//不可修改
        }
        else
        {
            TextBoxuser.ReadOnly = false;//可修改        
        }
         */

    }
    /// <summary>
    /// 获取IP对应机器名，返回主机名
    /// 支持多网段自动识别机房
    /// </summary>
    /// <param name="Pip"></param>
    /// <returns></returns>
    private string GetHostNameMy(string aPip)
    {
        string msg = "否";//表示不自动获取主机名
        if (!string.IsNullOrEmpty(aPip))
        {
            LearnSite.BLL.Computers cbll = new LearnSite.BLL.Computers();
            LearnSite.Model.Computers cmodel = new LearnSite.Model.Computers();
            cmodel = cbll.GetModelByIp(aPip);

            bool autohostname = LearnSite.Common.XmlHelp.GetAutoHostName();//自动取主机名开关
            if (cmodel != null)//如果存在
            {
                msg = cmodel.Pmachine;//获取主机名
                if (!cmodel.Plock && autohostname)//如果未锁定，更新主机名并锁定
                {
                    string newMachine = LearnSite.Common.Computer.GetGuestHost(aPip);
                    cbll.UpdateByPid(cmodel.Pid, newMachine);
                    msg = newMachine;//返回新主机名
                }
            }
            else
            {
                string inum = TryGetInumByNetSegment(aPip);
                if (!string.IsNullOrEmpty(inum))
                {
                    msg = inum;
                }
                else if (autohostname)
                {
                    LearnSite.Model.Computers newmodel = new LearnSite.Model.Computers();
                    newmodel.Pip = aPip;
                    newmodel.Plock = true;
                    string addMachine = LearnSite.Common.Computer.GetGuestHost(aPip);
                    newmodel.Pmachine = addMachine;
                    newmodel.Pdate = DateTime.Now;
                    cbll.Add(newmodel);
                    msg = addMachine;
                }
            }
        }
        else
        {
            msg = "空";//表示获取不到IP，反回主机名为空
        }
        return msg;
    }

    /// <summary>
    /// 通过网段识别机房，获取机号
    /// 用于多机房不同网段的情况
    /// </summary>
    /// <param name="ip">客户端IP地址</param>
    /// <returns>机号，如果未找到返回空字符串</returns>
    private string TryGetInumByNetSegment(string ip)
    {
        try
        {
            int? hid = LearnSite.Common.Computer.GetHidByIp(ip);
            if (hid.HasValue)
            {
                LearnSite.BLL.Ip ipBll = new LearnSite.BLL.Ip();
                string inum = ipBll.GetInumByIpAndHid(ip, hid.Value);
                return inum;
            }
        }
        catch
        {
        }
        return string.Empty;
    }

    protected void Btnlogin_Click(object sender, EventArgs e)
    {
        bool issamenet = LearnSite.Common.XmlHelp.GetLogin();
        bool checkresult = LearnSite.Common.Computer.IsSameNet();
        if (issamenet)
        {
            if (checkresult)
                LoginCode();//如果受限制，且在同网段内，则允许访问
            else
            {
                Labelmsg.Text = "『许可访问→内网√』";
                TextBoxuser.Text = "";
                TextBoxpwd.Text = "";
            }
        }
        else
        {
            LoginCode();
        }       
    }

    private void LoginCode()
    {
        string Snum = TextBoxuser.Text.Trim();
        string Spwd = TextBoxpwd.Text.Trim();
        string lbip = Labelip.Text;
        string msg = "";
        if (Snum != "" && Spwd != "")
        {
            if (LearnSite.Common.WordProcess.IsNum(Snum) && LearnSite.Common.WordProcess.IsEnNum(Spwd))
            {
                LearnSite.Model.Students model = new LearnSite.Model.Students();
                LearnSite.BLL.Students bll = new LearnSite.BLL.Students();
                int loginm = LearnSite.Common.XmlHelp.LoginMode();//获取登录方式 0表示个人密码方式登录 1表示班级密码方式登录
                if (loginm == 1)
                {
                    if (bll.ExistsLogin(Snum, Spwd))
                    {
                        model = bll.SnumGetModel(Snum);//查询该学号和班级密码的学生是否存在，存在返回实体，不存在返回null
                    }
                    else
                    {
                        //判断该学号是否允许个人模式登录
                        if (bll.isRlogin(Snum))
                        {
                            model = bll.GetStudentModel(Snum, Spwd);//查询该学号密码学生是否存在，存在返回实体，不存在返回null
                        }
                        else
                        {
                            model = null;
                        }
                    }
                }
                else
                {
                    model = bll.GetStudentModel(Snum, Spwd);//查询该学号密码学生是否存在，存在返回实体，不存在返回null
                }

                if (model != null)
                {
                    int Qgrade = model.Sgrade.Value;
                    int Qclass = model.Sclass.Value;
                    int Qsid = model.Sid;
                    string Qname = model.Sname;
                    int Qsyear = model.Syear.Value;
                    int Qterm = Int32.Parse(LearnSite.Common.XmlHelp.GetTerm());
                    //以下代码为学生登录时把当前学案标题写入数据表Signin 湖南湘西罗启斌
                    // 获取当前节次（使用统一的节次判断方法）
                    string Qsession = LearnSite.Common.TimeSlotHelper.GetCurrentTimeSlot();

                    // 以下为新加学生登录把课程名写入数据表
                    int Sgrade = Qgrade;
                    int Sclass = Qclass;
                    LearnSite.BLL.Room rm = new LearnSite.BLL.Room();

                    // 获取课程标题（优先从课程表获取，找不到则从Room表获取）
                    string Qtitle = string.Empty;
                    string teacherName = string.Empty;

                    // 方法1：从课程表获取（用于常规课程）
                    Qtitle = GetCourseTitleFromSchedule(Sgrade, Sclass, out teacherName);

                    // 方法2：从Room表获取（用于兴趣小组或其他临时课程）
                    if (string.IsNullOrEmpty(Qtitle))
                    {
                        Qtitle = GetCourseTitleFromRoom(Sgrade, Sclass);
                    }
                    //新增代码结束
                    if (LearnSite.Common.XmlHelp.GetSingleLogin())//如果是单点登录
                    {
                        // 使用带IP检测的方法：如果检测到已登录，先清除旧状态，允许重新登录
                        // 这样学生关闭浏览器或重启后，可以再次输入密码登录
                        string existingIp = LearnSite.Common.App.GetUserIp(Snum);
                        if (!string.IsNullOrEmpty(existingIp))
                        {
                            // 检测到用户已登录，先清除旧状态
                            LearnSite.Common.App.AppUserRemove(Snum);
                            LearnSite.Common.App.AppKickUserRemove(Snum);
                        }

                    if (LearnSite.Common.CookieHelp.SetStudentCookies(model, lbip))//写cookies
                    {
                        DateTime LoginTime = DateTime.Now;
                        LearnSite.BLL.Signin gbll = new LearnSite.BLL.Signin();
                        gbll.SigninToday(Snum, LoginTime, lbip, Qgrade, Qterm, Qsid, Qname, Qclass, Qsyear, Qtitle, Qsession);//签到 新增, Qtitle, Qsession 记录课程标题和节次
                        // 更新学生的固定座位信息（仅在班级IP锁定且学生表中Sseat为空时）
                        if (rm.IsLoginLock(Qgrade, Qclass) && string.IsNullOrEmpty(model.Sseat))
                        {
                            LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
                            string seatInfo = GetHostNameMy(lbip); // 获取座位信息（主机名）
                            sbll.UpdateFixedSeat(Snum, seatInfo);
                        }

                        Btnlogin.Enabled = false;
                        LearnSite.Common.App.AppKickUserRemove(Snum);//将踢除列表中的学号去掉（2011-9-20修）
                        LearnSite.Common.App.AppUserAddWithIp(Snum, lbip);//给网站全局变量列表中增加该用户及其IP
                        System.Threading.Thread.Sleep(200);
                        OpenJump(Qgrade, Qclass);//跳转选择
                    }
                    else
                    {
                        msg = "请不要换机，否则无法登录！";
                    }
                    }
                    {
                        if (LearnSite.Common.CookieHelp.SetStudentCookies(model, lbip))//写cookies
                        {
                            DateTime LoginTime = DateTime.Now;
                            LearnSite.BLL.Signin gbll = new LearnSite.BLL.Signin();
                            gbll.SigninToday(Snum, LoginTime, lbip, Qgrade, Qterm, Qsid, Qname, Qclass, Qsyear, Qtitle, Qsession);//签到 //新增：, Qtitle, Qsession 把课程标题和节次信息写入数据表

                            // 更新学生的固定座位信息（仅在班级IP锁定且学生表中Sseat为空时）
                            if (rm.IsLoginLock(Qgrade, Qclass) && string.IsNullOrEmpty(model.Sseat))
                            {
                                LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
                                string seatInfo = GetHostNameMy(lbip); // 获取座位信息（主机名）
                                sbll.UpdateFixedSeat(Snum, seatInfo);
                            }

                            Btnlogin.Enabled = false;
                            System.Threading.Thread.Sleep(200);
                            OpenJump(Qgrade, Qclass);//跳转选择
                        }
                        else
                        {
                            msg = "请不要换机，否则无法登录！";
                        }
                    }
                }
                else
                {
                    string msgstr = "『当前为班级密码模式』";
                    if (loginm == 0)
                        msgstr = "『当前为个人密码模式』";
                    msg = "用户名或密码错误！" + msgstr;
                    TextBoxuser.Text = "";
                    TextBoxpwd.Text = "";
                }

            }
            else
            {
                msg = "用户名或密码含有非法字符，学号必须为数字";
            }
        }
        Labelmsg.Text = msg;
    }

    private void OpenJump(int Sgrade, int Sclass)
    {
        // 检查是否启用课前检查
        bool enablePreClassCheck = false;
            string preClassCheckSetting = LearnSite.Common.XmlHelp.GetTypeName("EnablePreClassCheck");
            if (!string.IsNullOrEmpty(preClassCheckSetting))
            {
                bool.TryParse(preClassCheckSetting, out enablePreClassCheck);
            }

            if (enablePreClassCheck)
        {
            // 课前检查模式：无论是否有课程，都跳转到sub.aspx进行课前检查
            Response.Redirect("sub.aspx", false);
        }
        else
        {
            // 正常模式：无论是否有课程，都不跳转到课前检查页面
            LearnSite.BLL.Room rm = new LearnSite.BLL.Room();

            // 检查是否开启快速模式
            bool isOpenMode = rm.IsRopen(Sgrade, Sclass);
            string Cid = rm.IsRopenRcid(Sgrade, Sclass);

            if (isOpenMode)
            {
                // 快速模式开启时，直接跳转到课程页面，不管是否有课程
                if (!string.IsNullOrEmpty(Cid))
                {
                    // 有课程时直接跳转到课程页面
                    string myurl = "student/showcourse.aspx?Cid=" + Cid;
                    Response.Redirect(myurl, true);
                }
                else
                {
                    // 没有课程时跳转到我的课程页面
                    Response.Redirect("student/mycourse.aspx", true);
                }
            }
            else
            {
                // 非快速模式时，按照原有逻辑处理
                if (string.IsNullOrEmpty(Cid))
                {
                    // 没有课程时跳转到学生信息页面
                    Response.Redirect("student/myinfo.aspx", false);
                }
                else
                {
                    // 有课程时直接跳转到课程页面
                    string myurl = "student/showcourse.aspx?Cid=" + Cid;
                    Response.Redirect(myurl, true);
                }
            }
        }
    }
    private bool verChecking()
    {
        if (!LearnSite.DBUtility.SqlHelper.DatabaseExist())
        {
            Response.Redirect("~/upgrade.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return false;
        }

        if (!LearnSite.DBUtility.UpdateGrade.TableCheck())
        {
            string ch = "您的数据库未创建、连接失败或版本需要更新，现在将跳到更新程序UpGrade.aspx，请执行检查或修改配置，不影响原有数据！";
            LearnSite.Common.WordProcess.Alert(ch, this.Page);
            Response.Redirect("~/upgrade.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
            return false;
        }

        return true;
    }

    /// <summary>
    /// 获取星期几的数字表示(1-7)
    /// </summary>
    private int GetDayOfWeekNumber(DayOfWeek dayOfWeek)
    {
        switch (dayOfWeek)
        {
            case DayOfWeek.Monday: return 1;
            case DayOfWeek.Tuesday: return 2;
            case DayOfWeek.Wednesday: return 3;
            case DayOfWeek.Thursday: return 4;
            case DayOfWeek.Friday: return 5;
            case DayOfWeek.Saturday: return 6;
            case DayOfWeek.Sunday: return 7;
            default: return 0;
        }
    }

    /// <summary>
    /// 从课程表获取当前课程的标题和教师信息
    /// 优先从CourseSchedule表获取，找不到则返回空
    /// </summary>
    /// <param name="grade">年级</param>
    /// <param name="classNum">班级号</param>
    /// <returns>课程标题，找不到则返回空字符串</returns>
    private string GetCourseTitleFromSchedule(int grade, int classNum, out string teacherName)
    {
        teacherName = string.Empty;
        try
        {
            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;
            int currentDay = DateTime.Now.Day;

            // 计算当前学年
            int schoolYear;
            if (currentMonth >= 9)
            {
                schoolYear = currentYear;
            }
            else if (currentMonth == 1 || (currentMonth == 2 && currentDay <= 10))
            {
                schoolYear = currentYear - 1;
            }
            else if (currentMonth == 2 && currentDay > 10)
            {
                schoolYear = currentYear;
            }
            else if (currentMonth >= 3 && currentMonth <= 8)
            {
                schoolYear = currentYear;
            }
            else
            {
                schoolYear = currentYear;
            }

            // 计算学期
            int term = (currentMonth >= 2 && currentMonth <= 7) ? 2 : 1;

            // 获取当前星期几
            int weekDay = GetDayOfWeekNumber(DateTime.Now.DayOfWeek);

            // 获取当前节次（使用统一的节次判断方法）
            string timeSlot = LearnSite.Common.TimeSlotHelper.GetCurrentTimeSlot();
            int slotNumber = 0;
            if (!string.IsNullOrEmpty(timeSlot) && timeSlot != "其他")
            {
                // 尝试从节次名称中提取数字（如"第1节" → 1）
                System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(timeSlot, @"\d+");
                if (match.Success)
                {
                    int.TryParse(match.Value, out slotNumber);
                }
            }

            if (slotNumber == 0)
            {
                return string.Empty; // 无法获取节次，直接返回
            }

            // 构建班级名称（格式：年级-班级，如"1-1"）
            string className = grade + "-" + classNum;

            // 从CourseSchedule表查询课程信息
            string sql = @"
                SELECT DISTINCT
                    cs.Subject AS Subject
                FROM CourseSchedule cs
                WHERE cs.SchoolYear = @SchoolYear
                  AND cs.Term = @Term
                  AND cs.WeekDay = @WeekDay
                  AND cs.TimeSlot = @TimeSlot
                  AND cs.ClassName = @ClassName";

            System.Collections.Generic.List<System.Data.SqlClient.SqlParameter> parameters =
                new System.Collections.Generic.List<System.Data.SqlClient.SqlParameter>();
            parameters.Add(new System.Data.SqlClient.SqlParameter("@SchoolYear", schoolYear));
            parameters.Add(new System.Data.SqlClient.SqlParameter("@Term", term));
            parameters.Add(new System.Data.SqlClient.SqlParameter("@WeekDay", weekDay));
            parameters.Add(new System.Data.SqlClient.SqlParameter("@TimeSlot", slotNumber));
            parameters.Add(new System.Data.SqlClient.SqlParameter("@ClassName", className));

            object result = LearnSite.DBUtility.DbHelperSQL.GetSingle(sql, parameters.ToArray());

            if (result != null && result != DBNull.Value)
            {
                teacherName = "课程表";
                return result.ToString();
            }

            return string.Empty;
        }
        catch (Exception ex)
        {
            // 出错时返回空，使用备用方法
            return string.Empty;
        }
    }

    /// <summary>
    /// 从Room表获取当前课程的标题（备用方法）
    /// 用于兴趣小组或其他临时课程
    /// </summary>
    /// <param name="grade">年级</param>
    /// <param name="classNum">班级号</param>
    /// <returns>课程标题，找不到则返回空字符串</returns>
    private string GetCourseTitleFromRoom(int grade, int classNum)
    {
        try
        {
            LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
            string Cid = rm.IsRopenRcid(grade, classNum);

            // 获取课程标题
            if (!string.IsNullOrEmpty(Cid))
            {
                int courseId;
                if (int.TryParse(Cid, out courseId))
                {
                    LearnSite.BLL.Courses cs = new LearnSite.BLL.Courses();
                    LearnSite.Model.Courses courseModel = cs.GetModel(courseId);
                    if (courseModel != null)
                    {
                        return courseModel.Ctitle;
                    }
                }
            }
            return string.Empty;
        }
        catch (Exception ex)
        {
            // 出错时返回空
            return string.Empty;
        }
    }
    protected void BtnStartClass_Click(object sender, EventArgs e)
    {
        // 移除当前时间段的会话缓存
        Session.Remove("CurrentTimeSlot");

        DateTime now = DateTime.Now;
        TimeSpan currentTime = now.TimeOfDay;
        int dayOfWeek = GetDayOfWeekNumber(now.DayOfWeek);

        // 只在工作日查询课程表
        if (dayOfWeek < 1 || dayOfWeek > 5)
        {
            Session["CurrentTimeSlot"] = "其他";
            return;
        }

        // 从kechengbiao表获取当前时间段的节次信息
        string sql = @"
            SELECT TOP 1 CAST(SlotNumber AS VARCHAR(10)) as SlotName
            FROM kechengbiao 
            WHERE Year = @Year 
              AND Term = @Term
              AND SlotNumber IS NOT NULL
              AND StartTime IS NOT NULL
              AND EndTime IS NOT NULL
              AND StartTime <= @CurrentTime 
              AND EndTime >= @CurrentTime
            ORDER BY SlotNumber";

        System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[]
        {
            new System.Data.SqlClient.SqlParameter("@Year", System.Data.SqlDbType.Int),
            new System.Data.SqlClient.SqlParameter("@Term", System.Data.SqlDbType.Int),
            new System.Data.SqlClient.SqlParameter("@CurrentTime", System.Data.SqlDbType.Time)
        };
        parameters[0].Value = now.Year;
        parameters[1].Value = LearnSite.Common.XmlHelp.GetIntTerm();
        parameters[2].Value = currentTime;

        object result = LearnSite.DBUtility.DbHelperSQL.GetSingle(sql, parameters);
        string slot = result != null ? result.ToString() : "其他";

        // 将时间段存入会话
        Session["CurrentTimeSlot"] = slot;
    }
}
