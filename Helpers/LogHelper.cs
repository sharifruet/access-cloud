using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CloudDemo
{
    public class LogHelper
    {
        //日志文件所在路径
        private static string logPath = string.Empty;
        /// <summary>
        /// 保存日志的文件夹
        /// </summary>
        public static string LogPath
        {
            get
            {
                if (logPath == string.Empty)
                {
                    logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
                }
                return logPath;
            }
            set { logPath = value; }
        }

        /// <summary>
        /// 写日志
        /// <param name="msg">日志内容</param> 
        /// <param name="logType">日志类型</param>
        /// </summary>
        private static void WriteLog(string msg,string sn="")
        {
            System.IO.StreamWriter sw = null;
            try
            {
                var currentPath = LogPath;
                if (string.IsNullOrEmpty(sn) == false)
                {
                    currentPath = Path.Combine(currentPath, sn);
                }
                if (System.IO.Directory.Exists(currentPath) == false)
                    System.IO.Directory.CreateDirectory(currentPath);

                //string content =DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss:]:") + msg;
                sw = new System.IO.StreamWriter(currentPath + "/" + DateTime.Now.ToString("yyyyMMdd") + ".Log", true);
                sw.WriteLine(msg);
            }
            catch
            { }
            finally
            {
                sw.Close();
            }
        }

        private enum FlowType
        {
            Send,
            Recive
        }

        /// <summary>
        /// 流程日志
        /// </summary>
        /// <param name="folowType"></param>
        /// <param name="msg"></param>
        /// <param name="sn"></param>
        private static void FlowLog(FlowType folowType, string msg,string sn="")
        {
            System.IO.StreamWriter sw = null;
            try
            {
                var currentPath = LogPath;
                if (string.IsNullOrEmpty(sn) == false)
                {
                    currentPath = Path.Combine(currentPath, sn);
                }
                if (System.IO.Directory.Exists(currentPath) == false)
                    System.IO.Directory.CreateDirectory(currentPath);
                string content;
                if (folowType == FlowType.Send)
                {
                    content = "Send:>>>>>>>>>>>>>>>>>>> \r\n" + DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss:]") + msg;
                }
                else
                {
                    content = "Recv:<<<<<<<<<<<<<<<<<<< \r\n" + DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss:]:") + msg;
                }
                sw = new System.IO.StreamWriter(currentPath + "/Flow_" + DateTime.Now.ToString("yyyyMMdd") + ".Log", true);
                sw.WriteLine(content);
            }
            catch
            { }
            finally
            {
               // sw.Close();
            }

        }

        public static void Send(string message, string sn = "")
        {
            Console.WriteLine("Send:>>>>>>>>>>>>>>>>>>>>");
            string msg = string.Format("{0}[Info]{1}{2}", DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]"), string.IsNullOrEmpty(sn) ? sn : "[" + sn + "]", message);
            Console.WriteLine(msg);
            FlowLog(FlowType.Send, msg, sn);
        }

        public static void Receive(string message, string sn = "")
        {
            Console.WriteLine("Recv:<<<<<<<<<<<<<<<<<<<<");
            string msg = string.Format("{0}[Info]{1}{2}", DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]"), string.IsNullOrEmpty(sn) ? sn : "[" + sn + "]", message);
            Console.WriteLine(msg);
            FlowLog(FlowType.Recive, msg, sn);
        }

        public static void Error(string message, string sn = "")
        {
            string msg = string.Format("{0}[Error]{1}{2}", DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]"), string.IsNullOrEmpty(sn) ? sn : "[" + sn + "]", message);
            Console.WriteLine(msg);
            WriteLog(msg, sn);
        }
        public static void Info(string message, string sn = "")
        {
            string msg = string.Format("{0}[Info]{1}{2}", DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]"), string.IsNullOrEmpty(sn) ? sn : "[" + sn + "]", message);
            Console.WriteLine(msg);
            WriteLog(msg, sn);
        }
        public static string gb2312_utf8(string text)
        {
            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            //utf8   
            utf8 = System.Text.Encoding.GetEncoding("utf-8");
            byte[] gb;
            gb = gb2312.GetBytes(text);
            gb = System.Text.Encoding.Convert(gb2312, utf8, gb);
            //返回转换后的字符   
            return utf8.GetString(gb);
        }

        /// <summary>
        /// UTF8转换成GB2312
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string utf8_gb2312(string text)
        {
            //声明字符集   
            System.Text.Encoding utf8, gb2312;
            //utf8   
            utf8 = System.Text.Encoding.GetEncoding("utf-8");
            //gb2312   
            gb2312 = System.Text.Encoding.GetEncoding("gb2312");
            byte[] utf;
            utf = utf8.GetBytes(text);
            utf = System.Text.Encoding.Convert(utf8, gb2312, utf);
            //返回转换后的字符   
            return gb2312.GetString(utf);
        }
    }
}
