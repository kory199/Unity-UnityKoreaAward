using UnityEngine;

public class CursorChange : MonoBehaviour
{
    [SerializeField] Texture2D cursorImg = null;

    void Start()
    {
        Cursor.SetCursor(cursorImg, Vector2.zero, CursorMode.Auto);
    }
}