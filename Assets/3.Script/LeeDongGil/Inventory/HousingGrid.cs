using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HousingGrid : MonoBehaviour
{
    private void Start()
    {
        RectTransform rect = GetComponent<RectTransform>();
        //Grid grid = new Grid(10, 20, 100f);
        Debug.Log(rect.position);
    }
}
