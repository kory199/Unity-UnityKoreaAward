using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginTest : MonoBehaviour
{
    [SerializeField] InputField inputField_ID;
    [SerializeField] InputField inputField_PW;
    [SerializeField] Button createIDButton;
    [SerializeField] Button loginButton;

    string road_ID;
    string road_PW;

    public void OnEndEdit()
    {
        road_ID = inputField_ID.text;
    }
}
