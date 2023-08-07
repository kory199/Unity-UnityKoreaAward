using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] GameObject[] panels = null;
    // lobby : 0 , stage : 1 

    [SerializeField] Button stageBut = null;

    private GameObject curPanel = null;

    private void Awake()
    {
        curPanel = panels[0];

        stageBut.onClick.AddListener(() => ShowUI(panels[1])); 
    }

    private void ShowUI(GameObject showUIPanel)
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(panel == showUIPanel);
        }
    }
}