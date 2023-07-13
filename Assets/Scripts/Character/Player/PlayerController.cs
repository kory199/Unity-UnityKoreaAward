using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player
{
    public override void  Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X");

        Vector3 moveDir = new Vector3(horizontalInput, 0, verticalInput).normalized;
        playerRb.velocity = moveDir * playerSpeed;

        // 게임 뷰에 보이는 마우스 위치를 가져옴
        Vector3 mousePosition = Input.mousePosition;

        // 마우스 위치를 월드 좌표로 변환
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            // 플레이어의 위치와 마우스 위치 간의 벡터를 계산하여 플레이어를 회전시킴
            Vector3 targetDirection = hit.point - transform.position;
            targetDirection.y = 0f; // y 축 회전 방지

            if (targetDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                playerRb.MoveRotation(targetRotation);
            }
        }
    }
}
