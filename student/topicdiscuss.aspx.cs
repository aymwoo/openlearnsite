using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_topicdiscuss : System.Web.UI.Page
{
    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
    protected string myCid;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            LearnSite.Common.CookieHelp.KickStudent();

            if (!IsPostBack)
            {
                showtopic();
                showreply();
            }
        }
        else
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }
    }
    private void showreply()
    {
        int sgrade = cook.Sgrade;
        int sclass = cook.Sclass;
        string rsnum = cook.Snum;
        int rsid = cook.Sid;
        LearnSite.BLL.TopicReply rbll = new LearnSite.BLL.TopicReply();
        if (isTeacher())
        {
            GVtopicDiscuss.DataSource = rbll.GetClassList(sgrade, sclass, Int32.Parse(LabelTid.Text));
            GVtopicDiscuss.DataBind();
            Labelreplycount.Text = "共" + GVtopicDiscuss.Rows.Count.ToString() + "贴";
            Labelreplycountbtm.Text = Labelreplycount.Text;
        }
        else
        {
            if (rbll.ReplyCount(Int32.Parse(LabelTid.Text), rsid) > 0)
            {
                GVtopicDiscuss.DataSource = rbll.GetClassList(sgrade, sclass, Int32.Parse(LabelTid.Text));
                GVtopicDiscuss.DataBind();
                Labelreplycount.Text = "共" + GVtopicDiscuss.Rows.Count.ToString() + "贴";
                Labelreplycountbtm.Text = Labelreplycount.Text;
            }
        }
        Labelnostu.Text = rbll.GetNoReplay(sgrade, sclass, Int32.Parse(LabelTid.Text));
    }
    protected void Btnword_Click(object sender, EventArgs e)
    {
        int rsid = cook.Sid;
        string rsnum = cook.Snum;
        string rip = cook.LoginIp;
        int rgrade = cook.Sgrade;
        int rclass = cook.Sclass;
        int ryear = cook.Syear;
        int rterm = cook.ThisTerm;
        string Wtime = cook.LoginTime;
        DateTime Wdate = DateTime.Now;
        int Tid = Int32.Parse(LabelTid.Text);
        int Cid = Int32.Parse(LabelCid.Text);
        int Lid = Int32.Parse(LabelLid.Text);

        string rwords = Request.Form["textareaWord"].Trim();
        string tempcut = LearnSite.Common.WordProcess.DropHTML(rwords);
        int counts = tempcut.Length;
        int rscore = 2;
        bool rban = false;

        if (counts > 1 && counts < 600)
        {
            //rwords = rwords.Replace("<br /><br />", "<br />");
            //rwords = rwords.Replace(",", "，");

            LearnSite.BLL.TopicReply rbll = new LearnSite.BLL.TopicReply();
            LearnSite.Model.TopicReply rmodel = new LearnSite.Model.TopicReply();
            LearnSite.Model.TopicReply rmodelget = new LearnSite.Model.TopicReply();

            rmodel.Rban = rban;
            rmodel.Rip = rip;
            rmodel.Rscore = rscore;
            rmodel.Rsnum = rsnum;
            rmodel.Rtid = Tid;
            rmodel.Rtime = DateTime.Now;
            rmodel.Rwords = HttpUtility.HtmlEncode(rwords);
            rmodel.Rgrade = rgrade;
            rmodel.Rterm = rterm;
            rmodel.Rcid = Cid;
            rmodel.Rclass = rclass;
            rmodel.Rsid = rsid;
            rmodel.Ryear = ryear;
            rmodel.Redit = false;
            rmodel.Ragree = 0;
            rmodel.Rlid = Lid;
            int myreply = rbll.ReplyCount(Tid, rsid);//获取该学号回复数

            //添加课堂活动记录
            LearnSite.Model.MenuWorks kmodel = new LearnSite.Model.MenuWorks();
            kmodel.Klid = Lid;
            kmodel.Ksid = rsid;
            kmodel.Ktime = LearnSite.Common.Computer.GoneMinute(DateTime.Parse(Wtime), Wdate);
            kmodel.Kseconds = Int32.Parse(LearnSite.Common.Computer.Datagone(DateTime.Parse(Wtime), Wdate));
            kmodel.Kcheck = false;
            LearnSite.BLL.MenuWorks kbll = new LearnSite.BLL.MenuWorks();

            if (isTeacher())
            {
                if (myreply > 0)
                {
                    rbll.UpdateTeacher(Tid, rsid, rwords);//更新老师总结
                }
                else
                {
                    rbll.Add(rmodel);//增加老师总结
                    kbll.Add(kmodel);
                    string myurl = "~/student/topicdiscuss.aspx?lid=" + Lid.ToString();
                    Response.Redirect(myurl, false);
                }

                Labeldiscuss.Text = "总结成功！";
            }
            else
            {
                bool historyban = rbll.Isban(Tid, rsid);
                if (!historyban)
                {
                    if (myreply < 1)
                    {
                        rbll.Add(rmodel);//增加一条回复
                        kbll.Add(kmodel);
                        Labeldiscuss.Text = "回复成功！";
                        string myurl = "~/student/topicdiscuss.aspx?lid=" + Lid.ToString();
                        Response.Redirect(myurl);
                    }
                    else
                    {
                        rbll.UpdateOne(rmodel);//修改一条回复
                        Labeldiscuss.Text = "修改成功！";
                    }
                }
                else
                {
                    Labeldiscuss.Text = "你在回复中存在违纪言论已被老师禁言！";
                }
            }
            System.Threading.Thread.Sleep(200);
            showtopic();
            showreply();
        }
        else
        {
            Labeldiscuss.Text = "回复不能少于2个汉字或超过300个汉字，谢谢！";
        }
    }
    private void showtopic()
    {
        string Lid = Request.QueryString["lid"].ToString();
        LabelLid.Text = Lid;
        if (LearnSite.Common.WordProcess.IsNum(Lid))
        {
            LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
            LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
            lmodel = lbll.GetModel(Int32.Parse(Lid));
            LabelCid.Text = lmodel.Lcid.ToString();
            LabelTid.Text = lmodel.Lxid.ToString();
            myCid = lmodel.Lcid.ToString();

            LearnSite.BLL.TopicDiscuss tbll = new LearnSite.BLL.TopicDiscuss();
            LearnSite.Model.TopicDiscuss tmodel = new LearnSite.Model.TopicDiscuss();
            tmodel = tbll.GetModel(lmodel.Lxid.Value);//获取主题讨论实体
            Labeltopic.Text = tmodel.Ttitle;
            TcloseCheck.Checked = tmodel.Tclose;

            Topics.InnerHtml = HttpUtility.HtmlDecode(tmodel.Tcontent);
            LearnSite.BLL.Courses cbll = new LearnSite.BLL.Courses();

            string mynum = cook.Snum;
            int sgrade = cook.Sgrade;
            int sclass = cook.Sclass;
            int syear = cook.Syear;
            int hid = cook.Rhid;

            string teasnum = "s" + hid + syear.ToString() + sgrade.ToString() + sclass.ToString();
            LearnSite.BLL.TopicReply rbll = new LearnSite.BLL.TopicReply();
            string treply = rbll.getteareply(lmodel.Lxid.Value, teasnum);
            TopicsResult.InnerHtml = HttpUtility.HtmlDecode(treply);

            if (mynum == teasnum)
            {
                Btnclock.Enabled = true;
                //Btnword.Text = "老师总结";
                ImageBtngoodall.Visible = true;
                ImageBtngood2.Visible = true;
            }
            if (tmodel.Tclose)
            {
                //Btnword.Enabled = false;//不可发表讨论
                Btnclock.ImageUrl = "~/images/clockred.gif" + "?temp=" + DateTime.Now.Millisecond.ToString();
                Btnclock.ToolTip = "主题讨论暂停！";
                plant.Visible = false;
            }
            else
            {
                plant.Visible = true;
                //Btnword.Enabled = true;//可发表讨论
                Btnclock.ImageUrl = "~/images/clock.gif" + "?temp=" + DateTime.Now.Millisecond.ToString();
                Btnclock.ToolTip = "主题讨论开启!";
            }
        }
    }

    protected void GVtopicDiscuss_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string eid = e.CommandArgument.ToString();
        LearnSite.BLL.TopicReply rbll = new LearnSite.BLL.TopicReply();
        if (e.CommandName == "Good")
        {
            int index = Convert.ToInt32(eid);
            rbll.Updatescore(index);
            System.Threading.Thread.Sleep(200);
            showreply();
        }
        if (e.CommandName == "Less")
        {
            int index = Convert.ToInt32(eid);
            rbll.Lessscore(index);
            System.Threading.Thread.Sleep(200);
            showreply();
        }
        if (e.CommandName == "Reply")
        {
            int index = Convert.ToInt32(eid);
            rbll.UpdateEdit(index);
            System.Threading.Thread.Sleep(200);
            showreply();
        }
        if (e.CommandName == "Del")
        {
            int index = Convert.ToInt32(eid);
            rbll.Delete(index);
            System.Threading.Thread.Sleep(200);
            showreply();
        }
        if (e.CommandName == "Agree")
        {
            int index = Convert.ToInt32(eid);
            string sid = cook.Sid.ToString();
            if (Session["Topic" + sid] != null)
            {
                int agcount = Int32.Parse(Session["Topic" + sid].ToString());
                if (agcount < 10)
                {
                    if (Session["Topic" + sid + "Agree" + index.ToString()] == null)
                    {
                        Session["Topic" + sid + "Agree" + index.ToString()] = "T";
                        Session["Topic" + sid] = agcount + 1;
                        rbll.UpdateAgree(index);
                        System.Threading.Thread.Sleep(200);
                        showreply();
                    }
                    else
                    {
                        LearnSite.Common.WordProcess.Alert("您已经点赞过了！", this.Page);
                    }
                }
                else
                {
                    LearnSite.Common.WordProcess.Alert("最多点赞9次！", this.Page);
                }
            }
            else
            {
                Session["Topic" + sid] = 1;
                rbll.UpdateAgree(index);
                System.Threading.Thread.Sleep(200);
                showreply();
            }
        }
    }
    protected void GVtopicDiscuss_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowIndex > -1)
        {
            Label lb = (Label)e.Row.FindControl("Labelfloor");
            Label lbsc = (Label)e.Row.FindControl("Labelscore");
            Label lbdate = (Label)e.Row.FindControl("Labeldate");
            Label lbagree = (Label)e.Row.FindControl("Labelagree");
            Image imgagree = (Image)e.Row.FindControl("Imageagree");
            Label lbsnum = (Label)e.Row.FindControl("Labelsnum");
            CheckBox ckSleader = (CheckBox)e.Row.FindControl("CheckSleader");
            Image imagegroup = (Image)e.Row.FindControl("Imagegroup");
            Image imagestu = (Image)e.Row.FindControl("Imagestu");
            if (ckSleader.Checked)
            {
                imagegroup.ImageUrl = "~/images/gflag.gif";
            }
            if (lbagree.Text != "")
            {
                int agree = Int32.Parse(lbagree.Text);
                if (agree > 9)
                    imgagree.Visible = true;
            }
            lb.Text = Convert.ToString(e.Row.RowIndex + 1);
            string mynum = cook.Snum;

            imagestu.ImageUrl = LearnSite.Common.Photo.GetStudentPhotoUrl(lbsnum.Text); ;

            int score = Int32.Parse(lbsc.Text);
            if (score > 0)
            {
                Image im = (Image)e.Row.FindControl("Imageflag");
                im.ImageUrl = "~/images/topichot.png";
            }
            ImageButton imbtn = (ImageButton)e.Row.FindControl("ImageButtonDel");
            ImageButton imbtngood = (ImageButton)e.Row.FindControl("ImageButtonGood");
            ImageButton imbtnedit = (ImageButton)e.Row.FindControl("ImageButtonEdit");
            ImageButton imbtnless = (ImageButton)e.Row.FindControl("ImageButtonless");
            CheckBox ckbtn = (CheckBox)e.Row.FindControl("Ckedit");
            if (isTeacher())
            {
                imbtn.Visible = true;
                imbtngood.Visible = true;
                imbtnedit.Visible = false;
                imbtnless.Visible = true;
                imbtnedit.ToolTip = "不允许学生修改！";
                imbtngood.ToolTip = "加分！";
                imbtn.ToolTip = "删除！";
                Label lbsname = (Label)e.Row.FindControl("Labelsname");
                string strdeljs = "if(confirm('您确定删除" + lbsname.Text + "同学帖子吗?'))return true;else return false; ";

                imbtn.OnClientClick = strdeljs;
                if (ckbtn.Checked)
                {
                    imbtnedit.ImageUrl = "~/images/ed.gif";
                    imbtnedit.ToolTip = "允许学生回复修改！";
                }
            }
            else
            {
                imbtn.Visible = false;
                imbtngood.Visible = false;
                imbtnedit.Visible = false;
                imbtnless.Visible = false;
                ImageButton imgbtnagree = (ImageButton)e.Row.FindControl("ImageButtonAgree");
                if (lbsnum.Text == mynum)
                {
                    imgbtnagree.Enabled = false;
                }
            }
        }
    }
    protected void Btnclock_Click(object sender, ImageClickEventArgs e)
    {
        if (isTeacher())
        {
            bool chbtn = false;

            if (TcloseCheck.Checked)
                chbtn = false;//取反
            else
                chbtn = true;

            int tid = Int32.Parse(LabelTid.Text);
            LearnSite.BLL.TopicDiscuss tdbll = new LearnSite.BLL.TopicDiscuss();
            tdbll.UpdateTclose(tid, chbtn);//更新
            System.Threading.Thread.Sleep(200);
            showtopic();
        }
    }
    protected void ImageBtnFresh_Click(object sender, ImageClickEventArgs e)
    {
        showreply();
        //showtopic();
        System.Threading.Thread.Sleep(200);
    }
    protected void ImageBtngoodall_Click(object sender, ImageClickEventArgs e)
    {
        if (isTeacher())
        {
            int sgrade = cook.Sgrade;
            int sclass = cook.Sclass;
            int syear = cook.Syear;
            LearnSite.BLL.TopicReply rbll = new LearnSite.BLL.TopicReply();
            rbll.UpdateAllscore(Int32.Parse(LabelTid.Text), sgrade, sclass, syear);
            System.Threading.Thread.Sleep(200);
            showreply();
        }
    }

    protected bool isTeacher()
    {
        if (cook.Snum.IndexOf("s") > -1)
            return true;
        else
            return false;
    }
    protected void ImageBtngood2_Click(object sender, ImageClickEventArgs e)
    {
        if (isTeacher())
        {
            int sgrade = cook.Sgrade;
            int sclass = cook.Sclass;
            int syear = cook.Syear;
            LearnSite.BLL.TopicReply rbll = new LearnSite.BLL.TopicReply();
            rbll.UpdateAllscore6(Int32.Parse(LabelTid.Text), sgrade, sclass, syear);
            System.Threading.Thread.Sleep(200);
            showreply();
        }

    }
}
