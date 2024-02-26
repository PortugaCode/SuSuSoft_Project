using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;




public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float smothing = 0.2f;
    [SerializeField] Vector2 minCameraPos;
    [SerializeField] Vector2 maxCameraPos;

    [SerializeField] private Animator animator;


    private void Start()
    {
        //스테이지 씬이라면 조건 추가 해야함
        if (Utils.Instance.nowScene == SceneNames.OnGame)
        {
            player.gameObject.GetComponent<PlayerProperty>().onDamage = ShakeCamera;
            AudioManager.Instance.PlayBGM(BGM_Name.Stage);
        }
        else
        {
            AudioManager.Instance.PlayBGM(BGM_Name.Main);
        }
    }

    private void ShakeCamera(object sender, EventArgs e)
    {
        animator.SetTrigger("Shake");
    }

    public void SetPlayer(GameObject a)
    {
        player = a.transform;
    }



    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "CharacterTest" || SceneManager.GetActiveScene().name == "OnGame") return;
        CameraPos();
    }

    private void FixedUpdate()
    {

        CameraPos_Stage();
    }


    private void CameraPos_Stage()
    {
        if (player == null) return;
        Vector3 targetPos = new Vector3(player.position.x, player.position.y, this.transform.position.z);
        targetPos.y = Mathf.Clamp(targetPos.y, minCameraPos.y, maxCameraPos.y);
        //targetPos.x = Mathf.Clamp(targetPos.x, minCameraPos.x, maxCameraPos.x);

        transform.position = Vector3.Lerp(transform.position, targetPos, smothing * Time.deltaTime);
    }

    private void CameraPos()
    {
        Vector3 targetPos = new Vector3(player.position.x, player.position.y, this.transform.position.z);
        targetPos.y = Mathf.Clamp(targetPos.y, minCameraPos.y, maxCameraPos.y);
        //targetPos.x = Mathf.Clamp(targetPos.x, minCameraPos.x, maxCameraPos.x);
        transform.position = targetPos;

        //transform.position = Vector3.Lerp(transform.position, targetPos, smothing * Time.deltaTime);
    }

    private void RepeatCam(Vector3 targetpos)
    {
        if (Vector2.Distance(transform.position, targetpos) > 8f)
        {
            //transform.position = new Vector3(transform.position.x * -1f, transform.position.y, transform.position.z);
            float x;
            if (targetpos.x > 0)
            {
                x = (transform.position.x * -1f) + (Vector2.Distance(new Vector3(transform.position.x * -1f, transform.position.y, transform.position.z), targetpos) * 2f);
            }
            else
            {
                x = (transform.position.x * -1f) - (Vector2.Distance(new Vector3(transform.position.x * -1f, transform.position.y, transform.position.z), targetpos) * 2f);
            }
            

            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }
    }

}
