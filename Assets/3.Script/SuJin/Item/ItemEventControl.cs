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

    private void Start()
    {
        if (gameObject.CompareTag("Lever"))
        {
            horizontalPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<HorizontalPlayer>();
            horizontalPlayer.OnLaver += OnMoveRock;
        }
    }

    public void OnMoveRock(object sender, EventArgs args)
    {
        StartCoroutine(MoveRock_Co());
        leverOn.SetActive(true);
        leverOff.SetActive(false);
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
        if(collision.CompareTag("Player") && gameObject.CompareTag("Respawn"))
        {
            onItemEquip?.Invoke(this, EventArgs.Empty);
        }
        else if(collision.CompareTag("Player") && gameObject.CompareTag("Finish"))
        {
            //Todo End => Stage 클리어 보상 팝업 띄우기
            onEndGame?.Invoke(this, EventArgs.Empty);
        }
    }
}
