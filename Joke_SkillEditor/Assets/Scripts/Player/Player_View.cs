using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKFrame;
/// <summary>
/// �����ͼ
/// </summary>
public class Player_View : MonoBehaviour
{
    [SerializeField] new Animation_Controller animation;
    public Animation_Controller Animation { get => animation; }

    [SerializeField] SkinnedMeshRenderer[] partSkinnedMeshRenderers;    // ��λ����Ⱦ��
    [SerializeField] Material[] partMaterials;                          // ��λ�Ĳ�����Դ
    [SerializeField] Transform neckRootTransform;                       // ͷ���ĸ��ڵ�
    private CustomCharacterData customCharacterData;                    // ����Զ���Ľ�ɫ����-���ڴ浵
    private Dictionary<int, CharacterPartConfigBase> characterPartDic = new Dictionary<int, CharacterPartConfigBase>(); // ��ɫ��λ�ֵ�
    /// <summary>
    /// ��ʼ��
    /// </summary>
    public void Init(CustomCharacterData customCharacterData)
    {
        animation.Init();
        // ��ÿһ����λ�Ĳ��ʶ�ʵ����һ���Լ��Ĳ����򣬻�������
        partSkinnedMeshRenderers[0].material = Instantiate(partMaterials[0]);
        partSkinnedMeshRenderers[1].material = Instantiate(partMaterials[0]);
        partSkinnedMeshRenderers[2].material = Instantiate(partMaterials[2]);
        this.customCharacterData = customCharacterData;
    }

    /// <summary>
    /// ��Ϸ�еĳ�ʼ��
    /// </summary>
    public void InitOnGame(CustomCharacterData customCharacterData)
    {
        Init(customCharacterData);
        // �����������õ�ǰ��λ
        CharacterPartConfigBase faceConfig = ConfigTool.LoadCharacterPartConfig(CharacterPartType.Face, customCharacterData.CustomPartDataDic.Dictionary[(int)CharacterPartType.Face].Index);
        CharacterPartConfigBase clothCofig = ConfigTool.LoadCharacterPartConfig(CharacterPartType.Cloth, customCharacterData.CustomPartDataDic.Dictionary[(int)CharacterPartType.Cloth].Index);
        CharacterPartConfigBase hairConfig = ConfigTool.LoadCharacterPartConfig(CharacterPartType.Hair, customCharacterData.CustomPartDataDic.Dictionary[(int)CharacterPartType.Hair].Index);
        CustomCharacterPartData facePartData = customCharacterData.CustomPartDataDic.Dictionary[(int)CharacterPartType.Face];
        
        SetPart(faceConfig,true);
        SetPart(clothCofig, true);
        SetPart(hairConfig, true);

        SetSize(CharacterPartType.Face, facePartData.Size);
        SetHeight(CharacterPartType.Face, facePartData.Height);
    }

    /// <summary>
    /// ��ȡ��ǰ�Ľ�ɫ����
    /// </summary>
    public CharacterPartConfigBase GetCurrentPartConfig(CharacterPartType characterPartType)
    {
        if (characterPartDic.TryGetValue((int)characterPartType, out CharacterPartConfigBase characterPartConfig))
        {
            return characterPartConfig;
        }
        return null;
    }

