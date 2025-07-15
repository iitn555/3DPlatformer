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

        transform.position = MyPos;

        if (Input.GetKeyDown(KeyCode.T))
        {
            debugText.gameObject.SetActive(!debugText.gameObject.activeSelf);
            debugText2.gameObject.SetActive(!debugText2.gameObject.activeSelf);
        }
    }

    private void LateUpdate()
    {
        debugText.text = $"m_fVerticalSpeed: {m_fVerticalSpeed}";
        PlayerCollisionCheck();
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

        // 바닥(y=0) 이하로 내려가면 착지
        //if (MyPos.y <= 0)
        //{
        //    MyPos.y = 0;
        //    m_fVerticalSpeed = 0;
        //    SetMyState(Player_State.Idle);
        //}


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
            MyPos = this.transform.position;
            MyPos.y -= m_fGravity * Time.deltaTime * 0.5f;

        }





    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ���ø����̼� ����
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

    void PlayerCollisionCheck()
    {
        bool col = false;

        if (Managers.Pool_Instance.Dictionary_AllGameObject.ContainsKey("Block"))
        {
            foreach (var box in Managers.Pool_Instance.Dictionary_AllGameObject["Block"])
            {
                if (CollisionChecker.RectCollsionAndPush(Managers.Pool_Instance.Dictionary_AllGameObject["Player"][0].gameObject, box.gameObject))
                {
                    SetMyState(Player_State.Idle);

                    col = true;
                    Debug.Log("블록과충돌!");

                    break;

                }

            }
        }

        if (col)
        {
            debugText2.text = "충돌중";
            m_fVerticalSpeed = 0;
        }
        else
        {
            debugText2.text = "충돌XX";

            if (!GetMyState(Player_State.Falling))
            {
                SetMyState(Player_State.Falling);

            }
        }

    }

}
