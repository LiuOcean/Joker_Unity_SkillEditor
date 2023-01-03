using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillSingleLineTrackStyle : SkillTrackStyleBase
{
    private const string MenuAssetPath = "Assets/SkillEditor/Editor/Track/Assets/SingleLineTrackStyle/SingleLineTrackMenu.uxml";
    private const string TrackAssetPath = "Assets/SkillEditor/Editor/Track/Assets/SingleLineTrackStyle/SingleLineTrackContent.uxml";

    public void Init(VisualElement menuParent, VisualElement contentParent, string title)
    {
        this.menuParent = menuParent;
        this.contentParent = contentParent;

        menuRoot = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(MenuAssetPath).Instantiate().Query().ToList()[1];//��Ҫ������ֱ�ӳ���Ŀ������
        contentRoot = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(TrackAssetPath).Instantiate().Query().ToList()[1];//��Ҫ������ֱ�ӳ���Ŀ������
        menuParent.Add(menuRoot);
        contentParent.Add(contentRoot);

        titleLabel = (Label)menuRoot;
        titleLabel.text = title;
    }











}
