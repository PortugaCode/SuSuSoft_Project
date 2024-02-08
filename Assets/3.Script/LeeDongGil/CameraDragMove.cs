using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDragMove : MonoBehaviour
{
    private enum Gesture
    {
        Move = 1,
        Zoom
    }
    private void Update()
    {
        
    }

    private void MoveCamera()
    {
        if(Input.touchCount == (int)Gesture.Move)
        {
            //Touch touch = Input.tou
        }
    }
}
