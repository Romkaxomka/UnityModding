using Cysharp.Threading.Tasks;
using MoonSharp.Interpreter;
using System.Collections.Generic;

namespace APIs
{
    public class EventsAPI
    {
        private Dictionary<string, List<Closure>> handlers = new();

        public void On(string eventName, Closure function)
        {
            if (!handlers.ContainsKey(eventName))
                handlers[eventName] = new();
            handlers[eventName].Add(function);
        }

        public void Emit(string eventName)
        {
            if (handlers.TryGetValue(eventName, out var list))
            {
                foreach (var fn in list)
                {
                    Script script = fn.OwnerScript;
                    DynValue coroutine = script.CreateCoroutine(fn);
                    ModdingProvider.Instance.ExecuteWithTimeout(coroutine, 0.5f).Forget();
                }
            }
        }
    }
}
