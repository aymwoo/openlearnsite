/**********************************************
 * 类作用：   执行命令类
 * 建立人：   abaal
 * 建立时间： 2008-09-03 
 * Copyright (C) 2007-2008 abaal
 * All rights reserved
 * http://blog.csdn.net/abaal888
 * 
 * 修改说明：添加跨平台支持
 ***********************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LearnSite.Common
{
    /// <summary>
    /// 执行命令 - 跨平台支持
    /// </summary>
    public class CmdUtil
    {
        /// <summary>
        /// 判断是否为 Windows 系统
        /// </summary>
        public static bool IsWindows
        {
            get
            {
                return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            }
        }

        /// <summary>
        /// 获取系统 Shell 命令
        /// </summary>
        private static string ShellCommand
        {
            get
            {
                return IsWindows ? "cmd.exe" : "/bin/bash";
            }
        }

        /// <summary>
        /// 获取 Shell 参数
        /// </summary>
        private static string ShellArgument
        {
            get
            {
                return IsWindows ? "/c" : "-c";
            }
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="commandText">命令文本</param>
        /// <returns>命令输出文本</returns>
        public static string ExeCommand(string commandText)
        {
            return ExeCommand(new string[] { commandText });
        }

        /// <summary>
        /// 执行多条命令
        /// </summary>
        /// <param name="commandTexts">命令文本数组</param>
        /// <returns>命令输出文本</returns>
        public static string ExeCommand(string[] commandTexts)
        {
            string strOutput = null;
            using (Process p = new Process())
            {
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                try
                {
                    p.Start();
                    foreach (string item in commandTexts)
                    {
                        p.StandardInput.WriteLine(item);
                    }
                    p.StandardInput.WriteLine("exit");
                    p.StandardInput.Flush();
                    p.StandardInput.Close();
                    strOutput = p.StandardOutput.ReadToEnd();
                    //strOutput = Encoding.UTF8.GetString(Encoding.Default.GetBytes(strOutput));
                    p.WaitForExit();
                }
                catch (Exception e)
                {
                    strOutput = e.Message;
                }
            }
            return strOutput;
        }

        /// <summary>
        /// 启动外部应用程序，隐藏程序界面
        /// </summary>
        /// <param name="appName">应用程序路径名称</param>
        /// <returns>true表示成功，false表示失败</returns>
        public static bool StartApp(string appName)
        {
            return StartApp(appName, ProcessWindowStyle.Hidden);
        }

        /// <summary>
        /// 启动外部应用程序
        /// </summary>
        /// <param name="appName">应用程序路径名称</param>
        /// <param name="style">进程窗口模式</param>
        /// <returns>true表示成功，false表示失败</returns>
        public static bool StartApp(string appName, ProcessWindowStyle style)
        {
            return StartApp(appName, null, style);
        }

        /// <summary>
        /// 启动外部应用程序，隐藏程序界面
        /// </summary>
        /// <param name="appName">应用程序路径名称</param>
        /// <param name="arguments">启动参数</param>
        /// <returns>true表示成功，false表示失败</returns>
        public static bool StartApp(string appName, string arguments)
        {
            return StartApp(appName, arguments, ProcessWindowStyle.Hidden);
        }

        /// <summary>
        /// 启动外部应用程序
        /// </summary>
        /// <param name="appName">应用程序路径名称</param>
        /// <param name="arguments">启动参数</param>
        /// <param name="style">进程窗口模式</param>
        /// <returns>true表示成功，false表示失败</returns>
        public static bool StartApp(string appName, string arguments, ProcessWindowStyle style)
        {
            bool blnRst = false;
            using (Process p = new Process())
            {
                p.StartInfo.FileName = appName;//exe,bat and so on
                p.StartInfo.WindowStyle = style;
                p.StartInfo.Arguments = arguments;
                try
                {
                    p.Start();
                    p.WaitForExit();
                    blnRst = true;
                }
                catch
                {
                }
            }
            return blnRst;
        }

        /// <summary>
        /// 实现压缩（跨平台）
        /// Windows: 需要 rar.exe 上传到网站根目录
        /// Linux: 使用系统 zip 命令
        /// </summary>
        /// <param name="s">源目录</param>
        /// <param name="d">目标文件</param>
        public static void Rar(string s, string d)
        {
            if (IsWindows)
            {
                string rarPath = System.Web.HttpContext.Current.Server.MapPath("~/rar.exe");
                if (System.IO.File.Exists(rarPath))
                {
                    ExeCommand(rarPath + " a \"" + d + "\" \"" + s + "\" -ep1");
                }
                else
                {
                    // 使用系统 zip 命令作为备选
                    ExeCommand("powershell -Command \"Compress-Archive -Path '" + s + "' -DestinationPath '" + d + "' -Force\"");
                }
            }
            else
            {
                // Linux 使用 zip 命令
                ExeCommand("zip -r \"" + d + "\" \"" + s + "\"");
            }
        }

        /// <summary>
        /// 实现解压缩（跨平台）
        /// Windows: 需要 rar.exe 上传到网站根目录
        /// Linux: 使用系统 unzip 命令
        /// </summary>
        /// <param name="s">源文件</param>
        /// <param name="d">目标目录</param>
        public static void UnRar(string s, string d)
        {
            if (IsWindows)
            {
                string rarPath = System.Web.HttpContext.Current.Server.MapPath("~/rar.exe");
                if (System.IO.File.Exists(rarPath))
                {
                    ExeCommand(rarPath + " x \"" + s + "\" \"" + d + "\" -o+");
                }
                else
                {
                    // 使用系统 unzip 命令作为备选
                    ExeCommand("powershell -Command \"Expand-Archive -Path '" + s + "' -DestinationPath '" + d + "' -Force\"");
                }
            }
            else
            {
                // Linux 使用 unzip 命令
                ExeCommand("unzip -o \"" + s + "\" -d \"" + d + "\"");
            }
        }

    }
}
