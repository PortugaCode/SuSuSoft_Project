using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatLineControl : MonoBehaviour
{
    [SerializeField] private Transform pivot;


    //한 줄씩 뜨게하는 메서드
    public void SetChatLine(GameObject a)
    {
        GameObject clone = GameObject.Instantiate(a, a.transform.position, Quaternion.identity);
        clone.transform.SetParent(pivot);
        RectTransform cloneRect = clone.GetComponent<RectTransform>();

        cloneRect.anchoredPosition = new Vector2(500, 0);
        cloneRect.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);

        if (pivot.childCount >= 1)
        {
            Destroy(pivot.GetChild(0).gameObject);
        }

    }
}
