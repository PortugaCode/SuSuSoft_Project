using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneNames
{
    Loading,
    Jinwon_Login,
    Chatting,
    MatchRoom,
    MatchLoad,
    OnGame
}

public enum Level
{
    Level_1,
    Level_2,
    Level_3,
    Level_4,
    Level_5,
}

public class Utils : MonoBehaviour
{
    public static Utils Instance;

    public SceneNames nowScene;
    public Level currentLevel;
    public bool isFourStage = false;
    public bool isBossStage = false;

    public int stageIndex;

    private void Awake()
    {
        #region [싱글톤] 
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        #endregion
    }


    public string GetActiveScene()
    {
        //현재 씬 name 반환
        return SceneManager.GetActiveScene().name;
    }

    public void LoadScene(string name = "")
    {
       if(name.Equals(""))
        {
            SceneManager.LoadScene(GetActiveScene());
        }
       else
        {
            SceneManager.LoadScene(name);
            nowScene = (SceneNames)Enum.Parse(typeof(SceneNames), name);
        }
    }

    public void LoadScene(SceneNames sceneNames)
    {
         
        nowScene = sceneNames;
        SceneManager.LoadScene(sceneNames.ToString());
    }

    public void SelectLevel()
    {
        switch((int)currentLevel)
        {
            case 0:
                {
                    currentLevel = Level.Level_1;
                    isFourStage = false;
                    isBossStage = false;
                    break;
                }
            case 1:
                {
                    currentLevel = Level.Level_2;
                    isFourStage = false;
                    isBossStage = false;
                    break;
                }
            case 2:
                {
                    currentLevel = Level.Level_3;
                    isFourStage = false;
                    isBossStage = false;
                    break;
                }
            case 3:
                {
                    isFourStage = true;
                    isBossStage = false;
                    currentLevel = Level.Level_4;
                    break;
                }
            case 4:
                {
                    isBossStage = true;
                    isBossStage = false;
                    currentLevel = Level.Level_5;
                    break;
                }
            default :
                {
                    isFourStage = false;
                    isBossStage = false;
                    break;
                }
        }
    }
    
    public void SelectStage(int index)
    {
        
        stageIndex = index;

        Debug.Log($"stageIndex : {stageIndex}");
        switch (index)
        {
            case 0:
                currentLevel = Level.Level_1;
                isFourStage = false;
                isBossStage = false;
                break;
            case 1:
                currentLevel = Level.Level_2;
                isFourStage = false;
                isBossStage = false;
                break;
            case 2:
                currentLevel = Level.Level_3;
                isFourStage = false;
                isBossStage = false;
                break;
            case 3:
                currentLevel = Level.Level_4;
                isFourStage = true;
                isBossStage = false;
                break;
            case 4:
                currentLevel = Level.Level_5;
                isFourStage = false;
                isBossStage = true;
                break;
        }
    }
    
}
