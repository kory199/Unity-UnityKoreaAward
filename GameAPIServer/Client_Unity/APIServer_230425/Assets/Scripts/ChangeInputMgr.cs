using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeInputMgr : MonoBehaviour
{
    [SerializeField] TMP_InputField[] inputFields;
    [SerializeField] Button saveBut;

    private int currentIndex = 0;

    void Start()
    {
        // 첫 번째 입력 필드에 포커스를 줌
        inputFields[currentIndex].Select();

        // Tap 키 입력 감지를 위한 코루틴 시작
        StartCoroutine(DetectTabKey());

        // Enter 키 입력 감지를 위한 코루틴 시작
        StartCoroutine(DetectEnterKey());
    }

    IEnumerator DetectTabKey()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                yield return StartCoroutine(MoveToNextInputField());
            }

            yield return null;
        }
    }

    IEnumerator DetectEnterKey()
    {
        while(true)
        {
            bool allInputFieldsFilled = true;

            foreach(TMP_InputField inputField in inputFields)
            {
                if(string.IsNullOrEmpty(inputField.text))
                {
                    allInputFieldsFilled = false;
                    break;
                }
            }

            if(allInputFieldsFilled && Input.GetKeyDown(KeyCode.Return))
            {
                print($"capw : {UIManager.Inst.create_pwInput.text}");
                saveBut.Select();
            }

            yield return null;
        }
    }

    IEnumerator MoveToNextInputField()
    {
        // 현재 포커스된 입력 필드를 비활성화
        inputFields[currentIndex].DeactivateInputField();

        // 다음 입력 필드의 인덱스 계산 (배열의 범위를 넘어가는 경우, 인덱스가 다시 처음 부터 시작)
        currentIndex = (currentIndex + 1) % inputFields.Length;

        // 다음 입력 필드에 포커스를 줌
        inputFields[currentIndex].ActivateInputField();

        // 다음 입력 필드로 포커스 이동이 완료될 때까지 대기
        while (inputFields[currentIndex].isFocused)
        {
            yield return null;
        }
    }
}