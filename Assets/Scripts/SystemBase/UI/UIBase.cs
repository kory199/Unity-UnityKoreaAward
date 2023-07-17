using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIBase : MonoBehaviour, IProcess
{
    public bool activeSelf { get { return gameObject.activeSelf; } }
    private ProcessManager _processManager { get { return UIManager.Instance.processManager; } }
    protected virtual void Awake()
    {
        
    }
    protected virtual void Start()
    {
    }


    public void OnShow()
    {
        gameObject.SetActive(true);
        if (false == _processManager.processingUIStack.Contains(this))
            _processManager.processingUIStack.Push(this);
    }

    public void OnHide()
    {
        gameObject.SetActive(false);

        if (true == _processManager.processingUIStack.Contains(this))
            _processManager.processingUIStack.Pop();

    }

    public abstract IProcess.NextProcess ProcessInput();
}
