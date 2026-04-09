using System;
using System.Web;
namespace LearnSite.Model
{
    /// <summary>
    /// 实体类Courses 
    /// </summary>
    [Serializable]
    /// <summary>
    ///Cook 的摘要说明
    /// </summary>
    public class Cook
    {
        public Cook()
        {
            if (HttpContext.Current.Request.Cookies[LearnSite.Common.CookieHelp.stuCookieNname] != null)
            {
                string scook = HttpContext.Current.Request.Cookies[LearnSite.Common.CookieHelp.stuCookieNname].Value;
                this.ToModel(scook);
            }
        }

        #region Model
        private int _sid;
        private string _snum;
        private int _syear;
        private int _sgrade;
        private int _sclass;
        private string _sname;
        private string _spwd;
        private string _sex;
        private int _sscore;
        private int _squiz;
        private int _sattitude;
        private string _sape;
        private int _sgroup;//其他，组号
        private string _logintime;
        private string _loginip;
        private int _thisterm;
        private int _rhid;
        private string _rankimage;
        private string _ss;//标记为cookie名称
        private string _sessionid;

        /// <summary>
        /// cookie是否存在
        /// </summary>
        /// <returns></returns>
        public bool IsExist()
        {
            if (string.IsNullOrEmpty(_ss))
                return false;
            else
                return true;
        }
        /// <summary>
        /// 是否模拟学生
        /// </summary>
        /// <returns></returns>
        public bool IsSimiStu()
        {
            if (_sid > 0)
                return false;
            else
                return true;
        }
        /// <summary>
        ///模型转字符串
        /// </summary>
        /// <returns></returns>
        public string ToStr()
        {
            string value = _sid.ToString() + "|" + _snum + "|" + _syear.ToString() + "|" + _sgrade.ToString()
                + "|" + _sclass.ToString() + "|" + _sname + "|" + _spwd + "|" + _sex + "|" + _sscore.ToString() 
                + "|" + _squiz.ToString() + "|" + _sattitude.ToString() + "|" + _sape + "|" + _sgroup.ToString() 
                + "|" + _logintime+ "|" + _loginip + "|" + _thisterm.ToString() + "|" + _rhid.ToString() 
                + "|" + _rankimage + "|" + _ss + "|" + _sessionid;

            string result =Common.DesCode.StringEncoder(value);//Base64加密
            return result;
        }
        /// <summary>
        /// 字符串转模型
        /// </summary>
        /// <param name="ciphervalue"></param>
        private void ToModel(string ciphervalue)
        {
            try
            {
                string value =Common.DesCode.StringDecder(ciphervalue);
                string[] result = value.Split('|');
                if (result.Length == 20)
                {
                    _sid = Int32.Parse(result[0]);
                    _snum = result[1];
                    _syear = Int32.Parse(result[2]);
                    _sgrade = Int32.Parse(result[3]);
                    _sclass = Int32.Parse(result[4]);
                    _sname = result[5];
                    _spwd = result[6];
                    _sex = result[7];
                    _sscore = Int32.Parse(result[8]);
                    _squiz = Int32.Parse(result[9]);
                    _sattitude = Int32.Parse(result[10]);
                    _sape = result[11];
                    _sgroup = Int32.Parse(result[12]);//其他
                    _logintime = result[13];
                    _loginip = result[14];
                    _thisterm = Int32.Parse(result[15]);
                    _rhid = Int32.Parse(result[16]);
                    _rankimage = result[17];
                    _ss = result[18];
                    _sessionid = result[19];
                }
                else
                    LearnSite.Common.CookieHelp.ClearStudentCookies();
            }
            catch
            {
                LearnSite.Common.CookieHelp.ClearStudentCookies();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Sid
        {
            set { _sid = value; }
            get { return _sid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Snum
        {
            set { _snum = value; }
            get { return _snum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Syear
        {
            set { _syear = value; }
            get { return _syear; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Sgrade
        {
            set { _sgrade = value; }
            get { return _sgrade; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Sclass
        {
            set { _sclass = value; }
            get { return _sclass; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Sname
        {
            set { _sname = value; }
            get { return _sname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Spwd
        {
            set { _spwd = value; }
            get { return _spwd; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Sex
        {
            set { _sex = value; }
            get { return _sex; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Sscore
        {
            set { _sscore = value; }
            get { return _sscore; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Squiz
        {
            set { _squiz = value; }
            get { return _squiz; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Sattitude
        {
            set { _sattitude = value; }
            get { return _sattitude; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Sape
        {
            set { _sape = value; }
            get { return _sape; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Sgroup
        {
            set { _sgroup = value; }
            get { return _sgroup; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LoginTime
        {
            set { _logintime = value; }
            get { return _logintime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string LoginIp
        {
            set { _loginip = value; }
            get { return _loginip; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ThisTerm
        {
            set { _thisterm = value; }
            get { return _thisterm; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Rhid
        {
            set { _rhid = value; }
            get { return _rhid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string RankImage
        {
            set { _rankimage = value; }
            get { return _rankimage; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Ss
        {
            set { _ss = value; }
            get { return _ss; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SessionId
        {
            set { _sessionid = value; }
            get { return _sessionid; }
        }
        #endregion Model
    }
}