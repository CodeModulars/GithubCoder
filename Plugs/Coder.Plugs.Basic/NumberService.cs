using Coder.Serivces;
using Coder.Serivces.Attributes;
using Coder.Serivces.Dependency;
using System.ComponentModel;

namespace Coder.Plugs.Basic
{
    /// <summary>
    /// 数值服务
    /// </summary>
    public sealed class NumberService : CoderService
    {
        /// <summary>
        /// 计算算术和
        /// </summary>
        /// <returns></returns>
        [CoderAction(ActionNameTransformType.LowerUnderlineCase)]
        [Description("计算算术和")]
        public double Add(double num1, double num2)
        {
            return num1 + num2;
        }

        /// <summary>
        /// 计算算术差
        /// </summary>
        /// <returns></returns>
        [CoderAction(ActionNameTransformType.LowerUnderlineCase)]
        [Description("计算算术和")]
        public double Sub(double num1, double num2)
        {
            return num1 - num2;
        }

        /// <summary>
        /// 计算算术积
        /// </summary>
        /// <returns></returns>
        [CoderAction(ActionNameTransformType.LowerUnderlineCase)]
        [Description("计算算术积")]
        public double Mul(double num1, double num2)
        {
            return num1 * num2;
        }

        /// <summary>
        /// 计算算术商
        /// </summary>
        /// <returns></returns>
        [CoderAction(ActionNameTransformType.LowerUnderlineCase)]
        [Description("计算算术积")]
        public double Div(double num1, double num2)
        {
            return num1 / num2;
        }

        /// <summary>
        /// 计算整除
        /// </summary>
        /// <returns></returns>
        [CoderAction(ActionNameTransformType.LowerUnderlineCase)]
        [Description("计算整除")]
        public double Divi(double num1, double num2)
        {
            return (long)num1 / (long)num2;
        }

        /// <summary>
        /// 计算取余
        /// </summary>
        /// <returns></returns>
        [CoderAction(ActionNameTransformType.LowerUnderlineCase)]
        [Description("计算取余")]
        public double Mod(double num1, double num2)
        {
            return (long)num1 % (long)num2;
        }

    }
}