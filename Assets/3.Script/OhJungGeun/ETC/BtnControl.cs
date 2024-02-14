using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnControl : MonoBehaviour
{
    [SerializeField] private GameObject chatUI;


    public void SetChatUI(bool value)
    {
        chatUI.SetActive(value);
    }

    public void SetInterfaceUI(GameObject a)
    {
        a.SetActive(true);
        gameObject.SetActive(false);
    }
}
