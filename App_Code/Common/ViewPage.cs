using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Drawing;
using System.Text;

namespace LearnSite.Common
{
    /// <summary>
    ///ViewPage 的摘要说明
    /// </summary>
    public class ViewPage
    {
        public ViewPage()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        /// <summary>
        /// 作品预览页面
        /// </summary>
        /// <param name="Wid">作品ID</param>
        /// <param name="Wtype">作品类型</param>
        /// <param name="Wurl">作品链接</param>
        /// <param name="Wcode">作品内容</param>
        /// <param name="isflex">是否采用flexpaper预览</param>
        /// <param name="istea">是否教师，显示链接</param>
        /// <returns></returns>
        public static string SelectWritePlugin(string Wid, string Wtype, string Wurl, string Wcode, string Wthumbnail, bool isflex, bool istea,string Wnum)
        {
            Wurl =HttpUtility.UrlDecode(Wurl);
            string str = "";
            switch (Wtype)
            {
                case "website":
                    str = WordProcess.htmlsite(Wthumbnail,Wnum);
                    break;
                case "markdown":
                    str = WordProcess.markdownhtml(Wcode);
                    break;
                case "speek":
                    str = WordProcess.htmlauto(Wcode);
                    break;
                case "ai":
                case "ocr":
                case "sound":
                case "tic-tac-toe":
                case "handnum":
                case "text-to-image":
                case "iframe":
                case "web":
                case "ware":
                    str = WordProcess.photohtmlauto(Wthumbnail);
                    break;
                case "sokoban":
                    str = WordProcess.sokobanview(Wcode);
                    break;
                case "excalidraw":
                    Wurl = Wurl.Replace("excalidraw", "png");
                    str = WordProcess.photohtmlauto(Wurl);
                    break;
                case "mqtt":
                    str = WordProcess.mqtthtml(Wcode,Wthumbnail);
                    break;
                case "face":
                    str = WordProcess.photohtmlauto(Wurl);
                    break;
                case "mlimg":
                    str = WordProcess.mlimg(Wurl);
                    break;
                case "poster":
                    Wurl = Wurl.Replace("json", "png");
                    str = WordProcess.photohtmlauto(Wurl);
                    break;
                case "pptist":
                    str = WordProcess.ppthtml(Wurl);
                    break;
                case "word":
                    str = WordProcess.wordhtml(Wcode);
                    break;
                case "qrcode":
                    str = WordProcess.qrcodehtml(Wurl);
                    break;
                case "block":
                    str = WordProcess.pyBlock(Wcode);
                    break;
                case "sheet":
                    str = WordProcess.onlineSheet(Wcode);
                    break;
                case "pdf":
                    str = WordProcess.pdfhtml(Wurl);
                    break;
                case "htm":
                case "html":
                    string siteurl = Wurl.Replace("~", "..");
                    str = "<Iframe name='ls2012' frameborder='0' src='" + siteurl + "' width='100%' height='1024'>浏览器不支持嵌入式框架</iframe>";
                    break;
                case "txt":
                    str = WordProcess.txtswf(Wurl);
                    break;
                case "pas":
                case "c":
                case "cpp":
                case "frm":
                case "as":
                case "java":
                case "ino":
                case "lgo":
                    str = WordProcess.txtview(Wurl);
                    break;
                case "pxl":
                    str = WordProcess.pixelview(Wcode);
                    break;
                case "py":
                    str = WordProcess.pyWcode(Wcode, Wurl);
                    break;
                case "swf":
                    str = WordProcess.flashhtml(Wurl);
                    break;
                case "xlsx":
                    str = WordProcess.xlsxView(Wurl);
                    break;
                case "docx":
                    str = WordProcess.docxView(Wurl);
                    break;
                case "pptx":
                    str = WordProcess.pptxView(Wurl);
                    break;
                case "xls":
                case "doc":
                case "ppt":
                case "wps":
                case "dps":
                case "et":
                    str = WordProcess.flashofficehtmltea(Wurl, isflex);
                    break;
                case "mht":
                    string mhturl = Wurl.Replace("~", "..");
                    str = "<Iframe name='ls2012' frameborder='0' src='" + mhturl + "' width='100%' height='600'>浏览器不支持嵌入式框架</iframe>";
                    break;
                case "mm":
                    str = WordProcess.mmhtml(Wurl);
                    break;
                case "km":
                    str = WordProcess.kmhtml(Wurl);
                    break;
                case "sb":
                case "sb2":
                case "sb3":
                    str = WordProcess.sb3htmlcode(Wurl);//sbhtml(Wurl);
                    break;
                case "dae":
                    str = WordProcess.daehtml(Wurl);
                    break;
                case "dxf":
                    str = WordProcess.dxfhtml(Wurl);
                    break;
                case "psd":
                    str = WordProcess.psdhtml(Wurl);
                    break;
                case "bmp":
                    str = WordProcess.photobaseauto(Wurl);
                    break;
                case "jpg":
                case "png":
                case "gif":
                case "wmf":
                case "svg":
                    str = WordProcess.photohtmlauto(Wurl);
                    break;
                case "xml":
                    str = WordProcess.photohtmlauto(Wurl.Replace("xml", "png"));
                    break;
                case "flv":
                case "mp4":
                    str = WordProcess.flvhtml(Wurl);
                    break;
                case "mp3":
                case "wma":
                case "rm":
                case "ra":
                case "ram":
                case "asf":
                case "mid":
                case "wmv":
                case "wav":
                case "avi":
                case "mpg":
                case "3gp":
                case "mkv":
                case "mov":
                    str = WordProcess.mp3html(Wurl, Wtype);
                    break;
                default:
                    str = "<div style='text-align: center;font-size: 14pt;'>该文件格式无法支持在线预览~!</div>";
                    break;
            }
            if (istea)
            {
                string mydown = Wurl.Replace("~", "..");
                string url = "<br /><br /><div style='text-align: center;font-size: 16px; color:#333333;'><a href='" + mydown + "' target='_blank'>下载查看</a>  </div>";
                str = str + url;
            }
            return str;//返回预览的html生成页面
        }
    }
}