using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDragMove : MonoBehaviour
{
    public float camSpeed = 0.005f;
    public HousingGrid grid;


    private enum Gesture
    {
        Move = 1,
        Zoom
    }

    private void Start()
    {
        grid = FindObjectOfType<HousingGrid>();
    }

    private void Update()
    {
        if (TestManager.instance.isEditMode)
        {
            MoveCamera();
        }
    }

    private void MoveCamera()
    {
        Camera mainCam = new Camera();
        if (Input.touchCount == (int)Gesture.Move)
        {
            mainCam = Camera.main;
            Touch touch = Input.GetTouch(0);
            Ray cameraRay = mainCam.ScreenPointToRay(touch.position);
            RaycastHit hit;
            float cameraX;
            float cameraY;
            if (Physics.Raycast(cameraRay, out hit))
            {
                if (hit.collider.CompareTag("CanBuild") || !TestManager.instance.isEditMode)
                {
                    Vector3 cam_RT = mainCam.ViewportToWorldPoint(new Vector3(1, 1, mainCam.nearClipPlane));
                    Vector3 cam_LT = mainCam.ViewportToWorldPoint(new Vector3(0, 1, mainCam.nearClipPlane));
                    Vector3 cam_LB = mainCam.ViewportToWorldPoint(new Vector3(0, 0, mainCam.nearClipPlane));
                    Vector3 cam_RB = mainCam.ViewportToWorldPoint(new Vector3(1, 0, mainCam.nearClipPlane));

                    float camX = cam_RT.x - cam_LT.x;
                    float camY = cam_RT.y - cam_RB.y;
                    //Debug.Log("RT : " + cam_RT);
                    //Debug.Log("LT : " + cam_LT);
                    //Debug.Log("LB : " + cam_LB);
                    //Debug.Log("RB : " + cam_RB);

                    cameraX = Mathf.Clamp(mainCam.transform.position.x, -(grid.boundX / 2) + (camX / 2) + grid.posX, (grid.boundX / 2) - (camX / 2) + grid.posX);
                    cameraY = Mathf.Clamp(mainCam.transform.position.y, -(grid.boundY / 2) + (camY / 2) + grid.posY, (grid.boundY / 2) - (camY / 2) + grid.posY);

                    mainCam.transform.position = new Vector3(
                    cameraX - touch.deltaPosition.x * camSpeed,
                    cameraY - touch.deltaPosition.y * camSpeed,
                    mainCam.transform.position.z);
                }
            }

        }
    }

}
