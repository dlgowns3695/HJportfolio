using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : MoveMentBaseState
{
    // �������°ź��� ���ȭ�� ����, ���ڵ� ��������
    const string WALK = "Walk";

    // abstract �Լ� ���ָ� ������ �׾����⿡ Ŭ���� �� abstract �߰�������Ѵ�.
    // �Ű������� PlayerŸ���� �� ������ Player���� ����ó���� �ϰ� �ֱ� �����̴�

    // ���� ���� �� ���� �� �޼���
    public override void EnterState(Player movement)
    {
        movement.SetAnimationState(WALK, true);
    }

    // ���� ���� �� ��� ������Ʈ �� �޼���
    public override void UpdateState(Player movement)
    {
        // ���� ����ƮŰ�� ���ȴٸ� 
        if (Input.GetKey(KeyCode.LeftShift))
            // ���� �ִϸ��̼� �� ���� Run ��Ų��
            ExitState(movement, movement.Run);
        else if (!GameManager.Instance.player.isJumping && GameManager.Instance.player.IsGround())
        {
            ExitState(movement, movement.Jumping);
        }
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
        movement.SetAnimationState(WALK, false); 
        // SwitchState�� �Ű������� MoveMentBaseState Ÿ�Ե�(��ӹ���) ���̵�, �ȱ�, �ٱ� �� �� �Ű������� ���� �� �ֵ�
        // Player�� �ִϸ��̼��� ����Ī�Ѵ�, �ι�° �Ű������� ��� �� �ִϸ��̼�����.
        movement.SwitchState(nextSate); 
        
    }
}
