using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JKFrame;
/// <summary>
/// ְҵѡ��ť
/// </summary>
public class UI_ProfessionButton : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] Image icon;
    [SerializeField] Image selectFrame;
    [SerializeField] Text nameText;
    [SerializeField] AudioClip clickAudioClip;

    public ProfessionType ProfessionType { get; private set; }
    private UI_CreateCharacterWindow window;

    private static Color[] colors;
    static UI_ProfessionButton()
    {
        colors = new Color[2];
        colors[0] = Color.white;
        colors[1] = new Color(0.964f, 0.882f, 0.611f);
    }

    /// <summary>
    /// ��ʼ��
    /// </summary>
    public void Init(UI_CreateCharacterWindow window,ProfessionType professionType)
    {
        button.onClick.AddListener(ButtonClick);
        this.window = window;
        this.ProfessionType = professionType;
        UnSelect(); // Ĭ��û��ѡ��
    }

    /// <summary>
    /// ������¼�
    /// </summary>
    private void ButtonClick()
    {
        // ���ߴ��ڣ���ǰ��ť�����ְҵ�����ѡ����
        AudioManager.Instance.PlayOnShot(clickAudioClip, Vector3.zero, 1, false);
        window.SelectProfession_Button(this);
    }

    /// <summary>
    /// ѡ��
    /// </summary>
    public void Select()
    {
        icon.color = colors[1];
        nameText.color = colors[1];
        selectFrame.enabled = true;
    }
    /// <summary>
    /// ȡ��ѡ��
    /// </summary>
    public void UnSelect()
    {
        icon.color = colors[0];
        nameText.color = colors[0];
        selectFrame.enabled = false;
    }
}
