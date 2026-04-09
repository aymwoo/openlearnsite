using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using LearnSite.DBUtility;
public partial class Teacher_start : System.Web.UI.Page
{
    LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
    // 供学习状态AJAX轮询JS使用
    protected string LsGrade = "0";
    protected string LsClass = "0";
    protected string LsCid = "0";
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
        {
            string Hid = tcook.Hid.ToString();

            if (!IsPostBack)
            {
                // 每次首次进入页面时清除上课状态的Session，实现点击导航链接后退出上课状态
                Session.Remove(Hid + "grade");
                Session.Remove(Hid + "class");
                Session.Remove(Hid + "StartTime");

                Master.Page.Title = LearnSite.Common.CookieHelp.SetMainPageTitle() + "上课开始页面";
                Btnstudent.Attributes.Add("onclick", "this.form.target='_blank'");
                Btnrefresh.Attributes.Add("onclick", "this.form.target='_self'");
                BtnaAllQuit.Attributes["OnClick"] = "return confirm('您确定要将当前上课班级学生全体下线吗？');";
                GradeClass();

                // 初始化打字宝典开关状态
                LoadTypingEnabledStatus();
                // 初始化学习汇总开关状态
                LoadSummaryEnabledStatus();
                // 初始化荣誉榜开关状态
                LoadHonorsEnabledStatus();

                // 自动执行智能选择
                bool smartSelectionSuccess = TrySmartSelection();
                
                showCid();//放在签到显示前，以便绑定
                Showkc();
                showhouse();
                ShowSigin();
                ShowNoSigin();
                HowManyWork();
                showLock();
                showwtUrl();
                showMenu();
                
                // 如果智能选择失败，确保界面正常显示
                if (!smartSelectionSuccess)
                {
                    // 智能选择失败时的处理已经在各个show方法中完成
                }
            }
            else
            {
                // PostBack时检查上课状态并锁定控件
                if (Session[Hid + "grade"] != null && Session[Hid + "class"] != null)
                {
                    // 已开始上课，锁定控件
                    Btnset.Enabled = false;
                    DDLgrade.Enabled = false;
                    DDLclass.Enabled = false;
                    DDLCid.Enabled = false;
                    Btnstudent.Enabled = true;
                    Btnrefresh.Enabled = true;

                    // 添加JavaScript确保客户端控件也被禁用
                    string disableScriptForPageLoad = @"setTimeout(function() {
                        var ddlgrade = document.getElementById('" + DDLgrade.ClientID + @"');
                        var ddlclass = document.getElementById('" + DDLclass.ClientID + @"');
                        var ddlcid = document.getElementById('" + DDLCid.ClientID + @"');
                        var ddlhouse = document.getElementById('" + DDLhouse.ClientID + @"');
                        if(ddlgrade) ddlgrade.disabled = true;
                        if(ddlclass) ddlclass.disabled = true;
                        if(ddlcid) ddlcid.disabled = true;
                        if(ddlhouse) ddlhouse.disabled = true;
                    }, 100);";
                    ClientScript.RegisterStartupScript(this.GetType(), "DisableControls", disableScriptForPageLoad, true);
                }
                else
                {
                    // 未开始上课，保持控件可用
                    Btnset.Enabled = true;
                    Btnstudent.Enabled = false;
                    Btnrefresh.Enabled = false;
                }
            }
        }
    }

    private string GetChineseDayOfWeek(DayOfWeek dayOfWeek)
    {
        switch (dayOfWeek)
        {
            case DayOfWeek.Sunday:
                return "星期日";
            case DayOfWeek.Monday:
                return "星期一";
            case DayOfWeek.Tuesday:
                return "星期二";
            case DayOfWeek.Wednesday:
                return "星期三";
            case DayOfWeek.Thursday:
                return "星期四";
            case DayOfWeek.Friday:
                return "星期五";
            case DayOfWeek.Saturday:
                return "星期六";
            default:
                return "未知";
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (DDLgrade != null && DDLgrade.Items.Count > 0)
        {
            LsGrade = DDLgrade.SelectedValue;
        }
        if (DDLclass != null && DDLclass.Items.Count > 0)
        {
            LsClass = DDLclass.SelectedValue;
        }
        if (DDLCid != null && DDLCid.Items.Count > 0)
        {
            LsCid = DDLCid.SelectedValue;
        }
    }

    private void showhouse()
    {
        string pcroom = tcook.Hroom;
        if (!LearnSite.Common.XmlHelp.GetHouseMode())
        {
            LearnSite.BLL.Computers cbll = new LearnSite.BLL.Computers();
            DDLhouse.DataSource = cbll.CmpRoom();
            DDLhouse.DataTextField = "Pm";
            DDLhouse.DataValueField = "Pm";
            DDLhouse.DataBind();
            HyperLinkSeat.Visible = false;
        }
        else
        {
            HyperLinkSeat.Visible = true;
            LearnSite.BLL.House hbll = new LearnSite.BLL.House();
            DDLhouse.DataSource = hbll.GetListHouse();
            DDLhouse.DataTextField = "Hname";
            DDLhouse.DataValueField = "Hid";
            DDLhouse.DataBind();
        }
        if (DDLhouse.Items.FindByValue(pcroom) != null)
        {
            DDLhouse.SelectedValue = pcroom;
        }
    }
    private void showLock()
    {
        int Rgrade = Int32.Parse(DDLgrade.SelectedValue);
        int Rclass = Int32.Parse(DDLclass.SelectedValue);
        LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
        LearnSite.Model.Room rmodel = rm.GetModel(Rgrade, Rclass);
        if (rmodel == null) return;
        CheckBoxip.Checked = rmodel.Rlock;
        CheckBoxOpen.Checked = rmodel.Ropen;
        CheckBoxRgauge.Checked = rmodel.Rgauge;
        CheckBoxPwd.Checked = rmodel.Rpwdsee;
        CheckBoxShare.Checked = rmodel.Rshare;
        CheckBoxGroupShare.Checked = rmodel.Rgroupshare;
        CheckBoxScratch.Checked = rmodel.Rscratch;
        CheckBoxLogin.Checked = rmodel.Rlogin;
        CheckBoxPass.Checked = rmodel.Rpass;

        // 初始化课前检查开关状态
        string preClassCheckSetting = LearnSite.Common.XmlHelp.GetTypeName("EnablePreClassCheck");
        if (!string.IsNullOrEmpty(preClassCheckSetting))
        {
            bool preClassCheck;
            if (bool.TryParse(preClassCheckSetting, out preClassCheck))
            {
                CheckBoxPreClassCheck.Checked = preClassCheck;
            }
            else
            {
                // 默认不启用课前检查
                CheckBoxPreClassCheck.Checked = false;
            }
        }
        else
        {
            // 默认不启用课前检查
            CheckBoxPreClassCheck.Checked = false;
        }

        // 初始化游戏开关状态
        string gamesSetting = LearnSite.Common.XmlHelp.GetTypeName("EnableGames");
        if (!string.IsNullOrEmpty(gamesSetting))
        {
            bool gamesEnabled;
            if (bool.TryParse(gamesSetting, out gamesEnabled))
            {
                CheckBoxGames.Checked = gamesEnabled;
            }
        }

        // 初始化小组讨论开关状态
        string groupChatSetting = LearnSite.Common.XmlHelp.GetTypeName("EnableGroupChat");
        if (!string.IsNullOrEmpty(groupChatSetting))
        {
            bool groupChatEnabled;
            if (bool.TryParse(groupChatSetting, out groupChatEnabled))
            {
                CheckBoxGroupChat.Checked = groupChatEnabled;
            }
        }

        // 初始化登记开关状态
        string registerSetting = LearnSite.Common.XmlHelp.GetTypeName("EnableRegister");
        if (!string.IsNullOrEmpty(registerSetting))
        {
            bool registerEnabled;
            if (bool.TryParse(registerSetting, out registerEnabled))
            {
                CheckBoxRegister.Checked = registerEnabled;
            }
            else
            {
                // 默认不启用登记
                CheckBoxRegister.Checked = false;
            }
        }
        else
        {
            // 默认不启用登记
            CheckBoxRegister.Checked = false;
        }

        string reat = rmodel.Rseat.Value.ToString();
        TBpwd.Text = rmodel.Rpwd;
        TBpwd.ToolTip = "班级模式：学生使用生成的班级密码登录";
        if (LearnSite.Common.XmlHelp.LoginMode() == 0) //如果是0，个人模式，1为班级模式
        {
            TBpwd.Text = "个人模式";
            CheckBoxLogin.Visible = false;
            TBpwd.ToolTip = "个人模式：学生使用个人密码登录";
        }

        BtnaAllQuit.Visible = rmodel.Rset;
        LessonQuitFooter.Visible = rmodel.Rset;

    }

    private void showCid()
    {
        int Chid = tcook.Hid;
        int Cobj = Int32.Parse(DDLgrade.SelectedValue);
        int Cterm = Int32.Parse(LearnSite.Common.XmlHelp.GetTerm());
        string courseId = Request.QueryString["courseid"];
        LearnSite.BLL.Courses cbll = new LearnSite.BLL.Courses();
        //加载数据前，先把要开始上课课程改为发布状态哈
        if (!string.IsNullOrEmpty(courseId))
        {
            cbll.UpdateCpublish(Convert.ToInt32(courseId), true);
        }
        DDLCid.DataSource = cbll.ShowCidCtitle(Chid, Cobj, Cterm);
        DDLCid.DataTextField = "Ctitle";
        DDLCid.DataValueField = "Cid";
        
        DDLCid.DataBind();
        //绑定后，选中对应的课程id
        if (!string.IsNullOrEmpty(courseId))
        {
         
            DDLCid.SelectedValue = courseId;
        }
    }

    /// <summary>
    /// 显示本班级已学和未学学案
    /// </summary>
    private void Showkc()
    {
        int Rhid = tcook.Hid;
        int Rgrade = Int32.Parse(DDLgrade.SelectedValue);
        int Rclass = Int32.Parse(DDLclass.SelectedValue);
        LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
        int Syear = sbll.GetYear(Rgrade, Rclass);
        LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
        int Wterm = Int32.Parse(LearnSite.Common.XmlHelp.GetTerm());
        string Wcids = wbll.ShowDoneWorkCids(Rgrade, Rclass, Wterm, Syear);//作品
        LearnSite.BLL.TopicReply Tbll = new LearnSite.BLL.TopicReply();
        string Tcids = Tbll.ShowDoneReplyCids(Rgrade, Rclass, Wterm, Syear);//讨论回复
        LearnSite.BLL.SurveyFeedback fbll = new LearnSite.BLL.SurveyFeedback();
        string Fcids = fbll.ShowFeedbackCids(Rgrade, Rclass, Wterm, Syear);//调查测验
        LearnSite.BLL.Solves vbll = new LearnSite.BLL.Solves();
        string vcids = vbll.ShowDoneSovleCids(Rgrade.ToString(), Rclass.ToString(), Wterm.ToString(), Syear.ToString());//python测评

        string rrr = Syear.ToString() + Rgrade.ToString() + Rclass.ToString();
        int simiSid = 0 - Int32.Parse(rrr);//将入学年度和班级号作为模拟学生的ID
        LearnSite.BLL.MenuWorks kbll = new LearnSite.BLL.MenuWorks();
        string kcids = kbll.readCids(simiSid);//阅读任务

        LearnSite.BLL.TxtFormBack txbll = new LearnSite.BLL.TxtFormBack();
        string txcids = txbll.ShowDoneBackCids(Rgrade, Rclass, Wterm, Syear);//填表

        string allCids = Wcids + Tcids + Fcids + vcids + kcids + txcids;
        allCids = LearnSite.Common.WordProcess.SimpleWords(allCids);
        LearnSite.BLL.Courses cs = new LearnSite.BLL.Courses();
        DLdonekc.DataSource = cs.ShowClassDoneCourse(Rgrade, Rhid, allCids);
        DLdonekc.DataBind();
        DLnewkc.DataSource = cs.ShowClassnewCourse(Rgrade, Rhid, allCids);
        DLnewkc.DataBind();
    }

    protected void Btnset_Click(object sender, EventArgs e)
    {
        if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
        {
            string Hid =  tcook.Hid.ToString();
            int Rhid = Int32.Parse(Hid);//教师编号
            int Rgrade = Int32.Parse(DDLgrade.SelectedValue);
            int Rclass = Int32.Parse(DDLclass.SelectedValue);

            // 检查确认状态
            string confirmValue = Request.Form["confirmRegistration"] ?? "";

            // 检查登记开关状态（直接使用页面上的控件）
            bool showConfirmDialog = CheckBoxRegister.Checked;

            if (confirmValue == "cancelled")
            {
                // 用户点击了取消，但仍然要进入上课状态，锁定按钮
                // 执行与确认相同的上课逻辑，但不记录到Skdj表
                LearnSite.BLL.Room rm_cancel = new LearnSite.BLL.Room();
                string cid_cancel = DDLCid.SelectedValue;
                if (!string.IsNullOrEmpty(cid_cancel))
                    rm_cancel.UpdateRcid(Rgrade, Rclass, Int32.Parse(cid_cancel));
                if (LearnSite.Common.XmlHelp.LoginMode() == 1) //如果是0，个人模式，1为班级模式
                {
                    int pwdlen = 3;
                    TBpwd.Text = rm_cancel.TeachingRoomSet(Rhid, Rgrade, Rclass, pwdlen);//返回班级密码
                    TBpwd.ToolTip = "班级模式下学生登录的密码！";
                }
                else
                {
                    TBpwd.Text = "个人模式";
                    int pwdlen = 6;
                    rm_cancel.TeachingRoomSet(Rhid, Rgrade, Rclass, pwdlen);//设置一下配合黄池祥老师的管理软件
                    TBpwd.ToolTip = "学生登录请使用自己的个人密码！";
                }
                Session[Hid + "grade"] = DDLgrade.SelectedValue;
                Session[Hid + "class"] = DDLclass.SelectedValue;
                Application["MyNumGrade"] = DDLgrade.SelectedValue;//学生选择学号页面MyNum.aspx，自动呈现上课班级
                Application[Application["MyNumGrade"] + "MyNumClass"] = DDLclass.SelectedValue;//学生选择学号页面MyNum.aspx，自动呈现上课班级
                Session[Hid + "StartTime"] = DateTime.Now.ToString();

                // 锁定控件，进入上课状态
                Btnset.Enabled = false;
                DDLgrade.Enabled = false;
                DDLclass.Enabled = false;
                DDLhouse.Enabled = false;
                Btnstudent.Enabled = true;
                Btnrefresh.Enabled = true;
                LearnSite.BLL.Students stu_cancel = new LearnSite.BLL.Students();
                stu_cancel.ThisClassTeamScoresNew(Rgrade, Rclass);//批量更新group新方法
                ShowNoSigin();
                DDLCid.Enabled = false;

                // 添加JavaScript确保客户端控件也被禁用（解决Jexus环境下锁定失效问题）
                string disableScriptForCancel = @"setTimeout(function() {
                    var ddlgrade = document.getElementById('" + DDLgrade.ClientID + @"');
                    var ddlclass = document.getElementById('" + DDLclass.ClientID + @"');
                    var ddlcid = document.getElementById('" + DDLCid.ClientID + @"');
                    var ddlhouse = document.getElementById('" + DDLhouse.ClientID + @"');
                    if(ddlgrade) ddlgrade.disabled = true;
                    if(ddlclass) ddlclass.disabled = true;
                    if(ddlcid) ddlcid.disabled = true;
                    if(ddlhouse) ddlhouse.disabled = true;
                }, 100);";
                ClientScript.RegisterStartupScript(this.GetType(), "DisableControls", disableScriptForCancel, true);
                return;
            }

            // 检查是否需要显示确认对话框
            if (showConfirmDialog && confirmValue != "confirmed")
            {
                // 登记开关开启且是第一次点击，显示确认弹窗
                // 注意：不在服务器端锁定控件，避免JavaScript解锁失效的问题

                string script = @"
                setTimeout(function() {
                    var msg = '教师：' + '" + HttpUtility.UrlDecode(tcook.Hname) + @"' + String.fromCharCode(10) +
                              '班级：' + '" + DDLgrade.SelectedValue + @"年级' + '" + DDLclass.SelectedValue + @"班' + String.fromCharCode(10) +
                              '课程：' + '" + DDLCid.SelectedItem.Text + @"' + String.fromCharCode(10) +
                              '节次：' + '" + GetCurrentTimeSlot() + @"节' + String.fromCharCode(10) + String.fromCharCode(10) +
                              '确认要登记本次上课信息吗？';
                    if(confirm(msg)) {
                        // 创建隐藏字段并设置为confirmed，然后重新提交表单
                        var hiddenField = document.createElement('input');
                        hiddenField.type = 'hidden';
                        hiddenField.name = 'confirmRegistration';
                        hiddenField.value = 'confirmed';
                        document.forms[0].appendChild(hiddenField);
                        // 使用__doPostBack重新提交，确保事件被触发
                        __doPostBack('" + Btnset.UniqueID + @"', '');
                    } else {
                        // 创建隐藏字段并设置为cancelled，然后重新提交表单
                        var hiddenField = document.createElement('input');
                        hiddenField.type = 'hidden';
                        hiddenField.name = 'confirmRegistration';
                        hiddenField.value = 'cancelled';
                        document.forms[0].appendChild(hiddenField);
                        // 使用__doPostBack重新提交，确保事件被触发
                        __doPostBack('" + Btnset.UniqueID + @"', '');
                    }
                }, 100);";
                ClientScript.RegisterStartupScript(this.GetType(), "ConfirmRegistration", script, true);
                return; // 直接返回，不执行后续代码
            }

            // 用户确认后执行的代码
            LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
            string cid = DDLCid.SelectedValue;
            if (!string.IsNullOrEmpty(cid))
                rm.UpdateRcid(Rgrade, Rclass, Int32.Parse(cid));
            if (LearnSite.Common.XmlHelp.LoginMode() == 1) //如果是0，个人模式，1为班级模式
            {
                int pwdlen = 3;
                TBpwd.Text = rm.TeachingRoomSet(Rhid, Rgrade, Rclass, pwdlen);//返回班级密码
                TBpwd.ToolTip = "班级模式下学生登录的密码！";
            }
            else
            {
                TBpwd.Text = "个人模式";
                int pwdlen = 6;
                rm.TeachingRoomSet(Rhid, Rgrade, Rclass, pwdlen);//设置一下配合黄池祥老师的管理软件
                TBpwd.ToolTip = "学生登录请使用自己的个人密码！";
            }
            Session[Hid + "grade"] = DDLgrade.SelectedValue;
            Session[Hid + "class"] = DDLclass.SelectedValue;
            Application["MyNumGrade"] = DDLgrade.SelectedValue;//学生选择学号页面MyNum.aspx，自动呈现上课班级
            Application[Application["MyNumGrade"] + "MyNumClass"] = DDLclass.SelectedValue;//学生选择学号页面MyNum.aspx，自动呈现上课班级
            Session[Hid + "StartTime"] = DateTime.Now.ToString();
            Btnset.Enabled = false;
            DDLgrade.Enabled = false;
            DDLclass.Enabled = false;
            DDLhouse.Enabled = false;
            DDLCid.Enabled = false;
            Btnstudent.Enabled = true;
            Btnrefresh.Enabled = true;

            // 添加JavaScript确保客户端控件也被禁用（解决Jexus环境下锁定失效问题）
            string disableScriptForConfirm = @"setTimeout(function() {
                var ddlgrade = document.getElementById('" + DDLgrade.ClientID + @"');
                var ddlclass = document.getElementById('" + DDLclass.ClientID + @"');
                var ddlcid = document.getElementById('" + DDLCid.ClientID + @"');
                var ddlhouse = document.getElementById('" + DDLhouse.ClientID + @"');
                if(ddlgrade) ddlgrade.disabled = true;
                if(ddlclass) ddlclass.disabled = true;
                if(ddlcid) ddlcid.disabled = true;
                if(ddlhouse) ddlhouse.disabled = true;
            }, 100);";
            ClientScript.RegisterStartupScript(this.GetType(), "DisableControls", disableScriptForConfirm, true);
            LearnSite.BLL.Students stu = new LearnSite.BLL.Students();
            stu.ThisClassTeamScoresNew(Rgrade, Rclass);//批量更新group新方法
            ShowNoSigin();
            //LearnSite.BLL.Survey vbll = new LearnSite.BLL.Survey();
            //vbll.SetClose(Rhid);//开始上课时，先将测验处于关闭状态
            
            // 获取当前节次
            string currentSlot = GetCurrentTimeSlot();

            // 检查是否已存在相同记录（现在只检查当天记录）
            if (!IsRecordExists(Rgrade, Rclass, cid, currentSlot))
            {
                // 获取课程标题
                LearnSite.Model.Courses model = new LearnSite.Model.Courses();
                LearnSite.BLL.Courses cs = new LearnSite.BLL.Courses();
                model = cs.GetModel(Int32.Parse(cid));
                string Ssctitle = model.Ctitle;

                // 获取当前时间信息
                DateTime dt = DateTime.Now;
                int Ssyear = dt.Year;
                int Ssmonth = dt.Month;
                int Ssday = dt.Day;
                string dayOfWeek = GetChineseDayOfWeek(dt.DayOfWeek);
                string Ssweek = dayOfWeek;
                string Sstname = HttpUtility.UrlDecode(tcook.Hname);
                int Sstid = Int32.Parse(Hid);

                // 登记上课信息
                LearnSite.BLL.Skdj gbll = new LearnSite.BLL.Skdj();
                gbll.addskdj(Sstid, dt, Rgrade, Rclass, Ssctitle, Ssyear, Ssmonth, Ssday, Ssweek, currentSlot, "正常", Sstname);
            }
        }
    }

    private void ShowSigin()
    {
        if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
        {
            string Hid =  tcook.Hid.ToString();
            int Rgrade = Int32.Parse(DDLgrade.SelectedValue);
            int Rclass = Int32.Parse(DDLclass.SelectedValue);
            DateTime dt = DateTime.Now;
            int Qyear = dt.Year;
            int Qmonth = dt.Month;
            int Qday = dt.Day;
            string SignSort = RBsort.SelectedValue;//获取上课页面签到排序方法
            LabelToday.Text = " 服务器日期校准：" + Qyear.ToString() + "年" + Qmonth.ToString() + "月" + Qday.ToString() + "日";
            LearnSite.BLL.Signin sg = new LearnSite.BLL.Signin();
            string pcroom = DDLhouse.SelectedValue;
            //机房视图
            if (SignSort.Equals("3"))
            {
                LearnSite.Model.SeatCollect sctm = sg.StartSignTable(Rgrade, Rclass, Qyear, Qmonth, Qday, pcroom);
                DLonline.DataSource = sctm.Dt;
                DLonline.DataBind();
                DLonline.RepeatColumns = sctm.Column;
                Labelsigin.Text = sctm.Online.ToString();
                
            }

            //其他的视图
            else
            {
                DLonline.DataSource = sg.StartSignClass(Rgrade, Rclass, Qyear, Qmonth, Qday, SignSort);
                DLonline.DataBind();
                DLonline.RepeatColumns = 8;
                Labelsigin.Text = DLonline.Items.Count.ToString();
                //if (((DataTable)DLonline.DataSource).Rows.Count==0)
                //{
                //    lblNoData.Visible = true;
                //}
                
            }
            lblNoData.Visible = (((DataTable)DLonline.DataSource).Rows.Count == 0);
        }
    }
    /// <summary>
    /// 设置作品展示和汇总表、座位表 链接
    /// </summary>
    private void showwtUrl()
    {
        string Rgrade = DDLgrade.SelectedValue;
        string Rclass = DDLclass.SelectedValue;
        string Rcid = DDLCid.SelectedValue;
        string Houseid = DDLhouse.SelectedValue;
        if (!string.IsNullOrEmpty(Rcid))
        {
            HylkDiskGroup.NavigateUrl = "~/teacher/sharegview.aspx?wgrade=" + Rgrade + "&wclass=" + Rclass;
            HylkDiskstu.NavigateUrl = "~/teacher/shareview.aspx?wgrade=" + Rgrade + "&wclass=" + Rclass;
            HLworkshow.NavigateUrl = "~/teacher/workshow.aspx?wgrade=" + Rgrade + "&wclass=" + Rclass + "&wcid=" + Rcid;
            HLtotal.NavigateUrl = "~/teacher/coursetotal.aspx?wgrade=" + Rgrade + "&wclass=" + Rclass + "&wcid=" + Rcid;
            HLrate.NavigateUrl = "~/teacher/learnrate.aspx?wgrade=" + Rgrade + "&wclass=" + Rclass + "&wcid=" + Rcid;


            if (Houseid != "")
                HyperLinkSeat.NavigateUrl = "~/seat/seatshow.aspx?hid=" + Houseid + "&sgrade=" + Rgrade + "&sclass=" + Rclass;
        }
    }
    private void ShowNoSigin()
    {
        int Rgrade = Int32.Parse(DDLgrade.SelectedValue);
        int Rclass = Int32.Parse(DDLclass.SelectedValue);
        LearnSite.BLL.Students sbll = new LearnSite.BLL.Students();
        int Syear = sbll.GetYear(Rgrade, Rclass);
        DateTime dt = DateTime.Now;
        int Qyear = dt.Year;
        int Qmonth = dt.Month;
        int Qday = dt.Day;
        LearnSite.BLL.Signin sg = new LearnSite.BLL.Signin();
        DLnotline.DataSource = sg.StartNoSignClassTwo(Rgrade, Rclass, Syear, Qyear, Qmonth, Qday);
        DLnotline.DataBind();
        Labelsigno.Text = DLnotline.Items.Count.ToString();
    }

    private void GradeClass()
    {
        if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
        {
            LearnSite.BLL.Room room = new LearnSite.BLL.Room();

            string Hid = tcook.Hid.ToString();
            string grad = Request.QueryString["grade"];
            string classCode = Request.QueryString["class"];
            
            // 首先尝试智能选择
            bool smartSelected = TrySmartSelection();
            
            if (!smartSelected)
            {
                // 如果智能选择失败，使用原有逻辑
                DDLgrade.DataSource = room.GetGrade(Int32.Parse(Hid));
                DDLgrade.DataTextField = "Rgrade";
                DDLgrade.DataValueField = "Rgrade";
                DDLgrade.DataBind();
                if (!string.IsNullOrEmpty(grad))
                {
                    Session[Hid + "grade"] = grad;
                }
                if (Session[Hid + "grade"] != null)
                    DDLgrade.SelectedValue = Session[Hid + "grade"].ToString();

                int Rgrade = Int32.Parse(DDLgrade.SelectedValue);
                LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
                DDLclass.DataSource = rm.GetLimitClass(Rgrade);
                DDLclass.DataTextField = "Rclass";
                DDLclass.DataValueField = "Rclass";
                DDLclass.DataBind();
                if (!string.IsNullOrEmpty(classCode))
                {
                    Session[Hid + "class"] = classCode;
                }
                if (Session[Hid + "class"] != null)
                    DDLclass.SelectedValue = Session[Hid + "class"].ToString();
            }
        }
    }

    private void HowManyWork()
    {
        int i = 0;
        foreach (DataListItem item in this.DLonline.Items)
        {
            if (Int32.Parse(((Label)item.FindControl("Labelwork")).Text) > 0)
            {
                i++;
            }
        }
        Labelcount.Text = "已经提交作品共" + i.ToString() + "位";
    }

    protected void DLonline_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        Color cr = Color.Silver;
        Label hl = new Label();
        hl = (Label)e.Item.FindControl("HyperSname");
        HyperLink lk = (HyperLink)e.Item.FindControl("Groupflag");
        LinkButton lbunlock = (LinkButton)e.Item.FindControl("Lunlock");
        if (!string.IsNullOrEmpty(hl.Text))
        {
            Label lb = new Label();
            lb = (Label)e.Item.FindControl("Labelwork");
            int Qwork = Int32.Parse((lb.Text));
            
            // 获取学生学号，用于查询作业得分
            Label lbqnum = (Label)e.Item.FindControl("Labelqnum");
            string myqnum = lbqnum.Text;
            
            // 查询学生作业得分（直接从Utility获取）
            string sql = "SELECT TOP 1 Wscore FROM Works WHERE Wnum = '" + myqnum + "' AND Wscore > 0 ORDER BY Wid DESC";
            int Wscore = LearnSite.DBUtility.DbHelperSQL.FindNum(sql);
            
            // 设置作业得分显示
            Label lbwscore = (Label)e.Item.FindControl("LabelWscore");
            if (lbwscore != null)
            {
                lbwscore.Text = Wscore.ToString();
                // 得分为0时显示不变，有分数时显示为红色
                if (Wscore > 0)
                {
                    lbwscore.ForeColor = Color.Red;
                }
                else
                {
                    lbwscore.ForeColor = Color.Black; // 恢复默认颜色
                }
            }
            
            // 查询当前选中学案的最新作品评价状态
            CheckBox cbgrade = (CheckBox)e.Item.FindControl("CBgrade");
            if (cbgrade != null && !string.IsNullOrEmpty(DDLCid.SelectedValue))
            {
                int Wcid = Int32.Parse(DDLCid.SelectedValue);
                string checkSql = "SELECT TOP 1 Wcheck FROM Works WHERE Wnum = '" + myqnum + "' AND Wcid = " + Wcid + " ORDER BY Wid DESC";
                bool isChecked = LearnSite.DBUtility.DbHelperSQL.FindNum(checkSql) > 0;
                cbgrade.Checked = isChecked;
            }
            
            switch (Qwork)
            {
                case 1:
                    hl.BackColor = Labelone.BackColor;
                    break;
                case 2:
                    hl.BackColor = Labeltwo.BackColor;
                    break;
                case 3:
                    hl.BackColor = Labelthree.BackColor;
                    break;
                case 4:
                    hl.BackColor = Labelfour.BackColor;
                    break;
            }
            if (Qwork > 4)
            {
                hl.BackColor = Labelmore.BackColor;
            }
            string sleader = ((Label)e.Item.FindControl("LabelSleader")).Text.ToLower();
            string sgroup = ((Label)e.Item.FindControl("LabelSgroup")).Text;
            string sgtitle = ((Label)e.Item.FindControl("LabelSgtitle")).Text;
            string vpath = "~/images/gcard.gif";
            string Qid = this.DLonline.DataKeys[e.Item.ItemIndex].ToString();
            string curCid = DDLCid.SelectedValue;
            if (sgroup == "" || sgroup == "0")
            {
                lk.ToolTip = "未分组";
                vpath = "~/images/ncard.gif";//如果未分组,换图标
            }
            else
            {
                int sgp = Int32.Parse(sgroup);
                ((Label)e.Item.FindControl("Labelcolor")).ForeColor = LearnSite.Common.ColorDeel.GroupColor(sgp);//设置组颜色标志，以便签到页面查找

                if (sleader == "true" && sgp > 0)
                {
                    vpath = "~/images/gflag.gif";//如果是组长的话,换图标
                    lk.ToolTip = sgroup + "小组组长";
                    if (sgtitle != "")
                        lk.ToolTip = sgtitle + "组长";
                    if (curCid != "")
                    {
                        string jslk = "attitudegroup('" + sgroup + "', '" + hl.Text + "', '" + Qid + "', '" + curCid + "');";
                        lk.Attributes.Add("onclick", jslk);
                    }
                }
                else
                {
                    vpath = "~/images/gcard.gif";//小组成员图标
                    lk.ToolTip = sgroup + "小组成员";
                    if (sgtitle != "")
                        lk.ToolTip = sgtitle + "小组成员";
                }
            }
            lk.ImageUrl = vpath + "?temp=" + DateTime.Now.Millisecond.ToString();
            int Qattitude = Int32.Parse(((Label)e.Item.FindControl("Labelattitude")).Text);
            string Qnote = ((Label)e.Item.FindControl("Labelnote")).Text;
            if (curCid != "")
            {
                // 修改点击事件，先显示评价内容，然后再打开attitude.aspx页面
                string jsstr = "setTimeout(function() { attitude('" + Qid + "', '" + Server.UrlEncode(hl.Text) + "', '" + Qattitude + "', '" + curCid + "'); }, 0);";
                hl.Attributes.Add("onclick", jsstr);
            }
            string pp = LearnSite.Common.Photo.ExistStuPhoto(myqnum);
            if (pp != "none")
            {
                lbqnum.Attributes.Add("class", "tooltip");
                lbqnum.ToolTip = pp;
                lbqnum.BackColor = Color.Bisque;
            }
            if (Qattitude != 0)
            {
                hl.ToolTip = "学习表现：" + Qnote + "\n评价得分：  " + Qattitude;
            }
            else
            {
                hl.ToolTip = "点击评价学习表现";
            }
            if (!LearnSite.Common.App.IsLogin(myqnum))
            {
                lbunlock.Visible = false;
            }
            else
            {
                lbunlock.Attributes.Add("onclick", "return   confirm('您确定要将该生踢下线吗？');");
            }
        }
        else
        {
            lbunlock.Visible = false;
            lk.ImageUrl = "~/images/nocomputer.gif" + "?temp=" + DateTime.Now.Millisecond.ToString();
            ((Label)e.Item.FindControl("Labelcolor")).Visible = false;
            ((Label)e.Item.FindControl("HyperSname")).Visible = false;
            ((Label)e.Item.FindControl("Labelqnum")).Visible = false;

        }
    }

    protected void DDLgrade_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
        {
            if (DDLgrade.SelectedValue != null)
            {
                int Rgrade = Int32.Parse(DDLgrade.SelectedValue);
                LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
                DDLclass.DataSource = rm.GetLimitClass(Rgrade);
                DDLclass.DataBind();
                Showkc();
                ShowSigin();
                ShowNoSigin();
                HowManyWork();
                string Hid = tcook.Hid.ToString();
                Session[Hid + "grade"] = DDLgrade.SelectedValue;
                Session[Hid + "class"] = DDLclass.SelectedValue;
                BtnaAllQuit.Enabled = true;
                showLock();
                showCid();
                showwtUrl();
                showMenu();
            }
        }
    }
    protected void Btnstudent_Click(object sender, EventArgs e)
    {
        int Rgrade = Int32.Parse(DDLgrade.SelectedValue);
        int Rclass = Int32.Parse(DDLclass.SelectedValue);
        LearnSite.Common.CookieHelp.SimulationStudentCookies(Rgrade, Rclass);//模拟该班级学生登录cookies设置
        System.Threading.Thread.Sleep(200);
        Response.Redirect("~/index.aspx", true);
    }
    protected void DDLclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (DDLclass.SelectedValue != null)
        {
            string Hid = tcook.Hid.ToString();
            Session[Hid + "class"] = DDLclass.SelectedValue;
            Showkc();
            ShowSigin();
            ShowNoSigin();
            HowManyWork();
            BtnaAllQuit.Enabled = true;
            showLock();
            showwtUrl();
            showMenu();
        }
    }
    protected void DLdonekc_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
        {
            return;
        }
        int Wcid = Int32.Parse(DLdonekc.DataKeys[e.Item.ItemIndex].ToString());
        LearnSite.BLL.Works bll = new LearnSite.BLL.Works();
        int Rgrade = Int32.Parse(DDLgrade.SelectedValue);
        int Rclass = Int32.Parse(DDLclass.SelectedValue);
        ((Label)e.Item.FindControl("wk")).Text = bll.HowCourseWorks(Wcid, Rgrade, Rclass);
    }
    protected void DLnotline_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
        {
            return;
        }
        Label hl = new Label();
        hl = (Label)e.Item.FindControl("lbQname");
        Label lb = new Label();
        lb = (Label)e.Item.FindControl("LabelNnum");
        string jsstr = "notsg('" + lb.Text + "', '" + DDLgrade.SelectedValue + "', '" + Server.UrlEncode(hl.Text) + "');";
        hl.Attributes.Add("onclick", jsstr);

        string pp = LearnSite.Common.Photo.ExistStuPhoto(lb.Text);
        if (pp != "none")
        {
            lb.Attributes.Add("class", "tooltip");
            lb.ToolTip = pp;
            lb.BackColor = Color.Bisque;
        }

        LearnSite.BLL.NotSign bll = new LearnSite.BLL.NotSign();
        string getnote = bll.GetNoteToday(lb.Text);
        if (getnote.Trim() != "")
            hl.ToolTip = getnote;
        else
            hl.ToolTip = "点击请给未签到备注";
    }
    protected void DLnewkc_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
        {
            return;
        }
        CheckBox cb = new CheckBox();
        cb = (CheckBox)e.Item.FindControl("Ck");
        Button imgbtn = new Button();
        imgbtn = (Button)e.Item.FindControl("PubSet");
        if (cb.Checked)
        {
            cb.ToolTip = "已发布";
            imgbtn.ToolTip = "点击隐藏";
        }
        else
        {
            cb.ToolTip = "已隐藏";
            imgbtn.ToolTip = "点击发布";
        }
    }
    protected void DLonline_ItemCommand(object source, DataListCommandEventArgs e)
    {
        string myqnum = ((Label)e.Item.FindControl("Labelqnum")).Text;
        if (e.CommandName == "UnLock")
        {
            LearnSite.Common.App.AppUserRemove(myqnum);//在线列表中踢除
            LearnSite.Common.App.AppKickUserAdd(myqnum);//踢除列表中增加
            ((LinkButton)e.Item.FindControl("Lunlock")).Enabled = false;
            System.Threading.Thread.Sleep(500);
        }
        ShowSigin();
    }
    protected void BtnaAllQuit_Click(object sender, EventArgs e)
    {
        if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
        {
            int sgrade = 0;
            int sclass = 0;
            if (DDLclass.SelectedValue != "")
                sclass = Int32.Parse(DDLclass.SelectedValue);
            if (DDLgrade.SelectedValue != "")
                sgrade = Int32.Parse(DDLgrade.SelectedValue);
            LearnSite.Common.App.GradeClassRemove(sgrade, sclass);
            BtnaAllQuit.Enabled = false;
            ShowSigin();
        }
    }
    protected void RBsort_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowSigin();
    }
    protected void CheckBoxip_CheckedChanged(object sender, EventArgs e)
    {
        int Rgrade = Int32.Parse(DDLgrade.SelectedValue);
        int Rclass = Int32.Parse(DDLclass.SelectedValue);
        LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
        bool rlock = CheckBoxip.Checked;
        rm.UpdateLock(Rgrade, Rclass, rlock);
    }
    protected void CheckBoxFace_CheckedChanged(object sender, EventArgs e)
    {
        ShowSigin();
        ShowNoSigin();
    }
    protected void DLnewkc_ItemCommand(object source, DataListCommandEventArgs e)
    {
        int Cid = Int32.Parse(DLnewkc.DataKeys[e.Item.ItemIndex].ToString());
        if (e.CommandName == "P" && Btnset.Enabled)
        {
            LearnSite.BLL.Courses cbll = new LearnSite.BLL.Courses();
            cbll.UpdateCpublish(Cid);
            System.Threading.Thread.Sleep(200);
            Showkc();
            showCid(); 
            showMenu();
        }
    }
    protected void CheckBoxOpen_CheckedChanged(object sender, EventArgs e)
    {
        int Rgrade = Int32.Parse(DDLgrade.SelectedValue);
        int Rclass = Int32.Parse(DDLclass.SelectedValue);
        LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
        bool ropen = CheckBoxOpen.Checked;
        rm.UpdateRopen(Rgrade, Rclass, ropen);
    }
    protected void Btnrefresh_Click(object sender, EventArgs e)
    {
        if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
        {
            if (!Btnset.Enabled)
            {
                string Hid = tcook.Hid.ToString();
                Btnset.Enabled = false;
                DDLgrade.Enabled = false;
                DDLclass.Enabled = false;
                DDLhouse.Enabled = false;
                DDLCid.Enabled = false;
                Btnstudent.Enabled = true;
                ShowSigin();
                ShowNoSigin();
                HowManyWork();

                // 添加JavaScript确保客户端控件也被禁用
                string disableScriptForRefresh = @"setTimeout(function() {
                    var ddlgrade = document.getElementById('" + DDLgrade.ClientID + @"');
                    var ddlclass = document.getElementById('" + DDLclass.ClientID + @"');
                    var ddlcid = document.getElementById('" + DDLCid.ClientID + @"');
                    var ddlhouse = document.getElementById('" + DDLhouse.ClientID + @"');
                    if(ddlgrade) ddlgrade.disabled = true;
                    if(ddlclass) ddlclass.disabled = true;
                    if(ddlcid) ddlcid.disabled = true;
                    if(ddlhouse) ddlhouse.disabled = true;
                }, 100);";
                ClientScript.RegisterStartupScript(this.GetType(), "DisableControls", disableScriptForRefresh, true);
                //if (Session[Hid + "StartTime"] != null)
                //{         注释掉是因为某些服务器时间转换出错
                //    DateTime oldtime = DateTime.Parse(Session[Hid + "StartTime"].ToString()); 
                //    DateTime nowtime = DateTime.Now;
                //    Labelfresh.Text = "上课时间已经过去：" + LearnSite.Common.Computer.DatagoneMinute(oldtime, nowtime) + "分钟";
                //}
            }
        }
    }

    protected void CheckBoxRgauge_CheckedChanged(object sender, EventArgs e)
    {
        LearnSite.BLL.Room rbll = new LearnSite.BLL.Room();
        bool isgauge = CheckBoxRgauge.Checked;
        int Rgrade = Int32.Parse(DDLgrade.SelectedValue);
        int Rclass = Int32.Parse(DDLclass.SelectedValue);
        rbll.UpdateMyRgauge(Rgrade, Rclass, isgauge);//选择班级上课时，默认将互评自动设置为关，方便适当使用
    }
    protected void CheckBoxPwd_CheckedChanged(object sender, EventArgs e)
    {
        LearnSite.BLL.Room rbll = new LearnSite.BLL.Room();
        int Rgrade = Int32.Parse(DDLgrade.SelectedValue);
        int Rclass = Int32.Parse(DDLclass.SelectedValue);
        bool ispwdsee = CheckBoxPwd.Checked;
        rbll.UpdateRpwdsee(Rgrade, Rclass, ispwdsee);
    }
    protected void DDLCid_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Rgrade = DDLgrade.SelectedValue;
        string Rclass = DDLclass.SelectedValue;
        string Rcid = DDLCid.SelectedValue;
        if (Rcid != "")
        {
            HLworkshow.NavigateUrl = "~/teacher/workshow.aspx?wgrade=" + Rgrade + "&wclass=" + Rclass + "&wcid=" + Rcid;
            HLtotal.NavigateUrl = "~/teacher/coursetotal.aspx?wgrade=" + Rgrade + "&wclass=" + Rclass + "&wcid=" + Rcid;
            HLrate.NavigateUrl = "~/teacher/learnrate.aspx?wgrade=" + Rgrade + "&wclass=" + Rclass + "&wcid=" + Rcid;
            ShowSigin();
            showMenu();
        }
    }
    protected void DDLhouse_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowSigin();
        string pcroom = DDLhouse.SelectedValue;
        if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null && pcroom != "")
        {
            string Hid =  tcook.Hid.ToString();
            LearnSite.BLL.Teacher tbll = new LearnSite.BLL.Teacher();
            tbll.updateHroom(Int32.Parse(Hid), pcroom);//记录上课的电脑室名称
        }
    }
    protected void CheckBoxShare_CheckedChanged(object sender, EventArgs e)
    {
        LearnSite.BLL.Room rbll = new LearnSite.BLL.Room();
        int Rgrade = Int32.Parse(DDLgrade.SelectedValue);
        int Rclass = Int32.Parse(DDLclass.SelectedValue);
        bool isshare = CheckBoxShare.Checked;
        rbll.UpdateRshare(Rgrade, Rclass, isshare);
        if (!isshare)
        {
            if (CheckBoxGroupShare.Checked)
            {
                CheckBoxGroupShare.Checked = false;
                rbll.UpdateRgroupshare(Rgrade, Rclass, isshare);//网盘总开关，同步一下小组网盘
            }
        }
    }
    protected void CheckBoxGroupShare_CheckedChanged(object sender, EventArgs e)
    {
        bool isshare = CheckBoxShare.Checked;
        if (isshare)
        {
            bool isgroupshare = CheckBoxGroupShare.Checked;
            LearnSite.BLL.Room rbll = new LearnSite.BLL.Room();
            int Rgrade = Int32.Parse(DDLgrade.SelectedValue);
            int Rclass = Int32.Parse(DDLclass.SelectedValue);
            rbll.UpdateRgroupshare(Rgrade, Rclass, isgroupshare);
        }
        else
        {
            CheckBoxGroupShare.Checked = false;
            LearnSite.Common.WordProcess.Alert("请先选中前面的网盘开关，启用网盘系统！", this.Page);
        }
    }
    protected void CheckBoxScratch_CheckedChanged(object sender, EventArgs e)
    {
        LearnSite.BLL.Room rbll = new LearnSite.BLL.Room();
        bool isScratch = CheckBoxScratch.Checked;
        int Rgrade = Int32.Parse(DDLgrade.SelectedValue);
        int Rclass = Int32.Parse(DDLclass.SelectedValue);
        rbll.UpdateMyRscratch(Rgrade, Rclass, isScratch);//可以在上课页面控制编程开始或暂停，不用进入模拟学生
    }
    protected void CheckBoxLogin_CheckedChanged(object sender, EventArgs e)
    {
        LearnSite.BLL.Room rbll = new LearnSite.BLL.Room();
        bool isLogin = CheckBoxLogin.Checked;
        int Rgrade = Int32.Parse(DDLgrade.SelectedValue);
        int Rclass = Int32.Parse(DDLclass.SelectedValue);
        rbll.UpdateMyRlogin(Rgrade, Rclass, isLogin);//可以在上课页面控制编程开始或暂停，不用进入模拟学生

    }

    protected void CBgrade_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = sender as CheckBox;
        //取得当前被选中项的索引  
        int index = (chk.NamingContainer as DataListItem).ItemIndex;
        //取得当前选中项中的学号
        Label lbqnum = this.DLonline.Items[index].FindControl("Labelqnum") as Label;
        string myqnum = lbqnum.Text;
        
        //确保当前选中学案
        if (!string.IsNullOrEmpty(DDLCid.SelectedValue))
        {
            int Wcid = Int32.Parse(DDLCid.SelectedValue);
            
            //更新Works表的Wcheck字段
            LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
            //获取作品Wid（最新的作品）
            string getWidSql = "SELECT TOP 1 Wid FROM Works WHERE Wnum = '" + myqnum + "' AND Wcid = " + Wcid + " ORDER BY Wid DESC";
            string WidStr = LearnSite.DBUtility.DbHelperSQL.FindString(getWidSql);
            if (!string.IsNullOrEmpty(WidStr))
            {
                int Wid = Int32.Parse(WidStr);
                wbll.CancleScoreWork(Wid, chk.Checked);
            }
            System.Threading.Thread.Sleep(200);
            ShowSigin();
        }
    }
    



    private void showMenu()
    {
        string cid = DDLCid.SelectedValue;
        if (!String.IsNullOrEmpty(cid))
        {
            LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
            System.Data.DataTable dt = lbll.GetMenu(Int32.Parse(cid)).Tables[0];
            dt.Columns.Add("Limgurl");
            int dcount = dt.Rows.Count;
            if (dcount > 0)
            {
                for (int i = 0; i < dcount; i++)
                {
                    string Ltype = dt.Rows[i]["Ltype"].ToString();
                    string imgurl = "";
                    switch (Ltype)
                    {
                        case "1"://活动
                            imgurl = "~/images/mission.png";
                            break;
                        case "2"://调查
                            imgurl = "~/images/survey.png";
                            break;
                        case "3"://讨论
                            imgurl = "~/images/topic.png";
                            break;
                        case "4"://表单
                            imgurl = "~/images/inquiry.png";
                            break;
                        case "5"://编程
                            imgurl = "~/images/program.png";
                            break;
                        case "6"://描述
                            imgurl = "~/images/description.png";
                            break;
                        case "8"://编程
                            imgurl = "~/images/python.png";
                            break;
                        case "9"://交互式python测评
                            imgurl = "~/images/console.png";
                            break;
                        case "10"://流程图
                            imgurl = "~/images/mxgraph.png";
                            break;
                        case "11"://像素画
                            imgurl = GetCustomActivityIconUrl("11");
                            break;
                        case "12"://单网页
                            imgurl = "~/images/html.png";
                            break;
                        case "13"://拼图
                            imgurl = "~/images/pythonblock.png";
                            break;
                        case "14"://积木
                            imgurl = "~/images/blockpy.png";
                            break;
                        case "15"://导图
                            imgurl = "~/images/kitymind.png";
                            break;
                        case "16"://表格处理
                            imgurl = "~/images/sheet.png";
                            break;
                        case "17"://二维码
                            imgurl = GetCustomActivityIconUrl("17");
                            break;
                        case "18"://在线文档
                            imgurl = GetCustomActivityIconUrl("18");
                            break;
                        case "19"://在线演示文稿
                            imgurl = GetCustomActivityIconUrl("19");
                            break;
                        case "20"://在线海报设计
                            imgurl = GetCustomActivityIconUrl("20");
                            break;
                        case "21"://风格迁移
                            imgurl = GetCustomActivityIconUrl("21");
                            break;
                        case "22"://图像分类
                            imgurl = GetCustomActivityIconUrl("22");
                            break;
                        case "23"://人脸识别
                            imgurl = GetCustomActivityIconUrl("23");
                            break;
                        case "24"://物联网
                            imgurl = GetCustomActivityIconUrl("24");
                            break;
                        case "25"://手绘画布
                            imgurl = GetCustomActivityIconUrl("25");
                            break;
                        case "26"://推箱子地图
                            imgurl = GetCustomActivityIconUrl("26");
                            break;
                        case "27"://人工智能对话
                            imgurl = GetCustomActivityIconUrl("27");
                            break;
                        case "28"://人工智能对话
                            imgurl = GetCustomActivityIconUrl("28");
                            break;
                        case "29"://文字识别
                            imgurl = GetCustomActivityIconUrl("29");
                            break;
                        case "30"://声音分析
                            imgurl = GetCustomActivityIconUrl("30");
                            break;
                        case "31"://井字棋
                            imgurl = GetCustomActivityIconUrl("31");
                            break;
                        case "32"://手写数字识别
                            imgurl = GetCustomActivityIconUrl("32");
                            break;
                        case "33"://Markdown写作
                            imgurl = GetCustomActivityIconUrl("33");
                            break;
                        case "34"://iframe嵌入网页
                            imgurl = GetCustomActivityIconUrl("34");
                            break;
                        case "35"://文生图
                            imgurl = GetCustomActivityIconUrl("35");
                            break;
                        case "36"://素材库
                            imgurl = GetCustomActivityIconUrl("36");
                            break;
                        case "37"://网站设计
                            imgurl = GetCustomActivityIconUrl("37");
                            break;

                        default://默认
                            imgurl = "~/images/lesson.png";
                            break;
                    }
                    dt.Rows[i]["Limgurl"] = imgurl;
                }
            }
            DataListMenu.DataSource = dt;
            DataListMenu.DataBind();
            DataListMenu.Visible = true;
        }
        else
        {
            DataListMenu.Visible = false;
        }
    }

    protected void DataListMenu_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
        {
            return;
        }
        CheckBox cb = new CheckBox();
        cb = (CheckBox)e.Item.FindControl("CheckBoxShow");
        LinkButton imgbtn = new LinkButton();
        imgbtn = (LinkButton)e.Item.FindControl("imgBtn");
        Label lt = new Label();
        lt = (Label)e.Item.FindControl("lableTitle");
        if (cb.Checked)
        {
            imgbtn.ToolTip = "点击隐藏";
            lt.ToolTip = "已发布";
            lt.ForeColor = System.Drawing.Color.Black;
        }
        else
        {
            imgbtn.ToolTip = "点击发布";
            lt.ToolTip = "已隐藏";
            lt.ForeColor = System.Drawing.Color.Gainsboro;
        }
    }
    protected void DataListMenu_ItemCommand(object source, DataListCommandEventArgs e)
    {
        int Lid = Int32.Parse(DataListMenu.DataKeys[e.Item.ItemIndex].ToString());
        if (e.CommandName == "P" )
        {
            LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
            lbll.UpdateLshow(Lid);
            System.Threading.Thread.Sleep(200);
            showMenu();
        }
    }
    protected void BtnMenuOpenAll_Click(object sender, EventArgs e)
    {
        BatchSetCurrentCourseMenuVisibility(true);
    }
    protected void BtnMenuCloseAll_Click(object sender, EventArgs e)
    {
        BatchSetCurrentCourseMenuVisibility(false);
    }
    private void BatchSetCurrentCourseMenuVisibility(bool isOpen)
    {
        string cid = DDLCid.SelectedValue;
        if (String.IsNullOrEmpty(cid))
        {
            return;
        }
        LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
        System.Data.DataTable dt = lbll.GetMenu(Int32.Parse(cid)).Tables[0];
        int count = dt.Rows.Count;
        for (int i = 0; i < count; i++)
        {
            int lid = Convert.ToInt32(dt.Rows[i]["Lid"]);
            bool currentShow = Convert.ToBoolean(dt.Rows[i]["Lshow"]);
            if (currentShow != isOpen)
            {
                if (isOpen)
                {
                    lbll.OpenLshow(lid);
                }
                else
                {
                    lbll.CloseLshow(lid);
                }
            }
        }
        System.Threading.Thread.Sleep(200);
        showMenu();
    }
    protected void CheckBoxPass_CheckedChanged(object sender, EventArgs e)
    {
        int Rgrade = Int32.Parse(DDLgrade.SelectedValue);
        int Rclass = Int32.Parse(DDLclass.SelectedValue);
        LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
        bool rpass = CheckBoxPass.Checked;
        rm.UpdateRpass(Rgrade, Rclass, rpass);
    }

    private string GetCustomActivityIconUrl(string category)
    {
        return LearnSite.Common.CustomActivityCatalog.GetMeta(category).IconUrl;
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        if (Request.Cookies[LearnSite.Common.CookieHelp.teaCookieNname] != null)
        {
            string Hid = tcook.Hid.ToString();
            int Rhid = Int32.Parse(Hid);//教师编号
            int Rgrade = Int32.Parse(DDLgrade.SelectedValue);
            int Rclass = Int32.Parse(DDLclass.SelectedValue);
            string script = @"
            setTimeout(function() {
                var msg = '教师：' + '" + HttpUtility.UrlDecode(tcook.Hname) + @"' + String.fromCharCode(10) +
                          '班级：' + '" + DDLgrade.SelectedValue + @"年级' + '" + DDLclass.SelectedValue + @"班' + String.fromCharCode(10) + 
                          '课程：' + '" + DDLCid.SelectedItem.Text + @"' + String.fromCharCode(10) + 
                          '节次：' + '" + GetCurrentTimeSlot() + @"节' + String.fromCharCode(10) + String.fromCharCode(10) + 
                          '确认要登记本次上课信息吗？';
                if(confirm(msg)) {
                    document.getElementById('" + Button1.ClientID + @"').click();
                }
            }, 1000);";
            string Ssession = GetCurrentTimeSlot();  // 自动获取当前节次
            // string Ssnotes = Textbeizhu.Text.Trim();  // 备注功能已移除
            string Sstname = HttpUtility.UrlDecode(tcook.Hname); // 获取当前登录教师姓名
            LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
            string cid = DDLCid.SelectedValue;
            int Sstid = Int32.Parse(Hid);
            int Ssgrade = Int32.Parse(DDLgrade.SelectedValue);
            int Ssclass = Int32.Parse(DDLclass.SelectedValue);
            //string Ssctitle = DDLCid.SelectedValue;
            //根据Cid获取课程标题
            LearnSite.Model.Courses model = new LearnSite.Model.Courses();
            LearnSite.BLL.Courses cs = new LearnSite.BLL.Courses();
            model = cs.GetModel(Int32.Parse(cid));
            string Ssctitle = model.Ctitle;
            //DDLCid.DataValueField = "Cid";
            //DDLCid.DataBind();
            //获取当前时间，转换成年、月、日、星期
            DateTime dt = DateTime.Now; //获取当前系统日期和时间，并将其存储在dt变量中
            int Ssyear = dt.Year;//Qyear 被赋值为 dt 的年份部分。
            int Ssmonth = dt.Month;//Qmonth 被赋值为 dt 的月份部分。
            int Ssday = dt.Day;//Qday 被赋值为 dt 的日期部分。
            DateTime Ssdate = DateTime.Now;
            string dayOfWeek = GetChineseDayOfWeek(dt.DayOfWeek);//把当前时间转换成星期
            string Ssweek = dayOfWeek;//把当前时间转换成星期
            //string Qweek = Qdate.DayOfWeek.ToString();
            //int NewSid = stubll.AddStudent(student);//
            LearnSite.BLL.Skdj gbll = new LearnSite.BLL.Skdj();
            gbll.addskdj(Sstid, Ssdate, Ssgrade, Ssclass, Ssctitle, Ssyear, Ssmonth, Ssday, Ssweek, Ssession, "正常", Sstname);//教师上课内容登记
            if (!string.IsNullOrEmpty(cid))
                rm.UpdateRcid(Rgrade, Rclass, Int32.Parse(cid));
        }
    }

