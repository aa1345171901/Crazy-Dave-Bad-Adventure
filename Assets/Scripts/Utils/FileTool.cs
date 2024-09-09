using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

/// <summary>
/// 文件操作工具
/// </summary>
public static class FileTool
{
    /// <summary>
    /// 同步写入文本
    /// </summary>
    public static void WriteText(string filePath, string dataStr)
    {
        byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(dataStr);
        WriteByte(filePath, bytes);
    }

    /// <summary>
    /// 异步写入文本
    /// </summary>
    public static void WriteTextAsync(string filePath, string dataStr, Action call = null)
    {
        byte[] bytes = Encoding.GetEncoding("utf-8").GetBytes(dataStr);
        WriteByteAsync(filePath, bytes, call);
    }

    /// <summary>
    /// 同步写入数据
    /// </summary>
    public static void WriteByte(string filePath, byte[] bytes)
    {
        if (!DirectoryExists(PathTool.GetDirectoryName(filePath)))
        {
            CreateDirectory(PathTool.GetDirectoryName(filePath));
        }

        FileStream writer = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        writer.Write(bytes, 0, bytes.Length);
        writer.Close();
    }

    /// <summary>
    /// 异步写入数据
    /// </summary>
    public static void WriteByteAsync(string filePath, byte[] bytes, Action call = null)
    {
        if (!DirectoryExists(PathTool.GetDirectoryName(filePath)))
        {
            CreateDirectory(PathTool.GetDirectoryName(filePath));
        }

        FileStream writer = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        void Call(IAsyncResult asr)
        {
            using (Stream str = (Stream)asr.AsyncState)
            {
                str.EndWrite(asr);
            }

            writer.Close();
            call?.Invoke();
        }
        writer.BeginWrite(bytes, 0, bytes.Length, new AsyncCallback(Call), writer);
    }

    /// <summary>
    /// 同步读取文本
    /// </summary>
    public static string ReadText(string filePath)
    {
        if (!FileTool.FileExists(filePath))
            return "";
        return Encoding.UTF8.GetString(ReadByte(filePath));
    }

    /// <summary>
    /// 异步读取文本
    /// </summary>
    public static void ReadTextAsync(string filePath, Action<string> call)
    {
        void Call(byte[] bytes)
        {
            call(Encoding.UTF8.GetString(bytes));
        }
        ReadByteAsync(filePath, Call);
    }

    /// <summary>
    /// 同步读取数据
    /// </summary>
    public static byte[] ReadByte(string filePath)
    {
        FileStream read = new FileStream(filePath, FileMode.Open);
        var bytes = new byte[read.Length];
        read.Read(bytes, 0, bytes.Length);
        read.Close();

        return bytes;
    }

    /// <summary>
    /// 异步读取数据
    /// </summary>
    public static void ReadByteAsync(string filePath, Action<byte[]> call)
    {
        FileStream read = new FileStream(filePath, FileMode.Open);
        var bytes = new byte[read.Length];
        void Call(IAsyncResult asr)
        {
            using (Stream str = (Stream)asr.AsyncState)
            {
                str.EndRead(asr);
            }

            read.Close();
            call?.Invoke(bytes);
        }
        read.BeginRead(bytes, 0, bytes.Length, new AsyncCallback(Call), read);
    }

    /// <summary>
    /// 复制文件
    /// </summary>
    public static void FileCopy(string fileFrom, string fileTo)
    {
        if (!FileExists(fileFrom))
        {
            return;
        }
        String[] folders = fileTo.Replace("\\", "/").Split('/');

        String dir = folders[0];
        for (int i = 1; i < folders.Length - 1; i++)
        {
            dir += "/" + folders[i];

            if (!DirectoryExists(dir))
            {
                CreateDirectory(dir);
            }
        }

        FileCopy(fileFrom, fileTo, true);
    }

