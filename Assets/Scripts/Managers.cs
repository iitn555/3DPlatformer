using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }

    CameraManager _camera = new CameraManager();
    public static CameraManager Camera_Instance { get { return Instance._camera; } }

    PoolManager _pool = new PoolManager();
    //InputManager _input = new InputManager();
    //ResourceManager _resource = new ResourceManager();

    public static PoolManager Pool_Instance { get { return Instance._pool; } }
    //public static InputManager Input_Instance { get { return Instance._input; } }
    //public static ResourceManager Resource_Instance { get { return Instance._resource; } }


    void Start()
    {


    }


    static void Init()
    {
        if (s_instance == null)
        {
            GameObject _obj = GameObject.Find("@Managers");
            if (_obj == null)
            {
                _obj = new GameObject { name = "@Managers" };
                _obj.AddComponent<Managers>();
            }

            s_instance = _obj.GetComponent<Managers>();
            _obj.transform.SetParent(GameObject.Find("MainGame").transform);
            Debug.Log("@Managers Init!");

            s_instance._camera.Init();
            s_instance._pool.Init();


        }
    }

}
