using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float smothing = 0.2f;
    [SerializeField] Vector2 minCameraPos;
    [SerializeField] Vector2 maxCameraPos;

    private void FixedUpdate()
    {
        CameraPos();
    }

    private void CameraPos()
    {
        Vector3 targetPos = new Vector3(player.position.x, player.position.y, this.transform.position.z);

        targetPos.x = Mathf.Clamp(targetPos.x, minCameraPos.x, maxCameraPos.x);
        targetPos.y = Mathf.Clamp(targetPos.y, minCameraPos.y, maxCameraPos.y);

        transform.position = Vector3.Lerp(transform.position, targetPos, smothing);
    }
}
