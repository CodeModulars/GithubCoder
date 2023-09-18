using Coder.Serivces;
using Coder.Serivces.Attributes;
using Suyaa.Arguments;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private readonly EArguments _arguments;

        public EnvironmentService(
            EArguments arguments
            )
        {
            _arguments = arguments;
        }

        /// <summary>
        /// 获取运行参数
        /// </summary>
        /// <returns></returns>
        [CoderAction(ActionNameTransformType.LowerUnderlineCase)]
        [Description("获取运行参数")]
        public string GetArgument(string name)
        {
            if (!_arguments.ContainsKey(name)) return string.Empty;
            return _arguments[name];
        }
    }
}
