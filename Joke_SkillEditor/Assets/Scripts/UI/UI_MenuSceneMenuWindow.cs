using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JKFrame;
using System;

[UIElement(false, "UI_MenuSceneMenuWindow",1)]
public class UI_MenuSceneMenuWindow : UI_WindowBase
{
    [SerializeField] Button continueButton;
    [SerializeField] Button startButton;
    [SerializeField] Button quitButton;
    public override void Init()
    {
        continueButton.onClick.AddListener(ContinueButtonClick);
        startButton.onClick.AddListener(StartButtonClick);
        quitButton.onClick.AddListener(QuitButtonClick);
        // �����ǰû�д浵��Ӧ�����ؼ�����Ϸ��ť
        if (!DataManager.HaveArchive)
        {
            continueButton.gameObject.SetActive(false);
        }
    }

    private void ContinueButtonClick()
    {
        Close();
        // ʹ�õ�ǰ�浵������Ϸ
        GameManager.Instance.UseCurrentArchiveAndEnterGame();
    }
    private void StartButtonClick()
    {
        Close();
        // �����浵������Ϸ -�� �����Զ����ɫ����
        GameManager.Instance.CreateNewArchiveAndEnterGame();
    }

    public override void OnClose()
    {
        base.OnClose();
        // �ͷ�������Դ
        ResManager.ReleaseInstance(gameObject);
    }

    private void QuitButtonClick()
    {
        Application.Quit();
    }

}
