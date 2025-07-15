using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainGame : MonoBehaviour
{
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Managers.Camera_Instance.CameraUpdate();
    }

    private void LateUpdate()
    {
       



        
    }
}
