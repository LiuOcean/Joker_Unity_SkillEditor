using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKFrame;
public class GameManager : SingletonMono<GameManager>
{

    /// <summary>
    /// �����´浵�����ҽ�����Ϸ
    /// </summary>
    public void CreateNewArchiveAndEnterGame()
    {
        // ��ʼ���浵
        DataManager.CreateArchive();
        // �����Զ����ɫ����
        SceneManager.LoadScene("CreateCharacter");
    }

    /// <summary>
    /// ʹ�þʹ浵��������Ϸ
    /// </summary>
    public void UseCurrentArchiveAndEnterGame()
    { 
        // ���ص�ǰ�浵
        DataManager.LoadCurrentArchive();
        // ������Ϸ����
        SceneManager.LoadScene("Game");
    }
}
