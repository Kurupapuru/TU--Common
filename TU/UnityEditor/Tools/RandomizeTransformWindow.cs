#if UNITY_EDITOR
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace TU.UnityEditor.Tools
{
    public class RandomizeTransformWindow : OdinEditorWindow
    {
        [MenuItem("Tools/RandomizeTransform")]
        public static void GetWindow()
        {
            var window = GetWindow<RandomizeTransformWindow>();
        }

        public bool position = true;
        public Vector3 positionRandomize;

        private bool transformsNotEmpty => transforms != null && transforms.Length > 0 && transforms[0] != null;
        private Transform[] transforms;
        private Vector3[] originalPositions;
        
        [Button, ShowIf("transformsNotEmpty")]
        public void Redo()
        {
            Undo.RecordObjects(Selection.transforms, "Transform Randomize");
            if (position)
            {
                for (var i = 0; i < transforms.Length; i++)
                {
                    transforms[i].position = originalPositions[i] + new Vector3(
                        Random.Range(-positionRandomize.x, positionRandomize.x),
                        Random.Range(-positionRandomize.y, positionRandomize.y),
                        Random.Range(-positionRandomize.z, positionRandomize.z)
                    );
                }
            }
        }
        
        [Button]
        public void Randomize()
        {
            transforms = Selection.transforms;
            originalPositions = transforms.Select(x => x.position).ToArray();
            Redo();
        }

        [Button(name: "Close")]
        public void CloseButton()
        {
            this.Close();
        }
    }
}
#endif