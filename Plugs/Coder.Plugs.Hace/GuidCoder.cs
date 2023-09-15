using Coder.Serivces.Attributes;
using Coder.Serivces.Dependency;

namespace Coder.Plugs.Hace
{
    /// <summary>
    /// GuidHelper
    /// </summary>
    public class GuidCoder : ICoderService
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