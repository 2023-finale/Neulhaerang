using UnityEditor;
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
        //RequestMemberStats();
        //RequestMemberStatus();
        //RequestDefeatMonster();
        //RequestCharacterItems();
        //ModifyCharacterItems(new MemberItem(1,2,1,1,1));
        RequestUserTitles();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // �޼ҵ尡 static�̸� �� ��
    /**
     * ��� �ɷ�ġ ��ȸ
     */
    void RequestMemberStats()
    {
        string androidMethod = "getMemberStats";
        _pluginInstance.Call(androidMethod);
    }

    void ReceiveMemberStats(string jsonMessage)
    {
        Debug.Log("heejeong [ReceiveMemberStats]" + jsonMessage);
        MemberStat datas = JsonUtility.FromJson<MemberStat>(jsonMessage);
        Debug.Log("heejeong Life" + datas.ToString());
    }

    /**
     * ��� ���� ��ȸ
     */
    void RequestMemberStatus()
    {
        string androidMethod = "getMemberStats";
        _pluginInstance.Call(androidMethod);
    }
    void ReceiveMemberStatus(string jsonMessage)
    {
        Debug.Log("heejeong [ReceiveMemberStatus]" + jsonMessage);
        MemberStatus datas = JsonUtility.FromJson<MemberStatus>(jsonMessage);

        Debug.Log("heejeong ���µ�" + datas.Indolence);
        Debug.Log("heejeong �Ƿε�" + datas.Tiredness);
    }


    /**
     * ���� ���� óġ �Ϸ�
     */
    void RequestDefeatMonster()
    {
        string androidMethod = "defeatLazyMonster";
        _pluginInstance.Call(androidMethod);
    }

    /**
     * ���� ���� ������ ��ȸ
     */
    void RequestCharacterItems()
    {
        string androidMethod = "getCharacterItems";
        _pluginInstance.Call(androidMethod);
    }

    void ReceiveCharacterItems(string jsonMessage)
    {
        Debug.Log("heejeong [ReceiveCharacterItems]" + jsonMessage);
        MemberItem datas = JsonUtility.FromJson<MemberItem>(jsonMessage);
        Debug.Log("heejeong ������ ����� ������ ::" + datas.ToString());
    }

    /**
     * ���� ���� ������ ����
     */
    void ModifyCharacterItems(MemberItem item)
    {
        string datas = JsonUtility.ToJson(item);
        Debug.Log("heejeong ���� ������ ���� ���::" + item.ToString());
        string androidMethod = "modifyCharacterItems";
        _pluginInstance.Call(androidMethod, datas);
    }

    /**
     * ���� ���� Īȣ ��ȸ
     */
    void RequestUserTitles()
    {
        string androidMethod = "getUserTitles";
        _pluginInstance.Call(androidMethod);
    }

    void ReceiveUserTitles(string jsonMessage)
    {
        Debug.Log("heejeong [ReceiveUserTitles]" + jsonMessage);
        MemberTitles datas = JsonUtility.FromJson<MemberTitles>(jsonMessage);
        Debug.Log("heejeong ������ ����� ������ ::" + datas.ToString());

        foreach (MemberTitle lt in datas.titles)
        {
            Debug.Log("heejeong ���� ���� Īȣ ���::" + lt.TitleId);
            Debug.Log("heejeong ���� ���� Īȣ ���::" + lt.Content);
        }
    }
}
