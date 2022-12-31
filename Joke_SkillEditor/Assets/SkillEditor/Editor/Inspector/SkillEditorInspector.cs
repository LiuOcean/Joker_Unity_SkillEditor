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
            for (int i = 0; i < root.childCount; i++)
            {
                root.RemoveAt(i);
            }
        }
    }


    private void DrawAnimationTrackItem(AnimationTrackItem animationTrackItem)
    {
        ObjectField animationClipAssetField = new ObjectField("������Դ");
        animationClipAssetField.objectType = typeof(AnimationClip);
        animationClipAssetField.value = animationTrackItem.AnimationEvent.AnimationClip;
        root.Add(animationClipAssetField);

    }

}
