using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MainGame : MonoBehaviour
{
    private void Awake()
    {
        LoadPositions();
    }

    void Start()
    {
        
    }

    void Update()
    {
        Managers.Camera_Instance.CameraUpdate();
    }

    private void LateUpdate()
    {
        
    }

    string folderPath = "Assets/Save";
    string fileName = "positions.json";
    string savePath;
    GameObject SampleCubeObject;
    Transform CubeBoxTransform;

    public void LoadPositions() // ·Îµå
    {
        CubeBoxTransform = transform.Find("Blocks").transform;
        SampleCubeObject = CubeBoxTransform.GetChild(0).gameObject;

        savePath = Path.Combine(folderPath, fileName);

        if (!File.Exists(savePath))
        {
            Debug.LogWarning("Save file not found!");
            return;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        int count = 0;
        foreach (PositionData posData in data.objects)
        {
            MakeCube(ref count, posData.position);
        }

        Debug.Log("Load complete.");
    }

    void MakeCube(ref int _count, Vector3 _pos = default)
    {
        GameObject obj = Instantiate(SampleCubeObject, CubeBoxTransform);
        obj.SetActive(true);
        obj.transform.position = _pos;
        obj.name = $"Cube_{_count}";
        _count++;
    }

}
