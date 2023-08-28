using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    public override void Move()
    {
        if (playerRb == null)
        {
            Debug.LogError("playerRb (Rigidbody2D) is not initialized properly.");
            return;
        }

        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector2 moveDir = new Vector2(moveHorizontal, moveVertical).normalized;
        playerRb.velocity = moveDir * playerSpeed;

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10;
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        targetDirection = (targetPosition - transform.position).normalized;

        if (targetDirection != Vector3.zero)
        {
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle+90, Vector3.forward);
            playerRb.SetRotation(targetRotation);
        }
    }
}
