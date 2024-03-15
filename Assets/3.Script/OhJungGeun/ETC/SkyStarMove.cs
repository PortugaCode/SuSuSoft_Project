using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyStarMove : MonoBehaviour
{
    [SerializeField] private float speed;

    private void FixedUpdate()
    {
        StarMove();
    }

    private void StarMove()
    {
        #region [Repeat BG]
        if (transform.position.x <= -14.97f)
        {
            transform.position = new Vector2(transform.position.x * -1f, transform.position.y);
            return;
        }
        #endregion

        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
