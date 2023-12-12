using Coder.Serivces;
using Coder.Serivces.Attributes;
using Coder.Serivces.Dependency;
using System.ComponentModel;

namespace Coder.Plugs.Basic
{
    /// <summary>
    /// 字符串列表服务
    /// </summary>
    public sealed class StringListService : CoderService
    {
        /// <summary>
        /// 字符串数组获取
        /// </summary>
        /// <returns></returns>
        [CoderAction(ActionNameTransformType.LowerUnderlineCase)]
        [Description("字符串数组获取")]
        public string StringListGet(List<string> list, double index)
        {
            return list[(int)index];
        }

        /// <summary>
        /// 字符串数组长度获取
        /// </summary>
        /// <returns></returns>
        [CoderAction(ActionNameTransformType.LowerUnderlineCase)]
        [Description("字符串数组长度获取")]
        public double StringListLength(List<string> list)
        {
            return list.Count;
        }

        /// <summary>
        /// 字符串数组截取
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        [CoderAction(ActionNameTransformType.LowerUnderlineCase)]
        [Description("字符串数组截取")]
        public List<string> StringListSlice(List<string> list, double index, double len)
        {
            return list.Skip((int)index).Take((int)len).ToList();
        }

        /// <summary>
        /// 字符串数组连接
        /// </summary>
        /// <param name="list"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        [CoderAction(ActionNameTransformType.LowerUnderlineCase)]
        [Description("字符串数组连接")]
        public string StringListJoin(List<string> list, string separator)
        {
            return string.Join(separator, list.ToArray());
        }
    }
}