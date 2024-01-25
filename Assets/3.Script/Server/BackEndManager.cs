using UnityEngine;
using BackEnd;    // 뒤끝 SDK

public class BackEndManager : MonoBehaviour
{
    private void Awake()
    {
        // Update에 Backend.AsyncPoll() 호출을 위해 해당 오브젝트는 Destroy가 되면 안된다.
        DontDestroyOnLoad(gameObject);

        

        //뒤끝 서버 초기화
        BackEndSetUp();
    }

    private void Update()
    {
        //서버의 비동기 메서드 호출 (콜백 함수 폴링)을 위해 작성
        if(Backend.IsInitialized)
        {
            Debug.Log("비동기 메서드 중");
            Backend.AsyncPoll();
        }
    }


    private void BackEndSetUp()
    {
        //뒤끝 초기화
        var bro = Backend.Initialize(true);

        //뒤끝 초기화에 따른 응답값
        if(bro.IsSuccess())
        {
            //초기화 성공 시 statusCode 204 Success
            Debug.Log($"초기화 성공 : {bro}");
        }
        else
        {
            //초기화 실패 시 statusCode 400대 에러 발생
            Debug.LogError($"초기화 실패 : {bro}");
        }
    }
}
