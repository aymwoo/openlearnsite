using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Globalization;

namespace LearnSite.Common
{
    /// <summary>
    ///chathistory 的摘要说明
    /// </summary>
    public class chathistory
    {
        public  chathistory()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }
        static List<string> list = new List<string>();
        public static string chatDir = "~/Chatfile/";
        public static string chatUrl = "../Chatfile/";

        /// <summary>
        /// 获取聊天上传的物理路径，如果没有目录则创建
        /// </summary>
        /// <param name="Sid"></param>
        /// <returns></returns>
        public static string GetChatPath(string Sid)
        {
            string chatPath = HttpContext.Current.Server.MapPath(chatDir);
            if (!Directory.Exists(chatPath))
                Directory.CreateDirectory(chatPath);

            string SidPath = Flatform.Checkbdir(chatPath) + Sid;//学生id
            if (!Directory.Exists(SidPath))
                Directory.CreateDirectory(SidPath);

            return Flatform.Checkbdir(SidPath);
        }

        public class Fcupmsg {
            private int _status=0;
            private string _message="没有数据";
            private string _url = "";
            private string _file_index = "";

            public int status
            {
                get { return this._status; }
                set { this._status = value; }
            }
            public string message
            {
                get { return this._message; }
                set { this._message = value; }
            }
            public string url
            {
                get { return this._url; }
                set { this._url = value; }
            }
            public string file_index
            {
                get { return this._file_index; }
                set { this._file_index = value; }
            }
        }

        /// <summary>
        /// 返回上传的文件名
        /// </summary>
        /// <returns></returns>
        public static string UpChatFile()
        {
            Model.Cook cook = new Model.Cook();
            string sid = cook.Sid.ToString();
            string furl = chatUrl + sid;
            string fpath = GetChatPath(sid);

            Fcupmsg msg = new Fcupmsg();

            HttpPostedFile upfile = HttpContext.Current.Request.Files["file_data"];
            int flen = upfile.ContentLength;
            string fname = HttpContext.Current.Request.Form["file_name"];
            fname = LearnSite.Common.WordProcess.FilterSpecial(fname);
            string file_index = HttpContext.Current.Request.Form["file_index"];
            string file_total = HttpContext.Current.Request.Form["file_total"];
            furl = furl + "/" + fname;
            fpath = System.IO.Path.Combine(fpath, fname);
            if (flen > 0)
            {
                FileMode fMode = File.Exists(fpath) ? FileMode.Append : FileMode.Create;
                Byte[] fdata = new Byte[flen];
                Stream sr = upfile.InputStream;
                sr.Read(fdata, 0, flen);

                using (FileStream fs = new FileStream(fpath, fMode))
                {
                    fs.Position = fs.Length;
                    fs.Write(fdata, 0, fdata.Length);
                }

                msg.url = furl;
                msg.file_index = file_index;
                if (file_index == file_total)
                {
                    msg.status = 2;
                    msg.message = "上传成功";
                }
                else
                {
                    msg.status = 1;
                    msg.message = "正在上传";
                }

            }
            return JsonConvert.SerializeObject(msg);
        }



        public static int add(string dic)
        {
            list.Add(dic);
            return list.Count;
        }

        /// <summary>
        /// 读取历史记录集
        /// </summary>
        /// <returns></returns>
        public static string get()
        {
            string jsonData = "";
            if (list.Count > 0)
            {
                if (list.Count > 200)
                {
                    list.RemoveRange(0, 100); //如果记录数大于200，则移除一半               
                }

                jsonData = JsonConvert.SerializeObject(list);//把List集合转换为json字符串
            }
            return jsonData;
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


        public static string Substring(string str, int startIndex, int length)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            if (startIndex < 0)
            {
                startIndex = Math.Max(StringInfo.ParseCombiningCharacters(str).Length + startIndex, 0);
            }

            if (length < 0)
            {
                length = Math.Max(StringInfo.ParseCombiningCharacters(str).Length + length - startIndex, 0);
            }

            return new StringInfo(str).SubstringByTextElements(startIndex, length);
        }

        /// <summary>
        /// 附件类
        /// </summary>
        public class DiskInfo
        {
            private DataView _dw = null;//文件列表
            private string _durl;//网盘虚拟目录
            private string _droot = chatUrl;//网盘虚拟目录 "../Chatfile/";
            private string _dpath;//网盘物理路径
            private int _dcount = 0;//文件数
            /// <summary>
            /// 网盘虚拟目录
            /// </summary>
            /// <param name="Sgroup"></param>
            /// <param name="Snum"></param>
            public DiskInfo(string Sid)
            {
                _durl = _droot + Sid + "/";
                _dpath = GetChatPath(Sid);
                string ftypes = "jpg,png,jpeg,gif";
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
                            string[] farray = fname.Split('.');
                            string fext = getextions(fname);
                            if (!ftypes.Contains(fext))
                            {
                                string fshortname = farray[0];
                                if (fshortname.Length > 6)
                                {
                                    fshortname = Substring(fshortname, 0, 6);
                                }

                                row[0] = fshortname; //文件名
                                float fl = fi.Length;
                                dsizes = dsizes + fl;
                                row[1] = (fl / 1024).ToString("0") + "kb";//大小
                                row[2] = _durl + fname;//超连接
                                row[3] = fi.CreationTime;//创建日期
                                row[4] = "../images/FileType/" + fext + ".gif";//文件类型图标
                                ds.Tables[0].Rows.Add(row);
                                _dcount++;
                            }
                        }
                        ds.AcceptChanges();
                        _dw = ds.Tables[0].DefaultView;
                        _dw.Sort = "fdate desc";
                    }
                    ds.Dispose();
                }
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
