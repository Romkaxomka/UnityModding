using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace APIs
{
    public class UIAPI
    {
        public void ShowButton(string label, DynValue callback)
        {
            UiHelper.Instance.AddButton(label, callback);
        }
    }
}
