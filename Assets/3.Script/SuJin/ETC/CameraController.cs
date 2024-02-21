using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float smothing = 0.2f;
    [SerializeField] Vector2 minCameraPos;
    [SerializeField] Vector2 maxCameraPos;



    public void SetPlayer(GameObject a)
    {
        player = a.transform;
    }



    private void Update()
    {
        if (Utils.Instance.nowScene == SceneNames.Chatting)
        {
            CameraPos();
        }

    }


    private void CameraPos()
    {
        Vector3 targetPos = new Vector3(player.position.x, player.position.y, this.transform.position.z);
        targetPos.y = Mathf.Clamp(targetPos.y, minCameraPos.y, maxCameraPos.y);
        //targetPos.x = Mathf.Clamp(targetPos.x, minCameraPos.x, maxCameraPos.x);

        if (Utils.Instance.nowScene == SceneNames.Chatting)
        {
            transform.position = targetPos;
            return;
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, smothing * Time.deltaTime);
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
