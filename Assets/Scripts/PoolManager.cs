using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PoolManager
{
    Vector3 RespawnPosition = new Vector3(5f, -2.9f);
    //public GameObject ZombieMeleeSample;
    //public GameObject BulletSample;
    private Transform ZombieMelee_Root;
    private Transform Bullet_Root;
    private Transform Box_Root;

    //private Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>(); // ALLSAMPLE
    public Dictionary<string, List<Unit>> Dictionary_AllGameObject = new Dictionary<string, List<Unit>>();

    Transform _root;

    private GameObject[] AllBox = new GameObject[5];
    private GameObject LastPlayer;


    public void Init()
    {
        if (_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            _root.transform.SetParent(GameObject.Find("@Managers").transform);

        }

        Setting();
    }

    void Setting() // �ʼ�������Ʈ ����
    {
        CreatePlayer();

    }
    public void CreatePlayer()
    {
        
        var _playercomponent = GameObject.Find("Player").AddComponent<PlayerController>();
        //����� Player ������Ʈ�� �ٴ´� ���߿� ���������� �ٲ� ����

        RegisterUnit<PlayerController>(_playercomponent, typeof(PlayerController).Name);


    }

    //public T MakeOrGetObject<T>() where T : Unit
    //{

    //    if (Dictionary_AllGameObject.ContainsKey(typeof(T).Name))
    //    {
    //        foreach (var _Object in Dictionary_AllGameObject[typeof(T).Name])
    //        {
    //            if (_Object.bDie)
    //            {
    //                _Object.CommonRespawn();
    //                _Object.Respawn();

    //                return _Object as T;
    //            }

    //        }
    //        return CreateObject<T>() as T;
    //    }
    //    else
    //    {
    //        return CreateObject<T>() as T;
    //    }

    //}

    //public T GetObject<T>() where T : Unit // ���ⰳ��?
    //{
    //    if (typeof(T) == typeof(T))
    //    {
    //        if (Dictionary_AllGameObject.ContainsKey(typeof(T).Name))
    //        {
    //            foreach (var obj in Dictionary_AllGameObject[typeof(T).Name])
    //            {
    //                if (!obj.bDie) // �׾������ʴٸ� 
    //                {
    //                    return obj as T; //��ȯ
    //                }

    //            }
    //        }
    //    }

    //    return null;
    //}




    //public T CreateObject<T>() where T : Unit
    //{
    //    GameObject _unitobject = Managers.Resource_Instance.Instantiate(typeof(T).Name, _root);
    //    T _unitcomponent = _unitobject.AddComponent<T>();
    //    RegisterUnit<T>(_unitcomponent, typeof(T).Name);
    //    _unitcomponent.Respawn();

    //    return _unitcomponent;
    //}

    T RegisterUnit<T>(T New_CObj, string _name) where T : Unit

    {

        if (!Dictionary_AllGameObject.ContainsKey(_name))
        {
            List<Unit> New_ = new List<Unit>();
            Dictionary_AllGameObject.Add(_name, New_);
            Dictionary_AllGameObject[_name].Add(New_CObj);
        }
        else
        {
            Dictionary_AllGameObject[_name].Add(New_CObj);
        }

        return New_CObj;
    }


 

}
