using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Spec_Research : System.Web.UI.Page
{
    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            Seacher();
        }
        else
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }
    }

    private void Seacher()
    {
        string Snum = cook.Snum;
        int Sid = cook.Sid;
        LearnSite.BLL.Research rbll = new LearnSite.BLL.Research();
        if (Snum.Contains("s"))
        {
            list();
        }
        else
        {
            if (rbll.Exist(Sid))
            {
                list();
                researchform.Visible = false;
            }
        }
    }
    private void list()
    {
        LearnSite.BLL.Research rbll = new LearnSite.BLL.Research();
        GVresearch.DataSource = rbll.GetClassList(cook.Syear, cook.Sgrade, cook.Sclass, cook.ThisTerm);
        GVresearch.DataBind();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        decimal  learn =Decimal.Parse( TextBox1.Text);
        decimal play = Decimal.Parse(TextBox2.Text);
        decimal sleep = Decimal.Parse(TextBox3.Text);
        decimal free = Decimal.Parse(TextBox4.Text);
        decimal sumtime = learn + play + sleep + free;
        if (sumtime > 24)
        {
            LearnSite.Common.WordProcess.Alert("你所花费的时间已经超过24小时，请仔细填写！", this.Page);
        }
        else
        {
           LearnSite.Model.Research rmode = new LearnSite.Model.Research();
           rmode.Rsid = cook.Sid;
           rmode.Ryear = cook.Syear;
           rmode.Rgrade = cook.Sgrade;
           rmode.Rclass = cook.Sclass;
           rmode.Rterm = cook.ThisTerm;
            rmode.Rlearn = learn;
            rmode.Rplay = play;
            rmode.Rsleep = sleep;
            rmode.Rfree = free;
            rmode.Rdate = DateTime.Now;
            LearnSite.BLL.Research rbll = new LearnSite.BLL.Research();
            if (!rbll.Exist(cook.Sid)) rbll.Add(rmode);
            researchform.Visible = false;
            System.Threading.Thread.Sleep(500);
            list();
        }
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        list();
    }
}