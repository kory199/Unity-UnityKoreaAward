using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    private void InitComponent()
    {
        if (gameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            playerRb = rigidbody;
        else
            playerRb = gameObject.AddComponent<Rigidbody>();
    }

    private void InitSetting()
    {
        playerSpeed = 100f;

        // Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
}
