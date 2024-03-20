using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] private GameObject endUI;

    public void ActiveOnEndUI()
    {
        StartCoroutine(ActiveOnEndUI_Co());
    }

    private IEnumerator ActiveOnEndUI_Co()
    {
        yield return new WaitForSeconds(1.5f);
        endUI.SetActive(true);
        AudioManager.Instance.PlaySFX(SFX_Name.StageFail);
        AudioManager.Instance.BGM_AudioSource.Stop();
    }
}
