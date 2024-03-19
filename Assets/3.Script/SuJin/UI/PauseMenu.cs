using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void Home()
    {
        StopAllCoroutines();
        Utils.Instance.LoadScene(SceneNames.Chatting);
        Time.timeScale = 1;
    }

    public void Continue()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1; ;
    }

    public void Restart()
    {
        StopAllCoroutines();
        Utils.Instance.LoadScene(SceneNames.OnGame);
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        AudioManager.Instance.PlaySFX(SFX_Name.StageFail);
        Time.timeScale = 0;
    }
}
