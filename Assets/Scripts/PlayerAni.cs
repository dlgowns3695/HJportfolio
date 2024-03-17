using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

/** �÷��̾��� �ִϸ��̼� ��ũ��Ʈ */
public class PlayerAni : MonoBehaviour
{
    // �ִϸ��̼� ���� ����
    private enum PlayerAniState
    {
        Idel,
        Walk,
        Run,
        Jump,
        /*WalkBack,*/
        Attack
    }

    // �ִϸ����͸� �����ϱ� ����
    private Animator anim;
    
    private PlayerAniState state;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }


    /** �ʱ�ȭ �κ� */
    private void Awake()
    {
        
        state = PlayerAniState.Idel;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimationState();

    }

    

    private void UpdateAnimationState()
    {
        // ���� �÷��̾��� ���� ������
        bool isJumping = GameManager.Instance.player.isJumping;
        // bool isGround = GameManager.Instance.player.grounded; // ���� �ִ��� ����
        bool isStop = GameManager.Instance.player.isStop; // �����ִ��� ����
        bool isRun = GameManager.Instance.player.isRuning; // �޸��� ������ ����
        // bool isAttack = GameManager.Instance.player.IsAttack(); // ���� ������ ����

        // bool jumpAni = !isGround && isJumping; // ���̸鼭 �������� �ƴϿ�����

        switch (state)
        {
            case PlayerAniState.Idel:
                if (true)
                {
                    state = PlayerAniState.Jump;
                }
                state = isStop ? PlayerAniState.Idel : (isRun ? PlayerAniState.Run : PlayerAniState.Walk);
                break;
            case PlayerAniState.Walk:
                if (true)
                {
                    state = PlayerAniState.Jump;
                }
                state = isRun ? PlayerAniState.Run : (isStop ? PlayerAniState.Idel : PlayerAniState.Walk);
                break;
            case PlayerAniState.Run:
                if (true)
                {
                    state = PlayerAniState.Jump;
                }
                state = isStop ? PlayerAniState.Idel : (isRun ? PlayerAniState.Run : PlayerAniState.Walk);
                break;
            case PlayerAniState.Jump:
                if (true)
                {
                    state = PlayerAniState.Jump;
                }
                state = isRun ? PlayerAniState.Run : (isStop ? PlayerAniState.Idel : PlayerAniState.Walk);

                break;
                
        }

        // �ִϸ����Ϳ� ���� ���¸� �����Ͽ� �ִϸ��̼��� ����
        anim.SetInteger("state", (int)state);
    }




}
