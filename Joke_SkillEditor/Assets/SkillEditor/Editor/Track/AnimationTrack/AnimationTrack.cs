using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AnimationTrack : SkillTrackBase
{
    public override string MenuAssetPath => "Assets/SkillEditor/Editor/Track/AnimationTrack/AnimationTrackMenu.uxml";
    public override string TrackAssetPath => "Assets/SkillEditor/Editor/Track/AnimationTrack/AnimationTrackContent.uxml";

    private Dictionary<int, AnimationTrackItem> trackItemDic = new Dictionary<int, AnimationTrackItem>();
    public SkillAnimationData AnimationData { get => SkillEditorWindows.Instance.SkillConfig.SkillAnimationData; }

    public override void Init(VisualElement menuParent, VisualElement trackParent, float frameWidth)
    {
        base.Init(menuParent, trackParent, frameWidth);
        track.RegisterCallback<DragUpdatedEvent>(OnDragUpdatedEvent);
        track.RegisterCallback<DragExitedEvent>(OnDragExitedEvent);

        ResetView();
    }

    public override void ResetView(float frameWidth)
    {
        base.ResetView(frameWidth);
        //���ٵ�ǰ����
        foreach (var item in trackItemDic)
        {
            track.Remove(item.Value.root);
        }

        trackItemDic.Clear();
        if (SkillEditorWindows.Instance.SkillConfig == null) return;

        foreach (var item in AnimationData.FrameDataDic)
        {
            AnimationTrackItem trackItem = new AnimationTrackItem();
            trackItem.Init(this, track, item.Key, frameWidth, item.Value);
            trackItemDic.Add(item.Key, trackItem);
        }

        //�������ݻ��� TrackItem

    }

    #region  ��ק��Դ
    private void OnDragUpdatedEvent(DragUpdatedEvent evt)
    {
        UnityEngine.Object[] objs = DragAndDrop.objectReferences;
        AnimationClip clip = objs[0] as AnimationClip;
        if (clip != null)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        }
    }

    private void OnDragExitedEvent(DragExitedEvent evt)
    {
        UnityEngine.Object[] objs = DragAndDrop.objectReferences;
        AnimationClip clip = objs[0] as AnimationClip;
        if (clip != null)
        {
            //���ö�����Դ

            //��ǰѡ�е�֡��λ�� ����Ƿ��ܷ��ö���
            int selectFrameIndex = SkillEditorWindows.Instance.GetFrameIndexByPos(evt.localMousePosition.x);
            bool canPlace = true;
            int durationFrame = -1;//-1 ���������ԭ�� AnimationClip �ĳ���ʱ��
            int clipFrameCount = (int)(clip.length * clip.frameRate);
            int nextTrackItem = -1;
            int currentOffset = int.MaxValue;

            foreach (var item in AnimationData.FrameDataDic)
            {
                //������ѡ��֡�� TrackItem �м䣨�����¼�����㵽�����յ�֮�䣩
                if (selectFrameIndex > item.Key && selectFrameIndex < item.Key + item.Value.DurationFrame)
                {
                    //���ܷ���
                    canPlace = false;
                    break;
                }

                //�ҵ��Ҳ����� TrackItem
                if (item.Key > selectFrameIndex)
                {
                    int tempOffset = item.Key - selectFrameIndex;
                    if (tempOffset < currentOffset)
                    {
                        currentOffset = tempOffset;
                        nextTrackItem = item.Key;
                    }
                }
            }

            //ʵ�ʵķ���
            if (canPlace)
            {
                // �ұ������� TrackItem ��Ҫ���� Track �����ص�������
                if (nextTrackItem != -1)
                {
                    int offset = clipFrameCount - currentOffset;
                    durationFrame = offset < 0 ? clipFrameCount : currentOffset; //��������ռ��ܲ�������������Ƭ�ηŽ�ȥ
                }
                else
                {
                    //�Ҳ�ɶ��û��
                    durationFrame = clipFrameCount;
                }

                //������������
                SkillAnimationEvent animationEvent = new SkillAnimationEvent()
                {
                    AnimationClip = clip,
                    DurationFrame = durationFrame,
                    TransitionTime = 0.25f
                };

                //���������Ķ�������
                AnimationData.FrameDataDic.Add(selectFrameIndex, animationEvent);
                SkillEditorWindows.Instance.SaveConfig();

                //ͬ���޸ı༭����ͼ
                ResetView();
            }
        }
    }

    #endregion

    public bool CheckFrameIndexOnDrag(int targetindex, int selfIndex, bool isLeft)
    {
        foreach (var item in AnimationData.FrameDataDic)
        {
            //��קʱ���������
            if (item.Key == selfIndex) continue;

            //�����ƶ�&&ԭ�����ұ�&&Ŀ��û���ص�
            if (isLeft && selfIndex > item.Key && targetindex < item.Key + item.Value.DurationFrame)
            {
                return false;
            }
            //�����ƶ�&&ԭ�������&&Ŀ��û���ص�
            else if (!isLeft && selfIndex < item.Key && targetindex > item.Key)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// �� oldIndex �����ݱ�Ϊ newIndex
    /// </summary>
    public void SetFrameIndex(int oldIndex, int newIndex)
    {
        if (AnimationData.FrameDataDic.Remove(oldIndex, out SkillAnimationEvent animationEvent))
        {
            AnimationData.FrameDataDic.Add(newIndex, animationEvent);
            SkillEditorWindows.Instance.SaveConfig();
        }
    }

    public override void DeleteTrackItem(int frameIndex)
    {
        //�Ƴ�����
        AnimationData.FrameDataDic.Remove(frameIndex);
        if (trackItemDic.Remove(frameIndex, out AnimationTrackItem item))
        {
            //�Ƴ���ͼ
            track.Remove(item.root);
        }
        SkillEditorWindows.Instance.SaveConfig();
    }

    public override void OnConfigChanged()
    {
        foreach (var item in trackItemDic.Values)
        {
            item.OnConfigChanged();
        }
    }

}
