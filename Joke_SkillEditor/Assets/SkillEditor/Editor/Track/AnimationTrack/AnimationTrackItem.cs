using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AnimationTrackItem : TrackItemBase
{
    private const string trackItemAssetPath = "Assets/SkillEditor/Editor/Track/AnimationTrack/AnimationTrackItem.uxml";
    private AnimationTrack animationTrack;
    private int frameIndex;
    private float frameUnitWidth;
    private SkillAnimationEvent animationEvent;

    private VisualElement mainDragArea;
    private VisualElement animationOverLine;

    public Label root { get; private set; }

    public void Init(AnimationTrack animationTrack, VisualElement parent, int startFrameIndex, float frameUnitWidth, SkillAnimationEvent animationEvent)
    {
        this.animationTrack = animationTrack;
        this.frameIndex = startFrameIndex;
        this.frameUnitWidth = frameUnitWidth;
        this.animationEvent = animationEvent;

        root = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(trackItemAssetPath).Instantiate().Query<Label>();//��Ҫ������ֱ�ӳ���Ŀ������
        mainDragArea = root.Q<VisualElement>("Main");
        animationOverLine = root.Q<VisualElement>("OverLine");
        parent.Add(root);

        RestView(frameUnitWidth);
    }

    public void RestView(float frameUnitWidth)
    {
        this.frameUnitWidth = frameUnitWidth;
        root.text = animationEvent.AnimationClip.name;

        //λ�ü���
        Vector3 mainPos = root.transform.position;
        mainPos.x = frameIndex * frameUnitWidth;
        root.transform.position = mainPos;
        root.style.width = animationEvent.DurationFrame * frameUnitWidth;

        //���㶯�������ߵ�λ��
        int animationClipFrameCount = (int)(animationEvent.AnimationClip.length * animationEvent.AnimationClip.frameRate);
        if (animationClipFrameCount > animationEvent.DurationFrame)
        {
            animationOverLine.style.display = DisplayStyle.None;
        }
        else
        {
            animationOverLine.style.display = DisplayStyle.Flex;
            Vector3 overLinePos = animationOverLine.transform.position;
            //overLinePos.x = animationClipFrameCount * frameUnitWidth - animationOverLine.style.width.value.value / 2;
            overLinePos.x = animationClipFrameCount * frameUnitWidth - 1; //�������Ϊ2��ȡһ��
            animationOverLine.transform.position = overLinePos;
        }

    }


}
