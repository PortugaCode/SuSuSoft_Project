using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Progress : MonoBehaviour
{
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI textProgressData;
    [SerializeField] private float progressTime; // 로딩 바 재생시간

    public void Play(UnityAction action = null)
    {
        StartCoroutine(OnProgress(action));
    }

    private IEnumerator OnProgress(UnityAction action)
    {
        float current = 0f;
        float percent = 0f;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / progressTime;

            // 로딩 Text 설정
            textProgressData.text = $"현재 로딩 중입니다... {progressSlider.value * 100:F0}%";
            progressSlider.value = Mathf.Lerp(0, 1, percent);
            
            
            yield return null;
        }

        action?.Invoke();
    }



}
