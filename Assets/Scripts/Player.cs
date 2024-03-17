using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UIElements;


/** 플레이어 제어 스크립트 */

public class Player : MonoBehaviour
{
    private Rigidbody rigid;
    private Animator anim;
    
    [Header("Player Moving")]
    [SerializeField] private float walkSpeed = 3f, backwalkSpeed = 2f;
    [SerializeField] private float runSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float playerForward = 15f;
    [SerializeField] private float plyaerRotationSpeed = 10f;
    private float plyaerMoveSpeed = 0f;
    

    [Header("Mouse Controller")]
    [SerializeField] private float lookSensitivy;
    
    [Header("Camare Controller")]
    [SerializeField] private float cameraRotationLimit;
    [SerializeField] Camera theCamera;
    private float currentCameraRotationX = 0f;

    private Vector3 dir = Vector3.zero; // 플레이어의 움직임 제어하기 위해 설정
    
    [Header("Ray Cast")]
    [SerializeField] private LayerMask groundLayer;

    /** 플레이어 상태 체크를 위한 변수들 */

    private MoveMentBaseState currentState;
    [HideInInspector] public IdelState Idle = new();
    [HideInInspector] public WalkingState Walk = new();
    [HideInInspector] public RunningState Run = new();
    [HideInInspector] public JumpingState Jumping = new();

    // public bool grounded = false; // 땅이 아니다, 기본으로 점프 불가하게 설정, true면 점프 가능
    public bool isRuning = false;    // 뛰는 중 아니다 기본값
    public bool isStop = false;      // 정지 가 아니다 기본값
    public bool isJumping = false;   // 점프시전 중 아니다 기본값

    /** 다른 클래스에서 사용할 수 있게 프로퍼티로 만듬 */
    public Vector3 Direction
    {
        get { return dir; }
    }
    /** 초기화 ex(컴포넌트들을 쓰기위함)*/
    private void Awake()
    {
        anim = FindObjectOfType<Animator>();
        rigid = GetComponent<Rigidbody>();
        SwitchState(Idle); // 플레이어의 상태는 처음부터 아이들상태로 있기 위한 초기화, SwitchState(Idle(MoveMentBaseState 타입임)); 매개변수로 Idel이 들어갔음
    }
    void Update()
    {
        anim.SetFloat("HInput", dir.x);
        anim.SetFloat("VInput", dir.z);

        IsGround();
        IsStop();

        Jump();

        CameraRotation();
        CameraCharacterRotation();

        // SetRunState(Input.GetKey(KeyCode.LeftShift));
        currentState.UpdateState(this); // MoveMentBaseState의 업데이트 메서드에 Player, 자기자신의 객체를 넘긴다 --> 그 후 업데이트 될 로직이 실행이 된다.(ex.걷기상태 계속 업데이트.)
    }
    private void FixedUpdate()
    {
        PlayerMove();
    }

    public void SwitchState(MoveMentBaseState state)
    {
        // MoveMentBaseState는 currentState이다 (위에서 선언해줬음)
        // MoveMentBaseState는 = state ==> 위쪽에서 초기에 Idel로 매개변수가 들어갔음, 기본,걷기,뛰기 등등 들어올 수 있음
        // currentState = Idel 임
        currentState = state;
        // MoveMentBaseState 의 EnterState 메서드 호출을 함 매개변수로 자기자신의 객체를 넣음 Player --> 컨트롤 눌러서 따라가보기
        // 처음 진입했을 때 상태제어 메서드임
        currentState.EnterState(this);

    }

    // 상태(클래스)에 따라 뒤로, 앞으로 가는 속도 조절과 뛰는 스피드로 업데이트
    public void UpdateSpeed(MoveMentBaseState state)
    {
        if ( state == Walk)
        {
            isRuning = false;
            plyaerMoveSpeed = dir.y > 0f ? walkSpeed : backwalkSpeed;
        }
        else if(state == Run)
        {
            isRuning = true;
            plyaerMoveSpeed = runSpeed;
        }
    }

    // 바뀔 애니메이션의 이름과 true, false값을 매개변수로 받는다.
    public void SetAnimationState(string animationName, bool trigger)
    {
        anim.SetBool(animationName, trigger);
    }

    #region 플레이어 무브 제어
    private void PlayerMove()
    {
        // 키입력을 받은값으로 X,Z축 값 할당
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.z = Input.GetAxisRaw("Vertical");

        // 플레이어의 앞,뒤 ->Vertical 의 -1, 0, 1 값으로 앞뒤 조절 (W S키),
        // 플레이어의 왼,오 ->Horizontal 의 -1, 0, 1 값으로 앞뒤 조절 (A D키)
        dir = (transform.forward * dir.z) + (transform.right * dir.x);
        dir.Normalize(); // 대각선일 때 빨라지는걸 방지하는 정규화 

        rigid.MovePosition(this.gameObject.transform.position + dir * plyaerMoveSpeed * Time.fixedDeltaTime);
    }
    #endregion // 플레이어 무브 제어

    #region 마우스 무브

    private void CameraCharacterRotation()
    {
        // 마우스의 X축 움직임을 감지하여 회전 각도를 결정
        float yRotation = Input.GetAxisRaw("Mouse X");

        // 플레이어 캐릭터의 회전을 위한 벡터를 생성
        // Y축(좌,우 ==> 마우스의 X축임) 회전만 고려 * 민감도
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivy;

        // 좌우를 돌려보면 Y축만 변경이 됨
        // 회전 각도를 Quaternion.Euler로 변환 (X,Y,Z순으로 회전값 구함)
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(characterRotationY));

        // 쿼터니온으로 변환 한 값 --> 0f, 구한 Y값, 0f
    }

    private void CameraRotation()
    {
        // 마우스의 Y축 움직임을 감지하여 회전 각도를 결정 (위 아래로 건드려보면 X값이 조절됨)
        float xRotation = Input.GetAxisRaw("Mouse Y");

        // cameraRotationX(카메라의 위아래는) = xRotation(구한 X값) * 민감도
        float cameraRotationX = xRotation * lookSensitivy;

        currentCameraRotationX -= cameraRotationX; // +=으로 하면 반전된 상태가 나와서 -=로 해주어야함
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit); // Clamp함수로 범위제한 기준값, 최솟값, 최댓값

        // 이전에 할당 된 Y, Z 값을 같이 가져가지 않기 위해 new 를 사용하여 새로운 X값, 0f, 0f를 사용
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f); //


    }
    #endregion // 마우스 무브 

    #region 공격
    public bool IsAttack()
    {
        return Input.GetButtonDown("Fire1") == true;
    }
    #endregion // 공격

    #region 정지상태체크
    public void IsStop()
    {
        // 현재 속도의 크기가 임계값보다 작으면 멈춰있는 상태로 판단
        if (dir == Vector3.zero)
        {
            isStop = true;
        }
        else
        {
            isStop = false;
        }

    }
    #endregion // 정지상태체크

    #region 점프
    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGround()) // 땅에 닿아있고(true) 점프 키가 눌린 경우
        {
            Vector3 JumpVelocity = Vector3.up * jumpPower;
            rigid.AddForce(JumpVelocity, ForceMode.Impulse);
            /*grounded = false; // 점프 후 땅에서 떨어진 상태로 변경*/
            //Debug.Log("Jump!");
            isJumping = true;
        }
    }
    #endregion // 점프
    

    #region 땅 검출

    public bool IsGround() // 땅바닥인지 체크
    {
        return Physics.Raycast(transform.position, Vector3.down, 1f, groundLayer); // 바닥에 쏴서 땅바닥이면
    }
    #endregion // 땅 검출

}
