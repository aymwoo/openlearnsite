using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Data.SqlClient;

public partial class Python_manage : System.Web.UI.Page
{
    protected string xmlFile = "~/python/thumbnail/turtle.xml";
    protected string xmlmatch = "~/python/imgmatch/match.xml";

    LearnSite.Model.TeaCook tcook = new LearnSite.Model.TeaCook();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            showmatch();
        }
    }

    private void showmatch()
    {
        LearnSite.BLL.TurtleMatch bll = new LearnSite.BLL.TurtleMatch();
        DropDownListmatch.DataSource = bll.GetAllList();
        DropDownListmatch.DataTextField = "Mtitle";
        DropDownListmatch.DataValueField = "Mid";
        DropDownListmatch.DataBind();

    }
    protected void Btnpackage_Click(object sender, EventArgs e)
    {
        LearnSite.BLL.Turtle bll = new LearnSite.BLL.Turtle();
        DataTable dt = bll.GetAllList();
        dt.Columns.Remove("Tpage");

        string xmlpath = Server.MapPath(xmlFile);

        string msg = "";

        if (dt != null)
        {
            try
            {
                if (File.Exists(xmlpath))
                {
                    File.Delete(xmlpath);//如果已经存在则删除，不存在不会引发异常
                }
                dt.TableName = "Turtle";
                dt.WriteXml(xmlpath, XmlWriteMode.WriteSchema);//生成xml文件
                msg = "生成作品xml文件成功！";
            }
            catch (Exception ex)
            {
                msg = ex.ToString();
                LearnSite.Common.WordProcess.jsdialog(msg);
            }
        }
        else
        {
            msg = "无内容！";
        }

        dt.Dispose();
        Labelmsg.Text = msg;
    }
    protected void Btnimport_Click(object sender, EventArgs e)
    {
        string serverip = LearnSite.Common.Computer.GetServerIp();
        string guestip = LearnSite.Common.Computer.GetGuestIP();
        if (serverip.Equals(guestip))
        {
            string msg = "";
            string xmlpath = Server.MapPath(xmlFile);
            if (File.Exists(xmlpath))
            {
                if (tcook.IsExist())
                {
                    DataTable dt = new DataTable();
                    dt.ReadXml(xmlpath);
                    if (dt.TableName == "Turtle")
                    {
                        if (dt != null)
                        {
                            int dCount = dt.Rows.Count;
                            LearnSite.BLL.Turtle bll = new LearnSite.BLL.Turtle();
                            for (int i = 0; i < dCount; i++)
                            {
                                LearnSite.Model.Turtle model = new LearnSite.Model.Turtle();
                                model = bll.GetModel(dt, i);
                                //更换导入老师Thid
                                model.Thid = tcook.Hid;
                                bll.Add(model);//增加作品                    
                            }
                            msg = "导入成功，共" + dCount.ToString() + "条作品记录！";

                        }
                        else
                        {
                            msg = "无作品记录！";
                        }
                    }
                    else
                    {
                        msg = "xml文档不包含正确的数据表！";
                    }
                }
            }
            else
            {
                msg = "xml文档不存在！";
            }
            Labelmsg.Text = msg;
        }
        else
        {
            Labelmsg.Text = "服务器IP" + serverip + " 客户端IP" + guestip;
        }
    }
    protected void Buttonmatch_Click(object sender, EventArgs e)
    {
        string mid = DropDownListmatch.SelectedValue;
        if (!string.IsNullOrEmpty(mid))
        {
            LearnSite.BLL.TurtleMatch bll = new LearnSite.BLL.TurtleMatch();
            DataTable dtmatch = bll.GetMidList(mid).Tables[0];
            LearnSite.BLL.TurtleQuestion qbll = new LearnSite.BLL.TurtleQuestion();
            DataTable dtquestion = qbll.GetAllList().Tables[0];


            string msg = "";

            if (dtquestion != null)
            {
                DataSet ds = new DataSet();
                DataTable dt1 = dtmatch.Copy();
                dt1.TableName = "TurtleMatch";
                ds.Tables.Add(dt1);
                DataTable dt2 = dtquestion.Copy();
                dt2.TableName = "TurtleQuestion";

                ds.Tables.Add(dt2);

                string xmlpath = Server.MapPath(xmlmatch);
                try
                {
                    if (File.Exists(xmlpath))
                    {
                        File.Delete(xmlpath);//如果已经存在则删除，不存在不会引发异常
                    }
                    ds.DataSetName = "Match";
                    ds.WriteXml(xmlpath, XmlWriteMode.WriteSchema);//生成xml文件
                    msg = "生成比赛xml文件成功！";
                }
                catch (Exception ex)
                {
                    LearnSite.Common.WordProcess.jsdialog(ex.ToString());
                }
                ds.Dispose();
            }
            else
            {
                msg = "无内容！";
            }

            Labelmsg.Text = msg;
        }
    }
    protected void Buttonimport_Click(object sender, EventArgs e)
    {
        string serverip = LearnSite.Common.Computer.GetServerIp();
        string guestip = LearnSite.Common.Computer.GetGuestIP();
        if (serverip.Equals(guestip))
        {
            string msg = "";
            string xmlpath = Server.MapPath(xmlmatch);
            if (File.Exists(xmlpath))
            {
                if (tcook.IsExist())
                {
                    DataSet ds = new DataSet();
                    ds.ReadXml(xmlpath);
                    if (ds.DataSetName == "Match")
                    {
                        if (ds.Tables.Count == 2)
                        {
                            DataTable dt = ds.Tables[0];
                            int dCount = dt.Rows.Count;
                            LearnSite.BLL.TurtleMatch bll = new LearnSite.BLL.TurtleMatch();
                            for (int i = 0; i < dCount; i++)
                            {
                                LearnSite.Model.TurtleMatch model = new LearnSite.Model.TurtleMatch();
                                model = bll.GetModel(dt, i);
                                //更换导入老师Thid 
                                model.Mhid = tcook.Hid;
                                int oldmid = model.Mid;
                                int newmid = bll.Add(model);//增加比赛 

                                DataView dv = ds.Tables[1].DefaultView;
                                dv.RowFilter = " Qmid=" + oldmid;
                                DataTable dtques = dv.ToTable();
                                int dqcount = dtques.Rows.Count;
                                LearnSite.BLL.TurtleQuestion qbll = new LearnSite.BLL.TurtleQuestion();
                                for (int j = 0; j < dqcount; j++)
                                {
                                    LearnSite.Model.TurtleQuestion qmodel = new LearnSite.Model.TurtleQuestion();
                                    qmodel = qbll.GetModel(dtques, j);
                                    qmodel.Qmid = newmid;
                                    qbll.Add(qmodel);//增加问题                            
                                }
                            }
                            msg = "导入成功，共" + dCount.ToString() + "条作品记录！";

                        }
                        else
                        {
                            msg = "无作品记录！";
                        }
                    }
                    else
                    {
                        msg = "xml文档不包含正确的数据表！";
                    }
                }
            }
            else
            {
                msg = "xml文档不存在！";
            }
            Labelmsg.Text = msg;
        }
        else
        {
            Labelmsg.Text = "服务器IP" + serverip + " 客户端IP" + guestip;
        }
    }
}