using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NowStageSelect : MonoBehaviour
{
    public void NowStageIndex(int index)
    {
        Utils.Instance.SelectStage(index);
    }
}
