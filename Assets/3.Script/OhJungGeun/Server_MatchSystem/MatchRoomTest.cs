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
        Backend.Match.OnSessionOffline += (MatchInGameSessionEventArgs args) =>
        {
            Debug.Log("누군가 나감");
            SetText();
        };
        SetText();
    }


    private void SetText()
    {
        Debug.Log("SetText");
        for(int i = 0; i < textMeshProList.Length; i++)
        {
            textMeshProList[i].text = "AI";
        }

        for (int i = 0; i < BackEndManager.Instance.GetMatchSystem().gameRecords.Count; i++)
        {
            textMeshProList[i].text = BackEndManager.Instance.GetMatchSystem().gameRecords[i];
        }
    }

    public void LeaveGameServer()
    {
        BackEndManager.Instance.GetMatchSystem().LeaveGameServer();
    }
}
