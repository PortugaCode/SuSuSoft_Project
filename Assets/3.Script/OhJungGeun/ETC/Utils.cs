using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneNames
{
    Loading,
    Login
}

public class Utils : MonoBehaviour
{
    public static Utils Instance;

    private void Awake()
    {
        #region [�̱���] 
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
        //���� �� name ��ȯ
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
        SceneManager.LoadScene(sceneNames.ToString());
    }
}
