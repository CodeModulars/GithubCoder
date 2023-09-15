using sy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GithubCoder
{
    /// <summary>
    /// IO助手类
    /// </summary>
    public static class IOHelper
    {
        // 删除文件夹
        public static void DeleteFolder(string path)
        {
            // 跳过不存在的目录
            if (!sy.IO.FolderExists(path)) return;
            // 获取所有文件
            var files = sy.IO.GetFiles(path);
            // 删除所有文件
            foreach (var file in files)
            {
                sy.IO.DeleteFile(file);
            }
            // 获取所有子目录
            var folders = sy.IO.GetFolders(path);
            // 删除所有子目录
            foreach (var folder in folders)
            {
                DeleteFolder(folder);
            }
            // 删除本级目录
            System.IO.Directory.Delete(path);
        }

        /// <summary>
        /// 获取脚本文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> GetScriptFiles(string path)
        {
            // 获取所有脚本文件
            var files = sy.IO.GetFiles(path, "*.crs").ToList();
            // 获取所有子目录中的脚本文件
            var folders = sy.IO.GetFolders(path);
            foreach (var folder in folders)
            {
                files.AddRange(GetScriptFiles(folder));
            }
            return files;
        }
    }
}