    /// <summary>
    /// 复制文件夹
    /// </summary>
    public static void CopyDirectory(string sourceDirPath, string targetDirPath)
    {
        if (!DirectoryExists(sourceDirPath))
        {
            return;
        }

        //如果指定的存储路径不存在，则创建该存储路径
        if (!DirectoryExists(targetDirPath))
        {
            //创建
            CreateDirectory(targetDirPath);
        }

        if (GetFormatPath(sourceDirPath) == GetFormatPath(targetDirPath))
        {
            return;
        }

        //获取源路径文件的名称
        string[] files = DirectoryGetFiles(sourceDirPath);
        //遍历子文件夹的所有文件
        foreach (string file in files)
        {
            string pFilePath = targetDirPath + "/" + PathTool.GetFileName(file);
            FileCopy(file, pFilePath, true);
        }
        string[] dirs = DirectoryGetDirectories(sourceDirPath);
        //递归，遍历文件夹
        foreach (string dir in dirs)
        {
            CopyDirectory(dir, targetDirPath + "/" + PathTool.GetFileName(dir));
        }
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    public static void DeleteFiles(List<string> filesPath)
    {
        for (int i = 0; i < filesPath.Count; i++)
        {
            if (FileExists(filesPath[i]))
            {
                FileDelete(filesPath[i]);
            }
        }
    }

    /// <summary>
    /// 删除文件夹
    /// </summary>
    public static bool DeleteDirectory(string file)
    {
        if (!DirectoryExists(file))
        {
            return false;
        }
        //去除文件夹和子文件的只读属性
        //去除文件夹的只读属性
        System.IO.DirectoryInfo fileInfo = new DirectoryInfo(file);
        fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;

        //去除文件的只读属性
        System.IO.File.SetAttributes(file, System.IO.FileAttributes.Normal);
        //判断文件夹是否还存在
        if (DirectoryExists(file))
        {
            foreach (string f in Directory.GetFileSystemEntries(file))
            {
                if (FileExists(f))
                {
                    //如果有子文件删除文件
                    FileDelete(f);
                    Console.WriteLine(f);
                }
                else
                {
                    //循环递归删除子文件夹
                    DeleteDirectory(f);
                }
            }

            //删除空文件夹
            DirectoryDelete(file);

        }
        return true;
    }

    /// <summary>
    /// 删除空文件夹
    /// </summary>
    public static void DeleteEmptyFolders(string parentFolder)
    {
        var dir = new DirectoryInfo(parentFolder);
        var subdirs = dir.GetDirectories("*.*", SearchOption.AllDirectories);

        foreach (var subdir in subdirs)
        {
            if (!DirectoryExists(subdir.FullName)) continue;

            var subFiles = subdir.GetFileSystemInfos("*.*", SearchOption.AllDirectories);

            var findFile = false;
            foreach (var sub in subFiles)
            {
                findFile = (sub.Attributes & FileAttributes.Directory) == 0;

                if (findFile) break;
            }

            if (!findFile) subdir.Delete(true);
        }
    }

    /// <summary>
    /// 获取文件列表
    /// </summary>
    public static List<string> GetFiles(string path, string searchPattern = "*.*", List<string> ingorePattern = null)
    {
        List<string> files = new List<string>();
        GetAllFiles(path, searchPattern, files);

        if (ingorePattern != null)
        {
            for (int i = 0; i < files.Count; i++)
            {
                if (ingorePattern.Contains(PathTool.GetExtension(files[i]).Replace(".", "")))
                {
                    files.RemoveAt(i);
                    i--;
                }
            }
        }

        return files;
    }

    /// <summary>
    /// 获取文件内容md5
    /// </summary>
    public static string GetMD5HashFromFile(string filePath)
    {
        FileStream file = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] retVal = md5.ComputeHash(file);   //计算指定Stream 对象的哈希值  
        file.Close();
        file.Dispose();

        StringBuilder Ac = new StringBuilder();
        for (int i = 0; i < retVal.Length; i++)
        {
            Ac.Append(retVal[i].ToString("x2"));
        }
        return Ac.ToString();
    }

    /// <summary>
    /// 字符串转换为MD5
    /// </summary>
    public static string GetMD5HashFromString(string input)
    {
        if (input == null)
        {
            input = "";
        }
        using (MD5 hash = MD5.Create())
        {
            return string.Join("", hash.ComputeHash(Encoding.UTF8.GetBytes(input)).Select(x => x.ToString("x2"))).ToUpper();
        }
    }

