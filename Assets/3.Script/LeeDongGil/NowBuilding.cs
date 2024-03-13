using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NowBuilding : MonoBehaviour
{
    private void Awake()
    {
        LoadHousing.instance.isLoading = true;
    }
    private void Start()
    {
        LoadHousing.instance.LoadHousingData();
        StartCoroutine(LoadBuilding());
    }

    private IEnumerator LoadBuilding()
    {
        yield return new WaitForSeconds(0.1f);
        LoadHousing.instance.isLoading = false;
    }
}
