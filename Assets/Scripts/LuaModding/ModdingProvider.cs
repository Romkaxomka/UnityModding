using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ModdingProvider : MonoBehaviour
{
    public List<ModLogic> Mods = new List<ModLogic>();

    private void Awake()
    {
        try
        {
            string[] dirs = Directory.GetDirectories(Path.Combine(Application.dataPath, "Mods"), "*", SearchOption.TopDirectoryOnly);

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
}
