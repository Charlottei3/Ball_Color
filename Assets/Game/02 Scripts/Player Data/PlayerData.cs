using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class PlayerData : MonoBehaviour
{
#if UNITY_EDITOR
    public const string path = "Assets/Game/06. Data/JsonText/PlayerData";
#else
    private  string path = Application.persistentDataPath;
#endif
    public static UserData UserData = new UserData();

    private void Awake()
    {

    }
    private void Start()
    {
        InitData();
    }

    private void InitData()
    {
        //********USERDATA**********
        if (!PlayerPrefs.HasKey(Const.KEY_USER_DATA))
        {
            SaveUserData();
            if (!File.Exists(path + "/UserData.txt"))
            {
                // Create a file to write to.
                File.Create(path + "/UserData.txt");
            }
        }
        else
        {
            LoadUserData();
        }
    }

    #region UserData
    public static void LoadUserData()
    {
        var saveData = PlayerPrefs.GetString(Const.KEY_USER_DATA);
        var data = JsonUtility.FromJson<UserData>(saveData);
        UserData = data;
    }

    public static void SaveUserData()
    {
        string saveData = JsonUtility.ToJson(UserData);
        PlayerPrefs.SetString(Const.KEY_USER_DATA, saveData);
        File.WriteAllText(path + "/UserData.txt", saveData);
    }
    #endregion
}
