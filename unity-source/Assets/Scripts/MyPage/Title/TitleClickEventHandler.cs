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
        }
    }
}
