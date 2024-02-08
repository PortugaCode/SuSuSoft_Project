using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HousingGrid : MonoBehaviour
{
    [Header("Bound")]
    public Vector2 boundRT;
    public Vector2 boundLT;
    public Vector2 boundLB;
    public Vector2 boundRB;

    public float boundX;
    public float boundY;

    public float posX;
    public float posY;

    private void Start()
    {
        boundX = transform.localScale.x;
        boundY = transform.localScale.y;

        posX = transform.position.x;
        posY = transform.position.y;


        boundRT = new Vector2((boundX / 2) + posX, (boundY / 2) + posY);
        boundLT = new Vector2(-(boundX / 2) + posX, (boundY / 2) + posY);
        boundLB = new Vector2(-(boundX / 2) + posX, -(boundY / 2) + posY);
        boundRB = new Vector2((boundX / 2) + posX, -(boundY / 2) + posY);
    }
}
