using Coder.Serivces;
using Coder.Serivces.Attributes;
using Coder.Serivces.Dependency;

namespace Coder.Plugs.Basic
{
    /// <summary>
    /// Guid服务
    /// </summary>
    public sealed class GuidService : CoderService
    {
        /// <summary>
        /// 创建Guid
        /// </summary>
        /// <returns></returns>
        [CoderAction(ActionNameTransformType.LowerUnderlineCase)]
        public string CreateGuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}