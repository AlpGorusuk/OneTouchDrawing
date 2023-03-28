using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : SingletonPersistent<GameControl>
{
    private int Save_Data_Version = 1;
    private Scene Current_Level_Scene;
    public string Current_Scene_Name = ""; // also flag to determine game played once
    public int Current_Level_Index = 0;
    public List<string> Levels;
    public void Load_Game()
    {
        Load_Game_Data();
        Load_Level();
    }
    // Save/Load Game Data
    private void Load_Game_Data()
    {
        if (!Is_Save_Data_Valid()) return;
        if (Current_Level_Index == 0)
        {
            Current_Level_Index = PlayerPrefs.GetInt("Current_Level_Index", 0);
        }
        Debug.Log("Game Data Loaded!");
    }

    public void Save_Game_Data()
    {
        PlayerPrefs.SetInt("Current_Level_Index", Current_Level_Index);
        PlayerPrefs.SetInt("Save_Data_Version", Save_Data_Version);
        Debug.Log("Game Data Saved!");
    }
    public void Finish_Current_Level()
    {
        Current_Level_Index++;
        Save_Game_Data();
    }
    private bool Is_Save_Data_Valid()
    {
        int save_data_version = PlayerPrefs.GetInt("Save_Data_Version", 0);
        if (save_data_version == Save_Data_Version)
        {
            return true;
        }
        else
        {
            Debug.Log("Game data invalid!");
            return false;
        }
    }
    private void Load_Level()
    {
        int index = Get_Real_Level_Index(Current_Level_Index);
        Current_Scene_Name = Levels[index];
        LoadingSceneManager.Instance.StartLoadScene(Current_Scene_Name);
    }
    // Looping levels
    private int Get_Real_Level_Index(int level)
    {
        return level % Levels.Count;
    }
}
