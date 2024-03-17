using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdelState : MoveMentBaseState
{
    // abstract 함수 써주면 빨간줄 그어지기에 클래스 도 abstract 추가해줘야한다. --> override 로 재정의 해줘야함
    public override void EnterState(Player movement) { }


    // 이 로직은 스위치문으로 애니메이션 상태정의하는거와 비슷하다고 생각하는게 이해가 빠를듯
    public override void UpdateState(Player movement)
    {   // 플레이어객체의 벡터값이 0.1f보다 클 경우  , 이 때 Direction은 프로퍼티로 선언해서 쓸 수 있었던 것
        if (movement.Direction.magnitude > 0.1f)
        {
            // Player Walk 또는 Run 상태로 변경
            Debug.Log("Walk or Run");
            movement.SwitchState(Input.GetKey(KeyCode.LeftShift) ? movement.Run : movement.Walk);
        }
        else if (!GameManager.Instance.player.isJumping && GameManager.Instance.player.IsGround())
        {
            Debug.Log("Jumping");
            ExitState(movement, movement.Jumping);
        }
    }

    public override void ExitState(Player movement, MoveMentBaseState nextSate)
    {

    }
}
