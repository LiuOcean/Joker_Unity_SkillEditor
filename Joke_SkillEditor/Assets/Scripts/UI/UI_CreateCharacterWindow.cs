using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using JKFrame;
using UnityEngine.EventSystems;
using System;

[UIElement(false, "UI_CreateCharacterWindow",2)]
public class UI_CreateCharacterWindow : UI_WindowBase
{
    // ģ�ͽ���Image
    [SerializeField] Image modelTouchImage;
    // ��λ����Text
    [SerializeField] Text partNameText;
    // ���ͷ��ť
    [SerializeField] Button leftArrowButton;
    // �Ҽ�ͷ��ť
    [SerializeField] Button rightArrowButton;

    [SerializeField] Slider sizeSlider;
    [SerializeField] Slider heightSlider;
    [SerializeField] Button color1Button;
    [SerializeField] Button color2Button;

    [SerializeField] Button backButton;
    [SerializeField] Button submitButton;

    // ���е�ְҵ��ť
    [SerializeField] UI_ProfessionButton[] professionButtons;
    // ���е�������Ͱ�ť
    [SerializeField] UI_FacadeMenus_Tab[] facadeMenus_Tabs;
    [SerializeField] AudioClip arrowClickAudioClip;

    // ��ǰѡ���ְҵ��ť
    private UI_ProfessionButton currentProfessionButton;
    // ��ǰѡ����������
    private UI_FacadeMenus_Tab currentFacadeMenus_Tab;

    // �Զ����ɫ������
    private CustomCharacterData customCharacterData => DataManager.CustomCharacterData;
    private ProjectConfig projectConfig;
    // ��ҵ�ǰÿһ����λ ѡ�����projectConfig�еڼ�������
    private Dictionary<int, int> characterConfigIndexDic;

    public override void Init()
    {
        // ��ȡ����
        projectConfig = ConfigManager.Instance.GetConfig<ProjectConfig>(ConfigTool.ProjectConfigName);
        characterConfigIndexDic = new Dictionary<int, int>(3);
        characterConfigIndexDic.Add((int)CharacterPartType.Face, 0);
        characterConfigIndexDic.Add((int)CharacterPartType.Hair, 0);
        characterConfigIndexDic.Add((int)CharacterPartType.Cloth, 0);

        // ��modelTouchImage����ק�¼�
        modelTouchImage.OnDrag(ModelTouchImageDrag,6);
        leftArrowButton.onClick.AddListener(LeftArrowButtonClick);
        rightArrowButton.onClick.AddListener(RightArrowButtonClick);
        sizeSlider.onValueChanged.AddListener(OnSizeSliderValueChanged);
        heightSlider.onValueChanged.AddListener(OnHeightSliderValueChanged);

        // ����ɫѡ�ť�ĵ���¼�
        color1Button.onClick.AddListener(Color1ButtonClick);
        color2Button.onClick.AddListener(Color2ButtonClick);

        // ��ʼ��������Ͳ˵�
        facadeMenus_Tabs[0].Init(this, CharacterPartType.Face);
        facadeMenus_Tabs[1].Init(this, CharacterPartType.Hair);
        facadeMenus_Tabs[2].Init(this, CharacterPartType.Cloth);
        // ѡ��Ĭ��������� (����)
        SelectFacedeMenusTab(facadeMenus_Tabs[0]);
        // Ӧ��Ĭ�ϵĲ�λ
        SetCharacterPart(CharacterPartType.Face, customCharacterData.CustomPartDataDic.Dictionary[(int)CharacterPartType.Face].Index, true, true);
        SetCharacterPart(CharacterPartType.Hair, customCharacterData.CustomPartDataDic.Dictionary[(int)CharacterPartType.Hair].Index, false, true);
        SetCharacterPart(CharacterPartType.Cloth, customCharacterData.CustomPartDataDic.Dictionary[(int)CharacterPartType.Cloth].Index, false, true);

        // ��ʼ��ְҵ��ť
        professionButtons[0].Init(this, ProfessionType.Warrior);
        professionButtons[1].Init(this, ProfessionType.Assassin);
        professionButtons[2].Init(this, ProfessionType.Archer);
        professionButtons[3].Init(this, ProfessionType.Tanke);
        // Ĭ��ѡ��һ��ְҵ (սʿ)
        SelectProfession_Button(professionButtons[0]);

        backButton.onClick.AddListener(BackButtonClick);
        submitButton.onClick.AddListener(SubmitButtonClick);
        
    }

