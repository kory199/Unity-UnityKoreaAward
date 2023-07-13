using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    private void InitComponent()
    {
        if (gameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            playerRb = rigidbody;

    }

    private void InitSetting()
    {
        playerSpeed = 50f;

        Cursor.lockState = CursorLockMode.Confined;
    }
}
