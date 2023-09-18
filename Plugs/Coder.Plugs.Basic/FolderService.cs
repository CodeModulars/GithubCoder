using Coder.Serivces;
using Coder.Serivces.Attributes;
using Coder.Serivces.Dependency;
using System.ComponentModel;

namespace Coder.Plugs.Basic
{
    /// <summary>
    /// 目录服务
    /// </summary>
    public sealed class FolderService : CoderService
    {
        /// <summary>
        /// 目录创建
        /// </summary>
        /// <returns></returns>
        [CoderAction(ActionNameTransformType.LowerUnderlineCase)]
        [Description("目录创建")]
        public void FolderCreate(string path)
        {
            sy.IO.CreateFolder(path);
        }
    }
}