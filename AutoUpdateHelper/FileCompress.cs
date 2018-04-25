using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoUpdateHelper
{
    /// <summary>
    /// 压缩文件
    /// </summary>
    public class FileCompress
    {
        /// <summary>
        /// ZIP：压缩文件夹
        /// </summary>
        /// <param name="DirectoryToZip">需要压缩的文件夹（绝对路径）</param>
        /// <param name="ZipedPath">压缩后的文件路径（绝对路径）</param>
        /// <param name="ZipedFileName">压缩后的文件名称（文件名，默认 同源文件夹同名）</param>
        public static void ZipDirectory(string DirectoryToZip, string ZipedPath, string ZipedFileName = "")
        {
            //如果目录不存在，则报错
            if (!System.IO.Directory.Exists(DirectoryToZip))
            {
                throw new System.IO.FileNotFoundException("指定的目录: " + DirectoryToZip + " 不存在!");
            }

            //文件名称（默认同源文件名称相同）
            string ZipFileName = string.IsNullOrEmpty(ZipedFileName) ? Path.Combine(ZipedPath, new DirectoryInfo(DirectoryToZip).Name + ".zip") : Path.Combine(ZipedPath, ZipedFileName + ".zip");

            using (System.IO.FileStream ZipFile = System.IO.File.Create(ZipFileName))
            {
                using (ZipOutputStream s = new ZipOutputStream(ZipFile))
                {
                    ZipSetp(DirectoryToZip, s, "");
                }
            }
        }
        /// <summary>
        /// 递归遍历目录
        /// 
        /// </summary>
        private static void ZipSetp(string strDirectory, ZipOutputStream s, string parentPath)
        {
            if (strDirectory[strDirectory.Length - 1] != Path.DirectorySeparatorChar)
            {
                strDirectory += Path.DirectorySeparatorChar;
            }
            Crc32 crc = new Crc32();
            string[] filenames = Directory.GetFileSystemEntries(strDirectory);

            foreach (string file in filenames)// 遍历所有的文件和目录
            {

                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {
                    string pPath = parentPath;
                    pPath += file.Substring(file.LastIndexOf("\\") + 1);
                    pPath += "\\";
                    ZipSetp(file, s, pPath);
                }

                else // 否则直接压缩文件
                {
                    //排除自身
                    if (!Path.GetExtension(file).Contains(".zip"))
                    {
                        if (File.Exists(file))
                        {
                            //打开压缩文件
                            using (FileStream fs = File.OpenRead(file))
                            {

                                byte[] buffer = new byte[fs.Length];
                                fs.Read(buffer, 0, buffer.Length);

                                string fileName = parentPath + file.Substring(file.LastIndexOf("\\") + 1);
                                ZipEntry entry = new ZipEntry(fileName);

                                entry.DateTime = DateTime.Now;
                                entry.Size = fs.Length;

                                fs.Close();

                                crc.Reset();
                                crc.Update(buffer);

                                entry.Crc = crc.Value;
                                s.PutNextEntry(entry);

                                s.Write(buffer, 0, buffer.Length);
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// ZIP:解压一个zip文件
        /// </summary>
        /// <param name="ZipFile">需要解压的Zip文件（绝对路径）</param>
        /// <param name="TargetDirectory">解压到的目录</param>
        /// <param name="OverWrite">是否覆盖已存在的文件</param>
        public static void UnZip(string ZipFile, string TargetDirectory, bool OverWrite = true)
        {
            try
            {
                //如果解压到的目录不存在，则报错
                if (!System.IO.Directory.Exists(TargetDirectory))
                {
                    throw new System.IO.FileNotFoundException("指定的目录: " + TargetDirectory + " 不存在!");
                }
                //目录结尾
                if (!TargetDirectory.EndsWith("\\"))
                {
                    TargetDirectory = TargetDirectory + "\\";
                }

                if (File.Exists(ZipFile))
                {
                    FileStream fileStream = File.OpenRead(ZipFile);
                    using (ZipInputStream zipfiles = new ZipInputStream(fileStream))
                    {
                        //zipfiles.Password = Password;
                        ZipEntry theEntry;

                        while ((theEntry = zipfiles.GetNextEntry()) != null)
                        {
                            string directoryName = "";
                            string pathToZip = "";
                            pathToZip = theEntry.Name;

                            if (pathToZip != "")
                                directoryName = Path.GetDirectoryName(pathToZip) + "\\";

                            string fileName = Path.GetFileName(pathToZip);

                            Directory.CreateDirectory(TargetDirectory + directoryName);

                            if (fileName != "")
                            {
                                if ((File.Exists(TargetDirectory + directoryName + fileName) && OverWrite) || (!File.Exists(TargetDirectory + directoryName + fileName)))
                                {
                                    using (FileStream streamWriter = File.Create(TargetDirectory + directoryName + fileName))
                                    {
                                        int size = 2048;
                                        byte[] data = new byte[2048];
                                        while (true)
                                        {
                                            size = zipfiles.Read(data, 0, data.Length);

                                            if (size > 0)
                                                streamWriter.Write(data, 0, size);
                                            else
                                                break;
                                        }
                                        streamWriter.Close();
                                        streamWriter.Dispose();
                                    }
                                }
                            }
                        }

                        zipfiles.Close();
                        zipfiles.Dispose();
                    }
                    fileStream.Close();
                    fileStream.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
