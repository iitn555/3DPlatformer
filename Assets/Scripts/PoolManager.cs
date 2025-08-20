using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PoolManager
{
    public Dictionary<string, List<Unit>> Dictionary_AllGameObject = new Dictionary<string, List<Unit>>();


    Transform MainGame_tf;


    public void Init()
    {
        GameObject _root = GameObject.Find("@Pool_Root");

        if (_root == null) //최초 한번만 실행
        {
            _root = new GameObject { name = "@Pool_Root" };
            _root.transform.SetParent(GameObject.Find("@Managers").transform);

            Setting();
            Debug.Log("PoolManager Init!");

        }

    }

    void Setting() // 필수오브젝트 생성
    {
        CreatePlayer();

        MainGame_tf = GameObject.Find("MainGame").transform;
        Transform blocks_tf = MainGame_tf.Find("Blocks");

        for (int i = 0; i < blocks_tf.childCount; ++i)
        {
            var _component = blocks_tf.GetChild(i).gameObject.AddComponent<Unit>();
            RegisterUnit<Unit>(_component, "Block");

        }

    }
    public void CreatePlayer()
    {
        
        var _playercomponent = GameObject.Find("Player").AddComponent<PlayerController>();
        //현재는 Player 오브젝트에 붙는다 나중에 프리팹으로 바꿀 생각

        RegisterUnit<PlayerController>(_playercomponent, "Player");
        //RegisterUnit<PlayerController>(_playercomponent, typeof(PlayerController).Name);


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

    //public T GetObject<T>() where T : Unit // 여기개선?
    //{
    //    if (typeof(T) == typeof(T))
    //    {
    //        if (Dictionary_AllGameObject.ContainsKey(typeof(T).Name))
    //        {
    //            foreach (var obj in Dictionary_AllGameObject[typeof(T).Name])
    //            {
    //                if (!obj.bDie) // 죽어있지않다면 
    //                {
    //                    return obj as T; //반환
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