//以下新增为自动判断上课节次 罗启斌
    private string GetCurrentTimeSlot()
    {
        // 使用统一的节次判断方法
        return LearnSite.Common.TimeSlotHelper.GetCurrentTimeSlot();
    }

    private bool IsRecordExists(int grade, int classNum, string cid, string slot)
    {
        if (string.IsNullOrEmpty(cid) || string.IsNullOrEmpty(slot))
            return false;

        try
        {
            DateTime today = DateTime.Today;
            string sql = @"SELECT COUNT(*) FROM Skdj 
                        WHERE Ssgrade=@Grade AND Ssclass=@Class 
                        AND Ssctitle=@Ctitle 
                        AND Ssession=@Session
                        AND CONVERT(date,Ssdate)=@Today";
            object result = LearnSite.DBUtility.DbHelperSQL.GetSingle(sql,
                new System.Data.SqlClient.SqlParameter("@Grade", grade),
                new System.Data.SqlClient.SqlParameter("@Class", classNum),
                new System.Data.SqlClient.SqlParameter("@Ctitle", GetCourseTitle(cid)),
                new System.Data.SqlClient.SqlParameter("@Session", slot),
                new System.Data.SqlClient.SqlParameter("@Today", today));

            return Convert.ToInt32(result) > 0;
        }
        catch (Exception ex)
        {
            // 记录错误日志
            // LearnSite.Common.LogHelper.WriteLog("检查重复记录出错：" + ex.Message);
            return false;
        }
    }

    private string GetCourseTitle(string cid)
    {
        LearnSite.Model.Courses model = new LearnSite.Model.Courses();
        LearnSite.BLL.Courses cs = new LearnSite.BLL.Courses();
        model = cs.GetModel(Int32.Parse(cid));
        return model.Ctitle;
    }
