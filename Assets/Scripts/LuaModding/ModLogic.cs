using MoonSharp.Interpreter.REPL;
using MoonSharp.Interpreter;
using UnityEngine;
using APIs;
using System;
using System.IO;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

public class ModLogic
{
    private string folderPath;
    private Script script;
    
    public ModInfo modInfo { get; private set; }

    public ModLogic(string folderPath)
    {
        this.folderPath = folderPath;

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
        script.Globals["require"] = (Func<string, DynValue>)((moduleName) =>
        {
            var path = moduleName + ".lua";
            return script.DoFile(path);
        });

        //script.Options.ExecutionTimeoutMilliseconds = 100;
        script.Options.CheckThreadAccess = false;
        script.Options.DebugPrint = s => Debug.Log($"[LuaDebug] {s}");
    }

    public void Init()
    {
        try
        {
            script.DoFile("init.lua");
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