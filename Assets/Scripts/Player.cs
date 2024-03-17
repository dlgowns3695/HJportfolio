using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UIElements;


/** �÷��̾� ���� ��ũ��Ʈ */

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

    private Vector3 dir = Vector3.zero; // �÷��̾��� ������ �����ϱ� ���� ����
    
    [Header("Ray Cast")]
    [SerializeField] private LayerMask groundLayer;

    /** �÷��̾� ���� üũ�� ���� ������ */

    private MoveMentBaseState currentState;
    [HideInInspector] public IdelState Idle = new();
    [HideInInspector] public WalkingState Walk = new();
    [HideInInspector] public RunningState Run = new();
    [HideInInspector] public JumpingState Jumping = new();

    // public bool grounded = false; // ���� �ƴϴ�, �⺻���� ���� �Ұ��ϰ� ����, true�� ���� ����
    public bool isRuning = false;    // �ٴ� �� �ƴϴ� �⺻��
    public bool isStop = false;      // ���� �� �ƴϴ� �⺻��
    public bool isJumping = false;   // �������� �� �ƴϴ� �⺻��

    /** �ٸ� Ŭ�������� ����� �� �ְ� ������Ƽ�� ���� */
    public Vector3 Direction
    {
        get { return dir; }
    }
    /** �ʱ�ȭ ex(������Ʈ���� ��������)*/
    private void Awake()
    {
        anim = FindObjectOfType<Animator>();
        rigid = GetComponent<Rigidbody>();
        SwitchState(Idle); // �÷��̾��� ���´� ó������ ���̵���·� �ֱ� ���� �ʱ�ȭ, SwitchState(Idle(MoveMentBaseState Ÿ����)); �Ű������� Idel�� ����
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
        currentState.UpdateState(this); // MoveMentBaseState�� ������Ʈ �޼��忡 Player, �ڱ��ڽ��� ��ü�� �ѱ�� --> �� �� ������Ʈ �� ������ ������ �ȴ�.(ex.�ȱ���� ��� ������Ʈ.)
    }
    private void FixedUpdate()
    {
        PlayerMove();
    }

    public void SwitchState(MoveMentBaseState state)
    {
        // MoveMentBaseState�� currentState�̴� (������ ����������)
        // MoveMentBaseState�� = state ==> ���ʿ��� �ʱ⿡ Idel�� �Ű������� ����, �⺻,�ȱ�,�ٱ� ��� ���� �� ����
        // currentState = Idel ��
        currentState = state;
        // MoveMentBaseState �� EnterState �޼��� ȣ���� �� �Ű������� �ڱ��ڽ��� ��ü�� ���� Player --> ��Ʈ�� ������ ���󰡺���
        // ó�� �������� �� �������� �޼�����
        currentState.EnterState(this);

    }

    // ����(Ŭ����)�� ���� �ڷ�, ������ ���� �ӵ� ������ �ٴ� ���ǵ�� ������Ʈ
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

    // �ٲ� �ִϸ��̼��� �̸��� true, false���� �Ű������� �޴´�.
    public void SetAnimationState(string animationName, bool trigger)
    {
        anim.SetBool(animationName, trigger);
    }

    #region �÷��̾� ���� ����
    private void PlayerMove()
    {
        // Ű�Է��� ���������� X,Z�� �� �Ҵ�
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.z = Input.GetAxisRaw("Vertical");

        // �÷��̾��� ��,�� ->Vertical �� -1, 0, 1 ������ �յ� ���� (W SŰ),
        // �÷��̾��� ��,�� ->Horizontal �� -1, 0, 1 ������ �յ� ���� (A DŰ)
        dir = (transform.forward * dir.z) + (transform.right * dir.x);
        dir.Normalize(); // �밢���� �� �������°� �����ϴ� ����ȭ 

        rigid.MovePosition(this.gameObject.transform.position + dir * plyaerMoveSpeed * Time.fixedDeltaTime);
    }
    #endregion // �÷��̾� ���� ����

    #region ���콺 ����

    private void CameraCharacterRotation()
    {
        // ���콺�� X�� �������� �����Ͽ� ȸ�� ������ ����
        float yRotation = Input.GetAxisRaw("Mouse X");

        // �÷��̾� ĳ������ ȸ���� ���� ���͸� ����
        // Y��(��,�� ==> ���콺�� X����) ȸ���� ��� * �ΰ���
        Vector3 characterRotationY = new Vector3(0f, yRotation, 0f) * lookSensitivy;

        // �¿츦 �������� Y�ุ ������ ��
        // ȸ�� ������ Quaternion.Euler�� ��ȯ (X,Y,Z������ ȸ���� ����)
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(characterRotationY));

        // ���ʹϿ����� ��ȯ �� �� --> 0f, ���� Y��, 0f
    }

    private void CameraRotation()
    {
        // ���콺�� Y�� �������� �����Ͽ� ȸ�� ������ ���� (�� �Ʒ��� �ǵ������ X���� ������)
        float xRotation = Input.GetAxisRaw("Mouse Y");

        // cameraRotationX(ī�޶��� ���Ʒ���) = xRotation(���� X��) * �ΰ���
        float cameraRotationX = xRotation * lookSensitivy;

        currentCameraRotationX -= cameraRotationX; // +=���� �ϸ� ������ ���°� ���ͼ� -=�� ���־����
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit); // Clamp�Լ��� �������� ���ذ�, �ּڰ�, �ִ�

        // ������ �Ҵ� �� Y, Z ���� ���� �������� �ʱ� ���� new �� ����Ͽ� ���ο� X��, 0f, 0f�� ���
        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f); //


    }
    #endregion // ���콺 ���� 

    #region ����
    public bool IsAttack()
    {
        return Input.GetButtonDown("Fire1") == true;
    }
    #endregion // ����

    #region ��������üũ
    public void IsStop()
    {
        // ���� �ӵ��� ũ�Ⱑ �Ӱ谪���� ������ �����ִ� ���·� �Ǵ�
        if (dir == Vector3.zero)
        {
            isStop = true;
        }
        else
        {
            isStop = false;
        }

    }
    #endregion // ��������üũ

    #region ����
    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGround()) // ���� ����ְ�(true) ���� Ű�� ���� ���
        {
            Vector3 JumpVelocity = Vector3.up * jumpPower;
            rigid.AddForce(JumpVelocity, ForceMode.Impulse);
            /*grounded = false; // ���� �� ������ ������ ���·� ����*/
            //Debug.Log("Jump!");
            isJumping = true;
        }
    }
    #endregion // ����
    

    #region �� ����

    public bool IsGround() // ���ٴ����� üũ
    {
        return Physics.Raycast(transform.position, Vector3.down, 1f, groundLayer); // �ٴڿ� ���� ���ٴ��̸�
    }
    #endregion // �� ����

}
