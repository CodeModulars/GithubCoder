using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GithubCoder
{
    /// <summary>
    /// Zip
    /// </summary>
    internal static class ZipHelper
    {

        #region   公有方法

        /// <summary>
        /// 创建 zip 存档，该文档包含指定目录的文件和子目录（单个目录）。
        /// </summary>
        /// <param name="sourceDirectoryName">将要压缩存档的文件目录的路径，可以为相对路径或绝对路径。 相对路径是指相对于当前工作目录的路径。</param>
        /// <param name="destinationArchiveFileName">将要生成的压缩包的存档路径，可以为相对路径或绝对路径。相对路径是指相对于当前工作目录的路径。</param>
        /// <param name="compressionLevel">指示压缩操作是强调速度还是强调压缩大小的枚举值</param>
        /// <param name="includeBaseDirectory">压缩包中是否包含父目录</param>
        /// <returns>返回结果（true：表示成功）</returns>
        public static bool CreatZip(string sourceDirectoryName, string destinationArchiveFileName, CompressionLevel compressionLevel = CompressionLevel.NoCompression, bool includeBaseDirectory = true)
        {
            int i = 1;
            try
            {
                if (Directory.Exists(sourceDirectoryName))
                    if (!File.Exists(destinationArchiveFileName))
                    {
                        ZipFile.CreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, compressionLevel, includeBaseDirectory);
                    }
                    else
                    {
                        var toZipFileDictionaryList = GetAllDirList(sourceDirectoryName, includeBaseDirectory);
                        using (var archive = ZipFile.Open(destinationArchiveFileName, ZipArchiveMode.Update))
                        {
                            var count = toZipFileDictionaryList.Keys.Count;
                            foreach (var toZipFileKey in toZipFileDictionaryList.Keys)
                            {
                                if (toZipFileKey != destinationArchiveFileName)
                                {
                                    var toZipedFileName = Path.GetFileName(toZipFileKey);
                                    var toDelArchives = new List<ZipArchiveEntry>();
                                    foreach (var zipArchiveEntry in archive.Entries)
                                    {
                                        if (toZipedFileName != null && (zipArchiveEntry.FullName.StartsWith(toZipedFileName) || toZipedFileName.StartsWith(zipArchiveEntry.FullName)))
                                        {
                                            i++;
                                            //compressProgress(this, new CompressProgressEventArgs { Size = zipArchiveEntry.Length, Count = count, Index = i, Path = zipArchiveEntry.FullName, Name = zipArchiveEntry.Name });
                                            toDelArchives.Add(zipArchiveEntry);
                                        }
                                    }

                                    foreach (var zipArchiveEntry in toDelArchives)
                                        zipArchiveEntry.Delete();
                                    archive.CreateEntryFromFile(toZipFileKey, toZipFileDictionaryList[toZipFileKey], compressionLevel);
                                }
                            }
                        }
                    }
                else if (File.Exists(sourceDirectoryName))
                    if (!File.Exists(destinationArchiveFileName))
                        ZipFile.CreateFromDirectory(sourceDirectoryName, destinationArchiveFileName, compressionLevel, false);
                    else
                    {
                        using (var archive = ZipFile.Open(destinationArchiveFileName, ZipArchiveMode.Update))
                        {
                            if (sourceDirectoryName != destinationArchiveFileName)
                            {
                                var toZipedFileName = Path.GetFileName(sourceDirectoryName);
                                var toDelArchives = new List<ZipArchiveEntry>();
                                var count = archive.Entries.Count;
                                foreach (var zipArchiveEntry in archive.Entries)
                                {
                                    if (toZipedFileName != null && (zipArchiveEntry.FullName.StartsWith(toZipedFileName) || toZipedFileName.StartsWith(zipArchiveEntry.FullName)))
                                    {
                                        i++;
                                        //compressProgress(this, new CompressProgressEventArgs { Size = zipArchiveEntry.Length, Count = count, Index = i, Path = zipArchiveEntry.FullName, Name = zipArchiveEntry.Name });
                                        toDelArchives.Add(zipArchiveEntry);
                                    }
                                }

                                foreach (var zipArchiveEntry in toDelArchives)
                                    zipArchiveEntry.Delete();
                                archive.CreateEntryFromFile(sourceDirectoryName, toZipedFileName, compressionLevel);
                            }
                        }
                    }
                else
                    return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 创建 zip 存档，该存档包含指定目录的文件和目录（多个目录）
        /// </summary>
        /// <param name="sourceDirectoryName">将要压缩存档的文件目录的路径，可以为相对路径或绝对路径。 相对路径是指相对于当前工作目录的路径。</param>
        /// <param name="destinationArchiveFileName">将要生成的压缩包的存档路径，可以为相对路径或绝对路径。 相对路径是指相对于当前工作目录的路径。</param>
        /// <param name="compressionLevel">指示压缩操作是强调速度还是强调压缩大小的枚举值</param>
        /// <returns>返回结果（true：表示成功）</returns>
        public static bool CreatZip(Dictionary<string, string> sourceDirectoryName, string destinationArchiveFileName, CompressionLevel compressionLevel = CompressionLevel.NoCompression)
        {
            int i = 1;
            try
            {
                using (FileStream zipToOpen = new FileStream(destinationArchiveFileName, FileMode.OpenOrCreate))
                {
                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                    {
                        foreach (var toZipFileKey in sourceDirectoryName.Keys)
                        {
                            if (toZipFileKey != destinationArchiveFileName)
                            {
                                var toZipedFileName = Path.GetFileName(toZipFileKey);
                                var toDelArchives = new List<ZipArchiveEntry>();
                                var count = archive.Entries.Count;
                                foreach (var zipArchiveEntry in archive.Entries)
                                {
                                    if (toZipedFileName != null && (zipArchiveEntry.FullName.StartsWith(toZipedFileName) || toZipedFileName.StartsWith(zipArchiveEntry.FullName)))
                                    {
                                        i++;
                                        //compressProgress(this, new CompressProgressEventArgs { Size = zipArchiveEntry.Length, Count = count, Index = i, Path = toZipedFileName });
                                        toDelArchives.Add(zipArchiveEntry);
                                    }
                                }

                                foreach (var zipArchiveEntry in toDelArchives)
                                    zipArchiveEntry.Delete();
                                archive.CreateEntryFromFile(toZipFileKey, sourceDirectoryName[toZipFileKey], compressionLevel);
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 递归删除磁盘上的指定文件夹目录及文件
        /// </summary>
        /// <param name="baseDirectory">需要删除的文件夹路径</param>
        /// <returns>返回结果（true：表示成功）</returns>
        public static bool DeleteFolder(string baseDirectory)
        {
            var successed = true;
            try
            {
                if (Directory.Exists(baseDirectory)) //如果存在这个文件夹删除之 
                {
                    foreach (var directory in Directory.GetFileSystemEntries(baseDirectory))
                        if (File.Exists(directory))
                            File.Delete(directory); //直接删除其中的文件  
                        else
                            successed = DeleteFolder(directory); //递归删除子文件夹 
                    Directory.Delete(baseDirectory); //删除已空文件夹     
                }
            }
            catch (Exception)
            {
                successed = false;
            }
            return successed;
        }

        /// <summary>
        /// 递归获取磁盘上的指定目录下所有文件的集合，返回类型是：字典[文件名，要压缩的相对文件名]
        /// </summary>
        /// <param name="strBaseDir">需要递归的目录路径</param>
        /// <param name="includeBaseDirectory">是否包含本目录（false：表示不包含）</param>
        /// <param name="namePrefix">目录前缀</param>
        /// <returns>返回当前递归目录下的所有文件集合</returns>
        public static Dictionary<string, string> GetAllDirList(string strBaseDir, bool includeBaseDirectory = false, string namePrefix = "")
        {
            var resultDictionary = new Dictionary<string, string>();
            var directoryInfo = new DirectoryInfo(strBaseDir);
            var directories = directoryInfo.GetDirectories();
            var fileInfos = directoryInfo.GetFiles();
            if (includeBaseDirectory)
                namePrefix += directoryInfo.Name + "\\";
            foreach (var directory in directories)
                resultDictionary = resultDictionary.Concat(GetAllDirList(directory.FullName, true, namePrefix)).ToDictionary(k => k.Key, k => k.Value); //FullName是某个子目录的绝对地址，
            foreach (var fileInfo in fileInfos)
                if (!resultDictionary.ContainsKey(fileInfo.FullName))
                    resultDictionary.Add(fileInfo.FullName, namePrefix + fileInfo.Name);
            return resultDictionary;
        }

        /// <summary>
        /// 解压Zip文件，并覆盖保存到指定的目标路径文件夹下
        /// </summary>
        /// <param name="zipFilePath">将要解压缩的zip文件的路径</param>
        /// <param name="unZipDir">解压后将zip中的文件存储到磁盘的目标路径</param>
        /// <param name="zipArchive">解压信息处理</param>
        public static void UnZip(string zipFilePath, string unZipDir, Func<ZipArchiveEntry, bool>? zipArchive = null)
        {
            // 校验存在性
            if (!sy.IO.FileExists(zipFilePath)) throw new Exception($"文件'{zipFilePath}'不存在");
            if (!sy.IO.FolderExists(unZipDir)) throw new Exception($"文件夹'{unZipDir}'不存在");
            using (var zipToOpen = new FileStream(zipFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
            {
                using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                {
                    var count = archive.Entries.Count;
                    for (int i = 0; i < count; i++)
                    {
                        var entry = archive.Entries[i];
                        // 判断是否为文件夹
                        if (entry.FullName.EndsWith("/"))
                        {
                            string path = sy.IO.CombinePath(unZipDir, entry.FullName);
                            sy.IO.CreateFolder(path);
                        }
                        else
                        {
                            // 解压信息处理
                            if (zipArchive != null)
                            {
                                if (!zipArchive(entry)) continue;
                            }
                            // 输出内容
                            string path = sy.IO.CombinePath(unZipDir, entry.FullName);
                            var content = new byte[entry.Length];
                            entry.Open().Read(content, 0, content.Length);
                            File.WriteAllBytes(path, content);
                            content = new byte[0];
                        }
                        //if (!entries.FullName.EndsWith("/"))
                        //{
                        //    var entryFilePath = Regex.Replace(entries.FullName.Replace("/", @"\"), @"^\\*", "");
                        //    var filePath = directoryInfo + entryFilePath; //设置解压路径
                        //                                                  //unZipProgress(this, new UnZipProgressEventArgs { Size = entries.Length, Count = count, Index = i + 1, Path = entries.FullName, Name = entries.Name });
                        //    var content = new byte[entries.Length];
                        //    entries.Open().Read(content, 0, content.Length);
                        //    var greatFolder = Directory.GetParent(filePath);
                        //    if (!greatFolder.Exists)
                        //        greatFolder.Create();
                        //    File.WriteAllBytes(filePath, content);
                        //}
                    }
                }
            }
        }

        /// <summary>
        /// 获取Zip压缩包中的文件列表
        /// </summary>
        /// <param name="zipFilePath">Zip压缩包文件的物理路径</param>
        /// <returns>返回解压缩包的文件列表</returns>
        public static List<string> GetZipFileList(string zipFilePath)
        {
            List<string> fList = new List<string>();
            if (!File.Exists(zipFilePath))
                return fList;
            try
            {
                using (var zipToOpen = new FileStream(zipFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                    {
                        foreach (var zipArchiveEntry in archive.Entries)
                            if (!zipArchiveEntry.FullName.EndsWith("/"))
                                fList.Add(Regex.Replace(zipArchiveEntry.FullName.Replace("/", @"\"), @"^\\*", ""));
                    }
                }
            }
            catch (Exception)
            {

            }
            return fList;
        }

        #endregion

    }//Class_end
}
