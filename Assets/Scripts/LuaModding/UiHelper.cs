using MoonSharp.Interpreter;
using UnityEngine;

public class UiHelper : MonoBehaviour
{
    [SerializeField]
    public GameObject buttonPrefab;

    public static UiHelper Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

    public void AddButton(string label, DynValue callback)
    {
        var button = Instantiate(buttonPrefab, transform).GetComponent<UiButton>();
        button.InitButton(label, callback);
    }
}