    /// <summary>
    /// ���ò�λ
    /// </summary>
    public void SetPart(CharacterPartConfigBase characterPartConfig, bool updateCharacterView = true)
    {
        if (characterPartDic.TryGetValue((int)characterPartConfig.CharacterPartType,out CharacterPartConfigBase currPartConfig))
        {
            // �ͷž����õ���Դ
            ResManager.Release<CharacterPartConfigBase>(currPartConfig);
            characterPartDic[(int)characterPartConfig.CharacterPartType] = characterPartConfig;
        }
        else
        {
            // �����λ֮ǰ�ǿյģ����Բ�������Դ�ͷ�����
            characterPartDic.Add((int)characterPartConfig.CharacterPartType, characterPartConfig);
        }

        // ������ʵ�ʵĻ���
        if (!updateCharacterView) return;

        switch (characterPartConfig.CharacterPartType)
        {
            case CharacterPartType.Face:
                partSkinnedMeshRenderers[0].sharedMesh = characterPartConfig.Mesh1;
                break;
            case CharacterPartType.Hair:
                HairConfig hairConfig = characterPartConfig as HairConfig;
                partSkinnedMeshRenderers[1].sharedMesh = hairConfig.Mesh1;
                SetColor1(CharacterPartType.Hair, customCharacterData.CustomPartDataDic.Dictionary[(int)CharacterPartType.Hair].Color1.ConverToUnityColor());
                break;
            case CharacterPartType.Cloth:
                ClothConfig clothConfig = characterPartConfig as ClothConfig;
                partSkinnedMeshRenderers[2].sharedMesh = clothConfig.Mesh1;
                SetColor1(CharacterPartType.Cloth, customCharacterData.CustomPartDataDic.Dictionary[(int)CharacterPartType.Cloth].Color1.ConverToUnityColor());
                SetColor2(CharacterPartType.Cloth, customCharacterData.CustomPartDataDic.Dictionary[(int)CharacterPartType.Cloth].Color2.ConverToUnityColor());
                break;
        }
    }

    /// <summary>
    /// ������ɫ1
    /// </summary>
    public void SetColor1(CharacterPartType characterPartType, Color color)
    {
        CharacterPartConfigBase partConfig = GetCurrentPartConfig(characterPartType);
        // ���ݲ�ͬ�Ĳ�λ���ͣ�ȷ��������Ҫ�Ĳ������е���һ����ɫ
        switch (characterPartType)
        {
            case CharacterPartType.Hair:
                HairConfig hairConfig = partConfig as HairConfig;
                if (hairConfig.ColorIndex >= 0)
                {
                    partSkinnedMeshRenderers[1].sharedMaterial.SetColor("_Color0" + (hairConfig.ColorIndex + 1), color);
                }
                break;
            case CharacterPartType.Cloth:
                ClothConfig clothConfig = partConfig as ClothConfig;
                if (clothConfig.ColorIndex1 >= 0)
                {
                    partSkinnedMeshRenderers[2].sharedMaterial.SetColor("_Color0" + (clothConfig.ColorIndex1 + 1), color);
                }
                break;
        }
    }
    /// <summary>
    /// ������ɫ2
    /// </summary>
    public void SetColor2(CharacterPartType characterPartType, Color color)
    {
        CharacterPartConfigBase partConfig = GetCurrentPartConfig(characterPartType);
        // ���ݲ�ͬ�Ĳ�λ���ͣ�ȷ��������Ҫ�Ĳ������е���һ����ɫ
        switch (characterPartType)
        {
            case CharacterPartType.Cloth:
                ClothConfig clothConfig = partConfig as ClothConfig;
                if (clothConfig.ColorIndex2>=0)
                {
                    partSkinnedMeshRenderers[2].sharedMaterial.SetColor("_Color0" + (clothConfig.ColorIndex2 + 1), color);
                }
                break;
        }
    }
    /// <summary>
    /// ����ĳ����λ�ĳߴ�
    /// </summary>
    public void SetSize(CharacterPartType characterPartType, float size)
    {
        if (characterPartType == CharacterPartType.Face)
        {
            neckRootTransform.localScale = Vector3.one * size;
        }
    }

    /// <summary>
    /// ����ĳ����λ�ĸ߶�
    /// </summary>
    public void SetHeight(CharacterPartType characterPartType, float height)
    {
        if (characterPartType == CharacterPartType.Face)
        {
            neckRootTransform.localPosition = new Vector3(-height, 0, 0);
        }
    }

    private void OnDestroy()
    {
        // �ͷ�ȫ����Դ
        foreach (var item in characterPartDic)
        {
            ResManager.Release(item.Value);
        }
    }
}

