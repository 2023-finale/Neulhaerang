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
    public Button monsterButton;

    // Mypage Status
    public List<GameObject> bagList;
    public List<GameObject> glassesList;
    public List<GameObject> minihatList;
    public List<GameObject> scarfList;
    public Button titleObject;

    // Clothes Button Image List
    public List<Sprite> bagOn;
    public List<Sprite> glassesOn;
    public List<Sprite> minihatOn;
    public List<Sprite> scarfOn;
    public List<Sprite> titleSprites;

    public List<Button> bagButtons;
    public List<Button> glassesButtons;
    public List<Button> minihatButtons;
    public List<Button> scarfButtons;
    public List<Button> titleButtons;

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
        RequestMemberStats();
        RequestMemberStatus();
        //RequestDefeatMonster();
        RequestCharacterItems();
        //ModifyCharacterItems(new MemberItem(1, 2, 1, 1, 1));
        //RequestUserTitles();
        RequestGetUserProfile();

        //Debug.Log("stat Level : " + statLevelList[0].text);

        statLevelList.Add(GameObject.Find("GodsangLevel").GetComponent<TextMeshProUGUI>());
        statLevelList.Add(GameObject.Find("SurviveLevel").GetComponent<TextMeshProUGUI>());
        statLevelList.Add(GameObject.Find("InssaLevel").GetComponent<TextMeshProUGUI>());
        statLevelList.Add(GameObject.Find("TeunteunLevel").GetComponent<TextMeshProUGUI>());
        statLevelList.Add(GameObject.Find("GoodideaLevel").GetComponent<TextMeshProUGUI>());
        statLevelList.Add(GameObject.Find("LoveLevel").GetComponent<TextMeshProUGUI>());
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

        int index = 0;
        foreach (MemberStatItem lt in datas.stats)
        {
            Debug.Log("heejeong ���� ���� ����::" + lt.Score);
            Debug.Log("heejeong ���� ���� ����::" + lt.Level);
            scores[index] = lt.Score;
            statLevelList[index++].text = lt.Level;
            Debug.Log("stat Level : " + statLevelList[index - 1].text);
            /*TODO*/
            // Level UI ���� �� �� ǥ�� �۾� �ʿ�
        }
        scores = changeValue(scores);

        //// godsang
        //scores[0] = 2500;
        //// survive
        //scores[1] = 2000;
        //// inssa
        //scores[2] = 1500;
        //// teunteun
        //scores[3] = 2000;
        //// goodidea
        //scores[4] = 2500;
        //// love
        //scores[5] = 1500;

        Debug.Log("score 0 : " + scores[0]);
        Debug.Log("score 1 : " + scores[1]);
        Debug.Log("score 2 : " + scores[2]);
        Debug.Log("score 3 : " + scores[3]);
        Debug.Log("score 4 : " + scores[4]);
        Debug.Log("score 5 : " + scores[5]);

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
        string androidMethod = "getMemberStats";
        _pluginInstance.Call(androidMethod);
    }
    void ReceiveMemberStatus(string jsonMessage)
    {
        Debug.Log("heejeong [ReceiveMemberStatus]" + jsonMessage);
        MemberStatus datas = JsonUtility.FromJson<MemberStatus>(jsonMessage);

        Debug.Log("heejeong ���µ�" + datas.Indolence);
        Debug.Log("heejeong �Ƿε�" + datas.Tiredness);

        // ���µ��� 50 ���ϸ� ���±��� �ȳ�Ÿ����
        if (monsterButton != null && datas.Indolence < 50)
        {
            Image buttonImage = monsterButton.GetComponent<Image>();
            Color newColor = buttonImage.color;
            newColor.a = 0.0f;
            buttonImage.color = newColor;
        }

    }


    /**
     * ���� ���� óġ �Ϸ�
     */
    public void RequestDefeatMonster()
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

        /*
         * MemberItem list
         * 
         * backpack;
         * glasses;
         * hat;
         * scarf;
         * title;
         */

        // �������� �����ߴٸ� Active

        if (datas.Backpack != 0)
        {
            bagList[datas.Backpack].SetActive(true);
            bagButtons[datas.Backpack].GetComponent<Image>().sprite = bagOn[datas.Backpack];
        }

        if (datas.Glasses != 0)
        {
            glassesList[datas.Glasses].SetActive(true);
            glassesButtons[datas.Glasses].GetComponent<Image>().sprite = glassesOn[datas.Glasses];
        }

        if (datas.Hat != 0)
        {
            minihatList[datas.Hat].SetActive(true);
            minihatButtons[datas.Hat].GetComponent<Image>().sprite = minihatOn[datas.Hat];
        }

        if (datas.Scarf != 0)
        {
            scarfList[datas.Scarf].SetActive(true);
            scarfButtons[datas.Scarf].GetComponent<Image>().sprite = scarfOn[datas.Scarf];
        }

        if (datas.Title != 0)
        {
            // transparent
            Image buttonImage = titleObject.GetComponent<Image>();
            Color newColor = buttonImage.color;
            newColor.a = 1.0f;
            buttonImage.color = newColor;
            Debug.Log("color : " + newColor);
            titleObject.GetComponent<Image>().sprite = titleSprites[datas.Title];
        }

        // ������ ���� ����
        PlayerPrefs.SetInt("Bag", datas.Backpack);
        PlayerPrefs.SetInt("Glasses", datas.Glasses);
        PlayerPrefs.SetInt("Minihat", datas.Hat);
        PlayerPrefs.SetInt("Scarf", datas.Scarf);
        PlayerPrefs.SetInt("Title", datas.Title);
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

        foreach (MemberTitle lt in datas.titles)
        {
            Debug.Log("heejeong ���� ���� Īȣ ���::" + lt.TitleId);
            Debug.Log("heejeong ���� ���� Īȣ ���::" + lt.Content);
        }

        /*TODO*/
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
            Debug.Log("heejeong ��ó �����::" +it.ToString());
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
