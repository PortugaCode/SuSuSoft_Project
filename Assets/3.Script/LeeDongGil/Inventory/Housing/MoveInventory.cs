using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInventory : MonoBehaviour
{
    [Header("Parent")]
    public Transform inventoryScroll;
    public Transform inventoryWindow;

    [Header("Go to Where")]
    public GameObject windowBG;
    public GameObject transparency;
    public GameObject scroll;
    public GameObject editBtn;
    public GameObject testBtn;


    public void GotoWindow()
    {
        windowBG.SetActive(true);
        transparency.SetActive(true);
        scroll.SetActive(false);
        editBtn.SetActive(false);
        testBtn.SetActive(false);
    }

    public void GotoScroll()
    {
        scroll.SetActive(true);
        editBtn.SetActive(true);
        testBtn.SetActive(true);
        windowBG.SetActive(false);
        transparency.SetActive(false);
    }
}
