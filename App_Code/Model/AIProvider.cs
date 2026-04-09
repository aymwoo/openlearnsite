using System;

namespace LearnSite.Model
{
    /// <summary>
    /// AIProvider:实体类
    /// </summary>
    [Serializable]
    public class AIProvider
    {
        public AIProvider()
        {}

        #region Model
        private int _id;
        private string _displayname;
        private string _providername;
        private string _modelname;
        private string _apikey;
        private string _baseurl;
        private bool _isdefault;

        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayName
        {
            set { _displayname = value; }
            get { return _displayname; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ProviderName
        {
            set { _providername = value; }
            get { return _providername; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ModelName
        {
            set { _modelname = value; }
            get { return _modelname; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ApiKey
        {
            set { _apikey = value; }
            get { return _apikey; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string BaseUrl
        {
            set { _baseurl = value; }
            get { return _baseurl; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDefault
        {
            set { _isdefault = value; }
            get { return _isdefault; }
        }
        #endregion Model
    }
}
