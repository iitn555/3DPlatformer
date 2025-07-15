using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MYRECT
{
    public float left;
    public float top;
    public float right;
    public float bottom;
    public float front;
    public float back;

    public float HeadBot;
    public float FootTop;
};



public class Unit : MonoBehaviour
{
    public float Width { get; protected set; }
    public float Height { get; protected set; }

    //protected float Width = 0;
    //protected float Height = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
}
