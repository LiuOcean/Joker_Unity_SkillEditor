using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using System;

[CustomEditor(typeof(SkillEditorWindows))]
public class SkillEditorInspector : Editor
{
    public static SkillEditorInspector Instance;
    private static TrackItemBase currentTrackItem;
    private static SkillTrackBase currentTrack;

    private VisualElement root;


    public static void SetTrackItem(TrackItemBase trackItem, SkillTrackBase track)
    {
        if (currentTrackItem != null)
        {
            currentTrackItem.OnUnSelect();
        }

        currentTrackItem = trackItem;
        currentTrackItem.OnSelect();
        currentTrack = track;

        //�����Ѿ���Inspector�����µ����ˢ�²���ʱ
        if (Instance != null) Instance.Show();
    }

    private void OnDestroy()
    {
        //˵������ж��
        if (currentTrackItem != null)
        {
            currentTrackItem.OnUnSelect();
            currentTrackItem = null;
            currentTrack = null;
        }
    }


    public override VisualElement CreateInspectorGUI()
    {
        Instance = this;
        root = new VisualElement();
        //root.Add(new Label("AAAA"));

        Show();
        return root;
    }


    private void Show()
    {
        Clean();
        if (currentTrackItem == null) return;

        //Ŀǰֻ�ж�����һ�����
        if (currentTrackItem.GetType() == typeof(AnimationTrackItem))
        {
            DrawAnimationTrackItem((AnimationTrackItem)currentTrackItem);
        }

    }

    private void Clean()
    {
        if (root != null)
        {
            for (int i = root.childCount - 1; i >= 0; i--)
            {
                root.RemoveAt(i);
            }
        }
    }

    private int trackItemFrameIndex; //�����Ӧ��֡����
    public void SetTrackItemFrameIndex(int trackItemFrameIndex)
    {
        this.trackItemFrameIndex = trackItemFrameIndex;
    }

    #region �������
    private Label clipFrameLabel;
    private Label isLoopLable;
    private IntegerField durationField;
    private FloatField transitionTimeField;

    private void DrawAnimationTrackItem(AnimationTrackItem animationTrackItem)
    {
        trackItemFrameIndex = animationTrackItem.FrameIndex;

        //������Դ
        ObjectField animationClipAssetField = new ObjectField("������Դ");
        animationClipAssetField.objectType = typeof(AnimationClip);
        animationClipAssetField.value = animationTrackItem.AnimationEvent.AnimationClip;
        animationClipAssetField.RegisterValueChangedCallback(AnimationClipValueChangedCallback);
        root.Add(animationClipAssetField);

        //�������
        durationField = new IntegerField("�������");
        durationField.value = animationTrackItem.AnimationEvent.DurationFrame;
        durationField.RegisterValueChangedCallback(DurtionFieldValueChangedCallback);
        root.Add(durationField);

        //����ʱ��
        transitionTimeField = new FloatField("����ʱ��");
        transitionTimeField.value = animationTrackItem.AnimationEvent.TransitionTime;
        transitionTimeField.RegisterValueChangedCallback(TransitionTimeFieldValueChangedCallback);
        root.Add(transitionTimeField);

        //������ص���Ϣ
        int clipFrameCount = (int)(animationTrackItem.AnimationEvent.AnimationClip.length * animationTrackItem.AnimationEvent.AnimationClip.frameRate);
        clipFrameLabel = new Label("������Դ���ȣ�" + clipFrameCount);
        root.Add(clipFrameLabel);

        isLoopLable = new Label("ѭ��������" + animationTrackItem.AnimationEvent.AnimationClip.isLooping);
        root.Add(isLoopLable);

        //ɾ��
        Button deleteButton = new Button(DeleteButtonClick);
        deleteButton.text = "ɾ��";
        deleteButton.style.backgroundColor = new Color(1, 0, 0, 0.5f);
        root.Add(deleteButton);
    }

    private void AnimationClipValueChangedCallback(ChangeEvent<UnityEngine.Object> evt)
    {
        if (evt.previousValue == evt.newValue) return;
        AnimationClip clip = evt.newValue as AnimationClip;

        //�޸�������ʾЧ��
        clipFrameLabel.text = "������Դ���ȣ�" + ((int)(clip.length * clip.frameRate));
        isLoopLable.text = "ѭ��������" + clip.isLooping;

        //���浽����
        SkillEditorWindows.Instance.SkillConfig.SkillAnimationData.FrameDataDic[trackItemFrameIndex].AnimationClip = clip;
        SkillEditorWindows.Instance.SaveConfig();
        currentTrackItem.ResetView();
    }

    private void DurtionFieldValueChangedCallback(ChangeEvent<int> evt)
    {
        if (evt.previousValue == evt.newValue) return;
        int value = evt.newValue;

        //��ȫУ��
        if ((currentTrack as AnimationTrack).CheckFrameIndexOnDrag(trackItemFrameIndex + value, trackItemFrameIndex, false))
        {
            //�޸����ݣ�ˢ����ͼ
            SkillEditorWindows.Instance.SkillConfig.SkillAnimationData.FrameDataDic[trackItemFrameIndex].DurationFrame = value;
            (currentTrackItem as AnimationTrackItem).CheckFrameCount();
            SkillEditorWindows.Instance.SaveConfig();//ע��Ҫ��󱣴棬��Ȼ�¾����ݻ�Բ���
            currentTrackItem.ResetView();
        }
        else
        {
            durationField.value = evt.previousValue;
        }
    }

    private void TransitionTimeFieldValueChangedCallback(ChangeEvent<float> evt)
    {
        if (evt.previousValue == evt.newValue) return;
        SkillEditorWindows.Instance.SkillConfig.SkillAnimationData.FrameDataDic[trackItemFrameIndex].TransitionTime = evt.newValue;
        SkillEditorWindows.Instance.SaveConfig();
        currentTrack.ResetView();
    }

    private void DeleteButtonClick()
    {
        currentTrack.DeleteTrackItem(trackItemFrameIndex); //�˺����ṩ���ݱ����ˢ����ͼ���߼�
        Selection.activeObject = null;
    }

    #endregion


}
