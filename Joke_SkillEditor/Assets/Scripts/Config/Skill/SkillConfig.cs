using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKFrame;
using Sirenix.OdinInspector;
using System;
using Sirenix.Serialization;

[CreateAssetMenu(menuName = "Config/SkillConfig", fileName = "SkillConfig")]
public class SkillConfig : ConfigBase
{
    [LabelText("��������")] public string SkillName;
    [LabelText("֡������")] public int FrameCount = 100;

    [NonSerialized, OdinSerialize]
    public SkillAnimationData SkillAnimationData = new SkillAnimationData();
}

/// <summary>
/// ���ܶ�������
/// </summary>
[Serializable]
public class SkillAnimationData
{
    /// <summary>
    /// ����֡�¼�
    /// key:֡��
    /// value���¼�����
    /// </summary>
    [NonSerialized, OdinSerialize]//��ͨ��Unity�����л�����Odin �����л�
    [DictionaryDrawerSettings(KeyLabel = "֡��", ValueLabel = "��������")]
    public Dictionary<int, SkillAnimationEvent> FrameDataDic = new Dictionary<int, SkillAnimationEvent>();
}

/// <summary>
/// ֡�¼�����
/// </summary>
[Serializable]
public abstract class SkillFrameEventBase
{

}

public class SkillAnimationEvent : SkillFrameEventBase
{
    public AnimationClip AnimationClip;
    public float TransitionTime = 0.25f;

#if UNITY_EDITOR
    public int DurationFrame;

#endif

}