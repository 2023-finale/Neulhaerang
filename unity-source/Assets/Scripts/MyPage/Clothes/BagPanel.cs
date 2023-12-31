using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagPanel : MonoBehaviour
{
    public List<Button> bagButtons;
    public List<GameObject> bagObjects;

    public List<Sprite> bagOff;
    public List<Sprite> bagOn;

    public void ClickTab(int id)
    {   
        // active -> not active
        if (bagObjects[id].activeSelf) 
        {
            bagObjects[id].SetActive(false);
            bagButtons[id].GetComponent<Image>().sprite = bagOff[id];
            PlayerPrefs.SetInt("Bag", 0);
        }
        else // not active -> all off -> activate
        {
            for (int i = 0; i < bagObjects.Count; i++)
            {
                if (i == id)
                {
                    bagObjects[i].SetActive(true);
                    bagButtons[i].GetComponent<Image>().sprite = bagOn[i];
                }
                else
                {
                    bagObjects[i].SetActive(false);
                    bagButtons[i].GetComponent<Image>().sprite = bagOff[i];
                }
            }
            PlayerPrefs.SetInt("Bag", id + 1);
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
