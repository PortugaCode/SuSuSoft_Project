using UnityEngine;
using BackEnd;    // �ڳ� SDK

public class BackEndManager : MonoBehaviour
{
    public static BackEndManager Instance;

    [SerializeField] private MatchSystem matchSystem;
    [SerializeField] private ChatManager chatManager;



    public MatchSystem GetMatchSystem()
    {
        return matchSystem;
    }

    public ChatManager GetChatManager()
    {
        return chatManager;
    }

    private void Awake()
    {
        #region [싱글톤]
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            // Update�� Backend.AsyncPoll() ȣ���� ���� �ش� ������Ʈ�� Destroy�� �Ǹ� �ȵȴ�.
            DontDestroyOnLoad(gameObject);
        }
        #endregion

        
        matchSystem = new MatchSystem();
        chatManager = new ChatManager();

        BackEndSetUp();
        matchSystem.OnMatchMakingRoomJoin();
    }

    private void Update()
    {
        if(Backend.IsInitialized)
        {
            Debug.Log("AsyncPoll On");
            Backend.AsyncPoll();

            Backend.Match.Poll();
            //matchSystem.SetTimer();



            Backend.Chat.Poll();
            chatManager.ReceiveChat();
            chatManager.ReceiveWhisperChat();
        }
    }


    private void BackEndSetUp()
    {
        var bro = Backend.Initialize(true);

        if(bro.IsSuccess())
        {
            Debug.Log($"BackEnd Server : {bro}");
        }
        else
        {
            Debug.LogError($"BackEnd Server : {bro}");
        }
    }
}
