using MoonSharp.Interpreter;
using UnityEngine;

namespace APIs
{
    public class WorldAPI
    {
        public string Spawn(string prefabName, Table position)
        {
            var pos = new Vector3(
                (float)position.Get("x").Number,
                (float)position.Get("y").Number,
                (float)position.Get("z").Number
            );

            var res = Resources.Load(prefabName);

            if (res == null)
            {
                Debug.LogError($"[WorldAPI] Prefab '{prefabName}' not found in Resources.");
                return null;
            }

            var go = GameObject.Instantiate(res) as GameObject;
            go.transform.position = pos;
            return go.GetInstanceID().ToString();
        }
    }
}
