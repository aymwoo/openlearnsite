using System;
using System.Collections.Generic;
using System.Web;
using System.Drawing;
using SimplePsd;
namespace LearnSite.Common
{
    /// <summary>
    ///psdToBmp 的摘要说明
    /// </summary>
    public class psdToBmp
    {
        public static Bitmap myImg(string fpath)
        {
            CPSD psd = new CPSD();//新取一个变量
            int res = psd.Load(fpath);
            if (res == 0)
            {
                return Bitmap.FromHbitmap(psd.GetHBitmap());
            }
            else
            {
                Bitmap bm = new Bitmap(2, 2);
                return bm;
            }
        }

        public static void Wthumbnail(string fpath)
        {
            Bitmap bmsource = myImg(fpath);
            int wi, hi;
            wi = bmsource.Width;
            hi = bmsource.Height;
            if (wi > 360) {
                wi = 360;
                hi = hi / wi * 360;
            }
            Image myThumbnail = bmsource.GetThumbnailImage(wi, hi, null, IntPtr.Zero);
            string nailpath = fpath.Replace(".psd", ".jpg");
            myThumbnail.Save(nailpath);
        }
    }
}