using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SocialController : MonoBehaviour
{
    public static SocialController instance;

    AndroidJavaObject _pluginInstance;
   

    void Awake()
    {
        // �����͸� ���� �ȵ���̵� �÷������� Ŭ���� �ּ�
        string androidClass = "com.finale.neulhaerang.data.unity.SocialWithUnity";

        // �����͸� ���� �ȵ���̵� Ŭ������ �ν��Ͻ�ȭ
        _pluginInstance = new AndroidJavaObject(androidClass);

        // �̱���
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        RequestNearByUsers();
        RequestFriendStats(4);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // �޼ҵ尡 static�̸� �� ��
    

    /**
     * Ŭ���� ����� ���� ��ȸ
     */
    void RequestFriendStats(long memberId)
    {
        Debug.Log("heejeong SocialController Call: RequestFriendStats(" + memberId+")");
        string androidMethod = "getFriendStats";
        _pluginInstance.Call(androidMethod, memberId);
    }

    void ReceiveFriendStats(string jsonMessage)
    {
        int[] scores = new int[6];

        Debug.Log("heejeong [ReceiveFriendStats]" + jsonMessage);
        MemberStats datas = JsonUtility.FromJson<MemberStats>(jsonMessage);

        //for (var i = 0; i < scores.Length; i++)
        //{
        //    Debug.Log("heejeong ���� ���� ����::" + datas.stats[i].Score);
        //    Debug.Log("stat type : " + datas.stats[i].Score.GetType().Name);
        //    Debug.Log("heejeong ���� ���� ����::" + datas.stats[i].Level);
        //    Debug.Log("level type : " + datas.stats[i].Level.GetType().Name);
        //}
    }

    /**
     * ���� ����� ��ȸ
     */
    public void RequestNearByUsers()
    {
        Debug.Log("heejeong SocialController Call: RequestNearByUsers");
        string androidMethod = "getNearByUsers";
        _pluginInstance.Call(androidMethod);
    }

    public void ReceiveNearByUsers(string jsonMessage)
    {
        Debug.Log("heejeong [ReceiveNearByUsers]" + jsonMessage);
        AroundMembers datas = JsonUtility.FromJson<AroundMembers>(jsonMessage);
        foreach (AroundMember it in datas.members)
        {
            Debug.Log("heejeong ��ó �����::" + it.ToString());
        }
    }


    /**
     * ���� ����� Ŭ�� ��
     */
    void RequestClickOtherUser(long memberId)
    {
        Debug.Log("heejeong [RequestClickOtherUser]" + memberId);
        string androidMethod = "clickOtherUser";
        _pluginInstance.Call(androidMethod, memberId);
    }

}
