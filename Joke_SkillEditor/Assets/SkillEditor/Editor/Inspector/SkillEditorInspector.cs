using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

[CustomEditor(typeof(SkillEditorWindows))]
public class SkillEditorInspector : Editor
{
    public static SkillEditorInspector Instance;
    private static TrackItemBase currentTrackItem;

    private VisualElement root;


    public static void SetTrackItem(TrackItemBase trackItem)
    {
        currentTrackItem = trackItem;
        if (Instance != null)
        {
            Instance.Show();  //�����Ѿ���Inspector�����µ����ˢ�²���ʱ
        }
    }

    public override VisualElement CreateInspectorGUI()
    {
        Instance = this;
        root = new VisualElement();
        root.Add(new Label("AAAA"));

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


    private void DrawAnimationTrackItem(AnimationTrackItem animationTrackItem)
    {
        //������Դ
        ObjectField animationClipAssetField = new ObjectField("������Դ");
        animationClipAssetField.objectType = typeof(AnimationClip);
        animationClipAssetField.value = animationTrackItem.AnimationEvent.AnimationClip;
        root.Add(animationClipAssetField);

        //�������
        IntegerField duration = new IntegerField("�������");
        duration.value = animationTrackItem.AnimationEvent.DurationFrame;
        root.Add(duration);

        //����ʱ��
        FloatField transitionTime = new FloatField("����ʱ��");
        transitionTime.value = animationTrackItem.AnimationEvent.TransitionTime;
        root.Add(transitionTime);

        //������ص���Ϣ
        int clipFrameCount = (int)(animationTrackItem.AnimationEvent.AnimationClip.length * animationTrackItem.AnimationEvent.AnimationClip.frameRate);
        Label clipFrame = new Label("������Դ���ȣ�" + clipFrameCount);
        root.Add(clipFrame);

        Label isLoopLable = new Label("ѭ��������" + animationTrackItem.AnimationEvent.AnimationClip.isLooping);
        root.Add(isLoopLable);

        //ɾ��
        Button deleteButton = new Button();
        deleteButton.text = "ɾ��";
        deleteButton.style.backgroundColor = new Color(1, 0, 0, 0.5f);
        root.Add(deleteButton);



    }

}