    private void BackButtonClick()
    {
        Close();
        SceneManager.LoadScene("Menu");
    }
    private void SubmitButtonClick()
    { 
        Close();
        // ��������
        DataManager.SaveCustomCharacterData();
        // ������Ϸ����
        SceneManager.LoadScene("Game");
    }


    // ���һ������
    float lastPosXOnDragModel = 0;
    /// <summary>
    /// ��ģ�ͽ���������קʱ�Ļص�
    /// </summary>
    private void ModelTouchImageDrag(PointerEventData eventData, object[] arg2)
    {
        float offset = eventData.position.x - lastPosXOnDragModel;
        lastPosXOnDragModel = eventData.position.x;
        CharacterCreator.Instance.RotateCharacter(new Vector3(0, -offset * Time.deltaTime * 60f, 0));
    }

    /// <summary>
    /// ѡ��ְҵ��ť
    /// </summary>
    public void SelectProfession_Button(UI_ProfessionButton newButton)
    {
        // ��������ɶҲ����
        if (currentProfessionButton == newButton) return;

        // ����֮ǰ��ְҵ��ť
        if (currentProfessionButton!=null) currentProfessionButton.UnSelect();

        // �����°�ť
        newButton.Select();
        currentProfessionButton = newButton;
        SelectProfession(newButton.ProfessionType);
    }

    /// <summary>
    /// ѡ��ְҵ
    /// </summary>
    private void SelectProfession(ProfessionType professionType)
    {
        // ����ʵ�ʵ�ְҵ�л��߼�
        CharacterCreator.Instance.SetProfession(professionType);

        // ���������λ�Ƿ��в�֧�����ְҵ�����
        CharacterPartConfigBase partConfig = GetCurrentCharacterPartConfig(CharacterPartType.Face);
        // TODO:���������Դ��ж������
        if (!partConfig.ProfessionTypes.Contains(professionType))
        {
            // �л�������λ
            SetNextChracterPart(false, CharacterPartType.Face==currentFacadeMenus_Tab.CharacterPartType, CharacterPartType.Face);
        }

        partConfig = GetCurrentCharacterPartConfig(CharacterPartType.Hair);
        if (!partConfig.ProfessionTypes.Contains(professionType))
        {
            // �л�ͷ����λ
            SetNextChracterPart(false, CharacterPartType.Hair == currentFacadeMenus_Tab.CharacterPartType, CharacterPartType.Hair);
        }

        partConfig = GetCurrentCharacterPartConfig(CharacterPartType.Cloth);
        if (!partConfig.ProfessionTypes.Contains(professionType))
        {
            // �л����²�λ
            SetNextChracterPart(false, CharacterPartType.Cloth == currentFacadeMenus_Tab.CharacterPartType, CharacterPartType.Cloth);
        }
    }

    /// <summary>
    /// ѡ����۲˵�
    /// </summary>
    public void SelectFacedeMenusTab(UI_FacadeMenus_Tab newTab)
    {
        if (currentFacadeMenus_Tab!=null)
        {
            currentFacadeMenus_Tab.UnSelect();
        }
        newTab.Select();
        currentFacadeMenus_Tab = newTab;
        int currIndex = characterConfigIndexDic[(int)currentFacadeMenus_Tab.CharacterPartType];
        // ˢ�½���
        SetCharacterPart(currentFacadeMenus_Tab.CharacterPartType, projectConfig.CustomCharacterPartConfigIDDic[currentFacadeMenus_Tab.CharacterPartType][currIndex], true,false);
    }


