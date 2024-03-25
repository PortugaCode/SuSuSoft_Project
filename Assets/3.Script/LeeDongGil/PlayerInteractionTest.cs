using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionTest : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private TestTouchMove touchMove;
/*
    private void OnTriggerEnter(Collider collision)
    {
            Debug.Log("들어왔니?");
        if (collision.CompareTag("Building") && touchMove.isInteraction)
        {
            HousingDrag housingDrag = collision.GetComponent<HousingDrag>();
            Debug.Log(housingDrag.gameObject.name);
            if (housingDrag.data.housingType == HousingType.interactionable)
            {
                Debug.Log("housingType 들어옴");
                switch (housingDrag.data.housingID)
                {
                    case 5001:
                        Debug.Log("5001 들어옴");
                        touchMove.interactionObject = collision.gameObject;
                        touchMove.interactionControl.doAnimatorArray[0].Invoke();
                        break;
                    default:
                        Debug.Log("Unknown housingID type");
                        return;
                }
            }
        }
    }*/
}
