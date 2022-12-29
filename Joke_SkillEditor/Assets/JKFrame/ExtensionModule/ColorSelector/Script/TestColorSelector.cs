using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JKFrame;
using System;

public class TestColorSelector : MonoBehaviour
{
    public GameObject Cube;
    void Start()
    {
        UI_ColorSelectorWindow colorSelectorWindow = UIManager.Instance.Show<UI_ColorSelectorWindow>();
        Cube.GetComponent<MeshRenderer>().material.color = colorSelectorWindow.GetColor();// ������ȡ
        colorSelectorWindow.Init(OnColorSelected,Color.white);    // �¼���ȡ

    }

    private void OnColorSelected(Color obj)
    {
        Cube.GetComponent<MeshRenderer>().material.color = obj;
    }

}
