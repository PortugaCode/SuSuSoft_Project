using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject Level_1;
    [SerializeField] private GameObject Level_2;
    [SerializeField] private GameObject Level_3;
    [SerializeField] private GameObject Level_4;
    [SerializeField] private GameObject Level_5;

    [SerializeField] private GameObject Player_1;
    [SerializeField] private GameObject Player_2;
    [SerializeField] private GameObject Player_3;
    [SerializeField] private GameObject Player_4;
    [SerializeField] private GameObject Player_5;

    [SerializeField] private GameObject Top_UI;
    [SerializeField] private GameObject BossTop_UI;

    [SerializeField] private GameObject endWhite;
    public GameObject EndWhite => endWhite;


    private void Awake()
    {
        endWhite.SetActive(false);
        SelectLevel();
    }


    private void SelectLevel()
    {
        if(Utils.Instance.currentLevel == Level.Level_1)
        {
            GameObject level = GameObject.Instantiate(Level_1, Level_1.transform.position, Quaternion.identity);
            Player_1.gameObject.SetActive(true);
            Top_UI.gameObject.SetActive(true);
        }
        else if(Utils.Instance.currentLevel == Level.Level_2)
        {
            GameObject level = GameObject.Instantiate(Level_2, Level_2.transform.position, Quaternion.identity);
            Player_2.gameObject.SetActive(true);
            Top_UI.gameObject.SetActive(true); 
        }
        else if (Utils.Instance.currentLevel == Level.Level_3)
        {
            GameObject level = GameObject.Instantiate(Level_3, Level_3.transform.position, Quaternion.identity);
            Player_3.gameObject.SetActive(true);
            Top_UI.gameObject.SetActive(true);
        }
        else if (Utils.Instance.currentLevel == Level.Level_4)
        {
            GameObject level = GameObject.Instantiate(Level_4, Level_4.transform.position, Quaternion.identity);
            Player_4.gameObject.SetActive(true);
            Top_UI.gameObject.SetActive(true);
        }
        else if (Utils.Instance.currentLevel == Level.Level_5)
        {
            GameObject level = GameObject.Instantiate(Level_5, Level_5.transform.position, Quaternion.identity);
            Player_5.gameObject.SetActive(true);
            BossTop_UI.gameObject.SetActive(true);
        }

    }
}
