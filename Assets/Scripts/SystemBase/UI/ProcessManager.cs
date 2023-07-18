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
        // 이미 이벤트 시스템이 씬에 존재하는지 확인
        EventSystem existingEventSystem = FindObjectOfType<EventSystem>();
        if (existingEventSystem != null)
        {
            // 이미 이벤트 시스템이 존재하는 경우, 추가 생성 필요 없음
            return;
        }

        // 이벤트 시스템이 존재하지 않는 경우, 새로운 이벤트 시스템 생성
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
