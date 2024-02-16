using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneNames
{
    Loading,
    Login,
    Chatting,
    MatchRoom,
    MatchLoad
}

public class Utils : MonoBehaviour
{
    public static Utils Instance;

    public SceneNames nowScene;

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
        }
    }

    public void LoadScene(SceneNames sceneNames)
    {
        nowScene = sceneNames;
        SceneManager.LoadScene(sceneNames.ToString());
    }
}
