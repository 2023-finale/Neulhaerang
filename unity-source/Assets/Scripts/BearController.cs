using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour
{
    private int score = 0;

    void Start()
    {
        // �ʱ�ȭ�� ������ �ۼ��մϴ�.
    }

    void Update()
    {
        // ������Ʈ�� ������ �ۼ��մϴ�.
    }

    void OnMouseDown()
    {
        // ������Ʈ�� ��ġ���� �� ȣ��˴ϴ�.
        IncreaseScore();
    }

    void IncreaseScore()
    {
        score++;
        Debug.Log("Score: " + score);

        // ���⿡�� ������ ������Ʈ�ϰ� �ʿ��� �۾��� �����մϴ�.
    }
}
