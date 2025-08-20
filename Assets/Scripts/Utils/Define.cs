using System.Collections.Generic;
using UnityEngine;

public class Define
{

    public enum Camera_State { R_0, R_90, R_180, R_270 }

    //public enum WorldObject
    //{
    //    Unknown,
    //    Player,
    //    Monster,
    //}

    //public enum State
    //{
    //    Die,
    //    Moving,
    //    Idle,
    //    Skill,
    //}

    //public enum Layer
    //{
    //    Monster = 8,
    //    Ground = 9,
    //    Block = 10,
    //}

    //public enum Scene
    //{
    //    Unknown,
    //    Login,
    //    Lobby,
    //    Game,
    //}

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    //public enum UIEvent
    //{
    //    Click,
    //    Drag,
    //}

    //public enum MouseEvent
    //{
    //    Press,
    //    PointerDown,
    //    PointerUp,
    //    Click,
    //}

    //public enum CameraMode
    //{
    //    QuarterView,
    //}
}

[System.Serializable] // 직렬화 안하면 JSON 파일이 정상적으로 만들어지지 않음.
public class PositionData
{
    public string name;
    public Vector3 position;
}


public class SaveData
{
    public List<PositionData> objects = new List<PositionData>();
}
