using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoUI : MonoBehaviour
{
    GameObject myObject;
    Coroutine updateUI;

    private void Start()
    {
        myObject = this.gameObject;
        updateUI = StartCoroutine(UpdateUI());
    }

    public void CloseInfoUI()
    {
        myObject.SetActive(false);
    }

    private void OnDisable()
    {
        StopCoroutine(updateUI);
    }


    private void OnDestroy()
    {
        StopCoroutine(updateUI);
    }


    private IEnumerator UpdateUI()
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}
