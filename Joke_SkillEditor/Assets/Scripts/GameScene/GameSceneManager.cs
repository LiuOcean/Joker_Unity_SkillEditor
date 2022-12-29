using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKFrame;
public class GameSceneManager : LogicManagerBase<GameSceneManager>
{
    #region �����߼�
    public bool IsTest;
    public bool IsCreateArchive;
    #endregion
    private void Start()
    {
        #region �����߼�
        if (IsTest)
        {
            if (IsCreateArchive)
            {
                DataManager.CreateArchive();
            }
            else
            {
                DataManager.LoadCurrentArchive();
            }
        }
        #endregion
        // ��ʼ����ɫ
        Player_Controller.Instance.Init();
    }
}
