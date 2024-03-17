using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

/** 플레이어의 애니메이션 스크립트 */
public class PlayerAni : MonoBehaviour
{
    // 애니메이션 상태 정의
    private enum PlayerAniState
    {
        Idel,
        Walk,
        Run,
        Jump,
        /*WalkBack,*/
        Attack
    }

    // 애니메이터를 제어하기 위해
    private Animator anim;
    
    private PlayerAniState state;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }


    /** 초기화 부분 */
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
        // 현재 플레이어의 상태 정보들
        bool isJumping = GameManager.Instance.player.isJumping;
        // bool isGround = GameManager.Instance.player.grounded; // 땅에 있는지 여부
        bool isStop = GameManager.Instance.player.isStop; // 멈춰있는지 여부
        bool isRun = GameManager.Instance.player.isRuning; // 달리는 중인지 여부
        // bool isAttack = GameManager.Instance.player.IsAttack(); // 공격 중인지 여부

        // bool jumpAni = !isGround && isJumping; // 땅이면서 점프중이 아니여야함

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

        // 애니메이터에 현재 상태를 전달하여 애니메이션을 변경
        anim.SetInteger("state", (int)state);
    }




}
