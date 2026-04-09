using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;

namespace LearnSite.Common
{
    /// <summary>
    ///Fileupload 的摘要说明
    /// </summary>
    public class Fileupload
    {
        public Fileupload()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        /// <summary>
        /// 保存资源上传文件
        /// </summary>
        /// <param name="FUsoft"></param>
        /// <returns></returns>
        public static string Fupload(FileUpload FUsoft)
        {            
            string myfile = "";
            if (FUsoft.HasFile)
            {
                string uploadfile = FUsoft.PostedFile.FileName;
                string filename = System.IO.Path.GetFileName(uploadfile);
                string DownloadPath = "~/Download/";
                string realpath = HttpContext.Current.Server.MapPath(DownloadPath);
                DateTime dt = DateTime.Now;
                string NowTime = dt.Year.ToString()+"-"+dt.Month.ToString()+"-"+dt.Day.ToString();
                string Creatfile = NowTime + filename;
                if (!Directory.Exists(realpath))
                {
                    Directory.CreateDirectory(realpath);
                }
                FUsoft.SaveAs(Common.Flatform.Checkbdir(realpath) + Creatfile);
                
                myfile = Checkpdir(DownloadPath) + Creatfile;
            }
            return myfile;
        }

        private static string Checkpdir(string str)
        {
            if (!str.EndsWith("/"))
            {
                str = str + "/";
            }
            return str;
        }
    }
}
