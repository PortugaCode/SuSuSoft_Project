using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [SerializeField]private float moveSpeed;
    private float deltaX, deltaY;
    private Rigidbody2D rigidbody2D;

    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        if(Input.touchCount>0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);

            switch(touch.phase)
            {
                case TouchPhase.Began:
                    deltaX = touchPos.x - transform.position.x;
                    break;

                case TouchPhase.Moved:
                    rigidbody2D.MovePosition(new Vector2(touchPos.x - deltaX, -3f));
                    break;

                case TouchPhase.Ended:
                    rigidbody2D.velocity = Vector2.zero;
                    break;
            }
        }
    }

    //Player Position limit

    /* [단순이동]
    
     private void TouchMove()
    {
        if(Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (mousePos.x > 1)
            {
                transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
            }
            else if (mousePos.x < -1)
            {
                transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
            }
        }
    }
     */

}
