using MoonSharp.Interpreter;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiButton : MonoBehaviour
{
    [SerializeField]
    Button button;

    [SerializeField]
    TMP_Text text;

    public void InitButton(string label, DynValue callback)
    {
        text.text = label;

        button.onClick.AddListener(() =>
        {
            callback.Function.Call();
        });
    }
}
