using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillMultiLineTrackStyle : SkillTrackStyleBase
{
    private const string menuAssetPath = "Assets/SkillEditor/Editor/Track/Assets/MultiLineTrackStyle/MultiLineTrackMenu.uxml";
    private const string trackAssetPath = "Assets/SkillEditor/Editor/Track/Assets/SingleLineTrackStyle/SingleLineTrackContent.uxml";
    private Func<bool> addChildTrackFunc;

    private VisualElement menuItemParent;//�ӹ���Ĳ˵�������


    public void Init(VisualElement menuParent, VisualElement contentParent, string title, Func<bool> addChildTrackFunc)
    {
        this.menuParent = menuParent;
        this.contentParent = contentParent;
        this.addChildTrackFunc = addChildTrackFunc;

        menuRoot = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(menuAssetPath).Instantiate().Query().ToList()[1];//��Ҫ������ֱ�ӳ���Ŀ������
        //contentRoot = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(trackAssetPath).Instantiate().Query().ToList()[1];//��Ҫ������ֱ�ӳ���Ŀ������
        menuParent.Add(menuRoot);
        //contentParent.Add(contentRoot);

        titleLabel = menuRoot.Q<Label>("Title");
        titleLabel.text = title;

        menuItemParent = menuRoot.Q<VisualElement>("TrackMenuList");

        //����ӹ���İ�ť
        Button addButton = menuRoot.Q<Button>("AddButton");
        addButton.clicked += AddButtonClicked;
    }

    //����ӹ��
    private void AddButtonClicked()
    {
        if (addChildTrackFunc == null) return;

        //���ϼ������������ж��ܲ������
        if (addChildTrackFunc())
        {
            ChildTrack childTrack = new ChildTrack();
            childTrack.Init(menuParent, null);
        }

    }


    /// <summary>
    /// ���й�����ӹ��
    /// </summary>
    public class ChildTrack
    {
        private const string menuItemAssetPath = "Assets/SkillEditor/Editor/Track/Assets/MultiLineTrackStyle/MultiLineTrackMenuItem.uxml";
        private const string trackItemAssetPath = "Assets/SkillEditor/Editor/Track/Assets/SingleLineTrackStyle/SingleLineTrackContent.uxml";

        public Label titleLabel;
        #region ������ڵ㣨���Լ���
        public VisualElement menuRoot;
        public VisualElement contentRoot;
        #endregion
        #region �����ڵ㣨�ŵ�˭���棩
        public VisualElement menuParent;
        public VisualElement trackParent;
        #endregion

        public void Init(VisualElement menuParent, VisualElement trackParent)
        {
            this.menuParent = menuParent;
            this.trackParent = trackParent;

            menuRoot = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(menuItemAssetPath).Instantiate().Query().ToList()[1];//��Ҫ������ֱ�ӳ���Ŀ������
            menuParent.Add(menuRoot);                                                                                             //contentRoot = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(trackAssetPath).Instantiate().Query().ToList()[1];//��Ҫ������ֱ�ӳ���Ŀ������

        }

        public virtual void AddItem(VisualElement ve)
        {
            contentRoot.Add(ve);
        }

        public virtual void DeleteItem(VisualElement ve)
        {
            contentRoot.Remove(ve);
        }

        public virtual void Destory()
        {
            if (menuRoot != null) menuParent.Remove(menuRoot);
            if (contentRoot != null) trackParent.Remove(contentRoot);
        }










    }

}
