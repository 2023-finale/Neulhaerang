using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinihatPanel : MonoBehaviour
{
    public List<ClothesButton> minihatButtons;
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
        }

        // TODO
        // Update equipment state into server
    }
}