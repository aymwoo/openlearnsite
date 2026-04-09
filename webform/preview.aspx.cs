using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

public partial class webform_preview : System.Web.UI.Page
{
    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
    public int Cid = 0;
    public int Eid = 0;
    public int Lid = 0;
    public string Examjson = "";
    public int Done = 0;
    public string Score = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        LearnSite.Common.CookieHelp.KickStudent();
        if (!IsPostBack)
        {
            showExam();
        }

    }

    private void showExam()
    {
        if (Request.QueryString["lid"] != null)
        {
            string Lidstr = Request.QueryString["lid"].ToString();
            if (LearnSite.Common.WordProcess.IsNum(Lidstr))
            {
                Lid = Int32.Parse(Lidstr);
                LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
                LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
                lmodel = lbll.GetModel(Lid);
                Cid = lmodel.Lcid.Value;
                Eid = lmodel.Lxid.Value;
                LearnSite.Model.Exams emodel = new LearnSite.Model.Exams();
                LearnSite.BLL.Exams ebll = new LearnSite.BLL.Exams();
                emodel = ebll.GetModel(Eid);
                Examjson = emodel.Edata;

                LearnSite.BLL.Answers abll=new LearnSite.BLL.Answers();
                LearnSite.Model.Answers amodel =new LearnSite.Model.Answers();
                amodel = abll.GetModelme(Eid,cook.Sid);
                if(amodel!=null){
                    Done =1;
                    Score = amodel.Ascore.ToString();
                    HyperLinkAnalysis.Visible = true;
                    HyperLinkAnalysis.NavigateUrl = "Analysis.aspx?eid=" + Eid.ToString();
                }
                else{
                    Done =0;
                    HyperLinkAnalysis.Visible = false;
                }
            }
        }
    }
}