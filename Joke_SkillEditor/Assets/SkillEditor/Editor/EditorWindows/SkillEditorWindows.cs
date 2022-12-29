using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.SceneManagement;
using System;

public class SkillEditorWindows : EditorWindow
{
    [MenuItem("SkillEditor/SkillEditorWindows")]
    public static void ShowExample()
    {
        SkillEditorWindows wnd = GetWindow<SkillEditorWindows>();
        wnd.titleContent = new GUIContent("���ܱ༭��");
    }

    private VisualElement root;
    public void CreateGUI()
    {
        root = rootVisualElement;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/SkillEditor/Editor/EditorWindows/SkillEditorWindows.uxml");
        VisualElement labelFromUXML = visualTree.Instantiate();
        root.Add(labelFromUXML);

        InitTopMenu();
        InitTimeShaft();
    }


    #region TopMenu 
    private const string skillEditorScenePath = "Assets/SkillEditor/SkillEditorScene.unity";
    private const string PreviewCharacterParentPath = "PreviewCharacterRoot";
    private string oldScenePath;

    private Button LoadEditorSceneButton;
    private Button LoadOldSceneButton;
    private Button SkillBasicButton;

    private ObjectField PreviewCharacterPrefabObjectField;
    private ObjectField SkillConfigObjectField;
    private GameObject currentPreviewCharacterObj;

    private void InitTopMenu()
    {
        LoadEditorSceneButton = root.Q<Button>(nameof(LoadEditorSceneButton));
        LoadOldSceneButton = root.Q<Button>(nameof(LoadOldSceneButton));
        SkillBasicButton = root.Q<Button>(nameof(SkillBasicButton));

        PreviewCharacterPrefabObjectField = root.Q<ObjectField>(nameof(PreviewCharacterPrefabObjectField));
        SkillConfigObjectField = root.Q<ObjectField>(nameof(SkillConfigObjectField));

        LoadEditorSceneButton.clicked += LoadEditorSceneButtonClick;
        LoadOldSceneButton.clicked += LoadOldSceneButtonClick;
        SkillBasicButton.clicked += SkillBasicButtonClick;

        PreviewCharacterPrefabObjectField.RegisterValueChangedCallback(PreviewCharacterPrefabObjectFieldChanged);
        SkillConfigObjectField.RegisterValueChangedCallback(SkillConfigObjectFieldChanged);
    }



    /// <summary>
    /// ���ر༭������
    /// </summary>
    private void LoadEditorSceneButtonClick()
    {
        string currentpath = EditorSceneManager.GetActiveScene().path;
        if (currentpath == skillEditorScenePath) return;

        oldScenePath = currentpath;
        EditorSceneManager.OpenScene(skillEditorScenePath);
    }

    /// <summary>
    /// �ع�ɳ���
    /// </summary>
    private void LoadOldSceneButtonClick()
    {
        if (!string.IsNullOrEmpty(oldScenePath))
        {
            string currentpath = EditorSceneManager.GetActiveScene().path;
            if (currentpath == oldScenePath) return;

            EditorSceneManager.OpenScene(oldScenePath);
        }
        else
        {
            Debug.LogWarning("����������");
        }
    }

    /// <summary>
    /// �鿴���ܻ�����Ϣ
    /// </summary>
    private void SkillBasicButtonClick()
    {
        if (skillConfig != null)
        {
            Selection.activeObject = skillConfig;
        }
    }

    /// <summary>
    /// ��ɫԤ�����޸�
    /// </summary>
    /// <param name="evt"></param>
    private void PreviewCharacterPrefabObjectFieldChanged(ChangeEvent<UnityEngine.Object> evt)
    {
        string currentpath = EditorSceneManager.GetActiveScene().path;
        if (currentpath != skillEditorScenePath) return;

        //���پɵ�
        if (currentPreviewCharacterObj != null) DestroyImmediate(currentPreviewCharacterObj);


        Transform parent = GameObject.Find(PreviewCharacterParentPath).transform;
        if (parent != null && parent.childCount > 0)
        {
            DestroyImmediate(parent.GetChild(0).gameObject);
        }

        if (evt.newValue != null)
        {
            currentPreviewCharacterObj = Instantiate(evt.newValue as GameObject, Vector3.zero, Quaternion.identity, parent);
            currentPreviewCharacterObj.transform.localEulerAngles = Vector3.zero;
        }

    }

