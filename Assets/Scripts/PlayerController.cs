using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : Unit
{

    enum Player_State
    {
        Idle,
        Jumping,
        Falling
    }

    //private float fCurrentSpeed = 0;
    private float fLeftSpeed = 0;
    private float fRightSpeed = 0;

    private float fMinSpeed = 0;
    private float fNormalSpeed = 5;
    private float fMaxSpeed = 10;
    //bool m_bJumping = false;

    private float m_fGravity = 15;
    private float m_fJumpingPower = 10;
    private float m_fJumpChargeTime = 0;
    private float m_fVerticalSpeed = 0; // y축 속도(점프/중력)

    private Player_State NowState = Player_State.Idle;


    public TextMeshProUGUI debugText;
    public TextMeshProUGUI debugText2;

    private Vector3 MyPos = new Vector3();


    private void Awake()
    {
        Width = 100.1f;
        Height = 146.3f;
    }
    void Start()
    {
        debugText = GameObject.Find("debugText").GetComponent<TextMeshProUGUI>();
        debugText2 = GameObject.Find("debugText2").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        MyPos = transform.position;
        
        PlayerMoveControll();
        PlayerJumpControll();

        if (MyPos.y < -5)
        {
            MyPos.y = 10;
            //ExitGame();// 테스트용 게임 종료
        }

        transform.position = MyPos;


    }

    private void LateUpdate()
    {
        PlayerCollisionCheck();


        if (Input.GetKeyDown(KeyCode.T))
        {
            debugText.gameObject.SetActive(!debugText.gameObject.activeSelf);
            debugText2.gameObject.SetActive(!debugText2.gameObject.activeSelf);
        }

        debugText.text = $"m_fVerticalSpeed: {m_fVerticalSpeed}";

        
    }


    void Jump()
    {
        if (m_fVerticalSpeed == 0)
        {
            m_fVerticalSpeed = m_fJumpingPower;
            SetMyState(Player_State.Jumping);
        }
    }

    void PlayerJumpControll()
    {

        // 중력 적용
        if (GetMyState(Player_State.Jumping))
        {
            m_fVerticalSpeed -= m_fGravity * Time.deltaTime;
            MyPos.y += m_fVerticalSpeed * Time.deltaTime;

            if (m_fVerticalSpeed < 0)
            {
                SetMyState(Player_State.Falling);
            }

        }

        if (GetMyState(Player_State.Falling))
        {
            if (m_fVerticalSpeed >= -m_fGravity)
                m_fVerticalSpeed -= m_fGravity * Time.deltaTime;

            MyPos.y += m_fVerticalSpeed * Time.deltaTime;

        }
    }


    void PlayerMoveControll()
    {
        Define.Camera_State NowCameraState = Managers.Camera_Instance.Get_Camera_State;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            MyPos.y += Time.deltaTime * fMaxSpeed;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            MyPos.y -= Time.deltaTime * fMaxSpeed;
        }



        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (Input.GetKey(KeyCode.X))
            {
                if (fLeftSpeed < fMaxSpeed)
                    fLeftSpeed += Time.deltaTime * 40;
            }
            else
            {
                if (fLeftSpeed < fNormalSpeed)
                    fLeftSpeed += Time.deltaTime * 40;
            }

            if (NowCameraState == Define.Camera_State.R_0)
                MyPos.x -= Time.deltaTime * fLeftSpeed;
            else if (NowCameraState == Define.Camera_State.R_90)
                MyPos.z -= Time.deltaTime * fLeftSpeed;
            else if (NowCameraState == Define.Camera_State.R_180)
                MyPos.x += Time.deltaTime * fLeftSpeed;
            else if (NowCameraState == Define.Camera_State.R_270)
                MyPos.z += Time.deltaTime * fLeftSpeed;
        }
        else
        {
            if (fLeftSpeed > fMinSpeed)
                fLeftSpeed -= Time.deltaTime * 20;
            else
                fLeftSpeed = fMinSpeed;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.X))
            {
                if (fRightSpeed < fMaxSpeed)
                    fRightSpeed += Time.deltaTime * 40;
            }
            else
            {
                if (fRightSpeed < fNormalSpeed)
                    fRightSpeed += Time.deltaTime * 40;
            }

            if (NowCameraState == Define.Camera_State.R_0)
                MyPos.x += Time.deltaTime * fRightSpeed;
            else if (NowCameraState == Define.Camera_State.R_90)
                MyPos.z += Time.deltaTime * fRightSpeed;
            else if (NowCameraState == Define.Camera_State.R_180)
                MyPos.x -= Time.deltaTime * fRightSpeed;
            else if (NowCameraState == Define.Camera_State.R_270)
                MyPos.z -= Time.deltaTime * fRightSpeed;
        }
        else
        {
            if (fRightSpeed > fMinSpeed)
                fRightSpeed -= Time.deltaTime * 20;
            else
                fRightSpeed = fMinSpeed;
        }

        if (Input.GetKey(KeyCode.C))
        {
            if (!GetMyState(Player_State.Falling))
                Jump();
        }

        if (Input.GetKey(KeyCode.P))
        {
            MyPos.y -= m_fGravity * Time.deltaTime * 0.5f;
        }

    }



    enum CollisionDirection { None, Top, Bottom, Left, Right }

    void PlayerCollisionCheck()
    {
        CollisionDirection colDir = CollisionDirection.None;
        GameObject collidedBlock = null;

        if (Managers.Pool_Instance.Dictionary_AllGameObject.ContainsKey("Block"))
        {
            foreach (var box in Managers.Pool_Instance.Dictionary_AllGameObject["Block"])
            {

                colDir = GetCollisionDirection(gameObject, box.gameObject);

                if (colDir != CollisionDirection.None)
                {
                    collidedBlock = box.gameObject;
                    break;
                }


            }
        }

        
        if (colDir == CollisionDirection.Left || colDir == CollisionDirection.Right)
        {
            // 측면 충돌: x/z 위치만 보정, 점프 상태 유지
            debugText2.text = "측면 충돌";
        }
        else if (colDir == CollisionDirection.Bottom)
        {
            // 플레이어가 블록 아래에 부딪힘(천장)
            m_fVerticalSpeed = 0;
            debugText2.text = "천장 충돌";
        }
        else if (colDir == CollisionDirection.Top && collidedBlock != null)
        {
            // 블록 위에 착지
            var blockRect = CollisionChecker.Update_GameObject(collidedBlock);
            MyPos.y = blockRect.top;
            m_fVerticalSpeed = 0;
            SetMyState(Player_State.Idle);
            debugText2.text = "착지(블록 위)";
        }
        else
        {
            //debugText2.text = "충돌XX";
            SetMyState(Player_State.Falling);
        }
    }

    // 충돌 방향 판정 함수
    CollisionDirection GetCollisionDirection(GameObject player, GameObject block)
    {
        var playerRect = CollisionChecker.Update_GameObject(player);
        var blockRect = CollisionChecker.Update_GameObject(block);

        // AABB 충돌 체크
        bool isColliding = CollisionChecker.bRectCollsionCheck(player, block); // 충돌체크

        if (!isColliding)
            return CollisionDirection.None;

        float fromTop = Mathf.Abs(playerRect.bottom - blockRect.top);
        float fromBottom = Mathf.Abs(playerRect.top - blockRect.bottom);
        float fromLeft = Mathf.Abs(playerRect.right - blockRect.left);
        float fromRight = Mathf.Abs(playerRect.left - blockRect.right);
        float minDist = Mathf.Min(fromTop, fromBottom, fromLeft, fromRight);

        CollisionChecker.PushDestObjPosition(player, block); // 밀기

        if (minDist == fromTop && m_fVerticalSpeed <= 0 && player.transform.position.y >= block.transform.position.y)
            return CollisionDirection.Top; // 위에서 착지
        if (minDist == fromBottom && m_fVerticalSpeed > 0 && player.transform.position.y < block.transform.position.y)
            return CollisionDirection.Bottom; // 아래에서 천장 충돌
        if (minDist == fromLeft)
            return CollisionDirection.Left;
        if (minDist == fromRight)
            return CollisionDirection.Right;

        return CollisionDirection.None;
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); //
#endif
    }


    bool GetMyState(Player_State currentstate)
    {
        return NowState == currentstate ? true : false;
    }

    void SetMyState(Player_State state)
    {
        if (NowState != state)
            NowState = state;
    }

}
