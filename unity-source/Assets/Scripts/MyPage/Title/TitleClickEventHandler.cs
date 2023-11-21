using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TitleClickEventHandler : MonoBehaviour
{
    public Button[] buttons;
    public Button titleObject;

    void Start()
    {
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => OnButtonClick(button));
        }
    }

    // ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    void OnButtonClick(Button clickedButton)
    {
        // Ŭ���� ��ư�� �̹��� ��������
        Image clickedImage = clickedButton.GetComponent<Image>();

        // Ŭ���� ��ư�� �̹��� �̸� Ȯ��
        string buttonImageName = clickedImage.sprite.name;

        // �̹��� �̸��� "on_"���� �����ϴ� ��쿡�� ó��
        if (buttonImageName.StartsWith("on_"))
        {
            titleObject.GetComponent<Image>().sprite = clickedImage.sprite;

            // "on_" ������ ���� ����
            string numberPart = buttonImageName.Substring(3);

            // ���ڸ� int�� ��ȯ
            if (int.TryParse(numberPart, out int number))
            {
                PlayerPrefs.SetInt("Title", number);
                int bag = PlayerPrefs.GetInt("Bag");
                int glasses = PlayerPrefs.GetInt("Glasses");
                int minihat = PlayerPrefs.GetInt("Minihat");
                int scarf = PlayerPrefs.GetInt("Scarf");
                int title = PlayerPrefs.GetInt("Title");
                int skin = PlayerPrefs.GetInt("Skin");
                int hand = PlayerPrefs.GetInt("Hand");
                MemberItem datas = new MemberItem(bag, glasses, minihat, scarf, title, skin, hand);
                AndroidController.instance.ModifyCharacterItems(datas);
            }
            else
            {
                Debug.Log("junyeong ���� ��ȯ ����");
            }
        }
    }
}
