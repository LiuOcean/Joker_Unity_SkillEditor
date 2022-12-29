using JKFrame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ɫ��������_��۲˵���ѡ��
/// </summary>
public class UI_FacadeMenus_Tab : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Image iconImg;
    [SerializeField] Image tabFocus;
    private UI_CreateCharacterWindow window;

    [SerializeField] AudioClip clickAudioClip;

    /// <summary>
    ///  ��ǰ�˵�ѡ�� ������������
    /// </summary>
    public CharacterPartType CharacterPartType { get; private set; }

    private static Color[] colors;
    static UI_FacadeMenus_Tab()
    {
        colors = new Color[2];
        colors[0] = Color.white;
        colors[1] = new Color(0.964f, 0.882f, 0.611f);
    }
    /// <summary>
    /// ��ʼ��
    /// </summary>
    public void Init(UI_CreateCharacterWindow window,CharacterPartType characterPartType)
    {
        this.window = window;
        this.CharacterPartType = characterPartType;
        button.onClick.AddListener(ButtonClick);
        UnSelect();
    }

    private void ButtonClick()
    {
        // ���ߴ��ڣ���ǰ��ť�����ְҵ�����ѡ����
        AudioManager.Instance.PlayOnShot(clickAudioClip, Vector3.zero, 1, false);
        window.SelectFacedeMenusTab(this);
    }
    /// <summary>
    /// ѡ��
    /// </summary>
    public void Select()
    {
        iconImg.color = colors[1];
        tabFocus.enabled = true;
    }
    /// <summary>
    /// ȡ��ѡ��
    /// </summary>
    public void UnSelect()
    {
        iconImg.color = colors[0];
        tabFocus.enabled = false;
    }
}
