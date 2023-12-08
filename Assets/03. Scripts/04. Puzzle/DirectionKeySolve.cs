using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.Windows;

public class DirectionKeySolve : MonoBehaviour
{
    DirectionKey puzzleKey;

    float checkValue = 0.03f; //���� üũ�� �Ǳ� �����ϴ� �̵� ����

    int[] solveKeyNum = new int[] { 2, 3, 1, 1, 2, 4 };
    int[] inputKeyNum = new int[6];
    int i;

    private void Awake()
    {
        puzzleKey = GetComponent<DirectionKey>();
        int i = 0;
    }
    private void Update()
    {
        if (puzzleKey.keyInput == true)//��ǲ�� ���� �� �ִ� ���¿��� ����
        {
            keyArrayCheck();
        }
        if (i == 6)
        {
            puzzleKey.keySole = true;
        }
    }
    void keyArrayCheck()
    {
        if (InputNum() == 0)
            return;
        inputKeyNum[i] = InputNum();
        if (solveKeyNum[i] == inputKeyNum[i])
            i++;
        else if (solveKeyNum[i] != inputKeyNum[i])
            i = 0;
        puzzleKey.keyInput = false;

        Debug.Log(i);
    }
    int InputNum()
    {
        if ((puzzleKey.keyPoint.x - puzzleKey.key.transform.position.x) < -checkValue) //�������� ���� ��
        {
            return 3;
        }
        else if ((puzzleKey.keyPoint.x - puzzleKey.key.transform.position.x) > checkValue) //���������� ���� ��
        {
            return 1;
        }
        else if ((puzzleKey.keyPoint.y - puzzleKey.key.transform.position.y) < -checkValue) //�Ʒ��� ���� ��
        {
            return 2;
        }
        else if ((puzzleKey.keyPoint.y - puzzleKey.key.transform.position.y) > checkValue) //���� ���� ��
        {
            return 4;
        }
        else
        {
            puzzleKey.keyInput = true;
            return 0;
        }

    }


}
