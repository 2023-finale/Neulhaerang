using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterSceneManager : MonoBehaviour
{
    public GameObject ElementalSuperDemonDark;
    public GameObject ElementalSuperDemonFire;
    public GameObject ElementalSuperDemonWater;

    void Update()
    {
        if(!ElementalSuperDemonDark.activeSelf && !ElementalSuperDemonFire.activeSelf && !ElementalSuperDemonWater.activeSelf)
        {
            Invoke("GotoGameOverScene", 1.0f);
        }
    }

    void GotoGameOverScene()
    {
        // �ȵ���̵� ���� ���� óġ API �Լ� ȣ��
        GoSceneManager goSceneManager = GameObject.Find("MonsterSceneManager").GetComponent<GoSceneManager>();
        goSceneManager.GotoSceneSingle("GameOverScene");
    }
}
