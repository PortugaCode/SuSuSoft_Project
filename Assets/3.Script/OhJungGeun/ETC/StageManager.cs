using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private GameObject stageSelectUI;


    private void Start()
    {
        if(Utils.Instance.isGoToHome)
        {
            stageSelectUI.SetActive(true);
            Utils.Instance.isGoToHome = false;
        }
        else
        {
            stageSelectUI.SetActive(false);
        }
    }
}
