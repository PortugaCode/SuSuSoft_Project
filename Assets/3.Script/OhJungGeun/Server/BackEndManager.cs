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
        #region [�̱���]
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

        //�ڳ� ���� �ʱ�ȭ
        BackEndSetUp();
    }

    private void Update()
    {
        //������ �񵿱� �޼��� ȣ�� (�ݹ� �Լ� ����)�� ���� �ۼ�
        if(Backend.IsInitialized)
        {
            Debug.Log("�񵿱� �޼��� ��");
            Backend.AsyncPoll();

            Backend.Match.Poll(); //��ġ ���� �񵿱� �޼��� ȣ���� ���� �ۼ�
            Backend.Chat.Poll();
            chatManager.ReceiveChat();

        }
    }


    private void BackEndSetUp()
    {
        //�ڳ� �ʱ�ȭ
        var bro = Backend.Initialize(true);

        //�ڳ� �ʱ�ȭ�� ���� ���䰪
        if(bro.IsSuccess())
        {
            //�ʱ�ȭ ���� �� statusCode 204 Success
            Debug.Log($"�ʱ�ȭ ���� : {bro}");
        }
        else
        {
            //�ʱ�ȭ ���� �� statusCode 400�� ���� �߻�
            Debug.LogError($"�ʱ�ȭ ���� : {bro}");
        }
    }
}
