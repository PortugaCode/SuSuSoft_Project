using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEventControl : MonoBehaviour
{
    public EventHandler onItemEquip;
    public EventHandler onEndGame;

    [Header("Lever Control")]
    [SerializeField] private HorizontalPlayer horizontalPlayer;
    [SerializeField] private Transform moveRock;
    [SerializeField] private Transform targetTransForm;
    [SerializeField] private GameObject leverOn;
    [SerializeField] private GameObject leverOff;

    [Header("Stage Clear UI")]
    [SerializeField] private GameObject stageClearPopup;


    private bool isTriggered = false;

    private void Awake()
    {
        stageClearPopup = GameObject.FindGameObjectWithTag("EndPopUp").transform.GetChild(1).gameObject;
    }



    private void Start()
    {
        if (gameObject.CompareTag("Lever"))
        {
            horizontalPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<HorizontalPlayer>();
            horizontalPlayer.OnLaver += OnMoveRock;
        }

        if(gameObject.CompareTag("Finish"))
        {
            Debug.Log($"{gameObject.name}에서 Finish 씬 로드");
            onEndGame += StageClear;
        }
    }

    private void OnDestroy()
    {
        if (gameObject.CompareTag("Lever"))
        {
            horizontalPlayer.OnLaver -= OnMoveRock;
        }

        if (gameObject.CompareTag("Finish"))
        {
            onEndGame -= StageClear;
        }
    }

    public void OnMoveRock(object sender, EventArgs args)
    {
        StartCoroutine(MoveRock_Co());
        leverOn.SetActive(true);
        leverOff.SetActive(false);
        AudioManager.Instance.PlaySFX(SFX_Name.WorkshopSound02);
    }

    private IEnumerator MoveRock_Co()
    {
        while(Vector2.Distance(moveRock.position, targetTransForm.position) >= 0.1f)
        {

            moveRock.position = Vector2.Lerp(moveRock.position, targetTransForm.position, 0.6f * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && gameObject.CompareTag("Respawn") && !isTriggered)
        {
            isTriggered = true;
            onItemEquip?.Invoke(this, EventArgs.Empty);
        }

        else if(collision.CompareTag("Player") && gameObject.CompareTag("Finish") && !isTriggered)
        {
            isTriggered = true;
            Debug.Log("OnTriggerEnter2D");
            //Todo End => Stage 클리어 보상 팝업 띄우기
            AudioManager.Instance.PlaySFX(SFX_Name.StageSuccess);
            onEndGame?.Invoke(this, EventArgs.Empty);
        }
    }

    public void StageClear(object sender, EventArgs args)
    {
        stageClearPopup.GetComponent<StageClear>().ShowClearUI();

        stageClearPopup.SetActive(true);
    }
}
