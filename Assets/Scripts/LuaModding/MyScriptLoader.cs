using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;
using System;
using System.IO;
using System.Text;

public class MyScriptLoader : ScriptLoaderBase
{
    private string basePath;

    public MyScriptLoader(string basePath)
    {
        this.basePath = Path.GetFullPath(basePath);
        ModulePaths = new[] { "?.lua" };
    }

    public override object LoadFile(string file, Table globalContext)
    {
        string fullPath = GetSafeFullPath(file);
        return File.ReadAllText(fullPath, Encoding.UTF8);
    }

    public override bool ScriptFileExists(string name)
    {
        try
        {
            GetSafeFullPath(name);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private string GetSafeFullPath(string relativePath)
    {
        relativePath = relativePath.Replace("..", "");
        relativePath = relativePath.Replace("/", "");
        relativePath = relativePath.Replace("\\", "");

        //string rawPath = relativePath.Replace('/', Path.DirectorySeparatorChar);

        string combinedPath = Path.Combine(basePath, relativePath);
        string fullPath = Path.GetFullPath(combinedPath);

        if (!fullPath.StartsWith(basePath, StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException($"Доступ к '{relativePath}' запрещён.");
        }

        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException($"Файл не найден: {relativePath}");
        }

        return fullPath;
    }
}

