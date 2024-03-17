using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 공통된 애니메이션 상태처리들 상속해주기 위한 스크립트(부모임)
public abstract class MoveMentBaseState
{
    // abstract 함수 써주면 빨간줄 그어지기에 클래스 도 abstract 추가해줘야한다.
    // 매개변수에 Player타입을 쓴 이유는 Player에서 상태처리를 하고 있기 때문이다

    // 상태 변경 후 진입 될 메서드
    public abstract void EnterState(Player movement);

    // 상태 변경 후 계속 업데이트 될 메서드
    public abstract void UpdateState(Player movement);

    // 진입 된 메서드 는 없애고, 새로운 상태로 변경 하기 위한, 플레이어에서 MoveMentBaseState -> currentState 로 변수 선언했었음
    public abstract void ExitState(Player movement, MoveMentBaseState nextSate);
}
