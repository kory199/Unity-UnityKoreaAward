using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TitleCall : MonoBehaviour
{
    AccountUI account;
 

    IEnumerator Start()
    {
        account = FindObjectOfType<AccountUI>();
        if (account == null)
        {
            account = UIManager.Instance.CreateObject<AccountUI>("UI_SceneTitle", EnumTypes.LayoutType.First);
            yield return new WaitUntil(() => account != null);
        }

        account.OnShow();
    }
}