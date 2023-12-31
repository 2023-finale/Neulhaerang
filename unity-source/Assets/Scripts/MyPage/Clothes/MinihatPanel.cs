using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinihatPanel : MonoBehaviour
{
    public List<Button> minihatButtons;
    public List<GameObject> minihatObjects;

    public List<Sprite> minihatOff;
    public List<Sprite> minihatOn;

    public void ClickTab(int id)
    {
        // active -> not active
        if (minihatObjects[id].activeSelf)
        {
            minihatObjects[id].SetActive(false);
            minihatButtons[id].GetComponent<Image>().sprite = minihatOff[id];
            PlayerPrefs.SetInt("Minihat", 0);
        }
        else // not active -> all off -> activate
        {
            for (int i = 0; i < minihatObjects.Count; i++)
            {
                if (i == id)
                {
                    minihatObjects[i].SetActive(true);
                    minihatButtons[i].GetComponent<Image>().sprite = minihatOn[i];
                }
                else
                {
                    minihatObjects[i].SetActive(false);
                    minihatButtons[i].GetComponent<Image>().sprite = minihatOff[i];
                }
            }
            PlayerPrefs.SetInt("Minihat", id + 1);
        }

        // Update equipment state into server
        int bag = PlayerPrefs.GetInt("Bag");
        int glasses = PlayerPrefs.GetInt("Glasses");
        int minihat = PlayerPrefs.GetInt("Minihat");
        int scarf = PlayerPrefs.GetInt("Scarf");
        int title = PlayerPrefs.GetInt("Title");
        int skin = PlayerPrefs.GetInt("Skin");
        int hand = PlayerPrefs.GetInt("Hand");
        MemberItem datas = new MemberItem(bag, glasses, minihat, scarf, title, skin, hand);
        //var andController = new AndroidController();
        //andController.ModifyCharacterItems(datas);

        AndroidController.instance.ModifyCharacterItems(datas);
    }
}
