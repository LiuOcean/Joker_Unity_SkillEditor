using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class SkillTrackBase
{
    protected float frameWidth;
    protected VisualElement menuParent;
    protected VisualElement trackParent;
    protected VisualElement menu;
    protected VisualElement track;

    public abstract string MenuAssetPath { get; }
    public abstract string TrackAssetPath { get; }

    public virtual void Init(VisualElement menuParent, VisualElement trackParent, float frameWidth)
    {
        this.menuParent = menuParent;
        this.trackParent = trackParent;
        this.frameWidth = frameWidth;

        menu = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(MenuAssetPath).Instantiate().Query().ToList()[1];//��Ҫ������ֱ�ӳ���Ŀ������
        track = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(TrackAssetPath).Instantiate().Query().ToList()[1];//��Ҫ������ֱ�ӳ���Ŀ������
        menuParent.Add(menu);
        trackParent.Add(track);
    }

    /// <summary>
    /// �ڵ�ǰ��߽���ˢ�£��ڲ��仯��
    /// </summary>
    public virtual void ResetView()
    {
        ResetView(frameWidth);
    }

    /// <summary>
    /// ����б仯��ˢ�£����ֻ�����
    /// </summary>
    /// <param name="frameWidth"></param>
    public virtual void ResetView(float frameWidth)
    {
        this.frameWidth = frameWidth;
    }

    public virtual void DeleteTrackItem(int frameIndex)
    {

    }

}
