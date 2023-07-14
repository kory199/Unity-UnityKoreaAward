using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IProcess
{
    enum NextProcess
    {
        Continue,
        Break
    }
    NextProcess ProcessInput();
}

public class ProcessManager : MonoBehaviour
{
    public Stack<IProcess> processingUIStack { get { return _processingUIStack; } }
    private Stack<IProcess> _processingUIStack = new();
    private EventSystem _eventSystem;
    private void Start()
    {
        _eventSystem = new GameObject().AddComponent<EventSystem>();
        _eventSystem.AddComponent<StandaloneInputModule>();
        _eventSystem.name = nameof(EventSystem);
        DontDestroyOnLoad(_eventSystem);
    }

    void Update()
    {
        foreach (var process in processingUIStack)
        {
            var result = process.ProcessInput();
            if (IProcess.NextProcess.Break == result)
                return;
        }
    }
}
