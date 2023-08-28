using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnMouseInteraction : MonoBehaviour
{
    EventTrigger _eventTrigger = null;

    private void Awake()
    {
        if (_eventTrigger == null)
        {
            if (TryGetComponent<EventTrigger>(out _eventTrigger) == false)
            {
                _eventTrigger = gameObject.AddComponent<EventTrigger>();
            }
        }
        EventTrigger.Entry entryDown = new EventTrigger.Entry();
        entryDown.eventID = EventTriggerType.PointerDown;
        entryDown.callback.AddListener((eventdata) => gameObject.transform.DOScale(gameObject.transform.localScale * 0.9f, 0.05f));

        EventTrigger.Entry entryUp = new EventTrigger.Entry();
        entryUp.eventID = EventTriggerType.PointerUp;
        entryUp.callback.AddListener((eventdata) => gameObject.transform.DOScale(gameObject.transform.localScale * (10f / 9f), 0.05f));

        _eventTrigger.triggers.Add(entryDown);
        _eventTrigger.triggers.Add(entryUp);
    }
}