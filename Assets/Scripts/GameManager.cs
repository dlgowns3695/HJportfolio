using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // public 을 걸면 보안상 취약하기 때문에 private
    private static GameManager instance = null;
    public Player player;

    // GameManager 의 프로퍼티
    public static GameManager Instance
    {
        get
        {
            // 만약 게임매니져의 instance가 null이라면
            if (instance == null)
            {// return으로 null값을 반환
                return null;
            }
            // 그게 아니라면 게임매니져의 instance 를 반환한다. (디자인패턴 : 싱글톤, 정적메모리에 오직 하나만 존재해야 하기 때문에)
            return instance;
        }
    }
    /** 초기화 부분 */
    private void Awake()
    {
        // 만약 instance가 null 이라면 
        if (instance == null)
        {// instance는 게임매니져 자신으로 값 할당
            instance = this;
            // 씬이 전환 되어도 유지되도록
            DontDestroyOnLoad(this.gameObject);
        }
        // 그렇지 않다면 instance가 존재하면, 하나만 존재해야하기 때문에 자기자신 오브젝트를 파괴
        else
        {
            Destroy(this.gameObject);
        }
    }

    // 플레이어 스크립트의 땅검출 변수 가져오기 위함 프로퍼티 구현
/*    public bool IsGrounded
    {
        get { return player.grounded; }
    }*/


}
