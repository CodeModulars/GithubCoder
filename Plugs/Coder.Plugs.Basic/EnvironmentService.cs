using Coder.Serivces;
using Coder.Serivces.Attributes;
using Suyaa.Arguments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Plugs.Basic
{
    /// <summary>
    /// 环境服务
    /// </summary>
    public sealed class EnvironmentService : CoderService
    {

        // 系统变量
        public static EArguments? arguments;

        /// <summary>
        /// 获取运行参数
        /// </summary>
        /// <returns></returns>
        [CoderAction(ActionNameTransformType.LowerUnderlineCase)]
        public string GetArgument(string name)
        {
            if (arguments is null) return string.Empty;
            if (!arguments.ContainsKey(name)) return string.Empty;
            return arguments[name];
        }
    }
}
