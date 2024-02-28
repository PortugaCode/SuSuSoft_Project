using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TEEEESSSTT : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene("Inventory");
    }

    private void Update()
    {
        Debug.Log("1번째");

        StartCoroutine(TestCoroutine());


        Debug.Log("5번째");
    }

    private IEnumerator TestCoroutine()
    {
        Debug.Log("2번째");

        int a = 0;

        while(a < 3)
        {
            Debug.Log($"3번째 {a}");
            a++;
            yield return new WaitForSeconds(1.0f);
        }


        Debug.Log("4번째");

    }
}
