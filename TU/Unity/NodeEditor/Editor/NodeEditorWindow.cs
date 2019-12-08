using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace TU.NodeEditor
{
    public class NodeEditorWindow : EditorWindow
    {
        private UQueryBuilder<VisualElement> nodes;
        
        [MenuItem("NodeEditor/Open")]
        public static void ShowWindow()
        {
            var window = GetWindow<NodeEditorWindow>();
            
            window.titleContent = new GUIContent("NodeEditor");
            
            window.minSize = new Vector2(800, 600);
        }

        private void OnEnable()
        {
            var root = rootVisualElement;

            root.styleSheets.Add(Resources.Load<StyleSheet>("NodeEditor_Main_Style"));
            root.styleSheets.Add(Resources.Load<StyleSheet>("NodeEditor_Node_Style"));

            var visualTree = Resources.Load<VisualTreeAsset>("NodeEditor_Main");
            visualTree.CloneTree(root);

            
            
            nodes = root.Query(className: "Node");
        }

        private void OnGUI()
        {
            Event cur = Event.current;
            
        }

        private void OnInspectorUpdate()
        {
            
        }
    }
}
