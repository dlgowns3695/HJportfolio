using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // public �� �ɸ� ���Ȼ� ����ϱ� ������ private
    private static GameManager instance = null;
    public Player player;

    // GameManager �� ������Ƽ
    public static GameManager Instance
    {
        get
        {
            // ���� ���ӸŴ����� instance�� null�̶��
            if (instance == null)
            {// return���� null���� ��ȯ
                return null;
            }
            // �װ� �ƴ϶�� ���ӸŴ����� instance �� ��ȯ�Ѵ�. (���������� : �̱���, �����޸𸮿� ���� �ϳ��� �����ؾ� �ϱ� ������)
            return instance;
        }
    }
    /** �ʱ�ȭ �κ� */
    private void Awake()
    {
        // ���� instance�� null �̶�� 
        if (instance == null)
        {// instance�� ���ӸŴ��� �ڽ����� �� �Ҵ�
            instance = this;
            // ���� ��ȯ �Ǿ �����ǵ���
            DontDestroyOnLoad(this.gameObject);
        }
        // �׷��� �ʴٸ� instance�� �����ϸ�, �ϳ��� �����ؾ��ϱ� ������ �ڱ��ڽ� ������Ʈ�� �ı�
        else
        {
            Destroy(this.gameObject);
        }
    }

    // �÷��̾� ��ũ��Ʈ�� ������ ���� �������� ���� ������Ƽ ����
/*    public bool IsGrounded
    {
        get { return player.grounded; }
    }*/


}
