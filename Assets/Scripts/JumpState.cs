using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : MoveMentBaseState
{
    // 직접쓰는거보단 상수화가 좋다, 숫자도 마찬가지
    const string JUMP = "Jump";

    // abstract 함수 써주면 빨간줄 그어지기에 클래스 도 abstract 추가해줘야한다.
    // 매개변수에 Player타입을 쓴 이유는 Player에서 상태처리를 하고 있기 때문이다

    // 상태 변경 후 진입 될 메서드
    public override void EnterState(Player movement)
    {
        movement.SetAnimationState(JUMP, true);
        
    }

    // 상태 변경 후 계속 업데이트 될 메서드
    public override void UpdateState(Player movement)
    {
        // 점프를 할 수 있는 상태가 된다면 --> 땅에 있다면
        bool jumping = !GameManager.Instance.player.isJumping && GameManager.Instance.player.IsGround();
        if (jumping)
            // 기존 애니메이션 을 끄고 Run 시킨다
            ExitState(movement, movement.Walk);
        // 하지만 값이 0.1f보다 작으면 아이들 상태로 바꾼다
        else if (movement.Direction.magnitude < 0.1f)
            ExitState(movement, movement.Idle);

        // 상태(클래스에 따라 움직이는 속도를 조절한다)
        
        movement.UpdateSpeed(this);
    }

    // 진입 된 메서드 는 없애고, 새로운 상태로 변경 하기 위한, 플레이어에서 MoveMentBaseState -> currentState 로 변수 선언했었음
    public override void ExitState(Player movement, MoveMentBaseState nextSate)
    {
        // 첫번째 매개변수로 들어온 (즉, 초기에 들어온 애니메이션 상태)를 꺼준다
        movement.SetAnimationState(JUMP, false); 
        // SwitchState의 매개변수는 MoveMentBaseState 타입들(상속받은) 아이들, 걷기, 뛰기 등 이 매개변수로 들어올 수 있따
        // Player의 애니메이션을 스위칭한다, 두번째 매개변수로 들어 온 애니메이션으로.
        movement.SwitchState(nextSate); 
        
    }
}
