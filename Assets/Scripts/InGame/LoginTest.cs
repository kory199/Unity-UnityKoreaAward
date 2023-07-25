using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginTest : MonoBehaviour
{
    [Header("Account")]
    [SerializeField] TMP_InputField inputField_ID = null;
    [SerializeField] TMP_InputField inputField_PW = null;
    [SerializeField] Button createIDButton = null;
    [SerializeField] Button loginButton = null;

    string input_ID;
    string input_PW;

    private int currentIndex = 0;

    private void Start()
    {
        inputField_ID.characterLimit = 12;
    }

    public void OnEndEditID()
    {
        input_ID = inputField_ID.text;
        Debug.Log(input_ID);
    }
    public void OnEndEditPW()
    {
        input_PW = inputField_PW.text;
        Debug.Log(input_PW);
    }

    // ���� ���� ����, �α��� ���� ������ �߰� ����
    public void CreateID()
    {
        input_ID = inputField_ID.text;
        input_PW = inputField_PW.text;
        Debug.Log($"ID ���� �Ϸ� : {input_ID}");
    }

    public void Login()
    {
        input_ID = inputField_ID.text;
        input_PW = inputField_PW.text;

        Debug.Log($"Login �Ϸ� : {input_ID}");
    }
}