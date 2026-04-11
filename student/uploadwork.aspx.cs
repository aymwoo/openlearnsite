using System;
using System.Collections.Generic;
using System.Web;

public partial class Student_uploadwork : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        uploadthiswork();
    }

    private void uploadthiswork()
    {
        if (Request.Files["Filedata"] != null)
        {
            try
            {
                HttpPostedFile work_upload = Request.Files["Filedata"];
                string lidValue = Request.QueryString["lid"];
                if (string.IsNullOrEmpty(lidValue) || !LearnSite.Common.WordProcess.IsNum(lidValue))
                {
                    Response.StatusCode = 200;
                    Response.Write("活动参数错误!");
                    Response.End();
                    return;
                }

                string Wlid = lidValue;
                LearnSite.BLL.ListMenu lbll = new LearnSite.BLL.ListMenu();
                LearnSite.Model.ListMenu lmodel = new LearnSite.Model.ListMenu();
                lmodel = lbll.GetModel(Int32.Parse(Wlid));
                if (lmodel == null || !lmodel.Lxid.HasValue)
                {
                    Response.StatusCode = 200;
                    Response.Write("此活动不存在!");
                    Response.End();
                    return;
                }

                string Wmid = lmodel.Lxid.Value.ToString();
                //string Wnum = Request.QueryString["num"].ToString();
                LearnSite.BLL.Mission mbll = new LearnSite.BLL.Mission();
                LearnSite.Model.Mission mmodel = new LearnSite.Model.Mission();
                mmodel = mbll.GetModel(lmodel.Lxid.Value);
                if (mmodel == null)
                {
                    Response.StatusCode = 200;
                    Response.Write("此活动不存在!");
                    Response.End();
                    return;
                }

                string Wcid = mmodel.Mcid.ToString();
                string Wmsort = mmodel.Msort.ToString();
                string Wfiletype = work_upload.FileName.Substring(work_upload.FileName.LastIndexOf(".") + 1).ToLower(); 
                string Wextention = mmodel.Mfiletype;
                string limitext = Wextention;
                switch (Wextention)
                {
                    case "doc":
                        limitext = "*.doc;*.docx";
                        break;
                    case "ppt":
                        limitext = "*.ppt;*.pptx";
                        break;
                    case "xls":
                        limitext = "*.xls;*.xlsx";
                        break;
                    case "office":
                        limitext = "*.doc;*.docx;*.ppt;*.pptx;*.xls;*.xlsx";
                        break;
                    case "sb":
                        limitext = "*.sb;*.sb2;*.sb3";
                        break;
                    case "iframe":
                        limitext = "*.psd;*.iframe";
                        break;
                    case "png":
                    case "jpg":
                        limitext = "*.png;*.jpg;*.jpeg;*.gif";
                        break;
                    default:
                        limitext = "*." + Wextention;
                        break;
                }

                if (!(Wfiletype == Wextention || limitext.Contains(Wfiletype)))
                {
                    Response.StatusCode = 200;
                    Response.Write("选择的文件类型错误!");
                    Response.End();
                    return;
                }

                int Wlength = work_upload.ContentLength;
                //Syear | Sgrade | Sclass | Sid | Sname | Wip | Sterm | LoginTime
                
                LearnSite.Model.Cook cook = new LearnSite.Model.Cook();
                string Syear = cook.Syear.ToString();
                string Sgrade = cook.Sgrade.ToString();
                string Sclass = cook.Sclass.ToString();
                string Wsid = cook.Sid.ToString();
                string Wip = cook.LoginIp;
                string Sname = cook.Sname;
                string Wnum = cook.Snum;
                string Wterm = cook.ThisTerm.ToString();
                string LoginTime = cook.LoginTime;

                DateTime Wdate = DateTime.Now;
                bool checkcan = true;

                LearnSite.BLL.Works ws = new LearnSite.BLL.Works();
                LearnSite.Model.Works wmodelp = new LearnSite.Model.Works();
                
                wmodelp = ws.GetModelByStu(Int32.Parse(Wmid), Wnum);

                //string Wid = ws.WorkDone(Wnum, Int32.Parse(Wcid), Int32.Parse(Wmid));//返回空字符表示不存在该记录
                if (wmodelp != null)
                {
                    if (!wmodelp.Wcheck)
                    {
                        //如果未评价，则重新提交修改作品
                        string MySavePath = LearnSite.Common.WorkUpload.GetWurl(Syear, Sgrade, Sclass, Wcid, Wmid);//获得作品保存路径（如果不存在，自动创建）
                        //string RndTime = LearnSite.Common.WordProcess.GetRandomNum(99).ToString();
                        string OnlyFileName = Wnum + "_" + Wcid + "_" + Wmid;// +"_" + RndTime;
                        string NewFileName = OnlyFileName + "." + Wfiletype;
                        string Wurl = MySavePath + "/" + NewFileName;
                        
                        string resaveFilename = Server.MapPath(Wurl);
                        try
                        {
                            ///////////添加时间
                            int today = DateTime.Now.DayOfYear;
                            int workday = wmodelp.Wdate.Value.DayOfYear;
                            if (today == workday)
                            {
                                ws.UpdateWorkUp(wmodelp.Wid, Wurl, NewFileName, Wlength, Wdate, checkcan, "");//更新Wfilename, Wurl,Wlength, Wdate
                            }
                            else
                                ws.UpdateWorkUpday(wmodelp.Wid, Wurl, NewFileName, Wlength, Wdate, checkcan, "","");//更新Wfilename, Wurl,Wlength, Wdate
                            //LearnSite.BLL.Signin sn = new LearnSite.BLL.Signin();
                            //sn.UpdateQwork(Int32.Parse(Wsid), Int32.Parse(Wcid));//更新今天签到表中的作品数量
                            work_upload.SaveAs(resaveFilename);//保存提交作品 

                            Response.StatusCode = 200;
                            Response.Write(NewFileName);
                        }
                        catch (Exception ex)
                        {
                            Response.StatusCode = 200;
                            Response.Write(ex);
                        }
                        finally
                        {
                            Response.End();
                        }
                    }
                    else
                    {
                        Response.StatusCode = 200;
                        Response.Write("老师已经评价了!");
                        Response.End();
                    }
                }
                else
                {
                    //如果作品未提交，提交作品(Wnum, Wcid,Wmid,Wmsort, Wfilename, Wurl,Wlength, Wdate, Wip, Wtime)                            
                    string MySavePath = LearnSite.Common.WorkUpload.GetWurl(Syear, Sgrade, Sclass, Wcid, Wmid);//获得作品保存路径（如果不存在，自动创建）
                    string RndTime = LearnSite.Common.WordProcess.GetRandomNum(99).ToString();
                    string OnlyFileName = Wnum + "_" + Wcid + "_" + Wmid + "_" + RndTime;
                    string NewFileName = OnlyFileName + "." + Wfiletype;
                    string Wurl = MySavePath + "/" + NewFileName;

                    string Wthumbnail = MySavePath + "/" + OnlyFileName + ".jpg";

                    string Wtime = LearnSite.Common.Computer.TimePassed(LoginTime).ToString();

                    LearnSite.Model.Works wmodel = new LearnSite.Model.Works();
                    wmodel.Wnum = Wnum;
                    wmodel.Wcid = Int32.Parse(Wcid);
                    wmodel.Wmid = Int32.Parse(Wmid);
                    wmodel.Wmsort = Int32.Parse(Wmsort);
                    wmodel.Wfilename = NewFileName;
                    wmodel.Wtype = Wfiletype;
                    wmodel.Wurl = Wurl;
                    wmodel.Wlength = Wlength;
                    wmodel.Wdate = Wdate;
                    wmodel.Wip = Wip;
                    wmodel.Wtime = Wtime;
                    wmodel.Wcan = checkcan;
                    wmodel.Wcheck = false;
                    wmodel.Wegg = 12;//设定票数为12张
                    wmodel.Whit = 0;
                    wmodel.Wgrade = Int32.Parse(Sgrade);
                    wmodel.Wterm = Int32.Parse(Wterm);
                    wmodel.Wsid = Int32.Parse(Wsid);
                    wmodel.Wclass = Int32.Parse(Sclass);
                    wmodel.Wname = HttpUtility.UrlDecode(Sname);
                    wmodel.Wyear = Int32.Parse(Syear);
                    wmodel.Wthumbnail = Wthumbnail;
                    switch (Wfiletype)
                    {
                        case "doc":
                        case "ppt":
                        case "xls":
                        case "docx":
                        case "pptx":
                        case "xlsx":
                        case "wps":
                        case "dps":
                        case "et":
                            wmodel.Woffice = true;
                            break;
                        default:
                            wmodel.Woffice = false;
                            break;
                    }
                    wmodel.Wflash = false;
                    wmodel.Werror = false;
                    string saveFilename = Server.MapPath(Wurl);
                    try
                    {
                        ws.AddWorkUp(wmodel);//添加作品提交记录

                        //添加课堂活动记录，重复提交时仍保持一条完成记录
                        ws.EnsureMenuWorksCompletion(Int32.Parse(Wsid), Int32.Parse(Wlid), DateTime.Parse(LoginTime), Wdate);

                        LearnSite.BLL.Signin sn = new LearnSite.BLL.Signin();
                        sn.UpdateQwork(Int32.Parse(Wsid), Int32.Parse(Wcid));//更新今天签到表中的作品数量

                        work_upload.SaveAs(saveFilename);//保存提交作品

                        Response.StatusCode = 200;
                        Response.Write(NewFileName);
                    }
                    catch (Exception ex)
                    {
                        Response.StatusCode = 200;
                        Response.Write(ex);
                    }
                    finally
                    {
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 200;
                Response.Write(ex);
            }
            finally
            {
                Response.End();
            }
        }
    }

}
