using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����� �ִϸ��̼� ����ó���� ������ֱ� ���� ��ũ��Ʈ(�θ���)
public abstract class MoveMentBaseState
{
    // abstract �Լ� ���ָ� ������ �׾����⿡ Ŭ���� �� abstract �߰�������Ѵ�.
    // �Ű������� PlayerŸ���� �� ������ Player���� ����ó���� �ϰ� �ֱ� �����̴�

    // ���� ���� �� ���� �� �޼���
    public abstract void EnterState(Player movement);

    // ���� ���� �� ��� ������Ʈ �� �޼���
    public abstract void UpdateState(Player movement);

    // ���� �� �޼��� �� ���ְ�, ���ο� ���·� ���� �ϱ� ����, �÷��̾�� MoveMentBaseState -> currentState �� ���� �����߾���
    public abstract void ExitState(Player movement, MoveMentBaseState nextSate);
}
