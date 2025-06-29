using MoonSharp.Interpreter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ModdingProvider : MonoBehaviour
{
    public static ModdingProvider Instance { get; private set; }

    public List<ModLogic> Mods = new List<ModLogic>();

    private string basePath => Path.Combine(Application.streamingAssetsPath, "Mods");

    private void Awake()
    {
        Instance = this;

        try
        {
            string[] dirs = Directory.GetDirectories(basePath, "*", SearchOption.TopDirectoryOnly);

            foreach (string dir in dirs)
            {
                Mods.Add(new ModLogic(dir));
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"The process failed: {e.ToString()}");
        }

        foreach (var mod in Mods)
        {
            mod.Init();
        }
    }

    private void Start()
    {
        foreach (var mod in Mods)
        {
            mod.Emit("GameStart");
        }
    }

    public async UniTask<DynValue> ExecuteWithTimeout(DynValue coroutine, float maxSeconds)
    {
        MoonSharp.Interpreter.Coroutine luaCoroutine = coroutine.Coroutine;

        float startTime = Time.realtimeSinceStartup;

        while (luaCoroutine.State != CoroutineState.Dead)
        {
            luaCoroutine.Resume();

            float elapsed = Time.realtimeSinceStartup - startTime;
            if (elapsed > maxSeconds)
            {
                Debug.LogError("Lua-скрипт превысил лимит времени и был остановлен");
                return null;
            }

            await UniTask.Yield();
        }

        return coroutine;
    }
}
