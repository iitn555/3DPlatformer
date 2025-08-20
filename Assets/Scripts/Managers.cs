using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance;
    static Managers Instance { get { Init(); return s_instance; } }

    ResourceManager _resource = new ResourceManager();
    CameraManager _camera = new CameraManager();
    PoolManager _pool = new PoolManager();
    SoundManager _sound = new SoundManager();

    //public static ResourceManager Resource_Instance { get { return Instance._resource; } }
    public static CameraManager Camera_Instance { get { return Instance._camera; } }
    public static SoundManager Sound_Instance { get { return Instance._sound; } }
    public static PoolManager Pool_Instance { get { return Instance._pool; } }
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
            s_instance._sound.Init();

        }
    }

}
