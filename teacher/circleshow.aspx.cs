using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
public partial class Teacher_circleshow : System.Web.UI.Page
{
    protected string url = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.JudgeTeacherCookies();
        Btnflash.Attributes.Add("onclick", "this.form.target='_self'");
        ImageBtnDel.Attributes["OnClick"] = "return confirm('您确定要删除该作品吗？');";
        if (!IsPostBack)
        {
            ReadMtitle();
            Readwork();
            showflash();
        }
    }
    private void showflash()
    {
        DDLname.SelectedIndex = 0;
        RBLselect.ClearSelection();
        int icn = DDLstore.Items.Count;
        if (icn > 0)
        {
            string Wid = DDLstore.SelectedValue;
            if (!string.IsNullOrEmpty(Wid))
            {
                LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
                LearnSite.Model.Works wmodel = new LearnSite.Model.Works();
                wmodel = wbll.GetModel(Int32.Parse(Wid));

                url = Server.UrlEncode(wmodel.Wurl);
                string ext = wmodel.Wtype;
                int cur = DDLstore.SelectedIndex + 1;
                Labelnum.Text = cur.ToString() + "/" + icn.ToString();

                string Wnum = wmodel.Wnum;
                Labelnum.ToolTip = Wnum;
                
                Labelname.Text = Server.UrlDecode(wmodel.Wname);

                GetScore(Wnum);

                Literal1.Text = LearnSite.Common.ViewPage.SelectWritePlugin(Wid, ext, url, wmodel.Wcode, wmodel.Wthumbnail, false, true,Wnum);

            }
            else
            {
                Literal1.Text = "";
            }
        }
        else
        {
            Literal1.Text = "";
        }
    }

    private void Readwork()
    {
        if (Request.QueryString["sg"] != null && Request.QueryString["sc"] != null && Request.QueryString["ci"] != null && Request.QueryString["mi"] != null)
        {
            int Sgrade = Int32.Parse(Request.QueryString["sg"].ToString());
            int Sclass = Int32.Parse(Request.QueryString["sc"].ToString());
            int Cid = Int32.Parse(Request.QueryString["ci"].ToString());
            int Mid = Int32.Parse(Request.QueryString["mi"].ToString());
            int Wscore = 0;
            bool Wnone = false;
            if (CkselectG.Checked)
            {
                Wscore = 12;
            }
            if (CheckselectA.Checked)
            {
                Wscore = 10;
            }
            if (CheckBoxW.Checked)
            {
                Wnone = true;
            }
            LearnSite.BLL.Works ws = new LearnSite.BLL.Works();
            DataTable dt = ws.ShowCircleWorksSelect(Sgrade, Sclass, Cid, Mid, Wscore,Wnone);
            int n = dt.Rows.Count;
            int curindex = Int32.Parse(lbcurindex.Text);
            DDLname.Items.Clear(); 
            DDLstore.Items.Clear();
            if (n > 0)
            {
                ListItem[] lmname = new ListItem[n + 1];
                ListItem[] lmnone = new ListItem[n];
                lmname[0] = new ListItem("", "");
                for (int i = 0; i < n; i++)
                {
                    bool wcheck = bool.Parse(dt.Rows[i]["Wcheck"].ToString());
                    string sname = dt.Rows[i]["Sname"].ToString();
                    string wid = dt.Rows[i]["Wid"].ToString();
                    lmname[i + 1] = new ListItem(sname, wid);

                    if (wcheck)
                        sname = sname + (i + 1).ToString() + "√";
                    else
                        sname = sname + (i + 1).ToString();
                    lmnone[i] = new ListItem(sname, wid);
                }
                DDLname.Items.AddRange(lmname);
                DDLstore.Items.AddRange(lmnone);

                int allindex = n - 1;
                if (curindex == allindex)
                    lbcurindex.Text = "0";
                if (curindex < allindex)
                    DDLstore.SelectedIndex = curindex;
            }
            else {
                ListItem noneitem = new ListItem("", "");
                DDLname.Items.Add(noneitem);
                DDLstore.Items.Add(noneitem);    
            }
        }
    }
    private void ReadMtitle()
    {
        if (Request.QueryString["mi"] != null)
        {
            int Mid = Int32.Parse(Request.QueryString["mi"].ToString());
            LearnSite.BLL.Mission mbll = new LearnSite.BLL.Mission();
            LabeMtitle.Text = "〖" + mbll.GetMissionTitle(Mid) + "〗";
        }
    }
    protected void Btnflash_Click(object sender, EventArgs e)
    {
        int mc = DDLstore.Items.Count;
        if (mc > 0)
        {
            int curindex = DDLstore.SelectedIndex;//保存当前索引位置
            lbcurindex.Text = curindex.ToString();
            Readwork();
            if (curindex < DDLstore.Items.Count-1)
            {
                DDLstore.SelectedIndex = curindex;//未评筛选完成后刷新，会出错
            }
            showflash();
        }
    }
    protected void Btnrestart_Click(object sender, EventArgs e)
    {
        if (DDLstore.Items.Count > 0)
        {
            lbcurindex.Text = "0";
            Readwork();
            showflash();
        }
    }
    protected void DDLstore_SelectedIndexChanged(object sender, EventArgs e)
    {
        showflash();
    }
    protected void ImgBtnLeft_Click(object sender, EventArgs e)
    {
        int sdx = DDLstore.SelectedIndex;
        if (sdx > 0)
        {
            DDLstore.SelectedIndex = sdx - 1;
        }
        showflash();
    }
    protected void ImgBtnright_Click(object sender, EventArgs e)
    {
        int sdx = DDLstore.SelectedIndex;
        if (sdx < DDLstore.Items.Count - 1)
        {
            DDLstore.SelectedIndex = sdx + 1;
        }
        showflash();
    }
    protected void Btnstop_Click(object sender, EventArgs e)
    {
        if (Btnstop.Text == "暂停")
        {
            Btnstop.Text = "继续";
        }
        else
        {
            Btnstop.Text = "暂停";
        }
        showflash();
    }
    private void GetScore(string mySnum)
    {
        if (Request.QueryString["mi"] != null)
        {
            int Mid = Int32.Parse(Request.QueryString["mi"].ToString());
            LearnSite.BLL.Works ws = new LearnSite.BLL.Works();
            string[] myscoreself = ws.GetmyScoreWself(Mid, mySnum);
            string myscore = myscoreself[0].ToString();
            TextBoxWself.Text = HttpUtility.HtmlDecode(myscoreself[1].ToString());
            TextBoxWdsocre.Text = myscoreself[2].ToString();
            string mycheck = myscoreself[3].ToString();
            if (myscore != "")
            {
                int ascore = Int32.Parse(myscore);
                switch (ascore)
                {
                    case 12:
                        RBLselect.SelectedValue = "G";
                        break;
                    case 10:
                        RBLselect.SelectedValue = "A";
                        break;
                    case 8:
                        RBLselect.SelectedValue = "B";
                        break;
                    case 6:
                        RBLselect.SelectedValue = "C";
                        break;
                    case 4:
                        RBLselect.SelectedValue = "D";
                        break;
                    case 2:
                        RBLselect.SelectedValue = "E";
                        break;
                    case 0:
                        if(mycheck=="1"||mycheck.ToLower()=="true")
                            RBLselect.SelectedValue = "O";
                        break;
                }
            }
        }
    }
    protected void RBLselect_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Request.QueryString["mi"] != null)
        {
            int Mid = Int32.Parse(Request.QueryString["mi"].ToString());
            string Wnum = Labelnum.ToolTip;
            if (Wnum != "")
            {
                string selectStr = RBLselect.SelectedValue;
                int myscore = 0;
                switch (selectStr)
                {
                    case "G":
                        myscore = 12;
                        break;
                    case "A":
                        myscore = 10;
                        break;
                    case "B":
                        myscore = 8;
                        break;
                    case "C":
                        myscore = 6;
                        break;
                    case "D":
                        myscore = 4;
                        break;
                    case "E":
                        myscore = 2;
                        break;
                    case "O":
                        myscore = 0;
                        break;
                }
                string teaself = TextBoxWself.Text.Trim();
                string wself = HttpUtility.HtmlEncode(teaself);
                LearnSite.BLL.Works ws = new LearnSite.BLL.Works();
                if (wself == "")
                {
                    switch (myscore)
                    {
                        case 12:
                            wself = "作品已收藏";
                            break;
                        case 10:
                            wself = "作品很优秀";
                            break;
                        case 8:
                            wself = "作品良好";
                            break;
                        case 6:
                            wself = "作品一般";
                            break;
                        case 4:
                            wself = "有待改进";
                            break;
                        case 2:
                            wself = "作品不完整";
                            break;
                    }
                }
                string wdscorestr = TextBoxWdsocre.Text;
                int wdscore = 0;
                if (LearnSite.Common.WordProcess.IsNum(wdscorestr))
                    wdscore = Int32.Parse(wdscorestr);
                ws.Updatemscoreself(Mid, Wnum, myscore, wself, wdscore);//打分并评语
                if (DDLstore.Items.Count > 0)
                {
                    int sindex = DDLstore.SelectedIndex;
                    if (sindex < DDLstore.Items.Count - 1)
                    {
                        int curindex = sindex + 1;
                        DDLstore.SelectedIndex = curindex;//保存当前索引位置
                        lbcurindex.Text = curindex.ToString();
                        showflash();
                    }
                }
            }
        }
    }

    protected void ImgBtn_Click(object sender, EventArgs e)
    {
        int mc = DDLstore.Items.Count;
        if (mc > 0)
        {
            int curindex = DDLstore.SelectedIndex;//保存当前索引位置
            curindex = curindex + 1;
            lbcurindex.Text = curindex.ToString();
            if (curindex < mc)
            {
                DDLstore.SelectedIndex = curindex;
                showflash();
            }
            else
            {
                Readwork();
                showflash();
            }
        }
    }

    protected void ImageBtnDel_Click(object sender, EventArgs e)
    {
        int Mid = Int32.Parse(Request.QueryString["mi"].ToString());
        string Wnum = Labelnum.ToolTip;
        if (Wnum != "")
        {
            LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
            string wid = wbll.GetWidold(Mid.ToString(), Wnum);

            if (!string.IsNullOrEmpty(wid))
                wbll.Delete(Int32.Parse(wid));

            Readwork();
            showflash();
        }
    }
    protected void ImgBtnTextbox_Click(object sender, EventArgs e)
    {
        string flag = ImgBtnTextbox.CommandName;
        switch (flag)
        {
            case "v":
                ImgBtnTextbox.CommandName = "h";
                ImgBtnTextbox.Text = "显示评语";
                TextBoxWself.Visible = false;
                break;
            default:
                ImgBtnTextbox.CommandName = "v";
                ImgBtnTextbox.Text = "隐藏评语";
                TextBoxWself.Visible = true;
                break;
        }
    }

    protected void CkselectG_CheckedChanged(object sender, EventArgs e)
    {
        lbcurindex.Text = "0";
        Readwork();
        showflash();
        CheckselectA.Checked = false;
        CheckBoxW.Checked = false;
    }
    protected void CheckselectA_CheckedChanged(object sender, EventArgs e)
    {
        lbcurindex.Text = "0";
        Readwork();
        showflash();
        CheckBoxW.Checked = false;
        CkselectG.Checked = false;
    }
    protected void BtnCheck_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["mi"] != null)
        {
            int Mid = Int32.Parse(Request.QueryString["mi"].ToString());
            string Wnum = Labelnum.ToolTip;
            if (Wnum != "")
            {
                LearnSite.BLL.Works ws = new LearnSite.BLL.Works();
                ws.UpdateWcheck(Mid, Wnum);//打分并评语
                if (DDLstore.Items.Count > 0)
                {
                    int sindex = DDLstore.SelectedIndex;
                    if (sindex < DDLstore.Items.Count - 1)
                    {
                        int curindex = sindex + 1;
                        DDLstore.SelectedIndex = curindex;//保存当前索引位置
                        lbcurindex.Text = curindex.ToString();
                        showflash();
                    }
                }
            }
        }
    }

    protected void DDLname_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = DDLname.SelectedIndex;
        if (i > 0)
        {
            int curindex = i-1;
            DDLstore.SelectedIndex = curindex;//保存当前索引位置
            lbcurindex.Text = curindex.ToString();
            showflash();
        }
    }
    protected void CheckBoxW_CheckedChanged(object sender, EventArgs e)
    {
        lbcurindex.Text = "0";
        Readwork();
        showflash();
        CheckselectA.Checked = false;
        CkselectG.Checked = false;
    }
}
