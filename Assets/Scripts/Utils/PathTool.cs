using System.IO;

/// <summary>
/// 路径操作工具
/// </summary>
public static class PathTool
{
    public static string GetFileNameWithoutExtension(string path)
    {
        return Path.GetFileNameWithoutExtension(path);
    }

    public static string GetFileName(string path)
    {
        return Path.GetFileName(path);
    }

    public static string GetDirectoryName(string path)
    {
        return Path.GetDirectoryName(path);
    }

    public static string GetFullPath(string path)
    {
        return Path.GetFullPath(path);
    }

    public static string GetExtension(string path)
    {
        return Path.GetExtension(path);
    }
}