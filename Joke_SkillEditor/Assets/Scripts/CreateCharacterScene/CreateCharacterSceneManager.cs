using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKFrame;
public class CreateCharacterSceneManager : LogicManagerBase<CreateCharacterSceneManager>
{
    private void Start()
    {
        // ��ʼ����ɫ������
        CharacterCreator.Instance.Init();
        // ��ʾ������ɫ��������
        UIManager.Instance.Show<UI_CreateCharacterWindow>();
    }
}
