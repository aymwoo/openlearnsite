using System;
using System.Collections.Generic;
using System.Web;

namespace LearnSite.Common
{
    /// <summary>
    ///DesCode 的摘要说明
    /// </summary>
    public class DesCode
    {
        public DesCode()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

         #region 加密解密

        public static DES s = new DES();

        public static string StringKey = "20171227"; // 加密密钥

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StringDecder(string str)
        {
            return s.DesDecrypt(str, StringKey);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string StringEncoder(string str)
        {
            return s.DesEncrypt(str, StringKey);
        }

        #endregion 

    }
}