using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningState : MoveMentBaseState
{
    // �������°ź��� ���ȭ�� ����, ���ڵ� ��������
    const string RUN = "Run";

    // abstract �Լ� ���ָ� ������ �׾����⿡ Ŭ���� �� abstract �߰�������Ѵ�.
    // �Ű������� PlayerŸ���� �� ������ Player���� ����ó���� �ϰ� �ֱ� �����̴�

    // ���� ���� �� ���� �� �޼���
    public override void EnterState(Player movement)
    {
        movement.SetAnimationState(RUN, true);
    }

    // ���� ���� �� ��� ������Ʈ �� �޼���
    public override void UpdateState(Player movement)
    {
        // �ٴٰ� Ű ������  ��ũ���·� ���ư���
        if (Input.GetKeyUp(KeyCode.LeftShift))
            // ������ ����, �ι�° �� ���
            ExitState(movement, movement.Walk); 
        else if (!GameManager.Instance.player.isJumping && GameManager.Instance.player.IsGround())
        {
            ExitState(movement, movement.Jumping);
        }
        else if (movement.Direction.magnitude < 0.1f) // �ӵ��� 0.1���� �������� ���̵� ����.
            ExitState(movement, movement.Idle);
        // ���¿� ���� ���ǵ� ����
        movement.UpdateSpeed(this);
    }

    // ���� �� �޼��� �� ���ְ�, ���ο� ���·� ���� �ϱ� ����, �÷��̾�� MoveMentBaseState -> currentState �� ���� �����߾���
    public override void ExitState(Player movement, MoveMentBaseState nextSate)
    {
        // ù��° �Ű������� ���� (��, �ʱ⿡ ���� �ִϸ��̼� ����)�� ���ش�
        movement.SetAnimationState(RUN, false);
        // SwitchState�� �Ű������� MoveMentBaseState Ÿ�Ե�(��ӹ���) ���̵�, �ȱ�, �ٱ� �� �� �Ű������� ���� �� �ֵ�
        // Player�� �ִϸ��̼��� ����Ī�Ѵ�, �ι�° �Ű������� ��� �� �ִϸ��̼�����.
        movement.SwitchState(nextSate);

    }

}
