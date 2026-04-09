using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;

namespace LearnSite.Common
{
    /// <summary>
    ///ShareDisk 的摘要说明
    /// </summary>
    public class ShareDisk
    {
        public ShareDisk()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        public static string shareDir = "~/ShareDisk/";
        public static string webDir = "~/website/";
        private static int sharelimit = Common.XmlHelp.ShareDiskLimit();
        private static string webPath = HttpContext.Current.Server.MapPath(webDir);


        /// <summary>
        /// 获取网页空间的物理路径，如果没有目录则创建
        /// </summary>
        /// <param name="Snum"></param>
        /// <returns></returns>
        public static string GetWebPath(string Snum)
        {
            //string webPath = HttpContext.Current.Server.MapPath(webDir);
            if (!Directory.Exists(webPath))
                Directory.CreateDirectory(webPath);

            string Dirpath = Flatform.Checkbdir(webPath) + Snum;//学号或组号物理路径
            if (!Directory.Exists(Dirpath))
                Directory.CreateDirectory(Dirpath);

            return Flatform.Checkbdir(Dirpath);
        }

        /// <summary>
        /// 获取网盘的物理路径，如果没有目录则创建
        /// </summary>
        /// <param name="Syear">入学年份</param>
        /// <param name="Sclass">班级</param>
        /// <param name="Dir">学号或组号</param>
        /// <returns></returns>
        public static string GetSharePath(string Syear, string Sclass, string Dir)
        {
            string sharePath = HttpContext.Current.Server.MapPath(shareDir);
            if (!Directory.Exists(sharePath))
                Directory.CreateDirectory(sharePath);

            string SyearPath = Flatform.Checkbdir(sharePath) + Syear;//入学年份物理路径
            if (!Directory.Exists(SyearPath))
                Directory.CreateDirectory(SyearPath);

            string Sclasspath = Flatform.Checkbdir(SyearPath) + Sclass;//班级物理路径
            if (!Directory.Exists(Sclasspath))
                Directory.CreateDirectory(Sclasspath);

            string Dirpath = Flatform.Checkbdir(Sclasspath)+ Dir;//学号或组号物理路径
            if (!Directory.Exists(Dirpath))
                Directory.CreateDirectory(Dirpath);
            return Flatform.Checkbdir(Dirpath);
        }

        /// <summary>
        /// 获取网盘的物理路径，如果没有目录则创建
        /// </summary>
        /// <param name="Syear">入学年份</param>
        /// <param name="Sclass">班级</param>
        /// <param name="Dir">学号或组号</param>
        /// <returns></returns>
        public static string GetSharePath(string Syear, string Dir)
        {
            string sharePath = HttpContext.Current.Server.MapPath(shareDir);
            if (!Directory.Exists(sharePath))
                Directory.CreateDirectory(sharePath);

            string SyearPath = Flatform.Checkbdir(sharePath) + Syear;//入学年份物理路径
            if (!Directory.Exists(SyearPath))
                Directory.CreateDirectory(SyearPath);

            string Dirpath = Flatform.Checkbdir(SyearPath) + Dir;//学号或组号物理路径
            if (!Directory.Exists(Dirpath))
                Directory.CreateDirectory(Dirpath);
            return Flatform.Checkbdir(Dirpath);
        }

        /// <summary>
        /// 返回短文件名（无后缀）
        /// </summary>
        /// <param name="fname"></param>
        /// <returns></returns>
        private static string getsinglefilename(string fname)
        {
            int ln = fname.LastIndexOf(".");
            string mypath = fname.Substring(0, ln);
            return mypath;
        }
        /// <summary>
        /// 取扩展名（如jpg）不含点
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static string getextions(string filename)
        {
            string ext = Path.GetExtension(filename).ToLower();
            return ext.Replace(".", "");
        }
        /// <summary>
        /// 返回物理路径中的文件名称（3011006_92_171_8_122）
        /// </summary>
        /// <param name="fname">物理路径</param>
        /// <returns></returns>
        public static string getpathfilename(string fname)
        {
            return Path.GetFileNameWithoutExtension(fname);
        }

