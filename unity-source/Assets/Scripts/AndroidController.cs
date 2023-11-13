using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AndroidController : MonoBehaviour
{
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

    public List<ClothesButton> bagButtons;
    public List<ClothesButton> glassesButtons;
    public List<ClothesButton> minihatButtons;
    public List<ClothesButton> scarfButtons;
    public List<TitleButton> titleButtons;

    
    

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
        RequestDefeatMonster();
        RequestCharacterItems();
        //ModifyCharacterItems(new MemberItem(1, 2, 1, 1, 1));
        //RequestUserTitles();
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

        // StartStat

        int[] scores = new int[6];

        scores[0] = datas.Life;
        scores[1] = datas.Survival;
        scores[2] = datas.Popularity;
        scores[3] = datas.Power;
        scores[4] = datas.Creative;
        scores[5] = datas.Love;

        scores = changeValue(scores);
        /*
         * A+ => 2500
         * A  => 2000
         * B+ => 1500
         * B  => 1000
         * C+ => 500
         */

        Stats stats = new Stats(scores[0], scores[1], scores[2], scores[3], scores[4], scores[5]);
        statsRadarChart.SetStats(stats);
    }

    /*
     * Radar chart �� �°� ��ġ ����
     */
    private int[] changeValue(int[] scores)
    {
        for (int i = 0; i < 6; i++)
        {
            if (scores[i] >= 150) scores[i] = 2500;
            else
            {
                scores[i] = scores[i] * 2500 / 150;
            }

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
}
