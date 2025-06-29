using MoonSharp.Interpreter.REPL;
using MoonSharp.Interpreter;
using UnityEngine;
using APIs;
using System;
using System.IO;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class ModLogic
{
    private Script script;
    
    public ModInfo modInfo { get; private set; }

    Dictionary<string, DynValue> moduleCache = new Dictionary<string, DynValue>();

    public ModLogic(string folderPath)
    {
        var deserializerYaml = new DeserializerBuilder()
            .WithNamingConvention(UnderscoredNamingConvention.Instance)
            .Build();
        modInfo = deserializerYaml.Deserialize<ModInfo>(File.ReadAllText(Path.Combine(folderPath, "mod_info.yaml")));

        Debug.Log($"Init mod {modInfo.Name} ({modInfo.Version})");

        script = new Script(CoreModules.Preset_My_Sandbox);
        script.Options.ScriptLoader = new MyScriptLoader(folderPath);

        UserData.RegisterType<WorldAPI>();
        UserData.RegisterType<UIAPI>();
        UserData.RegisterType<EventsAPI>();

        script.Globals["World"] = new WorldAPI();
        script.Globals["UI"] = new UIAPI();
        script.Globals["Events"] = new EventsAPI();

        script.Globals["print"] = (Action<string>)(s => Debug.Log($"[{modInfo.Name}] {s}"));
        //script.Globals["require"] = (Func<string, DynValue>) moduleExecute;

        script.Options.CheckThreadAccess = false;
        script.Options.DebugPrint = s => Debug.Log($"[LuaDebug] {s}");
    }

    private DynValue moduleExecute(string moduleName)
    {
        if (moduleCache.TryGetValue(moduleName, out var cached))
            return cached;

        var path = moduleName.Replace('.', Path.DirectorySeparatorChar) + ".lua";
        var result = script.DoFile(path);
        moduleCache[moduleName] = result;
        return result;
    }

    public void Init()
    {
        try
        {
            var initFunction = @$"
            return function()
                {script.Options.ScriptLoader.LoadFile("init.lua", null)}
            end
            ";

            DynValue entry = script.DoString(initFunction);
            DynValue coroutine = script.CreateCoroutine(entry);
            ModdingProvider.Instance.ExecuteWithTimeout(coroutine, 0.5f).Forget();
        }
        catch (Exception exp)
        {
            Debug.LogError($"[{modInfo.Name}] {exp.Message}");
        }
    }

    public void Emit(string eventName)
    {
        (script.Globals.Get("Events").UserData.Object as EventsAPI).Emit(eventName);
    }
}

public class ModInfo
{
    public string Name { get; set; }
    public string Version { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
}