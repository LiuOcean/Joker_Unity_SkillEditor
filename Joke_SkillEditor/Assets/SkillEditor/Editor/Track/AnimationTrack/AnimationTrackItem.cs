using System;
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
    public int FrameIndex { get => frameIndex; }

    private float frameUnitWidth;
    private SkillAnimationEvent animationEvent;
    public SkillAnimationEvent AnimationEvent { get => animationEvent; }

    public Label root { get; private set; }
    private VisualElement mainDragArea;
    private VisualElement animationOverLine;


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

        //���¼�
        mainDragArea.RegisterCallback<MouseDownEvent>(OnMouseDownEvent);
        mainDragArea.RegisterCallback<MouseUpEvent>(OnMouseUpEvent);
        mainDragArea.RegisterCallback<MouseOutEvent>(OnMouseOutEvent);
        mainDragArea.RegisterCallback<MouseMoveEvent>(OnMouseMoveEvent);

        ResetView(frameUnitWidth);
    }

    public void ResetView(float frameUnitWidth)
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


    #region  ��꽻��

    private static Color normalColor = new Color(0.388f, 0.850f, 0.905f, 0.5f);
    private static Color selectColor = new Color(0.388f, 0.850f, 0.905f, 1f);
    private bool mouseDrag = false;
    private float startDragPosX;
    private int startDragFrameIndex;

    private void OnMouseDownEvent(MouseDownEvent evt)
    {
        root.style.backgroundColor = selectColor;
        startDragPosX = evt.mousePosition.x;
        startDragFrameIndex = frameIndex;
        mouseDrag = true;

        SkillEditorWindows.Instance.ShowTrackItemOnInspector(this, animationTrack);
    }

    private void OnMouseUpEvent(MouseUpEvent evt)
    {
        if (mouseDrag) ApplyDrag();
        mouseDrag = false;
    }

    private void OnMouseOutEvent(MouseOutEvent evt)
    {
        root.style.backgroundColor = normalColor;
        if (mouseDrag) ApplyDrag();
        mouseDrag = false;
    }

    private void OnMouseMoveEvent(MouseMoveEvent evt)
    {
        if (mouseDrag)
        {
            float offsetPos = evt.mousePosition.x - startDragPosX;
            int offsetFrame = Mathf.RoundToInt(offsetPos / frameUnitWidth);
            int targetFrameIndex = startDragFrameIndex + offsetFrame;
            bool checkDrag = false;

            if (targetFrameIndex < 0) return; //��������ק�����������

            if (offsetFrame < 0)
            {
                checkDrag = animationTrack.CheckFrameIndexOnDrag(targetFrameIndex);
            }
            else if (offsetFrame > 0)
            {
                checkDrag = animationTrack.CheckFrameIndexOnDrag(targetFrameIndex + animationEvent.DurationFrame);
            }
            else return;

            if (checkDrag)
            {
                //ȷ���޸ĵ�����
                frameIndex = targetFrameIndex;

                //��������Ҳ�߽磬��չ�߽�
                if (frameIndex + animationEvent.DurationFrame > SkillEditorWindows.Instance.SkillConfig.FrameCount)
                {
                    SkillEditorWindows.Instance.CurrentFrameCount = frameIndex + animationEvent.DurationFrame;
                }

                //ˢ����ͼ
                ResetView(frameUnitWidth);
            }
        }
    }

    private void ApplyDrag()
    {
        if (startDragFrameIndex != frameIndex)
        {
            animationTrack.SetFrameIndex(startDragFrameIndex, frameIndex);
        }
    }

    #endregion

}
