using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour
{

    enum Buttons {
        SaveButton,
        LoadButton
    }

    enum GameObjects {
        Cube
        
    }

    Dictionary<Type, UnityEngine.Object[]> Dic_Objects = new Dictionary<Type, UnityEngine.Object[]>();

    

    public Button SaveButton;
    public Button LoadButton;

    private void Awake()
    {
        Bind<Button>(typeof(Buttons));
        //Bind<GameObject>(typeof(GameObjects));


        SaveButton = Get<Button>((int)Buttons.SaveButton);
        SaveButton.onClick.AddListener(ClicKSaveButton);

        LoadButton = Get<Button>((int)Buttons.LoadButton);
        LoadButton.onClick.AddListener(ClicKLoadButton);
    }

    void ClicKSaveButton()
    {
        Debug.Log("SaveButton!");
    }

    void ClicKLoadButton()
    {
        Debug.Log("LoadButton!");
    }


    void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[names.Length];
        Dic_Objects.Add(typeof(T), objects);

        for (int i = 0; i < names.Length; ++i)
        {
            if (typeof(T) == typeof(GameObject))
                objects[i] = Util.FindChild(gameObject, names[i], true);
            else
                objects[i] = Util.FindChild<T>(gameObject, names[i], true);

            if(objects[i] == null)
            {
                Debug.Log($"Fail Bind {names[i]}");
            }
            else
                Debug.Log($"Success Bind {names[i]}");
        }

    }

    T Get <T>(int idx) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;
        if(Dic_Objects.TryGetValue(typeof(T), out objects))
        {
            return objects[idx] as T;
        }

        return null;
    }
}
