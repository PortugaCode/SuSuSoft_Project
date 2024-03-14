using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControll : MonoBehaviour
{
    [SerializeField] HorizontalPlayer horizontalPlayer;
    [SerializeField] Rigidbody2D rb;

    [SerializeField] Vector2 screenBound;


    private void Start()
    {
        horizontalPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<HorizontalPlayer>();
        rb = GetComponent<Rigidbody2D>();

    }

    private void FixedUpdate()
    {
        FollowPlayer();
    }

    private void LateUpdate()
    {
        ClampScreen();
    }

    public void ClampScreen()
    {
        screenBound = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        Vector3 objectPos = transform.position;
        objectPos.y = Mathf.Clamp(objectPos.y, screenBound.y * -1, screenBound.y - 0.65f);
        transform.position = objectPos;
    }

    private void FollowPlayer()
    {
        rb.velocity = horizontalPlayer.GetRigidbody2D().velocity;
        transform.position = new Vector2(horizontalPlayer.gameObject.transform.position.x, transform.position.y);
    }
}

