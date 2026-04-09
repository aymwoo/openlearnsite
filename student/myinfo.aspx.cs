using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Student_myinfo : System.Web.UI.Page
{
    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
    protected void Page_Load(object sender, EventArgs e)
    {
        // Handle logout action
        if (Request.QueryString["action"] == "logout")
        {
            if (LearnSite.Common.CookieHelp.IsStudentLogin())
            {
                string mysnum = cook.Snum;
                LearnSite.Common.App.AppUserRemove(mysnum);
            }
            LearnSite.Common.CookieHelp.ClearStudentCookies();
            Session.Abandon();
            Session.Clear();
            Response.Redirect("~/index.aspx", false);
            return;
        }

        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            LearnSite.Common.CookieHelp.KickStudent();
            if (!IsPostBack)
            {
                ShowStudent();
                ShowOnline();
                ShowSelf();
                getoldCids();
                shownew();
                showold();
            }
        }
        else
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }
    }
    private void getoldCids()
    {
        int Cterm = cook.ThisTerm;
        int Cgrade = cook.Sgrade;
        string mysnum = cook.Snum;
        string mySid = cook.Sid.ToString();
        LearnSite.BLL.SurveyFeedback fbll = new LearnSite.BLL.SurveyFeedback();
        string fcids = fbll.ShowStuFeedbackCids(mysnum, Cterm, Cgrade);
        LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
        string wcids = wbll.ShowStuDoneWorkCids(mysnum, Cterm, Cgrade);
        LearnSite.BLL.TopicReply tbll = new LearnSite.BLL.TopicReply();
        string tcids = tbll.ShowStuDoneReplyCids(mysnum, Cterm, Cgrade);
        LearnSite.BLL.TxtFormBack mbll = new LearnSite.BLL.TxtFormBack();
        string mcids = mbll.ShowStuDoneBackCids(mysnum, Cterm, Cgrade);
        LearnSite.BLL.Solves vbll = new LearnSite.BLL.Solves();
        string vcids = vbll.ShowStuDoneSovleCids(mySid, Cterm.ToString(), Cgrade.ToString());

        string rrr = cook.Syear.ToString() + cook.Sgrade.ToString() + cook.Sclass.ToString();
        int simiSid = 0 - Int32.Parse(rrr);//将入学年度和班级号作为模拟学生的ID
        LearnSite.BLL.MenuWorks kbll = new LearnSite.BLL.MenuWorks();
        string kcids = kbll.readCids(simiSid);
        string rcids = kbll.readCids(Int32.Parse(mySid));//当前学生完成的阅读任务

        string allcids = "";
        if (wcids != "")
            allcids = allcids + wcids;
        if (fcids != "")
            allcids = allcids + fcids;
        if (tcids != "")
            allcids = allcids + tcids;
        if (mcids != "")
            allcids = allcids + mcids;
        if (vcids != "")
            allcids = allcids + vcids;
        if (kcids != "")
            allcids = allcids + kcids;
        if (rcids != "")
            allcids = allcids + rcids;


        LabelCids.Text = LearnSite.Common.WordProcess.SimpleWordsNew(allcids);
    }
    private void shownew()
    {
        int Cterm = Int32.Parse(LearnSite.Common.XmlHelp.GetTerm());
        string Cnum = cook.Snum;
        int Cgrade = cook.Sgrade;
        int Chid = cook.Rhid;
        LearnSite.BLL.Courses cs = new LearnSite.BLL.Courses();
        GridViewnewkc.DataSource = cs.ShowNewCourseNew(Cgrade, Cterm, Chid, LabelCids.Text);
        GridViewnewkc.DataBind();
    }
    private void showold()
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            LearnSite.BLL.Courses cs = new LearnSite.BLL.Courses();
            GridViewdonekc.DataSource = cs.ShowDoneCourseNew(LabelCids.Text);
            GridViewdonekc.DataBind();
        }
    }
    protected void GridViewnewkc_RowDataBound(object sender, GridViewRowEventArgs e)
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

    protected void GridViewnewkc_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView theGrid = sender as GridView;  // refer to the GridView
        int newPageIndex = 0;

        if (-2 == e.NewPageIndex)
        { // when click the "GO" Button
            TextBox txtNewPageIndex = null;

            GridViewRow pagerRow = theGrid.BottomPagerRow;

            if (null != pagerRow)
            {
                txtNewPageIndex = pagerRow.FindControl("txtNewPageIndex") as TextBox;   // refer to the TextBox with the NewPageIndex value
            }

            if (null != txtNewPageIndex)
            {

                newPageIndex = int.Parse(txtNewPageIndex.Text) - 1; // get the NewPageIndex
            }
        }
        else
        {  // when click the first, last, previous and next Button
            newPageIndex = e.NewPageIndex;
        }

        // check to prevent form the NewPageIndex out of the range
        newPageIndex = newPageIndex < 0 ? 0 : newPageIndex;
        newPageIndex = newPageIndex >= theGrid.PageCount ? theGrid.PageCount - 1 : newPageIndex;
        theGrid.PageIndex = newPageIndex;
        shownew();
    }
    protected void GridViewdonekc_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView theGrid = sender as GridView;  // refer to the GridView
        int newPageIndex = 0;

        if (-2 == e.NewPageIndex)
        { // when click the "GO" Button
            TextBox txtNewPageIndex = null;

            GridViewRow pagerRow = theGrid.BottomPagerRow;

            if (null != pagerRow)
            {
                txtNewPageIndex = pagerRow.FindControl("txtNewPageIndex") as TextBox;   // refer to the TextBox with the NewPageIndex value
            }

            if (null != txtNewPageIndex)
            {

                newPageIndex = int.Parse(txtNewPageIndex.Text) - 1; // get the NewPageIndex
            }
        }
        else
        {  // when click the first, last, previous and next Button
            newPageIndex = e.NewPageIndex;
        }

        // check to prevent form the NewPageIndex out of the range
        newPageIndex = newPageIndex < 0 ? 0 : newPageIndex;
        newPageIndex = newPageIndex >= theGrid.PageCount ? theGrid.PageCount - 1 : newPageIndex;
        theGrid.PageIndex = newPageIndex;
        showold();
    }
    protected void GridViewdonekc_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex > -1)
        {
            DataKey datakey = GridViewdonekc.DataKeys[e.Row.RowIndex];
            string myCid = datakey[0].ToString();
            Literal ps = (Literal)e.Row.FindControl("Process");
            LearnSite.BLL.Courses cbll = new LearnSite.BLL.Courses();

            if (!String.IsNullOrEmpty(myCid))
            {
                int[] score = cbll.Workrecord(myCid);//获取任务完成得分，未完成为-1
                string worknone = "<span style='display:inline-block;background-color:#E8E8E8;height:20px;width:20px; margin: 1px; text-align: center;' title='未完成'></span>";
                string workdone = "<span style='display:inline-block;background-color:#B4E7B4;height:20px;width:20px;margin: 1px; text-align: center;' title='已完成'></span>";
                string workgood = "<span style='display:inline-block;background-color:#5EA2EC;height:20px;width:20px;margin: 1px; text-align: center;' title='优秀'></span>";
                string workok = "<span style='display:inline-block;background-color:#A894DC;height:20px;width:20px;margin: 1px; text-align: center;' title='收藏'></span>";
                int count = score.Length;
                if (count > 0)
                {
                    string psstr = "";
                    int done = 0;
                    for (int i = 0; i < count; i++)
                    {
                        int s = score[i];
                        if (s < 0)
                        {
                            psstr = psstr + worknone;
                        }
                        else
                        {
                            done++;
                            if (s == 12)
                            {
                                psstr = psstr + workok;
                            }
                                else
                            {
                                if (s == 10)
                                {
                                    psstr = psstr + workgood;
                                }
                                else
                                {
                                    psstr = psstr + workdone;
                                }
                            }
                        }
                    }
                   // Decimal perdone = Convert.ToDecimal(done) / Convert.ToDecimal(count);
                    ps.Text = psstr;// +perdone.ToString("P0");
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //当鼠标放上去的时候 先保存当前行的背景颜色 并给附一颜色 
            e.Row.Attributes.Add("onmouseover", "currentcolor=this.style.backgroundColor;this.style.backgroundColor='transparent',this.style.fontWeight='';");
            //当鼠标离开的时候 将背景颜色还原的以前的颜色 
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=currentcolor,this.style.fontWeight='';");
        }
    }
    private void ShowSelf()
    {
        int loginm = LearnSite.Common.XmlHelp.LoginMode();//获取登录方式： 0 表示个人密码方式登录  1 表示班级密码方式登录
        if (loginm == 0)
        {
            BtnExit.CssClass = "buttonimg";
            BtnExit.Text = "平台退出";
        }
        else
        {
            BtnExit.CssClass = "buttonnone";
        }
    }

    private void ShowOnline()
    {
        DateTime today=DateTime.Now;
        int Qyear=today.Year;
        int Qmonth=today.Month;
        int Qday=today.Day;

        LearnSite.BLL.Signin sg = new LearnSite.BLL.Signin();
        DataListonline.DataSource = sg.OnlineToday(cook.Sgrade, cook.Sclass, Qyear, Qmonth, Qday);
        DataListonline.DataBind();
    }

    private void ShowStudent()
    {
        string mysnum = cook.Snum;
        int mySid = cook.Sid;
        snum.Text = mysnum;
        string Sgrade = cook.Sgrade.ToString();
        string Sclass = cook.Sclass.ToString();
        sclass.Text = Sgrade + "." + Sclass + "班";
        sname.Text = Server.UrlDecode(cook.Sname);

        // 获取当天表现分
        LearnSite.BLL.Signin sgll = new LearnSite.BLL.Signin();
        int todayAttitude = sgll.GetTodayAttitudeBySnum(mysnum);
        lblTodayAttitude.Text = todayAttitude.ToString();

        // 获取最新表现评语
        string latestAttitudeNote = sgll.GetLatestAttitudeNote(mysnum);
        LabelAttitudeNote.Text = latestAttitudeNote;

        // 从数据库实时获取最新学分和表现分
        LearnSite.BLL.Students dbll = new LearnSite.BLL.Students();
        DataSet ds = dbll.GetList("Sid=" + mySid);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            int dbSscore = dr["Sscore"] != DBNull.Value ? Convert.ToInt32(dr["Sscore"]) : 0;
            int dbSattitude = dr["Sattitude"] != DBNull.Value ? Convert.ToInt32(dr["Sattitude"]) : 0;
            int totalScore = dbSscore + dbSattitude; // 总分 = 学分 + 表现分

            sscore.Text = dbSscore.ToString();
            sattitude.Text = dbSattitude.ToString();
            lblTotalScore.Text = totalScore.ToString(); // 显示总分

            // 更新Cook对象中的分数并重新写入Cookie
            cook.Sscore = dbSscore;
            cook.Sattitude = dbSattitude;

            // 重新写入Cookie
            System.Web.HttpCookie StuCookie = new System.Web.HttpCookie(LearnSite.Common.CookieHelp.stuCookieNname);
            StuCookie.Value = cook.ToStr();
            string str = LearnSite.Common.XmlHelp.GetStudentCookiesPeriod();
            if (str == "1")
            {
                StuCookie.Expires = DateTime.Now.AddHours(1);
            }
            else if (str == "2")
            {
                StuCookie.Expires = DateTime.Now.AddHours(2);
            }
            else if (str == "3")
            {
                StuCookie.Expires = DateTime.Now.AddHours(3);
            }
            else if (str == "4")
            {
                StuCookie.Expires = DateTime.Now.AddHours(12);
            }
            else if (str == "5")
            {
                StuCookie.Expires = DateTime.Now.AddDays(1);
            }
            StuCookie.Path = "/";
            StuCookie.HttpOnly = true;
            StuCookie.Secure = Request.IsSecureConnection;
            Response.AppendCookie(StuCookie);
        }
        else
        {
            // 如果数据库查询失败，使用Cookie中的值
            sscore.Text = cook.Sscore.ToString();
            sattitude.Text = cook.Sattitude.ToString();
        }

        string ssex = Server.UrlDecode(cook.Sex);
        int Sterm = cook.ThisTerm;
        LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
        string[] tem = wbll.ShowLastWorkSelf(mySid);//2012-12-14修
        string mywid = tem[0];
        string myself = tem[1];
        if (mywid != "" && myself != "")
        {
            LabelWself.Text = HttpUtility.HtmlDecode(myself);
            Hlwork.NavigateUrl = "javascript:showWorkModal('" + mywid + "');";
            Hlwork.Visible = true;
        }
        else
        {
            Hlwork.Visible = false;
        }
        int Sgroup = dbll.GetSgroup(mySid);
        string Sgtitle = dbll.GetMySgtitle(mySid);//根据自己的组号，获取小组名称
        Labelteam.Text = dbll.GroupTeam(cook.Sgrade, cook.Sclass, Sgroup);

        // 获取学生座位信息
        string mySeat = dbll.GetActualSeat(cook.Snum);
        if (string.IsNullOrEmpty(mySeat))
        {
            // 如果没有固定座位，尝试获取当天签到机号
            mySeat = dbll.GetTodayMachine(cook.Snum);
        }
        Labelseat.Text = mySeat;

        if (Sgtitle != "")
        {
            HLgroup.Text = Server.UrlDecode(Sgtitle);
            HLgroup.NavigateUrl = "javascript:showGroupModal();";
        }
        else
        {
            HLgroup.Text = "申请组队";
            HLgroup.NavigateUrl = "javascript:showGroupModal();";
        }
        string murl = LearnSite.Common.Photo.GetStudentPhotoUrl(snum.Text, ssex);
        Imageface.ImageUrl = murl + "?temp=" + DateTime.Now.Millisecond.ToString();

        LearnSite.BLL.GroupWork gwbll = new LearnSite.BLL.GroupWork();
        int groupscore = gwbll.GetGscoreAll(Sgroup, Int32.Parse(Sgrade), Int32.Parse(Sclass), Sterm);
        string myrank = Server.UrlDecode(cook.RankImage);
        string grouprank = LearnSite.Common.Rank.RankImage(groupscore, false);
        LabelRank.Text = myrank + "<br/>" + grouprank;
        string mytip = "你的学分等级为：" + cook.Sscore / 3 + "级";
        string grouptip = "你的小组等级为：" + groupscore / 3 + "级";
        LabelRank.ToolTip = mytip + " \r " + grouptip;//tooltip换行原来是\r
    }
    protected void BtnExit_Click(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            string mysnum = cook.Snum;
            LearnSite.Common.App.AppUserRemove(mysnum);//移除网站全局变量列表中该用户名

            LearnSite.Common.CookieHelp.ClearStudentCookies();
            Session.Abandon();//取消当前会话   
            Session.Clear();//清除当前浏览器进程所有session 
            Request.Cookies.Clear();
            System.Threading.Thread.Sleep(200);
            string rurl = "~/index.aspx?qt=" + DateTime.Now.Millisecond.ToString();
            Response.Redirect(rurl, false);
        }
    }
    protected void BtnProfile_Click(object sender, EventArgs e)
    {
        // Now handled by client-side javascript Modal
        // string url = "~/profile/mygroup.aspx";
        // Response.Redirect(url, false);
    }
    protected void DataListonline_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        string sleader = ((Label)e.Item.FindControl("LabelSleader")).Text.ToLower();
        string sgroup = ((Label)e.Item.FindControl("LabelSgroup")).Text;
        string qnum = ((Label)e.Item.FindControl("LabelQnum")).Text;
        HyperLink hl = (HyperLink)e.Item.FindControl("HyperQname");
        hl.NavigateUrl = "javascript:showPortfolioModal('" + qnum + "');"; //本学期作品浏览 Modal 
        string vpath = "~/images/gcard.gif";
        Image imga = new Image();
        imga = (Image)e.Item.FindControl("Imageflag");
        if (sleader == "true")
        {
            vpath = "~/images/gflag.gif";//如果是组长的话,换图标
            imga.ToolTip = sgroup + "小组组长";
        }
        else
        {
            if (sgroup == "")
            {
                imga.ToolTip = "未分组";
                vpath = "~/images/ncard.gif";//如果未分组,换图标
            }
            else
            {
                imga.ToolTip = sgroup + "小组成员";
            }
        }
        imga.ImageUrl = vpath + "?temp=" + DateTime.Now.Millisecond.ToString();
    }
}
