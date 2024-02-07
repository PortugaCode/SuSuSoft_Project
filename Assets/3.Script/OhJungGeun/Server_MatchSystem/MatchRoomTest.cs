using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Tcp;
using TMPro;

public class MatchRoomTest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] textMeshProList;

    private void Start()
    {
        for(int i = 0; i < BackEndManager.Instance.GetMatchSystem().gameRecords.Count; i++)
        {
            textMeshProList[i].text = BackEndManager.Instance.GetMatchSystem().gameRecords[i].m_nickname;
        }
    }
}
