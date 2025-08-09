using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class MaptoolManager : MonoBehaviour
{
    private CooldownManager CDManager = new CooldownManager();

    private GameObject SampleCubeObject;

    //public Transform CubeBox;

    [HideInInspector]
    public Transform CubeBoxTransform
    {
        get
        {
            GameObject root = GameObject.Find("CubeBox");
            if (root == null)
            {
                root = new GameObject { name = "CubeBox" };
                root.transform.SetParent(this.transform);
            }
            return root.transform;
        }

    }



    //private Vector3[] Array_6Side = new Vector3[6];
    private Vector3[] Array_6Side = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
    private string[] Array_6String = { "위쪽 면", "아래쪽 면", "왼쪽 면", "오른쪽 면", "앞쪽 면", "뒤쪽 면" };

    private void Awake()
    {
        savePath = Path.Combine(folderPath, fileName);

        SampleCubeObject = transform.Find("Cube").gameObject;

        MakeCube();
        CDManager.RegisterSkill("CreateCube", 0.1f);


    }

    private void Start()
    {
        //Test(Vector3.one);
        //Test(Vector3.up);
        //Test(Vector3.right);



        UI_Base_Manager = this.GetComponent<UI_Base>();
        UI_Base_Manager.SaveButton.onClick.AddListener(SavePositions);
        UI_Base_Manager.LoadButton.onClick.AddListener(LoadPositions);

        LoadPositions();
    }

    void Update()
    {
        //Keyboard.current.Key.isPressed
            

        if (Mouse.current.leftButton.isPressed)
        {
            //if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) // UI를 클릭했을때 방지하는
            //    return;

            if (CDManager.CheckCooldownSkill("CreateCube"))
                ClickAndCreateCube();

        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            DeleteCube();

        }
    }


    void ClickAndCreateCube()
    {
        Vector3 pos = Vector3.zero;

        if (CubeBoxTransform.childCount == 0)
        {
            MakeCube();
            return;
        }


        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f))
        {

            pos = hit.transform.position;
            Vector3 normal = hit.normal;

            for (int i = 0; i < 6; ++i)
            {
                if (normal == Array_6Side[i])
                {
                    Debug.Log(Array_6String[i]);
                    pos += Array_6Side[i];
                    MakeCube(pos);

                }
            }

        }

    }

    void DeleteCube()
    {

        Vector3 pos = Vector3.zero;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            pos = hit.transform.position;
            Vector3 normal = hit.normal;
            Destroy(hit.transform.gameObject);

        }

    }





    string folderPath = "Assets/Save";
    string fileName = "positions.json";
    string savePath;
    private UI_Base UI_Base_Manager;




    public void SavePositions()
    {

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        if (File.Exists(savePath))
        {
            //string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            //string backupPath = Path.Combine(folderPath, $"positions_{timestamp}.json");
            string backupPath = Path.Combine(folderPath, $"positions_2.json");

            if (File.Exists(backupPath))
                File.Delete(backupPath);

            File.Copy(savePath, backupPath);

        }

        GameObject[] targets = new GameObject[CubeBoxTransform.childCount];

        for (int i = 0; i < targets.Length; ++i)
        {
            targets[i] = CubeBoxTransform.GetChild(i).gameObject;

        }

        SaveData data = new SaveData();

        foreach (GameObject obj in targets)
        {
            PositionData posData = new PositionData
            {
                name = obj.name,
                position = obj.transform.position
            };
            data.objects.Add(posData);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Saved to: " + savePath);




    }

    public void LoadPositions()
    {
        for (int i = CubeBoxTransform.childCount - 1; i >= 0; --i) // 기존 큐브 전부 지우기
            Destroy(CubeBoxTransform.GetChild(i).gameObject);

        if (!File.Exists(savePath))
        {
            Debug.LogWarning("Save file not found!");
            return;
        }

        string json = File.ReadAllText(savePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        foreach (PositionData posData in data.objects)
        {
            MakeCube(posData.position);
        }

        Debug.Log("Load complete.");
    }

    void MakeCube(Vector3 _pos = default)
    {
        GameObject obj = Instantiate(SampleCubeObject, CubeBoxTransform);
        obj.SetActive(true);
        obj.transform.position = _pos;
    }
}
