using System;
using System.Collections.Generic;
using System.Text;

namespace Coder.Script
{
    /// <summary>
    /// 解析异常
    /// </summary>
    public class ParseException : Exception
    {
        /// <summary>
        /// 解析异常
        /// </summary>
        /// <param name="line"></param>
        /// <param name="site"></param>
        /// <param name="chr"></param>
        public ParseException(int line, int site, char chr) : base($"行{line}位置{site}发生异常: 意外的字符'{(chr == '\0' ? "结束符" : chr.ToString())}'。")
        {
            Line = line;
            Site = site;
        }

        /// <summary>
        /// 解析异常
        /// </summary>
        /// <param name="line"></param>
        /// <param name="site"></param>
        /// <param name="message"></param>
        public ParseException(int line, int site, string message) : base($"行{line}位置{site}发生异常: {message}。")
        {
            Line = line;
            Site = site;
        }

        /// <summary>
        /// 行号
        /// </summary>
        public int Line { get; }

        /// <summary>
        /// 位置
        /// </summary>
        public int Site { get; }
    }
}