        public static string getStr(string s, int l, string endStr)
        {
            string temp = s.Substring(0, (s.Length < l) ? s.Length : l);

            if (Regex.Replace(temp, "[\u4e00-\u9fa5]", "zz", RegexOptions.IgnoreCase).Length <= l)
            {
                return temp;
            }
            for (int i = temp.Length; i >= 0; i--)
            {
                temp = temp.Substring(0, i);
                if (Regex.Replace(temp, "[\u4e00-\u9fa5]", "zz", RegexOptions.IgnoreCase).Length <= l - endStr.Length)
                {
                    return temp + endStr;
                }
            }
            return endStr;
        }

        /// <summary>
        /// 
        /// </summary>
        public static string SaveWebNew()
        {
            Model.Cook cook = new Model.Cook();
            string result = "未知错误！";
            if (cook != null)
            {
                HttpPostedFile file = HttpContext.Current.Request.Files["file"];
                int flen = file.ContentLength;
                    string savefile = file.FileName;
                    string shortf = getpathfilename(savefile);
                    shortf = LearnSite.Common.WordProcess.FilterSpecial(shortf);
                    if (shortf.Length > 12) {
                        shortf = shortf.Substring(0,12);
                    }
                    string ftype = getextions(savefile);

                        string Sname = LearnSite.Common.WordProcess.FilterSpecial(HttpUtility.UrlDecode(cook.Sname));
                        string newfile = shortf + "." + ftype;

                        string savepath = GetWebPath(cook.Snum) + newfile;
                  
                        try
                        {
                            file.SaveAs(savepath);
                            result = "保存" + shortf + "文件到网页空间成功!";
                            Model.ShareDisk kmodel = new Model.ShareDisk();
                            kmodel.Kown = false;//是否小组文档
                            kmodel.Kyear = cook.Syear;
                            kmodel.Kgrade = cook.Sgrade;
                            kmodel.Kclass = cook.Sclass;
                            kmodel.Kgroup = 0;
                            kmodel.Knum = cook.Snum;
                            kmodel.Kname = Sname;
                            kmodel.Kfilename = newfile;
                            kmodel.Kfsize = flen;
                            kmodel.Kfurl = webDir + cook.Snum + "/" + newfile;
                            kmodel.Kftpe = ftype;
                            kmodel.Kfdate = DateTime.Now;
                            BLL.ShareDisk kbll = new BLL.ShareDisk();
                            kbll.Add(kmodel);//记录到数据库
                        }
                        catch
                        {
                            string msgtype = "网页空间上传出错";
                            string msg = "当前上传路径为" + savepath;
                            LearnSite.Common.Log.Addlog(msgtype, msg);
                        }


            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="FileUp"></param>
        /// <param name="Syear"></param>
        /// <param name="Sclass"></param>
        /// <param name="Sname"></param>
        /// <param name="Snum"></param>
        /// <param name="Sgroup"></param>
        /// <param name="isGroup"></param>
        /// <returns></returns>
        public static string SaveFileNew(bool isGroup,bool isCommon)
        {
            Model.Cook cook = new Model.Cook();
            string result = "未知错误！";
            if (cook != null)
            {
                BLL.Students sbll = new BLL.Students();
                int Sgroup= sbll.GetSgroup(cook.Sid);//获取自己的组号
                string Dir = cook.Snum;
                if (isGroup)
                    Dir = Sgroup.ToString();
                if(isCommon)
                {
                    Dir="tea";
                    Sgroup=-1;
                }
                string limitType = "exe|asp|aspx|asxh|php";
                HttpPostedFile file = HttpContext.Current.Request.Files["file"];
                int flen = file.ContentLength;
                int k = 1024;
                if (flen / k / k < sharelimit)
                {
                    string savefile = file.FileName;
                    string shortf = getpathfilename(savefile);
                    //shortf = getStr(shortf, 16, "");
                    shortf = LearnSite.Common.WordProcess.FilterSpecial(shortf);
                    string ftype = getextions(savefile);
                    if (!limitType.Contains(ftype))//不是限制类型
                    {
                        string Sname = LearnSite.Common.WordProcess.FilterSpecial(HttpUtility.UrlDecode(cook.Sname));
                        string newfile = shortf + "." + ftype;
                        if (isGroup) newfile = shortf + "_" + Sname.Trim() + "." + ftype;

                        string savepath = "";
                        if (isCommon)
                        {
                            savepath = GetSharePath(cook.Syear.ToString(), Dir) + newfile;
                        }
                        else
                        {
                            savepath = GetSharePath(cook.Syear.ToString(), cook.Sclass.ToString(), Dir) + newfile;
                        }
                        try
                        {
                            file.SaveAs(savepath);
                            result = "保存" + shortf + "文件到网盘成功!";
                            Model.ShareDisk kmodel = new Model.ShareDisk();
                            kmodel.Kown = isGroup;//是否小组文档
                            kmodel.Kyear = cook.Syear;
                            kmodel.Kgrade = cook.Sgrade;
                            kmodel.Kclass = cook.Sclass;
                            kmodel.Kgroup = Sgroup;
                            kmodel.Knum = cook.Snum;
                            kmodel.Kname = Sname;
                            kmodel.Kfilename = newfile;
                            kmodel.Kfsize = flen;
                            if (isCommon)
                            {
                                kmodel.Kfurl = shareDir + cook.Syear + "/"  + Dir + "/" + newfile;
                            }
                            else
                            {
                                kmodel.Kfurl = shareDir + cook.Syear + "/" + cook.Sclass + "/" + Dir + "/" + newfile;
                            }
                            kmodel.Kftpe = ftype;
                            kmodel.Kfdate = DateTime.Now;
                            BLL.ShareDisk kbll = new BLL.ShareDisk();
                            kbll.Add(kmodel);//记录到数据库
                        }
                        catch
                        {
                            string msgtype = "网盘上传出错";
                            string msg = "当前上传路径为" + savepath;
                            LearnSite.Common.Log.Addlog(msgtype, msg);
                        }
                    }
                    else
                    {
                        result = "保存失败！文件类型不能为" + limitType;
                    }
                }
                else
                {
                    result = "上传的文件大小不能超过" + sharelimit.ToString() + "MB!";
                }
            }
            return result;
        }


        public static bool DelFile(string furl)
        {
            bool isok = true;
            if (!string.IsNullOrEmpty(furl))
            {
                try
                {
                    string fpath = HttpContext.Current.Server.MapPath(furl);
                    File.Delete(fpath);
                }
                catch
                {
                    isok = false;
                }
            }
            return isok;
        }

        /// <summary>
        /// 网盘类
        /// </summary>
        public class DiskInfoNew
        {
            private DataView _dw = null;//文件列表
            private double _dsize = 0;//网盘空间
            private int _dlimit = sharelimit;//网盘上限
            private double _dleft = 0;//网盘剩余
            private string _durl;//网盘虚拟目录
            private string _droot = shareDir;//网盘根虚拟目录 "~/ShareDisk/";
            private string _dpath;//网盘物理路径
            private bool _dupload = true;
            private int _dcount = 0;//文件数
            /// <summary>
            /// 网盘虚拟目录
            /// </summary>
            /// <param name="Sgroup"></param>
            /// <param name="Snum"></param>
            public DiskInfoNew(string Syear, string Sclass, string Dir, bool IsGroup)
            {
                if (IsGroup)
                    _dlimit = sharelimit * 2;
                _durl = _droot + Syear + "/" + Sclass + "/" + Dir + "/";
                _dpath = GetSharePath(Syear, Sclass, Dir);
                if (!string.IsNullOrEmpty(_durl))
                {
                    BLL.ShareDisk bll = new BLL.ShareDisk();

                    DataTable dt = new DataTable();
                    if (!IsGroup)
                        dt = bll.GetSnumListall(Dir);
                    else
                        dt = bll.GetGroupListall(Dir);

                    _dw = dt.DefaultView;

                    _dcount = dt.Rows.Count;
                    int dsizes = 0;
                    object kfsize = dt.Compute("SUM(Kfsize)", "");

                    //LearnSite.Common.Log.Addlog("网盘占用求和出错:", kfsize.ToString()+"结束");
                    if (kfsize != DBNull.Value)
                        dsizes = Convert.ToInt32(kfsize);
                    double k = 1024;
                    _dsize = dsizes / k / k;
                    _dleft = _dlimit - _dsize;
                    if (_dleft < 0)
                        _dupload = false;//如果空余空间不足，则不能上传

                    dt.Dispose();
                }
            }

            /// <summary>
            /// 网盘已占用MB
            /// </summary>
            public double Dsize
            {
                set { _dsize = value; }
                get { return _dsize; }
            }
            /// <summary>
            /// 网盘存储上限MB
            /// </summary>
            public int Dlimit
            {
                set { _dlimit = value; }
                get { return _dlimit; }
            }
            /// <summary>
            /// 网盘剩余
            /// </summary>
            public double Dleft
            {
                set { _dleft = value; }
                get { return _dleft; }
            }
            /// <summary>
            /// 网盘虚拟目录
            /// </summary>
            public string Durl
            {
                set { _durl = value; }
                get { return _durl; }
            }
            /// <summary>
            /// 网盘根虚拟目录
            /// </summary>
            public string Droot
            {
                set { _droot = value; }
                get { return _droot; }
            }
            /// <summary>
            /// 网盘物理路径
            /// </summary>
            public string Dpath
            {
                set { _dpath = value; }
                get { return _dpath; }
            }
            /// <summary>
            /// 是否可上传
            /// </summary>
            public bool Dupload
            {
                set { _dupload = value; }
                get { return _dupload; }
            }
            /// <summary>
            /// 文件数
            /// </summary>
            public int Dcount
            {
                set { _dcount = value; }
                get { return _dcount; }
            }
            /// <summary>
            /// 网盘文件列表
            /// </summary>
            public DataView Dw
            {
                set { _dw = value; }
                get { return _dw; }
            }
        }

        /// <summary>
        /// 网盘类
        /// </summary>
        public class DiskInfoCommon
        {
            private DataView _dw = null;//文件列表
            private double _dsize = 0;//网盘空间
            private int _dlimit = sharelimit;//网盘上限
            private double _dleft = 0;//网盘剩余
            private string _durl;//网盘虚拟目录
            private string _droot = shareDir;//网盘根虚拟目录 "~/ShareDisk/";
            private string _dpath;//网盘物理路径
            private bool _dupload = true;
            private int _dcount = 0;//文件数

            /// <summary>
            /// 
            /// </summary>
            /// <param name="Syear"></param>
            /// <param name="Sclass"></param>
            public  DiskInfoCommon(string Syear)
            {
                _dlimit = sharelimit * 5;
                string Dir="tea";
                _durl = _droot + Syear + "/"  + Dir + "/";
                _dpath = GetSharePath(Syear, Dir);
                if (!string.IsNullOrEmpty(_durl))
                {
                    BLL.ShareDisk bll = new BLL.ShareDisk();

                    DataTable dt = new DataTable();

                    dt = bll.GetGroupListall("-1");

                    _dw = dt.DefaultView;

                    _dcount = dt.Rows.Count;
                    int dsizes = 0;
                    object kfsize = dt.Compute("SUM(Kfsize)", "");

                    if (kfsize != DBNull.Value)
                        dsizes = Convert.ToInt32(kfsize);
                    double k = 1024;
                    _dsize = dsizes / k / k;
                    _dleft = _dlimit - _dsize;
                    if (_dleft < 0)
                        _dupload = false;//如果空余空间不足，则不能上传

                    dt.Dispose();
                }
            }

            /// <summary>
            /// 网盘已占用MB
            /// </summary>
            public double Dsize
            {
                set { _dsize = value; }
                get { return _dsize; }
            }
            /// <summary>
            /// 网盘存储上限MB
            /// </summary>
            public int Dlimit
            {
                set { _dlimit = value; }
                get { return _dlimit; }
            }
            /// <summary>
            /// 网盘剩余
            /// </summary>
            public double Dleft
            {
                set { _dleft = value; }
                get { return _dleft; }
            }
            /// <summary>
            /// 网盘虚拟目录
            /// </summary>
            public string Durl
            {
                set { _durl = value; }
                get { return _durl; }
            }
            /// <summary>
            /// 网盘根虚拟目录
            /// </summary>
            public string Droot
            {
                set { _droot = value; }
                get { return _droot; }
            }
            /// <summary>
            /// 网盘物理路径
            /// </summary>
            public string Dpath
            {
                set { _dpath = value; }
                get { return _dpath; }
            }
            /// <summary>
            /// 是否可上传
            /// </summary>
            public bool Dupload
            {
                set { _dupload = value; }
                get { return _dupload; }
            }
            /// <summary>
            /// 文件数
            /// </summary>
            public int Dcount
            {
                set { _dcount = value; }
                get { return _dcount; }
            }
            /// <summary>
            /// 网盘文件列表
            /// </summary>
            public DataView Dw
            {
                set { _dw = value; }
                get { return _dw; }
            }
        }


        /// <summary>
        /// 网盘类
        /// </summary>
        public class DiskInfo
        {
            private DataView _dw = null;//文件列表
            private float _dsize = 0;//网盘空间
            private int _dlimit = sharelimit;//网盘上限
            private float _dleft = 0;//网盘剩余
            private string _durl;//网盘虚拟目录
            private string _droot = shareDir;//网盘根虚拟目录 "~/ShareDisk/";
            private string _dpath;//网盘物理路径
            private bool _dupload = true;
            private int _dcount = 0;//文件数
            /// <summary>
            /// 网盘虚拟目录
            /// </summary>
            /// <param name="Sgroup"></param>
            /// <param name="Snum"></param>
            public DiskInfo(string Syear, string Sclass, string Dir, bool IsGroup)
            {
                if (IsGroup)
                    _dlimit = sharelimit * 2;
                _durl = _droot + Syear + "/" + Sclass + "/" + Dir + "/";
                _dpath = GetSharePath(Syear, Sclass, Dir);
                if (!string.IsNullOrEmpty(_durl))
                {
                    DataSet ds = new DataSet();
                    ds.Tables.Add();
                    ds.Tables[0].TableName = "disktable";
                    ds.Tables[0].Columns.Add("fname", typeof(String));
                    ds.Tables[0].Columns.Add("fsize", typeof(String));
                    ds.Tables[0].Columns.Add("furl", typeof(String));
                    ds.Tables[0].Columns.Add("fdate", typeof(DateTime));
                    ds.Tables[0].Columns.Add("ftype", typeof(String));
                    if (Directory.Exists(_dpath))
                    {
                        DirectoryInfo di = new DirectoryInfo(_dpath);
                        FileInfo[] fis = di.GetFiles();
                        float dsizes = 0;
                        foreach (FileInfo fi in fis)
                        {
                            DataRow row;
                            row = ds.Tables[0].NewRow();
                            string fname = fi.Name;
                            string fext = getextions(fname);

                            row[0] = fname; //文件名
                            float fl = fi.Length;
                            dsizes = dsizes + fl;
                            row[1] = (fl / 1024).ToString("0") + "kb";//大小
                            row[2] = _durl + fname;//超连接
                            row[3] = fi.CreationTime;//创建日期
                            row[4] = "~/images/FileType/" + fext + ".gif";//文件类型图标
                            ds.Tables[0].Rows.Add(row);
                            _dcount++;
                        }
                        ds.AcceptChanges();
                        _dw = ds.Tables[0].DefaultView;
                        _dw.Sort = "fdate desc";
                        float k = 1024;
                        _dsize = dsizes / k / k;
                        _dleft = _dlimit - _dsize;
                        if (_dleft < 0)
                            _dupload = false;//如果空余空间不足，则不能上传
                    }
                    ds.Dispose();
                }
            }
            /// <summary>
            /// 网盘已占用MB
            /// </summary>
            public float Dsize
            {
                set { _dsize = value; }
                get { return _dsize; }
            }
            /// <summary>
            /// 网盘存储上限MB
            /// </summary>
            public int Dlimit
            {
                set { _dlimit = value; }
                get { return _dlimit; }
            }
            /// <summary>
            /// 网盘剩余
            /// </summary>
            public float Dleft
            {
                set { _dleft = value; }
                get { return _dleft; }
            }
            /// <summary>
            /// 网盘虚拟目录
            /// </summary>
            public string Durl
            {
                set { _durl = value; }
                get { return _durl; }
            }
            /// <summary>
            /// 网盘根虚拟目录
            /// </summary>
            public string Droot
            {
                set { _droot = value; }
                get { return _droot; }
            }
            /// <summary>
            /// 网盘物理路径
            /// </summary>
            public string Dpath
            {
                set { _dpath = value; }
                get { return _dpath; }
            }
            /// <summary>
            /// 是否可上传
            /// </summary>
            public bool Dupload
            {
                set { _dupload = value; }
                get { return _dupload; }
            }
            /// <summary>
            /// 文件数
            /// </summary>
            public int Dcount
            {
                set { _dcount = value; }
                get { return _dcount; }
            }
            /// <summary>
            /// 网盘文件列表
            /// </summary>
            public DataView Dw
            {
                set { _dw = value; }
                get { return _dw; }
            }
        }
        
        
        public static string extGif(string ext)
        {
            string worktype = LearnSite.Common.XmlHelp.GetTypeName("WorksType");
            if (worktype.Contains(ext))
                return ext;
            else
                return "unknown";
        }


        /// <summary>
        /// 网盘类
        /// </summary>
        public class WebInfo
        {
            private DataView _dw = null;//文件列表
            private float _dsize = 0;//网盘空间
            private int _dlimit = sharelimit;//网盘上限
            private float _dleft = 0;//网盘剩余
            private string _durl;//网盘虚拟目录
            private string _droot = webDir;//网盘根虚拟目录 "~/website/";
            private string _dpath;//网盘物理路径
            private bool _dupload = true;
            private int _dcount = 0;//文件数
            /// <summary>
            /// 网盘虚拟目录
            /// </summary>
            /// <param name="Sgroup"></param>
            /// <param name="Snum"></param>
            public WebInfo(string Snum)
            {
                _durl = _droot + Snum + "/";
                _dpath = GetWebPath(Snum);
                if (!string.IsNullOrEmpty(_durl))
                {
                    DataSet ds = new DataSet();
                    ds.Tables.Add();
                    ds.Tables[0].TableName = "disktable";
                    ds.Tables[0].Columns.Add("KfnameShort", typeof(String));
                    ds.Tables[0].Columns.Add("Kfsize", typeof(String));
                    ds.Tables[0].Columns.Add("Kfurl", typeof(String));
                    ds.Tables[0].Columns.Add("Kfdate", typeof(DateTime));
                    ds.Tables[0].Columns.Add("Kftpe", typeof(String));
                    ds.Tables[0].Columns.Add("Kfnum", typeof(String));
                    if (Directory.Exists(_dpath))
                    {
                        DirectoryInfo di = new DirectoryInfo(_dpath);
                        FileInfo[] fis = di.GetFiles();
                        float dsizes = 0;
                        foreach (FileInfo fi in fis)
                        {
                            DataRow row;
                            row = ds.Tables[0].NewRow();
                            string fname = fi.Name;
                            string fext = getextions(fname);

                            row[0] = fname; //文件名
                            float fl = fi.Length;
                            dsizes = dsizes + fl;
                            row[1] = (fl / 1024).ToString("0") + "kb";//大小
                            row[2] = _durl + fname;//超连接
                            row[3] = fi.CreationTime;//创建日期
                            row[4] = "~/images/FileType/" + fext + ".gif";//文件类型图标
                            switch (fext) {
                                case "png":
                                case "gif":
                                case "jpg":
                                case "jpeg":
                                    row[4] = row[2];
                                    break;                            
                            }
                            if (fext == "html")
                            {
                                row[5] = "";
                            }
                            else {
                                row[5] = Snum;
                            }
                            if (fext != "ico")
                            {
                                ds.Tables[0].Rows.Add(row);
                                _dcount++;
                            }
                            
                        }

                        DirectoryInfo ri = new DirectoryInfo(webPath);
                        FileInfo[] ris = ri.GetFiles();
                        foreach (FileInfo fi in ris)
                        {
                            DataRow row;
                            row = ds.Tables[0].NewRow();
                            string fname = fi.Name;
                            string fext = getextions(fname);
                            if (fext != "html")
                            {
                                row[0] = fname; //文件名
                                float fl = fi.Length;
                                dsizes = dsizes + fl;
                                row[1] = (fl / 1024).ToString("0") + "kb";//大小
                                row[2] = _droot + fname;//超连接
                                row[3] = fi.CreationTime;//创建日期
                                row[4] = "~/images/FileType/" + fext + ".gif";//文件类型图标
                                row[5] = "";
                                ds.Tables[0].Rows.Add(row);
                                _dcount++;
                            }
                        }

                        ds.AcceptChanges();
                        _dw = ds.Tables[0].DefaultView;
                        _dw.Sort = "Kftpe asc";
                        float k = 1024;
                        _dsize = dsizes / k / k;
                        _dleft = _dlimit - _dsize;
                        if (_dleft < 0)
                            _dupload = false;//如果空余空间不足，则不能上传
                    }
                    ds.Dispose();
                }
            }
            /// <summary>
            /// 网盘已占用MB
            /// </summary>
            public float Dsize
            {
                set { _dsize = value; }
                get { return _dsize; }
            }
            /// <summary>
            /// 网盘存储上限MB
            /// </summary>
            public int Dlimit
            {
                set { _dlimit = value; }
                get { return _dlimit; }
            }
            /// <summary>
            /// 网盘剩余
            /// </summary>
            public float Dleft
            {
                set { _dleft = value; }
                get { return _dleft; }
            }
            /// <summary>
            /// 网盘虚拟目录
            /// </summary>
            public string Durl
            {
                set { _durl = value; }
                get { return _durl; }
            }
            /// <summary>
            /// 网盘根虚拟目录
            /// </summary>
            public string Droot
            {
                set { _droot = value; }
                get { return _droot; }
            }
            /// <summary>
            /// 网盘物理路径
            /// </summary>
            public string Dpath
            {
                set { _dpath = value; }
                get { return _dpath; }
            }
            /// <summary>
            /// 是否可上传
            /// </summary>
            public bool Dupload
            {
                set { _dupload = value; }
                get { return _dupload; }
            }
            /// <summary>
            /// 文件数
            /// </summary>
            public int Dcount
            {
                set { _dcount = value; }
                get { return _dcount; }
            }
            /// <summary>
            /// 网盘文件列表
            /// </summary>
            public DataView Dw
            {
                set { _dw = value; }
                get { return _dw; }
            }
        }





    }
}