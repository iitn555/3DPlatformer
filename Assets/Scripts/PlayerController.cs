using System.Collections;
using System.Collections.Generic;
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
    bool m_bJumping = false;

    private float m_fGravity = 15;
    private float m_fJumpingPower = 10;
    private float m_fJumpChargeTime = 0;

    private Player_State NowState = Player_State.Idle;


    //public TextMeshProUGUI debugText; // 디버그용
    //public TextMeshProUGUI debugText2; // 디버그 스피드

    private Vector3 MyPos = new Vector3();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMoveControll();
    }

    void PlayerMoveControll()
    {
        MyPos = transform.position;

        Define.Camera_State NowCameraState = Managers.Camera_Instance.Get_Camera_State;


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

        //if (Input.GetKey(KeyCode.C))
        //{
        //    if (!GetMyState(Player_State.Falling))
        //        Jump();
        //}

        if (Input.GetKey(KeyCode.P))
        {
            MyPos = this.transform.position;
            MyPos.y -= m_fGravity * Time.deltaTime * 0.5f;
            
        }


        transform.position = MyPos;


    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
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
