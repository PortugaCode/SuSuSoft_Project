using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMoveTest : MonoBehaviour
{
    public float offset = 1.5f;
    public float moveSpeed = 5.0f;
    private bool goingLeft = false;

    // Update is called once per frame
    void Update()
    {
        // -1.5 ~ 1.5
        if (goingLeft)
        {
            gameObject.transform.localPosition += new Vector3(-0.01f * moveSpeed, 0, 0);

            if (gameObject.transform.localPosition.x <= -offset)
            {
                goingLeft = false;
            }
        }
        else
        {
            gameObject.transform.localPosition += new Vector3(0.01f * moveSpeed, 0, 0);

            if (gameObject.transform.localPosition.x >= offset)
            {
                goingLeft = true;
            }
        }
    }
}
