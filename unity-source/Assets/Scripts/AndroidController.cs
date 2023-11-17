using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class AndroidController : MonoBehaviour
{
    public static AndroidController instance;

    AndroidJavaObject _pluginInstance;

    [SerializeField] private StatsRadarChart statsRadarChart;

    // Monster button
    public GameObject monsterButton;

    // Mypage Status
    public List<GameObject> bagList;
    public List<GameObject> glassesList;
    public List<GameObject> minihatList;
    public List<GameObject> scarfList;
    public Button titleObject;
    //public List<GameObject> skinList;
    public List<GameObject> handList;

    // Clothes Button Image List
    public List<Sprite> bagOn;
    public List<Sprite> glassesOn;
    public List<Sprite> minihatOn;
    public List<Sprite> scarfOn;
    public List<Sprite> titleOn;
    //public List<Sprite> skinOn;
    public List<Sprite> handOn;

    public List<Button> bagButtons;
    public List<Button> glassesButtons;
    public List<Button> minihatButtons;
    public List<Button> scarfButtons;
    public List<Button> titleButtons;
    //public List<Button> skinButtons;
    public List<Button> handButtons;

    // ������ ������ ǥ���ϴ� �ؽ�Ʈ
    public List<TextMeshProUGUI> statLevelList;

    void Awake()
    {
        // �����͸� ���� �ȵ���̵� �÷������� Ŭ���� �ּ�
        string androidClass = "com.finale.neulhaerang.data.unity.TransferWithUnity";

        // �����͸� ���� �ȵ���̵� Ŭ������ �ν��Ͻ�ȭ
        _pluginInstance = new AndroidJavaObject(androidClass);

        // �̱���
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Level UI �߰�
        statLevelList.Add(GameObject.Find("GodsangLevel").GetComponent<TextMeshProUGUI>());
        statLevelList.Add(GameObject.Find("SurviveLevel").GetComponent<TextMeshProUGUI>());
        statLevelList.Add(GameObject.Find("InssaLevel").GetComponent<TextMeshProUGUI>());
        statLevelList.Add(GameObject.Find("TeunteunLevel").GetComponent<TextMeshProUGUI>());
        statLevelList.Add(GameObject.Find("GoodideaLevel").GetComponent<TextMeshProUGUI>());
        statLevelList.Add(GameObject.Find("LoveLevel").GetComponent<TextMeshProUGUI>());

        RequestMemberStats();
        RequestMemberStatus();
        RequestCharacterItems();
        RequestUserTitles();
        RequestGetUserProfile();
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
        //// StartStat

        int[] scores = new int[6];

        Debug.Log("heejeong [ReceiveMemberStats]" + jsonMessage);
        MemberStats datas = JsonUtility.FromJson<MemberStats>(jsonMessage);

        for (var i = 0; i < scores.Length; i++)
        {
            Debug.Log("heejeong ���� ���� ����::" + datas.stats[i].Score);
            Debug.Log("stat type : " + datas.stats[i].Score.GetType().Name);
            Debug.Log("heejeong ���� ���� ����::" + datas.stats[i].Level);
            Debug.Log("level type : " + datas.stats[i].Level.GetType().Name);
            scores[i] = datas.stats[i].Score;
            statLevelList[i].text = datas.stats[i].Level.ToString();
        }

        scores = changeValue(scores);
        Stats stats = new Stats(scores[0], scores[1], scores[2], scores[3], scores[4], scores[5]);
        statsRadarChart.SetStats(stats);
    }

    /*
    * Radar chart �� �°� ��ġ ����
    * A+ => 2500       150
    * A  => 2100       120
    * B+ => 1700       90
    * B  => 1300       60
    * C+ => 900        30
    * C  => 500        0
    */
    private int[] changeValue(int[] scores)
    {
        for (int i = 0; i < 6; i++)
        {
            if (scores[i] >= 150) scores[i] = 2000;
            else
            {
                scores[i] = scores[i] * 2000 / 150;
            }
            scores[i] += 500;
            Debug.Log("change score value - index : " + i + "  value : " + scores[i]);
        }
        return scores;
    }

    /**
     * ��� ���� ��ȸ
     */
    void RequestMemberStatus()
    {
        string androidMethod = "getMemberStatus";
        _pluginInstance.Call(androidMethod);
    }
    void ReceiveMemberStatus(string jsonMessage)
    {
        Debug.Log("heejeong [ReceiveMemberStatus]" + jsonMessage);
        MemberStatus datas = JsonUtility.FromJson<MemberStatus>(jsonMessage);

        Debug.Log("heejeong ���µ�" + datas.Indolence);
        Debug.Log("heejeong �Ƿε�" + datas.Tiredness);


        if (monsterButton == null) Debug.Log("junyeong ���� ��ư null Ȯ��" + monsterButton);
        // ���µ��� 50 ���ϸ� ���±��� �ȳ�Ÿ����
        if (monsterButton != null && datas.Indolence < 50)
        {
            //Image buttonImage = monsterButton.GetComponent<Image>();
            //Color newColor = buttonImage.color;
            //newColor.a = 0.0f;
            //buttonImage.color = newColor;

            monsterButton.SetActive(false);
        }

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

        /*
         * MemberItem list
         * 
         * backpack;
         * glasses;
         * hat;
         * scarf;
         * title;
         * skin;
         * hand;
         */

        // �������� �����ߴٸ� Active
        ClothesInit();

        if (datas.Backpack != 0)
        {
            int index = datas.Backpack - 1;
            bagList[index].SetActive(true);
            bagButtons[index].GetComponent<Image>().sprite = bagOn[index];
        }

        if (datas.Glasses != 0)
        {
            int index = datas.Glasses - 1;
            glassesList[index].SetActive(true);
            glassesButtons[index].GetComponent<Image>().sprite = glassesOn[index];
        }

        if (datas.Hat != 0)
        {
            int index = datas.Hat - 1;
            minihatList[index].SetActive(true);
            minihatButtons[index].GetComponent<Image>().sprite = minihatOn[index];
        }

        if (datas.Scarf != 0)
        {
            int index = datas.Scarf - 1;
            scarfList[index].SetActive(true);
            scarfButtons[index].GetComponent<Image>().sprite = scarfOn[index];
        }

        if (datas.Title != 0)
        {
            // transparent
            int index = datas.Title - 1;
            Image buttonImage = titleObject.GetComponent<Image>();
            Color newColor = buttonImage.color;
            newColor.a = 1.0f;
            buttonImage.color = newColor;
            Debug.Log("color : " + newColor);
            titleObject.GetComponent<Image>().sprite = titleOn[index];
        }

        //if (datas.Skin != 0)
        //{
        //    scarfList[datas.Skin].SetActive(true);
        //    scarfButtons[datas.Skin].GetComponent<Image>().sprite = scarfOn[datas.Skin];
        //}

        if (datas.Hand != 0)
        {
            Debug.Log("junyeong change hand");
            int index = datas.Hand - 1;
            handList[index].SetActive(true);
            handButtons[index].GetComponent<Image>().sprite = handOn[index];
        }

        // ������ ���� ����
        PlayerPrefs.SetInt("Bag", datas.Backpack);
        PlayerPrefs.SetInt("Glasses", datas.Glasses);
        PlayerPrefs.SetInt("Minihat", datas.Hat);
        PlayerPrefs.SetInt("Scarf", datas.Scarf);
        PlayerPrefs.SetInt("Title", datas.Title);
        PlayerPrefs.SetInt("Skin", datas.Skin);
        PlayerPrefs.SetInt("Hand", datas.Hand);
        PlayerPrefs.Save();
    }

    void ClothesInit()
    {
        foreach (var bag in bagList)
        {
            bag.SetActive(false);
        }

        foreach (var glasses in glassesList)
        {
            glasses.SetActive(false);
        }

        foreach (var minihat in minihatList)
        {
            minihat.SetActive(false);
        }

        foreach (var scarf in scarfList)
        {
            scarf.SetActive(false);
        }

        foreach (var hand in handList)
        {
            hand.SetActive(false);
        }
    }

    /**
     * ���� ���� ������ ����
     */
    public void ModifyCharacterItems(MemberItem item)
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

        for (var i = 0; i < datas.titles.Length; i++)
        {
            // Ȱ��ȭ
            titleButtons[(int)datas.titles[i].TitleId - 1].GetComponent<Image>().sprite = titleOn[(int)datas.titles[i].TitleId - 1];            
        }
    }

    /**
     * ���� ����� ��ȸ
     */
    public void RequestNearByUsers()
    {
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

    /**
    * ���� ������ ��ȸ (����, ����ġ, �г���)
    */
    void RequestGetUserProfile()
    {
        string androidMethod = "getUserProfile";
        _pluginInstance.Call(androidMethod);
    }

    void ReceiveGetUserProfile(string jsonMessage)
    {
        Debug.Log("junyeong [ReceiveGetUserProfile]" + jsonMessage);
        MemberProfile datas = JsonUtility.FromJson<MemberProfile>(jsonMessage);
        Debug.Log("junyeong ����� ������ ����ġ ::" + datas.ToString());

        Debug.Log("junyeong ����� ����" + datas.Level);
        Debug.Log("junyeong ����� ���� ����ġ" + datas.NxtExp);
        Debug.Log("junyeong ����� ���� ����ġ" + datas.CurExp);
        Debug.Log("junyeong ����� �г���" + datas.Nickname);
    }
}