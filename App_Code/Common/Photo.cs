using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
namespace LearnSite.Common
{
    /// <summary>
    ///Photo 的摘要说明
    /// </summary>
    public class Photo
    {
        static string photopath = "~/studentphoto/";
        public Photo()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        public static string GetStudentPhotoUrl(string snumstr)
        {
            string url = "~/images/nothing.gif";
            string photojpgurl = photopath + snumstr + ".jpg";
            string jpgpath = HttpContext.Current.Server.MapPath(photojpgurl);
            if (File.Exists(jpgpath))
            {
                url = photojpgurl;
            }

            return url;

        }
        public static string GetStudentPhotoUrl(string snumstr, string sexstr)
        {
            string url = "~/images/nothing.gif";
            if (sexstr == "男")
            {
                url = "~/images/boy.gif";
            }
            if (sexstr == "女")
            {
                url = "~/images/girl.gif";
            }
            string photojpgurl = photopath + snumstr + ".jpg";
            string jpgpath = HttpContext.Current.Server.MapPath(photojpgurl);
            if (File.Exists(jpgpath))
            {
                url = photojpgurl;
            }

            return url;

        }
        public static string ExistStuPhoto(string snumstr)
        {
            string photojpgurl = photopath + snumstr + ".jpg";
            if (File.Exists(HttpContext.Current.Server.MapPath(photojpgurl)))
                return "jpg";
            else
                return "none";

        }
        /// <summary>
        /// 返回值1为jpg,2为gif,0为无
        /// </summary>
        /// <param name="snumstr"></param>
        /// <returns></returns>
        public static string ExistStuPhotoIntStr(string snumstr)
        {
            string photojpgurl = photopath + snumstr + ".jpg";
            if (File.Exists(HttpContext.Current.Server.MapPath(photojpgurl)))
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        /// <summary>
        /// 保存资源上传文件
        /// </summary>
        /// <param name="FUsoft"></param>
        /// <returns></returns>
        public static string PhotoUpload(FileUpload FUphoto, string Snum)
        {
            string msg = "0";
            int whdefine = 321;//默认最大宽高
            if (FUphoto.HasFile)
            {
                string ftype = Path.GetExtension(FUphoto.PostedFile.FileName).ToLower();
                if ( ftype == ".jpg" || ftype == ".jpeg" || ftype == ".png")
                {
                    if (FUphoto.PostedFile.ContentLength < 2097152)
                    {
                        Stream streamup = FUphoto.PostedFile.InputStream;
                        try
                        {
                            System.Drawing.Image imagefile = System.Drawing.Image.FromStream(streamup);
                            int width = imagefile.Width;
                            int height = imagefile.Height;
                            string newfilename = photopath + Snum + ".jpg";
                            MakePhotoDir();

                            string newfilepath = HttpContext.Current.Server.MapPath(newfilename);

                            if (width < whdefine && height < whdefine)
                            {
                                imagefile.Save(newfilepath);
                                imagefile.Dispose();
                                msg = "1";
                            }
                            else
                            {
                                int thumbwidth = 320;
                                int thumbheight = height * 320 / width;
                                GetThumbnail(imagefile, thumbwidth, thumbheight,newfilepath);//生成高清缩略图

                                imagefile.Dispose();
                                msg = "2";
                            }
                            streamup.Close();
                        }
                        catch
                        {
                            msg = "3";
                            streamup.Close();
                            return msg;
                        }
                    }
                    else
                    {
                        msg = "4";
                    }
                }
                else
                {
                    msg = "5";
                }
            }
            return msg;
        }

        /// <summary>
        /// 为图片生成缩略图  
        /// </summary>
        /// <param name="phyPath">原图片的路径</param>
        /// <param name="width">缩略图宽</param>
        /// <param name="height">缩略图高</param>
        /// <returns></returns>
        public static void  GetThumbnail(System.Drawing.Image image, int width, int height, string newfilepath)
        {
            Bitmap bmp = new Bitmap(width, height);
            //从Bitmap创建一个System.Drawing.Graphics
            System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp);
            //设置 
            gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //下面这个也设成高质量
            gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            //下面这个设成High
            gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //把原始图像绘制成上面所设置宽高的缩小图
            System.Drawing.Rectangle rectDestination = new System.Drawing.Rectangle(0, 0, width, height);

            gr.DrawImage(image, rectDestination, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);
            bmp.Save(newfilepath,System.Drawing.Imaging.ImageFormat.Jpeg);
            bmp.Dispose();
        }

        /// <summary>
        /// 如果不存在，创建目录
        /// </summary>
        /// <param name="savepath">物理路径</param>
        public static void MakePhotoDir()
        {
            string savepath = HttpContext.Current.Server.MapPath(photopath);
            if (!Directory.Exists(savepath))
            {
                Directory.CreateDirectory(savepath);
            }
        }

        /// <summary>
        /// 获取图片的长度和宽度属性，超大图片按指定比例缩小
        /// </summary>
        public class Facephoto
        {
            private int _width;
            private int _height;
            private bool _exist = false;
            public Facephoto(string photourl)
            {
                string photopath = HttpContext.Current.Server.MapPath(photourl);
                if (File.Exists(photopath))
                {
                    try
                    {
                        System.Drawing.Image img = System.Drawing.Image.FromFile(photopath);
                        _width = img.Width;
                        _height = img.Height;
                        if (_width > 160)
                        {
                            _height = _height * 160 / _width;
                            _width = 160;
                        }
                        img.Dispose();
                        _exist = true;
                    }
                    catch
                    {
                        _exist = false;
                    }
                }
            }
            public int Width
            {
                set { _width = value; }
                get { return _width; }            
            }
            public int Height
            {
                set { _height = value; }
                get { return _height; }

            }
            public bool Exist
            {
                set { _exist = value; }
                get { return _exist; }
            }
        }
    }
}