using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Profile_myphoto : System.Web.UI.Page
{
    LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            CanUpload();
            if (!IsPostBack)
            {
                showphoto();
            }
        }
        else
        {
            LearnSite.Common.CookieHelp.JudgeStudentCookies();
        }
    }
    protected void Btnphoto_Click(object sender, EventArgs e)
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            string mynum = cook.Snum;
            string mysex = cook.Sex;
            string rs = LearnSite.Common.Photo.PhotoUpload(PhotoFileUpload, mynum);
            string msg = "您好！发生了什么事？请告诉您的老师！";
            switch (rs)
            {
                case "0":
                    msg = "请选择你要上传的相片！";
                    LearnSite.Common.WordProcess.Alert(msg, this.Page);
                    break;
                case "1":
                    msg = "相片提交成功！";
                    showphoto();
                    break;
                case "2":
                    msg = "相片提交成功！";
                    showphoto();
                    break;
                case "3":
                    msg = "不是真实的图片格式，请仔细查看！（改后缀无效）";
                    LearnSite.Common.WordProcess.Alert(msg, this.Page);
                    break;
                case "4":
                    msg = "相片太大，不能超过2048KB！";
                    LearnSite.Common.WordProcess.Alert(msg, this.Page);
                    break;
                case "5":
                    msg = "相片要求为png、jpg或jpeg图片格式！";
                    LearnSite.Common.WordProcess.Alert(msg, this.Page);
                    break;
            }
        }
    }

    private void showphoto()
    {
        if (LearnSite.Common.CookieHelp.IsStudentLogin())
        {
            string mynum = cook.Snum;
            string mysex = cook.Sex;
            string imgurl = LearnSite.Common.Photo.GetStudentPhotoUrl(mynum, mysex);
            Imageface.ImageUrl = imgurl + "?temp=" + DateTime.Now.Millisecond.ToString();
        }
    }
    private void CanUpload()
    {
        int sclass = cook.Sclass;
        int sgrade = cook.Sgrade;
        LearnSite.BLL.Room rbll = new LearnSite.BLL.Room();
        if (rbll.GetRphotoedit(sgrade, sclass))
        {
            Btnphoto.Enabled = true;
        }
        else
        {
            Btnphoto.Enabled = false;
            Labelstr.Text = "限制修改相片";
        }
    }
}