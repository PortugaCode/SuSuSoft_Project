using UnityEngine;
using BackEnd;    // Backend SDK
using BackEnd.Tcp;

public class BackEndManager : MonoBehaviour
{
    public static BackEndManager Instance;

    [SerializeField] private MatchSystem matchSystem;
    [SerializeField] private ChatManager chatManager;
    public string googleHashKey;

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

        Backend.Match.OnMatchMakingRoomInviteResponse = (MatchMakingInteractionEventArgs args) =>
        {
            // TODO
            if(args.ErrInfo == ErrorCode.Match_Making_InvalidRoom)
            {
                chatManager.chatListManager.matchInvitedFail.SetActive(true);
            }
            Debug.Log(args.ErrInfo);
        };
    }

    private void Start()
    {
        Backend.ErrorHandler.InitializePoll(true);
        Backend.ErrorHandler.OnOtherDeviceLoginDetectedError = () => {
            Debug.Log("중복 로그인 감지");
            Backend.MultiCharacter.Account.LogoutAccount();
            // Loading 씬으로 이동
            Utils.Instance.LoadScene(SceneNames.Loading);
        };
    }

    private void Update()
    {
        if(Backend.IsInitialized)
        {
            Debug.Log("AsyncPoll On");
            Backend.AsyncPoll();

            Backend.Match.Poll();
            matchSystem.SetTimer();

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
            GoogleHashKey();
        }
        else
        {
            Debug.LogError($"BackEnd Server : {bro}");
        }
    }

    private void GoogleHashKey()
    {
        googleHashKey = Backend.Utils.GetGoogleHash();
        Debug.Log("GoogleHashKey 확인 : " + googleHashKey);
    }
}