    /// <summary>
    /// ���þ���Ĳ�λ
    /// </summary>
    public void SetCharacterPart(CharacterPartType partType,int configIndex,bool updateUIView = false,bool updateCharacterView = false)
    {
        // ��ȡ�����ļ� 
        // ��������ļ�����Դ�ͷ�ʱ����Player_View������
        CharacterPartConfigBase partConfig = ConfigTool.LoadCharacterPartConfig(partType, configIndex);

        // ����UI
        if (updateUIView)
        {
            partNameText.text = partConfig.Name;
            switch (partType)
            {
                case CharacterPartType.Face:
                    // �߶�
                    heightSlider.transform.parent.gameObject.SetActive(true);
                    heightSlider.value = customCharacterData.CustomPartDataDic.Dictionary[(int)CharacterPartType.Face].Height;
                    heightSlider.minValue = 0;
                    heightSlider.maxValue = 0.1f;
              
                    // �ߴ�
                    sizeSlider.transform.parent.gameObject.SetActive(true);
                    sizeSlider.value = customCharacterData.CustomPartDataDic.Dictionary[(int)CharacterPartType.Face].Size;
                    sizeSlider.minValue = 0.9f;
                    sizeSlider.maxValue = 1.1f;
            
                    // ������ɫѡ��
                    color1Button.gameObject.SetActive(false);
                    color2Button.gameObject.SetActive(false);
                    break;
                case CharacterPartType.Hair:
                    heightSlider.transform.parent.gameObject.SetActive(false);
                    sizeSlider.transform.parent.gameObject.SetActive(false);
                    color2Button.gameObject.SetActive(false);

                    // �������õ���Ч���������Ƿ�������ɫ
                    if ((partConfig as HairConfig).ColorIndex != -1) 
                    {
                        color1Button.gameObject.SetActive(true);
                        Color color = customCharacterData.CustomPartDataDic.Dictionary[(int)CharacterPartType.Hair].Color1.ConverToUnityColor();
                        // ����ɫ��ť��ͼƬ�͵�ǰ����һ��
                        color1Button.image.color = new Color(color.r, color.g, color.b, 0.6f);
                    }
                    else color1Button.gameObject.SetActive(false);

                    break;
                case CharacterPartType.Cloth:
                    heightSlider.transform.parent.gameObject.SetActive(false);
                    sizeSlider.transform.parent.gameObject.SetActive(false);

                    // �������õ���Ч���������Ƿ�������ɫ
                    if ((partConfig as ClothConfig).ColorIndex1 != -1)
                    {
                        color1Button.gameObject.SetActive(true);
                        Color color = customCharacterData.CustomPartDataDic.Dictionary[(int)CharacterPartType.Cloth].Color1.ConverToUnityColor();
                        // ����ɫ��ť��ͼƬ�͵�ǰ����һ��
                        color1Button.image.color = new Color(color.r, color.g, color.b, 0.6f);
                    }
                    else color1Button.gameObject.SetActive(false);

                    // �������õ���Ч���������Ƿ�������ɫ
                    if ((partConfig as ClothConfig).ColorIndex2 != -1)
                    {
                        color2Button.gameObject.SetActive(true);
                        Color color = customCharacterData.CustomPartDataDic.Dictionary[(int)CharacterPartType.Cloth].Color2.ConverToUnityColor();
                        // ����ɫ��ť��ͼƬ�͵�ǰ����һ��
                        color2Button.image.color = new Color(color.r, color.g, color.b, 0.6f);
                    }
                    else color2Button.gameObject.SetActive(false);
                    break;
            }
        }
        // �ý�ɫ�޸�ģ��
        CharacterCreator.Instance.SetPart(partConfig, updateCharacterView);

        // ��������
        customCharacterData.CustomPartDataDic.Dictionary[(int)partType].Index = configIndex;
    }

