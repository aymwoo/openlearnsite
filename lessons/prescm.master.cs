using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Lessons_prescm : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ShowListMenu();
        }
    }

    private void AddLessonFirst(string CurWay, string Cid)
    {
        MenuItem mic = new MenuItem();
        mic.Text = "本课首页";
        mic.ImageUrl = "~/images/lesson.png";
        mic.SeparatorImageUrl = "~/images/separate.png";
        mic.NavigateUrl = "~/lessons/precourse.aspx?cid=" + Cid;
        Menuact.Items.Add(mic);//添加本课导学菜单
    }

    private void AddReturn()
    {
        MenuItem ms = new MenuItem();
        ms.Text = "返回";
        ms.ImageUrl = "~/images/return.png";
        ms.NavigateUrl = "~/lessons/mylesson.aspx";
        Menuact.Items.Add(ms);
    }

    private void ShowListMenu()
    {
        if (Request.QueryString["cid"] != null)
        {
            string Cid = Request.QueryString["cid"].ToString();
            string Uploadmode = LearnSite.Common.XmlHelp.GetUploadMode();
            if (LearnSite.Common.WordProcess.IsNum(Cid))
            {
                string CurWay = "";
                LearnSite.BLL.Courses cbll = new LearnSite.BLL.Courses();
                string Ctitle = cbll.GetTitle(Int32.Parse(Cid));

                AddLessonFirst(CurWay, Cid);
                LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
                DataTable dt = lbll.GetShowedMenu(Int32.Parse(Cid)).Tables[0];
                int dcount = dt.Rows.Count;
                if (dcount > 0)
                {
                    string myLid = "";
                    if (Request.QueryString["Lid"] != null)
                    {
                        myLid = Request.QueryString["Lid"].ToString();
                    }
                    for (int i = 0; i < dcount; i++)
                    {
                        string Lid = dt.Rows[i]["Lid"].ToString();
                        string Lsort = dt.Rows[i]["Lsort"].ToString();
                        string Ltype = dt.Rows[i]["Ltype"].ToString();
                        string Lxidstr = dt.Rows[i]["Lxid"].ToString();
                        string Ltitlestr = dt.Rows[i]["Ltitle"].ToString();
                        MenuItem ma = new MenuItem();
                        ma.Text = Ltitlestr;
                        ma.SeparatorImageUrl = "~/images/separate.png";
                        switch (Ltype)
                        {
                            case "1"://活动
                                ma.ImageUrl = "~/images/mission.png";
                                ma.NavigateUrl = "~/lessons/premission.aspx?cid=" + Cid + "&Mid=" + Lxidstr + "&Lid=" + Lid + "&Lsort=" + Lsort;
                                break;
                            case "2"://调查
                                ma.ImageUrl = "~/images/survey.png";
                                ma.NavigateUrl = "~/lessons/presurvey.aspx?cid=" + Cid + "&Vid=" + Lxidstr + "&Lid=" + Lid + "&Lsort=" + Lsort;
                                break;
                            case "3"://讨论
                                ma.ImageUrl = "~/images/topic.png";
                                ma.NavigateUrl = "~/lessons/pretopicdiscuss.aspx?cid=" + Cid + "&Tid=" + Lxidstr + "&Lid=" + Lid + "&Lsort=" + Lsort;
                                break;
                        }
                        if (myLid == Lid)
                        {
                            CurWay = Ltitlestr;
                            ma.Selected = true;
                        }
                        Menuact.Items.Add(ma);//添加活动菜单
                    }
                }
                dt.Dispose();
                AddReturn();
                this.Page.Title = Ctitle + "—>" + CurWay;
            }
        }
    }
}
