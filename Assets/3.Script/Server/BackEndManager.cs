using UnityEngine;
using BackEnd;    // �ڳ� SDK

public class BackEndManager : MonoBehaviour
{
    private void Awake()
    {
        // Update�� Backend.AsyncPoll() ȣ���� ���� �ش� ������Ʈ�� Destroy�� �Ǹ� �ȵȴ�.
        DontDestroyOnLoad(gameObject);

        

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