//新增自动判断上课时间结束 罗启斌

    /// <summary>
    /// 尝试根据课程表智能选择班级
    /// </summary>
    /// <returns>是否成功智能选择</returns>
    private bool TrySmartSelection()
    {
        try
        {
            DateTime now = DateTime.Now;
            TimeSpan currentTime = now.TimeOfDay;
            int dayOfWeek = GetDayOfWeekNumber(now.DayOfWeek);

            // 只在周一到周五（1-5）进行智能选择
            if (dayOfWeek >= 1 && dayOfWeek <= 5)
            {
                LearnSite.BLL.Courses courseBll = new LearnSite.BLL.Courses();
                int teacherId = tcook.Hid;

                // 首先查找当前时间段的课程
                DataTable currentSchedule = courseBll.GetCurrentSchedule(teacherId, currentTime, dayOfWeek);

                if (currentSchedule.Rows.Count > 0)
                {
                    // 找到当前时间段的课程
                    DataRow row = currentSchedule.Rows[0];
                    string grade = row["Grade"].ToString();
                    string classNum = row["Class"].ToString();

                    SetGradeAndClass(grade, classNum, "当前课程");
                    lblScheduleInfo.Text = string.Format("当前时间 {0:HH:mm} 正在上课：{1}年级{2}班", now, grade, classNum);
                    return true;
                }
                else
                {
                    // 查找下一节课
                    DataTable nextSchedule = courseBll.GetNextSchedule(teacherId, currentTime, dayOfWeek);

                    if (nextSchedule.Rows.Count > 0)
                    {
                        DataRow row = nextSchedule.Rows[0];
                        string grade = row["Grade"].ToString();
                        string classNum = row["Class"].ToString();
                        TimeSpan startTime = (TimeSpan)row["StartTime"];

                        SetGradeAndClass(grade, classNum, "下节课程");
                        lblScheduleInfo.Text = string.Format("下节课 {0:hh\\:mm} 开始：{1}年级{2}班", startTime, grade, classNum);
                        return true;
                    }
                    else
                    {
                        lblScheduleInfo.Text = "今天没有更多课程安排";
                        return false;
                    }
                }
            }
            else
            {
                // 周末显示选择框，提示用户选择按哪一天的课表
                string chineseDay = GetChineseDayOfWeek(now.DayOfWeek);
                lblScheduleInfo.Text = string.Format("今天是{0}，请选择按哪一天的课表上课", chineseDay);

                // 使用JavaScript显示周末选择框
                ClientScript.RegisterStartupScript(this.GetType(), "ShowWeekendSelector",
                    @"<script type='text/javascript'>document.getElementById('weekendSelector').style.display = 'block';</script>", false);

                return false;
            }
        }
        catch (Exception ex)
        {
            lblScheduleInfo.Text = "智能选择失败：" + ex.Message;
            return false;
        }
    }

    /// <summary>
    /// 周末选择确定按钮点击事件
    /// </summary>
    protected void BtnWeekendSelect_Click(object sender, EventArgs e)
    {
        try
        {
            int selectedDay = Int32.Parse(DDLWeekendDay.SelectedValue);
            DateTime now = DateTime.Now;
            TimeSpan currentTime = now.TimeOfDay;
            LearnSite.BLL.Courses courseBll = new LearnSite.BLL.Courses();
            int teacherId = tcook.Hid;

            // 首先查询所选日是否有课程安排（不考虑时间）
            string dayColumn = "Day" + selectedDay;
            string checkSql = string.Format(@"
                SELECT COUNT(*) FROM kechengbiao
                WHERE TeacherId = @TeacherId
                  AND Year = @Year
                  AND Term = @Term
                  AND {0} != ''", dayColumn);

            System.Data.SqlClient.SqlParameter[] checkParams = new System.Data.SqlClient.SqlParameter[]
            {
                new System.Data.SqlClient.SqlParameter("@TeacherId", System.Data.SqlDbType.Int),
                new System.Data.SqlClient.SqlParameter("@Year", System.Data.SqlDbType.Int),
                new System.Data.SqlClient.SqlParameter("@Term", System.Data.SqlDbType.Int)
            };
            checkParams[0].Value = teacherId;
            checkParams[1].Value = now.Year;
            checkParams[2].Value = LearnSite.Common.XmlHelp.GetIntTerm();

            object countResult = LearnSite.DBUtility.DbHelperSQL.GetSingle(checkSql, checkParams);
            int courseCount = countResult != null ? Convert.ToInt32(countResult) : 0;

            if (courseCount == 0)
            {
                // 所选日没有课程安排，显示所有有课程的日期
                string allDaysSql = @"
                    SELECT DISTINCT
                        CASE
                            WHEN Day1 != '' THEN 1
                            WHEN Day2 != '' THEN 2
                            WHEN Day3 != '' THEN 3
                            WHEN Day4 != '' THEN 4
                            WHEN Day5 != '' THEN 5
                        END as DayNum
                    FROM kechengbiao
                    WHERE TeacherId = @TeacherId
                      AND Year = @Year
                      AND Term = @Term";

                DataTable daysTable = LearnSite.DBUtility.DbHelperSQL.Query(allDaysSql, checkParams).Tables[0];

                string dayNames = "";
                foreach (DataRow row in daysTable.Rows)
                {
                    int dayNum = Convert.ToInt32(row["DayNum"]);
                    if (!string.IsNullOrEmpty(dayNames)) dayNames += "、";
                    dayNames += GetChineseDayName(dayNum);
                }

                if (string.IsNullOrEmpty(dayNames))
                {
                    lblScheduleInfo.Text = "您还没有设置任何课程安排";
                }
                else
                {
                    lblScheduleInfo.Text = string.Format("您在{0}没有课程，有课程的日期是：{1}",
                        GetChineseDayName(selectedDay), dayNames);
                }
                return;
            }

            // 查找当前时间段的课程
            DataTable currentSchedule = courseBll.GetCurrentSchedule(teacherId, currentTime, selectedDay);

            if (currentSchedule.Rows.Count > 0)
            {
                // 找到当前时间段的课程
                DataRow row = currentSchedule.Rows[0];
                string grade = row["Grade"].ToString();
                string classNum = row["Class"].ToString();

                SetGradeAndClass(grade, classNum, "周末课程");
                string dayName = GetChineseDayName(selectedDay);
                lblScheduleInfo.Text = string.Format("按{0}课表，当前时间 {1:HH:mm} 正在上课：{2}年级{3}班", dayName, now, grade, classNum);

                // 隐藏周末选择框
                ClientScript.RegisterStartupScript(this.GetType(), "HideWeekendSelector",
                    @"<script type='text/javascript'>document.getElementById('weekendSelector').style.display = 'none';</script>", false);
            }
            else
            {
                // 查找所有该日的课程（不考虑时间）
                string allCoursesSql = string.Format(@"
                    SELECT TOP 1 k.*,
                           PARSENAME(REPLACE({0}, '.', '.'), 2) as Grade,
                           PARSENAME(REPLACE({0}, '.', '.'), 1) as Class,
                           k.StartTime
                    FROM kechengbiao k
                    WHERE k.TeacherId = @TeacherId
                      AND k.Year = @Year
                      AND k.Term = @Term
                      AND {0} != ''
                    ORDER BY k.StartTime", dayColumn);

                System.Data.SqlClient.SqlParameter[] courseParams = new System.Data.SqlClient.SqlParameter[]
                {
                    new System.Data.SqlClient.SqlParameter("@TeacherId", System.Data.SqlDbType.Int),
                    new System.Data.SqlClient.SqlParameter("@Year", System.Data.SqlDbType.Int),
                    new System.Data.SqlClient.SqlParameter("@Term", System.Data.SqlDbType.Int)
                };
                courseParams[0].Value = teacherId;
                courseParams[1].Value = now.Year;
                courseParams[2].Value = LearnSite.Common.XmlHelp.GetIntTerm();

                DataTable allCourses = LearnSite.DBUtility.DbHelperSQL.Query(allCoursesSql, courseParams).Tables[0];

                if (allCourses.Rows.Count > 0)
                {
                    DataRow row = allCourses.Rows[0];
                    string grade = row["Grade"].ToString();
                    string classNum = row["Class"].ToString();
                    TimeSpan startTime = (TimeSpan)row["StartTime"];

                    SetGradeAndClass(grade, classNum, "周末课程");
                    string dayName = GetChineseDayName(selectedDay);

                    if (currentTime < startTime)
                    {
                        lblScheduleInfo.Text = string.Format("按{0}课表，还未到上课时间。第一节课 {1:hh\\:mm} 开始：{2}年级{3}班", dayName, startTime, grade, classNum);
                    }
                    else
                    {
                        // 找最后一节课
                        DataTable lastCourse = LearnSite.DBUtility.DbHelperSQL.Query(
                            allCoursesSql.Replace("TOP 1", "TOP 1"), courseParams).Tables[0];
                        if (lastCourse.Rows.Count > 0)
                        {
                            lblScheduleInfo.Text = string.Format("按{0}课表，当前时间 {1:HH:mm} 已过所有上课时间。已为您选择：{2}年级{3}班", dayName, now, grade, classNum);
                        }
                    }

                    // 隐藏周末选择框
                    ClientScript.RegisterStartupScript(this.GetType(), "HideWeekendSelector",
                        @"<script type='text/javascript'>document.getElementById('weekendSelector').style.display = 'none';</script>", false);
                }
                else
                {
                    lblScheduleInfo.Text = "所选日没有课程安排";
                }
            }
        }
        catch (Exception ex)
        {
            lblScheduleInfo.Text = "周末选择失败：" + ex.Message;
        }
    }

    /// <summary>
    /// 获取星期几的中文名称（数字转中文）
    /// </summary>
    private string GetChineseDayName(int dayOfWeek)
    {
        switch (dayOfWeek)
        {
            case 1: return "星期一";
            case 2: return "星期二";
            case 3: return "星期三";
            case 4: return "星期四";
            case 5: return "星期五";
            default: return "";
        }
    }

    /// <summary>
    /// 设置年级和班级下拉列表
    /// </summary>
    private void SetGradeAndClass(string grade, string classNum, string source)
    {
        try
        {
            LearnSite.BLL.Room room = new LearnSite.BLL.Room();
            string Hid = tcook.Hid.ToString();

            // 填充年级下拉列表
            DDLgrade.DataSource = room.GetGrade(Int32.Parse(Hid));
            DDLgrade.DataTextField = "Rgrade";
            DDLgrade.DataValueField = "Rgrade";
            DDLgrade.DataBind();

            // 设置选中的年级
            if (DDLgrade.Items.FindByValue(grade) != null)
            {
                DDLgrade.SelectedValue = grade;
                Session[Hid + "grade"] = grade;

                // 填充班级下拉列表
                int Rgrade = Int32.Parse(grade);
                LearnSite.BLL.Room rm = new LearnSite.BLL.Room();
                DDLclass.DataSource = rm.GetLimitClass(Rgrade);
                DDLclass.DataTextField = "Rclass";
                DDLclass.DataValueField = "Rclass";
                DDLclass.DataBind();

                // 处理班级格式转换：课程表返回"02"，但下拉列表可能是"2"
                string targetClass = classNum;
                bool matchFound = false;
                
                // 尝试直接匹配
                if (DDLclass.Items.FindByValue(classNum) != null)
                {
                    DDLclass.SelectedValue = classNum;
                    Session[Hid + "class"] = classNum;
                    targetClass = classNum;
                    matchFound = true;
                }
                else
                {
                    // 如果直接匹配失败，尝试去掉前导零
                    string classWithoutZero = classNum.TrimStart('0');
                    if (!string.IsNullOrEmpty(classWithoutZero) && DDLclass.Items.FindByValue(classWithoutZero) != null)
                    {
                        DDLclass.SelectedValue = classWithoutZero;
                        Session[Hid + "class"] = classWithoutZero;
                        targetClass = classWithoutZero;
                        matchFound = true;
                    }
                    else
                    {
                        // 如果还是匹配失败，尝试添加前导零
                        string classWithZero = classNum.PadLeft(2, '0');
                        if (DDLclass.Items.FindByValue(classWithZero) != null)
                        {
                            DDLclass.SelectedValue = classWithZero;
                            Session[Hid + "class"] = classWithZero;
                            targetClass = classWithZero;
                            matchFound = true;
                        }
                    }
                }
                
                // 更新课程列表
                showCid();
                
                // 更新显示信息
                if (matchFound)
                {
                    // 成功匹配，更新显示文本
                    lblScheduleInfo.Text = lblScheduleInfo.Text.Replace("：" + grade + "年级" + classNum + "班", 
                        "：" + grade + "年级" + targetClass + "班");
                }
                else
                {
                    // 匹配失败，添加调试信息
                    lblScheduleInfo.Text = lblScheduleInfo.Text + " (班级匹配失败: " + classNum + ")";
                }
            }
            else
            {
                lblScheduleInfo.Text = "年级匹配失败：" + grade;
            }
        }
        catch (Exception ex)
        {
            lblScheduleInfo.Text = "设置班级失败：" + ex.Message;
        }
    }

    /// <summary>
    /// 课前检查开关状态改变事件
    /// </summary>
    protected void CheckBoxPreClassCheck_CheckedChanged(object sender, EventArgs e)
    {
        // 更新website.xml中的课前检查开关状态
        bool isPreClassCheck = CheckBoxPreClassCheck.Checked;
        LearnSite.Common.XmlHelp.SetPreClassCheck(isPreClassCheck.ToString().ToLower());
    }

    /// <summary>
    /// 游戏开关状态改变事件
    /// </summary>
    protected void CheckBoxGames_CheckedChanged(object sender, EventArgs e)
    {
        // 更新website.xml中的游戏功能开关状态
        bool isGamesEnabled = CheckBoxGames.Checked;
        LearnSite.Common.XmlHelp.SetGames(isGamesEnabled.ToString().ToLower());
    }

    /// <summary>
    /// 小组讨论开关状态改变事件
    /// </summary>
    protected void CheckBoxGroupChat_CheckedChanged(object sender, EventArgs e)
    {
        // 更新website.xml中的小组讨论开关状态
        bool isGroupChatEnabled = CheckBoxGroupChat.Checked;
        LearnSite.Common.XmlHelp.SetGroupChat(isGroupChatEnabled.ToString().ToLower());
    }

    /// <summary>
    /// 登记开关状态改变事件
    /// </summary>
    protected void CheckBoxRegister_CheckedChanged(object sender, EventArgs e)
    {
        // 更新website.xml中的登记开关状态，供其他页面使用
        bool isRegisterEnabled = CheckBoxRegister.Checked;
        LearnSite.Common.XmlHelp.SetRegister(isRegisterEnabled.ToString().ToLower());
    }

    /// <summary>
    /// 加载打字宝典启用状态
    /// </summary>
    private void LoadTypingEnabledStatus()
    {
        try
        {
            // 从website.xml中获取打字宝典启用状态
            string typingSetting = LearnSite.Common.XmlHelp.GetTypeName("EnableTyping");
            if (!string.IsNullOrEmpty(typingSetting))
            {
                chkTypingEnabled.Checked = bool.Parse(typingSetting);
            }
            else
            {
                // 默认关闭打字宝典
                chkTypingEnabled.Checked = false;
                LearnSite.Common.XmlHelp.SetTyping("false");
            }
        }
        catch (Exception ex)
        {
            // 出错时默认关闭
            chkTypingEnabled.Checked = false;
        }
    }

    /// <summary>
    /// 加载学习汇总启用状态
    /// </summary>
    private void LoadSummaryEnabledStatus()
    {
        try
        {
            // 从website.xml中获取学习汇总启用状态
            string summarySetting = LearnSite.Common.XmlHelp.GetTypeName("EnableSummary");
            if (!string.IsNullOrEmpty(summarySetting))
            {
                chkSummaryEnabled.Checked = bool.Parse(summarySetting);
            }
            else
            {
                // 默认关闭学习汇总
                chkSummaryEnabled.Checked = false;
                LearnSite.Common.XmlHelp.SetSummary("false");
            }
        }
        catch (Exception ex)
        {
            // 出错时默认关闭
            chkSummaryEnabled.Checked = false;
        }
    }

    /// <summary>
    /// 加载荣誉榜启用状态
    /// </summary>
    private void LoadHonorsEnabledStatus()
    {
        try
        {
            // 从website.xml中获取荣誉榜启用状态
            string honorsSetting = LearnSite.Common.XmlHelp.GetTypeName("EnableHonors");
            if (!string.IsNullOrEmpty(honorsSetting))
            {
                chkHonorsEnabled.Checked = bool.Parse(honorsSetting);
            }
            else
            {
                // 默认关闭荣誉榜
                chkHonorsEnabled.Checked = false;
                LearnSite.Common.XmlHelp.SetHonors("false");
            }
        }
        catch (Exception ex)
        {
            // 出错时默认关闭
            chkHonorsEnabled.Checked = false;
        }
    }

    /// <summary>
    /// 打字宝典开关状态改变事件
    /// </summary>
    protected void chkTypingEnabled_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            // 保存打字宝典启用状态到XML配置文件
            LearnSite.Common.XmlHelp.SetTyping(chkTypingEnabled.Checked.ToString().ToLower());

            // 可以在这里添加其他处理逻辑，比如记录日志
            string status = chkTypingEnabled.Checked ? "启用" : "禁用";
            // LearnSite.Common.LogHelper.WriteLog("教师 " + tcook.Hnick + " " + status + "了打字宝典功能");
        }
        catch (Exception ex)
        {
            // 处理异常
            // LearnSite.Common.LogHelper.WriteLog("设置打字宝典状态失败：" + ex.Message);
        }
    }

    /// <summary>
    /// 学习汇总开关状态改变事件
    /// </summary>
    protected void chkSummaryEnabled_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                // 保存学习汇总启用状态到Application对象，以便所有用户共享
                Application["SummaryEnabled"] = chkSummaryEnabled.Checked;

                // 同时保存到Session中，保持兼容性
                Session["SummaryEnabled"] = chkSummaryEnabled.Checked;

                // 保存到XML配置文件
                LearnSite.Common.XmlHelp.SetSummary(chkSummaryEnabled.Checked.ToString().ToLower());

                // 可以在这里添加其他处理逻辑，比如记录日志
                string status = chkSummaryEnabled.Checked ? "启用" : "禁用";
                // LearnSite.Common.LogHelper.WriteLog("教师 " + tcook.Hnick + " " + status + "了学习汇总功能");
            }
            catch (Exception ex)
            {
                // 处理异常
                // LearnSite.Common.LogHelper.WriteLog("设置学习汇总状态失败：" + ex.Message);
            }
        }

    /// <summary>
    /// 荣誉榜开关状态改变事件
    /// </summary>
    protected void chkHonorsEnabled_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                // 保存荣誉榜启用状态到Application对象，以便所有用户共享
                Application["HonorsEnabled"] = chkHonorsEnabled.Checked;

                // 同时保存到Session中，保持兼容性
                Session["HonorsEnabled"] = chkHonorsEnabled.Checked;

                // 保存到XML配置文件
                LearnSite.Common.XmlHelp.SetHonors(chkHonorsEnabled.Checked.ToString().ToLower());

                // 可以在这里添加其他处理逻辑，比如记录日志
                string status = chkHonorsEnabled.Checked ? "启用" : "禁用";
                // LearnSite.Common.LogHelper.WriteLog("教师 " + tcook.Hnick + " " + status + "了荣誉榜功能");
            }
        catch (Exception ex)
        {
            // 处理异常
            // LearnSite.Common.LogHelper.WriteLog("设置荣誉榜状态失败：" + ex.Message);
        }
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
}
