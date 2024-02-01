using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoScenario : MonoBehaviour
{
    [SerializeField] private Progress progress;
    [SerializeField] private SceneNames nextScene;

    private void Awake()
    {
        SystemSetUp();
    }

    private void SystemSetUp()
    {
        //활성화되지 않은 상태에서도 게임 계속 진행
        Application.runInBackground = true;

        //해상도 설정 9:18.5 (1440x2960)
        int width = Screen.width;
        int height = (int)(Screen.width * 18.5f / 9);
        Screen.SetResolution(width, height, true);

        // 화면이 꺼지지 않도록 설정
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // 로딩 애니메이션 실행, 끝났을 시 OnAfterProgress() 메서드 실행
        progress.Play(OnAfterProgress);
    }


    private void OnAfterProgress()
    {
        Utils.Instance.LoadScene(nextScene);
    }
}
