using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView m_graphView;

    [MenuItem("Graph/对话编辑")]
    public static void Open()
    {
        DialogueGraph window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("Dialogue Graph");
        window.Show();
    }

    private void OnEnable()
    {
        ConstructGraphViw();
        GennerateToolBar();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(m_graphView);
    }

    private void ConstructGraphViw()
    {
        m_graphView = new DialogueGraphView
        {
            name = "Dialogue Grahp",
        };
        m_graphView.StretchToParentSize();
        rootVisualElement.Add(m_graphView);
    }

    private void GennerateToolBar()
    {
        var toolbar = new Toolbar();

        var nodeCreateButton = new Button(() =>
        {
            m_graphView.CreateNode("Dialogue Node");
        });

        nodeCreateButton.text = "Create Node";

        toolbar.Add(nodeCreateButton);

        rootVisualElement.Add(toolbar);
    }

}
