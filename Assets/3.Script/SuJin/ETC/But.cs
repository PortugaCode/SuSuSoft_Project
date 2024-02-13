using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class But : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("눌림");
        }
    }

}