    /// <summary>
    /// 获取路径下所有文件
    /// </summary>
    private static void GetAllFiles(string path, string searchPattern, List<string> files)
    {
        if (!DirectoryExists(path))
        {
            return;
        }

        DirectoryInfo dirInfo = new DirectoryInfo(path);

        foreach (var item in dirInfo.GetFiles(searchPattern))
        {
            files.Add(item.FullName);
        }
        foreach (System.IO.DirectoryInfo subdir in dirInfo.GetDirectories())
        {
            GetAllFiles(subdir.FullName, searchPattern, files);
        }
    }

    /// <summary>
    /// 删除空文件夹
    /// </summary>
    public static void DeleteEmptyDir(string parentFolder)
    {
        if (!DirectoryExists(parentFolder))
        {
            return;
        }

        var dir = new DirectoryInfo(parentFolder);
        var subdirs = dir.GetDirectories("*.*", SearchOption.AllDirectories);

        foreach (var subdir in subdirs)
        {
            if (!DirectoryExists(subdir.FullName)) continue;

            var subFiles = subdir.GetFileSystemInfos("*.*", SearchOption.AllDirectories);

            var findFile = false;
            foreach (var sub in subFiles)
            {
                findFile = (sub.Attributes & FileAttributes.Directory) == 0;

                if (findFile) break;
            }

            if (!findFile)
            {
                subdir.Delete(true);
                if (FileExists(subdir.FullName + ".meta"))
                {
                    FileDelete(subdir.FullName + ".meta");
                }
            }
        }
    }

    /// <summary>
    /// 是否包含指定文件
    /// </summary>
    public static bool FileExists(string path)
    {
        return File.Exists(path);
    }

    /// <summary>
    /// 是否包含指定文件夹
    /// </summary>
    public static bool DirectoryExists(string path)
    {
        return Directory.Exists(path);
    }

    /// <summary>
    /// 获取指定路径下的所有文件夹
    /// </summary>
    public static string[] GetDirectories(string path)
    {
        return Directory.GetDirectories(path);
    }

    /// <summary>
    /// 获取指定路径下的所有文件夹
    /// </summary>
    public static DateTime GetDirectoriesLastAccessTime(string path)
    {
        return Directory.GetLastAccessTime(path);
    }

    /// <summary>
    /// 创建文件夹
    /// </summary>
    public static void CreateDirectory(string path)
    {
        Directory.CreateDirectory(path);
    }

    /// <summary>
    /// 获取指定文件夹下的所有文件
    /// </summary>
    public static string[] DirectoryGetFiles(string path)
    {
        return Directory.GetFiles(path);
    }

    /// <summary>
    /// 获取指定文件夹下的所有文件夹
    /// </summary>
    public static string[] DirectoryGetDirectories(string path)
    {
        return Directory.GetDirectories(path);
    }

    /// <summary>
    /// 删除文件夹
    /// </summary>
    public static void DirectoryDelete(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path);
        }
    }

    /// <summary>
    /// 删除文件
    /// </summary>
    public static void FileDelete(string path)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    /// <summary>
    /// 重命名文件
    /// </summary>
    public static void FileMove(string fileFrom, string fileTo)
    {
        if (File.Exists(fileFrom))
        {
            if (File.Exists(fileTo))
                FileDelete(fileTo);
            File.Move(fileFrom, fileTo);
        }
    }

    /// <summary>
    /// 内存流
    /// </summary>
    public static MemoryStream MemoryStream()
    {
        return new MemoryStream();
    }

    /// <summary>
    /// 内存流
    /// </summary>
    public static MemoryStream MemoryStream(byte[] bytes)
    {
        return new MemoryStream(bytes);
    }

    /// <summary>
    /// 文件流
    /// </summary>
    public static FileStream FileCreate(string path)
    {
        return File.Create(path);
    }

    /// <summary>
    /// 文件流
    /// </summary>
    public static FileStream FileOpenRead(string path)
    {
        return File.OpenRead(path);
    }

    /// <summary>
    /// 复制文件
    /// </summary>
    private static void FileCopy(string sourceFileName, string destFileName, bool overwrite)
    {
        File.Copy(sourceFileName, destFileName, overwrite);
    }

    /// <summary>
    /// 获取统一格式化的路径
    /// </summary>
    private static string GetFormatPath(string path)
    {
        return path.Replace("/", "\\");
    }
}