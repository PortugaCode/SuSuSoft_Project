using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NowBuilding : MonoBehaviour
{
    private void Start()
    {
        LoadHousing.instance.isLoading = true;
        LoadHousing.instance.LoadHousingData();
        StartCoroutine(LoadBuilding());

    }

    private IEnumerator LoadBuilding()
    {
        yield return new WaitForSeconds(0.1f);
        LoadHousing.instance.isLoading = false;
    }
}
