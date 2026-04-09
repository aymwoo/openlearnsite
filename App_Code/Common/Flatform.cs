using System;
using System.Collections.Generic;
using System.Web;
using System.IO;
namespace LearnSite.Common
{
    /// <summary>
    /// Flatform 的摘要说明 - 跨平台路径处理工具类
    /// </summary>
    public class Flatform
    {
        public Flatform()
        {
        }
        
        /// <summary>
        /// 判断是否为 Unix/Linux 系统
        /// </summary>
        public static bool isUnix
        {
            get
            {
                var platform = Environment.OSVersion.Platform;
                return platform == PlatformID.Unix || platform == PlatformID.MacOSX;
            }
        }

        /// <summary>
        /// 获取跨平台路径分隔符
        /// </summary>
        public static char PathSeparator
        {
            get { return Path.DirectorySeparatorChar; }
        }

        /// <summary>
        /// 检验物理路径最后一个字符是否缺少路径分隔符，缺少则添加
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Checkbdir(string str)
        {
            char separator = Path.DirectorySeparatorChar;
            if (!str.EndsWith(separator.ToString()))
            {
                str = str + separator;
            }
            return str;
        }

        /// <summary>
        /// 合并路径（跨平台安全）
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string CombinePath(params string[] paths)
        {
            return Path.Combine(paths);
        }

        /// <summary>
        /// 获取文件名（从完整路径中提取，跨平台安全）
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string GetFileName(string fullPath)
        {
            return Path.GetFileName(fullPath);
        }

        /// <summary>
        /// 获取目录路径（从完整路径中提取，跨平台安全）
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string GetDirectoryName(string fullPath)
        {
            return Path.GetDirectoryName(fullPath);
        }

    }
}
