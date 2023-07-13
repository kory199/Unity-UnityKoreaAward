using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        JsonLoader loader = new JsonLoader();
        loader.Load();

        var uiManager = new GameObject().AddComponent<UIManager>();
        uiManager.name = "UIManager";
    }

}
