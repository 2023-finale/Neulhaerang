using UnityEngine;

public class AndroidController : MonoBehaviour
{
    AndroidJavaObject _pluginInstance;

    void Awake()
    {
        // �����͸� ���� �ȵ���̵� �÷������� Ŭ���� �ּ�
        string androidClass = "com.finale.neulhaerang.data.unity.TransferWithUnity";

        // �����͸� ���� �ȵ���̵� Ŭ������ �ν��Ͻ�ȭ
        _pluginInstance = new AndroidJavaObject(androidClass);
    }

    // Start is called before the first frame update
    void Start()
    {
        RequestMemberStats();
        RequestMemberStatus();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //���� ��ȸ

    void RequestMemberStats()
    {
        string androidMethod = "getMemberStats";
        _pluginInstance.Call(androidMethod);
    }

    // �޼ҵ尡 static�̸� �� ��
    void ReceiveMemberStats(string jsonMessage)
    {
        Debug.Log("heejeong ReceiveMemberStats");
        MemberStat datas = JsonUtility.FromJson<MemberStat>(jsonMessage);

        //������ �����ͷ� �۾� ����: �α� ����
        Debug.Log("heejeong Life" + datas.Life);
        Debug.Log("heejeong Love" + datas.Love);
        Debug.Log("heejeong Survival" + datas.Survival);
        Debug.Log("heejeong Popularity" + datas.Popularity);
        Debug.Log("heejeong Power" + datas.Power);
        Debug.Log("heejeong Creative" + datas.Creative);

        //foreach (var data in datas)
        //{
        //    Debug.Log("heejeong" + data.ToString());
        //}
    }


    //���� ����

    //���� ��ȸ

    void RequestMemberStatus()
    {
        string androidMethod = "getMemberStatus";
        _pluginInstance.Call(androidMethod);
    }

    // �޼ҵ尡 static�̸� �� ��
    void ReceiveMemberStatus(string jsonMessage)
    {
        //������ JSON �����͸� UserData��ü�� ������ȭ (UnityEngine ���̺귯�� ���)
        MemberStatus datas = JsonUtility.FromJson<MemberStatus>(jsonMessage);

        //������ �����ͷ� �۾� ����: �α� ����
        Debug.Log("heejeong ���µ�" + datas.Indolence);
        Debug.Log("heejeong �Ƿε�" + datas.Tiredness);
    }

    //���� ���� óġ �Ϸ�

    //���� ������ ��ȸ

    //���� ������ ����
}
