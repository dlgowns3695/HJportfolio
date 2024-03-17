using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdelState : MoveMentBaseState
{
    // abstract �Լ� ���ָ� ������ �׾����⿡ Ŭ���� �� abstract �߰�������Ѵ�. --> override �� ������ �������
    public override void EnterState(Player movement) { }


    // �� ������ ����ġ������ �ִϸ��̼� ���������ϴ°ſ� ����ϴٰ� �����ϴ°� ���ذ� ������
    public override void UpdateState(Player movement)
    {   // �÷��̾ü�� ���Ͱ��� 0.1f���� Ŭ ���  , �� �� Direction�� ������Ƽ�� �����ؼ� �� �� �־��� ��
        if (movement.Direction.magnitude > 0.1f)
        {
            // Player Walk �Ǵ� Run ���·� ����
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
