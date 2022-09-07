using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//저장하기
//1. 데이터(코드 = 클래스)를 만들어야함 => 저장할 데이터 생성
//2. 데이터를 Json으로 변환
//3. Json을 외부에 저장

//불러오기
//1. 외부에 저장된 Json을 가져옴
//2. Json을 데이터형태로 변환
//3. 불러온 데이터를 사용

public class PlayerData
{
    //변수에 전부 퍼블릭 필요
    //public
    public GameManager gameManager;
}

public class DataManager : MonoBehaviour
{
    //싱글톤
    public static DataManager instance;

    PlayerData nowplayer = new PlayerData() { };

    string path;
    string filename = "save";

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(instance.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        path = Application.persistentDataPath + "/";
    }

    void Start()
    {
        

        //PlayerData player2 = JsonUtility.FromJson<PlayerData>(jsonData);
    }

    public void SaveData()
    {
        string jsonData = JsonUtility.ToJson(nowplayer);

        File.WriteAllText(path + filename, jsonData);
    }

    public void LoadData()
    {
        string data = File.ReadAllText(path + filename);
        nowplayer = JsonUtility.FromJson<PlayerData>(data);
    }
}
