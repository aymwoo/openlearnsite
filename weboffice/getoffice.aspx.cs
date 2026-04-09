using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
public partial class weboffice_getoffice : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Clear();
        string id = Request.QueryString["id"];
        if (id == null || id == "")
        {
            Response.End();
            return;
        }
        else
        {
            LearnSite.BLL.Works wbll = new LearnSite.BLL.Works();
            string officeurl = wbll.GetWorkWurl(Int32.Parse(id));
            if (officeurl != "")
            {
                string officepath = MapPath(officeurl);
                if (File.Exists(officepath))
                {
                    byte[] Buffer;
                    using (FileStream myfileStream = new FileStream(officepath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        long fileSize = myfileStream.Length;
                        Buffer = new byte[(int)fileSize];
                        myfileStream.Read(Buffer, 0, (int)fileSize);
                    }
                    Response.BinaryWrite(Buffer);
                    Response.End();
                }
            }
        }
        Response.End();
    }
}