    /// <summary>
    /// ���������޸�
    /// </summary>
    /// <param name="evt"></param>
    private void SkillConfigObjectFieldChanged(ChangeEvent<UnityEngine.Object> evt)
    {
        skillConfig = evt.newValue as SkillConfig;
    }

    #endregion Config

    #region TimeShaft
    private IMGUIContainer timeShaft;//ʱ��������
    private VisualElement contentContainer;// ScrollView ����,����ó�  ScrollView ��������ק�ĳߴ����� 
    /// <summary>
    /// ��ǰ���������ƫ������
    /// </summary>
    private float contentOffsetPos { get => Mathf.Abs(contentContainer.transform.position.x); }

    private void InitTimeShaft()
    {
        timeShaft = root.Q<IMGUIContainer>("TimeShaft");

        ScrollView MainContentView = root.Q<ScrollView>("MainContentView");
        contentContainer = MainContentView.Q<VisualElement>("unity-content-container");

        timeShaft.onGUIHandler = DrawTimeShaft;
        timeShaft.RegisterCallback<WheelEvent>(TimeShaftWheel);
    }

    private void DrawTimeShaft()
    {
        Handles.BeginGUI();
        Handles.color = Color.white;
        Rect rect = timeShaft.contentRect; //ʱ����ĳߴ�

        //��ʼ����
        int index = Mathf.CeilToInt(contentOffsetPos / skillEditorConfig.frameUnitWidth);
        //�����������ƫ��
        float startOffset = 0;
        //10-(98 % 10)
        //=10-8=2
        if (index > 0) startOffset = skillEditorConfig.frameUnitWidth - (contentOffsetPos % skillEditorConfig.frameUnitWidth);

        int tickStep = SkillEditorConfig.MaxFrameWidthLV + 1 - (skillEditorConfig.frameUnitWidth / SkillEditorConfig.StandframeUnitWidth);
        //tickStep = 10+1-(100/10)=1
        //tickStep = 11-9=2
        //tickStep = 11-8=3
        //tickStep = 11-1=10

        tickStep = Mathf.Clamp(tickStep / 2, 1, SkillEditorConfig.MaxFrameWidthLV);

        for (float i = startOffset; i < rect.width; i += skillEditorConfig.frameUnitWidth)
        {
            //���Ƴ��������ı�
            if (index % tickStep == 0)
            {
                Handles.DrawLine(new Vector3(i, rect.height - 10), new Vector3(i, rect.height));
                string indexStr = index.ToString();
                GUI.Label(new Rect(i - indexStr.Length * 4.5f, 0, 35, 20), indexStr);
            }
            else
            {
                Handles.DrawLine(new Vector3(i, rect.height - 5), new Vector3(i, rect.height));
            }

            index += 1;
        }
        Handles.EndGUI();
    }

    private void TimeShaftWheel(WheelEvent evt)
    {
        int delta = (int)evt.delta.y;
        skillEditorConfig.frameUnitWidth = Mathf.Clamp(skillEditorConfig.frameUnitWidth - delta,
            SkillEditorConfig.StandframeUnitWidth, SkillEditorConfig.MaxFrameWidthLV * SkillEditorConfig.StandframeUnitWidth);

        timeShaft.MarkDirtyLayout();//���Ϊ��Ҫ�������»��Ƶ�
        //Debug.Log(delta);
    }

    #endregion


    #region
    private SkillConfig skillConfig;
    private SkillEditorConfig skillEditorConfig = new SkillEditorConfig();
    #endregion



    #region

    #endregion

}

public class SkillEditorConfig
{
    /// <summary>
    /// ÿ֡�ı�׼��λ���ؿ̶�
    /// </summary>
    public const int StandframeUnitWidth = 10;

    /// <summary>
    /// ��10��
    /// </summary>
    public const int MaxFrameWidthLV = 10;

    /// <summary>
    /// ��ǰ��֡��λ�̶ȣ������Ŷ��仯��
    /// </summary>
    public int frameUnitWidth = 10;



}