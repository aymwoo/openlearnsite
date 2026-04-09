using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
namespace LearnSite.Common
{
    /// <summary>
    ///Htmlcheck 的摘要说明
    /// </summary>
    public class Htmlcheck
    {
        /// <summary>
        /// 检测html文件是否更新
        /// </summary>
        /// <param name="Syear"></param>
        /// <param name="Sclass"></param>
        /// <param name="Snum"></param>
        /// <param name="htmlname"></param>
        /// <returns></returns>
        public static string HtmlUpdatetime(int Syear, int Sclass, string Snum, string htmlname)
        {
            htmlname = htmlname.Trim().ToLower();
            if (htmlname.EndsWith("l"))
            {
                htmlname = htmlname.Substring(0, htmlname.Length - 2);
            }
            string updatetime = "";
            string htmlfilenme = "~/FtpSpace/" + Syear.ToString() + "/" + Sclass.ToString() + "/" + Snum + "/" + htmlname;
                string filepath = HttpContext.Current.Server.MapPath(htmlfilenme);
                if (File.Exists(filepath))
                {
                    FileInfo filemsg = new FileInfo(filepath);
                    updatetime = filemsg.LastWriteTime.ToString();
                }
                else
                {
                    filepath = filepath + "l";
                    if (File.Exists(filepath))
                    {
                        FileInfo filemsg = new FileInfo(filepath);
                        updatetime = filemsg.LastWriteTime.ToString();
                    }
                }
            return updatetime;
        }

        public static long countDirSize(System.IO.DirectoryInfo dir)
        {
            long size = 0;
            FileInfo[] files = dir.GetFiles();
            //通过GetFiles方法,获取目录中所有文件的大小
            foreach (System.IO.FileInfo info in files)
            {
                size += info.Length;
            }
            DirectoryInfo[] dirs = dir.GetDirectories();
            //获取目录下所有文件夹大小,并存到一个新的对象数组中
            foreach (DirectoryInfo dirinfo in dirs)
            {
                size += countDirSize(dirinfo);
            }
            return size;
        } 
    }
}