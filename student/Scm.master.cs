using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Student_Scm : System.Web.UI.MasterPage
{
    protected string Cbanner = "";
    protected string SiteTitle = "";
    // 学习状态上报所需的学生信息
    protected string LsSnum = "";
    protected string LsSname = "";
    protected string LsSgrade = "0";
    protected string LsSclass = "0";
    protected string LsSid = "0";
    protected string LsCid = "0";
    protected string LsLid = "0";
    protected string LsLtitle = "";
    protected string LsLtype = "";
    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
    protected void Page_Load(object sender, EventArgs e)
    {
        SiteTitle = LearnSite.Common.XmlHelp.SiteTitle();
        // 初始化学生学习状态上报信息
        if (cook.IsExist())
        {
            LsSnum = cook.Snum;
            LsSname = cook.Sname;
            LsSgrade = cook.Sgrade.ToString();
            LsSclass = cook.Sclass.ToString();
            LsSid = cook.Sid.ToString();
        }
        if (!IsPostBack)
        {
            ShowListMenu();
        }
    }

    private void AddLessonFirst(string CurWay, string Cid, string Ctitle)
    {
        MenuItem mic = new MenuItem();
        mic.Text = Ctitle;
        mic.ImageUrl = "~/images/home.gif";
        mic.SeparatorImageUrl = "../images/separate.gif";
        mic.NavigateUrl = "~/student/showcourse.aspx?cid=" + Cid;
        Menuact.Items.Add(mic);//添加本课导学菜单
    }

    private void AddReturn()
    {
        MenuItem ms = new MenuItem();
        ms.Text = "返回";
        ms.ImageUrl = "~/images/return.png";
        ms.NavigateUrl = "~/student/myinfo.aspx";
        Menuact.Items.Add(ms);
    }

    private void ShowListMenu()
    {
        string myCid = "";
        string Lidstr = "";
        if (Request.QueryString["cid"] != null)
        {
            myCid = Request.QueryString["cid"].ToString();
        }
        else
        {
            if (Request.QueryString["lid"] != null)
            {
                Lidstr = Request.QueryString["lid"].ToString();
                if (LearnSite.Common.WordProcess.IsNum(Lidstr))
                {
                    LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
                    LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
                    lmodel = lbll.GetModel(Int32.Parse(Lidstr));
                    if (lmodel != null)
                    {
                        myCid = lmodel.Lcid.ToString();
                    }
                }
            }
        }
        if (LearnSite.Common.WordProcess.IsNum(myCid))
        {
            // 设置当前课程ID，供学习状态上报使用
            LsCid = myCid;

            string Uploadmode = LearnSite.Common.XmlHelp.GetUploadMode();
            string mUrl;
            switch (Uploadmode)
            {
                case "0":
                    mUrl = "mission";
                    break;
                case "1":
                    mUrl = "task";
                    break;
                default:
                    mUrl = "task";
                    break;
            }

            string CurWay = "";
            LearnSite.BLL.Courses cbll = new LearnSite.BLL.Courses();
            Cbanner = cbll.GetBanner(Int32.Parse(myCid)).Replace("~", "../..");
            string Ctitle = " 首页 "; 
            AddLessonFirst(CurWay, myCid.ToString(), Ctitle);
            LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
            DataTable dt = lbll.GetShowedMenu(Int32.Parse(myCid)).Tables[0];
            int dcount = dt.Rows.Count;

            if (dcount > 0)
            {
                string sepUrl = "../images/separate.gif";
                string urlarrow = "~/images/arrow.png";
                string urllocker = "~/images/locker.png";
                string urlfinish = "~/images/finish.png";
                LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
                bool ispass = false;
                LearnSite.BLL.Room rbll = new LearnSite.BLL.Room();
                ispass = rbll.GetRpass(cook.Sgrade, cook.Sclass);
                int lcount = dcount;
                if (ispass)
                {
                    LearnSite.BLL.MenuWorks kbll = new LearnSite.BLL.MenuWorks();
                    string lidall = "";
                    for (int i = 0; i < dcount; i++)
                    {
                        string Lid = dt.Rows[i]["lid"].ToString();
                        lidall = lidall + "'" + Lid + "'";
                        if (i < dcount - 1)
                            lidall = lidall + ",";
                    }
                    lcount = kbll.GetMyLidCount(cook.Sid, lidall);
                }
                for (int i = 0; i < dcount; i++)
                {
                    string Lid = dt.Rows[i]["lid"].ToString();
                    string Lsort = dt.Rows[i]["Lsort"].ToString();
                    string Ltype = dt.Rows[i]["Ltype"].ToString();
                    string Lxidstr = dt.Rows[i]["Lxid"].ToString();
                    string Ltitlestr = dt.Rows[i]["Ltitle"].ToString();

                    MenuItem ma = new MenuItem();
                    ma.Text = Ltitlestr;
                    ma.SeparatorImageUrl = sepUrl;
                    ma.ImageUrl = urlarrow;

                    switch (Ltype)
                    {
                        case "1":
                            ma.ImageUrl = "~/images/mission.png";
                            ma.NavigateUrl = "~/student/show" + mUrl + ".aspx?lid=" + Lid;
                            break;
                        case "2":
                            ma.ImageUrl = "~/images/survey.png";
                            ma.NavigateUrl = "~/student/myexam.aspx?lid=" + Lid;
                            break;
                        case "3":
                            ma.ImageUrl = "~/images/topic.png";
                            ma.NavigateUrl = "~/student/topicdiscuss.aspx?lid=" + Lid;
                            break;
                        case "4":
                            ma.ImageUrl = "~/images/inquiry.png";
                            ma.NavigateUrl = "~/student/txtform.aspx?lid=" + Lid;
                            break;
                        case "5":
                            ma.ImageUrl = "~/images/program.png";
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "6":
                            ma.ImageUrl = "~/images/description.png";
                            ma.NavigateUrl = "~/student/description.aspx?lid=" + Lid;
                            break;
                        case "8":
                            ma.ImageUrl = "~/images/python.png";
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "9":
                            ma.ImageUrl = "~/images/console.png";
                            ma.NavigateUrl = "~/student/console.aspx?lid=" + Lid;
                            break;
                        case "10":
                            ma.ImageUrl = "~/images/mxgraph.png";
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "11"://像素画
                            ApplyCustomActivityVisual(ma, "11");
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "12":
                            ma.ImageUrl = "~/images/html.png";
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "13":
                            ma.ImageUrl = "~/images/pythonblock.png";
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "14":
                            ma.ImageUrl = "~/images/blockpy.png";
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "15":
                            ma.ImageUrl = "~/images/kitymind.png";
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "16":
                            ma.ImageUrl = "~/images/excel.png";
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "17"://二维码
                            ApplyCustomActivityVisual(ma, "17");
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "18"://在线文档
                            ApplyCustomActivityVisual(ma, "18");
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "19"://演示文稿
                            ApplyCustomActivityVisual(ma, "19");
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "20"://海报设计
                            ApplyCustomActivityVisual(ma, "20");
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "21"://风格迁移
                            ApplyCustomActivityVisual(ma, "21");
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "22"://图像分类
                            ApplyCustomActivityVisual(ma, "22");
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "23"://人脸识别
                            ApplyCustomActivityVisual(ma, "23");
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "24"://物联网mqtt
                            ApplyCustomActivityVisual(ma, "24");
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "25"://手绘画布
                            ApplyCustomActivityVisual(ma, "25");
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "26"://推箱子地图
                            ApplyCustomActivityVisual(ma, "26");
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "27"://人工智能对话
                            ApplyCustomActivityVisual(ma, "27");
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "28"://语音合成
                            ApplyCustomActivityVisual(ma, "28");
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "29"://文字识别
                            ApplyCustomActivityVisual(ma, "29");
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "30"://声音分析
                            ApplyCustomActivityVisual(ma, "30");
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "31"://井字棋
                            ApplyCustomActivityVisual(ma, "31");
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "32"://手写数字识别
                            ApplyCustomActivityVisual(ma, "32");
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "33"://Markdown写作
                            ApplyCustomActivityVisual(ma, "33");
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "34"://iframe嵌入网页
                            ApplyCustomActivityVisual(ma, "34");
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "35"://文生图
                            ApplyCustomActivityVisual(ma, "35");
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "36"://素材库
                            ApplyCustomActivityVisual(ma, "36");
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "37"://网站设计
                            ApplyCustomActivityVisual(ma, "37");
                            ma.NavigateUrl = "~/student/program.aspx?lid=" + Lid;
                            break;
                        case "38":
                            ma.ImageUrl = "~/images/ware.png";
                            ma.NavigateUrl = "~/student/ware.aspx?lid=" + Lid;
                            break;
                        case "39":
                            ma.ImageUrl = "~/images/wvote.png";
                            ma.NavigateUrl = "~/webform/preview.aspx?lid=" + Lid;
                            break;
                    }
                    if (ispass)
                    {
                        if (i < lcount)
                        {
                            if (LearnSite.Common.WordProcess.IsNum(Lxidstr))
                            {
                                bool codepass = wbll.WorkPass(cook.Sid, Int32.Parse(Lxidstr));
                                if(codepass)
                                    ma.ImageUrl = urlfinish;
                            }
                        }
                        if (i > lcount)
                        {
                            ma.ImageUrl = urllocker;
                            if (cook.Sid > 0)
                                ma.NavigateUrl = "#";
                        }
                    }
                    if (Lidstr == Lid)
                    {
                        CurWay = Ltitlestr;
                        ma.Selected = true;
                        // 设置当前学案环节信息，供学习状态上报使用
                        LsCid = myCid;
                        LsLid = Lid;
                        LsLtitle = Ltitlestr;
                        LsLtype = Ltype;
                    }
                    Menuact.Items.Add(ma);
                }
            }
            dt.Dispose();
            AddReturn();
            
            // 添加学习汇总和荣誉榜菜单项
            AddSummaryAndHonorsMenuItems();
            
            int timepass = LearnSite.Common.Computer.TimePassed();
            this.Page.Title = HttpUtility.UrlDecode(cook.Sname) + " " + cook.Snum + " (" + timepass + "分钟)";
        }
    }
    
    /// <summary>
    /// 添加学习汇总和荣誉榜菜单项
    /// </summary>
    private void AddSummaryAndHonorsMenuItems()
    {
        // 检查学习汇总开关状态
        bool summaryEnabled = false;
        if (Application["SummaryEnabled"] != null)
        {
            summaryEnabled = Convert.ToBoolean(Application["SummaryEnabled"]);
        }
        else if (Session["SummaryEnabled"] != null)
        {
            summaryEnabled = Convert.ToBoolean(Session["SummaryEnabled"]);
        }
        else
        {
            // 从XML配置文件读取
            string summarySetting = LearnSite.Common.XmlHelp.GetTypeName("EnableSummary");
            if (!string.IsNullOrEmpty(summarySetting))
            {
                summaryEnabled = Convert.ToBoolean(summarySetting);
            }
        }
        
        // 检查荣誉榜开关状态
        bool honorsEnabled = false;
        if (Application["HonorsEnabled"] != null)
        {
            honorsEnabled = Convert.ToBoolean(Application["HonorsEnabled"]);
        }
        else if (Session["HonorsEnabled"] != null)
        {
            honorsEnabled = Convert.ToBoolean(Session["HonorsEnabled"]);
        }
        else
        {
            // 从XML配置文件读取
            string honorsSetting = LearnSite.Common.XmlHelp.GetTypeName("EnableHonors");
            if (!string.IsNullOrEmpty(honorsSetting))
            {
                honorsEnabled = Convert.ToBoolean(honorsSetting);
            }
        }
        
        // 添加学习汇总菜单项
        if (summaryEnabled)
        {
            MenuItem summaryItem = new MenuItem();
            summaryItem.Text = "学习汇总";
            summaryItem.ImageUrl = "~/images/summary.png";
            summaryItem.SeparatorImageUrl = "../images/separate.gif";
            summaryItem.NavigateUrl = "~/student/mytotal.aspx";
            Menuact.Items.Add(summaryItem);
        }
        
        // 添加荣誉榜菜单项
        if (honorsEnabled)
        {
            MenuItem honorsItem = new MenuItem();
            honorsItem.Text = "荣誉榜";
            honorsItem.ImageUrl = "~/images/honors.png";
            honorsItem.SeparatorImageUrl = "../images/separate.gif";
            honorsItem.NavigateUrl = "~/student/honorboard.aspx";
            Menuact.Items.Add(honorsItem);
        }
    }

    private void ApplyCustomActivityVisual(MenuItem menuItem, string category)
    {
        LearnSite.Common.CustomActivityMeta meta = LearnSite.Common.CustomActivityCatalog.GetMeta(category);
        menuItem.ImageUrl = meta.IconUrl;
    }
}
