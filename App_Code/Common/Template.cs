using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
namespace LearnSite.Common
{
    /// <summary>
    ///Template 的摘要说明
    /// </summary>
    public class Template
    {
        public Template()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        public static string markdownhtm()
        {
            string strhtm = "~/markdown/preview.html";
            return readhtm(strhtm);
        }

        public static string sokobanhtm()
        {
            string strhtm = "~/sokoban/mapview.html";
            return readhtm(strhtm);
        }
        public static string xlsxhtml()
        {
            //暂时用luckysheet显示xlsx表格
            string strhtm = "~/plugins/luckysheet/index.html";
            return readhtm(strhtm);
        }
        public static string pptxhtml()
        {
            string strhtm = "~/plugins/pptx/index.html";
            return readhtm(strhtm);
        }
        public static string docxhtml()
        {
            string strhtm = "~/plugins/docx/index.html";
            return readhtm(strhtm);
        }
        public static string mqtthtml()
        {
            string strhtm = "~/plugins/mqtt.html";
            return readhtm(strhtm);
        }
        public static string ppthtm()
        {
            string strhtm = "~/plugins/PPTist/ppt.html";
            return readhtm(strhtm);
        }
        public static string wordhtm()
        {
            string strhtm = "~/plugins/canvas-editor/word.html";
            return readhtm(strhtm);
        }
        public static string qrcodehtm()
        {
            string strhtm = "~/plugins/qrcode/canvas.html";
            return readhtm(strhtm);
        }
        public static string sheethtm()
        {
            string strhtm = "~/plugins/luckysheet/example.html";
            return readhtm(strhtm);
        }
        public static string pixelhtm()
        {
            string strhtm = "~/pixelartmaker/PixelArt.html";
            return readhtm(strhtm);
        }
        public static string codehtm()
        {
            string strhtm = "~/plugins/python/code.htm";
            return readhtm(strhtm);
        }
        public static string pythonblockhtm()
        {
            string strhtm = "~/plugins/python/blockpy.htm";
            return readhtm(strhtm);
        }
        public static string pythonhtm()
        {
            string strhtm = "~/plugins/python/python.htm";
            return readhtm(strhtm);
        }
        public static string pythonstuhtm()
        {
            string strhtm = "~/plugins/python/stu.htm";
            return readhtm(strhtm);
        }
        public static string txthtm()
        {
            string strhtm = "~/plugins/txt/txt.htm";
            return readhtm(strhtm);
        }
        public static string mp3htm(string ext)
        {
            string strhtm = "~/plugins/mp3/src.htm";
            if (ext == "mp3")
                strhtm = "~/plugins/mp3/mp3.htm";//如果是mp3，则使用mp3 flash player
            return readhtm(strhtm);
        }

        public static string flashhtm()
        {
            string strhtm = "~/plugins/flash.htm";
            return readhtm(strhtm);
        }

        public static string officehtm()
        {
            string strhtm = "~/plugins/office.htm";
            return readhtm(strhtm);
        }

        public static string webofficetwohtm()
        {
            string strhtm = "~/weboffice/webofficetwo.htm";
            return readhtm(strhtm);
        }
        public static string webofficehtm()
        {
            string strhtm = "~/weboffice/weboffice.htm";
            return readhtm(strhtm);
        }

        public static string sketchuphtm()
        {
            string strhtm = "~/plugins/sketchup/sketchup.htm";
            return readhtm(strhtm);
        }
        public static string scratchflashnewhtm()
        {
            string strhtm = "~/Statics/scratch.htm";
            return readhtm(strhtm);
        }
        public static string scratchflashhtm()
        {
            string strhtm = "~/plugins/scratch/scratch.htm";
            return readhtm(strhtm);
        }
        public static string scratchjshtm()
        {
            string strhtm = "~/plugins/scratch/scratch.htm";
            return readhtm(strhtm);
        }
        public static string scratchcodehtm()
        {
            string strhtm = "~/plugins/scratch/code.htm";
            return readhtm(strhtm);
        }

        public static string psdhtm()
        {
            string strhtmsrc = "~/plugins/psd/psd.htm";
            return readhtm(strhtmsrc);//直接显示
        }

        public static string mlimghtm()
        {
            string strhtmsrc = "~/machine/preview.html";
            return readhtm(strhtmsrc);//直接显示
        }

        public static string previewhtm()
        {
            string strhtmsrc = "~/plugins/photo/preview.htm";
            return readhtm(strhtmsrc);//直接显示
        }
        public static string sitehtm()
        {
            string strhtmsrc = "~/plugins/photo/site.htm";
            return readhtm(strhtmsrc);//直接显示
        }
        public static string photohtm()
        {
            string strhtmsrc = "~/plugins/photo/src.htm";
            return readhtm(strhtmsrc);//直接显示
        }
        public static string jwplayer()
        {
            string strhtm = "~/plugins/photo/photo.htm";
            return readhtm(strhtm);//直接显示
        }
        public static string flvhtm()
        {
            string strhtm = "~/plugins/flv/flv.htm";
            return readhtm(strhtm);//直接显示
        }
        public static string freemindhtm()
        {
            string strhtm = "~/plugins/freemind/freemindbrowser.htm";
            return readhtm(strhtm);
        }
        public static string kitymindhtm()
        {
            string strhtm = "~/plugins/km/index.htm";
            return readhtm(strhtm);
        }
        private static string readhtm(string filepath)
        {
            string str = "找不到预览的插件模板！<br/>"+filepath;
            string fpath = HttpContext.Current.Server.MapPath(filepath);
            if (File.Exists(fpath))
            {
                using (StreamReader fr = new StreamReader(fpath, System.Text.Encoding.UTF8))
                {
                    str = fr.ReadToEnd();
                }
            }
            return str;
        }
        /// <summary>
        /// 修改txt中的内容
        /// </summary>
        private static void UpdateTextFile(string FileName, string oldStr, string newStr)
        {
            string s;
            using (System.IO.StreamReader sr = new System.IO.StreamReader(FileName, System.Text.Encoding.Default))//GetEncoding("gb2312 "));
            {
                s = sr.ReadToEnd();
            }
            s = s.Replace(oldStr, newStr);
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(FileName, false))
            {
                sw.Write(s);
            }
        }
        /// <summary>
        /// 无效
        /// </summary>
        /// <param name="upmodel"></param>
        public static void UpdateRoadJs(string upmodel)
        {
            string jsfile = "~/js/road.js";
            string jspath = HttpContext.Current.Server.MapPath(jsfile);
            if (File.Exists(jspath))
            {
                string oldStr = "0';";
                string newStr = "1';";
                if (upmodel == "0")
                    UpdateTextFile(jspath, oldStr, newStr);//0换1
                else
                    UpdateTextFile(jspath, newStr, oldStr);//反过来1换0
            }
        }
    }
}
