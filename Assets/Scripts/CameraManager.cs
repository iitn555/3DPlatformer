using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager
{
    enum Camera_RotateState { Idle, Minus, Plus }
    private Camera_RotateState NowRotateState = Camera_RotateState.Idle;

    private Define.Camera_State Now_State = Define.Camera_State.R_0;
    public Define.Camera_State Get_Camera_State { get { return Now_State; } }

    private Camera Main_Camera;
    private float CenterDistance = 16;
    private Vector3 Tempvec3 = new Vector3();
    private float TargetAngle = 0;

    public float fAngle = 180;
    private float fSpeed = 90;

    //private Transform _root = null;

    public void Init()
    {

        GameObject _root = GameObject.Find("@Camera_Root");

        if (_root == null)
        {
            _root = new GameObject { name = "@Camera_Root" };
            _root.transform.SetParent(GameObject.Find("@Managers").transform);
        }

        Main_Camera = Camera.main;

        Debug.Log("CameraManager Init!");

    }
    public bool bNowRotateStateIsIdle()
    {
        return NowRotateState == Camera_RotateState.Idle;
    }

    public void CameraUpdate()
    {

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            Main_Camera.orthographic = !Main_Camera.orthographic;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (NowRotateState == Camera_RotateState.Idle)
            {
                NowRotateState = Camera_RotateState.Minus;
                if (fAngle == 360)
                    fAngle = 0;

                TargetAngle = fAngle + 90;
            }

        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            if (NowRotateState == Camera_RotateState.Idle)
            {
                NowRotateState = Camera_RotateState.Plus;
                if (fAngle == 0)
                    fAngle = 360;

                TargetAngle = fAngle - 90;
            }

        }

        if (NowRotateState == Camera_RotateState.Minus)
        {
            if (fAngle < TargetAngle)
            {
                fAngle += Time.deltaTime * fSpeed;
            }
            else
            {
                fAngle = TargetAngle;
                NowRotateState = Camera_RotateState.Idle;

                if (Now_State != (Define.Camera_State)0)
                    Now_State -= 1;
                else
                    Now_State = Define.Camera_State.R_270;

                Debug.Log(Now_State);
            }
        }

        if (NowRotateState == Camera_RotateState.Plus)
        {
            if (fAngle > TargetAngle)
            {
                fAngle -= Time.deltaTime * fSpeed;
            }
            else
            {
                fAngle = TargetAngle;
                NowRotateState = Camera_RotateState.Idle;

                if (Now_State != (Define.Camera_State)3)
                    Now_State += 1;
                else
                    Now_State = Define.Camera_State.R_0;

                Debug.Log(Now_State);
            }
        }

        ChasePlayer();

    }

    void ChasePlayer()
    {
       
        var x = Mathf.Sin(fAngle * Mathf.Deg2Rad) * CenterDistance + Managers.Pool_Instance.Dictionary_AllGameObject["Player"][0].transform.position.x;
        var y = Managers.Pool_Instance.Dictionary_AllGameObject["Player"][0].transform.position.y;
        var z = Mathf.Cos(fAngle * Mathf.Deg2Rad) * CenterDistance + Managers.Pool_Instance.Dictionary_AllGameObject["Player"][0].transform.position.z;

        Tempvec3.Set(x, y, z);

        Main_Camera.transform.position = Tempvec3;

        Tempvec3.Set(0, fAngle - 180, 0);
        Main_Camera.transform.localRotation = Quaternion.Euler(Tempvec3);
        Managers.Pool_Instance.Dictionary_AllGameObject["Player"][0].transform.localEulerAngles = Tempvec3; // 플레이어 빌보드
    }

    void GetCameraState()
    {

    }

    //LookAtSlowly()
    //{

    //    // this.LookDir = this.m_Master.GetWorldPosition() - this.m_Pet.transform.position;
    //    // var rot = Quaternion.LookRotation(this.LookDir.normalized);
    //    // this.m_Pet.transform.rotation = rot;

    //    var dir = this.m_Master.GetWorldPosition() - this.m_Pet.transform.position;
    //    var nextRot = Quaternion.LookRotation(dir.normalized);

    //    this.m_Pet.transform.rotation = Quaternion.Slerp(this.m_Pet.transform.rotation, nextRot, Time.deltaTime * 1);
    //}

    //LookDirCalculation()
    //{
    //    var nowpos = this.m_Pet.transform.position;
    //    nowpos.x = Mathf.Sin(this.m_Pet.transform.eulerAngles.y * Mathf.Deg2Rad) + this.m_Pet.transform.position.x;
    //    nowpos.z = Mathf.Cos(this.m_Pet.transform.eulerAngles.y * Mathf.Deg2Rad) + this.m_Pet.transform.position.z;
    //    this.LookDir = nowpos - this.m_Pet.transform.position;

    //}
}
