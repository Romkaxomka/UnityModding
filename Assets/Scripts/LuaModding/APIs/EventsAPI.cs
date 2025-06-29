using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    fn.Call();
            }
        }
    }
}
