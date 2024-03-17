using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : MoveMentBaseState
{
    // �������°ź��� ���ȭ�� ����, ���ڵ� ��������
    const string JUMP = "Jump";

    // abstract �Լ� ���ָ� ������ �׾����⿡ Ŭ���� �� abstract �߰�������Ѵ�.
    // �Ű������� PlayerŸ���� �� ������ Player���� ����ó���� �ϰ� �ֱ� �����̴�

    // ���� ���� �� ���� �� �޼���
    public override void EnterState(Player movement)
    {
        movement.SetAnimationState(JUMP, true);
        
    }

    // ���� ���� �� ��� ������Ʈ �� �޼���
    public override void UpdateState(Player movement)
    {
        // ������ �� �� �ִ� ���°� �ȴٸ� --> ���� �ִٸ�
        bool jumping = !GameManager.Instance.player.isJumping && GameManager.Instance.player.IsGround();
        if (jumping)
            // ���� �ִϸ��̼� �� ���� Run ��Ų��
            ExitState(movement, movement.Walk);
        // ������ ���� 0.1f���� ������ ���̵� ���·� �ٲ۴�
        else if (movement.Direction.magnitude < 0.1f)
            ExitState(movement, movement.Idle);

        // ����(Ŭ������ ���� �����̴� �ӵ��� �����Ѵ�)
        
        movement.UpdateSpeed(this);
    }

    // ���� �� �޼��� �� ���ְ�, ���ο� ���·� ���� �ϱ� ����, �÷��̾�� MoveMentBaseState -> currentState �� ���� �����߾���
    public override void ExitState(Player movement, MoveMentBaseState nextSate)
    {
        // ù��° �Ű������� ���� (��, �ʱ⿡ ���� �ִϸ��̼� ����)�� ���ش�
        movement.SetAnimationState(JUMP, false); 
        // SwitchState�� �Ű������� MoveMentBaseState Ÿ�Ե�(��ӹ���) ���̵�, �ȱ�, �ٱ� �� �� �Ű������� ���� �� �ֵ�
        // Player�� �ִϸ��̼��� ����Ī�Ѵ�, �ι�° �Ű������� ��� �� �ִϸ��̼�����.
        movement.SwitchState(nextSate); 
        
    }
}
