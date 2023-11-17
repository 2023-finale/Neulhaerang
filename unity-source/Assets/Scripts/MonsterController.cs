using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MonsterController : MonoBehaviour
{
    public static MonsterController instance;

    AndroidJavaObject _pluginInstance;


    void Awake()
    {
        // �����͸� ���� �ȵ���̵� �÷������� Ŭ���� �ּ�
        string androidClass = "com.finale.neulhaerang.data.unity.MonsterWithUnity";

        // �����͸� ���� �ȵ���̵� Ŭ������ �ν��Ͻ�ȭ
        _pluginInstance = new AndroidJavaObject(androidClass);

        // �̱���
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }


    /**
     * ���� ���� óġ �Ϸ�
     */
    public void RequestDefeatMonster()
    {
        Debug.Log("heejeong MonsterController Call: RequestDefeatMonster");
        string androidMethod = "defeatLazyMonster";
        _pluginInstance.Call(androidMethod);
    }

}
