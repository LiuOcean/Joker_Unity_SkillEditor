using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// �Զ���ɫ��ȫ������
/// </summary>
[Serializable]
public class CustomCharacterData
{
    public Serialization_Dic<int, CustomCharacterPartData> CustomPartDataDic;
}

/// <summary>
/// �Զ����ɫ��λ������
/// </summary>
[Serializable]
public class CustomCharacterPartData
{
    public int Index;
    public float Size;
    public float Height;
    public Serialization_Color Color1;
    public Serialization_Color Color2;
}

