using Coder.Serivces;
using Coder.Serivces.Attributes;
using Coder.Serivces.Dependency;
using System.ComponentModel;

namespace Coder.Plugs.Basic
{
    /// <summary>
    /// 字符串服务
    /// </summary>
    public sealed class StringService : CoderService
    {
        /// <summary>
        /// 转为大写
        /// </summary>
        /// <returns></returns>
        [CoderAction(ActionNameTransformType.LowerUnderlineCase)]
        [Description("转为大写")]
        public string StringUpper(string str)
        {
            return str.ToUpper();
        }

        /// <summary>
        /// 转为小写
        /// </summary>
        /// <returns></returns>
        [CoderAction(ActionNameTransformType.LowerUnderlineCase)]
        [Description("转为小写")]
        public string StringLower(string str)
        {
            return str.ToLower();
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <returns></returns>
        [CoderAction(ActionNameTransformType.LowerUnderlineCase)]
        [Description("分割字符串")]
        public List<string> StringSplit(string str, string chr)
        {
            return str.Split(chr).ToList();
        }

    }
}