    private void LeftArrowButtonClick()
    {
        SetNextChracterPart(true,true, currentFacadeMenus_Tab.CharacterPartType);
    }
    private void RightArrowButtonClick()
    {
        SetNextChracterPart(false, true, currentFacadeMenus_Tab.CharacterPartType);
    }
    private void SetNextChracterPart(bool isLeft,bool updateUI, CharacterPartType currPartType)
    {
        // ��ǰ��ְҵ
        ProfessionType professionType = currentProfessionButton.ProfessionType;
        // ��ǰ����-��projectConfig�еڼ�������
        int currIndex = characterConfigIndexDic[(int)currPartType];
        if (isLeft) currIndex -= 1;
        else currIndex += 1;

        // ����߽��ˣ�ֱ����Ϊ����һ���߽�
        if (currIndex < 0) currIndex = projectConfig.CustomCharacterPartConfigIDDic[currPartType].Count-1;
        else if(currIndex > projectConfig.CustomCharacterPartConfigIDDic[currPartType].Count-1) currIndex = 0;

        // ���ְҵ��Ч��
        // ͨ��currIndex֪����ǰ�ǵڼ������ã�Ȼ��������������ID��ҪȥprojectConfig����ȥ��ȡ
        CharacterPartConfigBase partConfig = ConfigTool.LoadCharacterPartConfig(currPartType, projectConfig.CustomCharacterPartConfigIDDic[currPartType][currIndex]);
        while (!partConfig.ProfessionTypes.Contains(professionType))
        {
            if (isLeft) currIndex -= 1;
            else currIndex += 1;
            // ����߽��ˣ�ֱ����Ϊ����һ���߽�
            if (currIndex < 0) currIndex = projectConfig.CustomCharacterPartConfigIDDic[currPartType].Count-1;
            else if (currIndex > projectConfig.CustomCharacterPartConfigIDDic[currPartType].Count-1) currIndex = 0;
            // �ͷ���Դ
            ResManager.Release<CharacterPartConfigBase>(partConfig);
            partConfig = ConfigTool.LoadCharacterPartConfig(currPartType, projectConfig.CustomCharacterPartConfigIDDic[currPartType][currIndex]);
        }
        // �ͷ���Դ
        ResManager.Release<CharacterPartConfigBase>(partConfig);

        // ������ְҵ��Ч��
        characterConfigIndexDic[(int)currPartType] = currIndex;
        SetCharacterPart(currPartType, projectConfig.CustomCharacterPartConfigIDDic[currPartType][currIndex], updateUI, true);
        AudioManager.Instance.PlayOnShot(arrowClickAudioClip, Vector3.zero, 1);
    }
    private void OnHeightSliderValueChanged(float height)
    {
        GetCharacterPartData().Height = height;
        CharacterCreator.Instance.SetHieght(currentFacadeMenus_Tab.CharacterPartType, height);
    }

    private void OnSizeSliderValueChanged(float size)
    {
        GetCharacterPartData().Size = size;
        CharacterCreator.Instance.SetSize(currentFacadeMenus_Tab.CharacterPartType, size);
    }

    private void Color1ButtonClick()
    {
        // ��ʾ��ɫѡ��������
        UIManager.Instance.Show<UI_ColorSelectorWindow>().Init(OnColor1Seletec, color1Button.image.color);
    }

    private void Color2ButtonClick()
    {
        // ��ʾ��ɫѡ��������
        UIManager.Instance.Show<UI_ColorSelectorWindow>().Init(OnColor2Seletec, color1Button.image.color);
    }

    // ���ȷ���˵�һ����ɫ��ť��ֵ
    private void OnColor1Seletec(Color newColor)
    {
        GetCharacterPartData().Color1 = newColor.ConverToSerializationColor();
        // ������ɫ�Ǳ��޸���ɫ
        CharacterCreator.Instance.SetColor1(currentFacadeMenus_Tab.CharacterPartType, newColor);

        // �޸���ɫ��ť����ɫֵ
        color1Button.image.color = new Color(newColor.r, newColor.g, newColor.b, 0.6f);
    }


    // ���ȷ���˵ڶ�����ɫ��ť��ֵ
    private void OnColor2Seletec(Color newColor)
    {
        GetCharacterPartData().Color2 = newColor.ConverToSerializationColor();

        // ������ɫ�Ǳ��޸���ɫ
        CharacterCreator.Instance.SetColor2(currentFacadeMenus_Tab.CharacterPartType, newColor);

        // �޸���ɫ��ť����ɫֵ
        color2Button.image.color = new Color(newColor.r, newColor.g, newColor.b, 0.6f);
    }

    // ��ȡ��ǰ��ɫ��λ����
    private CharacterPartConfigBase GetCurrentCharacterPartConfig(CharacterPartType currPartType)
    {
        return CharacterCreator.Instance.GetCurrentPartConfig(currPartType);
    }

    // ��ȡ��ǰ��ɫ��λ����
    private CustomCharacterPartData GetCharacterPartData()
    {
        // ȷ����ǰ��ʲô��λ
        CharacterPartType currPartType = currentFacadeMenus_Tab.CharacterPartType;
        // ���ݲ��沿λ����
        CustomCharacterPartData partData = customCharacterData.CustomPartDataDic.Dictionary[(int)currPartType];
        return partData;
    }

    public override void OnClose()
    {
        base.OnClose();
        // �ͷ�������Դ
        ResManager.ReleaseInstance(gameObject);
    }
}
